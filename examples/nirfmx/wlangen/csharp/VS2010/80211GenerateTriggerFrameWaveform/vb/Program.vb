Imports NationalInstruments.RFToolkits.Wlan.Generation
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.NIRfsg.Internal
Imports System.Runtime.InteropServices

Namespace _80211GenerateWaveformMultiUser
   Class Program
      Friend Shared Sub Main(args As String())
         Dim example As New GenerateWaveformMultipleRFSGExample()
         example.Run()
      End Sub
   End Class

   Class GenerateWaveformMultipleRFSGExample
      Const triggerFrameSize As Integer = 100
      Private triggerFrameguardIntervalType As Integer
      Private triggerFrameAPTxPower As Integer, triggerFrameSTBCAllStreamsEnabled As Integer, triggerFrameNumberOfLtfSymbols As Integer
      Private triggerFrameRUSize As Integer, triggerFrameRUOffset As Integer, triggerFrameMCSIndex As Integer, triggerFramenumberOfSpaceTimeStream As Integer
      Private triggerFrameDCMEnabled As Integer, triggerFrameFECCodingType As Integer, triggerFramePayloadDataLength As Integer, triggerFrameTargetRSSI As Integer
      Private triggerFrameSTAID As Integer
      Private csRequired As Integer, ltfSize As Integer
      Private triggerFrameMSDUBits As Integer() = New Integer(triggerFrameSize - 1) {}
      Private triggerFrameChannelBandwidth As Double
      Private channelBandwidth As Double
      Private standard As Integer, MCSIndex As Integer, MACHeaderFrameControl As Integer, MACHeaderDuration As Integer, macHeaderRA As Integer, macHeaderTA As Integer
      Private payloadDataLength As Integer, ofdmDataRate As Integer, frameType As Integer, macPaddingDuration As Integer, triggerFrameNumberOfUsers As Integer, midamblePeriodicity As Integer
      Private lSigLength As Integer, preFecPaddingFactor As Integer, peDisambiguity As Integer, ldpcExtraSymbolSegment As Integer

      Private rfsgSession As NIRfsg
      Private externalLOSession As NIRfsg = Nothing
      Private rfsgResourceName As [String] = "RIO0"
      Private powerLevel As Double
      Private externalAttenuation As Double
      Private carrierFrequency As Double
      Private wlanSession As niWLANG = Nothing
      Private triggerFrameSession As niWLANG = Nothing
      Private rfsgClockSource As [String] = RfsgFrequencyReferenceSource.OnboardClock

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
            Console.WriteLine("Press any key exit")
            Console.ReadKey()
         Finally
            If wlanSession IsNot Nothing Then
               wlanSession.Close()
            End If
            If rfsgSession IsNot Nothing Then
               rfsgSession.Close()
            End If
         End Try
      End Sub

      Private Sub initGlobalVaribales()
         triggerFrameChannelBandwidth = 20000000.0
         triggerFrameguardIntervalType = niWLANGConstants.GuardIntervalTypeOneByFour
         triggerFrameAPTxPower = 0
         triggerFrameSTBCAllStreamsEnabled = niWLANGConstants.[False]
         triggerFrameNumberOfLtfSymbols = -1
         triggerFrameRUSize = niWLANGConstants.RuSize26
         triggerFrameRUOffset = 0
         triggerFrameMCSIndex = 0
         triggerFramenumberOfSpaceTimeStream = 1
         triggerFrameDCMEnabled = niWLANGConstants.[False]
         triggerFrameFECCodingType = niWLANGConstants.FecCodingTypeLdpc
         triggerFramePayloadDataLength = 100
         triggerFrameTargetRSSI = 78
         triggerFrameSTAID = 0
         triggerFrameNumberOfUsers = 1
         midamblePeriodicity = niWLANGConstants.MidamblePeriodicityNone

         standard = niWLANGConstants.Standard80211agOfdm
         channelBandwidth = 20000000.0
         MCSIndex = 0
         csRequired = 0
         ltfSize = niWLANGConstants.HeLtfSizeAuto
         ofdmDataRate = niWLANGConstants.OfdmDataRate6
         frameType = niWLANGConstants.PayloadFrameTypeTriggerFrame
         macPaddingDuration = niWLANGConstants.MaximumPaddingDuration0us
         MACHeaderFrameControl = &H24
         MACHeaderDuration = &H0
         macHeaderRA = &H0
         macHeaderTA = &H0

         lSigLength = -1
         preFecPaddingFactor = -1
         peDisambiguity = -1
         ldpcExtraSymbolSegment = -1

         powerLevel = -10
         externalAttenuation = 0
         carrierFrequency = 5180000000.0
      End Sub

      Private Sub ConfigureWlanGenerationSession()
         Dim isNewSession As Integer, actualArraySize As Integer, arraySize As Integer, generationDone As Integer
         Dim MSDUBitSize As Double
         If triggerFrameSession Is Nothing Then
            triggerFrameSession = New niWLANG("WLANGT", niWLANGConstants.CompatibilityVersion060000, isNewSession)
         End If

         triggerFrameSession.SetStandard(Nothing, niWLANGConstants.Standard80211AxMimoOfdm)
         triggerFrameSession.SetChannelBandwidth(Nothing, triggerFrameChannelBandwidth)
         triggerFrameSession.SetOFDMGuardIntervalType(Nothing, triggerFrameguardIntervalType)
         triggerFrameSession.SetPPDUType(Nothing, niWLANGConstants.PpduTypeTriggerBasedPpdu)
         triggerFrameSession.SetAPTXPower(Nothing, triggerFrameAPTxPower)
         triggerFrameSession.SetAmpduEnabled(Nothing, niWLANGConstants.[False])
         triggerFrameSession.SetStbcAllStreamsEnabled(Nothing, triggerFrameSTBCAllStreamsEnabled)
         triggerFrameSession.SetNumberOfLtfSymbols(Nothing, triggerFrameNumberOfLtfSymbols)
         triggerFrameSession.SetNumberOfUsers(Nothing, triggerFrameNumberOfUsers)
         triggerFrameSession.SetTriggerFrameCSRequired(channelString, csRequired)
         triggerFrameSession.SetLtfSize(channelString, ltfSize)
         triggerFrameSession.SetOfdmMidamblePeriodicity(Nothing, midamblePeriodicity)

         channelString = "user0"
         triggerFrameSession.SetRUSize(channelString, triggerFrameRUSize)
         triggerFrameSession.SetRUOffsetMruIndex(channelString, triggerFrameRUOffset)
         wlanSession.SetScalarAttributeI32(channelString, niWLANGProperties.McsIndex, triggerFrameMCSIndex)
         triggerFrameSession.SetNumberOfSpaceTimeStreams(channelString, triggerFramenumberOfSpaceTimeStream)
         triggerFrameSession.SetDualCarrierModulationEnabled(channelString, triggerFrameDCMEnabled)
         triggerFrameSession.SetFecCodingType(channelString, triggerFrameFECCodingType)
         triggerFrameSession.SetPayloadDataLength(channelString, triggerFramePayloadDataLength)
         triggerFrameSession.SetTargetRSSI(channelString, triggerFrameTargetRSSI)
         triggerFrameSession.SetSTAID(channelString, triggerFrameSTAID)

         If lSigLength = -1 Then
            triggerFrameSession.SetMacFcsEnabled(channelString, niWLANGConstants.[False])
            triggerFrameSession.SetMacHeaderEnabled(channelString, niWLANGConstants.[False])
            triggerFrameSession.SetPayloadDataLength(channelString, triggerFramePayloadDataLength)
         Else
            triggerFrameSession.SetLSIGLength(Nothing, lSigLength)
            triggerFrameSession.SetPreFECPaddingFactor(Nothing, preFecPaddingFactor)
            triggerFrameSession.SetPEDisambiguity(Nothing, peDisambiguity)
            triggerFrameSession.SetLdpcExtraSymbolSegment(Nothing, ldpcExtraSymbolSegment)
            triggerFrameSession.SetNumberOfMPDUs(Nothing, niWLANGConstants.[True])
            triggerFrameSession.SetAutoPayloadDataLengthMode(Nothing, niWLANGConstants.True)
         End If

         triggerFrameSession.CreateTriggerFrameMSDU(Nothing, generationDone, Nothing, actualArraySize)
         If actualArraySize > 0 Then
            triggerFrameMSDUBits = New Integer(actualArraySize - 1) {}

            triggerFrameSession.CreateTriggerFrameMSDU(Nothing, generationDone, triggerFrameMSDUBits, arraySize)
         End If
         If wlanSession Is Nothing Then
            wlanSession = New niWLANG("WLANG", niWLANGConstants.CompatibilityVersion050000, isNewSession)
         End If

         wlanSession.SetStandard(Nothing, standard)
         wlanSession.SetChannelBandwidth(Nothing, channelBandwidth)
         wlanSession.SetScalarAttributeI32(Nothing, niWLANGProperties.OfdmDataRate, ofdmDataRate)
         wlanSession.SetScalarAttributeI32(Nothing, niWLANGProperties.McsIndex, MCSIndex)

         wlanSession.SetRFBlankingEnabled(Nothing, niWLANGConstants.[True])

         wlanSession.SetPayloadMacFrameType(Nothing, frameType)
         wlanSession.SetScalarAttributeI32(Nothing, niWLANGProperties.PayloadMacFrameType, ofdmDataRate)
         wlanSession.SetMacFrameControl(Nothing, MACHeaderFrameControl)
         wlanSession.SetMacDurationOrId(Nothing, MACHeaderDuration)
         wlanSession.SetMacAddress1(Nothing, macHeaderRA)
         wlanSession.SetMacAddress2(Nothing, macHeaderTA)
         wlanSession.SetTriggerFrameMaximumMacPaddingDuration(Nothing, macPaddingDuration)

         wlanSession.SetPayloadDataType(Nothing, niWLANGConstants.UserDefined)
         wlanSession.SetPayloadUserDefinedBits(Nothing, triggerFrameMSDUBits, actualArraySize)
         MSDUBitSize = actualArraySize \ 8
         payloadDataLength = CInt(Math.Truncate(Math.Ceiling(MSDUBitSize)))
         wlanSession.SetPayloadDataLength(Nothing, payloadDataLength)
      End Sub

      Private Sub CreateAndDownloadWaveform()
         Dim iqRate As Double, waveformDuration As Double, actualHeadRoom As Double
         Dim iqWaveformSize As Integer
         Dim externalLOHandle As New IntPtr()

         Dim script As [String] = "script GenerateWlan" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "                repeat forever" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & vbTab & "                generate Wlan" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "                end repeat" & vbCr & vbLf & vbTab & vbTab & vbTab & "                end script"

         If externalLOSession IsNot Nothing Then
            externalLOHandle = externalLOSession.GetInstrumentHandle().DangerousGetHandle()
         End If
         wlanSession.RFSGConfigureFrequencySingleLO(New IntPtr() {rfsgSession.GetInstrumentHandle().DangerousGetHandle()}, niWLANGConstants.LOSourceOnboard, externalLOHandle, carrierFrequency, niWLANGConstants.[False], niWLANGConstants.[False])

         If standard = niWLANGConstants.Standard80211AcMimoOfdm OrElse standard = niWLANGConstants.Standard80211nMimoOfdm OrElse standard = niWLANGConstants.Standard80211AxMimoOfdm OrElse standard = niWLANGConstants.Standard80211beMimoOfdm OrElse standard = niWLANGConstants.Standard80211bnMimoOfdm Then
            wlanSession.RFSGCreateAndDownloadMIMOWaveforms(New IntPtr() {rfsgSession.GetInstrumentHandle().DangerousGetHandle()}, Nothing, 1, waveformName)
         Else
            wlanSession.RFSGCreateAndDownloadMIMOWaveforms(New IntPtr() {rfsgSession.GetInstrumentHandle().DangerousGetHandle()}, Nothing, 1, waveformName)
            channelString = ""
         End If

         triggerFrameSession.GetIqRate(Nothing, iqRate)
         triggerFrameSession.GetIqWaveformSize(Nothing, iqWaveformSize)
         waveformDuration = iqWaveformSize / iqRate

         Console.WriteLine("Waveform Duration : {0}", waveformDuration)
         channelString = "channel0"
         triggerFrameSession.GetActualHeadroom(channelString, actualHeadRoom)

         Console.WriteLine("Actual HeadRoom :", actualHeadRoom)

         wlanSession.Close()

         niWLANG.WLANG_RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, script, powerLevel)
         rfsgSession.Initiate()
         'Check for successful generation
         CheckGeneration()
         Console.WriteLine("Press any key to exit")
         Console.ReadKey()
      End Sub

      Private Sub ConfigureRfsgSession()
         'Open all RFSG Sessions
         If rfsgSession Is Nothing Then
            rfsgSession = New NIRfsg(rfsgResourceName, True, False)
         End If
         rfsgSession.FrequencyReference.Configure(rfsgClockSource, 10000000.0)
         rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
         rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
         rfsgSession.RF.ExternalGain = -(externalAttenuation)
      End Sub

      Private Function CheckGeneration() As Boolean
         Dim status As RfsgGenerationStatus = RfsgGenerationStatus.InProgress
         status = rfsgSession.CheckGenerationStatus()
         Return Convert.ToBoolean(status)
      End Function

      Private Sub StopGeneration()
         rfsgSession.Abort()
         rfsgSession.RF.OutputEnabled = False
         rfsgSession.Utility.Commit()
         niWLANG.WLANG_RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", waveformName)
      End Sub
   End Class
End Namespace
