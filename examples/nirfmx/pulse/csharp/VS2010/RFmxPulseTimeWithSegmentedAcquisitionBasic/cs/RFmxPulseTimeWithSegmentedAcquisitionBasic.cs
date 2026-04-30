//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic RF signal properties(Center Frequency, RF Attenuation and External Attenuation)
//4. Enabling Pulse and Disabling Traces Result
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Bandwidth and Filter Type.
//7. Configure Segmented Acquisition Settings.
//8. Configure Pulse Detection Settings.
//9. Configure State and Thershold Level Settings.
//10. Configure Pulse Metrics and Pulse Stability enabled settings.
//11. Initiate the Measurement.
//12. Wait for Measurement to complete.
//13. Fetch Pulse Count and Timing Results.
//14. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.PulseMX;

namespace NationalInstruments.Examples.RFmxPulseTimeWithSegmentedAcquisitionBasic
{
    public class RFmxPulseTimeWithSegmentedAcquisitionBasic
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
        double segmentAcquisitionLength;
        RFmxPulseMXSegmentedAcquisitionEnabled segmentAcquisitionLengthEnabled;
        int numberOfSegments;

        RFmxPulseMXPulseDetectionReference pulseDetectionReference;
        double pulseDetectionThreshold;
        double pulseDetectionHysteresis;
        double pulseDetectionMinimumOffDuration;

        RFmxPulseMXPulseLevelComputationMethod pulseLevelComputationMethod;
        RFmxPulseMXPulseAmplitudeLevelDomain pulseAmplitudeLevelDomain;
        double pulseUpperThresholdLevel;
        double pulseWidthThresholdLevel;
        double pulseLowerThresholdLevel;
        double timeout; 
                                                              /* (s) */
        int pulseCount;
        double[] pulseResultsRiseTime;                                        /* (s) */
        double[] pulseResultsFallTime;                                        /* (s) */
        double[] pulseResultsPulseWidth;                                      /* (s) */
        double[] pulseResultsPulseOffDuration;                                /* (s) */
        double[] pulseResultsDutyCycle;                                       /* (%) */
        double[] pulseResultsPulseRepetitionInterval;                         /* (s) */



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
            segmentAcquisitionLength = 15.0e-6;                                      /* (s) */
            numberOfSegments = 100;
            segmentAcquisitionLengthEnabled = RFmxPulseMXSegmentedAcquisitionEnabled.True;

            pulseDetectionReference = RFmxPulseMXPulseDetectionReference.ReferenceLevel;
            pulseDetectionThreshold = -20.0;
            pulseDetectionHysteresis = 1.0;                                   /* (dB) */
            pulseDetectionMinimumOffDuration = 50.0e-9;                                /* (s) */

            pulseLevelComputationMethod = RFmxPulseMXPulseLevelComputationMethod.Median;
            pulseAmplitudeLevelDomain = RFmxPulseMXPulseAmplitudeLevelDomain.Volts;
            pulseUpperThresholdLevel = 90.0;                                   /* (%) */
            pulseWidthThresholdLevel = 50.0;                                   /* (%) */
            pulseLowerThresholdLevel = 10.0;                                   /* (%) */


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
            Pulse.SetSegmentedAcquisitionEnabled("", segmentAcquisitionLengthEnabled);
            Pulse.SetNumberOfSegments("", numberOfSegments);
            Pulse.SetAcquisitionLength("", segmentAcquisitionLength);

            Pulse.Pulse.Configuration.SetDetectionReference("", pulseDetectionReference);
            Pulse.Pulse.Configuration.SetDetectionThreshold("", pulseDetectionThreshold);
            Pulse.Pulse.Configuration.SetDetectionHysteresis("", pulseDetectionHysteresis);
            Pulse.Pulse.Configuration.SetDetectionMinimumOffDuration("", pulseDetectionMinimumOffDuration);

            Pulse.Pulse.Configuration.SetLevelComputationMethod("", pulseLevelComputationMethod);
            Pulse.Pulse.Configuration.SetAmplitudeLevelDomain("", pulseAmplitudeLevelDomain);
            Pulse.Pulse.Configuration.SetUpperThresholdLevel("", pulseUpperThresholdLevel);
            Pulse.Pulse.Configuration.SetWidthThresholdLevel("", pulseWidthThresholdLevel);
            Pulse.Pulse.Configuration.SetLowerThresholdLevel("", pulseLowerThresholdLevel);

            Pulse.Pulse.Configuration.SetMetricsEnabled("", RFmxPulseMXPulseMetricsEnabled.True);
            Pulse.Pulse.Configuration.SetStabilityEnabled("", RFmxPulseMXPulseStabilityEnabled.False);

            Pulse.Initiate("", "");

            Pulse.WaitForMeasurementComplete("", timeout);
        }

        void RetrieveResults()
        {
            Pulse.Pulse.Results.GetPulseCount("", out pulseCount);
            
            Pulse.Pulse.Results.GetRiseTime("", ref pulseResultsRiseTime);
            Pulse.Pulse.Results.GetFallTime("", ref pulseResultsFallTime);  
            Pulse.Pulse.Results.GetPulseWidth("", ref pulseResultsPulseWidth);
            Pulse.Pulse.Results.GetPulseOffDuration("", ref pulseResultsPulseOffDuration);
            Pulse.Pulse.Results.GetDutyCycle("", ref pulseResultsDutyCycle);
            Pulse.Pulse.Results.GetPulseRepetitionInterval("", ref pulseResultsPulseRepetitionInterval);
        }

        void PrintResults()
        {
            Console.WriteLine("\nPulse Count                            : {0}\n", pulseCount);
            Console.WriteLine("\n------------------- Timing Results ----------------------\n");
            for (int i = 0; i < pulseResultsRiseTime.Length; i++)
            {
                Console.WriteLine("Index                                  : {0}", i);
                Console.WriteLine("Rise Time (s)                          : {0:F11}", pulseResultsRiseTime[i]);
                Console.WriteLine("Fall Time (s)                          : {0:F11}", pulseResultsFallTime[i]);
                Console.WriteLine("Pulse Width (s)                        : {0:F9}", pulseResultsPulseWidth[i]);
                Console.WriteLine("Pulse Off Duration (s)                 : {0:F9}", pulseResultsPulseOffDuration[i]);
                Console.WriteLine("Duty Cycle (%)                         : {0:F9}", pulseResultsDutyCycle[i]);
                Console.WriteLine("Pulse repetition Interval (s)          : {0:F9}", pulseResultsPulseRepetitionInterval[i]);
                Console.WriteLine("-------------------------------------------------------\n");
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


