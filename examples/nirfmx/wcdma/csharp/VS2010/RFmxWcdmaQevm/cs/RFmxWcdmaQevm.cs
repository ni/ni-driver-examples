//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select QEVM measurement and enable Traces.
//6. Configure Averaging.
//7. Configure Measurement Length.
//8. Initiate the Measurement.
//9. Fetch QEVM Measurements and Traces.
//10. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WcdmaMX;

namespace NationalInstruments.Examples.RFmxWcdmaQevm
{
    public class RFmxWcdmaQevm
    {
        RFmxInstrMX instrSession;
        RFmxWcdmaMX wcdma;

        string resourceName;
        RFmxWcdmaMXMeasurementTypes measurement;
        string frequencyReferenceSource;
        double frequencyReferenceFrequency;           /* Hz */
        double centerFrequency;                       /* Hz */
        double externalAttenuation;                  /* dB */

        string digitalEdgeSource;
        RFmxWcdmaMXDigitalEdgeTriggerEdge digitalEdge;
        double triggerDelay;                         /* seconds */
        double referenceLevel;                       /* dBm */
        RFmxWcdmaMXQevmAveragingEnabled averagingEnabled;
        int averagingCount;

        int measurementLength;                           /* chips */

        double timeout;                                    /* seconds */

        double meanPhaseError;				        /*(deg) */
        double meanMagnitudeError;			    	/*(%) */
        double meanRmsEvm;			                /*(%) */
        double maximumPeakEvm;				        /*(%) */
        double meanFrequencyError;				    /*(Hz) */
        double meanChipRateError;				    /*(ppm) */

        double maximumIQGainImbalence;				/*(dB) */
        double maximumIQOriginOffset;				/*(dB) */
        double meanIQOriginOffset;			        /*(dB) */
        double meanIQGainImbalence;				    /*(dB) */
        double meanIQQuadratureError;				/*(deg) */
        double maximumIQQuadratureError;				/*(deg) */

        bool enableAllTraces;
        bool enableTrigger;


        AnalogWaveform<float> evm = null;
        ComplexSingle[] constellationTrace = null;


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

        void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void InitializeVariables()
        {
            /* Initialize input variables */

            resourceName = "RFSA";
            centerFrequency = 1.95e+9;                       /* Hz */
            referenceLevel = 0.000000;                       /* dBm */
            externalAttenuation = 0.000000;                  /* dB */

            measurement = RFmxWcdmaMXMeasurementTypes.Qevm;

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;           /* Hz */

            digitalEdgeSource = RFmxWcdmaMXConstants.Pfi0;
            digitalEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.000000;                         /* seconds */
            enableTrigger = false;


            averagingEnabled = RFmxWcdmaMXQevmAveragingEnabled.False;
            averagingCount = 10;

            measurementLength = 2560;                           /* chips */

            timeout = 10;                                    /* seconds */

            enableAllTraces = true;

        }

        void ConfigureWcdma()
        {
            wcdma = instrSession.GetWcdmaSignalConfiguration();
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
            wcdma.SelectMeasurements("", measurement, enableAllTraces);
            wcdma.Qevm.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount);
            wcdma.Qevm.Configuration.ConfigureMeasurementLength("", measurementLength);
            wcdma.Initiate("", "");
        }

        void RetrieveResults()
        {
            wcdma.Qevm.Results.FetchEvm("", timeout, out meanRmsEvm, out maximumPeakEvm,
               out meanFrequencyError, out meanMagnitudeError, out meanPhaseError, out meanChipRateError);
            wcdma.Qevm.Results.FetchIQImpairments("", timeout, out meanIQOriginOffset, out meanIQGainImbalence,
                out meanIQQuadratureError, out maximumIQOriginOffset, out maximumIQGainImbalence,
                out maximumIQQuadratureError);
            wcdma.Qevm.Results.FetchEvmTrace("", timeout, ref evm);
            wcdma.Qevm.Results.FetchConstellationTrace("", timeout, ref constellationTrace);
        }

        void PrintResults()
        {
            Console.WriteLine("------------EVM------------\n");
            Console.WriteLine("Mean RMS EVM (%)                     : {0}", meanRmsEvm);
            Console.WriteLine("Maximum Peak EVM (%)                 : {0}", maximumPeakEvm);
            Console.WriteLine("Mean Frequency Error (Hz)            : {0}", meanFrequencyError);
            Console.WriteLine("Mean Magnitude Error (%)             : {0}", meanMagnitudeError);
            Console.WriteLine("Mean Phase Error (deg)               : {0}", meanPhaseError);
            Console.WriteLine("Mean Chip Rate Error (ppm)           : {0}", meanChipRateError);

            Console.WriteLine("------------IQ Impairments------------\n");
            Console.WriteLine("Mean I/Q Origin Offset (dB)          : {0}", meanIQOriginOffset);
            Console.WriteLine("Maximum I/Q Origin Offset (dB)       : {0}", maximumIQOriginOffset);
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