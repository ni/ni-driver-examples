//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure operating Band.
//5. Configure Link Direction.
//6. Configure Trigger Parameters for IQ Power Edge Trigger.
//7. Configure Number of Timeslots.
//8. Configure Auto TSC Detection Enabled.
//9. Configure Signal Type.
//10. Configure TSC.
//11. Configure Power Control Level.
//12. Select PVT measurement and enable Traces.
//13. Configure RBW Filter Bandwidth (Hz) for PVT measurement.
//14. Configure Averaging Parameters for PVT measurement.
//15. Initiate the Measurement.
//16. Fetch PVT Measurements and Traces.
//17. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.GsmMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxGsmPvt
{
    public class RFmxGsmPvtExample
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
        int powerControlLevel, numberOfTimeslots, averagingCount;
        string frequencyReferenceSource;
        RFmxGsmMXBand band;
        RFmxGsmMXLinkDirection linkDirection;
        RFmxGsmMXPvtAveragingType averagingType;
        RFmxGsmMXBurstType burstType;
        RFmxGsmMXHBFilterWidth hbFilterWidth;
        RFmxGsmMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        RFmxGsmMXAutoTscDetectionEnabled autoTscDetectionEnabled;
        RFmxGsmMXModulationType modulationType;
        RFmxGsmMXPvtAveragingEnabled averagingEnabled;
        RFmxGsmMXTsc tsc;
        RFmxGsmMXPvtMeasurementStatus measurementStatus;
        RFmxGsmMXPvtSlotMeasurementStatus[] slotMeasurementStatus;
        AnalogWaveform<float> upperMask, signalPower, lowerMask;

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
            band = RFmxGsmMXBand.Pgsm;
            linkDirection = RFmxGsmMXLinkDirection.Uplink;
            enableTrigger = true;
            triggerDelay = 0.00;
            iqPowerEdgeLevel = -20.00;
            minimumQuietTimeMode = RFmxGsmMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 582e-6;
            numberOfTimeslots = 1;
            averagingEnabled = RFmxGsmMXPvtAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxGsmMXPvtAveragingType.Rms;
            rbwFilterBandwidth = 500000;
            autoTscDetectionEnabled = RFmxGsmMXAutoTscDetectionEnabled.True;
            modulationType = RFmxGsmMXModulationType.ModulationType8Psk;
            burstType = RFmxGsmMXBurstType.NB;
            hbFilterWidth = RFmxGsmMXHBFilterWidth.Narrow;
            tsc = RFmxGsmMXTsc.Tsc0;
            powerControlLevel = 0;
            timeout = 10.00;
        }

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void ConfigureGsm()
        {
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource,
                frequencyReferenceFrequency);

            gsm = instrSession.GetGsmSignalConfiguration();
            gsm.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            gsm.ConfigureBand("", band);
            gsm.ConfigureLinkDirection("", linkDirection);
            gsm.ConfigureIQPowerEdgeTrigger("", "0", RFmxGsmMXIQPowerEdgeTriggerSlope.Rising,
                iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
                RFmxGsmMXIQPowerEdgeTriggerLevelType.Relative,
                enableTrigger);
            gsm.ConfigureNumberOfTimeslots("", numberOfTimeslots);
            gsm.ConfigureAutoTscDetectionEnabled("", autoTscDetectionEnabled);
            gsm.ConfigureSignalType("slot::all", modulationType, burstType, hbFilterWidth);
            gsm.ConfigureTsc("slot::all", tsc);
            gsm.ConfigurePowerControlLevel("slot::all", powerControlLevel);
            gsm.SelectMeasurements("", RFmxGsmMXMeasurementTypes.Pvt, true);
            gsm.Pvt.Configuration.SetRbwFilterBandwidth("", rbwFilterBandwidth);
            gsm.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                averagingType);
            gsm.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */
            gsm.Pvt.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
            gsm.Pvt.Results.FetchSlotMeasurementArray("", timeout, ref slotAveragePower, ref slotBurstWidth,
                ref slotMeasurementStatus, ref slotMaximumPower, ref slotMinimumPower, ref slotBurstThreshold);
            gsm.Pvt.Results.FetchPowerTrace("", timeout, ref upperMask, ref signalPower, ref lowerMask);
        }

        private void PrintResults()
        {
            Console.WriteLine("Measurement Status      : {0}\n", measurementStatus);
            for (int i = 0; i < slotAveragePower.Length; i++)
            {
                Console.WriteLine("\nSlot Measurement        : {0}", i);
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