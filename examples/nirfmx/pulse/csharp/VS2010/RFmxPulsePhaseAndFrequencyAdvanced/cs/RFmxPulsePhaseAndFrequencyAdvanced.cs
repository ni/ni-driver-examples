//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure LO Leakage Avoidance Enabled, LO source and Downconverter Frequency Offset(Hz).
//4. Configure basic RF signal properties(Center Frequency, RF Attenuation and External Attenuation)
//5. Enabling Pulse and Traces Result.
//6. Configure Trigger Type and Trigger Parameters.
//7. Configure Acquisition Settings.
//8. Configure Pulse Detection Settings
//9. Configure State and Threshold Level Settings.
//10. Configure Measurement Point Settings.
//11. Configure Frequency & Phase Settings.
//12. Configure Modulation Settings.
//13. Enabling Pulse Stability enabled as False and Pulse Metrics enabled settings as True.
//14. Initiate the Measurement.
//15. Wait for Measurement to complete.
//16. Results Phase, Frequency Measurements and FM Chirp results as well as their Statistical results,Phase(Wrapped) Trace, Frequency Trace. 
//17. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.PulseMX;

namespace NationalInstruments.Examples.RFmxPulsePhaseAndFrequencyAdvanced
{
    public class RFmxPulsePhaseAndFrequencyAdvanced
    {
        RFmxInstrMX instrSession;
        RFmxPulseMX Pulse;
        string resourceName;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;
        
        double downConverterFrequencyOffset;

        RFmxInstrMXLOLeakageAvoidanceEnabled LOLeakageAvoidanceEnabled;
        string LOSource;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        bool iqPowerEdgeEnabled;
        double iqPowerEdgeLevel;
        double triggerDelay;
        RFmxPulseMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        double minimumQuietTime;

        double measurementBandwidth;
        RFmxPulseMXMeasurementFilterType measurementFilterType;
        double acquisitionLength;
        RFmxPulseMXMaximumPulseCountEnabled maximumPulseCountEnabled;
        int maximumPulseCount;

        RFmxPulseMXPulseLevelComputationMethod pulseLevelComputationMethod;
        double pulseUpperThresholdLevel;
        double pulseWidthThresholdLevel;
        double pulseLowerThresholdLevel;

        RFmxPulseMXPulseMeasurementPointReference pulseMeasurementPointReference;
        double pulseMeasurementPointOffset;
        double pulseMeasurementPointAveragingDuration;

        RFmxPulseMXPulseDetectionReference pulseDetectionReference;
        double pulseDetectionThreshold;
        double pulseDetectionHysteresis;
        double pulseDetectionMinimumOffDuration;

        RFmxPulseMXPulseFrequencyAndPhaseDeviationRangeReference pulseFrequencyPhaseDeviationRangeReference;
        double pulseFrequencyPhaseDeviationRangeLength;
        double pulseFrequencyPhaseDeviationRangeEdgeStart;
        double pulseFrequencyPhaseDeviationRangeEdgeStop;

        RFmxPulseMXPulseModulationType pulseModulationType;
        RFmxPulseMXPulseCWFrequencyOffsetAuto pulseCWFrequencyOffsetAuto;
        double pulseCWFrequencyOffset;

        double timeout;                                                       /* (s) */

        double[] pulseResultsAveragePhase;                                    /* (deg) */
        double[] pulseResultsPhaseDeviation;                                  /* (deg) */
        double[] pulseResultsPhaseErrorRms;                                   /* (deg) */

        double[] pulseResultsAverageFrequency;                                /* (Hz) */
        double[] pulseResultsFrequencyDeviation;                              /* (Hz) */
        double[] pulseResultsFrequencyErrorRms;                               /* (Hz) */

        double[] pulseResultsFMChirpRate;                                     /* (Hz/us)*/
        double[] pulseResultsFMChirpRate2;                                    /* (Hz/us)*/

        double pulseResultsAveragePhaseMean;                                  /* (deg) */
        double pulseResultsAveragePhaseMaximum;                               /* (deg) */
        double pulseResultsAveragePhaseMinimum;                               /* (deg) */
        double pulseResultsAveragePhaseSD;                                    /* (deg) */
        double pulseResultsPhaseDeviationMean;                                /* (deg) */
        double pulseResultsPhaseDeviationMaximum;                             /* (deg) */
        double pulseResultsPhaseDeviationMinimum;                             /* (deg) */
        double pulseResultsPhaseDeviationSD;                                  /* (deg) */
        double pulseResultsPhaseErrorRmsMean;                                 /* (deg) */
        double pulseResultsPhaseErrorRmsMaximum;                              /* (deg) */
        double pulseResultsPhaseErrorRmsMinimum;                              /* (deg) */
        double pulseResultsPhaseErrorRmsSD;                                   /* (deg) */

        double pulseResultsAverageFrequencyMean;                              /* (Hz) */
        double pulseResultsAverageFrequencyMaximum;                           /* (Hz) */
        double pulseResultsAverageFrequencyMinimum;                           /* (Hz) */
        double pulseResultsAverageFrequencySD;                                /* (Hz) */
        double pulseResultsFrequencyDeviationMean;                            /* (Hz) */
        double pulseResultsFrequencyDeviationMaximum;                         /* (Hz) */
        double pulseResultsFrequencyDeviationMinimum;                         /* (Hz) */
        double pulseResultsFrequencyDeviationSD;                              /* (Hz) */
        double pulseResultsFrequencyErrorRmsMean;                             /* (Hz) */
        double pulseResultsFrequencyErrorRmsMaximum;                          /* (Hz) */
        double pulseResultsFrequencyErrorRmsMinimum;                          /* (Hz) */
        double pulseResultsFrequencyErrorRmsSD;                               /* (Hz) */

        double pulseResultsFMChirpRateMean;                                   /* (Hz/us) */
        double pulseResultsFMChirpRateMaximum;                                /* (Hz/us) */
        double pulseResultsFMChirpRateMinimum;                                /* (Hz/us) */
        double pulseResultsFMChirpRateSD;                                     /* (Hz/us) */
        double pulseResultsFMChirpRate2Mean;                                  /* (Hz/us) */
        double pulseResultsFMChirpRate2Maximum;                               /* (Hz/us) */
        double pulseResultsFMChirpRate2Minimum;                               /* (Hz/us) */
        double pulseResultsFMChirpRate2SD;                                    /* (Hz/us) */
        
        AnalogWaveform<float> phaseWrappedTrace;                               /* (deg) */
        AnalogWaveform<float> frequencyTrace;                               /* (Hz) */

  public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigurePulse();
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
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        void InitializeVariables()
        {
            resourceName = "RFSA";

            centerFrequency = 1.0e9;                                           /* (Hz) */
            referenceLevel = -10.0;                                            /* (dBm) */
            externalAttenuation = 0.0;                                         /* (dB) */

            downConverterFrequencyOffset = 0.0;                                /* (Hz) */

            LOLeakageAvoidanceEnabled = RFmxInstrMXLOLeakageAvoidanceEnabled.True;
            LOSource = RFmxInstrMXConstants.LOSourceOnboard;

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e6;                              /* (Hz) */

            iqPowerEdgeEnabled = true;
            iqPowerEdgeLevel = -20.0;                                          /* (dBm) */
            triggerDelay = 0.0;                                                /* (s) */
            minimumQuietTimeMode = RFmxPulseMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 5.0e-6;                                         /* (s) */

            measurementBandwidth = 80.0e6;                                     /* (Hz) */
            measurementFilterType = RFmxPulseMXMeasurementFilterType.Gaussian;
            acquisitionLength = 1.0e-3;                                        /* (s) */
            maximumPulseCountEnabled = RFmxPulseMXMaximumPulseCountEnabled.False;
            maximumPulseCount = 100;

            pulseLevelComputationMethod = RFmxPulseMXPulseLevelComputationMethod.Median;
            pulseUpperThresholdLevel = 90.0;                                   /* (%) */
            pulseWidthThresholdLevel = 50.0;                                   /* (%) */
            pulseLowerThresholdLevel = 10.0;                                   /* (%) */

            pulseMeasurementPointReference = RFmxPulseMXPulseMeasurementPointReference.Center;
            pulseMeasurementPointOffset = 0.0;                                 /* (s) */
            pulseMeasurementPointAveragingDuration = 0.0;                      /* (s) */

            pulseDetectionReference = RFmxPulseMXPulseDetectionReference.ReferenceLevel;
            pulseDetectionThreshold = -20.0;
            pulseDetectionHysteresis = 1.0;                                    /* (dB) */
            pulseDetectionMinimumOffDuration = 50.0e-9;                                 /* (s) */

            pulseFrequencyPhaseDeviationRangeReference = 
                RFmxPulseMXPulseFrequencyAndPhaseDeviationRangeReference.Center;
            pulseFrequencyPhaseDeviationRangeLength = 75.0;                    /* (%) */
            pulseFrequencyPhaseDeviationRangeEdgeStart = 0.0;                  /* (s) */
            pulseFrequencyPhaseDeviationRangeEdgeStop = 0.0;                   /* (s) */

            pulseModulationType = RFmxPulseMXPulseModulationType.CW;
            pulseCWFrequencyOffsetAuto = RFmxPulseMXPulseCWFrequencyOffsetAuto.True;
            pulseCWFrequencyOffset = 0.0;                                      /* (Hz) */

            timeout = 10.0;
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigurePulse()
        {
            Pulse = instrSession.GetPulseSignalConfiguration();                /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            instrSession.SetLOLeakageAvoidanceEnabled("", LOLeakageAvoidanceEnabled);
            instrSession.SetLOSource("", LOSource);
            instrSession.SetDownconverterFrequencyOffset("", downConverterFrequencyOffset);
            Pulse.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            Pulse.SelectMeasurements("", RFmxPulseMXMeasurementTypes.Pulse, false);
            Pulse.ConfigureIQPowerEdgeTrigger("", "0", RFmxPulseMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
               triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxPulseMXIQPowerEdgeTriggerLevelType.Relative,
               iqPowerEdgeEnabled);

            Pulse.SetMeasurementBandwidth("", measurementBandwidth);
            Pulse.SetMeasurementFilterType("", measurementFilterType);
            Pulse.SetAcquisitionLength("", acquisitionLength);
            Pulse.SetMaximumPulseCountEnabled("", maximumPulseCountEnabled);
            Pulse.SetMaximumPulseCount("", maximumPulseCount);

            Pulse.Pulse.Configuration.SetDetectionReference("", pulseDetectionReference);
            Pulse.Pulse.Configuration.SetDetectionThreshold("", pulseDetectionThreshold);
            Pulse.Pulse.Configuration.SetDetectionHysteresis("", pulseDetectionHysteresis);
            Pulse.Pulse.Configuration.SetDetectionMinimumOffDuration("", pulseDetectionMinimumOffDuration);

            Pulse.Pulse.Configuration.SetLevelComputationMethod("", pulseLevelComputationMethod);
            Pulse.Pulse.Configuration.SetUpperThresholdLevel("", pulseUpperThresholdLevel);
            Pulse.Pulse.Configuration.SetWidthThresholdLevel("", pulseWidthThresholdLevel);
            Pulse.Pulse.Configuration.SetLowerThresholdLevel("", pulseLowerThresholdLevel);

            Pulse.Pulse.Configuration.SetMeasurementPointReference("", pulseMeasurementPointReference);
            Pulse.Pulse.Configuration.SetMeasurementPointOffset("", pulseMeasurementPointOffset);
            Pulse.Pulse.Configuration.SetMeasurementPointAveragingDuration("", pulseMeasurementPointAveragingDuration);

            Pulse.Pulse.Configuration.SetFrequencyAndPhaseDeviationRangeReference("",
                pulseFrequencyPhaseDeviationRangeReference);
            Pulse.Pulse.Configuration.SetFrequencyAndPhaseDeviationRangeLength("",
                pulseFrequencyPhaseDeviationRangeLength);
            Pulse.Pulse.Configuration.SetFrequencyAndPhaseDeviationRangeEdgeStart("",
                pulseFrequencyPhaseDeviationRangeEdgeStart);
            Pulse.Pulse.Configuration.SetFrequencyAndPhaseDeviationRangeEdgeStop("",
                pulseFrequencyPhaseDeviationRangeEdgeStop);

            Pulse.Pulse.Configuration.SetFrequencyAndPhaseModulationType("", pulseModulationType);
            Pulse.Pulse.Configuration.SetFrequencyAndPhaseCWFrequencyOffsetAuto("", pulseCWFrequencyOffsetAuto);
            Pulse.Pulse.Configuration.SetFrequencyAndPhaseCWFrequencyOffset("", pulseCWFrequencyOffset);

            Pulse.Pulse.Configuration.SetMetricsEnabled("", RFmxPulseMXPulseMetricsEnabled.True);
            Pulse.Pulse.Configuration.SetStabilityEnabled("", RFmxPulseMXPulseStabilityEnabled.False);
            
            Pulse.Pulse.Configuration.SetAllTracesEnabled("", true);

            Pulse.Initiate("", "");

            Pulse.WaitForMeasurementComplete("", timeout);
        }

        void RetrieveResults()
        {
            Pulse.Pulse.Results.GetAveragePhase("", ref pulseResultsAveragePhase);
            Pulse.Pulse.Results.GetPhaseDeviation("", ref pulseResultsPhaseDeviation);
            Pulse.Pulse.Results.GetPhaseErrorRms("", ref pulseResultsPhaseErrorRms);
            
            Pulse.Pulse.Results.GetAverageFrequency("", ref pulseResultsAverageFrequency);
            Pulse.Pulse.Results.GetFrequencyDeviation("", ref pulseResultsFrequencyDeviation);
            Pulse.Pulse.Results.GetFrequencyErrorRms("", ref pulseResultsFrequencyErrorRms);
            
            Pulse.Pulse.Results.GetFMChirpRate("", ref pulseResultsFMChirpRate);
            Pulse.Pulse.Results.GetFMChirpRate2("", ref pulseResultsFMChirpRate2);

            Pulse.Pulse.Results.GetAveragePhaseMean("", out pulseResultsAveragePhaseMean);
            Pulse.Pulse.Results.GetAveragePhaseMaximum("", out pulseResultsAveragePhaseMaximum);
            Pulse.Pulse.Results.GetAveragePhaseMinimum("", out pulseResultsAveragePhaseMinimum);
            Pulse.Pulse.Results.GetAveragePhaseStandardDeviation("", out pulseResultsAveragePhaseSD);
            
            Pulse.Pulse.Results.GetPhaseDeviationMean("", out pulseResultsPhaseDeviationMean);
            Pulse.Pulse.Results.GetPhaseDeviationMaximum("", out pulseResultsPhaseDeviationMaximum);
            Pulse.Pulse.Results.GetPhaseDeviationMinimum("", out pulseResultsPhaseDeviationMinimum);
            Pulse.Pulse.Results.GetPhaseDeviationStandardDeviation("", out pulseResultsPhaseDeviationSD);
            
            Pulse.Pulse.Results.GetPhaseErrorRmsMean("", out pulseResultsPhaseErrorRmsMean);
            Pulse.Pulse.Results.GetPhaseErrorRmsMaximum("", out pulseResultsPhaseErrorRmsMaximum);
            Pulse.Pulse.Results.GetPhaseErrorRmsMinimum("", out pulseResultsPhaseErrorRmsMinimum);
            Pulse.Pulse.Results.GetPhaseErrorRmsStandardDeviation("", out pulseResultsPhaseErrorRmsSD);
            
            Pulse.Pulse.Results.GetAverageFrequencyMean("", out pulseResultsAverageFrequencyMean);
            Pulse.Pulse.Results.GetAverageFrequencyMaximum("", out pulseResultsAverageFrequencyMaximum);
            Pulse.Pulse.Results.GetAverageFrequencyMinimum("", out pulseResultsAverageFrequencyMinimum);
            Pulse.Pulse.Results.GetAverageFrequencyStandardDeviation("", out pulseResultsAverageFrequencySD);
            
            Pulse.Pulse.Results.GetFrequencyDeviationMean("", out pulseResultsFrequencyDeviationMean);
            Pulse.Pulse.Results.GetFrequencyDeviationMaximum("", out pulseResultsFrequencyDeviationMaximum);
            Pulse.Pulse.Results.GetFrequencyDeviationMinimum("", out pulseResultsFrequencyDeviationMinimum);
            Pulse.Pulse.Results.GetFrequencyDeviationStandardDeviation("", out pulseResultsFrequencyDeviationSD);
            
            Pulse.Pulse.Results.GetFrequencyErrorRmsMean("", out pulseResultsFrequencyErrorRmsMean);
            Pulse.Pulse.Results.GetFrequencyErrorRmsMaximum("", out pulseResultsFrequencyErrorRmsMaximum);
            Pulse.Pulse.Results.GetFrequencyErrorRmsMinimum("", out pulseResultsFrequencyErrorRmsMinimum);
            Pulse.Pulse.Results.GetFrequencyErrorRmsStandardDeviation("", out pulseResultsFrequencyErrorRmsSD);
            
            Pulse.Pulse.Results.GetFMChirpRateMean("", out pulseResultsFMChirpRateMean);
            Pulse.Pulse.Results.GetFMChirpRateMaximum("", out pulseResultsFMChirpRateMaximum);
            Pulse.Pulse.Results.GetFMChirpRateMinimum("", out pulseResultsFMChirpRateMinimum);
            Pulse.Pulse.Results.GetFMChirpRateStandardDeviation("", out pulseResultsFMChirpRateSD);
            
            Pulse.Pulse.Results.GetFMChirpRate2Mean("", out pulseResultsFMChirpRate2Mean);
            Pulse.Pulse.Results.GetFMChirpRate2Maximum("", out pulseResultsFMChirpRate2Maximum);
            Pulse.Pulse.Results.GetFMChirpRate2Minimum("", out pulseResultsFMChirpRate2Minimum);
            Pulse.Pulse.Results.GetFMChirpRate2StandardDeviation("", out pulseResultsFMChirpRate2SD);

            Pulse.Pulse.Results.FetchPhaseWrappedTrace("", timeout, ref phaseWrappedTrace);
            Pulse.Pulse.Results.FetchFrequencyTrace("", timeout, ref frequencyTrace);

        }

        void PrintResults()
        {
            Console.WriteLine("\n-----------------------Phase Results--------------------------\n");
            for (int i = 0; i < pulseResultsAveragePhase.Length; i++)
            {
                Console.WriteLine("Index                                                : {0}", i);
                Console.WriteLine("Average Phase (deg)                                  : {0}", 
                    pulseResultsAveragePhase[i]);
                Console.WriteLine("Phase Deviation (deg)                                : {0}", 
                    pulseResultsPhaseDeviation[i]);
                Console.WriteLine("Phase Error RMS (deg)                                : {0}", 
                    pulseResultsPhaseErrorRms[i]);
                Console.WriteLine("--------------------------------------------------------------\n");
            }

            Console.WriteLine("\n----------------------Frequency Results-----------------------\n");
            for (int i = 0; i < pulseResultsAverageFrequency.Length; i++)
            {
                Console.WriteLine("Index                                                : {0}", i);
                Console.WriteLine("Average Frequency (Hz)                               : {0}", 
                    pulseResultsAverageFrequency[i]);
                Console.WriteLine("Frequency Deviation (Hz)                             : {0}", 
                    pulseResultsFrequencyDeviation[i]);
                Console.WriteLine("Frequency Error RMS (Hz)                             : {0}", 
                    pulseResultsFrequencyErrorRms[i]);
                Console.WriteLine("--------------------------------------------------------------\n");
            }

            Console.WriteLine("\n\n---------------------FM Chirp Results-------------------------\n");
            for (int i = 0; i < pulseResultsFMChirpRate.Length; i++)
            {
                Console.WriteLine("Index                                                : {0}", i);
                Console.WriteLine("Chrip Rate (Hz/us)                                   : {0:F11}", 
                    pulseResultsFMChirpRate[i]);
                Console.WriteLine("Chirp Rate2 (Hz/us)                                  : {0:F11}", 
                    pulseResultsFMChirpRate2[i]);
                Console.WriteLine("--------------------------------------------------------------\n");
            }

            Console.WriteLine("\n\n----------------Statistical Phase Results---------------------\n");
            Console.WriteLine("Average Phase Mean (deg)                             : {0}", 
                pulseResultsAveragePhaseMean);
            Console.WriteLine("Average Phase Maximum (deg)                          : {0}", 
                pulseResultsAveragePhaseMaximum);
            Console.WriteLine("Average Phase Minimum(deg)                           : {0}", 
                pulseResultsAveragePhaseMinimum);
            Console.WriteLine("Average Phase Standard Deviation (deg)               : {0}", 
                pulseResultsAveragePhaseSD);
            Console.WriteLine("Phase Deviation Mean (deg)                           : {0}", 
                pulseResultsPhaseDeviationMean);
            Console.WriteLine("Phase Deviation Maximum (deg)                        : {0}", 
                pulseResultsPhaseDeviationMaximum);
            Console.WriteLine("Phase Deviation Minimum(deg)                         : {0}", 
                pulseResultsPhaseDeviationMinimum);
            Console.WriteLine("Phase Deviation Standard Deviation (deg)             : {0}", 
                pulseResultsPhaseDeviationSD);
            Console.WriteLine("Phase Error RMS Mean (deg)                           : {0}", 
                pulseResultsPhaseErrorRmsMean);
            Console.WriteLine("Phase Error RMS Maximum (deg)                        : {0}", 
                pulseResultsPhaseErrorRmsMaximum);
            Console.WriteLine("Phase Error RMS Minimum(deg)                         : {0}", 
                pulseResultsPhaseErrorRmsMinimum);
            Console.WriteLine("Phase Error RMS Standard Deviation (deg)             : {0}", 
                pulseResultsPhaseErrorRmsSD);
            Console.WriteLine("--------------------------------------------------------------\n");

            Console.WriteLine("\n\n--------------Statistical Frequency Results-------------------\n");
            Console.WriteLine("Average Frequency Mean (Hz)                          : {0}", 
                pulseResultsAverageFrequencyMean);
            Console.WriteLine("Average Frequency Maximum (Hz)                       : {0}", 
                pulseResultsAverageFrequencyMaximum);
            Console.WriteLine("Average Frequency Minimum(Hz)                        : {0}", 
                pulseResultsAverageFrequencyMinimum);
            Console.WriteLine("Average Frequency Standard Deviation (Hz)            : {0}", 
                pulseResultsAverageFrequencySD);
            Console.WriteLine("Frequency Deviation Mean (Hz)                        : {0}", 
                pulseResultsFrequencyDeviationMean);
            Console.WriteLine("Frequency Deviation Maximum (Hz)                     : {0}", 
                pulseResultsFrequencyDeviationMaximum);
            Console.WriteLine("Frequency Deviation Minimum(Hz)                      : {0}", 
                pulseResultsFrequencyDeviationMinimum);
            Console.WriteLine("Frequency Deviation Standard Deviation (Hz)          : {0}", 
                pulseResultsFrequencyDeviationSD);
            Console.WriteLine("Frequency Error RMS Mean (Hz)                        : {0}", 
                pulseResultsFrequencyErrorRmsMean);
            Console.WriteLine("Frequency Error RMS Maximum (Hz)                     : {0}", 
                pulseResultsFrequencyErrorRmsMaximum);
            Console.WriteLine("Frequency Error RMS Minimum(Hz)                      : {0}", 
                pulseResultsFrequencyErrorRmsMinimum);
            Console.WriteLine("Frequency Error RMS Standard Deviation (Hz)          : {0}", 
                pulseResultsFrequencyErrorRmsSD);
            Console.WriteLine("--------------------------------------------------------------\n");

            Console.WriteLine("\n\n--------------Statistical FM Chirp Results--------------------\n");
            Console.WriteLine("Chirp Rate Mean (Hz/us)                             : {0}", 
                pulseResultsFMChirpRateMean);
            Console.WriteLine("Chirp Rate Maximum (Hz/us)                          : {0}", 
                pulseResultsFMChirpRateMaximum);
            Console.WriteLine("Chirp Rate Minimum(Hz/us)                           : {0}", 
                pulseResultsFMChirpRateMinimum);
            Console.WriteLine("Chirp Rate Standard Deviation (Hz/us)               : {0}", 
                pulseResultsFMChirpRateSD);
            Console.WriteLine("Chirp Rate 2 Mean (Hz/us)                            : {0}", 
                pulseResultsFMChirpRate2Mean);
            Console.WriteLine("Chirp Rate 2 Maximum (Hz/us)                         : {0}", 
                pulseResultsFMChirpRate2Maximum);
            Console.WriteLine("Chirp Rate 2 Minimum(Hz/us)                          : {0}", 
                pulseResultsFMChirpRate2Minimum);
            Console.WriteLine("Chirp Rate 2 Standard Deviation (Hz/us)              : {0}", 
                pulseResultsFMChirpRate2SD);
            Console.WriteLine("--------------------------------------------------------------\n");

        }

        void CloseSession()
        {
            if (Pulse != null)
            {
                Pulse.Dispose();
                Pulse = null;
            }
            if (instrSession != null)
            {
                instrSession.Close();
                instrSession = null;
            }
        }

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

    }
}


