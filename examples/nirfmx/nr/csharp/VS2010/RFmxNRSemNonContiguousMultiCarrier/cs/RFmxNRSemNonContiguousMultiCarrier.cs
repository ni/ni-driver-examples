//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Parameters for Digitial Edge Trigger.
//6. Configure Number of Subblocks and Link Direction.
//7. Configure Frequency Range, Component Carrier Spacing, Channel Raster,
//   Component Carrier Center Frequency, Subblock Frequency and Number of Component Carriers.
//8. Configure Component Carriers.
//9. Configure Bandwidth Part Subcarrier Spacing.
//10. Select SEM measurement and enable Traces.
//11. Configure Uplink Mask Type, or Downlink Mask, Band, gNodeB Category and Delta F_Max(Hz) based on Link Direction.
//12. Configure Component Carrier Rated Output Power based on Link Direction.
//13. Configure Offsets.
//14. Configure Sweep Time Parameters.
//15. Configure Averaging Parameters for SEM measurement.
//16. Initiate the Measurement.
//17. Fetch SEM Measurements and Traces.
//18. Close the RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRSemNonContiguousMultiCarrier
{
   public class RFmxNRSemNonContiguousMultiCarrier
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

      RFmxNRMXSemUplinkMaskType uplinkMaskType;
      RFmxNRMXgNodeBCategory gNodeBCategory;
      RFmxNRMXSemDownlinkMaskType downlinkMaskType;
      double deltaFMaximum;
      int band;

      double subcarrierSpacing;

      RFmxNRMXSemSweepTimeAuto sweepTimeAuto;
      double sweepTimeInterval;

      RFmxNRMXSemAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxNRMXSemAveragingType averagingType;

      double timeout;

      string subblockString;
      string carrierString;

      const int NumberOfSubblocks = 2;
      const int NumberOfComponentCarriers = 2;
      const int NumberOfOffsets = 4;

      /* Subblock inputs structure */
      struct SubblockMeasurementInput
      {
         public double subblockFrequency;                                           /* (Hz) */
         public RFmxNRMXComponentCarrierSpacingType componentCarrierSpacingType;
         public double channelRaster;                                               /* (Hz) */
         public int componentCarrierAtCenterFrequency;

         public double[] componentCarrierBandwidth;                                 /* (Hz) */
         public double[] componentCarrierFrequency;                                 /* (Hz) */
         public double[] componentCarrierRatedOutputPower;                          /* (dBm) */

         public double[] offsetStartFrequency;                                      /* (Hz) */
         public double[] offsetStopFrequency;                                       /* (Hz) */
         public RFmxNRMXSemOffsetSideband[] offsetSideband;
         public double[] offsetRbw;                                                 /* (Hz) */
         public RFmxNRMXSemOffsetRbwFilterType[] offsetRbwFilterType;
         public int[] bandwidthIntegral;
         public RFmxNRMXSemOffsetLimitFailMask[] offsetLimitFailMask;
         public double[] absoluteLimitStart;                                        /* (dBm) */
         public double[] absoluteLimitStop;                                         /* (dBm) */
         public double[] relativeLimitStart;                                        /* (dB) */
         public double[] relativeLimitStop;                                         /* (dB) */
      }

      /* Subblock measurement outputs structure */
      struct SubblockMeasurementOutput
      {
         public double subblockPower;
         public double integrationBandwidth;
         public double frequency;

         public double[] lowerOffsetMarginRelativePower;                            /* (dB) */
         public double[] lowerOffsetMarginAbsolutePower;                            /* (dBm) */
         public double[] lowerOffsetMargin;
         public double[] lowerOffsetMarginFrequency;                                /* (Hz) */
         public RFmxNRMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;

         public double[] upperOffsetMarginRelativePower;                            /* (dB) */
         public double[] upperOffsetMarginAbsolutePower;                            /* (dBm) */
         public double[] upperOffsetMargin;
         public double[] upperOffsetMarginFrequency;                                /* (Hz) */
         public RFmxNRMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;
      }

      SubblockMeasurementInput[] subblockInput;
      SubblockMeasurementOutput[] subblockOutput;

      Spectrum<float> spectrum;
      Spectrum<float> absoluteMask;
      double totalAggregatedPower;                                                  /* (dBm) */
      RFmxNRMXSemMeasurementStatus measurementStatus;


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
            CloseSession();
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
         }
      }

      void InitializeVariables()
      {
         resourceName = "RFSA";

         selectedPorts = "";
            
         centerFrequency = 3.5e9;                                                      /* (Hz) */
         referenceLevel = 0.0;                                                      /* (dBm) */
         externalAttenuation = 0.0;                                                 /* (dB) */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e6;                                      /* (Hz) */

         enableTrigger = false;
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0;
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                                        /* (s) */

         linkDirection = RFmxNRMXLinkDirection.Uplink;
         frequencyRange = RFmxNRMXFrequencyRange.Range1;

         uplinkMaskType = RFmxNRMXSemUplinkMaskType.General;

         gNodeBCategory = RFmxNRMXgNodeBCategory.WideAreaBaseStationCategoryA;
         downlinkMaskType = RFmxNRMXSemDownlinkMaskType.Standard;
         deltaFMaximum = 15.0e6;                                                    /* (Hz) */
         band = 78;

         subcarrierSpacing = 30e3;                                                  /* (Hz) */

         sweepTimeAuto = RFmxNRMXSemSweepTimeAuto.True;
         sweepTimeInterval = 1.0e-3;                                                /* (s) */

         averagingEnabled = RFmxNRMXSemAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxNRMXSemAveragingType.Rms;

         timeout = 10.0;                                                            /* (s) */

         subblockOutput = new SubblockMeasurementOutput[NumberOfSubblocks];

         subblockInput = new SubblockMeasurementInput[NumberOfSubblocks]
         {
            new SubblockMeasurementInput
            {
               subblockFrequency = 0.0,
               componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal,
               channelRaster = 15e3,
               componentCarrierAtCenterFrequency = -1,

               componentCarrierBandwidth = new double[NumberOfComponentCarriers] { 100e6, 100e6 },
               componentCarrierFrequency = new double[NumberOfComponentCarriers] { -49.98e6, 50.01e6},
               componentCarrierRatedOutputPower = new double[NumberOfComponentCarriers] { 0.0, 0.0 },

               offsetStartFrequency = new double[NumberOfOffsets] { 15.0e3, 1.5e6, 5.5e6, 20.5e6 },
               offsetStopFrequency = new double[NumberOfOffsets] { 985.0e3, 4.5e6, 19.5e6, 24.5e6 },
               offsetSideband = new RFmxNRMXSemOffsetSideband[NumberOfOffsets]
                  { RFmxNRMXSemOffsetSideband.Both, RFmxNRMXSemOffsetSideband.Both,
                    RFmxNRMXSemOffsetSideband.Both, RFmxNRMXSemOffsetSideband.Both },
               offsetRbw = new double[NumberOfOffsets] { 10.0e3, 250.0e3, 250.0e3, 250.0e3 },
               offsetRbwFilterType = new RFmxNRMXSemOffsetRbwFilterType[NumberOfOffsets]
                  { RFmxNRMXSemOffsetRbwFilterType.Gaussian, RFmxNRMXSemOffsetRbwFilterType.Gaussian,
                    RFmxNRMXSemOffsetRbwFilterType.Gaussian, RFmxNRMXSemOffsetRbwFilterType.Gaussian },
               bandwidthIntegral = new int[NumberOfOffsets] { 3, 4, 4, 4 },
               offsetLimitFailMask = new RFmxNRMXSemOffsetLimitFailMask[NumberOfOffsets]
                  { RFmxNRMXSemOffsetLimitFailMask.Absolute, RFmxNRMXSemOffsetLimitFailMask.Absolute,
                    RFmxNRMXSemOffsetLimitFailMask.Absolute, RFmxNRMXSemOffsetLimitFailMask.Absolute },
               absoluteLimitStart = new double[NumberOfOffsets] { -22.50, -8.5, -11.5, -23.5 },
               absoluteLimitStop = new double[NumberOfOffsets] { -22.50, -8.5, -11.5, -23.5 },
               relativeLimitStart = new double[NumberOfOffsets] { -53.0, -53.0, -53.0, -53.0 },
               relativeLimitStop = new double[NumberOfOffsets] { -60.0, -60.0, -60.0, -60.0 }
            },
            new SubblockMeasurementInput {
               subblockFrequency = 200e6,
               componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal,
               channelRaster = 15e3,
               componentCarrierAtCenterFrequency = -1,

               componentCarrierBandwidth = new double[NumberOfComponentCarriers] { 100e6, 100e6 },
               componentCarrierFrequency = new double[NumberOfComponentCarriers] { -49.98e6, 50.01e6},
               componentCarrierRatedOutputPower = new double[NumberOfComponentCarriers] { 0.0, 0.0 },

               offsetStartFrequency = new double[NumberOfOffsets] { 15.0e3, 1.5e6, 5.5e6, 20.5e6 },
               offsetStopFrequency = new double[NumberOfOffsets] { 985.0e3, 4.5e6, 19.5e6, 24.5e6 },
               offsetSideband = new RFmxNRMXSemOffsetSideband[NumberOfOffsets]
                  { RFmxNRMXSemOffsetSideband.Both, RFmxNRMXSemOffsetSideband.Both,
                    RFmxNRMXSemOffsetSideband.Both, RFmxNRMXSemOffsetSideband.Both },
               offsetRbw = new double[NumberOfOffsets] { 10.0e3, 250.0e3, 250.0e3, 250.0e3 },
               offsetRbwFilterType = new RFmxNRMXSemOffsetRbwFilterType[NumberOfOffsets]
                  { RFmxNRMXSemOffsetRbwFilterType.Gaussian, RFmxNRMXSemOffsetRbwFilterType.Gaussian,
                    RFmxNRMXSemOffsetRbwFilterType.Gaussian, RFmxNRMXSemOffsetRbwFilterType.Gaussian },
               bandwidthIntegral = new int[NumberOfOffsets] { 3, 4, 4, 4 },
               offsetLimitFailMask = new RFmxNRMXSemOffsetLimitFailMask[NumberOfOffsets]
                  { RFmxNRMXSemOffsetLimitFailMask.Absolute, RFmxNRMXSemOffsetLimitFailMask.Absolute,
                    RFmxNRMXSemOffsetLimitFailMask.Absolute, RFmxNRMXSemOffsetLimitFailMask.Absolute },
               absoluteLimitStart = new double[NumberOfOffsets] { -22.50, -8.5, -11.5, -23.5 },
               absoluteLimitStop = new double[NumberOfOffsets] { -22.50, -8.5, -11.5, -23.5 },
               relativeLimitStart = new double[NumberOfOffsets] { -53.0, -53.0, -53.0, -51.5 },
               relativeLimitStop = new double[NumberOfOffsets] { -60.0, -60.0, -60.0, -58.5 }
            }
         };
      }

      void InitializeInstr()
      {
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      void ConfigureNR()
      {
         NR = instrSession.GetNRSignalConfiguration();
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         NR.SetSelectedPorts("", selectedPorts);
         NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);

         NR.SetNumberOfSubblocks("", NumberOfSubblocks);
         NR.SetLinkDirection("", linkDirection);

         for (int i = 0; i < NumberOfSubblocks; i++)
         {
            subblockString = RFmxNRMX.BuildSubblockString("", i);
            NR.SetFrequencyRange(subblockString, frequencyRange);
            NR.SetComponentCarrierSpacingType(subblockString, subblockInput[i].componentCarrierSpacingType);
            NR.SetChannelRaster(subblockString, subblockInput[i].channelRaster);
            NR.SetComponentCarrierAtCenterFrequency(subblockString, subblockInput[i].componentCarrierAtCenterFrequency);
            NR.SetSubblockFrequency(subblockString, subblockInput[i].subblockFrequency);
            NR.ComponentCarrier.SetNumberOfComponentCarriers(subblockString, NumberOfComponentCarriers);

            for (int j = 0; j < NumberOfComponentCarriers; j++)
            {
               carrierString = RFmxNRMX.BuildCarrierString(subblockString, j);
               NR.ComponentCarrier.SetBandwidth(carrierString, subblockInput[i].componentCarrierBandwidth[j]);
               NR.ComponentCarrier.SetFrequency(carrierString, subblockInput[i].componentCarrierFrequency[j]);
            }

            carrierString = RFmxNRMX.BuildCarrierString(subblockString, -1);
            NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing);
         }

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Sem, true);

         if (linkDirection == RFmxNRMXLinkDirection.Uplink)
            NR.Sem.Configuration.ConfigureUplinkMaskType("", uplinkMaskType);
         else
         {
            NR.ConfiguregNodeBCategory("", gNodeBCategory);
            NR.Sem.Configuration.SetDownlinkMaskType("", downlinkMaskType);
            NR.Sem.Configuration.SetDeltaFMaximum("", deltaFMaximum);
            subblockString = RFmxNRMX.BuildSubblockString("", -1);
            NR.SetBand(subblockString, band);
         }

         for (int i = 0; i < NumberOfSubblocks; i++)
         {
            subblockString = RFmxNRMX.BuildSubblockString("", i);
            if (linkDirection == RFmxNRMXLinkDirection.Downlink)
            {
               NR.Sem.Configuration.ComponentCarrier.ConfigureRatedOutputPowerArray(subblockString,
               subblockInput[i].componentCarrierRatedOutputPower);
            }
            NR.Sem.Configuration.ConfigureNumberOfOffsets(subblockString, NumberOfOffsets);
            NR.Sem.Configuration.ConfigureOffsetFrequencyArray(subblockString, subblockInput[i].offsetStartFrequency,
               subblockInput[i].offsetStopFrequency, subblockInput[i].offsetSideband);
            NR.Sem.Configuration.ConfigureOffsetRbwFilterArray(subblockString, subblockInput[i].offsetRbw,
               subblockInput[i].offsetRbwFilterType);
            NR.Sem.Configuration.ConfigureOffsetBandwidthIntegralArray(subblockString, subblockInput[i].bandwidthIntegral);
            NR.Sem.Configuration.ConfigureOffsetLimitFailMaskArray(subblockString, subblockInput[i].offsetLimitFailMask);
            NR.Sem.Configuration.ConfigureOffsetAbsoluteLimitArray(subblockString, subblockInput[i].absoluteLimitStart,
               subblockInput[i].absoluteLimitStop);
            NR.Sem.Configuration.ConfigureOffsetRelativeLimitArray(subblockString, subblockInput[i].relativeLimitStart,
               subblockInput[i].relativeLimitStop);
         }

         NR.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         NR.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         NR.Initiate("", "");
      }

      void RetrieveResults()
      {
         for (int i = 0; i < NumberOfSubblocks; i++)
         {
            subblockString = RFmxNRMX.BuildSubblockString("", i);

            NR.Sem.Results.FetchUpperOffsetMarginArray(subblockString, timeout,
               ref subblockOutput[i].upperOffsetMeasurementStatus, ref subblockOutput[i].upperOffsetMargin,
               ref subblockOutput[i].upperOffsetMarginFrequency, ref subblockOutput[i].upperOffsetMarginAbsolutePower,
               ref subblockOutput[i].upperOffsetMarginRelativePower);

            NR.Sem.Results.FetchLowerOffsetMarginArray(subblockString, timeout,
               ref subblockOutput[i].lowerOffsetMeasurementStatus, ref subblockOutput[i].lowerOffsetMargin,
               ref subblockOutput[i].lowerOffsetMarginFrequency, ref subblockOutput[i].lowerOffsetMarginAbsolutePower,
               ref subblockOutput[i].lowerOffsetMarginRelativePower);

            NR.Sem.Results.FetchSubblockMeasurement(subblockString, timeout, out subblockOutput[i].subblockPower,
              out subblockOutput[i].integrationBandwidth, out subblockOutput[i].frequency);
         }

         NR.Sem.Results.FetchTotalAggregatedPower("", timeout, out totalAggregatedPower);
         NR.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
         NR.Sem.Results.FetchSpectrum("", timeout, ref spectrum, ref absoluteMask);
      }

      void PrintResults()
      {
         Console.WriteLine("Total Aggregated Power (dBm)    :{0}", totalAggregatedPower);
         Console.WriteLine("Measurement Status              :{0}", measurementStatus);
         Console.WriteLine("\n--------------------Subblock Measurements--------------------");
         for (var i = 0; i < NumberOfSubblocks; i++)
         {
            Console.WriteLine("\nSubblock {0}\n", i);

            Console.WriteLine("Subblock Power (dBm)            :{0}", subblockOutput[i].subblockPower);
            Console.WriteLine("Integration Bandwidth (Hz)      :{0}", subblockOutput[i].integrationBandwidth);
            Console.WriteLine("Frequency (Hz)                  :{0}", subblockOutput[i].frequency);

            Console.WriteLine("\nOffset Segment Measurements");
            for (int j = 0; j < subblockOutput[i].lowerOffsetMargin.Length; j++)
            {
               Console.WriteLine("\nLower Offset Segement Measurement {0}", j);

               Console.WriteLine("Measurement Status              :{0}", subblockOutput[i].lowerOffsetMeasurementStatus[j]);
               Console.WriteLine("Margin (dB)                     :{0}", subblockOutput[i].lowerOffsetMargin[j]);
               Console.WriteLine("Margin Frequency (Hz)           :{0}", subblockOutput[i].lowerOffsetMarginFrequency[j]);
               Console.WriteLine("Margin Absolute Power (dBm)     :{0}", subblockOutput[i].lowerOffsetMarginAbsolutePower[j]);

               Console.WriteLine("\nUpper Offset Segement Measurement {0}", j);

               Console.WriteLine("Measurement Status              :{0}", subblockOutput[i].upperOffsetMeasurementStatus[j]);
               Console.WriteLine("Margin (dB)                     :{0}", subblockOutput[i].upperOffsetMargin[j]);
               Console.WriteLine("Margin Frequency (Hz)           :{0}", subblockOutput[i].upperOffsetMarginFrequency[j]);
               Console.WriteLine("Margin Absolute Power (dBm)     :{0}", subblockOutput[i].upperOffsetMarginAbsolutePower[j]);
            }
            Console.WriteLine("\n-----------------------------------------------------");
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
