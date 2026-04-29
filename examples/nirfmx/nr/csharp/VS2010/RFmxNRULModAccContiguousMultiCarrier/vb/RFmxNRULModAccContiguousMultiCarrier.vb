'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Type and Trigger Parameters.
'6. Configure Frequency Range, Band, Channel Raster, Component Carrier Spacing, CC at Center Frequency
'   Auto RB Detection Enabled and Auto Increment Cell ID Enabled.
'7. Configure Carrier.
'8. Configure PUSCH and PUSCH RB Allocation.
'9. Configure PUSCH DMRS.
'10. Select ModAcc measurement and enable Traces.
'11. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
'12. Configure Measurement Interval.
'13. Initiate the Measurement.
'14. Fetch ModAcc Measurements and Traces.
'15. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRULModAccContiguousMultiCarrier
   Public Class RFmxNRULModAccContiguousMultiCarrier
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
      Private band As Integer
      Private subcarrierSpacing As Double
      Private autoResourceBlockDetectionEnabled As RFmxNRMXAutoResourceBlockDetectionEnabled
      Private autoIncrementCellIDEnabled As RFmxNRMXAutoIncrementCellIDEnabled

      Private componentCarrierSpacingType As RFmxNRMXComponentCarrierSpacingType
      Private channelRaster As Double
      Private componentCarrierAtCenterFrequency As Integer

      Const NumberOfComponentCarriers As Integer = 2
      Private componentCarrierBandwidth As Double() = New Double(NumberOfComponentCarriers - 1) {}
      Private componentCarrierFrequency As Double() = New Double(NumberOfComponentCarriers - 1) {}
      Private cellID As Integer() = New Integer(NumberOfComponentCarriers - 1) {}

      Private puschTransformPrecodingEnabled As RFmxNRMXPuschTransformPrecodingEnabled
      Private puschModulationType As RFmxNRMXPuschModulationType
      Const NumberOfResourceBlockClusters As Integer = 1
      Private puschResourceBlockOffset As Integer() = New Integer(NumberOfResourceBlockClusters - 1) {}
      Private puschNumberOfResourceBlocks As Integer() = New Integer(NumberOfResourceBlockClusters - 1) {}
      Private puschSlotAllocation As String
      Private puschSymbolAllocation As String

      Private puschDmrsPowerMode As RFmxNRMXPuschDmrsPowerMode
      Private puschDmrsPower As Double
      Private puschDmrsConfigurationType As RFmxNRMXPuschDmrsConfigurationType
      Private puschMappingType As RFmxNRMXPuschMappingType
      Private puschDmrsTypeAPosition As Integer
      Private puschDmrsDuration As RFmxNRMXPuschDmrsDuration
      Private puschDmrsAdditionalPositions As Integer

      Private synchronizationMode As RFmxNRMXModAccSynchronizationMode

      Private measurementLengthUnit As RFmxNRMXModAccMeasurementLengthUnit
      Private measurementOffset As Double
      Private measurementLength As Double

      Private averagingEnabled As RFmxNRMXModAccAveragingEnabled
      Private averagingCount As Integer

      Private subblockString As String
      Private carrierString As String
      Private puschClusterString As String

      Private timeout As Double

      Private compositeRmsEvmMean As Double() = New Double(NumberOfComponentCarriers - 1) {}
      ' (%)
      Private compositePeakEvmMaximum As Double() = New Double(NumberOfComponentCarriers - 1) {}
      ' (%)
      Private compositePeakEvmSlotIndex As Integer() = New Integer(NumberOfComponentCarriers - 1) {}
      Private compositePeakEvmSymbolIndex As Integer() = New Integer(NumberOfComponentCarriers - 1) {}
      Private compositePeakEvmSubcarrierIndex As Integer() = New Integer(NumberOfComponentCarriers - 1) {}
      Private componentCarrierFrequencyErrorMean As Double() = New Double(NumberOfComponentCarriers - 1) {}
      ' (Hz)
      Private componentCarrierIQOriginOffsetMean As Double() = New Double(NumberOfComponentCarriers - 1) {}
      ' (dBc)
      Private componentCarrierIQGainImbalanceMean As Double() = New Double(NumberOfComponentCarriers - 1) {}
      ' (dB)
      Private componentCarrierQuadratureErrorMean As Double() = New Double(NumberOfComponentCarriers - 1) {}
      ' (deg)
      Private inBandEmissionMargin As Double() = New Double(NumberOfComponentCarriers - 1) {}
      ' (dB)

      Private puschDataConstellation As ComplexSingle()() = New ComplexSingle(NumberOfComponentCarriers - 1)() {}
      Private puschDmrsConstellation As ComplexSingle()() = New ComplexSingle(NumberOfComponentCarriers - 1)() {}

      Private rmsEvmPerSubcarrierMean As AnalogWaveform(Of Single)() =
         New AnalogWaveform(Of Single)(NumberOfComponentCarriers - 1) {}
      Private rmsEvmPerSymbolMean As AnalogWaveform(Of Single)() =
         New AnalogWaveform(Of Single)(NumberOfComponentCarriers - 1) {}

      Private spectralFlatness As Spectrum(Of Single)
      Private spectralFlatnessLowerMask As Spectrum(Of Single)
      Private spectralFlatnessUpperMask As Spectrum(Of Single)

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
         band = 78
         subcarrierSpacing = 30000.0
         ' (Hz)
         autoResourceBlockDetectionEnabled = RFmxNRMXAutoResourceBlockDetectionEnabled.True
         autoIncrementCellIDEnabled = RFmxNRMXAutoIncrementCellIDEnabled.True

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
         cellID(0) = 0
         cellID(1) = 1

         puschTransformPrecodingEnabled = RFmxNRMXPuschTransformPrecodingEnabled.False
         puschModulationType = RFmxNRMXPuschModulationType.Qpsk
         puschResourceBlockOffset(0) = 0
         puschNumberOfResourceBlocks(0) = -1
         puschSlotAllocation = "0-Last"
         puschSymbolAllocation = "0-Last"

         puschDmrsPowerMode = RFmxNRMXPuschDmrsPowerMode.CdmGroups
         puschDmrsPower = 0.0
         ' (dB)
         puschDmrsConfigurationType = RFmxNRMXPuschDmrsConfigurationType.Type1
         puschMappingType = RFmxNRMXPuschMappingType.TypeA
         puschDmrsTypeAPosition = 2
         puschDmrsDuration = RFmxNRMXPuschDmrsDuration.SingleSymbol
         puschDmrsAdditionalPositions = 0

         synchronizationMode = RFmxNRMXModAccSynchronizationMode.Slot

         measurementLengthUnit = RFmxNRMXModAccMeasurementLengthUnit.Slot
         measurementOffset = 0.0
         measurementLength = 1

         averagingEnabled = RFmxNRMXModAccAveragingEnabled.False
         averagingCount = 10

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

         NR.SetFrequencyRange("", frequencyRange)
         NR.SetBand("", band)
         NR.SetChannelRaster("", channelRaster)
         NR.SetComponentCarrierSpacingType("", componentCarrierSpacingType)
         NR.SetComponentCarrierAtCenterFrequency("", componentCarrierAtCenterFrequency)
         NR.SetAutoResourceBlockDetectionEnabled("", autoResourceBlockDetectionEnabled)
         NR.SetAutoIncrementCellIDEnabled("", autoIncrementCellIDEnabled)

         NR.ComponentCarrier.SetNumberOfComponentCarriers("", NumberOfComponentCarriers)

         subblockString = RFmxNRMX.BuildSubblockString("", 0)
         For i As Integer = 0 To NumberOfComponentCarriers - 1
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i)
            NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth(i))
            NR.ComponentCarrier.SetCellID(carrierString, cellID(i))
            NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency(i))
         Next

         carrierString = "carrier::all"
         NR.ComponentCarrier.SetPuschTransformPrecodingEnabled(carrierString, puschTransformPrecodingEnabled)
         NR.ComponentCarrier.SetPuschModulationType(carrierString, puschModulationType)
         NR.ComponentCarrier.SetPuschSlotAllocation(carrierString, puschSlotAllocation)
         NR.ComponentCarrier.SetPuschSymbolAllocation(carrierString, puschSymbolAllocation)

         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing)
         NR.ComponentCarrier.SetPuschNumberOfResourceBlockClusters(carrierString, NumberOfResourceBlockClusters)

         For i As Integer = 0 To NumberOfResourceBlockClusters - 1
            puschClusterString = RFmxNRMX.BuildPuschClusterString(carrierString, i)
            NR.ComponentCarrier.SetPuschResourceBlockOffset(puschClusterString, puschResourceBlockOffset(i))
            NR.ComponentCarrier.SetPuschNumberOfResourceBlocks(puschClusterString, puschNumberOfResourceBlocks(i))
         Next

         NR.ComponentCarrier.SetPuschDmrsPowerMode(carrierString, puschDmrsPowerMode)
         NR.ComponentCarrier.SetPuschDmrsPower(carrierString, puschDmrsPower)
         NR.ComponentCarrier.SetPuschDmrsConfigurationType(carrierString, puschDmrsConfigurationType)
         NR.ComponentCarrier.SetPuschMappingType(carrierString, puschMappingType)
         NR.ComponentCarrier.SetPuschDmrsTypeAPosition(carrierString, puschDmrsTypeAPosition)
         NR.ComponentCarrier.SetPuschDmrsDuration(carrierString, puschDmrsDuration)
         NR.ComponentCarrier.SetPuschDmrsAdditionalPositions(carrierString, puschDmrsAdditionalPositions)

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
            NR.ModAcc.Results.GetInBandEmissionMargin(carrierString, inBandEmissionMargin(i))
         Next

         For i As Integer = 0 To NumberOfComponentCarriers - 1
            carrierString = RFmxNRMX.BuildCarrierString("", i)
            NR.ModAcc.Results.FetchPuschDataConstellationTrace(carrierString, timeout, puschDataConstellation(i))
            NR.ModAcc.Results.FetchPuschDmrsConstellationTrace(carrierString, timeout, puschDmrsConstellation(i))
         Next

         For i As Integer = 0 To NumberOfComponentCarriers - 1
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i)
            NR.ModAcc.Results.FetchRmsEvmPerSubcarrierMeanTrace(carrierString, timeout, rmsEvmPerSubcarrierMean(i))
            NR.ModAcc.Results.FetchRmsEvmPerSymbolMeanTrace(carrierString, timeout, rmsEvmPerSymbolMean(i))
         Next

         NR.ModAcc.Results.FetchSpectralFlatnessTrace("", timeout, spectralFlatness,
            spectralFlatnessLowerMask, spectralFlatnessUpperMask)
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
            Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean(i))
            Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}", componentCarrierIQOriginOffsetMean(i))
            Console.WriteLine("Component Carrier IQ Gain Imbalance Mean (dB)  : {0}", componentCarrierIQGainImbalanceMean(i))
            Console.WriteLine("Component Carrier Quadrature Error Mean (deg)  : {0}", componentCarrierQuadratureErrorMean(i))
            Console.WriteLine("In-Band Emission Margin (dB)                   : {0}", inBandEmissionMargin(i))
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
