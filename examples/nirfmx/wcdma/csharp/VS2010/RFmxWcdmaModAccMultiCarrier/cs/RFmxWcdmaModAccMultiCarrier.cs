//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Contiguous Carriers.
//6. Configure Uplink Scramble (Array).
//7. Select ModAcc measurement and enable Traces.
//8. Configure Syncronization Mode and Interval.
//9. Initiate the Measurement.
//10. Fetch ModAcc Measurements and Traces.
//11.Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WcdmaMX;

namespace NationalInstruments.Examples.RFmxWcdmaModAccMultiCarrier
{
    struct CarrierMeasurement
    {
        public AnalogWaveform<float> evm;
        public ComplexSingle[] constellation;
    };

    public class RFmxWcdmaModAccMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxWcdmaMX wcdma;

        string resourceName;
        RFmxWcdmaMXMeasurementTypes measurement;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;
        double centerFrequency;
        double externalAttenuation;

        string digitalEdgeTriggerSource;
        RFmxWcdmaMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;
        double triggerDelay;
        double referenceLevel;
        double timeout;

        const int numberOfCarriers = 2;
        bool enableAllTraces;
        bool enableTrigger;
        int[] uplinkScramblingCode = { 0, 0 };
        RFmxWcdmaMXUplinkScramblingType[] uplinkScramblingType = { RFmxWcdmaMXUplinkScramblingType.Long, RFmxWcdmaMXUplinkScramblingType.Long };
        RFmxWcdmaMXModAccSynchronizationMode synchronizationMode;

        int measurementOffset;
        int measurementLength;
        int carrierAtCenterFrequency;

        double[] rmsEvm, peakEvm, rho, frequencyError, chipRateError, rmsMagnitudeError, rmsPhaseError;
        double[] iqOriginOffset, iqGainImbalance, iqQuadratureError;

        double[] peakCde, peakActiveCde, peakRcde;
        int[] peakCdeCode, peakActiveCdeSpreadingFactor, peakActiveCdeCode, peakRcdeSpreadingFactor, peakRcdeCode;
        RFmxWcdmaMXModAccPeakCdeBranch[] peakCdeBranch;
        RFmxWcdmaMXModAccPeakActiveCdeBranch[] peakActiveCdeBranch;
        RFmxWcdmaMXModAccPeakRcdeBranch[] peakRcdeBranch;
        CarrierMeasurement[] carrierChannelOutput = new CarrierMeasurement[numberOfCarriers];


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
            measurement = RFmxWcdmaMXMeasurementTypes.ModAcc;
            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;                                              /* Hz */
            centerFrequency = 1.95e+9;                                                          /* Hz */
            externalAttenuation = 0.000000;                                                     /* dB */

            digitalEdgeTriggerSource = RFmxWcdmaMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.000000;                                                            /* seconds */
            referenceLevel = 0.000000;                                                          /* dBm */

            synchronizationMode = RFmxWcdmaMXModAccSynchronizationMode.Slot;
            measurementOffset = 0;                                                              /*slots*/
            measurementLength = 1;                                                             /*slots*/
            carrierAtCenterFrequency = -1;

            enableAllTraces = true;
            enableTrigger = false;

            timeout = 10.0;
        }

        void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureWcdma()
        {
            /* Get Wcdma signal */
            wcdma = instrSession.GetWcdmaSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger);
            wcdma.ConfigureContiguousCarriers("", numberOfCarriers, carrierAtCenterFrequency);
            wcdma.ConfigureUplinkScramblingArray("", uplinkScramblingType, uplinkScramblingCode);

            wcdma.SelectMeasurements("", measurement, enableAllTraces);

            wcdma.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
                measurementOffset, measurementLength);
            wcdma.Initiate("", "");
        }

        void RetrieveResults()
        {
            string carrierString;

            wcdma.ModAcc.Results.FetchEvmArray("", timeout, ref rmsEvm, ref peakEvm, ref rho, ref frequencyError,
                ref chipRateError, ref rmsMagnitudeError, ref rmsPhaseError);
            wcdma.ModAcc.Results.FetchIQImpairmentsArray("", timeout, ref iqOriginOffset, ref iqGainImbalance,
                ref iqQuadratureError);
            wcdma.ModAcc.Results.FetchPeakCdeArray("", timeout, ref peakCde, ref peakCdeCode, ref peakCdeBranch);
            wcdma.ModAcc.Results.FetchPeakActiveCdeArray("", timeout, ref peakActiveCde,
                ref peakActiveCdeSpreadingFactor, ref peakActiveCdeCode, ref peakActiveCdeBranch);
            wcdma.ModAcc.Results.FetchRcdeArray("", timeout, ref peakRcde, ref peakRcdeSpreadingFactor,
                ref peakRcdeCode, ref peakRcdeBranch);
            for (int i = 0; i < numberOfCarriers; i++)
            {
                carrierString = RFmxWcdmaMX.BuildCarrierString("", i);
                wcdma.ModAcc.Results.FetchEvmTrace(carrierString, timeout, ref carrierChannelOutput[i].evm);
                wcdma.ModAcc.Results.FetchConstellationTrace("", timeout, ref carrierChannelOutput[i].constellation);
            }
        }

        void PrintResults()
        {

            Console.WriteLine("-----------------------------EVM------------------------\n");
            for (int i = 0; i < numberOfCarriers; i++)
            {
                Console.WriteLine("\nCarrier {0}", i);
                Console.WriteLine("RMS EVM (%)                                      : {0}", rmsEvm[i]);
                Console.WriteLine("Peak EVM (%)                                     : {0}", peakEvm[i]);
                Console.WriteLine("Rho                                              : {0}", rho[i]);
                Console.WriteLine("Frequency Error (Hz)                             : {0}", frequencyError[i]);
                Console.WriteLine("Chip Rate Error (ppm)                            : {0}", chipRateError[i]);
                Console.WriteLine("RMS Magnitude Error (%)                          : {0}", rmsMagnitudeError[i]);
                Console.WriteLine("RMS Phase Error (deg)                            : {0}", rmsPhaseError[i]);
            }

            Console.WriteLine("\n---------------------IQ Impairments------------------\n");
            for (int i = 0; i < numberOfCarriers; i++)
            {
                Console.WriteLine("\nCarrier {0}", i);
                Console.WriteLine("\nI/Q Origin Offset (dB)                           : {0}", iqOriginOffset[i]);
                Console.WriteLine("I/Q Gain Imbalance (dB)                          : {0}", iqGainImbalance[i]);
                Console.WriteLine("I/Q Quadrature Error (deg)                       : {0}", iqQuadratureError[i]);
            }

            Console.WriteLine("\n---------------------Peak CDE----------------------------");
            for (int i = 0; i < numberOfCarriers; i++)
            {
                Console.WriteLine("\nCarrier {0}", i);
                Console.WriteLine("Peak CDE (dB)                                    : {0}", peakCde[i]);
                Console.WriteLine("Peak CDE Code                                    : {0}", peakCdeCode[i]);
                Console.WriteLine("Peak CDE Branch                                  : {0}", peakCdeBranch[i]);
            }

            Console.WriteLine("\n----------------------Peak Active CDE----------------\n");
            for (int i = 0; i < numberOfCarriers; i++)
            {
                Console.WriteLine("\nCarrier {0}", i);
                Console.WriteLine("Peak Active CDE (dB)                             : {0}", peakActiveCde[i]);
                Console.WriteLine("Peak Active CDE Spreading Factor                 : {0}", peakActiveCdeSpreadingFactor[i]);
                Console.WriteLine("Peak Active CDE Code                             : {0}", peakActiveCdeCode[i]);
                Console.WriteLine("Peak Active CDE Branch                           : {0}", peakActiveCdeBranch[i]);
            }

            Console.WriteLine("\n------------Peak RCDE------------\n");
            for (int i = 0; i < numberOfCarriers; i++)
            {
                Console.WriteLine("\nCarrier {0}", i);
                Console.WriteLine("Peak RCDE (dB)                                   : {0}", peakRcde[i]);
                Console.WriteLine("Peak RCDE Spreading Factor                       : {0}", peakRcdeSpreadingFactor[i]);
                Console.WriteLine("Peak RCDE Code                                   : {0}", peakRcdeCode[i]);
                Console.WriteLine("Peak RCDE Branch                                 : {0}", peakRcdeBranch[i]);
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