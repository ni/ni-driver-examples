//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Channel Configuration Mode.
//6. Configure Physical Layer Subtype.
//7. Configure Uplink Data Modulation Type.
//8. Configure Uplink Spreading Parameters. 
//9. Select CDA measurement and enable traces.
//10. Configure Synchronization Mode and Interval.
//11. Configure Measurement Channel.
//12. Configure Power Unit.
//13. Initiate the Measurement.
//14. Fetch CDA Measurements and Traces.
//15. Close the RFmx session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.EvdoMX;

namespace NationalInstruments.Examples.RFmxEvdoCda
{
    public class RFmxEvdoCda
    {
        RFmxInstrMX instrSession;
        RFmxEvdoMX evdo;

        string resourceName;
        RFmxEvdoMXMeasurementTypes measurement;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;                                    
        double centerFrequency;                                                 
        double externalAttenuation;                                             

        string digitalEdgeTriggerSource;
        RFmxEvdoMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;
        double triggerDelay;                                                    
        double referenceLevel;                                                  

        long uplinkSpreadingIMask;
        long uplinkSpreadingQMask;
        RFmxEvdoMXCdaSynchronizationMode synchronizationMode;
        RFmxEvdoMXChannelConfigurationMode channelConfigurationMode; 
        int measurementOffset;
        int measurementLength;

        int walshCodeNumber;	                                                     
        int walshCodeLength;
        RFmxEvdoMXCdaUplinkBranch branch;

        RFmxEvdoMXCdaPowerUnit powerUnit;
        RFmxEvdoMXPhysicalLayerSubtype physicalLayerSubtype;
        RFmxEvdoMXUplinkDataModulationType uplinkDataModulationType;

        double timeout;                                                         
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
                ConfigureEvdo();
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
            centerFrequency = 833.49e+6;                                          /* Hz */
            referenceLevel = 0.000000;                                            /* dBm */
            externalAttenuation = 0.000000;                                       /* dB */

            measurement = RFmxEvdoMXMeasurementTypes.Cda;

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;                                /* Hz */

            digitalEdgeTriggerSource = RFmxEvdoMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.000000;                                              /* seconds */
            enableTrigger = false;

            uplinkSpreadingIMask = 0;
            uplinkSpreadingQMask = 0;
            synchronizationMode = RFmxEvdoMXCdaSynchronizationMode.Slot;
            channelConfigurationMode = RFmxEvdoMXChannelConfigurationMode.AutoDetect;
            measurementOffset = 0;                                                /* slots */
            measurementLength = 1;                                                /* slots */

            walshCodeNumber = 0;
            walshCodeLength = 16;
            branch = RFmxEvdoMXCdaUplinkBranch.I;

            powerUnit = RFmxEvdoMXCdaPowerUnit.dB;
            physicalLayerSubtype = RFmxEvdoMXPhysicalLayerSubtype.Subtype0_1;
            uplinkDataModulationType = RFmxEvdoMXUplinkDataModulationType.Auto;

            timeout = 10;                                                         /* seconds */

            enableAllTraces = true;
        }

        void ConfigureEvdo()
        {
            evdo = instrSession.GetEvdoSignalConfiguration();
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            evdo.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            evdo.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger);
            evdo.ConfigureChannelConfigurationMode("", channelConfigurationMode);
            evdo.ConfigurePhysicalLayerSubtype("", physicalLayerSubtype);
            evdo.ConfigureUplinkDataModulationType("", uplinkDataModulationType);
            evdo.ConfigureUplinkSpreading("", uplinkSpreadingIMask, uplinkSpreadingQMask);
            evdo.SelectMeasurements("", measurement, enableAllTraces);
            evdo.Cda.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength);
            evdo.Cda.Configuration.ConfigureUplinkMeasurementChannel("", walshCodeLength, walshCodeNumber, branch);
            evdo.Cda.Configuration.ConfigurePowerUnit("", powerUnit);
            evdo.Initiate("", "");
        }

        void RetrieveResults()
        {
            evdo.Cda.Results.FetchUplinkCodeDomainPower("", timeout, out totalPower, out totalActivePower, out meanActivePower, out peakActivePower, out meanInactivePower, out peakInactivePower);
            evdo.Cda.Results.FetchUplinkCodeDomainIAndQPower("", timeout, out iMeanActivePower, out qMeanActivePower, out iPeakInactivePower, out qPeakInactivePower);
            evdo.Cda.Results.FetchUplinkSymbolEvm("", timeout, out rmsSymbolEvm, out peakSymbolEvm, out rmsSymbolMagnitudeError, out rmsSymbolPhaseError, out meanSymbolPower, out frequencyError, out chipRateError);
            evdo.Cda.Results.FetchIQImpairments("", timeout, out iqOriginOffset, out iqGainImbalance, out iqQuadratureError);
            evdo.Cda.Results.FetchUplinkCodeDomainIandQPowerTrace("", timeout, ref iCodeDomainPowers, ref qCodeDomainPowers);
            evdo.Cda.Results.FetchUplinkSymbolEvmTrace("", timeout, ref symbolEvm);
            evdo.Cda.Results.FetchUplinkSymbolConstellationTrace("", timeout, ref symbolConstellation);
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
            if (evdo != null)
            {
                evdo.Dispose();
                evdo = null;
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