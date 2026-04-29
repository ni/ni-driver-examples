//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Select OBW measurement and enable Traces.
//7. Configure Sweep Time Parameters.
//8. Configure Averaging Parameters for OBW measurement.
//9. Initiate the Measurement.
//10. Fetch OBW Measurements and Traces.
//11. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteObwSingleCarrier
{
    public class RFmxLteObwSingleCarrier
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

        RFmxLteMXLinkDirection linkDirection;
        double componentCarrierBandwidth;
        double componentCarrierFrequency;
        int cellID;

        RFmxLteMXObwSweepTimeAuto sweepTimeAuto;
        double sweepTimeInterval;

        RFmxLteMXObwAveragingEnabled averagingEnabled;
        int averagingCount;

        RFmxLteMXObwAveragingType averagingType;
        double timeout;

        double stopFrequency;
        double startFrequency;
        double occupiedBandwidth;
        double absolutePower;

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

            centerFrequency = 1.95e9; /* (Hz) */
            referenceLevel = 0.00; /* (dBm) */
            externalAttenuation = 0.0; /* (dBm) */

            enableTrigger = false;
            digitalEdgeSource = RFmxLteMXConstants.Pfi0;
            digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.0; /* (s) */

            linkDirection = RFmxLteMXLinkDirection.Uplink;

            componentCarrierBandwidth = 200e3; /* (Hz) */
            componentCarrierFrequency = 0.0; /* (Hz) */
            cellID = 0;

            sweepTimeAuto = RFmxLteMXObwSweepTimeAuto.True;
            sweepTimeInterval = 0.001; /* (s) */

            averagingEnabled = RFmxLteMXObwAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxLteMXObwAveragingType.Rms;

            timeout = 10.0; /* (s) */
        }

        void ConfigureLte()
        {
            lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
            lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID);
            lte.ConfigureLinkDirection("", linkDirection);
            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Obw, true);
            lte.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            lte.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            lte.Initiate("", "");
        }

        void RetrieveResults()
        {
            lte.Obw.Results.FetchMeasurement("", timeout, out occupiedBandwidth, out absolutePower,
                                             out startFrequency, out stopFrequency);
            lte.Obw.Results.FetchSpectrum("", timeout, ref spectrum);
        }

        void PrintResults()
        {
            Console.WriteLine("Occupied Bandwidth (Hz)      :{0}", occupiedBandwidth);

            Console.WriteLine("Absolute Power (dBm)         :{0}", absolutePower);

            Console.WriteLine("Start Frequency (Hz)         :{0}", startFrequency);

            Console.WriteLine("Stop Frequency (Hz)          :{0}", stopFrequency);


        }

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

    }
}