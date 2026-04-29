'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation)
'4. Configure Trigger Type and Trigger Parameters.
'5. Configure Component Carrier Spacing.
'6. Configure Component Carriers.
'7. Configure Link Direction.
'8. Select SEM measurement and enable Traces.
'9. Configure Sweep Time Parameters.
'10. Configure Averaging Parameters for SEM measurement.
'11. Configure Uplink Mask Type, or Downlink Mask, eNodeB Category and
'    Component Carrier Maximum Output Power depending On Link Direction.
'12. Initiate the Measurement.
'13. Fetch SEM Measurements and Traces.
'14. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.LteMX

Namespace NationalInstruments.Examples.RFmxLteSemContiguousMultiCarrier
   Public Class RFmxLteSemContiguousMultiCarrier
      Private instrSession As RFmxInstrMX
      Private lte As RFmxLteMX
      Private resourceName As String

      Const numberOfComponentCarriers As Integer = 2

      Private centerFrequency As Double
      Private referenceLevel As Double
      Private externalAttenuation As Double

      Private frequencyReferenceSource As String
      Private frequencyReferenceFrequency As Double

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

      Private sidelinkMaskType As RFmxLteMXSemSidelinkMaskType

      Private componentCarrierSpacingType As RFmxLteMXComponentCarrierSpacingType
      Private componentCarrierAtCenterFrequency As Integer

      Private componentCarrierBandwidth As Double() = {20000000.0, 20000000.0}
      '(Hz)
      Private componentCarrierFrequency As Double() = {-9900000.0, 9900000.0}
      '(Hz)
      Private componentCarrierMaximumOutputPower As Double() = {0.0, 0.0}
      '(dBm)

      Private sweepTimeAuto As RFmxLteMXSemSweepTimeAuto
      Private sweepTimeInterval As Double

      Private averagingEnabled As RFmxLteMXSemAveragingEnabled
      Private averagingCount As Integer
      Private averagingType As RFmxLteMXSemAveragingType

      Private timeout As Double

      Private totalAggregatedPower As Double

      Private measurementStatus As RFmxLteMXSemMeasurementStatus

      Private upperOffsetMeasurementStatus As RFmxLteMXSemUpperOffsetMeasurementStatus()
      Private upperOffsetMargin As Double()
      Private upperOffsetMarginFrequency As Double()
      '(Hz)
      Private upperOffsetMarginAbsolutePower As Double()
      '(dBm)
      Private upperOffsetMarginRelativePower As Double()
      '(dBm)

      Private lowerOffsetMeasurementStatus As RFmxLteMXSemLowerOffsetMeasurementStatus()
      Private lowerOffsetMargin As Double()
      Private lowerOffsetMarginFrequency As Double()
      '(Hz)
      Private lowerOffsetMarginAbsolutePower As Double()
      '(dBm)
      Private lowerOffsetMarginRelativePower As Double()
      '(dBm)

      Private spectrum As Spectrum(Of Single)
      Private absoluteMask As Spectrum(Of Single)

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

      Private Sub InitializeVariables()
         resourceName = "RFSA"

         centerFrequency = 1950000000.0
         ' (Hz)
         referenceLevel = 0.0
         ' (dBm)
         externalAttenuation = 0.0
         ' (dBm)

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
         frequencyReferenceFrequency = 10000000.0
         ' (Hz)

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

         sidelinkMaskType = RFmxLteMXSemSidelinkMaskType.General_NS01

         componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal
         componentCarrierAtCenterFrequency = -1

         sweepTimeAuto = RFmxLteMXSemSweepTimeAuto.True
         sweepTimeInterval = 0.001
         ' (s)

         averagingEnabled = RFmxLteMXSemAveragingEnabled.False
         averagingCount = 10
         averagingType = RFmxLteMXSemAveragingType.Rms

         timeout = 10.0
         ' (s)
      End Sub

      Private Sub InitializeInstr()
         instrSession = New RFmxInstrMX(resourceName, "")
      End Sub

      Private Sub ConfigureLte()
         lte = instrSession.GetLteSignalConfiguration()
         ' Create a new RFmx Session
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
         lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)
         lte.ComponentCarrier.ConfigureSpacing("", componentCarrierSpacingType, componentCarrierAtCenterFrequency)
         lte.ConfigureNumberOfComponentCarriers("", numberOfComponentCarriers)
         lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth, componentCarrierFrequency, Nothing)
         lte.ConfigureLinkDirection("", linkDirection)
         lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Sem, True)
         lte.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
         lte.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)
         If linkDirection = RFmxLteMXLinkDirection.Uplink Then
            lte.Sem.Configuration.ConfigureUplinkMaskType("", uplinkMaskType)
         ElseIf linkDirection = RFmxLteMXLinkDirection.Downlink Then
            lte.ConfigureeNodeBCategory("", eNodeBCategory)
            lte.Sem.Configuration.ConfigureDownlinkMask("", downlinkMaskType, deltaFMaximum, aggregatedMaximumPower)
            lte.Sem.Configuration.ComponentCarrier.ConfigureMaximumOutputPowerArray("", componentCarrierMaximumOutputPower)
         Else
            lte.Sem.Configuration.SetSidelinkMaskType("", sidelinkMaskType)
         End If
         lte.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         lte.Sem.Results.FetchTotalAggregatedPower("", timeout, totalAggregatedPower)

         lte.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)

         lte.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffsetMeasurementStatus, upperOffsetMargin,
            upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower)

         lte.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffsetMeasurementStatus, lowerOffsetMargin,
            lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower)

         lte.Sem.Results.FetchSpectrum("", timeout, spectrum, absoluteMask)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("Total Aggregated Power (dBm)    :{0}", totalAggregatedPower)
         Console.WriteLine("Measurement Status              :{0}", measurementStatus)

         Console.WriteLine(vbLf & "--------  Lower Offset Segement Measurement --------" & vbLf)
         For i As Integer = 0 To lowerOffsetMargin.Length - 1
            Console.WriteLine(vbLf & "Offset {0}", i)
            Console.WriteLine("Measurement Status              :{0}", lowerOffsetMeasurementStatus(i))
            Console.WriteLine("Margin (dB)                     :{0}", lowerOffsetMargin(i))
            Console.WriteLine("Margin Frequency (Hz)           :{0}", lowerOffsetMarginFrequency(i))
            Console.WriteLine("Margin Absolute Power (dBm)     :{0}", lowerOffsetMarginAbsolutePower(i))
         Next

         Console.WriteLine(vbLf & "--------  Upper  Offset Segement Measurement --------" & vbLf)
         For i As Integer = 0 To upperOffsetMargin.Length - 1
            Console.WriteLine(vbLf & "Offset {0}", i)
            Console.WriteLine("Measurement Status              :{0}", upperOffsetMeasurementStatus(i))
            Console.WriteLine("Margin (dB)                     :{0}", upperOffsetMargin(i))
            Console.WriteLine("Margin Frequency (Hz)           :{0}", upperOffsetMarginFrequency(i))
            Console.WriteLine("Margin Absolute Power (dBm)     :{0}", upperOffsetMarginAbsolutePower(i))
         Next
      End Sub

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

      Private Shared Sub DisplayError(ex As Exception)
         Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
      End Sub

   End Class
End Namespace