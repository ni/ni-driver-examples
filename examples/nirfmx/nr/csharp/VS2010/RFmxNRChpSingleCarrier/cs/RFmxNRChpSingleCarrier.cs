//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Parameters for IQ Power Edge Trigger.
//6. Configure Link Direction, Frequency Range, Carrier Bandwidth and Subcarrier Spacing.
//7. Select CHP measurement and enable Traces.
//8. Configure Sweep Time Parameters.
//9. Configure Averaging Parameters for CHP measurement.
//10. Initiate the Measurement.
//11. Fetch CHP Measurements and Traces.
//12. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRChpSingleCarrier
{
   public class RFmxNRChpSingleCarrier
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

      bool iqPowerEdgeEnabled;
      double iqPowerEdgeLevel;
      double triggerDelay;
      RFmxNRMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
      double minimumQuietTime;

      RFmxNRMXLinkDirection linkDirection;
      RFmxNRMXFrequencyRange frequencyRange;
      double carrierBandwidth;
      double subcarrierSpacing;

      RFmxNRMXChpSweepTimeAuto sweepTimeAuto;
      double sweepTimeInterval;

      RFmxNRMXChpAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxNRMXChpAveragingType averagingType;

      double timeout;

      double absolutePower;                                                /* (dBm) */
      double relativePower;                                                /* (dB) */

      Spectrum<float> spectrum;

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
         centerFrequency = 3.5e9;                                          /* (Hz) */
         referenceLevel = 0.0;                                             /* (dBm) */
         externalAttenuation = 0.0;                                        /* (dB) */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e6;                             /* (Hz) */

         iqPowerEdgeEnabled = false;
         iqPowerEdgeLevel = -20.0;                                         /* (dB or dBm) */
         triggerDelay = 0.0;                                               /* (s) */
         minimumQuietTimeMode = RFmxNRMXTriggerMinimumQuietTimeMode.Auto;
         minimumQuietTime = 8.0e-6;                                        /* (s) */

         linkDirection = RFmxNRMXLinkDirection.Uplink;
         frequencyRange = RFmxNRMXFrequencyRange.Range1;
         carrierBandwidth = 100e6;                                         /* (Hz) */
         subcarrierSpacing = 30e3;                                         /* (Hz) */

         sweepTimeAuto = RFmxNRMXChpSweepTimeAuto.True;
         sweepTimeInterval = 1.0e-3;                                       /* (s) */

         averagingEnabled = RFmxNRMXChpAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxNRMXChpAveragingType.Rms;

         timeout = 10.0;                                                   /* (s) */
      }

      void InitializeInstr()
      {
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      void ConfigureNR()
      {
         NR = instrSession.GetNRSignalConfiguration();      /* Create a new RFmx Session */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         NR.SetSelectedPorts("", selectedPorts);
         NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         NR.ConfigureIQPowerEdgeTrigger("", "0", RFmxNRMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
            triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxNRMXIQPowerEdgeTriggerLevelType.Relative,
            iqPowerEdgeEnabled);
         NR.SetLinkDirection("", linkDirection);
         NR.SetFrequencyRange("", frequencyRange);
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth);
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing);
         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Chp, true);
         NR.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         NR.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         NR.Initiate("", "");
      }

      void RetrieveResults()
      {
         NR.Chp.Results.ComponentCarrier.FetchMeasurement("", timeout, out absolutePower, out relativePower);
         NR.Chp.Results.FetchSpectrum("", timeout, ref spectrum);
      }

      void PrintResults()
      {
         Console.WriteLine("Absolute Power (dBm)     : {0}", absolutePower);
         Console.WriteLine("Relative Power (dB)      : {0}\n", relativePower);
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
