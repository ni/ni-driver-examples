//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Link Direction as Uplink, Frequency Range, Band, Component Carrier and Subcarrier Spacing.
//7. Select ModAcc, ACP, CHP, OBW, SEM and TXP measurements and enable Traces.
//8. Configure ACP Sweep Time.
//9. Configure CHP Sweep Time.
//10. Configure OBW Sweep Time.
//11. Configure SEM Sweep Time.
//12. Configure Averaging Parameters for ModAcc.
//13. Configure Averaging Parameters for ACP.
//14. Configure Averaging Parameters for CHP.
//15. Configure Averaging Parameters for OBW.
//16. Configure Averaging Parameters for SEM.
//17. Configure Averaging Parameters for TXP.
//18. Configure Measurement Interval for ModAcc.
//19. Configure Measurement Interval for TXP.
//20. Configure SEM Uplink Mask Type, or Downlink Mask, gNodeB Category, Delta F_Max (Hz) and Component Carrier Rated Output Power depending on Link Direction.
//21. Initiate the Measurement.
//22. Fetch ModAcc Measurements.
//23. Fetch ACP Measurements.
//24. Fetch CHP Measurements.
//25. Fetch OBW Measurements.
//26. Fetch SEM Measurements.
//27. Fetch TXP Measurements
//28. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRModAccAcpChpObwSemTxpCompositeSingleCarrier
{
   public class RFmxNRModAccAcpChpObwSemTxpCompositeSingleCarrier
   {
      RFmxInstrMX instrSession;
      RFmxNRMX NR;
      string resourceName;

      string selectedPorts;
      double centerFrequency;
      double referenceLevel;
      double externalAttenuation;

      string frequencyReferenceSource;
      double frequencyReferenceFrequency;

      bool enableTrigger;
      string digitalEdgeSource;
      RFmxNRMXDigitalEdgeTriggerEdge digitalEdge;
      double triggerDelay;

      RFmxNRMXLinkDirection linkDirection;
      RFmxNRMXFrequencyRange frequencyRange;
      double carrierBandwidth;
      double subcarrierSpacing;
      int modaccBand;

      RFmxNRMXModAccMeasurementLengthUnit measurementLengthUnit;
      double measurementOffset;
      double measurementLength;

      double txpMeasurementOffset;
      double txpMeasurementLength;

        RFmxNRMXSemUplinkMaskType uplinkMaskType;

      RFmxNRMXgNodeBCategory gNodeBCategory;
      RFmxNRMXSemDownlinkMaskType downlinkMaskType;
      double deltaFMaximum;
      double componentCarrierRatedOutputPower;

      double sweepTimeInterval;
      int averagingCount;

      double timeout;

      double compositeRmsEvmMean;                                                         /* (%) */
      double compositePeakEvmMaximum;                                                     /* (%) */
      double componentCarrierFrequencyErrorMean;                                          /* (Hz) */
      double componentCarrierIQOriginOffsetMean;                                          /* (dBc) */

      double chpAbsolutePower;                                                            /* (dBm) */
      double chpRelativePower;                                                            /* (dB) */

      double acpAbsolutePower;                                                            /* (dBm) */
      double acpRelativePower;                                                            /* (dB) */
      double[] acpLowerRelativePower;                                                     /* (dB) */
      double[] acpUpperRelativePower;                                                     /* (dB) */
      double[] acpLowerAbsolutePower;                                                     /* (dBm) */
      double[] acpUpperAbsolutePower;                                                     /* (dBm) */

      double obwOccupiedBandwidth;                                                        /* (Hz) */
      double obwAbsolutePower;                                                            /* (dBm) */
      double obwStartFrequency;                                                           /* (Hz) */
      double obwStopFrequency;                                                            /* (Hz) */

      RFmxNRMXSemMeasurementStatus semMeasurementStatus;
      double semAbsoluteIntegratedPower;                                                  /* (dBm) */
      double semRelativeIntegratedPower;                                                  /* (dB) */
      double semPeakAbsoluteIntegratedPower;
      double semPeakFrequency;
      RFmxNRMXSemLowerOffsetMeasurementStatus[] semLowerOffsetMeasurementStatus;
      double[] semLowerOffsetMargin;                                                      /* (dB) */
      double[] semLowerOffsetMarginFrequency;                                             /* (Hz) */
      double[] semLowerOffsetMarginAbsolutePower;                                         /* (dBm) */
      double[] semLowerOffsetMarginRelativePower;                                         /* (dB) */
      RFmxNRMXSemUpperOffsetMeasurementStatus[] semUpperOffsetMeasurementStatus;
      double[] semUpperOffsetMargin;                                                      /* (dB) */
      double[] semUpperOffsetMarginFrequency;                                             /* (Hz) */
      double[] semUpperOffsetMarginAbsolutePower;                                         /* (dBm) */
      double[] semUpperOffsetMarginRelativePower;                                         /* (dB) */

      double averagePowerMean;                                                            /* (dBm) */
      double peakPowerMaximum;                                                            /* (dBm) */
      
      public void Run()
      {
         try
         {
            InitializeVariables();
            InitializeInstr();
            ConfigureNR();
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
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
         }
      }

      private void InitializeVariables()
      {
         resourceName = "RFSA";

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e6;                                            /* (Hz) */

         selectedPorts = "";
         centerFrequency = 3.5e9;                                                         /* (Hz) */
         referenceLevel = 0.00;                                                           /* (dBm) */
         externalAttenuation = 0.0;                                                       /* (dB) */

         enableTrigger = false;
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0;
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                                              /* (s) */

         linkDirection = RFmxNRMXLinkDirection.Uplink;
         frequencyRange = RFmxNRMXFrequencyRange.Range1;
         carrierBandwidth = 100e6;                                                        /* (Hz) */
         subcarrierSpacing = 30e3;                                                        /* (Hz) */
         modaccBand = 78;

         measurementLengthUnit = RFmxNRMXModAccMeasurementLengthUnit.Slot;
         measurementOffset = 0.0;
         measurementLength = 1;

         txpMeasurementOffset = 0.0;                                                         /* (s) */
         txpMeasurementLength = 1.0e-3;                                                      /* (s) */

         uplinkMaskType = RFmxNRMXSemUplinkMaskType.General;

         gNodeBCategory = RFmxNRMXgNodeBCategory.WideAreaBaseStationCategoryA;
         downlinkMaskType = RFmxNRMXSemDownlinkMaskType.Standard;
         deltaFMaximum = 15.0e6;                                                          /* (Hz) */
         componentCarrierRatedOutputPower = 0.0;                                          /* (dBm) */

         sweepTimeInterval = 1.0e-3;                                                      /* (s) */

         averagingCount = 10;

         timeout = 10.0;                                                                  /* (s) */
      }

      private void InitializeInstr()
      {
         /* Create a new RFmx Session */
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      private void ConfigureNR()
      {
         NR = instrSession.GetNRSignalConfiguration();

         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);

         NR.SetSelectedPorts("", selectedPorts);

         NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);

         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);

         NR.SetLinkDirection("", linkDirection);
         NR.SetFrequencyRange("", frequencyRange);
         NR.SetBand("", modaccBand);
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth);
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing);

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc | RFmxNRMXMeasurementTypes.Acp |
            RFmxNRMXMeasurementTypes.Chp | RFmxNRMXMeasurementTypes.Obw | RFmxNRMXMeasurementTypes.Sem | RFmxNRMXMeasurementTypes.Txp, true);

         NR.Acp.Configuration.ConfigureSweepTime("", RFmxNRMXAcpSweepTimeAuto.True, sweepTimeInterval);

         NR.Chp.Configuration.ConfigureSweepTime("", RFmxNRMXChpSweepTimeAuto.True, sweepTimeInterval);

         NR.Obw.Configuration.ConfigureSweepTime("", RFmxNRMXObwSweepTimeAuto.True, sweepTimeInterval);

         NR.Sem.Configuration.ConfigureSweepTime("", RFmxNRMXSemSweepTimeAuto.True, sweepTimeInterval);

         NR.ModAcc.Configuration.SetAveragingEnabled("", RFmxNRMXModAccAveragingEnabled.False);
         NR.ModAcc.Configuration.SetAveragingCount("", averagingCount);

         NR.Acp.Configuration.ConfigureAveraging("", RFmxNRMXAcpAveragingEnabled.False, averagingCount,
            RFmxNRMXAcpAveragingType.Rms);

         NR.Chp.Configuration.ConfigureAveraging("", RFmxNRMXChpAveragingEnabled.False, averagingCount,
            RFmxNRMXChpAveragingType.Rms);

         NR.Obw.Configuration.ConfigureAveraging("", RFmxNRMXObwAveragingEnabled.False, averagingCount,
            RFmxNRMXObwAveragingType.Rms);

         NR.Sem.Configuration.ConfigureAveraging("", RFmxNRMXSemAveragingEnabled.False, averagingCount,
            RFmxNRMXSemAveragingType.Rms);

         NR.Txp.Configuration.SetAveragingEnabled("", RFmxNRMXTxpAveragingEnabled.False);
         NR.Txp.Configuration.SetAveragingCount("", averagingCount);

         NR.ModAcc.Configuration.SetMeasurementLengthUnit("", measurementLengthUnit);
         NR.ModAcc.Configuration.SetMeasurementOffset("", measurementOffset);
         NR.ModAcc.Configuration.SetMeasurementLength("", measurementLength);

         NR.Txp.Configuration.SetMeasurementOffset("", txpMeasurementOffset);
         NR.Txp.Configuration.SetMeasurementInterval("", txpMeasurementLength);

         if (linkDirection == RFmxNRMXLinkDirection.Uplink)
         {
            NR.Sem.Configuration.ConfigureUplinkMaskType("", uplinkMaskType);
         }
         else
         {
            NR.ConfiguregNodeBCategory("", gNodeBCategory);
            NR.Sem.Configuration.SetDownlinkMaskType("", downlinkMaskType);
            NR.Sem.Configuration.SetDeltaFMaximum("", deltaFMaximum);
            NR.Sem.Configuration.ComponentCarrier.ConfigureRatedOutputPower("", componentCarrierRatedOutputPower);
         }

         NR.Initiate("", "");
      }

      private void RetrieveResults()
      {
         NR.ModAcc.Results.GetCompositeRmsEvmMean("", out compositeRmsEvmMean);
         NR.ModAcc.Results.GetCompositePeakEvmMaximum("", out compositePeakEvmMaximum);
         NR.ModAcc.Results.GetComponentCarrierFrequencyErrorMean("", out componentCarrierFrequencyErrorMean);
         NR.ModAcc.Results.GetComponentCarrierIQOriginOffsetMean("", out componentCarrierIQOriginOffsetMean);

         NR.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref acpLowerRelativePower,
            ref acpUpperRelativePower, ref acpLowerAbsolutePower, ref acpUpperAbsolutePower);

         NR.Acp.Results.ComponentCarrier.FetchMeasurement("", timeout, out acpAbsolutePower, out acpRelativePower);

         NR.Chp.Results.ComponentCarrier.FetchMeasurement("", timeout, out chpAbsolutePower, out chpRelativePower);

         NR.Obw.Results.FetchMeasurement("", timeout, out obwOccupiedBandwidth, out obwAbsolutePower,
            out obwStartFrequency, out obwStopFrequency);

         NR.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref semLowerOffsetMeasurementStatus,
            ref semLowerOffsetMargin, ref semLowerOffsetMarginFrequency, ref semLowerOffsetMarginAbsolutePower,
            ref semLowerOffsetMarginRelativePower);

         NR.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref semUpperOffsetMeasurementStatus,
            ref semUpperOffsetMargin, ref semUpperOffsetMarginFrequency, ref semUpperOffsetMarginAbsolutePower,
            ref semUpperOffsetMarginRelativePower);

         NR.Sem.Results.ComponentCarrier.FetchMeasurement("", timeout, out semAbsoluteIntegratedPower,
            out semPeakAbsoluteIntegratedPower, out semPeakFrequency, out semRelativeIntegratedPower);

         NR.Sem.Results.FetchMeasurementStatus("", timeout, out semMeasurementStatus);

         NR.Txp.Results.FetchMeasurement("", timeout, out averagePowerMean, out peakPowerMaximum);
        }

      private void PrintResults()
      {
         Console.WriteLine("************************* ModAcc *************************\n");
         Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean);
         Console.WriteLine("Composite Peak EVM Maximum (% )                : {0}", compositePeakEvmMaximum);
         Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean);
         Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}\n", componentCarrierIQOriginOffsetMean);

         Console.WriteLine("\n\n************************* CHP *************************\n");
         Console.WriteLine("Carrier Absolute Power (dBm)                   : {0}\n", chpAbsolutePower);

         Console.WriteLine("\n\n************************* ACP *************************\n");
         Console.WriteLine("Carrier Absolute Power (dBm)                   : {0}", acpAbsolutePower);
         Console.WriteLine("\n------- Offset Channel Measurements -------");
         for (int i = 0; i < acpLowerRelativePower.Length; i++)
         {
            Console.WriteLine("\nOffset  {0}", i);
            Console.WriteLine("Lower Relative Power (dB)                      : {0}", acpLowerRelativePower[i]);
            Console.WriteLine("Upper Relative Power (dB)                      : {0}", acpUpperRelativePower[i]);
            Console.WriteLine("Lower Absolute Power (dBm)                     : {0}", acpLowerAbsolutePower[i]);
            Console.WriteLine("Upper Absolute Power (dBm)                     : {0}", acpUpperAbsolutePower[i]);
         }

         Console.WriteLine("\n\n\n************************* OBW *************************\n");
         Console.WriteLine("Occupied Bandwidth (Hz)                        : {0}", obwOccupiedBandwidth);
         Console.WriteLine("Absolute Power (dBm)                           : {0}", obwAbsolutePower);
         Console.WriteLine("Start Frequency (Hz)                           : {0}", obwStartFrequency);
         Console.WriteLine("Stop Frequency (Hz)                            : {0}\n", obwStopFrequency);

         Console.WriteLine("\n\n************************* SEM *************************\n");
         Console.WriteLine("Measurement Status                             : {0}", semMeasurementStatus);
         Console.WriteLine("Carrier Absolute Integrated Power (dBm)        : {0}", semAbsoluteIntegratedPower);
         Console.WriteLine("\n----- Lower Offset Segment Measurements -----");
         for (int i = 0; i < semLowerOffsetMargin.Length; i++)
         {
            Console.WriteLine("\nOffset  {0}", i);
            Console.WriteLine("Measurement Status                             : {0}", semLowerOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin (dB)                                    : {0}", semLowerOffsetMargin[i]);
            Console.WriteLine("Margin Frequency (Hz)                          : {0}", semLowerOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power (dBm)                    : {0}", semLowerOffsetMarginAbsolutePower[i]);

         }
         Console.WriteLine("\n----- Upper Offset Segment Measurements -----");
         for (int i = 0; i < semUpperOffsetMargin.Length; i++)
         {
            Console.WriteLine("\nOffset  {0}", i);
            Console.WriteLine("Measurement Status                             : {0}", semUpperOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin (dB)                                    : {0}", semUpperOffsetMargin[i]);
            Console.WriteLine("Margin Frequency (Hz)                          : {0}", semUpperOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power (dBm)                    : {0}", semUpperOffsetMarginAbsolutePower[i]);
         }

         Console.WriteLine("\n\n************************* TXP *************************\n");
         Console.WriteLine("Average Power Mean (dBm)                          : {0}\n", averagePowerMean);
         Console.WriteLine("Peak Power Maximum (dBm)                          : {0}\n", peakPowerMaximum);
        }

      private void CloseSession()
      {
         try
         {
            if (NR != null)
            {
               NR.Dispose();
               NR = null;
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
