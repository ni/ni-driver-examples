//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select Carrier Bandwidth as 200k.
//6. Configure Uplink Subcarrier Spacing.
//7. Select SEM measurement and enable Traces.
//8. Configure Sweep Time Parameters.
//9. Configure Averaging Parameters for SEM measurement.
//10. Initiate the Measurement.
//11. Fetch SEM Measurements and Traces.
//12. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteNBIoTSem
{
   public class RFmxLteNBIoTSem
   {
      RFmxInstrMX instrSession;
      RFmxLteMX lte;
      string rfsaResourceName;

      string frequencyReferenceSource;
      double frequencyReferenceFrequency;

      double centerFrequency;
      double referenceLevel;
      double externalAttenuation;

      bool enableTrigger;
      RFmxLteMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
      double iqPowerEdgeTriggerLevel;
      RFmxLteMXTriggerMinimumQuietTimeMode minimumQuiteTimeMode;
      double minimumQuietTime;
      RFmxLteMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
      double triggerDelay;
	  RFmxLteMXLinkDirection linkDirection;

      RFmxLteMXNBIoTUplinkSubcarrierSpacing uplinkSubcarrierSpacing;
	  RFmxLteMXeNodeBCategory eNodeBCategory;
      RFmxLteMXSemDownlinkMaskType downlinkMaskType;
      double deltaFMaximum;
	  double aggregatedMaximumPower;
      double maximumOutputPower;

      RFmxLteMXSemSweepTimeAuto sweepTimeAuto;
      double sweepTimeInterval;

      RFmxLteMXSemAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxLteMXSemAveragingType averagingType;

      double componentCarrierBandwidth;
      double componentCarrierFrequency;
      int cellID;

      int nCellID;

      double timeout;

      RFmxLteMXSemMeasurementStatus measurementStatus;

      double absoluteIntegratedPower;                                /*(dBm) */
      double relativeIntegratedPower;                                /*(dBm) */

      RFmxLteMXSemUpperOffsetMeasurementStatus[] upperOffsetMeasurementStatus;
      double[] upperOffsetMargin;                                    /*(dB) */
      double[] upperOffsetMarginFrequency;                           /*(Hz) */
      double[] upperOffsetMarginAbsolutePower;                       /*(dBm) */
      double[] upperOffsetMarginRelativePower;                       /*(dBm) */

      RFmxLteMXSemLowerOffsetMeasurementStatus[] lowerOffsetMeasurementStatus;
      double[] lowerOffsetMargin;                                    /*(dB) */
      double[] lowerOffsetMarginFrequency;                           /*(Hz) */
      double[] lowerOffsetMarginAbsolutePower;                       /*(dBm) */
      double[] lowerOffsetMarginRelativePower;                       /*(dBm) */

      Spectrum<float> spectrum;                                      /*(dBm) */
      Spectrum<float> compositeMask;

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

      void InitializeInstr()
      {
         instrSession = new RFmxInstrMX(rfsaResourceName, "");
      }

      void InitializeVariables()
      {
         rfsaResourceName = "RFSA";

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10e6;                       /* (Hz) */

         centerFrequency = 1.95e9;                                 /* (Hz) */
         referenceLevel = 0.00;                                    /* (dBm) */
         externalAttenuation = 0.0;                                /* (dB) */

         enableTrigger = true;
         iqPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising;
         iqPowerEdgeTriggerLevel = -20.0;                          /* (dB) */
         minimumQuiteTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto;
         minimumQuietTime = 100e-6;                                /*(s) */
         iqPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative;
         triggerDelay = 0.0;                                       /*(s) */
		 
		 linkDirection = RFmxLteMXLinkDirection.Uplink;

         uplinkSubcarrierSpacing = RFmxLteMXNBIoTUplinkSubcarrierSpacing.SubcarrierSpacing15kHz;
		 
		 eNodeBCategory = RFmxLteMXeNodeBCategory.WideAreaBaseStationCategoryA;
         downlinkMaskType = RFmxLteMXSemDownlinkMaskType.ENodeBCategoryBased;
         deltaFMaximum = 15.0e6;                                   /* (Hz) */
		 aggregatedMaximumPower = 0.0;                             /* (dBm) */
         maximumOutputPower = 0.0;                                 /* (dBm) */


         sweepTimeAuto = RFmxLteMXSemSweepTimeAuto.True;
         sweepTimeInterval = 0.001;                                /* (s) */

         averagingEnabled = RFmxLteMXSemAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxLteMXSemAveragingType.Rms;

         componentCarrierBandwidth = 200e3;                        /* (Hz) */
         componentCarrierFrequency = 0.0;                          /* (Hz) */
         cellID = 0;

         nCellID = 0;

         timeout = 10.0;                                           /* (s) */
      }

      void ConfigureLte()
      {
         lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         lte.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel, triggerDelay,
            minimumQuiteTimeMode, minimumQuietTime, iqPowerEdgeTriggerLevelType, enableTrigger);
	     lte.ConfigureLinkDirection("", linkDirection);
         lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID);
         lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.Sem, true);
         lte.Sem.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         lte.Sem.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
		 if (linkDirection == RFmxLteMXLinkDirection.Uplink)
           lte.ComponentCarrier.ConfigureNBIoTComponentCarrier("", nCellID, uplinkSubcarrierSpacing);
         else if (linkDirection == RFmxLteMXLinkDirection.Downlink)
         {
            lte.ConfigureeNodeBCategory("", eNodeBCategory);
            lte.Sem.Configuration.ConfigureDownlinkMask("", downlinkMaskType, deltaFMaximum, aggregatedMaximumPower);
            lte.Sem.Configuration.ComponentCarrier.ConfigureMaximumOutputPower("", maximumOutputPower);
         }
        
         lte.Initiate("", "");
      }

      void RetrieveResults()
      {
         lte.Sem.Results.FetchUpperOffsetMarginArray("", timeout, ref upperOffsetMeasurementStatus, ref upperOffsetMargin,
            ref upperOffsetMarginFrequency, ref upperOffsetMarginAbsolutePower, ref upperOffsetMarginRelativePower);
         lte.Sem.Results.FetchLowerOffsetMarginArray("", timeout, ref lowerOffsetMeasurementStatus,
            ref lowerOffsetMargin, ref lowerOffsetMarginFrequency, ref lowerOffsetMarginAbsolutePower, ref lowerOffsetMarginRelativePower);
         lte.Sem.Results.ComponentCarrier.FetchMeasurement("", timeout, out absoluteIntegratedPower, out relativeIntegratedPower);
         lte.Sem.Results.FetchMeasurementStatus("", timeout, out measurementStatus);
         lte.Sem.Results.FetchSpectrum("", timeout, ref spectrum, ref compositeMask);
      }

      void PrintResults()
      {
         Console.WriteLine("Measurement Status                         :{0}", measurementStatus);
         Console.WriteLine("Carrier Absolute Integrated Power (dBm)    :{0}", absoluteIntegratedPower);

         Console.WriteLine("\n\n----------Lower Offset Segment Measurements----------\n");
         for (int i = 0; i < lowerOffsetMargin.Length; i++)
         {
            Console.WriteLine("Offset {0}", i);
            Console.WriteLine("Measurement Status              :{0}",
                 lowerOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin (dB)                     :{0}", lowerOffsetMargin[i]);
            Console.WriteLine("Margin Frequency (Hz)           :{0}", lowerOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power (dBm)     :{0}\n", lowerOffsetMarginAbsolutePower[i]);
         }

         Console.WriteLine("\n\n----------Upper Offset Segment Measurements----------\n");
         for (int i = 0; i < upperOffsetMargin.Length; i++)
         {
            Console.WriteLine("Offset {0}", i);
            Console.WriteLine("Measurement Status              :{0}", upperOffsetMeasurementStatus[i]);
            Console.WriteLine("Margin (dB)                     :{0}", upperOffsetMargin[i]);
            Console.WriteLine("Margin Frequency (Hz)           :{0}", upperOffsetMarginFrequency[i]);
            Console.WriteLine("Margin Absolute Power (dBm)     :{0}\n", upperOffsetMarginAbsolutePower[i]);
         }
      }

      static void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

   }
}
