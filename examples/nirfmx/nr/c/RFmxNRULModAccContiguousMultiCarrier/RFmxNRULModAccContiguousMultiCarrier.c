//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Frequency Range, Band, Channel Raster, Component Carrier Spacing, CC at Center Frequency,
//   Auto RB Detection Enabled and Auto Increment Cell ID Enabled.
//7. Configure Carrier.
//8. Configure PUSCH and PUSCH RB Allocation.
//9. Configure PUSCH DMRS.
//10. Select ModAcc measurement and enable Traces.
//11. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
//12. Configure Measurement Interval.
//13. Initiate the Measurement.
//14. Fetch ModAcc Measurements and Traces.
//15. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <string.h>

#include "niRFmxNR.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                  256

#define NUMBER_OF_COMPONENT_CARRIERS         2
#define NUMBER_OF_RESOURCE_BLOCK_CLUSTERS    1

int main(int argc, char *argv[])
{
   char* resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0, j = 0;

   char subblockString[MAX_SELECTOR_STRING];
   char carrierString[MAX_SELECTOR_STRING];
   char PUSCHClusterString[MAX_SELECTOR_STRING];

   char* selectedPorts = "";
   float64 centerFrequency = 3.5e9;                                                          /* (Hz) */
   float64 referenceLevel = 0.0;                                                             /* (dBm) */
   float64 externalAttenuation = 0.0;                                                        /* (dB) */

   char* frequencyReferenceSource = RFMXNR_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e6;                                             /* (Hz) */

   int32 enableTrigger = RFMXNR_VAL_FALSE;
   char* digitalEdgeSource = RFMXNR_VAL_PXI_TRIG0_STR;
   int32 digitalEdge = RFMXNR_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                                               /* (s) */

   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;
   int32 band = 78;
   float64 subcarrierSpacing = 30e3;                                                         /* (Hz) */
   int32 autoResourceBlockDetectionEnabled = RFMXNR_VAL_AUTO_RESOURCE_BLOCK_DETECTION_ENABLED_TRUE;
   int32 autoIncrementCellIDEnabled = RFMXNR_VAL_AUTO_INCREMENT_CELL_ID_ENABLED_TRUE;

   int32 componentCarrierSpacingType = RFMXNR_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL;
   float64 channelRaster = 15e3;                                                             /* (Hz) */
   int32 componentCarrierAtCenterFrequency = -1;

   float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS] = { 100e6, 100e6 };       /* (Hz) */
   float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS] = { -49.98e6, 50.01e6 };  /* (Hz) */
   int32 cellID[NUMBER_OF_COMPONENT_CARRIERS] = { 0, 1 };

   int32 PUSCHTransformPrecodingEnabled = RFMXNR_VAL_PUSCH_TRANSFORM_PRECODING_ENABLED_FALSE;
   int32 PUSCHModulationType = RFMXNR_VAL_PUSCH_MODULATION_TYPE_QPSK;
   int32 PUSCHResourceBlockOffset[NUMBER_OF_RESOURCE_BLOCK_CLUSTERS] = { 0 };
   int32 PUSCHNumberOfResourceBlocks[NUMBER_OF_RESOURCE_BLOCK_CLUSTERS] = { -1 };
   char* PUSCHSlotAllocation = "0-Last";
   char* PUSCHSymbolAllocation = "0-Last";

   int32 PUSCHDMRSPowerMode = RFMXNR_VAL_PUSCH_DMRS_POWER_MODE_CDM_GROUPS;
   float64 PUSCHDMRSPower = 0.0;                                                             /* (dB) */
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

   float64 timeout = 10.0;                                                                   /* (s) */

   float64 compositeRMSEVMMean[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };                      /* (%) */
   float64 compositePeakEVMMaximum[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };                  /* (%) */
   int32 compositePeakEVMSlotIndex[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };
   int32 compositePeakEVMSymbolIndex[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };
   int32 compositePeakEVMSubcarrierIndex[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };
   float64 componentCarrierFrequencyErrorMean[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };       /* (Hz) */
   float64 componentCarrierIQOriginOffsetMean[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };       /* (dBc) */
   float64 componentCarrierIQGainImbalanceMean[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };      /* (dB) */
   float64 componentCarrierQuadratureErrorMean[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };      /* (deg) */
   float64 inBandEmissionMargin[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };                     /* (dB) */

   int32 actualArraySize = 0;
   NIComplexSingle* PUSCHDataConstellation[NUMBER_OF_COMPONENT_CARRIERS] = { NULL };
   NIComplexSingle* PUSCHDMRSConstellation[NUMBER_OF_COMPONENT_CARRIERS] = { NULL };

   float64 x0 = 0.0, dx = 0.0;
   float32* RMSEVMPerSubcarrierMean[NUMBER_OF_COMPONENT_CARRIERS] = { NULL };
   float32* RMSEVMPerSymbolMean[NUMBER_OF_COMPONENT_CARRIERS] = { NULL };

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
   RFmxCheckWarn(RFmxNR_SetBand(instrumentHandle, "", band));
   RFmxCheckWarn(RFmxNR_SetChannelRaster(instrumentHandle, "", channelRaster));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierSpacingType(instrumentHandle, "", componentCarrierSpacingType));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierAtCenterFrequency(instrumentHandle, "", componentCarrierAtCenterFrequency));
   RFmxCheckWarn(RFmxNR_SetAutoResourceBlockDetectionEnabled(instrumentHandle, "", autoResourceBlockDetectionEnabled));
   RFmxCheckWarn(RFmxNR_SetAutoIncrementCellIDEnabled(instrumentHandle, "", autoIncrementCellIDEnabled));

   RFmxCheckWarn(RFmxNR_SetNumberOfComponentCarriers(instrumentHandle, "", NUMBER_OF_COMPONENT_CARRIERS));

   RFmxNR_BuildSubblockString("", 0, MAX_SELECTOR_STRING, subblockString);
   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      RFmxNR_BuildCarrierString(subblockString, i, MAX_SELECTOR_STRING, carrierString);
      RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, carrierString, componentCarrierBandwidth[i]));
      RFmxCheckWarn(RFmxNR_SetCellID(instrumentHandle, carrierString, cellID[i]));
      RFmxCheckWarn(RFmxNR_SetComponentCarrierFrequency(instrumentHandle, carrierString, componentCarrierFrequency[i]));
   }

   strcpy_s(carrierString, sizeof("carrier::all"), "carrier::all");
   RFmxCheckWarn(RFmxNR_SetPUSCHTransformPrecodingEnabled(instrumentHandle, carrierString,
      PUSCHTransformPrecodingEnabled));
   RFmxCheckWarn(RFmxNR_SetPUSCHModulationType(instrumentHandle, carrierString, PUSCHModulationType));
   RFmxCheckWarn(RFmxNR_SetPUSCHSlotAllocation(instrumentHandle, carrierString, PUSCHSlotAllocation));
   RFmxCheckWarn(RFmxNR_SetPUSCHSymbolAllocation(instrumentHandle, carrierString, PUSCHSymbolAllocation));

   RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, carrierString, subcarrierSpacing));
   RFmxCheckWarn(RFmxNR_SetPUSCHNumberOfResourceBlockClusters(instrumentHandle, carrierString,
      NUMBER_OF_RESOURCE_BLOCK_CLUSTERS));

   for (i = 0; i < NUMBER_OF_RESOURCE_BLOCK_CLUSTERS; i++)
   {
      RFmxNR_BuildPUSCHClusterString(carrierString, i, MAX_SELECTOR_STRING, PUSCHClusterString);
      RFmxCheckWarn(RFmxNR_SetPUSCHResourceBlockOffset(instrumentHandle, PUSCHClusterString,
         PUSCHResourceBlockOffset[i]));
      RFmxCheckWarn(RFmxNR_SetPUSCHNumberOfResourceBlocks(instrumentHandle, PUSCHClusterString,
         PUSCHNumberOfResourceBlocks[i]));
   }

   RFmxCheckWarn(RFmxNR_SetPUSCHDMRSPowerMode(instrumentHandle, carrierString, PUSCHDMRSPowerMode));
   RFmxCheckWarn(RFmxNR_SetPUSCHDMRSPower(instrumentHandle, carrierString, PUSCHDMRSPower));
   RFmxCheckWarn(RFmxNR_SetPUSCHDMRSConfigurationType(instrumentHandle, carrierString, PUSCHDMRSConfigurationType));
   RFmxCheckWarn(RFmxNR_SetPUSCHMappingType(instrumentHandle, carrierString, PUSCHMappingType));
   RFmxCheckWarn(RFmxNR_SetPUSCHDMRSTypeAPosition(instrumentHandle, carrierString, PUSCHDMRSTypeAPosition));
   RFmxCheckWarn(RFmxNR_SetPUSCHDMRSDuration(instrumentHandle, carrierString, PUSCHDMRSDuration));
   RFmxCheckWarn(RFmxNR_SetPUSCHDMRSAdditionalPositions(instrumentHandle, carrierString, PUSCHDMRSAdditionalPositions));

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_MODACC, RFMXNR_VAL_TRUE));

   RFmxCheckWarn(RFmxNR_ModAccSetSynchronizationMode(instrumentHandle, "", synchronizationMode));
   RFmxCheckWarn(RFmxNR_ModAccSetAveragingEnabled(instrumentHandle, "", averagingEnabled));
   RFmxCheckWarn(RFmxNR_ModAccSetAveragingCount(instrumentHandle, "", averagingCount));

   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementLengthUnit(instrumentHandle, "", measurementLengthUnit));
   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementOffset(instrumentHandle, "", measurementOffset));
   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementLength(instrumentHandle, "", measurementLength));

   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

   /* Fetch results */
   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      RFmxNR_BuildCarrierString(subblockString, i, MAX_SELECTOR_STRING, carrierString);

      RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositeRMSEVMMean(instrumentHandle, carrierString,
         &compositeRMSEVMMean[i]));
      RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMMaximum(instrumentHandle, carrierString,
         &compositePeakEVMMaximum[i]));
      RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMSlotIndex(instrumentHandle, carrierString,
         &compositePeakEVMSlotIndex[i]));
      RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMSymbolIndex(instrumentHandle, carrierString,
         &compositePeakEVMSymbolIndex[i]));
      RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMSubcarrierIndex(instrumentHandle, carrierString,
         &compositePeakEVMSubcarrierIndex[i]));
      RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierFrequencyErrorMean(instrumentHandle, carrierString,
         &componentCarrierFrequencyErrorMean[i]));
      RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierIQOriginOffsetMean(instrumentHandle, carrierString,
         &componentCarrierIQOriginOffsetMean[i]));
      RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierIQGainImbalanceMean(instrumentHandle, carrierString,
         &componentCarrierIQGainImbalanceMean[i]));
      RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierQuadratureErrorMean(instrumentHandle, carrierString,
         &componentCarrierQuadratureErrorMean[i]));
      RFmxCheckWarn(RFmxNR_ModAccGetResultsInBandEmissionMargin(instrumentHandle, carrierString,
         &inBandEmissionMargin[i]));
   }

   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      RFmxNR_BuildCarrierString("", i, MAX_SELECTOR_STRING, carrierString);

      actualArraySize = 0;
      RFmxCheckWarn(RFmxNR_ModAccFetchPUSCHDataConstellationTrace(instrumentHandle, carrierString, timeout,
         NULL, 0, &actualArraySize));
      if (actualArraySize > 0)
      {
         PUSCHDataConstellation[i] = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
         if (PUSCHDataConstellation)
         {
            RFmxCheckWarn(RFmxNR_ModAccFetchPUSCHDataConstellationTrace(instrumentHandle, carrierString, timeout,
               PUSCHDataConstellation[i], actualArraySize, NULL));
         }
         else
         {
            printf("malloc failed.\n");
            goto Error;
         }
      }

      actualArraySize = 0;
      RFmxCheckWarn(RFmxNR_ModAccFetchPUSCHDMRSConstellationTrace(instrumentHandle, carrierString, timeout,
         NULL, 0, &actualArraySize));
      if (actualArraySize > 0)
      {
         PUSCHDMRSConstellation[i] = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
         if (PUSCHDMRSConstellation)
         {
            RFmxCheckWarn(RFmxNR_ModAccFetchPUSCHDMRSConstellationTrace(instrumentHandle, carrierString, timeout,
               PUSCHDMRSConstellation[i], actualArraySize, NULL));
         }
         else
         {
            printf("malloc failed.\n");
            goto Error;
         }
      }
   }

   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      RFmxNR_BuildCarrierString(subblockString, i, MAX_SELECTOR_STRING, carrierString);

      actualArraySize = 0;
      RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSubcarrierMeanTrace(instrumentHandle, carrierString, timeout,
         NULL, NULL, NULL, 0, &actualArraySize));
      if (actualArraySize > 0)
      {
         RMSEVMPerSubcarrierMean[i] = (float32*)malloc(sizeof(float32) * actualArraySize);
         if (RMSEVMPerSubcarrierMean[i])
         {
            RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSubcarrierMeanTrace(instrumentHandle, carrierString, timeout,
               &x0, &dx, RMSEVMPerSubcarrierMean[i], actualArraySize, NULL));
         }
         else
         {
            printf("malloc failed.\n");
            goto Error;
         }
      }

      actualArraySize = 0;
      RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSymbolMeanTrace(instrumentHandle, carrierString, timeout,
         NULL, NULL, NULL, 0, &actualArraySize));
      if (actualArraySize > 0)
      {
         RMSEVMPerSymbolMean[i] = (float32*)malloc(sizeof(float32) * actualArraySize);
         if (RMSEVMPerSymbolMean[i])
         {
            RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSymbolMeanTrace(instrumentHandle, carrierString, timeout,
               &x0, &dx, RMSEVMPerSymbolMean[i], actualArraySize, NULL));
         }
         else
         {
            printf("malloc failed.\n");
            goto Error;
         }
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_ModAccFetchSpectralFlatnessTrace(instrumentHandle, "", timeout, NULL, NULL,
      NULL, NULL, NULL, 0, &actualArraySize));
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

   printf("------------------------Measurements------------------------\n\n");
   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      printf("Carrier  : %d\n", i);
      printf("Composite RMS EVM Mean (%%)                    : %lf\n", compositeRMSEVMMean[i]);
      printf("Composite Peak EVM Maximum (%%)                : %lf\n", compositePeakEVMMaximum[i]);
      printf("Composite Peak EVM Slot Index                 : %d\n", compositePeakEVMSlotIndex[i]);
      printf("Composite Peak EVM Symbol Index               : %d\n", compositePeakEVMSymbolIndex[i]);
      printf("Composite Peak EVM Subcarrier Index           : %d\n", compositePeakEVMSubcarrierIndex[i]);
      printf("Component Carrier Frequency Error Mean (Hz)   : %lf\n", componentCarrierFrequencyErrorMean[i]);
      printf("Component Carrier IQ Origin Offset Mean (dBc) : %lf\n", componentCarrierIQOriginOffsetMean[i]);
      printf("Component Carrier IQ Gain Imbalance Mean (dB) : %lf\n", componentCarrierIQGainImbalanceMean[i]);
      printf("Component Carrier Quadrature Error Mean (deg) : %lf\n", componentCarrierQuadratureErrorMean[i]);
      printf("In-Band Emission Margin (dB)                  : %lf\n", inBandEmissionMargin[i]);
      printf("-------------------------------------------------\n\n");
   }

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
   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
       if (PUSCHDataConstellation[i])
       {
         free(PUSCHDataConstellation[i]);
      }
       if (PUSCHDMRSConstellation[i])
       {
         free(PUSCHDMRSConstellation[i]);
       }
       if (RMSEVMPerSubcarrierMean[i])
       {
         free(RMSEVMPerSubcarrierMean[i]);
       }
       if (RMSEVMPerSymbolMean[i])
       {
         free(RMSEVMPerSymbolMean[i]);
       }
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
