'Steps:
'1. Open a New RFmx Session.
'2. Configure Frequency Reference.
'3. Configure LO Leakage Avoidance Enabled, LO source, Downconverter Frequency Offset (Hz), Frequency Settling unit And Frequency Settling duration..
'4. Configure basic RF signal properties (Center Frequency, RF Attenuation And External Attenuation)
'5. Enabling Pulse And Traces Result.
'6. Configure Trigger Type And Trigger Parameters.
'7. Configure Acquisition Settings.
'8. Configure Measurement Point Settings
'9. Configure Stability Settings.
'10. Configure Selected Traces setting And enabling Pulse Stability enabled And disable Pulse Metrics enabled settings.
'11. Configure Multiburst Settings.
'12. Initiate the Measurement.
'13. Wait for Measurement to complete.
'14. Results Average Stability, Per Pulse Stability Measurements, Pulse Indices, Burst Selected Position Stability Traces And Pulse to Pulse Stability Traces. 
'15. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.PulseMX

Namespace NationalInstruments.Examples.RFmxPulseMultiburstStabilityBasic
    Public Class RFmxPulseMultiburstStabilityBasic
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

        Private frequencySettlingUnits As RFmxInstrMXFrequencySettlingUnits
        Private frequencySettling As Double

        Private digitalEdgeEnabled As Boolean
        Private triggerDelay As Double
        Private digitalEdgeSource As String
        Private digitalEdge As RFmxPulseMXDigitalEdgeTriggerEdge

        Private measurementBandwidth As Double
        Private measurementFilterType As RFmxPulseMXMeasurementFilterType
        Private acquisitionLength As Double
        Private maximumPulseCountEnabled As RFmxPulseMXMaximumPulseCountEnabled
        Private maximumPulseCount As Integer

        Private pulseMultiburstEnabled As RFmxPulseMXMultiburstEnabled
        Private pulseBurstLength As Integer ' Pulses

        Private pulseMeasurementPointReference As RFmxPulseMXPulseMeasurementPointReference
        Private pulseMeasurementPointOffset As Double
        Private pulseMeasurementPointAveragingDuration As Double

        Private pulseStabilityMeasurmentOffset As Integer
        Private pulseStabilityReferenceOffset As Integer
        Private pulseStabilityPulseToPulseOffset As Integer
        Private pulseStabilityFrequencyErrorCompensation As RFmxPulseMXPulseStabilityFrequencyErrorCompensation

        Private pulseSelectedPulseTrace As Integer

        Private timeout As Double ' (s)

        Private averageAmplitudeStability As Double ' (dB)
        Private averagePhaseStability As Double ' (dB)
        Private averageTotalStability As Double ' (dB)

        Private amplitudeStability As Double() ' (dB)
        Private phaseStability As Double() ' (dB)
        Private totalStability As Double() ' (dB)

        Private burstIndex As Integer()
        Private pulsePositionIndex As Integer()

        Private pulseAmplitudeStability As AnalogWaveform(Of Single) ' (dB)
        Private pulsePhaseStability As AnalogWaveform(Of Single) ' (dB)
        Private pulseTotalStability As AnalogWaveform(Of Single) ' (dB)

        Private pulseIndex As Integer()
        Private pulseToPulseAmplitudeStability As Double() ' (dB)
        Private pulseToPulsePhaseStability As Double() ' (dB)
        Private pulseToPulseTotalStability As Double() ' (dB)

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

            centerFrequency = 1000000000.0 ' (Hz)
            referenceLevel = -10.0 ' (dBm)
            externalAttenuation = 0.0 ' (dB)

            downConverterFrequencyOffset = 0.0 ' (Hz)

            LOLeakageAvoidanceEnabled = RFmxInstrMXLOLeakageAvoidanceEnabled.True
            LOSource = RFmxInstrMXConstants.LOSourceOnboard

            frequencySettlingUnits = RFmxInstrMXFrequencySettlingUnits.Ppm
            frequencySettling = 0.1 ' (s)

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
            frequencyReferenceFrequency = 10000000.0 ' (Hz)

            triggerDelay = 0.0 ' (s)
            digitalEdgeEnabled = True
            digitalEdgeSource = RFmxInstrMXConstants.PxiTriggerLine0
            digitalEdge = RFmxPulseMXDigitalEdgeTriggerEdge.Rising

            measurementBandwidth = 80000000.0 '(Hz)
            measurementFilterType = RFmxPulseMXMeasurementFilterType.Gaussian
            acquisitionLength = 0.001 '(s)
            maximumPulseCountEnabled = RFmxPulseMXMaximumPulseCountEnabled.False
            maximumPulseCount = 100

            pulseMultiburstEnabled = RFmxPulseMXMultiburstEnabled.True
            pulseBurstLength = 10

            pulseMeasurementPointReference = RFmxPulseMXPulseMeasurementPointReference.Center
            pulseMeasurementPointOffset = 0.0 '(s)
            pulseMeasurementPointAveragingDuration = 0.0 '(s)

            pulseStabilityMeasurmentOffset = 0
            pulseStabilityReferenceOffset = 0
            pulseStabilityPulseToPulseOffset = 1
            pulseStabilityFrequencyErrorCompensation = RFmxPulseMXPulseStabilityFrequencyErrorCompensation.On

            pulseSelectedPulseTrace = 0

            timeout = 10.0 ' (s)
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
            instrSession.SetFrequencySettlingUnits("", frequencySettlingUnits)
            instrSession.SetFrequencySettling("", frequencySettling)
            Pulse.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
            Pulse.SelectMeasurements("", RFmxPulseMXMeasurementTypes.Pulse, True)
            Pulse.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, digitalEdgeEnabled)

            Pulse.SetMeasurementBandwidth("", measurementBandwidth)
            Pulse.SetMeasurementFilterType("", measurementFilterType)
            Pulse.SetAcquisitionLength("", acquisitionLength)
            Pulse.SetMaximumPulseCountEnabled("", maximumPulseCountEnabled)
            Pulse.SetMaximumPulseCount("", maximumPulseCount)

            Pulse.Pulse.Configuration.SetMeasurementPointReference("", pulseMeasurementPointReference)
            Pulse.Pulse.Configuration.SetMeasurementPointOffset("", pulseMeasurementPointOffset)
            Pulse.Pulse.Configuration.SetMeasurementPointAveragingDuration("", pulseMeasurementPointAveragingDuration)

            Pulse.Pulse.Configuration.SetStabilityMeasurementOffset("", pulseStabilityReferenceOffset)
            Pulse.Pulse.Configuration.SetStabilityReferenceOffset("", pulseStabilityReferenceOffset)
            Pulse.Pulse.Configuration.SetStabilityPulseToPulseOffset("", pulseStabilityPulseToPulseOffset)
            Pulse.Pulse.Configuration.SetStabilityFrequencyErrorCompensation("", pulseStabilityFrequencyErrorCompensation)
            Pulse.Pulse.Configuration.SetSelectedPulseTrace("", pulseSelectedPulseTrace)
            Pulse.Pulse.Configuration.SetMetricsEnabled("", RFmxPulseMXPulseMetricsEnabled.False)
            Pulse.Pulse.Configuration.SetStabilityEnabled("", RFmxPulseMXPulseStabilityEnabled.True)
            Pulse.Pulse.Configuration.SetMultiburstEnabled("", pulseMultiburstEnabled)
            Pulse.Pulse.Configuration.SetMultiburstLength("", pulseBurstLength)

            Pulse.Initiate("", "")

            Pulse.WaitForMeasurementComplete("", timeout)
        End Sub

            Private Sub RetrieveResults()
            Pulse.Pulse.Results.GetAverageAmplitudeStability("", averageAmplitudeStability)
            Pulse.Pulse.Results.GetAveragePhaseStability("", averagePhaseStability)
            Pulse.Pulse.Results.GetAverageTotalStability("", averageTotalStability)

            Pulse.Pulse.Results.GetAmplitudeStability("", amplitudeStability)
            Pulse.Pulse.Results.GetPhaseStability("", phaseStability)
            Pulse.Pulse.Results.GetTotalStability("", totalStability)

            Pulse.Pulse.Results.GetBurstIndex("", burstIndex)
            Pulse.Pulse.Results.GetPulsePositionIndex("", pulsePositionIndex)

            Pulse.Pulse.Results.FetchBurstSelectedPositionStabilityTrace("", timeout, pulseAmplitudeStability, pulsePhaseStability,
                pulseTotalStability)

            Pulse.Pulse.Results.FetchPulseToPulseStabilityTrace("", timeout, pulseIndex,
                pulseToPulseAmplitudeStability, pulseToPulsePhaseStability, pulseToPulseTotalStability)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine(vbLf & "------------- Average Stability Results -------------------" & vbLf)
            Console.WriteLine("Average Amplitude Stability (dB)     : {0}", averageAmplitudeStability)
            Console.WriteLine("Average Phase Stability (dB)         : {0}", averagePhaseStability)
            Console.WriteLine("Average Total Stability (dB)         : {0}" & vbLf, averageTotalStability & vbLf)

            Console.WriteLine("----------------- Stability Results -----------------------" & vbLf)
            For i As Integer = 0 To amplitudeStability.Length - 1
                Console.WriteLine("Index                                : {0}", i)
                Console.WriteLine("Burst Index                          : {0}", burstIndex.GetValue(i))
                Console.WriteLine("Pulse Position Index                 : {0}", pulsePositionIndex.GetValue(i))
                Console.WriteLine("Amplitude Stability (dB)             : {0}", amplitudeStability.GetValue(i))
                Console.WriteLine("Phase Stability (dB)                 : {0}", phaseStability.GetValue(i))
                Console.WriteLine("Total Stability (dB)                 : {0}", totalStability.GetValue(i))
                Console.WriteLine("-----------------------------------------------------------" & vbLf & vbLf)
            Next
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
