//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select OBW measurement and enable traces.
//6. Configure Sweep Time Parameters.
//7. Configure Averaging Parameters.
//8. Initiate the Measurement.
//9. Fetch OBW Measurements and Traces.
//10. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.EvdoMX;

namespace NationalInstruments.Examples.RFmxEvdoObwSingleCarrier
{
    public class RFmxEvdoObwSingleCarrier
    {
        RFmxInstrMX instrSession;
        RFmxEvdoMX evdo;

        string resourceName = "RFSA";

        RFmxEvdoMXMeasurementTypes measurement = RFmxEvdoMXMeasurementTypes.Obw;

        string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
        double frequencyReferenceFrequency = 10.0e+6;               /* Hz */

        double centerFrequency = 833.49e+6;                           /* Hz */
        double externalAttenuation = 0.0;                           /* dB */
        double referenceLevel = 0.0;                                /* dBm */

        string digitalEdgeSource = RFmxEvdoMXConstants.Pfi0;
        RFmxEvdoMXDigitalEdgeTriggerEdge digitalEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising;
        double triggerDelay = 0.0;                                  /* seconds */

        RFmxEvdoMXObwAveragingEnabled averagingEnabled = RFmxEvdoMXObwAveragingEnabled.False;
        int averagingCount = 10;
        RFmxEvdoMXObwAveragingType averagingType = RFmxEvdoMXObwAveragingType.Rms;

        RFmxEvdoMXObwSweepTimeAuto sweepTimeAuto = RFmxEvdoMXObwSweepTimeAuto.True;
        double sweepTimeInterval = 1.67e-3;                       /* seconds */
        double timeout = 10.00;                                   /* seconds */

        bool enableAllTraces = true;
        bool enableTrigger = false;

        double occupiedBandwidth;
        double startFrequency;
        double stopFrequency;
        double absolutePower;
        Spectrum<float> spectrum;

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

        void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureEvdo()
        {
            /* Get Evdo signal */

            evdo = instrSession.GetEvdoSignalConfiguration();

            /* Configure measurement */

            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            evdo.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            evdo.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
            evdo.SelectMeasurements("", measurement, enableAllTraces);
            evdo.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            evdo.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            evdo.Initiate("", "");
        }

        void RetrieveResults()
        {
            /* Retrieve results */
            evdo.Obw.Results.FetchMeasurement("", timeout, out occupiedBandwidth, out absolutePower,
                out startFrequency, out stopFrequency);

            evdo.Obw.Results.FetchSpectrum("", timeout, ref spectrum);
        }

        void PrintResults()
        {
            Console.WriteLine("Measurement");
            Console.WriteLine("Occupied Bandwidth (Hz)    : {0}", occupiedBandwidth);
            Console.WriteLine("Absolute Power (dBm)       : {0}", absolutePower);
            Console.WriteLine("Start Frequency (Hz)       : {0}", startFrequency);
            Console.WriteLine("Stop Frequency (Hz)        : {0}", stopFrequency);

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

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }
    }
}
