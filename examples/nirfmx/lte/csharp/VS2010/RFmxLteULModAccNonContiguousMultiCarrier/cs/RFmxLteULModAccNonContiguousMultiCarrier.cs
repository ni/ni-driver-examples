//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Duplex Mode.
//6[A-F]. Configure Subblock Parameters.
//6A. Configure Number of Subblocks.
//6B. Configure subblock Frequency.
//6C. Configure Component Carrier Spacing.
//6D. Configure Band.
//6E. Configure Number of Component Carriers.
//6F. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
//7. Configure Auto DMRS Detection Enabled.
//8. Select ModAcc measurement and enable Traces.
//9. Configure Synchronization Mode and Measurement Interval.
//10. Configure EVM Unit.
//11. Configure In-Band Emission Mask Type.
//12. Configure Averaging Parameters for ModAcc measurement.
//13. Initiate the Measurement.
//14. Fetch ModAcc Measurements and Traces
//15. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteULModAccNonContiguousMultiCarrier
{
   /* Input: Subblock inputs structure */
   struct SubblockInput
   {
	  public double subblockFrequency;                      /*(Hz) */
      public RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;
      public int componentCarrierAtCenterFrequency;
      public int band;
      public double[] componentCarrierBandwidth;            /*(Hz) */
      public double[] componentCarrierFrequency;            /*(Hz) */
      public int[] componentCarrierCellId;                  /*(Hz) */
   }

   /* Input: Subblock measurement outputs structure */
   struct SubblockMeasurement
   {
      public double[] meanRmsCompositeEvm;                  /*(dBm) */
      public double[] maximumPeakCompositeEvm;              /*(dBm) */
      public double[] meanFrequencyError;                   /*(dB) */
      public int[] peakCompositeEvmSlotIndex;               /*(dB) */
      public int[] peakCompositeEvmSymbolIndex;
      public int[] peakCompositeEvmSubcarrierIndex;
      public double[] meanIQOriginOffset;
      public double[] meanIQGainImbalance;
      public double[] meanIQQuadratureError;
      public double[] inBandEmissionMargin;
      public ComplexSingle[][] dataConstellationTraces;
      public ComplexSingle[][] dmrsDataConstellationTraces;
      public AnalogWaveform<float>[] meanRmsEvmPerSubcarrier;
   }

   public class RFmxLteULModAccNonContiguousMultiCarrier
   {
      RFmxInstrMX instrSession;
      RFmxLteMX lte;
      RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration;

      string rfsaResourceName;

      const int NumberOfComponentCarriers = 1;
      const int NumberOfSubblocks = 2;

      string frequencyReferenceSource;
      double frequencyReferenceFrequency;

      double centerFrequency;
      double referenceLevel;
      double externalAttenuation;

      bool enableTrigger;
      string digitalEdgeSource;
      RFmxLteMXDigitalEdgeTriggerEdge digitalEdge;
      double triggerDelay;

      RFmxLteMXModAccAveragingEnabled averagingEnabled;
      int averagingCount;

      RFmxLteMXAutoDmrsDetectionEnabled autoDmrsDetectionEnabled;

      double timeout;

      string[] subblockString;

      SubblockInput[] subblocks;
      SubblockMeasurement[] subblocksMsr;

      RFmxLteMXDuplexScheme duplexScheme;
      RFmxLteMXModAccSynchronizationMode synchronizationMode;
      int measurementOffset;
      int measurementLength;
      RFmxLteMXModAccEvmUnit evmUnit;
      RFmxLteMXModAccInBandEmissionMaskType inBandEmissionMaskType;
      string subblockCarrierString;

      void CloseSession()
      {
         if (lte != null)
         {
            lte.Dispose();
            lte = null;
         }
         if (instrSession != null)
         {
            instrSession.Close();
            instrSession = null;
         }
      }

      public void Run()
      {
         try
         {
            InitializeVariables();
            InitializeInstr();
            ConfigureLte();
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

      void InitializeInstr()
      {
         instrSession = new RFmxInstrMX(rfsaResourceName, "");
      }

      void InitializeVariables()
      {
         rfsaResourceName = "RFSA";

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10e6;   /* (Hz) */

         centerFrequency = 1.95e9;             /* (Hz) */
         referenceLevel = 0.00;                /* (dBm) */
         externalAttenuation = 0.0;            /* (dBm) */

         enableTrigger = false;
         digitalEdgeSource = RFmxLteMXConstants.Pfi0;
         digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                   /* (s) */

         averagingEnabled = RFmxLteMXModAccAveragingEnabled.False;
         averagingCount = 10;

         timeout = 10.0;                       /* (s) */

         uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;
         duplexScheme = RFmxLteMXDuplexScheme.Fdd;

         synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot;
         measurementOffset = 0;
         measurementLength = 1;

         evmUnit = RFmxLteMXModAccEvmUnit.Percentage;

         inBandEmissionMaskType = RFmxLteMXModAccInBandEmissionMaskType.Release11Onwards;

         autoDmrsDetectionEnabled = RFmxLteMXAutoDmrsDetectionEnabled.True;

         subblocks = new SubblockInput[NumberOfSubblocks] {
            new SubblockInput {
                           subblockFrequency = 0.0,
                           componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                           componentCarrierAtCenterFrequency = -1,
                           band = 1,
                           componentCarrierBandwidth = new double[NumberOfComponentCarriers] {20e6},
                           componentCarrierFrequency = new double[NumberOfComponentCarriers] {0.0},
                           componentCarrierCellId = new int[NumberOfComponentCarriers]{0}
                        },
            new SubblockInput {
                           subblockFrequency = 30e6, 
                           componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                           componentCarrierAtCenterFrequency = -1,
                           band = 1,
                           componentCarrierBandwidth = new double[NumberOfComponentCarriers] {20e6},
                           componentCarrierFrequency =  new double[NumberOfComponentCarriers] {0.0},
                           componentCarrierCellId = new int[NumberOfComponentCarriers]{0}
                        }
                                                            };

         subblocksMsr = new SubblockMeasurement[NumberOfSubblocks];
      }

      void ConfigureLte()
      {
         lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

         lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource,
                                                      digitalEdge, triggerDelay, enableTrigger);
         lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration);
         lte.ConfigureNumberOfSubblocks("", NumberOfSubblocks);

         subblockString = new string[NumberOfSubblocks];
         for (int i = 0; i < NumberOfSubblocks; i++)
         {
            subblockString[i] = RFmxLteMX.BuildSubblockString("", i);
            lte.SetSubblockFrequency(subblockString[i], subblocks[i].subblockFrequency);
            lte.ComponentCarrier.ConfigureSpacing(subblockString[i], subblocks[i].componentCarrierSpacingType,
                                                    subblocks[i].componentCarrierAtCenterFrequency);
            lte.ConfigureBand(subblockString[i], subblocks[i].band);
            lte.ConfigureNumberOfComponentCarriers(subblockString[i], NumberOfComponentCarriers);
            lte.ComponentCarrier.ConfigureArray(subblockString[i], subblocks[i].componentCarrierBandwidth,
                subblocks[i].componentCarrierFrequency, subblocks[i].componentCarrierCellId);
         }
         lte.ConfigureAutoDmrsDetectionEnabled("", autoDmrsDetectionEnabled);

         lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, true);
         lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
                 measurementOffset, measurementLength);
         lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit);
         lte.ModAcc.Configuration.ConfigureInBandEmissionMaskType("", inBandEmissionMaskType);
         lte.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
         lte.Initiate("", "");
      }

      void RetrieveResults()
      {
         subblocksMsr = new SubblockMeasurement[NumberOfSubblocks];
         for (int i = 0; i < NumberOfSubblocks; i++)
         {
            subblocksMsr[i].dataConstellationTraces = new ComplexSingle[NumberOfComponentCarriers][];
            subblocksMsr[i].dmrsDataConstellationTraces = new ComplexSingle[NumberOfComponentCarriers][];
            subblocksMsr[i].meanRmsEvmPerSubcarrier = new AnalogWaveform<float>[NumberOfComponentCarriers];
            lte.ModAcc.Results.FetchCompositeEvmArray(subblockString[i], timeout,
               ref subblocksMsr[i].meanRmsCompositeEvm, ref subblocksMsr[i].maximumPeakCompositeEvm,
               ref subblocksMsr[i].meanFrequencyError, ref subblocksMsr[i].peakCompositeEvmSymbolIndex,
               ref subblocksMsr[i].peakCompositeEvmSubcarrierIndex, ref subblocksMsr[i].peakCompositeEvmSlotIndex);
            lte.ModAcc.Results.FetchIQImpairmentsArray(subblockString[i], timeout,
               ref subblocksMsr[i].meanIQOriginOffset, ref subblocksMsr[i].meanIQGainImbalance,
               ref subblocksMsr[i].meanIQQuadratureError);
            lte.ModAcc.Results.FetchInBandEmissionMarginArray(subblockString[i], timeout,
               ref subblocksMsr[i].inBandEmissionMargin);

            for (int j = 0; j < NumberOfComponentCarriers; j++)
            {
               subblockCarrierString = RFmxLteMX.BuildCarrierString(subblockString[i], j);
               lte.ModAcc.Results.FetchEvmPerSubcarrierTrace(subblockCarrierString, timeout,
                  ref subblocksMsr[i].meanRmsEvmPerSubcarrier[j]);
               lte.ModAcc.Results.FetchPuschConstellationTrace(subblockCarrierString, timeout,
                  ref subblocksMsr[i].dataConstellationTraces[j], ref subblocksMsr[i].dmrsDataConstellationTraces[j]);
            }
         }
      }

      void PrintResults()
      {
         Console.WriteLine("----------------------Measurements--------------------");
         for (int i = 0; i < NumberOfSubblocks; i++)
         {
            Console.WriteLine("\nSubblock Number {0}", i);

            Console.WriteLine("-----------Component Carrier Measurements----------------", i);
            for (int j = 0; j < NumberOfComponentCarriers; j++)
            {
               Console.WriteLine("Carrier {0}", j);
               Console.WriteLine("Mean Rms Composite Evm  (%  or dB)      : {0}", subblocksMsr[i].meanRmsCompositeEvm[j]);
               Console.WriteLine("Max Peak Composite Evm  (%  or dB)      : {0}", subblocksMsr[i].maximumPeakCompositeEvm[j]);
               Console.WriteLine("Peak Composite Evm Slot Index           : {0}", subblocksMsr[i].peakCompositeEvmSlotIndex[j]);
               Console.WriteLine("Peak Composite Evm Symbol Index         : {0}", subblocksMsr[i].peakCompositeEvmSymbolIndex[j]);
               Console.WriteLine("Peak Composite Evm Subcarrier Index     : {0}", subblocksMsr[i].peakCompositeEvmSubcarrierIndex[j]);
               Console.WriteLine("Mean Frequency Error  (Hz)              : {0}", subblocksMsr[i].meanFrequencyError[j]);
               Console.WriteLine("Mean IQ Origin Offset  (dBc)            : {0}", subblocksMsr[i].meanIQOriginOffset[j]);
               Console.WriteLine("Mean IQ Gain Imbalance  (dB)            : {0}", subblocksMsr[i].meanIQGainImbalance[j]);
               Console.WriteLine("Mean IQ Quadrature Error  (deg)         : {0}", subblocksMsr[i].meanIQQuadratureError[j]);
               Console.WriteLine("In Band Emission Margin  (dB)           : {0}", subblocksMsr[i].inBandEmissionMargin[j]);
               Console.WriteLine("-------------------------------------------------\n");
            }
         }
      }

      static void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

   }
}
