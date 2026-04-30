//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure operating Band.
//7. Configure Duplex Mode.
//8. Select Downlink as Link Direction.
//9. Configure Component Carriers.
//10. Configure Downlink Test Model.
//11. Select ModAcc measurement and enable Traces.
//12. Configure Averaging Parameters for ModAcc measurement.
//13. Select Frame as Synchronization Mode and configure Measurement Interval.
//14. Configure EVM Unit.
//15. Initiate the Measurement.
//16. Fetch ModAcc Measurements and Traces.
//17. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION           4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING             256
#define NUMBER_OF_COMPONENT_CARRIERS    2

int main (int argc, char *argv[])
{
    char *resourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle = NULL;    

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0, lastErrorCode = 0;
    int i = 0;

    char subblockCarrierString[NUMBER_OF_COMPONENT_CARRIERS][MAX_SELECTOR_STRING];

    char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;                                               /*(Hz) */

    float64 centerFrequency = 2.14e9;                                                         /*(Hz) */
    float64 referenceLevel = 0.0;                                                             /*(dBm)*/
    float64 externalAttenuation = 0.0;                                                        /*(dB) */

    int32 enableTrigger = RFMXLTE_VAL_FALSE;
    char * digitalEdgeTriggerSource = RFMXLTE_VAL_PFI0_STR;
    int32 digitalEdgeTriggerEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
    float64 triggerDelay = 0.0;                                                               /*(s) */

    int32 componentCarrierSpacingType = RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL;
    int32 componentCarrierAtCenterFrequency = -1;

    int32 band = 1;
    int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_FDD;
    int32 uplinkDownlinkConfiguraiton = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;
    int32 linkDirection = RFMXLTE_VAL_LINK_DIRECTION_DOWNLINK;

    float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS] = {20e6, 20e6};
    float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS] = {-9.9e6, 9.9e6};
    int32 downlinkTestModel[NUMBER_OF_COMPONENT_CARRIERS] = {RFMXLTE_VAL_DOWNLINK_TEST_MODEL_TM1_1, RFMXLTE_VAL_DOWNLINK_TEST_MODEL_TM1_1};

    int32 averagingEnabled = RFMXLTE_VAL_MODACC_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;

    int32 synchronizationMode = RFMXLTE_VAL_MODACC_SYNCHRONIZATION_MODE_FRAME;
    int32 measurementOffset = 0;                                                              /*(slots) */
    int32 measurementLength = 1;                                                              /*(slots) */

    int32 evmUnit = RFMXLTE_VAL_MODACC_EVM_UNIT_PERCENTAGE;

    float64 timeout = 10.000000;                                                              /*(s) */

    int32 peakCompositeEVMSubcarrierIndex[NUMBER_OF_COMPONENT_CARRIERS] = {0};
    int32 peakCompositeEVMSymbolIndex[NUMBER_OF_COMPONENT_CARRIERS] = {0};
    int32 peakCompositeEVMSlotIndex[NUMBER_OF_COMPONENT_CARRIERS] = {0};
    float64 meanRMSCompositeEVM[NUMBER_OF_COMPONENT_CARRIERS] = {0};                         /*(% or dB) */
    float64 meanRMSEVM[NUMBER_OF_COMPONENT_CARRIERS] = {0};                                  /*(% or dB) */
    float64 meanRMSQPSKEVM[NUMBER_OF_COMPONENT_CARRIERS] = {0};                              /*(% or dB) */
    float64 meanRMS16QAMEVM[NUMBER_OF_COMPONENT_CARRIERS] = {0};                             /*(% or dB) */
    float64 meanRMS64QAMEVM[NUMBER_OF_COMPONENT_CARRIERS] = {0};                             /*(% or dB) */
    float64 meanRMS256QAMEVM[NUMBER_OF_COMPONENT_CARRIERS] = {0};                            /*(% or dB) */
    float64 meanRMS1024QAMEVM[NUMBER_OF_COMPONENT_CARRIERS] = {0};                           /*(% or dB) */
    float64 maximumPeakCompositeEVM[NUMBER_OF_COMPONENT_CARRIERS] = {0};                     /*(% or dB) */
    float64 meanFrequencyError[NUMBER_OF_COMPONENT_CARRIERS] = {0};                          /*(Hz) */

    float64 meanIQOriginOffset[NUMBER_OF_COMPONENT_CARRIERS] = {0};                           /*(dBc) */
    float64 meanIQGainImbalance[NUMBER_OF_COMPONENT_CARRIERS] = {0};                          /*(dB) */
    float64 meanIQQuadratureError[NUMBER_OF_COMPONENT_CARRIERS] = {0};                        /*(deg) */

    NIComplexSingle* QPSKConstellation[NUMBER_OF_COMPONENT_CARRIERS] = {NULL};

    float32* meanRMSEVMPerSubcarrier[NUMBER_OF_COMPONENT_CARRIERS] = {NULL};
    float64 x0[NUMBER_OF_COMPONENT_CARRIERS];
    float64 dx[NUMBER_OF_COMPONENT_CARRIERS];

    int32 actualArraySize = 0;
    int32 QPSKConstellationActualArraySize = 0;

    /* Initialize a session */
    RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "",frequencyReferenceSource, 
                                    frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, 
                                                                  enableTrigger));
    RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, "", componentCarrierSpacingType, 
                                          componentCarrierAtCenterFrequency));
    RFmxCheckWarn(RFmxLTE_CfgBand(instrumentHandle, "", band));
    RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));
    RFmxCheckWarn(RFmxLTE_CfgLinkDirection(instrumentHandle, "", linkDirection));
    RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, "", NUMBER_OF_COMPONENT_CARRIERS));
    RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, "", componentCarrierBandwidth, componentCarrierFrequency, 
                                                        0, NUMBER_OF_COMPONENT_CARRIERS));
    RFmxCheckWarn(RFmxLTE_CfgDownlinkTestModelArray(instrumentHandle, "", downlinkTestModel, NUMBER_OF_COMPONENT_CARRIERS));
    RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_MODACC, RFMXLTE_VAL_TRUE));
    RFmxCheckWarn(RFmxLTE_ModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
    RFmxCheckWarn(RFmxLTE_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode, measurementOffset, measurementLength));
    RFmxCheckWarn(RFmxLTE_ModAccCfgEVMUnit(instrumentHandle, "", evmUnit));
    RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

    RFmxCheckWarn(RFmxLTE_ModAccFetchCompositeEVMArray(instrumentHandle, "", timeout, meanRMSCompositeEVM, 
                maximumPeakCompositeEVM, meanFrequencyError, peakCompositeEVMSymbolIndex, 
            peakCompositeEVMSubcarrierIndex, peakCompositeEVMSlotIndex,
                NUMBER_OF_COMPONENT_CARRIERS, NULL));

    RFmxCheckWarn(RFmxLTE_ModAccFetchIQImpairmentsArray(instrumentHandle, "", timeout, meanIQOriginOffset, 
                meanIQGainImbalance,meanIQQuadratureError, NUMBER_OF_COMPONENT_CARRIERS, NULL));

    RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCHEVMArray(instrumentHandle, "", timeout, meanRMSEVM, meanRMSQPSKEVM,
                meanRMS16QAMEVM, meanRMS64QAMEVM , meanRMS256QAMEVM, NUMBER_OF_COMPONENT_CARRIERS, NULL));

    RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCH1024QAMEVMArray(instrumentHandle, "", timeout, meanRMS1024QAMEVM, NUMBER_OF_COMPONENT_CARRIERS, NULL));


    for( i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
    {        
        RFmxLTE_BuildCarrierString("", i, MAX_SELECTOR_STRING, subblockCarrierString[i]);
        RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCHQPSKConstellation(instrumentHandle, subblockCarrierString[i], timeout,
               NULL,0, &QPSKConstellationActualArraySize));
                                                                    

        if( QPSKConstellationActualArraySize > 0 )
        {
            QPSKConstellation[i] = (NIComplexSingle *) malloc(sizeof(NIComplexSingle) * QPSKConstellationActualArraySize);
            if( QPSKConstellation[i])
            {
                RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCHQPSKConstellation(instrumentHandle, subblockCarrierString[i], timeout, 
                                                                            QPSKConstellation[i],
                                                         QPSKConstellationActualArraySize, NULL));
         }
            else
            {
                printf("malloc failed.\n");
                goto Error;
            }
        }

        actualArraySize = 0;
        RFmxCheckWarn(RFmxLTE_ModAccFetchEVMPerSubcarrierTrace(instrumentHandle, subblockCarrierString[i], timeout, NULL, NULL, NULL, 0, &actualArraySize));
        if( actualArraySize > 0 )
        {
            meanRMSEVMPerSubcarrier[i] = (float32 *) malloc(sizeof(float32) * actualArraySize);
            if( meanRMSEVMPerSubcarrier[i] )
            {
                RFmxCheckWarn(RFmxLTE_ModAccFetchEVMPerSubcarrierTrace(instrumentHandle, subblockCarrierString[i], timeout, &x0[i], &dx[i], 
                                                                            meanRMSEVMPerSubcarrier[i], actualArraySize, 0));
            }
            else
            {
                printf("malloc failed.\n");
                goto Error;
            }
        }
    }

    printf("------------------------Measurements------------------------\n\n"    );
    for( i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++ )
    {
        printf("Carrier %d\n",i);
        printf("Mean RMS Composite EVM  (%% or dB)      : %lf\n",meanRMSCompositeEVM[i]);
        printf("Mean RMS  EVM  (%% or dB)               : %lf\n",meanRMSEVM[i]);
        printf("Mean RMS QPSK EVM  (%% or dB)           : %lf\n",meanRMSQPSKEVM[i]);
        printf("Mean RMS 16QAM EVM  (%% or dB)          : %lf\n",meanRMS16QAMEVM[i]);
        printf("Mean RMS 64QAM EVM  (%% or dB)          : %lf\n",meanRMS64QAMEVM[i]);
        printf("Mean RMS 256QAM EVM  (%% or dB)         : %lf\n",meanRMS256QAMEVM[i]);
        printf("Mean RMS 1024QAM EVM  (%% or dB)        : %lf\n",meanRMS1024QAMEVM[i]);
        printf("Mean Frequency Error  (Hz)             : %lf\n",meanFrequencyError[i]);
        printf("Mean IQ Origin Offset  (dBc)           : %lf\n",meanIQOriginOffset[i]);
        printf("Mean IQ Gain Imbalance  (dB)           : %lf\n",meanIQGainImbalance[i]);
        printf("Mean IQ Quadrature Error  (deg)        : %lf\n",meanIQQuadratureError[i]);
        printf("-------------------------------------------------\n");
    }

Error:
    if( error )
    {
        RFmxLTE_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
        if (error < 0)
            printf("ERROR: %s\n", errorMessage);
        else
            printf("WARNING: %s\n", errorMessage);
    }

    if(instrumentHandle)
    {
        RFmxLTE_Close(instrumentHandle, RFMXLTE_VAL_FALSE);
    }

    /* Free allocated memory */
    
    for( i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++ )
    {
        if (QPSKConstellation[i])
        {
            free(QPSKConstellation[i]);
        }
        if (meanRMSEVMPerSubcarrier[i])
        {
            free(meanRMSEVMPerSubcarrier[i]);
        }
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}
