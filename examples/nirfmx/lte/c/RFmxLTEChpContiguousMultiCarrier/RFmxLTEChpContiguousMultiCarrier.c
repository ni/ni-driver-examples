//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Component Carrier Spacing.
//6. Configure Component Carriers.
//7. Select CHP measurement and enable Traces.
//8. Configure Sweep Time Parameters.
//9. Configure Averaging Parameters for CHP measurement.
//10. Initiate the Measurement.
//11[A-C]. Fetch CHP Measurements and Traces.
//16. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

#define NUMBER_OF_COMPONENT_CARRIERS        2

int main (int argc, char *argv[])
{
    char *resourceName = "RFSA";

	niRFmxInstrHandle instrumentHandle = NULL;
	
    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0, lastErrorCode = 0;
    int i = 0;

	char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;								/*(Hz) */

    float64 centerFrequency = 1.95e9;                           /*(Hz) */
    float64 referenceLevel = 0.0;                               /*(dBm) */
    float64 externalAttenuation = 0.0;                          /*(dB) */

    int32 enableTrigger = RFMXLTE_VAL_FALSE;
    char * digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
    int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
    float64 triggerDelay = 0.0;                                 /*(s) */

    int32 componentCarrierSpacingType = RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL;
    int32 componentCarrierAtCenterFrequency = -1;
    
	float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS] = {20e6, 20e6};
	float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS] = {-9.9e6, 9.9e6};

    int32 sweepTimeAuto = RFMXLTE_VAL_CHP_SWEEP_TIME_AUTO_TRUE;
    float64 sweepTimeInterval = 0.001;                          /*(s) */

    int32 averagingEnabled = RFMXLTE_VAL_CHP_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;
    int32 averagingType = RFMXLTE_VAL_CHP_AVERAGING_TYPE_RMS;

    float64 timeout = 10.0;												/*(s) */
    float64 absolutePower[NUMBER_OF_COMPONENT_CARRIERS] = {0};          /*(dBm) */
    float64 relativePower[NUMBER_OF_COMPONENT_CARRIERS] = {0};          /*(dB) */

    float64 totalAggregatedPower = 0.0;									/*(dBm) */

    float64 x0 = 0.0, dx = 0.0;
    float32 *spectrum = NULL;											/*(dBm) */

    int32 actualArraySize = 0;

	/* Initialize a session */
	RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, 
												enableTrigger));
	RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, "", componentCarrierSpacingType, 
		componentCarrierAtCenterFrequency));
	RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, "", NUMBER_OF_COMPONENT_CARRIERS));
	RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, "", componentCarrierBandwidth, componentCarrierFrequency, 
		NULL, NUMBER_OF_COMPONENT_CARRIERS));
	RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_CHP, RFMXLTE_VAL_TRUE));
	RFmxCheckWarn(RFmxLTE_CHPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
	RFmxCheckWarn(RFmxLTE_CHPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
	RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

	
    RFmxCheckWarn(RFmxLTE_CHPFetchComponentCarrierMeasurementArray(instrumentHandle, "", timeout, absolutePower, relativePower, 
				NUMBER_OF_COMPONENT_CARRIERS, NULL));
		

    RFmxCheckWarn(RFmxLTE_CHPFetchTotalAggregatedPower(instrumentHandle, "", timeout, &totalAggregatedPower));
    
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

    printf("Total Aggregated Power(dBm) : %lf\n",totalAggregatedPower);

    printf("\nComponent Carrier Measurements: \n");
    printf("-------------------------------------------------\n");
    for( i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++ )
    {
        printf("Carrier  :  %d\n", i);
        printf("Absolute Power (dBm)        : %lf\n", absolutePower[i]);
        printf("Relative Power (dB)         : %lf\n", relativePower[i]);
        printf("-------------------------------------------------\n");
    }

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
