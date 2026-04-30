//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Parameters for IQ Power Edge Trigger.
//5. Configure Auto TSC Detection Enabled.
//6. Configure Number of Timeslots.
//7. Configure Signal Type.
//8. Configure TSC.
//9. Select ModAcc measurement and enable Traces.
//10. Configure Averaging Parameters for ModAcc measurement.
//11. Initiate the Measurement.
//12  Fetch ModAcc Measurements and Traces.
//13. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.GsmMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxGsmMultiSlotPferEvm
{
    public class RFmxGsmMultiSlotPferEvmExample
    {
        RFmxInstrMX instrSession;
        RFmxGsmMX gsm;
        string resourceName;

        double frequencyReferenceFrequency, centerFrequency, referenceLevel, externalAttenuation;
        double iqPowerEdgeLevel, triggerDelay, minimumQuietTime, timeout, meanRmsPhaseError;
        double meanFrequencyError,meanIQGainImbalance, maximumIQGainImbalance, meanIQOriginOffset;
        double maximumIQOriginOffset, maximumRmsPhaseError, meanPeakPhaseError, maximumPeakPhaseError;
        double meanRmsEvm, maximumRmsEvm, meanPeakEvm, maximumPeakEvm, ninetyFifthPercentileEvm;

        bool enableTrigger;
        int numberOfTimeslots, averagingCount, peakSymbol, peakEvmSymbol, measurementOffset;
        string frequencyReferenceSource, slotString;
        RFmxGsmMXModAccDetectedTsc[] detectedTsc;
        RFmxGsmMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        RFmxGsmMXAutoTscDetectionEnabled autoTscDetectionEnabled;
        RFmxGsmMXModAccAveragingEnabled averagingEnabled;
        RFmxGsmMXModAccMeasurementInterval measurementInterval;
        AnalogWaveform<float> meanTraceError;
        AnalogWaveform<float> evm;
        struct SlotConfiguration
        {
            public RFmxGsmMXModulationType modulationType;
            public RFmxGsmMXBurstType burstType;
            public RFmxGsmMXHBFilterWidth hbFilterWidth;
            public RFmxGsmMXTsc tsc;
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

            enableTrigger = true;
            triggerDelay = 0.00;                    /* second */
            iqPowerEdgeLevel = -20.00;              /* dBm */
            minimumQuietTimeMode = RFmxGsmMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 582e-6;              /* second */

            averagingEnabled = RFmxGsmMXModAccAveragingEnabled.False;
            averagingCount = 10;

            autoTscDetectionEnabled = RFmxGsmMXAutoTscDetectionEnabled.True;

            numberOfTimeslots = 1;
            measurementInterval = RFmxGsmMXModAccMeasurementInterval.NumberOfTimeslots;
            measurementOffset = 0;

            timeout = 10.00;                        /* second */
            slotConfigurationInput = new SlotConfiguration[numberOfTimeslots];

            for (int i = 0; i < numberOfTimeslots; i++)
            {
                slotConfigurationInput[i].modulationType = RFmxGsmMXModulationType.ModulationTypeGmsk;
                slotConfigurationInput[i].burstType = RFmxGsmMXBurstType.NB;
                slotConfigurationInput[i].hbFilterWidth = RFmxGsmMXHBFilterWidth.Narrow;    /* Hz */
                slotConfigurationInput[i].tsc = RFmxGsmMXTsc.Tsc0;
            }
        }

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void ConfigureGsm()
        {
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            gsm = instrSession.GetGsmSignalConfiguration();
            gsm.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            gsm.ConfigureIQPowerEdgeTrigger("", "0", RFmxGsmMXIQPowerEdgeTriggerSlope.Rising,
                iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
                RFmxGsmMXIQPowerEdgeTriggerLevelType.Relative,
                enableTrigger);
            gsm.ConfigureAutoTscDetectionEnabled("", autoTscDetectionEnabled);
            gsm.ConfigureNumberOfTimeslots("", numberOfTimeslots);

            for (int i = 0; i < numberOfTimeslots; i++)
            {
                slotString = RFmxGsmMX.BuildSlotString("", i);
                gsm.ConfigureSignalType(slotString, slotConfigurationInput[i].modulationType,
                                        slotConfigurationInput[i].burstType, slotConfigurationInput[i].hbFilterWidth);
                gsm.ConfigureTsc(slotString, slotConfigurationInput[i].tsc);
            }

            gsm.SelectMeasurements("", RFmxGsmMXMeasurementTypes.ModAcc, true);
            gsm.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            gsm.ModAcc.Configuration.SetMeasurementInterval("", measurementInterval);
            gsm.ModAcc.Configuration.SetMeasurementOffset("", measurementOffset);
            gsm.Initiate("", "");
        }

        private void RetrieveResults()
        {
            if (slotConfigurationInput[measurementOffset].modulationType != RFmxGsmMXModulationType.ModulationTypeGmsk)
            {
                gsm.ModAcc.Results.FetchEvm("", timeout, out meanRmsEvm, out maximumRmsEvm, out meanPeakEvm,
                    out maximumPeakEvm, out ninetyFifthPercentileEvm, out meanFrequencyError, out peakEvmSymbol);
                gsm.ModAcc.Results.FetchEvmTrace("", timeout, ref evm);
            }
            else
            {
                gsm.ModAcc.Results.FetchPfer("", timeout, out meanRmsPhaseError, out maximumRmsPhaseError,
                 out meanPeakPhaseError, out maximumPeakPhaseError, out meanFrequencyError, out peakSymbol);
                gsm.ModAcc.Results.FetchPhaseErrorTrace("", timeout, ref meanTraceError);
            }

            gsm.ModAcc.Results.FetchIQImpairments("", timeout, out meanIQGainImbalance, out maximumIQGainImbalance,
                    out meanIQOriginOffset, out maximumIQOriginOffset);
            gsm.ModAcc.Results.FetchDetectedTscArray("", timeout, ref detectedTsc);

        }

        private void PrintResults()
        {
            if (slotConfigurationInput[measurementOffset].modulationType == RFmxGsmMXModulationType.ModulationTypeGmsk)
            {
                Console.WriteLine("---------PFER Measurement---------");
                Console.WriteLine("Mean RMS Phase Error (deg)     {0}", meanRmsPhaseError);
                Console.WriteLine("Maximum RMS Phase Error (deg)  {0}", maximumRmsPhaseError);
                Console.WriteLine("Mean Peak Phase Error (deg)    {0}", meanPeakPhaseError);
                Console.WriteLine("Maximum Peak Phase Error (deg) {0}", maximumPeakPhaseError);
                Console.WriteLine("Mean Frequency Error (Hz)      {0}", meanFrequencyError);
                Console.WriteLine("Peak Symbol                    {0}\n", peakSymbol);
            }
            else
            {
                Console.WriteLine("---------------EVM Measurement---------------");
                Console.WriteLine("Mean RMS EVM (%)              {0}", meanRmsEvm);
                Console.WriteLine("Maximum RMS EVM (%)           {0}", maximumRmsEvm);
                Console.WriteLine("Mean Peak EVM (%)             {0}", meanPeakEvm);
                Console.WriteLine("Maximum Peak EVM (%)          {0}", maximumPeakEvm);
                Console.WriteLine("95th Percentile EVM (%)       {0}", ninetyFifthPercentileEvm);
                Console.WriteLine("Mean Frequency Error (Hz)     {0}", meanFrequencyError);
                Console.WriteLine("Peak EVM Symbol               {0}\n", peakEvmSymbol);
            }

            Console.WriteLine("-----------------IQ Impairments----------------");
            Console.WriteLine("Maximum IQ Gain Imbalance (dB) {0}", maximumIQGainImbalance);
            Console.WriteLine("Maximum IQ Origin Offset (dB)  {0}", maximumIQOriginOffset);
            Console.WriteLine("Mean IQ Gain Imbalance (dB)    {0}", meanIQGainImbalance);
            Console.WriteLine("Mean IQ Origin Offset (dB)     {0}\n", meanIQOriginOffset);

            Console.WriteLine("------------Detected TSC-----------\n");
            for (int i = 0; i < detectedTsc.Length; i++)
            {
                if (measurementInterval == RFmxGsmMXModAccMeasurementInterval.NumberOfTimeslots)
                    Console.WriteLine("Slot {0}                         {1}\n", i, detectedTsc[i]);
                else
                    Console.WriteLine("Slot {0}                         {1}\n", measurementOffset, detectedTsc[i]);
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
