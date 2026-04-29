//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, RF Attenuation and External Attenuation).
//5. Configure Trigger Parameters for Digital Edge Trigger.
//6. Configure Link Direction and Number of Subblocks.
//7. Configure Frequency Range, Center Frequency, Subblock Frequency Definition, Component Carrier Spacing Type,
//   Channel Raster, Component Carrier Center Frequency and Number of Component Carriers.
//8. Configure Subcarrier Spacing.
//9. Configure Component Carriers.
//10. Configure Reference Level.
//11. Select ACP measurement and enable Traces.
//12. Configure Measurement Method.
//13. Configure Noise Compensation Parameter.
//14. Configure Sweep Time Parameters.
//15. Configure Averaging Parameters for ACP measurement.
//16. Initiate the Measurement.
//17. Fetch ACP Measurements and Traces.
//18. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRAcpNonContiguousMultiCarrier
{
   public class RFmxNRAcpNonContiguousMultiCarrier
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

      const int NumberOfSubblocks = 2;
      const int NumberOfComponentCarriers = 2;

      double[] subblockFrequency = new double[NumberOfSubblocks];
      RFmxNRMXComponentCarrierSpacingType[] componentCarrierSpacingType =
         new RFmxNRMXComponentCarrierSpacingType[NumberOfSubblocks];
      double[] channelRaster = new double[NumberOfSubblocks];
      int[] componentCarrierAtCenterFrequency = new int[NumberOfSubblocks];

      double[,] componentCarrierBandwidth = new double[NumberOfSubblocks, NumberOfComponentCarriers];
      double[,] componentCarrierFrequency = new double[NumberOfSubblocks, NumberOfComponentCarriers];

      double subcarrierSpacing;
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

      double totalAggregatedPower;                      /* (dBm or dBm/Hz) */

      /* Subblock measurement outputs structure */
      struct SubblockMeasurementOutput
      {
         public double subblockPower;                          /* (dBm or dBm/Hz) */
         public double integrationBandwidth;                   /* (Hz) */
         public double frequency;                              /* (Hz) */
         public double[] lowerRelativePower;                   /* (dB) */
         public double[] upperRelativePower;                   /* (dB) */
         public double[] lowerAbsolutePower;                   /* (dBm) */
         public double[] upperAbsolutePower;                   /* (dBm) */
      };

      SubblockMeasurementOutput[] subblockOutput = new SubblockMeasurementOutput[NumberOfSubblocks];

      Spectrum<float> spectrum;
      Spectrum<float>[] relativePowersTrace = new Spectrum<float>[NumberOfSubblocks];

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

      private void InitializeVariables()
      {
         resourceName = "RFSA";

         selectedPorts = "";
         centerFrequency = 3.5e9;                                                /* (Hz) */
         externalAttenuation = 0.0;                                                    /* (dB) */

         autoLevel = true;
         referenceLevel = 0.0;                                                         /* (dBm) */
         measurementInterval = 10.0e-3;                                                /* (s) */

         rfAttenuationAuto = RFmxInstrMXRFAttenuationAuto.True;
         rfAttenuation = 10.0;                                                         /* (dB) */

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10.0e6;                                         /* (Hz) */

         enableTrigger = false;
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0;
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                                           /* (s) */

         linkDirection = RFmxNRMXLinkDirection.Uplink;
         frequencyRange = RFmxNRMXFrequencyRange.Range1;

         subblockFrequency[0] = 0.0;                                                   /* (Hz) */
         subblockFrequency[1] = 200e6;                                                 /* (Hz) */
         componentCarrierSpacingType[0] = RFmxNRMXComponentCarrierSpacingType.Nominal;
         componentCarrierSpacingType[1] = RFmxNRMXComponentCarrierSpacingType.Nominal;
         channelRaster[0] = 15e3;                                                      /* (Hz) */
         channelRaster[1] = 15e3;                                                      /* (Hz) */
         componentCarrierAtCenterFrequency[0] = -1;
         componentCarrierAtCenterFrequency[1] = -1;

         componentCarrierBandwidth[0, 0] = 100e6;                                      /* (Hz) */
         componentCarrierBandwidth[0, 1] = 100e6;                                      /* (Hz) */
         componentCarrierBandwidth[1, 0] = 100e6;                                      /* (Hz) */
         componentCarrierBandwidth[1, 1] = 100e6;                                      /* (Hz) */
         componentCarrierFrequency[0, 0] = -49.98e6;                                   /* (Hz) */
         componentCarrierFrequency[0, 1] = 50.01e6;                                    /* (Hz) */
         componentCarrierFrequency[1, 0] = -49.98e6;                                   /* (Hz) */
         componentCarrierFrequency[1, 1] = 50.01e6;                                    /* (Hz) */

         subcarrierSpacing = 30e3;                                                     /* (Hz) */
         measurementMethod = RFmxNRMXAcpMeasurementMethod.Normal;
         noiseCompensationEnabled = RFmxNRMXAcpNoiseCompensationEnabled.False;

         sweepTimeAuto = RFmxNRMXAcpSweepTimeAuto.True;
         sweepTimeInterval = 1.0e-3;                                                   /* (s) */

         averagingEnabled = RFmxNRMXAcpAveragingEnabled.False;
         averagingCount = 10;
         averagingType = RFmxNRMXAcpAveragingType.Rms;

         timeout = 10.0;                                                               /* (s) */
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
         NR.SetNumberOfSubblocks("", NumberOfSubblocks);

         for (int i = 0; i < NumberOfSubblocks; i++)
         {
            subblockString = RFmxNRMX.BuildSubblockString("", i);
            NR.SetFrequencyRange(subblockString, frequencyRange);
            NR.SetSubblockFrequency(subblockString, subblockFrequency[i]);
            NR.SetChannelRaster(subblockString, channelRaster[i]);
            NR.SetComponentCarrierSpacingType(subblockString, componentCarrierSpacingType[i]);
            NR.SetComponentCarrierAtCenterFrequency(subblockString, componentCarrierAtCenterFrequency[i]);
            NR.ComponentCarrier.SetNumberOfComponentCarriers(subblockString, NumberOfComponentCarriers);

            carrierString = RFmxNRMX.BuildCarrierString(subblockString, -1);
            NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing);

            for (int j = 0; j < NumberOfComponentCarriers; j++)
            {
               carrierString = RFmxNRMX.BuildCarrierString(subblockString, j);
               NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth[i, j]);
               NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency[i, j]);
            }
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
         for (int i = 0; i < NumberOfSubblocks; i++)
         {
            subblockString = RFmxNRMX.BuildSubblockString("", i);

            NR.Acp.Results.FetchSubblockMeasurement(subblockString, timeout, out subblockOutput[i].subblockPower,
               out subblockOutput[i].integrationBandwidth, out subblockOutput[i].frequency);

            NR.Acp.Results.FetchOffsetMeasurementArray(subblockString, timeout,
               ref subblockOutput[i].lowerRelativePower, ref subblockOutput[i].upperRelativePower,
               ref subblockOutput[i].lowerAbsolutePower, ref subblockOutput[i].upperAbsolutePower);
         }

         NR.Acp.Results.FetchTotalAggregatedPower("", timeout, out totalAggregatedPower);

         for (int i = 0; i < NumberOfSubblocks; i++)
         {
            NR.Acp.Results.FetchRelativePowersTrace("", timeout, i, ref relativePowersTrace[i]);
         }

         NR.Acp.Results.FetchSpectrum("", timeout, ref spectrum);

      }

      private void PrintResults()
      {
         Console.WriteLine("Total Aggregated Power (dBm or dBm/Hz)    : {0}", totalAggregatedPower);
         Console.WriteLine("\n-----------Subblock Measurements------------\n");
         for (int i = 0; i < NumberOfSubblocks; i++)
         {
            Console.WriteLine("Subblock  : {0}", i);
            Console.WriteLine("Subblock Power (dBm or dBm/Hz)     : {0}", subblockOutput[i].subblockPower);
            Console.WriteLine("Integration Bandwidth (Hz)         : {0}", subblockOutput[i].integrationBandwidth);
            Console.WriteLine("Frequency (Hz)                     : {0}", subblockOutput[i].frequency);
            Console.WriteLine("\n-----------Offset Channel Measurements------------\n");
            for (int j = 0; j < subblockOutput[i].lowerRelativePower.Length; j++)
            {
               Console.WriteLine("Offset  : {0}", j);
               Console.WriteLine("Lower Relative Power (dB)       : {0}", subblockOutput[i].lowerRelativePower[j]);
               Console.WriteLine("Upper Relative Power (dB)       : {0}", subblockOutput[i].upperRelativePower[j]);
               Console.WriteLine("Lower Absolute Power (dBm)      : {0}", subblockOutput[i].lowerAbsolutePower[j]);
               Console.WriteLine("Upper Absolute Power (dBm)      : {0}", subblockOutput[i].upperAbsolutePower[j]);
               Console.WriteLine("---------------------------------------------------\n");
            }
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
