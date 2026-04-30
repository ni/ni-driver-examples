//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure External Attenuation.
//4. Configure Center Frequency. 
//5. Configure Link Direction.
//6. Configure Trigger Parameters for IQ Power Edge Trigger.
//7. Configure Auto Level.
//8. Configure Auto TSC Detection Enabled.
//9. Configure Number of Timeslots.
//10. Configure Signal Type.
//11. Configure TSC.
//12. Select  ORFS  measurement and enable Traces.
//13. Configure Noise Compensation Enabled.
//14. Configure Measurement Type. 
//15. Configure Offset Frequency Mode.
//16. Configure Evaluation Symbols.
//17. Configure Averaging Parameters.
//18. Initiate the Measurement.
//19  Fetch ORFS Measurements and Traces.
//20. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.GsmMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxGsmMultiSlotOrfs
{
    public class RFmxGsmMultiSlotOrfsExample
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
            autoReferenceLevel,
            timeout,
            modulationCarrierPower,
            evaluationSymbolsStart,
            evaluationSymbolsStop,
            switchingCarrierPower;

        double[] modulationResultsLowerRelativePower,
            modulationResultsUpperRelativePower,
            modulationResultsLowerAbsolutePower,
            modulationResultsUpperAbsolutePower,
            switchingResultsLowerRelativePower,
            switchingResultsUpperRelativePower,
            switchingResultsLowerAbsolutePower,
            switchingResultsUpperAbsolutePower;

        float[] modulationPowerTraceOffsetFrequency,
            modulationPowerTraceAbsolutePower,
            modulationPowerTraceRelativePower,
            switchingPowerTraceOffsetFrequency,
            switchingPowerTraceAbsolutePower,
            switchingPowerTraceRelativePower;
            
        bool enableTrigger;
        int numberOfTimeslots, averagingCount, measurementOffset;
        string frequencyReferenceSource, slotString;
        RFmxGsmMXLinkDirection linkDirection;
        RFmxGsmMXOrfsAveragingType averagingType;
        RFmxGsmMXOrfsMeasurementType measurementType;
        RFmxGsmMXOrfsOffsetFrequencyMode offsetFrequencyMode;
        RFmxGsmMXBurstType burstType;
        RFmxGsmMXHBFilterWidth hbFilterWidth;
        RFmxGsmMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        RFmxGsmMXAutoTscDetectionEnabled autoTscDetectionEnabled;
        RFmxGsmMXModulationType modulationType;
        RFmxGsmMXOrfsAveragingEnabled averagingEnabled;
        RFmxGsmMXTsc tsc;
        RFmxGsmMXOrfsNoiseCompensationEnabled noiseCompensationEnabled;
        RFmxGsmMXOrfsEvaluationSymbolsIncludeTsc evaluationSymbolsIncludeTsc;
        RFmxGsmMXOrfsMeasurementInterval measurementInterval;
        
        bool autoLevel = true;
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
            iqPowerEdgeLevel = -20.00;              /* dBm */
            triggerDelay = 0.00;                    /* second */
            minimumQuietTimeMode = RFmxGsmMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 582e-6;              /* second */

            linkDirection = RFmxGsmMXLinkDirection.Uplink;
            noiseCompensationEnabled = RFmxGsmMXOrfsNoiseCompensationEnabled.False;

            averagingEnabled = RFmxGsmMXOrfsAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxGsmMXOrfsAveragingType.Log;

            evaluationSymbolsStart = 50.00;
            evaluationSymbolsStop = 90.00;
            evaluationSymbolsIncludeTsc = RFmxGsmMXOrfsEvaluationSymbolsIncludeTsc.False;

            offsetFrequencyMode = RFmxGsmMXOrfsOffsetFrequencyMode.Standard;
            measurementType = RFmxGsmMXOrfsMeasurementType.ModulationAndSwitching;

            autoTscDetectionEnabled = RFmxGsmMXAutoTscDetectionEnabled.True;

            numberOfTimeslots = 1;
            measurementInterval = RFmxGsmMXOrfsMeasurementInterval.NumberOfTimeslots;
            measurementOffset = 0;
            
            timeout = 10.00;                       /* second */

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
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource,
                frequencyReferenceFrequency);

            gsm = instrSession.GetGsmSignalConfiguration();
            gsm.ConfigureExternalAttenuation("", externalAttenuation);
            gsm.ConfigureFrequency("", centerFrequency);
            gsm.ConfigureLinkDirection("", linkDirection);
            gsm.ConfigureIQPowerEdgeTrigger("", "0", RFmxGsmMXIQPowerEdgeTriggerSlope.Rising,
                iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
                RFmxGsmMXIQPowerEdgeTriggerLevelType.Relative,
                enableTrigger);
            if (autoLevel)
            {
                gsm.AutoLevel("", 0.0046, out autoReferenceLevel);
                Console.WriteLine("Reference Level : {0}", autoReferenceLevel);
            }
            else
                gsm.ConfigureReferenceLevel("", referenceLevel);

            gsm.ConfigureAutoTscDetectionEnabled("", autoTscDetectionEnabled);
            gsm.ConfigureNumberOfTimeslots("", numberOfTimeslots);
            for (int i = 0; i < numberOfTimeslots; i++)
            {
                slotString = RFmxGsmMX.BuildSlotString("", i);
                gsm.ConfigureSignalType(slotString, slotConfigurationInput[i].modulationType,
                                        slotConfigurationInput[i].burstType, slotConfigurationInput[i].hbFilterWidth);
                gsm.ConfigureTsc(slotString, slotConfigurationInput[i].tsc);
            }
            
            gsm.SelectMeasurements("", RFmxGsmMXMeasurementTypes.Orfs, true);
            gsm.Orfs.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled);
            gsm.Orfs.Configuration.ConfigureMeasurementType("", measurementType);
            gsm.Orfs.Configuration.ConfigureOffsetFrequencyMode("", offsetFrequencyMode);
            gsm.Orfs.Configuration.ConfigureEvaluationSymbols("", evaluationSymbolsStart, evaluationSymbolsIncludeTsc,
                evaluationSymbolsStop);
            gsm.Orfs.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            gsm.Orfs.Configuration.SetMeasurementInterval("", measurementInterval);
            gsm.Orfs.Configuration.SetMeasurementOffset("", measurementOffset);
            gsm.Initiate("", "");
        }

        private void RetrieveResults()
        {
            gsm.Orfs.Results.FetchModulationResultsArray("", timeout, out modulationCarrierPower,
                ref modulationResultsLowerRelativePower, ref modulationResultsUpperRelativePower,
                ref modulationResultsLowerAbsolutePower, ref modulationResultsUpperAbsolutePower);
            gsm.Orfs.Results.FetchSwitchingResultsArray("", timeout, out switchingCarrierPower,
                ref switchingResultsLowerRelativePower, ref switchingResultsUpperRelativePower,
                ref switchingResultsLowerAbsolutePower, ref switchingResultsUpperAbsolutePower);

            gsm.Orfs.Results.FetchModulationPowerTrace("", timeout, ref modulationPowerTraceOffsetFrequency,
                ref modulationPowerTraceAbsolutePower, ref modulationPowerTraceRelativePower);
            gsm.Orfs.Results.FetchSwitchingPowerTrace("", timeout, ref switchingPowerTraceOffsetFrequency,
                ref switchingPowerTraceAbsolutePower, ref switchingPowerTraceRelativePower);
        }

        private void PrintResults()
        {
            Console.WriteLine("---------Modulation Results---------");
            Console.WriteLine("Modulation Carrier Power (dBm) {0}\n", modulationCarrierPower);
            for (int i = 0; i < modulationResultsLowerAbsolutePower.Length; i++)
            {
                Console.WriteLine("Offset : {0}", i);
                Console.WriteLine("Lower Absolute Power (dBm)   {0}", modulationResultsLowerAbsolutePower[i]);
                Console.WriteLine("Lower Relative Power (dB)    {0}", modulationResultsLowerRelativePower[i]);
                Console.WriteLine("Upper Absolute Power (dBm)   {0}", modulationResultsUpperAbsolutePower[i]);
                Console.WriteLine("Upper Relative Power (dB)    {0}\n", modulationResultsUpperRelativePower[i]);
            }
            Console.WriteLine("---------Switching Results---------");
            Console.WriteLine("Switching Carrier Power (dBm)  {0}\n", switchingCarrierPower);

            for (int i = 0; i < switchingResultsLowerAbsolutePower.Length; i++)
            {
                Console.WriteLine("Offset : {0}", i);
                Console.WriteLine("Lower Absolute Power (dBm)   {0}", switchingResultsLowerAbsolutePower[i]);
                Console.WriteLine("Lower Relative Power (dB)    {0}", switchingResultsLowerRelativePower[i]);
                Console.WriteLine("Upper Absolute Power (dBm)   {0}", switchingResultsUpperAbsolutePower[i]);
                Console.WriteLine("Upper Relative Power (dB)    {0}\n", switchingResultsUpperRelativePower[i]);
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
