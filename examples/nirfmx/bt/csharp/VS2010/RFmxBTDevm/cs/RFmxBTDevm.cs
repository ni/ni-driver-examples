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

namespace NationalInstruments.Examples.RFmxBTDevm
{
    public class RFmxBTDevm
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
        double iqPowerEdgeTriggerLevel;
        RFmxBTMXTriggerMinimumQuietTimeMode minimumQuiteTimeMode;
        double minimumQuietTime;
        RFmxBTMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
        double triggerDelay;

        RFmxBTMXPacketType packetType;

        RFmxBTMXPayloadLengthMode payloadLengthMode;
        int payloadLength;

        RFmxBTMXModAccBurstSynchronizationType burstSynchronizationType;

        RFmxBTMXMeasurementTypes measurement;
        bool enableAllTraces;

        RFmxBTMXModAccAveragingEnabled averagingEnabled;
        int averagingCount;

        double timeout;
        double peakRmsDevmMaximum;
        double peakDevmMaximum;
        double ninetyninePercentDevm;

        double headerFrequencyErrorWiMaximum;
        double peakFrequencyErrorWiPlusW0Maximum;
        double peakFrequencyErrorW0Maximum;

        float[] time;
        float[] frequencyErrorWiPlusW0;

        float[] devmPerSymbol;
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
            rfsaResourceName = "RFSA";

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

            packetType = RFmxBTMXPacketType.PacketType2DH1;

            payloadLengthMode = RFmxBTMXPayloadLengthMode.Auto;
            payloadLength = 10;                                                      /*(bytes) */

            burstSynchronizationType = RFmxBTMXModAccBurstSynchronizationType.Preamble;

            measurement = RFmxBTMXMeasurementTypes.ModAcc;
            enableAllTraces = true;

            averagingEnabled = RFmxBTMXModAccAveragingEnabled.False;
            averagingCount = 10;

            timeout = 10.0;                                                         /*(seconds) */
            peakRmsDevmMaximum = 0;                                                 /*(%) */
            peakDevmMaximum = 0;                                                    /*(%) */
            ninetyninePercentDevm = 0.0;                                            /*(%) */
            headerFrequencyErrorWiMaximum = 0.0;                                    /*(Hz) */
            peakFrequencyErrorWiPlusW0Maximum = 0.0;                                /*(Hz) */
            peakFrequencyErrorW0Maximum = 0.0;                                      /*(Hz) */
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
            BT.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel,
                triggerDelay, minimumQuiteTimeMode, minimumQuietTime, iqPowerEdgeTriggerLevelType,
                enableTrigger);
            BT.ConfigurePacketType("", packetType);
            BT.ConfigurePayloadLength("", payloadLengthMode, payloadLength);
            BT.SelectMeasurements("", measurement, enableAllTraces);
            BT.ModAcc.Configuration.ConfigureBurstSynchronizationType("", burstSynchronizationType);
            BT.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            BT.Initiate("", "");
        }

        void RetrieveResults()
        {
            BT.ModAcc.Results.FetchDevm("", timeout, out peakRmsDevmMaximum, out peakDevmMaximum, out ninetyninePercentDevm);
            BT.ModAcc.Results.FetchFrequencyErrorEdr("", timeout, out headerFrequencyErrorWiMaximum, out peakFrequencyErrorWiPlusW0Maximum,
                out peakFrequencyErrorW0Maximum);
            BT.ModAcc.Results.FetchDevmPerSymbolTrace("", timeout, ref devmPerSymbol);
            BT.ModAcc.Results.FetchFrequencyErrorWiPlusW0TraceEdr("", timeout, ref time, ref frequencyErrorWiPlusW0);
            BT.ModAcc.Results.FetchConstellationTrace("", timeout, ref constellation);
        }

        void PrintResults()
        {
            Console.WriteLine("------------------DEVM------------------");
            Console.WriteLine("Peak Rms Devm Maximum (%)                : {0}", peakRmsDevmMaximum);
            Console.WriteLine("Peak Devm Maximum (%)                    : {0}", peakDevmMaximum);
            Console.WriteLine("99% Devm (%)                             : {0}", ninetyninePercentDevm);
            Console.WriteLine("\n------------------EDR Frequency Error------------------\n");
            Console.WriteLine("Header Frequency Error wi Maximum (Hz)   : {0}", headerFrequencyErrorWiMaximum);
            Console.WriteLine("Peak Frequency Error wi+w0 Maximum (Hz)  : {0}", peakFrequencyErrorWiPlusW0Maximum);
            Console.WriteLine("Peak Frequency Error w0 Maximum (Hz)     : {0}", peakFrequencyErrorW0Maximum);
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