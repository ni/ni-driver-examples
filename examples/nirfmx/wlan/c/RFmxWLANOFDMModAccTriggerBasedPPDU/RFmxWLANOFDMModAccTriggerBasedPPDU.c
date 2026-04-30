//Steps:
//1. Open a new RFmx session.
//2. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//5. Configure Standard to 802.11ax and Channel Bandwidth properties.
//6. Configure MCS Index, RU Size, RU Offset, RU Type, Distribution Bandwidth, Guard Interval Type, LTF Size and PE Disambiguity.
//7. Select OFDMModAcc measurement and enable the traces.
//8. Configure Measurement Interval.
//9. Configure Unused Tone Error Mask Reference.
//10. Configure Averaging parameters.
//11. Initiate Measurement.
//12. Fetch OFDMModAcc Measurements.
//13. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWLAN.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;

   char * frequencyReferenceSource = RFMXWLAN_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                                  /*(Hz) */

   float64 centerFrequency = 2.412e9;                                           /*(Hz) */
   float64 referenceLevel = 0.0;                                                /*(dBm) */
   float64 externalAttenuation = 0.0;                                           /*(dB) */

   int32 IQPowerEdgeEnabled = RFMXWLAN_VAL_TRUE;
   int32 IQPowerEdgeSlope = RFMXWLAN_VAL_IQ_POWER_EDGE_RISING_SLOPE;
   float64 IQPowerEdgeLevel = -20.0;                                            /*(dB) */
   int32 minimumQuiteTimeMode = RFMXWLAN_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 5e-6;                                             /*(seconds) */
   int32 IQPowerEdgeLevelType = RFMXWLAN_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
   float64 triggerDelay = 0.0;                                                  /*(seconds) */

   uInt32 measurements = RFMXWLAN_VAL_OFDMMODACC;
   int32 enableAllTraces = RFMXWLAN_VAL_TRUE;

   int32 standard = RFMXWLAN_VAL_STANDARD_802_11_AX;
   float64 channelBandwidth = 20e06;                                            /*(Hz) */
   float64 distributionBandwidth = 20e06;                                       /*(Hz) */

   int32 MCSIndex = 0;
   int32 RUSize = 26;
   int32 RUOffsetMRUIndex = 0;
   int32 RUType = RFMXWLAN_VAL_OFDM_RU_TYPE_RRU;
   int32 guardIntervalType = RFMXWLAN_VAL_OFDM_GUARD_INTERVAL_TYPE_1_4;
   int32 LTFSize = RFMXWLAN_VAL_OFDM_LTF_SIZE_4X;
   int32 PEDisambiguity = 0;

   int32 measurementOffset = 0;                                                 /*(symbols)*/
   int32 maximumMeasurementLength = 16;                                         /*(symbols)*/

   int32 unusedToneErrorMaskReference = RFMXWLAN_VAL_OFDMMODACC_UNUSED_TONE_ERROR_MASK_REFERENCE_LIMIT1;

   int32 averagingEnabled = RFMXWLAN_VAL_OFDMMODACC_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   float64 timeout = 10.0;                                                      /*(seconds) */

   float64 compositeRMSEVMMean = 0.0;                                           /*(dB) */
   float64 compositeDataRMSEVMMean = 0.0;                                       /*(dB) */
   float64 compositePilotRMSEVMMean = 0.0;                                      /*(dB) */

   float64 unusedToneErrorMargin = 0.0;                                         /*(dB) */
   int32 unusedToneErrorMarginRUIndex = 0;

   float64 *unusedToneErrorMarginPerRU = NULL;                                  /*(dB) */
   int32 marginPerRUSize = 0;
   float64 frequencyErrorMean = 0.0;                                            /*(Hz) */
   float64 frequencyErrorCCDF10Percent = 0.0;                                   /*(Hz) */
   float64 symbolClockErrorMean = 0.0;                                          /*(ppm) */
   int32 PPDUType = RFMXWLAN_VAL_OFDM_PPDU_TYPE_NON_HT;

   NIComplexSingle *pilotConstellation = NULL;
   NIComplexSingle *dataConstellation = NULL;

   int32 actualArraySize = 0;
   float64 x0 = 0.0;
   float64 dx = 0.0;
   float32 *unusedToneError = NULL;
   float32 *unusedToneErrorMask = NULL;

   int32 i = 0;

   /* Initialize a session */
   RFmxCheckWarn(RFmxInstr_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxInstr_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxWLAN_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxWLAN_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxWLAN_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxWLAN_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeSlope, IQPowerEdgeLevel, triggerDelay,
      minimumQuiteTimeMode, minimumQuietTime, IQPowerEdgeLevelType, IQPowerEdgeEnabled));
   RFmxCheckWarn(RFmxWLAN_CfgStandard(instrumentHandle, "", standard));
   RFmxCheckWarn(RFmxWLAN_CfgChannelBandwidth(instrumentHandle, "", channelBandwidth));
   RFmxCheckWarn(RFmxWLAN_SetOFDMMCSIndex(instrumentHandle, "", MCSIndex));
   RFmxCheckWarn(RFmxWLAN_SetOFDMRUSize(instrumentHandle, "", RUSize));
   RFmxCheckWarn(RFmxWLAN_SetOFDMRUOffsetMRUIndex(instrumentHandle, "", RUOffsetMRUIndex));
   RFmxCheckWarn(RFmxWLAN_SetOFDMRUType(instrumentHandle, "", RUType));
   RFmxCheckWarn(RFmxWLAN_SetOFDMDistributionBandwidth(instrumentHandle, "", distributionBandwidth));
   RFmxCheckWarn(RFmxWLAN_SetOFDMGuardIntervalType(instrumentHandle, "", guardIntervalType));
   RFmxCheckWarn(RFmxWLAN_SetOFDMLTFSize(instrumentHandle, "", LTFSize));
   RFmxCheckWarn(RFmxWLAN_SetOFDMPEDisambiguity(instrumentHandle, "", PEDisambiguity));
   RFmxCheckWarn(RFmxWLAN_SelectMeasurements(instrumentHandle, "", measurements, enableAllTraces));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgMeasurementLength(instrumentHandle, "", measurementOffset, maximumMeasurementLength));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccSetUnusedToneErrorMaskReference(instrumentHandle, "", unusedToneErrorMaskReference));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
   RFmxCheckWarn(RFmxWLAN_Initiate(instrumentHandle, "", ""));

   /* Fetch Results */
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchCompositeRMSEVM(instrumentHandle, "", timeout, &compositeRMSEVMMean,
      &compositeDataRMSEVMMean, &compositePilotRMSEVMMean));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchUnusedToneError(instrumentHandle, "", timeout, &unusedToneErrorMargin,
      &unusedToneErrorMarginRUIndex));
   actualArraySize = 0;
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchUnusedToneErrorMarginPerRU(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      unusedToneErrorMarginPerRU = (float64*)malloc(sizeof(float64) * actualArraySize);
      marginPerRUSize = actualArraySize;
      if (unusedToneErrorMarginPerRU)
      {
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchUnusedToneErrorMarginPerRU(instrumentHandle, "", timeout,
            unusedToneErrorMarginPerRU, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchFrequencyErrorMean(instrumentHandle, "", timeout, &frequencyErrorMean));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchFrequencyErrorCCDF10Percent(instrumentHandle, "", timeout,
      &frequencyErrorCCDF10Percent));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchSymbolClockErrorMean(instrumentHandle, "", timeout, &symbolClockErrorMean));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchPPDUType(instrumentHandle, "", timeout, &PPDUType));

   actualArraySize = 0;
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchPilotConstellationTrace(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      pilotConstellation = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
      if (pilotConstellation)
      {
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchPilotConstellationTrace(instrumentHandle, "", timeout,
            pilotConstellation, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchDataConstellationTrace(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      dataConstellation = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
      if (dataConstellation)
      {
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchDataConstellationTrace(instrumentHandle, "", timeout,
            dataConstellation, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchUnusedToneErrorMeanTrace(instrumentHandle, "", timeout, NULL, NULL, NULL,
      NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      unusedToneError = (float32*)malloc(sizeof(float32) * actualArraySize);
      unusedToneErrorMask = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (unusedToneError && unusedToneErrorMask)
      {
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchUnusedToneErrorMeanTrace(instrumentHandle, "", timeout, &x0, &dx,
            unusedToneError, unusedToneErrorMask, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   /*Print Results */
   printf("------------------EVM & Impairments------------------\n\n");
   printf("RMS EVM Mean (dB)                       : %lf\n", compositeRMSEVMMean);
   printf("Frequency Error Mean (Hz)               : %lf\n", frequencyErrorMean);
   printf("Frequency Error CCDF 10%% (Hz)           : %lf\n", frequencyErrorCCDF10Percent);
   printf("Symbol Clock Error Mean (ppm)           : %lf\n", symbolClockErrorMean);
   switch (PPDUType)
   {
   case 0: printf("PPDU Type                               : Non-HT\n\n");
      break;
   case 1: printf("PPDU Type                               : Mixed\n\n");
      break;
   case 2: printf("PPDU Type                               : GreenField\n\n");
      break;
   case 3: printf("PPDU Type                               : SU\n\n");
      break;
   case 4: printf("PPDU Type                               : MU\n\n");
      break;
   case 5: printf("PPDU Type                               : Extended Range SU\n\n");
      break;
   case 6: printf("PPDU Type                               : Trigger-Based\n\n");
      break;
   }
   printf("------------------Unused Tone Error------------------\n\n");
   printf("Margin (dB)                             : %lf\n", unusedToneErrorMargin);
   printf("Margin RU Index                         : %d\n\n", unusedToneErrorMarginRUIndex);
   for (i = 0; i < marginPerRUSize; i++)
   {
      printf("Unused Tone Error Margin per RU (dB)     : %lf\n\n", unusedToneErrorMarginPerRU[i]);
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

   /* Free allocated memory */
   if (unusedToneErrorMarginPerRU)
   {
      free(unusedToneErrorMarginPerRU);
   }
   if (pilotConstellation)
   {
      free(pilotConstellation);
   }
   if (dataConstellation)
   {
      free(dataConstellation);
   }
   if (unusedToneError)
   {
      free(unusedToneError);
   }
   if (unusedToneErrorMask)
   {
      free(unusedToneErrorMask);
   }
   printf("Press any key to exit\n");
   _getch();

   return error;
}