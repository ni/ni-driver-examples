//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure operating Band.
//7. Configure Duplex Mode.
//8. Configure Component Carriers.
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

namespace NationalInstruments.Examples.RFmxLteULModAccContiguousMultiCarrier
{
   public class RFmxLteULModAccContiguousMultiCarrier
   {
      RFmxInstrMX instrSession;
      RFmxLteMX lte;
      string rfsaResourceName;

      string frequencyReferenceSource;
      double frequencyReferenceFrequency;

      double centerFrequency;
      double referenceLevel;
      double externalAttenuation;

      bool enableTrigger;
      string digitalEdgeSource;
      RFmxLteMXDigitalEdgeTriggerEdge digitalEdge;
      double triggerDelay;

      const int numberOfComponentCarriers = 2;
      double[] componentCarrierBandwidth = { 20e6, 20e6 };
      double[] componentCarrierFrequency = { -9.9e6, 9.9e6 };
      int[] componentCarrierCellId = { 0, 0 };

      RFmxLteMXModAccAveragingEnabled averagingEnabled;
      int averagingCount;

      double timeout;

      RFmxLteMXComponentCarrierSpacingType componentSpacingType;
      int componentCarrierAtCenterFrequency;

      int band;

      RFmxLteMXDuplexScheme duplexScheme;

      RFmxLteMXModAccSynchronizationMode synchronizationMode;
      RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration;
      int measurementOffset;
      int measurementLength;

      RFmxLteMXModAccEvmUnit evmUnit;

      RFmxLteMXModAccInBandEmissionMaskType inBandEmissionMaskType;

      RFmxLteMXAutoDmrsDetectionEnabled autoDmrsDetectionEnabled;

      string subblockCarrierString;

      double[] meanRMSCompositeEvm;
      double[] maximumPeakCompositeEvm;
      double[] meanFrequencyError;
      int[] peakCompositeEvmSlotIndex;
      int[] peakCompositeEvmSymbolIndex;
      int[] peakCompositeEvmSubcarrierIndex;

      double[] meanIQOriginOffset;
      double[] meanIQGainImbalance;
      double[] meanIQQuadratureError;
      double[] inBandEmissionMargin;

      ComplexSingle[][] dataConstellation, dmrsDataConstellation;

      AnalogWaveform<float>[] meanRmsEvmPerSubcarrier;

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


      void InitializeVariables()
      {
         rfsaResourceName = "RFSA";

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10e6;                               /* (Hz) */

         centerFrequency = 1.95e9;                                         /* (Hz) */
         referenceLevel = 0.00;                                            /* (dBm) */
         externalAttenuation = 0.0;                                        /* (dBm) */

         enableTrigger = false;
         digitalEdgeSource = RFmxLteMXConstants.Pfi0;
         digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                               /* (s) */

         componentSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal;
         componentCarrierAtCenterFrequency = -1;

         band = 1;
         duplexScheme = RFmxLteMXDuplexScheme.Fdd;
         uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;

         dataConstellation = new ComplexSingle[numberOfComponentCarriers][];
         dmrsDataConstellation = new ComplexSingle[numberOfComponentCarriers][];

         meanRmsEvmPerSubcarrier = new AnalogWaveform<float>[numberOfComponentCarriers];

         averagingEnabled = RFmxLteMXModAccAveragingEnabled.False;
         averagingCount = 10;

         synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot;
         measurementOffset = 0;
         measurementLength = 1;

         evmUnit = RFmxLteMXModAccEvmUnit.Percentage;

         inBandEmissionMaskType = RFmxLteMXModAccInBandEmissionMaskType.Release11Onwards;

         autoDmrsDetectionEnabled = RFmxLteMXAutoDmrsDetectionEnabled.True;

         timeout = 10.0;                                                   /* (s) */
      }

      void ConfigureLte()
      {
         lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

         lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource,
                                                      digitalEdge, triggerDelay, enableTrigger);
         lte.ComponentCarrier.ConfigureSpacing("", componentSpacingType,
                                                 componentCarrierAtCenterFrequency);
         lte.ConfigureBand("", band);
         lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration);
         lte.ConfigureNumberOfComponentCarriers("", numberOfComponentCarriers);
         lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth,
             componentCarrierFrequency, componentCarrierCellId);
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
         lte.ModAcc.Results.FetchCompositeEvmArray("", timeout, ref meanRMSCompositeEvm,
             ref maximumPeakCompositeEvm, ref meanFrequencyError, ref peakCompositeEvmSymbolIndex,
             ref peakCompositeEvmSubcarrierIndex, ref peakCompositeEvmSlotIndex);
         lte.ModAcc.Results.FetchIQImpairmentsArray("", timeout, ref meanIQOriginOffset,
             ref meanIQGainImbalance, ref meanIQQuadratureError);
         lte.ModAcc.Results.FetchInBandEmissionMarginArray("", timeout, ref inBandEmissionMargin);
         for (int i = 0; i < numberOfComponentCarriers; i++)
         {
            subblockCarrierString = RFmxLteMX.BuildCarrierString("", i);
            lte.ModAcc.Results.FetchPuschConstellationTrace(subblockCarrierString, timeout,
                ref dataConstellation[i], ref dmrsDataConstellation[i]);
            lte.ModAcc.Results.FetchEvmPerSubcarrierTrace(subblockCarrierString, timeout,
                ref meanRmsEvmPerSubcarrier[i]);
         }
      }

      void PrintResults()
      {
         Console.WriteLine("------------------------Measurements------------------------\n");
         for (int i = 0; i < numberOfComponentCarriers; i++)
         {
            Console.WriteLine("Carrier  : {0}\n", i);
            Console.WriteLine("Mean RMS Composite EVM  (% or dB)    : {0}", meanRMSCompositeEvm[i]);
            Console.WriteLine("Max Peak Composite EVM  (% or dB)    : {0}", maximumPeakCompositeEvm[i]);
            Console.WriteLine("Peak Composite EVM Slot Index        : {0}", peakCompositeEvmSlotIndex[i]);
            Console.WriteLine("Peak Composite EVM Symbol Index      : {0}", peakCompositeEvmSymbolIndex[i]);
            Console.WriteLine("Peak Composite EVM Subcarrier Index  : {0}", peakCompositeEvmSubcarrierIndex[i]);
            Console.WriteLine("Mean Frequency Error  (Hz)           : {0}", meanFrequencyError[i]);
            Console.WriteLine("Mean IQ Origin Offset  (dBc)         : {0}", meanIQOriginOffset[i]);
            Console.WriteLine("Mean IQ Gain Imbalance  (dB)         : {0}", meanIQGainImbalance[i]);
            Console.WriteLine("Mean IQ Quadrature Error  (deg)      : {0}", meanIQQuadratureError[i]);
            Console.WriteLine("In Band Emission Margin  (dB)        : {0}", inBandEmissionMargin[i]);
            Console.WriteLine("-------------------------------------------------\n");
         }
      }

      void InitializeInstr()
      {
         instrSession = new RFmxInstrMX(rfsaResourceName, "");
      }

      static void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

   }
}
