//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Parameters for IQ Power Edge Trigger.
//6. Configure Link Direction, Frequency Range, Carrier Bandwidth and BWP Subcarrier Spacing.
//7. Select SEM measurement and enable Traces.
//8. Configure Uplink Mask Type, or Downlink Mask, Band, gNodeB Category, Delta F_Max(Hz) and
//   Component Carrier Rated Output Power based on Link Direction.
//9. Configure Sweep Time Parameters.
//10. Configure Averaging Parameters for SEM measurement.
//11. Initiate the Measurement.
//12. Fetch SEM Measurements and Traces.
//13. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxNR.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION               4096

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0;

   char* selectedPorts = "";
   float64 centerFrequency = 3.5e9;                                                          /* (Hz) */
   float64 referenceLevel = 0.0;                                                             /* (dBm) */
   float64 externalAttenuation = 0.0;                                                        /* (dB) */

   char *frequencyReferenceSource = RFMXNR_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                                               /* (Hz) */

   int32 IQPowerEdgeEnabled = RFMXNR_VAL_FALSE;
   float64 IQPowerEdgeLevel = -20.0;                                                         /* (dB or dBm) */
   float64 triggerDelay = 0.0;                                                               /* (s) */
   int32 minimumQuietTimeMode = RFMXNR_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 8.0e-6;                                                        /* (s) */

   int32 linkDirection = RFMXNR_VAL_LINK_DIRECTION_UPLINK;

   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;

   int32 uplinkMaskType = RFMXNR_VAL_SEM_UPLINK_MASK_TYPE_GENERAL;

   int32 gNodeBCategory = RFMXNR_VAL_GNODEB_CATEGORY_WIDE_AREA_BASE_STATION_CATEGORY_A;
   int32 downlinkMaskType = RFMXNR_VAL_SEM_DOWNLINK_MASK_TYPE_STANDARD;
   float64 deltaFMaximum = 15.0e6;                                                           /* (Hz) */
   float64 componentCarrierRatedOutputPower = 0.0;                                           /* (dBm) */
   int32 band = 78;

   float64 carrierBandwidth = 100e6;                                                         /* (Hz) */
   float64 subcarrierSpacing = 30e3;                                                         /* (Hz) */

   int32 sweepTimeAuto = RFMXNR_VAL_SEM_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 1.0e-3;                                                       /* (s) */

   int32 averagingEnabled = RFMXNR_VAL_SEM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXNR_VAL_SEM_AVERAGING_TYPE_RMS;

   float64 timeout = 10.0;                                                                   /* (s) */

   int32 measurementStatus = RFMXNR_VAL_SEM_MEASUREMENT_STATUS_FAIL;

   float64 absolutePower = 0.0;                                                              /* (dBm) */
   float64 peakAbsolutePower = 0.0;                                                          /* (dBm) */
   float64 peakFrequency = 0.0;                                                              /* (Hz) */
   float64 relativePower = 0.0;                                                              /* (dB) */

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
   RFmxCheckWarn(RFmxNR_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXNR_VAL_IQ_POWER_EDGE_RISING_SLOPE,
      IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
      RFMXNR_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, IQPowerEdgeEnabled));

   RFmxCheckWarn(RFmxNR_SetLinkDirection(instrumentHandle, "", linkDirection));
   RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, "", frequencyRange));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, "", carrierBandwidth));
   RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, "", subcarrierSpacing));

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_SEM, RFMXNR_VAL_TRUE));

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
      RFmxCheckWarn(RFmxNR_SEMCfgComponentCarrierRatedOutputPower(instrumentHandle, "",
         componentCarrierRatedOutputPower));
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

      RFmxCheckWarn(RFmxNR_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, upperOffsetMeasurementStatus,
         upperOffsetMargin, upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower,
         upperOffsetMarginRelativePower, actualArraySize, &arraySize));
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

      RFmxCheckWarn(RFmxNR_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, lowerOffsetMeasurementStatus,
         lowerOffsetMargin, lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower,
         lowerOffsetMarginRelativePower, actualArraySize, NULL));
   }

   RFmxCheckWarn(RFmxNR_SEMFetchComponentCarrierMeasurement(instrumentHandle, "", timeout, &absolutePower,
      &peakAbsolutePower, &peakFrequency, &relativePower));

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

   printf("Measurement Status                      : %s\n",
      measurementStatus == RFMXNR_VAL_SEM_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
   printf("Carrier Absolute Integrated Power (dBm) : %lf\n", absolutePower);

   printf("\n----------Lower Offset Segment Measurements----------\n\n");

   for (i = 0; i < arraySize; i++)
   {
      printf("Offset  %d\n", i);
      printf("Measurement Status                      : %s\n",
         lowerOffsetMeasurementStatus[i] == RFMXNR_VAL_SEM_LOWER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
      printf("Margin (dB)                             : %lf\n", lowerOffsetMargin[i]);
      printf("Margin Frequency (Hz)                   : %lf\n", lowerOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)             : %lf\n\n", lowerOffsetMarginAbsolutePower[i]);
   }

   printf("\n----------Upper Offset Segment Measurements----------\n\n");
   for (i = 0; i < arraySize; i++)
   {
      printf("Offset  %d\n", i);
      printf("Measurement Status                      : %s\n",
         upperOffsetMeasurementStatus[i] == RFMXNR_VAL_SEM_UPPER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
      printf("Margin (dB)                             : %lf\n", upperOffsetMargin[i]);
      printf("Margin Frequency (Hz)                   : %lf\n", upperOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)             : %lf\n\n", upperOffsetMarginAbsolutePower[i]);
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
