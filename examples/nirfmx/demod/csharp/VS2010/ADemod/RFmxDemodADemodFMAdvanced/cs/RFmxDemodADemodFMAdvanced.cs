//Steps:
//1. Open a new RFmxInstrMX session and create a Demod Signal
//2. Configure the basic instrument properties (Clock Source, Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select ADemod Measurement and enable  the traces 
//6. Configure FM Modulation
//7. Configure ADemod RBW Filter, Measurement Interval, and Carrier Correction
//8. Configure ADemod FM DeEmphasis, Audio Filter and Averaging
//9. Initiate Measurement


using System;
using NationalInstruments;
using NationalInstruments.RFmx.DemodMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxDemodADemodFMAdvanced
{
    public class RFmxDemodADemodFMAdvanced
    {
        RFmxInstrMX instrSession;
        RFmxDemodMX demod;

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
            string frequencySource;
            string selectedPorts;
            double centerFrequency, referenceLevel, externalAttenuation, frequency;
            double measurementInterval, deEmphasis, rbw, rbwRrcAlpha;
            double audioFilterLowerCutoff, audioFilterUpperCutoff;
            int averagingCount;

            selectedPorts = "";
            centerFrequency = 1e+9;
            referenceLevel = 0.00;
            externalAttenuation = 0.00;

            frequencySource = RFmxInstrMXConstants.OnboardClock;
            frequency = 10.0e+6;

            measurementInterval = 10.00e-3;
            deEmphasis = 0.0;

            rbw = 100.00e+3;
            rbwRrcAlpha = 0.100;

            audioFilterLowerCutoff = 100.000;
            audioFilterUpperCutoff = 10.000e+3;

            //Averaging 
            averagingCount = 10;

            instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
            demod.SetSelectedPorts("", selectedPorts);
            demod.ConfigureFrequency("", centerFrequency);
            demod.ConfigureReferenceLevel("", referenceLevel);
            demod.ConfigureExternalAttenuation("", externalAttenuation);
            demod.SelectMeasurements("", RFmxDemodMXMeasurementTypes.ADemod, true);
            demod.ADemod.Configuration.SetAudioMeasurementEnabled("", RFmxDemodMXADemodAudioMeasurementEnabled.True);
            demod.ADemod.Configuration.ConfigureModulationType("", RFmxDemodMXADemodModulationType.FM);
            demod.ADemod.Configuration.ConfigureRbwFilter("", rbw, RFmxDemodMXADemodRbwFilterType.Flat, rbwRrcAlpha);
            demod.ADemod.Configuration.ConfigureMeasurementInterval("", measurementInterval);
            demod.ADemod.Configuration.ConfigureCarrierCorrection("", RFmxDemodMXADemodCarrierFrequencyCorrectionEnabled.True,
                RFmxDemodMXADemodCarrierPhaseCorrectionEnabled.True);
            demod.ADemod.Configuration.ConfigureFMDeEmphasis("", deEmphasis);
            demod.ADemod.Configuration.ConfigureAudioFilter("", RFmxDemodMXADemodAudioFilterType.None, audioFilterLowerCutoff,
                audioFilterUpperCutoff);
            demod.ADemod.Configuration.ConfigureAveraging("", RFmxDemodMXADemodAveragingEnabled.False, averagingCount,
                RFmxDemodMXADemodAveragingType.Linear);

            demod.Initiate("", "");
        }

        void RetrieveResults()
        {
            double meanCarrierPower, averageSinad, averageThdWithNoise, meanCarrierFrequencyError, averageSnr,
                averageThd, meanModulationFrequency;
            double meanDeviation, meanHalfPeaktoPeak, meanPositivePeak, meanNegativePeak, meanRms;
            double maxDeviation, maxPeaktoPeak, maxRms, maxPositivePeak, maxNegativePeak;
            double timeout = 10.0;
            Spectrum<float> demodSpectrumTrace = null;
            AnalogWaveform<float> demodSignalTrace = null;
            demod.ADemod.Results.FetchDistortions("", timeout, out averageSinad, out averageSnr, out averageThd,
                out averageThdWithNoise);
            demod.ADemod.Results.FetchMeanModulationFrequency("", timeout, out meanModulationFrequency);
            demod.ADemod.Results.FetchCarrierMeasurement("", timeout, out meanCarrierFrequencyError, out meanCarrierPower);
            demod.ADemod.Results.FetchFMMeanDeviation("", timeout, out meanDeviation, out meanHalfPeaktoPeak,
                out meanRms, out meanPositivePeak, out meanNegativePeak);
            demod.ADemod.Results.FetchFMMaximumDeviation("", timeout, out maxDeviation, out maxPeaktoPeak,
                out maxRms, out maxPositivePeak, out maxNegativePeak);
            demod.ADemod.Results.FetchDemodSpectrumTrace("", timeout, ref demodSpectrumTrace);
            demod.ADemod.Results.FetchDemodSignalTrace("", timeout, ref demodSignalTrace);


            Console.WriteLine("------------------------------------------------------\n");
            Console.WriteLine("Mean Carrier Power (dBm)         " + meanCarrierPower );
            Console.WriteLine("Average SINAD (dB)               " + averageSinad );
            Console.WriteLine("Average THD with Noise (%)       " + averageThdWithNoise);
            Console.WriteLine("Mean Carrier Frequency Error(Hz) " + meanCarrierFrequencyError);
            Console.WriteLine("Average SNR (dB)                 " + averageSnr);
            Console.WriteLine("Average THD (%)                  " + averageThd);
            Console.WriteLine("Mean Modulation Frequency (Hz)   " + meanModulationFrequency);


            Console.WriteLine("\n-------------------FM Deviations---------------------\n");
            Console.WriteLine("Mean Deviation (Hz)            " + meanDeviation);
            Console.WriteLine("Maximum Deviation (Hz)         " + maxDeviation);
            Console.WriteLine("Mean Peak to Peak/2 (Hz)       " + meanHalfPeaktoPeak);
            Console.WriteLine("Maximum Peak to Peak/2 (Hz)    " + maxPeaktoPeak);
            Console.WriteLine("Mean Positive Peak (Hz)        " + meanPositivePeak);
            Console.WriteLine("Maximum Positive Peak (Hz)     " + maxPositivePeak);
            Console.WriteLine("Mean Negative peak (Hz)        " + meanNegativePeak);
            Console.WriteLine("Maximum Negative peak (Hz)     " + maxNegativePeak);
            Console.WriteLine("Mean RMS (Hz)                  " + meanRms);
            Console.WriteLine("Maximum RMS (Hz)               " + maxRms);
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
