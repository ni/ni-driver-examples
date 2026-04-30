'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure RF Attenuation.
'4. Configure the basic signal properties (External Attenuation, Center Frequency and Reference Level).
'5. Configure Trigger Type and Trigger Parameters.
'6. Select ACP measurement and enable traces.
'7. Configure Band Class.
'8. Configure Measurement Method.
'9. Configure Averaging Parameters.
'10. Configure Sweep Time Parameters.
'11. Configure Noise Compensation Parameter.
'12. Configure Number of Offsets.
'13. Initiate the Measurement.
'14. Fetch ACP Measurements and Traces.
'15. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.EvdoMX

Public Class RFmxEvdoAcpSingleCarrier
    Private instrSession As RFmxInstrMX
    Private evdo As RFmxEvdoMX

    Private resourceName As String = "RFSA"
    Private measurement As RFmxEvdoMXMeasurementTypes = RFmxEvdoMXMeasurementTypes.Acp

    Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
    Private frequency As Double = 10000000.0                              ' Hz 
    Private centerFrequency As Double = 833490000.0                       ' Hz 
    Private externalAttenuation As Double = 0.0                           ' dB 

    Private rfAttenuationAuto As RFmxInstrMXRFAttenuationAuto = RFmxInstrMXRFAttenuationAuto.[True]
    Private rfAttenuation As Double = 10.0

    Private digitalEdgeSource As String = RFmxEvdoMXConstants.Pfi0
    Private digitalEdge As RFmxEvdoMXDigitalEdgeTriggerEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising
    Private triggerDelay As Double = 0.0                                 ' seconds 

    Private measurementInterval As Double = 0.02667                      ' seconds 
    Private referenceLevel As Double                                     ' dBm 

    Private measurementMethod As RFmxEvdoMXAcpMeasurementMethod = RFmxEvdoMXAcpMeasurementMethod.Normal
    Private averagingEnabled As RFmxEvdoMXAcpAveragingEnabled = RFmxEvdoMXAcpAveragingEnabled.[False]
    Private averagingCount As Integer = 10
    Private averagingType As RFmxEvdoMXAcpAveragingType = RFmxEvdoMXAcpAveragingType.Rms

    Private autoLevel As Boolean = True

    Private sweepTimeAuto As RFmxEvdoMXAcpSweepTimeAuto = RFmxEvdoMXAcpSweepTimeAuto.[True]
    Private sweepTimeInterval As Double = 0.00167                        ' seconds 

    Private noiseCompensationEnabled As RFmxEvdoMXAcpNoiseCompensationEnabled = RFmxEvdoMXAcpNoiseCompensationEnabled.[False]

    Private numberOfOffsets As Integer = 2
    Private bandClass As Integer = 0

    Private timeout As Double = 10.0                                    ' seconds 

    Private enableAllTraces As Boolean = True
    Private enableTrigger As Boolean = False

    Private absolutePower As Double                                     ' dBm 
    Private relativePower As Double                                     ' dB 

    Private spectrum As Spectrum(Of Single)

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
        instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation)
        evdo.ConfigureExternalAttenuation("", externalAttenuation)
        evdo.ConfigureFrequency("", centerFrequency)

        If autoLevel Then
            evdo.AutoLevel("", measurementInterval, referenceLevel)
            Console.WriteLine("Reference Level (dBm)             : {0}", referenceLevel)
        Else
            evdo.ConfigureReferenceLevel("", referenceLevel)
        End If

        evdo.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)

        evdo.SelectMeasurements("", measurement, enableAllTraces)
        evdo.ConfigureBandClass("", bandClass)
        evdo.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod)
        evdo.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
        evdo.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
        evdo.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled)
        evdo.Acp.Configuration.ConfigureNumberOfOffsets("", numberOfOffsets)
        evdo.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        ' Retrieve results 


        evdo.Acp.Results.FetchOffsetMeasurementArray("", timeout, lowerRelativePower, upperRelativePower, lowerAbsolutePower,
                                                     upperAbsolutePower)
        evdo.Acp.Results.FetchCarrierMeasurement("", timeout, absolutePower, relativePower)

        evdo.Acp.Results.FetchSpectrum("", timeout, spectrum)
    End Sub

    Private Sub PrintResults()
        Console.WriteLine("Carrier Absolute Power  (dBm)     : {0}", absolutePower)
        Console.WriteLine(vbLf & "Offset Channel Measurements:")
        For i As Integer = 0 To lowerRelativePower.Length - 1
            Console.WriteLine(vbLf & "Offset  {0}", i)
            Console.WriteLine("Lower Relative Power (dB)        : {0}", lowerRelativePower(i))
            Console.WriteLine("Upper Relative Power (dB)        : {0}", upperRelativePower(i))
            Console.WriteLine("Lower Absolute Power (dBm)       : {0}", lowerAbsolutePower(i))
            Console.WriteLine("Upper Absolute Power (dBm)       : {0}", upperAbsolutePower(i))
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
