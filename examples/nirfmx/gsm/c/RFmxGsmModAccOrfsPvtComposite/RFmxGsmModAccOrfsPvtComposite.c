// Steps:
// 1. Open a new RFmx Session.
// 2. Configure Frequency Reference.
// 3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
// 4. Configure operating Band.
// 5. Configure Link Direction.
// 6. Configure Trigger Parameters for IQ Power Edge Trigger.
// 7. Select  measurements and enable Traces.
// 8. Configure Number of Timeslots.
// 9. Configure Averaging Parameters for ModAcc measurement.
// 10. Configure Averaging Parameters for ORFS measurement.
// 11. Configure Averaging Parameters for PVT measurement.
// 12. Configure ORFS Measurement Type.
// 13. Configure Offset Frequency Mode for ORFS measurement.
// 14. Configure Auto TSC Detection Enabled.
// 15. Configure Signal Type.
// 16. Configure TSC.
// 17. Configure Power Control Level.
// 18. Initiate the Measurement.
// 19. Fetch ModAcc/ORFS/PVT Measurements and Traces.
// 20. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxGSM.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

#define NUMBER_OF_TIMESLOTS			1

int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";	
	niRFmxInstrHandle instrumentHandle = NULL;
	
	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0;
	int32 lastErrorCode = 0;
	int i = 0;

	char * frequencyReferenceSource = RFMXGSM_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;				/* Hz */

	float64 centerFrequency = 890.2e6;						/* Hz */
	float64 referenceLevel = 0.0;							/* dBm */
	float64 externalAttenuation = 0.0;						/* dB */

	int32 band = RFMXGSM_VAL_BAND_PGSM;

	int32 linkDirection = RFMXGSM_VAL_LINK_DIRECTION_UPLINK;

	float64 triggerDelay = 0.0;								/* seconds */
	int32 enableTrigger = RFMXGSM_VAL_TRUE;
	float64 IQPowerEdgeLevel = -20.0;						/* dB */
	int32 minimumQuietTimeMode = RFMXGSM_VAL_MINIMUM_QUIET_TIME_MODE_AUTO;
	float64 minimumQuietTime = 582e-6;						/* seconds */

	int32 averagingEnabled = RFMXGSM_VAL_PVT_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;
	int32 averagingType = RFMXGSM_VAL_PVT_AVERAGING_TYPE_RMS;

	int32 measurementType = RFMXGSM_VAL_ORFS_MEASUREMENT_TYPE_MODULATION_AND_SWITCHING;

	int32 autoTSCDetectionEnabled = RFMXGSM_VAL_AUTO_TSC_DETECTION_ENABLED_TRUE;

	int32 modulationType = RFMXGSM_VAL_MODULATION_TYPE_8PSK;
	int32 burstType = RFMXGSM_VAL_BURST_TYPE_NB;
	int32 HBFilterWidth = RFMXGSM_VAL_HB_FILTER_WIDTH_NARROW;

	int32 TSC = RFMXGSM_VAL_TSC0;

	int32 powerControlLevel = 0;
	float64 timeout = 10.0;									/* seconds */

	int32 peakEVMSymbol = 0;
	float64 EVMMeanFrequencyError = 0.0;					/* Hz */
	float64 meanIQGainImbalance = 0.0;						/* dB */
	float64 maximumIQGainImbalance = 0.0;					/* dB */
	float64 meanIQOriginOffset = 0.0;						/* dB */
	float64 maximumIQOriginOffset = 0.0;					/* dB */
	
	int32 detectedTSC[NUMBER_OF_TIMESLOTS] = {0};
	
	float64 meanPeakEVM = 0.0;								/*(%) */
	float64 meanRMSEVM = 0.0;								/*(%) */
	float64 maximumPeakEVM = 0.0;							/*(%) */
	float64 maximumRMSEVM = 0.0;							/*(%) */
	float64 ninetyFifthPercentileEVM = 0.0;					/*(%) */
	float64 maximumPeakPhaseError = 0.0;					/*(deg) */
	float64 meanRMSPhaseError = 0.0;						/*(deg) */
	float64 maximumRMSPhaseError = 0.0;						/*(deg) */
	float64 meanPeakPhaseError = 0.0;						/*(deg) */
	float64 PFERMeanFrequencyError = 0.0;					/* Hz */
	int32 peakSymbol = 0;

	float64 modulationCarrierPower = 0.0;					/*(dBm) */
	float64* modLowerAbsolutePower = NULL;
	float64* modUpperAbsolutePower = NULL;
	float64* modLowerRelativePower = NULL;
	float64* modUpperRelativePower = NULL;
	int32 modResultsArraySize = 0;
	
	float64 switchingCarrierPower = 0.0;					/*(dBm) */
	float64* switchLowerRelativePower = NULL;
	float64* switchUpperRelativePower = NULL;
	float64* switchLowerAbsolutePower = NULL;
	float64* switchUpperAbsolutePower = NULL;
	int32 switchingResultsArraySize = 0;

	float64 slotMinimumPower[NUMBER_OF_TIMESLOTS] = {0};
	float64 slotMaximumPower[NUMBER_OF_TIMESLOTS] = {0};
	float64 slotAveragePower[NUMBER_OF_TIMESLOTS] = {0};
	float64 slotBurstWidth[NUMBER_OF_TIMESLOTS] = {0};
	int32 slotMeasurementStatus[NUMBER_OF_TIMESLOTS] = {0};
	float64 slotBurstThreshold[NUMBER_OF_TIMESLOTS] = {0};

	int32 measurementStatus = RFMXGSM_VAL_PVT_MEASUREMENT_STATUS_FAIL;

	RFmxCheckWarn(RFmxGSM_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxGSM_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxGSM_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxGSM_CfgBand(instrumentHandle, "", band));
	RFmxCheckWarn(RFmxGSM_CfgLinkDirection(instrumentHandle, "", linkDirection));
	RFmxCheckWarn(RFmxGSM_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXGSM_VAL_IQ_POWER_EDGE_RISING_SLOPE, 
		                                        IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
												RFMXGSM_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, enableTrigger)); 
	RFmxCheckWarn(RFmxGSM_SelectMeasurements(instrumentHandle, "", RFMXGSM_VAL_MODACC | RFMXGSM_VAL_ORFS | RFMXGSM_VAL_PVT, RFMXGSM_VAL_TRUE));
	RFmxCheckWarn(RFmxGSM_CfgNumberOfTimeslots(instrumentHandle, "", NUMBER_OF_TIMESLOTS));
	RFmxCheckWarn(RFmxGSM_ModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
	RFmxCheckWarn(RFmxGSM_ORFSCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
	RFmxCheckWarn(RFmxGSM_PVTCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
	RFmxCheckWarn(RFmxGSM_ORFSCfgMeasurementType(instrumentHandle, "", measurementType));
	RFmxCheckWarn(RFmxGSM_ORFSCfgOffsetFrequencyMode(instrumentHandle, "", RFMXGSM_VAL_ORFS_OFFSET_FREQUENCY_MODE_STANDARD));
	RFmxCheckWarn(RFmxGSM_CfgAutoTSCDetectionEnabled(instrumentHandle, "", autoTSCDetectionEnabled));
	RFmxCheckWarn(RFmxGSM_CfgSignalType(instrumentHandle, "slot::all", modulationType, burstType, HBFilterWidth));
	RFmxCheckWarn(RFmxGSM_CfgTSC(instrumentHandle, "slot::all", TSC));
	RFmxCheckWarn(RFmxGSM_CfgPowerControlLevel(instrumentHandle, "slot::all", powerControlLevel));
	RFmxCheckWarn(RFmxGSM_Initiate(instrumentHandle, "", ""));

	RFmxCheckWarn(RFmxGSM_ModAccFetchEVM(instrumentHandle, "", timeout, &meanRMSEVM, &maximumRMSEVM, &meanPeakEVM, &maximumPeakEVM, 
		&ninetyFifthPercentileEVM, &EVMMeanFrequencyError, &peakEVMSymbol));
	RFmxCheckWarn(RFmxGSM_ModAccFetchIQImpairments(instrumentHandle, "", timeout, &meanIQGainImbalance, &maximumIQGainImbalance, 
		&meanIQOriginOffset, &maximumIQOriginOffset));

	RFmxCheckWarn(RFmxGSM_ModAccFetchDetectedTSCArray(instrumentHandle, "", timeout, detectedTSC, NUMBER_OF_TIMESLOTS, NULL));
	
	RFmxCheckWarn(RFmxGSM_ModAccFetchPFER(instrumentHandle, "", timeout, &meanRMSPhaseError, &maximumRMSPhaseError,
		                                  &PFERMeanFrequencyError, 
		&meanPeakPhaseError, &maximumPeakPhaseError, &peakSymbol));

	RFmxCheckWarn(RFmxGSM_ORFSFetchModulationResultsArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, 0, 
		                                                  &modResultsArraySize));
	if( modResultsArraySize > 0 )
	{
		modLowerRelativePower = (float64*)malloc(sizeof(float64) * modResultsArraySize);
		modUpperRelativePower = (float64*)malloc(sizeof(float64) * modResultsArraySize);
		modLowerAbsolutePower = (float64*)malloc(sizeof(float64) * modResultsArraySize);
		modUpperAbsolutePower = (float64*)malloc(sizeof(float64) * modResultsArraySize);

		if( modLowerRelativePower && modUpperRelativePower && modLowerAbsolutePower &&  modUpperAbsolutePower )
		{
			RFmxCheckWarn(RFmxGSM_ORFSFetchModulationResultsArray(instrumentHandle, "", timeout, &modulationCarrierPower, 
				modLowerRelativePower, modUpperRelativePower, modLowerAbsolutePower, modUpperAbsolutePower, modResultsArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxGSM_ORFSFetchSwitchingResultsArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, 0, 
		&switchingResultsArraySize));
	if( switchingResultsArraySize > 0 )
	{
		switchLowerRelativePower = (float64*)malloc(sizeof(float64) * switchingResultsArraySize);
		switchUpperRelativePower = (float64*)malloc(sizeof(float64) * switchingResultsArraySize);
		switchLowerAbsolutePower = (float64*)malloc(sizeof(float64) * switchingResultsArraySize);
		switchUpperAbsolutePower = (float64*)malloc(sizeof(float64) * switchingResultsArraySize);

		if( switchLowerRelativePower && switchUpperRelativePower && switchLowerAbsolutePower &&  switchUpperAbsolutePower )
		{
			RFmxCheckWarn(RFmxGSM_ORFSFetchSwitchingResultsArray(instrumentHandle, "", timeout, &switchingCarrierPower, 
				switchLowerRelativePower, switchUpperRelativePower, switchLowerAbsolutePower, switchUpperAbsolutePower,
				switchingResultsArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}
	RFmxCheckWarn(RFmxGSM_PVTFetchSlotMeasurementArray(instrumentHandle, "", timeout, slotAveragePower, slotBurstWidth, 
				slotMeasurementStatus, slotMaximumPower, slotMinimumPower, slotBurstThreshold, NUMBER_OF_TIMESLOTS, NULL));
		
	RFmxCheckWarn(RFmxGSM_PVTFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));


	printf("-----------------ModAcc Measurements---------------\n\n");
	printf("-----------------EVM Measurement-----------------\n");
	printf("Mean RMS EVM  (%%)                   : %lf\n",meanRMSEVM);
	printf("Maximum RMS EVM  (%%)                : %lf\n",maximumRMSEVM);
	printf("Mean Peak EVM  (%%)                  : %lf\n",meanPeakEVM);
	printf("Maximum Peak EVM  (%%)               : %lf\n",maximumPeakEVM);
	printf("95th Percentile EVM  (%%)            : %lf\n",ninetyFifthPercentileEVM);
	printf("Mean Frequency Error  (Hz)          : %lf\n",EVMMeanFrequencyError);
	printf("Peak EVM Symbol                     : %d\n",peakEVMSymbol);
	
	printf("-----------------PFER Measurement-----------------\n");
	printf("Mean RMS Phase Error  (deg)         : %lf\n",meanRMSPhaseError);
	printf("Maximum RMS Phase Error  (deg)      : %lf\n",maximumRMSPhaseError);
	printf("Mean Peak Phase Error  (deg)        : %lf\n",meanPeakPhaseError);
	printf("Maximum Peak Phase Error  (deg)     : %lf\n",maximumPeakPhaseError);
	printf("Mean Frequency Error  (Hz)          : %lf\n",PFERMeanFrequencyError);
	printf("Peak Symbol                         : %d\n",peakSymbol);

	printf("----------------IQ Impairments-----------------\n");
	printf("Mean IQ Gain Imbalance  (dB)        : %lf\n",meanIQGainImbalance);
	printf("Maximum IQ Gain Imbalance  (dB)     : %lf\n",maximumIQGainImbalance);
	printf("Maximum IQ Origin Offset  (dB)      : %lf\n",maximumIQOriginOffset);
	printf("Mean IQ Origin Offset  (dB)         : %lf\n",meanIQOriginOffset);

	printf("----------------Detected TSC------------------\n");
	for(i = 0; i < NUMBER_OF_TIMESLOTS; i++)
	{
		if(detectedTSC[i] < 0)
			printf("Slot %d                              : Unknown\n", i);
		else									         
			printf("Slot %d                              : TSC%d\n", i, detectedTSC[i]);
	}

	printf("-----------------ORFS Measurements-------------\n\n");
	printf("-----------------Modulation Results------------\n");
	printf("Modulation Carrier Power (dBm)      : %lf\n",modulationCarrierPower);
	for(i = 0; i < modResultsArraySize; i++)
	{
		printf("Offset : %d\n", i);
		printf("Lower Absolute Power (dBm)          : %lf\n", modLowerAbsolutePower[i]);
		printf("Lower Relative Power (dB)           : %lf\n", modLowerRelativePower[i]);
		printf("Upper Absolute Power (dBm)          : %lf\n", modUpperAbsolutePower[i]);
		printf("Upper Relative Power (dB)           : %lf\n\n", modUpperRelativePower[i]);
	}
	printf("-----------------Switching Results-------------\n");
	printf("Switching Carrier Power (dBm)       : %lf\n",switchingCarrierPower);
	for(i = 0; i < switchingResultsArraySize; i++)
	{
		printf("Offset : %d\n", i);
		printf("Lower Absolute Power (dBm)          : %lf\n", switchLowerAbsolutePower[i]);
		printf("Lower Relative Power (dB)           : %lf\n", switchLowerRelativePower[i]);
		printf("Upper Absolute Power (dBm)          : %lf\n", switchUpperAbsolutePower[i]);
		printf("Upper Relative Power (dB)           : %lf\n\n", switchUpperRelativePower[i]);
	}
	
	printf("-----------------PVT Measurements--------------\n\n");
	printf("Measurement Status                  : %s\n",measurementStatus == RFMXGSM_VAL_PVT_MEASUREMENT_STATUS_PASS?"Pass":"Fail");
	for(i=0;i<NUMBER_OF_TIMESLOTS;i++)
	{
		printf("Slot Measurement : %d\n", i);
		printf("Average Power (dBm)                 : %lf\n",slotAveragePower[i]);
		printf("Burst Width (s)                     : %lf\n",slotBurstWidth[i]);
		printf("Maximum Power (dBm)                 : %lf\n",slotMaximumPower[i]);
		printf("Minimum Power (dBm)                 : %lf\n",slotMinimumPower[i]);
		printf("Burst Threshold (dBm)               : %lf\n",slotBurstThreshold[i]);
		printf("Measurement Status                  : %s\n",slotMeasurementStatus[i] == RFMXGSM_VAL_PVT_MEASUREMENT_STATUS_PASS?"Pass":"Fail");
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
	if( instrumentHandle )
	{
		RFmxGSM_Close(instrumentHandle, RFMXGSM_VAL_FALSE);
	}
	if( modLowerAbsolutePower )
		free(modLowerAbsolutePower);
	if( modUpperAbsolutePower )
		free(modUpperAbsolutePower);
	if( modLowerRelativePower )
		free(modLowerRelativePower);
	if( modUpperRelativePower )
		free(modUpperRelativePower);
	if( switchLowerRelativePower )
		free(switchLowerRelativePower);
	if( switchUpperRelativePower )
		free(switchUpperRelativePower);
	if( switchLowerAbsolutePower )
		free(switchLowerAbsolutePower);
	if( switchUpperAbsolutePower )
		free(switchUpperAbsolutePower);
	printf("Press any key to exit\n");
	_getch();

	return error;
}
