'Steps:
'1. Open a New RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties(Center Frequency, Reference Level And External Attenuation).
'4. Configure Trigger Type And Trigger Parameters.
'5[A - F].Configure Subblock Configurations.
'5A.Configure Number of Subblocks.
'5B.Configure Subblock Frequency.
'5D.Configure Component Carrier Spacing.
'5E.Configure Number of Component Carriers.
'5F.Configure Component Carriers(Component Carrier Frequency And Component Carrier Bandwidth).
'6. Select CHP measurement And enable Traces.
'7. Configure Sweep Time Parameters.
'8. Configure Averaging Parameters for CHP measurement.
'9. Initiate the Measurement.
'10. Fetch CHP Measurements And Traces.
'11. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

' Input: Subblock inputs structure 

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

' Input: Subblock measurement outputs structure 

Structure SubblockMeasurement
    Public subblockPower As Double
    '(dBm) 
    Public integrationBandwidth As Double
    '(Hz) 
    Public frequency As Double
    '(Hz) 
    Public numberOfOffsets As Integer
    Public absolutePower As Double()
    ' = NULL;	/*(dBm) */
    Public relativePower As Double()
    ' = NULL;	/*(dB) */
End Structure

Public Class RFmxLteChpNonContiguousMultiCarrier
    Private instrSession As RFmxInstrMX
    Private lte As RFmxLteMX
    Private rfsaResourceName As String

    Private Const NumberOfComponentCarriers As Integer = 1
    Private Const NumberOfSubblocks As Integer = 2

    Private frequencyReferenceSource As String
    Private frequencyReferenceFrequency As Double

    Private centerFrequency As Double
    Private referenceLevel As Double
    Private externalAttenuation As Double

    Private enableTrigger As Boolean
    Private digitalEdgeSource As String
    Private digitalEdge As RFmxLteMXDigitalEdgeTriggerEdge
    Private triggerDelay As Double

    Private sweepTimeAuto As RFmxLteMXChpSweepTimeAuto
    Private sweepTimeInterval As Double

    Private averagingEnabled As RFmxLteMXChpAveragingEnabled
    Private averagingCount As Integer

    Private averagingType As RFmxLteMXChpAveragingType
    Private timeout As Double

    Private spectrum As Spectrum(Of Single)

    Private subblockString As String

    Private subblock As SubblockInput()
    Private subblockMeasurement As SubblockMeasurement()

    Private totalAggregatedPower As Double

    Private Sub CloseSession()
        If lte IsNot Nothing Then
            lte.Dispose()
            lte = Nothing
        End If
        If instrSession IsNot Nothing Then
            instrSession.Close()
            instrSession = Nothing
        End If
    End Sub

    Public Sub Run()
        Try
            InitializeVariables()
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
        instrSession = New RFmxInstrMX(rfsaResourceName, "")
    End Sub

    Private Sub InitializeVariables()
        rfsaResourceName = "RFSA"

        frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
        frequencyReferenceFrequency = 10000000.0
        ' (Hz) 

        centerFrequency = 1950000000.0
        ' (Hz) 
        referenceLevel = 0.0
        ' (dBm) 
        externalAttenuation = 0.0
        ' (dBm) 

        enableTrigger = False
        digitalEdgeSource = RFmxLteMXConstants.Pfi0
        digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising
        triggerDelay = 0.0
        ' (s) 

        sweepTimeAuto = RFmxLteMXChpSweepTimeAuto.[True]
        sweepTimeInterval = 0.001
        ' (s) 

        averagingEnabled = RFmxLteMXChpAveragingEnabled.[False]
        averagingCount = 10
        averagingType = RFmxLteMXChpAveragingType.Rms

        timeout = 10.0
        ' (s) 


        subblockMeasurement = New SubblockMeasurement(NumberOfSubblocks - 1) {}
        subblock = New SubblockInput() {
                                            New SubblockInput() With {
                                                                        .subblockFrequency = 0.0,
                                                                        .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                                                                        .componentCarrierAtCenterFrequency = -1,
                                                                        .componentCarrierBandwidth = New Double(NumberOfComponentCarriers - 1) {20000000.0},
                                                                        .componentCarrierFrequency = New Double(NumberOfComponentCarriers - 1) {0.0}
                                                                     },
                                            New SubblockInput() With {
                                                                        .subblockFrequency = 30000000.0,
                                                                        .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                                                                        .componentCarrierAtCenterFrequency = -1,
                                                                        .componentCarrierBandwidth = New Double(NumberOfComponentCarriers - 1) {20000000.0},
                                                                        .componentCarrierFrequency = New Double(NumberOfComponentCarriers - 1) {0.0}
                                                                    }
                                        }
    End Sub

    Private Sub ConfigureLte()
        lte = instrSession.GetLteSignalConfiguration()
        ' Create a new RFmx Session 
        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

        lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
        lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
        lte.ConfigureNumberOfSubblocks("", numberOfSubblocks)
        For i As Integer = 0 To numberOfSubblocks - 1
            subblockString = RFmxLteMX.BuildSubblockString("", i)
            lte.SetSubblockFrequency(subblockString, subblock(i).subblockFrequency)
            lte.ComponentCarrier.ConfigureSpacing(subblockString, subblock(i).componentCarrierSpacingType,
                                                  subblock(i).componentCarrierAtCenterFrequency)
            lte.ConfigureNumberOfComponentCarriers(subblockString, NumberOfComponentCarriers)
            lte.ComponentCarrier.ConfigureArray(subblockString, subblock(i).componentCarrierBandwidth,
                                                subblock(i).componentCarrierFrequency, Nothing)
        Next

        lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Chp, True)
        lte.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
        lte.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
        lte.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()

        subblockMeasurement = New SubblockMeasurement(numberOfSubblocks - 1) {}
        For i As Integer = 0 To numberOfSubblocks - 1
            subblockString = RFmxLteMX.BuildSubblockString("", i)

            lte.Chp.Results.FetchSubblockMeasurement(subblockString, timeout, subblockMeasurement(i).subblockPower,
                                                     subblockMeasurement(i).integrationBandwidth,
                                                     subblockMeasurement(i).frequency)

            lte.Chp.Results.ComponentCarrier.FetchMeasurementArray(subblockString, timeout, subblockMeasurement(i).absolutePower,
                                                                   subblockMeasurement(i).relativePower)
            subblockMeasurement(i).numberOfOffsets = subblockMeasurement(i).absolutePower.Length
        Next

        lte.Chp.Results.FetchTotalAggregatedPower("", timeout, totalAggregatedPower)

        lte.Chp.Results.FetchSpectrum("", timeout, spectrum)

    End Sub

    Private Sub PrintResults()
        Console.WriteLine("Total Aggregated Power (dBm)                : {0}", totalAggregatedPower)

        Console.WriteLine("---------------Subblock Measurements---------------")
        For i As Integer = 0 To NumberOfSubblocks - 1
            Console.WriteLine("Subblock  : {0}", i)
            Console.WriteLine("Subblock Power (dBm)                        : {0}", subblockMeasurement(i).subblockPower)
            Console.WriteLine("Integration Bandwidth (Hz)                  : {0}", subblockMeasurement(i).integrationBandwidth)
            Console.WriteLine("Frequency (Hz)                              : {0}", subblockMeasurement(i).frequency)
            Console.WriteLine("------Component Carrier Measurements------")
            For j As Integer = 0 To subblockMeasurement(i).numberOfOffsets - 1
                Console.WriteLine("Carrier  : {0}", j)
                Console.WriteLine("Absolute Power (dBm)                        : {0}", subblockMeasurement(i).absolutePower(j))
                Console.WriteLine("Relative Power (dB)                         : {0}", subblockMeasurement(i).relativePower(j))
            Next
        Next
    End Sub

    Private Shared Sub DisplayError(ex As Exception)
        Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
    End Sub

End Class
