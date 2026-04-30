//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3.A. Configure the basic signal properties - Center Frequency and External Attenuation
//3.B. Configure the Reference Level to be used in the first run of ACP Measurement
//4. Select ACP measurement and enable the traces
//5. Configure Averaging parameters for the ACP Measurement
//6. Configure Integration BW of the Carrier channel, Number of Offset Channels, Channel Spacing
//7. Initiate ACP Measurement with a Result name.
//   When Result name is wired to Initiate same result name should be used to retrieve results from session.
//8. Wait for ACP_1 acquisition to complete
//9. Configure the Reference level to be used in the second run of ACP Measurement
//10. Initiate another ACP Measurement with a Result name. Use this result name while retrieving the results from session
//11. Fetch ACP_1 Measurement Results using Result name for Selector String
//12. Fetch ACP_2 Measurement Results using Result name for Selector String
//13. Close the RFmx Session

// Include files

#include <stdio.h>
#include <conio.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING         256

#define NUM_OF_OFFSET_CHANNELS      2

int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   char *resourceName = "RFSA";
   char errorMessage[MAX_ERROR_DESCRIPTION];
   char result1Name[MAX_SELECTOR_STRING];
   char result2Name[MAX_SELECTOR_STRING];
   int32 error = 0, lastErrorCode = 0;

   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;        /* Hz */
   float64 referenceLevel1 = 0.00;        /* dBm */
   float64 referenceLevel2 = -10.00;      /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */

   /* ACP */
   float64 integrationBandwidth = 1.0e+6; /* Hz */
   int32 numberOfOffsetChannels = NUM_OF_OFFSET_CHANNELS;
   float64 channelSpacing = 1.0e+6;       /* Hz */

   int32 averagingEnabled = RFMXSPECAN_VAL_ACP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_ACP_AVERAGING_TYPE_RMS;

   float64 carrierAbsolutePower1, carrierAbsolutePower2;
   float64 lowerRelativePower1[NUM_OF_OFFSET_CHANNELS];
   float64 upperRelativePower1[NUM_OF_OFFSET_CHANNELS];
   float64 lowerRelativePower2[NUM_OF_OFFSET_CHANNELS];
   float64 upperRelativePower2[NUM_OF_OFFSET_CHANNELS];
   float64 timeout = 10;                  /* seconds */
   int32 i = 0;

   /*Create a new RFmx Session.*/
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /*Configure measurement parameters.*/
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel1));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_ACP, RFMXSPECAN_VAL_FALSE));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount,
      averagingType));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgCarrierAndOffsets(instrumentHandle, "", integrationBandwidth,
      numberOfOffsetChannels, channelSpacing));

   RFmxSpecAn_BuildSignalString("", "ACP_Results_1", MAX_SELECTOR_STRING, result1Name);
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", result1Name));
   RFmxCheckWarn(RFmxSpecAn_WaitForAcquisitionComplete(instrumentHandle, timeout));

   RFmxSpecAn_BuildSignalString("", "ACP_Results_2", MAX_SELECTOR_STRING, result2Name);
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel2));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", result2Name));

   /*Retrieve results.*/
   RFmxCheckWarn(RFmxSpecAn_ACPFetchCarrierMeasurement(instrumentHandle, result1Name, timeout,
      &carrierAbsolutePower1, NULL, NULL, NULL));

   RFmxCheckWarn(RFmxSpecAn_ACPFetchOffsetMeasurementArray(instrumentHandle, result1Name, timeout,
      lowerRelativePower1,
      upperRelativePower1,
      NULL, NULL, numberOfOffsetChannels, NULL));

   RFmxCheckWarn(RFmxSpecAn_ACPFetchCarrierMeasurement(instrumentHandle, result2Name, timeout,
      &carrierAbsolutePower2, NULL, NULL, NULL));

   RFmxCheckWarn(RFmxSpecAn_ACPFetchOffsetMeasurementArray(instrumentHandle, result2Name, timeout,
      lowerRelativePower2,
      upperRelativePower2,
      NULL, NULL, numberOfOffsetChannels, NULL));

   printf("------------------------ACP Measurement1-----------------------------\n");
   printf("Carrier Abs Power (dBm or dBm/Hz)   :%f\n", carrierAbsolutePower1);
   for (i = 0; i < numberOfOffsetChannels; i++)
   {
      printf("\nOffset Channel                      : %d\n", i);
      printf("Lower Relative Power (dB)           : %f\n", lowerRelativePower1[i]);
      printf("Upper Relative Power (dB)           : %f\n", upperRelativePower1[i]);
   }

   printf("\n------------------------ACP Measurement2-----------------------------\n");
   printf("Carrier Abs Power (dBm or dBm/Hz)   : %f\n", carrierAbsolutePower2);
   for (i = 0; i < numberOfOffsetChannels; i++)
   {
      printf("\nOffset Channel %d\n", i);
      printf("Lower Relative Power (dB)           : %f\n", lowerRelativePower2[i]);
      printf("Upper Relative Power (dB)           : %f\n", upperRelativePower2[i]);
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
   printf("Press any key to exit\n");
   _getch();
   return error;
}
