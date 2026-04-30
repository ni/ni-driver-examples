//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Parameters for Digitial Edge Trigger.
//6. Configure Number of Subblocks and Link Direction.
//7. Configure Frequency Range, Center Frequency, Component Carrier Spacing, Channel Raster,
//   Component Carrier Center Frequency, Subblock Frequency and Number of Component Carriers.
//8. Configure Component Carriers.
//9. Configure Bandwidth Part Subcarrier Spacing.
//10. Select SEM measurement and enable Traces.
//11. Configure Uplink Mask Type, or Downlink Mask, Band, gNodeB Category and Delta F_Max(Hz) based on Link Direction.
//12. Configure Component Carrier Rated Output Power based on Link Direction.
//13. Configure Offsets.
//14. Configure Sweep Time Parameters.
//15. Configure Averaging Parameters for SEM measurement.
//16. Initiate the Measurement.
//17. Fetch SEM Measurements and Traces.
//18. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxNR.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                  256

#define NUMBER_OF_SUBBLOCKS                  2
#define NUMBER_OF_COMPONENT_CARRIERS         2
#define NUMBER_OF_OFFSETS                    4

/* Input: Subblock inputs structure */
typedef struct
{
   float64 subblockFrequency;                                                                /* (Hz) */
   int32 componentCarrierSpacingType;
   float64 channelRaster;                                                                    /* (Hz) */
   int32 componentCarrierAtCenterFrequency;
   float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS];                          /* (Hz) */
   float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS];                          /* (Hz) */
   float64 componentCarrierRatedOutputPower[NUMBER_OF_COMPONENT_CARRIERS];                   /* (dBm) */
   float64 offsetStartFrequency[NUMBER_OF_OFFSETS];                                          /* (Hz) */
   float64 offsetStopFrequency[NUMBER_OF_OFFSETS];                                           /* (Hz) */
   int32 offsetSideband[NUMBER_OF_OFFSETS];
   float64 offsetRBW[NUMBER_OF_OFFSETS];                                                     /* (Hz) */
   int32 offsetRBWFilterType[NUMBER_OF_OFFSETS];
   int32 bandwidthIntegral[NUMBER_OF_OFFSETS];
   int32 limitFailMask[NUMBER_OF_OFFSETS];
   float64 absoluteLimitStart[NUMBER_OF_OFFSETS];                                            /* (dBm) */
   float64 absoluteLimitStop[NUMBER_OF_OFFSETS];                                             /* (dBm) */
   float64 relativeLimitStart[NUMBER_OF_OFFSETS];                                            /* (dB) */
   float64 relativeLimitStop[NUMBER_OF_OFFSETS];                                             /* (dB) */
}subblockInputs_t;

/* Input: Subblock measurement outputs structure */
typedef struct
{
   float64 subblockPower;                                                                    /* (dBm) */
   float64 integrationBandwidth;                                                             /* (Hz) */
   float64 subblockFrequency;                                                                /* (Hz) */

   float64* lowerOffsetMarginRelativePower;                                                  /* (dB) */
   float64* lowerOffsetMarginAbsolutePower;                                                  /* (dBm) */
   float64* lowerOffsetMargin;
   float64* lowerOffsetMarginFrequency;                                                      /* (Hz) */
   int32* lowerOffsetMeasurementStatus;

   float64* upperOffsetMarginRelativePower;                                                  /* (dB) */
   float64* upperOffsetMarginAbsolutePower;                                                  /* (dBm) */
   float64* upperOffsetMargin;
   float64* upperOffsetMarginFrequency;                                                      /* (Hz) */
   int32* upperOffsetMeasurementStatus;

}subblockMeasurements_t;

int main(int argc, char* argv[])
{
   char* resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0, j = 0;

   char subblockString[MAX_SELECTOR_STRING];
   char carrierString[MAX_SELECTOR_STRING];

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

   int32 linkDirection = RFMXNR_VAL_LINK_DIRECTION_UPLINK;

   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;

   int32 uplinkMaskType = RFMXNR_VAL_SEM_UPLINK_MASK_TYPE_GENERAL;

   int32 gNodeBCategory = RFMXNR_VAL_GNODEB_CATEGORY_WIDE_AREA_BASE_STATION_CATEGORY_A;
   int32 downlinkMaskType = RFMXNR_VAL_SEM_DOWNLINK_MASK_TYPE_STANDARD;
   float64 deltaFMaximum = 15.0e6;                                                           /* (Hz) */
   int32 band = 78;

   float64 subcarrierSpacing = 30e3;                                                         /* (Hz) */

   int32 sweepTimeAuto = RFMXNR_VAL_SEM_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 1.0e-3;                                                       /* (s) */

   int32 averagingEnabled = RFMXNR_VAL_SEM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXNR_VAL_SEM_AVERAGING_TYPE_RMS;

   subblockInputs_t    subblocks[NUMBER_OF_SUBBLOCKS] = {                                    /* Set up subblock 0 inputs */
      {
         0.0,													                             /* subblockFrequency */
         RFMXNR_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL,
         15e3,
         -1,                                                                                 /* componentCarrierAtCenterFrequency */
         { 100e6, 100e6},                                                                    /* componentCarrierBandwidth */
         { -49.98e6, 50.01e6 },                                                              /* componentCarrierFrequency */
         { 0.0, 0.0 },                                                                       /* componentCarrierRatedOutputPower */
         { 15.0e3, 1.5e6, 5.5e6, 20.5e6 },                                                   /* offsetStartFrequency */
         { 985.0e3, 4.5e6, 19.5e6, 24.5e6 },                                                 /* offsetStopFrequency */
         { RFMXNR_VAL_SEM_OFFSET_SIDEBAND_BOTH,
           RFMXNR_VAL_SEM_OFFSET_SIDEBAND_BOTH,
           RFMXNR_VAL_SEM_OFFSET_SIDEBAND_BOTH,
           RFMXNR_VAL_SEM_OFFSET_SIDEBAND_BOTH },
         { 10.0e3, 250.0e3, 250.0e3, 250.0e3 },                                              /* offsetRBW */
         { RFMXNR_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN,
           RFMXNR_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN,
           RFMXNR_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN,
           RFMXNR_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN },
         { 3, 4, 4, 4 },                                                                     /* bandwidthIntegral */
         { RFMXNR_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE,
           RFMXNR_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE,
           RFMXNR_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE,
           RFMXNR_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE },
         { -22.5, -8.5, -11.5, -23.5 },                                                      /* absoluteLimitStart */
         { -22.5, -8.5, -11.5, -23.5 },                                                      /* absoluteLimitStop */
         { -53.0, -53.0, -53.0, -53.0 },                                                     /* relativeLimitStart */
         { -60.0, -60.0, -60.0, -60.0 }                                                      /* relativeLimitStop */
      },
      {                                                                                      /* Set up subblock 1 inputs */
         200e6,													                             /* subblockFrequency */
         RFMXNR_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL,
         15e3,
         -1,                                                                                 /* componentCarrierAtCenterFrequency */
         { 100e6 , 100e6  },                                                                 /* componentCarrierBandwidth */
         { -49.98e6 , 50.01e6},                                                              /* componentCarrierFrequency */
         { 0.0 , 0.0 },                                                                      /* componentCarrierRatedOutputPower */
         { 15.0e3, 1.5e6, 5.5e6, 20.5e6 },                                                   /* offsetStartFrequency */
         { 985.0e3, 4.5e6, 19.5e6, 24.5e6 },                                                 /* offsetStopFrequency */
         { RFMXNR_VAL_SEM_OFFSET_SIDEBAND_BOTH,
           RFMXNR_VAL_SEM_OFFSET_SIDEBAND_BOTH,
           RFMXNR_VAL_SEM_OFFSET_SIDEBAND_BOTH,
           RFMXNR_VAL_SEM_OFFSET_SIDEBAND_BOTH },
         { 10.0e3, 250.0e3, 250.0e3, 250.0e3 },                                              /* offsetRBW */
         { RFMXNR_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN,
           RFMXNR_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN,
           RFMXNR_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN,
           RFMXNR_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN },
         { 3, 4, 4, 4 },                                                                     /* bandwidthIntegral*/
         { RFMXNR_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE,
           RFMXNR_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE,
           RFMXNR_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE,
           RFMXNR_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE },
         { -22.5, -8.5, -11.5, -23.5 },                                                      /* absoluteLimitStart */
         { -22.5, -8.5, -11.5, -23.5 },                                                      /* absoluteLimitStop */
         { -53.0, -53.0, -53.0, -51.5 },                                                     /* relativeLimitStart */
         { -60.0, -60.0, -60.0, -58.5 }                                                      /* relativeLimitStop */
      }
   };

   float64 totalAggregatedPower = 0.0;                                                       /* (dBm) */

   int32 measurementStatus = RFMXNR_VAL_SEM_MEASUREMENT_STATUS_FAIL;

   float64 timeout = 10.000000;                                                              /* (s) */

   subblockMeasurements_t subblocksMsr[NUMBER_OF_SUBBLOCKS];

   int32 actualArraySize = 0, arraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32* spectrum = NULL;
   float32* absoluteMask = NULL;
   int32 offSetArraySize = 0;

   /* Set up subblock outputs */
   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      subblocksMsr[i].lowerOffsetMarginRelativePower = NULL;
      subblocksMsr[i].lowerOffsetMarginAbsolutePower = NULL;
      subblocksMsr[i].lowerOffsetMargin = NULL;
      subblocksMsr[i].lowerOffsetMarginFrequency = NULL;
      subblocksMsr[i].lowerOffsetMeasurementStatus = NULL;

      subblocksMsr[i].upperOffsetMarginRelativePower = NULL;
      subblocksMsr[i].upperOffsetMarginAbsolutePower = NULL;
      subblocksMsr[i].upperOffsetMargin = NULL;
      subblocksMsr[i].upperOffsetMarginFrequency = NULL;
      subblocksMsr[i].upperOffsetMeasurementStatus = NULL;
   }

   /* Initialize a session */
   RFmxCheckWarn(RFmxNR_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxNR_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxNR_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxNR_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxNR_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge,
      triggerDelay, enableTrigger));

   RFmxCheckWarn(RFmxNR_SetNumberOfSubblocks(instrumentHandle, "", NUMBER_OF_SUBBLOCKS));
   RFmxCheckWarn(RFmxNR_SetLinkDirection(instrumentHandle, "", linkDirection));

   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      RFmxNR_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString);
      RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, subblockString, frequencyRange));
      RFmxCheckWarn(RFmxNR_SetComponentCarrierSpacingType(instrumentHandle, subblockString,
         subblocks[i].componentCarrierSpacingType));
      RFmxCheckWarn(RFmxNR_SetChannelRaster(instrumentHandle, subblockString, subblocks[i].channelRaster));
      RFmxCheckWarn(RFmxNR_SetComponentCarrierAtCenterFrequency(instrumentHandle, subblockString,
         subblocks[i].componentCarrierAtCenterFrequency));
      RFmxCheckWarn(RFmxNR_SetSubblockFrequency(instrumentHandle, subblockString,
         subblocks[i].subblockFrequency));
      RFmxCheckWarn(RFmxNR_SetNumberOfComponentCarriers(instrumentHandle, subblockString, NUMBER_OF_COMPONENT_CARRIERS));

      for (j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++)
      {
         RFmxNR_BuildCarrierString(subblockString, j, MAX_SELECTOR_STRING, carrierString);
         RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, carrierString,
            subblocks[i].componentCarrierBandwidth[j]));
         RFmxCheckWarn(RFmxNR_SetComponentCarrierFrequency(instrumentHandle, carrierString,
            subblocks[i].componentCarrierFrequency[j]));
      }

      RFmxNR_BuildCarrierString(subblockString, -1, MAX_SELECTOR_STRING, carrierString);
      RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, carrierString, subcarrierSpacing));
   }

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_SEM, RFMXNR_VAL_TRUE));

   if (linkDirection == RFMXNR_VAL_LINK_DIRECTION_UPLINK)
   {
      RFmxCheckWarn(RFmxNR_SEMCfgUplinkMaskType(instrumentHandle, "", uplinkMaskType));
   }
   else
   {
      RFmxCheckWarn(RFmxNR_CfggNodeBCategory(instrumentHandle, "", gNodeBCategory));
      RFmxCheckWarn(RFmxNR_SEMSetDownlinkMaskType(instrumentHandle, "", downlinkMaskType));
      RFmxCheckWarn(RFmxNR_SEMSetDeltaFMaximum(instrumentHandle, "", deltaFMaximum));
      RFmxNR_BuildSubblockString("", -1, MAX_SELECTOR_STRING, subblockString);
      RFmxCheckWarn(RFmxNR_SetBand(instrumentHandle, subblockString, band));
   }

   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      RFmxNR_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString);
      if (linkDirection == RFMXNR_VAL_LINK_DIRECTION_DOWNLINK)
      {
         RFmxCheckWarn(RFmxNR_SEMCfgComponentCarrierRatedOutputPowerArray(instrumentHandle, subblockString,
            subblocks[i].componentCarrierRatedOutputPower, NUMBER_OF_COMPONENT_CARRIERS));
      }
      RFmxCheckWarn(RFmxNR_SEMCfgNumberOfOffsets(instrumentHandle, subblockString, NUMBER_OF_OFFSETS));
      RFmxCheckWarn(RFmxNR_SEMCfgOffsetFrequencyArray(instrumentHandle, subblockString, subblocks[i].offsetStartFrequency,
         subblocks[i].offsetStopFrequency, subblocks[i].offsetSideband, NUMBER_OF_OFFSETS));
      RFmxCheckWarn(RFmxNR_SEMCfgOffsetRBWFilterArray(instrumentHandle, subblockString, subblocks[i].offsetRBW,
         subblocks[i].offsetRBWFilterType, NUMBER_OF_OFFSETS));
      RFmxCheckWarn(RFmxNR_SEMCfgOffsetBandwidthIntegralArray(instrumentHandle, subblockString, subblocks[i].bandwidthIntegral,
         NUMBER_OF_OFFSETS));
      RFmxCheckWarn(RFmxNR_SEMCfgOffsetLimitFailMaskArray(instrumentHandle, subblockString, subblocks[i].limitFailMask,
         NUMBER_OF_OFFSETS));
      RFmxCheckWarn(RFmxNR_SEMCfgOffsetAbsoluteLimitArray(instrumentHandle, subblockString, subblocks[i].absoluteLimitStart,
         subblocks[i].absoluteLimitStop, NUMBER_OF_OFFSETS));
      RFmxCheckWarn(RFmxNR_SEMCfgOffsetRelativeLimitArray(instrumentHandle, subblockString, subblocks[i].relativeLimitStart,
         subblocks[i].relativeLimitStop, NUMBER_OF_OFFSETS));
   }

   RFmxCheckWarn(RFmxNR_SEMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxNR_SEMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));

   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));


   /* Fetch results */
   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      RFmxCheckWarn(RFmxNR_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString));

      RFmxCheckWarn(RFmxNR_SEMFetchUpperOffsetMarginArray(instrumentHandle, subblockString, timeout, NULL,
         NULL, NULL, NULL, NULL, 0, &actualArraySize));
      if (actualArraySize > 0)
      {
         subblocksMsr[i].upperOffsetMeasurementStatus = (int32*)malloc(sizeof(int32) * actualArraySize);
         subblocksMsr[i].upperOffsetMargin = (float64*)malloc(sizeof(float64) * actualArraySize);
         subblocksMsr[i].upperOffsetMarginFrequency = (float64*)malloc(sizeof(float64) * actualArraySize);
         subblocksMsr[i].upperOffsetMarginAbsolutePower = (float64*)malloc(sizeof(float64) * actualArraySize);
         subblocksMsr[i].upperOffsetMarginRelativePower = (float64*)malloc(sizeof(float64) * actualArraySize);

         RFmxCheckWarn(RFmxNR_SEMFetchUpperOffsetMarginArray(instrumentHandle, subblockString, timeout,
            subblocksMsr[i].upperOffsetMeasurementStatus, subblocksMsr[i].upperOffsetMargin,
            subblocksMsr[i].upperOffsetMarginFrequency, subblocksMsr[i].upperOffsetMarginAbsolutePower,
            subblocksMsr[i].upperOffsetMarginRelativePower, actualArraySize, &arraySize));
      }

      actualArraySize = 0;
      RFmxCheckWarn(RFmxNR_SEMFetchLowerOffsetMarginArray(instrumentHandle, subblockString, timeout, NULL,
         NULL, NULL, NULL, NULL, 0, &actualArraySize));
      if (actualArraySize > 0)
      {
         subblocksMsr[i].lowerOffsetMeasurementStatus = (int32*)malloc(sizeof(int32) * actualArraySize);
         subblocksMsr[i].lowerOffsetMargin = (float64*)malloc(sizeof(float64) * actualArraySize);
         subblocksMsr[i].lowerOffsetMarginFrequency = (float64*)malloc(sizeof(float64) * actualArraySize);
         subblocksMsr[i].lowerOffsetMarginAbsolutePower = (float64*)malloc(sizeof(float64) * actualArraySize);
         subblocksMsr[i].lowerOffsetMarginRelativePower = (float64*)malloc(sizeof(float64) * actualArraySize);

         RFmxCheckWarn(RFmxNR_SEMFetchLowerOffsetMarginArray(instrumentHandle, subblockString, timeout,
            subblocksMsr[i].lowerOffsetMeasurementStatus, subblocksMsr[i].lowerOffsetMargin,
            subblocksMsr[i].lowerOffsetMarginFrequency, subblocksMsr[i].lowerOffsetMarginAbsolutePower,
            subblocksMsr[i].lowerOffsetMarginRelativePower, actualArraySize, &arraySize));
      }

      RFmxCheckWarn(RFmxNR_SEMFetchSubblockMeasurement(instrumentHandle, subblockString, timeout,
         &subblocksMsr[i].subblockPower, &subblocksMsr[i].integrationBandwidth, &subblocksMsr[i].subblockFrequency));
   }
   offSetArraySize = actualArraySize;

   RFmxCheckWarn(RFmxNR_SEMFetchTotalAggregatedPower(instrumentHandle, "", timeout, &totalAggregatedPower));
   RFmxCheckWarn(RFmxNR_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));

   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_SEMFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32*)malloc(sizeof(float32) * actualArraySize);
      absoluteMask = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (spectrum)
      {
         RFmxCheckWarn(RFmxNR_SEMFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, absoluteMask,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("Total Aggregated Power (dBm)     : %lf\n", totalAggregatedPower);
   printf("Measurement Status               : %s\n", measurementStatus == RFMXNR_VAL_SEM_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");

   printf("\n--------------------Subblock Measurements--------------------\n");
   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      printf("\nSubblock %d\n", i);

      printf("Subblock Power (dBm)             : %lf\n", subblocksMsr[i].subblockPower);
      printf("Integration Bandwidth (Hz)       : %lf\n", subblocksMsr[i].integrationBandwidth);
      printf("Frequency (Hz)                   : %lf\n", subblocksMsr[i].subblockFrequency);

      printf("\n Offset Segment Measurements \n");
      for (j = 0; j < offSetArraySize; j++)
      {
         printf("\nLower Offset Segment Measurement %d\n", j);
         printf("Measurement Status               : %s\n",
            subblocksMsr[i].lowerOffsetMeasurementStatus[j] == RFMXNR_VAL_SEM_UPPER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
         printf("Margin(dBm)                      : %lf\n", subblocksMsr[i].lowerOffsetMargin[j]);
         printf("Margin Frequency(Hz)             : %lf\n", subblocksMsr[i].lowerOffsetMarginFrequency[j]);
         printf("Margin Absolute Power(Hz)        : %lf\n", subblocksMsr[i].lowerOffsetMarginAbsolutePower[j]);

         printf("\nUpper Offset Segment Measurement %d\n", j);
         printf("Measurement Status               : %s\n",
            subblocksMsr[i].upperOffsetMeasurementStatus[j] == RFMXNR_VAL_SEM_LOWER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
         printf("Margin(dBm)                      : %lf\n", subblocksMsr[i].upperOffsetMargin[j]);
         printf("Margin Frequency(Hz)             : %lf\n", subblocksMsr[i].upperOffsetMarginFrequency[j]);
         printf("Margin Absolute Power(Hz)        : %lf\n", subblocksMsr[i].upperOffsetMarginAbsolutePower[j]);

      }
      printf("\n------------------------------------------------\n");
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
   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
       if (subblocksMsr[i].lowerOffsetMeasurementStatus)
       {
           free(subblocksMsr[i].lowerOffsetMeasurementStatus);
       }
       if (subblocksMsr[i].lowerOffsetMargin)
       {
           free(subblocksMsr[i].lowerOffsetMargin);
       }
       if (subblocksMsr[i].lowerOffsetMarginFrequency)
       {
           free(subblocksMsr[i].lowerOffsetMarginFrequency);
       }
       if (subblocksMsr[i].lowerOffsetMarginAbsolutePower)
       {
           free(subblocksMsr[i].lowerOffsetMarginAbsolutePower);
       }
       if (subblocksMsr[i].lowerOffsetMarginRelativePower)
       {
           free(subblocksMsr[i].lowerOffsetMarginRelativePower);
       }

       if (subblocksMsr[i].upperOffsetMeasurementStatus)
       {
           free(subblocksMsr[i].upperOffsetMeasurementStatus);
       }
       if (subblocksMsr[i].upperOffsetMargin)
       {
           free(subblocksMsr[i].upperOffsetMargin);
       }
       if (subblocksMsr[i].upperOffsetMarginFrequency)
       {
           free(subblocksMsr[i].upperOffsetMarginFrequency);
       }
       if (subblocksMsr[i].upperOffsetMarginAbsolutePower)
       {
           free(subblocksMsr[i].upperOffsetMarginAbsolutePower);
       }
       if (subblocksMsr[i].upperOffsetMarginRelativePower)
       {
           free(subblocksMsr[i].upperOffsetMarginRelativePower);
       }
   }

   if (spectrum)
   {
       free(spectrum);
   }
   if (absoluteMask)
   {
       free(absoluteMask);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
