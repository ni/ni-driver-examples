//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Load all the Instrument and SpecAn attributes from the Configuration File
//4. Configure Frequency Reference
//5. Configure Center Frequency and Reference Level
//The Steps 4 & 5 will override the configurations for Frequency Reference, Center Frequency
//and Reference Level loaded from the rfmxconfig file with the user defined values
//6. Initiate the Measurement
//7. Fetch ACP Measurements and Trace
//8. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char *argv[])
{
   int i = 0;
   niRFmxInstrHandle instrumentHandle = NULL;

   char configurationFilePath[] = "..\\Support\\SpecAn_Configurations.rfmxconfig";

   char errorMessage[MAX_ERROR_DESCRIPTION];
   int32 error = 0, lastErrorCode = 0;

   char *resourceName = "RFSA";
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;           /* Hz */
   float64 referenceLevel = 0.00;            /* dBm */
   float64 externalAttenuation = 0.00;       /* dB */
   char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   float64 frequency = 10.0e+6;              /* Hz */
   float64 timeout = 10.0;                   /* seconds */

   /* Variables to store the results */
   float64 absolutePower;
   int32 offsetMeasArraySize;
   float64 *lowerRelativePower = (float64 *)NULL;
   float64 *upperRelativePower = (float64 *)NULL;
   float64 *lowerAbsolutePower = (float64 *)NULL;
   float64 *upperAbsolutePower = (float64 *)NULL;
   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = (float32 *)NULL;
   int32 actualArraySize;

   /* Create new RFmx session */
   RFmxCheckWarn(RFmxInstr_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Load all the Instrument and SpecAn attributes from the Configuration File */
   RFmxCheckWarn(RFmxInstr_LoadAllConfigurations(instrumentHandle, configurationFilePath,
      RFMXINSTR_VAL_TRUE));

   /* Configure ACP measurements */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource,
      frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel,
      externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */

   /* Get actual size of the array */
   RFmxCheckWarn(RFmxSpecAn_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout,
      NULL, NULL, NULL, NULL, 0,
      &offsetMeasArraySize));
   if (offsetMeasArraySize > 0)
   {
      lowerRelativePower = (float64 *)malloc(sizeof(float64) * offsetMeasArraySize);
      upperRelativePower = (float64 *)malloc(sizeof(float64) * offsetMeasArraySize);
      lowerAbsolutePower = (float64 *)malloc(sizeof(float64) * offsetMeasArraySize);
      upperAbsolutePower = (float64 *)malloc(sizeof(float64) * offsetMeasArraySize);
      /* Fetch the measurements array */
      RFmxCheckWarn(RFmxSpecAn_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout,
         lowerRelativePower, upperRelativePower,
         lowerAbsolutePower, upperAbsolutePower,
         offsetMeasArraySize, NULL));
   }

   RFmxCheckWarn(RFmxSpecAn_ACPFetchCarrierMeasurement(instrumentHandle, "", timeout,
      &absolutePower, NULL, NULL, NULL));

   actualArraySize = 0; x0 = 0.0; dx = 0.0;
   /* Get actual size of the array */
   RFmxCheckWarn(RFmxSpecAn_ACPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
      if (spectrum)
      {
         RFmxCheckWarn(RFmxSpecAn_ACPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("Carrier Measurements: \n");
   printf("Absolute Power (dBm or dBm/Hz)  %f\n", absolutePower);
   printf("---------------------------------------------------\n\n");

   printf("Offset Channel Measurements: \n");
   for (i = 0; i < offsetMeasArraySize; i++)
   {
      printf("Offset  :  %d\n", i);
      printf("Lower Relative Power (dB)            %f\n", lowerRelativePower[i]);
      printf("Upper Relative Power (dB)            %f\n", upperRelativePower[i]);
      printf("Lower Absolute Power (dBm or dBm/Hz) %f\n", lowerAbsolutePower[i]);
      printf("Upper Absolute Power (dBm or dBm/Hz) %f\n", upperAbsolutePower[i]);
      printf("-------------------------------------------------\n");
   }

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
      RFmxInstr_Close(instrumentHandle, RFMXINSTR_VAL_FALSE);
   }
   /* Free allocated memory */
   if (spectrum)
      free(spectrum);
   if (lowerRelativePower)
      free(lowerRelativePower);
   if (upperRelativePower)
      free(upperRelativePower);
   if (upperAbsolutePower)
      free(upperAbsolutePower);
   if (lowerAbsolutePower)
      free(lowerAbsolutePower);
   printf("Press any key to exit\n");
   _getch();

   return error;
}
