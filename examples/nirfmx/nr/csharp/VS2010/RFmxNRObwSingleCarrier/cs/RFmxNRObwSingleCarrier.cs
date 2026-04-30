//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Parameters for IQ Power Edge Trigger.
//6. Configure Link Direction, Frequency Range, Carrier Bandwidth and Subcarrier Spacing.
//7. Select OBW measurement and enable Traces.
//8. Configure Sweep Time Parameters.
//9. Configure Span Parameters for OBW measurement.
//10. Configure Averaging Parameters for OBW measurement.
//12. Initiate the Measurement.
//13. Fetch OBW Measurements and Traces.
//14. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRObwSingleCarrier
{
   public class RFmxNRObwSingleCarrier
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

      RFmxNRMXObwSweepTimeAuto sweepTimeAuto;
      double sweepTimeInterval;

      RFmxNRMXObwAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxNRMXObwAveragingType averagingType;

      RFmxNRMXObwSpanAuto spanAuto;
      double span;
	  RFmxNRMXObwPowerIntegrationMethod powerIntegrationMethod;

      double timeout;

      double occupiedBandwidth;                                            /* (Hz) */
      double absolutePower;                                                /* (dBm) */
      double startFrequency;                                               /* (Hz) */
      double stopFrequency;                                                /* (Hz) */

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

         sweepTimeAuto = RFmxNRMXObwSweepTimeAuto.True;
         sweepTimeInterval = 1.0e-3;                                       /* (s) */

         averagingEnabled = RFmxNRMXObwAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxNRMXObwAveragingType.Rms;

         spanAuto = RFmxNRMXObwSpanAuto.True;
         span = 200e6;                                                     /* (Hz) */
		 powerIntegrationMethod = RFmxNRMXObwPowerIntegrationMethod.Normal;

         timeout = 10.0;                                                   /* (s) */
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
            triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxNRMXIQPowerEdgeTriggerLevelType.Relative,
            iqPowerEdgeEnabled);

         NR.SetLinkDirection("", linkDirection);
         NR.SetFrequencyRange("", frequencyRange);
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth);
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing);

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Obw, true);

         NR.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         NR.Obw.Configuration.SetSpanAuto("", spanAuto);
		 NR.Obw.Configuration.SetPowerIntegrationMethod("", powerIntegrationMethod);
         NR.Obw.Configuration.SetSpan("subblock0", span);
         NR.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         NR.Initiate("", "");
      }

      void RetrieveResults()
      {
         NR.Obw.Results.FetchSpectrum("", timeout, ref spectrum);

         NR.Obw.Results.FetchMeasurement("", timeout, out occupiedBandwidth, out absolutePower,
            out startFrequency, out stopFrequency);
      }

      void PrintResults()
      {
         Console.WriteLine("----------------- Measurement -----------------\n");
         Console.WriteLine("Occupied Bandwidth (Hz)   : {0}", occupiedBandwidth);
         Console.WriteLine("Absolute Power (dBm)      : {0}", absolutePower);
         Console.WriteLine("Start Frequency (Hz)      : {0}", startFrequency);
         Console.WriteLine("Stop Frequency (Hz)       : {0}\n", stopFrequency);
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
