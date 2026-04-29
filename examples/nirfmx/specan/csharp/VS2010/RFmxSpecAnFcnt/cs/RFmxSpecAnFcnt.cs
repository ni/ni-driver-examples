//Steps:
//1. Open a new RFmx session
//2. Configure the instrument properties Clock Source and Clock Frequency
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select FCnt measurement and enable the traces
//6. Configure FCnt Measurement Interval
//7. Configure FCnt RBW filter
//8. Configure FCnt Averaging
//9. Configure FCnt Threshold
//10. Initiate Measurment
//11. Fetch FCnt Measurements and Traces
//12. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnFcnt
{
   public class RFmxSpecAnFcnt
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;

      string selectedPorts;
      double centerFrequency, referenceLevel, externalAttenuation, frequency,
             measurementInterval, rbw, timeout, rrcAlpha, thresholdLevel;
      string resourceName, frequencySource;
      RFmxSpecAnMXFcntAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxSpecAnMXFcntAveragingType averagingType;
      RFmxSpecAnMXFcntRbwFilterType rbwFilterType;
      RFmxSpecAnMXFcntThresholdEnabled thresholdEnabled;
      RFmxSpecAnMXFcntThresholdType thresholdType;

      double averageRelativeFrequency;    /* Hz */
      double averageAbsoluteFrequency;    /* Hz */
      double meanPhase;

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
         centerFrequency = 1e+9;         /* Hz */
         referenceLevel = 0.00;          /* dBm */
         externalAttenuation = 0.00;     /* dB */

         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10e+6;              /* Hz */

         measurementInterval = 1e-3;     /* seconds */
         timeout = 10;                   /* seconds */

         //RBW Filter
         rbw = 100e+3;                   /* Hz */
         rbwFilterType = RFmxSpecAnMXFcntRbwFilterType.None;
         rrcAlpha = 0.10;

         //Averaging
         averagingType = RFmxSpecAnMXFcntAveragingType.Mean;
         averagingEnabled = RFmxSpecAnMXFcntAveragingEnabled.False;
         averagingCount = 10;

         //Threshold
         thresholdEnabled = RFmxSpecAnMXFcntThresholdEnabled.False;
         thresholdType = RFmxSpecAnMXFcntThresholdType.Relative;
         thresholdLevel = -20.0;         /* dB or dBm*/
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
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Fcnt, true);
         specAn.Fcnt.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha);
         specAn.Fcnt.Configuration.ConfigureMeasurementInterval("", measurementInterval);
         specAn.Fcnt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         specAn.Fcnt.Configuration.ConfigureThreshold("", thresholdEnabled, thresholdLevel, thresholdType);
         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */
         AnalogWaveform<float> frequencyTrace = null;
         specAn.Fcnt.Results.FetchFrequencyTrace("", timeout, ref frequencyTrace);
         specAn.Fcnt.Results.FetchMeasurement("", timeout, out averageRelativeFrequency,
                                             out averageAbsoluteFrequency, out meanPhase);
      }

      private void PrintResults()
      {
         Console.WriteLine("Average Relative Frequency (Hz) {0}", averageRelativeFrequency);
         Console.WriteLine("Average Absolute Frequency (Hz) {0}", averageAbsoluteFrequency);
         Console.WriteLine("Mean Phase (deg)                {0}", meanPhase);
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
