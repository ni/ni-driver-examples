//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, RF Attenuation and External Attenuation)
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Contiguous Carriers.
//6. Configure Reference Level.
//7. Select ACP measurement and enable Traces.
//8. Configure Measurement Method.
//9. Configure Averaging Parameters for ACP measurement.
//10. Configure Sweep Time Parameters.
//11. Configure Noise Compensation Parameter.
//12. Configure Number of offset channels. 
//13. Configure Offset Power Reference
//14. Initiate the Measurement.
//15. Fetch ACP Measurements and Traces.
//16. Close RFmx Session.
using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WcdmaMX;

namespace NationalInstruments.Examples.RFmxWcdmaAcpMultiCarrier
{
    public class RFmxWcdmaAcpMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxWcdmaMX wcdma;

        string resourceName = "RFSA";
        RFmxWcdmaMXMeasurementTypes measurement = RFmxWcdmaMXMeasurementTypes.Acp;
        string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
        double frequencyReferenceFrequency = 10.0e+6;                     /* Hz */
        double centerFrequency = 1.95e+9;               /* Hz */
        double externalAttenuation = 0.000000;          /* dB */
        RFmxInstrMXRFAttenuationAuto rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.True;
        double rfAttenuation = 10.0;                    
        string digitalEdgeSource = RFmxWcdmaMXConstants.Pfi0;
        RFmxWcdmaMXDigitalEdgeTriggerEdge digitalEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising;
        double triggerDelay = 0.000000;                 /* seconds */
        double measurementInterval = 0.010000;          /* seconds */
        double referenceLevel = 0.000000;               /* dBm */
        RFmxWcdmaMXAcpMeasurementMethod measurementMethod = RFmxWcdmaMXAcpMeasurementMethod.Normal;
        RFmxWcdmaMXAcpAveragingEnabled averagingEnabled = RFmxWcdmaMXAcpAveragingEnabled.False;
        int averagingCount = 10;
        RFmxWcdmaMXAcpAveragingType averagingType = RFmxWcdmaMXAcpAveragingType.Rms;
        bool autoLevel = true;
        RFmxWcdmaMXAcpSweepTimeAuto sweepTimeAuto = RFmxWcdmaMXAcpSweepTimeAuto.True;
        double sweepTimeInterval = 6.67e-4;             /* seconds */
        RFmxWcdmaMXAcpNoiseCompensationEnabled noiseCompensationEnabled = RFmxWcdmaMXAcpNoiseCompensationEnabled.False;
        int numberOfOffsets = 2;
        double timeout = 10.000000;                     /* seconds */
        Spectrum<float> spectrum;

        bool enableAllTraces = true;
        bool enableTrigger = false;

        int numberOfCarriers = 2;
        int carrierAtCenterFrequency = -1;
        RFmxWcdmaMXAcpOffsetPowerReferenceCarrier offsetPowerReferenceCarrier = RFmxWcdmaMXAcpOffsetPowerReferenceCarrier.Composite;
        int offsetPowerReferenceSpecific = 0;
        double totalCarrierPower = 0;

        double[] lowerAbsolutePower, upperAbsolutePower, lowerRelativePower, upperRelativePower;
        double[] absolutePower, relativePower;

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
            wcdma.ConfigureFrequency("", centerFrequency);
            wcdma.ConfigureExternalAttenuation("", externalAttenuation);

            instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation);
            wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
            wcdma.ConfigureContiguousCarriers("", numberOfCarriers, carrierAtCenterFrequency);

            if (autoLevel)
            {
                wcdma.AutoLevel("", measurementInterval, out referenceLevel);
                Console.WriteLine("Reference Level (dBm)      : {0}", referenceLevel);
            }
            else
            {
                wcdma.ConfigureReferenceLevel("", referenceLevel);

            }
            wcdma.SelectMeasurements("", measurement, enableAllTraces);
            wcdma.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod);
            wcdma.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            wcdma.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            wcdma.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled);
            wcdma.Acp.Configuration.ConfigureNumberOfOffsets("", numberOfOffsets);
            wcdma.Acp.Configuration.ConfigureOffsetPowerReference("", offsetPowerReferenceCarrier, offsetPowerReferenceSpecific);
            wcdma.Initiate("", "");
        }

        private void RetrieveResults()
        {
            wcdma.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref lowerRelativePower, ref upperRelativePower,
                                                          ref lowerAbsolutePower,ref upperAbsolutePower);

            wcdma.Acp.Results.FetchCarrierMeasurementArray("", timeout, ref absolutePower, ref relativePower);

            wcdma.Acp.Results.FetchTotalCarrierPower("", timeout, out totalCarrierPower);            

            wcdma.Acp.Results.FetchSpectrum("", timeout, ref spectrum);
        }

        private void PrintResults()
        {
            Console.WriteLine("\nTotal Carrier Power  (dBm) : {0}", totalCarrierPower);

            Console.WriteLine("\nCarrier Measurements       :");
            for (int i = 0; i < absolutePower.Length; i++)
            {
                Console.WriteLine("\nCarrier                    : {0}", i);
                Console.WriteLine("Absolute Power (dBm)       : {0}", absolutePower[i]);
                Console.WriteLine("Relative Power (dB)        : {0}", relativePower[i]);
            }
            Console.WriteLine("\nOffset Channel Measurements:");
            for (int i = 0; i < lowerRelativePower.Length; i++)
            {
                Console.WriteLine("\nOffset                     : {0}", i);
                Console.WriteLine("Lower Relative Power (dB)  : {0}", lowerRelativePower[i]);
                Console.WriteLine("Upper Relative Power (dB)  : {0}", upperRelativePower[i]);
                Console.WriteLine("Lower Absolute Power (dBm) : {0}", lowerAbsolutePower[i]);
                Console.WriteLine("Upper Absolute Power (dBm) : {0}", upperAbsolutePower[i]);
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
