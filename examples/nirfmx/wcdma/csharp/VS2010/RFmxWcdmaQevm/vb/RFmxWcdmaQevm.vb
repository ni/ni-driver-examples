'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Select QEVM measurement and enable Traces.
'6. Configure Averaging.
'7. Configure Measurement Length.
'8. Initiate the Measurement.
'9. Fetch QEVM Measurements and Traces.
'10. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.WcdmaMX

Public Class RFmxWcdmaQevm
	Private instrSession As RFmxInstrMX
	Private wcdma As RFmxWcdmaMX

    Private resourceName As String
    Private measurement As RFmxWcdmaMXMeasurementTypes
    Private frequencyReferenceSource As String
    Private frequencyReferenceFrequency As Double
    ' Hz 
    Private centerFrequency As Double
    ' Hz 
    Private externalAttenuation As Double
    ' dB 

    Private digitalEdgeSource As String
    Private digitalEdge As RFmxWcdmaMXDigitalEdgeTriggerEdge
    Private triggerDelay As Double
    ' seconds 
    Private referenceLevel As Double
    ' dBm 
    Private averagingEnabled As RFmxWcdmaMXQevmAveragingEnabled
    Private averagingCount As Integer

    Private measurementLength As Integer
    ' chips 

    Private timeout As Double
    ' seconds 

    Private meanPhaseError As Double
    '(deg) 
    Private meanMagnitudeError As Double
    '(%) 
    Private meanRmsEvm As Double
    '(%) 
    Private maximumPeakEvm As Double
    '(%) 
    Private meanFrequencyError As Double
    '(Hz) 
    Private meanChipRateError As Double
    '(ppm) 

    Private maximumIQGainImbalence As Double
    '(dB) 
    Private maximumIQOriginOffset As Double
    '(dB) 
    Private meanIQOriginOffset As Double
    '(dB) 
    Private meanIQGainImbalence As Double
    '(dB) 
    Private meanIQQuadratureError As Double
    '(deg) 
    Private maximumIQQuadratureError As Double
    '(deg) 

    Private enableAllTraces As Boolean
    Private enableTrigger As Boolean


    Private evm As AnalogWaveform(Of Single) = Nothing
    Private constellationTrace As ComplexSingle() = Nothing


    Public Sub Run()
        Try
            InitializeVariables()
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

    Private Sub InitializeVariables()
        ' Initialize input variables 


        resourceName = "RFSA"
        centerFrequency = 1950000000.0
        ' Hz 
        referenceLevel = 0.0
        ' dBm 
        externalAttenuation = 0.0
        ' dB 

        measurement = RFmxWcdmaMXMeasurementTypes.Qevm

        frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0
        ' Hz 

        digitalEdgeSource = RFmxWcdmaMXConstants.Pfi0
        digitalEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising
        triggerDelay = 0.0
        ' seconds 
        enableTrigger = False


        averagingEnabled = RFmxWcdmaMXQevmAveragingEnabled.[False]
        averagingCount = 10

        measurementLength = 2560
        ' chips 

        timeout = 10
        ' seconds 

        enableAllTraces = True

    End Sub

    Private Sub ConfigureWcdma()
        wcdma = instrSession.GetWcdmaSignalConfiguration()
        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
        wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
        wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
        wcdma.SelectMeasurements("", measurement, enableAllTraces)
        wcdma.Qevm.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)
        wcdma.Qevm.Configuration.ConfigureMeasurementLength("", measurementLength)
        wcdma.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        wcdma.Qevm.Results.FetchEvm("", timeout, meanRmsEvm, maximumPeakEvm, meanFrequencyError, meanMagnitudeError, _
         meanPhaseError, meanChipRateError)
        wcdma.Qevm.Results.FetchIQImpairments("", timeout, meanIQOriginOffset, meanIQGainImbalence, meanIQQuadratureError, maximumIQOriginOffset, _
         maximumIQGainImbalence, maximumIQQuadratureError)
        wcdma.Qevm.Results.FetchEvmTrace("", timeout, evm)
        wcdma.Qevm.Results.FetchConstellationTrace("", timeout, constellationTrace)
    End Sub

    Private Sub PrintResults()
        Console.WriteLine("------------EVM------------" & vbLf)
        Console.WriteLine("Mean RMS EVM (%)                     : {0}", meanRmsEvm)
        Console.WriteLine("Maximum Peak EVM (%)                 : {0}", maximumPeakEvm)
        Console.WriteLine("Mean Frequency Error (Hz)            : {0}", meanFrequencyError)
        Console.WriteLine("Mean Magnitude Error (%)             : {0}", meanMagnitudeError)
        Console.WriteLine("Mean Phase Error (deg)               : {0}", meanPhaseError)
        Console.WriteLine("Mean Chip Rate Error (ppm)           : {0}", meanChipRateError)

        Console.WriteLine("------------IQ Impairments------------" & vbLf)
        Console.WriteLine("Mean I/Q Origin Offset (dB)          : {0}", meanIQOriginOffset)
        Console.WriteLine("Maximum I/Q Origin Offset (dB)       : {0}", maximumIQOriginOffset)
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
