//Steps:
//1. Open a new RFmx session
//2. Create WCDMA Signal
//3. Configure Selected Ports for WCDMA Signal
//4. Configure the basic signal properties for WCDMA Signal (Center Frequency, Reference Level and External Attenuation)
//5. Configure ACP Averaging for WCDMA Signal
//6. Configure ACP Integration BW, Number of Offset Channels, Channel Spacing for WCDMA Signal
//7. Create LTE Signal
//8. Configure Selected Ports for LTE Signal
//9. Configure the basic signal properties for LTE Signal (Center Frequency, Reference Level and External Attenuation)
//10. Configure ACP Averaging for LTE Signal
//11. Configure ACP Number of Carriers = "1" for LTE Signal
//12. Configure ACP Carrier Integration BW for LTE Signal
//13. Configure ACP Number of Offsets for LTE Signal
//14. Configure ACP Offset Integration BW, Offset Frequency, Sidebands and RRC Filter for LTE Signal
//15. Read ACP Measurements for WCDMA Signal
//16. Read ACP Measurements for LTE Signal
//17. Delete WCDMA Signal
//18. Delete LTE Signal
//19. Close the RFmx session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

/* Maximum size of signal string */
#define MAX_SIGNAL_STRING           256

/* Maximum size of carrier string */
#define MAX_CARRIER_STRING          256

/* Maximum size of offset string */
#define MAX_OFFSET_STRING           256

#define NUMBER_OF_OFFSET_CHANNELS   2
#define NUMBER_OF_CARRIERS          1

typedef struct
{
   int32 enabled;
   float64 offset;
   int32 sideband;
   float64 integrationBandwidth;
   int32 RRCFilterEnabled;
   float64 RRCFilterAlpha;
}offsetChannels_t;

int main(int argc, char *argv[])
{
   int32 i = 0;
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];
   char wcdmaSignalString[MAX_SIGNAL_STRING];
   char lteSignalString[MAX_SIGNAL_STRING];

   char *resourceName = "RFSA";
   char *selectedPorts = "";
   float64 referenceLevel = 0.00;         /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */
   float64 timeout = 10.0;                /* seconds */

   /* WCDMA Signal settings */
   float64 wcdmaCenterFrequency = 468.0e+6;        /* Hz */
   float64 wcdmaIntegrationBandwidth = 3.840e+6;   /* Hz */
   int32 wcdmaNumberOfOffsetChannels = NUMBER_OF_OFFSET_CHANNELS;
   float64 wcdmaChannelSpacing = 5.0e+6;           /* Hz */

   /* Averaging */
   int32 wcdmaAveragingEnabled = RFMXSPECAN_VAL_ACP_AVERAGING_ENABLED_FALSE;
   int32 wcdmaAveragingCount = 10;
   int32 wcdmaAveragingType = RFMXSPECAN_VAL_ACP_AVERAGING_TYPE_RMS;

   /* LTE Signal settings */
   float64 lteCenterFrequency = 2.1e+9;         /* Hz */
   float64 lteIntegrationBandwidth = 9.0e+6;    /* Hz */
   int32 numOfOffsetChannels = NUMBER_OF_OFFSET_CHANNELS;
   int32 numOfCarriers = NUMBER_OF_CARRIERS;
   int32 offChsEnabled[NUMBER_OF_OFFSET_CHANNELS];
   float64 offChsOffset[NUMBER_OF_OFFSET_CHANNELS];
   int32 offChsSideband[NUMBER_OF_OFFSET_CHANNELS];
   float64 offChsIntegrationBandwidth[NUMBER_OF_OFFSET_CHANNELS];
   int32 offChsRRCFilterEnabled[NUMBER_OF_OFFSET_CHANNELS];
   float64 offChsRRCFilterAlpha[NUMBER_OF_OFFSET_CHANNELS];

   /* LTE Averaging */
   int32 lteAveragingEnabled = RFMXSPECAN_VAL_ACP_AVERAGING_ENABLED_FALSE;
   int32 lteAveragingCount = 10;
   int32 lteAveragingType = RFMXSPECAN_VAL_ACP_AVERAGING_TYPE_RMS;

   /* Variables to store WCDMA signal measurement results */
   float64 wcdmaCarrierAbsPower = 0;
   float64 wcdmaOffCh0LowerRelativePower = 0;
   float64 wcdmaOffCh0UpperRelativePower = 0;
   float64 wcdmaOffCh1LowerRelativePower = 0;
   float64 wcdmaOffCh1UpperRelativePower = 0;

   /* Variables to store LTE signal measurement results */
   float64 lteCarrierAbsPower = 0;
   float64 lteOffCh0LowerRelativePower = 0;
   float64 lteOffCh0UpperRelativePower = 0;
   float64 lteOffCh1LowerRelativePower = 0;
   float64 lteOffCh1UpperRelativePower = 0;

   for (i = 0; i < numOfOffsetChannels; i++)
   {
      offChsEnabled[i] = RFMXSPECAN_VAL_ACP_OFFSET_ENABLED_TRUE;
      offChsSideband[i] = RFMXSPECAN_VAL_ACP_OFFSET_SIDEBAND_BOTH;
      offChsRRCFilterAlpha[i] = 0.220;
      if (i == 0)
      {
         offChsOffset[i] = 10.0e+6;
         offChsIntegrationBandwidth[i] = 9.0e+6;
         offChsRRCFilterEnabled[i] = RFMXSPECAN_VAL_ACP_RRC_FILTER_ENABLED_FALSE;
      }
      else
      {
         offChsOffset[i] = 7.500e+6;
         offChsIntegrationBandwidth[i] = 3.840e+6;
         offChsRRCFilterEnabled[i] = RFMXSPECAN_VAL_ACP_RRC_FILTER_ENABLED_TRUE;
      }
   }

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure ACP parameters for WCDMA signal analysis */
   RFmxSpecAn_BuildSignalString("WCDMA", "", MAX_SIGNAL_STRING, wcdmaSignalString);
   RFmxCheckWarn(RFmxSpecAn_CreateSignalConfiguration(instrumentHandle, wcdmaSignalString));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, wcdmaSignalString, selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, wcdmaSignalString, wcdmaCenterFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgAveraging(instrumentHandle, wcdmaSignalString, wcdmaAveragingEnabled,
      wcdmaAveragingCount, wcdmaAveragingType));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgCarrierAndOffsets(instrumentHandle, wcdmaSignalString,
      wcdmaIntegrationBandwidth, wcdmaNumberOfOffsetChannels,
      wcdmaChannelSpacing));

   /* Configure ACP parameters for LTE signal analysis */
   RFmxSpecAn_BuildSignalString("LTE", "", MAX_SIGNAL_STRING, lteSignalString);
   RFmxCheckWarn(RFmxSpecAn_CreateSignalConfiguration(instrumentHandle, lteSignalString));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, lteSignalString, selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, lteSignalString, lteCenterFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgAveraging(instrumentHandle, lteSignalString, lteAveragingEnabled,
      lteAveragingCount, lteAveragingType));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgNumberOfCarriers(instrumentHandle, lteSignalString, numOfCarriers));

   RFmxCheckWarn(RFmxSpecAn_ACPCfgCarrierIntegrationBandwidth(instrumentHandle, lteSignalString,
      lteIntegrationBandwidth));

   RFmxCheckWarn(RFmxSpecAn_ACPCfgNumberOfOffsets(instrumentHandle, lteSignalString, numOfOffsetChannels));

   RFmxCheckWarn(RFmxSpecAn_ACPCfgOffsetIntegrationBandwidthArray(instrumentHandle, lteSignalString,
      offChsIntegrationBandwidth, NUMBER_OF_OFFSET_CHANNELS));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgOffsetArray(instrumentHandle, lteSignalString,
      offChsOffset, offChsSideband, offChsEnabled, NUMBER_OF_OFFSET_CHANNELS));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgOffsetRRCFilterArray(instrumentHandle, lteSignalString,
      offChsRRCFilterEnabled, offChsRRCFilterAlpha, NUMBER_OF_OFFSET_CHANNELS));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_ACPRead(instrumentHandle, wcdmaSignalString, timeout, &wcdmaCarrierAbsPower,
      &wcdmaOffCh0LowerRelativePower, &wcdmaOffCh0UpperRelativePower,
      &wcdmaOffCh1LowerRelativePower, &wcdmaOffCh1UpperRelativePower));
   RFmxCheckWarn(RFmxSpecAn_ACPRead(instrumentHandle, lteSignalString, timeout, &lteCarrierAbsPower,
      &lteOffCh0LowerRelativePower, &lteOffCh0UpperRelativePower,
      &lteOffCh1LowerRelativePower, &lteOffCh1UpperRelativePower));

   /* Display WCDAM and LTE ACP measurement results */
   printf("\n---------------WCDMA Result--------------------\n");
   printf("Carrier Abs Power (dBm or dBm/Hz)   : %f\n", wcdmaCarrierAbsPower);
   printf("Off ch0 Lower Relative Power (dB)   : %f\n", wcdmaOffCh0LowerRelativePower);
   printf("Off ch0 Upper Relative Power(dB)    : %f\n", wcdmaOffCh0UpperRelativePower);
   printf("Off ch1 Lower Relative Power(dB)    : %f\n", wcdmaOffCh1LowerRelativePower);
   printf("Off ch1 Upper Relative Power(dB)    : %f\n", wcdmaOffCh1UpperRelativePower);

   printf("\n---------------LTE Result--------------------\n");
   printf("Carrier Abs Power (dBm or dBm/Hz)   : %f\n", lteCarrierAbsPower);
   printf("Off ch0 Lower Relative Power (dB)   : %f\n", lteOffCh0LowerRelativePower);
   printf("Off ch0 Upper Relative Power(dB)    : %f\n", lteOffCh0UpperRelativePower);
   printf("Off ch1 Lower Relative Power(dB)    : %f\n", lteOffCh1LowerRelativePower);
   printf("Off ch1 Upper Relative Power(dB)    : %f\n", lteOffCh1UpperRelativePower);

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
      RFmxSpecAn_DeleteSignalConfiguration(instrumentHandle, wcdmaSignalString);
      RFmxSpecAn_DeleteSignalConfiguration(instrumentHandle, lteSignalString);
      RFmxSpecAn_Close(instrumentHandle, RFMXSPECAN_VAL_FALSE);
      printf("\nPress any key to exit\n");
      _getch();
   }
   return error;
}
