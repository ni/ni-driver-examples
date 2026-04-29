//Steps:
//1. Open a new RFmx session
//2.  Configure the basic instrument properties (Clock Source and Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Reference Level and External Attenuation)
//6. Configure TXP measurement and enable the traces
//7. Configure TXP Measurement Interval
//8. Configure TXP RBW Filter
//9. Configure TXP Averaging
//10. Create a Queue for producer-consumer setup
//11. Producer loop - Configure frequency step, Initiate Measurement, enqueue result name and wait for the acquisiton to be complete
//12. Consumer loop - Dequeue result name and Fetch TXP reults
//13. Release Queue
//14. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;
using System.Collections.Concurrent;
using System.Threading;


namespace NationalInstruments.Examples.RFmxSpecAnTxpMultipleResultNames
{
   public class RFmxSpecAnTxpMultipleResultNames
   {
      const int numberOfMeasurements = 2;
      const double timeout = 10.0;             /* seconds */
      struct MeasurementResults
      {
         public double[] averageMeanPower, peakToAverageRatio, maximumPower, minimumPower;
         public MeasurementResults(int numberOfMeasurements)
         {
            averageMeanPower = new double[numberOfMeasurements];
            peakToAverageRatio = new double[numberOfMeasurements];
            maximumPower = new double[numberOfMeasurements];
            minimumPower = new double[numberOfMeasurements];
         }
      }

      string frequencySource;
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      double referenceLevel, externalAttenuation;
      double measurementInterval, rbw, rrcAlpha, frequency, centerFrequency;
      string resourceName;
      string selectedPorts;
      RFmxSpecAnMXTxpAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxSpecAnMXTxpAveragingType averagingType;
      RFmxSpecAnMXTxpRbwFilterType rbwFilterType;
      bool errorFlag;

      MeasurementResults TxpMeasurements = new MeasurementResults(numberOfMeasurements);

      BlockingCollection<string> taskQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());

      public void Run()
      {
         try
         {
            InitializeVariables();
            InitializeInstr();
            ConfigureSpecAn();
            BeginProducerConsumerLoop();
            PrintResults();
            CloseSession();

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
         referenceLevel = 0.00;                  /* dBm */
         externalAttenuation = 0.00;             /* dB */
         selectedPorts = "";
         centerFrequency = 1E+9;                 /* Hz */

         measurementInterval = 1e-3;             /* seconds */
         rbw = 100e+3;                           /*  Hz */

         averagingEnabled = RFmxSpecAnMXTxpAveragingEnabled.False;
         averagingType = RFmxSpecAnMXTxpAveragingType.Rms;
         averagingCount = 10;

         frequencySource = RFmxInstrMXConstants.OnboardClock;
         frequency = 10e+6;

         rbwFilterType = RFmxSpecAnMXTxpRbwFilterType.Gaussian;
         rrcAlpha = 0.010;

         errorFlag = false;
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
         specAn.ConfigureReferenceLevel("", referenceLevel);
         specAn.ConfigureExternalAttenuation("", externalAttenuation);

         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Txp, true);
         specAn.Txp.Configuration.ConfigureMeasurementInterval("", measurementInterval);
         specAn.Txp.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha);
         specAn.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
      }

      private void BeginProducerConsumerLoop()
      {
         Thread FetchResultsThread = new Thread(new ThreadStart(FetchResults));
         FetchResultsThread.Start();//Start a new thread for Consumer loop
         ConfigureAndInitiate(); //Start Producer loop
         FetchResultsThread.Join();
         //Producer and Consumer have finished at this point.
      }

      public void ConfigureAndInitiate()
      {
         try
         {
            for (int i = 0; i < numberOfMeasurements; i++)
            {
               double offset = i * 1000000;
               specAn.ConfigureFrequency("", centerFrequency + offset);
               string resultString = RFmxSpecAnMX.BuildResultString("TXP_Result" + i);
               specAn.Initiate("", resultString);
               taskQueue.Add(resultString);
               instrSession.WaitForAcquisitionComplete(timeout);
            }
         }
         catch (Exception ex)
         {
            errorFlag = true;
            Console.WriteLine("ERROR in ConfigureAndInitiate thread:\n" + ex.GetType() + ": " + ex.Message + "\n\n");
         }
      }

      public void FetchResults()
      {
         try
         {
            int queueTimeout = 10000; /*milliseconds*/
            for (int i = 0; i < numberOfMeasurements; i++)
            {
               string resultString;
               if (taskQueue.TryTake(out resultString, queueTimeout))
                  specAn.Txp.Results.FetchMeasurement(resultString, timeout, out TxpMeasurements.averageMeanPower[i],
                                                      out TxpMeasurements.peakToAverageRatio[i],
                                                      out TxpMeasurements.maximumPower[i], out TxpMeasurements.minimumPower[i]);
               else throw new Exception("Could not fetch the result string as dequeue operation timed out");
            }
         }
         catch (Exception ex)
         {
            errorFlag = true;
            Console.WriteLine("ERROR  in FetchResults thread:\n" + ex.GetType() + ": " + ex.Message);
         }
      }

      private void PrintResults()
      {
         if (!errorFlag)
         {
            for (int i = 0; i < numberOfMeasurements; i++)
            {
               Console.WriteLine("-------------------Measurement" + (i + 1) + "---------------------\n");
               Console.WriteLine("Average Mean Frequency (Hz)    {0}\n", TxpMeasurements.averageMeanPower[i]);
               Console.WriteLine("Peak to Average Ratio (dB)     {0}\n", TxpMeasurements.peakToAverageRatio[i]);
               Console.WriteLine("Maximum Power (dBm)            {0}\n", TxpMeasurements.maximumPower[i]);
               Console.WriteLine("Minimum Power (dBm)            {0}\n", TxpMeasurements.minimumPower[i]);
            }
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
