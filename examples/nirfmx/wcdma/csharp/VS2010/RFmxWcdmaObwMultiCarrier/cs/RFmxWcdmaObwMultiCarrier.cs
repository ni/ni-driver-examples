//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Contiguous Carriers
//6. Select OBW measurement and enable Traces.
//7. Configure Sweep Time Parameters.
//8. Configure Averaging Parameters for OBW measurement.
//9. Initiate the Measurement.
//10. Fetch OBW Measurements and Traces.
//11. Close RFmx Session. 
using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WcdmaMX;

namespace NationalInstruments.Examples.RFmxWcdmaObwMultiCarrier
{
    public class RFmxWcdmaObwMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxWcdmaMX wcdma;

        string resourceName = "RFSA";
        RFmxWcdmaMXMeasurementTypes measurement = RFmxWcdmaMXMeasurementTypes.Obw;
        string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
        double frequencyReferenceFrequency = 10.0e+6;                     /* Hz */
        double centerFrequency = 1.95e+9;               /* Hz */
        double externalAttenuation = 0.000000;          /* dB */

        string digitalEdgeSource = RFmxWcdmaMXConstants.Pfi0;
        RFmxWcdmaMXDigitalEdgeTriggerEdge digitalEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising;
        double triggerDelay = 0;                        /* seconds */
        double referenceLevel = 0.000000;               /* dBm */
        RFmxWcdmaMXObwAveragingEnabled averagingEnabled = RFmxWcdmaMXObwAveragingEnabled.False;
        int averagingCount = 10;
        RFmxWcdmaMXObwAveragingType averagingType = RFmxWcdmaMXObwAveragingType.Rms;
        RFmxWcdmaMXObwSweepTimeAuto sweepTimeAuto = RFmxWcdmaMXObwSweepTimeAuto.True;
        double sweepTimeInterval = 6.6667e-8;           /* seconds */
        double timeout = 10;                            /* seconds */
        Spectrum<float> spectrum;

        bool enableAllTraces = true;
        bool enableTrigger = false;

        int numberOfCarriers = 2;
        int carrierAtCenterFrequency = -1;
        double occupiedBandwidth;
        double absolutePower;
        double startFrequency;
        double stopFrequency;


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
                /* Close session */
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

        private void ConfigureWcdma()
        {
            wcdma = instrSession.GetWcdmaSignalConfiguration();
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);

            wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
            wcdma.ConfigureContiguousCarriers("", numberOfCarriers, carrierAtCenterFrequency);

            wcdma.SelectMeasurements("", measurement, enableAllTraces);
            wcdma.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            wcdma.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            wcdma.Initiate("", "");
        }

        private void RetrieveResults()
        {
            wcdma.Obw.Results.FetchMeasurement("", timeout, out occupiedBandwidth, out absolutePower, out startFrequency, out stopFrequency);
            wcdma.Obw.Results.FetchSpectrum("", timeout, ref spectrum);
        }

        private void PrintResults()
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

        static private void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

    }
}
