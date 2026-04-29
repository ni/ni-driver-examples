//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Configure Link Direction to Sidelink.
//7. Configure operating Band to 47.
//8. Configure Auto Resource Block Detection Enabled to True.
//9. Configure Auto DMRS Detection Enabled to True.
//10. Select ModAcc measurement and enable Traces.
//11. Configure Synchronization Mode and Measurement Interval.
//12. Configure EVM Unit.
//13. Initiate the Measurement.
//14. Fetch ModAcc Measurements and Traces.
//15. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteSLModAccSingleCarrier
{
   public class RFmxLteSLModAccSingleCarrier
   {
      RFmxInstrMX instrSession;
      RFmxLteMX lte;

      string resourceName;
      double centerFrequency;
      double referenceLevel;
      double externalAttenuation;

      string frequencyReferenceSource;
      double frequencyReferenceFrequency;

      bool iqPowerEdgeEnabled;
      double iqPowerEdgeLevel;
      double triggerDelay;
      RFmxLteMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
      double minimumQuietTime;

      double carrierBandwidth;

      int measurementOffset;
      int measurementLength;

      RFmxLteMXModAccEvmUnit evmUnit;

      double timeout;

      double meanRmsCompositeEvm;                                       /* (% or dB) */
      double maxPeakCompositeEvm;                                       /* (% or dB) */
      double meanFrequencyError;                                        /* (Hz) */
      int peakCompositeEvmSlotIndex;
      int peakCompositeEvmSymbolIndex;
      int peakCompositeEvmSubcarrierIndex;
      double meanIQOriginOffset;                                        /* (dBc) */
      double meanIQGainImbalance;                                       /* (dB) */
      double meanIQQuadratureError;                                     /* (deg) */
      double inBandEmissionMargin;                                      /* (dB) */
      ComplexSingle[] dataConstellation, dmrsConstellation;
      AnalogWaveform<float> evmPerSubcarrier;                           /* (% or dB) */

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
         centerFrequency = 5.89e9;                                      /* (Hz) */
         referenceLevel = 0.0;                                          /* (dBm) */
         externalAttenuation = 0.0;                                     /* (dB) */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e6;                          /* (Hz) */

         iqPowerEdgeEnabled = true;
         iqPowerEdgeLevel = -20.0;                                      /* (dB) */
         triggerDelay = 0.0;                                            /* (s) */
         minimumQuietTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto;
         minimumQuietTime = 50.0e-6;                                    /* (s) */

         carrierBandwidth = 10e6;                                       /* (Hz) */

         measurementOffset = 0;                                         /* (slots) */
         measurementLength = 1;                                         /* (slots) */

         evmUnit = RFmxLteMXModAccEvmUnit.Percentage;

         timeout = 10.0;                                                /* (s) */
      }

      void InitializeInstr()
      {
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      void ConfigureLte()
      {
         lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         lte.ConfigureIQPowerEdgeTrigger("", "0", RFmxLteMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel, triggerDelay,
            minimumQuietTimeMode, minimumQuietTime, RFmxLteMXIQPowerEdgeTriggerLevelType.Relative, iqPowerEdgeEnabled);
         lte.ComponentCarrier.Configure("", carrierBandwidth, 0.0, 0);
         lte.ConfigureLinkDirection("", RFmxLteMXLinkDirection.Sidelink);
         lte.ConfigureBand("", 47);
         lte.ComponentCarrier.ConfigureAutoResourceBlockDetectionEnabled("", RFmxLteMXAutoResourceBlockDetectionEnabled.True);
         lte.ConfigureAutoDmrsDetectionEnabled("", RFmxLteMXAutoDmrsDetectionEnabled.True);
         lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, true);
         lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", RFmxLteMXModAccSynchronizationMode.Slot,
            measurementOffset, measurementLength);
         lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit);
         lte.Initiate("", "");
      }

      void RetrieveResults()
      {
         lte.ModAcc.Results.FetchCompositeEvm("", timeout, out meanRmsCompositeEvm, out maxPeakCompositeEvm,
            out meanFrequencyError, out peakCompositeEvmSymbolIndex, out peakCompositeEvmSubcarrierIndex,
            out peakCompositeEvmSlotIndex);
         lte.ModAcc.Results.FetchIQImpairments("", timeout, out meanIQOriginOffset, out meanIQGainImbalance,
            out meanIQQuadratureError);
         lte.ModAcc.Results.FetchInBandEmissionMargin("", timeout, out inBandEmissionMargin);
         lte.ModAcc.Results.FetchPsschConstellationTrace("", timeout, ref dataConstellation, ref dmrsConstellation);
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

      static void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

   }
}
