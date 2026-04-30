'Steps:
'1. Open a new RFmx session
'2. Configure the basic instrument properties: Clock Source and Clock Frequency
'3. Configure the basic signal properties: External Attenuation 
'4. Configure the Center Frequency (or Channel Number which derives the Center Frequency)
'5. Configure the trigger properties
'6. Configure Auto Level 
'7. Select ACP measurement and enable the traces
'8. Configure Averaging parameters for the ACP measurement
'9. Configure Sweep Time for the ACP measurement
'10. Configure Noise Compensation for the ACP measurement
'11. Configure Number of Offset Channels for the ACP measurement
'12. Initiate Measurement
'13. Fetch ACP Measurements and Traces
'14. Close the RFmx session

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.TdscdmaMX

Public Class RFmxTdscdmaAcp
	Private instrSession As RFmxInstrMX
	Private tdscdma As RFmxTdscdmaMX
	Private resourceName As String, frequencySource As String, iqPowerEdgeTriggerSource As String

	Const NumberOfOffsets As Integer = 2

    Private centerFrequency As Double, referenceLevel As Double, autoSetReferenceLevel As Double, externalAttenuation As Double, frequencyReferenceFrequency As Double, sweepTimeInterval As Double, _
  measurementInterval As Double, timeout As Double, triggerDelay As Double, minimumQuietTimeDuration As Double, iqPowerEdgeTriggerLevel As Double
	Private autoLevel As Boolean, enableTrigger As Boolean

	Private noiseCompensationEnabled As RFmxTdscdmaMXAcpNoiseCompensationEnabled

	Private sweepTimeAuto As RFmxTdscdmaMXAcpSweepTimeAuto
	Private averagingCount As Integer
	Private averagingEnabled As RFmxTdscdmaMXAcpAveragingEnabled
	Private averagingType As RFmxTdscdmaMXAcpAveragingType

	Private iqPowerEdgeTriggerSlope As RFmxTdscdmaMXIQPowerEdgeTriggerSlope
	Private minimumQuietTimeMode As RFmxTdscdmaMXTriggerMinimumQuietTimeMode
	Private iqPowerEdgeTriggerLevelType As RFmxTdscdmaMXIQPowerEdgeTriggerLevelType


	Private carrierAbsolutePower As Double
	Private lowerRelativePower As Double()
	Private upperRelativePower As Double()
	Private lowerAbsolutePower As Double()
	Private upperAbsolutePower As Double()

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

		frequencySource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0
		' Hz 

		measurementInterval = 0.005
		' seconds 
        autoLevel = False

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

		noiseCompensationEnabled = RFmxTdscdmaMXAcpNoiseCompensationEnabled.[False]

		' Sweep Time
		sweepTimeAuto = RFmxTdscdmaMXAcpSweepTimeAuto.[True]
		sweepTimeInterval = 0.00066
		' seconds 

		'Averaging 
		averagingEnabled = RFmxTdscdmaMXAcpAveragingEnabled.[False]
		averagingCount = 10
		averagingType = RFmxTdscdmaMXAcpAveragingType.Rms

		timeout = 10.0
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
		tdscdma.ConfigureExternalAttenuation("", externalAttenuation)
		tdscdma.ConfigureFrequency("", centerFrequency)
		tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel, triggerDelay, minimumQuietTimeMode, _
			minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger)


		If autoLevel Then
			tdscdma.AutoLevel("", measurementInterval, autoSetReferenceLevel)
            Console.WriteLine("Reference Level (dBm):                 {0}" & vbLf, autoSetReferenceLevel)
		Else
			tdscdma.ConfigureReferenceLevel("", referenceLevel)
		End If

		tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.Acp, True)
		tdscdma.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
		tdscdma.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
		tdscdma.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled)
		tdscdma.Acp.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsets)

		tdscdma.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		' Retrieve results 


		Dim spectrum As Spectrum(Of Single) = Nothing

		tdscdma.Acp.Results.FetchOffsetMeasurementArray("", timeout, lowerRelativePower, upperRelativePower, lowerAbsolutePower, upperAbsolutePower)

		tdscdma.Acp.Results.FetchCarrierAbsolutePower("", timeout, carrierAbsolutePower)

		tdscdma.Acp.Results.FetchSpectrum("", timeout, spectrum)
	End Sub

	Private Sub PrintResults()
		Console.WriteLine("-----------------Carrier Measurements-----------------" & vbLf)
        Console.WriteLine("Carrier Absolute Power (dBm)         {0}", carrierAbsolutePower)

		Console.WriteLine(vbLf & "--------------Offset Channel Measurements-------------" & vbLf)
		For i As Integer = 0 To NumberOfOffsets - 1
            Console.WriteLine("----Offset {0}" & vbLf, i)
			Console.WriteLine("Lower Relative Power (dB)            {0}", lowerRelativePower(i))
			Console.WriteLine("Upper Relative Power (dB)            {0}", upperRelativePower(i))
			Console.WriteLine("Lower Absolute Power (dBm)           {0}", lowerAbsolutePower(i))
			Console.WriteLine("Upper Absolute Power (dBm)           {0}", upperAbsolutePower(i))
		Next
		Console.WriteLine("-------------------------------------------------" & vbLf)
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
