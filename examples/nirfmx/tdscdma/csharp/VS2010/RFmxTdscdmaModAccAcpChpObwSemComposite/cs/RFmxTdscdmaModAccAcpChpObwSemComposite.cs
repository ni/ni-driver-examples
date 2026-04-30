//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties: Clock Source and Clock Frequency
//3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation
//4. Configure the trigger properties
//5. Select all measurements (ModAcc, ACP, CHP, OBW, SEM)  and enable the traces
//6. Configure Measurement Offset/ Length, Uplink Scrambling Code and Midamble Code/ Shift for the ModAcc measurement
//7. Configure Sweep Time and Averaging Parameters for the ACP measurement
//8. Configure Sweep Time and Averaging Parameters for the CHP measurement
//9. Configure Sweep Time and Averaging Parameters for the OBW measurement
//10. Configure Sweep Time and Averaging Parameters for the SEM measurement
//11. Initiate Measurement
//12. Fetch SEM Measurements and Traces
//13. Fetch OBW Measurements and Traces
//14. Fetch CHP Measurements and Traces
//15. Fetch ACP Measurements and Traces
//16. Fetch ModAcc Measurements and Traces
//17. Close the RFmx session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.TdscdmaMX;

namespace NationalInstruments.Examples.RFmxTdscdmaModAccAcpChpObwSemComposite
{
    public class RFmxTdscdmaModAccAcpChpObwSemComposite
    {
        RFmxInstrMX instrSession;
        RFmxTdscdmaMX tdscdma;
        string resourceName, frequencyReferenceSource,iqPowerEdgeTriggerSource;
        double centerFrequency, referenceLevel, externalAttenuation, frequencyReferenceFrequency,sweepTimeInterval,
               triggerDelay, minimumQuietTimeDuration, iqPowerEdgeTriggerLevel,timeout;

        int averagingCount, measurementOffset, measurementLength, maximumNumberOfUsers, midambleShift, uplinkScramblingCode;

        RFmxTdscdmaMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
        RFmxTdscdmaMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        bool enableTrigger;
        RFmxTdscdmaMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
                
        RFmxTdscdmaMXModAccSynchronizationMode synchronizationMode;

        RFmxTdscdmaMXMidambleAutoDetectionMode midambleAutoDetectionMode;


        double rmsCompositeEvm, peakCompositeEvm, compositeRho, frequencyError, chipRateError, rmsCompositeMagnitudeError,
               rmsCompositePhaseError;

        double carrierAbsolutePowerAcp;
        double[] lowerRelativePower;
        double[] upperRelativePower;
        double[] lowerAbsolutePower;
        double[] upperAbsolutePower;

        double carrierAbsolutePowerChp;

        double stopFrequency, startFrequency, occupiedBandwidth, absolutePower;

        double carrierAbsoluteIntegratedPower;
        double[] lowerOffsetMargin;
        double[] lowerOffsetMarginAbsolutePower;
        double[] lowerOffsetMarginRelativePower;
        double[] lowerOffsetMarginFrequency;
        RFmxTdscdmaMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;

        RFmxTdscdmaMXSemMeasurementStatus measurementStatus;

        double[] upperOffsetMargin;
        double[] upperOffsetMarginAbsolutePower;
        double[] upperOffsetMarginRelativePower;
        double[] upperOffsetMarginFrequency;
        RFmxTdscdmaMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureTdscdma();
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

            centerFrequency = 1.91e+9;             /* Hz */
            referenceLevel = 0.00;                 /* dBm */
            externalAttenuation = 0.00;            /* dB */
            uplinkScramblingCode = 0;
            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e+6;   /* Hz */

            triggerDelay = 0.00;                   /* seconds */
            minimumQuietTimeDuration = 80E-6;      /* seconds */
            iqPowerEdgeTriggerLevel = -20.00;      /*dB*/
            iqPowerEdgeTriggerSource = "0";
            iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising;
            enableTrigger = true;
            iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative;
            minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto;

            //Sweep Time
            sweepTimeInterval = 660e-6;            /* seconds */

            //Averaging
            averagingCount = 10;

            synchronizationMode = RFmxTdscdmaMXModAccSynchronizationMode.Slot;

            measurementOffset = 0;
            measurementLength = 1;
            maximumNumberOfUsers = 16;
            midambleShift = 8;
            midambleAutoDetectionMode = RFmxTdscdmaMXMidambleAutoDetectionMode.MidambleShift;
            timeout = 10;                       /* seconds */

        }

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }
        
        private void ConfigureTdscdma()
        {
            /* Get SpecAn signal */
            tdscdma = instrSession.GetTdscdmaSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            tdscdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, 
                iqPowerEdgeTriggerLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration, 
                iqPowerEdgeTriggerLevelType, enableTrigger);
            
            tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.ModAcc | RFmxTdscdmaMXMeasurementTypes.Acp|
                RFmxTdscdmaMXMeasurementTypes.Chp|RFmxTdscdmaMXMeasurementTypes.Obw|RFmxTdscdmaMXMeasurementTypes.Sem, true);

            
            tdscdma.ConfigureMidambleShift("", midambleAutoDetectionMode, maximumNumberOfUsers, midambleShift);
            tdscdma.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("",synchronizationMode,measurementOffset,
                measurementLength);
            tdscdma.ConfigureUplinkScramblingCode("", uplinkScramblingCode); 
 
             tdscdma.Acp.Configuration.ConfigureSweepTime("", RFmxTdscdmaMXAcpSweepTimeAuto.False, sweepTimeInterval);
             tdscdma.Acp.Configuration.ConfigureAveraging("", RFmxTdscdmaMXAcpAveragingEnabled.False, averagingCount,
                 RFmxTdscdmaMXAcpAveragingType.Rms);

            tdscdma.Chp.Configuration.ConfigureSweepTime("", RFmxTdscdmaMXChpSweepTimeAuto.False, sweepTimeInterval);
            tdscdma.Chp.Configuration.ConfigureAveraging("", RFmxTdscdmaMXChpAveragingEnabled.False, averagingCount,
                RFmxTdscdmaMXChpAveragingType.Rms);

            tdscdma.Obw.Configuration.ConfigureSweepTime("", RFmxTdscdmaMXObwSweepTimeAuto.False, sweepTimeInterval);
            tdscdma.Obw.Configuration.ConfigureAveraging("", RFmxTdscdmaMXObwAveragingEnabled.False, averagingCount,
                RFmxTdscdmaMXObwAveragingType.Rms);
            
            tdscdma.Sem.Configuration.ConfigureSweepTime("", RFmxTdscdmaMXSemSweepTimeAuto.False, sweepTimeInterval);
            tdscdma.Sem.Configuration.ConfigureAveraging("", RFmxTdscdmaMXSemAveragingEnabled.False, averagingCount,
                RFmxTdscdmaMXSemAveragingType.Rms);

            tdscdma.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */
            tdscdma.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref lowerOffsetMeasurementStatus, 
                                                           ref lowerOffsetMargin, 
                                                           ref lowerOffsetMarginFrequency, 
                                                           ref lowerOffsetMarginAbsolutePower, 
                                                           ref lowerOffsetMarginRelativePower);

            tdscdma.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref upperOffsetMeasurementStatus, 
                                                           ref upperOffsetMargin,
                                                           ref upperOffsetMarginFrequency, 
                                                           ref upperOffsetMarginAbsolutePower, 
                                                           ref upperOffsetMarginRelativePower);
            tdscdma.Sem.Results.FetchCarrierAbsoluteIntegratedPower("", timeout, out carrierAbsoluteIntegratedPower);
            tdscdma.Sem.Results.FetchMeasurementStatus("", timeout,out measurementStatus);

            tdscdma.Obw.Results.FetchMeasurement("", timeout, out occupiedBandwidth, out absolutePower, out startFrequency, 
                                                out stopFrequency);


            tdscdma.Chp.Results.FetchCarrierAbsolutePower("", timeout, out carrierAbsolutePowerChp);
      
            tdscdma.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref lowerRelativePower,
                                                           ref upperRelativePower,
                                                           ref lowerAbsolutePower,
                                                           ref upperAbsolutePower);

            tdscdma.Acp.Results.FetchCarrierAbsolutePower("", timeout, out carrierAbsolutePowerAcp);


            tdscdma.ModAcc.Results.FetchCompositeEvm("", timeout, out  rmsCompositeEvm, out  peakCompositeEvm, out  compositeRho, 
                out  frequencyError, out  chipRateError, out  rmsCompositeMagnitudeError, out  rmsCompositePhaseError);
            
        }

        private void PrintResults()
        {
            Console.WriteLine("-----------------------------ModAcc Results----------------------------");
            Console.WriteLine("-------------------Composite EVM Results--------------------");
            Console.WriteLine("RMS Composite EVM (%)                     {0}", rmsCompositeEvm);
            Console.WriteLine("Peak Composite EVM (%)                    {0}", peakCompositeEvm);
            Console.WriteLine("Composite Rho                             {0}", compositeRho);
            Console.WriteLine("Frequency Error (Hz)                      {0}", frequencyError);
            Console.WriteLine("Chip Rate Error (ppm)                     {0}", chipRateError);
            Console.WriteLine("RMS Composite Phase Error (deg)           {0}", rmsCompositePhaseError);
            Console.WriteLine("RMS Composite Magnitude Error (%)         {0}", rmsCompositeMagnitudeError);

            Console.WriteLine("---------------------ACP Results------------------");

            Console.WriteLine("---------------------Carrier Measurements-----------------------\n");
            Console.WriteLine("Carrier Absolute Power (dBm)              {0}", carrierAbsolutePowerAcp);
              
            Console.WriteLine("\n-------------------Offset Channel Measurements------------------\n");
            for (int i = 0; i < lowerRelativePower.Length; i++)
            {
                Console.WriteLine("----Offset {0}\n", i);
                Console.WriteLine("Lower Relative Power (dB)                {0}", lowerRelativePower[i]);
                Console.WriteLine("Upper Relative Power (dB)                {0}", upperRelativePower[i]);
                Console.WriteLine("Lower Absolute Power (dBm)               {0}", lowerAbsolutePower[i]);
                Console.WriteLine("Upper Absolute Power (dBm)               {0}", upperAbsolutePower[i]);
            }                                                              
            Console.WriteLine("---------------------CHP Results-------------------------------");
            Console.WriteLine("Carrier Absolute Power (dBm)             {0}", carrierAbsolutePowerChp);
            Console.WriteLine("---------------------OBW Results------------------------------");
            Console.WriteLine("Occupied Bandwidth (Hz)                  {0}", occupiedBandwidth);
            Console.WriteLine("Absolute Power (dBm)                     {0}", absolutePower);
            Console.WriteLine("Start Frequency (Hz)                     {0}", startFrequency);
            Console.WriteLine("Stop Frequency (Hz)                      {0}", stopFrequency);
                                                                      
            Console.WriteLine("---------------------SEM Results----------------------------");


            Console.WriteLine("Measurement Status  :                  {0}\n", measurementStatus);
            Console.WriteLine("\n-------------------Carrier Measurements-----------------------\n");
            Console.WriteLine("Carrier Absolute Integrated Power(dBm):{0}", carrierAbsoluteIntegratedPower);

            Console.WriteLine("\n-------------------Offset segment measurements ------------------------\n");
            for (int i = 0; i < lowerOffsetMargin.Length; i++)
            {
                Console.WriteLine("Offset {0}\n", i);

                Console.WriteLine("Lower Offset : Margin (dB):                           {0}",
                                  lowerOffsetMargin[i]);
                Console.WriteLine("Lower Offset : Margin Absolute Power (dBm):           {0}",
                                  lowerOffsetMarginAbsolutePower[i]);
                Console.WriteLine("Lower Offset : Margin Relative Power (dB):            {0}",
                                  lowerOffsetMarginRelativePower[i]);
                Console.WriteLine("Lower Offset : Margin Frequency (Hz):                 {0}",
                                  lowerOffsetMarginFrequency[i]);


                Console.WriteLine("Lower Offset : Measurement Status :                   {0}\n", lowerOffsetMeasurementStatus[i]);


                Console.WriteLine("Upper Offset : Margin (dB):                            {0}",
                                  upperOffsetMargin[i]);
                Console.WriteLine("Upper Offset : Margin Absolute Power (dBm):            {0}",
                                  upperOffsetMarginAbsolutePower[i]);
                Console.WriteLine("Upper Offset : Margin Relative Power (dB):             {0}",
                                  upperOffsetMarginRelativePower[i]);
                Console.WriteLine("Upper Offset : Margin Frequency (Hz):                  {0}",
                                  upperOffsetMarginFrequency[i]);

               
                Console.WriteLine("Upper Offset : Measurement Status :                    {0}\n", upperOffsetMeasurementStatus[i]);
                Console.WriteLine("-----------------------------------------------------------------------\n");
            }
        }

        private void CloseSession()
        {
            try
            {
                if (tdscdma != null)
                {
                    tdscdma.Dispose();
                    tdscdma = null;
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

        static private void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }
    }
}
