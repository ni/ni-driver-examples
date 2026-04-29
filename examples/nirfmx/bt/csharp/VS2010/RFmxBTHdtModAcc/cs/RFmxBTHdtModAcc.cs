//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Packet Type.
//6. Configure Payload Length.
//7. Select ModAcc measurement and enable Traces.
//8. Configure ModAcc Burst Synchronization Mode.
//9. Configure Averaging Parameters for ModAcc measurement.
//10. Initiate the Measurement.
//11. Fetch ModAcc Measurements and Trace.
//12. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.BTMX;

namespace NationalInstruments.Examples.RFmxBTHdtModAcc
{
    public class RFmxBTHdtModAcc
    {
        RFmxInstrMX instrSession;
        RFmxBTMX BT;
        string resourceName;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        bool enableTrigger;
        RFmxBTMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
        double iqPowerEdgeTriggerLevel;
        RFmxBTMXTriggerMinimumQuietTimeMode minimumQuiteTimeMode;
        double minimumQuietTime;
        RFmxBTMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
        double triggerDelay;

        RFmxBTMXPacketType packetType;
        int dataRate;

        int zadoffChuIndex;
        RFmxBTMXHighDataThroughputPacketFormat highDataThroughputPacketFormat;

        RFmxBTMXPayloadLengthMode payloadLengthMode;
        int payloadLength;

        RFmxBTMXModAccBurstSynchronizationType burstSynchronizationType;

        RFmxBTMXMeasurementTypes measurement;
        bool enableAllTraces;

        RFmxBTMXModAccAveragingEnabled averagingEnabled;
        int averagingCount;

        double timeout;
        double preambleRmsEvmMean;
        double controlHeaderRmsEvmMean;
        double payloadRmsEvmMean;

        float[] evmPerSymbol;
        ComplexSingle[] constellation;

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
            resourceName = "RFSA";

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e6;                                     /* (Hz) */

            centerFrequency = 2.402e9;                                              /* (Hz) */
            referenceLevel = 0.00;                                                  /* (dBm) */
            externalAttenuation = 0.0;                                              /* (dB) */

            enableTrigger = true;
            iqPowerEdgeTriggerSlope = RFmxBTMXIQPowerEdgeTriggerSlope.Rising;
            iqPowerEdgeTriggerLevel = -20.0;                                        /* (dB) */
            minimumQuiteTimeMode = RFmxBTMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 100e-6;                                              /*(seconds) */
            iqPowerEdgeTriggerLevelType = RFmxBTMXIQPowerEdgeTriggerLevelType.Relative;
            triggerDelay = 0.0;                                                     /*(seconds) */

            packetType = RFmxBTMXPacketType.PacketTypeLEHdt;
            dataRate = 2000000;														/*(bps) */

            zadoffChuIndex = 7;
            highDataThroughputPacketFormat = RFmxBTMXHighDataThroughputPacketFormat.Format0;

            payloadLengthMode = RFmxBTMXPayloadLengthMode.Auto;
            payloadLength = 10;                                                      /*(bytes) */

            burstSynchronizationType = RFmxBTMXModAccBurstSynchronizationType.Preamble;

            measurement = RFmxBTMXMeasurementTypes.ModAcc;
            enableAllTraces = true;

            averagingEnabled = RFmxBTMXModAccAveragingEnabled.False;
            averagingCount = 10;

            timeout = 10.0;                                                         /*(seconds) */
            preambleRmsEvmMean = 0.0;                                               /*(dB) */
            controlHeaderRmsEvmMean = 0.0;                                          /*(dB) */
            payloadRmsEvmMean = 0.0;                                                /*(dB) */

        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureBT()
        {
            BT = instrSession.GetBTSignalConfiguration();       /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            BT.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            BT.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel,
                triggerDelay, minimumQuiteTimeMode, minimumQuietTime, iqPowerEdgeTriggerLevelType,
                enableTrigger);
            BT.ConfigurePacketType("", packetType);
            BT.ConfigureDataRate("", dataRate);
            BT.SetZadoffChuIndex("", zadoffChuIndex);
            BT.SetHighDataThroughputPacketFormat("", highDataThroughputPacketFormat);
            BT.ConfigurePayloadLength("", payloadLengthMode, payloadLength);
            BT.SelectMeasurements("", measurement, enableAllTraces);
            BT.ModAcc.Configuration.ConfigureBurstSynchronizationType("", burstSynchronizationType);
            BT.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            BT.Initiate("", "");
        }

        void RetrieveResults()
        {
            BT.ModAcc.Results.GetPreambleRmsEvmMean("", out preambleRmsEvmMean);
            BT.ModAcc.Results.GetControlHeaderRmsEvmMean("",out controlHeaderRmsEvmMean);
            BT.ModAcc.Results.GetPayloadRmsEvmMean("", out payloadRmsEvmMean);
            BT.ModAcc.Results.FetchEvmPerSymbolTrace("", timeout, ref evmPerSymbol);
            BT.ModAcc.Results.FetchConstellationTrace("", timeout, ref constellation);
        }

        void PrintResults()
        {
            Console.WriteLine("------------------EVM------------------");
            Console.WriteLine("Preamble RMS EVM Mean (dB)                : {0}", preambleRmsEvmMean);
            Console.WriteLine("Control Header RMS EVM Mean (dB)          : {0}", controlHeaderRmsEvmMean);
            Console.WriteLine("Payload RMS EVM Mean (dB)                 : {0}", payloadRmsEvmMean);
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
