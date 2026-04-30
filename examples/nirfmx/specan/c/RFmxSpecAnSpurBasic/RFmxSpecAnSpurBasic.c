//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Reference Level and External Attenuation)
//4. Select Spur measurement 
//5. Configure Spur Number of Ranges
//6. Configure Spur Start frequency, Stop frequency, RBW filter and Absolute Limit Start for all the ranges
//7. Initiate Measurement
//8. Read Spur Measurement Status
//9. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

#define NUM_OF_RANGES               1

int main(int argc, char *argv[])
{
   int32 i = 0;
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];

   char *resourceName = "RFSA";
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;        /* Hz */
   float64 referenceLevel = 0.00;         /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */

   float64 timeout = 10.0;                /* seconds */

   /* Range List */
   int32 rangeListSize = NUM_OF_RANGES;
   float64 startFrequency[NUM_OF_RANGES];
   float64 stopFrequency[NUM_OF_RANGES];
   int32 rangeEnabled[NUM_OF_RANGES];

   /* RBW Filter */
   int32 RBWFilterAutoBandwidth[NUM_OF_RANGES];
   float64 RBWFilterBandwidth[NUM_OF_RANGES];
   int32 RBWFilterType[NUM_OF_RANGES];

   float64 limit[NUM_OF_RANGES];

   /* Spur measurement status */
   int32 measurementStatus = 0;
   char *status = "Fail";

   for (i = 0; i < rangeListSize; i++)
   {
      startFrequency[i] = 1e+9;        /* Hz */
      stopFrequency[i] = 1.5e+9;       /* Hz */
      rangeEnabled[i] = RFMXSPECAN_VAL_SPUR_RANGE_ENABLED_TRUE;

      RBWFilterAutoBandwidth[i] = RFMXSPECAN_VAL_SPUR_RBW_AUTO_FALSE;
      RBWFilterBandwidth[i] = 30e+3;   /* Hz */
      RBWFilterType[i] = RFMXSPECAN_VAL_SPUR_RBW_FILTER_TYPE_GAUSSIAN;

      limit[i] = -10.00;               /* dBm */
   }

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_SPUR, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgNumberOfRanges(instrumentHandle, "", rangeListSize));

   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeFrequencyArray(instrumentHandle, "", startFrequency, stopFrequency,
      rangeEnabled, rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeRBWArray(instrumentHandle, "", RBWFilterAutoBandwidth, RBWFilterBandwidth,
      RBWFilterType, rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeAbsoluteLimitArray(instrumentHandle, "", NULL, limit, NULL, rangeListSize));

   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Fetch */
   RFmxCheckWarn(RFmxSpecAn_SpurFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));

   /* Display results */
   if (measurementStatus == RFMXSPECAN_VAL_SPUR_MEASUREMENT_STATUS_PASS)
      status = "Pass";
   printf("Measurement Status : %s\n", status);

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
