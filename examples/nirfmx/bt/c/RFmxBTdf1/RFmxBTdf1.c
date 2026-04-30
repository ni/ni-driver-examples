//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Packet Type.
//6. Configure Data Rate.
//7. Configure Payload Bit pattern.
//8. Configure Payload Length.
//9. Configure Direction Finding.
//10. Select ModAcc measurement and enable Traces.
//11. Configure ModAcc Burst Synchronization Type.
//12. Configure Averaging Parameters for ModAcc measurement.
//13. Initiate the Measurement.
//14. Fetch ModAcc Measurements and Trace.
//15. Close RFmx Session.

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
    int32 IQPowerEdgeSlope = RFMXBT_VAL_IQ_POWER_EDGE_RISING_SLOPE;
    float64 IQPowerEdgeLevel = -20.0;                                            /*dB */
    int32 minimumQuiteTimeMode = RFMXBT_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
    float64 minimumQuietTime = 100e-6;                                           /*seconds */
    int32 IQPowerEdgeLevelType = RFMXBT_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
    float64 triggerDelay = 0.0;                                                  /*seconds */

    int32 packetType = RFMXBT_VAL_PACKET_TYPE_DH1;
    int32 LEDataRate = 1000000;                                                  /*bps */

    int32 payloadBitPattern = RFMXBT_VAL_PAYLOAD_BIT_PATTERN_11110000;
    int32 payloadLengthMode = RFMXBT_VAL_PAYLOAD_LENGTH_MODE_AUTO;
    int32 payloadLength = 10;                                                     /*bytes */

    int32 directionFindingMode = RFMXBT_VAL_DIRECTION_FINDING_MODE_DISABLED;
    float64 CTELength = 160e-6;                                                   /*seconds */
    float64 CTESlotDuration = 1e-6;                                               /*seconds */

    int32 burstSynchronizationType = RFMXBT_VAL_MODACC_BURST_SYNCHRONIZATION_TYPE_PREAMBLE;

    uInt32 measurements = RFMXBT_VAL_MODACC;
    int32 enableAllTraces = RFMXBT_VAL_TRUE;

    int32 averagingEnabled = RFMXBT_VAL_MODACC_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;

    float64 timeout = 10.0;                                                       /*seconds */
    float64 df1avgMaximum = 0;                                                    /*(Hz) */
    float64 df1avgMinimum = 0;                                                    /*(Hz) */

    float64 peakFrequencyErrorMaximum = 0;                                        /*(Hz) */
    float64 initialFrequencyDriftMaximum = 0;                                     /*(Hz) */
    float64 peakFrequencyDriftMaximum = 0;                                        /*(Hz) */
    float64 peakFrequencyDriftRateMaximum = 0;                                    /*(Hz) */

    int32 actualArraySize = 0;

    float32* time = NULL;
    float32* df1max = NULL;
    float32* timeLE = NULL;
    float32* frequencyErrorLE = NULL;

    /* Initialize a session */
    RFmxCheckWarn(RFmxBT_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxBT_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxBT_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxBT_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeSlope, IQPowerEdgeLevel, triggerDelay,
        minimumQuiteTimeMode, minimumQuietTime, IQPowerEdgeLevelType, enableTrigger));
    RFmxCheckWarn(RFmxBT_CfgPacketType(instrumentHandle, "", packetType));
    RFmxCheckWarn(RFmxBT_CfgDataRate(instrumentHandle, "", LEDataRate));
    RFmxCheckWarn(RFmxBT_CfgPayloadBitPattern(instrumentHandle, "", payloadBitPattern));
    RFmxCheckWarn(RFmxBT_CfgPayloadLength(instrumentHandle, "", payloadLengthMode, payloadLength));
    RFmxCheckWarn(RFmxBT_CfgLEDirectionFinding(instrumentHandle, "", directionFindingMode, CTELength, CTESlotDuration));
    RFmxCheckWarn(RFmxBT_SelectMeasurements(instrumentHandle, "", measurements, enableAllTraces));
    RFmxCheckWarn(RFmxBT_ModAccCfgBurstSynchronizationType(instrumentHandle, "", burstSynchronizationType));
    RFmxCheckWarn(RFmxBT_ModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
    RFmxCheckWarn(RFmxBT_Initiate(instrumentHandle, "", ""));

    /* Fetch Results */
    RFmxCheckWarn(RFmxBT_ModAccFetchDf1(instrumentHandle, "", timeout, &df1avgMaximum, &df1avgMinimum));
    RFmxCheckWarn(RFmxBT_ModAccFetchFrequencyErrorLE(instrumentHandle, "", timeout, &peakFrequencyErrorMaximum,
        &initialFrequencyDriftMaximum, &peakFrequencyDriftMaximum, &peakFrequencyDriftRateMaximum));
    actualArraySize = 0;
    RFmxCheckWarn(RFmxBT_ModAccFetchDf1maxTrace(instrumentHandle, "", timeout, NULL, NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
        time = (float32*)malloc(sizeof(float32) * actualArraySize);
        df1max = (float32*)malloc(sizeof(float32) * actualArraySize);
        if (df1max && time)
        {
            RFmxCheckWarn(RFmxBT_ModAccFetchDf1maxTrace(instrumentHandle, "", timeout, time, df1max,
                actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    actualArraySize = 0;
    RFmxCheckWarn(RFmxBT_ModAccFetchFrequencyErrorTraceLE(instrumentHandle, "", timeout, NULL, NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
        timeLE = (float32*)malloc(sizeof(float32) * actualArraySize);
        frequencyErrorLE = (float32*)malloc(sizeof(float32) * actualArraySize);
        if (timeLE && frequencyErrorLE)
        {
            RFmxCheckWarn(RFmxBT_ModAccFetchFrequencyErrorTraceLE(instrumentHandle, "", timeout, timeLE, frequencyErrorLE,
                actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    printf("------------------df1 Measurement------------------\n");
    printf("df1avg Maximum (Hz)                             : %lf\n", df1avgMaximum);
    printf("df1avg Minimum (Hz)                             : %lf\n", df1avgMinimum);

    printf("------------------LE Frequency Error------------------\n");
    printf("Peak Frequency Error Maximum (Hz)               : %lf\n", peakFrequencyErrorMaximum);
    printf("Initial Frequency Drift Maximum  (Hz)           : %lf\n", initialFrequencyDriftMaximum);
    printf("Peak Frequency Drift Maximum (Hz)               : %lf\n", peakFrequencyDriftMaximum);
    printf("Peak Frequency Drift RateMaximum  (Hz)          : %lf\n", peakFrequencyDriftRateMaximum);

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
    if (df1max)
    {
        free(df1max);
    }
    if (time)
    {
        free(time);
    }
    if (timeLE)
    {
        free(timeLE);
    }
    if (frequencyErrorLE)
    {
        free(frequencyErrorLE);
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}