//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Packet Type.
//6. Configure Data Rate.
//7. Configure Channel Sounding properties (CS Packet Format, CS Sync Sequence, CS Phase Measurement Period, CS Tone Extension Slot).
//8. Select ModAcc measurement and enable Traces.
//9. Configure ModAcc Burst Synchronization Type.
//10. Configure Averaging Parameters for ModAcc measurement.
//11. Initiate the Measurement.
//12. Fetch ModAcc Measurements and Trace.
//13. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxBT.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char *argv[])
{
    char *resourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle = NULL;

    char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
    int32 error = 0, lastErrorCode = 0;

    char * frequencyReferenceSource = RFMXBT_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;                                  /*(Hz) */

    float64 centerFrequency = 2.402000e9;                                        /*(Hz) */
    float64 referenceLevel = 0.0;                                                /*(dBm) */
    float64 externalAttenuation = 0.0;                                           /*(dB) */

    int32 enableTrigger = RFMXBT_VAL_TRUE;
    float64 IQPowerEdgeLevel = -20.0;                                            /*dB */
    int32 minimumQuietTimeMode = RFMXBT_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
    float64 minimumQuietTime = 100e-6;                                           /*seconds */
    float64 triggerDelay = 0.0;                                                  /*seconds */

    int32 packetType = RFMXBT_VAL_PACKET_TYPE_LE_CS;
    int32 LEDataRate = 1000000;                                                  /*bps */

    int channelSoundingPacketFormat = RFMXBT_VAL_CHANNEL_SOUNDING_PACKET_FORMAT_SYNC;
    int channelSoundingSyncSequence = RFMXBT_VAL_CHANNEL_SOUNDING_SYNC_SEQUENCE_NONE;
    float64 channelSoundingPhaseMeasurmentPeriod = 10e-6;
    int channelSoundingToneExtensionSlot = RFMXBT_VAL_CHANNEL_SOUNDING_TONE_EXTENSION_SLOT_DISABLED;

    int32 averagingEnabled = RFMXBT_VAL_MODACC_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;

    float64 timeout = 10.0;                                                       /*seconds */

    float64 peakFrequencyErrorMaximum = 0;                                        /*(Hz) */
    float64 initialFrequencyDriftMaximum = 0;                                     /*(Hz) */
    float64 peakFrequencyDriftMaximum = 0;                                        /*(Hz) */
    float64 peakFrequencyDriftRateMaximum = 0;                                    /*(Hz) */

    float64 clockDriftMean = 0;                                                   /*ppm*/
    float64 preambleStartTimeMean = 0;                                            /*seconds*/

    int32 actualArraySize = 0;

    float64 x0 = 0.0, dx = 0.0;
    float32* CSDetrendedPhase = NULL;                                             /*(deg)*/
    float32* CSToneAmplitude = NULL;                                              /*(dBm)*/
    float32* CSTonePhase = NULL;                                                  /*(deg)*/

    /* Initialize a session */
    RFmxCheckWarn(RFmxBT_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxBT_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxBT_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxBT_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXBT_VAL_IQ_POWER_EDGE_RISING_SLOPE, IQPowerEdgeLevel, triggerDelay,
        minimumQuietTimeMode, minimumQuietTime, RFMXBT_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, enableTrigger));
    RFmxCheckWarn(RFmxBT_CfgPacketType(instrumentHandle, "", packetType));
    RFmxCheckWarn(RFmxBT_CfgDataRate(instrumentHandle, "", LEDataRate));
    RFmxCheckWarn(RFmxBT_SetChannelSoundingPacketFormat(instrumentHandle, "", channelSoundingPacketFormat));
    RFmxCheckWarn(RFmxBT_SetChannelSoundingSyncSequence(instrumentHandle, "", channelSoundingSyncSequence));
    RFmxCheckWarn(RFmxBT_SetChannelSoundingPhaseMeasurementPeriod(instrumentHandle, "", channelSoundingPhaseMeasurmentPeriod));
    RFmxCheckWarn(RFmxBT_SetChannelSoundingToneExtensionSlot(instrumentHandle, "", channelSoundingToneExtensionSlot));
    RFmxCheckWarn(RFmxBT_SelectMeasurements(instrumentHandle, "", RFMXBT_VAL_MODACC, RFMXBT_VAL_TRUE));
    RFmxCheckWarn(RFmxBT_ModAccCfgBurstSynchronizationType(instrumentHandle, "", RFMXBT_VAL_MODACC_BURST_SYNCHRONIZATION_TYPE_PREAMBLE));
    RFmxCheckWarn(RFmxBT_ModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
    RFmxCheckWarn(RFmxBT_Initiate(instrumentHandle, "", ""));

    /* Fetch Results */
    RFmxCheckWarn(RFmxBT_ModAccGetResultsClockDriftMean(instrumentHandle, "", &clockDriftMean));
    RFmxCheckWarn(RFmxBT_ModAccGetResultsPreambleStartTimeMean(instrumentHandle, "", &preambleStartTimeMean));
    RFmxCheckWarn(RFmxBT_ModAccFetchFrequencyErrorLE(instrumentHandle, "", timeout, &peakFrequencyErrorMaximum,
        &initialFrequencyDriftMaximum, &peakFrequencyDriftMaximum, &peakFrequencyDriftRateMaximum));
    actualArraySize = 0;
    RFmxCheckWarn(RFmxBT_ModAccFetchCSDetrendedPhaseTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));

    actualArraySize = 0;
    RFmxCheckWarn(RFmxBT_ModAccFetchCSToneTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, 0, &actualArraySize));
    if (actualArraySize > 0) {
        CSToneAmplitude = (float32*)malloc(sizeof(float32) * actualArraySize);
        CSTonePhase = (float32*)malloc(sizeof(float32) * actualArraySize);
        if (CSToneAmplitude && CSTonePhase) {
            RFmxCheckWarn(RFmxBT_ModAccFetchCSToneTrace(instrumentHandle, "", timeout, &x0, &dx, CSToneAmplitude, CSTonePhase, actualArraySize, NULL));
        } else {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    if (actualArraySize > 0)
    {
        CSDetrendedPhase = (float32*)malloc(sizeof(float32) * actualArraySize);
        if (CSDetrendedPhase)
        {
            RFmxCheckWarn(RFmxBT_ModAccFetchCSDetrendedPhaseTrace(instrumentHandle, "", timeout, &x0, &dx,
                CSDetrendedPhase, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    printf("------------------ Channel Sounding Measurement ------------------\n");
    printf("Peak Frequency Error Maximum (Hz)                : %lf\n", peakFrequencyErrorMaximum);
    printf("Clock Drift Mean (ppm)                           : %lf\n", clockDriftMean);
    printf("Preamble Start Time Mean (seconds)               : %lf\n", preambleStartTimeMean);

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

    /* Free allocated memory */
    if (CSDetrendedPhase)
    {
       free(CSDetrendedPhase);
    }
    if (CSToneAmplitude)
    {
       free(CSToneAmplitude);
    }
    if (CSTonePhase)
    {
       free(CSTonePhase);
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}