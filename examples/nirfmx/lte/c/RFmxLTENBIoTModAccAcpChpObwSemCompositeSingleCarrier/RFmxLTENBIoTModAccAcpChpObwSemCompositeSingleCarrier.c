//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier to 200k.
//6. Configure NB-IoT Component Carrier.
//7. Configure NPUSCH Format.
//8. Configure Auto NPUSCH Channel Detection Enabled.
//9. Configure NPUSCH Starting Slot.
//10. Configure NPUSCH DMRS.
//11. Select ACP, CHP, ModAcc, OBW and SEM measurements and enable Traces.
//12. Configure Averaging Parameters for ModAcc.
//13. Configure Averaging Parameters for ACP.
//14. Configure Averaging Parameters for CHP.
//15. Configure Averaging Parameters for OBW.
//16. Configure Averaging Parameters for SEM.
//17. Configure ACP Sweep Time.
//18. Configure CHP Sweep Time.
//19. Configure OBW Sweep Time.
//20. Configure SEM Sweep Time.
//21. Configure ModAcc Synchronization Mode and Measurement Interval.
//22. Initiate the Measurement.
//23. Fetch ModAcc Measurements.
//24. Fetch ACP Measurements.
//25. Fetch SEM Measurements.
//26. Fetch OBW Measurements.
//27. Fetch CHP Measurements.
//28. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                     256

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0;

   char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                          /*(Hz) */

   float64 centerFrequency = 1.95e9;                                    /*(Hz) */
   float64 referenceLevel = 0.0;                                        /*(dBm) */
   float64 externalAttenuation = 0.0;                                   /*(dB) */

   int32 enableTrigger = RFMXLTE_VAL_TRUE;
   int32 IQPowerEdgeSlope = RFMXLTE_VAL_IQ_POWER_EDGE_RISING_SLOPE;
   float64 IQPowerEdgeLevel = -20.0;                                    /*(dB) */
   int32 minimumQuiteTimeMode = RFMXLTE_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 100e-6;                                   /*(s) */
   int32 IQPowerEdgeLevelType = RFMXLTE_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
   float64 triggerDelay = 0.0;                                          /*(s) */

   int32 NCellID = 0;
   int32 uplinkSubcarrierSpacing = RFMXLTE_VAL_NB_IOT_UPLINK_SUBCARRIER_SPACING_15KHZ;
   int32 NPUSCHFormat = 1;
   int32 NPUSCHStartingSlot = 0;

   int32 baseSequenceMode = RFMXLTE_VAL_NPUSCH_DMRS_BASE_SEQUENCE_MODE_AUTO;
   int32 baseSequenceIndex = 0;
   int32 cyclicShift = 0;
   int32 groupHoppingEnabled = RFMXLTE_VAL_NPUSCH_DMRS_GROUP_HOPPING_ENABLED_FALSE;
   int32 deltaSS = 0;

   int32 synchronizationMode = RFMXLTE_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;
   int32 measurementOffset = 0;                                         /*(slots) */
   int32 measurementLength = 1;                                         /*(slots) */

   float64 sweepTimeInterval = 0.001;                                   /*(s) */

   int32 averagingCount = 10;

   float64 componentCarrierBandwidth = 200e3;                           /*(Hz) */
   float64 componentCarrierFrequency = 0.0;                             /*(Hz) */
   int32 cellID = 0;

   int32 autoNPUSCHChannelDetectionEnabled = RFMXLTE_VAL_AUTO_NPUSCH_CHANNEL_DETECTION_ENABLED_TRUE;

   float64 timeout = 10.0;                                              /*(s) */

   float64 meanRMSCompositeEVM = 0.0;                                   /*(% or dB) */
   float64 maxPeakCompositeEVM = 0.0;                                   /*(% or dB) */
   int32 peakCompositeEVMSlotIndex = 0;
   int32 peakCompositeEVMSymbolIndex = 0;
   int32 peakCompositeEVMSubcarrierIndex = 0;
   float64 meanFrequencyError = 0.0;                                    /*(Hz) */

   float64 meanIQOriginOffset = 0.0;                                    /*(dBc) */
   float64 meanIQGainImbalance = 0.0;                                   /*(dB) */
   float64 meanIQQuadratureError = 0.0;                                 /*(deg) */

   float64 inBandEmissionMargin = 0.0;                                  /*(dB) */

   float64 ACPAbsolutePower = 0.0;                                      /*(dBm) */
   float64* ACPLowerAbsolutePower = NULL;                               /*(dBm) */
   float64* ACPUpperAbsolutePower = NULL;                               /*(dBm) */
   float64* ACPLowerRelativePower = NULL;                               /*(dB) */
   float64* ACPUpperRelativePower = NULL;                               /*(dB) */
   int32 ACPOffsetMeasArraySize = 0;

   float64 CHPAbsolutePower = 0.0;                                      /*(dBm) */

   float64 OBWOccupiedBandwidth = 0.0;                                  /*(Hz) */
   float64 OBWAbsolutePower = 0.0;                                      /*(dBm) */
   float64 OBWStartFrequency = 0.0;                                     /*(Hz) */
   float64 OBWStopFrequency = 0.0;                                      /*(Hz) */

   int32 SEMMeasurementStatus = 0;
   float64 SEMAbsoluteIntegratedPower = 0.0;                            /*(dBm) */
   int32* SEMLowerOffsetMeasurementStatus = NULL;
   float64* SEMLowerOffsetMargin = NULL;                                /*(dB) */
   float64* SEMLowerOffsetMarginFrequency = NULL;                       /*(Hz) */
   float64* SEMLowerOffsetMarginAbsolutePower = NULL;                   /*(dBm) */
   int32* SEMUpperOffsetMeasurementStatus = NULL;
   float64* SEMUpperOffsetMargin = NULL;                                /*(dB) */
   float64* SEMUpperOffsetMarginFrequency = NULL;                       /*(Hz) */
   float64* SEMUpperOffsetMarginAbsolutePower = NULL;                   /*(dBm) */
   int32 SEMLowerOffsetMeasArraySize = 0, SEMUpperOffsetMeasArraySize = 0;

   /* Initialize a session */
   RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));

   RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));

   RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel,
      externalAttenuation));

   RFmxCheckWarn(RFmxLTE_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeSlope, IQPowerEdgeLevel,
      triggerDelay, minimumQuiteTimeMode, minimumQuietTime, IQPowerEdgeLevelType, enableTrigger));

   RFmxCheckWarn(RFmxLTE_CfgComponentCarrier(instrumentHandle, "", componentCarrierBandwidth,
      componentCarrierFrequency, cellID));

   RFmxCheckWarn(RFmxLTE_CfgNBIoTComponentCarrier(instrumentHandle, "", NCellID, uplinkSubcarrierSpacing));

   RFmxCheckWarn(RFmxLTE_CfgNPUSCHFormat(instrumentHandle, "", NPUSCHFormat));

   RFmxCheckWarn(RFmxLTE_CfgAutoNPUSCHChannelDetectionEnabled(instrumentHandle, "",
      autoNPUSCHChannelDetectionEnabled));

   RFmxCheckWarn(RFmxLTE_CfgNPUSCHStartingSlot(instrumentHandle, "", NPUSCHStartingSlot));

   RFmxCheckWarn(RFmxLTE_CfgNPUSCHDMRS(instrumentHandle, "", baseSequenceMode, baseSequenceIndex, cyclicShift,
      groupHoppingEnabled, deltaSS));

   RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_ACP | RFMXLTE_VAL_CHP |
      RFMXLTE_VAL_MODACC | RFMXLTE_VAL_OBW | RFMXLTE_VAL_SEM, RFMXLTE_VAL_TRUE));

   RFmxCheckWarn(RFmxLTE_ModAccCfgAveraging(instrumentHandle, "", RFMXLTE_VAL_MODACC_AVERAGING_ENABLED_FALSE,
      averagingCount));

   RFmxCheckWarn(RFmxLTE_ACPCfgAveraging(instrumentHandle, "", RFMXLTE_VAL_ACP_AVERAGING_ENABLED_FALSE,
      averagingCount, RFMXLTE_VAL_ACP_AVERAGING_TYPE_RMS));

   RFmxCheckWarn(RFmxLTE_CHPCfgAveraging(instrumentHandle, "", RFMXLTE_VAL_CHP_AVERAGING_ENABLED_FALSE,
      averagingCount, RFMXLTE_VAL_CHP_AVERAGING_TYPE_RMS));

   RFmxCheckWarn(RFmxLTE_OBWCfgAveraging(instrumentHandle, "", RFMXLTE_VAL_OBW_AVERAGING_ENABLED_FALSE,
      averagingCount, RFMXLTE_VAL_OBW_AVERAGING_TYPE_RMS));

   RFmxCheckWarn(RFmxLTE_SEMCfgAveraging(instrumentHandle, "", RFMXLTE_VAL_SEM_AVERAGING_ENABLED_FALSE,
      averagingCount, RFMXLTE_VAL_SEM_AVERAGING_TYPE_RMS));

   RFmxCheckWarn(RFmxLTE_ACPCfgSweepTime(instrumentHandle, "", RFMXLTE_VAL_ACP_SWEEP_TIME_AUTO_TRUE,
      sweepTimeInterval));

   RFmxCheckWarn(RFmxLTE_CHPCfgSweepTime(instrumentHandle, "", RFMXLTE_VAL_CHP_SWEEP_TIME_AUTO_TRUE,
      sweepTimeInterval));

   RFmxCheckWarn(RFmxLTE_OBWCfgSweepTime(instrumentHandle, "", RFMXLTE_VAL_OBW_SWEEP_TIME_AUTO_TRUE,
      sweepTimeInterval));

   RFmxCheckWarn(RFmxLTE_SEMCfgSweepTime(instrumentHandle, "", RFMXLTE_VAL_SEM_SWEEP_TIME_AUTO_TRUE,
      sweepTimeInterval));

   RFmxCheckWarn(RFmxLTE_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "",
      synchronizationMode, measurementOffset, measurementLength));

   RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxLTE_ModAccFetchCompositeEVM(instrumentHandle, "", timeout, &meanRMSCompositeEVM,
      &maxPeakCompositeEVM, &meanFrequencyError, &peakCompositeEVMSymbolIndex, &peakCompositeEVMSubcarrierIndex,
      &peakCompositeEVMSlotIndex));

   RFmxCheckWarn(RFmxLTE_ModAccFetchIQImpairments(instrumentHandle, "", timeout,
      &meanIQOriginOffset, &meanIQGainImbalance, &meanIQQuadratureError));

   RFmxCheckWarn(RFmxLTE_ModAccFetchInBandEmissionMargin(instrumentHandle, "", timeout, &inBandEmissionMargin));

   RFmxCheckWarn(RFmxLTE_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL,
      0, &ACPOffsetMeasArraySize));
   if (ACPOffsetMeasArraySize > 0)
   {
      ACPLowerRelativePower = (float64 *)malloc(sizeof(float64) * ACPOffsetMeasArraySize);
      ACPUpperRelativePower = (float64 *)malloc(sizeof(float64) * ACPOffsetMeasArraySize);
      ACPLowerAbsolutePower = (float64 *)malloc(sizeof(float64) * ACPOffsetMeasArraySize);
      ACPUpperAbsolutePower = (float64 *)malloc(sizeof(float64) * ACPOffsetMeasArraySize);
      if (ACPLowerRelativePower && ACPUpperRelativePower && ACPLowerAbsolutePower && ACPUpperAbsolutePower)
      {
         RFmxCheckWarn(RFmxLTE_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, ACPLowerRelativePower,
            ACPUpperRelativePower, ACPLowerAbsolutePower, ACPUpperAbsolutePower, ACPOffsetMeasArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxLTE_ACPFetchComponentCarrierMeasurement(instrumentHandle, "", timeout,
      &ACPAbsolutePower, NULL));

   RFmxCheckWarn(RFmxLTE_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL,
      0, &SEMLowerOffsetMeasArraySize));
   if (SEMLowerOffsetMeasArraySize > 0)
   {
      SEMLowerOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * SEMLowerOffsetMeasArraySize);
      SEMLowerOffsetMargin = (float64 *)malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
      SEMLowerOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
      SEMLowerOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
      if (SEMLowerOffsetMeasurementStatus && SEMLowerOffsetMargin && SEMLowerOffsetMarginFrequency &&
         SEMLowerOffsetMarginAbsolutePower)
      {
         RFmxCheckWarn(RFmxLTE_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout,
            SEMLowerOffsetMeasurementStatus, SEMLowerOffsetMargin, SEMLowerOffsetMarginFrequency,
            SEMLowerOffsetMarginAbsolutePower, NULL, SEMLowerOffsetMeasArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxLTE_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL,
      0, &SEMUpperOffsetMeasArraySize));
   if (SEMUpperOffsetMeasArraySize > 0)
   {
      SEMUpperOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * SEMUpperOffsetMeasArraySize);
      SEMUpperOffsetMargin = (float64 *)malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
      SEMUpperOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
      SEMUpperOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
      if (SEMUpperOffsetMeasurementStatus && SEMUpperOffsetMargin && SEMUpperOffsetMarginFrequency &&
         SEMUpperOffsetMarginAbsolutePower)
      {
         RFmxCheckWarn(RFmxLTE_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout,
            SEMUpperOffsetMeasurementStatus, SEMUpperOffsetMargin, SEMUpperOffsetMarginFrequency,
            SEMUpperOffsetMarginAbsolutePower, NULL, SEMUpperOffsetMeasArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxLTE_SEMFetchComponentCarrierMeasurement(instrumentHandle, "", timeout,
      &SEMAbsoluteIntegratedPower, NULL));

   RFmxCheckWarn(RFmxLTE_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &SEMMeasurementStatus));

   RFmxCheckWarn(RFmxLTE_OBWFetchMeasurement(instrumentHandle, "", timeout, &OBWOccupiedBandwidth,
      &OBWAbsolutePower, &OBWStartFrequency, &OBWStopFrequency));

   RFmxCheckWarn(RFmxLTE_CHPFetchComponentCarrierMeasurement(instrumentHandle, "", timeout,
      &CHPAbsolutePower, NULL));


   printf("************************* ModAcc *************************\n\n");
   printf("Mean RMS Composite EVM (%% or dB)        : %lf\n", meanRMSCompositeEVM);
   printf("Max Peak Composite EVM (%% or dB)        : %lf\n", maxPeakCompositeEVM);
   printf("Peak Composite EVM Slot Index           : %d\n", peakCompositeEVMSlotIndex);
   printf("Peak Composite EVM Symbol Index         : %d\n", peakCompositeEVMSymbolIndex);
   printf("Peak Composite EVM Subcarrier Index     : %d\n", peakCompositeEVMSubcarrierIndex);
   printf("Mean Frequency Error (Hz)               : %lf\n", meanFrequencyError);
   printf("Mean IQ Origin Offset (dBc)             : %lf\n", meanIQOriginOffset);
   printf("Mean IQ Gain Imbalance (dB)             : %lf\n", meanIQGainImbalance);
   printf("Mean IQ Quadrature Error (deg)          : %lf\n", meanIQQuadratureError);
   printf("In-Band Emission Margin (dB)            : %lf\n", inBandEmissionMargin);

   printf("\n\n************************* ACP *************************\n\n");
   printf("Carrier Absolute Power (dBm)   : %lf\n", ACPAbsolutePower);
   printf("\n------- Offset Channel Measurements ------- \n");
   for (i = 0; i < ACPOffsetMeasArraySize; i++)
   {
      printf("\nOffset %d\n", i);
      printf("Lower Relative Power (dB)     : %lf\n", ACPLowerRelativePower[i]);
      printf("Upper Relative Power (dB)     : %lf\n", ACPUpperRelativePower[i]);
      printf("Lower Absolute Power (dBm)    : %lf\n", ACPLowerAbsolutePower[i]);
      printf("Upper Absolute Power (dBm)    : %lf\n", ACPUpperAbsolutePower[i]);
   }

   printf("\n\n************************* SEM *************************\n\n");
   printf("Measurement Status                         : %s\n", (SEMMeasurementStatus) ? "PASS" : "FAIL");
   printf("Carrier Absolute Integrated Power (dBm)    : %lf\n", SEMAbsoluteIntegratedPower);
   printf("\n----- Lower Offset Segment Measurements -----\n");
   for (i = 0; i < SEMLowerOffsetMeasArraySize; i++)
   {
      printf("\nOffset %d\n", i);
      printf("Measurement Status             : %s\n", (SEMLowerOffsetMeasurementStatus[i]) ? "PASS" : "FAIL");
      printf("Margin (dB)                    : %lf\n", SEMLowerOffsetMargin[i]);
      printf("Margin Frequency (Hz)          : %lf\n", SEMLowerOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)    : %lf\n", SEMLowerOffsetMarginAbsolutePower[i]);
   }
   printf("\n----- Upper Offset Segment Measurements -----\n");
   for (i = 0; i < SEMUpperOffsetMeasArraySize; i++)
   {
      printf("\nOffset %d\n", i);
      printf("Measurement Status             : %s\n", (SEMUpperOffsetMeasurementStatus[i]) ? "PASS" : "FAIL");
      printf("Margin (dB)                    : %lf\n", SEMUpperOffsetMargin[i]);
      printf("Margin Frequency (Hz)          : %lf\n", SEMUpperOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)    : %lf\n", SEMUpperOffsetMarginAbsolutePower[i]);
   }

   printf("\n\n************************* OBW *************************\n\n");
   printf("Occupied Bandwidth (Hz)        : %lf\n", OBWOccupiedBandwidth);
   printf("Absolute Power (dBm)           : %lf\n", OBWAbsolutePower);
   printf("Start Frequency (Hz)           : %lf\n", OBWStartFrequency);
   printf("Stop Frequency (Hz)            : %lf\n", OBWStopFrequency);

   printf("\n\n************************* CHP *************************\n\n");
   printf("Carrier Absolute Power (dBm)   : %lf\n\n", CHPAbsolutePower);

Error:
   if (error)
   {
      RFmxLTE_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (instrumentHandle)
   {
      RFmxLTE_Close(instrumentHandle, RFMXLTE_VAL_FALSE);
   }

   /* Free allocated memory */
   if (ACPLowerAbsolutePower)
   {
      free(ACPLowerAbsolutePower);
   }
   if (ACPUpperAbsolutePower)
   {
      free(ACPUpperAbsolutePower);
   }
   if (ACPLowerRelativePower)
   {
      free(ACPLowerRelativePower);
   }
   if (ACPUpperRelativePower)
   {
      free(ACPUpperRelativePower);
   }
   if (SEMLowerOffsetMeasurementStatus) 
   {
      free(SEMLowerOffsetMeasurementStatus);
   }
   if (SEMLowerOffsetMargin) 
   {
      free(SEMLowerOffsetMargin);
   }
   if (SEMLowerOffsetMarginFrequency) 
   {
      free(SEMLowerOffsetMarginFrequency);
   }
   if (SEMLowerOffsetMarginAbsolutePower) 
   {
      free(SEMLowerOffsetMarginAbsolutePower);
   }
   if (SEMUpperOffsetMeasurementStatus) 
   {
      free(SEMUpperOffsetMeasurementStatus);
   }
   if (SEMUpperOffsetMargin) 
   {
      free(SEMUpperOffsetMargin);
   }
   if (SEMUpperOffsetMarginFrequency) 
   {
      free(SEMUpperOffsetMarginFrequency);
   }
   if (SEMUpperOffsetMarginAbsolutePower) 
   {
      free(SEMUpperOffsetMarginAbsolutePower);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
