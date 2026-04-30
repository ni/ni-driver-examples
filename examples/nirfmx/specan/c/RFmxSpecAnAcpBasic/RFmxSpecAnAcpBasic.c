//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure ACP Averaging Parameters
//5. Configure ACP Integration BW, Number of Offset Channels and Channel Spacing
//This function configures a Carrier Channel with Offset Channels as specified by the Number of Offsets
//6. Read ACP Measurement Results
//This function returns Absolute Power for the Carrier Channel and Relative Powers for two Offset Channels
//7. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0;
   int32 lastErrorCode = 0;

   char *resourceName = "RFSA";
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;        /* Hz */
   float64 referenceLevel = 0.00;         /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */

   float64 integrationBandwidth = 1.0e+6; /* Hz */
   float64 channelSpacing = 1.0e+6;       /* Hz */

   int32 numberOfOffsetChannels = 2;

   int32 averagingEnabled = RFMXSPECAN_VAL_ACP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_ACP_AVERAGING_TYPE_RMS;
   float64 timeout = 10;                  /* seconds */

   /* Variables to store the result */
   float64 carrierAbsPower;
   float64 offCh0LowerRelativePower;
   float64 offCh0UpperRelativePower;
   float64 offCh1LowerRelativePower;
   float64 offCh1UpperRelativePower;

   /* Initialize a session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure ACP measurement */
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel,
      externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount,
      averagingType));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgCarrierAndOffsets(instrumentHandle, "", integrationBandwidth,
      numberOfOffsetChannels, channelSpacing));

   /* Read ACP measurements */
   RFmxCheckWarn(RFmxSpecAn_ACPRead(instrumentHandle, "", timeout, &carrierAbsPower,
      &offCh0LowerRelativePower, &offCh0UpperRelativePower,
      &offCh1LowerRelativePower, &offCh1UpperRelativePower));

   printf("Carrier Absolute Power (dBm or dBm/Hz)  %f\n", carrierAbsPower);
   printf("Offset ch0 Lower Relative Power (dB)    %f\n", offCh0LowerRelativePower);
   printf("Offset ch0 Upper Relative Power(dB)     %f\n", offCh0UpperRelativePower);
   printf("Offset ch1 Lower Relative Power(dB)     %f\n", offCh1LowerRelativePower);
   printf("Offset ch1 Upper Relative Power(dB)     %f\n", offCh1UpperRelativePower);

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
   printf("Press any key to exit\n");
   _getch();

   return error;
}
