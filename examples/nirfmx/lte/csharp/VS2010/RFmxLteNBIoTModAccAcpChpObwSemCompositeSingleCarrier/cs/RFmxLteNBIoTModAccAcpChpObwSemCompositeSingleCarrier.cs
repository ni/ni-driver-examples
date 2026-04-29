//// Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier to 200k.
//6. Configure NB-IoT Component Carrier.
//7. Configure NPUSCH Format.
//8. Configure Auto NPUSCH Channel Detection Enabled.
//9. Configure NPUSCH Starting Slot.
//10. Configure NPUSCH DMRS.
//11. Select ACP,ModAcc,OBW,CHP and SEM measurements and enable Traces.
//12. Configure Averaging Parameters for ModAcc.
//13. Configure Averaging Parameters for ACP.
//14. Configure Averaging Parameters for CHP.
//15. Configure Averaging Parameters for OBW.
//16. Configure Averaging Parameters for SEM.
//17. Configure ACP Sweep Time.
//18. Configure CHP Sweep Time.
//19. Configure OBW Sweep Time.
//20. Configure SEM Sweep Time.
//21. Configure ModAcc Synchronization Mode and Measurement Interval.
//22. Initiate the Measurement.
//23. Fetch ModAcc Measurements.
//24. Fetch ACP Measurements.
//25. Fetch SEM Measurements.
//26. Fetch OBW Measurement.
//27. Fetch CHP Measurement.
//28. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteNBIoTModAccAcpChpObwSemCompositeSingleCarrier
{
   public class RFmxLteNBIoTModAccAcpChpObwSemCompositeSingleCarrier
   {
      RFmxInstrMX instrSession;
      RFmxLteMX lte;

      string resourceName, frequencyReferenceSource, iqPowerEdgeTriggerSource;

      double frequencyReferenceFrequency, centerFrequency, referenceLevel, externalAttenuation, triggerDelay,
             sweepTimeInterval, timeout;

      bool enableTrigger;

      int measurementOffset, measurementLength, averagingCount;

      double componentCarrierFrequency;
      double componentCarrierBandwidth;
      int cellID, nCellID;
      int nPuschFormat;

      RFmxLteMXModAccSynchronizationMode synchronizationMode;
      RFmxLteMXNBIoTUplinkSubcarrierSpacing uplinkSubcarrierSpacing;
      RFmxLteMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
      RFmxLteMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
      double iqPowerEdgeTriggerLevel, minimumQuietTimeDuration;
      RFmxLteMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
      double obwOccupiedBandwidth, obwAbsolutePower, obwStopFrequency, obwStartFrequency;

      double[] acpLowerAbsolutePower, acpUpperAbsolutePower, acpLowerRelativePower, acpUpperRelativePower,
               semLowerOffsetMargin, semLowerOffsetMarginFrequency,
               semLowerOffsetMarginAbsolutePower, semLowerOffsetMarginRelativePower, semUpperOffsetMargin,
               semUpperOffsetMarginFrequency, semUpperOffsetMarginAbsolutePower,
               semUpperOffsetMarginRelativePower;
      double modAccMeanRmsCompositeEvm, modAccMaxPeakCompositeEvm, modAccMeanFrequencyError, modAccMeanIQOriginOffset,
               modAccMeanIQGainImbalance, modAccMeanIQQuadratureError, modAccInBandEmissionMargin,
               semAbsoluteIntegratedPower, semRelativeIntegratedPower, chpAbsolutePower, chpRelativePower,
               acpAbsolutePower, acpRelativePower;

      int modAccPeakCompositeEvmSlotIndex, modAccPeakCompositeEvmSymbolIndex,
            modAccPeakCompositeEvmSubcarrierIndex;

      int nPuschDmrsBaseSequenceIndex, nPuschDmrsCyclicShift, nPuschDmrsDeltaSS;

      int nPuschStartingSlot;

      RFmxLteMXAutoNPuschChannelDetectionEnabled autoNPuschChannelDetectionEnabled;

      RFmxLteMXNPuschDmrsBaseSequenceMode nPuschDmrsBaseSequenceMode;
      RFmxLteMXNPuschDmrsGroupHoppingEnabled nPuschDmrsGroupHoppingEnabled;

      RFmxLteMXSemMeasurementStatus semMeasurementStatus;
      RFmxLteMXSemLowerOffsetMeasurementStatus[] semLowerOffsetMeasurementStatus;
      RFmxLteMXSemUpperOffsetMeasurementStatus[] semUpperOffsetMeasurementStatus;

      public void Run()
      {
         try
         {
            InitializeVariables();
            InitializeInstr();
            ConfigureLte();
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

      private void InitializeVariables()
      {
         /* Initialize input variables */

         resourceName = "RFSA";

         centerFrequency = 1.95e+9;          /* Hz */
         referenceLevel = 0.00;              /* dBm */
         externalAttenuation = 0.00;         /* dB */

         componentCarrierFrequency = 0.000;
         componentCarrierBandwidth = 200e3;
         cellID = 0;
         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e+6;                /* Hz */

         iqPowerEdgeTriggerSource = "0";
         iqPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising;
         iqPowerEdgeTriggerLevel = -20.00;

         enableTrigger = true;
         triggerDelay = 0.0;                 /* seconds */
         minimumQuietTimeDuration = 100e-6;  /* seconds */
         minimumQuietTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto;
         iqPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative;

         nPuschStartingSlot = 0;

         nPuschDmrsBaseSequenceIndex = 0;
         nPuschDmrsCyclicShift = 0;
         nPuschDmrsDeltaSS = 0;

         nPuschDmrsBaseSequenceMode = RFmxLteMXNPuschDmrsBaseSequenceMode.Auto;
         nPuschDmrsGroupHoppingEnabled = RFmxLteMXNPuschDmrsGroupHoppingEnabled.False;

         nPuschFormat = 1;
         uplinkSubcarrierSpacing = RFmxLteMXNBIoTUplinkSubcarrierSpacing.SubcarrierSpacing15kHz;
         nCellID = 0;
         synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot;
         measurementOffset = 0;              /* Slots */
         measurementLength = 1;              /* Slots */

         sweepTimeInterval = 0.001;          /* seconds */

         averagingCount = 10;

         autoNPuschChannelDetectionEnabled = RFmxLteMXAutoNPuschChannelDetectionEnabled.True;

         timeout = 10.0;                     /* seconds */
      }

      private void InitializeInstr()
      {
         /* Create a new RFmx Session */
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      private void ConfigureLte()
      {
         /* Get Lte signal */
         lte = instrSession.GetLteSignalConfiguration();

         /* Configure measurement */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

         lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);

         lte.ConfigureIQPowerEdgeTrigger("", iqPowerEdgeTriggerSource, iqPowerEdgeTriggerSlope,
            iqPowerEdgeTriggerLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTimeDuration,
            iqPowerEdgeTriggerLevelType, enableTrigger);

         lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency,
                                        cellID);

         lte.ComponentCarrier.ConfigureNBIoTComponentCarrier("", nCellID, uplinkSubcarrierSpacing);

         lte.ComponentCarrier.ConfigureNPuschFormat("", nPuschFormat);

         lte.ComponentCarrier.ConfigureAutoNPuschChannelDetectionEnabled("", autoNPuschChannelDetectionEnabled);

         lte.ComponentCarrier.ConfigureNPuschStartingSlot("", nPuschStartingSlot);

         lte.ComponentCarrier.ConfigureNPuschDmrs("", nPuschDmrsBaseSequenceMode, nPuschDmrsBaseSequenceIndex,
            nPuschDmrsCyclicShift, nPuschDmrsGroupHoppingEnabled, nPuschDmrsDeltaSS);

         lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Acp | RFmxLteMXMeasurementTypes.Chp |
                                RFmxLteMXMeasurementTypes.ModAcc | RFmxLteMXMeasurementTypes.Obw |
                                RFmxLteMXMeasurementTypes.Sem, true);

         lte.ModAcc.Configuration.ConfigureAveraging("", RFmxLteMXModAccAveragingEnabled.False,
                                                     averagingCount);

         lte.Acp.Configuration.ConfigureAveraging("", RFmxLteMXAcpAveragingEnabled.False, averagingCount,
                                                 RFmxLteMXAcpAveragingType.Rms);

         lte.Chp.Configuration.ConfigureAveraging("", RFmxLteMXChpAveragingEnabled.False, averagingCount,
                                                 RFmxLteMXChpAveragingType.Rms);

         lte.Obw.Configuration.ConfigureAveraging("", RFmxLteMXObwAveragingEnabled.False, averagingCount,
                                                 RFmxLteMXObwAveragingType.Rms);

         lte.Sem.Configuration.ConfigureAveraging("", RFmxLteMXSemAveragingEnabled.False, averagingCount,
                                                 RFmxLteMXSemAveragingType.Rms);

         lte.Acp.Configuration.ConfigureSweepTime("", RFmxLteMXAcpSweepTimeAuto.True, sweepTimeInterval);

         lte.Chp.Configuration.ConfigureSweepTime("", RFmxLteMXChpSweepTimeAuto.True, sweepTimeInterval);

         lte.Obw.Configuration.ConfigureSweepTime("", RFmxLteMXObwSweepTimeAuto.True, sweepTimeInterval);

         lte.Sem.Configuration.ConfigureSweepTime("", RFmxLteMXSemSweepTimeAuto.True, sweepTimeInterval);

         lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
            measurementOffset, measurementLength);

         lte.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         lte.ModAcc.Results.FetchCompositeEvm("", timeout, out modAccMeanRmsCompositeEvm,
                                          out modAccMaxPeakCompositeEvm, out modAccMeanFrequencyError,
                                          out modAccPeakCompositeEvmSymbolIndex,
                                          out modAccPeakCompositeEvmSubcarrierIndex,
                                          out modAccPeakCompositeEvmSlotIndex);

         lte.ModAcc.Results.FetchIQImpairments("", timeout, out modAccMeanIQOriginOffset,
                                      out modAccMeanIQGainImbalance, out modAccMeanIQQuadratureError);

         lte.ModAcc.Results.FetchInBandEmissionMargin("", timeout, out modAccInBandEmissionMargin);

         lte.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref acpLowerRelativePower,
                                                     ref acpUpperRelativePower, ref acpLowerAbsolutePower,
                                                     ref acpUpperAbsolutePower);

         lte.Acp.Results.ComponentCarrier.FetchMeasurement("", timeout, out acpAbsolutePower,
                                                           out acpRelativePower);

         lte.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref semLowerOffsetMeasurementStatus,
                                                     ref semLowerOffsetMargin, ref semLowerOffsetMarginFrequency,
                                                     ref semLowerOffsetMarginAbsolutePower,
                                                     ref semLowerOffsetMarginRelativePower);

         lte.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref semUpperOffsetMeasurementStatus,
                                                     ref semUpperOffsetMargin, ref semUpperOffsetMarginFrequency,
                                                     ref semUpperOffsetMarginAbsolutePower,
                                                     ref semUpperOffsetMarginRelativePower);

         lte.Sem.Results.ComponentCarrier.FetchMeasurement("", timeout, out semAbsoluteIntegratedPower,
                                                           out semRelativeIntegratedPower);

         lte.Sem.Results.FetchMeasurementStatus("", timeout, out semMeasurementStatus);

         lte.Obw.Results.FetchMeasurement("", timeout, out obwOccupiedBandwidth, out obwAbsolutePower,
                                          out obwStartFrequency, out obwStopFrequency);

         lte.Chp.Results.ComponentCarrier.FetchMeasurement("", timeout, out chpAbsolutePower,
                                                           out chpRelativePower);
      }

      private void PrintResults()
      {
         Console.WriteLine("************************* ModAcc *************************\n");
         Console.WriteLine("---------------- Measurements ----------------");
         Console.WriteLine("Mean RMS Composite EVM (% or dB)         : {0}", modAccMeanRmsCompositeEvm);
         Console.WriteLine("Maximum Peak Composite EVM (% or dB)     : {0}", modAccMaxPeakCompositeEvm);
         Console.WriteLine("Peak Composite EVM Slot Index            : {0}", modAccPeakCompositeEvmSlotIndex);
         Console.WriteLine("Peak Composite EVM Symbol Index          : {0}", modAccPeakCompositeEvmSymbolIndex);
         Console.WriteLine("Peak Composite EVM Subcarrier Index      : {0}", modAccPeakCompositeEvmSubcarrierIndex);
         Console.WriteLine("Mean Frequency Error    (Hz)             : {0}", modAccMeanFrequencyError);
         Console.WriteLine("Mean IQ Origin Offset   (dBc)            : {0}", modAccMeanIQOriginOffset);
         Console.WriteLine("Mean IQ Gain Imbalance (dB)              : {0}", modAccMeanIQGainImbalance);
         Console.WriteLine("Mean IQ Quadrature Error (deg)           : {0}", modAccMeanIQQuadratureError);
         Console.WriteLine("In-Band Emission Margin (dB)             : {0}", modAccInBandEmissionMargin);

         Console.WriteLine("\n************************* ACP *************************\n");
         Console.WriteLine("Carrier Absolute Power (dBm)    : {0}", acpAbsolutePower);

         Console.WriteLine("\n------ Offset Channel Measurements -------");
         for (int i = 0; i < acpLowerRelativePower.Length; i++)
         {
            Console.WriteLine("\nOffset  :{0}", i);
            Console.WriteLine("Lower Relative Power (dB)  : {0}", acpLowerRelativePower[i]);
            Console.WriteLine("Upper Relative Power (dB)  : {0}", acpUpperRelativePower[i]);
            Console.WriteLine("Lower Absolute Power (dBm) : {0}", acpLowerAbsolutePower[i]);
            Console.WriteLine("Upper Absolute Power (dBm) : {0}", acpUpperAbsolutePower[i]);
         }

         Console.WriteLine("\n************************* SEM *************************\n");
         Console.WriteLine("Measurement Status                      : {0}", semMeasurementStatus);
         Console.WriteLine("Carrier Absolute Integrated Power (dBm) : {0}", semAbsoluteIntegratedPower);

         Console.WriteLine("\n---- Lower Offset Segment Measurements ---- ");
         for (int i = 0; i < semLowerOffsetMarginAbsolutePower.Length; i++)
         {
            Console.WriteLine("\nOffset  : {0}", i);
            Console.WriteLine("Measurement Status            : {0}", semLowerOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin                 (dB)   : {0}", semLowerOffsetMargin[i]);
            Console.WriteLine("Margin Frequency       (Hz)   : {0}", semLowerOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power  (dBm)  : {0}", semLowerOffsetMarginAbsolutePower[i]);

         }
         Console.WriteLine("\n---- Upper Offset Segment Measurements ---- ");
         for (int i = 0; i < semUpperOffsetMargin.Length; i++)
         {
            Console.WriteLine("\nOffset  :{0}", i);
            Console.WriteLine("Measurement Status            : {0}", semUpperOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin                 (dB)   : {0}", semUpperOffsetMargin[i]);
            Console.WriteLine("Margin Frequency       (Hz)   : {0}", semUpperOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power  (dBm)  : {0}", semUpperOffsetMarginAbsolutePower[i]);
         }

         Console.WriteLine("\n************************* OBW *************************\n");
         Console.WriteLine("---------------- Measurement ----------------");
         Console.WriteLine("Occupied Bandwidth  (Hz)  : {0}", obwOccupiedBandwidth);
         Console.WriteLine("Absolute Power      (dBm) : {0}", obwAbsolutePower);
         Console.WriteLine("Start Frequency     (Hz)  : {0}", obwStartFrequency);
         Console.WriteLine("Stop Frequency      (Hz)  : {0}", obwStopFrequency);

         Console.WriteLine("\n************************* CHP *************************\n");
         Console.WriteLine("Carrier Absolute Power (dBm)   : {0}", chpAbsolutePower);
      }

      private void CloseSession()
      {
         try
         {
            if (lte != null)
            {
               lte.Dispose();
               lte = null;
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
