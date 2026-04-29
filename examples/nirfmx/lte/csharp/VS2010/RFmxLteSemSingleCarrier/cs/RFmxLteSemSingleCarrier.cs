//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Configure Link Direction.
//7. Select SEM measurement and enable Traces.
//8. Configure Sweep Time Parameters.
//9. Configure Averaging Parameters for SEM measurement.
//10. Configure Uplink Mask Type, or Downlink Mask, eNodeB Category and
//    Component Carrier Maximum Output Power depending on Link Direction.
//11. Initiate the Measurement.
//12. Fetch SEM Measurements and Traces.
//13. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteSemSingleCarrier
{
   public class RFmxLteSemSingleCarrier
   {
      RFmxInstrMX instrSession;
      RFmxLteMX lte;
      string resourceName;

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
      double maximumOutputPower;

      RFmxLteMXSemSidelinkMaskType sidelinkMaskType;

      double componentCarrierBandwidth;

      RFmxLteMXSemSweepTimeAuto sweepTimeAuto;
      double sweepTimeInterval;

      RFmxLteMXSemAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxLteMXSemAveragingType averagingType;

      double timeout;

      double absoluteIntegratedPower;
      double relativeIntegratedPower;

      Spectrum<float> spectrum;
      Spectrum<float> absoluteMask;
      RFmxLteMXSemMeasurementStatus measurementStatus;

      RFmxLteMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;
      double[] upperOffsetMargin;
      double[] upperOffsetMarginFrequency;                          /*(Hz) */
      double[] upperOffsetMarginAbsolutePower;                      /*(dBm) */
      double[] upperOffsetMarginRelativePower;                      /*(dBm) */

      RFmxLteMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;
      double[] lowerOffsetMargin;
      double[] lowerOffsetMarginFrequency;                          /*(Hz) */
      double[] lowerOffsetMarginAbsolutePower;                      /*(dBm) */
      double[] lowerOffsetMarginRelativePower;                      /*(dBm) */

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

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e6;                     /* (Hz) */

         centerFrequency = 1.95e9;                                 /* (Hz) */
         referenceLevel = 0.0;                                     /* (dBm) */
         externalAttenuation = 0.0;                                /* (dBm) */

         enableTrigger = false;
         digitalEdgeSource = RFmxLteMXConstants.Pfi0;
         digitalEdge = RFmxLteMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                       /* (s) */

         linkDirection = RFmxLteMXLinkDirection.Uplink;

         uplinkMaskType = RFmxLteMXSemUplinkMaskType.General_NS01;

         eNodeBCategory = RFmxLteMXeNodeBCategory.WideAreaBaseStationCategoryA;
         downlinkMaskType = RFmxLteMXSemDownlinkMaskType.ENodeBCategoryBased;
         deltaFMaximum = 15.0e6;                                   /* (Hz) */
         aggregatedMaximumPower = 0.0;                             /* (dBm) */
         maximumOutputPower = 0.0;                                 /* (dBm) */

         sidelinkMaskType = RFmxLteMXSemSidelinkMaskType.General_NS01;

         componentCarrierBandwidth = 10.0e6;                       /* (Hz) */

         sweepTimeAuto = RFmxLteMXSemSweepTimeAuto.True;
         sweepTimeInterval = 0.001;                                /* (s) */

         averagingEnabled = RFmxLteMXSemAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxLteMXSemAveragingType.Rms;

         timeout = 10.0;                                           /* (s) */
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
         lte.ComponentCarrier.Configure("", componentCarrierBandwidth, 0.0, 0);
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
            lte.Sem.Configuration.ComponentCarrier.ConfigureMaximumOutputPower("", maximumOutputPower);
         }
         else
            lte.Sem.Configuration.SetSidelinkMaskType("", sidelinkMaskType);
         lte.Initiate("", "");
      }

      void RetrieveResults()
      {
         lte.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref upperOffsetMeasurementStatus,
            ref upperOffsetMargin, ref upperOffsetMarginFrequency, ref upperOffsetMarginAbsolutePower,
            ref upperOffsetMarginRelativePower);
         lte.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref lowerOffsetMeasurementStatus,
            ref lowerOffsetMargin, ref lowerOffsetMarginFrequency, ref lowerOffsetMarginAbsolutePower,
            ref lowerOffsetMarginRelativePower);
         lte.Sem.Results.ComponentCarrier.FetchMeasurement("", timeout, out absoluteIntegratedPower,
            out relativeIntegratedPower);
         lte.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
         lte.Sem.Results.FetchSpectrum("", timeout, ref spectrum, ref absoluteMask);
      }

      void PrintResults()
      {
         Console.WriteLine("Measurement Status              :{0}", measurementStatus);
         Console.WriteLine("Carrier Absolute Power (dBm)    :{0}", absoluteIntegratedPower);

         Console.WriteLine("\n----------Lower Offset Segment Measurements----------\n\n");
         for (int i = 0; i < lowerOffsetMargin.Length; i++)
         {
            Console.WriteLine("Offset {0}", i);
            Console.WriteLine("Measurement Status              :{0}",
                 lowerOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin (dB)                     :{0}", lowerOffsetMargin[i]);
            Console.WriteLine("Margin Frequency (Hz)           :{0}", lowerOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power (dBm)     :{0}", lowerOffsetMarginAbsolutePower[i]);
         }

         Console.WriteLine("\n----------Upper Offset Segment Measurements----------\n\n");
         for (int i = 0; i < upperOffsetMargin.Length; i++)
         {
            Console.WriteLine("Offset {0}", i);
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
