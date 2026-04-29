//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties(Center Frequency, RF Attenuation and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Configure Reference Level.
//7. Configure Duplex Mode.
//8. Configure Link Direction.
//9. Select ACP measurement and enable Traces.
//10. Configure Measurement Method.
//11. Configure Averaging Parameters for ACP measurement.
//12. Configure Sweep Time Parameters.
//13. Configure Noise Compensation Parameter.
//14. Initiate the Measurement.
//15. Fetch ACP Measurements and Traces.
//16. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteAcpSingleCarrier
{
   public class RFmxLteAcpSingleCarrier
   {
      RFmxInstrMX instrSession;
      RFmxLteMX lte;

      string resourceName, frequencyReferenceSource, digitalEdgeSource;

      double frequencyReferenceFrequency, centerFrequency, referenceLevel, autoSetReferenceLevel, externalAttenuation,
             rfAttenuation, triggerDelay, componentCarrierFrequency, componentCarrierBandwidth,
             measurementInterval, sweepTimeInterval, timeout;

      bool enableTrigger, autoLevel;

      int cellID, averagingCount, numberOfOffsets, i;

      RFmxInstrMXRFAttenuationAuto rfAttenuationAuto;
      RFmxLteMXAcpAveragingEnabled averagingEnabled;
      RFmxLteMXAcpAveragingType averagingType;
      RFmxLteMXDigitalEdgeTriggerEdge digitalEdge;
      RFmxLteMXDuplexScheme duplexScheme;
      RFmxLteMXAcpMeasurementMethod measurementMethod;
      RFmxLteMXAcpNoiseCompensationEnabled noiseCompensationEnabled;
      RFmxLteMXAcpSweepTimeAuto sweepTimeAuto;
      RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration;
      RFmxLteMXLinkDirection linkDirection;

      double absolutePower, relativePower;
      double[] lowerAbsolutePower, upperAbsolutePower, lowerRelativePower, upperRelativePower;
      Spectrum<float> spectrum;
      Spectrum<float> absolutePowersTrace;
      Spectrum<float> relativePowersTrace;

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

      private void InitializeVariables()
      {
         resourceName = "RFSA";

         centerFrequency = 1.95e+9;          /* Hz */
         referenceLevel = 0.00;              /* dBm */
         externalAttenuation = 0.00;         /* dB */

         autoLevel = true;

         rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.True;
         rfAttenuation = 10.0;               /* dB */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10e6;                   /* Hz */

         enableTrigger = false;
         digitalEdgeSource = RFmxLteMXConstants.Pfi0;
         digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                 /* seconds */

         uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;
         duplexScheme = RFmxLteMXDuplexScheme.Fdd;
         linkDirection = RFmxLteMXLinkDirection.Uplink;

         componentCarrierBandwidth = 10e6;    /* Hz */
         componentCarrierFrequency = 0.0;    /* Hz */
         cellID = 0;

         measurementInterval = 0.01;

         measurementMethod = RFmxLteMXAcpMeasurementMethod.Normal;

         noiseCompensationEnabled = RFmxLteMXAcpNoiseCompensationEnabled.False;

         sweepTimeAuto = RFmxLteMXAcpSweepTimeAuto.True;
         sweepTimeInterval = 0.001;          /* seconds */

         averagingEnabled = RFmxLteMXAcpAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxLteMXAcpAveragingType.Rms;

         numberOfOffsets = 3;

         timeout = 10.0;                    /* seconds */
      }

      private void InitializeInstr()
      {
         /* Create a new RFmx Session */
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      private void ConfigureLte()
      {
         /* Get Lte signal */
         lte = instrSession.GetLteSignalConfiguration();

         /* Configure measurement */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

         lte.ConfigureFrequency("", centerFrequency);

         lte.ConfigureExternalAttenuation("", externalAttenuation);

         instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation);

         lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);

         lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency,
                                        cellID);

         if (autoLevel)
         {
            lte.AutoLevel("", measurementInterval, out autoSetReferenceLevel);
            Console.WriteLine("Reference level (dBm)  : {0}\n", autoSetReferenceLevel);
         }
         else
         {
            lte.ConfigureReferenceLevel("", referenceLevel);
         }

         lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration);

         lte.ConfigureLinkDirection("", linkDirection);

         lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Acp, true);

         lte.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod);

         lte.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);

         lte.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);

         lte.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled);

         lte.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         lte.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref lowerRelativePower,
                                                      ref upperRelativePower, ref lowerAbsolutePower,
                                                      ref upperAbsolutePower);

         lte.Acp.Results.ComponentCarrier.FetchMeasurement("", timeout, out absolutePower,
                                                           out relativePower);

         for (i = 0; i < numberOfOffsets; i++)
         {
            lte.Acp.Results.FetchAbsolutePowersTrace("", timeout, i, ref absolutePowersTrace);
         }

         for (i = 0; i < numberOfOffsets; i++)
         {
            lte.Acp.Results.FetchRelativePowersTrace("", timeout, i, ref relativePowersTrace);
         }

         lte.Acp.Results.FetchSpectrum("", timeout, ref spectrum);
      }

      private void PrintResults()
      {
         Console.WriteLine("Carrier Absolute Power  (dBm)   : {0}", absolutePower);
         Console.WriteLine("\n-----------Offset Channel Measurements-----------");
         for (int i = 0; i < lowerRelativePower.Length; i++)
         {
            Console.WriteLine("\nOffset  {0}", i);
            Console.WriteLine("Lower Relative Power (dB)  : {0}", lowerRelativePower[i]);
            Console.WriteLine("Upper Relative Power (dB)  : {0}", upperRelativePower[i]);
            Console.WriteLine("Lower Absolute Power (dBm) : {0}", lowerAbsolutePower[i]);
            Console.WriteLine("Upper Absolute Power (dBm) : {0}", upperAbsolutePower[i]);
            Console.WriteLine("------------------------------------------");
         }
      }

      private void CloseSession()
      {
         try
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
         catch (Exception ex)
         {
            DisplayError(ex);
         }
      }

      static private void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }
   }
}
