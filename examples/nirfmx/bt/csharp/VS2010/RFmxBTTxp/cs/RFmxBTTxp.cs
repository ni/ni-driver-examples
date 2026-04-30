//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties(Center Frequency and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Packet Type.
//6. Configure Data Rate.
//7. Configure Payload Length.
//8. Configure Direction Finding.
//9. Configure Reference Level.
//10. Select Txp measurement and enable Traces.
//11. Configure Txp Burst Synchronization Type.
//12. Configure Averaging Parameters for Txp measurement.
//13. Initiate the Measurement.
//14. Fetch Txp Measurements and Trace.
//15. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.BTMX;

namespace NationalInstruments.Examples.RFmxBTTxp
{
    public class RFmxBTTxp
    {
        RFmxInstrMX instrSession;
        RFmxBTMX BT;
        string resourceName;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;
        bool autoLevel;
        double measurementInterval;

        bool enableTrigger;
        RFmxBTMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
        double iqPowerEdgeLevel;
        RFmxBTMXTriggerMinimumQuietTimeMode minimumQuiteTimeMode;
        double minimumQuietTime;
        RFmxBTMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
        double triggerDelay;

        RFmxBTMXPacketType packetType;
        int dataRate;

        RFmxBTMXPayloadLengthMode payloadLengthMode;
        int payloadLength;

        RFmxBTMXDirectionFindingMode directionFindingMode;
        double cteLength;
        double cteSlotDuration;

        RFmxBTMXChannelSoundingPacketFormat packetFormat;
        RFmxBTMXChannelSoundingSyncSequence syncSequence;
        double phaseMeasurementPeriod;
        RFmxBTMXChannelSoundingToneExtensionSlot toneExtensionSlot;

        int zadoffChuIndex;
        RFmxBTMXHighDataThroughputPacketFormat highDataThroughputPacketFormat;
        
        RFmxBTMXTxpBurstSynchronizationType burstSynchronizationType;

        RFmxBTMXMeasurementTypes measurement;
        bool enableAllTraces;

        RFmxBTMXTxpAveragingEnabled averagingEnabled;
        int averagingCount;

        double timeout;
        double averagePowerMean;
        double averagePowerMaximum;
        double averagePowerMinimum;
        double peakToAveragePowerRatioMaximum;
        double edrGfskAveragePowerMean;
        double edrDpskAveragePowerMean;
        double edrDpskGfskAveragePowerRatioMean;

        double referencePeriodAveragePowerMean;
        double referencePeriodPeakAbsolutePowerDeviationMaximum;

        double[] transmitSlotAveragePowerMean;
        double[] transmitSlotPeakAbsolutePowerDeviationMaximum;

        AnalogWaveform<float> power = null;

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
            autoLevel = true;
            measurementInterval = 10e-3;                                            /* (s) */

            enableTrigger = true;
            iqPowerEdgeTriggerSlope = RFmxBTMXIQPowerEdgeTriggerSlope.Rising;
            iqPowerEdgeLevel = -20.0;                                               /*dB */
            minimumQuiteTimeMode = RFmxBTMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 100e-6;                                              /*seconds */
            iqPowerEdgeTriggerLevelType = RFmxBTMXIQPowerEdgeTriggerLevelType.Relative;
            triggerDelay = 0.0;                                                     /*seconds */

            packetType = RFmxBTMXPacketType.PacketTypeDH1;
            dataRate = 1000000;                                                   /*bps */

            payloadLengthMode = RFmxBTMXPayloadLengthMode.Auto;
            payloadLength = 10;                                                     /*bytes */

            directionFindingMode = RFmxBTMXDirectionFindingMode.Disabled;
            cteLength = 160e-6;                                                     /*seconds */
            cteSlotDuration = 1e-6;                                                 /*seconds */

            packetFormat = RFmxBTMXChannelSoundingPacketFormat.Sync;
            syncSequence = RFmxBTMXChannelSoundingSyncSequence.None;
            phaseMeasurementPeriod = 10e-6;                                         /*seconds */
            toneExtensionSlot = RFmxBTMXChannelSoundingToneExtensionSlot.Disabled;

            zadoffChuIndex = 7;
            highDataThroughputPacketFormat = RFmxBTMXHighDataThroughputPacketFormat.Format0;

            burstSynchronizationType = RFmxBTMXTxpBurstSynchronizationType.Preamble;

            measurement = RFmxBTMXMeasurementTypes.Txp;
            enableAllTraces = true;

            averagingEnabled = RFmxBTMXTxpAveragingEnabled.False;
            averagingCount = 10;

            timeout = 10.0;                                                         /*seconds */
            averagePowerMean = 0;                                                   /*(dBm) */
            averagePowerMaximum = 0;                                                /*(dBm) */
            averagePowerMinimum = 0.0;                                              /*(dBm) */
            peakToAveragePowerRatioMaximum = 0.0;                                   /*(dB) */
            edrGfskAveragePowerMean = 0.0;                                          /*(dBm) */
            edrDpskAveragePowerMean = 0.0;                                          /*(dBm) */
            edrDpskGfskAveragePowerRatioMean = 0.0;                                 /*(dB) */
            referencePeriodAveragePowerMean = 0.0;                                  /*(dBm) */
            referencePeriodPeakAbsolutePowerDeviationMaximum = 0.0;                 /*(%) */
      	}

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureBT()
        {
            BT = instrSession.GetBTSignalConfiguration();       /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            BT.ConfigureFrequency("", centerFrequency);
            BT.ConfigureExternalAttenuation("", externalAttenuation);
            BT.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeTriggerSlope, iqPowerEdgeLevel,
                triggerDelay, minimumQuiteTimeMode, minimumQuietTime, iqPowerEdgeTriggerLevelType,
                enableTrigger);
            BT.ConfigurePacketType("", packetType);
            BT.ConfigureDataRate("", dataRate);
            BT.ConfigurePayloadLength("", payloadLengthMode, payloadLength);
            BT.ConfigureLEDirectionFinding("", directionFindingMode, cteLength, cteSlotDuration);
            BT.SetChannelSoundingPacketFormat("",packetFormat);
            BT.SetChannelSoundingSyncSequence("",syncSequence);
            BT.SetChannelSoundingPhaseMeasurementPeriod("",phaseMeasurementPeriod);
            BT.SetChannelSoundingToneExtensionSlot("",toneExtensionSlot);
            BT.SetZadoffChuIndex("",zadoffChuIndex);
            BT.SetHighDataThroughputPacketFormat("",highDataThroughputPacketFormat);    

            if (autoLevel)
            {
               BT.AutoLevel("", measurementInterval, out referenceLevel);
            }
            else
            {
               BT.ConfigureReferenceLevel("", referenceLevel);
            }
            BT.SelectMeasurements("", measurement, enableAllTraces);
            BT.Txp.Configuration.ConfigureBurstSynchronizationType("", burstSynchronizationType);
            BT.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            BT.Initiate("", "");
        }

        void RetrieveResults()
        {
            BT.Txp.Results.FetchPowers("", timeout, out averagePowerMean, out averagePowerMaximum, out averagePowerMinimum, out peakToAveragePowerRatioMaximum);
            BT.Txp.Results.FetchEdrPowers("", timeout, out edrGfskAveragePowerMean, out edrDpskAveragePowerMean, out edrDpskGfskAveragePowerRatioMean);
            BT.Txp.Results.FetchLECteReferencePeriodPowers("", timeout, out referencePeriodAveragePowerMean, out referencePeriodPeakAbsolutePowerDeviationMaximum);
            BT.Txp.Results.FetchLECteTransmitSlotPowersArray("", timeout, ref transmitSlotAveragePowerMean, ref transmitSlotPeakAbsolutePowerDeviationMaximum);
            BT.Txp.Results.FetchPowerTrace("", timeout, ref power);
        }

        void PrintResults()
        {
            Console.WriteLine("------------------Measurement------------------");
            Console.WriteLine("Average Power Mean (dBm)                        : {0}", averagePowerMean);
            Console.WriteLine("Average Power Maximum (dBm)                     : {0}", averagePowerMaximum);
            Console.WriteLine("Average Power Minimum (dBm)                     : {0}", averagePowerMinimum);
            Console.WriteLine("Peak to Average Power Ratio Maximum (dB)        : {0}", peakToAveragePowerRatioMaximum);
            Console.WriteLine("EDR GFSK Average Power Mean (dBm)               : {0}", edrGfskAveragePowerMean);
            Console.WriteLine("EDR DPSK Average Power Mean (dBm)               : {0}", edrDpskAveragePowerMean);
            Console.WriteLine("EDR DPSK GFSK Average Power Ratio Mean (dB)     : {0}", edrDpskGfskAveragePowerRatioMean);

         	Console.WriteLine("------------------LE CTE Reference Period Measurement------------------");
        	Console.WriteLine("Average Power Mean (dBm)                                         : {0}", referencePeriodAveragePowerMean);
         	Console.WriteLine("Peak Absolute Power Deviation Maximum (%)                         : {0}", referencePeriodPeakAbsolutePowerDeviationMaximum);

         	Console.WriteLine("------------------LE CTE Transmit Slot Power Measurement------------------");
         	for (int i = 0; i < transmitSlotAveragePowerMean.Length; i++)
         		{
            	Console.WriteLine("Average Power Mean (dBm)[{0}]                     : {1}", i, transmitSlotAveragePowerMean[i]);
            	Console.WriteLine("Peak Absolute Power Deviation Maximum (%)[{0}]     : {1}", i, transmitSlotPeakAbsolutePowerDeviationMaximum[i]);
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
