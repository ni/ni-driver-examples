//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties Clock Source, Clock Frequency
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Reference Level and External Attenuation)
//5. Select Spur measurement and enable the traces
//6. Configure Spur Averaging
//7. Configure Spur Number of Ranges
//8. Configure Spur Range Start and Stop frequency
//9. Configure Spur Range RBW filter
//10. Configure Spur Range Limit Mode, Absolute Start and Stop Limit
//11. Configure Spur Range Number of Spurs to Report
//12. Configure Spur Trace Range Index
//13. Initiate Measurement
//14. Fetch Spur Measurements, Traces and Status
//15. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION          4096

/* Maximum size of a selector string */
#define MAX_RANGE_STRING               256

#define NUMBER_OF_RANGE_LIST           1
#define NUMBER_OF_SPURS_TO_REPORT      10


int main(int argc, char *argv[])
{
   int32 i = 0;
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];
   char rangeString[MAX_RANGE_STRING];
   char *resourceName = "RFSA";
   char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   char *selectedPorts = "";
   float64 referenceLevel = 0.00;         /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */
   float64 frequency = 10.0e+6;           /* Hz */

   float64 timeout = 10.0;                /* seconds */

   char *sts = "Fail";
   float64 x0 = 0.0, dx = 0.0;
   float32 *absLimitTrace = (float32 *)NULL;
   float64 z0 = 0.0, dz = 0.0;
   float32 *spectrumTrace = (float32 *)NULL;
   int32 actualArraySize = 0, rangeNumber;

   /* Input array values */
   int32   rangeEnabledArray[NUMBER_OF_RANGE_LIST];
   float64 startFrequencyArray[NUMBER_OF_RANGE_LIST];
   float64 stopFrequencyArray[NUMBER_OF_RANGE_LIST];

   int32 RBWFilterTypeArray[NUMBER_OF_RANGE_LIST];
   int32 RBWFilterAutoBandwidthArray[NUMBER_OF_RANGE_LIST];
   float64 RBWFilterBandwidthArray[NUMBER_OF_RANGE_LIST];

   /* VBW */
   int32 VBWAuto[NUMBER_OF_RANGE_LIST];
   float64 VBW[NUMBER_OF_RANGE_LIST];     /* Hz */
   float64 VBWToRBWRatio[NUMBER_OF_RANGE_LIST];

   /* Detectors */
   int32 detectorType[NUMBER_OF_RANGE_LIST];
   int32 detectorPoints[NUMBER_OF_RANGE_LIST];

   int32 absLimitModeArray[NUMBER_OF_RANGE_LIST];
   float64 absStartLimitArray[NUMBER_OF_RANGE_LIST];
   float64 absStopLimitArray[NUMBER_OF_RANGE_LIST];

   float64 peakThreashold[NUMBER_OF_RANGE_LIST];
   float64 peakExcursion[NUMBER_OF_RANGE_LIST];

   int32 totalSpur = 0;
   int32 numOfSpursToReportArray[NUMBER_OF_RANGE_LIST];

   float64* spurFrequency = (float64*)NULL;        /* Hz */
   float64* spurAmplitude = (float64*)NULL;        /* dBm */
   float64* spurAbsoluteLimit = (float64*)NULL;    /* dBm */
   float64* spurMargin = (float64*)NULL;           /* dB */
   int32* spurRangeIndex = (int32*)NULL;

   int32 rangeMeasurementStatus[NUMBER_OF_RANGE_LIST];
   int32 rangeDetectedSpurs[NUMBER_OF_RANGE_LIST];

   int32 traceRangeIndex = 0;

   int32 averagingEnabled = RFMXSPECAN_VAL_SPUR_AVERAGING_ENABLED_FALSE;
   int32 averagingType = RFMXSPECAN_VAL_SPUR_AVERAGING_TYPE_RMS;
   int32 averagingCount = 10;

   /* Variables to store spur measurements */
   int32 measurementStatus = 0;

   /* Rangelist */
   int32 rangeListSize = NUMBER_OF_RANGE_LIST;

   for (i = 0; i < rangeListSize; i++)
   {
      rangeEnabledArray[i] = RFMXSPECAN_VAL_SPUR_RANGE_ENABLED_TRUE;
      startFrequencyArray[i] = 1e+9;
      stopFrequencyArray[i] = 1.5e+9;

      RBWFilterTypeArray[i] = RFMXSPECAN_VAL_SPUR_RBW_FILTER_TYPE_GAUSSIAN;
      RBWFilterAutoBandwidthArray[i] = RFMXSPECAN_VAL_SPUR_RBW_AUTO_TRUE;
      RBWFilterBandwidthArray[i] = 30e+3;

      VBWAuto[i] = RFMXSPECAN_VAL_SPUR_RANGE_VBW_FILTER_AUTO_BANDWIDTH_TRUE;
      VBW[i] = 30.0e3;
      VBWToRBWRatio[i] = 3;

      detectorType[i] = RFMXSPECAN_VAL_SPUR_RANGE_DETECTOR_TYPE_NONE;
      detectorPoints[i] = 1001;

      absLimitModeArray[i] = RFMXSPECAN_VAL_SPUR_ABSOLUTE_LIMIT_MODE_COUPLE;
      absStartLimitArray[i] = -10.00;
      absStopLimitArray[i] = -10.00;

      peakThreashold[i] = -200;
      peakExcursion[i] = 0.00;

      numOfSpursToReportArray[i] = NUMBER_OF_SPURS_TO_REPORT;
   }

   /* Create new RFmx session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure Spur measurement parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_SPUR, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount,
      averagingType));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgNumberOfRanges(instrumentHandle, "", rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeFrequencyArray(instrumentHandle, "", startFrequencyArray,
      stopFrequencyArray, rangeEnabledArray,
      rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeRBWArray(instrumentHandle, "", RBWFilterAutoBandwidthArray,
      RBWFilterBandwidthArray, RBWFilterTypeArray,
      rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeAbsoluteLimitArray(instrumentHandle, "", absLimitModeArray,
      absStartLimitArray, absStopLimitArray,
      rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeNumberOfSpursToReportArray(instrumentHandle, "", numOfSpursToReportArray,
      rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangePeakCriteriaArray(instrumentHandle, "", peakThreashold, peakExcursion, rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeDetectorArray(instrumentHandle, "", detectorType, detectorPoints, rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgRangeVBWFilterArray(instrumentHandle, "", VBWAuto, VBW, VBWToRBWRatio, rangeListSize));
   RFmxCheckWarn(RFmxSpecAn_SpurCfgTraceRangeIndex(instrumentHandle, "", traceRangeIndex));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve Spur measurement results */
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
      spurRangeIndex = (int32*)malloc(sizeof(int32)*totalSpur);
      if (spurFrequency && spurAmplitude && spurMargin && spurAbsoluteLimit && spurRangeIndex)
      {
         RFmxCheckWarn(RFmxSpecAn_SpurFetchAllSpurs(instrumentHandle, "", timeout, spurFrequency, spurAmplitude,
            spurMargin, spurAbsoluteLimit, spurRangeIndex, totalSpur, &actualArraySize));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   /* Fetch Absolute limit and Spectrum trace */

   rangeNumber = (traceRangeIndex == -1) ? 0 : traceRangeIndex;
   RFmxSpecAn_BuildRangeString2("", rangeNumber, MAX_RANGE_STRING, rangeString);
   RFmxCheckWarn(RFmxSpecAn_SpurFetchRangeAbsoluteLimitTrace(instrumentHandle, rangeString, timeout,
      NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      absLimitTrace = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (absLimitTrace)
      {
         RFmxCheckWarn(RFmxSpecAn_SpurFetchRangeAbsoluteLimitTrace(instrumentHandle, rangeString,
            timeout, &x0, &dx, absLimitTrace,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0, z0 = 0.0; dz = 0.0;
   RFmxCheckWarn(RFmxSpecAn_SpurFetchRangeSpectrumTrace(instrumentHandle, rangeString, timeout,
      NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrumTrace = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (spectrumTrace)
      {
         RFmxCheckWarn(RFmxSpecAn_SpurFetchRangeSpectrumTrace(instrumentHandle, rangeString, timeout,
            &z0, &dz, spectrumTrace, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   /* Status of Spur measurement */
   RFmxCheckWarn(RFmxSpecAn_SpurFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));

   /* Display results */
   if (measurementStatus == RFMXSPECAN_VAL_SPUR_MEASUREMENT_STATUS_PASS)
      sts = "Pass";
   printf("Measurement Status: %s\n", sts);

   printf("\nSpur List\n");
   for (i = 0; i < NUMBER_OF_SPURS_TO_REPORT; i++)
   {
      printf("\nSpur %d\n", i);
      printf("Frequency(Hz)            %f\n", spurFrequency[i]);
      printf("Amplitude(dBm)           %f\n", spurAmplitude[i]);
      printf("Absolute Limit(dBm)      %f\n", spurAbsoluteLimit[i]);
      printf("Margin(dB)               %f\n", spurMargin[i]);
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
   if (absLimitTrace)
      free(absLimitTrace);

   if (spectrumTrace)
      free(spectrumTrace);

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
