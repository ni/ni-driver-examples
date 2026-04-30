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
Imports NationalInstruments.RFmx.Cdma2kMX

Class RFmxCdma2kChp
	Private instrSession As RFmxInstrMX
	Private cdma2k As RFmxCdma2kMX
	Private resourceName As String, frequencySource As String
	Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, frequency As Double, sweepTimeInterval As Double
	Private sweepTimeAuto As RFmxCdma2kMXChpSweepTimeAuto
	Private averagingEnabled As RFmxCdma2kMXChpAveragingEnabled
	Private averagingCount As Integer
	Private averagingType As RFmxCdma2kMXChpAveragingType

	Private digitalEdgeTriggerSource As String
	Private digitalEdgeTriggerEdge As RFmxCdma2kMXDigitalEdgeTriggerEdge
	Private triggerDelay As Double
	Private enableTrigger As Boolean

	Private carrierAbsolutePower As Double, timeout As Double

	Friend Sub Run()
		Try
			InitializeVariables()
			InitializeInstr()
			ConfigureCdma2k()
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
		centerFrequency = 833490000.0
		' Hz 
		referenceLevel = 0.0
		' dBm 
		externalAttenuation = 0.0
		' dB 
		timeout = 10.0
		' seconds 

		frequencySource = RFmxInstrMXConstants.OnboardClock
		frequency = 10000000.0
		' Hz 

		'Sweep time
		sweepTimeAuto = RFmxCdma2kMXChpSweepTimeAuto.[True]
		sweepTimeInterval = 0.00167
		' seconds 

		'Averaging
		averagingEnabled = RFmxCdma2kMXChpAveragingEnabled.[False]
		averagingCount = 10
		averagingType = RFmxCdma2kMXChpAveragingType.Rms

		triggerDelay = 0.0
		' seconds 

		digitalEdgeTriggerSource = RFmxInstrMXConstants.Pfi0
		digitalEdgeTriggerEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising
		enableTrigger = False
	End Sub

	Private Sub InitializeInstr()
		' Create a new RFmx Session 

		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureCdma2k()
		' Get SpecAn signal 

		cdma2k = instrSession.GetCdma2kSignalConfiguration()

		' Configure measurement 

		instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
		cdma2k.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		cdma2k.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)

		cdma2k.SelectMeasurements("", RFmxCdma2kMXMeasurementTypes.Chp, True)

		cdma2k.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
		cdma2k.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)

		cdma2k.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		' Retrieve results 

		Dim spectrum As Spectrum(Of Single) = Nothing
		cdma2k.Chp.Results.FetchCarrierAbsolutePower("", timeout, carrierAbsolutePower)
		cdma2k.Chp.Results.FetchSpectrum("", timeout, spectrum)
	End Sub

	Private Sub PrintResults()
		Console.WriteLine("Carrier Absolute Power (dBm)  : {0}", carrierAbsolutePower)
	End Sub

	Private Sub CloseSession()
		Try
			If cdma2k IsNot Nothing Then
				cdma2k.Dispose()
				cdma2k = Nothing
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
