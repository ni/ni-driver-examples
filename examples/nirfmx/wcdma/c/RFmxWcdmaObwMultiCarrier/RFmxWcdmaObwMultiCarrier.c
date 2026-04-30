// Steps:
// 1. Open a new RFmx Session.
// 2. Configure Frequency Reference.
// 3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
// 4. Configure Trigger Type and Trigger Parameters.
// 5. Configure Contiguous Carriers
// 6. Select OBW measurement and enable Traces.
// 7. Configure Sweep Time Parameters.
// 8. Configure Averaging Parameters for OBW measurement.
// 9. Initiate the Measurement.
// 10. Fetch OBW Measurements and Traces.
// 11. Close RFmx Session. 

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

	float64 centerFrequency = 1.95e9;	/*(Hz) */
    float64 referenceLevel = 0.000000;				/*(dBm) */
    float64 externalAttenuation = 0.000000;			/*(dB) */
   
    char * frequencyReferenceSource = RFMXWCDMA_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;			/*(Hz) */

    int32 enableTrigger = RFMXWCDMA_VAL_FALSE;
    char * digitalEdgeSource = RFMXWCDMA_VAL_PFI0_STR;
    int32 digitalEdge = RFMXWCDMA_VAL_DIGITAL_EDGE_RISING_EDGE;
    float64 triggerDelay = 0.000000;				/*(s) */

    int32 numberOfCarriers = 2;
	int32 carrierAtCenterFrequency = -1;
    
	int32 sweepTimeAuto = RFMXWCDMA_VAL_OBW_SWEEP_TIME_AUTO_TRUE;
    float64 sweepTimeInterval = 0.000667;			/*(s) */

    int32 averagingEnabled = RFMXWCDMA_VAL_OBW_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;
    int32 averagingType = RFMXWCDMA_VAL_OBW_AVERAGING_TYPE_RMS;

	float64 timeout = 10.000000;					/*(s) */
	int32 actualArraySize = 0;
	float64 x0 = 0.0, dx = 0.0;

    float64 stopFrequency = 0.000000;				/*(Hz) */
    float64 startFrequency = 0.000000;				/*(Hz) */
    float64 occupiedBandwidth = 0.000000;			/*(Hz) */
    float64 absolutePower = 0.000000;				/*(dBm) */

	float32* spectrum = NULL;						/*(dBm) */
  
    /* Initialize a session */
    RFmxCheckWarn(RFmxWCDMA_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxWCDMA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxWCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxWCDMA_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
    RFmxCheckWarn(RFmxWCDMA_CfgContiguousCarriers(instrumentHandle, "", numberOfCarriers, carrierAtCenterFrequency));
    RFmxCheckWarn(RFmxWCDMA_SelectMeasurements(instrumentHandle, "", RFMXWCDMA_VAL_OBW, RFMXWCDMA_VAL_TRUE));
    RFmxCheckWarn(RFmxWCDMA_OBWCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
    RFmxCheckWarn(RFmxWCDMA_OBWCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
    RFmxCheckWarn(RFmxWCDMA_Initiate(instrumentHandle, "", ""));

    RFmxCheckWarn(RFmxWCDMA_OBWFetchMeasurement(instrumentHandle, "", timeout, &occupiedBandwidth, &absolutePower, 
													&startFrequency, &stopFrequency));
    RFmxCheckWarn(RFmxWCDMA_OBWFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		spectrum = (float32 *) malloc(sizeof(float32) * actualArraySize);
		if( spectrum )
		{
			RFmxCheckWarn(RFmxWCDMA_OBWFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}
    
	printf("------------Measurement------------\n");
	printf("Occupied Bandwidth (Hz)	: %lf\n",occupiedBandwidth);
	printf("Absolute Power (dBm)	: %lf\n",absolutePower);
	printf("Start Frequency (Hz)	: %lf\n",startFrequency);
    printf("Stop Frequency (Hz)	: %lf\n",stopFrequency);

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
    if (spectrum)
    {
        free(spectrum);
    }

    return error;
}
