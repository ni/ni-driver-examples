//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters..
//5. Select ModAcc measurement and enable traces.
//6. Configure Uplink Scrambling Code.
//7. Configure Synchronization Mode and Measurement Interval.
//8. Configure Midamble Parameters.
//9. Initiate the Measurement.
//10. Fetch ModAcc Measurements and Traces.
//11. Close the RFmx session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxTDSCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096
#define MAX_NUMBER_OF_USERS         16

int main ()
{
    //RFSA Configuration
	char *rfsaResourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle = NULL;    
    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0;
    int32 lastErrorCode = 0;

    /*Frequency Reference*/
    char * frequencyReferenceSource = RFMXTDSCDMA_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10E+6;/*(Hz) */
    float64 centerFrequency = 1.91E+9;          /*(Hz) */

    float64 referenceLevel = 0.00;              /*(dBm) */
    float64 externalAttenuation = 0.00;         /*(dB) */

    /*Trigger */
    float64 triggerDelay = 0.00;                /*(s) */
    int32 enableTrigger = RFMXTDSCDMA_VAL_TRUE;
    char * IQPowerEdgeSource = "0";
    int32 IQPowerEdgeSlope = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_RISING_SLOPE;
    float64 IQPowerEdgeLevel = -20.00;          /*(dB) */
    int32 minimumQuietTimeMode = RFMXTDSCDMA_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
    float64 minimumQuietTime = 80E-6;           /*(s) */
    int32 IQPowerEdgeLevelType = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;

    int32 uplinkScramblingCode = 0;
    int32 synchronizationMode = RFMXTDSCDMA_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;

    /*Measurement Settings*/
    int32 measurementOffset = 0;                /*(slots) */
    int32 measurementLength = 1;                /*(slots) */

    /*Midamble Settings*/
    int32 midambleAutoDetectionMode = RFMXTDSCDMA_VAL_MIDAMBLE_AUTO_DETECTION_MODE_MIDAMBLE_SHIFT;
    int32 maximumNumberOfUsers = MAX_NUMBER_OF_USERS;
    int32 midambleShift = 8;					/*(chips) */
	int32 actualArraySize=0;

    float64 timeout = 10.00;					/*(s) */

	/*Averaging Settings*/
	int32 averagingEnabled = RFMXTDSCDMA_VAL_MODACC_AVERAGING_ENABLED_FALSE;           
    int32 averagingCount = 10;            

    /*Variables to Store Results*/
    float64 chipRateError = 0.00;				/*(ppm) */
    float64 frequencyError = 0.00;				/*(Hz) */

    float64 RMSCompositeEVM = 0.00;				/*(%) */
    float64 peakCompositeEVM = 0.00;			/*(%) */
    float64 compositeRho = 0.00;
    float64 RMSCompositePhaseError = 0.00;      /*(deg) */
    float64 RMSCompositeMagnitudeError = 0.00;  /*(%) */
    float64 RMSDataPhaseError = 0.00;			/*(deg) */
    float64 RMSDataMagnitudeError = 0.00;       /*(%) */
    float64 RMSDataEVM = 0.00;					/*(%) */
    float64 peakDataEVM = 0.00;					/*(%) */
    float64 dataRho = 0.00;
    float64 RMSMidamblePhaseError = 0.00;       /*(deg) */
    float64 RMSMidambleMagnitudeError = 0.00;   /*(%) */
	float64 IQOriginOffset = 0.00;
	float64 IQGainImbalance = 0.0;
	float64 IQQuadratureError = 0.0;
    float64 RMSMidambleEVM = 0.00;				/*(%) */
    float64 peakMidambleEVM = 0.00;				/*(%) */
    float64 midambleRho = 0.00;
    float64 midamblePower = 0.00;				/*(dBm) */
    float64 dataField1Power = 0.00;				/*(dBm) */
    float64 dataField2Power = 0.00;				/*(dBm) */
    float64 x0 = 0.0, dx = 0.0;
	float32 *EVM=(float32*)NULL;
    NIComplexSingle* constellation = (NIComplexSingle*)NULL;

    /* Initialize a session */
    RFmxCheckWarn(RFmxTDSCDMA_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxTDSCDMA_CfgFrequencyReference(instrumentHandle,"",frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxTDSCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxTDSCDMA_CfgIQPowerEdgeTrigger(instrumentHandle, "", IQPowerEdgeSource, IQPowerEdgeSlope,
														IQPowerEdgeLevel,triggerDelay, minimumQuietTimeMode, 
														minimumQuietTime,IQPowerEdgeLevelType,enableTrigger));
    RFmxCheckWarn(RFmxTDSCDMA_SelectMeasurements(instrumentHandle, "",RFMXTDSCDMA_VAL_MODACC , RFMXTDSCDMA_VAL_TRUE));
	RFmxCheckWarn(RFmxTDSCDMA_ModAccCfgAveraging(instrumentHandle,"",averagingEnabled,averagingCount));
    RFmxCheckWarn(RFmxTDSCDMA_CfgUplinkScramblingCode(instrumentHandle, "", uplinkScramblingCode));
    RFmxCheckWarn(RFmxTDSCDMA_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode,
														measurementOffset, measurementLength));
    RFmxCheckWarn(RFmxTDSCDMA_CfgMidambleShift(instrumentHandle, "",midambleAutoDetectionMode, maximumNumberOfUsers, midambleShift));
    RFmxCheckWarn(RFmxTDSCDMA_Initiate(instrumentHandle, "", ""));

    RFmxCheckWarn(RFmxTDSCDMA_ModAccFetchCompositeEVM(instrumentHandle, "", timeout, &RMSCompositeEVM, &peakCompositeEVM, 
														&compositeRho, &frequencyError, &chipRateError,   
                                                        &RMSCompositeMagnitudeError, &RMSCompositePhaseError ));
    RFmxCheckWarn(RFmxTDSCDMA_ModAccFetchDataEVM(instrumentHandle, "", timeout, &RMSDataEVM, &peakDataEVM, 
                                                         &dataRho, &RMSDataMagnitudeError, &RMSDataPhaseError));
	RFmxCheckWarn(RFmxTDSCDMA_ModAccFetchIQImpairments(instrumentHandle, "" ,timeout, &IQOriginOffset,&IQGainImbalance,&IQQuadratureError));
    RFmxCheckWarn(RFmxTDSCDMA_ModAccFetchConstellationTrace(instrumentHandle, "", timeout, NULL,0,&actualArraySize));
    if(actualArraySize>0)
    {
        constellation = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
        if(constellation)
        {
            RFmxCheckWarn(RFmxTDSCDMA_ModAccFetchConstellationTrace(instrumentHandle, "", timeout, constellation,
				actualArraySize,NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }
    RFmxCheckWarn(RFmxTDSCDMA_ModAccFetchMidambleEVM(instrumentHandle, "", timeout,
														&RMSMidambleEVM, &peakMidambleEVM, &midambleRho,                                                
														&RMSMidambleMagnitudeError, &RMSMidamblePhaseError
                                                        ));
    RFmxCheckWarn(RFmxTDSCDMA_ModAccFetchMidambleAndDataPower(instrumentHandle, "", timeout, &midamblePower, 
		&dataField1Power, &dataField2Power));
   RFmxCheckWarn(RFmxTDSCDMA_ModAccFetchEVMTrace(instrumentHandle, "", timeout, &x0,&dx,NULL,0,&actualArraySize));
   if(actualArraySize>0)
    {
        EVM = (float32 *)malloc(sizeof(float32) * actualArraySize);
        if(EVM)
        {
            RFmxCheckWarn(RFmxTDSCDMA_ModAccFetchEVMTrace(instrumentHandle, "", timeout,&x0,&dx, EVM,
				actualArraySize,&actualArraySize));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    printf("Composite EVM Results\n");
    printf("RMS Composite EVM  (%%)             : %lf\n",RMSCompositeEVM);
    printf("Peak Composite EVM  (%%)            : %lf\n",peakCompositeEVM);
    printf("Composite Rho                      : %lf\n",compositeRho);
    printf("Frequency Error  (Hz)              : %lf\n",frequencyError);
    printf("Chip Rate Error  (ppm)             : %lf\n",chipRateError);
    printf("RMS Composite Phase Error  (deg)   : %lf\n",RMSCompositePhaseError);
    printf("RMS Composite Magnitude Error  (%%) : %lf\n",RMSCompositeMagnitudeError);
    
    printf("\nData EVM Results\n");    
    printf("RMS Data EVM  (%%)                  : %lf\n",RMSDataEVM);
    printf("Peak Data EVM  (%%)                 : %lf\n",peakDataEVM);
    printf("Data Rho                           : %lf\n",dataRho);
    printf("RMS Data Phase Error  (deg)        : %lf\n",RMSDataPhaseError);
    printf("RMS Data Magnitude Error  (%%)      : %lf\n",RMSDataMagnitudeError);
    printf("Data Field1 Power  (dBm)           : %lf\n",dataField1Power);
    printf("Data Field2 Power  (dBm)           : %lf\n",dataField2Power);

	printf("\nIQ Impairments\n");
	printf("I/Q Origin Offset (dB)             : %lf\n",IQOriginOffset);
	printf("I/Q Gain Imbalance (dB)            : %lf\n",IQGainImbalance);
	printf("I/Q Quadrature Error (deg)         : %lf\n",IQQuadratureError);

    printf("\nMidamble EVM\n");
    printf("RMS Midamble EVM  (%%)              : %lf\n",RMSMidambleEVM);
    printf("Peak Midamble EVM  (%%)             : %lf\n",peakMidambleEVM);
    printf("Midamble Rho                       : %lf\n",midambleRho);
    printf("RMS Midamble Phase Error  (deg)    : %lf\n",RMSMidamblePhaseError);
    printf("RMS Midamble Magnitude Error  (%%)  : %lf\n",RMSMidambleMagnitudeError);   
    printf("Midamble Power  (dBm)              : %lf\n",midamblePower);

Error:
    if( error )
    {
        RFmxTDSCDMA_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
        if (error < 0)
            printf("ERROR: %s\n", errorMessage);
        else
            printf("WARNING: %s\n", errorMessage);
    }
    if(instrumentHandle)
    {
        RFmxTDSCDMA_Close(instrumentHandle, RFMXTDSCDMA_VAL_FALSE);
    }

    if (constellation)
    {
        free(constellation);
    }
    if (EVM)
    {
        free(EVM);
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}
