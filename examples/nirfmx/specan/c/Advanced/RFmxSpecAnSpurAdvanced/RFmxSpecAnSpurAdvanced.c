//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties Clock Source, Clock Frequency
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Reference Level and External Attenuation)
//5. Select Spur measurement and enable the traces
//6. Configure Spur Averaging
//7. Configure Spur FFT Window
//8. Configure Spur Trace Range Index
//9. Configure Spur Number of Ranges
//10. Configure Spur Range List properties:
//Start and Stop Frequency, Relative Attenuation, RBW Filter, Absolute Limit and Number of Spurs to Report using Selector String
//11. Initiate Measurement
//12. Fetch Range Status for all Ranges
//13. Use Number of Detected Spurs and Fetch Spur Measurement Results
//14. Fetch Spur Range Traces for all Ranges
//15. Fetch Measurement Status
//16. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

/* Maximum size of range string */
#define MAX_RANGE_STRING            256

#define NUMBER_OF_RANGE_LIST        1
#define NUMBER_OF_SPURS_TO_REPORT   10

typedef struct {
   float64 x0;
   float64 dx;
   float32* y;
}trace_t;

int main(int argc, char *argv[])
{
   int32 i = 0;
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];
   char rangeString[MAX_RANGE_STRING] = { '\0' };

   char *resourceName = "RFSA";
   char *selectedPorts = "";
   char *frequencyReferenceSource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   float64 referenceLevel = 0.00;                        /* dBm */
   float64 externalAttenuation = 0.00;                   /* dB */
   float64 frequencyReferenceFrequency = 10.0e+6;        /* Hz */

   float64 timeout = 10.0;                               /* seconds */

   char *sts = "Fail";

   int32 rangeListSize = NUMBER_OF_RANGE_LIST;

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_SPUR_AVERAGING_ENABLED_FALSE;
   int32 averagingType = RFMXSPECAN_VAL_SPUR_AVERAGING_TYPE_RMS;
   int32 averagingCount = 10;

   /* Trace range index */
   int32 traceRangeIndex = 0;

   /* FFT window */
   int32 FFTWindow = RFMXSPECAN_VAL_SPUR_FFT_WINDOW_FLAT_TOP;

   /* Variables to store the measurement results */
   int32 measurementStatus = 0;
   trace_t absLimitTrace;
   trace_t rangeSpectrumTrace;
   int32 actualArraySize = 0;
   int32 totalSpur = 0;

   int32 rangeEnabled[NUMBER_OF_RANGE_LIST];
   float64 startFrequency[NUMBER_OF_RANGE_LIST];   /* Hz */
   float64 stopFrequency[NUMBER_OF_RANGE_LIST];    /* Hz */
   float64 relAttenuation[NUMBER_OF_RANGE_LIST];   /* dB */

   /* RBW Filter */
   int32 RBWFilterType[NUMBER_OF_RANGE_LIST];
   int32 RBWFilterAutoBandwidth[NUMBER_OF_RANGE_LIST];
   float64 RBWFilterBandwidth[NUMBER_OF_RANGE_LIST];     /* Hz */

   /* VBW */
   int32 VBWAuto[NUMBER_OF_RANGE_LIST];
   float64 VBW[NUMBER_OF_RANGE_LIST];                    /* Hz */
   float64 VBWToRBWRatio[NUMBER_OF_RANGE_LIST];

   /* Detectors */
   int32 detectorType[NUMBER_OF_RANGE_LIST];
   int32 detectorPoints[NUMBER_OF_RANGE_LIST];

   /* Absolute limit */
   int32 absLimitMode[NUMBER_OF_RANGE_LIST];
   float64 absStartLimit[NUMBER_OF_RANGE_LIST];       /* dBm */
   float64 absStopLimit[NUMBER_OF_RANGE_LIST];        /* dBm */

   float64 peakThreshold[NUMBER_OF_RANGE_LIST];       /* dBm */
   float64 peakExcursion[NUMBER_OF_RANGE_LIST];       /* dB */

   int32 numOfSpursToReport[NUMBER_OF_RANGE_LIST];

   float64* spurFrequency = (float64*)NULL;           /* Hz */
   float64* spurAmplitude = (float64*)NULL;           /* dBm */
   float64* spurAbsoluteLimit = (float64*)NULL;       /* dBm */
   float64* spurMargin = (float64*)NULL;              /* dB */
   int32* spurRangeIndex = (int32*)NULL;

   int32	rangeMeasurementStatus[NUMBER_OF_RANGE_LIST];
   int32	rangeDetectedSpurs[NUMBER_OF_RANGE_LIST];

   absLimitTrace.dx = 0;
   absLimitTrace.x0 = 0;
   absLimitTrace.y = 0;

   rangeSpectrumTrace.dx = 0;
   rangeSpectrumTrace.x0 = 0;
   rangeSpectrumTrace.y = 0;

   for (i = 0; i < rangeListSize; i++)
   {
      rangeEnabled[i] = RFMXSPECAN_VAL_SPUR_RANGE_ENABLED_TRUE;
      startFrequency[i] = 1e+9;
      stopFrequency[i] = 1.5e+9;
      relAttenuation[i] = 0.00;

      RBWFilterType[i] = RFMXSPECAN_VAL_SPUR_RBW_FILTER_TYPE_GAUSSIAN;
      RBWFilterAutoBandwidth[i] = RFMXSPECAN_VAL_SPUR_RBW_AUTO_TRUE;
      RBWFilterBandwidth[i] = 30e+3;

      VBWAuto[i] = RFMXSPECAN_VAL_SPUR_RANGE_VBW_FILTER_AUTO_BANDWIDTH_TRUE;
      VBW[i] = 30.0e3;
      VBWToRBWRatio[i] = 3;

      detectorType[i] = RFMXSPECAN_VAL_SPUR_RANGE_DETECTOR_TYPE_NONE;
      detectorPoints[i] = 1001;

      absLimitMode[i] = RFMXSPECAN_VAL_SPUR_ABSOLUTE_LIMIT_MODE_COUPLE;
      absStartLimit[i] = -10.00;
      absStopLimit[i] = -10.00;

      peakThreshold[i] = -200.00;
      peakExcursion[i] = 0.0;
      numOfSpursToReport[i] = NUMBER_OF_SPURS_TO_REPORT;
   }

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure Spur measurement parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_SPUR, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgFFTWindowType(instrumentHandle, "", FFTWindow));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgTraceRangeIndex(instrumentHandle, "", traceRangeIndex));

   RFmxCheckWarn(RFmxSpecAn_SpurCfgNumberOfRanges(instrumentHandle, "", rangeListSize));

   /*array configure*/
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeFrequencyArray(instrumentHandle, "", startFrequency, stopFrequency, rangeEnabled,
      rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeRBWArray(instrumentHandle, "", RBWFilterAutoBandwidth, RBWFilterBandwidth,
      RBWFilterType, rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeRelativeAttenuationArray(instrumentHandle, "", relAttenuation, rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeAbsoluteLimitArray(instrumentHandle, "", absLimitMode, absStartLimit, absStopLimit,
      rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeNumberOfSpursToReportArray(instrumentHandle, "", numOfSpursToReport, rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangePeakCriteriaArray(instrumentHandle, "", peakThreshold, peakExcursion, rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeDetectorArray(instrumentHandle, "", detectorType, detectorPoints, rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeVBWFilterArray(instrumentHandle, "", VBWAuto, VBW, VBWToRBWRatio, rangeListSize));

   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_SpurFetchRangeStatusArray(instrumentHandle, "", timeout,
      rangeMeasurementStatus,
      rangeDetectedSpurs, rangeListSize, &actualArraySize));
   totalSpur = 0;
   for (i = 0; i < rangeListSize; i++)
   {
      totalSpur += rangeDetectedSpurs[i];
   }

   if (totalSpur > 0)
   {
      spurFrequency = (float64*)malloc(sizeof(float64)*totalSpur);
      spurAmplitude = (float64*)malloc(sizeof(float64)*totalSpur);
      spurMargin = (float64*)malloc(sizeof(float64)*totalSpur);
      spurAbsoluteLimit = (float64*)malloc(sizeof(float64)*totalSpur);
      spurRangeIndex = (int32*)malloc(sizeof(float64)*totalSpur);
      if (spurFrequency && spurAmplitude && spurMargin && spurAbsoluteLimit && spurRangeIndex)
      {
         RFmxCheckWarn(RFmxSpecAn_SpurFetchAllSpurs(instrumentHandle, "", timeout, spurFrequency, spurAmplitude, spurMargin,
            spurAbsoluteLimit, spurRangeIndex, totalSpur, &actualArraySize));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   if (traceRangeIndex == -1)
      traceRangeIndex = 0;

   RFmxSpecAn_BuildRangeString2("", traceRangeIndex, MAX_RANGE_STRING, rangeString);


   RFmxCheckWarn(RFmxSpecAn_SpurFetchRangeAbsoluteLimitTrace(instrumentHandle, rangeString, timeout,
      NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      absLimitTrace.y = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (absLimitTrace.y)
      {
         RFmxCheckWarn(RFmxSpecAn_SpurFetchRangeAbsoluteLimitTrace(instrumentHandle, rangeString,
            timeout, &absLimitTrace.x0,
            &absLimitTrace.dx,
            absLimitTrace.y,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxSpecAn_SpurFetchRangeSpectrumTrace(instrumentHandle, rangeString, timeout,
      NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      rangeSpectrumTrace.y = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (rangeSpectrumTrace.y)
      {
         RFmxCheckWarn(RFmxSpecAn_SpurFetchRangeSpectrumTrace(instrumentHandle, rangeString, timeout,
            &rangeSpectrumTrace.x0,
            &rangeSpectrumTrace.dx,
            rangeSpectrumTrace.y,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxSpecAn_SpurFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));

   /* Display Results */
   printf("Measurement\n");
   if (measurementStatus == RFMXSPECAN_VAL_SPUR_MEASUREMENT_STATUS_PASS)
      sts = "Pass";
   printf("Measurement Status          : %s\n", sts);
   printf("Spur List\n");
   for (i = 0; i < totalSpur; i++)
   {
      sts = "Fail";
      if (rangeMeasurementStatus[spurRangeIndex[i]] == RFMXSPECAN_VAL_SPUR_RANGE_STATUS_PASS)
         sts = "Pass";

      printf("Spur                        : %d\n", i + 1);
      printf("Spur Range Index            : %d\n", spurRangeIndex[i]);
      printf("Range Measurement Status    : %s\n", sts);
      printf("Frequency(Hz)               : %f\n", spurFrequency[i]);
      printf("Amplitude(dBm)              : %f\n", spurAmplitude[i]);
      printf("Absolute Limit(dBm)         : %f\n", spurAbsoluteLimit[i]);
      printf("Margin(dB)                  : %f\n", spurMargin[i]);
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
   if (absLimitTrace.y)
      free(absLimitTrace.y);
   if (rangeSpectrumTrace.y)
      free(rangeSpectrumTrace.y);
   if (spurFrequency)
      free(spurFrequency);
   if (spurAmplitude)
      free(spurAmplitude);
   if (spurMargin)
      free(spurMargin);
   if (spurAbsoluteLimit)
      free(spurAbsoluteLimit);
   if (spurRangeIndex)
      free(spurRangeIndex);

   printf("Press any key to exit\n");
   _getch();

   return error;
}
