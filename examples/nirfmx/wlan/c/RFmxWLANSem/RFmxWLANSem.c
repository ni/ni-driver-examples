//Steps:
//1. Open a new RFmx session.
//2. Configure the frequency reference properties(Clock Source and Clock Frequency).
//3. Configure the basic signal properties(Center Frequency, Reference Level and External Attenuation).
//4. Configure IQ Power Edge Trigger properties(Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//5. Configure Standard and Channel Bandwidth Properties.
//6. Select SEM measurement and enable the traces.
//7. Configure Averaging parameters.
//8. Configure Sweep Time and Span parameters.
//9. Initiate Measurement.
//10. Fetch SEM Traces and Measurements.
//11. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWLAN.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION               4096

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };

   int32 error = 0, lastErrorCode = 0;
   int i = 0;

   float64 centerFrequency = 2.412e9;                                /*(Hz) */
   float64 referenceLevel = 0.0;                                     /*(dBm) */
   float64 externalAttenuation = 0.0;                                /*(dB) */

   char *frequencyReferenceSource = RFMXWLAN_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                       /*(Hz) */

   int32 IQPowerEdgeEnabled = RFMXWLAN_VAL_TRUE;
   float64 IQPowerEdgeLevel = -20.0;                                 /*(dB) */
   float64 triggerDelay = 0.0;                                       /*(s) */
   int32 minimumQuietTimeMode = RFMXWLAN_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 5.0e-6;                                /* (s) */

   int32 standard = RFMXWLAN_VAL_STANDARD_802_11_AG;

   float64 channelBandwidth = 20e6;                                  /*(Hz) */

   int32 averagingEnabled = RFMXWLAN_VAL_SEM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXWLAN_VAL_SEM_AVERAGING_TYPE_RMS;

   int32 spanAuto = RFMXWLAN_VAL_SEM_SPAN_AUTO_TRUE;
   float64 span = 66.0e6;                                            /*(Hz) */

   int32 sweepTimeAuto = RFMXWLAN_VAL_SEM_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTime = 1.0e-3;                                       /*(s) */

   int32 maskType = RFMXWLAN_VAL_SEM_MASK_TYPE_STANDARD;

   float64 timeout = 10.0;                                           /*(s) */

   int32 measurementStatus = RFMXWLAN_VAL_SEM_MEASUREMENT_STATUS_FAIL;

   float64 absolutePower = 0.0;                                      /*(dBm) */
   float64 relativePower = 0.0;                                      /*(dBm) */

   float64 *lowerOffsetMarginRelativePower = NULL;                   /*(dBm) */
   float64 *lowerOffsetMarginAbsolutePower = NULL;                   /*(dBm) */
   float64 *lowerOffsetMargin = NULL;                                /*(dB) */
   float64 *lowerOffsetMarginFrequency = NULL;                       /*(Hz) */
   int32 *lowerOffsetMeasurementStatus = NULL;

   float64 *upperOffsetMarginRelativePower = NULL;                   /*(dBm) */
   float64 *upperOffsetMarginAbsolutePower = NULL;                   /*(dBm) */
   float64 *upperOffsetMargin = NULL;                                /*(dB) */
   float64 *upperOffsetMarginFrequency = NULL;                       /*(Hz) */
   int32 *upperOffsetMeasurementStatus = NULL;

   int32 actualArraySize = 0, arraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = NULL;
   float32 *compositeMask = NULL;

   /* Initialize a session */
   RFmxCheckWarn(RFmxWLAN_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxWLAN_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxWLAN_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxWLAN_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxWLAN_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxWLAN_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXWLAN_VAL_IQ_POWER_EDGE_RISING_SLOPE,
      IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
      RFMXWLAN_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, IQPowerEdgeEnabled));
   RFmxCheckWarn(RFmxWLAN_CfgStandard(instrumentHandle, "", standard));
   RFmxCheckWarn(RFmxWLAN_CfgChannelBandwidth(instrumentHandle, "", channelBandwidth));
   RFmxCheckWarn(RFmxWLAN_SelectMeasurements(instrumentHandle, "", RFMXWLAN_VAL_SEM, RFMXWLAN_VAL_TRUE));
   RFmxCheckWarn(RFmxWLAN_SEMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxWLAN_SEMCfgMaskType(instrumentHandle, "", maskType));
   RFmxCheckWarn(RFmxWLAN_SEMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTime));
   RFmxCheckWarn(RFmxWLAN_SEMCfgSpan(instrumentHandle, "", spanAuto, span));
   RFmxCheckWarn(RFmxWLAN_Initiate(instrumentHandle, "", ""));

   RFmxCheckWarn(RFmxWLAN_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));

   RFmxCheckWarn(RFmxWLAN_SEMFetchCarrierMeasurement(instrumentHandle, "", timeout, &absolutePower,
       &relativePower));

   RFmxCheckWarn(RFmxWLAN_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL,
      NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      lowerOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * actualArraySize);
      lowerOffsetMargin = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerOffsetMarginRelativePower = (float64 *)malloc(sizeof(float64) * actualArraySize);

      RFmxCheckWarn(RFmxWLAN_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout,
         lowerOffsetMeasurementStatus, lowerOffsetMargin, lowerOffsetMarginFrequency,
         lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower, actualArraySize, NULL));
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxWLAN_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL,
      NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      upperOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * actualArraySize);
      upperOffsetMargin = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperOffsetMarginRelativePower = (float64 *)malloc(sizeof(float64) * actualArraySize);

      RFmxCheckWarn(RFmxWLAN_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout,
         upperOffsetMeasurementStatus, upperOffsetMargin, upperOffsetMarginFrequency,
         upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower, actualArraySize, &arraySize));
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxWLAN_SEMFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL,
      0, &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
      compositeMask = (float32 *)malloc(sizeof(float32) * actualArraySize);
      if (spectrum && compositeMask)
      {
         RFmxCheckWarn(RFmxWLAN_SEMFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, compositeMask,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("Measurement Status                      : %s\n",
      measurementStatus == RFMXWLAN_VAL_SEM_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
   printf("Carrier Absolute Power (dBm)            : %lf\n", absolutePower);

   printf("\n----------Lower Offset Measurements----------\n\n");

   for (i = 0; i < arraySize; i++)
   {
      printf("Offset %d\n", i);
      printf("Measurement Status           : %s\n",
         lowerOffsetMeasurementStatus[i] == RFMXWLAN_VAL_SEM_LOWER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
      printf("Margin (dB)                  : %lf\n", lowerOffsetMargin[i]);
      printf("Margin Frequency (Hz)        : %lf\n", lowerOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)  : %lf\n\n", lowerOffsetMarginAbsolutePower[i]);
   }

   printf("\n----------Upper Offset Measurements----------\n\n");
   for (i = 0; i < arraySize; i++)
   {
      printf("Offset %d\n", i);
      printf("Measurement Status           : %s\n",
         upperOffsetMeasurementStatus[i] == RFMXWLAN_VAL_SEM_UPPER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
      printf("Margin (dB)                  : %lf\n", upperOffsetMargin[i]);
      printf("Margin Frequency (Hz)        : %lf\n", upperOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)  : %lf\n\n", upperOffsetMarginAbsolutePower[i]);
   }


Error:
   if (error)
   {
      RFmxWLAN_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("\nERROR: %s\n", errorMessage);
      else
         printf("\nWARNING: %s\n", errorMessage);
   }
   if (instrumentHandle)
   {
      RFmxWLAN_Close(instrumentHandle, RFMXWLAN_VAL_FALSE);
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

   printf("\n\nPress any key to exit\n");
   _getch();

   return error;
}
