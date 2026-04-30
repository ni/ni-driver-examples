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
using NationalInstruments.RFmx.Cdma2kMX;

namespace NationalInstruments.Examples.RFmxCdma2kChp
{
    class RFmxCdma2kChp
    {
        RFmxInstrMX instrSession;
        RFmxCdma2kMX cdma2k;
        string resourceName, frequencySource;
        double centerFrequency, referenceLevel, externalAttenuation, frequency, sweepTimeInterval;
        RFmxCdma2kMXChpSweepTimeAuto sweepTimeAuto;
        RFmxCdma2kMXChpAveragingEnabled averagingEnabled;
        int averagingCount;
        RFmxCdma2kMXChpAveragingType averagingType;

        string digitalEdgeTriggerSource;
        RFmxCdma2kMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;
        double triggerDelay;
        bool enableTrigger;

        double carrierAbsolutePower, timeout;

        internal void Run()
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
            centerFrequency = 833.49e+6;                                        /* Hz */
            referenceLevel = 0.00;                                              /* dBm */
            externalAttenuation = 0.00;                                         /* dB */
            timeout = 10.0;                                                     /* seconds */

            frequencySource = RFmxInstrMXConstants.OnboardClock;
            frequency = 10.0e+6;                                                /* Hz */

            //Sweep time
            sweepTimeAuto = RFmxCdma2kMXChpSweepTimeAuto.True;
            sweepTimeInterval = 1.67e-3;                                        /* seconds */

            //Averaging
            averagingEnabled = RFmxCdma2kMXChpAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxCdma2kMXChpAveragingType.Rms;

            triggerDelay = 0.00;                                                /* seconds */

            digitalEdgeTriggerSource = RFmxInstrMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising;
            enableTrigger = false;
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

            cdma2k.SelectMeasurements("", RFmxCdma2kMXMeasurementTypes.Chp, true);

            cdma2k.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            cdma2k.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);

            cdma2k.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */
            Spectrum<float> spectrum = null;
            cdma2k.Chp.Results.FetchCarrierAbsolutePower("", timeout, out carrierAbsolutePower);
            cdma2k.Chp.Results.FetchSpectrum("", timeout, ref spectrum);            
        }

        private void PrintResults()
        {
            Console.WriteLine("Carrier Absolute Power (dBm)  : {0}", carrierAbsolutePower);
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
