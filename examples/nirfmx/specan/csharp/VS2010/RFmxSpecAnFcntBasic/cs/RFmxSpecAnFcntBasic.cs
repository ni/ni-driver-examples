//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure FCnt Measurement Interval
//5. Configure FCnt Averaging
//6. Configure FCnt RBW
//7. Read FCnt Measurement Results
//8. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnFcntBasic
{
   public class RFmxSpecAnFcntBasic
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
         double measurementInterval = 1e-3;      /* seconds */
         double timeout = 10;                    /* seconds */

         RFmxSpecAnMXFcntAveragingEnabled averagingEnabled = RFmxSpecAnMXFcntAveragingEnabled.False;
         int averagingCount = 10;
         RFmxSpecAnMXFcntAveragingType averagingType = RFmxSpecAnMXFcntAveragingType.Mean;

         double rbw = 100e+3;
         double rrcAlpha = 1e-1;
         RFmxSpecAnMXFcntRbwFilterType rbwFilterType = RFmxSpecAnMXFcntRbwFilterType.None;

         double averageRelativeFrequency;    /* Hz */
         double averageAbsoluteFrequency;    /* Hz */
         double meanPhase;                   /* deg */

         try
         {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");

            /* Get SpecAn signal */
            specAn = instrSession.GetSpecAnSignalConfiguration();

            /* Configure measurement */
            specAn.SetSelectedPorts("", selectedPorts);
            specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            specAn.Fcnt.Configuration.ConfigureMeasurementInterval("", measurementInterval);
            specAn.Fcnt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            specAn.Fcnt.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha);

            /* Retrieve results */
            specAn.Fcnt.Results.Read("", timeout, out averageRelativeFrequency, out averageAbsoluteFrequency, out meanPhase);

            Console.WriteLine("Average Relative Frequency (Hz) {0}", averageRelativeFrequency);
            Console.WriteLine("Average Absolute Frequency (Hz) {0}", averageAbsoluteFrequency);
            Console.WriteLine("Mean Phase (deg)                {0}", meanPhase);

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
