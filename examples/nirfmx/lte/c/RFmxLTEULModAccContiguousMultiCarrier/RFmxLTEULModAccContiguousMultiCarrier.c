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
//10. Select ModAcc measurement and enable Traces.
//11. Configure Synchronization Mode and Measurement Interval.
//12. Configure EVM Unit.
//13. Configure In-Band Emission Mask Type.
//14. Configure Averaging Parameters for ModAcc measurement.
//15. Initiate the Measurement.
//16. Fetch ModAcc Measurements and Traces.
//17. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION           4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING             256
#define NUMBER_OF_COMPONENT_CARRIERS    2

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0;

   char subblockCarrierString[NUMBER_OF_COMPONENT_CARRIERS][MAX_SELECTOR_STRING];

   char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                         /*(Hz) */

   float64 centerFrequency = 1.95e9;                                   /*(Hz) */
   float64 referenceLevel = 0.0;                                       /*(dBm) */
   float64 externalAttenuation = 0.0;                                  /*(dB) */

   int32 enableTrigger = RFMXLTE_VAL_FALSE;
   char * digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
   int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                         /*(s) */

   int32 componentCarrierSpacingType = RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL;
   int32 componentCarrierAtCenterFrequency = -1;

   int32 band = 1;
   int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_FDD;
   int32 uplinkDownlinkConfiguraiton = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;

   float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS] = { 20e6, 20e6 };
   float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS] = { -9.9e6, 9.9e6 };
   int32 componentCarrierCellId[NUMBER_OF_COMPONENT_CARRIERS] = { 0, 0 };

   int32 averagingEnabled = RFMXLTE_VAL_MODACC_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   int32 synchronizationMode = RFMXLTE_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;
   int32 measurementOffset = 0;                                         /*(slots) */
   int32 measurementLength = 1;                                         /*(slots) */

   int32 evmUnit = RFMXLTE_VAL_MODACC_EVM_UNIT_PERCENTAGE;

   int32 inBandEmissionMaskType = RFMXLTE_VAL_MODACC_IN_BAND_EMISSION_MASK_TYPE_RELEASE_11_ONWARDS;

   int32 autoDMRSDetectionEnabled = RFMXLTE_VAL_AUTO_DMRS_DETECTION_ENABLED_TRUE;

   float64 timeout = 10.000000;                                         /*(s) */
   int32 peakCompositeEVMSubcarrierIndex[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };
   int32 peakCompositeEVMSymbolIndex[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };
   float64  meanRMSCompositeEVM[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };                                /*(% or dB) */
   float64 maximumPeakCompositeEVM[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };                             /*(% or dB) */
   float64  meanFrequencyError[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };                                 /*(Hz) */
   int32 peakCompositeEVMSlotIndex[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };

   float64 meanIQOriginOffset[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };                                  /*(dBc) */
   float64 meanIQGainImbalance[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };                                 /*(dB) */
   float64 meanIQQuadratureError[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };                               /*(deg) */

   float64 inBandEmissionMargin[NUMBER_OF_COMPONENT_CARRIERS] = { 0 };                                /*(dB) */

   NIComplexSingle* dataConstellation[NUMBER_OF_COMPONENT_CARRIERS] = { NULL };
   NIComplexSingle* DMRSConstellation[NUMBER_OF_COMPONENT_CARRIERS] = { NULL };

   float32* meanRMSEVMPerSubcarrier[NUMBER_OF_COMPONENT_CARRIERS] = { NULL };
   float64 x0[NUMBER_OF_COMPONENT_CARRIERS];
   float64    dx[NUMBER_OF_COMPONENT_CARRIERS];

   int32 actualArraySize = 0;
   int32 dataConstellationActualArraySize = 0;
   int32 DMRSConstellationActualArraySize = 0;

   /* Initialize a session */
   RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay,
      enableTrigger));
   RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, "", componentCarrierSpacingType,
      componentCarrierAtCenterFrequency));
   RFmxCheckWarn(RFmxLTE_CfgBand(instrumentHandle, "", band));
   RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));
   RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, "", NUMBER_OF_COMPONENT_CARRIERS));
   RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, "", componentCarrierBandwidth,
      componentCarrierFrequency, componentCarrierCellId, NUMBER_OF_COMPONENT_CARRIERS));
   RFmxCheckWarn(RFmxLTE_CfgAutoDMRSDetectionEnabled(instrumentHandle, "", autoDMRSDetectionEnabled));
   RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_MODACC, RFMXLTE_VAL_TRUE));
   RFmxCheckWarn(RFmxLTE_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode,
      measurementOffset, measurementLength));
   RFmxCheckWarn(RFmxLTE_ModAccCfgEVMUnit(instrumentHandle, "", evmUnit));
   RFmxCheckWarn(RFmxLTE_ModAccCfgInBandEmissionMaskType(instrumentHandle, "", inBandEmissionMaskType));
   RFmxCheckWarn(RFmxLTE_ModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
   RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

   RFmxCheckWarn(RFmxLTE_ModAccFetchCompositeEVMArray(instrumentHandle, "", timeout, meanRMSCompositeEVM,
      maximumPeakCompositeEVM, meanFrequencyError, peakCompositeEVMSymbolIndex,
      peakCompositeEVMSubcarrierIndex, peakCompositeEVMSlotIndex,
      NUMBER_OF_COMPONENT_CARRIERS, NULL));

   RFmxCheckWarn(RFmxLTE_ModAccFetchIQImpairmentsArray(instrumentHandle, "", timeout, meanIQOriginOffset,
      meanIQGainImbalance, meanIQQuadratureError, NUMBER_OF_COMPONENT_CARRIERS, NULL));

   RFmxCheckWarn(RFmxLTE_ModAccFetchInBandEmissionMarginArray(instrumentHandle, "", timeout, inBandEmissionMargin,
      NUMBER_OF_COMPONENT_CARRIERS, NULL));

   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      RFmxLTE_BuildCarrierString("", i, MAX_SELECTOR_STRING, subblockCarrierString[i]);
      RFmxCheckWarn(RFmxLTE_ModAccFetchPUSCHConstellationTrace(instrumentHandle, subblockCarrierString[i], timeout,
         NULL, 0, &dataConstellationActualArraySize,
         NULL, 0, &DMRSConstellationActualArraySize));

      if (dataConstellationActualArraySize && DMRSConstellationActualArraySize > 0)
      {
         dataConstellation[i] = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * dataConstellationActualArraySize);
         DMRSConstellation[i] = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * DMRSConstellationActualArraySize);
         if (dataConstellation[i] && DMRSConstellation[i])
         {
            RFmxCheckWarn(RFmxLTE_ModAccFetchPUSCHConstellationTrace(instrumentHandle, subblockCarrierString[i],
               timeout, dataConstellation[i], dataConstellationActualArraySize, NULL,
               DMRSConstellation[i], DMRSConstellationActualArraySize, NULL));
         }
         else
         {
            printf("malloc failed.\n");
            goto Error;
         }
      }

      actualArraySize = 0;
      RFmxCheckWarn(RFmxLTE_ModAccFetchEVMPerSubcarrierTrace(instrumentHandle, subblockCarrierString[i], timeout,
         NULL, NULL, NULL, 0, &actualArraySize));
      if (actualArraySize > 0)
      {
         meanRMSEVMPerSubcarrier[i] = (float32 *)malloc(sizeof(float32) * actualArraySize);
         if (meanRMSEVMPerSubcarrier[i])
         {
            RFmxCheckWarn(RFmxLTE_ModAccFetchEVMPerSubcarrierTrace(instrumentHandle, subblockCarrierString[i],
               timeout, &x0[i], &dx[i], meanRMSEVMPerSubcarrier[i], actualArraySize, 0));
         }
         else
         {
            printf("malloc failed.\n");
            goto Error;
         }
      }
   }

   printf("------------------------Measurements------------------------\n\n");
   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      printf("Carrier  : %d\n", i);
      printf("Mean RMS Composite EVM  (%% or dB)       : %lf\n", meanRMSCompositeEVM[i]);
      printf("Max Peak Composite EVM  (%% or dB)       : %lf\n", maximumPeakCompositeEVM[i]);
      printf("Peak Composite EVM Slot Index           : %d\n", peakCompositeEVMSlotIndex[i]);
      printf("Peak Composite EVM Symbol Index         : %d\n", peakCompositeEVMSymbolIndex[i]);
      printf("Peak Composite EVM Subcarrier Index     : %d\n", peakCompositeEVMSubcarrierIndex[i]);
      printf("Mean Frequency Error  (Hz)              : %lf\n", meanFrequencyError[i]);
      printf("Mean IQ Origin Offset  (dBc)            : %lf\n", meanIQOriginOffset[i]);
      printf("Mean IQ Gain Imbalance  (dB)            : %lf\n", meanIQGainImbalance[i]);
      printf("Mean IQ Quadrature Error  (deg)         : %lf\n", meanIQQuadratureError[i]);
      printf("In-Band Emission Margin  (dB)           : %lf\n", inBandEmissionMargin[i]);
      printf("-------------------------------------------------\n\n");
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

   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      if (dataConstellation[i]) {
         free(dataConstellation[i]);
      }
      if (DMRSConstellation[i]) {
         free(DMRSConstellation[i]);
      }
      if (meanRMSEVMPerSubcarrier[i]) {
         free(meanRMSEVMPerSubcarrier[i]);
      }
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
