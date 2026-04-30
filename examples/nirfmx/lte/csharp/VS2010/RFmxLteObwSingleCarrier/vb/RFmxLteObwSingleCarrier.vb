'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Carrier Bandwidth.
'6. Select OBW measurement and enable Traces.
'7. Configure Sweep Time Parameters.
'8. Configure Averaging Parameters for OBW measurement.
'9. Initiate the Measurement.
'10. Fetch OBW Measurements and Traces.
'11. Close RFmx Session. 

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteObwSingleCarrier
	Private instrSession As RFmxInstrMX
	Private lte As RFmxLteMX
	Private rfsaResourceName As String

	Private frequencyReferenceSource As String
	Private frequencyReferenceFrequency As Double

	Private centerFrequency As Double
	Private referenceLevel As Double
	Private externalAttenuation As Double

	Private enableTrigger As Boolean
	Private digitalEdgeSource As String
	Private digitalEdge As RFmxLteMXDigitalEdgeTriggerEdge
	Private triggerDelay As Double

	Private componentCarrierBandwidth As Double
	Private componentCarrierFrequency As Double
	Private cellID As Integer
    Private linkDirection As RFmxLteMXLinkDirection

	Private sweepTimeAuto As RFmxLteMXObwSweepTimeAuto
	Private sweepTimeInterval As Double

	Private averagingEnabled As RFmxLteMXObwAveragingEnabled
	Private averagingCount As Integer

	Private averagingType As RFmxLteMXObwAveragingType
	Private timeout As Double

	Private stopFrequency As Double
	Private startFrequency As Double
	Private occupiedBandwidth As Double
	Private absolutePower As Double

	Private spectrum As Spectrum(Of Single)

	Private Sub CloseSession()
		If lte IsNot Nothing Then
			lte.Dispose()
			lte = Nothing
		End If
		If instrSession IsNot Nothing Then
			instrSession.Close()
			instrSession = Nothing
		End If
	End Sub

	Public Sub Run()
		Try
			InitializeVariables()
			InitializeInstr()
			ConfigureLte()
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

	Private Sub InitializeInstr()
		instrSession = New RFmxInstrMX(rfsaResourceName, "")
	End Sub

	Private Sub InitializeVariables()
		rfsaResourceName = "RFSA"

		frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
		frequencyReferenceFrequency = 10000000.0
		' (Hz) 

		centerFrequency = 1950000000.0
		' (Hz) 
		referenceLevel = 0.0
		' (dBm) 
		externalAttenuation = 0.0
		' (dBm) 

		enableTrigger = False
		digitalEdgeSource = RFmxLteMXConstants.Pfi0
		digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising
		triggerDelay = 0.0
		' (s) 

        linkDirection = RFmxLteMXLinkDirection.Uplink
		componentCarrierBandwidth = 200e3
		' (Hz) 
		componentCarrierFrequency = 0.0
		' (Hz) 
		cellID = 0

		sweepTimeAuto = RFmxLteMXObwSweepTimeAuto.[True]
		sweepTimeInterval = 0.001
		' (s) 

		averagingEnabled = RFmxLteMXObwAveragingEnabled.[False]
		averagingCount = 10
		averagingType = RFmxLteMXObwAveragingType.Rms

		timeout = 10.0
		' (s) 
	End Sub

	Private Sub ConfigureLte()
		lte = instrSession.GetLteSignalConfiguration()
		' Create a new RFmx Session 
		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
		lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
        lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID)
        lte.ConfigureLinkDirection("", linkDirection)
		lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Obw, True)
		lte.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
        lte.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
        lte.Initiate("", "")
	End Sub

    Private Sub RetrieveResults()
        lte.Obw.Results.FetchMeasurement("", timeout, occupiedBandwidth, absolutePower, startFrequency, stopFrequency)
        lte.Obw.Results.FetchSpectrum("", timeout, spectrum)

    End Sub

	Private Sub PrintResults()
		Console.WriteLine("Occupied Bandwidth (Hz)      :{0}", occupiedBandwidth)

		Console.WriteLine("Absolute Power (dBm)         :{0}", absolutePower)

		Console.WriteLine("Start Frequency (Hz)         :{0}", startFrequency)

		Console.WriteLine("Stop Frequency (Hz)          :{0}", stopFrequency)


	End Sub

	Private Shared Sub DisplayError(ex As Exception)
		Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
	End Sub

End Class
