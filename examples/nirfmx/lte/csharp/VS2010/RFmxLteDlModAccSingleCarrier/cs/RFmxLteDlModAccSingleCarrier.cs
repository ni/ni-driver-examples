//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Configure operating Band.
//7. Configure Duplex Scheme.
//8. Select Downlink as Link Direction.
//9. Configure Downlink Test Model. 
//10. Select ModAcc measurement and enable Traces.
//11. Configure Averaging Parameters for ModAcc measurement.
//12. Select Frame as Synchronization Mode and configure Measurement Interval.
//13. Configure EVM Unit.
//14. Initiate the Measurement.
//15[A-E]. Fetch ModAcc Measurements and Traces.
//16. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteDLModAccSingleCarrier
{
    public class RFmxLteDLModAccSingleCarrier
    {
        RFmxInstrMX instrSession;
        RFmxLteMX lte;
        string rfsaResourceName;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        bool enableTrigger;
        string digitalEdgeTriggerSource;
        RFmxLteMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;
        double triggerDelay;

        double componentCarrierBandwidth;
        double componentCarrierFrequency;
        int cellID;
        RFmxLteMXDownlinkTestModel downlinkTestModel;

        RFmxLteMXModAccAveragingEnabled averagingEnabled;
        int averagingCount;

        RFmxLteMXModAccSynchronizationMode synchronizationMode;
        int measurementOffset;
        int measurementLength;
        RFmxLteMXModAccEvmUnit evmUnit;
        int band;
        RFmxLteMXLinkDirection linkDirection;
        RFmxLteMXDuplexScheme duplexScheme;
        RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration;

        double timeout;

        double meanRmsCompositeEvm;
        double maxPeakCompositeEvm;
        double meanFrequencyError;
        int peakCompositeEvmSlotIndex;
        int peakCompositeEvmSymbolIndex;
        int peakCompositeEvmSubcarrierIndex;
        double meanIQOriginOffset;
        double meanIQGainImbalance;
        double meanIQQuadratureError;
        double meanRmsEvm;
        double meanRmsQpskEvm;
        double meanRms16QamEvm;
        double meanRms64QamEvm;
        double meanRms256QamEvm;
        double meanRms1024QamEvm;
        ComplexSingle[] qpskConstellation, qam16Constellation, qam64Constellation, qam256Constellation, qam1024Constellation;
        AnalogWaveform<float> meanRmsEvmPerSubcarrier;

        void CloseSession()
        {
            if (lte != null)
            {
                lte.Dispose();
                lte = null;
            }
            if (instrSession != null)
            {
                instrSession.Close();
                instrSession = null;
            }
        }

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureLte();
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
            instrSession = new RFmxInstrMX(rfsaResourceName, "");
        }

        void InitializeVariables()
        {
            rfsaResourceName = "RFSA";

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e6;                              /* (Hz) */

            centerFrequency = 2.14e9;                                        /* (Hz) */
            referenceLevel = 0.00;                                           /* (dBm) */
            externalAttenuation = 0.0;                                       /* (dBm) */

            enableTrigger = false;
            digitalEdgeTriggerSource = RFmxLteMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.0; /* (s) */

            componentCarrierBandwidth = 10e6;                                /* (Hz) */
            componentCarrierFrequency = 0.0;                                 /* (Hz) */
            cellID = 0;
            downlinkTestModel = RFmxLteMXDownlinkTestModel.TM1_1;

            averagingEnabled = RFmxLteMXModAccAveragingEnabled.False;
            averagingCount = 10;

            synchronizationMode = RFmxLteMXModAccSynchronizationMode.Frame;
            measurementOffset = 0;                                           /*(slots) */
            measurementLength = 1;                                           /*(slots) */

            linkDirection = RFmxLteMXLinkDirection.Downlink;
            uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;
            duplexScheme = RFmxLteMXDuplexScheme.Fdd;

            band = 1;

            evmUnit = RFmxLteMXModAccEvmUnit.Percentage;

            timeout = 10.0; /* (s) */
        }

        void ConfigureLte()
        {
            lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            lte.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger);
            lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID);
            lte.ConfigureBand("", band);
            lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration);
            lte.ConfigureLinkDirection("", linkDirection);
            lte.ComponentCarrier.ConfigureDownlinkTestModel("", downlinkTestModel);
            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, true);
            lte.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength);
            lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit);
            lte.Initiate("", "");
        }

        void RetrieveResults()
        {
            lte.ModAcc.Results.FetchCompositeEvm("", timeout, out meanRmsCompositeEvm, out maxPeakCompositeEvm, 
                                                out meanFrequencyError,
                out peakCompositeEvmSymbolIndex, out peakCompositeEvmSubcarrierIndex, out peakCompositeEvmSlotIndex);
            lte.ModAcc.Results.FetchIQImpairments("", timeout, out meanIQOriginOffset, out meanIQGainImbalance, 
                                                  out meanIQQuadratureError);
            lte.ModAcc.Results.FetchPdschEvm("", timeout, out meanRmsEvm, out meanRmsQpskEvm, out meanRms16QamEvm,
                                             out meanRms64QamEvm, out meanRms256QamEvm);
            lte.ModAcc.Results.FetchPdsch1024QamEvm("", timeout, out meanRms1024QamEvm);
            lte.ModAcc.Results.FetchPdschQpskConstellation("", timeout, ref qpskConstellation);
            lte.ModAcc.Results.FetchPdsch16QamConstellation("", timeout, ref qam16Constellation);
            lte.ModAcc.Results.FetchPdsch64QamConstellation("", timeout, ref qam64Constellation);
            lte.ModAcc.Results.FetchPdsch256QamConstellation("", timeout, ref qam256Constellation);
            lte.ModAcc.Results.FetchPdsch1024QamConstellation("", timeout, ref qam1024Constellation);
            lte.ModAcc.Results.FetchEvmPerSubcarrierTrace("", timeout, ref meanRmsEvmPerSubcarrier);
        }

        void PrintResults()
        {
            Console.WriteLine("------------------Measurement------------------");
            Console.WriteLine("Mean RMS Composite EVM  (% or dB)       : {0}", meanRmsCompositeEvm);
            Console.WriteLine("Mean RMS  EVM  (% or dB)                : {0}", meanRmsEvm);
            Console.WriteLine("Mean RMS QPSK EVM  (% or dB)            : {0}", meanRmsQpskEvm);
            Console.WriteLine("Mean RMS 16QAM EVM  (% or dB)           : {0}", meanRms16QamEvm);
            Console.WriteLine("Mean RMS 64QAM EVM  (% or dB)           : {0}", meanRms64QamEvm);
            Console.WriteLine("Mean RMS 256QAM EVM (% or dB)           : {0}", meanRms256QamEvm);
            Console.WriteLine("Mean RMS 1024QAM EVM (% or dB)          : {0}", meanRms1024QamEvm);
            Console.WriteLine("Mean Frequency Error  (Hz)              : {0}", meanFrequencyError);
            Console.WriteLine("Mean IQ Origin Offset  (dBc)            : {0}", meanIQOriginOffset);
            Console.WriteLine("Mean IQ Gain Imbalance  (dB)            : {0}", meanIQGainImbalance);
            Console.WriteLine("Mean IQ Quadrature Error  (deg)         : {0}", meanIQQuadratureError);
        }

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

    }
}