/*Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Carrier Bandwidth.
6. Select PvT measurement and enable Traces.
7. Configure Duplex Scheme.
8. Configure Measurement Methods.
9. Configure Averaging Parameters for PvT measurement.
10. Initiate the Measurement.
11. Fetch PvT  Traces and Measurements.
12. Close RFmx Session.*/ 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096

int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0, lastErrorCode = 0;

	float64 centerFrequency = 1.95e9;																				/*(Hz) */	
	float64 referenceLevel = 0.0;																					/*(dBm) */
	float64 externalAttenuation = 0.0;																				/*(dB) */

	/* Frequency Reference */
	char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;																		/*(Hz) */

	/* Trigger */
	int32 enableTrigger = RFMXLTE_VAL_TRUE;    
	float64 IQPowerEdgeLevel = -20.0;																				/*(dB) */
	float64 triggerDelay = 0.0;																						/*(s) */
	int32 triggerMinimumQuietTimeMode = RFMXLTE_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
	float64 triggerMinimumQuietTimeDuration = 50.0e-6;																/*(s) */ 
	char * IQPowerEdgeSource = "0";
	int32 IQPowerEdgeSlope = RFMXLTE_VAL_IQ_POWER_EDGE_RISING_SLOPE;
	int32 IQPowerEdgeLevelType = RFMXLTE_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;

	int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_TDD;

	float64 componentCarrierBandwidth = 10.0e6;																		/*(Hz) */
	float64 componentCarrierFrequency = 0.0;																		/*(Hz) */
	int32 cellID = 0;

	int32 measurementMethod = RFMXLTE_VAL_PVT_MEASUREMENT_METHOD_NORMAL;

	/* Averaging */
	int32 averagingEnabled = RFMXLTE_VAL_PVT_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;
	int32 averagingType = RFMXLTE_VAL_PVT_AVERAGING_TYPE_RMS;

	int32 uplinkDownlinkConfiguraiton = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;

	float64 timeout = 10.0;																							/*(s) */
	float64 OFFPowerExclusionBefore = 0.0;                                                       
	float64 OFFPowerExclusionAfter = 0.0;

	int32 measurementStatus = 0;
	float64 meanAbsoluteOFFPowerBefore = 0.0;																		/*(dBm) */
	float64 meanAbsoluteOFFPowerAfter = 0.0;																		/*(dBm) */
	float64 meanAbsoluteONPower = 0.0;																				/*(dBm) */
	float64 burstWidth = 0.0;																						/*(s) */

	int32 actualArraySize = 0;
	float64 x0=0.0,dx=0.0;
	float32* signalPower = NULL;
	float32* absoluteLimit = NULL;

	/* Initialize a session */
	RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, 
		frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxLTE_CfgIQPowerEdgeTrigger(instrumentHandle, "", IQPowerEdgeSource, IQPowerEdgeSlope, 
		IQPowerEdgeLevel, triggerDelay, triggerMinimumQuietTimeMode, triggerMinimumQuietTimeDuration, 
		IQPowerEdgeLevelType, enableTrigger));
	RFmxCheckWarn(RFmxLTE_CfgComponentCarrier(instrumentHandle, "", componentCarrierBandwidth, 
		componentCarrierFrequency, cellID));
	RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_PVT, RFMXLTE_VAL_TRUE));
	RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));
	RFmxCheckWarn(RFmxLTE_PVTCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
	RFmxCheckWarn(RFmxLTE_PVTCfgOFFPowerExclusionPeriods(instrumentHandle,"",OFFPowerExclusionBefore,OFFPowerExclusionAfter));
	RFmxCheckWarn(RFmxLTE_PVTCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));    
	RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

	/* Retrieve results */

	RFmxCheckWarn(RFmxLTE_PVTFetchSignalPowerTrace(instrumentHandle, "", 
		timeout, NULL, NULL, NULL, NULL, 0, &actualArraySize));		
	if( actualArraySize > 0 )
	{	
		signalPower = (float32 *) malloc(sizeof(float32) * actualArraySize);
		absoluteLimit = (float32 *) malloc(sizeof(float32) * actualArraySize);
		if(signalPower && absoluteLimit)
		{
			RFmxCheckWarn(RFmxLTE_PVTFetchSignalPowerTrace(instrumentHandle, "", 
				timeout, &x0, &dx, signalPower, absoluteLimit, actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxLTE_PVTFetchMeasurement(instrumentHandle, "", timeout, &measurementStatus, 
		&meanAbsoluteOFFPowerBefore, &meanAbsoluteOFFPowerAfter, 
		&meanAbsoluteONPower, &burstWidth));

	printf("\n********** Measurement ********** \n");

	printf("Status                               : %s\n", measurementStatus? "Pass": "Fail");
	printf("Mean Absolute OFF Power Before (dBm) : %f\n", meanAbsoluteOFFPowerBefore);
	printf("Mean Absolute OFF Power After (dBm)  : %f\n", meanAbsoluteOFFPowerAfter);
	printf("Mean Absolute ON Power (dBm)         : %f\n", meanAbsoluteONPower);
	printf("Burst Width (s)                      : %f\n", burstWidth);

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
    if (signalPower)
    {
        free(signalPower);
    }
    if (absoluteLimit)
    {
        free(absoluteLimit);
    }

	printf("Press any key to exit\n");
	_getch();

	return error;
}
