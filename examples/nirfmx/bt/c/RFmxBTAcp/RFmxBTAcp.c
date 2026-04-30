//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Packet Type.
//6. Configure Data Rate.
//7. Configure Payload Length.
//8. Select ACP measurement and enable Traces.
//9. Configure ACP Burst Sync Type.
//10. Configure Averaging Parameters for ACP measurement.
//11. Configure Number of Offsets or Channel Number depending on Offset Channel Mode.
//12. Initiate the Measurement.
//13. Fetch ACP Measurements and Trace.
//14. Close RFmx Session.

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

    int32 packetType = RFMXBT_VAL_PACKET_TYPE_DH1;
    int32 LEDataRate = 1000000;                                                  /*(bps) */

    int32 payloadLengthMode = RFMXBT_VAL_PAYLOAD_LENGTH_MODE_AUTO;
    int32 payloadLength = 10;                                                     /*(bytes) */

    int32 burstSynchronizationType = RFMXBT_VAL_ACP_BURST_SYNCHRONIZATION_TYPE_PREAMBLE;

    uInt32 measurements = RFMXBT_VAL_ACP;
    int32 enableAllTraces = RFMXBT_VAL_TRUE;

    int32 averagingEnabled = RFMXBT_VAL_ACP_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;

    int32 numberOfOffsets = 5;
    int32 offsetChannelMode = RFMXBT_VAL_ACP_OFFSET_CHANNEL_MODE_SYMMETRIC;
    int32 channelNumber = 0;

    float64 timeout = 10.0;                                                      /*(seconds) */

    int32 measurementStatus = 0;
    char *measurementStatusString;
    float64 referenceChannelPower = 0.0;                                         /*(dBm) */

    float64* lowerAbsolutePower = NULL;                                          /*(dBm) */
    float64* upperAbsolutePower = NULL;                                          /*(dBm) */
    float64* lowerRelativePower = NULL;                                          /*(dB) */
    float64* upperRelativePower = NULL;                                          /*(dB) */
    float64* lowerMargin = NULL;                                                 /*(dB) */
    float64* upperMargin = NULL;                                                 /*(dB) */

    float64 x0 = 0.0, dx = 0.0;
    float32* limitWithExceptionMask = NULL;
    float32* limitWithoutExceptionMask = NULL;

    float32* absolutePower = NULL;

    float32* spectrum = NULL;

    int32 actualArraySize = 0;
    int32 offsetMeasurementArraySize = 0;

    int32 i = 0;

    /* Initialize a session */
    RFmxCheckWarn(RFmxBT_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxBT_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxBT_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxBT_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeSlope, IQPowerEdgeLevel, triggerDelay,
        minimumQuiteTimeMode, minimumQuietTime, IQPowerEdgeLevelType, enableTrigger));
    RFmxCheckWarn(RFmxBT_CfgPacketType(instrumentHandle, "", packetType));
    RFmxCheckWarn(RFmxBT_CfgDataRate(instrumentHandle, "", LEDataRate));
    RFmxCheckWarn(RFmxBT_CfgPayloadLength(instrumentHandle, "", payloadLengthMode, payloadLength));
    RFmxCheckWarn(RFmxBT_SelectMeasurements(instrumentHandle, "", measurements, enableAllTraces));
    RFmxCheckWarn(RFmxBT_ACPCfgBurstSynchronizationType(instrumentHandle, "", burstSynchronizationType));
    RFmxCheckWarn(RFmxBT_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
    RFmxCheckWarn(RFmxBT_ACPCfgOffsetChannelMode(instrumentHandle, "", offsetChannelMode));
    if (offsetChannelMode == RFMXBT_VAL_ACP_OFFSET_CHANNEL_MODE_SYMMETRIC)
    {
        RFmxCheckWarn(RFmxBT_ACPCfgNumberOfOffsets(instrumentHandle, "", numberOfOffsets));
    }
    else if (offsetChannelMode == RFMXBT_VAL_ACP_OFFSET_CHANNEL_MODE_INBAND)
    {
        RFmxCheckWarn(RFmxBT_CfgChannelNumber(instrumentHandle, "", channelNumber));
    }
    RFmxCheckWarn(RFmxBT_Initiate(instrumentHandle, "", ""));

    /* Fetch Results */
    RFmxCheckWarn(RFmxBT_ACPFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));
    if (measurementStatus == RFMXBT_VAL_ACP_RESULTS_MEASUREMENT_STATUS_NOT_APPLICABLE)
        measurementStatusString = "Not Applicable";
    else if (measurementStatus == RFMXBT_VAL_ACP_RESULTS_MEASUREMENT_STATUS_PASS)
        measurementStatusString = "Pass";
    else
        measurementStatusString = "Fail";

    RFmxCheckWarn(RFmxBT_ACPFetchReferenceChannelPower(instrumentHandle, "", timeout, &referenceChannelPower));

    actualArraySize = 0;
    RFmxCheckWarn(RFmxBT_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
        offsetMeasurementArraySize = actualArraySize;
        lowerAbsolutePower = (float64*)malloc(sizeof(float64) * actualArraySize);
        upperAbsolutePower = (float64*)malloc(sizeof(float64) * actualArraySize);
        lowerRelativePower = (float64*)malloc(sizeof(float64) * actualArraySize);
        upperRelativePower = (float64*)malloc(sizeof(float64) * actualArraySize);
        lowerMargin = (float64*)malloc(sizeof(float64) * actualArraySize);
        upperMargin = (float64*)malloc(sizeof(float64) * actualArraySize);
        if (lowerAbsolutePower && upperAbsolutePower && lowerRelativePower && upperRelativePower && lowerMargin && upperMargin)
        {
            RFmxCheckWarn(RFmxBT_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, lowerAbsolutePower, upperAbsolutePower,
                lowerRelativePower, upperRelativePower, lowerMargin, upperMargin, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    actualArraySize = 0;
    RFmxCheckWarn(RFmxBT_ACPFetchMaskTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
        limitWithExceptionMask = (float32*)malloc(sizeof(float32) * actualArraySize);
        limitWithoutExceptionMask = (float32*)malloc(sizeof(float32) * actualArraySize);
        if (limitWithExceptionMask && limitWithoutExceptionMask)
        {
            RFmxCheckWarn(RFmxBT_ACPFetchMaskTrace(instrumentHandle, "", timeout, &x0, &dx, limitWithExceptionMask, limitWithoutExceptionMask,
                actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    actualArraySize = 0;
    x0 = 0.0;
    dx = 0.0;
    RFmxCheckWarn(RFmxBT_ACPFetchAbsolutePowerTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
        absolutePower = (float32*)malloc(sizeof(float32) * actualArraySize);
        if (absolutePower)
        {
            RFmxCheckWarn(RFmxBT_ACPFetchAbsolutePowerTrace(instrumentHandle, "", timeout, &x0, &dx, absolutePower,
                actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    actualArraySize = 0;
    x0 = 0.0;
    dx = 0.0;
    RFmxCheckWarn(RFmxBT_ACPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
        spectrum = (float32*)malloc(sizeof(float32) * actualArraySize);
        if (spectrum)
        {
            RFmxCheckWarn(RFmxBT_ACPFetchAbsolutePowerTrace(instrumentHandle, "", timeout, &x0, &dx, spectrum,
                actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    printf("------------------ACP------------------\n");
    printf("Measurement Status                   : %s\n", measurementStatusString);
    printf("Reference Channel Power (dBm)        : %lf\n\n", referenceChannelPower);
    printf("------------------Offset Measuremensts------------------\n");
    for (i = 0; i < offsetMeasurementArraySize; i++)
    {
        printf("Offset %d\n", i);
        printf("Lower Absolute Powers (dBm)          : %lf\n", lowerAbsolutePower[i]);
        printf("Upper Absolute Powers (dBm)          : %lf\n", upperAbsolutePower[i]);
        printf("Lower Relative Powers (dB)           : %lf\n", lowerRelativePower[i]);
        printf("Upper Relative Powers (dB)           : %lf\n", upperRelativePower[i]);
        printf("Lower Margin (dB)                    : %lf\n", lowerMargin[i]);
        printf("Upper Margin (dB)                    : %lf\n\n", upperMargin[i]);
    }

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
    if (lowerAbsolutePower) 
    {
        free(lowerAbsolutePower);
    }
    if (upperAbsolutePower) 
    {
        free(upperAbsolutePower);
    }
    if (lowerRelativePower) 
    {
        free(lowerRelativePower);
    }
    if (upperRelativePower) 
    {
        free(upperRelativePower);
    }
    if (lowerMargin) 
    {
        free(lowerMargin);
    }
    if (upperMargin) 
    {
        free(upperMargin);
    }
    if (limitWithExceptionMask) 
    {
        free(limitWithExceptionMask);
    }
    if (limitWithoutExceptionMask) 
    {
        free(limitWithoutExceptionMask);
    }
    if (absolutePower) 
    {
        free(absolutePower);
    }
    if (spectrum) 
    {
        free(spectrum);
    }
    printf("Press any key to exit\n");
    _getch();

    return error;
}