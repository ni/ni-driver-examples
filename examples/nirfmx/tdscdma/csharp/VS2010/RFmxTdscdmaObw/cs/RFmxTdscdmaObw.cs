//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties: Clock Source and Clock Frequency
//3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation
//4. Configure the trigger properties
//5. Select OBW measurement and enable the traces
//6. Configure Sweep Time for the OBW measurement
//7. Configure Averaging Parameters for the OBW measurement
//8. Initiate Measurement
//9. Fetch OBW Measurements and Traces
//10. Close the RFmx session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.TdscdmaMX;

namespace NationalInstruments.Examples.RFmxTdscdmaObw
{
    public class RFmxTdscdmaObw
    {
        RFmxInstrMX instrSession;
        RFmxTdscdmaMX tdscdma;
        string resourceName, frequencySource,iqPowerEdgeTriggerSource;
        double centerFrequency, referenceLevel, externalAttenuation, timeout, frequencyReferenceFrequency, sweepTimeInterval,
               triggerDelay, minimumQuietTimeDuration, iqPowerEdgeTriggerLevel;
        RFmxTdscdmaMXObwAveragingEnabled averagingEnabled;
        RFmxTdscdmaMXObwAveragingType averagingType;
        RFmxTdscdmaMXObwSweepTimeAuto sweepTimeAuto;        
        int averagingCount;

        RFmxTdscdmaMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
        RFmxTdscdmaMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        bool enableTrigger;
        RFmxTdscdmaMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
                
        double stopFrequency, startFrequency, occupiedBandwidth, absolutePower;

        public void Run()
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

            centerFrequency = 1.91e+9;             /* Hz */
            referenceLevel = 0.00;              /* dBm */
            externalAttenuation = 0.00;         /* dB */

            frequencySource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10e+6;                  /* Hz */

            triggerDelay = 0.00;                    /* seconds */
            minimumQuietTimeDuration = 16E-6;     /* seconds */
            iqPowerEdgeTriggerLevel = -20.00;       /*dB*/
            iqPowerEdgeTriggerSource = "0";
            iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising;
            enableTrigger = true;
            iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative;
            minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto;

            //Sweep Time
            sweepTimeAuto = RFmxTdscdmaMXObwSweepTimeAuto.True;
            sweepTimeInterval = 660e-6;           /* seconds */

            //Averaging
            averagingEnabled = RFmxTdscdmaMXObwAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxTdscdmaMXObwAveragingType.Rms;

            timeout = 10;                       /* seconds */

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
            
            tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.Obw, true);
            tdscdma.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            tdscdma.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, 
                                                        averagingType);            

            tdscdma.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */
            Spectrum<float> spectrum = null;
            tdscdma.Obw.Results.FetchSpectrum("", timeout, ref spectrum);
            tdscdma.Obw.Results.FetchMeasurement("", timeout, out occupiedBandwidth, out absolutePower, out startFrequency, 
                                                out stopFrequency);
        }

        private void PrintResults()
        {
            Console.WriteLine("Occupied Bandwidth (Hz)       {0}", occupiedBandwidth);
            Console.WriteLine("Absolute Power (dBm)          {0}", absolutePower);
            Console.WriteLine("Start Frequency (Hz)          {0}", startFrequency);
            Console.WriteLine("Stop Frequency (Hz)           {0}", stopFrequency);
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
