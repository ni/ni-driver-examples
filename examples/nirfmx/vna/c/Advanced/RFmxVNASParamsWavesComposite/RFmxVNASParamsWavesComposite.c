//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
//4. Configure Averaging.
//5. Select S-Parameter and Waves measurements.
//6. Configure number of S-Parameters.
//7. Configure each S-Parameter and Format.
//8. Configure Magnitude Units & Phase Trace Type.
//9. Configure Number of Waves.
//10. Configure each Wave and Format
//11. Configure Magnitude Units & Phase Trace Type
//12. Initiate the Measurement.
//13. Read Number of SParams and X-Axis Values (aggregated frequency list).
//14. Fetch S-Parameter X data.
//15. Fetch S-Parameter Y data for each S-Parameter.
//16. Read the Num Waves.
//17. Fetch Waves X data.
//18. Fetch Waves Y Data for each Wave.
//19. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxVNA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION 4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING 256
#define NUMBER_OF_SPARAMS 4
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
    float64 port1PowerLevel = -10.0; /* (dBm) */
    float64 port2PowerLevel = -10.0; /* (dBm) */
    float64 port1TestReceiverAttenuation = 0.0; /* (dB) */
    float64 port2TestReceiverAttenuation = 0.0; /* (dB) */
    float64 IFBandwidth = 100.0e3; /* (Hz) */

    char* frequencyReferenceSource = RFMXVNA_VAL_PXI_CLK_STR;
    float64 frequencyReferenceFrequency = 100.0e6; /* (Hz) */

    char* sParamsSParameters[NUMBER_OF_SPARAMS] = {"S11", "S12", "S21", "S22"};
    int32 sParamsFormats[NUMBER_OF_SPARAMS] = {RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE, RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE,
        RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE, RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE};
    char* waves[NUMBER_OF_WAVES] = {"a1_1", "b1_1", "a1_2", "b1_2"};
    int32 wavesFormats[NUMBER_OF_WAVES] = {RFMXVNA_VAL_WAVES_FORMAT_MAGNITUDE, RFMXVNA_VAL_WAVES_FORMAT_PHASE,
        RFMXVNA_VAL_WAVES_FORMAT_MAGNITUDE, RFMXVNA_VAL_WAVES_FORMAT_PHASE};

    int32 sparamsMagnitudeUnits = RFMXVNA_VAL_SPARAMS_MAGNITUDE_UNITS_DB;
    int32 sparamsPhaseTraceType = RFMXVNA_VAL_SPARAMS_PHASE_TRACE_TYPE_WRAPPED;
    int32 wavesMagnitudeUnits = RFMXVNA_VAL_WAVES_MAGNITUDE_UNITS_DBM;
    int32 wavesPhaseTraceType = RFMXVNA_VAL_WAVES_PHASE_TRACE_TYPE_WRAPPED;

    int32 averagingEnabled = RFMXVNA_VAL_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;

    char sParamSelectorString[MAX_SELECTOR_STRING];
    char waveSelectorString[MAX_SELECTOR_STRING];
    char portSelectorString[MAX_SELECTOR_STRING];

    int32 index = 0;

    float64 timeout = 10.0; /*(seconds) */

    int32 actualArraySize = 0;
    int32 numberOfSParamsResult = 0;
    float64* sParamsXDataResult = NULL;
    float32** sParamsY1DataResult = NULL;
    float32** sParamsY2DataResult = NULL;
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
    RFmxCheckWarn(RFmxVNA_SelectMeasurements(instrumentHandle, "", (RFMXVNA_VAL_SPARAMS | RFMXVNA_VAL_WAVES), RFMXVNA_VAL_FALSE));

    /*Configure SParameter measurement*/
    RFmxCheckWarn(RFmxVNA_SParamsSetNumberOfSParameters(instrumentHandle, "", NUMBER_OF_SPARAMS));
    for (index = 0; index < NUMBER_OF_SPARAMS; index++)
    {
        RFmxCheckWarn(RFmxVNA_BuildSParameterString("", index, MAX_SELECTOR_STRING, sParamSelectorString));
        RFmxCheckWarn(RFmxVNA_SParamsCfgSParameter(instrumentHandle, sParamSelectorString, sParamsSParameters[index]));
        RFmxCheckWarn(RFmxVNA_SParamsSetFormat(instrumentHandle, sParamSelectorString, sParamsFormats[index]));
    }
    RFmxCheckWarn(RFmxVNA_SParamsSetMagnitudeUnits(instrumentHandle, "", sparamsMagnitudeUnits));
    RFmxCheckWarn(RFmxVNA_SParamsSetPhaseTraceType(instrumentHandle, "", sparamsPhaseTraceType));

    /*Configure Wave measurement*/
    RFmxCheckWarn(RFmxVNA_WavesSetNumberOfWaves(instrumentHandle, "", NUMBER_OF_WAVES));
    for (index = 0; index < NUMBER_OF_WAVES; index++)
    {
        RFmxCheckWarn(RFmxVNA_BuildWaveString("", index, MAX_SELECTOR_STRING, waveSelectorString));
        RFmxCheckWarn(RFmxVNA_WavesCfgWave(instrumentHandle, waveSelectorString, waves[index]));
        RFmxCheckWarn(RFmxVNA_WavesSetFormat(instrumentHandle, waveSelectorString, wavesFormats[index]));
    }
    RFmxCheckWarn(RFmxVNA_WavesSetMagnitudeUnits(instrumentHandle, "", wavesMagnitudeUnits));
    RFmxCheckWarn(RFmxVNA_WavesSetPhaseTraceType(instrumentHandle, "", wavesPhaseTraceType));

    RFmxCheckWarn(RFmxVNA_Initiate(instrumentHandle, "", ""));

    /* Fetch SParameter measurement results */
    RFmxCheckWarn(RFmxVNA_SParamsGetNumberOfSParameters(instrumentHandle, "", &numberOfSParamsResult));
    RFmxCheckWarn(RFmxVNA_SParamsFetchXData(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
        sParamsXDataResult = (float64*)malloc(sizeof(float64) * actualArraySize);
        if (sParamsXDataResult)
        {
            RFmxCheckWarn(RFmxVNA_SParamsFetchXData(instrumentHandle, "", timeout, sParamsXDataResult, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    sParamsY1DataResult = (float32**)malloc(sizeof(float32*) * numberOfSParamsResult);
    sParamsY2DataResult = (float32**)malloc(sizeof(float32*) * numberOfSParamsResult);
    if (sParamsY1DataResult && sParamsY2DataResult)
    {
        for (index = 0; index < numberOfSParamsResult; index++)
        {
            actualArraySize = 0;
            RFmxCheckWarn(RFmxVNA_BuildSParameterString("", index, MAX_SELECTOR_STRING, sParamSelectorString));
            RFmxCheckWarn(RFmxVNA_SParamsFetchYData(instrumentHandle, sParamSelectorString, timeout, NULL, NULL, 0, &actualArraySize));
            if (actualArraySize > 0)
            {
                sParamsY1DataResult[index] = (float32*)malloc(sizeof(float32) * actualArraySize);
                sParamsY2DataResult[index] = (float32*)malloc(sizeof(float32) * actualArraySize);
                if (sParamsY1DataResult[index] && sParamsY2DataResult[index])
                {
                    RFmxCheckWarn(RFmxVNA_SParamsFetchYData(instrumentHandle, sParamSelectorString, timeout, sParamsY1DataResult[index], sParamsY2DataResult[index], actualArraySize, NULL));
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

    /* Fetch Wave measurement results */
    actualArraySize = 0;
    RFmxCheckWarn(RFmxVNA_WavesGetNumberOfWaves(instrumentHandle, "", &numberOfWavesResult));
    RFmxCheckWarn(RFmxVNA_WavesFetchXData(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
    if (actualArraySize > 0)
    {
        wavesXDataResult = (float64*)malloc(sizeof(float64) * actualArraySize);
        if (wavesXDataResult)
        {
            RFmxCheckWarn(RFmxVNA_WavesFetchXData(instrumentHandle, "", timeout, wavesXDataResult, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    wavesY1DataResult = (float32**)malloc(sizeof(float32*) * numberOfWavesResult);
    wavesY2DataResult = (float32**)malloc(sizeof(float32*) * numberOfWavesResult);
    if (wavesY1DataResult && wavesY2DataResult)
    {
        for (index = 0; index < numberOfWavesResult; index++)
        {
            actualArraySize = 0;
            RFmxCheckWarn(RFmxVNA_BuildWaveString("", index, MAX_SELECTOR_STRING, waveSelectorString));
            RFmxCheckWarn(RFmxVNA_WavesFetchYData(instrumentHandle, waveSelectorString, timeout, NULL, NULL, 0, &actualArraySize));
            if (actualArraySize > 0)
            {
                wavesY1DataResult[index] = (float32*)malloc(sizeof(float32) * actualArraySize);
                wavesY2DataResult[index] = (float32*)malloc(sizeof(float32) * actualArraySize);
                if (wavesY1DataResult[index] && wavesY2DataResult[index])
                {
                    RFmxCheckWarn(RFmxVNA_WavesFetchYData(instrumentHandle, waveSelectorString, timeout, wavesY1DataResult[index], wavesY2DataResult[index], actualArraySize, NULL));
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
    if (sParamsXDataResult)
    {
        free(sParamsXDataResult);
    }
    for (index = 0; index < numberOfSParamsResult; index++)
    {
        if (sParamsY1DataResult[index])
        {
            free(sParamsY1DataResult[index]);
        }
        if (sParamsY2DataResult[index])
        {
            free(sParamsY2DataResult[index]);
        }
    }
    if (sParamsY1DataResult)
    {
        free(sParamsY1DataResult);
    }
    if (sParamsY2DataResult)
    {
        free(sParamsY2DataResult);
    }

    if (wavesXDataResult)
    {
        free(wavesXDataResult);
    }
    for (index = 0; index < numberOfWavesResult; index++)
    {
        if (wavesY1DataResult[index])
        {
            free(wavesY1DataResult[index]);
        }
        if (wavesY2DataResult[index])
        {
            free(wavesY2DataResult[index]);
        }
    }
    if (wavesY1DataResult)
    {
        free(wavesY1DataResult);
    }
    if (wavesY2DataResult)
    {
        free(wavesY2DataResult);
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}