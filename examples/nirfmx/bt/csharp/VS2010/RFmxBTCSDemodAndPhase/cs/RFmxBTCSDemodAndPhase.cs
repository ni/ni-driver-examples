//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Packet Type.
//6. Configure Data Rate.
//7. Configure Channel Sounding properties (CS Packet Format, CS Sync Sequence, CS Phase Measurement Period, CS Tone Extension Slot).
//8. Select ModAcc measurement and enable Traces.
//9. Configure ModAcc Burst Synchronization Type.
//10. Configure Averaging Parameters for ModAcc measurement.
//11. Initiate the Measurement.
//12. Fetch ModAcc Measurements and Trace.
//13. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.BTMX;

namespace NationalInstruments.Examples.RFmxBTCSDemodAndPhase
{
    public class RFmxBTCSDemodAndPhase
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
        double iqPowerEdgeLevel;
        RFmxBTMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        double minimumQuietTime;
        double triggerDelay;

        RFmxBTMXPacketType packetType;
        int LEDataRate;

        RFmxBTMXChannelSoundingPacketFormat channelSoundingPacketFormat;
        RFmxBTMXChannelSoundingSyncSequence channelSoundingSyncSequence;
        double channelSoundingPhaseMeasurementPeriod;
        RFmxBTMXChannelSoundingToneExtensionSlot channelSoundingToneExtensionSlot;

        RFmxBTMXModAccAveragingEnabled averagingEnabled;
        int averagingCount;

        double timeout;

        double peakFrequencyErrorMaximum;                                           /*(Hz) */
        double initialFrequencyDriftMaximum;                                        /*(Hz) */
        double peakFrequencyDriftMaximum;                                           /*(Hz) */
        double peakFrequencyDriftRateMaximum;                                       /*(Hz) */

        double clockDriftMean;                                                      /*(ppm) */
        double preambleStartTimeMean;                                               /*(seconds) */

        AnalogWaveform<float> CSDetrendedTrace;
        AnalogWaveform<float> CSToneAmplitude;
        AnalogWaveform<float> CSTonePhase;

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
            iqPowerEdgeLevel = -20.0;                                               /*dB */
            minimumQuietTimeMode = RFmxBTMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 100e-6;                                              /*seconds */
            triggerDelay = 0.0;                                                     /*seconds */

            packetType = RFmxBTMXPacketType.PacketTypeLECS;
            LEDataRate = 1000000;                                                   /*bps */

            channelSoundingPacketFormat = RFmxBTMXChannelSoundingPacketFormat.Sync;
            channelSoundingSyncSequence = RFmxBTMXChannelSoundingSyncSequence.None;
            channelSoundingPhaseMeasurementPeriod = 10e-6;
            channelSoundingToneExtensionSlot = RFmxBTMXChannelSoundingToneExtensionSlot.Disabled;

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
            BT.ConfigureIQPowerEdgeTrigger("", "0", RFmxBTMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
                triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxBTMXIQPowerEdgeTriggerLevelType.Relative,
                enableTrigger);
            BT.ConfigurePacketType("", packetType);
            BT.ConfigureDataRate("", LEDataRate);
            BT.SetChannelSoundingPacketFormat("", channelSoundingPacketFormat);
            BT.SetChannelSoundingSyncSequence("", channelSoundingSyncSequence);
            BT.SetChannelSoundingPhaseMeasurementPeriod("", channelSoundingPhaseMeasurementPeriod);
            BT.SetChannelSoundingToneExtensionSlot("", channelSoundingToneExtensionSlot);
            BT.SelectMeasurements("", RFmxBTMXMeasurementTypes.ModAcc, true);
            BT.ModAcc.Configuration.ConfigureBurstSynchronizationType("", RFmxBTMXModAccBurstSynchronizationType.Preamble);
            BT.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            BT.Initiate("", "");
        }

        void RetrieveResults()
        {
            BT.ModAcc.Results.GetClockDriftMean("", out clockDriftMean);
            BT.ModAcc.Results.GetPreambleStartTimeMean("", out preambleStartTimeMean);
            BT.ModAcc.Results.FetchFrequencyErrorLE("", timeout, out peakFrequencyErrorMaximum,
                out initialFrequencyDriftMaximum, out peakFrequencyDriftMaximum, out peakFrequencyDriftRateMaximum);
            BT.ModAcc.Results.FetchCSToneTrace("", timeout, ref CSToneAmplitude, ref CSTonePhase);
            BT.ModAcc.Results.FetchCSDetrendedPhaseTrace("", timeout, ref CSDetrendedTrace);
        }

        void PrintResults()
        {
            Console.WriteLine("------------------ Channel Sounding Measurement ------------------");
            Console.WriteLine("Peak Frequency Error Maximum (Hz)               : {0}", peakFrequencyErrorMaximum);
            Console.WriteLine("Clock Drift Mean  (ppm)                         : {0}", clockDriftMean);
            Console.WriteLine("Preamble Start Time Mean (seconds)              : {0}", preambleStartTimeMean);
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
