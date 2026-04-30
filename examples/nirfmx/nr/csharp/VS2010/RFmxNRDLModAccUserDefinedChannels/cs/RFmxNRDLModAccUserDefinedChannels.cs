//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Link Direction as Downlink, Frequency Range, Carrier Bandwidth, Cell ID, BWP Subcarrier Spacing,
//   Auto RB Detection Enabled, DL Channel Configuration Mode and Auto Increment Cell ID Enabled.
//7. Configure PDSCH and PDSCH RB Allocation.
//8. Configure PDSCH DMRS.
//9. Configure SSB.
//10. Select ModAcc measurement and enable Traces.
//11. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
//12. Configure Measurement Interval.
//13. Initiate the Measurement.
//14. Fetch ModAcc Measurements and Traces.
//15. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRDLModAccUserDefinedChannels
{
   public class RFmxNRDLModAccUserDefinedChannels
   {
      RFmxInstrMX instrSession;
      RFmxNRMX NR;
      string resourceName;

      string selectedPorts;
      double centerFrequency;
      double referenceLevel;
      double externalAttenuation;

      string frequencyReferenceSource;
      double frequencyReferenceFrequency;

      bool enableTrigger;
      string digitalEdgeSource;
      RFmxNRMXDigitalEdgeTriggerEdge digitalEdge;
      double triggerDelay;

      RFmxNRMXFrequencyRange frequencyRange;
      int cellID;
      double carrierBandwidth;
      double subcarrierSpacing;
      RFmxNRMXAutoResourceBlockDetectionEnabled autoResourceBlockDetectionEnabled;
      RFmxNRMXAutoIncrementCellIDEnabled autoIncrementCellIDEnabled;

      RFmxNRMXPdschModulationType pdschModulationType;
      const int NumberOfResourceBlockClusters = 1;
      int[] pdschResourceBlockOffset = new int[NumberOfResourceBlockClusters];
      int[] pdschNumberOfResourceBlocks = new int[NumberOfResourceBlockClusters];
      string pdschSlotAllocation;
      string pdschSymbolAllocation;

      RFmxNRMXPdschDmrsPowerMode pdschDmrsPowerMode;
      double pdschDmrsPower;
      RFmxNRMXPdschDmrsConfigurationType pdschDmrsConfigurationType;
      RFmxNRMXPdschMappingType pdschMappingType;
      int pdschDmrsTypeAPosition;
      RFmxNRMXPdschDmrsDuration pdschDmrsDuration;
      int pdschDmrsAdditionalPositions;

      RFmxNRMXSsbEnabled ssbEnabled;
      int ssbCrbOffset;
      int ssbSubcarrierOffset;
      RFmxNRMXSsbPattern ssbPattern;

      RFmxNRMXModAccSynchronizationMode synchronizationMode;

      RFmxNRMXModAccMeasurementLengthUnit measurementLengthUnit;
      double measurementOffset;
      double measurementLength;

      RFmxNRMXModAccAveragingEnabled averagingEnabled;
      int averagingCount;

      string subblockString;
      string carrierString;
      string bandwidthPartString;
      string userString;
      string pdschString;
      string pdschClusterString;

      double timeout;

      double compositeRmsEvmMean;                                                /* (%) */
      double compositePeakEvmMaximum;                                            /* (%) */
      int compositePeakEvmSlotIndex;
      int compositePeakEvmSymbolIndex;
      int compositePeakEvmSubcarrierIndex;

      double pdschQpskRmsEvmMean;                                                /* (%) */
      double pdsch16QamRmsEvmMean;                                               /* (%) */
      double pdsch64QamRmsEvmMean;                                               /* (%) */
      double pdsch256QamRmsEvmMean;                                              /* (%) */

      double componentCarrierFrequencyErrorMean;                                 /* (Hz) */
      double componentCarrierIQOriginOffsetMean;                                 /* (dBc) */
      double componentCarrierIQGainImbalanceMean;                                /* (dB) */
      double componentCarrierQuadratureErrorMean;                                /* (deg) */

      ComplexSingle[] qpskConstellation;
      ComplexSingle[] qam16Constellation;
      ComplexSingle[] qam64Constellation;
      ComplexSingle[] qam256Constellation;

      AnalogWaveform<float> rmsEvmPerSubcarrierMean;
      AnalogWaveform<float> rmsEvmPerSymbolMean;

      public void Run()
      {
         try
         {
            InitializeVariables();
            InitializeInstr();
            ConfigureNR();
            RetrieveResults();
            PrintResults();
         }
         catch (Exception ex)
         {
            DisplayError(ex);
         }
         finally
         {
            /* Close session */
            CloseSession();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
         }
      }

      void InitializeVariables()
      {
         resourceName = "RFSA";

         selectedPorts = "";
         centerFrequency = 3.5e9;                                                /* (Hz) */
         referenceLevel = 0.0;                                                   /* (dBm) */
         externalAttenuation = 0.0;                                              /* (dB) */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e6;                                   /* (Hz) */

         enableTrigger = false;
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0;
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                                     /* (s) */

         frequencyRange = RFmxNRMXFrequencyRange.Range1;
         cellID = 0;
         carrierBandwidth = 100e6;                                               /* (Hz) */
         subcarrierSpacing = 30e3;                                               /* (Hz) */
         autoResourceBlockDetectionEnabled = RFmxNRMXAutoResourceBlockDetectionEnabled.True;
         autoIncrementCellIDEnabled = RFmxNRMXAutoIncrementCellIDEnabled.True;

         pdschModulationType = RFmxNRMXPdschModulationType.Qpsk;
         pdschResourceBlockOffset[0] = 0;
         pdschNumberOfResourceBlocks[0] = -1;
         pdschSlotAllocation = "0-Last";
         pdschSymbolAllocation = "0-Last";

         pdschDmrsPowerMode = RFmxNRMXPdschDmrsPowerMode.CdmGroups;
         pdschDmrsPower = 0.0;                                                   /* (dB) */
         pdschDmrsConfigurationType = RFmxNRMXPdschDmrsConfigurationType.Type1;
         pdschMappingType = RFmxNRMXPdschMappingType.TypeA;
         pdschDmrsTypeAPosition = 2;
         pdschDmrsDuration = RFmxNRMXPdschDmrsDuration.SingleSymbol;
         pdschDmrsAdditionalPositions = 0;

         ssbEnabled = RFmxNRMXSsbEnabled.False;
         ssbCrbOffset = 0;
         ssbSubcarrierOffset = 0;
         ssbPattern = RFmxNRMXSsbPattern.CaseB3GHzTo6GHz;

         synchronizationMode = RFmxNRMXModAccSynchronizationMode.Slot;

         measurementLengthUnit = RFmxNRMXModAccMeasurementLengthUnit.Slot;
         measurementOffset = 0.0;
         measurementLength = 1;

         averagingEnabled = RFmxNRMXModAccAveragingEnabled.False;
         averagingCount = 10;

         timeout = 10.0;                                                         /* (s) */
      }

      void InitializeInstr()
      {
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      void ConfigureNR()
      {
         NR = instrSession.GetNRSignalConfiguration();     /* Create a new RFmx Session */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         NR.SetSelectedPorts("", selectedPorts);
         NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);

         NR.SetLinkDirection("", RFmxNRMXLinkDirection.Downlink);
         NR.SetFrequencyRange("", frequencyRange);
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth);
         NR.ComponentCarrier.SetCellID("", cellID);
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing);
         NR.SetAutoResourceBlockDetectionEnabled("", autoResourceBlockDetectionEnabled);
         NR.SetDownlinkChannelConfigurationMode("", RFmxNRMXDownlinkChannelConfigurationMode.UserDefined);
         NR.SetAutoIncrementCellIDEnabled("", autoIncrementCellIDEnabled);

         NR.ComponentCarrier.SetPdschModulationType("", pdschModulationType);
         NR.ComponentCarrier.SetPdschSlotAllocation("", pdschSlotAllocation);
         NR.ComponentCarrier.SetPdschSymbolAllocation("", pdschSymbolAllocation);

         NR.ComponentCarrier.SetPdschNumberOfResourceBlockClusters("", NumberOfResourceBlockClusters);

         subblockString = RFmxNRMX.BuildSubblockString("", 0);
         carrierString = RFmxNRMX.BuildCarrierString(subblockString, 0);
         bandwidthPartString = RFmxNRMX.BuildBandwidthPartString(carrierString, 0);
         userString = RFmxNRMX.BuildUserString(bandwidthPartString, 0);
         pdschString = RFmxNRMX.BuildPdschString(userString, 0);
         for (int i=0; i<NumberOfResourceBlockClusters; i++)
         {
            pdschClusterString = RFmxNRMX.BuildPdschClusterString(pdschString, i);
            NR.ComponentCarrier.SetPdschResourceBlockOffset(pdschClusterString, pdschResourceBlockOffset[i]);
            NR.ComponentCarrier.SetPdschNumberOfResourceBlocks(pdschClusterString, pdschNumberOfResourceBlocks[i]);
         }

         NR.ComponentCarrier.SetPdschDmrsPowerMode("", pdschDmrsPowerMode);
         NR.ComponentCarrier.SetPdschDmrsPower("", pdschDmrsPower);
         NR.ComponentCarrier.SetPdschDmrsConfigurationType("", pdschDmrsConfigurationType);
         NR.ComponentCarrier.SetPdschMappingType("", pdschMappingType);
         NR.ComponentCarrier.SetPdschDmrsTypeAPosition("", pdschDmrsTypeAPosition);
         NR.ComponentCarrier.SetPdschDmrsDuration("", pdschDmrsDuration);
         NR.ComponentCarrier.SetPdschDmrsAdditionalPositions("", pdschDmrsAdditionalPositions);

         NR.ComponentCarrier.SetSsbEnabled("", ssbEnabled);
         NR.ComponentCarrier.SetSsbCrbOffset("", ssbCrbOffset);
         NR.ComponentCarrier.SetSsbSubcarrierOffset("", ssbSubcarrierOffset);
         NR.ComponentCarrier.SetSsbPattern("", ssbPattern);

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc, true);

         NR.ModAcc.Configuration.SetSynchronizationMode("", synchronizationMode);
         NR.ModAcc.Configuration.SetAveragingEnabled("", averagingEnabled);
         NR.ModAcc.Configuration.SetAveragingCount("", averagingCount);

         NR.ModAcc.Configuration.SetMeasurementLengthUnit("", measurementLengthUnit);
         NR.ModAcc.Configuration.SetMeasurementOffset("", measurementOffset);
         NR.ModAcc.Configuration.SetMeasurementLength("", measurementLength);

         NR.Initiate("", "");
      }

      void RetrieveResults()
      {
         NR.ModAcc.Results.GetCompositeRmsEvmMean("", out compositeRmsEvmMean);
         NR.ModAcc.Results.GetCompositePeakEvmMaximum("", out compositePeakEvmMaximum);
         NR.ModAcc.Results.GetCompositePeakEvmSlotIndex("", out compositePeakEvmSlotIndex);
         NR.ModAcc.Results.GetCompositePeakEvmSymbolIndex("", out compositePeakEvmSymbolIndex);
         NR.ModAcc.Results.GetCompositePeakEvmSubcarrierIndex("", out compositePeakEvmSubcarrierIndex);

         NR.ModAcc.Results.GetComponentCarrierFrequencyErrorMean("", out componentCarrierFrequencyErrorMean);
         NR.ModAcc.Results.GetComponentCarrierIQOriginOffsetMean("", out componentCarrierIQOriginOffsetMean);
         NR.ModAcc.Results.GetComponentCarrierIQGainImbalanceMean("", out componentCarrierIQGainImbalanceMean);
         NR.ModAcc.Results.GetComponentCarrierQuadratureErrorMean("", out componentCarrierQuadratureErrorMean);

         NR.ModAcc.Results.GetPdschQpskRmsEvmMean("", out pdschQpskRmsEvmMean);
         NR.ModAcc.Results.GetPdsch16QamRmsEvmMean("", out pdsch16QamRmsEvmMean);
         NR.ModAcc.Results.GetPdsch64QamRmsEvmMean("", out pdsch64QamRmsEvmMean);
         NR.ModAcc.Results.GetPdsch256QamRmsEvmMean("", out pdsch256QamRmsEvmMean);

         NR.ModAcc.Results.FetchPdschQpskConstellationTrace("", timeout, ref qpskConstellation);
         NR.ModAcc.Results.FetchPdsch16QamConstellationTrace("", timeout, ref qam16Constellation);
         NR.ModAcc.Results.FetchPdsch64QamConstellationTrace("", timeout, ref qam64Constellation);
         NR.ModAcc.Results.FetchPdsch256QamConstellationTrace("", timeout, ref qam256Constellation);

         NR.ModAcc.Results.FetchRmsEvmPerSubcarrierMeanTrace("", timeout, ref rmsEvmPerSubcarrierMean);
         NR.ModAcc.Results.FetchRmsEvmPerSymbolMeanTrace("", timeout, ref rmsEvmPerSymbolMean);
      }

      void PrintResults()
      {
         Console.WriteLine("------------------Measurement------------------\n");
         Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean);
         Console.WriteLine("Composite Peak EVM Maximum (%)                 : {0}", compositePeakEvmMaximum);
         Console.WriteLine("Composite Peak EVM Slot Index                  : {0}", compositePeakEvmSlotIndex);
         Console.WriteLine("Composite Peak EVM Symbol Index                : {0}", compositePeakEvmSymbolIndex);
         Console.WriteLine("Composite Peak EVM Subcarrier Index            : {0}", compositePeakEvmSubcarrierIndex);
         Console.WriteLine("PDSCH QPSK RMS EVM Mean (%)                    : {0}", pdschQpskRmsEvmMean);
         Console.WriteLine("PDSCH 16QAM RMS EVM Mean (%)                   : {0}", pdsch16QamRmsEvmMean);
         Console.WriteLine("PDSCH 64QAM RMS EVM Mean (%)                   : {0}", pdsch64QamRmsEvmMean);
         Console.WriteLine("PDSCH 256QAM RMS EVM Mean (%)                  : {0}", pdsch256QamRmsEvmMean);
         Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean);
         Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}", componentCarrierIQOriginOffsetMean);
         Console.WriteLine("Component Carrier IQ Gain Imbalance Mean (dB)  : {0}", componentCarrierIQGainImbalanceMean);
         Console.WriteLine("Component Carrier Quadrature Error Mean (deg)  : {0}\n", componentCarrierQuadratureErrorMean);
      }

      void CloseSession()
      {
         if (NR != null)
         {
            NR.Dispose();
            NR = null;
         }
         if (instrSession != null)
         {
            instrSession.Close();
            instrSession = null;
         }
      }

      static void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

   }
}
