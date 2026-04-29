//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties(Center Frequency, RF Attenuation and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure Component Carriers.
//7. Configure Reference Level.
//8. Configure Duplex Mode.
//9. Configure Link Direction.
//10. Select ACP measurement and enable Traces.
//11. Configure Measurement Method.
//12. Configure Averaging Parameters for ACP measurement.
//13. Configure Sweep Time Parameters.
//14. Configure Noise Compensation Parameter.
//15. Initiate the Measurement.
//16. Fetch ACP Measurements and Traces.
//17. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096
#define NUMBER_OF_COMPONENT_CARRIERS            2
#define NUMBER_OF_OFFSETS                       3

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0;

   char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                                 /*(Hz) */

   float64 centerFrequency = 1.95e9;                                           /*(Hz) */
   float64 externalAttenuation = 0.0;                                          /*(dB) */

   int32 RFAttenuationAuto = RFMXLTE_VAL_RF_ATTENUATION_AUTO_TRUE;
   float64 RFAttenuation = 10.0;                                               /*(dB) */

   int32 enableTrigger = RFMXLTE_VAL_FALSE;
   char * digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
   int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                                 /*(s) */

   int32 componentCarrierSpacingType = RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL;
   int32 componentCarrierAtCenterFrequency = -1;

   int32 numberOfComponentCarriers = NUMBER_OF_COMPONENT_CARRIERS;
   float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS] = { 20e6, 20e6 };            /*(Hz) */
   float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS] = { -9.9e6, 9.9e6 };         /*(Hz) */

   int32 autoLevel = RFMXLTE_VAL_TRUE;
   float64 referenceLevel = 0.0;                                               /*(dBm) */

   float64 measurementInterval = 0.01;                                         /*(s) */

   int32 linkDirection = RFMXLTE_VAL_LINK_DIRECTION_UPLINK;

   int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_FDD;
   int32 uplinkDownlinkConfiguraiton = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;
   int32 measurementMethod = RFMXLTE_VAL_ACP_MEASUREMENT_METHOD_NORMAL;

   int32 averagingEnabled = RFMXLTE_VAL_ACP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXLTE_VAL_ACP_AVERAGING_TYPE_RMS;

   int32 sweepTimeAuto = RFMXLTE_VAL_ACP_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 0.001;                                          /*(s) */

   int32 noiseCompensationEnabled = RFMXLTE_VAL_ACP_NOISE_COMPENSATION_ENABLED_FALSE;

   float64 timeout = 10.0;                                                     /*(s) */

   float64 lowerAbsolutePower[NUMBER_OF_OFFSETS] = { 0 };                      /*(dBm) */
   float64 upperAbsolutePower[NUMBER_OF_OFFSETS] = { 0 };                      /*(dBm) */
   float64 lowerRelativePower[NUMBER_OF_OFFSETS] = { 0 };                      /*(dB) */
   float64 upperRelativePower[NUMBER_OF_OFFSETS] = { 0 };                      /*(dB) */

   float64 totalAggregatedPower = 0.0;                                         /*(dBm) */

   int32 actualArraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = NULL;                                                   /*(dBm) */
   float32 *absolutePowersTrace = NULL;                                        /*(dBm) */
   float32 *relativePowersTrace = NULL;                                        /*(dBm) */

   /* Initialize a session */
   RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxLTE_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxLTE_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxLTE_CfgRFAttenuation(instrumentHandle, "", RFAttenuationAuto, RFAttenuation));
   RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource,
      digitalEdge, triggerDelay, enableTrigger));

   RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, "",
      componentCarrierSpacingType,
      componentCarrierAtCenterFrequency));
   RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, "",
      numberOfComponentCarriers));
   RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, "",
      componentCarrierBandwidth,
      componentCarrierFrequency, NULL, numberOfComponentCarriers));
   if (autoLevel)
   {
      RFmxCheckWarn(RFmxLTE_AutoLevel(instrumentHandle, "", measurementInterval, &referenceLevel));
      printf("Reference level (dBm)           : %f\n", referenceLevel);
   }
   else
   {
      RFmxCheckWarn(RFmxLTE_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   }

   RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));
   RFmxCheckWarn(RFmxLTE_CfgLinkDirection(instrumentHandle, "", linkDirection));
   RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_ACP, RFMXLTE_VAL_TRUE));
   RFmxCheckWarn(RFmxLTE_ACPCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
   RFmxCheckWarn(RFmxLTE_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxLTE_ACPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxLTE_ACPCfgNoiseCompensationEnabled(instrumentHandle, "", noiseCompensationEnabled));
   RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */

   RFmxCheckWarn(RFmxLTE_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout,
      lowerRelativePower, upperRelativePower,
      lowerAbsolutePower, upperAbsolutePower,
      NUMBER_OF_OFFSETS, NULL));

   RFmxCheckWarn(RFmxLTE_ACPFetchTotalAggregatedPower(instrumentHandle, "", timeout, &totalAggregatedPower));

   for (i = 0; i < NUMBER_OF_OFFSETS; i++)
   {
      RFmxCheckWarn(RFmxLTE_ACPFetchAbsolutePowersTrace(instrumentHandle, "", timeout, i,
         NULL, NULL, NULL, 0, &actualArraySize));
      if (actualArraySize > 0)
      {
         absolutePowersTrace = (float32 *)malloc(sizeof(float32) * actualArraySize);
         if (absolutePowersTrace)
            RFmxCheckWarn(RFmxLTE_ACPFetchAbsolutePowersTrace(instrumentHandle, "", timeout, i,
               &x0, &dx, absolutePowersTrace, actualArraySize, NULL));
      }
   }

   actualArraySize = 0;
   for (i = 0; i < NUMBER_OF_OFFSETS; i++)
   {
      RFmxCheckWarn(RFmxLTE_ACPFetchRelativePowersTrace(instrumentHandle, "", timeout, i,
         NULL, NULL, NULL, 0, &actualArraySize));
      if (actualArraySize > 0)
      {
         relativePowersTrace = (float32 *)malloc(sizeof(float32) * actualArraySize);
         if (relativePowersTrace)
            RFmxCheckWarn(RFmxLTE_ACPFetchRelativePowersTrace(instrumentHandle, "", timeout,
               i, &x0, &dx, relativePowersTrace, actualArraySize, NULL));
      }
   }

   RFmxCheckWarn(RFmxLTE_ACPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
      if (spectrum)
      {
         RFmxCheckWarn(RFmxLTE_ACPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("\nTotal Aggregated Power (dBm)    : %f\n", totalAggregatedPower);

   printf("\n----------Offset Channel Measurements----------\n");
   for (i = 0; i < NUMBER_OF_OFFSETS; i++)
   {
      printf("Offset  : %d\n", i);
      printf("Lower Relative Power (dB)       : %f\n", lowerRelativePower[i]);
      printf("Upper Relative Power (dB)       : %f\n", upperRelativePower[i]);
      printf("Lower Absolute Power (dBm)      : %f\n", lowerAbsolutePower[i]);
      printf("Upper Absolute Power (dBm)      : %f\n", upperAbsolutePower[i]);
      printf("---------------------------------------------\n");
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
   if (spectrum)
   {
      free(spectrum);
   }
   if (relativePowersTrace)
   {
      free(relativePowersTrace);
   }
   if (absolutePowersTrace)
   {
      free(absolutePowersTrace);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
