//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Parameters for IQ Power Edge Trigger.
//6. Configure Link Direction, Frequency Range, Carrier Bandwidth and BWP Subcarrier Spacing.
//7. Select SEM measurement and enable Traces.
//8. Configure Uplink Mask Type, or Downlink Mask, Band, gNodeB Category, Delta F_Max(Hz) and
//   Component Carrier Rated Output Power based on Link Direction.
//9. Configure Sweep Time Parameters.
//10. Configure Averaging Parameters for SEM measurement.
//11. Initiate the Measurement.
//12. Fetch SEM Measurements and Traces.
//13. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRSemSingleCarrier
{
   public class RFmxNRSemSingleCarrier
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

      bool iqPowerEdgeEnabled;
      double iqPowerEdgeLevel;
      double triggerDelay;
      RFmxNRMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
      double minimumQuietTime;

      RFmxNRMXLinkDirection linkDirection;

      RFmxNRMXFrequencyRange frequencyRange;

      RFmxNRMXSemUplinkMaskType uplinkMaskType;

      RFmxNRMXgNodeBCategory gNodeBCategory;
      RFmxNRMXSemDownlinkMaskType downlinkMaskType;
      double deltaFMaximum;
      double componentCarrierRatedOutputPower;
      int band;

      double carrierBandwidth;
      double subcarrierSpacing;

      RFmxNRMXSemSweepTimeAuto sweepTimeAuto;
      double sweepTimeInterval;

      RFmxNRMXSemAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxNRMXSemAveragingType averagingType;

      double timeout;

      RFmxNRMXSemMeasurementStatus measurementStatus;

      double absolutePower;                                                      /* (dBm) */
      double peakAbsolutePower;                                                  /* (dBm) */
      double peakFrequency;                                                      /* (Hz) */
      double relativePower;                                                      /* (dB) */

      RFmxNRMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;
      double[] upperOffsetMargin;                                                /* (dB) */
      double[] upperOffsetMarginFrequency;                                       /* (Hz) */
      double[] upperOffsetMarginAbsolutePower;                                   /* (dBm) */
      double[] upperOffsetMarginRelativePower;                                   /* (dB) */

      RFmxNRMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;
      double[] lowerOffsetMargin;                                                /* (dB) */
      double[] lowerOffsetMarginFrequency;                                       /* (Hz) */
      double[] lowerOffsetMarginAbsolutePower;                                   /* (dBm) */
      double[] lowerOffsetMarginRelativePower;                                   /* (dB) */

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
         centerFrequency = 3.5e9;                                                /* (Hz) */
         referenceLevel = 0.0;                                                   /* (dBm) */
         externalAttenuation = 0.0;                                              /* (dB) */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10e6;                                     /* (Hz) */

         iqPowerEdgeEnabled = false;
         iqPowerEdgeLevel = -20.0;                                               /* (dB or dBm) */
         triggerDelay = 0.0;                                                     /* (s) */
         minimumQuietTimeMode = RFmxNRMXTriggerMinimumQuietTimeMode.Auto;
         minimumQuietTime = 8.0e-6;                                              /* (s) */

         linkDirection = RFmxNRMXLinkDirection.Uplink;

         frequencyRange = RFmxNRMXFrequencyRange.Range1;

         uplinkMaskType = RFmxNRMXSemUplinkMaskType.General;

         gNodeBCategory = RFmxNRMXgNodeBCategory.WideAreaBaseStationCategoryA;
         downlinkMaskType = RFmxNRMXSemDownlinkMaskType.Standard;
         deltaFMaximum = 15.0e6;                                                 /* (Hz) */
         componentCarrierRatedOutputPower = 0.0;                                 /* (dBm) */
         band = 78;

         carrierBandwidth = 100e6;                                               /* (Hz) */
         subcarrierSpacing = 30e3;                                               /* (Hz) */

         sweepTimeAuto = RFmxNRMXSemSweepTimeAuto.True;
         sweepTimeInterval = 1.0e-3;                                             /* (s) */

         averagingEnabled = RFmxNRMXSemAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxNRMXSemAveragingType.Rms;

         timeout = 10.0;                                                         /* (s) */
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
         NR.ConfigureIQPowerEdgeTrigger("", "0", RFmxNRMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
            triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxNRMXIQPowerEdgeTriggerLevelType.Relative,
            iqPowerEdgeEnabled);

         NR.SetLinkDirection("", linkDirection);
         NR.SetFrequencyRange("", frequencyRange);
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth);
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing);

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Sem, true);

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
            NR.Sem.Configuration.ComponentCarrier.ConfigureRatedOutputPower("", componentCarrierRatedOutputPower);
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

         NR.Sem.Results.ComponentCarrier.FetchMeasurement("", timeout, out absolutePower, out peakAbsolutePower,
            out peakFrequency, out relativePower);

         NR.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);

         NR.Sem.Results.FetchSpectrum("", timeout, ref spectrum, ref compositeMask);
      }

      void PrintResults()
      {
         Console.WriteLine("Measurement Status                       : {0}", measurementStatus);
         Console.WriteLine("Carrier Absolute Integrated Power (dBm)  : {0}", absolutePower);

         Console.WriteLine("\n----------Lower Offset Segment Measurements----------\n");
         for (int i = 0; i < lowerOffsetMargin.Length; i++)
         {
            Console.WriteLine("Offset  {0}", i);
            Console.WriteLine("Measurement Status                       : {0}", lowerOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin (dB)                              : {0}", lowerOffsetMargin[i]);
            Console.WriteLine("Margin Frequency (Hz)                    : {0}", lowerOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power (dBm)              : {0}\n", lowerOffsetMarginAbsolutePower[i]);
         }

         Console.WriteLine("\n----------Upper Offset Segment Measurements----------\n");
         for (int i = 0; i < upperOffsetMargin.Length; i++)
         {
            Console.WriteLine("Offset  {0}", i);
            Console.WriteLine("Measurement Status                       : {0}", upperOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin (dB)                              : {0}", upperOffsetMargin[i]);
            Console.WriteLine("Margin Frequency (Hz)                    : {0}", upperOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power (dBm)              : {0}\n", upperOffsetMarginAbsolutePower[i]);
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
