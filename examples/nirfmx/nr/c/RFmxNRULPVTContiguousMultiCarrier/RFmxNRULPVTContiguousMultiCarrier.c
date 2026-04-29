//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Parameters for IQ Power Edge Trigger.
//6. Configure Frequency Range, Channel Raster, Component Carrier Spacing and Number of Component Carriers.
//7. Configure Component Carriers.
//8. Configure PUSCH and PUSCH RB Allocation.
//9. Configure PUSCH DMRS.
//10. Select PVT measurement and enable Traces.
//11. Configure Measurement Methods.
//12. Configure OFF Power Exclusion Periods.
//13. Configure Averaging Parameters for PVT measurement.
//14. Initiate the Measurement.
//15. Fetch PVT Traces and Measurements.
//16. Close RFmx Session.

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

   float64 IQPowerEdgeLevel = -20.0;                                                         /* (dB) */
   float64 triggerDelay = 0.0;                                                               /* (s) */
   int32 minimumQuietTimeMode = RFMXNR_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 8.0e-6;                                                        /* (s) */

   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;

   int32 componentCarrierSpacingType = RFMXNR_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL;
   float64 channelRaster = 15e3;                                                             /* (Hz) */
   int32 componentCarrierAtCenterFrequency = -1;
   float64 subcarrierSpacing = 30e3;                                                         /* (Hz) */

   float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS] = { 100e6, 100e6 };       /* (Hz) */
   float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS] = { -49.98e6, 50.01e6 };  /* (Hz) */
   int32 cellID[NUMBER_OF_COMPONENT_CARRIERS] = { 0, 1 };

   int32 PUSCHTransformPrecodingEnabled = RFMXNR_VAL_PUSCH_TRANSFORM_PRECODING_ENABLED_FALSE;
   int32 PUSCHModulationType = RFMXNR_VAL_PUSCH_MODULATION_TYPE_QPSK;
   int32 PUSCHResourceBlockOffset[NUMBER_OF_RESOURCE_BLOCK_CLUSTERS] = { 0 };
   int32 PUSCHNumberOfResourceBlocks[NUMBER_OF_RESOURCE_BLOCK_CLUSTERS] = { -1 };
   char* PUSCHSlotAllocation = "1";
   char* PUSCHSymbolAllocation = "0-Last";

   int32 PUSCHDMRSPowerMode = RFMXNR_VAL_PUSCH_DMRS_POWER_MODE_CDM_GROUPS;
   float64 PUSCHDMRSPower = 0.0;                                                             /* (dB) */
   int32 PUSCHDMRSConfigurationType = RFMXNR_VAL_PUSCH_DMRS_CONFIGURATION_TYPE_TYPE1;
   int32 PUSCHMappingType = RFMXNR_VAL_PUSCH_MAPPING_TYPE_TYPE_A;
   int32 PUSCHDMRSTypeAPosition = 2;
   int32 PUSCHDMRSDuration = RFMXNR_VAL_PUSCH_DMRS_DURATION_SINGLE_SYMBOL;
   int32 PUSCHDMRSAdditionalPositions = 0;

   int32 measurementMethod = RFMXNR_VAL_PVT_MEASUREMENT_METHOD_NORMAL;
   float64 OFFPowerExclusionBefore = 0.0;                                                    /* (s) */
   float64 OFFPowerExclusionAfter = 0.0;                                                     /* (s) */

   int32 averagingEnabled = RFMXNR_VAL_PVT_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXNR_VAL_PVT_AVERAGING_TYPE_RMS;

   float64 timeout = 10.0;                                                                   /* (s) */

   int32 measurementStatus[NUMBER_OF_COMPONENT_CARRIERS] = { RFMXNR_VAL_PVT_MEASUREMENT_STATUS_FAIL };
   float64 absoluteOFFPowerBefore[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };                   /* (dBm) */
   float64 absoluteOFFPowerAfter[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };                    /* (dBm) */
   float64 absoluteONPower[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };                          /* (dBm) */
   float64 burstWidth[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };                               /* (s) */

   int32 actualArraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32* signalPower[NUMBER_OF_COMPONENT_CARRIERS] = { NULL };                             /* (dBm) */
   float32* absoluteLimit[NUMBER_OF_COMPONENT_CARRIERS] = { NULL };                           /* (dBm) */

   /* Initialize a session */
   RFmxCheckWarn(RFmxNR_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxNR_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxNR_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxNR_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxNR_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXNR_VAL_IQ_POWER_EDGE_RISING_SLOPE,
      IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
      RFMXNR_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, RFMXNR_VAL_TRUE));

   RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, "", frequencyRange));
   RFmxCheckWarn(RFmxNR_SetChannelRaster(instrumentHandle, "", channelRaster));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierSpacingType(instrumentHandle, "", componentCarrierSpacingType));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierAtCenterFrequency(instrumentHandle, "", componentCarrierAtCenterFrequency));
   RFmxCheckWarn(RFmxNR_SetNumberOfComponentCarriers(instrumentHandle, "", NUMBER_OF_COMPONENT_CARRIERS));

   RFmxNR_BuildSubblockString("", 0, MAX_SELECTOR_STRING, subblockString);
   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      RFmxNR_BuildCarrierString(subblockString, i, MAX_SELECTOR_STRING, carrierString);
      RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, carrierString, componentCarrierBandwidth[i]));
      RFmxCheckWarn(RFmxNR_SetComponentCarrierFrequency(instrumentHandle, carrierString, componentCarrierFrequency[i]));
      RFmxCheckWarn(RFmxNR_SetCellID(instrumentHandle, carrierString, cellID[i]));
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

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_PVT, RFMXNR_VAL_TRUE));

   RFmxCheckWarn(RFmxNR_PVTCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
   RFmxCheckWarn(RFmxNR_PVTCfgOFFPowerExclusionPeriods(instrumentHandle, "", OFFPowerExclusionBefore,
      OFFPowerExclusionAfter));
   RFmxCheckWarn(RFmxNR_PVTCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));

   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

   /* Fetch results */
   RFmxCheckWarn(RFmxNR_PVTFetchMeasurementArray(instrumentHandle, "", timeout, measurementStatus,
      absoluteOFFPowerBefore, absoluteOFFPowerAfter, absoluteONPower, burstWidth, NUMBER_OF_COMPONENT_CARRIERS, NULL));

   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      RFmxNR_BuildCarrierString("", i, MAX_SELECTOR_STRING, carrierString);

      actualArraySize = 0;
      RFmxCheckWarn(RFmxNR_PVTFetchSignalPowerTrace(instrumentHandle, carrierString, timeout, NULL, NULL, NULL, NULL,
         0, &actualArraySize));
      if (actualArraySize > 0)
      {
         signalPower[i] = (float32*)malloc(sizeof(float32) * actualArraySize);
         absoluteLimit[i] = (float32*)malloc(sizeof(float32) * actualArraySize);
         if (signalPower[i] && absoluteLimit[i])
         {
            RFmxCheckWarn(RFmxNR_PVTFetchSignalPowerTrace(instrumentHandle, carrierString, timeout, &x0, &dx,
               signalPower[i], absoluteLimit[i], actualArraySize, NULL));
         }
         else
         {
            printf("malloc failed.\n");
            goto Error;
         }
      }
   }

   printf("------------------------Measurements------------------------\n\n");
   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      printf("Carrier  : %d\n", i);
      printf("Measurement Status                      : %s\n",
         measurementStatus[i] == RFMXNR_VAL_PVT_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
      printf("Mean Absolute OFF power Before (dBm)    : %lf\n", absoluteOFFPowerBefore[i]);
      printf("Mean Absolute OFF power After (dBm)     : %lf\n", absoluteOFFPowerAfter[i]);
      printf("Mean Absolute ON power (dBm)            : %lf\n", absoluteONPower[i]);
      printf("Burst Width (s)                         : %lf\n", burstWidth[i]);
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
       if (signalPower[i])
       {
           free(signalPower[i]);
       }
       if (absoluteLimit[i])
       {
           free(absoluteLimit[i]);
       }
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
