//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select FrequencyRange measurement and enable Traces.
//6. Configure Averaging Parameters for FrequencyRange measurement.
//7. Configure Span.
//8. Initiate the Measurement.
//9. Fetch FrequencyRange Measurements and Trace.
//10. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.BTMX;

namespace NationalInstruments.Examples.RFmxBTFrequencyRange
{
    public class RFmxBTFrequencyRange
    {
        RFmxInstrMX instrSession;
        RFmxBTMX BT;
        string rfsaResourceName;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        bool enableTrigger;
        RFmxBTMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
        double iqPowerEdgeLevel;
        RFmxBTMXTriggerMinimumQuietTimeMode minimumQuiteTimeMode;
        double minimumQuietTime;
        RFmxBTMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
        double triggerDelay;

        double span;

        RFmxBTMXMeasurementTypes measurement;
        bool enableAllTraces;

        RFmxBTMXFrequencyRangeAveragingEnabled averagingEnabled;
        int averagingCount;

        double timeout;
        double highFrequency;
        double lowFrequency;
        Spectrum<float> spectrum = null;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureBT();
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
            rfsaResourceName = "RFSA";

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e6;                                     /* (Hz) */

            centerFrequency = 2.402e9;                                              /* (Hz) */
            referenceLevel = 0.00;                                                  /* (dBm) */
            externalAttenuation = 0.0;                                              /* (dB) */

            enableTrigger = true;
            iqPowerEdgeTriggerSlope = RFmxBTMXIQPowerEdgeTriggerSlope.Rising;
            iqPowerEdgeLevel = -20.0;                                               /*dB */
            minimumQuiteTimeMode = RFmxBTMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 100e-6;                                              /*seconds */
            iqPowerEdgeTriggerLevelType = RFmxBTMXIQPowerEdgeTriggerLevelType.Relative;
            triggerDelay = 0.0;                                                     /*seconds */

            span = 10e06;                                                           /* (Hz) */

            measurement = RFmxBTMXMeasurementTypes.FrequencyRange;
            enableAllTraces = true;

            averagingEnabled = RFmxBTMXFrequencyRangeAveragingEnabled.False;
            averagingCount = 10;

            timeout = 10.0;                                                         /*seconds */
            highFrequency = 0.0;                                                    /*(Hz) */
            lowFrequency = 0.0;                                                     /*(Hz) */
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(rfsaResourceName, "");
        }

        void ConfigureBT()
        {
            BT = instrSession.GetBTSignalConfiguration();       /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            BT.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            BT.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeTriggerSlope, iqPowerEdgeLevel,
                triggerDelay, minimumQuiteTimeMode, minimumQuietTime, iqPowerEdgeTriggerLevelType,
                enableTrigger);
            BT.SelectMeasurements("", measurement, enableAllTraces);
            BT.FrequencyRange.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            BT.FrequencyRange.Configuration.ConfigureSpan("", span);
            BT.Initiate("", "");
        }

        void RetrieveResults()
        {
            BT.FrequencyRange.Results.FetchMeasurement("", timeout, out highFrequency, out lowFrequency);
            BT.FrequencyRange.Results.FetchSpectrum("", timeout, ref spectrum);
        }

        void PrintResults()
        {
            Console.WriteLine("------------------Measurement------------------");
            Console.WriteLine("High Frequency (Hz)                      : {0}", highFrequency);
            Console.WriteLine("Low Frequency (Hz)                       : {0}", lowFrequency);
        }

        void CloseSession()
        {
            if (BT != null)
            {
                BT.Dispose();
                BT = null;
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