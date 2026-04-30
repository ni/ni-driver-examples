//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Load configuration from rfmxconfig file
//4. Configure Frequency Reference
//5. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//6. Initiate Measurement
//7. Fetch ACP Measurements and Traces
//8. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnAcpFromConfigurationFile
{
   public class RFmxSpecAnAcpFromConfigurationFile
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      string resourceName;
      string configurationFileName;

      string selectedPorts;
      double centerFrequency, referenceLevel, externalAttenuation;
      double frequency;
      double timeout;
      string frequencySource;

      //Output values
      double absolutePower;
      double[] lowerRelativePower;
      double[] upperRelativePower;
      double[] lowerAbsolutePower;
      double[] upperAbsolutePower;

      internal void Run()
      {
         resourceName = "RFSA";
         selectedPorts = "";
         centerFrequency = 1e+9;             /* Hz */
         referenceLevel = 0.00;              /* dBm */
         externalAttenuation = 0.00;         /* dB */
         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10.0e+6;                /* Hz */
         configurationFileName = "SpecAn_Configurations.rfmxconfig";
         timeout = 10.0;   /* seconds */

         try
         {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");

            /* Get SpecAn signal */
            specAn = instrSession.GetSpecAnSignalConfiguration();

            /* Configure measurement */
            instrSession.LoadAllConfigurations(configurationFileName, true);
            instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
            specAn.SetSelectedPorts("", selectedPorts);
            specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);

            specAn.Initiate("", "");

            /* Retrieve results */
            Spectrum<float> spectrum = null;
            double totalRelativePower, carrierFrequency, integrationBandwidth;

            specAn.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref lowerRelativePower,
                                                           ref upperRelativePower,
                                                           ref lowerAbsolutePower,
                                                           ref upperAbsolutePower);

            specAn.Acp.Results.FetchCarrierMeasurement("", timeout, out absolutePower,
                                                       out totalRelativePower,
                                                       out carrierFrequency,
                                                       out integrationBandwidth);

            specAn.Acp.Results.FetchSpectrum("", timeout, ref spectrum);

            Console.WriteLine("-----------------Carrier Measurements-----------------\n");
            Console.WriteLine("Absolute Power (dBm or dBm/Hz)       {0}", absolutePower);

            Console.WriteLine("\n--------------Offset Channel Measurements-------------\n");
            for (int i = 0; i < lowerRelativePower.Length; i++)
            {
               Console.WriteLine("----Offset 0\n", i);
               Console.WriteLine("Lower Relative Power (dB)            {0}", lowerRelativePower[i]);
               Console.WriteLine("Upper Relative Power (dB)            {0}", upperRelativePower[i]);
               Console.WriteLine("Lower Absolute Power (dBm or dBm/Hz) {0}", lowerAbsolutePower[i]);
               Console.WriteLine("Upper Absolute Power (dBm or dBm/Hz) {0}", upperAbsolutePower[i]);
            }
            Console.WriteLine("-------------------------------------------------\n");
         }
         catch (Exception ex)
         {
            DisplayError(ex);
         }
         finally
         {
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

      static private void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }
   }
}
