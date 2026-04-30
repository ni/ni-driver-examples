//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Radio Configuration Parameter.
//6. Configure Uplink Spreading Long Code Mask Parameter.
//7. Select QEVM measurement and enable traces.
//8. Configure Synchronization Mode.
//9. Configure  Measurement Mode & Measurement Length.
//10. Initiate the Measurement.
//11. Fetch QEVM Measurements and Traces.
//12. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxCDMA2k.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char *argv[])
{
	char *resourceName = "RFSA";	
	niRFmxInstrHandle instrumentHandle = NULL;
	int32 error = 0,  lastErrorCode = 0;
	char errorMessage[MAX_ERROR_DESCRIPTION] = {'\0'};

	float64 centerFrequency = 833.490e+6;																/* Hz */
	float64 referenceLevel = 0.00;																		/* dBm */
	float64 externalAttenuation = 0.00;																	/* dB */

	/* Frequency Reference */
	float64 frequencyReferenceFrequency = 10.0e+6;													/* Hz */
	char *frequencySource = RFMXCDMA2K_VAL_ONBOARD_CLOCK_STR;	

	/* Trigger */
	float64 triggerDelay = 0.00;																			/* s */
	int32 enableTrigger = RFMXCDMA2K_VAL_FALSE;
	char* digitalEdgeSource = RFMXCDMA2K_VAL_PFI0_STR;
	int32 digitalEdge = RFMXCDMA2K_VAL_DIGITAL_EDGE_RISING_EDGE;

	int32 radioConfiguration = RFMXCDMA2K_VAL_RADIO_CONFIGURATION_RC3;
	int64 uplinkSpreadingLongCodeMask = 0;
	int32 measurementLength = 1536;																			/* chips */

	int32 averagingEnabled = RFMXCDMA2K_VAL_QEVM_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;

	/* Variables to store the measurement results */

	/* Store EVM Results*/
	float64 meanRMSEVM = 0.0;					     				/* % */
	float64 maximumPeakEVM = 0.0;				     				/* % */	
	float64 meanFrequencyError = 0.0;			     				/* Hz */
	float64 meanChipRateError = 0.0;			     				/* ppm */
	float64 meanMagnitudeError = 0.0;			     				/* % */
	float64 meanPhaseError = 0.0;				     				/* deg */
												     
	/*IQ Impairments*/							     
	float64 meanIQOriginOffset = 0.0;			     				/* dB */
	float64 meanIQGainImbalance = 0.0;			     				/* dB */
	float64 meanIQQuadratureError = 0.0;		     				/* deg */
	float64 maximumIQOriginOffset = 0.0;		     				/* dB */
	float64 maximumIQGainImbalance = 0.0;		        			/* dB */
	float64 maximumIQQuadratureError = 0.0;		     				/* deg */
	
	/* variables to store traces */
	float64 x0 = 0.0, dx = 0.0;	
	float32 *EVMTrace = (float32 *)NULL;
	int32 actualArraySize = 0;
	NIComplexSingle* constellation = NULL;	

	float64 timeout = 10.0;											/* seconds */	

	/* Create new RFmx session */
	RFmxCheckWarn(RFmxCDMA2k_Initialize(resourceName, "", &instrumentHandle, NULL ));		
	RFmxCheckWarn(RFmxCDMA2k_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequencyReferenceFrequency ));
	RFmxCheckWarn(RFmxCDMA2k_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation ));
	RFmxCheckWarn(RFmxCDMA2k_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger ));
	RFmxCheckWarn(RFmxCDMA2k_CfgRadioConfiguration(instrumentHandle, "", radioConfiguration));
	RFmxCheckWarn(RFmxCDMA2k_CfgUplinkSpreading(instrumentHandle, "", uplinkSpreadingLongCodeMask ));
	RFmxCheckWarn(RFmxCDMA2k_SelectMeasurements(instrumentHandle, "", RFMXCDMA2K_VAL_QEVM, RFMXCDMA2K_VAL_TRUE));
	RFmxCheckWarn(RFmxCDMA2k_QEVMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
	RFmxCheckWarn(RFmxCDMA2k_QEVMCfgMeasurementLength(instrumentHandle, "", measurementLength));
	RFmxCheckWarn(RFmxCDMA2k_Initiate(instrumentHandle, "", ""));	

	/* Retrieve results */	

	RFmxCheckWarn(RFmxCDMA2k_QEVMFetchEVM(instrumentHandle, "", timeout, &meanRMSEVM, &maximumPeakEVM, &meanFrequencyError, 
														&meanMagnitudeError, &meanPhaseError, &meanChipRateError ));
	RFmxCheckWarn(RFmxCDMA2k_QEVMFetchIQImpairments(instrumentHandle, "", timeout, &meanIQOriginOffset, &meanIQGainImbalance, 
														&meanIQQuadratureError, &maximumIQOriginOffset, &maximumIQGainImbalance, &maximumIQQuadratureError ));	
	RFmxCheckWarn(RFmxCDMA2k_QEVMFetchEVMTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize ));
	if(actualArraySize > 0)
	{
		EVMTrace = (float32*)malloc(actualArraySize * sizeof(float32));
		if(EVMTrace)
		{
			RFmxCheckWarn(RFmxCDMA2k_QEVMFetchEVMTrace(instrumentHandle, "", timeout, &x0, &dx, EVMTrace, actualArraySize, NULL ));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}	
	}	
	actualArraySize = 0;
	RFmxCheckWarn(RFmxCDMA2k_QEVMFetchConstellationTrace(instrumentHandle, "", timeout, NULL, 0, &actualArraySize ));
	if(actualArraySize > 0)
	{
		constellation = (NIComplexSingle *) malloc(actualArraySize * sizeof(NIComplexSingle));
		if(constellation)
		{			
			RFmxCheckWarn(RFmxCDMA2k_QEVMFetchConstellationTrace(instrumentHandle, "", timeout, constellation, actualArraySize, NULL ));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	/* Display results */

	printf("--------------EVM ------------------------------------------\n");
	printf("Mean RMS EVM (%%)                        : %f\n", meanRMSEVM);	
	printf("Maximum Peak EVM (%%)                    : %f\n", maximumPeakEVM);
	printf("Mean Frequency Error (Hz)               : %f\n", meanFrequencyError);
	printf("Mean Chip Rate Error (ppm)              : %f\n", meanChipRateError );
	printf("Mean Magnitude Error (%%)                : %f\n", meanMagnitudeError);
	printf("Mean Phase Error (deg)                  : %f\n", meanPhaseError);	

	printf("\n\n--------------I/Q Impairments----------------------------\n");
	printf("Mean I/Q Origin Offset (dB)             : %f\n", meanIQOriginOffset);
	printf("Mean I/Q Gain Imbalance (dB)            : %f\n", meanIQGainImbalance);
	printf("Mean I/Q Quadrature Error (deg)         : %f\n", meanIQQuadratureError);
	printf("Maximum I/Q Origin Offset (dB)          : %f\n", maximumIQOriginOffset);
	printf("Maximum I/Q Gain Imbalance (dB)         : %f\n", maximumIQGainImbalance);
	printf("Maximum I/Q Quadrature Error (deg)      : %f\n", maximumIQQuadratureError);	

Error:
	if( error ) 
	{
		RFmxCDMA2k_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if(error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}
	if(instrumentHandle)
	{
		RFmxCDMA2k_Close(instrumentHandle, RFMXCDMA2K_VAL_FALSE);
	}
	/* Free allocated memory */	
	if(constellation)
		free(constellation);
	if(EVMTrace)
		free(EVMTrace);	

	printf("\nPress any key to exit");
	_getch();

	return error;
}
