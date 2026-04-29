//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Number of DUT Antennas.
//6. Configure Transmit Antenna to Analyze.
//7. Configure Duplex Mode.
//8[A-G]. Configure Subblock Parameters.
//8A. Configure Number of Subblocks.
//8B. Configure subblock Frequency.
//8C. Configure Component Carrier Spacing.
//8D. Configure Band.
//8E. Configure Number of Component Carriers.
//8F. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
//8G. Configure DMRS OCC Enabled and Cyclic Shift Field.
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
#define MAX_ERROR_DESCRIPTION                   4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                     256

#define NUMBER_OF_SUBBLOCKS                     2
#define NUMBER_OF_COMPONENT_CARRIERS            1

/* Input: Subblock inputs structure */
typedef struct
{
   float64 subblockFrequency;                                                       /*(Hz) */
   int32 componentCarrierSpacingType;
   int32 componentCarrierAtCenterFrequency;
   int32 band;
   /*Component Carrier Settings*/
   float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS];                  /*(Hz) */
   float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS];                  /*(Hz) */
   int32 componentCarrierCellId[NUMBER_OF_COMPONENT_CARRIERS];
}subblockInputs_t;

/* Input: Subblock measurement outputs structure */
typedef struct
{
   float64 meanRMSCompositeEVM[NUMBER_OF_COMPONENT_CARRIERS];                       /*(% or dB) */
   float64 maximumPeakCompositeEVM[NUMBER_OF_COMPONENT_CARRIERS];                   /*(% or dB) */
   int32 peakCompositeEVMSlotIndex[NUMBER_OF_COMPONENT_CARRIERS];                   /*(dB) */
   int32 peakCompositeEVMSymbolIndex[NUMBER_OF_COMPONENT_CARRIERS];
   int32 peakCompositeEVMSubcarrierIndex[NUMBER_OF_COMPONENT_CARRIERS];
   float64 meanFrequencyError[NUMBER_OF_COMPONENT_CARRIERS];                        /*(Hz) */
   float64 meanIQOriginOffset[NUMBER_OF_COMPONENT_CARRIERS];                        /*(dBc) */
   float64 meanIQGainImbalance[NUMBER_OF_COMPONENT_CARRIERS];                       /*(dB) */
   float64 meanIQQuadratureError[NUMBER_OF_COMPONENT_CARRIERS];                     /*(deg) */
   float64 inBandEmissionMargin[NUMBER_OF_COMPONENT_CARRIERS];                      /*(dB) */
   float64 x0[NUMBER_OF_COMPONENT_CARRIERS];
   float64 dx[NUMBER_OF_COMPONENT_CARRIERS];
   float32* meanRMSEVMPerSubcarrier[NUMBER_OF_COMPONENT_CARRIERS];                   /*(% or dB) */
}subblockMeasurements_t;

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char subblockString[NUMBER_OF_SUBBLOCKS][MAX_SELECTOR_STRING];
   char subblockCarrierString[MAX_SELECTOR_STRING];

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0, j = 0;

   char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                                       /*(Hz) */

   float64 centerFrequency = 1.95e9;                                                 /*(Hz) */
   float64 referenceLevel = 0.0;                                                     /*(dBm) */
   float64 externalAttenuation = 0.0;                                                /*(dB) */

   int32 enableTrigger = RFMXLTE_VAL_FALSE;
   char * digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
   int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                                       /*(s) */

   int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_FDD;
   int32 uplinkDownlinkConfiguraiton = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;
   int32 numberOfDUTAntennas = 2;
   int32 transmitAntennaToAnalyze = 0;

   subblockInputs_t subblocks[NUMBER_OF_SUBBLOCKS] = {                              /*  Set up subblock 0 inputs  */
	                                         {	0.0,				                /* subblockFrequency */	
                                                RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL,
                                                -1,                                 /* componentCarrierAtCenterFrequency */
                                                1,                                  /* band */
                                                {20e6},                             /* componentCarrierBandwidth */
                                                {0.0},                              /* componentCarrierFrequency */
                                                {0},                                /* componentCarrierCellId */
                                             },
                                             {                                      /*  Set up subblock 1 inputs  */
		                                        30e6,					            /* subblockFrequency */	
                                                RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL,
                                                -1,                                 /* componentCarrierAtCenterFrequency */
                                                1,                                  /* band */
                                                {20e6},                             /* componentCarrierBandwidth */
                                                {0.0},                              /* componentCarrierFrequency */
                                                {0},                                /* componentCarrierCellId */
                                             }
   };

   /* EVM Unit */
   int32 EVMUnit = RFMXLTE_VAL_MODACC_EVM_UNIT_PERCENTAGE;

   /* InBandEmissionMaskType */
   int32 inBandEmissionMaskType = RFMXLTE_VAL_MODACC_IN_BAND_EMISSION_MASK_TYPE_RELEASE_11_ONWARDS;

   /* Measurement Interval */
   int32 synchronizationMode = RFMXLTE_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;
   int32 measurementOffset = 0;                                                      /*(slots) */
   int32 measurementLength = 1;                                                      /*(slots) */

  /* Averaging */
   int32 averagingEnabled = RFMXLTE_VAL_MODACC_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   int32 autoDMRSDetectionEnabled = RFMXLTE_VAL_AUTO_DMRS_DETECTION_ENABLED_TRUE;

   float64 timeout = 10.0;                                                           /*(s) */
   subblockMeasurements_t subblocksMsr[NUMBER_OF_SUBBLOCKS];
   NIComplexSingle* dataConstellation[NUMBER_OF_SUBBLOCKS] = { NULL };
   NIComplexSingle* DMRSConstellation[NUMBER_OF_SUBBLOCKS] = { NULL };
   int32 actualArraySize = 0;
   int32 dataConstellationActualArraySize = 0;
   int32 DMRSConstellationActualArraySize = 0;

   /* Set up subblock outputs */
   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      for (j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++)
      {
         subblocksMsr[i].meanRMSCompositeEVM[j] = 0;
         subblocksMsr[i].maximumPeakCompositeEVM[j] = 0;
         subblocksMsr[i].meanFrequencyError[j] = 0;
         subblocksMsr[i].peakCompositeEVMSlotIndex[j] = 0;
         subblocksMsr[i].peakCompositeEVMSymbolIndex[j] = 0;
         subblocksMsr[i].peakCompositeEVMSubcarrierIndex[j] = 0;
         subblocksMsr[i].meanIQOriginOffset[j] = 0;
         subblocksMsr[i].meanIQGainImbalance[j] = 0;
         subblocksMsr[i].meanIQQuadratureError[j] = 0;
         subblocksMsr[i].inBandEmissionMargin[j] = 0;
         subblocksMsr[i].meanRMSEVMPerSubcarrier[j] = NULL;
      }
   }

   /* Initialize a session */
   RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge,
      triggerDelay, enableTrigger));
   RFmxCheckWarn(RFmxLTE_CfgNumberOfDUTAntennas(instrumentHandle, "", numberOfDUTAntennas));
   RFmxCheckWarn(RFmxLTE_CfgTransmitAntennaToAnalyze(instrumentHandle, "", transmitAntennaToAnalyze));
   RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));
   RFmxCheckWarn(RFmxLTE_CfgNumberOfSubblocks(instrumentHandle, "", NUMBER_OF_SUBBLOCKS));
   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      RFmxCheckWarn(RFmxLTE_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString[i]));
      RFmxCheckWarn(RFmxLTE_SetSubblockFrequency(instrumentHandle, subblockString[i], subblocks[i].subblockFrequency));
      RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, subblockString[i],
         subblocks[i].componentCarrierSpacingType,
         subblocks[i].componentCarrierAtCenterFrequency));
      RFmxCheckWarn(RFmxLTE_CfgBand(instrumentHandle, subblockString[i], subblocks[i].band));
      RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, subblockString[i],
         NUMBER_OF_COMPONENT_CARRIERS));
      RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, subblockString[i],
         subblocks[i].componentCarrierBandwidth,
         subblocks[i].componentCarrierFrequency,
         subblocks[i].componentCarrierCellId, NUMBER_OF_COMPONENT_CARRIERS));
   }
   RFmxCheckWarn(RFmxLTE_CfgAutoDMRSDetectionEnabled(instrumentHandle, "", autoDMRSDetectionEnabled));

   RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_MODACC, RFMXLTE_VAL_TRUE));
   RFmxCheckWarn(RFmxLTE_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode,
      measurementOffset, measurementLength));
   RFmxCheckWarn(RFmxLTE_ModAccCfgEVMUnit(instrumentHandle, "", EVMUnit));
   RFmxCheckWarn(RFmxLTE_ModAccCfgInBandEmissionMaskType(instrumentHandle, "", inBandEmissionMaskType));
   RFmxCheckWarn(RFmxLTE_ModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
   RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      RFmxCheckWarn(RFmxLTE_ModAccFetchCompositeEVMArray(instrumentHandle, subblockString[i], timeout,
         subblocksMsr[i].meanRMSCompositeEVM, subblocksMsr[i].maximumPeakCompositeEVM,
         subblocksMsr[i].meanFrequencyError, subblocksMsr[i].peakCompositeEVMSymbolIndex,
         subblocksMsr[i].peakCompositeEVMSubcarrierIndex, subblocksMsr[i].peakCompositeEVMSlotIndex,
         NUMBER_OF_COMPONENT_CARRIERS, NULL));

      RFmxCheckWarn(RFmxLTE_ModAccFetchIQImpairmentsArray(instrumentHandle, subblockString[i], timeout,
         subblocksMsr[i].meanIQOriginOffset, subblocksMsr[i].meanIQGainImbalance,
         subblocksMsr[i].meanIQQuadratureError, NUMBER_OF_COMPONENT_CARRIERS, NULL));

      RFmxCheckWarn(RFmxLTE_ModAccFetchInBandEmissionMarginArray(instrumentHandle, subblockString[i], timeout,
         subblocksMsr[i].inBandEmissionMargin, NUMBER_OF_COMPONENT_CARRIERS, NULL));

      for (j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++)
      {
         actualArraySize = 0;
         RFmxCheckWarn(RFmxLTE_BuildCarrierString(subblockString[i], j, MAX_SELECTOR_STRING, subblockCarrierString));
         RFmxCheckWarn(RFmxLTE_ModAccFetchEVMPerSubcarrierTrace(instrumentHandle, subblockCarrierString, timeout,
            NULL, NULL, NULL, 0, &actualArraySize));
         if (actualArraySize > 0)
         {
            subblocksMsr[i].meanRMSEVMPerSubcarrier[j] = (float32 *)malloc(sizeof(float32) * actualArraySize);
            if (subblocksMsr[i].meanRMSEVMPerSubcarrier[j])
            {
               RFmxCheckWarn(RFmxLTE_ModAccFetchEVMPerSubcarrierTrace(instrumentHandle, subblockCarrierString, timeout,
                  &subblocksMsr[i].x0[j], &subblocksMsr[i].dx[j],
                  subblocksMsr[i].meanRMSEVMPerSubcarrier[j], actualArraySize, NULL));
            }
            else
            {
               printf("malloc failed.\n");
               goto Error;
            }
         }
      }
   }

   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      actualArraySize = 0;
      RFmxCheckWarn(RFmxLTE_BuildCarrierString(subblockString[i], 0, MAX_SELECTOR_STRING, subblockCarrierString));
      RFmxCheckWarn(RFmxLTE_ModAccFetchPUSCHConstellationTrace(instrumentHandle, subblockCarrierString, timeout,
         NULL, 0, &dataConstellationActualArraySize,
         NULL, 0, &DMRSConstellationActualArraySize));
      if (dataConstellationActualArraySize > 0 && DMRSConstellationActualArraySize > 0)
      {
         dataConstellation[i] = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * dataConstellationActualArraySize);
         DMRSConstellation[i] = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * DMRSConstellationActualArraySize);
         if (dataConstellation[i] && DMRSConstellation[i])
         {
            RFmxCheckWarn(RFmxLTE_ModAccFetchPUSCHConstellationTrace(instrumentHandle, subblockCarrierString, timeout,
               dataConstellation[i], dataConstellationActualArraySize, NULL,
               DMRSConstellation[i], DMRSConstellationActualArraySize, NULL));
         }
         else
         {
            printf("malloc failed.\n");
            goto Error;
         }
      }
   }

   printf("----------------------Measurements--------------------\n");
   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      printf("\nSubblock Number %d\n", i);

      printf("-----------Component Carrier Measurements----------------\n");
      for (j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++)
      {
         printf("Carrier %d\n", j);
         printf("Mean RMS Composite EVM  (%% or dB)       : %lf\n", subblocksMsr[i].meanRMSCompositeEVM[j]);
         printf("Max Peak Composite EVM  (%% or dB)       : %lf\n", subblocksMsr[i].maximumPeakCompositeEVM[j]);
         printf("Peak Composite EVM Slot Index           : %d\n", subblocksMsr[i].peakCompositeEVMSlotIndex[j]);
         printf("Peak Composite EVM Symbol Index         : %d\n", subblocksMsr[i].peakCompositeEVMSymbolIndex[j]);
         printf("Peak Composite EVM Subcarrier Index     : %d\n", subblocksMsr[i].peakCompositeEVMSubcarrierIndex[j]);
         printf("Mean Frequency Error  (Hz)              : %lf\n", subblocksMsr[i].meanFrequencyError[j]);
         printf("Mean IQ Origin Offset  (dBc)            : %lf\n", subblocksMsr[i].meanIQOriginOffset[j]);
         printf("Mean IQ Gain Imbalance  (dB)            : %lf\n", subblocksMsr[i].meanIQGainImbalance[j]);
         printf("Mean IQ Quadrature Error  (deg)         : %lf\n", subblocksMsr[i].meanIQQuadratureError[j]);
         printf("In Band Emission Margin  (dB)           : %lf\n", subblocksMsr[i].inBandEmissionMargin[j]);
         printf("-------------------------------------------------\n");
      }
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
   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      for (j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++)
      {
          if (subblocksMsr[i].meanRMSEVMPerSubcarrier[j])
          {
            free(subblocksMsr[i].meanRMSEVMPerSubcarrier[j]);
          }
      }
      if (dataConstellation[i]) {
         free(dataConstellation[i]);
      }
      if (DMRSConstellation[i]) {
         free(DMRSConstellation[i]);
      }
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
