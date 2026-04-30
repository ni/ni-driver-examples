//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select 20dBBandwidth measurement and enable Traces.
//6. Configure Averaging Parameters for 20dBBandwidth measurement.
//7. Initiate the Measurement.
//8. Fetch 20dBBandwidth Measurements and Trace.
//9. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxBT.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;

   char * frequencyReferenceSource = RFMXBT_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                                  /*(Hz) */

   float64 centerFrequency = 2.402000e9;                                        /*(Hz) */
   float64 referenceLevel = 0.0;                                                /*(dBm) */
   float64 externalAttenuation = 0.0;                                           /*(dB) */

   int32 enableTrigger = RFMXBT_VAL_TRUE;
   int32 IQPowerEdgeSlope = RFMXBT_VAL_IQ_POWER_EDGE_RISING_SLOPE;
   float64 IQPowerEdgeLevel = -20.0;                                            /*(dB) */
   int32 minimumQuiteTimeMode = RFMXBT_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 100e-6;                                           /*(seconds) */
   int32 IQPowerEdgeLevelType = RFMXBT_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
   float64 triggerDelay = 0.0;                                                  /*(seconds) */

   uInt32 measurements = RFMXBT_VAL_20DB_BANDWIDTH;
   int32 enableAllTraces = RFMXBT_VAL_TRUE;

   int32 averagingEnabled = RFMXBT_VAL_20DB_BANDWIDTH_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   float64 timeout = 10.0;                                                      /*seconds */
   float64 peakPower = 0.0;                                                     /*(dBm) */
   float64 bandwidth = 0.0;                                                     /*(Hz) */
   float64 highFrequency = 0.0;                                                 /*(Hz) */
   float64 lowFrequency = 0.0;                                                  /*(Hz) */

   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = NULL;
   int32 actualArraySize = 0;

   /* Initialize a session */
   RFmxCheckWarn(RFmxBT_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxBT_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxBT_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxBT_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeSlope, IQPowerEdgeLevel, triggerDelay,
	   minimumQuiteTimeMode, minimumQuietTime, IQPowerEdgeLevelType, enableTrigger));
   RFmxCheckWarn(RFmxBT_SelectMeasurements(instrumentHandle, "", measurements, enableAllTraces));
   RFmxCheckWarn(RFmxBT_20dBBandwidthCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
   RFmxCheckWarn(RFmxBT_Initiate(instrumentHandle, "", ""));

   /* Fetch Results */
   RFmxCheckWarn(RFmxBT_20dBBandwidthFetchMeasurement(instrumentHandle, "", timeout, &peakPower, &bandwidth, &highFrequency, &lowFrequency));
   actualArraySize = 0;
   RFmxCheckWarn(RFmxBT_20dBBandwidthFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
	  spectrum = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (spectrum)
      {
         RFmxCheckWarn(RFmxBT_20dBBandwidthFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("------------------Measurement------------------\n");
   printf("Peak Power (dBm)                           : %lf\n", peakPower);
   printf("Bandwidth (Hz)                             : %lf\n", bandwidth);
   printf("High Frequency (Hz)                        : %lf\n", highFrequency);
   printf("Low Frequency (Hz)                         : %lf\n", lowFrequency);

Error:
   if (error)
   {
      RFmxBT_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (instrumentHandle)
   {
      RFmxBT_Close(instrumentHandle, RFMXBT_VAL_FALSE);
   }

    /* Free allocated memory */
    if (spectrum)
    {
       free(spectrum);
    }

   printf("Press any key to exit\n");
   _getch();

   return error;
}