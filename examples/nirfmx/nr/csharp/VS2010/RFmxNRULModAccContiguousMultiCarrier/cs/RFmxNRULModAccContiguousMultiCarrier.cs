//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Frequency Range, Band, Channel Raster, Component Carrier Spacing, CC at Center Frequency
//   Auto RB Detection Enabled and Auto Increment Cell ID Enabled.
//7. Configure Carrier.
//8. Configure PUSCH and PUSCH RB Allocation.
//9. Configure PUSCH DMRS.
//10. Select ModAcc measurement and enable Traces.
//11. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
//12. Configure Measurement Interval.
//13. Initiate the Measurement.
//14. Fetch ModAcc Measurements and Traces.
//15. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRULModAccContiguousMultiCarrier
{
   public class RFmxNRULModAccContiguousMultiCarrier
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
      double subcarrierSpacing;
      RFmxNRMXAutoResourceBlockDetectionEnabled autoResourceBlockDetectionEnabled;
      RFmxNRMXAutoIncrementCellIDEnabled autoIncrementCellIDEnabled;

      RFmxNRMXComponentCarrierSpacingType componentCarrierSpacingType;
      double channelRaster;
      int componentCarrierAtCenterFrequency;

      const int NumberOfComponentCarriers = 2;
      double[] componentCarrierBandwidth = new double[NumberOfComponentCarriers];
      double[] componentCarrierFrequency = new double[NumberOfComponentCarriers];
      int[] cellID = new int[NumberOfComponentCarriers];

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
      string puschClusterString;

      double timeout;

      double[] compositeRmsEvmMean = new double[NumberOfComponentCarriers];                        /* (%) */
      double[] compositePeakEvmMaximum = new double[NumberOfComponentCarriers];                    /* (%) */
      int[] compositePeakEvmSlotIndex = new int[NumberOfComponentCarriers];
      int[] compositePeakEvmSymbolIndex = new int[NumberOfComponentCarriers];
      int[] compositePeakEvmSubcarrierIndex = new int[NumberOfComponentCarriers];
      double[] componentCarrierFrequencyErrorMean = new double[NumberOfComponentCarriers];         /* (Hz) */
      double[] componentCarrierIQOriginOffsetMean = new double[NumberOfComponentCarriers];         /* (dBc) */
      double[] componentCarrierIQGainImbalanceMean = new double[NumberOfComponentCarriers];        /* (dB) */
      double[] componentCarrierQuadratureErrorMean = new double[NumberOfComponentCarriers];        /* (deg) */
      double[] inBandEmissionMargin = new double[NumberOfComponentCarriers];                       /* (dB) */

      ComplexSingle[][] puschDataConstellation = new ComplexSingle[NumberOfComponentCarriers][];
      ComplexSingle[][] puschDmrsConstellation = new ComplexSingle[NumberOfComponentCarriers][];

      AnalogWaveform<float>[] rmsEvmPerSubcarrierMean = new AnalogWaveform<float>[NumberOfComponentCarriers];
      AnalogWaveform<float>[] rmsEvmPerSymbolMean = new AnalogWaveform<float>[NumberOfComponentCarriers];

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
         centerFrequency = 3.5e9;                                                      /* (Hz) */
         referenceLevel = 0.0;                                                         /* (dBm) */
         externalAttenuation = 0.0;                                                    /* (dB) */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e6;                                         /* (Hz) */

         enableTrigger = false;
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0;
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                                           /* (s) */

         frequencyRange = RFmxNRMXFrequencyRange.Range1;
         band = 78;
         subcarrierSpacing = 30e3;                                                     /* (Hz) */
         autoResourceBlockDetectionEnabled = RFmxNRMXAutoResourceBlockDetectionEnabled.True;
         autoIncrementCellIDEnabled = RFmxNRMXAutoIncrementCellIDEnabled.True;

         componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal;
         channelRaster = 15e3;                                                         /* (Hz) */
         componentCarrierAtCenterFrequency = -1;

         componentCarrierBandwidth[0] = 100e6;                                         /* (Hz) */
         componentCarrierBandwidth[1] = 100e6;                                         /* (Hz) */
         componentCarrierFrequency[0] = -49.98e6;                                      /* (Hz) */
         componentCarrierFrequency[1] = 50.01e6;                                       /* (Hz) */
         cellID[0] = 0;
         cellID[1] = 1;

         puschTransformPrecodingEnabled = RFmxNRMXPuschTransformPrecodingEnabled.False;
         puschModulationType = RFmxNRMXPuschModulationType.Qpsk;
         puschResourceBlockOffset[0] = 0;
         puschNumberOfResourceBlocks[0] = -1;
         puschSlotAllocation = "0-Last";
         puschSymbolAllocation = "0-Last";

         puschDmrsPowerMode = RFmxNRMXPuschDmrsPowerMode.CdmGroups;
         puschDmrsPower = 0.0;                                                         /* (dB) */
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

         timeout = 10.0;                                                               /* (s) */
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
         NR.SetBand("", band);
         NR.SetChannelRaster("", channelRaster);
         NR.SetComponentCarrierSpacingType("", componentCarrierSpacingType);
         NR.SetComponentCarrierAtCenterFrequency("", componentCarrierAtCenterFrequency);
         NR.SetAutoResourceBlockDetectionEnabled("", autoResourceBlockDetectionEnabled);
         NR.SetAutoIncrementCellIDEnabled("", autoIncrementCellIDEnabled);

         NR.ComponentCarrier.SetNumberOfComponentCarriers("", NumberOfComponentCarriers);

         subblockString = RFmxNRMX.BuildSubblockString("", 0);
         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i);
            NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth[i]);
            NR.ComponentCarrier.SetCellID(carrierString, cellID[i]);
            NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency[i]);
         }

         carrierString = "carrier::all";
         NR.ComponentCarrier.SetPuschTransformPrecodingEnabled(carrierString, puschTransformPrecodingEnabled);
         NR.ComponentCarrier.SetPuschModulationType(carrierString, puschModulationType);
         NR.ComponentCarrier.SetPuschSlotAllocation(carrierString, puschSlotAllocation);
         NR.ComponentCarrier.SetPuschSymbolAllocation(carrierString, puschSymbolAllocation);

         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing);
         NR.ComponentCarrier.SetPuschNumberOfResourceBlockClusters(carrierString, NumberOfResourceBlockClusters);

         for (int i = 0; i < NumberOfResourceBlockClusters; i++)
         {
            puschClusterString = RFmxNRMX.BuildPuschClusterString(carrierString, i);
            NR.ComponentCarrier.SetPuschResourceBlockOffset(puschClusterString, puschResourceBlockOffset[i]);
            NR.ComponentCarrier.SetPuschNumberOfResourceBlocks(puschClusterString, puschNumberOfResourceBlocks[i]);
         }

         NR.ComponentCarrier.SetPuschDmrsPowerMode(carrierString, puschDmrsPowerMode);
         NR.ComponentCarrier.SetPuschDmrsPower(carrierString, puschDmrsPower);
         NR.ComponentCarrier.SetPuschDmrsConfigurationType(carrierString, puschDmrsConfigurationType);
         NR.ComponentCarrier.SetPuschMappingType(carrierString, puschMappingType);
         NR.ComponentCarrier.SetPuschDmrsTypeAPosition(carrierString, puschDmrsTypeAPosition);
         NR.ComponentCarrier.SetPuschDmrsDuration(carrierString, puschDmrsDuration);
         NR.ComponentCarrier.SetPuschDmrsAdditionalPositions(carrierString, puschDmrsAdditionalPositions);

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
         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i);

            NR.ModAcc.Results.GetCompositeRmsEvmMean(carrierString, out compositeRmsEvmMean[i]);
            NR.ModAcc.Results.GetCompositePeakEvmMaximum(carrierString, out compositePeakEvmMaximum[i]);
            NR.ModAcc.Results.GetCompositePeakEvmSlotIndex(carrierString, out compositePeakEvmSlotIndex[i]);
            NR.ModAcc.Results.GetCompositePeakEvmSymbolIndex(carrierString, out compositePeakEvmSymbolIndex[i]);
            NR.ModAcc.Results.GetCompositePeakEvmSubcarrierIndex(carrierString,
               out compositePeakEvmSubcarrierIndex[i]);
            NR.ModAcc.Results.GetComponentCarrierFrequencyErrorMean(carrierString,
               out componentCarrierFrequencyErrorMean[i]);
            NR.ModAcc.Results.GetComponentCarrierIQOriginOffsetMean(carrierString,
               out componentCarrierIQOriginOffsetMean[i]);
            NR.ModAcc.Results.GetComponentCarrierIQGainImbalanceMean(carrierString,
               out componentCarrierIQGainImbalanceMean[i]);
            NR.ModAcc.Results.GetComponentCarrierQuadratureErrorMean(carrierString,
               out componentCarrierQuadratureErrorMean[i]);
            NR.ModAcc.Results.GetInBandEmissionMargin(carrierString, out inBandEmissionMargin[i]);
         }

         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            carrierString = RFmxNRMX.BuildCarrierString("", i);
            NR.ModAcc.Results.FetchPuschDataConstellationTrace(carrierString, timeout, ref puschDataConstellation[i]);
            NR.ModAcc.Results.FetchPuschDmrsConstellationTrace(carrierString, timeout, ref puschDmrsConstellation[i]);
         }

         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i);
            NR.ModAcc.Results.FetchRmsEvmPerSubcarrierMeanTrace(carrierString, timeout, ref rmsEvmPerSubcarrierMean[i]);
            NR.ModAcc.Results.FetchRmsEvmPerSymbolMeanTrace(carrierString, timeout, ref rmsEvmPerSymbolMean[i]);
         }

         NR.ModAcc.Results.FetchSpectralFlatnessTrace("", timeout, ref spectralFlatness,
            ref spectralFlatnessLowerMask, ref spectralFlatnessUpperMask);
      }

      void PrintResults()
      {
         Console.WriteLine("------------------------Measurements------------------------\n");
         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            Console.WriteLine("Carrier  : {0}", i);
            Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean[i]);
            Console.WriteLine("Composite Peak EVM Maximum (%)                 : {0}", compositePeakEvmMaximum[i]);
            Console.WriteLine("Composite Peak EVM Slot Index                  : {0}", compositePeakEvmSlotIndex[i]);
            Console.WriteLine("Composite Peak EVM Symbol Index                : {0}", compositePeakEvmSymbolIndex[i]);
            Console.WriteLine("Composite Peak EVM Subcarrier Index            : {0}", compositePeakEvmSubcarrierIndex[i]);
            Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean[i]);
            Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}", componentCarrierIQOriginOffsetMean[i]);
            Console.WriteLine("Component Carrier IQ Gain Imbalance Mean (dB)  : {0}", componentCarrierIQGainImbalanceMean[i]);
            Console.WriteLine("Component Carrier Quadrature Error Mean (deg)  : {0}", componentCarrierQuadratureErrorMean[i]);
            Console.WriteLine("In-Band Emission Margin (dB)                   : {0}", inBandEmissionMargin[i]);
            Console.WriteLine("-----------------------------------------------------------------\n");
         }
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
