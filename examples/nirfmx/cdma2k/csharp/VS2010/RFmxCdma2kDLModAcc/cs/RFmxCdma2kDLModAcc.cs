//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation.
//4. Configure the Trigger Type and Trigger Parameters.
//5. Configure Radio Configuration Parameter.
//6. Configure Downlink PN Offset.
//7. Select ModAcc measurement and enable the traces.
//8. Configure Synchronization Mode and Measurement Interval. 
//9. Configure Multi Carrier filter.
//10. Initiate the Measurement.
//11. Fetch ModAcc Measurements and Traces.
//12. Close the RFmx session.

using System;
using NationalInstruments.RFmx.Cdma2kMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxCdma2kDLModAcc
{
    public class RFmxCdma2kDLModAcc
    {
        RFmxInstrMX instrSession;
        RFmxCdma2kMX cdma2k;
        string resourceName, frequencySource, digitalEdgeTriggerSource;
        double centerFrequency, referenceLevel, externalAttenuation, frequency;

        double triggerDelay;
        bool enableTrigger;
        RFmxCdma2kMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;

        RFmxCdma2kMXRadioConfiguration radioConfiguration;

        int DLSpreadingPNOffset;

        RFmxCdma2kMXModAccMultiCarrierFilterEnabled multiCarrierFilterEnabled;

        RFmxCdma2kMXModAccPeakActiveCdeBranch peakActiveCdeBranch;
        RFmxCdma2kMXModAccSynchronizationMode synchronizationMode;
        int measurementOffset, measurementLength;

        int peakActiveCdeWalshCodeLength, peakActiveCdeWalshCodeNumber;

        double timeout;
        double RMSEvm, peakEvm, rho, frequencyError, chipRateError, RMSMagnitudeError, rmsPhaseError;
        double peakCde, peakActiveCde;
        double iqOriginOffset, iqGainImbalence, iqQuadratureError;
        int peakCdeWalshCodeNumber;
        RFmxCdma2kMXModAccPeakCdeBranch peakCdeBranch;
        int[] detectedWalshCodeLength, detectedWalshCodeNumber;
        RFmxCdma2kMXModAccDetectedBranch[] detectedBranch;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureCdma2k();
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
                Console.WriteLine("Press any key to exit.....");
                Console.ReadKey();
            }
        }

        private void InitializeVariables()
        {
            /* Initialize input variables */

            resourceName = "RFSA";

            centerFrequency = 833.49e+6;                                            /* Hz */
            referenceLevel = 0.00;                                                  /* dBm */
            externalAttenuation = 0.00;                                             /* dB */

            frequencySource = RFmxInstrMXConstants.OnboardClock;
            frequency = 10e+6;                                                      /* Hz */

            triggerDelay = 0.00;                                                    /* seconds */
            enableTrigger = false;
            digitalEdgeTriggerEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising;
            digitalEdgeTriggerSource = RFmxCdma2kMXConstants.Pfi0;

            radioConfiguration = RFmxCdma2kMXRadioConfiguration.RC3;
            DLSpreadingPNOffset = 0;                                    /* 64 chips */
            multiCarrierFilterEnabled = RFmxCdma2kMXModAccMultiCarrierFilterEnabled.False;

            synchronizationMode = RFmxCdma2kMXModAccSynchronizationMode.Slot;

            measurementOffset = 0;                                                  /* slots */
            measurementLength = 1;                                                  /* slots */
            
            timeout = 10;                                                           /* seconds */
        }

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void ConfigureCdma2k()
        {
            /* Get SpecAn signal */
            cdma2k = instrSession.GetCdma2kSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
            cdma2k.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            cdma2k.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger);
            cdma2k.ConfigureRadioConfiguration("", radioConfiguration);
            cdma2k.SetLinkDirection("", RFmxCdma2kMXLinkDirection.Downlink);
            cdma2k.SetDownlinkSpreadingPNOffset("", DLSpreadingPNOffset);
            cdma2k.SelectMeasurements("", RFmxCdma2kMXMeasurementTypes.ModAcc, true);
            cdma2k.ModAcc.Configuration.SetMultiCarrierFilterEnabled("", multiCarrierFilterEnabled);
            cdma2k.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset,
                                                                               measurementLength);
            cdma2k.Initiate("", "");
        }

        private void RetrieveResults()
        {
            ComplexSingle[] constellation = null;
            AnalogWaveform<float> evm = null;
            /* Retrieve results */
            cdma2k.ModAcc.Results.FetchEvm("", timeout, out  RMSEvm, out  peakEvm, out  rho, out  frequencyError,
                                           out  chipRateError, out  RMSMagnitudeError, out  rmsPhaseError);
            cdma2k.ModAcc.Results.FetchIQImpairments("", timeout, out iqOriginOffset, out iqGainImbalence, out iqQuadratureError);
            cdma2k.ModAcc.Results.FetchPeakCde("", timeout, out peakCde, out peakCdeWalshCodeNumber, out peakCdeBranch);
            cdma2k.ModAcc.Results.FetchPeakActiveCde("", timeout, out peakActiveCde, out peakActiveCdeWalshCodeLength, 
                                                     out peakActiveCdeWalshCodeNumber, out peakActiveCdeBranch);
            cdma2k.ModAcc.Results.FetchConstellationTrace("", timeout, ref constellation);
            cdma2k.ModAcc.Results.FetchEvmTrace("", timeout, ref evm);
            cdma2k.ModAcc.Results.FetchDetectedChannelArray("", timeout, ref detectedWalshCodeLength,
                ref detectedWalshCodeNumber, ref detectedBranch);
        }

        private void PrintResults()
        {
            Console.WriteLine("--------------------EVM Results--------------------");
            Console.WriteLine("RMS EVM (%)                       : {0}", RMSEvm);
            Console.WriteLine("Peak EVM (%)                      : {0}", peakEvm);
            Console.WriteLine("Rho                               : {0}", rho);
            Console.WriteLine("Frequency Error (Hz)              : {0}", frequencyError);
            Console.WriteLine("Chip Rate Error (ppm)             : {0}", chipRateError);
            Console.WriteLine("RMS Magnitude Error (%)           : {0}", RMSMagnitudeError);
            Console.WriteLine("RMS Phase Error (deg)             : {0}", rmsPhaseError);


            Console.WriteLine("\n---------------------I/Q Impairments------------------------");
            Console.WriteLine("I/Q Origin Offset (dB)            : {0}", iqOriginOffset);
            Console.WriteLine("I/Q Gain Imbalance (dB)           : {0}", iqGainImbalence);
            Console.WriteLine("I/Q Quadrature Error (deg)        : {0}", iqQuadratureError); 


            Console.WriteLine("\n-------------------Code Domain Error------------------------");
            Console.WriteLine("Peak CDE (dB)                     : {0}", peakCde);
            Console.WriteLine("Peak CDE Walsh Code Number        : {0}", peakCdeWalshCodeNumber);
            Console.WriteLine("Peak CDE Branch                   : {0}", peakCdeBranch);
            Console.WriteLine("Peak Active CDE (dB)              : {0}", peakActiveCde);
            Console.WriteLine("Peak Active CDE Walsh Code Number : {0}", peakActiveCdeWalshCodeNumber);
            Console.WriteLine("Peak Active CDE Walsh Code Length : {0}", peakActiveCdeWalshCodeLength);
            Console.WriteLine("Peak Active CDE Branch            : {0}", peakActiveCdeBranch);

            Console.WriteLine("\n---------------------Detected Channels--------------------");
            for (int i = 0; i < detectedWalshCodeLength.Length; i++)
            {
                Console.WriteLine("Idx                               : {0}", i);
                Console.WriteLine("Length                            : {0}", detectedWalshCodeLength[i]);
                Console.WriteLine("Number                            : {0}", detectedWalshCodeNumber[i]);
                Console.WriteLine("Branch                            : {0}\n", detectedBranch[i]);
            }
        }

        private void CloseSession()
        {
            try
            {
                if (cdma2k != null)
                {
                    cdma2k.Dispose();
                    cdma2k = null;
                }

                if (instrSession != null)
                {
                    instrSession.Close();
                    instrSession = null;
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        static private void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }
    }
}
