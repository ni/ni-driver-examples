//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties: Clock Source and Clock Frequency
//3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation
//4. Configure the trigger properties
//5. Select ModAcc measurement and enable the traces
//6. Configure Uplink Scrambling Code
//7. Configure the basic Measurement Settings
//8. Configure the Midamble Settings 
//9. Initiate Measurement
//10. Fetch ModAcc Measurements and Traces
//11. Close the RFmx session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.TdscdmaMX;

namespace NationalInstruments.Examples.TdscdmaModAcc
{
    public class RFmxTdscdmaModAcc
    {
        RFmxInstrMX instrSession;
        RFmxTdscdmaMX tdscdma;
        string resourceName, frequencySource,iqPowerEdgeTriggerSource;
        double centerFrequency, referenceLevel, externalAttenuation, frequencyReferenceFrequency,
               triggerDelay, minimumQuietTimeDuration, iqPowerEdgeTriggerLevel;    
        
        RFmxTdscdmaMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
        RFmxTdscdmaMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        bool enableTrigger;
        RFmxTdscdmaMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
                

        RFmxTdscdmaMXModAccSynchronizationMode synchronizationMode;
        int measurementOffset, measurementLength,uplinkScramblingCode;

        RFmxTdscdmaMXMidambleAutoDetectionMode midambleAutoDetectionMode;
        int maximumNumberOfUsers, midambleShift;

        double timeout;
        double rmsCompositeEvm, peakCompositeEvm, compositeRho, frequencyError, chipRateErrorPpm,
            rmsCompositeMagnitudeError, rmsCompositePhaseError;
        double iqOriginOffset, iqGainImbalance, iqQuadratureError;
        double rmsMidambleEvm, peakMidambleEvm, midambleRho, rmsMidambleMagnitudeError, rmsMidamblePhaseError;
        double midamblePower, dataField1Power, dataField2Power;
        double rmsDataEvm, peakDataEvm, dataRho, rmsDataMagnitudeError, rmsDataPhaseError;
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
            referenceLevel = 0.00;              /* dBm */
            externalAttenuation = 0.00;         /* dB */

            frequencySource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e+6;                  /* Hz */
            uplinkScramblingCode = 0;

            triggerDelay = 0.00;                    /* seconds */
            minimumQuietTimeDuration = 80E-6;     /* seconds */
            iqPowerEdgeTriggerLevel = -20.00;       /*dB*/
            iqPowerEdgeTriggerSource = "0";
            iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising;
            enableTrigger = true;
            iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative;
            minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto;

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
            instrSession.ConfigureFrequencyReference("", frequencySource, frequencyReferenceFrequency);
            tdscdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, 
                iqPowerEdgeTriggerLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration, 
                iqPowerEdgeTriggerLevelType, enableTrigger);
            
            tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.ModAcc, true);

            tdscdma.ConfigureUplinkScramblingCode("",uplinkScramblingCode); 

            tdscdma.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("",synchronizationMode,measurementOffset, measurementLength);
            tdscdma.ConfigureMidambleShift("", midambleAutoDetectionMode, maximumNumberOfUsers, midambleShift);

            tdscdma.Initiate("", "");
        }

        private void RetrieveResults()
        {
            ComplexSingle[] constellation = null;
            AnalogWaveform<float> evm = null;
            /* Retrieve results */
            tdscdma.ModAcc.Results.FetchCompositeEvm("", timeout, out rmsCompositeEvm, out peakCompositeEvm, out compositeRho, 
                out frequencyError, out chipRateErrorPpm, out rmsCompositeMagnitudeError, out rmsCompositePhaseError);
            tdscdma.ModAcc.Results.FetchConstellationTrace("", timeout, ref constellation);
            tdscdma.ModAcc.Results.FetchDataEvm("", timeout, out rmsDataEvm, out peakDataEvm, out dataRho, 
                out rmsDataMagnitudeError, out rmsDataPhaseError);
            tdscdma.ModAcc.Results.FetchMidambleEvm("", timeout, out rmsMidambleEvm, out peakMidambleEvm, out midambleRho,
                out rmsMidambleMagnitudeError, out rmsMidamblePhaseError);
            tdscdma.ModAcc.Results.FetchIQImpairments("", timeout, out iqOriginOffset, out iqGainImbalance, out iqQuadratureError);
            tdscdma.ModAcc.Results.FetchMidambleAndDataPower("",timeout,out midamblePower,out dataField1Power,out dataField2Power);
            tdscdma.ModAcc.Results.FetchEvmTrace("",timeout,ref evm);
        }

        private void PrintResults()
        {
            Console.WriteLine("--------------------Composite EVM Results--------------------");
            Console.WriteLine("RMS Composite EVM (%)             {0}",rmsCompositeEvm);
            Console.WriteLine("Peak Composite EVM (%)            {0}",peakCompositeEvm);
            Console.WriteLine("Composite Rho                     {0}",compositeRho);
            Console.WriteLine("Frequency Error (Hz)              {0}",frequencyError);
            Console.WriteLine("Chip Rate Error (ppm)             {0}",chipRateErrorPpm);
            Console.WriteLine("RMS Composite Phase Error (deg)   {0}",rmsCompositePhaseError);
            Console.WriteLine("RMS Composite Magnitude Error (%) {0}",rmsCompositeMagnitudeError);

            Console.WriteLine("---------------------Data EVM Results------------------------");
            Console.WriteLine("RMS Data EVM (%)                  {0}",rmsDataEvm);
            Console.WriteLine("Peak Data EVM (%)                 {0}",peakDataEvm);
            Console.WriteLine("Data Rho                          {0}",dataRho);
            Console.WriteLine("RMS Data Phase Error (deg)        {0}",rmsDataPhaseError);
            Console.WriteLine("RMS Data Magnitude Error (%)      {0}",rmsDataMagnitudeError);
            Console.WriteLine("Data Field 1 Power (dBm)          {0}",dataField1Power);
            Console.WriteLine("Data Field 2 Power (dBm)          {0}",dataField2Power);
                                                                 

            Console.WriteLine("---------------------IQ Impairments------------------------");
            Console.WriteLine("I/Q Origin Offset (dB)            {0}", iqOriginOffset);
            Console.WriteLine("I/Q Gain Imbalance (dB)           {0}", iqGainImbalance);
            Console.WriteLine("I/Q Quadrature Error (deg)        {0}", iqQuadratureError);
                                                                 
            Console.WriteLine("-------------------Midable EVM Results------------------------");
            Console.WriteLine("RMS Midamble EVM (%)               {0}",rmsMidambleEvm);
            Console.WriteLine("Peak Midamble EVM (%)              {0}",peakMidambleEvm);
            Console.WriteLine("Midamble Rho                       {0}",midambleRho);
            Console.WriteLine("RMS Midamble Phase Error (deg)     {0}",rmsMidamblePhaseError);
            Console.WriteLine("RMS Midamble Magnitude Error (%)   {0}",rmsMidambleMagnitudeError);
            Console.WriteLine("Midamble Power (dBm)               {0}",midamblePower);


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
