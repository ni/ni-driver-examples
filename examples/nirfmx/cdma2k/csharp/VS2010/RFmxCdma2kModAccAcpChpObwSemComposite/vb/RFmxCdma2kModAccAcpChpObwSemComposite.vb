'Steps:
'1. Open a new RFmx session.
'2. Configure Frequency Reference.
'3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Band Class Parameter.
'6. Configure Radio Configuration Parameter.
'7. Configure Uplink Spreading Long Code Mask Parameter.
'8. Select ModAcc, ACP, CHP, OBW and SEM measurements and enable Traces.
'9. Configure Synchronization Mode and Measurement Interval.
'10. Configure Sweep Time Parameters.
'11. Configure Averaging Parameters.
'12. Initiate the Measurement.
'13. Fetch SEM Measurements and Traces.
'14. Fetch OBW Measurements and Traces.
'15. Fetch CHP Measurements and Traces.
'16. Fetch ACP Measurements and Traces.
'17. Fetch ModAcc Measurements and Traces.
'18. Close the RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.Cdma2kMX

Public Class RFmxCdma2kModAccAcpChpObwSemComposite
    Private instrSession As RFmxInstrMX
    Private cdma2k As RFmxCdma2kMX
    Private resourceName As String, frequencySource As String
    Private centerFrequency As Double, referenceLevel As Double, externalAttenuation As Double, frequency As Double, sweepTimeInterval As Double

    Private averagingCount As Integer

    Private digitalEdgeTriggerSource As String
    Private digitalEdgeTriggerEdge As RFmxCdma2kMXDigitalEdgeTriggerEdge
    Private triggerDelay As Double
    Private enableTrigger As Boolean

    Private bandclass As Integer

    Private radioConfiguration As RFmxCdma2kMXRadioConfiguration

    Private uplinkSpreadingLongCodeMask As Long

    Private synchronizationMode As RFmxCdma2kMXModAccSynchronizationMode
    Private measurementOffset As Integer, measurementLength As Integer

    Private timeout As Double

    'modacc results
    Private rmsEvm As Double, peakEvm As Double, rho As Double, frequencyError As Double, chipRateError As Double, rmsMagnitudeError As Double, _
     rmsPhaseError As Double

    'ACP results
    Private carrierAbsolutePowerAcp As Double
    Private lowerRelativePower As Double()
    Private upperRelativePower As Double()
    Private lowerAbsolutePower As Double()
    Private upperAbsolutePower As Double()

    'CHP results
    Private carrierAbsolutePowerChp As Double

    'OBW results
    Private stopFrequency As Double, startFrequency As Double, occupiedBandwidth As Double, absolutePower As Double

    'SEM results
    Private carrierAbsoluteIntegratedPower As Double
    Private lowerOffsetMargin As Double()
    Private lowerOffsetMarginAbsolutePower As Double()
    Private lowerOffsetMarginRelativePower As Double()
    Private lowerOffsetMarginFrequency As Double()
    Private lowerOffsetMeasurementStatus As RFmxCdma2kMXSemLowerOffsetMeasurementStatus()

    Private measurementStatus As RFmxCdma2kMXSemMeasurementStatus

    Private upperOffsetMargin As Double()
    Private upperOffsetMarginAbsolutePower As Double()
    Private upperOffsetMarginRelativePower As Double()
    Private upperOffsetMarginFrequency As Double()
    Private upperOffsetMeasurementStatus As RFmxCdma2kMXSemUpperOffsetMeasurementStatus()

    Public Sub Run()
        Try
            InitializeVariables()
            InitializeInstr()
            ConfigureCdma2k()
            RetrieveResults()
            PrintResults()
        Catch ex As Exception
            DisplayError(ex)
        Finally
            ' Close session 

            CloseSession()
            Console.WriteLine("Press any key to exit.....")
            Console.ReadKey()
        End Try
    End Sub

    Private Sub InitializeVariables()
        ' Initialize input variables 


        resourceName = "RFSA"

        centerFrequency = 833490000.0
        ' Hz 
        referenceLevel = 0.0
        ' dBm 
        externalAttenuation = 0.0
        ' dB 

        frequencySource = RFmxInstrMXConstants.OnboardClock
        frequency = 10000000.0
        ' Hz 

        triggerDelay = 0.0
        ' seconds 

        digitalEdgeTriggerSource = RFmxCdma2kMXConstants.Pfi0
        digitalEdgeTriggerEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising
        enableTrigger = False

        bandclass = 0
        radioConfiguration = RFmxCdma2kMXRadioConfiguration.RC3
        uplinkSpreadingLongCodeMask = 0

        'Sweep Time
        sweepTimeInterval = 0.00167
        ' seconds 

        'Averaging
        averagingCount = 10

        synchronizationMode = RFmxCdma2kMXModAccSynchronizationMode.Slot

        measurementOffset = 0
        measurementLength = 1
        timeout = 10
        ' seconds 
    End Sub

    Private Sub InitializeInstr()
        ' Create a new RFmx Session 

        instrSession = New RFmxInstrMX(resourceName, "")
    End Sub

    Private Sub ConfigureCdma2k()
        ' Get SpecAn signal 

        cdma2k = instrSession.GetCdma2kSignalConfiguration()

        ' Configure measurement 

        instrSession.ConfigureFrequencyReference("", frequencySource, frequency)
        cdma2k.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
        cdma2k.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger)
        cdma2k.ConfigureBandClass("", bandclass)
        cdma2k.ConfigureRadioConfiguration("", radioConfiguration)
        cdma2k.ConfigureUplinkSpreading("", uplinkSpreadingLongCodeMask)

        cdma2k.SelectMeasurements("", RFmxCdma2kMXMeasurementTypes.ModAcc Or RFmxCdma2kMXMeasurementTypes.Acp Or
                                  RFmxCdma2kMXMeasurementTypes.Chp Or RFmxCdma2kMXMeasurementTypes.Obw Or
                                  RFmxCdma2kMXMeasurementTypes.Sem, False)

        cdma2k.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset,
                                                                            measurementLength)

        cdma2k.Acp.Configuration.ConfigureSweepTime("", RFmxCdma2kMXAcpSweepTimeAuto.[False], sweepTimeInterval)
        cdma2k.Acp.Configuration.ConfigureAveraging("", RFmxCdma2kMXAcpAveragingEnabled.[False], averagingCount,
                                                    RFmxCdma2kMXAcpAveragingType.Rms)

        cdma2k.Chp.Configuration.ConfigureSweepTime("", RFmxCdma2kMXChpSweepTimeAuto.[False], sweepTimeInterval)
        cdma2k.Chp.Configuration.ConfigureAveraging("", RFmxCdma2kMXChpAveragingEnabled.[False], averagingCount,
                                                    RFmxCdma2kMXChpAveragingType.Rms)

        cdma2k.Obw.Configuration.ConfigureSweepTime("", RFmxCdma2kMXObwSweepTimeAuto.[False], sweepTimeInterval)
        cdma2k.Obw.Configuration.ConfigureAveraging("", RFmxCdma2kMXObwAveragingEnabled.[False], averagingCount,
                                                    RFmxCdma2kMXObwAveragingType.Rms)

        cdma2k.Sem.Configuration.ConfigureSweepTime("", RFmxCdma2kMXSemSweepTimeAuto.[False], sweepTimeInterval)
        cdma2k.Sem.Configuration.ConfigureAveraging("", RFmxCdma2kMXSemAveragingEnabled.[False], averagingCount, RFmxCdma2kMXSemAveragingType.Rms)

        cdma2k.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        ' Retrieve results 



        cdma2k.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffsetMeasurementStatus, lowerOffsetMargin,
                                                      lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower, _
         lowerOffsetMarginRelativePower)

        cdma2k.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffsetMeasurementStatus, upperOffsetMargin,
                                                    upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower, _
         upperOffsetMarginRelativePower)
        cdma2k.Sem.Results.FetchCarrierAbsoluteIntegratedPower("", timeout, carrierAbsoluteIntegratedPower)
        cdma2k.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)

        cdma2k.Obw.Results.FetchMeasurement("", timeout, occupiedBandwidth, absolutePower, startFrequency, stopFrequency)


        cdma2k.Chp.Results.FetchCarrierAbsolutePower("", timeout, carrierAbsolutePowerChp)

        cdma2k.Acp.Results.FetchOffsetMeasurementArray("", timeout, lowerRelativePower, upperRelativePower,
                                                    lowerAbsolutePower, upperAbsolutePower)

        cdma2k.Acp.Results.FetchCarrierAbsolutePower("", timeout, carrierAbsolutePowerAcp)


        cdma2k.ModAcc.Results.FetchEvm("", timeout, rmsEvm, peakEvm, rho, frequencyError, _
         chipRateError, rmsMagnitudeError, rmsPhaseError)

    End Sub

    Private Sub PrintResults()
        Console.WriteLine("-----------------------------ModAcc Results----------------------------" & vbLf)
        Console.WriteLine("-------------------Composite EVM Results--------------------")
        Console.WriteLine("RMS EVM (%)                               : {0}", rmsEvm)
        Console.WriteLine("Peak EVM (%)                              : {0}", peakEvm)
        Console.WriteLine("Rho                                       : {0}", rho)
        Console.WriteLine("Frequency Error (Hz)                      : {0}", frequencyError)
        Console.WriteLine("Chip Rate Error (ppm)                     : {0}", chipRateError)
        Console.WriteLine("RMS Phase Error (deg)                     : {0}", rmsPhaseError)
        Console.WriteLine("RMS Magnitude Error (%)                   : {0}", rmsMagnitudeError)



        Console.WriteLine("----------------------------------ACP Results----------------------------" & vbLf)

        Console.WriteLine("---------------------Carrier Measurements-----------------------" & vbLf)
        Console.WriteLine("Carrier Absolute Power (dBm)              : {0}", carrierAbsolutePowerAcp)

        Console.WriteLine(vbLf & "-----------------Offset Channel Measurements------------------" & vbLf)
        For i As Integer = 0 To lowerRelativePower.Length - 1
            Console.WriteLine("Offset                                    : {0}" & vbLf, i)
            Console.WriteLine("Lower Relative Power (dB)                 : {0}", lowerRelativePower(i))
            Console.WriteLine("Upper Relative Power (dB)                 : {0}", upperRelativePower(i))
            Console.WriteLine("Lower Absolute Power (dBm)                : {0}", lowerAbsolutePower(i))
            Console.WriteLine("Upper Absolute Power (dBm)                : {0}", upperAbsolutePower(i))
        Next
        Console.WriteLine("-----------------------------------CHP Result--------------------------------")
        Console.WriteLine("Carrier Absolute Power (dBm)              : {0}", carrierAbsolutePowerChp)


        Console.WriteLine("------------------------------------OBW Results------------------------------" & vbLf)
        Console.WriteLine("Occupied Bandwidth (Hz)                   : {0}", occupiedBandwidth)
        Console.WriteLine("Absolute Power (dBm)                      : {0}", absolutePower)
        Console.WriteLine("Start Frequency (Hz)                      : {0}", startFrequency)
        Console.WriteLine("Stop Frequency (Hz)                       : {0}", stopFrequency)

        Console.WriteLine("--------------------------------------SEM Results----------------------------" & vbLf)
        Console.WriteLine("Composite measurement status              : {0}" & vbLf, measurementStatus)

        Console.WriteLine(vbLf & "---------------------------Carrier Measurement-----------------------" & vbLf)
        Console.WriteLine("Carrier Absolute Integrated Power (dBm)    : {0}", carrierAbsoluteIntegratedPower)

        Console.WriteLine(vbLf & "--------------Offset segment measurements ---------------------------" & vbLf)
        Console.WriteLine(vbLf & "Lower Offset segment measurements")
        For i As Integer = 0 To lowerOffsetMargin.Length - 1
            Console.WriteLine("Offset                                    : {0}" & vbLf, i)

            Console.WriteLine("Lower Offset : Margin (dB)                : {0}", lowerOffsetMargin(i))
            Console.WriteLine("Lower offset : Margin Absolute Power (dBm): {0}", lowerOffsetMarginAbsolutePower(i))
            Console.WriteLine("Lower offset : Margin Relative Power (dB) : {0}", lowerOffsetMarginRelativePower(i))
            Console.WriteLine("Lower offset : Margin Frequency (Hz)      : {0}", lowerOffsetMarginFrequency(i))
            Console.WriteLine("Lower offset : Measurement Status         : {0}" & vbLf, lowerOffsetMeasurementStatus(i))
        Next
        Console.WriteLine(vbLf & vbLf)
        Console.WriteLine(vbLf & "Upper Offset segment measurements")
        For j As Integer = 0 To upperOffsetMargin.Length - 1
            Console.WriteLine("Offset                                    : {0}" & vbLf, j)
            Console.WriteLine("Upper Offset : Margin (dB)                : {0}", upperOffsetMargin(j))
            Console.WriteLine("Upper offset : Margin Absolute Power (dBm): {0}", upperOffsetMarginAbsolutePower(j))
            Console.WriteLine("Upper offset : Margin Relative Power (dB) : {0}", upperOffsetMarginRelativePower(j))
            Console.WriteLine("Upper offset : Margin Frequency (Hz)      : {0}", upperOffsetMarginFrequency(j))
            Console.WriteLine("Upper offset : Measurement Status         : {0}" & vbLf, upperOffsetMeasurementStatus(j))

        Next
      End Sub

    Private Sub CloseSession()
        Try
            If cdma2k IsNot Nothing Then
                cdma2k.Dispose()
                cdma2k = Nothing
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
        Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
    End Sub
End Class
