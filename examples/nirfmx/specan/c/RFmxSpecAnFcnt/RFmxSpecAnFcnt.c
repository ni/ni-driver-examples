//Steps:
//1. Open a new RFmx session
//2. Configure the instrument properties Clock Source and Clock Frequency
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select FCnt measurement and enable the traces
//6. Configure FCnt Measurement Interval
//7. Configure FCnt RBW filter
//8. Configure FCnt Averaging
//9. Configure FCnt Threshold
//10. Initiate Measurment
//11. Fetch FCnt Measurements and Traces
//12. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];

   char *resourceName = "RFSA";
   char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;        /* Hz */
   float64 referenceLevel = 0.00;         /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */
   float64 frequency = 10.0e+6;           /* Hz */

   float64 timeout = 10.0;                /* seconds */

   /* measurement interval */
   float64 measInterval = 1e-3;           /* seconds */

   /* RBW Filter */
   int32 RBWFilterType = RFMXSPECAN_VAL_FCNT_RBW_FILTER_TYPE_NONE;
   float64 RBW = 100e+3;                  /* Hz */
   float64 RRCAlpha = 0.10;

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_FCNT_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_FCNT_AVERAGING_TYPE_MEAN;

   /* Threshold */
   int32 thresholdEnabled = RFMXSPECAN_VAL_FCNT_THRESHOLD_ENABLED_FALSE;
   int32 thresholdType = RFMXSPECAN_VAL_FCNT_THRESHOLD_TYPE_RELATIVE;
   float64 thresholdLevel = -20.00;       /* dB or dBm*/

   /* Variables to store the measurement results */
   int32 actualArraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32 *frequencyTrace = NULL;

   float64 averageRelativeFrequency = 0;     /* Hz */
   float64 averageAbsoluteFrequency = 0;     /* Hz */
   float64 meanPhase = 0;                    /* deg */

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure FCnt parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_FCNT, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_FCntCfgMeasurementInterval(instrumentHandle, "", measInterval));
   RFmxCheckWarn(RFmxSpecAn_FCntCfgRBWFilter(instrumentHandle, "", RBW, RBWFilterType, RRCAlpha));
   RFmxCheckWarn(RFmxSpecAn_FCntCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount,
      averagingType));
   RFmxCheckWarn(RFmxSpecAn_FCntCfgThreshold(instrumentHandle, "", thresholdEnabled, thresholdLevel,
      thresholdType));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_FCntFetchFrequencyTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      frequencyTrace = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (frequencyTrace != NULL)
      {
         RFmxCheckWarn(RFmxSpecAn_FCntFetchFrequencyTrace(instrumentHandle, "", timeout, &x0, &dx, frequencyTrace,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }
   RFmxCheckWarn(RFmxSpecAn_FCntFetchMeasurement(instrumentHandle, "", timeout, &averageRelativeFrequency,
      &averageAbsoluteFrequency, &meanPhase));

   printf("Average Relative Frequency (Hz)   %f\n", averageRelativeFrequency);
   printf("Average Absolute Frequency (Hz)   %f\n", averageAbsoluteFrequency);
   printf("Mean Phase (deg)                  %f\n", meanPhase);

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
   /* Free allocated memory */
   if (frequencyTrace)
      free(frequencyTrace);
   printf("Press any key to exit\n");
   _getch();
   return error;
}
