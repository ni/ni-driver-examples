//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select SlotPhase measurement and enable Traces.
//6. Configure Radio Configuration and Uplink Spreading Long code mask.
//7. Configure Synchronization Mode and Interval
//8. Initiate the Measurement.
//9. Fetch SlotPhase Measurements and Traces.
//10. Close RFmx Session. 

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
	int32 synchronizationMode = RFMXCDMA2K_VAL_SLOTPHASE_SYNCHRONIZATION_MODE_SLOT;
    int32 measurementOffset = 0 ;                      /* slots */
	int32 measurementLength = NUMBER_OF_SLOTS;         /* slots */

	float64 timeout = 10.0;	
	int32 actualArraySize = 0;

	/* Variables to store SlotPhase Code Domain Power */	
	float64 maximumPhaseDiscontinuity = 0.0;           /* deg */

	/* variables to store traces */
	float64 x0 = 0.0;
	float64 dx = 0.0;
	float64 y0 = 0.0;
	float64 dy = 0.0;
	float64 slotPhaseDiscontinuity[NUMBER_OF_SLOTS];   /* deg */
	float32* chipPhaseError = NULL;                    /* deg */
	float32* chipPhaseErrorLinearFit = NULL;           /* deg */

	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxCDMA2k_Initialize(resourceName, "", &instrumentHandle, NULL));

	/* Configure CDMA2k SlotPhase measurement parameters */
	RFmxCheckWarn(RFmxCDMA2k_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxCDMA2k_CfgRF(instrumentHandle,"",centerFrequency,referenceLevel,externalAttenuation));
	RFmxCheckWarn(RFmxCDMA2k_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxCDMA2k_CfgRadioConfiguration(instrumentHandle,"",radioConfiguration));
	RFmxCheckWarn(RFmxCDMA2k_CfgUplinkSpreading(instrumentHandle,"",uplinkSpreadingLongCodeMask));
	
	RFmxCheckWarn(RFmxCDMA2k_SelectMeasurements(instrumentHandle, "",RFMXCDMA2K_VAL_SLOTPHASE, RFMXCDMA2K_VAL_TRUE));

	RFmxCheckWarn(RFmxCDMA2k_SlotPhaseCfgSynchronizationModeAndInterval(instrumentHandle,"", synchronizationMode, measurementOffset, measurementLength)); 
	RFmxCheckWarn(RFmxCDMA2k_Initiate(instrumentHandle,"",""));
	
	/* Fetch SlotPhase Measurement Results */	
	
	RFmxCheckWarn(RFmxCDMA2k_SlotPhaseFetchMaximumPhaseDiscontinuity(instrumentHandle, "", timeout, &maximumPhaseDiscontinuity));

    RFmxCheckWarn(RFmxCDMA2k_SlotPhaseFetchPhaseDiscontinuities(instrumentHandle, "", timeout, slotPhaseDiscontinuity, NUMBER_OF_SLOTS, NULL));		


	RFmxCheckWarn(RFmxCDMA2k_SlotPhaseFetchChipPhaseErrorTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, 
		&actualArraySize));
	if( actualArraySize > 0 )
	{
		chipPhaseError = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if(chipPhaseError)
		{
			RFmxCheckWarn(RFmxCDMA2k_SlotPhaseFetchChipPhaseErrorTrace(instrumentHandle, "", timeout, &x0, &dx, chipPhaseError, actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxCDMA2k_SlotPhaseFetchChipPhaseErrorLinearFitTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, 
		&actualArraySize));
	if( actualArraySize > 0 )
	{
		chipPhaseErrorLinearFit = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if(chipPhaseErrorLinearFit)
		{
			RFmxCheckWarn(RFmxCDMA2k_SlotPhaseFetchChipPhaseErrorLinearFitTrace(instrumentHandle, "", timeout, &y0, &dy, chipPhaseErrorLinearFit, actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}


	/* Display SlotPhase Measurement Results */	
	printf("\n-------------- Slot Phase Results --------------\n");

	printf("Maximum Phase Discontinuity (deg)             : %f\n", maximumPhaseDiscontinuity);

	for(i = 0; i < NUMBER_OF_SLOTS; i++)
	{
		printf("\nSLOT NUMBER %d\n", i);
		printf("Slot Phase Discontinuity (deg)                : %f\n", slotPhaseDiscontinuity[i]);
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

	/* Free allocated memory */	
	if(chipPhaseError)
		free(chipPhaseError);
	if(chipPhaseErrorLinearFit)
		free(chipPhaseErrorLinearFit);
	
	printf("\nPress any key to exit");
	_getch();
	return error;
}
