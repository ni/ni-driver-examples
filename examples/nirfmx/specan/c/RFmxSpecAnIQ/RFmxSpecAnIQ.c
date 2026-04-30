//Steps:
//1. Open a new RFmx session.
//2. Configure the basic instrument properties (Clock Source and Clock Frequency).
//3. Configure Selected Ports.
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation).
//5. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time).
//6. Configure IQ measurement.
//7. Configure Acquisition parameters.
//8. Initiate Measurement.
//9. Fetch IQ Data.
//10. Mean Power calculation - Power in dBm = 10* log (((I^2+Q^2)/(2*R))/1mW), where R=50 Ohms.
//11. Close the RFmx Session.

#include <stdio.h>
#include <stdlib.h>
#include <conio.h>
#include <math.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0;
   int32 lastErrorCode = 0;

   char *resourceName = "RFSA";
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;                             /* Hz */
   float64 referenceLevel = 0.00;                              /* dBm */
   float64 externalAttenuation = 0.00;                         /* dB */

   char *frequencySource = RFMXSPECAN_VAL_ONBOARD_CLOCK_STR;
   float64 frequency = 10e+6;

   int32 IQPowerEdgeEnabled = RFMXSPECAN_VAL_FALSE;
   float64 IQPowerEdgeLevel = -20.0;                           /* dBm */
   float64 triggerDelay = 0.0;                                 /* seconds */
   float64 minQuietTime = 0.0;                                 /* seconds */

   float64 sampleRate = 10e+6;                                 /* samples per second */
   float64 acquisitionTime = 0.001;                            /* seconds */

   int32 recordToFetch = 0;
   int64 samplesToRead = -1;

   float64 meanPower = 0.0;                                    /* dBm */
   float64 t0 = 0.0, dt = 0.0, timeout = 10.0;
   NIComplexSingle *data = NULL;
   int32 arraySize = 0, actualArraySize = 0;

   int32 i = 0;
   float64 sum = 0.0;
   float64 powerInDbm = 0.0;

   /* Initialize a session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));

   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));

   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel,
      externalAttenuation));

   RFmxCheckWarn(RFmxSpecAn_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeLevel,
      RFMXSPECAN_VAL_IQ_POWER_EDGE_RISING_SLOPE, triggerDelay,
      RFMXSPECAN_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_MANUAL, minQuietTime,
      IQPowerEdgeEnabled));

   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_IQ, RFMXSPECAN_VAL_FALSE));

   RFmxCheckWarn(RFmxSpecAn_IQCfgAcquisition(instrumentHandle, "", sampleRate, 1, acquisitionTime, 0));

   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   arraySize = (int32)ceil(sampleRate * acquisitionTime) + 10;
   if (arraySize > 0)
   {
      data = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * arraySize);
      if (data)
      {
         RFmxCheckWarn(RFmxSpecAn_IQFetchData(instrumentHandle, "", timeout, recordToFetch, samplesToRead,
            &t0, &dt, data, arraySize, &actualArraySize));
      }
      else
      {
         printf("malloc failed\n");
         goto Error;
      }
   }

   for (i = 0; i < actualArraySize; i++)
   {
      sum += (float64)((data[i].real * data[i].real) + (data[i].imaginary * data[i].imaginary));
   }

   meanPower = sum / (float64)actualArraySize;
   powerInDbm = 10 * log10((meanPower / (2 * 50)) / 0.001);
   printf("Mean Power (dBm)	%f\n", powerInDbm);

Error:
   if (error)
   {
      RFmxSpecAn_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }
   if (instrumentHandle)
   {
      RFmxSpecAn_Close(instrumentHandle, RFMXSPECAN_VAL_FALSE);
   }
   if (data)
      free(data);

   printf("Press any key to exit\n");
   _getch();

   return error;
}
