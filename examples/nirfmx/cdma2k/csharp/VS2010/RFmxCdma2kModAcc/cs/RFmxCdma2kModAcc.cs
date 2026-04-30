//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties: Clock Source and Clock Frequency
//3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation
//4. Configure the trigger properties
//5. Select ModAcc measurement and enable the traces
//6. Configure Uplink Scrambling Code
//7. Configure the basic Measurement Settings
//8. Configure the Midamble Settings 
//9. Initiate Measurement
//10. Fetch ModAcc Measurements and Traces
//11. Close the RFmx session

using System;
using NationalInstruments.RFmx.Cdma2kMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxCdma2kModAcc
{
    public class RFmxCdma2kModAcc
    {
        RFmxInstrMX instrSession;
        RFmxCdma2kMX cdma2k;
        string resourceName, frequencySource, digitalEdgeTriggerSource;
        double centerFrequency, referenceLevel, externalAttenuation, frequency;

        double triggerDelay;
        bool enableTrigger;
        RFmxCdma2kMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;

        RFmxCdma2kMXRadioConfiguration radioConfiguration;

        long uplinkSpreadingLongCodeMask;

        RFmxCdma2kMXModAccPeakActiveCdeBranch peakActiveCdeBranch;
        RFmxCdma2kMXModAccSynchronizationMode synchronizationMode;
        int measurementOffset, measurementLength;

        int peakActiveCdeWalshCodeLength, peakActiveCdeWalshCodeNumber;

        double timeout;
        double rmsEvm, peakEvm, rho, frequencyError, chipRateError, rmsMagnitudeError, rmsPhaseError;
        double peakCde, peakActiveCde;
        double iqOriginOffset, iqGainImbalence, iqQuadratureError;
        int peakCdeWalshCodeNumber;
        RFmxCdma2kMXModAccPeakCdeBranch peakCdeBranch;

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
            uplinkSpreadingLongCodeMask = 0;

            synchronizationMode = RFmxCdma2kMXModAccSynchronizationMode.Slot;

            measurementOffset = 0;
            measurementLength = 1;
            
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
            cdma2k.ConfigureUplinkSpreading("", uplinkSpreadingLongCodeMask);
            cdma2k.SelectMeasurements("", RFmxCdma2kMXMeasurementTypes.ModAcc, true);
            cdma2k.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset,
                                                                               measurementLength);
            cdma2k.Initiate("", "");
        }

        private void RetrieveResults()
        {
            ComplexSingle[] constellation = null;
            AnalogWaveform<float> evm = null;
            /* Retrieve results */
            cdma2k.ModAcc.Results.FetchEvm("", timeout, out  rmsEvm, out  peakEvm, out  rho, out  frequencyError,
                                           out  chipRateError, out  rmsMagnitudeError, out  rmsPhaseError);
            cdma2k.ModAcc.Results.FetchIQImpairments("", timeout, out iqOriginOffset, out iqGainImbalence, out iqQuadratureError);
            cdma2k.ModAcc.Results.FetchPeakCde("", timeout, out peakCde, out peakCdeWalshCodeNumber, out peakCdeBranch);
            cdma2k.ModAcc.Results.FetchPeakActiveCde("", timeout, out peakActiveCde, out peakActiveCdeWalshCodeLength, 
                                                     out peakActiveCdeWalshCodeNumber, out peakActiveCdeBranch);
            cdma2k.ModAcc.Results.FetchConstellationTrace("", timeout, ref constellation);
            cdma2k.ModAcc.Results.FetchEvmTrace("", timeout, ref evm);
        }

        private void PrintResults()
        {
            Console.WriteLine("--------------------EVM Results--------------------");
            Console.WriteLine("RMS EVM (%)                       : {0}", rmsEvm);
            Console.WriteLine("Peak EVM (%)                      : {0}", peakEvm);
            Console.WriteLine("Rho                               : {0}", rho);
            Console.WriteLine("Frequency Error (Hz)              : {0}", frequencyError);
            Console.WriteLine("Chip Rate Error (ppm)             : {0}", chipRateError);
            Console.WriteLine("RMS Magnitude Error (%)           : {0}", rmsMagnitudeError);
            Console.WriteLine("RMS Phase Error (deg)             : {0}", rmsPhaseError);


            Console.WriteLine("---------------------I/Q Impairments------------------------");
            Console.WriteLine("I/Q Origin Offset (dB)            : {0}", iqOriginOffset);
            Console.WriteLine("I/Q Gain Imbalance (dB)           : {0}", iqGainImbalence);
            Console.WriteLine("I/Q Quadrature Error (deg)        : {0}", iqQuadratureError); 


            Console.WriteLine("-------------------Code Domain Error------------------------");
            Console.WriteLine("Peak CDE (dB)                     : {0}", peakCde);
            Console.WriteLine("Peak CDE Walsh Code Number        : {0}", peakCdeWalshCodeNumber);
            Console.WriteLine("Peak Active CDE (dB)              : {0}", peakActiveCde);
            Console.WriteLine("Peak CDE Branch                   : {0}", (peakCdeBranch == RFmxCdma2kMXModAccPeakCdeBranch.I) ? "I" : "Q");
            Console.WriteLine("Peak Active CDE Walsh Code Number : {0}", peakActiveCdeWalshCodeNumber);
            Console.WriteLine("Peak Active CDE Walsh Code Length : {0}", peakActiveCdeWalshCodeLength);
            Console.WriteLine("Peak Active CDE Branch            : {0}", (peakActiveCdeBranch == RFmxCdma2kMXModAccPeakActiveCdeBranch.I) ? "I" : "Q");
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
