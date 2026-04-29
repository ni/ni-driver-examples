//Steps:
//1. Open a new RFmx session
//2. Configure the instrument properties Clock Source and Clock Frequency
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select OBW measurement and enable the traces
//6. Configure OBW Bandwidth Percentange, Span and Sweep Time
//7. Configure OBW Averaging
//8. Configure OBW RBW filter
//9. Configure OBW FFT
//10. Configure OBW Power Units
//11. Initiate Measurement
//12. Fetch OBW Measurement and Traces
//13. Close the RFmx Session

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
   char errorMessage[MAX_ERROR_DESCRIPTION] = { '\0' };

   char *resourceName = "RFSA";
   char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;        /* Hz */
   float64 referenceLevel = 0.00;         /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */
   float64 frequency = 10.0e+6;           /* Hz */

   float64 timeout = 10.0;                /* seconds */

   /* Span */
   float64 span = 1e+6;                   /* Hz */

   /* Bandwidth percentage */
   float64 bwdPercentage = 99.00;         /* % */

   /* Power units (dBm) */
   int32 powerUnits = RFMXSPECAN_VAL_OBW_POWER_UNITS_DBM;      /* dBm */

   /* RBW filter settings */
   int32 RBWFilterType = RFMXSPECAN_VAL_OBW_RBW_FILTER_TYPE_GAUSSIAN;
   float64 RBW = 10e+3;
   int32 RBWAuto = RFMXSPECAN_VAL_OBW_RBW_AUTO_TRUE;

   /* Sweep time */
   int32 sweepTimeAuto = RFMXSPECAN_VAL_OBW_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 1e-3;      /* seconds */

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_OBW_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_OBW_AVERAGING_TYPE_RMS;

   /* FFT */
   int32 FFTWindow = RFMXSPECAN_VAL_OBW_FFT_WINDOW_FLAT_TOP;
   float64 FFTPadding = -1.00;

   /* Variables to store OBW measurement results */
   float64 stopFrequency = 0;          /* Hz */
   float64 startFrequency = 0;         /* Hz */
   float64 occupiedBandwidth = 0;      /* Hz */
   float64 averagePower = 0;           /* dBm or dBm/Hz */
   float64 frequencyResolution = 0;    /* Hz */

   int32 actualArraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrumTrace = NULL;

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure OBW parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_OBW, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_OBWCfgBandwidthPercentage(instrumentHandle, "", bwdPercentage));
   RFmxCheckWarn(RFmxSpecAn_OBWCfgSpan(instrumentHandle, "", span));
   RFmxCheckWarn(RFmxSpecAn_OBWCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxSpecAn_OBWCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxSpecAn_OBWCfgRBWFilter(instrumentHandle, "", RBWAuto, RBW, RBWFilterType));
   RFmxCheckWarn(RFmxSpecAn_OBWCfgFFT(instrumentHandle, "", FFTWindow, FFTPadding));
   RFmxCheckWarn(RFmxSpecAn_OBWCfgPowerUnits(instrumentHandle, "", powerUnits));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_OBWFetchSpectrumTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrumTrace = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (spectrumTrace != NULL)
      {
         RFmxCheckWarn(RFmxSpecAn_OBWFetchSpectrumTrace(instrumentHandle, "", timeout, &x0, &dx, spectrumTrace,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }
   RFmxCheckWarn(RFmxSpecAn_OBWFetchMeasurement(instrumentHandle, "", timeout, &occupiedBandwidth, &averagePower,
      &frequencyResolution, &startFrequency, &stopFrequency));

   /* Display results */
   printf("Occupied Bandwidth (Hz)        %f\n", occupiedBandwidth);
   printf("Average Power (dBm or dBm/Hz)  %f\n", averagePower);
   printf("Frequency Resolution (Hz)      %f\n", frequencyResolution);
   printf("Start Frequency (Hz)           %f\n", startFrequency);
   printf("Stop Frequency (Hz)            %f\n", stopFrequency);
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
   if (spectrumTrace)
      free(spectrumTrace);
   printf("Press any key to exit\n");
   _getch();

   return error;
}
