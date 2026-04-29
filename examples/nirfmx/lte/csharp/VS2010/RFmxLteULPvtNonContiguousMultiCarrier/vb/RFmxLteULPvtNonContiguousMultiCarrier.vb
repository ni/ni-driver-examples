'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Subblock Configurations.
'5A. Configure Number of Subblocks.
'5B. Configure subblock Frequency.
'5C. Configure Component Carrier Spacing.
'5D. Configure Number of Component Carriers.
'5E. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
'6. Select PvT measurement and enable Traces.
'7. Configure Duplex Scheme.
'8. Configure Measurements.
'9. Configure Averaging Parameters for PvT measurement.
'10. Initiate the Measurement.
'11. Fetch PvT Measurements and Traces.
'12. Close RFmx Session.


Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Public Class RFmxLteULPvtNonContiguousMultiCarrier
    Private instrSession As RFmxInstrMX
    Private lte As RFmxLteMX

    Private resourceName As String = "RFSA"
    Private frequencyReferenceSource As String = RFmxInstrMXConstants.OnboardClock
    Private iqPowerEdgeTriggerSource As String = "0"

    Private frequencyReferenceFrequency As Double = 10000000.0                'Hz
    Private iqPowerEdgeTriggerLevel As Double = -20.0                         'dB
    Private triggerDelay As Double = 0.0                                      'sec
    Private minimumQuietTimeDuration As Double = 0.000005                     'sec
    Private timeout As Double = 10                                            'sec
    Private centerFrequency As Double = 1950000000.0                          'Hz 
    Private referenceLevel As Double = 0.0                                    'dBm
    Private externalAttenuation As Double = 0.0                               'dB
    Private enableTrigger As Boolean = True

    Private averagingCount As Integer = 10

    Const NumberOfSubblocks As Integer = 2
    Const NumberOfComponentCarriers As Integer = 1

    Private offPowerExclusionBefore As Double = 0.0                           'sec
    Private offPowerExclusionAfter As Double = 0.0                            'sec

    Private uplinkDownlinkConfiguration As RFmxLteMXUplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0
    Private measurementMethod As RFmxLteMXPvtMeasurementMethod = RFmxLteMXPvtMeasurementMethod.Normal
    Private averagingEnabled As RFmxLteMXPvtAveragingEnabled = RFmxLteMXPvtAveragingEnabled.[False]
    Private averagingType As RFmxLteMXPvtAveragingType = RFmxLteMXPvtAveragingType.Rms
    Private minimumQuietTimeMode As RFmxLteMXTriggerMinimumQuietTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto
    Private iqPowerEdgeTriggerLevelType As RFmxLteMXIQPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative
    Private iqPowerEdgeTriggerSlope As RFmxLteMXIQPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising
    Private duplexScheme As RFmxLteMXDuplexScheme = RFmxLteMXDuplexScheme.Tdd

    ' Subblock inputs structure 

    Structure SubblockInput
        Public subblockFrequency As Double
        '(Hz)
        Public componentCarrierSpacingType As RFmxLteMXComponentCarrierSpacingType
        Public componentCarrierAtCenterFrequency As Integer
        Public componentCarrierBandwidth As Double()
        '(Hz) 
        Public componentCarrierFrequency As Double()
        '(Hz) 
    End Structure

    ' Subblock measurement outputs structure 

    Structure SubblockMeasurement
        Public measurementStatus As RFmxLteMXPvtMeasurementStatus()
        Public meanAbsoluteOffPowerBefore As Double()
        '(dBm) 
        Public meanAbsoluteOffPowerAfter As Double()
        '(dBm) 
        Public meanAbsoluteOnPower As Double()
        '(dBm) 
        Public burstWidth As Double()
        '(s) 
    End Structure

    Private subblocks As SubblockInput() = New SubblockInput(NumberOfSubblocks - 1) {New SubblockInput() With {
                                          .subblockFrequency = 0.0,
                                          .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                                          .componentCarrierAtCenterFrequency = -1,
                                          .componentCarrierBandwidth = New Double(NumberOfComponentCarriers - 1) {20000000.0},
                                          .componentCarrierFrequency = New Double(NumberOfComponentCarriers - 1) {0.0}
                                          }, New SubblockInput() With {
                                          .subblockFrequency = 30000000.0,
                                          .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                                          .componentCarrierAtCenterFrequency = -1,
                                          .componentCarrierBandwidth = New Double(NumberOfComponentCarriers - 1) {20000000.0},
                                          .componentCarrierFrequency = New Double(NumberOfComponentCarriers - 1) {0.0}
                                        }}

    Private subblockMsr As SubblockMeasurement() = New SubblockMeasurement(NumberOfSubblocks - 1) {}

    Private signalPower As AnalogWaveform(Of Single) = Nothing, absoluteLimit As AnalogWaveform(Of Single) = Nothing
    Private subblockString As String() = New String(NumberOfSubblocks) {}, subblockCarrierString As String() = New String(NumberOfSubblocks) {}

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

        lte.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel, triggerDelay, minimumQuietTimeMode, _
         minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger)

        lte.ConfigureNumberOfSubblocks("", NumberOfSubblocks)

        For i As Integer = 0 To NumberOfSubblocks - 1
            subblockString(i) = RFmxLteMX.BuildSubblockString("", i)

            lte.SetSubblockFrequency(subblockString(i), subblocks(i).subblockFrequency)
            lte.ComponentCarrier.ConfigureSpacing(subblockString(i), subblocks(i).componentCarrierSpacingType,
                                                  subblocks(i).componentCarrierAtCenterFrequency)

            lte.ConfigureNumberOfComponentCarriers(subblockString(i), NumberOfComponentCarriers)

            lte.ComponentCarrier.ConfigureArray(subblockString(i), subblocks(i).componentCarrierBandwidth,
                                                subblocks(i).componentCarrierFrequency, Nothing)
        Next

        lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Pvt, True)

        lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration)

        lte.Pvt.Configuration.ConfigureMeasurementMethod("", measurementMethod)

        lte.Pvt.Configuration.ConfigureOffPowerExclusionPeriods("", offPowerExclusionBefore, offPowerExclusionAfter)

        lte.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)

        lte.Initiate("", "")

    End Sub

    Private Sub RetrieveResults()
        ' Retrieve results 


        For i As Integer = 0 To NumberOfSubblocks - 1
            lte.Pvt.Results.FetchMeasurementArray(subblockString(i), timeout, subblockMsr(i).measurementStatus,
                                                  subblockMsr(i).meanAbsoluteOffPowerBefore,
                                                  subblockMsr(i).meanAbsoluteOffPowerAfter,
                                                  subblockMsr(i).meanAbsoluteOnPower, subblockMsr(i).burstWidth)

            subblockCarrierString(i) = RFmxLteMX.BuildCarrierString(subblockString(i), 0)

            lte.Pvt.Results.FetchSignalPowerTrace(subblockCarrierString(i), timeout, signalPower, absoluteLimit)
        Next
    End Sub

    Private Sub PrintResults()
        Console.WriteLine(vbLf & "********** Measurements ********** ")
        For i As Integer = 0 To NumberOfSubblocks - 1
            Console.WriteLine("Subblock                             : {0}", i)
            For j As Integer = 0 To NumberOfComponentCarriers - 1
                Console.WriteLine("Carrier                              : {0}", i)
                Console.WriteLine("Status                               : {0}" & vbLf, subblockMsr(i).measurementStatus(j))
                Console.WriteLine("Mean Absolute OFF Power Before (dBm) : {0}" & vbLf, subblockMsr(i).meanAbsoluteOffPowerBefore(j))
                Console.WriteLine("Mean Absolute OFF Power After (dBm)  : {0}" & vbLf, subblockMsr(i).meanAbsoluteOffPowerAfter(j))
                Console.WriteLine("Mean Absolute ON Power (dBm)         : {0}" & vbLf, subblockMsr(i).meanAbsoluteOnPower(j))
                Console.WriteLine("Burst Width (s)                      : {0}" & vbLf, subblockMsr(i).burstWidth(j))
                Console.WriteLine("---------------------------------------------" & vbLf)
            Next
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
