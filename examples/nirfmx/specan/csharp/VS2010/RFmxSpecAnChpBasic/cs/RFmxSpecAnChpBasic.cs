//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure CHP Integration BW
//5. Configure CHP Averaging
//6. Read CHP Measurement Results
//7. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnChpBasic
{
   public class RFmxSpecAnChpBasic
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;

      internal void Run()
      {
         string resourceName = "RFSA";
         string selectedPorts = "";
         double centerFrequency = 1e+9;          /* Hz */
         double referenceLevel = 0.00;           /* dBm */
         double externalAttenuation = 0.00;      /* dB */
         double timeout = 10.0;                  /* seconds */
         double integrationBandwidth = 1.0e+6;   /* Hz */
         RFmxSpecAnMXChpAveragingEnabled averagingEnabled = RFmxSpecAnMXChpAveragingEnabled.False;
         int averagingCount = 10;
         double absolutePower;             /* dBm */
         double psd;            /* dBm/Hz */

         try
         {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");

            /* Get SpecAn signal */
            specAn = instrSession.GetSpecAnSignalConfiguration();

            /* Configure measurement */
            specAn.SetSelectedPorts("", selectedPorts);
            specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            specAn.Chp.Configuration.ConfigureIntegrationBandwidth("", integrationBandwidth);
            specAn.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, RFmxSpecAnMXChpAveragingType.Rms);

            /* Retrieve results */
            specAn.Chp.Results.Read("", timeout, out absolutePower, out psd);

            Console.WriteLine("Absolute Power (dBm) {0}", absolutePower);
            Console.WriteLine("PSD (dBm/Hz)         {0}", psd);

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
