//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Packet Type.
//6. Configure Payload Length.
//7. Select ModAcc measurement and enable Traces.
//8. Configure ModAcc Burst Synchronization Mode.
//9. Configure Averaging Parameters for ModAcc measurement.
//10. Initiate the Measurement.
//11 .Fetch ModAcc Measurements and Trace.
//12. Close RFmx Session.

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

    int32 burstSynchronizationType = RFMXBT_VAL_MODACC_BURST_SYNCHRONIZATION_TYPE_PREAMBLE;

    uInt32 measurements = RFMXBT_VAL_MODACC;
    int32 enableAllTraces = RFMXBT_VAL_TRUE;

    int32 averagingEnabled = RFMXBT_VAL_MODACC_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;

    float64 timeout = 10.0;                                                      /*(seconds) */
    float64 peakRMSDEVMMaximum = 0.0;                                            /*(%) */
    float64 peakDEVMMaximum = 0.0;                                               /*(%) */
    float64 ninetyninePercentDEVM = 0.0;                                         /*(%) */

    float64 headerFrequencyErrorWiMaximum = 0.0;                                 /*(Hz) */
    float64 peakFrequencyErrorWiPlusW0Maximum = 0.0;                             /*(Hz) */
    float64 peakFrequencyErrorW0Maximum = 0.0;                                   /*(Hz) */

    int32 actualArraySize = 0;

    float32* time = NULL;
    float32* frequencyErrorWiPlusW0 = NULL;

    float32* DEVMPerSymbol = NULL;
    NIComplexSingle *constellation = NULL;

    /* Initialize a session */
    RFmxCheckWarn(RFmxBT_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxBT_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxBT_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxBT_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeSlope, IQPowerEdgeLevel, triggerDelay,
        minimumQuiteTimeMode, minimumQuietTime, IQPowerEdgeLevelType, enableTrigger));
    RFmxCheckWarn(RFmxBT_CfgPacketType(instrumentHandle, "", packetType));
    RFmxCheckWarn(RFmxBT_CfgPayloadLength(instrumentHandle, "", payloadLengthMode, payloadLength));
    RFmxCheckWarn(RFmxBT_SelectMeasurements(instrumentHandle, "", measurements, enableAllTraces));
    RFmxCheckWarn(RFmxBT_ModAccCfgBurstSynchronizationType(instrumentHandle, "", burstSynchronizationType));
    RFmxCheckWarn(RFmxBT_ModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
    RFmxCheckWarn(RFmxBT_Initiate(instrumentHandle, "", ""));

    /* Fetch Results */
    RFmxCheckWarn(RFmxBT_ModAccFetchDEVM(instrumentHandle, "", timeout, &peakRMSDEVMMaximum, &peakDEVMMaximum, &ninetyninePercentDEVM));
    RFmxCheckWarn(RFmxBT_ModAccFetchFrequencyErrorEDR(instrumentHandle, "", timeout, &headerFrequencyErrorWiMaximum, &peakFrequencyErrorWiPlusW0Maximum,
        &peakFrequencyErrorW0Maximum));
    actualArraySize = 0;
    RFmxCheckWarn(RFmxBT_ModAccFetchDEVMPerSymbolTrace(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
        DEVMPerSymbol = (float32*)malloc(sizeof(float32) * actualArraySize);
        if (DEVMPerSymbol)
        {
            RFmxCheckWarn(RFmxBT_ModAccFetchDEVMPerSymbolTrace(instrumentHandle, "", timeout, DEVMPerSymbol,
                actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    actualArraySize = 0;
    RFmxCheckWarn(RFmxBT_ModAccFetchFrequencyErrorWiPlusW0TraceEDR(instrumentHandle, "", timeout, NULL, NULL, 0,
        &actualArraySize));
    if (actualArraySize > 0)
    {
        time = (float32*)malloc(sizeof(float32) * actualArraySize);
        frequencyErrorWiPlusW0 = (float32*)malloc(sizeof(float32) * actualArraySize);
        if (time && frequencyErrorWiPlusW0)
        {
            RFmxCheckWarn(RFmxBT_ModAccFetchFrequencyErrorWiPlusW0TraceEDR(instrumentHandle, "", timeout, time,
                frequencyErrorWiPlusW0, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    actualArraySize = 0;
    RFmxCheckWarn(RFmxBT_ModAccFetchConstellationTrace(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
        constellation = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
        if (constellation)
        {
            RFmxCheckWarn(RFmxBT_ModAccFetchConstellationTrace(instrumentHandle, "", timeout, (NIComplexSingle*)constellation,
                actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    printf("------------------DEVM------------------\n");
    printf("Peak RMS DEVM Maximum (%%)                 : %lf\n", peakRMSDEVMMaximum);
    printf("Peak DEVM Maximum (%%)                     : %lf\n", peakDEVMMaximum);
    printf("99%% DEVM (%%)                                : %lf\n", ninetyninePercentDEVM);
    printf("\n------------------EDR Frequency Error------------------\n");
    printf("Header Frequency Error wi Maximum (Hz)    : %lf\n", headerFrequencyErrorWiMaximum);
    printf("Peak Frequency Error wi+w0 Maximum (Hz)   : %lf\n", peakFrequencyErrorWiPlusW0Maximum);
    printf("Peak Frequency Error w0 Maximum (Hz)      : %lf\n", peakFrequencyErrorW0Maximum);

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
    if (DEVMPerSymbol)
    {
        free(DEVMPerSymbol);
    }
    if (constellation)
    {
        free(constellation);
    }
    if (time)
    {
        free(time);
    }
    if (frequencyErrorWiPlusW0)
    {
        free(frequencyErrorWiPlusW0);
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}