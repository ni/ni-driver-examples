//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select ModAcc measurement and enable traces.
//6. Configure Synchronization Mode and Measurement Interval.
//7. Configure Channel Configuration Mode.
//8. Configure Physical Layer Subtype.
//9. Configure Uplink Data Modulation Type.
//10. Configure Uplink Spreading Parameters.
//11. Initiate the Measurement.
//12. Fetch ModAcc Measurements and Traces.
//13. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.EvdoMX;

namespace NationalInstruments.Examples.RFmxEvdoModAcc
{
    public class RFmxEvdoModAcc
    {
        RFmxInstrMX instrSession;
        RFmxEvdoMX evdo;

        string resourceName = "RFSA";
        RFmxEvdoMXMeasurementTypes measurement = RFmxEvdoMXMeasurementTypes.ModAcc;
        string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
        double frequency = 10.0e+6;                     /* Hz */
        double centerFrequency = 833.49e+6;             /* Hz */
        double externalAttenuation = 0.00;              /* dBm */

        string digitalEdgeSource = RFmxEvdoMXConstants.Pfi0;
        RFmxEvdoMXDigitalEdgeTriggerEdge digitalEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising;
        double triggerDelay = 0.00;                 /* seconds */
        double referenceLevel = 0.00;               /* dBm */
        double timeout = 10.00;                     /* seconds */

        bool enableAllTraces = true;
        bool enableTrigger = false;

        RFmxEvdoMXChannelConfigurationMode channelConfigurationMode = RFmxEvdoMXChannelConfigurationMode.AutoDetect;

        RFmxEvdoMXPhysicalLayerSubtype physicalLayerSubtype = RFmxEvdoMXPhysicalLayerSubtype.Subtype0_1;

        RFmxEvdoMXUplinkDataModulationType uplinkDataModulationType = RFmxEvdoMXUplinkDataModulationType.Auto;
        long uplinkSpreadingIMask = 0x0;
        long uplinkSpreadingQMask = 0x0;

        RFmxEvdoMXModAccSynchronizationMode synchronizationMode = RFmxEvdoMXModAccSynchronizationMode.Slot;
        int measurementOffset = 0;
        int measurementLength = 1;

        RFmxEvdoMXModAccUplinkDetectedDataModulationType detectedDataModulationType;
        double rmsEvm;
        double peakEvm;
        double rho;
        double frequencyError;
        double chipRateError;
        double rmsMagnitudeError;
        double rmsPhaseError;
        double iqOriginOffset;
        double iqGainImbalance;
        double iqQuadratureError;
        double peakCde;
        int peakCdeWalshCodeNumber;
        RFmxEvdoMXModAccUplinkPeakCdeBranch peakCdeBranch;
        double peakActiveCde;
        int peakCdeWalshCodeLength;
        int peakActiveCdeWalshCodeNumber;
        RFmxEvdoMXModAccUplinkPeakActiveCdeBranch peakActiveCdeBranch;
        AnalogWaveform<float> evm;
        ComplexSingle[] constellation;

        public void Run()
        {
            try
            {
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

        void ConfigureEvdo()
        {
            /* Get Evdo signal */

            evdo = instrSession.GetEvdoSignalConfiguration();

            /* Configure measurement */

            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequency);
            evdo.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);

            evdo.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);


            evdo.SelectMeasurements("", measurement, enableAllTraces);

            evdo.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
                measurementOffset, measurementLength);

            evdo.ConfigureChannelConfigurationMode("", channelConfigurationMode);
            evdo.ConfigurePhysicalLayerSubtype("", physicalLayerSubtype);
            evdo.ConfigureUplinkDataModulationType("", uplinkDataModulationType);
            evdo.ConfigureUplinkSpreading("", uplinkSpreadingIMask, uplinkSpreadingQMask);
            evdo.Initiate("", "");
        }

        void RetrieveResults()
        {
            /* Retrieve results */

            evdo.ModAcc.Results.FetchUplinkEvm("", timeout, out rmsEvm, out peakEvm, out rho, out frequencyError,
                out chipRateError, out rmsMagnitudeError, out rmsPhaseError);
            evdo.ModAcc.Results.FetchIQImpairments("", timeout, out iqOriginOffset, out iqGainImbalance,
                out iqQuadratureError);
            evdo.ModAcc.Results.FetchUplinkPeakCde("", timeout, out peakCde, out peakCdeWalshCodeNumber, out peakCdeBranch);
            evdo.ModAcc.Results.FetchUplinkPeakActiveCde("", timeout, out peakActiveCde,
                out peakCdeWalshCodeLength, out peakActiveCdeWalshCodeNumber, out peakActiveCdeBranch);
            evdo.ModAcc.Results.FetchUplinkDetectedDataModulationType("", timeout, out detectedDataModulationType);
            evdo.ModAcc.Results.FetchEvmTrace("", timeout, ref evm);
            evdo.ModAcc.Results.FetchConstellationTrace("", timeout, ref constellation);
        }

        void PrintResults()
        {

            Console.WriteLine("\n***************************************************:");
            Console.WriteLine("EVM:");
            Console.WriteLine("RMS EVM (%)                                      : {0}", rmsEvm);
            Console.WriteLine("Peak EVM (%)                                     : {0}", peakEvm);
            Console.WriteLine("Rho                                              : {0}", rho);
            Console.WriteLine("Frequency Error (Hz)                             : {0}", frequencyError);
            Console.WriteLine("Chip Rate Error (ppm)                            : {0}", chipRateError);
            Console.WriteLine("RMS Magnitude Error (%)                          : {0}", rmsMagnitudeError);
            Console.WriteLine("RMS Phase Error (deg)                            : {0}", rmsPhaseError);

            Console.WriteLine("\n***************************************************:");
            Console.WriteLine("IQ Impairments:");
            Console.WriteLine("I/Q Origin Offset (dB)                           : {0}", iqOriginOffset);
            Console.WriteLine("I/Q Gain Imbalance (dB)                          : {0}", iqGainImbalance);
            Console.WriteLine("I/Q Quadrature Error (deg)                       : {0}", iqQuadratureError);

            Console.WriteLine("\n***************************************************:");
            Console.WriteLine("Code Domain Error:");
            Console.WriteLine("Peak CDE (dB)                                    : {0}", peakCde);
            Console.WriteLine("Peak CDE Code                                    : {0}", peakCdeWalshCodeNumber);
            Console.WriteLine("Peak CDE Branch                                  : {0}", peakCdeBranch);
            Console.WriteLine("Peak Active CDE (dB)                             : {0}", peakActiveCde);
            Console.WriteLine("Peak Active CDE Code                             : {0}", peakActiveCdeWalshCodeNumber);
            Console.WriteLine("Peak Active CDE Spreading Factor                 : {0}", peakCdeWalshCodeLength);
            Console.WriteLine("Peak Active CDE Branch                           : {0}", peakActiveCdeBranch);

            Console.WriteLine("\n***************************************************:");
            Console.WriteLine("Data Modulation:");
            Console.WriteLine("Detected Data Modulation Type                    : {0}", detectedDataModulationType);
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
