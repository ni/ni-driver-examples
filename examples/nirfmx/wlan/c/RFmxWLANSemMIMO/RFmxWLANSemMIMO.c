//Steps:
//1. Open a new RFmx session.
//2. Configure the Frequency Reference properties(Clock Source and Clock Frequency).
//3. Configure Number of Frequency Segment and Receive Chain.
//4. Configure Center Frequency for each Segment.
//5. Configure the basic signal port specific properties(Reference Level and External Attenuation).
//6. Configure Selected Port.
//7. Configure IQ Power Edge Trigger properties(Trigger Delay, IQ Power Edge Level, Min Quiet Time).
//8. Configure Standard and Channel Bandwidth Properties.
//9. Select SEM measurement and enable the traces.
//10. Configure SEM Mask Type.
//11. Configure Averaging parameters.
//12. Configure Sweep Time and Span parameters.
//13. Initiate Measurement.
//14. Fetch SEM Traces and Measurements.
//15. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWLAN.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                     4096

/* Maximum number of devices */
#define NUMBER_OF_DEVICES                         2

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING_LENGTH                256

/* Number of frequency segments and offset chains */
#define NUMBER_OF_FREQUENCY_SEGMENTS              1
#define NUMBER_OF_RECEIVE_CHAINS                  2

void GetCommaSeparatedStringFromArray(char* arrayOfNames, int numOfNames, int nameLength, char* commaSeparatedName)
{
   int i = 0;
   int j = 0;
   while (i < numOfNames)
   {
      int k;
      for (k = 0; k < nameLength; ++k)
      {
         if (*(arrayOfNames + i*nameLength + k) == '\0') break;
         commaSeparatedName[j++] = *(arrayOfNames + i*nameLength + k);
      }
      if (++i != numOfNames)
      {
         commaSeparatedName[j++] = ',';
      }
   }
   commaSeparatedName[j] = '\0';
}

int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };

   int32 error = 0, lastErrorCode = 0;
   int i = 0;
   int j = 0;
   int k = 0;

   char resourceNames[NUMBER_OF_DEVICES][MAX_SELECTOR_STRING_LENGTH] = { "RFSA1", "RFSA2" };
   char commaSeparatedResourceName[MAX_SELECTOR_STRING_LENGTH * 2 + 1];

   char selectedPorts[NUMBER_OF_DEVICES][MAX_SELECTOR_STRING_LENGTH] = { "", "" };

   char *frequencyReferenceSource = RFMXWLAN_VAL_PXI_CLK_STR;
   float64 frequencyReferenceFrequency = 10e6;                                                                 /*(Hz) */

   char segmentString[MAX_SELECTOR_STRING_LENGTH];
   char chainString[MAX_SELECTOR_STRING_LENGTH];

   float64 centerFrequencyArray[NUMBER_OF_DEVICES] = { 5.180000e9, 5.260000e9 };                               /*(Hz) */

   float64 referenceLevelArray[2] = { 0.0, 0.0 };                                                              /*(dBm) */
   float64 externalAttenuationArray[2] = { 0.0, 0.0 };                                                         /*(dB) */

   char portString[NUMBER_OF_DEVICES][MAX_SELECTOR_STRING_LENGTH];

   char selectedPortsString[NUMBER_OF_DEVICES][MAX_SELECTOR_STRING_LENGTH];
   char selectedPortsStringCommaSeparated[MAX_SELECTOR_STRING_LENGTH * 2 + 1];

   int32 IQPowerEdgeEnabled = RFMXWLAN_VAL_TRUE;
   float64 IQPowerEdgeLevel = -20.0;                                                                           /*(dB) */
   float64 triggerDelay = 0.0;                                                                                 /*(s) */
   int32 minimumQuietTimeMode = RFMXWLAN_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 5.0e-6;                                                                          /* (s) */

   int32 standard = RFMXWLAN_VAL_STANDARD_802_11_N;

   float64 channelBandwidth = 20e6;                                                                            /*(Hz) */

   int32 averagingEnabled = RFMXWLAN_VAL_SEM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXWLAN_VAL_SEM_AVERAGING_TYPE_RMS;

   int32 sweepTimeAuto = RFMXWLAN_VAL_SEM_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTime = 1.0e-3;                                                                                 /*(s) */

   int32 spanAuto = RFMXWLAN_VAL_SEM_SPAN_AUTO_TRUE;
   float64 span = 66.0e6;                                                                                      /*(Hz) */

   float64 timeout = 10.0;                                                                                     /*(s) */

   int32 measurementStatus = RFMXWLAN_VAL_SEM_MEASUREMENT_STATUS_FAIL;

   float64 absolutePower[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { 0.0 };                    /*(dBm) */
   float64 relativePower[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { 0.0 };                    /*(dBm) */

   float64 *lowerOffsetMarginRelativePower[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { NULL }; /*(dBm) */
   float64 *lowerOffsetMarginAbsolutePower[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { NULL }; /*(dBm) */
   float64 *lowerOffsetMargin[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { NULL };              /*(dB) */
   float64 *lowerOffsetMarginFrequency[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { NULL };     /*(Hz) */
   int32 *lowerOffsetMeasurementStatus[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { NULL };

   float64 *upperOffsetMarginRelativePower[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { NULL }; /*(dBm) */
   float64 *upperOffsetMarginAbsolutePower[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { NULL }; /*(dBm) */
   float64 *upperOffsetMargin[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { NULL };              /*(dB) */
   float64 *upperOffsetMarginFrequency[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { NULL };     /*(Hz) */
   int32 *upperOffsetMeasurementStatus[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { NULL };

   int32 actualArraySize = 0, arraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { NULL };
   float32 *compositeMask[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { NULL };

   /* Initialize a session */
   GetCommaSeparatedStringFromArray((char *)resourceNames, NUMBER_OF_DEVICES, MAX_SELECTOR_STRING_LENGTH, commaSeparatedResourceName);
   RFmxCheckWarn(RFmxWLAN_Initialize(commaSeparatedResourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxWLAN_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxWLAN_CfgNumberOfFrequencySegmentsAndReceiveChains(instrumentHandle, "",
      NUMBER_OF_FREQUENCY_SEGMENTS, NUMBER_OF_RECEIVE_CHAINS));
   for (i = 0; i < NUMBER_OF_FREQUENCY_SEGMENTS; ++i)
   {
      RFmxCheckWarn(RFmxWLAN_BuildSegmentString("", i, MAX_SELECTOR_STRING_LENGTH, segmentString));
      RFmxCheckWarn(RFmxWLAN_CfgFrequency(instrumentHandle, segmentString, centerFrequencyArray[i]));
   }
   for (i = 0; i < NUMBER_OF_DEVICES; ++i)
   {
      RFmxCheckWarn(RFmxInstr_BuildPortString2("", selectedPorts[i], resourceNames[i], 0, MAX_SELECTOR_STRING_LENGTH, selectedPortsString[i]));
      RFmxCheckWarn(RFmxInstr_BuildPortString2("", "", resourceNames[i], 0, MAX_SELECTOR_STRING_LENGTH, portString[i]));
      RFmxCheckWarn(RFmxWLAN_CfgReferenceLevel(instrumentHandle, portString[i], referenceLevelArray[i]));
      RFmxCheckWarn(RFmxWLAN_CfgExternalAttenuation(instrumentHandle, portString[i], externalAttenuationArray[i]));
   }
   GetCommaSeparatedStringFromArray((char*)selectedPortsString, NUMBER_OF_DEVICES, MAX_SELECTOR_STRING_LENGTH, selectedPortsStringCommaSeparated);
   RFmxCheckWarn(RFmxWLAN_CfgSelectedPortsMultiple(instrumentHandle, "", selectedPortsStringCommaSeparated));
   RFmxCheckWarn(RFmxWLAN_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXWLAN_VAL_IQ_POWER_EDGE_RISING_SLOPE,
      IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
      RFMXWLAN_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, IQPowerEdgeEnabled));
   RFmxCheckWarn(RFmxWLAN_CfgStandard(instrumentHandle, "", standard));
   RFmxCheckWarn(RFmxWLAN_CfgChannelBandwidth(instrumentHandle, "", channelBandwidth));
   RFmxCheckWarn(RFmxWLAN_SelectMeasurements(instrumentHandle, "", RFMXWLAN_VAL_SEM, RFMXWLAN_VAL_TRUE));
   RFmxCheckWarn(RFmxWLAN_SEMCfgMaskType(instrumentHandle, "", RFMXWLAN_VAL_SEM_MASK_TYPE_STANDARD));
   RFmxCheckWarn(RFmxWLAN_SEMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxWLAN_SEMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTime));
   RFmxCheckWarn(RFmxWLAN_SEMCfgSpan(instrumentHandle, "", spanAuto, span));
   RFmxCheckWarn(RFmxWLAN_Initiate(instrumentHandle, "", ""));

   RFmxCheckWarn(RFmxWLAN_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));
   printf("Measurement Status                      : %s\n",
      measurementStatus == RFMXWLAN_VAL_SEM_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
   for (i = 0; i < NUMBER_OF_FREQUENCY_SEGMENTS; ++i)
   {
      RFmxCheckWarn(RFmxWLAN_BuildSegmentString("", i, MAX_SELECTOR_STRING_LENGTH, segmentString));
      for (j = 0; j < NUMBER_OF_RECEIVE_CHAINS; ++j)
      {
         RFmxCheckWarn(RFmxWLAN_BuildChainString(segmentString, j, MAX_SELECTOR_STRING_LENGTH, chainString));

         RFmxCheckWarn(RFmxWLAN_SEMFetchCarrierMeasurement(instrumentHandle, chainString, timeout, &absolutePower[i][j],
            &relativePower[i][j]));

         RFmxCheckWarn(RFmxWLAN_SEMFetchLowerOffsetMarginArray(instrumentHandle, chainString, timeout, NULL,
            NULL, NULL, NULL, NULL, 0, &actualArraySize));
         if (actualArraySize > 0)
         {
            lowerOffsetMeasurementStatus[i][j] = (int32 *)malloc(sizeof(int32) * actualArraySize);
            lowerOffsetMargin[i][j] = (float64 *)malloc(sizeof(float64) * actualArraySize);
            lowerOffsetMarginFrequency[i][j] = (float64 *)malloc(sizeof(float64) * actualArraySize);
            lowerOffsetMarginAbsolutePower[i][j] = (float64 *)malloc(sizeof(float64) * actualArraySize);
            lowerOffsetMarginRelativePower[i][j] = (float64 *)malloc(sizeof(float64) * actualArraySize);

            RFmxCheckWarn(RFmxWLAN_SEMFetchLowerOffsetMarginArray(instrumentHandle, chainString, timeout,
               lowerOffsetMeasurementStatus[i][j], lowerOffsetMargin[i][j], lowerOffsetMarginFrequency[i][j],
               lowerOffsetMarginAbsolutePower[i][j], lowerOffsetMarginRelativePower[i][j], actualArraySize, NULL));
         }

         actualArraySize = 0;
         RFmxCheckWarn(RFmxWLAN_SEMFetchUpperOffsetMarginArray(instrumentHandle, chainString, timeout, NULL,
            NULL, NULL, NULL, NULL, 0, &actualArraySize));
         if (actualArraySize > 0)
         {
            upperOffsetMeasurementStatus[i][j] = (int32 *)malloc(sizeof(int32) * actualArraySize);
            upperOffsetMargin[i][j] = (float64 *)malloc(sizeof(float64) * actualArraySize);
            upperOffsetMarginFrequency[i][j] = (float64 *)malloc(sizeof(float64) * actualArraySize);
            upperOffsetMarginAbsolutePower[i][j] = (float64 *)malloc(sizeof(float64) * actualArraySize);
            upperOffsetMarginRelativePower[i][j] = (float64 *)malloc(sizeof(float64) * actualArraySize);

            RFmxCheckWarn(RFmxWLAN_SEMFetchUpperOffsetMarginArray(instrumentHandle, chainString, timeout,
               upperOffsetMeasurementStatus[i][j], upperOffsetMargin[i][j], upperOffsetMarginFrequency[i][j],
               upperOffsetMarginAbsolutePower[i][j], upperOffsetMarginRelativePower[i][j], actualArraySize, &arraySize));
         }

         actualArraySize = 0;
         RFmxCheckWarn(RFmxWLAN_SEMFetchSpectrum(instrumentHandle, chainString, timeout, NULL, NULL, NULL, NULL,
            0, &actualArraySize));
         if (actualArraySize > 0)
         {
            spectrum[i][j] = (float32 *)malloc(sizeof(float32) * actualArraySize);
            compositeMask[i][j] = (float32 *)malloc(sizeof(float32) * actualArraySize);
            if (spectrum[i][j] && compositeMask[i][j])
            {
               RFmxCheckWarn(RFmxWLAN_SEMFetchSpectrum(instrumentHandle, chainString, timeout, &x0, &dx, spectrum[i][j], compositeMask[i][j],
                  actualArraySize, NULL));
            }
            else
            {
               printf("malloc failed.\n");
               goto Error;
            }
         }
         printf("\n-------Measurement for %s-------\n\n", chainString);
         printf("Carrier Absolute Power (dBm)            : %lf\n", absolutePower[i][j]);

         printf("\n----------Lower Offset Measurements----------\n\n");

         for (k = 0; k < arraySize; k++)
         {
            printf("Offset %d\n", k);
            printf("Measurement Status           : %s\n",
               lowerOffsetMeasurementStatus[i][j][k] == RFMXWLAN_VAL_SEM_LOWER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
            printf("Margin (dB)                  : %lf\n", lowerOffsetMargin[i][j][k]);
            printf("Margin Frequency (Hz)        : %lf\n", lowerOffsetMarginFrequency[i][j][k]);
            printf("Margin Absolute Power (dBm)  : %lf\n\n", lowerOffsetMarginAbsolutePower[i][j][k]);
         }

         printf("\n----------Upper Offset Measurements----------\n\n");
         for (k = 0; k < arraySize; k++)
         {
            printf("Offset %d\n", k);
            printf("Measurement Status           : %s\n",
               upperOffsetMeasurementStatus[i][j][k] == RFMXWLAN_VAL_SEM_UPPER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
            printf("Margin (dB)                  : %lf\n", upperOffsetMargin[i][j][k]);
            printf("Margin Frequency (Hz)        : %lf\n", upperOffsetMarginFrequency[i][j][k]);
            printf("Margin Absolute Power (dBm)  : %lf\n\n", upperOffsetMarginAbsolutePower[i][j][k]);
         }

         printf("\n---------------------------------------------\n\n\n");
      }
   }

Error:
   if (error)
   {
      RFmxWLAN_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("\nERROR: %s\n", errorMessage);
      else
         printf("\nWARNING: %s\n", errorMessage);
   }
   if (instrumentHandle)
   {
      RFmxWLAN_Close(instrumentHandle, RFMXWLAN_VAL_FALSE);
   }

   for (i = 0; i < NUMBER_OF_FREQUENCY_SEGMENTS; ++i)
   {
      for (j = 0; j < NUMBER_OF_RECEIVE_CHAINS; ++j)
      {
         if (lowerOffsetMeasurementStatus[i][j])
            free(lowerOffsetMeasurementStatus[i][j]);
         if (lowerOffsetMarginAbsolutePower[i][j])
            free(lowerOffsetMarginAbsolutePower[i][j]);
         if (lowerOffsetMarginFrequency[i][j])
            free(lowerOffsetMarginFrequency[i][j]);
         if (lowerOffsetMarginRelativePower[i][j])
            free(lowerOffsetMarginRelativePower[i][j]);
         if (lowerOffsetMargin[i][j])
            free(lowerOffsetMargin[i][j]);
         if (upperOffsetMeasurementStatus[i][j])
            free(upperOffsetMeasurementStatus[i][j]);
         if (upperOffsetMarginAbsolutePower[i][j])
            free(upperOffsetMarginAbsolutePower[i][j]);
         if (upperOffsetMarginFrequency[i][j])
            free(upperOffsetMarginFrequency[i][j]);
         if (upperOffsetMarginRelativePower[i][j])
            free(upperOffsetMarginRelativePower[i][j]);
         if (upperOffsetMargin[i][j])
            free(upperOffsetMargin[i][j]);
         if (spectrum[i][j])
            free(spectrum[i][j]);
         if (compositeMask[i][j])
            free(compositeMask[i][j]);
      }
   }

   printf("\n\nPress any key to exit\n");
   _getch();

   return error;
}
