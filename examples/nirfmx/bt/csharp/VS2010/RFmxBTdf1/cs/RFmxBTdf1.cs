//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Packet Type.
//6. Configure Data Rate.
//7. Configure Payload Bit pattern.
//8. Configure Payload Length.
//9. Configure Direction Finding.
//10. Select ModAcc measurement and enable Traces.
//11. Configure ModAcc Burst Synchronization Type.
//12. Configure Averaging Parameters for ModAcc measurement.
//13. Initiate the Measurement.
//14. Fetch ModAcc Measurements and Trace.
//15. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.BTMX;

namespace NationalInstruments.Examples.RFmxBTdf1
{
    public class RFmxBTdf1
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

        RFmxBTMXPacketType packetType;
        int LEDataRate;

        RFmxBTMXPayloadBitPattern payloadBitPattern;
        RFmxBTMXPayloadLengthMode payloadLengthMode;
        int payloadLength;

        RFmxBTMXDirectionFindingMode directionFindingMode;
        double cteLength;
        double cteSlotDuration;

        RFmxBTMXModAccBurstSynchronizationType burstSynchronizationType;

        RFmxBTMXMeasurementTypes measurement;
        bool enableAllTraces;

        RFmxBTMXModAccAveragingEnabled averagingEnabled;
        int averagingCount;

        double timeout;
        double df1avgMaximum;                                                       /*(Hz) */
        double df1avgMinimum;                                                       /*(Hz) */

        double peakFrequencyErrorMaximum;                                           /*(Hz) */
        double initialFrequencyDriftMaximum;                                        /*(Hz) */
        double peakFrequencyDriftMaximum;                                           /*(Hz) */
        double peakFrequencyDriftRateMaximum;                                       /*(Hz) */

        float[] time, df1max;
        float[] timeLE, frequencyErrorLE;

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

            packetType = RFmxBTMXPacketType.PacketTypeDH1;
            LEDataRate = 1000000;                                                   /*bps */

            payloadBitPattern = RFmxBTMXPayloadBitPattern.Pattern11110000;
            payloadLengthMode = RFmxBTMXPayloadLengthMode.Auto;
            payloadLength = 10;                                                      /*bytes */

            directionFindingMode = RFmxBTMXDirectionFindingMode.Disabled;
            cteLength = 160e-6;                                                     /*seconds */
            cteSlotDuration = 1e-6;                                                 /*seconds */

            burstSynchronizationType = RFmxBTMXModAccBurstSynchronizationType.Preamble;

            measurement = RFmxBTMXMeasurementTypes.ModAcc;
            enableAllTraces = true;

            averagingEnabled = RFmxBTMXModAccAveragingEnabled.False;
            averagingCount = 10;

            timeout = 10.0;                                                         /*seconds */
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
            BT.ConfigurePacketType("", packetType);
            BT.ConfigureDataRate("", LEDataRate);
            BT.ConfigurePayloadBitPattern("", payloadBitPattern);
            BT.ConfigurePayloadLength("", payloadLengthMode, payloadLength);
            BT.ConfigureLEDirectionFinding("", directionFindingMode, cteLength, cteSlotDuration);
            BT.SelectMeasurements("", measurement, enableAllTraces);
            BT.ModAcc.Configuration.ConfigureBurstSynchronizationType("", burstSynchronizationType);
            BT.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            BT.Initiate("", "");
        }

        void RetrieveResults()
        {
            BT.ModAcc.Results.FetchDf1("", timeout, out df1avgMaximum, out df1avgMinimum);
            BT.ModAcc.Results.FetchFrequencyErrorLE("", timeout, out peakFrequencyErrorMaximum,
                out initialFrequencyDriftMaximum, out peakFrequencyDriftMaximum, out peakFrequencyDriftRateMaximum);
            BT.ModAcc.Results.FetchDf1maxTrace("", timeout, ref time, ref df1max);
            BT.ModAcc.Results.FetchFrequencyErrorTraceLE("", timeout, ref timeLE, ref frequencyErrorLE);
        }

        void PrintResults()
        {
            Console.WriteLine("------------------df1 Measurement------------------");
            Console.WriteLine("df1avg Maximum (Hz)                             : {0}", df1avgMaximum);
            Console.WriteLine("df1avg Minimum (Hz)                             : {0}\n", df1avgMinimum);

            Console.WriteLine("------------------LE Frequency Error------------------");
            Console.WriteLine("Peak Frequency Error Maximum (Hz)               : {0}", peakFrequencyErrorMaximum);
            Console.WriteLine("Initial Frequency Drift Maximum  (Hz)           : {0}", initialFrequencyDriftMaximum);
            Console.WriteLine("Peak Frequency Drift Maximum (Hz)               : {0}", peakFrequencyDriftMaximum);
            Console.WriteLine("Peak Frequency Drift RateMaximum  (Hz)          : {0}", peakFrequencyDriftRateMaximum);
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