//Steps:
//1. Open a new RFmx session
//2. Configure the basic instrument properties (Clock Source and Clock Frequency)
//3. Configure Selected Ports
//4. Configure instrument RF Attenuation
//5. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//6. Configure PhaseNoise measurement and enable the traces
//7. Configure Auto Range
//8. Configure Averaging Multiplier
//9. Configure Smoothing
//10. Initiate Measurement
//11. Fetch PhaseNoise Measurements and Traces
//12. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                       4096

int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];

   char *resourceName = "RFSA";

   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;                                                  /* Hz */
   int32 RFAttenuationAuto = RFMXINSTR_VAL_RF_ATTENUATION_AUTO_TRUE;
   float64 RFAttenuation = 10.00;                                                   /* dB */
   int32 autolevel = RFMXSPECAN_VAL_TRUE;
   float64 referenceLevel = 0.00;                                                   /* dBm */
   float64 externalAttenuation = 0.00;                                              /* dB */

   char *frequencyReferenceSource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e+6;                                   /* Hz */

   float64 timeout = 10.0;                                                          /* seconds */
   float64 bandwidth = 2.0e+5;                                                      /* Hz */
   float64 measurementInterval = 10e-3;                                             /* seconds */

   /* Auto Ranges */
   float64 startFrequency = 1e+3;                                                   /* Hz */
   float64 stopFrequency = 1e+6;                                                    /* Hz */
   float64 RBWPercentage = 10.00;                                                   /* % */

   /* Averaging Multiplier */
   int32 averagingMultiplier = 1;

   /* Smoothing */
   int32 smoothingType = RFMXSPECAN_VAL_PHASENOISE_SMOOTHING_TYPE_LOGARITHMIC;
   float64 smoothingPercentage = 2.00;                                              /* % */

   /* Variables to store the measurement results */
   float64 carrierFrequency = 0.0;                                                  /* Hz */
   float64 carrierPower = 0.0;                                                      /* dBm */
   float64 *integratedPhaseNoise = NULL;                                            /* dBc */
   float64 *residualPMInRadian = NULL;                                              /* rad */
   float64 *residualPMInDegree = NULL;                                              /* deg */
   float64 *residualFM = NULL;                                                      /* Hz */
   float64 *jitter = NULL;                                                          /* s */
   float32 *measuredFrequency = NULL;                                               /* Hz */
   float32 *measuredPhaseNoise = NULL;                                              /* dBc/Hz */
   float32 *smoothedFrequency = NULL;                                               /* Hz */
   float32 *smoothedPhaseNoise = NULL;                                              /* dBc/Hz */

   int32 actualArraySize = 0;

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

   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseCfgAutoRange(instrumentHandle, "", startFrequency, stopFrequency, RBWPercentage));

   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseCfgAveragingMultiplier(instrumentHandle, "", averagingMultiplier));
   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseCfgSmoothing(instrumentHandle, "", smoothingType, smoothingPercentage));

   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_PhaseNoiseFetchCarrierMeasurement(instrumentHandle, "", timeout, &carrierFrequency, &carrierPower));

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
         RFmxCheckWarn(RFmxSpecAn_PhaseNoiseFetchIntegratedNoise(instrumentHandle, "", timeout, integratedPhaseNoise, residualPMInRadian,
            residualPMInDegree, residualFM, jitter, integratedNoiseActualArraySize, NULL));
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
         RFmxCheckWarn(RFmxSpecAn_PhaseNoiseFetchMeasuredLogPlotTrace(instrumentHandle, "", timeout, measuredFrequency, measuredPhaseNoise,
            actualArraySize, NULL));
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
         RFmxCheckWarn(RFmxSpecAn_PhaseNoiseFetchSmoothedLogPlotTrace(instrumentHandle, "", timeout, smoothedFrequency, smoothedPhaseNoise,
            actualArraySize, NULL));
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

   printf("\nIntegrated Noise\n\n");

   printf("Integrated Phase Noise(dBc)      : %f\n", integratedPhaseNoise[0]);
   printf("Residual PM(rad)                 : %f\n", residualPMInRadian[0]);
   printf("Residual PM(deg)                 : %f\n", residualPMInDegree[0]);
   printf("Residual FM(Hz)                  : %f\n", residualFM[0]);
   printf("Jitter(s)                        : %.12g\n", jitter[0]);

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
