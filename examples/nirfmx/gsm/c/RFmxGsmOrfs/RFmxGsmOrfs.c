// Steps:
// 1. Open a new RFmx Session.
// 2. Configure Frequency Reference.
// 3. Configure External Attenuation.
// 4. Configure Center Frequency. 
// 5. Configure Link Direction.
// 6. Configure Trigger Parameters for IQ Power Edge Trigger.
// 7. Configure Number of Timeslots.
// 8. Configure Signal Type.
// 9. Configure Auto Level.
// 10. Configure Auto TSC Detection Enabled.
// 11. Configure TSC.
// 12. Select  ORFS  measurement and enable Traces.
// 13. Configure Noise Compensation Enabled.
// 14. Configure Measurement Type. 
// 15. Configure Offset Frequency Mode.
// 16. Configure Evaluation Symbols.
// 17. Configure Averaging Parameters.
// 18. Initiate the Measurement.
// 19. Fetch ORFS Measurements and Traces.
// 20. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxGSM.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

#define AUTO_LEVEL_ON				1
#define AUTO_LEVEL_OFF				0

int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";

	niRFmxInstrHandle instrumentHandle = NULL;
	
	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0;
	int32 lastErrorCode = 0;
	
	char * frequencyReferenceSource = RFMXGSM_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;				/* Hz */
	
	float64 centerFrequency = 890.2e6;						/* Hz */
	float64 referenceLevel = 0.0;							/* dBm */
	float64 externalAttenuation = 0.0;						/* dB */
	
	int32 linkDirection = RFMXGSM_VAL_LINK_DIRECTION_UPLINK;
	
	float64 triggerDelay = 0.0;								/* seconds */
	int32 enableTrigger = RFMXGSM_VAL_TRUE;
	float64 iqPowerEdgeLevel = -20.0;						/* dB */
	int32 minimumQuietTimeMode = RFMXGSM_VAL_MINIMUM_QUIET_TIME_MODE_AUTO;
	float64 minimumQuietTime = 582e-6;						/* seconds */
	
	int32 numberOfTimeslots = 1;

	int32 autoLevel = AUTO_LEVEL_ON;
	float64 autoReferenceLevel = 0;
	float64 measurementInterval = 0.0046;

	int32 modulationType = RFMXGSM_VAL_MODULATION_TYPE_8PSK;
	int32 burstType = RFMXGSM_VAL_BURST_TYPE_NB;
	int32 HBFilterWidth = RFMXGSM_VAL_HB_FILTER_WIDTH_NARROW;
	
	int32 autoTSCDetectionEnabled = RFMXGSM_VAL_AUTO_TSC_DETECTION_ENABLED_TRUE;
	
	int32 TSC = RFMXGSM_VAL_TSC0;
	
	int32 noiseCompensationEnabled = RFMXGSM_VAL_ORFS_NOISE_COMPENSATION_ENABLED_FALSE;
	
	int32 measurementType = RFMXGSM_VAL_ORFS_MEASUREMENT_TYPE_MODULATION_AND_SWITCHING;
	
	int32 offsetFrequencyMode = RFMXGSM_VAL_ORFS_OFFSET_FREQUENCY_MODE_STANDARD;
	
	float64 evaluationSymbolsStart = 50.0;					/*(%) */
	int32 evaluationSymbolsIncludeTSC = 0;
	float64 evaluationSymbolsStop = 90.0;					/*(%) */
	
	int32 averagingEnabled = RFMXGSM_VAL_ORFS_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;
	int32 averagingType = RFMXGSM_VAL_ORFS_AVERAGING_TYPE_LOG;
	
	float64 timeout = 10.0;									/* seconds */
	float64 modulationCarrierPower = 0.0;					/* dBm */
	float64* modLowerAbsolutePower = NULL;
	float64* modUpperAbsolutePower = NULL;
	float64* modLowerRelativePower = NULL;
	float64* modUpperRelativePower = NULL;
	int32 modulationResArraySize = 0;

	float64 switchingCarrierPower = 0.0;					/* dBm */
	float64* switchLowerAbsolutePower = NULL;
	float64* switchUpperAbsolutePower = NULL;
	float64* switchLowerRelativePower = NULL;
	float64* switchUpperRelativePower = NULL;
	int32 switchingResArraySize = 0;
	int32 i = 0;

	float32* modOffsetFrequency = NULL;
	float32* modAbsolutePower = NULL;
	float32* modRelativePower = NULL;
	float32* switchOffsetFrequency = NULL;
	float32* switchAbsolutePower = NULL;
	float32* switchRelativePower = NULL;
	int32 actualArraySize = 0;

	RFmxCheckWarn(RFmxGSM_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxGSM_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxGSM_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
	RFmxCheckWarn(RFmxGSM_CfgFrequency(instrumentHandle, "", centerFrequency));
	RFmxCheckWarn(RFmxGSM_CfgLinkDirection(instrumentHandle, "", linkDirection));
	RFmxCheckWarn(RFmxGSM_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXGSM_VAL_IQ_POWER_EDGE_RISING_SLOPE, 
               		                            iqPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime, 
												RFMXGSM_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, enableTrigger)); 
	RFmxCheckWarn(RFmxGSM_CfgNumberOfTimeslots(instrumentHandle, "", numberOfTimeslots));

	RFmxCheckWarn(RFmxGSM_CfgSignalType(instrumentHandle, "slot::all", modulationType, burstType, HBFilterWidth));

	if(autoLevel)
	{
		RFmxCheckWarn(RFmxGSM_AutoLevel(instrumentHandle, "", measurementInterval, &autoReferenceLevel));
		printf("Reference Level                     : %f\n", autoReferenceLevel);
	}
	else
		RFmxCheckWarn(RFmxGSM_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
		
	RFmxCheckWarn(RFmxGSM_CfgAutoTSCDetectionEnabled(instrumentHandle, "", autoTSCDetectionEnabled));
	RFmxCheckWarn(RFmxGSM_CfgTSC(instrumentHandle, "slot::all", TSC));
	RFmxCheckWarn(RFmxGSM_SelectMeasurements(instrumentHandle, "", RFMXGSM_VAL_ORFS, RFMXGSM_VAL_TRUE));
	RFmxCheckWarn(RFmxGSM_ORFSCfgNoiseCompensationEnabled(instrumentHandle, "", noiseCompensationEnabled));
	RFmxCheckWarn(RFmxGSM_ORFSCfgMeasurementType(instrumentHandle, "", measurementType));
	RFmxCheckWarn(RFmxGSM_ORFSCfgOffsetFrequencyMode(instrumentHandle, "", offsetFrequencyMode));
	RFmxCheckWarn(RFmxGSM_ORFSCfgEvaluationSymbols(instrumentHandle, "", evaluationSymbolsStart, evaluationSymbolsIncludeTSC, 
		                                           evaluationSymbolsStop));
	RFmxCheckWarn(RFmxGSM_ORFSCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
	RFmxCheckWarn(RFmxGSM_Initiate(instrumentHandle, "", ""));

	RFmxCheckWarn(RFmxGSM_ORFSFetchModulationResultsArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, 0, 
		&modulationResArraySize));

	if( modulationResArraySize > 0 )
	{
		modLowerRelativePower = (float64*)malloc(sizeof(float64) * modulationResArraySize);
		modUpperRelativePower = (float64*)malloc(sizeof(float64) * modulationResArraySize);
		modLowerAbsolutePower = (float64*)malloc(sizeof(float64) * modulationResArraySize);
		modUpperAbsolutePower = (float64*)malloc(sizeof(float64) * modulationResArraySize);

		if( modLowerRelativePower && modUpperRelativePower && modLowerAbsolutePower &&  modUpperAbsolutePower )
		{
			RFmxCheckWarn(RFmxGSM_ORFSFetchModulationResultsArray(instrumentHandle, "", timeout, &modulationCarrierPower, 
				                                                  modLowerRelativePower, modUpperRelativePower,
																  modLowerAbsolutePower, modUpperAbsolutePower, 
																  modulationResArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}

	}
	
	RFmxCheckWarn(RFmxGSM_ORFSFetchSwitchingResultsArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, 0, 
	                                	                 &switchingResArraySize));

	if( switchingResArraySize > 0 )
	{
		switchLowerRelativePower = (float64*)malloc(sizeof(float64) * switchingResArraySize);
		switchUpperRelativePower = (float64*)malloc(sizeof(float64) * switchingResArraySize);
		switchLowerAbsolutePower = (float64*)malloc(sizeof(float64) * switchingResArraySize);
		switchUpperAbsolutePower = (float64*)malloc(sizeof(float64) * switchingResArraySize);

		if( switchLowerRelativePower && switchUpperRelativePower && switchLowerAbsolutePower &&  switchUpperAbsolutePower )
		{
			RFmxCheckWarn(RFmxGSM_ORFSFetchSwitchingResultsArray(instrumentHandle, "", timeout, &switchingCarrierPower, 
				switchLowerRelativePower, switchUpperRelativePower, switchLowerAbsolutePower, switchUpperAbsolutePower, 
				switchingResArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}

	}
	
	actualArraySize = 0;
	RFmxCheckWarn(RFmxGSM_ORFSFetchModulationPowerTrace(instrumentHandle, "", timeout, NULL, 
														NULL, NULL, 0, &actualArraySize));
	
	if( actualArraySize > 0 )
	{
		modOffsetFrequency = (float32*)malloc(sizeof(float32) * actualArraySize);
		modAbsolutePower = (float32*)malloc(sizeof(float32) * actualArraySize);
		modRelativePower = (float32*)malloc(sizeof(float32) * actualArraySize);

		if( modOffsetFrequency && modAbsolutePower && modRelativePower )
		{
			RFmxCheckWarn(RFmxGSM_ORFSFetchModulationPowerTrace(instrumentHandle, "", timeout, modOffsetFrequency, 
																modAbsolutePower, modRelativePower, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}

	}
	
	actualArraySize = 0;
	RFmxCheckWarn(RFmxGSM_ORFSFetchSwitchingPowerTrace(instrumentHandle, "", timeout, NULL, 
																NULL, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		switchOffsetFrequency = (float32*)malloc(sizeof(float32) * actualArraySize);
		switchAbsolutePower = (float32*)malloc(sizeof(float32) * actualArraySize);
		switchRelativePower = (float32*)malloc(sizeof(float32) * actualArraySize);

		if( switchOffsetFrequency && switchAbsolutePower && switchRelativePower )
		{
			RFmxCheckWarn(RFmxGSM_ORFSFetchSwitchingPowerTrace(instrumentHandle, "", timeout, switchOffsetFrequency, 
																switchAbsolutePower, switchRelativePower, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}

	}
	
	printf("---------Modulation Results---------\n");
	printf("Modulation Carrier Power  (dBm)     : %lf\n\n",modulationCarrierPower);
	for(i = 0; i < modulationResArraySize; i++)
	{
		printf("Offset : %d\n", i);
		printf("Lower Absolute Power (dBm)	    : %lf\n", modLowerAbsolutePower[i]);
		printf("Lower Relative Power (dB)	    : %lf\n", modLowerRelativePower[i]);
		printf("Upper Absolute Power (dBm)	    : %lf\n", modUpperAbsolutePower[i]);
		printf("Upper Relative Power (dB)	    : %lf\n", modUpperRelativePower[i]);
	}
	printf("---------Switching Results---------\n");
	printf("Switching Carrier Power  (dBm)      : %lf\n\n",switchingCarrierPower);
	for(i = 0; i < switchingResArraySize; i++)
	{
		printf("Offset : %d\n", i);
		printf("Lower Absolute Power (dBm)          : %lf\n", switchLowerAbsolutePower[i]);
		printf("Lower Relative Power (dB)           : %lf\n", switchLowerRelativePower[i]);
		printf("Upper Absolute Power (dBm)          : %lf\n", switchUpperAbsolutePower[i]);
		printf("Upper Relative Power (dB)           : %lf\n", switchUpperRelativePower[i]);
	}
	
Error:
	if( error )
	{
		RFmxGSM_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if (error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}
	if(instrumentHandle)
	{
		RFmxGSM_Close(instrumentHandle, RFMXGSM_VAL_FALSE);
	}

	if( modLowerRelativePower )
		free(modLowerRelativePower);
	if( modUpperRelativePower )
		free(modUpperRelativePower);
	if( modLowerAbsolutePower )
		free(modLowerAbsolutePower);
	if( modUpperAbsolutePower )
		free(modUpperAbsolutePower);
	if( switchLowerRelativePower )
		free(switchLowerRelativePower);
	if( switchUpperRelativePower )
		free(switchUpperRelativePower);
	if( switchLowerAbsolutePower )
		free(switchLowerAbsolutePower);
	if( switchUpperAbsolutePower )
		free(switchUpperAbsolutePower);
	if( modOffsetFrequency )
		free(modOffsetFrequency);
	if( modAbsolutePower )
		free(modAbsolutePower);
	if( modRelativePower )
		free(modRelativePower);
	if( switchOffsetFrequency )
		free(switchOffsetFrequency);
	if( switchAbsolutePower )
		free(switchAbsolutePower);
	if( switchRelativePower )
		free(switchRelativePower);
	printf("Press any key to exit\n");
	_getch();

	return error;
}
