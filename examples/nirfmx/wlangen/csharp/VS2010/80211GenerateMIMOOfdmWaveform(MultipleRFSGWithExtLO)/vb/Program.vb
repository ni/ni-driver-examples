
Imports NationalInstruments.RFToolkits.Wlan.Generation
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsg.Internal
Imports System.Runtime.InteropServices


Class Program
   Friend Shared Sub Main(args As String())
      Dim example As New GenerateWaveformMultipleRFSGExample()
      example.Run()
   End Sub
End Class

Class GenerateWaveformMultipleRFSGExample

   Const MAX_WLAN_CHANNELS As Integer = 4
   Private standard As Integer, numTx As Integer

   Private carrierFrequency As Double
   Private channelBandwidth As Double
   Private rfsgResourceName As [String]() = New String(MAX_WLAN_CHANNELS - 1) {"RIO0", "RIO1", "RIO2", "RIO3"}
   Private powerLevel As Double() = New Double(MAX_WLAN_CHANNELS - 1) {-10.0, -10.0, -10.0, -10.0}
   Private externalAttenuation As Double() = New Double(MAX_WLAN_CHANNELS - 1) {0, 0, 0, 0}

   Private rfsgSessions As NIRfsg() = New NIRfsg(MAX_WLAN_CHANNELS - 1) {}
   Private wlanSession As niWLANG
   Private rfsgClockSource As [String] = RfsgFrequencyReferenceSource.PxiClock
   Private LOSource As Integer, rfsaLODaisyChainEnabled As Integer, LOExportToExternalDevicesEnabled As Integer

   Private externalLOResourceName As String = Nothing
   Private externalLOReferenceClockSource As String = RfsgFrequencyReferenceSource.OnboardClock
   Private externalLOSession As NIRfsg = Nothing

   Private waveformName As [String] = "Wlan"
   Private mCSIndex As Integer, frameFormat As Integer, ahPreambleType As Integer
   Private numOfSpaceTimeStreams As Integer, mappingMatrixType As Integer
   Private triggerLines As Integer() = {0, 1}

   Public Sub Run()
      Try
         initGlobalVaribales()
         ConfigureWlanGenerationSession()
         ConfigureRfsgSession()
         configureLOSession()
         CreateAndDownloadWaveform()
         StopGeneration()
         CloseexternalLOSessions()
      Catch e As Exception
         Console.WriteLine("Error : " & e.ToString())
         Console.ReadKey()
      Finally
         For i As Integer = 0 To numTx - 1
            If rfsgSessions(i) IsNot Nothing Then
               rfsgSessions(i).close()
            End If
         Next

         If wlanSession IsNot Nothing Then
            wlanSession.Close()
         End If
      End Try
   End Sub

   Private Sub initGlobalVaribales()
      standard = niWLANGConstants.Standard80211AxMimoOfdm
      channelBandwidth = 80000000.0
      numTx = 1
      mCSIndex = 0
      mappingMatrixType = niWLANGConstants.MappingMatrixTypeDirect
      numOfSpaceTimeStreams = 1
      frameFormat = niWLANGConstants._80211nPlcpFrameFormatMixed
      ahPreambleType = niWLANGConstants.PreambleTypeShortPreamble
      LOSource = niWLANGConstants.LOSourceExternal
      rfsaLODaisyChainEnabled = niWLANGConstants.[False]
      LOExportToExternalDevicesEnabled = niWLANGConstants.[False]
      carrierFrequency = 5180000000.0
   End Sub

   Private Sub ConfigureWlanGenerationSession()
      Dim isNewSession As Integer
      If wlanSession Is Nothing Then
         wlanSession = New niWLANG("WLANG", niWLANGConstants.CompatibilityVersion060000, isNewSession)
      End If
      wlanSession.SetStandard(Nothing, standard)
      wlanSession.SetChannelBandwidth(Nothing, channelBandwidth)
      wlanSession.SetNumberOfTransmitChannels(Nothing, numTx)
      wlanSession.SetScalarAttributeI32(Nothing, niWLANGProperties.McsIndex, mCSIndex)
      wlanSession.SetMappingMatrixType(Nothing, mappingMatrixType)
      wlanSession.Set_80211nPlcpFrameFormat(Nothing, frameFormat)
      wlanSession.SetNumberOfSpaceTimeStreams(Nothing, numOfSpaceTimeStreams)
      wlanSession.Set_80211ahPreambleType(Nothing, ahPreambleType)
      wlanSession.SetLOFrequencyOffsetMode(Nothing, niWLANGConstants.LOFrequencyOffsetModeAuto)

      wlanSession.SetRFBlankingEnabled(Nothing, niWLANGConstants.[True])
   End Sub

   Private Sub configureLOSession()
      If externalLOResourceName IsNot Nothing AndAlso LOSource = niWLANGConstants.LOSourceExternal AndAlso carrierFrequency > 3200000000.0 Then
         externalLOSession = New NIRfsg(externalLOResourceName, True, False)
         externalLOSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave
         externalLOSession.FrequencyReference.Configure(externalLOReferenceClockSource, 10000000.0)
      End If
   End Sub

   Private Sub CreateAndDownloadWaveform()
      Dim iqRate As Double, waveformDuration As Double
      Dim iqWaveformSize As Integer
      Dim channelString As String
      Dim actualHeadRoom As Double() = New Double(numTx - 1) {}
      Dim script As [String] = "script GenerateWlan" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "                repeat forever" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & vbTab & "                generate Wlan" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "                end repeat" & vbCr & vbLf & vbTab & vbTab & vbTab & "                end script"

      Dim rfsgHandle As IntPtr() = New IntPtr(numTx - 1) {}
      Dim externalLOref As New IntPtr()
      If externalLOSession IsNot Nothing Then
         externalLOref = externalLOSession.GetInstrumentHandle().DangerousGetHandle()
      End If

      For i As Integer = 0 To numTx - 1
         rfsgHandle(i) = rfsgSessions(i).GetInstrumentHandle().DangerousGetHandle()
      Next

      wlanSession.ConfigureMultipleDeviceSynchronization(rfsgHandle, numTx, rfsgClockSource, triggerLines, triggerLines.Length)
      wlanSession.RFSGConfigureFrequencySingleLO(rfsgHandle, LOSource, externalLOref, carrierFrequency, rfsaLODaisyChainEnabled, LOExportToExternalDevicesEnabled)

      If externalLOSession IsNot Nothing Then
         externalLOSession.Initiate()
      End If

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

      Console.WriteLine("Waveform Duration {s}" & waveformDuration)
      Console.WriteLine("Actual Headroom (dB)")
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
            rfsgSessions(i) = New NIRfsg(rfsgResourceName(i), True, False)
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
         End If
      Next
   End Sub

   Private Sub CloseexternalLOSessions()
      If externalLOSession IsNot Nothing Then
         externalLOSession.Abort()
         externalLOSession.RF.OutputEnabled = False
         externalLOSession.close()
      End If
   End Sub

End Class
