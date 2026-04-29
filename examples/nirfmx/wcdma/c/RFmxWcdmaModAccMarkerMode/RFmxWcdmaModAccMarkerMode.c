// Steps:
// 1. Open a new RFmx Session.
// 2. Configure Frequency Reference.
// 3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
// 4. Configure Trigger Type and Trigger Parameters.
// 5. Configure Uplink Scrambling.
// 6. Configure Channel Configuration Mode.
// 7. No VIs to Configure for Channels, Configure User Defined Channels and Configure Test Model.
// 8. Select ModAcc measurement and enable Traces.
// 9. Configure Synchronization Mode and Measurement Interval.
// 10. Initiate Measurement & Fetch Reference Waveform 
// 11. Configure Reference Waveform.
// 12. Initiate the Measurement. 
// 13. Fetch ModAcc Measurements and Traces.
// 14. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

#define NUMBER_OF_USER_DEFINED_CHANNELS		2

int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;	

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0, lastErrorCode = 0;

	char * frequencyReferenceSource = RFMXWCDMA_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency  = 10e6;			/*(Hz) */

	float64 centerFrequency = 1.95e9;	/*(Hz) */
	float64 referenceLevel = 0.000000;				/*(dBm) */
	float64 externalAttenuation = 0.000000;			/*(dB) */

	int32 enableTrigger = RFMXWCDMA_VAL_TRUE;
	char * digitalEdgeSource = RFMXWCDMA_VAL_PFI0_STR;
	int32 digitalEdge = RFMXWCDMA_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.000000;				/*(s) */

	int32 measurementOffset = 0;					/*(slots) */
	int32 measurementLength = 1;					/*(slots) */

	int32 uplinkScramblingType = RFMXWCDMA_VAL_UPLINK_SCRAMBLING_TYPE_LONG;
	int32 uplinkScramblingCode = 0x0;

	int32 channelConfigurationMode = RFMXWCDMA_VAL_CHANNEL_CONFIGURATION_MODE_TEST_MODEL;
	int32 ulTestModel = RFMXWCDMA_VAL_UPLINK_TEST_MODEL_R6C_2_1;

	int32 numberOfChannels = NUMBER_OF_USER_DEFINED_CHANNELS;
	int32 spreadingFactor[NUMBER_OF_USER_DEFINED_CHANNELS] = {256,64};
	int32 spreadingCode[NUMBER_OF_USER_DEFINED_CHANNELS] = {0,16};
	int32 modulationType[NUMBER_OF_USER_DEFINED_CHANNELS] = {RFMXWCDMA_VAL_MODULATION_TYPE_BPSK_QPSK,RFMXWCDMA_VAL_MODULATION_TYPE_BPSK_QPSK};  
	int32 branch[NUMBER_OF_USER_DEFINED_CHANNELS] = {RFMXWCDMA_VAL_BRANCH_Q,RFMXWCDMA_VAL_BRANCH_I}; 	

	float64 timeout = 10.000000;					/*(s) */

	float64 x0 = 0.0, dx = 0.0;
	NIComplexSingle* referenceWaveform = NULL;
	int32 actualArraySize = 0;

	float64 chipRateError = 0.000000;				/*(ppm) */
	float64 frequencyError = 0.000000;				/*(Hz) */
	float64 rmsEVM = 0.000000;						/*(%) */
	float64 peakEVM = 0.000000;						/*(%) */
	float64 rho = 0.000000;
	float64 rmsPhaseError = 0.000000;				/*(deg) */
	float64 rmsMagnitudeError = 0.000000;			/*(%) */

	float64 IQOriginOffset = 0.000000;				/*(dB) */
	float64 IQGainImbalance = 0.000000;				/*(dB) */
	float64 IQQuadratureError = 0.000000;			/*(deg) */

	int32 peakCDEBranch = 0;
	float64 peakCDE = 0.000000;						/*(dB) */
	int32 peakCDECode = 0;

	int32 peakActiveCDEBranch = 0;
	float64 peakActiveCDE = 0.000000;				/*(dB) */
	int32 peakActiveCDESpreadingFactor = 0;
	int32 peakActiveCDECode = 0;

	int32 peakRCDEBranch = 0;
	float64 peakRCDE = 0.000000;					/*(dB) */
	int32 peakRCDESpreadingFactor = 0;
	int32 peakRCDECode = 0;

	float32* evm = NULL;							/*(%) */
	NIComplexSingle* constellation = NULL;


	/* Initialize a session */
	RFmxCheckWarn(RFmxWCDMA_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxWCDMA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxWCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxWCDMA_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxWCDMA_CfgUplinkScrambling(instrumentHandle, "", uplinkScramblingCode, uplinkScramblingType));
	RFmxCheckWarn(RFmxWCDMA_CfgChannelConfigurationMode(instrumentHandle, "", channelConfigurationMode));
	if( channelConfigurationMode == RFMXWCDMA_VAL_CHANNEL_CONFIGURATION_MODE_TEST_MODEL )
	{	
		RFmxCheckWarn(RFmxWCDMA_CfgUplinkTestModel(instrumentHandle, "", ulTestModel));
	} 
	else if( channelConfigurationMode == RFMXWCDMA_VAL_CHANNEL_CONFIGURATION_MODE_USER_DEFINED ) 
	{
		RFmxCheckWarn(RFmxWCDMA_CfgNumberOfChannels(instrumentHandle, "", numberOfChannels));
		RFmxCheckWarn(RFmxWCDMA_CfgUserDefinedChannelArray(instrumentHandle, "", spreadingFactor, spreadingCode, modulationType, 
			branch, numberOfChannels));
	}
	RFmxCheckWarn(RFmxWCDMA_SelectMeasurements(instrumentHandle, "", RFMXWCDMA_VAL_MODACC, RFMXWCDMA_VAL_TRUE));
	RFmxCheckWarn(RFmxWCDMA_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", RFMXWCDMA_VAL_MODACC_SYNCHRONIZATION_MODE_MARKER, 
		measurementOffset, measurementLength));
	if( referenceWaveform == NULL ) 
	{
		RFmxCheckWarn(RFmxWCDMA_Initiate(instrumentHandle, "", ""));
		RFmxCheckWarn(RFmxWCDMA_ModAccFetchReferenceWaveform(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
		if( actualArraySize > 0 )
		{
			referenceWaveform = (NIComplexSingle *) malloc(sizeof(NIComplexSingle) * actualArraySize);
			if( referenceWaveform )
			{
				RFmxCheckWarn(RFmxWCDMA_ModAccFetchReferenceWaveform(instrumentHandle, "", timeout, &x0, &dx, referenceWaveform, 
					actualArraySize, NULL));
			}
			else
			{
				printf("malloc failed.\n");
				goto Error;
			}
		}
	}
	RFmxCheckWarn(RFmxWCDMA_ModAccCfgReferenceWaveform(instrumentHandle, "", x0, dx, referenceWaveform, actualArraySize));
	RFmxCheckWarn(RFmxWCDMA_Initiate(instrumentHandle, "", ""));

	RFmxCheckWarn(RFmxWCDMA_ModAccFetchEVM(instrumentHandle, "", timeout, &rmsEVM, &peakEVM, &rho, &frequencyError, &chipRateError, 
		&rmsMagnitudeError, &rmsPhaseError));
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchIQImpairments(instrumentHandle, "", timeout, &IQOriginOffset, &IQGainImbalance, &IQQuadratureError));
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchPeakCDE(instrumentHandle, "", timeout, &peakCDE, &peakCDECode, &peakCDEBranch));
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchPeakActiveCDE(instrumentHandle, "", timeout, &peakActiveCDE, &peakActiveCDESpreadingFactor, 
		&peakActiveCDECode, &peakActiveCDEBranch));
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchRCDE(instrumentHandle, "", timeout, &peakRCDE, &peakRCDESpreadingFactor, 
		&peakRCDECode, &peakRCDEBranch));

	actualArraySize = 0;
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchEVMTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		evm = (float32 *) malloc(sizeof(float32) * actualArraySize);
		if( evm )
		{
			RFmxCheckWarn(RFmxWCDMA_ModAccFetchEVMTrace(instrumentHandle, "", timeout, &x0, &dx, evm, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchConstellationTrace(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		constellation = (NIComplexSingle *) malloc(sizeof(NIComplexSingle) * actualArraySize);
		if( constellation )
		{
			RFmxCheckWarn(RFmxWCDMA_ModAccFetchConstellationTrace(instrumentHandle, "", timeout, constellation, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	printf("------------EVM------------\n");
	printf("RMS EVM (%%)                     : %lf\n",rmsEVM);
	printf("Peak EVM (%%)                    : %lf\n",peakEVM);
	printf("Rho                             : %lf\n",rho);
	printf("Frequency Error (Hz)            : %lf\n",frequencyError);
	printf("Chip Rate Error (ppm)           : %lf\n",chipRateError);
	printf("RMS Magnitude Error (%%)         : %lf\n",rmsMagnitudeError);
	printf("RMS Phase Error (deg)           : %lf\n",rmsPhaseError);

	printf("\nIQ Impairments :\n");
	printf("I/Q Origin Offset (dB)          : %lf\n",IQOriginOffset);
	printf("I/Q Gain Imbalance (dB)         : %lf\n",IQGainImbalance);
	printf("I/Q Quadrature Error (deg)      : %lf\n",IQQuadratureError);

	printf("\n------------Code Domain Error------------\n");
	printf("Peak CDE (dB)                   : %lf\n",peakCDE);
	printf("Peak CDE Code                   : %d\n",peakCDECode);
	printf("Peak CDE Branch                 : %s\n",(peakCDEBranch==RFMXWCDMA_VAL_MODACC_PEAK_CDE_BRANCH_I)?"I":"Q");
	printf("Peak Active CDE (dB)            : %lf\n",peakActiveCDE);
	printf("Peak Active CDE Code            : %d\n",peakActiveCDECode);
	printf("Peak Active CDE Spreading Factor: %d\n",peakActiveCDESpreadingFactor);
	printf("Peak Active CDE Branch          : %d\n",peakActiveCDEBranch);
	printf("Peak RCDE (dB)                  : %lf\n",peakRCDE);
	printf("Peak RCDE Code                  : %d\n",peakRCDECode);
	printf("Peak RCDE Spreading Factor      : %d\n",peakRCDESpreadingFactor);
	printf("Peak RCDE Branch                : %s\n",(peakRCDEBranch==RFMXWCDMA_VAL_MODACC_PEAK_RCDE_BRANCH_I)?"I":"Q");

Error:
	if( error )
	{
		RFmxWCDMA_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if (error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}
	if(instrumentHandle)
	{
		RFmxWCDMA_Close(instrumentHandle, RFMXWCDMA_VAL_FALSE);
	}
	printf("Press any key to exit\n");
	_getch();

	/* Free allocated memory */
    if (referenceWaveform)
    {
        free(referenceWaveform);
    }
    if (evm)
    {
        free(evm);
    }
    if (constellation)
    {
        free(constellation);
    }

	return error;
}
