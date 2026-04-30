//Steps :
//RFmxSpecAn IM Example
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the Center Frequency
//4. Configure the basic instrument properties (Clock Source, Clock Frequency)
//5. Configure the basic signal properties  (Reference Level, External Attenuation and RF Attenuation)
//6. Select IM measurement and enable the traces
//7. Configure Averaging
//8. Configure RBW Filter parameters
//9. Configure Sweep Time
//10. Configure FFT
//11. Configure Frequency definition
//12. Configure Measurement Method
//13. Configure Fundamental tones
//14. Configure Auto Intermods Setup Enabled
//15. Configure Number of Intermods
//16. Configure Intermod (Array)
//17. Initiate Measurement
//18. Fetch IM Measurements and Trace
//19. Close RFmx Session


using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnIMAdvanced
{

   class RFmxSpecAnIMAdvanced
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      const int numberOfIntermods = 1;
      string selectedPorts;
      double centerFrequency, frequency, referenceLevel, externalAttenuation, rfAttenuation, rbw;
      double sweepTimeInterval, fftPadding, lowerToneFrequency, upperToneFrequency, timeout;
      double lowerTonePower, upperTonePower;
      string resourceName, frequencySource;
      private bool enableAllTraces;
      RFmxInstrMXRFAttenuationAuto rfAttenuationAuto;

      RFmxSpecAnMXIMAveragingEnabled averagingEnabled;
      RFmxSpecAnMXIMAveragingType averagingType;
      int averagingCount;

      RFmxSpecAnMXIMRbwFilterAutoBandwidth rbwAuto;
      RFmxSpecAnMXIMRbwFilterType rbwFilterType;
      RFmxSpecAnMXIMFrequencyDefinition frequencyDefinition;

      RFmxSpecAnMXIMSweepTimeAuto sweepTimeAuto;
      RFmxSpecAnMXIMFftWindow fftWindow;
      RFmxSpecAnMXIMMeasurementMethod measurementMethod;

      RFmxSpecAnMXIMAutoIntermodsSetupEnabled autoIntermodsSetupEnabled;
      int maximumIntermodOrder;
      int actualNumberOfIntermods;

      int[] order = new int[numberOfIntermods];
      RFmxSpecAnMXIMIntermodSide[] side = new RFmxSpecAnMXIMIntermodSide[numberOfIntermods];
      RFmxSpecAnMXIMIntermodEnabled[] enabled = new RFmxSpecAnMXIMIntermodEnabled[numberOfIntermods];
      double[] lowerIntermodFrequency = new double[numberOfIntermods];
      double[] upperIntermodFrequency = new double[numberOfIntermods];

      int[] intermodOrder = new int[numberOfIntermods];
      double[] lowerIntermodPower = new double[numberOfIntermods];
      double[] upperIntermodPower = new double[numberOfIntermods];
      double[] worstCaseOutputInterceptPower = new double[numberOfIntermods];
      double[] lowerOutputInterceptPower = new double[numberOfIntermods];
      double[] upperOutputInterceptPower = new double[numberOfIntermods];

      Spectrum<float>[] spectrum;
      int numberOfSpectrums;

      internal void Run()
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
         centerFrequency = 1e+9;                             /* Hz */
         referenceLevel = 0.00;                              /* dBm */
         externalAttenuation = 0.00;                         /* dB */
         timeout = 10.00;                                     /* seconds */

         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10.0e+6;                                /* Hz */

         rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.True;
         rfAttenuation = 10.00;                              /* dB */

         enableAllTraces = true;

         //Averaging 
         averagingEnabled = RFmxSpecAnMXIMAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxSpecAnMXIMAveragingType.Rms;

         // RBW Filter
         rbwFilterType = RFmxSpecAnMXIMRbwFilterType.Gaussian;
         rbwAuto = RFmxSpecAnMXIMRbwFilterAutoBandwidth.True;
         rbw = 10.0e+3;                                  /* Hz */

         // Sweep Time
         sweepTimeAuto = RFmxSpecAnMXIMSweepTimeAuto.True;
         sweepTimeInterval = 1.00e-3;                    /* seconds */

         // FFT
         fftWindow = RFmxSpecAnMXIMFftWindow.FlatTop;
         fftPadding = -1.0;

         //Frequency Definition
         frequencyDefinition = RFmxSpecAnMXIMFrequencyDefinition.Relative;

         //Measurement Method
         measurementMethod = RFmxSpecAnMXIMMeasurementMethod.Normal;

         //Fundamental Tones
         lowerToneFrequency = -1.00e+6;                      /* Hz */
         upperToneFrequency = 1.00e+6;                       /* Hz */

         //Auto Intermods Setup
         autoIntermodsSetupEnabled = RFmxSpecAnMXIMAutoIntermodsSetupEnabled.True;
         maximumIntermodOrder = 3;

         //Intermods
         for (int i = 0; i < numberOfIntermods; i++)
         {
            enabled[i] = RFmxSpecAnMXIMIntermodEnabled.True;
            order[i] = 3;
            side[i] = RFmxSpecAnMXIMIntermodSide.Both;
            lowerIntermodFrequency[i] = -3.00e+6;                      /* Hz */
            upperIntermodFrequency[i] = 3.00e+6;                       /* Hz */
         }
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

         specAn.ConfigureFrequency("", centerFrequency);
         instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
         specAn.SetSelectedPorts("", selectedPorts);
         specAn.ConfigureReferenceLevel("", referenceLevel);
         specAn.ConfigureExternalAttenuation("", externalAttenuation);
         instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.IM, enableAllTraces);

         specAn.IM.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         specAn.IM.Configuration.ConfigureRbwFilter("", rbwAuto, rbw, rbwFilterType);
         specAn.IM.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         specAn.IM.Configuration.ConfigureFft("", fftWindow, fftPadding);
         specAn.IM.Configuration.ConfigureFrequencyDefinition("", frequencyDefinition);
         specAn.IM.Configuration.ConfigureMeasurementMethod("", measurementMethod);
         specAn.IM.Configuration.ConfigureFundamentalTones("", lowerToneFrequency, upperToneFrequency);
         specAn.IM.Configuration.ConfigureAutoIntermodsSetup("", autoIntermodsSetupEnabled, maximumIntermodOrder);

         if (autoIntermodsSetupEnabled == RFmxSpecAnMXIMAutoIntermodsSetupEnabled.False)
         {
            specAn.IM.Configuration.ConfigureNumberOfIntermods("", numberOfIntermods);

            specAn.IM.Configuration.ConfigureIntermodArray("", order, lowerIntermodFrequency,
                     upperIntermodFrequency, side, enabled);
         }
         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */
         specAn.IM.Configuration.GetNumberOfIntermods("", out actualNumberOfIntermods);
         if (measurementMethod == RFmxSpecAnMXIMMeasurementMethod.Normal)
            numberOfSpectrums = 1;
         else
            numberOfSpectrums = 2 * actualNumberOfIntermods + 2;
         spectrum = new Spectrum<float>[numberOfSpectrums];

         specAn.IM.Results.FetchFundamentalMeasurement("", timeout, out lowerTonePower, out upperTonePower);
         specAn.IM.Results.FetchIntermodMeasurementArray("", timeout, ref intermodOrder, ref lowerIntermodPower, ref upperIntermodPower);
         specAn.IM.Results.FetchInterceptPowerArray("", timeout, ref intermodOrder, ref worstCaseOutputInterceptPower,
             ref lowerOutputInterceptPower, ref upperOutputInterceptPower);
         for (int spectrumIndex = 0; spectrumIndex < numberOfSpectrums; spectrumIndex++)
         {
            specAn.IM.Results.FetchSpectrum("", timeout, spectrumIndex, ref spectrum[spectrumIndex]);
         }
      }

      private void PrintResults()
      {
         /* Display the results */
         Console.WriteLine("Fundamental Tone Measurement              \n");
         Console.WriteLine("Lower Tone Power(dBm)                    :{0}", lowerTonePower);
         Console.WriteLine("Upper Tone Power(dBm)                    :{0}", upperTonePower);

         Console.WriteLine("\nIntermod Measurements                     \n");

         for (int i = 0; i < actualNumberOfIntermods; i++)
         {
            Console.WriteLine("\nIntermod Measurement                     : {0}", i);
            Console.WriteLine("Order                                    : {0}", intermodOrder[i]);
            Console.WriteLine("Lower Intermod Power(dBm)                :{0}", lowerIntermodPower[i]);
            Console.WriteLine("Upper Intermod Power(dBm)                :{0}", upperIntermodPower[i]);
            Console.WriteLine("Lower Output Intercept Power(dBm)        :{0}", lowerOutputInterceptPower[i]);
            Console.WriteLine("Upper Output Intercept Power(dBm)        :{0}", upperOutputInterceptPower[i]);
            Console.WriteLine("Worst Case Output Intercept Power(dBm)   :{0}", worstCaseOutputInterceptPower[i]);
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
