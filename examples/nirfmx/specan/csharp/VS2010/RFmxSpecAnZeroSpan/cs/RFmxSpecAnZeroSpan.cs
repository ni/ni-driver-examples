//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties (Clock Source and Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select Spectrum measurement and enable the traces
//6. Configure Spectrum RBW filter
//7. Configure Spectrum Span to Zero
//8. Configure Spectrum Sweep Time Interval
//9. Configure Spectrum Averaging
//10. Initiate Measurement
//11. Fetch Spectrum Power Trace and Measurement
//12. Close the RFmx Session

using System;
using NationalInstruments;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnZeroSpan
{
   public class RFmxSpecAnZeroSpan
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
         double frequency = 10.0e+6;                 /* Hz */
         double timeout = 10.0;                      /* seconds */
         string frequencySource = RFmxInstrMXConstants.OnboardClock;

         //RBW Filter
         RFmxSpecAnMXSpectrumRbwFilterType rbwFilterType = RFmxSpecAnMXSpectrumRbwFilterType.Gaussian;
         double rbw = 10.0e+3;

         //Averaging 
         RFmxSpecAnMXSpectrumAveragingEnabled averagingEnabled = RFmxSpecAnMXSpectrumAveragingEnabled.False;
         int averagingCount = 10;
         RFmxSpecAnMXSpectrumAveragingType averagingType = RFmxSpecAnMXSpectrumAveragingType.Rms;

         /* Sweep time */
         double sweepTimeInterval = 1.0e-3;         /* seconds */

         AnalogWaveform<float> power = null;

         try
         {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");

            /* Get SpecAn signal */
            specAn = instrSession.GetSpecAnSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
            specAn.SetSelectedPorts("", selectedPorts);
            specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Spectrum, true);
            specAn.Spectrum.Configuration.ConfigureRbwFilter("", RFmxSpecAnMXSpectrumRbwAutoBandwidth.False, rbw, rbwFilterType);
            specAn.Spectrum.Configuration.ConfigureSpan("", 0.0); //Zero Span
            specAn.Spectrum.Configuration.ConfigureSweepTime("", RFmxSpecAnMXSpectrumSweepTimeAuto.False, sweepTimeInterval);
            specAn.Spectrum.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            specAn.Initiate("", "");

            /* Retrieve results */
            specAn.Spectrum.Results.FetchPowerTrace("", timeout, ref power);

            Console.WriteLine("Measurement Complete. \n");
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
