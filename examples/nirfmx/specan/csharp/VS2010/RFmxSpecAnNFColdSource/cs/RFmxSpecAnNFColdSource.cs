//Steps:
//1. Open a new RFmx session.
//2. Configure Selected Ports.
//3. Select NF measurement.
//4. Configure Measurement Method.
//5. Configure measurement frequencies
//       5.1. Specify Start Frequency, Stop Frequency and  Frequency Step Size.
//       5.2. Specify Start Frequency, Stop Frequency and Frequency Points.
//       5.3. Specify Frequency List.
//6. Configure Measurement Bandwidth.
//7. Configure Measurement Interval.
//8. Configure  Averaging.
//9. Configure Calibration Loss.
//10. Configure DUT Input Loss.
//11. Configure DUT Output Loss.
//12. Configure Cold Source Mode.
//13. Configure Cold Source DUT S-Parameters.
//14. Configure Reference Level
//         14.1. Let measurement recommend a Reference Level.
//         14.2. Manually configure Reference Level.
//15. Initiate the measurement.
//16. Fetch NF Measurements and Create Graphs.
//17. Close RFmx Session.

using System;
using NationalInstruments;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnNFColdSource
{
   public enum FrequencyListConfigurationType
   {
      Step = (int)0,
      Points = (int)1,
      Frequency = (int)2,
   }

   class RFmxSpecAnNFColdSource
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      String resourceName;
      string selectedPorts;
      double referenceLevel, dutMaxGain, dutMaxNoiseFigure;
      double timeout;
      int averagingCount;
      RFmxSpecAnMXNFAveragingEnabled averagingEnabled;

      double startFrequency;
      double stopFrequency;

      double stepSize;
      int numberOfPoints;
      double[] frequencyList = null;

      RFmxSpecAnMXNFMeasurementMethod measurementMethod;
      RFmxSpecAnMXNFColdSourceMode coldSourceMode;
      double measurementBandwidth, measurementInterval;

      RFmxSpecAnMXNFDutInputLossCompensationEnabled DutInputLossCompEnabled;
      double DutInputLossTemperature;
      double[] DutInputLoss, DutInputLossFrequency;

      RFmxSpecAnMXNFDutOutputLossCompensationEnabled DutOutputLossCompEnabled;
      double DutOutputLossTemperature;
      double[] DutOutputLoss, DutOutputLossFrequency;

      RFmxSpecAnMXNFCalibrationLossCompensationEnabled calibrationLossCompensationEnabled;
      double calibrationLossTemperature;
      double[] calibrationLoss, calibrationLossFrequency;

      double[] sParamFrequency;
      double[] s11, s12, s21, s22;            /*dB*/

      FrequencyListConfigurationType frequencyListConfiguration;
      bool manualReferenceLevel;

      /*Result variables*/
      double[] coldSourcePower;                /*dBm*/
      double[] dutGain;                      /*dB*/
      double[] dutNoiseFigure;               /*dB*/
      double[] dutNoiseTemperature;          /*K*/
      double[] frequencyListOut;

      double[] analyserNoiseFigure;          /*dB*/
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
         resourceName = "RFSA";
         selectedPorts = "";

         startFrequency = 1e+9;               /* Hz */
         stopFrequency = 2.0e+9;              /* Hz */

         stepSize = 100e+6;                   /* Hz */
         numberOfPoints = 10;

         dutMaxGain = 0.00;                   /*dB*/
         dutMaxNoiseFigure = 0.0;             /*dB*/
         referenceLevel = -55.00;             /* dBm */

         frequencyListConfiguration = FrequencyListConfigurationType.Step;
         manualReferenceLevel = true;

         measurementMethod = RFmxSpecAnMXNFMeasurementMethod.ColdSource;
         coldSourceMode = RFmxSpecAnMXNFColdSourceMode.Measure;
         measurementBandwidth = 100e+3;       /*Hz*/
         measurementInterval = 1e-3;          /*seconds*/

         DutInputLossCompEnabled = RFmxSpecAnMXNFDutInputLossCompensationEnabled.False;
         DutInputLossTemperature = 297;          /*K*/
         DutInputLoss = DutInputLossFrequency = null;

         DutOutputLossCompEnabled = RFmxSpecAnMXNFDutOutputLossCompensationEnabled.False;
         DutOutputLossTemperature = 297;         /*K*/
         DutOutputLoss = DutOutputLossFrequency = null;

         calibrationLossCompensationEnabled = RFmxSpecAnMXNFCalibrationLossCompensationEnabled.False;
         calibrationLossTemperature = 297;    /*K*/
         calibrationLoss = calibrationLossFrequency = null;

         averagingCount = 10;
         averagingEnabled = RFmxSpecAnMXNFAveragingEnabled.False;

         sParamFrequency = s11 = s12 = s21 = s22 = null;

         timeout = 10.0;                      /* seconds */
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
         specAn.SetSelectedPorts("", selectedPorts);

         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.NF, false);

         specAn.NF.Configuration.ConfigureMeasurementMethod("", measurementMethod);

         if (frequencyListConfiguration == FrequencyListConfigurationType.Step)
         {
            specAn.NF.Configuration.ConfigureFrequencyListStartStopStep("", startFrequency, stopFrequency, stepSize);
         }
         else if (frequencyListConfiguration == FrequencyListConfigurationType.Points)
         {
            specAn.NF.Configuration.ConfigureFrequencyListStartStopPoints("", startFrequency, stopFrequency, numberOfPoints);
         }
         else if (frequencyListConfiguration == FrequencyListConfigurationType.Frequency)
         {
            specAn.NF.Configuration.ConfigureFrequencyList("", frequencyList);
         }

         specAn.NF.Configuration.ConfigureMeasurementBandwidth("", measurementBandwidth);
         specAn.NF.Configuration.ConfigureMeasurementInterval("", measurementInterval);
         specAn.NF.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
         specAn.NF.Configuration.ConfigureCalibrationLoss("", calibrationLossCompensationEnabled, calibrationLossFrequency, calibrationLoss, calibrationLossTemperature);
         specAn.NF.Configuration.ConfigureDutInputLoss("", DutInputLossCompEnabled, DutInputLossFrequency, DutInputLoss, DutInputLossTemperature);
         specAn.NF.Configuration.ConfigureDutOutputLoss("", DutOutputLossCompEnabled, DutOutputLossFrequency, DutOutputLoss, DutOutputLossTemperature);
         specAn.NF.Configuration.ConfigureColdSourceMode("", coldSourceMode);
         specAn.NF.Configuration.ConfigureColdSourceDutSParameters("", sParamFrequency, s21, s12, s11, s22);

         if (!manualReferenceLevel)
         {
            specAn.NF.Configuration.RecommendReferenceLevel("", dutMaxGain, dutMaxNoiseFigure, out referenceLevel);
            Console.WriteLine("Reference Level :        {0}", referenceLevel);
            Console.WriteLine();
         }
         else
         {
            specAn.ConfigureReferenceLevel("", referenceLevel);
         }
         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */
         specAn.NF.Results.FetchColdSourcePower("", timeout, ref coldSourcePower);
         specAn.NF.Results.FetchAnalyzerNoiseFigure("", timeout, ref analyserNoiseFigure);
         specAn.NF.Results.FetchDutNoiseFigureAndGain("", timeout, ref dutNoiseFigure, ref dutNoiseTemperature, ref dutGain);
         specAn.NF.Configuration.GetFrequencyList("", ref frequencyListOut);
      }

      private void PrintResults()
      {
         int resultSize = 0;
         resultSize = (coldSourcePower == null) ? 0 : coldSourcePower.Length;
         Console.WriteLine("\nResults\n");
         for (int i = 0; i < resultSize; i++)
         {
            Console.WriteLine("\nResult {0}:\n", i);
            Console.WriteLine("Frequency (Hz)             :      {0}", frequencyListOut[i]);
            Console.WriteLine("DUT Noise Figure (dB)      :      {0}", dutNoiseFigure[i]);
            Console.WriteLine("DUT Noise Temperature (K)  :      {0}", dutNoiseTemperature[i]);
            Console.WriteLine("DUT Gain (dB)              :      {0}", dutGain[i]);
            Console.WriteLine("Analyser Noise Figure(dB)  :      {0}", analyserNoiseFigure[i]);
            Console.WriteLine("Measured Power (dBm)       :      {0}", coldSourcePower[i]);
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
