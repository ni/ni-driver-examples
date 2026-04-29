//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Packet Type.
//6. Configure Payload Length.
//7. Select ModAcc, ACP and TXP measurements.
//8. Configure Averaging Parameters for ModAcc measurement.
//9. Configure Averaging Parameters for ACP measurement.
//10. Configure Averaging Parameters for TXP measurement.
//11. Configure ACP Number of Offsets.
//12. Configure ACP Offset Channel Mode.
//13. Initiate the Measurement.
//14. Fetch ModAcc measurements.
//15. Fetch ACP measurements.
//16. Fetch TXP measurements.
//17. Close RFmx Session.

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

    int32 packetType = RFMXBT_VAL_PACKET_TYPE_2_DH1;

    int32 payloadLengthMode = RFMXBT_VAL_PAYLOAD_LENGTH_MODE_AUTO;
    int32 payloadLength = 10;                                                     /*(bytes) */

    uInt32 measurements = RFMXBT_VAL_MODACC|RFMXBT_VAL_ACP|RFMXBT_VAL_TXP;
    int32 enableAllTraces = RFMXBT_VAL_FALSE;

    int32 offsetChannelMode = RFMXBT_VAL_ACP_OFFSET_CHANNEL_MODE_SYMMETRIC;
    int32 numberOfOffsets = 5;

    int32 averagingCount = 10;

    float64 timeout = 10.0;                                                      /*(seconds) */

    float64 peakRMSDEVMMaximum = 0.0;                                            /*(%) */
    float64 peakDEVMMaximum = 0.0;                                               /*(%) */
    float64 ninetyninePercentDEVM = 0.0;                                         /*(%) */

    float64 headerFrequencyErrorWiMaximum = 0.0;                                 /*(Hz) */
    float64 peakFrequencyErrorWiPlusW0Maximum = 0.0;                             /*(Hz) */
    float64 peakFrequencyErrorW0Maximum = 0.0;                                   /*(Hz) */

    float64 referenceChannelPower = 0.0;                                         /*(dBm) */

    float64* lowerAbsolutePower = NULL;                                          /*(dBm) */
    float64* upperAbsolutePower = NULL;                                          /*(dBm) */
    float64* lowerRelativePower = NULL;                                          /*(dB) */
    float64* upperRelativePower = NULL;                                          /*(dB) */
    float64* lowerMargin = NULL;                                                 /*(dB) */
    float64* upperMargin = NULL;                                                 /*(dB) */

    float64 averagePowerMean = 0.0;                                              /*(dBm) */
    float64 averagePowerMaximum = 0.0;                                           /*(dBm) */
    float64 averagePowerMinimum = 0.0;                                           /*(dBm) */
    float64 peakToAveragePowerRatioMaximum = 0.0;                                /*(dB) */
    float64 EDRGFSKAveragePowerMean = 0.0;                                       /*(dBm) */
    float64 EDRDPSKAveragePowerMean = 0.0;                                       /*(dBm) */
    float64 EDR_DPSK_GFSKAveragePowerRatioMean = 0.0;                            /*(dB) */

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
    RFmxCheckWarn(RFmxBT_CfgPayloadLength(instrumentHandle, "", payloadLengthMode, payloadLength));
    RFmxCheckWarn(RFmxBT_SelectMeasurements(instrumentHandle, "", measurements, enableAllTraces));
    RFmxCheckWarn(RFmxBT_ModAccCfgAveraging(instrumentHandle, "", RFMXBT_VAL_MODACC_AVERAGING_ENABLED_FALSE, averagingCount));
    RFmxCheckWarn(RFmxBT_ACPCfgAveraging(instrumentHandle, "", RFMXBT_VAL_ACP_AVERAGING_ENABLED_FALSE, averagingCount));
    RFmxCheckWarn(RFmxBT_TXPCfgAveraging(instrumentHandle, "", RFMXBT_VAL_TXP_AVERAGING_ENABLED_FALSE, averagingCount));
    RFmxCheckWarn(RFmxBT_ACPCfgNumberOfOffsets(instrumentHandle, "", numberOfOffsets));
    RFmxCheckWarn(RFmxBT_ACPCfgOffsetChannelMode(instrumentHandle, "", offsetChannelMode));
    RFmxCheckWarn(RFmxBT_Initiate(instrumentHandle, "", ""));

    /* Fetch Results */
    RFmxCheckWarn(RFmxBT_ModAccFetchDEVM(instrumentHandle, "", timeout, &peakRMSDEVMMaximum, &peakDEVMMaximum, &ninetyninePercentDEVM));
    RFmxCheckWarn(RFmxBT_ModAccFetchFrequencyErrorEDR(instrumentHandle, "", timeout, &headerFrequencyErrorWiMaximum, &peakFrequencyErrorWiPlusW0Maximum,
        &peakFrequencyErrorW0Maximum));

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

    RFmxCheckWarn(RFmxBT_TXPFetchPowers(instrumentHandle, "", timeout, &averagePowerMean, &averagePowerMaximum,
        &averagePowerMinimum, &peakToAveragePowerRatioMaximum));
    RFmxCheckWarn(RFmxBT_TXPFetchEDRPowers(instrumentHandle, "", timeout, &EDRGFSKAveragePowerMean, &EDRDPSKAveragePowerMean,
       &EDR_DPSK_GFSKAveragePowerRatioMean));

    printf("------------------ModAcc------------------\n");
    printf("------------------DEVM------------------\n");
    printf("Peak RMS DEVM Maximum (%%)                      : %lf\n", peakRMSDEVMMaximum);
    printf("Peak DEVM Maximum (%%)                          : %lf\n", peakDEVMMaximum);
    printf("99%% DEVM (%%)                                   : %lf\n\n", ninetyninePercentDEVM);
    printf("------------------EDR Frequency Error------------------\n");
    printf("Header Frequency Error wi Maximum (Hz)         : %lf\n", headerFrequencyErrorWiMaximum);
    printf("Peak Frequency Error wi+w0 Maximum (Hz)        : %lf\n", peakFrequencyErrorWiPlusW0Maximum);
    printf("Peak Frequency Error w0 Maximum (Hz)           : %lf\n\n", peakFrequencyErrorW0Maximum);

    printf("------------------ACP------------------\n");
    printf("Reference Channel Power (dBm)                  : %lf\n\n", referenceChannelPower);
    printf("------------------Offset Measuremensts------------------\n");
    for (i = 0; i < offsetMeasurementArraySize; i++)
    {
        printf("Offset %d\n", i);
        printf("Lower Absolute Powers (dBm)                    : %lf\n", lowerAbsolutePower[i]);
        printf("Upper Absolute Powers (dBm)                    : %lf\n", upperAbsolutePower[i]);
        printf("Lower Relative Powers (dB)                     : %lf\n", lowerRelativePower[i]);
        printf("Upper Relative Powers (dB)                     : %lf\n", upperRelativePower[i]);
        printf("Lower Margin (dB)                              : %lf\n", lowerMargin[i]);
        printf("Upper Margin (dB)                              : %lf\n\n", upperMargin[i]);
    }

    printf("------------------TXP------------------\n");
    printf("Average Power Mean (dBm)                       : %lf\n", averagePowerMean);
    printf("Average Power Maximum (dBm)                    : %lf\n", averagePowerMaximum);
    printf("Average Power Minimum (dBm)                    : %lf\n", averagePowerMinimum);
    printf("Peak to Average Power Ratio Maximum (dB)       : %lf\n", peakToAveragePowerRatioMaximum);
    printf("EDR GFSK Average Power Mean (dBm)              : %lf\n", EDRGFSKAveragePowerMean);
    printf("EDR DPSK Average Power Mean (dBm)              : %lf\n", EDRDPSKAveragePowerMean);
    printf("EDR DPSK GFSK Average Power Ratio Mean (dB)    : %lf\n", EDR_DPSK_GFSKAveragePowerRatioMean);

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
    printf("Press any key to exit\n");
    _getch();

    return error;
}