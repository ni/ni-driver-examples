//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Frequency Range, CC bandwidth, Cell ID, Band, BWP Subcarrier Spacing and Auto RB Detection Enabled.
//7. Configure PUSCH and PUSCH RB Allocation.
//8. Configure PUSCH DMRS.
//9. Select ModAcc measurement and enable Traces.
//10. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
//11. Configure Measurement Interval.
//12. Initiate the Measurement.
//13. Fetch ModAcc Measurements and Traces.
//14. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxNR.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                     256

#define NUMBER_OF_RESOURCE_BLOCK_CLUSTERS       1

int main(int argc, char *argv[])
{
   char* resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0;

   char subblockString[MAX_SELECTOR_STRING];
   char carrierString[MAX_SELECTOR_STRING];
   char bandwidthPartString[MAX_SELECTOR_STRING];
   char userString[MAX_SELECTOR_STRING];
   char PUSCHString[MAX_SELECTOR_STRING];
   char PUSCHClusterString[MAX_SELECTOR_STRING];

   char* selectedPorts = "";
   float64 centerFrequency = 3.5e9;                                                       /* (Hz) */
   float64 referenceLevel = 0.0;                                                          /* (dBm) */
   float64 externalAttenuation = 0.0;                                                     /* (dB) */

   char* frequencyReferenceSource = RFMXNR_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e6;                                          /* (Hz) */

   int32 enableTrigger = RFMXNR_VAL_FALSE;
   char* digitalEdgeSource = RFMXNR_VAL_PXI_TRIG0_STR;
   int32 digitalEdge = RFMXNR_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                                            /* (s) */

   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;
   int32 band = 78;
   int32 cellID = 0;
   float64 carrierBandwidth = 100e6;                                                      /* (Hz) */
   float64 subcarrierSpacing = 30e3;                                                      /* (Hz) */
   int32 autoResourceBlockDetectionEnabled = RFMXNR_VAL_AUTO_RESOURCE_BLOCK_DETECTION_ENABLED_TRUE;

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

   int32 synchronizationMode = RFMXNR_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;

   int32 measurementLengthUnit = RFMXNR_VAL_MODACC_MEASUREMENT_LENGTH_UNIT_SLOT;
   float64 measurementOffset = 0.0;
   float64 measurementLength = 1;

   int32 averagingEnabled = RFMXNR_VAL_MODACC_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   float64 timeout = 10.0;                                                                /* (s) */

   float64 compositeRMSEVMMean = 0.0;                                                     /* (%) */
   float64 compositePeakEVMMaximum = 0.0;                                                 /* (%) */
   int32 compositePeakEVMSlotIndex = 0;
   int32 compositePeakEVMSymbolIndex = 0;
   int32 compositePeakEVMSubcarrierIndex = 0;

   float64 componentCarrierFrequencyErrorMean = 0.0;                                      /* (Hz) */
   float64 componentCarrierIQOriginOffsetMean = 0.0;                                      /* (dBc) */
   float64 componentCarrierIQGainImbalanceMean = 0.0;                                     /* (dB) */
   float64 componentCarrierQuadratureErrorMean = 0.0;                                     /* (deg) */
   float64 inBandEmissionMargin = 0.0;                                                    /* (dB) */

   int32 actualArraySize = 0;
   NIComplexSingle* PUSCHDataConstellation = NULL;
   NIComplexSingle* PUSCHDMRSConstellation = NULL;

   float64 x0 = 0.0, dx = 0.0;
   float32* RMSEVMPerSubcarrierMean = NULL;
   float32* RMSEVMPerSymbolMean = NULL;

   float32* spectralFlatness = NULL;
   float32* spectralFlatnessLowerMask = NULL;
   float32* spectralFlatnessUpperMask = NULL;

   /* Initialize a session */
   RFmxCheckWarn(RFmxNR_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxNR_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
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

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_MODACC, RFMXNR_VAL_TRUE));

   RFmxCheckWarn(RFmxNR_ModAccSetSynchronizationMode(instrumentHandle, "", synchronizationMode));
   RFmxCheckWarn(RFmxNR_ModAccSetAveragingEnabled(instrumentHandle, "", averagingEnabled));
   RFmxCheckWarn(RFmxNR_ModAccSetAveragingCount(instrumentHandle, "", averagingCount));

   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementLengthUnit(instrumentHandle, "", measurementLengthUnit));
   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementOffset(instrumentHandle, "", measurementOffset));
   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementLength(instrumentHandle, "", measurementLength));

   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

   /* Fetch results */
   RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositeRMSEVMMean(instrumentHandle, "", &compositeRMSEVMMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMMaximum(instrumentHandle, "", &compositePeakEVMMaximum));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMSlotIndex(instrumentHandle, "", &compositePeakEVMSlotIndex));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMSymbolIndex(instrumentHandle, "",
      &compositePeakEVMSymbolIndex));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMSubcarrierIndex(instrumentHandle, "",
      &compositePeakEVMSubcarrierIndex));

   RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierFrequencyErrorMean(instrumentHandle, "",
      &componentCarrierFrequencyErrorMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierIQOriginOffsetMean(instrumentHandle, "",
      &componentCarrierIQOriginOffsetMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierIQGainImbalanceMean(instrumentHandle, "",
      &componentCarrierIQGainImbalanceMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierQuadratureErrorMean(instrumentHandle, "",
      &componentCarrierQuadratureErrorMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsInBandEmissionMargin(instrumentHandle, "", &inBandEmissionMargin));

   RFmxCheckWarn(RFmxNR_ModAccFetchPUSCHDataConstellationTrace(instrumentHandle, "", timeout,
      NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      PUSCHDataConstellation = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
      if (PUSCHDataConstellation)
      {
         RFmxCheckWarn(RFmxNR_ModAccFetchPUSCHDataConstellationTrace(instrumentHandle, "", timeout,
            PUSCHDataConstellation, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_ModAccFetchPUSCHDMRSConstellationTrace(instrumentHandle, "", timeout,
      NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      PUSCHDMRSConstellation = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
      if (PUSCHDMRSConstellation)
      {
         RFmxCheckWarn(RFmxNR_ModAccFetchPUSCHDMRSConstellationTrace(instrumentHandle, "", timeout,
            PUSCHDMRSConstellation, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSubcarrierMeanTrace(instrumentHandle, "", timeout,
      NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      RMSEVMPerSubcarrierMean = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (RMSEVMPerSubcarrierMean)
      {
         RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSubcarrierMeanTrace(instrumentHandle, "", timeout,
            &x0, &dx, RMSEVMPerSubcarrierMean, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSymbolMeanTrace(instrumentHandle, "", timeout,
      NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      RMSEVMPerSymbolMean = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (RMSEVMPerSymbolMean)
      {
         RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSymbolMeanTrace(instrumentHandle, "", timeout,
            &x0, &dx, RMSEVMPerSymbolMean, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_ModAccFetchSpectralFlatnessTrace(instrumentHandle, "", timeout,
      NULL, NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      spectralFlatness = (float32*)malloc(sizeof(float32) * actualArraySize);
      spectralFlatnessLowerMask = (float32*)malloc(sizeof(float32) * actualArraySize);
      spectralFlatnessUpperMask = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (spectralFlatness && spectralFlatnessLowerMask && spectralFlatnessUpperMask)
      {
         RFmxCheckWarn(RFmxNR_ModAccFetchSpectralFlatnessTrace(instrumentHandle, "", timeout, &x0, &dx,
            spectralFlatness, spectralFlatnessLowerMask, spectralFlatnessUpperMask, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("------------------Measurement------------------\n\n");
   printf("Composite RMS EVM Mean (%%)                    : %lf\n", compositeRMSEVMMean);
   printf("Composite Peak EVM Maximum (%%)                : %lf\n", compositePeakEVMMaximum);
   printf("Composite Peak EVM Slot Index                 : %d\n", compositePeakEVMSlotIndex);
   printf("Composite Peak EVM Symbol Index               : %d\n", compositePeakEVMSymbolIndex);
   printf("Composite Peak EVM Subcarrier Index           : %d\n", compositePeakEVMSubcarrierIndex);
   printf("Component Carrier Frequency Error Mean (Hz)   : %lf\n", componentCarrierFrequencyErrorMean);
   printf("Component Carrier IQ Origin Offset Mean (dBc) : %lf\n", componentCarrierIQOriginOffsetMean);
   printf("Component Carrier IQ Gain Imbalance Mean (dB) : %lf\n", componentCarrierIQGainImbalanceMean);
   printf("Component Carrier Quadrature Error Mean (deg) : %lf\n", componentCarrierQuadratureErrorMean);
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

   /* Free allocated memory */
   if (PUSCHDataConstellation)
   {
      free(PUSCHDataConstellation);
   }
   if (PUSCHDMRSConstellation)
   {
      free(PUSCHDMRSConstellation);
   }
   if (RMSEVMPerSubcarrierMean) 
   {
      free(RMSEVMPerSubcarrierMean);
   }
   if (RMSEVMPerSymbolMean) 
   {
      free(RMSEVMPerSymbolMean);
   }
   if (spectralFlatness)
   {
      free(spectralFlatness);
   }
   if (spectralFlatnessLowerMask)
   {
      free(spectralFlatnessLowerMask);
   }
   if (spectralFlatnessUpperMask)
   {
      free(spectralFlatnessUpperMask);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
