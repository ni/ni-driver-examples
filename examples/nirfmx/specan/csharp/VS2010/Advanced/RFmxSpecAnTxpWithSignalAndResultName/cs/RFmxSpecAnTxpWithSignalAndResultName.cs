//Steps:
//1. Open a new RFmx session
//2. Create a Named signal
//3. Configure the basic instrument properties (Clock Source and Clock Frequency)
//4. Configure Selected Ports
//5. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation)
//6. Configure TXP measurement and enable the traces
//7. Configure the Measurement Interval
//8. Configure RBW filter parameters
//9. Configure Averaging parameters
//10. Initiate Measurement
//11. Fetch TXP Traces and Measurements
//12. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnTxpWithSignalAndResultName
{
   public class RFmxSpecAnTxpWithSignalAndResultName
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      string resourceName;

      const int NumberOfOffsets = 2;

      string selectedPorts;
      double centerFrequency, referenceLevel, externalAttenuation,
             rbw, frequency, measurementInterval,
             timeout, rrcAlpha;
      string frequencySource;

      int averagingCount;
      RFmxSpecAnMXTxpAveragingEnabled averagingEnabled;
      RFmxSpecAnMXTxpAveragingType averagingType;

      RFmxSpecAnMXTxpRbwFilterType RBWFilterType;

      //Output values
      double averageMeanPower;
      double peakToAverageRatio;
      double maxPower;
      double minPower;
      string resultName;

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

         measurementInterval = 1e-3;        /* seconds */

         // RBW Filter
         RBWFilterType = RFmxSpecAnMXTxpRbwFilterType.Gaussian;
         rbw = 100.0e+3;                       /* Hz */
         rrcAlpha = 0.010;

         //Averaging 
         averagingEnabled = RFmxSpecAnMXTxpAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxSpecAnMXTxpAveragingType.Rms;

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
         specAn = instrSession.GetSpecAnSignalConfiguration("TxP_Signal");

         /* Configure measurement */
         instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
         specAn.SetSelectedPorts("", selectedPorts);
         specAn.ConfigureExternalAttenuation("", externalAttenuation);
         specAn.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Txp, true);

         specAn.Txp.Configuration.ConfigureMeasurementInterval("", measurementInterval);
         specAn.Txp.Configuration.ConfigureRbwFilter("", rbw, RBWFilterType, rrcAlpha);
         specAn.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         resultName = RFmxSpecAnMX.BuildResultString("TxP_Result");
         specAn.Initiate("", resultName);
      }

      private void RetrieveResults()
      {
         /* Retrieve results */
         AnalogWaveform<float> power = null;
         specAn.Txp.Results.FetchPowerTrace(resultName, timeout, ref power);
         specAn.Txp.Results.FetchMeasurement(resultName, timeout, out averageMeanPower, out peakToAverageRatio,
                                             out maxPower, out minPower);
      }

      private void PrintResults()
      {
         Console.WriteLine("Average Mean Frequency (Hz)    {0}", averageMeanPower);
         Console.WriteLine("Mean Phase (deg)               {0}", peakToAverageRatio);
         Console.WriteLine("Maximum Power (dBm)            {0}", maxPower);
         Console.WriteLine("Minimum Power (dBm)            {0}", minPower);
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
