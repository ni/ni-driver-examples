'Steps:
'1. Open a New RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic RF signal properties(Center Frequency, RF Attenuation And External Attenuation)
'4. Enabling Pulse And Traces Result
'5. Configure Trigger Type And Trigger Parameters.
'6. Configure Acquisition Settings.
'7. Configure Pulse Detection Settings.
'8. Configure State And Thershold Level Settings.
'9. Configure Selected Traces Settings And constant control, Pulse Metrics And Pulse Stability enabled settings.
'10. Initiate the Measurement.
'11. Wait for Measurement to complete.
'12. Fetch  Pulse Count, Timing, Amplitude Measurements And Amplitude Traces.
'13. Close RFmx Session. 

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.PulseMX

Namespace NationalInstruments.Examples.RFmxPulseTimeAndPowerBasic
    Public Class RFmxPulseTimeAndPowerBasic
        Private instrSession As RFmxInstrMX
        Private Pulse As RFmxPulseMX
        Private resourceName As String

        Private centerFrequency As Double
        Private referenceLevel As Double
        Private externalAttenuation As Double

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

        Private pulseLevelComputationMethod As RFmxPulseMXPulseLevelComputationMethod
        Private pulseDroopCompensationEnabled As RFmxPulseMXPulseDroopCompensationEnabled

        Private pulseSelectedPulseTrace As Integer
        Private pulseAmplitudeTraceUnit As RFmxPulseMXPulseAmplitudeTraceUnit

        Private timeout As Double
        ' (s)

        Private pulseCount As Integer

        Private pulseResultsRiseTime As Double()
        ' (s)
        Private pulseResultsFallTime As Double()
        ' (s)
        Private pulseResultsPulseWidth As Double()
        ' (s)
        Private pulseResultsPulseRepetitionInterval As Double()
        ' (s)

        Private pulseResultsTopLevel As Double()
        ' (dBm)
        Private pulseResultsBaseLevel As Double()
        ' (dBm)
        Private pulseresultsAverageOnLevel As Double()
        ' (dBm)
        Private pulseResultsOvershoot As Double()
        ' (%)
        Private pulseResultsDroop As Double()
        ' (%)
        Private pulseResultsRipple As Double()
        ' (%)

        Private amplitude As AnalogWaveform(Of Single)
        ' (dB)

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

            pulseDetectionReference = RFmxPulseMXPulseDetectionReference.ReferenceLevel
            pulseDetectionThreshold = -20.0
            pulseDetectionHysteresis = 1.0
            '(dB)
            pulseDetectionMinimumOffDuration = 0.00000005
            '(s)

            pulseLevelComputationMethod = RFmxPulseMXPulseLevelComputationMethod.Median
            pulseDroopCompensationEnabled = RFmxPulseMXPulseDroopCompensationEnabled.True

            pulseSelectedPulseTrace = 0
            pulseAmplitudeTraceUnit = RFmxPulseMXPulseAmplitudeTraceUnit.dBm

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
            Pulse.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
            Pulse.SelectMeasurements("", RFmxPulseMXMeasurementTypes.Pulse, True)
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
            Pulse.Pulse.Configuration.SetDroopCompensationEnabled("", pulseDroopCompensationEnabled)

            Pulse.Pulse.Configuration.SetMetricsEnabled("", RFmxPulseMXPulseMetricsEnabled.True)
            Pulse.Pulse.Configuration.SetStabilityEnabled("", RFmxPulseMXPulseStabilityEnabled.False)
            Pulse.Pulse.Configuration.SetSelectedPulseTrace("", pulseSelectedPulseTrace)
            Pulse.Pulse.Configuration.SetAmplitudeTraceUnit("", pulseAmplitudeTraceUnit)

            Pulse.Initiate("", "")

            Pulse.WaitForMeasurementComplete("", timeout)
        End Sub

        Private Sub RetrieveResults()
            Pulse.Pulse.Results.GetPulseCount("", pulseCount)

            Pulse.Pulse.Results.GetRiseTime("", pulseResultsRiseTime)
            Pulse.Pulse.Results.GetFallTime("", pulseResultsFallTime)
            Pulse.Pulse.Results.GetPulseWidth("", pulseResultsPulseWidth)
            Pulse.Pulse.Results.GetPulseRepetitionInterval("", pulseResultsPulseRepetitionInterval)

            Pulse.Pulse.Results.GetTopLevel("", pulseResultsTopLevel)
            Pulse.Pulse.Results.GetBaseLevel("", pulseResultsBaseLevel)
            Pulse.Pulse.Results.GetAverageOnLevel("", pulseresultsAverageOnLevel)
            Pulse.Pulse.Results.GetOvershoot("", pulseResultsOvershoot)
            Pulse.Pulse.Results.GetDroop("", pulseResultsDroop)
            Pulse.Pulse.Results.GetRipple("", pulseResultsRipple)

            Pulse.Pulse.Results.FetchAmplitudeTrace("", timeout, amplitude)
        End Sub

        Private Sub PrintResults()
            Console.WriteLine(vbLf & "Pulse Count                            : {0}", pulseCount)
            Console.WriteLine(vbLf & vbLf & "------------------- Timing Results ----------------------" & vbLf)
            For i As Integer = 0 To pulseResultsRiseTime.Length - 1
                Console.WriteLine("Index                                  : {0}", i)
                Console.WriteLine("Rise Time (dB)                         : {0:F11}", pulseResultsRiseTime(i))
                Console.WriteLine("Fall Time (dB)                         : {0:F11}", pulseResultsFallTime(i))
                Console.WriteLine("Pulse Width (dB)                       : {0:F9}", pulseResultsPulseWidth(i))
                Console.WriteLine("Pulse repetition Interval (dB)         : {0:F9}", pulseResultsPulseRepetitionInterval(i))
                Console.WriteLine("---------------------------------------------------------" & vbLf)
            Next

            Console.WriteLine(vbLf & vbLf & "------------------ Level Results ------------------------" & vbLf)
            For i As Integer = 0 To pulseResultsTopLevel.Length - 1
                Console.WriteLine("Index                                  : {0}", i)
                Console.WriteLine("Top Level (dBm)                        : {0}", pulseResultsTopLevel(i))
                Console.WriteLine("Base Level (dBm)                       : {0}", pulseResultsBaseLevel(i))
                Console.WriteLine("Average On Level (dBm)                 : {0}", pulseresultsAverageOnLevel(i))
                Console.WriteLine("Overshoot (%)                          : {0}", pulseResultsOvershoot(i))
                Console.WriteLine("Droop (%)                              : {0}", pulseResultsDroop(i))
                Console.WriteLine("Ripple (%)                             : {0}", pulseResultsRipple(i))
                Console.WriteLine("---------------------------------------------------------" & vbLf)
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
