// Steps:
// 1. Open a new RFmx session
// 2. Configure Reference Clock
// 3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
// 4. Configure Trigger
// 5. Set Measurement to CHP
// 6. Configure Sweep Time
// 7. Configure Averaging Parameters
// 8. Commit Settings and Initiate Measurement
// 9. Fetch diverse CHP Measurement Results.
// 10. Close the RFmx Session

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
	float64 centerFrequency = 833.490e+6;		/* Hz */
	float64 referenceLevel = 0.00;				/* dBm */
	float64 externalAttenuation = 0.00;			/* dB */

	/* Frequency Reference */
	char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;	
	float64 frequency = 10.0e+6;				/* Hz */	

	/* Trigger */
	float64 triggerDelay = 0.00;				/* s */
	int32 digitalTriggerEnabled = RFMXCDMA2K_VAL_FALSE;
	char* digitalEdgeSource = RFMXCDMA2K_VAL_PFI0_STR;
	int32 digitalEdge = RFMXCDMA2K_VAL_DIGITAL_EDGE_RISING_EDGE;


	/* Sweep time settings */
	int32 sweepTimeAuto = RFMXCDMA2K_VAL_CHP_SWEEP_TIME_AUTO_TRUE;
	float64 sweepTimeInterval = 1.670e-3;		/* s */

	/* Averaging */ 
	int32 averagingEnabled = RFMXCDMA2K_VAL_CHP_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;
	int32 averagingType = RFMXCDMA2K_VAL_CHP_AVERAGING_TYPE_RMS; 

	/* Variables to store the measurement results */
	float64 carrierAbsolutePower = 0;			/* dBm */
	float64 timeout = 10.0;						/* s */

	/* Variables to store Traces */
	float64 x0 = 0.0, dx = 0.0;
	float32 *spectrum = (float32 *)NULL;
	int32 actualArraySize = 0;    

	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxCDMA2k_Initialize(resourceName, "", &instrumentHandle, NULL));

	/* Configure CHP parameters */

	RFmxCheckWarn(RFmxCDMA2k_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
	RFmxCheckWarn(RFmxCDMA2k_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation ));	
	RFmxCheckWarn(RFmxCDMA2k_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, digitalTriggerEnabled ));
	RFmxCheckWarn(RFmxCDMA2k_SelectMeasurements(instrumentHandle, "", RFMXCDMA2K_VAL_CHP, RFMXCDMA2K_VAL_TRUE));
	RFmxCheckWarn(RFmxCDMA2k_CHPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval ));
	RFmxCheckWarn(RFmxCDMA2k_CHPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType ));
	RFmxCheckWarn(RFmxCDMA2k_Initiate(instrumentHandle, "", ""));	

	/* Retrieve results */	

	RFmxCheckWarn(RFmxCDMA2k_CHPFetchCarrierAbsolutePower(instrumentHandle, "", timeout, &carrierAbsolutePower));

	RFmxCheckWarn(RFmxCDMA2k_CHPFetchSpectrum (instrumentHandle, "", timeout, NULL, NULL, NULL, 0, 
		&actualArraySize));
	if( actualArraySize > 0)
	{
		spectrum = (float32 *)malloc(sizeof(float32)*actualArraySize);

		if(spectrum)
		{
			RFmxCheckWarn(RFmxCDMA2k_CHPFetchSpectrum (instrumentHandle, "", timeout, &x0, &dx, spectrum, 
				actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	/* Display Results */
	printf("Carrier Absolute Power (dBm)            : %f\n", carrierAbsolutePower);

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

	printf("\nPress any key to exit");
	_getch();
	return error;
}
