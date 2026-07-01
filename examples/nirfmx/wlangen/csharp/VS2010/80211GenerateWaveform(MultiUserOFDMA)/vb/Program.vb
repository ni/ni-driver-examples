Imports NationalInstruments.RFToolkits.Wlan.Generation
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsg.Internal
Imports System.Runtime.InteropServices


Namespace _80211GenerateWaveformMultiUserOFDMA
   Class Program
      Friend Shared Sub Main(args As String())
         Dim example As New GenerateWaveformMultipleRFSGExample()
         example.Run()
      End Sub
   End Class

   Class GenerateWaveformMultipleRFSGExample

      Const MAX_WLAN_CHANNELS As Integer = 4
      Private numTx As Integer, mappingMatrixType As Integer, PPDUType As Integer, numberOfUsers As Integer, guardIntervalType As Integer

      Private carrierFrequency As Double
      Private channelBandwidth As Double
      Private powerLevel As Double() = New Double(MAX_WLAN_CHANNELS - 1) {-10.0, -10.0, -10.0, -10.0}
      Private externalAttenuation As Double() = New Double(MAX_WLAN_CHANNELS - 1) {0, 0, 0, 0}

      Private RuSize As Integer() = {niWLANGConstants.RuSize26, niWLANGConstants.RuSize26, niWLANGConstants.RuSize52, niWLANGConstants.RuSize26, niWLANGConstants.RuSize106}
      Private RuOffsetMruIndex As Integer() = {0, 1, 2, 4, 5}
      Private McsIndex As Integer() = {0, 0, 0, 0, 0}
      Private numberOfSpaceTimeStream As Integer() = {1, 1, 1, 1, 1}
      Private payloadLength As Integer() = {100, 100, 100, 100, 100}

      Private rfsgSessions As NIRfsg() = New NIRfsg(MAX_WLAN_CHANNELS - 1) {}
      Private wlanSession As niWLANG
      Private externalLOSession As NIRfsg = Nothing
      Private rfsgResourceName As [String]() = New String(MAX_WLAN_CHANNELS - 1) {"RIO0", "RIO1", "RIO2", "RIO3"}
      Private rfsgClockSource As [String] = RfsgFrequencyReferenceSource.PxiClock

      Private waveformName As [String] = "Wlan"
      Private channelString As [String]
      Private triggerLines As Integer() = {0, 1}

      Public Sub Run()
         Try
            initGlobalVaribales()
            ConfigureWlanGenerationSession()
            ConfigureRfsgSession()
            CreateAndDownloadWaveform()
            StopGeneration()
         Catch e As Exception
            Console.WriteLine("Error : " & e.ToString())
            Console.WriteLine("press any key to exit")
            Console.ReadKey()
         Finally
            If wlanSession IsNot Nothing Then
               wlanSession.Close()
            End If
         End Try
      End Sub

      Private Sub initGlobalVaribales()
         channelBandwidth = 20000000.0
         numTx = 1
         mappingMatrixType = niWLANGConstants.MappingMatrixTypeDirect
         PPDUType = niWLANGConstants.PpduTypeMuPpdu
         numberOfUsers = 5
         guardIntervalType = niWLANGConstants.GuardIntervalTypeOneByFour

         carrierFrequency = 5180000000.0
      End Sub

      Private Sub ConfigureWlanGenerationSession()
         Dim isNewSession As Integer

         If wlanSession Is Nothing Then
            wlanSession = New niWLANG("WLANG", niWLANGConstants.CompatibilityVersion060000, isNewSession)
         End If

         wlanSession.SetStandard(Nothing, niWLANGConstants.Standard80211AxMimoOfdm)
         wlanSession.SetChannelBandwidth(Nothing, channelBandwidth)
         wlanSession.SetNumberOfTransmitChannels(Nothing, numTx)
         wlanSession.SetMappingMatrixType(Nothing, mappingMatrixType)
         wlanSession.SetPPDUType(Nothing, PPDUType)
         wlanSession.SetNumberOfUsers(Nothing, numberOfUsers)
         wlanSession.SetOFDMGuardIntervalType(Nothing, guardIntervalType)

         For i As Integer = 0 To numberOfUsers - 1
            channelString = "user" & i
            wlanSession.SetRUSize(channelString, RuSize(i))
            wlanSession.SetRUOffsetMruIndex(channelString, RuOffsetMruIndex(i))
            wlanSession.SetScalarAttributeI32(channelString, niWLANGProperties.McsIndex, McsIndex(i))
            wlanSession.SetNumberOfSpaceTimeStreams(channelString, numberOfSpaceTimeStream(i))
            channelString = channelString & "/mpdu0"
            wlanSession.SetPayloadDataLength(channelString, payloadLength(i))
         Next

         wlanSession.SetRFBlankingEnabled(Nothing, niWLANGConstants.[True])
      End Sub

      Private Sub CreateAndDownloadWaveform()
         Dim iqRate As Double, waveformDuration As Double
         Dim iqWaveformSize As Integer
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
         'Configure Script

         For i As Integer = 0 To numTx - 1
            niWLANG.WLANG_RFSGConfigureScript(rfsgHandle(i), Nothing, script, powerLevel(i))
         Next
         wlanSession.RFSGMultipleDeviceInitiate(rfsgHandle)
         'Check for successful generation
         CheckGeneration()
         wlanSession.GetIqRate([String].Empty, iqRate)
         wlanSession.GetIqWaveformSize([String].Empty, iqWaveformSize)
         waveformDuration = iqWaveformSize / iqRate

         Console.WriteLine("Waveform Duration : {s}" & waveformDuration)
         Console.WriteLine("Actual HeadRoom" & vbLf)
         For i As Integer = 0 To numTx - 1
            channelString = "Channel" & i
            wlanSession.GetActualHeadroom(channelString, actualHeadRoom(i))
            Console.WriteLine(vbTab & actualHeadRoom(i))
         Next
         Console.WriteLine("Press any key to exit")
         Console.ReadKey()

      End Sub

      Private Sub ConfigureRfsgSession()
         'Open all RFSG Sessions
         For i As Integer = 0 To numTx - 1
            If rfsgSessions(i) Is Nothing Then
               rfsgSessions(i) = New NIRfsg(rfsgResourceName(i), False, True)
            End If
            rfsgSessions(i).RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
            rfsgSessions(i).Arb.GenerationMode = RfsgWaveformGenerationMode.Script
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
               niWLANG.WLANG_RFSGClearDatabase(rfsgSessions(i).GetInstrumentHandle().DangerousGetHandle(), "", waveformName)
               rfsgSessions(i).Close()
            End If
         Next
      End Sub
   End Class
End Namespace
