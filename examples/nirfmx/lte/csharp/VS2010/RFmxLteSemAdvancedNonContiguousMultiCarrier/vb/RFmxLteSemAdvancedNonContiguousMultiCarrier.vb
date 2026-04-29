'Steps:
'1.Open a new RFmx Session.
'2.Configure Frequency Reference.
'3.Configure basic signal properties (Reference Level and External Attenuation).
'4.Configure Trigger Type and Trigger Parameters.
'5[A-F]. Configure Subblock Parameters.
'6.Select SEM measurement and enable Traces.
'7.Configure Sweep Time Parameters.
'8.Configure Averaging Parameters for SEM measurement.
'9.Configure Standard Mask Type.
'10.[A-E]. Configure Subblock Offset Segments.
'11.Initiate the Measurement.
'12[A-F]. Fetch SEM Measurements and Traces.
'13. Close RFmx Session. 

'Step 5 :
'A.Configure Number of Subblocks.
'B.Configure subblock Frequency.
'C.Configure Component Carrier Spacing.
'D.Configure Number of Component Carriers.
'E.Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
'F.Configure Number of Offsets.
'G.Configure Offset Frequency.
'H.Configure Offset RBW Filter.
'I.Configure Offset Bandwidth Integral.
'J.Configure Offset Absolute Limit.

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
    Public componentCarrierMaximumOutputPower As Double()
    '(dBm) 
    Public startFrequency As Double()
    '(Hz) 
    Public stopFrequency As Double()
    '(Hz) 
    Public sideband As RFmxLteMXSemOffsetSideband()
    Public RBW As Double()
    '(Hz) 
    Public RBWFilterType As RFmxLteMXSemOffsetRbwFilterType()
    Public bandwidthIntegral As Integer()
    Public offsetAbsoluteLimitStart As Double()
    '(dBm) 
    Public offsetAbsoluteLimitStop As Double()
    '(dBm) 
    Public offsetRelativeLimitStart As Double()
    '(dBm) 
    Public offsetRelativeLimitStop As Double()
    '(dBm) 
    Public offsetLimitFailMask As RFmxLteMXSemOffsetLimitFailMask()
End Structure

'  Subblock measurement outputs structure 

Structure SubblockMeasurement
    Public subblockPower As Double
    '(dBm) 
    Public integrationBandwidth As Double
    '(Hz) 
    Public frequency As Double
    '(Hz) 
    Public lowerOffsetMeasurementStatus As RFmxLteMXSemLowerOffsetMeasurementStatus()
    Public lowerOffsetMargin As Double()
    '(dB) 
    Public lowerOffsetMarginFrequency As Double()
    '(Hz) 
    Public lowerOffsetMarginAbsolutePower As Double()
    '(dBm) 
    Public upperOffsetMeasurementStatus As RFmxLteMXSemUpperOffsetMeasurementStatus()
    Public upperOffsetMargin As Double()
    '(dB) 
    Public upperOffsetMarginFrequency As Double()
    '(Hz) 
    Public upperOffsetMarginAbsolutePower As Double()
    '(dBm) 
End Structure

Public Class RFmxLteSemAdvancedNonContiguousMultiCarrier
    Private instrSession As RFmxInstrMX
    Private lte As RFmxLteMX
    Private rfsaResourceName As String

    Const NumberOfSubblocks As Integer = 2
    Const NumberOfComponentCarrier As Integer = 1
    Const NumberOfOffsetSegments As Integer = 4

    Private frequencyReferenceSource As String
    Private frequencyReferenceFrequency As Double

    Private centerFrequency As Double
    Private referenceLevel As Double
    Private externalAttenuation As Double

    Private enableTrigger As Boolean
    Private digitalEdgeSource As String
    Private digitalEdge As RFmxLteMXDigitalEdgeTriggerEdge
    Private triggerDelay As Double

    Private linkDirection As RFmxLteMXLinkDirection
    Private uplinkMaskType As RFmxLteMXSemUplinkMaskType
    Private eNodeBCategory As RFmxLteMXeNodeBCategory
    Private downlinkMaskType As RFmxLteMXSemDownlinkMaskType
    Private deltaFMaximum As Double
    Private aggregatedMaximumPower As Double

    Private subblocks As SubblockInput()
    Private subblocksMsr As SubblockMeasurement()

    Private sweepTimeAuto As RFmxLteMXSemSweepTimeAuto
    Private sweepTimeInterval As Double

    Private averagingEnabled As RFmxLteMXSemAveragingEnabled
    Private averagingCount As Integer
    Private averagingType As RFmxLteMXSemAveragingType

    Private measurementStatus As RFmxLteMXSemMeasurementStatus

    Private relativePower As Double()

    Private timeout As Double
    Private totalAggregatedPower As Double

    Private spectrum As Spectrum(Of Single)
    Private absoluteMask As Spectrum(Of Single)



    Private subblockString As String

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

        linkDirection = RFmxLteMXLinkDirection.Uplink
        uplinkMaskType = RFmxLteMXSemUplinkMaskType.General_NS01
        eNodeBCategory = RFmxLteMXeNodeBCategory.WideAreaBaseStationCategoryA
        downlinkMaskType = RFmxLteMXSemDownlinkMaskType.ENodeBCategoryBased

        deltaFMaximum = 15000000.0
        ' (Hz) 
        aggregatedMaximumPower = 0.0
        ' (dBm) 

        subblocksMsr = New SubblockMeasurement(NumberOfSubblocks - 1) {}

        subblocks = New SubblockInput(NumberOfSubblocks - 1) {New SubblockInput() With {
         .subblockFrequency = 0.0,
         .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
         .componentCarrierAtCenterFrequency = -1,
         .componentCarrierBandwidth = New Double(NumberOfComponentCarrier - 1) {20000000.0},
         .componentCarrierFrequency = New Double(NumberOfComponentCarrier - 1) {0.0},
         .componentCarrierMaximumOutputPower = New Double(NumberOfComponentCarrier - 1) {0.0},
         .startFrequency = New Double(NumberOfOffsetSegments - 1) {15000.0, 1500000.0, 5500000.0, 20500000.0},
         .stopFrequency = New Double(NumberOfOffsetSegments - 1) {985000.0, 4500000.0, 19500000.0, 24500000.0},
         .sideband = New RFmxLteMXSemOffsetSideband(NumberOfOffsetSegments - 1) {RFmxLteMXSemOffsetSideband.Both, RFmxLteMXSemOffsetSideband.Both, RFmxLteMXSemOffsetSideband.Both, RFmxLteMXSemOffsetSideband.Both},
         .RBW = New Double(NumberOfOffsetSegments - 1) {10000.0, 250000.0, 250000.0, 250000.0},
         .RBWFilterType = New RFmxLteMXSemOffsetRbwFilterType(NumberOfOffsetSegments - 1) {RFmxLteMXSemOffsetRbwFilterType.Gaussian, RFmxLteMXSemOffsetRbwFilterType.Gaussian, RFmxLteMXSemOffsetRbwFilterType.Gaussian, RFmxLteMXSemOffsetRbwFilterType.Gaussian},
         .bandwidthIntegral = New Integer(NumberOfOffsetSegments - 1) {3, 4, 4, 4},
         .offsetAbsoluteLimitStart = New Double(NumberOfOffsetSegments - 1) {-19.5, -8.5, -11.5, -23.5},
         .offsetAbsoluteLimitStop = New Double(NumberOfOffsetSegments - 1) {-19.5, -8.5, -11.5, -23.5},
         .offsetRelativeLimitStart = New Double(NumberOfOffsetSegments - 1) {-51.5, -51.5, -51.5, -51.5},
         .offsetRelativeLimitStop = New Double(NumberOfOffsetSegments - 1) {-58.5, -58.5, -58.5, -58.5},
         .offsetLimitFailMask = New RFmxLteMXSemOffsetLimitFailMask(NumberOfOffsetSegments - 1) {RFmxLteMXSemOffsetLimitFailMask.Absolute, RFmxLteMXSemOffsetLimitFailMask.Absolute, RFmxLteMXSemOffsetLimitFailMask.Absolute, RFmxLteMXSemOffsetLimitFailMask.Absolute}
        }, New SubblockInput() With {
         .subblockFrequency = 30000000.0,
         .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
         .componentCarrierAtCenterFrequency = -1,
         .componentCarrierBandwidth = New Double(NumberOfComponentCarrier - 1) {20000000.0},
         .componentCarrierFrequency = New Double(NumberOfComponentCarrier - 1) {0.0},
         .componentCarrierMaximumOutputPower = New Double(NumberOfComponentCarrier - 1) {0.0},
         .startFrequency = New Double(NumberOfOffsetSegments - 1) {15000.0, 1500000.0, 5500000.0, 20500000.0},
         .stopFrequency = New Double(NumberOfOffsetSegments - 1) {985000.0, 4500000.0, 19500000.0, 24500000.0},
         .sideband = New RFmxLteMXSemOffsetSideband(NumberOfOffsetSegments - 1) {RFmxLteMXSemOffsetSideband.Both, RFmxLteMXSemOffsetSideband.Both, RFmxLteMXSemOffsetSideband.Both, RFmxLteMXSemOffsetSideband.Both},
         .RBW = New Double(NumberOfOffsetSegments - 1) {10000.0, 250000.0, 250000.0, 250000.0},
         .RBWFilterType = New RFmxLteMXSemOffsetRbwFilterType(NumberOfOffsetSegments - 1) {RFmxLteMXSemOffsetRbwFilterType.Gaussian, RFmxLteMXSemOffsetRbwFilterType.Gaussian, RFmxLteMXSemOffsetRbwFilterType.Gaussian, RFmxLteMXSemOffsetRbwFilterType.Gaussian},
         .bandwidthIntegral = New Integer(NumberOfOffsetSegments - 1) {3, 4, 4, 4},
         .offsetAbsoluteLimitStart = New Double(NumberOfOffsetSegments - 1) {-19.5, -8.5, -11.5, -23.5},
         .offsetAbsoluteLimitStop = New Double(NumberOfOffsetSegments - 1) {-19.5, -8.5, -11.5, -23.5},
         .offsetRelativeLimitStart = New Double(NumberOfOffsetSegments - 1) {-51.5, -51.5, -51.5, -51.5},
         .offsetRelativeLimitStop = New Double(NumberOfOffsetSegments - 1) {-58.5, -58.5, -58.5, -58.5},
         .offsetLimitFailMask = New RFmxLteMXSemOffsetLimitFailMask(NumberOfOffsetSegments - 1) {RFmxLteMXSemOffsetLimitFailMask.Absolute, RFmxLteMXSemOffsetLimitFailMask.Absolute, RFmxLteMXSemOffsetLimitFailMask.Absolute, RFmxLteMXSemOffsetLimitFailMask.Absolute}
        }}
        sweepTimeAuto = RFmxLteMXSemSweepTimeAuto.[True]
        sweepTimeInterval = 0.001
        ' (s) 
        averagingEnabled = RFmxLteMXSemAveragingEnabled.[False]
        averagingCount = 10
        averagingType = RFmxLteMXSemAveragingType.Rms
        timeout = 10
    End Sub


    Private Sub ConfigureLte()
        lte = instrSession.GetLteSignalConfiguration()
        ' Create a new RFmx Session 
        instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)

        lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)

        lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)

        lte.ConfigureNumberOfSubblocks("", NumberOfSubblocks)
        For i As Integer = 0 To NumberOfSubblocks - 1
            subblockString = RFmxLteMX.BuildSubblockString("", i)
            lte.SetSubblockFrequency(subblockString, subblocks(i).subblockFrequency)
            lte.ComponentCarrier.ConfigureSpacing(subblockString, subblocks(i).componentCarrierSpacingType, subblocks(i).componentCarrierAtCenterFrequency)
            lte.ConfigureNumberOfComponentCarriers(subblockString, NumberOfComponentCarrier)
            lte.ComponentCarrier.ConfigureArray(subblockString, subblocks(i).componentCarrierBandwidth, subblocks(i).componentCarrierFrequency, Nothing)
            lte.Sem.Configuration.ConfigureNumberOfOffsets(subblockString, NumberOfOffsetSegments)
            lte.Sem.Configuration.ConfigureOffsetFrequencyArray(subblockString, subblocks(i).startFrequency, subblocks(i).stopFrequency, subblocks(i).sideband)
            lte.Sem.Configuration.ConfigureOffsetRbwFilterArray(subblockString, subblocks(i).RBW, subblocks(i).RBWFilterType)
            lte.Sem.Configuration.ConfigureOffsetBandwidthIntegralArray(subblockString, subblocks(i).bandwidthIntegral)
            lte.Sem.Configuration.ConfigureOffsetAbsoluteLimitArray(subblockString, subblocks(i).offsetAbsoluteLimitStart, subblocks(i).offsetAbsoluteLimitStop)
            lte.Sem.Configuration.ConfigureOffsetRelativeLimitArray(subblockString, subblocks(i).offsetRelativeLimitStart, subblocks(i).offsetRelativeLimitStop)

            lte.Sem.Configuration.ConfigureOffsetLimitFailMaskArray(subblockString, subblocks(i).offsetLimitFailMask)
        Next
        lte.ConfigureLinkDirection("", linkDirection)
        lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Sem, True)
        lte.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
        lte.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)

        If linkDirection = RFmxLteMXLinkDirection.Uplink Then
            lte.Sem.Configuration.ConfigureUplinkMaskType("", uplinkMaskType)
        Else
            lte.ConfigureeNodeBCategory("", eNodeBCategory)
            lte.Sem.Configuration.ConfigureDownlinkMask("", downlinkMaskType, deltaFMaximum, aggregatedMaximumPower)
            For i As Integer = 0 To NumberOfSubblocks - 1
                subblockString = RFmxLteMX.BuildSubblockString("", i)
                lte.Sem.Configuration.ComponentCarrier.ConfigureMaximumOutputPowerArray(subblockString, subblocks(i).componentCarrierMaximumOutputPower)
            Next
        End If
        lte.Initiate("", "")
    End Sub

    Private Sub RetrieveResults()
        For i As Integer = 0 To NumberOfSubblocks - 1
            subblockString = RFmxLteMX.BuildSubblockString("", i)
            lte.Sem.Results.FetchUpperOffsetMarginArray(subblockString, timeout, subblocksMsr(i).upperOffsetMeasurementStatus, subblocksMsr(i).upperOffsetMargin, subblocksMsr(i).upperOffsetMarginFrequency, subblocksMsr(i).upperOffsetMarginAbsolutePower, _
             relativePower)
            lte.Sem.Results.FetchLowerOffsetMarginArray(subblockString, timeout, subblocksMsr(i).lowerOffsetMeasurementStatus, subblocksMsr(i).lowerOffsetMargin, subblocksMsr(i).lowerOffsetMarginFrequency, subblocksMsr(i).lowerOffsetMarginAbsolutePower, _
             relativePower)
            lte.Sem.Results.FetchSubblockMeasurement(subblockString, timeout, subblocksMsr(i).subblockPower, subblocksMsr(i).integrationBandwidth, subblocksMsr(i).frequency)
        Next
        lte.Sem.Results.FetchTotalAggregatedPower("", timeout, totalAggregatedPower)
        lte.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)
        lte.Sem.Results.FetchSpectrum("", timeout, spectrum, absoluteMask)
    End Sub

    Private Sub PrintResults()
        Console.WriteLine("Total Aggregated Power (dBm)                  :{0}", totalAggregatedPower)
        Console.WriteLine("Measurement Status                            :{0}", measurementStatus)
        Console.WriteLine("Subblock Measurements" & vbLf)
        For i As Integer = 0 To subblocksMsr.Length - 1
            Console.WriteLine("Subblock  {0}" & vbLf, i)
            Console.WriteLine("Subblock Power (dBm)                           :{0}", subblocksMsr(i).subblockPower)
            Console.WriteLine("Integration Bandwidth (Hz)                     :{0}", subblocksMsr(i).integrationBandwidth)
            Console.WriteLine("Frequency (Hz)                                 :{0}" & vbLf, subblocksMsr(i).frequency)
            For j As Integer = 0 To subblocksMsr(i).lowerOffsetMargin.Length - 1
                Console.WriteLine("Offset measurement   {0}" & vbLf, j)
                Console.WriteLine("Lower Offset Segement Measurement  ")
                Console.WriteLine("Measurement Status                             :{0}", subblocksMsr(i).lowerOffsetMeasurementStatus(j))
                Console.WriteLine("Margin (dB)                                    :{0}", subblocksMsr(i).lowerOffsetMargin(j))
                Console.WriteLine("Margin Frequency (Hz)                          :{0}", subblocksMsr(i).lowerOffsetMarginFrequency(j))
                Console.WriteLine("Margin Absolute Power (dBm)                    :{0}", subblocksMsr(i).lowerOffsetMarginAbsolutePower(j))
                Console.WriteLine("Upper Offset Segement Measurement  ")
                Console.WriteLine("Measurement Status                             :{0}", subblocksMsr(i).upperOffsetMeasurementStatus(j))
                Console.WriteLine("Margin (dB)                                    :{0}", subblocksMsr(i).upperOffsetMargin(j))
                Console.WriteLine("Margin Frequency (Hz)                          :{0}", subblocksMsr(i).upperOffsetMarginFrequency(j))
                Console.WriteLine("Margin Absolute Power (dBm)                    :{0}" & vbLf, subblocksMsr(i).upperOffsetMarginAbsolutePower(j))
            Next
        Next
    End Sub
    Private Shared Sub DisplayError(ex As Exception)
        Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
    End Sub

End Class








