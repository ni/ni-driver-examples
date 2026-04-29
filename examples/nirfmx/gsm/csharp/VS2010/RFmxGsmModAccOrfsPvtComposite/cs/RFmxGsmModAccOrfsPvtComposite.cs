//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure operating Band.
//5. Configure Link Direction.
//6. Configure Trigger Parameters for IQ Power Edge Trigger.
//7. Select  measurements and enable Traces.
//8. Configure Number of Timeslots.
//9. Configure Averaging Parameters for ModAcc measurement.
//10. Configure Averaging Parameters for ORFS measurement.
//11. Configure Averaging Parameters for PVT measurement.
//12. Configure ORFS Measurement Type.
//13. Configure Offset Frequency Mode for ORFS measurement.
//14. Configure Auto TSC Detection Enabled.
//15. Configure Signal Type.
//16. Configure TSC.
//17. Configure Power Control Level.
//18. Initiate the Measurement.
//19  Fetch ModAcc/ORFS/PVT Measurements and Traces.
//20. Close RFmx Session. 


using System;
using NationalInstruments.RFmx.GsmMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxGsmModAccOrfsPvtComposite
{
    public class RFmxGsmModAccOrfsPvtCompositeExample
    {
        RFmxInstrMX instrSession;
        RFmxGsmMX gsm;
        string resourceName;

        double frequencyReferenceFrequency, centerFrequency,referenceLevel,externalAttenuation;
        double iqPowerEdgeLevel, triggerDelay,minimumQuietTime,timeout,meanRmsEvm,maximumRmsEvm;
        double meanPeakEvm, meanRmsPhaseError,maximumPeakEvm,ninetyFifthPercentileEvm,meanFrequencyErrorEvm;
        double meanFrequencyErrorPfer,meanIQGainImbalance,maximumIQGainImbalance,meanIQOriginOffset;
        double maximumIQOriginOffset, maximumRmsPhaseError,modulationCarrierPower,meanPeakPhaseError;
        double maximumPeakPhaseError, switchingCarrierPower;

        double[] modulationResultsLowerRelativePower, modulationResultsUpperRelativePower,modulationResultsLowerAbsolutePower;
        double[] modulationResultsUpperAbsolutePower, switchingResultsLowerRelativePower,switchingResultsUpperRelativePower;
        double[] switchingResultsLowerAbsolutePower, switchingResultsUpperAbsolutePower,slotAveragePower;
        double[] slotBurstWidth,slotMaximumPower,slotMinimumPower,slotBurstThreshold;

        float[] modulationPowerTraceOffsetFrequency, modulationPowerTraceAbsolutePower,modulationPowerTraceRelativePower;
        float[] switchingPowerTraceOffsetFrequency, switchingPowerTraceAbsolutePower, switchingPowerTraceRelativePower;
            

        bool enableTrigger;
        int powerControlLevel, numberOfTimeslots, averagingCount, peakEvmSymbol, peakSymbol;
        string frequencyReferenceSource;
        RFmxGsmMXModAccDetectedTsc[] detectedTsc;
        RFmxGsmMXBand band;
        RFmxGsmMXLinkDirection linkDirection;
        RFmxGsmMXOrfsAveragingType orfsAveragingType;
        RFmxGsmMXPvtAveragingType pvtAveragingType;
        RFmxGsmMXOrfsMeasurementType measurementType;
        RFmxGsmMXBurstType burstType;
        RFmxGsmMXHBFilterWidth hbFilterWidth;
        RFmxGsmMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        RFmxGsmMXAutoTscDetectionEnabled autoTscDetectionEnabled;
        RFmxGsmMXModulationType modulationType;
        RFmxGsmMXModAccAveragingEnabled modAccaveragingEnabled;
        RFmxGsmMXOrfsAveragingEnabled orfsAveragingEnabled;
        RFmxGsmMXPvtAveragingEnabled pvtAveragingEnabled;
        RFmxGsmMXTsc tsc;
        RFmxGsmMXPvtMeasurementStatus measurementStatus;
        RFmxGsmMXPvtSlotMeasurementStatus[] slotMeasurementStatus;
        AnalogWaveform<float> evm, meanTraceError, upperMask, signalPower, lowerMask;

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
            modAccaveragingEnabled = RFmxGsmMXModAccAveragingEnabled.False;
            orfsAveragingEnabled = RFmxGsmMXOrfsAveragingEnabled.False;
            pvtAveragingEnabled = RFmxGsmMXPvtAveragingEnabled.False;
            averagingCount = 10;
            pvtAveragingType = RFmxGsmMXPvtAveragingType.Rms;
            orfsAveragingType = RFmxGsmMXOrfsAveragingType.Rms;
            measurementType = RFmxGsmMXOrfsMeasurementType.ModulationAndSwitching;
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
            gsm.SelectMeasurements("",
                                   RFmxGsmMXMeasurementTypes.ModAcc | RFmxGsmMXMeasurementTypes.Orfs | RFmxGsmMXMeasurementTypes.Pvt, 
                                   true);
            gsm.ConfigureNumberOfTimeslots("", numberOfTimeslots);
            gsm.ModAcc.Configuration.ConfigureAveraging("", modAccaveragingEnabled, averagingCount);
            gsm.Orfs.Configuration.ConfigureAveraging("", orfsAveragingEnabled, averagingCount,
                orfsAveragingType);
            gsm.Pvt.Configuration.ConfigureAveraging("", pvtAveragingEnabled, averagingCount,
                pvtAveragingType);
            gsm.Orfs.Configuration.ConfigureMeasurementType("", measurementType);
            gsm.Orfs.Configuration.ConfigureOffsetFrequencyMode("", RFmxGsmMXOrfsOffsetFrequencyMode.Standard);
            gsm.ConfigureAutoTscDetectionEnabled("", autoTscDetectionEnabled);
            gsm.ConfigureSignalType("slot::all", modulationType, burstType, hbFilterWidth);
            gsm.ConfigureTsc("slot::all", tsc);
            gsm.ConfigurePowerControlLevel("slot::all", powerControlLevel);
            gsm.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Fetch Results */
            gsm.ModAcc.Results.FetchEvm("", timeout, out meanRmsEvm, out maximumRmsEvm, out meanPeakEvm,
                out maximumPeakEvm,
                out ninetyFifthPercentileEvm, out meanFrequencyErrorEvm, out peakEvmSymbol);
            gsm.ModAcc.Results.FetchIQImpairments("", timeout, out meanIQGainImbalance,
                out maximumIQGainImbalance, out meanIQOriginOffset, out maximumIQOriginOffset);
            gsm.ModAcc.Results.FetchEvmTrace("", timeout, ref evm);
            gsm.ModAcc.Results.FetchDetectedTscArray("", timeout, ref detectedTsc);

            gsm.ModAcc.Results.FetchPfer("", timeout, out meanRmsPhaseError, out maximumRmsPhaseError,
                out meanFrequencyErrorPfer, out meanPeakPhaseError, out maximumPeakPhaseError, out peakSymbol);
            gsm.ModAcc.Results.FetchPhaseErrorTrace("", timeout, ref meanTraceError);

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

            gsm.Pvt.Results.FetchSlotMeasurementArray("", timeout, ref slotAveragePower, ref slotBurstWidth,
                ref slotMeasurementStatus, ref slotMaximumPower, ref slotMinimumPower, ref slotBurstThreshold);
            gsm.Pvt.Results.FetchMeasurementStatus("", timeout, out measurementStatus);

            gsm.Pvt.Results.FetchPowerTrace("", timeout, ref upperMask, ref signalPower, ref lowerMask);
        }

        private void PrintResults()
        {
            Console.WriteLine("Measurement Status              {0}", measurementStatus);

            Console.WriteLine("-----------------ModAcc Measurements---------------\n\n");
            Console.WriteLine("-----------------EVM Measurement-----------------\n");
            Console.WriteLine("Mean RMS EVM (%)                {0}", meanRmsEvm);
            Console.WriteLine("Maximum RMS EVM (%)             {0}", maximumRmsEvm);
            Console.WriteLine("Mean Peak EVM (%)               {0}", meanPeakEvm);
            Console.WriteLine("Maximum Peak EVM (%)            {0}", maximumPeakEvm);
            Console.WriteLine("95th Percentile EVM (%)         {0}", ninetyFifthPercentileEvm);
            Console.WriteLine("Mean Frequency Error (Hz)       {0}", meanFrequencyErrorEvm);
            Console.WriteLine("Peak EVM Symbol                 {0}\n", peakEvmSymbol);

            Console.WriteLine("-----------------PFER Measurement-----------------\n");
            Console.WriteLine("Mean RMS Phase Error (deg)      {0}", meanRmsPhaseError);
            Console.WriteLine("Maximum RMS Phase Error (deg)   {0}", maximumRmsPhaseError);
            Console.WriteLine("Mean Peak Phase Error  (deg)    {0}", meanPeakPhaseError);
            Console.WriteLine("Maximum Peak Phase Error (deg)  {0}", maximumPeakPhaseError);
            Console.WriteLine("Mean Frequency Error (Hz)       {0}", meanFrequencyErrorPfer);
            Console.WriteLine("Peak Symbol                     {0}\n", peakSymbol);

            Console.WriteLine("----------------IQ Impairments-----------------\n");
            Console.WriteLine("Mean IQ Gain Imbalance (dB)     {0}", meanIQGainImbalance);
            Console.WriteLine("Maximum IQ Gain Imbalance (dB)  {0}", maximumIQGainImbalance);
            Console.WriteLine("Maximum IQ Origin Offset (dB)   {0}", maximumIQOriginOffset);
            Console.WriteLine("Mean IQ Origin Offset (dB)      {0}\n", meanIQOriginOffset);

			Console.WriteLine("----------------Detected TSC-----------------\n");
            for (int i = 0; i < detectedTsc.Length; i++)
            {
                Console.WriteLine("Slot {0}                          {1}\n", i, detectedTsc[i]);
            }
			
            Console.WriteLine("-----------------ORFS Measurements-------------\n");
            Console.WriteLine("----------------Modulation Results--------------\n");
            Console.WriteLine("Modulation Carrier Power (dBm) {0}", modulationCarrierPower);
            for(int i = 0; i < modulationResultsLowerAbsolutePower.Length; i++)
            {
                Console.WriteLine("Offset : {0}", i);
                Console.WriteLine("Lower Absolute Power (dBm)   {0}", modulationResultsLowerAbsolutePower[i]);
                Console.WriteLine("Lower Relative Power (dB)    {0}", modulationResultsLowerRelativePower[i]);
                Console.WriteLine("Upper Absolute Power (dBm)   {0}", modulationResultsUpperAbsolutePower[i]);
                Console.WriteLine("Upper Relative Power (dB)    {0}\n", modulationResultsUpperRelativePower[i]);
            }
            Console.WriteLine("----------------Switching Results--------------\n");
            Console.WriteLine("Switching Carrier Power (dBm)    {0}", switchingCarrierPower);
            for (int i = 0; i < switchingResultsLowerAbsolutePower.Length; i++)
            {
                Console.WriteLine("Offset : {0}", i);
                Console.WriteLine("Lower Absolute Power (dBm)   {0}", switchingResultsLowerAbsolutePower[i]);
                Console.WriteLine("Lower Relative Power (dB)    {0}", switchingResultsLowerRelativePower[i]);
                Console.WriteLine("Upper Absolute Power (dBm)   {0}", switchingResultsUpperAbsolutePower[i]);
                Console.WriteLine("Upper Relative Power (dB)    {0}\n", switchingResultsUpperRelativePower[i]);
            }
            
            Console.WriteLine("-----------------PVT Measurements--------------\n");
            Console.WriteLine("Slot Measurement Status :    {0}", measurementStatus);
            for (int i = 0; i < numberOfTimeslots; i++)
            {
                Console.WriteLine("\nSlot Measurement : {0}\n", i);
                Console.WriteLine("Average Power (dBm)          {0}", slotAveragePower[i]);
                Console.WriteLine("Burst Width (s)              {0}", slotBurstWidth[i]);
                Console.WriteLine("Maximum Power (dBm)          {0}", slotMaximumPower[i]);
                Console.WriteLine("Minimum Power (dBm)          {0}", slotMinimumPower[i]);
                Console.WriteLine("Burst Threshold (dBm)        {0}", slotBurstThreshold[i]);
                Console.WriteLine("Measurement Status           {0}", slotMeasurementStatus[i]);
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