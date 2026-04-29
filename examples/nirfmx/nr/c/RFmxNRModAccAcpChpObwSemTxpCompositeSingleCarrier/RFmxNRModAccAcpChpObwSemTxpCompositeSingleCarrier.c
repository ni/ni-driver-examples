//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Link Direction as Uplink, Frequency Range, Band, Component Carrier and Subcarrier Spacing.
//7. Select ModAcc, ACP, CHP, OBW, SEM and TXP measurements and enable Traces.
//8. Configure ACP Sweep Time.
//9. Configure CHP Sweep Time.
//10. Configure OBW Sweep Time.
//11. Configure SEM Sweep Time.
//12. Configure Averaging Parameters for ModAcc.
//13. Configure Averaging Parameters for ACP.
//14. Configure Averaging Parameters for CHP.
//15. Configure Averaging Parameters for OBW.
//16. Configure Averaging Parameters for SEM.
//17. Configure Averaging Parameters for TXP.
//18. Configure Measurement Interval for ModAcc.
//19. Configure Measurement Interval for TXP.
//20. Configure SEM Uplink Mask Type, or Downlink Mask, gNodeB Category, Delta F_Max(Hz) and Component Carrier Rated Output Power depending on Link Direction.
//21. Initiate the Measurement.
//22. Fetch ModAcc Measurements.
//23. Fetch ACP Measurements.
//24. Fetch CHP Measurements.
//25. Fetch OBW Measurements.
//26. Fetch SEM Measurements.
//27. Fetch TXP Measurements
//28. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxNR.h"

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

   char* selectedPorts = "";
   float64 centerFrequency = 3.5e9;                                                       /* (Hz) */
   float64 referenceLevel = 0.0;                                                          /* (dBm) */
   float64 externalAttenuation = 0.0;                                                     /* (dB) */

   char * frequencyReferenceSource = RFMXNR_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e6;                                          /* (Hz) */

   int32 enableTrigger = RFMXNR_VAL_FALSE;
   char * digitalEdgeSource = RFMXNR_VAL_PXI_TRIG0_STR;
   int32 digitalEdge = RFMXNR_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                                            /* (s) */

   int32 linkDirection = RFMXNR_VAL_LINK_DIRECTION_UPLINK;
   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;
   float64 carrierBandwidth = 100e6;                                                      /* (Hz) */
   float64 subcarrierSpacing = 30e3;                                                      /* (Hz) */
   int32 ModAccBand = 78;

   int32 measurementLengthUnit = RFMXNR_VAL_MODACC_MEASUREMENT_LENGTH_UNIT_SLOT;
   float64 measurementOffset = 0.0;
   float64 measurementLength = 1;

   float64 txpMeasurementOffset = 0.0;                                                    /* (s) */
   float64 txpMeasurementLength = 1e-3;                                                   /* (s) */

   int32 uplinkMaskType = RFMXNR_VAL_SEM_UPLINK_MASK_TYPE_GENERAL;

   int32 gNodeBCategory = RFMXNR_VAL_GNODEB_CATEGORY_WIDE_AREA_BASE_STATION_CATEGORY_A;
   int32 downlinkMaskType = RFMXNR_VAL_SEM_DOWNLINK_MASK_TYPE_STANDARD;
   float64 deltaFMaximum = 15.0e6;                                                        /* (Hz) */
   float64 componentCarrierRatedOutputPower = 0.0;                                        /* (dBm) */

   float64 sweepTimeInterval = 1.0e-3;                                                    /* (s) */

   int32 averagingCount = 10;

   float64 timeout = 10.0;                                                                /* (s) */

   float64 compositeRMSEVMMean = 0.0;                                                     /* (%) */
   float64 compositePeakEVMMaximum = 0.0;                                                 /* (%) */
   float64 componentCarrierFrequencyErrorMean = 0.0;                                      /* (Hz) */
   float64 componentCarrierIQOriginOffsetMean = 0.0;                                      /* (dBc) */

   float64 CHPAbsolutePower = 0.0;                                                        /* (dBm) */

   float64 ACPAbsolutePower = 0.0;                                                        /* (dBm) */
   float64* ACPLowerRelativePower = NULL;                                                 /* (dB) */
   float64* ACPUpperRelativePower = NULL;                                                 /* (dB) */
   float64* ACPLowerAbsolutePower = NULL;                                                 /* (dBm) */
   float64* ACPUpperAbsolutePower = NULL;                                                 /* (dBm) */
   int32 ACPOffsetMeasurementArraySize = 0;

   float64 OBWOccupiedBandwidth = 0.0;                                                    /* (Hz) */
   float64 OBWAbsolutePower = 0.0;                                                        /* (dBm) */
   float64 OBWStartFrequency = 0.0;                                                       /* (Hz) */
   float64 OBWStopFrequency = 0.0;                                                        /* (Hz) */

   int32 SEMMeasurementStatus = 0;
   float64 SEMAbsoluteIntegratedPower = 0.0;                                              /* (dBm) */
   int32* SEMLowerOffsetMeasurementStatus = NULL;
   float64* SEMLowerOffsetMargin = NULL;                                                  /* (dB) */
   float64* SEMLowerOffsetMarginFrequency = NULL;                                         /* (Hz) */
   float64* SEMLowerOffsetMarginAbsolutePower = NULL;                                     /* (dBm) */
   int32* SEMUpperOffsetMeasurementStatus = NULL;
   float64* SEMUpperOffsetMargin = NULL;                                                  /* (dB) */
   float64* SEMUpperOffsetMarginFrequency = NULL;                                         /* (Hz) */
   float64* SEMUpperOffsetMarginAbsolutePower = NULL;                                     /* (dBm) */
   int32 SEMLowerOffsetMeasurementArraySize = 0, SEMUpperOffsetMeasurementArraySize = 0;

   float64 averagePowerMean = 0.0;                                                        /* (dBm) */
   float64 peakPowerMaximum = 0.0;                                                        /* (dBm) */

   /* Initialize a session */
   RFmxCheckWarn(RFmxNR_Initialize(resourceName, "", &instrumentHandle, NULL));

   RFmxCheckWarn(RFmxNR_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));

   RFmxCheckWarn(RFmxNR_SetSelectedPorts(instrumentHandle, "", selectedPorts));

   RFmxCheckWarn(RFmxNR_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));

   RFmxCheckWarn(RFmxNR_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay,
      enableTrigger));

   RFmxCheckWarn(RFmxNR_SetLinkDirection(instrumentHandle, "", linkDirection));
   RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, "", frequencyRange));
   RFmxCheckWarn(RFmxNR_SetBand(instrumentHandle, "", ModAccBand));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, "", carrierBandwidth));
   RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, "", subcarrierSpacing));

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_MODACC | RFMXNR_VAL_ACP | RFMXNR_VAL_CHP |
      RFMXNR_VAL_OBW | RFMXNR_VAL_SEM | RFMXNR_VAL_TXP, RFMXNR_VAL_TRUE));

   RFmxCheckWarn(RFmxNR_ACPCfgSweepTime(instrumentHandle, "", RFMXNR_VAL_ACP_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));

   RFmxCheckWarn(RFmxNR_CHPCfgSweepTime(instrumentHandle, "", RFMXNR_VAL_CHP_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));

   RFmxCheckWarn(RFmxNR_OBWCfgSweepTime(instrumentHandle, "", RFMXNR_VAL_OBW_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));

   RFmxCheckWarn(RFmxNR_SEMCfgSweepTime(instrumentHandle, "", RFMXNR_VAL_SEM_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));

   RFmxCheckWarn(RFmxNR_ModAccSetAveragingEnabled(instrumentHandle, "", RFMXNR_VAL_MODACC_AVERAGING_ENABLED_FALSE));
   RFmxCheckWarn(RFmxNR_ModAccSetAveragingCount(instrumentHandle, "", averagingCount));

   RFmxCheckWarn(RFmxNR_ACPCfgAveraging(instrumentHandle, "", RFMXNR_VAL_ACP_AVERAGING_ENABLED_FALSE, averagingCount,
      RFMXNR_VAL_ACP_AVERAGING_TYPE_RMS));

   RFmxCheckWarn(RFmxNR_CHPCfgAveraging(instrumentHandle, "", RFMXNR_VAL_CHP_AVERAGING_ENABLED_FALSE, averagingCount,
      RFMXNR_VAL_CHP_AVERAGING_TYPE_RMS));

   RFmxCheckWarn(RFmxNR_OBWCfgAveraging(instrumentHandle, "", RFMXNR_VAL_OBW_AVERAGING_ENABLED_FALSE, averagingCount,
      RFMXNR_VAL_OBW_AVERAGING_TYPE_RMS));

   RFmxCheckWarn(RFmxNR_SEMCfgAveraging(instrumentHandle, "", RFMXNR_VAL_SEM_AVERAGING_ENABLED_FALSE, averagingCount,
      RFMXNR_VAL_SEM_AVERAGING_TYPE_RMS));

   RFmxCheckWarn(RFmxNR_TXPSetAveragingEnabled(instrumentHandle, "", RFMXNR_VAL_TXP_AVERAGING_ENABLED_FALSE));
   RFmxCheckWarn(RFmxNR_TXPSetAveragingCount(instrumentHandle, "", averagingCount));

   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementLengthUnit(instrumentHandle, "", measurementLengthUnit));
   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementOffset(instrumentHandle, "", measurementOffset));
   RFmxCheckWarn(RFmxNR_ModAccSetMeasurementLength(instrumentHandle, "", measurementLength));

   RFmxCheckWarn(RFmxNR_TXPSetMeasurementOffset(instrumentHandle, "", txpMeasurementOffset));
   RFmxCheckWarn(RFmxNR_TXPSetMeasurementInterval(instrumentHandle, "", txpMeasurementLength));

   if (linkDirection == RFMXNR_VAL_LINK_DIRECTION_UPLINK)
   {
      RFmxCheckWarn(RFmxNR_SEMCfgUplinkMaskType(instrumentHandle, "", uplinkMaskType));
   }
   else
   {
      RFmxCheckWarn(RFmxNR_CfggNodeBCategory(instrumentHandle, "", gNodeBCategory));
      RFmxCheckWarn(RFmxNR_SEMSetDownlinkMaskType(instrumentHandle, "", downlinkMaskType));
      RFmxCheckWarn(RFmxNR_SEMSetDeltaFMaximum(instrumentHandle, "", deltaFMaximum));
      RFmxCheckWarn(RFmxNR_SEMCfgComponentCarrierRatedOutputPower(instrumentHandle, "",
         componentCarrierRatedOutputPower));
   }

   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

   /* Fetch results */
   RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositeRMSEVMMean(instrumentHandle, "", &compositeRMSEVMMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMMaximum(instrumentHandle, "", &compositePeakEVMMaximum));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierFrequencyErrorMean(instrumentHandle, "",
      &componentCarrierFrequencyErrorMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsComponentCarrierIQOriginOffsetMean(instrumentHandle, "",
      &componentCarrierIQOriginOffsetMean));

   RFmxCheckWarn(RFmxNR_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL,
      0, &ACPOffsetMeasurementArraySize));
   if (ACPOffsetMeasurementArraySize > 0)
   {
      ACPLowerRelativePower = (float64 *)malloc(sizeof(float64) * ACPOffsetMeasurementArraySize);
      ACPUpperRelativePower = (float64 *)malloc(sizeof(float64) * ACPOffsetMeasurementArraySize);
      ACPLowerAbsolutePower = (float64 *)malloc(sizeof(float64) * ACPOffsetMeasurementArraySize);
      ACPUpperAbsolutePower = (float64 *)malloc(sizeof(float64) * ACPOffsetMeasurementArraySize);
      if (ACPLowerRelativePower && ACPUpperRelativePower && ACPLowerAbsolutePower && ACPUpperAbsolutePower)
      {
         RFmxCheckWarn(RFmxNR_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, ACPLowerRelativePower,
            ACPUpperRelativePower, ACPLowerAbsolutePower, ACPUpperAbsolutePower, ACPOffsetMeasurementArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxNR_ACPFetchComponentCarrierMeasurement(instrumentHandle, "", timeout, &ACPAbsolutePower, NULL));

   RFmxCheckWarn(RFmxNR_CHPFetchComponentCarrierMeasurement(instrumentHandle, "", timeout, &CHPAbsolutePower, NULL));

   RFmxCheckWarn(RFmxNR_OBWFetchMeasurement(instrumentHandle, "", timeout, &OBWOccupiedBandwidth, &OBWAbsolutePower,
      &OBWStartFrequency, &OBWStopFrequency));

   RFmxCheckWarn(RFmxNR_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL,
      0, &SEMLowerOffsetMeasurementArraySize));
   if (SEMLowerOffsetMeasurementArraySize > 0)
   {
      SEMLowerOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * SEMLowerOffsetMeasurementArraySize);
      SEMLowerOffsetMargin = (float64 *)malloc(sizeof(float64) * SEMLowerOffsetMeasurementArraySize);
      SEMLowerOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * SEMLowerOffsetMeasurementArraySize);
      SEMLowerOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * SEMLowerOffsetMeasurementArraySize);
      if (SEMLowerOffsetMeasurementStatus && SEMLowerOffsetMargin && SEMLowerOffsetMarginFrequency &&
         SEMLowerOffsetMarginAbsolutePower)
      {
         RFmxCheckWarn(RFmxNR_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout,
            SEMLowerOffsetMeasurementStatus, SEMLowerOffsetMargin, SEMLowerOffsetMarginFrequency,
            SEMLowerOffsetMarginAbsolutePower, NULL, SEMLowerOffsetMeasurementArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxNR_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL,
      0, &SEMUpperOffsetMeasurementArraySize));
   if (SEMUpperOffsetMeasurementArraySize > 0)
   {
      SEMUpperOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * SEMUpperOffsetMeasurementArraySize);
      SEMUpperOffsetMargin = (float64 *)malloc(sizeof(float64) * SEMUpperOffsetMeasurementArraySize);
      SEMUpperOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * SEMUpperOffsetMeasurementArraySize);
      SEMUpperOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * SEMUpperOffsetMeasurementArraySize);
      if (SEMUpperOffsetMeasurementStatus && SEMUpperOffsetMargin && SEMUpperOffsetMarginFrequency &&
         SEMUpperOffsetMarginAbsolutePower)
      {
         RFmxCheckWarn(RFmxNR_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout,
            SEMUpperOffsetMeasurementStatus, SEMUpperOffsetMargin, SEMUpperOffsetMarginFrequency,
            SEMUpperOffsetMarginAbsolutePower, NULL, SEMUpperOffsetMeasurementArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxNR_SEMFetchComponentCarrierMeasurement(instrumentHandle, "", timeout, &SEMAbsoluteIntegratedPower,
      NULL, NULL, NULL));

   RFmxCheckWarn(RFmxNR_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &SEMMeasurementStatus));

   RFmxCheckWarn(RFmxNR_TXPFetchMeasurement(instrumentHandle, "", timeout, &averagePowerMean, &peakPowerMaximum));

   printf("************************* ModAcc *************************\n\n");
   printf("Composite RMS EVM Mean (%%)                    : %lf\n", compositeRMSEVMMean);
   printf("Composite Peak EVM Maximum (%%)                : %lf\n", compositePeakEVMMaximum);
   printf("Component Carrier Frequency Error Mean (Hz)   : %lf\n", componentCarrierFrequencyErrorMean);
   printf("Component Carrier IQ Origin Offset Mean (dBc) : %lf\n\n", componentCarrierIQOriginOffsetMean);

   printf("\n\n************************* CHP *************************\n\n");
   printf("Carrier Absolute Power (dBm)                  : %lf\n\n", CHPAbsolutePower);

   printf("\n\n************************* ACP *************************\n\n");
   printf("Carrier Absolute Power (dBm)                  : %lf\n", ACPAbsolutePower);
   printf("\n------- Offset Channel Measurements ------- \n");
   for (i = 0; i < ACPOffsetMeasurementArraySize; i++)
   {
      printf("\nOffset  %d\n", i);
      printf("Lower Relative Power (dB)                     : %lf\n", ACPLowerRelativePower[i]);
      printf("Upper Relative Power (dB)                     : %lf\n", ACPUpperRelativePower[i]);
      printf("Lower Absolute Power (dBm)                    : %lf\n", ACPLowerAbsolutePower[i]);
      printf("Upper Absolute Power (dBm)                    : %lf\n", ACPUpperAbsolutePower[i]);
   }

   printf("\n\n\n************************* OBW *************************\n\n");
   printf("Occupied Bandwidth (Hz)                       : %lf\n", OBWOccupiedBandwidth);
   printf("Absolute Power (dBm)                          : %lf\n", OBWAbsolutePower);
   printf("Start Frequency (Hz)                          : %lf\n", OBWStartFrequency);
   printf("Stop Frequency (Hz)                           : %lf\n\n", OBWStopFrequency);

   printf("\n\n************************* SEM *************************\n\n");
   printf("Measurement Status                            : %s\n", (SEMMeasurementStatus) ? "PASS" : "FAIL");
   printf("Carrier Absolute Integrated Power (dBm)       : %lf\n", SEMAbsoluteIntegratedPower);
   printf("\n----- Lower Offset Segment Measurements -----\n");
   for (i = 0; i < SEMLowerOffsetMeasurementArraySize; i++)
   {
      printf("\nOffset  %d\n", i);
      printf("Measurement Status                            : %s\n", (SEMLowerOffsetMeasurementStatus[i]) ? "PASS" : "FAIL");
      printf("Margin (dB)                                   : %lf\n", SEMLowerOffsetMargin[i]);
      printf("Margin Frequency (Hz)                         : %lf\n", SEMLowerOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)                   : %lf\n", SEMLowerOffsetMarginAbsolutePower[i]);
   }
   printf("\n----- Upper Offset Segment Measurements -----\n");
   for (i = 0; i < SEMUpperOffsetMeasurementArraySize; i++)
   {
      printf("\nOffset  %d\n", i);
      printf("Measurement Status                            : %s\n", (SEMUpperOffsetMeasurementStatus[i]) ? "PASS" : "FAIL");
      printf("Margin (dB)                                   : %lf\n", SEMUpperOffsetMargin[i]);
      printf("Margin Frequency (Hz)                         : %lf\n", SEMUpperOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)                   : %lf\n", SEMUpperOffsetMarginAbsolutePower[i]);
   }

   printf("************************* TXP *************************\n\n");
   printf("Average Power Mean (dBm)                         : %lf\n", averagePowerMean);
   printf("Peak Power Maximum (dBm)                         : %lf\n", peakPowerMaximum);

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

   printf("\nPress any key to exit\n");
   _getch();

   return error;
}
