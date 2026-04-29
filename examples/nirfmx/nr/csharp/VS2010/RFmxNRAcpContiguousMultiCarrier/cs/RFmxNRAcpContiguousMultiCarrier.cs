//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, RF Attenuation and External Attenuation).
//5.Configure Trigger Parameters for Digital Edge Trigger.
//6. Configure Link Direction, Frequency Range, Channel Raster, Component Carrier Spacing and
//   Number of Component Carriers.
//7. Configure Subcarrier Spacing.
//8. Configure Component Carriers.
//9. Configure Reference Level.
//10. Select ACP measurement and enable Traces.
//11. Configure Measurement Method.
//12. Configure Noise Compensation Parameter.
//13. Configure Sweep Time Parameters.
//14. Configure Averaging Parameters for ACP measurement.
//15. Initiate the Measurement.
//16. Fetch ACP Measurements and Traces.
//17. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRAcpContiguousMultiCarrier
{
   public class RFmxNRAcpContiguousMultiCarrier
   {
      RFmxInstrMX instrSession;
      RFmxNRMX NR;
      string resourceName;

      string selectedPorts;
      double centerFrequency;
      double externalAttenuation;

      bool autoLevel;
      double referenceLevel;
      double measurementInterval;

      RFmxInstrMXRFAttenuationAuto rfAttenuationAuto;
      double rfAttenuation;

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

      RFmxNRMXAcpMeasurementMethod measurementMethod;
      RFmxNRMXAcpNoiseCompensationEnabled noiseCompensationEnabled;

      RFmxNRMXAcpSweepTimeAuto sweepTimeAuto;
      double sweepTimeInterval;

      RFmxNRMXAcpAveragingEnabled averagingEnabled;
      int averagingCount;
      RFmxNRMXAcpAveragingType averagingType;

      string subblockString;
      string carrierString;

      double timeout;

      double totalAggregatedPower;                                               /* (dBm) */

      double[] lowerRelativePower;                                               /* (dB) */
      double[] upperRelativePower;                                               /* (dB) */
      double[] lowerAbsolutePower;                                               /* (dBm) */
      double[] upperAbsolutePower;                                               /* (dBm) */

      Spectrum<float> spectrum;
      Spectrum<float> relativePowersTrace;

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

      private void InitializeVariables()
      {
         resourceName = "RFSA";

         selectedPorts = "";
         centerFrequency = 3.5e9;                                                /* (Hz) */
         externalAttenuation = 0.0;                                              /* (dB) */

         autoLevel = true;
         referenceLevel = 0.0;                                                   /* (dBm) */
         measurementInterval = 10.0e-3;                                          /* (s) */

         rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.True;
         rfAttenuation = 10.0;                                                   /* (dB) */

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

         measurementMethod = RFmxNRMXAcpMeasurementMethod.Normal;
         noiseCompensationEnabled = RFmxNRMXAcpNoiseCompensationEnabled.False;

         sweepTimeAuto = RFmxNRMXAcpSweepTimeAuto.True;
         sweepTimeInterval = 1.0e-3;                                             /* (s) */

         averagingEnabled = RFmxNRMXAcpAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxNRMXAcpAveragingType.Rms;

         timeout = 10.0;                                                         /* (s) */
      }

      private void InitializeInstr()
      {
         /* Create a new RFmx Session */
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      private void ConfigureNR()
      {
         NR = instrSession.GetNRSignalConfiguration();
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         NR.SetSelectedPorts("", selectedPorts);
         NR.ConfigureFrequency("", centerFrequency);
         NR.ConfigureExternalAttenuation("", externalAttenuation);
         instrSession.ConfigureRFAttenuation("", rfAttenuationAuto, rfAttenuation);
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

         if (autoLevel)
         {
            NR.AutoLevel("", measurementInterval, out referenceLevel);
            Console.WriteLine("Reference level (dBm)           : {0}\n", referenceLevel);
         }
         else
         {
            NR.ConfigureReferenceLevel("", referenceLevel);
         }

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.Acp, true);
         NR.Acp.Configuration.ConfigureMeasurementMethod("", measurementMethod);
         NR.Acp.Configuration.ConfigureNoiseCompensationEnabled("", noiseCompensationEnabled);
         NR.Acp.Configuration.ConfigureSweepTime("", sweepTimeAuto, sweepTimeInterval);
         NR.Acp.Configuration.ConfigureAveraging("", averagingEnabled, averagingCount, averagingType);
         NR.Initiate("", "");
      }

      private void RetrieveResults()
      {
         NR.Acp.Results.FetchOffsetMeasurementArray("", timeout, ref lowerRelativePower, ref upperRelativePower,
            ref lowerAbsolutePower, ref upperAbsolutePower);

         NR.Acp.Results.FetchTotalAggregatedPower("", timeout, out totalAggregatedPower);

         for (int i = 0; i < lowerRelativePower.Length; i++)
         {
            NR.Acp.Results.FetchRelativePowersTrace("", timeout, i, ref relativePowersTrace);
         }

         NR.Acp.Results.FetchSpectrum("", timeout, ref spectrum);
      }

      private void PrintResults()
      {
         Console.WriteLine("Total Aggregated Power (dBm or dBm/Hz)    : {0}", totalAggregatedPower);
         Console.WriteLine("\n-----------Offset Channel Measurements------------\n");
         for (int i = 0; i < lowerRelativePower.Length; i++)
         {
            Console.WriteLine("Offset  : {0}", i);
            Console.WriteLine("Lower Relative Power (dB)                 : {0}", lowerRelativePower[i]);
            Console.WriteLine("Upper Relative Power (dB)                 : {0}", upperRelativePower[i]);
            Console.WriteLine("Lower Absolute Power (dBm or dBm/Hz)      : {0}", lowerAbsolutePower[i]);
            Console.WriteLine("Upper Absolute Power (dBm or dBm/Hz)      : {0}", upperAbsolutePower[i]);
            Console.WriteLine("---------------------------------------------------\n");
         }
      }

      private void CloseSession()
      {
         try
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
         catch (Exception ex)
         {
            DisplayError(ex);
         }
      }

      static private void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

   }
}
