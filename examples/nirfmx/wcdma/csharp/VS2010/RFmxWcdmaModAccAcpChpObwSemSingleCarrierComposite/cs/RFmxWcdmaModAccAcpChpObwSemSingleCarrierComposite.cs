//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure UARFCN Band.
//6. Configure Uplink Scrambling.
//7. Select ACP,CHP,ModAcc,OBW and SEM measurements and enable Traces.
//8. Configure Synchronization Mode and Measurement Interval.
//9. Configure Sweep Time Parameters for ACP. 
//10. Configure Averaging Parameters for ACP.
//11. Configure Sweep Time Parameters for CHP. 
//12. Configure Averaging Parameters for CHP.
//13. Configure Sweep Time Parameters for OBW. 
//14. Configure Averaging Parameters for OBW.
//15. Configure Sweep Time Parameters for SEM. 
//16. Configure Averaging Parameters for SEM.
//17. Initiate the Measurement.
//18. Fetch ACP,SEM,ModAcc,CHP & OBW Measurements.
//19. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.WcdmaMX;

namespace NationalInstruments.Examples.RFmxWcdmaModAccAcpChpObwSemSingleCarrierComposite
{
    public class RFmxWcdmaModAccAcpChpObwSemSingleCarrierComposite
    {
        RFmxInstrMX instrSession;
        RFmxWcdmaMX wcdma;

        string resourceName = "RFSA";
        int i;

        string frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
        double frequencyReferenceFrequency = 10.0e+6;                 /* Hz */
        double centerFrequency = 1.95e+9;                             /* Hz */
        double referenceLevel = 0.000000;                             /* dBm */
        double externalAttenuation = 0.000000;                        /* dB */

        bool enableTrigger = false;
        string digitalEdgeSource = RFmxWcdmaMXConstants.Pfi0;
        RFmxWcdmaMXDigitalEdgeTriggerEdge digitalEdge = RFmxWcdmaMXDigitalEdgeTriggerEdge.Rising;
        double triggerDelay = 0.000000;

        int numberOfCarriers = 2;
        int band = 1;
        int carrierAtCenterFrequency = -1;

        RFmxWcdmaMXUplinkScramblingType uplinkScramblingType = RFmxWcdmaMXUplinkScramblingType.Long;
        int uplinkScramblingCode = 0x0;
       
        RFmxWcdmaMXModAccSynchronizationMode synchronizationMode = RFmxWcdmaMXModAccSynchronizationMode.Slot;
        int measurementOffset = 0;                                  /* slots */
        int measurementLength = 1;                                  /* slots */

        RFmxWcdmaMXAcpSweepTimeAuto acpSweepTimeAuto = RFmxWcdmaMXAcpSweepTimeAuto.True;
        RFmxWcdmaMXChpSweepTimeAuto chpSweepTimeAuto = RFmxWcdmaMXChpSweepTimeAuto.True;
        RFmxWcdmaMXObwSweepTimeAuto obwSweepTimeAuto = RFmxWcdmaMXObwSweepTimeAuto.True;
        RFmxWcdmaMXSemSweepTimeAuto semSweepTimeAuto = RFmxWcdmaMXSemSweepTimeAuto.True;
        double sweepTimeInterval = 6.67e-4;         /* seconds */

        RFmxWcdmaMXAcpAveragingEnabled acpAveragingEnabled = RFmxWcdmaMXAcpAveragingEnabled.False;
        RFmxWcdmaMXChpAveragingEnabled chpAveragingEnabled = RFmxWcdmaMXChpAveragingEnabled.False;
        RFmxWcdmaMXObwAveragingEnabled obwAveragingEnabled = RFmxWcdmaMXObwAveragingEnabled.False;
        RFmxWcdmaMXSemAveragingEnabled semAveragingEnabled = RFmxWcdmaMXSemAveragingEnabled.False;
        int averagingCount = 10;
        RFmxWcdmaMXAcpAveragingType acpAveragingType = RFmxWcdmaMXAcpAveragingType.Rms;
        RFmxWcdmaMXChpAveragingType chpAveragingType = RFmxWcdmaMXChpAveragingType.Rms;
        RFmxWcdmaMXObwAveragingType obwAveragingType = RFmxWcdmaMXObwAveragingType.Rms;
        RFmxWcdmaMXSemAveragingType semAveragingType = RFmxWcdmaMXSemAveragingType.Rms;

        double acpAbsolutePower;
        double acpRelativePower;
        double timeout = 10.000000;                 /* seconds */
        double chipRateError;
        double frequencyError;
        double rmsEvm;
        double peakEvm;
        double rho;
        double rmsPhaseError;
        double rmsMagnitudeError;
        RFmxWcdmaMXSemMeasurementStatus semMeasurementStatus;
        double chpAbsolutePower;
        double chpRelativePower;
        double obwAbsolutePower;
        double obwStopFrequency;
        double obwStartFrequency;
        double obwOccupiedBandwidth;
        double semAbsoluteIntegratedPower;
        double semRelativeIntegratedPower;


        double[] acpLowerAbsolutePower;
        /*(dBm) */
        double[] acpUpperAbsolutePower;
        /*(dBm) */
        double[] acpLowerRelativePower;
        /*(dB) */
        double[] acpUpperRelativePower;

        double[] semLowerOffsetMarginRelativePower;
        /*(dB) */
        double[] semLowerOffsetMarginAbsolutePower;
        /*(dBm) */
        RFmxWcdmaMXSemLowerOffsetMeasurementStatus[] semLowerOffsetMeasurementStatus;

        double[] semLowerOffsetMargin;
        /*(dB) */
        double[] semLowerOffsetMarginFrequency;
        /*(Hz) */
        double[] semUpperOffsetMarginRelativePower;
        /*(dB) */
        double[] semUpperOffsetMarginAbsolutePower;
        /*(dBm) */
        RFmxWcdmaMXSemUpperOffsetMeasurementStatus[] semUpperOffsetMeasurementStatus;

        double[] semUpperOffsetMargin;
        /*(dB) */
        double[] semUpperOffsetMarginFrequency;
        /*(Hz) */

        public void Run()
        {
            try
            {
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

        private void ConfigureWcdma()
        {
            wcdma = instrSession.GetWcdmaSignalConfiguration();
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            wcdma.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
            wcdma.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
            wcdma.ConfigureBand("", band);
            wcdma.ConfigureContiguousCarriers("", numberOfCarriers, carrierAtCenterFrequency);
            wcdma.ConfigureUplinkScrambling("", uplinkScramblingCode, uplinkScramblingType);
            wcdma.SelectMeasurements("", RFmxWcdmaMXMeasurementTypes.Acp |
                                RFmxWcdmaMXMeasurementTypes.Chp | RFmxWcdmaMXMeasurementTypes.Obw |
                                RFmxWcdmaMXMeasurementTypes.Sem | RFmxWcdmaMXMeasurementTypes.ModAcc, true);
            wcdma.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode, measurementOffset, measurementLength);
            wcdma.Acp.Configuration.ConfigureSweepTime("", acpSweepTimeAuto, sweepTimeInterval);
            wcdma.Acp.Configuration.ConfigureAveraging("", acpAveragingEnabled, averagingCount,
               acpAveragingType);
            wcdma.Chp.Configuration.ConfigureSweepTime("", chpSweepTimeAuto, sweepTimeInterval);
            wcdma.Chp.Configuration.ConfigureAveraging("", chpAveragingEnabled, averagingCount,
                chpAveragingType);
            wcdma.Obw.Configuration.ConfigureSweepTime("", obwSweepTimeAuto, sweepTimeInterval);
            wcdma.Obw.Configuration.ConfigureAveraging("", obwAveragingEnabled, averagingCount,
                obwAveragingType);
            wcdma.Sem.Configuration.ConfigureSweepTime("", semSweepTimeAuto, sweepTimeInterval);
            wcdma.Sem.Configuration.ConfigureAveraging("", semAveragingEnabled, averagingCount,
                semAveragingType);
            wcdma.Initiate("", "");
        }

        private void RetrieveResults()
        {
            wcdma.Acp.Results.FetchOffsetMeasurementArray("", timeout,
                    ref acpLowerRelativePower,
                    ref acpUpperRelativePower,
                    ref acpLowerAbsolutePower,
                    ref acpUpperAbsolutePower);


            wcdma.Sem.Results.FetchLowerOffsetMarginArray("", timeout,
                    ref semLowerOffsetMeasurementStatus,
                    ref semLowerOffsetMargin,
                    ref semLowerOffsetMarginFrequency,
                    ref semLowerOffsetMarginAbsolutePower,
                    ref semLowerOffsetMarginRelativePower);

            wcdma.Sem.Results.FetchUpperOffsetMarginArray("", timeout,
                    ref semUpperOffsetMeasurementStatus,
                    ref semUpperOffsetMargin,
                    ref semUpperOffsetMarginFrequency,
                    ref semUpperOffsetMarginAbsolutePower,
                    ref semUpperOffsetMarginRelativePower);


            wcdma.ModAcc.Results.FetchEvm("", timeout, out rmsEvm, out peakEvm, out rho, out frequencyError, out chipRateError, out rmsMagnitudeError, out rmsPhaseError);
            wcdma.Sem.Results.FetchMeasurementStatus("", timeout, out semMeasurementStatus);
            wcdma.Acp.Results.FetchCarrierMeasurement("", timeout, out acpAbsolutePower, out acpRelativePower);
            wcdma.Chp.Results.FetchCarrierMeasurement("", timeout, out chpAbsolutePower, out chpRelativePower);
            wcdma.Obw.Results.FetchMeasurement("", timeout, out obwOccupiedBandwidth, out obwAbsolutePower, out obwStartFrequency, out obwStopFrequency);
            wcdma.Sem.Results.FetchCarrierMeasurement("", timeout, out semAbsoluteIntegratedPower, out semRelativeIntegratedPower);

        }

        private void PrintResults()
        {
            Console.WriteLine("************************* ModAcc *************************\n");
            Console.WriteLine("RMS EVM (%)                    : {0}", rmsEvm);
            Console.WriteLine("Peak EVM (%)                   : {0}", peakEvm);
            Console.WriteLine("Rho                            : {0}", rho);
            Console.WriteLine("Frequency Error (Hz)           : {0}", frequencyError);
            Console.WriteLine("Chip Rate Error (ppm)          : {0}", chipRateError);
            Console.WriteLine("RMS Magnitude Error (%)        : {0}", rmsMagnitudeError);
            Console.WriteLine("RMS Phase Error (deg)          : {0}", rmsPhaseError);

            Console.WriteLine("\n************************* ACP *************************\n");
            Console.WriteLine("Carrier Absolute Power (dBm)    : {0}", acpAbsolutePower);
            for (i = 0; i < acpLowerRelativePower.Length; i++)
            {
                Console.WriteLine("\nOffset Channel Measurements	:  {0}", i);
                Console.WriteLine("Lower Relative Power (dB)	:  {0}", acpLowerRelativePower[i]);
                Console.WriteLine("Upper Relative Power (dB)	:  {0}", acpUpperRelativePower[i]);
                Console.WriteLine("Lower Absolute Power (dBm)	:  {0}", acpLowerAbsolutePower[i]);
                Console.WriteLine("Upper Absolute Power (dBm)	:  {0}", acpUpperAbsolutePower[i]);
            }

            Console.WriteLine("\n************************* CHP *************************\n");
            Console.WriteLine("Carrier Absolute Power (dBm)    :  {0}", chpAbsolutePower);

            Console.WriteLine("\n************************* OBW *************************\n");
            Console.WriteLine("Occupied Bandwidth (Hz)		:  {0}", obwOccupiedBandwidth);
            Console.WriteLine("Absoulte Power (dBm)		:  {0}", obwAbsolutePower);
            Console.WriteLine("Start Frequency (Hz)		:  {0}", obwStartFrequency);
            Console.WriteLine("Stop Frequency (Hz)		:  {0}", obwStopFrequency);

            Console.WriteLine("\n************************* SEM *************************\n");
            Console.WriteLine("Measurement Status		:  {0}", semMeasurementStatus);
            Console.WriteLine("Carrier Absolute Integrated Power (dBm)	:  {0}", semAbsoluteIntegratedPower);

            Console.WriteLine("\n---------------Lower Offset---------------\n");
            for (i = 0; i < semLowerOffsetMargin.Length; i++)
            {
                Console.WriteLine("\nOffset Channel Measurements	:  {0}", i);
                Console.WriteLine("Margin (dB)			:  {0}", semLowerOffsetMargin[i]);
                Console.WriteLine("Margin Absolute Power (dBm)	:  {0}", semLowerOffsetMarginAbsolutePower[i]);
                Console.WriteLine("Margin Relative Power (dB)	:  {0}", semLowerOffsetMarginRelativePower[i]);
                Console.WriteLine("Margin Frequency (Hz)		:  {0}", semLowerOffsetMarginFrequency[i]);
                Console.WriteLine("Measurement Status		:  {0}", semLowerOffsetMeasurementStatus[i]);
            }

            Console.WriteLine("\n---------------Upper Offset---------------\n");
            for (i = 0; i < semUpperOffsetMargin.Length; i++)
            {
                Console.WriteLine("\nOffset Channel Measurements	:  {0}", i);
                Console.WriteLine("Margin (dB)			:  {0}", semUpperOffsetMargin[i]);
                Console.WriteLine("Margin Absolute Power (dBm)	:  {0}", semUpperOffsetMarginAbsolutePower[i]);
                Console.WriteLine("Margin Relative Power (dB)	:  {0}", semUpperOffsetMarginRelativePower[i]);
                Console.WriteLine("Margin Frequency (Hz)		:  {0}", semUpperOffsetMarginFrequency[i]);
                Console.WriteLine("Measurement Status		:  {0}", semUpperOffsetMeasurementStatus[i]);
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
