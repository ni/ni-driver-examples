//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Parameters for Digital Edge Trigger.
//6. Configure Link Direction, Frequency Range, Channel Raster, Component Carrier Spacing and Number of Component Carriers.
//7. Configure Component Carriers.
//8. Configure Subcarrier Spacing for all Component Carrier.
//9. Select TXP measurement and enable Traces. 
//10. Configure Measurement Offset & Measurement Length Parameters and Averaging Parameters for TXP measurement.
//11. Initiate the Measurement.
//12. Fetch TXP Traces and Measurements.
//13. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRTxpContiguousMultiCarrier
{
   public class RFmxNRTxpContiguousMultiCarrier
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

      string digitalEdgeSource;
      RFmxNRMXDigitalEdgeTriggerEdge digitalEdge;
      bool digitalEdgeEnabled;
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

      double measurementOffset;
      double measurementLength;

      RFmxNRMXTxpAveragingEnabled averagingEnabled;
      int averagingCount;

      double timeout;

      string subblockString;
      string carrierString;

      double averagePowerMean;                                                /* (dBm) */
      double peakPowerMaximum;                                                /* (dBm) */

      AnalogWaveform<float> power;

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

         digitalEdgeEnabled = false;
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0;
         triggerDelay = 0.0;                                                           /* (s) */
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising;

         linkDirection = RFmxNRMXLinkDirection.Uplink;
         frequencyRange = RFmxNRMXFrequencyRange.Range1;

         componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal;
         channelRaster = 15e3;                                                         /* (Hz) */
         componentCarrierAtCenterFrequency = -1;
         subcarrierSpacing = 30e3;                                                     /* (Hz) */

         componentCarrierBandwidth[0] = 100e6;                                         /* (Hz) */
         componentCarrierBandwidth[1] = 100e6;                                         /* (Hz) */
         componentCarrierFrequency[0] = -49.98e6;                                      /* (Hz) */
         componentCarrierFrequency[1] = 50.01e6;                                       /* (Hz) */

         measurementOffset = 0.0;                                                      /* (s) */
         measurementLength = 1.0e-3;                                                   /* (s) */

         averagingEnabled = RFmxNRMXTxpAveragingEnabled.False;
         averagingCount = 10;

         timeout = 10.0;                                                               /* (s) */
      }

      void InitializeInstr()
      {
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      void ConfigureNR()
      {
         NR = instrSession.GetNRSignalConfiguration();      /* Create a new RFmx Session */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         NR.SetSelectedPorts("", selectedPorts);
         NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge,
            triggerDelay, digitalEdgeEnabled);
         NR.SetLinkDirection("", linkDirection);
         NR.SetFrequencyRange("", frequencyRange);
         NR.SetChannelRaster("", channelRaster);
         NR.SetComponentCarrierSpacingType("", componentCarrierSpacingType);
         NR.SetComponentCarrierAtCenterFrequency("", componentCarrierAtCenterFrequency);
         NR.ComponentCarrier.SetNumberOfComponentCarriers("", NumberOfComponentCarriers);

         subblockString = RFmxNRMX.BuildSubblockString("", 0);
         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i);
            NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth[i]);
            NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency[i]);
         }

         carrierString = "carrier::all";
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing);

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Txp, true);

         NR.Txp.Configuration.SetMeasurementInterval("", measurementLength);
         NR.Txp.Configuration.SetMeasurementOffset("", measurementOffset);
         NR.Txp.Configuration.SetAveragingEnabled("", averagingEnabled);
         NR.Txp.Configuration.SetAveragingCount("", averagingCount);

         NR.Initiate("", "");
      }

      void RetrieveResults()
      {
         NR.Txp.Results.FetchMeasurement("", timeout, out averagePowerMean, out peakPowerMaximum);
         NR.Txp.Results.FetchPowerTrace("", timeout, ref power);
      }

      void PrintResults()
      {
         Console.WriteLine("\n-------------Measurement-------------\n");
         Console.WriteLine("Average Power Mean (dBm)      : {0}", averagePowerMean);
         Console.WriteLine("Peak Power Maximum (dBm)      : {0}\n", peakPowerMaximum);
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
