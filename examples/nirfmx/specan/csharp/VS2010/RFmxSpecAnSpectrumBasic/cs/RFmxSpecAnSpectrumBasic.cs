//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure Spectrum Span
//5. Configure Spectrum RBW filter
//6. Configure Spectrum Averaging
//7. Read Spectrum Measurement Results
//8. Close the RFmx Session

using System;
using NationalInstruments;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnSpectrumBasic
{
   public class RFmxSpecAnSpectrumBasic
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
         double timeout = 10;                        /* seconds */
         double span = 1.0e+6;                       /* Hz */

         //RBW Filter
         RFmxSpecAnMXSpectrumRbwAutoBandwidth rbwAuto = RFmxSpecAnMXSpectrumRbwAutoBandwidth.True;
         double rbw = 100.0e+3;
         RFmxSpecAnMXSpectrumRbwFilterType rbwFilterType = RFmxSpecAnMXSpectrumRbwFilterType.Gaussian;

         //Averaging 
         RFmxSpecAnMXSpectrumAveragingEnabled averagingEnabled = RFmxSpecAnMXSpectrumAveragingEnabled.False;
         int averagingCount = 10;
         RFmxSpecAnMXSpectrumAveragingType averagingType = RFmxSpecAnMXSpectrumAveragingType.Rms;

         Spectrum<float> spectrum = null;

         try
         {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");

            /* Get SpecAn signal */
            specAn = instrSession.GetSpecAnSignalConfiguration();

            /* Configure measurement */
            specAn.SetSelectedPorts("", selectedPorts);
            specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            specAn.Spectrum.Configuration.ConfigureSpan("", span);
            specAn.Spectrum.Configuration.ConfigureRbwFilter("", rbwAuto, rbw, rbwFilterType);
            specAn.Spectrum.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);

            /* Retrieve results */
            specAn.Spectrum.Results.Read("", timeout, ref spectrum);

            Console.WriteLine("Start Frequency(Hz)       {0}", spectrum.StartFrequency);
            Console.WriteLine("Frequency Increment(Hz)   {0}", spectrum.FrequencyIncrement);
            Console.WriteLine("Sample Count              {0}", spectrum.SampleCount);

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
