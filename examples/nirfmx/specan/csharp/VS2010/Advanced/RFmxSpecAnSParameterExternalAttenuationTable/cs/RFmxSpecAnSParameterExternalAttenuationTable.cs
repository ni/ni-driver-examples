//Steps:
//1. Open a new RFmx session.
//2. Configure the basic instrument properties (Clock Source and Clock Frequency).
//3. Configure S - parameter External Attenuation Table.
//4. Configure External Attenuation Interpolation.
//5. Configure S - parameter External Attenuation Type.
//6. Configure Selected Ports.
//7. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//8. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time).
//9. Configure TXP measurement and enable the traces.
//10. Configure the Measurement Interval.
//11. Configure RBW filter parameters.
//12. Configure Thresholding.
//13. Configure Averaging parameters.
//14. Configure VBW filter parameters.
//15. Initiate Measurement.
//16. Fetch TXP Traces and Measurements.
//17. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.SpecAnMX;

namespace NationalInstruments.Examples.RFmxSpecAnSParameterExternalAttenuationTable
{
    public class RFmxSpecAnSParameterExternalAttenuationTable
    {
        RFmxInstrMX instrSession;
        RFmxSpecAnMX specAn;

        string resourceName;
        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

        string selectedPorts;
        string portString;

        string frequencySource;
        double frequency;

        double triggerDelay;
        double iqPowerEdgeLevel;
        double minimumQuietTime;
        bool enableTrigger;

        const int frequencyArraySize = 3;
        string tableName;
        double[] frequencyArray = new double[frequencyArraySize] { 997.0e+6, 1.0e+9, 1.003e+9 }; /* Hz */
        ComplexDouble[,,] sParameters = new ComplexDouble[3, 2, 2]
           {
            {{new ComplexDouble(1.00, 0.00), new ComplexDouble(1.00, 0.10)}, {new ComplexDouble(1.00, 0.10), new ComplexDouble(1.00, 0.00)}},
            {{new ComplexDouble(0.80, 0.10), new ComplexDouble(0.80, 0.25)}, {new ComplexDouble(0.80, 0.25), new ComplexDouble(0.80, 0.10)}},
            {{new ComplexDouble(1.00, 0.25), new ComplexDouble(1.00, 0.50)}, {new ComplexDouble(1.00, 0.50), new ComplexDouble(1.00, 0.25)}}
           };
        RFmxInstrMXSParameterOrientation sParameterOrientation;
        RFmxInstrMXLinearInterpolationFormat format;
        RFmxInstrMXSParameterType sParameterType;

        double measurementInterval;

        RFmxSpecAnMXTxpRbwFilterType rbwFilterType;
        double rbw;
        double rrcAlpha ;

        RFmxSpecAnMXTxpVbwFilterAutoBandwidth vbwAuto;
        double vbw;
        double vbwToRbwRatio;
        RFmxSpecAnMXTxpAveragingEnabled averagingEnabled;
        int averagingCount;
        RFmxSpecAnMXTxpAveragingType averagingType;

        RFmxSpecAnMXTxpThresholdEnabled thresholdEnabled;
        RFmxSpecAnMXTxpThresholdType thresholdType;
        double thresholdLevel;

        double timeout;
        double averageMeanPower;
        double peakToAverageRatio;
        double maximumPower;
        double minimumPower;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureSpecAn();
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
            selectedPorts = "";
            centerFrequency = 1e+9;                 /* Hz */
            referenceLevel = 0.00;                  /* dBm */
            externalAttenuation = 0.00;             /* dB */

            frequencySource = RFmxInstrMXConstants.OnboardClock;
            frequency = 10e+6;                      /* Hz */

            iqPowerEdgeLevel = -20.0;               /* dBm */
            triggerDelay = 0.0;                     /* seconds */
            minimumQuietTime = 0.0;                 /* seconds */
            enableTrigger = false;

            tableName = "";
            format = RFmxInstrMXLinearInterpolationFormat.RealAndImaginary;
            sParameterOrientation = RFmxInstrMXSParameterOrientation.Port1TowardsDut;
            sParameterType = RFmxInstrMXSParameterType.Scalar;

            measurementInterval = 1e-3;             /* seconds */

            rbwFilterType = RFmxSpecAnMXTxpRbwFilterType.Gaussian;
            rbw = 100e+3;                           /* Hz */
            rrcAlpha = 0.010;

            vbwAuto = RFmxSpecAnMXTxpVbwFilterAutoBandwidth.True;
            vbw = 30.0e3;                           /* Hz */
            vbwToRbwRatio = 3;

            averagingEnabled = RFmxSpecAnMXTxpAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxSpecAnMXTxpAveragingType.Rms;

            thresholdEnabled = RFmxSpecAnMXTxpThresholdEnabled.False;
            thresholdType = RFmxSpecAnMXTxpThresholdType.Relative;
            thresholdLevel = -20.0;                 /* (dB or dBm) */

            timeout = 10;                           /* seconds */
        }

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void ConfigureSpecAn()
        {
            /* Get SpecAn signal */
            specAn = instrSession.GetSpecAnSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
            portString = RFmxInstrMX.BuildPortString2("", selectedPorts, "", 0);
            instrSession.ConfigureSParameterExternalAttenuationTable(portString, tableName, frequencyArray, sParameters, sParameterOrientation);
            instrSession.ConfigureExternalAttenuationInterpolationLinear(portString, tableName, format);
            instrSession.ConfigureSParameterExternalAttenuationType(portString, sParameterType);
            specAn.SetSelectedPorts("", selectedPorts);
            specAn.ConfigureFrequency("", centerFrequency);
            specAn.ConfigureReferenceLevel("", referenceLevel);
            specAn.ConfigureExternalAttenuation("", externalAttenuation);
            specAn.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeLevel, RFmxSpecAnMXIQPowerEdgeTriggerSlope.Rising,
               triggerDelay, RFmxSpecAnMXTriggerMinimumQuietTimeMode.Manual, minimumQuietTime, enableTrigger);
            specAn.SelectMeasurements("", RFmxSpecAnMXMeasurementTypes.Txp, true);
            specAn.Txp.Configuration.ConfigureMeasurementInterval("", measurementInterval);
            specAn.Txp.Configuration.ConfigureRbwFilter("", rbw, rbwFilterType, rrcAlpha);
            specAn.Txp.Configuration.ConfigureThreshold("", thresholdEnabled, thresholdLevel, thresholdType);
            specAn.Txp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            specAn.Txp.Configuration.ConfigureVbwFilter("", vbwAuto, vbw, vbwToRbwRatio);
            specAn.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */

            AnalogWaveform<float> power = null;
            specAn.Txp.Results.FetchPowerTrace("", timeout, ref power);
            specAn.Txp.Results.FetchMeasurement("", timeout, out averageMeanPower, out peakToAverageRatio, out maximumPower,
                                                out minimumPower);
        }

        private void PrintResults()
        {
            Console.WriteLine("---------------Measurement---------------");
            Console.WriteLine("Average Mean Power  (dBm)      : {0}", averageMeanPower);
            Console.WriteLine("Peak to Average Ratio(dB)      : {0}", peakToAverageRatio);
            Console.WriteLine("Maximum Power (dBm)            : {0}", maximumPower);
            Console.WriteLine("Minimum Power (dBm)            : {0}", minimumPower);
        }

        private void CloseSession()
        {
            try
            {
                if (specAn != null)
                {
                    specAn.Dispose();
                    specAn = null;
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
