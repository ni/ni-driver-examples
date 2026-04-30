//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Band Class Parameter.
//6. Configure Radio Configuration Parameter.
//7. Configure Uplink Spreading Long Code Mask Parameter.
//8. Select ModAcc, ACP, CHP, OBW and SEM measurements and enable Traces.
//9. Configure Synchronization Mode and Measurement Interval.
//10. Configure Sweep Time Parameters.
//11. Configure Averaging Parameters.
//12. Initiate the Measurement.
//13. Fetch SEM Measurements and Traces.
//14. Fetch OBW Measurements and Traces.
//15. Fetch CHP Measurements and Traces.
//16. Fetch ACP Measurements and Traces.
//17. Fetch ModAcc Measurements and Traces.
//18. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.Cdma2kMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxCdma2kModAccAcpChpObwSemComposite
{
    public class RFmxCdma2kModAccAcpChpObwSemComposite
    {
        RFmxInstrMX instrSession;
        RFmxCdma2kMX cdma2k;
        string resourceName, frequencySource;
        double centerFrequency, referenceLevel, externalAttenuation, frequency, sweepTimeInterval;

        int averagingCount;

        string digitalEdgeTriggerSource;
        RFmxCdma2kMXDigitalEdgeTriggerEdge digitalEdgeTriggerEdge;
        double triggerDelay;
        bool enableTrigger;

        int bandclass;

        RFmxCdma2kMXRadioConfiguration radioConfiguration;

		RFmxCdma2kMXMeasurementTypes measMask;

		long uplinkSpreadingLongCodeMask;

        RFmxCdma2kMXModAccSynchronizationMode ModAccSynchronizationMode;
        RFmxCdma2kMXCdaSynchronizationMode CdaSynchronizationMode;
        RFmxCdma2kMXSlotPhaseSynchronizationMode SlotPhaseSynchronizationMode;
        RFmxCdma2kMXSlotPowerSynchronizationMode SlotPowerSynchronizationMode;

        int measurementOffset, ModAccMeasurementLength, CdaMeasurementLength, SlotPhaseMeasurementLength;
        int SlotPowerMeasurementLength, QevmMeasurementLength;

        double timeout;

        //modacc results
        double rmsEvm, peakEvm, rho, frequencyError, chipRateError, rmsMagnitudeError, rmsPhaseError;

        //ACP results
        double carrierAbsolutePowerAcp;
        double[] lowerRelativePower;
        double[] upperRelativePower;
        double[] lowerAbsolutePower;
        double[] upperAbsolutePower;

        //CHP results
        double carrierAbsolutePowerChp;

        //OBW results
        double stopFrequency, startFrequency, occupiedBandwidth, absolutePower;

        //SEM results
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

        //CDA results
        double iMeanActivePower,qMeanActivePower,iPeakInactivePower;
        double qPeakInactivePower,totalPower,totalActivePower,meanActivePower;
        double peakActivePower,meanInactivePower,peakInactivePower;
        double iqOriginOffset,iqQuadratureError,iqGainImbalance;
        double rmsSymbolEvm,peakSymbolEvm,rmsSymbolMagnitudeError;
        double rmsSymbolPhaseError,CdaFrequencyError,CdaChipRateError;
        double meanSymbolPower;

        //QEVM results
        double meanRmsEvm;
        double maximumPeakEvm;
        double meanFrequencyError;
        double meanMagnitudeError;
        double meanPhaseError;
        double meanChipRateError;


        //SlotPhase results
        double[] slotPhaseDiscontinuities;
        double maxPhaseDiscontinuity;


        //SlotPower results
        double[] slotPower;
        double[] slotPowerDelta;


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

            centerFrequency = 833.49e+6;                                            /* Hz */
            referenceLevel = 0.00;                                                  /* dBm */
            externalAttenuation = 0.00;                                             /* dB */

            frequencySource = RFmxInstrMXConstants.OnboardClock;
            frequency = 10e+6;                                                      /* Hz */

            triggerDelay = 0.00;                                                    /* seconds */

            digitalEdgeTriggerSource = RFmxCdma2kMXConstants.Pfi0;
            digitalEdgeTriggerEdge = RFmxCdma2kMXDigitalEdgeTriggerEdge.Rising;
            enableTrigger = false;

            bandclass = 0;
            radioConfiguration = RFmxCdma2kMXRadioConfiguration.RC3;
            uplinkSpreadingLongCodeMask = 0;

			//select measurements
			measMask = RFmxCdma2kMXMeasurementTypes.ModAcc |
                       RFmxCdma2kMXMeasurementTypes.Acp |
                       RFmxCdma2kMXMeasurementTypes.Chp |
                       RFmxCdma2kMXMeasurementTypes.Obw |
                       RFmxCdma2kMXMeasurementTypes.Sem |
                       RFmxCdma2kMXMeasurementTypes.Cda |
                       RFmxCdma2kMXMeasurementTypes.Qevm |
                       RFmxCdma2kMXMeasurementTypes.SlotPhase |
                       RFmxCdma2kMXMeasurementTypes.SlotPower;

			//Sweep Time
			sweepTimeInterval = 1.67e-3;                                            /* seconds */

            //Averaging
            averagingCount = 10;

            //SynchronizationModes
            ModAccSynchronizationMode = RFmxCdma2kMXModAccSynchronizationMode.Slot;
            CdaSynchronizationMode = RFmxCdma2kMXCdaSynchronizationMode.Slot;
            SlotPhaseSynchronizationMode = RFmxCdma2kMXSlotPhaseSynchronizationMode.Slot;
            SlotPowerSynchronizationMode = RFmxCdma2kMXSlotPowerSynchronizationMode.Slot;

            measurementOffset = 0;
            ModAccMeasurementLength = 1;                                            /* slots */
            CdaMeasurementLength = 1;                                               /* slots */
            SlotPhaseMeasurementLength = 1;                                         /* slots */
            SlotPowerMeasurementLength = 1;                                         /* slots */
            QevmMeasurementLength = 700;                                            /* chips */

            timeout = 10;                                                           /* seconds */
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
            cdma2k.ConfigureRadioConfiguration("", radioConfiguration);
            cdma2k.ConfigureUplinkSpreading("", uplinkSpreadingLongCodeMask);

            cdma2k.SelectMeasurements("", measMask, false);

            cdma2k.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", ModAccSynchronizationMode,
                                                                                measurementOffset, ModAccMeasurementLength);

            cdma2k.Acp.Configuration.ConfigureSweepTime("", RFmxCdma2kMXAcpSweepTimeAuto.False, sweepTimeInterval); 
            cdma2k.Acp.Configuration.ConfigureAveraging("", RFmxCdma2kMXAcpAveragingEnabled.False, averagingCount, 
                                                        RFmxCdma2kMXAcpAveragingType.Rms);

            cdma2k.Chp.Configuration.ConfigureSweepTime("", RFmxCdma2kMXChpSweepTimeAuto.False, sweepTimeInterval);
            cdma2k.Chp.Configuration.ConfigureAveraging("", RFmxCdma2kMXChpAveragingEnabled.False, averagingCount, 
                                                        RFmxCdma2kMXChpAveragingType.Rms);

            cdma2k.Obw.Configuration.ConfigureSweepTime("", RFmxCdma2kMXObwSweepTimeAuto.False, sweepTimeInterval);
            cdma2k.Obw.Configuration.ConfigureAveraging("", RFmxCdma2kMXObwAveragingEnabled.False, averagingCount, 
                                                         RFmxCdma2kMXObwAveragingType.Rms);

            cdma2k.Sem.Configuration.ConfigureSweepTime("", RFmxCdma2kMXSemSweepTimeAuto.False, sweepTimeInterval);
            cdma2k.Sem.Configuration.ConfigureAveraging("", RFmxCdma2kMXSemAveragingEnabled.False, averagingCount,
                                                        RFmxCdma2kMXSemAveragingType.Rms);

            cdma2k.Cda.Configuration.ConfigureSynchronizationModeAndInterval("", CdaSynchronizationMode, measurementOffset, 
                                                                             CdaMeasurementLength);

            cdma2k.SlotPhase.Configuration.ConfigureSynchronizationModeAndInterval("", SlotPhaseSynchronizationMode, 
                                                                                  measurementOffset, SlotPhaseMeasurementLength);

            cdma2k.SlotPower.Configuration.ConfigureSynchronizationModeAndInterval("", SlotPowerSynchronizationMode,
                                                                           measurementOffset, SlotPowerMeasurementLength);

            cdma2k.Qevm.Configuration.ConfigureAveraging("", RFmxCdma2kMXQevmAveragingEnabled.False, averagingCount);
            cdma2k.Qevm.Configuration.ConfigureMeasurementLength("", QevmMeasurementLength);

            cdma2k.Initiate("", "");
        }

        private void RetrieveResults()
        {
            /* Retrieve results */

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
            cdma2k.Sem.Results.FetchCarrierAbsoluteIntegratedPower("", timeout, out carrierAbsoluteIntegratedPower);
            cdma2k.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);

            cdma2k.Obw.Results.FetchMeasurement("", timeout,
                                                out occupiedBandwidth,
                                                out absolutePower,
                                                out startFrequency,
                                                out stopFrequency);

            cdma2k.Chp.Results.FetchCarrierAbsolutePower("", timeout, out carrierAbsolutePowerChp);

            cdma2k.Acp.Results.FetchOffsetMeasurementArray("", timeout,
                                                           ref lowerRelativePower,
                                                           ref upperRelativePower,
                                                           ref lowerAbsolutePower,
                                                           ref upperAbsolutePower);

            cdma2k.Acp.Results.FetchCarrierAbsolutePower("", timeout, out carrierAbsolutePowerAcp);

            cdma2k.ModAcc.Results.FetchEvm("", timeout, out rmsEvm,
                                            out peakEvm,
                                            out rho,
                                            out frequencyError,
                                            out chipRateError,
                                            out rmsMagnitudeError,
                                            out rmsPhaseError);

            cdma2k.Cda.Results.FetchCodeDomainIAndQPower("", timeout, 
                                                         out iMeanActivePower,
                                                         out qMeanActivePower,
                                                         out iPeakInactivePower,
                                                         out qPeakInactivePower);

            cdma2k.Cda.Results.FetchCodeDomainPower("", timeout, 
                                                    out totalPower,
                                                    out totalActivePower,
                                                    out meanActivePower,
                                                    out peakActivePower,
                                                    out meanInactivePower,
                                                    out peakInactivePower);
            cdma2k.Cda.Results.FetchIQImpairments("", timeout,
                                                  out iqOriginOffset,
                                                  out iqGainImbalance,
                                                  out iqQuadratureError);

            cdma2k.Cda.Results.FetchSymbolEvm("", timeout,
                                              out rmsSymbolEvm,
                                              out peakSymbolEvm,
                                              out rmsSymbolMagnitudeError,
                                              out rmsSymbolPhaseError,
                                              out meanSymbolPower,
                                              out CdaFrequencyError,
                                              out CdaChipRateError);

            cdma2k.Qevm.Results.FetchEvm("", timeout, out meanRmsEvm,
                                            out maximumPeakEvm,
                                            out meanFrequencyError,
                                            out meanMagnitudeError,
                                            out meanPhaseError,
                                            out meanChipRateError);

            cdma2k.SlotPhase.Results.FetchPhaseDiscontinuities("", timeout, ref slotPhaseDiscontinuities);
            cdma2k.SlotPhase.Results.FetchMaximumPhaseDiscontinuity("", timeout, out maxPhaseDiscontinuity);

            cdma2k.SlotPower.Results.FetchPowers("", timeout, ref slotPower, ref slotPowerDelta);

        }

        private void PrintResults()
        {
            Console.WriteLine("-----------------------------ModAcc Results----------------------------\n");
            Console.WriteLine("-------------------Composite EVM Results--------------------");
            Console.WriteLine("RMS EVM (%)                               : {0}", rmsEvm);
            Console.WriteLine("Peak EVM (%)                              : {0}", peakEvm);
            Console.WriteLine("Rho                                       : {0}", rho);
            Console.WriteLine("Frequency Error (Hz)                      : {0}", frequencyError);
            Console.WriteLine("Chip Rate Error (ppm)                     : {0}", chipRateError);
            Console.WriteLine("RMS Phase Error (deg)                     : {0}", rmsPhaseError);
            Console.WriteLine("RMS Magnitude Error (%)                   : {0}", rmsMagnitudeError);



            Console.WriteLine("----------------------------------ACP Results----------------------------\n");

            Console.WriteLine("---------------------Carrier Measurements-----------------------\n");
            Console.WriteLine("Carrier Absolute Power (dBm)              : {0}", carrierAbsolutePowerAcp);

            Console.WriteLine("\n-----------------Offset Channel Measurements------------------\n");

            for (int i = 0; i < lowerRelativePower.Length; i++)
            {
                Console.WriteLine("Offset                                    : {0}\n", i);
                Console.WriteLine("Lower Relative Power (dB)                 : {0}", lowerRelativePower[i]);
                Console.WriteLine("Upper Relative Power (dB)                 : {0}", upperRelativePower[i]);
                Console.WriteLine("Lower Absolute Power (dBm)                : {0}", lowerAbsolutePower[i]);
                Console.WriteLine("Upper Absolute Power (dBm)                : {0}", upperAbsolutePower[i]);
            }
            Console.WriteLine("-----------------------------------CHP Results-------------------------------");
            Console.WriteLine("Carrier Absolute Power (dBm)              : {0}", carrierAbsolutePowerChp);


            Console.WriteLine("------------------------------------OBW Results------------------------------\n");
            Console.WriteLine("Occupied Bandwidth (Hz)                   : {0}", occupiedBandwidth);
            Console.WriteLine("Absolute Power (dBm)                      : {0}", absolutePower);
            Console.WriteLine("Start Frequency (Hz)                      : {0}", startFrequency);
            Console.WriteLine("Stop Frequency (Hz)                       : {0}", stopFrequency);

            Console.WriteLine("--------------------------------------SEM Results----------------------------\n");
            Console.WriteLine("Composite measurement status              : {0}\n", measurementStatus);

            Console.WriteLine("\n---------------------------Carrier Measurement------------------------\n");
            Console.WriteLine("Carrier Absolute Integrated Power (dBm)   : {0}", carrierAbsoluteIntegratedPower);

            Console.WriteLine("\n--------------Offset segment measurements ---------------------------\n");
            Console.WriteLine("\n Lower Offset segment measurements");
            for (int i = 0; i < lowerOffsetMargin.Length; i++)
            {
                Console.WriteLine("Offset                                    : {0}\n", i);

                Console.WriteLine("Lower Offset : Margin (dB)                : {0}",
                                  lowerOffsetMargin[i]);
                Console.WriteLine("Lower offset : Margin Absolute Power (dBm): {0}",
                                  lowerOffsetMarginAbsolutePower[i]);
                Console.WriteLine("Lower offset : Margin Relative Power (dB) : {0}",
                                  lowerOffsetMarginRelativePower[i]);
                Console.WriteLine("Lower offset : Margin Frequency (Hz)      : {0}",
                                  lowerOffsetMarginFrequency[i]);
                Console.WriteLine("Lower offset : Measurement Status         : {0}\n", lowerOffsetMeasurementStatus[i]);
            }
            Console.WriteLine("\n\n");
            Console.WriteLine("\n Upper Offset segment measurements");
            for (int i = 0; i < upperOffsetMargin.Length; i++)
            {
                Console.WriteLine("Offset                                    : {0}\n", i);
                Console.WriteLine("Upper Offset : Margin (dB)                : {0}",
                                  upperOffsetMargin[i]);
                Console.WriteLine("Upper offset : Margin Absolute Power (dBm): {0}",
                                  upperOffsetMarginAbsolutePower[i]);
                Console.WriteLine("Upper offset : Margin Relative Power (dB) : {0}",
                                  upperOffsetMarginRelativePower[i]);
                Console.WriteLine("Upper offset : Margin Frequency (Hz)      : {0}",
                                  upperOffsetMarginFrequency[i]);
                Console.WriteLine("Upper offset : Measurement Status         : {0}\n",
                    upperOffsetMeasurementStatus[i]);
            }
       
            Console.WriteLine("------------------------------------CDA Results------------------------------\n");
            Console.WriteLine("RMS Symbol EVM (dB or %)                  : {0}", rmsSymbolEvm);
            Console.WriteLine("Peak Symbol EVM (dB or %)                 : {0}", peakSymbolEvm);
            Console.WriteLine("RMS Symbol Magnitude Error (dB)           : {0}", rmsSymbolMagnitudeError);
            Console.WriteLine("RMS Symbol Phase Error (deg)              : {0}", rmsSymbolPhaseError);
            Console.WriteLine("Mean Symbol Power (Hz)                    : {0}", meanSymbolPower);
            Console.WriteLine("Frequency Error (Hz)                      : {0}", CdaFrequencyError);
            Console.WriteLine("Chiprate Error (Hz)                       : {0}", CdaChipRateError);
          
            Console.WriteLine("------------------------------------QEVM Results------------------------------\n");
            Console.WriteLine("Max. Peak EVM (dB or %)                   : {0}", maximumPeakEvm);
            Console.WriteLine("Mean Frequency Error (Hz)                 : {0}", meanFrequencyError);
            Console.WriteLine("Mean Magnitude Error (dB)                 : {0}", meanMagnitudeError);
            Console.WriteLine("Mean Phase Error (deg)                    : {0}", meanPhaseError);
            Console.WriteLine("Mean Chiprate Error (Hz)                  : {0}", meanChipRateError);
            
            Console.WriteLine("------------------------------------SlotPower Results------------------------------\n");
            for (int i = 0; i < slotPower.Length; i++)
            {
                Console.WriteLine("Slot Power (dBm)                          : {0}", slotPower[i]);
                Console.WriteLine("Slot Power Delta (dB)                     : {0}", slotPowerDelta[i]);
            }
            

            Console.WriteLine("------------------------------------SlotPhase Results------------------------------\n");
            for (int i = 0; i < slotPhaseDiscontinuities.Length; i++)
            {
                Console.WriteLine("Slot Phase Discontinuity (deg)            : {0}", slotPhaseDiscontinuities[i]);
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

        static private void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }
    }
}
