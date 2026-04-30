//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure CCDF Measurement Interval
//5. Configure CCDF RBW
//6. Read CCDF Measurement Results
//7. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnCcdfBasic
{
   public class RFmxSpecAnCcdfBasic
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;

      public void Run()
      {
         string resourceName = "RFSA";
         string selectedPorts = "";
         double centerFrequency = 1e+9;        /* Hz */
         double referenceLevel = 0.00;         /*  dBm */
         double externalAttenuation = 0.00;    /*  dB */
         double measurementInterval = 1.0e-3;  /* seconds */
         double rbw = 100.0e+3;                /* Hz */
         bool enableAllTraces = true;
         double timeout = 10.0;                /* seconds */
         double meanPower;                     /* dBm     */
         double meanPowerPercentile;           /* % */
         double peakPower;                     /* dB */
         int measuredSamplesCount;

         try
         {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");

            /* Get SpecAn signal */
            specAn = instrSession.GetSpecAnSignalConfiguration();

            /* Configure measurement */
            specAn.SetSelectedPorts("", selectedPorts);
            specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Ccdf, enableAllTraces);
            specAn.Ccdf.Configuration.ConfigureMeasurementInterval("", measurementInterval);
            specAn.Ccdf.Configuration.SetRbwFilterBandwidth("", rbw);

            /* Retrieve results */
            specAn.Ccdf.Results.Read("", timeout, out meanPower, out meanPowerPercentile, out peakPower, out measuredSamplesCount);

            Console.WriteLine(" Mean Power (dBm)           {0}", meanPower);
            Console.WriteLine(" Mean Power Percentile (%)  {0}", meanPowerPercentile);
            Console.WriteLine(" Peak Power (dB)            {0}", peakPower);
            Console.WriteLine(" Measured Samples Count     {0}", measuredSamplesCount);

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
