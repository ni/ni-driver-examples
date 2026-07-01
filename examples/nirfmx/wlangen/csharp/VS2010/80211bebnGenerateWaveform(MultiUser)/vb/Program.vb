' Comments:
' * Steps
'1.
'     A. Compute number of generators as the number of segments times the number of transmit channels (N_tx). 
'     B. Create an array of carrier frequencies with segment 0 carrier frequency, segment 1 carrier frequency if the    Number of Segments is equal to 2. otherwise, use the segment 0 carrier frequency.
'2. For each generator,
'     A. Open an NI-RFSG session.
'     B. Configure basic NI-RFSG properties. Set Generation Mode to Script and Power Level Type to Peak Power. 
'3. Open an NI WLAN Generation session.
'4. Configure basic WLAN generation properties. 
'   - Set Standard to 80211BE MIMOOFDM or 80211BN MIMOOFDM.
'5. Set headroom for each channel in each segment.
'6. Configures advanced WLANG properties and user specific properties.
'7. Configure payload and spectrum control properties.
'8. Enable RF blanking to attenuate the RF OUT signal during the idle interval. 
'9. Synchronize the generators. Configure the reference clock source for the master (the first generator) and share that with slaves. 
'10. Configure frequency on the generators.
'11. Create the waveform and download it to the NI RF vector signal generators memory.
'12. For each generator, configure NI-RFSG for the waveform mentioned in the script.
'13.  Initiate signal generation by calling niWLANG RFSG Multiple Device Initiate VI.
'14. 
'    A. Check the generation status.
'    B. Exit if an error has occurred, the Stop button is pressed or generation is complete.
'15. For each generator,
'    A. Abort signal generation.
'    B. Disable the output. This sets the noise floor as low as possible.
'    C. Clear the waveform from the device memory. Clear the waveform properties from RFSG database.
'    D. Close the NI-RFSG session.
'16. Read signal properties (waveform size, actual headroom and packet extension duration) for display purpose.
'17. Close the NI WLAN Generation session.


Imports NationalInstruments.RFToolkits.Wlan.Generation
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsg.Internal
Imports System.Runtime.InteropServices


Namespace _80211bebnGenerateWaveformMultiUser
   Class Program
      Friend Shared Sub Main(args As String())
         Dim example As New GenerateWaveformMultipleRFSGExample()
         example.Run()
      End Sub
   End Class

   Class GenerateWaveformMultipleRFSGExample

      Const MAX_WLAN_CHANNELS As Integer = 8
      Private numTx As Integer, mappingMatrixType As Integer, PPDUType As Integer, numberOfUsers As Integer, guardIntervalType As Integer
      Private SwapIandQEnabled As Integer, SigCompression As Integer, LtfSize As Integer, OverSamplingFactor As Integer, AutoHeadroomEnabled As Integer
      Private NumberOfFrames As Integer, TransmissionMode As Integer, NominalPacketPadding As Integer, NumSeg As Integer
      Private PreamblePuncturingEnabled As Integer, Primary20MhzChannelIndex As Integer, PreamblePuncturingMask As Integer
      Private PulseShapingFilterEnabled As Integer, FilterType As Integer, FilterLength As Integer, OfdmWindowLength As Integer, WindowingMethod As Integer, RuAllocationMode As Integer

      Private carrierFrequency As Double() = {5180000000.0}
      Private channelBandwidth As Double
      Private FilterParameter As Double, SampleClockRateFactor As Double, IdleInterval As Double

      Private powerLevel As Double() = New Double(MAX_WLAN_CHANNELS - 1) {-10.0, -10.0, -10.0, -10.0, -10.0, -10.0,
          -10.0, -10.0}
      Private externalAttenuation As Double() = New Double(MAX_WLAN_CHANNELS - 1) {0, 0, 0, 0, 0, 0,
          0, 0}
      Private Headroom As Double() = New Double(MAX_WLAN_CHANNELS - 1) {12, 12, 12, 12, 12, 12,
          12, 12}


      Private RuSize As Integer() = {niWLANGConstants.RuSize26, niWLANGConstants.RuSize26, niWLANGConstants.RuSize52, niWLANGConstants.RuSize26, niWLANGConstants.RuSize106}
      Private RuOffset As Integer() = {0, 1, 2, 4, 5}
      Private McsIndex As Integer() = {0, 0, 0, 0, 0}
      Private numberOfSpaceTimeStream As Integer() = {1, 1, 1, 1, 1}
      Private payloadLength As Integer() = {100, 100, 100, 100, 100}
      Private RuAllocation As Integer() = {80}
      Private UserEnabled As Integer() = {niWLANGConstants.[True], niWLANGConstants.[True], niWLANGConstants.[True], niWLANGConstants.[True], niWLANGConstants.[True]}
      Private DcmEnabled As Integer() = {niWLANGConstants.[False], niWLANGConstants.[False], niWLANGConstants.[False], niWLANGConstants.[False], niWLANGConstants.[False]}
      Private StaId As Integer() = {0, 1, 2, 3, 4}
      Private PowerBoostFactor As Integer() = {1, 1, 1, 1, 1}
      Private FecCodingType As Integer() = {niWLANGConstants.FecCodingTypeLdpc, niWLANGConstants.FecCodingTypeLdpc, niWLANGConstants.FecCodingTypeLdpc, niWLANGConstants.FecCodingTypeLdpc, niWLANGConstants.FecCodingTypeLdpc}

      Private rfsgSessions As NIRfsg() = New NIRfsg(MAX_WLAN_CHANNELS - 1) {}
      Private wlanSession As niWLANG
      Private externalLOSession As NIRfsg() = New NIRfsg(MAX_WLAN_CHANNELS - 1) {Nothing, Nothing, Nothing, Nothing, Nothing, Nothing,
          Nothing, Nothing}
      Private rfsgResourceName As [String]() = New String(MAX_WLAN_CHANNELS - 1) {"RIO0", "RIO1", "RIO2", "RIO3", "RIO4", "RIO5",
      "RIO6", "RIO7"}
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
         NumSeg = 1
         SwapIandQEnabled = niWLANGConstants.[False]
         SampleClockRateFactor = 1
         SigCompression = niWLANGConstants.SigCompressionEnabled
         LtfSize = niWLANGConstants.LtfSizeAuto
         OverSamplingFactor = 4
         AutoHeadroomEnabled = niWLANGConstants.[True]
         NumberOfFrames = 1
         TransmissionMode = niWLANGConstants.TransmissionModeDownlinl
         NominalPacketPadding = niWLANGConstants.MaximumPaddingDurationAuto
         PreamblePuncturingEnabled = niWLANGConstants.[False]
         Primary20MhzChannelIndex = 0
         PreamblePuncturingMask = &HFFFF
         PulseShapingFilterEnabled = niWLANGConstants.[False]
         FilterType = niWLANGConstants.FilterRectangular
         FilterLength = 8
         OfdmWindowLength = 2
         WindowingMethod = niWLANGConstants.WinMethodCenteredAtSymbolBoundary
         RuAllocationMode = niWLANGConstants.RUAllocationModeIndividual
         FilterParameter = 0.5
         SampleClockRateFactor = 1
         IdleInterval = 0.0001
      End Sub

      Private Sub ConfigureWlanGenerationSession()
         Dim isNewSession As Integer

         If wlanSession Is Nothing Then
            wlanSession = New niWLANG("WLANG", niWLANGConstants.CompatibilityVersion060000, isNewSession)
         End If

         wlanSession.SetStandard(Nothing, niWLANGConstants.Standard80211beMimoOfdm)
         wlanSession.SetChannelBandwidth(Nothing, channelBandwidth)
         wlanSession.SetOversamplingFactor(Nothing, OverSamplingFactor)
         wlanSession.SetIdleInterval(Nothing, IdleInterval)
         wlanSession.SetNumberOfFrames(Nothing, NumberOfFrames)
         wlanSession.SetAutoHeadroomEnabled(Nothing, AutoHeadroomEnabled)
         wlanSession.SetLtfSize(Nothing, LtfSize)
         wlanSession.SetSigCompressionEnabled(Nothing, SigCompression)
         wlanSession.SetOFDMGuardIntervalType(Nothing, guardIntervalType)

         For i As Integer = 0 To (numTx * NumSeg) - 1
            channelString = "channel" & i
            wlanSession.SetHeadroom(channelString, Headroom(i))
         Next

         wlanSession.SetNumberOfTransmitChannels(Nothing, numTx)
         wlanSession.SetMappingMatrixType(Nothing, mappingMatrixType)
         wlanSession.SetNumberOfSegments(Nothing, NumSeg)
         wlanSession.SetNumberOfUsers(Nothing, numberOfUsers)
         wlanSession.SetTransmissionMode(Nothing, TransmissionMode)
         wlanSession.SetPreamblePuncturingEnabled(Nothing, PreamblePuncturingEnabled)
         wlanSession.SetPrimary20MHzChannelIndex(Nothing, Primary20MhzChannelIndex)
         wlanSession.SetPreamblePuncturingMask(Nothing, PreamblePuncturingMask)
         wlanSession.SetNominalPacketPadding(Nothing, NominalPacketPadding)

         If PPDUType = niWLANGConstants.PpduTypeSuPpdu Then
            wlanSession.SetPPDUType(Nothing, niWLANGConstants.PpduTypeSuPpdu)
            wlanSession.SetScalarAttributeI32(Nothing, niWLANGProperties.McsIndex, McsIndex(0))
            wlanSession.SetNumberOfSpaceTimeStreams(Nothing, numberOfSpaceTimeStream(0))
            wlanSession.SetDualCarrierModulationEnabled(Nothing, DcmEnabled(0))
            wlanSession.SetFecCodingType(Nothing, FecCodingType(0))
            wlanSession.SetPayloadDataLength("mpdu0", payloadLength(0))
         Else
            wlanSession.SetPPDUType(Nothing, niWLANGConstants.PpduTypeMuPpdu)
            wlanSession.SetRUAllocationMode(Nothing, RuAllocationMode)
            wlanSession.SetRUAllocation(Nothing, RuAllocation, RuAllocation.Length)

            If RuAllocationMode = niWLANGConstants.RUAllocationModeIndividual Then
               For i As Integer = 0 To numberOfUsers - 1
                  channelString = "user" & i
                  wlanSession.SetRUSize(channelString, RuSize(i))
                  wlanSession.SetRUOffsetMruIndex(channelString, RuOffset(i))

               Next
            Else
               wlanSession.GetNumberOfUsersFromRUAllocation(Nothing, numberOfUsers)
            End If

            For i As Integer = 0 To numberOfUsers - 1
               channelString = "user" & i
               wlanSession.SetScalarAttributeI32(channelString, niWLANGProperties.McsIndex, McsIndex(i))
               wlanSession.SetNumberOfSpaceTimeStreams(channelString, numberOfSpaceTimeStream(i))
               wlanSession.SetSTAID(channelString, StaId(i))
               wlanSession.SetDualCarrierModulationEnabled(channelString, DcmEnabled(i))
               wlanSession.SetFecCodingType(channelString, FecCodingType(i))
               wlanSession.SetPowerBoostFactor(channelString, PowerBoostFactor(i))
               wlanSession.SetUserEnabled(channelString, UserEnabled(i))
               channelString = channelString & "/mpdu0"
               wlanSession.SetPayloadDataLength(channelString, payloadLength(i))
            Next
         End If

         wlanSession.SetPulseShapingFilterEnabled(Nothing, PulseShapingFilterEnabled)
         wlanSession.SetPulseShapingFilterType(Nothing, FilterType)
         wlanSession.SetPulseShapingFilterParameter(Nothing, FilterParameter)
         wlanSession.SetOfdmWindowLength(Nothing, OfdmWindowLength)
         wlanSession.SetWindowingMethod(Nothing, WindowingMethod)
         wlanSession.SetPulseShapingFilterLength(Nothing, FilterLength)
         wlanSession.SetSwapIAndQEnabled(Nothing, SwapIandQEnabled)
         wlanSession.SetSampleClockRateFactor(Nothing, SampleClockRateFactor)

         wlanSession.SetRFBlankingEnabled(Nothing, niWLANGConstants.[True])
      End Sub

      Private Sub CreateAndDownloadWaveform()
         Dim iqRate As Double, waveformDuration As Double
         Dim PacketExtensionDuration As Double
         Dim iqWaveformSize As Integer
         Dim actualHeadRoom As Double() = New Double(numTx - 1) {}
         Dim script As [String] = "script GenerateWlan" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "                repeat forever" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & vbTab & "                generate Wlan" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "                end repeat" & vbCr & vbLf & vbTab & vbTab & vbTab & "                end script"

         Dim rfsgHandle As IntPtr() = New IntPtr(numTx - 1) {}
         Dim externalLOHandle As IntPtr() = New IntPtr(MAX_WLAN_CHANNELS - 1) {}

         For i As Integer = 0 To numTx - 1
            rfsgHandle(i) = rfsgSessions(i).GetInstrumentHandle().DangerousGetHandle()
            'if (externalLOSession(i) != null)
            '    externalLOHandle(i) = externalLOSession(i).GetInstrumentHandle().DangerousGetHandle()
         Next

         wlanSession.ConfigureMultipleDeviceSynchronization(rfsgHandle, (numTx * NumSeg), rfsgClockSource, triggerLines, triggerLines.Length)
         wlanSession.RFSGConfigureFrequencyMultipleLO(rfsgHandle, niWLANGConstants.LOSourceOnboard, externalLOHandle, carrierFrequency, carrierFrequency.Length, niWLANGConstants.[False],
             niWLANGConstants.[False])

         wlanSession.RFSGCreateAndDownloadMIMOWaveforms(rfsgHandle, Nothing, numTx, waveformName)
         'Configure Script

         For i As Integer = 0 To (numTx * NumSeg) - 1
            niWLANG.WLANG_RFSGConfigureScript(rfsgHandle(i), Nothing, script, powerLevel(i))
         Next
         wlanSession.RFSGMultipleDeviceInitiate(rfsgHandle)

         'Check for successful generation
         CheckGeneration()

         wlanSession.GetIqRate([String].Empty, iqRate)
         wlanSession.GetIqWaveformSize([String].Empty, iqWaveformSize)
         waveformDuration = iqWaveformSize / iqRate
         wlanSession.GetPacketExtensionDuration([String].Empty, PacketExtensionDuration)

         Console.WriteLine("Waveform Duration{s} : " & waveformDuration)
         Console.WriteLine("Packet Extension Duration{s} : " & PacketExtensionDuration)

         For i As Integer = 0 To numTx - 1
            channelString = "Channel" & i
            wlanSession.GetActualHeadroom(channelString, actualHeadRoom(i))
         Next

         Console.WriteLine("Press any key to exit")
         Console.ReadKey()
      End Sub

      Private Sub ConfigureRfsgSession()
         'Open all RFSG Sessions
         For i As Integer = 0 To (numTx * NumSeg) - 1
            If rfsgSessions(i) Is Nothing Then
               rfsgSessions(i) = New NIRfsg(rfsgResourceName(i), False, True)
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
               niWLANG.WLANG_RFSGClearDatabase(rfsgSessions(i).GetInstrumentHandle().DangerousGetHandle(), "", waveformName)
               rfsgSessions(i).Close()
            End If
         Next
      End Sub
   End Class
End Namespace
