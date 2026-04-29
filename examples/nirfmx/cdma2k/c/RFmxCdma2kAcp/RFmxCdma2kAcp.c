//Steps:
//1. Open a new RFmx session
//2. Configure Reference Clock
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure RF Attenuation
//5. Configure Trigger
//6. Configure Band Class
//7. Set Measurement to ACP
//8. Configure Number of Offsets
//9. Set Noise Compensation
//10. Set Dynamic Range Mode
//11. Configure Sweep Time
//12. Configure Averaging Parameters
//13. Commit Settings and Initiate Measurement
//14. Fetch diverse ACP Measurement Results
//15. Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxCDMA2k.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096
#define NUMBER_OF_OFFSETS			2

int main (int argc, char *argv[])
{	    
	niRFmxInstrHandle instrumentHandle = NULL;
	int32 error = 0, lastErrorCode = 0;
	char errorMessage[MAX_ERROR_DESCRIPTION] = {'\0'};
	int32 i = 0;

	char* resourceName = "RFSA";
	float64 centerFrequency = 833.490e+6;				/* Hz */
	float64 externalAttenuation = 0.00;					/* dB */

	/* Auto Level */
	int32 autoLevel = 1;
	float64 referenceLevel = 0.00;						/* dBm */

	int32 RFAttenuationAuto = RFMXCDMA2K_VAL_TRUE;
	float64 RFAttenuation = 10.00;						/* dB */

	/* Frequency Reference */
	char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
	float64 frequency = 10.0e+6;						/* Hz */

	/* Trigger */
	int32 digitalTriggerEnabled = RFMXCDMA2K_VAL_FALSE;
	char* digitalEdgeSource = RFMXCDMA2K_VAL_PFI0_STR;
	int32 digitalEdge =  RFMXCDMA2K_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.0;							/* s */

	int32 bandClass = 0;

	int32 noiseCompensationEnabled = RFMXCDMA2K_VAL_ACP_NOISE_COMPENSATION_ENABLED_FALSE;
	int32 measurementMethod = RFMXCDMA2K_VAL_ACP_MEASUREMENT_METHOD_NORMAL;

	/* Sweep Time */
	int32 sweepTimeAuto = RFMXCDMA2K_VAL_ACP_SWEEP_TIME_AUTO_TRUE;
	float64 sweepTimeInterval = 1.67e-3;				/* s */

	/* Averaging */
	int32 averagingEnabled = RFMXCDMA2K_VAL_ACP_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;	
	int32 averagingType = RFMXCDMA2K_VAL_ACP_AVERAGING_TYPE_RMS;

	float64 timeout = 10.0;	
	int32 actualArraySize = 0;

	/* Variables to store ACP measurement status */	
	float64 carrierAbsolutePower = 0.0;		/* dBm */
	
	float64 lowerRelativePower[NUMBER_OF_OFFSETS] = {0};		/* dB */
	float64 upperRelativePower[NUMBER_OF_OFFSETS] = {0};		/* dB */
	float64 lowerAbsolutePower[NUMBER_OF_OFFSETS] = {0};		/* dBm */
	float64 upperAbsolutePower[NUMBER_OF_OFFSETS] = {0};		/* dBm */

	/* variables to store traces */
	float64 x0 = 0.0,dx = 0.0;
	float32 *spectrum = (float32 *)NULL;	

	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxCDMA2k_Initialize(resourceName, "", &instrumentHandle, NULL));

	/* Configure CDMA2k ACP measurement parameters */
	RFmxCheckWarn(RFmxCDMA2k_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
	RFmxCheckWarn(RFmxCDMA2k_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
	RFmxCheckWarn(RFmxCDMA2k_CfgFrequency(instrumentHandle, "", centerFrequency ));		    
	RFmxCheckWarn(RFmxCDMA2k_CfgRFAttenuation(instrumentHandle, "", RFAttenuationAuto, RFAttenuation));	
	RFmxCheckWarn(RFmxCDMA2k_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, digitalTriggerEnabled));
	if(autoLevel)
	{
		RFmxCheckWarn(RFmxCDMA2k_AutoLevel(instrumentHandle, "", .02, &referenceLevel ));
		printf("Reference level (dBm)                    : %f\n", referenceLevel);
	}
	else
	{
		RFmxCheckWarn(RFmxCDMA2k_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
	}
	RFmxCheckWarn(RFmxCDMA2k_CfgBandClass(instrumentHandle, "", bandClass ));	
	RFmxCheckWarn(RFmxCDMA2k_SelectMeasurements(instrumentHandle, "", RFMXCDMA2K_VAL_ACP, RFMXCDMA2K_VAL_TRUE));
	RFmxCheckWarn(RFmxCDMA2k_ACPCfgNumberOfOffsets(instrumentHandle, "", NUMBER_OF_OFFSETS ));
	RFmxCheckWarn(RFmxCDMA2k_ACPCfgNoiseCompensationEnabled(instrumentHandle, "", noiseCompensationEnabled ));
	RFmxCheckWarn(RFmxCDMA2k_ACPCfgMeasurementMethod(instrumentHandle, "", measurementMethod ));
	RFmxCheckWarn(RFmxCDMA2k_ACPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval ));
	RFmxCheckWarn(RFmxCDMA2k_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType ));
	RFmxCheckWarn(RFmxCDMA2k_Initiate(instrumentHandle,"",""));

	/* Fetch diverse ACP Measurement Results */	

	RFmxCheckWarn(RFmxCDMA2k_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, 
				lowerRelativePower, upperRelativePower, upperAbsolutePower, lowerAbsolutePower, NUMBER_OF_OFFSETS, NULL));			
		
	RFmxCheckWarn(RFmxCDMA2k_ACPFetchCarrierAbsolutePower(instrumentHandle, "", timeout, &carrierAbsolutePower));		    

	RFmxCheckWarn(RFmxCDMA2k_ACPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, 
		&actualArraySize));
	if( actualArraySize > 0 )
	{
		spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if(spectrum)
		{
			RFmxCheckWarn(RFmxCDMA2k_ACPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	/* Display ACP Measurement Results */	
	printf("Carrier Absolute Power (dBm)             : %f\n", carrierAbsolutePower);

	printf("\n-------------- Offset Channel Measurements --------------\n");

	for(i = 0; i < NUMBER_OF_OFFSETS; i++)
	{
		printf("\nOFFSET %d\n", i);
		printf("Lower Relative Power (dB)                : %f\n", lowerRelativePower[i]);
		printf("Upper Relative Power (dB)                : %f\n", upperRelativePower[i]);
		printf("Lower Absolute Power (dBm)               : %f\n", lowerAbsolutePower[i]);
		printf("Upper Absolute Power (dBm)               : %f\n", upperAbsolutePower[i]);
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
	
	printf("\nPress any key to exit");
	_getch();
	return error;
}
