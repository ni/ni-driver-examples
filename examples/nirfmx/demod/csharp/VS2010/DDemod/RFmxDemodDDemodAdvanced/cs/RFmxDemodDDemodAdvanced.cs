//Steps:
//1. Open a new RFmxInstrMX session and create a Demod Signal
//2. Configure the basic instrument properties (Clock Source, Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time)
//6. Select DDemod Measurement and enable  the traces 
//7. Configure Modulation Type
//8. Configure DDemod Symbol Rate, Sample Per Symbol, Number of Symbols
//9. Configure DDemod PSK Format and EVM Norm Reference
//10. Configure DDemod FSK Deviation


using System;
using NationalInstruments;
using NationalInstruments.RFmx.DemodMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxDemodDDemodAdvanced
{
    public class RFmxDemodDDemodAdvanced
    {
        RFmxInstrMX instrSession;
        RFmxDemodMX demod;

        double meanCarrierFrequencyError, meanFrequencyDrift, meanCarrierPhaseError;
        double meanRmsEvm, meanPeakEvm, meanRmsOffsetEvm, meanPeakOffsetEvm, meanMer, maxRmsEvm, maxPeakEvm,
            maxRmsOffsetEvm, maxPeakOffsetEvm;
        double meanFskDeviation, meanRmsFskError, maxPeakFskError, timeout = 10.0;

        bool syncFound;
        double meanMagnitudeError, maxMagnitudeError, meanPhaseError, maxPhaseError, meanIQOriginOffset,
            meanIQGainImbalance, meanQuadratureSkew, meanRhoFactor, meanAmplitudeDroop;


        void CreateRFmxSession()
        {
            string resourceName = "RFSA";
            if (instrSession == null)
            {
                instrSession = new RFmxInstrMX(resourceName, "");
                demod = instrSession.GetDemodSignalConfiguration();
            }
        }

        void ConfigureDemodSignal()
        {
            float[] pluseShappingFilterCustomCoefficientY = null;
            float[] measurementFilterCustomCoefficientY = null;
            ComplexSingle[] equalizerInitialCoefficientY = null;
            sbyte[] syncBits = null;

            string selectedPorts = "";
            double centerFrequency = 1e+9;
            double referenceLevel = 0.00;
            double externalAttenuation = 0.00;

            double frequency = 10.0e+6;
            string frequencySource = RFmxInstrMXConstants.OnboardClock;

            bool iqPowerEdgeEnabled = false;
            double iqPowerEdgeLevel = -20.00;
            double triggerDelay = 0.00;
            double minQuietTime = 0.00;

            int samplesPerSymbol = -1;
            double fskDeviation = 15.000e+3;
            double apskR2toR1Ratio = 2.84;
            double apskR3toR1Ratio = 5.27;
            double symbolRate = 100.000e+3;
            int numOfSymbols = 1000;

            int measurementOffset = 0;

            //Averaging 
            int averagingCount = 10;

            /* Signal Structure*/
            RFmxDemodMXDDemodSignalStructure signalStructure = RFmxDemodMXDDemodSignalStructure.Continuous;

            /* Burst Start Exclusion Symbols */
            int burstStartExclusionSymbols = 0;
            /* Burst End Exclusion Symbols */
            int burstEndExclusionSymbols = 0;

            double pulseShappingFilterAlphaOrBT = 0.50;
            double pluseShappingFilterCustomCoefficientX0 = 0;
            double pluseShappingFilterCustomCoefficientDx = 1.00e+0;

            double measurementFilterCustomCoefficientX0 = 0.00e+0;
            double measurementFilterCustomCoefficientDx = 1.00e+0;

            int equalizerLength = 20;
            int equalizerTrainingCount = 10;
            double equalizerConvergenceFactor = 0.01e+0;

            instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
            demod.SetSelectedPorts("", selectedPorts);
            demod.ConfigureFrequency("", centerFrequency);
            demod.ConfigureReferenceLevel("", referenceLevel);
            demod.ConfigureExternalAttenuation("", externalAttenuation);

            demod.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeLevel,
                RFmxDemodMXIQPowerEdgeTriggerSlope.Rising, triggerDelay, RFmxDemodMXTriggerMinimumQuietTimeMode.Manual,
                minQuietTime, iqPowerEdgeEnabled);

            demod.SelectMeasurements("", RFmxDemodMXMeasurementTypes.DDemod, true);
            demod.DDemod.Configuration.ConfigureModulationType("", RFmxDemodMXDDemodModulationType.Psk,
                RFmxDemodMXDDemodM.M4, RFmxDemodMXDDemodDifferentialEnabled.False);
            demod.DDemod.Configuration.ConfigureSymbolRate("", symbolRate);
            demod.DDemod.Configuration.ConfigureSamplesPerSymbol("", samplesPerSymbol);
            demod.DDemod.Configuration.ConfigureNumberOfSymbols("", numOfSymbols);

            demod.DDemod.Configuration.ConfigurePskFormat("", RFmxDemodMXDDemodPskFormat.Normal);
            demod.DDemod.Configuration.ConfigureEvmNormalizationReference("", RFmxDemodMXDDemodEvmNormalizationReference.Peak);
            demod.DDemod.Configuration.ConfigureFskDeviation("", fskDeviation, RFmxDemodMXDDemodFskReferenceCompensationEnabled.False);
            demod.DDemod.Configuration.SetApskR2ToR1Ratio("", apskR2toR1Ratio);
            demod.DDemod.Configuration.SetApskR3ToR1Ratio("", apskR3toR1Ratio);
            demod.DDemod.Configuration.ConfigurePulseShapingFilter("", RFmxDemodMXDDemodPulseShapingFilterType.RootRaisedCosine,
                pulseShappingFilterAlphaOrBT, pluseShappingFilterCustomCoefficientX0, pluseShappingFilterCustomCoefficientDx,
                pluseShappingFilterCustomCoefficientY);
            demod.DDemod.Configuration.ConfigureMeasurementFilter("", RFmxDemodMXDDemodMeasurementFilterType.Auto,
                measurementFilterCustomCoefficientX0, measurementFilterCustomCoefficientDx, measurementFilterCustomCoefficientY);
            demod.DDemod.Configuration.ConfigureEqualizer("", RFmxDemodMXDDemodEqualizerMode.Off, equalizerLength, 0.0, 1.0,
                equalizerInitialCoefficientY, equalizerTrainingCount, equalizerConvergenceFactor);
            demod.DDemod.Configuration.ConfigureSynchronization("", RFmxDemodMXDDemodSynchronizationEnabled.False,
                syncBits, measurementOffset);
            demod.DDemod.Configuration.ConfigureAveraging("", RFmxDemodMXDDemodAveragingEnabled.False, averagingCount);
            demod.DDemod.Configuration.ConfigureSignalStructure("", signalStructure);
            demod.DDemod.Configuration.SetBurstStartExclusionSymbols("", burstStartExclusionSymbols);
            demod.DDemod.Configuration.SetBurstEndExclusionSymbols("", burstEndExclusionSymbols);
            demod.Initiate("", "");
        }

        void RetrieveResults()
        {
            ComplexSingle[] constellationTrace = null;
            AnalogWaveform<float> evmTrace = null;
            AnalogWaveform<float> offsetEvmTrace = null;

            demod.DDemod.Results.FetchCarrierMeasurement("", timeout, out meanCarrierFrequencyError, out meanFrequencyDrift,
                out meanCarrierPhaseError);
            demod.DDemod.Results.FetchEvm("", timeout, out meanRmsEvm, out maxRmsEvm, out meanMer, out maxPeakEvm, out meanPeakEvm);
            demod.DDemod.Results.FetchOffsetEvm("", timeout, out meanRmsOffsetEvm, out maxRmsOffsetEvm,
                out maxPeakOffsetEvm, out meanPeakOffsetEvm);
            demod.DDemod.Results.FetchMagnitudeError("", timeout, out meanMagnitudeError, out maxMagnitudeError);
            demod.DDemod.Results.FetchPhaseError("", timeout, out meanPhaseError, out maxPhaseError);
            demod.DDemod.Results.FetchFskResults("", timeout, out meanFskDeviation, out meanRmsFskError,
                out maxPeakFskError);
            demod.DDemod.Results.FetchIQImpairments("", timeout, out meanIQGainImbalance, out meanQuadratureSkew,
                out meanIQOriginOffset);
            demod.DDemod.Results.FetchSyncFound("", timeout, out syncFound);
            demod.DDemod.Results.FetchMeanRhoFactor("", timeout, out meanRhoFactor);
            demod.DDemod.Results.FetchMeanAmplitudeDroop("", timeout, out meanAmplitudeDroop);
            demod.DDemod.Results.FetchConstellationTrace("", timeout, ref constellationTrace);
            demod.DDemod.Results.FetchEvmTrace("", timeout, ref evmTrace);
            demod.DDemod.Results.FetchOffsetEvmTrace("", timeout, ref offsetEvmTrace);


            Console.WriteLine("-------------------Carrier measurements----------\n");
            Console.WriteLine("Mean Carrier Frequency Error(Hz)        " + meanCarrierFrequencyError);
            Console.WriteLine("Mean Frequency Drift (Hz)               " + meanFrequencyDrift);
            Console.WriteLine("Mean Phase Error (deg)                  " + meanCarrierPhaseError);

            Console.WriteLine("\n---------------------------EVM-----------------\n");
            Console.WriteLine("Mean MER (dB)                           " + meanMer);
            Console.WriteLine("Mean RMS EVM (%)                        " + meanRmsEvm);
            Console.WriteLine("Maximum RMS EVM (%)                     " + maxRmsEvm);
            Console.WriteLine("Mean Peak EVM (%)                       " + meanPeakEvm);
            Console.WriteLine("Maximum Peak EVM (%)                    " + maxPeakEvm);
            Console.WriteLine("Mean RMS Offset EVM (%)                 " + meanRmsOffsetEvm);
            Console.WriteLine("Maximum RMS Offset EVM (%)              " + maxRmsOffsetEvm);
            Console.WriteLine("Mean Peak Offset EVM (%)                " + meanPeakOffsetEvm);
            Console.WriteLine("Maximum Peak Offset EVM (%)             " + maxPeakOffsetEvm);

            Console.WriteLine("\n--------------------------FSK Results--------------\n");
            Console.WriteLine("Mean Deviation (Hz)                     " + meanFskDeviation);
            Console.WriteLine("Mean RMS FSK Error (Hz)                 " + meanRmsFskError);
            Console.WriteLine("Maximum Peak FSK Error (%)              " + maxPeakFskError);

            Console.WriteLine("\n--------------------------Measurements------------\n");
            if (syncFound)
            {
                Console.WriteLine("Sync Found is True\n");
            }
            else
            {
                Console.WriteLine("Sync Found is False\n");
            }

            Console.WriteLine("Mean Magnitude Error (%)                " + meanMagnitudeError);
            Console.WriteLine("Maximum Magnitude Error (%)             " + maxMagnitudeError);
            Console.WriteLine("Mean Phase Error (deg)                  " + meanPhaseError);
            Console.WriteLine("Maximum Phase Error (deg)               " + maxPhaseError);
            Console.WriteLine("Mean IQ Origin Offset (dB)              " + meanIQOriginOffset);
            Console.WriteLine("Mean IQ Gain Imbalance (dB)             " + meanIQGainImbalance);
            Console.WriteLine("Mean Quadrature Skew (deg)              " + meanQuadratureSkew);
            Console.WriteLine("Mean Rho Factor                         " + meanRhoFactor);
            Console.WriteLine("Mean Amplitude Droop (dB/Symbol)        " + meanAmplitudeDroop);
                                                                     
        }

        void CloseSession()
        {
            if (demod != null)
            {
                demod.Dispose();
                demod = null;
            }
            if (instrSession != null)
            {
                instrSession.Close();
                instrSession = null;
            }
        }

        public void Run()
        {
            try
            {
                CreateRFmxSession();
                ConfigureDemodSignal();
                RetrieveResults();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                CloseSession();
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }


    }
}
