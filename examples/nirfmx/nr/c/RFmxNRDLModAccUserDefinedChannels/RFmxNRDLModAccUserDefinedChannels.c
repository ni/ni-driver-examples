//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Link Direction as Downlink, Frequency Range, Carrier Bandwidth, Cell ID, BWP Subcarrier Spacing,
//   Auto RB Detection Enabled, DL Channel Configuration Mode and Auto Increment Cell ID Enabled.
//7. Configure PDSCH and PDSCH RB Allocation.
//8. Configure PDSCH DMRS.
//9. Configure SSB.
//10. Select ModAcc measurement and enable Traces.
//11. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
//12. Configure Measurement Interval.
//13. Initiate the Measurement.
//14. Fetch ModAcc Measurements and Traces.
//15. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxNR.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                     256

#define NUMBER_OF_RESOURCE_BLOCK_CLUSTERS       1

int main(int argc, char *argv[])
{
   char* resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int32 i = 0;

   char subblockString[MAX_SELECTOR_STRING];
   char carrierString[MAX_SELECTOR_STRING];
   char bandwidthPartString[MAX_SELECTOR_STRING];
   char userString[MAX_SELECTOR_STRING];
   char PDSCHString[MAX_SELECTOR_STRING];
   char PDSCHClusterString[MAX_SELECTOR_STRING];

   char* selectedPorts = "";
   float64 centerFrequency = 3.5e9;                                                       /* (Hz) */
   float64 referenceLevel = 0.0;                                                          /* (dBm) */
   float64 externalAttenuation = 0.0;                                                     /* (dB) */

   char* frequencyReferenceSource = RFMXNR_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e6;                                          /* (Hz) */

   int32 enableTrigger = RFMXNR_VAL_FALSE;
   char* digitalEdgeSource = RFMXNR_VAL_PXI_TRIG0_STR;
   int32 digitalEdge = RFMXNR_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                                            /* (s) */

   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;
   int32 cellID = 0;
   float64 carrierBandwidth = 100e6;                                                      /* (Hz) */
   float64 subcarrierSpacing = 30e3;                                                      /* (Hz) */
   int32 autoResourceBlockDetectionEnabled = RFMXNR_VAL_AUTO_RESOURCE_BLOCK_DETECTION_ENABLED_TRUE;
   int32 autoIncrementCellIDEnabled = RFMXNR_VAL_AUTO_INCREMENT_CELL_ID_ENABLED_TRUE;

   int32 PDSCHModulationType = RFMXNR_VAL_PDSCH_MODULATION_TYPE_QPSK;
   int32 PDSCHResourceBlockOffset[NUMBER_OF_RESOURCE_BLOCK_CLUSTERS] = { 0 };
   int32 PDSCHNumberOfResourceBlocks[NUMBER_OF_RESOURCE_BLOCK_CLUSTERS] = { -1 };
   char* PDSCHSlotAllocation = "0-Last";
   char* PDSCHSymbolAllocation = "0-Last";

   int32 PDSCHDMRSPowerMode = RFMXNR_VAL_PDSCH_DMRS_POWER_MODE_CDM_GROUPS;
   float64 PDSCHDMRSPower = 0.0;                                                          /* (dB) */
   int32 PDSCHDMRSConfigurationType = RFMXNR_VAL_PDSCH_DMRS_CONFIGURATION_TYPE_TYPE1;
   int32 PDSCHMappingType = RFMXNR_VAL_PDSCH_MAPPING_TYPE_TYPE_A;
   int32 PDSCHDMRSTypeAPosition = 2;
   int32 PDSCHDMRSDuration = RFMXNR_VAL_PDSCH_DMRS_DURATION_SINGLE_SYMBOL;
   int32 PDSCHDMRSAdditionalPositions = 0;

   int32 SSBEnabled = RFMXNR_VAL_SSB_ENABLED_FALSE;
   int32 SSBCRBOffset = 0;
   int32 SSBSubcarrierOffset = 0;
   int32 SSBPattern = RFMXNR_VAL_SSB_PATTERN_CASE_B_3GHZ_TO_6GHZ;

   int32 synchronizationMode = RFMXNR_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;

   int32 measurementLengthUnit = RFMXNR_VAL_MODACC_MEASUREMENT_LENGTH_UNIT_SLOT;
   float64 measurementOffset = 0.0;
   float64 measurementLength = 1;

   int32 averagingEnabled = RFMXNR_VAL_MODACC_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   float64 timeout = 10.000000;                                                           /* (s) */

   float64 compositeRMSEVMMean = 0.0;                                                     /* (%) */
   float64 compositePeakEVMMaximum = 0.0;                                                 /* (%) */
   int32 compositePeakEVMSlotIndex = 0;
   int32 compositePeakEVMSymbolIndex = 0;
   int32 compositePeakEVMSubcarrierIndex = 0;

   float64 PDSCHQPSKRMSEVMMean = 0.0;                                                     /* (%) */
   float64 PDSCH16QAMRMSEVMMean = 0.0;                                                    /* (%) */
   float64 PDSCH64QAMRMSEVMMean = 0.0;                                                    /* (%) */
   float64 PDSCH256QAMRMSEVMMean = 0.0;                                                   /* (%) */

   float64 componentCarrierFrequencyErrorMean = 0.0;                                      /* (Hz) */
   float64 componentCarrierIQOriginOffsetMean = 0.0;                                      /* (dBc) */
   float64 componentCarrierIQGainImbalanceMean = 0.0;                                     /* (dB) */
   float64 componentCarrierQuadratureErrorMean = 0.0;                                     /* (deg) */

   int32 actualArraySize = 0;
   NIComplexSingle* QPSKConstellation = NULL;
   NIComplexSingle* QAM16Constellation = NULL;
   NIComplexSingle* QAM64Constellation = NULL;
   NIComplexSingle* QAM256Constellation = NULL;

   float64 x0 = 0.0, dx = 0.0;
   float32* RMSEVMPerSubcarrierMean = NULL;
   float32* RMSEVMPerSymbolMean = NULL;

   /* Initialize a session */
   RFmxCheckWarn(RFmxNR_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxNR_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxNR_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxNR_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxNR_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge,
      triggerDelay, enableTrigger));

   RFmxCheckWarn(RFmxNR_SetLinkDirection(instrumentHandle, "", RFMXNR_VAL_LINK_DIRECTION_DOWNLINK));
   RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, "", frequencyRange));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, "", carrierBandwidth));
   RFmxCheckWarn(RFmxNR_SetCellID(instrumentHandle, "", cellID));
   RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, "", subcarrierSpacing));
   RFmxCheckWarn(RFmxNR_SetAutoResourceBlockDetectionEnabled(instrumentHandle, "", autoResourceBlockDetectionEnabled));
   RFmxCheckWarn(RFmxNR_SetDownlinkChannelConfigurationMode(instrumentHandle, "",
      RFMXNR_VAL_DOWNLINK_CHANNEL_CONFIGURATION_MODE_USER_DEFINED));
   RFmxCheckWarn(RFmxNR_SetAutoIncrementCellIDEnabled(instrumentHandle, "", autoIncrementCellIDEnabled));

   RFmxCheckWarn(RFmxNR_SetPDSCHModulationType(instrumentHandle, "", PDSCHModulationType));
   RFmxCheckWarn(RFmxNR_SetPDSCHSlotAllocation(instrumentHandle, "", PDSCHSlotAllocation));
   RFmxCheckWarn(RFmxNR_SetPDSCHSymbolAllocation(instrumentHandle, "", PDSCHSymbolAllocation));

   RFmxCheckWarn(RFmxNR_SetPDSCHNumberOfResourceBlockClusters(instrumentHandle, "", NUMBER_OF_RESOURCE_BLOCK_CLUSTERS));

   RFmxNR_BuildSubblockString("", 0, MAX_SELECTOR_STRING, subblockString);
   RFmxNR_BuildCarrierString(subblockString, 0, MAX_SELECTOR_STRING, carrierString);
   RFmxNR_BuildBandwidthPartString(carrierString, 0, MAX_SELECTOR_STRING, bandwidthPartString);
   RFmxNR_BuildUserString(bandwidthPartString, 0, MAX_SELECTOR_STRING, userString);
   RFmxNR_BuildPDSCHString(userString, 0, MAX_SELECTOR_STRING, PDSCHString);
   for (i = 0; i < NUMBER_OF_RESOURCE_BLOCK_CLUSTERS; i++)
   {
      RFmxNR_BuildPDSCHClusterString(PDSCHString, i, MAX_SELECTOR_STRING, PDSCHClusterString);
      RFmxCheckWarn(RFmxNR_SetPDSCHResourceBlockOffset(instrumentHandle, PDSCHClusterString,
         PDSCHResourceBlockOffset[i]));
      RFmxCheckWarn(RFmxNR_SetPDSCHNumberOfResourceBlocks(instrumentHandle, PDSCHClusterString,
         PDSCHNumberOfResourceBlocks[i]));
   }

   RFmxCheckWarn(RFmxNR_SetPDSCHDMRSPowerMode(instrumentHandle, "", PDSCHDMRSPowerMode));
   RFmxCheckWarn(RFmxNR_SetPDSCHDMRSPower(instrumentHandle, "", PDSCHDMRSPower));
   RFmxCheckWarn(RFmxNR_SetPDSCHDMRSConfigurationType(instrumentHandle, "", PDSCHDMRSConfigurationType));
   RFmxCheckWarn(RFmxNR_SetPDSCHMappingType(instrumentHandle, "", PDSCHMappingType));
   RFmxCheckWarn(RFmxNR_SetPDSCHDMRSTypeAPosition(instrumentHandle, "", PDSCHDMRSTypeAPosition));
   RFmxCheckWarn(RFmxNR_SetPDSCHDMRSDuration(instrumentHandle, "", PDSCHDMRSDuration));
   RFmxCheckWarn(RFmxNR_SetPDSCHDMRSAdditionalPositions(instrumentHandle, "", PDSCHDMRSAdditionalPositions));

   RFmxCheckWarn(RFmxNR_SetSSBEnabled(instrumentHandle, "", SSBEnabled));
   RFmxCheckWarn(RFmxNR_SetSSBCRBOffset(instrumentHandle, "", SSBCRBOffset));
   RFmxCheckWarn(RFmxNR_SetSSBSubcarrierOffset(instrumentHandle, "", SSBSubcarrierOffset));
   RFmxCheckWarn(RFmxNR_SetSSBPattern(instrumentHandle, "", SSBPattern));

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_MODACC, RFMXNR_VAL_TRUE));

   RFmxCheckWarn(RFmxNR_ModAccSetSynchronizationMode(instrumentHandle, "", synchronizationMode));
   RFmxCheckWarn(RFmxNR_ModAccSetAveragingEnabled(instrumentHandle, "", averagingEnabled));
   RFmxCheckWarn(RFmxNR_ModAccSetAveragingCount(instrumentHandle, "", averagingCount));

   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementLengthUnit(instrumentHandle, "", measurementLengthUnit));
   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementOffset(instrumentHandle, "", measurementOffset));
   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementLength(instrumentHandle, "", measurementLength));

   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

   /* Fetch results */
   RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositeRMSEVMMean(instrumentHandle, "", &compositeRMSEVMMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMMaximum(instrumentHandle, "", &compositePeakEVMMaximum));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMSlotIndex(instrumentHandle, "", &compositePeakEVMSlotIndex));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMSymbolIndex(instrumentHandle, "",
      &compositePeakEVMSymbolIndex));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMSubcarrierIndex(instrumentHandle, "",
      &compositePeakEVMSubcarrierIndex));

   RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierFrequencyErrorMean(instrumentHandle, "",
      &componentCarrierFrequencyErrorMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierIQOriginOffsetMean(instrumentHandle, "",
      &componentCarrierIQOriginOffsetMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierIQGainImbalanceMean(instrumentHandle, "",
      &componentCarrierIQGainImbalanceMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierQuadratureErrorMean(instrumentHandle, "",
      &componentCarrierQuadratureErrorMean));

   RFmxCheckWarn(RFmxNR_ModAccGetResultsPDSCHQPSKRMSEVMMean(instrumentHandle, "", &PDSCHQPSKRMSEVMMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsPDSCH16QAMRMSEVMMean(instrumentHandle, "", &PDSCH16QAMRMSEVMMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsPDSCH64QAMRMSEVMMean(instrumentHandle, "", &PDSCH64QAMRMSEVMMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsPDSCH256QAMRMSEVMMean(instrumentHandle, "", &PDSCH256QAMRMSEVMMean));

   RFmxCheckWarn(RFmxNR_ModAccFetchPDSCHQPSKConstellationTrace(instrumentHandle, "", timeout, NULL,
      0, &actualArraySize));
   if (actualArraySize > 0)
   {
      QPSKConstellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * actualArraySize);
      if (QPSKConstellation)
      {
         RFmxCheckWarn(RFmxNR_ModAccFetchPDSCHQPSKConstellationTrace(instrumentHandle, "", timeout,
            QPSKConstellation, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_ModAccFetchPDSCH16QAMConstellationTrace(instrumentHandle, "", timeout, NULL,
      0, &actualArraySize));
   if (actualArraySize > 0)
   {
      QAM16Constellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * actualArraySize);
      if (QAM16Constellation)
      {
         RFmxCheckWarn(RFmxNR_ModAccFetchPDSCH16QAMConstellationTrace(instrumentHandle, "", timeout,
            QAM16Constellation, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_ModAccFetchPDSCH64QAMConstellationTrace(instrumentHandle, "", timeout, NULL,
      0, &actualArraySize));
   if (actualArraySize > 0)
   {
      QAM64Constellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * actualArraySize);
      if (QAM64Constellation)
      {
         RFmxCheckWarn(RFmxNR_ModAccFetchPDSCH64QAMConstellationTrace(instrumentHandle, "", timeout,
            QAM64Constellation, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_ModAccFetchPDSCH256QAMConstellationTrace(instrumentHandle, "", timeout, NULL,
      0, &actualArraySize));
   if (actualArraySize > 0)
   {
      QAM256Constellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * actualArraySize);
      if (QAM256Constellation)
      {
         RFmxCheckWarn(RFmxNR_ModAccFetchPDSCH256QAMConstellationTrace(instrumentHandle, "", timeout,
            QAM256Constellation, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSubcarrierMeanTrace(instrumentHandle, "", timeout,
      NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      RMSEVMPerSubcarrierMean = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (RMSEVMPerSubcarrierMean)
      {
         RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSubcarrierMeanTrace(instrumentHandle, "", timeout,
            &x0, &dx, RMSEVMPerSubcarrierMean, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSymbolMeanTrace(instrumentHandle, "", timeout,
      NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      RMSEVMPerSymbolMean = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (RMSEVMPerSymbolMean)
      {
         RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSymbolMeanTrace(instrumentHandle, "", timeout,
            &x0, &dx, RMSEVMPerSymbolMean, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("------------------Measurement------------------\n\n");
   printf("Composite RMS EVM Mean (%%)                    : %lf\n", compositeRMSEVMMean);
   printf("Composite Peak EVM Maximum (%%)                : %lf\n", compositePeakEVMMaximum);
   printf("Composite Peak EVM Slot Index                 : %d\n", compositePeakEVMSlotIndex);
   printf("Composite Peak EVM Symbol Index               : %d\n", compositePeakEVMSymbolIndex);
   printf("Composite Peak EVM Subcarrier Index           : %d\n", compositePeakEVMSubcarrierIndex);
   printf("PDSCH QPSK RMS EVM Mean (%%)                   : %lf\n", PDSCHQPSKRMSEVMMean);
   printf("PDSCH 16QAM RMS EVM Mean (%%)                  : %lf\n", PDSCH16QAMRMSEVMMean);
   printf("PDSCH 64QAM RMS EVM Mean (%%)                  : %lf\n", PDSCH64QAMRMSEVMMean);
   printf("PDSCH 256QAM RMS EVM Mean (%%)                 : %lf\n", PDSCH256QAMRMSEVMMean);
   printf("Component Carrier Frequency Error Mean (Hz)   : %lf\n", componentCarrierFrequencyErrorMean);
   printf("Component Carrier IQ Origin Offset Mean (dBc) : %lf\n", componentCarrierIQOriginOffsetMean);
   printf("Component Carrier IQ Gain Imbalance Mean (dB) : %lf\n", componentCarrierIQGainImbalanceMean);
   printf("Component Carrier Quadrature Error Mean (deg) : %lf\n\n", componentCarrierQuadratureErrorMean);

Error:
   if (error)
   {
      RFmxNR_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (instrumentHandle)
   {
      RFmxNR_Close(instrumentHandle, RFMXNR_VAL_FALSE);
   }

   /* Free allocated memory */
   if (QPSKConstellation)
   {
      free(QPSKConstellation);
   }
   if (QAM16Constellation)
   {
      free(QAM16Constellation);
   }
   if (QAM64Constellation)
   {
      free(QAM64Constellation);
   }
   if (QAM256Constellation)
   {
      free(QAM256Constellation);
   }
   if (RMSEVMPerSubcarrierMean)
   {
      free(RMSEVMPerSubcarrierMean);
   }
   if (RMSEVMPerSymbolMean)
   {
      free(RMSEVMPerSymbolMean);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
