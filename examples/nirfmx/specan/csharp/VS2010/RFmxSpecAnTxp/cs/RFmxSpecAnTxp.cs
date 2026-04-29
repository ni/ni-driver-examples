//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties (Clock Source and Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time)
//6. Configure TXP measurement and enable the traces
//7. Configure TXP Measurement Interval
//8. Configure TXP RBW Filter
//9. Configure TXP Threshold
//10. Configure TXP Averaging
//11. Initiate Measurement
//12. Fetch TXP Traces and Measurements
//13. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnTxp
{
   public class RFmxSpecAnTxp
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;

      string selectedPorts;
      double centerFrequency, referenceLevel, externalAttenuation;
      double measurementInterval, rbw, timeout, rrcAlpha, frequency;
      double vbw, vbwToRbwRatio;
      string resourceName;
      RFmxSpecAnMXTxpAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxSpecAnMXTxpAveragingType averagingType;
      RFmxSpecAnMXTxpRbwFilterType rbwFilterType;
      RFmxSpecAnMXTxpThresholdEnabled thresholdEnabled;
      RFmxSpecAnMXTxpThresholdType thresholdType;
      RFmxSpecAnMXTxpVbwFilterAutoBandwidth vbwAuto;
      double triggerDelay, iqPowerEdgeLevel, minQuietTime, thresholdLevel;
      bool iqPowerEdgeEnabled, enableTrigger;
      private string frequencySource;

      double averageMeanPower, peakToAverageRatio, maximumPower, minimumPower;

      public void Run()
      {
         try
         {
            InitializeVariables();
            InitializeInstr();
            ConfigureSpecAn();
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
            Console.WriteLine("Press any key to exit.....");
            Console.ReadKey();
         }
      }

      private void InitializeVariables()
      {
         /* Initialize input variables */

         resourceName = "RFSA";
         selectedPorts = "";
         centerFrequency = 1e+9;                 /* Hz */
         referenceLevel = 0.00;                  /* dBm */
         externalAttenuation = 0.00;             /* dB */

         measurementInterval = 1e-3;             /* seconds */
         rbw = 100e+3;                           /* Hz */
         timeout = 10;                           /* seconds */

         averagingEnabled = RFmxSpecAnMXTxpAveragingEnabled.False;
         averagingType = RFmxSpecAnMXTxpAveragingType.Rms;
         averagingCount = 10;

         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10e+6;                      /* Hz */

         rbwFilterType = RFmxSpecAnMXTxpRbwFilterType.Gaussian;
         rrcAlpha = 0.010;

         vbwAuto = RFmxSpecAnMXTxpVbwFilterAutoBandwidth.True;
         vbw = 30.0e3;                           /* Hz */
         vbwToRbwRatio = 3;

         thresholdEnabled = RFmxSpecAnMXTxpThresholdEnabled.False;
         thresholdType = RFmxSpecAnMXTxpThresholdType.Relative;
         thresholdLevel = -20.0;                 /* (dBm or dBm / Hz) */

         iqPowerEdgeEnabled = false;
         iqPowerEdgeLevel = -20.0;               /* dBm */
         triggerDelay = 0.0;                     /* seconds */
         minQuietTime = 0.0;                     /* seconds */
         enableTrigger = true;
      }

      private void InitializeInstr()
      {
         /* Create a new RFmx Session */
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      private void ConfigureSpecAn()
      {
         /* Get SpecAn signal */
         specAn = instrSession.GetSpecAnSignalConfiguration();

         /* Configure measurement */
         instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
         specAn.SetSelectedPorts("", selectedPorts);
         specAn.ConfigureFrequency("", centerFrequency);
         specAn.ConfigureReferenceLevel("", referenceLevel);
         specAn.ConfigureExternalAttenuation("", externalAttenuation);
         if (iqPowerEdgeEnabled)
         {
            specAn.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeLevel, RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising,
               triggerDelay, RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual, minQuietTime, enableTrigger);
         }
         else
         {
            specAn.DisableTrigger("");
         }
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Txp, true);
         specAn.Txp.Configuration.ConfigureMeasurementInterval("", measurementInterval);
         specAn.Txp.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha);
         specAn.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         specAn.Txp.Configuration.ConfigureVbwFilter("", vbwAuto, vbw, vbwToRbwRatio);
         specAn.Txp.Configuration.ConfigureThreshold("", thresholdEnabled, thresholdLevel, thresholdType);
         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         AnalogWaveform<float> power = null;
         specAn.Txp.Results.FetchPowerTrace("", timeout, ref power);
         specAn.Txp.Results.FetchMeasurement("", timeout, out averageMeanPower, out peakToAverageRatio, out maximumPower,
                                             out minimumPower);
      }

      private void PrintResults()
      {
         Console.WriteLine("Average Mean Power  (dBm)      : {0}", averageMeanPower);
         Console.WriteLine("Peak to Average Ratio(dB)      : {0}", peakToAverageRatio);
         Console.WriteLine("Maximum Power (dBm)            : {0}", maximumPower);
         Console.WriteLine("Minimum Power (dBm)            : {0}", minimumPower);
      }

      private void CloseSession()
      {
         try
         {
            if (specAn != null)
            {
               specAn.Dispose();
               specAn = null;
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
