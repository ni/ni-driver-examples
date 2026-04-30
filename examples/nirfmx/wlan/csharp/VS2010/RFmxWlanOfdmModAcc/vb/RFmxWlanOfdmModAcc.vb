'Steps:
'1. Open a new RFmx session.
'2. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
'5. Configure Standard and Channel Bandwidth Properties.
'6. Select OFDMModAcc measurement and enable the traces.
'7. Configure the Measurement Interval.
'8. Configure Frequency Error Estimation Method.
'9. Configure Amplitude Tracking Enabled.
'10. Configure Phase Tracking Enabled.
'11. Configure Symbol Clock Error Correction Enabled.
'12. Configure Channel Estimation Type.
'13. Configure Averaging parameters.
'14. Initiate Measurement.
'15. Fetch OFDMModAcc Measurements.
'16. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WlanMX

Namespace NationalInstruments.Examples.RFmxWlanOfdmModAcc

   Public Class RFmxWlanOfdmModAcc
      Private instrSession As RFmxInstrMX
      Private wlan As RFmxWlanMX
      Private resourceName As String

      Private centerFrequency As Double
      Private referenceLevel As Double
      Private externalAttenuation As Double

      Private frequencyReferenceSource As String
      Private frequencyReferenceFrequency As Double

      Private iqPowerEdgeEnabled As Boolean
      Private iqPowerEdgeLevel As Double
      Private triggerDelay As Double
      Private minimumQuietTimeMode As RFmxWlanMXTriggerMinimumQuietTimeMode
      Private minimumQuietTime As Double

      Private standard As RFmxWlanMXStandard

      Private channelBandwidth As Double

      Private measurementOffset As Integer
      Private maximumMeasurementLength As Integer

      Private frequencyErrorEstimationMethod As RFmxWlanMXOfdmModAccFrequencyErrorEstimationMethod
      Private channelEstimationType As RFmxWlanMXOfdmModAccChannelEstimationType
      Private phaseTrackingEnabled As RFmxWlanMXOfdmModAccPhaseTrackingEnabled
      Private amplitudeTrackingEnabled As RFmxWlanMXOfdmModAccAmplitudeTrackingEnabled
      Private symbolClockErrorCorrectionEnabled As RFmxWlanMXOfdmModAccSymbolClockErrorCorrectionEnabled

      Private averagingEnabled As RFmxWlanMXOfdmModAccAveragingEnabled
      Private averagingCount As Integer

      Private timeout As Double

      Private compositeRmsEvmMean As Double
      Private compositeDataRmsEvmMean As Double
      Private compositePilotRmsEvmMean As Double
      Private numberOfSymbolsUsed As Integer
      Private frequencyErrorMean As Double
      Private symbolClockErrorMean As Double

      Private ppduType As RFmxWlanMXOfdmPpduType
      Private mcsIndex As Integer
      Private guardIntervalType As RFmxWlanMXOfdmGuardIntervalType
      Private lSigParityCheckStatus As RFmxWlanMXOfdmModAccLSigParityCheckStatus
      Private sigCrcStatus As RFmxWlanMXOfdmModAccSigCrcStatus
      Private sigBCrcStatus As RFmxWlanMXOfdmModAccSigBCrcStatus

      Private relativeIQOriginOffsetMean As Double
      Private iqGainImbalanceMean As Double
      Private iqQuadratureErrorMean As Double
      Private absoluteIQOriginOffsetMean As Double
      Private iqTimingSkewMean As Double

      Private pilotConstellation As ComplexSingle()
      Private dataConstellation As ComplexSingle()
      Private chainRmsEvmPerSubcarrierMean As AnalogWaveform(Of Single)

      Public Sub Run()
         Try
            InitializeVariables()
            InitializeInstr()
            ConfigureWlan()
            RetrieveResults()
            PrintResults()
         Catch ex As Exception
            DisplayError(ex)
         Finally
            ' Close session

            CloseSession()
            Console.WriteLine("Press any key to exit")
            Console.ReadKey()
         End Try
      End Sub

      Private Sub InitializeVariables()
         resourceName = "RFSA"

         centerFrequency = 2412000000.0
         ' (Hz)
         referenceLevel = 0.0
         ' (dBm)
         externalAttenuation = 0.0
         ' (dB)

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
         frequencyReferenceFrequency = 10000000.0
         ' (Hz)

         iqPowerEdgeEnabled = True
         iqPowerEdgeLevel = -20.0
         ' (dB)
         triggerDelay = 0.0
         ' (s)
         minimumQuietTimeMode = RFmxWlanMXTriggerMinimumQuietTimeMode.Auto
         minimumQuietTime = 0.000005
         ' (s)

         standard = RFmxWlanMXStandard.Standard802_11ag

         channelBandwidth = 20000000.0
         ' (Hz)

         measurementOffset = 0
         ' (symbols)
         maximumMeasurementLength = 16
         ' (symbols)

         frequencyErrorEstimationMethod = RFmxWlanMXOfdmModAccFrequencyErrorEstimationMethod.PreambleAndPilots
         channelEstimationType = RFmxWlanMXOfdmModAccChannelEstimationType.Reference
         phaseTrackingEnabled = RFmxWlanMXOfdmModAccPhaseTrackingEnabled.True
         amplitudeTrackingEnabled = RFmxWlanMXOfdmModAccAmplitudeTrackingEnabled.False
         symbolClockErrorCorrectionEnabled = RFmxWlanMXOfdmModAccSymbolClockErrorCorrectionEnabled.True

         averagingEnabled = RFmxWlanMXOfdmModAccAveragingEnabled.False
         averagingCount = 10

         timeout = 10.0
         ' (s)

         ppduType = RFmxWlanMXOfdmPpduType.NonHT
         guardIntervalType = RFmxWlanMXOfdmGuardIntervalType.OneByFour
         lSigParityCheckStatus = RFmxWlanMXOfdmModAccLSigParityCheckStatus.NotApplicable
         sigCrcStatus = RFmxWlanMXOfdmModAccSigCrcStatus.NotApplicable
         sigBCrcStatus = RFmxWlanMXOfdmModAccSigBCrcStatus.NotApplicable

      End Sub

      Private Sub InitializeInstr()
         instrSession = New RFmxInstrMX(resourceName, "")
      End Sub

      Private Sub ConfigureWlan()
         wlan = instrSession.GetWlanSignalConfiguration()
         ' Create a new RFmx Session
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
         wlan.ConfigureFrequency("", centerFrequency)
         wlan.ConfigureReferenceLevel("", referenceLevel)
         wlan.ConfigureExternalAttenuation("", externalAttenuation)
         wlan.ConfigureIQPowerEdgeTrigger("", "0", RFmxWlanMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
            triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxWlanMXIQPowerEdgeTriggerLevelType.Relative,
            iqPowerEdgeEnabled)
         wlan.ConfigureStandard("", standard)
         wlan.ConfigureChannelBandwidth("", channelBandwidth)
         wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.OfdmModAcc, True)
         wlan.OfdmModAcc.Configuration.ConfigureMeasurementLength("", measurementOffset, maximumMeasurementLength)
         wlan.OfdmModAcc.Configuration.ConfigureFrequencyErrorEstimationMethod("", frequencyErrorEstimationMethod)
         wlan.OfdmModAcc.Configuration.ConfigureAmplitudeTrackingEnabled("", amplitudeTrackingEnabled)
         wlan.OfdmModAcc.Configuration.ConfigurePhaseTrackingEnabled("", phaseTrackingEnabled)
         wlan.OfdmModAcc.Configuration.ConfigureSymbolClockErrorCorrectionEnabled("", symbolClockErrorCorrectionEnabled)
         wlan.OfdmModAcc.Configuration.ConfigureChannelEstimationType("", channelEstimationType)
         wlan.OfdmModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
         wlan.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         wlan.OfdmModAcc.Results.FetchCompositeRmsEvm("", timeout, compositeRmsEvmMean, compositeDataRmsEvmMean,
            compositePilotRmsEvmMean)
         wlan.OfdmModAcc.Results.FetchNumberOfSymbolsUsed("", timeout, numberOfSymbolsUsed)
         wlan.OfdmModAcc.Results.FetchFrequencyErrorMean("", timeout, frequencyErrorMean)
         wlan.OfdmModAcc.Results.FetchSymbolClockErrorMean("", timeout, symbolClockErrorMean)
         wlan.OfdmModAcc.Results.FetchIQImpairments("", timeout, relativeIQOriginOffsetMean, iqGainImbalanceMean,
            iqQuadratureErrorMean, absoluteIQOriginOffsetMean, iqTimingSkewMean)
         wlan.OfdmModAcc.Results.FetchPpduType("", timeout, ppduType)
         wlan.OfdmModAcc.Results.FetchMcsIndex("", timeout, mcsIndex)
         wlan.OfdmModAcc.Results.FetchGuardIntervalType("", timeout, guardIntervalType)
         wlan.OfdmModAcc.Results.FetchLSigParityCheckStatus("", timeout, lSigParityCheckStatus)
         wlan.OfdmModAcc.Results.FetchSigCrcStatus("", timeout, sigCrcStatus)
         wlan.OfdmModAcc.Results.FetchSigBCrcStatus("", timeout, sigBCrcStatus)
         wlan.OfdmModAcc.Results.FetchPilotConstellationTrace("", timeout, pilotConstellation)
         wlan.OfdmModAcc.Results.FetchDataConstellationTrace("", timeout, dataConstellation)
         wlan.OfdmModAcc.Results.FetchChainRmsEvmPerSubcarrierMeanTrace("", timeout, chainRmsEvmPerSubcarrierMean)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("------------------EVM------------------" & vbLf)
         Console.WriteLine("------------------Composite EVM------------------")
         Console.WriteLine("RMS EVM Mean (dB)                       : {0}", compositeRmsEvmMean)
         Console.WriteLine("Data RMS EVM Mean (dB)                  : {0}", compositeDataRmsEvmMean)
         Console.WriteLine("Pilot RMS EVM Mean (dB)                 : {0}" & vbLf, compositePilotRmsEvmMean)
         Console.WriteLine("Number of Symbols Used                  : {0}", numberOfSymbolsUsed)
         Console.WriteLine(vbLf & "------------------Impairments & PPDU Info------------------" & vbLf)
         Console.WriteLine("Frequency Error Mean (Hz)               : {0}", frequencyErrorMean)
         Console.WriteLine("Symbol Clock Error Mean (ppm)           : {0}", symbolClockErrorMean)
         Console.WriteLine(vbLf & "------------------IQ Impairments------------------")
         Console.WriteLine("Relative I/Q Origin Offset Mean (dB)    : {0}", relativeIQOriginOffsetMean)
         Console.WriteLine("Absolute I/Q Origin Offset Mean (dBm)   : {0}", absoluteIQOriginOffsetMean)
         Console.WriteLine("I/Q Gain Imbalance Mean (dB)            : {0}", iqGainImbalanceMean)
         Console.WriteLine("I/Q Quadrature Error Mean (deg)         : {0}", iqQuadratureErrorMean)
         Console.WriteLine("I/Q Timing Skew Mean (s)                : {0}", iqTimingSkewMean)
         Console.WriteLine(vbLf & "------------------PPDU Info------------------")
         Console.WriteLine("PPDU Type                               : {0}", ppduType)
         Console.WriteLine("MCS Index                               : {0}", mcsIndex)
         Console.WriteLine("Guard Interval Type                     : {0}", guardIntervalType)
         Console.WriteLine("L-SIG Parity Check Status               : {0}", lSigParityCheckStatus)
         Console.WriteLine("SIG CRC Status                          : {0}", sigCrcStatus)
         Console.WriteLine("SIG-B CRC Status                        : {0}" & vbLf, sigBCrcStatus)
      End Sub

      Private Sub CloseSession()
         If wlan IsNot Nothing Then
            wlan.Dispose()
            wlan = Nothing
         End If
         If instrSession IsNot Nothing Then
            instrSession.Close()
            instrSession = Nothing
         End If
      End Sub

      Private Shared Sub DisplayError(ex As Exception)
         Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
      End Sub

   End Class
End Namespace
