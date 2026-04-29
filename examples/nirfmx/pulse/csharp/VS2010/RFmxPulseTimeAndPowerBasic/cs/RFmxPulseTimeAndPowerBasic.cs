//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic RF signal properties(Center Frequency, RF Attenuation and External Attenuation)
//4. Enabling Pulse and Traces Result
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Acquisition Settings.
//7. Configure Pulse Detection Settings.
//8. Configure State and Thershold Level Settings.
//9. Configure Selected Traces Settings and constant control, Pulse Metrics and Pulse Stability enabled settings.
//10. Initiate the Measurement.
//11. Wait for Measurement to complete.
//12. Fetch  Pulse Count, Timing, Amplitude Measurements and Amplitude Traces.
//13. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.PulseMX;

namespace NationalInstruments.Examples.RFmxPulseTimeAndPowerBasic
{
    public class RFmxPulseTimeAndPowerBasic
    {
        RFmxInstrMX instrSession;
        RFmxPulseMX Pulse;
        string resourceName;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

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

        RFmxPulseMXPulseDetectionReference pulseDetectionReference;
        double pulseDetectionThreshold;
        double pulseDetectionHysteresis;
        double pulseDetectionMinimumOffDuration;

        RFmxPulseMXPulseLevelComputationMethod pulseLevelComputationMethod;
        RFmxPulseMXPulseDroopCompensationEnabled pulseDroopCompensationEnabled;

        int pulseSelectedPulseTrace;
        RFmxPulseMXPulseAmplitudeTraceUnit pulseAmplitudeTraceUnit;

        double timeout;                                                       /* (s) */

        int pulseCount;

        double[] pulseResultsRiseTime;                                        /* (s) */
        double[] pulseResultsFallTime;                                        /* (s) */
        double[] pulseResultsPulseWidth;                                      /* (s) */
        double[] pulseResultsPulseRepetitionInterval;                         /* (s) */

        double[] pulseResultsTopLevel;                                        /* (dBm) */
        double[] pulseResultsBaseLevel;                                       /* (dBm) */
        double[] pulseResultsAverageOnLevel;                                  /* (dBm) */
        double[] pulseResultsOvershoot;                                       /* (%) */
        double[] pulseResultsDroop;                                           /* (%) */
        double[] pulseResultsRipple;                                          /* (%) */

        AnalogWaveform<float> amplitude;                                      /* (dBm) */


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

            centerFrequency = 1.0e9;                                          /* (Hz) */
            referenceLevel = -10.0;                                           /* (dBm) */
            externalAttenuation = 0.0;                                        /* (dB) */

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e6;                             /* (Hz) */

            iqPowerEdgeEnabled = true;
            iqPowerEdgeLevel = -20.0;                                         /* (dBm) */
            triggerDelay = 0.0;                                               /* (s) */
            minimumQuietTimeMode = RFmxPulseMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 5.0e-6;                                        /* (s) */

            measurementBandwidth = 80.0e6;                                    /* (Hz) */
            measurementFilterType = RFmxPulseMXMeasurementFilterType.Gaussian;
            acquisitionLength = 1.0e-3;                                       /* (s) */
            maximumPulseCountEnabled = RFmxPulseMXMaximumPulseCountEnabled.False;
            maximumPulseCount = 100;

            pulseDetectionReference = RFmxPulseMXPulseDetectionReference.ReferenceLevel;
            pulseDetectionThreshold = -20.0;
            pulseDetectionHysteresis = 1.0;                                   /* (dB) */
            pulseDetectionMinimumOffDuration = 50.0e-9;                                /* (s) */

            pulseLevelComputationMethod = RFmxPulseMXPulseLevelComputationMethod.Median;
            pulseDroopCompensationEnabled = RFmxPulseMXPulseDroopCompensationEnabled.True;

            pulseSelectedPulseTrace = 0;
            pulseAmplitudeTraceUnit = RFmxPulseMXPulseAmplitudeTraceUnit.dBm;

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
            Pulse.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            Pulse.SelectMeasurements("", RFmxPulseMXMeasurementTypes.Pulse, true);
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
            Pulse.Pulse.Configuration.SetDroopCompensationEnabled("", pulseDroopCompensationEnabled);

            Pulse.Pulse.Configuration.SetMetricsEnabled("", RFmxPulseMXPulseMetricsEnabled.True);
            Pulse.Pulse.Configuration.SetStabilityEnabled("", RFmxPulseMXPulseStabilityEnabled.False);
            Pulse.Pulse.Configuration.SetSelectedPulseTrace("", pulseSelectedPulseTrace);
            Pulse.Pulse.Configuration.SetAmplitudeTraceUnit("", pulseAmplitudeTraceUnit);

            Pulse.Initiate("", "");

            Pulse.WaitForMeasurementComplete("", timeout);
        }

        void RetrieveResults()
        {
            Pulse.Pulse.Results.GetPulseCount("", out pulseCount);
            
            Pulse.Pulse.Results.GetRiseTime("", ref pulseResultsRiseTime);
            Pulse.Pulse.Results.GetFallTime("", ref pulseResultsFallTime);  
            Pulse.Pulse.Results.GetPulseWidth("", ref pulseResultsPulseWidth);
            Pulse.Pulse.Results.GetPulseRepetitionInterval("", ref pulseResultsPulseRepetitionInterval);

            Pulse.Pulse.Results.GetTopLevel("", ref pulseResultsTopLevel);
            Pulse.Pulse.Results.GetBaseLevel("", ref pulseResultsBaseLevel);
            Pulse.Pulse.Results.GetAverageOnLevel("", ref pulseResultsAverageOnLevel);
            Pulse.Pulse.Results.GetOvershoot("", ref pulseResultsOvershoot);
            Pulse.Pulse.Results.GetDroop("", ref pulseResultsDroop);
            Pulse.Pulse.Results.GetRipple("", ref pulseResultsRipple);

            Pulse.Pulse.Results.FetchAmplitudeTrace("", timeout, ref amplitude);
        }

        void PrintResults()
        {
            Console.WriteLine("\nPulse Count                            : {0}\n", pulseCount);
            Console.WriteLine("\n------------------- Timing Results ----------------------\n");
            for (int i = 0; i < pulseResultsRiseTime.Length; i++)
            {
                Console.WriteLine("Index                                  : {0}", i);
                Console.WriteLine("Rise Time (dB)                         : {0:F11}", pulseResultsRiseTime[i]);
                Console.WriteLine("Fall Time (dB)                         : {0:F11}", pulseResultsFallTime[i]);
                Console.WriteLine("Pulse Width (dB)                       : {0:F9}", pulseResultsPulseWidth[i]);
                Console.WriteLine("Pulse repetition Interval (dB)         : {0:F9}", pulseResultsPulseRepetitionInterval[i]);
                Console.WriteLine("-------------------------------------------------------\n");
            }

            Console.WriteLine("\n\n------------------ Level Results ------------------------\n");
            for (int i = 0; i < pulseResultsTopLevel.Length; i++)
            {
                Console.WriteLine("Index                                  : {0}", i);
                Console.WriteLine("Top Level (dBm)                        : {0}", pulseResultsTopLevel[i]);
                Console.WriteLine("Base Level (dBm)                       : {0}", pulseResultsBaseLevel[i]);
                Console.WriteLine("Average On Level (dBm)                 : {0}", pulseResultsAverageOnLevel[i]);
                Console.WriteLine("Overshoot (%)                          : {0}", pulseResultsOvershoot[i]);
                Console.WriteLine("Droop (%)                              : {0}", pulseResultsDroop[i]);
                Console.WriteLine("Ripple (%)                             : {0}", pulseResultsRipple[i]);
                Console.WriteLine("---------------------------------------------------------\n");
            }

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


