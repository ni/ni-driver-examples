//Steps:
//1. Open a new RFmx session.
//2. Configure the frequency reference properties (Clock Source and Clock Frequency).
//3. Configure Number of Frequency Segment and Receive Chain.
//4. Configure the Center Frequency for each Segment.
//5. Configure Selected Port.
//6. Configure Standard and Channel Bandwidth Properties.
//7. Configure Reference Level.
//8. Configure the External Attenuation.
//9. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//10. Select TXP measurement and enable the traces.
//11. Configure the Measurement Interval.
//12. Configure Averaging parameters.
//13. Initiate Measurement.
//14. Fetch TXP Traces and Measurements.
//15. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WlanMX;

namespace NationalInstruments.Examples.RFmxWlanTxpMimo
{
    public class RFmxWlanTxpMimo
    {
        RFmxInstrMX instrSession;
        RFmxWlanMX wlan;
        string[] resourceName;
        int numberOfDevices;

        string[] selectedPorts;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        int numberOfFrequencySegments;
        int numberOfReceiveChains;

        string segmentString;
        string chainString;

        double[] centerFrequency;

        string[] portString;

        string[] selectedPortsString;

        RFmxWlanMXStandard standard;

        double channelBandwidth;

        bool autoLevel;
        double measurementInterval;
        double[] referenceLevel;
        double[] externalAttenuation;

        bool iqPowerEdgeEnabled;
        double iqPowerEdgeLevel;
        double triggerDelay;
        RFmxWlanMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        double minimumQuietTime;

        RFmxWlanMXTxpAveragingEnabled averagingEnabled;
        int averagingCount;

        double timeout;

        double maximumMeasurementInterval;

        AnalogWaveform<float>[,] power;

        double[,] averagePowerMean, peakPowerMaximum;

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
            resourceName = new string[] { "RFSA1", "RFSA2" };
            numberOfDevices = resourceName.GetLength(0);

            selectedPorts = new string[] { "", "" };

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock;
            frequencyReferenceFrequency = 10e6;                                     /* (Hz) */

            numberOfFrequencySegments = 1;
            numberOfReceiveChains = 2;

            centerFrequency = new double[] { 5.180000e9, 5.260000e9 };              /* (Hz) */

            portString = new string[numberOfDevices];

            selectedPortsString = new string[numberOfDevices];

            standard = RFmxWlanMXStandard.Standard802_11n;

            channelBandwidth = 20e6;                                                /* (Hz) */

            autoLevel = true;
            measurementInterval = 10e-3;                                            /* (s) */
            referenceLevel = new double[] { 0.0, 0.0 };                             /* (dBm) */
            externalAttenuation = new double[] { 0.0, 0.0 };                        /* (dB) */

            iqPowerEdgeEnabled = true;
            iqPowerEdgeLevel = -20.0;                                               /* (dB) */
            triggerDelay = 0.0;                                                     /* (s) */
            minimumQuietTimeMode = RFmxWlanMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 5.0e-6;                                              /* (s) */

            averagingEnabled = RFmxWlanMXTxpAveragingEnabled.False;
            averagingCount = 10;

            maximumMeasurementInterval = 1e-3;                                      /* (s) */

            timeout = 10.0;                                                         /* (s) */

            power = new AnalogWaveform<float>[numberOfFrequencySegments, numberOfReceiveChains];
            averagePowerMean = new double[numberOfFrequencySegments, numberOfReceiveChains];                                                   /* (dBm) */
            peakPowerMaximum = new double[numberOfFrequencySegments, numberOfReceiveChains];                                                   /* (dBm) */
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureWlan()
        {
            wlan = instrSession.GetWlanSignalConfiguration();     /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            wlan.ConfigureNumberOfFrequencySegmentsAndReceiveChains("", numberOfFrequencySegments, numberOfReceiveChains);
            for (int i = 0; i < numberOfFrequencySegments; ++i)
            {
                segmentString = RFmxWlanMX.BuildSegmentString("", i);
                wlan.ConfigureFrequency(segmentString, centerFrequency[i]);
            }
            for (int i = 0; i < numberOfDevices; ++i)
            {
                selectedPortsString[i] = RFmxInstrMX.BuildPortString2("", selectedPorts[i], resourceName[i], 0);
                portString[i] = RFmxInstrMX.BuildPortString2("", "", resourceName[i], 0);
            }
            wlan.ConfigureSelectedPortsMultiple("", selectedPortsString);
            wlan.ConfigureStandard("", standard);
            wlan.ConfigureChannelBandwidth("", channelBandwidth);
            if (autoLevel)
            {
                wlan.AutoLevel("", measurementInterval);
            }
            else
            {
                for (int i = 0; i < numberOfDevices; ++i)
                {
                    wlan.ConfigureReferenceLevel(portString[i], referenceLevel[i]);
                }
            }
            for (int i = 0; i < numberOfDevices; ++i)
            {
                wlan.ConfigureExternalAttenuation(portString[i], externalAttenuation[i]);
            }
            wlan.ConfigureIQPowerEdgeTrigger("", "0", RFmxWlanMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
               triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxWlanMXIQPowerEdgeTriggerLevelType.Relative,
               iqPowerEdgeEnabled);
            wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.Txp, true);
            wlan.Txp.Configuration.ConfigureMaximumMeasurementInterval("", maximumMeasurementInterval);
            wlan.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            wlan.Initiate("", "");
        }

        void RetrieveResults()
        {
            for (int i = 0; i < numberOfFrequencySegments; ++i)
            {
                segmentString = RFmxWlanMX.BuildSegmentString("", i);
                for (int j = 0; j < numberOfReceiveChains; ++j)
                {
                    chainString = RFmxWlanMX.BuildChainString(segmentString, j);
                    wlan.Txp.Results.FetchMeasurement(chainString, timeout, out averagePowerMean[i, j], out peakPowerMaximum[i, j]);
                    wlan.Txp.Results.FetchPowerTrace(chainString, timeout, ref power[i, j]);
                }
            }
        }

        void PrintResults()
        {
            for (int i = 0; i < numberOfFrequencySegments; ++i)
            {
                segmentString = RFmxWlanMX.BuildSegmentString("", i);
                for (int j = 0; j < numberOfReceiveChains; ++j)
                {
                    chainString = RFmxWlanMX.BuildChainString(segmentString, j);
                    Console.WriteLine("\n----------Measurement for {0}----------\n", chainString);
                    Console.WriteLine("Average Power Mean (dBm)         :{0}", averagePowerMean[i, j]);
                    Console.WriteLine("Peak Power Maximum (dBm)         :{0}", peakPowerMaximum[i, j]);
                    Console.WriteLine();
                }
            }
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
