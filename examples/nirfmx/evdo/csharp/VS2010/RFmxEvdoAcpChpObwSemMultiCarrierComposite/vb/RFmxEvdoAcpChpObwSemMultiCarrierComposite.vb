'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Contiguous Carriers.
'6. Select ACP, CHP, OBW and SEM measurements and enable Traces.
'7. Configure Sweep Time Parameters for ACP.
'8. Configure Averaging Parameters for ACP.
'9. Configure Sweep Time Parameters for CHP.
'10. Configure Averaging Parameters for CHP.
'11. Configure Sweep Time Parameters for OBW.
'12. Configure Averaging Parameters for OBW.
'13. Configure Sweep Time Parameters for SEM.
'14. Configure Averaging Parameters for SEM.
'15. Initiate the Measurement.
'16. Fetch ACP, CHP, OBW and SEM Measurements.
'17. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.EvdoMX

Public Class RFmxEvdoAcpChpObwSemMultiCarrierComposite
    Private instrSession As RFmxInstrMX
    Private evdo As RFmxEvdoMX

    Private resourceName As String = "RFSA"

    Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
    Private frequency As Double = 10000000.0                   ' Hz 
    Private centerFrequency As Double = 833490000.0            ' Hz 
    Private referenceLevel As Double = 0.0                     ' dBm 
    Private externalAttenuation As Double = 0.0                ' dB 
    Private enableTrigger As Boolean = False
    Private digitalEdgeSource As String = RFmxEvdoMXConstants.Pfi0
    Private digitalEdge As RFmxEvdoMXDigitalEdgeTriggerEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising
    Private triggerDelay As Double = 0.0                       ' seconds 
    Private timeout As Double = 10.0                           ' seconds 

    Private bandClass As Integer = 0
    Private frequencyReferenceCarrier As Integer = -1
    Private numberOfCarriers As Integer = 3

    Private acpSweepTimeAuto As RFmxEvdoMXAcpSweepTimeAuto = RFmxEvdoMXAcpSweepTimeAuto.[True]
    Private chpSweepTimeAuto As RFmxEvdoMXChpSweepTimeAuto = RFmxEvdoMXChpSweepTimeAuto.[True]
    Private obwSweepTimeAuto As RFmxEvdoMXObwSweepTimeAuto = RFmxEvdoMXObwSweepTimeAuto.[True]
    Private semSweepTimeAuto As RFmxEvdoMXSemSweepTimeAuto = RFmxEvdoMXSemSweepTimeAuto.[True]
    Private sweepTimeInterval As Double = 0.00167
    Private acpAveragingEnabled As RFmxEvdoMXAcpAveragingEnabled = RFmxEvdoMXAcpAveragingEnabled.[False]
    Private chpAveragingEnabled As RFmxEvdoMXChpAveragingEnabled = RFmxEvdoMXChpAveragingEnabled.[False]
    Private obwAveragingEnabled As RFmxEvdoMXObwAveragingEnabled = RFmxEvdoMXObwAveragingEnabled.[False]
    Private semAveragingEnabled As RFmxEvdoMXSemAveragingEnabled = RFmxEvdoMXSemAveragingEnabled.[False]
    Private averagingCount As Integer = 10
    Private acpAveragingType As RFmxEvdoMXAcpAveragingType = RFmxEvdoMXAcpAveragingType.Rms
    Private chpAveragingType As RFmxEvdoMXChpAveragingType = RFmxEvdoMXChpAveragingType.Rms
    Private obwAveragingType As RFmxEvdoMXObwAveragingType = RFmxEvdoMXObwAveragingType.Rms
    Private semAveragingType As RFmxEvdoMXSemAveragingType = RFmxEvdoMXSemAveragingType.Rms
    Private chpTotalAggregatedPower As Double
    Private acpTotalAggregatedPower As Double
    Private obwOccupiedBandwidth As Double
    Private obwAbsolutePower As Double
    Private obwStopFrequency As Double
    Private obwStartFrequency As Double
    Private semMeasurementStatus As RFmxEvdoMXSemCompositeMeasurementStatus
    Private semTotalAggregatedPower As Double

    Private chpAbsolutePower As Double()                      '(dBm) 
    Private chpRelativePower As Double()                      '(dB) 

    Private semAbsoluteIntegratedPower As Double()
    Private semRelativeIntegratedPower As Double()

    Private acpAbsolutePower As Double()
    Private acpRelativePower As Double()

    Private acpLowerAbsolutePower As Double()                '(dBm) 
    Private acpUpperAbsolutePower As Double()
    Private acpLowerRelativePower As Double()                '(dB) 
    Private acpUpperRelativePower As Double()

    Private semLowerOffsetMeasurementStatus As RFmxEvdoMXSemLowerOffsetMeasurementStatus()
    Private semLowerOffsetMargin As Double()                 '(dB) 
    Private semLowerOffsetMarginFrequency As Double()        '(Hz) 
    Private semLowerOffsetMarginAbsolutePower As Double()    '(dBm) 
    Private semLowerOffsetMarginRelativePower As Double()

    Private semUpperOffsetMeasurementStatus As RFmxEvdoMXSemUpperOffsetMeasurementStatus()
    Private semUpperOffsetMargin As Double()
    Private semUpperOffsetMarginFrequency As Double()
    Private semUpperOffsetMarginAbsolutePower As Double()
    Private semUpperOffsetMarginRelativePower As Double()

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

        evdo.SelectMeasurements("", RFmxEvdoMXMeasurementTypes.Acp Or RFmxEvdoMXMeasurementTypes.Chp Or
                                RFmxEvdoMXMeasurementTypes.Obw Or RFmxEvdoMXMeasurementTypes.Sem, True)
        evdo.Acp.Configuration.ConfigureSweepTime("", acpSweepTimeAuto, sweepTimeInterval)
        evdo.Acp.Configuration.ConfigureAveraging("", acpAveragingEnabled, averagingCount, acpAveragingType)
        evdo.Chp.Configuration.ConfigureSweepTime("", chpSweepTimeAuto, sweepTimeInterval)
        evdo.Chp.Configuration.ConfigureAveraging("", chpAveragingEnabled, averagingCount, chpAveragingType)
        evdo.Obw.Configuration.ConfigureSweepTime("", obwSweepTimeAuto, sweepTimeInterval)
        evdo.Obw.Configuration.ConfigureAveraging("", obwAveragingEnabled, averagingCount, obwAveragingType)
        evdo.Sem.Configuration.ConfigureSweepTime("", semSweepTimeAuto, sweepTimeInterval)
        evdo.Sem.Configuration.ConfigureAveraging("", semAveragingEnabled, averagingCount, semAveragingType)
        evdo.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        ' Retrieve results 


        'ACP
        evdo.Acp.Results.FetchOffsetMeasurementArray("", timeout, acpLowerRelativePower, acpUpperRelativePower, acpLowerAbsolutePower, acpUpperAbsolutePower)
        evdo.Acp.Results.FetchCarrierMeasurementArray("", timeout, acpAbsolutePower, acpRelativePower)
        evdo.Acp.Results.FetchTotalCarrierPower("", timeout, acpTotalAggregatedPower)

        'CHP
        evdo.Chp.Results.FetchCarrierMeasurementArray("", timeout, chpAbsolutePower, chpRelativePower)
        evdo.Chp.Results.FetchTotalCarrierPower("", timeout, chpTotalAggregatedPower)

        'OBW
        evdo.Obw.Results.FetchMeasurement("", timeout, obwOccupiedBandwidth, obwAbsolutePower, obwStartFrequency, obwStopFrequency)

        'SEM
        evdo.Sem.Results.FetchLowerOffsetMarginArray("", timeout, semLowerOffsetMeasurementStatus, semLowerOffsetMargin,
                                                     semLowerOffsetMarginFrequency, semLowerOffsetMarginAbsolutePower, semLowerOffsetMarginRelativePower)
        evdo.Sem.Results.FetchUpperOffsetMarginArray("", timeout, semUpperOffsetMeasurementStatus, semUpperOffsetMargin,
                                                     semUpperOffsetMarginFrequency, semUpperOffsetMarginAbsolutePower, semUpperOffsetMarginRelativePower)

        evdo.Sem.Results.FetchCarrierMeasurementArray("", timeout, semAbsoluteIntegratedPower, semRelativeIntegratedPower)

        evdo.Sem.Results.FetchMeasurementStatus("", timeout, semMeasurementStatus)
        evdo.Sem.Results.FetchTotalCarrierPower("", timeout, semTotalAggregatedPower)

    End Sub

    Private Sub PrintResults()

        Console.WriteLine(vbLf & "************************* ACP *************************" & vbLf & vbLf)
        Console.WriteLine("Total Carrier Power  (dBm)" & vbTab & " : {0}", acpTotalAggregatedPower)
        Console.WriteLine(vbLf & "Carrier Measurements" & vbTab & "         : " & vbLf)
        For i As Integer = 0 To acpAbsolutePower.Length - 1
            Console.WriteLine(vbLf & "Carrier                          : {0}", i)
            Console.WriteLine("Absolute Power  (dBm)            : {0}", acpAbsolutePower(i))
            Console.WriteLine("Relative Power  (dB)             : {0}", acpRelativePower(i))
        Next
        Console.WriteLine(vbLf & "Offset Channel Measurements" & vbTab & " : " & vbLf)
        For i As Integer = 0 To acpLowerRelativePower.Length - 1
            Console.WriteLine(vbLf & "Offset                           : {0}", i)
            Console.WriteLine(vbLf & "Lower Relative Power (dB)        : {0}", acpLowerRelativePower(i))
            Console.WriteLine("Upper Relative Power (dB)        : {0}", acpUpperRelativePower(i))
            Console.WriteLine("Lower Absolute Power (dBm)       : {0}", acpLowerAbsolutePower(i))
            Console.WriteLine("Upper Absolute Power (dBm)       : {0}", acpUpperAbsolutePower(i))
        Next

        Console.WriteLine(vbLf & "************************* CHP *************************" & vbLf & vbLf)
        Console.WriteLine("Total Carrier Power  (dBm)" & vbTab & " : {0}", chpTotalAggregatedPower)
        Console.WriteLine(vbLf & "Carrier Measurements             : " & vbLf)
        For i As Integer = 0 To chpAbsolutePower.Length - 1
            Console.WriteLine("Carrier                          : {0}", i)
            Console.WriteLine("Absolute Power  (dBm)            : {0}", chpAbsolutePower(i))
            Console.WriteLine("Relative Power  (dB)             : {0}", chpRelativePower(i))
        Next


        Console.WriteLine(vbLf & "************************* OBW *************************" & vbLf & vbLf)
        Console.WriteLine("Occupied Bandwidth  (Hz)" & vbTab & " : {0}", obwOccupiedBandwidth)
        Console.WriteLine("Absolute Power  (dBm)" & vbTab & vbTab & " : {0}", obwAbsolutePower)
        Console.WriteLine("Start Frequency  (Hz)" & vbTab & vbTab & " : {0}", obwStartFrequency)
        Console.WriteLine("Stop Frequency  (Hz)" & vbTab & vbTab & " : {0}", obwStopFrequency)

        Console.WriteLine(vbLf & "************************* SEM *************************" & vbLf & vbLf)
        Console.WriteLine("Measurement Status               : {0}", semMeasurementStatus)
        Console.WriteLine("Total Carrier Power  (dBm)" & vbTab & " : {0}", semTotalAggregatedPower)
        Console.WriteLine(vbLf & "Carrier Measurements" & vbTab & "         : " & vbLf)
        For i As Integer = 0 To semAbsoluteIntegratedPower.Length - 1
            Console.WriteLine(vbLf & "Carrier                          : {0}", i)
            Console.WriteLine("Absolute Integrated Power  (dBm) : {0}", semAbsoluteIntegratedPower(i))
            Console.WriteLine("Relative Integrated Power  (dB)  : {0}", semRelativeIntegratedPower(i))
        Next
        Console.WriteLine(vbLf & "Lower Offset Segment Measurements: " & vbLf)

        For i As Integer = 0 To semLowerOffsetMargin.Length - 1
            Console.WriteLine(vbLf & "Offset                           : {0}", i)
            Console.WriteLine("Margin  (dB)                     : {0}", semLowerOffsetMargin(i))
            Console.WriteLine("Margin Absolute Power  (dBm)     : {0}", semLowerOffsetMarginAbsolutePower(i))
            Console.WriteLine("Margin Relative Power  (dB)      : {0}", semLowerOffsetMarginRelativePower(i))
            Console.WriteLine("Margin Frequency  (Hz)           : {0}", semLowerOffsetMarginFrequency(i))
            Console.WriteLine("Measurement Status               : {0}", semLowerOffsetMeasurementStatus(i))
        Next
        Console.WriteLine(vbLf & "Upper Offset Segment Measurements: " & vbLf)

        For i As Integer = 0 To semUpperOffsetMeasurementStatus.Length - 1
            Console.WriteLine(vbLf & "Offset                           : {0}", i)
            Console.WriteLine("Margin  (dB)                     : {0}", semUpperOffsetMargin(i))
            Console.WriteLine("Margin Absolute Power  (dBm)     : {0}", semUpperOffsetMarginAbsolutePower(i))
            Console.WriteLine("Margin Relative Power  (dB)      : {0}", semUpperOffsetMarginRelativePower(i))
            Console.WriteLine("Margin Frequency  (Hz)           : {0}", semUpperOffsetMarginFrequency(i))
            Console.WriteLine("Measurement Status               : {0}", semUpperOffsetMeasurementStatus(i))
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
