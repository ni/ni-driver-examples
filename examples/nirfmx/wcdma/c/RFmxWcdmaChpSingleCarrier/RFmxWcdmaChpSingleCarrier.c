// Steps:
// 1. Open a new RFmx Session.
// 2. Configure Frequency Reference.
// 3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
// 4. Configure Trigger Type and Trigger Parameters.
// 5. Select CHP measurement and enable Traces.
// 6. Configure Sweep Time Parameters.
// 7. Configure Averaging Parameters for CHP measurement.
// 8. Initiate the Measurement.
// 9. Fetch CHP Measurements and Traces.
// 10. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWCDMA.h"


/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main (int argc, char *argv[])
{
	char *rfsaResourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle = NULL;  

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0;
    int32 lastErrorCode = 0;

	char * frequencyReferenceSource = RFMXWCDMA_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;/*(Hz) */

    float64 centerFrequency = 1.95e9;/*(Hz) */
    float64 referenceLevel = 0.000000;/*(dBm) */
    float64 externalAttenuation = 0.000000;/*(dB) */

    int32 enableTrigger = RFMXWCDMA_VAL_FALSE;
    char * digitalEdgeSource = RFMXWCDMA_VAL_PFI0_STR;
	int32 digitalEdge = RFMXWCDMA_VAL_DIGITAL_EDGE_RISING_EDGE; /*ring*/
    float64 triggerDelay = 0.000000;/*(s) */

    int32 sweepTimeAuto = RFMXWCDMA_VAL_CHP_SWEEP_TIME_AUTO_TRUE; /*ring*/
    float64 sweepTimeInterval = 0.000667;/*(s) */
    int32 averagingEnabled = RFMXWCDMA_VAL_CHP_AVERAGING_ENABLED_FALSE; /*ring*/
    int32 averagingCount = 10;
    int32 averagingType = RFMXWCDMA_VAL_CHP_AVERAGING_TYPE_RMS; /*ring*/

    char * resultName = "";
    float64 timeout = 10.000000;/*(s) */
    float64 absolutePower = 0.000000;/*(dBm) */
    float64 relativePower = 0.000000;/*(dB) */
    
	int32 actualArraySize = 0;
	float64 x0=0.0,dx=0.0;
    float32 *spectrum = NULL;/*(dBm) */


    /* Initialize a session */
    RFmxCheckWarn(RFmxWCDMA_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));

    //Call RFmxInstr Initialize NIRFSA.vi
    RFmxCheckWarn(RFmxWCDMA_CfgFrequencyReference(instrumentHandle, "",frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxWCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxWCDMA_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
    RFmxCheckWarn(RFmxWCDMA_SelectMeasurements(instrumentHandle, "", RFMXWCDMA_VAL_CHP, RFMXWCDMA_VAL_TRUE));
    RFmxCheckWarn(RFmxWCDMA_CHPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
    RFmxCheckWarn(RFmxWCDMA_CHPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
    RFmxCheckWarn(RFmxWCDMA_Initiate(instrumentHandle, "", resultName));

    RFmxCheckWarn(RFmxWCDMA_CHPFetchCarrierMeasurement(instrumentHandle, "", timeout, &absolutePower, &relativePower));
    

	RFmxCheckWarn(RFmxWCDMA_CHPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, 
										   &actualArraySize));
	if( actualArraySize > 0 )
	{
		spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if(spectrum)
		{
				RFmxCheckWarn(RFmxWCDMA_CHPFetchSpectrum(instrumentHandle, "", timeout, 
					&x0, &dx, spectrum, actualArraySize, NULL));	
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}
    
    printf("Carrier Absolute Power (dBm): %f \n",absolutePower);


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

    if (spectrum)
    {
        free(spectrum);
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}
