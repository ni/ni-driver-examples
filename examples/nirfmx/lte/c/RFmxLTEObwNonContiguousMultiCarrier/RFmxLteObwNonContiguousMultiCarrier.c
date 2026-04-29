//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5[A-E]. Configure Subblock Configurations.
//5A. Configure Number of Subblocks.
//5B. Configure subblock Frequency.
//5C. Configure Component Carrier Spacing.
//5D. Configure Number of Component Carriers.
//5E. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
//6. Select OBW measurement and enable Traces.
//7. Configure Sweep Time Parameters.
//8. Configure Averaging Parameters for OBW measurement.
//9. Initiate the Measurement.
//10. Fetch OBW Measurements and Traces.
//11. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                     256
#define NUMBER_OF_COMPONENT_CARRIERS            1
#define NUMBER_OF_SUBBLOCKS                     2

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
    double startFrequency, stopFrequency, occupiedBandwidth, absolutePower;
}subblockMeasurements_t;

int main (int argc, char *argv[])
{
    char *resourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle = NULL;
    
    char subblockString[MAX_SELECTOR_STRING];

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0;
    int32 lastErrorCode = 0;

    float64 centerFrequency = 1.95e9;               /*(Hz) */
    float64 referenceLevel = 0.0;                   /*(dBm) */
    float64 externalAttenuation = 0.0;              /*(dB) */
    int32 enableTrigger = RFMXLTE_VAL_FALSE;

    char * digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
    int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;

    float64 triggerDelay = 0.0;/*(s) */

    char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;     /*(Hz) */

	int32 linkDirection = RFMXLTE_VAL_LINK_DIRECTION_UPLINK;

	subblockInputs_t    subblocks[NUMBER_OF_SUBBLOCKS] = {					/*  Set up subblock 0 inputs  */
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

    int32 sweepTimeAuto = RFMXLTE_VAL_OBW_SWEEP_TIME_AUTO_TRUE;
    float64 sweepTimeInterval = 0.001;              /*(s) */

    int32 averagingEnabled = RFMXLTE_VAL_OBW_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;
    int32 averagingType = RFMXLTE_VAL_OBW_AVERAGING_TYPE_RMS;

    float64 timeout = 10.0;                         /*(s) */

    subblockMeasurements_t subblocksMsr[NUMBER_OF_SUBBLOCKS];

    int32 actualArraySize = 0;
    float64 x0=0.0,dx=0.0;
    float32 *spectrum = NULL;                       /*(dBm) */ 
    int i;

    RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
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

	RFmxCheckWarn(RFmxLTE_CfgLinkDirection(instrumentHandle, "", linkDirection));
    RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_OBW, RFMXLTE_VAL_TRUE));
    RFmxCheckWarn(RFmxLTE_OBWCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
    RFmxCheckWarn(RFmxLTE_OBWCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
    RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

    for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
    {
        RFmxCheckWarn(RFmxLTE_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString));
        RFmxCheckWarn(RFmxLTE_OBWFetchMeasurement(instrumentHandle, subblockString, timeout,
                                                        &subblocksMsr[i].occupiedBandwidth, &subblocksMsr[i].absolutePower,
                                                        &subblocksMsr[i].startFrequency, &subblocksMsr[i].stopFrequency));
    }

    RFmxCheckWarn(RFmxLTE_OBWFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
    if( actualArraySize > 0 )
    {
        spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
        if(spectrum)
        {
            RFmxCheckWarn(RFmxLTE_OBWFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, 
                                                  actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

     printf("\n-----------Subblock Measurements-----------\n");
     for(i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
     {
         printf("\nSubblock  : %d\n", i);
		 printf("Occupied Bandwidth (Hz) : %lf\n",subblocksMsr[i].occupiedBandwidth);
         printf("Absolute Power (dBm)    : %lf\n",subblocksMsr[i].absolutePower);         
         printf("Start Frequency (Hz)    : %lf\n",subblocksMsr[i].startFrequency);
         printf("Stop Frequency (Hz)     : %lf\n",subblocksMsr[i].stopFrequency);
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

    if (spectrum)
    {
        free(spectrum);
    }

    printf("\nPress any key to exit\n");
    _getch();

    return error;
}
