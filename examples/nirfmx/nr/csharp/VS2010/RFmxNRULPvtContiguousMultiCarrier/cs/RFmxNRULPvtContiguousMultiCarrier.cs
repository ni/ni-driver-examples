//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Parameters for IQ Power Edge Trigger.
//6. Configure Frequency Range, Channel Raster, Component Carrier Spacing and Number of Component Carriers.
//7. Configure Component Carriers.
//8. Configure PUSCH and PUSCH RB Allocation.
//9. Configure PUSCH DMRS.
//10. Select PVT measurement and enable Traces.
//11. Configure Measurement Methods.
//12. Configure OFF Power Exclusion Periods.
//13. Configure Averaging Parameters for PVT measurement.
//14. Initiate the Measurement.
//15. Fetch PVT Traces and Measurements.
//16. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRULPvtContiguousMultiCarrier
{
   public class RFmxNRULPvtContiguousMultiCarrier
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

      double iqPowerEdgeLevel;
      double triggerDelay;
      RFmxNRMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
      double minimumQuietTime;

      RFmxNRMXFrequencyRange frequencyRange;

      RFmxNRMXComponentCarrierSpacingType componentCarrierSpacingType;
      double channelRaster;
      int componentCarrierAtCenterFrequency;
      double subcarrierSpacing;

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

      RFmxNRMXPvtMeasurementMethod measurementMethod;
      double offPowerExclusionBefore;
      double offPowerExclusionAfter;

      RFmxNRMXPvtAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxNRMXPvtAveragingType averagingType;

      string subblockString;
      string carrierString;
      string puschClusterString;

      double timeout;

      RFmxNRMXPvtMeasurementStatus[] measurementStatus = new RFmxNRMXPvtMeasurementStatus[NumberOfComponentCarriers];
      double[] absoluteOffPowerBefore = new double[NumberOfComponentCarriers];                        /* (dBm) */
      double[] absoluteOffPowerAfter = new double[NumberOfComponentCarriers];                         /* (dBm) */
      double[] absoluteONPower = new double[NumberOfComponentCarriers];                               /* (dBm) */
      double[] burstWidth = new double[NumberOfComponentCarriers];                                    /* (s) */

      AnalogWaveform<float>[] signalPower = new AnalogWaveform<float>[NumberOfComponentCarriers];
      AnalogWaveform<float>[] absoluteLimit = new AnalogWaveform<float>[NumberOfComponentCarriers];

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

         iqPowerEdgeLevel = -20.0;                                                     /* (dB) */
         triggerDelay = 0.0;                                                           /* (s) */
         minimumQuietTimeMode = RFmxNRMXTriggerMinimumQuietTimeMode.Auto;
         minimumQuietTime = 8.0e-6;                                                    /* (s) */

         frequencyRange = RFmxNRMXFrequencyRange.Range1;

         componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal;
         channelRaster = 15e3;                                                         /* (Hz) */
         componentCarrierAtCenterFrequency = -1;
         subcarrierSpacing = 30e3;                                                     /* (Hz) */

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
         puschSlotAllocation = "1";
         puschSymbolAllocation = "0-Last";

         puschDmrsPowerMode = RFmxNRMXPuschDmrsPowerMode.CdmGroups;
         puschDmrsPower = 0.0;                                                         /* (dB) */
         puschDmrsConfigurationType = RFmxNRMXPuschDmrsConfigurationType.Type1;
         puschMappingType = RFmxNRMXPuschMappingType.TypeA;
         puschDmrsTypeAPosition = 2;
         puschDmrsDuration = RFmxNRMXPuschDmrsDuration.SingleSymbol;
         puschDmrsAdditionalPositions = 0;

         measurementMethod = RFmxNRMXPvtMeasurementMethod.Normal;
         offPowerExclusionBefore = 0.0;                                                /* (s) */
         offPowerExclusionAfter = 0.0;                                                 /* (s) */

         averagingEnabled = RFmxNRMXPvtAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxNRMXPvtAveragingType.Rms;

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
         NR.ConfigureIQPowerEdgeTrigger("", "0", RFmxNRMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
            triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxNRMXIQPowerEdgeTriggerLevelType.Relative, true);

         NR.SetFrequencyRange("", frequencyRange);
         NR.SetChannelRaster("", channelRaster);
         NR.SetComponentCarrierSpacingType("", componentCarrierSpacingType);
         NR.SetComponentCarrierAtCenterFrequency("", componentCarrierAtCenterFrequency);
         NR.ComponentCarrier.SetNumberOfComponentCarriers("", NumberOfComponentCarriers);

         subblockString = RFmxNRMX.BuildSubblockString("", 0);
         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i);
            NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth[i]);
            NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency[i]);
            NR.ComponentCarrier.SetCellID(carrierString, cellID[i]);
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

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Pvt, true);

         NR.Pvt.Configuration.ConfigureMeasurementMethod("", measurementMethod);
         NR.Pvt.Configuration.ConfigureOffPowerExclusionPeriods("", offPowerExclusionBefore, offPowerExclusionAfter);
         NR.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);

         NR.Initiate("", "");
      }

      void RetrieveResults()
      {
         NR.Pvt.Results.FetchMeasurementArray("", timeout, ref measurementStatus, ref absoluteOffPowerBefore,
            ref absoluteOffPowerAfter, ref absoluteONPower, ref burstWidth);

         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i);
            NR.Pvt.Results.FetchSignalPowerTrace(carrierString, timeout, ref signalPower[i], ref absoluteLimit[i]);
         }
      }

      void PrintResults()
      {
         Console.WriteLine("------------------------Measurements------------------------\n");
         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            Console.WriteLine("Carrier  : {0}", i);
            Console.WriteLine("Measurement Status                       : {0}", measurementStatus[i]);
            Console.WriteLine("Mean Absolute OFF power Before (dBm)     : {0}", absoluteOffPowerBefore[i]);
            Console.WriteLine("Mean Absolute OFF power After (dBm)      : {0}", absoluteOffPowerAfter[i]);
            Console.WriteLine("Mean Absolute ON power (dBm)             : {0}", absoluteONPower[i]);
            Console.WriteLine("Burst Width (s)                          : {0}", burstWidth[i]);
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
