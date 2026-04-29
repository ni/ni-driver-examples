//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select ModAcc measurement and enable traces.
//6. Configure Synchronization Mode and Measurement Interval.
//7. Configure Channel Configuration Mode.
//8. Configure Physical Layer Subtype.
//9. Configure Uplink Data Modulation Type.
//10. Configure Uplink Spreading Parameters.
//11. Initiate the Measurement.
//12. Fetch ModAcc Measurements and Traces.
//13. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxEVDO.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main ()
{
    niRFmxInstrHandle instrumentHandle = NULL;
	char *resourceName = "RFSA";    

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0, lastErrorCode = 0;

    float64 centerFrequency = 833.49e6; /*(Hz) */
    float64 referenceLevel = 0.00;      /*(dBm) */
    float64 externalAttenuation = 0.00; /*(dB) */

    char * frequencyReferenceSource = RFMXEVDO_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;           /*(Hz) */

    int32 enableTrigger = RFMXEVDO_VAL_FALSE;
    char * digitalEdgeSource = RFMXEVDO_VAL_PFI0_STR;
    int32 digitalEdge = RFMXEVDO_VAL_DIGITAL_EDGE_RISING_EDGE;
    float64 triggerDelay = 0.00;        /*(s) */

    int32 channelConfigurationMode = RFMXEVDO_VAL_CHANNEL_CONFIGURATION_MODE_AUTO_DETECT;
    int32 physicalLayerSubtype = RFMXEVDO_VAL_PHYSICAL_LAYER_SUBTYPE_0_1;
    int32 uplinkDataModulationType = RFMXEVDO_VAL_UPLINK_DATA_MODULATION_TYPE_AUTO;
    int32 uplinkSpreadingIMask = 0x0, uplinkSpreadingQMask = 0x0;

    int32 synchronizationMode = RFMXEVDO_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;
    int32 measurementOffset = 0;  /*(slots) */
    int32 measurementLength = 1;  /*(slots) */
    float64 timeout = 10.00;      /*(s) */

    float64 x0 = 0.0, dx = 0.0;
    int32 actualArraySize = 0;

    float64 chipRateError = 0.00;       /*(ppm) */
    float64 frequencyError = 0.00;      /*(Hz) */
    float64 rmsEVM = 0.00;              /*(%) */
    float64 peakEVM = 0.00;             /*(%) */
    float64 rho = 0.00;
    float64 rmsPhaseError = 0.00;       /*(deg) */
    float64 rmsMagnitudeError = 0.00;   /*(%) */

    float64 IQOriginOffset = 0.00;      /*(dB) */
    float64 IQGainImbalance = 0.00;     /*(dB) */
    float64 IQQuadratureError = 0.00;   /*(deg) */

    int32 peakCDEBranch = 0;
    float64 peakCDE = 0.00;             /*(dB) */
    int32 peakCDECode = 0;

    int32 peakActiveCDEBranch = 0;
    float64 peakActiveCDE = 0.00;       /*(dB) */
    int32 peakActiveCDESpreadingFactor = 0;
    int32 peakActiveCDECode = 0;
    int32 uplinkDetectedDataModulationType = RFMXEVDO_VAL_MODACC_UPLINK_DETECTED_DATA_MODULATION_TYPE_DATA_CHANNEL_ABSENT;

    float32* evm = NULL;                /*(%) */
    NIComplexSingle* constellation = NULL;

    /* Initialize a session */ 
    RFmxCheckWarn(RFmxEVDO_Initialize(resourceName, "", &instrumentHandle, NULL));

    RFmxCheckWarn(RFmxEVDO_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
		frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxEVDO_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxEVDO_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, 
		enableTrigger));
    RFmxCheckWarn(RFmxEVDO_SelectMeasurements(instrumentHandle, "", RFMXEVDO_VAL_MODACC, RFMXEVDO_VAL_TRUE));
    RFmxCheckWarn(RFmxEVDO_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode, 
		measurementOffset, measurementLength));
    RFmxCheckWarn(RFmxEVDO_CfgChannelConfigurationMode(instrumentHandle, "", channelConfigurationMode));
    RFmxCheckWarn(RFmxEVDO_CfgPhysicalLayerSubtype(instrumentHandle, "", physicalLayerSubtype));
    RFmxCheckWarn(RFmxEVDO_CfgUplinkDataModulationType(instrumentHandle, "", uplinkDataModulationType));
    RFmxCheckWarn(RFmxEVDO_CfgUplinkSpreading(instrumentHandle, "", uplinkSpreadingIMask, uplinkSpreadingQMask));
    RFmxCheckWarn(RFmxEVDO_Initiate(instrumentHandle, "", ""));

    RFmxCheckWarn(RFmxEVDO_ModAccFetchUplinkEVM(instrumentHandle, "", timeout, &rmsEVM, &peakEVM, &rho, &frequencyError,
		&chipRateError, &rmsMagnitudeError, &rmsPhaseError));
    RFmxCheckWarn(RFmxEVDO_ModAccFetchIQImpairments(instrumentHandle, "", timeout, &IQOriginOffset, 
		&IQGainImbalance, &IQQuadratureError));
    RFmxCheckWarn(RFmxEVDO_ModAccFetchUplinkPeakCDE(instrumentHandle, "", timeout, &peakCDE, &peakCDECode, &peakCDEBranch));
    RFmxCheckWarn(RFmxEVDO_ModAccFetchUplinkPeakActiveCDE(instrumentHandle, "", timeout, &peakActiveCDE,
		&peakActiveCDESpreadingFactor, &peakActiveCDECode, &peakActiveCDEBranch));
    RFmxCheckWarn(RFmxEVDO_ModAccFetchUplinkDetectedDataModulationType(instrumentHandle, "", timeout, 
		&uplinkDetectedDataModulationType));

    actualArraySize = 0;
    RFmxCheckWarn(RFmxEVDO_ModAccFetchEVMTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
    if( actualArraySize > 0 )
    {
        evm = (float32 *) malloc(sizeof(float32) * actualArraySize);
        if( evm )
        {
            RFmxCheckWarn(RFmxEVDO_ModAccFetchEVMTrace(instrumentHandle, "", timeout, &x0, &dx, evm, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    actualArraySize = 0;
    RFmxCheckWarn(RFmxEVDO_ModAccFetchConstellationTrace(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
    if( actualArraySize > 0 )
    {
        constellation = (NIComplexSingle *) malloc(sizeof(NIComplexSingle) * actualArraySize);
        if( constellation )
        {
            RFmxCheckWarn(RFmxEVDO_ModAccFetchConstellationTrace(instrumentHandle, "", timeout, constellation, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    
    printf("\n------------EVM------------\n");
    printf("RMS EVM (%%)                          : %lf\n",rmsEVM);
    printf("Peak EVM (%%)                         : %lf\n",peakEVM);
    printf("Rho                                  : %lf\n",rho);
    printf("Frequency Error (Hz)                 : %lf\n",frequencyError);
    printf("Chip Rate Error (ppm)                : %lf\n",chipRateError);
    printf("RMS Magnitude Error (%%)              : %lf\n",rmsMagnitudeError);
    printf("RMS Phase Error (deg)                : %lf\n",rmsPhaseError);
											    
    printf("\nI/Q Origin Offset (dB)               : %lf\n",IQOriginOffset);
	printf("I/Q Gain Imbalance (dB)              : %lf\n",IQGainImbalance);
	printf("I/Q Quadrature Error (deg)           : %lf\n",IQQuadratureError);
											   
    printf("\n------------Code Domain Error------------\n");
    printf("Peak CDE (dB)                        : %lf\n",peakCDE);
    printf("Peak CDE Code                        : %d\n",peakCDECode);
	printf("Peak CDE Branch                      : %c\n",(peakCDEBranch == 0)? 'I':'Q');
    printf("Peak Active CDE (dB)                 : %lf\n",peakActiveCDE);
    printf("Peak Active CDE Code                 : %d\n",peakActiveCDECode);
    printf("Peak Active CDE Spreading Factor     : %d\n",peakActiveCDESpreadingFactor);
	printf("Peak Active CDE Branch               : %c\n\n",(peakActiveCDEBranch == 0)? 'I':'Q');


	printf("\nUplink Detected Data Modulation Type : %d\n\n", uplinkDetectedDataModulationType);


Error:
    if( error )
    {
        RFmxEVDO_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
        if (error < 0)
            printf("ERROR: %s\n", errorMessage);
        else
            printf("WARNING: %s\n", errorMessage);
    }
    if(instrumentHandle)
    {
        RFmxEVDO_Close(instrumentHandle, RFMXEVDO_VAL_FALSE);
    }
    printf("Press any key to exit\n");
    _getch();

    /* Free allocated memory */
    if (evm)
    {
        free(evm);
    }
    if (constellation)
    {
        free(constellation);
    }

    return error;
}
