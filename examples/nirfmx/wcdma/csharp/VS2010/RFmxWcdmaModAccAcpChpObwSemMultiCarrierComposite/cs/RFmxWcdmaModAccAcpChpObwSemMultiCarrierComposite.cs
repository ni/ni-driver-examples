//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure UARFCN Band.
//6. Configure Contiguous Carriers.
//7. Configure Uplink Scrambling (Array).
//8. Select ModAcc,ACP,CHP,OBW and SEM measurements and enable Traces.
//9. Configure Synchronization Mode and Interval for ModAcc
//10. Configure Sweep Time Parameters for ACP.
//11. Configure Averaging Parameters for ACP.
//12. Configure Sweep Time Parameters for CHP. 
//13. Configure Averaging Parameters for CHP.
//14. Configure Sweep Time Parameters for OBW. 
//15. Configure Averaging Parameters for OBW.
//16. Configure Sweep Time Parameters for SEM. 
//17. Configure Averaging Parameters for SEM.
//18. Initiate the Measurement.
//19. Fetch ModAcc, ACP, CHP, SEM & OBW Measurements.
//20. Close RFmx Session.  

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WcdmaMX;

namespace NationalInstruments.Examples.RFmxWcdmaModAccAcpChpObwSemMultiCarrierComposite
{
    public class RFmxWcdmaModAccAcpChpObwSemMultiCarrierComposite
    {
        RFmxInstrMX instrSession;
        RFmxWcdmaMX wcdma;

        string resourceName;
        string selectorString;
        int i;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;
        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;
        bool enableTrigger = false;
        string digitalEdgeSource;
        RFmxWcdmaMXDigitalEdgeTriggerEdge digitalEdge;
        double triggerDelay;
        int band;
        double timeout;

        int carrierAtCenterFrequency;
        const int numberOfCarriers=2;

        RFmxWcdmaMXModAccSynchronizationMode synchronizationMode;
        int measurementOffset;
        int measurementLength;
        RFmxWcdmaMXUplinkScramblingType[] uplinkScramblingType = new RFmxWcdmaMXUplinkScramblingType[numberOfCarriers];
        int[] uplinkScramblingCode = new int[numberOfCarriers];

        RFmxWcdmaMXAcpSweepTimeAuto acpSweepTimeAuto;
        RFmxWcdmaMXChpSweepTimeAuto chpSweepTimeAuto;
        RFmxWcdmaMXObwSweepTimeAuto obwSweepTimeAuto;
        RFmxWcdmaMXSemSweepTimeAuto semSweepTimeAuto;
        double sweepTimeInterval;
        RFmxWcdmaMXAcpAveragingEnabled acpAveragingEnabled;
        RFmxWcdmaMXChpAveragingEnabled chpAveragingEnabled;
        RFmxWcdmaMXObwAveragingEnabled obwAveragingEnabled;
        RFmxWcdmaMXSemAveragingEnabled semAveragingEnabled;
        int averagingCount;
        RFmxWcdmaMXAcpAveragingType acpAveragingType;
        RFmxWcdmaMXChpAveragingType chpAveragingType;
        RFmxWcdmaMXObwAveragingType obwAveragingType;
        RFmxWcdmaMXSemAveragingType semAveragingType;


        double chpTotalCarrierPower;
        double acpTotalCarrierPower;
        double obwOccupiedBandwidth;
        double obwAbsolutePower;
        double obwStopFrequency;
        double obwStartFrequency;
        RFmxWcdmaMXSemMeasurementStatus semMeasurementStatus;
        double semTotalCarrierPower;

        double[] chipRateError;
        double[] frequencyError;
        double[] rmsEvm;
        double[] peakEvm;
        double[] rho;
        double[] rmsPhaseError;
        double[] rmsMagnitudeError;

        double[] chpAbsolutePower; /*(dBm) */
        double[] chpRelativePower; /*(dB) */

        double[] semAbsoluteIntegratedPower;
        double[] semRelativeIntegratedPower;

        double[] acpAbsolutePower;
        double[] acpRelativePower;

        double[] acpLowerAbsolutePower;                /*(dBm) */
        double[] acpUpperAbsolutePower;                /*(dBm) */
        double[] acpLowerRelativePower;                /*(dB) */
        double[] acpUpperRelativePower;                /*(dB) */

        RFmxWcdmaMXSemLowerOffsetMeasurementStatus[] semLowerOffsetMeasurementStatus;
        double[] semLowerOffsetMargin;                 /*(dB) */
        double[] semLowerOffsetMarginFrequency;        /*(Hz) */
        double[] semLowerOffsetMarginAbsolutePower;    /*(dBm) */
        double[] semLowerOffsetMarginRelativePower;    /*(dB) */

        RFmxWcdmaMXSemUpperOffsetMeasurementStatus[] semUpperOffsetMeasurementStatus;
        double[] semUpperOffsetMargin;                 /*(dB) */
        double[] semUpperOffsetMarginFrequency;        /*(Hz) */
        double[] semUpperOffsetMarginAbsolutePower;    /*(dBm) */
        double[] semUpperOffsetMarginRelativePower;    /*(dB) */

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
                CloseSession();
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        private void InitializeInstr()
        {
            /* Create a new RFmx Session */
            instrSession = new RFmxInstrMX(resourceName, "");
        }

         private void InitializeVariables()
        {
            resourceName = "RFSA";
            selectorString = "";
            i = 0;

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e+6;                         /* Hz */
            centerFrequency = 1.95e+9;                   /* Hz */
            referenceLevel = 0.000000;                   /* dBm */
            externalAttenuation = 0.000000;              /* dB */
            enableTrigger = false;
            digitalEdgeSource = RFmxWcdmaMXConstants.Pfi0;
            digitalEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.000000;                     /* seconds */
            band = 1;
            timeout = 10.0;                              /* seconds */

            carrierAtCenterFrequency = -1;

            synchronizationMode = RFmxWcdmaMXModAccSynchronizationMode.Slot;
            measurementOffset = 0;                       /* slots */
            measurementLength = 1;                       /* slots */

            for (i = 0; i < numberOfCarriers; i++)
            {

                uplinkScramblingType[i] = RFmxWcdmaMXUplinkScramblingType.Long;
                uplinkScramblingCode[i] = 0;
            }

            acpSweepTimeAuto = RFmxWcdmaMXAcpSweepTimeAuto.True;
            chpSweepTimeAuto = RFmxWcdmaMXChpSweepTimeAuto.True;
            obwSweepTimeAuto = RFmxWcdmaMXObwSweepTimeAuto.True;
            semSweepTimeAuto = RFmxWcdmaMXSemSweepTimeAuto.True;
            sweepTimeInterval = 0.000667;
            acpAveragingEnabled = RFmxWcdmaMXAcpAveragingEnabled.False;
            chpAveragingEnabled = RFmxWcdmaMXChpAveragingEnabled.False;
            obwAveragingEnabled = RFmxWcdmaMXObwAveragingEnabled.False;
            semAveragingEnabled = RFmxWcdmaMXSemAveragingEnabled.False;
            averagingCount = 10;
            acpAveragingType = RFmxWcdmaMXAcpAveragingType.Rms;
            chpAveragingType = RFmxWcdmaMXChpAveragingType.Rms;
            obwAveragingType = RFmxWcdmaMXObwAveragingType.Rms;
            semAveragingType = RFmxWcdmaMXSemAveragingType.Rms;
        }

        private void ConfigureWcdma()
        {
            wcdma = instrSession.GetWcdmaSignalConfiguration();

            instrSession.ConfigureFrequencyReference(selectorString, frequencyReferenceSource, frequencyReferenceFrequency);

            wcdma.ConfigureRF(selectorString, centerFrequency, referenceLevel, externalAttenuation);
            wcdma.ConfigureDigitalEdgeTrigger(selectorString, digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
            wcdma.ConfigureBand(selectorString, band);
            wcdma.ConfigureContiguousCarriers(selectorString, numberOfCarriers, carrierAtCenterFrequency);
            wcdma.ConfigureUplinkScramblingArray("", uplinkScramblingType, uplinkScramblingCode);
            wcdma.SelectMeasurements(selectorString, RFmxWcdmaMXMeasurementTypes.ModAcc | RFmxWcdmaMXMeasurementTypes.Acp |
                                RFmxWcdmaMXMeasurementTypes.Chp | RFmxWcdmaMXMeasurementTypes.Obw |
                                RFmxWcdmaMXMeasurementTypes.Sem, true);
            wcdma.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength);
            wcdma.Acp.Configuration.ConfigureSweepTime(selectorString, acpSweepTimeAuto, sweepTimeInterval);
            wcdma.Acp.Configuration.ConfigureAveraging(selectorString, acpAveragingEnabled, averagingCount, acpAveragingType);
            wcdma.Chp.Configuration.ConfigureSweepTime(selectorString, chpSweepTimeAuto, sweepTimeInterval);
            wcdma.Chp.Configuration.ConfigureAveraging(selectorString, chpAveragingEnabled, averagingCount, chpAveragingType);
            wcdma.Obw.Configuration.ConfigureSweepTime(selectorString, obwSweepTimeAuto, sweepTimeInterval);
            wcdma.Obw.Configuration.ConfigureAveraging(selectorString, obwAveragingEnabled, averagingCount, obwAveragingType);
            wcdma.Sem.Configuration.ConfigureSweepTime(selectorString, semSweepTimeAuto, sweepTimeInterval);
            wcdma.Sem.Configuration.ConfigureAveraging(selectorString, semAveragingEnabled, averagingCount, semAveragingType);
            wcdma.Initiate(selectorString, "");
        }

        private void RetrieveResults()
        {
            //ModAcc
            wcdma.ModAcc.Results.FetchEvmArray("", timeout, ref rmsEvm, ref peakEvm,
                     ref rho, ref frequencyError, ref chipRateError,
                     ref rmsMagnitudeError, ref rmsPhaseError);

            //ACP
            wcdma.Acp.Results.FetchOffsetMeasurementArray(selectorString, timeout,
                    ref acpLowerRelativePower, ref acpUpperRelativePower,
                    ref acpLowerAbsolutePower, ref acpUpperAbsolutePower);
            wcdma.Acp.Results.FetchCarrierMeasurementArray(selectorString, timeout,
                    ref acpAbsolutePower,
                    ref acpRelativePower);
            wcdma.Acp.Results.FetchTotalCarrierPower(selectorString, timeout, out acpTotalCarrierPower);

            //CHP
            wcdma.Chp.Results.FetchCarrierMeasurementArray(selectorString, timeout,
                          ref chpAbsolutePower,
                          ref chpRelativePower);
            wcdma.Chp.Results.FetchTotalCarrierPower("", timeout, out chpTotalCarrierPower);

            //SEM
            wcdma.Sem.Results.FetchLowerOffsetMarginArray(selectorString, timeout,
                    ref semLowerOffsetMeasurementStatus,
                    ref semLowerOffsetMargin,
                    ref semLowerOffsetMarginFrequency,
                    ref semLowerOffsetMarginAbsolutePower,
                    ref semLowerOffsetMarginRelativePower);
            wcdma.Sem.Results.FetchUpperOffsetMarginArray(selectorString, timeout,
                    ref semUpperOffsetMeasurementStatus,
                    ref semUpperOffsetMargin,
                    ref semUpperOffsetMarginFrequency,
                    ref semUpperOffsetMarginAbsolutePower,
                    ref semUpperOffsetMarginRelativePower);

            wcdma.Sem.Results.FetchCarrierMeasurementArray(selectorString, timeout,
                    ref semAbsoluteIntegratedPower,
                    ref semRelativeIntegratedPower);

            wcdma.Sem.Results.FetchMeasurementStatus("", timeout, out semMeasurementStatus);
            wcdma.Sem.Results.FetchTotalCarrierPower("", timeout, out semTotalCarrierPower);

            //OBW
            wcdma.Obw.Results.FetchMeasurement(selectorString, timeout, out obwOccupiedBandwidth, out obwAbsolutePower, out obwStartFrequency, out obwStopFrequency);

        }

        private void PrintResults()
        {

            Console.WriteLine("************************* ModAcc *************************\n");

            for (i = 0; i < rmsEvm.Length; i++)
            {
                Console.WriteLine("\nMeasurement {0}", i);
                Console.WriteLine("RMS EVM (%)                      : {0}", rmsEvm[i]);
                Console.WriteLine("Peak EVM (%)                     : {0}", peakEvm[i]);
                Console.WriteLine("Rho                              : {0}", rho[i]);
                Console.WriteLine("Frequency Error (Hz)             : {0}", frequencyError[i]);
                Console.WriteLine("Chip Rate Error (ppm)            : {0}", chipRateError[i]);
                Console.WriteLine("RMS Magnitude Error (%)          : {0}", rmsMagnitudeError[i]);
                Console.WriteLine("RMS Phase Error (deg)            : {0}", rmsPhaseError[i]);
            }

            Console.WriteLine("\n************************* ACP *************************\n\n");
            Console.WriteLine("Total Carrier Power  (dBm)       : {0}", acpTotalCarrierPower);
            Console.WriteLine("Carrier Measurements	         : \n");
            for (i = 0; i < acpAbsolutePower.Length; i++)
            {
                Console.WriteLine("Carrier {0}", i);
                Console.WriteLine("Absolute Power  (dBm)            : {0}", acpAbsolutePower[i]);
                Console.WriteLine("Relative Power  (dB)             : {0}", acpRelativePower[i]);
            }
            Console.WriteLine("\nOffset Channel Measurements      : \n");
            for (i = 0; i < acpLowerRelativePower.Length; i++)
            {
                Console.WriteLine("\nOffset {0}", i);
                Console.WriteLine("Lower Relative Power (dB)        : {0}", acpLowerRelativePower[i]);
                Console.WriteLine("Upper Relative Power (dB)        : {0}", acpUpperRelativePower[i]);
                Console.WriteLine("Lower Absolute Power (dBm)       : {0}", acpLowerAbsolutePower[i]);
                Console.WriteLine("Upper Absolute Power (dBm)       : {0}", acpUpperAbsolutePower[i]);
            }

            Console.WriteLine("\n************************* CHP *************************\n\n");
            Console.WriteLine("Total Carrier Power  (dBm)       : {0}", chpTotalCarrierPower);
            Console.WriteLine("Carrier Measurements             : \n");
            for (i = 0; i < chpAbsolutePower.Length; i++)
            {
                Console.WriteLine("Carrier {0}", i);
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
            Console.WriteLine("Total Carrier Power  (dBm)       : {0}", semTotalCarrierPower);
            Console.WriteLine("\nCarrier Measurements	         : \n");
            for (i = 0; i < semAbsoluteIntegratedPower.Length; i++)
            {
                Console.WriteLine("\nCarrier {0}", i);
                Console.WriteLine("Absolute Integrated Power  (dBm) : {0}", semAbsoluteIntegratedPower[i]);
                Console.WriteLine("Relative Integrated Power  (dB)  : {0}", semRelativeIntegratedPower[i]);
            }
            Console.WriteLine("\nLower Offset Segment Measurements: \n");

            for (i = 0; i < semLowerOffsetMargin.Length; i++)
            {
                Console.WriteLine("\nOffset {0}", i);
                Console.WriteLine("Margin  (dB)                     : {0}", semLowerOffsetMargin[i]);
                Console.WriteLine("Margin Absolute Power  (dBm)     : {0}", semLowerOffsetMarginAbsolutePower[i]);
                Console.WriteLine("Margin Frequency  (Hz)           : {0}", semLowerOffsetMarginFrequency[i]);
                Console.WriteLine("Measurement Status               : {0}", semLowerOffsetMeasurementStatus[i]);                
            }
            Console.WriteLine("\nUpper Offset Segment Measurements: \n");

            for (i = 0; i < semUpperOffsetMeasurementStatus.Length; i++)
            {
                Console.WriteLine("\nOffset {0}", i);
                Console.WriteLine("Margin  (dB)                     : {0}", semUpperOffsetMargin[i]);
                Console.WriteLine("Margin Absolute Power  (dBm)     : {0}", semUpperOffsetMarginAbsolutePower[i]);
                Console.WriteLine("Margin Frequency  (Hz)           : {0}", semUpperOffsetMarginFrequency[i]);
                Console.WriteLine("Measurement Status               : {0}", semUpperOffsetMeasurementStatus[i]);           
            }
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

        static private void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }

    }
}
