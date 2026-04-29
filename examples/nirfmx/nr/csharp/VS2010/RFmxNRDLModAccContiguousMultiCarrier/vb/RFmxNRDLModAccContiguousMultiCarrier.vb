'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Type and Trigger Parameters.
'6. Configure Link Direction as Downlink, Frequency Range, Channel Raster and Component Carrier Spacing.
'7. Configure Carrier.
'8. Configure BWP Subcarrier Spacing, DL Test Model Duplex Scheme and DL Test Model for all carriers.
'9. Select ModAcc measurement and enable Traces.
'10. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
'11. Configure Measurement Interval.
'12. Initiate the Measurement.
'13. Fetch ModAcc Measurements and Traces.
'14. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRDLModAccContiguousMultiCarrier
   Public Class RFmxNRDLModAccContiguousMultiCarrier
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

      Private frequencyRange As RFmxNRMXFrequencyRange
      Private subcarrierSpacing As Double

      Private componentCarrierSpacingType As RFmxNRMXComponentCarrierSpacingType
      Private channelRaster As Double
      Private componentCarrierAtCenterFrequency As Integer

      Const NumberOfComponentCarriers As Integer = 2
      Private componentCarrierBandwidth As Double() = New Double(NumberOfComponentCarriers - 1) {}
      Private componentCarrierFrequency As Double() = New Double(NumberOfComponentCarriers - 1) {}

      Private downlinkTestModel As RFmxNRMXDownlinkTestModel

      Private downlinkTestModelDuplexScheme As RFmxNRMXDownlinkTestModelDuplexScheme

      Private synchronizationMode As RFmxNRMXModAccSynchronizationMode

      Private averagingEnabled As RFmxNRMXModAccAveragingEnabled
      Private averagingCount As Integer

      Private measurementLengthUnit As RFmxNRMXModAccMeasurementLengthUnit
      Private measurementOffset As Double
      Private measurementLength As Double

      Private subblockString As String
      Private carrierString As String

      Private timeout As Double

      Private compositeRmsEvmMean As Double() = New Double(NumberOfComponentCarriers - 1) {}
      ' (%)
      Private compositePeakEvmMaximum As Double() = New Double(NumberOfComponentCarriers - 1) {}
      ' (%)
      Private compositePeakEvmSlotIndex As Integer() = New Integer(NumberOfComponentCarriers - 1) {}
      Private compositePeakEvmSymbolIndex As Integer() = New Integer(NumberOfComponentCarriers - 1) {}
      Private compositePeakEvmSubcarrierIndex As Integer() = New Integer(NumberOfComponentCarriers - 1) {}

      Private pdschRmsEvmMean As Double() = New Double(NumberOfComponentCarriers - 1) {}
      ' (%)

      Private componentCarrierFrequencyErrorMean As Double() = New Double(NumberOfComponentCarriers - 1) {}
      ' (Hz)
      Private componentCarrierIQOriginOffsetMean As Double() = New Double(NumberOfComponentCarriers - 1) {}
      ' (dBc)
      Private componentCarrierIQGainImbalanceMean As Double() = New Double(NumberOfComponentCarriers - 1) {}
      ' (dB)
      Private componentCarrierQuadratureErrorMean As Double() = New Double(NumberOfComponentCarriers - 1) {}
      ' (deg)

      Private rmsEvmPerSubcarrierMean As AnalogWaveform(Of Single)() =
         New AnalogWaveform(Of Single)(NumberOfComponentCarriers - 1) {}
      Private rmsEvmPerSymbolMean As AnalogWaveform(Of Single)() =
         New AnalogWaveform(Of Single)(NumberOfComponentCarriers - 1) {}

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

         frequencyRange = RFmxNRMXFrequencyRange.Range1
         subcarrierSpacing = 30000.0
         ' (Hz)

         componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal
         channelRaster = 15000.0
         ' (Hz)
         componentCarrierAtCenterFrequency = -1

         componentCarrierBandwidth(0) = 100000000.0
         ' (Hz)
         componentCarrierBandwidth(1) = 100000000.0
         ' (Hz)
         componentCarrierFrequency(0) = -49980000.0
         ' (Hz)
         componentCarrierFrequency(1) = 50010000.0
         ' (Hz)

         downlinkTestModel = RFmxNRMXDownlinkTestModel.TM1_1

         downlinkTestModelDuplexScheme = RFmxNRMXDownlinkTestModelDuplexScheme.Fdd

         synchronizationMode = RFmxNRMXModAccSynchronizationMode.Slot

         averagingEnabled = RFmxNRMXModAccAveragingEnabled.False
         averagingCount = 10

         measurementLengthUnit = RFmxNRMXModAccMeasurementLengthUnit.Slot
         measurementOffset = 0.0
         measurementLength = 1

         timeout = 10.0
         ' (s)
      End Sub

      Private Sub InitializeInstr()
         instrSession = New RFmxInstrMX(resourceName, "")
      End Sub

      Private Sub ConfigureNR()
         ' Create a new RFmx Session
         NR = instrSession.GetNRSignalConfiguration()
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
         NR.SetSelectedPorts("", selectedPorts)
         NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation)
         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger)

         NR.SetLinkDirection("", RFmxNRMXLinkDirection.Downlink)
         NR.SetFrequencyRange("", frequencyRange)
         NR.SetChannelRaster("", channelRaster)
         NR.SetComponentCarrierSpacingType("", componentCarrierSpacingType)
         NR.SetComponentCarrierAtCenterFrequency("", componentCarrierAtCenterFrequency)

         NR.ComponentCarrier.SetNumberOfComponentCarriers("", NumberOfComponentCarriers)

         subblockString = RFmxNRMX.BuildSubblockString("", 0)
         For i As Integer = 0 To NumberOfComponentCarriers - 1
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i)
            NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth(i))
            NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency(i))
         Next

         carrierString = "carrier::all"
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing)
         NR.ComponentCarrier.SetDownlinkTestModelDuplexScheme(carrierString, downlinkTestModelDuplexScheme)
         NR.ComponentCarrier.SetDownlinkTestModel(carrierString, downlinkTestModel)

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc, True)

         NR.ModAcc.Configuration.SetSynchronizationMode("", synchronizationMode)
         NR.ModAcc.Configuration.SetAveragingEnabled("", averagingEnabled)
         NR.ModAcc.Configuration.SetAveragingCount("", averagingCount)

         NR.ModAcc.Configuration.SetMeasurementLengthUnit("", measurementLengthUnit)
         NR.ModAcc.Configuration.SetMeasurementOffset("", measurementOffset)
         NR.ModAcc.Configuration.SetMeasurementLength("", measurementLength)

         NR.Initiate("", "")
      End Sub

      Private Sub RetrieveResults()
         For i As Integer = 0 To NumberOfComponentCarriers - 1
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i)

            NR.ModAcc.Results.GetCompositeRmsEvmMean(carrierString, compositeRmsEvmMean(i))
            NR.ModAcc.Results.GetCompositePeakEvmMaximum(carrierString, compositePeakEvmMaximum(i))
            NR.ModAcc.Results.GetCompositePeakEvmSlotIndex(carrierString, compositePeakEvmSlotIndex(i))
            NR.ModAcc.Results.GetCompositePeakEvmSymbolIndex(carrierString, compositePeakEvmSymbolIndex(i))
            NR.ModAcc.Results.GetCompositePeakEvmSubcarrierIndex(carrierString, compositePeakEvmSubcarrierIndex(i))
            NR.ModAcc.Results.GetComponentCarrierFrequencyErrorMean(carrierString,
               componentCarrierFrequencyErrorMean(i))
            NR.ModAcc.Results.GetComponentCarrierIQOriginOffsetMean(carrierString,
               componentCarrierIQOriginOffsetMean(i))
            NR.ModAcc.Results.GetComponentCarrierIQGainImbalanceMean(carrierString,
               componentCarrierIQGainImbalanceMean(i))
            NR.ModAcc.Results.GetComponentCarrierQuadratureErrorMean(carrierString,
               componentCarrierQuadratureErrorMean(i))

            Select Case downlinkTestModel

               Case RFmxNRMXDownlinkTestModel.TM1_1, RFmxNRMXDownlinkTestModel.TM1_2, RFmxNRMXDownlinkTestModel.TM3_3
                  NR.ModAcc.Results.GetPdschQpskRmsEvmMean(carrierString, pdschRmsEvmMean(i))

               Case RFmxNRMXDownlinkTestModel.TM2, RFmxNRMXDownlinkTestModel.TM3_1
                  NR.ModAcc.Results.GetPdsch64QamRmsEvmMean(carrierString, pdschRmsEvmMean(i))

               Case RFmxNRMXDownlinkTestModel.TM2a, RFmxNRMXDownlinkTestModel.TM3_1a
                  NR.ModAcc.Results.GetPdsch256QamRmsEvmMean(carrierString, pdschRmsEvmMean(i))

               Case RFmxNRMXDownlinkTestModel.TM3_2
                  NR.ModAcc.Results.GetPdsch16QamRmsEvmMean(carrierString, pdschRmsEvmMean(i))

            End Select

            NR.ModAcc.Results.FetchRmsEvmPerSubcarrierMeanTrace(carrierString, timeout, rmsEvmPerSubcarrierMean(i))
            NR.ModAcc.Results.FetchRmsEvmPerSymbolMeanTrace(carrierString, timeout, rmsEvmPerSymbolMean(i))
         Next
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("------------------------Measurements------------------------" & vbLf)
         For i As Integer = 0 To NumberOfComponentCarriers - 1
            Console.WriteLine("Carrier  : {0}", i)
            Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean(i))
            Console.WriteLine("Composite Peak EVM Maximum (%)                 : {0}", compositePeakEvmMaximum(i))
            Console.WriteLine("Composite Peak EVM Slot Index                  : {0}", compositePeakEvmSlotIndex(i))
            Console.WriteLine("Composite Peak EVM Symbol Index                : {0}", compositePeakEvmSymbolIndex(i))
            Console.WriteLine("Composite Peak EVM Subcarrier Index            : {0}", compositePeakEvmSubcarrierIndex(i))
            Console.WriteLine("PDSCH RMS EVM Mean (%)                         : {0}", pdschRmsEvmMean(i))
            Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean(i))
            Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}", componentCarrierIQOriginOffsetMean(i))
            Console.WriteLine("Component Carrier IQ Gain Imbalance Mean (dB)  : {0}", componentCarrierIQGainImbalanceMean(i))
            Console.WriteLine("Component Carrier Quadrature Error Mean (deg)  : {0}", componentCarrierQuadratureErrorMean(i))
            Console.WriteLine("-----------------------------------------------------------------" & vbLf)
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
