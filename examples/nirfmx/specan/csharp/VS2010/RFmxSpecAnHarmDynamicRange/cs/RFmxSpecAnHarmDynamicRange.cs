//Steps:
//1. Open a new RFmx session.
//2. Configure the instrument properties Clock Source and Clock Frequency.
//3. Configure Selected Ports.
//4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Select Harmonics measurement and enable the traces.
//6. Configure RBW Filter parameters.
//7. Configure Measurement Interval of the Fundamental.
//8. Configure Auto Harmonics setup.
//9A. If Harmonics Setup is Auto, configure Number of Harmonics.
//9B. If Harmonics Setup is Manual, configure Order, BW and Measurement Interval for each Harmonic using Selector String.
//10. Configure Measurement Method and Noise Compensation Enabled.
//11. Configure Averaging parameters.
//12. Initiate Harmonics Measurment.
//13. Fetch Total Harmonic Distortion[THD].
//14. Fetch Harmonic Measurement results and Power Trace for all the Harmonics using Selector String.
//15. Close the RFmx Session.

using System;
using NationalInstruments;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnHarmDynamicRange
{
   public class RFmxSpecAnHarmDynamicRange
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      const int NumberOfHarmonics = 3;
      string resourceName, frequencySource, harmonicString;
      string selectedPorts;
      double referenceLevel, externalAttenuation, centerFrequency, frequency, rbw,
             rrcAlpha, measurementInterval, timeout;
      RFmxSpecAnMXHarmRbwFilterType rbwFilterType;
      RFmxSpecAnMXHarmAutoHarmonicsSetupEnabled autoHarmonicsSetup;
      RFmxSpecAnMXHarmMeasurementMethod measurementMethod;
      RFmxSpecAnMXHarmNoiseCompensationEnabled noiseCompensationEnabled;
      RFmxSpecAnMXHarmAveragingEnabled averagingEnabled;
      RFmxSpecAnMXHarmAveragingType averagingType;
      int averagingCount;

      RFmxSpecAnMXHarmHarmonicEnabled[] harmEnabled = new RFmxSpecAnMXHarmHarmonicEnabled[NumberOfHarmonics];
      int[] harmOrder = new int[NumberOfHarmonics];
      double[] harmBandwidth = new double[NumberOfHarmonics];
      double[] harmMeasurementInterval = new double[NumberOfHarmonics];

      double totalHarmonicDistortion, averageFundamentalPower, fundamentalFrequency;
      double[] harmAverageRelativePower = new double[NumberOfHarmonics];
      double[] harmAverageAbsolutePower = new double[NumberOfHarmonics];
      double[] harmRbw = new double[NumberOfHarmonics];
      double[] harmFrequency = new double[NumberOfHarmonics];

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
         /* Initialize input variables */

         resourceName = "RFSA";

         selectedPorts = "";

         centerFrequency = 1.0e+9;                                   /* Hz */
         referenceLevel = 0.00;                                      /* dBm */
         externalAttenuation = 0.00;                                 /* dB */

         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10.0e+6;                                        /* Hz */

         rbwFilterType = RFmxSpecAnMXHarmRbwFilterType.Gaussian;
         rbw = 100e+3;                                               /* Hz */
         rrcAlpha = 0.010;
         measurementInterval = 1.0e-3;                               /* seconds */

         autoHarmonicsSetup = RFmxSpecAnMXHarmAutoHarmonicsSetupEnabled.True;

         for (int i = 0; i < NumberOfHarmonics; i++)
         {
            harmEnabled[i] = RFmxSpecAnMXHarmHarmonicEnabled.True;
            harmOrder[i] = i + 1;
            harmBandwidth[i] = 100e3;                                /* Hz */
            harmMeasurementInterval[i] = 1e-3;                       /* seconds */
         }

         measurementMethod = RFmxSpecAnMXHarmMeasurementMethod.DynamicRange;
         noiseCompensationEnabled = RFmxSpecAnMXHarmNoiseCompensationEnabled.True;

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
         specAn.ConfigureFrequency("", centerFrequency);
         specAn.ConfigureReferenceLevel("", referenceLevel);
         specAn.ConfigureExternalAttenuation("", externalAttenuation);
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
            specAn.Harm.Configuration.ConfigureHarmonicArray("", harmOrder, harmBandwidth, harmEnabled,
               harmMeasurementInterval);
         }
         specAn.Harm.Configuration.SetMeasurementMethod("", measurementMethod);
         specAn.Harm.Configuration.SetNoiseCompensationEnabled("", noiseCompensationEnabled);
         specAn.Harm.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         specAn.Initiate("", "");
      }

      private void RetreiveResults()
      {
         specAn.Harm.Results.FetchTotalHarmonicDistortion("", timeout, out totalHarmonicDistortion,
            out averageFundamentalPower, out fundamentalFrequency);

         for (int i = 0; i < NumberOfHarmonics; i++)
         {
            harmonicString = RFmxSpecAnMX.BuildHarmonicString2("", i);
            specAn.Harm.Results.FetchHarmonicPowerTrace(harmonicString, timeout, ref power[i]);
         }

         specAn.Harm.Results.FetchHarmonicMeasurementArray("", timeout, ref harmAverageRelativePower,
            ref harmAverageAbsolutePower, ref harmRbw, ref harmFrequency);
      }

      private void PrintResults()
      {
         Console.WriteLine("Measurement\n");
         Console.WriteLine("Total Harmonic Distoration (%)     : {0}", totalHarmonicDistortion);
         Console.WriteLine("Average Fundamental Power (dBm)    : {0}", averageFundamentalPower);
         Console.WriteLine("Fundamental Frequency (Hz)         : {0}", fundamentalFrequency);

         Console.WriteLine("\n----------------Harmonics----------------------\n");
         for (int i = 0; i < NumberOfHarmonics; i++)
         {
            Console.WriteLine("Harmonic {0}:", i + 1);
            Console.WriteLine("Harmonics Frequency    (Hz)       : {0}", harmFrequency[i]);
            Console.WriteLine("Harmonics RBW          (Hz)       : {0}", harmRbw[i]);
            Console.WriteLine("Average Absolute Power (dBm)      : {0}", harmAverageAbsolutePower[i]);
            Console.WriteLine("Average Relative Power (dB)       : {0}", harmAverageRelativePower[i]);
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

      static private void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }
   }
}
