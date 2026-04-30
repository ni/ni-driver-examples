//Steps:
//1. Open a new RFmx session.
//2. Configure the basic instrument properties (Clock Source and Clock Frequency).
//3. Configure Selected Ports.
//4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//6. Configure Txp measurement and enable the traces.
//7. Configure the Measurement Interval.
//8. Configure Rbw filter parameters.
//9. Configure Thresholding.
//10. Configure Averaging parameters.
//11. Configure Vbw filter parameters.
//12. Initiate Measurement.
//13. Fetch Txp Traces and Measurements.
//14. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnTxpIQDevice
{
   public class RFmxSpecAnTxpIQDevice
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;

      string resourceName;
      string selectedPorts;
      double centerFrequency;
      double referenceLevel;
      double externalAttenuation;
      string frequencySource;
      double frequency;

      double triggerDelay;
      double iqPowerEdgeLevel;
      double minimumQuietTime;
      bool iqPowerEdgeEnabled;

      double measurementInterval;

      RFmxSpecAnMXTxpRbwFilterType rbwFilterType;
      double rbw;
      double rrcAlpha;

      RFmxSpecAnMXTxpVbwFilterAutoBandwidth vbwAuto;
      double vbw;
      double vbwToRbwRatio;

      RFmxSpecAnMXTxpThresholdEnabled thresholdEnabled;
      RFmxSpecAnMXTxpThresholdType thresholdType;
      double thresholdLevel;

      RFmxSpecAnMXTxpAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxSpecAnMXTxpAveragingType averagingType;

      double timeout;
      double averageMeanPower;
      double peakToAverageRatio;
      double maximumPower;
      double minimumPower;
      AnalogWaveform<float> power = null;

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

         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10e+6;                      /* Hz */

         iqPowerEdgeEnabled = false;
         iqPowerEdgeLevel = -20.0;               /* dBm */
         triggerDelay = 0.0;                     /* seconds */
         minimumQuietTime = 0.0;                     /* seconds */

         measurementInterval = 1e-3;             /* seconds */

         rbwFilterType = RFmxSpecAnMXTxpRbwFilterType.Gaussian;
         rbw = 100e+3;                           /* Hz */
         rrcAlpha = 0.010;

         vbwAuto = RFmxSpecAnMXTxpVbwFilterAutoBandwidth.True;
         vbw = 30.0e3;                           /* Hz */
         vbwToRbwRatio = 3;

         thresholdEnabled = RFmxSpecAnMXTxpThresholdEnabled.False;
         thresholdType = RFmxSpecAnMXTxpThresholdType.Relative;
         thresholdLevel = -20.0;                 /* (dB or dBm) */

         averagingEnabled = RFmxSpecAnMXTxpAveragingEnabled.False;
         averagingType = RFmxSpecAnMXTxpAveragingType.Rms;
         averagingCount = 10;

         timeout = 10;                           /* seconds */
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
         specAn.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeLevel, RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising,
             triggerDelay, RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual, minimumQuietTime, iqPowerEdgeEnabled);
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
