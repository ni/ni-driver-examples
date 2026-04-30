//Steps:
//1. Open a new RFmx session
//2. Configure the instrument properties Clock Source and Clock Frequency
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select OBW measurement and enable the traces
//6. Configure OBW Bandwidth Percentange, Span and Sweep Time
//7. Configure OBW Averaging
//8. Configure OBW RBW filter
//9. Configure OBW FFT
//10. Configure OBW Power Units
//11. Initiate Measurement
//12. Fetch OBW Measurement and Traces
//13. Close the RFmx Session

using System;
using NationalInstruments;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnObw
{
   public class RFmxSpecAnObw
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      string resourceName, frequencySource;
      string selectedPorts;
      double centerFrequency, referenceLevel, externalAttenuation,
             fftPadding, rbw, timeOut, frequency, span,
             bandWidthPercentage, sweepTimeInterval;
      RFmxSpecAnMXObwAveragingEnabled averagingEnabled;
      RFmxSpecAnMXObwAveragingType averagingType;
      RFmxSpecAnMXObwRbwFilterType rbwFilterType;
      RFmxSpecAnMXObwRbwAutoBandwidth rbwAutoBandwidth;
      RFmxSpecAnMXObwPowerUnits powerUnits;
      RFmxSpecAnMXObwFftWindow fftWindow;
      RFmxSpecAnMXObwSweepTimeAuto sweepTimeAuto;
      int averagingCount;

      double stopFrequency, startFrequency, occupiedBandwidth, averagePower, frequencyResolution;

      public void Run()
      {
         try
         {
            InitializeVariables();
            InitializeInstr();
            ConfigureSpecAn();
            RetrieveResults();
            PrintResults();
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

      private void InitializeVariables()
      {
         /* Initialize input variables */

         resourceName = "RFSA";

         selectedPorts = "";
         centerFrequency = 1e+9;             /* Hz */
         referenceLevel = 0.00;              /* dBm */
         externalAttenuation = 0.00;         /* dB */

         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10e+6;                  /* Hz */

         span = 1e+6;                        /* Hz */
         bandWidthPercentage = 99.00;
         powerUnits = RFmxSpecAnMXObwPowerUnits.dBm;

         //RBW Filter
         rbwFilterType = RFmxSpecAnMXObwRbwFilterType.Gaussian;
         rbw = 10e+3;
         rbwAutoBandwidth = RFmxSpecAnMXObwRbwAutoBandwidth.True;

         //Sweep Time
         sweepTimeAuto = RFmxSpecAnMXObwSweepTimeAuto.True;
         sweepTimeInterval = 1e-3;           /* seconds */

         //Averaging
         averagingEnabled = RFmxSpecAnMXObwAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxSpecAnMXObwAveragingType.Rms;

         //FFT
         fftWindow = RFmxSpecAnMXObwFftWindow.FlatTop;
         fftPadding = -1.00;

         timeOut = 10;                       /* seconds */
      }

      private void InitializeInstr()
      {
         /* Create a new RFmx Session */
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      private void ConfigureSpecAn()
      {
         /* Get SpecAn signal */
         specAn = instrSession.GetSpecAnSignalConfiguration();

         /* Configure measurement */
         instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
         specAn.SetSelectedPorts("", selectedPorts);
         specAn.ConfigureFrequency("", centerFrequency);
         specAn.ConfigureReferenceLevel("", referenceLevel);
         specAn.ConfigureExternalAttenuation("", externalAttenuation);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Obw, true);
         specAn.Obw.Configuration.ConfigureBandwidthPercentage("", bandWidthPercentage);
         specAn.Obw.Configuration.ConfigureSpan("", span);
         specAn.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         specAn.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                     averagingType);
         specAn.Obw.Configuration.ConfigureRbwFilter("", rbwAutoBandwidth, rbw, rbwFilterType);
         specAn.Obw.Configuration.ConfigureFft("", fftWindow, fftPadding);
         specAn.Obw.Configuration.ConfigurePowerUnits("", powerUnits);

         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */
         Spectrum<float> spectrum = null;
         specAn.Obw.Results.FetchSpectrumTrace("", timeOut, ref spectrum);
         specAn.Obw.Results.FetchMeasurement("", timeOut, out occupiedBandwidth, out averagePower,
                                             out frequencyResolution, out startFrequency,
                                             out stopFrequency);
      }

      private void PrintResults()
      {
         Console.WriteLine("Occupied Bandwidth (Hz)       {0}", occupiedBandwidth);
         Console.WriteLine("Average Power (dBm or dBm/Hz) {0}", averagePower);
         Console.WriteLine("Frequency Resolution (Hz)     {0}", frequencyResolution);
         Console.WriteLine("Start Frequency (Hz)          {0}", startFrequency);
         Console.WriteLine("Stop Frequency (Hz)           {0}", stopFrequency);
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
