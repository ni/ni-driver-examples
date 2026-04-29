//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure TXP RBW
//5. Configure TXP Measurement Interval
//6. Configure TXP Averaging
//7. Read TXP Measurement Results
//8. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnTxpBasic
{
   public class RFmxSpecAnTxpBasic
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;

      public void Run()
      {
         string resourceName = "RFSA";
         string selectedPorts = "";
         double centerFrequency = 1e+9;                  /* Hz */
         double referenceLevel = 0.00;                   /* dBm */
         double externalAttenuation = 0.00;              /* dB */
         double measurementInterval = 1e-3;              /* seconds */
         double timeout = 10;                            /* seconds */

         //RBW Filter
         double rbw = 100e+3;
         RFmxSpecAnMXTxpRbwFilterType rbwFilterType = RFmxSpecAnMXTxpRbwFilterType.Gaussian;
         double rrcAlpha = 0.1;

         //Averaging
         RFmxSpecAnMXTxpAveragingEnabled averagingEnabled = RFmxSpecAnMXTxpAveragingEnabled.False;
         int averagingCount = 10;
         RFmxSpecAnMXTxpAveragingType averagingType = RFmxSpecAnMXTxpAveragingType.Rms;

         double averageMeanPower, peakToAverageRatio, maximumPower, minimumPower;

         try
         {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");

            /* Get SpecAn signal */
            specAn = instrSession.GetSpecAnSignalConfiguration();

            /* Configure measurement */
            specAn.SetSelectedPorts("", selectedPorts);
            specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            specAn.Txp.Configuration.ConfigureMeasurementInterval("", measurementInterval);
            specAn.Txp.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha);
            specAn.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                        averagingType);

            /* Retrieve results */
            specAn.Txp.Results.Read("", timeout, out averageMeanPower, out peakToAverageRatio,
                                    out maximumPower, out minimumPower);

            Console.WriteLine("Average Mean Frequency (Hz)  " + averageMeanPower);
            Console.WriteLine("Mean Phase (deg)             " + peakToAverageRatio);
            Console.WriteLine("Maximum Power (dBm)          " + maximumPower);
            Console.WriteLine("Minimum Power (dBm)          " + minimumPower);

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
