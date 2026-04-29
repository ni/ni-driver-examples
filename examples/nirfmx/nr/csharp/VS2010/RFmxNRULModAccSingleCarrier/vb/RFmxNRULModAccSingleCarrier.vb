'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Type and Trigger Parameters.
'6. Configure Frequency Range, CC bandwidth, Cell ID, Band, BWP Subcarrier Spacing and Auto RB Detection Enabled.
'7. Configure PUSCH and PUSCH RB Allocation.
'8. Configure PUSCH DMRS.
'9. Select ModAcc measurement and enable Traces.
'10. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
'11. Configure Measurement Interval.
'12. Initiate the Measurement.
'13. Fetch ModAcc Measurements and Traces.
'14. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRULModAccSingleCarrier
   Public Class RFmxNRULModAccSingleCarrier
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
      Private cellID As Integer
      Private carrierBandwidth As Double
      Private subcarrierSpacing As Double
      Private autoResourceBlockDetectionEnabled As RFmxNRMXAutoResourceBlockDetectionEnabled

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
      Private bandwidthPartString As String
      Private userString As String
      Private puschString As String
      Private puschClusterString As String

      Private timeout As Double

      Private compositeRmsEvmMean As Double
      ' (%)
      Private compositePeakEvmMaximum As Double
      ' (%)
      Private compositePeakEvmSlotIndex As Integer
      Private compositePeakEvmSymbolIndex As Integer
      Private compositePeakEvmSubcarrierIndex As Integer

      Private componentCarrierFrequencyErrorMean As Double
      ' (Hz)
      Private componentCarrierIQOriginOffsetMean As Double
      ' (dBc)
      Private componentCarrierIQGainImbalanceMean As Double
      ' (dB)
      Private componentCarrierQuadratureErrorMean As Double
      ' (deg)
      Private inBandEmissionMargin As Double
      ' (dB)

      Private puschDataConstellation As ComplexSingle(), puschDmrsConstellation As ComplexSingle()

      Private rmsEvmPerSubcarrierMean As AnalogWaveform(Of Single)
      Private rmsEvmPerSymbolMean As AnalogWaveform(Of Single)

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
         cellID = 0
         carrierBandwidth = 100000000.0
         ' (Hz)
         subcarrierSpacing = 30000.0
         ' (Hz)
         autoResourceBlockDetectionEnabled = RFmxNRMXAutoResourceBlockDetectionEnabled.True

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
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth)
         NR.ComponentCarrier.SetCellID("", cellID)
         NR.SetBand("", band)
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing)
         NR.SetAutoResourceBlockDetectionEnabled("", autoResourceBlockDetectionEnabled)

         NR.ComponentCarrier.SetPuschTransformPrecodingEnabled("", puschTransformPrecodingEnabled)
         NR.ComponentCarrier.SetPuschSlotAllocation("", puschSlotAllocation)
         NR.ComponentCarrier.SetPuschSymbolAllocation("", puschSymbolAllocation)
         NR.ComponentCarrier.SetPuschModulationType("", puschModulationType)

         NR.ComponentCarrier.SetPuschNumberOfResourceBlockClusters("", NumberOfResourceBlockClusters)

         subblockString = RFmxNRMX.BuildSubblockString("", 0)
         carrierString = RFmxNRMX.BuildCarrierString(subblockString, 0)
         bandwidthPartString = RFmxNRMX.BuildBandwidthPartString(carrierString, 0)
         userString = RFmxNRMX.BuildUserString(bandwidthPartString, 0)
         puschString = RFmxNRMX.BuildPuschString(userString, 0)
         For i As Integer = 0 To NumberOfResourceBlockClusters - 1
            puschClusterString = RFmxNRMX.BuildPuschClusterString(puschString, i)
            NR.ComponentCarrier.SetPuschResourceBlockOffset(puschClusterString, puschResourceBlockOffset(i))
            NR.ComponentCarrier.SetPuschNumberOfResourceBlocks(puschClusterString, puschNumberOfResourceBlocks(i))
         Next

         NR.ComponentCarrier.SetPuschDmrsPowerMode("", puschDmrsPowerMode)
         NR.ComponentCarrier.SetPuschDmrsPower("", puschDmrsPower)
         NR.ComponentCarrier.SetPuschDmrsConfigurationType("", puschDmrsConfigurationType)
         NR.ComponentCarrier.SetPuschMappingType("", puschMappingType)
         NR.ComponentCarrier.SetPuschDmrsTypeAPosition("", puschDmrsTypeAPosition)
         NR.ComponentCarrier.SetPuschDmrsDuration("", puschDmrsDuration)
         NR.ComponentCarrier.SetPuschDmrsAdditionalPositions("", puschDmrsAdditionalPositions)

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
         NR.ModAcc.Results.GetCompositeRmsEvmMean("", compositeRmsEvmMean)
         NR.ModAcc.Results.GetCompositePeakEvmMaximum("", compositePeakEvmMaximum)
         NR.ModAcc.Results.GetCompositePeakEvmSlotIndex("", compositePeakEvmSlotIndex)
         NR.ModAcc.Results.GetCompositePeakEvmSymbolIndex("", compositePeakEvmSymbolIndex)
         NR.ModAcc.Results.GetCompositePeakEvmSubcarrierIndex("", compositePeakEvmSubcarrierIndex)

         NR.ModAcc.Results.GetComponentCarrierFrequencyErrorMean("", componentCarrierFrequencyErrorMean)
         NR.ModAcc.Results.GetComponentCarrierIQOriginOffsetMean("", componentCarrierIQOriginOffsetMean)
         NR.ModAcc.Results.GetComponentCarrierIQGainImbalanceMean("", componentCarrierIQGainImbalanceMean)
         NR.ModAcc.Results.GetComponentCarrierQuadratureErrorMean("", componentCarrierQuadratureErrorMean)
         NR.ModAcc.Results.GetInBandEmissionMargin("", inBandEmissionMargin)

         NR.ModAcc.Results.FetchPuschDataConstellationTrace("", timeout, puschDataConstellation)

         NR.ModAcc.Results.FetchPuschDmrsConstellationTrace("", timeout, puschDmrsConstellation)

         NR.ModAcc.Results.FetchRmsEvmPerSubcarrierMeanTrace("", timeout, rmsEvmPerSubcarrierMean)

         NR.ModAcc.Results.FetchRmsEvmPerSymbolMeanTrace("", timeout, rmsEvmPerSymbolMean)

         NR.ModAcc.Results.FetchSpectralFlatnessTrace("", timeout, spectralFlatness,
            spectralFlatnessLowerMask, spectralFlatnessUpperMask)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("------------------Measurement------------------" & vbLf)
         Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean)
         Console.WriteLine("Composite Peak EVM Maximum (%)                 : {0}", compositePeakEvmMaximum)
         Console.WriteLine("Composite Peak EVM Slot Index                  : {0}", compositePeakEvmSlotIndex)
         Console.WriteLine("Composite Peak EVM Symbol Index                : {0}", compositePeakEvmSymbolIndex)
         Console.WriteLine("Composite Peak EVM Subcarrier Index            : {0}", compositePeakEvmSubcarrierIndex)
         Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean)
         Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}", componentCarrierIQOriginOffsetMean)
         Console.WriteLine("Component Carrier IQ Gain Imbalance Mean (dB)  : {0}", componentCarrierIQGainImbalanceMean)
         Console.WriteLine("Component Carrier Quadrature Error Mean (deg)  : {0}", componentCarrierQuadratureErrorMean)
         Console.WriteLine("In-Band Emission Margin (dB)                   : {0}" & vbLf, inBandEmissionMargin)
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
