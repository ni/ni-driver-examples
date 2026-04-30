//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Link Direction as Downlink, Frequency Range, Carrier Bandwidth and BWP Subcarrier Spacing.
//7. Configure DL Test Model and DL Test Model Duplex Scheme.
//8. Select ModAcc measurement and enable Traces.
//9. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
//10. Configure Measurement Interval.
//11. Initiate the Measurement.
//12. Fetch ModAcc Measurements and Traces.
//13. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;

namespace NationalInstruments.Examples.RFmxNRDLModAccSingleCarrier
{
   public class RFmxNRDLModAccSingleCarrier
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
      double carrierBandwidth;
      double subcarrierSpacing;

      RFmxNRMXDownlinkTestModelDuplexScheme downlinkTestModelDuplexScheme;
      RFmxNRMXDownlinkTestModel downlinkTestModel;

      RFmxNRMXModAccSynchronizationMode synchronizationMode;

      RFmxNRMXModAccAveragingEnabled averagingEnabled;
      int averagingCount;

      RFmxNRMXModAccMeasurementLengthUnit measurementLengthUnit;
      double measurementOffset;
      double measurementLength;

      double timeout;

      double compositeRmsEvmMean;                                                /* (%) */
      double compositePeakEvmMaximum;                                            /* (%) */
      int compositePeakEvmSlotIndex;
      int compositePeakEvmSymbolIndex;
      int compositePeakEvmSubcarrierIndex;

      double pdschRmsEvmMean;                                                    /* (%) */

      double componentCarrierFrequencyErrorMean;                                 /* (Hz) */
      double componentCarrierIQOriginOffsetMean;                                 /* (dBc) */
      double componentCarrierIQGainImbalanceMean;                                /* (dB) */
      double componentCarrierQuadratureErrorMean;                                /* (deg) */

      ComplexSingle[] pdschConstellation;

      AnalogWaveform<float> rmsEvmPerSubcarrierMean;
      AnalogWaveform<float> rmsEvmPerSymbolMean;

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

         frequencyRange = RFmxNRMXFrequencyRange.Range1;
         carrierBandwidth = 100e6;                                               /* (Hz) */
         subcarrierSpacing = 30e3;                                               /* (Hz) */

         downlinkTestModel = RFmxNRMXDownlinkTestModel.TM1_1;
         downlinkTestModelDuplexScheme = RFmxNRMXDownlinkTestModelDuplexScheme.Fdd;

         synchronizationMode = RFmxNRMXModAccSynchronizationMode.Slot;

         averagingEnabled = RFmxNRMXModAccAveragingEnabled.False;
         averagingCount = 10;

         measurementLengthUnit = RFmxNRMXModAccMeasurementLengthUnit.Slot;
         measurementOffset = 0.0;
         measurementLength = 1;

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

         NR.SetLinkDirection("", RFmxNRMXLinkDirection.Downlink);
         NR.SetFrequencyRange("", frequencyRange);
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth);
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing);

         NR.ComponentCarrier.SetDownlinkTestModel("", downlinkTestModel);
         NR.ComponentCarrier.SetDownlinkTestModelDuplexScheme("", downlinkTestModelDuplexScheme);

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
         NR.ModAcc.Results.GetCompositeRmsEvmMean("", out compositeRmsEvmMean);
         NR.ModAcc.Results.GetCompositePeakEvmMaximum("", out compositePeakEvmMaximum);
         NR.ModAcc.Results.GetCompositePeakEvmSlotIndex("", out compositePeakEvmSlotIndex);
         NR.ModAcc.Results.GetCompositePeakEvmSymbolIndex("", out compositePeakEvmSymbolIndex);
         NR.ModAcc.Results.GetCompositePeakEvmSubcarrierIndex("", out compositePeakEvmSubcarrierIndex);

         NR.ModAcc.Results.GetComponentCarrierFrequencyErrorMean("", out componentCarrierFrequencyErrorMean);
         NR.ModAcc.Results.GetComponentCarrierIQOriginOffsetMean("", out componentCarrierIQOriginOffsetMean);
         NR.ModAcc.Results.GetComponentCarrierIQGainImbalanceMean("", out componentCarrierIQGainImbalanceMean);
         NR.ModAcc.Results.GetComponentCarrierQuadratureErrorMean("", out componentCarrierQuadratureErrorMean);

         switch(downlinkTestModel)
         {
            case RFmxNRMXDownlinkTestModel.TM1_1:
            case RFmxNRMXDownlinkTestModel.TM1_2:
            case RFmxNRMXDownlinkTestModel.TM3_3:
               NR.ModAcc.Results.GetPdschQpskRmsEvmMean("", out pdschRmsEvmMean);
               NR.ModAcc.Results.FetchPdschQpskConstellationTrace("", timeout, ref pdschConstellation);
               break;

            case RFmxNRMXDownlinkTestModel.TM2:
            case RFmxNRMXDownlinkTestModel.TM3_1:
               NR.ModAcc.Results.GetPdsch64QamRmsEvmMean("", out pdschRmsEvmMean);
               NR.ModAcc.Results.FetchPdsch64QamConstellationTrace("", timeout, ref pdschConstellation);
               break;

            case RFmxNRMXDownlinkTestModel.TM2a:
            case RFmxNRMXDownlinkTestModel.TM3_1a:
               NR.ModAcc.Results.GetPdsch256QamRmsEvmMean("", out pdschRmsEvmMean);
               NR.ModAcc.Results.FetchPdsch256QamConstellationTrace("", timeout, ref pdschConstellation);
               break;

            case RFmxNRMXDownlinkTestModel.TM3_2:
               NR.ModAcc.Results.GetPdsch16QamRmsEvmMean("", out pdschRmsEvmMean);
               NR.ModAcc.Results.FetchPdsch16QamConstellationTrace("", timeout, ref pdschConstellation);
               break;
         }

         NR.ModAcc.Results.FetchRmsEvmPerSubcarrierMeanTrace("", timeout, ref rmsEvmPerSubcarrierMean);
         NR.ModAcc.Results.FetchRmsEvmPerSymbolMeanTrace("", timeout, ref rmsEvmPerSymbolMean);
      }

      void PrintResults()
      {
         Console.WriteLine("------------------Measurement------------------\n");
         Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean);
         Console.WriteLine("Composite Peak EVM Maximum (%)                 : {0}", compositePeakEvmMaximum);
         Console.WriteLine("Composite Peak EVM Slot Index                  : {0}", compositePeakEvmSlotIndex);
         Console.WriteLine("Composite Peak EVM Symbol Index                : {0}", compositePeakEvmSymbolIndex);
         Console.WriteLine("Composite Peak EVM Subcarrier Index            : {0}", compositePeakEvmSubcarrierIndex);
         Console.WriteLine("PDSCH RMS EVM Mean (%)                         : {0}", pdschRmsEvmMean);
         Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean);
         Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}", componentCarrierIQOriginOffsetMean);
         Console.WriteLine("Component Carrier IQ Gain Imbalance Mean (dB)  : {0}", componentCarrierIQGainImbalanceMean);
         Console.WriteLine("Component Carrier Quadrature Error Mean (deg)  : {0}\n", componentCarrierQuadratureErrorMean);
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
