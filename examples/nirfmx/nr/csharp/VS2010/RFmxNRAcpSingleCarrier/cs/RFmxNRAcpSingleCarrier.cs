//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, RF Attenuation and External Attenuation).
//5. Configure Trigger Parameters for IQ Power Edge Trigger.
//6. Configure Link Direction, Frequency Range and Carrier Bandwidth and Subcarrier Spacing.
//7. Configure Reference Level.
//8. Select ACP measurement and enable Traces.
//9. Configure Measurement Method.
//10. Configure Noise Compensation Parameter.
//11. Configure Sweep Time Parameters.
//12. Configure Averaging Parameters for ACP measurement.
//13. Initiate the Measurement.
//14. Fetch ACP Measurements and Traces.
//15. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRAcpSingleCarrier
{
   public class RFmxNRAcpSingleCarrier
   {
      RFmxInstrMX instrSession;
      RFmxNRMX NR;
      string resourceName;

      string selectedPorts;
      double centerFrequency;
      double externalAttenuation;

      bool autoLevel;
      double referenceLevel;
      double measurementInterval;

      RFmxInstrMXRFAttenuationAuto rfAttenuationAuto;
      double rfAttenuation;

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
      RFmxNRMXAcpMeasurementMethod measurementMethod;
      RFmxNRMXAcpNoiseCompensationEnabled noiseCompensationEnabled;

      RFmxNRMXAcpSweepTimeAuto sweepTimeAuto;
      double sweepTimeInterval;

      RFmxNRMXAcpAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxNRMXAcpAveragingType averagingType;

      double timeout;

      double absolutePower;                                                      /* (dBm) */
      double relativePower;                                                      /* (dB) */

      double[] lowerRelativePower;                                               /* (dB) */
      double[] upperRelativePower;                                               /* (dB) */
      double[] lowerAbsolutePower;                                               /* (dBm) */
      double[] upperAbsolutePower;                                               /* (dBm) */

      Spectrum<float> spectrum;
      Spectrum<float> relativePowersTrace;

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
         externalAttenuation = 0.0;                                              /* (dB) */

         autoLevel = true;
         referenceLevel = 0.0;                                                   /* (dBm) */
         measurementInterval = 10.0e-3;                                          /* (s) */

         rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.True;
         rfAttenuation = 10.0;                                                   /* (dB) */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e6;                                   /* (Hz) */

         iqPowerEdgeEnabled = false;
         iqPowerEdgeLevel = -20.0;                                               /* (dB or dBm) */
         triggerDelay = 0.0;                                                     /* (s) */
         minimumQuietTimeMode = RFmxNRMXTriggerMinimumQuietTimeMode.Auto;
         minimumQuietTime = 8.0e-6;                                              /* (s) */

         linkDirection = RFmxNRMXLinkDirection.Uplink;
         frequencyRange = RFmxNRMXFrequencyRange.Range1;
         carrierBandwidth = 100e6;                                               /* (Hz) */
         subcarrierSpacing = 30e3;                                               /* (Hz) */
         measurementMethod = RFmxNRMXAcpMeasurementMethod.Normal;
         noiseCompensationEnabled = RFmxNRMXAcpNoiseCompensationEnabled.False;

         sweepTimeAuto = RFmxNRMXAcpSweepTimeAuto.True;
         sweepTimeInterval = 1.0e-3;                                             /* (s) */

         averagingEnabled = RFmxNRMXAcpAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxNRMXAcpAveragingType.Rms;

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
         NR.ConfigureFrequency("", centerFrequency);
         NR.ConfigureExternalAttenuation("", externalAttenuation);
         instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation);
         NR.ConfigureIQPowerEdgeTrigger("", "0", RFmxNRMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
            triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxNRMXIQPowerEdgeTriggerLevelType.Relative,
            iqPowerEdgeEnabled);
         NR.SetLinkDirection("", linkDirection);
         NR.SetFrequencyRange("", frequencyRange);
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth);
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing);
         if (autoLevel)
         {
             NR.AutoLevel("", measurementInterval, out referenceLevel);
             Console.WriteLine("Reference level  (dBm)       : {0}", referenceLevel);
         }
         else
         {
             NR.ConfigureReferenceLevel("", referenceLevel);
         }
         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Acp, true);
         NR.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod);
         NR.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled);
         NR.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         NR.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         NR.Initiate("", "");
      }

      void RetrieveResults()
      {
         NR.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref lowerRelativePower,
            ref upperRelativePower, ref lowerAbsolutePower, ref upperAbsolutePower);

         NR.Acp.Results.ComponentCarrier.FetchMeasurement("", timeout, out absolutePower, out relativePower);

         for (int i = 0; i < lowerRelativePower.Length; i++)
         {
             NR.Acp.Results.FetchRelativePowersTrace("", timeout, i, ref relativePowersTrace);
         }

         NR.Acp.Results.FetchSpectrum("", timeout, ref spectrum);
      }

      void PrintResults()
      {
         Console.WriteLine("\nCarrier Absolute Power (dBm or dBm/Hz) : {0}", absolutePower);

         Console.WriteLine("\n-----------Offset Channel Measurements----------- \n");
         for (int i = 0; i < lowerRelativePower.Length; i++)
         {
             Console.WriteLine("Offset  {0}", i);
             Console.WriteLine("Lower Relative Power (dB)              : {0}", lowerRelativePower[i]);
             Console.WriteLine("Upper Relative Power (dB)              : {0}", upperRelativePower[i]);
             Console.WriteLine("Lower Absolute Power (dBm or dBm/Hz)   : {0}", lowerAbsolutePower[i]);
             Console.WriteLine("Upper Absolute Power (dBm or dBm/Hz)   : {0}", upperAbsolutePower[i]);
             Console.WriteLine("-------------------------------------------------\n");
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
