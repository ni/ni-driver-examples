//Steps:
//1.  Open a new RFmx Session.
//2.  Configure Frequency Reference.
//3.  Configure LO Source to Automatic SG SA Shared.
//4.  Configure Selected Ports.
//5.  Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//6.  Configure Trigger Type and Trigger Parameters.
//7.  Configure Frequency Range, CC bandwidth, Cell ID, Band, BWP Subcarrier Spacing and Auto RB Detection Enabled.
//    Setting Auto RB Detection Enabled to False reduces the measurement time.
//8.  Configure PUSCH and PUSCH RB Allocation.
//9.  Configure PUSCH DMRS.
//10. Select ModAcc measurement and disable Traces.
//11. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
//12. Configure Measurement Interval.
//13. Set ModAcc Magnitude and Phase Error Enabled and IQ Mismatch Estimation Enabled to False.
//    This disables computation of the corresponding results. Configure ModAcc Frequency Error Estimation,
//    Symbol Clock Error Estimation Enabled, Phase Tracking Mode, Timing Tracking Mode, and IQ Origin Offset Estimation Enabled.
//    Set these attributes to False/Disabled to reduce the measurement time. This disables estimation and, in turn,
//    correction of the corresponding impairments. You may disable estimation of an impairment only if it is not present
//    in the signal to be measured.
//14. Configure EVM Reference Data Sympbol Mode.
//15. Configure Reference Waveform if EVM Reference Data Symbol Mode is set as Reference Waveform.
//16. Initiate the Measurement.
//17. Fetch ModAcc Measurements.
//18. Close RFmx Session.

using System;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.NRMX;
using NationalInstruments.ModularInstruments.NIRfsgPlayback;


namespace NationalInstruments.Examples.RFmxNRULModAccSpeedOptimized
{
    public class RFmxNRULModAccSpeedOptimized
    {
        RFmxInstrMX instrSession;
        RFmxNRMX NR;
        string resourceName;

        string frequencyReferenceSource;
        double frequencyReferenceFrequency;

        string selectedPorts;
        double centerFrequency;
        double referenceLevel;
        double externalAttenuation;

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

        RFmxNRMXModAccSynchronizationMode synchronizationMode;

        RFmxNRMXModAccMeasurementLengthUnit measurementLengthUnit;
        double measurementOffset;
        double measurementLength;

        RFmxNRMXModAccAveragingEnabled averagingEnabled;
        int averagingCount;

        RFmxNRMXModAccFrequencyErrorEstimation frequencyErrorEstimation;
        RFmxNRMXModAccSymbolClockErrorEstimationEnabled symbolClockErrorEstimationEnabled;
        RFmxNRMXModAccPhaseTrackingMode phaseTrackingMode;
        RFmxNRMXModAccTimingTrackingMode timingTrackingMode;
        RFmxNRMXModAccIQOriginOffsetEstimationEnabled IQOriginOffsetEstimationEnabled;

        RFmxNRMXModAccEvmReferenceDataSymbolsMode evmReferenceDataSymbolsMode;
        string waveformFileName;

        string subblockString;
        string carrierString;
        string bandwidthPartString;
        string userString;
        string puschString;
        string puschClusterString;
        ComplexWaveform<ComplexSingle> referenceWaveformSingle;

        double compositeRmsEvmMean;                                                /* (%) */
        double inBandEmissionMargin;                                               /* (dB) */

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

            frequencyReferenceSource = RFmxInstrMXConstants.OnboardClock;
            frequencyReferenceFrequency = 10.0e6;                                   /* (Hz) */

            selectedPorts = "";
            centerFrequency = 3.5e9;                                                /* (Hz) */
            referenceLevel = 0.0;                                                   /* (dBm) */
            externalAttenuation = 0.0;                                              /* (dB) */

            enableTrigger = true;
            digitalEdgeSource = RFmxNRMXConstants.PxiTriggerLine0;
            digitalEdge = RFmxNRMXDigitalEdgeTriggerEdge.Rising;
            triggerDelay = 0.0;                                                     /* (s) */

            frequencyRange = RFmxNRMXFrequencyRange.Range1;
            band = 78;
            cellID = 0;
            carrierBandwidth = 100e6;                                               /* (Hz) */
            subcarrierSpacing = 30e3;                                               /* (Hz) */
            autoResourceBlockDetectionEnabled = RFmxNRMXAutoResourceBlockDetectionEnabled.False;

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

            synchronizationMode = RFmxNRMXModAccSynchronizationMode.Frame;

            measurementLengthUnit = RFmxNRMXModAccMeasurementLengthUnit.Slot;
            measurementOffset = 0.0;
            measurementLength = 1;

            averagingEnabled = RFmxNRMXModAccAveragingEnabled.False;
            averagingCount = 10;

            frequencyErrorEstimation = RFmxNRMXModAccFrequencyErrorEstimation.Disabled;
            symbolClockErrorEstimationEnabled = RFmxNRMXModAccSymbolClockErrorEstimationEnabled.False;
            phaseTrackingMode = RFmxNRMXModAccPhaseTrackingMode.Disabled;
            timingTrackingMode = RFmxNRMXModAccTimingTrackingMode.Disabled;
            IQOriginOffsetEstimationEnabled = RFmxNRMXModAccIQOriginOffsetEstimationEnabled.False;

            evmReferenceDataSymbolsMode = RFmxNRMXModAccEvmReferenceDataSymbolsMode.AcquiredWaveform;
            waveformFileName = "";

        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureNR()
        {
            NR = instrSession.GetNRSignalConfiguration();     /* Create a new RFmx Session */
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency);
            instrSession.SetLOSource("", RFmxInstrMXConstants.LOSourceAutomaticSGSAShared);
            NR.SetSelectedPorts("", selectedPorts);
            NR.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
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

            NR.SelectMeasurements("", RFmxNRMXMeasurementTypes.ModAcc, false);

            NR.ModAcc.Configuration.SetSynchronizationMode("", synchronizationMode);
            NR.ModAcc.Configuration.SetAveragingEnabled("", averagingEnabled);
            NR.ModAcc.Configuration.SetAveragingCount("", averagingCount);

            NR.ModAcc.Configuration.SetMeasurementLengthUnit("", measurementLengthUnit);
            NR.ModAcc.Configuration.SetMeasurementOffset("", measurementOffset);
            NR.ModAcc.Configuration.SetMeasurementLength("", measurementLength);

            NR.ModAcc.Configuration.SetMagnitudeAndPhaseErrorEnabled("", RFmxNRMXModAccMagnitudeAndPhaseErrorEnabled.False);
            NR.ModAcc.Configuration.SetIQMismatchEstimationEnabled("", RFmxNRMXModAccIQMismatchEstimationEnabled.False);
            NR.ModAcc.Configuration.SetFrequencyErrorEstimation("", frequencyErrorEstimation);
            NR.ModAcc.Configuration.SetSymbolClockErrorEstimationEnabled("", symbolClockErrorEstimationEnabled);
            NR.ModAcc.Configuration.SetPhaseTrackingMode("", phaseTrackingMode);
            NR.ModAcc.Configuration.SetTimingTrackingMode("", timingTrackingMode);
            NR.ModAcc.Configuration.SetIQOriginOffsetEstimationEnabled("", IQOriginOffsetEstimationEnabled);

            NR.ModAcc.Configuration.SetEvmReferenceDataSymbolsMode("", evmReferenceDataSymbolsMode);
            if (evmReferenceDataSymbolsMode == RFmxNRMXModAccEvmReferenceDataSymbolsMode.ReferenceWaveform)
            {
                NIRfsgPlayback.ReadWaveformFromFileByIndexComplex(waveformFileName, 0, ref referenceWaveformSingle);
                NR.ModAcc.Configuration.ConfigureReferenceWaveform("", referenceWaveformSingle);
            }

            NR.Initiate("", "");
        }

        void RetrieveResults()
        {
            NR.ModAcc.Results.GetCompositeRmsEvmMean("", out compositeRmsEvmMean);
            NR.ModAcc.Results.GetInBandEmissionMargin("", out inBandEmissionMargin);
        }

        void PrintResults()
        {
            Console.WriteLine("------------------Measurement------------------\n");
            Console.WriteLine("Composite RMS EVM Mean (%)                     : {0}", compositeRmsEvmMean);
            Console.WriteLine("In-Band Emission Margin (dB)                   : {0}\n", inBandEmissionMargin);
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

