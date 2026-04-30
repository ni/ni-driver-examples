//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure CCDF Measurement Interval
//5. Configure CCDF RBW
//6. Read CCDF Measurement Results
//7. Close the RFmx Session

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
   float64 measurementInterval = 1.0e-3;  /* seconds */
   float64 RBW = 100.0e+3;                /* Hz */

   /* Variables to store the measurement result */
   int32 count = 0;
   float64 meanPower = 0;
   float64 meanPowerPercentile = 0;
   float64 peakPower = 0;

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure CCDF parameters */
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_CCDF, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_CCDFCfgMeasurementInterval(instrumentHandle, "", measurementInterval));
   RFmxCheckWarn(RFmxSpecAn_CCDFSetRBWFilterBandwidth(instrumentHandle, "", RBW));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_CCDFRead(instrumentHandle, "", timeout, &meanPower, &meanPowerPercentile,
      &peakPower, &count));

   printf(" Mean Power (dBm)          %f\n", meanPower);
   printf(" Mean Power Percentile (%%) %f\n", meanPowerPercentile);
   printf(" Peak Power (dB)           %f\n", peakPower);
   printf(" Count                     %d\n", count);

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
