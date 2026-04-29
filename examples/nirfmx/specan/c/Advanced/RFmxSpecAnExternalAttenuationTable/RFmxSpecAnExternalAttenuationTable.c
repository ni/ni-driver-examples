//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties (Clock Source, Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Configure External Attenuation Table
//6. Select SEM measurement and enable the traces
//7. Configure SEM Power Units and Reference Type
//8. Configure Averaging parameters
//9. Configure Integration BW of the carrier
//10. Configure RBW Filter parameters
//11. Configure RRC Filter to be applied on the acquired carrier channel
//12. Configure Number of Offsets
//13. Configure Frequency Ranges of Offset channels
//Use Array API's to configure all offset parameters as an array
//14. Configure Absolute Limit mask for Offset Channels
//15. Configure Relative Limit mask for Offset Channels
//16. Configure RBW Filter parameters for the Offset channels
//17. Configure Offset Limit Fail Mask
//18. Configure Amplitude Correction Type

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

/* Maximum size of selector string */
#define MAX_SELECTOR_STRING         256

#define NUM_OF_OFFSETS              4
#define TABLE_SIZE                  3

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];
   char offsetString[MAX_SELECTOR_STRING];
   char portString[MAX_SELECTOR_STRING];
   int32 i = 0;

   char *selectedPorts = "";
   float64 centerFrequency = 1.0e+9;                  /* Hz */
   float64 referenceLevel = 0.00;                     /* dBm */
   float64 externalAttenuation = 0.00;                /* dB */

   /* Frequency Reference */
   char *frequencyReferenceSource = RFMXSPECAN_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e+6;     /* Hz */

   /* External Attenuation Table */
   int32 tableSize = TABLE_SIZE;
   float64 frequency[TABLE_SIZE] = { 997.0e+6, 1.0e+9, 1.003e+9 };   /* Hz */
   float64 attenuation[TABLE_SIZE] = { 0.20, 0.50, 0.30 };           /* dB */

   float64 integrationBandwidth = 3.840e+6;                          /* Hz */

   /* RBW Filter */
   int32 RBWAuto = RFMXSPECAN_VAL_SEM_RBW_AUTO_FALSE;
   int32 RBWFilterType = RFMXSPECAN_VAL_SEM_RBW_FILTER_TYPE_GAUSSIAN;
   float64 RBW = 30.0e+3;                                            /* Hz */

   /* RRC Filter */
   int32 RRCFilterEnabled = RFMXSPECAN_VAL_SEM_RRC_FILTER_ENABLED_TRUE;
   float64 RRCAlpha = 0.220;

   int32 amplitudeCorrectionType = RFMXSPECAN_VAL_SEM_AMPLITUDE_CORRECTION_TYPE_RF_CENTER_FREQUENCY;

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_SEM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_SEM_AVERAGING_TYPE_RMS;

   int32 referenceType = RFMXSPECAN_VAL_SEM_REFERENCE_TYPE_INTEGRATION;
   int32 powerUnits = RFMXSPECAN_VAL_SEM_POWER_UNITS_DBM;

   /* Declare input variables for Offset Segments*/

   const int32 numOfOffsets = NUM_OF_OFFSETS;

   int32 offsetEnabled[NUM_OF_OFFSETS] = { RFMXSPECAN_VAL_SEM_OFFSET_ENABLED_TRUE, RFMXSPECAN_VAL_SEM_OFFSET_ENABLED_TRUE,
                                 RFMXSPECAN_VAL_SEM_OFFSET_ENABLED_TRUE, RFMXSPECAN_VAL_SEM_OFFSET_ENABLED_TRUE };

   /* Offset Frequency */
   int32 offsetSideband[NUM_OF_OFFSETS] = { RFMXSPECAN_VAL_SEM_OFFSET_SIDEBAND_BOTH, RFMXSPECAN_VAL_SEM_OFFSET_SIDEBAND_BOTH,
                                 RFMXSPECAN_VAL_SEM_OFFSET_SIDEBAND_BOTH, RFMXSPECAN_VAL_SEM_OFFSET_SIDEBAND_BOTH };
   float64 offsetStartFrequency[NUM_OF_OFFSETS] = { 2.515e6, 4.000e6, 7.500e6, 8.500e6 };    /* Hz */
   float64 offsetStopFrequency[NUM_OF_OFFSETS] = { 3.485e6, 7.500e6, 8.500e6, 12.000e6 };    /* Hz */

   /* RBW Filter */
   int32 offsetRBWFilterType[NUM_OF_OFFSETS] = { RFMXSPECAN_VAL_SEM_RBW_FILTER_TYPE_GAUSSIAN, RFMXSPECAN_VAL_SEM_RBW_FILTER_TYPE_GAUSSIAN,
                                 RFMXSPECAN_VAL_SEM_RBW_FILTER_TYPE_GAUSSIAN, RFMXSPECAN_VAL_SEM_RBW_FILTER_TYPE_GAUSSIAN };
   int32 offsetRBWAuto[NUM_OF_OFFSETS] = { RFMXSPECAN_VAL_SEM_RBW_AUTO_FALSE, RFMXSPECAN_VAL_SEM_RBW_AUTO_FALSE,
                              RFMXSPECAN_VAL_SEM_RBW_AUTO_FALSE, RFMXSPECAN_VAL_SEM_RBW_AUTO_FALSE };
   float64 offsetRBW[NUM_OF_OFFSETS] = { 30.0e3, 500.0e3, 1.000e6, 1.000e6 };                /* Hz */

   /* Absolute Limit */
   int32 absoluteLimitMode[NUM_OF_OFFSETS] = { RFMXSPECAN_VAL_SEM_OFFSET_ABSOLUTE_LIMIT_MODE_COUPLE, RFMXSPECAN_VAL_SEM_OFFSET_ABSOLUTE_LIMIT_MODE_COUPLE,
                                    RFMXSPECAN_VAL_SEM_OFFSET_ABSOLUTE_LIMIT_MODE_COUPLE, RFMXSPECAN_VAL_SEM_OFFSET_ABSOLUTE_LIMIT_MODE_COUPLE };
   float64 absoluteLimitStart[NUM_OF_OFFSETS] = { -69.60, -54.30, -54.30, -54.30 };          /* dBm */
   float64 absoluteLimitStop[NUM_OF_OFFSETS] = { -69.60, -54.30, -54.30, -54.30 };           /* dBm */

   /* Relative Limit */
   int32 relativeLimitMode[NUM_OF_OFFSETS] = { RFMXSPECAN_VAL_SEM_OFFSET_RELATIVE_LIMIT_MODE_MANUAL, RFMXSPECAN_VAL_SEM_OFFSET_RELATIVE_LIMIT_MODE_MANUAL,
                                    RFMXSPECAN_VAL_SEM_OFFSET_RELATIVE_LIMIT_MODE_MANUAL, RFMXSPECAN_VAL_SEM_OFFSET_RELATIVE_LIMIT_MODE_COUPLE };
   float64 relativeLimitStart[NUM_OF_OFFSETS] = { -33.73, -34.00, -37.50, -47.50 };          /* dBm */
   float64 relativeLimitStop[NUM_OF_OFFSETS] = { -48.27, -37.50, -47.50, -47.50 };           /* dBm */

   int32 offsetLimitFailMask = RFMXSPECAN_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE_AND_RELATIVE;

   float64 timeout = 10.0;

   /* Set up outputs */
   int32 compositeMeasurementStatus = 0;
   float64 carrierAbsolutePower = 0.0;                               /*(dBm or dBm/Hz) */

   float64 lowerOffsetMargin[NUM_OF_OFFSETS] = { 0 };                /*(dB) */
   float64 lowerOffsetMarginAbsolutePower[NUM_OF_OFFSETS] = { 0 };   /*(dBm) */
   float64 lowerOffsetMarginRelativePower[NUM_OF_OFFSETS] = { 0 };   /*(dB) */
   float64 lowerOffsetMarginFrequency[NUM_OF_OFFSETS] = { 0 };       /*(Hz) */
   int32 lowerOffsetMeasurementStatus[NUM_OF_OFFSETS] = { 0 };

   float64 upperOffsetMargin[NUM_OF_OFFSETS] = { 0 };                /*(dB) */
   float64 upperOffsetMarginAbsolutePower[NUM_OF_OFFSETS] = { 0 };   /*(dBm) */
   float64 upperOffsetMarginRelativePower[NUM_OF_OFFSETS] = { 0 };   /*(dB) */
   float64 upperOffsetMarginFrequency[NUM_OF_OFFSETS] = { 0 };       /*(Hz) */
   int32 upperOffsetMeasurementStatus[NUM_OF_OFFSETS] = { 0 };

   int32 actualArraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = NULL;
   float32 *absoluteMask = NULL;
   float32 *relativeMask = NULL;

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxInstr_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel,
      externalAttenuation));
   RFmxCheckWarn(RFmxInstr_BuildPortString2("", selectedPorts, "", 0, MAX_SELECTOR_STRING, portString));
   RFmxCheckWarn(RFmxInstr_CfgExternalAttenuationTable(instrumentHandle, portString, "", frequency,
      attenuation, tableSize));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_SEM,
      RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgPowerUnits(instrumentHandle, "", powerUnits));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgReferenceType(instrumentHandle, "", referenceType));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgAveraging(instrumentHandle, "", averagingEnabled,
      averagingCount, averagingType));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgCarrierIntegrationBandwidth(instrumentHandle, "",
      integrationBandwidth));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgCarrierRBWFilter(instrumentHandle, "", RBWAuto,
      RBW, RBWFilterType));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgCarrierRRCFilter(instrumentHandle, "",
      RRCFilterEnabled, RRCAlpha));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgNumberOfOffsets(instrumentHandle, "", numOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetFrequencyArray(instrumentHandle, "", offsetStartFrequency,
      offsetStopFrequency, offsetEnabled, offsetSideband, numOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetAbsoluteLimitArray(instrumentHandle, "", absoluteLimitMode,
      absoluteLimitStart, absoluteLimitStop, numOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetRelativeLimitArray(instrumentHandle, "", relativeLimitMode,
      relativeLimitStart, relativeLimitStop, numOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetRBWFilterArray(instrumentHandle, "", offsetRBWAuto, offsetRBW, offsetRBWFilterType,
      numOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_BuildOffsetString2("", -1, MAX_SELECTOR_STRING, offsetString));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetLimitFailMask(instrumentHandle, offsetString, offsetLimitFailMask));
   RFmxCheckWarn(RFmxSpecAn_SEMSetAmplitudeCorrectionType(instrumentHandle, "", amplitudeCorrectionType));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */

   RFmxCheckWarn(RFmxSpecAn_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout,
      lowerOffsetMeasurementStatus, lowerOffsetMargin, lowerOffsetMarginFrequency,
      lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower, numOfOffsets, NULL));

   RFmxCheckWarn(RFmxSpecAn_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout,
      upperOffsetMeasurementStatus, upperOffsetMargin, upperOffsetMarginFrequency,
      upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower, numOfOffsets, NULL));

   RFmxCheckWarn(RFmxSpecAn_SEMFetchCarrierMeasurement(instrumentHandle, "", timeout,
      &carrierAbsolutePower, NULL, NULL, NULL));

   RFmxCheckWarn(RFmxSpecAn_SEMFetchAbsoluteMaskTrace(instrumentHandle, "", timeout, NULL, NULL, NULL,
      0, &actualArraySize));
   if (actualArraySize > 0)
   {
      absoluteMask = (float32*)malloc(sizeof(float32)*actualArraySize);
      if (absoluteMask)
      {
         RFmxCheckWarn(RFmxSpecAn_SEMFetchAbsoluteMaskTrace(instrumentHandle, "", timeout, &x0, &dx,
            absoluteMask, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxSpecAn_SEMFetchRelativeMaskTrace(instrumentHandle, "", timeout, NULL, NULL, NULL,
      0, &actualArraySize));
   if (actualArraySize > 0)
   {
      relativeMask = (float32*)malloc(sizeof(float32)*actualArraySize);
      if (relativeMask)
      {
         RFmxCheckWarn(RFmxSpecAn_SEMFetchRelativeMaskTrace(instrumentHandle, "", timeout, &x0, &dx,
            relativeMask, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxSpecAn_SEMFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL,
      NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32*)malloc(sizeof(float32)*actualArraySize);
      if (spectrum)
      {
         RFmxCheckWarn(RFmxSpecAn_SEMFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx,
            spectrum, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }


   RFmxCheckWarn(RFmxSpecAn_SEMFetchCompositeMeasurementStatus(instrumentHandle, "", timeout, &compositeMeasurementStatus));

   /* Display the results of the measurement */

   printf("Measurement Status                       : %s\n", (compositeMeasurementStatus) ? "Pass" : "Fail");
   printf("Carrier Absolute Power (dBm or dBm/Hz)   : %lf\n", carrierAbsolutePower);

   printf("\n---------------Lower Offset---------------\n");
   printf("\nLower Offset Segment Measurements \n");
   for (i = 0; i < numOfOffsets; i++)
   {
      printf("\nOffset : %d\n", i);
      printf("Margin (dB)                              : %lf\n", lowerOffsetMargin[i]);
      printf("Margin Absolute Power (dBm)              : %lf\n", lowerOffsetMarginAbsolutePower[i]);
      printf("Margin Relative Power (dB)               : %lf\n", lowerOffsetMarginRelativePower[i]);
      printf("Margin Frequency (Hz)                    : %lf\n", lowerOffsetMarginFrequency[i]);
      printf("Measurement Status                       : %s\n", (lowerOffsetMeasurementStatus[i]) ? "Pass" : "Fail");
   }

   printf("\n---------------Upper Offset---------------\n");
   printf("\nUpper Offset Segment Measurements \n");
   for (i = 0; i < numOfOffsets; i++)
   {
      printf("\nOffset : %d\n", i);
      printf("Margin (dB)                              : %lf\n", upperOffsetMargin[i]);
      printf("Margin Absolute Power (dBm)              : %lf\n", upperOffsetMarginAbsolutePower[i]);
      printf("Margin Relative Power (dB)               : %lf\n", upperOffsetMarginRelativePower[i]);
      printf("Margin Frequency (Hz)                    : %lf\n", upperOffsetMarginFrequency[i]);
      printf("Measurement Status                       : %s\n", (upperOffsetMeasurementStatus[i]) ? "Pass" : "Fail");
   }

Error:
   if (error)
   {
      RFmxSpecAn_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (instrumentHandle)
   {
      RFmxSpecAn_Close(instrumentHandle, RFMXSPECAN_VAL_FALSE);
   }

   /* Free allocated memory */
   if (spectrum)
      free(spectrum);
   if (absoluteMask)
      free(absoluteMask);
   if (relativeMask)
      free(relativeMask);

   printf("Press any key to exit\n");
   _getch();
   return error;
}
