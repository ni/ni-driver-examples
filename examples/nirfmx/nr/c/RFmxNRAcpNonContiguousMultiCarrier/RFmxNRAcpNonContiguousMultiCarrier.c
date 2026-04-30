//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, RF Attenuation and External Attenuation).
//5. Configure Trigger Parameters for Digital Edge Trigger.
//6. Configure Link Direction and Number of Subblocks.
//7. Configure Frequency Range, Subblock Frequency, Component Carrier Spacing Type,
//   Channel Raster, Component Carrier Center Frequencyand Number of Component Carriers.
//8. Configure Subcarrier Spacing.
//9. Configure Component Carriers.
//10. Configure Reference Level.
//11. Select ACP measurement and enable Traces.
//12. Configure Measurement Method.
//13. Configure Noise Compensation Parameter.
//14. Configure Sweep Time Parameters.
//15. Configure Averaging Parameters for ACP measurement.
//16. Initiate the Measurement.
//17. Fetch ACP Measurements and Traces.
//18. Close RFmx Session.

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

/* Subblock measurement outputs structure */
typedef struct
{
    float64 subblockPower;                                  /*(dBm) */
    float64 integrationBandwidth;                           /*(Hz) */
    float64 frequency;                                      /*(Hz) */
    int32 numberOfOffsets;
    float64* lowerAbsolutePower;                            /*(dBm) */
    float64* upperAbsolutePower;                            /*(dBm) */
    float64* lowerRelativePower;                            /*(dB) */
    float64* upperRelativePower;                            /*(dB) */
}subblockMeasurement_t;

int main(int argc, char *argv[])
{
   char* resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 autoLevel = RFMXNR_VAL_TRUE;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0, j = 0;

   char subblockString[MAX_SELECTOR_STRING];
   char carrierString[MAX_SELECTOR_STRING];

   char* selectedPorts = "";
   float64 centerFrequency = 3.5e9;                                                          /* (Hz) */
   float64 referenceLevel = 0.0;                                                             /* (dBm) */
   float64 externalAttenuation = 0.0;                                                        /* (dB) */

   int32 RFAttenuationAuto = RFMXNR_VAL_RF_ATTENUATION_AUTO_TRUE;
   float64 RFAttenuation = 10.0;                                                             /* (dB) */

   char* frequencyReferenceSource = RFMXNR_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e6;                                             /* (Hz) */

   int32 enableTrigger = RFMXNR_VAL_FALSE;
   char* digitalEdgeSource = RFMXNR_VAL_PXI_TRIG0_STR;
   int32 digitalEdge = RFMXNR_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                                               /* (s) */

   int32 linkDirection = RFMXNR_VAL_LINK_DIRECTION_UPLINK;
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

   int32 measurementMethod = RFMXNR_VAL_ACP_MEASUREMENT_METHOD_NORMAL;

   int32 noiseCompensationEnabled = RFMXNR_VAL_ACP_NOISE_COMPENSATION_ENABLED_FALSE;

   int32 sweepTimeAuto = RFMXNR_VAL_ACP_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 0.001;                                                        /* (s) */

   int32 averagingEnabled = RFMXNR_VAL_ACP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXNR_VAL_ACP_AVERAGING_TYPE_RMS;

   subblockMeasurement_t subblocksMsr[NUMBER_OF_SUBBLOCKS];

   float64 totalAggregatedPower = 0.0;                                                       /* (dBm or dBm/Hz) */

   int32 actualArraySize = 0, arraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32* spectrum = NULL;                                                                 /* (dBm) */
   float64 timeout = 10.000000;                                                              /* (s) */
   float32* relativePowersTrace = NULL;

   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
       subblocksMsr[i].lowerAbsolutePower = NULL;
       subblocksMsr[i].upperAbsolutePower = NULL;
       subblocksMsr[i].lowerRelativePower = NULL;
       subblocksMsr[i].upperRelativePower = NULL;
   }
 
   /* Initialize a session */
   RFmxCheckWarn(RFmxNR_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxNR_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxNR_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxNR_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxNR_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxNR_CfgRFAttenuation(instrumentHandle, "", RFAttenuationAuto, RFAttenuation));

   RFmxCheckWarn(RFmxNR_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay,
      enableTrigger));
   RFmxCheckWarn(RFmxNR_SetLinkDirection(instrumentHandle, "", RFMXNR_VAL_LINK_DIRECTION_UPLINK));
   RFmxCheckWarn(RFmxNR_SetNumberOfSubblocks(instrumentHandle, "", NUMBER_OF_SUBBLOCKS));

   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
      RFmxNR_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString);
      RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, subblockString, frequencyRange));
      RFmxCheckWarn(RFmxNR_SetSubblockFrequency(instrumentHandle, subblockString, subblockFrequency[i]));
      RFmxCheckWarn(RFmxNR_SetComponentCarrierSpacingType(instrumentHandle, subblockString, componentCarrierSpacingType[i]));
      RFmxCheckWarn(RFmxNR_SetChannelRaster(instrumentHandle, subblockString, channelRaster[i]));
      RFmxCheckWarn(RFmxNR_SetComponentCarrierAtCenterFrequency(instrumentHandle, subblockString, componentCarrierAtCenterFrequency[i]));
      RFmxCheckWarn(RFmxNR_SetNumberOfComponentCarriers(instrumentHandle, subblockString, NUMBER_OF_COMPONENT_CARRIERS));
      
      RFmxNR_BuildCarrierString(subblockString, -1, MAX_SELECTOR_STRING, carrierString);
      RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, carrierString, subcarrierSpacing));

      for (j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++)
      {
         RFmxNR_BuildCarrierString(subblockString, j, MAX_SELECTOR_STRING, carrierString);
         RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, carrierString, componentCarrierBandwidth[i][j]));
         RFmxCheckWarn(RFmxNR_SetComponentCarrierFrequency(instrumentHandle, carrierString, componentCarrierFrequency[i][j]));
      }
   }

   if (autoLevel) 
   {
       RFmxCheckWarn(RFmxNR_AutoLevel(instrumentHandle, "", 10.0e-3, &referenceLevel));
       printf("Reference level (dBm)           : %f\n", referenceLevel);
   }
   else
   {
       RFmxCheckWarn(RFmxNR_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   }

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_ACP, RFMXNR_VAL_TRUE));
   RFmxCheckWarn(RFmxNR_ACPCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
   RFmxCheckWarn(RFmxNR_ACPCfgNoiseCompensationEnabled(instrumentHandle, "", noiseCompensationEnabled));
   RFmxCheckWarn(RFmxNR_ACPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxNR_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

   
   /* Fetch results */
   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
       RFmxNR_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString);
       RFmxCheckWarn(RFmxNR_ACPFetchSubblockMeasurement(instrumentHandle, subblockString, timeout,
           &subblocksMsr[i].subblockPower,
           &subblocksMsr[i].integrationBandwidth,
           &subblocksMsr[i].frequency));
       
       RFmxCheckWarn(RFmxNR_ACPFetchOffsetMeasurementArray(instrumentHandle, subblockString, timeout,
           NULL, NULL, NULL, NULL, 0,
           &subblocksMsr[i].numberOfOffsets));
       if (subblocksMsr[i].numberOfOffsets > 0)
       {
           subblocksMsr[i].lowerRelativePower = (float64*)malloc(sizeof(float64) * subblocksMsr[i].numberOfOffsets);
           subblocksMsr[i].upperRelativePower = (float64*)malloc(sizeof(float64) * subblocksMsr[i].numberOfOffsets);
           subblocksMsr[i].lowerAbsolutePower = (float64*)malloc(sizeof(float64) * subblocksMsr[i].numberOfOffsets);
           subblocksMsr[i].upperAbsolutePower = (float64*)malloc(sizeof(float64) * subblocksMsr[i].numberOfOffsets);
           
           if (subblocksMsr[i].lowerRelativePower && subblocksMsr[i].upperRelativePower &&
               subblocksMsr[i].lowerAbsolutePower && subblocksMsr[i].upperAbsolutePower)
           {
               RFmxCheckWarn(RFmxNR_ACPFetchOffsetMeasurementArray(instrumentHandle, subblockString, timeout,
                   subblocksMsr[i].lowerRelativePower,
                   subblocksMsr[i].upperRelativePower,
                   subblocksMsr[i].lowerAbsolutePower,
                   subblocksMsr[i].upperAbsolutePower,
                   subblocksMsr[i].numberOfOffsets, &arraySize));
           }
           else
           {
               printf("malloc failed.\n");
               goto Error;
           }
       }
   }
   RFmxCheckWarn(RFmxNR_ACPFetchTotalAggregatedPower(instrumentHandle, "", timeout, &totalAggregatedPower));

   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
       actualArraySize = 0;
       RFmxCheckWarn(RFmxNR_ACPFetchRelativePowersTrace(instrumentHandle, "", timeout, i,
           NULL, NULL, NULL, 0, &actualArraySize));
       if (actualArraySize > 0)
       {
           relativePowersTrace = (float32*)malloc(sizeof(float32) * actualArraySize);
           if (relativePowersTrace)
               RFmxCheckWarn(RFmxNR_ACPFetchRelativePowersTrace(instrumentHandle, "", timeout,
                   i, &x0, &dx, relativePowersTrace, actualArraySize, NULL));
       }
   }
   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_ACPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
       spectrum = (float32*)malloc(sizeof(float32) * actualArraySize);
       if (spectrum)
       {
           RFmxCheckWarn(RFmxNR_ACPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum,
               actualArraySize, NULL));
       }
       else
       {
           printf("malloc failed.\n");
           goto Error;
       }
   }
   
   printf("------------------------Measurement------------------------\n\n");

   printf("\nTotal Aggregated Power (dBm or dBm/Hz)    : %f\n", totalAggregatedPower);

   printf("\n****************Subblock Measurements****************\n");
   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
        printf("\nSubblock                                       : %d\n", i);
        printf("Subblock Power (dBm or dBm/Hz)                 : %lf\n", subblocksMsr[i].subblockPower);
        printf("Integration Bandwidth (Hz)                     : %lf\n", subblocksMsr[i].integrationBandwidth);
        printf("Frequency (Hz)                                 : %lf\n", subblocksMsr[i].frequency);
        for (j = 0; j < subblocksMsr[i].numberOfOffsets; j++) 
        {
            printf("\nOffset Number                                  : %d\n", j);
            printf("Lower Relative Power (dB)                      : %lf\n", subblocksMsr[i].lowerRelativePower[j]);
            printf("Upper Relative Power (dB)                      : %lf\n", subblocksMsr[i].upperRelativePower[j]);
            printf("Lower Absolute Power (dBm)                     : %lf\n", subblocksMsr[i].lowerAbsolutePower[j]);
            printf("Upper Absolute Power (dBm)                     : %lf\n", subblocksMsr[i].upperAbsolutePower[j]);
        }     
        printf("---------------------------------------------------------------\n\n");
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
   if (spectrum)
   {
       free(spectrum);
   }
   for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
   {
       if (subblocksMsr[i].lowerRelativePower)
       {
         free(subblocksMsr[i].lowerRelativePower);
       }
       if (subblocksMsr[i].upperRelativePower)
       {
         free(subblocksMsr[i].upperRelativePower);
       }
       if (subblocksMsr[i].lowerAbsolutePower)
       {
         free(subblocksMsr[i].lowerAbsolutePower);
       }
       if (subblocksMsr[i].upperAbsolutePower)
       {
         free(subblocksMsr[i].upperAbsolutePower);
       }
   }
   if (relativePowersTrace)
   {
       free(relativePowersTrace);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}