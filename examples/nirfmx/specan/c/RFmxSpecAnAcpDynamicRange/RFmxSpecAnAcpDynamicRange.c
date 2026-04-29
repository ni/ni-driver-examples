//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure Frequency Reference
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Configure RF Attenuation
//6. Select ACP measurement and enable the traces
//7. Configure ACP Measurement Method, Power Units and Averaging Parameters
//8. Configure ACP RBW filter
//9. Configure ACP Sweep Time
//10. Configure ACP Noise Compensation
//11. Configure ACP Carrier Channel Settings (Integration BW, RRC Filter)
//12. Configure ACP Number of Offset Channels
//13. Configure ACP Offset Channel Settings (Offset Frequencies, Integration BW, RRC Filter)
//Use "offset:all" selector string to set a parameter for all the Offset Channels
//14. Initiate Measurement
//15. Fetch ACP Measurements and Traces
//16. Close the RFmx Session

// Include files

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

#define NUMBER_OF_OFFSETS           2
#define AUTO_LEVEL_OFF              0
#define AUTO_LEVEL_ON               1

/* Datatype for input Values */
typedef struct {
   float64 integrationBandwidth;
   int32 RRCFilterEnabled;
   float64 RRCFilterAlpha;
}carrierChannel_t;

typedef struct {
   float64 integrationBandwidth;
   float64 frequencyOffset[NUMBER_OF_OFFSETS];
   int32 RRCFilterEnabled;
   float64 RRCFilterAlpha;
}offsetChannel_t;

int main(int argc, char *argv[])
{
   int i = 0;
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION];
   int32 error = 0, lastErrorCode = 0;

   char *resourceName = "RFSA";
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;           /* Hz */
   float64 autoSetReferenceLevel;
   float64 referenceLevel = 0.00;            /* dBm */
   float64 externalAttenuation = 0.00;       /* dB */
   float64 integrationBandwidth = 1e+6;
   float64 measurementInterval = 10e-3;
   int32 autoLevel = AUTO_LEVEL_ON;

   int32 rfAttenuationAuto = RFMXSPECAN_VAL_TRUE;
   float64 rfAttenuation = 10.00;            /* dB */

   char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   float64 frequency = 10.0e+6;              /* Hz */

   int32 powerUnits = RFMXSPECAN_VAL_ACP_POWER_UNITS_DBM;
   int32 measurementMethod = RFMXSPECAN_VAL_ACP_MEASUREMENT_METHOD_DYNAMIC_RANGE;
   int32 noiseCompensationEnabled = RFMXSPECAN_VAL_ACP_NOISE_COMPENSATION_ENABLED_TRUE;

   /* Sweep Time */
   int32 sweepTimeAuto = RFMXSPECAN_VAL_ACP_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 1.00e-3;      /* seconds */

   /* RBW Filter */
   int32 RBWFilterType = RFMXSPECAN_VAL_ACP_RBW_FILTER_TYPE_GAUSSIAN;
   int32 RBWAuto = RFMXSPECAN_VAL_ACP_RBW_AUTO_TRUE;
   float64 RBW = 10.0e+3;                    /* Hz */

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_ACP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_ACP_AVERAGING_TYPE_RMS;

   float64 timeout = 10.0;                   /* seconds */
   int32 numberOfOffsets = NUMBER_OF_OFFSETS;

   /* Variables to store the results */
   float64 absolutePower;
   int32 offsetMeasArraySize;
   float64 *lowerRelativePower = (float64 *)NULL;
   float64 *upperRelativePower = (float64 *)NULL;
   float64 *lowerAbsolutePower = (float64 *)NULL;
   float64 *upperAbsolutePower = (float64 *)NULL;
   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = (float32 *)NULL;
   int32 actualArraySize;

   /* Carrier Channels */
   carrierChannel_t carrierChannelInput;
   offsetChannel_t offsetChannelInput;

   carrierChannelInput.integrationBandwidth = 1.0e+6;    /* Hz */
   carrierChannelInput.RRCFilterEnabled = RFMXSPECAN_VAL_ACP_RRC_FILTER_ENABLED_FALSE;
   carrierChannelInput.RRCFilterAlpha = 0.220;

   /* Offset Channels	*/
   offsetChannelInput.integrationBandwidth = 1.0e+6;     /* Hz */
   offsetChannelInput.RRCFilterEnabled = RFMXSPECAN_VAL_ACP_RRC_FILTER_ENABLED_FALSE;
   offsetChannelInput.RRCFilterAlpha = 0.220;

   offsetChannelInput.frequencyOffset[0] = 1.0e+6;       /* Hz */
   offsetChannelInput.frequencyOffset[1] = 2.0e+6;       /* Hz */

   /* Create new RFmx session */

   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure ACP measurements */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_CfgRFAttenuation(instrumentHandle, "", rfAttenuationAuto, rfAttenuation));

   if (autoLevel)
   {
      RFmxCheckWarn(RFmxSpecAn_AutoLevel(instrumentHandle, "", integrationBandwidth, measurementInterval, &autoSetReferenceLevel));
      printf("Reference level : %f\n", autoSetReferenceLevel);
   }
   else
   {
      RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   }
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_ACP,
      RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgPowerUnits(instrumentHandle, "", powerUnits));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount,
      averagingType));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgRBWFilter(instrumentHandle, "", RBWAuto, RBW, RBWFilterType));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgNoiseCompensationEnabled(instrumentHandle, "", noiseCompensationEnabled));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgCarrierIntegrationBandwidth(instrumentHandle, "",
      carrierChannelInput.integrationBandwidth));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgCarrierRRCFilter(instrumentHandle, "",
      carrierChannelInput.RRCFilterEnabled,
      carrierChannelInput.RRCFilterAlpha));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgNumberOfOffsets(instrumentHandle, "", numberOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgOffsetArray(instrumentHandle, "",
      offsetChannelInput.frequencyOffset, NULL, NULL,
      numberOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgOffsetIntegrationBandwidth(instrumentHandle, "offset::all",
      offsetChannelInput.integrationBandwidth));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgOffsetRRCFilter(instrumentHandle, "offset::all",
      offsetChannelInput.RRCFilterEnabled,
      offsetChannelInput.RRCFilterAlpha));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   /* Get actual size of the array */
   RFmxCheckWarn(RFmxSpecAn_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout,
      NULL, NULL, NULL, NULL, 0,
      &offsetMeasArraySize));
   if (offsetMeasArraySize > 0)
   {
      lowerRelativePower = (float64 *)malloc(sizeof(float64) * offsetMeasArraySize);
      upperRelativePower = (float64 *)malloc(sizeof(float64) * offsetMeasArraySize);
      lowerAbsolutePower = (float64 *)malloc(sizeof(float64) * offsetMeasArraySize);
      upperAbsolutePower = (float64 *)malloc(sizeof(float64) * offsetMeasArraySize);
      /* Fetch the measurements array */
      RFmxCheckWarn(RFmxSpecAn_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout,
         lowerRelativePower, upperRelativePower,
         lowerAbsolutePower, upperAbsolutePower,
         offsetMeasArraySize, NULL));
   }
   RFmxCheckWarn(RFmxSpecAn_ACPFetchCarrierMeasurement(instrumentHandle, "", timeout,
      &absolutePower, NULL, NULL, NULL));
   actualArraySize = 0; x0 = 0.0; dx = 0.0;
   RFmxCheckWarn(RFmxSpecAn_ACPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
      if (spectrum)
      {
         RFmxCheckWarn(RFmxSpecAn_ACPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("Carrier Measurements: \n");
   printf("Absolute Power (dBm or dBm/Hz) %f\n", absolutePower);
   printf("---------------------------------------------------\n");


   printf("Offset Channel Measurements: \n");
   for (i = 0; i < offsetMeasArraySize; i++)
   {
      printf("Offset  :  %d\n", i);
      printf("Lower Relative Power (dB)            %f\n", lowerRelativePower[i]);
      printf("Upper Relative Power (dB)            %f\n", upperRelativePower[i]);
      printf("Lower Absolute Power (dBm or dBm/Hz) %f\n", lowerAbsolutePower[i]);
      printf("Upper Absolute Power (dBm or dBm/Hz) %f\n", upperAbsolutePower[i]);
      printf("-------------------------------------------------\n");
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
   if (lowerRelativePower)
      free(lowerRelativePower);
   if (upperRelativePower)
      free(upperRelativePower);
   if (upperAbsolutePower)
      free(upperAbsolutePower);
   if (lowerAbsolutePower)
      free(lowerAbsolutePower);
   printf("Press any key to exit\n");
   _getch();

   return error;
}
