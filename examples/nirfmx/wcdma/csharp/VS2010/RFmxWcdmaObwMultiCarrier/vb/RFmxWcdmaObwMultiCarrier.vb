'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Contiguous Carriers
'6. Select OBW measurement and enable Traces.
'7. Configure Sweep Time Parameters.
'8. Configure Averaging Parameters for OBW measurement.
'9. Initiate the Measurement.
'10. Fetch OBW Measurements and Traces.
'11. Close RFmx Session. 
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WcdmaMX

Public Class RFmxWcdmaObwMultiCarrier
	Private instrSession As RFmxInstrMX
	Private wcdma As RFmxWcdmaMX

	Private resourceName As String = "RFSA"
	Private measurement As RFmxWcdmaMXMeasurementTypes = RFmxWcdmaMXMeasurementTypes.Obw
	Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
    Private frequencyReferenceFrequency As Double = 10000000.0                ' Hz 
    Private centerFrequency As Double = 1950000000.0        ' Hz 
    Private externalAttenuation As Double = 0.0             ' dB 

	Private digitalEdgeSource As String = RFmxWcdmaMXConstants.Pfi0
	Private digitalEdge As RFmxWcdmaMXDigitalEdgeTriggerEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising
    Private triggerDelay As Double = 0                      ' seconds 
    Private referenceLevel As Double = 0.0                  ' dBm 
    Private averagingEnabled As RFmxWcdmaMXObwAveragingEnabled = RFmxWcdmaMXObwAveragingEnabled.False
	Private averagingCount As Integer = 10
	Private averagingType As RFmxWcdmaMXObwAveragingType = RFmxWcdmaMXObwAveragingType.Rms
    Private sweepTimeAuto As RFmxWcdmaMXObwSweepTimeAuto = RFmxWcdmaMXObwSweepTimeAuto.True
    Private sweepTimeInterval As Double = 0.000000066667    ' seconds 
    Private timeout As Double = 10                          ' seconds 
	Private spectrum As Spectrum(Of Single)

	Private enableAllTraces As Boolean = True
	Private enableTrigger As Boolean = False

    Private numberOfCarriers As Integer = 2
    Private carrierAtCenterFrequency As Integer = -1
	Private occupiedBandwidth As Double
	Private absolutePower As Double
	Private startFrequency As Double
	Private stopFrequency As Double


	Public Sub Run()
		Try
			InitializeInstr()
			ConfigureWcdma()
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
		wcdma.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
		wcdma.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
		wcdma.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
		wcdma.Obw.Results.FetchMeasurement("", timeout, occupiedBandwidth, absolutePower, startFrequency, stopFrequency)
		wcdma.Obw.Results.FetchSpectrum("", timeout, spectrum)
	End Sub

	Private Sub PrintResults()
		Console.WriteLine("Measurement" & vbLf)
        Console.WriteLine("Occupied Bandwidth (Hz)   : {0}", occupiedBandwidth)
		Console.WriteLine("Absolute Power (dBm)       : {0}", absolutePower)
		Console.WriteLine("Start Frequency (Hz)       : {0}", startFrequency)
		Console.WriteLine("Stop Frequency (Hz)        : {0}", stopFrequency)
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
