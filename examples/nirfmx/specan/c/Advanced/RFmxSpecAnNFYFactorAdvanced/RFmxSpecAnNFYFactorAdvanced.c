//Steps:
//1. Open a new RFmx session.
//2. Configure RF Attenuation.
//3. Configure Selected Ports.
//4. Select NF measurement.
//5. Configure Measurement Method.
//6. Configure measurement frequencies
//    6.1. Specify Start Frequency, Stop Frequency and  Frequency Step Size.
//    6.2. Specify Start Frequency, Stop Frequency and Frequency Points.
//    6.3. Specify Frequency List.
//7. Configure DUT Type.
//8. Configure Frequency Converter DUT specific properties.
//9. Configure Measurement Bandwidth.
//10. Configure Measurement Interval.
//11. Configure Averaging.
//12. Configure Calibration loss.
//13. Configure DUT Input Loss.
//14. Configure DUT Output Loss.
//15. Configure RF Preamplifier and Preselector.
//16. Configure Y-Factor Mode.
//17. Configure Y-Factor Noise Source ENR.
//18. Configure Y-Factor Noise Source Settling Time.
//19. Configure Y-Factor Noise Source Loss.
//20. Configure Reference Level
//    20.1. Let measurement recommend a Reference Level.
//    20.2. Manually Configure Reference Level.
//21. Intiate the measurement.
//22. Fetch NF Measurements and Create Graphs.
//23. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

#define NUMBER_OF_POINTS            10

#define RFMXSPECAN_VAL_STEP         0
#define RFMXSPECAN_VAL_POINTS       1
#define RFMXSPECAN_VAL_LIST         2

int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   int32 i = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];

   char *resourceName = "RFSA";
   char *selectedPorts = "";

   int32 RFAttenuationAuto = RFMXSPECAN_VAL_TRUE;
   float64 RFAttenuationValue = 10.00;       /* dB */

   int32 configureFrequencyList = RFMXSPECAN_VAL_STEP;

   float64 startFrequency = 1E+9;            /* Hz */
   float64 stopFrequency = 2E+9;             /* Hz */

   float64 stepSize = 100E6;                 /* Hz */
   int32 numberOfPoints = NUMBER_OF_POINTS;
   float64* frequencyList = NULL;            /* Hz */

   int32 recommend = RFMXSPECAN_VAL_FALSE;
   int32 preampEnabled = RFMXINSTR_VAL_PREAMP_ENABLED;
   int32 preselectorEnabled = RFMXINSTR_VAL_DOWNCONVERTER_PRESELECTOR_ENABLED;

   float64 DUTMaxGain = 0.00;                /* dB */
   float64 DUTMaxNoiseFigure = 0.00;         /* dB */
   float64 referenceLevel = -55.00;          /* dBm */

   int32 DUTType = RFMXSPECAN_VAL_NF_DUT_TYPE_AMPLIFIER;

   float64 LOFrequency = 10.0e6;             /* Hz */
   int32 frequencyContext = RFMXSPECAN_VAL_NF_FREQUENCY_CONVERTER_FREQUENCY_CONTEXT_RF;
   int32 sideband = RFMXSPECAN_VAL_NF_FREQUENCY_CONVERTER_SIDEBAND_LSB;
   float64 imageRejection = 999.99;          /* dB */

   int32 measurementMethod = RFMXSPECAN_VAL_NF_MEASUREMENT_METHOD_Y_FACTOR;
   int32 yFactorMode = RFMXSPECAN_VAL_NF_Y_FACTOR_MODE_MEASURE;

   float64 measurementBandwidth = 100E+3;    /* Hz */
   float64 measurementInterval = 1E-3;       /* seconds */

   float64 settlingTime = 0.00;              /* seconds */

   float64 coldTemperature = 302.80;         /* K */
   float64 offTemperature = 297;             /* K */

   int32 DUTInputLossCompensationEnabled = RFMXSPECAN_VAL_NF_DUT_INPUT_LOSS_COMPENSATION_ENABLED_FALSE;
   float64 DUTInputLossTemperature = 297;    /* K */

   int32 DUTOutputLossCompensationEnabled = RFMXSPECAN_VAL_NF_DUT_OUTPUT_LOSS_COMPENSATION_ENABLED_FALSE;
   float64 DUTOutputLossTemperature = 297;   /* K */

   int32 calibrationLossCompensationEnabled = RFMXSPECAN_VAL_NF_CALIBRATION_LOSS_COMPENSATION_ENABLED_FALSE;
   float64 calibrationLossTemperature = 297; /* K */

   int32 noiseSourceLossCompensationEnabled = RFMXSPECAN_VAL_NF_Y_FACTOR_NOISE_SOURCE_LOSS_COMPENSATION_ENABLED_FALSE;
   float64 noiseSourceLossTemperature = 297; /* K */

   int32 averagingEnabled = RFMXSPECAN_VAL_NF_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   float64 timeout = 10.0;                   /* seconds */

   int32 arraySize = 0;
   int32 actualArraySize = 0;

   float64* calibrationLossFrequency = NULL; /* Hz */
   float64* calibrationLoss = NULL;          /* dB */

   float64* DUTInputLossFrequency = NULL;    /* Hz */
   float64* DUTInputLoss = NULL;             /* dB */

   float64* DUTOutputLossFrequency = NULL;   /* Hz */
   float64* DUTOutputLoss = NULL;            /* dB */

   float64* ENRFrequency = NULL;             /* Hz */
   float64* ENR = NULL;                      /* dB */

   float64* noiseSourceLossFrequency = NULL; /* Hz */
   float64* noiseSourceLoss = NULL;          /* dB */

   float64* measurementYFactor = NULL;       /* dB */
   float64* calibrationYFactor = NULL;       /* dB */

   float64* hotPower = NULL;                 /* dBm */
   float64* coldPower = NULL;                /* dBm */

   float64* analyzerNoiseFigure = NULL;      /* dB */

   float64* DUTNoiseFigure = NULL;           /* dB */
   float64* DUTNoiseTemperature = NULL;      /* K */
   float64* DUTGain = NULL;                  /* dB */


   /* Create new RFmx session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure NF measurement parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgRFAttenuation(instrumentHandle, "", RFAttenuationAuto, RFAttenuationValue));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_NF, RFMXSPECAN_VAL_FALSE));
   RFmxCheckWarn(RFmxSpecAn_NFCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
   switch (configureFrequencyList)
   {
   case RFMXSPECAN_VAL_STEP:
      RFmxCheckWarn(RFmxSpecAn_NFCfgFrequencyList_StartStopStep(instrumentHandle, "", startFrequency, stopFrequency,
         stepSize));
      break;

   case RFMXSPECAN_VAL_POINTS:
      RFmxCheckWarn(RFmxSpecAn_NFCfgFrequencyList_StartStopPoints(instrumentHandle, "", startFrequency,
         stopFrequency, numberOfPoints));
      break;

   case RFMXSPECAN_VAL_LIST:
      RFmxCheckWarn(RFmxSpecAn_NFCfgFrequencyList(instrumentHandle, "", frequencyList, arraySize));
      break;

   default:
      RFmxCheckWarn(RFmxSpecAn_NFCfgFrequencyList_StartStopStep(instrumentHandle, "", startFrequency, stopFrequency,
         stepSize));
   }

   RFmxCheckWarn(RFmxSpecAn_NFSetDUTType(instrumentHandle, "", DUTType));

   RFmxCheckWarn(RFmxSpecAn_NFSetFrequencyConverterLOFrequency(instrumentHandle, "", LOFrequency));
   RFmxCheckWarn(RFmxSpecAn_NFSetFrequencyConverterFrequencyContext(instrumentHandle, "", frequencyContext));
   RFmxCheckWarn(RFmxSpecAn_NFSetFrequencyConverterSideband(instrumentHandle, "", sideband));
   RFmxCheckWarn(RFmxSpecAn_NFSetFrequencyConverterImageRejection(instrumentHandle, "", imageRejection));

   RFmxCheckWarn(RFmxSpecAn_NFCfgMeasurementBandwidth(instrumentHandle, "", measurementBandwidth));
   RFmxCheckWarn(RFmxSpecAn_NFCfgMeasurementInterval(instrumentHandle, "", measurementInterval));

   RFmxCheckWarn(RFmxSpecAn_NFCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));

   RFmxCheckWarn(RFmxSpecAn_NFCfgCalibrationLoss(instrumentHandle, "", calibrationLossCompensationEnabled,
      calibrationLossFrequency, calibrationLoss, calibrationLossTemperature, 0));
   RFmxCheckWarn(RFmxSpecAn_NFCfgDUTInputLoss(instrumentHandle, "", DUTInputLossCompensationEnabled,
      DUTInputLossFrequency, DUTInputLoss, DUTInputLossTemperature, 0));
   RFmxCheckWarn(RFmxSpecAn_NFCfgDUTOutputLoss(instrumentHandle, "", DUTOutputLossCompensationEnabled,
      DUTOutputLossFrequency, DUTOutputLoss, DUTOutputLossTemperature, 0));
   RFmxCheckWarn(RFmxInstr_SetPreampEnabled(instrumentHandle, "", preampEnabled));
   RFmxCheckWarn(RFmxInstr_SetDownconverterPreselectorEnabled(instrumentHandle, "", preselectorEnabled));
   RFmxCheckWarn(RFmxSpecAn_NFCfgYFactorMode(instrumentHandle, "", yFactorMode));
   RFmxCheckWarn(RFmxSpecAn_NFCfgYFactorNoiseSourceENR(instrumentHandle, "", ENRFrequency, ENR, coldTemperature,
      offTemperature, 0));
   RFmxCheckWarn(RFmxSpecAn_NFCfgYFactorNoiseSourceSettlingTime(instrumentHandle, "", settlingTime));
   RFmxCheckWarn(RFmxSpecAn_NFCfgYFactorNoiseSourceLoss(instrumentHandle, "", noiseSourceLossCompensationEnabled,
      noiseSourceLossFrequency, noiseSourceLoss, noiseSourceLossTemperature, 0));
   if (recommend)
   {
      RFmxCheckWarn(RFmxSpecAn_NFRecommendReferenceLevel(instrumentHandle, "", DUTMaxGain, DUTMaxNoiseFigure,
         &referenceLevel));
      printf("Reference Level                    : %f\n", referenceLevel);
   }
   else
   {
      RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   }
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve NF measurement results */
   RFmxCheckWarn(RFmxSpecAn_NFFetchYFactors(instrumentHandle, "", timeout, NULL, NULL, 0, &actualArraySize));

   if (actualArraySize > 0)
   {
      arraySize = actualArraySize;

      measurementYFactor = (float64*)malloc(arraySize * sizeof(float64));
      calibrationYFactor = (float64*)malloc(arraySize * sizeof(float64));
      hotPower = (float64*)malloc(arraySize * sizeof(float64));
      coldPower = (float64*)malloc(arraySize * sizeof(float64));
      analyzerNoiseFigure = (float64*)malloc(arraySize * sizeof(float64));
      DUTNoiseFigure = (float64*)malloc(arraySize * sizeof(float64));
      DUTNoiseTemperature = (float64*)malloc(arraySize * sizeof(float64));
      DUTGain = (float64*)malloc(arraySize * sizeof(float64));
      frequencyList = (float64*)malloc(arraySize * sizeof(float64));
   }

   if (measurementYFactor && calibrationYFactor)
   {
      RFmxCheckWarn(RFmxSpecAn_NFFetchYFactors(instrumentHandle, "", timeout, measurementYFactor,
         calibrationYFactor, arraySize, &actualArraySize));
   }
   else
   {
      printf("malloc failed.\n");
      goto Error;
   }

   if (hotPower && coldPower)
      RFmxCheckWarn(RFmxSpecAn_NFFetchYFactorPowers(instrumentHandle, "", timeout, hotPower, coldPower,
         arraySize, &actualArraySize));
   else
   {
      printf("malloc failed.\n");
      goto Error;
   }

   if (analyzerNoiseFigure)
      RFmxCheckWarn(RFmxSpecAn_NFFetchAnalyzerNoiseFigure(instrumentHandle, "", timeout, analyzerNoiseFigure,
         arraySize, &actualArraySize));
   else
   {
      printf("malloc failed.\n");
      goto Error;
   }

   if (DUTNoiseFigure && DUTNoiseTemperature && DUTGain)
      RFmxCheckWarn(RFmxSpecAn_NFFetchDUTNoiseFigureAndGain(instrumentHandle, "", timeout, DUTNoiseFigure,
         DUTNoiseTemperature, DUTGain, arraySize,
         &actualArraySize));
   else
   {
      printf("malloc failed.\n");
      goto Error;
   }

   if (frequencyList)
      RFmxCheckWarn(RFmxSpecAn_NFGetFrequencyList(instrumentHandle, "", frequencyList, arraySize, &actualArraySize));
   else
   {
      printf("malloc failed.\n");
      goto Error;
   }


   /* Display results */
   printf("\nResults\n");

   for (i = 0; i < arraySize; i++)
   {
      printf("\nResult %d\n", i);
      printf("\nFrequency (Hz)                  %0.3e\n", frequencyList[i]);
      printf("DUT Noise Figure (dB)         %f\n", DUTNoiseFigure[i]);
      printf("DUT Noise Temp (K)            %f\n", DUTNoiseTemperature[i]);
      printf("DUT Gain(dB)                  %f\n", DUTGain[i]);
      printf("Analyzer Noise Figure (dB)    %f\n", analyzerNoiseFigure[i]);
      printf("Hot Power (dBm)               %f\n", hotPower[i]);
      printf("Cold Power (dBm)              %f\n", coldPower[i]);
      printf("Meas Y-Factor (dB)            %f\n", measurementYFactor[i]);
      printf("Cal Y-Factor (dB)             %f\n", calibrationYFactor[i]);
      printf("--------------------------------------------------------------\n");
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
   if (measurementYFactor)
      free(measurementYFactor);

   if (calibrationYFactor)
      free(calibrationYFactor);

   if (hotPower)
      free(hotPower);

   if (coldPower)
      free(coldPower);

   if (analyzerNoiseFigure)
      free(analyzerNoiseFigure);

   if (DUTNoiseFigure)
      free(DUTNoiseFigure);

   if (DUTNoiseTemperature)
      free(DUTNoiseTemperature);

   if (DUTGain)
      free(DUTGain);

   if (frequencyList)
      free(frequencyList);

   if (calibrationLossFrequency)
      free(calibrationLossFrequency);

   if (calibrationLoss)
      free(calibrationLoss);

   if (DUTInputLossFrequency)
      free(DUTInputLossFrequency);

   if (DUTInputLoss)
      free(DUTInputLoss);

   if (DUTOutputLossFrequency)
      free(DUTOutputLossFrequency);

   if (DUTOutputLoss)
      free(DUTOutputLoss);

   if (ENRFrequency)
      free(ENRFrequency);

   if (ENR)
      free(ENR);

   if (noiseSourceLossFrequency)
      free(noiseSourceLossFrequency);

   if (noiseSourceLoss)
      free(noiseSourceLoss);

   printf("Press any key to exit\n");
   _getch();

   return error;
}
