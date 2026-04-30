//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Configure NB-IoT Component Carrier.
//7. Configure NPUSCH Format.
//8. Configure Auto NPUSCH Channel Detection Enabled.
//9. Configure NPUSCH Starting Slot.
//10. Configure NPUSCH DMRS.
//11. Select ModAcc measurement and enable Traces.
//12. Configure Measurement Interval.
//13. Configure EVM Unit.
//14. Initiate the Measurement.
//15. Fetch ModAcc Measurements and Traces.
//16. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.LteMX;

namespace NationalInstruments.Examples.RFmxLteNBIoTModAcc
{
   public class RFmxLteNBIoTModAcc
   {
      RFmxInstrMX instrSession;
      RFmxLteMX lte;
      string rfsaResourceName;

      string frequencyReferenceSource;
      double frequencyReferenceFrequency;

      double centerFrequency;
      double referenceLevel;
      double externalAttenuation;

      bool enableTrigger;
      RFmxLteMXIQPowerEdgeTriggerSlope iqPowerEdgeTriggerSlope;
      double iqPowerEdgeTriggerLevel;
      RFmxLteMXTriggerMinimumQuietTimeMode minimumQuiteTimeMode;
      double minimumQuietTime;
      RFmxLteMXIQPowerEdgeTriggerLevelType iqPowerEdgeTriggerLevelType;
      double triggerDelay;

      int nCellID;
      RFmxLteMXNBIoTUplinkSubcarrierSpacing uplinkSubcarrierSpacing;
      int nPuschFormat;
      int nPuschStartingSlot;

      RFmxLteMXNPuschDmrsBaseSequenceMode baseSequenceMode;
      int baseSequenceIndex;
      int cyclicShift;
      RFmxLteMXNPuschDmrsGroupHoppingEnabled groupHoppingEnabled;
      int deltaSS;

      RFmxLteMXModAccSynchronizationMode synchronizationMode;
      int measurementOffset;
      int measurementLength;

      RFmxLteMXModAccEvmUnit evmUnit;

      double componentCarrierBandwidth;
      double componentCarrierFrequency;
      int cellID;

      RFmxLteMXAutoNPuschChannelDetectionEnabled autoNPuschChannelDetectionEnabled;

      double timeout;

      double meanRmsCompositeEvm;
      double maxPeakCompositeEvm;
      double meanFrequencyError;
      int peakCompositeEvmSlotIndex;
      int peakCompositeEvmSymbolIndex;
      int peakCompositeEvmSubcarrierIndex;
      double meanIQOriginOffset;
      double meanIQGainImbalance;
      double meanIQQuadratureError;
      double inBandEmissionMargin;
      ComplexSingle[] dataConstellation, dmrsDataConstellation;
      AnalogWaveform<float> rmsEvmPerSymbol;

      void CloseSession()
      {
         if (lte != null)
         {
            lte.Dispose();
            lte = null;
         }
         if (instrSession != null)
         {
            instrSession.Close();
            instrSession = null;
         }
      }

      public void Run()
      {
         try
         {
            InitializeVariables();
            InitializeInstr();
            ConfigureLte();
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

      void InitializeInstr()
      {
         instrSession = new RFmxInstrMX(rfsaResourceName, "");
      }

      void InitializeVariables()
      {
         rfsaResourceName = "RFSA";

         frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
         frequencyReferenceFrequency = 10e6;                                  /* (Hz) */

         centerFrequency = 1.95e9;                                            /* (Hz) */
         referenceLevel = 0.00;                                               /* (dBm) */
         externalAttenuation = 0.0;                                           /* (dB) */

         enableTrigger = true;
         iqPowerEdgeTriggerSlope = RFmxLteMXIQPowerEdgeTriggerSlope.Rising;
         iqPowerEdgeTriggerLevel = -20.0;                                     /* (dB) */
         minimumQuiteTimeMode = RFmxLteMXTriggerMinimumQuietTimeMode.Auto;
         minimumQuietTime = 100e-6;                                           /*(s) */
         iqPowerEdgeTriggerLevelType = RFmxLteMXIQPowerEdgeTriggerLevelType.Relative;
         triggerDelay = 0.0;                                                  /*(s) */

         nCellID = 0;
         uplinkSubcarrierSpacing = RFmxLteMXNBIoTUplinkSubcarrierSpacing.SubcarrierSpacing15kHz;
         nPuschFormat = 1;
         nPuschStartingSlot = 0;

         baseSequenceMode = RFmxLteMXNPuschDmrsBaseSequenceMode.Auto;
         baseSequenceIndex = 0;
         cyclicShift = 0;
         groupHoppingEnabled = RFmxLteMXNPuschDmrsGroupHoppingEnabled.False;
         deltaSS = 0;

         synchronizationMode = RFmxLteMXModAccSynchronizationMode.Slot;
         measurementOffset = 0;                                               /*(slots) */
         measurementLength = 1;                                               /*(slots) */

         evmUnit = RFmxLteMXModAccEvmUnit.Percentage;

         componentCarrierBandwidth = 200e3;                                   /* (Hz) */
         componentCarrierFrequency = 0.0;                                     /* (Hz) */
         cellID = 0;

         autoNPuschChannelDetectionEnabled = RFmxLteMXAutoNPuschChannelDetectionEnabled.True;

         timeout = 10.0;                                                      /* (s) */
      }

      void ConfigureLte()
      {
         lte = instrSession.GetLteSignalConfiguration();     /* Create a new RFmx Session */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         lte.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
         lte.ConfigureIQPowerEdgeTrigger("", "0", iqPowerEdgeTriggerSlope, iqPowerEdgeTriggerLevel, triggerDelay,
            minimumQuiteTimeMode, minimumQuietTime, iqPowerEdgeTriggerLevelType, enableTrigger);
         lte.ComponentCarrier.Configure("", componentCarrierBandwidth, componentCarrierFrequency, cellID);
         lte.ComponentCarrier.ConfigureNBIoTComponentCarrier("", nCellID, uplinkSubcarrierSpacing);
         lte.ComponentCarrier.ConfigureNPuschFormat("", nPuschFormat);
         lte.ComponentCarrier.ConfigureAutoNPuschChannelDetectionEnabled("", autoNPuschChannelDetectionEnabled);
         lte.ComponentCarrier.ConfigureNPuschStartingSlot("", nPuschStartingSlot);
         lte.ComponentCarrier.ConfigureNPuschDmrs("", baseSequenceMode, baseSequenceIndex, cyclicShift,
            groupHoppingEnabled, deltaSS);
         lte.SelectMeasurements("", RFmxLteMXMeasurementTypes.ModAcc, true);
         lte.ModAcc.Configuration.ConfigureSynchronizationModeAndInterval("", synchronizationMode,
            measurementOffset, measurementLength);
         lte.ModAcc.Configuration.ConfigureEvmUnit("", evmUnit);
         lte.Initiate("", "");
      }

      void RetrieveResults()
      {
         lte.ModAcc.Results.FetchCompositeEvm("", timeout, out meanRmsCompositeEvm, out maxPeakCompositeEvm,
            out meanFrequencyError, out peakCompositeEvmSymbolIndex, out peakCompositeEvmSubcarrierIndex,
            out peakCompositeEvmSlotIndex);
         lte.ModAcc.Results.FetchIQImpairments("", timeout, out meanIQOriginOffset, out meanIQGainImbalance,
            out meanIQQuadratureError);
         lte.ModAcc.Results.FetchInBandEmissionMargin("", timeout, out inBandEmissionMargin);
         lte.ModAcc.Results.FetchNPuschConstellationTrace("", timeout, ref dataConstellation,
            ref dmrsDataConstellation);
         lte.ModAcc.Results.FetchEvmPerSymbolTrace("", timeout, ref rmsEvmPerSymbol);
      }

      void PrintResults()
      {
         Console.WriteLine("------------------Measurements------------------");
         Console.WriteLine("Mean RMS Composite EVM  (% or dB)       : {0}", meanRmsCompositeEvm);
         Console.WriteLine("Max Peak Composite EVM  (% or dB)       : {0}", maxPeakCompositeEvm);
         Console.WriteLine("Peak Composite EVM Slot Index           : {0}", peakCompositeEvmSlotIndex);
         Console.WriteLine("Peak Composite EVM Symbol Index         : {0}", peakCompositeEvmSymbolIndex);
         Console.WriteLine("Peak Composite EVM Subcarrier Index     : {0}", peakCompositeEvmSubcarrierIndex);
         Console.WriteLine("Mean Frequency Error  (Hz)              : {0}", meanFrequencyError);
         Console.WriteLine("Mean IQ Origin Offset  (dBc)            : {0}", meanIQOriginOffset);
         Console.WriteLine("Mean IQ Gain Imbalance  (dB)            : {0}", meanIQGainImbalance);
         Console.WriteLine("Mean IQ Quadrature Error  (deg)         : {0}", meanIQQuadratureError);
         Console.WriteLine("In-Band Emission Margin  (dB)           : {0}", inBandEmissionMargin);
      }

      static void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

   }
}
