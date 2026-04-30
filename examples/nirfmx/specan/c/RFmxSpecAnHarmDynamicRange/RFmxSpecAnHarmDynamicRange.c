//Steps:
//1. Open a new RFmx session.
//2. Configure the instrument properties Clock Source and Clock Frequency.
//3. Configure Selected Ports.
//4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Select Harmonics measurement and enable the traces.
//6. Configure RBW Filter parameters.
//7. Configure Measurement Interval of the Fundamental.
//8. Configure Auto Harmonics setup.
//9A. If Harmonics Setup is Auto, configure Number of Harmonics.
//9B. If Harmonics Setup is Manual, configure Order, BW and Measurement Interval for each Harmonic using Selector String.
//10. Configure Measurement Method and Noise Compensation Enabled.
//11. Configure Averaging parameters.
//12. Initiate Harmonics Measurment.
//13. Fetch Total Harmonic Distortion[THD].
//14. Fetch Harmonic Measurement results and Power Trace for all the Harmonics using Selector String.
//15. Close the RFmx Session.

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

   float64 centerFrequency = 1e+9;                                      /* Hz */
   float64 referenceLevel = 0.00;                                       /* dBm */
   float64 externalAttenuation = 0.00;                                  /* dB */

   char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   float64 frequency = 10.0e+6;                                         /* Hz */

   /* Fundamental */
   int32 RBWFilterType = RFMXSPECAN_VAL_HARM_RBW_FILTER_TYPE_GAUSSIAN;
   float64 RBW = 100e+3;                                                /* Hz */
   float64 RRCAlpha = 0.010;
   float64 measurementInterval = 1.0e-3;                                /* seconds */

   /* Auto harmonics setup */
   int32 autoHarmonicsSetup = RFMXSPECAN_VAL_HARM_AUTO_HARMONICS_SETUP_ENABLED_TRUE;

   /* Number of harmonics */
   int32 numberOfHarmonics = NUMBER_OF_HARMONICS;

   int32 harmonicEnabled[NUMBER_OF_HARMONICS];
   int32 harmonicOrder[NUMBER_OF_HARMONICS];
   float64 harmonicBandwidth[NUMBER_OF_HARMONICS];                      /* Hz */
   float64 harmonicMeasurementInterval[NUMBER_OF_HARMONICS];            /* seconds */

   int32 measurementMethod = RFMXSPECAN_VAL_HARM_MEASUREMENT_METHOD_DYNAMIC_RANGE;
   int32 noiseCompensationEnabled = RFMXSPECAN_VAL_HARM_NOISE_COMPENSATION_ENABLED_TRUE;

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_HARM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_HARM_AVERAGING_TYPE_RMS;

   float64 timeout = 10.0;                                              /* seconds */

   /* Variables to store the result */
   float64 totalHarmonicDistortion = 0;
   float64 averageFundamentalPower = 0;
   float64 fundamentalFrequency = 0;
   float64 x0[NUMBER_OF_HARMONICS] = { 0 };
   float64 dx[NUMBER_OF_HARMONICS] = { 0 };
   float32 *power[NUMBER_OF_HARMONICS] = { 0 };
   int32 actualArraySize = 0;

   float64 averageRelativePower[NUMBER_OF_HARMONICS];                   /* dB */
   float64 averageAbsolutePower[NUMBER_OF_HARMONICS];                   /* dBm */
   float64 harmonicsRBW[NUMBER_OF_HARMONICS];                           /* Hz */
   float64 harmonicsFrequency[NUMBER_OF_HARMONICS];                     /* Hz */
   
   for (i = 0; i < numberOfHarmonics; i++)
   {
      /* Setup Harmonics parameters */
      harmonicEnabled[i] = RFMXSPECAN_VAL_HARM_HARMONIC_ENABLED_TRUE;
      harmonicOrder[i] = i + 1;
      harmonicBandwidth[i] = 100.0e+3;
      harmonicMeasurementInterval[i] = 1.0e-3;
   }

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure Harmonic parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_HARMONICS, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_HarmCfgFundamentalRBW(instrumentHandle, "", RBW, RBWFilterType, RRCAlpha));
   RFmxCheckWarn(RFmxSpecAn_HarmCfgFundamentalMeasurementInterval(instrumentHandle, "", measurementInterval));
   RFmxCheckWarn(RFmxSpecAn_HarmCfgAutoHarmonics(instrumentHandle, "", autoHarmonicsSetup));
   if (autoHarmonicsSetup)
   {
      RFmxCheckWarn(RFmxSpecAn_HarmCfgNumberOfHarmonics(instrumentHandle, "", numberOfHarmonics));
   }
   else
   {
      RFmxCheckWarn(RFmxSpecAn_HarmCfgNumberOfHarmonics(instrumentHandle, "", numberOfHarmonics));
      RFmxCheckWarn(RFmxSpecAn_HarmCfgHarmonicArray(instrumentHandle, "", harmonicOrder, harmonicBandwidth,
         harmonicEnabled, harmonicMeasurementInterval, numberOfHarmonics));
   }
   RFmxCheckWarn(RFmxSpecAn_HarmSetMeasurementMethod(instrumentHandle, "", measurementMethod));
   RFmxCheckWarn(RFmxSpecAn_HarmSetNoiseCompensationEnabled(instrumentHandle, "", noiseCompensationEnabled));
   RFmxCheckWarn(RFmxSpecAn_HarmCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_HarmFetchTHD(instrumentHandle, "", timeout, &totalHarmonicDistortion,
      &averageFundamentalPower, &fundamentalFrequency));

   RFmxCheckWarn(RFmxSpecAn_HarmFetchHarmonicMeasurementArray(instrumentHandle, "", timeout, averageRelativePower,
      averageAbsolutePower, harmonicsRBW, harmonicsFrequency, numberOfHarmonics, &actualArraySize));

   for (i = 0; i < numberOfHarmonics; i++)
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
            RFmxCheckWarn(RFmxSpecAn_HarmFetchHarmonicPowerTrace(instrumentHandle, harmonicString, timeout,
               &x0[i], &dx[i], power[i], actualArraySize, NULL));
         }
         else
         {
            printf("malloc failed.\n");
            goto Error;
         }
      }
   }

   printf("Measurement\n");
   printf("Total Harmonic Distortion (%%)     : %f\n", totalHarmonicDistortion);
   printf("Average Fundamental Power (dBm)   : %f\n", averageFundamentalPower);
   printf("Fundamental Frequency (Hz)        : %f\n", fundamentalFrequency);
   printf("\n----------------Harmonics----------------------\n");
   for (i = 0; i < numberOfHarmonics; i++)
   {
      printf("Harmonic                          : %d\n", i + 1);
      printf("Harmonics Frequency    (Hz)       : %f\n", harmonicsFrequency[i]);
      printf("Harmonics RBW          (Hz)       : %f\n", harmonicsRBW[i]);
      printf("Average Absolute Power (dBm)      : %f\n", averageAbsolutePower[i]);
      printf("Average Relative Power (dB)       : %f\n", averageRelativePower[i]);
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
   {
      RFmxSpecAn_Close(instrumentHandle, RFMXSPECAN_VAL_FALSE);
   }

   /* Free allocated memory */
   for (i = 0; i < numberOfHarmonics; i++)
   {
      if (power[i])
         free(power[i]);
   }
   printf("Press any key to exit\n");
   _getch();

   return error;
}
