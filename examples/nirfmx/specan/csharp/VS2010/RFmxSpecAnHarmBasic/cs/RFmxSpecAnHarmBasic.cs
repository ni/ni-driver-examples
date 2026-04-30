//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure Harm RBW
//5. Configure Harm Measurement Interval
//6. Configure Harm Number of Harmonics
//7. Configure Harm Averaging
//8. Read Harmonics Measurement Results
//9. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnHarmBasic
{
   public class RFmxSpecAnMXHarmBasic
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;

      public void Run()
      {
         string resourceName = "RFSA";
         string selectedPorts = "";
         double centerFrequency = 1e+9;          /* Hz */
         double referenceLevel = 0.00;           /* dBm */
         double externalAttenuation = 0.00;      /* dB */

         // Fundamental
         double rbw = 100e+3;                    /* Hz */
         double measurementInterval = 1.0e-3;    /* seconds */
         RFmxSpecAnMXHarmRbwFilterType rbwFilterType = RFmxSpecAnMXHarmRbwFilterType.Gaussian;
         double rrcAlpha = 0.1;

         int numberOfHarmonics = 3;

         //Averaging 
         RFmxSpecAnMXHarmAveragingEnabled averagingEnabled = RFmxSpecAnMXHarmAveragingEnabled.False;
         int averagingCount = 10;
         RFmxSpecAnMXHarmAveragingType averagingType = RFmxSpecAnMXHarmAveragingType.Rms;

         double timeout = 10.0;                  /* seconds */

         double totalHarmonicDistortion, averageFundamentalPower;

         try
         {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");

            /* Get SpecAn signal */
            specAn = instrSession.GetSpecAnSignalConfiguration();

            /* Configure measurement */
            specAn.SetSelectedPorts("", selectedPorts);
            specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            specAn.Harm.Configuration.ConfigureFundamentalRbw("", rbw, rbwFilterType, rrcAlpha);
            specAn.Harm.Configuration.ConfigureFundamentalMeasurementInterval("", measurementInterval);
            specAn.Harm.Configuration.ConfigureNumberOfHarmonics("", numberOfHarmonics);
            specAn.Harm.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);

            /* Retrieve results */
            specAn.Harm.Results.Read("", timeout, out totalHarmonicDistortion, out averageFundamentalPower);

            Console.WriteLine("Total Harmonic Distoration (%) {0}\n", totalHarmonicDistortion);
            Console.WriteLine("Average Fundamental Power (dBm) {0}\n", averageFundamentalPower);
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

      private void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }
   }
}
