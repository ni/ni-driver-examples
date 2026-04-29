'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties  (Center Frequency and External Attenuation).
'4. Configure RF Attenuation.
'5. Configure Trigger Type and Trigger Parameters.
'6. Configure Contiguous Carriers.
'7. Confiure Reference Level. 
'8. Select ACP measurement and enable traces.
'9. Configure Measurement Method Parameter.
'10. Configure Averaging Parameters.
'11. Configure Sweep Time Parameters.
'12. Configure Noise Compensation Parameter.
'13. Configure Number of Offsets.
'14. Configure Offset Power Reference Parameters.
'15. Initiate the Measurement.
'16. Fetch ACP Measurements and Traces.
'17. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.EvdoMX

Public Class RFmxEvdoAcpMultiCarrier
    Private instrSession As RFmxInstrMX
    Private evdo As RFmxEvdoMX

    Private resourceName As String = "RFSA"

    Private measurement As RFmxEvdoMXMeasurementTypes = RFmxEvdoMXMeasurementTypes.Acp

    Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
    Private frequency As Double = 10000000.0                           ' Hz 

    Private centerFrequency As Double = 833490000.0                    ' Hz 
    Private externalAttenuation As Double = 0.0                        ' dB 

    Private rfAttenuationAuto As RFmxInstrMXRFAttenuationAuto = RFmxInstrMXRFAttenuationAuto.[True]
    Private rfAttenuation As Double = 10.0

    Private digitalEdgeSource As String = RFmxEvdoMXConstants.Pfi0
    Private digitalEdge As RFmxEvdoMXDigitalEdgeTriggerEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising
    Private triggerDelay As Double = 0.0                               ' seconds 

    Private measurementInterval As Double = 0.02667                    ' seconds 
    Private referenceLevel As Double = 0.0                             ' dBm 

    Private measurementMethod As RFmxEvdoMXAcpMeasurementMethod = RFmxEvdoMXAcpMeasurementMethod.Normal

    Private averagingEnabled As RFmxEvdoMXAcpAveragingEnabled = RFmxEvdoMXAcpAveragingEnabled.[False]
    Private averagingCount As Integer = 10
    Private averagingType As RFmxEvdoMXAcpAveragingType = RFmxEvdoMXAcpAveragingType.Rms

    Private autoLevel As Boolean = True

    Private sweepTimeAuto As RFmxEvdoMXAcpSweepTimeAuto = RFmxEvdoMXAcpSweepTimeAuto.[True]
    Private sweepTimeInterval As Double = 0.00167                     ' seconds 

    Private noiseCompensationEnabled As RFmxEvdoMXAcpNoiseCompensationEnabled = RFmxEvdoMXAcpNoiseCompensationEnabled.[False]

    Private numberOfOffsets As Integer = 2

    Private enableAllTraces As Boolean = True
    Private enableTrigger As Boolean = False

    Private numberOfCarriers As Integer = 3
    Private frequencyReferenceCarrier As Integer = -1
    Private bandClass As Integer = 0

    Private offsetPowerReferenceCarrier As RFmxEvdoMXAcpOffsetPowerReferenceCarrier = RFmxEvdoMXAcpOffsetPowerReferenceCarrier.Composite
    Private offsetPowerReferenceSpecific As Integer = 0

    Private timeout As Double = 10.0                                  ' seconds 
    Private spectrum As Spectrum(Of Single)
    Private totalCarrierPower As Double = 0

    Private carrierAbsolutePower As Double(), carrierRelativePower As Double()
    Private lowerAbsolutePower As Double(), upperAbsolutePower As Double(), lowerRelativePower As Double(), upperRelativePower As Double()

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
        evdo.ConfigureFrequency("", centerFrequency)
        evdo.ConfigureExternalAttenuation("", externalAttenuation)
        instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation)

        evdo.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
        evdo.ConfigureContiguousCarriers("", numberOfCarriers, frequencyReferenceCarrier, bandClass)

        If autoLevel Then
            evdo.AutoLevel("", measurementInterval, referenceLevel)
            Console.WriteLine("Reference Level (dBm)          : {0}", referenceLevel)
        Else
            evdo.ConfigureReferenceLevel("", referenceLevel)
        End If

        evdo.SelectMeasurements("", measurement, enableAllTraces)
        evdo.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod)
        evdo.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
        evdo.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
        evdo.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled)
        evdo.Acp.Configuration.ConfigureNumberOfOffsets("", numberOfOffsets)
        evdo.Acp.Configuration.ConfigureOffsetPowerReference("", offsetPowerReferenceCarrier, offsetPowerReferenceSpecific)
        evdo.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        ' Retrieve results 
        evdo.Acp.Results.FetchOffsetMeasurementArray("", timeout, lowerRelativePower, upperRelativePower, lowerAbsolutePower, upperAbsolutePower)
        evdo.Acp.Results.FetchTotalCarrierPower("", timeout, totalCarrierPower)
        evdo.Acp.Results.FetchCarrierMeasurementArray("", timeout, carrierAbsolutePower, carrierRelativePower)
        evdo.Acp.Results.FetchSpectrum("", timeout, spectrum)
    End Sub

    Private Sub PrintResults()
        Console.WriteLine("Total Carrier Power (dBm)      : {0}", totalCarrierPower)

        Console.WriteLine(vbLf & "Carrier Measurements:")
        For i As Integer = 0 To numberOfCarriers - 1
            Console.WriteLine(vbLf & "Carrier: {0}", i)
            Console.WriteLine("Absolute Power (dBm)           : {0}", carrierAbsolutePower(i))
            Console.WriteLine("Relative Power (dB)            : {0}", carrierRelativePower(i))
        Next

        Console.WriteLine(vbLf & "Offset Channel Measurements:")
        For i As Integer = 0 To lowerRelativePower.Length - 1
            Console.WriteLine(vbLf & "Offset  {0}", i)
            Console.WriteLine("Lower Relative Power (dB)      : {0}", lowerRelativePower(i))
            Console.WriteLine("Upper Relative Power (dB)      : {0}", upperRelativePower(i))
            Console.WriteLine("Lower Absolute Power (dBm)     : {0}", lowerAbsolutePower(i))
            Console.WriteLine("Upper Absolute Power (dBm)     : {0}", upperAbsolutePower(i))
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
