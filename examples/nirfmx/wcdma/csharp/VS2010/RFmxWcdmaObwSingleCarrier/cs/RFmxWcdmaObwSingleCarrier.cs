//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select OBW measurement and enable Traces.
//6. Configure Sweep Time Parameters.
//7. Configure Averaging Parameters for OBW measurement.
//8. Initiate the Measurement.
//9. Fetch OBW Measurements and Traces.
//10. Close RFmx Session. 
using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WcdmaMX;

namespace NationalInstruments.Examples.RFmxWcdmaObwSingleCarrier
{
    public class RFmxWcdmaObwSingleCarrier
    {
        RFmxInstrMX instrSession;
        RFmxWcdmaMX wcdma;

        string resourceName = "RFSA";

        RFmxWcdmaMXMeasurementTypes measurement = RFmxWcdmaMXMeasurementTypes.Obw;

        string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
        double frequencyReferenceFrequency = 10.0e+6;               /* Hz */

        double centerFrequency = 1.95e+9;                           /* Hz */
        double externalAttenuation = 0.0;                           /* dB */
        double referenceLevel = 0.0;                                /* dBm */

        string digitalEdgeSource = RFmxWcdmaMXConstants.Pfi0;
        RFmxWcdmaMXDigitalEdgeTriggerEdge digitalEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising;
        double triggerDelay = 0.0;                                  /* seconds */

        RFmxWcdmaMXObwAveragingEnabled averagingEnabled = RFmxWcdmaMXObwAveragingEnabled.False;
        int averagingCount = 10;
        RFmxWcdmaMXObwAveragingType averagingType = RFmxWcdmaMXObwAveragingType.Rms;

        RFmxWcdmaMXObwSweepTimeAuto sweepTimeAuto = RFmxWcdmaMXObwSweepTimeAuto.True;
        double sweepTimeInterval = 6.6667e-8;                       /* seconds */
        double timeout = 10;                                        /* seconds */

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
                ConfigureWcdma();
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

        void ConfigureWcdma()
        {
            wcdma = instrSession.GetWcdmaSignalConfiguration();
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
            wcdma.SelectMeasurements("", measurement, enableAllTraces);
            wcdma.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            wcdma.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
        }

        void RetrieveResults()
        {
            wcdma.Initiate("", "");
            wcdma.Obw.Results.FetchMeasurement("", timeout, out occupiedBandwidth, out absolutePower, 
                out startFrequency, out stopFrequency);

            wcdma.Obw.Results.FetchSpectrum("", timeout, ref spectrum);
        }

        void PrintResults()
        {
            Console.WriteLine("Measurement\n");
            Console.WriteLine("Occupied Bandwidth (Hz)   : {0}", occupiedBandwidth);
            Console.WriteLine("Absolute Power (dBm)       : {0}", absolutePower);
            Console.WriteLine("Start Frequency (Hz)       : {0}", startFrequency);
            Console.WriteLine("Stop Frequency (Hz)        : {0}", stopFrequency);
        }

        void CloseSession()
        {
            if (wcdma != null)
            {
                wcdma.Dispose();
                wcdma = null;
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
