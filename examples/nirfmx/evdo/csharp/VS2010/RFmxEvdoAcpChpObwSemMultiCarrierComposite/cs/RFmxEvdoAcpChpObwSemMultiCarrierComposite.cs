//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Contiguous Carriers.
//6. Select ACP, CHP, OBW and SEM measurements and enable Traces.
//7. Configure Sweep Time Parameters for ACP.
//8. Configure Averaging Parameters for ACP.
//9. Configure Sweep Time Parameters for CHP.
//10. Configure Averaging Parameters for CHP.
//11. Configure Sweep Time Parameters for OBW.
//12. Configure Averaging Parameters for OBW.
//13. Configure Sweep Time Parameters for SEM.
//14. Configure Averaging Parameters for SEM.
//15. Initiate the Measurement.
//16. Fetch ACP, CHP, OBW and SEM Measurements.
//17. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.EvdoMX;

namespace NationalInstruments.Examples.RFmxEvdoAcpChpObwSemMultiCarrierComposite
{
    public class RFmxEvdoAcpChpObwSemMultiCarrierComposite
    {
        RFmxInstrMX instrSession;
        RFmxEvdoMX evdo;

        string resourceName = "RFSA";

        string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
        double frequency = 10.0e+6;                         /* Hz */
        double centerFrequency = 833.49e+6;                   /* Hz */
        double referenceLevel = 0.00;                   /* dBm */
        double externalAttenuation = 0.00;              /* dB */
        bool enableTrigger = false;
        string digitalEdgeSource = RFmxEvdoMXConstants.Pfi0;
        RFmxEvdoMXDigitalEdgeTriggerEdge digitalEdge = RFmxEvdoMXDigitalEdgeTriggerEdge.Rising;
        double triggerDelay = 0.00;                     /* seconds */

        double timeout = 10.00;                              /* seconds */

        int bandClass = 0;
        int frequencyReferenceCarrier = -1;
        int numberOfCarriers = 3;

        RFmxEvdoMXAcpSweepTimeAuto acpSweepTimeAuto = RFmxEvdoMXAcpSweepTimeAuto.True;
        RFmxEvdoMXChpSweepTimeAuto chpSweepTimeAuto = RFmxEvdoMXChpSweepTimeAuto.True;
        RFmxEvdoMXObwSweepTimeAuto obwSweepTimeAuto = RFmxEvdoMXObwSweepTimeAuto.True;
        RFmxEvdoMXSemSweepTimeAuto semSweepTimeAuto = RFmxEvdoMXSemSweepTimeAuto.True;
        double sweepTimeInterval = 1.67e-3;
        RFmxEvdoMXAcpAveragingEnabled acpAveragingEnabled = RFmxEvdoMXAcpAveragingEnabled.False;
        RFmxEvdoMXChpAveragingEnabled chpAveragingEnabled = RFmxEvdoMXChpAveragingEnabled.False;
        RFmxEvdoMXObwAveragingEnabled obwAveragingEnabled = RFmxEvdoMXObwAveragingEnabled.False;
        RFmxEvdoMXSemAveragingEnabled semAveragingEnabled = RFmxEvdoMXSemAveragingEnabled.False;
        int averagingCount = 10;
        RFmxEvdoMXAcpAveragingType acpAveragingType = RFmxEvdoMXAcpAveragingType.Rms;
        RFmxEvdoMXChpAveragingType chpAveragingType = RFmxEvdoMXChpAveragingType.Rms;
        RFmxEvdoMXObwAveragingType obwAveragingType = RFmxEvdoMXObwAveragingType.Rms;
        RFmxEvdoMXSemAveragingType semAveragingType = RFmxEvdoMXSemAveragingType.Rms;
        double chpTotalAggregatedPower;
        double acpTotalAggregatedPower;
        double obwOccupiedBandwidth;
        double obwAbsolutePower;
        double obwStopFrequency;
        double obwStartFrequency;
        RFmxEvdoMXSemCompositeMeasurementStatus semMeasurementStatus;
        double semTotalAggregatedPower;

        double[] chpAbsolutePower; /*(dBm) */
        double[] chpRelativePower; /*(dB) */

        double[] semAbsoluteIntegratedPower;
        double[] semRelativeIntegratedPower;

        double[] acpAbsolutePower;
        double[] acpRelativePower;

        double[] acpLowerAbsolutePower; /*(dBm) */
        double[] acpUpperAbsolutePower;
        double[] acpLowerRelativePower; /*(dB) */
        double[] acpUpperRelativePower;

        RFmxEvdoMXSemLowerOffsetMeasurementStatus[] semLowerOffsetMeasurementStatus;
        double[] semLowerOffsetMargin; /*(dB) */
        double[] semLowerOffsetMarginFrequency; /*(Hz) */
        double[] semLowerOffsetMarginAbsolutePower; /*(dBm) */
        double[] semLowerOffsetMarginRelativePower;

        RFmxEvdoMXSemUpperOffsetMeasurementStatus[] semUpperOffsetMeasurementStatus;
        double[] semUpperOffsetMargin;
        double[] semUpperOffsetMarginFrequency;
        double[] semUpperOffsetMarginAbsolutePower;
        double[] semUpperOffsetMarginRelativePower;

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

            evdo.ConfigureContiguousCarriers("", numberOfCarriers, frequencyReferenceCarrier, bandClass);


            evdo.SelectMeasurements("", RFmxEvdoMXMeasurementTypes.Acp |
                                RFmxEvdoMXMeasurementTypes.Chp | RFmxEvdoMXMeasurementTypes.Obw |
                                RFmxEvdoMXMeasurementTypes.Sem, true);
            evdo.Acp.Configuration.ConfigureSweepTime("", acpSweepTimeAuto, sweepTimeInterval);
            evdo.Acp.Configuration.ConfigureAveraging("", acpAveragingEnabled, averagingCount, acpAveragingType);
            evdo.Chp.Configuration.ConfigureSweepTime("", chpSweepTimeAuto, sweepTimeInterval);
            evdo.Chp.Configuration.ConfigureAveraging("", chpAveragingEnabled, averagingCount, chpAveragingType);
            evdo.Obw.Configuration.ConfigureSweepTime("", obwSweepTimeAuto, sweepTimeInterval);
            evdo.Obw.Configuration.ConfigureAveraging("", obwAveragingEnabled, averagingCount, obwAveragingType);
            evdo.Sem.Configuration.ConfigureSweepTime("", semSweepTimeAuto, sweepTimeInterval);
            evdo.Sem.Configuration.ConfigureAveraging("", semAveragingEnabled, averagingCount, semAveragingType);
            evdo.Initiate("", "");
        }

        void RetrieveResults()
        {
            /* Retrieve results */

            //ACP
            evdo.Acp.Results.FetchOffsetMeasurementArray("", timeout,
                    ref acpLowerRelativePower, ref acpUpperRelativePower,
                    ref acpLowerAbsolutePower, ref acpUpperAbsolutePower);
            evdo.Acp.Results.FetchCarrierMeasurementArray("", timeout,
                    ref acpAbsolutePower,
                    ref acpRelativePower);
            evdo.Acp.Results.FetchTotalCarrierPower("", timeout, out acpTotalAggregatedPower);

            //CHP
            evdo.Chp.Results.FetchCarrierMeasurementArray("", timeout,
                          ref chpAbsolutePower,
                          ref chpRelativePower);
            evdo.Chp.Results.FetchTotalCarrierPower("", timeout, out chpTotalAggregatedPower);

            //OBW
            evdo.Obw.Results.FetchMeasurement("", timeout, out obwOccupiedBandwidth, out obwAbsolutePower, out obwStartFrequency, out obwStopFrequency);

            //SEM
            evdo.Sem.Results.FetchLowerOffsetMarginArray("", timeout,
                    ref semLowerOffsetMeasurementStatus,
                    ref semLowerOffsetMargin,
                    ref semLowerOffsetMarginFrequency,
                    ref semLowerOffsetMarginAbsolutePower,
                    ref semLowerOffsetMarginRelativePower);
            evdo.Sem.Results.FetchUpperOffsetMarginArray("", timeout,
                    ref semUpperOffsetMeasurementStatus,
                    ref semUpperOffsetMargin,
                    ref semUpperOffsetMarginFrequency,
                    ref semUpperOffsetMarginAbsolutePower,
                    ref semUpperOffsetMarginRelativePower);

            evdo.Sem.Results.FetchCarrierMeasurementArray("", timeout,
                    ref semAbsoluteIntegratedPower,
                    ref semRelativeIntegratedPower);

            evdo.Sem.Results.FetchMeasurementStatus("", timeout, out semMeasurementStatus);
            evdo.Sem.Results.FetchTotalCarrierPower("", timeout, out semTotalAggregatedPower);

        }

        void PrintResults()
        {

            Console.WriteLine("\n************************* ACP *************************\n\n");
            Console.WriteLine("Total Carrier Power  (dBm)	 : {0}", acpTotalAggregatedPower);
            Console.WriteLine("\nCarrier Measurements	         : \n");
            for (int i = 0; i < acpAbsolutePower.Length; i++)
            {
                Console.WriteLine("\nCarrier                          : {0}", i);
                Console.WriteLine("Absolute Power  (dBm)            : {0}", acpAbsolutePower[i]);
                Console.WriteLine("Relative Power  (dB)             : {0}", acpRelativePower[i]);
            }
            Console.WriteLine("\nOffset Channel Measurements	 : \n");
            for (int i = 0; i < acpLowerRelativePower.Length; i++)
            {
                Console.WriteLine("\nOffset                           : {0}", i);
                Console.WriteLine("Lower Relative Power (dB)        : {0}", acpLowerRelativePower[i]);
                Console.WriteLine("Upper Relative Power (dB)        : {0}", acpUpperRelativePower[i]);
                Console.WriteLine("Lower Absolute Power (dBm)       : {0}", acpLowerAbsolutePower[i]);
                Console.WriteLine("Upper Absolute Power (dBm)       : {0}", acpUpperAbsolutePower[i]);
            }

            Console.WriteLine("\n************************* CHP *************************\n\n");
            Console.WriteLine("Total Carrier Power  (dBm)	 : {0}", chpTotalAggregatedPower);
            Console.WriteLine("\nCarrier Measurements             : \n");
            for (int i = 0; i < chpAbsolutePower.Length; i++)
            {
                Console.WriteLine("Carrier                          : {0}", i);
                Console.WriteLine("Absolute Power  (dBm)            : {0}", chpAbsolutePower[i]);
                Console.WriteLine("Relative Power  (dB)             : {0}", chpRelativePower[i]);
            }


            Console.WriteLine("\n************************* OBW *************************\n\n");
            Console.WriteLine("Occupied Bandwidth  (Hz)         : {0}", obwOccupiedBandwidth);
            Console.WriteLine("Absolute Power  (dBm)            : {0}", obwAbsolutePower);
            Console.WriteLine("Start Frequency  (Hz)            : {0}", obwStartFrequency);
            Console.WriteLine("Stop Frequency  (Hz)             : {0}", obwStopFrequency);

            Console.WriteLine("\n************************* SEM *************************\n\n");
            Console.WriteLine("Measurement Status               : {0}", semMeasurementStatus);
            Console.WriteLine("Total Carrier Power  (dBm)       : {0}", semTotalAggregatedPower);
            Console.WriteLine("\nCarrier Measurements	         : \n");
            for (int i = 0; i < semAbsoluteIntegratedPower.Length; i++)
            {
                Console.WriteLine("\nCarrier                          : {0}", i);
                Console.WriteLine("Absolute Integrated Power  (dBm) : {0}", semAbsoluteIntegratedPower[i]);
                Console.WriteLine("Relative Integrated Power  (dB)  : {0}", semRelativeIntegratedPower[i]);
            }
            Console.WriteLine("\nLower Offset Segment Measurements: \n");

            for (int i = 0; i < semLowerOffsetMargin.Length; i++)
            {
                Console.WriteLine("\nOffset                           : {0}", i);
                Console.WriteLine("Margin  (dB)                     : {0}", semLowerOffsetMargin[i]);
                Console.WriteLine("Margin Absolute Power  (dBm)     : {0}", semLowerOffsetMarginAbsolutePower[i]);
                Console.WriteLine("Margin Relative Power  (dB)      : {0}", semLowerOffsetMarginRelativePower[i]);
                Console.WriteLine("Margin Frequency  (Hz)           : {0}", semLowerOffsetMarginFrequency[i]);
                Console.WriteLine("Measurement Status               : {0}", semLowerOffsetMeasurementStatus[i]);
            }
            Console.WriteLine("\nUpper Offset Segment Measurements: \n");

            for (int i = 0; i < semUpperOffsetMeasurementStatus.Length; i++)
            {
                Console.WriteLine("\nOffset                           : {0}", i);
                Console.WriteLine("Margin  (dB)                     : {0}", semUpperOffsetMargin[i]);
                Console.WriteLine("Margin Absolute Power  (dBm)     : {0}", semUpperOffsetMarginAbsolutePower[i]);
                Console.WriteLine("Margin Relative Power  (dB)      : {0}", semUpperOffsetMarginRelativePower[i]);
                Console.WriteLine("Margin Frequency  (Hz)           : {0}", semUpperOffsetMarginFrequency[i]);
                Console.WriteLine("Measurement Status               : {0}", semUpperOffsetMeasurementStatus[i]);
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
