// steps:
// 1. open a new rfmx session.
// 2. configure frequency reference.
// 3. configure s-parameter external attenuation table (external fixture's de-embedding table) from s2p file.
// 4. configure port extension.
// 5 & 6. configure sweep settings: start frequency, stop frequency, number of frequency points, if bandwidth, and power level and test rx attenuation with different port names.
// 7. configure averaging.
// 8. configure trigger.
// 9. select s-parameter measurement.
// 10. configure number of s-parameters.
// 11. configure s-parameter and format.
// 12. configure magnitude units, phase trace type & group delay aperture settings.
// 13. load calset data from a file.
// 14. Enable Correction, Configure Interpolation Enabled and Configure correction port subset settings.
// 15. initiate the measurement after user confirmation.
// 16. read number of sparams.
// 17. fetch s-parameter x data.
// 18. fetch s-parameter y data for each s-parameter.
// 19. fetch s-Parameter correction level.
// 20. fetch s-parameter correction state.
// 21. set snp export attributes (can be accessed and written before or after measurement initiate) and save s-parameter data to file.
// 22. close rfmx session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <string.h>

#include "niRFmxVNA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION 4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING 256
#define FREQUENCY_LIST_SIZE 251
#define NUMBER_OF_SPARAMS 4
#define NUMBER_OF_EXTERNAL_FIXTURES 2

void modify_path(char* path, const char* new_subpath)
{
    // Find the first occurrence of '/'
    char* last_slash = strrchr(path, '\\');

    if (last_slash != NULL) {
        // Terminate the path at the desired sub-path (before the last '/')
        *last_slash = '\0';
    }
    // Concatenate the new sub-path
    strcat(path, new_subpath);
}
int main(int argc, char* argv[])
{
    char* resourceName = "VNA";
    niRFmxInstrHandle instrumentHandle = NULL;

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0;
    int32 lastErrorCode = 0;

    char* frequencyReferenceSource = RFMXVNA_VAL_PXI_CLK_STR;
    float64 frequencyReferenceFrequency = 100.0e6; /* (Hz) */

    int32 sweepType = RFMXVNA_VAL_SWEEP_TYPE_LINEAR;
    float64 startFrequency = 1.0e9; /* (Hz) */
    float64 stopFrequency = 26.0e9; /* (Hz) */
    float64 frequencyStep = 10.0e6;
    int32 numberOfFrequencyPoints = 251;
    float64 frequencyList[FREQUENCY_LIST_SIZE];
    for (int i = 0; i < FREQUENCY_LIST_SIZE; i++) {
        frequencyList[i] = startFrequency + i * frequencyStep;
    }
    float64 port1PowerLevel = -10.0; /* (dBm) */
    float64 port2PowerLevel = -10.0; /* (dBm) */
    float64 port1TestReceiverAttenuation = 0.0; /* (dB) */
    float64 port2TestReceiverAttenuation = 0.0; /* (dB) */
    float64 IFBandwidth = 100.0e3; /* (Hz) */

    char* portNames[NUMBER_OF_EXTERNAL_FIXTURES] = {"port1", "port2"};
    char* s2pFilePaths[NUMBER_OF_EXTERNAL_FIXTURES] =
        {
            "\\Support\\1dB_Attenuation.s2p",
            "\\Support\\1dB_Attenuation.s2p"};
    int32 sParameterOrientations[NUMBER_OF_EXTERNAL_FIXTURES] =
        {
            RFMXINSTR_VAL_PORT2_TOWARDS_DUT,
            RFMXINSTR_VAL_PORT2_TOWARDS_DUT};

    char currentDirectoryPath[_MAX_PATH];
    char filePath[_MAX_PATH];

    char* sParamsSParameters[NUMBER_OF_SPARAMS] = {"S11", "S12", "S21", "S22"};
    int32 sParamsFormats[NUMBER_OF_SPARAMS] = {RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE, RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE, RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE, RFMXVNA_VAL_SPARAMS_FORMAT_MAGNITUDE};

    int32 magnitudeUnits = RFMXVNA_VAL_SPARAMS_MAGNITUDE_UNITS_DB;
    int32 phaseTraceType = RFMXVNA_VAL_SPARAMS_PHASE_TRACE_TYPE_WRAPPED;
    int32 groupDelayApertureMode = RFMXVNA_VAL_SPARAMS_GROUP_DELAY_APERTURE_MODE_POINTS;
    float64 groupDelayAperturePoints = 11.0;
    float64 groupDelayAperturePercentage = 4.0;      /* (%) */
    float64 groupDelayApertureFrequencySpan = 1.0e9; /* (Hz) */



    char* snPFilePath = NULL;

    int numberOfPortExtension = 2;
    int portExtensionEnabled = RFMXVNA_VAL_CORRECTION_PORT_EXTENSION_ENABLED_FALSE;
    int32 portExtensionDelayDomain = RFMXVNA_VAL_CORRECTION_PORT_EXTENSION_DELAY_DOMAIN_DELAY;
    double portExtensionDelay = 100.0e-12; /* (seconds) */
    double portExtensionDistance = 29.9792e-3;
    int32 portExtensionDistanceUnit = RFMXVNA_VAL_CORRECTION_PORT_EXTENSION_DISTANCE_UNIT_METERS;
    double portExtensionVelocityFactor = 1.0;
    int portExtensionDCLossEnabled = RFMXVNA_VAL_CORRECTION_PORT_EXTENSION_DC_LOSS_ENABLED_FALSE;
    double portExtensionDCLoss = 0.0; /* (dB) */
    int portExtensionLoss1Enabled = RFMXVNA_VAL_CORRECTION_PORT_EXTENSION_LOSS1_ENABLED_FALSE;
    int portExtensionLoss2Enabled = RFMXVNA_VAL_CORRECTION_PORT_EXTENSION_LOSS2_ENABLED_FALSE;
    double portExtensionLoss1Frequency = 0.0; /* (Hz) */
    double portExtensionLoss2Frequency = 0.0; /* (Hz) */
    double portExtensionLoss1 = 0.0; /* (dB) */
    double portExtensionLoss2 = 0.0; /* (dB) */

    int32 averagingEnabled = RFMXVNA_VAL_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;

    int32 triggerType = RFMXVNA_VAL_TRIGGER_TYPE_NONE;
    int32 triggerMode = RFMXVNA_VAL_TRIGGER_MODE_SIGNAL;
    double triggerDelay = 0.0; /*(seconds)*/

    char* calsetFilePath = NULL;
    int32 interpolationEnabled = RFMXVNA_VAL_CORRECTION_INTERPOLATION_ENABLED_TRUE;
    int32 portSubsetEnabled = RFMXVNA_VAL_CORRECTION_PORT_SUBSET_ENABLED_FALSE;


    char sParamSelectorString[MAX_SELECTOR_STRING];
    char portSelectorString[MAX_SELECTOR_STRING];

    int32 index = 0;

    float64 timeout = 10.0; /*(seconds) */

    int32 actualArraySize = 0;
    int32 numberOfSParamsResult = 0;
    int32 correctionStateResult = 0;
    float64* sParamsXDataResult = NULL;
    float32** sParamsY1DataResult = NULL;
    float32** sParamsY2DataResult = NULL;

    /* Initialize a session */
    RFmxCheckWarn(RFmxVNA_Initialize(resourceName, "", &instrumentHandle, NULL));

    /* Configure the session */
    RFmxCheckWarn(RFmxVNA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    getcwd(currentDirectoryPath, _MAX_PATH);
    if (currentDirectoryPath == NULL) {
        printf("Failed to get current directory\n");
        goto Error;
    }

    for (index = 0; index < NUMBER_OF_EXTERNAL_FIXTURES; index++) {
        RFmxCheckWarn(RFmxVNA_BuildPortString("", portNames[index], MAX_SELECTOR_STRING, portSelectorString));
        strcpy(filePath, currentDirectoryPath);
        modify_path(filePath, "");
        modify_path(filePath, s2pFilePaths[index]);
        RFmxCheckWarn(RFmxInstr_LoadSParameterExternalAttenuationTableFromS2PFile(instrumentHandle,
            portSelectorString,
            "",
            filePath,
            sParameterOrientations[index]));
    }
    for (index = 0; index < numberOfPortExtension; index++) {
        RFmxCheckWarn(RFmxVNA_BuildPortString("", portNames[index], MAX_SELECTOR_STRING, portSelectorString));
        RFmxCheckWarn(RFmxVNA_SetCorrectionPortExtensionEnabled(instrumentHandle, portSelectorString, portExtensionEnabled));
        RFmxCheckWarn(RFmxVNA_SetCorrectionPortExtensionDelayDomain(instrumentHandle, portSelectorString, portExtensionDelayDomain));
        RFmxCheckWarn(RFmxVNA_SetCorrectionPortExtensionDelay(instrumentHandle, portSelectorString, portExtensionDelay));
        RFmxCheckWarn(RFmxVNA_SetCorrectionPortExtensionDistance(instrumentHandle, portSelectorString, portExtensionDistance));
        RFmxCheckWarn(RFmxVNA_SetCorrectionPortExtensionDistanceUnit(instrumentHandle, portSelectorString, portExtensionDistanceUnit));
        RFmxCheckWarn(RFmxVNA_SetCorrectionPortExtensionVelocityFactor(instrumentHandle, portSelectorString, portExtensionVelocityFactor));
        RFmxCheckWarn(RFmxVNA_SetCorrectionPortExtensionDCLossEnabled(instrumentHandle, portSelectorString, portExtensionDCLossEnabled));
        RFmxCheckWarn(RFmxVNA_SetCorrectionPortExtensionDCLoss(instrumentHandle, portSelectorString, portExtensionDCLoss));
        RFmxCheckWarn(RFmxVNA_SetCorrectionPortExtensionLoss1Enabled(instrumentHandle, portSelectorString, portExtensionLoss1Enabled));
        RFmxCheckWarn(RFmxVNA_SetCorrectionPortExtensionLoss2Enabled(instrumentHandle, portSelectorString, portExtensionLoss2Enabled));
        RFmxCheckWarn(RFmxVNA_SetCorrectionPortExtensionLoss1Frequency(instrumentHandle, portSelectorString, portExtensionLoss1Frequency));
        RFmxCheckWarn(RFmxVNA_SetCorrectionPortExtensionLoss2Frequency(instrumentHandle, portSelectorString, portExtensionLoss2Frequency));
        RFmxCheckWarn(RFmxVNA_SetCorrectionPortExtensionLoss1(instrumentHandle, portSelectorString, portExtensionLoss1));
        RFmxCheckWarn(RFmxVNA_SetCorrectionPortExtensionLoss2(instrumentHandle, portSelectorString, portExtensionLoss2));
    }
    RFmxCheckWarn(RFmxVNA_SetSweepType(instrumentHandle, "", sweepType));
    switch (sweepType) {
        case RFMXVNA_VAL_SWEEP_TYPE_LIST:
            RFmxCheckWarn(RFmxVNA_SetFrequencyList(instrumentHandle, "", frequencyList, FREQUENCY_LIST_SIZE));
            break;

        case RFMXVNA_VAL_SWEEP_TYPE_LINEAR:
            RFmxCheckWarn(RFmxVNA_SetStartFrequency(instrumentHandle, "", startFrequency));
            RFmxCheckWarn(RFmxVNA_SetStopFrequency(instrumentHandle, "", stopFrequency));
            RFmxCheckWarn(RFmxVNA_SetNumberOfPoints(instrumentHandle, "", numberOfFrequencyPoints));
            break;

        default:
            break;
    }
    RFmxCheckWarn(RFmxVNA_SetIFBandwidth(instrumentHandle, "", IFBandwidth));
    RFmxCheckWarn(RFmxVNA_BuildPortString("", "port1", MAX_SELECTOR_STRING, portSelectorString));
    RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, port1PowerLevel));
    RFmxCheckWarn(RFmxVNA_SetTestReceiverAttenuation(instrumentHandle, portSelectorString, port1TestReceiverAttenuation));
    RFmxCheckWarn(RFmxVNA_BuildPortString("", "port2", MAX_SELECTOR_STRING, portSelectorString));
    RFmxCheckWarn(RFmxVNA_SetPowerLevel(instrumentHandle, portSelectorString, port2PowerLevel));
    RFmxCheckWarn(RFmxVNA_SetTestReceiverAttenuation(instrumentHandle, portSelectorString, port2TestReceiverAttenuation));
    RFmxCheckWarn(RFmxVNA_SetAveragingEnabled(instrumentHandle, "", averagingEnabled));
    RFmxCheckWarn(RFmxVNA_SetAveragingCount(instrumentHandle, "", averagingCount));
    RFmxCheckWarn(RFmxVNA_SetTriggerType(instrumentHandle, "", triggerType));
    RFmxCheckWarn(RFmxVNA_SetTriggerMode(instrumentHandle, "", triggerMode));
    RFmxCheckWarn(RFmxVNA_SetTriggerDelay(instrumentHandle, "", triggerDelay));
    RFmxCheckWarn(RFmxVNA_SelectMeasurements(instrumentHandle, "", RFMXVNA_VAL_SPARAMS, RFMXVNA_VAL_FALSE));
    RFmxCheckWarn(RFmxVNA_SParamsSetNumberOfSParameters(instrumentHandle, "", NUMBER_OF_SPARAMS));
    for (index = 0; index < NUMBER_OF_SPARAMS; index++) {
        RFmxCheckWarn(RFmxVNA_BuildSParameterString("", index, MAX_SELECTOR_STRING, sParamSelectorString));
        RFmxCheckWarn(RFmxVNA_SParamsCfgSParameter(instrumentHandle, sParamSelectorString, sParamsSParameters[index]));
        RFmxCheckWarn(RFmxVNA_SParamsSetFormat(instrumentHandle, sParamSelectorString, sParamsFormats[index]));
    }
    RFmxCheckWarn(RFmxVNA_SParamsSetMagnitudeUnits(instrumentHandle, "", magnitudeUnits));
    RFmxCheckWarn(RFmxVNA_SParamsSetPhaseTraceType(instrumentHandle, "", phaseTraceType));
    RFmxCheckWarn(RFmxVNA_SParamsSetGroupDelayApertureMode(instrumentHandle, "", groupDelayApertureMode));
    RFmxCheckWarn(RFmxVNA_SParamsSetGroupDelayAperturePoints(instrumentHandle, "", groupDelayAperturePoints));
    RFmxCheckWarn(RFmxVNA_SParamsSetGroupDelayAperturePercentage(instrumentHandle, "", groupDelayAperturePercentage));
    RFmxCheckWarn(RFmxVNA_SParamsSetGroupDelayApertureFrequencySpan(instrumentHandle, "", groupDelayApertureFrequencySpan));
    RFmxCheckWarn(RFmxVNA_CalsetLoadFromFile(instrumentHandle, "", "", calsetFilePath));
    RFmxCheckWarn(RFmxVNA_SetCorrectionEnabled(instrumentHandle, "", RFMXVNA_VAL_CORRECTION_ENABLED_TRUE));
    RFmxCheckWarn(RFmxVNA_SetCorrectionInterpolationEnabled(instrumentHandle, "", interpolationEnabled));
    RFmxCheckWarn(RFmxVNA_SetCorrectionPortSubsetEnabled(instrumentHandle, "", portSubsetEnabled));
    RFmxCheckWarn(RFmxVNA_SetCorrectionPortSubsetFullPorts(instrumentHandle, "", "port1,port2"));
    RFmxCheckWarn(RFmxVNA_SetCorrectionPortSubsetResponsePorts(instrumentHandle, "", ""));
    RFmxCheckWarn(RFmxVNA_Initiate(instrumentHandle, "", ""));

    /* Fetch results */
    RFmxCheckWarn(RFmxVNA_SParamsGetNumberOfSParameters(instrumentHandle, "", &numberOfSParamsResult));

    RFmxCheckWarn(RFmxVNA_SParamsFetchXData(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
    if (actualArraySize > 0) {
        sParamsXDataResult = (float64*)malloc(sizeof(float64) * actualArraySize);
        if (sParamsXDataResult) {
            RFmxCheckWarn(RFmxVNA_SParamsFetchXData(instrumentHandle, "", timeout, sParamsXDataResult, actualArraySize, NULL));
        } else {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    sParamsY1DataResult = (float32**)malloc(sizeof(float32*) * numberOfSParamsResult);
    sParamsY2DataResult = (float32**)malloc(sizeof(float32*) * numberOfSParamsResult);
    if (sParamsY1DataResult && sParamsY2DataResult) {
        for (index = 0; index < numberOfSParamsResult; index++) {
            actualArraySize = 0;
            RFmxCheckWarn(RFmxVNA_BuildSParameterString("", index, MAX_SELECTOR_STRING, sParamSelectorString));
            RFmxCheckWarn(RFmxVNA_SParamsFetchYData(instrumentHandle, sParamSelectorString, timeout, NULL, NULL, 0, &actualArraySize));
            if (actualArraySize > 0) {
                sParamsY1DataResult[index] = (float32*)malloc(sizeof(float32) * actualArraySize);
                sParamsY2DataResult[index] = (float32*)malloc(sizeof(float32) * actualArraySize);
                if (sParamsY1DataResult[index] && sParamsY2DataResult[index]) {
                    RFmxCheckWarn(RFmxVNA_SParamsFetchYData(instrumentHandle, sParamSelectorString, timeout, sParamsY1DataResult[index], sParamsY2DataResult[index], actualArraySize, NULL));
                } else {
                    printf("malloc failed.\n");
                    goto Error;
                }
            }

            char* attrCorrectionLevel = (char*)malloc(sizeof(char) * MAX_SELECTOR_STRING);
            if (!attrCorrectionLevel) {
                printf("malloc failed.\n");
                goto Error;
            }
            RFmxCheckWarn(RFmxVNA_SParamsGetResultsCorrectionLevel(
                instrumentHandle,
                sParamSelectorString,
                actualArraySize,
                attrCorrectionLevel
            ));
            free(attrCorrectionLevel);
        }
    } else {
        printf("malloc failed.\n");
        goto Error;
    }
    RFmxCheckWarn(RFmxVNA_SParamsGetResultsCorrectionState(instrumentHandle, "", &correctionStateResult));
    RFmxCheckWarn(RFmxVNA_SParamsSetSnPDataFormat(instrumentHandle, "", RFMXVNA_VAL_SPARAMS_SNP_DATA_FORMAT_AUTO));
    RFmxCheckWarn(RFmxVNA_SParamsSetSnPPorts(instrumentHandle, "", "port1,port2"));
    RFmxCheckWarn(RFmxVNA_SParamsExportToSnPFile(instrumentHandle, "", snPFilePath));
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
    if (sParamsXDataResult) {
        free(sParamsXDataResult);
    }
    for (index = 0; index < numberOfSParamsResult; index++) {
        if (sParamsY1DataResult[index]) {
            free(sParamsY1DataResult[index]);
        }
        if (sParamsY2DataResult[index]) {
            free(sParamsY2DataResult[index]);
        }
    }
    if (sParamsY1DataResult) {
        free(sParamsY1DataResult);
    }
    if (sParamsY2DataResult) {
        free(sParamsY2DataResult);
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}
