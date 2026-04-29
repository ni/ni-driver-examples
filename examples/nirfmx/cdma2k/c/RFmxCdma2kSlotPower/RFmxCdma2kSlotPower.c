//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Radio Configuration and Uplink Spreading Long Code Mask.
//6. Select SlotPower measurement.
//7. Configure Synchronization Mode and Interval
//8. Initiate the Measurement.
//9. Fetch SlotPower Measurement.
//10 Close the RFmx Seesion


#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxCDMA2k.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096
#define NUMBER_OF_SLOTS			16

int main (int argc, char *argv[])
{	    
	niRFmxInstrHandle instrumentHandle = NULL;
	int32 error = 0, lastErrorCode = 0;
	char errorMessage[MAX_ERROR_DESCRIPTION] = {'\0'};
	int32 i = 0;

	char* resourceName = "RFSA";
	float64 centerFrequency = 833.490e+6;				/* Hz */
	float64 externalAttenuation = 0.00;					/* dB */
	float64 referenceLevel = 0.00;						/* dBm */

	/* Frequency Reference */
	char *frequencyReferenceSource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10.0e+6;		/* Hz */

	/* Trigger */
	int32 enableTrigger = RFMXCDMA2K_VAL_FALSE;
	char* digitalEdgeTriggerSource = RFMXCDMA2K_VAL_PFI0_STR;
	int32 digitalEdgeTriggerEdge =  RFMXCDMA2K_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.0;							/* s */

	int32 radioConfiguration = RFMXCDMA2K_VAL_RADIO_CONFIGURATION_RC3;
	int64 uplinkSpreadingLongCodeMask = 0;
	int32 synchronizationMode = RFMXCDMA2K_VAL_SLOTPOWER_SYNCHRONIZATION_MODE_SLOT;
    int32 measurementOffset = 0 ;
	int32 measurementLength = NUMBER_OF_SLOTS;

	float64 timeout = 10.0;	

	/* variables to store traces */
	float64 slotPower[NUMBER_OF_SLOTS];                 /* dBm */
	float64 slotPowerDelta[NUMBER_OF_SLOTS];            /* dB */

	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxCDMA2k_Initialize(resourceName, "", &instrumentHandle, NULL));

	/* Configure CDMA2k SlotPower measurement parameters */
	RFmxCheckWarn(RFmxCDMA2k_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxCDMA2k_CfgRF(instrumentHandle,"",centerFrequency,referenceLevel,externalAttenuation));
	RFmxCheckWarn(RFmxCDMA2k_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, 
		                                           triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxCDMA2k_CfgRadioConfiguration(instrumentHandle,"",radioConfiguration));
	RFmxCheckWarn(RFmxCDMA2k_CfgUplinkSpreading(instrumentHandle,"",uplinkSpreadingLongCodeMask));

	RFmxCheckWarn(RFmxCDMA2k_SelectMeasurements(instrumentHandle, "", RFMXCDMA2K_VAL_SLOTPOWER, RFMXCDMA2K_VAL_TRUE));
	RFmxCheckWarn(RFmxCDMA2k_SlotPowerCfgSynchronizationModeAndInterval(instrumentHandle,"", synchronizationMode, measurementOffset,
		                                                                measurementLength)); 
	RFmxCheckWarn(RFmxCDMA2k_Initiate(instrumentHandle,"",""));

	/* Fetch SlotPower Measurement Results */	
    RFmxCheckWarn(RFmxCDMA2k_SlotPowerFetchPowers(instrumentHandle, "", timeout, slotPower, slotPowerDelta, NUMBER_OF_SLOTS, NULL));		

	/* Display Slot Power Results */	
	printf("\n-------------- Slot Powers --------------\n");

	for(i = 0; i < NUMBER_OF_SLOTS; i++)
	{
		printf("\nSLOT NUMBER %d\n", i);
		printf("Slot Power (dBm)                : %f\n", slotPower[i]);
		printf("Slot Power Delta (dB)           : %f\n", slotPowerDelta[i]);
	}
	
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
	
	printf("\nPress any key to exit");
	_getch();
	return error;
}
