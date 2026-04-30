//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select OBW measurement and enable traces.
//6. Configure Sweep Time Parameters.
//7. Configure Averaging Parameters.
//8. Initiate the Measurement.
//9. Fetch OBW Measurements and Traces.
//10. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxCDMA2k.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main (int argc, char *argv[])
{
	niRFmxInstrHandle instrumentHandle = NULL;
	int32 error = 0, lastErrorCode = 0;
	char errorMessage[MAX_ERROR_DESCRIPTION] = {'\0'};

	char *resourceName = "RFSA";	
	float64 centerFrequency = 833.490e+6;			/* Hz */
	float64 referenceLevel = 0.00;					/* dBm */
	float64 externalAttenuation = 0.00;				/* dB */

	float64 timeout = 10.0;							/* s */	

	/* Frequency Reference */
	char *frequencySource = RFMXCDMA2K_VAL_ONBOARD_CLOCK_STR;	
	float64 frequency = 10.0e+6;					/* Hz */

	/* Sweep time */
	int32 sweepTimeAuto = RFMXCDMA2K_VAL_OBW_SWEEP_TIME_AUTO_TRUE;
	float64 sweepTimeInterval = 1.670e-3;			/* s */

	/* Averaging */
	int32 averagingEnabled = RFMXCDMA2K_VAL_OBW_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;
	int32 averagingType = RFMXCDMA2K_VAL_OBW_AVERAGING_TYPE_RMS;	

	/* Variables to store OBW measurement results */
	float64 stopFrequency = 0;						/* Hz */
	float64 startFrequency = 0;						/* Hz */
	float64 occupiedBandwidth = 0;						/* Hz */
	float64 absolutePower = 0;						/* dBm */

	int32 actualArraySize = 0;
	float64 x0 = 0.0, dx = 0.0;
	float32 *spectrum = NULL;

	/* Trigger */
	int32 digitalTriggerEnabled = RFMXCDMA2K_VAL_FALSE;
	char* digitalEdgeSource = RFMXCDMA2K_VAL_PFI0_STR;
	int32 digitalEdge =  RFMXCDMA2K_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.00;

	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxCDMA2k_Initialize(resourceName, "", &instrumentHandle, NULL));

	/* Configure OBW parameters */
	RFmxCheckWarn(RFmxCDMA2k_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
	RFmxCheckWarn(RFmxCDMA2k_CfgRF(instrumentHandle,"",centerFrequency,referenceLevel,externalAttenuation ));
	RFmxCheckWarn(RFmxCDMA2k_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, digitalTriggerEnabled ));
	RFmxCheckWarn(RFmxCDMA2k_SelectMeasurements(instrumentHandle, "", RFMXCDMA2K_VAL_OBW, RFMXCDMA2K_VAL_TRUE));
	RFmxCheckWarn(RFmxCDMA2k_OBWCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
	RFmxCheckWarn(RFmxCDMA2k_OBWCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
	RFmxCheckWarn(RFmxCDMA2k_Initiate(instrumentHandle, "", ""));	

	/* Retrieve results */

	RFmxCheckWarn(RFmxCDMA2k_OBWFetchMeasurement(instrumentHandle, "", timeout, &occupiedBandwidth, &absolutePower, &startFrequency, &stopFrequency));

	RFmxCheckWarn(RFmxCDMA2k_OBWFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, 
		&actualArraySize));
	if( actualArraySize > 0 )
	{
		spectrum = (float32 *)malloc(sizeof(float32)*actualArraySize);
		if(spectrum != NULL)
		{
			RFmxCheckWarn(RFmxCDMA2k_OBWFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, 
				actualArraySize, NULL));
		}	
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}	

	/* Display results */
	printf("---------------------- Measurement --------------------------\n\n");
	printf("Occupied Bandwidth (Hz)       : %f\n", occupiedBandwidth);
	printf("Absolute Power (dBm)          : %f\n", absolutePower);
	printf("Start Frequency (Hz)          : %f\n", startFrequency);
	printf("Stop Frequency (Hz)           : %f\n", stopFrequency);
Error:
	if( error ) 
	{
		RFmxCDMA2k_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if (error < 0)
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
	printf("\n\nPress any key to exit\n");
	_getch();

	return error;
}
