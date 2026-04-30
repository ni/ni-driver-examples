//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Downlink Scrambling.
//6. Select ModAcc measurement and enable Traces.
//7. Configure Synchronization Mode and Measurement Interval.
//8. Initiate the Measurement.
//9. Fetch ModAcc Measurements and Traces.
//10. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0, lastErrorCode = 0;

	float64 centerFrequency = 1.95e9;							/*(Hz) */
	float64 referenceLevel = 0.000000;							/*(dBm) */
	float64 externalAttenuation = 0.000000;						/*(dB) */

	char * frequencyReferenceSource = RFMXWCDMA_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;					/*(Hz) */

	int32 enableTrigger = RFMXWCDMA_VAL_FALSE;
	char * digitalEdgeTriggerSource = RFMXWCDMA_VAL_PFI0_STR;
	int32 digitalEdgeTriggerEdge = RFMXWCDMA_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.000000;							/*(s) */

	int32 synchronizationMode = RFMXWCDMA_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;
	int32 measurementOffset = 0;								/*(slots) */
	int32 measurementLength = 1;								/*(slots) */

	int32 downlinkScramblingType = RFMXWCDMA_VAL_DOWNLINK_SCRAMBLING_TYPE_STANDARD;
	int32 downlinkScramblingPrimaryCode = 0;
	int32 downlinkScramblingSecondaryCode = 0;

	float64 timeout = 10.000000;								/*(s) */

	float64 x0 = 0.0, dx = 0.0;
	int32 actualArraySize = 0;

	float64 chipRateError = 0.000000;							/*(ppm) */
	float64 frequencyError = 0.000000;							/*(Hz) */
	float64 RMSEVM = 0.000000;									/*(%) */
	float64 peakEVM = 0.000000;									/*(%) */
	float64 rho = 0.000000;
	float64 RMSPhaseError = 0.000000;							/*(deg) */
	float64 RMSMagnitudeError = 0.000000;						/*(%) */

	float64 IQOriginOffset = 0.000000;							/*(dB) */
	float64 IQGainImbalance = 0.000000;							/*(dB) */
	float64 IQQuadratureError = 0.000000;						/*(deg) */

	int32 peakCDEBranch = RFMXWCDMA_VAL_MODACC_PEAK_CDE_BRANCH_I;
	float64 peakCDE = 0.000000;									/*(dB) */
	int32 peakCDECode = 0;

	int32 peakActiveCDEBranch = RFMXWCDMA_VAL_MODACC_PEAK_ACTIVE_CDE_BRANCH_I;
	float64 peakActiveCDE = 0.000000;							/*(dB) */
	int32 peakActiveCDESpreadingFactor = 0;
	int32 peakActiveCDECode = 0;

	int32 peakRCDEBranch =  RFMXWCDMA_VAL_MODACC_PEAK_RCDE_BRANCH_I;
	float64 peakRCDE = 0.000000;								/*(dB) */
	int32 peakRCDESpreadingFactor = 0;
	int32 peakRCDECode = 0;

	float32*  evm = NULL;										/*(%) */
	NIComplexSingle* constellation = NULL;
	int32*  detectedSpreadingFactor = NULL;
	int32*  detectedSpreadingCode = NULL;
	int32*  detectedModulationType = NULL;
	int32*  detectedBranch = NULL;
	int i = 0;

	/* Initialize a session */
	RFmxCheckWarn(RFmxWCDMA_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxWCDMA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, 
		frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxWCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxWCDMA_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeTriggerSource, 
		digitalEdgeTriggerEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxWCDMA_SetLinkDirection(instrumentHandle, "", RFMXWCDMA_VAL_LINK_DIRECTION_DOWNLINK));
	RFmxCheckWarn(RFmxWCDMA_SetDownlinkScramblingType(instrumentHandle, "", downlinkScramblingType));
	RFmxCheckWarn(RFmxWCDMA_SetDownlinkScramblingPrimaryCode(instrumentHandle, "", downlinkScramblingPrimaryCode));
	RFmxCheckWarn(RFmxWCDMA_SetDownlinkScramblingSecondaryCode(instrumentHandle, "", downlinkScramblingSecondaryCode));
	RFmxCheckWarn(RFmxWCDMA_SelectMeasurements(instrumentHandle, "", RFMXWCDMA_VAL_MODACC, RFMXWCDMA_VAL_TRUE));
	RFmxCheckWarn(RFmxWCDMA_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode, 
		measurementOffset, measurementLength));
	RFmxCheckWarn(RFmxWCDMA_Initiate(instrumentHandle, "", ""));

	RFmxCheckWarn(RFmxWCDMA_ModAccFetchEVM(instrumentHandle, "", timeout, &RMSEVM, &peakEVM, &rho, &frequencyError, 
		&chipRateError, &RMSMagnitudeError, &RMSPhaseError));
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchIQImpairments(instrumentHandle, "", timeout, &IQOriginOffset, &IQGainImbalance, 
		&IQQuadratureError));
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchPeakCDE(instrumentHandle, "", timeout, &peakCDE, &peakCDECode, &peakCDEBranch));
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchPeakActiveCDE(instrumentHandle, "", timeout, &peakActiveCDE, 
		&peakActiveCDESpreadingFactor, &peakActiveCDECode, &peakActiveCDEBranch));
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchRCDE(instrumentHandle, "", timeout, &peakRCDE, &peakRCDESpreadingFactor, 
		&peakRCDECode, &peakRCDEBranch));
	actualArraySize = 0;
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchEVMTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		evm = (float32 *) malloc(sizeof(float32) * actualArraySize);
		if( evm )
		{
			RFmxCheckWarn(RFmxWCDMA_ModAccFetchEVMTrace(instrumentHandle, "", timeout, &x0, &dx, evm, 
				actualArraySize, NULL));
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
			RFmxCheckWarn(RFmxWCDMA_ModAccFetchConstellationTrace(instrumentHandle, "", timeout, constellation, 
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchDetectedChannelArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, 
		0, &actualArraySize));
	if (actualArraySize > 0)
	{
		detectedSpreadingFactor = (int32 *) malloc(sizeof(int) * actualArraySize);
		detectedSpreadingCode = (int32 *) malloc(sizeof(int) * actualArraySize);
		detectedModulationType = (int32 *) malloc(sizeof(int32) * actualArraySize);
		detectedBranch = (int32 *) malloc(sizeof(int32) * actualArraySize);

		if (detectedSpreadingFactor && detectedSpreadingCode && detectedModulationType && detectedBranch)
		{

			RFmxCheckWarn(RFmxWCDMA_ModAccFetchDetectedChannelArray(instrumentHandle, "", timeout, detectedSpreadingFactor, 
				detectedSpreadingCode, detectedModulationType, detectedBranch, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}
	

	printf("----------------------------EVM---------------------------\n");
	printf("RMS EVM (%%)			        : %lf\n",RMSEVM);
	printf("Peak EVM (%%)			        : %lf\n",peakEVM);
	printf("Rho	                                : %lf\n",rho);
	printf("Frequency Error (Hz)                    : %lf\n",frequencyError);
	printf("Chip Rate Error (ppm)                   : %lf\n",chipRateError);
	printf("RMS Magnitude Error (%%)                 : %lf\n",RMSMagnitudeError);
	printf("RMS Phase Error (deg)                   : %lf\n",RMSPhaseError);

	printf("\n----------------------IQ Impairments----------------------\n");
	printf("I/Q Origin Offset (dB)			: %lf\n",IQOriginOffset);
	printf("I/Q Gain Imbalance (dB)			: %lf\n",IQGainImbalance);
	printf("I/Q Quadrature Error (deg)		: %lf\n",IQQuadratureError);

	printf("\n---------------------Code Domain Error--------------------\n");
	printf("Peak CDE (dB)                           : %lf\n",peakCDE);
	printf("Peak CDE Code                           : %d\n",peakCDECode);
	switch (peakCDEBranch)
	{
	case 0: printf("Peak CDE Branch                         : I\n");
		break;
	case 1: printf("Peak CDE Branch                         : Q\n");
		break;
	case 2: printf("Peak CDE Branch                         : I and Q\n");
		break;
	}
	printf("Peak Active CDE (dB)                    : %lf\n",peakActiveCDE);
	printf("Peak Active CDE Code                    : %d\n",peakActiveCDECode);
	printf("Peak Active CDE Spreading Factor        : %d\n",peakActiveCDESpreadingFactor);
	switch (peakActiveCDEBranch)
	{
	case 0: printf("Peak Active CDE Branch                  : I\n");
		break;
	case 1: printf("Peak Active CDE Branch                  : Q\n");
		break;
	case 2: printf("Peak Active CDE Branch                  : I and Q\n");
		break;
	}
	printf("Peak RCDE (dB)                          : %lf\n",peakRCDE);
	printf("Peak RCDE Code                          : %d\n",peakRCDECode);
	printf("Peak RCDE Spreading Factor              : %d\n",peakRCDESpreadingFactor);
	switch (peakRCDEBranch)
	{
	case 0: printf("Peak RCDE Branch                        : I\n");
		break;
	case 1: printf("Peak RCDE Branch                        : Q\n");
		break;
	case 2: printf("Peak RCDE Branch                        : I and Q\n");
		break;
	}

	printf("\n---------------------Detected Channels--------------------\n");
	for (i = 0; i < actualArraySize; i++)
	{
		printf("Idx                                     : %d\n", i);
		printf("SF                                      : %d\n", detectedSpreadingFactor[i]);
		printf("Code                                    : %d\n", detectedSpreadingCode[i]);
		switch (detectedModulationType[i])
		{
		case 0: printf("Modulation                              : BPSK/QPSK\n");
			break;
		case 1: printf("Modulation                              : 4PAM/16QAM\n");
			break;
		case 2: printf("Modulation                              : 64QAM\n");
			break;
		}
		switch (detectedBranch[i])
		{
		case 0: printf("Branch                                  : I\n");
			break;
		case 1: printf("Branch                                  : Q\n");
			break;
		case 2: printf("Branch                                  : I and Q\n");
			break;
		}
		
		printf("\n");
	}

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

	/* Free allocated memory */
    if (evm)
    {
        free(evm);
    }
    if (constellation)
    {
        free(constellation);
    }
    if (detectedSpreadingFactor)
    {
        free(detectedSpreadingFactor);
    }
    if (detectedSpreadingCode)
    {
        free(detectedSpreadingCode);
    }
    if (detectedModulationType)
    {
        free(detectedModulationType);
    }
    if (detectedBranch)
    {
        free(detectedBranch);
    }

	printf("Press any key to exit\n");
	_getch();

	return error;
}
