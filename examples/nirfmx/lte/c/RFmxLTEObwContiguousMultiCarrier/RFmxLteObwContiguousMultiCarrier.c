//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure Component Carriers.
//7. Select OBW measurement and enable Traces.
//8. Configure Sweep Time Parameters.
//9. Configure Averaging Parameters for OBW measurement.
//10. Initiate the Measurement.
//11[A-B]. Fetch OBW Measurements and Traces.
//12. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION               4096

#define NUMBER_OF_COMPONENT_CARRIERS        2

int main (int argc, char *argv[])
{
    char *resourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle = NULL;

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0, lastErrorCode = 0;

    float64 centerFrequency = 1.95e9;                                   /*(Hz) */
    float64 referenceLevel = 0.0;                                       /*(dBm) */
    float64 externalAttenuation = 0.0;                                  /*(dB) */
    int32 enableTrigger = RFMXLTE_VAL_FALSE;

    char *digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
    int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;

    float64 triggerDelay = 0.0;                                         /*(s) */

	int32 linkDirection = RFMXLTE_VAL_LINK_DIRECTION_UPLINK;
    int32 componentCarrierSpacingType = RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL;
    int32 componentCarrierAtCenterFrequency = -1;
    char *frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;                         /*(Hz) */

    int32 numberOfComponentCarriers = NUMBER_OF_COMPONENT_CARRIERS;
	float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS] = {20e6, 20e6};    /*(Hz) */
	float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS] = {-9.9e6, 9.9e6};    /*(Hz) */

    int32 sweepTimeAuto = RFMXLTE_VAL_OBW_SWEEP_TIME_AUTO_TRUE;
    float64 sweepTimeInterval = 0.001;/*(s) */

    int32 averagingEnabled = RFMXLTE_VAL_OBW_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;
    int32 averagingType = RFMXLTE_VAL_OBW_AVERAGING_TYPE_RMS;

    float64 timeout = 10.0;                                             /*(s) */

    float64 stopFrequency = 0.0;                                        /*(Hz) */
    float64 startFrequency = 0.0;                                       /*(Hz) */
    float64 occupiedBandwidth = 0.0;                                    /*(Hz) */
    float64 absolutePower = 0.0;                                        /*(dBm) */

    int32 actualArraySize = 0;
    float64 x0=0.0,dx=0.0;
    float32 *spectrum = NULL;                                           /*(dBm) */

	RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    
	RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource,
												digitalEdge, triggerDelay, enableTrigger));

    RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, "", componentCarrierSpacingType, 
		componentCarrierAtCenterFrequency));
    RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, "", numberOfComponentCarriers));
    RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, "", 
													componentCarrierBandwidth, 
													componentCarrierFrequency, NULL, numberOfComponentCarriers));
	RFmxCheckWarn(RFmxLTE_CfgLinkDirection(instrumentHandle, "", linkDirection));
	RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_OBW, RFMXLTE_VAL_TRUE));
    RFmxCheckWarn(RFmxLTE_OBWCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
    RFmxCheckWarn(RFmxLTE_OBWCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
    RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

    RFmxCheckWarn(RFmxLTE_OBWFetchMeasurement(instrumentHandle, "", timeout, &occupiedBandwidth, &absolutePower,
                                                &startFrequency, &stopFrequency));

    RFmxCheckWarn(RFmxLTE_OBWFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
    if( actualArraySize > 0 )
    {
        spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
        if(spectrum)
        {
            RFmxCheckWarn(RFmxLTE_OBWFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

	printf("---------- Measurement ----------\n");

    printf("Occupied Bandwidth (Hz)	: %lf\n",occupiedBandwidth);
    printf("Absolute Power (dBm)	: %lf\n",absolutePower);
	printf("Start Frequency (Hz)	: %lf\n",startFrequency);
	 printf("Stop Frequency (Hz)	: %lf\n",stopFrequency);
	
	

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
