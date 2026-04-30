//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Configure operating Band.
//7. Configure Duplex Mode.
//8. Configure Auto DMRS Detection Enabled.
//9. Select ModAcc measurement and enable Traces.
//10. Configure Synchronization Mode and Measurement Interval.
//11. Configure EVM Unit.
//12. Configure In-Band Emission Mask Type.
//13. Configure Averaging Parameters for ModAcc measurement.
//14. Initiate the Measurement.
//15. Fetch ModAcc Measurements and Traces.
//16. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteULModAccSingleCarrier
{
   public class RFmxLteULModAccSingleCarrier
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

      double componentCarrierBandwidth;
      double componentCarrierFrequency;
      int cellID;

      RFmxLteMXModAccAveragingEnabled averagingEnabled;
      int averagingCount;

      RFmxLteMXModAccSynchronizationMode synchronizationMode;
      int measurementOffset;
      int measurementLength;
      RFmxLteMXModAccEvmUnit evmUnit;
      RFmxLteMXModAccInBandEmissionMaskType inBandEmissionMaskType;
      int band;
      RFmxLteMXDuplexScheme duplexScheme;
      RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration;

      RFmxLteMXAutoDmrsDetectionEnabled autoDmrsDetectionEnabled;

      double timeout;

      double meanRmsCompositeEvm;
      double maxPeakCompositeEvm;
      double meanFrequencyError;
      int peakCompositeEvmSlotIndex;
      int peakCompositeEvmSymbolIndex;
      int peakCompositeEvmSubcarrierIndex;
      double meanIQOriginOffset;
      double meanIQGainImbalance;
      double meanIQQuadratureError;
      double inBandEmissionMargin;
      ComplexSingle[] dataConstellation, dmrsDataConstellation;
      AnalogWaveform<float> evmPerSubcarrier;

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
         frequencyReferenceFrequency = 10e6;                            /* (Hz) */

         centerFrequency = 1.95e9;                                      /* (Hz) */
         referenceLevel = 0.00;                                         /* (dBm) */
         externalAttenuation = 0.0;                                     /* (dBm) */

         enableTrigger = false;
         digitalEdgeSource = RFmxLteMXConstants.Pfi0;
         digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                            /* (s) */

         componentCarrierBandwidth = 10e6;                              /* (Hz) */
         componentCarrierFrequency = 0.0;                               /* (Hz) */
         cellID = 0;

         averagingEnabled = RFmxLteMXModAccAveragingEnabled.False;
         averagingCount = 10;

         synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot;
         measurementOffset = 0;                                         /*(slots) */
         measurementLength = 1;                                         /*(slots) */

         uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;
         duplexScheme = RFmxLteMXDuplexScheme.Fdd;

         band = 1;

         evmUnit = RFmxLteMXModAccEvmUnit.Percentage;

         inBandEmissionMaskType = RFmxLteMXModAccInBandEmissionMaskType.Release11Onwards;

         autoDmrsDetectionEnabled = RFmxLteMXAutoDmrsDetectionEnabled.True;

         timeout = 10.0; /* (s) */
      }

      void ConfigureLte()
      {
         lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
         lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID);
         lte.ConfigureBand("", band);
         lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration);
         lte.ConfigureAutoDmrsDetectionEnabled("", autoDmrsDetectionEnabled);
         lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, true);
         lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset,
            measurementLength);
         lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit);
         lte.ModAcc.Configuration.ConfigureInBandEmissionMaskType("", inBandEmissionMaskType);
         lte.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
         lte.Initiate("", "");
      }

      void RetrieveResults()
      {
         lte.ModAcc.Results.FetchCompositeEvm("", timeout, out meanRmsCompositeEvm, out maxPeakCompositeEvm,
            out meanFrequencyError,
             out peakCompositeEvmSymbolIndex, out peakCompositeEvmSubcarrierIndex, out peakCompositeEvmSlotIndex);
         lte.ModAcc.Results.FetchIQImpairments("", timeout, out meanIQOriginOffset, out meanIQGainImbalance,
            out meanIQQuadratureError);
         lte.ModAcc.Results.FetchInBandEmissionMargin("", timeout, out inBandEmissionMargin);
         lte.ModAcc.Results.FetchPuschConstellationTrace("", timeout, ref dataConstellation, ref dmrsDataConstellation);
         lte.ModAcc.Results.FetchEvmPerSubcarrierTrace("", timeout, ref evmPerSubcarrier);
      }

      void PrintResults()
      {
         Console.WriteLine("------------------Measurement------------------");
         Console.WriteLine("Mean RMS Composite EVM  (% or dB)       : {0}", meanRmsCompositeEvm);
         Console.WriteLine("Max Peak Composite EVM  (% or dB)       : {0}", maxPeakCompositeEvm);
         Console.WriteLine("Peak Composite EVM Slot Index           : {0}", peakCompositeEvmSlotIndex);
         Console.WriteLine("Peak Composite EVM Symbol Index         : {0}", peakCompositeEvmSymbolIndex);
         Console.WriteLine("Peak Composite EVM Subcarrier Index     : {0}", peakCompositeEvmSubcarrierIndex);
         Console.WriteLine("Mean Frequency Error  (Hz)              : {0}", meanFrequencyError);
         Console.WriteLine("Mean IQ Origin Offset  (dBc)            : {0}", meanIQOriginOffset);
         Console.WriteLine("Mean IQ Gain Imbalance  (dB)            : {0}", meanIQGainImbalance);
         Console.WriteLine("Mean IQ Quadrature Error  (deg)         : {0}", meanIQQuadratureError);
         Console.WriteLine("In Band Emission Margin  (dB)           : {0}", inBandEmissionMargin);
      }

      static void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

   }
}
