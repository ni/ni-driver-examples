'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Select CHP measurement and enable traces.
'6. Configure Sweep Time Parameters.
'7. Configure Averaging Parameters.
'8. Initiate the Measurement.
'9. Fetch CHP Measurements and Traces.
'10. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.EvdoMX

Public Class RFmxEvdoChpSingleCarrier
    Private instrSession As RFmxInstrMX
    Private evdo As RFmxEvdoMX

    Private resourceName As String = "RFSA"

    Private measurement As RFmxEvdoMXMeasurementTypes = RFmxEvdoMXMeasurementTypes.Chp

    Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
    Private frequency As Double = 10000000.0                           ' Hz 

    Private centerFrequency As Double = 833490000.0                    ' Hz 
    Private externalAttenuation As Double = 0.0                        ' dB 
    Private digitalEdgeSource As String = RFmxEvdoMXConstants.Pfi0

    Private digitalEdge As RFmxEvdoMXDigitalEdgeTriggerEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising
    Private triggerDelay As Double = 0.0
    Private referenceLevel As Double = 0.0

    Private averagingEnabled As RFmxEvdoMXChpAveragingEnabled = RFmxEvdoMXChpAveragingEnabled.[False]
    Private averagingCount As Integer = 10
    Private averagingType As RFmxEvdoMXChpAveragingType = RFmxEvdoMXChpAveragingType.Rms

    Private sweepTimeAuto As RFmxEvdoMXChpSweepTimeAuto = RFmxEvdoMXChpSweepTimeAuto.[True]
    Private sweepTimeInterval As Double = 0.00167                    ' seconds 

    Private timeout As Double = 10.0                                 ' seconds 
    Private carrierAbsolutePower As Double                                  ' dBm 
    Private carrierRelativePower As Double                                  ' dB 
    Private spectrum As Spectrum(Of Single)

    Private enableAllTraces As Boolean = True
    Private enableTrigger As Boolean = False

    Public Sub Run()
        Try
            InitializeInstr()
            ConfigureEvdo()
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

    Private Sub ConfigureEvdo()
        ' Get Evdo signal 

        evdo = instrSession.GetEvdoSignalConfiguration()

        ' Configure measurement 

        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequency)
        evdo.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
        evdo.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
        evdo.SelectMeasurements("", measurement, enableAllTraces)
        evdo.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
        evdo.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
        evdo.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        ' Retrieve results 

        evdo.Chp.Results.FetchCarrierMeasurement("", timeout, carrierAbsolutePower, carrierRelativePower)
        evdo.Chp.Results.FetchSpectrum("", timeout, spectrum)
    End Sub

    Private Sub PrintResults()
        Console.WriteLine("Carrier Absolute Power  (dBm) : {0}", carrierAbsolutePower)
    End Sub

    Private Sub CloseSession()
        If evdo IsNot Nothing Then
            evdo.Dispose()
            evdo = Nothing
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
