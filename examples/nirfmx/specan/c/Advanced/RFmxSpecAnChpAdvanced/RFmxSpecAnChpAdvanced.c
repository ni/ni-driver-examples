//Steps:
//1. Open a new RFmx session
//2. Configure the instrument properties Clock Source and Clock Frequency
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select CHP measurement and enable the traces
//6. Configure CHP Integration BW, Span and Sweep Time
//7. Configure CHP Averaging
//8. Configure CHP RBW filter
//9. Configure CHP FFT
//10. Configure CHP RRC Filter
//11. Initiate Measurement
//12. Fetch CHP Measurements and Traces
//13. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

/* Maximum size of selector string */
#define MAX_SELECTOR_STRING         256

#define NUM_OF_CARRIERS             1

typedef struct
{
   float64 carrierFrequency;
   float64 integrationBandwidth;
   int32 RRCFilterEnabled;
   float64 RRCAlpha;

}carrierChannels_t;

typedef struct
{
   float64 absolutePower;
   float64 PSD;
   float64 relativePower;

}carrierMeasurement_t;

int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];
   int32 i = 0;
   char carrierString[MAX_SELECTOR_STRING] = { '\0' };

   char *resourceName = "RFSA";
   char *frequencySource = RFMXSPECAN_VAL_ONBOARD_CLOCK_STR;
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;        /* Hz */
   float64 referenceLevel = 0.00;         /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */
   float64 frequency = 10.0e+6;           /* Hz */

   float64 span = 1.0e+6;                 /* Hz */

      /* Sweep time settings */
   int32 sweepTimeAuto = RFMXSPECAN_VAL_CHP_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 1.00e-3;   /* seconds */

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_CHP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_CHP_AVERAGING_TYPE_RMS;

   /* RBW settings */
   int32 RBWFilterType = RFMXSPECAN_VAL_CHP_RBW_FILTER_TYPE_GAUSSIAN;
   int32 RBWAuto = RFMXSPECAN_VAL_CHP_RBW_AUTO_TRUE;
   float64 RBW = 10.0e+3;                 /* Hz */

   /* FFT */
   int32 FFTWindow = RFMXSPECAN_VAL_CHP_FFT_WINDOW_FLAT_TOP;
   float64 FFTPadding = -1.0;

   int32 numberOfCarriers = NUM_OF_CARRIERS;
   carrierChannels_t carrierChannels[NUM_OF_CARRIERS];

   float64 timeout = 10.0;
   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = (float32 *)NULL;
   int32 actualArraySize = 0;
   float64 totalCarrierPower = 0.0;
   carrierMeasurement_t carrierMeasurement[NUM_OF_CARRIERS];

   for (i = 0; i < numberOfCarriers; i++)
   {
      carrierChannels[i].carrierFrequency = 0.00;        /* Hz */
      carrierChannels[i].integrationBandwidth = 1.0e+6;  /* Hz */

      /* RRC filter */
      carrierChannels[i].RRCFilterEnabled = RFMXSPECAN_VAL_CHP_RRC_FILTER_ENABLED_FALSE;
      carrierChannels[i].RRCAlpha = 0.220;
   }

   for (i = 0; i < numberOfCarriers; i++)
   {
      carrierMeasurement[i].absolutePower = 0.0;
      carrierMeasurement[i].PSD = 0.0;
      carrierMeasurement[i].relativePower = 0.0;
   }

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure CHP parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_CHP, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_CHPCfgSpan(instrumentHandle, "", span));
   RFmxCheckWarn(RFmxSpecAn_CHPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxSpecAn_CHPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount,
      averagingType));
   RFmxCheckWarn(RFmxSpecAn_CHPCfgRBWFilter(instrumentHandle, "", RBWAuto, RBW, RBWFilterType));
   RFmxCheckWarn(RFmxSpecAn_CHPCfgFFT(instrumentHandle, "", FFTWindow, FFTPadding));
   RFmxCheckWarn(RFmxSpecAn_CHPCfgNumberOfCarriers(instrumentHandle, "", numberOfCarriers));

   for (i = 0; i < numberOfCarriers; i++)
   {
      RFmxSpecAn_BuildCarrierString2("", i, MAX_SELECTOR_STRING, carrierString);
      RFmxCheckWarn(RFmxSpecAn_CHPCfgCarrierOffset(instrumentHandle, carrierString, carrierChannels[i].carrierFrequency));
      RFmxCheckWarn(RFmxSpecAn_CHPCfgIntegrationBandwidth(instrumentHandle, carrierString,
         carrierChannels[i].integrationBandwidth));
      RFmxCheckWarn(RFmxSpecAn_CHPCfgRRCFilter(instrumentHandle, carrierString, carrierChannels[i].RRCFilterEnabled,
         carrierChannels[i].RRCAlpha));
   }
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_CHPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32)*actualArraySize);

      if (spectrum)
      {
         RFmxCheckWarn(RFmxSpecAn_CHPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxSpecAn_CHPFetchTotalCarrierPower(instrumentHandle, "", timeout, &totalCarrierPower));

   for (i = 0; i < numberOfCarriers; i++)
   {
      RFmxSpecAn_BuildCarrierString2("", i, MAX_SELECTOR_STRING, carrierString);
      RFmxCheckWarn(RFmxSpecAn_CHPFetchCarrierMeasurement(instrumentHandle, carrierString, timeout,
         &carrierMeasurement[i].absolutePower, &carrierMeasurement[i].PSD, &carrierMeasurement[i].relativePower));
   }

   printf("Total Carrier Power (dBm)   %f\n", totalCarrierPower);
   printf("\nCarrier Measurements\n");
   for (i = 0; i < numberOfCarriers; i++)
   {
      printf("\nCarrier : %d\n", i);
      printf("Absolute Power (dBm)        %f\n", carrierMeasurement[i].absolutePower);
      printf("PSD (dBm/Hz)                %f\n", carrierMeasurement[i].PSD);
      printf("Relative Power (dB)         %f\n", carrierMeasurement[i].relativePower);
      printf("---------------------------------------------------\n");
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
   printf("Press any key to exit\n");
   _getch();
   return error;
}
