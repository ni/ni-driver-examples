'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Contiguous Carriers.
'6. Select CHP measurement and enable traces.
'7. Configure Sweep Time Parameters.
'8. Configure Averaging Parameters.
'9. Initiate the Measurement.
'10. Fetch CHP Measurements and Traces.
'11. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.EvdoMX

Public Class RFmxEvdoChpMultiCarrier
    Private instrSession As RFmxInstrMX
    Private evdo As RFmxEvdoMX

    Private resourceName As String = "RFSA"
    Private measurement As RFmxEvdoMXMeasurementTypes = RFmxEvdoMXMeasurementTypes.Chp

    Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
    Private frequency As Double = 10000000.0                          ' Hz 

    Private centerFrequency As Double = 833490000.0                   ' Hz 
    Private externalAttenuation As Double = 0.0                       ' dB 

    Private digitalEdgeSource As String = RFmxEvdoMXConstants.Pfi0
    Private digitalEdge As RFmxEvdoMXDigitalEdgeTriggerEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising

    Private triggerDelay As Double = 0.0                              ' seconds 
    Private referenceLevel As Double = 0.0                            ' dBm 

    Private averagingEnabled As RFmxEvdoMXChpAveragingEnabled = RFmxEvdoMXChpAveragingEnabled.[False]

    Private bandClass As Integer = 0
    Private frequencyReferenceCarrier As Integer = -1
    Private numberOfCarriers As Integer = 3

    Private averagingCount As Integer = 10
    Private averagingType As RFmxEvdoMXChpAveragingType = RFmxEvdoMXChpAveragingType.Rms

    Private sweepTimeAuto As RFmxEvdoMXChpSweepTimeAuto = RFmxEvdoMXChpSweepTimeAuto.[True]
    Private sweepTimeInterval As Double = 0.00167                    ' seconds 

    Private timeout As Double = 10.0                                 ' seconds 
    Private carrierAbsolutePower As Double()
    Private carrierRelativePower As Double()
    Private totalCarrierPower As Double
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
        evdo.ConfigureContiguousCarriers("", numberOfCarriers, frequencyReferenceCarrier, bandClass)
        evdo.SelectMeasurements("", measurement, enableAllTraces)
        evdo.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
        evdo.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
        evdo.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        ' Retrieve results 


        evdo.Chp.Results.FetchCarrierMeasurementArray("", timeout, carrierAbsolutePower, carrierRelativePower)
        evdo.Chp.Results.FetchTotalCarrierPower("", timeout, totalCarrierPower)
        evdo.Chp.Results.FetchSpectrum("", timeout, spectrum)
    End Sub

    Private Sub PrintResults()
        Console.WriteLine("Total Carrier Power (dBm)  : {0}", totalCarrierPower)

        Console.WriteLine(vbLf & "Carrier Measurements :")

        For i As Integer = 0 To numberOfCarriers - 1
            Console.WriteLine(vbLf & "Carrier : {0}", i)
            Console.WriteLine("Absolute Power  (dBm)      : {0}", carrierAbsolutePower(i))
            Console.WriteLine("Relative Power  (dB)       : {0}", carrierRelativePower(i))
        Next
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
