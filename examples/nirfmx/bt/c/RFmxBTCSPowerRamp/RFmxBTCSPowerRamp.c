/*Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Packet Type.
6. Configure Data Rate.
7. Configure CS Packet Format, CS SYNC Sequence, CS Phase Measurement Period and CS Tone Extension Slot.
8. Select PowerRamp measurement
9. Configure Burst Synchronization Type 
10. Configure Averaging Parameters for PowerRamp measurement.
11. Initiate the Measurement.
12.  Fetch PowerRamp Measurements.
13. Close RFmx Session. */

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxBT.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char* argv[])
{
 char* resourceName = "RFSA";
 niRFmxInstrHandle instrumentHandle = NULL;

 char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
 int32 error = 0, lastErrorCode = 0;

 char* frequencyReferenceSource = RFMXBT_VAL_ONBOARD_CLOCK_STR;
 float64 frequencyReferenceFrequency = 10e6;                                  /*(Hz) */

 float64 centerFrequency = 2.402e9;                                           /*(Hz) */
 float64 referenceLevel = 0.0;                                                /*(dBm) */
 float64 externalAttenuation = 0.0;                                           /*(dB) */

 int32 enableTrigger = RFMXBT_VAL_TRUE;
 int32 IQPowerEdgeSlope = RFMXBT_VAL_IQ_POWER_EDGE_RISING_SLOPE;
 float64 IQPowerEdgeLevel = -20.0;                                            /*(dB) */
 int32 minimumQuiteTimeMode = RFMXBT_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
 float64 minimumQuietTime = 100e-6;                                           /*(seconds) */
 int32 IQPowerEdgeLevelType = RFMXBT_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
 float64 triggerDelay = 0.0;                                                  /*(seconds) */

 int32 packetType = RFMXBT_VAL_PACKET_TYPE_LE_CS;
 int32 LEDataRate = 1000000;                                                  /*(bps) */

 int channelSoundingPacketFormat = RFMXBT_VAL_CHANNEL_SOUNDING_PACKET_FORMAT_SYNC;
 int channelSoundingSyncSequence = RFMXBT_VAL_CHANNEL_SOUNDING_SYNC_SEQUENCE_NONE;
 float64 channelSoundingPhaseMeasurmentPeriod = 10e-6;
 int channelSoundingToneExtensionSlot = RFMXBT_VAL_CHANNEL_SOUNDING_TONE_EXTENSION_SLOT_DISABLED;

 int32 averagingEnabled = RFMXBT_VAL_POWERRAMP_AVERAGING_ENABLED_FALSE;
 int32 averagingCount = 10;

 float64* riseTimeMean = 0;                                                   /*(seconds) */
 float64* fallTimeMean = 0;                                                   /*(seconds) */
 float64* fortydBFallTimeMean = 0;                                            /*(seconds) */

 /* Initialize a session */
 RFmxCheckWarn(RFmxBT_Initialize(resourceName, "", &instrumentHandle, NULL));
 RFmxCheckWarn(RFmxBT_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
 RFmxCheckWarn(RFmxBT_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
 RFmxCheckWarn(RFmxBT_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeSlope, IQPowerEdgeLevel, triggerDelay,
  minimumQuiteTimeMode, minimumQuietTime, IQPowerEdgeLevelType, enableTrigger));
 RFmxCheckWarn(RFmxBT_CfgPacketType(instrumentHandle, "", packetType));
 RFmxCheckWarn(RFmxBT_CfgDataRate(instrumentHandle, "", LEDataRate));
 RFmxCheckWarn(RFmxBT_SetChannelSoundingPacketFormat(instrumentHandle, "", channelSoundingPacketFormat));
 RFmxCheckWarn(RFmxBT_SetChannelSoundingSyncSequence(instrumentHandle, "", channelSoundingSyncSequence));
 RFmxCheckWarn(RFmxBT_SetChannelSoundingPhaseMeasurementPeriod(instrumentHandle, "", channelSoundingPhaseMeasurmentPeriod));
 RFmxCheckWarn(RFmxBT_SetChannelSoundingToneExtensionSlot(instrumentHandle, "", channelSoundingToneExtensionSlot));
 RFmxCheckWarn(RFmxBT_SelectMeasurements(instrumentHandle, "", RFMXBT_VAL_POWERRAMP, RFMXBT_VAL_TRUE));
 RFmxCheckWarn(RFmxBT_PowerRampCfgBurstSynchronizationType(instrumentHandle, "", RFMXBT_VAL_POWERRAMP_BURST_SYNCHRONIZATION_TYPE_PREAMBLE));
 RFmxCheckWarn(RFmxBT_PowerRampCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
 RFmxCheckWarn(RFmxBT_Initiate(instrumentHandle, "", ""));

 /* Fetch Results */
 RFmxCheckWarn(RFmxBT_PowerRampGetResultsRiseTimeMean(instrumentHandle, "", &riseTimeMean));
 RFmxCheckWarn(RFmxBT_PowerRampGetResultsFallTimeMean(instrumentHandle, "", &fallTimeMean));
 RFmxCheckWarn(RFmxBT_PowerRampGetResults40dBFallTimeMean(instrumentHandle, "", &fortydBFallTimeMean));

 printf("----------------------PowerRamp-----------------------\n");
 printf("Rise Time Mean (s)                   : %lf\n", riseTimeMean);
 printf("Fall Time Mean (s)                   : %lf\n", fallTimeMean);
 printf("40dB Fall Time Mean (s)              : %lf\n\n", fortydBFallTimeMean);

Error:
 if (error)
 {
  RFmxBT_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
  if (error < 0)
   printf("ERROR: %s\n", errorMessage);
  else
   printf("WARNING: %s\n", errorMessage);
 }

 if (instrumentHandle)
 {
  RFmxBT_Close(instrumentHandle, RFMXBT_VAL_FALSE);
 }

 printf("Press any key to exit\n");
 _getch();

 return error;
}