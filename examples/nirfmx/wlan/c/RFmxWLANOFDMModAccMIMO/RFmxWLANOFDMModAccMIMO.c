//Steps:
//1. Open a new RFmx session.
//2. Configure the Frequency Reference properties(Clock Source and Clock Frequency).
//3. Configure Number of Frequency Segment and Receive Chain.
//4. Configure Center Frequency for each Segment.
//5.Configure Selected Port.
//6. Configure the basic signal port specific properties(Reference Level and External Attenuation).
//7. Configure IQ Power Edge Trigger properties(Trigger Delay, IQ Power Edge Level, Min Quiet Time).
//8. Configure Standard and Channel Bandwidth Properties.
//9. Select OFDMModAcc measurement and enable the traces.
//10. Configure the Measurement Interval.
//11. Configure Frequency Error Estimation Method.
//12. Configure Amplitude Tracking Enabled.
//13. Configure Phase Tracking Enabled.
//14. Configure Symbol Clock Error Correction Enabled.
//15. Configure Channel Estimation Type.
//16. Configure Averaging parameters.
//17. Configure Channel Matrix Power Enabled.
//18. Initiate Measurement.
//19. Fetch OFDMModAcc Measurements.
//19. Fetch User Specific Results based on the PPDU Type.
//20. Close the RFmx Session.

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
         if (*(arrayOfNames + i * nameLength + k) == '\0') break;
         commaSeparatedName[j++] = *(arrayOfNames + i * nameLength + k);
      }
      if (++i != numOfNames)
      {
         commaSeparatedName[j++] = ',';
      }
   }
   commaSeparatedName[j] = '\0';
}


int main(int argc, char* argv[])
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
   float64 frequencyReferenceFrequency = 10e6;                                        /*(Hz) */

   char segmentString[MAX_SELECTOR_STRING_LENGTH];
   char chainString[MAX_SELECTOR_STRING_LENGTH];

   char streamString[MAX_SELECTOR_STRING_LENGTH];

   float64 centerFrequencyArray[NUMBER_OF_DEVICES] = { 5.180000e9, 5.260000e9 };      /*(Hz) */

   char portString[NUMBER_OF_DEVICES][MAX_SELECTOR_STRING_LENGTH];

   char selectedPortsString[NUMBER_OF_DEVICES][MAX_SELECTOR_STRING_LENGTH];
   char selectedPortsStringCommaSeparated[MAX_SELECTOR_STRING_LENGTH * 2 + 1];

   float64 referenceLevelArray[NUMBER_OF_DEVICES] = { 0.0, 0.0 };                     /*(dBm) */
   float64 externalAttenuationArray[NUMBER_OF_DEVICES] = { 0.0, 0.0 };                /*(dB) */

   int32 IQPowerEdgeEnabled = RFMXWLAN_VAL_TRUE;
   float64 IQPowerEdgeLevel = -20.0;                                                  /* (dB) */
   float64 triggerDelay = 0.0;                                                        /* (seconds) */
   int32 minimumQuiteTimeMode = RFMXWLAN_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 5e-6;                                                   /* (seconds) */

   int32 standard = RFMXWLAN_VAL_STANDARD_802_11_N;
   float64 channelBandwidth = 20e06;                                                  /* (Hz) */

   int32 measurementOffset = 0;                                                       /* (symbols) */
   int32 maximumMeasurementLength = 16;                                               /* (symbols) */

   int32 frequencyErrorEstimationMethod = RFMXWLAN_VAL_OFDMMODACC_FREQUENCY_ERROR_ESTIMATION_METHOD_PREAMBLE_AND_PILOTS;
   int32 amplitudeTrackingEnabled = RFMXWLAN_VAL_OFDMMODACC_AMPLITUDE_TRACKING_ENABLED_FALSE;
   int32 phaseTrackingEnabled = RFMXWLAN_VAL_OFDMMODACC_PHASE_TRACKING_ENABLED_TRUE;
   int32 symbolClockErrorCorrectionEnabled = RFMXWLAN_VAL_OFDMMODACC_SYMBOL_CLOCK_ERROR_CORRECTION_ENABLED_TRUE;
   int32 channelEstimationType = RFMXWLAN_VAL_OFDMMODACC_CHANNEL_ESTIMATION_TYPE_REFERENCE;
   int32 channelMatrixPowerEnabled = RFMXWLAN_VAL_OFDMMODACC_CHANNEL_MATRIX_POWER_ENABLED_TRUE;

   int32 averagingEnabled = RFMXWLAN_VAL_OFDMMODACC_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   float64 timeout = 10.0;                                                            /* (seconds) */

   float64 averagePowerMean = 0.0;                                                    /*(dBm) */
   float64 peakPowerMaximum = 0.0;                                                    /*(dBm) */

   float64 compositeRMSEVMMean = 0.0;
   float64 compositeDataRMSEVMMean = 0.0;
   float64 compositePilotRMSEVMMean = 0.0;
   int32 numberOfSymbolsUsed = 0;
   int32 PPDUType = RFMXWLAN_VAL_OFDM_PPDU_TYPE_NON_HT;
   int32 guardIntervalType = 0;
   int32 LSIGParityCheckStatus = RFMXWLAN_VAL_OFDMMODACC_L_SIG_PARITY_CHECK_STATUS_NOT_APPLICABLE;
   int32 SIGCRCStatus = RFMXWLAN_VAL_OFDMMODACC_SIG_CRC_STATUS_NOT_APPLICABLE;
   int32 SIGBCRCStatus = RFMXWLAN_VAL_OFDMMODACC_SIG_B_CRC_STATUS_NOT_APPLICABLE;

   int32 numberOfUsers = 0;
   char userString[MAX_SELECTOR_STRING_LENGTH];
   int32* MCSIndexArray = NULL;
   int32 MCSIndexArrayLength = 0;
   int32* numberOfSpaceTimeStreamsArray = NULL;
   int32 numberOfSpaceTimeStreamsArrayLength = 0;
   int32 spaceTimeStreamOffset = 0;
   int32 numberOfStreamResults = INT_MIN;

   float64 frequencyErrorMeanArray[NUMBER_OF_FREQUENCY_SEGMENTS] = { 0.0 };
   float64 symbolClockErrorMeanArray[NUMBER_OF_FREQUENCY_SEGMENTS] = { 0.0 };

   float64* streamRMSEVMMean[NUMBER_OF_FREQUENCY_SEGMENTS] = { NULL };
   float64* streamDataRMSEVMMean[NUMBER_OF_FREQUENCY_SEGMENTS] = { NULL };
   float64* streamPilotRMSEVMMean[NUMBER_OF_FREQUENCY_SEGMENTS] = { NULL };

   float64 x0 = 0.0;
   float64 dx = 0.0;
   int32 arraySize = 0;
   int32 actualArraySize = 0;

   float32** streamRMSEVMPerSubcarrierMean[NUMBER_OF_FREQUENCY_SEGMENTS] = { NULL };

   NIComplexSingle** pilotConstellation[NUMBER_OF_FREQUENCY_SEGMENTS] = { NULL };
   NIComplexSingle** dataConstellation[NUMBER_OF_FREQUENCY_SEGMENTS] = { NULL };

   float64 crossPowerMean[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { 0.0 };

   float64 relativeIQOriginOffsetMean[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { 0.0 };
   float64 IQGainImbalanceMean[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { 0.0 };
   float64 IQQuadratureErrorMean[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { 0.0 };
   float64 absoluteIQOriginOfsetMean[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { 0.0 };
   float64 IQTimingSkewMean[NUMBER_OF_FREQUENCY_SEGMENTS][NUMBER_OF_RECEIVE_CHAINS] = { 0.0 };

   /* Initialize a session */
   GetCommaSeparatedStringFromArray((char*)resourceNames, NUMBER_OF_DEVICES, MAX_SELECTOR_STRING_LENGTH, commaSeparatedResourceName);
   RFmxCheckWarn(RFmxWLAN_Initialize(commaSeparatedResourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxWLAN_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
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
      IQPowerEdgeLevel, triggerDelay, minimumQuiteTimeMode, minimumQuietTime,
      RFMXWLAN_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, IQPowerEdgeEnabled));


   RFmxCheckWarn(RFmxWLAN_CfgStandard(instrumentHandle, "", standard));
   RFmxCheckWarn(RFmxWLAN_CfgChannelBandwidth(instrumentHandle, "", channelBandwidth));

   RFmxCheckWarn(RFmxWLAN_SelectMeasurements(instrumentHandle, "", RFMXWLAN_VAL_OFDMMODACC, RFMXWLAN_VAL_TRUE));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgMeasurementLength(instrumentHandle, "", measurementOffset, maximumMeasurementLength));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgFrequencyErrorEstimationMethod(instrumentHandle, "", frequencyErrorEstimationMethod));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgAmplitudeTrackingEnabled(instrumentHandle, "", amplitudeTrackingEnabled));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgPhaseTrackingEnabled(instrumentHandle, "", phaseTrackingEnabled));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgSymbolClockErrorCorrectionEnabled(instrumentHandle, "", symbolClockErrorCorrectionEnabled));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgChannelEstimationType(instrumentHandle, "", channelEstimationType));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccSetChannelMatrixPowerEnabled(instrumentHandle, "", channelMatrixPowerEnabled));
   RFmxCheckWarn(RFmxWLAN_Initiate(instrumentHandle, "", ""));

   /* Fetch Results */
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchCompositeRMSEVM(instrumentHandle, "", timeout, &compositeRMSEVMMean,
      &compositeDataRMSEVMMean, &compositePilotRMSEVMMean));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchNumberofSymbolsUsed(instrumentHandle, "", timeout, &numberOfSymbolsUsed));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchPPDUType(instrumentHandle, "", timeout, &PPDUType));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchGuardIntervalType(instrumentHandle, "", timeout, &guardIntervalType));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchLSIGParityCheckStatus(instrumentHandle, "", timeout, &LSIGParityCheckStatus));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchSIGCRCStatus(instrumentHandle, "", timeout, &SIGCRCStatus));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchSIGBCRCStatus(instrumentHandle, "", timeout, &SIGBCRCStatus));

   if (PPDUType == RFMXWLAN_VAL_OFDM_PPDU_TYPE_MU)
   {
      RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchNumberOfUsers(instrumentHandle, "", timeout, &numberOfUsers));
      MCSIndexArrayLength = numberOfUsers;
      MCSIndexArray = (int32*)malloc(MCSIndexArrayLength * sizeof(int32));
      numberOfSpaceTimeStreamsArrayLength = numberOfUsers;
      numberOfSpaceTimeStreamsArray = (int32*)malloc(numberOfSpaceTimeStreamsArrayLength * sizeof(int32));
      if (MCSIndexArray == NULL || numberOfSpaceTimeStreamsArray == NULL)
      {
         printf("malloc failed.\n");
         goto Error;
      }

      for (i = 0; i < numberOfUsers; i++)
      {
         RFmxCheckWarn(RFmxWLAN_BuildUserString("", i, MAX_SELECTOR_STRING_LENGTH, userString));
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchMCSIndex(instrumentHandle, userString, timeout, &MCSIndexArray[i]));
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchNumberOfSpaceTimeStreams(instrumentHandle, userString, timeout, &numberOfSpaceTimeStreamsArray[i]));
         RFmxCheckWarn(RFmxWLAN_OFDMModAccGetResultsSpaceTimeStreamOffset(instrumentHandle, userString, &spaceTimeStreamOffset));
         if ((spaceTimeStreamOffset + numberOfSpaceTimeStreamsArray[i]) > numberOfStreamResults)
         {
            numberOfStreamResults = spaceTimeStreamOffset + numberOfSpaceTimeStreamsArray[i];
         }
      }
   }
   else
   {
      MCSIndexArrayLength = 1;
      MCSIndexArray = (int32*)malloc(MCSIndexArrayLength * sizeof(int32));
      numberOfSpaceTimeStreamsArrayLength = 1;
      numberOfSpaceTimeStreamsArray = (int32*)malloc(numberOfSpaceTimeStreamsArrayLength * sizeof(int32));
      if (MCSIndexArray == NULL || numberOfSpaceTimeStreamsArray == NULL)
      {
         printf("malloc failed.\n");
         goto Error;
      }

      RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchMCSIndex(instrumentHandle, "", timeout, &MCSIndexArray[0]));
      RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchNumberOfSpaceTimeStreams(instrumentHandle, "", timeout, &numberOfSpaceTimeStreamsArray[0]));
      numberOfStreamResults = numberOfSpaceTimeStreamsArray[0];
   }

   for (i = 0; i < NUMBER_OF_FREQUENCY_SEGMENTS; i++)
   {
      RFmxCheckWarn(RFmxWLAN_BuildSegmentString("", i, MAX_SELECTOR_STRING_LENGTH, segmentString));
      RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchFrequencyErrorMean(instrumentHandle, segmentString, timeout, &frequencyErrorMeanArray[i]));
      RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchSymbolClockErrorMean(instrumentHandle, segmentString, timeout, &symbolClockErrorMeanArray[i]));

      streamRMSEVMMean[i] = (float64*)(malloc(numberOfStreamResults * sizeof(float64)));

      streamDataRMSEVMMean[i] = (float64*)(malloc(numberOfStreamResults * sizeof(float64)));

      streamPilotRMSEVMMean[i] = (float64*)(malloc(numberOfStreamResults * sizeof(float64)));

      streamRMSEVMPerSubcarrierMean[i] = (float32**)(malloc(numberOfStreamResults * sizeof(float32*)));

      pilotConstellation[i] = (NIComplexSingle**)(malloc(numberOfStreamResults * sizeof(NIComplexSingle*)));

      dataConstellation[i] = (NIComplexSingle**)(malloc(numberOfStreamResults * sizeof(NIComplexSingle*)));

      if (streamRMSEVMMean[i] == NULL
         || streamDataRMSEVMMean[i] == NULL
         || streamPilotRMSEVMMean[i] == NULL
         || streamRMSEVMPerSubcarrierMean[i] == NULL
         || pilotConstellation[i] == NULL
         || dataConstellation[i] == NULL)
      {
         printf("malloc failed. \n");
         goto  Error;
      }

      for (j = 0; j < numberOfStreamResults; j++)
      {
         RFmxCheckWarn(RFmxWLAN_BuildStreamString(segmentString, j, MAX_SELECTOR_STRING_LENGTH, streamString));
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchStreamRMSEVM(
            instrumentHandle,
            streamString,
            timeout,
            &streamRMSEVMMean[i][j],
            &streamDataRMSEVMMean[i][j],
            &streamPilotRMSEVMMean[i][j]));

         actualArraySize = 0;
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchStreamRMSEVMPerSubcarrierMeanTrace(
            instrumentHandle,
            streamString,
            timeout,
            NULL,
            NULL,
            NULL,
            0,
            &actualArraySize));
         if (actualArraySize > 0)
         {
            streamRMSEVMPerSubcarrierMean[i][j] = (float32*)(malloc(actualArraySize * sizeof(float32)));
            if (streamRMSEVMPerSubcarrierMean[i][j] == NULL)
            {
               printf("malloc failed. \n");
               goto  Error;
            }
            RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchStreamRMSEVMPerSubcarrierMeanTrace(
               instrumentHandle,
               streamString,
               timeout,
               &x0,
               &dx,
               streamRMSEVMPerSubcarrierMean[i][j],
               actualArraySize,
               NULL));
         }

         actualArraySize = 0;
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchPilotConstellationTrace(
            instrumentHandle,
            streamString,
            timeout,
            NULL,
            0,
            &actualArraySize));
         if (actualArraySize > 0)
         {
            pilotConstellation[i][j] = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
            if (pilotConstellation[i][j] == NULL)
            {
               RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchPilotConstellationTrace(
                  instrumentHandle,
                  streamString,
                  timeout,
                  pilotConstellation[i][j],
                  actualArraySize,
                  NULL));
            }
         }

         actualArraySize = 0;
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchDataConstellationTrace(instrumentHandle, streamString, timeout, NULL, 0, &actualArraySize));
         if (actualArraySize > 0)
         {
            dataConstellation[i][j] = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
            if (dataConstellation[i][j] == NULL)
            {
               RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchDataConstellationTrace(
                  instrumentHandle,
                  streamString,
                  timeout,
                  dataConstellation[i][j],
                  actualArraySize,
                  NULL));
            }
         }
      }

      for (j = 0; j < NUMBER_OF_RECEIVE_CHAINS; j++)
      {
         RFmxCheckWarn(RFmxWLAN_BuildChainString(segmentString, j, MAX_SELECTOR_STRING_LENGTH, chainString));
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchCrossPower(instrumentHandle, chainString, timeout, &crossPowerMean[i][j]));
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchIQImpairments(
            instrumentHandle,
            chainString,
            timeout,
            &relativeIQOriginOffsetMean[i][j],
            &IQGainImbalanceMean[i][j],
            &IQQuadratureErrorMean[i][j],
            &absoluteIQOriginOfsetMean[i][j],
            &IQTimingSkewMean[i][j]));
      }
   }

   printf("-----------------------EVM-----------------------\n\n");
   printf("------------------Composite EVM------------------\n");
   printf("RMS EVM Mean (dB)                       : %lf\n", compositeRMSEVMMean);
   printf("Data RMS EVM Mean (dB)                  : %lf\n", compositeDataRMSEVMMean);
   printf("Pilot RMS EVM Mean (dB)                 : %lf\n\n", compositePilotRMSEVMMean);
   printf("Number of Symbols Used                  : %d\n\n", numberOfSymbolsUsed);
   printf("\n--------------------------------------------------\n\n\n");
   printf("--------------------PPDU Info--------------------\n");
   switch (PPDUType)
   {
   case 0: printf("PPDU Type                               : Non-HT\n");
      break;
   case 1: printf("PPDU Type                               : Mixed\n");
      break;
   case 2: printf("PPDU Type                               : GreenField\n");
      break;
   case 3: printf("PPDU Type                               : SU\n");
      break;
   case 4: printf("PPDU Type                               : MU\n");
      break;
   case 5: printf("PPDU Type                               : Extended Range SU\n");
      break;
   case 6: printf("PPDU Type                               : Trigger-Based\n");
      break;
   }
   if (PPDUType == RFMXWLAN_VAL_OFDM_PPDU_TYPE_MU)
   {
      for (i = 0; i < numberOfUsers; i++)
      {
         printf("\nNSTS %d                                  : %d\n", i, numberOfSpaceTimeStreamsArray[i]);
         printf("MCS Index %d                             : %d\n\n", i, MCSIndexArray[i]);
      }
   }
   else
   {
      printf("NSTS                                    : %d\n", numberOfSpaceTimeStreamsArray[0]);
      printf("MCS Index                               : %d\n", MCSIndexArray[0]);
   }
   switch (guardIntervalType)
   {
   case 0: printf("Guard Interval Type                     : 1/4\n");
      break;
   case 1: printf("Guard Interval Type                     : 1/8\n");
      break;
   case 2: printf("Guard Interval Type                     : 1/16\n");
      break;
   }
   switch (LSIGParityCheckStatus)
   {
   case -1: printf("L-SIG Parity Check Status              : Not Applicable\n");
      break;
   case 0: printf("L-SIG Parity Check Status               : Fail\n");
      break;
   case 1: printf("L-SIG Parity Check Status               : Pass\n");
      break;
   }
   switch (SIGCRCStatus)
   {
   case -1: printf("SIG CRC Status                          : Not Applicable\n");
      break;
   case 0: printf("SIG CRC Status                           : Fail\n");
      break;
   case 1: printf("SIG CRC Status                           : Pass\n");
      break;
   }
   switch (SIGBCRCStatus)
   {
   case -1: printf("SIG-B CRC Status                        : Not Applicable\n");
      break;
   case 0: printf("SIG-B CRC Status                         : Fail\n");
      break;
   case 1: printf("SIG-B CRC Status                         : Pass\n");
      break;
   }

   printf("\n--------------------------------------------------\n\n\n");

   for (i = 0; i < NUMBER_OF_FREQUENCY_SEGMENTS; i++)
   {
      RFmxCheckWarn(RFmxWLAN_BuildSegmentString("", i, MAX_SELECTOR_STRING_LENGTH, segmentString));
      printf("------------Measurements for %s-------------\n\n", segmentString);
      printf("Frequency Error Mean (Hz)               : %1f\n", frequencyErrorMeanArray[i]);
      printf("Symbol Clock Error Mean (ppm)           : %1f\n\n", symbolClockErrorMeanArray[i]);
      for (j = 0; j < numberOfStreamResults; j++)
      {
         RFmxCheckWarn(RFmxWLAN_BuildStreamString(segmentString, j, MAX_SELECTOR_STRING_LENGTH, streamString));
         printf("\n---------Measurements for %s--------\n", streamString);
         printf("Stream RMS EVM Mean (dB)                 : %1f\n", streamRMSEVMMean[i][j]);
         printf("Stream Pilot RMS EVM Mean (dB)           : %1f\n", streamPilotRMSEVMMean[i][j]);
         printf("Stream Data RMS EVM Mean (dB)            : %1f\n\n", streamDataRMSEVMMean[i][j]);
      }

      for (j = 0; j < NUMBER_OF_RECEIVE_CHAINS; j++)
      {
         RFmxCheckWarn(RFmxWLAN_BuildChainString(segmentString, j, MAX_SELECTOR_STRING_LENGTH, chainString));
         printf("\n---------Measurements for %s---------\n", chainString);
         printf("Cross Power Mean (dB)                   : %1f\n", crossPowerMean[i][j]);
         printf("\n------------------IQ Impairments------------------\n");
         printf("Relative I/Q Origin Offset Mean (dB)    : %1f\n", relativeIQOriginOffsetMean[i][j]);
         printf("Absolute I/Q Origin Offset Mean (dBm)   : %1f\n", absoluteIQOriginOfsetMean[i][j]);
         printf("I/Q Gain Imbalance Mean (dB)            : %1f\n", IQGainImbalanceMean[i][j]);
         printf("I/Q Quadrature Error Mean (deg)         : %1f\n", IQQuadratureErrorMean[i][j]);
         printf("I/Q Timing Skew Mean (s)                : %1f\n\n", IQTimingSkewMean[i][j]);
      }
      printf("\n--------------------------------------------------\n\n\n");
   }

Error:
   if (error)
   {
      RFmxWLAN_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (instrumentHandle)
   {
      RFmxWLAN_Close(instrumentHandle, 0);
   }

   if (MCSIndexArray)
   {
      free(MCSIndexArray);
   }
   if (numberOfSpaceTimeStreamsArray)
   {
      free(numberOfSpaceTimeStreamsArray);
   }

   for (i = 0; i < NUMBER_OF_FREQUENCY_SEGMENTS; i++)
   {
       if (streamRMSEVMMean[i])
       {
           free(streamRMSEVMMean[i]);
       }
       if (streamDataRMSEVMMean[i])
       {
           free(streamDataRMSEVMMean[i]);
       }
       if (streamPilotRMSEVMMean[i])
       {
           free(streamPilotRMSEVMMean[i]);
       }
      for (j = 0; j < numberOfStreamResults; j++)
      {
          if (streamRMSEVMPerSubcarrierMean[i][j])
          {
              free(streamRMSEVMPerSubcarrierMean[i][j]);
          }
          if (pilotConstellation[i][j])
          {
              free(pilotConstellation[i][j]);
          }
          if (dataConstellation[i][j])
          {
              free(dataConstellation[i][j]);
          }
      }
      if (streamRMSEVMPerSubcarrierMean[i])
      {
          free(streamRMSEVMPerSubcarrierMean[i]);
      }
      if (pilotConstellation[i])
      {
          free(pilotConstellation[i]);
      }
      if (dataConstellation[i])
      {
          free(dataConstellation[i]);
      }
   }

   printf("\nPress any key to exit\n");
   _getch();

   return error;
}