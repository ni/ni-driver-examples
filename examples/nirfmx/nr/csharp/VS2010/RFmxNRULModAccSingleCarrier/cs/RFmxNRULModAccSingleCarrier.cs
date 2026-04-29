//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Frequency Range, CC bandwidth, Cell ID, Band, BWP Subcarrier Spacing and Auto RB Detection Enabled.
//7. Configure PUSCH and PUSCH RB Allocation.
//8. Configure PUSCH DMRS.
//9. Select ModAcc measurement and enable Traces.
//10. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
//11. Configure Measurement Interval.
//12. Initiate the Measurement.
//13. Fetch ModAcc Measurements and Traces.
//14. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRULModAccSingleCarrier
{
   public class RFmxNRULModAccSingleCarrier
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
      int band;
      int cellID;
      double carrierBandwidth;
      double subcarrierSpacing;
      RFmxNRMXAutoResourceBlockDetectionEnabled autoResourceBlockDetectionEnabled;

      RFmxNRMXPuschTransformPrecodingEnabled puschTransformPrecodingEnabled;
      RFmxNRMXPuschModulationType puschModulationType;
      const int NumberOfResourceBlockClusters = 1;
      int[] puschResourceBlockOffset = new int[NumberOfResourceBlockClusters];
      int[] puschNumberOfResourceBlocks = new int[NumberOfResourceBlockClusters];
      string puschSlotAllocation;
      string puschSymbolAllocation;

      RFmxNRMXPuschDmrsPowerMode puschDmrsPowerMode;
      double puschDmrsPower;
      RFmxNRMXPuschDmrsConfigurationType puschDmrsConfigurationType;
      RFmxNRMXPuschMappingType puschMappingType;
      int puschDmrsTypeAPosition;
      RFmxNRMXPuschDmrsDuration puschDmrsDuration;
      int puschDmrsAdditionalPositions;

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
      string puschString;
      string puschClusterString;

      double timeout;

      double compositeRmsEvmMean;                                                /* (%) */
      double compositePeakEvmMaximum;                                            /* (%) */
      int compositePeakEvmSlotIndex;
      int compositePeakEvmSymbolIndex;
      int compositePeakEvmSubcarrierIndex;

      double componentCarrierFrequencyErrorMean;                                 /* (Hz) */
      double componentCarrierIQOriginOffsetMean;                                 /* (dBc) */
      double componentCarrierIQGainImbalanceMean;                                /* (dB) */
      double componentCarrierQuadratureErrorMean;                                /* (deg) */
      double inBandEmissionMargin;                                               /* (dB) */

      ComplexSingle[] puschDataConstellation, puschDmrsConstellation;

      AnalogWaveform<float> rmsEvmPerSubcarrierMean;
      AnalogWaveform<float> rmsEvmPerSymbolMean;

      Spectrum<float> spectralFlatness;
      Spectrum<float> spectralFlatnessLowerMask;
      Spectrum<float> spectralFlatnessUpperMask;

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
         band = 78;
         cellID = 0;
         carrierBandwidth = 100e6;                                               /* (Hz) */
         subcarrierSpacing = 30e3;                                               /* (Hz) */
         autoResourceBlockDetectionEnabled = RFmxNRMXAutoResourceBlockDetectionEnabled.True;

         puschTransformPrecodingEnabled = RFmxNRMXPuschTransformPrecodingEnabled.False;
         puschModulationType = RFmxNRMXPuschModulationType.Qpsk;
         puschResourceBlockOffset[0] = 0;
         puschNumberOfResourceBlocks[0] = -1;
         puschSlotAllocation = "0-Last";
         puschSymbolAllocation = "0-Last";

         puschDmrsPowerMode = RFmxNRMXPuschDmrsPowerMode.CdmGroups;
         puschDmrsPower = 0.0;                                                   /* (dB) */
         puschDmrsConfigurationType = RFmxNRMXPuschDmrsConfigurationType.Type1;
         puschMappingType = RFmxNRMXPuschMappingType.TypeA;
         puschDmrsTypeAPosition = 2;
         puschDmrsDuration = RFmxNRMXPuschDmrsDuration.SingleSymbol;
         puschDmrsAdditionalPositions = 0;

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

         NR.SetFrequencyRange("", frequencyRange);
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth);
         NR.ComponentCarrier.SetCellID("", cellID);
         NR.SetBand("", band);
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing);
         NR.SetAutoResourceBlockDetectionEnabled("", autoResourceBlockDetectionEnabled);

         NR.ComponentCarrier.SetPuschTransformPrecodingEnabled("", puschTransformPrecodingEnabled);
         NR.ComponentCarrier.SetPuschSlotAllocation("", puschSlotAllocation);
         NR.ComponentCarrier.SetPuschSymbolAllocation("", puschSymbolAllocation);
         NR.ComponentCarrier.SetPuschModulationType("", puschModulationType);

         NR.ComponentCarrier.SetPuschNumberOfResourceBlockClusters("", NumberOfResourceBlockClusters);

         subblockString = RFmxNRMX.BuildSubblockString("", 0);
         carrierString = RFmxNRMX.BuildCarrierString(subblockString, 0);
         bandwidthPartString = RFmxNRMX.BuildBandwidthPartString(carrierString, 0);
         userString = RFmxNRMX.BuildUserString(bandwidthPartString, 0);
         puschString = RFmxNRMX.BuildPuschString(userString, 0);
         for (int i = 0; i < NumberOfResourceBlockClusters; i++)
         {
            puschClusterString = RFmxNRMX.BuildPuschClusterString(puschString, i);
            NR.ComponentCarrier.SetPuschResourceBlockOffset(puschClusterString, puschResourceBlockOffset[i]);
            NR.ComponentCarrier.SetPuschNumberOfResourceBlocks(puschClusterString, puschNumberOfResourceBlocks[i]);
         }

         NR.ComponentCarrier.SetPuschDmrsPowerMode("", puschDmrsPowerMode);
         NR.ComponentCarrier.SetPuschDmrsPower("", puschDmrsPower);
         NR.ComponentCarrier.SetPuschDmrsConfigurationType("", puschDmrsConfigurationType);
         NR.ComponentCarrier.SetPuschMappingType("", puschMappingType);
         NR.ComponentCarrier.SetPuschDmrsTypeAPosition("", puschDmrsTypeAPosition);
         NR.ComponentCarrier.SetPuschDmrsDuration("", puschDmrsDuration);
         NR.ComponentCarrier.SetPuschDmrsAdditionalPositions("", puschDmrsAdditionalPositions);

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
         NR.ModAcc.Results.GetInBandEmissionMargin("", out inBandEmissionMargin);

         NR.ModAcc.Results.FetchPuschDataConstellationTrace("", timeout, ref puschDataConstellation);

         NR.ModAcc.Results.FetchPuschDmrsConstellationTrace("", timeout, ref puschDmrsConstellation);

         NR.ModAcc.Results.FetchRmsEvmPerSubcarrierMeanTrace("", timeout, ref rmsEvmPerSubcarrierMean);

         NR.ModAcc.Results.FetchRmsEvmPerSymbolMeanTrace("", timeout, ref rmsEvmPerSymbolMean);

         NR.ModAcc.Results.FetchSpectralFlatnessTrace("", timeout, ref spectralFlatness,
            ref spectralFlatnessLowerMask, ref spectralFlatnessUpperMask);
      }

      void PrintResults()
      {
         Console.WriteLine("------------------Measurement------------------\n");
         Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean);
         Console.WriteLine("Composite Peak EVM Maximum (%)                 : {0}", compositePeakEvmMaximum);
         Console.WriteLine("Composite Peak EVM Slot Index                  : {0}", compositePeakEvmSlotIndex);
         Console.WriteLine("Composite Peak EVM Symbol Index                : {0}", compositePeakEvmSymbolIndex);
         Console.WriteLine("Composite Peak EVM Subcarrier Index            : {0}", compositePeakEvmSubcarrierIndex);
         Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean);
         Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}", componentCarrierIQOriginOffsetMean);
         Console.WriteLine("Component Carrier IQ Gain Imbalance Mean (dB)  : {0}", componentCarrierIQGainImbalanceMean);
         Console.WriteLine("Component Carrier Quadrature Error Mean (deg)  : {0}", componentCarrierQuadratureErrorMean);
         Console.WriteLine("In-Band Emission Margin (dB)                   : {0}\n", inBandEmissionMargin);
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
