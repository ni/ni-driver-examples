//Steps:
//1. Open a new RFmx session
//2. Configure the instrument properties Clock Source and Clock Frequency
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select CHP measurement and enable the traces
//6. Configure CHP Integration BW, Span and Sweep Time
//7. Configure CHP Averaging
//8. Configure CHP RBW filter
//9. Configure CHP FFT
//10. Configure CHP RRC Filter
//11. Initiate Measurement
//12. Fetch CHP Measurements and Traces
//13. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnChpAdvanced
{
   class RFmxSpecAnChpAdvanced
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      string resourceName, frequencySource;
      string selectedPorts;
      double centerFrequency, referenceLevel, externalAttenuation, frequency,
             span, rbw, sweepTimeInterval, fftPadding, timeout;
      bool enableAllTraces;
      RFmxSpecAnMXChpRbwFilterType rbwFilterType;
      RFmxSpecAnMXChpRbwAutoBandwidth rbwAuto;
      RFmxSpecAnMXChpSweepTimeAuto sweepTimeAuto;
      RFmxSpecAnMXChpAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxSpecAnMXChpAveragingType averagingType;
      RFmxSpecAnMXChpFftWindow fftWindow;

      const int NumberOfCarriers = 1;

      double totalCarrierPower = 0.0;

      struct CarrierChannel
      {
         public double carrierFrequency, integrationBandwidth, rrcFilterAlpha;
         public RFmxSpecAnMXChpCarrierRrcFilterEnabled rrcFilterEnabled;
      };

      struct CarrierMeasurement
      {
         public double absolutePower;
         public double psd;
         public double relativePower;
      };

      CarrierChannel[] carrierChannel = new CarrierChannel[NumberOfCarriers];
      CarrierMeasurement[] carrierMeasuremnt = new CarrierMeasurement[NumberOfCarriers];

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
         centerFrequency = 1e+9;         /* Hz */
         referenceLevel = 0.00;          /* dBm */
         externalAttenuation = 0.00;     /* dB */
         timeout = 10.0;                 /* seconds */

         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10.0e+6;            /* Hz */

         span = 1.0e+6;                  /* Hz */

         //Sweep time
         sweepTimeAuto = RFmxSpecAnMXChpSweepTimeAuto.True;
         sweepTimeInterval = 1.00e-3;    /* seconds */

         //Averaging
         averagingEnabled = RFmxSpecAnMXChpAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxSpecAnMXChpAveragingType.Rms;

         //RBW Filter
         rbwFilterType = RFmxSpecAnMXChpRbwFilterType.Gaussian;
         rbwAuto = RFmxSpecAnMXChpRbwAutoBandwidth.True;
         rbw = 10.0e+3;                  /* Hz */

         //FFT
         fftWindow = RFmxSpecAnMXChpFftWindow.FlatTop;
         fftPadding = -1.0;

         enableAllTraces = true;

         //Set up the carrier channel inputs
         for (int i = 0; i < NumberOfCarriers; i++)
         {
            carrierChannel[i].carrierFrequency = 0.00;          /* Hz */
            carrierChannel[i].integrationBandwidth = 1.0e+6;    /* Hz */
            carrierChannel[i].rrcFilterEnabled = RFmxSpecAnMXChpCarrierRrcFilterEnabled.False;
            carrierChannel[i].rrcFilterAlpha = 0.220;
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

         /* Configure measurement */
         instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
         specAn.SetSelectedPorts("", selectedPorts);
         specAn.ConfigureFrequency("", centerFrequency);
         specAn.ConfigureReferenceLevel("", referenceLevel);
         specAn.ConfigureExternalAttenuation("", externalAttenuation);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Chp, enableAllTraces);
         specAn.Chp.Configuration.ConfigureSpan("", span);
         specAn.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         specAn.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                     averagingType);
         specAn.Chp.Configuration.ConfigureRbwFilter("", rbwAuto, rbw, rbwFilterType);
         specAn.Chp.Configuration.ConfigureFft("", fftWindow, fftPadding);
         specAn.Chp.Configuration.ConfigureNumberOfCarriers("", NumberOfCarriers);

         string selectorString;
         for (int i = 0; i < NumberOfCarriers; i++)
         {
            selectorString = RFmxSpecAnMX.BuildCarrierString2("", i);
            specAn.Chp.Configuration.ConfigureCarrierOffset(selectorString, carrierChannel[i].carrierFrequency);
            specAn.Chp.Configuration.ConfigureIntegrationBandwidth(selectorString, carrierChannel[i].integrationBandwidth);
            specAn.Chp.Configuration.ConfigureRrcFilter(selectorString, carrierChannel[i].rrcFilterEnabled,
                                                        carrierChannel[i].rrcFilterAlpha);
         }

         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */
         Spectrum<float> spectrum = null;
         specAn.Chp.Results.FetchSpectrum("", timeout, ref spectrum);
         specAn.Chp.Results.FetchTotalCarrierPower("", timeout, out totalCarrierPower);

         string selectorString;
         for (int i = 0; i < NumberOfCarriers; i++)
         {
            selectorString = RFmxSpecAnMX.BuildCarrierString2("", i);
            specAn.Chp.Results.FetchCarrierMeasurement(selectorString, timeout, out carrierMeasuremnt[i].absolutePower,
                                                       out carrierMeasuremnt[i].psd, out carrierMeasuremnt[i].relativePower);
         }
      }

      private void PrintResults()
      {
         Console.WriteLine("Total Carrier Power (dBm) {0}", totalCarrierPower);
         Console.WriteLine("\nCarrier Measurements");
         for (int i = 0; i < NumberOfCarriers; i++)
         {
            Console.WriteLine("\nCarrier : {0}", i);
            Console.WriteLine("Absolute power (dBm)     {0}", carrierMeasuremnt[i].absolutePower);
            Console.WriteLine("PSD (dBm/Hz)             {0}", carrierMeasuremnt[i].psd);
            Console.WriteLine("Relative Power (dB)      {0}", carrierMeasuremnt[i].relativePower);
            Console.WriteLine("-----------------------------------------------------------");
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
