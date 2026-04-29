//Steps:
//1. Open a new RFmx session
//2. Configure the instrument properties Clock Source and Clock Frequency
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select CHP measurement and enable the traces
//6. Configure CHP Integration BW, Span and Sweep Time
//7. Configure CHP Averaging
//8. Configure CHP RBW filter
//9. Configure CHP FFT
//10. Configure CHP RRC Filter
//11. Initiate Measurement
//12. Fetch CHP Measurements and Traces
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
   char errorMessage[MAX_ERROR_DESCRIPTION];

   char *resourceName = "RFSA";
   char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;        /* Hz */
   float64 referenceLevel = 0.00;         /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */
   float64 frequency = 10.0e+6;           /* Hz */

   float64 integrationBandwidth = 1.0e+6; /* Hz */
   float64 span = 1.0e+6;

   /* RBW settings */
   int32 RBWFilterType = RFMXSPECAN_VAL_CHP_RBW_FILTER_TYPE_GAUSSIAN;
   int32 RBWAuto = RFMXSPECAN_VAL_CHP_RBW_AUTO_TRUE;
   float64 RBW = 10.0e+3;                 /* Hz */

   /* Sweep time settings */
   int32 sweepTimeAuto = RFMXSPECAN_VAL_CHP_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 1.00e-3;   /* seconds */

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_CHP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_CHP_AVERAGING_TYPE_RMS;

   /* RRC filter */
   int32 RRCFilterEnabled = RFMXSPECAN_VAL_CHP_RRC_FILTER_ENABLED_FALSE;
   float64 RRCAlpha = 0.220;

   /* FFT */
   int32 FFTWindow = RFMXSPECAN_VAL_CHP_FFT_WINDOW_FLAT_TOP;
   float64 FFTPadding = -1.0;

   /* Variables to store the measurement results */
   float64 absolutePower = 0;          /* dBm */
   float64 psd = 0;                    /* dBm/Hz */
   float64 relativePower = 0;          /* Hz */
   float64 timeout = 10.0;             /* seconds */

   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = (float32 *)NULL;
   int32 actualArraySize = 0;

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure CHP parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_CHP, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_CHPCfgIntegrationBandwidth(instrumentHandle, "", integrationBandwidth));
   RFmxCheckWarn(RFmxSpecAn_CHPCfgSpan(instrumentHandle, "", span));
   RFmxCheckWarn(RFmxSpecAn_CHPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxSpecAn_CHPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount,
      averagingType));
   RFmxCheckWarn(RFmxSpecAn_CHPCfgRBWFilter(instrumentHandle, "", RBWAuto, RBW, RBWFilterType));
   RFmxCheckWarn(RFmxSpecAn_CHPCfgFFT(instrumentHandle, "", FFTWindow, FFTPadding));
   RFmxCheckWarn(RFmxSpecAn_CHPCfgRRCFilter(instrumentHandle, "", RRCFilterEnabled, RRCAlpha));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_CHPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32)*actualArraySize);

      if (spectrum)
      {
         RFmxCheckWarn(RFmxSpecAn_CHPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }
   RFmxCheckWarn(RFmxSpecAn_CHPFetchCarrierMeasurement(instrumentHandle, "", timeout, &absolutePower,
      &psd, &relativePower));

   printf("Absolute Power (dBm) %f\n", absolutePower);
   printf("PSD (dBm/Hz)         %f\n", psd);

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
   if (spectrum)
      free(spectrum);
   printf("Press any key to exit\n");
   _getch();
   return error;
}
