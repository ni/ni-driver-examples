// Steps:
// 1. Open a new RFmx session.
// 2. Configure Frequency Reference.
// 3. Configure Sweep settings: Frequency List, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
// 4. Configure Averaging.
// 5. Select Wave measurement.
// 6. Configure Number of Waves.
// 7. Configure each Wave and Format.
// 8. Configure Magnitude Units & Phase Trace Type.
// 9. Initiate the Measurement.
// 10. Read the Num Waves.
// 11. Fetch Waves X data.
// 12. Fetch Waves Y data for each Wave.
// 13. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxVNA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION 4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING 256
#define NUMBER_OF_WAVES 4

int main(int argc, char* argv[])
{
    char* resourceName = "VNA";
    niRFmxInstrHandle instrumentHandle = NULL;

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0;
    int32 lastErrorCode = 0;

    float64 startFrequency = 1.0e9;
    float64 stopFrequency = 26.0e9;
    int32 numberOfFrequencyPoints = 251;
    float64 port1PowerLevel = -10.0;                                                                      /* (dBm) */
    float64 port2PowerLevel = -10.0;                                                                      /* (dBm) */
    float64 port1TestReceiverAttenuation = 0.0;                                                           /* (dB) */
    float64 port2TestReceiverAttenuation = 0.0;                                                           /* (dB) */
    float64 IFBandwidth = 100.0e3;                                                                        /* (Hz) */

    char* frequencyReferenceSource = RFMXVNA_VAL_PXI_CLK_STR;
    float64 frequencyReferenceFrequency = 100.0e6;                                                        /* (Hz) */

    char* waves[NUMBER_OF_WAVES] = {"a1_1", "b1_1", "a1_2", "b1_2"};
    int32 wavesFormats[NUMBER_OF_WAVES] = {
        RFMXVNA_VAL_WAVES_FORMAT_MAGNITUDE,
        RFMXVNA_VAL_WAVES_FORMAT_PHASE,
        RFMXVNA_VAL_WAVES_FORMAT_MAGNITUDE,
        RFMXVNA_VAL_WAVES_FORMAT_PHASE };

    int32 magnitudeUnits = RFMXVNA_VAL_WAVES_MAGNITUDE_UNITS_DBM;
    int32 phaseTraceType = RFMXVNA_VAL_WAVES_PHASE_TRACE_TYPE_WRAPPED;

    int32 averagingEnabled = RFMXVNA_VAL_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;

    char waveSelectorString[MAX_SELECTOR_STRING];
    char portSelectorString[MAX_SELECTOR_STRING];

    int32 index = 0;

    float64 timeout = 10.0;                                                                               /*(seconds) */

    int32 actualArraySize = 0;
    int32 numberOfWavesResult = 0;
    float64* wavesXDataResult = NULL;
    float32** wavesY1DataResult = NULL;
    float32** wavesY2DataResult = NULL;

    /* Initialize a session */
    RFmxCheckWarn(RFmxVNA_Initialize(resourceName, "", &instrumentHandle, NULL));

    /* Configure the session */
    RFmxCheckWarn(RFmxVNA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxVNA_SetSweepType(instrumentHandle, "", RFMXVNA_VAL_SWEEP_TYPE_LINEAR));
    RFmxCheckWarn(RFmxVNA_SetStartFrequency(instrumentHandle, "", startFrequency));
    RFmxCheckWarn(RFmxVNA_SetStopFrequency(instrumentHandle, "", stopFrequency));
    RFmxCheckWarn(RFmxVNA_SetNumberOfPoints(instrumentHandle, "", numberOfFrequencyPoints));
    RFmxCheckWarn(RFmxVNA_SetIFBandwidth(instrumentHandle, "", IFBandwidth));
    RFmxCheckWarn(RFmxVNA_BuildPortString("", "port1", MAX_SELECTOR_STRING, portSelectorString));
    RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, port1PowerLevel));
    RFmxCheckWarn(RFmxVNA_SetTestReceiverAttenuation(instrumentHandle, portSelectorString, port1TestReceiverAttenuation));
    RFmxCheckWarn(RFmxVNA_BuildPortString("", "port2", MAX_SELECTOR_STRING, portSelectorString));
    RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, port2PowerLevel));
    RFmxCheckWarn(RFmxVNA_SetTestReceiverAttenuation(instrumentHandle, portSelectorString, port2TestReceiverAttenuation));
    RFmxCheckWarn(RFmxVNA_SetAveragingEnabled(instrumentHandle, "", averagingEnabled));
    RFmxCheckWarn(RFmxVNA_SetAveragingCount(instrumentHandle, "", averagingCount));
    RFmxCheckWarn(RFmxVNA_SelectMeasurements(instrumentHandle, "", RFMXVNA_VAL_WAVES, RFMXVNA_VAL_FALSE));
    RFmxCheckWarn(RFmxVNA_WavesSetNumberOfWaves(instrumentHandle, "", NUMBER_OF_WAVES));
    for (index = 0; index < NUMBER_OF_WAVES; index++) {
        RFmxCheckWarn(RFmxVNA_BuildWaveString("", index, MAX_SELECTOR_STRING, waveSelectorString));
        RFmxCheckWarn(RFmxVNA_WavesCfgWave(instrumentHandle, waveSelectorString, waves[index]));
        RFmxCheckWarn(RFmxVNA_WavesSetFormat(instrumentHandle, waveSelectorString, wavesFormats[index]));
    }
    RFmxCheckWarn(RFmxVNA_WavesSetMagnitudeUnits(instrumentHandle, "", magnitudeUnits));
    RFmxCheckWarn(RFmxVNA_WavesSetPhaseTraceType(instrumentHandle, "", phaseTraceType));
    RFmxCheckWarn(RFmxVNA_Initiate(instrumentHandle, "", ""));

    /* Fetch results */
    RFmxCheckWarn(RFmxVNA_WavesGetNumberOfWaves(instrumentHandle, "", &numberOfWavesResult));

    RFmxCheckWarn(RFmxVNA_WavesFetchXData(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
    if (actualArraySize > 0) {
        wavesXDataResult = (float64*)malloc(sizeof(float64) * actualArraySize);
        if (wavesXDataResult) {
            RFmxCheckWarn(RFmxVNA_WavesFetchXData(instrumentHandle, "", timeout, wavesXDataResult, actualArraySize, NULL));
        } else {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    wavesY1DataResult = (float32**)malloc(sizeof(float32*) * numberOfWavesResult);
    wavesY2DataResult = (float32**)malloc(sizeof(float32*) * numberOfWavesResult);
    if (wavesY1DataResult && wavesY2DataResult) {
        for (index = 0; index < numberOfWavesResult; index++) {
            actualArraySize = 0;
            RFmxCheckWarn(RFmxVNA_BuildWaveString("", index, MAX_SELECTOR_STRING, waveSelectorString));
            RFmxCheckWarn(RFmxVNA_WavesFetchYData(instrumentHandle, waveSelectorString, timeout, NULL, NULL, 0, &actualArraySize));
            if (actualArraySize > 0) {
                wavesY1DataResult[index] = (float32*)malloc(sizeof(float32) * actualArraySize);
                wavesY2DataResult[index] = (float32*)malloc(sizeof(float32) * actualArraySize);
                if (wavesY1DataResult[index] && wavesY2DataResult[index]) {
                    RFmxCheckWarn(RFmxVNA_WavesFetchYData(instrumentHandle, waveSelectorString, timeout, wavesY1DataResult[index], wavesY2DataResult[index], actualArraySize, NULL));
                } else {
                    printf("malloc failed.\n");
                    goto Error;
                }
            }
        }
    } else {
        printf("malloc failed.\n");
        goto Error;
    }

Error:
    if (error) {
        RFmxVNA_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
        if (error < 0)
            printf("ERROR: %s\n", errorMessage);
        else
            printf("WARNING: %s %d\n", errorMessage, error);
    }

    if (instrumentHandle) {
        RFmxVNA_Close(instrumentHandle, RFMXVNA_VAL_FALSE);
    }

    /* Free allocated memory */
    if (wavesXDataResult) {
        free(wavesXDataResult);
    }
    for (index = 0; index < numberOfWavesResult; index++) {
        if (wavesY1DataResult[index]) {
            free(wavesY1DataResult[index]);
        }
        if (wavesY2DataResult[index]) {
            free(wavesY2DataResult[index]);
        }
    }
    if (wavesY1DataResult) {
        free(wavesY1DataResult);
    }
    if (wavesY2DataResult) {
        free(wavesY2DataResult);
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}