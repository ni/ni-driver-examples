'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Reference Level and External Attenuation).
'4. Configure Trigger Type and Trigger Parameters.
'5[A-F]. Configure Subblock Parameters.
'5A. Configure Number of Subblocks.
'5B. Configure subblock Frequency.
'5C. Configure Component Carrier Spacing.
'5D. Configure Number of Component Carriers.
'5E. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
'5F. Configure Component Carrier Maximum Output Power for Downlink Link Direction.
'6. Configure Link Direction.
'7. Select SEM measurement and enable Traces.
'8. Configure Sweep Time Parameters.
'9. Configure Averaging Parameters for SEM measurement.
'10. Configure Uplink Mask Type, or Downlink Mask, eNodeB Category.
'11. Initiate the Measurement.
'12. Fetch SEM Measurements and Traces.
'13. Close RFmx Session.

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
End Structure

' Input: Subblock measurement outputs structure

Structure SubblockMeasurement
   Public subblockPower As Double
   Public integrationBandwidth As Double
   Public subblockFrequency As Double

   Public lowerOffsetMarginRelativePower As Double()
   '(dBm)
   Public lowerOffsetMarginAbsolutePower As Double()
   '(dBm)
   Public lowerOffsetMargin As Double()
   Public lowerOffsetMarginFrequency As Double()
   '(Hz)
   Public lowerOffsetMeasurementStatus As RFmxLteMXSemLowerOffsetMeasurementStatus()
   Public upperOffsetMarginRelativePower As Double()
   '(dBm)
   Public upperOffsetMarginAbsolutePower As Double()
   '(dBm)
   Public upperOffsetMargin As Double()
   Public upperOffsetMarginFrequency As Double()
   '(Hz)
   Public upperOffsetMeasurementStatus As RFmxLteMXSemUpperOffsetMeasurementStatus()
End Structure

Public Class RFmxLteSemNonContiguousMultiCarrier
   Private instrSession As RFmxInstrMX
   Private lte As RFmxLteMX
   Private rfsaResourceName As String

   Const NumberOfComponentCarriers As Integer = 1
   Const NumberOfSubblocks As Integer = 2

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

   Private sweepTimeAuto As RFmxLteMXSemSweepTimeAuto
   Private sweepTimeInterval As Double

   Private averagingEnabled As RFmxLteMXSemAveragingEnabled
   Private averagingCount As Integer

   Private averagingType As RFmxLteMXSemAveragingType
   Private timeout As Double

   Private subblockString As String

   Private subblocks As SubblockInput()
   Private subblocksMsr As SubblockMeasurement()

   Private spectrum As Spectrum(Of Single)
   Private absoluteMask As Spectrum(Of Single)
   Private totalAggregatedPower As Double
   Private measurementStatus As RFmxLteMXSemMeasurementStatus

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

      sweepTimeAuto = RFmxLteMXSemSweepTimeAuto.[True]
      sweepTimeInterval = 0.001
      ' (s)

      averagingEnabled = RFmxLteMXSemAveragingEnabled.[False]
      averagingCount = 10
      averagingType = RFmxLteMXSemAveragingType.Rms

      timeout = 10.0
      ' (s)

      subblocksMsr = New SubblockMeasurement(NumberOfSubblocks - 1) {}
      subblocks = New SubblockInput(NumberOfSubblocks - 1) {New SubblockInput() With {
         .subblockFrequency = 0.0,
         .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
         .componentCarrierAtCenterFrequency = -1,
         .componentCarrierBandwidth = New Double(NumberOfComponentCarriers - 1) {20000000.0},
         .componentCarrierFrequency = New Double(NumberOfComponentCarriers - 1) {0.0},
         .componentCarrierMaximumOutputPower = New Double(NumberOfComponentCarriers - 1) {0.0}
        }, New SubblockInput() With {
         .subblockFrequency = 30000000.0,
         .componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
         .componentCarrierAtCenterFrequency = -1,
         .componentCarrierBandwidth = New Double(NumberOfComponentCarriers - 1) {20000000.0},
         .componentCarrierFrequency = New Double(NumberOfComponentCarriers - 1) {0.0},
         .componentCarrierMaximumOutputPower = New Double(NumberOfComponentCarriers - 1) {0.0}
        }}
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
         lte.ConfigureNumberOfComponentCarriers(subblockString, NumberOfComponentCarriers)
         lte.ComponentCarrier.ConfigureArray(subblockString, subblocks(i).componentCarrierBandwidth, subblocks(i).componentCarrierFrequency, Nothing)
         If linkDirection = RFmxLteMXLinkDirection.Downlink Then
            lte.Sem.Configuration.ComponentCarrier.ConfigureMaximumOutputPowerArray(subblockString, subblocks(i).componentCarrierMaximumOutputPower)
         End If
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
      End If
      lte.Initiate("", "")
   End Sub

   Private Sub RetrieveResults()
      subblocksMsr = New SubblockMeasurement(NumberOfSubblocks - 1) {}
      For i As Integer = 0 To NumberOfSubblocks - 1
         subblockString = RFmxLteMX.BuildSubblockString("", i)

         lte.Sem.Results.FetchSubblockMeasurement(subblockString, timeout, subblocksMsr(i).subblockPower, subblocksMsr(i).integrationBandwidth, subblocksMsr(i).subblockFrequency)

         lte.Sem.Results.FetchUpperOffsetMarginArray(subblockString, timeout, subblocksMsr(i).upperOffsetMeasurementStatus, subblocksMsr(i).upperOffsetMargin, subblocksMsr(i).upperOffsetMarginFrequency, subblocksMsr(i).upperOffsetMarginAbsolutePower,
            subblocksMsr(i).upperOffsetMarginRelativePower)

         lte.Sem.Results.FetchLowerOffsetMarginArray(subblockString, timeout, subblocksMsr(i).lowerOffsetMeasurementStatus, subblocksMsr(i).lowerOffsetMargin, subblocksMsr(i).lowerOffsetMarginFrequency, subblocksMsr(i).lowerOffsetMarginAbsolutePower,
            subblocksMsr(i).lowerOffsetMarginRelativePower)
      Next

      lte.Sem.Results.FetchTotalAggregatedPower("", timeout, totalAggregatedPower)
      lte.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)
      lte.Sem.Results.FetchSpectrum("", timeout, spectrum, absoluteMask)
   End Sub

   Private Sub PrintResults()
      Console.WriteLine("Total Aggregated Power (dBm)    :{0}", totalAggregatedPower)
      Console.WriteLine("Measurement Status              :{0}", measurementStatus)
      Console.WriteLine("--------------------Subblock Measurements--------------------")
      For i As Integer = 0 To subblocksMsr.Length - 1
         Console.WriteLine(vbLf & "Subblock {0}" & vbLf, i)

         Console.WriteLine("Subblock Power (dBm)            :{0}", subblocksMsr(i).subblockPower)
         Console.WriteLine("Integration Bandwidth (Hz)      :{0}", subblocksMsr(i).integrationBandwidth)
         Console.WriteLine("Frequency (Hz)                  :{0}", subblocksMsr(i).subblockFrequency)

         Console.WriteLine(vbLf & "Offset Segment Measurements " & vbLf)
         For j As Integer = 0 To subblocksMsr(i).lowerOffsetMargin.Length - 1
            Console.WriteLine(vbLf & "Lower Offset Segement Measurement {0}", j)

            Console.WriteLine("Measurement Status              :{0}", subblocksMsr(i).lowerOffsetMeasurementStatus(j))
            Console.WriteLine("Margin (dB)                     :{0}", subblocksMsr(i).lowerOffsetMargin(j))
            Console.WriteLine("Margin Frequency (Hz)           :{0}", subblocksMsr(i).lowerOffsetMarginFrequency(j))
            Console.WriteLine("Margin Absolute Power (dBm)     :{0}", subblocksMsr(i).lowerOffsetMarginAbsolutePower(j))

            Console.WriteLine(vbLf & "Upper Offset Segement Measurement {0}", j)

            Console.WriteLine("Measurement Status              :{0}", subblocksMsr(i).upperOffsetMeasurementStatus(j))
            Console.WriteLine("Margin (dB)                     :{0}", subblocksMsr(i).upperOffsetMargin(j))
            Console.WriteLine("Margin Frequency (Hz)           :{0}", subblocksMsr(i).upperOffsetMarginFrequency(j))
            Console.WriteLine("Margin Absolute Power (dBm)     :{0}", subblocksMsr(i).upperOffsetMarginAbsolutePower(j))
         Next
      Next
   End Sub

   Private Shared Sub DisplayError(ex As Exception)
      Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
   End Sub

End Class
