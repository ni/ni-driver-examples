//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties (Clock Source and Clock Frequency)
//3. Create a Named signal
//4. Configure Selected Ports
//5. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//6. Configure TXP measurement and enable the traces
//7. Configure the Measurement Interval
//8. Configure RBW filter parameters
//9. Configure Averaging parameters
//10. Initiate Measurement
//11. Fetch TXP Traces and Measurements
//12. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING         256

int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   char *resourceName = "RFSA";
   char errorMessage[MAX_ERROR_DESCRIPTION];
   int32 error = 0, lastErrorCode = 0;
   char signalSelectorString[MAX_SELECTOR_STRING];
   char signalAndResultName[MAX_SELECTOR_STRING];

   char *frequencySource = RFMXSPECAN_VAL_ONBOARD_CLOCK_STR;
   float64 frequency = 10e6;

   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;        /* Hz */
   float64 referenceLevel = 0.00;         /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */

   int32 averagingEnabled = RFMXSPECAN_VAL_ACP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_ACP_AVERAGING_TYPE_RMS;

   float64 measurementInterval = 1.0e-3;  /* seconds */
   float64 RBW = 100.00e+3;               /* Hz */
   int32 RBWFilterType = RFMXSPECAN_VAL_TXP_RBW_FILTER_TYPE_GAUSSIAN;
   float64 RRCAlpha = 0.100;

   float64 timeout = 10;                  /* seconds */
   float64 x0 = 0.0, dx = 0.0;
   float32 *powerTrace = NULL;
   int32 actualArraySize = 0;

   float64 averageMeanPower = 0;
   float64 peakToAverageRatio = 0;
   float64 maxPower = 0;
   float64 minPower = 0;

   /*Create a new RFmx Session.*/
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /*Configure measurement parameters.*/
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));

   RFmxSpecAn_BuildSignalString("TxP_Signal", "", MAX_SELECTOR_STRING, signalSelectorString);
   RFmxCheckWarn(RFmxSpecAn_CreateSignalConfiguration(instrumentHandle, signalSelectorString));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, signalSelectorString, selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, signalSelectorString, centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, signalSelectorString, RFMXSPECAN_VAL_TXP, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgMeasurementInterval(instrumentHandle, signalSelectorString, measurementInterval));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgRBWFilter(instrumentHandle, signalSelectorString, RBW, RBWFilterType, RRCAlpha));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgAveraging(instrumentHandle, signalSelectorString, averagingEnabled,
      averagingCount, averagingType));

   RFmxSpecAn_BuildSignalString(signalSelectorString, "TxP_Result", MAX_SELECTOR_STRING, signalAndResultName);
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, signalAndResultName, ""));

   RFmxCheckWarn(RFmxSpecAn_TXPFetchPowerTrace(instrumentHandle, signalAndResultName, timeout, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      powerTrace = (float32 *)malloc(sizeof(float32) * actualArraySize);
      if (powerTrace)
      {
         RFmxCheckWarn(RFmxSpecAn_TXPFetchPowerTrace(instrumentHandle, signalAndResultName, timeout,
            &x0, &dx, powerTrace, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed\n");
      }
   }
   RFmxCheckWarn(RFmxSpecAn_TXPFetchMeasurement(instrumentHandle, signalAndResultName, timeout,
      &averageMeanPower, &peakToAverageRatio,
      &maxPower, &minPower));

   printf("------------------------Measurement-----------------------------\n");
   printf("Average Mean Power    (dBm) %f\n", averageMeanPower);
   printf("Peak to Average Ratio (dB)  %f\n", peakToAverageRatio);
   printf("Maximum Power         (dBm) %f\n", maxPower);
   printf("Minimum Power         (dBm) %f\n", minPower);

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
