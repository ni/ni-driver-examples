//Steps:
//1. Open a new RFmx session.
//Note : To configure for more than 4 waveform, set the maxnumwfms in the option string
//       to that many number of waveform.
//2. Configure Number of Frequency Segment and Receive Chain.
//3. Configure Center Frequency for each Segment.
//4. Configure Standard and Channel Bandwidth Properties.
//5. Select OFDMModAcc measurement and enable all the traces.
//6. Configure the Measurement Interval.
//7. Configure Frequency Error Estimation Method.
//8. Configure Amplitude Tracking Enabled.
//9. Configure Phase Tracking Enabled.
//10. Configure Symbol Clock Error Correction Enabled.
//11. Configure Channel Estimation Type.
//12. Read the MIMO Waveform from the.tdms file.
//13. Wire the waveforms to RFmx Analyze N Wfms(IQ) API for performing the measurement.
//14. Fetch OFDMModAcc Measurements.
//14. Fetch the User specific results based on PPDU Type.
//15. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWLAN.h"
#include "niRFSG.h"
#include "niRFSGPlayback.h"

#define playbackCheckWarn(fCall)     if (1) {ViStatus _code_; if (_code_ = (fCall), _code_ < 0)    \
                                    {playbackError = _code_;goto Error;}        \
                                    else playbackError = (playbackError==0)?_code_:playbackError;}    \
                                    else playbackError = playbackError

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                     4096

/* Maximum number of devices */
#define NUMBER_OF_DEVICES                         2

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING_LENGTH                256

/* Number of frequency segments and offset chains */
#define NUMBER_OF_FREQUENCY_SEGMENTS              1
#define NUMBER_OF_RECEIVE_CHAINS                  2

#define NUMBER_OF_WAVEFORMS                       2


float64 IQx0[NUMBER_OF_WAVEFORMS] = { 0.0 }, IQdx[NUMBER_OF_WAVEFORMS] = { 0.0 };
int32 IQSize[NUMBER_OF_WAVEFORMS] = { 0 };
int32 error = 0, errorOccured = 0, playbackError = 0, lastErrorCode = 0;
NIComplexSingle* referenceWaveformF32 = NULL;
char errorMessage[MAX_ERROR_DESCRIPTION];

int32 ReadWaveformsFromTDMSFile(char* fileName)
{
   int32 sizePerWaveform = 0;
   ViReal64 x0v[NUMBER_OF_WAVEFORMS] = { 0.0 }, dxv[NUMBER_OF_WAVEFORMS] = { 0.0 };
   int i;

   playbackCheckWarn(niRFSGPlayback_ReadWaveformsFromFileComplexF32(fileName, NUMBER_OF_WAVEFORMS,
      0, NULL, NULL, NULL, &sizePerWaveform));

   if (sizePerWaveform > 0)
   {
      referenceWaveformF32 = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * sizePerWaveform * NUMBER_OF_WAVEFORMS);
      if (referenceWaveformF32)
      {
         playbackCheckWarn(niRFSGPlayback_ReadWaveformsFromFileComplexF32(fileName, NUMBER_OF_WAVEFORMS,
            sizePerWaveform, x0v, dxv, (NIComplexNumberF32*)referenceWaveformF32, &sizePerWaveform));
      }
      else
      {
         printf("malloc failed\n");
         return -1;
      }
      for (i = 0; i < NUMBER_OF_WAVEFORMS; i++)
      {
         IQx0[i] = (float64)x0v[i];
         IQdx[i] = (float64)dxv[i];
         IQSize[i] = sizePerWaveform;
      }
   }

Error:
   if (playbackError)
   {
      errorOccured = playbackError;
      niRFSGPlayback_GetError(&lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (playbackError < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }
   return errorOccured;
}

int main(int argc, char* argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };

   int i = 0, j = 0, k = 0;

   char* optionString = "Analysisonly = 1; MaxNumWfms:8";

   char* waveformFileName = "..\\Support\\WLAN_80211n_20MHz_1Seg_2Chain_MIMO.tdms";

   float64 centerFrequencyArray[NUMBER_OF_DEVICES] = { 5.18e9, 5.26e9 };               /*(Hz) */

   int32 standard = RFMXWLAN_VAL_STANDARD_802_11_N;
   float64 channelBandwidth = 20e6;                                                    /* (Hz) */

   int32 measurementOffset = 0;                                                        /* (symbols) */
   int32 maximumMeasurementLength = 16;                                                /* (symbols) */

   int32 frequencyErrorEstimationMethod = RFMXWLAN_VAL_OFDMMODACC_FREQUENCY_ERROR_ESTIMATION_METHOD_PREAMBLE_AND_PILOTS;
   int32 phaseTrackingEnabled = RFMXWLAN_VAL_OFDMMODACC_PHASE_TRACKING_ENABLED_TRUE;
   int32 channelEstimationType = RFMXWLAN_VAL_OFDMMODACC_CHANNEL_ESTIMATION_TYPE_REFERENCE;
   int32 amplitudeTrackingEnabled = RFMXWLAN_VAL_OFDMMODACC_AMPLITUDE_TRACKING_ENABLED_FALSE;
   int32 symbolClockErrorCorrectionEnabled = RFMXWLAN_VAL_OFDMMODACC_SYMBOL_CLOCK_ERROR_CORRECTION_ENABLED_TRUE;

   float64 timeout = 10.0;                                                             /* (seconds) */

   char segmentString[MAX_SELECTOR_STRING_LENGTH];
   char chainString[MAX_SELECTOR_STRING_LENGTH];
   char streamString[MAX_SELECTOR_STRING_LENGTH];

   float64 averagePowerMean = 0.0;
   float64 peakPowerMaximum = 0.0;

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
   RFmxCheckWarn(RFmxWLAN_Initialize(NULL, optionString, &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxWLAN_CfgNumberOfFrequencySegmentsAndReceiveChains(instrumentHandle, "",
      NUMBER_OF_FREQUENCY_SEGMENTS, NUMBER_OF_RECEIVE_CHAINS));

   for (i = 0; i < NUMBER_OF_FREQUENCY_SEGMENTS; ++i)
   {
      RFmxCheckWarn(RFmxWLAN_BuildSegmentString("", i, MAX_SELECTOR_STRING_LENGTH, segmentString));
      RFmxCheckWarn(RFmxWLAN_CfgFrequency(instrumentHandle, segmentString, centerFrequencyArray[i]));
   }

   RFmxCheckWarn(RFmxWLAN_CfgStandard(instrumentHandle, "", standard));
   RFmxCheckWarn(RFmxWLAN_CfgChannelBandwidth(instrumentHandle, "", channelBandwidth));

   RFmxCheckWarn(RFmxWLAN_SelectMeasurements(instrumentHandle, "", RFMXWLAN_VAL_OFDMMODACC, RFMXWLAN_VAL_TRUE));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgMeasurementLength(instrumentHandle, "", measurementOffset, maximumMeasurementLength));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgFrequencyErrorEstimationMethod(instrumentHandle, "", frequencyErrorEstimationMethod));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgAmplitudeTrackingEnabled(instrumentHandle, "", amplitudeTrackingEnabled));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgPhaseTrackingEnabled(instrumentHandle, "", phaseTrackingEnabled));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgSymbolClockErrorCorrectionEnabled(instrumentHandle, "", symbolClockErrorCorrectionEnabled));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgChannelEstimationType(instrumentHandle, "", channelEstimationType));

   errorOccured = ReadWaveformsFromTDMSFile(waveformFileName);
   if (errorOccured)
   {
      printf("Cannot open the specified waveform file\n");
      goto Exit;
   }

   RFmxCheckWarn(RFmxWLAN_AnalyzeNWaveformsIQ(instrumentHandle, "", "", IQx0, IQdx, referenceWaveformF32, IQSize,
      NUMBER_OF_WAVEFORMS, RFMXWLAN_VAL_TRUE));


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
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchNumberOfSpaceTimeStreams(instrumentHandle, userString, timeout,
            &numberOfSpaceTimeStreamsArray[i]));
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
      RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchNumberOfSpaceTimeStreams(instrumentHandle, "", timeout,
         &numberOfSpaceTimeStreamsArray[0]));
      numberOfStreamResults = numberOfSpaceTimeStreamsArray[0];
   }

   for (i = 0; i < NUMBER_OF_FREQUENCY_SEGMENTS; i++)
   {
      RFmxCheckWarn(RFmxWLAN_BuildSegmentString("", i, MAX_SELECTOR_STRING_LENGTH, segmentString));
      RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchFrequencyErrorMean(instrumentHandle, segmentString, timeout,
         &frequencyErrorMeanArray[i]));
      RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchSymbolClockErrorMean(instrumentHandle, segmentString, timeout,
         &symbolClockErrorMeanArray[i]));

      streamRMSEVMMean[i] = (float64*)(malloc(numberOfStreamResults * sizeof(float64)));
      streamDataRMSEVMMean[i] = (float64*)(malloc(numberOfStreamResults * sizeof(float64)));
      streamPilotRMSEVMMean[i] = (float64*)(malloc(numberOfStreamResults * sizeof(float64)));
      streamRMSEVMPerSubcarrierMean[i] = (float32**)(malloc(numberOfStreamResults * sizeof(float32*)));
      pilotConstellation[i] = (NIComplexSingle**)(malloc(numberOfStreamResults * sizeof(NIComplexSingle*)));
      dataConstellation[i] = (NIComplexSingle**)(malloc(numberOfStreamResults * sizeof(NIComplexSingle*)));

      if (streamRMSEVMMean[i] == NULL || streamDataRMSEVMMean[i] == NULL || streamPilotRMSEVMMean[i] == NULL
         || streamRMSEVMPerSubcarrierMean[i] == NULL || pilotConstellation[i] == NULL || dataConstellation[i] == NULL)
      {
         printf("malloc failed. \n");
         goto  Error;
      }

      for (j = 0; j < numberOfStreamResults; j++)
      {
         RFmxCheckWarn(RFmxWLAN_BuildStreamString(segmentString, j, MAX_SELECTOR_STRING_LENGTH, streamString));
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchStreamRMSEVM(instrumentHandle, streamString, timeout,
            &streamRMSEVMMean[i][j], &streamDataRMSEVMMean[i][j], &streamPilotRMSEVMMean[i][j]));

         actualArraySize = 0;
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchStreamRMSEVMPerSubcarrierMeanTrace(instrumentHandle, streamString,
            timeout, NULL, NULL, NULL, 0, &actualArraySize));
         if (actualArraySize > 0)
         {
            streamRMSEVMPerSubcarrierMean[i][j] = (float32*)(malloc(actualArraySize * sizeof(float32)));
            if (streamRMSEVMPerSubcarrierMean[i][j] == NULL)
            {
               printf("malloc failed. \n");
               goto  Error;
            }
            RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchStreamRMSEVMPerSubcarrierMeanTrace(instrumentHandle, streamString,
               timeout, &x0, &dx, streamRMSEVMPerSubcarrierMean[i][j], actualArraySize, NULL));
         }

         actualArraySize = 0;
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchPilotConstellationTrace(instrumentHandle, streamString,
            timeout, NULL, 0, &actualArraySize));
         if (actualArraySize > 0)
         {
            pilotConstellation[i][j] = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
            if (pilotConstellation[i][j] == NULL)
            {
               RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchPilotConstellationTrace(instrumentHandle, streamString,
                  timeout, pilotConstellation[i][j], actualArraySize, NULL));
            }
         }

         actualArraySize = 0;
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchDataConstellationTrace(instrumentHandle, streamString,
            timeout, NULL, 0, &actualArraySize));
         if (actualArraySize > 0)
         {
            dataConstellation[i][j] = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
            if (dataConstellation[i][j] == NULL)
            {
               RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchDataConstellationTrace(instrumentHandle, streamString,
                  timeout, dataConstellation[i][j], actualArraySize, NULL));
            }
         }
      }

      for (j = 0; j < NUMBER_OF_RECEIVE_CHAINS; j++)
      {
         RFmxCheckWarn(RFmxWLAN_BuildChainString(segmentString, j, MAX_SELECTOR_STRING_LENGTH, chainString));
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchCrossPower(instrumentHandle, chainString, timeout, &crossPowerMean[i][j]));
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchIQImpairments(instrumentHandle, chainString, timeout,
            &relativeIQOriginOffsetMean[i][j], &IQGainImbalanceMean[i][j], &IQQuadratureErrorMean[i][j],
            &absoluteIQOriginOfsetMean[i][j], &IQTimingSkewMean[i][j]));
      }
   }

   printf("-----------------------EVM-----------------------\n\n");
   printf("------------------Composite EVM------------------\n");
   printf("RMS EVM Mean (dB)                       : %lf\n", compositeRMSEVMMean);
   printf("Data RMS EVM Mean (dB)                  : %lf\n", compositeDataRMSEVMMean);
   printf("Pilot RMS EVM Mean (dB)                 : %lf\n\n", compositePilotRMSEVMMean);
   printf("Number of Symbols Used                  : %d\n\n", numberOfSymbolsUsed);
   printf("\n--------------------------------------------------\n\n\n");

   for (i = 0; i < NUMBER_OF_FREQUENCY_SEGMENTS; i++)
   {
      RFmxCheckWarn(RFmxWLAN_BuildSegmentString("", i, MAX_SELECTOR_STRING_LENGTH, segmentString));
      printf("------------Measurements for %s-------------\n\n", segmentString);
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
      }
      printf("\n--------------------------------------------------\n\n\n");
   }
   printf("--------------------Impairments & PPDU Info--------------------\n\n");

   for (i = 0; i < NUMBER_OF_FREQUENCY_SEGMENTS; i++)
   {
      RFmxCheckWarn(RFmxWLAN_BuildSegmentString("", i, MAX_SELECTOR_STRING_LENGTH, segmentString));
      printf("------------Measurements for %s-------------\n\n", segmentString);
      printf("Frequency Error Mean (Hz)               : %1f\n", frequencyErrorMeanArray[i]);
      printf("Symbol Clock Error Mean (ppm)           : %1f\n\n", symbolClockErrorMeanArray[i]);

      for (j = 0; j < NUMBER_OF_RECEIVE_CHAINS; j++)
      {
         RFmxCheckWarn(RFmxWLAN_BuildChainString(segmentString, j, MAX_SELECTOR_STRING_LENGTH, chainString));
         printf("\n---------IQ Impairments for %s---------\n", chainString);
         printf("Relative I/Q Origin Offset Mean (dB)    : %1f\n", relativeIQOriginOffsetMean[i][j]);
         printf("Absolute I/Q Origin Offset Mean (dBm)   : %1f\n", absoluteIQOriginOfsetMean[i][j]);
         printf("I/Q Gain Imbalance Mean (dB)            : %1f\n", IQGainImbalanceMean[i][j]);
         printf("I/Q Quadrature Error Mean (deg)         : %1f\n", IQQuadratureErrorMean[i][j]);
         printf("I/Q Timing Skew Mean (s)                : %1f\n\n", IQTimingSkewMean[i][j]);
      }
   }
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
   case -1: printf("SIG CRC Status                         : Not Applicable\n");
      break;
   case 0: printf("SIG CRC Status                          : Fail\n");
      break;
   case 1: printf("SIG CRC Status                          : Pass\n");
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

Exit:
   printf("\nPress any key to exit\n");
   _getch();

   return error;
}