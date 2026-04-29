'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Component Carrier Spacing.
'6. Configure Component Carriers.
'7. Select PvT measurement and enable Traces.
'8. Configure Duplex Scheme.
'9. Configure Measurement Method.
'10. Configure Averaging Parameters for PvT measurement.
'11. Initiate the Measurement.
'12. Fetch PvT Measurements and Traces.
'13. Close RFmx Session. 

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteULPvtContiguousMultiCarrier
    Private instrSession As RFmxInstrMX
    Private lte As RFmxLteMX

    Private resourceName As String = "RFSA"
    Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
    Private iqPowerEdgeTriggerSource As String = "0"

    Private frequencyReferenceFrequency As Double = 10000000.0              'Hz
    Private centerFrequency As Double = 1950000000.0                        'Hz
    Private referenceLevel As Double = 0.0                                  'dBm
    Private externalAttenuation As Double = 0.0                             'dB
    Private iqPowerEdgeTriggerLevel As Double = -20.0                       'dB
    Private triggerDelay As Double = 0.0                                    'sec
    Private minimumQuietTimeDuration As Double = 0.00005                    'sec
    Private timeout As Double = 10                                          'sec
    Private enableTrigger As Boolean = True
    Private averagingCount As Integer = 10
    Private componentCarrierAtCenterFrequency As Integer = -1

    Const NumberOfComponentCarriers As Integer = 2
    Private componentCarrierBandwidth As Double() = {20000000.0, 20000000.0} 'Hz
    Private componentCarrierFrequency As Double() = {-9900000.0, 9900000.0}  'Hz

    Private offPowerExclusionBefore As Double = 0.0                          'sec
    Private offPowerExclusionAfter As Double = 0.0                           'sec

    Private uplinkDownlinkConfiguration As RFmxLteMXUplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0
    Private componentCarrierSpacingType As RFmxLteMXComponentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal
    Private measurementMethod As RFmxLteMXPvtMeasurementMethod = RFmxLteMXPvtMeasurementMethod.Normal
    Private averagingEnabled As RFmxLteMXPvtAveragingEnabled = RFmxLteMXPvtAveragingEnabled.[False]
    Private averagingType As RFmxLteMXPvtAveragingType = RFmxLteMXPvtAveragingType.Rms
    Private minimumQuietTimeMode As RFmxLteMXTriggerMinimumQuietTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto
    Private iqPowerEdgeTriggerLevelType As RFmxLteMXIQPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative
    Private iqPowerEdgeTriggerSlope As RFmxLteMXIQPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising
    Private duplexScheme As RFmxLteMXDuplexScheme = RFmxLteMXDuplexScheme.Tdd

    Private measurementStatus As RFmxLteMXPvtMeasurementStatus()
    Private meanAbsoluteOFFPowerBefore As Double(), meanAbsoluteOFFPowerAfter As Double(), meanAbsoluteONPower As Double(), burstWidth As Double()
    Private signalPower As AnalogWaveform(Of Single), absoluteLimit As AnalogWaveform(Of Single)

    Private subblockCarrierString As String

    Public Sub Run()
        Try
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

        lte.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel,
                                        triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType,
                                        enableTrigger)

        lte.ComponentCarrier.ConfigureSpacing("", componentCarrierSpacingType, componentCarrierAtCenterFrequency)

        lte.ConfigureNumberOfComponentCarriers("", NumberOfComponentCarriers)

        lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth, componentCarrierFrequency, Nothing)

        lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Pvt, True)

        lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration)

        lte.Pvt.Configuration.ConfigureMeasurementMethod("", measurementMethod)

        lte.Pvt.Configuration.ConfigureOffPowerExclusionPeriods("", offPowerExclusionBefore, offPowerExclusionAfter)

        lte.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)

        lte.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        ' Retrieve results 


        lte.Pvt.Results.FetchMeasurementArray("", timeout, measurementStatus, meanAbsoluteOFFPowerBefore, meanAbsoluteOFFPowerAfter,
                                              meanAbsoluteONPower, burstWidth)
        For i As Integer = 0 To NumberOfComponentCarriers - 1
            subblockCarrierString = RFmxLteMX.BuildCarrierString("", i)
            lte.Pvt.Results.FetchSignalPowerTrace(subblockCarrierString, timeout, signalPower, absoluteLimit)
        Next

    End Sub

    Private Sub PrintResults()
        Console.WriteLine(vbLf & "********** Measurement **********")

        For i As Integer = 0 To NumberOfComponentCarriers - 1
            Console.WriteLine(vbLf & "Carrier  : {0}", i)
            Console.WriteLine("Status                               : {0}", measurementStatus(i))
            Console.WriteLine("Mean Absolute OFF Power Before (dBm) : {0}", meanAbsoluteOFFPowerBefore(i))
            Console.WriteLine("Mean Absolute OFF Power After (dBm)  : {0}", meanAbsoluteOFFPowerAfter(i))
            Console.WriteLine("Mean Absolute ON Power (dBm)         : {0}", meanAbsoluteONPower(i))
            Console.WriteLine("Burst Width (s)                      : {0}", burstWidth(i))
            Console.WriteLine("---------------------------------------------")
        Next
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
        Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
    End Sub
End Class
