//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Contiguous Carriers
//6. Select CHP measurement and enable Traces.
//7. Configure Sweep Time Parameters.
//8. Configure Averaging Parameters for CHP measurement.
//9. Initiate the Measurement.
//10. Fetch CHP Measurements and Traces.
//11. Close RFmx Session. 
using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WcdmaMX;

namespace NationalInstruments.Examples.RFmxWcdmaChpMultiCarrier
{
    public class RFmxWcdmaChpMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxWcdmaMX wcdma;

        string resourceName = "RFSA";
        RFmxWcdmaMXMeasurementTypes measurement = RFmxWcdmaMXMeasurementTypes.Chp;
        string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
        double frequencyReferenceFrequency = 10.0e+6;                     /* Hz */
        double centerFrequency = 1.95e+9;               /* Hz */
        double externalAttenuation = 0.000000;          /* dB */
        string digitalEdgeSource = RFmxWcdmaMXConstants.Pfi0;
        RFmxWcdmaMXDigitalEdgeTriggerEdge digitalEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising;
        double triggerDelay = 0.000000;                 /* seconds */
        double referenceLevel = 0.000000;               /* dBm */
        RFmxWcdmaMXChpAveragingEnabled averagingEnabled = RFmxWcdmaMXChpAveragingEnabled.False;
        int averagingCount = 10;
        int carrierAtCenterFrequency = -1;
        int numberOfCarriers = 2;
        RFmxWcdmaMXChpAveragingType averagingType = RFmxWcdmaMXChpAveragingType.Rms;
        RFmxWcdmaMXChpSweepTimeAuto sweepTimeAuto = RFmxWcdmaMXChpSweepTimeAuto.True;
        double sweepTimeInterval = 6.67e-4;             /* seconds */
        double timeout = 10.000000;                     /* seconds */
        double[] absolutePower;
        double[] relativePower;
        double totalCarrierPower;
        Spectrum<float> spectrum;

        bool enableAllTraces = true;
        bool enableTrigger = false;

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
            wcdma.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            wcdma.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            wcdma.Initiate("", "");
        }

        private void RetrieveResults()
        {
            wcdma.Chp.Results.FetchCarrierMeasurementArray("", timeout, ref absolutePower, ref relativePower);
            wcdma.Chp.Results.FetchTotalCarrierPower("", timeout, out totalCarrierPower);
            wcdma.Chp.Results.FetchSpectrum("", timeout, ref spectrum);
        }

        private void PrintResults()
        {
            Console.WriteLine("Total Carrier Power  (dBm)  : {0}\n\n", totalCarrierPower);
            Console.WriteLine("Carrier Measurements        :  ");
            for (int i = 0; i < absolutePower.Length; i++)
            {
                Console.WriteLine("Carrier                     : {0} ", i);
                Console.WriteLine("Absolute Power  (dBm)       : {0}", absolutePower[i]);
                Console.WriteLine("Relative Power  (dB)        : {0}", relativePower[i]);
                Console.WriteLine("---------------------------------------------------------");
            }
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
