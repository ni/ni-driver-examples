'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Parameters for Digital Edge Trigger.
'6. Configure Link Direction, Frequency Range, Channel Raster and Component Carrier Spacing.
'7. Configure Bandwidth Part Subcarrier Spacing.
'8. Configure Component Carriers.
'9. Select SEM measurement and enable Traces.
'10. Configure Offsets.
'11. Configure Uplink Mask Type, or Downlink Mask, Band, gNodeB Category, Delta F_Max(Hz) and
'    Component Carrier Rated Output Power based on Link Direction.
'12. Configure Sweep Time Parameters.
'13. Configure Averaging Parameters for SEM measurement.
'14. Initiate the Measurement.
'15. Fetch SEM Measurements and Traces.
'16. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRSemContiguousMultiCarrier
   Public Class RFmxNRSemContiguousMultiCarrier
      Private instrSession As RFmxInstrMX
      Private NR As RFmxNRMX
      Private resourceName As String

      Private selectedPorts As String
      Private centerFrequency As Double
      Private referenceLevel As Double
      Private externalAttenuation As Double

      Private frequencyReferenceSource As String
      Private frequencyReferenceFrequency As Double

      Private enableTrigger As Boolean
      Private digitalEdgeSource As String
      Private digitalEdge As RFmxNRMXDigitalEdgeTriggerEdge
      Private triggerDelay As Double

      Private linkDirection As RFmxNRMXLinkDirection

      Private uplinkMaskType As RFmxNRMXSemUplinkMaskType

      Private gNodeBCategory As RFmxNRMXgNodeBCategory
      Private downlinkMaskType As RFmxNRMXSemDownlinkMaskType
      Private deltaFMaximum As Double
      Private band As Integer

      Const NumberOfComponentCarriers As Integer = 2
      Private componentCarrierBandwidth As Double() = New Double(NumberOfComponentCarriers - 1) {}
      Private componentCarrierFrequency As Double() = New Double(NumberOfComponentCarriers - 1) {}
      Private componentCarrierRatedOutputPower As Double() = New Double(NumberOfComponentCarriers - 1) {}

      Const NumberOfOffsets As Integer = 4
      Private offsetStartFrequency As Double() = New Double(NumberOfOffsets - 1) {}
      Private offsetStopFrequency As Double() = New Double(NumberOfOffsets - 1) {}
      Private offsetSideband As RFmxNRMXSemOffsetSideband() = New RFmxNRMXSemOffsetSideband(NumberOfOffsets - 1) {}
      Private offsetRbw As Double() = New Double(NumberOfOffsets - 1) {}
      Private offsetRbwFilterType As RFmxNRMXSemOffsetRbwFilterType() =
         New RFmxNRMXSemOffsetRbwFilterType(NumberOfOffsets - 1) {}
      Private bandwidthIntegral As Integer() = New Integer(NumberOfOffsets - 1) {}
      Private limitFailMask As RFmxNRMXSemOffsetLimitFailMask() =
         New RFmxNRMXSemOffsetLimitFailMask(NumberOfOffsets - 1) {}
      Private absoluteLimitStart As Double() = New Double(NumberOfOffsets - 1) {}
      Private absoluteLimitStop As Double() = New Double(NumberOfOffsets - 1) {}
      Private relativeLimitStart As Double() = New Double(NumberOfOffsets - 1) {}
      Private relativeLimitStop As Double() = New Double(NumberOfOffsets - 1) {}

      Private frequencyRange As RFmxNRMXFrequencyRange

      Private componentCarrierSpacingType As RFmxNRMXComponentCarrierSpacingType
      Private channelRaster As Double
      Private componentCarrierAtCenterFrequency As Integer
      Private subcarrierSpacing As Double

      Private sweepTimeAuto As RFmxNRMXSemSweepTimeAuto
      Private sweepTimeInterval As Double

      Private averagingEnabled As RFmxNRMXSemAveragingEnabled
      Private averagingCount As Integer
      Private averagingType As RFmxNRMXSemAveragingType

      Private subblockString As String
      Private carrierString As String

      Private timeout As Double

      Private totalAggregatedPower As Double
      ' (dBm)

      Private measurementStatus As RFmxNRMXSemMeasurementStatus

      Private upperOffsetMeasurementStatus As RFmxNRMXSemUpperOffsetMeasurementStatus()
      Private upperOffsetMargin As Double()
      ' (dB)
      Private upperOffsetMarginFrequency As Double()
      ' (Hz)
      Private upperOffsetMarginAbsolutePower As Double()
      ' (dBm)
      Private upperOffsetMarginRelativePower As Double()
      ' (dB)

      Private lowerOffsetMeasurementStatus As RFmxNRMXSemLowerOffsetMeasurementStatus()
      Private lowerOffsetMargin As Double()
      ' (dB)
      Private lowerOffsetMarginFrequency As Double()
      ' (Hz)
      Private lowerOffsetMarginAbsolutePower As Double()
      ' (dBm)
      Private lowerOffsetMarginRelativePower As Double()
      ' (dB)

      Private spectrum As Spectrum(Of Single)
      Private compositeMask As Spectrum(Of Single)

      Public Sub Run()
         Try
            InitializeVariables()
            InitializeInstr()
            ConfigureNR()
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

         selectedPorts = ""
         centerFrequency = 3500000000.0
         ' (Hz)
         referenceLevel = 0.0
         ' (dBm)
         externalAttenuation = 0.0
         ' (dB)

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock
         frequencyReferenceFrequency = 10000000.0
         ' (Hz)

         enableTrigger = False
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising
         triggerDelay = 0.0
         ' (s)

         linkDirection = RFmxNRMXLinkDirection.Uplink

         uplinkMaskType = RFmxNRMXSemUplinkMaskType.General

         gNodeBCategory = RFmxNRMXgNodeBCategory.WideAreaBaseStationCategoryA
         downlinkMaskType = RFmxNRMXSemDownlinkMaskType.Standard
         deltaFMaximum = 15000000.0
         ' (Hz) 
         band = 78

         componentCarrierBandwidth(0) = 100000000.0
         ' (Hz)
         componentCarrierBandwidth(1) = 100000000.0
         ' (Hz)
         componentCarrierFrequency(0) = -49980000.0
         ' (Hz)
         componentCarrierFrequency(1) = 50010000.0
         ' (Hz)
         componentCarrierRatedOutputPower(0) = 0.0
         ' (dBm)
         componentCarrierRatedOutputPower(1) = 0.0
         ' (dBm)

         offsetStartFrequency(0) = 15000.0
         ' (Hz)
         offsetStartFrequency(1) = 1500000.0
         ' (Hz)
         offsetStartFrequency(2) = 5500000.0
         ' (Hz)
         offsetStartFrequency(3) = 40300000.0
         ' (Hz)
         offsetStopFrequency(0) = 985000.0
         ' (Hz)
         offsetStopFrequency(1) = 5500000.0
         ' (Hz)
         offsetStopFrequency(2) = 39300000.0
         ' (Hz)
         offsetStopFrequency(3) = 44300000.0
         ' (Hz)
         offsetSideband(0) = RFmxNRMXSemOffsetSideband.Both
         offsetSideband(1) = RFmxNRMXSemOffsetSideband.Both
         offsetSideband(2) = RFmxNRMXSemOffsetSideband.Both
         offsetSideband(3) = RFmxNRMXSemOffsetSideband.Both
         offsetRbw(0) = 10000.0
         ' (Hz)
         offsetRbw(1) = 250000.0
         ' (Hz)
         offsetRbw(2) = 1000000.0
         ' (Hz)
         offsetRbw(3) = 1000000.0
         ' (Hz)
         offsetRbwFilterType(0) = RFmxNRMXSemOffsetRbwFilterType.Gaussian
         offsetRbwFilterType(1) = RFmxNRMXSemOffsetRbwFilterType.Gaussian
         offsetRbwFilterType(2) = RFmxNRMXSemOffsetRbwFilterType.Gaussian
         offsetRbwFilterType(3) = RFmxNRMXSemOffsetRbwFilterType.Gaussian
         bandwidthIntegral(0) = 3
         bandwidthIntegral(1) = 4
         bandwidthIntegral(2) = 1
         bandwidthIntegral(3) = 1
         limitFailMask(0) = RFmxNRMXSemOffsetLimitFailMask.Absolute
         limitFailMask(1) = RFmxNRMXSemOffsetLimitFailMask.Absolute
         limitFailMask(2) = RFmxNRMXSemOffsetLimitFailMask.Absolute
         limitFailMask(3) = RFmxNRMXSemOffsetLimitFailMask.Absolute
         absoluteLimitStart(0) = -22.5
         ' (dBm)
         absoluteLimitStart(1) = -8.5
         ' (dBm)
         absoluteLimitStart(2) = -11.5
         ' (dBm)
         absoluteLimitStart(3) = -23.5
         ' (dBm)
         absoluteLimitStop(0) = -22.5
         ' (dBm)
         absoluteLimitStop(1) = -8.5
         ' (dBm)
         absoluteLimitStop(2) = -11.5
         ' (dBm)
         absoluteLimitStop(3) = -23.5
         ' (dBm)
         relativeLimitStart(0) = -53.0
         ' (dB)
         relativeLimitStart(1) = -53.0
         ' (dB)
         relativeLimitStart(2) = -53.0
         ' (dB)
         relativeLimitStart(3) = -53.0
         ' (dB)
         relativeLimitStop(0) = -60.0
         ' (dB)
         relativeLimitStop(1) = -60.0
         ' (dB)
         relativeLimitStop(2) = -60.0
         ' (dB)
         relativeLimitStop(3) = -60.0
         ' (dB)

         frequencyRange = RFmxNRMXFrequencyRange.Range1

         componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal
         channelRaster = 15000.0
         ' (Hz)
         componentCarrierAtCenterFrequency = -1
         subcarrierSpacing = 30000.0
         ' (Hz)

         sweepTimeAuto = RFmxNRMXSemSweepTimeAuto.True
         sweepTimeInterval = 0.001
         ' (s)

         averagingEnabled = RFmxNRMXSemAveragingEnabled.False
         averagingCount = 10
         averagingType = RFmxNRMXSemAveragingType.Rms

         timeout = 10.0
         ' (s)
      End Sub

      Private Sub InitializeInstr()
         instrSession = New RFmxInstrMX(resourceName, "")
      End Sub

      Private Sub ConfigureNR()
         NR = instrSession.GetNRSignalConfiguration()
         ' Create a new RFmx Session
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
         NR.SetSelectedPorts("", selectedPorts)
         NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)

         NR.SetLinkDirection("", linkDirection)
         NR.SetFrequencyRange("", frequencyRange)
         NR.SetChannelRaster("", channelRaster)
         NR.SetComponentCarrierSpacingType("", componentCarrierSpacingType)
         NR.SetComponentCarrierAtCenterFrequency("", componentCarrierAtCenterFrequency)

         carrierString = "carrier::all"
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing)

         NR.ComponentCarrier.SetNumberOfComponentCarriers("", NumberOfComponentCarriers)

         subblockString = RFmxNRMX.BuildSubblockString("", 0)
         For i As Integer = 0 To NumberOfComponentCarriers - 1
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i)
            NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth(i))
            NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency(i))
         Next

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Sem, True)

         NR.Sem.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsets)
         NR.Sem.Configuration.ConfigureOffsetFrequencyArray("", offsetStartFrequency, offsetStopFrequency,
            offsetSideband)
         NR.Sem.Configuration.ConfigureOffsetRbwFilterArray("", offsetRbw, offsetRbwFilterType)
         NR.Sem.Configuration.ConfigureOffsetBandwidthIntegralArray("", bandwidthIntegral)
         NR.Sem.Configuration.ConfigureOffsetLimitFailMaskArray("", limitFailMask)
         NR.Sem.Configuration.ConfigureOffsetAbsoluteLimitArray("", absoluteLimitStart, absoluteLimitStop)
         NR.Sem.Configuration.ConfigureOffsetRelativeLimitArray("", relativeLimitStart, relativeLimitStop)

         If linkDirection = RFmxNRMXLinkDirection.Uplink Then
            NR.Sem.Configuration.ConfigureUplinkMaskType("", uplinkMaskType)
         Else
            NR.ConfiguregNodeBCategory("", gNodeBCategory)
            NR.SetBand("", band)
            NR.Sem.Configuration.SetDownlinkMaskType("", downlinkMaskType)
            NR.Sem.Configuration.SetDeltaFMaximum("", deltaFMaximum)
            NR.Sem.Configuration.ComponentCarrier.ConfigureRatedOutputPowerArray("", componentCarrierRatedOutputPower)
         End If

         NR.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval)
         NR.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType)

         NR.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         NR.Sem.Results.FetchUpperOffsetMarginArray("", timeout, upperOffsetMeasurementStatus, upperOffsetMargin,
            upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower)

         NR.Sem.Results.FetchLowerOffsetMarginArray("", timeout, lowerOffsetMeasurementStatus, lowerOffsetMargin,
            lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower)

         NR.Sem.Results.FetchTotalAggregatedPower("", timeout, totalAggregatedPower)

         NR.Sem.Results.FetchMeasurementStatus("", timeout, measurementStatus)

         NR.Sem.Results.FetchSpectrum("", timeout, spectrum, compositeMask)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("Total Aggregated Power (dBm)    : {0}", totalAggregatedPower)
         Console.WriteLine("Measurement Status              : {0}", measurementStatus)

         Console.WriteLine(vbLf & "--------  Lower Offset Segement Measurements --------" & vbLf)
         For i As Integer = 0 To lowerOffsetMargin.Length - 1
            Console.WriteLine("Offset  {0}", i)
            Console.WriteLine("Measurement Status              : {0}", lowerOffsetMeasurementStatus(i))
            Console.WriteLine("Margin (dB)                     : {0}", lowerOffsetMargin(i))
            Console.WriteLine("Margin Frequency (Hz)           : {0}", lowerOffsetMarginFrequency(i))
            Console.WriteLine("Margin Absolute Power (dBm)     : {0}" & vbLf, lowerOffsetMarginAbsolutePower(i))
         Next

         Console.WriteLine(vbLf & "--------  Upper  Offset Segement Measurements --------" & vbLf)
         For i As Integer = 0 To upperOffsetMargin.Length - 1
            Console.WriteLine("Offset  {0}", i)
            Console.WriteLine("Measurement Status              : {0}", upperOffsetMeasurementStatus(i))
            Console.WriteLine("Margin (dB)                     : {0}", upperOffsetMargin(i))
            Console.WriteLine("Margin Frequency (Hz)           : {0}", upperOffsetMarginFrequency(i))
            Console.WriteLine("Margin Absolute Power (dBm)     : {0}" & vbLf, upperOffsetMarginAbsolutePower(i))
         Next
      End Sub

      Private Sub CloseSession()
         If NR IsNot Nothing Then
            NR.Dispose()
            NR = Nothing
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
