//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure operating Band.
//5. Configure Link Direction.
//6. Configure Trigger Parameters for IQ Power Edge Trigger.
//7. Configure Auto TSC Detection Enabled.
//8. Configure Number of Timeslots.
//9. Configure Signal Type.
//10. Configure TSC.
//11. Configure Power Control Level.
//12. Select PVT measurement and enable Traces.
//13. Configure RBW Filter Bandwidth (Hz) for PVT measurement.
//14. Configure Averaging Parameters for PVT measurement.
//15. Initiate the Measurement.
//16 Fetch PVT Measurements and Traces.
//17. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.GsmMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxGsmMultiSlotPvt
{
    public class RFmxGsmMultiSlotPvtExample
    {
        RFmxInstrMX instrSession;
        RFmxGsmMX gsm;
        string resourceName;

        double frequencyReferenceFrequency,
            centerFrequency,
            referenceLevel,
            externalAttenuation,
            iqPowerEdgeLevel,
            triggerDelay,
            minimumQuietTime,
            timeout,
            rbwFilterBandwidth;

        double[] slotAveragePower,
            slotBurstWidth,
            slotMaximumPower,
            slotMinimumPower,
            slotBurstThreshold;

        bool enableTrigger;
        int numberOfSlots, numberOfTimeslots, averagingCount;
        string frequencyReferenceSource, slotString;
        RFmxGsmMXBand band;
        RFmxGsmMXLinkDirection linkDirection;
        RFmxGsmMXPvtAveragingType averagingType;
        RFmxGsmMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        RFmxGsmMXAutoTscDetectionEnabled autoTscDetectionEnabled;
        RFmxGsmMXPvtAveragingEnabled averagingEnabled;
        RFmxGsmMXPvtMeasurementStatus measurementStatus;
        RFmxGsmMXPvtSlotMeasurementStatus[] slotMeasurementStatus;
        AnalogWaveform<float> upperMask, signalPower, lowerMask;

        struct SlotConfiguration
        {
            public RFmxGsmMXModulationType modulationType;
            public RFmxGsmMXBurstType burstType;
            public RFmxGsmMXHBFilterWidth hbFilterWidth;
            public RFmxGsmMXTsc tsc;
            public int powerControlLevel;
        }
        SlotConfiguration[] slotConfigurationInput;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureGsm();
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
                Console.WriteLine("Press any key to exit.....");
                Console.ReadKey();
            }
        }

        private void InitializeVariables()
        {
            /* Initialize input variables */
            resourceName = "RFSA";
            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e+6;    /* Hz */
            centerFrequency = 890.2e+6;             /* Hz */
            referenceLevel = 0.00;                  /* dBm */
            externalAttenuation = 0.00;             /* dB */
            numberOfSlots = 1;
            slotConfigurationInput = new SlotConfiguration[numberOfSlots];
            band = RFmxGsmMXBand.Pgsm;
            linkDirection = RFmxGsmMXLinkDirection.Uplink;
            triggerDelay = 0.00;
            enableTrigger = true;
            iqPowerEdgeLevel = -20.00;
            minimumQuietTimeMode = RFmxGsmMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 582e-6;
            numberOfTimeslots = 1;
            averagingEnabled = RFmxGsmMXPvtAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxGsmMXPvtAveragingType.Rms;
            rbwFilterBandwidth = 500000;
            autoTscDetectionEnabled = RFmxGsmMXAutoTscDetectionEnabled.True;
            timeout = 10.00;

            for (int i = 0; i < numberOfSlots; i++)
            {
                slotConfigurationInput[i].modulationType = RFmxGsmMXModulationType.ModulationType8Psk;
                slotConfigurationInput[i].burstType = RFmxGsmMXBurstType.NB;
                slotConfigurationInput[i].hbFilterWidth = RFmxGsmMXHBFilterWidth.Narrow;    /* Hz */
                slotConfigurationInput[i].tsc = RFmxGsmMXTsc.Tsc0;
                slotConfigurationInput[i].powerControlLevel = 0;
            } 
 }

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void ConfigureGsm()
        {
            /* Configure PVT measurements */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

            gsm = instrSession.GetGsmSignalConfiguration();
            gsm.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            gsm.ConfigureBand("", band);
            gsm.ConfigureLinkDirection("", linkDirection);
            gsm.ConfigureIQPowerEdgeTrigger("", "0", RFmxGsmMXIQPowerEdgeTriggerSlope.Rising,
                iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
                RFmxGsmMXIQPowerEdgeTriggerLevelType.Relative,
                enableTrigger);

            gsm.ConfigureNumberOfTimeslots("", numberOfTimeslots); gsm.ConfigureAutoTscDetectionEnabled("", autoTscDetectionEnabled);
            for (int i = 0; i < numberOfSlots; i++)
            {
                slotString = RFmxGsmMX.BuildSlotString("", i);
                gsm.ConfigureSignalType(slotString, slotConfigurationInput[i].modulationType,
                                        slotConfigurationInput[i].burstType, slotConfigurationInput[i].hbFilterWidth);
                gsm.ConfigureTsc(slotString, slotConfigurationInput[i].tsc);
                gsm.ConfigurePowerControlLevel(slotString, slotConfigurationInput[i].powerControlLevel);
            }

            gsm.SelectMeasurements("", RFmxGsmMXMeasurementTypes.Pvt, true);
            gsm.Pvt.Configuration.SetRbwFilterBandwidth("", rbwFilterBandwidth);
            gsm.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            gsm.Initiate("", "");
        }

        private void RetrieveResults()
        {
            gsm.Pvt.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
            gsm.Pvt.Results.FetchSlotMeasurementArray("", timeout, ref slotAveragePower, ref slotBurstWidth,
                ref slotMeasurementStatus, ref slotMaximumPower, ref slotMinimumPower, ref slotBurstThreshold);
            gsm.Pvt.Results.FetchPowerTrace("", timeout, ref upperMask, ref signalPower, ref lowerMask);
        }

        private void PrintResults()
        {
            Console.WriteLine("Measurement Status      : {0}\n", measurementStatus);
            Console.WriteLine("\n--------------Slot Measurement--------------");
            for (int i = 0; i < slotAveragePower.Length; i++)
            {
                Console.WriteLine("\nSlot Measurement : {0}\n", i);
                Console.WriteLine("Average Power (dBm)     : {0}", slotAveragePower[i]);
                Console.WriteLine("Burst Width (s)         : {0}", slotBurstWidth[i]);
                Console.WriteLine("Maximum Power (dBm)     : {0}", slotMaximumPower[i]);
                Console.WriteLine("Minimum Power (dBm)     : {0}", slotMinimumPower[i]);
                Console.WriteLine("Burst Threshold (dBm)   : {0}", slotBurstThreshold[i]);
                Console.WriteLine("Measurement Status      : {0}", slotMeasurementStatus[i]);
            }
        }

        private void CloseSession()
        {
            try
            {
                if (gsm != null)
                {
                    gsm.Dispose();
                    gsm = null;
                }

                if (instrSession != null)
                {
                    instrSession.Close();
                    instrSession = null;
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        private static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }
    }
}