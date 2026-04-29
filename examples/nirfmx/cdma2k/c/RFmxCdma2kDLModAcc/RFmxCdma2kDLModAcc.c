//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Radio Configuration Parameter.
//6. Configure DownLink PN Offset.
//7. Select ModAcc measurement and enable traces. 
//8. Configure Synchronization Mode and Measurement Interval.
//9. Configure Multi Carrier filter.
//10. Initiate the Measurement.
//11. Fetch ModAcc Measurement and Traces.
//12. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxCDMA2k.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char *argv[])
{
	niRFmxInstrHandle instrumentHandle = NULL;
	int32 error = 0,  lastErrorCode = 0;
	char errorMessage[MAX_ERROR_DESCRIPTION] = {'\0'};

	char *resourceName = "RFSA";	
	float64 centerFrequency = 833.490e+6;			/* Hz */
	float64 referenceLevel = 0.00;					/* dBm */
	float64 externalAttenuation = 0.00;				/* dB */

	/* Frequency Reference */
	float64 frequency = 10.0e+6;					/* Hz */
	char *frequencySource = RFMXCDMA2K_VAL_ONBOARD_CLOCK_STR;	

	/* Trigger */
	float64 triggerDelay = 0.00;                    /* s */
	int32 digitalTriggerEnabled = RFMXCDMA2K_VAL_FALSE;
	char* digitalEdgeSource = RFMXCDMA2K_VAL_PFI0_STR;
	int32 digitalEdge = RFMXCDMA2K_VAL_DIGITAL_EDGE_RISING_EDGE;

	int32 radioConfiguration = RFMXCDMA2K_VAL_RADIO_CONFIGURATION_RC3;
	int32 DLSpreadingPNOffset = 0;                  /* 64 chips */
	int32 multiCarrierFilterEnabled = RFMXCDMA2K_VAL_MODACC_MULTI_CARRIER_FILTER_ENABLED_FALSE;

	/* Measurement Settings */
	int32 synchronizationMode = RFMXCDMA2K_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;
	int32 measurementOffset = 0;					/* slots */
	int32 measurementLength = 1;					/* slots */

	/* Variables to store the measurement results */

	/* Store EVM Results*/
	float64 RMSEVM = 0.0;							/* % */
	float64 peakEVM = 0.0;							/* % */
	float64 rho = 0.0;
	float64 frequencyError = 0.0;					/* Hz */
	float64 chipRateError = 0.0;					/* ppm */
	float64 RMSMagnitudeError = 0.0;				/* % */
	float64 RMSPhaseError = 0.0;					/* deg */

	/*IQ Impairments*/
	float64 IQOriginOffset = 0.0;					/* dB */
	float64 IQGainImbalance = 0.0;                  /* dB */
    float64 IQQuadratureError = 0.0;                /* deg */

	/* Code Domain Error*/
	float64 peakCDE = 0.0;							/* dB */
	int32 peakCDEWalshCodeNumber = 0;				
	float64 peakActiveCDE = 0.0;                    /* dB */
	int32 peakCDEBranch = RFMXCDMA2K_VAL_MODACC_PEAK_CDE_BRANCH_I;	
	int32 peakActiveCDEWalshCodeLength = 0;
	int32 peakActiveCDEWalshCodeNumber = 0;
	int32 peakActiveCDEBranch = RFMXCDMA2K_VAL_MODACC_PEAK_ACTIVE_CDE_BRANCH_I;

	/* variables to store traces */
	float64 x0 = 0.0, dx = 0.0;	
	float32 *EVMTrace = (float32 *)NULL;
	int32 actualArraySize = 0;
	NIComplexSingle* constellation = NULL;
	int32*  detectedWalshCodeLength = NULL;
	int32*  detectedWalshCodeNumber = NULL;
	int32*  detectedBranch = NULL;
	int i = 0;

	float64 timeout = 10.0;							/* seconds */	

	/* Create new RFmx session */
	RFmxCheckWarn(RFmxCDMA2k_Initialize (resourceName, "", &instrumentHandle, NULL ));		
	RFmxCheckWarn(RFmxCDMA2k_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency ));
	RFmxCheckWarn(RFmxCDMA2k_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation ));
	RFmxCheckWarn(RFmxCDMA2k_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, digitalTriggerEnabled ));
	RFmxCheckWarn(RFmxCDMA2k_CfgRadioConfiguration(instrumentHandle, "", radioConfiguration));
	RFmxCheckWarn(RFmxCDMA2k_SetLinkDirection(instrumentHandle, "", RFMXCDMA2K_VAL_LINK_DIRECTION_DOWNLINK));
	RFmxCheckWarn(RFmxCDMA2k_SetDownlinkSpreadingPNOffset(instrumentHandle, "", DLSpreadingPNOffset));
	RFmxCheckWarn(RFmxCDMA2k_SelectMeasurements(instrumentHandle, "", RFMXCDMA2K_VAL_MODACC, RFMXCDMA2K_VAL_TRUE));
	RFmxCheckWarn(RFmxCDMA2k_ModAccSetMultiCarrierFilterEnabled(instrumentHandle, "", multiCarrierFilterEnabled));
	RFmxCheckWarn(RFmxCDMA2k_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode, measurementOffset, measurementLength ));
	RFmxCheckWarn(RFmxCDMA2k_Initiate(instrumentHandle, "", ""));	

	/* Retrieve results */	

	RFmxCheckWarn(RFmxCDMA2k_ModAccFetchEVM(instrumentHandle, "", timeout, &RMSEVM, &peakEVM, &rho, &frequencyError, &chipRateError, &RMSMagnitudeError, &RMSPhaseError ));
	RFmxCheckWarn(RFmxCDMA2k_ModAccFetchIQImpairments(instrumentHandle, "", timeout, &IQOriginOffset, &IQGainImbalance, &IQQuadratureError));	
	RFmxCheckWarn(RFmxCDMA2k_ModAccFetchPeakCDE(instrumentHandle, "", timeout, &peakCDE, &peakCDEWalshCodeNumber, &peakCDEBranch ));	
	RFmxCheckWarn(RFmxCDMA2k_ModAccFetchPeakActiveCDE(instrumentHandle, "", timeout, &peakActiveCDE, &peakActiveCDEWalshCodeLength, &peakActiveCDEWalshCodeNumber, &peakActiveCDEBranch ));

	RFmxCheckWarn(RFmxCDMA2k_ModAccFetchConstellationTrace(instrumentHandle, "", timeout, NULL, 0, &actualArraySize ));
	if(actualArraySize > 0)
	{
		constellation = (NIComplexSingle *) malloc(actualArraySize * sizeof(NIComplexSingle));
		if(constellation)
		{			
			RFmxCheckWarn(RFmxCDMA2k_ModAccFetchConstellationTrace(instrumentHandle, "", timeout, constellation, actualArraySize, NULL ));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxCDMA2k_ModAccFetchEVMTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize ));
	if(actualArraySize > 0)
	{
		EVMTrace = (float32*)malloc(actualArraySize * sizeof(float32));
		if(EVMTrace)
		{
			RFmxCheckWarn(RFmxCDMA2k_ModAccFetchEVMTrace(instrumentHandle, "", timeout, &x0, &dx, EVMTrace, actualArraySize, NULL ));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}	
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxCDMA2k_ModAccFetchDetectedChannelArray(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		detectedWalshCodeLength = (int32 *) malloc(sizeof(int) * actualArraySize);
		detectedWalshCodeNumber = (int32 *) malloc(sizeof(int) * actualArraySize);
		detectedBranch = (int32 *) malloc(sizeof(int32) * actualArraySize);

		if (detectedWalshCodeLength && detectedWalshCodeNumber && detectedBranch)
		{

			RFmxCheckWarn(RFmxCDMA2k_ModAccFetchDetectedChannelArray(instrumentHandle, "", timeout, detectedWalshCodeLength, 
				detectedWalshCodeNumber, detectedBranch, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	/* Display results */

	printf("--------------EVM ------------------------------------------\n");
	printf("RMS EVM (%%)                       : %f\n", RMSEVM);	
	printf("Peak EVM (%%)                      : %f\n", peakEVM);
	printf("Rho                               : %f\n", rho);
	printf("Frequency Error (Hz)              : %f\n", frequencyError);
	printf("Chip Rate Error (ppm)             : %f\n", chipRateError);
	printf("RMS Magnitude Error (%%)           : %f\n", RMSMagnitudeError);
	printf("RMS Phase Error (deg)             : %f\n", RMSPhaseError);	

	printf("\n\n--------------Code Domain Error--------------------------\n");
	printf("Peak CDE (dB)                     : %f\n", peakCDE);	
	printf("Peak CDE Walsh Code Number        : %d\n", peakCDEWalshCodeNumber);
	switch (peakCDEBranch)
	{
	case 0: printf("Peak CDE Branch                   : I\n");
		break;
	case 1: printf("Peak CDE Branch                   : Q\n");
		break;
	case 2: printf("Peak CDE Branch                   : I and Q\n");
		break;
	}
    printf("Peak Active CDE (dB)              : %f\n", peakActiveCDE);
	printf("Peak Active CDE Walsh Code Length : %d\n", peakActiveCDEWalshCodeLength);
	printf("Peak Active CDE Walsh Code Number : %d\n", peakActiveCDEWalshCodeNumber);
	switch (peakActiveCDEBranch)
	{
	case 0: printf("Peak Active CDE Branch            : I\n");
		break;
	case 1: printf("Peak Active CDE Branch            : Q\n");
		break;
	case 2: printf("Peak Active CDE Branch            : I and Q\n");
		break;
	}		

	printf("\n\n--------------I/Q Impairments----------------------------\n");
	printf("I/Q Origin Offset (dB)            : %f\n", IQOriginOffset);		
	printf("I/Q Gain Imbalance (dB)           : %f\n", IQGainImbalance);
	printf("I/Q Quadrature Error (deg)        : %f\n", IQQuadratureError);

	printf("\n\n---------------------Detected Channels-------------------\n");
	for (i = 0; i < actualArraySize; i++)
	{
		printf("Idx                               : %d\n", i);
		printf("Length                            : %d\n", detectedWalshCodeLength[i]);		
		printf("Number                            : %d\n", detectedWalshCodeNumber[i]);
		switch (detectedBranch[i])
	    {
			case 0: printf("Peak CDE Branch                   : I\n");
				break;
			case 1: printf("Peak CDE Branch                   : Q\n");
				break;
			case 2: printf("Peak CDE Branch                   : I and Q\n");
				break;
		}
	}


Error:
	if( error ) 
	{
		RFmxCDMA2k_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if(error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}
	if(instrumentHandle)
	{
		RFmxCDMA2k_Close(instrumentHandle, RFMXCDMA2K_VAL_FALSE);
	}
	/* Free allocated memory */	
	if(constellation)
		free(constellation);
	if(EVMTrace)
		free(EVMTrace);
	if(actualArraySize)
	{
		free(detectedWalshCodeLength);
		free(detectedWalshCodeNumber);
		free(detectedBranch);
	}

	printf("\nPress any key to exit");
	_getch();

	return error;
}
