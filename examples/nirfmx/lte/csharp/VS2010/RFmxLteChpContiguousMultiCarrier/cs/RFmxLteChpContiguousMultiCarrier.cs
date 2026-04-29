//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure Component Carriers.
//7. Select CHP measurement and enable Traces.
//8. Configure Sweep Time Parameters.
//9. Configure Averaging Parameters for CHP measurement.
//10. Initiate the Measurement.
//11. Fetch CHP Measurements and Traces.
//16. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteChpContiguousMultiCarrier
{
    public class RFmxLteChpContiguousMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxLteMX lte;
        string rfsaResourceName;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;
        int componentCarrierAtCenterFrequency;

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

        double totalAggregatedPower;
        double[] absolutePower;
        double[] relativePower;

        const int numberOfComponentCarriers = 2;
        int[] cellID;

        double[] componentCarrierFrequency = {-9.9e6, 9.9e6}, componentCarrierBandwidth = {20e6, 20e6};
        RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;

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
            frequencyReferenceFrequency = 10e6; /* (Hz) */
            componentCarrierAtCenterFrequency = -1;

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

           
            cellID = null;
    
            for (int i = 0; i < numberOfComponentCarriers; i++)
            {
                componentCarrierBandwidth[i] = 20e6;

                if (i == 0) /* For offset 0, setfreq = -9.9 MHz */
                    componentCarrierFrequency[i] = -9.9e6;

                else if (i == 1) /* For offset 1, setfreq = 9.9 MHz */
                    componentCarrierFrequency[i] = 9.9e6;
            }

            componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal;
        }

        void ConfigureLte()
        {
           lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
           instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            
           lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
           lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource,
                                                        digitalEdge, triggerDelay, enableTrigger);
                       
           lte.ComponentCarrier.ConfigureSpacing("", componentCarrierSpacingType,
                                                 componentCarrierAtCenterFrequency);

           lte.ConfigureNumberOfComponentCarriers("", numberOfComponentCarriers);

           lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth,
                                               componentCarrierFrequency, cellID);
           lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Chp, true);
           lte.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
           lte.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
           lte.Initiate("", "");

        }

        void RetrieveResults()
        {
            lte.Chp.Results.ComponentCarrier.FetchMeasurementArray("", timeout, ref absolutePower, ref relativePower);
            lte.Chp.Results.FetchTotalAggregatedPower("", timeout, out totalAggregatedPower);
            lte.Chp.Results.FetchSpectrum("", timeout, ref spectrum);

        }

        void PrintResults()
        {
             Console.WriteLine("\nTotal Aggregated Power  (dBm)  : {0}", totalAggregatedPower);
  
             Console.WriteLine("\nComponent Carrier  Measurements:");
             for (int i = 0; i < absolutePower.Length; i++)
             {
                 Console.WriteLine("\nCarrier:  {0}", i);
                 Console.WriteLine("Absolute Power (dBm)            : {0}", absolutePower[i]);
                 Console.WriteLine("Relative Power (dB)             : {0}", relativePower[i]);
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