//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Parameters for IQ Power Edge Trigger.
//5. Configure Number of Timeslots.
//6. Configure Auto TSC Detection Enabled.
//7. Configure Signal Type.
//8. Configure TSC.
//9. Select ModAcc measurement and enable Traces.
//10. Configure Averaging Parameters for ModAcc measurement.
//11. Initiate the Measurement.
//12 Fetch ModAcc Measurements and Traces.
//13. Close RFmx Session.

using System;
using NationalInstruments.RFmx.GsmMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxGsmEvm
{
    public class RFmxGsmEvmExample
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
            meanRmsEvm,
            maximumRmsEvm,
            meanPeakEvm,
            maximumPeakEvm,
            ninetyFifthPercentileEvm,
            meanFrequencyError,
            meanIQGainImbalance,
            maximumIQGainImbalance,
            meanIQOriginOffset,
            maximumIQOriginOffset;

        bool enableTrigger;
        string frequencyReferenceSource;
        int numberOfTimeslots, averagingCount, peakEvmSymbol;
        RFmxGsmMXModAccDetectedTsc[] detectedTsc;
        AnalogWaveform<float> evm;
        RFmxGsmMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        RFmxGsmMXAutoTscDetectionEnabled autoTscDetectionEnabled;
        RFmxGsmMXModulationType modulationType;
        RFmxGsmMXBurstType burstType;
        RFmxGsmMXHBFilterWidth hbFilterWidth;
        RFmxGsmMXModAccAveragingEnabled averagingEnabled;
        RFmxGsmMXTsc tsc;

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
            iqPowerEdgeLevel = -20.00;
            triggerDelay = 0.00;
            minimumQuietTimeMode = RFmxGsmMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 582e-6;
            enableTrigger = true;
            numberOfTimeslots = 1;
            autoTscDetectionEnabled = RFmxGsmMXAutoTscDetectionEnabled.True;
            modulationType = RFmxGsmMXModulationType.ModulationType8Psk;
            burstType = RFmxGsmMXBurstType.NB;
            hbFilterWidth = RFmxGsmMXHBFilterWidth.Narrow;
            tsc = RFmxGsmMXTsc.Tsc0;
            averagingEnabled = RFmxGsmMXModAccAveragingEnabled.False;
            averagingCount = 10;
            timeout = 10.0;
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
            gsm.ConfigureIQPowerEdgeTrigger("", "0", RFmxGsmMXIQPowerEdgeTriggerSlope.Rising,
                iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
                RFmxGsmMXIQPowerEdgeTriggerLevelType.Relative,
                enableTrigger);
            gsm.ConfigureNumberOfTimeslots("", numberOfTimeslots);
            gsm.ConfigureAutoTscDetectionEnabled("", autoTscDetectionEnabled);

            gsm.ConfigureSignalType("slot::all", modulationType, burstType, hbFilterWidth);
            gsm.ConfigureTsc("slot::all", tsc);
            gsm.SelectMeasurements("", RFmxGsmMXMeasurementTypes.ModAcc, true);
            gsm.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            gsm.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Fetch Results */
            gsm.ModAcc.Results.FetchEvm("", timeout, out meanRmsEvm, out maximumRmsEvm, out meanPeakEvm,
                out maximumPeakEvm,
                out ninetyFifthPercentileEvm, out meanFrequencyError, out peakEvmSymbol);
            gsm.ModAcc.Results.FetchIQImpairments("", timeout, out meanIQGainImbalance,
                out maximumIQGainImbalance, out meanIQOriginOffset, out maximumIQOriginOffset);
            gsm.ModAcc.Results.FetchDetectedTscArray("", timeout, ref detectedTsc);
            gsm.ModAcc.Results.FetchEvmTrace("", timeout, ref evm);
        }

        private void PrintResults()
        {
            Console.WriteLine("-----------------Measurement-----------------");
            Console.WriteLine("Mean RMS EVM (%)              {0}", meanRmsEvm);
            Console.WriteLine("Maximum RMS EVM (%)           {0}", maximumRmsEvm);
            Console.WriteLine("Mean Peak EVM (%)             {0}", meanPeakEvm);
            Console.WriteLine("Maximum Peak EVM (%)          {0}", maximumPeakEvm);
            Console.WriteLine("95th Percentile EVM (%)       {0}", ninetyFifthPercentileEvm);
            Console.WriteLine("Mean Frequency Error (Hz)     {0}", meanFrequencyError);
            Console.WriteLine("Peak EVM Symbol               {0}\n", peakEvmSymbol);

            Console.WriteLine("----------------IQ Impairments-----------------");
            Console.WriteLine("Mean IQ Origin Offset (dB)        {0}", meanIQOriginOffset);
            Console.WriteLine("Maximum IQ Origin Offset (dB)     {0}", maximumIQOriginOffset);
            Console.WriteLine("Mean IQ Gain Imbalance (dB)       {0}", meanIQGainImbalance);
            Console.WriteLine("Maximum IQ Gain Imbalance (dB)    {0}\n", maximumIQGainImbalance);


            Console.WriteLine("----------------Detected TSC------------------");
            for (int i = 0; i < detectedTsc.Length; i++)
            {
                Console.WriteLine("Slot {0}                   {1}\n", i, detectedTsc[i]);
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