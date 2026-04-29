//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure WCDMA centre frequency.
//4. Configure Trigger Type and Trigger Parameters.
//5. Select SlotPhase measurement and enable Traces.
//6. Configure Uplink Scrambling.
//7. Configure Synchronisation Mode and Interval
//8. Initiate the Measurement.
//9. Fetch SlotPhase Measurements and Traces.
//10. Close RFmx Session. 

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
   
	float64 centerFrequency = 1.95e9;	                      /*(Hz) */
    float64 referenceLevel = 0.000000;				          /*(dBm) */
    float64 externalAttenuation = 0.000000;			          /*(dB) */

    char * frequencyReferenceSource = RFMXWCDMA_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;	              /*(Hz) */
    
    int32 enableTrigger = RFMXWCDMA_VAL_FALSE;
    char * digitalEdgeSource = RFMXWCDMA_VAL_PFI0_STR;
    int32 digitalEdge = RFMXWCDMA_VAL_DIGITAL_EDGE_RISING_EDGE;
    float64 triggerDelay = 0.000000;				          /*(s) */

    int32 synchronizationMode = RFMXWCDMA_VAL_SLOTPHASE_SYNCHRONIZATION_MODE_SLOT;
	int32 measurementOffset = 0;                              /*(slots) */
	int32 measurementLength = 15;                             /*(slots) */
    
    int32 uplinkScramblingType = RFMXWCDMA_VAL_UPLINK_SCRAMBLING_TYPE_LONG;
    int32 uplinkScramblingCode = 0;
    	
	float64 timeout = 10.000000;					          /*(s) */
	int32 actualArraySize = 0;
	float64 x0 = 0.0, dx = 0.0;

    int32 discontinuityMinimumDistance = 0;                   /*slots*/   
	int32 discontinuityCountGreaterThanlimit1 = 0;
	int32 discontinuityCountGreaterThanlimit2 = 0;    
	float64 maximumPhaseDiscontinuity = 0.0;                  /*deg*/

	float64* slotPhaseDiscontinuity = NULL ;				  /*(deg) */
	float32* chipPhaseError = NULL ;						  /*(deg) */
	float32* chipPhaseErrorLinearFit = NULL ;				  /*(deg) */

	int32 i;

    /* Initialize a session */
    RFmxCheckWarn(RFmxWCDMA_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxWCDMA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxWCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxWCDMA_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
    RFmxCheckWarn(RFmxWCDMA_SelectMeasurements(instrumentHandle, "", RFMXWCDMA_VAL_SLOTPHASE, RFMXWCDMA_VAL_TRUE));
    
	RFmxCheckWarn(RFmxWCDMA_CfgUplinkScrambling(instrumentHandle, "", uplinkScramblingCode, uplinkScramblingType));
	RFmxCheckWarn(RFmxWCDMA_SlotPhaseCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode,measurementOffset,measurementLength));
    RFmxCheckWarn(RFmxWCDMA_Initiate(instrumentHandle, "", ""));

    RFmxCheckWarn(RFmxWCDMA_SlotPhaseFetchMeasurement(instrumentHandle, "", timeout,&maximumPhaseDiscontinuity ,&discontinuityCountGreaterThanlimit1 ,
		&discontinuityCountGreaterThanlimit2 ,&discontinuityMinimumDistance));
	
	RFmxCheckWarn(RFmxWCDMA_SlotPhaseFetchPhaseDiscontinuities(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		slotPhaseDiscontinuity = (float64 *) malloc(sizeof(float64) * actualArraySize);
		if( slotPhaseDiscontinuity )
		{
			RFmxCheckWarn(RFmxWCDMA_SlotPhaseFetchPhaseDiscontinuities(instrumentHandle, "", timeout, slotPhaseDiscontinuity, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxWCDMA_SlotPhaseFetchChipPhaseErrorLinearFitTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		chipPhaseErrorLinearFit = (float32*) malloc(sizeof(float32) * actualArraySize);
		if( chipPhaseErrorLinearFit )
		{
			RFmxCheckWarn(RFmxWCDMA_SlotPhaseFetchChipPhaseErrorLinearFitTrace(instrumentHandle, "", timeout, &x0, &dx, chipPhaseErrorLinearFit, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxWCDMA_SlotPhaseFetchChipPhaseErrorTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		chipPhaseError = (float32*) malloc(sizeof(float32) * actualArraySize);
		if( chipPhaseError )
		{
			RFmxCheckWarn(RFmxWCDMA_SlotPhaseFetchChipPhaseErrorTrace(instrumentHandle, "", timeout, &x0, &dx, chipPhaseError, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}
    
	printf("------------Measurement------------\n");
	printf("Maximum Phase Discontinuity (deg)       : %lf\n",maximumPhaseDiscontinuity);
	printf("Discontinuity Count > Limit1            : %d\n",discontinuityCountGreaterThanlimit1);
	printf("Discontinuity Count > Limit1            : %d\n",discontinuityCountGreaterThanlimit2);
	printf("Discontinuity Minimum Distance (slots)	: %d\n",discontinuityMinimumDistance);

	printf("Slot Phase Discontinuity (deg)          : \n");
   for(i = 0; i < measurementLength ; i++)
		printf("%d : %lf \n", i, slotPhaseDiscontinuity[i]);
		
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
    if (slotPhaseDiscontinuity)
    {
        free(slotPhaseDiscontinuity);
    }
    if (chipPhaseError)
    {
        free(chipPhaseError);
    }
    if (chipPhaseErrorLinearFit)
    {
        free(chipPhaseErrorLinearFit);
    }

    return error;
}
