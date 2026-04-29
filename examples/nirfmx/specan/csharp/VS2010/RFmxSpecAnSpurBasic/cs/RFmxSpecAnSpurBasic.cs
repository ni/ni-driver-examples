//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Reference Level and External Attenuation)
//4. Select Spur measurement 
//5. Configure Spur Number of Ranges
//6. Configure Spur Start frequency, Stop frequency, RBW filter and Absolute Limit Start for all the ranges
//7. Initiate Measurement
//8. Read Spur Measurement Status
//9. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnSpurBasic
{
   public class RFmxSpecAnSpurBasic
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;

      public void Run()
      {
         string resourceName = "RFSA";
         string selectedPorts = "";
         double centerFrequency = 0.0;               /* Hz */
         double referenceLevel = 0.00;               /* dBm */
         double externalAttenuation = 0.00;          /* dB */

         //Range list
         int rangeListSize = 1;
         double[] startFrequency = { 1e+9 };               /* Hz */
         double[] stopFrequency = { 1.5e+9 };              /* Hz */
         RFmxSpecAnMXSpurRangeEnabled[] rangeEnabled = { RFmxSpecAnMXSpurRangeEnabled.True };

         //RBW Filter
         RFmxSpecAnMXSpurRbwAutoBandwidth[] rbwFilterAutoBandwidth = { RFmxSpecAnMXSpurRbwAutoBandwidth.False };
         double[] rbwFilterBandwidth = { 30e+3 };          /* Hz */
         RFmxSpecAnMXSpurRbwFilterType[] rbwFilterType = { RFmxSpecAnMXSpurRbwFilterType.Gaussian };

         double[] limit = { -10.00 };                      /* dBm */
         double timeout = 10;                        /* seconds */

         RFmxSpecAnMXSpurMeasurementStatus measurementStatus;

         try
         {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");

            /* Get SpecAn signal */
            specAn = instrSession.GetSpecAnSignalConfiguration();

            /* Configure measurement */

            specAn.SetSelectedPorts("", selectedPorts);
            specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Spur, true);
            specAn.Spur.Configuration.ConfigureNumberOfRanges("", rangeListSize);

            specAn.Spur.Configuration.ConfigureRangeFrequencyArray("", startFrequency, stopFrequency,
                                                                  rangeEnabled);
            specAn.Spur.Configuration.ConfigureRangeRbwArray("", rbwFilterAutoBandwidth,
                                                                  rbwFilterBandwidth, rbwFilterType);
            specAn.Spur.Configuration.ConfigureRangeAbsoluteLimitArray("", null, limit, null);

            specAn.Initiate("", "");

            /* Retrieve results */
            specAn.Spur.Results.FetchMeasurementStatus("", timeout, out measurementStatus);

            Console.WriteLine("Measurement Status: {0}\n", measurementStatus);
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
