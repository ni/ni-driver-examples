'Steps:
'1. Open a New RFmx Session.
'2. Configure Frequency Reference.
'3. Configure LO Leakage Avoidance Enabled, LO source And Downconverter Frequency Offset(Hz).
'4. Configure basic RF signal properties(Center Frequency, RF Attenuation And External Attenuation)
'5. Enabling Pulse And Traces Result.
'6. Configure Trigger Type And Trigger Parameters.
'7. Configure Acquisition Settings.
'8. Configure Pulse Detection Settings
'9. Configure State And Threshold Level Settings.
'10. Configure Measurement Point Settings.
'11. Configure Frequency & Phase Settings.
'12. Configure Modulation Settings.
'13. Enabling Pulse Stability enabled as False And Pulse Metrics enabled settings as True.
'14. Initiate the Measurement.
'15. Wait for Measurement to complete.
'16. Results Phase, Frequency Measurements And FM Chirp results as well as their Statistical results,Phase(Wrapped) Trace, Frequency Trace. 
'17. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.PulseMX

Namespace NationalInstruments.Examples.RFmxPulsePhaseAndFrequencyAdvanced
    Public Class RFmxPulsePhaseAndFrequencyAdvanced
        Private instrSession As RFmxInstrMX
        Private Pulse As RFmxPulseMX
        Private resourceName As String

        Private centerFrequency As Double
        Private referenceLevel As Double
        Private externalAttenuation As Double

        Private downConverterFrequencyOffset As Double

        Private LOLeakageAvoidanceEnabled As RFmxInstrMXLOLeakageAvoidanceEnabled
        Private LOSource As String

        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double

        Private iqPowerEdgeEnabled As Boolean
        Private iqPowerEdgeLevel As Double
        Private triggerDelay As Double
        Private minimumQuietTimeMode As RFmxPulseMXTriggerMinimumQuietTimeMode
        Private minimumQuietTime As Double

        Private measurementBandwidth As Double
        Private measurementFilterType As RFmxPulseMXMeasurementFilterType
        Private acquisitionLength As Double
        Private maximumPulseCountEnabled As RFmxPulseMXMaximumPulseCountEnabled
        Private maximumPulseCount As Integer

        Private pulseDetectionReference As RFmxPulseMXPulseDetectionReference
        Private pulseDetectionThreshold As Double
        Private pulseDetectionHysteresis As Double
        Private pulseDetectionMinimumOffDuration As Double

        Private pulseMeasurementPointReference As RFmxPulseMXPulseMeasurementPointReference
        Private pulseMeasurementPointOffset As Double
        Private pulseMeasurementPointAveragingDuration As Double

        Private pulseLevelComputationMethod As RFmxPulseMXPulseLevelComputationMethod
        Private pulseUpperThresholdLevel As Double
        Private pulseWidthThresholdLevel As Double
        Private pulseLowerThresholdLevel As Double

        Private pulseFrequencyPhaseDeviationRangeReference As RFmxPulseMXPulseFrequencyAndPhaseDeviationRangeReference
        Private pulseFrequencyPhaseDeviationRangeLength As Double
        Private pulseFrequencyPhaseDeviationRangeEdgeStart As Double
        Private pulseFrequencyPhaseDeviationRangeEdgeStop As Double

        Private pulseModulationType As RFmxPulseMXPulseModulationType
        Private pulseCWFrequencyOffsetAuto As RFmxPulseMXPulseCWFrequencyOffsetAuto
        Private pulseCWFrequencyOffset As Double

        Private timeout As Double
        ' (s)


        Private pulseResultsAveragePhase As Double()
        ' (deg)
        Private pulseResultsPhaseDeviation As Double()
        ' (deg)
        Private pulseResultsPhaseErrorRms As Double()
        ' (deg)

        Private pulseResultsAverageFrequency As Double()
        ' (Hz)
        Private pulseResultsFrequencyDeviation As Double()
        ' (Hz)
        Private pulseResultsFrequencyErrorRms As Double()
        ' (Hz)

        Private pulseResultsFMChirpRate As Double()
        ' (Hz/us)
        Private pulseResultsFMChirpRate2 As Double()
        ' (Hz/us)

        Private pulseResultsAveragePhaseMean As Double
        '(deg)
        Private pulseResultsAveragePhaseMaximum As Double
        '(deg)
        Private pulseResultsAveragePhaseMinimum As Double
        '(deg)
        Private pulseResultsAveragePhaseSD As Double
        '(deg)
        Private pulseResultsPhaseDeviationMean As Double
        '(deg)
        Private pulseResultsPhaseDeviationMaximum As Double
        '(deg)
        Private pulseResultsPhaseDeviationMinimum As Double
        '(deg)
        Private pulseResultsPhaseDeviationSD As Double
        '(deg)
        Private pulseResultsPhaseErrorRmsMean As Double
        '(deg)
        Private pulseResultsPhaseErrorRmsMaximum As Double
        '(deg)
        Private pulseResultsPhaseErrorRmsMinimum As Double
        '(deg)
        Private pulseResultsPhaseErrorRmsSD As Double
        '(deg)

        Private pulseResultsAverageFrequencyMean As Double
        '(Hz)
        Private pulseResultsAverageFrequencyMaximum As Double
        '(Hz)
        Private pulseResultsAverageFrequencyMinimum As Double
        '(Hz)
        Private pulseResultsAverageFrequencySD As Double
        '(Hz)
        Private pulseResultsFrequencyDeviationMean As Double
        '(Hz)
        Private pulseResultsFrequencyDeviationMaximum As Double
        '(Hz)
        Private pulseResultsFrequencyDeviationMinimum As Double
        '(Hz)
        Private pulseResultsFrequencyDeviationSD As Double
        '(Hz)
        Private pulseResultsFrequencyErrorRmsMean As Double
        '(Hz)
        Private pulseResultsFrequencyErrorRmsMaximum As Double
        '(Hz)
        Private pulseResultsFrequencyErrorRmsMinimum As Double
        '(Hz)
        Private pulseResultsFrequencyErrorRmsSD As Double
        '(Hz)

        Private pulseResultsFMChirpRateMean As Double
        '(Hz/us)
        Private pulseResultsFMChirpRateMaximum As Double
        '(Hz/us)
        Private pulseResultsFMChirpRateMinimum As Double
        '(Hz/us)
        Private pulseResultsFMChirpRateSD As Double
        '(Hz/us)
        Private pulseResultsFMChirpRate2Mean As Double
        '(Hz/us)
        Private pulseResultsFMChirpRate2Maximum As Double
        '(Hz/us)
        Private pulseResultsFMChirpRate2Minimum As Double
        '(Hz/us)
        Private pulseResultsFMChirpRate2SD As Double
        '(Hz/us)

        Private phaseWrappedTrace As AnalogWaveform(Of Single)
        ' (deg)
        Private frequencyTrace As AnalogWaveform(Of Single)
       ' (Hz)

  Public Sub Run()
            Try
                InitializeVariables()
                InitializeInstr()
                ConfigurePulse()
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

            centerFrequency = 1000000000.0
            ' (Hz)
            referenceLevel = -10.0
            ' (dBm)
            externalAttenuation = 0.0
            ' (dB)

            downConverterFrequencyOffset = 0.0
            ' (Hz)

            LOLeakageAvoidanceEnabled = RFmxInstrMXLOLeakageAvoidanceEnabled.True
            LOSource = RFmxInstrMXConstants.LOSourceOnboard

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
            frequencyReferenceFrequency = 10000000.0
            ' (Hz)

            iqPowerEdgeEnabled = True
            iqPowerEdgeLevel = -20.0
            ' (dBm)
            triggerDelay = 0.0
            ' (s)
            minimumQuietTimeMode = RFmxPulseMXTriggerMinimumQuietTimeMode.Auto
            minimumQuietTime = 0.000005
            ' (s)

            measurementBandwidth = 80000000.0
            '(Hz)
            measurementFilterType = RFmxPulseMXMeasurementFilterType.Gaussian
            acquisitionLength = 0.001
            '(s)
            maximumPulseCountEnabled = RFmxPulseMXMaximumPulseCountEnabled.False
            maximumPulseCount = 100

            pulseLevelComputationMethod = RFmxPulseMXPulseLevelComputationMethod.Median
            pulseUpperThresholdLevel = 90.0
            '(%)
            pulseWidthThresholdLevel = 50.0
            '(%)
            pulseLowerThresholdLevel = 10.0
            '(%)

            pulseMeasurementPointReference = RFmxPulseMXPulseMeasurementPointReference.Center
            pulseMeasurementPointOffset = 0.0
            '(s)
            pulseMeasurementPointAveragingDuration = 0.0
            '(s)

            pulseDetectionReference = RFmxPulseMXPulseDetectionReference.ReferenceLevel
            pulseDetectionThreshold = -20.0
            pulseDetectionHysteresis = 1.0
            '(dB)
            pulseDetectionMinimumOffDuration = 0.00000005
            '(s)

            pulseFrequencyPhaseDeviationRangeReference =
                RFmxPulseMXPulseFrequencyAndPhaseDeviationRangeReference.Center
            pulseFrequencyPhaseDeviationRangeLength = 75.0
            '(%)
            pulseFrequencyPhaseDeviationRangeEdgeStart = 0.0
            '(s)
            pulseFrequencyPhaseDeviationRangeEdgeStop = 0.0
            '(s)

            pulseModulationType = RFmxPulseMXPulseModulationType.CW
            pulseCWFrequencyOffsetAuto = RFmxPulseMXPulseCWFrequencyOffsetAuto.True
            pulseCWFrequencyOffset = 0.0
            '(Hz)

            timeout = 10.0
            ' (s)
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigurePulse()
            Pulse = instrSession.GetPulseSignalConfiguration()
            ' Create a new RFmx Session
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            instrSession.SetLOLeakageAvoidanceEnabled("", LOLeakageAvoidanceEnabled)
            instrSession.SetLOSource("", LOSource)
            instrSession.SetDownconverterFrequencyOffset("", downConverterFrequencyOffset)
            Pulse.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
            Pulse.SelectMeasurements("", RFmxPulseMXMeasurementTypes.Pulse, False)
            Pulse.ConfigureIQPowerEdgeTrigger("", "0", RFmxPulseMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
               triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxPulseMXIQPowerEdgeTriggerLevelType.Relative,
               iqPowerEdgeEnabled)

            Pulse.SetMeasurementBandwidth("", measurementBandwidth)
            Pulse.SetMeasurementFilterType("", measurementFilterType)
            Pulse.SetAcquisitionLength("", acquisitionLength)
            Pulse.SetMaximumPulseCountEnabled("", maximumPulseCountEnabled)
            Pulse.SetMaximumPulseCount("", maximumPulseCount)

            Pulse.Pulse.Configuration.SetDetectionReference("", pulseDetectionReference)
            Pulse.Pulse.Configuration.SetDetectionThreshold("", pulseDetectionThreshold)
            Pulse.Pulse.Configuration.SetDetectionHysteresis("", pulseDetectionHysteresis)
            Pulse.Pulse.Configuration.SetDetectionMinimumOffDuration("", pulseDetectionMinimumOffDuration)

            Pulse.Pulse.Configuration.SetLevelComputationMethod("", pulseLevelComputationMethod)
            Pulse.Pulse.Configuration.SetUpperThresholdLevel("", pulseUpperThresholdLevel)
            Pulse.Pulse.Configuration.SetWidthThresholdLevel("", pulseWidthThresholdLevel)
            Pulse.Pulse.Configuration.SetLowerThresholdLevel("", pulseLowerThresholdLevel)

            Pulse.Pulse.Configuration.SetMeasurementPointReference("", pulseMeasurementPointReference)
            Pulse.Pulse.Configuration.SetMeasurementPointOffset("", pulseMeasurementPointOffset)
            Pulse.Pulse.Configuration.SetMeasurementPointAveragingDuration("", pulseMeasurementPointAveragingDuration)

            Pulse.Pulse.Configuration.SetFrequencyAndPhaseDeviationRangeReference("",
                pulseFrequencyPhaseDeviationRangeReference)
            Pulse.Pulse.Configuration.SetFrequencyAndPhaseDeviationRangeLength("",
                pulseFrequencyPhaseDeviationRangeLength)
            Pulse.Pulse.Configuration.SetFrequencyAndPhaseDeviationRangeEdgeStart("",
                pulseFrequencyPhaseDeviationRangeEdgeStart)
            Pulse.Pulse.Configuration.SetFrequencyAndPhaseDeviationRangeEdgeStop("",
                pulseFrequencyPhaseDeviationRangeEdgeStop)

            Pulse.Pulse.Configuration.SetFrequencyAndPhaseModulationType("", pulseModulationType)
            Pulse.Pulse.Configuration.SetFrequencyAndPhaseCWFrequencyOffsetAuto("", pulseCWFrequencyOffsetAuto)
            Pulse.Pulse.Configuration.SetFrequencyAndPhaseCWFrequencyOffset("", pulseCWFrequencyOffset)

            Pulse.Pulse.Configuration.SetMetricsEnabled("", RFmxPulseMXPulseMetricsEnabled.True)
            Pulse.Pulse.Configuration.SetStabilityEnabled("", RFmxPulseMXPulseStabilityEnabled.False)
            Pulse.Pulse.Configuration.SetAllTracesEnabled("", True)

            Pulse.Initiate("", "")

            Pulse.WaitForMeasurementComplete("", timeout)
        End Sub

        Private Sub RetrieveResults()
            Pulse.Pulse.Results.GetAveragePhase("", pulseResultsAveragePhase)
            Pulse.Pulse.Results.GetPhaseDeviation("", pulseResultsPhaseDeviation)
            Pulse.Pulse.Results.GetPhaseErrorRms("", pulseResultsPhaseErrorRms)

            Pulse.Pulse.Results.GetAverageFrequency("", pulseResultsAverageFrequency)
            Pulse.Pulse.Results.GetFrequencyDeviation("", pulseResultsFrequencyDeviation)
            Pulse.Pulse.Results.GetFrequencyErrorRms("", pulseResultsFrequencyErrorRms)

            Pulse.Pulse.Results.GetFMChirpRate("", pulseResultsFMChirpRate)
            Pulse.Pulse.Results.GetFMChirpRate2("", pulseResultsFMChirpRate2)

            Pulse.Pulse.Results.GetAveragePhaseMean("", pulseResultsAveragePhaseMean)
            Pulse.Pulse.Results.GetAveragePhaseMaximum("", pulseResultsAveragePhaseMaximum)
            Pulse.Pulse.Results.GetAveragePhaseMinimum("", pulseResultsAveragePhaseMinimum)
            Pulse.Pulse.Results.GetAveragePhaseStandardDeviation("", pulseResultsAveragePhaseSD)

            Pulse.Pulse.Results.GetPhaseDeviationMean("", pulseResultsPhaseDeviationMean)
            Pulse.Pulse.Results.GetPhaseDeviationMaximum("", pulseResultsPhaseDeviationMaximum)
            Pulse.Pulse.Results.GetPhaseDeviationMinimum("", pulseResultsPhaseDeviationMinimum)
            Pulse.Pulse.Results.GetPhaseDeviationStandardDeviation("", pulseResultsPhaseDeviationSD)

            Pulse.Pulse.Results.GetPhaseErrorRmsMean("", pulseResultsPhaseErrorRmsMean)
            Pulse.Pulse.Results.GetPhaseErrorRmsMaximum("", pulseResultsPhaseErrorRmsMaximum)
            Pulse.Pulse.Results.GetPhaseErrorRmsMinimum("", pulseResultsPhaseErrorRmsMinimum)
            Pulse.Pulse.Results.GetPhaseErrorRmsStandardDeviation("", pulseResultsPhaseErrorRmsSD)

            Pulse.Pulse.Results.GetAverageFrequencyMean("", pulseResultsAverageFrequencyMean)
            Pulse.Pulse.Results.GetAverageFrequencyMaximum("", pulseResultsAverageFrequencyMaximum)
            Pulse.Pulse.Results.GetAverageFrequencyMinimum("", pulseResultsAverageFrequencyMinimum)
            Pulse.Pulse.Results.GetAverageFrequencyStandardDeviation("", pulseResultsAverageFrequencySD)

            Pulse.Pulse.Results.GetFrequencyDeviationMean("", pulseResultsFrequencyDeviationMean)
            Pulse.Pulse.Results.GetFrequencyDeviationMaximum("", pulseResultsFrequencyDeviationMaximum)
            Pulse.Pulse.Results.GetFrequencyDeviationMinimum("", pulseResultsFrequencyDeviationMinimum)
            Pulse.Pulse.Results.GetFrequencyDeviationStandardDeviation("", pulseResultsFrequencyDeviationSD)

            Pulse.Pulse.Results.GetFrequencyErrorRmsMean("", pulseResultsFrequencyErrorRmsMean)
            Pulse.Pulse.Results.GetFrequencyErrorRmsMaximum("", pulseResultsFrequencyErrorRmsMaximum)
            Pulse.Pulse.Results.GetFrequencyErrorRmsMinimum("", pulseResultsFrequencyErrorRmsMinimum)
            Pulse.Pulse.Results.GetFrequencyErrorRmsStandardDeviation("", pulseResultsFrequencyErrorRmsSD)

            Pulse.Pulse.Results.GetFMChirpRateMean("", pulseResultsFMChirpRateMean)
            Pulse.Pulse.Results.GetFMChirpRateMaximum("", pulseResultsFMChirpRateMaximum)
            Pulse.Pulse.Results.GetFMChirpRateMinimum("", pulseResultsFMChirpRateMinimum)
            Pulse.Pulse.Results.GetFMChirpRateStandardDeviation("", pulseResultsFMChirpRateSD)

            Pulse.Pulse.Results.GetFMChirpRate2Mean("", pulseResultsFMChirpRate2Mean)
            Pulse.Pulse.Results.GetFMChirpRate2Maximum("", pulseResultsFMChirpRate2Maximum)
            Pulse.Pulse.Results.GetFMChirpRate2Minimum("", pulseResultsFMChirpRate2Minimum)
            Pulse.Pulse.Results.GetFMChirpRate2StandardDeviation("", pulseResultsFMChirpRate2SD)

            Pulse.Pulse.Results.FetchPhaseWrappedTrace("", timeout, phaseWrappedTrace)
            Pulse.Pulse.Results.FetchFrequencyTrace("", timeout, frequencyTrace)

  End Sub

        Private Sub PrintResults()
            Console.WriteLine(vbLf & "-----------------------Phase Results--------------------------" & vbLf)
            For i As Integer = 0 To pulseResultsAveragePhase.Length - 1
                Console.WriteLine("Index                                                : {0}", i)
                Console.WriteLine("Average Phase (deg)                                  : {0}",
                    pulseResultsAveragePhase(i))
                Console.WriteLine("Phase Deviation (deg)                                : {0}",
                    pulseResultsPhaseDeviation(i))
                Console.WriteLine("Phase Error RMS (deg)                                : {0}",
                    pulseResultsPhaseErrorRms(i))
                Console.WriteLine("--------------------------------------------------------------" & vbLf)
            Next

            Console.WriteLine(vbLf & "----------------------Frequency Results-----------------------" & vbLf)
            For i As Integer = 0 To pulseResultsAverageFrequency.Length - 1
                Console.WriteLine("Index                                                : {0}", i)
                Console.WriteLine("Average Frequency (Hz)                               : {0}",
                pulseResultsAverageFrequency(i))
                Console.WriteLine("Frequency Deviation (Hz)                             : {0}",
                pulseResultsFrequencyDeviation(i))
                Console.WriteLine("Frequency Error RMS (Hz)                             : {0}",
                pulseResultsFrequencyErrorRms(i))
                Console.WriteLine("--------------------------------------------------------------" & vbLf)
            Next

            Console.WriteLine(vbLf & vbLf & "---------------------FM Chirp Results-------------------------" & vbLf)
            For i As Integer = 0 To pulseResultsFMChirpRate.Length - 1
                Console.WriteLine("Index                                                : {0}", i)
                Console.WriteLine("Chrip Rate (Hz/us)                                   : {0:F11}",
            pulseResultsFMChirpRate(i))
                Console.WriteLine("Chirp Rate2 (Hz/us)                                  : {0:F11}",
            pulseResultsFMChirpRate2(i))
                Console.WriteLine("--------------------------------------------------------------" & vbLf)
            Next

            Console.WriteLine(vbLf & vbLf & "----------------Statistical Phase Results---------------------" & vbLf)
            Console.WriteLine("Average Phase Mean (deg)                             : {0}",
    pulseResultsAveragePhaseMean)
            Console.WriteLine("Average Phase Maximum (deg)                          : {0}",
    pulseResultsAveragePhaseMaximum)
            Console.WriteLine("Average Phase Minimum(deg)                           : {0}",
    pulseResultsAveragePhaseMinimum)
            Console.WriteLine("Average Phase Standard Deviation (deg)               : {0}",
    pulseResultsAveragePhaseSD)
            Console.WriteLine("Phase Deviation Mean (deg)                           : {0}",
    pulseResultsPhaseDeviationMean)
            Console.WriteLine("Phase Deviation Maximum (deg)                        : {0}",
    pulseResultsPhaseDeviationMaximum)
            Console.WriteLine("Phase Deviation Minimum(deg)                         : {0}",
    pulseResultsPhaseDeviationMinimum)
            Console.WriteLine("Phase Deviation Standard Deviation (deg)             : {0}",
    pulseResultsPhaseDeviationSD)
            Console.WriteLine("Phase Error RMS Mean (deg)                           : {0}",
    pulseResultsPhaseErrorRmsMean)
            Console.WriteLine("Phase Error RMS Maximum (deg)                        : {0}",
    pulseResultsPhaseErrorRmsMaximum)
            Console.WriteLine("Phase Error RMS Minimum(deg)                         : {0}",
    pulseResultsPhaseErrorRmsMinimum)
            Console.WriteLine("Phase Error RMS Standard Deviation (deg)             : {0}",
    pulseResultsPhaseErrorRmsSD)
            Console.WriteLine("--------------------------------------------------------------" & vbLf)

            Console.WriteLine(vbLf & vbLf & "--------------Statistical Frequency Results-------------------" & vbLf)
            Console.WriteLine("Average Frequency Mean (Hz)                          : {0}",
    pulseResultsAverageFrequencyMean)
            Console.WriteLine("Average Frequency Maximum (Hz)                       : {0}",
    pulseResultsAverageFrequencyMaximum)
            Console.WriteLine("Average Frequency Minimum(Hz)                        : {0}",
    pulseResultsAverageFrequencyMinimum)
            Console.WriteLine("Average Frequency Standard Deviation (Hz)            : {0}",
    pulseResultsAverageFrequencySD)
            Console.WriteLine("Frequency Deviation Mean (Hz)                        : {0}",
    pulseResultsFrequencyDeviationMean)
            Console.WriteLine("Frequency Deviation Maximum (Hz)                     : {0}",
    pulseResultsFrequencyDeviationMaximum)
            Console.WriteLine("Frequency Deviation Minimum(Hz)                      : {0}",
    pulseResultsFrequencyDeviationMinimum)
            Console.WriteLine("Frequency Deviation Standard Deviation (Hz)          : {0}",
    pulseResultsFrequencyDeviationSD)
            Console.WriteLine("Frequency Error RMS Mean (Hz)                        : {0}",
    pulseResultsFrequencyErrorRmsMean)
            Console.WriteLine("Frequency Error RMS Maximum (Hz)                     : {0}",
    pulseResultsFrequencyErrorRmsMaximum)
            Console.WriteLine("Frequency Error RMS Minimum(Hz)                      : {0}",
    pulseResultsFrequencyErrorRmsMinimum)
            Console.WriteLine("Frequency Error RMS Standard Deviation (Hz)          : {0}",
    pulseResultsFrequencyErrorRmsSD)
            Console.WriteLine("--------------------------------------------------------------" & vbLf)

            Console.WriteLine(vbLf & vbLf & "--------------Statistical FM Chirp Results--------------------" & vbLf)
            Console.WriteLine("Chirp Rate Mean (Hz/us)                             : {0}",
    pulseResultsFMChirpRateMean)
            Console.WriteLine("Chirp Rate Maximum (Hz/us)                          : {0}",
    pulseResultsFMChirpRateMaximum)
            Console.WriteLine("Chirp Rate Minimum(Hz/us)                           : {0}",
    pulseResultsFMChirpRateMinimum)
            Console.WriteLine("Chirp Rate Standard Deviation (Hz/us)               : {0}",
    pulseResultsFMChirpRateSD)
            Console.WriteLine("Chirp Rate 2 Mean (Hz/us)                            : {0}",
    pulseResultsFMChirpRate2Mean)
            Console.WriteLine("Chirp Rate 2 Maximum (Hz/us)                         : {0}",
    pulseResultsFMChirpRate2Maximum)
            Console.WriteLine("Chirp Rate 2 Minimum(Hz/us)                          : {0}",
    pulseResultsFMChirpRate2Minimum)
            Console.WriteLine("Chirp Rate 2 Standard Deviation (Hz/us)              : {0}",
    pulseResultsFMChirpRate2SD)
            Console.WriteLine("--------------------------------------------------------------" & vbLf)

        End Sub

        Private Sub CloseSession()
            If Pulse IsNot Nothing Then
                Pulse.Dispose()
                Pulse = Nothing
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
