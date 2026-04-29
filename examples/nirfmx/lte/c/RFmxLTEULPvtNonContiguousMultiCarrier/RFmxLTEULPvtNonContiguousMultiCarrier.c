/*Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Subblock Configurations.
5A. Configure Number of Subblocks.
5B. Configure subblock Frequency.
5C. Configure Component Carrier Spacing.
5D. Configure Number of Component Carriers.
5E. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
6. Select PvT measurement and enable Traces.
7. Configure Duplex Scheme.
8. Configure Measurements.
9. Configure Averaging Parameters for PvT measurement.
10. Initiate the Measurement.
11. Fetch PvT Measurements and Traces.
12. Close RFmx Session.*/  

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
}subblockInput_t;

/* Subblock measurement outputs structure */
typedef struct 
{
	int32 measurementStatus[NUMBER_OF_COMPONENT_CARRIERS];
	float64 meanAbsoluteOFFPowerBefore[NUMBER_OF_COMPONENT_CARRIERS];									/*(dBm) */
	float64 meanAbsoluteOFFPowerAfter[NUMBER_OF_COMPONENT_CARRIERS];	                        /*(dBm) */    
	float64 meanAbsoluteONPower[NUMBER_OF_COMPONENT_CARRIERS];											/*(dBm) */
	float64 burstWidth[NUMBER_OF_COMPONENT_CARRIERS];														/*(s) */
}subblockMeasurement_t;


int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;

	char subblockString[NUMBER_OF_SUBBLOCKS][MAX_SELECTOR_STRING];
	char subblockCarrierString[NUMBER_OF_SUBBLOCKS][MAX_SELECTOR_STRING];

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0, lastErrorCode = 0;
	int i = 0, j = 0;    

	/* Frequency Reference */
	char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;																/*(Hz) */

	/* Trigger */
	int32 enableTrigger = RFMXLTE_VAL_TRUE;    
	float64 IQPowerEdgeLevel = -20.0;																			/*(dB) */
	float64 OFFPowerExclusionBefore = 0.0;
	float64 OFFPowerExclusionAfter = 0.0;
	float64 triggerDelay = 0.0;																					/*(s) */
	int32 triggerMinimumQuietTimeMode = RFMXLTE_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
	float64 triggerMinimumQuietTimeDuration = 5.0e-6;														/*(s) */ 
	char * IQPowerEdgeSource = "0";
	int32 IQPowerEdgeSlope = RFMXLTE_VAL_IQ_POWER_EDGE_RISING_SLOPE;
	int32 IQPowerEdgeLevelType = RFMXLTE_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;

	int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_TDD;

	subblockInput_t subblockInput[NUMBER_OF_SUBBLOCKS] = {					/*  Set up subblock 0 inputs  */
	    {	
		 0.0,				                                    /* subblockFrequency */	
         RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL,
		-1,														/* componentCarrierAtCenterFrequency */
		{20e6},													/* componentCarrierBandwidth */
		{0.0}												    /* componentCarrierFrequency */
		},
		{				/*  Set up subblock 1 inputs  */
		    30e6,					                                /* subblockFrequency */	
			RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL, 
			-1,														/* componentCarrierAtCenterFrequency */
			{20e6},													/* componentCarrierBandwidth */
			{0.0}												    /* componentCarrierFrequency */
		}
	};	


	int32 measurementMethod = RFMXLTE_VAL_PVT_MEASUREMENT_METHOD_NORMAL;

	/* Averaging */
	int32 averagingEnabled = RFMXLTE_VAL_PVT_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;
	int32 averagingType = RFMXLTE_VAL_PVT_AVERAGING_TYPE_RMS;

	int32 uplinkDownlinkConfiguraiton = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;
	float64 timeout = 10.0;																				/*(s) */
	float64 centerFrequency = 1.95e9;																    /*(Hz) */
	float64 referenceLevel = 0.0;																	    /*(dBm) */
	float64 externalAttenuation = 0.0;																	/*(dB) */

	subblockMeasurement_t subblocksMsr[NUMBER_OF_SUBBLOCKS];

	int32 actualArraySize = 0;
	float64 x0[NUMBER_OF_SUBBLOCKS] = {0.0}, dx[NUMBER_OF_SUBBLOCKS] = {0.0};
	float32 *signalPower[NUMBER_OF_SUBBLOCKS] = {NULL};													/*(dBm) */
	float32 *absoluteLimit[NUMBER_OF_SUBBLOCKS] = {NULL};												/*(dBm) */

	/* Set up subblock outputs */
	for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
	{
		for( j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++ )
		{
			subblocksMsr[i].measurementStatus[j] = 0;
			subblocksMsr[i].meanAbsoluteOFFPowerBefore[j] = 0;
			subblocksMsr[i].meanAbsoluteOFFPowerAfter[j] = 0;
			subblocksMsr[i].meanAbsoluteONPower[j] = 0;
			subblocksMsr[i].burstWidth[j] = 0;
		}
	}

	/* Initialize a session */
	RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, 
		frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxLTE_CfgIQPowerEdgeTrigger(instrumentHandle, "", IQPowerEdgeSource, IQPowerEdgeSlope, 
		IQPowerEdgeLevel, triggerDelay, triggerMinimumQuietTimeMode, triggerMinimumQuietTimeDuration, 
		IQPowerEdgeLevelType, enableTrigger));
	RFmxCheckWarn(RFmxLTE_CfgNumberOfSubblocks(instrumentHandle, "", NUMBER_OF_SUBBLOCKS));

	for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
	{
		RFmxCheckWarn(RFmxLTE_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString[i]));
        RFmxCheckWarn(RFmxLTE_SetSubblockFrequency(instrumentHandle, subblockString[i], subblockInput[i].subblockFrequency));
		RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, subblockString[i], 
			subblockInput[i].componentCarrierSpacingType,
			subblockInput[i].componentCarrierAtCenterFrequency));
		RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, subblockString[i],
			NUMBER_OF_COMPONENT_CARRIERS));
		RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, subblockString[i],
			subblockInput[i].componentCarrierBandwidth, subblockInput[i].componentCarrierFrequency,
			NULL, NUMBER_OF_COMPONENT_CARRIERS));
	}
	RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle,"",RFMXLTE_VAL_PVT, RFMXLTE_VAL_TRUE));
	RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));
	RFmxCheckWarn(RFmxLTE_PVTCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
	RFmxCheckWarn(RFmxLTE_PVTCfgOFFPowerExclusionPeriods(instrumentHandle,"",OFFPowerExclusionBefore,OFFPowerExclusionAfter));
	RFmxCheckWarn(RFmxLTE_PVTCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
	RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

	for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
	{
		RFmxCheckWarn(RFmxLTE_PVTFetchMeasurementArray(instrumentHandle, subblockString[i], timeout, subblocksMsr[i].measurementStatus, 
			subblocksMsr[i].meanAbsoluteOFFPowerBefore, subblocksMsr[i].meanAbsoluteOFFPowerAfter, 
			subblocksMsr[i].meanAbsoluteONPower, subblocksMsr[i].burstWidth,
			NUMBER_OF_COMPONENT_CARRIERS, NULL));     
	}

	for( i = 0; i < NUMBER_OF_SUBBLOCKS; i++ )
	{
		actualArraySize = 0;
		RFmxCheckWarn(RFmxLTE_BuildCarrierString(subblockString[i], 0, MAX_SELECTOR_STRING, subblockCarrierString[i]));
		RFmxCheckWarn(RFmxLTE_PVTFetchSignalPowerTrace(instrumentHandle, subblockCarrierString[i], 
			timeout, NULL, NULL, NULL, NULL, 0, &actualArraySize));		
		if( actualArraySize > 0 )
		{	
			signalPower[i] = (float32 *) malloc(sizeof(float32) * actualArraySize);
			absoluteLimit[i] = (float32 *) malloc(sizeof(float32) * actualArraySize);
			if(signalPower && absoluteLimit)
			{
				RFmxCheckWarn(RFmxLTE_PVTFetchSignalPowerTrace(instrumentHandle, subblockCarrierString[i], 
					timeout, &x0[i], &dx[i], signalPower[i], absoluteLimit[i], actualArraySize, NULL));		
			}
			else
			{
				printf("malloc failed.\n");
				goto Error;
			}
		}
	}

	printf("\n********** Measurements ********** \n");
	for( i = 0; i < NUMBER_OF_SUBBLOCKS; i++ )
	{
		printf("Subblock  : %d\n", i);
		for(j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++)
		{
			printf("Carrier  : %d\n", j);
			printf("Status                               : %s\n", subblocksMsr[i].measurementStatus[j]? "Pass": "Fail");
			printf("Mean Absolute OFF Power Before (dBm) : %f\n", subblocksMsr[i].meanAbsoluteOFFPowerBefore[j]);
			printf("Mean Absolute OFF Power After (dBm)  : %f\n", subblocksMsr[i].meanAbsoluteOFFPowerAfter[j]);
			printf("Mean Absolute ON Power (dBm)         : %f\n", subblocksMsr[i].meanAbsoluteONPower[j]);
			printf("Burst Width (s)                      : %f\n", subblocksMsr[i].burstWidth[j]);
			printf("---------------------------------------------\n");
		}
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
	for( i = 0; i < NUMBER_OF_SUBBLOCKS; i++ )
	{
        if (signalPower[i])
        {
            free(signalPower[i]);
        }
        if (absoluteLimit[i])
        {
            free(absoluteLimit[i]);
        }
	}
	printf("Press any key to exit\n");
	_getch();   

	return error;
}
