//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure External Attenuation.
//4. Configure the Center Frequency directly or via Channel Number.
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure the Reference Level directly or via Auto Level.
//7. Select ACP measurement and enable traces.
//8. Configure Averaging parameters.
//9. Configure Sweep Time parameters.
//10. Configure Noise Compensation parameter.
//11. Configure Number of Offset Channels.
//12. Initiate the Measurement.
//13[A-E]. Fetch ACP Measurements and Traces.
//14. Close the RFmx session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxTDSCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096
#define NUMBER_OF_OFFSETS			2

int main ()
{
    //RFSA Configuration
	char *rfsaResourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle = NULL;    
    int32 i=0;

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0;
    int32 lastErrorCode = 0;

    float64 externalAttenuation = 0.00;                               /*(dB) */
    float64 centerFrequency = 1.91e+9;                                /*(Hz) */

    /*Frequency Reference*/
    char * frequencyReferenceSource = RFMXTDSCDMA_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e+6;                      /*(Hz) */

    /*AutoLevel*/
    int32 autoLevel = RFMXTDSCDMA_VAL_FALSE;
    float64 referenceLevel = 0.00;                                    /*(dBm) */

    /*Trigger */
    float64 triggerDelay = 0.00;                                      /*(s) */
    int32 enableTrigger = RFMXTDSCDMA_VAL_TRUE;
    char * IQPowerEdgeSource = "0";
    int32 IQPowerEdgeSlope = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_RISING_SLOPE;
    float64 IQPowerEdgeLevel = -20.00;                                /*(dB) */
    int32 IQPowerEdgeLevelType = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
    int32 minimumQuietTimeMode = RFMXTDSCDMA_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
    int32 actualArraySize = 0;
    float64 minimumQuietTime = 16E-6;                                 /*(s) */

    float64 measurementInterval = 0.005;                              /*(s) */

    /*Averaging */
    int32 averagingEnabled = RFMXTDSCDMA_VAL_ACP_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;
    int32 averagingType = RFMXTDSCDMA_VAL_ACP_AVERAGING_TYPE_RMS;

    /*Sweep Time Configuration*/
    int32 sweepTimeAuto = RFMXTDSCDMA_VAL_ACP_SWEEP_TIME_AUTO_TRUE;
    float64 sweepTimeInterval = 0.000660;           /*(s) */

    int32 noiseCompensationEnabled = RFMXTDSCDMA_VAL_ACP_NOISE_COMPENSATION_ENABLED_FALSE;
   
    /* Variables to store the results */
	float64 timeout = 10.00, x0 = 0.0, dx = 0.0;
	float64 lowerAbsolutePower[NUMBER_OF_OFFSETS] = {0};		     /*(dBm) */
    float64 upperAbsolutePower[NUMBER_OF_OFFSETS] = {0};  		     /*(dBm) */ 
    float64 lowerRelativePower[NUMBER_OF_OFFSETS] = {0};		     /*(dBm) */
    float64 upperRelativePower[NUMBER_OF_OFFSETS] = {0};		     /*(dBm) */
    float64 carrierAbsolutePower = 0.00;        			         /*(dBm) */
    float32* spectrum = NULL;

    /* Initialize a session */
    RFmxCheckWarn(RFmxTDSCDMA_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxTDSCDMA_CfgFrequencyReference(instrumentHandle, "",frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxTDSCDMA_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
    RFmxCheckWarn(RFmxTDSCDMA_CfgFrequency(instrumentHandle, "", centerFrequency));
    RFmxCheckWarn(RFmxTDSCDMA_CfgIQPowerEdgeTrigger(instrumentHandle, "", IQPowerEdgeSource, IQPowerEdgeSlope, 
		IQPowerEdgeLevel,triggerDelay, minimumQuietTimeMode, minimumQuietTime,IQPowerEdgeLevelType,enableTrigger));
    if(autoLevel)
    {
        RFmxCheckWarn(RFmxTDSCDMA_AutoLevel(instrumentHandle, "", measurementInterval, &referenceLevel));
        printf("Reference Level (dBm)        : %lf\n",referenceLevel);
    }
    else 
    {
        RFmxCheckWarn(RFmxTDSCDMA_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
    }
    RFmxCheckWarn(RFmxTDSCDMA_SelectMeasurements(instrumentHandle, "", RFMXTDSCDMA_VAL_ACP, RFMXTDSCDMA_VAL_TRUE));
    RFmxCheckWarn(RFmxTDSCDMA_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
    RFmxCheckWarn(RFmxTDSCDMA_ACPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
    RFmxCheckWarn(RFmxTDSCDMA_ACPCfgNoiseCompensationEnabled(instrumentHandle, "", noiseCompensationEnabled));
    RFmxCheckWarn(RFmxTDSCDMA_ACPCfgNumberOfOffsets(instrumentHandle, "", NUMBER_OF_OFFSETS));
    RFmxCheckWarn(RFmxTDSCDMA_Initiate(instrumentHandle, "", ""));

    /* Fetch the measurements array */
    RFmxCheckWarn(RFmxTDSCDMA_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout,
                                                                    lowerRelativePower, upperRelativePower,
                                                                    lowerAbsolutePower, upperAbsolutePower,
                                                                    NUMBER_OF_OFFSETS, NULL));
   

    RFmxCheckWarn(RFmxTDSCDMA_ACPFetchCarrierAbsolutePower(instrumentHandle, "", timeout, &carrierAbsolutePower));
    RFmxCheckWarn(RFmxTDSCDMA_ACPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
    if( actualArraySize > 0 )
    {
        spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
        if(spectrum)
        {
            RFmxCheckWarn(RFmxTDSCDMA_ACPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, 
				actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    printf("Carrier Absolute Power (dBm) : %lf\n",carrierAbsolutePower);
    for(i=0;i<NUMBER_OF_OFFSETS;i++)
    {
        printf("\nOffset                       :  %d\n", i);
        printf("Lower Relative Power (dB)    : %f\n", lowerRelativePower[i]);
        printf("Upper Relative Power (dB)    : %f\n", upperRelativePower[i]);
        printf("Lower Absolute Power (dBm)   : %f\n", lowerAbsolutePower[i]);
        printf("Upper Absolute Power (dBm)   : %f\n", upperAbsolutePower[i]);
        printf("-------------------------------------------------\n");
    }

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
