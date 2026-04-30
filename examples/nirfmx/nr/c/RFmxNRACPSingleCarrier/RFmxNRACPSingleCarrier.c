//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, RF Attenuation and External Attenuation).
//5. Configure Trigger Parameters for IQ Power Edge Trigger.
//6. Configure Link Direction, Frequency Range and Carrier Bandwidth and Subcarrier Spacing.
//7. Configure Reference Level.
//8. Select ACP measurement and enable Traces.
//9. Configure Measurement Method.
//10. Configure Noise Compensation Parameter.
//11. Configure Sweep Time Parameters.
//12. Configure Averaging Parameters for ACP measurement.
//13. Initiate the Measurement.
//14. Fetch ACP Measurements and Traces.
//15. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxNR.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096


int main(int argc, char *argv[])
{
   char* resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0;

   char* selectedPorts = "";
   float64 centerFrequency = 3.5e9;                                              /* (Hz) */
   float64 externalAttenuation = 0.0;                                            /* (dB) */

   int32 autoLevel = RFMXNR_VAL_TRUE;
   float64 referenceLevel = 0.0;                                                 /* (dBm) */
   float64 measurementInterval = 10.0e-3;                                        /* (s) */

   int32 RFAttenuationAuto = RFMXNR_VAL_RF_ATTENUATION_AUTO_TRUE;
   float64 RFAttenuation = 10.0;                                                 /* (dB) */

   char* frequencyReferenceSource = RFMXNR_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e6;                                 /* (Hz) */

   int32 IQPowerEdgeEnabled = RFMXNR_VAL_FALSE;
   float64 IQPowerEdgeLevel = -20.0;                                             /* (dB or dBm) */
   float64 triggerDelay = 0.0;                                                   /* (s) */
   int32 minimumQuietTimeMode = RFMXNR_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 8.0e-6;                                            /* (s) */

   int32 linkDirection = RFMXNR_VAL_LINK_DIRECTION_UPLINK;
   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;
   float64 carrierBandwidth = 100e6;                                             /* (Hz) */
   float64 subcarrierSpacing = 30e3;                                             /* (Hz) */
   int32 measurementMethod = RFMXNR_VAL_ACP_MEASUREMENT_METHOD_NORMAL;
   int32 noiseCompensationEnabled = RFMXNR_VAL_ACP_NOISE_COMPENSATION_ENABLED_FALSE;

   int32 sweepTimeAuto = RFMXNR_VAL_ACP_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 1.0e-3;                                           /* (s) */

   int32 averagingEnabled = RFMXNR_VAL_ACP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXNR_VAL_ACP_AVERAGING_TYPE_RMS;

   float64 timeout = 10.0;                                                       /* (s) */

   float64 absolutePower = 0.0;                                                  /* (dBm) */
   float64 relativePower = 0.0;                                                  /* (dB) */

   float64* lowerRelativePower = NULL;                                           /* (dB) */
   float64* upperRelativePower = NULL;                                           /* (dB) */
   float64* lowerAbsolutePower = NULL;                                           /* (dBm) */
   float64* upperAbsolutePower = NULL;                                           /* (dBm) */

   int32 actualArraySize = 0, arraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32* relativePowersTrace = NULL;
   float32* spectrum = NULL;

   /* Initialize a session */
   RFmxCheckWarn(RFmxNR_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxNR_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxNR_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxNR_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxNR_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxNR_CfgRFAttenuation(instrumentHandle, "", RFAttenuationAuto, RFAttenuation));
   RFmxCheckWarn(RFmxNR_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXNR_VAL_IQ_POWER_EDGE_RISING_SLOPE,
      IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
      RFMXNR_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, IQPowerEdgeEnabled));
   RFmxCheckWarn(RFmxNR_SetLinkDirection(instrumentHandle, "", linkDirection));
   RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, "", frequencyRange));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, "", carrierBandwidth));
   RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, "", subcarrierSpacing));
   if (autoLevel)
   {
      RFmxCheckWarn(RFmxNR_AutoLevel(instrumentHandle, "", measurementInterval, &referenceLevel));
      printf("Reference level (dBm)        : %f\n", referenceLevel);
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
   RFmxCheckWarn(RFmxNR_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, NULL,
      NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      lowerRelativePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperRelativePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerAbsolutePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperAbsolutePower = (float64 *)malloc(sizeof(float64) * actualArraySize);

      RFmxCheckWarn(RFmxNR_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, lowerRelativePower,
         upperRelativePower, lowerAbsolutePower, upperAbsolutePower, actualArraySize, &arraySize));
   }

   RFmxCheckWarn(RFmxNR_ACPFetchComponentCarrierMeasurement(instrumentHandle, "", timeout, &absolutePower,
      &relativePower));

   for (i = 0; i < arraySize; i++)
   {
      actualArraySize = 0;
      RFmxCheckWarn(RFmxNR_ACPFetchRelativePowersTrace(instrumentHandle, "", timeout, i,
         NULL, NULL, NULL, 0, &actualArraySize));
      if (actualArraySize > 0)
      {
         relativePowersTrace = (float32 *)malloc(sizeof(float32) * actualArraySize);
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

   printf("\nCarrier Absolute Power (dBm or dBm/Hz) : %f\n", absolutePower);

   printf("\n-----------Offset Channel Measurements----------- \n\n");
   for (i = 0; i < arraySize; i++)
   {
      printf("Offset  %d\n", i);
      printf("Lower Relative Power (dB)              : %f\n", lowerRelativePower[i]);
      printf("Upper Relative Power (dB)              : %f\n", upperRelativePower[i]);
      printf("Lower Absolute Power (dBm or dBm/Hz)   : %f\n", lowerAbsolutePower[i]);
      printf("Upper Absolute Power (dBm or dBm/Hz)   : %f\n", upperAbsolutePower[i]);
      printf("-------------------------------------------------\n\n");
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
   if (lowerRelativePower) 
   {
      free(lowerRelativePower);
   }
   if (upperRelativePower) 
   {
      free(upperRelativePower);
   }
   if (lowerAbsolutePower) 
   {
      free(lowerAbsolutePower);
   }
   if (upperAbsolutePower) 
   {
      free(upperAbsolutePower);
   }
   if (relativePowersTrace) 
   {
      free(relativePowersTrace);
   }
   if (spectrum) 
   {
      free(spectrum);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
