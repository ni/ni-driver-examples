//Steps:
//1. Open a new RFmx session.
//2. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//5. Configure Standard and Channel Bandwidth Properties.
//6. Select OFDMModAcc and TXP measurements.
//7. Configure the Measurement Interval.
//8. Configure Averaging parameters for OFDMModAcc.
//9. Configure Averaging parameters for TXP.
//10. Configure the Maximum Measurement Interval.
//11. Initiate Measurement.
//12. Fetch OFDMModAcc Measurement.
//13. Fetch TXP Measurement.
//14. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWLAN.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION               4096

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };

   int32 error = 0, lastErrorCode = 0;
   int i = 0;

   float64 centerFrequency = 2.412e9;                                /*(Hz) */
   float64 referenceLevel = 0.0;                                     /*(dBm) */
   float64 externalAttenuation = 0.0;                                /*(dB) */

   char *frequencyReferenceSource = RFMXWLAN_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10e6;                       /*(Hz) */

   int32 IQPowerEdgeEnabled = RFMXWLAN_VAL_TRUE;
   float64 IQPowerEdgeLevel = -20.0;                                 /*(dB) */
   float64 triggerDelay = 0.0;                                       /*(s) */
   int32 minimumQuietTimeMode = RFMXWLAN_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 5.0e-6;                                /* (s) */

   int32 standard = RFMXWLAN_VAL_STANDARD_802_11_AG;

   float64 channelBandwidth = 20e6;                                  /*(Hz) */

   int32 OFDMModAccAveragingEnabled = RFMXWLAN_VAL_OFDMMODACC_AVERAGING_ENABLED_FALSE;
   int32 OFDMModAccAveragingCount = 10;

   int32 measurementOffset = 0;                                      /*(symbols)*/
   int32 maximumMeasurementLength = 16;                              /*(symbols)*/

   int32 TXPAveragingEnabled = RFMXWLAN_VAL_TXP_AVERAGING_ENABLED_FALSE;
   int32 TXPAveragingCount = 10;

   float64 maximumMeasurementInterval = 1e-3;                        /*(s) */

   float64 timeout = 10.0;                                           /*(s) */

   float64 compositeRMSEVMMean = 0.0;                                /*(dB) */
   float64 compositeDataRMSEVMMean = 0.0;                            /*(dB) */
   float64 compositePilotRMSEVMMean = 0.0;                           /*(dB) */

   float64 averagePowerMean = 0.0;                                   /*(dBm) */
   float64 peakPowerMaximum = 0.0;                                   /*(dBm) */

   /* Initialize a session */
   RFmxCheckWarn(RFmxWLAN_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxWLAN_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxWLAN_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxWLAN_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxWLAN_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxWLAN_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXWLAN_VAL_IQ_POWER_EDGE_RISING_SLOPE,
      IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
      RFMXWLAN_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, IQPowerEdgeEnabled));
   RFmxCheckWarn(RFmxWLAN_CfgStandard(instrumentHandle, "", standard));
   RFmxCheckWarn(RFmxWLAN_CfgChannelBandwidth(instrumentHandle, "", channelBandwidth));
   RFmxCheckWarn(RFmxWLAN_SelectMeasurements(instrumentHandle, "", RFMXWLAN_VAL_TXP|RFMXWLAN_VAL_OFDMMODACC, RFMXWLAN_VAL_TRUE));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgMeasurementLength(instrumentHandle, "", measurementOffset, maximumMeasurementLength));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgAveraging(instrumentHandle, "", OFDMModAccAveragingEnabled, OFDMModAccAveragingCount));
   RFmxCheckWarn(RFmxWLAN_TXPCfgAveraging(instrumentHandle, "", TXPAveragingEnabled, TXPAveragingCount));
   RFmxCheckWarn(RFmxWLAN_TXPCfgMaximumMeasurementInterval(instrumentHandle, "", maximumMeasurementInterval));
   RFmxCheckWarn(RFmxWLAN_Initiate(instrumentHandle, "", ""));

   /* Fetch Results */
   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchCompositeRMSEVM(instrumentHandle, "", timeout, &compositeRMSEVMMean,
       &compositeDataRMSEVMMean, &compositePilotRMSEVMMean));
   RFmxCheckWarn(RFmxWLAN_TXPFetchMeasurement(instrumentHandle, "", timeout, &averagePowerMean, &peakPowerMaximum));

   printf("------------------OFDMModAcc Measurement------------------\n");
   printf("RMS EVM Mean (dB)                       : %lf\n", compositeRMSEVMMean);
   printf("Data RMS EVM Mean (dB)                  : %lf\n", compositeDataRMSEVMMean);
   printf("Pilot RMS EVM Mean (dB)                 : %lf\n\n", compositePilotRMSEVMMean);

   printf("\n----------TXP Measurement----------\n\n");
   printf("Average Power Mean (dBm)                : %lf\n", averagePowerMean);
   printf("Peak Power Maximum (dBm)                : %lf\n\n", peakPowerMaximum);

Error:
   if (error)
   {
      RFmxWLAN_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("\nERROR: %s\n", errorMessage);
      else
         printf("\nWARNING: %s\n", errorMessage);
   }
   if (instrumentHandle)
   {
      RFmxWLAN_Close(instrumentHandle, RFMXWLAN_VAL_FALSE);
   }

   printf("\n\nPress any key to exit\n");
   _getch();

   return error;
}
