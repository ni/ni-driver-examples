'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Band Class.
'6. Select SEM measurement and enable traces.
'7. Configure Sweep Time Parameters.
'8. Configure Averaging Parameters.
'9. Initiate the Measurement.
'10. Fetch SEM Measurements and Traces.
'11. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.EvdoMX

Public Class RFmxEvdoSemSingleCarrier
    Private instrSession As RFmxInstrMX
    Private evdo As RFmxEvdoMX

    Private resourceName As String = "RFSA"

    Private measurement As RFmxEvdoMXMeasurementTypes = RFmxEvdoMXMeasurementTypes.Sem

    Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
    Private frequencyReferenceFrequency As Double = 10000000.0               ' Hz 

    Private centerFrequency As Double = 833490000.0                          ' Hz 
    Private externalAttenuation As Double = 0.0                              ' dB 
    Private referenceLevel As Double = 0.0                                   ' dBm 

    Private digitalEdgeSource As String = RFmxEvdoMXConstants.Pfi0
    Private digitalEdge As RFmxEvdoMXDigitalEdgeTriggerEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising
    Private triggerDelay As Double = 0.0                                     ' seconds 

    Private averagingEnabled As RFmxEvdoMXSemAveragingEnabled = RFmxEvdoMXSemAveragingEnabled.[False]
    Private averagingCount As Integer = 10
    Private averagingType As RFmxEvdoMXSemAveragingType = RFmxEvdoMXSemAveragingType.Rms

    Private sweepTimeAuto As RFmxEvdoMXSemSweepTimeAuto = RFmxEvdoMXSemSweepTimeAuto.[True]
    Private sweepTimeInterval As Double = 0.00167                           ' seconds 
    Private timeout As Double = 10                                          ' seconds 

    Private enableAllTraces As Boolean = True
    Private enableTrigger As Boolean = False

    Private totalCarrierPower As Double
    Private spectrum As Spectrum(Of Single), relativeMask As Spectrum(Of Single), absoluteMask As Spectrum(Of Single)
    Private band As Integer = 0
    Private lowerOffsetMeasurementStatus As RFmxEvdoMXSemLowerOffsetMeasurementStatus()
    Private lowerOffsetMargin As Double()
    Private lowerOffsetMarginFrequency As Double()
    Private lowerOffsetMarginAbsolutePower As Double()
    Private lowerOffsetMarginRelativePower As Double()

    Private upperOffsetMeasurementStatus As RFmxEvdoMXSemUpperOffsetMeasurementStatus()
    Private upperOffsetMargin As Double()
    Private upperOffsetMarginFrequency As Double()
    Private upperOffsetMarginAbsolutePower As Double()
    Private upperOffsetMarginRelativePower As Double()
    Private measurementStatus As RFmxEvdoMXSemCompositeMeasurementStatus


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

        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
        evdo.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
        evdo.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
        evdo.ConfigureBandClass("", band)
        evdo.SelectMeasurements("", measurement, enableAllTraces)
        evdo.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
        evdo.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
        evdo.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        ' Retrieve results 

        evdo.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffsetMeasurementStatus, lowerOffsetMargin, lowerOffsetMarginFrequency,
                                                     lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower)
        evdo.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffsetMeasurementStatus, upperOffsetMargin, upperOffsetMarginFrequency,
                                                     upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower)
        evdo.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)
        evdo.Sem.Results.FetchTotalCarrierPower("", timeout, totalCarrierPower)
        evdo.Sem.Results.FetchSpectrum("", timeout, spectrum, relativeMask, absoluteMask)
    End Sub

    Private Sub PrintResults()
        Console.WriteLine("Measurement Status                : {0}", measurementStatus)
        Console.WriteLine("Carrier Absolute Power (dBm)      : {0}", totalCarrierPower)
        Console.WriteLine(vbLf & "Lower Offset Segment Measurements" & vbLf)
        For i As Integer = 0 To lowerOffsetMargin.Length - 1
            Console.WriteLine(vbLf & "Measurement {0}", i)
            Console.WriteLine("Margin (dB)                         : {0}", lowerOffsetMargin(i))
            Console.WriteLine("Margin Absolute Power (dBm)         : {0}", lowerOffsetMarginAbsolutePower(i))
            Console.WriteLine("Margin Relative Power (dB)          : {0}", lowerOffsetMarginRelativePower(i))
            Console.WriteLine("Margin Frequency (Hz)               : {0}", lowerOffsetMarginFrequency(i))
            Console.WriteLine("Measurement Status                  : {0}", lowerOffsetMeasurementStatus(i))
        Next

        Console.WriteLine(vbLf & "Upper Offset Segment Measurements" & vbLf)
        For i As Integer = 0 To upperOffsetMargin.Length - 1
            Console.WriteLine(vbLf & "Measurement {0}", i)
            Console.WriteLine("Margin (dB)                         : {0}", upperOffsetMargin(i))
            Console.WriteLine("Margin Absolute Power (dBm)         : {0}", upperOffsetMarginAbsolutePower(i))
            Console.WriteLine("Margin Relative Power (dB)          : {0}", upperOffsetMarginRelativePower(i))
            Console.WriteLine("Margin Frequency (Hz)               : {0}", upperOffsetMarginFrequency(i))
            Console.WriteLine("Measurement Status                  : {0}", upperOffsetMeasurementStatus(i))
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
