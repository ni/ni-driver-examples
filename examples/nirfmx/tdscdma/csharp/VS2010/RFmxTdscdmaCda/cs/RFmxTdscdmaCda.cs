//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters..
//5. Select CDA measurement and enable traces.
//6. Configure Synchronization Mode and Offset.
//7. Configure Measurement Channel.
//8. Configure Power Unit.
//9. Initiate the Measurement.
//10. Fetch CDA Measurements and Traces.
//11. Close the RFmx session.


using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.TdscdmaMX;

namespace NationalInstruments.Examples.RFmxTdscdmaCda
{
    public class RFmxTdscdmaCda
    {
        RFmxInstrMX instrSession;
        RFmxTdscdmaMX tdscdma;

        string resourceName, iqPowerEdgeTriggerSource, frequencySource;
        double centerFrequency, referenceLevel, externalAttenuation, minimumQuietTimeDuration, frequencyReferenceFrequency,
               triggerDelay, iqPowerEdgeTriggerLevel;

        RFmxTdscdmaMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        bool enableTrigger;
        RFmxTdscdmaMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
        RFmxTdscdmaMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;


        RFmxTdscdmaMXCdaSynchronizationMode synchronizationMode;
        RFmxTdscdmaMXCdaPowerUnit powerUnit;
        RFmxTdscdmaMXMidambleAutoDetectionMode midambleMode;
        RFmxTdscdmaMXCdaAveragingEnabled averagingEnabled;
        int measurementOffset, SpreadingFactor, midambleShift, channelizationCode, uplinkScramblingCode;
        int averagingCount, maximumNumberOfUsers;

        double timeout;
        double meanRmsSymbolMagnitudeError, maximumPeakSymbolEvm, frequencyError, chipRateError;
        double meanRmsSymbolEvm, meanRmsSymbolPhaseError;
        double meanSymbolPower, meanTotalPower, meanActivePower, maximumPeakActivePower, meanInactivePower, maximumPeakInactivePower;
        double meanTotalActivePower, iqOriginOffset, iqGainImbalance, iqQuadratureError;

        float[] meanCodeDomainPowers, meanSymbolEvm;
        ComplexSingle[] symbolConstellation;

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
            resourceName = "RFSA";
            frequencySource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10E+6;         /* Hz */
            centerFrequency = 1.91E+9;                   /* Hz */
            referenceLevel = 0.00;                       /* dBm */
            externalAttenuation = 0.00;                  /* dB */
            enableTrigger = true;
            SpreadingFactor = 16;
            iqPowerEdgeTriggerSource = "0";
            iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising;
            iqPowerEdgeTriggerLevel = -20.00;            /*dB*/
            iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative;
            triggerDelay = 0.00;                         /* seconds */
            midambleMode = RFmxTdscdmaMXMidambleAutoDetectionMode.MidambleShift;
            maximumNumberOfUsers = 16;
            midambleShift = 8;
            channelizationCode = 1;
            averagingEnabled = RFmxTdscdmaMXCdaAveragingEnabled.False;
            averagingCount = 10;
            uplinkScramblingCode = 0;
            powerUnit = RFmxTdscdmaMXCdaPowerUnit.dB;
            minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTimeDuration = 16E-6;            /* seconds */
            synchronizationMode = RFmxTdscdmaMXCdaSynchronizationMode.Slot;
            measurementOffset = 0;
            timeout = 10.00;                             /* seconds */
        }

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void ConfigureTdscdma()
        {
            /* Get Tdscdma signal */
            tdscdma = instrSession.GetTdscdmaSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencySource, frequencyReferenceFrequency);
            tdscdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope,
                iqPowerEdgeTriggerLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration,
                iqPowerEdgeTriggerLevelType, enableTrigger);
            tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.Cda, true);
            tdscdma.Cda.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            tdscdma.Cda.Configuration.ConfigureSynchronizationModeAndOffset("", synchronizationMode, measurementOffset);
            tdscdma.Cda.Configuration.ConfigureMeasurementChannel("", SpreadingFactor, channelizationCode);
            tdscdma.Cda.Configuration.ConfigurePowerUnit("", powerUnit);
            tdscdma.ConfigureMidambleShift("", midambleMode, maximumNumberOfUsers, midambleShift);
            tdscdma.ConfigureUplinkScramblingCode("", uplinkScramblingCode);
            tdscdma.Initiate("", "");
        }

        private void RetrieveResults()
        {
            tdscdma.Cda.Results.FetchIQImpairments("", timeout, out iqOriginOffset, out iqGainImbalance, out iqQuadratureError);
            tdscdma.Cda.Results.FetchCodeDomainPower("", timeout, out meanTotalPower, out meanTotalActivePower, out meanActivePower,
                out maximumPeakActivePower, out meanInactivePower, out maximumPeakInactivePower);
            tdscdma.Cda.Results.FetchSymbolEvm("", timeout, out meanRmsSymbolEvm, out maximumPeakSymbolEvm, out frequencyError,
                out chipRateError, out meanRmsSymbolMagnitudeError, out meanRmsSymbolPhaseError, out meanSymbolPower);
            tdscdma.Cda.Results.FetchMeanSymbolEvmTrace("", timeout, ref meanSymbolEvm);
            tdscdma.Cda.Results.FetchSymbolConstellationTrace("", timeout, ref symbolConstellation);
            tdscdma.Cda.Results.FetchMeanCodeDomainPowerTrace("", timeout, ref meanCodeDomainPowers);
        }

        private void PrintResults()
        {
            Console.WriteLine("--------------------Code Domain Power--------------------");

            Console.WriteLine("Mean Total Power (dBm)                    {0}", meanTotalPower);

            Console.WriteLine("Mean Total Active Power (dB or dBm)       {0}", meanTotalActivePower);

            Console.WriteLine("Mean Active Power (dB or dBm)             {0}", meanActivePower);

            Console.WriteLine("Maximum Peak Active Power (dB or dBm)     {0}", maximumPeakActivePower);

            Console.WriteLine("Mean Inactive Power (dB or dBm)           {0}", meanInactivePower);

            Console.WriteLine("Maximum Peak Inactive Power (dB or dBm)   {0}", maximumPeakInactivePower);

            Console.WriteLine("\n--------------------Symbol EVM--------------------");

            Console.WriteLine("Mean RMS Symbol EVM (%)                   {0}", meanRmsSymbolEvm);

            Console.WriteLine("Maximum Peak Symbol EVM (%)               {0}", maximumPeakSymbolEvm);

            Console.WriteLine("Frequency  error (Hz)                     {0}", frequencyError);

            Console.WriteLine("Chip Rate Error (ppm)                     {0}", chipRateError);

            Console.WriteLine("Mean RMS Symbol Magnitude Error (%)       {0}", meanRmsSymbolMagnitudeError);

            Console.WriteLine("Mean RMS Symbol Phase error (deg)         {0}", meanRmsSymbolPhaseError);

            Console.WriteLine("Mean Symbol Power (dB or dBm)             {0}", meanSymbolPower);

            Console.WriteLine("\n--------------------IQ Impairments--------------------");

            Console.WriteLine("IQ Origin Offset (dB)                     {0}", iqOriginOffset);

            Console.WriteLine("IQ Gain Imbalance (dB)                    {0}", iqGainImbalance);

            Console.WriteLine("IQ Quadrature Error (deg)                 {0}", iqQuadratureError);

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

