//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure operating Band.
//7. Configure Duplex Mode.
//8. Select Downlink as Link Direction.
//9. Configure Component Carriers.
//10. Configure Downlink Test Model.
//11. Select ModAcc measurement and enable Traces.
//12. Configure Averaging Parameters for ModAcc measurement.
//13. Select Frame as Synchronization Mode and configure Measurement Interval.
//14. Configure EVM Unit.
//15. Initiate the Measurement.
//16. Fetch ModAcc Measurements and Traces.
//17. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteDLModAccContiguousMultiCarrier
{
    public class RFmxLteDLModAccContiguousMultiCarrier
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

        const int numberOfComponentCarriers = 2;
        double[] componentCarrierBandwidth = { 20e6, 20e6 };
        double[] componentCarrierFrequency = { -9.9e6, 9.9e6 };
        int[] componentCarrierCellId = { 0, 0 };
        RFmxLteMXDownlinkTestModel[] downlinkTestModel = { RFmxLteMXDownlinkTestModel.TM1_1, RFmxLteMXDownlinkTestModel.TM1_1 };

        RFmxLteMXModAccAveragingEnabled averagingEnabled;
        int averagingCount;

        double timeout;

        RFmxLteMXComponentCarrierSpacingType componentSpacingType;
        int componentCarrierAtCenterFrequency;

        int band;
        RFmxLteMXDuplexScheme duplexScheme;
        RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration;
        RFmxLteMXLinkDirection linkDirection;

        RFmxLteMXModAccSynchronizationMode synchronizationMode;
        int measurementOffset;
        int measurementLength;

        RFmxLteMXModAccEvmUnit evmUnit;

        string subblockCarrierString;

        double[] meanRmsCompositeEvm;
        double[] maximumPeakCompositeEvm;
        double[] meanFrequencyError;
        int[] peakCompositeEvmSlotIndex;
        int[] peakCompositeEvmSymbolIndex;
        int[] peakCompositeEvmSubcarrierIndex;

        double[] meanIQOriginOffset;
        double[] meanIQGainImbalance;
        double[] meanIQQuadratureError;

        double[] meanRmsEvm;
        double[] meanRmsQpskEvm;
        double[] meanRms16QamEvm;
        double[] meanRms64QamEvm;
        double[] meanRms256QamEvm;
        double[] meanRms1024QamEvm;

        ComplexSingle[][] qpskConstellation;
        AnalogWaveform<float>[] meanRmsEvmPerSubcarrier;

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


        void InitializeVariables()
        {
            rfsaResourceName = "RFSA";

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e6;                                   /* (Hz) */

            centerFrequency = 2.14e9;                                             /* (Hz) */
            referenceLevel = 0.00;                                                /* (dBm) */
            externalAttenuation = 0.0;                                            /* (dBm) */

            enableTrigger = false;
            digitalEdgeTriggerSource = RFmxLteMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.0;                                                   /* (s) */

            componentSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal;
            componentCarrierAtCenterFrequency = -1;

            band = 1;
            duplexScheme = RFmxLteMXDuplexScheme.Fdd;
            uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;
            linkDirection = RFmxLteMXLinkDirection.Downlink;

            qpskConstellation = new ComplexSingle[numberOfComponentCarriers][];

            meanRmsEvmPerSubcarrier = new AnalogWaveform<float>[numberOfComponentCarriers];

            averagingEnabled = RFmxLteMXModAccAveragingEnabled.False;
            averagingCount = 10;

            synchronizationMode = RFmxLteMXModAccSynchronizationMode.Frame;
            measurementOffset = 0;
            measurementLength = 1;

            evmUnit = RFmxLteMXModAccEvmUnit.Percentage;

            timeout = 10.0;                                                       /* (s) */
        }

        void ConfigureLte()
        {
            /* Create a new RFmx Session */
            lte = instrSession.GetLteSignalConfiguration();
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

            lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            lte.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource,
                                                         digitalEdgeTriggerEdge, triggerDelay, enableTrigger);
            lte.ComponentCarrier.ConfigureSpacing("", componentSpacingType,
                                                    componentCarrierAtCenterFrequency);
            lte.ConfigureBand("", band);
            lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration);
            lte.ConfigureLinkDirection("", linkDirection);
            lte.ConfigureNumberOfComponentCarriers("", numberOfComponentCarriers);
            lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth,
                componentCarrierFrequency, componentCarrierCellId);
            lte.ComponentCarrier.ConfigureDownlinkTestModelArray("", downlinkTestModel);
            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, true);
            lte.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
                measurementOffset, measurementLength);
            lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit);
            lte.Initiate("", "");
        }

        void RetrieveResults()
        {
            lte.ModAcc.Results.FetchCompositeEvmArray("", timeout, ref meanRmsCompositeEvm,
                ref maximumPeakCompositeEvm, ref meanFrequencyError, ref peakCompositeEvmSymbolIndex,
                ref peakCompositeEvmSubcarrierIndex, ref peakCompositeEvmSlotIndex);
            lte.ModAcc.Results.FetchIQImpairmentsArray("", timeout, ref meanIQOriginOffset,
                ref meanIQGainImbalance, ref meanIQQuadratureError);
            lte.ModAcc.Results.FetchPdschEvmArray("", timeout, ref meanRmsEvm, ref meanRmsQpskEvm,
                 ref meanRms16QamEvm, ref meanRms64QamEvm, ref meanRms256QamEvm);
            lte.ModAcc.Results.FetchPdsch1024QamEvmArray("", timeout, ref meanRms1024QamEvm);
            for (int i = 0; i < numberOfComponentCarriers; i++)
            {
                subblockCarrierString = RFmxLteMX.BuildCarrierString("", i);
                lte.ModAcc.Results.FetchPdschQpskConstellation(subblockCarrierString, timeout, ref qpskConstellation[i]);
                lte.ModAcc.Results.FetchEvmPerSubcarrierTrace(subblockCarrierString, timeout, ref meanRmsEvmPerSubcarrier[i]);
            }
        }

        void PrintResults()
        {
            Console.WriteLine("------------------------Measurements------------------------");
            for (int i = 0; i < numberOfComponentCarriers; i++)
            {
                Console.WriteLine("\nCarrier {0}\n", i);
                Console.WriteLine("Mean RMS Composite EVM  (% or dB)    : {0}", meanRmsCompositeEvm[i]);
                Console.WriteLine("Mean RMS EVM  (% or dB)              : {0}", meanRmsEvm[i]);
                Console.WriteLine("Mean RMS QPSK EVM  (% or dB)         : {0}", meanRmsQpskEvm[i]);
                Console.WriteLine("Mean RMS 16QAM EVM  (% or dB)        : {0}", meanRms16QamEvm[i]);
                Console.WriteLine("Mean RMS 64QAM EVM  (% or dB)        : {0}", meanRms64QamEvm[i]);
                Console.WriteLine("Mean RMS 256QAM EVM  (% or dB)       : {0}", meanRms256QamEvm[i]);
                Console.WriteLine("Mean RMS 1024QAM EVM  (% or dB)      : {0}", meanRms1024QamEvm[i]);
                Console.WriteLine("Mean Frequency Error  (Hz)           : {0}", meanFrequencyError[i]);
                Console.WriteLine("Mean IQ Gain Imbalance  (dB)         : {0}", meanIQGainImbalance[i]);
                Console.WriteLine("Mean IQ Origin Offset  (dBc)         : {0}", meanIQOriginOffset[i]);
                Console.WriteLine("Mean IQ Quadrature Error  (deg)      : {0}", meanIQQuadratureError[i]);
            }
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(rfsaResourceName, "");
        }

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

    }
}