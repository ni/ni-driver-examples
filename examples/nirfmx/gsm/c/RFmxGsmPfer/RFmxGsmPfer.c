// Steps:
// 1. Open a new RFmx Session.
// 2. Configure Frequency Reference.
// 3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
// 4. Configure Trigger Parameters for IQ Power Edge Trigger.
// 5. Configure Number of Timeslots.
// 6. Configure Signal Type.
// 7. Configure Auto TSC Detection Enabled.
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

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0;
    int32 lastErrorCode = 0;
	int i = 0;

	char * frequencyReferenceSource = RFMXGSM_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;				/* Hz */

	float64 centerFrequency = 890.2e6;						/* Hz */
	float64 referenceLevel = 0.0;							/* dBm */
	float64 externalAttenuation = 0.0;						/* dB */
   
	float64 triggerDelay = 0.0;								/* seconds */
    int32 enableTrigger = RFMXGSM_VAL_TRUE;
    float64 IQPowerEdgeLevel = -20.0;						/* dB */
    int32 minimumQuietTimeMode = RFMXGSM_VAL_MINIMUM_QUIET_TIME_MODE_AUTO;
    float64 minimumQuietTime = 582e-6;						/* seconds */
   
	int32 autoTSCDetectionEnabled = RFMXGSM_VAL_AUTO_TSC_DETECTION_ENABLED_TRUE;
    int32 TSC = RFMXGSM_VAL_TSC0;
    
	int32 averagingEnabled = RFMXGSM_VAL_MODACC_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;

	float64 maximumIQGainImbalance = 0.0;					/* dB */
    float64 maximumIQOriginOffset = 0.0;					/* dB */
    float64 timeout = 10.0;									/* seconds */
    float64 meanIQGainImbalance = 0.0;						/* dB */
    float64 meanIQOriginOffset = 0.0;						/* dB */
    float64 meanFrequencyError = 0.0;						/* Hz */
    float64 maximumPeakPhaseError = 0.0;					/* deg */
    float64 meanRMSPhaseError = 0.0;						/* deg */
    float64 maximumRMSPhaseError = 0.0;						/* deg */
    float64 meanPeakPhaseError = 0.0;						/* deg */
	float64 x0 = 0;
	float64 dx = 0;
	int32 detectedTSC[NUMBER_OF_TIMESLOTS] = {0};
    int32 peakSymbol = 0;
	int32 actualArraySize = 0;
    float32* meanPhaseError = NULL;

    RFmxCheckWarn(RFmxGSM_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxGSM_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxGSM_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxGSM_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXGSM_VAL_IQ_POWER_EDGE_RISING_SLOPE, 
		                                        IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime, 
												RFMXGSM_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, enableTrigger)); 
    RFmxCheckWarn(RFmxGSM_CfgNumberOfTimeslots(instrumentHandle, "", NUMBER_OF_TIMESLOTS));
	RFmxCheckWarn(RFmxGSM_CfgSignalType(instrumentHandle, "slot::all", RFMXGSM_VAL_MODULATION_TYPE_GMSK, RFMXGSM_VAL_BURST_TYPE_NB, 
		                                RFMXGSM_VAL_HB_FILTER_WIDTH_NARROW));
    RFmxCheckWarn(RFmxGSM_CfgAutoTSCDetectionEnabled(instrumentHandle, "", autoTSCDetectionEnabled));
    RFmxCheckWarn(RFmxGSM_CfgTSC(instrumentHandle, "slot::all", TSC));
    RFmxCheckWarn(RFmxGSM_SelectMeasurements(instrumentHandle, "", RFMXGSM_VAL_MODACC, RFMXGSM_VAL_TRUE));
    RFmxCheckWarn(RFmxGSM_ModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
    RFmxCheckWarn(RFmxGSM_Initiate(instrumentHandle, "", ""));

	RFmxCheckWarn(RFmxGSM_ModAccFetchIQImpairments(instrumentHandle, "", timeout, &meanIQGainImbalance, 
													&maximumIQGainImbalance, &meanIQOriginOffset ,&maximumIQOriginOffset ));
    RFmxCheckWarn(RFmxGSM_ModAccFetchPFER(instrumentHandle, "", timeout, &meanRMSPhaseError, &maximumRMSPhaseError, 
											&meanPeakPhaseError, &maximumPeakPhaseError, &meanFrequencyError, &peakSymbol));

	RFmxCheckWarn(RFmxGSM_ModAccFetchDetectedTSCArray(instrumentHandle, "", timeout, detectedTSC , NUMBER_OF_TIMESLOTS, NULL));
		
	RFmxCheckWarn(RFmxGSM_ModAccFetchPhaseErrorTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		meanPhaseError = (float32*)malloc(sizeof(float32) * actualArraySize);
		
		if( meanPhaseError )
		{
			RFmxCheckWarn(RFmxGSM_ModAccFetchPhaseErrorTrace(instrumentHandle, "", timeout, &x0, &dx, meanPhaseError,
				                                             actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}

	}
    
	printf("---------Measurement---------\n");
	printf("Mean RMS Phase Error (deg)      : %lf\n",meanRMSPhaseError);
	printf("Maximum RMS Phase Error (deg)   : %lf\n",maximumRMSPhaseError);
	printf("Mean Peak Phase Error (deg)     : %lf\n",meanPeakPhaseError);
	printf("Maximum Peak Phase Error (deg)  : %lf\n",maximumPeakPhaseError);
	printf("Mean Frequency Error (Hz)       : %lf\n",meanFrequencyError);
	printf("Peak Symbol                     : %d\n",peakSymbol);
	printf("\n---------IQ Impairments---------\n");
    printf("Maximum IQ Gain Imbalance (dB)  : %lf\n",maximumIQGainImbalance);
    printf("Maximum IQ Origin Offset (dB)   : %lf\n",maximumIQOriginOffset);
    printf("Mean IQ Gain Imbalance (dB)     : %lf\n",meanIQGainImbalance);
    printf("Mean IQ Origin Offset (dB)      : %lf\n",meanIQOriginOffset);

	printf("\n---------Detected TSC-----------\n");
	for(i = 0; i< NUMBER_OF_TIMESLOTS; i++)
	{
		if(detectedTSC[i] < 0)
			printf("Slot %d                          : Unknown\n", i);
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
	
	if( meanPhaseError )
		free(meanPhaseError);
    printf("Press any key to exit\n");
    _getch();

    return error;
}
