//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure TXP RBW
//5. Configure TXP Measurement Interval
//6. Configure TXP Averaging
//7. Read TXP Measurement Results
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

   float64 measInterval = 1e-3;           /* seconds */

   /* RBW Filter */
   float64 RBW = 100e3;
   int32 RBWFilterType = RFMXSPECAN_VAL_TXP_RBW_FILTER_TYPE_GAUSSIAN;
   float64 RRCAlpha = 0.1;

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_TXP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_TXP_AVERAGING_TYPE_RMS;

   /* Variables to store measurement data */
   float64 averageMeanPower = 0;
   float64 peakToAverageRatio = 0;
   float64 maximumPower = 0;
   float64 minimumPower = 0;

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure TXP parameters */
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgMeasurementInterval(instrumentHandle, "", measInterval));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgRBWFilter(instrumentHandle, "", RBW, RBWFilterType, RRCAlpha));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));

   /* Fetch measurement data */
   RFmxCheckWarn(RFmxSpecAn_TXPRead(instrumentHandle, "", timeout, &averageMeanPower,
      &peakToAverageRatio, &maximumPower, &minimumPower));

   /* Display results */
   printf("Average Mean Power(dBm)    %f\n", averageMeanPower);
   printf("Peak to Average Ratio(dB)  %f\n", peakToAverageRatio);
   printf("Maximum Power(dBm)         %f\n", maximumPower);
   printf("Minimum Power(dBm)         %f\n", minimumPower);
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
