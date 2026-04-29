// Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure operating Band.
//7. Configure Duplex Mode.
//8. Configure Component Carriers.
//9. Configure Auto DMRS Detection Enabled.
//10. Select ACP,CHP,ModAcc,OBW and SEM measurements and enable Traces.
//11. Configure Averaging Parameters for ModAcc.
//12. Configure Averaging Parameters for ACP.
//13. Configure Averaging Parameters for CHP.
//14. Configure Averaging Parameters for OBW.
//15. Configure Averaging Parameters for SEM.
//16. Configure ACP Sweep Time.
//17. Configure CHP Sweep Time.
//18. Configure OBW Sweep Time.
//19. Configure SEM Sweep Time.
//20. Configure Uplink Mask Type for SEM.
//21. Configure Synchronization Mode and Measurement Interval.
//22. Initiate the Measurement.
//23. Fetch SEM Measurements.
//24. Fetch OBW Measurements.
//25. Fetch CHP Measurements.
//26. Fetch ACP Measurements.
//27. Fetch ModAcc Measurements.
//28. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteULEvmAcpChpObwSemCompositeContiguousMultiCarrier
{
   public class RFmxLteULEvmAcpChpObwSemCompositeContiguousMultiCarrier
   {
      RFmxInstrMX instrSession;
      RFmxLteMX lte;

      string resourceName, frequencyReferenceSource, digitalEdgeSource;

      double frequencyReferenceFrequency, centerFrequency, referenceLevel, externalAttenuation, triggerDelay,
             sweepTimeInterval, timeout;

      bool enableTrigger;

      int band, measurementOffset, measurementLength, averagingCount, componentCarrierAtCenterFrequency;

      const int NumberOfComponentCarriers = 2;
      double[] componentCarrierFrequency = { -9.225e6, 2.475e6 }, componentCarrierBandwidth = { 5e6, 20e6 };
      int[] cellID = { 0, 1 };

      RFmxLteMXDigitalEdgeTriggerEdge digitalEdge;
      RFmxLteMXDuplexScheme duplexScheme;
      RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;
      RFmxLteMXModAccSynchronizationMode synchronizationMode;
      RFmxLteMXSemUplinkMaskType uplinkMaskType;
      RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration;

      RFmxLteMXAutoDmrsDetectionEnabled autoDmrsDetectionEnabled;

      double acpTotalAggregatedPower, chpTotalAggregatedPower, semTotalAggregatedPower,
               obwOccupiedBandwidth, obwAbsolutePower, obwStopFrequency, obwStartFrequency;

      double[] acpLowerAbsolutePower, acpUpperAbsolutePower, acpLowerRelativePower, acpUpperRelativePower,
               acpAbsolutePower, acpRelativePower, chpAbsolutePower, chpRelativePower,
               semAbsoluteIntegratedPower, semRelativeIntegratedPower, semLowerOffsetMargin,
               semLowerOffsetMarginFrequency, semLowerOffsetMarginAbsolutePower, semLowerOffsetMarginRelativePower,
               semUpperOffsetMargin, semUpperOffsetMarginFrequency, semUpperOffsetMarginAbsolutePower,
               semUpperOffsetMarginRelativePower, modAccMeanRmsCompositeEvm, modAccMaxPeakCompositeEvm,
               modAccMeanFrequencyError, modAccMeanIQOriginOffset, modAccMeanIQGainImbalance,
               modAccMeanIQQuadratureError;

      int[] modAccPeakCompositeEvmSlotIndex, modAccPeakCompositeEvmSymbolIndex,
            modAccPeakCompositeEvmSubcarrierIndex;

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

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e+6;                /* Hz */

         enableTrigger = false;
         digitalEdgeSource = RFmxLteMXConstants.Pfi0;
         digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                 /* seconds */

         duplexScheme = RFmxLteMXDuplexScheme.Fdd;

         band = 1;

         componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal;
         componentCarrierAtCenterFrequency = -1;

         synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot;
         measurementOffset = 0;
         measurementLength = 1;

         uplinkMaskType = RFmxLteMXSemUplinkMaskType.General_NS01;
         uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;

         sweepTimeInterval = 0.001;          /* seconds */

         averagingCount = 10;

         autoDmrsDetectionEnabled = RFmxLteMXAutoDmrsDetectionEnabled.True;

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

         lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);

         lte.ComponentCarrier.ConfigureSpacing("", componentCarrierSpacingType,
                                               componentCarrierAtCenterFrequency);

         lte.ConfigureBand("", band);

         lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration);

         lte.ConfigureNumberOfComponentCarriers("", NumberOfComponentCarriers);

         lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth, componentCarrierFrequency,
                                        cellID);

         lte.ConfigureAutoDmrsDetectionEnabled("", autoDmrsDetectionEnabled);

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

         lte.Sem.Configuration.ConfigureUplinkMaskType("", uplinkMaskType);

         lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
                                                                          measurementOffset, measurementLength);

         lte.Initiate("", "");
      }

      private void RetrieveResults()
      {
         /* Retrieve results */

         lte.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref semLowerOffsetMeasurementStatus,
                                                     ref semLowerOffsetMargin, ref semLowerOffsetMarginFrequency,
                                                     ref semLowerOffsetMarginAbsolutePower,
                                                     ref semLowerOffsetMarginRelativePower);

         lte.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref semUpperOffsetMeasurementStatus,
                                                     ref semUpperOffsetMargin, ref semUpperOffsetMarginFrequency,
                                                     ref semUpperOffsetMarginAbsolutePower,
                                                     ref semUpperOffsetMarginRelativePower);

         lte.Sem.Results.ComponentCarrier.FetchMeasurementArray("", timeout, ref semAbsoluteIntegratedPower,
                                                           ref semRelativeIntegratedPower);

         lte.Sem.Results.FetchMeasurementStatus("", timeout, out semMeasurementStatus);

         lte.Sem.Results.FetchTotalAggregatedPower("", timeout, out semTotalAggregatedPower);

         lte.Obw.Results.FetchMeasurement("", timeout, out obwOccupiedBandwidth, out obwAbsolutePower,
                                          out obwStartFrequency, out obwStopFrequency);

         lte.Chp.Results.ComponentCarrier.FetchMeasurementArray("", timeout, ref chpAbsolutePower,
                                                           ref chpRelativePower);

         lte.Chp.Results.FetchTotalAggregatedPower("", timeout, out chpTotalAggregatedPower);

         lte.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref acpLowerRelativePower,
                                                     ref acpUpperRelativePower, ref acpLowerAbsolutePower,
                                                     ref acpUpperAbsolutePower);

         lte.Acp.Results.ComponentCarrier.FetchMeasurementArray("", timeout, ref acpAbsolutePower,
                                                           ref acpRelativePower);

         lte.Acp.Results.FetchTotalAggregatedPower("", timeout, out acpTotalAggregatedPower);

         lte.ModAcc.Results.FetchCompositeEvmArray("", timeout, ref modAccMeanRmsCompositeEvm,
                                              ref modAccMaxPeakCompositeEvm, ref modAccMeanFrequencyError,
                                              ref modAccPeakCompositeEvmSymbolIndex,
                                              ref modAccPeakCompositeEvmSubcarrierIndex,
                                  ref modAccPeakCompositeEvmSlotIndex);

         lte.ModAcc.Results.FetchIQImpairmentsArray("", timeout, ref modAccMeanIQOriginOffset,
                                               ref modAccMeanIQGainImbalance, ref modAccMeanIQQuadratureError);
      }

      private void PrintResults()
      {
         Console.WriteLine("************************* ModAcc *************************\n");
         Console.WriteLine("---------------- Measurements ----------------");
         for (int i = 0; i < modAccMeanRmsCompositeEvm.Length; i++)
         {
            Console.WriteLine("Carrier  : {0}\n", i);
            Console.WriteLine("Mean RMS Composite EVM  (%)    : {0}", modAccMeanRmsCompositeEvm[i]);
            Console.WriteLine("Max Peak Composite EVM  (% )   : {0}", modAccMaxPeakCompositeEvm[i]);
            Console.WriteLine("Mean Frequency Error    (Hz)   : {0}", modAccMeanFrequencyError[i]);
            Console.WriteLine("Mean IQ Origin Offset   (dBc)  : {0}", modAccMeanIQOriginOffset[i]);
         }

         Console.WriteLine("\n************************* CHP *************************\n");
         Console.WriteLine("Total Aggregated Power  (dBm)    : {0}", chpTotalAggregatedPower);
         Console.WriteLine("\n----- Component Carrier Measurements -----");
         for (int i = 0; i < chpAbsolutePower.Length; i++)
         {
            Console.WriteLine("\n Carrier  :{0}", i);
            Console.WriteLine("Absolute Power  (dBm)  : {0}", chpAbsolutePower[i]);
            Console.WriteLine("Relative Power  (dB)   : {0}", chpRelativePower[i]);
         }

         Console.WriteLine("\n************************* ACP *************************\n");
         Console.WriteLine("Total Aggregated Power  (dBm)    : {0}", acpTotalAggregatedPower);
         Console.WriteLine("\n----- Component Carrier Measurements -----");
         for (int i = 0; i < acpAbsolutePower.Length; i++)
         {
            Console.WriteLine("\n Carrier  :{0}", i);
            Console.WriteLine("Absolute Power  (dBm)  : {0}", acpAbsolutePower[i]);
            Console.WriteLine("Relative Power  (dB)   : {0}", acpRelativePower[i]);
         }

         Console.WriteLine("\n------ Offset Channel Measurements -------");
         for (int i = 0; i < acpLowerRelativePower.Length; i++)
         {
            Console.WriteLine("\nOffset  :{0}", i);
            Console.WriteLine("Lower Relative Power (dB)  : {0}", acpLowerRelativePower[i]);
            Console.WriteLine("Upper Relative Power (dB)  : {0}", acpUpperRelativePower[i]);
            Console.WriteLine("Lower Absolute Power (dBm) : {0}", acpLowerAbsolutePower[i]);
            Console.WriteLine("Upper Absolute Power (dBm) : {0}", acpUpperAbsolutePower[i]);
         }

         Console.WriteLine("\n************************* OBW *************************\n");
         Console.WriteLine("---------------- Measurement ----------------");
         Console.WriteLine("Occupied Bandwidth  (Hz)  : {0}", obwOccupiedBandwidth);
         Console.WriteLine("Absolute Power      (dBm) : {0}", obwAbsolutePower);
         Console.WriteLine("Start Frequency     (Hz)  : {0}", obwStartFrequency);
         Console.WriteLine("Stop Frequency      (Hz)  : {0}", obwStopFrequency);

         Console.WriteLine("\n************************* SEM *************************\n");
         Console.WriteLine("Measurement Status               : {0}", semMeasurementStatus);
         Console.WriteLine("Total Aggregated Power  (dBm)    : {0}", semTotalAggregatedPower);
         Console.WriteLine("\n-----Component Carrier Measurements ------");
         for (int i = 0; i < semAbsoluteIntegratedPower.Length; i++)
         {
            Console.WriteLine("Carrier  : {0}", i);
            Console.WriteLine("Absolute Integrated Power  (dBm)  : {0}", semAbsoluteIntegratedPower[i]);
            Console.WriteLine("Relative Integrated Power  (dB)   : {0}", semRelativeIntegratedPower[i]);
         }

         Console.WriteLine("\n---- Lower Offset Segment Measurements ---- ");
         for (int i = 0; i < semLowerOffsetMargin.Length; i++)
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
