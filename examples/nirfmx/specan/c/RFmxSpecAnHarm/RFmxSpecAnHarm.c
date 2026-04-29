//Steps:
//1. Open a new RFmx session.
//2. Configure the instrument properties Clock Source and Clock Frequency.
//3. Configure Selected Ports.
//4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Select Harmonics measurement and enable the traces.
//6. Configure RBW Filter parameters.
//7. Configure Measurement Interval of the Fundamental signal.
//8. Configure Number of Harmonics to measure and Auto Harmonics setup.
//9. Configure the parameters of the Harmonics (Measurement Interval, Order, BW and Harmonics Enabled).
//10. Configure Averaging parameters.
//11. Initiate Measurement.
//12. Fetch Harm Measurements and Traces.
//13. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

/* Maximum size of a selector string */
#define MAX_HARMONIC_STRING         256

#define NUMBER_OF_HARMONICS         3


int main(int argc, char *argv[])
{
   int32 i = 0;
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];
   char harmonicString[MAX_HARMONIC_STRING];

   char *resourceName = "RFSA";

   char *selectedPorts = "";

   float64 centerFrequency = 1.0e+9;                           /* Hz */
   float64 referenceLevel = 0.00;                              /* dBm */
   float64 externalAttenuation = 0.00;                         /* dB */

   char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   float64 frequency = 10.0e+6;                                /* Hz */

   /* Fundamental */
   int32 RBWFilterType = RFMXSPECAN_VAL_HARM_RBW_FILTER_TYPE_GAUSSIAN;
   float64 RBW = 100.0e+3;                                     /* Hz */
   float64 RRCAlpha = 0.010;
   float64 measInterval = 1.0e-3;                              /* seconds */

   int32 autoHarmonicsSetup = RFMXSPECAN_VAL_HARM_AUTO_HARMONICS_SETUP_ENABLED_TRUE;
   int32 numberOfHormonics = NUMBER_OF_HARMONICS;

   /* Setup harmonic parameters */
   int32 enabledArray[NUMBER_OF_HARMONICS];
   int32 harmonicOrderArray[NUMBER_OF_HARMONICS];
   float64 bandwidthArray[NUMBER_OF_HARMONICS];                /* Hz */
   float64 measurementIntervalArray[NUMBER_OF_HARMONICS];      /* seconds */

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_HARM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_HARM_AVERAGING_TYPE_RMS;

   float64 timeout = 10.0;                                     /* seconds */

   /* Variables to store measurement results */
   float64 totalHarmonicDistortion = 0;
   float64 averageFundamentalPower = 0;
   float64 fundamentalFrequency = 0;

   float64 averageRelativePower[NUMBER_OF_HARMONICS] = { 0 };
   float64 averageAbsolutePower[NUMBER_OF_HARMONICS] = { 0 };
   float64 harmonicsRBW[NUMBER_OF_HARMONICS] = { 0 };
   float64 harmonicsFrequency[NUMBER_OF_HARMONICS] = { 0 };

   float64 x0[NUMBER_OF_HARMONICS] = { 0 };
   float64 dx[NUMBER_OF_HARMONICS] = { 0 };
   float32 *power[NUMBER_OF_HARMONICS] = { 0 };
   int32 actualArraySize = 0;
   
   for (i = 0; i < numberOfHormonics; i++)
   {
      enabledArray[i] = RFMXSPECAN_VAL_HARM_HARMONIC_ENABLED_TRUE;
      harmonicOrderArray[i] = i + 1;
      bandwidthArray[i] = 100.0e+3;
      measurementIntervalArray[i] = 1.0e-3;
   }

   /* Create new RFmx session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure harmonic parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_HARMONICS, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_HarmCfgFundamentalRBW(instrumentHandle, "", RBW, RBWFilterType, RRCAlpha));
   RFmxCheckWarn(RFmxSpecAn_HarmCfgFundamentalMeasurementInterval(instrumentHandle, "", measInterval));
   RFmxCheckWarn(RFmxSpecAn_HarmCfgAutoHarmonics(instrumentHandle, "", autoHarmonicsSetup));

   if (autoHarmonicsSetup == RFMXSPECAN_VAL_HARM_AUTO_HARMONICS_SETUP_ENABLED_TRUE)
   {
      RFmxCheckWarn(RFmxSpecAn_HarmCfgNumberOfHarmonics(instrumentHandle, "", numberOfHormonics));
   }
   else
   {
      RFmxCheckWarn(RFmxSpecAn_HarmCfgNumberOfHarmonics(instrumentHandle, "", numberOfHormonics));
      RFmxCheckWarn(RFmxSpecAn_HarmCfgHarmonicArray(instrumentHandle, "", harmonicOrderArray, bandwidthArray,
         enabledArray, measurementIntervalArray, numberOfHormonics));
   }
   RFmxCheckWarn(RFmxSpecAn_HarmCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_HarmFetchTHD(instrumentHandle, "", timeout, &totalHarmonicDistortion,
      &averageFundamentalPower, &fundamentalFrequency));

   RFmxCheckWarn(RFmxSpecAn_HarmFetchHarmonicMeasurementArray(instrumentHandle, "", timeout, averageRelativePower,
      averageAbsolutePower, harmonicsRBW, harmonicsFrequency, numberOfHormonics, NULL));

   for (i = 0; i < numberOfHormonics; i++)
   {
      RFmxSpecAn_BuildHarmonicString2("", i, MAX_HARMONIC_STRING, harmonicString);
      power[i] = (float32 *)NULL;
      x0[i] = 0.0;
      dx[i] = 0.0;

      actualArraySize = 0;
      RFmxCheckWarn(RFmxSpecAn_HarmFetchHarmonicPowerTrace(instrumentHandle, harmonicString, timeout,
         NULL, NULL, NULL, 0, &actualArraySize));
      if (actualArraySize > 0)
      {
         power[i] = (float32 *)malloc(sizeof(float32)*actualArraySize);
         if (power[i])
         {
            RFmxCheckWarn(RFmxSpecAn_HarmFetchHarmonicPowerTrace(instrumentHandle, harmonicString,
               timeout, &x0[i], &dx[i], power[i], actualArraySize, NULL));
         }
         else
         {
            printf("malloc failed.\n");
            goto Error;
         }
      }
   }

   /* Display results */
   printf("Total Harmonic Distortion (%%)     : %f\n", totalHarmonicDistortion);
   printf("Average Fundamental Power (dBm)   : %f\n", averageFundamentalPower);
   printf("Fundamental Frequency (Hz)        : %f\n", fundamentalFrequency);
   printf("\n----------------Harmonics----------------------\n");
   for (i = 0; i < NUMBER_OF_HARMONICS; i++)
   {
      printf("Harmonic %d\n", i + 1);
      printf("Harmonics Frequency    (Hz)      : %f\n", harmonicsFrequency[i]);
      printf("Harmonics RBW          (Hz)      : %f\n", harmonicsRBW[i]);
      printf("Average Absolute Power (dBm)     : %f\n", averageAbsolutePower[i]);
      printf("Average Relative Power (dB)      : %f\n", averageRelativePower[i]);
      printf("---------------------------------------------\n");
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
      RFmxSpecAn_Close(instrumentHandle, RFMXSPECAN_VAL_FALSE);

   /* Free allocated memory */
   for (i = 0; i < numberOfHormonics; i++)
   {
      if (power[i])
         free(power[i]);
   }
   printf("Press any key to exit\n");
   _getch();

   return error;
}
