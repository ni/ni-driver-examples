//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure OBW Span
//5. Configure OBW Averaging
//6. Read OBW Measurement Results
//7. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnObwBasic
{
   class RFmxSpecAnMXObwBasic
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;

      public void Run()
      {
         string resourceName = "RFSA";
         string selectedPorts = "";
         double centerFrequency = 1e+9;              /* Hz */
         double referenceLevel = 0.00;               /* dBm */
         double externalAttenuation = 0.00;          /* dB */
         double span = 1e+6;                         /* Hz */
         double timeout = 10;                        /* seconds */
         RFmxSpecAnMXObwAveragingEnabled averagingEnabled = RFmxSpecAnMXObwAveragingEnabled.False;
         RFmxSpecAnMXObwAveragingType averagingType = RFmxSpecAnMXObwAveragingType.Rms;
         int averagingCount = 10;

         double stopFrequency, startFrequency, occupiedBandwidth, averagePower, frequencyResolution;

         try
         {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");

            /* Get SpecAn signal */
            specAn = instrSession.GetSpecAnSignalConfiguration();

            /* Configure measurement */
            specAn.SetSelectedPorts("", selectedPorts);
            specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            specAn.Obw.Configuration.ConfigureSpan("", span);
            specAn.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);

            /* Retrieve results */
            specAn.Obw.Results.Read("", timeout, out occupiedBandwidth, out averagePower, out frequencyResolution,
                                    out startFrequency, out stopFrequency);

            Console.WriteLine("Occupied Bandwidth (Hz)   {0}", occupiedBandwidth);
            Console.WriteLine("Average total Power (dBm) {0}", averagePower);
            Console.WriteLine("Frequency Resolution (Hz) {0}", frequencyResolution);
            Console.WriteLine("Start Frequency (Hz)      {0}", startFrequency);
            Console.WriteLine("Stop Frequency (Hz)       {0}", stopFrequency);
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
