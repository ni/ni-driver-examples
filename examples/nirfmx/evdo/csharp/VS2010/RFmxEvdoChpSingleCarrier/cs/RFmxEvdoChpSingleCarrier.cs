//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select CHP measurement and enable traces.
//6. Configure Sweep Time Parameters.
//7. Configure Averaging Parameters.
//8. Initiate the Measurement.
//9. Fetch CHP Measurements and Traces.
//10. Close the RFmx Session.


using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.EvdoMX;

namespace NationalInstruments.Examples.RFmxEvdoChpSingleCarrier
{
    public class RFmxEvdoChpSingleCarrier
    {
        RFmxInstrMX instrSession;
        RFmxEvdoMX evdo;

        string resourceName = "RFSA";

        RFmxEvdoMXMeasurementTypes measurement = RFmxEvdoMXMeasurementTypes.Chp;

        string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
        double frequency = 10.0e+6;                     /* Hz */

        double centerFrequency = 833.49e+6;               /* Hz */
        double externalAttenuation = 0.00;          /* dB */
        string digitalEdgeSource = RFmxEvdoMXConstants.Pfi0;

        RFmxEvdoMXDigitalEdgeTriggerEdge digitalEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising;
        double triggerDelay = 0.00;
        double referenceLevel = 0.00;

        RFmxEvdoMXChpAveragingEnabled averagingEnabled = RFmxEvdoMXChpAveragingEnabled.False;
        int averagingCount = 10;
        RFmxEvdoMXChpAveragingType averagingType = RFmxEvdoMXChpAveragingType.Rms;

        RFmxEvdoMXChpSweepTimeAuto sweepTimeAuto = RFmxEvdoMXChpSweepTimeAuto.True;
        double sweepTimeInterval = 1.67e-3;             /* seconds */

        double timeout = 10.00;                     /* seconds */
        double carrierAbsolutePower;                /* dBm */
        double carrierRelativePower;                /* dB */
        Spectrum<float> spectrum;

        bool enableAllTraces = true;
        bool enableTrigger = false;

        public void Run()
        {
            try
            {
                InitializeInstr();
                ConfigureEvdo();
                RetrieveResults();
                PrintResults();
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
            finally
            {
                CloseSession();
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void ConfigureEvdo()
        {
            /* Get Evdo signal */

            evdo = instrSession.GetEvdoSignalConfiguration();

            /* Configure measurement */

            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequency);
            evdo.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            evdo.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
            evdo.SelectMeasurements("", measurement, enableAllTraces);
            evdo.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            evdo.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            evdo.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */

            evdo.Chp.Results.FetchCarrierMeasurement("", timeout, out carrierAbsolutePower,
                out carrierRelativePower);
            evdo.Chp.Results.FetchSpectrum("", timeout, ref spectrum);
        }

        private void PrintResults()
        {
            Console.WriteLine("Carrier Absolute Power  (dBm) : {0}", carrierAbsolutePower);
        }

        void CloseSession()
        {
            if (evdo != null)
            {
                evdo.Dispose();
                evdo = null;
            }
            if (instrSession != null)
            {
                instrSession.Close();
                instrSession = null;
            }
        }

        static private void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

    }
}
