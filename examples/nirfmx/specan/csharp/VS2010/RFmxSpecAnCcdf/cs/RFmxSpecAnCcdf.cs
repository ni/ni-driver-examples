//Steps:
//1. Open a new RFmx session
//2. Configure the instrument properties Clock Source and Clock Frequency
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select CCDF measurement and enable the traces
//6. Configure CCDF Number of Records and Measurement Interval
//7. Configure CCDF RBW filter
//8. Configure CCDF Threshold
//9. Initiate Measurement
//10. Fetch CCDF Measurements and Traces
//11. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnCcdf
{
   class RFmxSpecAnCcdf
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      string resourceName, frequencySource;
      string selectedPorts;
      double centerFrequency, referenceLevel, externalAttenuation, measurementInterval,
             rbw, rrcAlpha, thresholdLevel, frequency;
      int measuredSamplesCount, numOfRecords;
      bool enableAllTraces;
      RFmxSpecAnMXCcdfRbwFilterType rbwFilterType;
      RFmxSpecAnMXCcdfThresholdEnabled thresholdEnabled;
      RFmxSpecAnMXCcdfThresholdType thresholdType;

      double tenPercentPower, onePercentPower, oneTenthPercentPower, oneHundredthPercentPower,
             oneThousandthPercentPower, oneTenThousandthPercentPower, meanPower,
             meanPowerPercentile, peakPower, timeout;
      AnalogWaveform<float> gaussianProbabilitiesWaveform, probabilitiesWaveform;

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
         centerFrequency = 1e+9;             /* Hz */
         referenceLevel = 0.00;              /* dBm */
         externalAttenuation = 0.00;         /* dB */

         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10.0e+6;                /* Hz */

         numOfRecords = 1;
         measurementInterval = 1.0e-3;       /* seconds */

         rbwFilterType = RFmxSpecAnMXCcdfRbwFilterType.None;
         rbw = 100.0e+3;                     /* Hz */
         rrcAlpha = 0.010;

         thresholdEnabled = RFmxSpecAnMXCcdfThresholdEnabled.False;
         thresholdType = RFmxSpecAnMXCcdfThresholdType.Relative;
         thresholdLevel = -20.00;            /* dB or dBm*/

         enableAllTraces = true;
         timeout = 10.0;                     /* seconds */
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
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Ccdf, enableAllTraces);
         specAn.Ccdf.Configuration.ConfigureNumberOfRecords("", numOfRecords);
         specAn.Ccdf.Configuration.ConfigureMeasurementInterval("", measurementInterval);
         specAn.Ccdf.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha);
         specAn.Ccdf.Configuration.ConfigureThreshold("", thresholdEnabled, thresholdLevel, thresholdType);
         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         specAn.Ccdf.Results.FetchPower("", timeout, out meanPower, out meanPowerPercentile,
                                        out peakPower, out measuredSamplesCount);
         specAn.Ccdf.Results.FetchBasicPowerProbabilities("", timeout, out tenPercentPower,
                                                          out onePercentPower,
                                                          out oneTenthPercentPower,
                                                          out oneHundredthPercentPower,
                                                          out oneThousandthPercentPower,
                                                          out oneTenThousandthPercentPower);
         specAn.Ccdf.Results.FetchGaussianProbabilitiesTrace("", timeout, ref gaussianProbabilitiesWaveform);
         specAn.Ccdf.Results.FetchProbabilitiesTrace("", timeout, ref probabilitiesWaveform);
      }

      private void PrintResults()
      {
         Console.WriteLine("--------------Power----------------------\n");
         Console.WriteLine(" Mean Power (dBm)           {0}", meanPower);
         Console.WriteLine(" Mean Power Percentile (%)  {0}", meanPowerPercentile);
         Console.WriteLine(" Peak Power (dB)            {0}", peakPower);
         Console.WriteLine(" Measured Samples Count     {0}", measuredSamplesCount);

         Console.WriteLine("--------------Power Probabilities-------------\n");
         Console.WriteLine(" 10 % Power (dB)            {0}", tenPercentPower);
         Console.WriteLine(" 1 % Power (dB)             {0}", onePercentPower);
         Console.WriteLine(" 0.1 % Power (dB)           {0}", oneTenthPercentPower);
         Console.WriteLine(" 0.01 % Power (dB)          {0}", oneHundredthPercentPower);
         Console.WriteLine(" 0.001 % Power (dB)         {0}", oneThousandthPercentPower);
         Console.WriteLine(" 0.0001 % Power (dB)        {0}", oneTenThousandthPercentPower);
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
