// Steps:
// 1. Open a new RFmx Session.
// 2. Configure Frequency Reference.
// 3. Configure basic signal properties (Center Frequency, RF Attenuation and External Attenuation)
// 4. Configure Trigger Type and Trigger Parameters.
// 5. Configure Reference Level.
// 6. Select ACP measurement and enable Traces.
// 7. Configure Measurement Method.
// 8. Configure Averaging Parameters for ACP measurement.
// 9. Configure Sweep Time Parameters.
// 10. Configure Noise Compensation Parameter.
// 11. Configure Number of offset channels. 
// 12. Initiate the Measurement.
// 13. Fetch ACP Measurements and Traces.
// 14. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxWCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096
#define NUMBER_OF_OFFSETS			2

int main (int argc, char *argv[])
{
    //RFSA Configuration	
    char *rfsaResourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle = NULL;

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0;
    int32 lastErrorCode = 0;
	int i = 0;

	int32 enableAllTraces = RFMXWCDMA_VAL_TRUE;
    int32 measurement = RFMXWCDMA_VAL_ACP;
   
	char * frequencyReferenceSource = RFMXWCDMA_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;/*(Hz) */

    float64 centerFrequency = 1.95e9 ;/*(Hz) */
    float64 externalAttenuation = 0.000000;/*(dB) */

	int32 RFAttenuationAuto =  RFMXWCDMA_VAL_RF_ATTENUATION_AUTO_TRUE; 
    float64 RFAttenuation = 10.0;/*(dB) */

    int32 enableTrigger = RFMXWCDMA_VAL_FALSE;
    char * digitalEdgeSource = RFMXWCDMA_VAL_PFI0_STR;
	int32 digitalEdge = RFMXWCDMA_VAL_DIGITAL_EDGE_RISING_EDGE; /*ring*/
    float64 triggerDelay = 0.000000;/*(s) */

    float64 measurementInterval = 0.010000;/*(s) */
    float64 referenceLevel = 0.000000;/*(dBm) */
    int32 measurementMethod = RFMXWCDMA_VAL_ACP_MEASUREMENT_METHOD_NORMAL; /*ring*/
    int32 averagingEnabled = RFMXWCDMA_VAL_ACP_AVERAGING_ENABLED_FALSE; /*ring*/
    int32 averagingCount = 10;
    int32 averagingType = RFMXWCDMA_VAL_ACP_AVERAGING_TYPE_RMS; /*ring*/

	int32 autoLevel = RFMXWCDMA_VAL_TRUE;

    int32 sweepTimeAuto = RFMXWCDMA_VAL_ACP_SWEEP_TIME_AUTO_TRUE; /*ring*/
    float64 sweepTimeInterval = 0.000667;/*(s) */
    int32 noiseCompensationEnabled = RFMXWCDMA_VAL_ACP_NOISE_COMPENSATION_ENABLED_FALSE; /*ring*/
   
	float64 timeout = 10.000000;								/*(s) */

    float64 lowerAbsolutePower[NUMBER_OF_OFFSETS] = {0};		/*(dBm) */ 
    float64 upperAbsolutePower[NUMBER_OF_OFFSETS] = {0};		/*(dBm) */  
    float64 lowerRelativePower[NUMBER_OF_OFFSETS] = {0};		/*(dB) */ 
    float64 upperRelativePower[NUMBER_OF_OFFSETS] = {0};		/*(dB) */ 

    float64 absolutePower = 0.000000;							/*(dBm) */
    float64 relativePower = 0.000000;							/*(dB) */

	int32 actualArraySize = 0;
	float64 x0=0.0,dx=0.0;
    float32 *spectrum = NULL;									/*(dBm) */

    /* Initialize a session */
    RFmxCheckWarn(RFmxWCDMA_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));

    //Call RFmxInstr Initialize NIRFSA.vi
    RFmxCheckWarn(RFmxWCDMA_CfgFrequencyReference(instrumentHandle, "",frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxWCDMA_CfgFrequency(instrumentHandle, "", centerFrequency));
    RFmxCheckWarn(RFmxWCDMA_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
    RFmxCheckWarn(RFmxWCDMA_CfgRFAttenuation(instrumentHandle, "", RFAttenuationAuto, RFAttenuation));
    RFmxCheckWarn(RFmxWCDMA_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
    if(autoLevel)
    {
        RFmxCheckWarn(RFmxWCDMA_AutoLevel(instrumentHandle, "", measurementInterval, &referenceLevel));
		printf("Reference level (dBm)          : %f\n", referenceLevel);
    }
    else
	{
        RFmxCheckWarn(RFmxWCDMA_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
    }
    RFmxCheckWarn(RFmxWCDMA_SelectMeasurements(instrumentHandle, "", measurement, enableAllTraces));
    RFmxCheckWarn(RFmxWCDMA_ACPCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
    RFmxCheckWarn(RFmxWCDMA_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
    RFmxCheckWarn(RFmxWCDMA_ACPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
    RFmxCheckWarn(RFmxWCDMA_ACPCfgNoiseCompensationEnabled(instrumentHandle, "", noiseCompensationEnabled));
    RFmxCheckWarn(RFmxWCDMA_ACPCfgNumberOfOffsets(instrumentHandle, "", NUMBER_OF_OFFSETS));
    RFmxCheckWarn(RFmxWCDMA_Initiate(instrumentHandle, "", ""));
    
	/* Fetch the measurements array */
	RFmxCheckWarn(RFmxWCDMA_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, lowerRelativePower, upperRelativePower, lowerAbsolutePower, upperAbsolutePower, NUMBER_OF_OFFSETS, NULL));
		
	RFmxCheckWarn(RFmxWCDMA_ACPFetchCarrierMeasurement(instrumentHandle, "", timeout, &absolutePower, &relativePower));
    
	RFmxCheckWarn(RFmxWCDMA_ACPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, 
										   &actualArraySize));
	if( actualArraySize > 0 )
	{
		spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if(spectrum)
		{
				RFmxCheckWarn(RFmxWCDMA_ACPFetchSpectrum(instrumentHandle, "", timeout, 
					&x0, &dx, spectrum, actualArraySize, NULL));	
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}
    
	printf("\nCarrier Absolute Power  (dBm)  : %f\n", absolutePower);
	
	printf("\nOffset Channel Measurements: \n");
	for(i=0;i<NUMBER_OF_OFFSETS;i++)
	{
		printf("Offset  :  %d\n", i);
		printf("Lower Relative Power (dB)      : %f\n", lowerRelativePower[i]);
		printf("Upper Relative Power (dB)      : %f\n", upperRelativePower[i]);
		printf("Lower Absolute Power (dBm)     : %f\n", lowerAbsolutePower[i]);
		printf("Upper Absolute Power (dBm)     : %f\n", upperAbsolutePower[i]);
		printf("-------------------------------------------------\n");
	}

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
