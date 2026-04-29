'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties: Clock Source and Clock Frequency
'3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation
'4. Configure the trigger properties
'5. Select CHP measurement and enable the traces
'6. Configure Sweep Time for the CHP measurement
'7. Configure Averaging Parameters for the CHP measurement
'8. Initiate Measurement
'9. Fetch CHP Measurements and Traces
'10. Close the RFmx session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.TdscdmaMX

Class RFmxTdscdmaChp
	Private instrSession As RFmxInstrMX
	Private tdscdma As RFmxTdscdmaMX
	Private resourceName As String, frequencySource As String, iqPowerEdgeTriggerSource As String
    Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, frequencyReferenceFrequency As Double, sweepTimeInterval As Double, _
        triggerDelay As Double, minimumQuietTimeDuration As Double, iqPowerEdgeTriggerLevel As Double, carrierAbsolutePower As Double, timeout As Double
	Private enableAllTraces As Boolean, enableTrigger As Boolean
	Private sweepTimeAuto As RFmxTdscdmaMXChpSweepTimeAuto
	Private averagingEnabled As RFmxTdscdmaMXChpAveragingEnabled
	Private averagingCount As Integer
	Private averagingType As RFmxTdscdmaMXChpAveragingType

	Private iqPowerEdgeTriggerSlope As RFmxTdscdmaMXIQPowerEdgeTriggerSlope
	Private minimumQuietTimeMode As RFmxTdscdmaMXTriggerMinimumQuietTimeMode
	Private iqPowerEdgeTriggerLevelType As RFmxTdscdmaMXIQPowerEdgeTriggerLevelType

	Friend Sub Run()
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
		timeout = 10.0
		' seconds 

		frequencySource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0
		' Hz 

		'Sweep time
		sweepTimeAuto = RFmxTdscdmaMXChpSweepTimeAuto.[True]
		sweepTimeInterval = 0.00066
		' seconds 

		'Averaging
		averagingEnabled = RFmxTdscdmaMXChpAveragingEnabled.[False]
		averagingCount = 10
		averagingType = RFmxTdscdmaMXChpAveragingType.Rms

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

		enableAllTraces = True
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

		tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.Chp, enableAllTraces)

		tdscdma.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
		tdscdma.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)

		tdscdma.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		' Retrieve results 

		Dim spectrum As Spectrum(Of Single) = Nothing
		tdscdma.Chp.Results.FetchCarrierAbsolutePower("", timeout, carrierAbsolutePower)
		tdscdma.Chp.Results.FetchSpectrum("", timeout, spectrum)
	End Sub

	Private Sub PrintResults()
		Console.WriteLine("Carrier Absolute Power (dBm)  {0}", carrierAbsolutePower)
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
