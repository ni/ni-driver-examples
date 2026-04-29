//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Parameters for IQ Power Edge Trigger.
//6. Configure Frequency Range, Carrier Bandwidth, Cell ID and Subcarrier Spacing.
//7. Configure PUSCH and PUSCH RB Allocation.
//8. Configure PUSCH DMRS.
//9. Select PVT measurement and enable Traces.
//10. Configure Measurement Methods.
//11. Configure OFF Power Exclusion Periods.
//12. Configure Averaging Parameters for PVT measurement.
//13. Initiate the Measurement.
//14. Fetch PVT Traces and Measurements.
//15. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRULPvtSingleCarrier
{
   public class RFmxNRULPvtSingleCarrier
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
      int cellID;
      double carrierBandwidth;
      double subcarrierSpacing;

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
      string bandwidthPartString;
      string userString;
      string puschString;
      string puschClusterString;

      double timeout;

      RFmxNRMXPvtMeasurementStatus measurementStatus;
      double absoluteOffPowerBefore;                                             /* (dBm) */
      double absoluteOffPowerAfter;                                              /* (dBm) */
      double absoluteONPower;                                                    /* (dBm) */
      double burstWidth;                                                         /* (s) */

      AnalogWaveform<float> signalPower;                                         /* (dBm) */
      AnalogWaveform<float> absoluteLimit;                                       /* (dBm) */

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

         iqPowerEdgeLevel = -20.0;                                               /* (dB) */
         triggerDelay = 0.0;                                                     /* (s) */
         minimumQuietTimeMode = RFmxNRMXTriggerMinimumQuietTimeMode.Auto;
         minimumQuietTime = 8.0e-6;                                              /* (s) */

         frequencyRange = RFmxNRMXFrequencyRange.Range1;
         cellID = 0;
         carrierBandwidth = 100e6;                                               /* (Hz) */
         subcarrierSpacing = 30e3;                                               /* (Hz) */

         puschTransformPrecodingEnabled = RFmxNRMXPuschTransformPrecodingEnabled.False;
         puschModulationType = RFmxNRMXPuschModulationType.Qpsk;
         puschResourceBlockOffset[0] = 0;
         puschNumberOfResourceBlocks[0] = -1;
         puschSlotAllocation = "1";
         puschSymbolAllocation = "0-Last";

         puschDmrsPowerMode = RFmxNRMXPuschDmrsPowerMode.CdmGroups;
         puschDmrsPower = 0.0;                                                   /* (dB) */
         puschDmrsConfigurationType = RFmxNRMXPuschDmrsConfigurationType.Type1;
         puschMappingType = RFmxNRMXPuschMappingType.TypeA;
         puschDmrsTypeAPosition = 2;
         puschDmrsDuration = RFmxNRMXPuschDmrsDuration.SingleSymbol;
         puschDmrsAdditionalPositions = 0;

         measurementMethod = RFmxNRMXPvtMeasurementMethod.Normal;
         offPowerExclusionBefore = 0.0;                                          /* (s) */
         offPowerExclusionAfter = 0.0;                                           /* (s) */

         averagingEnabled = RFmxNRMXPvtAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxNRMXPvtAveragingType.Rms;

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
         NR.ConfigureIQPowerEdgeTrigger("", "0", RFmxNRMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
            triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxNRMXIQPowerEdgeTriggerLevelType.Relative, true);

         NR.SetFrequencyRange("", frequencyRange);
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth);
         NR.ComponentCarrier.SetCellID("", cellID);
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing);

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

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Pvt, true);

         NR.Pvt.Configuration.ConfigureMeasurementMethod("", measurementMethod);
         NR.Pvt.Configuration.ConfigureOffPowerExclusionPeriods("", offPowerExclusionBefore, offPowerExclusionAfter);
         NR.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);

         NR.Initiate("", "");
      }

      void RetrieveResults()
      {
         NR.Pvt.Results.FetchMeasurement("", timeout, out measurementStatus, out absoluteOffPowerBefore,
            out absoluteOffPowerAfter, out absoluteONPower, out burstWidth);

         NR.Pvt.Results.FetchSignalPowerTrace("", timeout, ref signalPower, ref absoluteLimit);
      }

      void PrintResults()
      {
         Console.WriteLine("------------------Measurement------------------\n");
         Console.WriteLine("Measurement Status                       : {0}", measurementStatus);
         Console.WriteLine("Mean Absolute OFF power Before (dBm)     : {0}", absoluteOffPowerBefore);
         Console.WriteLine("Mean Absolute OFF power After (dBm)      : {0}", absoluteOffPowerAfter);
         Console.WriteLine("Mean Absolute ON power (dBm)             : {0}", absoluteONPower);
         Console.WriteLine("Burst Width (s)                          : {0}", burstWidth);
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
