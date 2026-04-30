//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties: Clock Source and Clock Frequency
//3. Configure the basic signal properties: External Attenuation 
//4. Configure the Center Frequency (or Channel Number which derives the Center Frequency)
//5. Configure the trigger properties
//6. Configure Auto Level 
//7. Select ACP measurement and enable the traces
//8. Configure Averaging parameters for the ACP measurement
//9. Configure Sweep Time for the ACP measurement
//10. Configure Noise Compensation for the ACP measurement
//11. Configure Number of Offset Channels for the ACP measurement
//12. Initiate Measurement
//13. Fetch ACP Measurements and Traces
//14. Close the RFmx session

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.TdscdmaMX;

namespace NationalInstruments.Examples.RFmxTdscdmaAcp
{
    public class RFmxTdscdmaAcp
    {
        RFmxInstrMX instrSession;
        RFmxTdscdmaMX tdscdma;
        string resourceName, frequencySource,iqPowerEdgeTriggerSource;

        const int NumberOfOffsets = 2;

        double centerFrequency, referenceLevel, autoSetReferenceLevel, externalAttenuation, frequencyReferenceFrequency,
               sweepTimeInterval, measurementInterval,timeout,triggerDelay, minimumQuietTimeDuration,
               iqPowerEdgeTriggerLevel;
        bool autoLevel,enableTrigger;
       
        RFmxTdscdmaMXAcpNoiseCompensationEnabled noiseCompensationEnabled;

        RFmxTdscdmaMXAcpSweepTimeAuto sweepTimeAuto;
        int averagingCount;
        RFmxTdscdmaMXAcpAveragingEnabled averagingEnabled;
        RFmxTdscdmaMXAcpAveragingType averagingType;

        RFmxTdscdmaMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
        RFmxTdscdmaMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
        RFmxTdscdmaMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;

  
        double carrierAbsolutePower;
        double[] lowerRelativePower;
        double[] upperRelativePower;
        double[] lowerAbsolutePower;
        double[] upperAbsolutePower;

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
            centerFrequency = 1.91e+9;             /* Hz */
            referenceLevel = 0.00;                 /* dBm */
            externalAttenuation = 0.00;            /* dB */

            frequencySource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;                /* Hz */

            measurementInterval = 5e-3;            /* seconds */
            autoLevel = false;

            triggerDelay = 0.00;                   /* seconds */
            minimumQuietTimeDuration = 16E-6;      /* seconds */  
            iqPowerEdgeTriggerLevel = -20.00;      /*dB*/
            iqPowerEdgeTriggerSource = "0";
            iqPowerEdgeTriggerSlope = RFmxTdscdmaMXIQPowerEdgeTriggerSlope.Rising;
            enableTrigger = true;
            iqPowerEdgeTriggerLevelType = RFmxTdscdmaMXIQPowerEdgeTriggerLevelType.Relative;
            minimumQuietTimeMode = RFmxTdscdmaMXTriggerMinimumQuietTimeMode.Auto;

            noiseCompensationEnabled = RFmxTdscdmaMXAcpNoiseCompensationEnabled.False;

            // Sweep Time
            sweepTimeAuto = RFmxTdscdmaMXAcpSweepTimeAuto.True;
            sweepTimeInterval = 660e-6;	          /* seconds */

            //Averaging 
            averagingEnabled = RFmxTdscdmaMXAcpAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxTdscdmaMXAcpAveragingType.Rms;

            timeout = 10.0;                       /* seconds */
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
            tdscdma.ConfigureExternalAttenuation("", externalAttenuation);
            tdscdma.ConfigureFrequency("", centerFrequency);
            tdscdma.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel,
                triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration, iqPowerEdgeTriggerLevelType, enableTrigger);
           
                
            if (autoLevel)
            {
                tdscdma.AutoLevel("", measurementInterval, out autoSetReferenceLevel);
                Console.WriteLine("Reference Level (dBm)                 : {0}\n", autoSetReferenceLevel);
            }
            else
            {
                tdscdma.ConfigureReferenceLevel("", referenceLevel);
            }

            tdscdma.SelectMeasurements("", RFmxTdscdmaMXMeasurementTypes.Acp, true);
            tdscdma.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            tdscdma.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            tdscdma.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled);
            tdscdma.Acp.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsets);
            
            tdscdma.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */

            Spectrum<float> spectrum = null;

            tdscdma.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref lowerRelativePower,
                                                           ref upperRelativePower,
                                                           ref lowerAbsolutePower,
                                                           ref upperAbsolutePower);

            tdscdma.Acp.Results.FetchCarrierAbsolutePower("", timeout, out carrierAbsolutePower);

            tdscdma.Acp.Results.FetchSpectrum("", timeout, ref spectrum);
        }

        private void PrintResults()
        {
            Console.WriteLine("-----------------Carrier Measurements-----------------\n");
            Console.WriteLine("Carrier Absolute Power (dBm)         {0}", carrierAbsolutePower);
            
            Console.WriteLine("\n--------------Offset Channel Measurements-------------\n");
            for (int i = 0; i < NumberOfOffsets; i++)
            {
                Console.WriteLine("----Offset {0}\n", i);
                Console.WriteLine("Lower Relative Power (dB)            {0}", lowerRelativePower[i]);
                Console.WriteLine("Upper Relative Power (dB)            {0}", upperRelativePower[i]);
                Console.WriteLine("Lower Absolute Power (dBm)           {0}", lowerAbsolutePower[i]);
                Console.WriteLine("Upper Absolute Power (dBm)           {0}", upperAbsolutePower[i]);
            }
            Console.WriteLine("-------------------------------------------------\n");
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
