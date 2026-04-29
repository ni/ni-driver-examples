//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Channel Configuration Mode.
//6. Configure Physical Layer Subtype.
//7. Configure Uplink Data Modulation Type. 
//8. Configure Uplink Spreading Parameters.
//9. Select SlotPhase measurement and enable Traces. 
//10. Configure Synchronization Mode and Interval
//11. Initiate the Measurement.
//12. Fetch SlotPhase Measurements and Traces.
//13. Close RFmx Session.  

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
	float64 centerFrequency = 833.490e+6;								/* Hz */
	float64 externalAttenuation = 0.00;									/* dB */

	float64 referenceLevel = 0.00;										/* dBm */


	/* Frequency Reference */
	char *frequencyReferenceSource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10.0e+6;						/* Hz */

	/* Trigger */
	int32 enableTrigger = RFMXEVDO_VAL_FALSE;
	char* digitalEdgeSource = RFMXEVDO_VAL_PFI0_STR;
	int32 digitalEdge =  RFMXEVDO_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.0;							                 /* s */

	int32 channelConfigurationMode = RFMXEVDO_VAL_CHANNEL_CONFIGURATION_MODE_AUTO_DETECT;
	int32 physicalLayerSubtype = RFMXEVDO_VAL_PHYSICAL_LAYER_SUBTYPE_0_1;
	int32 uplinkDataModulationType = RFMXEVDO_VAL_UPLINK_DATA_MODULATION_TYPE_AUTO;
	int64 uplinkSpreadingIMask = 0;
	int64 uplinkSpreadingQMask = 0;
	int32 synchronizationMode = RFMXEVDO_VAL_SLOTPHASE_SYNCHRONIZATION_MODE_SLOT;
    int32 measurementOffset = 0 ;										 /* slots */
	int32 measurementLength = 16;										 /* slots */

	float64 timeout = 10.0;	
	int32 actualArraySize = 0;

	/* Variables to store SlotPhase Code Domain Power */	
	float64 maximumHalfSlotPhaseDiscontinuity = 0.0;					/* deg */
	float64 halfSlotPhaseDiscontinuity[2*NUMBER_OF_SLOTS];				/* deg */


	/* variables to store traces */
	float64 x0 = 0.0;
	float64 dx = 0.0;
	float32* chipPhaseError = NULL;										/* deg */
	float32* chipPhaseErrorLinearFit = NULL;							/* deg */

	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxEVDO_Initialize(resourceName, "", &instrumentHandle, NULL));

	/* Configure EVDO SlotPhase measurement parameters */
	RFmxCheckWarn(RFmxEVDO_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxEVDO_CfgRF(instrumentHandle,"",centerFrequency,referenceLevel,externalAttenuation));
	RFmxCheckWarn(RFmxEVDO_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxEVDO_CfgChannelConfigurationMode(instrumentHandle, "", channelConfigurationMode));
	RFmxCheckWarn(RFmxEVDO_CfgPhysicalLayerSubtype(instrumentHandle, "", physicalLayerSubtype));
	RFmxCheckWarn(RFmxEVDO_CfgUplinkDataModulationType(instrumentHandle, "", uplinkDataModulationType));
	RFmxCheckWarn(RFmxEVDO_CfgUplinkSpreading(instrumentHandle,"",uplinkSpreadingIMask, uplinkSpreadingQMask));
	RFmxCheckWarn(RFmxEVDO_SelectMeasurements(instrumentHandle, "",RFMXEVDO_VAL_SLOTPHASE, RFMXEVDO_VAL_TRUE));

	RFmxCheckWarn(RFmxEVDO_SlotPhaseCfgSynchronizationModeAndInterval(instrumentHandle,"", synchronizationMode, measurementOffset, measurementLength)); 
	RFmxCheckWarn(RFmxEVDO_Initiate(instrumentHandle,"",""));

	/* Fetch diverse SlotPhase Measurement Results */	

	RFmxCheckWarn(RFmxEVDO_SlotPhaseFetchMaximumHalfSlotPhaseDiscontinuity(instrumentHandle, "", timeout, &maximumHalfSlotPhaseDiscontinuity));

    RFmxCheckWarn(RFmxEVDO_SlotPhaseFetchPhaseDiscontinuities(instrumentHandle, "", timeout, halfSlotPhaseDiscontinuity, 2*NUMBER_OF_SLOTS, NULL));		

	RFmxCheckWarn(RFmxEVDO_SlotPhaseFetchChipPhaseErrorTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, 
		&actualArraySize));
	if( actualArraySize > 0 )
	{
		chipPhaseError = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if(chipPhaseError)
		{
			RFmxCheckWarn(RFmxEVDO_SlotPhaseFetchChipPhaseErrorTrace(instrumentHandle, "", timeout, &x0, &dx, chipPhaseError, actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxEVDO_SlotPhaseFetchChipPhaseErrorLinearFitTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, 
		&actualArraySize));
	if( actualArraySize > 0 )
	{
		chipPhaseErrorLinearFit = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if(chipPhaseErrorLinearFit)
		{
			RFmxCheckWarn(RFmxEVDO_SlotPhaseFetchChipPhaseErrorLinearFitTrace(instrumentHandle, "", timeout, &x0, &dx, chipPhaseErrorLinearFit, actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}


	/* Display Code Domain Power Results Results */	
	printf("\n-------------- Slot Phase Results --------------\n");

	printf("Maximum Half Slot Phase Discontinuity (deg)        : %f\n", maximumHalfSlotPhaseDiscontinuity);

	for(i = 0; i < 2*NUMBER_OF_SLOTS; i++)
	{
		printf("\nSLOT NUMBER %d\n", i);
		printf("Half Slot Phase Discontinuity (deg)                : %f\n", halfSlotPhaseDiscontinuity[i]);
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

	/* Free allocated memory */	
	if(chipPhaseError)
		free(chipPhaseError);
	if(chipPhaseErrorLinearFit)
		free(chipPhaseErrorLinearFit);
	
	printf("\nPress any key to exit");
	_getch();
	return error;
}
