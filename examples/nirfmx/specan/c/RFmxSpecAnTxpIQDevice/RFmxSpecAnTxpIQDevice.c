//Steps:
//1. Open a new RFmx session.
//2. Configure the basic instrument properties (Clock Source and Clock Frequency).
//3. Configure Selected Ports.
//4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//6. Configure TXP measurement and enable the traces.
//7. Configure the Measurement Interval.
//8. Configure RBW filter parameters.
//9. Configure Thresholding.
//10. Configure Averaging parameters.
//11. Configure VBW filter parameters.
//12. Initiate Measurement.
//13. Fetch TXP Traces and Measurements.
//14. Close the RFmx Session.

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
   float64 centerFrequency = 1e+9;                                         /* Hz */
   float64 referenceLevel = 0.00;                                          /* dBm */
   float64 externalAttenuation = 0.00;                                     /* dB */
   char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   float64 frequency = 10.0e+6;                                            /* Hz */

   /* Trigger */
   int32 IQPowerEdgeEnabled = RFMXSPECAN_VAL_FALSE;
   float64 IQPowerEdgeLevel = -20.00;                                      /* dBm */
   float64 triggerDelay = 0.0;                                             /* seconds */
   float64 minimumQuietTime = 0.0;                                         /* seconds */

   float64 measurementInterval = 1e-3;                                     /* seconds */

   /* RBW Filter */
   int32 RBWFilterType = RFMXSPECAN_VAL_TXP_RBW_FILTER_TYPE_GAUSSIAN;
   float64 RBW = 100e+3;                                                   /* Hz */
   float64 RRCAlpha = 0.010;

   /* VBW */
   int32 VBWAuto = RFMXSPECAN_VAL_TXP_VBW_FILTER_AUTO_BANDWIDTH_TRUE;
   float64 VBW = 30.0e3;                                                   /* Hz */
   float64 VBWToRBWRatio = 3;

   /* Threshold */
   int32 thresholdEnabled = RFMXSPECAN_VAL_TXP_THRESHOLD_ENABLED_FALSE;
   int32 thresholdType = RFMXSPECAN_VAL_TXP_THRESHOLD_TYPE_RELATIVE;
   float64 thresholdLevel = -20.00;                                        /* (dB or dBm) */

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_TXP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_TXP_AVERAGING_TYPE_RMS;

   /* Variables to store the measurement results */
   float64 averageMeanPower = 0;                                           /* dBm */
   float64 peakToAverageRatio = 0;                                         /* dB */
   float64 maximumPower = 0;                                               /* dBm */
   float64 minimumPower = 0;                                               /* dBm */

   float64 timeout = 10.0;                                                 /* seconds */
   int32 actualArraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32 *powerTrace = NULL;

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeLevel,
      RFMXSPECAN_VAL_IQ_POWER_EDGE_RISING_SLOPE, triggerDelay, RFMXSPECAN_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_MANUAL,
      minimumQuietTime, IQPowerEdgeEnabled));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_TXP, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgMeasurementInterval(instrumentHandle, "", measurementInterval));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgRBWFilter(instrumentHandle, "", RBW, RBWFilterType, RRCAlpha));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgThreshold(instrumentHandle, "", thresholdEnabled, thresholdLevel, thresholdType));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgVBWFilter(instrumentHandle, "", VBWAuto, VBW, VBWToRBWRatio));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_TXPFetchPowerTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      powerTrace = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (powerTrace)
      {
         RFmxCheckWarn(RFmxSpecAn_TXPFetchPowerTrace(instrumentHandle, "", timeout, &x0, &dx, powerTrace, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }
   RFmxCheckWarn(RFmxSpecAn_TXPFetchMeasurement(instrumentHandle, "", timeout, &averageMeanPower, &peakToAverageRatio,
      &maximumPower, &minimumPower));

   /* Display results */
   printf("Average Mean Power(dBm)   %f\n", averageMeanPower);
   printf("Peak to Average Ratio(dB) %f\n", peakToAverageRatio);
   printf("Maximum Power(dBm)        %f\n", maximumPower);
   printf("Minimum Power(dBm)        %f\n", minimumPower);

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
