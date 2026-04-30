'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, RF Attenuation and External Attenuation)
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Reference Level.
'6. Select ACP measurement and enable Traces.
'7. Configure Measurement Method.
'8. Configure Averaging Parameters for ACP measurement.
'9. Configure Sweep Time Parameters.
'10. Configure Noise Compensation Parameter.
'11. Configure Number of offset channels. 
'12. Initiate the Measurement.
'13  Fetch ACP Measurements and Traces.
'14. Close RFmx Session.
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WcdmaMX

Public Class RFmxWcdmaAcpSingleCarrier
	Private instrSession As RFmxInstrMX
	Private wcdma As RFmxWcdmaMX

	Private resourceName As String = "RFSA"
	Private measurement As RFmxWcdmaMXMeasurementTypes = RFmxWcdmaMXMeasurementTypes.Acp
	Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
    Private frequencyReferenceFrequency As Double = 10000000.0                    ' Hz 
    Private centerFrequency As Double = 1950000000.0            ' Hz 
    Private externalAttenuation As Double = 0.0                 ' dB 
    Private rfAttenuationAuto As RFmxInstrMXRFAttenuationAuto = RFmxInstrMXRFAttenuationAuto.True
	Private rfAttenuation As Double = 10.0
	Private digitalEdgeSource As String = RFmxWcdmaMXConstants.Pfi0
	Private digitalEdge As RFmxWcdmaMXDigitalEdgeTriggerEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising
    Private triggerDelay As Double = 0.0                        ' seconds 
    Private measurementInterval As Double = 0.01                ' seconds 
    Private referenceLevel As Double = 0.0                      ' dBm 
	Private measurementMethod As RFmxWcdmaMXAcpMeasurementMethod = RFmxWcdmaMXAcpMeasurementMethod.Normal
    Private averagingEnabled As RFmxWcdmaMXAcpAveragingEnabled = RFmxWcdmaMXAcpAveragingEnabled.False
	Private averagingCount As Integer = 10
	Private averagingType As RFmxWcdmaMXAcpAveragingType = RFmxWcdmaMXAcpAveragingType.Rms
	Private autoLevel As Boolean = True
    Private sweepTimeAuto As RFmxWcdmaMXAcpSweepTimeAuto = RFmxWcdmaMXAcpSweepTimeAuto.True
    Private sweepTimeInterval As Double = 0.000667             ' seconds 
    Private noiseCompensationEnabled As RFmxWcdmaMXAcpNoiseCompensationEnabled = RFmxWcdmaMXAcpNoiseCompensationEnabled.False
	Private numberOfOffsets As Integer = 2
    Private timeout As Double = 10.0                           ' seconds 
    Private absolutePower As Double = 0.0                      ' dBm 
    Private relativePower As Double = 0.0                      ' dB 
	Private spectrum As Spectrum(Of Single)

	Private enableAllTraces As Boolean = True
    Private enableTrigger As Boolean = False

	Private lowerAbsolutePower As Double(), upperAbsolutePower As Double(), lowerRelativePower As Double(), upperRelativePower As Double()

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
		wcdma.ConfigureFrequency("", centerFrequency)
		wcdma.ConfigureExternalAttenuation("", externalAttenuation)
		instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation)
		wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
		If autoLevel Then
            wcdma.AutoLevel("", measurementInterval, referenceLevel)
            Console.WriteLine("Reference Level (dBm)         : {0}", referenceLevel)
		Else

			wcdma.ConfigureReferenceLevel("", referenceLevel)
		End If
		wcdma.SelectMeasurements("", measurement, enableAllTraces)
		wcdma.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod)
		wcdma.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
		wcdma.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
		wcdma.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled)
		wcdma.Acp.Configuration.ConfigureNumberOfOffsets("", numberOfOffsets)
		wcdma.Initiate("", "")
	End Sub

	Private Sub RetrieveResults()
        wcdma.Acp.Results.FetchOffsetMeasurementArray("", timeout, lowerRelativePower, upperRelativePower,
                                                      lowerAbsolutePower, upperAbsolutePower)

        wcdma.Acp.Results.FetchCarrierMeasurement("", timeout, absolutePower, relativePower)

		wcdma.Acp.Results.FetchSpectrum("", timeout, spectrum)
	End Sub

	Private Sub PrintResults()
        Console.WriteLine(vbLf & "Carrier Absolute Power  (dBm) : {0}", absolutePower)
		Console.WriteLine(vbLf & "Offset Channel Measurements:")
		For i As Integer = 0 To lowerRelativePower.Length - 1
			Console.WriteLine(vbLf & "Offset  {0}", i)
            Console.WriteLine("Lower Relative Power (dB)     : {0}", lowerRelativePower(i))
            Console.WriteLine("Upper Relative Power (dB)     : {0}", upperRelativePower(i))
            Console.WriteLine("Lower Absolute Power (dBm)    : {0}", lowerAbsolutePower(i))
            Console.WriteLine("Upper Absolute Power (dBm)    : {0}", upperAbsolutePower(i))
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
