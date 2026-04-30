//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select Carrier Bandwidth as 200k.
//6. Configure Uplink Subcarrier Spacing.
//7. Select SEM measurement and enable Traces.
//8. Configure Sweep Time Parameters.
//9. Configure Averaging Parameters for SEM measurement.
//10. Initiate the Measurement.
//11. Fetch SEM Measurements and Traces.
//12. Close RFmx Session.

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

   float64 centerFrequency = 1.95e9;                                 /*(Hz) */
   float64 referenceLevel = 0.0;                                     /*(dBm) */
   float64 externalAttenuation = 0.0;                                /*(dB) */

   char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                       /*(Hz) */

   int32 enableTrigger = RFMXLTE_VAL_TRUE;
   int32 IQPowerEdgeSlope = RFMXLTE_VAL_IQ_POWER_EDGE_RISING_SLOPE;
   float64 IQPowerEdgeLevel = -20.0;                                 /*(dB) */
   int32 minimumQuiteTimeMode = RFMXLTE_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 100e-6;                                /*(s) */
   int32 IQPowerEdgeLevelType = RFMXLTE_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
   float64 triggerDelay = 0.0;                                       /*(s) */

   int32 linkDirection = RFMXLTE_VAL_LINK_DIRECTION_UPLINK;
   
   int32 uplinkSubcarrierSpacing = RFMXLTE_VAL_NB_IOT_UPLINK_SUBCARRIER_SPACING_15KHZ;
   
   /* Downlink */
   int32 eNodeBCategory = RFMXLTE_VAL_ENODEB_WIDE_AREA_BASE_STATION_CATEGORY_A;
   int32 downlinkMaskType = RFMXLTE_VAL_SEM_DOWNLINK_MASK_TYPE_ENODEB_CATEGORY_BASED;
   float64 deltaFMaximum = 15.0e6;                     /* (Hz) */
   float64 aggregatedMaximumPower = 0.0;               /* (dBm) */
   float64 maximumOutputPower = 0.0;                   /* (dBm) */

   int32 sweepTimeAuto = RFMXLTE_VAL_SEM_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 0.001;                                /*(s) */

   int32 averagingEnabled = RFMXLTE_VAL_SEM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXLTE_VAL_SEM_AVERAGING_TYPE_RMS;

   float64 componentCarrierBandwidth = 200e3;                        /*(Hz) */
   float64 componentCarrierFrequency = 0.0;                          /*(Hz) */
   int32 cellID = 0;

   int32 NCellID = 0;

   float64 timeout = 10.0;                                           /*(s) */

   int32 measurementStatus = RFMXLTE_VAL_SEM_MEASUREMENT_STATUS_FAIL;

   float64 absoluteIntegratedPower = 0.0;                            /*(dBm) */
   float64 relativeIntegratedPower = 0.0;                            /*(dBm) */

   int32 *lowerOffsetMeasurementStatus = NULL;
   float64 *lowerOffsetMargin = NULL;                                /*(dB) */
   float64 *lowerOffsetMarginFrequency = NULL;                       /*(Hz) */
   float64 *lowerOffsetMarginAbsolutePower = NULL;                   /*(dBm) */
   float64 *lowerOffsetMarginRelativePower = NULL;                   /*(dBm) */

   int32 *upperOffsetMeasurementStatus = NULL;
   float64 *upperOffsetMargin = NULL;                                /*(dB) */
   float64 *upperOffsetMarginFrequency = NULL;                       /*(Hz) */
   float64 *upperOffsetMarginAbsolutePower = NULL;                   /*(dBm) */
   float64 *upperOffsetMarginRelativePower = NULL;                   /*(dBm) */

   int32 actualArraySize = 0, arraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = NULL;                                         /*(dBm) */
   float32 *compositeMask = NULL;

   /* Initialize a session */
   RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxLTE_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeSlope, IQPowerEdgeLevel,
      triggerDelay, minimumQuiteTimeMode, minimumQuietTime, IQPowerEdgeLevelType, enableTrigger));
   RFmxCheckWarn(RFmxLTE_CfgComponentCarrier(instrumentHandle, "", componentCarrierBandwidth,
      componentCarrierFrequency, cellID));
   RFmxCheckWarn(RFmxLTE_CfgLinkDirection(instrumentHandle, "", linkDirection));
   RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_SEM, RFMXLTE_VAL_TRUE));
   RFmxCheckWarn(RFmxLTE_SEMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxLTE_SEMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   
   if (linkDirection == RFMXLTE_VAL_LINK_DIRECTION_UPLINK)
	   
   RFmxCheckWarn(RFmxLTE_CfgNBIoTComponentCarrier(instrumentHandle, "", NCellID, uplinkSubcarrierSpacing)); 
   
   else if (linkDirection == RFMXLTE_VAL_LINK_DIRECTION_DOWNLINK)
   {
      RFmxCheckWarn(RFmxLTE_CfgeNodeBCategory(instrumentHandle, "", eNodeBCategory));
      RFmxCheckWarn(RFmxLTE_SEMCfgDownlinkMask(instrumentHandle, "", downlinkMaskType, deltaFMaximum,
         aggregatedMaximumPower));
      RFmxCheckWarn(RFmxLTE_SEMCfgComponentCarrierMaximumOutputPower(instrumentHandle, "", maximumOutputPower));
   }
   
   RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));
   
   RFmxCheckWarn(RFmxLTE_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL,
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
         RFmxCheckWarn(RFmxLTE_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout,
            upperOffsetMeasurementStatus,upperOffsetMargin, upperOffsetMarginFrequency,
            upperOffsetMarginAbsolutePower,upperOffsetMarginRelativePower, actualArraySize, &arraySize));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxLTE_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL,
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
         RFmxCheckWarn(RFmxLTE_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout,
            lowerOffsetMeasurementStatus, lowerOffsetMargin, lowerOffsetMarginFrequency,
            lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxLTE_SEMFetchComponentCarrierMeasurement(instrumentHandle, "", timeout,
      &absoluteIntegratedPower, &relativeIntegratedPower));

   RFmxCheckWarn(RFmxLTE_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));

   actualArraySize = 0;
   RFmxCheckWarn(RFmxLTE_SEMFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
      compositeMask = (float32 *)malloc(sizeof(float32) * actualArraySize);
      if (spectrum && compositeMask)
      {
         RFmxCheckWarn(RFmxLTE_SEMFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, compositeMask,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("Measurement Status                      : %s\n",
      measurementStatus == RFMXLTE_VAL_SEM_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
   printf("Carrier Absolute Integrated Power (dBm) : %lf\n", absoluteIntegratedPower);

   printf("\n\n----------Lower Offset Segment Measurements----------\n\n");

   for (i = 0; i < arraySize; i++)
   {
      printf("Offset %d\n", i);
      printf("Measurement Status           : %s\n",
         lowerOffsetMeasurementStatus[i] == RFMXLTE_VAL_SEM_LOWER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
      printf("Margin (dB)                  : %lf\n", lowerOffsetMargin[i]);
      printf("Margin Frequency (Hz)        : %lf\n", lowerOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)  : %lf\n\n", lowerOffsetMarginAbsolutePower[i]);

   }

   printf("\n\n----------Upper Offset Segment Measurements----------\n\n");
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
   if (spectrum)
      free(spectrum);
   if (compositeMask)
      free(compositeMask);

   printf("\n\nPress any key to exit\n");
   _getch();

   return error;
}
