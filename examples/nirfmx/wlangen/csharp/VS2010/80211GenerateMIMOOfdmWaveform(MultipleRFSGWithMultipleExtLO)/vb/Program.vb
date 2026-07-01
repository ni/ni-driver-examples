Imports NationalInstruments.RFToolkits.Wlan.Generation
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsg.Internal
Imports System.Runtime.InteropServices


Namespace _80211GenerateMIMOOfdmWaveformMultipleRFSGWithExtLO
   Class Program
      Friend Shared Sub Main(args As String())
         Dim example As New GenerateWaveformMultipleRFSGExample()
         example.Run()
      End Sub
   End Class

   Class GenerateWaveformMultipleRFSGExample

      Const MAX_WLAN_CHANNELS As Integer = 4
      Const NUMBER_OF_EXTERNAL_LO As Integer = 1
      Const NUMBER_OF_SEGMENTS As Integer = 1

      Private standard As Integer, numTx As Integer

      Private segment0CarrierFrequency As Double, segment1CarrierFrequency As Double, channelBandwidth As Double
      Private rfsgResourceName As [String]() = New String(MAX_WLAN_CHANNELS - 1) {"RIO0", "RIO1", "RIO2", "RIO3"}
      Private powerLevel As Double() = New Double(MAX_WLAN_CHANNELS - 1) {-10.0, -10.0, -10.0, -10.0}
      Private externalAttenuation As Double() = New Double(MAX_WLAN_CHANNELS - 1) {0, 0, 0, 0}
      Private carrierFrequencies As Double() = New Double(NUMBER_OF_SEGMENTS - 1) {}

      Private rfsgSessions As NIRfsg() = New NIRfsg(MAX_WLAN_CHANNELS - 1) {}
      Private wlanSession As niWLANG
      Private rfsgClockSource As [String] = RfsgFrequencyReferenceSource.PxiClock
      Private LOSource As Integer, rfsaLODaisyChainEnabled As Integer, LOExportToExternalDevicesEnabled As Integer

      Private externalLOResourceName As String() = New String(NUMBER_OF_EXTERNAL_LO - 1) {Nothing}
      Private externalLOReferenceClockSource As String() = New String(NUMBER_OF_EXTERNAL_LO - 1) {RfsgFrequencyReferenceSource.OnboardClock}
      Private externalLOSession As NIRfsg()

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
                  rfsgSessions(i).Close()
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

         segment0CarrierFrequency = 5210000000.0
         segment1CarrierFrequency = 5530000000.0
         If NUMBER_OF_SEGMENTS = 1 Then
            carrierFrequencies = New Double(0) {}
            carrierFrequencies(0) = segment0CarrierFrequency
         Else
            carrierFrequencies = New Double(1) {}
            carrierFrequencies(0) = segment0CarrierFrequency
            carrierFrequencies(1) = segment1CarrierFrequency
         End If
      End Sub

      Private Sub ConfigureWlanGenerationSession()
         Dim isNewSession As Integer
         If wlanSession Is Nothing Then
            wlanSession = New niWLANG("WLANG", niWLANGConstants.CompatibilityVersion060000, isNewSession)
         End If
         wlanSession.SetStandard(Nothing, standard)
         wlanSession.SetChannelBandwidth(Nothing, channelBandwidth)
         wlanSession.SetNumberOfTransmitChannels(Nothing, numTx)
         wlanSession.SetNumberOfSegments(Nothing, NUMBER_OF_SEGMENTS)
         wlanSession.SetScalarAttributeI32(Nothing, niWLANGProperties.McsIndex, mCSIndex)
         wlanSession.SetMappingMatrixType(Nothing, mappingMatrixType)
         wlanSession.Set_80211nPlcpFrameFormat(Nothing, frameFormat)
         wlanSession.SetNumberOfSpaceTimeStreams(Nothing, numOfSpaceTimeStreams)
         wlanSession.Set_80211ahPreambleType(Nothing, ahPreambleType)
         wlanSession.SetLOFrequencyOffsetMode(Nothing, niWLANGConstants.LOFrequencyOffsetModeAuto)

         wlanSession.SetRFBlankingEnabled(Nothing, niWLANGConstants.[True])
      End Sub

      Private Sub configureLOSession()
         externalLOSession = New NIRfsg(NUMBER_OF_EXTERNAL_LO - 1) {}
         If externalLOResourceName(0) IsNot Nothing AndAlso LOSource = niWLANGConstants.LOSourceExternal AndAlso segment0CarrierFrequency > 3200000000.0 Then
            For i As Integer = 0 To NUMBER_OF_EXTERNAL_LO - 1
               externalLOSession(i) = New NIRfsg(externalLOResourceName(i), True, False)
               externalLOSession(i).Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave
               externalLOSession(i).FrequencyReference.Configure(externalLOReferenceClockSource(i), 10000000.0)
            Next
         End If
      End Sub

      Private Sub CreateAndDownloadWaveform()
         Dim iqRate As Double, waveformDuration As Double
         Dim iqWaveformSize As Integer, k As Integer = 0
         Dim rfsgArraySize As Integer = numTx * NUMBER_OF_SEGMENTS
         Dim channelString As String() = New String(29) {}
         Dim actualHeadRoom As Double() = New Double(rfsgArraySize - 1) {}
         Dim script As [String] = "script GenerateWlan" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "                repeat forever" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & vbTab & "                generate Wlan" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "                end repeat" & vbCr & vbLf & vbTab & vbTab & vbTab & "                end script"

         Dim rfsgHandle As IntPtr() = New IntPtr(rfsgArraySize - 1) {}
         Dim externalLOref As IntPtr() = New IntPtr(NUMBER_OF_EXTERNAL_LO - 1) {}

         For i As Integer = 0 To NUMBER_OF_EXTERNAL_LO - 1
            If externalLOSession(i) IsNot Nothing Then
               externalLOref(i) = externalLOSession(i).GetInstrumentHandle().DangerousGetHandle()
            End If
         Next
         For i As Integer = 0 To rfsgArraySize - 1
            rfsgHandle(i) = rfsgSessions(i).GetInstrumentHandle().DangerousGetHandle()
         Next

         wlanSession.ConfigureMultipleDeviceSynchronization(rfsgHandle, rfsgArraySize, rfsgClockSource, triggerLines, triggerLines.Length)
         wlanSession.RFSGConfigureFrequencyMultipleLO(rfsgHandle, LOSource, externalLOref, carrierFrequencies, carrierFrequencies.Length, rfsaLODaisyChainEnabled,
            LOExportToExternalDevicesEnabled)

         For i As Integer = 0 To NUMBER_OF_EXTERNAL_LO - 1
            If externalLOSession(i) IsNot Nothing Then
               externalLOSession(i).Initiate()
            End If
         Next
         wlanSession.RFSGCreateAndDownloadMIMOWaveforms(rfsgHandle, Nothing, rfsgArraySize, waveformName)
         'Configure Script

         For i As Integer = 0 To numTx * NUMBER_OF_SEGMENTS - 1
            niWLANG.WLANG_RFSGConfigureScript(rfsgHandle(i), Nothing, script, powerLevel(i))
         Next

         wlanSession.RFSGMultipleDeviceInitiate(rfsgHandle)
         'Check for successful generation
         CheckGeneration()

         If standard = niWLANGConstants.Standard80211nMimoOfdm OrElse standard = niWLANGConstants.Standard80211AhMimoOfdm OrElse standard = niWLANGConstants.Standard80211beMimoOfdm OrElse standard = niWLANGConstants.Standard80211bnMimoOfdm Then

            For i As Integer = 0 To numTx - 1
               channelString(i) = "Channel" & i
            Next
         Else

            For i As Integer = 0 To NUMBER_OF_SEGMENTS - 1

               For j As Integer = 0 To numTx - 1
                  channelString(Math.Min(System.Threading.Interlocked.Increment(k), k - 1)) = "Segment" & i & "/Channel" & j
               Next
            Next
         End If
         wlanSession.GetIqRate([String].Empty, iqRate)
         wlanSession.GetIqWaveformSize([String].Empty, iqWaveformSize)
         waveformDuration = iqWaveformSize / iqRate

         Console.WriteLine("Waveform Duration {0}" & waveformDuration)
         Console.WriteLine("Actual Headroom (dB)")
         For i As Integer = 0 To numTx * NUMBER_OF_SEGMENTS - 1
            wlanSession.GetActualHeadroom(channelString(i), actualHeadRoom(i))
            Console.WriteLine(vbTab & actualHeadRoom(i))
         Next
         Console.WriteLine("Press any key to exit")
         Console.ReadKey()
      End Sub

      Private Sub ConfigureRfsgSession()
         'Open all RFSG Sessions
         For i As Integer = 0 To numTx * NUMBER_OF_SEGMENTS - 1
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
         For i As Integer = 0 To NUMBER_OF_EXTERNAL_LO - 1
            If externalLOSession(i) IsNot Nothing Then
               externalLOSession(i).Abort()
               externalLOSession(i).RF.OutputEnabled = False
               externalLOSession(i).Close()
            End If
         Next
      End Sub

   End Class
End Namespace
