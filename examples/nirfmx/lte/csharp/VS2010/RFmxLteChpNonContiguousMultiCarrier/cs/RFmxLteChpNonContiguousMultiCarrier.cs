//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5[A - F].Configure Subblock Configurations.
//5A.Configure Number of Subblocks.
//5B.Configure Subblock Frequency.
//5D.Configure Component Carrier Spacing.
//5E.Configure Number of Component Carriers.
//5F.Configure Component Carriers(Component Carrier Frequency and Component Carrier Bandwidth).
//6. Select CHP measurement and enable Traces.
//7. Configure Sweep Time Parameters.
//8. Configure Averaging Parameters for CHP measurement.
//9. Initiate the Measurement.
//10. Fetch CHP Measurements and Traces.
//11. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteChpNonContiguousMultiCarrier
{
    /* Input: Subblock inputs structure */
    struct SubblockInput
    {
	    public double subblockFrequency;/*(Hz) */
        public RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;
        public int componentCarrierAtCenterFrequency;
        public double[] componentCarrierBandwidth;/*(Hz) */
        public double[] componentCarrierFrequency;/*(Hz) */
    }

    /* Input: Subblock measurement outputs structure */
    struct SubblockMeasurement
    {
        public double subblockPower;				/*(dBm) */
        public double integrationBandwidth;		/*(Hz) */
        public double frequency;					/*(Hz) */
        public int numberOfOffsets;
        public double[] absolutePower;// = NULL;	/*(dBm) */
        public double[] relativePower;// = NULL;	/*(dB) */
    }

    public class RFmxLteChpNonContiguousMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxLteMX lte;
        string rfsaResourceName;

        const int NumberOfComponentCarriers = 1;
        const int NumberOfSubblocks = 2;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        bool enableTrigger;
        string digitalEdgeSource;
        RFmxLteMXDigitalEdgeTriggerEdge digitalEdge;
        double triggerDelay;

        RFmxLteMXChpSweepTimeAuto sweepTimeAuto;
        double sweepTimeInterval;

        RFmxLteMXChpAveragingEnabled averagingEnabled;
        int averagingCount;

        RFmxLteMXChpAveragingType averagingType;
        double timeout;

        Spectrum<float> spectrum;

        string subblockString;

        SubblockInput[] subblock;
        SubblockMeasurement[] subblockMeasurement;        

        double totalAggregatedPower;

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
            frequencyReferenceFrequency = 10e6; /* (Hz) */

            centerFrequency = 1.95e9; /* (Hz) */
            referenceLevel = 0.00; /* (dBm) */
            externalAttenuation = 0.0; /* (dBm) */

            enableTrigger = false;
            digitalEdgeSource = RFmxLteMXConstants.Pfi0;
            digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.0; /* (s) */

            sweepTimeAuto = RFmxLteMXChpSweepTimeAuto.True;
            sweepTimeInterval = 0.001; /* (s) */

            averagingEnabled = RFmxLteMXChpAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxLteMXChpAveragingType.Rms;

            timeout = 10.0; /* (s) */

            subblockMeasurement = new SubblockMeasurement[NumberOfSubblocks];        
            subblock = new SubblockInput[NumberOfSubblocks] {
                                              new SubblockInput {  
                                                                    subblockFrequency = 0.0,
                                                                    componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal, 
                                                                    componentCarrierAtCenterFrequency = -1, 
                                                                    componentCarrierBandwidth = new double[NumberOfComponentCarriers] {20e6}, 
                                                                    componentCarrierFrequency = new double[NumberOfComponentCarriers] {0.0}
                                                                },
                                              new SubblockInput {
                                                                    subblockFrequency = 30e6, 
                                                                    componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal, 
                                                                    componentCarrierAtCenterFrequency = -1, 
                                                                    componentCarrierBandwidth = new double[NumberOfComponentCarriers] {20e6}, 
                                                                    componentCarrierFrequency =  new double[NumberOfComponentCarriers] {0.0}
                                                                }
                                                            };          
        }

        void ConfigureLte()
        {
            lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

            lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource,
                                                         digitalEdge, triggerDelay, enableTrigger);
            lte.ConfigureNumberOfSubblocks("", NumberOfSubblocks);
            for (int i = 0; i < NumberOfSubblocks; i++)
            {
                subblockString = RFmxLteMX.BuildSubblockString("", i);
                lte.SetSubblockFrequency(subblockString, subblock[i].subblockFrequency);
                lte.ComponentCarrier.ConfigureSpacing(subblockString, subblock[i].componentCarrierSpacingType, 
                    subblock[i].componentCarrierAtCenterFrequency);
                lte.ConfigureNumberOfComponentCarriers(subblockString, NumberOfComponentCarriers);
                lte.ComponentCarrier.ConfigureArray(subblockString, subblock[i].componentCarrierBandwidth, 
                    subblock[i].componentCarrierFrequency, null);
            }

            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Chp, true);
            lte.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            lte.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            lte.Initiate("", "");
        }

        void RetrieveResults()
        {     
            subblockMeasurement = new SubblockMeasurement[NumberOfSubblocks];
            for (int i = 0; i < NumberOfSubblocks; i++)
            {
                subblockString = RFmxLteMX.BuildSubblockString("", i);

                lte.Chp.Results.FetchSubblockMeasurement(subblockString, timeout,
                    out subblockMeasurement[i].subblockPower, out subblockMeasurement[i].integrationBandwidth, 
                    out subblockMeasurement[i].frequency);

                lte.Chp.Results.ComponentCarrier.FetchMeasurementArray(subblockString, timeout,
                    ref subblockMeasurement[i].absolutePower, ref subblockMeasurement[i].relativePower);
                subblockMeasurement[i].numberOfOffsets = subblockMeasurement[i].absolutePower.Length;
            }

            lte.Chp.Results.FetchTotalAggregatedPower("", timeout, out totalAggregatedPower);

            lte.Chp.Results.FetchSpectrum("", timeout, ref spectrum);

        }

        void PrintResults()
        {
            Console.WriteLine("Total Aggregated Power (dBm)                : {0}", totalAggregatedPower);

            Console.WriteLine("---------------Subblock Measurements---------------");
            for (int i = 0; i < NumberOfSubblocks; i++)
            {                
                Console.WriteLine("Subblock  : {0}", i);
                Console.WriteLine("Subblock Power (dBm)                        : {0}", subblockMeasurement[i].subblockPower);
                Console.WriteLine("Integration Bandwidth (Hz)                  : {0}", subblockMeasurement[i].integrationBandwidth);
                Console.WriteLine("Frequency (Hz)                              : {0}", subblockMeasurement[i].frequency);
                Console.WriteLine("------Component Carrier Measurements------");
                for (int j = 0; j < subblockMeasurement[i].numberOfOffsets; j++)
                {
                    Console.WriteLine("Carrier : {0}", j);
                    Console.WriteLine("Absolute Power (dBm)                        : {0}", subblockMeasurement[i].absolutePower[j]);
                    Console.WriteLine("Relative Power (dB)                         : {0}", subblockMeasurement[i].relativePower[j]);
                }
            }
        }

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

    }
}