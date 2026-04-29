'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Component Carrier Spacing.
'6. Configure Component Carriers.
'7. Select OBW measurement and enable Traces.
'8. Configure Sweep Time Parameters.
'9. Configure Averaging Parameters for OBW measurement.
'10. Initiate the Measurement.
'11. Fetch OBW Measurements and Traces.
'12. Close RFmx Session. 

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteObwContiguousMultiCarrier
	Private instrSession As RFmxInstrMX
	Private lte As RFmxLteMX
	Private rfsaResourceName As String

    Const NumberOfComponentCarriers As Integer = 2

	Private frequencyReferenceSource As String
	Private frequencyReferenceFrequency As Double

	Private centerFrequency As Double
	Private referenceLevel As Double
	Private externalAttenuation As Double

	Private enableTrigger As Boolean
	Private digitalEdgeSource As String
	Private digitalEdge As RFmxLteMXDigitalEdgeTriggerEdge
	Private triggerDelay As Double

    Private linkDirection As RFmxLteMXLinkDirection
    Private componentCarrierBandwidth As Double() = {20000000.0, 20000000.0}
    Private componentCarrierFrequency As Double() = {-9900000.0, 9900000.0}

	Private sweepTimeAuto As RFmxLteMXObwSweepTimeAuto
	Private sweepTimeInterval As Double

	Private averagingEnabled As RFmxLteMXObwAveragingEnabled
	Private averagingCount As Integer

	Private averagingType As RFmxLteMXObwAveragingType
	Private timeout As Double

	Private spectrum As Spectrum(Of Single)

	Private componentCarrierSpacingType As RFmxLteMXComponentCarrierSpacingType
	Private componentCarrierAtCenterFrequency As Integer
	Private occupiedBandwidth As Double
	Private absolutePower As Double
	Private startFrequency As Double
	Private stopFrequency As Double

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
		sweepTimeAuto = RFmxLteMXObwSweepTimeAuto.[True]
		sweepTimeInterval = 0.001
		' (s) 

		averagingEnabled = RFmxLteMXObwAveragingEnabled.[False]
		averagingCount = 10
		averagingType = RFmxLteMXObwAveragingType.Rms

		timeout = 10.0
		' (s) 

		componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal
		componentCarrierAtCenterFrequency = -1

	End Sub

	Private Sub ConfigureLte()
		lte = instrSession.GetLteSignalConfiguration()
		' Create a new RFmx Session 

		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
		lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
        lte.ComponentCarrier.ConfigureSpacing("", componentCarrierSpacingType, componentCarrierAtCenterFrequency)
        lte.ConfigureNumberOfComponentCarriers("", NumberOfComponentCarriers)
        lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth, componentCarrierFrequency, Nothing)
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
        Console.WriteLine("Occupied Bandwidth (Hz)  : {0}", occupiedBandwidth)
        Console.WriteLine("Absolute Power (dBm)     : {0}", absolutePower)
        Console.WriteLine("Start Frequency (Hz)     : {0}", startFrequency)
        Console.WriteLine("Stop Frequency (Hz)      : {0}", stopFrequency)
	End Sub

	Private Sub InitializeInstr()
		instrSession = New RFmxInstrMX(rfsaResourceName, "")
	End Sub

	Private Shared Sub DisplayError(ex As Exception)
		Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
	End Sub

End Class
