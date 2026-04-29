//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties (Clock Source, Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select SEM measurement and enable the traces
//6. Configure SEM Sweep Time
//7. Configure SEM Power Units and Reference Type
//8. Configure SEM Averaging
//9. Configure SEM FFT
//10. Configure SEM Number of Carrier Channel
//11. Configure SEM Carrier Offset, Integration BW, RBW Filter for all the Carriers using Selector String
//12. Configure SEM Number of Offsets
//13. Configure SEM Offset Frequency, RBW filter, Limit Fail Mask, Absolute Limit
//    for all offsets using Selector String
//14. Initiate Measurement
//15. Fetch SEM Lower Offset Power, Lower Offset Margin for all the offsets
//16. Fetch SEM Upper Offset Power, Upper Offset Margin for all the offsets
//17. Fetch SEM Carrier Measurement for all the Carriers
//18. Fetch SEM Traces
//19. Close the RFmx session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

/* Maximum size of selector string */
#define MAX_SELECTOR_STRING         256

#define NUMBER_OF_CARRIERS          1
#define NUMBER_OF_OFFSETS           2

/* Output: offset segment measurements structure */
typedef struct
{
   float64 offSegAbspower[NUMBER_OF_OFFSETS];
   float64 offSegTotalRelPower[NUMBER_OF_OFFSETS];
   float64 offSegPeakAbsPower[NUMBER_OF_OFFSETS];
   float64 offSegPeakFrequency[NUMBER_OF_OFFSETS];
   float64 offSegPeakRelFreq[NUMBER_OF_OFFSETS];
}offsetMeasPower_t;

typedef struct
{
   float64 offSegMargin[NUMBER_OF_OFFSETS];
   float64 offSegMarginAbsPwr[NUMBER_OF_OFFSETS];
   float64 offSegMarginRelPwr[NUMBER_OF_OFFSETS];
   float64 offSegMarginFreq[NUMBER_OF_OFFSETS];
}offsetMeasMargin_t;

typedef struct
{
   offsetMeasPower_t power;
   offsetMeasMargin_t margin;
   int32 status[NUMBER_OF_OFFSETS];
}offsetMeasurement_t;

typedef struct
{
   offsetMeasurement_t lowerOffset;
   offsetMeasurement_t upperOffset;
}offsetSegmentMeasurements_t;

/* Output: Carrier measurements structure */
typedef struct
{
   float64 abspower;          /* dBm or dBm/Hz */
   float64 totalRelPower;     /* dB */
   float64 peakAbsPower;      /* dBm or dBm/Hz */
   float64 peakFrequency;     /* Hz */
}carrierMeasurements_t;

/* Input: Carrier channels settings structure */
typedef struct {
   /* Carrier Offset */
   float64 carrierOffset;           /* Hz */

   /* Integration Bandwidth */
   float64 integrationBandwidth;    /* Hz */
   float64 channelBandwidth;        /* Hz */

   /* RBW filter */
   int32 RBWAuto;
   int32 RBWFilterType;
   float64 RBW;                     /* Hz */

   /* RRC Filter */
   int32 RRCFilterEnabled;
   float64 RRCFilterAlpha;
}carrierChannels_t;


int main(int argc, char *argv[])
{
   int32 i = 0;
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];
   char carrierString[MAX_SELECTOR_STRING], offsetString[MAX_SELECTOR_STRING];

   char *resourceName = "RFSA";
   char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;        /* Hz */
   float64 referenceLevel = 0.00;         /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */
   float64 frequency = 10.0e+6;           /* Hz */

   float64 timeout = 10.0;                /* seconds */

   int32 numberOfCarriers = NUMBER_OF_CARRIERS;
   int32 numberOfOffsets = NUMBER_OF_OFFSETS;

   char *status = "Fail";

   /* Carrier channels and offset segments input settings */
   carrierChannels_t	carrierChs[NUMBER_OF_CARRIERS];

   /* Reference Type */
   int32 referenceType = RFMXSPECAN_VAL_SEM_REFERENCE_TYPE_INTEGRATION;

   /* Power Units */
   int32 powerUnits = RFMXSPECAN_VAL_SEM_POWER_UNITS_DBM;

   /* Sweep time */
   int32 sweepTimeAuto = RFMXSPECAN_VAL_SEM_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 1.0e-3; /* seconds */

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_SEM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_SEM_AVERAGING_TYPE_RMS;

   /* FFT */
   int32 FFTWindow = RFMXSPECAN_VAL_SEM_FFT_WINDOW_FLAT_TOP;
   float64 FFTPadding = -1.00;

   /* Composite measurement status */
   int32 compMeasurementStatus = 0;

   /* Total carrier power */
   float64 toatalCarrierPower = 0;  /* dBm or dBm/Hz */

   /* Variables to store the measurement results */
   offsetSegmentMeasurements_t offMeas;
   carrierMeasurements_t carrierMeas[NUMBER_OF_CARRIERS];
   float64 x0 = 0.0, dx = 0.0;
   float32 *absTrace = (float32 *)NULL;
   float32 *relTrace = (float32 *)NULL;
   float32 *spectrum = (float32 *)NULL;
   int32 actualArraySize = 0;

   /* Variables to hold offset inputs*/
   int32 enabled[NUMBER_OF_OFFSETS];

   /* Offset frequency */
   float64 startFrequency[NUMBER_OF_OFFSETS];   /*Hz */
   float64 stopFrequency[NUMBER_OF_OFFSETS];    /*Hz */
   int32 freqSideband[NUMBER_OF_OFFSETS];

   /* RBW filter */
   float64 RBW[NUMBER_OF_OFFSETS];              /*Hz */
   int32 RBWAuto[NUMBER_OF_OFFSETS];
   int32 RBWFilterType[NUMBER_OF_OFFSETS];

   int32 limitFailMask[NUMBER_OF_OFFSETS];

   /* Absolute limit */
   int32 absLimitMode[NUMBER_OF_OFFSETS];
   float64 absStartLimit[NUMBER_OF_OFFSETS];    /* dBm */
   float64 absStopLimit[NUMBER_OF_OFFSETS];     /* dBm */

   /* Relative limit */
   int32 relLimitMode[NUMBER_OF_OFFSETS];
   float64 relStartLimit[NUMBER_OF_OFFSETS];    /* dBm */
   float64 relStopLimit[NUMBER_OF_OFFSETS];     /* dBm */

   int32 frequencyDefinition[NUMBER_OF_OFFSETS];
   /* Set up the carrier channel variables */
   for (i = 0; i < numberOfCarriers; i++)
   {
      carrierChs[i].carrierOffset = 0.00;
      carrierChs[i].integrationBandwidth = 2.0e+6;
      carrierChs[i].channelBandwidth = 2.0e+6;

      carrierChs[i].RBWAuto = RFMXSPECAN_VAL_SEM_RBW_AUTO_TRUE;
      carrierChs[i].RBWFilterType = RFMXSPECAN_VAL_SEM_RBW_FILTER_TYPE_GAUSSIAN;
      carrierChs[i].RBW = 10.0e+3;

      carrierChs[i].RRCFilterEnabled = RFMXSPECAN_VAL_SEM_RRC_FILTER_ENABLED_FALSE;
      carrierChs[i].RRCFilterAlpha = 0.220;
   }

   /* Set up offset segment variables */
   for (i = 0; i < numberOfOffsets; i++)
   {
      if (i == 0)
      {
         startFrequency[i] = 1.0e+6;
         stopFrequency[i] = 2.0e+6;
         relLimitMode[i] = RFMXSPECAN_VAL_SEM_OFFSET_RELATIVE_LIMIT_MODE_MANUAL;
         relStartLimit[i] = -10.00;
         relStopLimit[i] = -30.00;
      }
      else
      {
         startFrequency[i] = 2.0e+6;
         stopFrequency[i] = 3.0e+6;
         relLimitMode[i] = RFMXSPECAN_VAL_SEM_OFFSET_RELATIVE_LIMIT_MODE_COUPLE;
         relStartLimit[i] = -30.00;
         relStopLimit[i] = -30.00;
      }
      enabled[i] = RFMXSPECAN_VAL_SEM_OFFSET_ENABLED_TRUE;

      freqSideband[i] = RFMXSPECAN_VAL_SEM_OFFSET_SIDEBAND_BOTH;

      RBWAuto[i] = RFMXSPECAN_VAL_SEM_RBW_AUTO_TRUE;
      RBWFilterType[i] = RFMXSPECAN_VAL_SEM_RBW_FILTER_TYPE_GAUSSIAN;
      RBW[i] = 10.0e+3;

      limitFailMask[i] = RFMXSPECAN_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE;

      absLimitMode[i] = RFMXSPECAN_VAL_SEM_OFFSET_ABSOLUTE_LIMIT_MODE_COUPLE;
      absStartLimit[i] = -10.00;
      absStopLimit[i] = -10.00;
      frequencyDefinition[i] = RFMXSPECAN_VAL_SEM_CARRIER_CENTER_TO_MEASUREMENT_BANDWIDTH_CENTER;
   }

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure SEM measurement parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_SEM, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgPowerUnits(instrumentHandle, "", powerUnits));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgReferenceType(instrumentHandle, "", referenceType));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgFFT(instrumentHandle, "", FFTWindow, FFTPadding));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgNumberOfCarriers(instrumentHandle, "", numberOfCarriers));

   for (i = 0; i < numberOfCarriers; i++)
   {
      RFmxSpecAn_BuildCarrierString2("", i, MAX_SELECTOR_STRING, carrierString);
      RFmxCheckWarn(RFmxSpecAn_SEMCfgCarrierFrequency(instrumentHandle, carrierString, carrierChs[i].carrierOffset));
      RFmxCheckWarn(RFmxSpecAn_SEMCfgCarrierIntegrationBandwidth(instrumentHandle, carrierString,
         carrierChs[i].integrationBandwidth));
      RFmxCheckWarn(RFmxSpecAn_SEMCfgCarrierRBWFilter(instrumentHandle, carrierString,
         carrierChs[i].RBWAuto, carrierChs[i].RBW,
         carrierChs[i].RBWFilterType));
      RFmxCheckWarn(RFmxSpecAn_SEMCfgCarrierRRCFilter(instrumentHandle, carrierString,
         carrierChs[i].RRCFilterEnabled,
         carrierChs[i].RRCFilterAlpha));
      RFmxCheckWarn(RFmxSpecAn_SEMCfgCarrierChannelBandwidth(instrumentHandle, carrierString,
         carrierChs[i].channelBandwidth));
   }

   RFmxCheckWarn(RFmxSpecAn_SEMCfgNumberOfOffsets(instrumentHandle, "", numberOfOffsets));

   for (i = 0; i < numberOfOffsets; i++)
   {
      RFmxSpecAn_BuildOffsetString2("", i, MAX_SELECTOR_STRING, offsetString);

      RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetFrequencyDefinition(instrumentHandle, offsetString,
         frequencyDefinition[i]));
      RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetLimitFailMask(instrumentHandle, offsetString,
         limitFailMask[i]));
   }

   RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetFrequencyArray(instrumentHandle, "",
      startFrequency, stopFrequency,
      enabled, freqSideband, numberOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetRBWFilterArray(instrumentHandle, "",
      RBWAuto, RBW,
      RBWFilterType, numberOfOffsets));

   RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetAbsoluteLimitArray(instrumentHandle, "",
      absLimitMode, absStartLimit,
      absStopLimit, numberOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_SEMCfgOffsetRelativeLimitArray(instrumentHandle, "",
      relLimitMode, relStartLimit,
      relStopLimit, numberOfOffsets));

   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */

   RFmxCheckWarn(RFmxSpecAn_SEMFetchLowerOffsetPowerArray(instrumentHandle, "", timeout,
      offMeas.lowerOffset.power.offSegAbspower,
      offMeas.lowerOffset.power.offSegTotalRelPower,
      offMeas.lowerOffset.power.offSegPeakAbsPower,
      offMeas.lowerOffset.power.offSegPeakFrequency,
      offMeas.lowerOffset.power.offSegPeakRelFreq, numberOfOffsets, NULL));

   RFmxCheckWarn(RFmxSpecAn_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout,
      offMeas.lowerOffset.status,
      offMeas.lowerOffset.margin.offSegMargin,
      offMeas.lowerOffset.margin.offSegMarginFreq,
      offMeas.lowerOffset.margin.offSegMarginAbsPwr,
      offMeas.lowerOffset.margin.offSegMarginRelPwr, numberOfOffsets, NULL));

   RFmxCheckWarn(RFmxSpecAn_SEMFetchUpperOffsetPowerArray(instrumentHandle, "", timeout,
      offMeas.upperOffset.power.offSegAbspower,
      offMeas.upperOffset.power.offSegTotalRelPower,
      offMeas.upperOffset.power.offSegPeakAbsPower,
      offMeas.upperOffset.power.offSegPeakFrequency,
      offMeas.upperOffset.power.offSegPeakRelFreq, numberOfOffsets, NULL));

   RFmxCheckWarn(RFmxSpecAn_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout,
      offMeas.upperOffset.status,
      offMeas.upperOffset.margin.offSegMargin,
      offMeas.upperOffset.margin.offSegMarginFreq,
      offMeas.upperOffset.margin.offSegMarginAbsPwr,
      offMeas.upperOffset.margin.offSegMarginRelPwr, numberOfOffsets, NULL));

   for (i = 0; i < numberOfCarriers; i++)
   {
      RFmxSpecAn_BuildCarrierString2("", i, MAX_SELECTOR_STRING, carrierString);
      RFmxCheckWarn(RFmxSpecAn_SEMFetchCarrierMeasurement(instrumentHandle, carrierString, timeout,
         &carrierMeas[i].abspower,
         &carrierMeas[i].peakAbsPower,
         &carrierMeas[i].peakFrequency,
         &carrierMeas[i].totalRelPower));
   }

   RFmxCheckWarn(RFmxSpecAn_SEMFetchAbsoluteMaskTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      absTrace = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (absTrace)
      {
         RFmxCheckWarn(RFmxSpecAn_SEMFetchAbsoluteMaskTrace(instrumentHandle, "", timeout, &x0, &dx, absTrace,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0; x0 = 0; dx = 0;
   RFmxCheckWarn(RFmxSpecAn_SEMFetchRelativeMaskTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      relTrace = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (relTrace)
      {
         RFmxCheckWarn(RFmxSpecAn_SEMFetchRelativeMaskTrace(instrumentHandle, "", timeout, &x0, &dx,
            relTrace, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0; x0 = 0; dx = 0;
   RFmxCheckWarn(RFmxSpecAn_SEMFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (spectrum)
      {
         RFmxCheckWarn(RFmxSpecAn_SEMFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxSpecAn_SEMFetchTotalCarrierPower(instrumentHandle, "", timeout, &toatalCarrierPower));
   RFmxCheckWarn(RFmxSpecAn_SEMFetchCompositeMeasurementStatus(instrumentHandle, "", timeout, &compMeasurementStatus));

   /* Display results */
   if (compMeasurementStatus == RFMXSPECAN_VAL_SEM_COMPOSITE_MEASUREMENT_STATUS_PASS)
      status = "Pass";
   printf("Composite measurement status                          %s\n", status);
   printf("Total Carrier Power(dBm or dBm/Hz)                    %f\n", toatalCarrierPower);

   printf("--------------Carrier Measurements----------------------------\n");
   for (i = 0; i < numberOfCarriers; i++)
   {
      printf("*** CARRIER %d ***\n", i + 1);
      printf("Absolute Power(dBm or dBm/Hz)                         %f\n", carrierMeas[i].abspower);
      printf("Total Relative Power(dB)                              %f\n", carrierMeas[i].totalRelPower);
      printf("Peak Absolute Power(dBm or dBm/Hz)                    %f\n", carrierMeas[i].peakAbsPower);
      printf("Peak Frequency                                        %f\n", carrierMeas[i].peakFrequency);
      printf("--------------------------------------------------------------------\n\n");
   }

   printf("--------------Offset segment measurements ---------------------------\n");
   for (i = 0; i < numberOfOffsets; i++)
   {
      printf("*** OFFSET %d ***\n", i + 1);
      printf("Lower offset : Total Absolute Power(dBm or dBm/Hz)    %f\n", offMeas.lowerOffset.power.offSegAbspower[i]);
      printf("Lower offset : Total Relative Power(dB)               %f\n", offMeas.lowerOffset.power.offSegTotalRelPower[i]);
      printf("Lower Offset : Peak Absolute Power(dBm or dBm/Hz)     %f\n", offMeas.lowerOffset.power.offSegPeakAbsPower[i]);
      printf("Lower offset : Peak Frequency(Hz)                     %f\n", offMeas.lowerOffset.power.offSegPeakFrequency[i]);
      printf("Lower offset : Peak Relative Power(dB)                %f\n", offMeas.lowerOffset.power.offSegPeakRelFreq[i]);
      printf("Lower Offset : Margin(dB)                             %f\n", offMeas.lowerOffset.margin.offSegMargin[i]);
      printf("Lower offset : Margin Absolute Power(dBm or dBm/Hz)   %f\n", offMeas.lowerOffset.margin.offSegMarginAbsPwr[i]);
      printf("Lower offset : Margin Relative Power(dB)              %f\n", offMeas.lowerOffset.margin.offSegMarginRelPwr[i]);
      printf("Lower offset : Margin Frequency(Hz)                   %f\n", offMeas.lowerOffset.margin.offSegMarginFreq[i]);

      status = "Fail";
      if (offMeas.lowerOffset.status[i] == RFMXSPECAN_VAL_SEM_MEASUREMENT_STATUS_PASS)
         status = "Pass";
      printf("Lower offset : Measurement Status                     %s\n\n", status);

      printf("Upper offset : Total Absolute Power(dBm or dBm/Hz)   %f\n", offMeas.upperOffset.power.offSegAbspower[i]);
      printf("Upper offset : Total Relative Power(dB)              %f\n", offMeas.upperOffset.power.offSegTotalRelPower[i]);
      printf("Upper Offset : Peak Absolute Power(dBm or dBm/Hz)    %f\n", offMeas.upperOffset.power.offSegPeakAbsPower[i]);
      printf("Upper offset : Peak Frequency(Hz)                    %f\n", offMeas.upperOffset.power.offSegPeakFrequency[i]);
      printf("Upper offset : Peak Relative Power(dB)               %f\n", offMeas.upperOffset.power.offSegPeakRelFreq[i]);
      printf("Upper Offset : Margin(dB)                            %f\n", offMeas.upperOffset.margin.offSegMargin[i]);
      printf("Upper offset : Margin Absolute Power(dBm or dBm/Hz)  %f\n", offMeas.upperOffset.margin.offSegMarginAbsPwr[i]);
      printf("Upper offset : Margin Relative Power(dB)             %f\n", offMeas.upperOffset.margin.offSegMarginRelPwr[i]);
      printf("Upper offset : Margin Frequency(Hz)                  %f\n", offMeas.upperOffset.margin.offSegMarginFreq[i]);

      status = "Fail";
      if (offMeas.upperOffset.status[i] == RFMXSPECAN_VAL_SEM_MEASUREMENT_STATUS_PASS)
         status = "Pass";
      printf("Upper offset : Measurement Status                     %s\n", status);
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
   if (absTrace)
      free(absTrace);
   if (relTrace)
      free(relTrace);
   if (spectrum)
      free(spectrum);
   printf("Press any key to exit\n");
   _getch();

   return error;
}
