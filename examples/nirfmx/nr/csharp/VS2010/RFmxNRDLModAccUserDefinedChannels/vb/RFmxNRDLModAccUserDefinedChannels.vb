'Steps:
'1. Open a new RFmx Session.
'2. Configure Frequency Reference.
'3. Configure Selected Ports.
'4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
'5. Configure Trigger Type and Trigger Parameters.
'6. Configure Link Direction as Downlink, Frequency Range, Carrier Bandwidth, Cell ID, BWP Subcarrier Spacing, 
'   Auto RB Detection Enabled, DL Channel Configuration Mode and Auto Increment Cell ID Enabled.
'7. Configure PDSCH and PDSCH RB Allocation.
'8. Configure PDSCH DMRS.
'9. Configure SSB.
'10. Select ModAcc measurement and enable Traces.
'11. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
'12. Configure Measurement Interval.
'13. Initiate the Measurement.
'14. Fetch ModAcc Measurements and Traces.
'15. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.NRMX

Namespace NationalInstruments.Examples.RFmxNRDLModAccUserDefinedChannels
   Public Class RFmxNRDLModAccUserDefinedChannels
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
      Private cellID As Integer
      Private carrierBandwidth As Double
      Private subcarrierSpacing As Double
      Private autoResourceBlockDetectionEnabled As RFmxNRMXAutoResourceBlockDetectionEnabled
      Private autoIncrementCellIDEnabled As RFmxNRMXAutoIncrementCellIDEnabled

      Private pdschModulationType As RFmxNRMXPdschModulationType
      Const NumberOfResourceBlockClusters As Integer = 1
      Private pdschResourceBlockOffset As Integer() = New Integer(NumberOfResourceBlockClusters - 1) {}
      Private pdschNumberOfResourceBlocks As Integer() = New Integer(NumberOfResourceBlockClusters - 1) {}
      Private pdschSlotAllocation As String
      Private pdschSymbolAllocation As String

      Private pdschDmrsPowerMode As RFmxNRMXPdschDmrsPowerMode
      Private pdschDmrsPower As Double
      Private pdschDmrsConfigurationType As RFmxNRMXPdschDmrsConfigurationType
      Private pdschMappingType As RFmxNRMXPdschMappingType
      Private pdschDmrsTypeAPosition As Integer
      Private pdschDmrsDuration As RFmxNRMXPdschDmrsDuration
      Private pdschDmrsAdditionalPositions As Integer

      Private ssbEnabled As RFmxNRMXSsbEnabled
      Private ssbCrbOffset As Integer
      Private ssbSubcarrierOffset As Integer
      Private ssbPattern As RFmxNRMXSsbPattern

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
      Private pdschString As String
      Private pdschClusterString As String

      Private timeout As Double

      Private compositeRmsEvmMean As Double
      ' (%)
      Private compositePeakEvmMaximum As Double
      ' (%)
      Private compositePeakEvmSlotIndex As Integer
      Private compositePeakEvmSymbolIndex As Integer
      Private compositePeakEvmSubcarrierIndex As Integer

      Private pdschQpskRmsEvmMean As Double
      ' (%)
      Private pdsch16QamRmsEvmMean As Double
      ' (%)
      Private pdsch64QamRmsEvmMean As Double
      ' (%)
      Private pdsch256QamRmsEvmMean As Double
      ' (%)

      Private componentCarrierFrequencyErrorMean As Double
      ' (Hz)
      Private componentCarrierIQOriginOffsetMean As Double
      ' (dBc)
      Private componentCarrierIQGainImbalanceMean As Double
      ' (dB)
      Private componentCarrierQuadratureErrorMean As Double
      ' (deg)

      Private qpskConstellation As ComplexSingle()
      Private qam16Constellation As ComplexSingle()
      Private qam64Constellation As ComplexSingle()
      Private qam256Constellation As ComplexSingle()

      Private rmsEvmPerSubcarrierMean As AnalogWaveform(Of Single)
      Private rmsEvmPerSymbolMean As AnalogWaveform(Of Single)

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
         cellID = 0
         carrierBandwidth = 100000000.0
         ' (Hz)
         subcarrierSpacing = 30000.0
         ' (Hz)
         autoResourceBlockDetectionEnabled = RFmxNRMXAutoResourceBlockDetectionEnabled.True
         autoIncrementCellIDEnabled = RFmxNRMXAutoIncrementCellIDEnabled.True

         pdschModulationType = RFmxNRMXPdschModulationType.Qpsk
         pdschResourceBlockOffset(0) = 0
         pdschNumberOfResourceBlocks(0) = -1
         pdschSlotAllocation = "0-Last"
         pdschSymbolAllocation = "0-Last"

         pdschDmrsPowerMode = RFmxNRMXPdschDmrsPowerMode.CdmGroups
         pdschDmrsPower = 0.0
         ' (dB)
         pdschDmrsConfigurationType = RFmxNRMXPdschDmrsConfigurationType.Type1
         pdschMappingType = RFmxNRMXPdschMappingType.TypeA
         pdschDmrsTypeAPosition = 2
         pdschDmrsDuration = RFmxNRMXPdschDmrsDuration.SingleSymbol
         pdschDmrsAdditionalPositions = 0

         ssbEnabled = RFmxNRMXSsbEnabled.False
         ssbCrbOffset = 0
         ssbSubcarrierOffset = 0
         ssbPattern = RFmxNRMXSsbPattern.CaseB3GHzTo6GHz

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

         NR.SetLinkDirection("", RFmxNRMXLinkDirection.Downlink)
         NR.SetFrequencyRange("", frequencyRange)
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth)
         NR.ComponentCarrier.SetCellID("", cellID)
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing)
         NR.SetAutoResourceBlockDetectionEnabled("", autoResourceBlockDetectionEnabled)
         NR.SetDownlinkChannelConfigurationMode("", RFmxNRMXDownlinkChannelConfigurationMode.UserDefined)
         NR.SetAutoIncrementCellIDEnabled("", autoIncrementCellIDEnabled)

         NR.ComponentCarrier.SetPdschModulationType("", pdschModulationType)
         NR.ComponentCarrier.SetPdschSlotAllocation("", pdschSlotAllocation)
         NR.ComponentCarrier.SetPdschSymbolAllocation("", pdschSymbolAllocation)

         NR.ComponentCarrier.SetPdschNumberOfResourceBlockClusters("", NumberOfResourceBlockClusters)

         subblockString = RFmxNRMX.BuildSubblockString("", 0)
         carrierString = RFmxNRMX.BuildCarrierString(subblockString, 0)
         bandwidthPartString = RFmxNRMX.BuildBandwidthPartString(carrierString, 0)
         userString = RFmxNRMX.BuildUserString(bandwidthPartString, 0)
         pdschString = RFmxNRMX.BuildPdschString(userString, 0)
         For i As Integer = 0 To NumberOfResourceBlockClusters - 1
            pdschClusterString = RFmxNRMX.BuildPdschClusterString(pdschString, i)
            NR.ComponentCarrier.SetPdschResourceBlockOffset(pdschClusterString, pdschResourceBlockOffset(i))
            NR.ComponentCarrier.SetPdschNumberOfResourceBlocks(pdschClusterString, pdschNumberOfResourceBlocks(i))
         Next

         NR.ComponentCarrier.SetPdschDmrsPowerMode("", pdschDmrsPowerMode)
         NR.ComponentCarrier.SetPdschDmrsPower("", pdschDmrsPower)
         NR.ComponentCarrier.SetPdschDmrsConfigurationType("", pdschDmrsConfigurationType)
         NR.ComponentCarrier.SetPdschMappingType("", pdschMappingType)
         NR.ComponentCarrier.SetPdschDmrsTypeAPosition("", pdschDmrsTypeAPosition)
         NR.ComponentCarrier.SetPdschDmrsDuration("", pdschDmrsDuration)
         NR.ComponentCarrier.SetPdschDmrsAdditionalPositions("", pdschDmrsAdditionalPositions)

         NR.ComponentCarrier.SetSsbEnabled("", ssbEnabled)
         NR.ComponentCarrier.SetSsbCrbOffset("", ssbCrbOffset)
         NR.ComponentCarrier.SetSsbSubcarrierOffset("", ssbSubcarrierOffset)
         NR.ComponentCarrier.SetSsbPattern("", ssbPattern)

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

         NR.ModAcc.Results.GetPdschQpskRmsEvmMean("", pdschQpskRmsEvmMean)
         NR.ModAcc.Results.GetPdsch16QamRmsEvmMean("", pdsch16QamRmsEvmMean)
         NR.ModAcc.Results.GetPdsch64QamRmsEvmMean("", pdsch64QamRmsEvmMean)
         NR.ModAcc.Results.GetPdsch256QamRmsEvmMean("", pdsch256QamRmsEvmMean)

         NR.ModAcc.Results.FetchPdschQpskConstellationTrace("", timeout, qpskConstellation)
         NR.ModAcc.Results.FetchPdsch16QamConstellationTrace("", timeout, qam16Constellation)
         NR.ModAcc.Results.FetchPdsch64QamConstellationTrace("", timeout, qam64Constellation)
         NR.ModAcc.Results.FetchPdsch256QamConstellationTrace("", timeout, qam256Constellation)

         NR.ModAcc.Results.FetchRmsEvmPerSubcarrierMeanTrace("", timeout, rmsEvmPerSubcarrierMean)
         NR.ModAcc.Results.FetchRmsEvmPerSymbolMeanTrace("", timeout, rmsEvmPerSymbolMean)
      End Sub

      Private Sub PrintResults()
         Console.WriteLine("------------------Measurement------------------" & vbLf)
         Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean)
         Console.WriteLine("Composite Peak EVM Maximum (%)                 : {0}", compositePeakEvmMaximum)
         Console.WriteLine("Composite Peak EVM Slot Index                  : {0}", compositePeakEvmSlotIndex)
         Console.WriteLine("Composite Peak EVM Symbol Index                : {0}", compositePeakEvmSymbolIndex)
         Console.WriteLine("Composite Peak EVM Subcarrier Index            : {0}", compositePeakEvmSubcarrierIndex)
         Console.WriteLine("PDSCH QPSK RMS EVM Mean (%)                    : {0}", pdschQpskRmsEvmMean)
         Console.WriteLine("PDSCH 16QAM RMS EVM Mean (%)                   : {0}", pdsch16QamRmsEvmMean)
         Console.WriteLine("PDSCH 64QAM RMS EVM Mean (%)                   : {0}", pdsch64QamRmsEvmMean)
         Console.WriteLine("PDSCH 256QAM RMS EVM Mean (%)                  : {0}", pdsch256QamRmsEvmMean)
         Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean)
         Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}", componentCarrierIQOriginOffsetMean)
         Console.WriteLine("Component Carrier IQ Gain Imbalance Mean (dB)  : {0}", componentCarrierIQGainImbalanceMean)
         Console.WriteLine("Component Carrier Quadrature Error Mean (deg)  : {0}" & vbLf, componentCarrierQuadratureErrorMean)
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
