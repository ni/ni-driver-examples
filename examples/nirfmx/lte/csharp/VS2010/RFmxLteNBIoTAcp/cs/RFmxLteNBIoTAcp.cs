//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Configure Uplink Subcarrier Spacing.
//7. Select ACP measurement and enable Traces.
//8. Configure Averaging Parameters for ACP measurement.
//9. Configure Sweep Time Parameters.
//10. Initiate the Measurement.
//11. Fetch ACP Measurements and Traces.
//12. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteNBIoTAcp
{
   public class RFmxLteNBIoTAcp
   {
      RFmxInstrMX instrSession;
      RFmxLteMX lte;

      string resourceName, frequencyReferenceSource, iqPowerEdgeTriggerSource;

      double frequencyReferenceFrequency, centerFrequency, referenceLevel, externalAttenuation,
             triggerDelay, componentCarrierFrequency, componentCarrierBandwidth,
             sweepTimeInterval, timeout;

      bool enableTrigger;
      int cellID, averagingCount, numberOfOffsets;
      int i;
      double iqPowerEdgeTriggerLevel, minimumQuietTimeDuration;
      RFmxLteMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
      RFmxLteMXAcpAveragingEnabled averagingEnabled;
      RFmxLteMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
      RFmxLteMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
      RFmxLteMXAcpAveragingType averagingType;
      RFmxLteMXAcpSweepTimeAuto sweepTimeAuto;
      RFmxLteMXNBIoTUplinkSubcarrierSpacing uplinkSubcarrierSpacing;
	  RFmxLteMXLinkDirection linkDirection;

      double absolutePower, relativePower;
      double[] lowerAbsolutePower, upperAbsolutePower, lowerRelativePower, upperRelativePower;
      Spectrum<float> spectrum;
      Spectrum<float> absolutePowerTrace;
      Spectrum<float> relativePowerTrace;

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

         enableTrigger = true;
         iqPowerEdgeTriggerSource = "0";
         iqPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising;
         iqPowerEdgeTriggerLevel = -20.00;   /*(dB) */
         iqPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative;
         minimumQuietTimeDuration = 100e-6;  /* seconds */
         minimumQuietTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto;
         triggerDelay = 0.0;                 /* seconds */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10e6; /* Hz */

         linkDirection = RFmxLteMXLinkDirection.Uplink;

         uplinkSubcarrierSpacing = RFmxLteMXNBIoTUplinkSubcarrierSpacing.SubcarrierSpacing15kHz;

         sweepTimeAuto = RFmxLteMXAcpSweepTimeAuto.True;
         sweepTimeInterval = 0.001;          /* seconds */

         averagingEnabled = RFmxLteMXAcpAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxLteMXAcpAveragingType.Rms;

         componentCarrierBandwidth = 200e3;  /* Hz */
         componentCarrierFrequency = 0.0;    /* Hz */
         cellID = 0;

         numberOfOffsets = 2;

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

         lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);

         lte.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel,
            triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger);

         lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID);

         lte.ComponentCarrier.ConfigureNBIoTComponentCarrier("", cellID, uplinkSubcarrierSpacing);
		 
		 lte.ConfigureLinkDirection("", linkDirection);

         lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Acp, true);

         lte.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);

         lte.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);

         lte.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         lte.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref lowerRelativePower, ref upperRelativePower,
            ref lowerAbsolutePower, ref upperAbsolutePower);

         lte.Acp.Results.ComponentCarrier.FetchMeasurement("", timeout, out absolutePower, out relativePower);

         for (i = 0; i < numberOfOffsets; i++)
         {
            lte.Acp.Results.FetchAbsolutePowersTrace("", timeout, i, ref absolutePowerTrace);
         }

         for (i = 0; i < numberOfOffsets; i++)
         {
            lte.Acp.Results.FetchRelativePowersTrace("", timeout, i, ref relativePowerTrace);
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
