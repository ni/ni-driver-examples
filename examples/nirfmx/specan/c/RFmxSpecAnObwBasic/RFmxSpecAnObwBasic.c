//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure OBW Span
//5. Configure OBW Averaging
//6. Read OBW Measurement Results
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

   /* Span */
   float64 span = 1e+6;                   /* Hz */

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_OBW_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_OBW_AVERAGING_TYPE_RMS;

   /* Variables to store OBW measurement results */
   float64 stopFrequency = 0;
   float64 startFrequency = 0;
   float64 occupiedBandwidth = 0;
   float64 averageTotalPower = 0;
   float64 frequencyResolution = 0;

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure OBW measurements */
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_OBWCfgSpan(instrumentHandle, "", span));
   RFmxCheckWarn(RFmxSpecAn_OBWCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));

   /* Retrieve measurement data */
   RFmxCheckWarn(RFmxSpecAn_OBWRead(instrumentHandle, "", timeout, &occupiedBandwidth, &averageTotalPower,
      &frequencyResolution, &startFrequency, &stopFrequency));

   /* Display results */
   printf("Occupied Bandwidth (Hz)   %f\n", occupiedBandwidth);
   printf("Average total Power (dBm) %f\n", averageTotalPower);
   printf("Frequency Resolution (Hz) %f\n", frequencyResolution);
   printf("Start Frequency (Hz)      %f\n", startFrequency);
   printf("Stop Frequency (Hz)       %f\n", stopFrequency);
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
