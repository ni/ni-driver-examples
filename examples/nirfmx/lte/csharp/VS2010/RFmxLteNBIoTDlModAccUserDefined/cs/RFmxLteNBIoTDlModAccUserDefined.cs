//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Select Downlink as Link Direction. 
//7. Select ModAcc measurement and enable Traces.
//8. Configure Averaging Parameters for ModAcc measurement.
//9. Select Frame as Synchronization Mode and configure Measurement Interval.
//10. Configure EVM Unit.
//11. configure user-defined channel configuration mode.
//12. Configure NPDSCH channel on subframes.
//13. Initiate the Measurement.
//14. Fetch ModAcc Measurements and Traces.
//15. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;


namespace NationalInstruments.Examples.RFmxLteNBIoTDLModAccUserDefined
{
    public class RFmxLteNBIoTDLModAccUserDefined
    {
        RFmxInstrMX instrSession;
        RFmxLteMX lte;
        string resourceName;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        bool enableTrigger;
        string digitalEdgeTriggerSource;
        RFmxLteMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;
        double triggerDelay;

        RFmxLteMXModAccAveragingEnabled averagingEnabled;
        int averagingCount;
        int measurementOffset;
        int measurementLength;

        int nCellID;
        double[] npdschPowers;
        double timeout;
        string[] subframeString;
        RFmxLteMXNpdschEnabled[] npdschEnabled;
        RFmxLteMXNpdschModulationType[] npdschModulationType;
        double npssPower;
        double nsssPower;
        int downlinkNumberOfSubframes;

        double meanRmsCompositeEvm;
        double maxPeakCompositeEvm;
        double meanFrequencyError;
        int peakCompositeEvmSlotIndex;
        int peakCompositeEvmSymbolIndex;
        int peakCompositeEvmSubcarrierIndex;
        double meanIQOriginOffset;
        double meanIQGainImbalance;
        double meanIQQuadratureError;
        double meanRmsEvm;
        double meanRmsQpskEvm;
        double meanRms16QamEvm;
        double meanRmsNpssEvm;
        double meanRmsNsssEvm;
        double meanRmsNrsEvm;
        ComplexSingle[] qpskConstellation;
        ComplexSingle[] qam16Constellation;
        AnalogWaveform<float> meanRmsEvmPerSubcarrier;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureLte();
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

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void InitializeVariables()
        {
            resourceName = "RFSA";

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e6;                                          /* (Hz) */

            centerFrequency = 2.14e9;                                                    /* (Hz) */
            referenceLevel = 0.00;                                                       /* (dBm) */
            externalAttenuation = 0.0;                                                   /* (dB) */

            enableTrigger = false;
            digitalEdgeTriggerSource = RFmxLteMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.0; /* (s) */

            averagingEnabled = RFmxLteMXModAccAveragingEnabled.False;
            averagingCount = 10;

            measurementOffset = 0;                                                        /*(slots) */
            measurementLength = 20;                                                       /*(slots) */

            nCellID = 0;
            npssPower = 0;                                                                 /* (dB) */
            nsssPower = 0;                                                                 /* (dB) */
            downlinkNumberOfSubframes = 20;
            npdschPowers = new double[20]{ 0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0 };      /* (dB) */

            npdschEnabled = new RFmxLteMXNpdschEnabled[20]{ RFmxLteMXNpdschEnabled.False, RFmxLteMXNpdschEnabled.False, RFmxLteMXNpdschEnabled.False,
                RFmxLteMXNpdschEnabled.False, RFmxLteMXNpdschEnabled.False, RFmxLteMXNpdschEnabled.False, RFmxLteMXNpdschEnabled.True,
                RFmxLteMXNpdschEnabled.False,RFmxLteMXNpdschEnabled.False, RFmxLteMXNpdschEnabled.False, RFmxLteMXNpdschEnabled.False, RFmxLteMXNpdschEnabled.False, RFmxLteMXNpdschEnabled.False,
                RFmxLteMXNpdschEnabled.False, RFmxLteMXNpdschEnabled.False, RFmxLteMXNpdschEnabled.False, RFmxLteMXNpdschEnabled.False,
                RFmxLteMXNpdschEnabled.False,RFmxLteMXNpdschEnabled.False, RFmxLteMXNpdschEnabled.False };

            npdschModulationType = new RFmxLteMXNpdschModulationType[20]{ RFmxLteMXNpdschModulationType.Qpsk, RFmxLteMXNpdschModulationType.Qpsk, RFmxLteMXNpdschModulationType.Qpsk,
                RFmxLteMXNpdschModulationType.Qpsk, RFmxLteMXNpdschModulationType.Qpsk, RFmxLteMXNpdschModulationType.Qpsk, RFmxLteMXNpdschModulationType.Qpsk,
                RFmxLteMXNpdschModulationType.Qpsk,RFmxLteMXNpdschModulationType.Qpsk, RFmxLteMXNpdschModulationType.Qpsk, RFmxLteMXNpdschModulationType.Qpsk, RFmxLteMXNpdschModulationType.Qpsk, RFmxLteMXNpdschModulationType.Qpsk,
                RFmxLteMXNpdschModulationType.Qpsk, RFmxLteMXNpdschModulationType.Qpsk, RFmxLteMXNpdschModulationType.Qpsk, RFmxLteMXNpdschModulationType.Qpsk,
                RFmxLteMXNpdschModulationType.Qpsk,RFmxLteMXNpdschModulationType.Qpsk, RFmxLteMXNpdschModulationType.Qpsk };

            timeout = 10.0; /* (s) */
        }

        void ConfigureLte()
        {
            lte = instrSession.GetLteSignalConfiguration();                                /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            lte.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger);
            lte.ComponentCarrier.Configure("", 200e3, 0.0, 0);
            lte.ConfigureLinkDirection("", RFmxLteMXLinkDirection.Downlink);
            lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, true);
            lte.ModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", RFmxLteMXModAccSynchronizationMode.Frame, measurementOffset, measurementLength);
            lte.ModAcc.Configuration.ConfigureEvmUnit("", RFmxLteMXModAccEvmUnit.Percentage);
            lte.ComponentCarrier.SetNCellId("", nCellID);
            lte.ComponentCarrier.SetNpssPower("", npssPower);
            lte.ComponentCarrier.SetNsssPower("", nsssPower);
            lte.ComponentCarrier.SetDownlinkNumberOfSubframes("", 20);
            lte.ComponentCarrier.SetNBIoTDownlinkChannelConfigurationMode("", RFmxLteMXNBIoTDownlinkChannelConfigurationMode.UserDefined);
            subframeString = new string[20];
            for (int i = 0; i < 20; i++)
            {
                subframeString[i] = RFmxLteMX.BuildSubframeString("", i);
                lte.ComponentCarrier.SetNpdschEnabled(subframeString[i], npdschEnabled[i]);
                lte.ComponentCarrier.SetNpdschPower(subframeString[i], npdschPowers[i]);
                lte.ComponentCarrier.SetNpdschModulationType(subframeString[i], npdschModulationType[i]);
            }
            lte.Initiate("", "");
        }

        void RetrieveResults()
        {
            lte.ModAcc.Results.FetchCompositeEvm("", timeout, out meanRmsCompositeEvm, out maxPeakCompositeEvm, 
                                                out meanFrequencyError,
                out peakCompositeEvmSymbolIndex, out peakCompositeEvmSubcarrierIndex, out peakCompositeEvmSlotIndex);
            lte.ModAcc.Results.FetchIQImpairments("", timeout, out meanIQOriginOffset, out meanIQGainImbalance, 
                                                  out meanIQQuadratureError);
            lte.ModAcc.Results.GetNpdschMeanRmsEvm("", out meanRmsEvm);
            lte.ModAcc.Results.GetNpdschMeanRmsQpskEvm("", out meanRmsQpskEvm);
            lte.ModAcc.Results.GetNpdschMeanRms16QamEvm("", out meanRms16QamEvm);
            lte.ModAcc.Results.GetMeanRmsNpssEvm("", out meanRmsNpssEvm);
            lte.ModAcc.Results.GetMeanRmsNsssEvm("", out meanRmsNsssEvm);
            lte.ModAcc.Results.GetMeanRmsNrsEvm("", out meanRmsNrsEvm);
            lte.ModAcc.Results.FetchNpdschQpskConstellation("", timeout, ref qpskConstellation);
            lte.ModAcc.Results.FetchNpdsch16QamConstellation("", timeout, ref qam16Constellation);
            lte.ModAcc.Results.FetchEvmPerSubcarrierTrace("", timeout, ref meanRmsEvmPerSubcarrier);
        }

        void PrintResults()
        {
            Console.WriteLine("------------------Measurement---------------");
            Console.WriteLine("Mean RMS Composite EVM  (% or dB)    : {0}", meanRmsCompositeEvm);
            Console.WriteLine("Max Peak Composite EVM  (% or dB)    : {0}", maxPeakCompositeEvm);
            Console.WriteLine("NPDSCH Mean RMS  EVM  (% or dB)      : {0}", meanRmsEvm);
            Console.WriteLine("NPDSCH Mean RMS QPSK EVM  (% or dB)  : {0}", meanRmsQpskEvm);
            Console.WriteLine("NPDSCH Mean RMS 16QAM EVM  (% or dB) : {0}", meanRms16QamEvm);
            Console.WriteLine("Mean RMS NPSS EVM  (% or dB)         : {0}", meanRmsNpssEvm);
            Console.WriteLine("Mean RMS NSSS EVM  (% or dB)         : {0}", meanRmsNsssEvm);
            Console.WriteLine("Mean RMS NRS EVM (% or dB)           : {0}", meanRmsNrsEvm);
            Console.WriteLine("Mean Frequency Error  (Hz)           : {0}", meanFrequencyError);
            Console.WriteLine("Mean IQ Origin Offset  (dBc)         : {0}", meanIQOriginOffset);
        }

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

        void CloseSession()
        {
            if (lte != null)
            {
                lte.Dispose();
                lte = null;
            }
            if (instrSession != null)
            {
                instrSession.Close();
                instrSession = null;
            }
        }
    }
}
