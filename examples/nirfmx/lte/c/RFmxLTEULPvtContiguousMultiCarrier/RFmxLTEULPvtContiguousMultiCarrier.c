//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure Component Carriers.
//7. Select PvT measurement and enable Traces.
//8. Configure Duplex Scheme.
//9. Configure Measurement Method.
//10. Configure Averaging Parameters for PvT measurement.
//11. Initiate the Measurement.
//12. Fetch PvT Measurements and Traces.
//13. Close RFmx Session.  

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096
#define NUMBER_OF_COMPONENT_CARRIERS            2

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                     256

int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0, lastErrorCode = 0;
	int i = 0;
	char subblockCarrierString[NUMBER_OF_COMPONENT_CARRIERS][MAX_SELECTOR_STRING];

	float64 centerFrequency = 1.95e9;																			/*(Hz) */
	float64 externalAttenuation = 0.0;																			/*(dB) */
	float64 referenceLevel = 0.0;																				/*(dBm) */

	/* Frequency Reference */
	char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;																    /*(Hz) */

	/* Trigger */
	int32 enableTrigger = RFMXLTE_VAL_TRUE;    
	float64 IQPowerEdgeLevel = -20.0;																			/*(dB) */
	float64 triggerDelay = 0.0;																					/*(s) */
	int32 triggerMinimumQuietTimeMode = RFMXLTE_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
	float64 triggerMinimumQuietTimeDuration = 50.0e-6;														    /*(s) */ 
	char * IQPowerEdgeSource = "0";
	int32 IQPowerEdgeSlope = RFMXLTE_VAL_IQ_POWER_EDGE_RISING_SLOPE;
	int32 IQPowerEdgeLevelType = RFMXLTE_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
	float64 OFFPowerExclusionBefore = 0.0;                                                                      /*(s) */
	float64 OFFPowerExclusionAfter = 0.0;                                                                       /*(s) */
	int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_TDD;

	/* Spacing Settings */
	int32 componentCarrierSpacingType = RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL;
	int32 componentCarrierAtCenterFrequency = -1;

	/* Component Carrier Settings */
	float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS] = {20e6, 20e6};                             /*(Hz) */
	float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS] = {-9.9e6, 9.9e6};                          /*(Hz) */
	 
	int32 measurementMethod = RFMXLTE_VAL_PVT_MEASUREMENT_METHOD_NORMAL;

	/* Averaging */
	int32 averagingEnabled = RFMXLTE_VAL_PVT_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;
	int32 averagingType = RFMXLTE_VAL_PVT_AVERAGING_TYPE_RMS;

	int32 uplinkDownlinkConfiguraiton = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;     

	float64 timeout = 10.0;																						/*(s) */ 

	int32 measurementStatus[NUMBER_OF_COMPONENT_CARRIERS] = {0};
	float64 meanAbsoluteOFFPowerBefore[NUMBER_OF_COMPONENT_CARRIERS] = {0};								        /*(dBm) */
	float64 meanAbsoluteOFFPowerAfter[NUMBER_OF_COMPONENT_CARRIERS] = {0};								        /*(dBm) */
	float64 meanAbsoluteONPower[NUMBER_OF_COMPONENT_CARRIERS] = {0};										    /*(dBm) */
	float64 burstWidth[NUMBER_OF_COMPONENT_CARRIERS] = {0};													    /*(s) */

	float64 x0[NUMBER_OF_COMPONENT_CARRIERS] = {0.0}, dx[NUMBER_OF_COMPONENT_CARRIERS] = {0.0};
	float32 *signalPower[NUMBER_OF_COMPONENT_CARRIERS] = {NULL};
	float32 *absoluteLimit[NUMBER_OF_COMPONENT_CARRIERS] = {NULL};
	int32 actualArraySize = 0;

	/* Initialize a session */
	RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, 
		frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxLTE_CfgIQPowerEdgeTrigger(instrumentHandle, "", IQPowerEdgeSource, IQPowerEdgeSlope, 
		IQPowerEdgeLevel,	triggerDelay, triggerMinimumQuietTimeMode, triggerMinimumQuietTimeDuration, 
		IQPowerEdgeLevelType, enableTrigger));
	RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, "", componentCarrierSpacingType, 
		componentCarrierAtCenterFrequency));
	RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, "", NUMBER_OF_COMPONENT_CARRIERS));	
	RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, "", componentCarrierBandwidth, 
		componentCarrierFrequency, NULL, NUMBER_OF_COMPONENT_CARRIERS));    
	RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_PVT, RFMXLTE_VAL_TRUE));
	RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));
	RFmxCheckWarn(RFmxLTE_PVTCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
	RFmxCheckWarn(RFmxLTE_PVTCfgOFFPowerExclusionPeriods(instrumentHandle,"", OFFPowerExclusionBefore, 
		OFFPowerExclusionAfter));
	RFmxCheckWarn(RFmxLTE_PVTCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));    
	RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

	/* Retrieve results */

	RFmxCheckWarn(RFmxLTE_PVTFetchMeasurementArray(instrumentHandle, "", timeout, measurementStatus, 
		meanAbsoluteOFFPowerBefore, meanAbsoluteOFFPowerAfter, meanAbsoluteONPower, burstWidth, 
		NUMBER_OF_COMPONENT_CARRIERS, NULL));

	for( i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++ )
	{
		actualArraySize = 0;
		RFmxLTE_BuildCarrierString("", i, MAX_SELECTOR_STRING, subblockCarrierString[i]);
		RFmxCheckWarn(RFmxLTE_PVTFetchSignalPowerTrace(instrumentHandle, subblockCarrierString[i], timeout, 
			NULL, NULL, NULL, NULL, 0, &actualArraySize));		
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

	printf("\n********** Measurement ********** \n");

	for(i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
	{
		printf("Carrier  : %d\n", i);
		printf("Status                               : %s\n", measurementStatus[i]? "Pass": "Fail");
		printf("Mean Absolute OFF Power Before (dBm) : %f\n", meanAbsoluteOFFPowerBefore[i]);
		printf("Mean Absolute OFF Power After (dBm)  : %f\n", meanAbsoluteOFFPowerAfter[i]);
		printf("Mean Absolute ON Power (dBm)         : %f\n", meanAbsoluteONPower[i]);
		printf("Burst Width (s)                      : %f\n", burstWidth[i]);
		printf("---------------------------------------------\n");
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
	for( i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++ )
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
