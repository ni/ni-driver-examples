//Steps:
//1. Open a new RFmx session
//2. Configure Reference Clock
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure Trigger
//5. Configure Band Class
//6. Configure Radio Configuration
//7. Configure Long Code Mask
//8. Set all Measurements at once
//9. Configure Synchronization Mode
//10. Configure Sweep Time
//11. Configure Averaging Parameters
//12. Commit Settings and Initiate Measurement
//13. Fetch diverse SEM Measurement Results
//14. Fetch diverse OBW Measurement Results
//15. Fetch diverse CHP Measurement Results
//16. Fetch diverse ACP Measurement Results
//17. Fetch diverse ModAcc Measurement Results
//18. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxCDMA2k.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char *argv[])
{
	int32 i = 0;
	niRFmxInstrHandle instrumentHandle = NULL;
	int32 error = 0,  lastErrorCode = 0;
	char errorMessage[MAX_ERROR_DESCRIPTION] = {'\0'};

	char *resourceName = "RFSA";	
	float64 centerFrequency = 833.490e+6;						/* Hz */
	float64 referenceLevel = 0.00;								/* dBm */
	float64 externalAttenuation = 0.00;							/* dB */

	/* Frequency Refernce Settings */
	float64 frequency = 10.0e+6;								/* Hz */
	char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;	

	/* Trigger Settings */
	float64 triggerDelay = 0.00;								/* s */
	int32 digitalTriggerEnabled = RFMXCDMA2K_VAL_FALSE;
	char* digitalEdgeSource = RFMXCDMA2K_VAL_PFI0_STR;
	int32 digitalEdge =  RFMXCDMA2K_VAL_DIGITAL_EDGE_RISING_EDGE;

	int32 radioConfiguration = RFMXCDMA2K_VAL_RADIO_CONFIGURATION_RC3;
	int32 bandClass = 0;	

	/* ModAcc Measurement Settings */	
	int32 synchronizationMode = RFMXCDMA2K_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;
	int32 measurementOffset = 0;								/* slots */
	int32 measurementLength = 1;								/* slots */
	int64 uplinkSpreadingLongCodeMask = 0;

	/* Sweep Time Settings */
	float64 sweepTimeInterval = 1.670e-3;						/* s */
	int32 sweepTimeAuto = RFMXCDMA2K_VAL_ACP_SWEEP_TIME_AUTO_TRUE;	

	/* Averaging Settings*/
	int32 averagingEnabled = RFMXCDMA2K_VAL_FALSE;
	int32 averagingCount = 10;

	float64 timeout = 10.0;										/* s */

	/* Variables to store ModAcc measurement results */
	float64 rmsEVM = 0.0;										/* % */
	float64 peakEVM = 0.0;										/* % */
	float64 rho = 0.0;
	float64 frequencyError = 0.0;								/* Hz */
	float64 chipRateError = 0.0;								/* ppm */
	float64 rmsMagnitudeError = 0.0;							/* % */
	float64 rmsPhaseError = 0.0;								/* deg */

	/* Variables to store ACP measurement status */
	float64 ACPcarrierAbsolutePower = 0.0;						/* dBm */
	int32 offsetArraySize = 0;
	float64 *lowerRelativePower = (float64 *)NULL;				/* dB */
	float64 *upperRelativePower = (float64 *)NULL;				/* dB */
	float64 *lowerAbsolutePower = (float64 *)NULL;				/* dBm */
	float64 *upperAbsolutePower = (float64 *)NULL;				/* dBm */

	/* Variables to store CHP measurement results */
	float64 CHPcarrierAbsolutePower = 0;						/* dBm */

	/* Variables to store OBW measurement results */
	float64 stopFrequency = 0.00;								/* Hz */
	float64 startFrequency = 0.00;								/* Hz */
	float64 occupiedBandwidth = 0.00;									/* Hz */
	float64 absolutePower = 0.00;								/* dBm */

	/* Variables to store SEM measurement status */
	int32 measurementStatus;
	float64 carrierAbsoluteIntegratedPower = 0.00;				/* dBm */
	int32 lowerOffsetMarginArraySize = 0, upperOffsetMarginArraySize = 0;

	float64 *lowerOffsetMargin = (float64 *)NULL;				/* dB */
	float64 *lowerOffsetMarginAbsolutePower = (float64 *)NULL;			/* dBm */
	float64 *lowerOffsetMarginRelativePower = (float64 *)NULL;			/* dB */
	float64 *lowerOffsetMarginFrequency = (float64 *)NULL;			/* Hz */
	int32	*lowerOffsetMeasurementStatus = (int32 *)NULL;	

	float64 *upperOffsetMargin = (float64 *)NULL;				/* dB */
	float64 *upperOffsetMarginAbsolutePower = (float64 *)NULL;			/* dBm */
	float64 *upperOffsetMarginRelativePower = (float64 *)NULL;			/* dB */
	float64 *upperOffsetMarginFrequency = (float64 *)NULL;			/* Hz */
	int32	*upperOffsetMeasurementStatus = (int32 *)NULL;

	/* Create new RFmx session */
	RFmxCheckWarn(RFmxCDMA2k_Initialize (resourceName, "", &instrumentHandle, NULL));	

	RFmxCheckWarn(RFmxCDMA2k_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
	RFmxCheckWarn(RFmxCDMA2k_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxCDMA2k_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, digitalTriggerEnabled));
	RFmxCheckWarn(RFmxCDMA2k_CfgBandClass(instrumentHandle, "", bandClass));
	RFmxCheckWarn(RFmxCDMA2k_CfgRadioConfiguration(instrumentHandle, "", radioConfiguration));
	RFmxCheckWarn(RFmxCDMA2k_CfgUplinkSpreading(instrumentHandle, "", uplinkSpreadingLongCodeMask));

	RFmxCheckWarn(RFmxCDMA2k_SelectMeasurements(instrumentHandle, "", RFMXCDMA2K_VAL_MODACC | RFMXCDMA2K_VAL_ACP | RFMXCDMA2K_VAL_CHP |RFMXCDMA2K_VAL_OBW | RFMXCDMA2K_VAL_SEM , RFMXCDMA2K_VAL_FALSE));
	RFmxCheckWarn(RFmxCDMA2k_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode, measurementOffset, measurementLength ));

	RFmxCheckWarn(RFmxCDMA2k_ACPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval ));
	RFmxCheckWarn(RFmxCDMA2k_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, RFMXCDMA2K_VAL_ACP_AVERAGING_TYPE_RMS ));

	RFmxCheckWarn(RFmxCDMA2k_CHPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval ));
	RFmxCheckWarn(RFmxCDMA2k_CHPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, RFMXCDMA2K_VAL_CHP_AVERAGING_TYPE_RMS ));

	RFmxCheckWarn(RFmxCDMA2k_OBWCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
	RFmxCheckWarn(RFmxCDMA2k_OBWCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, RFMXCDMA2K_VAL_OBW_AVERAGING_TYPE_RMS));

	RFmxCheckWarn(RFmxCDMA2k_SEMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
	RFmxCheckWarn(RFmxCDMA2k_SEMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, RFMXCDMA2K_VAL_SEM_AVERAGING_TYPE_RMS));

	RFmxCheckWarn(RFmxCDMA2k_Initiate(instrumentHandle, "", ""));

	/* Retrieve results */

	/* Fetch diverse SEM Measurement Results */	

	RFmxCheckWarn(RFmxCDMA2k_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL, 
		NULL, NULL, 0, &lowerOffsetMarginArraySize));
	if(lowerOffsetMarginArraySize > 0)
	{
		lowerOffsetMargin	    =	(float64 *)malloc(sizeof(float64)*lowerOffsetMarginArraySize);
		lowerOffsetMarginAbsolutePower =	(float64 *)malloc(sizeof(float64)*lowerOffsetMarginArraySize); 
		lowerOffsetMarginRelativePower =	(float64 *)malloc(sizeof(float64)*lowerOffsetMarginArraySize);
		lowerOffsetMarginFrequency   =	(float64 *)malloc(sizeof(float64)*lowerOffsetMarginArraySize);
		lowerOffsetMeasurementStatus   =	(int32 *)malloc(sizeof(int32)*lowerOffsetMarginArraySize);

		if(lowerOffsetMargin && lowerOffsetMarginAbsolutePower && lowerOffsetMarginRelativePower && lowerOffsetMarginFrequency && lowerOffsetMeasurementStatus)
		{
			RFmxCheckWarn(RFmxCDMA2k_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, lowerOffsetMeasurementStatus, 
				lowerOffsetMargin, lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower, lowerOffsetMarginArraySize, NULL));			
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}


	RFmxCheckWarn(RFmxCDMA2k_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL, 
		NULL, NULL, 0, &upperOffsetMarginArraySize));
	if(upperOffsetMarginArraySize > 0)
	{
		upperOffsetMargin	    =	(float64 *)malloc(sizeof(float64)*upperOffsetMarginArraySize);
		upperOffsetMarginAbsolutePower =	(float64 *)malloc(sizeof(float64)*upperOffsetMarginArraySize); 
		upperOffsetMarginRelativePower =	(float64 *)malloc(sizeof(float64)*upperOffsetMarginArraySize);
		upperOffsetMarginFrequency   =	(float64 *)malloc(sizeof(float64)*upperOffsetMarginArraySize);
		upperOffsetMeasurementStatus   =	(int32 *)malloc(sizeof(int32)*upperOffsetMarginArraySize);

		if(upperOffsetMargin && upperOffsetMarginAbsolutePower && upperOffsetMarginRelativePower && upperOffsetMarginFrequency && upperOffsetMeasurementStatus)
		{
			RFmxCheckWarn(RFmxCDMA2k_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, upperOffsetMeasurementStatus, upperOffsetMargin, 
				upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower, upperOffsetMarginArraySize, NULL));				
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxCDMA2k_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));

	RFmxCheckWarn(RFmxCDMA2k_SEMFetchCarrierAbsoluteIntegratedPower(instrumentHandle, "",timeout, &carrierAbsoluteIntegratedPower));	

	/* Fetch diverse OBW Measurement Results */
	RFmxCheckWarn(RFmxCDMA2k_OBWFetchMeasurement(instrumentHandle, "", timeout, &occupiedBandwidth, &absolutePower, &startFrequency, &stopFrequency));

	/* Fetch diverse CHP Measurement Results */
	RFmxCheckWarn(RFmxCDMA2k_CHPFetchCarrierAbsolutePower(instrumentHandle, "", timeout, &CHPcarrierAbsolutePower));

	/* Fetch diverse ACP Measurement Results */
	RFmxCheckWarn(RFmxCDMA2k_ACPFetchCarrierAbsolutePower(instrumentHandle, "", timeout, &ACPcarrierAbsolutePower));

	RFmxCheckWarn(RFmxCDMA2k_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, NULL, NULL, NULL, 
		NULL, 0, &offsetArraySize));
	if(offsetArraySize > 0)
	{
		lowerRelativePower	    =	(float64 *)malloc(sizeof(float64)*offsetArraySize);
		upperRelativePower		=	(float64 *)malloc(sizeof(float64)*offsetArraySize); 
		lowerAbsolutePower		=	(float64 *)malloc(sizeof(float64)*offsetArraySize);
		upperAbsolutePower		=	(float64 *)malloc(sizeof(float64)*offsetArraySize);

		if(lowerRelativePower && upperRelativePower && lowerAbsolutePower && upperAbsolutePower)
		{
			RFmxCDMA2k_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, lowerRelativePower, upperRelativePower, 
				lowerAbsolutePower, upperAbsolutePower, offsetArraySize, NULL);				
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	/* Fetch diverse ModAcc Measurement Results */	
	RFmxCheckWarn(RFmxCDMA2k_ModAccFetchEVM(instrumentHandle, "", timeout, &rmsEVM, &peakEVM, &rho, &frequencyError, &chipRateError, &rmsMagnitudeError, &rmsPhaseError ));

	/* Display results */

	/* Display ModAcc Measurement Reults */
	printf("--------------------- ModAcc Measurement Results -------------------\n");	
	printf("                         EVM \n");
	printf("RMS EVM (%%)                                  : %f\n", rmsEVM);	
	printf("Peak EVM (%%)                                 : %f\n", peakEVM);
	printf("Rho                                          : %f\n", rho);
	printf("Frequency Error (Hz)                         : %f\n", frequencyError);
	printf("Chip Rate Error (ppm)                        : %f\n", chipRateError);
	printf("RMS Magnitude Error (%%)                      : %f\n", rmsMagnitudeError);
	printf("RMS Phase Error (deg)                        : %f\n", rmsPhaseError);	

	/* Display ACP Measurement Results */
	printf("\n\n--------------------- ACP Measurement Results -----------------------\n");	
	printf("Carrier Absolute Power (dBm)                 : %f\n", ACPcarrierAbsolutePower);

	printf("\n                  Offset Channel Measurements \n");

	for( i=0; i < offsetArraySize; i++)
	{
		printf("\nOFFSET %d\n", i);
		printf("Lower Relative Power (dB)                    : %f\n", lowerRelativePower[i]);
		printf("Upper Relative Power (dB)                    : %f\n", upperRelativePower[i]);
		printf("Lower Absolute Power (dBm)                   : %f\n", lowerAbsolutePower[i]);
		printf("Upper Absolute Power (dBm)                   : %f\n", upperAbsolutePower[i]);
	}

	/* Display CHP Measurement Reults */
	printf("\n\n--------------------- CHP Measurement Results ------------------------\n");	
	printf("Carrier Absolute Power (dBm)                 : %f\n", CHPcarrierAbsolutePower);	

	/* Display OBW Measurement Reults */
	printf("\n\n--------------------- OBW Measurement Results ------------------------\n");	
	printf("Occupied Bandwidth (Hz)                      : %f\n", occupiedBandwidth);
	printf("Absolute Power (dBm)                         : %f\n", absolutePower);
	printf("Start Frequency (Hz)                         : %f\n", startFrequency);
	printf("Stop Frequency (Hz)                          : %f\n", stopFrequency);		

	/* Display SEM Measurement Reults */
	printf("\n\n--------------------- SEM Measurement Results ------------------------\n");
	printf("Measurement Status                           : %s\n",measurementStatus?"Pass":"Fail");
	printf("Carrier Absolute Integrated Power (dBm)      : %f\n",carrierAbsoluteIntegratedPower);	

	printf("\n Lower Offset Segment Measurements \n");
	for(i = 0; i < lowerOffsetMarginArraySize; i++)
	{
		printf("\nOFFSET                                       : %d\n", i);
		printf("Margin (dB)                                  : %f\n", lowerOffsetMargin[i]);
		printf("Margin Absolute Power (dBm)                  : %f\n", lowerOffsetMarginAbsolutePower[i]);
		printf("Margin Relative Power (dB)                   : %f\n", lowerOffsetMarginRelativePower[i]);
		printf("Margin Frequency (Hz)                        : %f\n", lowerOffsetMarginFrequency[i]);
		printf("Measurement Status                           : %s\n", lowerOffsetMeasurementStatus[i]?"Pass":"Fail");				
	}
	printf("\n Upper Offset Segment Measurements \n");
	for(i = 0; i < upperOffsetMarginArraySize; i++)
	{
		printf("\nOFFSET                                       : %d\n", i);
		printf("Margin (dB)                                  : %f\n", upperOffsetMargin[i]);
		printf("Margin Absolute Power (dBm)                  : %f\n", upperOffsetMarginAbsolutePower[i]);
		printf("Margin Relative Power (dB)                   : %f\n", upperOffsetMarginRelativePower[i]);
		printf("Margin Frequency (Hz)                        : %f\n", upperOffsetMarginFrequency[i]);
		printf("Measurement Status                           : %s\n", upperOffsetMeasurementStatus[i]?"Pass":"Fail");				
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

	if(lowerOffsetMargin)
		free(lowerOffsetMargin);
	if(lowerOffsetMarginAbsolutePower)
		free(lowerOffsetMarginAbsolutePower);
	if(lowerOffsetMarginRelativePower)
		free(lowerOffsetMarginRelativePower);
	if(lowerOffsetMarginFrequency)
		free(lowerOffsetMarginFrequency);
	if(lowerOffsetMeasurementStatus)
		free(lowerOffsetMeasurementStatus);
	if(upperOffsetMargin)
		free(upperOffsetMargin);
	if(upperOffsetMarginAbsolutePower)
		free(upperOffsetMarginAbsolutePower);
	if(upperOffsetMarginRelativePower)
		free(upperOffsetMarginRelativePower);
	if(upperOffsetMarginFrequency)
		free(upperOffsetMarginFrequency);
	if(upperOffsetMeasurementStatus)
		free(upperOffsetMeasurementStatus);
	if(lowerRelativePower)
		free(lowerRelativePower);
	if(upperRelativePower)
		free(upperRelativePower);
	if(lowerAbsolutePower)
		free(lowerAbsolutePower);
	if(upperAbsolutePower)
		free(upperAbsolutePower);

	printf("\n\nPress any key to exit\n");
	_getch();

	return error;
}
