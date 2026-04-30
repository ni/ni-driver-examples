//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Downlink Scrambling.
//6. Select ModAcc measurement and enable Traces.
//7. Configure Synchronization Mode and Measurement Interval.
//8. Initiate the Measurement.
//9. Fetch ModAcc Measurements and Traces.
//10. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WcdmaMX;

namespace NationalInstruments.Examples.RFmxWcdmaDLModAccSingleCarrier
{
    public class RFmxWcdmaDLModAccSingleCarrier
    {
        RFmxInstrMX instrSession;
        RFmxWcdmaMX wcdma;

        string resourceName;
        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        bool enableTrigger;
        string digitalEdgeTriggerSource;
        RFmxWcdmaMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;
        double triggerDelay;

        RFmxWcdmaMXModAccSynchronizationMode synchronizationMode;
        int measurementOffset;
        int measurementLength;

        RFmxWcdmaMXDownlinkScramblingType downlinkScramblingType;
        int downlinkScramblingPrimaryCode;
        int downlinkScramblingSecondaryCode;

        RFmxWcdmaMXMeasurementTypes measurement;
        bool enableAllTraces;
        double timeout;

        double rmsEvm, peakEvm, rho, frequencyError, chipRateError, rmsMagnitudeError, rmsPhaseError;
        double iqOriginOffset, iqGainImbalance, iqQuadratureError;
        double peakCde, peakActiveCde, peakRcde;
        int peakCdeCode, peakActiveCdeSpreadingFactor, peakActiveCdeCode, peakRcdeSpreadingFactor, peakRcdeCode;
        RFmxWcdmaMXModAccPeakCdeBranch peakCdeBranch;
        RFmxWcdmaMXModAccPeakActiveCdeBranch peakActiveCdeBranch;
        RFmxWcdmaMXModAccPeakRcdeBranch peakRcdeBranch;
        AnalogWaveform<float> evm;
        ComplexSingle[] constellation;
        int[] detectedSpreadingFactor, detectedSpreadingCode;
        RFmxWcdmaMXModAccDetectedModulationType[] detectedModulationType;
        RFmxWcdmaMXModAccDetectedBranch[] detectedBranch;

        public void Run()
        {
            try
            {
                InitializeVariable();
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

        private void InitializeVariable()
        {
            /* Initialize input variables */

            resourceName = "RFSA";
            centerFrequency = 1.95e+9;                                                          /* Hz */
            referenceLevel = 0.0;                                                               /* dBm */
            externalAttenuation = 0.0;                                                          /* dB */

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;                                              /* Hz */

            enableTrigger = false;
            digitalEdgeTriggerSource = RFmxWcdmaMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.0;                                                                 /* seconds */

            synchronizationMode = RFmxWcdmaMXModAccSynchronizationMode.Slot;
            measurementOffset = 0;                                                              /* slots */
            measurementLength = 1;                                                              /* slots */

            downlinkScramblingType = RFmxWcdmaMXDownlinkScramblingType.Standard;
            downlinkScramblingPrimaryCode = 0;
            downlinkScramblingSecondaryCode = 0;

            measurement = RFmxWcdmaMXMeasurementTypes.ModAcc;
            enableAllTraces = true;

            timeout = 10.0;                                                                     /* seconds */
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
            wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge,
                triggerDelay, enableTrigger);
            wcdma.SetLinkDirection("", RFmxWcdmaMXLinkDirection.Downlink);
            wcdma.SetDownlinkScramblingType("", downlinkScramblingType);
            wcdma.SetDownlinkScramblingPrimaryCode("", downlinkScramblingPrimaryCode);
            wcdma.SetDownlinkScramblingSecondaryCode("", downlinkScramblingSecondaryCode);
            wcdma.SelectMeasurements("", measurement, enableAllTraces);
            wcdma.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
                measurementOffset, measurementLength);
            wcdma.Initiate("", "");
        }

        void RetrieveResults()
        {
            wcdma.ModAcc.Results.FetchEvm("", timeout, out rmsEvm, out peakEvm, out rho, out frequencyError,
                out chipRateError, out rmsMagnitudeError, out rmsPhaseError);
            wcdma.ModAcc.Results.FetchIQImpairments("", timeout, out iqOriginOffset, out iqGainImbalance,
                out iqQuadratureError);
            wcdma.ModAcc.Results.FetchPeakCde("", timeout, out peakCde, out peakCdeCode, out peakCdeBranch);
            wcdma.ModAcc.Results.FetchPeakActiveCde("", timeout, out peakActiveCde,
                out peakActiveCdeSpreadingFactor, out peakActiveCdeCode, out peakActiveCdeBranch);
            wcdma.ModAcc.Results.FetchRcde("", timeout, out peakRcde, out peakRcdeSpreadingFactor,
                out peakRcdeCode, out peakRcdeBranch);
            wcdma.ModAcc.Results.FetchEvmTrace("", timeout, ref evm);
            wcdma.ModAcc.Results.FetchConstellationTrace("", timeout, ref constellation);
            wcdma.ModAcc.Results.FetchDetectedChannelArray("", timeout, ref detectedSpreadingFactor,
                ref detectedSpreadingCode, ref detectedModulationType, ref detectedBranch);
        }

        void PrintResults()
        {
            Console.WriteLine("----------------------------EVM---------------------------");
            Console.WriteLine("RMS EVM (%)                              : {0}", rmsEvm);
            Console.WriteLine("Peak EVM (%)                             : {0}", peakEvm);
            Console.WriteLine("Rho                                      : {0}", rho);
            Console.WriteLine("Frequency Error (Hz)                     : {0}", frequencyError);
            Console.WriteLine("Chip Rate Error (ppm)                    : {0}", chipRateError);
            Console.WriteLine("RMS Magnitude Error (%)                  : {0}", rmsMagnitudeError);
            Console.WriteLine("RMS Phase Error (deg)                    : {0}", rmsPhaseError);

            Console.WriteLine("\n----------------------IQ Impairments----------------------");
            Console.WriteLine("I/Q Origin Offset (dB)                   : {0}", iqOriginOffset);
            Console.WriteLine("I/Q Gain Imbalance (dB)                  : {0}", iqGainImbalance);
            Console.WriteLine("I/Q Quadrature Error (deg)               : {0}", iqQuadratureError);

            Console.WriteLine("\n---------------------Code Domain Error--------------------");
            Console.WriteLine("Peak CDE (dB)                            : {0}", peakCde);
            Console.WriteLine("Peak CDE Code                            : {0}", peakCdeCode);
            Console.WriteLine("Peak CDE Branch                          : {0}", peakCdeBranch);
            Console.WriteLine("Peak Active CDE (dB)                     : {0}", peakActiveCde);
            Console.WriteLine("Peak Active CDE Code                     : {0}", peakActiveCdeCode);
            Console.WriteLine("Peak Active CDE Spreading Factor         : {0}", peakActiveCdeSpreadingFactor);
            Console.WriteLine("Peak Active CDE Branch                   : {0}", peakActiveCdeBranch);
            Console.WriteLine("Peak RCDE (dB)                           : {0}", peakRcde);
            Console.WriteLine("Peak RCDE Code                           : {0}", peakRcdeCode);
            Console.WriteLine("Peak RCDE Spreading Factor               : {0}", peakRcdeSpreadingFactor);
            Console.WriteLine("Peak RCDE Branch                         : {0}", peakRcdeBranch);

            Console.WriteLine("\n---------------------Detected Channels--------------------");
            for (int i = 0; i < detectedSpreadingFactor.Length; i++)
            {
                Console.WriteLine("Idx                                      : {0}", i);
                Console.WriteLine("SF                                       : {0}", detectedSpreadingFactor[i]);
                Console.WriteLine("Code                                     : {0}", detectedSpreadingCode[i]);
                Console.WriteLine("Modulation                               : {0}", detectedModulationType[i]);
                Console.WriteLine("Branch                                   : {0}\n", detectedBranch[i]);
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

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

    }
}