//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal pproperties(Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Link Direction as Downlink and Number of Subblocks.
//7. Configure Sublocks and Carriers .
//8. Configure BWP Subcarrier Spacing, DL Test Model Duplex Scheme and DL Test Model for all carriers in each subblock.
//9. Select ModAcc measurement and enable Traces.
//10. Configure Synchronization Mode for ModAcc measurement.
//11. Configure Measurement Interval.
//12. Initiate the Measurement.
//13. Fetch ModAcc Measurements and Traces.
//14. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxNR.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                  256

#define NUMBER_OF_SUBBLOCKS                  2
#define NUMBER_OF_COMPONENT_CARRIERS         2

int main(int argc, char *argv[])
{
   char* resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0, j = 0;

   char subblockString[MAX_SELECTOR_STRING];
   char carrierString[MAX_SELECTOR_STRING];

   char* selectedPorts = "";
   float64 centerFrequency = 3.5e9;                                                          /* (Hz) */
   float64 referenceLevel = 0.0;                                                             /* (dBm) */
   float64 externalAttenuation = 0.0;                                                        /* (dB) */

   char* frequencyReferenceSource = RFMXNR_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e6;                                             /* (Hz) */

   int32 enableTrigger = RFMXNR_VAL_FALSE;
   char* digitalEdgeSource = RFMXNR_VAL_PXI_TRIG0_STR;
   int32 digitalEdge = RFMXNR_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                                               /* (s) */

   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;

   float64 subblockFrequency[NUMBER_OF_SUBBLOCKS] = { 0.0, 200e6 };                          /* (Hz) */
   int32 componentCarrierSpacingType[NUMBER_OF_SUBBLOCKS]
      = { RFMXNR_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL, RFMXNR_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL };
   float64 channelRaster[NUMBER_OF_SUBBLOCKS] = { 15e3, 15e3 };                              /* (Hz) */
   int32 componentCarrierAtCenterFrequency[NUMBER_OF_SUBBLOCKS] = { -1, -1 };

   float64 componentCarrierBandwidth[NUMBER_OF_SUBBLOCKS][NUMBER_OF_COMPONENT_CARRIERS]
      = { { 100e6, 100e6 }, { 100e6, 100e6 } };                                              /* (Hz) */
   float64 componentCarrierFrequency[NUMBER_OF_SUBBLOCKS][NUMBER_OF_COMPONENT_CARRIERS]
      = { { -49.98e6, 50.01e6 }, { -49.98e6, 50.01e6 } };                                    /* (Hz) */

   float64 subcarrierSpacing = 30e3;                                                         /* (Hz) */

   int32 downlinkTestModel = RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM1_1;

   int32 downlinkTestModelDuplexScheme = RFMXNR_VAL_DOWNLINK_TEST_MODEL_DUPLEX_SCHEME_FDD;

   int32 synchronizationMode = RFMXNR_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;

   int32 measurementLengthUnit = RFMXNR_VAL_MODACC_MEASUREMENT_LENGTH_UNIT_SLOT;
   float64 measurementOffset = 0.0;
   float64 measurementLength = 1;

   float64 timeout = 10.000000;                                                                                /* (s) */

   float64 compositeRMSEVMMean[NUMBER_OF_SUBBLOCKS][NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };                   /* (%) */
   float64 compositePeakEVMMaximum[NUMBER_OF_SUBBLOCKS][NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };               /* (%) */
   int32 compositePeakEVMSlotIndex[NUMBER_OF_SUBBLOCKS][NUMBER_OF_COMPONENT_CARRIERS] = { 0 };
   int32 compositePeakEVMSymbolIndex[NUMBER_OF_SUBBLOCKS][NUMBER_OF_COMPONENT_CARRIERS] = { 0 };
   int32 compositePeakEVMSubcarrierIndex[NUMBER_OF_SUBBLOCKS][NUMBER_OF_COMPONENT_CARRIERS] = { 0 };

   float64 PDSCHRMSEVMMean[NUMBER_OF_SUBBLOCKS][NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };                       /* (%) */

   float64 componentCarrierFrequencyErrorMean[NUMBER_OF_SUBBLOCKS][NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };    /* (Hz) */
   float64 componentCarrierIQOriginOffsetMean[NUMBER_OF_SUBBLOCKS][NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };    /* (dBc) */
   float64 componentCarrierIQGainImbalanceMean[NUMBER_OF_SUBBLOCKS][NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };   /* (dB) */
   float64 componentCarrierQuadratureErrorMean[NUMBER_OF_SUBBLOCKS][NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };   /* (deg) */

   int32 actualArraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32* RMSEVMPerSubcarrierMean[NUMBER_OF_SUBBLOCKS][NUMBER_OF_COMPONENT_CARRIERS] = { NULL };
   float32* RMSEVMPerSymbolMean[NUMBER_OF_SUBBLOCKS][NUMBER_OF_COMPONENT_CARRIERS] = { NULL };

   /* Initialize a session */
   RFmxCheckWarn(RFmxNR_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxNR_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxNR_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxNR_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxNR_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay,
      enableTrigger));
   RFmxCheckWarn(RFmxNR_SetLinkDirection(instrumentHandle, "", RFMXNR_VAL_LINK_DIRECTION_DOWNLINK));
   RFmxCheckWarn(RFmxNR_SetNumberOfSubblocks(instrumentHandle, "", NUMBER_OF_SUBBLOCKS));

   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      RFmxNR_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString);
      RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, subblockString, frequencyRange));
      RFmxCheckWarn(RFmxNR_SetSubblockFrequency(instrumentHandle, subblockString, subblockFrequency[i]));
      RFmxCheckWarn(RFmxNR_SetChannelRaster(instrumentHandle, subblockString, channelRaster[i]));
      RFmxCheckWarn(RFmxNR_SetComponentCarrierSpacingType(instrumentHandle, subblockString, componentCarrierSpacingType[i]));
      RFmxCheckWarn(RFmxNR_SetComponentCarrierAtCenterFrequency(instrumentHandle, subblockString, componentCarrierAtCenterFrequency[i]));
      RFmxCheckWarn(RFmxNR_SetNumberOfComponentCarriers(instrumentHandle, subblockString, NUMBER_OF_COMPONENT_CARRIERS));

      for (j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++)
      {
         RFmxNR_BuildCarrierString(subblockString, j, MAX_SELECTOR_STRING, carrierString);
         RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, carrierString, componentCarrierBandwidth[i][j]));
         RFmxCheckWarn(RFmxNR_SetComponentCarrierFrequency(instrumentHandle, carrierString, componentCarrierFrequency[i][j]));
      }

      RFmxNR_BuildCarrierString(subblockString, -1, MAX_SELECTOR_STRING, carrierString);
      RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, carrierString, subcarrierSpacing));
      RFmxCheckWarn(RFmxNR_SetDownlinkTestModel(instrumentHandle, carrierString, downlinkTestModel));
      RFmxCheckWarn(RFmxNR_SetDownlinkTestModelDuplexScheme(instrumentHandle, carrierString, downlinkTestModelDuplexScheme));
   }

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_MODACC, RFMXNR_VAL_TRUE));

   RFmxCheckWarn(RFmxNR_ModAccSetSynchronizationMode(instrumentHandle, "", synchronizationMode));
   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementLengthUnit(instrumentHandle, "", measurementLengthUnit));
   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementOffset(instrumentHandle, "", measurementOffset));
   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementLength(instrumentHandle, "", measurementLength));
   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

   /* Fetch results */
   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      RFmxNR_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString);

      for (j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++)
      {
         RFmxNR_BuildCarrierString(subblockString, j, MAX_SELECTOR_STRING, carrierString);
         RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositeRMSEVMMean(instrumentHandle, carrierString,
            &compositeRMSEVMMean[i][j]));
         RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMMaximum(instrumentHandle, carrierString,
            &compositePeakEVMMaximum[i][j]));
         RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMSlotIndex(instrumentHandle, carrierString,
            &compositePeakEVMSlotIndex[i][j]));
         RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMSymbolIndex(instrumentHandle, carrierString,
            &compositePeakEVMSymbolIndex[i][j]));
         RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMSubcarrierIndex(instrumentHandle, carrierString,
            &compositePeakEVMSubcarrierIndex[i][j]));
         RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierFrequencyErrorMean(instrumentHandle, carrierString,
            &componentCarrierFrequencyErrorMean[i][j]));
         RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierIQOriginOffsetMean(instrumentHandle, carrierString,
            &componentCarrierIQOriginOffsetMean[i][j]));
         RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierIQGainImbalanceMean(instrumentHandle, carrierString,
            &componentCarrierIQGainImbalanceMean[i][j]));
         RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierQuadratureErrorMean(instrumentHandle, carrierString,
            &componentCarrierQuadratureErrorMean[i][j]));

         switch (downlinkTestModel)
         {
         case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM1_1:
         case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM1_2:
         case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM3_3:
            RFmxCheckWarn(RFmxNR_ModAccGetResultsPDSCHQPSKRMSEVMMean(instrumentHandle, carrierString,
               &PDSCHRMSEVMMean[i][j]));
            break;

         case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM2:
         case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM3_1:
            RFmxCheckWarn(RFmxNR_ModAccGetResultsPDSCH64QAMRMSEVMMean(instrumentHandle, carrierString,
               &PDSCHRMSEVMMean[i][j]));
            break;

         case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM2A:
         case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM3_1A:
            RFmxCheckWarn(RFmxNR_ModAccGetResultsPDSCH256QAMRMSEVMMean(instrumentHandle, carrierString,
               &PDSCHRMSEVMMean[i][j]));
            break;

         case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM3_2:
            RFmxCheckWarn(RFmxNR_ModAccGetResultsPDSCH16QAMRMSEVMMean(instrumentHandle, carrierString,
               &PDSCHRMSEVMMean[i][j]));
            break;
         }

         actualArraySize = 0;
         RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSubcarrierMeanTrace(instrumentHandle, carrierString, timeout,
            NULL, NULL, NULL, 0, &actualArraySize));
         if (actualArraySize > 0)
         {
            RMSEVMPerSubcarrierMean[i][j] = (float32*)malloc(sizeof(float32) * actualArraySize);
            if (RMSEVMPerSubcarrierMean[i][j])
            {
               RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSubcarrierMeanTrace(instrumentHandle, carrierString, timeout,
                  &x0, &dx, RMSEVMPerSubcarrierMean[i][j], actualArraySize, NULL));
            }
            else
            {
               printf("malloc failed.\n");
               goto Error;
            }
         }

         actualArraySize = 0;
         RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSymbolMeanTrace(instrumentHandle, carrierString, timeout,
            NULL, NULL, NULL, 0, &actualArraySize));
         if (actualArraySize > 0)
         {
            RMSEVMPerSymbolMean[i][j] = (float32*)malloc(sizeof(float32) * actualArraySize);
            if (RMSEVMPerSymbolMean[i][j])
            {
               RFmxCheckWarn(RFmxNR_ModAccFetchRMSEVMPerSymbolMeanTrace(instrumentHandle, carrierString, timeout,
                  &x0, &dx, RMSEVMPerSymbolMean[i][j], actualArraySize, NULL));
            }
            else
            {
               printf("malloc failed.\n");
               goto Error;
            }
         }
      }
   }

   printf("------------------------Measurement------------------------\n\n");
   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      printf("Subblock  : %d\n\n", i);
      for (j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++)
      {
         printf("Carrier  : %d\n", j);
         printf("Composite RMS EVM Mean (%%)                    : %lf\n", compositeRMSEVMMean[i][j]);
         printf("Composite Peak EVM Maximum (%%)                : %lf\n", compositePeakEVMMaximum[i][j]);
         printf("Composite Peak EVM Slot Index                 : %d\n", compositePeakEVMSlotIndex[i][j]);
         printf("Composite Peak EVM Symbol Index               : %d\n", compositePeakEVMSymbolIndex[i][j]);
         printf("Composite Peak EVM Subcarrier Index           : %d\n", compositePeakEVMSubcarrierIndex[i][j]);
         printf("PDSCH RMS EVM Mean (%%)                        : %lf\n", PDSCHRMSEVMMean[i][j]);
         printf("Component Carrier Frequency Error Mean (Hz)   : %lf\n", componentCarrierFrequencyErrorMean[i][j]);
         printf("Component Carrier IQ Origin Offset Mean (dBc) : %lf\n", componentCarrierIQOriginOffsetMean[i][j]);
         printf("Component Carrier IQ Gain Imbalance Mean (dB) : %lf\n", componentCarrierIQGainImbalanceMean[i][j]);
         printf("Component Carrier Quadrature Error Mean (deg) : %lf\n", componentCarrierQuadratureErrorMean[i][j]);
         printf("-------------------------------------------------\n\n");
      }
   }


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
   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      for (j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++)
      {
          if (RMSEVMPerSubcarrierMean[i][j])
          {
              free(RMSEVMPerSubcarrierMean[i][j]);
          }
          if (RMSEVMPerSymbolMean[i][j])
          {
              free(RMSEVMPerSymbolMean[i][j]);
          }
      }
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
