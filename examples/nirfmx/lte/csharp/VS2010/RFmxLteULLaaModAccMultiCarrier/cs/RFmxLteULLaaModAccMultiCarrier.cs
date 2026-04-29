//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure operating Band to 46.
//7. Configure Duplex Mode to LAA.
//8. Configure Component Carriers settings.
//9. Configure Auto DMRS Detection Enabled.
//10. Select ModAcc measurement and enable Traces.
//11. Configure Synchronization Mode and Measurement Interval.
//12. Configure EVM Unit.
//13. Configure Averaging Parameters for ModAcc measurement.
//14. Initiate the Measurement.
//15. Fetch ModAcc Measurements and Traces.
//16. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteULLaaModAccMultiCarrier
{
   public class RFmxLteULLaaModAccMultiCarrier
   {
      RFmxInstrMX instrSession;
      RFmxLteMX lte;
      string resourceName;

      string frequencyReferenceSource;
      double frequencyReferenceFrequency;

      double centerFrequency;
      double referenceLevel;
      double externalAttenuation;

      string iqPowerEdgeTriggerSource;
      bool enableTrigger;
      double iqPowerEdgeTriggerLevel;
      double triggerDelay;
      double minimumQuietTimeDuration;
      RFmxLteMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
      RFmxLteMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
      RFmxLteMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;

      const int numberOfComponentCarriers = 2;
      double[] componentCarrierBandwidth;
      double[] componentCarrierFrequency;
      int[] componentCarrierCellId;
      int[] laaNumberOfSubframes;
      int[] laaStartingSubframe;
      RFmxLteMXLaaUplinkStartPosition[] laaUplinkStartPosition;
      RFmxLteMXLaaUplinkEndingSymbol[] laaUplinkEndingSymbol;

      RFmxLteMXModAccAveragingEnabled averagingEnabled;
      int averagingCount;

      double timeout;

      RFmxLteMXComponentCarrierSpacingType componentSpacingType;
      int componentCarrierAtCenterFrequency;

      RFmxLteMXModAccSynchronizationMode synchronizationMode;
      RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration;
      int measurementOffset;
      int measurementLength;

      RFmxLteMXModAccEvmUnit evmUnit;

      RFmxLteMXAutoDmrsDetectionEnabled autoDmrsDetectionEnabled;

      string subblockCarrierString;

      double[] meanRmsCompositeEvm;
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
         resourceName = "RFSA";

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10e6; /* (Hz) */

         centerFrequency = 1.95e9; /* (Hz) */
         referenceLevel = 0.00; /* (dBm) */
         externalAttenuation = 0.0; /* (dBm) */

         iqPowerEdgeTriggerSource = "0";
         enableTrigger = true;
         iqPowerEdgeTriggerLevel = -20.0; /* (dB) */
         triggerDelay = 0.0; /* (s) */
         minimumQuietTimeDuration = 50.0e-6; /* (s) */
         minimumQuietTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto;
         iqPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative;
         iqPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising;

         componentCarrierBandwidth = new double[] { 20e6, 20e6 };
         componentCarrierFrequency = new double[] { -9.9e6, 9.9e6 };
         componentCarrierCellId = new int[] { 0, 0 };
         laaNumberOfSubframes = new int[] { 1, 1 };
         laaStartingSubframe = new int[] { 0, 0 };
         laaUplinkStartPosition = new RFmxLteMXLaaUplinkStartPosition[]{ RFmxLteMXLaaUplinkStartPosition.StartPosition00,
                                                                       RFmxLteMXLaaUplinkStartPosition.StartPosition00 };
         laaUplinkEndingSymbol = new RFmxLteMXLaaUplinkEndingSymbol[]{ RFmxLteMXLaaUplinkEndingSymbol.EndingSymbol13,
                                                                     RFmxLteMXLaaUplinkEndingSymbol.EndingSymbol13 };

         componentSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal;
         componentCarrierAtCenterFrequency = -1;

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

         autoDmrsDetectionEnabled = RFmxLteMXAutoDmrsDetectionEnabled.True;

         timeout = 10.0; /* (s) */
      }

      void ConfigureLte()
      {
         lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

         lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         lte.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope,
            iqPowerEdgeTriggerLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration,
            iqPowerEdgeTriggerLevelType, enableTrigger);
         lte.ComponentCarrier.ConfigureSpacing("", componentSpacingType,
                                                 componentCarrierAtCenterFrequency);
         lte.ConfigureBand("", 46);
         lte.ConfigureDuplexScheme("", RFmxLteMXDuplexScheme.Laa, uplinkDownlinkConfiguration);
         lte.ConfigureNumberOfComponentCarriers("", numberOfComponentCarriers);
         for (int i = 0; i < numberOfComponentCarriers; i++)
         {
            subblockCarrierString = RFmxLteMX.BuildCarrierString("", i);
            lte.ComponentCarrier.Configure(subblockCarrierString, componentCarrierBandwidth[i],
               componentCarrierFrequency[i], componentCarrierCellId[i]);
            lte.ComponentCarrier.SetLaaStartingSubframe(subblockCarrierString, laaStartingSubframe[i]);
            lte.ComponentCarrier.SetLaaNumberOfSubframes(subblockCarrierString, laaNumberOfSubframes[i]);
            lte.ComponentCarrier.SetLaaUplinkStartPosition(subblockCarrierString, laaUplinkStartPosition[i]);
            lte.ComponentCarrier.SetLaaUplinkEndingSymbol(subblockCarrierString, laaUplinkEndingSymbol[i]);
         }
         lte.ConfigureAutoDmrsDetectionEnabled("", autoDmrsDetectionEnabled);

         lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, true);
         lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
             measurementOffset, measurementLength);
         lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit);
         lte.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
         lte.Initiate("", "");
      }

      void RetrieveResults()
      {
         lte.ModAcc.Results.FetchCompositeEvmArray("", timeout, ref meanRmsCompositeEvm,
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
            Console.WriteLine("Mean RMS Composite EVM  (% or dB)    : {0}", meanRmsCompositeEvm[i]);
            Console.WriteLine("Maximum Peak Composite EVM  (% or dB): {0}", maximumPeakCompositeEvm[i]);
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
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      static void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

   }
}