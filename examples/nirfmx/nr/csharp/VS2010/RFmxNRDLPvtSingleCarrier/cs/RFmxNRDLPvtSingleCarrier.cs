//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Link Direction as Downlink, Frequency Range, Carrier Bandwidth and BWP Subcarrier Spacing.
//7. Configure DL Test Model and DL Test Model Duplex Scheme.
//8. Configure gNodeB Type.
//9. Configure Rated TRP and Rated EIRP.
//10. Select PVT measurement and enable Traces.
//11. Configure Measurement Methods.
//12. Configure OFF Power Exclusion Periods.
//13. Configure Averaging Parameters for PVT measurement.
//14. Configure Measurement Interval.
//15. Initiate the Measurement.
//16. Fetch PVT Measurements and Traces.
//17. Close RFmx Session. 

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRDLPvtSingleCarrier
{
   public class RFmxNRDLPvtSingleCarrier
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

      double iqPowerEdgeLevel;
      double triggerDelay;
      RFmxNRMXTriggerMinimumQuietTimeMode minimumQuietTimeMode;
      double minimumQuietTime;

      RFmxNRMXFrequencyRange frequencyRange;
      double carrierBandwidth;
      double subcarrierSpacing;
      RFmxNRMXDownlinkTestModel downlinkTestModel;
      RFmxNRMXDownlinkTestModelDuplexScheme downlinkTestModelDuplexScheme;

      RFmxNRMXgNodeBType gNodeBType;
      double ratedTRP;
      double ratedEIRP;
      RFmxNRMXPvtMeasurementMethod measurementMethod;
      double offPowerExclusionBefore;
      double offPowerExclusionAfter;

      RFmxNRMXPvtAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxNRMXPvtAveragingType averagingType;

      RFmxNRMXPvtMeasurementIntervalAuto measurementIntervalAuto;
      double measurementInterval;

      double timeout;

      RFmxNRMXPvtMeasurementStatus measurementStatus;
      double pvtResultsPkWindowedOffPwr;                                         /* (dBm/MHz) */
      double pvtResultsPkWindowedOffPwrMargin;                                   /* (dB) */
      double pvtResultsPkWindowedOffPwrTime;                                     /* (s) */
      double absoluteONPower;                                                    /* (dBm) */

      AnalogWaveform<float> signalPower;                                         /* (dBm/MHz) */
      AnalogWaveform<float> absoluteLimit;                                       /* (dBm/MHz) */
      AnalogWaveform<float> windowedSignalPower;                                 /* (dBm/MHz) */

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
         frequencyReferenceFrequency = 10.0e6;                                   /* (Hz) */

         iqPowerEdgeLevel = -20.0;                                               /* (dB) */
         triggerDelay = 0.0;                                                     /* (s) */
         minimumQuietTimeMode = RFmxNRMXTriggerMinimumQuietTimeMode.Auto;
         minimumQuietTime = 8.0e-6;                                              /* (s) */

         frequencyRange = RFmxNRMXFrequencyRange.Range1;
         carrierBandwidth = 100e6;                                               /* (Hz) */
         subcarrierSpacing = 30e3;                                               /* (Hz) */
         downlinkTestModelDuplexScheme = RFmxNRMXDownlinkTestModelDuplexScheme.Tdd;
         downlinkTestModel = RFmxNRMXDownlinkTestModel.TM1_1;

         gNodeBType = RFmxNRMXgNodeBType.Type1C;
         ratedTRP = 0.0;                                                         /* (dBm) */
         ratedEIRP = 0.0;                                                        /* (dBm) */
         measurementMethod = RFmxNRMXPvtMeasurementMethod.Normal;
         offPowerExclusionBefore = 0.0;                                          /* (s) */
         offPowerExclusionAfter = 0.0;                                           /* (s) */

         averagingEnabled = RFmxNRMXPvtAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxNRMXPvtAveragingType.Rms;

         measurementIntervalAuto = RFmxNRMXPvtMeasurementIntervalAuto.True;
         measurementInterval = 0.01;                                             /* (s) */

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
            triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxNRMXIQPowerEdgeTriggerLevelType.Relative, true);

         NR.SetLinkDirection("", RFmxNRMXLinkDirection.Downlink);
         NR.SetFrequencyRange("", frequencyRange);
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth);
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing);

         NR.ComponentCarrier.SetDownlinkTestModel("", downlinkTestModel);
         NR.ComponentCarrier.SetDownlinkTestModelDuplexScheme("", downlinkTestModelDuplexScheme);

         NR.SetgNodeBType("", gNodeBType);

         NR.ComponentCarrier.SetRatedTrp("", ratedTRP);
         NR.ComponentCarrier.SetRatedEirp("", ratedEIRP);

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Pvt, true);

         NR.Pvt.Configuration.ConfigureMeasurementMethod("", measurementMethod);
         NR.Pvt.Configuration.ConfigureOffPowerExclusionPeriods("", offPowerExclusionBefore, offPowerExclusionAfter);
         NR.Pvt.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         NR.Pvt.Configuration.SetMeasurementIntervalAuto("", measurementIntervalAuto);
         NR.Pvt.Configuration.SetMeasurementInterval("", measurementInterval);

         NR.Initiate("", "");
      }

      void RetrieveResults()
      {
         NR.Pvt.Results.GetMeasurementStatus("", out measurementStatus);
         NR.Pvt.Results.GetPeakWindowedOffPower("", out pvtResultsPkWindowedOffPwr);
         NR.Pvt.Results.GetPeakWindowedOffPowerMargin("", out pvtResultsPkWindowedOffPwrMargin);
         NR.Pvt.Results.GetPeakWindowedOffPowerTime("", out pvtResultsPkWindowedOffPwrTime);
         NR.Pvt.Results.GetAbsoluteONPower("", out absoluteONPower);

         NR.Pvt.Results.FetchSignalPowerTrace("", timeout, ref signalPower, ref absoluteLimit);
         
         NR.Pvt.Results.FetchWindowedSignalPowerTrace("", timeout, ref windowedSignalPower);
      }

      void PrintResults()
      {
         Console.WriteLine("------------------Measurement------------------\n");
         Console.WriteLine("Measurement Status                             : {0}", measurementStatus);
         Console.WriteLine("PVT Results Pk Windowed OFF Pwr (dBm/MHz)      : {0}", pvtResultsPkWindowedOffPwr);
         Console.WriteLine("PVT Results Pk Windowed OFF Pwr Margin (dB)    : {0}", pvtResultsPkWindowedOffPwrMargin);
         Console.WriteLine("PVT Results Pk Windowed OFF Pwr Time (s)       : {0}", pvtResultsPkWindowedOffPwrTime);
         Console.WriteLine("Absolute ON Power (dBm)                        : {0}", absoluteONPower);
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
