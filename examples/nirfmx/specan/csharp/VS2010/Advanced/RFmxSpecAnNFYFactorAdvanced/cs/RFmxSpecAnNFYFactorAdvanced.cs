//Steps:
//1. Open a new RFmx session.
//2. Configure RF Attenuation.
//3. Configure Selected Ports.
//4. Select NF measurement.
//5. Configure Measurement Method.
//6. Configure measurement frequencies
//    6.1. Specify Start Frequency, Stop Frequency and  Frequency Step Size.
//    6.2. Specify Start Frequency, Stop Frequency and Frequency Points.
//    6.3. Specify Frequency List.
//7. Configure DUT Type.
//8. Configure Frequency Converter DUT specific properties.
//9. Configure Measurement Bandwidth.
//10. Configure Measurement Interval.
//11. Configure Averaging.
//12. Configure Calibration loss.
//13. Configure DUT Input Loss.
//14. Configure DUT Output Loss.
//15. Configure RF Preamplifier and Preselector.
//16. Configure Y-Factor Mode.
//17. Configure Y-Factor Noise Source ENR.
//18. Configure Y-Factor Noise Source Settling Time.
//19. Configure Y-Factor Noise Source Loss.
//20. Configure Reference Level
//    20.1. Let measurement recommend a Reference Level.
//    20.2. Manually Configure Reference Level.
//21. Intiate the measurement.
//22. Fetch NF Measurements and Create Graphs.
//23. Close RFmx Session.

using System;
using NationalInstruments;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnNFYFactorAdvanced
{
   public enum FrequencyListConfigurationType
   {
      Step = (int)0,
      Points = (int)1,
      Frequency = (int)2,
   }

   class RFmxSpecAnNFYFactorAdvanced
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      String resourceName;
      string selectedPorts;
      double referenceLevel, dutMaxGain, dutMaxNoiseFigure;
      double timeout;
      int averagingCount;
      RFmxSpecAnMXNFAveragingEnabled averagingEnabled;
      RFmxInstrMXPreampEnabled preamp;

      RFmxInstrMXRFAttenuationAuto rfAttenuationAuto;
      double rfAttenuation;

      double startFrequency;
      double stopFrequency;

      double stepSize;
      int numberOfPoints;
      double[] frequencyList = null;
      RFmxInstrMXDownconverterPreselectorEnabled preselectorEnabled;

      RFmxSpecAnMXNFDutType dutType;

      double LOFrequency;
      RFmxSpecAnMXNFFrequencyConverterFrequencyContext frequencyContext;
      RFmxSpecAnMXNFFrequencyConverterSideband sideband;
      double imageRejection;

      RFmxSpecAnMXNFMeasurementMethod measurementMethod;
      RFmxSpecAnMXNFYFactorMode yFactorMode;
      double measurementBandwidth, measurementInterval;

      RFmxSpecAnMXNFDutInputLossCompensationEnabled dutInputLossCompensationEnabled;
      double dutInputLossTemperature;
      double[] dutInputLoss, dutInputLossFrequency;

      RFmxSpecAnMXNFDutOutputLossCompensationEnabled dutOutputLossCompensationEnabled;
      double dutOutputLossTemperature;
      double[] dutOutputLoss, dutOutputLossFrequency;

      RFmxSpecAnMXNFCalibrationLossCompensationEnabled calibrationLossCompensationEnabled;
      double calibrationLossTemperature;
      double[] calibrationLoss, calibrationLossFrequency;

      RFmxSpecAnMXNFYFactorNoiseSourceLossCompensationEnabled noiseSourceLossCompensationEnabled;
      double noiseSourceLossTemperature;
      double[] noiseSourceLoss, noiseSourceLossFrequency;

      FrequencyListConfigurationType frequencyListConfiguration;
      bool recommend;

      double settlingTime, coldTemperature, offTemperature;
      double[] enr, enrFrequency;

      /*Result variables*/
      double[] hotPower, coldPower;            /* dBm */
      double[] dutGain;                        /* dB */
      double[] dutNoiseFigure;                 /* dB */
      double[] dutNoiseTemperature;            /* K */
      double[] measurementYFactor, calibrationYFactor;
      double[] analyserNoiseFigure;            /* dB */
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

         rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.True;
         rfAttenuation = 10;                            /* dB */

         averagingCount = 10;
         averagingEnabled = RFmxSpecAnMXNFAveragingEnabled.False;

         startFrequency = 1e+9;                         /* Hz */
         stopFrequency = 2.0e+9;                        /* Hz */

         stepSize = 100e+6;                             /* Hz */
         numberOfPoints = 10;

         dutMaxGain = 0.0;                              /* dB */
         dutMaxNoiseFigure = 0.0;                       /* dB */
         referenceLevel = -55.00;                       /* dBm */

         frequencyListConfiguration = FrequencyListConfigurationType.Step;
         recommend = false;
         preamp = RFmxInstrMXPreampEnabled.Enabled;
         preselectorEnabled = RFmxInstrMXDownconverterPreselectorEnabled.Enabled;

         dutType = RFmxSpecAnMXNFDutType.Amplifier;

         LOFrequency = 10.0e6;                           /* Hz */
         frequencyContext = RFmxSpecAnMXNFFrequencyConverterFrequencyContext.RF;
         sideband = RFmxSpecAnMXNFFrequencyConverterSideband.Lsb;
         imageRejection = 999.99;                        /* dB */

         measurementMethod = RFmxSpecAnMXNFMeasurementMethod.YFactor;
         yFactorMode = RFmxSpecAnMXNFYFactorMode.Measure;
         measurementBandwidth = 100e+3;                  /* Hz */
         measurementInterval = 1e-3;                     /* seconds */

         dutInputLossCompensationEnabled = RFmxSpecAnMXNFDutInputLossCompensationEnabled.False;
         dutInputLossTemperature = 297;                  /* K */
         dutInputLoss = null;                            /* dB */
         dutInputLossFrequency = null;                   /* Hz */

         dutOutputLossCompensationEnabled = RFmxSpecAnMXNFDutOutputLossCompensationEnabled.False;
         dutOutputLossTemperature = 297;                 /* K */
         dutOutputLoss = null;                           /* dB */
         dutOutputLossFrequency = null;                  /* Hz */

         settlingTime = 0.0;                             /* seconds */
         coldTemperature = 302.8;                        /* K */
         offTemperature = 297.0;                         /* K */
         enr = null;                                     /* dB */
         enrFrequency = null;                            /* Hz */

         calibrationLossCompensationEnabled = RFmxSpecAnMXNFCalibrationLossCompensationEnabled.False;
         calibrationLossTemperature = 297;               /* K */
         calibrationLoss = null;                         /* dB */
         calibrationLossFrequency = null;                /* Hz */

         noiseSourceLossCompensationEnabled = RFmxSpecAnMXNFYFactorNoiseSourceLossCompensationEnabled.False;
         noiseSourceLossTemperature = 297;               /* K */
         noiseSourceLoss = null;                         /* dB */
         noiseSourceLossFrequency = null;                /* Hz */

         timeout = 10.0;                                 /* seconds */
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

         instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation);

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

         specAn.NF.Configuration.SetDutType("", dutType);

         specAn.NF.Configuration.SetFrequencyConverterLOFrequency("", LOFrequency);
         specAn.NF.Configuration.SetFrequencyConverterFrequencyContext("", frequencyContext);
         specAn.NF.Configuration.SetFrequencyConverterSideband("", sideband);
         specAn.NF.Configuration.SetFrequencyConverterImageRejection("", imageRejection);

         specAn.NF.Configuration.ConfigureMeasurementBandwidth("", measurementBandwidth);
         specAn.NF.Configuration.ConfigureMeasurementInterval("", measurementInterval);
         specAn.NF.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
         specAn.NF.Configuration.ConfigureCalibrationLoss("", calibrationLossCompensationEnabled, calibrationLossFrequency, calibrationLoss, calibrationLossTemperature);
         specAn.NF.Configuration.ConfigureDutInputLoss("", dutInputLossCompensationEnabled, dutInputLossFrequency, dutInputLoss, dutInputLossTemperature);
         specAn.NF.Configuration.ConfigureDutOutputLoss("", dutOutputLossCompensationEnabled, dutOutputLossFrequency, dutOutputLoss, dutOutputLossTemperature);
         instrSession.SetPreampEnabled("", preamp);
         instrSession.SetDownconverterPreselectorEnabled("", preselectorEnabled);
         specAn.NF.Configuration.ConfigureYFactorMode("", yFactorMode);
         specAn.NF.Configuration.ConfigureYFactorNoiseSourceEnr("", enrFrequency, enr, coldTemperature, offTemperature);
         specAn.NF.Configuration.ConfigureYFactorNoiseSourceSettlingTime("", settlingTime);
         specAn.NF.Configuration.ConfigureYFactorNoiseSourceLoss("", noiseSourceLossCompensationEnabled, noiseSourceLossFrequency, noiseSourceLoss, noiseSourceLossTemperature);
         if (recommend)
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
         specAn.NF.Results.FetchYFactors("", timeout, ref measurementYFactor, ref calibrationYFactor);
         specAn.NF.Results.FetchYFactorPowers("", timeout, ref hotPower, ref coldPower);
         specAn.NF.Results.FetchAnalyzerNoiseFigure("", timeout, ref analyserNoiseFigure);
         specAn.NF.Results.FetchDutNoiseFigureAndGain("", timeout, ref dutNoiseFigure, ref dutNoiseTemperature, ref dutGain);
         specAn.NF.Configuration.GetFrequencyList("", ref frequencyList);
      }

      private void PrintResults()
      {
         int resultSize = 0;
         resultSize = (hotPower == null) ? 0 : hotPower.Length;
         Console.WriteLine("\nResults :\n");
         for (int i = 0; i < resultSize; i++)
         {
            Console.WriteLine("\nResult {0}:\n", i);
            Console.WriteLine("Frequency (Hz)             :      {0}", frequencyList[i]);
            Console.WriteLine("DUT Noise Figure (dB)      :      {0}", dutNoiseFigure[i]);
            Console.WriteLine("DUT Noise Temperature (K)  :      {0}", dutNoiseTemperature[i]);
            Console.WriteLine("DUT Gain (dB)              :      {0}", dutGain[i]);
            Console.WriteLine("Analyser Noise Figure(dB)  :      {0}", analyserNoiseFigure[i]);
            Console.WriteLine("Hot Power (dBm)            :      {0}", hotPower[i]);
            Console.WriteLine("Cold Power (dBm)           :      {0}", coldPower[i]);
            Console.WriteLine("Measurement Y-Factor (dB)  :      {0}", measurementYFactor[i]);
            Console.WriteLine("Calibration Y-Factor (dB)  :      {0}", calibrationYFactor[i]);
            Console.WriteLine();
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
