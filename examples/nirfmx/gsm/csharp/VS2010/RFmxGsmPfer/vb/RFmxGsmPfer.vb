'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Parameters for IQ Power Edge Trigger.
'5. Configure Number of Timeslots.
'6. Configure Signal Type.
'7. Configure Auto TSC Detection Enabled.
'8. Configure TSC.
'9. Select ModAcc measurement and enable Traces.
'10. Configure Averaging Parameters for ModAcc measurement.
'11. Initiate the Measurement.
'12  Fetch ModAcc Measurements and Traces.
'13. Close RFmx Session. 

Imports NationalInstruments.RFmx.GsmMX
Imports NationalInstruments.RFmx.InstrMX

Public Class RFmxGsmPferExample
	Private instrSession As RFmxInstrMX
	Private gsm As RFmxGsmMX
	Private resourceName As String

	Private frequencyReferenceFrequency As Double, centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, iqPowerEdgeLevel As Double, triggerDelay As Double, _
		minimumQuietTime As Double, timeout As Double, meanRmsPhaseError As Double, meanFrequencyError As Double, meanIQGainImbalance As Double, maximumIQGainImbalance As Double, _
		meanIQOriginOffset As Double, maximumIQOriginOffset As Double, maximumRmsPhaseError As Double, meanPeakPhaseError As Double, maximumPeakPhaseError As Double

	Private enableTrigger As Boolean
	Private numberOfTimeslots As Integer, averagingCount As Integer, peakSymbol As Integer
	Private frequencyReferenceSource As String
	Private detectedTsc As RFmxGsmMXModAccDetectedTsc()
	Private minimumQuietTimeMode As RFmxGsmMXTriggerMinimumQuietTimeMode
	Private autoTscDetectionEnabled As RFmxGsmMXAutoTscDetectionEnabled
	Private averagingEnabled As RFmxGsmMXModAccAveragingEnabled
	Private tsc As RFmxGsmMXTsc

	Private meanTraceError As AnalogWaveform(Of Single)

	Public Sub Run()
		Try
			InitializeVariables()
			InitializeInstr()
			ConfigureGsm()
			RetrieveResults()
			PrintResults()
		Catch ex As Exception
			DisplayError(ex)
		Finally
			' Close session 

			CloseSession()
			Console.WriteLine("Press any key to exit.....")
			Console.ReadKey()
		End Try
	End Sub

	Private Sub InitializeVariables()
		' Initialize input variables 

		resourceName = "RFSA"
		frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0              ' Hz 
        centerFrequency = 890200000.0                         ' Hz 
        referenceLevel = 0.0                                  ' dBm 
        externalAttenuation = 0.0                             ' dB 
		enableTrigger = True
		triggerDelay = 0.0
		iqPowerEdgeLevel = -20.0
		minimumQuietTimeMode = RFmxGsmMXTriggerMinimumQuietTimeMode.Auto
		minimumQuietTime = 0.000582
		numberOfTimeslots = 1
		averagingEnabled = RFmxGsmMXModAccAveragingEnabled.[False]
		averagingCount = 10
		autoTscDetectionEnabled = RFmxGsmMXAutoTscDetectionEnabled.[True]
		tsc = RFmxGsmMXTsc.Tsc0
		timeout = 10.0
	End Sub

	Private Sub InitializeInstr()
		' Create a new RFmx Session 

		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureGsm()
		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

		gsm = instrSession.GetGsmSignalConfiguration()
		gsm.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		gsm.ConfigureIQPowerEdgeTrigger("", "0", RFmxGsmMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, _
			minimumQuietTime, RFmxGsmMXIQPowerEdgeTriggerLevelType.Relative, enableTrigger)
		gsm.ConfigureNumberOfTimeslots("", numberOfTimeslots)
        gsm.ConfigureSignalType("slot::all", RFmxGsmMXModulationType.ModulationTypeGmsk, RFmxGsmMXBurstType.NB, RFmxGsmMXHBFilterWidth.Narrow)
		gsm.ConfigureAutoTscDetectionEnabled("", autoTscDetectionEnabled)
        gsm.ConfigureTsc("slot::all", tsc)
		gsm.SelectMeasurements("", RFmxGsmMXMeasurementTypes.ModAcc, True)
		gsm.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
		gsm.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		gsm.ModAcc.Results.FetchIQImpairments("", timeout, meanIQGainImbalance, maximumIQGainImbalance, meanIQOriginOffset, maximumIQOriginOffset)
		gsm.ModAcc.Results.FetchPfer("", timeout, meanRmsPhaseError, maximumRmsPhaseError, meanPeakPhaseError, maximumPeakPhaseError, _
			meanFrequencyError, peakSymbol)
		gsm.ModAcc.Results.FetchDetectedTscArray("", timeout, detectedTsc)
		gsm.ModAcc.Results.FetchPhaseErrorTrace("", timeout, meanTraceError)
	End Sub

	Private Sub PrintResults()
		Console.WriteLine("---------Measurement---------")
		Console.WriteLine("Mean RMS Phase Error (deg)     {0}", meanRmsPhaseError)
		Console.WriteLine("Maximum RMS Phase Error (deg)  {0}", maximumRmsPhaseError)
		Console.WriteLine("Mean Peak Phase Error (deg)    {0}", meanPeakPhaseError)
		Console.WriteLine("Maximum Peak Phase Error (deg) {0}", maximumPeakPhaseError)
		Console.WriteLine("Mean Frequency Error (Hz)      {0}", meanFrequencyError)
		Console.WriteLine("Peak Symbol                    {0}" & vbLf, peakSymbol)

		Console.WriteLine("---------IQ Impairments---------")
		Console.WriteLine("Maximum IQ Gain Imbalance (dB) {0}", maximumIQGainImbalance)
		Console.WriteLine("Maximum IQ Origin Offset (dB)  {0}", maximumIQOriginOffset)
		Console.WriteLine("Mean IQ Gain Imbalance (dB)    {0}", meanIQGainImbalance)
		Console.WriteLine("Mean IQ Origin Offset (dB)     {0}" & vbLf, meanIQOriginOffset)

		Console.WriteLine("---------Detected TSC-----------" & vbLf)
		For i As Integer = 0 To numberOfTimeslots - 1
				Console.WriteLine("Slot {0}                   {1}" & vbLf, i, detectedTsc(i))
		Next
	End Sub

	Private Sub CloseSession()
		Try
			If gsm IsNot Nothing Then
				gsm.Dispose()
				gsm = Nothing
			End If

			If instrSession IsNot Nothing Then
				instrSession.Close()
				instrSession = Nothing
			End If
		Catch ex As Exception
			DisplayError(ex)
		End Try
	End Sub

	Private Shared Sub DisplayError(ex As Exception)
		Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
	End Sub
End Class
