//Steps:
//1. Open a new RFmx session.
//2. Configure the frequency reference properties (Clock Source and Clock Frequency).
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//5. Configure Standard as 802.11b.
//6. Select PowerRamp measurement and enable the traces.
//7. Configure the Acquisition Length.
//8. Configure Averaging parameters.
//9. Initiate Measurement.
//10. Fetch PowerRamp Traces and Measurements.
//11. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWLAN.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;

   char * frequencyReferenceSource = RFMXWLAN_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                                  /*(Hz) */

   float64 centerFrequency = 2.412e9;                                           /*(Hz) */
   float64 referenceLevel = 0.0;                                                /*(dBm) */
   float64 externalAttenuation = 0.0;                                           /*(dB) */

   int32 IQPowerEdgeEnabled = RFMXWLAN_VAL_TRUE;
   int32 IQPowerEdgeSlope = RFMXWLAN_VAL_IQ_POWER_EDGE_RISING_SLOPE;
   float64 IQPowerEdgeLevel = -20.0;                                            /*(dB) */
   int32 minimumQuiteTimeMode = RFMXWLAN_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 5e-6;                                             /*(seconds) */
   int32 IQPowerEdgeLevelType = RFMXWLAN_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
   float64 triggerDelay = 0.0;                                                  /*(seconds) */

   int32 standard = RFMXWLAN_VAL_STANDARD_802_11_B;

   float64 acquisitionLength = 1e-3;                                            /*(seconds) */

   uInt32 measurements = RFMXWLAN_VAL_POWERRAMP;
   int32 enableAllTraces = RFMXWLAN_VAL_TRUE;

   int32 averagingEnabled = RFMXWLAN_VAL_POWERRAMP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   float64 riseTimeMean = 0.0;                                                  /*(seconds) */
   float64 fallTimeMean = 0.0;                                                  /*(seconds) */

   float64 timeout = 10.0;                                                      /*(seconds) */

   int32 actualArraySize = 0;
   float64 x0 = 0, dx = 0;
   float32* riseRawWaveform = NULL;
   float32* riseProcessedWaveform = NULL;
   float32* riseThreshold = NULL;
   float32* risePowerReference = NULL;

   float32* fallRawWaveform = NULL;
   float32* fallProcessedWaveform = NULL;
   float32* fallThreshold = NULL;
   float32* fallPowerReference = NULL;

   /* Initialize a session */
   RFmxCheckWarn(RFmxInstr_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxInstr_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxWLAN_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxWLAN_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxWLAN_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxWLAN_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeSlope, IQPowerEdgeLevel,
      triggerDelay, minimumQuiteTimeMode, minimumQuietTime, IQPowerEdgeLevelType, IQPowerEdgeEnabled));
   RFmxCheckWarn(RFmxWLAN_CfgStandard(instrumentHandle, "", standard));
   RFmxCheckWarn(RFmxWLAN_SelectMeasurements(instrumentHandle, "", measurements, enableAllTraces));
   RFmxCheckWarn(RFmxWLAN_PowerRampCfgAcquisitionLength(instrumentHandle, "", acquisitionLength));
   RFmxCheckWarn(RFmxWLAN_PowerRampCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
   RFmxCheckWarn(RFmxWLAN_Initiate(instrumentHandle, "", ""));

   /* Fetch Results */
   RFmxCheckWarn(RFmxWLAN_PowerRampFetchMeasurement(instrumentHandle, "", timeout, &riseTimeMean,
      &fallTimeMean));

   actualArraySize = 0;
   RFmxCheckWarn(RFmxWLAN_PowerRampFetchRiseTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      riseRawWaveform = (float32*)malloc(sizeof(float32) * actualArraySize);
      riseProcessedWaveform = (float32*)malloc(sizeof(float32) * actualArraySize);
      riseThreshold = (float32*)malloc(sizeof(float32) * actualArraySize);
      risePowerReference = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (riseRawWaveform && riseProcessedWaveform && riseThreshold && risePowerReference)
      {
         RFmxCheckWarn(RFmxWLAN_PowerRampFetchRiseTrace(instrumentHandle, "", timeout, &x0, &dx, riseRawWaveform,
            riseProcessedWaveform, riseThreshold, risePowerReference, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }
   actualArraySize = 0;
   x0 = 0;
   dx = 0;
   RFmxCheckWarn(RFmxWLAN_PowerRampFetchFallTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      fallRawWaveform = (float32*)malloc(sizeof(float32) * actualArraySize);
      fallProcessedWaveform = (float32*)malloc(sizeof(float32) * actualArraySize);
      fallThreshold = (float32*)malloc(sizeof(float32) * actualArraySize);
      fallPowerReference = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (fallRawWaveform && fallProcessedWaveform && fallThreshold && fallPowerReference)
      {
         RFmxCheckWarn(RFmxWLAN_PowerRampFetchFallTrace(instrumentHandle, "", timeout, &x0, &dx, fallRawWaveform,
            fallProcessedWaveform, fallThreshold, fallPowerReference, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("------------------Measurement------------------\n");
   printf("Rise Time (s)                   : %lf\n", riseTimeMean);
   printf("Fall Time (s)                   : %lf\n\n", fallTimeMean);

Error:
   if (error)
   {
      RFmxWLAN_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (instrumentHandle)
   {
      RFmxWLAN_Close(instrumentHandle, 0);
   }

   /* Free allocated memory */
   if (riseRawWaveform)
   {
      free(riseRawWaveform);
   }
   if (riseProcessedWaveform)
   {
      free(riseProcessedWaveform);
   }
   if (riseThreshold)
   {
      free(riseThreshold);
   }
   if (risePowerReference)
   {
      free(risePowerReference);
   }
   if (fallRawWaveform)
   {
      free(fallRawWaveform);
   }
   if (fallProcessedWaveform)
   {
      free(fallProcessedWaveform);
   }
   if (fallThreshold)
   {
      free(fallThreshold);
   }
   if (fallPowerReference)
   {
      free(fallPowerReference);
   }
   printf("Press any key to exit\n");
   _getch();

   return error;
}