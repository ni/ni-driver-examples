//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Select CHP measurement and enable Traces.
//7. Configure Sweep Time Parameters.
//8. Configure Averaging Parameters for CHP measurement.
//9. Initiate the Measurement.
//10. Fetch CHP Measurements and Traces.
//11. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteChpSingleCarrier
{
    public class RFmxLteChpSingleCarrier
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
        string digitalEdgeSource;
        RFmxLteMXDigitalEdgeTriggerEdge digitalEdge;
        double triggerDelay;

        double componentCarrierBandwidth;
        double componentCarrierFrequency;
        int cellID;

        RFmxLteMXChpSweepTimeAuto sweepTimeAuto;
        double sweepTimeInterval;

        RFmxLteMXChpAveragingEnabled averagingEnabled;
        int averagingCount;

        RFmxLteMXChpAveragingType averagingType;
        double timeout;

        double absolutePower;
        double relativePower;

        Spectrum<float> spectrum;

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

            centerFrequency = 1.95e9;  /* (Hz) */
            referenceLevel = 0.00;     /* (dBm) */
            externalAttenuation = 0.0; /* (dBm) */

            enableTrigger = false;
            digitalEdgeSource = RFmxLteMXConstants.Pfi0;
            digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.0; /* (s) */

            componentCarrierBandwidth = 200e3; /* (Hz) */
            componentCarrierFrequency = 0.0; /* (Hz) */
            cellID = 0;
            
            sweepTimeAuto = RFmxLteMXChpSweepTimeAuto.True;
            sweepTimeInterval = 0.001; /* (s) */

            averagingEnabled = RFmxLteMXChpAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxLteMXChpAveragingType.Rms;

            timeout = 10.0; /* (s) */

            
        }

        void ConfigureLte()
        {            
            lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, 
                                                    frequencyReferenceFrequency);
            lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, 
                                                enableTrigger);
            lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID);
            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Chp, true);
            lte.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            lte.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            lte.Initiate("", "");
        }

        void RetrieveResults()
        {
            lte.Chp.Results.ComponentCarrier.FetchMeasurement("", timeout, 
                out absolutePower, out relativePower);
            lte.Chp.Results.FetchSpectrum("", timeout, ref spectrum);

        }

        void PrintResults()
        {
            Console.WriteLine("Carrier Absolute Power (dBm)         :{0}", absolutePower);
        }

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

    }
}