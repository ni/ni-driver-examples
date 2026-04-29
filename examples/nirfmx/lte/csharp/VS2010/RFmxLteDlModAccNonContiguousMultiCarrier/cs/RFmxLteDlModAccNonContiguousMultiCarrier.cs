//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Duplex Mode.
//6. Configure Link Direction as Downlink.
//7[A-F]. Configure Subblock Parameters.
//7A. Configure Number of Subblocks.
//7B. Configure subblock Frequency.
//7C. Configure Component Carrier Spacing.
//7D. Configure Band.
//7E. Configure Number of Component Carriers.
//7F. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
//8. Configure Downlink Test Model.
//9. Select ModAcc measurement and enable Traces.
//10. Configure Averaging Parameters for ModAcc measurement.
//11. Select Frame as Synchronization Mode and configure Measurement Interval.
//12. Configure EVM Unit.
//13. Initiate the Measurement
//14. Fetch ModAcc Measurements and Traces
//15. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteDLModAccNonContiguousMultiCarrier
{
    /* Input: Subblock inputs structure */
    struct SubblockInput
    {
	    public double subblockFrequency;                                        /*(Hz) */
        public RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;
        public int componentCarrierAtCenterFrequency;
        public int band;
        public double[] componentCarrierBandwidth;                              /*(Hz) */
        public double[] componentCarrierFrequency;                              /*(Hz) */
        public int[] componentCarrierCellId;                                    /*(Hz) */
        public RFmxLteMXDownlinkTestModel[] downlinkTestModel;
    }

    /* Input: Subblock measurement outputs structure */
    struct SubblockMeasurement
    {
        public double[] meanRmsCompositeEvm;                                   /*(dBm) */
        public double[] maximumPeakCompositeEvm;                               /*(dBm) */
        public int[] peakCompositeEvmSlotIndex;                                /*(dB) */
        public int[] peakCompositeEvmSymbolIndex;
        public int[] peakCompositeEvmSubcarrierIndex;
        public double[] meanRmsEvm;                                            /*(% or dB) */
        public double[] meanRmsQpskEvm;                                        /*(% or dB) */
        public double[] meanRms16QamEvm;                                       /*(% or dB) */
        public double[] meanRms64QamEvm;                                       /*(% or dB) */
        public double[] meanRms256QamEvm;                                      /*(% or dB) */
        public double[] meanRms1024QamEvm;                                     /*(% or dB) */
        public double[] meanFrequencyError;                                    /*(Hz) */
        public double[] meanIQOriginOffset;
        public double[] meanIQGainImbalance;
        public double[] meanIQQuadratureError;
        public ComplexSingle[][] qpskConstellation;
        public AnalogWaveform<float>[] meanRmsEvmPerSubcarrier;
    }

    public class RFmxLteDLModAccNonContiguousMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxLteMX lte;

        string rfsaResourceName;

        const int numberOfComponentCarriers = 1;
        const int numberOfSubblocks = 2;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        bool enableTrigger;
        string digitalEdgeTriggerSource;
        RFmxLteMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;
        double triggerDelay;

        RFmxLteMXModAccAveragingEnabled averagingEnabled;
        int averagingCount;

        double timeout;

        string[] subblockString;

        SubblockInput[] subblocks;
        SubblockMeasurement[] subblocksMsr;

        RFmxLteMXLinkDirection linkDirection;
        RFmxLteMXDuplexScheme duplexScheme;
        RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration;
        RFmxLteMXModAccSynchronizationMode synchronizationMode;
        int measurementOffset;
        int measurementLength;
        RFmxLteMXModAccEvmUnit evmUnit;
        string subblockCarrierString;

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
            frequencyReferenceFrequency = 10e6;                                 /* (Hz) */

            centerFrequency = 2.14e9;                                           /* (Hz) */
            referenceLevel = 0.00;                                              /* (dBm) */
            externalAttenuation = 0.0;                                          /* (dBm) */

            enableTrigger = false;
            digitalEdgeTriggerSource = RFmxLteMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.0;                                                 /* (s) */

            averagingEnabled = RFmxLteMXModAccAveragingEnabled.False;
            averagingCount = 10;

            timeout = 10.0;                                                     /* (s) */

            linkDirection = RFmxLteMXLinkDirection.Downlink;
            uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;
            duplexScheme = RFmxLteMXDuplexScheme.Fdd;

            synchronizationMode = RFmxLteMXModAccSynchronizationMode.Frame;
            measurementOffset = 0;
            measurementLength = 1;

            evmUnit = RFmxLteMXModAccEvmUnit.Percentage;

            subblocks = new SubblockInput[numberOfSubblocks] {
                                              new SubblockInput {  
                                                                     subblockFrequency = 0.0,
                                                                     componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal, 
                                                                     componentCarrierAtCenterFrequency = -1, 
                                                                     componentCarrierBandwidth = new double[numberOfComponentCarriers] {20e6}, 
                                                                     componentCarrierFrequency = new double[numberOfComponentCarriers] {0.0}, 
                                                                     componentCarrierCellId = new int[numberOfComponentCarriers]{0}, 
                                                                     band = 1,
                                                                     downlinkTestModel = new RFmxLteMXDownlinkTestModel[numberOfComponentCarriers]{RFmxLteMXDownlinkTestModel.TM1_1}
                                                                 },
                                              new SubblockInput  {
                                                                     subblockFrequency = 30e6, 
                                                                     componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal, 
                                                                     componentCarrierAtCenterFrequency = -1, 
                                                                     componentCarrierBandwidth = new double[numberOfComponentCarriers] {20e6},
                                                                     componentCarrierFrequency =  new double[numberOfComponentCarriers] {0.0}, 
                                                                     componentCarrierCellId = new int[numberOfComponentCarriers]{0},
                                                                     band = 1,
                                                                     downlinkTestModel = new RFmxLteMXDownlinkTestModel[numberOfComponentCarriers]{RFmxLteMXDownlinkTestModel.TM1_1}
                                                                 }
                                                            };

            subblocksMsr = new SubblockMeasurement[numberOfSubblocks];
        }

        void ConfigureLte()
        {
            /* Create a new RFmx Session */
            lte = instrSession.GetLteSignalConfiguration();
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

            lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            lte.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource,
                                                         digitalEdgeTriggerEdge, triggerDelay, enableTrigger);
            lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration);
            lte.ConfigureLinkDirection("", linkDirection);
            lte.ConfigureNumberOfSubblocks("", numberOfSubblocks);

            subblockString = new string[numberOfSubblocks];
            for (int i = 0; i < numberOfSubblocks; i++)
            {
                subblockString[i] = RFmxLteMX.BuildSubblockString("", i);
                lte.SetSubblockFrequency(subblockString[i], subblocks[i].subblockFrequency);
                lte.ComponentCarrier.ConfigureSpacing(subblockString[i], subblocks[i].componentCarrierSpacingType,
                                                        subblocks[i].componentCarrierAtCenterFrequency);
                lte.ConfigureBand(subblockString[i], subblocks[i].band);
                lte.ConfigureNumberOfComponentCarriers(subblockString[i], numberOfComponentCarriers);
                lte.ComponentCarrier.ConfigureArray(subblockString[i], subblocks[i].componentCarrierBandwidth,
                    subblocks[i].componentCarrierFrequency, subblocks[i].componentCarrierCellId);
                lte.ComponentCarrier.ConfigureDownlinkTestModelArray(subblockString[i], subblocks[i].downlinkTestModel);
            }

            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, true);
            lte.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
                    measurementOffset, measurementLength);
            lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit);
            lte.Initiate("", "");
        }

        void RetrieveResults()
        {
            subblocksMsr = new SubblockMeasurement[numberOfSubblocks];
            for (int i = 0; i < numberOfSubblocks; i++)
            {
                subblocksMsr[i].qpskConstellation = new ComplexSingle[numberOfComponentCarriers][];
                subblocksMsr[i].meanRmsEvmPerSubcarrier = new AnalogWaveform<float>[numberOfComponentCarriers];
                lte.ModAcc.Results.FetchCompositeEvmArray(subblockString[i], timeout, ref subblocksMsr[i].meanRmsCompositeEvm,
                    ref subblocksMsr[i].maximumPeakCompositeEvm, ref subblocksMsr[i].meanFrequencyError, ref subblocksMsr[i].peakCompositeEvmSymbolIndex,
                    ref subblocksMsr[i].peakCompositeEvmSubcarrierIndex, ref subblocksMsr[i].peakCompositeEvmSlotIndex);
                lte.ModAcc.Results.FetchIQImpairmentsArray(subblockString[i], timeout, ref subblocksMsr[i].meanIQOriginOffset,
                    ref subblocksMsr[i].meanIQGainImbalance, ref subblocksMsr[i].meanIQQuadratureError);
                lte.ModAcc.Results.FetchPdschEvmArray(subblockString[i], timeout, ref subblocksMsr[i].meanRmsEvm, ref subblocksMsr[i].meanRmsQpskEvm,
                    ref subblocksMsr[i].meanRms16QamEvm, ref subblocksMsr[i].meanRms64QamEvm, ref subblocksMsr[i].meanRms256QamEvm);
                lte.ModAcc.Results.FetchPdsch1024QamEvmArray(subblockString[i], timeout, ref subblocksMsr[i].meanRms1024QamEvm);

                for (int j = 0; j < numberOfComponentCarriers; j++)
                {
                    subblockCarrierString = RFmxLteMX.BuildCarrierString(subblockString[i], j);
                    lte.ModAcc.Results.FetchPdschQpskConstellation(subblockCarrierString, timeout, ref subblocksMsr[i].qpskConstellation[j]);
                    lte.ModAcc.Results.FetchEvmPerSubcarrierTrace(subblockCarrierString, timeout, ref subblocksMsr[i].meanRmsEvmPerSubcarrier[j]);
                }

            }

        }

        void PrintResults()
        {
            Console.WriteLine("----------------------Measurements--------------------");
            for (int i = 0; i < numberOfSubblocks; i++)
            {
                Console.WriteLine("\nSubblock Number {0}", i);

                Console.WriteLine("-----------Component Carrier Measurements----------------", i);
                for (int j = 0; j < numberOfComponentCarriers; j++)
                {
                    Console.WriteLine("Carrier {0}", j);
                    Console.WriteLine("Mean Rms Composite Evm  (%  or dB)      : {0}", subblocksMsr[i].meanRmsCompositeEvm[j]);
                    Console.WriteLine("Mean Rms Evm  (%  or dB)                : {0}", subblocksMsr[i].meanRmsEvm[j]);
                    Console.WriteLine("Mean Rms Qpsk Evm  (%  or dB)           : {0}", subblocksMsr[i].meanRmsQpskEvm[j]);
                    Console.WriteLine("Mean Rms 16Qam Evm  (%  or dB)          : {0}", subblocksMsr[i].meanRms16QamEvm[j]);
                    Console.WriteLine("Mean Rms 64Qam Evm  (%  or dB)          : {0}", subblocksMsr[i].meanRms64QamEvm[j]);
                    Console.WriteLine("Mean Rms 256Qam Evm  (%  or dB)         : {0}", subblocksMsr[i].meanRms256QamEvm[j]);
                    Console.WriteLine("Mean Rms 1024Qam Evm  (%  or dB)        : {0}", subblocksMsr[i].meanRms1024QamEvm[j]);
                    Console.WriteLine("Mean Frequency Error  (Hz)              : {0}", subblocksMsr[i].meanFrequencyError[j]);
                    Console.WriteLine("Mean IQ Origin Offset  (dBc)            : {0}", subblocksMsr[i].meanIQOriginOffset[j]);
                    Console.WriteLine("Mean IQ Gain Imbalance  (dB)            : {0}", subblocksMsr[i].meanIQGainImbalance[j]);
                    Console.WriteLine("Mean IQ Quadrature Error  (deg)         : {0}", subblocksMsr[i].meanIQQuadratureError[j]);
                }
            }
        }

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

    }
}