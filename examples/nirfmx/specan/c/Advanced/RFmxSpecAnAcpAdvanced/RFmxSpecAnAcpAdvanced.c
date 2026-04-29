//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the Center Frequency
//4. Configure the basic instrument properties (Clock Source, Clock Frequency)
//5. Configure the basic signal properties  (Reference Level, External Attenuation and RF Attenuation)
//6. Select ACP measurement and enable the traces
//7. Configure ACP Measurement Method, Power Units and Averaging Parameters
//8. Configure ACP FFT
//9. Configure ACP RBW Filter
//10. Configure ACP Sweep Time
//11. Configure ACP Noise Compensation
//12. Configure ACP Number of Carrier Channels
//13. Configure ACP Carrier Channel Settings (Integration BW, Carrier Mode, RRC Filter, Carrier Offset)
//14. Configure ACP Number of Offset Channels
//15. Configure ACP Offset Channel Settings (Integration BW, Offset Frequency, Offset Power Reference,
//    Relative Attenuation, RRC Filter)
//16. Initiate Measurement
//17. Fetch ACP Measurements and Traces
//18. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING         256

#define NUMBER_OF_CARRIERS          1
#define NUMBER_OF_OFFSETS           2

typedef struct {
   int32 mode;
   float64 carrierFrequency;
   float64 integrationBandwidth;
   int32 RRCFilterEnabled;
   float64 RRCFilterAlpha;
}carrierChannel_t;

typedef struct {
   float64 absolutePower;
   float64 totalRelativePower;
   float64 resCarrierOffset;
   float64 resIntegrationBandwidth;
}carrierMeas_t;


int main(int argc, char *argv[])
{
   int i;
   char offsetString[MAX_SELECTOR_STRING], carrierString[MAX_SELECTOR_STRING];
   char *resourceName = "RFSA";
   char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;

   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];

   int32 numberOfCarriers = NUMBER_OF_CARRIERS;
   int32 numberOfOffsets = NUMBER_OF_OFFSETS;
   float64 timeout = 10.0;                /* seconds */

   /* Variables to store the result */
   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = (float32 *)NULL;
   int32 actualArraySize = 0;
   float64 totalCarrierPower = 0;

   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;        /* Hz */
   float64 referenceLevel = 0.00;         /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */

   int32 rfAttenuationAuto = RFMXSPECAN_VAL_TRUE;
   float64 rfAttenuation = 10.00;         /* dB */

   float64 frequency = 10.0e+6;           /* Hz */

   int32 powerUnits = RFMXSPECAN_VAL_ACP_POWER_UNITS_DBM;
   int32 measurementMethod = RFMXSPECAN_VAL_ACP_MEASUREMENT_METHOD_NORMAL;
   int32 noiseCompensationEnabled = RFMXSPECAN_VAL_ACP_NOISE_COMPENSATION_ENABLED_FALSE;

   /* Sweep Time */
   int32 sweepTimeAuto = RFMXSPECAN_VAL_ACP_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 1.00e-3;   /* seconds */

   /* RBW Filter */
   int32 RBWFilterType = RFMXSPECAN_VAL_ACP_RBW_FILTER_TYPE_GAUSSIAN;
   int32 RBWAuto = RFMXSPECAN_VAL_ACP_RBW_AUTO_TRUE;
   float64 RBW = 10.0e+3;                 /* Hz */

   /* FFT */
   int32 FFTWindow = RFMXSPECAN_VAL_ACP_FFT_WINDOW_FLAT_TOP;
   float64 FFTPadding = -1.0;

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_ACP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_ACP_AVERAGING_TYPE_RMS;

   /* Declare input variables for array configs*/
   int32 offsetEnabled[NUMBER_OF_OFFSETS];
   int32 offsetFrequencySideband[NUMBER_OF_OFFSETS];
   float64 offsetFrequency[NUMBER_OF_OFFSETS];
   int32 offsetReferenceCarrier[NUMBER_OF_OFFSETS];
   int32 offsetReferenceSpecific[NUMBER_OF_OFFSETS];
   int32 offsetRRCFilterEnabled[NUMBER_OF_OFFSETS];
   float64 offsetIntegrationBandwidth[NUMBER_OF_OFFSETS];
   float64 offsetRelativeAttenuation[NUMBER_OF_OFFSETS];
   float64 offsetRRCFilterAlpha[NUMBER_OF_OFFSETS];
   int32 offsetFrequencyDefinition[NUMBER_OF_OFFSETS];

   /*Declare arrays to hold result values*/
   float64 lowerRelativePower[NUMBER_OF_OFFSETS];
   float64 upperRelativePower[NUMBER_OF_OFFSETS];
   float64 lowerAbsolutePower[NUMBER_OF_OFFSETS];
   float64 upperAbsolutePower[NUMBER_OF_OFFSETS];

   carrierChannel_t carrierChannelInput[NUMBER_OF_CARRIERS];
   carrierMeas_t carrierChannelOutput[NUMBER_OF_CARRIERS];

   /* Setup carrier Channels */
   for (i = 0; i < numberOfCarriers; i++)
   {
      carrierChannelInput[i].mode = RFMXSPECAN_VAL_ACP_CARRIER_MODE_ACTIVE;
      carrierChannelInput[i].carrierFrequency = 0.000;
      carrierChannelInput[i].integrationBandwidth = 1.0e+6;	/* Hz */
      carrierChannelInput[i].RRCFilterEnabled = RFMXSPECAN_VAL_ACP_RRC_FILTER_ENABLED_FALSE;
      carrierChannelInput[i].RRCFilterAlpha = 0.220;
   }

   /* Setup offset Channels */
   for (i = 0; i < numberOfOffsets; i++)
   {
      offsetEnabled[i] = RFMXSPECAN_VAL_ACP_OFFSET_ENABLED_TRUE;
      if (i == 0)	/* For offset 0, set freq offset = 1 MHz */
         offsetFrequency[i] = 1.0e+6;
      else
         offsetFrequency[i] = 2.0e+6;
      offsetFrequencySideband[i] = RFMXSPECAN_VAL_ACP_OFFSET_SIDEBAND_BOTH;
      offsetReferenceCarrier[i] = RFMXSPECAN_VAL_ACP_OFFSET_POWER_REFERENCE_CARRIER_CLOSEST;
      offsetReferenceSpecific[i] = 0;
      offsetIntegrationBandwidth[i] = 1.0e+6;
      offsetRelativeAttenuation[i] = 0.00;
      offsetRRCFilterEnabled[i] = RFMXSPECAN_VAL_ACP_RRC_FILTER_ENABLED_FALSE;
      offsetRRCFilterAlpha[i] = 0.220;
      offsetFrequencyDefinition[i] = RFMXSPECAN_VAL_ACP_CARRIER_CENTER_TO_OFFSET_CENTER;
   }

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure ACP parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_CfgRFAttenuation(instrumentHandle, "", rfAttenuationAuto, rfAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_ACP, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgPowerUnits(instrumentHandle, "", powerUnits));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgFFT(instrumentHandle, "", FFTWindow, FFTPadding));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgRBWFilter(instrumentHandle, "", RBWAuto, RBW, RBWFilterType));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgNoiseCompensationEnabled(instrumentHandle, "", noiseCompensationEnabled));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgNumberOfCarriers(instrumentHandle, "", numberOfCarriers));
   for (i = 0; i < numberOfCarriers; i++)
   {
      RFmxSpecAn_BuildCarrierString2("", i, MAX_SELECTOR_STRING, carrierString);
      RFmxCheckWarn(RFmxSpecAn_ACPCfgCarrierIntegrationBandwidth(instrumentHandle, carrierString,
         carrierChannelInput[i].integrationBandwidth));
      RFmxCheckWarn(RFmxSpecAn_ACPCfgCarrierMode(instrumentHandle, carrierString,
         carrierChannelInput[i].mode));
      RFmxCheckWarn(RFmxSpecAn_ACPCfgCarrierRRCFilter(instrumentHandle, carrierString,
         carrierChannelInput[i].RRCFilterEnabled,
         carrierChannelInput[i].RRCFilterAlpha));
      RFmxCheckWarn(RFmxSpecAn_ACPCfgCarrierFrequency(instrumentHandle, carrierString,
         carrierChannelInput[i].carrierFrequency));
   }
   RFmxCheckWarn(RFmxSpecAn_ACPCfgNumberOfOffsets(instrumentHandle, "", numberOfOffsets));
   for (i = 0; i < numberOfOffsets; i++)
   {
      RFmxSpecAn_BuildOffsetString2("", i, MAX_SELECTOR_STRING, offsetString);
      RFmxCheckWarn(RFmxSpecAn_ACPCfgOffsetFrequencyDefinition(instrumentHandle, offsetString,
         offsetFrequencyDefinition[i]));
   }

   RFmxCheckWarn(RFmxSpecAn_ACPCfgOffsetArray(instrumentHandle, "", offsetFrequency, offsetFrequencySideband,
      offsetEnabled, numberOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgOffsetIntegrationBandwidthArray(instrumentHandle, "", offsetIntegrationBandwidth,
      numberOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgOffsetPowerReferenceArray(instrumentHandle, "", offsetReferenceCarrier,
      offsetReferenceSpecific, numberOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgOffsetRelativeAttenuationArray(instrumentHandle, "", offsetRelativeAttenuation,
      numberOfOffsets));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgOffsetRRCFilterArray(instrumentHandle, "", offsetRRCFilterEnabled,
      offsetRRCFilterAlpha, numberOfOffsets));

   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, lowerRelativePower,
      upperRelativePower, lowerAbsolutePower, upperAbsolutePower, numberOfOffsets, &actualArraySize));

   for (i = 0; i < numberOfCarriers; i++)
   {
      RFmxSpecAn_BuildCarrierString2("", i, MAX_SELECTOR_STRING, carrierString);
      RFmxCheckWarn(RFmxSpecAn_ACPFetchCarrierMeasurement(instrumentHandle, carrierString, timeout,
         &carrierChannelOutput[i].absolutePower,
         &carrierChannelOutput[i].totalRelativePower,
         &carrierChannelOutput[i].resCarrierOffset,
         &carrierChannelOutput[i].resIntegrationBandwidth));
   }

   RFmxCheckWarn(RFmxSpecAn_ACPFetchTotalCarrierPower(instrumentHandle, "", timeout, &totalCarrierPower));

   /* Retrieve the size of the spectrum first */
   RFmxCheckWarn(RFmxSpecAn_ACPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (spectrum)
      {
         /* Retrieve the spectrum */
         RFmxCheckWarn(RFmxSpecAn_ACPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   /* Display the results of the measurement */
   printf("Total Carrier Power (dBm or dBm/Hz)   %f\n", totalCarrierPower);
   printf("\nCarrier Measurements: \n");
   for (i = 0; i < numberOfCarriers; i++)
   {
      printf("\nCarrier %d: \n", i);
      printf("Abosulte Power (dBm or dBm/Hz)        %f\n", carrierChannelOutput[i].absolutePower);
      printf("Total Relative Power (dB)             %f\n", carrierChannelOutput[i].totalRelativePower);
      printf("Carrier Offset (Hz)                   %f\n", carrierChannelOutput[i].resCarrierOffset);
      printf("Integration Bandwidth (Hz)            %f\n", carrierChannelOutput[i].resIntegrationBandwidth);
      printf("---------------------------------------------------\n");
   }

   printf("\nOffset Channel Measurements: \n");
   for (i = 0; i < numberOfOffsets; i++)
   {
      printf("\nOffset %d: \n", i);
      printf("Lower Relative Power (dB)             %f\n", lowerRelativePower[i]);
      printf("Upper Relative Power (dB)             %f\n", upperRelativePower[i]);
      printf("Lower Absolute Power (dBm or dBm/Hz)  %f\n", lowerAbsolutePower[i]);
      printf("Upper Absolute Power (dBm or dBm/Hz)  %f\n", upperAbsolutePower[i]);
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

   printf("Press any key to exit\n");
   _getch();

   return error;
}
