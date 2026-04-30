//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation)
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure Component Carriers.
//7. Configure Link Direction.
//8. Select SEM measurement and enable Traces.
//9. Configure Sweep Time Parameters.
//10. Configure Averaging Parameters for SEM measurement.
//11. Configure Uplink Mask Type, or Downlink Mask, eNodeB Category and
//    Component Carrier Maximum Output Power depending on Link Direction.
//12. Initiate the Measurement.
//13. Fetch SEM Measurements and Traces.
//14. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteSemContiguousMultiCarrier
{
   public class RFmxLteSemContiguousMultiCarrier
   {
      RFmxInstrMX instrSession;
      RFmxLteMX lte;
      string resourceName;

      const int numberOfComponentCarriers = 2;

      double centerFrequency;
      double referenceLevel;
      double externalAttenuation;

      string frequencyReferenceSource;
      double frequencyReferenceFrequency;

      bool enableTrigger;
      string digitalEdgeSource;
      RFmxLteMXDigitalEdgeTriggerEdge digitalEdge;
      double triggerDelay;

      RFmxLteMXLinkDirection linkDirection;

      RFmxLteMXSemUplinkMaskType uplinkMaskType;

      RFmxLteMXeNodeBCategory eNodeBCategory;
      RFmxLteMXSemDownlinkMaskType downlinkMaskType;
      double deltaFMaximum;
      double aggregatedMaximumPower;

      RFmxLteMXSemSidelinkMaskType sidelinkMaskType;

      RFmxLteMXComponentCarrierSpacingType componentCarrierSpacingType;
      int componentCarrierAtCenterFrequency;

      double[] componentCarrierBandwidth = { 20e6, 20e6 };              /*(Hz) */
      double[] componentCarrierFrequency = { -9.9e6, 9.9e6 };           /*(Hz) */
      double[] componentCarrierMaximumOutputPower = { 0.0, 0.0 };       /*(dBm) */

      RFmxLteMXSemSweepTimeAuto sweepTimeAuto;
      double sweepTimeInterval;

      RFmxLteMXSemAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxLteMXSemAveragingType averagingType;

      double timeout;

      double totalAggregatedPower;

      RFmxLteMXSemMeasurementStatus measurementStatus;

      RFmxLteMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;
      double[] upperOffsetMargin;
      double[] upperOffsetMarginFrequency;                            /*(Hz) */
      double[] upperOffsetMarginAbsolutePower;                        /*(dBm) */
      double[] upperOffsetMarginRelativePower;                        /*(dBm) */

      RFmxLteMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;
      double[] lowerOffsetMargin;
      double[] lowerOffsetMarginFrequency;                            /*(Hz) */
      double[] lowerOffsetMarginAbsolutePower;                        /*(dBm) */
      double[] lowerOffsetMarginRelativePower;                        /*(dBm) */

      Spectrum<float> spectrum;
      Spectrum<float> absoluteMask;

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

      void InitializeVariables()
      {
         resourceName = "RFSA";

         centerFrequency = 1.95e9;                                   /* (Hz) */
         referenceLevel = 0.0;                                       /* (dBm) */
         externalAttenuation = 0.0;                                  /* (dBm) */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e6;                       /* (Hz) */

         enableTrigger = false;
         digitalEdgeSource = RFmxLteMXConstants.Pfi0;
         digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                         /* (s) */

         linkDirection = RFmxLteMXLinkDirection.Uplink;

         uplinkMaskType = RFmxLteMXSemUplinkMaskType.General_NS01;

         eNodeBCategory = RFmxLteMXeNodeBCategory.WideAreaBaseStationCategoryA;
         deltaFMaximum = 15.0e6;                                     /* (Hz) */
         downlinkMaskType = RFmxLteMXSemDownlinkMaskType.ENodeBCategoryBased;
         aggregatedMaximumPower = 0.0;                               /* (dBm) */

         sidelinkMaskType = RFmxLteMXSemSidelinkMaskType.General_NS01;

         componentCarrierSpacingType = RFmxLteMXComponentCarrierSpacingType.Nominal;
         componentCarrierAtCenterFrequency = -1;

         sweepTimeAuto = RFmxLteMXSemSweepTimeAuto.True;
         sweepTimeInterval = 0.001;                                  /* (s) */

         averagingEnabled = RFmxLteMXSemAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxLteMXSemAveragingType.Rms;

         timeout = 10.0;                                             /* (s) */
      }

      void InitializeInstr()
      {
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      void ConfigureLte()
      {
         lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         lte.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);
         lte.ComponentCarrier.ConfigureSpacing("", componentCarrierSpacingType, componentCarrierAtCenterFrequency);
         lte.ConfigureNumberOfComponentCarriers("", numberOfComponentCarriers);
         lte.ComponentCarrier.ConfigureArray("", componentCarrierBandwidth, componentCarrierFrequency, null);
         lte.ConfigureLinkDirection("", linkDirection);
         lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Sem, true);
         lte.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         lte.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         if (linkDirection == RFmxLteMXLinkDirection.Uplink)
            lte.Sem.Configuration.ConfigureUplinkMaskType("", uplinkMaskType);
         else if (linkDirection == RFmxLteMXLinkDirection.Downlink)
         {
            lte.ConfigureeNodeBCategory("", eNodeBCategory);
            lte.Sem.Configuration.ConfigureDownlinkMask("", downlinkMaskType, deltaFMaximum, aggregatedMaximumPower);
            lte.Sem.Configuration.ComponentCarrier.ConfigureMaximumOutputPowerArray("", componentCarrierMaximumOutputPower);
         }
         else
            lte.Sem.Configuration.SetSidelinkMaskType("", sidelinkMaskType);
         lte.Initiate("", "");
      }

      void RetrieveResults()
      {
         lte.Sem.Results.FetchTotalAggregatedPower("", timeout, out totalAggregatedPower);

         lte.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);

         lte.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref upperOffsetMeasurementStatus,
            ref upperOffsetMargin, ref upperOffsetMarginFrequency, ref upperOffsetMarginAbsolutePower,
            ref upperOffsetMarginRelativePower);

         lte.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref lowerOffsetMeasurementStatus,
            ref lowerOffsetMargin, ref lowerOffsetMarginFrequency, ref lowerOffsetMarginAbsolutePower,
            ref lowerOffsetMarginRelativePower);

         lte.Sem.Results.FetchSpectrum("", timeout, ref spectrum, ref absoluteMask);
      }

      void PrintResults()
      {
         Console.WriteLine("Total Aggregated Power (dBm)    :{0}", totalAggregatedPower);
         Console.WriteLine("Measurement Status              :{0}", measurementStatus);

         Console.WriteLine("\n--------  Lower Offset Segement Measurement --------\n");
         for (int i = 0; i < lowerOffsetMargin.Length; i++)
         {
            Console.WriteLine("\nOffset {0}", i);
            Console.WriteLine("Measurement Status              :{0}", lowerOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin (dB)                     :{0}", lowerOffsetMargin[i]);
            Console.WriteLine("Margin Frequency (Hz)           :{0}", lowerOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power (dBm)     :{0}", lowerOffsetMarginAbsolutePower[i]);
         }

         Console.WriteLine("\n--------  Upper  Offset Segement Measurement --------\n");
         for (int i = 0; i < upperOffsetMargin.Length; i++)
         {
            Console.WriteLine("\nOffset {0}", i);
            Console.WriteLine("Measurement Status              :{0}", upperOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin (dB)                     :{0}", upperOffsetMargin[i]);
            Console.WriteLine("Margin Frequency (Hz)           :{0}", upperOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power (dBm)     :{0}", upperOffsetMarginAbsolutePower[i]);
         }
      }

      void CloseSession()
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

      static void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

   }
}