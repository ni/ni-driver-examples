//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the Center Frequency
//4. Configure the basic instrument properties (Clock Source, Clock Frequency)
//5. Configure the basic signal properties  (Reference Level, External Attenuation and RF Attenuation)
//6. Select IM measurement and enable the traces
//7. Configure Averaging
//8. Configure RBW Filter parameters
//9. Configure Sweep Time
//10. Configure FFT
//11. Configure Frequency definition
//12. Configure Measurement Method
//13. Configure Fundamental tones
//14. Configure Auto Intermods Setup Enabled
//15. Configure Number of Intermods
//16. Configure Intermod (Array)
//17. Initiate Measurement
//18. Fetch IM Measurements and Trace
//19. Close RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096
#define NUMBER_OF_INTERMODS         1

int main()
{
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION] = { '\0' };
   int32 spectrumIndex = 0;
   int32 i = 0;

   char *resourceName = "RFSA";
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;                                               /* Hz */
   float64 referenceLevel = 0.00;                                                /* dBm */
   float64 externalAttenuation = 0.00;                                           /* dB */

   float64 timeout = 10.0;                                                       /* seconds */

   int32 RFAttenuationAuto = RFMXSPECAN_VAL_RF_ATTENUATION_AUTO_TRUE;
   float64 RFAttenuationValue = 10.00;                                           /* dB */

   /* Frequency Reference */
   float64 frequency = 10.0e+6;                                                  /* Hz */
   char *frequencySource = RFMXSPECAN_VAL_ONBOARD_CLOCK_STR;

   /* Fundamental Tones */
   float64 lowerToneFrequency = -1e+6;                                           /* Hz */
   float64 upperToneFrequency = 1e+6;                                            /* Hz */

   /* Frequency Definition */
   int32 frequencyDefinition = RFMXSPECAN_VAL_IM_FREQUENCY_DEFINITION_RELATIVE;

   /* Measurement Method */
   int32 measurementMethod = RFMXSPECAN_VAL_IM_MEASUREMENT_METHOD_NORMAL;

   /* RBW Filter */
   int32 RBWFilterType = RFMXSPECAN_VAL_IM_RBW_FILTER_TYPE_GAUSSIAN;
   int32 RBWAuto = RFMXSPECAN_VAL_IM_RBW_FILTER_AUTO_BANDWIDTH_TRUE;
   float64 RBW = 10.0e+3;                                                        /* Hz */

   /* Sweep Time */
   int32 sweepTimeAuto = RFMXSPECAN_VAL_IM_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 1.00e-3;                                          /* seconds */

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_IM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_IM_AVERAGING_TYPE_RMS;

   /* FFT */
   int32 FFTWindow = RFMXSPECAN_VAL_IM_FFT_WINDOW_FLAT_TOP;
   float64 FFTPadding = -1.0;

   int32 autoIntermodSetupEnabled = RFMXSPECAN_VAL_IM_AUTO_INTERMODS_SETUP_ENABLED_TRUE;
   int32 maximumIntermodOrder = 3;

   int32 actualArraySize = 0;
   int32 intermodSize = 0;
   int32 numberOfSpectrums = 0;
   int32 numberOfIntermods = NUMBER_OF_INTERMODS;
   int32 numElements = NUMBER_OF_INTERMODS;
   int32 intermodEnabled[NUMBER_OF_INTERMODS];
   int32 intermodSide[NUMBER_OF_INTERMODS];
   int32 order[NUMBER_OF_INTERMODS] = { 0 };
   float64 lowerIntermodFrequency[NUMBER_OF_INTERMODS];                          /* Hz */
   float64 upperIntermodFrequency[NUMBER_OF_INTERMODS];                          /* seconds */

    /* Variables to store the measurement results */
   float64 lowerTonePower = 0.0;
   float64 upperTonePower = 0.0;
   int32 *intermodOrder = NULL;
   float64 *lowerIntermodPower = NULL;
   float64 *upperIntermodPower = NULL;
   float64 *worstCaseOutputInterceptPower = NULL;
   float64 *lowerOutputInterceptPower = NULL;
   float64 *upperOutputInterceptPower = NULL;
   float64 *x0 = NULL;
   float64 *dx = NULL;
   float32 **spectrum = NULL;


   for (i = 0; i < numberOfIntermods; i++)
   {
      /* Setup Intermods parameters */
      intermodEnabled[i] = RFMXSPECAN_VAL_IM_INTERMOD_ENABLED_TRUE;
      order[i] = 3;
      intermodSide[i] = RFMXSPECAN_VAL_IM_INTERMOD_SIDE_BOTH;
      lowerIntermodFrequency[i] = -3.0e+6;
      upperIntermodFrequency[i] = 3.0e+6;
   }

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure IM parameters */
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_CfgRFAttenuation(instrumentHandle, "", RFAttenuationAuto, RFAttenuationValue));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_IM,
      RFMXSPECAN_VAL_TRUE));

   RFmxCheckWarn(RFmxSpecAn_IMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxSpecAn_IMCfgRBWFilter(instrumentHandle, "", RBWAuto, RBW, RBWFilterType));
   RFmxCheckWarn(RFmxSpecAn_IMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxSpecAn_IMCfgFFT(instrumentHandle, "", FFTWindow, FFTPadding));
   RFmxCheckWarn(RFmxSpecAn_IMCfgFrequencyDefinition(instrumentHandle, "", frequencyDefinition));
   RFmxCheckWarn(RFmxSpecAn_IMCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
   RFmxCheckWarn(RFmxSpecAn_IMCfgFundamentalTones(instrumentHandle, "", lowerToneFrequency, upperToneFrequency));
   RFmxCheckWarn(RFmxSpecAn_IMCfgAutoIntermodsSetup(instrumentHandle, "", autoIntermodSetupEnabled, maximumIntermodOrder));

   if (autoIntermodSetupEnabled == RFMXSPECAN_VAL_IM_AUTO_INTERMODS_SETUP_ENABLED_FALSE)
   {
      RFmxCheckWarn(RFmxSpecAn_IMCfgNumberOfIntermods(instrumentHandle, "", numberOfIntermods));
      RFmxCheckWarn(RFmxSpecAn_IMCfgIntermodArray(instrumentHandle, "", order, lowerIntermodFrequency, upperIntermodFrequency,
         intermodSide, intermodEnabled, numElements));
   }
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve IM results */
   RFmxCheckWarn(RFmxSpecAn_IMGetNumberOfIntermods(instrumentHandle, "", &numberOfIntermods));
   intermodSize = numberOfIntermods;
   /* Retrieve IM Spectrum numberOfSpectrums according to measurement method */
   if (measurementMethod == RFMXSPECAN_VAL_IM_MEASUREMENT_METHOD_NORMAL)
      numberOfSpectrums = 1;
   else
      numberOfSpectrums = 2 * numberOfIntermods + 2;

   x0 = (float64 *)malloc(sizeof(float64)*numberOfSpectrums);
   dx = (float64 *)malloc(sizeof(float64)*numberOfSpectrums);
   spectrum = (float32 **)malloc(sizeof(float32 *)*numberOfSpectrums);


   /*Allocation of memory for Result attributes */
   intermodOrder = (int32 *)malloc(sizeof(int32)*intermodSize);
   lowerIntermodPower = (float64 *)malloc(sizeof(float64)*intermodSize);
   upperIntermodPower = (float64 *)malloc(sizeof(float64)*intermodSize);
   worstCaseOutputInterceptPower = (float64 *)malloc(sizeof(float64)*intermodSize);
   lowerOutputInterceptPower = (float64 *)malloc(sizeof(float64)*intermodSize);
   upperOutputInterceptPower = (float64 *)malloc(sizeof(float64)*intermodSize);

   RFmxCheckWarn(RFmxSpecAn_IMFetchFundamentalMeasurement(instrumentHandle, "", timeout, &lowerTonePower, &upperTonePower));
   RFmxCheckWarn(RFmxSpecAn_IMFetchIntermodMeasurementArray(instrumentHandle, "", timeout, intermodOrder, lowerIntermodPower,
      upperIntermodPower, intermodSize, &actualArraySize));
   RFmxCheckWarn(RFmxSpecAn_IMFetchInterceptPowerArray(instrumentHandle, "", timeout, intermodOrder,
      worstCaseOutputInterceptPower, lowerOutputInterceptPower, upperOutputInterceptPower, intermodSize, &actualArraySize));

   /* Retrieve IM Spectrum  */
   for (spectrumIndex = 0; spectrumIndex < numberOfSpectrums; spectrumIndex++)
   {
      spectrum[spectrumIndex] = (float32 *)NULL;
      x0[spectrumIndex] = 0.0;
      dx[spectrumIndex] = 0.0;

      RFmxCheckWarn(RFmxSpecAn_IMFetchSpectrum(instrumentHandle, "", timeout, spectrumIndex, NULL, NULL, NULL, 0, &actualArraySize));
      if (actualArraySize > 0)
      {
         spectrum[spectrumIndex] = (float32 *)malloc(sizeof(float32)*actualArraySize);
         if (spectrum[spectrumIndex])
         {
            RFmxCheckWarn(RFmxSpecAn_IMFetchSpectrum(instrumentHandle, "", timeout, spectrumIndex, &x0[spectrumIndex],
               &dx[spectrumIndex], spectrum[spectrumIndex], actualArraySize, NULL));
         }
         else
         {
            printf("malloc failed.\n");
            goto Error;
         }
      }
   }

   /* Display results */
   printf("Fundamental Tone Measurement                 \n");
   printf("Lower Tone Power (dBm)                     :%f\n", lowerTonePower);
   printf("Upper Tone Power (dBm)                     :%f\n", upperTonePower);
   printf("\nIntermod Measurements                      \n");

   for (i = 0; i < intermodSize; i++)
   {
      printf("\nIntermod Measurement                       : %d \n", i);
      printf("Order                                      : %d\n", intermodOrder[i]);
      printf("Lower Intermod Power (dBm)                 :%f\n", lowerIntermodPower[i]);
      printf("Upper Intermod Power (dBm)                 :%f\n", upperIntermodPower[i]);
      printf("Lower Output Intercept Power (dBm)         :%f\n", lowerOutputInterceptPower[i]);
      printf("Upper Output Intercept Power (dBm)         :%f\n", upperOutputInterceptPower[i]);
      printf("Worst Case Output Intercept Power (dBm)    :%f\n", worstCaseOutputInterceptPower[i]);
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
   if (intermodOrder)
      free(intermodOrder);

   if (lowerIntermodPower)
      free(lowerIntermodPower);

   if (upperIntermodPower)
      free(upperIntermodPower);

   if (worstCaseOutputInterceptPower)
      free(worstCaseOutputInterceptPower);

   if (lowerOutputInterceptPower)
      free(lowerOutputInterceptPower);

   if (upperOutputInterceptPower)
      free(upperOutputInterceptPower);

   for (spectrumIndex = 0; spectrumIndex < numberOfSpectrums; spectrumIndex++)
   {
      if (spectrum[spectrumIndex])
         free(spectrum[spectrumIndex]);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
