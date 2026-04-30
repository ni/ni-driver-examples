//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure CHP Integration BW
//5. Configure CHP Averaging
//6. Read CHP Measurement Results
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

   /* Integration bandwidth */
   float64 integrationBandwidth = 1.0e+6; /* Hz */

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_CHP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   /* Variables to store the results */
   float64 absolutePower = 0;             /* dBm */
   float64 PSD = 0;                       /* dBm/Hz */

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure CHP parameters */
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel,
      externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_CHPCfgIntegrationBandwidth(instrumentHandle, "", integrationBandwidth));
   RFmxCheckWarn(RFmxSpecAn_CHPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount,
      RFMXSPECAN_VAL_CHP_AVERAGING_TYPE_RMS));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_CHPRead(instrumentHandle, "", timeout, &absolutePower, &PSD));

   /* Display the results */
   printf("Absolute Power (dBm) %f\n", absolutePower);
   printf("PSD (dBm/Hz)         %f\n", PSD);

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
