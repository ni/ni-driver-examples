//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties  (Center Frequency and External Attenuation).
//4. Configure RF Attenuation.
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Contiguous Carriers.
//7. Confiure Reference Level. 
//8. Select ACP measurement and enable traces.
//9. Configure Measurement Method Parameter.
//10. Configure Averaging Parameters.
//11. Configure Sweep Time Parameters.
//12. Configure Noise Compensation Parameter.
//13. Configure Number of Offsets.
//14. Configure Offset Power Reference Parameters.
//15. Initiate the Measurement.
//16. Fetch ACP Measurements and Traces.
//17. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.EvdoMX;

namespace NationalInstruments.Examples.RFmxEvdoAcpMultiCarrier
{
    public class RFmxEvdoAcpMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxEvdoMX evdo;

        string resourceName = "RFSA";

        RFmxEvdoMXMeasurementTypes measurement = RFmxEvdoMXMeasurementTypes.Acp;

        string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
        double frequency = 10.0e+6;                     /* Hz */

        double centerFrequency = 833.49e+6;             /* Hz */
        double externalAttenuation = 0.00;              /* dB */

        RFmxInstrMXRFAttenuationAuto rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.True;
        double rfAttenuation = 10.0;

        string digitalEdgeSource = RFmxEvdoMXConstants.Pfi0;
        RFmxEvdoMXDigitalEdgeTriggerEdge digitalEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising;
        double triggerDelay = 0.00;                 /* seconds */

        double measurementInterval = 0.026670;      /* seconds */
        double referenceLevel = 0.00;               /* dBm */

        RFmxEvdoMXAcpMeasurementMethod measurementMethod = RFmxEvdoMXAcpMeasurementMethod.Normal;

        RFmxEvdoMXAcpAveragingEnabled averagingEnabled = RFmxEvdoMXAcpAveragingEnabled.False;
        int averagingCount = 10;
        RFmxEvdoMXAcpAveragingType averagingType = RFmxEvdoMXAcpAveragingType.Rms;

        bool autoLevel = true;

        RFmxEvdoMXAcpSweepTimeAuto sweepTimeAuto = RFmxEvdoMXAcpSweepTimeAuto.True;
        double sweepTimeInterval = 0.001670;       /* seconds */

        RFmxEvdoMXAcpNoiseCompensationEnabled noiseCompensationEnabled = RFmxEvdoMXAcpNoiseCompensationEnabled.False;

        int numberOfOffsets = 2;

        bool enableAllTraces = true;
        bool enableTrigger = false;

        int numberOfCarriers = 3;
        int frequencyReferenceCarrier = -1;
        int bandClass = 0;

        RFmxEvdoMXAcpOffsetPowerReferenceCarrier offsetPowerReferenceCarrier = RFmxEvdoMXAcpOffsetPowerReferenceCarrier.Composite;
        int offsetPowerReferenceSpecific = 0;

        double timeout = 10.00;                     /* seconds */
        Spectrum<float> spectrum;
        double totalCarrierPower = 0;

        double[] carrierAbsolutePower, carrierRelativePower;
        double[] lowerAbsolutePower, upperAbsolutePower, lowerRelativePower, upperRelativePower;

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
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequency);
            evdo.ConfigureFrequency("", centerFrequency);
            evdo.ConfigureExternalAttenuation("", externalAttenuation);
            instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation);

            evdo.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
            evdo.ConfigureContiguousCarriers("", numberOfCarriers, frequencyReferenceCarrier, bandClass);

            if (autoLevel)
            {
                evdo.AutoLevel("", measurementInterval, out referenceLevel);
                Console.WriteLine("Reference Level (dBm)          : {0}", referenceLevel);
            }
            else
            {
                evdo.ConfigureReferenceLevel("", referenceLevel);
            }

            evdo.SelectMeasurements("", measurement, enableAllTraces);
            evdo.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod);
            evdo.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            evdo.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            evdo.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled);
            evdo.Acp.Configuration.ConfigureNumberOfOffsets("", numberOfOffsets);
            evdo.Acp.Configuration.ConfigureOffsetPowerReference("", offsetPowerReferenceCarrier, offsetPowerReferenceSpecific);
            evdo.Initiate("", "");
        }

        void RetrieveResults()
        {
            /* Retrieve results */
            evdo.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref lowerRelativePower, ref upperRelativePower, ref lowerAbsolutePower,
                ref upperAbsolutePower);
            evdo.Acp.Results.FetchTotalCarrierPower("", timeout, out totalCarrierPower);
            evdo.Acp.Results.FetchCarrierMeasurementArray("", timeout, ref carrierAbsolutePower,
                ref carrierRelativePower);
            evdo.Acp.Results.FetchSpectrum("", timeout, ref spectrum);
        }

        void PrintResults()
        {
            Console.WriteLine("Total Carrier Power (dBm)      : {0}", totalCarrierPower);

            Console.WriteLine("\nCarrier Measurements:");
            for (int i = 0; i < numberOfCarriers; i++)
            {
                Console.WriteLine("\nCarrier: {0}", i);
                Console.WriteLine("Absolute Power (dBm)           : {0}", carrierAbsolutePower[i]);
                Console.WriteLine("Relative Power (dB)            : {0}", carrierRelativePower[i]);
            }

            Console.WriteLine("\nOffset Channel Measurements:");
            for (int i = 0; i < lowerRelativePower.Length; i++)
            {
                Console.WriteLine("\nOffset: {0}", i);
                Console.WriteLine("Lower Relative Power (dB)      : {0}", lowerRelativePower[i]);
                Console.WriteLine("Upper Relative Power (dB)      : {0}", upperRelativePower[i]);
                Console.WriteLine("Lower Absolute Power (dBm)     : {0}", lowerAbsolutePower[i]);
                Console.WriteLine("Upper Absolute Power (dBm)     : {0}", upperAbsolutePower[i]);
            }
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
