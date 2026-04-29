// Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select QEVM measurement and enable Traces.
//6. Configure Averaging.
//7. Configure Measurement Length.
//8. Initiate the Measurement.
//9. Fetch QEVM Measurements and Traces.
//10. Close RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWCDMA.h"


/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle = NULL;   

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0, lastErrorCode = 0;
   
	float64 centerFrequency = 1.95e9;	            /*(Hz) */
    float64 referenceLevel = 0.000000;				/*(dBm) */
    float64 externalAttenuation = 0.000000;			/*(dB) */

    char * frequencyReferenceSource = RFMXWCDMA_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;		/*(Hz) */
    
    int32 enableTrigger = RFMXWCDMA_VAL_FALSE;
    char * digitalEdgeSource = RFMXWCDMA_VAL_PFI0_STR;
    int32 digitalEdge = RFMXWCDMA_VAL_DIGITAL_EDGE_RISING_EDGE;
    float64 triggerDelay = 0.000000;				/*(s) */

    int32 measurementLength = 2560;                 /*(chips)*/
    
    int32 averagingEnabled = RFMXWCDMA_VAL_QEVM_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;
    	
	float64 timeout = 10.000000;					/*(s) */
	int32 actualArraySize = 0;
	float64 x0 = 0.0, dx = 0.0;

    float64 meanPhaseError = 0.000000;				/*(deg) */
    float64 meanMagnitudeError = 0.000000;			/*(%) */
    float64 meanRMSEVM = 0.000000;			        /*(%) */
    float64 maximumPeakEVM = 0.000000;				/*(%) */
    float64 meanFrequencyError = 0.000000;			/*(Hz) */
	float64 meanChipRateError = 0.000000;			/*(ppm) */
	
	float64 maximumIQOriginOffset = 0.000000;		/*(dB) */
	float64 meanIQOriginOffset = 0.000000;			/*(dB) */
	
	float32* evmTrace = NULL ;						/*(dBm) */
	NIComplexSingle*  constellationTrace = NULL;
    /* Initialize a session */
    RFmxCheckWarn(RFmxWCDMA_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxWCDMA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, 
		                                          frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxWCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxWCDMA_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge,
		                                          triggerDelay, enableTrigger));
    RFmxCheckWarn(RFmxWCDMA_SelectMeasurements(instrumentHandle, "", RFMXWCDMA_VAL_QEVM, RFMXWCDMA_VAL_TRUE));
    RFmxCheckWarn(RFmxWCDMA_QEVMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
	RFmxCheckWarn(RFmxWCDMA_QEVMCfgMeasurementLength(instrumentHandle, "", measurementLength));
    RFmxCheckWarn(RFmxWCDMA_Initiate(instrumentHandle, "", ""));

    RFmxCheckWarn(RFmxWCDMA_QEVMFetchEVM(instrumentHandle, "", timeout,&meanRMSEVM ,&maximumPeakEVM ,&meanFrequencyError ,
		&meanMagnitudeError ,&meanPhaseError , &meanChipRateError ));
	
	RFmxCheckWarn(RFmxWCDMA_QEVMFetchIQImpairments(instrumentHandle, "", timeout,&meanIQOriginOffset ,NULL,
		                                           NULL,&maximumIQOriginOffset,NULL,NULL));
    
	 RFmxCheckWarn(RFmxWCDMA_QEVMFetchEVMTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		evmTrace = (float32 *) malloc(sizeof(float32) * actualArraySize);
		if( evmTrace )
		{
			RFmxCheckWarn(RFmxWCDMA_QEVMFetchEVMTrace(instrumentHandle, "", timeout, &x0, &dx, evmTrace, 
				                                      actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	 RFmxCheckWarn(RFmxWCDMA_QEVMFetchConstellationTrace(instrumentHandle, "", timeout,  NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		constellationTrace = (NIComplexSingle *) malloc(sizeof(NIComplexSingle) * actualArraySize);
		if( constellationTrace )
		{
			RFmxCheckWarn(RFmxWCDMA_QEVMFetchConstellationTrace(instrumentHandle, "", timeout, constellationTrace,
				                                                actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}
    
	printf("------------EVM------------\n");
	printf("Mean RMS EVM (%%)                    : %lf\n",meanRMSEVM);
	printf("Maximum Peak EVM (%%)                : %lf\n",maximumPeakEVM);
	printf("Mean Frequency Error (Hz)     	    : %lf\n",meanFrequencyError);
	printf("Mean Magnitude Error (%%)            : %lf\n", meanMagnitudeError);
	printf("Mean Phase Error (deg)              : %lf\n", meanPhaseError);
	printf("Mean Chip Rate Error (ppm)          : %lf\n\n",meanChipRateError);
	
	printf("------------IQ Impairments------------\n");
	printf("Mean I/Q Origin Offset (dB)         : %lf\n",meanIQOriginOffset);
	printf("Maximum I/Q Origin Offset (dB)      : %lf\n",maximumIQOriginOffset);

Error:
    if( error )
    {
        RFmxWCDMA_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
        if (error < 0)
            printf("ERROR: %s\n", errorMessage);
        else
            printf("WARNING: %s\n", errorMessage);
    }
    if(instrumentHandle)
    {
        RFmxWCDMA_Close(instrumentHandle, RFMXWCDMA_VAL_FALSE);
    }
    printf("Press any key to exit\n");
    _getch();

	/* Free allocated memory */
    if (evmTrace) 
	{
        free(evmTrace);
    }
    if (constellationTrace) 
	{
        free(constellationTrace);
    }

    return error;
}
