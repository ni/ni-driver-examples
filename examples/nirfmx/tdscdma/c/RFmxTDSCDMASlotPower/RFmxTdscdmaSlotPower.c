//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select SlotPower measurement and enable Traces.
//6. Configure Measurement Length.
//7. Initiate the Measurement.
//8. Fetch SlotPower Measurement.
//9. Close the RFmx session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxTDSCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096
#define NUMBER_OF_SLOTS			28

int main (int argc, char *argv[])
{	    
	niRFmxInstrHandle instrumentHandle = NULL;
	int32 error = 0, lastErrorCode = 0;
	char errorMessage[MAX_ERROR_DESCRIPTION] = {'\0'};
	int32 i = 0;

	char* resourceName = "RFSA";
	float64 centerFrequency = 1.91e+9;				    /* Hz */
	float64 externalAttenuation = 0.00;					/* dB */
	float64 referenceLevel = 0.00;						/* dBm */

	/* Frequency Reference */
	char *frequencyReferenceSource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10.0e+6;		/* Hz */
		
	/*Trigger */
	float64 triggerDelay = 0.00;						/*(s) */
	int32 enableTrigger = RFMXTDSCDMA_VAL_TRUE;
	char * IQPowerEdgeSource = "0";
	int32 IQPowerEdgeSlope = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_RISING_SLOPE;
	float64 IQPowerEdgeLevel = -20.00;					/*(dB) */
	int32 minimumQuietTimeMode = RFMXTDSCDMA_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
	float64 minimumQuietTime = 16E-6;					/*(s) */
	int32 IQPowerEdgeLevelType = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;

	int32 measurementLength = NUMBER_OF_SLOTS;
	float64 timeout = 10.0;	
	int32 actualArraySize = 0;

	/* Variables to store traces */
	float64* slotPower = NULL;                          /* dBm */
	float64* slotPowerDelta = NULL;                     /* dB */

	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxTDSCDMA_Initialize(resourceName, "", &instrumentHandle, NULL));

	/* Configure TDSCDMA SlotPower measurement parameters */
	RFmxCheckWarn(RFmxTDSCDMA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxTDSCDMA_CfgRF(instrumentHandle,"",centerFrequency,referenceLevel,externalAttenuation));
	RFmxCheckWarn(RFmxTDSCDMA_CfgIQPowerEdgeTrigger(instrumentHandle, "", IQPowerEdgeSource, IQPowerEdgeSlope, IQPowerEdgeLevel,
									triggerDelay, minimumQuietTimeMode, minimumQuietTime, IQPowerEdgeLevelType, enableTrigger));

	RFmxCheckWarn(RFmxTDSCDMA_SelectMeasurements(instrumentHandle, "", RFMXTDSCDMA_VAL_SLOTPOWER, RFMXTDSCDMA_VAL_TRUE));
	RFmxCheckWarn(RFmxTDSCDMA_SlotPowerCfgMeasurementLength(instrumentHandle, "", measurementLength)); 
	RFmxCheckWarn(RFmxTDSCDMA_Initiate(instrumentHandle,"",""));

	/* Fetch SlotPower Measurement Results */	
	RFmxCheckWarn(RFmxTDSCDMA_SlotPowerFetchPowers(instrumentHandle, "", timeout, NULL, NULL, 0, 
		&actualArraySize));
	if( actualArraySize > 0 )
	{
		slotPower = (float64 *)malloc(sizeof(float64) * actualArraySize);
		slotPowerDelta = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if(slotPower && slotPowerDelta)
		{
			RFmxCheckWarn(RFmxTDSCDMA_SlotPowerFetchPowers(instrumentHandle, "", timeout, slotPower, slotPowerDelta, actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	/* Display Slot Power Results */	
	printf("\n-------------- Slot Powers --------------\n");
	for(i = 0; i < actualArraySize; i++)
	{
		printf("\nSlot Number %d\n", i);
		printf("Slot Power (dBm)                : %f\n", slotPower[i]);
		printf("Slot Power Delta (dB)           : %f\n", slotPowerDelta[i]);
	}
	
Error:
	if( error ) 
	{
		RFmxTDSCDMA_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if(error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}
	if(instrumentHandle)
	{
		RFmxTDSCDMA_Close(instrumentHandle, RFMXTDSCDMA_VAL_FALSE);
	}

	/* Free allocated memory */	
	if(slotPower)
		free(slotPower);
	if(slotPowerDelta)
		free(slotPowerDelta);
	
	printf("\nPress any key to exit");
	_getch();
	return error;
}
