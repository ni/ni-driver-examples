//Steps:
//1. Open a new RFmx session.
//2. Configure the frequency reference properties (Clock Source and Clock Frequency).
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
//5. Configure Standard to 802.11b.
//6. Select DSSSModAcc measurement and enable the traces.
//7. Configure Measurement Length.
//8. Configure Pulse Shaping Filter Type and Parameter.
//9. Configure EVM unit.
//10. Configure Averaging parameters.
//11. Initiate Measurement.
//12. Fetch DSSSModAcc Traces and Measurements.
//13. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWLAN.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char *argv[])
{
    char *resourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle;

    char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
    int32 error = 0, lastErrorCode = 0;

    char * frequencyReferenceSource = RFMXWLAN_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;                                  /*(Hz) */

    float64 centerFrequency = 2.412e9;                                           /*(Hz) */
    float64 referenceLevel = 0.0;                                                /*(dBm) */
    float64 externalAttenuation = 0.0;                                           /*(dB) */

    int32 IQPowerEdgeEnabled = RFMXWLAN_VAL_TRUE;
    int32 IQPowerEdgeSlope = RFMXWLAN_VAL_IQ_POWER_EDGE_RISING_SLOPE;
    float64 IQPowerEdgeLevel = -20.0;                                            /*(dB) */
    int32 minimumQuiteTimeMode = RFMXWLAN_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
    float64 minimumQuietTime = 5e-6;                                             /*(seconds) */
    int32 IQPowerEdgeLevelType = RFMXWLAN_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
    float64 triggerDelay = 0.0;                                                  /*(seconds) */

    int32 standard = RFMXWLAN_VAL_STANDARD_802_11_B;

    int32 measurementOffset = 0;                                                 /*(chips)*/
    int32 maximumMeasurementLength = 1000;                                       /*(chips)*/

    int32 pulseShapingFilterType = RFMXWLAN_VAL_DSSSMODACC_PULSE_SHAPING_FILTER_TYPE_RECTANGULAR;
    float64 pulseShapingFilterParameter = 0.50;

    int32 evmUnit = RFMXWLAN_VAL_DSSSMODACC_EVM_UNIT_PERCENTAGE;

    uInt32 measurements = RFMXWLAN_VAL_DSSSMODACC;
    int32 enableAllTraces = RFMXWLAN_VAL_TRUE;

    int32 averagingEnabled = RFMXWLAN_VAL_DSSSMODACC_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;

    float64 timeout = 10.0;                                                      /*(seconds) */

    float64 RMSEVMMean = 0.0;                                                    /*(% or dB) */
    float64 peakEVM80211_2016Maximum = 0.0;                                      /*(% or dB) */
    float64 peakEVM80211_2007Maximum = 0.0;                                      /*(% or dB) */
    float64 peakEVM80211_1999Maximum = 0.0;                                      /*(% or dB) */
    float64 frequencyErrorMean = 0.0;                                            /*(Hz) */
    float64 chipClockErrorMean = 0.0;                                            /*(ppm) */
    int32 numberOfChipsUsed = 0;

    int32 dataModulationFormat = RFMXWLAN_VAL_DSSSMODACC_DATA_MODULATION_FORMAT_DSSS1MBPS;
    int32 payloadLength = 0;                                                     /*(byte) */
    int32 preambleType = RFMXWLAN_VAL_DSSSMODACC_PREAMBLE_TYPE_LONG;
    int32 lockedClocksBit = 0;
    int32 headerCRCStatus = 0;
    int32 PSDUCRCStatus = 0;

    float64 IQOriginOffsetMean = 0.0;                                            /*(dB) */
    float64 IQGainImbalanceMean = 0.0;                                           /*(dB) */
    float64 IQQuadratureErrorMean = 0.0;                                         /*(deg) */

    int32 actualArraySize = 0;
    float64 x0 = 0, dx = 0;
    float32* EVMPerChipMean = NULL;

    NIComplexSingle* constellation = NULL;

    /* Initialize a session */
    RFmxCheckWarn(RFmxInstr_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxInstr_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxWLAN_CfgFrequency(instrumentHandle, "", centerFrequency));
    RFmxCheckWarn(RFmxWLAN_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
    RFmxCheckWarn(RFmxWLAN_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
    RFmxCheckWarn(RFmxWLAN_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeSlope, IQPowerEdgeLevel, triggerDelay,
        minimumQuiteTimeMode, minimumQuietTime, IQPowerEdgeLevelType, IQPowerEdgeEnabled));
    RFmxCheckWarn(RFmxWLAN_CfgStandard(instrumentHandle, "", standard));
    RFmxCheckWarn(RFmxWLAN_SelectMeasurements(instrumentHandle, "", measurements, enableAllTraces));
    RFmxCheckWarn(RFmxWLAN_DSSSModAccCfgMeasurementLength(instrumentHandle, "", measurementOffset, maximumMeasurementLength));
    RFmxCheckWarn(RFmxWLAN_DSSSModAccSetPulseShapingFilterType(instrumentHandle, "", pulseShapingFilterType));
    RFmxCheckWarn(RFmxWLAN_DSSSModAccSetPulseShapingFilterParameter(instrumentHandle, "", pulseShapingFilterParameter));
    RFmxCheckWarn(RFmxWLAN_DSSSModAccCfgEVMUnit(instrumentHandle, "", evmUnit));
    RFmxCheckWarn(RFmxWLAN_DSSSModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
    RFmxCheckWarn(RFmxWLAN_Initiate(instrumentHandle, "", ""));

    /* Fetch Results */
    RFmxCheckWarn(RFmxWLAN_DSSSModAccFetchEVM(instrumentHandle, "", timeout, &RMSEVMMean, &peakEVM80211_2016Maximum,
        &peakEVM80211_2007Maximum, &peakEVM80211_1999Maximum, &frequencyErrorMean, &chipClockErrorMean,
        &numberOfChipsUsed));
    RFmxCheckWarn(RFmxWLAN_DSSSModAccFetchPPDUInformation(instrumentHandle, "", timeout, &dataModulationFormat,
        &payloadLength, &preambleType, &lockedClocksBit, &headerCRCStatus, &PSDUCRCStatus));
    RFmxCheckWarn(RFmxWLAN_DSSSModAccFetchIQImpairments(instrumentHandle, "", timeout, &IQOriginOffsetMean,
        &IQGainImbalanceMean, &IQQuadratureErrorMean));

    actualArraySize = 0;
    RFmxCheckWarn(RFmxWLAN_DSSSModAccFetchEVMPerChipMeanTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
        EVMPerChipMean = (float32*)malloc(sizeof(float32) * actualArraySize);
        if(EVMPerChipMean)
        {
            RFmxCheckWarn(RFmxWLAN_DSSSModAccFetchEVMPerChipMeanTrace(instrumentHandle, "", timeout, &x0, &dx, EVMPerChipMean, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }
    actualArraySize = 0;
    RFmxCheckWarn(RFmxWLAN_DSSSModAccFetchConstellationTrace(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
        constellation = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
        if(constellation)
        {
            RFmxCheckWarn(RFmxWLAN_DSSSModAccFetchConstellationTrace(instrumentHandle, "", timeout, constellation, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    printf("------------------EVM------------------\n");
    printf("RMS EVM Mean (%% or dB)                              : %lf\n", RMSEVMMean);
    printf("Peak EVM (802.11-2016) Maximum (%% or dB)            : %lf\n", peakEVM80211_2016Maximum);
    printf("Peak EVM (802.11-2007) Maximum (%% or dB)            : %lf\n", peakEVM80211_2007Maximum);
    printf("Peak EVM (802.11-1999) Maximum (%% or dB)            : %lf\n", peakEVM80211_1999Maximum);
    printf("Number of Chips Used                                : %d\n", numberOfChipsUsed);

    printf("\n---------------Impairments & PPDU Info---------------\n");
    printf("Frequency Error Mean (Hz)                           : %lf\n", frequencyErrorMean);
    printf("Chip Clock Error Mean (ppm)                         : %lf\n", chipClockErrorMean);

    printf("\n------------------IQ Impairments------------------\n");
    printf("I/Q Origin Offset Mean (dB)                         : %lf\n", IQOriginOffsetMean);
    printf("I/Q Gain Imbalance Mean (dB)                        : %lf\n", IQGainImbalanceMean);
    printf("I/Q Quadrature Error Mean (deg)                     : %lf\n", IQQuadratureErrorMean);

    printf("------------------PPDU Information------------------\n");
    switch (dataModulationFormat)
    {
        case 0: printf("Data Modulation Format                              : DSSS1MBPS\n");
            break;
        case 1: printf("Data Modulation Format                              : DSSS2MBPS\n");
            break;
        case 2: printf("Data Modulation Format                              : CCK5.5MBPS\n");
            break;
        case 3: printf("Data Modulation Format                              : CCK11MBPS\n");
            break;
        case 4: printf("Data Modulation Format                              : PBCC5.5MBPS\n");
            break;
        case 5: printf("Data Modulation Format                              : PBCC11MBPS\n");
            break;
        case 6: printf("Data Modulation Format                              : PBCC22MBPS\n");
            break;
        case 7: printf("Data Modulation Format                              : PBCC33MBPS\n");
            break;
    }
    printf("Payload length (bytes)                              : %d\n", payloadLength);
    switch (preambleType)
    {
        case 0: printf("Preamble Type                                       : Long\n");
            break;
        case 1: printf("Preamble Type                                       : Short\n");
            break;
    }
    printf("Locked Clocks Bit                                   : %d\n", lockedClocksBit);

Error:
    if (error)
    {
        RFmxWLAN_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
        if (error < 0)
            printf("ERROR: %s\n", errorMessage);
        else
            printf("WARNING: %s\n", errorMessage);
    }

    if (instrumentHandle)
    {
        RFmxWLAN_Close(instrumentHandle, 0);
    }

    /* Free allocated memory */
    if (EVMPerChipMean)
    {
        free(EVMPerChipMean);
    }
    if (constellation)
    {
        free(constellation);
    }
    printf("Press any key to exit\n");
    _getch();

    return error;
}