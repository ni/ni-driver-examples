//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Contiguous Carriers.
//6. Select SEM measurement and enable traces.
//7. Configure Sweep Time Parameters.
//8. Configure Averaging Parameters.
//9. Initiate the Measurement.
//10. Fetch SEM Measurements and Traces.
//11. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.EvdoMX;

namespace NationalInstruments.Examples.RFmxEvdoSemMultiCarrier
{
    public class RFmxEvdoSemMultiCarrier
    {
        RFmxInstrMX instrSession;
        RFmxEvdoMX evdo;

        string resourceName = "RFSA";
        RFmxEvdoMXMeasurementTypes measurement = RFmxEvdoMXMeasurementTypes.Sem;
        string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
        double frequency = 10.0e+6;                         /* Hz */
        double centerFrequency = 833.49e+6;                 /* Hz */
        double externalAttenuation = 0.00;              /* dB */

        string digitalEdgeSource = RFmxEvdoMXConstants.Pfi0;
        RFmxEvdoMXDigitalEdgeTriggerEdge digitalEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising;
        double triggerDelay = 0.00;                     /* seconds */

        double referenceLevel = 0.00;                   /* dBm */

        RFmxEvdoMXSemAveragingEnabled averagingEnabled = RFmxEvdoMXSemAveragingEnabled.False;
        int averagingCount = 10;
        RFmxEvdoMXSemAveragingType averagingType = RFmxEvdoMXSemAveragingType.Rms;

        RFmxEvdoMXSemSweepTimeAuto sweepTimeAuto = RFmxEvdoMXSemSweepTimeAuto.True;
        double sweepTimeInterval = 1.6667e-3;               /* seconds */

        double timeout = 10;                                /* seconds */
        Spectrum<float> spectrum;
        double totalCarrierPower;

        bool enableAllTraces = true;
        bool enableTrigger = false;

        int numberOfCarriers = 3;
        int carrierCenterFrequency = -1;

        Spectrum<float> absoluteMask, relativeMask;
        int bandClass = 1;

        RFmxEvdoMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;
        double[] lowerOffsetMargin;
        double[] lowerOffsetMarginFrequency;
        double[] lowerOffsetMarginAbsolutePower;
        double[] lowerOffsetMarginRelativePower;

        RFmxEvdoMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;
        double[] upperOffsetMargin;
        double[] upperOffsetMarginFrequency;
        double[] upperOffsetMarginAbsolutePower;
        double[] upperOffsetMarginRelativePower;
        RFmxEvdoMXSemCompositeMeasurementStatus measurementStatus;
        double[] absoluteIntegratedPower;
        double[] relativeIntegratedPower;

        public void Run()
        {
            try
            {
                InitializeInstr();
                ConfigureEvdo();
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

        void ConfigureEvdo()
        {
            /* Get Evdo signal */

            evdo = instrSession.GetEvdoSignalConfiguration();

            /* Configure measurement */

            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequency);
            evdo.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);

            evdo.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);

            evdo.ConfigureContiguousCarriers("", numberOfCarriers, carrierCenterFrequency, bandClass);
            evdo.SelectMeasurements("", measurement, enableAllTraces);
            evdo.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
            evdo.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
            evdo.Initiate("", "");


        }

        void RetrieveResults()
        {
            /* Retrieve results */
            evdo.Sem.Results.FetchCarrierMeasurementArray("", timeout, ref absoluteIntegratedPower, 
                                                          ref relativeIntegratedPower);
            evdo.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref lowerOffsetMeasurementStatus,
                 ref lowerOffsetMargin, ref lowerOffsetMarginFrequency, ref lowerOffsetMarginAbsolutePower, 
                 ref lowerOffsetMarginRelativePower);
            evdo.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref upperOffsetMeasurementStatus,
                             ref upperOffsetMargin, ref upperOffsetMarginFrequency, ref upperOffsetMarginAbsolutePower, 
                             ref upperOffsetMarginRelativePower);
            evdo.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
            evdo.Sem.Results.FetchTotalCarrierPower("", timeout, out totalCarrierPower);
            evdo.Sem.Results.FetchSpectrum("", timeout, ref spectrum, ref relativeMask, ref absoluteMask);
        }

        void PrintResults()
        {
            Console.WriteLine("Measurement Status                   : {0}", measurementStatus);
            Console.WriteLine("Total Carrier Power  (dBm)           : {0}", totalCarrierPower);
            Console.WriteLine("\nCarrier Measurements\n");
            for (int i = 0; i < absoluteIntegratedPower.Length; i++)
            {
                Console.WriteLine("\nCarrier {0}", i);
                Console.WriteLine("Absolute Integrated Power (dBm)     : {0}", absoluteIntegratedPower[i]);
                Console.WriteLine("Relative Integrated Power (dB)      : {0}", relativeIntegratedPower[i]);
            }
            Console.WriteLine("\nLower Offset Segment Measurements\n");
            for (int i = 0; i < lowerOffsetMargin.Length; i++)
            {
                Console.WriteLine("\nMeasurement {0}", i);
                Console.WriteLine("Margin (dB)                         : {0}", lowerOffsetMargin[i]);
                Console.WriteLine("Margin Absolute Power (dBm)         : {0}", lowerOffsetMarginAbsolutePower[i]);
                Console.WriteLine("Margin Relative Power (dB)          : {0}", lowerOffsetMarginRelativePower[i]);
                Console.WriteLine("Margin Frequency (Hz)               : {0}", lowerOffsetMarginFrequency[i]);
                Console.WriteLine("Measurement Status                  : {0}", lowerOffsetMeasurementStatus[i]);
            }

            Console.WriteLine("\nUpper Offset Segment Measurements\n");
            for (int i = 0; i < upperOffsetMargin.Length; i++)
            {
                Console.WriteLine("\nMeasurement {0}", i);
                Console.WriteLine("Margin (dB)                         : {0}", upperOffsetMargin[i]);
                Console.WriteLine("Margin Absolute Power (dBm)         : {0}", upperOffsetMarginAbsolutePower[i]);
                Console.WriteLine("Margin Relative Power (dB)          : {0}", upperOffsetMarginRelativePower[i]);
                Console.WriteLine("Margin Frequency (Hz)               : {0}", upperOffsetMarginFrequency[i]);
                Console.WriteLine("Measurement Status                  : {0}", upperOffsetMeasurementStatus[i]);
            }
        }

        void CloseSession()
        {
            if (evdo != null)
            {
                evdo.Dispose();
                evdo = null;
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