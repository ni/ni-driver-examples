//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure LO Leakage Avoidance Enabled, LO source and Downconverter Frequency Offset(Hz).
//4. Configure basic RF signal properties(Center Frequency, RF Attenuation and External Attenuation)
//5. Enabling Pulse and Traces Result.
//6. Configure Trigger Type and Trigger Parameters.
//7. Configure Acquisition Settings.
//8. Configure Measurement Point Settings.
//9. Configure Stability Settings.
//10. Configure Selected Traces setting and enabling Pulse Stability enabled and disable Pulse Metrics enabled settings.
//11. Initiate the Measurement.
//12. Wait for Measurement to complete.
//13. Results Average Stability, Per Pulse Stability Measurements, Stability and Pulse to Pulse Stability Traces.
//14. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.PulseMX;

namespace NationalInstruments.Examples.RFmxPulseStabilityBasic
{
    public class RFmxPulseStabilityBasic
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

        RFmxInstrMXFrequencySettlingUnits frequencySettlingUnits;
        double frequencySettling;

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

        RFmxPulseMXPulseMeasurementPointReference pulseMeasurementPointReference;
        double pulseMeasurementPointOffset;
        double pulseMeasurementPointAveragingDuration;

        int pulseStabilityMeasurementOffset;
        int pulseStabilityReferenceOffset;
        int pulseStabilityPulseToPulseOffset;
        RFmxPulseMXPulseStabilityFrequencyErrorCompensation pulseStabilityFrequencyErrorCompensation;

        int pulseSelectedPulseTrace;

        double timeout;                                                       /* (s) */

        double averageAmplitudeStability;                                     /* (dB) */
        double averagePhaseStability;                                         /* (dB) */
        double averageTotalStability;                                         /* (dB) */

        double[] amplitudeStability;                                          /* (dB) */
        double[] phaseStability;                                              /* (dB) */
        double[] totalStability;                                              /* (dB) */
        
        AnalogWaveform<float> pulseAmplitudeStability;                        /* (dB) */
        AnalogWaveform<float> pulsePhaseStability;                            /* (dB) */
        AnalogWaveform<float> pulseTotalStability;                            /* (dB) */

        int[] pulseIndex;
        double[] pulseToPulseAmplitudeStability;                              /* (dB) */
        double[] pulseToPulsePhaseStability;                                  /* (dB) */
        double[] pulseToPulseTotalStability;                                  /* (dB) */

        

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

            downConverterFrequencyOffset = 0.0;                               /* (Hz) */

            LOLeakageAvoidanceEnabled = RFmxInstrMXLOLeakageAvoidanceEnabled.True;
            LOSource = RFmxInstrMXConstants.LOSourceOnboard;

            frequencySettlingUnits = RFmxInstrMXFrequencySettlingUnits.Ppm;
            frequencySettling = 1.0e-1;                                       /* (s) */

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

            pulseMeasurementPointReference = RFmxPulseMXPulseMeasurementPointReference.Center;
            pulseMeasurementPointOffset = 0.0;                                /* (s) */
            pulseMeasurementPointAveragingDuration = 0.0;                     /* (s) */

            pulseStabilityMeasurementOffset = 0;
            pulseStabilityReferenceOffset = 0;
            pulseStabilityPulseToPulseOffset = 1;
            pulseStabilityFrequencyErrorCompensation = RFmxPulseMXPulseStabilityFrequencyErrorCompensation.On;

            pulseSelectedPulseTrace = 0;

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
            instrSession.SetFrequencySettlingUnits("", frequencySettlingUnits);
            instrSession.SetFrequencySettling("", frequencySettling);
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

            Pulse.Pulse.Configuration.SetMeasurementPointReference("", pulseMeasurementPointReference);
            Pulse.Pulse.Configuration.SetMeasurementPointOffset("", pulseMeasurementPointOffset);
            Pulse.Pulse.Configuration.SetMeasurementPointAveragingDuration("", pulseMeasurementPointAveragingDuration);

            Pulse.Pulse.Configuration.SetStabilityMeasurementOffset("", pulseStabilityMeasurementOffset);
            Pulse.Pulse.Configuration.SetStabilityReferenceOffset("", pulseStabilityReferenceOffset);
            Pulse.Pulse.Configuration.SetStabilityPulseToPulseOffset("", pulseStabilityPulseToPulseOffset);
            Pulse.Pulse.Configuration.SetStabilityFrequencyErrorCompensation("", pulseStabilityFrequencyErrorCompensation);
            Pulse.Pulse.Configuration.SetSelectedPulseTrace("", pulseSelectedPulseTrace);
            Pulse.Pulse.Configuration.SetMetricsEnabled("", RFmxPulseMXPulseMetricsEnabled.False);
            Pulse.Pulse.Configuration.SetStabilityEnabled("", RFmxPulseMXPulseStabilityEnabled.True);

            Pulse.Initiate("", "");

            Pulse.WaitForMeasurementComplete("", timeout);
        }

        void RetrieveResults()
        {
            Pulse.Pulse.Results.GetAverageAmplitudeStability("", out averageAmplitudeStability);
            Pulse.Pulse.Results.GetAveragePhaseStability("", out averagePhaseStability);
            Pulse.Pulse.Results.GetAverageTotalStability("", out averageTotalStability);

            Pulse.Pulse.Results.GetAmplitudeStability("", ref amplitudeStability);
            Pulse.Pulse.Results.GetPhaseStability("", ref phaseStability);
            Pulse.Pulse.Results.GetTotalStability("", ref totalStability);

            Pulse.Pulse.Results.FetchStabilityTrace("", timeout, ref pulseAmplitudeStability, ref pulsePhaseStability, 
                ref pulseTotalStability);

            Pulse.Pulse.Results.FetchPulseToPulseStabilityTrace("", timeout, ref pulseIndex, 
                ref pulseToPulseAmplitudeStability, ref pulseToPulsePhaseStability, ref pulseToPulseTotalStability);
        }

        void PrintResults()
        {
            Console.WriteLine("\n----------------- Average Stability Results ---------------\n");
            Console.WriteLine("Average Amplitude Stability (dB)     : {0}", averageAmplitudeStability);
            Console.WriteLine("Average Phase Stability (dB)         : {0}", averagePhaseStability);
            Console.WriteLine("Average Total Stability (dB)         : {0}", averageTotalStability);

            Console.WriteLine("\n\n-----------------  Stability Results ----------------------\n");
            for (int i = 0; i < amplitudeStability.Length; i++)
            {
                Console.WriteLine("Index                                : {0}", i);
                Console.WriteLine("Amplitude Stability (dB)             : {0}", amplitudeStability[i]);
                Console.WriteLine("Phase Stability (dB)                 : {0}", phaseStability[i]);
                Console.WriteLine("Total Stability (dB)                 : {0}", totalStability[i]);
                Console.WriteLine("-----------------------------------------------------------\n");
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


