//Steps:
//1. Open a new RFmx session.
//2. Configure the frequency reference properties (Clock Source and Clock Frequency).
//3. Configure Number of Frequency Segment and Receive Chain.
//4. Configure the Center Frequency for each Segment.
//5. Configure Selected Port.
//6. Configure Standard and Channel Bandwidth Properties.
//7. Configure Reference Level.
//8. Configure the External Attenuation.
//9. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//10. Select TXP measurement and enable the traces.
//11. Configure the Measurement Interval.
//12. Configure Averaging parameters.
//13. Initiate Measurement.
//14. Fetch TXP Traces and Measurements.
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
   int j=0;
   while(i < numOfNames)
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

   char resourceNames[NUMBER_OF_DEVICES][MAX_SELECTOR_STRING_LENGTH] = { "RFSA1", "RFSA2" };
   char commaSeparatedResourceName[MAX_SELECTOR_STRING_LENGTH * 2 + 1];

   char selectedPorts[NUMBER_OF_DEVICES][MAX_SELECTOR_STRING_LENGTH] = { "", "" };

   char *frequencyReferenceSource = RFMXWLAN_VAL_PXI_CLK_STR;
   float64 frequencyReferenceFrequency = 10e6;                                                /*(Hz) */

   char segmentString[MAX_SELECTOR_STRING_LENGTH];
   char chainString[MAX_SELECTOR_STRING_LENGTH];

   float64 centerFrequencyArray[NUMBER_OF_DEVICES] = { 5.180000e9, 5.260000e9 };              /*(Hz) */

   char portString[NUMBER_OF_DEVICES][MAX_SELECTOR_STRING_LENGTH];

   char selectedPortsString[NUMBER_OF_DEVICES][MAX_SELECTOR_STRING_LENGTH];
   char selectedPortsStringCommaSeparated[MAX_SELECTOR_STRING_LENGTH * 2 + 1];

   int32 standard = RFMXWLAN_VAL_STANDARD_802_11_N;

   float64 channelBandwidth = 20e6;                                                            /*(Hz) */

   int32 autoLevel = RFMXWLAN_VAL_TRUE;
   float64 measurementInterval = 10e-3;                                                        /*(s) */
   float64 referenceLevelArray[2] = { 0.0, 0.0 };                                              /*(dBm) */
   float64 externalAttenuationArray[2] = { 0.0, 0.0 };                                         /*(dB) */

   int32 IQPowerEdgeEnabled = RFMXWLAN_VAL_TRUE;
   float64 IQPowerEdgeLevel = -20.0;                                                           /*(dB) */
   float64 triggerDelay = 0.0;                                                                 /*(s) */
   int32 minimumQuietTimeMode = RFMXWLAN_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 5.0e-6;                                                          /*(s) */

   int32 averagingEnabled = RFMXWLAN_VAL_TXP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   float64 maximumMeasurementInterval = 1e-3;                                                  /*(s) */

   float64 timeout = 10.0;                                                                     /*(s) */

   float64 averagePowerMean[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { 0.0 }; /*(dBm) */
   float64 peakPowerMaximum[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { 0.0 }; /*(dBm) */

   int32 actualArraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32 *power[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { NULL };

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
   }
   GetCommaSeparatedStringFromArray((char *)selectedPortsString, NUMBER_OF_DEVICES, MAX_SELECTOR_STRING_LENGTH, selectedPortsStringCommaSeparated);
   RFmxCheckWarn(RFmxWLAN_CfgSelectedPortsMultiple(instrumentHandle, "", selectedPortsStringCommaSeparated));
   RFmxCheckWarn(RFmxWLAN_CfgStandard(instrumentHandle, "", standard));
   RFmxCheckWarn(RFmxWLAN_CfgChannelBandwidth(instrumentHandle, "", channelBandwidth));
   if (autoLevel)
   {
       RFmxCheckWarn(RFmxWLAN_AutoLevel(instrumentHandle, "", measurementInterval));
   }
   else
   {
      for (i = 0; i < NUMBER_OF_DEVICES; ++i)
      {
         RFmxCheckWarn(RFmxWLAN_CfgReferenceLevel(instrumentHandle, portString[i], referenceLevelArray[i]));
      }
   }
   for (i = 0; i < NUMBER_OF_DEVICES; ++i)
   {
      RFmxCheckWarn(RFmxWLAN_CfgExternalAttenuation(instrumentHandle, portString[i], externalAttenuationArray[i]));
   }
   RFmxCheckWarn(RFmxWLAN_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXWLAN_VAL_IQ_POWER_EDGE_RISING_SLOPE,
      IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
      RFMXWLAN_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, IQPowerEdgeEnabled));
   RFmxCheckWarn(RFmxWLAN_SelectMeasurements(instrumentHandle, "", RFMXWLAN_VAL_TXP, RFMXWLAN_VAL_TRUE));
   RFmxCheckWarn(RFmxWLAN_TXPCfgMaximumMeasurementInterval(instrumentHandle, "", maximumMeasurementInterval));
   RFmxCheckWarn(RFmxWLAN_TXPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
   RFmxCheckWarn(RFmxWLAN_Initiate(instrumentHandle, "", ""));

   for (i = 0; i < NUMBER_OF_FREQUENCY_SEGMENTS; ++i)
   {
      RFmxCheckWarn(RFmxWLAN_BuildSegmentString("", i, MAX_SELECTOR_STRING_LENGTH, segmentString));
      for (j = 0; j < NUMBER_OF_RECEIVE_CHAINS; ++j)
      {
         RFmxCheckWarn(RFmxWLAN_BuildChainString(segmentString, j, MAX_SELECTOR_STRING_LENGTH, chainString));

         RFmxCheckWarn(RFmxWLAN_TXPFetchMeasurement(instrumentHandle, chainString, timeout, &averagePowerMean[i][j], &peakPowerMaximum[i][j]));
         actualArraySize = 0;
         RFmxCheckWarn(RFmxWLAN_TXPFetchPowerTrace(instrumentHandle, chainString, timeout, NULL, NULL, NULL, 0, &actualArraySize));
         if (actualArraySize > 0)
         {
            power[i][j] = (float32 *)malloc(sizeof(float32) * actualArraySize);
            if (power[i][j])
            {
               RFmxCheckWarn(RFmxWLAN_TXPFetchPowerTrace(instrumentHandle, chainString, timeout, &x0, &dx, power[i][j], actualArraySize, NULL));
            }
         }

         printf("\n----------Measurement for %s----------\n\n", chainString);
         printf("Average Power Mean (dBm)          : %lf\n", averagePowerMean[i][j]);
         printf("Peak Power Maximum (dBm)          : %lf\n\n", peakPowerMaximum[i][j]);
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
         if (power[i][j])
            free(power[i][j]);
      }
   }

   printf("\n\nPress any key to exit\n");
   _getch();

   return error;
}