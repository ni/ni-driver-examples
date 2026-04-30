//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure operating Band.
//7. Configure Duplex Mode.
//8. Configure Component Carriers.
//9. Configure Auto DMRS Detection Enabled.
//10. Select ACP,CHP,ModAcc,OBW and SEM measurements and enable Traces.
//11. Configure Averaging Parameters for ModAcc.
//12. Configure Averaging Parameters for ACP.
//13. Configure Averaging Parameters for CHP.
//14. Configure Averaging Parameters for OBW.
//15. Configure Averaging Parameters for SEM.
//16. Configure ACP Sweep Time.
//17. Configure CHP Sweep Time.
//18. Configure OBW Sweep Time.
//19. Configure SEM Sweep Time.
//20. Configure Uplink Mask Type for SEM.
//21. Configure Synchronization Mode and Measurement Interval.
//22. Initiate the Measurement.
//23. Fetch SEM Measurements.
//24. Fetch OBW Measurements.
//25. Fetch CHP Measurements.
//26. Fetch ACP Measurements.
//27. Fetch ModAcc Measurements.
//28. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION               4096

#define NUMBER_OF_COMPONENT_CARRIERS        2

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0;

   float64 centerFrequency = 1.95e9;                                                      /*(Hz) */
   float64 externalAttenuation = 0.0;                                                     /*(dB) */
   float64 referenceLevel = 0.0;                                                          /*(dBm) */

   char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                                            /*(Hz) */

   int32 enableTrigger = RFMXLTE_VAL_FALSE;
   char * digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
   int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                                            /*(s) */

   int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_FDD;
   int32 uplinkDownlinkConfiguraiton = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;

   int32 modAccBand = 1;

   int32 componentCarrierSpacingType = RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL;
   int32 componentCarrierAtCenterFrequency = -1;

   float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS] = { 5e6, 20e6 };         /*(Hz) */
   float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS] = { -9.225e6, 2.475e6 }; /*(Hz) */
   int32 cellID[NUMBER_OF_COMPONENT_CARRIERS] = { 0, 0 };

   int32 synchronizationMode = RFMXLTE_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;
   int32 measurementOffset = 0;                                                     /*(slots) */
   int32 measurementLength = 1;                                                     /*(slots) */

   int32 uplinkMaskType = RFMXLTE_VAL_SEM_UPLINK_MASK_TYPE_GENERAL_NS01;

   float64 sweepTimeInterval = 0.001;                                               /*(s) */

   int32 averagingCount = 10;

   int32 autoDMRSDetectionEnabled = RFMXLTE_VAL_AUTO_DMRS_DETECTION_ENABLED_TRUE;

   float64 timeout = 10.0;                                                          /*(s) */

   float64 ModAccMeanRMSCompositeEVM[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };         /*(% or dB) */
   float64 ModAccMaxPeakCompositeEVM[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };         /*(% or dB) */
   float64 ModAccMeanFrequencyError[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };          /*(Hz) */
   float64 ModAccMeanIQOriginOffset[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };          /*(dB) */

   float64 CHPTotalAggregatedPower = 0.0;                                           /*(dBm) */
   float64 CHPAbsolutePower[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };                  /*(dBm) */
   float64 CHPRelativePower[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };                  /*(dB) */

   float64 ACPTotalAggregatedPower = 0.0;                                           /*(dBm) */
   float64 ACPAbsolutePower[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };                  /*(dBm) */
   float64 ACPRelativePower[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };                  /*(dB) */
   int32 ACPOffsetMeasArraySize = 0;
   float64* ACPLowerAbsolutePower = NULL;                                           /*(dBm) */
   float64* ACPUpperAbsolutePower = NULL;                                           /*(dBm) */
   float64* ACPLowerRelativePower = NULL;                                           /*(dB) */
   float64* ACPUpperRelativePower = NULL;                                           /*(dB) */

   float64 OBWOccupiedBandwidth = 0.0;                                              /*(Hz) */
   float64 OBWAbsolutePower = 0.0;                                                  /*(dBm) */
   float64 OBWStopFrequency = 0.0;                                                  /*(Hz) */
   float64 OBWStartFrequency = 0.0;                                                 /*(Hz) */

   int32 SEMMeasurementStatus = 0;
   float64 SEMTotalAggregatedPower = 0.0;                                           /*(dBm) */
   int32 SEMCCMeasArraySize = 0;
   float64* SEMAbsoluteIntegratedPower = NULL;                                      /*(dBm) */
   float64* SEMRelativeIntegratedPower = NULL;                                      /*(dB) */
   int32 SEMLowerOffsetMeasArraySize = 0, SEMUpperOffsetMeasArraySize = 0;
   int32* SEMLowerOffsetMeasurementStatus = NULL;
   float64* SEMLowerOffsetMargin = NULL;                                            /*(dB) */
   float64* SEMLowerOffsetMarginFrequency = NULL;                                   /*(Hz) */
   float64* SEMLowerOffsetMarginAbsolutePower = NULL;                               /*(dBm) */
   int32* SEMUpperOffsetMeasurementStatus = NULL;
   float64* SEMUpperOffsetMargin = NULL;                                            /*(dB) */
   float64* SEMUpperOffsetMarginFrequency = NULL;                                   /*(Hz) */
   float64* SEMUpperOffsetMarginAbsolutePower = NULL;                               /*(dBm) */

   /* Initialize a session */
   RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay,
      enableTrigger));

   RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, "", componentCarrierSpacingType,
      componentCarrierAtCenterFrequency));
   RFmxCheckWarn(RFmxLTE_CfgBand(instrumentHandle, "", modAccBand));
   RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));
   RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, "", NUMBER_OF_COMPONENT_CARRIERS));
   RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, "", componentCarrierBandwidth,
      componentCarrierFrequency, cellID, NUMBER_OF_COMPONENT_CARRIERS));
   RFmxCheckWarn(RFmxLTE_CfgAutoDMRSDetectionEnabled(instrumentHandle, "", autoDMRSDetectionEnabled));

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

   RFmxCheckWarn(RFmxLTE_SEMCfgUplinkMaskType(instrumentHandle, "", uplinkMaskType));
   RFmxCheckWarn(RFmxLTE_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode,
      measurementOffset, measurementLength));

   RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxLTE_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL,
      NULL, NULL, NULL, 0, &SEMLowerOffsetMeasArraySize));
   if (SEMLowerOffsetMeasArraySize > 0)
   {
      SEMLowerOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * SEMLowerOffsetMeasArraySize);
      SEMLowerOffsetMargin = (float64 *)malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
      SEMLowerOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
      SEMLowerOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
      if (SEMLowerOffsetMeasurementStatus && SEMLowerOffsetMargin && SEMLowerOffsetMarginFrequency && SEMLowerOffsetMarginAbsolutePower)
      {
         RFmxCheckWarn(RFmxLTE_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout,
            SEMLowerOffsetMeasurementStatus,
            SEMLowerOffsetMargin,
            SEMLowerOffsetMarginFrequency,
            SEMLowerOffsetMarginAbsolutePower,
            NULL,
            SEMLowerOffsetMeasArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxLTE_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL,
      NULL, NULL, NULL, 0, &SEMUpperOffsetMeasArraySize
   ));
   if (SEMUpperOffsetMeasArraySize > 0)
   {
      SEMUpperOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * SEMUpperOffsetMeasArraySize);
      SEMUpperOffsetMargin = (float64 *)malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
      SEMUpperOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
      SEMUpperOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
      if (SEMUpperOffsetMeasurementStatus && SEMUpperOffsetMargin && SEMUpperOffsetMarginFrequency && SEMUpperOffsetMarginAbsolutePower)
      {
         RFmxCheckWarn(RFmxLTE_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout,
            SEMUpperOffsetMeasurementStatus,
            SEMUpperOffsetMargin,
            SEMUpperOffsetMarginFrequency,
            SEMUpperOffsetMarginAbsolutePower,
            NULL,
            SEMUpperOffsetMeasArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxLTE_SEMFetchComponentCarrierMeasurementArray(instrumentHandle, "", timeout,
      NULL, NULL, 0, &SEMCCMeasArraySize));
   if (SEMCCMeasArraySize > 0)
   {
      SEMAbsoluteIntegratedPower = (float64 *)malloc(sizeof(float64) * SEMCCMeasArraySize);
      SEMRelativeIntegratedPower = (float64 *)malloc(sizeof(float64) * SEMCCMeasArraySize);
      if (SEMAbsoluteIntegratedPower && SEMRelativeIntegratedPower)
      {
         RFmxCheckWarn(RFmxLTE_SEMFetchComponentCarrierMeasurementArray(instrumentHandle, "", timeout,
            SEMAbsoluteIntegratedPower,
            SEMRelativeIntegratedPower,
            SEMCCMeasArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxLTE_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &SEMMeasurementStatus));

   RFmxCheckWarn(RFmxLTE_SEMFetchTotalAggregatedPower(instrumentHandle, "", timeout, &SEMTotalAggregatedPower));

   RFmxCheckWarn(RFmxLTE_OBWFetchMeasurement(instrumentHandle, "", timeout, &OBWOccupiedBandwidth,
      &OBWAbsolutePower, &OBWStartFrequency, &OBWStopFrequency));

   RFmxCheckWarn(RFmxLTE_CHPFetchComponentCarrierMeasurementArray(instrumentHandle, "", timeout,
      CHPAbsolutePower, CHPRelativePower, NUMBER_OF_COMPONENT_CARRIERS, NULL));

   RFmxCheckWarn(RFmxLTE_CHPFetchTotalAggregatedPower(instrumentHandle, "", timeout, &CHPTotalAggregatedPower));

   RFmxCheckWarn(RFmxLTE_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout,
      NULL, NULL, NULL, NULL, 0,
      &ACPOffsetMeasArraySize));
   if (ACPOffsetMeasArraySize > 0)
   {
      ACPLowerRelativePower = (float64 *)malloc(sizeof(float64) * ACPOffsetMeasArraySize);
      ACPUpperRelativePower = (float64 *)malloc(sizeof(float64) * ACPOffsetMeasArraySize);
      ACPLowerAbsolutePower = (float64 *)malloc(sizeof(float64) * ACPOffsetMeasArraySize);
      ACPUpperAbsolutePower = (float64 *)malloc(sizeof(float64) * ACPOffsetMeasArraySize);
      if (ACPLowerRelativePower && ACPUpperRelativePower && ACPLowerAbsolutePower && ACPUpperAbsolutePower)
      {
         RFmxCheckWarn(RFmxLTE_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout,
            ACPLowerRelativePower, ACPUpperRelativePower,
            ACPLowerAbsolutePower, ACPUpperAbsolutePower,
            ACPOffsetMeasArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxLTE_ACPFetchComponentCarrierMeasurementArray(instrumentHandle, "", timeout,
      ACPAbsolutePower, ACPRelativePower,
      NUMBER_OF_COMPONENT_CARRIERS, NULL));

   RFmxCheckWarn(RFmxLTE_ACPFetchTotalAggregatedPower(instrumentHandle, "", timeout, &ACPTotalAggregatedPower));

   RFmxCheckWarn(RFmxLTE_ModAccFetchCompositeEVMArray(instrumentHandle, "", timeout,
      ModAccMeanRMSCompositeEVM, ModAccMaxPeakCompositeEVM,
      ModAccMeanFrequencyError, NULL, NULL, NULL, NUMBER_OF_COMPONENT_CARRIERS, NULL));

   RFmxCheckWarn(RFmxLTE_ModAccFetchIQImpairmentsArray(instrumentHandle, "", timeout,
      ModAccMeanIQOriginOffset, NULL, NULL,
      NUMBER_OF_COMPONENT_CARRIERS, NULL));

   printf("************************* ModAcc *************************\n\n");
   printf("---------------- Measurements ----------------\n");
   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      printf("Carrier  : %d\n", i);
      printf("Mean RMS Composite EVM  (%%)  : %lf\n", ModAccMeanRMSCompositeEVM[i]);
      printf("Max Peak Composite EVM  (%%)  : %lf\n", ModAccMaxPeakCompositeEVM[i]);
      printf("Mean Frequency Error    (Hz) : %lf\n", ModAccMeanFrequencyError[i]);
      printf("Mean IQ Origin Offset   (dBc) : %lf\n", ModAccMeanIQOriginOffset[i]);
   }

   printf("\n************************* CHP *************************\n\n");
   printf("Total Aggregated Power  (dBm): %lf\n", CHPTotalAggregatedPower);
   printf("\n----- Component Carrier Measurements ------\n");
   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      printf("Carrier  : %d\n", i);
      printf("Absolute Power  (dBm)        : %lf\n", CHPAbsolutePower[i]);
      printf("Relative Power  (dB)         : %lf\n", CHPRelativePower[i]);
   }

   printf("\n************************* ACP *************************\n\n");
   printf("Total Aggregated Power  (dBm): %lf\n", ACPTotalAggregatedPower);
   printf("\n----- Component Carrier Measurements ------\n");
   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      printf("\nCarrier  : %d\n", i);
      printf("Absolute Power  (dBm)        : %lf\n", ACPAbsolutePower[i]);
      printf("Relative Power  (dB)         : %lf\n", ACPRelativePower[i]);
   }
   printf("\n------ Offset Channel Measurements -------\n");
   for (i = 0; i < ACPOffsetMeasArraySize; i++)
   {
      printf("\nOffset  : %d\n", i);
      printf("Lower Relative Power (dB)    : %lf\n", ACPLowerRelativePower[i]);
      printf("Upper Relative Power (dB)    : %lf\n", ACPUpperRelativePower[i]);
      printf("Lower Absolute Power (dBm)   : %lf\n", ACPLowerAbsolutePower[i]);
      printf("Upper Absolute Power (dBm)   : %lf\n", ACPUpperAbsolutePower[i]);
   }

   printf("\n************************* OBW *************************\n\n");
   printf("---------------- Measurement ----------------\n");
   printf("Occupied Bandwidth  (Hz)     : %lf\n", OBWOccupiedBandwidth);
   printf("Absolute Power  (dBm)        : %lf\n", OBWAbsolutePower);
   printf("Start Frequency  (Hz)        : %lf\n", OBWStartFrequency);
   printf("Stop Frequency  (Hz)         : %lf\n", OBWStopFrequency);

   printf("\n************************* SEM *************************\n\n");
   printf("Measurement Status           : %s\n", (SEMMeasurementStatus) ? "PASS" : "FAIL");
   printf("Total Aggregated Power  (dBm): %lf\n", SEMTotalAggregatedPower);
   printf("\n----- Component Carrier Measurements ------\n");
   for (i = 0; i < SEMCCMeasArraySize; i++)
   {
      printf("Carrier  : %d\n", i);
      printf("Absolute Integrated Power  (dBm)        : %lf\n", SEMAbsoluteIntegratedPower[i]);
      printf("Relative Integrated Power  (dB)         : %lf\n", SEMRelativeIntegratedPower[i]);
   }
   printf("\n---- Lower Offset Segment Measurements ----\n");
   for (i = 0; i < SEMLowerOffsetMeasArraySize; i++)
   {
      printf("\nOffset  : %d\n", i);
      printf("Measurement Status           : %s\n",
         (SEMLowerOffsetMeasurementStatus[i]) ? "PASS" : "FAIL");
      printf("Margin  (dB)                 : %lf\n", SEMLowerOffsetMargin[i]);
      printf("Margin Frequency  (Hz)       : %lf\n", SEMLowerOffsetMarginFrequency[i]);
      printf("Margin Absolute Power  (dBm) : %lf\n", SEMLowerOffsetMarginAbsolutePower[i]);
   }
   printf("\n---- Upper Offset Segment Measurements ----\n");
   for (i = 0; i < SEMUpperOffsetMeasArraySize; i++)
   {
      printf("\nOffset  : %d\n", i);
      printf("Measurement Status           : %s\n",
         (SEMUpperOffsetMeasurementStatus[i]) ? "PASS" : "FAIL");
      printf("Margin  (dB)                 : %lf\n", SEMUpperOffsetMargin[i]);
      printf("Margin Frequency  (Hz)       : %lf\n", SEMUpperOffsetMarginFrequency[i]);
      printf("Margin Absolute Power  (dBm) : %lf\n", SEMUpperOffsetMarginAbsolutePower[i]);
   }

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
   if (SEMAbsoluteIntegratedPower)
   {
      free(SEMAbsoluteIntegratedPower);
   }
   if (SEMRelativeIntegratedPower)
   {
      free(SEMRelativeIntegratedPower);
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

   printf("\nPress any key to exit\n");
   _getch();

   return error;
}
