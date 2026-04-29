//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure Spectrum Span
//5. Configure Spectrum RBW filter
//6. Configure Spectrum Averaging
//7. Read Spectrum Measurement Results
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

   /* Span */
   float64 span = 1.0e+6;                 /* Hz */

   /* RBW filter */
   int32 RBWAuto = RFMXSPECAN_VAL_SPECTRUM_RBW_AUTO_TRUE;
   float64 RBW = 100.0e+3;                /* Hz */
   int32 RBWFilterType = RFMXSPECAN_VAL_SPECTRUM_RBW_FILTER_TYPE_GAUSSIAN;

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_SPECTRUM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_SPECTRUM_AVERAGING_TYPE_RMS;

   /* Variables to fetch the spectrum */
   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = (float32 *)NULL;
   int32 actualArraySize = 0;

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure Spectrum measurement parameters */
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgSpan(instrumentHandle, "", span));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgRBWFilter(instrumentHandle, "", RBWAuto, RBW, RBWFilterType));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_SpectrumRead(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (spectrum)
      {
         RFmxCheckWarn(RFmxSpecAn_SpectrumRead(instrumentHandle, "", timeout, &x0, &dx, spectrum,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }
   /* Display results */
   printf("x0             %f\n", x0);
   printf("dx             %f\n", dx);
   printf("Spectrum size  %d\n", actualArraySize);
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
   if (spectrum)
      free(spectrum);
   printf("Press any key to exit\n");
   _getch();
   return error;
}
