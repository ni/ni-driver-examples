//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//5.Configure Trigger Parameters for Digital Edge Trigger.
//6. Configure Link Direction, Frequency Range, Channel Raster and Component Carrier Spacing.
//7. Configure Bandwidth Part Subcarrier Spacing.
//8. Configure Component Carriers.
//9. Select SEM measurement and enable Traces.
//10. Configure Offsets.
//11. Configure Uplink Mask Type, or Downlink Mask, Band, gNodeB Category, Delta F_Max(Hz) and
//    Component Carrier Rated Output Power based on Link Direction.
//12. Configure Sweep Time Parameters.
//13. Configure Averaging Parameters for SEM measurement.
//14. Initiate the Measurement.
//15. Fetch SEM Measurements and Traces.
//16. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRSemContiguousMultiCarrier
{
   public class RFmxNRSemContiguousMultiCarrier
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

      RFmxNRMXSemUplinkMaskType uplinkMaskType;

      RFmxNRMXgNodeBCategory gNodeBCategory;
      RFmxNRMXSemDownlinkMaskType downlinkMaskType;
      double deltaFMaximum;
      int band;

      const int NumberOfComponentCarriers = 2;
      double[] componentCarrierBandwidth = new double[NumberOfComponentCarriers];
      double[] componentCarrierFrequency = new double[NumberOfComponentCarriers];
      double[] componentCarrierRatedOutputPower = new double[NumberOfComponentCarriers];

      const int NumberOfOffsets = 4;
      double[] offsetStartFrequency = new double[NumberOfOffsets];
      double[] offsetStopFrequency = new double[NumberOfOffsets];
      RFmxNRMXSemOffsetSideband[] offsetSideband = new RFmxNRMXSemOffsetSideband[NumberOfOffsets];
      double[] offsetRbw = new double[NumberOfOffsets];
      RFmxNRMXSemOffsetRbwFilterType[] offsetRbwFilterType = new RFmxNRMXSemOffsetRbwFilterType[NumberOfOffsets];
      int[] bandwidthIntegral = new int[NumberOfOffsets];
      RFmxNRMXSemOffsetLimitFailMask[] limitFailMask = new RFmxNRMXSemOffsetLimitFailMask[NumberOfOffsets];
      double[] absoluteLimitStart  = new double[NumberOfOffsets];
      double[] absoluteLimitStop = new double[NumberOfOffsets];
      double[] relativeLimitStart = new double[NumberOfOffsets];
      double[] relativeLimitStop = new double[NumberOfOffsets];

      RFmxNRMXFrequencyRange frequencyRange;

      RFmxNRMXComponentCarrierSpacingType componentCarrierSpacingType;
      double channelRaster;
      int componentCarrierAtCenterFrequency;
      double subcarrierSpacing;

      RFmxNRMXSemSweepTimeAuto sweepTimeAuto;
      double sweepTimeInterval;

      RFmxNRMXSemAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxNRMXSemAveragingType averagingType;

      string subblockString;
      string carrierString;

      double timeout;

      double totalAggregatedPower;                                                        /* (dBm) */

      RFmxNRMXSemMeasurementStatus measurementStatus;

      RFmxNRMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;
      double[] upperOffsetMargin;                                                         /* (dB) */
      double[] upperOffsetMarginFrequency;                                                /* (Hz) */
      double[] upperOffsetMarginAbsolutePower;                                            /* (dBm) */
      double[] upperOffsetMarginRelativePower;                                            /* (dB) */

      RFmxNRMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;
      double[] lowerOffsetMargin;                                                         /* (dB) */
      double[] lowerOffsetMarginFrequency;                                                /* (Hz) */
      double[] lowerOffsetMarginAbsolutePower;                                            /* (dBm) */
      double[] lowerOffsetMarginRelativePower;                                            /* (dB) */

      Spectrum<float> spectrum;
      Spectrum<float> compositeMask;

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
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
         }
      }

      void InitializeVariables()
      {
         resourceName = "RFSA";

         selectedPorts = "";
         centerFrequency = 3.5e9;                                                      /* (Hz) */
         referenceLevel = 0.0;                                                         /* (dBm) */
         externalAttenuation = 0.0;                                                    /* (dB) */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10e6;                                           /* (Hz) */

         enableTrigger = false;
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0;
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                                           /* (s) */

         linkDirection = RFmxNRMXLinkDirection.Uplink;

         uplinkMaskType = RFmxNRMXSemUplinkMaskType.General;

         gNodeBCategory = RFmxNRMXgNodeBCategory.WideAreaBaseStationCategoryA;
         downlinkMaskType = RFmxNRMXSemDownlinkMaskType.Standard;
         deltaFMaximum = 15.0e6;                                                       /* (Hz) */
         band = 78;

         componentCarrierBandwidth[0] = 100e6;                                         /* (Hz) */
         componentCarrierBandwidth[1] = 100e6;                                         /* (Hz) */
         componentCarrierFrequency[0] = -49.98e6;                                      /* (Hz) */
         componentCarrierFrequency[1] = 50.01e6;                                       /* (Hz) */
         componentCarrierRatedOutputPower[0] = 0.0;                                    /* (dBm) */
         componentCarrierRatedOutputPower[1] = 0.0;                                    /* (dBm) */

         offsetStartFrequency[0] = 15.0e3;                                             /* (Hz) */
         offsetStartFrequency[1] = 1.5e6;                                              /* (Hz) */
         offsetStartFrequency[2] = 5.5e6;                                              /* (Hz) */
         offsetStartFrequency[3] = 40.3e6;                                             /* (Hz) */
         offsetStopFrequency[0] = 985.0e3;                                             /* (Hz) */
         offsetStopFrequency[1] = 5.5e6;                                               /* (Hz) */
         offsetStopFrequency[2] = 39.3e6;                                              /* (Hz) */
         offsetStopFrequency[3] = 44.3e6;                                              /* (Hz) */
         offsetSideband[0] = RFmxNRMXSemOffsetSideband.Both;
         offsetSideband[1] = RFmxNRMXSemOffsetSideband.Both;
         offsetSideband[2] = RFmxNRMXSemOffsetSideband.Both;
         offsetSideband[3] = RFmxNRMXSemOffsetSideband.Both;
         offsetRbw[0] = 10.0e3;                                                        /* (Hz) */
         offsetRbw[1] = 250.0e3;                                                       /* (Hz) */
         offsetRbw[2] = 1.0e6;                                                         /* (Hz) */
         offsetRbw[3] = 1.0e6;                                                         /* (Hz) */
         offsetRbwFilterType[0] = RFmxNRMXSemOffsetRbwFilterType.Gaussian;
         offsetRbwFilterType[1] = RFmxNRMXSemOffsetRbwFilterType.Gaussian;
         offsetRbwFilterType[2] = RFmxNRMXSemOffsetRbwFilterType.Gaussian;
         offsetRbwFilterType[3] = RFmxNRMXSemOffsetRbwFilterType.Gaussian;
         bandwidthIntegral[0] = 3;
         bandwidthIntegral[1] = 4;
         bandwidthIntegral[2] = 1;
         bandwidthIntegral[3] = 1;
         limitFailMask[0] = RFmxNRMXSemOffsetLimitFailMask.Absolute;
         limitFailMask[1] = RFmxNRMXSemOffsetLimitFailMask.Absolute;
         limitFailMask[2] = RFmxNRMXSemOffsetLimitFailMask.Absolute;
         limitFailMask[3] = RFmxNRMXSemOffsetLimitFailMask.Absolute;
         absoluteLimitStart[0] = -22.5;                                                /* (dBm) */
         absoluteLimitStart[1] = -8.5;                                                 /* (dBm) */
         absoluteLimitStart[2] = -11.5;                                                /* (dBm) */
         absoluteLimitStart[3] = -23.5;                                                /* (dBm) */
         absoluteLimitStop[0] = -22.5;                                                 /* (dBm) */
         absoluteLimitStop[1] = -8.5;                                                  /* (dBm) */
         absoluteLimitStop[2] = -11.5;                                                 /* (dBm) */
         absoluteLimitStop[3] = -23.5;                                                 /* (dBm) */
         relativeLimitStart[0] = -53.0;                                                /* (dB) */
         relativeLimitStart[1] = -53.0;                                                /* (dB) */
         relativeLimitStart[2] = -53.0;                                                /* (dB) */
         relativeLimitStart[3] = -53.0;                                                /* (dB) */
         relativeLimitStop[0] = -60.0;                                                 /* (dB) */
         relativeLimitStop[1] = -60.0;                                                 /* (dB) */
         relativeLimitStop[2] = -60.0;                                                 /* (dB) */
         relativeLimitStop[3] = -60.0;                                                 /* (dB) */

         frequencyRange = RFmxNRMXFrequencyRange.Range1;

         componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal;
         channelRaster = 15e3;                                                         /* (Hz) */
         componentCarrierAtCenterFrequency = -1;
         subcarrierSpacing = 30e3;                                                     /* (Hz) */

         sweepTimeAuto = RFmxNRMXSemSweepTimeAuto.True;
         sweepTimeInterval = 1.0e-3;                                                   /* (s) */

         averagingEnabled = RFmxNRMXSemAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxNRMXSemAveragingType.Rms;

         timeout = 10.0;                                                               /* (s) */
      }

      void InitializeInstr()
      {
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      void ConfigureNR()
      {
         NR = instrSession.GetNRSignalConfiguration();     /* Create a new RFmx Session */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         NR.SetSelectedPorts("", selectedPorts);
         NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);

         NR.SetLinkDirection("", linkDirection);
         NR.SetFrequencyRange("", frequencyRange);
         NR.SetChannelRaster("", channelRaster);
         NR.SetComponentCarrierSpacingType("", componentCarrierSpacingType);
         NR.SetComponentCarrierAtCenterFrequency("", componentCarrierAtCenterFrequency);

         carrierString = "carrier::all";
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing);

         NR.ComponentCarrier.SetNumberOfComponentCarriers("", NumberOfComponentCarriers);

         subblockString = RFmxNRMX.BuildSubblockString("", 0);
         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i);
            NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth[i]);
            NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency[i]);
         }

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Sem, true);

         NR.Sem.Configuration.ConfigureNumberOfOffsets("", NumberOfOffsets);
         NR.Sem.Configuration.ConfigureOffsetFrequencyArray("", offsetStartFrequency, offsetStopFrequency,
            offsetSideband);
         NR.Sem.Configuration.ConfigureOffsetRbwFilterArray("", offsetRbw, offsetRbwFilterType);
         NR.Sem.Configuration.ConfigureOffsetBandwidthIntegralArray("", bandwidthIntegral);
         NR.Sem.Configuration.ConfigureOffsetLimitFailMaskArray("", limitFailMask);
         NR.Sem.Configuration.ConfigureOffsetAbsoluteLimitArray("", absoluteLimitStart, absoluteLimitStop);
         NR.Sem.Configuration.ConfigureOffsetRelativeLimitArray("", relativeLimitStart, relativeLimitStop);

         if (linkDirection == RFmxNRMXLinkDirection.Uplink)
         {
            NR.Sem.Configuration.ConfigureUplinkMaskType("", uplinkMaskType);
         }
         else
         {
            NR.ConfiguregNodeBCategory("", gNodeBCategory);
            NR.SetBand("", band);
            NR.Sem.Configuration.SetDownlinkMaskType("", downlinkMaskType);
            NR.Sem.Configuration.SetDeltaFMaximum("", deltaFMaximum);
            NR.Sem.Configuration.ComponentCarrier.ConfigureRatedOutputPowerArray("", componentCarrierRatedOutputPower);
         }

         NR.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         NR.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);

         NR.Initiate("", "");
      }

      void RetrieveResults()
      {
         NR.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref upperOffsetMeasurementStatus,
            ref upperOffsetMargin, ref upperOffsetMarginFrequency, ref upperOffsetMarginAbsolutePower,
            ref upperOffsetMarginRelativePower);

         NR.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref lowerOffsetMeasurementStatus,
            ref lowerOffsetMargin, ref lowerOffsetMarginFrequency, ref lowerOffsetMarginAbsolutePower,
            ref lowerOffsetMarginRelativePower);

         NR.Sem.Results.FetchTotalAggregatedPower("", timeout, out totalAggregatedPower);

         NR.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);

         NR.Sem.Results.FetchSpectrum("", timeout, ref spectrum, ref compositeMask);
      }

      void PrintResults()
      {
         Console.WriteLine("Total Aggregated Power (dBm)    : {0}", totalAggregatedPower);
         Console.WriteLine("Measurement Status              : {0}", measurementStatus);

         Console.WriteLine("\n--------  Lower Offset Segement Measurements --------\n");
         for (int i = 0; i < lowerOffsetMargin.Length; i++)
         {
            Console.WriteLine("Offset  {0}", i);
            Console.WriteLine("Measurement Status              : {0}", lowerOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin (dB)                     : {0}", lowerOffsetMargin[i]);
            Console.WriteLine("Margin Frequency (Hz)           : {0}", lowerOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power (dBm)     : {0}\n", lowerOffsetMarginAbsolutePower[i]);
         }

         Console.WriteLine("\n--------  Upper  Offset Segement Measurements --------\n");
         for (int i = 0; i < upperOffsetMargin.Length; i++)
         {
            Console.WriteLine("Offset  {0}", i);
            Console.WriteLine("Measurement Status              : {0}", upperOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin (dB)                     : {0}", upperOffsetMargin[i]);
            Console.WriteLine("Margin Frequency (Hz)           : {0}", upperOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power (dBm)     : {0}\n", upperOffsetMarginAbsolutePower[i]);
         }
      }

      void CloseSession()
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

      static void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

   }
}
