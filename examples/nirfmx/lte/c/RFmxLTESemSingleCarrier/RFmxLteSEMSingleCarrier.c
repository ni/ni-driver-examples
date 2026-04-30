//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Configure Link Direction.
//7. Select SEM measurement and enable Traces.
//8. Configure Sweep Time Parameters.
//9. Configure Averaging Parameters for SEM measurement.
//10. Configure Uplink Mask Type, or Downlink Mask, eNodeB Category and
//    Component Carrier Maximum Output Power depending on Link Direction.
//11. Initiate the Measurement.
//12. Fetch SEM Measurements and Traces.
//13. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION               4096

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };

   int32 error = 0, lastErrorCode = 0;
   int i = 0;

   float64 centerFrequency = 1.95e9;                   /* (Hz) */
   float64 referenceLevel = 0.0;                       /* (dBm) */
   float64 externalAttenuation = 0.0;                  /* (dB) */

   char *frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e6;       /* (Hz) */

   int32 enableTrigger = RFMXLTE_VAL_FALSE;
   char *digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
   int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                         /* (s) */

   int32 linkDirection = RFMXLTE_VAL_LINK_DIRECTION_UPLINK;

   /* Uplink */
   int32 uplinkMaskType = RFMXLTE_VAL_SEM_UPLINK_MASK_TYPE_GENERAL_NS01;

   /* Downlink */
   int32 eNodeBCategory = RFMXLTE_VAL_ENODEB_WIDE_AREA_BASE_STATION_CATEGORY_A;
   int32 downlinkMaskType = RFMXLTE_VAL_SEM_DOWNLINK_MASK_TYPE_ENODEB_CATEGORY_BASED;
   float64 deltaFMaximum = 15.0e6;                     /* (Hz) */
   float64 aggregatedMaximumPower = 0.0;               /* (dBm) */
   float64 maximumOutputPower = 0.0;                   /* (dBm) */

   /* Sidelink */
   int32 sidelinkMaskType = RFMXLTE_VAL_SEM_SIDELINK_MASK_TYPE_GENERAL_NS01;

   float64 componentCarrierBandwidth = 10.0e6;         /* (Hz) */

   int32 sweepTimeAuto = RFMXLTE_VAL_SEM_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 0.001;                  /* (s) */

   int32 averagingEnabled = RFMXLTE_VAL_SEM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXLTE_VAL_SEM_AVERAGING_TYPE_RMS;

   float64 timeout = 10.0;                             /* (s) */

   float64 *lowerOffsetMarginRelativePower = NULL;     /* (dBm) */
   float64 *lowerOffsetMarginAbsolutePower = NULL;     /* (dBm) */
   float64 *lowerOffsetMargin = NULL;
   float64 *lowerOffsetMarginFrequency = NULL;         /* (Hz) */
   int32 *lowerOffsetMeasurementStatus = NULL;

   float64 *upperOffsetMarginRelativePower = NULL;     /* (dBm) */
   float64 *upperOffsetMarginAbsolutePower = NULL;     /* (dBm) */
   float64 *upperOffsetMargin = NULL;
   float64 *upperOffsetMarginFrequency = NULL;         /* (Hz) */
   int32 *upperOffsetMeasurementStatus = NULL;

   int32 measurementStatus;

   float64 absoluteIntegratedPower;
   float64 relativeIntegratedPower;

   int32 actualArraySize = 0, arraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = NULL;                           /* (dBm) */

   float32 *absoluteMask = NULL;

   /* Initialize a session */
   RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge,
      triggerDelay, enableTrigger));
   RFmxCheckWarn(RFmxLTE_CfgComponentCarrier(instrumentHandle, "", componentCarrierBandwidth, 0.0, 0));
   RFmxCheckWarn(RFmxLTE_CfgLinkDirection(instrumentHandle, "", linkDirection));
   RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_SEM, RFMXLTE_VAL_TRUE));
   RFmxCheckWarn(RFmxLTE_SEMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxLTE_SEMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   if (linkDirection == RFMXLTE_VAL_LINK_DIRECTION_UPLINK)
      RFmxCheckWarn(RFmxLTE_SEMCfgUplinkMaskType(instrumentHandle, "", uplinkMaskType));
   else if (linkDirection == RFMXLTE_VAL_LINK_DIRECTION_DOWNLINK)
   {
      RFmxCheckWarn(RFmxLTE_CfgeNodeBCategory(instrumentHandle, "", eNodeBCategory));
      RFmxCheckWarn(RFmxLTE_SEMCfgDownlinkMask(instrumentHandle, "", downlinkMaskType, deltaFMaximum,
         aggregatedMaximumPower));
      RFmxCheckWarn(RFmxLTE_SEMCfgComponentCarrierMaximumOutputPower(instrumentHandle, "", maximumOutputPower));
   }
   else
      RFmxCheckWarn(RFmxLTE_SEMSetSidelinkMaskType(instrumentHandle, "", sidelinkMaskType));
   RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

   /* Retrive Results*/
   RFmxCheckWarn(RFmxLTE_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL,
      NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      upperOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * actualArraySize);
      upperOffsetMargin = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperOffsetMarginRelativePower = (float64 *)malloc(sizeof(float64) * actualArraySize);

      RFmxCheckWarn(RFmxLTE_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, upperOffsetMeasurementStatus,
         upperOffsetMargin, upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower,
         upperOffsetMarginRelativePower, actualArraySize, &arraySize));
   }

   RFmxCheckWarn(RFmxLTE_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL,
      NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      lowerOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * actualArraySize);
      lowerOffsetMargin = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerOffsetMarginRelativePower = (float64 *)malloc(sizeof(float64) * actualArraySize);

      RFmxCheckWarn(RFmxLTE_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, lowerOffsetMeasurementStatus,
         lowerOffsetMargin, lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower,
         lowerOffsetMarginRelativePower, actualArraySize, NULL));
   }

   RFmxCheckWarn(RFmxLTE_SEMFetchComponentCarrierMeasurement(instrumentHandle, "", timeout,
      &absoluteIntegratedPower, &relativeIntegratedPower));
   RFmxCheckWarn(RFmxLTE_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));

   RFmxCheckWarn(RFmxLTE_SEMFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
      absoluteMask = (float32 *)malloc(sizeof(float32) * actualArraySize);
      if (spectrum)
      {
         RFmxCheckWarn(RFmxLTE_SEMFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, absoluteMask,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("Measurement Status           : %s\n",
      measurementStatus == RFMXLTE_VAL_SEM_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
   printf("Carrier Absolute Power (dBm) : %lf\n", absoluteIntegratedPower);

   printf("\n----------Lower Offset Segment Measurements----------\n\n");

   for (i = 0; i < arraySize; i++)
   {
      printf("Offset %d\n", i);
      printf("Measurement Status           : %s\n",
         lowerOffsetMeasurementStatus[i] == RFMXLTE_VAL_SEM_LOWER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
      printf("Margin (dB)                  : %lf\n", lowerOffsetMargin[i]);
      printf("Margin Frequency (Hz)        : %lf\n", lowerOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)  : %lf\n\n", lowerOffsetMarginAbsolutePower[i]);

   }

   printf("\n----------Upper Offset Segment Measurements----------\n\n");
   for (i = 0; i < arraySize; i++)
   {
      printf("Offset %d\n", i);
      printf("Measurement Status           : %s\n",
         upperOffsetMeasurementStatus[i] == RFMXLTE_VAL_SEM_UPPER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
      printf("Margin (dB)                  : %lf\n", upperOffsetMargin[i]);
      printf("Margin Frequency (Hz)        : %lf\n", upperOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)  : %lf\n\n", upperOffsetMarginAbsolutePower[i]);
   }

Error:
   if (error)
   {
      RFmxLTE_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("\nERROR: %s\n", errorMessage);
      else
         printf("\nWARNING: %s\n", errorMessage);
   }
   if (instrumentHandle)
   {
      RFmxLTE_Close(instrumentHandle, RFMXLTE_VAL_FALSE);
   }

   if (spectrum)
      free(spectrum);
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
   if (absoluteMask)
      free(absoluteMask);

   printf("\n\nPress any key to exit\n");
   _getch();

   return error;
}