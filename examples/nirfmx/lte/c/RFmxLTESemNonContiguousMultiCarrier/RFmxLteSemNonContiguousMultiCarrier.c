//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5[A-F]. Configure Subblock Parameters.
//5A. Configure Number of Subblocks.
//5B. Configure subblock Frequency.
//5C. Configure Component Carrier Spacing.
//5D. Configure Number of Component Carriers.
//5E. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
//5F. Configure Component Carrier Maximum Output Power for Downlink Link Direction.
//6. Configure Link Direction.
//7. Select SEM measurement and enable Traces.
//8. Configure Sweep Time Parameters.
//9. Configure Averaging Parameters for SEM measurement.
//10. Configure Uplink Mask Type, or Downlink Mask, eNodeB Category.
//11. Initiate the Measurement.
//12. Fetch SEM Measurements and Traces.
//13. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION               4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                 256

#define NUMBER_OF_SUBBLOCKS                 2
#define NUMBER_OF_COMPONENT_CARRIERS        1


/* Input: Subblock inputs structure */
typedef struct
{
    float64 subblockFrequency;                                              /*(Hz) */
   int32 componentCarrierSpacingType;
   int32 componentCarrierAtCenterFrequency;
   float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS];         /*(Hz) */
   float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS];         /*(Hz) */
   float64 componentCarrierMaximumOutputPower[NUMBER_OF_COMPONENT_CARRIERS];/*(dBm) */
}subblockInputs_t;

/* Input: Subblock measurement outputs structure */
typedef struct
{
   float64 subblockPower;                                                   /*(dBm) */
   float64 integrationBandwidth;
   float64 subblockFrequency;

   float64 * lowerOffsetMarginRelativePower;                                /*(dBm) */
   float64 * lowerOffsetMarginAbsolutePower;                                /*(dBm) */
   float64 * lowerOffsetMargin;
   float64 * lowerOffsetMarginFrequency;                                    /*(Hz) */
   int32 * lowerOffsetMeasurementStatus;

   float64 * upperOffsetMarginRelativePower;                                /*(dBm) */
   float64 * upperOffsetMarginAbsolutePower;                                /*(dBm) */
   float64 * upperOffsetMargin;
   float64 * upperOffsetMarginFrequency;                                    /*(Hz) */
   int32 * upperOffsetMeasurementStatus;

   int32 numberOfOffsets;
}subblockMeasurements_t;

int main (int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
   int32 error = 0, lastErrorCode = 0, errorOccured = 0;
   int i = 0, j = 0;

   subblockInputs_t    subblocks[NUMBER_OF_SUBBLOCKS]= {                    /*  Set up subblock 0 inputs  */
	  {	 0.0,													            /* subblockFrequency */	
         RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL,
         -1,                                                                /* componentCarrierAtCenterFrequency */
         { 20e6 },                                                          /* componentCarrierBandwidth */
         { 0.0 },                                                           /* componentCarrierFrequency */
         { 0.0 }                                                            /* componentCarrierMaximumOutputPower */
      },
      {                                                                     /*  Set up subblock 1 inputs  */
		 30e6,													            /* subblockFrequency */	
         RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL,
         -1,                                                                /* componentCarrierAtCenterFrequency */
         { 20e6 },                                                          /* componentCarrierBandwidth */
         { 0.0 },                                                           /* componentCarrierFrequency */
         { 0.0 }                                                            /* componentCarrierMaximumOutputPower */
      }
   };
   char subblockString[MAX_SELECTOR_STRING];

   char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                              /*(Hz) */

   int32 enableTrigger = RFMXLTE_VAL_FALSE;
   char * digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
   int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                              /*(s) */

   float64 centerFrequency = 1.95e9;                                        /* (Hz) */
   float64 referenceLevel = 0.0;                                            /*(dBm) */
   float64 externalAttenuation = 0.0;                                       /*(dB) */

   int32 linkDirection = RFMXLTE_VAL_LINK_DIRECTION_UPLINK;

   /* Uplink */
   int32 uplinkMaskType = RFMXLTE_VAL_SEM_UPLINK_MASK_TYPE_GENERAL_NS01;

   /* Downlink */
   int32 eNodeBCategory = RFMXLTE_VAL_ENODEB_WIDE_AREA_BASE_STATION_CATEGORY_A;
   int32 downlinkMaskType =  RFMXLTE_VAL_SEM_DOWNLINK_MASK_TYPE_ENODEB_CATEGORY_BASED;
   float64 deltaFMaximum = 15.00e6;                                         /*(Hz) */
   float64 aggregatedMaximumPower = 0.00;                                   /*(dBm) */

   int32 averagingEnabled = RFMXLTE_VAL_SEM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXLTE_VAL_SEM_AVERAGING_TYPE_RMS;

   int32 sweepTimeAuto = RFMXLTE_VAL_SEM_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 0.001;                                       /*(s) */

   float64 timeout = 10.0;                                                  /*(s) */

   subblockMeasurements_t subblocksMsr[NUMBER_OF_SUBBLOCKS];

   int32 measurementStatus;

   int32 actualArraySize = 0, arraySize = 0;
   float64 x0=0.0,dx=0.0;
   float32 *spectrum = NULL;                                                /*(dBm) */

   float64 totalAggregatedPower = 0.0;                                      /*(dBm) */

   float32 *absoluteMask = NULL;

   /* Set up subblock outputs */
   for( i = 0; i < NUMBER_OF_SUBBLOCKS; i++ )
   {
      subblocksMsr[i].lowerOffsetMarginRelativePower = NULL;
      subblocksMsr[i].lowerOffsetMarginAbsolutePower = NULL;
      subblocksMsr[i].lowerOffsetMargin = NULL;
      subblocksMsr[i].lowerOffsetMarginFrequency = NULL;
      subblocksMsr[i].lowerOffsetMeasurementStatus = NULL;

      subblocksMsr[i].upperOffsetMarginRelativePower = NULL;
      subblocksMsr[i].upperOffsetMarginAbsolutePower = NULL;
      subblocksMsr[i].upperOffsetMargin = NULL;
      subblocksMsr[i].upperOffsetMarginFrequency = NULL;
      subblocksMsr[i].upperOffsetMeasurementStatus = NULL;
   }

   /* Initialize a session */
   RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
   RFmxCheckWarn(RFmxLTE_CfgNumberOfSubblocks(instrumentHandle, "", NUMBER_OF_SUBBLOCKS));
   for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      RFmxCheckWarn(RFmxLTE_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString));
      RFmxCheckWarn(RFmxLTE_SetSubblockFrequency(instrumentHandle, subblockString, subblocks[i].subblockFrequency));
      RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, subblockString,
         subblocks[i].componentCarrierSpacingType, subblocks[i].componentCarrierAtCenterFrequency));
      RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, subblockString, NUMBER_OF_COMPONENT_CARRIERS));
      RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, subblockString,
         subblocks[i].componentCarrierBandwidth, subblocks[i].componentCarrierFrequency,
         NULL, NUMBER_OF_COMPONENT_CARRIERS));
      if (linkDirection == RFMXLTE_VAL_LINK_DIRECTION_DOWNLINK)
         RFmxCheckWarn(RFmxLTE_SEMCfgComponentCarrierMaximumOutputPowerArray(instrumentHandle, subblockString,
            subblocks[i].componentCarrierMaximumOutputPower, NUMBER_OF_COMPONENT_CARRIERS));
   }
   RFmxCheckWarn(RFmxLTE_CfgLinkDirection(instrumentHandle, "", linkDirection));
   RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_SEM, RFMXLTE_VAL_TRUE));
   RFmxCheckWarn(RFmxLTE_SEMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxLTE_SEMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   if(linkDirection == RFMXLTE_VAL_LINK_DIRECTION_UPLINK)
       RFmxCheckWarn(RFmxLTE_SEMCfgUplinkMaskType(instrumentHandle, "", uplinkMaskType));
   else
   {
      RFmxCheckWarn(RFmxLTE_CfgeNodeBCategory(instrumentHandle, "", eNodeBCategory));
      RFmxCheckWarn(RFmxLTE_SEMCfgDownlinkMask(instrumentHandle, "", downlinkMaskType, deltaFMaximum, aggregatedMaximumPower));
   }
   RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

   for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      RFmxCheckWarn(RFmxLTE_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString));

      RFmxCheckWarn(RFmxLTE_SEMFetchUpperOffsetMarginArray(instrumentHandle, subblockString, timeout, NULL,
         NULL, NULL, NULL, NULL, 0, &actualArraySize));
      if(actualArraySize > 0)
      {
         subblocksMsr[i].upperOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * actualArraySize);
         subblocksMsr[i].upperOffsetMargin = (float64 *)malloc(sizeof(float64) * actualArraySize);
         subblocksMsr[i].upperOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * actualArraySize);
         subblocksMsr[i].upperOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
         subblocksMsr[i].upperOffsetMarginRelativePower = (float64 *)malloc(sizeof(float64) * actualArraySize);

         RFmxCheckWarn(RFmxLTE_SEMFetchUpperOffsetMarginArray(instrumentHandle, subblockString, timeout,
            subblocksMsr[i].upperOffsetMeasurementStatus, subblocksMsr[i].upperOffsetMargin, subblocksMsr[i].upperOffsetMarginFrequency,
            subblocksMsr[i].upperOffsetMarginAbsolutePower, subblocksMsr[i].upperOffsetMarginRelativePower,
            actualArraySize, &arraySize));
      }

      RFmxCheckWarn(RFmxLTE_SEMFetchLowerOffsetMarginArray(instrumentHandle, subblockString, timeout, NULL,
         NULL, NULL, NULL, NULL, 0, &actualArraySize));
      if(actualArraySize > 0)
      {
         subblocksMsr[i].lowerOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * actualArraySize);
         subblocksMsr[i].lowerOffsetMargin = (float64 *)malloc(sizeof(float64) * actualArraySize);
         subblocksMsr[i].lowerOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * actualArraySize);
         subblocksMsr[i].lowerOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
         subblocksMsr[i].lowerOffsetMarginRelativePower = (float64 *)malloc(sizeof(float64) * actualArraySize);

         RFmxCheckWarn(RFmxLTE_SEMFetchLowerOffsetMarginArray(instrumentHandle, subblockString, timeout,
            subblocksMsr[i].lowerOffsetMeasurementStatus, subblocksMsr[i].lowerOffsetMargin, subblocksMsr[i].lowerOffsetMarginFrequency,
            subblocksMsr[i].lowerOffsetMarginAbsolutePower, subblocksMsr[i].lowerOffsetMarginRelativePower,
            actualArraySize, &arraySize));
      }
      subblocksMsr[i].numberOfOffsets = arraySize * 2;

      RFmxCheckWarn(RFmxLTE_SEMFetchSubblockMeasurement(instrumentHandle, subblockString, timeout,
         &subblocksMsr[i].subblockPower, &subblocksMsr[i].integrationBandwidth, &subblocksMsr[i].subblockFrequency));
   }

   RFmxCheckWarn(RFmxLTE_SEMFetchTotalAggregatedPower(instrumentHandle, "", timeout, &totalAggregatedPower));
   RFmxCheckWarn(RFmxLTE_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));

   RFmxCheckWarn(RFmxLTE_SEMFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if( actualArraySize > 0 )
   {
      spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
      absoluteMask = (float32 *)malloc(sizeof(float32) * actualArraySize);
      if(spectrum)
      {
         RFmxCheckWarn(RFmxLTE_SEMFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, absoluteMask,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("Total Aggregated Power (dBm)     : %lf\n", totalAggregatedPower);
   printf("Measurement Status               : %s\n", measurementStatus == RFMXLTE_VAL_SEM_MEASUREMENT_STATUS_PASS?"PASS":"FAIL");

   printf("\n--------------------Subblock Measurements--------------------\n");
   for(i=0;i<NUMBER_OF_SUBBLOCKS;i++)
   {
      printf("\nSubblock %d\n", i);

      printf("Subblock Power (dBm)             : %lf\n", subblocksMsr[i].subblockPower);
      printf("Integration Bandwidth (Hz)       : %lf\n", subblocksMsr[i].integrationBandwidth);
      printf("Frequency (Hz)                   : %lf\n", subblocksMsr[i].subblockFrequency);

      printf("\n Offset Segment Measurements \n");
      for( j = 0; j < subblocksMsr[i].numberOfOffsets/2; j++)
      {
         printf("\nLower Offset Segment Measurement %d\n", j);
         printf("Measurement Status               : %s\n", subblocksMsr[i].lowerOffsetMeasurementStatus[j] == RFMXLTE_VAL_SEM_UPPER_OFFSET_MEASUREMENT_STATUS_PASS?"PASS":"FAIL");
         printf("Margin(dBm)                      : %lf\n", subblocksMsr[i].lowerOffsetMargin[j]);
         printf("Margin Frequency(Hz)             : %lf\n", subblocksMsr[i].lowerOffsetMarginFrequency[j]);
         printf("Margin Absolute Power(Hz)        : %lf\n", subblocksMsr[i].lowerOffsetMarginAbsolutePower[j]);

         printf("\nUpper Offset Segment Measurement %d\n", j);
         printf("Measurement Status               : %s\n", subblocksMsr[i].upperOffsetMeasurementStatus[j] == RFMXLTE_VAL_SEM_LOWER_OFFSET_MEASUREMENT_STATUS_PASS?"PASS":"FAIL");
         printf("Margin(dBm)                      : %lf\n", subblocksMsr[i].upperOffsetMargin[j]);
         printf("Margin Frequency(Hz)             : %lf\n", subblocksMsr[i].upperOffsetMarginFrequency[j]);
         printf("Margin Absolute Power(Hz)        : %lf\n", subblocksMsr[i].upperOffsetMarginAbsolutePower[j]);

      }
      printf("\n------------------------------------------------\n");

   }

Error:
   if( error )
   {
      errorOccured = error;
      RFmxLTE_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("\nERROR: %s\n", errorMessage);
      else
         printf("\nWARNING: %s\n", errorMessage);
   }

   if(instrumentHandle)
   {
      RFmxLTE_Close(instrumentHandle, RFMXLTE_VAL_FALSE);
   }

   if(spectrum)
      free(spectrum);
   if(absoluteMask)
      free(absoluteMask);

   for(i =0; i<NUMBER_OF_SUBBLOCKS;i++)
   {
      if(subblocksMsr[i].lowerOffsetMarginRelativePower)
         free(subblocksMsr[i].lowerOffsetMarginRelativePower);
      if(subblocksMsr[i].lowerOffsetMarginAbsolutePower)
         free(subblocksMsr[i].lowerOffsetMarginAbsolutePower);
      if(subblocksMsr[i].lowerOffsetMargin)
         free(subblocksMsr[i].lowerOffsetMargin);
      if(subblocksMsr[i].lowerOffsetMarginFrequency)
         free(subblocksMsr[i].lowerOffsetMarginFrequency);
      if(subblocksMsr[i].lowerOffsetMeasurementStatus)
         free(subblocksMsr[i].lowerOffsetMeasurementStatus);

      if(subblocksMsr[i].upperOffsetMarginRelativePower)
         free(subblocksMsr[i].upperOffsetMarginRelativePower);
      if(subblocksMsr[i].upperOffsetMarginAbsolutePower)
         free(subblocksMsr[i].upperOffsetMarginAbsolutePower);
      if(subblocksMsr[i].upperOffsetMargin)
         free(subblocksMsr[i].upperOffsetMargin);
      if(subblocksMsr[i].upperOffsetMarginFrequency)
         free(subblocksMsr[i].upperOffsetMarginFrequency);
      if(subblocksMsr[i].upperOffsetMeasurementStatus)
         free(subblocksMsr[i].upperOffsetMeasurementStatus);
   }

   printf("\n\nPress any key to exit\n");
   _getch();

   return errorOccured;
}
