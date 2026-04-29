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
//11. Configure Measurement Method
//12. Configure Fundamental tones
//13. Configure auto setup of third order intermod frequencies
//14. Initiate Measurement
//15. Fetch IM Measurements and Trace
//16. Close RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096
#define MAXIMUM_NUMBER_OF_SPECTRUMS          4


int main()
{
   int32 spectrumIndex = 0;
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION] = { '\0' };

   char *resourceName = "RFSA";
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;                          /* Hz */
   float64 referenceLevel = 0.00;                           /* dBm */
   float64 externalAttenuation = 0.00;                      /* dB */

   float64 timeout = 10.0;                                  /* seconds */

   int32 RFAttenuationAuto = RFMXSPECAN_VAL_RF_ATTENUATION_AUTO_TRUE;
   float64 RFAttenuationValue = 10.00;                      /* dB */

   /* Frequency Reference */
   float64 frequency = 10.0e+6;                             /* Hz */
   char *frequencySource = RFMXSPECAN_VAL_ONBOARD_CLOCK_STR;

   /* Fundamental Tones */
   float64 lowerToneFrequency = -1e+6;                      /* Hz */
   float64 upperToneFrequency = 1e+6;                       /* Hz */

   /* Measurement Method */
   int32 measurementMethod = RFMXSPECAN_VAL_IM_MEASUREMENT_METHOD_NORMAL;

   /* RBW Filter */
   int32 RBWFilterType = RFMXSPECAN_VAL_IM_RBW_FILTER_TYPE_GAUSSIAN;
   int32 RBWAuto = RFMXSPECAN_VAL_IM_RBW_FILTER_AUTO_BANDWIDTH_TRUE;
   float64 RBW = 10.0e+3;                                   /* Hz */

   /* Sweep Time */
   int32 sweepTimeAuto = RFMXSPECAN_VAL_IM_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 1.00e-3;                     /* seconds */

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_IM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_IM_AVERAGING_TYPE_RMS;

   /* FFT */
   int32 FFTWindow = RFMXSPECAN_VAL_IM_FFT_WINDOW_FLAT_TOP;
   float64 FFTPadding = -1.0;

   int32 autoIntermodsSetupEnabled = RFMXSPECAN_VAL_IM_AUTO_INTERMODS_SETUP_ENABLED_TRUE;
   int32 maximumIntermodOrder = 3;
   int32 numberOfSpectrums = MAXIMUM_NUMBER_OF_SPECTRUMS;

   /* Variables to store the measurement results */
   int32 actualArraySize = 0;
   float64 lowerTonePower = 0.0;
   float64 upperTonePower = 0.0;
   int32 intermodOrder = 0;
   float64 lowerIntermodPower = 0.0;
   float64 upperIntermodPower = 0.0;
   float64 worstCaseOutputInterceptPower = 0.0;
   float64 lowerOutputInterceptPower = 0.0;
   float64 upperOutputInterceptPower = 0.0;
   float64 x0[MAXIMUM_NUMBER_OF_SPECTRUMS] = { 0 };
   float64 dx[MAXIMUM_NUMBER_OF_SPECTRUMS] = { 0 };
   float32 *spectrum[MAXIMUM_NUMBER_OF_SPECTRUMS] = { 0 };

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure IM parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_CfgRFAttenuation(instrumentHandle, "", RFAttenuationAuto, RFAttenuationValue));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_IM,
      RFMXSPECAN_VAL_TRUE));

   RFmxCheckWarn(RFmxSpecAn_IMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxSpecAn_IMCfgRBWFilter(instrumentHandle, "", RBWAuto, RBW, RBWFilterType));
   RFmxCheckWarn(RFmxSpecAn_IMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxSpecAn_IMCfgFFT(instrumentHandle, "", FFTWindow, FFTPadding));
   RFmxCheckWarn(RFmxSpecAn_IMCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
   RFmxCheckWarn(RFmxSpecAn_IMCfgFundamentalTones(instrumentHandle, "", lowerToneFrequency, upperToneFrequency));
   RFmxCheckWarn(RFmxSpecAn_IMCfgAutoIntermodsSetup(instrumentHandle, "", autoIntermodsSetupEnabled, maximumIntermodOrder));

   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve IM results */
   RFmxCheckWarn(RFmxSpecAn_IMFetchFundamentalMeasurement(instrumentHandle, "", timeout, &lowerTonePower, &upperTonePower));
   RFmxCheckWarn(RFmxSpecAn_IMFetchIntermodMeasurement(instrumentHandle, "", timeout, &intermodOrder, &lowerIntermodPower,
      &upperIntermodPower));
   RFmxCheckWarn(RFmxSpecAn_IMFetchInterceptPower(instrumentHandle, "", timeout, &intermodOrder,
      &worstCaseOutputInterceptPower, &lowerOutputInterceptPower, &upperOutputInterceptPower));
   /* Retrieve IM Spectrum numberOfSpectrums according to measurement method */
   if (measurementMethod == RFMXSPECAN_VAL_IM_MEASUREMENT_METHOD_NORMAL)
      numberOfSpectrums = 1;

   /* Retrieve IM Spectrum  */
   for (spectrumIndex = 0; spectrumIndex < numberOfSpectrums; spectrumIndex++)
   {
      RFmxCheckWarn(RFmxSpecAn_IMFetchSpectrum(instrumentHandle, "", timeout, spectrumIndex, NULL, NULL, NULL,
         0, &actualArraySize));
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
   printf("Fundamental Tone Measurement            \n");
   printf("Lower Tone Power (dBm)                 :%f\n", lowerTonePower);
   printf("Upper Tone Power (dBm)                 :%f\n", upperTonePower);
   printf("\nIntermod Measurements                  \n");
   printf("Lower Intermod Power (dBm)             :%f\n", lowerIntermodPower);
   printf("Upper Intermod Power (dBm)             :%f\n", upperIntermodPower);
   printf("Lower TOI (dBm)                        :%f\n", lowerOutputInterceptPower);
   printf("Upper TOI (dBm)                        :%f\n", upperOutputInterceptPower);
   printf("Worst Case TOI (dBm)                   :%f\n", worstCaseOutputInterceptPower);

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
   for (spectrumIndex = 0; spectrumIndex < numberOfSpectrums; spectrumIndex++)
   {
      if (spectrum[spectrumIndex])
         free(spectrum[spectrumIndex]);
   }
   printf("Press any key to exit\n");
   _getch();

   return error;
}
