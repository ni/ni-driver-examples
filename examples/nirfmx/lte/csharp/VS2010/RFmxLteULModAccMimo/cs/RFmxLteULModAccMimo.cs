//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Number of DUT Antennas.
//6. Configure Transmit Antenna to Analyze.
//7. Configure Duplex Mode.
//8[A-G]. Configure Subblock Parameters.
//8A. Configure Number of Subblocks.
//8B. Configure subblock Frequency.
//8C. Configure Component Carrier Spacing.
//8D. Configure Band.
//8E. Configure Number of Component Carriers.
//8F. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
//8G. Configure DMRS OCC Enabled and Cyclic Shift Field.
//9. Configure Auto DMRS Detection Enabled.
//10. Select ModAcc measurement and enable Traces.
//11. Configure Synchronization Mode and Measurement Interval.
//12. Configure EVM Unit.
//13. Configure In-Band Emission Mask Type.
//14. Configure Averaging Parameters for ModAcc measurement.
//15. Initiate the Measurement.
//16. Fetch ModAcc Measurements and Traces.
//17. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteULModAccMimo
{
   /* Input: Subblock inputs structure */
   struct SubblockInput
   {
	  public double subblockFrequency;                        /*(Hz) */
      public RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;
      public int componentCarrierAtCenterFrequency;
      public int band;
      /*Component Carrier Settings*/
      public double[] componentCarrierBandwidth;              /*(Hz) */
      public double[] componentCarrierFrequency;              /*(Hz) */
      public int[] componentCarrierCellId;
   }

   /* Input: Subblock measurement outputs structure */
   struct SubblockMeasurement
   {
      public double[] meanRmsCompositeEvm;                    /*(% or dB) */
      public double[] maximumPeakCompositeEvm;                /*(% or dB) */
      public int[] peakCompositeEvmSlotIndex;
      public int[] peakCompositeEvmSymbolIndex;
      public int[] peakCompositeEvmSubcarrierIndex;
      public double[] meanFrequencyError;                     /*(Hz) */
      public double[] meanIQOriginOffset;                     /*(dBc) */
      public double[] meanIQGainImbalance;                    /*(dB) */
      public double[] meanIQQuadratureError;                  /*(deg) */
      public double[] inBandEmissionMargin;                   /*(dB) */
      public ComplexSingle[][] dataConstellationTraces;
      public ComplexSingle[][] dmrsDataConstellationTraces;
      public AnalogWaveform<float>[] meanRmsEvmPerSubcarrier; /*(% or dB) */
   }

   public class RFmxLteULModAccMimo
   {
      RFmxInstrMX instrSession;
      RFmxLteMX lte;

      string rfsaResourceName;

      const int numberOfComponentCarriers = 1;
      const int numberOfSubblocks = 2;

      string frequencyReferenceSource;
      double frequencyReferenceFrequency;

      double centerFrequency;
      double referenceLevel;
      double externalAttenuation;

      bool enableTrigger;
      string digitalEdgeSource;
      RFmxLteMXDigitalEdgeTriggerEdge digitalEdge;
      double triggerDelay;

      RFmxLteMXDuplexScheme duplexScheme;
      RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration;
      int numberOfDutAntennas;
      int transmitAntennaToAnalyze;

      /* EVM Unit */
      RFmxLteMXModAccEvmUnit evmUnit;

      /* InBandEmissionMaskType */
      RFmxLteMXModAccInBandEmissionMaskType inBandEmissionMaskType;

      /* Measurement Interval */
      RFmxLteMXModAccSynchronizationMode synchronizationMode;
      int measurementOffset;
      int measurementLength;

      /* Averaging */
      RFmxLteMXModAccAveragingEnabled averagingEnabled;
      int averagingCount;

      RFmxLteMXAutoDmrsDetectionEnabled autoDmrsDetectionEnabled;

      double timeout;
      string[] subblockString;
      SubblockInput[] subblocks;
      SubblockMeasurement[] subblocksMsr;

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

         uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;
         duplexScheme = RFmxLteMXDuplexScheme.Fdd;
         numberOfDutAntennas = 2;
         transmitAntennaToAnalyze = 0;

         evmUnit = RFmxLteMXModAccEvmUnit.Percentage;

         inBandEmissionMaskType = RFmxLteMXModAccInBandEmissionMaskType.Release11Onwards;

         synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot;
         measurementOffset = 0;
         measurementLength = 1;

         averagingEnabled = RFmxLteMXModAccAveragingEnabled.False;
         averagingCount = 10;

         autoDmrsDetectionEnabled = RFmxLteMXAutoDmrsDetectionEnabled.True;

         timeout = 10.0;                       /* (s) */

         subblocks = new SubblockInput[numberOfSubblocks] {
            new SubblockInput {
                           subblockFrequency = 0.0,
                           componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                           componentCarrierAtCenterFrequency = -1,
                           band = 1,
                           componentCarrierBandwidth = new double[numberOfComponentCarriers] {20e6},
                           componentCarrierFrequency = new double[numberOfComponentCarriers] {0.0},
                           componentCarrierCellId = new int[numberOfComponentCarriers]{0}
                        },
            new SubblockInput {
                           subblockFrequency = 30e6, 
                           componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal,
                           componentCarrierAtCenterFrequency = -1,
                           band = 1,
                           componentCarrierBandwidth = new double[numberOfComponentCarriers] {20e6},
                           componentCarrierFrequency = new double[numberOfComponentCarriers] {0.0},
                           componentCarrierCellId = new int[numberOfComponentCarriers]{0}
                        }
                                                            };

         subblocksMsr = new SubblockMeasurement[numberOfSubblocks];
      }

      void ConfigureLte()
      {
         lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

         lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource,
                                                      digitalEdge, triggerDelay, enableTrigger);
         lte.ConfigureNumberOfDutAntennas("", numberOfDutAntennas);
         lte.ConfigureTransmitAntennaToAnalyze("", transmitAntennaToAnalyze);
         lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration);
         lte.ConfigureNumberOfSubblocks("", numberOfSubblocks);

         subblockString = new string[numberOfSubblocks];
         for (int i = 0; i < numberOfSubblocks; i++)
         {
            subblockString[i] = RFmxLteMX.BuildSubblockString("", i);
            lte.SetSubblockFrequency(subblockString[i], subblocks[i].subblockFrequency);
            lte.ComponentCarrier.ConfigureSpacing(subblockString[i], subblocks[i].componentCarrierSpacingType,
                                                    subblocks[i].componentCarrierAtCenterFrequency);
            lte.ConfigureBand(subblockString[i], subblocks[i].band);
            lte.ConfigureNumberOfComponentCarriers(subblockString[i], numberOfComponentCarriers);
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
         for (int i = 0; i < numberOfSubblocks; i++)
         {
            subblocksMsr[i].dataConstellationTraces = new ComplexSingle[numberOfComponentCarriers][];
            subblocksMsr[i].dmrsDataConstellationTraces = new ComplexSingle[numberOfComponentCarriers][];
            subblocksMsr[i].meanRmsEvmPerSubcarrier = new AnalogWaveform<float>[numberOfComponentCarriers];
            lte.ModAcc.Results.FetchCompositeEvmArray(subblockString[i], timeout,
               ref subblocksMsr[i].meanRmsCompositeEvm, ref subblocksMsr[i].maximumPeakCompositeEvm,
               ref subblocksMsr[i].meanFrequencyError,
           ref subblocksMsr[i].peakCompositeEvmSymbolIndex, ref subblocksMsr[i].peakCompositeEvmSubcarrierIndex,
                ref subblocksMsr[i].peakCompositeEvmSlotIndex);
            lte.ModAcc.Results.FetchIQImpairmentsArray(subblockString[i], timeout,
               ref subblocksMsr[i].meanIQOriginOffset, ref subblocksMsr[i].meanIQGainImbalance,
               ref subblocksMsr[i].meanIQQuadratureError);
            lte.ModAcc.Results.FetchInBandEmissionMarginArray(subblockString[i], timeout,
               ref subblocksMsr[i].inBandEmissionMargin);

            for (int j = 0; j < numberOfComponentCarriers; j++)
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
         for (int i = 0; i < numberOfSubblocks; i++)
         {
            Console.WriteLine("\nSubblock Number {0}", i);

            Console.WriteLine("-----------Component Carrier Measurements----------------", i);
            for (int j = 0; j < numberOfComponentCarriers; j++)
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
