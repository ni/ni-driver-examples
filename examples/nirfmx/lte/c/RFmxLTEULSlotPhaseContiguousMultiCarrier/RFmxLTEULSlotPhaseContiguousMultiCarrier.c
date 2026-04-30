// Steps:
// 1. Open a new RFmx Session.
// 2. Configure Frequency Reference.
// 3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
// 4. Configure Trigger Type and Trigger Parameters.
// 5. Configure Component Carrier Spacing.
// 6. Configure Component Carriers.
// 7. Configure Duplex Scheme.
// 8. Select SlotPower measurement and enable Traces.
// 9. Configure Measurement Method.
// 10. Initiate the Measurement.
// 11. Fetch SlotPower Measurements and Traces.
// 12. Close RFmx Session.  

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION          4096
#define NUMBER_OF_COMPONENT_CARRIERS   2
#define NUMBER_OF_SLOTS			       20

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING            256

int main (int argc, char *argv[])
{	    
	niRFmxInstrHandle instrumentHandle = NULL;
	int32 error = 0, lastErrorCode = 0;
	char errorMessage[MAX_ERROR_DESCRIPTION] = {'\0'};
	int32 i = 0;
	char subblockCarrierString[NUMBER_OF_COMPONENT_CARRIERS][MAX_SELECTOR_STRING];

	char* resourceName = "RFSA";
	float64 centerFrequency = 1.95e+9;								                                          /* Hz */
	float64 externalAttenuation = 0.00;									                                      /* dB */

	float64 referenceLevel = 0.00;										                                      /* dBm */


	/* Frequency Reference */
	char *frequencyReferenceSource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10.0e+6;						                                      /* Hz */

	/* Trigger */
	int32 enableTrigger = RFMXLTE_VAL_FALSE;
	char* digitalEdgeTriggerSource = RFMXLTE_VAL_PFI0_STR;
	int32 digitalEdgeTriggerEdge =  RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.0;							                                                      /* s */

	int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_FDD;
	int32 synchronizationMode = RFMXLTE_VAL_SLOTPHASE_SYNCHRONIZATION_MODE_SLOT;
	int32 measurementOffset = 0 ;                                                                             /* slots */
	int32 measurementLength = NUMBER_OF_SLOTS;                                                                /* slots */

	/* Spacing Settings */
	int32 componentCarrierSpacingType = RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL;
	int32 componentCarrierAtCenterFrequency = -1;

	/* Component Carrier Settings */
	float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS] = {20e6, 20e6};                           /*(Hz) */
	float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS] = {-9.9e6, 9.9e6};                        /*(Hz) */
	int32 cellID[NUMBER_OF_COMPONENT_CARRIERS] = {0, 1};
	
	int32 uplinkDownlinkConfiguraiton = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;
	float64 timeout = 10.0;																				      /*(s) */ 

	/* Variables to store SlotPhase Phase Discontinuities */	
	float64 maximumPhaseDiscontinuity[NUMBER_OF_COMPONENT_CARRIERS] = {0.0};
	float64 *slotPhaseDiscontinuity[NUMBER_OF_COMPONENT_CARRIERS] = {NULL};
	int32 actualArraySize = 0;


	/* variables to store traces */
	float64 x0[NUMBER_OF_COMPONENT_CARRIERS] = {0.0}, dx[NUMBER_OF_COMPONENT_CARRIERS] = {0.0};
	float32* samplePhaseError[NUMBER_OF_COMPONENT_CARRIERS] = {NULL};										 /* deg */
	float32* samplePhaseErrorLinearFit[NUMBER_OF_COMPONENT_CARRIERS] = {NULL};							     /* deg */

	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));

	/* Configure LTE SlotPhase measurement parameters */
	RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle,"",centerFrequency,referenceLevel,externalAttenuation));
	RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, "", componentCarrierSpacingType, 
		componentCarrierAtCenterFrequency));
	RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, "", NUMBER_OF_COMPONENT_CARRIERS));	
	RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, "", componentCarrierBandwidth, 
		componentCarrierFrequency, cellID, NUMBER_OF_COMPONENT_CARRIERS));  
	RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));
	RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "",RFMXLTE_VAL_SLOTPHASE, RFMXLTE_VAL_TRUE));
	RFmxCheckWarn(RFmxLTE_SlotPhaseCfgSynchronizationModeAndInterval(instrumentHandle,"", synchronizationMode, measurementOffset, measurementLength)); 
	RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle,"",""));

	/* Fetch diverse SlotPhase Measurement Results */	

	RFmxCheckWarn(RFmxLTE_SlotPhaseFetchMaximumPhaseDiscontinuityArray(instrumentHandle, "", timeout,
		          maximumPhaseDiscontinuity, NUMBER_OF_COMPONENT_CARRIERS, NULL));

   for( i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++ )
	{
		actualArraySize = 0;
		RFmxLTE_BuildCarrierString("", i, MAX_SELECTOR_STRING, subblockCarrierString[i]);
		RFmxCheckWarn(RFmxLTE_SlotPhaseFetchPhaseDiscontinuities(instrumentHandle, subblockCarrierString[i], timeout, 
			      NULL, 0, &actualArraySize));		
		if( actualArraySize > 0 )
		{	
			slotPhaseDiscontinuity[i] = (float64 *) malloc(sizeof(float64) * actualArraySize);
			if(slotPhaseDiscontinuity[i])
			{
				RFmxCheckWarn(RFmxLTE_SlotPhaseFetchPhaseDiscontinuities(instrumentHandle, subblockCarrierString[i], 
					timeout, slotPhaseDiscontinuity[i], actualArraySize, NULL));		
			}
			else
			{
				printf("malloc failed.\n");
				goto Error;
			}
		}
	}

   
   for( i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++ )
	{
	   actualArraySize = 0;
	   RFmxLTE_BuildCarrierString("", i, MAX_SELECTOR_STRING, subblockCarrierString[i]);
	   RFmxCheckWarn(RFmxLTE_SlotPhaseFetchSamplePhaseError(instrumentHandle, subblockCarrierString[i], timeout, NULL, NULL, NULL, 0, 
		           &actualArraySize));		
	  if( actualArraySize > 0 )
		{	
			samplePhaseError[i] = (float32 *)malloc(sizeof(float32) * actualArraySize);
		    if(samplePhaseError[i])
		    {
			 RFmxCheckWarn(RFmxLTE_SlotPhaseFetchSamplePhaseError(instrumentHandle, subblockCarrierString[i], timeout, &x0[i], &dx[i],
				   samplePhaseError[i], actualArraySize, NULL));		
		    }
		    else
		    {
			 printf("malloc failed.\n");
			 goto Error;
		    }
		}
	}

     for( i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++ )
	{
		actualArraySize = 0;
		RFmxLTE_BuildCarrierString("", i, MAX_SELECTOR_STRING, subblockCarrierString[i]);
		RFmxCheckWarn(RFmxLTE_SlotPhaseFetchSamplePhaseErrorLinearFitTrace(instrumentHandle, subblockCarrierString[i], timeout, NULL, NULL,
			      NULL, 0, &actualArraySize));		
		if( actualArraySize > 0 )
		{	
			samplePhaseErrorLinearFit[i] = (float32 *) malloc(sizeof(float32) * actualArraySize);
			if(samplePhaseErrorLinearFit[i])
			{
				RFmxCheckWarn(RFmxLTE_SlotPhaseFetchSamplePhaseErrorLinearFitTrace(instrumentHandle, subblockCarrierString[i], timeout, &x0[i], &dx[i],
					 samplePhaseErrorLinearFit[i], actualArraySize, NULL));		
			}
			else
			{
				printf("malloc failed.\n");
				goto Error;
			}
		}
	}
   


	/* Display Slot Phase Results */
	for( i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++ )
	 {
	   printf("\nCarrier %d\n", i);
	   printf("Maximum  Phase Discontinuity (deg)                : %f\n", maximumPhaseDiscontinuity[i]);
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
	 for( i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++ )
	 {
	   if(slotPhaseDiscontinuity[i])
		 free(slotPhaseDiscontinuity[i]);
	   if(samplePhaseError[i])
		 free(samplePhaseError[i]);
	   if(samplePhaseErrorLinearFit[i])
		 free(samplePhaseErrorLinearFit[i]);
	 }
	
	printf("\nPress any key to exit");
	_getch();
	return error;
}
