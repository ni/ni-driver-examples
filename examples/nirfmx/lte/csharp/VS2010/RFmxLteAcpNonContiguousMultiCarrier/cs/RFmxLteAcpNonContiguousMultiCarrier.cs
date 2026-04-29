//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level, External Attenuation and RF Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Duplex Mode.
//6[A-E]. Configure Subblock Parameters.
//Step 6 :
//6A. Configure Number of Subblocks.
//6B. Configure subblock Frequency.
//6C. Configure Component Carrier Spacing.
//6D. Configure Number of Component Carriers.
//6E. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
//7. Select ACP measurement and enable Traces.
//8. Configure Measurement Method.
//9. Configure Averaging Parameters for ACP measurement.
//10. Configure Sweep Time Parameters.
//11. Configure Noise Compensation Parameter.
//12. Initiate the Measurement.
//13. Fetch ACP Measurements and Traces.
//14. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteAcpNonContiguousMultiCarrier
{
    public class RFmxLteAcpNonContiguousMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxLteMX lte;

        string resourceName, frequencyReferenceSource, digitalEdgeSource, subblockString;

        double frequencyReferenceFrequency, centerFrequency, referenceLevel, externalAttenuation,
               rfAttenuation, triggerDelay, sweepTimeInterval, timeout;
        bool enableTrigger;

        int averagingCount;
        
        const int NumberOfSubblocks = 2;        
        const int NumberOfComponentCarriers = 1;        

        RFmxInstrMXRFAttenuationAuto rfAttenuationAuto;
        RFmxLteMXAcpAveragingEnabled averagingEnabled;
        RFmxLteMXAcpAveragingType averagingType;
        RFmxLteMXDigitalEdgeTriggerEdge digitalEdge;
        RFmxLteMXDuplexScheme duplexScheme;        
        RFmxLteMXAcpMeasurementMethod measurementMethod;
        RFmxLteMXAcpNoiseCompensationEnabled noiseCompensationEnabled;
        RFmxLteMXAcpSweepTimeAuto sweepTimeAuto;
        RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration;
        RFmxLteMXLinkDirection linkDirection;
        /* Subblock inputs structure */
        struct SubblockInput
        { 
	        public double subblockFrequency;    
	        public RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;
            public int componentCarrierAtCenterFrequency;   
            public double[] componentCarrierBandwidth;
            public double[] componentCarrierFrequency;
            public int[] cellID;
        };
       
        /* Subblock measurement outputs structure */
        struct SubblockMeasurement
        {
	        public double subblockPower;
            public double integrationBandwidth;
	        public double frequency;	        
	        public double[] lowerAbsolutePower;
            public double[] upperAbsolutePower;
	        public double[] lowerRelativePower;
            public double[] upperRelativePower;
        };

        SubblockInput[] subblockInput;
        SubblockMeasurement[] subblockMsr;

        double totalAggregatedPower;        
        Spectrum<float> spectrum;
       
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

        private void InitializeVariables()
        {
            resourceName = "RFSA";

            centerFrequency = 1.95e+9;          /* Hz */
            referenceLevel = 0.00;              /* dBm */
            externalAttenuation = 0.00;         /* dB */
       
            rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.True;
            rfAttenuation = 10.0;               /* dB */

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e6;                   /* Hz */

            enableTrigger = false;
            digitalEdgeSource = RFmxLteMXConstants.Pfi0;
            digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.0;                 /* seconds */

            uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;
            duplexScheme = RFmxLteMXDuplexScheme.Fdd;
            linkDirection = RFmxLteMXLinkDirection.Uplink;
                                    
            measurementMethod = RFmxLteMXAcpMeasurementMethod.Normal;

            noiseCompensationEnabled = RFmxLteMXAcpNoiseCompensationEnabled.False;

            sweepTimeAuto = RFmxLteMXAcpSweepTimeAuto.True;
            sweepTimeInterval = 0.001;          /* seconds */

            averagingEnabled = RFmxLteMXAcpAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxLteMXAcpAveragingType.Rms;

            timeout = 10.0;                    /* seconds */
            
            subblockMsr = new SubblockMeasurement[NumberOfSubblocks]; 
            subblockInput = new SubblockInput[NumberOfSubblocks] {
                                                                    new SubblockInput {  
                                                                                            subblockFrequency = 0.0,
                                                                                            componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal, 
                                                                                            componentCarrierAtCenterFrequency = -1, 
                                                                                            componentCarrierBandwidth = new double[NumberOfComponentCarriers] {20e6}, 
                                                                                            componentCarrierFrequency = new double[NumberOfComponentCarriers] {0.0}, 
                                                                                            cellID = null
                                                                                      },
                                                                    new SubblockInput {
                                                                                            subblockFrequency = 30e6, 
                                                                                            componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal, 
                                                                                            componentCarrierAtCenterFrequency = -1, 
                                                                                            componentCarrierBandwidth = new double[NumberOfComponentCarriers] {20e6}, 
                                                                                            componentCarrierFrequency =  new double[NumberOfComponentCarriers] {0.0}, 
                                                                                            cellID = null
                                                                                      }
                                                                };           
        }
    
        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void ConfigureLte()
        {
            /* Get Lte signal */
            lte = instrSession.GetLteSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

            lte.ConfigureFrequency("", centerFrequency);

            lte.ConfigureReferenceLevel("", referenceLevel);

            lte.ConfigureExternalAttenuation("", externalAttenuation);

            instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation);
                  
            lte.ConfigureDigitalEdgeTrigger( "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);

            lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration);           
                        
            lte.ConfigureNumberOfSubblocks("", NumberOfSubblocks);

            for(int i=0; i < NumberOfSubblocks; i++)
            {
                subblockString = RFmxLteMX.BuildSubblockString("", i);

                lte.SetSubblockFrequency(subblockString, subblockInput[i].subblockFrequency);
                lte.ComponentCarrier.ConfigureSpacing(subblockString, subblockInput[i].componentCarrierSpacingType,
                                                      subblockInput[i].componentCarrierAtCenterFrequency);
                
                lte.ConfigureNumberOfComponentCarriers(subblockString, NumberOfComponentCarriers);

                lte.ComponentCarrier.ConfigureArray(subblockString, subblockInput[i].componentCarrierBandwidth,
                                                    subblockInput[i].componentCarrierFrequency,
                                                    subblockInput[i].cellID);

            }
            lte.ConfigureLinkDirection("", linkDirection);

            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Acp, true);

            lte.Acp.Configuration.ConfigureMeasurementMethod( "", measurementMethod);
            
            lte.Acp.Configuration.ConfigureAveraging( "", averagingEnabled, averagingCount, averagingType);
            
            lte.Acp.Configuration.ConfigureSweepTime( "", sweepTimeAuto, sweepTimeInterval);
            
            lte.Acp.Configuration.ConfigureNoiseCompensationEnabled( "", noiseCompensationEnabled);
            
            lte.Initiate( "",  "");

          }

        private void RetrieveResults()
        {
            /* Retrieve results */            
           
            for(int i=0; i< NumberOfSubblocks; i++ )
            {
                subblockString = RFmxLteMX.BuildSubblockString("", i);
                
                lte.Acp.Results.FetchSubblockMeasurement(subblockString, timeout, out  subblockMsr[i].subblockPower,
                                                         out  subblockMsr[i].integrationBandwidth,
                                                         out  subblockMsr[i].frequency);
                               
                lte.Acp.Results.FetchOffsetMeasurementArray(subblockString, timeout,
                                                            ref subblockMsr[i].lowerRelativePower,
                                                            ref subblockMsr[i].upperRelativePower,
                                                            ref subblockMsr[i].lowerAbsolutePower,
                                                            ref subblockMsr[i].upperAbsolutePower);              
            }
            
            lte.Acp.Results.FetchTotalAggregatedPower( "", timeout, out totalAggregatedPower);
            
            lte.Acp.Results.FetchSpectrum("", timeout, ref spectrum);
       }
    
        private void PrintResults()
        {           
            Console.WriteLine("\nTotal Aggregated Power (dBm)  : {0}", totalAggregatedPower);
            Console.WriteLine("\n****************Subblock Measurements****************");

            for (int i = 0; i < NumberOfSubblocks; i++)
            {
                Console.WriteLine("\n******************************************************");
                Console.WriteLine("\nSubblock  :  {0}\n", i);
                Console.WriteLine("Subblock Power (dBm)       : {0}", subblockMsr[i].subblockPower);
                Console.WriteLine("Integration Bandwidth (Hz) : {0}", subblockMsr[i].integrationBandwidth);
                Console.WriteLine("Frequency (Hz)             : {0}", subblockMsr[i].frequency);

                Console.WriteLine("\n-------Offset Channel Measurements------");
                for (int j = 0; j < subblockMsr[i].lowerRelativePower.Length; j++)
                {
                    Console.WriteLine("\nOffset  : {0}", j);
                    Console.WriteLine("Lower Relative Power (dB)  : {0}", subblockMsr[i].lowerRelativePower[j]);
                    Console.WriteLine("Upper Relative Power (dB)  : {0}", subblockMsr[i].upperRelativePower[j]);
                    Console.WriteLine("Lower Absolute Power (dBm) : {0}", subblockMsr[i].lowerAbsolutePower[j]);
                    Console.WriteLine("Upper Absolute Power (dBm) : {0}", subblockMsr[i].upperAbsolutePower[j]);
                }
                Console.WriteLine("------------------------------------------\n");
            }        
    }

    private void CloseSession()
    {
        try
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
