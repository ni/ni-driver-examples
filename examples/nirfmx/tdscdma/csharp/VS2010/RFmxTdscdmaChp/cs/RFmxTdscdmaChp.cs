//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties: Clock Source and Clock Frequency
//3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation
//4. Configure the trigger properties
//5. Select CHP measurement and enable the traces
//6. Configure Sweep Time for the CHP measurement
//7. Configure Averaging Parameters for the CHP measurement
//8. Initiate Measurement
//9. Fetch CHP Measurements and Traces
//10. Close the RFmx session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.TdscdmaMX;

namespace NationalInstruments.Examples.RFmxTdscdmaChp
{
    class RFmxTdscdmaChp
    {
        RFmxInstrMX instrSession;
        RFmxTdscdmaMX tdscdma;
        string resourceName, frequencySource,iqPowerEdgeTriggerSource;
        double centerFrequency, referenceLevel, externalAttenuation, frequencyReferenceFrequency, sweepTimeInterval,
               triggerDelay, minimumQuietTimeDuration, iqPowerEdgeTriggerLevel,carrierAbsolutePower, timeout;
        bool enableAllTraces,enableTrigger;
        RFmxTdscdmaMXChpSweepTimeAuto sweepTimeAuto;
        RFmxTdscdmaMXChpAveragingEnabled averagingEnabled;
        int averagingCount;
        RFmxTdscdmaMXChpAveragingType averagingType;

        RFmxTdscdmaMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
        RFmxTdscdmaMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        RFmxTdscdmaMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;

        internal void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureTdscdma();
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
            centerFrequency = 1.91e+9;         /* Hz */
            referenceLevel = 0.00;          /* dBm */
            externalAttenuation = 0.00;     /* dB */
            timeout = 10.0;                 /* seconds */

            frequencySource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;            /* Hz */

            //Sweep time
            sweepTimeAuto = RFmxTdscdmaMXChpSweepTimeAuto.True;
            sweepTimeInterval = 660e-6;    /* seconds */

            //Averaging
            averagingEnabled = RFmxTdscdmaMXChpAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxTdscdmaMXChpAveragingType.Rms;

            triggerDelay = 0.00;                    /* seconds */
            minimumQuietTimeDuration = 16E-6;     /* seconds */
            iqPowerEdgeTriggerLevel = -20.00;       /*dB*/
            iqPowerEdgeTriggerSource = "0";
            iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising;
            enableTrigger = true;
            iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative;
            minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto;

            enableAllTraces = true;
        }

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void ConfigureTdscdma()
        {
            /* Get SpecAn signal */
            tdscdma = instrSession.GetTdscdmaSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencySource, frequencyReferenceFrequency);
            tdscdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel,
                triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger);
           
            tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.Chp, enableAllTraces);

            tdscdma.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            tdscdma.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,averagingType);

            tdscdma.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */
            Spectrum<float> spectrum = null;
            tdscdma.Chp.Results.FetchCarrierAbsolutePower("", timeout, out carrierAbsolutePower);
            tdscdma.Chp.Results.FetchSpectrum("", timeout, ref spectrum);
        }

        private void PrintResults()
        {
            Console.WriteLine("Carrier Absolute Power (dBm)  {0}", carrierAbsolutePower);
        }

        private void CloseSession()
        {
            try
            {
                if (tdscdma != null)
                {
                    tdscdma.Dispose();
                    tdscdma = null;
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
