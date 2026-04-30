//Steps:
//1. Open a new RFmx session.
//2. Configure the Frequency Reference properties(Clock Source and Clock Frequency).
//3. Configure the basic signal properties(Center Frequency, Reference Level and External Attenuation).
//Reference Level needs to be set to the peak power in the signal
//4. Configure IQ Power Edge Trigger properties(Trigger Delay, IQ Power Edge Level, Min Quiet Time).
//5. Configure Standard and Channel Bandwidth Properties.
//6. Select OFDMModAcc measurement and enable the traces.
//7. Configure the Measurement Interval.
//8. Configure Frequency Error Estimation Method.
//9. Configure Amplitude Tracking Enabled.
//10. Configure Phase Tracking Enabled.
//11. Configure Symbol Clock Error Correction Enabled.
//12. Configure Channel Estimation Type.
//13. Configure Averaging parameters.
//14. At this point we have a valid OFDMModAcc configuration, call RFmxWLAN OFDMModAcc Auto Level, to search for the reference level that gives the best EVM
//15. Initiate Measurement.
//16. Fetch OFDMModAcc Measurements.
//17. Close the RFmx Session.

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

   float64 centerFrequency = 2.412e9;                                           /* (Hz) */
   float64 referenceLevel = 0.0;                                                /* (dBm) */
   float64 externalAttenuation = 0.0;                                           /* (dB) */

   char *frequencyReferenceSource = RFMXWLAN_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                                  /* (Hz) */

   int32 IQPowerEdgeEnabled = RFMXWLAN_VAL_TRUE;
   float64 IQPowerEdgeLevel = -20.0;                                            /* (dB) */
   int32 minimumQuiteTimeMode = RFMXWLAN_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 5e-6;                                             /* (seconds) */
   float64 triggerDelay = 0.0;                                                  /* (seconds) */

   int32 standard = RFMXWLAN_VAL_STANDARD_802_11_AG;
   float64 channelBandwidth = 20e06;                                            /* (Hz) */

   int32 measurementOffset = 0;                                                 /* (symbols) */
   int32 maximumMeasurementLength = 16;                                         /* (symbols) */

   int32 frequencyErrorEstimationMethod = RFMXWLAN_VAL_OFDMMODACC_FREQUENCY_ERROR_ESTIMATION_METHOD_PREAMBLE_AND_PILOTS;
   int32 channelEstimationType = RFMXWLAN_VAL_OFDMMODACC_CHANNEL_ESTIMATION_TYPE_REFERENCE;
   int32 phaseTrackingEnabled = RFMXWLAN_VAL_OFDMMODACC_PHASE_TRACKING_ENABLED_TRUE;
   int32 amplitudeTrackingEnabled = RFMXWLAN_VAL_OFDMMODACC_AMPLITUDE_TRACKING_ENABLED_FALSE;
   int32 symbolClockErrorCorrectionEnabled = RFMXWLAN_VAL_OFDMMODACC_SYMBOL_CLOCK_ERROR_CORRECTION_ENABLED_TRUE;

   int32 averagingEnabled = RFMXWLAN_VAL_OFDMMODACC_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   float64 timeout = 10.0;                                                      /* (seconds) */

   float64 compositeRMSEVMMean = 0.0;
   float64 compositeDataRMSEVMMean = 0.0;
   float64 compositePilotRMSEVMMean = 0.0;

   int32 numberOfSymbolsUsed = 0;
   float64 frequencyErrorMean = 0.0;
   float64 symbolClockErrorMean = 0.0;
   int32 PPDUType = RFMXWLAN_VAL_OFDM_PPDU_TYPE_NON_HT;
   int32 MCSIndex = 0;
   int32 guardIntervalType = 0;
   int32 LSIGParityCheckStatus = RFMXWLAN_VAL_OFDMMODACC_L_SIG_PARITY_CHECK_STATUS_NOT_APPLICABLE;
   int32 SIGCRCStatus = RFMXWLAN_VAL_OFDMMODACC_SIG_CRC_STATUS_NOT_APPLICABLE;
   int32 SIGBCRCStatus = RFMXWLAN_VAL_OFDMMODACC_SIG_B_CRC_STATUS_NOT_APPLICABLE;

   float64 relativeIQOriginOffsetMean = 0.0;
   float64 IQGainImbalanceMean = 0.0;
   float64 IQQuadratureErrorMean = 0.0;
   float64 absoluteIQOriginOffsetMean = 0.0;
   float64 IQTimingSkewMean = 0.0;

   NIComplexSingle *pilotConstellation = NULL;
   NIComplexSingle *dataConstellation = NULL;

   int32 actualArraySize = 0;
   float64 x0 = 0.0;
   float64 dx = 0.0;
   float32 *chainRMSEVMPerSubcarrierMean = NULL;

   /* Initialize a session */
   RFmxCheckWarn(RFmxInstr_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxInstr_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxWLAN_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxWLAN_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxWLAN_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxWLAN_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXWLAN_VAL_IQ_POWER_EDGE_RISING_SLOPE, IQPowerEdgeLevel, triggerDelay,
      minimumQuiteTimeMode, minimumQuietTime, RFMXWLAN_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, IQPowerEdgeEnabled));
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
   RFmxCheckWarn(RFmxWLAN_OFDMModAccAutoLevel(instrumentHandle, "", timeout));
   RFmxCheckWarn(RFmxWLAN_Initiate(instrumentHandle, "", ""));

   /* Fetch Results */
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchCompositeRMSEVM(instrumentHandle, "", timeout, &compositeRMSEVMMean,
      &compositeDataRMSEVMMean, &compositePilotRMSEVMMean));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchNumberofSymbolsUsed(instrumentHandle, "", timeout, &numberOfSymbolsUsed));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchFrequencyErrorMean(instrumentHandle, "", timeout, &frequencyErrorMean));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchSymbolClockErrorMean(instrumentHandle, "", timeout, &symbolClockErrorMean));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchIQImpairments(instrumentHandle, "", timeout, &relativeIQOriginOffsetMean,
      &IQGainImbalanceMean, &IQQuadratureErrorMean, &absoluteIQOriginOffsetMean, &IQTimingSkewMean));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchPPDUType(instrumentHandle, "", timeout, &PPDUType));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchMCSIndex(instrumentHandle, "", timeout, &MCSIndex));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchGuardIntervalType(instrumentHandle, "", timeout, &guardIntervalType));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchLSIGParityCheckStatus(instrumentHandle, "", timeout, &LSIGParityCheckStatus));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchSIGCRCStatus(instrumentHandle, "", timeout, &SIGCRCStatus));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchSIGBCRCStatus(instrumentHandle, "", timeout, &SIGBCRCStatus));

   actualArraySize = 0;
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchPilotConstellationTrace(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      pilotConstellation = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
      if (pilotConstellation)
      {
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchPilotConstellationTrace(instrumentHandle, "", timeout, pilotConstellation,
            actualArraySize, NULL));
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
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchDataConstellationTrace(instrumentHandle, "", timeout, dataConstellation,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchChainRMSEVMPerSubcarrierMeanTrace(instrumentHandle, "", timeout, NULL, NULL, NULL,
      0, &actualArraySize));
   if (actualArraySize > 0)
   {
      chainRMSEVMPerSubcarrierMean = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (chainRMSEVMPerSubcarrierMean)
      {
         RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchChainRMSEVMPerSubcarrierMeanTrace(instrumentHandle, "", timeout, &x0, &dx,
            chainRMSEVMPerSubcarrierMean, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   /*Print Results */
   printf("------------------EVM------------------\n\n");
   printf("------------------Composite EVM------------------\n");
   printf("RMS EVM Mean (dB)                       : %lf\n", compositeRMSEVMMean);
   printf("Data RMS EVM Mean (dB)                  : %lf\n", compositeDataRMSEVMMean);
   printf("Pilot RMS EVM Mean (dB)                 : %lf\n\n", compositePilotRMSEVMMean);
   printf("Number of Symbols Used                  : %d\n\n", numberOfSymbolsUsed);
   printf("------------------Impairments & PPDU Info------------------\n\n");
   printf("Frequency Error Mean (Hz)               : %lf\n", frequencyErrorMean);
   printf("Symbol Clock Error Mean (ppm)           : %lf\n\n", symbolClockErrorMean);
   printf("------------------IQ Impairments------------------\n");
   printf("Relative I/Q Origin Offset Mean (dB)    : %lf\n", relativeIQOriginOffsetMean);
   printf("Absolute I/Q Origin Offset Mean (dBm)   : %lf\n", absoluteIQOriginOffsetMean);
   printf("I/Q Gain Imbalance Mean (dB)            : %lf\n", IQGainImbalanceMean);
   printf("I/Q Quadrature Error Mean (deg)         : %lf\n", IQQuadratureErrorMean);
   printf("I/Q Timing Skew Mean (s)                : %lf\n", IQTimingSkewMean);
   printf("\n------------------PPDU Info------------------\n");
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
   printf("MCS Index                               : %d\n", MCSIndex);
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
   if (pilotConstellation)
   {
      free(pilotConstellation);
   }
   if (dataConstellation)
   {
      free(dataConstellation);
   }
   if (chainRMSEVMPerSubcarrierMean)
   {
      free(chainRMSEVMPerSubcarrierMean);
   }
   printf("\nPress any key to exit\n");
   _getch();

   return error;
}