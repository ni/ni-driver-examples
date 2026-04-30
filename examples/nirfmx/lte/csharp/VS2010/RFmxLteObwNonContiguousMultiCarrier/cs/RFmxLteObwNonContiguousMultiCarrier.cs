//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5[A-E]. Configure Subblock Configurations.
//Step 5 :
//5A. Configure Number of Subblocks.
//5B. Configure subblock Frequency.
//5C. Configure Component Carrier Spacing.
//5D. Configure Number of Component Carriers.
//5E. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
//6. Select OBW measurement and enable Traces.
//7. Configure Sweep Time Parameters.
//8. Configure Averaging Parameters for OBW measurement.
//9. Initiate the Measurement.
//10. Fetch OBW Measurements and Traces.
//11. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteObwNonContiguousMultiCarrier
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
        public double startFrequency, stopFrequency, occupiedBandwidth, absolutePower;
    }

    public class RFmxLteObwNonContiguousMultiCarrier
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
        RFmxLteMXLinkDirection linkDirection;
        
        RFmxLteMXObwSweepTimeAuto sweepTimeAuto;
        double sweepTimeInterval;

        RFmxLteMXObwAveragingEnabled averagingEnabled;
        int averagingCount;

        RFmxLteMXObwAveragingType averagingType;
        double timeout;

        Spectrum<float> spectrum;

        string subblockString;

        SubblockInput[] subblocks;
        SubblockMeasurement[] subblockMsr;        

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

            linkDirection = RFmxLteMXLinkDirection.Uplink;
            sweepTimeAuto = RFmxLteMXObwSweepTimeAuto.True;
            sweepTimeInterval = 0.001; /* (s) */

            averagingEnabled = RFmxLteMXObwAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxLteMXObwAveragingType.Rms;

            timeout = 10.0; /* (s) */
                        
            subblockMsr = new SubblockMeasurement[NumberOfSubblocks];

            subblocks = new SubblockInput[NumberOfSubblocks] {
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
                lte.SetSubblockFrequency(subblockString, subblocks[i].subblockFrequency);
                lte.ComponentCarrier.ConfigureSpacing(subblockString, subblocks[i].componentCarrierSpacingType,
                                                      subblocks[i].componentCarrierAtCenterFrequency);
                lte.ConfigureNumberOfComponentCarriers(subblockString, NumberOfComponentCarriers);
                lte.ComponentCarrier.ConfigureArray(subblockString, subblocks[i].componentCarrierBandwidth,
                                                    subblocks[i].componentCarrierFrequency, null);
            }

            lte.ConfigureLinkDirection("", linkDirection);
            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Obw, true);
            lte.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            lte.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
			lte.Initiate("", "");
        }

        void RetrieveResults()
        {            
            for (int i = 0; i < NumberOfSubblocks; i++)
            {
                subblockString = RFmxLteMX.BuildSubblockString("", i);
                lte.Obw.Results.FetchMeasurement(subblockString, timeout,
                    out subblockMsr[i].occupiedBandwidth, out subblockMsr[i].absolutePower,
                    out subblockMsr[i].startFrequency, out subblockMsr[i].stopFrequency);
            }
            lte.Obw.Results.FetchSpectrum("", timeout, ref spectrum);

        }

        void PrintResults()
        {
            Console.WriteLine("Subblock Measurements            : ");
            for (var i = 0;i< subblockMsr.Length;i++)
            {
                Console.WriteLine("\n*****************************************");
                Console.WriteLine("\nSubblock  : {0}", i);
                Console.WriteLine("Occupied Bandwidth (Hz)          : {0}", subblockMsr[i].occupiedBandwidth);
                Console.WriteLine("Absolute Power (dBm)             : {0}", subblockMsr[i].absolutePower);               
                Console.WriteLine("Start Frequency (Hz)             : {0}", subblockMsr[i].startFrequency);
                Console.WriteLine("Stop Frequency (Hz)              : {0}", subblockMsr[i].stopFrequency);
            }
        }

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

    }
}