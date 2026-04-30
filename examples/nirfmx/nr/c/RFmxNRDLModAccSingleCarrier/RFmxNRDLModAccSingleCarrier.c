//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Link Direction as Downlink, Frequency Range, Carrier Bandwidth and BWP Subcarrier Spacing.
//7. Configure DL Test Model and DL Test Model Duplex Scheme.
//8. Select ModAcc measurement and enable Traces.
//9. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
//10. Configure Measurement Interval.
//11. Initiate the Measurement.
//12. Fetch ModAcc Measurements and Traces.
//13. Close RFmx Session.

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
   float64 carrierBandwidth = 100e6;                                                      /* (Hz) */
   float64 subcarrierSpacing = 30e3;                                                      /* (Hz) */

   int32 downlinkTestModelDuplexScheme = RFMXNR_VAL_DOWNLINK_TEST_MODEL_DUPLEX_SCHEME_FDD;
   int32 downlinkTestModel = RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM1_1;

   int32 synchronizationMode = RFMXNR_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;

   int32 averagingEnabled = RFMXNR_VAL_MODACC_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;

   int32 measurementLengthUnit = RFMXNR_VAL_MODACC_MEASUREMENT_LENGTH_UNIT_SLOT;
   float64 measurementOffset = 0.0;
   float64 measurementLength = 1;

   float64 timeout = 10.000000;                                                           /* (s) */

   float64 compositeRMSEVMMean = 0.0;                                                     /* (%) */
   float64 compositePeakEVMMaximum = 0.0;                                                 /* (%) */
   int32 compositePeakEVMSlotIndex = 0;
   int32 compositePeakEVMSymbolIndex = 0;
   int32 compositePeakEVMSubcarrierIndex = 0;

   float64 PDSCHRMSEVMMean = 0.0;                                                         /* (%) */

   float64 componentCarrierFrequencyErrorMean = 0.0;                                      /* (Hz) */
   float64 componentCarrierIQOriginOffsetMean = 0.0;                                      /* (dBc) */
   float64 componentCarrierIQGainImbalanceMean = 0.0;                                     /* (dB) */
   float64 componentCarrierQuadratureErrorMean = 0.0;                                     /* (deg) */

   int32 actualArraySize = 0;
   NIComplexSingle* PDSCHConstellation = NULL;

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
   RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, "", subcarrierSpacing));

   RFmxCheckWarn(RFmxNR_SetDownlinkTestModel(instrumentHandle, "", downlinkTestModel));
   RFmxCheckWarn(RFmxNR_SetDownlinkTestModelDuplexScheme(instrumentHandle, "", downlinkTestModelDuplexScheme));

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

   switch (downlinkTestModel)
   {
      case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM1_1:
      case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM1_2:
      case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM3_3:
         RFmxCheckWarn(RFmxNR_ModAccGetResultsPDSCHQPSKRMSEVMMean(instrumentHandle, "", &PDSCHRMSEVMMean));

         actualArraySize = 0;
         RFmxCheckWarn(RFmxNR_ModAccFetchPDSCHQPSKConstellationTrace(instrumentHandle, "", timeout, NULL,
            0, &actualArraySize));
         if (actualArraySize > 0)
         {
            PDSCHConstellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * actualArraySize);
            if (PDSCHConstellation)
            {
               RFmxCheckWarn(RFmxNR_ModAccFetchPDSCHQPSKConstellationTrace(instrumentHandle, "", timeout,
                  PDSCHConstellation, actualArraySize, NULL));
            }
            else
            {
               printf("malloc failed.\n");
               goto Error;
            }
         }
         break;

      case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM2:
      case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM3_1:
         RFmxCheckWarn(RFmxNR_ModAccGetResultsPDSCH64QAMRMSEVMMean(instrumentHandle, "", &PDSCHRMSEVMMean));

         actualArraySize = 0;
         RFmxCheckWarn(RFmxNR_ModAccFetchPDSCH64QAMConstellationTrace(instrumentHandle, "", timeout, NULL,
            0, &actualArraySize));
         if (actualArraySize > 0)
         {
            PDSCHConstellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * actualArraySize);
            if (PDSCHConstellation)
            {
               RFmxCheckWarn(RFmxNR_ModAccFetchPDSCH64QAMConstellationTrace(instrumentHandle, "", timeout,
                  PDSCHConstellation, actualArraySize, NULL));
            }
            else
            {
               printf("malloc failed.\n");
               goto Error;
            }
         }
         break;

      case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM2A:
      case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM3_1A:
         RFmxCheckWarn(RFmxNR_ModAccGetResultsPDSCH256QAMRMSEVMMean(instrumentHandle, "", &PDSCHRMSEVMMean));

         actualArraySize = 0;
         RFmxCheckWarn(RFmxNR_ModAccFetchPDSCH256QAMConstellationTrace(instrumentHandle, "", timeout, NULL,
            0, &actualArraySize));
         if (actualArraySize > 0)
         {
            PDSCHConstellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * actualArraySize);
            if (PDSCHConstellation)
            {
               RFmxCheckWarn(RFmxNR_ModAccFetchPDSCH256QAMConstellationTrace(instrumentHandle, "", timeout,
                  PDSCHConstellation, actualArraySize, NULL));
            }
            else
            {
               printf("malloc failed.\n");
               goto Error;
            }
         }
         break;

      case RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM3_2:
         RFmxCheckWarn(RFmxNR_ModAccGetResultsPDSCH16QAMRMSEVMMean(instrumentHandle, "", &PDSCHRMSEVMMean));

         actualArraySize = 0;
         RFmxCheckWarn(RFmxNR_ModAccFetchPDSCH16QAMConstellationTrace(instrumentHandle, "", timeout, NULL,
            0, &actualArraySize));
         if (actualArraySize > 0)
         {
            PDSCHConstellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * actualArraySize);
            if (PDSCHConstellation)
            {
               RFmxCheckWarn(RFmxNR_ModAccFetchPDSCH16QAMConstellationTrace(instrumentHandle, "", timeout,
                  PDSCHConstellation, actualArraySize, NULL));
            }
            else
            {
               printf("malloc failed.\n");
               goto Error;
            }
         }
         break;
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
   printf("PDSCH RMS EVM Mean (%%)                       : %lf\n", PDSCHRMSEVMMean);
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
   if (PDSCHConstellation)
   {
      free(PDSCHConstellation);
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
