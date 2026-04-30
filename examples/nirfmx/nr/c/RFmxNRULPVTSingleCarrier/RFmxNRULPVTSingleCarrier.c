//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Parameters for IQ Power Edge Trigger.
//6. Configure Frequency Range, Carrier Bandwidth, Cell ID and Subcarrier Spacing.
//7. Configure PUSCH and PUSCH RB Allocation.
//8. Configure PUSCH DMRS.
//9. Select PVT measurement and enable Traces.
//10. Configure Measurement Methods.
//11. Configure OFF Power Exclusion Periods.
//12. Configure Averaging Parameters for PVT measurement.
//13. Initiate the Measurement.
//14. Fetch PVT Traces and Measurements.
//15. Close RFmx Session.

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

   float64 IQPowerEdgeLevel = -20.0;                                                      /* (dB) */
   float64 triggerDelay = 0.0;                                                            /* (s) */
   int32 minimumQuietTimeMode = RFMXNR_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 8.0e-6;                                                     /* (s) */

   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;
   int32 cellID = 0;
   float64 carrierBandwidth = 100e6;                                                      /* (Hz) */
   float64 subcarrierSpacing = 30e3;                                                      /* (Hz) */

   int32 PUSCHTransformPrecodingEnabled = RFMXNR_VAL_PUSCH_TRANSFORM_PRECODING_ENABLED_FALSE;
   int32 PUSCHModulationType = RFMXNR_VAL_PUSCH_MODULATION_TYPE_QPSK;
   int32 PUSCHResourceBlockOffset[NUMBER_OF_RESOURCE_BLOCK_CLUSTERS] = { 0 };
   int32 PUSCHNumberOfResourceBlocks[NUMBER_OF_RESOURCE_BLOCK_CLUSTERS] = { -1 };
   char* PUSCHSlotAllocation = "1";
   char* PUSCHSymbolAllocation = "0-Last";

   int32 PUSCHDMRSPowerMode = RFMXNR_VAL_PUSCH_DMRS_POWER_MODE_CDM_GROUPS;
   float64 PUSCHDMRSPower = 0.0;                                                          /* (dB) */
   int32 PUSCHDMRSConfigurationType = RFMXNR_VAL_PUSCH_DMRS_CONFIGURATION_TYPE_TYPE1;
   int32 PUSCHMappingType = RFMXNR_VAL_PUSCH_MAPPING_TYPE_TYPE_A;
   int32 PUSCHDMRSTypeAPosition = 2;
   int32 PUSCHDMRSDuration = RFMXNR_VAL_PUSCH_DMRS_DURATION_SINGLE_SYMBOL;
   int32 PUSCHDMRSAdditionalPositions = 0;

   int32 measurementMethod = RFMXNR_VAL_PVT_MEASUREMENT_METHOD_NORMAL;
   float64 OFFPowerExclusionBefore = 0.0;                                                 /* (s) */
   float64 OFFPowerExclusionAfter = 0.0;                                                  /* (s) */

   int32 averagingEnabled = RFMXNR_VAL_PVT_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXNR_VAL_PVT_AVERAGING_TYPE_RMS;

   float64 timeout = 10.0;                                                                /* (s) */

   int32 measurementStatus;
   float64 absoluteOFFPowerBefore;                                                        /* (dBm) */
   float64 absoluteOFFPowerAfter;                                                         /* (dBm) */
   float64 absoluteONPower;                                                               /* (dBm) */
   float64 burstWidth;                                                                    /* (s) */

   int32 actualArraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32* signalPower = NULL;                                                           /* (dBm) */
   float32* absoluteLimit = NULL;                                                         /* (dBm) */

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
   RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, "", carrierBandwidth));
   RFmxCheckWarn(RFmxNR_SetCellID(instrumentHandle, "", cellID));
   RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, "", subcarrierSpacing));

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

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_PVT, RFMXNR_VAL_TRUE));

   RFmxCheckWarn(RFmxNR_PVTCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
   RFmxCheckWarn(RFmxNR_PVTCfgOFFPowerExclusionPeriods(instrumentHandle, "", OFFPowerExclusionBefore,
      OFFPowerExclusionAfter));
   RFmxCheckWarn(RFmxNR_PVTCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));

   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

   /* Fetch results */
   RFmxCheckWarn(RFmxNR_PVTFetchMeasurement(instrumentHandle, "", timeout, &measurementStatus, &absoluteOFFPowerBefore,
      &absoluteOFFPowerAfter, &absoluteONPower, &burstWidth));

   RFmxCheckWarn(RFmxNR_PVTFetchSignalPowerTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL,
      0, &actualArraySize));
   if (actualArraySize > 0)
   {
      signalPower = (float32*)malloc(sizeof(float32) * actualArraySize);
      absoluteLimit = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (signalPower && absoluteLimit)
      {
         RFmxCheckWarn(RFmxNR_PVTFetchSignalPowerTrace(instrumentHandle, "", timeout, &x0, &dx,
            signalPower, absoluteLimit, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("------------------Measurement------------------\n");
   printf("Measurement Status                      : %s\n",
      measurementStatus == RFMXNR_VAL_PVT_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
   printf("Mean Absolute OFF power Before (dBm)    : %lf\n", absoluteOFFPowerBefore);
   printf("Mean Absolute OFF power After (dBm)     : %lf\n", absoluteOFFPowerAfter);
   printf("Mean Absolute ON power (dBm)            : %lf\n", absoluteONPower);
   printf("Burst Width (s)                         : %lf\n\n", burstWidth);

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
   if (signalPower)
   {
      free(signalPower);
   }
   if (absoluteLimit)
   {
      free(absoluteLimit);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
