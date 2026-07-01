
Imports NationalInstruments.RFToolkits.Wlan.Generation
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsg.Internal
Imports System.Runtime.InteropServices


Class Program
   Friend Shared Sub Main(args As String())
      Dim example As New GenerateeWaveformMultiUserExample()
      example.Run()
   End Sub
End Class

Class GenerateeWaveformMultiUserExample

   Const maxWlanChannels As Integer = 4
   Private numTx As Integer = 2
   Private numberOfUsers As Integer = 2

   Private carrierFrequency As Double = 5180000000.0
   Private channelBandwidth As Double = 20000000.0
   Private powerLevel As Double() = {-10.0, -10.0, -10.0, -10.0}
   Private externalAttenuation As Double() = {0, 0, 0, 0}

   Private rfsgSessions As NIRfsg() = New NIRfsg(maxWlanChannels - 1) {}
   Private wlanSession As niWLANG
   Private externalLOSession As NIRfsg = Nothing
   Private rfsgResourceName As [String]() = {"RIO0", "RIO1", "RIO2", "RIO3"}
   Private rfsgClockSource As [String] = RfsgFrequencyReferenceSource.PxiClock

   Private waveformName As [String] = "Wlan"
   Private mCSIndex As Integer() = New Integer() {0, 8}
   Private numOfSpaceTimeStreams As Integer() = {1, 1}
   Private triggerLines As Integer() = {0, 1}

   Public Sub Run()
      Try
         ConfigureWlanGenerationSession()
         ConfigureRfsgSession()
         CreateAndDownloadWaveform()
         StopGeneration()
      Catch e As Exception
         Console.WriteLine("Error : " & e.ToString())
         Console.WriteLine(" Press any key to exit")
         Console.ReadKey()
      Finally
         For i As Integer = 0 To numTx - 1
            If rfsgSessions(i) IsNot Nothing Then
               rfsgSessions(i).Close()
            End If
         Next

         If wlanSession IsNot Nothing Then
            wlanSession.Close()
         End If
      End Try
   End Sub

   Private Sub ConfigureWlanGenerationSession()
      Dim isNewSession As Integer
      Dim mappingMatrixType As Integer = niWLANGConstants.MappingMatrixTypeDirect

      If wlanSession Is Nothing Then
         wlanSession = New niWLANG("WLANG", niWLANGConstants.CompatibilityVersion060000, isNewSession)
      End If

      wlanSession.SetStandard(Nothing, niWLANGConstants.Standard80211AcMimoOfdm)
      wlanSession.SetNumberOfTransmitChannels(Nothing, numTx)
      wlanSession.SetChannelBandwidth(Nothing, channelBandwidth)
      wlanSession.SetMappingMatrixType(Nothing, mappingMatrixType)
      wlanSession.SetPPDUType(Nothing, niWLANGConstants.PpduTypeMuPpdu)
      wlanSession.SetNumberOfUsers(Nothing, numberOfUsers)

      Dim activeChannel As [String] = "user"

      For i As Integer = 0 To numTx - 1
         wlanSession.SetScalarAttributeI32(activeChannel & i, niWLANGProperties.McsIndex, mCSIndex(i))
         wlanSession.SetNumberOfSpaceTimeStreams(activeChannel & i, numOfSpaceTimeStreams(i))
      Next

      wlanSession.SetRFBlankingEnabled(Nothing, niWLANGConstants.[True])
   End Sub


   Private Sub CreateAndDownloadWaveform()
      Dim iqRate As Double
      Dim actualHeadRoom As Double() = New Double(numTx - 1) {}
      Dim script As [String] = "script GenerateWlan" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "                repeat forever" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & vbTab & "                generate Wlan" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "                end repeat" & vbCr & vbLf & vbTab & vbTab & vbTab & "                end script"

      Dim rfsgHandle As IntPtr() = New IntPtr(numTx - 1) {}
      Dim externalLOHandle As New IntPtr()

      For i As Integer = 0 To numTx - 1
         rfsgHandle(i) = rfsgSessions(i).GetInstrumentHandle().DangerousGetHandle()
      Next

      If externalLOSession IsNot Nothing Then
         externalLOHandle = externalLOSession.GetInstrumentHandle().DangerousGetHandle()
      End If
      wlanSession.ConfigureMultipleDeviceSynchronization(rfsgHandle, numTx, rfsgClockSource, triggerLines, triggerLines.Length)
      wlanSession.RFSGConfigureFrequencySingleLO(rfsgHandle, niWLANGConstants.LOSourceOnboard, externalLOHandle, carrierFrequency, niWLANGConstants.[False], niWLANGConstants.[False])
      wlanSession.RFSGCreateAndDownloadMIMOWaveforms(rfsgHandle, Nothing, numTx, waveformName)


      For i As Integer = 0 To numTx - 1
         niWLANG.WLANG_RFSGConfigureScript(rfsgSessions(i).GetInstrumentHandle().DangerousGetHandle(), Nothing, script, powerLevel(i))
      Next
      wlanSession.RFSGMultipleDeviceInitiate(rfsgHandle)
      'Check for successful generation
      CheckGeneration()

      wlanSession.GetIqRate([String].Empty, iqRate)
      Console.WriteLine("IQ Rate : {0}", iqRate)

      Dim channelString As String = "segment0/channel"
      For i As Integer = 0 To numTx - 1
         wlanSession.GetActualHeadroom(channelString & i, actualHeadRoom(i))
         Console.WriteLine("Actual HeadRoom User: {0}", actualHeadRoom(i))
      Next
      Console.WriteLine("Press any key to exit")
      Console.ReadKey()

   End Sub

   Private Sub ConfigureRfsgSession()
      For i As Integer = 0 To numTx - 1
         If rfsgSessions(i) Is Nothing Then
            rfsgSessions(i) = New NIRfsg(rfsgResourceName(i), True, False)
         End If

         rfsgSessions(i).RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
         rfsgSessions(i).Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform
         rfsgSessions(i).RF.ExternalGain = -(externalAttenuation(i))
      Next
   End Sub

   Private Function CheckGeneration() As Boolean
      Dim status As RfsgGenerationStatus = RfsgGenerationStatus.InProgress
      For i As Integer = 0 To numTx - 1
         status = rfsgSessions(i).CheckGenerationStatus()
         If status = RfsgGenerationStatus.Complete Then
            Exit For
         End If
      Next
      Return Convert.ToBoolean(status)
   End Function

   Private Sub StopGeneration()
      For i As Integer = 0 To numTx - 1
         If rfsgSessions(i) IsNot Nothing Then
            rfsgSessions(i).Abort()
            rfsgSessions(i).RF.OutputEnabled = False
            rfsgSessions(i).Utility.Commit()
            niWLANG.WLANG_RFSGClearDatabase(rfsgSessions(i).GetInstrumentHandle().DangerousGetHandle(), "", Nothing)
         End If
      Next
   End Sub
End Class
