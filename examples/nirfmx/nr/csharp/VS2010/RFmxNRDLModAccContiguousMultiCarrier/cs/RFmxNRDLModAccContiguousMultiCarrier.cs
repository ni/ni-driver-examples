//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Link Direction as Downlink, Frequency Range, Channel Raster and Component Carrier Spacing.
//7. Configure Carrier.
//8. Configure BWP Subcarrier Spacing, DL Test Model Duplex Scheme and DL Test Model for all carriers.
//9. Select ModAcc measurement and enable Traces.
//10. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
//11. Configure Measurement Interval.
//12. Initiate the Measurement.
//13. Fetch ModAcc Measurements and Traces.
//14. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRDLModAccContiguousMultiCarrier
{
   public class RFmxNRDLModAccContiguousMultiCarrier
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

      RFmxNRMXFrequencyRange frequencyRange;
      double subcarrierSpacing;

      RFmxNRMXComponentCarrierSpacingType componentCarrierSpacingType;
      double channelRaster;
      int componentCarrierAtCenterFrequency;

      const int NumberOfComponentCarriers = 2;
      double[] componentCarrierBandwidth = new double[NumberOfComponentCarriers];
      double[] componentCarrierFrequency = new double[NumberOfComponentCarriers];

      RFmxNRMXDownlinkTestModel downlinkTestModel;

      RFmxNRMXDownlinkTestModelDuplexScheme downlinkTestModelDuplexScheme;

      RFmxNRMXModAccSynchronizationMode synchronizationMode;

      RFmxNRMXModAccAveragingEnabled averagingEnabled;
      int averagingCount;

      RFmxNRMXModAccMeasurementLengthUnit measurementLengthUnit;
      double measurementOffset;
      double measurementLength;

      string subblockString;
      string carrierString;

      double timeout;

      double[] compositeRmsEvmMean = new double[NumberOfComponentCarriers];                        /* (%) */
      double[] compositePeakEvmMaximum = new double[NumberOfComponentCarriers];                    /* (%) */
      int[] compositePeakEvmSlotIndex = new int[NumberOfComponentCarriers];
      int[] compositePeakEvmSymbolIndex = new int[NumberOfComponentCarriers];
      int[] compositePeakEvmSubcarrierIndex = new int[NumberOfComponentCarriers];

      double[] pdschRmsEvmMean = new double[NumberOfComponentCarriers];                            /* (%) */

      double[] componentCarrierFrequencyErrorMean = new double[NumberOfComponentCarriers];         /* (Hz) */
      double[] componentCarrierIQOriginOffsetMean = new double[NumberOfComponentCarriers];         /* (dBc) */
      double[] componentCarrierIQGainImbalanceMean = new double[NumberOfComponentCarriers];        /* (dB) */
      double[] componentCarrierQuadratureErrorMean = new double[NumberOfComponentCarriers];        /* (deg) */

      AnalogWaveform<float>[] rmsEvmPerSubcarrierMean = new AnalogWaveform<float>[NumberOfComponentCarriers];
      AnalogWaveform<float>[] rmsEvmPerSymbolMean = new AnalogWaveform<float>[NumberOfComponentCarriers];

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

         enableTrigger = false;
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0;
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                                           /* (s) */

         frequencyRange = RFmxNRMXFrequencyRange.Range1;
         subcarrierSpacing = 30e3;                                                     /* (Hz) */

         componentCarrierSpacingType = RFmxNRMXComponentCarrierSpacingType.Nominal;
         channelRaster = 15e3;                                                         /* (Hz) */
         componentCarrierAtCenterFrequency = -1;

         componentCarrierBandwidth[0] = 100e6;                                         /* (Hz) */
         componentCarrierBandwidth[1] = 100e6;                                         /* (Hz) */
         componentCarrierFrequency[0] = -49.98e6;                                      /* (Hz) */
         componentCarrierFrequency[1] = 50.01e6;                                       /* (Hz) */

         downlinkTestModel = RFmxNRMXDownlinkTestModel.TM1_1;

         downlinkTestModelDuplexScheme = RFmxNRMXDownlinkTestModelDuplexScheme.Fdd;

         synchronizationMode = RFmxNRMXModAccSynchronizationMode.Slot;

         averagingEnabled = RFmxNRMXModAccAveragingEnabled.False;
         averagingCount = 10;

         measurementLengthUnit = RFmxNRMXModAccMeasurementLengthUnit.Slot;
         measurementOffset = 0.0;
         measurementLength = 1;

         timeout = 10.0;                                                               /* (s) */
      }

      void InitializeInstr()
      {
         instrSession = new RFmxInstrMX(resourceName, "");
      }

      void ConfigureNR()
      {
         /* Create a new RFmx Session */
         NR = instrSession.GetNRSignalConfiguration();
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         NR.SetSelectedPorts("", selectedPorts);
         NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);

         NR.SetLinkDirection("", RFmxNRMXLinkDirection.Downlink);
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
         NR.ComponentCarrier.SetDownlinkTestModelDuplexScheme(carrierString, downlinkTestModelDuplexScheme);
         NR.ComponentCarrier.SetDownlinkTestModel(carrierString, downlinkTestModel);

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc, true);

         NR.ModAcc.Configuration.SetSynchronizationMode("", synchronizationMode);
         NR.ModAcc.Configuration.SetAveragingEnabled("", averagingEnabled);
         NR.ModAcc.Configuration.SetAveragingCount("", averagingCount);

         NR.ModAcc.Configuration.SetMeasurementLengthUnit("", measurementLengthUnit);
         NR.ModAcc.Configuration.SetMeasurementOffset("", measurementOffset);
         NR.ModAcc.Configuration.SetMeasurementLength("", measurementLength);

         NR.Initiate("", "");
      }

      void RetrieveResults()
      {
         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            carrierString = RFmxNRMX.BuildCarrierString(subblockString, i);

            NR.ModAcc.Results.GetCompositeRmsEvmMean(carrierString, out compositeRmsEvmMean[i]);
            NR.ModAcc.Results.GetCompositePeakEvmMaximum(carrierString, out compositePeakEvmMaximum[i]);
            NR.ModAcc.Results.GetCompositePeakEvmSlotIndex(carrierString, out compositePeakEvmSlotIndex[i]);
            NR.ModAcc.Results.GetCompositePeakEvmSymbolIndex(carrierString, out compositePeakEvmSymbolIndex[i]);
            NR.ModAcc.Results.GetCompositePeakEvmSubcarrierIndex(carrierString, out compositePeakEvmSubcarrierIndex[i]);
            NR.ModAcc.Results.GetComponentCarrierFrequencyErrorMean(carrierString,
               out componentCarrierFrequencyErrorMean[i]);
            NR.ModAcc.Results.GetComponentCarrierIQOriginOffsetMean(carrierString,
               out componentCarrierIQOriginOffsetMean[i]);
            NR.ModAcc.Results.GetComponentCarrierIQGainImbalanceMean(carrierString,
               out componentCarrierIQGainImbalanceMean[i]);
            NR.ModAcc.Results.GetComponentCarrierQuadratureErrorMean(carrierString,
               out componentCarrierQuadratureErrorMean[i]);

            switch (downlinkTestModel)
            {
               case RFmxNRMXDownlinkTestModel.TM1_1:
               case RFmxNRMXDownlinkTestModel.TM1_2:
               case RFmxNRMXDownlinkTestModel.TM3_3:
                  NR.ModAcc.Results.GetPdschQpskRmsEvmMean(carrierString, out pdschRmsEvmMean[i]);
                  break;

               case RFmxNRMXDownlinkTestModel.TM2:
               case RFmxNRMXDownlinkTestModel.TM3_1:
                  NR.ModAcc.Results.GetPdsch64QamRmsEvmMean(carrierString, out pdschRmsEvmMean[i]);
                  break;

               case RFmxNRMXDownlinkTestModel.TM2a:
               case RFmxNRMXDownlinkTestModel.TM3_1a:
                  NR.ModAcc.Results.GetPdsch256QamRmsEvmMean(carrierString, out pdschRmsEvmMean[i]);
                  break;

               case RFmxNRMXDownlinkTestModel.TM3_2:
                  NR.ModAcc.Results.GetPdsch16QamRmsEvmMean(carrierString, out pdschRmsEvmMean[i]);
                  break;
            }

            NR.ModAcc.Results.FetchRmsEvmPerSubcarrierMeanTrace(carrierString, timeout, ref rmsEvmPerSubcarrierMean[i]);
            NR.ModAcc.Results.FetchRmsEvmPerSymbolMeanTrace(carrierString, timeout, ref rmsEvmPerSymbolMean[i]);
         }
      }

      void PrintResults()
      {
         Console.WriteLine("------------------------Measurements------------------------\n");
         for (int i = 0; i < NumberOfComponentCarriers; i++)
         {
            Console.WriteLine("Carrier  : {0}", i);
            Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean[i]);
            Console.WriteLine("Composite Peak EVM Maximum (%)                 : {0}", compositePeakEvmMaximum[i]);
            Console.WriteLine("Composite Peak EVM Slot Index                  : {0}", compositePeakEvmSlotIndex[i]);
            Console.WriteLine("Composite Peak EVM Symbol Index                : {0}", compositePeakEvmSymbolIndex[i]);
            Console.WriteLine("Composite Peak EVM Subcarrier Index            : {0}", compositePeakEvmSubcarrierIndex[i]);
            Console.WriteLine("PDSCH RMS EVM Mean (%)                         : {0}", pdschRmsEvmMean[i]);
            Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean[i]);
            Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}", componentCarrierIQOriginOffsetMean[i]);
            Console.WriteLine("Component Carrier IQ Gain Imbalance Mean (dB)  : {0}", componentCarrierIQGainImbalanceMean[i]);
            Console.WriteLine("Component Carrier Quadrature Error Mean (deg)  : {0}", componentCarrierQuadratureErrorMean[i]);
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
