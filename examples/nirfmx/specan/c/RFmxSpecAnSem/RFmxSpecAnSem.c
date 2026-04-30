//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties (Clock Source, Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select SEM measurement and enable the traces
//6. Configure SEM Power Units and Reference Type
//7. Configure SEM Averaging
//8. Configure SEM Integration BW
//9. Configure SEM RBW Filter
//10. Configure SEM RRC Filter
//11. Configure SEM Number of Offsets
//12. Configure SEM Offset Frequency
//Use Array API's to configure all offset parameters as an array
//13. Configure SEM Offset Absolute Limit
//14. Configure SEM Offset Relative Limit
//15. Configure Offset RBW Filter
//16. Configure Offset Limit Fail Mask
//17. Initiate Measurement
//18. Fetch SEM Measurements and Traces
//19. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

#define NUMBER_OF_OFFSETS           2


int main(int argc, char *argv[])
{
   int32 i = 0;
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];

   char *resourceName = "RFSA";
   char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;        /* Hz */
   float64 referenceLevel = 0.00;         /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */
   float64 frequency = 10.0e+6;           /* Hz */

   float64 timeout = 10.0;                /* seconds */
   char *status = "Fail";

   int32 numberOfOffsets = NUMBER_OF_OFFSETS;

   /* Carrier channel settings variables */
   /* Integration Bandwidth */
   float64 integrationBandwidth = 2.0e+6;    /* Hz */

   /* RFW filter */
   int32 RBWAuto = RFMXSPECAN_VAL_SEM_RBW_AUTO_FALSE;
   int32 RBWFilterType = RFMXSPECAN_VAL_SEM_RBW_FILTER_TYPE_GAUSSIAN;
   float64 RBW = 10.0e+3;

   /* RRC filter */
   int32 RRCFilterEnabled = RFMXSPECAN_VAL_SEM_RRC_FILTER_ENABLED_FALSE;
   float64 RRCFilterAlpha = 0.220;

   /* Reference Type */
   int32 referenceType = RFMXSPECAN_VAL_SEM_REFERENCE_TYPE_INTEGRATION;

   /* Power units */
   int32 powerUnits = RFMXSPECAN_VAL_SEM_POWER_UNITS_DBM;

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_SEM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_SEM_AVERAGING_TYPE_RMS;

   /* Offset limit fail mask */
   int32 limitFailMask = RFMXSPECAN_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE;

   /* Offset segment settings variables */
   /* Offset segment enabled */
   int32 offsetEnabled[NUMBER_OF_OFFSETS] = { 0 };

   /* Offset frequency */
   int32 offsetFrequencySideband[NUMBER_OF_OFFSETS] = { 0 };
   float64 offsetStartFrequency[NUMBER_OF_OFFSETS] = { 0 };       /* Hz */
   float64 offsetStopFrequency[NUMBER_OF_OFFSETS] = { 0 };        /* Hz */

   /* RBW filter */
   int32 offsetRBWAuto[NUMBER_OF_OFFSETS] = { 0 };
   int32 offsetRBWFilterType[NUMBER_OF_OFFSETS] = { 0 };
   float64 offsetRBW[NUMBER_OF_OFFSETS] = { 0 };                  /* Hz */

   /* Absolute limit */
   int32 offsetAbsoluteLimitMode[NUMBER_OF_OFFSETS] = { 0 };
   float64 offsetAbsoluteStartLimit[NUMBER_OF_OFFSETS] = { 0 };   /* dBm */
   float64 offsetAbsoluteStopLimit[NUMBER_OF_OFFSETS] = { 0 };    /* dBm */

   /* Relative limit */
   int32 offsetRelativeLimitMode[NUMBER_OF_OFFSETS] = { 0 };
   float64 offsetRelativeStartLimit[NUMBER_OF_OFFSETS] = { 0 };   /* dB */
   float64 offsetRelativeStopLimit[NUMBER_OF_OFFSETS] = { 0 };    /* dB */

   /* Variables to store the measurement results */
   /* Composite measurement status */
   int32 compositeMeasurementStatus;
   /* Carrier measurement results */
   float64 absolutePower = 0;
   float64 peakAbsolutePower = 0;
   float64 peakFrequency = 0;

   /* variables to store traces */
   float64 x0 = 0.0, dx = 0.0;
   float32 *absoluteTrace = (float32 *)NULL;
   float32 *relativeTrace = (float32 *)NULL;
   float32 *spectrum = (float32 *)NULL;
   int32 actualArraySize = 0;

   float64 *lowerOffsetAbsolutePower = (float64 *)NULL;
   float64 *lowerOffsetTotalRelativePower = (float64 *)NULL;
   float64 *lowerOffsetPeakAbsolutePower = (float64 *)NULL;
   float64 *lowerOffsetPeakFrequency = (float64 *)NULL;
   float64 *lowerOffsetPeakRelativeFreq = (float64 *)NULL;
   float64 *lowerOffsetMargin = (float64 *)NULL;
   float64 *lowerOffsetMarginAbsolutePower = (float64 *)NULL;
   float64 *lowerOffsetMarginRelativePower = (float64 *)NULL;
   float64 *lowerOffsetMarginFrequency = (float64 *)NULL;
   int32	*lowerOffsetMeasurementStatus = (int32 *)NULL;

   float64 *upperOffsetAbsolutePower = (float64 *)NULL;
   float64 *upperOffsetTotalRelativePower = (float64 *)NULL;
   float64 *upperOffsetPeakAbsolutePower = (float64 *)NULL;
   float64 *upperOffsetPeakFrequency = (float64 *)NULL;
   float64 *upperOffsetPeakRelativePower = (float64 *)NULL;
   float64 *upperOffsetMargin = (float64 *)NULL;
   float64 *upperOffsetMarginAbsolutePower = (float64 *)NULL;
   float64 *upperOffsetMarginRelativePower = (float64 *)NULL;
   float64 *upperOffsetMarginFrequency = (float64 *)NULL;
   int32	*upperOffsetMeasurementStatus = (int32 *)NULL;

   int32 lowerOffsetPowerArraySize = 0;
   int32 lowerOffsetMarginArraySize = 0;
   int32 upperOffsetPowerArraySize = 0;
   int32 upperOffsetMarginArraySize = 0;

   status = "Fail";

   for (i = 0; i < numberOfOffsets; i++)
   {
      offsetEnabled[i] = RFMXSPECAN_VAL_SEM_OFFSET_ENABLED_TRUE;
      offsetFrequencySideband[i] = RFMXSPECAN_VAL_SEM_OFFSET_SIDEBAND_BOTH;
      if (i == 0)
      {
         offsetStartFrequency[i] = 1.0e+6;
         offsetStopFrequency[i] = 2.0e+6;

         offsetRelativeLimitMode[i] = RFMXSPECAN_VAL_SEM_OFFSET_RELATIVE_LIMIT_MODE_MANUAL;
         offsetRelativeStartLimit[i] = -10.00;
         offsetRelativeStopLimit[i] = -30.00;
      }
      else if (i == 1)
      {
         offsetStartFrequency[i] = 2.0e+6;
         offsetStopFrequency[i] = 3.0e+6;

         offsetRelativeLimitMode[i] = RFMXSPECAN_VAL_SEM_OFFSET_RELATIVE_LIMIT_MODE_COUPLE;
         offsetRelativeStartLimit[i] = -30.00;
         offsetRelativeStopLimit[i] = -30.00;
      }
      offsetRBWAuto[i] = RFMXSPECAN_VAL_SEM_RBW_AUTO_TRUE;
      offsetRBWFilterType[i] = RFMXSPECAN_VAL_SEM_RBW_FILTER_TYPE_GAUSSIAN;
      offsetRBW[i] = 10.0e+3;

      offsetAbsoluteLimitMode[i] = RFMXSPECAN_VAL_SEM_OFFSET_ABSOLUTE_LIMIT_MODE_COUPLE;
      offsetAbsoluteStartLimit[i] = -10.00;
      offsetAbsoluteStopLimit[i] = -10.00;

   }

   /* Create new RFmx session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure SEM measurement parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_SEM, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgPowerUnits(instrumentHandle, "", powerUnits));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgReferenceType(instrumentHandle, "", referenceType));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgCarrierIntegrationBandwidth(instrumentHandle, "", integrationBandwidth));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgCarrierRBWFilter(instrumentHandle, "", RBWAuto, RBW, RBWFilterType));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgCarrierRRCFilter(instrumentHandle, "", RRCFilterEnabled, RRCFilterAlpha));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgNumberOfOffsets(instrumentHandle, "", numberOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetFrequencyArray(instrumentHandle, "", offsetStartFrequency,
      offsetStopFrequency, offsetEnabled,
      offsetFrequencySideband, numberOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetAbsoluteLimitArray(instrumentHandle, "", offsetAbsoluteLimitMode,
      offsetAbsoluteStartLimit, offsetAbsoluteStopLimit,
      numberOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetRelativeLimitArray(instrumentHandle, "", offsetRelativeLimitMode,
      offsetRelativeStartLimit, offsetRelativeStopLimit,
      numberOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetRBWFilterArray(instrumentHandle, "", offsetRBWAuto, offsetRBW,
      offsetRBWFilterType, numberOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetLimitFailMask(instrumentHandle, "offset::all", limitFailMask));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_SEMFetchLowerOffsetPowerArray(instrumentHandle, "", timeout, NULL, NULL, NULL,
      NULL, NULL, 0, &lowerOffsetPowerArraySize));
   if (lowerOffsetPowerArraySize > 0)
   {
      lowerOffsetAbsolutePower = (float64 *)malloc(sizeof(float64)*lowerOffsetPowerArraySize);
      lowerOffsetTotalRelativePower = (float64 *)malloc(sizeof(float64)*lowerOffsetPowerArraySize);
      lowerOffsetPeakAbsolutePower = (float64 *)malloc(sizeof(float64)*lowerOffsetPowerArraySize);
      lowerOffsetPeakFrequency = (float64 *)malloc(sizeof(float64)*lowerOffsetPowerArraySize);
      lowerOffsetPeakRelativeFreq = (float64 *)malloc(sizeof(float64)*lowerOffsetPowerArraySize);

      RFmxCheckWarn(RFmxSpecAn_SEMFetchLowerOffsetPowerArray(instrumentHandle, "", timeout, lowerOffsetAbsolutePower,
         lowerOffsetTotalRelativePower, lowerOffsetPeakAbsolutePower,
         lowerOffsetPeakFrequency, lowerOffsetPeakRelativeFreq,
         lowerOffsetPowerArraySize, NULL));
   }

   RFmxCheckWarn(RFmxSpecAn_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL,
      NULL, NULL, 0, &lowerOffsetMarginArraySize));
   if (lowerOffsetMarginArraySize > 0)
   {
      lowerOffsetMargin = (float64 *)malloc(sizeof(float64)*lowerOffsetMarginArraySize);
      lowerOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64)*lowerOffsetMarginArraySize);
      lowerOffsetMarginRelativePower = (float64 *)malloc(sizeof(float64)*lowerOffsetMarginArraySize);
      lowerOffsetMarginFrequency = (float64 *)malloc(sizeof(float64)*lowerOffsetMarginArraySize);
      lowerOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32)*lowerOffsetMarginArraySize);

      RFmxCheckWarn(RFmxSpecAn_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, lowerOffsetMeasurementStatus,
         lowerOffsetMargin, lowerOffsetMarginFrequency,
         lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower,
         lowerOffsetMarginArraySize, NULL));
   }

   RFmxCheckWarn(RFmxSpecAn_SEMFetchUpperOffsetPowerArray(instrumentHandle, "", timeout, NULL, NULL, NULL,
      NULL, NULL, 0, &upperOffsetPowerArraySize));
   if (upperOffsetPowerArraySize > 0)
   {
      upperOffsetAbsolutePower = (float64 *)malloc(sizeof(float64)*upperOffsetPowerArraySize);
      upperOffsetTotalRelativePower = (float64 *)malloc(sizeof(float64)*upperOffsetPowerArraySize);
      upperOffsetPeakAbsolutePower = (float64 *)malloc(sizeof(float64)*upperOffsetPowerArraySize);
      upperOffsetPeakFrequency = (float64 *)malloc(sizeof(float64)*upperOffsetPowerArraySize);
      upperOffsetPeakRelativePower = (float64 *)malloc(sizeof(float64)*upperOffsetPowerArraySize);

      RFmxCheckWarn(RFmxSpecAn_SEMFetchUpperOffsetPowerArray(instrumentHandle, "", timeout, upperOffsetAbsolutePower,
         upperOffsetTotalRelativePower, upperOffsetPeakAbsolutePower,
         upperOffsetPeakFrequency, upperOffsetPeakRelativePower,
         upperOffsetPowerArraySize, NULL));
   }

   RFmxCheckWarn(RFmxSpecAn_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL,
      NULL, NULL, 0, &upperOffsetMarginArraySize));
   if (upperOffsetMarginArraySize > 0)
   {
      upperOffsetMargin = (float64 *)malloc(sizeof(float64)*upperOffsetMarginArraySize);
      upperOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64)*upperOffsetMarginArraySize);
      upperOffsetMarginRelativePower = (float64 *)malloc(sizeof(float64)*upperOffsetMarginArraySize);
      upperOffsetMarginFrequency = (float64 *)malloc(sizeof(float64)*upperOffsetMarginArraySize);
      upperOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32)*upperOffsetMarginArraySize);

      RFmxCheckWarn(RFmxSpecAn_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout,
         upperOffsetMeasurementStatus, upperOffsetMargin,
         upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower,
         upperOffsetMarginRelativePower, upperOffsetMarginArraySize,
         NULL));
   }

   RFmxCheckWarn(RFmxSpecAn_SEMFetchCarrierMeasurement(instrumentHandle, "", timeout, &absolutePower, &peakAbsolutePower,
      &peakFrequency, NULL));

   /* Fetch absolute and relative traces */
   RFmxCheckWarn(RFmxSpecAn_SEMFetchAbsoluteMaskTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      absoluteTrace = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (absoluteTrace)
      {
         RFmxCheckWarn(RFmxSpecAn_SEMFetchAbsoluteMaskTrace(instrumentHandle, "", timeout, &x0, &dx,
            absoluteTrace, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0; x0 = 0; dx = 0;
   RFmxCheckWarn(RFmxSpecAn_SEMFetchRelativeMaskTrace(instrumentHandle, "", timeout, NULL, NULL, NULL,
      0, &actualArraySize));
   if (actualArraySize > 0)
   {
      relativeTrace = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (relativeTrace)
      {
         RFmxCheckWarn(RFmxSpecAn_SEMFetchRelativeMaskTrace(instrumentHandle, "", timeout, &x0, &dx,
            relativeTrace, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }
   /* Fetch spectrum */
   actualArraySize = 0; x0 = 0; dx = 0;
   RFmxCheckWarn(RFmxSpecAn_SEMFetchSpectrum(instrumentHandle, "", 10.0, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (spectrum)
      {
         RFmxCheckWarn(RFmxSpecAn_SEMFetchSpectrum(instrumentHandle, "", 10.0, &x0, &dx, spectrum, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }
   /* Fetch composite measurement status */
   RFmxCheckWarn(RFmxSpecAn_SEMFetchCompositeMeasurementStatus(instrumentHandle, "", timeout, &compositeMeasurementStatus));

   /* Display results */
   if (compositeMeasurementStatus == RFMXSPECAN_VAL_SEM_COMPOSITE_MEASUREMENT_STATUS_PASS)
      status = "Pass";
   printf("Composite measurement status                          %s\n", status);
   printf("--------------Carrier Measurements----------------------------\n");
   printf("Absolute Power(dBm or dBm/Hz)                         %f\n", absolutePower);
   printf("Peak Absolute Power(dBm or dBm/Hz)                    %f\n", peakAbsolutePower);
   printf("Peak Frequency(Hz)                                    %f\n", peakFrequency);
   printf("--------------------------------------------------------------------\n\n");

   printf("--------------Offset segment measurements ---------------------------\n");
   for (i = 0; i < lowerOffsetPowerArraySize; i++)
   {
      printf("*** OFFSET %d ***\n", i + 1);
      printf("Lower offset : Total Absolute Power(dBm or dBm/Hz)   %f\n", lowerOffsetAbsolutePower[i]);
      printf("Lower offset : Total Relative Power(dB)              %f\n", lowerOffsetTotalRelativePower[i]);
      printf("Lower Offset : Peak Absolute Power(dBm or dBm/Hz)    %f\n", lowerOffsetPeakAbsolutePower[i]);
      printf("Lower offset : Peak Frequency(Hz)                    %f\n", lowerOffsetPeakFrequency[i]);
      printf("Lower offset : Peak Relative Power(dB)               %f\n", lowerOffsetPeakRelativeFreq[i]);
      printf("Lower Offset : Margin(dB)                            %f\n", lowerOffsetMargin[i]);
      printf("Lower offset : Margin Absolute Power(dBm or dBm/Hz)  %f\n", lowerOffsetMarginAbsolutePower[i]);
      printf("Lower offset : Margin Relative Power(dB)             %f\n", lowerOffsetMarginRelativePower[i]);
      printf("Lower offset : Margin Frequency(Hz)                  %f\n", lowerOffsetMarginFrequency[i]);

      status = "Fail";
      if (lowerOffsetMeasurementStatus[i] == RFMXSPECAN_VAL_SEM_MEASUREMENT_STATUS_PASS)
         status = "Pass";
      printf("Lower offset : Measurement Status : %s\n", status);
      printf("\n");

      printf("Upper offset : Total Absolute Power(dBm or dBm/Hz)  %f\n", upperOffsetAbsolutePower[i]);
      printf("Upper offset : Total Relative Power(dB)             %f\n", upperOffsetTotalRelativePower[i]);
      printf("Upper Offset : Peak Absolute Power(dBm or dBm/Hz)   %f\n", upperOffsetPeakAbsolutePower[i]);
      printf("Upper offset : Peak Frequency(Hz)                   %f\n", upperOffsetPeakFrequency[i]);
      printf("Upper offset : Peak Relative Power(dB)              %f\n", upperOffsetPeakRelativePower[i]);
      printf("Upper Offset : Margin(dB)                           %f\n", upperOffsetMargin[i]);
      printf("Upper offset : Margin Absolute Power(dBm or dBm/Hz) %f\n", upperOffsetMarginAbsolutePower[i]);
      printf("Upper offset : Margin Relative Power(dB)            %f\n", upperOffsetMarginRelativePower[i]);
      printf("Upper offset : Margin Frequency(Hz)                 %f\n", upperOffsetMarginFrequency[i]);

      status = "Fail";
      if (upperOffsetMeasurementStatus[i] == RFMXSPECAN_VAL_SEM_MEASUREMENT_STATUS_PASS)
         status = "Pass";
      printf("Upper offset : Measurement Status : %s\n", status);
      printf("-----------------------------------------------------------------------\n\n");
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
   if (absoluteTrace)
      free(absoluteTrace);
   if (relativeTrace)
      free(relativeTrace);
   if (spectrum)
      free(spectrum);

   if (lowerOffsetAbsolutePower)
      free(lowerOffsetAbsolutePower);
   if (lowerOffsetTotalRelativePower)
      free(lowerOffsetTotalRelativePower);
   if (lowerOffsetPeakAbsolutePower)
      free(lowerOffsetPeakAbsolutePower);
   if (lowerOffsetPeakFrequency)
      free(lowerOffsetPeakFrequency);
   if (lowerOffsetPeakRelativeFreq)
      free(lowerOffsetPeakRelativeFreq);
   if (lowerOffsetMargin)
      free(lowerOffsetMargin);
   if (lowerOffsetMarginAbsolutePower)
      free(lowerOffsetMarginAbsolutePower);
   if (lowerOffsetMarginRelativePower)
      free(lowerOffsetMarginRelativePower);
   if (lowerOffsetMarginFrequency)
      free(lowerOffsetMarginFrequency);
   if (lowerOffsetMeasurementStatus)
      free(lowerOffsetMeasurementStatus);

   if (upperOffsetAbsolutePower)
      free(upperOffsetAbsolutePower);
   if (lowerOffsetTotalRelativePower)
      free(upperOffsetTotalRelativePower);
   if (upperOffsetPeakAbsolutePower)
      free(upperOffsetPeakAbsolutePower);
   if (upperOffsetPeakFrequency)
      free(upperOffsetPeakFrequency);
   if (upperOffsetPeakRelativePower)
      free(upperOffsetPeakRelativePower);
   if (upperOffsetMargin)
      free(upperOffsetMargin);
   if (upperOffsetMarginAbsolutePower)
      free(upperOffsetMarginAbsolutePower);
   if (upperOffsetMarginRelativePower)
      free(upperOffsetMarginRelativePower);
   if (upperOffsetMarginFrequency)
      free(upperOffsetMarginFrequency);
   if (upperOffsetMeasurementStatus)
      free(upperOffsetMeasurementStatus);

   printf("Press any key to exit\n");
   _getch();

   return error;
}
