//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Band Class.
//6. Select SEM measurement and enable traces.
//7. Configure Sweep Time Parameters.
//8. Configure Averaging Parameters.
//9. Initiate the Measurement.
//10. Fetch SEM Measurements and Traces.
//11. Close the RFmx Session.

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
	float64 timeout = 10.0;										/* s */

	char *resourceName = "RFSA";	
	float64 centerFrequency = 833.490e+6;						/* Hz */
	float64 referenceLevel = 0.00;								/* dBm */
	float64 externalAttenuation = 0.00;							/* dB */	

	/* Variables to store Frequency Reference */
	char *frequencySource = RFMXCDMA2K_VAL_ONBOARD_CLOCK_STR;	
	float64 frequency = 10.0e+6;								/* Hz */

	/* Variables to store Trigger */
	float64 triggerDelay = 0.00;								/* s */
	int32 digitalTriggerEnabled = RFMXCDMA2K_VAL_FALSE;
	char* digitalEdgeSource = RFMXCDMA2K_VAL_PFI0_STR;
	int32 digitalEdge =  RFMXCDMA2K_VAL_DIGITAL_EDGE_RISING_EDGE;

	int32 bandClass = 0;		

	/* Variables to store Sweep Time */
	float64 sweepTimeInterval = 1.670e-3;						/* s */
	int32 sweepTimeAuto = RFMXCDMA2K_VAL_ACP_SWEEP_TIME_AUTO_TRUE;

	/* Averaging */
	int32 averagingEnabled = RFMXCDMA2K_VAL_SEM_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;
	int32 averagingType = RFMXCDMA2K_VAL_SEM_AVERAGING_TYPE_RMS;

	/* variables to store traces */
	float64 x0 = 0.0, dx = 0.0;
	float32 *spectrum = (float32 *)NULL;
	float32 *relativeMask = (float32 *)NULL;
	float32 *absoluteMask = (float32 *)NULL;
	int32 actualArraySize = 0;

	/* Variables to store SEM measurement status */
	int32 measurementStatus = 0;
	float64 carrierAbsoluteIntegratedPower = 0.00;				/* dBm */
	int32 lowerOffsetMarginArraySize = 0,upperOffsetMarginArraySize = 0;

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

	/* Configure SEM measurement parameters */
	RFmxCheckWarn(RFmxCDMA2k_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
	RFmxCheckWarn(RFmxCDMA2k_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxCDMA2k_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, digitalTriggerEnabled));
	RFmxCheckWarn(RFmxCDMA2k_CfgBandClass(instrumentHandle, "", bandClass));
	RFmxCheckWarn(RFmxCDMA2k_SelectMeasurements(instrumentHandle, "", RFMXCDMA2K_VAL_SEM, RFMXCDMA2K_VAL_TRUE));
	RFmxCheckWarn(RFmxCDMA2k_SEMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
	RFmxCheckWarn(RFmxCDMA2k_SEMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));	
	RFmxCheckWarn(RFmxCDMA2k_Initiate(instrumentHandle, "", ""));	

	/* Retrieve results */	

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
			RFmxCheckWarn(RFmxCDMA2k_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, lowerOffsetMeasurementStatus, lowerOffsetMargin, lowerOffsetMarginFrequency, 
				lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower, lowerOffsetMarginArraySize, NULL));
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
			RFmxCheckWarn(RFmxCDMA2k_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout,	upperOffsetMeasurementStatus, upperOffsetMargin, upperOffsetMarginFrequency,
				upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower, upperOffsetMarginArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxCDMA2k_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));

	RFmxCheckWarn(RFmxCDMA2k_SEMFetchCarrierAbsoluteIntegratedPower(instrumentHandle, "", timeout, &carrierAbsoluteIntegratedPower));	

	/* Fetch spectrum */	
	RFmxCheckWarn(RFmxCDMA2k_SEMFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		spectrum =(float32 *)malloc(sizeof(float32)*actualArraySize);
		relativeMask =(float32 *)malloc(sizeof(float32)*actualArraySize);
		absoluteMask =(float32 *)malloc(sizeof(float32)*actualArraySize);
		if(spectrum)
		{
			RFmxCheckWarn(RFmxCDMA2k_SEMFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, relativeMask, absoluteMask, actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}	
	}

	/* Display results */
	printf("Measurement Status                           : %s\n",measurementStatus?"Pass":"Fail");
	printf("Carrier Absolute Integrated Power (dBm)      : %f\n",carrierAbsoluteIntegratedPower);	

	printf("\n Lower Offser Segment Measurements \n");
	for(i = 0; i < lowerOffsetMarginArraySize; i++)
	{
		printf("\nOFFSET                                      : %d\n", i);
		printf("Margin (dB)                                 : %f\n", lowerOffsetMargin[i]);
		printf("Margin Absolute Power (dBm)                 : %f\n", lowerOffsetMarginAbsolutePower[i]);
		printf("Margin Relative Power (dB)                  : %f\n", lowerOffsetMarginRelativePower[i]);
		printf("Margin Frequency (Hz)                       : %f\n", lowerOffsetMarginFrequency[i]);
		printf("Measurement Status                          : %s\n", lowerOffsetMeasurementStatus[i]?"Pass":"Fail");				
	}
	printf("\n Upper Offser Segment Measurements \n");
	for(i = 0; i < upperOffsetMarginArraySize; i++)
	{
		printf("\nOFFSET                                      : %d\n", i);
		printf("Margin (dB)                                 : %f\n", upperOffsetMargin[i]);
		printf("Margin Absolute Power (dBm)                 : %f\n", upperOffsetMarginAbsolutePower[i]);
		printf("Margin Relative Power (dB)                  : %f\n", upperOffsetMarginRelativePower[i]);
		printf("Margin Frequency (Hz)                       : %f\n", upperOffsetMarginFrequency[i]);
		printf("Measurement Status                          : %s\n", upperOffsetMeasurementStatus[i]?"Pass":"Fail");				
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
	if(spectrum)
		free(spectrum);		
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

	printf("\nPress any key to exit");
	_getch();

	return error;
}
