//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties (Clock Source and Clock Frequency)
//3. Configure Selected Ports
//4. Configure instrument RF Attenuation
//5. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//6. Configure PhaseNoise measurement and enable the traces
//7. Configure Range Definition
//8. Configure Auto Range
//9. Configure Number of Ranges
//10. Configure Range(Array)
//11. Configure Averaging Multiplier
//12. Configure Smoothing
//13. Configure Spot Noise Frequency List
//14. Configure Integrated Noise
//15. Configure Spur Removal
//16. Configure Cancellation
//17. Initiate Measurement
//18. Fetch PhaseNoise Measurements and Traces
//19. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                       4096
#define NUMBER_OF_RANGES                            1

int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];

   char *resourceName = "RFSA";

   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;                                               /* Hz */
   int32 RFAttenuationAuto = RFMXINSTR_VAL_RF_ATTENUATION_AUTO_TRUE;
   float64 RFAttenuation = 10.00;                                                /* dB */
   int32 autolevel = RFMXSPECAN_VAL_TRUE;
   float64 referenceLevel = 0.00;                                                /* dBm */
   float64 externalAttenuation = 0.00;                                           /* dB */

   char *frequencyReferenceSource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e+6;                                /* Hz */

   float64 timeout = 10.0;                                                       /* seconds */
   float64 bandwidth = 2.0e+5;                                                   /* Hz */
   float64 measurementInterval = 10e-3;                                          /* seconds */

   int32 rangeDefinition = RFMXSPECAN_VAL_PHASENOISE_RANGE_DEFINITION_AUTO;
   int32 numberOfRanges = NUMBER_OF_RANGES;

   /* Auto Ranges */
   float64 startFrequency = 1e+3;                                                /* Hz */
   float64 stopFrequency = 1e+6;                                                 /* Hz */
   float64 RBWPercentage = 10.00;                                                /* % */

   /* Manual Ranges */
   float64 rangeStartFrequency[NUMBER_OF_RANGES] = { 1.0e+3 };                   /* Hz */
   float64 rangeStopFrequency[NUMBER_OF_RANGES] = { 1.0e+6 };                    /* Hz */
   float64 rangeRBWPercentage[NUMBER_OF_RANGES] = { 10.00 };                     /* % */
   int32 rangeAveragingCount[NUMBER_OF_RANGES] = { 10 };

   /* Averaging Multiplier */
   int32 averagingMultiplier = 1;
   int32 i = 0;

   /* Smoothing */
   int32 smoothingType = RFMXSPECAN_VAL_PHASENOISE_SMOOTHING_TYPE_LOGARITHMIC;
   float64 smoothingPercentage = 2.00;                                           /* % */

   /* Spot Noise Frequency List */
   float64 *frequencyList = NULL;                                                /* Hz */
   int32 spotNoiseFrequencyListSize = 0;

   /* Integrated Noise */
   int32 integratedNoiseRangeDefinition = RFMXSPECAN_VAL_PHASENOISE_INTEGRATED_NOISE_RANGE_DEFINITION_MEASUREMENT;

   /* Custom Ranges */
   float64 *integratedNoiseStartFrequency = NULL;                                /* Hz */
   float64 *integratedNoiseStopFrequency = NULL;                                 /* Hz */
   int32 numberOfCustomRanges = 0;

   /* Spur Removal */
   int32 spurRemovalEnabled = RFMXSPECAN_VAL_PHASENOISE_SPUR_REMOVAL_ENABLED_FALSE;
   float64 peakExcursion = 6.00;                                                 /* dB */

   /* Cancellation */
   int32 cancellationEnabled = RFMXSPECAN_VAL_PHASENOISE_CANCELLATION_ENABLED_FALSE;
   float64 cancellationThreshold = 0.01;                                         /* dB */
   float32 *cancellationFrequency = NULL;                                        /* Hz */
   float32 *cancellationReferencePhaseNoise = NULL;                              /* dBc/Hz */
   int32 cancellationReferencePhaseNoiseSize = 0;

   /* Variables to store the measurement results */
   float64 carrierFrequency = 0.0;                                               /* Hz */
   float64 carrierPower = 0.0;                                                   /* dBm */
   float64 *spotPhaseNoise = NULL;                                               /* dBc/Hz */
   float64 *integratedPhaseNoise = NULL;                                         /* dBc*/
   float64 *residualPMInRadian = NULL;                                           /* rad*/
   float64 *residualPMInDegree = NULL;                                           /* deg*/
   float64 *residualFM = NULL;                                                   /* Hz */
   float64 *jitter = NULL;                                                       /* s */
   float32 *measuredFrequency = NULL;                                            /* Hz */
   float32 *measuredPhaseNoise = NULL;                                           /* dBc/Hz */
   float32 *smoothedFrequency = NULL;                                            /* Hz */
   float32 *smoothedPhaseNoise = NULL;                                           /* dBc/Hz */

   int32 actualArraySize = 0;
   int32 spotNoiseActualArraySize = 0;
   int32 integratedNoiseActualArraySize = 0;

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure Phase Noise parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRFAttenuation(instrumentHandle, "", RFAttenuationAuto, RFAttenuation));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));

   if (autolevel)
   {
      RFmxCheckWarn(RFmxSpecAn_AutoLevel(instrumentHandle, "", bandwidth, measurementInterval, &referenceLevel));
      printf("Reference level (dBm)            : %f\n", referenceLevel);
   }
   else
   {
      RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   }

   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_PHASENOISE, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseCfgRangeDefinition(instrumentHandle, "", rangeDefinition));
   if (rangeDefinition == RFMXSPECAN_VAL_PHASENOISE_RANGE_DEFINITION_AUTO)
   {
      RFmxCheckWarn(RFmxSpecAn_PhaseNoiseCfgAutoRange(instrumentHandle, "", startFrequency, stopFrequency, RBWPercentage));
   }
   else
   {
      RFmxCheckWarn(RFmxSpecAn_PhaseNoiseCfgNumberOfRanges(instrumentHandle, "", numberOfRanges));
      RFmxCheckWarn(RFmxSpecAn_PhaseNoiseCfgRangeArray(instrumentHandle, "", rangeStartFrequency, rangeStopFrequency,
         rangeRBWPercentage, rangeAveragingCount, numberOfRanges));
   }
   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseCfgAveragingMultiplier(instrumentHandle, "", averagingMultiplier));
   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseCfgSmoothing(instrumentHandle, "", smoothingType, smoothingPercentage));
   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseCfgSpotNoiseFrequencyList(instrumentHandle, "", frequencyList, spotNoiseFrequencyListSize));
   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseCfgIntegratedNoise(instrumentHandle, "", integratedNoiseRangeDefinition,
      integratedNoiseStartFrequency, integratedNoiseStopFrequency, numberOfCustomRanges));
   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseCfgSpurRemoval(instrumentHandle, "", spurRemovalEnabled, peakExcursion));
   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseCfgCancellation(instrumentHandle, "", cancellationEnabled, cancellationThreshold,
      cancellationFrequency, cancellationReferencePhaseNoise, cancellationReferencePhaseNoiseSize));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseFetchCarrierMeasurement(instrumentHandle, "", timeout, &carrierFrequency, &carrierPower));

   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseFetchSpotNoise(instrumentHandle, "", timeout, NULL, 0, &spotNoiseActualArraySize));
   if (spotNoiseActualArraySize > 0)
   {
      spotPhaseNoise = (float64 *)malloc(sizeof(float64)*spotNoiseActualArraySize);
      if (spotPhaseNoise)
      {
         RFmxCheckWarn(RFmxSpecAn_PhaseNoiseFetchSpotNoise(instrumentHandle, "", timeout, spotPhaseNoise,
            spotNoiseActualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseFetchIntegratedNoise(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL,
      0, &integratedNoiseActualArraySize));
   if (integratedNoiseActualArraySize > 0)
   {
      integratedPhaseNoise = (float64 *)malloc(sizeof(float64)*integratedNoiseActualArraySize);
      residualPMInRadian = (float64 *)malloc(sizeof(float64)*integratedNoiseActualArraySize);
      residualPMInDegree = (float64 *)malloc(sizeof(float64)*integratedNoiseActualArraySize);
      residualFM = (float64 *)malloc(sizeof(float64)*integratedNoiseActualArraySize);
      jitter = (float64 *)malloc(sizeof(float64)*integratedNoiseActualArraySize);

      if (integratedPhaseNoise && residualPMInRadian && residualPMInDegree && residualFM && jitter)
      {
         RFmxCheckWarn(RFmxSpecAn_PhaseNoiseFetchIntegratedNoise(instrumentHandle, "", timeout, integratedPhaseNoise,
            residualPMInRadian, residualPMInDegree, residualFM, jitter, integratedNoiseActualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseFetchMeasuredLogPlotTrace(instrumentHandle, "", timeout, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      measuredFrequency = (float32 *)malloc(sizeof(float32)*actualArraySize);
      measuredPhaseNoise = (float32 *)malloc(sizeof(float32)*actualArraySize);

      if (measuredFrequency && measuredPhaseNoise)
      {
         RFmxCheckWarn(RFmxSpecAn_PhaseNoiseFetchMeasuredLogPlotTrace(instrumentHandle, "", timeout, measuredFrequency,
            measuredPhaseNoise, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseFetchSmoothedLogPlotTrace(instrumentHandle, "", timeout, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      smoothedFrequency = (float32 *)malloc(sizeof(float32)*actualArraySize);
      smoothedPhaseNoise = (float32 *)malloc(sizeof(float32)*actualArraySize);

      if (smoothedFrequency && smoothedPhaseNoise)
      {
         RFmxCheckWarn(RFmxSpecAn_PhaseNoiseFetchSmoothedLogPlotTrace(instrumentHandle, "", timeout, smoothedFrequency,
            smoothedPhaseNoise, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   /* Display results */
   printf("\nCarrier Measurement\n\n");
   printf("Carrier Frequency(Hz)            : %f\n", carrierFrequency);
   printf("Carrier Power(dBm)               : %f\n", carrierPower);

   if (spotNoiseActualArraySize)
      printf("\nSpot Phase Noise(dBc/Hz)\n\n");
   for (i = 0; i < spotNoiseActualArraySize; i++)
   {
      printf("Spot Phase Noise Index %d         : %f\n", i, spotPhaseNoise[i]);
   }

   printf("\nIntegrated Noise\n\n");
   for (i = 0; i < integratedNoiseActualArraySize; i++)
   {
      printf("Integrated  Noise Range %d\n", i);
      printf("Integrated Phase Noise(dBc)      : %f\n", integratedPhaseNoise[i]);
      printf("Residual PM(rad)                 : %f\n", residualPMInRadian[i]);
      printf("Residual PM(deg)                 : %f\n", residualPMInDegree[i]);
      printf("Residual FM(Hz)                  : %f\n", residualFM[i]);
      printf("Jitter(s)                        : %.12g\n", jitter[i]);
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
   if (spotPhaseNoise)
      free(spotPhaseNoise);
   if (integratedPhaseNoise)
      free(integratedPhaseNoise);
   if (residualPMInRadian)
      free(residualPMInRadian);
   if (residualPMInDegree)
      free(residualPMInDegree);
   if (residualFM)
      free(residualFM);
   if (jitter)
      free(jitter);
   if (measuredFrequency)
      free(measuredFrequency);
   if (measuredPhaseNoise)
      free(measuredPhaseNoise);
   if (smoothedFrequency)
      free(smoothedFrequency);
   if (smoothedPhaseNoise)
      free(smoothedPhaseNoise);

   printf("Press any key to exit\n");
   _getch();

   return error;
}
