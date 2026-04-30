//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Uplink Spreading Parameters.
//6. Select SlotPower measurement.
//7. Configure Synchronization Mode and Interval
//8. Initiate the Measurement.
//9. Fetch SlotPower Measurement.
//10 Close the RFmx Seesion


#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxEVDO.h"

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
	float64 frequencyReferenceFrequency = 10.0e+6;						/* Hz */

	/* Trigger */
	int32 enableTrigger = RFMXEVDO_VAL_FALSE;
	char* digitalEdgeSource = RFMXEVDO_VAL_PFI0_STR;
	int32 digitalEdge =  RFMXEVDO_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.0;							/* s */

	int64 uplinkSpreadingIMask = 0;
	int64 uplinkSpreadingQMask = 0;
	int32 synchronizationMode = RFMXEVDO_VAL_SLOTPOWER_SYNCHRONIZATION_MODE_SLOT;
    int32 measurementOffset = 0 ;						/* slots */
	int32 measurementLength = NUMBER_OF_SLOTS;			/* slots */

	float64 timeout = 10.0;								/* s */


	/* variables to store traces */
	float64 halfSlotPower[2*NUMBER_OF_SLOTS];			/* dBm */
	float64 halfSlotPowerDelta[2*NUMBER_OF_SLOTS];		/* dB */

	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxEVDO_Initialize(resourceName, "", &instrumentHandle, NULL));

	/* Configure EVDO SlotPower measurement parameters */
	RFmxCheckWarn(RFmxEVDO_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxEVDO_CfgRF(instrumentHandle,"",centerFrequency,referenceLevel,externalAttenuation));
	RFmxCheckWarn(RFmxEVDO_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxEVDO_CfgUplinkSpreading(instrumentHandle,"",uplinkSpreadingIMask, uplinkSpreadingQMask));

	RFmxCheckWarn(RFmxEVDO_SelectMeasurements(instrumentHandle, "", RFMXEVDO_VAL_SLOTPOWER, RFMXEVDO_VAL_TRUE));

	RFmxCheckWarn(RFmxEVDO_SlotPowerCfgSynchronizationModeAndInterval(instrumentHandle,"", synchronizationMode, measurementOffset, measurementLength)); 
	RFmxCheckWarn(RFmxEVDO_Initiate(instrumentHandle,"",""));

	/* Fetch diverse SlotPower Measurement Results */	


    RFmxCheckWarn(RFmxEVDO_SlotPowerFetchPowers(instrumentHandle, "", timeout, halfSlotPower, halfSlotPowerDelta, 2*NUMBER_OF_SLOTS, NULL));		




	/* Display Half Slot Power  Results */	
	printf("\n-------------- Half Slot Powers --------------\n");

	for(i = 0; i < 2 * NUMBER_OF_SLOTS; i++)
	{
		printf("\nHalf Slot Number %d\n", i);
		printf("Half Slot Power (dBm)                : %f\n", halfSlotPower[i]);
		printf("Half Slot Power Delta (dB)           : %f\n", halfSlotPowerDelta[i]);
	}
	
Error:
	if( error ) 
	{
		RFmxEVDO_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if(error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}
	if(instrumentHandle)
	{
		RFmxEVDO_Close(instrumentHandle, RFMXEVDO_VAL_FALSE);
	}

	
	printf("\nPress any key to exit");
	_getch();
	return error;
}
