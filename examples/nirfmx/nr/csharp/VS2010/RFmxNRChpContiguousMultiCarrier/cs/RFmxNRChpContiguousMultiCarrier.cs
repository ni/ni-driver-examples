//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//5.Configure Trigger Parameters for Digital Edge Trigger.
//6. Configure Link Direction, Frequency Range, Channel Raster, Component Carrier Spacing and Number of Component Carriers.
//7. Configure Subcarrier Spacing.
//8. Configure Component Carriers.
//9. Select CHP measurement and enable Traces.
//10. Configure Sweep Time Parameters.
//11. Configure Averaging Parameters for CHP measurement.
//12. Initiate the Measurement.
//13. Fetch CHP Measurements and Traces.
//14. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRChpContiguousMultiCarrier
{
   public class RFmxNRChpContiguousMultiCarrier
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

      RFmxNRMXComponentCarrierSpacingType componentCarrierSpacingType;
      double channelRaster;
      int componentCarrierAtCenterFrequency;
      double subcarrierSpacing;

      const int NumberOfComponentCarriers = 2;
      double[] componentCarrierBandwidth = new double[NumberOfComponentCarriers];
      double[] componentCarrierFrequency = new double[NumberOfComponentCarriers];

      RFmxNRMXChpSweepTimeAuto sweepTimeAuto;
      double sweepTimeInterval;

      RFmxNRMXChpAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxNRMXChpAveragingType averagingType;

      string subblockString;
      string carrierString;

      double timeout;

      double totalAggregatedPower;                                               /* (dBm) */

      double[] absolutePower;                                                    /* (dBm) */
      double[] relativePower;                                                    /* (dB) */

      Spectrum<float> spectrum;

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

         componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal;
         channelRaster = 15e3;                                                   /* (Hz) */
         componentCarrierAtCenterFrequency = -1;
         subcarrierSpacing = 30e3;                                               /* (Hz) */

         componentCarrierBandwidth[0] = 100e6;                                   /* (Hz) */
         componentCarrierBandwidth[1] = 100e6;                                   /* (Hz) */
         componentCarrierFrequency[0] = -49.98e6;                                /* (Hz) */
         componentCarrierFrequency[1] = 50.01e6;                                 /* (Hz) */

         sweepTimeAuto = RFmxNRMXChpSweepTimeAuto.True;
         sweepTimeInterval = 1.0e-3;                                             /* (s) */

         averagingEnabled = RFmxNRMXChpAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxNRMXChpAveragingType.Rms;

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

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Chp, true);
         NR.Chp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         NR.Chp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         NR.Initiate("", "");
      }

      void RetrieveResults()
      {
         NR.Chp.Results.ComponentCarrier.FetchMeasurementArray("", timeout, ref absolutePower, ref relativePower);
         NR.Chp.Results.FetchTotalAggregatedPower("", timeout, out totalAggregatedPower);
         NR.Chp.Results.FetchSpectrum("", timeout, ref spectrum);
      }

      void PrintResults()
      {
         Console.WriteLine("Total Aggregated Power (dBm)    : {0}", totalAggregatedPower);

         Console.WriteLine("\nComponent Carrier Measurements:\n");
         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            Console.WriteLine("Carrier  : {0}", i);
            Console.WriteLine("Absolute Power (dBm)            : {0}", absolutePower[i]);
            Console.WriteLine("Relative Power (dB)             : {0}", relativePower[i]);
            Console.WriteLine("-----------------------------------------------------\n");
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
