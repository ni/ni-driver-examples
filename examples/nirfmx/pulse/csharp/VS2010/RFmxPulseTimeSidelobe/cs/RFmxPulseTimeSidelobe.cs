/*Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic RF signal properties (Center Frequency, RF Attenuation and External Attenuation)
4. Enabling Pulse and Traces Result
5. Configure Trigger Type and Trigger Parameters.
6. Configure Reference Waveform.
7. Configure Acquisition Settings.
8. Configure Pulse Detection Settings.
9. Configure State and Thershold Level Settings.
10. Configure Time Sidelobe Settings.
11. Configure Selected Traces Settings, Pulse Metrics and Pulse Stability enabled settings.
12. Initiate the Measurement.
13. Wait for Measurement to complete.
14  Fetch  Pulse Count, Time Sidelobe, Statistical Time Sidelobe results and Time Sidelobe Trace.
15. Close RFmx Session.*/

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.PulseMX;
using NationalInstruments.ModularInstruments.NIRfsgPlayback;

namespace NationalInstruments.Examples.RFmxPulseTimeSidelobe
{
    public class RFmxPulseTimeSidelobe
    {
        RFmxInstrMX instrSession;
        RFmxPulseMX Pulse;
        string resourceName;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        bool iqPowerEdgeEnabled;
        double iqPowerEdgeLevel;
        double triggerDelay;
        RFmxPulseMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        double minimumQuietTime;

        double measurementBandwidth;
        RFmxPulseMXMeasurementFilterType measurementFilterType;
        double acquisitionLength;
        RFmxPulseMXMaximumPulseCountEnabled maximumPulseCountEnabled;
        int maximumPulseCount;

        RFmxPulseMXPulseDetectionReference pulseDetectionReference;
        double pulseDetectionThreshold;
        double pulseDetectionHysteresis;
        double pulseDetectionMinimumPulseOffDuration;

        RFmxPulseMXPulseLevelComputationMethod pulseLevelComputationMethod;
        RFmxPulseMXPulseDroopCompensationEnabled pulseDroopCompensationEnabled;

        RFmxPulseMXPulseTimeSidelobeReferenceWindowType timeSidelobeReferenceWindowType;
        RFmxPulseMXPulseTimeSidelobeKeepOutTimeAuto timeSidelobeKeepOutTimeAuto;
        double timeSidelobeKeepOutTime;                                           /* (s) */
        double timeSidelobeMinimumCorrelation;
        double timeout;                                                          /* (s) */

        int pulseSelectedPulseTrace;

        string referenceWaveformFilePath;
        ComplexWaveform<ComplexSingle> referenceWaveform;

        /* Time Sidelobe Results */
        int pulseCount;
        double[] pulseResultsMainlobeWidth;                                      /* (s) */
        double[] pulseResultsSidelobeDelay;                                      /* (s) */
        double[] pulseResultsPeakSidelobeLevel;                                 /* (dB) */
        double[] pulseResultsCompressionRatio;                                   /* (%) */
        double[] pulseResultsPeakCorrelation;

        /*Statistical Time Sidelobe Results*/
        double pulseResultsMainlobeWidthMean;                                    /* (s) */
        double pulseResultsMainlobeWidthMaximum;                                 /* (s) */
        double pulseResultsMainlobeWidthMinimum;                                 /* (s) */
        double pulseResultsMainlobeWidthStandardDeviation;                       /* (s) */
        double pulseResultSidelobesDelayMean;                                    /* (s) */
        double pulseResultsSidelobeDelayMaximum;                                 /* (s) */
        double pulseResultsSidelobeDelayMinimum;                                 /* (s) */
        double pulseResultsSidelobeDelayStandardDeviation;                       /* (s) */
        double pulseResultsPeakSidelobeLevelMean;                                /* (dB) */
        double pulseResultsPeakSidelobeLevelMaximum;                             /* (dB) */
        double pulseResultsPeakSidelobeLevelMinimum;                             /* (dB) */
        double pulseResultsPeakSidelobeLevelStandardDeviation;                   /* (dB) */
        double pulseResultsSidelobeCompressionRatioMean;                         /* (%) */
        double pulseResultsSidelobeCompressionRatioMaximum;                      /* (%) */
        double pulseResultsSidelobeCompressionRatioMinimum;                      /* (%) */
        double pulseResultsSidelobeCompressionRatioStandardDeviation;            /* (%) */
        double pulseResultsSidelobePeakCorrelationMean;                          /* (dB) */
        double pulseResultsSidelobePeakCorrelationMaximum;
        double pulseResultsSidelobePeakCorrelationMinimum;
        double pulseResultsSidelobePeakCorrelationStandardDeviation;
        AnalogWaveform<float> pulseTimeSidelobeTrace;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigurePulse();
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
            resourceName = "RFSA";

            centerFrequency = 1.0e9;                                          /* (Hz) */
            referenceLevel = -10.0;                                           /* (dBm) */
            externalAttenuation = 0.0;                                        /* (dB) */

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e6;                             /* (Hz) */

            iqPowerEdgeEnabled = true;
            iqPowerEdgeLevel = -20.0;                                         /* (dBm) */
            triggerDelay = 0.0;                                               /* (s) */
            minimumQuietTimeMode = RFmxPulseMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 5.0e-6;                                        /* (s) */

            measurementBandwidth = 80.0e6;                                    /* (Hz) */
            measurementFilterType = RFmxPulseMXMeasurementFilterType.Rectangular;
            acquisitionLength = 1.0e-3;                                       /* (s) */
            maximumPulseCountEnabled = RFmxPulseMXMaximumPulseCountEnabled.False;
            maximumPulseCount = 100;

            pulseDetectionReference = RFmxPulseMXPulseDetectionReference.ReferenceLevel;
            pulseDetectionThreshold = -20.0;
            pulseDetectionHysteresis = 1.0;                                   /* (dB) */
            pulseDetectionMinimumPulseOffDuration = 50.0e-9;                  /* (s) */

            pulseLevelComputationMethod = RFmxPulseMXPulseLevelComputationMethod.Median;
            pulseDroopCompensationEnabled = RFmxPulseMXPulseDroopCompensationEnabled.True;

            timeSidelobeReferenceWindowType = RFmxPulseMXPulseTimeSidelobeReferenceWindowType.None;
            timeSidelobeKeepOutTimeAuto = RFmxPulseMXPulseTimeSidelobeKeepOutTimeAuto.True;
            timeSidelobeKeepOutTime = 1.0e-6;                                /* (s) */
            timeSidelobeMinimumCorrelation = 0.5;

            referenceWaveformFilePath = "Pulse_FMChirpUp-10MHz_BW-80MHz_Rect-filter.tdms";
            referenceWaveform = null;

            pulseSelectedPulseTrace = 0;

            timeout = 10.0;                                                /* (s) */
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigurePulse()
        {
            Pulse = instrSession.GetPulseSignalConfiguration();                /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            Pulse.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            Pulse.SelectMeasurements("", RFmxPulseMXMeasurementTypes.Pulse, true);
            Pulse.ConfigureIQPowerEdgeTrigger("", "0", RFmxPulseMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
               triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxPulseMXIQPowerEdgeTriggerLevelType.Relative,
               iqPowerEdgeEnabled);
            NIRfsgPlayback.ReadWaveformFromFileComplex(referenceWaveformFilePath, ref referenceWaveform);

            Pulse.Pulse.Configuration.Configure1ReferenceWaveform("", referenceWaveform);

            Pulse.SetMeasurementBandwidth("", measurementBandwidth);
            Pulse.SetMeasurementFilterType("", measurementFilterType);
            Pulse.SetAcquisitionLength("", acquisitionLength);
            Pulse.SetMaximumPulseCountEnabled("", maximumPulseCountEnabled);
            Pulse.SetMaximumPulseCount("", maximumPulseCount);

            Pulse.Pulse.Configuration.SetDetectionReference("", pulseDetectionReference);
            Pulse.Pulse.Configuration.SetDetectionThreshold("", pulseDetectionThreshold);
            Pulse.Pulse.Configuration.SetDetectionHysteresis("", pulseDetectionHysteresis);
            Pulse.Pulse.Configuration.SetDetectionMinimumOffDuration("", pulseDetectionMinimumPulseOffDuration);

            Pulse.Pulse.Configuration.SetLevelComputationMethod("", pulseLevelComputationMethod);
            Pulse.Pulse.Configuration.SetDroopCompensationEnabled("", pulseDroopCompensationEnabled);
            Pulse.Pulse.Configuration.SetTimeSidelobeEnabled("", RFmxPulseMXPulseTimeSidelobeEnabled.True);
            Pulse.Pulse.Configuration.SetTimeSidelobeReferenceWindowType("", timeSidelobeReferenceWindowType);
            Pulse.Pulse.Configuration.SetTimeSidelobeKeepOutTimeAuto("", timeSidelobeKeepOutTimeAuto);
            Pulse.Pulse.Configuration.SetTimeSidelobeKeepOutTime("", timeSidelobeKeepOutTime);
            Pulse.Pulse.Configuration.SetTimeSidelobeMinimumCorrelation("", timeSidelobeMinimumCorrelation);
            Pulse.Pulse.Configuration.SetMetricsEnabled("", RFmxPulseMXPulseMetricsEnabled.False);
            Pulse.Pulse.Configuration.SetStabilityEnabled("", RFmxPulseMXPulseStabilityEnabled.False);
            Pulse.Pulse.Configuration.SetSelectedPulseTrace("", pulseSelectedPulseTrace);

            Pulse.Initiate("", "");

            Pulse.WaitForMeasurementComplete("", timeout);
        }

        void RetrieveResults()
        {
            Pulse.Pulse.Results.GetPulseCount("", out pulseCount);
            Pulse.Pulse.Results.GetTimeSidelobeMainlobeWidth("", ref pulseResultsMainlobeWidth);
            Pulse.Pulse.Results.GetTimeSidelobeDelay("", ref pulseResultsSidelobeDelay);
            Pulse.Pulse.Results.GetTimeSidelobePeakSidelobeLevel("", ref pulseResultsPeakSidelobeLevel);
            Pulse.Pulse.Results.GetTimeSidelobeCompressionRatio("", ref pulseResultsCompressionRatio);
            Pulse.Pulse.Results.GetTimeSidelobePeakCorrelation("", ref pulseResultsPeakCorrelation);
            Pulse.Pulse.Results.GetTimeSidelobeMainlobeWidthMean("", out pulseResultsMainlobeWidthMean);
            Pulse.Pulse.Results.GetTimeSidelobeMainlobeWidthMaximum("", out pulseResultsMainlobeWidthMaximum);
            Pulse.Pulse.Results.GetTimeSidelobeMainlobeWidthMinimum("", out pulseResultsMainlobeWidthMinimum);
            Pulse.Pulse.Results.GetTimeSidelobeMainlobeWidthStandardDeviation("", out pulseResultsMainlobeWidthStandardDeviation);
            Pulse.Pulse.Results.GetTimeSidelobeDelayMean("", out pulseResultSidelobesDelayMean);
            Pulse.Pulse.Results.GetTimeSidelobeDelayMaximum("", out pulseResultsSidelobeDelayMaximum);
            Pulse.Pulse.Results.GetTimeSidelobeDelayMinimum("", out pulseResultsSidelobeDelayMinimum);
            Pulse.Pulse.Results.GetTimeSidelobeDelayStandardDeviation("", out pulseResultsSidelobeDelayStandardDeviation);
            Pulse.Pulse.Results.GetTimeSidelobePeakSidelobeLevelMean("", out pulseResultsPeakSidelobeLevelMean);
            Pulse.Pulse.Results.GetTimeSidelobePeakSidelobeLevelMaximum("", out pulseResultsPeakSidelobeLevelMaximum);
            Pulse.Pulse.Results.GetTimeSidelobePeakSidelobeLevelMinimum("", out pulseResultsPeakSidelobeLevelMinimum);
            Pulse.Pulse.Results.GetTimeSidelobePeakSidelobeLevelStandardDeviation("", out pulseResultsPeakSidelobeLevelStandardDeviation);
            Pulse.Pulse.Results.GetTimeSidelobeCompressionRatioMean("", out pulseResultsSidelobeCompressionRatioMean);
            Pulse.Pulse.Results.GetTimeSidelobeCompressionRatioMaximum("", out pulseResultsSidelobeCompressionRatioMaximum);
            Pulse.Pulse.Results.GetTimeSidelobeCompressionRatioMinimum("", out pulseResultsSidelobeCompressionRatioMinimum);
            Pulse.Pulse.Results.GetTimeSidelobeCompressionRatioStandardDeviation("", out pulseResultsSidelobeCompressionRatioStandardDeviation);
            Pulse.Pulse.Results.GetTimeSidelobePeakCorrelationMean("", out pulseResultsSidelobePeakCorrelationMean);
            Pulse.Pulse.Results.GetTimeSidelobePeakCorrelationMaximum("", out pulseResultsSidelobePeakCorrelationMaximum);
            Pulse.Pulse.Results.GetTimeSidelobePeakCorrelationMinimum("", out pulseResultsSidelobePeakCorrelationMinimum);
            Pulse.Pulse.Results.GetTimeSidelobePeakCorrelationStandardDeviation("", out pulseResultsSidelobePeakCorrelationStandardDeviation);
            Pulse.Pulse.Results.FetchTimeSidelobeTrace("", timeout, ref pulseTimeSidelobeTrace);

        }

        void PrintResults()
        {
            Console.WriteLine("\nPulse Count                            : {0}\n", pulseCount);
            Console.WriteLine("---------------------Time Sidelobe Results------------------------------------------");
            for (int i = 0; i < pulseCount; i++)
            {

                Console.WriteLine("Index                                      : {0}", i);
                Console.WriteLine("Mainlobe Width (s)                         : {0:F11}", pulseResultsMainlobeWidth[i]);
                Console.WriteLine("Sidelobe Delay (s)                         : {0:F11}", pulseResultsSidelobeDelay[i]);
                Console.WriteLine("Peak Sidelobe Level (dB)                   : {0:F9}", pulseResultsPeakSidelobeLevel[i]);
                Console.WriteLine("Compression Ratio (%)                      : {0:F9}", pulseResultsCompressionRatio[i]);
                Console.WriteLine("Peak Correlation                           : {0:F9}", pulseResultsPeakCorrelation[i]);
                Console.WriteLine("-----------------------------------------------------------------------------------\n");
            }
            Console.WriteLine("--------------------Statistical Time Sidelobe Results-------------------------------");
            Console.WriteLine("Mainlobe Width Mean (s)                        : {0}", pulseResultsMainlobeWidthMean);
            Console.WriteLine("Mainlobe Width Max (s)                         : {0}", pulseResultsMainlobeWidthMaximum);
            Console.WriteLine("Mainlobe Width Min (s)                         : {0}", pulseResultsMainlobeWidthMinimum);
            Console.WriteLine("Mainlobe Width SD (s)                          : {0}", pulseResultsMainlobeWidthStandardDeviation);
            Console.WriteLine("Sidelobe Delay Mean (s)                        : {0}", pulseResultSidelobesDelayMean);
            Console.WriteLine("Sidelobe Delay Max (s)                         : {0}", pulseResultsSidelobeDelayMaximum);
            Console.WriteLine("Sidelobe Delay Min (s)                         : {0}", pulseResultsSidelobeDelayMinimum);
            Console.WriteLine("Sidelobe Delay SD (s)                          : {0}", pulseResultsSidelobeDelayStandardDeviation);
            Console.WriteLine("Peak Sidelobe Level Mean (dB)                  : {0}", pulseResultsPeakSidelobeLevelMean);
            Console.WriteLine("Peak Sidelobe Level Max (dB)                   : {0}", pulseResultsPeakSidelobeLevelMaximum);
            Console.WriteLine("Peak Sidelobe Level Min (dB)                   : {0}", pulseResultsPeakSidelobeLevelMinimum);
            Console.WriteLine("Peak Sidelobe Level SD (dB)                    : {0}", pulseResultsPeakSidelobeLevelStandardDeviation);
            Console.WriteLine("Compression Ratio Mean (%)                     : {0}", pulseResultsSidelobeCompressionRatioMean);
            Console.WriteLine("Compression Ratio Max (%)                      : {0}", pulseResultsSidelobeCompressionRatioMaximum);
            Console.WriteLine("Compression Ratio Min (%)                      : {0}", pulseResultsSidelobeCompressionRatioMinimum);
            Console.WriteLine("Compression Ratio SD (%)                       : {0}", pulseResultsSidelobeCompressionRatioStandardDeviation);
            Console.WriteLine("Peak Correlation Mean                          : {0}", pulseResultsSidelobePeakCorrelationMean);
            Console.WriteLine("Peak Correlation Max                           : {0}", pulseResultsSidelobePeakCorrelationMaximum);
            Console.WriteLine("Peak Correlation Min                           : {0}", pulseResultsSidelobePeakCorrelationMinimum);
            Console.WriteLine("Peak Correlation SD                            : {0}", pulseResultsSidelobePeakCorrelationStandardDeviation);
            Console.WriteLine("------------------------------------------------------------------------------------\n");

        }

        void CloseSession()
        {
            if (Pulse != null)
            {
                Pulse.Dispose();
                Pulse = null;
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


