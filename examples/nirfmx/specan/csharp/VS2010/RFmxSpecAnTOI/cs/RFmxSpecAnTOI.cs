// Steps :
//RFmxSpecAn IM TOI Example
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
//11. Configure Measurement Method
//12. Configure Fundamental tones
//13. Configure auto setup of third order intermod frequencies
//14. Initiate Measurement
//15. Fetch IM Measurements and Trace
//16. Close RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnTOI
{
   class RFmxSpecAnTOI
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      const int maximumNumberOfSpectrums = 4;
      string selectedPorts;
      double centerFrequency, frequency, referenceLevel, externalAttenuation, rfAttenuation, rbw;
      double sweepTimeInterval, fftPadding, lowerToneFrequency, upperToneFrequency, timeout;
      double lowerTonePower, upperTonePower, lowerIntermodPower, upperIntermodPower,
          worstCaseOutputInterceptPower, lowerOutputInterceptPower, upperOutputInterceptPower;
      string resourceName, frequencySource;
      private bool enableAllTraces;
      RFmxInstrMXRFAttenuationAuto rfAttenuationAuto;

      RFmxSpecAnMXIMAveragingEnabled averagingEnabled;
      RFmxSpecAnMXIMAveragingType averagingType;
      int averagingCount, intermodOrder;

      RFmxSpecAnMXIMRbwFilterAutoBandwidth rbwAuto;
      RFmxSpecAnMXIMRbwFilterType rbwFilterType;

      RFmxSpecAnMXIMSweepTimeAuto sweepTimeAuto;
      RFmxSpecAnMXIMFftWindow fftWindow;
      RFmxSpecAnMXIMMeasurementMethod measurementMethod;

      RFmxSpecAnMXIMAutoIntermodsSetupEnabled autoIntermodsSetupEnabled;

      Spectrum<float>[] spectrum = null;
      int numberOfSpectrums;
      int maximumIntermodOrder;


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

         //Measurement Method
         measurementMethod = RFmxSpecAnMXIMMeasurementMethod.Normal;

         //Fundamental Tones
         lowerToneFrequency = -1.00e+6;                      /* Hz */
         upperToneFrequency = 1.00e+6;                       /* Hz */

         //Auto Intermods Setup
         autoIntermodsSetupEnabled = RFmxSpecAnMXIMAutoIntermodsSetupEnabled.True;
         maximumIntermodOrder = 3;

         if (measurementMethod == RFmxSpecAnMXIMMeasurementMethod.Normal)
            numberOfSpectrums = 1;
         else
            numberOfSpectrums = maximumNumberOfSpectrums;
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
         specAn.IM.Configuration.ConfigureMeasurementMethod("", measurementMethod);
         specAn.IM.Configuration.ConfigureFundamentalTones("", lowerToneFrequency, upperToneFrequency);
         specAn.IM.Configuration.ConfigureAutoIntermodsSetup("", autoIntermodsSetupEnabled, maximumIntermodOrder);

         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         specAn.IM.Results.FetchFundamentalMeasurement("", timeout, out lowerTonePower, out upperTonePower);
         specAn.IM.Results.FetchIntermodMeasurement("", timeout, out intermodOrder, out lowerIntermodPower,
             out upperIntermodPower);
         specAn.IM.Results.FetchInterceptPower("", timeout, out intermodOrder, out worstCaseOutputInterceptPower,
             out lowerOutputInterceptPower, out upperOutputInterceptPower);
         spectrum = new Spectrum<float>[numberOfSpectrums];
         for (int spectrumIndex = 0; spectrumIndex < numberOfSpectrums; spectrumIndex++)
         {
            specAn.IM.Results.FetchSpectrum("", timeout, spectrumIndex, ref spectrum[spectrumIndex]);
         }
      }

      private void PrintResults()
      {
         /* Display the results */
         Console.WriteLine("Fundamental Tone Measurement            \n");
         Console.WriteLine("Lower Tone Power(dBm)      :{0}", lowerTonePower);
         Console.WriteLine("Upper Tone Power(dBm)      :{0}", upperTonePower);

         Console.WriteLine("\nIntermod Measurement                  \n");
         Console.WriteLine("Lower Intermod Power(dBm)  :{0}", lowerIntermodPower);
         Console.WriteLine("Upper Intermod Power(dBm)  :{0}", upperIntermodPower);
         Console.WriteLine("Lower TOI(dBm)             :{0}", lowerOutputInterceptPower);
         Console.WriteLine("Upper TOI(dBm)             :{0}", upperOutputInterceptPower);
         Console.WriteLine("Worst Case TOI(dBm)        :{0}", worstCaseOutputInterceptPower);
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
