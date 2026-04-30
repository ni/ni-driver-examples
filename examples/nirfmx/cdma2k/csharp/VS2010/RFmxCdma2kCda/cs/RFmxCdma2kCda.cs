//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Radio Configuration 
//6. Configure Uplink Scrambling.
//7. Select CDA measurement and enable traces.
//8. Configure Synchronization Mode and Interval.
//9. Configure Measurement Channel.
//10. Configure Power Unit.
//11. Initiate the Measurement.
//12. Fetch CDA Measurements and Traces.
//13. Close the RFmx session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.Cdma2kMX;

namespace NationalInstruments.Examples.RFmxCdma2kCda
{
    public class RFmxCdma2kCda
    {
        RFmxInstrMX instrSession;
        RFmxCdma2kMX cdma2k;

        string resourceName;
        RFmxCdma2kMXMeasurementTypes measurement;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;                                     /* Hz */
        double centerFrequency;                                                 /* Hz */
        double externalAttenuation;                                             /* dB */

        string digitalEdgeTriggerSource;
        RFmxCdma2kMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;
        double triggerDelay;                                                    /* seconds */
        double referenceLevel;                                                  /* dBm */


        RFmxCdma2kMXRadioConfiguration radioConfiguration;
        RFmxCdma2kMXCdaSynchronizationMode synchronizationMode;
        int uplinkSpreadingLongCodeMask;
        int measurementOffset;
        int measurementLength;

        int walshCodeNumber;	                                                /* s */
        int walshCodeLength;
        RFmxCdma2kMXCdaMeasurementChannelBranch branch;

        RFmxCdma2kMXCdaPowerUnit powerUnit;

        double timeout;                                                         /* seconds */

               
        bool enableAllTraces;
        bool enableTrigger;

        double totalPower, totalActivePower, meanActivePower, 
               peakActivePower, meanInactivePower, peakInactivePower;
        double iMeanActivePower, qMeanActivePower, iPeakInactivePower,
               qPeakInactivePower, iqOriginOffset, iqGainImbalance,
               iqQuadratureError;
        double rmsSymbolEvm, peakSymbolEvm, rmsSymbolMagnitudeError,
               rmsSymbolPhaseError, meanSymbolPower, frequencyError,
               chipRateError;

        float[] iCodeDomainPowers;
        float[] qCodeDomainPowers ;
        float[] symbolEvm ;
        ComplexSingle[] symbolConstellation;




        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureCdma2k();
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
            centerFrequency = 833.49e+6;                                         /* Hz */
            referenceLevel = 0.000000;                                           /* dBm */
            externalAttenuation = 0.000000;                                      /* dB */

            measurement = RFmxCdma2kMXMeasurementTypes.Cda;

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;                               /* Hz */

            digitalEdgeTriggerSource = RFmxCdma2kMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.000000;                                             /* seconds */
            enableTrigger = false;

            radioConfiguration = RFmxCdma2kMXRadioConfiguration.RC3;
            uplinkSpreadingLongCodeMask = 0;
            synchronizationMode = RFmxCdma2kMXCdaSynchronizationMode.Slot;
            measurementOffset = 0;                                               /* slots */
            measurementLength = 1;                                               /* slots */                           

            walshCodeNumber = 0;
            walshCodeLength = 64;
            branch = RFmxCdma2kMXCdaMeasurementChannelBranch.I;

            powerUnit = RFmxCdma2kMXCdaPowerUnit.dB;

            timeout = 10;                                                        /* seconds */

            enableAllTraces = true;

        }

        void ConfigureCdma2k()
        {
            cdma2k = instrSession.GetCdma2kSignalConfiguration();
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            cdma2k.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            cdma2k.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay,
    	                enableTrigger);
            cdma2k.ConfigureRadioConfiguration("", radioConfiguration);
            cdma2k.ConfigureUplinkSpreading("", uplinkSpreadingLongCodeMask);
            cdma2k.SelectMeasurements("", measurement, enableAllTraces);
            cdma2k.Cda.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset,
         	            measurementLength);
            cdma2k.Cda.Configuration.ConfigureMeasurementChannel("", walshCodeLength, walshCodeNumber, branch);
            cdma2k.Cda.Configuration.ConfigurePowerUnit("", powerUnit);
            cdma2k.Initiate("", "");
        }

        void RetrieveResults()
        {
            cdma2k.Cda.Results.FetchCodeDomainPower("", timeout, out totalPower, out totalActivePower, 
			            out meanActivePower, out peakActivePower, out meanInactivePower, out peakInactivePower);
            cdma2k.Cda.Results.FetchCodeDomainIAndQPower("", timeout, out iMeanActivePower, out qMeanActivePower,
                        out iPeakInactivePower, out qPeakInactivePower);
            cdma2k.Cda.Results.FetchSymbolEvm("", timeout, out rmsSymbolEvm, out peakSymbolEvm, out rmsSymbolMagnitudeError,
                        out rmsSymbolPhaseError, out meanSymbolPower, out frequencyError, out chipRateError);
            cdma2k.Cda.Results.FetchIQImpairments("", timeout, out iqOriginOffset, out iqGainImbalance, out iqQuadratureError);
            cdma2k.Cda.Results.FetchCodeDomainIAndQPowerTrace("", timeout, ref iCodeDomainPowers, ref qCodeDomainPowers);
            cdma2k.Cda.Results.FetchSymbolEvmTrace("", timeout, ref symbolEvm);
            cdma2k.Cda.Results.FetchSymbolConstellationTrace("", timeout, ref symbolConstellation);
        }

        void PrintResults()
        {
            Console.WriteLine("\n---------------------- Code Domain Power -------------------\n");
            Console.WriteLine("Total Power (dBm)                              : {0}\n", totalPower);
            Console.WriteLine("Total Active Power (dB or dBm)                 : {0}\n", totalActivePower);
            Console.WriteLine("Mean Inactive Power (dB or dBm)                : {0}\n", meanInactivePower);
            Console.WriteLine("Peak Inactive Power (dB or dBm)                : {0}\n", peakInactivePower);
            Console.WriteLine("I Peak Inactive Power (dB or dBm)              : {0}\n", iPeakInactivePower);
            Console.WriteLine("Q Peak Inactive Power (dB or dBm)              : {0}\n", qPeakInactivePower);

            Console.WriteLine("\n---------------------- IQ Impairments ----------------------\n");
            Console.WriteLine("I/Q Origin Offset (dB)                         : {0}\n", iqOriginOffset);
            Console.WriteLine("I/Q Gain Imbalance (dB)                        : {0}\n", iqGainImbalance);
            Console.WriteLine("I/Q Quadrature Error (deg)                     : {0}\n", iqQuadratureError);

            Console.WriteLine("\n------------------------- Symbol EVM -----------------------\n");
            Console.WriteLine("RMS Symbol EVM (%)                             : {0}\n", rmsSymbolEvm);
            Console.WriteLine("Peak Symbol EVM (%)                            : {0}\n", peakSymbolEvm);
            Console.WriteLine("Frequency Error (Hz)                           : {0}\n", frequencyError);
            Console.WriteLine("RMS Symbol Magnitude Error (%)                 : {0}\n", rmsSymbolMagnitudeError);
            Console.WriteLine("RMS Symbol Phase Error (deg)                   : {0}\n", rmsSymbolPhaseError);
            Console.WriteLine("Mean Symbol Power (dB or dBm)                  : {0}\n", meanSymbolPower);
            Console.WriteLine("Chip Rate Error (ppm)                          : {0}\n", chipRateError);
        }

        void CloseSession()
        {
            if (cdma2k != null)
            {
                cdma2k.Dispose();
                cdma2k = null;
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