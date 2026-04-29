//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure Component Carriers.
//7. Select OBW measurement and enable Traces.
//8. Configure Sweep Time Parameters.
//9. Configure Averaging Parameters for OBW measurement.
//10. Initiate the Measurement.
//11. Fetch OBW Measurements and Traces.
//12. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteObwContiguousMultiCarrier
{
    public class RFmxLteObwContiguousMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxLteMX lte;
        string rfsaResourceName;

        const int NumberOfComponentCarriers = 2;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        bool enableTrigger;
        string digitalEdgeSource;
        RFmxLteMXDigitalEdgeTriggerEdge digitalEdge;
        double triggerDelay;

        double[] componentCarrierBandwidth = {20e6, 20e6};
        double[] componentCarrierFrequency = {-9.9e6, 9.9e6};

        RFmxLteMXLinkDirection linkDirection;

        RFmxLteMXObwSweepTimeAuto sweepTimeAuto;
        double sweepTimeInterval;

        RFmxLteMXObwAveragingEnabled averagingEnabled;
        int averagingCount;

        RFmxLteMXObwAveragingType averagingType;
        double timeout;

        Spectrum<float> spectrum;

        RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;
        int componentCarrierAtCenterFrequency;
        double occupiedBandwidth;
        double absolutePower;
        double startFrequency;
        double stopFrequency;

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

            centerFrequency = 1.95e9;  /* (Hz) */
            referenceLevel = 0.00;     /* (dBm) */
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
            
            componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal;
            componentCarrierAtCenterFrequency = -1;
        }

        void ConfigureLte()
        {
            lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */

            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource,
                                                         digitalEdge, triggerDelay, enableTrigger);
            lte.ComponentCarrier.ConfigureSpacing("", componentCarrierSpacingType, componentCarrierAtCenterFrequency);
            lte.ConfigureNumberOfComponentCarriers("", NumberOfComponentCarriers);
            lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth, componentCarrierFrequency, null);
            lte.ConfigureLinkDirection("", linkDirection);
            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Obw, true);
            lte.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            lte.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
			lte.Initiate("", "");
        }

        void RetrieveResults()
        {
            lte.Obw.Results.FetchMeasurement("", timeout,
                out occupiedBandwidth, out absolutePower, out startFrequency, out stopFrequency);
            lte.Obw.Results.FetchSpectrum("", timeout, ref spectrum);

        }

        void PrintResults()
        {
            Console.WriteLine("Occupied Bandwidth (Hz)  : {0}", occupiedBandwidth);
            Console.WriteLine("Absolute Power (dBm)     : {0}", absolutePower);
            Console.WriteLine("Start Frequency (Hz)     : {0}", startFrequency);
            Console.WriteLine("Stop Frequency (Hz)      : {0}", stopFrequency);
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