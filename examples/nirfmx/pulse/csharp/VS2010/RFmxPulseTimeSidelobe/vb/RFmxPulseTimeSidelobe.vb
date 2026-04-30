' Steps:
' 1. Open a new RFmx Session.
' 2. Configure Frequency Reference.
' 3. Configure basic RF signal properties (Center Frequency, RF Attenuation and External Attenuation)
' 4. Enabling Pulse and Traces Result
' 5. Configure Trigger Type and Trigger Parameters.
' 6. Configure Reference Waveform.
' 7. Configure Acquisition Settings.
' 8. Configure Pulse Detection Settings.
' 9. Configure State and Thershold Level Settings.
' 10. Configure Time Sidelobe Settings.
' 11. Configure Selected Traces Settings, Pulse Metrics and Pulse Stability enabled settings.
' 12. Initiate the Measurement.
' 13. Wait for Measurement to complete.
' 14  Fetch  Pulse Count, Time Sidelobe, Statistical Time Sidelobe results and Time Sidelobe Trace.
' 15. Close RFmx Session.

Imports System
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.PulseMX
Imports NationalInstruments.ModularInstruments.NIRfsgPlayback

Namespace NationalInstruments.Examples.RFmxPulseTimeSidelobe
 Public Class RFmxPulseTimeSidelobe
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
  Private pulseDetectionMinimumPulseOffDuration As Double

  Private pulseLevelComputationMethod As RFmxPulseMXPulseLevelComputationMethod
  Private pulseDroopCompensationEnabled As RFmxPulseMXPulseDroopCompensationEnabled

  Private timeSidelobeReferenceWindowType As RFmxPulseMXPulseTimeSidelobeReferenceWindowType
  Private timeSidelobeKeepOutTimeAuto As RFmxPulseMXPulseTimeSidelobeKeepOutTimeAuto
  Private timeSidelobeKeepOutTime As Double                                           ' (s) 
  Private timeSidelobeMinimumCorrelation As Double
  Private timeout As Double                                                          ' (s) 

  Private pulseSelectedPulseTrace As Integer

  Private referenceWaveformFilePath As String
  Private referenceWaveform As ComplexWaveform(Of ComplexSingle)

  ' Time Sidelobe Results 
  Private pulseCount As Integer
  Private pulseResultsMainlobeWidth As Double()                                      ' (s) 
  Private pulseResultsSidelobeDelay As Double()                                      ' (s) 
  Private pulseResultsPeakSidelobeLevel As Double()                                 ' (dB) 
  Private pulseResultsCompressionRatio As Double()                                   ' (%) 
  Private pulseResultsPeakCorrelation As Double()

  ' Statistical Time Sidelobe Results
  Private pulseResultsMainlobeWidthMean As Double                                    ' (s) 
  Private pulseResultsMainlobeWidthMaximum As Double                                 ' (s) 
  Private pulseResultsMainlobeWidthMinimum As Double                                 ' (s) 
  Private pulseResultsMainlobeWidthStandardDeviation As Double                       ' (s) 
  Private pulseResultSidelobesDelayMean As Double                                    ' (s) 
  Private pulseResultsSidelobeDelayMaximum As Double                                 ' (s) 
  Private pulseResultsSidelobeDelayMinimum As Double                                 ' (s) 
  Private pulseResultsSidelobeDelayStandardDeviation As Double                       ' (s) 
  Private pulseResultsPeakSidelobeLevelMean As Double                                ' (dB) 
  Private pulseResultsPeakSidelobeLevelMaximum As Double                             ' (dB) 
  Private pulseResultsPeakSidelobeLevelMinimum As Double                             ' (dB) 
  Private pulseResultsPeakSidelobeLevelStandardDeviation As Double                   ' (dB) 
  Private pulseResultsSidelobeCompressionRatioMean As Double                         ' (%) 
  Private pulseResultsSidelobeCompressionRatioMaximum As Double                      ' (%) 
  Private pulseResultsSidelobeCompressionRatioMinimum As Double                      ' (%) 
  Private pulseResultsSidelobeCompressionRatioStandardDeviation As Double            ' (%) 
  Private pulseResultsSidelobePeakCorrelationMean As Double                          ' (dB) 
  Private pulseResultsSidelobePeakCorrelationMaximum As Double
  Private pulseResultsSidelobePeakCorrelationMinimum As Double
  Private pulseResultsSidelobePeakCorrelationStandardDeviation As Double
  Private pulseTimeSidelobeTrace As AnalogWaveform(Of Single)

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

   centerFrequency = 1000000000.0                                          ' (Hz) 
   referenceLevel = -10.0                                           ' (dBm) 
   externalAttenuation = 0.0                                        ' (dB) 

   frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
   frequencyReferenceFrequency = 10000000.0                             ' (Hz) 

   iqPowerEdgeEnabled = True
   iqPowerEdgeLevel = -20.0                                         ' (dBm) 
   triggerDelay = 0.0                                               ' (s) 
   minimumQuietTimeMode = RFmxPulseMXTriggerMinimumQuietTimeMode.Auto
   minimumQuietTime = 0.000005                                        ' (s) 

   measurementBandwidth = 80000000.0                                    ' (Hz) 
   measurementFilterType = RFmxPulseMXMeasurementFilterType.Rectangular
   acquisitionLength = 0.001                                       ' (s) 
   maximumPulseCountEnabled = RFmxPulseMXMaximumPulseCountEnabled.False
   maximumPulseCount = 100

   pulseDetectionReference = RFmxPulseMXPulseDetectionReference.ReferenceLevel
   pulseDetectionThreshold = -20.0
   pulseDetectionHysteresis = 1.0                                   ' (dB) 
   pulseDetectionMinimumPulseOffDuration = 0.00000005                  ' (s) 

   pulseLevelComputationMethod = RFmxPulseMXPulseLevelComputationMethod.Median
   pulseDroopCompensationEnabled = RFmxPulseMXPulseDroopCompensationEnabled.True

   timeSidelobeReferenceWindowType = RFmxPulseMXPulseTimeSidelobeReferenceWindowType.None
   timeSidelobeKeepOutTimeAuto = RFmxPulseMXPulseTimeSidelobeKeepOutTimeAuto.True
   timeSidelobeKeepOutTime = 0.000001                                ' (s) 
   timeSidelobeMinimumCorrelation = 0.5

   referenceWaveformFilePath = "Pulse_FMChirpUp-10MHz_BW-80MHz_Rect-filter.tdms"
   referenceWaveform = Nothing

   pulseSelectedPulseTrace = 0

   timeout = 10.0                                                ' (s) 
  End Sub

  Private Sub InitializeInstr()
   instrSession = New RFmxInstrMX(resourceName, "")
  End Sub

  Private Sub ConfigurePulse()
   Pulse = instrSession.GetPulseSignalConfiguration()                ' Create a new RFmx Session 
   instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
   Pulse.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
   Pulse.SelectMeasurements("", RFmxPulseMXMeasurementTypes.Pulse, True)
   Pulse.ConfigureIQPowerEdgeTrigger("", "0", RFmxPulseMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxPulseMXIQPowerEdgeTriggerLevelType.Relative, iqPowerEdgeEnabled)
   NIRfsgPlayback.ReadWaveformFromFileComplex(referenceWaveformFilePath, referenceWaveform)

   Pulse.Pulse.Configuration.Configure1ReferenceWaveform("", referenceWaveform)

   Pulse.SetMeasurementBandwidth("", measurementBandwidth)
   Pulse.SetMeasurementFilterType("", measurementFilterType)
   Pulse.SetAcquisitionLength("", acquisitionLength)
   Pulse.SetMaximumPulseCountEnabled("", maximumPulseCountEnabled)
   Pulse.SetMaximumPulseCount("", maximumPulseCount)

   Pulse.Pulse.Configuration.SetDetectionReference("", pulseDetectionReference)
   Pulse.Pulse.Configuration.SetDetectionThreshold("", pulseDetectionThreshold)
   Pulse.Pulse.Configuration.SetDetectionHysteresis("", pulseDetectionHysteresis)
   Pulse.Pulse.Configuration.SetDetectionMinimumOffDuration("", pulseDetectionMinimumPulseOffDuration)

   Pulse.Pulse.Configuration.SetLevelComputationMethod("", pulseLevelComputationMethod)
   Pulse.Pulse.Configuration.SetDroopCompensationEnabled("", pulseDroopCompensationEnabled)
   Pulse.Pulse.Configuration.SetTimeSidelobeEnabled("", RFmxPulseMXPulseTimeSidelobeEnabled.[True])
   Pulse.Pulse.Configuration.SetTimeSidelobeReferenceWindowType("", timeSidelobeReferenceWindowType)
   Pulse.Pulse.Configuration.SetTimeSidelobeKeepOutTimeAuto("", timeSidelobeKeepOutTimeAuto)
   Pulse.Pulse.Configuration.SetTimeSidelobeKeepOutTime("", timeSidelobeKeepOutTime)
   Pulse.Pulse.Configuration.SetTimeSidelobeMinimumCorrelation("", timeSidelobeMinimumCorrelation)
   Pulse.Pulse.Configuration.SetMetricsEnabled("", RFmxPulseMXPulseMetricsEnabled.[False])
   Pulse.Pulse.Configuration.SetStabilityEnabled("", RFmxPulseMXPulseStabilityEnabled.[False])
   Pulse.Pulse.Configuration.SetSelectedPulseTrace("", pulseSelectedPulseTrace)

   Pulse.Initiate("", "")

   Pulse.WaitForMeasurementComplete("", timeout)
  End Sub

  Private Sub RetrieveResults()
   Pulse.Pulse.Results.GetPulseCount("", pulseCount)
   Pulse.Pulse.Results.GetTimeSidelobeMainlobeWidth("", pulseResultsMainlobeWidth)
   Pulse.Pulse.Results.GetTimeSidelobeDelay("", pulseResultsSidelobeDelay)
   Pulse.Pulse.Results.GetTimeSidelobePeakSidelobeLevel("", pulseResultsPeakSidelobeLevel)
   Pulse.Pulse.Results.GetTimeSidelobeCompressionRatio("", pulseResultsCompressionRatio)
   Pulse.Pulse.Results.GetTimeSidelobePeakCorrelation("", pulseResultsPeakCorrelation)
   Pulse.Pulse.Results.GetTimeSidelobeMainlobeWidthMean("", pulseResultsMainlobeWidthMean)
   Pulse.Pulse.Results.GetTimeSidelobeMainlobeWidthMaximum("", pulseResultsMainlobeWidthMaximum)
   Pulse.Pulse.Results.GetTimeSidelobeMainlobeWidthMinimum("", pulseResultsMainlobeWidthMinimum)
   Pulse.Pulse.Results.GetTimeSidelobeMainlobeWidthStandardDeviation("", pulseResultsMainlobeWidthStandardDeviation)
   Pulse.Pulse.Results.GetTimeSidelobeDelayMean("", pulseResultSidelobesDelayMean)
   Pulse.Pulse.Results.GetTimeSidelobeDelayMaximum("", pulseResultsSidelobeDelayMaximum)
   Pulse.Pulse.Results.GetTimeSidelobeDelayMinimum("", pulseResultsSidelobeDelayMinimum)
   Pulse.Pulse.Results.GetTimeSidelobeDelayStandardDeviation("", pulseResultsSidelobeDelayStandardDeviation)
   Pulse.Pulse.Results.GetTimeSidelobePeakSidelobeLevelMean("", pulseResultsPeakSidelobeLevelMean)
   Pulse.Pulse.Results.GetTimeSidelobePeakSidelobeLevelMaximum("", pulseResultsPeakSidelobeLevelMaximum)
   Pulse.Pulse.Results.GetTimeSidelobePeakSidelobeLevelMinimum("", pulseResultsPeakSidelobeLevelMinimum)
   Pulse.Pulse.Results.GetTimeSidelobePeakSidelobeLevelStandardDeviation("", pulseResultsPeakSidelobeLevelStandardDeviation)
   Pulse.Pulse.Results.GetTimeSidelobeCompressionRatioMean("", pulseResultsSidelobeCompressionRatioMean)
   Pulse.Pulse.Results.GetTimeSidelobeCompressionRatioMaximum("", pulseResultsSidelobeCompressionRatioMaximum)
   Pulse.Pulse.Results.GetTimeSidelobeCompressionRatioMinimum("", pulseResultsSidelobeCompressionRatioMinimum)
   Pulse.Pulse.Results.GetTimeSidelobeCompressionRatioStandardDeviation("", pulseResultsSidelobeCompressionRatioStandardDeviation)
   Pulse.Pulse.Results.GetTimeSidelobePeakCorrelationMean("", pulseResultsSidelobePeakCorrelationMean)
   Pulse.Pulse.Results.GetTimeSidelobePeakCorrelationMaximum("", pulseResultsSidelobePeakCorrelationMaximum)
   Pulse.Pulse.Results.GetTimeSidelobePeakCorrelationMinimum("", pulseResultsSidelobePeakCorrelationMinimum)
   Pulse.Pulse.Results.GetTimeSidelobePeakCorrelationStandardDeviation("", pulseResultsSidelobePeakCorrelationStandardDeviation)
   Pulse.Pulse.Results.FetchTimeSidelobeTrace("", timeout, pulseTimeSidelobeTrace)

  End Sub

  Private Sub PrintResults()
   Console.WriteLine(vbLf & "Pulse Count                            : {0}" & vbLf, pulseCount)
   Console.WriteLine("---------------------Time Sidelobe Results------------------------------------------")
   For i As Integer = 0 To pulseCount - 1

    Console.WriteLine("Index                                      : {0}", i)
    Console.WriteLine("Mainlobe Width (s)                         : {0:F11}", pulseResultsMainlobeWidth(i))
    Console.WriteLine("Sidelobe Delay (s)                         : {0:F11}", pulseResultsSidelobeDelay(i))
    Console.WriteLine("Peak Sidelobe Level (dB)                   : {0:F9}", pulseResultsPeakSidelobeLevel(i))
    Console.WriteLine("Compression Ratio (%)                      : {0:F9}", pulseResultsCompressionRatio(i))
    Console.WriteLine("Peak Correlation                           : {0:F9}", pulseResultsPeakCorrelation(i))
    Console.WriteLine("-----------------------------------------------------------------------------------" & vbLf)
   Next
   Console.WriteLine("--------------------Statistical Time Sidelobe Results-------------------------------")
   Console.WriteLine("Mainlobe Width Mean (s)                        : {0}", pulseResultsMainlobeWidthMean)
   Console.WriteLine("Mainlobe Width Max (s)                         : {0}", pulseResultsMainlobeWidthMaximum)
   Console.WriteLine("Mainlobe Width Min (s)                         : {0}", pulseResultsMainlobeWidthMinimum)
   Console.WriteLine("Mainlobe Width SD (s)                          : {0}", pulseResultsMainlobeWidthStandardDeviation)
   Console.WriteLine("Sidelobe Delay Mean (s)                        : {0}", pulseResultSidelobesDelayMean)
   Console.WriteLine("Sidelobe Delay Max (s)                         : {0}", pulseResultsSidelobeDelayMaximum)
   Console.WriteLine("Sidelobe Delay Min (s)                         : {0}", pulseResultsSidelobeDelayMinimum)
   Console.WriteLine("Sidelobe Delay SD (s)                          : {0}", pulseResultsSidelobeDelayStandardDeviation)
   Console.WriteLine("Peak Sidelobe Level Mean (dB)                  : {0}", pulseResultsPeakSidelobeLevelMean)
   Console.WriteLine("Peak Sidelobe Level Max (dB)                   : {0}", pulseResultsPeakSidelobeLevelMaximum)
   Console.WriteLine("Peak Sidelobe Level Min (dB)                   : {0}", pulseResultsPeakSidelobeLevelMinimum)
   Console.WriteLine("Peak Sidelobe Level SD (dB)                    : {0}", pulseResultsPeakSidelobeLevelStandardDeviation)
   Console.WriteLine("Compression Ratio Mean (%)                     : {0}", pulseResultsSidelobeCompressionRatioMean)
   Console.WriteLine("Compression Ratio Max (%)                      : {0}", pulseResultsSidelobeCompressionRatioMaximum)
   Console.WriteLine("Compression Ratio Min (%)                      : {0}", pulseResultsSidelobeCompressionRatioMinimum)
   Console.WriteLine("Compression Ratio SD (%)                       : {0}", pulseResultsSidelobeCompressionRatioStandardDeviation)
   Console.WriteLine("Peak Correlation Mean                          : {0}", pulseResultsSidelobePeakCorrelationMean)
   Console.WriteLine("Peak Correlation Max                           : {0}", pulseResultsSidelobePeakCorrelationMaximum)
   Console.WriteLine("Peak Correlation Min                           : {0}", pulseResultsSidelobePeakCorrelationMinimum)
   Console.WriteLine("Peak Correlation SD                            : {0}", pulseResultsSidelobePeakCorrelationStandardDeviation)
   Console.WriteLine("------------------------------------------------------------------------------------" & vbLf)

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
