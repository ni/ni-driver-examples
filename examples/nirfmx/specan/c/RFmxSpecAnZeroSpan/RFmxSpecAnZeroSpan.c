//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties (Clock Source and Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select Spectrum measurement and enable the traces
//6. Configure Spectrum RBW filter
//7. Configure Spectrum Span to Zero
//8. Configure Spectrum Sweep Time Interval
//9. Configure Spectrum Averaging
//10. Initiate Measurement
//11. Fetch Spectrum Power Trace and Measurement
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

   /* Sweep time */
   float64 sweepTimeInterval = 1.0e-3;    /* seconds */
   int32 sweepTimeAuto = RFMXSPECAN_VAL_SPECTRUM_SWEEP_TIME_AUTO_FALSE;

   /* RBW Filter */
   int32 RBWFilterType = RFMXSPECAN_VAL_SPECTRUM_RBW_FILTER_TYPE_GAUSSIAN;
   float64 RBW = 10.0e+3;                 /* Hz */
   int32 RBWAuto = RFMXSPECAN_VAL_SPECTRUM_RBW_AUTO_FALSE;

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_SPECTRUM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_SPECTRUM_AVERAGING_TYPE_RMS;

   /* Variables to store measurement data */
   float64 x0 = 0.0, dx = 0.0;
   float32 *powerTrace = (float32 *)NULL;
   int32 actualArraySize = 0;

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure Spectrum parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_SPECTRUM, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgRBWFilter(instrumentHandle, "", RBWAuto, RBW, RBWFilterType));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgSpan(instrumentHandle, "", 0.0));   /* Zero Span */
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /*Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_SpectrumFetchPowerTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      powerTrace = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (powerTrace)
      {
         RFmxCheckWarn(RFmxSpecAn_SpectrumFetchPowerTrace(instrumentHandle, "", timeout, &x0, &dx, powerTrace,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   /* Display results */
   printf("Measurement Complete.\n");
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
   if (powerTrace)
      free(powerTrace);

   printf("Press any key to exit\n");
   _getch();

   return error;
}
