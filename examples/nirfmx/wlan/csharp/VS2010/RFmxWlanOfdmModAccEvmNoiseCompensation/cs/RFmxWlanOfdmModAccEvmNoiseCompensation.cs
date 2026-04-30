//Instructions:
//1. This example demonstrates the use of RFmxWLAN OFDM ModAcc measurement to compute EVM after compensating for the noise attributed to the VSA.
//2. The example uses an enum control, "Calibrate Noise Floor" with two values :
//Disabled(0) : select this to skip calibrating the VSA noise floor, and perform the OFDMModAcc measurement directly.You may want to do this when the VSA noise floor has already been calibrated or when Noise Compensation is disabled.
//Enabled(1) : select this to first calibrate the VSA noise floor, and then perform the OFDMModAcc measurement.

//Follow these steps to calibrate VSA noise, and then perform ModAcc measurement :
//1. Set Calibrate Noise Floor to "Enabled".
//2. Run the example.
//3. When "Turn OFF Generation" dialog box appears, ensure that signal generation is turned OFF and then click "OK". Wait for calibration to complete.
//4. When "Turn ON Generation" dialog box appears, ensure that signal generation is turned ON and then click "OK".

//Follow these steps to skip(re)calibrating and directly perform ModAcc measurement :
//1. Set Calibrate Noise Floor to "Disabled".
//2. Run the example

//Steps:
//1. Open a new RFmx session.
//2. Configure the frequency reference properties(Clock Source and Clock Frequency).
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//5. Configure Standard and Channel Bandwidth.
//6. Select OFDMModAcc measurement and enable the traces.
//7. Configure Optimize Dynamic Range for EVM.
//8. Configure Measurement Mode as Calibrate Noise Floor.
//9. Initiate Measurement.
//10. Wait for Measurement Complete.
//11. Configure Measurement Mode as Measure.
//12. Configure Measurement Interval.
//13. Configure Averaging parameters.
//14. Configure Noise Compensation Enabled.
//15. Initiate Measurement.
//16. Fetch OFDMModAcc Measurements.
//17. Close the RFmx Session

using System;
using System.Windows.Forms;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WlanMX;

namespace NationalInstruments.Examples.RFmxWlanOfdmModAccEvmNoiseCompensation
{
    public class RFmxWlanOfdmModAccEvmNoiseCompensation
    {
        RFmxInstrMX instrSession;
        RFmxWlanMX wlan;
        string resourceName;

        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        bool iqPowerEdgeEnabled;
        double iqPowerEdgeLevel;
        double triggerDelay;
        RFmxWlanMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        double minimumQuietTime;

        RFmxWlanMXStandard standard;

        double channelBandwidth;

        RFmxWlanMXOfdmModAccAveragingEnabled averagingEnabled;
        int averagingCount;

        RFmxWlanMXOfdmModAccNoiseCompensationEnabled noiseCompensationEnabled;
        RFmxWlanMXOfdmModAccOptimizeDynamicRangeForEvmEnabled optimizeDynamicRangeForEvmEnabled;
        double optimizeDynamicRangeForEvmMargin;
        bool calibrateNoiseFloor;

        int measurementOffset;
        int maximumMeasurementLength;

        double timeout;

        double compositeRmsEvmMean;
        double compositeDataRmsEvmMean;
        double compositePilotRmsEvmMean;

        ComplexSingle[] pilotConstellation;
        ComplexSingle[] dataConstellation;
        AnalogWaveform<float> chainRmsEvmPerSubcarrierMean;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureWlan();
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

            centerFrequency = 2.412e9;                                              /* (Hz) */
            referenceLevel = 0.0;                                                   /* (dBm) */
            externalAttenuation = 0.0;                                              /* (dB) */

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e6;                                     /* (Hz) */

            iqPowerEdgeEnabled = true;
            iqPowerEdgeLevel = -20.0;                                               /*(dB) */
            triggerDelay = 0.0;                                                     /* (s) */
            minimumQuietTimeMode = RFmxWlanMXTriggerMinimumQuietTimeMode.Auto;
            minimumQuietTime = 5.0e-6;                                              /* (s) */

            standard = RFmxWlanMXStandard.Standard802_11ag;

            channelBandwidth = 20e6;                                                /*(Hz) */

            averagingEnabled = RFmxWlanMXOfdmModAccAveragingEnabled.False;
            averagingCount = 10;

            noiseCompensationEnabled = RFmxWlanMXOfdmModAccNoiseCompensationEnabled.True;
            optimizeDynamicRangeForEvmEnabled = RFmxWlanMXOfdmModAccOptimizeDynamicRangeForEvmEnabled.True;
            optimizeDynamicRangeForEvmMargin = 0.0;                               /* (dB) */
            calibrateNoiseFloor = true; //true - Enabled, false - Disabled.

            measurementOffset = 0;                                                  /* (symbols) */
            maximumMeasurementLength = 16;                                          /* (symbols) */

            timeout = 10.0;                                                         /* (s) */

            compositeRmsEvmMean = 0.0;                                              /* (dB) */
            compositeDataRmsEvmMean = 0.0;                                          /* (dB) */
            compositePilotRmsEvmMean = 0.0;                                         /* (dB) */
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureWlan()
        {
            wlan = instrSession.GetWlanSignalConfiguration();     /* Create a new RFmx Session. */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            wlan.ConfigureFrequency("", centerFrequency);
            wlan.ConfigureReferenceLevel("", referenceLevel);
            wlan.ConfigureExternalAttenuation("", externalAttenuation);
            wlan.ConfigureIQPowerEdgeTrigger("", "0", RFmxWlanMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
               triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxWlanMXIQPowerEdgeTriggerLevelType.Relative,
               iqPowerEdgeEnabled);
            wlan.ConfigureStandard("", standard);
            wlan.ConfigureChannelBandwidth("", channelBandwidth);
            wlan.SelectMeasurements("", RFmxWlanMXMeasurementTypes.OfdmModAcc, true);
            wlan.OfdmModAcc.Configuration.ConfigureOptimizeDynamicRangeForEvm("", optimizeDynamicRangeForEvmEnabled,
               optimizeDynamicRangeForEvmMargin);

            if (calibrateNoiseFloor)
            {
                wlan.OfdmModAcc.Configuration.ConfigureMeasurementMode("",
                   RFmxWlanMXOfdmModAccMeasurementMode.CalibrateNoiseFloor);
                DialogResult userInput = MessageBox.Show("Turn OFF Generation", "", MessageBoxButtons.OKCancel);
                if (userInput == DialogResult.OK)
                {
                    wlan.Initiate("", "");
                    wlan.WaitForMeasurementComplete("", timeout);
                }
                MessageBox.Show("Turn ON Generaton");
            }

            wlan.OfdmModAcc.Configuration.ConfigureMeasurementMode("", RFmxWlanMXOfdmModAccMeasurementMode.Measure);
            wlan.OfdmModAcc.Configuration.ConfigureMeasurementLength("", measurementOffset, maximumMeasurementLength);
            wlan.OfdmModAcc.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            wlan.OfdmModAcc.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled);
            wlan.Initiate("", "");
        }

        void RetrieveResults()
        {
            wlan.OfdmModAcc.Results.FetchCompositeRmsEvm("", timeout, out compositeRmsEvmMean, out compositeDataRmsEvmMean, out compositePilotRmsEvmMean);
            wlan.OfdmModAcc.Results.FetchPilotConstellationTrace("", timeout, ref pilotConstellation);
            wlan.OfdmModAcc.Results.FetchDataConstellationTrace("", timeout, ref dataConstellation);
            wlan.OfdmModAcc.Results.FetchChainRmsEvmPerSubcarrierMeanTrace("", timeout, ref chainRmsEvmPerSubcarrierMean);
        }

        void PrintResults()
        {
            Console.WriteLine("------------------Composite EVM------------------");
            Console.WriteLine("RMS EVM Mean (dB)                       :{0}", compositeRmsEvmMean);
            Console.WriteLine("Data RMS EVM Mean (dB)                  :{0}", compositeDataRmsEvmMean);
            Console.WriteLine("Pilot RMS EVM Mean (dB)                 :{0}", compositePilotRmsEvmMean);
        }

        void CloseSession()
        {
            if (wlan != null)
            {
                wlan.Dispose();
                wlan = null;
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
