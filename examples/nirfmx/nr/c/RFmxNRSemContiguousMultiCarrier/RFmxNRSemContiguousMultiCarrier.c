//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Parameters for Digital Edge Trigger.
//6. Configure Link Direction, Frequency Range, Channel Raster and Component Carrier Spacing.
//7. Configure Bandwidth Part Subcarrier Spacing.
//8. Configure Component Carriers.
//9. Select SEM measurement and enable Traces.
//10. Configure Offsets.
//11. Configure Uplink Mask Type, or Downlink Mask, Band, gNodeB Category, Delta F_Max(Hz) and
//    Component Carrier Rated Output Power based on Link Direction.
//12. Configure Sweep Time Parameters.
//13. Configure Averaging Parameters for SEM measurement.
//14. Initiate the Measurement.
//15. Fetch SEM Measurements and Traces.
//16. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <string.h>

#include "niRFmxNR.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION               4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                 256

#define NUMBER_OF_COMPONENT_CARRIERS        2
#define NUMBER_OF_OFFSETS                   4

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char subblockString[MAX_SELECTOR_STRING];
   char carrierString[MAX_SELECTOR_STRING];

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0;

   char* selectedPorts = "";
   float64 centerFrequency = 3.5e9;                                                          /* (Hz) */
   float64 referenceLevel = 0.0;                                                             /* (dBm) */
   float64 externalAttenuation = 0.0;                                                        /* (dB) */

   char *frequencyReferenceSource = RFMXNR_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                                               /* (Hz) */

   int32 enableTrigger = RFMXNR_VAL_FALSE;
   char* digitalEdgeSource = RFMXNR_VAL_PXI_TRIG0_STR;
   int32 digitalEdge = RFMXNR_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                                               /* (s) */

   int32 linkDirection = RFMXNR_VAL_LINK_DIRECTION_UPLINK;

   int32 uplinkMaskType = RFMXNR_VAL_SEM_UPLINK_MASK_TYPE_GENERAL;

   int32 gNodeBCategory = RFMXNR_VAL_GNODEB_CATEGORY_WIDE_AREA_BASE_STATION_CATEGORY_A;
   int32 downlinkMaskType = RFMXNR_VAL_SEM_DOWNLINK_MASK_TYPE_STANDARD;
   float64 deltaFMaximum = 15.0e6;                                                           /* (Hz) */
   int32 band = 78;

   float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS] = { 100e6, 100e6 };       /* (Hz) */
   float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS] = { -49.98e6, 50.01e6 };  /* (Hz) */
   float64 componentCarrierRatedOutputPower[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0, 0.0 };    /* (dBm) */

   float64 offsetStartFrequency[NUMBER_OF_OFFSETS] = { 15.0e3, 1.5e6, 5.5e6, 40.3e6 };       /* (Hz) */
   float64 offsetStopFrequency[NUMBER_OF_OFFSETS] = { 985.0e3, 4.5e6, 39.3e6, 44.3e6 };      /* (Hz) */
   int32 offsetSideband[NUMBER_OF_OFFSETS] = { RFMXNR_VAL_SEM_OFFSET_SIDEBAND_BOTH,
      RFMXNR_VAL_SEM_OFFSET_SIDEBAND_BOTH,
      RFMXNR_VAL_SEM_OFFSET_SIDEBAND_BOTH,
      RFMXNR_VAL_SEM_OFFSET_SIDEBAND_BOTH };
   float64 offsetRBW[NUMBER_OF_OFFSETS] = { 10.0e3, 250.0e3, 1.0e6, 1.0e6 };                 /* (Hz) */
   int32 offsetRBWFilterType[NUMBER_OF_OFFSETS] = { RFMXNR_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN,
      RFMXNR_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN,
      RFMXNR_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN,
      RFMXNR_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN };
   int32 bandwidthIntegral[NUMBER_OF_OFFSETS] = { 3, 4, 1, 1 };
   int32 limitFailMask[NUMBER_OF_OFFSETS] = { RFMXNR_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE,
      RFMXNR_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE,
      RFMXNR_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE,
      RFMXNR_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE };
   float64 absoluteLimitStart[NUMBER_OF_OFFSETS] = { -22.5, -8.5, -11.5, -23.5 };            /* (dBm) */
   float64 absoluteLimitStop[NUMBER_OF_OFFSETS] = { -22.5, -8.5, -11.5, -23.5 };             /* (dBm) */
   float64 relativeLimitStart[NUMBER_OF_OFFSETS] = { -53.0, -53.0, -53.0, -53.0 };           /* (dB) */
   float64 relativeLimitStop[NUMBER_OF_OFFSETS] = { -60.0, -60.0, -60.0, -60.0 };            /* (dB) */

   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;

   int32 componentCarrierSpacingType = RFMXNR_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL;
   float64 channelRaster = 15e3;                                                             /* (Hz) */
   int32 componentCarrierAtCenterFrequency = -1;
   float64 subcarrierSpacing = 30e3;                                                         /* (Hz) */

   int32 sweepTimeAuto = RFMXNR_VAL_SEM_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 1.0e-3;                                                       /* (s) */

   int32 averagingEnabled = RFMXNR_VAL_SEM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXNR_VAL_SEM_AVERAGING_TYPE_RMS;

   float64 timeout = 10.0;                                                                   /* (s) */

   float64 totalAggregatedPower = 0.0;                                                       /* (dBm) */

   int32 measurementStatus = RFMXNR_VAL_SEM_MEASUREMENT_STATUS_FAIL;

   float64 *lowerOffsetMarginRelativePower = NULL;                                           /* (dB) */
   float64 *lowerOffsetMarginAbsolutePower = NULL;                                           /* (dBm) */
   float64 *lowerOffsetMargin = NULL;                                                        /* (dB) */
   float64 *lowerOffsetMarginFrequency = NULL;                                               /* (Hz) */
   int32 *lowerOffsetMeasurementStatus = NULL;

   float64 *upperOffsetMarginRelativePower = NULL;                                           /* (dB) */
   float64 *upperOffsetMarginAbsolutePower = NULL;                                           /* (dBm) */
   float64 *upperOffsetMargin = NULL;                                                        /* (dB) */
   float64 *upperOffsetMarginFrequency = NULL;                                               /* (Hz) */
   int32 *upperOffsetMeasurementStatus = NULL;

   int32 actualArraySize = 0, arraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = NULL;
   float32 *compositeMask = NULL;

   /* Initialize a session */
   RFmxCheckWarn(RFmxNR_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxNR_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxNR_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxNR_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxNR_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay,
      enableTrigger));

   RFmxCheckWarn(RFmxNR_SetLinkDirection(instrumentHandle, "", linkDirection));
   RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, "", frequencyRange));
   RFmxCheckWarn(RFmxNR_SetChannelRaster(instrumentHandle, "", channelRaster));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierSpacingType(instrumentHandle, "", componentCarrierSpacingType));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierAtCenterFrequency(instrumentHandle, "", componentCarrierAtCenterFrequency));

   strcpy_s(carrierString, sizeof("carrier::all"), "carrier::all");
   RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, carrierString, subcarrierSpacing));

   RFmxCheckWarn(RFmxNR_SetNumberOfComponentCarriers(instrumentHandle, "", NUMBER_OF_COMPONENT_CARRIERS));

   RFmxCheckWarn(RFmxNR_BuildSubblockString("", 0, MAX_SELECTOR_STRING, subblockString));
   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      RFmxCheckWarn(RFmxNR_BuildCarrierString(subblockString, i, MAX_SELECTOR_STRING, carrierString));
      RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, carrierString,
         componentCarrierBandwidth[i]));
      RFmxCheckWarn(RFmxNR_SetComponentCarrierFrequency(instrumentHandle, carrierString,
         componentCarrierFrequency[i]));
   }

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_SEM, RFMXNR_VAL_TRUE));

   RFmxCheckWarn(RFmxNR_SEMCfgNumberOfOffsets(instrumentHandle, "", NUMBER_OF_OFFSETS));
   RFmxCheckWarn(RFmxNR_SEMCfgOffsetFrequencyArray(instrumentHandle, "", offsetStartFrequency, offsetStopFrequency,
      offsetSideband, NUMBER_OF_OFFSETS));
   RFmxCheckWarn(RFmxNR_SEMCfgOffsetRBWFilterArray(instrumentHandle, "", offsetRBW, offsetRBWFilterType,
      NUMBER_OF_OFFSETS));
   RFmxCheckWarn(RFmxNR_SEMCfgOffsetBandwidthIntegralArray(instrumentHandle, "", bandwidthIntegral,
      NUMBER_OF_OFFSETS));
   RFmxCheckWarn(RFmxNR_SEMCfgOffsetLimitFailMaskArray(instrumentHandle, "", limitFailMask, NUMBER_OF_OFFSETS));
   RFmxCheckWarn(RFmxNR_SEMCfgOffsetAbsoluteLimitArray(instrumentHandle, "", absoluteLimitStart, absoluteLimitStop,
      NUMBER_OF_OFFSETS));
   RFmxCheckWarn(RFmxNR_SEMCfgOffsetRelativeLimitArray(instrumentHandle, "", relativeLimitStart, relativeLimitStop,
      NUMBER_OF_OFFSETS));

   if (linkDirection == RFMXNR_VAL_LINK_DIRECTION_UPLINK)
   {
      RFmxCheckWarn(RFmxNR_SEMCfgUplinkMaskType(instrumentHandle, "", uplinkMaskType));
   }
   else
   {
      RFmxCheckWarn(RFmxNR_CfggNodeBCategory(instrumentHandle, "", gNodeBCategory));
      RFmxCheckWarn(RFmxNR_SetBand(instrumentHandle, "", band));
      RFmxCheckWarn(RFmxNR_SEMSetDownlinkMaskType(instrumentHandle, "", downlinkMaskType));
      RFmxCheckWarn(RFmxNR_SEMSetDeltaFMaximum(instrumentHandle, "", deltaFMaximum));
      RFmxCheckWarn(RFmxNR_SEMCfgComponentCarrierRatedOutputPowerArray(instrumentHandle, "",
         componentCarrierRatedOutputPower, NUMBER_OF_COMPONENT_CARRIERS));
   }

   RFmxCheckWarn(RFmxNR_SEMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxNR_SEMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));

   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

   /* Fetch results */
   RFmxCheckWarn(RFmxNR_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL,
      NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      upperOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * actualArraySize);
      upperOffsetMargin = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperOffsetMarginRelativePower = (float64 *)malloc(sizeof(float64) * actualArraySize);

      if (upperOffsetMeasurementStatus && upperOffsetMargin && upperOffsetMarginFrequency &&
         upperOffsetMarginAbsolutePower && upperOffsetMarginRelativePower)
      {
         RFmxCheckWarn(RFmxNR_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout,
            upperOffsetMeasurementStatus, upperOffsetMargin, upperOffsetMarginFrequency,
            upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower, actualArraySize, &arraySize));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL,
      NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      lowerOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * actualArraySize);
      lowerOffsetMargin = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerOffsetMarginRelativePower = (float64 *)malloc(sizeof(float64) * actualArraySize);

      if (lowerOffsetMeasurementStatus && lowerOffsetMargin && lowerOffsetMarginFrequency &&
         lowerOffsetMarginAbsolutePower && lowerOffsetMarginRelativePower)
      {
         RFmxCheckWarn(RFmxNR_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout,
            lowerOffsetMeasurementStatus, lowerOffsetMargin, lowerOffsetMarginFrequency,
            lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxNR_SEMFetchTotalAggregatedPower(instrumentHandle, "", timeout, &totalAggregatedPower));

   RFmxCheckWarn(RFmxNR_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));

   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_SEMFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
      compositeMask = (float32 *)malloc(sizeof(float32) * actualArraySize);

      if (spectrum && compositeMask)
      {
         RFmxCheckWarn(RFmxNR_SEMFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, compositeMask,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("Total Aggregated Power (dBm)      : %lf\n", totalAggregatedPower);
   printf("Measurement Status                : %s\n",
      measurementStatus == RFMXNR_VAL_SEM_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");

   printf("\n--------  Lower Offset Segement Measurements --------\n");
   for (i = 0; i < arraySize; i++)
   {
      printf("\nOffset  %d\n", i);
      printf("Measurement Status                : %s\n",
         lowerOffsetMeasurementStatus[i] == RFMXNR_VAL_SEM_LOWER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
      printf("Margin (dB)                       : %lf\n", lowerOffsetMargin[i]);
      printf("Margin Frequency (Hz)             : %lf\n", lowerOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)       : %lf\n", lowerOffsetMarginAbsolutePower[i]);
   }

   printf("\n--------  Upper  Offset Segement Measurements --------\n");
   for (i = 0; i < arraySize; i++)
   {
      printf("\nOffset  %d\n", i);
      printf("Measurement Status                : %s\n",
         upperOffsetMeasurementStatus[i] == RFMXNR_VAL_SEM_UPPER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
      printf("Margin (dB)                       : %lf\n", upperOffsetMargin[i]);
      printf("Margin Frequency (Hz)             : %lf\n", upperOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)       : %lf\n", upperOffsetMarginAbsolutePower[i]);
   }

Error:
   if (error)
   {
      RFmxNR_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("\nERROR: %s\n", errorMessage);
      else
         printf("\nWARNING: %s\n", errorMessage);
   }

   if (instrumentHandle)
   {
      RFmxNR_Close(instrumentHandle, RFMXNR_VAL_FALSE);
   }

   if (lowerOffsetMeasurementStatus)
      free(lowerOffsetMeasurementStatus);
   if (lowerOffsetMarginAbsolutePower)
      free(lowerOffsetMarginAbsolutePower);
   if (lowerOffsetMarginFrequency)
      free(lowerOffsetMarginFrequency);
   if (lowerOffsetMarginRelativePower)
      free(lowerOffsetMarginRelativePower);
   if (lowerOffsetMargin)
      free(lowerOffsetMargin);
   if (upperOffsetMeasurementStatus)
      free(upperOffsetMeasurementStatus);
   if (upperOffsetMarginAbsolutePower)
      free(upperOffsetMarginAbsolutePower);
   if (upperOffsetMarginFrequency)
      free(upperOffsetMarginFrequency);
   if (upperOffsetMarginRelativePower)
      free(upperOffsetMarginRelativePower);
   if (upperOffsetMargin)
      free(upperOffsetMargin);
   if (spectrum)
      free(spectrum);
   if (compositeMask)
      free(compositeMask);

   printf("\nPress any key to exit\n");
   _getch();

   return error;
}
