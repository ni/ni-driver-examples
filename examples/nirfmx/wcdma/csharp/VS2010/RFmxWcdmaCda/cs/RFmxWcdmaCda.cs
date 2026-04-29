//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Uplink Scrambling.
//6. Select CDA measurement and enable traces.
//7. Configure Synchronization Mode and Interval.
//8. Configure Measurement Channel.
//9. Configure Power Unit.
//10. Initiate the Measurement.
//11. Fetch CDA Measurements and Traces.
//12. Close the RFmx session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WcdmaMX;

namespace NationalInstruments.Examples.RFmxWcdmaCda
{
    public class RFmxWcdmaCda
    {
        RFmxInstrMX instrSession;
        RFmxWcdmaMX wcdma;

        string resourceName;
        RFmxWcdmaMXMeasurementTypes measurement;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;                                     /* Hz */
        double centerFrequency;                                                 /* Hz */
        double externalAttenuation;                                             /* dB */

        string digitalEdgeTriggerSource;
        RFmxWcdmaMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;
        double triggerDelay;                                                     /* seconds */
        double referenceLevel;                                                   /* dBm */

        RFmxWcdmaMXCdaSynchronizationMode synchronizationMode;
        int measurementOffset;
        int measurementLength;

        int spreadingFactor;
        int spreadingCode;	                                                     /* s */
        RFmxWcdmaMXCdaMeasurementChannelModulationType modulationType;
        RFmxWcdmaMXCdaMeasurementChannelBranch branch;

        RFmxWcdmaMXCdaPowerUnit powerUnit;

        double timeout;                                                         /* seconds */

        RFmxWcdmaMXUplinkScramblingType uplinkScramblingType;
        int uplinkScramblingCode;
               
        bool enableAllTraces;
        bool enableTrigger;

        double totalPower, totalActivePower, meanActivePower, 
               peakActivePower, meanInactivePower, peakInactivePower;
        double iMeanActivePower, qMeanActivePower, iPeakInactivePower,
               qPeakInactivePower;
        double rmsSymbolEVM, peakSymbolEvm, rmsSymbolMagnitudeError, 
               rmsSymbolPhaseError, meanSymbolPower, chipRateError;

        float[] iCodeDomainPowers = null;
        float[] qCodeDomainPowers = null;
        float[] symbolEVM = null;




        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureWcdma();
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

        void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void InitializeVariables()
        {
            /* Initialize input variables */

            resourceName = "RFSA";
            centerFrequency = 1.95e+9;                                                             /* Hz */
            referenceLevel = 0.000000;                                                             /* dBm */
            externalAttenuation = 0.000000;                                                        /* dB */     
                            
            measurement = RFmxWcdmaMXMeasurementTypes.Cda;

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;                                                 /* Hz */                        
            digitalEdgeTriggerSource = RFmxWcdmaMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.000000;                                                               /* s */                                           
            enableTrigger = false;

            uplinkScramblingType = RFmxWcdmaMXUplinkScramblingType.Long;
            uplinkScramblingCode = 0;

            synchronizationMode = RFmxWcdmaMXCdaSynchronizationMode.Slot;
            measurementOffset = 0;                                                                 /* slots */
            measurementLength = 1;                                                                 /* slots */

            spreadingFactor = 256;
            spreadingCode = 0;
            modulationType = RFmxWcdmaMXCdaMeasurementChannelModulationType.ModulationTypeBpskQpsk;
            branch = RFmxWcdmaMXCdaMeasurementChannelBranch.Q;

            powerUnit = RFmxWcdmaMXCdaPowerUnit.dB;

            timeout = 10;                                                        

            enableAllTraces = true;

        }

        void ConfigureWcdma()
        {
            wcdma = instrSession.GetWcdmaSignalConfiguration();
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger);
            wcdma.ConfigureUplinkScrambling("", uplinkScramblingCode, uplinkScramblingType);
            wcdma.SelectMeasurements("", measurement, enableAllTraces);
            wcdma.Cda.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength);
            wcdma.Cda.Configuration.ConfigureMeasurementChannel("", spreadingFactor, spreadingCode, modulationType, branch);
            wcdma.Cda.Configuration.ConfigurePowerUnit("", powerUnit);
            wcdma.Initiate("", "");
        }

        void RetrieveResults()
        {
            wcdma.Cda.Results.FetchSymbolEvm("", timeout, out rmsSymbolEVM, out peakSymbolEvm, out rmsSymbolMagnitudeError,
                                             out rmsSymbolPhaseError, out meanSymbolPower, out chipRateError);
            wcdma.Cda.Results.FetchCodeDomainPower("", timeout, out totalPower, out totalActivePower, out meanActivePower,
                                                   out peakActivePower, out meanInactivePower, out peakInactivePower);
            wcdma.Cda.Results.FetchCodeDomainIAndQPower("", timeout, out iMeanActivePower, out qMeanActivePower, out iPeakInactivePower,
                                                        out qPeakInactivePower);
            wcdma.Cda.Results.FetchCodeDomainIAndQPowerTrace("", timeout, ref iCodeDomainPowers, ref qCodeDomainPowers);
            wcdma.Cda.Results.FetchSymbolEvmTrace("", timeout, ref symbolEVM);
        }

        void PrintResults()
        {
            Console.WriteLine("\n---------------------- Code Domain Power -------------------\n");
            Console.WriteLine("Total Power (dBm)                              : {0}", totalPower);
            Console.WriteLine("Total Active Power (dB or dBm)                 : {0}", totalActivePower);
            Console.WriteLine("Mean Inactive Power (dB or dBm)                : {0}", meanInactivePower);
            Console.WriteLine("Peak Inactive Power (dB or dBm)                : {0}", peakInactivePower);
            Console.WriteLine("I Peak Inactive Power (dB or dBm)              : {0}", iPeakInactivePower);
            Console.WriteLine("Q Peak Inactive Power (dB or dBm)              : {0}", qPeakInactivePower);

            Console.WriteLine("\n------------------------- Symbol EVM -----------------------\n");
            Console.WriteLine("RMS Symbol EVM (%)                             : {0}", rmsSymbolEVM);
            Console.WriteLine("Peak Symbol EVM (%)                            : {0}", peakSymbolEvm);
            Console.WriteLine("RMS Symbol Magnitude Error (%)                 : {0}", rmsSymbolMagnitudeError);
            Console.WriteLine("RMS Symbol Phase Error (deg)                   : {0}", rmsSymbolPhaseError);
            Console.WriteLine("Mean Symbol Power (dB or dBm)                  : {0}", meanSymbolPower);
            Console.WriteLine("Chip Rate Error (ppm)                          : {0}", chipRateError);
        }

        void CloseSession()
        {
            if (wcdma != null)
            {
                wcdma.Dispose();
                wcdma = null;
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