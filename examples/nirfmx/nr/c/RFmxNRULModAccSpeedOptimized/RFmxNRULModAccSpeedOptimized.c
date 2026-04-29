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
//13. Set ModAcc Magnitude and Phase Error Enabled and IQ Mismatch Estimation Enabled to False. This disables
//    computation of the corresponding results. Configure ModAcc Frequency Error Estimation,
//    Symbol Clock Error Estimation Enabled, Phase Tracking Mode, Timing Tracking Mode, and IQ Origin Offset Estimation Enabled.
//    Set these attributes to False/Disabled to reduce the measurement time. This disables estimation and, in turn,
//    correction of the corresponding impairments. You may disable estimation of an impairment only if it is not present
//    in the signal to be measured.
//14. Configure EVM Reference Data Sympbol Mode.
//15. Configure Reference Waveform if EVM Reference Data Symbol Mode is set as Reference Waveform.
//16. Initiate the Measurement.
//17. Fetch ModAcc Measurements.
//18. Close RFmx Session.


#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxNR.h"
#include "niRFSGPlayback.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                     256

#define NUMBER_OF_RESOURCE_BLOCK_CLUSTERS       1

#define playbackCheckWarn(fCall) if (1) {ViStatus _code_; if (_code_ = (fCall), _code_ < 0)    \
                                 {playbackError = _code_;goto Error;}        \
                                 else playbackError = (playbackError==0)?_code_:playbackError;} \
                                 else playbackError = playbackError

float64 t0 = 0.0, dt = 0.0;
int32 bufferSize = 0;
NIComplexSingle *referenceWaveformF32 = NULL;

int32 error = 0, errorOccured = 0, playbackError = 0, lastErrorCode = 0;
char errorMessage[MAX_ERROR_DESCRIPTION];

int32 ReadFromTDMSFile(char *fileName)
{
   int i = 0;
   ViReal64 t0v = 0, dtv = 0;
   playbackCheckWarn(niRFSGPlayback_ReadWaveformFromFileByIndexComplexF32(fileName, 0, 0, NULL, NULL, NULL, &bufferSize));
   if (bufferSize > 0)
   {
      referenceWaveformF32 = (NIComplexSingle*)malloc(sizeof(NIComplexSingle)*bufferSize);
      if (referenceWaveformF32)
         playbackCheckWarn(niRFSGPlayback_ReadWaveformFromFileByIndexComplexF32(fileName, 0, bufferSize, &t0v, &dtv,
         (NIComplexNumberF32*)referenceWaveformF32, NULL));
      else
      {
         printf("malloc failed\n");
         return -1;
      }
   }

   t0 = (float64)t0v;
   dt = (float64)dtv;

Error:
   if (playbackError)
   {
      errorOccured = playbackError;
      niRFSGPlayback_GetError(&lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (playbackError < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }
   return errorOccured;
}

int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   int i;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];

   char subblockString[MAX_SELECTOR_STRING];
   char carrierString[MAX_SELECTOR_STRING];
   char bandwidthPartString[MAX_SELECTOR_STRING];
   char userString[MAX_SELECTOR_STRING];
   char PUSCHString[MAX_SELECTOR_STRING];
   char PUSCHClusterString[MAX_SELECTOR_STRING];

   char *resourceName = "RFSA";

   char* frequencyReferenceSource = RFMXNR_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e6;                                          /* (Hz) */

   char* selectedPorts = "";
   float64 centerFrequency = 3.5e9;                                                       /* (Hz) */
   float64 referenceLevel = 0.0;                                                          /* (dBm) */
   float64 externalAttenuation = 0.0;                                                     /* (dB) */

   int32 enableTrigger = RFMXNR_VAL_TRUE;
   char* digitalEdgeSource = RFMXNR_VAL_PXI_TRIG0_STR;
   int32 digitalEdge = RFMXNR_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                                            /* (s) */

   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;
   int32 band = 78;
   int32 cellID = 0;
   float64 carrierBandwidth = 100e6;                                                      /* (Hz) */
   float64 subcarrierSpacing = 30e3;                                                      /* (Hz) */
   int32 autoResourceBlockDetectionEnabled = RFMXNR_VAL_AUTO_RESOURCE_BLOCK_DETECTION_ENABLED_FALSE;

   int32 PUSCHTransformPrecodingEnabled = RFMXNR_VAL_PUSCH_TRANSFORM_PRECODING_ENABLED_FALSE;
   int32 PUSCHModulationType = RFMXNR_VAL_PUSCH_MODULATION_TYPE_QPSK;
   int32 PUSCHResourceBlockOffset[NUMBER_OF_RESOURCE_BLOCK_CLUSTERS] = { 0 };
   int32 PUSCHNumberOfResourceBlocks[NUMBER_OF_RESOURCE_BLOCK_CLUSTERS] = { -1 };
   char* PUSCHSlotAllocation = "0-Last";
   char* PUSCHSymbolAllocation = "0-Last";

   int32 PUSCHDMRSPowerMode = RFMXNR_VAL_PUSCH_DMRS_POWER_MODE_CDM_GROUPS;
   float64 PUSCHDMRSPower = 0.0;                                                          /* (dB) */
   int32 PUSCHDMRSConfigurationType = RFMXNR_VAL_PUSCH_DMRS_CONFIGURATION_TYPE_TYPE1;
   int32 PUSCHMappingType = RFMXNR_VAL_PUSCH_MAPPING_TYPE_TYPE_A;
   int32 PUSCHDMRSTypeAPosition = 2;
   int32 PUSCHDMRSDuration = RFMXNR_VAL_PUSCH_DMRS_DURATION_SINGLE_SYMBOL;
   int32 PUSCHDMRSAdditionalPositions = 0;

   int32 synchronizationMode = RFMXNR_VAL_MODACC_SYNCHRONIZATION_MODE_FRAME;
   int32 averagingEnabled = RFMXNR_VAL_MODACC_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   int32 measurementLengthUnit = RFMXNR_VAL_MODACC_MEASUREMENT_LENGTH_UNIT_SLOT;
   float64 measurementOffset = 0.0;
   float64 measurementLength = 1;

   int32 frequencyErrorEstimation = RFMXNR_VAL_MODACC_FREQUENCY_ERROR_ESTIMATION_DISABLED;
   int32 symbolClockErrorEstimationEnabled = RFMXNR_VAL_MODACC_SYMBOL_CLOCK_ERROR_ESTIMATION_ENABLED_FALSE;
   int32 phaseTrackingMode = RFMXNR_VAL_MODACC_PHASE_TRACKING_MODE_DISABLED;
   int32 timingTrackingMode = RFMXNR_VAL_MODACC_TIMING_TRACKING_MODE_DISABLED;
   int32 IQOriginOffsetEstimationEnabled = RFMXNR_VAL_MODACC_IQ_ORIGIN_OFFSET_ESTIMATION_ENABLED_FALSE;

   int32 EVMReferenceDataSymbolsMode = RFMXNR_VAL_MODACC_EVM_REFERENCE_DATA_SYMBOLS_MODE_ACQUIRED_WAVEFORM;
   char waveFormFileName[] = "";

   float64 compositeRMSEVMMean = 0.0;                                                     /* (%) */
   float64 inBandEmissionMargin = 0.0;                                                    /* (dB) */

   /* Initialize a session */
   RFmxCheckWarn(RFmxNR_Initialize(resourceName, "", &instrumentHandle, NULL));

   RFmxCheckWarn(RFmxNR_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxInstr_SetLOSource(instrumentHandle, "", RFMXINSTR_VAL_LO_SOURCE_AUTOMATIC_SG_SA_SHARED));

   RFmxCheckWarn(RFmxNR_SetSelectedPorts(instrumentHandle, "", selectedPorts));

   RFmxCheckWarn(RFmxNR_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));

   RFmxCheckWarn(RFmxNR_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay,
      enableTrigger));

   RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, "", frequencyRange));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, "", carrierBandwidth));
   RFmxCheckWarn(RFmxNR_SetCellID(instrumentHandle, "", cellID));
   RFmxCheckWarn(RFmxNR_SetBand(instrumentHandle, "", band));
   RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, "", subcarrierSpacing));
   RFmxCheckWarn(RFmxNR_SetAutoResourceBlockDetectionEnabled(instrumentHandle, "", autoResourceBlockDetectionEnabled));

   RFmxCheckWarn(RFmxNR_SetPUSCHTransformPrecodingEnabled(instrumentHandle, "", PUSCHTransformPrecodingEnabled));
   RFmxCheckWarn(RFmxNR_SetPUSCHSlotAllocation(instrumentHandle, "", PUSCHSlotAllocation));
   RFmxCheckWarn(RFmxNR_SetPUSCHSymbolAllocation(instrumentHandle, "", PUSCHSymbolAllocation));
   RFmxCheckWarn(RFmxNR_SetPUSCHModulationType(instrumentHandle, "", PUSCHModulationType));

   RFmxCheckWarn(RFmxNR_SetPUSCHNumberOfResourceBlockClusters(instrumentHandle, "",
      NUMBER_OF_RESOURCE_BLOCK_CLUSTERS));

   RFmxNR_BuildSubblockString("", 0, MAX_SELECTOR_STRING, subblockString);
   RFmxNR_BuildCarrierString(subblockString, 0, MAX_SELECTOR_STRING, carrierString);
   RFmxNR_BuildBandwidthPartString(carrierString, 0, MAX_SELECTOR_STRING, bandwidthPartString);
   RFmxNR_BuildUserString(bandwidthPartString, 0, MAX_SELECTOR_STRING, userString);
   RFmxNR_BuildPUSCHString(userString, 0, MAX_SELECTOR_STRING, PUSCHString);
   for (i = 0; i < NUMBER_OF_RESOURCE_BLOCK_CLUSTERS; i++)
   {
      RFmxNR_BuildPUSCHClusterString(PUSCHString, i, MAX_SELECTOR_STRING, PUSCHClusterString);
      RFmxCheckWarn(RFmxNR_SetPUSCHResourceBlockOffset(instrumentHandle, PUSCHClusterString,
         PUSCHResourceBlockOffset[i]));
      RFmxCheckWarn(RFmxNR_SetPUSCHNumberOfResourceBlocks(instrumentHandle, PUSCHClusterString,
         PUSCHNumberOfResourceBlocks[i]));
   }

   RFmxCheckWarn(RFmxNR_SetPUSCHDMRSPowerMode(instrumentHandle, "", PUSCHDMRSPowerMode));
   RFmxCheckWarn(RFmxNR_SetPUSCHDMRSPower(instrumentHandle, "", PUSCHDMRSPower));
   RFmxCheckWarn(RFmxNR_SetPUSCHDMRSConfigurationType(instrumentHandle, "", PUSCHDMRSConfigurationType));
   RFmxCheckWarn(RFmxNR_SetPUSCHMappingType(instrumentHandle, "", PUSCHMappingType));
   RFmxCheckWarn(RFmxNR_SetPUSCHDMRSTypeAPosition(instrumentHandle, "", PUSCHDMRSTypeAPosition));
   RFmxCheckWarn(RFmxNR_SetPUSCHDMRSDuration(instrumentHandle, "", PUSCHDMRSDuration));
   RFmxCheckWarn(RFmxNR_SetPUSCHDMRSAdditionalPositions(instrumentHandle, "", PUSCHDMRSAdditionalPositions));

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_MODACC, RFMXNR_VAL_FALSE));

   RFmxCheckWarn(RFmxNR_ModAccSetSynchronizationMode(instrumentHandle, "", synchronizationMode));
   RFmxCheckWarn(RFmxNR_ModAccSetAveragingEnabled(instrumentHandle, "", averagingEnabled));
   RFmxCheckWarn(RFmxNR_ModAccSetAveragingCount(instrumentHandle, "", averagingCount));

   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementLengthUnit(instrumentHandle, "", measurementLengthUnit));
   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementOffset(instrumentHandle, "", measurementOffset));
   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementLength(instrumentHandle, "", measurementLength));

   RFmxCheckWarn(RFmxNR_ModAccSetMagnitudeAndPhaseErrorEnabled(instrumentHandle, "", RFMXNR_VAL_MODACC_MAGNITUDE_AND_PHASE_ERROR_ENABLED_FALSE));
   RFmxCheckWarn(RFmxNR_ModAccSetIQMismatchEstimationEnabled(instrumentHandle, "", RFMXNR_VAL_MODACC_IQ_MISMATCH_ESTIMATION_ENABLED_FALSE));
   RFmxCheckWarn(RFmxNR_ModAccSetFrequencyErrorEstimation(instrumentHandle, "", frequencyErrorEstimation));
   RFmxCheckWarn(RFmxNR_ModAccSetSymbolClockErrorEstimationEnabled(instrumentHandle, "", symbolClockErrorEstimationEnabled));
   RFmxCheckWarn(RFmxNR_ModAccSetPhaseTrackingMode(instrumentHandle, "", phaseTrackingMode));
   RFmxCheckWarn(RFmxNR_ModAccSetTimingTrackingMode(instrumentHandle, "", timingTrackingMode));
   RFmxCheckWarn(RFmxNR_ModAccSetIQOriginOffsetEstimationEnabled(instrumentHandle, "", IQOriginOffsetEstimationEnabled));

   RFmxCheckWarn(RFmxNR_ModAccSetEVMReferenceDataSymbolsMode(instrumentHandle, "", EVMReferenceDataSymbolsMode));
   if (EVMReferenceDataSymbolsMode == RFMXNR_VAL_MODACC_EVM_REFERENCE_DATA_SYMBOLS_MODE_REFERENCE_WAVEFORM)
   {
      int readError;
      readError = ReadFromTDMSFile(waveFormFileName);
      if (readError < 0)
      {
         printf("Cannot open the specified waveform file\n");
         goto Error;
      }
      RFmxCheckWarn(RFmxNR_ModAccCfgReferenceWaveform(instrumentHandle, "", t0, dt, referenceWaveformF32, bufferSize));
   }

   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

   /* Fetch results */
   RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositeRMSEVMMean(instrumentHandle, "", &compositeRMSEVMMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsInBandEmissionMargin(instrumentHandle, "", &inBandEmissionMargin));

   printf("------------------Measurement------------------\n\n");
   printf("Composite RMS EVM Mean (%%)                    : %lf\n", compositeRMSEVMMean);
   printf("In-Band Emission Margin (dB)                  : %lf\n\n", inBandEmissionMargin);


Error:
   if (error)
   {
      RFmxNR_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (instrumentHandle)
   {
      RFmxNR_Close(instrumentHandle, RFMXNR_VAL_FALSE);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}