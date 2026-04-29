//Steps:
//1. Open a new RFmx session
//2. Configure the instrument properties Clock Source and Clock Frequency
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select CCDF measurement and enable the traces
//6. Configure CCDF Number of Records and Measurement Interval
//7. Configure CCDF RBW filter
//8. Configure CCDF Threshold
//9. Initiate Measurement
//10. Fetch CCDF Measurements and Traces
//11. Close the RFmx Session

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

   int32 numOfRecords = 1;
   float64 measurementInterval = 1.0e-3;  /* seconds */

   /* RBW settings */
   int32 RBWFilterType = RFMXSPECAN_VAL_CCDF_RBW_FILTER_TYPE_NONE;
   float64 RBW = 100.0e+3;                /* Hz */
   float64 RRCAlpha = 0.010;

   /* Threshold settings */
   int32 thresholdEnabled = RFMXSPECAN_VAL_CCDF_THRESHOLD_ENABLED_FALSE;
   int32 thresholdType = RFMXSPECAN_VAL_CCDF_THRESHOLD_TYPE_RELATIVE;
   float64 thresholdLevel = -20.00;       /* dB or dBm*/

   float64 timeout = 10.0;                /* seconds */

   /* Variables to store the result */
   int32 count = 0;
   float64 meanPower = 0;
   float64 meanPwrPercentile = 0;
   float64 peakPower = 0;
   float64 power10Percentage = 0;
   float64 power1Percentage = 0;
   float64 power0_1Percentage = 0;
   float64 power0_01Percentage = 0;
   float64 power0_001Percentage = 0;
   float64 power0_0001Percentage = 0;

   float64 x0 = 0.0, dx = 0.0;
   float32 *probabilities = (float32 *)NULL;
   float32 *gaussianProbabilities = (float32 *)NULL;
   int32 actualArraySize = 0;

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure CCDF parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_CCDF, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_CCDFCfgNumberOfRecords(instrumentHandle, "", numOfRecords));
   RFmxCheckWarn(RFmxSpecAn_CCDFCfgMeasurementInterval(instrumentHandle, "", measurementInterval));
   RFmxCheckWarn(RFmxSpecAn_CCDFCfgRBWFilter(instrumentHandle, "", RBW, RBWFilterType, RRCAlpha));
   RFmxCheckWarn(RFmxSpecAn_CCDFCfgThreshold(instrumentHandle, "", thresholdEnabled, thresholdLevel, thresholdType));

   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_CCDFFetchPower(instrumentHandle, "", timeout, &meanPower,
      &meanPwrPercentile, &peakPower, &count));
   RFmxCheckWarn(RFmxSpecAn_CCDFFetchBasicPowerProbabilities(instrumentHandle, "", timeout,
      &power10Percentage, &power1Percentage,
      &power0_1Percentage, &power0_01Percentage,
      &power0_001Percentage, &power0_0001Percentage));
   RFmxCheckWarn(RFmxSpecAn_CCDFFetchGaussianProbabilitiesTrace(instrumentHandle, "", timeout, NULL,
      NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      gaussianProbabilities = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (gaussianProbabilities)
      {
         RFmxCheckWarn(RFmxSpecAn_CCDFFetchGaussianProbabilitiesTrace(instrumentHandle, "", timeout,
            &x0, &dx, gaussianProbabilities,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0; x0 = 0.0; dx = 0.0;
   RFmxCheckWarn(RFmxSpecAn_CCDFFetchProbabilitiesTrace(instrumentHandle, "", timeout, NULL, NULL,
      NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      probabilities = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (probabilities)
      {
         RFmxCheckWarn(RFmxSpecAn_CCDFFetchProbabilitiesTrace(instrumentHandle, "", timeout, &x0,
            &dx, probabilities, actualArraySize,
            NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("--------------Power----------------------\n");
   printf(" Mean Power (dBm)           %f\n", meanPower);
   printf(" Mean Power Percentile (%%)  %f\n", meanPwrPercentile);
   printf(" Peak Power (dB)            %f\n", peakPower);
   printf(" Count                      %d\n", count);

   printf("--------------Power Probabilities-------------\n");
   printf(" 10 %% Power (dB)           %f\n", power10Percentage);
   printf(" 1 %% Power (dB)            %f\n", power1Percentage);
   printf(" 0.1 %% Power (dB)          %f\n", power0_1Percentage);
   printf(" 0.01 %% Power (dB)         %f\n", power0_01Percentage);
   printf(" 0.001 %% Power (dB)        %f\n", power0_001Percentage);
   printf(" 0.0001 %% Power (dB)       %f\n", power0_0001Percentage);

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
   if (gaussianProbabilities)
      free(gaussianProbabilities);
   if (probabilities)
      free(probabilities);

   printf("Press any key to exit\n");
   _getch();

   return error;
}
