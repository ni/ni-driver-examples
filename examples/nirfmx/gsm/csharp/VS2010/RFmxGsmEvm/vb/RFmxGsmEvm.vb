'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Parameters for IQ Power Edge Trigger.
'5. Configure Number of Timeslots.
'6. Configure Auto TSC Detection Enabled.
'7. Configure Signal Type.
'8. Configure TSC.
'9. Select ModAcc measurement and enable Traces.
'10. Configure Averaging Parameters for ModAcc measurement.
'11. Initiate the Measurement.
'12 Fetch ModAcc Measurements and Traces.
'13. Close RFmx Session.

Imports NationalInstruments.RFmx.GsmMX
Imports NationalInstruments.RFmx.InstrMX

Public Class RFmxGsmEvmExample
	Private instrSession As RFmxInstrMX
	Private gsm As RFmxGsmMX
	Private resourceName As String

    Private frequencyReferenceFrequency As Double, centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, iqPowerEdgeLevel As Double, triggerDelay As Double, _
  minimumQuietTime As Double, timeout As Double, meanRmsEvm As Double, maximumRmsEvm As Double, meanPeakEvm As Double, maximumPeakEvm As Double, _
  ninetyFifthPercentileEvm As Double, meanFrequencyError As Double, meanIQGainImbalance As Double, maximumIQGainImbalance As Double, meanIQOriginOffset As Double, maximumIQOriginOffset As Double

	Private enableTrigger As Boolean
	Private frequencyReferenceSource As String
	Private numberOfTimeslots As Integer, averagingCount As Integer, peakEvmSymbol As Integer
	Private detectedTsc As RFmxGsmMXModAccDetectedTsc()
	Private evm As AnalogWaveform(Of Single)
	Private minimumQuietTimeMode As RFmxGsmMXTriggerMinimumQuietTimeMode
	Private autoTscDetectionEnabled As RFmxGsmMXAutoTscDetectionEnabled
	Private modulationType As RFmxGsmMXModulationType
	Private burstType As RFmxGsmMXBurstType
	Private hbFilterWidth As RFmxGsmMXHBFilterWidth
	Private averagingEnabled As RFmxGsmMXModAccAveragingEnabled
	Private tsc As RFmxGsmMXTsc

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
		frequencyReferenceFrequency = 10000000.0
		' Hz 
		centerFrequency = 890200000.0
		' Hz 
		referenceLevel = 0.0
		' dBm 
		externalAttenuation = 0.0
		' dB 
		iqPowerEdgeLevel = -20.0
		triggerDelay = 0.0
		minimumQuietTimeMode = RFmxGsmMXTriggerMinimumQuietTimeMode.Auto
		minimumQuietTime = 0.000582
		enableTrigger = True
		numberOfTimeslots = 1
		autoTscDetectionEnabled = RFmxGsmMXAutoTscDetectionEnabled.[True]
		modulationType = RFmxGsmMXModulationType.ModulationType8Psk
		burstType = RFmxGsmMXBurstType.NB
		hbFilterWidth = RFmxGsmMXHBFilterWidth.Narrow
		tsc = RFmxGsmMXTsc.Tsc0
		averagingEnabled = RFmxGsmMXModAccAveragingEnabled.[False]
		averagingCount = 10
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
        gsm.ConfigureAutoTscDetectionEnabled("", autoTscDetectionEnabled)
        gsm.ConfigureSignalType("slot::all", modulationType, burstType, hbFilterWidth)
        gsm.ConfigureTsc("slot::all", tsc)
		gsm.SelectMeasurements("", RFmxGsmMXMeasurementTypes.ModAcc, True)
		gsm.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
		gsm.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		' Fetch Results 

		gsm.ModAcc.Results.FetchEvm("", timeout, meanRmsEvm, maximumRmsEvm, meanPeakEvm, maximumPeakEvm, _
			ninetyFifthPercentileEvm, meanFrequencyError, peakEvmSymbol)
		gsm.ModAcc.Results.FetchIQImpairments("", timeout, meanIQGainImbalance, maximumIQGainImbalance, meanIQOriginOffset, maximumIQOriginOffset)
		gsm.ModAcc.Results.FetchDetectedTscArray("", timeout, detectedTsc)
		gsm.ModAcc.Results.FetchEvmTrace("", timeout, evm)
	End Sub

	Private Sub PrintResults()
		Console.WriteLine("-----------------Measurement-----------------")
        Console.WriteLine("Mean RMS EVM (%)              {0}", meanRmsEvm)
        Console.WriteLine("Maximum RMS EVM (%)           {0}", maximumRmsEvm)
        Console.WriteLine("Mean Peak EVM (%)             {0}", meanPeakEvm)
        Console.WriteLine("Maximum Peak EVM (%)          {0}", maximumPeakEvm)
        Console.WriteLine("95th Percentile EVM (%)       {0}", ninetyFifthPercentileEvm)
		Console.WriteLine("Mean Frequency Error (Hz)     {0}", meanFrequencyError)
        Console.WriteLine("Peak EVM Symbol               {0}" & vbLf, peakEvmSymbol)

		Console.WriteLine("----------------IQ Impairments-----------------")
        Console.WriteLine("Mean IQ Origin Offset (dB)        {0}", meanIQOriginOffset)
        Console.WriteLine("Maximum IQ Origin Offset (dB)     {0}", maximumIQOriginOffset)
        Console.WriteLine("Mean IQ Gain Imbalance (dB)       {0}", meanIQGainImbalance)
        Console.WriteLine("Maximum IQ Gain Imbalance (dB)    {0}" & vbLf, maximumIQGainImbalance)

		Console.WriteLine("----------------Detected TSC------------------")
		For i As Integer = 0 To detectedTsc.Length - 1
			Console.WriteLine("Slot {0}                : {1}", i, detectedTsc(i))
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
