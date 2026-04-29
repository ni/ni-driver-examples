'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Contiguous Carriers
'6. Select CHP measurement and enable Traces.
'7. Configure Sweep Time Parameters.
'8. Configure Averaging Parameters for CHP measurement.
'9. Initiate the Measurement.
'10. Fetch CHP Measurements and Traces.
'11. Close RFmx Session. 
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WcdmaMX

Public Class RFmxWcdmaChpMultiCarrier
	Private instrSession As RFmxInstrMX
	Private wcdma As RFmxWcdmaMX

	Private resourceName As String = "RFSA"
	Private measurement As RFmxWcdmaMXMeasurementTypes = RFmxWcdmaMXMeasurementTypes.Chp
	Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
    Private frequencyReferenceFrequency As Double = 10000000.0                    ' Hz 
    Private centerFrequency As Double = 1950000000.0            ' Hz 
    Private externalAttenuation As Double = 0.0                 ' dB 
	Private digitalEdgeSource As String = RFmxWcdmaMXConstants.Pfi0
	Private digitalEdge As RFmxWcdmaMXDigitalEdgeTriggerEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising
    Private triggerDelay As Double = 0.0                        ' seconds 
    Private referenceLevel As Double = 0.0                      ' dBm 
    Private averagingEnabled As RFmxWcdmaMXChpAveragingEnabled = RFmxWcdmaMXChpAveragingEnabled.False
	Private averagingCount As Integer = 10
    Private carrierAtCenterFrequency As Integer = -1
    Private numberOfCarriers As Integer = 2
	Private averagingType As RFmxWcdmaMXChpAveragingType = RFmxWcdmaMXChpAveragingType.Rms
    Private sweepTimeAuto As RFmxWcdmaMXChpSweepTimeAuto = RFmxWcdmaMXChpSweepTimeAuto.True
    Private sweepTimeInterval As Double = 0.000667              ' seconds 
    Private timeout As Double = 10.0                            ' seconds 
	Private absolutePower As Double()
	Private relativePower As Double()
	Private totalCarrierPower As Double
	Private spectrum As Spectrum(Of Single)

	Private enableAllTraces As Boolean = True
    Private enableTrigger As Boolean = False

	Public Sub Run()
		Try
			InitializeInstr()
			ConfigureWcdma()
			RetrieveResults()
			PrintResults()
		Catch ex As Exception
			DisplayError(ex)
		Finally
			CloseSession()
			Console.WriteLine("Press any key to exit")
			Console.ReadKey()
		End Try
	End Sub

	Private Sub InitializeInstr()
		' Create a new RFmx Session 

		instrSession = New RFmxInstrMX(resourceName, "")
	End Sub

	Private Sub ConfigureWcdma()
		wcdma = instrSession.GetWcdmaSignalConfiguration()
        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
		wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
		wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
        wcdma.ConfigureContiguousCarriers("", numberOfCarriers, carrierAtCenterFrequency)
		wcdma.SelectMeasurements("", measurement, enableAllTraces)
		wcdma.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
		wcdma.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
		wcdma.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		wcdma.Chp.Results.FetchCarrierMeasurementArray("", timeout, absolutePower, relativePower)
		wcdma.Chp.Results.FetchTotalCarrierPower("", timeout, totalCarrierPower)
		wcdma.Chp.Results.FetchSpectrum("", timeout, spectrum)
	End Sub

    Private Sub PrintResults()
        Console.WriteLine("Total Carrier Power  (dBm) : {0}" & vbCrLf & vbCrLf, totalCarrierPower)
        Console.WriteLine("Carrier Measurements       : ")
        For i As Integer = 0 To absolutePower.Length - 1
            Console.WriteLine("Carrier                    : {0} ", i)
            Console.WriteLine("Absolute Power  (dBm)      : {0}", absolutePower(i))
            Console.WriteLine("Relative Power  (dB)       : {0}", relativePower(i))
            Console.WriteLine("---------------------------------------------------------")
        Next
    End Sub

	Private Sub CloseSession()
		If wcdma IsNot Nothing Then
			wcdma.Dispose()
			wcdma = Nothing
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
