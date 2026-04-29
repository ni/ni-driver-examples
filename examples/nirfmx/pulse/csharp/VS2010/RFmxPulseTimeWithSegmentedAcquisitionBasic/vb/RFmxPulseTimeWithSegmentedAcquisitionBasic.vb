'Steps:
'1. Open a New RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic RF signal properties(Center Frequency, RF Attenuation And External Attenuation)
'4. Enabling Pulse And Disabling Traces Result
'5. Configure Trigger Type And Trigger Parameters.
'6. Configure Bandwidth And Filter Type.
'7. Configure Segmented Acquisition Settings.
'8. Configure Pulse Detection Settings.
'9. Configure State And Thershold Level Settings.
'10. Configure Pulse Metrics And Pulse Stability enabled settings.
'11. Initiate the Measurement.
'12. Wait for Measurement to complete.
'13. Fetch Pulse Count And Timing Results.
'14. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.PulseMX

Namespace NationalInstruments.Examples.RFmxPulseTimeWithSegmentedAcquisitionBasic
   Public Class RFmxPulseTimeWithSegmentedAcquisitionBasic
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
      Private segmentAcquisitionLength As Double
      Private segmentAcquisitionLengthEnabled As RFmxPulseMXSegmentedAcquisitionEnabled
      Private numberOfSegments As Integer

      Private pulseDetectionReference As RFmxPulseMXPulseDetectionReference
      Private pulseDetectionThreshold As Double
      Private pulseDetectionHysteresis As Double
      Private pulseDetectionMinimumOffDuration As Double

      Private pulseLevelComputationMethod As RFmxPulseMXPulseLevelComputationMethod
      Private pulseAmplitudeLevelDomain As RFmxPulseMXPulseAmplitudeLevelDomain
      Private pulseUpperThresholdLevel As Double
      Private pulseWidthThresholdLevel As Double
      Private pulseLowerThresholdLevel As Double

      Private pulseSelectedPulseTrace As Integer

      Private timeout As Double
      ' (s)

      Private pulseCount As Integer

      Private pulseResultsRiseTime As Double()
      ' (s)
      Private pulseResultsFallTime As Double()
      ' (s)
      Private pulseResultsPulseWidth As Double()
      ' (s)
      Private pulseResultsPulseOffDuration As Double()
      ' (s)
      Private pulseResultsDutyCycle As Double()
      ' (%)
      Private pulseResultsPulseRepetitionInterval As Double()
      ' (s)



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
         segmentAcquisitionLength = 0.000015
         '(s)
         segmentAcquisitionLengthEnabled = RFmxPulseMXSegmentedAcquisitionEnabled.True
         numberOfSegments = 100

         pulseDetectionReference = RFmxPulseMXPulseDetectionReference.ReferenceLevel
         pulseDetectionThreshold = -20.0
         pulseDetectionHysteresis = 1.0
         '(dB)
         pulseDetectionMinimumOffDuration = 0.00000005
         '(s)

         pulseLevelComputationMethod = RFmxPulseMXPulseLevelComputationMethod.Median
         pulseAmplitudeLevelDomain = RFmxPulseMXPulseAmplitudeLevelDomain.Volts
         pulseUpperThresholdLevel = 90.0
         '(%)
         pulseWidthThresholdLevel = 50.0
         '(%)
         pulseLowerThresholdLevel = 10.0
         '(%)

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
         Pulse.SetSegmentedAcquisitionEnabled("", segmentAcquisitionLengthEnabled)
         Pulse.SetNumberOfSegments("", numberOfSegments)
         Pulse.SetAcquisitionLength("", segmentAcquisitionLength)

         Pulse.Pulse.Configuration.SetDetectionReference("", pulseDetectionReference)
         Pulse.Pulse.Configuration.SetDetectionThreshold("", pulseDetectionThreshold)
         Pulse.Pulse.Configuration.SetDetectionHysteresis("", pulseDetectionHysteresis)
         Pulse.Pulse.Configuration.SetDetectionMinimumOffDuration("", pulseDetectionMinimumOffDuration)

         Pulse.Pulse.Configuration.SetLevelComputationMethod("", pulseLevelComputationMethod)
         Pulse.Pulse.Configuration.SetAmplitudeLevelDomain("", pulseAmplitudeLevelDomain)
         Pulse.Pulse.Configuration.SetUpperThresholdLevel("", pulseUpperThresholdLevel)
         Pulse.Pulse.Configuration.SetWidthThresholdLevel("", pulseWidthThresholdLevel)
         Pulse.Pulse.Configuration.SetLowerThresholdLevel("", pulseLowerThresholdLevel)

         Pulse.Pulse.Configuration.SetMetricsEnabled("", RFmxPulseMXPulseMetricsEnabled.True)
         Pulse.Pulse.Configuration.SetStabilityEnabled("", RFmxPulseMXPulseStabilityEnabled.False)

         Pulse.Initiate("", "")

         Pulse.WaitForMeasurementComplete("", timeout)
      End Sub

      Private Sub RetrieveResults()
         Pulse.Pulse.Results.GetPulseCount("", pulseCount)

         Pulse.Pulse.Results.GetRiseTime("", pulseResultsRiseTime)
         Pulse.Pulse.Results.GetFallTime("", pulseResultsFallTime)
         Pulse.Pulse.Results.GetPulseWidth("", pulseResultsPulseWidth)
         Pulse.Pulse.Results.GetPulseOffDuration("", pulseResultsPulseOffDuration)
         Pulse.Pulse.Results.GetDutyCycle("", pulseResultsDutyCycle)
         Pulse.Pulse.Results.GetPulseRepetitionInterval("", pulseResultsPulseRepetitionInterval)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine(vbLf & "Pulse Count                            : {0}", pulseCount)
         Console.WriteLine(vbLf & vbLf & "------------------- Timing Results ----------------------" & vbLf)
         For i As Integer = 0 To pulseResultsRiseTime.Length - 1
            Console.WriteLine("Index                                  : {0}", i)
            Console.WriteLine("Rise Time (s)                          : {0:F11}", pulseResultsRiseTime(i))
            Console.WriteLine("Fall Time (s)                          : {0:F11}", pulseResultsFallTime(i))
            Console.WriteLine("Pulse Width (s)                        : {0:F9}", pulseResultsPulseWidth(i))
            Console.WriteLine("Pulse Off Duration (s)                 : {0:F9}", pulseResultsPulseOffDuration(i))
            Console.WriteLine("Duty Cycle (%%)                        : {0:F9}", pulseResultsDutyCycle(i))
            Console.WriteLine("Pulse repetition Interval (dB)         : {0:F9}", pulseResultsPulseRepetitionInterval(i))
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
