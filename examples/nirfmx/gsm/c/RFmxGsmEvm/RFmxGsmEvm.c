// Steps:
// 1. Open a new RFmx Session.
// 2. Configure Frequency Reference.
// 3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
// 4. Configure Trigger Parameters for IQ Power Edge Trigger.
// 5. Configure Number of Timeslots.
// 6. Configure Auto TSC Detection Enabled.
// 7. Configure Signal Type.
// 8. Configure TSC.
// 9. Select ModAcc measurement and enable Traces.
// 10. Configure Averaging Parameters for ModAcc measurement.
// 11. Initiate the Measurement.
// 12. Fetch ModAcc Measurements and Traces.
// 13. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxGSM.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

#define NUMBER_OF_TIMESLOTS 1

int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;

	char* frequencyReferenceSource = RFMXGSM_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;

	float64 centerFrequency = 890.2e6;				/* Hz */
	float64 referenceLevel = 0.0;					/* dBm */
	float64 externalAttenuation = 0.0;				/* dB */

	float64 IQPowerEdgeLevel = -20.00;				/* dB */
	float64 triggerDelay = 0.0;						/* seconds */
	int32 minimumQuietTimeMode = RFMXGSM_VAL_MINIMUM_QUIET_TIME_MODE_AUTO;
	float64 minimumQuietTime = 582e-6;				/* seconds */
	int32 enableTrigger = RFMXGSM_VAL_TRUE;

	int32 autoTSCDetectionEnabled = RFMXGSM_VAL_AUTO_TSC_DETECTION_ENABLED_TRUE;

	int32 modulationType = RFMXGSM_VAL_MODULATION_TYPE_8PSK;
	int32 burstType = RFMXGSM_VAL_BURST_TYPE_NB;
	int32 HBFilterWidth = RFMXGSM_VAL_HB_FILTER_WIDTH_NARROW;

	int32 TSC = RFMXGSM_VAL_TSC0;

	int32 averagingEnabled = RFMXGSM_VAL_MODACC_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;

	float64 timeout = 10.0;

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0;
	int32 lastErrorCode = 0;

	float64 meanRMSEVM = 0.0;
	float64 maximumPeakEVM = 0.0;
	float64 maximumRMSEVM = 0.0;
	float64 meanPeakEVM = 0.0;
	float64 ninetyFifthPercentileEVM = 0.0;
	int32 peakEVMSymbol = 0;
	float64 meanFrequencyError = 0.0;
	float64 meanIQGainImbalance = 0.0;
	float64 maximumIQGainImbalance = 0.0;
	float64 meanIQOriginOffset = 0.0;
	float64 maximumIQOriginOffset = 0.0;
	float64 x0 = 0.0;
	float64 dx = 0.0;
	int32 detectedTSC[NUMBER_OF_TIMESLOTS] = {0};
	float32 *EVM = NULL;
	NIComplexSingle* constellationTrace = NULL;
	int32 actualArraySize = 0;
	int32 i = 0;

	RFmxCheckWarn(RFmxGSM_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxGSM_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxGSM_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxGSM_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXGSM_VAL_IQ_POWER_EDGE_RISING_SLOPE, IQPowerEdgeLevel, 
		triggerDelay, minimumQuietTimeMode, minimumQuietTime, RFMXGSM_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, enableTrigger));
	RFmxCheckWarn(RFmxGSM_CfgNumberOfTimeslots(instrumentHandle, "", NUMBER_OF_TIMESLOTS));
	RFmxCheckWarn(RFmxGSM_CfgAutoTSCDetectionEnabled(instrumentHandle, "", autoTSCDetectionEnabled));
	RFmxCheckWarn(RFmxGSM_CfgSignalType(instrumentHandle, "slot::all", modulationType, burstType, HBFilterWidth));
	RFmxCheckWarn(RFmxGSM_CfgTSC(instrumentHandle, "slot::all", TSC));
	RFmxCheckWarn(RFmxGSM_SelectMeasurements(instrumentHandle, "", RFMXGSM_VAL_MODACC, RFMXGSM_VAL_TRUE));
	RFmxCheckWarn(RFmxGSM_ModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));

	RFmxCheckWarn(RFmxGSM_Initiate(instrumentHandle, "", ""));

	RFmxCheckWarn(RFmxGSM_ModAccFetchEVM(instrumentHandle, "", timeout, &meanRMSEVM, &maximumRMSEVM, &meanPeakEVM, &maximumPeakEVM, 
		&ninetyFifthPercentileEVM,	&meanFrequencyError, &peakEVMSymbol));
	RFmxCheckWarn(RFmxGSM_ModAccFetchIQImpairments(instrumentHandle, "", timeout, &meanIQGainImbalance, &maximumIQGainImbalance, 
		&meanIQOriginOffset, &maximumIQOriginOffset));

	RFmxCheckWarn(RFmxGSM_ModAccFetchDetectedTSCArray(instrumentHandle, "", timeout, detectedTSC, NUMBER_OF_TIMESLOTS, NULL));

	actualArraySize = 0;
	RFmxCheckWarn(RFmxGSM_ModAccFetchEVMTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		EVM = (float32 *) malloc(sizeof(float32) * actualArraySize);
		if( EVM )
		{
			RFmxCheckWarn(RFmxGSM_ModAccFetchEVMTrace(instrumentHandle, "", timeout, &x0, &dx, EVM, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxGSM_ModAccFetchConstellationTrace(instrumentHandle,"",timeout,NULL,0,&actualArraySize));
	
	if( actualArraySize > 0 )
	{
		constellationTrace = (NIComplexSingle *) malloc(sizeof(NIComplexSingle) * actualArraySize);
		if( constellationTrace )
		{
			RFmxCheckWarn(RFmxGSM_ModAccFetchConstellationTrace(instrumentHandle, "", timeout, constellationTrace, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	printf("-----------------Measurement-----------------\n");
	printf("Mean RMS EVM (%%)                : %lf\n",meanRMSEVM);
	printf("Maximum RMS EVM (%%)             : %lf\n",maximumRMSEVM);
	printf("Mean Peak EVM (%%)               : %lf\n",meanPeakEVM);
	printf("Maximum Peak EVM (%%)            : %lf\n",maximumPeakEVM);
	printf("95th Percentile EVM (%%)         : %lf\n",ninetyFifthPercentileEVM);
	printf("Mean Frequency Error (Hz)       : %lf\n",meanFrequencyError);
	printf("Peak EVM Symbol                 : %d\n",peakEVMSymbol);


	printf("\n----------------IQ Impairments-----------------\n");
	printf("Mean IQ Origin Offset (dB)      : %lf\n",meanIQOriginOffset);
	printf("Maximum IQ Origin Offset (dB)   : %lf\n",maximumIQOriginOffset);
	printf("Mean IQ Gain Imbalance (dB)     : %lf\n",meanIQGainImbalance);
	printf("Maximum IQ Gain Imbalance (dB)  : %lf\n",maximumIQGainImbalance);

	printf("\n----------------Detected TSC-------------------\n");
	for(i = 0; i < NUMBER_OF_TIMESLOTS; i++)
	{
		if(detectedTSC[i] < 0)
			printf("Slot %d				             : Unknown\n", i);
		else
			printf("Slot %d                          : TSC%d\n", i, detectedTSC[i]); 
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
	if( EVM )
		free(EVM);
	printf("\nPress any key to exit\n");
	_getch();

	return error;
}
