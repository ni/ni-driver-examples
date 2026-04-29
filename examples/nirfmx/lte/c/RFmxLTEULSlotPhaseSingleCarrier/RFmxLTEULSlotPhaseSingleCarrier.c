//Steps:
//1.Open a new RFmx Session.
//2.Configure Frequency Reference.
//3.Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4.Configure Trigger Type and Trigger Parameters.
//5.Configure Carrier Bandwidth.
//6.Configure Duplex Scheme.
//7.Select SlotPhase measurement and enable Traces.
//8.Configure Synchronization Mode and Interval
//9. Initiate the Measurement.
//10. Fetch SlotPhase  Traces and Measurements.
//11. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096
#define NUMBER_OF_SLOTS			    20

int main (int argc, char *argv[])
{	    
	niRFmxInstrHandle instrumentHandle = NULL;
	int32 error = 0, lastErrorCode = 0;
	char errorMessage[MAX_ERROR_DESCRIPTION] = {'\0'};
	int32 i = 0;

	char* resourceName = "RFSA";
	float64 centerFrequency = 1.95e+9;								    /* Hz */
	float64 externalAttenuation = 0.00;									/* dB */

	float64 referenceLevel = 0.00;										/* dBm */


	/* Frequency Reference */
	char *frequencyReferenceSource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10.0e+6;						/* Hz */

	/* Trigger */
	int32 enableTrigger = RFMXLTE_VAL_FALSE;
	char* digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
	int32 digitalEdge =  RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.0;							                 /* s */

	int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_FDD;
	int32 uplinkDownlinkConfiguration = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;
	float64 componentCarrierBandwidth = 10e+6;                           /* Hz */
	float64 componentCarrierFrequency = 0.0;                             /* Hz */
	int32 cellID = 0;


	int32 synchronizationMode = RFMXLTE_VAL_SLOTPHASE_SYNCHRONIZATION_MODE_SLOT;
    int32 measurementOffset = 0 ;										 /* slots */
	int32 measurementLength = NUMBER_OF_SLOTS;							 /* slots */

	float64 timeout = 10.0;	
	int32 actualArraySize = 0;

	/* Variables to store SlotPhase Phase Discontinuities */	
	float64 maximumPhaseDiscontinuity = 0.0;					         /* deg */
	float64 phaseDiscontinuity[NUMBER_OF_SLOTS];				         /* deg */


	/* variables to store traces */
	float64 x0 = 0.0;
	float64 dx = 0.0;
	float32* samplePhaseError = NULL;									/* deg */
	float32* samplePhaseErrorLinearFit = NULL;							/* deg */

	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));

	/* Configure LTE SlotPhase measurement parameters */
	RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle,"",centerFrequency,referenceLevel,externalAttenuation));
	RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxLTE_CfgComponentCarrier(instrumentHandle, "", componentCarrierBandwidth, componentCarrierFrequency, cellID));
    RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguration));
	RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "",RFMXLTE_VAL_SLOTPHASE, RFMXLTE_VAL_TRUE));
	RFmxCheckWarn(RFmxLTE_SlotPhaseCfgSynchronizationModeAndInterval(instrumentHandle,"", synchronizationMode, measurementOffset, measurementLength)); 
	RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle,"",""));

	/* Fetch diverse SlotPhase Measurement Results */	

	RFmxCheckWarn(RFmxLTE_SlotPhaseFetchPhaseDiscontinuities(instrumentHandle, "", timeout, phaseDiscontinuity, NUMBER_OF_SLOTS, NULL));

    RFmxCheckWarn(RFmxLTE_SlotPhaseFetchMaximumPhaseDiscontinuity(instrumentHandle, "", timeout, &maximumPhaseDiscontinuity));		




	RFmxCheckWarn(RFmxLTE_SlotPhaseFetchSamplePhaseError(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, 
		&actualArraySize));
	if( actualArraySize > 0 )
	{
		samplePhaseError = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if(samplePhaseError)
		{
			RFmxCheckWarn(RFmxLTE_SlotPhaseFetchSamplePhaseError(instrumentHandle, "", timeout, &x0, &dx, samplePhaseError, actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxLTE_SlotPhaseFetchSamplePhaseErrorLinearFitTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, 
		&actualArraySize));
	if( actualArraySize > 0 )
	{
		samplePhaseErrorLinearFit = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if(samplePhaseErrorLinearFit)
		{
			RFmxCheckWarn(RFmxLTE_SlotPhaseFetchSamplePhaseErrorLinearFitTrace(instrumentHandle, "", timeout, &x0, &dx, samplePhaseErrorLinearFit, actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}


	/* Display Slot Phase Results */	

	printf("Maximum  Phase Discontinuity (deg)                : %f\n", maximumPhaseDiscontinuity);

	for(i = 0; i < NUMBER_OF_SLOTS; i++)
	{
		printf("\nSLOT NUMBER %d\n", i);
		printf("Slot Phase Discontinuities (deg)                : %f\n", phaseDiscontinuity[i]);
	}
	
Error:
	if( error ) 
	{
		RFmxLTE_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if(error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}
	if(instrumentHandle)
	{
		RFmxLTE_Close(instrumentHandle, RFMXLTE_VAL_FALSE);
	}

	/* Free allocated memory */	
	if(samplePhaseError)
		free(samplePhaseError);
	if(samplePhaseErrorLinearFit)
		free(samplePhaseErrorLinearFit);
	
	printf("\nPress any key to exit");
	_getch();
	return error;
}
