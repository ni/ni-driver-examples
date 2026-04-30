//Steps:
//1.Open a new RFmx Session.
//2.Configure Frequency Reference.
//3.Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4.Configure Trigger Type and Trigger Parameters.
//5[A-E]. Configure Subblock Configurations.
//5A.Configure Number of Subblocks.
//5B.Configure subblock Frequency.
//5C.Configure Component Carrier Spacing.
//5D.Configure Number of Component Carriers.
//5E.Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
//6.Select SlotPower measurement and enable Traces.
//7.Configure Duplex Scheme.
//8.Configure Measurement Interval.
//9.Initiate the Measurement.
//10. Fetch SlotPower Measurements and Traces.
//11. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                     256

#define NUMBER_OF_SUBBLOCKS                     2
#define NUMBER_OF_COMPONENT_CARRIERS            1

/* Subblock inputs structure */
typedef struct 
{
    float64 subblockFrequency;                                          /*(Hz) */
	int32 componentCarrierSpacingType;
	int32 componentCarrierAtCenterFrequency;
	float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS];    /*(Hz) */
	float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS];    /*(Hz) */
	int32 cellID[NUMBER_OF_COMPONENT_CARRIERS];
}subblockInput_t;

/* Subblock measurement outputs structure */
typedef struct 
{
	int32 numberOfSubframes;
	float64* subframePower;                                 /*(dBm) */
	float64* subframePowerDelta;                            /*(dBm) */
}subblockMeasurement_t;


int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;

	char subblockString[MAX_SELECTOR_STRING];
	char carrierString[MAX_SELECTOR_STRING];

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0, lastErrorCode = 0;
	int i = 0, j = 0;    

	char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;                     /*(Hz) */

	float64 centerFrequency = 1.95e9;                               /*(Hz) */
	float64 referenceLevel = 0.0;                                   /*(dBm) */
	float64 externalAttenuation = 0.0;                              /*(dB) */

	int32 enableTrigger = RFMXLTE_VAL_FALSE;
	char * digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
	int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.0;                                     /*(s) */

	int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_FDD;
	int32 uplinkDownlinkConfiguraiton = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;

	subblockInput_t subblockInput[NUMBER_OF_SUBBLOCKS] = {			/*  Set up subblock 0 inputs  */
	    {	
		 0.0,				                                        /* subblockFrequency */	
         RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL,
		-1,														    /* componentCarrierAtCenterFrequency */
		{20e6},													    /* componentCarrierBandwidth */
		{0.0},													    /* componentCarrierFrequency */
		{0}                                                         /* cell ID */
		},
		{				/*  Set up subblock 1 inputs  */
		    30e6,					                                /* subblockFrequency */	
			RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL, 
			-1,													    /* componentCarrierAtCenterFrequency */
			{20e6},													/* componentCarrierBandwidth */
			{0.0},													/* componentCarrierFrequency */
			{1}                                                     /* cell ID */
		}
	};	


	float64 timeout = 10.0;                                         /*(s) */

	subblockMeasurement_t subblocksMsr[NUMBER_OF_SUBBLOCKS];

	int32 measurementOffset = 0;                                    /*Subframes*/
	int32 measurementLength = 10;                                   /*Subframes*/

	/* Set up subblock outputs */
	for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
	{
		subblocksMsr[i].subframePower = NULL;
		subblocksMsr[i].subframePowerDelta = NULL;
	}

	/* Initialize a session */
	RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, 
		frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));

	RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, 
		enableTrigger));
	
	RFmxCheckWarn(RFmxLTE_CfgNumberOfSubblocks(instrumentHandle, "", NUMBER_OF_SUBBLOCKS));

	for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
	{
		RFmxCheckWarn(RFmxLTE_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString));
        RFmxCheckWarn(RFmxLTE_SetSubblockFrequency(instrumentHandle, subblockString, subblockInput[i].subblockFrequency));
		RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, subblockString, 
			subblockInput[i].componentCarrierSpacingType,
			subblockInput[i].componentCarrierAtCenterFrequency));
		RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, subblockString,
			NUMBER_OF_COMPONENT_CARRIERS));
		RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, subblockString,
			subblockInput[i].componentCarrierBandwidth, subblockInput[i].componentCarrierFrequency,
			subblockInput[i].cellID, NUMBER_OF_COMPONENT_CARRIERS));
	}

	RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));

	RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_SLOTPOWER, RFMXLTE_VAL_TRUE));
	
	RFmxCheckWarn(RFmxLTE_SlotPowerCfgMeasurementInterval(instrumentHandle, "",measurementOffset,measurementLength));

	RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

	RFmxCheckWarn(RFmxLTE_BuildCarrierString("", 0, MAX_SELECTOR_STRING, carrierString));

	for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
	{		
		RFmxCheckWarn(RFmxLTE_BuildSubblockString(carrierString, i, MAX_SELECTOR_STRING, subblockString));

		RFmxCheckWarn(RFmxLTE_SlotPowerFetchPowers(instrumentHandle, subblockString, timeout,
			NULL, NULL, 0,&subblocksMsr[i].numberOfSubframes));
		if( subblocksMsr[i].numberOfSubframes > 0 )
		{
			subblocksMsr[i].subframePower = (float64 *) malloc(sizeof(float64) * subblocksMsr[i].numberOfSubframes);
			subblocksMsr[i].subframePowerDelta = (float64 *) malloc(sizeof(float64) * subblocksMsr[i].numberOfSubframes);

			/* Fetch the measurements array */
			if( subblocksMsr[i].subframePower && subblocksMsr[i].subframePowerDelta )
			{
				RFmxCheckWarn(RFmxLTE_SlotPowerFetchPowers(instrumentHandle, subblockString, timeout,
					subblocksMsr[i].subframePower,
					subblocksMsr[i].subframePowerDelta,
					subblocksMsr[i].numberOfSubframes, NULL));
			}
			else
			{
				printf("malloc failed.\n");
				goto Error;
			}
		}
	}

	printf("\n****************Subblock Measurements****************\n");
	for(i = 0;i < NUMBER_OF_SUBBLOCKS; i++)
	{
		printf("\nSubblock %d\n", i);

		for(j=0;j<subblocksMsr[i].numberOfSubframes;j++)
		{
			printf("\nSubframe %d\n", j);
			printf("Subframe Power       (dB)       : %f\n", subblocksMsr[i].subframePower[j]);
			printf("Subframe Delta Power (dB)       : %f\n", subblocksMsr[i].subframePowerDelta[j]);            
		}
		printf("------------------------------------------\n");
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
	for(i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
	{
        if (subblocksMsr[i].subframePower)
        {
            free(subblocksMsr[i].subframePower);
        }
        if (subblocksMsr[i].subframePowerDelta)
        {
            free(subblocksMsr[i].subframePowerDelta);
        }
	}

	printf("Press any key to exit\n");
	_getch();

	return error;
}
