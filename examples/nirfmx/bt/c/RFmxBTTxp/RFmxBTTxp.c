//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties(Center Frequency and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Packet Type.
//6. Configure Data Rate.
//7. Configure Payload Length.
//8. Configure Direction Finding.
//9. Configure Reference Level.
//10. Select TXP measurement and enable Traces.
//11. Configure TXP Burst Synchronization Type.
//12. Configure Averaging Parameters for TXP measurement.
//13. Initiate the Measurement.
//14. Fetch TXP Measurements and Trace.
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
    int32 autoLevel = RFMXBT_VAL_TRUE;
    float64 measurementinterval = 10e-3;                                         /*seconds */

    int32 enableTrigger = RFMXBT_VAL_TRUE;
    int32 IQPowerEdgeSlope = RFMXBT_VAL_IQ_POWER_EDGE_RISING_SLOPE;
    float64 IQPowerEdgeLevel = -20.0;                                            /*(dB) */
    int32 minimumQuiteTimeMode = RFMXBT_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
    float64 minimumQuietTime = 100e-6;                                           /*seconds */
    int32 IQPowerEdgeLevelType = RFMXBT_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
    float64 triggerDelay = 0.0;                                                  /*seconds */

    int32 packetType = RFMXBT_VAL_PACKET_TYPE_DH1;
    int32 LEDataRate = 1000000;                                                  /*bps */

    int32 payloadLengthMode = RFMXBT_VAL_PAYLOAD_LENGTH_MODE_AUTO;
    int32 payloadLength = 10;                                                     /*bytes */

    int32 directionFindingMode = RFMXBT_VAL_DIRECTION_FINDING_MODE_DISABLED;
    float64 CTELength = 160e-6;                                                     /*seconds */
    float64 CTESlotDuration = 1e-6;                                                 /*seconds */

    int32 burstSynchronizationType = RFMXBT_VAL_TXP_BURST_SYNCHRONIZATION_TYPE_PREAMBLE;

    uInt32 measurements = RFMXBT_VAL_TXP;
    int32 enableAllTraces = RFMXBT_VAL_TRUE;

    int32 averagingEnabled = RFMXBT_VAL_TXP_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;

    float64 timeout = 10.0;                                                      /*seconds */
    float64 averagePowerMean = 0.0;                                              /*(dBm) */
    float64 averagePowerMaximum = 0.0;                                           /*(dBm) */
    float64 averagePowerMinimum = 0.0;                                           /*(dBm) */
    float64 peakToAveragePowerRatioMaximum = 0.0;                                /*(dB) */
    float64 EDRGFSKAveragePowerMean = 0.0;                                       /*(dBm) */
    float64 EDRDPSKAveragePowerMean = 0.0;                                       /*(dBm) */
    float64 EDR_DPSK_GFSKAveragePowerRatioMean = 0.0;                            /*(dB) */
    float64 referencePeriodAveragePowerMean = 0.0;                               /*(dBm) */
    float64 referencePeriodPeakAbsolutePowerDeviationMaximum = 0.0;              /*(%) */

    float64 *transmitSlotAveragePowerMean = NULL;                                            /*(dB) */
    float64 *transmitSlotPeakAbsolutePowerDeviationMaximum = NULL;                           /*(dB) */
    int32 transmitSlotAveragePowerMeanSize = 0;

    float64 x0 = 0.0, dx = 0.0;
    float32 *power = NULL;
    int32 actualArraySize = 0;

    int32 i = 0;

    /* Initialize a session */
    RFmxCheckWarn(RFmxBT_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxBT_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxBT_CfgFrequency(instrumentHandle, "", centerFrequency));
    RFmxCheckWarn(RFmxBT_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
    RFmxCheckWarn(RFmxBT_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeSlope, IQPowerEdgeLevel, triggerDelay,
        minimumQuiteTimeMode, minimumQuietTime, IQPowerEdgeLevelType, enableTrigger));
    RFmxCheckWarn(RFmxBT_CfgPacketType(instrumentHandle, "", packetType));
    RFmxCheckWarn(RFmxBT_CfgDataRate(instrumentHandle, "", LEDataRate));
    RFmxCheckWarn(RFmxBT_CfgPayloadLength(instrumentHandle, "", payloadLengthMode, payloadLength));
    RFmxCheckWarn(RFmxBT_CfgLEDirectionFinding(instrumentHandle, "", directionFindingMode, CTELength, CTESlotDuration));
    if (autoLevel)
    {
        RFmxCheckWarn(RFmxBT_AutoLevel(instrumentHandle, "", measurementinterval, &referenceLevel));
    }
    else
    {
        RFmxCheckWarn(RFmxBT_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
    }
    RFmxCheckWarn(RFmxBT_SelectMeasurements(instrumentHandle, "", measurements, enableAllTraces));
    RFmxCheckWarn(RFmxBT_TXPCfgBurstSynchronizationType(instrumentHandle, "", burstSynchronizationType));
    RFmxCheckWarn(RFmxBT_TXPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
    RFmxCheckWarn(RFmxBT_Initiate(instrumentHandle, "", ""));

    /* Fetch Results */
    RFmxCheckWarn(RFmxBT_TXPFetchPowers(instrumentHandle, "", timeout, &averagePowerMean, &averagePowerMaximum,
        &averagePowerMinimum, &peakToAveragePowerRatioMaximum));
    RFmxCheckWarn(RFmxBT_TXPFetchEDRPowers(instrumentHandle, "", timeout, &EDRGFSKAveragePowerMean, &EDRDPSKAveragePowerMean,
       &EDR_DPSK_GFSKAveragePowerRatioMean));
    RFmxCheckWarn(RFmxBT_TXPFetchLECTEReferencePeriodPowers(instrumentHandle, "", timeout, &referencePeriodAveragePowerMean, &referencePeriodPeakAbsolutePowerDeviationMaximum));

    actualArraySize = 0;
    RFmxCheckWarn(RFmxBT_TXPFetchLECTETransmitSlotPowersArray(instrumentHandle, "", timeout, NULL, NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
      transmitSlotAveragePowerMean = (float64*)malloc(sizeof(float64) * actualArraySize);
      transmitSlotPeakAbsolutePowerDeviationMaximum = (float64*)malloc(sizeof(float64) * actualArraySize);
      transmitSlotAveragePowerMeanSize = actualArraySize;
      if (transmitSlotAveragePowerMean)
      {
         RFmxCheckWarn(RFmxBT_TXPFetchLECTETransmitSlotPowersArray(instrumentHandle, "", timeout,
            transmitSlotAveragePowerMean, transmitSlotPeakAbsolutePowerDeviationMaximum, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
    }

    actualArraySize = 0;
    RFmxCheckWarn(RFmxBT_TXPFetchPowerTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
        power = (float32*)malloc(sizeof(float32) * actualArraySize);
        if (power)
        {
            RFmxCheckWarn(RFmxBT_TXPFetchPowerTrace(instrumentHandle, "", timeout, &x0, &dx, power, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    printf("------------------Measurement------------------\n");
    printf("Average Power Mean (dBm)                        : %lf\n", averagePowerMean);
    printf("Average Power Maximum (dBm)                     : %lf\n", averagePowerMaximum);
    printf("Average Power Minimum (dBm)                     : %lf\n", averagePowerMinimum);
    printf("Peak to Average Power Ratio Maximum (dB)        : %lf\n", peakToAveragePowerRatioMaximum);
    printf("EDR GFSK Average Power Mean (dBm)               : %lf\n", EDRGFSKAveragePowerMean);
    printf("EDR DPSK Average Power Mean (dBm)               : %lf\n", EDRDPSKAveragePowerMean);
    printf("EDR DPSK GFSK Average Power Ratio Mean (dB)     : %lf\n", EDR_DPSK_GFSKAveragePowerRatioMean);

    printf("------------------LE CTE Reference Period Measurement------------------\n");
    printf("Average Power Mean (dBm)                                         : %lf\n", referencePeriodAveragePowerMean);
    printf("Peak Absolute Power Deviation Maximum (%)                         : %lf\n\n", referencePeriodPeakAbsolutePowerDeviationMaximum);

    printf("------------------LE CTE Transmit Slot Power Measurement------------------\n");
    for (i = 0; i < transmitSlotAveragePowerMeanSize; i++)
    {
      printf("Average Power Mean (dBm)[%d]                     : %lf\n\n", i, transmitSlotAveragePowerMean[i]);
      printf("Peak Absolute Power Deviation Maximum (%)[%d]     : %lf\n\n", i, transmitSlotPeakAbsolutePowerDeviationMaximum[i]);
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
    if (power)
    {
        free(power);
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}