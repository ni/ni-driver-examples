//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Select CHP measurement and enable Traces.
//7. Configure Sweep Time Parameters.
//8. Configure Averaging Parameters for CHP measurement.
//9. Initiate the Measurement.
//10[A-B]. Fetch CHP Measurements and Traces.
//11. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION           4096

int main (int argc, char *argv[])
{
    char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;
    
    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0, lastErrorCode = 0;

    char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;                                               /*(Hz) */

    float64 centerFrequency = 1.95e9;                                       /*(Hz) */
    float64 referenceLevel = 0.0;                                           /*(dBm) */
    float64 externalAttenuation = 0.0;                                      /*(dB) */

    int32 enableTrigger = RFMXLTE_VAL_FALSE;
    char * digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
    int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
    float64 triggerDelay = 0.0;                                             /*(s) */

    float64 componentCarrierBandwidth = 200e3; /*(Hz) */

    int32 sweepTimeAuto = RFMXLTE_VAL_CHP_SWEEP_TIME_AUTO_TRUE;
    float64 sweepTimeInterval = 0.001;                                      /*(s) */

    int32 averagingEnabled = RFMXLTE_VAL_CHP_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;
    int32 averagingType = RFMXLTE_VAL_CHP_AVERAGING_TYPE_RMS;

    float64 timeout = 10.0;                                                 /*(s) */
    float64 absolutePower = 0.0;                                            /*(dBm) */
    float64 relativePower = 0.0;                                            /*(dB) */
    int32 actualArraySize = 0;
    float64 x0 = 0.0,dx = 0.0;
    float32 *spectrum = NULL;                                               /*(dBm) */

	/* Initialize a session */
	RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, 
												frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, 
												enableTrigger));
    
	RFmxCheckWarn(RFmxLTE_CfgComponentCarrier(instrumentHandle, "", componentCarrierBandwidth, 0.0, 0));
	RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_CHP, RFMXLTE_VAL_TRUE));
	RFmxCheckWarn(RFmxLTE_CHPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
	RFmxCheckWarn(RFmxLTE_CHPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
	RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

	RFmxCheckWarn(RFmxLTE_CHPFetchComponentCarrierMeasurement(instrumentHandle, "", timeout, &absolutePower, &relativePower));
	RFmxCheckWarn(RFmxLTE_CHPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{	
		spectrum = (float32 *) malloc(sizeof(float32) * actualArraySize);
		if( spectrum )
		{	
			RFmxCheckWarn(RFmxLTE_CHPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

    printf("Carrier Absolute Power  (dBm)    : %lf\n",absolutePower);

Error:
    if( error )
    {
        RFmxLTE_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
        if (error < 0)
            printf("ERROR: %s\n", errorMessage);
        else
            printf("WARNING: %s\n", errorMessage);
    }

    if(instrumentHandle)
    {
        RFmxLTE_Close(instrumentHandle, RFMXLTE_VAL_FALSE);
    }

    /* Free allocated memory */
    if (spectrum)
    {
        free(spectrum);
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}
