//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level, External Attenuation and RF Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Duplex Mode.
//6[A-E]. Configure Subblock Parameters.
//6A. Configure Number of Subblocks.
//6B. Configure subblock Frequency.
//6C. Configure Component Carrier Spacing.
//6D. Configure Number of Component Carriers.
//6E. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
//7. Select ACP measurement and enable Traces.
//8. Configure Measurement Method.
//9. Configure Averaging Parameters for ACP measurement.
//10. Configure Sweep Time Parameters.
//11. Configure Noise Compensation Parameter.
//12. Initiate the Measurement.
//13[A-F]. Fetch ACP Measurements and Traces.
//14. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                     256

#define NUMBER_OF_SUBBLOCKS                     2
#define NUMBER_OF_COMPONENT_CARRIERS            1

/* Subblock inputs structure */
typedef struct 
{
    float64 subblockFrequency;                                          /*(Hz) */
    int32 componentCarrierSpacingType;
    int32 componentCarrierAtCenterFrequency;
    float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS];    /*(Hz) */
    float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS];    /*(Hz) */
}subblockInput_t;

/* Subblock measurement outputs structure */
typedef struct 
{
    float64 subblockPower;                                  /*(dBm) */
    float64 integrationBandwidth;                           /*(Hz) */
    float64 frequency;                                      /*(Hz) */ 
    int32 numberOfOffsets;
    float64* lowerAbsolutePower;                            /*(dBm) */
    float64* upperAbsolutePower;                            /*(dBm) */
    float64* lowerRelativePower;                            /*(dB) */
    float64* upperRelativePower;                            /*(dB) */
}subblockMeasurement_t;


int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle = NULL;

    char subblockString[MAX_SELECTOR_STRING];

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0, lastErrorCode = 0;
    int i = 0, j = 0;    

    char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;             /*(Hz) */

    float64 centerFrequency = 1.95e9;                       /*(Hz) */
    float64 referenceLevel = 0.0;                           /*(dBm) */
    float64 externalAttenuation = 0.0;                      /*(dB) */

    int32 RFAttenuationAuto = RFMXLTE_VAL_RF_ATTENUATION_AUTO_TRUE;
    float64 RFAttenuation = 10.0;                           /*(dB) */

    int32 enableTrigger = RFMXLTE_VAL_FALSE;
    char * digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
    int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
    float64 triggerDelay = 0.0;                             /*(s) */

    int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_FDD;
    int32 uplinkDownlinkConfiguraiton = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;

	int32 linkDirection = RFMXLTE_VAL_LINK_DIRECTION_UPLINK;

	subblockInput_t subblockInput[NUMBER_OF_SUBBLOCKS] = {					/*  Set up subblock 0 inputs  */
															{	0.0,													/* subblockFrequency */	
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

    int32 averagingEnabled = RFMXLTE_VAL_ACP_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;
    int32 averagingType = RFMXLTE_VAL_ACP_AVERAGING_TYPE_RMS;

    int32 sweepTimeAuto = RFMXLTE_VAL_ACP_SWEEP_TIME_AUTO_TRUE;
    float64 sweepTimeInterval = 0.001;                      /*(s) */

    int32 measurementMethod = RFMXLTE_VAL_ACP_MEASUREMENT_METHOD_NORMAL;
    int32 noiseCompensationEnabled = RFMXLTE_VAL_ACP_NOISE_COMPENSATION_ENABLED_FALSE;

    float64 timeout = 10.0;                                 /*(s) */

    subblockMeasurement_t subblocksMsr[NUMBER_OF_SUBBLOCKS];

    float64 totalAggregatedPower = 0.0;                     /*(dBm) */

    int32 actualArraySize = 0;
    float64 x0=0.0,dx=0.0;
    float32 *spectrum = NULL;                               /*(dBm) */

	/* Set up subblock outputs */
	for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
        {
        subblocksMsr[i].lowerAbsolutePower = NULL;
        subblocksMsr[i].upperAbsolutePower = NULL;
        subblocksMsr[i].lowerRelativePower = NULL;
        subblocksMsr[i].upperRelativePower = NULL;
    }

    /* Initialize a session */
    RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, 
		frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxLTE_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
    RFmxCheckWarn(RFmxLTE_CfgFrequency(instrumentHandle, "", centerFrequency));
    RFmxCheckWarn(RFmxLTE_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
    RFmxCheckWarn(RFmxLTE_CfgRFAttenuation(instrumentHandle, "", RFAttenuationAuto, RFAttenuation));
    RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, 
		enableTrigger));
    RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));
    RFmxCheckWarn(RFmxLTE_CfgNumberOfSubblocks(instrumentHandle, "", NUMBER_OF_SUBBLOCKS));

    for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
    {
        RFmxCheckWarn(RFmxLTE_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString));
        RFmxCheckWarn(RFmxLTE_SetSubblockFrequency(instrumentHandle, subblockString, subblockInput[i].subblockFrequency));
        RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, subblockString, 
                                                            subblockInput[i].componentCarrierSpacingType,
                                                            subblockInput[i].componentCarrierAtCenterFrequency));
        RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, subblockString,
                                                            NUMBER_OF_COMPONENT_CARRIERS));
        RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, subblockString,
                                                            subblockInput[i].componentCarrierBandwidth, subblockInput[i].componentCarrierFrequency,
                                                            NULL, NUMBER_OF_COMPONENT_CARRIERS));
    }

	RFmxCheckWarn(RFmxLTE_CfgLinkDirection(instrumentHandle, "", linkDirection));
    RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_ACP, RFMXLTE_VAL_TRUE));
    RFmxCheckWarn(RFmxLTE_ACPCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
    RFmxCheckWarn(RFmxLTE_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
    RFmxCheckWarn(RFmxLTE_ACPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
    RFmxCheckWarn(RFmxLTE_ACPCfgNoiseCompensationEnabled(instrumentHandle, "", noiseCompensationEnabled));
    RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

    for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
    {
        RFmxCheckWarn(RFmxLTE_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString));
        RFmxCheckWarn(RFmxLTE_ACPFetchSubblockMeasurement(instrumentHandle, subblockString, timeout,
                                                            &subblocksMsr[i].subblockPower,
                                                            &subblocksMsr[i].integrationBandwidth,
                                                            &subblocksMsr[i].frequency));

        RFmxCheckWarn(RFmxLTE_ACPFetchOffsetMeasurementArray(instrumentHandle, subblockString, timeout,
                                                                NULL, NULL, NULL, NULL, 0, 
                                                                &subblocksMsr[i].numberOfOffsets));
        if( subblocksMsr[i].numberOfOffsets > 0 )
        {
            subblocksMsr[i].lowerRelativePower = (float64 *) malloc(sizeof(float64) * subblocksMsr[i].numberOfOffsets);
            subblocksMsr[i].upperRelativePower = (float64 *) malloc(sizeof(float64) * subblocksMsr[i].numberOfOffsets);
            subblocksMsr[i].lowerAbsolutePower = (float64 *) malloc(sizeof(float64) * subblocksMsr[i].numberOfOffsets);
            subblocksMsr[i].upperAbsolutePower = (float64 *) malloc(sizeof(float64) * subblocksMsr[i].numberOfOffsets);
            /* Fetch the measurements array */
            if( subblocksMsr[i].lowerRelativePower && subblocksMsr[i].upperRelativePower && 
                subblocksMsr[i].lowerAbsolutePower &&subblocksMsr[i].upperAbsolutePower )
            {
                RFmxCheckWarn(RFmxLTE_ACPFetchOffsetMeasurementArray(instrumentHandle, subblockString, timeout,
                                                                subblocksMsr[i].lowerRelativePower,
                                                                subblocksMsr[i].upperRelativePower,
                                                                subblocksMsr[i].lowerAbsolutePower,
                                                                subblocksMsr[i].upperAbsolutePower,
                                                                subblocksMsr[i].numberOfOffsets, NULL));
            }
            else
            {
                printf("malloc failed.\n");
                goto Error;
            }
        }
    }
    RFmxCheckWarn(RFmxLTE_ACPFetchTotalAggregatedPower(instrumentHandle, "", timeout, &totalAggregatedPower));

    RFmxCheckWarn(RFmxLTE_ACPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
    if( actualArraySize > 0 )
    {
        spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
        if(spectrum)
        {
            RFmxCheckWarn(RFmxLTE_ACPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    printf("\nTotal Aggregated Power (dBm)    : %f\n", totalAggregatedPower);

    printf("\n****************Subblock Measurements****************\n");
    for(i = 0;i < NUMBER_OF_SUBBLOCKS; i++)
    {
        printf("\nSubblock                        : %d\n", i);
        printf("Subblock Power (dBm)            : %lf\n",subblocksMsr[i].subblockPower);
        printf("Integration Bandwidth (Hz)      : %lf\n",subblocksMsr[i].integrationBandwidth);
        printf("Frequency (Hz)                  : %lf\n",subblocksMsr[i].frequency);

        printf("\n-------Offset Channel Measurements------\n");
        for(j=0;j<subblocksMsr[i].numberOfOffsets;j++)
        {
            printf("Offset  : %d\n", j);
            printf("Lower Relative Power (dB)       : %f\n", subblocksMsr[i].lowerRelativePower[j]);
            printf("Upper Relative Power (dB)       : %f\n", subblocksMsr[i].upperRelativePower[j]);
            printf("Lower Absolute Power (dBm)      : %f\n", subblocksMsr[i].lowerAbsolutePower[j]);
            printf("Upper Absolute Power (dBm)      : %f\n", subblocksMsr[i].upperAbsolutePower[j]);
        }
        printf("------------------------------------------\n");
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
    for(i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
        {
            if (subblocksMsr[i].lowerRelativePower)
            {
                free(subblocksMsr[i].lowerRelativePower);
            }
            if (subblocksMsr[i].upperRelativePower)
            {
                free(subblocksMsr[i].upperRelativePower);
            }
            if (subblocksMsr[i].lowerAbsolutePower)
            {
                free(subblocksMsr[i].lowerAbsolutePower);
            }
            if (subblocksMsr[i].upperAbsolutePower)
            {
                free(subblocksMsr[i].upperAbsolutePower);
            }
        }

    printf("Press any key to exit\n");
    _getch();

    return error;
}
