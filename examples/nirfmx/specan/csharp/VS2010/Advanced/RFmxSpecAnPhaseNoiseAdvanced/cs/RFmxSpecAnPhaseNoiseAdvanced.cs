//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties (Clock Source and Clock Frequency)
//3. Configure Selected Ports
//4. Configure instrument RF Attenuation
//5. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//6. Configure PhaseNoise measurement and enable the traces
//7. Configure Range Definition
//8. Configure Auto Range
//9. Configure Number of Ranges
//10. Configure Range(Array)
//11. Configure Averaging Multiplier
//12. Configure Smoothing
//13. Configure Spot Noise Frequency List
//14. Configure Integrated Noise
//15. Configure Spur Removal
//16. Configure Cancellation
//17. Initiate Measurement
//18. Fetch PhaseNoise Measurements and Traces
//19. Close the RFmx Session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnPhaseNoiseAdvanced
{
   class RFmxSpecAnPhaseNoiseAdvanced
   {
      RFmxInstrMX instrSession;
      RFmxSpecAnMX specAn;
      string selectedPorts;
      double centerFrequency, frequencyReferenceFrequency, referenceLevel, externalAttenuation, rbwPercentage, rfAttenuation;
      double startFrequency, stopFrequency, carrierFrequency, smoothingPercentage, timeout, bandwidth, measurementInterval;
      double peakExcursion, cancellationThreshold;
      string resourceName, frequencyReferenceSource;
      private bool enableAllTraces;
      RFmxInstrMXRFAttenuationAuto rfAttenuationAuto;
      private bool autolevel;
      RFmxSpecAnMXPhaseNoiseRangeDefinition rangeDefinition;
      RFmxSpecAnMXPhaseNoiseSpurRemovalEnabled spurRemovalEnabled;
      RFmxSpecAnMXPhaseNoiseCancellationEnabled cancellationEnabled;
      RFmxSpecAnMXPhaseNoiseIntegratedNoiseRangeDefinition integratedNoiseRangeDefinition;
      RFmxSpecAnMXPhaseNoiseSmoothingType smoothingType;
      int averagingMultiplier;
      const int numberOfRanges = 1;
      double carrierPower;
      double[] rangeStartFrequency = new double[numberOfRanges] { 1.0e+3 };               /* Hz */
      double[] rangeStopFrequency = new double[numberOfRanges] { 1.0e+6 };                /* Hz */
      double[] rangeRbwPercentage = new double[numberOfRanges] { 10.0 };                  /* % */
      int[] rangeAveragingCount = new int[numberOfRanges] { 10 };
      double[] frequencyList = null;                                                      /* Hz */
      double[] integratedNoiseStartFrequency = null, integratedNoiseStopFrequency = null; /* Hz */
      float[] frequency = null;                                                           /* Hz */
      float[] referencePhaseNoise = null;                                                 /* dBc/Hz */
      double[] spotPhaseNoise, integratedPhaseNoise, residualPMInRadian,
          residualPMInDegree, residualFM, jitter;
      float[] measuredFrequency, measuredPhaseNoise, smoothedFrequency, smoothedPhaseNoise;

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
         centerFrequency = 1e+9;                                                         /* Hz */
         referenceLevel = 0.00;                                                          /* dBm */
         externalAttenuation = 0.00;                                                     /* dB */
         timeout = 10.00;                                                                /* seconds */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e+6;                                          /* Hz */
         enableAllTraces = true;

         autolevel = true;
         rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.True;
         rfAttenuation = 10.00;                                                          /* dB */
         bandwidth = 2e+5;                                                               /* Hz */
         measurementInterval = 10e-3;                                                    /* seconds */

         //Auto Ranges
         startFrequency = 1e+3;                                                          /* Hz */
         stopFrequency = 1e+6;                                                           /* Hz */
         rbwPercentage = 10.00;                                                          /* % */

         rangeDefinition = RFmxSpecAnMXPhaseNoiseRangeDefinition.Auto;
         averagingMultiplier = 1;

         /* Smoothing */
         smoothingType = RFmxSpecAnMXPhaseNoiseSmoothingType.Logarithmic;
         smoothingPercentage = 2.00;                                                      /* % */

         /* Integrated Noise */
         integratedNoiseRangeDefinition = RFmxSpecAnMXPhaseNoiseIntegratedNoiseRangeDefinition.Measurement;

         /* Spur Removal */
         spurRemovalEnabled = RFmxSpecAnMXPhaseNoiseSpurRemovalEnabled.False;
         peakExcursion = 6.00;                                                           /* dB */

         /* Cancellation */
         cancellationEnabled = RFmxSpecAnMXPhaseNoiseCancellationEnabled.False;
         cancellationThreshold = 0.01;                                                   /* dB */
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
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         specAn.SetSelectedPorts("", selectedPorts);
         instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation);
         specAn.ConfigureFrequency("", centerFrequency);
         specAn.ConfigureExternalAttenuation("", externalAttenuation);
         if (autolevel)
         {
            specAn.AutoLevel("", bandwidth, measurementInterval, out referenceLevel);
            Console.WriteLine("Reference level(dBm)         : {0}", referenceLevel);
         }
         else
            specAn.ConfigureReferenceLevel("", referenceLevel);
         specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.PhaseNoise, enableAllTraces);

         specAn.PhaseNoise.Configuration.ConfigureRangeDefinition("", rangeDefinition);
         if (rangeDefinition == RFmxSpecAnMXPhaseNoiseRangeDefinition.Manual)
         {
            specAn.PhaseNoise.Configuration.ConfigureNumberOfRanges("", numberOfRanges);
            specAn.PhaseNoise.Configuration.ConfigureRangeArray("", rangeStartFrequency, rangeStopFrequency,
                      rangeRbwPercentage, rangeAveragingCount);
         }
         else
            specAn.PhaseNoise.Configuration.ConfigureAutoRange("", startFrequency, stopFrequency, rbwPercentage);
         specAn.PhaseNoise.Configuration.ConfigureAveragingMultiplier("", averagingMultiplier);
         specAn.PhaseNoise.Configuration.ConfigureSmoothing("", smoothingType, smoothingPercentage);
         specAn.PhaseNoise.Configuration.ConfigureSpotNoiseFrequencyList("", frequencyList);
         specAn.PhaseNoise.Configuration.ConfigureIntegratedNoise("", integratedNoiseRangeDefinition, integratedNoiseStartFrequency,
                      integratedNoiseStopFrequency);
         specAn.PhaseNoise.Configuration.ConfigureSpurRemoval("", spurRemovalEnabled, peakExcursion);
         specAn.PhaseNoise.Configuration.ConfigureCancellation("", cancellationEnabled, cancellationThreshold, frequency,
                      referencePhaseNoise);
         specAn.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         specAn.PhaseNoise.Results.FetchCarrierMeasurement("", timeout, out carrierFrequency, out carrierPower);
         specAn.PhaseNoise.Results.FetchSpotNoise("", timeout, ref spotPhaseNoise);
         specAn.PhaseNoise.Results.FetchIntegratedNoise("", timeout, ref integratedPhaseNoise, ref residualPMInRadian,
                                                       ref residualPMInDegree, ref residualFM, ref jitter);
         specAn.PhaseNoise.Results.FetchMeasuredLogPlotTrace("", timeout, ref measuredFrequency, ref measuredPhaseNoise);
         specAn.PhaseNoise.Results.FetchSmoothedLogPlotTrace("", timeout, ref smoothedFrequency, ref smoothedPhaseNoise);
      }

      private void PrintResults()
      {
         /* Display the results */
         Console.WriteLine("\nCarrier Measurement\n");
         Console.WriteLine("Carrier Frequency(Hz)        : {0}", carrierFrequency);
         Console.WriteLine("Carrier Power(dBm)           : {0}", carrierPower);

         if (spotPhaseNoise.Length > 0)
            Console.WriteLine("\nSpot Phase Noise(dBc/Hz)\n");
         for (int i = 0; i < spotPhaseNoise.Length; i++)
         {
            Console.WriteLine("Spot Phase Noise Index  {0}    : {1}", i, spotPhaseNoise[i]);
         }

         Console.WriteLine("\nIntegrated Noise");
         for (int i = 0; i < integratedPhaseNoise.Length; i++)
         {
            Console.WriteLine("\nIntegrated  Noise Range {0}", i);
            Console.WriteLine("Integrated Phase Noise(dBc)  : {0}", integratedPhaseNoise[i]);
            Console.WriteLine("Residual PM(rad)             : {0}", residualPMInRadian[i]);
            Console.WriteLine("Residual PM(deg)             : {0}", residualPMInDegree[i]);
            Console.WriteLine("Residual FM(Hz)              : {0}", residualFM[i]);
            Console.WriteLine("Jitter(s)                    : {0}", jitter[i]);
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
