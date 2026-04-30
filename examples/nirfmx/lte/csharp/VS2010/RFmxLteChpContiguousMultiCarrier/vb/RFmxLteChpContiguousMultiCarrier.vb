'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Component Carrier Spacing.
'6. Configure Component Carriers.
'7. Select CHP measurement and enable Traces.
'8. Configure Sweep Time Parameters.
'9. Configure Averaging Parameters for CHP measurement.
'10. Initiate the Measurement.
'11. Fetch CHP Measurements and Traces.
'16. Close RFmx Session. 

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteChpContiguousMultiCarrier
	Private instrSession As RFmxInstrMX
	Private lte As RFmxLteMX
    Private rfsaResourceName As String
	Private frequencyReferenceSource As String
	Private frequencyReferenceFrequency As Double
	Private componentCarrierAtCenterFrequency As Integer

	Private centerFrequency As Double
	Private referenceLevel As Double
	Private externalAttenuation As Double

	Private enableTrigger As Boolean
	Private digitalEdgeSource As String
	Private digitalEdge As RFmxLteMXDigitalEdgeTriggerEdge
	Private triggerDelay As Double

	Private sweepTimeAuto As RFmxLteMXChpSweepTimeAuto
	Private sweepTimeInterval As Double

	Private averagingEnabled As RFmxLteMXChpAveragingEnabled
	Private averagingCount As Integer

	Private averagingType As RFmxLteMXChpAveragingType
	Private timeout As Double

	Private spectrum As Spectrum(Of Single)

	Private totalAggregatedPower As Double
	Private absolutePower As Double()
	Private relativePower As Double()

    Const numberOfComponentCarriers As Integer = 2
	Private cellID As Integer()

    Private componentCarrierFrequency As Double() = {-9900000.0, 9900000.0}, componentCarrierBandwidth As Double() = {20000000.0, 20000000.0}
	Private componentCarrierSpacingType As RFmxLteMXComponentCarrierSpacingType

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
		componentCarrierAtCenterFrequency = -1

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

		sweepTimeAuto = RFmxLteMXChpSweepTimeAuto.[True]
		sweepTimeInterval = 0.001
		' (s) 

		averagingEnabled = RFmxLteMXChpAveragingEnabled.[False]
		averagingCount = 10
		averagingType = RFmxLteMXChpAveragingType.Rms

		timeout = 10.0
		' (s) 

        cellID = Nothing

		componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal
	End Sub

	Private Sub ConfigureLte()
		lte = instrSession.GetLteSignalConfiguration()
		' Create a new RFmx Session 
		instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

		lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)

        lte.ComponentCarrier.ConfigureSpacing("", componentCarrierSpacingType, componentCarrierAtCenterFrequency)

        lte.ConfigureNumberOfComponentCarriers("", numberOfComponentCarriers)

        lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth, componentCarrierFrequency, cellID)
		lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Chp, True)
		lte.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
		lte.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
		lte.Initiate("", "")

	End Sub

	Private Sub RetrieveResults()
        lte.Chp.Results.ComponentCarrier.FetchMeasurementArray("", timeout, absolutePower, relativePower)
        lte.Chp.Results.FetchTotalAggregatedPower("", timeout, totalAggregatedPower)
		lte.Chp.Results.FetchSpectrum("", timeout, spectrum)

	End Sub

	Private Sub PrintResults()
        Console.WriteLine(vbLf & "Total Aggregated Power  (dBm)        : {0}", totalAggregatedPower)

        Console.WriteLine(vbLf & "Component Carrier  Measurements:")
        For i As Integer = 0 To absolutePower.Length - 1
			Console.WriteLine(vbLf & "Carrier:  {0}", i)
            Console.WriteLine("Absolute Power (dBm)                 : {0}", absolutePower(i))
            Console.WriteLine("Relative Power (dB)                  : {0}", relativePower(i))
		Next

	End Sub

	Private Sub InitializeInstr()
		instrSession = New RFmxInstrMX(rfsaResourceName, "")
	End Sub

	Private Shared Sub DisplayError(ex As Exception)
		Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
	End Sub

End Class
