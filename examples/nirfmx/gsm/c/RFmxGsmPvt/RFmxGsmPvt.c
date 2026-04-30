// Steps:
// 1. Open a new RFmx Session.
// 2. Configure Frequency Reference.
// 3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
// 4. Configure operating Band.
// 5. Configure Link Direction.
// 6. Configure Trigger Parameters for IQ Power Edge Trigger.
// 7. Configure Number of Timeslots.
// 8. Configure Auto TSC Detection Enabled.
// 9. Configure Signal Type.
// 10. Configure TSC.
// 11. Configure Power Control Level.
// 12  Select PVT measurement and enable Traces.
// 13. Configure RBW Filter Bandwidth (Hz) for PVT measurement.
// 14. Configure Averaging Parameters for PVT measurement.
// 15. Initiate the Measurement.
// 16. Fetch PVT Measurements and Traces.
// 17. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxGSM.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

#define NUMBER_OF_TIME_SLOTS		1

int main (int argc, char *argv[])
{
	//RFSA Configuration
	char *rfsaResourceName = "RFSA";

	niRFmxInstrHandle instrumentHandle = NULL;
	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0, lastErrorCode = 0;
	int32 i = 0;

	float64 centerFrequency = 890.2e6;						/* Hz */
	float64 referenceLevel = 0.0;							/* dBm */
	float64 externalAttenuation = 0.0;						/* dB */

	int32 band = RFMXGSM_VAL_BAND_PGSM;

	int32 linkDirection = RFMXGSM_VAL_LINK_DIRECTION_UPLINK;

	char * frequencyReferenceSource = RFMXGSM_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;				/* Hz */

	float64 triggerDelay = 0.0;								/* seconds */
	int32 enableTrigger = RFMXGSM_VAL_TRUE;
	float64 IQPowerEdgeLevel = -20.0;						/* dB */
	int32 minimumQuietTimeMode = RFMXGSM_VAL_MINIMUM_QUIET_TIME_MODE_AUTO;
	float64 minimumQuietTime = 582e-6;						/* seconds */

	int32 autoTSCDetectionEnabled = RFMXGSM_VAL_AUTO_TSC_DETECTION_ENABLED_TRUE;

	int32 modulationType = RFMXGSM_VAL_MODULATION_TYPE_8PSK;
	int32 burstType = RFMXGSM_VAL_BURST_TYPE_NB;
	int32 hbFilterWidth = RFMXGSM_VAL_HB_FILTER_WIDTH_NARROW;

	int32 TSC = RFMXGSM_VAL_TSC0;

	int32 powerControlLevel = 0;

	int32 averagingEnabled = RFMXGSM_VAL_ORFS_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;
	int32 averagingType = RFMXGSM_VAL_ORFS_AVERAGING_TYPE_RMS;

	float64 RBWFilterBandwidth = 500000.0;

	float64 timeout = 10.0;									/* seconds */
	
	/* Variables to store the results */
	int32 measurementStatus = RFMXGSM_VAL_PVT_MEASUREMENT_STATUS_FAIL;

	float64 slotMinimumPower[NUMBER_OF_TIME_SLOTS] = {0};
    float64 slotMaximumPower[NUMBER_OF_TIME_SLOTS] = {0};
    float64 slotAveragePower[NUMBER_OF_TIME_SLOTS] = {0};
    float64 slotBurstWidth[NUMBER_OF_TIME_SLOTS] = {0};
    int32 slotMeasurementStatus[NUMBER_OF_TIME_SLOTS] = {RFMXGSM_VAL_PVT_MEASUREMENT_STATUS_FAIL};
    float64 slotBurstThreshold[NUMBER_OF_TIME_SLOTS] = {0};

	float32* upperMask = NULL;
	float32* signalPower = NULL;
	float32* lowerMask = NULL;
	float64 x0 = 0;
	float64 dx = 0;
	int32 actualArraySize = 0;
	
	RFmxCheckWarn(RFmxGSM_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxGSM_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxGSM_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxGSM_CfgBand(instrumentHandle, "", band));
	RFmxCheckWarn(RFmxGSM_CfgLinkDirection(instrumentHandle, "", linkDirection));
	RFmxCheckWarn(RFmxGSM_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXGSM_VAL_IQ_POWER_EDGE_RISING_SLOPE, IQPowerEdgeLevel, 
		triggerDelay,  minimumQuietTimeMode, minimumQuietTime, RFMXGSM_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, enableTrigger));
	RFmxCheckWarn(RFmxGSM_CfgNumberOfTimeslots(instrumentHandle, "", NUMBER_OF_TIME_SLOTS));
	RFmxCheckWarn(RFmxGSM_CfgAutoTSCDetectionEnabled(instrumentHandle, "", autoTSCDetectionEnabled));
	RFmxCheckWarn(RFmxGSM_CfgSignalType(instrumentHandle, "slot::all", modulationType, burstType, hbFilterWidth));
	RFmxCheckWarn(RFmxGSM_CfgTSC(instrumentHandle, "slot::all", TSC));
	RFmxCheckWarn(RFmxGSM_CfgPowerControlLevel(instrumentHandle, "slot::all", powerControlLevel));
	RFmxCheckWarn(RFmxGSM_SelectMeasurements(instrumentHandle, "", RFMXGSM_VAL_PVT, RFMXGSM_VAL_TRUE));
	RFmxCheckWarn(RFmxGSM_PVTSetRBWFilterBandwidth(instrumentHandle, "", RBWFilterBandwidth));
	RFmxCheckWarn(RFmxGSM_PVTCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
	RFmxCheckWarn(RFmxGSM_Initiate(instrumentHandle, "", ""));
	
	RFmxCheckWarn(RFmxGSM_PVTFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));
	
	RFmxCheckWarn(RFmxGSM_PVTFetchSlotMeasurementArray(instrumentHandle, "", timeout, slotAveragePower, slotBurstWidth, 
				slotMeasurementStatus, slotMaximumPower, slotMinimumPower, slotBurstThreshold, NUMBER_OF_TIME_SLOTS, NULL));
		
	actualArraySize = 0;
	RFmxCheckWarn(RFmxGSM_PVTFetchPowerTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, 
		NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		upperMask = (float32*)malloc(sizeof(float32) * actualArraySize);
		signalPower = (float32*)malloc(sizeof(float32) * actualArraySize);
		lowerMask = (float32*)malloc(sizeof(float32) * actualArraySize);

		if( upperMask && signalPower && lowerMask )
		{
			RFmxCheckWarn(RFmxGSM_PVTFetchPowerTrace(instrumentHandle, "", timeout, &x0, &dx, upperMask, signalPower, 
				lowerMask, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}

	}

	printf("Measurement Status      : %s\n",measurementStatus == RFMXGSM_VAL_PVT_MEASUREMENT_STATUS_PASS?"Pass":"Fail");
	printf("\n--------------Slot Measurement--------------\n");
	for( i = 0; i < NUMBER_OF_TIME_SLOTS; i++ )
	{
		printf("\nSlot Measurement        : %d\n", i);
		printf("Average Power (dBm)     : %lf\n",slotAveragePower[i]);
		printf("Burst Width (s)         : %lf\n",slotBurstWidth[i]);
		printf("Maximum Power (dBm)     : %lf\n",slotMaximumPower[i]);
		printf("Minimum Power (dBm)     : %lf\n",slotMinimumPower[i]);
		printf("Burst Threshold (dBm)   : %lf\n",slotBurstThreshold[i]);
		printf("Measurement Status      : %s\n",slotMeasurementStatus[i] == RFMXGSM_VAL_PVT_MEASUREMENT_STATUS_PASS?"Pass":"Fail");
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

	if( upperMask )
		free(upperMask);
	if( signalPower )
		free(signalPower);
	if( lowerMask )
		free(lowerMask);

	printf("Press any key to exit\n");
	_getch();

	return error;
}
