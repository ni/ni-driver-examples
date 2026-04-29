//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure Component Carriers.
//7. Configure Duplex Scheme.
//8. Select SlotPower measurement and enable Traces.
//9. Configure Measurement Method.
//10. Initiate the Measurement.
//11[A-B]. Fetch SlotPower Measurements and Traces.
//12. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096
#define NUMBER_OF_COMPONENT_CARRIERS            2
#define MAX_SELECTOR_STRING                     256

typedef struct 
{
	int32 numberOfCarriers;
	float64* subframePower;                                 /*(dBm) */
	float64* subframePowerDelta;                            /*(dBm) */
}componentCarrierMeasurement_t;

int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0, lastErrorCode = 0;
	int i = 0,j=0;

	char carrierString[MAX_SELECTOR_STRING];
	char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;                                                      /*(Hz) */

	int32 numberOfComponentCarriers = NUMBER_OF_COMPONENT_CARRIERS;

	float64 centerFrequency = 1.95e9;                                                                /*(Hz) */
	float64 externalAttenuation = 0.0;                                                               /*(dB) */

	int32 enableTrigger = RFMXLTE_VAL_FALSE;    
	char * digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
	int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.0;                                                                      /*(s) */

	int32 componentCarrierSpacingType = RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL;
	int32 componentCarrierAtCenterFrequency = -1;

	float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS] = {20e6, 20e6};                  /*(Hz) */
	float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS] = {-9.9e6, 9.9e6};               /*(Hz) */
	int32 cellID[NUMBER_OF_COMPONENT_CARRIERS] = {0, 1};

	float64 referenceLevel = 0.0;                                                                    /*(dBm) */

	int32 measurementOffset = 0;                                                                     /*Subframes*/
	int32 measurementLength = 10;                                                                    /*Subframes*/

	int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_FDD;
	int32 uplinkDownlinkConfiguraiton = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;

	float64 timeout = 10.0;                                                                          /*(s) */ 

	componentCarrierMeasurement_t componentCarrierMsr[NUMBER_OF_COMPONENT_CARRIERS];
	for(i = 0;i < numberOfComponentCarriers ; i++)
	{
		componentCarrierMsr[i].subframePower = NULL;
		componentCarrierMsr[i].subframePowerDelta = NULL;
	}

	/* Initialize a session */
	RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, 
		frequencyReferenceFrequency));

	RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "",centerFrequency,referenceLevel, externalAttenuation));

	RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource,
		digitalEdge, triggerDelay, enableTrigger));

	RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, "", 
		componentCarrierSpacingType, 
		componentCarrierAtCenterFrequency));
	RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, "", 
		numberOfComponentCarriers));	
	RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, "", 
		componentCarrierBandwidth, 
		componentCarrierFrequency, cellID, numberOfComponentCarriers));

	RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_SLOTPOWER, RFMXLTE_VAL_TRUE));

	RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));

	RFmxCheckWarn(RFmxLTE_SlotPowerCfgMeasurementInterval(instrumentHandle, "", measurementOffset,measurementLength));

	RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

	/* Retrieve results */
	for(i = 0;i < numberOfComponentCarriers ; i++)
	{
		RFmxCheckWarn(RFmxLTE_BuildCarrierString("", i, MAX_SELECTOR_STRING, carrierString));
		RFmxCheckWarn(RFmxLTE_SlotPowerFetchPowers(instrumentHandle,carrierString,timeout,
			componentCarrierMsr[i].subframePower,componentCarrierMsr[i].subframePowerDelta,0,&componentCarrierMsr[i].numberOfCarriers));
		if(componentCarrierMsr[i].numberOfCarriers > 0)
		{
			componentCarrierMsr[i].subframePower = (float64*)malloc(sizeof(float64)*componentCarrierMsr[i].numberOfCarriers);
			componentCarrierMsr[i].subframePowerDelta = (float64*)malloc(sizeof(float64)*componentCarrierMsr[i].numberOfCarriers);
			if( componentCarrierMsr[i].subframePower && componentCarrierMsr[i].subframePowerDelta )
			{
				RFmxCheckWarn(RFmxLTE_SlotPowerFetchPowers(instrumentHandle, carrierString, timeout,
					componentCarrierMsr[i].subframePower,
					componentCarrierMsr[i].subframePowerDelta,
					componentCarrierMsr[i].numberOfCarriers, NULL));
			}
			else
			{
				printf("malloc failed.\n");
				goto Error;
			}		
		}
	}

	/*Printing Results*/
	for(i = 0;i < numberOfComponentCarriers ; i++)
	{
		printf("CC%d Trace:\n",i);
		printf("\nSubframe Power:\n");
		for(j=0;j<componentCarrierMsr[i].numberOfCarriers;j++)
		{
			if(j==componentCarrierMsr[i].numberOfCarriers-1)
			{
				printf("%lf",componentCarrierMsr[i].subframePower[j]);
			}
			else
			{
			printf("%lf,",componentCarrierMsr[i].subframePower[j]);
			}
		}
		printf("\nSubframe Power Delta:\n");
		for(j=0;j<componentCarrierMsr[i].numberOfCarriers;j++)
		{
			if(j==componentCarrierMsr[i].numberOfCarriers-1)
			{
				printf("%lf",componentCarrierMsr[i].subframePowerDelta[j]);
			}
			else
			{
				printf("%lf,",componentCarrierMsr[i].subframePowerDelta[j]);
			}
		}
		printf("\n\n");
	}
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
	for(i = 0;i < numberOfComponentCarriers ; i++)
	{
        if (componentCarrierMsr[i].subframePower)
        {
            free(componentCarrierMsr[i].subframePower);
        }
        if (componentCarrierMsr[i].subframePowerDelta)
        {
            free(componentCarrierMsr[i].subframePowerDelta);
        }
	}

	printf("Press any key to exit\n");
	_getch();

	return error;
}
