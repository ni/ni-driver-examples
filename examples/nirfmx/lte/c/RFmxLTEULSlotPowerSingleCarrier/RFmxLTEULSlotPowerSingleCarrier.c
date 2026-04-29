//Steps:
//1.Open a new RFmx Session.
//2.Configure Frequency Reference.
//3.Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4.Configure Trigger Type and Trigger Parameters.
//5.Configure Carrier Bandwidth.
//6.Configure Duplex Scheme.
//7.Select SlotPower measurement and enable Traces.
//8.Configure Measurement Intervals.
//9.Initiate the Measurement.
//10.Fetch SlotPower Traces.
//13. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                     256

int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;
	float64 centerFrequency = 1.95e9;                                                        /*(Hz) */
	float64 externalAttenuation = 0.0;                                                       /*dB*/
	float64 referenceLevel = 0.0;                                                            /*dBm*/

	char subblockString[MAX_SELECTOR_STRING];
	char subblockCarrierString[MAX_SELECTOR_STRING];

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0, lastErrorCode = 0;
	  
	int32 measurementOffset = 0;                                                             /*Subframes*/
	int32 measurementLength = 10;                                                            /*Subframes*/

	char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;                                              /*(Hz) */
	int32 uplinkDownlinkConfiguraiton =0;

	int32 enableTrigger = RFMXLTE_VAL_FALSE;    
	char * digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
	int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.0;                                                              /*(s) */

	int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_FDD;
	float64 componentCarrierBandwidth = 10e+6;                                               /* Hz */
	float64 componentCarrierFrequency = 0.0;                                                 /* Hz */
	int32 cellID = 0;

	float64 timeout = 10.0;                                                                  /*(s) */

	int32 actualArraySize = 0;
	float64 *subFramePower = NULL;                                                           /*(dBm) */
	float64 *subFramePowerDelta = NULL;                                                      /*(dBm) */

	int32 i=0;

	/* Initialize a session */
	RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, 
		frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency,referenceLevel,externalAttenuation));
		
	RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge,
		triggerDelay, enableTrigger));

	RFmxCheckWarn(RFmxLTE_BuildSubblockString("", 0, MAX_SELECTOR_STRING, subblockString));

	RFmxCheckWarn(RFmxLTE_BuildCarrierString(subblockString, 0, MAX_SELECTOR_STRING, subblockCarrierString));

	RFmxCheckWarn(RFmxLTE_CfgComponentCarrier(instrumentHandle, subblockCarrierString ,componentCarrierBandwidth, componentCarrierFrequency, cellID));
	
	RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));

	RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_SLOTPOWER, RFMXLTE_VAL_TRUE));
	
	RFmxCheckWarn(RFmxLTE_SlotPowerCfgMeasurementInterval(instrumentHandle, "", measurementOffset,measurementLength));

	RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));
	
	
	/* Retrieve results */
	RFmxCheckWarn(RFmxLTE_SlotPowerFetchPowers(instrumentHandle, "",timeout,subFramePower,subFramePowerDelta,0,&actualArraySize));
	
	if( actualArraySize > 0 )
	{
		subFramePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
		subFramePowerDelta = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if(subFramePower && subFramePowerDelta)
		{
			RFmxCheckWarn(RFmxLTE_SlotPowerFetchPowers(instrumentHandle, "", timeout, subFramePower, subFramePowerDelta, 
				actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	/*Printing Results*/
	printf("Subframe Power(dBm):\n\n");
	for(i=0;i<actualArraySize;i++)
	{
		if(i== actualArraySize - 1)
		  printf("%lf ",subFramePower[i]);
		else
          printf("%lf, ",subFramePower[i]);
	}

	printf("\n\nSubframe Power Delta(dB):\n\n");
	for(i=0;i<actualArraySize-1;i++)
	{
		if(i== actualArraySize - 1)
		  printf("%lf",subFramePowerDelta[i]);
		else
		  printf("%lf, ",subFramePowerDelta[i]);
	}
	printf("\n");

Error:
	if( error )
	{
		RFmxLTE_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if (error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}

	if(instrumentHandle)
	{
		RFmxLTE_Close(instrumentHandle, RFMXLTE_VAL_FALSE);
	}

	/* Free allocated memory */
    if (subFramePower)
    {
        free(subFramePower);
    }
    if (subFramePowerDelta)
    {
        free(subFramePowerDelta);
    }

	printf("Press any key to exit\n");
	_getch();

	return error;
}
