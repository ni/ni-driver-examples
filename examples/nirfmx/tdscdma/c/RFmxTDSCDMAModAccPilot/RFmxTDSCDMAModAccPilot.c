//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Pilot.
//6. Select ModAcc measurement and enable traces.
//7. Configure ModAcc Averaging.
//8. Configure Slot Type.
//9. Initiate the Measurement.
//10. Fetch ModAcc Measurements and Traces.
//11. Close the RFmx session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxTDSCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

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
    float64 frequencyReferenceFrequency = 10E+6;              /*(Hz) */
    float64 centerFrequency = 1.91E+9;                        /*(Hz) */

    float64 referenceLevel = 0.00;                            /*(dBm) */
    float64 externalAttenuation = 0.00;                       /*(dB) */

    /*Trigger */
    float64 triggerDelay = 0.00;                              /*(s) */
    int32 enableTrigger = RFMXTDSCDMA_VAL_TRUE;
    char * IQPowerEdgeSource = "0";
    int32 IQPowerEdgeSlope = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_RISING_SLOPE;
    float64 IQPowerEdgeLevel = -20.00;                        /*(dB) */
    int32 minimumQuietTimeMode = RFMXTDSCDMA_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
    float64 minimumQuietTime = 50E-6;                         /*(s) */
    int32 IQPowerEdgeLevelType = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
    
	/*Pilot Settings*/
    int32 pilotCode = 0;
	int32 slotType = RFMXTDSCDMA_VAL_MODACC_SLOT_TYPE_PILOT;

	/*Averaging Settings*/
	int32 averagingEnabled = RFMXTDSCDMA_VAL_MODACC_AVERAGING_ENABLED_FALSE;           
    int32 averagingCount = 10;  

	int32 actualArraySize=0;
    float64 timeout = 10.00;					              /*(s) */

    /*Variables to Store Results*/
    float64 RMSPilotEVM = 0.00;				                  /*(%) */
    float64 peakPilotEVM = 0.00;			                  /*(%) */
    float64 pilotRho = 0.00;
    float64 RMSPilotMagnitudeError = 0.00;                    /*(%) */
	float64 RMSPilotPhaseError = 0.00;                        /*(deg) */
	float64 RMSCompositeEVM = 0.00;				              /*(%) */
    float64 peakCompositeEVM = 0.00;			              /*(%) */
    float64 compositeRho = 0.00;
    float64 RMSCompositeMagnitudeError = 0.00;                /*(%) */
	float64 RMSCompositePhaseError = 0.00;                    /*(deg) */
	float64 frequencyError = 0.00;				              /*(Hz) */
	float64 chipRateError = 0.00;	                          /*(ppm) */			              
	float64 IQOriginOffset = 0.00;                            /*(dB) */
	float64 IQGainImbalance = 0.0;                            /*(dB) */
	float64 IQQuadratureError = 0.0;                          /*(deg) */
    float64 x0 = 0.0, dx = 0.0;
	float32 *EVM=(float32*)NULL;                              /*(%) */
    NIComplexSingle* constellation = (NIComplexSingle*)NULL;

    /* Initialize a session */
    RFmxCheckWarn(RFmxTDSCDMA_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxTDSCDMA_CfgFrequencyReference(instrumentHandle,"",frequencyReferenceSource, 
		                                               frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxTDSCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxTDSCDMA_CfgIQPowerEdgeTrigger(instrumentHandle, "", IQPowerEdgeSource, IQPowerEdgeSlope,
														IQPowerEdgeLevel,triggerDelay, minimumQuietTimeMode, 
														minimumQuietTime,IQPowerEdgeLevelType,enableTrigger));
	RFmxCheckWarn(RFmxTDSCDMA_CfgPilot(instrumentHandle, "", pilotCode));
    RFmxCheckWarn(RFmxTDSCDMA_SelectMeasurements(instrumentHandle, "",RFMXTDSCDMA_VAL_MODACC , RFMXTDSCDMA_VAL_TRUE));
	RFmxCheckWarn(RFmxTDSCDMA_ModAccCfgAveraging(instrumentHandle,"",averagingEnabled,averagingCount));
	RFmxCheckWarn(RFmxTDSCDMA_ModAccCfgSlotType(instrumentHandle, "", slotType));
    RFmxCheckWarn(RFmxTDSCDMA_Initiate(instrumentHandle, "", ""));

    RFmxCheckWarn(RFmxTDSCDMA_ModAccFetchPilotEVM(instrumentHandle, "", timeout, &RMSPilotEVM, &peakPilotEVM, 
														&pilotRho, &RMSPilotMagnitudeError,   
                                                        &RMSPilotPhaseError ));
	RFmxCheckWarn(RFmxTDSCDMA_ModAccFetchCompositeEVM(instrumentHandle, "", timeout, &RMSCompositeEVM, &peakCompositeEVM, 
														&compositeRho, &frequencyError, &chipRateError, 
														&RMSCompositeMagnitudeError, &RMSCompositePhaseError ));  
                                                        
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

    RFmxCheckWarn(RFmxTDSCDMA_ModAccFetchIQImpairments(instrumentHandle, "" ,timeout, &IQOriginOffset,&IQGainImbalance,&IQQuadratureError));
    
	actualArraySize = 0;
	RFmxCheckWarn(RFmxTDSCDMA_ModAccFetchEVMTrace(instrumentHandle, "", timeout, &x0,&dx,NULL,0,&actualArraySize));
    if(actualArraySize>0)
    {
        EVM = (float32 *)malloc(sizeof(float32) * actualArraySize);
        if(EVM)
        {
            RFmxCheckWarn(RFmxTDSCDMA_ModAccFetchEVMTrace(instrumentHandle, "", timeout,&x0,&dx, EVM,
				actualArraySize,NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    printf("--------------------Pilot EVM Results--------------------\n");
    printf("RMS Pilot EVM  (%%)              : %lf\n",RMSPilotEVM);
    printf("Peak Pilot EVM  (%%)             : %lf\n",peakPilotEVM);
    printf("Pilot Rho                       : %lf\n",pilotRho);
	printf("RMS Pilot Magnitude Error  (%%)  : %lf\n",RMSPilotMagnitudeError);
    printf("RMS Pilot Phase Error  (deg)    : %lf\n",RMSPilotPhaseError);
    printf("Frequency Error  (Hz)           : %lf\n",frequencyError);

	printf("\n---------------------IQ Impairments------------------------\n");
	printf("I/Q Origin Offset (dB)          : %lf\n",IQOriginOffset);
	printf("I/Q Gain Imbalance (dB)         : %lf\n",IQGainImbalance);
	printf("I/Q Quadrature Error (deg)      : %lf\n",IQQuadratureError);

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
