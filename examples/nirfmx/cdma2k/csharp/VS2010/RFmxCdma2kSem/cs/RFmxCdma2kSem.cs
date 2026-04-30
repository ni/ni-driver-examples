//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties: Clock Source and Clock Frequency
//3. Configure the basic signal properties: Center Frequency, Reference Level and External Attenuation
//4. Configure the trigger properties
//5. Select SEM measurement and enable the traces
//6. Configure Sweep Time for the SEM measurement
//7. Configure Averaging Parameters for the SEM measurement
//8. Initiate Measurement
//9. Fetch SEM Measurements and Traces
//10. Close the RFmx session

using System;
using NationalInstruments.RFmx.Cdma2kMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxCdma2kSem
{
    public class RFmxCdma2kSem
    {
        RFmxInstrMX instrSession;
        RFmxCdma2kMX cdma2k;
        String resourceName, frequencySource;
        double centerFrequency, referenceLevel, externalAttenuation, frequency, sweepTimeInterval;

        int averagingCount;
        RFmxCdma2kMXSemAveragingEnabled averagingEnabled;
        RFmxCdma2kMXSemAveragingType averagingType;
        RFmxCdma2kMXSemSweepTimeAuto sweepTimeAuto;

        int bandclass;

        double timeout = 10.0;

        const int NumberOfOffsets = 2;

        //Output values
        double carrierAbsoluteIntegratedPower;
        double[] lowerOffsetMargin;
        double[] lowerOffsetMarginAbsolutePower;
        double[] lowerOffsetMarginRelativePower;
        double[] lowerOffsetMarginFrequency;
        RFmxCdma2kMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;

        RFmxCdma2kMXSemMeasurementStatus measurementStatus;

        double[] upperOffsetMargin;
        double[] upperOffsetMarginAbsolutePower;
        double[] upperOffsetMarginRelativePower;
        double[] upperOffsetMarginFrequency;

        RFmxCdma2kMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;

        string digitalEdgeTriggerSource;
        RFmxCdma2kMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;
        double triggerDelay;
        bool enableTrigger;

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
            resourceName = "RFSA";
            centerFrequency = 833.49e+6;                                            /* Hz */
            referenceLevel = 0.00;                                                  /* dBm */
            externalAttenuation = 0.00;                                             /* dB */

            frequencySource = RFmxInstrMXConstants.OnboardClock;
            frequency = 10.0e+6;                                                    /* Hz */

            triggerDelay = 0.00;                                                    /* seconds */

            digitalEdgeTriggerSource = RFmxCdma2kMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising;
            enableTrigger = false;

            bandclass = 0;

            // Sweep Time
            sweepTimeAuto = RFmxCdma2kMXSemSweepTimeAuto.True;
            sweepTimeInterval = 1.67e-3;	                                        /* seconds */

            averagingEnabled = RFmxCdma2kMXSemAveragingEnabled.False;
            averagingCount = 10;
            averagingType = RFmxCdma2kMXSemAveragingType.Rms;
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

            cdma2k.ConfigureBandClass("", bandclass);

            cdma2k.SelectMeasurements("", RFmxCdma2kMXMeasurementTypes.Sem, true);

            cdma2k.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);

            cdma2k.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);

            cdma2k.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */

            Spectrum<float> spectrum = null, absoluteMask = null, relativeMask = null;

            cdma2k.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref lowerOffsetMeasurementStatus,
                                                           ref lowerOffsetMargin,
                                                           ref lowerOffsetMarginFrequency,
                                                           ref lowerOffsetMarginAbsolutePower,
                                                           ref lowerOffsetMarginRelativePower);

            cdma2k.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref upperOffsetMeasurementStatus,
                                                           ref upperOffsetMargin,
                                                           ref upperOffsetMarginFrequency,
                                                           ref upperOffsetMarginAbsolutePower,
                                                           ref upperOffsetMarginRelativePower);

            cdma2k.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
            cdma2k.Sem.Results.FetchCarrierAbsoluteIntegratedPower("", timeout, out carrierAbsoluteIntegratedPower);
            cdma2k.Sem.Results.FetchSpectrum("", timeout, ref spectrum, ref absoluteMask, ref relativeMask);

        }

        private void PrintResults()
        {
            Console.WriteLine("Measurement Status                         : {0}\n", measurementStatus);

            Console.WriteLine("--------------------------Carrier Measurement----------------------------\n");
            Console.WriteLine("Carrier Absolute Integrated Power (dBm)    : {0}", carrierAbsoluteIntegratedPower);

            Console.WriteLine("\n--------------Offset segment measurements ---------------------------\n");
            Console.WriteLine("\n Lower Offset segment measurements");
            for (int i = 0; i < NumberOfOffsets; i++)
            {
                Console.WriteLine("Offset                                     : {0}\n", i);

                Console.WriteLine("Lower Offset : Margin (dB)                 : {0}", lowerOffsetMargin[i]);
                Console.WriteLine("Lower offset : Margin Absolute Power (dBm) : {0}", lowerOffsetMarginAbsolutePower[i]);
                Console.WriteLine("Lower offset : Margin Relative Power (dB)  : {0}", lowerOffsetMarginRelativePower[i]);
                Console.WriteLine("Lower offset : Margin Frequency (Hz)       : {0}", lowerOffsetMarginFrequency[i]);
                Console.WriteLine("Lower offset : Measurement Status          : {0}\n", lowerOffsetMeasurementStatus[i]);
            }

            Console.WriteLine("\n");
            Console.WriteLine("\n Upper Offset segment measurements");
            for (int i = 0; i < NumberOfOffsets; i++)
            {
                Console.WriteLine("Offset                                     : {0}\n", i);

                Console.WriteLine("Upper Offset : Margin (dB)                 : {0}", upperOffsetMargin[i]);
                Console.WriteLine("Upper offset : Margin Absolute Power (dBm) : {0}", upperOffsetMarginAbsolutePower[i]);
                Console.WriteLine("Upper offset : Margin Relative Power (dB)  : {0}", upperOffsetMarginRelativePower[i]);
                Console.WriteLine("Upper offset : Margin Frequency (Hz)       : {0}", upperOffsetMarginFrequency[i]);
                Console.WriteLine("Upper offset : Measurement Status          : {0}\n", upperOffsetMeasurementStatus[i]);
                Console.WriteLine("-----------------------------------------------------------------------\n");
            }
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

        private void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }
    }
}
