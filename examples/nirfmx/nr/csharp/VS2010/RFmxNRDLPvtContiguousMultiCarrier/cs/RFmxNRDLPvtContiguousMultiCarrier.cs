//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Link Direction as Downlink, Frequency Range, Channel Raster, Component Carrier Spacing and gNodeB Type.
//7. Configure Carrier.
//8. Configure BWP Subcarrier Spacing, DL Test Model Duplex Scheme and DL Test Model for all carriers
//9. Select PVT measurement and enable Traces.
//10. Configure Measurement Methods.
//11. Configure OFF Power Exclusion Periods.
//12. Configure Averaging Parameters for PVT measurement.
//13. Configure Measurement Interval.
//14. Initiate the Measurement.
//15. Fetch PVT Measurements and Traces.
//16. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRDLPvtContiguousMultiCarrier
{
   public class RFmxNRDLPvtContiguousMultiCarrier
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
      RFmxNRMXComponentCarrierSpacingType componentCarrierSpacingType;
      RFmxNRMXPvtMeasurementMethod measurementMethod;
      double offPowerExclusionBefore;
      double offPowerExclusionAfter;
      RFmxNRMXgNodeBType gNodeBType;

      double channelRaster;
      int componentCarrierAtCenterFrequency;
      double subcarrierSpacing;

      const int NumberOfComponentCarriers = 2;
      double[] componentCarrierBandwidth = new double[NumberOfComponentCarriers];
      double[] componentCarrierFrequency = new double[NumberOfComponentCarriers];
      double[] ratedTRP = new double[NumberOfComponentCarriers];
      double[] ratedEIRP = new double[NumberOfComponentCarriers];

      RFmxNRMXDownlinkTestModel downlinkTestModel;
      RFmxNRMXDownlinkTestModelDuplexScheme downlinkTestModelDuplexScheme;
      

      RFmxNRMXPvtAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxNRMXPvtAveragingType averagingType;

      RFmxNRMXPvtMeasurementIntervalAuto measurementIntervalAuto;
      double measurementInterval;

      string subblockString;
      string carrierString;

      double timeout;

      RFmxNRMXPvtMeasurementStatus[] measurementStatus = new RFmxNRMXPvtMeasurementStatus[NumberOfComponentCarriers];
      double[] pvtResultsPkWindowedOffPwr = new double[NumberOfComponentCarriers];                           /* (dBm/MHz) */
      double[] pvtResultsPkWindowedOffPwrMargin = new double[NumberOfComponentCarriers];                     /* (dB) */
      double[] pvtResultsPkWindowedOffPwrTime = new double[NumberOfComponentCarriers];                       /* (s) */
      double[] absoluteONPower = new double[NumberOfComponentCarriers];                                      /* (dBm) */

      AnalogWaveform<float>[] signalPower = new AnalogWaveform<float>[NumberOfComponentCarriers];            /* (dBm/MHz) */
      AnalogWaveform<float>[] absoluteLimit = new AnalogWaveform<float>[NumberOfComponentCarriers];          /* (dBm/MHz) */
      AnalogWaveform<float>[] windowedSignalPower = new AnalogWaveform<float>[NumberOfComponentCarriers];    /* (dBm/MHz) */

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
         frequencyReferenceFrequency = 10.0e6;                                         /* (Hz) */

         iqPowerEdgeLevel = -20.0;                                                     /* (dB) */
         triggerDelay = 0.0;                                                           /* (s) */
         minimumQuietTimeMode = RFmxNRMXTriggerMinimumQuietTimeMode.Auto;
         minimumQuietTime = 8.0e-6;                                                    /* (s) */

         frequencyRange = RFmxNRMXFrequencyRange.Range1;
         subcarrierSpacing = 30e3;                                                     /* (Hz) */
         measurementMethod = RFmxNRMXPvtMeasurementMethod.Normal;
         offPowerExclusionBefore = 0.0;                                                /* (s) */
         offPowerExclusionAfter = 0.0;                                                 /* (s) */
         gNodeBType = RFmxNRMXgNodeBType.Type1C;

         componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal;
         channelRaster = 15e3;                                                         /* (Hz) */
         componentCarrierAtCenterFrequency = -1;

         componentCarrierBandwidth[0] = 100e6;                                         /* (Hz) */
         componentCarrierBandwidth[1] = 100e6;                                         /* (Hz) */
         componentCarrierFrequency[0] = -49.98e6;                                      /* (Hz) */
         componentCarrierFrequency[1] = 50.01e6;                                       /* (Hz) */
         ratedTRP[0] = 0.0;                                                            /* (dBm) */
         ratedTRP[1] = 0.0;                                                            /* (dBm) */
         ratedEIRP[0] = 0.0;                                                           /* (dBm) */
         ratedEIRP[1] = 0.0;                                                           /* (dBm) */

         downlinkTestModel = RFmxNRMXDownlinkTestModel.TM1_1;
         downlinkTestModelDuplexScheme = RFmxNRMXDownlinkTestModelDuplexScheme.Tdd;
         
         averagingEnabled = RFmxNRMXPvtAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxNRMXPvtAveragingType.Rms;

         measurementIntervalAuto = RFmxNRMXPvtMeasurementIntervalAuto.True;
         measurementInterval = 0.01;                                                   /* (s) */

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
         NR.ConfigureIQPowerEdgeTrigger("", "0", RFmxNRMXIQPowerEdgeTriggerSlope.Rising, iqPowerEdgeLevel,
            triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFmxNRMXIQPowerEdgeTriggerLevelType.Relative, true);

         NR.SetLinkDirection("", RFmxNRMXLinkDirection.Downlink);
         NR.SetFrequencyRange("", frequencyRange);
         NR.SetChannelRaster("", channelRaster);
         NR.SetComponentCarrierSpacingType("", componentCarrierSpacingType);
         NR.SetComponentCarrierAtCenterFrequency("", componentCarrierAtCenterFrequency);
         NR.SetgNodeBType("", gNodeBType);

         NR.ComponentCarrier.SetNumberOfComponentCarriers("", NumberOfComponentCarriers);

         subblockString = RFmxNRMX.BuildSubblockString("", 0);
         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i);
            NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth[i]);
            NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency[i]);
            NR.ComponentCarrier.SetRatedTrp(carrierString, ratedTRP[i]);
            NR.ComponentCarrier.SetRatedEirp(carrierString, ratedEIRP[i]);
         }

         carrierString = "carrier::all";

         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing);
         NR.ComponentCarrier.SetDownlinkTestModelDuplexScheme(carrierString, downlinkTestModelDuplexScheme);
         NR.ComponentCarrier.SetDownlinkTestModel(carrierString, downlinkTestModel);

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
         subblockString = RFmxNRMX.BuildSubblockString("", 0);
         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i);
            NR.Pvt.Results.GetMeasurementStatus(carrierString, out measurementStatus[i]);
            NR.Pvt.Results.GetPeakWindowedOffPower(carrierString, out pvtResultsPkWindowedOffPwr[i]);
            NR.Pvt.Results.GetPeakWindowedOffPowerMargin(carrierString, out pvtResultsPkWindowedOffPwrMargin[i]);
            NR.Pvt.Results.GetPeakWindowedOffPowerTime(carrierString, out pvtResultsPkWindowedOffPwrTime[i]);
            NR.Pvt.Results.GetAbsoluteONPower(carrierString, out absoluteONPower[i]);
         }

         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            carrierString = RFmxNRMX.BuildCarrierString("", i);
            NR.Pvt.Results.FetchSignalPowerTrace(carrierString, timeout, ref signalPower[i], ref absoluteLimit[i]);

            NR.Pvt.Results.FetchWindowedSignalPowerTrace(carrierString, timeout, ref windowedSignalPower[i]);
         }
      }

      void PrintResults()
      {
         Console.WriteLine("------------------------Measurements------------------------\n");
         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            Console.WriteLine("Carrier  : {0}", i);
            Console.WriteLine("Measurement Status                                   : {0}", measurementStatus[i]);
            Console.WriteLine("PVT Results Pk Windowed OFF Pwr (dBm/MHz)            : {0}", pvtResultsPkWindowedOffPwr[i]);
            Console.WriteLine("PVT Results Pk Windowed OFF Pwr Margin (dB)          : {0}", pvtResultsPkWindowedOffPwrMargin[i]);
            Console.WriteLine("PVT Results Pk Windowed OFF Pwr Time (s)             : {0}", pvtResultsPkWindowedOffPwrTime[i]);
            Console.WriteLine("Absolute ON Power (dBm)                              : {0}", absoluteONPower[i]);
            Console.WriteLine("-----------------------------------------------------------------\n");
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
