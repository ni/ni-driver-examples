//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier.
//6. Configure operating Band.
//7. Configure Duplex Mode.
//8. Configure Auto DMRS Detection Enabled.
//9. Select ACP,CHP,ModAcc,OBW and SEM measurements and enable Traces.
//10. Configure Averaging Parameters for ModAcc.
//11. Configure Averaging Parameters for ACP.
//12. Configure Averaging Parameters for CHP.
//13. Configure Averaging Parameters for OBW.
//14. Configure Averaging Parameters for SEM.
//15. Configure ACP Sweep Time.
//16. Configure CHP Sweep Time.
//17. Configure OBW Sweep Time.
//18. Configure SEM Sweep Time.
//19. Configure Standard Mask Type for SEM.
//20. Configure Synchronization Mode and Measurement Interval.
//21. Initiate the Measurement.
//22. Fetch SEM Measurements.
//23. Fetch OBW Measurements.
//24. Fetch CHP Measurements.
//25. Fetch ACP Measurements.
//26. Fetch ModAcc Measurements.
//27. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteULModAccAcpChpObwSemCompositeSingleCarrier
{
   public class RFmxLteULModAccAcpChpObwSemCompositeSingleCarrier
   {
      RFmxInstrMX instrSession;
      RFmxLteMX lte;

      string resourceName, frequencyReferenceSource, digitalEdgeSource;

      double frequencyReferenceFrequency, centerFrequency, referenceLevel, externalAttenuation, triggerDelay,
             componentCarrierFrequency, componentCarrierBandwidth, sweepTimeInterval, timeout;

      bool enableTrigger;

      int band, cellID, measurementOffset, measurementLength, averagingCount;

      RFmxLteMXDigitalEdgeTriggerEdge digitalEdge;
      RFmxLteMXDuplexScheme duplexScheme;
      RFmxLteMXModAccSynchronizationMode synchronizationMode;
      RFmxLteMXSemUplinkMaskType uplinkMaskType;
      RFmxLteMXUplinkDownlinkConfiguration uplinkDownlinkConfiguration;

      RFmxLteMXAutoDmrsDetectionEnabled autoDmrsDetectionEnabled;

      double acpAbsolutePower, acpRelativePower, chpAbsolutePower, chpRelativePower,
               modAccMeanRmsCompositeEvm, modAccMaxPeakCompositeEvm, modAccMeanFrequencyError,
               modAccMeanIQOriginOffset, modAccMeanIQGainImbalance, modAccMeanIQQuadratureError,
               obwOccupiedBandwidth, obwAbsolutePower, obwStopFrequency, obwStartFrequency,
               semAbsoluteIntegratedPower, semRelativeIntegratedPower;

      double[] acpLowerAbsolutePower, acpUpperAbsolutePower, acpLowerRelativePower, acpUpperRelativePower,
               semLowerOffsetMargin, semLowerOffsetMarginFrequency, semLowerOffsetMarginAbsolutePower,
               semLowerOffsetMarginRelativePower, semUpperOffsetMargin, semUpperOffsetMarginFrequency,
               semUpperOffsetMarginAbsolutePower, semUpperOffsetMarginRelativePower;

      int modAccPeakCompositeEvmSlotIndex, modAccPeakCompositeEvmSymbolIndex,
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
            Console.WriteLine("Press any key to exit.....");
            Console.ReadKey();
         }
      }

      private void InitializeVariables()
      {
         /* Initialize input variables */

         resourceName = "RFSA";

         centerFrequency = 1.95e+9;                                  /* Hz */
         referenceLevel = 0.00;                                      /* dBm */
         externalAttenuation = 0.00;                                 /* dB */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e+6;                      /* Hz */

         enableTrigger = false;
         digitalEdgeSource = RFmxLteMXConstants.Pfi0;
         digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                         /* seconds */

         uplinkDownlinkConfiguration = RFmxLteMXUplinkDownlinkConfiguration.Configuration0;
         duplexScheme = RFmxLteMXDuplexScheme.Fdd;

         band = 1;

         componentCarrierBandwidth = 10e6;                           /* Hz */
         componentCarrierFrequency = 0.0;                            /* Hz */
         cellID = 0;

         synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot;
         measurementOffset = 0;
         measurementLength = 1;

         uplinkMaskType = RFmxLteMXSemUplinkMaskType.General_NS01;

         sweepTimeInterval = 0.001;                                  /* seconds */

         averagingCount = 10;

         autoDmrsDetectionEnabled = RFmxLteMXAutoDmrsDetectionEnabled.True;

         timeout = 10.0;                                             /* seconds */
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

         lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency,
                                        cellID);

         lte.ConfigureBand("", band);

         lte.ConfigureDuplexScheme("", duplexScheme, uplinkDownlinkConfiguration);

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
                                                                          measurementOffset,
                                                                          measurementLength);

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

         lte.Sem.Results.ComponentCarrier.FetchMeasurement("", timeout, out semAbsoluteIntegratedPower,
                                                           out semRelativeIntegratedPower);

         lte.Sem.Results.FetchMeasurementStatus("", timeout, out semMeasurementStatus);

         lte.Obw.Results.FetchMeasurement("", timeout, out obwOccupiedBandwidth, out obwAbsolutePower,
                                          out obwStartFrequency, out obwStopFrequency);

         lte.Chp.Results.ComponentCarrier.FetchMeasurement("", timeout, out chpAbsolutePower,
                                                           out chpRelativePower);

         lte.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref acpLowerRelativePower,
                                                     ref acpUpperRelativePower, ref acpLowerAbsolutePower,
                                                     ref acpUpperAbsolutePower);

         lte.Acp.Results.ComponentCarrier.FetchMeasurement("", timeout, out acpAbsolutePower,
                                                           out acpRelativePower);

         lte.ModAcc.Results.FetchCompositeEvm("", timeout, out modAccMeanRmsCompositeEvm,
                                              out modAccMaxPeakCompositeEvm, out modAccMeanFrequencyError,
                                              out modAccPeakCompositeEvmSymbolIndex,
                                              out modAccPeakCompositeEvmSubcarrierIndex,
                                  out modAccPeakCompositeEvmSlotIndex);

         lte.ModAcc.Results.FetchIQImpairments("", timeout, out modAccMeanIQOriginOffset,
                                               out modAccMeanIQGainImbalance, out modAccMeanIQQuadratureError);
      }

      private void PrintResults()
      {
         Console.WriteLine("************************* ModAcc *************************\n");
         Console.WriteLine("Mean RMS Composite EVM  (%)               : {0}", modAccMeanRmsCompositeEvm);
         Console.WriteLine("Max Peak Composite EVM  (% )              : {0}", modAccMaxPeakCompositeEvm);
         Console.WriteLine("Mean FrequencyError     (Hz)              : {0}", modAccMeanFrequencyError);
         Console.WriteLine("Mean IQ Origin Offset   (dBc)             : {0}", modAccMeanIQOriginOffset);

         Console.WriteLine("\n************************* CHP *************************\n");
         Console.WriteLine("Carrier Absolute Power  (dBm)             : {0}", chpAbsolutePower);

         Console.WriteLine("\n************************* ACP *************************\n");
         Console.WriteLine("Carrier Absolute Power  (dBm)             : {0}", acpAbsolutePower);
         Console.WriteLine("\n------- Offset Channel Measurements -------");
         for (int i = 0; i < acpLowerRelativePower.Length; i++)
         {
            Console.WriteLine("\nOffset  : {0}", i);
            Console.WriteLine("Lower Relative Power (dB)                 : {0}", acpLowerRelativePower[i]);
            Console.WriteLine("Upper Relative Power (dB)                 : {0}", acpUpperRelativePower[i]);
            Console.WriteLine("Lower Absolute Power (dBm)                : {0}", acpLowerAbsolutePower[i]);
            Console.WriteLine("Upper Absolute Power (dBm)                : {0}", acpUpperAbsolutePower[i]);
         }

         Console.WriteLine("\n************************* OBW *************************\n");
         Console.WriteLine("Occupied Bandwidth  (Hz)                  : {0}", obwOccupiedBandwidth);
         Console.WriteLine("Absolute Power      (dBm)                 : {0}", obwAbsolutePower);
         Console.WriteLine("Start Frequency     (Hz)                  : {0}", obwStartFrequency);
         Console.WriteLine("Stop Frequency      (Hz)                  : {0}", obwStopFrequency);

         Console.WriteLine("\n************************* SEM *************************\n");
         Console.WriteLine("Measurement Status                        : {0}", semMeasurementStatus);
         Console.WriteLine("Carrier Absolute Integrated Power  (dBm)  : {0}", semAbsoluteIntegratedPower);
         Console.WriteLine("\n----- Lower Offset Segment Measurements -----");
         for (int i = 0; i < semLowerOffsetMargin.Length; i++)
         {
            Console.WriteLine("\nOffset  : {0}", i);
            Console.WriteLine("Measurement Status                        : {0}", semLowerOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin                 (dB)               : {0}", semLowerOffsetMargin[i]);
            Console.WriteLine("Margin Frequency       (Hz)               : {0}", semLowerOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power  (dBm)              : {0}", semLowerOffsetMarginAbsolutePower[i]);

         }
         Console.WriteLine("\n----- Upper Offset Segment Measurements -----");
         for (int i = 0; i < semUpperOffsetMargin.Length; i++)
         {
            Console.WriteLine("\nOffset  : {0}", i);
            Console.WriteLine("Measurement Status                        : {0}", semUpperOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin                 (dB)               : {0}", semUpperOffsetMargin[i]);
            Console.WriteLine("Margin Frequency       (Hz)               : {0}", semUpperOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power  (dBm)              : {0}", semUpperOffsetMarginAbsolutePower[i]);
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
