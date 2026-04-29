// Steps:
// 1. Open a new RFmx session
//    Create two named Signals called 'Signal1' and 'Signal2'. Signals here can be considered equivalent to the concept of Channels in third party software.
// 2. For each Signal, configure sweep and measurement settings. Note that, Power Level varies for each Signal in this example.
// 3. Create Signal configuration
// 4. Configure the sweep properties - Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, Test Receiver Attn, Power Level
// 5. Select S-Parameter Measurement
// 6. Configure the number of S-Parameters
// 7. Configure the S-Parameter and Format
// 8. For each Signal, initiate the measurement and wait for the acquisition to complete.
//    Here note that, between the two signals, only acquisition is sequential and measurements are overlapped.
// 9. Fetch the measurement results of each Signal
// 10. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxVNA.h"
//
/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION 4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING 256
#define SIGNAL_NAME_LEN 50
#define NUMBER_OF_SPARAMS 4
#define NUM_OF_MEASUREMENTS 2

char* vnaSignal1 = NULL;
char* vnaSignal2 = NULL;
static char* GetSignalName(int index)
{
    switch (index)
    {
        case 0:
            return vnaSignal1;
        case 1:
            return vnaSignal2;
        default:
            printf("Invalid index\n");
            return NULL;
    }
}

int main(int argc, char* argv[])
{
    char* resourceName = "VNA";
    niRFmxInstrHandle instrumentHandle = NULL;

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0;
    int32 lastErrorCode = 0;
    int32 i = 0, j = 0, k = 0;

    float64 startFrequency = 1.0e9;
    float64 stopFrequency = 26.0e9;
    int32 numberOfFrequencyPoints = 251;
    float64 PowerLevel = -10.0; /* (dBm) */
    float64 port1TestReceiverAttenuation = 0.0; /* (dB) */
    float64 port2TestReceiverAttenuation = 0.0; /* (dB) */
    float64 IFBandwidth = 100.0e3; /* (Hz) */

    char* frequencyReferenceSource = RFMXVNA_VAL_PXI_CLK_STR;
    float64 frequencyReferenceFrequency = 100.0e6; /* (Hz) */

    char* sParamsSParameters[NUMBER_OF_SPARAMS] = {"S11", "S12", "S21", "S22"};
    int32 sParamsFormats[NUMBER_OF_SPARAMS] = {RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE, RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE,
        RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE, RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE};

    float64 acquisitionTimeout = 10.0; /*(seconds) */
    float64 fetchTimeout = 10.0; /*(seconds) */

    char sParamSelectorString[MAX_SELECTOR_STRING];
    char portSelectorString[MAX_SELECTOR_STRING];
    char* signalString[NUM_OF_MEASUREMENTS] = {"Signal1", "Signal2"};

    int32 index = 0;
    int32 actualArraySize = 0;
    int32 numberOfSParamsResult[NUM_OF_MEASUREMENTS] = {0};
    int32 correctionStateResult[NUM_OF_MEASUREMENTS] = {0};
    float64* sParamsXDataResult[NUM_OF_MEASUREMENTS] = {NULL};
    float32** sParamsY1DataResult[NUM_OF_MEASUREMENTS] = {NULL};
    float32** sParamsY2DataResult[NUM_OF_MEASUREMENTS] = {NULL};

    /* Initialize a session */
    RFmxCheckWarn(RFmxVNA_Initialize(resourceName, "", &instrumentHandle, NULL));

    /* Configure the session */
    RFmxCheckWarn(RFmxVNA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));

    vnaSignal1 = (char*)malloc(sizeof(char) * SIGNAL_NAME_LEN);
    vnaSignal2 = (char*)malloc(sizeof(char) * SIGNAL_NAME_LEN);
    if (vnaSignal1 && vnaSignal2)
    {
        RFmxCheckWarn(RFmxVNA_BuildSignalString(signalString[0], "", MAX_SELECTOR_STRING, vnaSignal1));
        RFmxCheckWarn(RFmxVNA_CreateSignalConfiguration(instrumentHandle, vnaSignal1));
        RFmxCheckWarn(RFmxVNA_BuildSignalString(signalString[1], "", MAX_SELECTOR_STRING, vnaSignal2));
        RFmxCheckWarn(RFmxVNA_CreateSignalConfiguration(instrumentHandle, vnaSignal2));
    }
    else
    {
        printf("malloc failed.\n");
        goto Error;
    }

    for (i = 0; i < NUM_OF_MEASUREMENTS; i++)
    {
        RFmxCheckWarn(RFmxVNA_SetSweepType(instrumentHandle, GetSignalName(i), RFMXVNA_VAL_SWEEP_TYPE_LINEAR));
        RFmxCheckWarn(RFmxVNA_SetStartFrequency(instrumentHandle, GetSignalName(i), startFrequency));
        RFmxCheckWarn(RFmxVNA_SetStopFrequency(instrumentHandle, GetSignalName(i), stopFrequency));
        RFmxCheckWarn(RFmxVNA_SetNumberOfPoints(instrumentHandle, GetSignalName(i), numberOfFrequencyPoints));
        RFmxCheckWarn(RFmxVNA_SetIFBandwidth(instrumentHandle, GetSignalName(i), IFBandwidth));
        RFmxCheckWarn(RFmxVNA_BuildPortString(GetSignalName(i), "port1", MAX_SELECTOR_STRING, portSelectorString));
        RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, PowerLevel - (10 * i)));
        RFmxCheckWarn(RFmxVNA_SetTestReceiverAttenuation(instrumentHandle, portSelectorString, port1TestReceiverAttenuation));
        RFmxCheckWarn(RFmxVNA_BuildPortString(GetSignalName(i), "port2", MAX_SELECTOR_STRING, portSelectorString));
        RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, PowerLevel - (10 * i)));
        RFmxCheckWarn(RFmxVNA_SetTestReceiverAttenuation(instrumentHandle, portSelectorString, port2TestReceiverAttenuation));
        RFmxCheckWarn(RFmxVNA_SelectMeasurements(instrumentHandle, GetSignalName(i), RFMXVNA_VAL_SPARAMS, RFMXVNA_VAL_FALSE));
        RFmxCheckWarn(RFmxVNA_SParamsSetNumberOfSParameters(instrumentHandle, GetSignalName(i), NUMBER_OF_SPARAMS));
        for (index = 0; index < NUMBER_OF_SPARAMS; index++)
        {
            RFmxCheckWarn(RFmxVNA_BuildSParameterString(GetSignalName(i), index, MAX_SELECTOR_STRING, sParamSelectorString));
            RFmxCheckWarn(RFmxVNA_SParamsCfgSParameter(instrumentHandle, sParamSelectorString, sParamsSParameters[index]));
            RFmxCheckWarn(RFmxVNA_SParamsSetFormat(instrumentHandle, sParamSelectorString, sParamsFormats[index]));
        }
    }

    for (j = 0; j < NUM_OF_MEASUREMENTS; j++)
    {
        RFmxCheckWarn(RFmxVNA_Initiate(instrumentHandle, GetSignalName(j), ""));
        RFmxCheckWarn(RFmxInstr_WaitForAcquisitionComplete(instrumentHandle, acquisitionTimeout));
    }

    /* Fetch results */
    for (k = 0; k < NUM_OF_MEASUREMENTS; k++)
    {
        RFmxCheckWarn(RFmxVNA_SParamsGetNumberOfSParameters(instrumentHandle, GetSignalName(k), &numberOfSParamsResult[k]));
        RFmxCheckWarn(RFmxVNA_SParamsGetResultsCorrectionState(instrumentHandle, GetSignalName(k), &correctionStateResult[k]));

        RFmxCheckWarn(RFmxVNA_SParamsFetchXData(instrumentHandle, GetSignalName(k), fetchTimeout, NULL, 0, &actualArraySize));
        if (actualArraySize > 0)
        {
            sParamsXDataResult[k] = (float64*)malloc(sizeof(float64) * actualArraySize);
            if (sParamsXDataResult)
            {
                RFmxCheckWarn(RFmxVNA_SParamsFetchXData(instrumentHandle, GetSignalName(k), fetchTimeout, sParamsXDataResult[k], actualArraySize, NULL));
            }
            else
            {
                printf("malloc failed.\n");
                goto Error;
            }
        }

        sParamsY1DataResult[k] = (float32**)malloc(sizeof(float32*) * numberOfSParamsResult[k]);
        sParamsY2DataResult[k] = (float32**)malloc(sizeof(float32*) * numberOfSParamsResult[k]);
        if (sParamsY1DataResult[k] && sParamsY2DataResult[k])
        {
            for (index = 0; index < numberOfSParamsResult[k]; index++)
            {
                actualArraySize = 0;
                RFmxCheckWarn(RFmxVNA_BuildSParameterString(GetSignalName(k), index, MAX_SELECTOR_STRING, sParamSelectorString));
                RFmxCheckWarn(RFmxVNA_SParamsFetchYData(instrumentHandle, sParamSelectorString, fetchTimeout, NULL, NULL, 0, &actualArraySize));
                if (actualArraySize > 0)
                {
                    sParamsY1DataResult[k][index] = (float32*)malloc(sizeof(float32) * actualArraySize);
                    sParamsY2DataResult[k][index] = (float32*)malloc(sizeof(float32) * actualArraySize);
                    if (sParamsY1DataResult[k][index] && sParamsY2DataResult[k][index])
                    {
                        RFmxCheckWarn(RFmxVNA_SParamsFetchYData(instrumentHandle, sParamSelectorString, fetchTimeout, sParamsY1DataResult[k][index], sParamsY2DataResult[k][index], actualArraySize, NULL));
                    }
                    else
                    {
                        printf("malloc failed.\n");
                        goto Error;
                    }
                }
            }
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

Error:
    if (error)
    {
        RFmxVNA_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
        if (error < 0)
            printf("ERROR: %s\n", errorMessage);
        else
            printf("WARNING: %s %d\n", errorMessage, error);
    }

    if (instrumentHandle)
    {
        RFmxVNA_Close(instrumentHandle, RFMXVNA_VAL_FALSE);
    }

    /* Free allocated memory */
    if (vnaSignal1)
        free(vnaSignal1);
    if (vnaSignal2)
        free(vnaSignal2);

    for (i = 0; i < NUM_OF_MEASUREMENTS; i++)
    {
        if (sParamsXDataResult[i])
            free(sParamsXDataResult[i]);
    }

    for (i = 0; i < NUM_OF_MEASUREMENTS; i++)
    {
        for (index = 0; index < numberOfSParamsResult[i]; index++)
        {
            if (sParamsY1DataResult[i][index])
            {
                free(sParamsY1DataResult[i][index]);
            }
            if (sParamsY2DataResult[i][index])
            {
                free(sParamsY2DataResult[i][index]);
            }
        }
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}