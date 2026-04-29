//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Parameters for Digital Edge Trigger.
//6. Configure Link Direction, Frequency Range, Carrier Bandwidth and Subcarrier Spacing.
//7. Select TXP measurement and enable Traces.
//8. Configure Measurement Offset & Measurement Length Parameters and Averaging Parameters for TXP measurement.
//9. Initiate the Measurement.
//10. Fetch TXP Measurements and Traces.
//11. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRTxpSingleCarrier
{
   public class RFmxNRTxpSingleCarrier
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
      double carrierBandwidth;
      double subcarrierSpacing;
        
      double measurementOffset;
      double measurementLength;

      RFmxNRMXTxpAveragingEnabled averagingEnabled;
      int averagingCount;

      double timeout;

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
         centerFrequency = 3.5e9;                                          /* (Hz) */
         referenceLevel = 0.0;                                             /* (dBm) */
         externalAttenuation = 0.0;                                        /* (dB) */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e6;                             /* (Hz) */

         digitalEdgeEnabled = false;
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0;
         triggerDelay = 0.0;                                               /* (s) */
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising;

         linkDirection = RFmxNRMXLinkDirection.Uplink;
         frequencyRange = RFmxNRMXFrequencyRange.Range1;
         carrierBandwidth = 100e6;                                         /* (Hz) */
         subcarrierSpacing = 30e3;                                         /* (Hz) */

         measurementOffset = 0.0;                                          /* (s) */
         measurementLength = 1.0e-3;                                       /* (s) */

         averagingEnabled = RFmxNRMXTxpAveragingEnabled.True;
         averagingCount = 10;

         timeout = 10.0;                                                   /* (s) */
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
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth);
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing);
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
         Console.WriteLine("\n-------------Measurement------------\n");
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
