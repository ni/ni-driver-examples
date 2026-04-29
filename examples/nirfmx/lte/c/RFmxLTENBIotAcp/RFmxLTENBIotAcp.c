//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Configure Uplink Subcarrier Spacing.
//7. Select ACP measurement and enable Traces.
//8. Configure Averaging Parameters for ACP measurement.
//9. Configure Sweep Time Parameters.
//10. Initiate the Measurement.
//11. Fetch ACP Measurements and Traces.
//12. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096

#define NUMBER_OF_OFFSETS                       2


int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0;

   char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                                         /*(Hz) */

   float64 centerFrequency = 1.95e9;                                                   /*(Hz) */
   float64 referenceLevel = 0.0;                                                       /*(dBm) */
   float64 externalAttenuation = 0.0;                                                  /*(dB) */

   char * IQPowerEdgeSource = "0";
   int32 IQPowerEdgeSlope = RFMXLTE_VAL_IQ_POWER_EDGE_RISING_SLOPE;
   int32 IQPowerEdgeLevelType = RFMXLTE_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
   float64 IQPowerEdgeLevel = -20.0;                                                   /*(dB) */
   float64 triggerDelay = 0.0;                                                         /*(s) */
   int32 triggerMinimumQuietTimeMode = RFMXLTE_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 triggerMinimumQuietTimeDuration = 100e-6;                                   /*(s) */
   int32 enableTrigger = RFMXLTE_VAL_TRUE;

   int32 linkDirection = RFMXLTE_VAL_LINK_DIRECTION_UPLINK;

   int32 uplinkSubCarrierSpacing = RFMXLTE_VAL_NB_IOT_UPLINK_SUBCARRIER_SPACING_15KHZ;

   int32 sweepTimeAuto = RFMXLTE_VAL_ACP_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 0.001;                                                  /*(s) */

   int32 averagingEnabled = RFMXLTE_VAL_ACP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXLTE_VAL_ACP_AVERAGING_TYPE_RMS;

   float64 componentCarrierBandwidth = 200e3;                                          /*(Hz) */
   float64 componentCarrierFrequency = 0.0;                                            /*(Hz) */
   int32 cellID = 0;

   float64 timeout = 10.0;                                                             /*(s) */

   float64 lowerAbsolutePower[NUMBER_OF_OFFSETS] = { 0 };                              /*(dBm) */
   float64 upperAbsolutePower[NUMBER_OF_OFFSETS] = { 0 };                              /*(dBm) */
   float64 lowerRelativePower[NUMBER_OF_OFFSETS] = { 0 };                              /*(dB) */
   float64 upperRelativePower[NUMBER_OF_OFFSETS] = { 0 };                              /*(dB) */

   float64 carrierAbsolutePower = 0.0;                                                 /*(dBm) */
   float64 carrierRelativePower = 0.0;                                                 /*(dBm) */

   int32 actualArraySize = 0;
   int32 numberOfIndex = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = NULL;                                                           /*(dBm) */
   float32 *absolutePowersTrace = NULL;                                                /*(dBm) */
   float32 *relativePowersTrace = NULL;                                                /*(dBm) */

   /* Initialize a session */
   RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxLTE_CfgIQPowerEdgeTrigger(instrumentHandle, "", IQPowerEdgeSource, IQPowerEdgeSlope,
      IQPowerEdgeLevel, triggerDelay, triggerMinimumQuietTimeMode, triggerMinimumQuietTimeDuration,
      IQPowerEdgeLevelType, enableTrigger));
   RFmxCheckWarn(RFmxLTE_CfgLinkDirection(instrumentHandle, "", linkDirection));
   RFmxCheckWarn(RFmxLTE_CfgComponentCarrier(instrumentHandle, "", componentCarrierBandwidth,
      componentCarrierFrequency, cellID));
   RFmxCheckWarn(RFmxLTE_CfgNBIoTComponentCarrier(instrumentHandle, "", cellID, uplinkSubCarrierSpacing));
   RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_ACP, RFMXLTE_VAL_TRUE));
   RFmxCheckWarn(RFmxLTE_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxLTE_ACPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */

   RFmxCheckWarn(RFmxLTE_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout,
      lowerRelativePower, upperRelativePower,
      lowerAbsolutePower, upperAbsolutePower,
      NUMBER_OF_OFFSETS, NULL));

   RFmxCheckWarn(RFmxLTE_ACPFetchComponentCarrierMeasurement(instrumentHandle, "", timeout,
      &carrierAbsolutePower, &carrierRelativePower));

   for (i = 0; i < NUMBER_OF_OFFSETS; i++)
   {
      RFmxCheckWarn(RFmxLTE_ACPFetchAbsolutePowersTrace(instrumentHandle, "", timeout, i,
         NULL, NULL, NULL, 0, &actualArraySize));
      if (actualArraySize > 0)
      {
         absolutePowersTrace = (float32 *)malloc(sizeof(float32) * actualArraySize);
         if (absolutePowersTrace)
            RFmxCheckWarn(RFmxLTE_ACPFetchAbsolutePowersTrace(instrumentHandle, "", timeout, i,
               &x0, &dx, absolutePowersTrace, actualArraySize, NULL));
      }
   }

   actualArraySize = 0;
   for (i = 0; i < NUMBER_OF_OFFSETS; i++)
   {
      RFmxCheckWarn(RFmxLTE_ACPFetchRelativePowersTrace(instrumentHandle, "", timeout, i,
         NULL, NULL, NULL, 0, &actualArraySize));
      if (actualArraySize > 0)
      {
         relativePowersTrace = (float32 *)malloc(sizeof(float32) * actualArraySize);
         if (relativePowersTrace)
            RFmxCheckWarn(RFmxLTE_ACPFetchRelativePowersTrace(instrumentHandle, "", timeout,
               i, &x0, &dx, relativePowersTrace, actualArraySize, NULL));
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxLTE_ACPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
      if (spectrum)
      {
         RFmxCheckWarn(RFmxLTE_ACPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("\nCarrier Absolute Power (dBm) : %f\n", carrierAbsolutePower);

   printf("\n-----------Offset Channel Measurements----------- \n");
   for (i = 0; i < NUMBER_OF_OFFSETS; i++)
   {
      printf("Offset : %d\n", i);
      printf("Lower Relative Power (dB)    : %f\n", lowerRelativePower[i]);
      printf("Upper Relative Power (dB)    : %f\n", upperRelativePower[i]);
      printf("Lower Absolute Power (dBm)   : %f\n", lowerAbsolutePower[i]);
      printf("Upper Absolute Power (dBm)   : %f\n", upperAbsolutePower[i]);
      printf("------------------------------------------\n");
   }


Error:
   if (error)
   {
      RFmxLTE_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (instrumentHandle)
   {
      RFmxLTE_Close(instrumentHandle, RFMXLTE_VAL_FALSE);
   }

   /* Free allocated memory */
   if (spectrum)
   {
      free(spectrum);
   }
   if (relativePowersTrace)
   {
      free(relativePowersTrace);
   }
   if (absolutePowersTrace)
   {
      free(absolutePowersTrace);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
