'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties: Clock Source and Clock Frequency
'3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation
'4. Configure the trigger properties
'5. Select OBW measurement and enable the traces
'6. Configure Sweep Time for the OBW measurement
'7. Configure Averaging Parameters for the OBW measurement
'8. Initiate Measurement
'9. Fetch OBW Measurements and Traces
'10. Close the RFmx session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.TdscdmaMX

Public Class RFmxTdscdmaObw
	Private instrSession As RFmxInstrMX
	Private tdscdma As RFmxTdscdmaMX
	Private resourceName As String, frequencySource As String, iqPowerEdgeTriggerSource As String
    Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, timeout As Double, _
        frequencyReferenceFrequency As Double, sweepTimeInterval As Double, _
     triggerDelay As Double, minimumQuietTimeDuration As Double, iqPowerEdgeTriggerLevel As Double
	Private averagingEnabled As RFmxTdscdmaMXObwAveragingEnabled
	Private averagingType As RFmxTdscdmaMXObwAveragingType
	Private sweepTimeAuto As RFmxTdscdmaMXObwSweepTimeAuto
	Private averagingCount As Integer

	Private iqPowerEdgeTriggerSlope As RFmxTdscdmaMXIQPowerEdgeTriggerSlope
	Private minimumQuietTimeMode As RFmxTdscdmaMXTriggerMinimumQuietTimeMode
	Private enableTrigger As Boolean
	Private iqPowerEdgeTriggerLevelType As RFmxTdscdmaMXIQPowerEdgeTriggerLevelType

	Private stopFrequency As Double, startFrequency As Double, occupiedBandwidth As Double, absolutePower As Double

	Public Sub Run()
		Try
			InitializeVariables()
			InitializeInstr()
			ConfigureTdscdma()
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

		centerFrequency = 1910000000.0
		' Hz 
		referenceLevel = 0.0
		' dBm 
		externalAttenuation = 0.0
		' dB 

		frequencySource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0
		' Hz 

		triggerDelay = 0.0
		' seconds 
		minimumQuietTimeDuration = 1.6E-05
		' seconds 
		iqPowerEdgeTriggerLevel = -20.0
		'dB
		iqPowerEdgeTriggerSource = "0"
		iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising
		enableTrigger = True
		iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative
        minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto

		'Sweep Time
		sweepTimeAuto = RFmxTdscdmaMXObwSweepTimeAuto.[True]
		sweepTimeInterval = 0.00066
		' seconds 

		'Averaging
		averagingEnabled = RFmxTdscdmaMXObwAveragingEnabled.[False]
		averagingCount = 10
        averagingType = RFmxTdscdmaMXObwAveragingType.Rms

		timeout = 10
		' seconds 

	End Sub

	Private Sub InitializeInstr()
		' Create a new RFmx Session 

		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureTdscdma()
		' Get SpecAn signal 

		tdscdma = instrSession.GetTdscdmaSignalConfiguration()

		' Configure measurement 

        instrSession.ConfigureFrequencyReference("", frequencySource, frequencyReferenceFrequency)
		tdscdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
        tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel, _
                   triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger)

		tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.Obw, True)
		tdscdma.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
        tdscdma.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)

		tdscdma.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		' Retrieve results 

		Dim spectrum As Spectrum(Of Single) = Nothing
		tdscdma.Obw.Results.FetchSpectrum("", timeout, spectrum)
		tdscdma.Obw.Results.FetchMeasurement("", timeout, occupiedBandwidth, absolutePower, startFrequency, stopFrequency)
	End Sub

	Private Sub PrintResults()
		Console.WriteLine("Occupied Bandwidth (Hz)       {0}", occupiedBandwidth)
		Console.WriteLine("Absolute Power (dBm)          {0}", absolutePower)
		Console.WriteLine("Start Frequency (Hz)          {0}", startFrequency)
		Console.WriteLine("Stop Frequency (Hz)           {0}", stopFrequency)
	End Sub

	Private Sub CloseSession()
		Try
			If tdscdma IsNot Nothing Then
				tdscdma.Dispose()
				tdscdma = Nothing
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
