//Steps:
//1. Open a new RFmx session.
//2. Configure the frequency reference properties (Clock Source and Clock Frequency).
//3. Configure the basic signal properties (Center Frequency and External Attenuation).
//4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//5. Configure Standard and Channel Bandwidth Properties.
//6. Configure Reference Level.
//7. Select TXP measurement and enable the traces.
//8. Configure the Measurement Interval.
//9. Configure Averaging parameters.
//10. Initiate Measurement.
//11. Fetch TXP Traces and Measurements.
//12. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WlanMX;

namespace NationalInstruments.Examples.RFmxWlanTxp
{
    public class RFmxWlanTxp
    {
        RFmxInstrMX instrSession;
        RFmxWlanMX wlan;
        string resourceName;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;
        bool autoLevel;
        double measurementInterval;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        bool iqPowerEdgeEnabled;
        double iqPowerEdgeLevel;
        double triggerDelay;
        RFmxWlanMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        double minimumQuietTime;

        RFmxWlanMXStandard standard;

        double channelBandwidth;

        RFmxWlanMXTxpAveragingEnabled averagingEnabled;
        int averagingCount;

        double timeout;

        double maximumMeasurementInterval;

        AnalogWaveform<float> power;

        double averagePowerMean, peakPowerMaximum;

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
            autoLevel = true;
            measurementInterval = 10e-3;                                            /* (s) */

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e6;                                     /* (Hz) */

            iqPowerEdgeEnabled = true;
            iqPowerEdgeLevel = -20.0;                                               /* (dB) */
            triggerDelay = 0.0;                                                     /* (s) */
            minimumQuietTimeMode = RFmxWlanMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 5.0e-6;                                              /* (s) */

            standard = RFmxWlanMXStandard.Standard802_11ag;

            channelBandwidth = 20e6;                                                /* (Hz) */

            averagingEnabled = RFmxWlanMXTxpAveragingEnabled.False;
            averagingCount = 10;

            maximumMeasurementInterval = 1e-3;                                      /* (s) */

            timeout = 10.0;                                                         /* (s) */

            averagePowerMean = 0;                                                   /* (dBm) */
            peakPowerMaximum = 0;                                                   /* (dBm) */
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
            wlan.ConfigureExternalAttenuation("", externalAttenuation);
            wlan.ConfigureIQPowerEdgeTrigger("", "0", RFmxWlanMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
               triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxWlanMXIQPowerEdgeTriggerLevelType.Relative,
               iqPowerEdgeEnabled);
            wlan.ConfigureStandard("", standard);
            wlan.ConfigureChannelBandwidth("", channelBandwidth);
            if (autoLevel)
            {
                wlan.AutoLevel("", measurementInterval);
            }
            else
            {
                wlan.ConfigureReferenceLevel("", referenceLevel);
            }
            wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.Txp, true);
            wlan.Txp.Configuration.ConfigureMaximumMeasurementInterval("", maximumMeasurementInterval);
            wlan.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            wlan.Initiate("", "");
        }

        void RetrieveResults()
        {
            wlan.Txp.Results.FetchPowerTrace("", timeout, ref power);
            wlan.Txp.Results.FetchMeasurement("", timeout, out averagePowerMean, out peakPowerMaximum);
        }

        void PrintResults()
        {
            Console.WriteLine("\n----------Measurement----------\n");
            Console.WriteLine("Average Power Mean (dBm)         :{0}", averagePowerMean);
            Console.WriteLine("Peak Power Maximum (dBm)         :{0}", peakPowerMaximum);
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
