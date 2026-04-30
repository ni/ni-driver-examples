//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5.Configure Trigger Parameters for Digital Edge Trigger.
//6. Configure Link Direction, Frequency Range, Channel Raster, Component Carrier Spacing and
//   Number of Component Carriers.
//7. Configure Subcarrier Spacing.
//8. Configure Component Carriers.
//9. Select OBW measurement and enable Traces.
//10. Configure Sweep Time Parameters.
//11. Configure Span Parameters for OBW measurement.
//12. Configure Averaging Parameters for OBW measurement.
//13. Initiate the Measurement.
//14. Fetch OBW Measurements and Traces.
//15. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRObwContiguousMultiCarrier
{
   public class RFmxNRObwContiguousMultiCarrier
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
      double subcarrierSpacing;

      RFmxNRMXComponentCarrierSpacingType componentCarrierSpacingType;
      double channelRaster;
      int componentCarrierAtCenterFrequency;

      const int NumberOfComponentCarriers = 2;
      double[] componentCarrierBandwidth = new double[NumberOfComponentCarriers];
      double[] componentCarrierFrequency = new double[NumberOfComponentCarriers];

      RFmxNRMXObwSweepTimeAuto sweepTimeAuto;
      double sweepTimeInterval;

      RFmxNRMXObwSpanAuto spanAuto;
      double span;
      RFmxNRMXObwPowerIntegrationMethod powerIntegrationMethod;

      RFmxNRMXObwAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxNRMXObwAveragingType averagingType;

      string subblockString;
      string carrierString;

      double timeout;

      Spectrum<float> spectrum;

      double occupiedBandwidth;                                                  /* (Hz) */
      double absolutePower;                                                      /* (dBm) */
      double startFrequency;                                                     /* (Hz) */
      double stopFrequency;                                                      /* (Hz) */

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

         enableTrigger = false;
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0;
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                                     /* (s) */

         linkDirection = RFmxNRMXLinkDirection.Uplink;
         frequencyRange = RFmxNRMXFrequencyRange.Range1;
         subcarrierSpacing = 30e3;                                               /* (Hz) */

         componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal;
         channelRaster = 15e3;                                                   /* (Hz) */
         componentCarrierAtCenterFrequency = -1;

         componentCarrierBandwidth[0] = 100e6;                                   /* (Hz) */
         componentCarrierBandwidth[1] = 100e6;                                   /* (Hz) */
         componentCarrierFrequency[0] = -49.98e6;                                /* (Hz) */
         componentCarrierFrequency[1] = 50.01e6;                                 /* (Hz) */

         sweepTimeAuto = RFmxNRMXObwSweepTimeAuto.True;
         sweepTimeInterval = 1.0e-3;                                             /* (s) */

         spanAuto = RFmxNRMXObwSpanAuto.True;
         span = 399.86e6;                                                        /* (Hz) */
		 powerIntegrationMethod = RFmxNRMXObwPowerIntegrationMethod.Normal;
		  
         averagingEnabled = RFmxNRMXObwAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxNRMXObwAveragingType.Rms;

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
         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);

         NR.SetLinkDirection("", linkDirection);
         NR.SetFrequencyRange("", frequencyRange);
         NR.SetChannelRaster("", channelRaster);
         NR.SetComponentCarrierSpacingType("", componentCarrierSpacingType);
         NR.SetComponentCarrierAtCenterFrequency("", componentCarrierAtCenterFrequency);
         NR.ComponentCarrier.SetNumberOfComponentCarriers("", NumberOfComponentCarriers);

         carrierString = "carrier::all";
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing);

         subblockString = RFmxNRMX.BuildSubblockString("", 0);
         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i);
            NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth[i]);
            NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency[i]);
         }

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Obw, true);

         NR.Obw.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         NR.Obw.Configuration.SetSpanAuto("", spanAuto);
		 NR.Obw.Configuration.SetPowerIntegrationMethod("", powerIntegrationMethod);
         NR.Obw.Configuration.SetSpan("subblock0", span);
         NR.Obw.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         NR.Initiate("", "");
      }

      void RetrieveResults()
      {
         NR.Obw.Results.FetchMeasurement("", timeout, out occupiedBandwidth, out absolutePower,
            out startFrequency, out stopFrequency);
         NR.Obw.Results.FetchSpectrum("", timeout, ref spectrum);
      }

      void PrintResults()
      {
         Console.WriteLine("--------------- Measurement ------------------\n");
         Console.WriteLine("Occupied Bandwidth (Hz)  : {0}", occupiedBandwidth);
         Console.WriteLine("Absolute Power (dBm)     : {0}", absolutePower);
         Console.WriteLine("Start Frequency (Hz)     : {0}", startFrequency);
         Console.WriteLine("Stop Frequency (Hz)      : {0}\n", stopFrequency);
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
