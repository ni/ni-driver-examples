//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select CHP measurement and enable traces.
//6. Configure Sweep Time Parameters.
//7. Configure Averaging Parameters.
//8. Initiate the Measurement.
//9. Fetch CHP Measurements and Traces.
//10. Close the RFmx session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxTDSCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main ()
{
    //RFSA Configuration
	char *rfsaResourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle = NULL;    

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0,lastErrorCode=0;
    int32 actualArraySize = 0;

    float64 referenceLevel = 0.00;          /*(dBm) */
    float64 externalAttenuation = 0.00;     /*(dB) */

    /*Frequency Reference*/
    char * frequencyReferenceSource = RFMXTDSCDMA_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e+6;                  /*(Hz) */
    float64 centerFrequency = 1.91e+9;                            /*(Hz) */

    /*Trigger */
    float64 triggerDelay = 0.00;                                  /*(s) */
    int32 enableTrigger = RFMXTDSCDMA_VAL_TRUE;
    char * IQPowerEdgeSource = "0";
    int32 IQPowerEdgeSlope = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_RISING_SLOPE;
    float64 IQPowerEdgeLevel = -20.00;                                                   /*(dB) */
    int32 minimumQuietTimeMode = RFMXTDSCDMA_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
    float64 minimumQuietTime = 16E-6;                                                    /*(s) */
    int32 IQPowerEdgeLevelType = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;

    /*Sweep Time Configuration*/
    int32 sweepTimeAuto = RFMXTDSCDMA_VAL_CHP_SWEEP_TIME_AUTO_TRUE;
    float64 sweepTimeInterval = 660E-6;                                                  /*(s) */

    /* Averaging */
    int32 averagingEnabled = RFMXTDSCDMA_VAL_CHP_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;
    int32 averagingType = RFMXTDSCDMA_VAL_CHP_AVERAGING_TYPE_RMS;
    float64 timeout = 10.00;                                                             /*(s) */

    /* Variables to store the results */
    float64 carrierAbsolutePower = 0.00, x0 = 0.0, dx = 0.0;    /*(dBm) */
    float32* spectrum = (float32*)NULL;                         /*(dBm) */

    /* Initialize a session */
    RFmxCheckWarn(RFmxTDSCDMA_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxTDSCDMA_CfgFrequencyReference(instrumentHandle,"", frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxTDSCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxTDSCDMA_CfgIQPowerEdgeTrigger(instrumentHandle, "", IQPowerEdgeSource, IQPowerEdgeSlope,
		IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime, IQPowerEdgeLevelType, enableTrigger));
    RFmxCheckWarn(RFmxTDSCDMA_SelectMeasurements(instrumentHandle, "",RFMXTDSCDMA_VAL_CHP , RFMXTDSCDMA_VAL_TRUE));
    RFmxCheckWarn(RFmxTDSCDMA_CHPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
    RFmxCheckWarn(RFmxTDSCDMA_CHPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
    RFmxCheckWarn(RFmxTDSCDMA_Initiate(instrumentHandle, "", ""));

	/* Fetch Results */
    RFmxCheckWarn(RFmxTDSCDMA_CHPFetchCarrierAbsolutePower(instrumentHandle, "", timeout, &carrierAbsolutePower));
    RFmxCheckWarn(RFmxTDSCDMA_CHPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, NULL, 0, &actualArraySize));
    if(actualArraySize > 0)
    {
        spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
        if(spectrum)
        {
            RFmxCheckWarn(RFmxTDSCDMA_CHPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, 
				actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    printf("Carrier Absolute Power (dBm)    : %lf\n", carrierAbsolutePower);

Error:
    if( error )
    {
        RFmxTDSCDMA_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
        if (error < 0)
            printf("ERROR: %s\n", errorMessage);
        else
            printf("WARNING: %s\n", errorMessage);
    }
    if(instrumentHandle)
    {
        RFmxTDSCDMA_Close(instrumentHandle, RFMXTDSCDMA_VAL_FALSE);
    }

    if (spectrum)
    {
        free(spectrum);
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}
