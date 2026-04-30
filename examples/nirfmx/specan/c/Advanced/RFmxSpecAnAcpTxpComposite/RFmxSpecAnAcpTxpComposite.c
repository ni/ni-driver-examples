//Steps:
//1. Open a new RFmx session
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Select ACP,TXP measurements in the measurements Array (to perform composite measurement)
//5. Configure ACP Averaging
//6. Configure ACP Integration BW, Number of Offset Channels and Channel Spacing
//This function configures a Carrier Channel with Offset Channels as specified by the Number of Offsets
//Refer to VI help for more information
//7. Configure ACP Measurement Interval and RBW
//8. Configure TXP Averaging
//9. Initiate Measurement
//10. Fetch ACP Carrier and Offset Measurements
//11. Fetch TXP Measurement
//12. Close the RFmx Session

// Include files

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   char *resourceName = "RFSA";
   char errorMessage[MAX_ERROR_DESCRIPTION];
   int32 error = 0, lastErrorCode = 0;

   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;        /* Hz */
   float64 referenceLevel = 0.00;         /* dBm */
   float64 externalAttenuation = 0.00;    /* dB */

   /* ACP */
   float64 integrationBandwidth = 1.0e+6; /* Hz */
   int32 numberOfOffsetChannels = 2;
   float64 channelSpacing = 1.0e+6;       /* Hz */

   int32 averagingEnabled = RFMXSPECAN_VAL_ACP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_ACP_AVERAGING_TYPE_RMS;

   /* TXP */
   float64 measurementInterval = 1.0e-3;  /* seconds */
   float64 RBW = 100.00e+3;               /* Hz */
   int32 RBWFilterType = RFMXSPECAN_VAL_TXP_RBW_FILTER_TYPE_GAUSSIAN;
   int32 TXPAveragingEnabled = RFMXSPECAN_VAL_TXP_AVERAGING_ENABLED_FALSE;
   int32 TXPAveragingCount = 10;
   int32 TXPAveragingType = RFMXSPECAN_VAL_TXP_AVERAGING_TYPE_RMS;
   float64 RRCAlpha = 0.100;

   float64 timeout = 10;                  /* seconds */

   /* Variables to store ACP and TXP measurement results */
   float64 carrierAbsPower = 0;
   float64 offCh0LowerRelativePower = 0;
   float64	offCh0UpperRelativePower = 0;
   float64	offCh1LowerRelativePower = 0;
   float64	offCh1UpperRelativePower = 0;

   float64 averageMeanPower = 0;
   float64 peakToAverageRatio = 0;
   float64 maximumPower = 0;
   float64 minimumPower = 0;

   /*Create a new RFmx Session.*/
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /*Configure measurement parameters.*/
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", (RFMXSPECAN_VAL_ACP | RFMXSPECAN_VAL_TXP),
      RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount,
      averagingType));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgCarrierAndOffsets(instrumentHandle, "", integrationBandwidth,
      numberOfOffsetChannels, channelSpacing));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgMeasurementInterval(instrumentHandle, "", measurementInterval));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgRBWFilter(instrumentHandle, "", RBW, RBWFilterType, RRCAlpha));
   RFmxCheckWarn(RFmxSpecAn_TXPCfgAveraging(instrumentHandle, "", TXPAveragingEnabled,
      TXPAveragingCount, TXPAveragingType));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /*Retrieve results.*/
   RFmxCheckWarn(RFmxSpecAn_ACPFetchCarrierMeasurement(instrumentHandle, "", timeout,
      &carrierAbsPower, NULL, NULL, NULL));
   RFmxCheckWarn(RFmxSpecAn_ACPFetchOffsetMeasurement(instrumentHandle, "offset0", timeout,
      &offCh0LowerRelativePower,
      &offCh0UpperRelativePower,
      NULL, NULL));
   RFmxCheckWarn(RFmxSpecAn_ACPFetchOffsetMeasurement(instrumentHandle, "offset1", timeout,
      &offCh1LowerRelativePower,
      &offCh1UpperRelativePower, NULL, NULL));
   RFmxCheckWarn(RFmxSpecAn_TXPFetchMeasurement(instrumentHandle, "", timeout,
      &averageMeanPower, &peakToAverageRatio,
      &maximumPower, &minimumPower));

   printf("------------------------ACP-----------------------------\n");
   printf("Carrier Abs Power (dBm or dBm/Hz)     %f\n", carrierAbsPower);
   printf("Off ch0 Lower Relative Power (dB)     %f\n", offCh0LowerRelativePower);
   printf("Off ch0 Upper Relative Power (dB)     %f\n", offCh0UpperRelativePower);
   printf("Off ch1 Lower Relative Power (dB)     %f\n", offCh1LowerRelativePower);
   printf("Off ch1 Upper Relative Power (dB)     %f\n", offCh1UpperRelativePower);

   printf("\n------------------------TXP-----------------------------\n");
   printf("Average Mean Power    (dBm)           %f\n", averageMeanPower);
   printf("Peak to Average Ratio (dB)            %f\n", peakToAverageRatio);
   printf("Maximum Power         (dBm)           %f\n", maximumPower);
   printf("Minimum Power         (dBm)           %f\n", minimumPower);

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
