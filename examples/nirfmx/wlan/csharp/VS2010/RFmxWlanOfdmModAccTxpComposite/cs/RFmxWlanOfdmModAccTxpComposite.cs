//Steps:
//1. Open a new RFmx session.
//2. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//5. Configure Standard and Channel Bandwidth Properties.
//6. Select OFDMModAcc and TXP measurements.
//7. Configure the Measurement Interval.
//8. Configure Averaging parameters for OFDMModAcc.
//9. Configure Averaging parameters for TXP.
//10. Configure the Maximum Measurement Interval.
//11. Initiate Measurement.
//12. Fetch OFDMModAcc Measurement.
//13. Fetch TXP Measurement.
//14. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WlanMX;

namespace NationalInstruments.Examples.RFmxWlanOfdmModAccTxpComposite
{
    public class RFmxWlanOfdmModAccTxpComposite
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

        double channelBandwidth;

        int measurementOffset;
        int maximumMeasurementLength;

        RFmxWlanMXOfdmModAccAveragingEnabled ofdmModAccAveragingEnabled;
        int ofdmModAccAveragingCount;

        RFmxWlanMXTxpAveragingEnabled txpAveragingEnabled;
        int txpAveragingCount;

        double timeout;

        double maximumMeasurementInterval;

        double compositeRmsEvmMean;
        double compositeDataRmsEvmMean;
        double compositePilotRmsEvmMean;

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

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e6;                                     /* (Hz) */

            iqPowerEdgeEnabled = true;
            iqPowerEdgeLevel = -20.0;                                               /*(dB) */
            triggerDelay = 0.0;                                                     /* (s) */
            minimumQuietTimeMode = RFmxWlanMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 5.0e-6;                                              /* (s) */

            standard = RFmxWlanMXStandard.Standard802_11ag;

            channelBandwidth = 20e6;                                                /*(Hz) */

            measurementOffset = 0;                                                  /* (symbols) */
            maximumMeasurementLength = 16;                                          /* (symbols) */

            ofdmModAccAveragingEnabled = RFmxWlanMXOfdmModAccAveragingEnabled.False;
            ofdmModAccAveragingCount = 10;

            txpAveragingEnabled = RFmxWlanMXTxpAveragingEnabled.False;
            txpAveragingCount = 10;

            maximumMeasurementInterval = 1e-3;                                      /* (s)*/

            timeout = 10.0;                                                         /* (s)*/

            compositeRmsEvmMean = 0.0;                                              /* (dB)*/
            compositeDataRmsEvmMean = 0.0;                                          /* (dB)*/
            compositePilotRmsEvmMean = 0.0;                                         /* (dB)*/

            averagePowerMean = 0;                                                   /* (dBm)*/
            peakPowerMaximum = 0;                                                   /* (dBm)*/
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
            wlan.ConfigureChannelBandwidth("", channelBandwidth);
            wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.Txp | RFmxWlanMXMeasurementTypes.OfdmModAcc, true);
            wlan.OfdmModAcc.Configuration.ConfigureMeasurementLength("", measurementOffset, maximumMeasurementLength);
            wlan.OfdmModAcc.Configuration.ConfigureAveraging("", ofdmModAccAveragingEnabled, ofdmModAccAveragingCount);
            wlan.Txp.Configuration.ConfigureAveraging("", txpAveragingEnabled, txpAveragingCount);
            wlan.Txp.Configuration.ConfigureMaximumMeasurementInterval("", maximumMeasurementInterval);
            wlan.Initiate("", "");
        }

        void RetrieveResults()
        {
            wlan.OfdmModAcc.Results.FetchCompositeRmsEvm("", timeout, out compositeRmsEvmMean,
                out compositeDataRmsEvmMean, out compositePilotRmsEvmMean);
            wlan.Txp.Results.FetchMeasurement("", timeout, out averagePowerMean, out peakPowerMaximum);
        }

        void PrintResults()
        {
            Console.WriteLine("------------------OFDMModAcc Measurement------------------\n");
            Console.WriteLine("RMS EVM Mean (dB)                       :{0}", compositeRmsEvmMean);
            Console.WriteLine("Data RMS EVM Mean (dB)                  :{0}", compositeDataRmsEvmMean);
            Console.WriteLine("Pilot RMS EVM Mean (dB)                 :{0}", compositePilotRmsEvmMean);
            Console.WriteLine("\n----------TXP Measurement----------\n");
            Console.WriteLine("Average Power Mean (dBm)                :{0}", averagePowerMean);
            Console.WriteLine("Peak Power Maximum (dBm)                :{0}", peakPowerMaximum);
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
