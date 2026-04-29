//Steps:
//1. Open a new RFmx session.
//2. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//5. Configure Standard and Channel Bandwidth Properties.
//6. Disable auto PPDU type detection and header decoding. Configure PPDU Type and properties that are otherwise decoded from Header.
//7. Select OFDMModAcc measurement and disable the traces.
//8. Configure the Measurement Interval. Make sure that the input signal has number of symbols at least equal to the specified Maximum Measurement Length.
//9. Configure Frequency Error Estimation Method. You may set Frequency Error Estimation Method to disabled to optimize speed of the measurement
//   when there is no frequency error between transmitter and receiver.
//10. Configure Amplitude Tracking Enabled.
//11. Configure Symbol Clock Error Correction Enabled. You may set Symbol Clock Correction Enabled to False to optimize speed of the measurement
//    when there is no symbol clock error between transmitter and receiver.
//12. Configure Averaging parameters.
//13. Disable burst start detection and I/Q impairments estimation.
//14. Initiate Measurement.
//15. Fetch OFDMModAcc Measurements.
//16. Close the RFmx Session.

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

   int32 standard = RFMXWLAN_VAL_STANDARD_802_11_AG;
   float64 channelBandwidth = 20e06;                                            /*(Hz) */

   int32 autoPPDUTypeDetectionEnabled = RFMXWLAN_VAL_OFDM_AUTO_PPDU_TYPE_DETECTION_ENABLED_FALSE;
   int32 PPDUType = RFMXWLAN_VAL_OFDM_PPDU_TYPE_NON_HT;
   int32 headerDecodingEnabled = RFMXWLAN_VAL_OFDM_HEADER_DECODING_ENABLED_FALSE;
   int32 MCSIndex = 0;
   int32 guardIntervalType = RFMXWLAN_VAL_OFDM_GUARD_INTERVAL_TYPE_1_4;
   int32 LTFSize = RFMXWLAN_VAL_OFDM_LTF_SIZE_4X;
   int32 RUSize = 26;
   int32 NumberOfSIGSymbols = 1;

   int32 burstStartDetectionEnabled = RFMXWLAN_VAL_OFDMMODACC_BURST_START_DETECTION_ENABLED_FALSE;
   int32 IQImpairmentsEstimationEnabled = RFMXWLAN_VAL_OFDMMODACC_IQ_IMPAIRMENTS_ESTIMATION_ENABLED_FALSE;

   int32 averagingEnabled = RFMXWLAN_VAL_OFDMMODACC_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   int32 measurementOffset = 0;                                                 /*(symbols)*/
   int32 maximumMeasurementLength = 16;                                         /*(symbols)*/
   int32 frequencyErrorEstimationMethod = RFMXWLAN_VAL_OFDMMODACC_FREQUENCY_ERROR_ESTIMATION_METHOD_DISABLED;

   int32 amplitudeTrackingEnabled = RFMXWLAN_VAL_OFDMMODACC_AMPLITUDE_TRACKING_ENABLED_FALSE;
   int32 symbolClockErrorCorrectionEnabled = RFMXWLAN_VAL_OFDMMODACC_SYMBOL_CLOCK_ERROR_CORRECTION_ENABLED_FALSE;

   float64 timeout = 10.0;

   float64 compositeRMSEVMMean = 0.0;
   float64 compositeDataRMSEVMMean = 0.0;
   float64 compositePilotRMSEVMMean = 0.0;

   int32 numberOfSymbolsUsed = 0;
   float64 frequencyErrorMean = 0.0;
   float64 symbolClockErrorMean = 0.0;

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
   RFmxCheckWarn(RFmxWLAN_SetOFDMAutoPPDUTypeDetectionEnabled(instrumentHandle, "", autoPPDUTypeDetectionEnabled));
   RFmxCheckWarn(RFmxWLAN_SetOFDMPPDUType(instrumentHandle, "", PPDUType));
   RFmxCheckWarn(RFmxWLAN_SetOFDMHeaderDecodingEnabled(instrumentHandle, "", headerDecodingEnabled));
   RFmxCheckWarn(RFmxWLAN_SetOFDMMCSIndex(instrumentHandle, "", MCSIndex));
   RFmxCheckWarn(RFmxWLAN_SetOFDMGuardIntervalType(instrumentHandle, "", guardIntervalType));
   RFmxCheckWarn(RFmxWLAN_SetOFDMLTFSize(instrumentHandle, "", LTFSize));
   RFmxCheckWarn(RFmxWLAN_SetOFDMRUSize(instrumentHandle, "", RUSize));
   RFmxCheckWarn(RFmxWLAN_SetOFDMNumberOfSIGSymbols(instrumentHandle, "", NumberOfSIGSymbols));
   RFmxCheckWarn(RFmxWLAN_SelectMeasurements(instrumentHandle, "", measurements, enableAllTraces));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgMeasurementLength(instrumentHandle, "", measurementOffset, maximumMeasurementLength));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgFrequencyErrorEstimationMethod(instrumentHandle, "", frequencyErrorEstimationMethod));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgAmplitudeTrackingEnabled(instrumentHandle, "", amplitudeTrackingEnabled));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgSymbolClockErrorCorrectionEnabled(instrumentHandle, "", symbolClockErrorCorrectionEnabled));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccSetBurstStartDetectionEnabled(instrumentHandle, "", burstStartDetectionEnabled));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccSetIQImpairmentsEstimationEnabled(instrumentHandle, "", IQImpairmentsEstimationEnabled));
   RFmxCheckWarn(RFmxWLAN_Initiate(instrumentHandle, "", ""));

   /* Fetch Results */
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchCompositeRMSEVM(instrumentHandle, "", timeout, &compositeRMSEVMMean,
      &compositeDataRMSEVMMean, &compositePilotRMSEVMMean));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchNumberofSymbolsUsed(instrumentHandle, "", timeout, &numberOfSymbolsUsed));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchFrequencyErrorMean(instrumentHandle, "", timeout, &frequencyErrorMean));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchSymbolClockErrorMean(instrumentHandle, "", timeout, &symbolClockErrorMean));

   /*Print Results */
   printf("------------------Composite EVM------------------\n");
   printf("RMS EVM Mean (dB)                       : %lf\n", compositeRMSEVMMean);
   printf("Data RMS EVM Mean (dB)                  : %lf\n", compositeDataRMSEVMMean);
   printf("Pilot RMS EVM Mean (dB)                 : %lf\n\n", compositePilotRMSEVMMean);
   printf("Number of Symbols Used                  : %d\n", numberOfSymbolsUsed);
   printf("Frequency Error Mean(Hz)                : %lf\n", frequencyErrorMean);
   printf("Symbol Clock Error Mean(ppm)            : %lf\n\n", symbolClockErrorMean);

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
   printf("Press any key to exit\n");
   _getch();

   return error;
}