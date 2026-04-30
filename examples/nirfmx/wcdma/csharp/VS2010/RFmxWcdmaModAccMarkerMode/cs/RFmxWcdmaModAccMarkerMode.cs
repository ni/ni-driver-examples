//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Uplink Scrambling.
//6. Configure Channel Configuration Mode.
//7[Auto Detect]. {No VIs to Configure for Channels}
//7[User Defined]. Configure User Defined Channels. 
//7[Test Model]Configure Test Model.
//8. Select ModAcc measurement and enable Traces.
//9. Configure Synchronization Mode and Measurement Interval.
//10[No Reference Waveform] Initiate Measurement & Fetch Reference Waveform
//10[Reference Waveform] {No VIs} 
//11. Configure Reference Waveform.
//12. Initiate the Measurement. 
//13. Fetch ModAcc Measurements and Traces.
//14. Close RFmx Session. 
using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WcdmaMX;

namespace NationalInstruments.Examples.RFmxWcdmaModAccMarkerMode
{
    public class RFmxWcdmaMXModAccMarkerMode
    {

        RFmxInstrMX instrSession;
        RFmxWcdmaMX wcdma;

        string resourceName = "RFSA";
        RFmxWcdmaMXMeasurementTypes measurement = RFmxWcdmaMXMeasurementTypes.ModAcc;
        string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
        double frequencyReferenceFrequency = 10.0e+6;                     /* Hz */
        double centerFrequency = 1.95e+9;               /* Hz */
        double externalAttenuation = 0.000000;          /* dB */

        string digitalEdgeSource = RFmxWcdmaMXConstants.Pfi0;
        RFmxWcdmaMXDigitalEdgeTriggerEdge digitalEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising;
        double triggerDelay = 0.000000;                 /* seconds */
        double referenceLevel = 0.000000;               /* dBm */

        double timeout = 10.000000;                     /* seconds */

        bool enableAllTraces = true;
        bool enableTrigger = true;
        int uplinkScramblingCode = 0x0;
        RFmxWcdmaMXUplinkScramblingType uplinkScramblingType = RFmxWcdmaMXUplinkScramblingType.Long;
        int measurementOffset = 0;
        int measurementLength = 1;

        int numberOfUserDefinedChannels;
        int[] userDefinedChannelsSpreadingFactor;
        int[] userDefinedChannelsSpreadingCode;
        RFmxWcdmaMXModulationType[] userDefinedChannelsModulationType;
        RFmxWcdmaMXBranch[] userDefinedChannelsBranch;

        double rmsEvm;
        double peakEvm;
        double rho;
        double frequencyError;
        double chipRateError;
        double rmsMagnitudeError;
        double rmsPhaseError;
        double iqOriginOffset;
        double iqGainImbalance;
        double iqQuadratureError;
        double peakCde;
        int peakCdeCode;
        RFmxWcdmaMXModAccPeakCdeBranch peakCdeBranch;
        double peakActiveCde;
        int peakActiveCdeSpreadingFactor;
        int peakActiveCdeCode;
        RFmxWcdmaMXModAccPeakActiveCdeBranch peakActiveCdeBranch;
        double peakRcde;
        int peakRcdeSpreadingFactor;
        int peakRcdeCode;
        RFmxWcdmaMXModAccPeakRcdeBranch peakRcdeBranch;
        AnalogWaveform<float> evm;
        ComplexSingle[] constellation;
        RFmxWcdmaMXChannelConfigurationMode channelConfigurationMode = RFmxWcdmaMXChannelConfigurationMode.TestModel;
        RFmxWcdmaMXUplinkTestModel uplinkTestModel = RFmxWcdmaMXUplinkTestModel.R6C_2_1;
        ComplexWaveform<ComplexSingle> referenceWaveform;

        public void Run()
        {
            try
            {
                InitializeVariables();
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

        void InitializeVariables()
        {
            numberOfUserDefinedChannels = 2;
            userDefinedChannelsBranch = new RFmxWcdmaMXBranch[] { RFmxWcdmaMXBranch.Q, RFmxWcdmaMXBranch.I };
            userDefinedChannelsSpreadingCode = new int[] { 0, 16 };
            userDefinedChannelsModulationType = new RFmxWcdmaMXModulationType[] { RFmxWcdmaMXModulationType.ModulationTypeBpskQpsk, RFmxWcdmaMXModulationType.ModulationTypeBpskQpsk };
            userDefinedChannelsSpreadingFactor = new int[] { 256, 64 };

            referenceWaveform = new ComplexWaveform<ComplexSingle>(0);
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
            wcdma.ConfigureUplinkScrambling("", uplinkScramblingCode, uplinkScramblingType);
            wcdma.ConfigureChannelConfigurationMode("", channelConfigurationMode);

            switch (channelConfigurationMode)
            {
                case RFmxWcdmaMXChannelConfigurationMode.UserDefined:
                    wcdma.ConfigureNumberOfChannels("", numberOfUserDefinedChannels);
                    wcdma.ConfigureUserDefinedChannelArray("", userDefinedChannelsSpreadingFactor,
                        userDefinedChannelsSpreadingCode, userDefinedChannelsModulationType, userDefinedChannelsBranch);
                    break;
                case RFmxWcdmaMXChannelConfigurationMode.TestModel:
                    wcdma.ConfigureUplinkTestModel("", uplinkTestModel);
                    break;
                default:
                    break;
            }

            wcdma.SelectMeasurements("", measurement, enableAllTraces);

            wcdma.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", RFmxWcdmaMXModAccSynchronizationMode.Marker,
                measurementOffset, measurementLength);
            if (referenceWaveform.Capacity == 0)
            {
                wcdma.Initiate("", "");
                wcdma.ModAcc.Results.FetchReferenceWaveform("", timeout, ref referenceWaveform);
            }

            wcdma.ModAcc.Configuration.ConfigureReferenceWaveform("", referenceWaveform);

        }

        void RetrieveResults()
        {
            wcdma.Initiate("", "");

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
        }

        void PrintResults()
        {

            Console.WriteLine("EVM:");
            Console.WriteLine("RMS EVM (%)                                      : {0}", rmsEvm);
            Console.WriteLine("Peak EVM (%)                                     : {0}", peakEvm);
            Console.WriteLine("Rho                                              : {0}", rho);
            Console.WriteLine("Frequency Error (Hz)                             : {0}", frequencyError);
            Console.WriteLine("Chip Rate Error (ppm)                            : {0}", chipRateError);
            Console.WriteLine("RMS Magnitude Error (%)                          : {0}", rmsMagnitudeError);
            Console.WriteLine("RMS Phase Error (deg)                            : {0}", rmsPhaseError);
            Console.WriteLine("---------------------------------------------------\n");
            Console.WriteLine("IQ Impairments :");
            Console.WriteLine("I/Q Origin Offset (dB)                           : {0}", iqOriginOffset);
            Console.WriteLine("I/Q Gain Imbalance (dB)                          : {0}",iqGainImbalance);
            Console.WriteLine("I/Q Quadrature Error (deg)                       : {0}", iqQuadratureError);
           
            Console.WriteLine("Code Domain Error:");
            Console.WriteLine("Peak CDE (dB)                                    : {0}", peakCde);
            Console.WriteLine("Peak CDE Code                                    : {0}", peakCdeCode);
            Console.WriteLine("Peak CDE Branch                                  : {0}", peakCdeBranch);
            Console.WriteLine("Peak Active CDE (dB)                             : {0}", peakActiveCde);
            Console.WriteLine("Peak Active CDE Code                             : {0}", peakActiveCdeCode);
            Console.WriteLine("Peak Active CDE Spreading Factor                 : {0}", peakActiveCdeSpreadingFactor);
            Console.WriteLine("Peak Active CDE Branch                           : {0}", peakActiveCdeBranch);
            Console.WriteLine("Peak RCDE (dB)                                   : {0}", peakRcde);
            Console.WriteLine("Peak RCDE Code                                   : {0}", peakRcdeCode);
            Console.WriteLine("Peak RCDE Spreading Factor                       : {0}", peakRcdeSpreadingFactor);
            Console.WriteLine("Peak RCDE Branch                                 : {0}", peakRcdeBranch);
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