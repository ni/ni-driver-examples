'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Select ModAcc measurement and enable traces.
'6. Configure Synchronization Mode and Measurement Interval.
'7. Configure Channel Configuration Mode.
'8. Configure Physical Layer Subtype.
'9. Configure Uplink Data Modulation Type.
'10. Configure Uplink Spreading Parameters.
'11. Initiate the Measurement.
'12. Fetch ModAcc Measurements and Traces.
'13. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.EvdoMX

Public Class RFmxEvdoModAcc
    Private instrSession As RFmxInstrMX
    Private evdo As RFmxEvdoMX

    Private resourceName As String = "RFSA"
    Private measurement As RFmxEvdoMXMeasurementTypes = RFmxEvdoMXMeasurementTypes.ModAcc
    Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
    Private frequency As Double = 10000000.0                            ' Hz 
    Private centerFrequency As Double = 833490000.0                     ' Hz 
    Private externalAttenuation As Double = 0.0                         ' dBm 

    Private digitalEdgeSource As String = RFmxEvdoMXConstants.Pfi0
    Private digitalEdge As RFmxEvdoMXDigitalEdgeTriggerEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising
    Private triggerDelay As Double = 0.0                                ' seconds 
    Private referenceLevel As Double = 0.0                              ' dBm 
    Private timeout As Double = 10.0                                    ' seconds 

    Private enableAllTraces As Boolean = True
    Private enableTrigger As Boolean = False

    Private channelConfigurationMode As RFmxEvdoMXChannelConfigurationMode = RFmxEvdoMXChannelConfigurationMode.AutoDetect

    Private physicalLayerSubtype As RFmxEvdoMXPhysicalLayerSubtype = RFmxEvdoMXPhysicalLayerSubtype.Subtype0_1

    Private uplinkDataModulationType As RFmxEvdoMXUplinkDataModulationType = RFmxEvdoMXUplinkDataModulationType.Auto
    Private uplinkSpreadingIMask As Long = &H0
    Private uplinkSpreadingQMask As Long = &H0

    Private synchronizationMode As RFmxEvdoMXModAccSynchronizationMode = RFmxEvdoMXModAccSynchronizationMode.Slot
    Private measurementOffset As Integer = 0
    Private measurementLength As Integer = 1

    Private detectedDataModulationType As RFmxEvdoMXModAccUplinkDetectedDataModulationType
    Private rmsEvm As Double
    Private peakEvm As Double
    Private Rho As Double
    Private frequencyError As Double
    Private chipRateError As Double
    Private rmsMagnitudeError As Double
    Private rmsPhaseError As Double
    Private iqOriginOffset As Double
    Private iqGainImbalance As Double
    Private iqQuadratureError As Double
    Private peakCde As Double
    Private peakCdeWalshCodeNumber As Integer
    Private peakCdeBranch As RFmxEvdoMXModAccUplinkPeakCdeBranch
    Private peakActiveCde As Double
    Private peakActiveCdeWalshCodeLength As Integer
    Private peakActiveCdeWalshCodeNumber As Integer
    Private peakActiveCdeBranch As RFmxEvdoMXModAccUplinkPeakActiveCdeBranch
    Private evm As AnalogWaveform(Of Single)
    Private constellation As ComplexSingle()

    Public Sub Run()
        Try
            InitializeInstr()
            ConfigureEvdo()
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

    Private Sub ConfigureEvdo()
        ' Get Evdo signal 

        evdo = instrSession.GetEvdoSignalConfiguration()

        ' Configure measurement 

        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequency)
        evdo.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)

        evdo.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)


        evdo.SelectMeasurements("", measurement, enableAllTraces)

        evdo.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset,
                                                                          measurementLength)

        evdo.ConfigureChannelConfigurationMode("", channelConfigurationMode)
        evdo.ConfigurePhysicalLayerSubtype("", physicalLayerSubtype)
        evdo.ConfigureUplinkDataModulationType("", uplinkDataModulationType)
        evdo.ConfigureUplinkSpreading("", uplinkSpreadingIMask, uplinkSpreadingQMask)
        evdo.Initiate("", "")

    End Sub

    Private Sub RetrieveResults()
        ' Retrieve results 


        evdo.ModAcc.Results.FetchUplinkEvm("", timeout, rmsEvm, peakEvm, Rho, frequencyError, _
         chipRateError, rmsMagnitudeError, rmsPhaseError)
        evdo.ModAcc.Results.FetchIQImpairments("", timeout, iqOriginOffset, iqGainImbalance, iqQuadratureError)
        evdo.ModAcc.Results.FetchUplinkPeakCde("", timeout, peakCde, peakCdeWalshCodeNumber, peakCdeBranch)
        evdo.ModAcc.Results.FetchUplinkPeakActiveCde("", timeout, peakActiveCde, peakActiveCdeWalshCodeLength,
                                                     peakActiveCdeWalshCodeNumber, peakActiveCdeBranch)
        evdo.ModAcc.Results.FetchUplinkDetectedDataModulationType("", timeout, detectedDataModulationType)
        evdo.ModAcc.Results.FetchEvmTrace("", timeout, evm)
        evdo.ModAcc.Results.FetchConstellationTrace("", timeout, constellation)
    End Sub

    Private Sub PrintResults()

        Console.WriteLine(vbLf & "***************************************************:")
        Console.WriteLine("EVM:")
        Console.WriteLine("RMS EVM (%)                                      : {0}", rmsEvm)
        Console.WriteLine("Peak EVM (%)                                     : {0}", peakEvm)
        Console.WriteLine("Rho                                              : {0}", Rho)
        Console.WriteLine("Frequency Error (Hz)                             : {0}", frequencyError)
        Console.WriteLine("Chip Rate Error (ppm)                            : {0}", chipRateError)
        Console.WriteLine("RMS Magnitude Error (%)                          : {0}", rmsMagnitudeError)
        Console.WriteLine("RMS Phase Error (deg)                            : {0}", rmsPhaseError)

        Console.WriteLine(vbLf & "***************************************************:")
        Console.WriteLine("IQ Impairments:")
        Console.WriteLine("I/Q Origin Offset (dB)                           : {0}", iqOriginOffset)
        Console.WriteLine("I/Q Gain Imbalance (dB)                          : {0}", iqGainImbalance)
        Console.WriteLine("I/Q Quadrature Error (deg)                       : {0}", iqQuadratureError)



        Console.WriteLine(vbLf & "***************************************************:")
        Console.WriteLine("Code Domain Error:")
        Console.WriteLine("Peak CDE (dB)                                    : {0}", peakCde)
        Console.WriteLine("Peak CDE Code                                    : {0}", peakCdeWalshCodeNumber)
        Console.WriteLine("Peak CDE Branch                                  : {0}", peakCdeBranch)
        Console.WriteLine("Peak Active CDE (dB)                             : {0}", peakActiveCde)
        Console.WriteLine("Peak Active CDE Walsh Code Number                : {0}", peakActiveCdeWalshCodeNumber)
        Console.WriteLine("Peak Active CDE Walsh Code Length                : {0}", peakActiveCdeWalshCodeLength)
        Console.WriteLine("Peak Active CDE Branch                           : {0}", peakActiveCdeBranch)

        Console.WriteLine(vbLf & "***************************************************:")
        Console.WriteLine("Uplink Data Modulation:")
        Console.WriteLine("Uplink Detected Data Modulation Type             : {0}", detectedDataModulationType)
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
