//Steps:
//1. Open a new RFmx session.
//2. Configure Selected Ports.
//3. Select NF measurement.
//4. Configure Measurement Method.
//5. Configure measurement frequencies
//   5.1. Specify Start Frequency, Stop Frequency and  Frequency Step Size.
//   5.2. Specify Start Frequency, Stop Frequency and Frequency Points.
//   5.3. Specify Frequency List.
//6. Configure Measurement Bandwidth.
//7. Configure Measurement Interval.
//8. Configure  Averaging.
//9. Configure Calibration Loss.
//10. Configure DUT Input Loss.
//11. Configure DUT Output Loss.
//12. Configure Cold Source Mode.
//13. Configure Cold Source DUT S-Parameters.
//14. Configure Reference Level
//   14.1. Let measurement recommend a Reference Level.
//   14.2. Manually configure Reference Level.
//15. Initiate the measurement.
//16. Fetch NF Measurements and Create Graphs.
//17. Close RFmx Session.

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

   int32 configureFrequencyList = RFMXSPECAN_VAL_STEP;

   float64 startFrequency = 1E+9;            /* Hz */
   float64 stopFrequency = 2E+9;             /* Hz */

   float64 stepSize = 100E6;
   int32 numberOfPoints = NUMBER_OF_POINTS;
   float64* frequencyList = NULL;

   int32 recommend = RFMXSPECAN_VAL_FALSE;

   float64 DUTMaxGain = 0.00;              /* dB */
   float64 DUTMaxNoiseFigure = 0.00;       /* dB */
   float64 referenceLevel = -55;           /* dBm */

   int32 measurementMethod = RFMXSPECAN_VAL_NF_MEASUREMENT_METHOD_COLD_SOURCE;
   int32 coldSourceMode = RFMXSPECAN_VAL_NF_COLD_SOURCE_MODE_MEASURE;

   float64 measurementBandwidth = 100E+3;     /* Hz */
   float64 measurementInterval = 1E-3;        /* seconds */

   float64* DUTSParametersFrequency = NULL;
   float64* DUTS21 = NULL;
   float64* DUTS12 = NULL;
   float64* DUTS11 = NULL;
   float64* DUTS22 = NULL;

   float64* calibrationLossFrequency = NULL;
   float64* calibrationLoss = NULL;

   float64* DUTOutputLossFrequency = NULL;
   float64* DUTOutputLoss = NULL;

   float64* DUTInputLossFrequency = NULL;
   float64* DUTInputLoss = NULL;

   float64* frequencyListOut = NULL;

   int32 DUTInputLossCompensationEnabled = RFMXSPECAN_VAL_NF_DUT_INPUT_LOSS_COMPENSATION_ENABLED_FALSE;
   float64 DUTInputLossTemperature = 297;     /* K */

   int32 DUTOutputLossCompensationEnabled = RFMXSPECAN_VAL_NF_DUT_OUTPUT_LOSS_COMPENSATION_ENABLED_FALSE;
   float64 DUTOutputLossTemperature = 297;    /* K */

   int32 calibrationLossCompensationEnabled = RFMXSPECAN_VAL_NF_CALIBRATION_LOSS_COMPENSATION_ENABLED_FALSE;
   float64 calibrationLossTemperature = 297;  /* K */

   int32 averagingEnabled = RFMXSPECAN_VAL_NF_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   float64 timeout = 10.0;                 /* seconds */

   int32 arraySize = 0;
   int32 actualArraySize = 0;

   float64* coldSourcePower = NULL;        /* dBm */

   float64* analyzerNoiseFigure = NULL;    /* dB */

   float64* DUTNoiseFigure = NULL;         /* dB */
   float64* DUTNoiseTemperature = NULL;    /* K */
   float64* DUTGain = NULL;                /* dB */


   /* Create new RFmx session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_NF, RFMXSPECAN_VAL_FALSE));
   RFmxCheckWarn(RFmxSpecAn_NFCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
   switch (configureFrequencyList)
   {
   case RFMXSPECAN_VAL_STEP:
      RFmxCheckWarn(RFmxSpecAn_NFCfgFrequencyList_StartStopStep(instrumentHandle, "", startFrequency,
         stopFrequency, stepSize));
      break;

   case RFMXSPECAN_VAL_POINTS:
      RFmxCheckWarn(RFmxSpecAn_NFCfgFrequencyList_StartStopPoints(instrumentHandle, "", startFrequency,
         stopFrequency, numberOfPoints));
      break;

   case RFMXSPECAN_VAL_LIST:
      RFmxCheckWarn(RFmxSpecAn_NFCfgFrequencyList(instrumentHandle, "", frequencyList, arraySize));
      break;

   default:
      RFmxCheckWarn(RFmxSpecAn_NFCfgFrequencyList_StartStopStep(instrumentHandle, "", startFrequency,
         stopFrequency, stepSize));
   }
   RFmxCheckWarn(RFmxSpecAn_NFCfgMeasurementBandwidth(instrumentHandle, "", measurementBandwidth));
   RFmxCheckWarn(RFmxSpecAn_NFCfgMeasurementInterval(instrumentHandle, "", measurementInterval));
   RFmxCheckWarn(RFmxSpecAn_NFCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
   RFmxCheckWarn(RFmxSpecAn_NFCfgCalibrationLoss(instrumentHandle, "", calibrationLossCompensationEnabled,
      calibrationLossFrequency, calibrationLoss, calibrationLossTemperature, 0));
   RFmxCheckWarn(RFmxSpecAn_NFCfgDUTInputLoss(instrumentHandle, "", DUTInputLossCompensationEnabled,
      DUTInputLossFrequency, DUTInputLoss, DUTInputLossTemperature, 0));
   RFmxCheckWarn(RFmxSpecAn_NFCfgDUTOutputLoss(instrumentHandle, "", DUTOutputLossCompensationEnabled,
      DUTOutputLossFrequency, DUTOutputLoss, DUTOutputLossTemperature, 0));
   RFmxCheckWarn(RFmxSpecAn_NFCfgColdSourceMode(instrumentHandle, "", coldSourceMode));

   RFmxCheckWarn(RFmxSpecAn_NFCfgColdSourceDUTSParameters(instrumentHandle, "", DUTSParametersFrequency, DUTS21,
      DUTS12, DUTS11, DUTS22, 0));

   if (recommend)
   {
      RFmxCheckWarn(RFmxSpecAn_NFRecommendReferenceLevel(instrumentHandle, "", DUTMaxGain,
         DUTMaxNoiseFigure, &referenceLevel));
      printf("Reference Level:                  %f\n", referenceLevel);
   }
   else
   {
      RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   }
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve NF measurement results */
   RFmxCheckWarn(RFmxSpecAn_NFFetchColdSourcePower(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      arraySize = actualArraySize;

      coldSourcePower = (float64*)malloc(arraySize * sizeof(float64));
      analyzerNoiseFigure = (float64*)malloc(arraySize * sizeof(float64));
      DUTNoiseFigure = (float64*)malloc(arraySize * sizeof(float64));
      DUTNoiseTemperature = (float64*)malloc(arraySize * sizeof(float64));
      DUTGain = (float64*)malloc(arraySize * sizeof(float64));
      frequencyListOut = (float64*)malloc(arraySize * sizeof(float64));
   }

   if (coldSourcePower)
      RFmxCheckWarn(RFmxSpecAn_NFFetchColdSourcePower(instrumentHandle, "", timeout, coldSourcePower, arraySize, &actualArraySize));
   else
   {
      printf("malloc failed.\n");
      goto Error;
   }

   if (analyzerNoiseFigure)
      RFmxCheckWarn(RFmxSpecAn_NFFetchAnalyzerNoiseFigure(instrumentHandle, "", timeout, analyzerNoiseFigure, arraySize, &actualArraySize));
   else
   {
      printf("malloc failed.\n");
      goto Error;
   }

   if (DUTNoiseFigure && DUTNoiseTemperature && DUTGain)
   {
      RFmxCheckWarn(RFmxSpecAn_NFFetchDUTNoiseFigureAndGain(instrumentHandle, "", timeout, DUTNoiseFigure,
         DUTNoiseTemperature, DUTGain, arraySize,
         &actualArraySize));
   }
   else
   {
      printf("malloc failed.\n");
      goto Error;
   }

   if (frequencyListOut)
      RFmxCheckWarn(RFmxSpecAn_NFGetFrequencyList(instrumentHandle, "", frequencyListOut, arraySize, &actualArraySize));
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
      printf("\nFrequency (Hz)                 %0.3e\n", frequencyListOut[i]);
      printf("DUT Noise Figure (dB)         %f\n", DUTNoiseFigure[i]);
      printf("DUT Noise Temp (K)            %f\n", DUTNoiseTemperature[i]);
      printf("DUT Gain(dB)                  %f\n", DUTGain[i]);
      printf("Analyzer Noise Figure (dB)    %f\n", analyzerNoiseFigure[i]);
      printf("Measured Power (dBm)          %f\n", coldSourcePower[i]);
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
   if (coldSourcePower)
      free(coldSourcePower);

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

   if (frequencyListOut)
      free(frequencyListOut);

   if (DUTSParametersFrequency)
      free(DUTSParametersFrequency);

   if (DUTS21)
      free(DUTS21);

   if (DUTS12)
      free(DUTS12);

   if (DUTS11)
      free(DUTS11);

   if (DUTS22)
      free(DUTS22);

   if (calibrationLossFrequency)
      free(calibrationLossFrequency);

   if (calibrationLoss)
      free(calibrationLoss);

   if (DUTOutputLossFrequency)
      free(DUTOutputLossFrequency);

   if (DUTOutputLoss)
      free(DUTOutputLoss);

   if (DUTInputLossFrequency)
      free(DUTInputLossFrequency);

   if (DUTInputLoss)
      free(DUTInputLoss);

   printf("Press any key to exit\n");
   _getch();

   return error;
}
