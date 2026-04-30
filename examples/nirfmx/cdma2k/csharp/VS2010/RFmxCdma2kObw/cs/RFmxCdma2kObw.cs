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
using NationalInstruments.RFmx.Cdma2kMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxCdma2kObw
{
    public class RFmxCdma2kObw
    {
        RFmxInstrMX instrSession;
        RFmxCdma2kMX cdma2k;
        string resourceName, frequencySource;
        double centerFrequency, referenceLevel, externalAttenuation, timeOut, frequency, sweepTimeInterval;
        RFmxCdma2kMXObwAveragingEnabled averagingEnabled;
        RFmxCdma2kMXObwAveragingType averagingType;
        RFmxCdma2kMXObwSweepTimeAuto sweepTimeAuto;
        int averagingCount;
        bool enableTrigger;
        string digitalEdgeTriggerSource;
        RFmxCdma2kMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;
        double triggerDelay, stopFrequency, startFrequency, occupiedBandwidth, absolutePower;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureCdma2k();
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

            centerFrequency = 833.49e+6;             /* Hz */
            referenceLevel = 0.00;              /* dBm */
            externalAttenuation = 0.00;         /* dB */

            frequencySource = RFmxInstrMXConstants.OnboardClock;
            frequency = 10e+6;                  /* Hz */


            

            //Sweep Time
            sweepTimeAuto = RFmxCdma2kMXObwSweepTimeAuto.True;
            sweepTimeInterval = 1.67e-3;           /* seconds */

            //Averaging
            averagingEnabled = RFmxCdma2kMXObwAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxCdma2kMXObwAveragingType.Rms;

            //trigger
            enableTrigger = false;
            triggerDelay = 0.00;                    /* seconds */
            digitalEdgeTriggerSource = RFmxCdma2kMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising;
            timeOut = 10;                       /* seconds */

        }

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        private void ConfigureCdma2k()
        {
            /* Get SpecAn signal */
            cdma2k = instrSession.GetCdma2kSignalConfiguration();

            /* Configure measurement */
            instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
            cdma2k.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            cdma2k.ConfigureDigitalEdgeTrigger("", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger);

            cdma2k.SelectMeasurements("", RFmxCdma2kMXMeasurementTypes.Obw, true);
            cdma2k.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            cdma2k.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount,
                                                        averagingType);


            cdma2k.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */
            Spectrum<float> spectrum = null;
            cdma2k.Obw.Results.FetchMeasurement("", timeOut, out occupiedBandwidth, out absolutePower, out startFrequency,
                                                out stopFrequency);
            cdma2k.Obw.Results.FetchSpectrum("", timeOut, ref spectrum);

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
                if (cdma2k != null)
                {
                    cdma2k.Dispose();
                    cdma2k = null;
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
