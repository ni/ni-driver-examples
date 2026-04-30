//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5[A - F].Configure Subblock Configurations.
//5A.Configure Number of Subblocks.
//5B.Configure Subblock Frequency.
//5D.Configure Component Carrier Spacing.
//5E.Configure Number of Component Carriers.
//5F.Configure Component Carriers(Component Carrier Frequency and Component Carrier Bandwidth).
//6. Select CHP measurement and enable Traces.
//7. Configure Sweep Time Parameters.
//8. Configure Averaging Parameters for CHP measurement.
//9. Initiate the Measurement.
//10. Fetch CHP Measurements and Traces.
//11. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION               4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                 256

#define NUMBER_OF_SUBBLOCKS                 2
#define NUMBER_OF_COMPONENT_CARRIERS        1

/* Input: Subblock inputs structure */
typedef struct 
{
    float64 subblockFrequency;                                          /*(Hz) */
    int32 componentCarrierSpacingType;
    int32 componentCarrierAtCenterFrequency;
    float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS];    /*(Hz) */
    float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS];    /*(Hz) */
}subblockInputs_t;

/* Input: Subblock measurement outputs structure */
typedef struct 
{
    float64 subblockPower;                      /*(dBm) */
    float64 integrationBandwidth;               /*(Hz) */
    float64 frequency;                          /*(Hz) */
    float64 absolutePower[NUMBER_OF_COMPONENT_CARRIERS];                     /*(dBm) */
    float64 relativePower[NUMBER_OF_COMPONENT_CARRIERS];                     /*(dB) */
}subblockMeasurements_t;

int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle = NULL;

    char subblockString[MAX_SELECTOR_STRING];    

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0, lastErrorCode = 0;
    int i = 0, j = 0;

    char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;                    /*(Hz) */

    float64 centerFrequency = 1.95e9;                              /*(Hz) */
    float64 referenceLevel = 0.0;                /*(dBm) */
    float64 externalAttenuation = 0.0;           /*(dB) */

    int32 enableTrigger = RFMXLTE_VAL_FALSE;
    char * digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
    int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
    float64 triggerDelay = 0.0;                 /*(s) */

	subblockInputs_t subblocks[NUMBER_OF_SUBBLOCKS] = {					/*  Set up subblock 0 inputs  */
															{	
															0.0,													/* subblockFrequency */	
															RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL, 
															-1,														/* componentCarrierAtCenterFrequency */
															{20e6},													/* componentCarrierBandwidth */
															{0.0}													/* componentCarrierFrequency */
														},
													    {				/*  Set up subblock 1 inputs  */
															30e6,													/* subblockFrequency */	
															RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL, 
															-1,														/* componentCarrierAtCenterFrequency */
															{20e6},													/* componentCarrierBandwidth */
															{0.0}													/* componentCarrierFrequency */
														}
													};

    int32 sweepTimeAuto = RFMXLTE_VAL_CHP_SWEEP_TIME_AUTO_TRUE;
    float64 sweepTimeInterval = 0.001;          /*(s) */

    int32 averagingEnabled = RFMXLTE_VAL_CHP_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;
    int32 averagingType = RFMXLTE_VAL_CHP_AVERAGING_TYPE_RMS;

    float64 timeout = 10.0;                     /*(s) */

    subblockMeasurements_t subblocksMsr[NUMBER_OF_SUBBLOCKS];

    float64 totalAggregatedPower = 0.0;         /*(dBm) */

    int32 actualArraySize = 0;
    float64 x0 = 0.0 ,dx = 0.0;
    float32 *spectrum = NULL;                   /*(dBm) */
    
    
    /* Set up subblock outputs */
    for( i = 0; i < NUMBER_OF_SUBBLOCKS; i++ )
    {
		 for( j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++ )
		{
			subblocksMsr[i].absolutePower[j] = 0;
			subblocksMsr[i].relativePower[j] = 0;
		}
    }

    /* Initialize a session */
    RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, 
												enableTrigger));
    RFmxCheckWarn(RFmxLTE_CfgNumberOfSubblocks(instrumentHandle, "", NUMBER_OF_SUBBLOCKS));
    for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
    {
        RFmxCheckWarn(RFmxLTE_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString));
        RFmxCheckWarn(RFmxLTE_SetSubblockFrequency(instrumentHandle, subblockString, subblocks[i].subblockFrequency));
        RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, subblockString,
                                                            subblocks[i].componentCarrierSpacingType,
                                                            subblocks[i].componentCarrierAtCenterFrequency));
        RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, subblockString, NUMBER_OF_COMPONENT_CARRIERS));
        RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, subblockString,
                                                            subblocks[i].componentCarrierBandwidth,
                                                            subblocks[i].componentCarrierFrequency,
                                                            NULL, NUMBER_OF_COMPONENT_CARRIERS));
    }
    RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_CHP, RFMXLTE_VAL_TRUE));
    RFmxCheckWarn(RFmxLTE_CHPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
    RFmxCheckWarn(RFmxLTE_CHPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
    RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

    for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
    {
        RFmxCheckWarn(RFmxLTE_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString));
        RFmxCheckWarn(RFmxLTE_CHPFetchSubblockMeasurement(instrumentHandle, subblockString, timeout,
                                                            &subblocksMsr[i].subblockPower,
                                                            &subblocksMsr[i].integrationBandwidth,
                                                            &subblocksMsr[i].frequency));
      /* Fetch the measurements array */
      RFmxCheckWarn(RFmxLTE_CHPFetchComponentCarrierMeasurementArray(instrumentHandle, subblockString, timeout,
                                                                                subblocksMsr[i].absolutePower,
                                                                                subblocksMsr[i].relativePower,
                                                                                NUMBER_OF_COMPONENT_CARRIERS, NULL));
      
    }
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

    printf("Total Aggregated Power  (dBm)   : %lf\n",totalAggregatedPower);

    printf("\n---------------Subblock Measurements--------------- \n");
    for( i = 0; i < NUMBER_OF_SUBBLOCKS; i++ )
    {
        printf("Subblock  : %d\n", i);
        printf("Subblock Power (dBm)            : %lf\n",subblocksMsr[i].subblockPower);
        printf("Integration Bandwidth (Hz)      : %lf\n",subblocksMsr[i].integrationBandwidth);
        printf("Frequency (Hz)                  : %lf\n",subblocksMsr[i].frequency);
        printf("------Component Carrier Measurements------ \n");
        for( j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++ )
        {
            printf("Carrier  : %d\n", j);
            printf("Absolute Power (dBm)            : %f\n", subblocksMsr[i].absolutePower[j]);
            printf("Relative Power (dB)             : %f\n", subblocksMsr[i].relativePower[j]);
            printf("----------------------------------------------\n");
        }
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

    /* Free Allocated Memory */
    if (spectrum)
    {
        free(spectrum);
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}
