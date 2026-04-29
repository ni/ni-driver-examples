//Steps:
//1. Open a new RFmx session.
//2. Configure the instrument properties Clock Source and Clock Frequency.
//3. Configure Selected Ports.
//4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Select Harmonics measurement and enable the traces.
//6. Configure RBW Filter parameters.
//7. Configure Measurement Interval of the Fundamental signal.
//8. Configure Number of Harmonics to measure and Auto Harmonics setup.
//9. Configure the parameters of the Harmonics (Measurement Interval, Order, BW and Harmonics Enabled).
//10. Configure Averaging parameters.
//11. Initiate Measurement.
//12. Fetch Harm Measurements and Traces.
//13. Close the RFmx Session.

using System;
using NationalInstruments;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnHarm
{
   public class RFmxSpecAnHarm
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      String resourceName, frequencySource;
      string selectedPorts, harmonicString;
      double referenceLevel, externalAttenuation, centerFrequency, frequency,
             rbw, rrcAlpha, measurementInterval;
      RFmxSpecAnMXHarmRbwFilterType rbwFilterType;
      RFmxSpecAnMXHarmAutoHarmonicsSetupEnabled autoHarmonicsSetup;
      RFmxSpecAnMXHarmAveragingEnabled averagingEnabled;
      RFmxSpecAnMXHarmAveragingType averagingType;
      int averagingCount;

      const int NumberOfHarmonics = 3;

      RFmxSpecAnMXHarmHarmonicEnabled[] harmonicsEnabled = new RFmxSpecAnMXHarmHarmonicEnabled[NumberOfHarmonics];
      double[] harmonicsBandwidth = new double[NumberOfHarmonics];
      int[] harmonicsOrder = new int[NumberOfHarmonics];
      double[] harmonicsMeasurementInterval = new double[NumberOfHarmonics];

      double totalHarmonicDistortion, averageFundamentalPower, fundamentalFrequency, timeout;
      double[] averageRelativePower;
      double[] averageAbsolutePower;
      double[] harmonicsRbw;
      double[] harmonicsFrequency;

      AnalogWaveform<float>[] power = new AnalogWaveform<float>[NumberOfHarmonics];

      public void Run()
      {
         try
         {
            InitializeVariables();
            InitializeInstr();
            ConfigureSpecAn();
            RetreiveResults();
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
         resourceName = "RFSA";

         selectedPorts = "";

         centerFrequency = 1.0e+9;                                   /* Hz */
         referenceLevel = 0.0;                                       /* dBm */
         externalAttenuation = 0.0;                                  /* dB */

         frequencySource = "OnboardClock";
         frequency = 10.0e+6;                                        /* Hz */

         rbwFilterType = RFmxSpecAnMXHarmRbwFilterType.Gaussian;
         rbw = 100e+3;                                               /* Hz */
         rrcAlpha = 0.010;
         measurementInterval = 1.0e-3;                               /* seconds */

         autoHarmonicsSetup = RFmxSpecAnMXHarmAutoHarmonicsSetupEnabled.True;

         for (int i = 0; i < NumberOfHarmonics; i++)
         {
            harmonicsEnabled[i] = RFmxSpecAnMXHarmHarmonicEnabled.True;
            harmonicsOrder[i] = i + 1;
            harmonicsBandwidth[i] = 100.0e+3;                        /* Hz */
            harmonicsMeasurementInterval[i] = 1.0e-3;                /* seconds */
         }

         averagingEnabled = RFmxSpecAnMXHarmAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxSpecAnMXHarmAveragingType.Rms;

         timeout = 10.0;                                             /* seconds */
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
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Harm, true);
         specAn.Harm.Configuration.ConfigureFundamentalRbw("", rbw, rbwFilterType, rrcAlpha);
         specAn.Harm.Configuration.ConfigureFundamentalMeasurementInterval("", measurementInterval);
         specAn.Harm.Configuration.ConfigureAutoHarmonics("", autoHarmonicsSetup);
         if (autoHarmonicsSetup == RFmxSpecAnMXHarmAutoHarmonicsSetupEnabled.True)
         {
            specAn.Harm.Configuration.ConfigureNumberOfHarmonics("", NumberOfHarmonics);
         }
         else
         {
            specAn.Harm.Configuration.ConfigureNumberOfHarmonics("", NumberOfHarmonics);
            specAn.Harm.Configuration.ConfigureHarmonicArray("", harmonicsOrder, harmonicsBandwidth,
               harmonicsEnabled, harmonicsMeasurementInterval);
         }
         specAn.Harm.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         specAn.Initiate("", "");
      }

      private void RetreiveResults()
      {
         specAn.Harm.Results.FetchTotalHarmonicDistortion("", timeout, out totalHarmonicDistortion,
            out averageFundamentalPower, out fundamentalFrequency);

         specAn.Harm.Results.FetchHarmonicMeasurementArray("", timeout, ref averageRelativePower,
            ref averageAbsolutePower, ref harmonicsRbw, ref harmonicsFrequency);

         for (int i = 0; i < NumberOfHarmonics; i++)
         {
            harmonicString = RFmxSpecAnMX.BuildHarmonicString2("", i);
            specAn.Harm.Results.FetchHarmonicPowerTrace(harmonicString, timeout, ref power[i]);
         }
      }

      private void PrintResults()
      {
         Console.WriteLine("Total Harmonic Distortion (%)   : {0}", totalHarmonicDistortion);
         Console.WriteLine("Average Fundamental Power (dBm) : {0}", averageFundamentalPower);
         Console.WriteLine("Fundamental Frequency (Hz)      : {0}", fundamentalFrequency);

         Console.WriteLine("\n-----------------Harmonics----------------------\n");
         for (int i = 0; i < NumberOfHarmonics; i++)
         {
            Console.WriteLine("Harmonic {0}", i + 1);
            Console.WriteLine("Harmonics Frequency    (Hz)  : {0}", harmonicsFrequency[i]);
            Console.WriteLine("Harmonics RBW          (Hz)  : {0}", harmonicsRbw[i]);
            Console.WriteLine("Average Absolute Power (dBm) : {0}", averageAbsolutePower[i]);
            Console.WriteLine("Average Relative Power (dB)  : {0}", averageRelativePower[i]);
            Console.WriteLine("---------------------------------------------\n");
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
