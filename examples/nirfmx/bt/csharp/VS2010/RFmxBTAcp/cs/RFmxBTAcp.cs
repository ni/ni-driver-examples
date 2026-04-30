//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Packet Type.
//6. Configure Data Rate.
//7. Configure Payload Length.
//8. Select ACP measurement and enable Traces.
//9. Configure ACP Burst Sync Type.
//10. Configure Averaging Parameters for ACP measurement.
//11. Configure Number of Offsets Or Channel Number depending on Offset Channel Mode.
//12. Initiate the Measurement.
//13. Fetch ACP Measurements and Trace.
//14. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.BTMX;

namespace NationalInstruments.Examples.RFmxBTAcp
{
    public class RFmxBTAcp
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

        RFmxBTMXPayloadLengthMode payloadLengthMode;
        int payloadLength;

        RFmxBTMXAcpBurstSynchronizationType burstSynchronizationType;

        RFmxBTMXMeasurementTypes measurement;
        bool enableAllTraces;

        RFmxBTMXAcpAveragingEnabled averagingEnabled;
        int averagingCount;

        int numberOfOffsets;
        RFmxBTMXAcpOffsetChannelMode offsetChannelMode;
        int channelNumber;

        double timeout;

        RFmxBTMXAcpResultsMeasurementStatus measurementStatus;
        double referenceChannelPower;

        double[] lowerAbsolutePower;
        double[] upperAbsolutePower;
        double[] lowerRelativePower;
        double[] upperRelativePower;
        double[] lowerMargin;
        double[] upperMargin;

        Spectrum<float> limitWithExceptionMask = null;
        Spectrum<float> limitWithoutExceptionMask = null;

        Spectrum<float> absolutePower = null;

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

            packetType = RFmxBTMXPacketType.PacketTypeDH1;
            dataRate = 1000000;                                                     /*(bps) */

            payloadLengthMode = RFmxBTMXPayloadLengthMode.Auto;
            payloadLength = 10;                                                      /*(bytes) */

            burstSynchronizationType = RFmxBTMXAcpBurstSynchronizationType.Preamble;

            measurement = RFmxBTMXMeasurementTypes.Acp;
            enableAllTraces = true;

            averagingEnabled = RFmxBTMXAcpAveragingEnabled.False;
            averagingCount = 10;

            numberOfOffsets = 5;
            offsetChannelMode = RFmxBTMXAcpOffsetChannelMode.Symmetric;
            channelNumber = 0;
            timeout = 10.0;                                                         /*seconds */

            measurementStatus = RFmxBTMXAcpResultsMeasurementStatus.Fail;
            referenceChannelPower = 0.0;                                            /*(dBm)*/
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
            BT.ConfigurePayloadLength("", payloadLengthMode, payloadLength);
            BT.SelectMeasurements("", measurement, enableAllTraces);
            BT.Acp.Configuration.ConfigureBurstSynchronizationType("", burstSynchronizationType);
            BT.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            BT.Acp.Configuration.ConfigureOffsetChannelMode("", offsetChannelMode);
            if (offsetChannelMode == RFmxBTMXAcpOffsetChannelMode.Symmetric)
            {
                BT.Acp.Configuration.ConfigureNumberOfOffsets("", numberOfOffsets);
            }
            else if (offsetChannelMode == RFmxBTMXAcpOffsetChannelMode.InBand)
            {
                BT.ConfigureChannelNumber("", channelNumber);
            }
            BT.Initiate("", "");
        }

        void RetrieveResults()
        {
            BT.Acp.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
            BT.Acp.Results.FetchReferenceChannelPower("", timeout, out referenceChannelPower);
            BT.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref lowerAbsolutePower, ref upperAbsolutePower, ref lowerRelativePower, ref upperRelativePower, ref lowerMargin, ref upperMargin);
            BT.Acp.Results.FetchMaskTrace("", timeout, ref limitWithExceptionMask, ref limitWithoutExceptionMask);
            BT.Acp.Results.FetchAbsolutePowerTrace("", timeout, ref absolutePower);
            BT.Acp.Results.FetchSpectrum("", timeout, ref spectrum);
        }

        void PrintResults()
        {
            Console.WriteLine("------------------ACP------------------");
            Console.WriteLine("Measurement Status                 : {0}", measurementStatus);
            Console.WriteLine("Reference Channel Power (dBm)      : {0} \n", referenceChannelPower);

            Console.WriteLine("------------------Offset Measuremensts------------------");
            for (int i = 0; i < lowerAbsolutePower.Length; i++)
            {
                Console.WriteLine("Offset " + i);
                Console.WriteLine("Lower Absolute Powers (dBm)        : {0} ", lowerAbsolutePower[i]);
                Console.WriteLine("Upper Absolute Powers (dBm)        : {0} ", upperAbsolutePower[i]);
                Console.WriteLine("Lower Relative Powers (dB)         : {0} ", lowerRelativePower[i]);
                Console.WriteLine("Upper Relative Powers (dB)         : {0} ", upperRelativePower[i]);
                Console.WriteLine("Lower Margin (dB)                  : {0} ", lowerMargin[i]);
                Console.WriteLine("Upper Margin (dB)                  : {0} \n", upperMargin[i]);
            }
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
