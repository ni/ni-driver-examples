//Instructions:
//1. This example demonstrates the use of RFmxWLAN OFDM ModAcc measurement to compute EVM after compensating for the noise attributed to the VSA.
//2. The example uses an enum control, "Calibrate Noise Floor" with two values :
//Disabled(0) : select this to skip calibrating the VSA noise floor, and perform the OFDMModAcc measurement directly.You may want to do this when the VSA noise floor has already been calibrated or when Noise Compensation is disabled.
//Enabled(1) : select this to first calibrate the VSA noise floor, and then perform the OFDMModAcc measurement.

//Follow these steps to calibrate VSA noise, and then perform ModAcc measurement :
//1. Set Calibrate Noise Floor to "Enabled".
//2. Run the example.
//3. When "Turn OFF Generation" message appears on console, ensure that signal generation is turned OFF and then enter "o".Wait for calibration to complete.
//4. When "Turn ON Generation" message appears on console, ensure that signal generation is turned ON and then enter any key to continue.

//Follow these steps to skip(re)calibrating and directly perform ModAcc measurement :
//1. Set Calibrate Noise Floor to "Disabled".
//2. Run the example

//Steps:
//1. Open a new RFmx session.
//2. Configure the frequency reference properties(Clock Source and Clock Frequency).
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time).
//5. Configure Standard and Channel Bandwidth.
//6. Select OFDMModAcc measurement and enable the traces.
//7. Configure Optimize Dynamic Range for EVM.
//8. Configure Measurement Mode as Calibrate Noise Floor.
//9. Initiate Measurement.
//10. Wait for Measurement Complete.
//11. Configure Measurement Mode as Measure.
//12. Configure Measurement Interval.
//13. Configure Averaging parameters.
//14. Configure Noise Compensation Enabled.
//15. Initiate Measurement.
//16. Fetch OFDMModAcc Measurements.
//17. Close the RFmx Session

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

    uInt32 measurements = RFMXWLAN_VAL_OFDMMODACC;
    int32 enableAllTraces = RFMXWLAN_VAL_TRUE;

    int32 standard = RFMXWLAN_VAL_STANDARD_802_11_AG;
    float64 channelBandwidth = 20e06;                                            /*(Hz) */

    int32 measurementOffset = 0;                                                 /*(symbols)*/
    int32 maximumMeasurementLength = 16;                                         /*(symbols)*/

    int32 noiseCompensationEnabled = RFMXWLAN_VAL_OFDMMODACC_NOISE_COMPENSATION_ENABLED_TRUE;

    int32 optimizeDynamicRangeForEVMEnabled = RFMXWLAN_VAL_OFDMMODACC_OPTIMIZE_DYNAMIC_RANGE_FOR_EVM_ENABLED_TRUE;
    float64 optimizeDynamicRangeForEVMMargin = 0;                              /*(dB) */

    int32 averagingEnabled = RFMXWLAN_VAL_OFDMMODACC_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;

    int32 calibrateNoiseFloor = 1; //1 - Enabled, 0 - Disabled.

    float64 timeout = 10.0;

    float64 compositeRMSEVMMean = 0.0;
    float64 compositeDataRMSEVMMean = 0.0;
    float64 compositePilotRMSEVMMean = 0.0;

    NIComplexSingle *pilotConstellation = NULL;
    NIComplexSingle *dataConstellation = NULL;

    int32 actualArraySize = 0;
    float64 x0 = 0.0;
    float64 dx = 0.0;
    float32 *chainRMSEVMPerSubcarrierMean = NULL;

    char userInput;

    /* Initialize a session. */
    RFmxCheckWarn(RFmxInstr_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxInstr_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
        frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxWLAN_CfgFrequency(instrumentHandle, "", centerFrequency));
    RFmxCheckWarn(RFmxWLAN_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
    RFmxCheckWarn(RFmxWLAN_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
    RFmxCheckWarn(RFmxWLAN_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeSlope, IQPowerEdgeLevel,
        triggerDelay, minimumQuiteTimeMode, minimumQuietTime, IQPowerEdgeLevelType, IQPowerEdgeEnabled));
    RFmxCheckWarn(RFmxWLAN_CfgStandard(instrumentHandle, "", standard));
    RFmxCheckWarn(RFmxWLAN_CfgChannelBandwidth(instrumentHandle, "", channelBandwidth));
    RFmxCheckWarn(RFmxWLAN_SelectMeasurements(instrumentHandle, "", measurements, enableAllTraces));
    RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgOptimizeDynamicRangeForEVM(instrumentHandle, "", optimizeDynamicRangeForEVMEnabled,
        optimizeDynamicRangeForEVMMargin));

    if(calibrateNoiseFloor)
    {
        RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgMeasurementMode(instrumentHandle, "",
            RFMXWLAN_VAL_OFDMMODACC_MEASUREMENT_MODE_CALIBRATE_NOISE_FLOOR));
        printf("Turn OFF Generation \nOK(o)\t Cancel(c)\n");
		scanf_s("%c", &userInput, 1);
        if (userInput == 'o' || userInput == 'O')
        {
            RFmxCheckWarn(RFmxWLAN_Initiate(instrumentHandle, "", ""));
            RFmxCheckWarn(RFmxWLAN_WaitForMeasurementComplete(instrumentHandle, "", timeout));
        }
        printf("Turn ON Generation\n");
        printf("Press any key to continue\n");
        _getch();
    }

    RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgMeasurementMode(instrumentHandle, "",
        RFMXWLAN_VAL_OFDMMODACC_MEASUREMENT_MODE_MEASURE));
    RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgMeasurementLength(instrumentHandle, "", measurementOffset,
        maximumMeasurementLength));
    RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
    RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgNoiseCompensationEnabled(instrumentHandle, "", noiseCompensationEnabled));
    RFmxCheckWarn(RFmxWLAN_Initiate(instrumentHandle, "", ""));

    /* Fetch Results. */
    RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchCompositeRMSEVM(instrumentHandle, "", timeout, &compositeRMSEVMMean,
        &compositeDataRMSEVMMean, &compositePilotRMSEVMMean));

    actualArraySize = 0;
    RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchPilotConstellationTrace(instrumentHandle, "", timeout, NULL, 0,
        &actualArraySize));
    if (actualArraySize > 0)
    {
        pilotConstellation = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
        if(pilotConstellation)
        {
            RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchPilotConstellationTrace(instrumentHandle, "", timeout,
                pilotConstellation, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    actualArraySize = 0;
    RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchDataConstellationTrace(instrumentHandle, "", timeout, NULL, 0,
        &actualArraySize));
    if (actualArraySize > 0)
    {
        dataConstellation = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
        if(dataConstellation)
        {
            RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchDataConstellationTrace(instrumentHandle, "", timeout,
                dataConstellation, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    actualArraySize = 0;
    RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchChainRMSEVMPerSubcarrierMeanTrace(instrumentHandle, "", timeout, NULL, NULL,
        NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
        chainRMSEVMPerSubcarrierMean = (float32*)malloc(sizeof(float32) * actualArraySize);
        if(chainRMSEVMPerSubcarrierMean)
        {
            RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchChainRMSEVMPerSubcarrierMeanTrace(instrumentHandle, "", timeout, &x0,
                &dx, chainRMSEVMPerSubcarrierMean, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    /*Print Results. */
    printf("------------------Composite EVM------------------\n");
    printf("RMS EVM Mean (dB)                       : %lf\n", compositeRMSEVMMean);
    printf("Data RMS EVM Mean (dB)                  : %lf\n", compositeDataRMSEVMMean);
    printf("Pilot RMS EVM Mean (dB)                 : %lf\n\n", compositePilotRMSEVMMean);

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

    /* Free allocated memory. */
    /* Free allocated memory */
    if (pilotConstellation)
    {
        free(pilotConstellation);
    }
    if (dataConstellation)
    {
        free(dataConstellation);
    }
    if (chainRMSEVMPerSubcarrierMean)
    {
        free(chainRMSEVMPerSubcarrierMean);
    }
    printf("Press any key to exit\n");
    _getch();

    return error;
}