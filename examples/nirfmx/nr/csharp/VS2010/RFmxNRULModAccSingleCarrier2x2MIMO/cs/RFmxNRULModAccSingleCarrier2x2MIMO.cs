//Steps:
// 1. Open NI-RFSG sessions. 
// 2. Configure Reference Clock Source, Frequency, Power Level, Power Level Type and External Gain. 
// 3.  Export marker0 event to the specified output terminal  
// 4.  Read the waveforms from the tdms file and write it to RFSG memory.
// 5. Set waveform generation mode to Script.
// 6. Set the Script to be used for generation. 
// 7. Retrieve the session reference. This is used by the NI-TClk VIs.
// 8. Configure the devices for homogeneous triggers
// 9. Synchronize the generators
// 10. Initiate generation
// 11. Open a new RFmx Session.
// 12. Configure Frequency Reference.
// 13. Configure Number of Receive Chains and Center Frequency.
// 14. Configure Selected Ports and Signal Analyser properties ( Reference Level and External Attenuation).
// 15. Configure Trigger Type and Trigger Parameters.
// 16. Configure Frequency Range, CC bandwidth, Cell ID, Band, BWP Subcarrier Spacing and Auto RB Detection Enabled
// 17. Configure PUSCH and PUSCH RB Allocation.
// 18. Configure PUSCH DMRS.
// 19. Select ModAcc measurement and enable Traces.
// 20. Configure the interval used for Pre-FFT error estimation, settings for the post-FFT tracking, Synchronization Mode and Averaging Parameters for the ModAcc measurement.
// 21. Configure Measurement Interval.
// 22. Initiate the Measurement.
// 23. Fetch ModAcc Measurements and Traces.
// 24. Close RFmx Session. 
// 25. Abort signal generation.
// 26. Disable the output. This sets the noise floor as low as possible.
// 27. Call NI-RFSG Commit.
// 28. Clear the waveforms and waveform properties from the device memory. 
// 29. Close the NI-RFSG sessions.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.TimingServices;
using NationalInstruments.ModularInstruments;

namespace NationalInstruments.Examples.RFmxNRULModAccSingleCarrier2x2MIMO
{
   public class RFmxNRULModAccSingleCarrier2x2MIMO
   {
      RFmxInstrMX instrSession;
      RFmxNRMX NR;
      NIRfsg[] rfsgSessions;
      TClock tClockSession;
      ITClockSynchronizableDevice[] rfsgSynchronizableDevices;

      string[] rfsgResourceNames;
      string[] rfsaResourceNames;
      int numberOfDevices;

      string[] selectedPorts;
      const int numberOfReceiveChains = 2;

      string layerString;
      string chainString;

      double centerFrequency;
      double referenceLevel;
      double rfsaExternalAttenuation;
      double rfsgExternalAttenuation;
      double powerLevel;
      int markerNumber;

      string[] portString;

      string[] selectedPortsString;

      string frequencyReferenceSource;
      string rfsgReferenceClockSource;
      double frequencyReferenceFrequency;
      string[] PxiTriggerLines;

      string waveformName;
      string waveformFile;
      string scriptName;
      string waveformScript;
      uint waveformIndex;

      bool enableTrigger;
      string digitalEdgeSource;
      RFmxNRMXDigitalEdgeTriggerEdge digitalEdge;
      double triggerDelay;

      RFmxNRMXFrequencyRange frequencyRange;
      int band;
      int cellID;
      double carrierBandwidth;
      double subcarrierSpacing;
      RFmxNRMXAutoResourceBlockDetectionEnabled autoResourceBlockDetectionEnabled;

      RFmxNRMXPuschTransformPrecodingEnabled puschTransformPrecodingEnabled;
      RFmxNRMXPuschModulationType puschModulationType;
      const int NumberOfResourceBlockClusters = 1;
      int[] puschResourceBlockOffset = new int[NumberOfResourceBlockClusters];
      int[] puschNumberOfResourceBlocks = new int[NumberOfResourceBlockClusters];
      string puschSlotAllocation;
      string puschSymbolAllocation;

      RFmxNRMXPuschDmrsPowerMode puschDmrsPowerMode;
      double puschDmrsPower;
      RFmxNRMXPuschDmrsConfigurationType puschDmrsConfigurationType;
      RFmxNRMXPuschMappingType puschMappingType;
      int puschDmrsTypeAPosition;
      RFmxNRMXPuschDmrsDuration puschDmrsDuration;
      int puschDmrsAdditionalPositions;
      int puschDmrsNumberOfCdmGroups;
      string puschDmrsAntennaPorts;

      RFmxNRMXModAccSynchronizationMode synchronizationMode;

      RFmxNRMXModAccMeasurementLengthUnit measurementLengthUnit;
      double measurementOffset;
      double measurementLength;

      RFmxNRMXModAccAveragingEnabled averagingEnabled;
      int averagingCount;
      string subblockString;
      string carrierString;
      string bandwidthPartString;
      string userString;
      string puschString;
      string puschClusterString;

      double timeout;

      double[] compositeRmsEvmMean = new double[numberOfReceiveChains];                                                /* (%) */
      double[] compositePeakEvmMaximum = new double[numberOfReceiveChains];                                            /* (%) */
      int[] compositePeakEvmSlotIndex = new int[numberOfReceiveChains];
      int[] compositePeakEvmSymbolIndex = new int[numberOfReceiveChains];
      int[] compositePeakEvmSubcarrierIndex = new int[numberOfReceiveChains];

      double[] componentCarrierFrequencyErrorMean = new double[numberOfReceiveChains];                                 /* (Hz) */
      double[] componentCarrierIQOriginOffsetMean = new double[numberOfReceiveChains];                                 /* (dBc) */
      double[] componentCarrierTimingOffsetMean = new double[numberOfReceiveChains];                                   /* (dB) */
      double[] componentCarrierSymbolClockErrorMean = new double[numberOfReceiveChains];                               /* (deg) */
      double[] inBandEmissionMargin = new double[numberOfReceiveChains];                                               /* (dB) */

      ComplexSingle[][] puschDataConstellation = new ComplexSingle[numberOfReceiveChains][];
      ComplexSingle[][] puschDmrsConstellation = new ComplexSingle[numberOfReceiveChains][];

      AnalogWaveform<float>[] rmsEvmPerSubcarrierMean = new AnalogWaveform<float>[numberOfReceiveChains];
      AnalogWaveform<float>[] rmsEvmPerSymbolMean = new AnalogWaveform<float>[numberOfReceiveChains];

      Spectrum<float>[] spectralFlatness = new Spectrum<float>[numberOfReceiveChains];
      Spectrum<float>[] spectralFlatnessLowerMask = new Spectrum<float>[numberOfReceiveChains];
      Spectrum<float>[] spectralFlatnessUpperMask = new Spectrum<float>[numberOfReceiveChains];

      public void Run()
      {
         try
         {
            InitializeVariables();
            InitializeInstr();
            ConfigureRfsg();
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
         rfsgResourceNames = new string[] { "RFSG1", "RFSG2" };
         rfsaResourceNames = new string[] { "RFSA1", "RFSA2" };
         numberOfDevices = rfsaResourceNames.GetLength(0);
         rfsgSynchronizableDevices = new ITClockSynchronizableDevice[numberOfDevices];

         selectedPorts = new string[] { "", "" };

         centerFrequency = 3.5e9;                                                /* (Hz) */
         referenceLevel = 0.0;                                                   /* (dBm) */
         rfsaExternalAttenuation = 0.0;                                          /* (dB) */
         rfsgExternalAttenuation = 0.0;                                          /* (dB) */
         powerLevel = -10.0;                                                       /* (dBm) */
         markerNumber = 0;

         portString = new string[numberOfDevices];

         selectedPortsString = new string[numberOfDevices];

         frequencyReferenceSource = RFmxInstrMXConstants.PxiClock;
         rfsgReferenceClockSource = RfsgFrequencyReferenceSource.PxiClock;
         frequencyReferenceFrequency = 10.0e6;                                   /* (Hz) */
         PxiTriggerLines = new string[] {RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine0,
            RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine1};

         scriptName = "GenerateWfm";
         waveformName = "Wfm";
         waveformFile = @"NR_FR1_UL_MIMO_BW-100MHz_SCS-30kHz_Ports-01_SF-1ms.tdms";

         enableTrigger = true;
         digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0;
         digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising;
         triggerDelay = 0.0;                                                     /* (s) */

         frequencyRange = RFmxNRMXFrequencyRange.Range1;
         band = 78;
         cellID = 0;
         carrierBandwidth = 100e6;                                               /* (Hz) */
         subcarrierSpacing = 30e3;                                               /* (Hz) */
         autoResourceBlockDetectionEnabled = RFmxNRMXAutoResourceBlockDetectionEnabled.True;

         puschTransformPrecodingEnabled = RFmxNRMXPuschTransformPrecodingEnabled.False;
         puschModulationType = RFmxNRMXPuschModulationType.Qpsk;
         puschResourceBlockOffset[0] = 0;
         puschNumberOfResourceBlocks[0] = -1;
         puschSlotAllocation = "0-Last";
         puschSymbolAllocation = "0-Last";

         puschDmrsPowerMode = RFmxNRMXPuschDmrsPowerMode.CdmGroups;
         puschDmrsPower = 0.0;                                                   /* (dB) */
         puschDmrsConfigurationType = RFmxNRMXPuschDmrsConfigurationType.Type1;
         puschMappingType = RFmxNRMXPuschMappingType.TypeA;
         puschDmrsTypeAPosition = 2;
         puschDmrsDuration = RFmxNRMXPuschDmrsDuration.SingleSymbol;
         puschDmrsAdditionalPositions = 0;
         puschDmrsNumberOfCdmGroups = 1;
         puschDmrsAntennaPorts = "0,1";

         synchronizationMode = RFmxNRMXModAccSynchronizationMode.Slot;

         measurementLengthUnit = RFmxNRMXModAccMeasurementLengthUnit.Slot;
         measurementOffset = 0.0;
         measurementLength = 1;

         averagingEnabled = RFmxNRMXModAccAveragingEnabled.False;
         averagingCount = 10;

         timeout = 10.0;                                                         /* (s) */
      }

      void InitializeInstr()
      {
         instrSession = new RFmxInstrMX(rfsaResourceNames, "");
      }
      void ConfigureRfsg()
      {
         rfsgSessions = new NIRfsg[numberOfDevices];
         for (int i = 0; i < numberOfDevices; i++)
         {
            rfsgSessions[i] = new NIRfsg(rfsgResourceNames[i], false, true);
            rfsgSynchronizableDevices[i] = (ITClockSynchronizableDevice)rfsgSessions[i];
            rfsgSessions[i].FrequencyReference.Configure(rfsgReferenceClockSource, frequencyReferenceFrequency);
            rfsgSessions[i].RF.Configure(centerFrequency, powerLevel);
            rfsgSessions[i].RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
            rfsgSessions[i].RF.ExternalGain = -rfsgExternalAttenuation;
            //instrumentHandles[i] = rfsgSessions[i].GetInstrumentHandle().DangerousGetHandle();
            rfsgSessions[i].DeviceEvents.MarkerEvents[markerNumber].ExportedOutputTerminal = PxiTriggerLines[i];
            waveformIndex = (uint)i;
            rfsgSessions[i].Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, waveformFile, waveformIndex);
            waveformScript = String.Format("script {0}{1}repeat forever{1}generate {2} marker{3}(0){1}end repeat{1}end script",
                  scriptName, Environment.NewLine, waveformName, markerNumber);
            rfsgSessions[i].RF.ExternalGain = -rfsgExternalAttenuation;
            rfsgSessions[i].Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
            rfsgSessions[i].Arb.Scripting.WriteScript(waveformScript);
         }
         tClockSession = new TClock(rfsgSynchronizableDevices);
         tClockSession.ConfigureForHomogeneousTriggers();
         tClockSession.Synchronize();
         tClockSession.Initiate();
      }
      void ConfigureNR()
      {
         NR = instrSession.GetNRSignalConfiguration();     /* Create a new RFmx Session */
         instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
         NR.SetNumberOfReceiveChains("", numberOfReceiveChains);
         NR.SetCenterFrequency("", centerFrequency);
         for (int i = 0; i < numberOfReceiveChains; ++i)
         {
            selectedPortsString[i] = RFmxInstrMX.BuildPortString2("", selectedPorts[i], rfsaResourceNames[i], 0);
            portString[i] = RFmxInstrMX.BuildPortString2("", "", rfsaResourceNames[i], 0);
            NR.ConfigureReferenceLevel(portString[i], referenceLevel);
            NR.ConfigureExternalAttenuation(portString[i], rfsaExternalAttenuation);
         }
         NR.ConfigureSelectedPortsMultiple("", selectedPortsString);
         NR.ConfigureDigitalEdgeTrigger("", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger);

         NR.SetFrequencyRange("", frequencyRange);
         NR.ComponentCarrier.SetBandwidth("", carrierBandwidth);
         NR.ComponentCarrier.SetCellID("", cellID);
         NR.SetBand("", band);
         NR.ComponentCarrier.SetBandwidthPartSubcarrierSpacing("", subcarrierSpacing);
         NR.SetAutoResourceBlockDetectionEnabled("", autoResourceBlockDetectionEnabled);

         NR.ComponentCarrier.SetPuschTransformPrecodingEnabled("", puschTransformPrecodingEnabled);
         NR.ComponentCarrier.SetPuschSlotAllocation("", puschSlotAllocation);
         NR.ComponentCarrier.SetPuschSymbolAllocation("", puschSymbolAllocation);
         NR.ComponentCarrier.SetPuschModulationType("", puschModulationType);

         NR.ComponentCarrier.SetPuschNumberOfResourceBlockClusters("", NumberOfResourceBlockClusters);

         subblockString = RFmxNRMX.BuildSubblockString("", 0);
         carrierString = RFmxNRMX.BuildCarrierString(subblockString, 0);
         bandwidthPartString = RFmxNRMX.BuildBandwidthPartString(carrierString, 0);
         userString = RFmxNRMX.BuildUserString(bandwidthPartString, 0);
         puschString = RFmxNRMX.BuildPuschString(userString, 0);
         for (int i = 0; i < NumberOfResourceBlockClusters; i++)
         {
            puschClusterString = RFmxNRMX.BuildPuschClusterString(puschString, i);
            NR.ComponentCarrier.SetPuschResourceBlockOffset(puschClusterString, puschResourceBlockOffset[i]);
            NR.ComponentCarrier.SetPuschNumberOfResourceBlocks(puschClusterString, puschNumberOfResourceBlocks[i]);
         }

         NR.ComponentCarrier.SetPuschDmrsPowerMode("", puschDmrsPowerMode);
         NR.ComponentCarrier.SetPuschDmrsPower("", puschDmrsPower);
         NR.ComponentCarrier.SetPuschDmrsConfigurationType("", puschDmrsConfigurationType);
         NR.ComponentCarrier.SetPuschMappingType("", puschMappingType);
         NR.ComponentCarrier.SetPuschDmrsTypeAPosition("", puschDmrsTypeAPosition);
         NR.ComponentCarrier.SetPuschDmrsDuration("", puschDmrsDuration);
         NR.ComponentCarrier.SetPuschDmrsAdditionalPositions("", puschDmrsAdditionalPositions);
         NR.ComponentCarrier.SetPuschDmrsAntennaPorts("", puschDmrsAntennaPorts);
         NR.ComponentCarrier.SetPuschDmrsNumberOfCdmGroups("", puschDmrsNumberOfCdmGroups);

         NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc, true);

         NR.ModAcc.Configuration.SetPreFftErrorEstimationInterval("", RFmxNRMXModAccPreFftErrorEstimationInterval.Slot);
         NR.ModAcc.Configuration.SetPhaseTrackingMode("", RFmxNRMXModAccPhaseTrackingMode.Disabled);
         NR.ModAcc.Configuration.SetTimingTrackingMode("", RFmxNRMXModAccTimingTrackingMode.Disabled);
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
         for (int i = 0; i < numberOfReceiveChains; i++)
         {
            layerString = RFmxNRMX.BuildLayerString("subblock0/carrier0", i);
            chainString = RFmxNRMX.BuildChainString("subblock0/carrier0", i);

            NR.ModAcc.Results.GetCompositeRmsEvmMean(layerString, out compositeRmsEvmMean[i]);
            NR.ModAcc.Results.GetCompositePeakEvmMaximum(layerString, out compositePeakEvmMaximum[i]);
            NR.ModAcc.Results.GetCompositePeakEvmSlotIndex(layerString, out compositePeakEvmSlotIndex[i]);
            NR.ModAcc.Results.GetCompositePeakEvmSymbolIndex(layerString, out compositePeakEvmSymbolIndex[i]);
            NR.ModAcc.Results.GetCompositePeakEvmSubcarrierIndex(layerString, out compositePeakEvmSubcarrierIndex[i]);
            NR.ModAcc.Results.GetComponentCarrierFrequencyErrorMean(layerString, out componentCarrierFrequencyErrorMean[i]);
            NR.ModAcc.Results.GetComponentCarrierIQOriginOffsetMean(layerString, out componentCarrierIQOriginOffsetMean[i]);
            NR.ModAcc.Results.GetComponentCarrierTimeOffsetMean(chainString, out componentCarrierTimingOffsetMean[i]);
            NR.ModAcc.Results.GetComponentCarrierSymbolClockErrorMean(chainString, out componentCarrierSymbolClockErrorMean[i]);
            NR.ModAcc.Results.GetInBandEmissionMargin(chainString, out inBandEmissionMargin[i]);
            NR.ModAcc.Results.FetchPuschDataConstellationTrace(layerString, timeout, ref puschDataConstellation[i]);
            NR.ModAcc.Results.FetchPuschDmrsConstellationTrace(layerString, timeout, ref puschDmrsConstellation[i]);
            NR.ModAcc.Results.FetchRmsEvmPerSubcarrierMeanTrace(layerString, timeout, ref rmsEvmPerSubcarrierMean[i]);
            NR.ModAcc.Results.FetchRmsEvmPerSymbolMeanTrace(layerString, timeout, ref rmsEvmPerSymbolMean[i]);
            NR.ModAcc.Results.FetchSpectralFlatnessTrace(layerString, timeout, ref spectralFlatness[i], ref spectralFlatnessLowerMask[i], ref spectralFlatnessUpperMask[i]);
         }

      }

      void PrintResults()
      {
         Console.WriteLine("------------------Measurement------------------\n");
         for (int i = 0; i < numberOfReceiveChains; i++)
         {
            Console.WriteLine("Layer  : {0}", i);
            Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean[i]);
            Console.WriteLine("Composite Peak EVM Maximum (%)                 : {0}", compositePeakEvmMaximum[i]);
            Console.WriteLine("Composite Peak EVM Slot Index                  : {0}", compositePeakEvmSlotIndex[i]);
            Console.WriteLine("Composite Peak EVM Symbol Index                : {0}", compositePeakEvmSymbolIndex[i]);
            Console.WriteLine("Composite Peak EVM Subcarrier Index            : {0}", compositePeakEvmSubcarrierIndex[i]);
            Console.WriteLine("Component Carrier Frequency Error Mean (Hz)    : {0}", componentCarrierFrequencyErrorMean[i]);
            Console.WriteLine("Component Carrier IQ Origin Offset Mean (dBc)  : {0}", componentCarrierIQOriginOffsetMean[i]);
            Console.WriteLine("Chain  : {0}", i);
            Console.WriteLine("Component Carrier Time Offset Mean (s)         : {0}", componentCarrierTimingOffsetMean[i]);
            Console.WriteLine("component Carrier Symbol Clock Error Mean (ppm): {0}", componentCarrierSymbolClockErrorMean[i]);
            Console.WriteLine("In-Band Emission Margin (dB)                   : {0}", inBandEmissionMargin[i]);
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
         if (rfsgSessions != null)
         {
            for (int i = 0; i < numberOfDevices; i++)
            {
               rfsgSessions[i].Abort();
               rfsgSessions[i].RF.OutputEnabled = false;
               rfsgSessions[i].Utility.Commit();
               rfsgSessions[i].Arb.ClearWaveform(waveformName);
               rfsgSessions[i].Close();
               rfsgSessions[i] = null;
            }
         }
         tClockSession = null;
      }

      static void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }

   }
}
