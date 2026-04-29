' Steps:
' 1.Open a new RFmx Session.
' 2.Configure Frequency Reference.
' 3.Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
' 4.Configure Trigger Type and Trigger Parameters.
' 5.Configure Carrier Bandwidth.
' 6.Select TXP measurement and enable Traces.
' 7.Configure TXP measurement offset and length.
' 8.Configure Averaging Parameters for TXP measurement.
' 9. Initiate the Measurement.
' 10. Fetch TXP Traces and Measurements.
' 11. Close RFmx Session.

Imports System
Imports NationalInstruments
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteTxpSingleCarrier
    Private instrSession As RFmxInstrMX
    Private lte As RFmxLteMX

    Private resourceName As String

    Private centerFrequency As Double
    Private referenceLevel As Double
    Private externalAttenuation As Double

    Private frequencyReferenceSource As String
    Private frequencyReferenceFrequency As Double

    Private iqPowerEdgeTriggerEnabled As Boolean
    Private iqPowerEdgeTriggerSource As String
    Private iqPowerEdgeTriggerLevel As Double
    Private triggerDelay As Double
    Private minimumQuietTimeDuration As Double
    Private minimumQuietTimeMode As RFmxLteMXTriggerMinimumQuietTimeMode
    Private iqPowerEdgeTriggerLevelType As RFmxLteMXIQPowerEdgeTriggerLevelType
    Private iqPowerEdgeTriggerSlope As RFmxLteMXIQPowerEdgeTriggerSlope

    Private componentCarrierBandwidth As Double
    Private componentCarrierFrequency As Double
    Private cellID As Integer

    Private averagingEnabled As RFmxLteMXTxpAveragingEnabled
    Private averagingCount As Integer

    Private measurementOffset As Double
    Private measurementLength As Double

    Private timeout As Double

    Private averagePowerMean As Double                                                                  ' (dBm) 
    Private peakPowerMaximum As Double                                                                  ' (dBm) 

    Private power As AnalogWaveform(Of Single)

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
        resourceName = "RFSA"

        centerFrequency = 1950000000.0                                                                  ' (Hz) 
        referenceLevel = 0.00                                                                           ' (dBm) 
        externalAttenuation = 0.0                                                                       ' (dBm) 

        frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0                                                        ' (Hz) 

        iqPowerEdgeTriggerEnabled = False
        iqPowerEdgeTriggerSource = "0"
        iqPowerEdgeTriggerLevel = -20.0                                                                 ' dB 
        triggerDelay = 0.0                                                                              ' seconds 
        minimumQuietTimeDuration = 0.00005                                                              ' seconds 
        minimumQuietTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto
        iqPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative
        iqPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising

        componentCarrierBandwidth = 10000000.0                                                          ' (Hz) 
        componentCarrierFrequency = 0.0                                                                 ' (Hz) 
        cellID = 0

        averagingEnabled = RFmxLteMXTxpAveragingEnabled.False
        averagingCount = 10

        measurementOffset = 0.0                                                                         ' seconds 
        measurementLength = 0.001                                                                       ' seconds 

        timeout = 10.0                                                                                  ' seconds 

    End Sub

    Private Sub InitializeInstr()
        ' Create a new RFmx Session 
        instrSession = New RFmxInstrMX(resourceName, "")
    End Sub

    Private Sub ConfigureLte()
        ' Get Lte signal 
        lte = instrSession.GetLteSignalConfiguration()

        ' Configure measurement 
        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

        lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)

        lte.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, iqPowerEdgeTriggerEnabled)

        lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID)

        lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Txp, True)

        lte.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount)

        lte.Txp.Configuration.ConfigureMeasurementOffsetAndInterval("", measurementOffset, measurementLength)
        lte.Initiate("", "")

    End Sub

    Private Sub RetrieveResults()
        ' Retrieve results 

        lte.Txp.Results.FetchMeasurement("", timeout, averagePowerMean, peakPowerMaximum)

        lte.Txp.Results.FetchPowerTrace("", timeout, power)
    End Sub

    Private Sub PrintResults()
        Console.WriteLine(Microsoft.VisualBasic.Constants.vbLf & "********** Measurement **********")

        Console.WriteLine("Average Power Mean (dBm)      : {0}", averagePowerMean)
        Console.WriteLine("Peak Power Maximum (dBm)      : {0}" & Microsoft.VisualBasic.Constants.vbLf, peakPowerMaximum)
    End Sub

    Private Sub CloseSession()
        Try
            If lte IsNot Nothing Then
                lte.Dispose()
                lte = Nothing
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
        Console.WriteLine("ERROR:" & Microsoft.VisualBasic.Constants.vbLf & ex.GetType().ToString() & ": " & ex.Message)
    End Sub
End Class
