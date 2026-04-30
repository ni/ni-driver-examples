//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure Harm RBW
//5. Configure Harm Measurement Interval
//6. Configure Harm Number of Harmonics
//7. Configure Harm Averaging
//8. Read Harmonics Measurement Results
//9. Close the RFmx Session

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

   /* Fundamental */
   float64 RBW = 100e+3;                  /* Hz */
   float64 measurementInterval = 1.0e-3;  /* seconds */
   int32 RBWFilterType = RFMXSPECAN_VAL_HARM_RBW_FILTER_TYPE_GAUSSIAN;
   float64 RRCAlpha = 0.1;

   int32 numberOfHarmonics = 3;

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_HARM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_HARM_AVERAGING_TYPE_RMS;

   /* Variables to store the measurement results */
   float64 totalHarmonicDistortion = 0;
   float64 averageFundamentalPower = 0;

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure harmonic parameters */
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_HarmCfgFundamentalRBW(instrumentHandle, "", RBW, RBWFilterType, RRCAlpha));
   RFmxCheckWarn(RFmxSpecAn_HarmCfgFundamentalMeasurementInterval(instrumentHandle, "", measurementInterval));
   RFmxCheckWarn(RFmxSpecAn_HarmCfgNumberOfHarmonics(instrumentHandle, "", numberOfHarmonics));
   RFmxCheckWarn(RFmxSpecAn_HarmCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));

   /* Retrieve measurement results */
   RFmxCheckWarn(RFmxSpecAn_HarmRead(instrumentHandle, "", timeout, &totalHarmonicDistortion,
      &averageFundamentalPower));

   /* Display results */
   printf("Total Harmonic Distortion (%%)    :  %f\n", totalHarmonicDistortion);
   printf("Average Fundamental Power (dBm)  : %f\n", averageFundamentalPower);

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
