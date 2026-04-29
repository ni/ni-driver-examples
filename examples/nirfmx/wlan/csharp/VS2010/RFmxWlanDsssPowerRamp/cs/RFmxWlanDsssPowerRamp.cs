//Steps:
//1. Open a new RFmx session.
//2. Configure the frequency reference properties (Clock Source and Clock Frequency).
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//5. Configure Standard as 802.11b.
//6. Select PowerRamp measurement and enable the traces.
//7. Configure the Acquisition Length.
//8. Configure Averaging parameters.
//9. Initiate Measurement.
//10. Fetch PowerRamp Traces and Measurements.
//11. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WlanMX;

namespace NationalInstruments.Examples.RFmxWlanDsssPowerRamp
{
    public class RFmxWlanDsssPowerRamp
    {
        RFmxInstrMX instrSession;
        RFmxWlanMX wlan;
        string resourceName;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        bool iqPowerEdgeEnabled;
        double iqPowerEdgeLevel;
        double triggerDelay;
        RFmxWlanMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        double minimumQuietTime;

        RFmxWlanMXStandard standard;

        double acquisitionLength;

        RFmxWlanMXPowerRampAveragingEnabled averagingEnabled;
        int averagingCount;

        double timeout;

        double fallTimeMean;
        double riseTimeMean;

        AnalogWaveform<float> riseTraceRawWaveform;
        AnalogWaveform<float> riseTraceProcessesWaveform;
        AnalogWaveform<float> riseTraceThreshold;
        AnalogWaveform<float> riseTracePowerReference;

        AnalogWaveform<float> fallTraceRawWaveform;
        AnalogWaveform<float> fallTraceProcessesWaveform;
        AnalogWaveform<float> fallTraceThreshold;
        AnalogWaveform<float> fallTracePowerReference;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureWlan();
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

            centerFrequency = 2.412e9;                                              /* (Hz) */
            referenceLevel = 0.0;                                                   /* (dBm) */
            externalAttenuation = 0.0;                                              /* (dB) */

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e6;                                     /* (Hz) */

            iqPowerEdgeEnabled = true;
            iqPowerEdgeLevel = -20.0;                                               /*(dB) */
            triggerDelay = 0.0;                                                     /* (s) */
            minimumQuietTimeMode = RFmxWlanMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 5.0e-6;                                              /* (s) */

            standard = RFmxWlanMXStandard.Standard802_11b;

            acquisitionLength = 1e-3;                                               /* (s) */

            averagingEnabled = RFmxWlanMXPowerRampAveragingEnabled.False;
            averagingCount = 10;

            timeout = 10.0;                                                         /* (s) */
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureWlan()
        {
            wlan = instrSession.GetWlanSignalConfiguration();     /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            wlan.ConfigureFrequency("", centerFrequency);
            wlan.ConfigureReferenceLevel("", referenceLevel);
            wlan.ConfigureExternalAttenuation("", externalAttenuation);
            wlan.ConfigureIQPowerEdgeTrigger("", "0", RFmxWlanMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
               triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxWlanMXIQPowerEdgeTriggerLevelType.Relative,
               iqPowerEdgeEnabled);
            wlan.ConfigureStandard("", standard);
            wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.PowerRamp, true);
            wlan.PowerRamp.Configuration.ConfigureAcquisitionLength("", acquisitionLength);
            wlan.PowerRamp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            wlan.Initiate("", "");
        }

        void RetrieveResults()
        {
            wlan.PowerRamp.Results.FetchMeasurement("", timeout, out riseTimeMean, out fallTimeMean);
            wlan.PowerRamp.Results.FetchRiseTrace("", timeout, ref riseTraceRawWaveform, ref riseTraceProcessesWaveform,
                ref riseTraceThreshold, ref riseTracePowerReference);
            wlan.PowerRamp.Results.FetchFallTrace("", timeout, ref fallTraceRawWaveform, ref fallTraceProcessesWaveform,
                ref fallTraceThreshold, ref fallTracePowerReference);
        }

        void PrintResults()
        {
            Console.WriteLine("\n---------------Measurement---------------\n");
            Console.WriteLine("Rise Time (s)                     :{0}", riseTimeMean);
            Console.WriteLine("Fall Time (s)                     :{0}", fallTimeMean);
        }

        void CloseSession()
        {
            if (wlan != null)
            {
                wlan.Dispose();
                wlan = null;
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
