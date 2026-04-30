//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Uplink Scrambling.
//6. Select SlotPower measurement and enable Traces.
//7. Configure Synchronization Mode and Interval
//8. Initiate the Measurement.
//9. Fetch SlotPower Measurement.
//10. Close the RFmx session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxWCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096
#define NUMBER_OF_SLOTS			15

int main (int argc, char *argv[])
{	    
	niRFmxInstrHandle instrumentHandle = NULL;
	int32 error = 0, lastErrorCode = 0;
	char errorMessage[MAX_ERROR_DESCRIPTION] = {'\0'};
	int32 i = 0;

	char* resourceName = "RFSA";
	float64 centerFrequency = 1.95e+9;				                    /* Hz */
	float64 externalAttenuation = 0.00;					                /* dB */
	float64 referenceLevel = 0.00;						                /* dBm */

	/* Frequency Reference */
	char *frequencyReferenceSource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10.0e+6;						/* Hz */

	/* Trigger */
	int32 enableTrigger = RFMXWCDMA_VAL_FALSE;
	char* digitalEdgeTriggerSource = RFMXWCDMA_VAL_PFI0_STR;
	int32 digitalEdgeTriggerEdge =  RFMXWCDMA_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.0;							                 /* s */

	int32 uplinkScramblingType = RFMXWCDMA_VAL_UPLINK_SCRAMBLING_TYPE_LONG;
	int32 uplinkScramblingCode = 0;
	int32 synchronizationMode = RFMXWCDMA_VAL_SLOTPOWER_SYNCHRONIZATION_MODE_SLOT;
    int32 measurementOffset = 0 ;                                       /* slots */
	int32 measurementLength = NUMBER_OF_SLOTS;                          /* slots */

	float64 timeout = 10.0;	


	/* variables to store traces */
	float64 slotPower[NUMBER_OF_SLOTS];                                 /* dBm */
	float64 slotPowerDelta[NUMBER_OF_SLOTS];                            /* dB  */

	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxWCDMA_Initialize(resourceName, "", &instrumentHandle, NULL));

	/* Configure WCDMA SlotPower measurement parameters */
	RFmxCheckWarn(RFmxWCDMA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxWCDMA_CfgRF(instrumentHandle,"",centerFrequency,referenceLevel,externalAttenuation));
	RFmxCheckWarn(RFmxWCDMA_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxWCDMA_CfgUplinkScrambling(instrumentHandle, "", uplinkScramblingCode, uplinkScramblingType));

	RFmxCheckWarn(RFmxWCDMA_SelectMeasurements(instrumentHandle, "", RFMXWCDMA_VAL_SLOTPOWER, RFMXWCDMA_VAL_TRUE));

	RFmxCheckWarn(RFmxWCDMA_SlotPowerCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode, measurementOffset, measurementLength)); 
	RFmxCheckWarn(RFmxWCDMA_Initiate(instrumentHandle,"",""));

	/* Fetch diverse SlotPower Measurement Results */	


    RFmxCheckWarn(RFmxWCDMA_SlotPowerFetchPowers(instrumentHandle, "", timeout, slotPower, slotPowerDelta, NUMBER_OF_SLOTS, NULL));		




	/* Display Slot Power Results */	
	printf("\n-------------- Slot Powers  --------------\n");

	for(i = 0; i < NUMBER_OF_SLOTS; i++)
	{
		printf("\nSlot Number %d\n", i);
		printf("Slot Power (dBm)                     : %f\n", slotPower[i]);
		printf("Slot Power Delta (dB)                : %f\n", slotPowerDelta[i]);
	}
	
Error:
	if( error ) 
	{
		RFmxWCDMA_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if(error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}
	if(instrumentHandle)
	{
		RFmxWCDMA_Close(instrumentHandle, RFMXWCDMA_VAL_FALSE);
	}

	printf("\nPress any key to exit");
	_getch();
	return error;
}
