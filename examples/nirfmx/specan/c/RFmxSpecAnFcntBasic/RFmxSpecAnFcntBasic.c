//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure FCnt Measurement Interval
//5. Configure FCnt Averaging
//6. Configure FCnt RBW
//7. Read FCnt Measurement Results
//8. Close the RFmx Session

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
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;        /* Hz */
   float64 referenceLevel = 0.00;         /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */

   float64 timeout = 10.0;                /* seconds */

   /* Measurement Interval */
   float64 measInterval = 1e-3;

   /* RBW filter settings */
   float64 RBW = 100e+3;
   int32 RBWFilterType = RFMXSPECAN_VAL_FCNT_RBW_FILTER_TYPE_NONE;
   float64 RRCAlpha = 0.1;

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_FCNT_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_FCNT_AVERAGING_TYPE_MEAN;

   /* Variables to store measurement results */
   float64 averageRelativeFrequency = 0;     /* Hz */
   float64 averageAbsoluteFrequency = 0;     /* Hz */
   float64 meanPhase = 0;                    /* deg */

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure FCnt parameters */
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_FCntCfgMeasurementInterval(instrumentHandle, "", measInterval));
   RFmxCheckWarn(RFmxSpecAn_FCntCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount,
      averagingType));
   RFmxCheckWarn(RFmxSpecAn_FCntCfgRBWFilter(instrumentHandle, "", RBW, RBWFilterType, RRCAlpha));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_FCntRead(instrumentHandle, "", timeout, &averageRelativeFrequency, &averageAbsoluteFrequency,
      &meanPhase));

   printf("Average Relative Frequency (Hz)  %lf\n", averageRelativeFrequency);
   printf("Average Absolute Frequency (Hz)  %lf\n", averageAbsoluteFrequency);
   printf("Mean Phase (deg)                 %lf\n", meanPhase);

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
   printf("Press any key to exit\n");
   _getch();

   return error;
}
