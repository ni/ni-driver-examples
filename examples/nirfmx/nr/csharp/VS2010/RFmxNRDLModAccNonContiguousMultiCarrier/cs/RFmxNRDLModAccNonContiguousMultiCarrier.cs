//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal pproperties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Link Direction as Downlink and Number of Subblocks.
//7. Configure Sublocks and Carriers .
//8. Configure BWP Subcarrier Spacing, DL Test Model Duplex Scheme and DL Test Model for all carriers in each subblock.
//9. Select ModAcc measurement and enable Traces.
//10. Configure Synchronization Mode for ModAcc measurement.
//11. Configure Measurement Interval.
//12. Initiate the Measurement.
//13. Fetch ModAcc Measurements and Traces.
//14. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRDLModAccNonContiguousMultiCarrier
{
   public class RFmxNRDLModAccNonContiguousMultiCarrier
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

      const int NumberOfSubblocks = 2;
      const int NumberOfComponentCarriers = 2;

      RFmxNRMXFrequencyRange frequencyRange;
        
      double[] subblockFrequency = new double[NumberOfSubblocks];
      RFmxNRMXComponentCarrierSpacingType[] componentCarrierSpacingType = new RFmxNRMXComponentCarrierSpacingType[NumberOfSubblocks];
      double[] channelRaster = new double[NumberOfSubblocks];
      int[] componentCarrierAtCenterFrequency = new int[NumberOfSubblocks];

      double[,] componentCarrierBandwidth = new double[NumberOfSubblocks, NumberOfComponentCarriers];
      double[,] componentCarrierFrequency = new double[NumberOfSubblocks, NumberOfComponentCarriers];

      double subcarrierSpacing;

      RFmxNRMXDownlinkTestModel downlinkTestModel;

      RFmxNRMXDownlinkTestModelDuplexScheme downlinkTestModelDuplexScheme;

      RFmxNRMXModAccSynchronizationMode synchronizationMode;

      RFmxNRMXModAccMeasurementLengthUnit measurementLengthUnit;
      double measurementOffset;
      double measurementLength;

      string subblockString;
      string carrierString;

      double timeout;

      double[,] compositeRmsEvmMean = new double[NumberOfSubblocks, NumberOfComponentCarriers];                   /* (%) */
      double[,] compositePeakEvmMaximum = new double[NumberOfSubblocks, NumberOfComponentCarriers];               /* (%) */
      int[,] compositePeakEvmSlotIndex = new int[NumberOfSubblocks, NumberOfComponentCarriers];
      int[,] compositePeakEvmSymbolIndex = new int[NumberOfSubblocks, NumberOfComponentCarriers];
      int[,] compositePeakEvmSubcarrierIndex = new int[NumberOfSubblocks, NumberOfComponentCarriers];

      double[,] pdschRmsEvmMean = new double[NumberOfSubblocks, NumberOfComponentCarriers];                       /* (%) */

      double[,] componentCarrierFrequencyErrorMean = new double[NumberOfSubblocks, NumberOfComponentCarriers];    /* (Hz) */
      double[,] componentCarrierIQOriginOffsetMean = new double[NumberOfSubblocks, NumberOfComponentCarriers];    /* (dBc) */
      double[,] componentCarrierIQGainImbalanceMean = new double[NumberOfSubblocks, NumberOfComponentCarriers];   /* (dB) */
      double[,] componentCarrierQuadratureErrorMean = new double[NumberOfSubblocks, NumberOfComponentCarriers];   /* (deg) */

      AnalogWaveform<float>[,] rmsEvmPerSubcarrierMean = new AnalogWaveform<float>[NumberOfSubblocks, NumberOfComponentCarriers];
      AnalogWaveform<float>[,] rmsEvmPerSymbolMean = new AnalogWaveform<float>[NumberOfSubblocks, NumberOfComponentCarriers];

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

         downlinkTestModel = RFmxNRMXDownlinkTestModel.TM1_1;

         downlinkTestModelDuplexScheme = RFmxNRMXDownlinkTestModelDuplexScheme.Fdd;

         synchronizationMode = RFmxNRMXModAccSynchronizationMode.Slot;

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

            for (int j = 0; j < NumberOfComponentCarriers; j++)
            {
               carrierString = RFmxNRMX.BuildCarrierString(subblockString, j);
               NR.ComponentCarrier.SetBandwidth(carrierString, componentCarrierBandwidth[i, j]);
               NR.ComponentCarrier.SetFrequency(carrierString, componentCarrierFrequency[i, j]);
            }

            carrierString = RFmxNRMX.BuildCarrierString(subblockString, -1);
            NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing(carrierString, subcarrierSpacing);
            NR.ComponentCarrier.SetDownlinkTestModelDuplexScheme(carrierString, downlinkTestModelDuplexScheme);
            NR.ComponentCarrier.SetDownlinkTestModel(carrierString, downlinkTestModel);
         }

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc, true);

         NR.ModAcc.Configuration.SetSynchronizationMode("", synchronizationMode);
         NR.ModAcc.Configuration.SetMeasurementLengthUnit("", measurementLengthUnit);
         NR.ModAcc.Configuration.SetMeasurementOffset("", measurementOffset);
         NR.ModAcc.Configuration.SetMeasurementLength("", measurementLength);
         NR.Initiate("", "");
      }

      void RetrieveResults()
      {
         for (int i = 0; i < NumberOfSubblocks; i++)
         {
            subblockString = RFmxNRMX.BuildSubblockString("", i);

            for (int j = 0; j < NumberOfComponentCarriers; j++)
            {
               carrierString = RFmxNRMX.BuildCarrierString(subblockString, j);
               NR.ModAcc.Results.GetCompositeRmsEvmMean(carrierString, out compositeRmsEvmMean[i, j]);
               NR.ModAcc.Results.GetCompositePeakEvmMaximum(carrierString, out compositePeakEvmMaximum[i, j]);
               NR.ModAcc.Results.GetCompositePeakEvmSlotIndex(carrierString, out compositePeakEvmSlotIndex[i, j]);
               NR.ModAcc.Results.GetCompositePeakEvmSymbolIndex(carrierString, out compositePeakEvmSymbolIndex[i, j]);
               NR.ModAcc.Results.GetCompositePeakEvmSubcarrierIndex(carrierString, out compositePeakEvmSubcarrierIndex[i, j]);
               NR.ModAcc.Results.GetComponentCarrierFrequencyErrorMean(carrierString, out componentCarrierFrequencyErrorMean[i, j]);
               NR.ModAcc.Results.GetComponentCarrierIQOriginOffsetMean(carrierString, out componentCarrierIQOriginOffsetMean[i, j]);
               NR.ModAcc.Results.GetComponentCarrierIQGainImbalanceMean(carrierString, out componentCarrierIQGainImbalanceMean[i, j]);
               NR.ModAcc.Results.GetComponentCarrierQuadratureErrorMean(carrierString, out componentCarrierQuadratureErrorMean[i, j]);

               switch (downlinkTestModel)
               {
                  case RFmxNRMXDownlinkTestModel.TM1_1:
                  case RFmxNRMXDownlinkTestModel.TM1_2:
                  case RFmxNRMXDownlinkTestModel.TM3_3:
                     NR.ModAcc.Results.GetPdschQpskRmsEvmMean(carrierString, out pdschRmsEvmMean[i, j]);
                     break;

                  case RFmxNRMXDownlinkTestModel.TM2:
                  case RFmxNRMXDownlinkTestModel.TM3_1:
                     NR.ModAcc.Results.GetPdsch64QamRmsEvmMean(carrierString, out pdschRmsEvmMean[i, j]);
                     break;

                  case RFmxNRMXDownlinkTestModel.TM2a:
                  case RFmxNRMXDownlinkTestModel.TM3_1a:
                     NR.ModAcc.Results.GetPdsch256QamRmsEvmMean(carrierString, out pdschRmsEvmMean[i, j]);
                     break;

                  case RFmxNRMXDownlinkTestModel.TM3_2:
                     NR.ModAcc.Results.GetPdsch16QamRmsEvmMean(carrierString, out pdschRmsEvmMean[i, j]);
                     break;
               }

               NR.ModAcc.Results.FetchRmsEvmPerSubcarrierMeanTrace(carrierString, timeout, ref rmsEvmPerSubcarrierMean[i, j]);
               NR.ModAcc.Results.FetchRmsEvmPerSymbolMeanTrace(carrierString, timeout, ref rmsEvmPerSymbolMean[i, j]);
            }
         }
      }

      void PrintResults()
      {
         Console.WriteLine("------------------------Measurement------------------------\n");
         for (int i = 0; i < NumberOfSubblocks; i++)
         {
            Console.WriteLine("Subblock  : {0}\n", i);
            for (int j = 0; j < NumberOfComponentCarriers; j++)
            {
               Console.WriteLine("Carrier  : {0}", j);
               Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean[i,j]);
               Console.WriteLine("Composite Peak EVM Maximum (%)                 : {0}", compositePeakEvmMaximum[i, j]);
               Console.WriteLine("Composite Peak EVM Slot Index                  : {0}", compositePeakEvmSlotIndex[i, j]);
               Console.WriteLine("Composite Peak EVM Symbol Index                : {0}", compositePeakEvmSymbolIndex[i, j]);
               Console.WriteLine("Composite Peak EVM Subcarrier Index            : {0}", compositePeakEvmSubcarrierIndex[i, j]);
               Console.WriteLine("PDSCH RMS EVM Mean (%)                         : {0}", pdschRmsEvmMean[i, j]);
               Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean[i, j]);
               Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}", componentCarrierIQOriginOffsetMean[i, j]);
               Console.WriteLine("Component Carrier IQ Gain Imbalance Mean (dB)  : {0}", componentCarrierIQGainImbalanceMean[i, j]);
               Console.WriteLine("Component Carrier Quadrature Error Mean (deg)  : {0}", componentCarrierQuadratureErrorMean[i, j]);
               Console.WriteLine("-----------------------------------------------------------------\n");
            }
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
