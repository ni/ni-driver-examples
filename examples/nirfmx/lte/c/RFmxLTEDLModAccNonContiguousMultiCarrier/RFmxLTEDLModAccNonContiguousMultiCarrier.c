//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Duplex Mode.
//6. Configure Link Direction as Downlink.
//7[A-F]. Configure Subblock Parameters.
//7A. Configure Number of Subblocks.
//7B. Configure subblock Frequency.
//7C. Configure Component Carrier Spacing.
//7D. Configure Band.
//7E. Configure Number of Component Carriers.
//7F. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
//8. Configure Downlink Test Model.
//9. Select ModAcc measurement and enable Traces.
//10. Configure Averaging Parameters for ModAcc measurement.
//11. Select Frame as Synchronization Mode and configure Measurement Interval.
//12. Configure EVM Unit.
//13. Initiate the Measurement
//14. Fetch ModAcc Measurements and Traces
//15. Close RFmx Session.

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

/* Input: Subblock inputs structure */
typedef struct 
{
    float64 subblockFrequency;                                                                                         /*(Hz) */
	int32 componentCarrierSpacingType;
	int32 componentCarrierAtCenterFrequency;
	int32 band;  
	float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS];                                                    /*(Hz) */
	float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS]; 
	int32 downlinkTestModel[NUMBER_OF_COMPONENT_CARRIERS];
}subblockInputs_t;

/* Input: Subblock measurement outputs structure */
typedef struct 
{
	float64 meanRMSCompositeEVM [NUMBER_OF_COMPONENT_CARRIERS];                                                         /*(dBm)*/
	float64 maximumPeakCompositeEVM [NUMBER_OF_COMPONENT_CARRIERS];                                                     /*(dBm)*/
	int32 peakCompositeEVMSlotIndex [NUMBER_OF_COMPONENT_CARRIERS];                                                     /*(dB) */
	int32 peakCompositeEVMSymbolIndex [NUMBER_OF_COMPONENT_CARRIERS];
	int32 peakCompositeEVMSubcarrierIndex [NUMBER_OF_COMPONENT_CARRIERS];
	float64 meanRMSEVM[NUMBER_OF_COMPONENT_CARRIERS];                                                                   /*(% or dB) */
	float64 meanRMSQPSKEVM[NUMBER_OF_COMPONENT_CARRIERS];                                                               /*(% or dB) */
	float64 meanRMS16QAMEVM[NUMBER_OF_COMPONENT_CARRIERS];                                                              /*(% or dB) */
	float64 meanRMS64QAMEVM[NUMBER_OF_COMPONENT_CARRIERS];                                                              /*(% or dB) */
	float64 meanRMS256QAMEVM[NUMBER_OF_COMPONENT_CARRIERS];                                                             /*(% or dB) */
   float64 meanRMS1024QAMEVM[NUMBER_OF_COMPONENT_CARRIERS];                                                            /*(% or dB) */
	float64 meanFrequencyError [NUMBER_OF_COMPONENT_CARRIERS];                                                          /*  (Hz) */
	float64 meanIQOriginOffset [NUMBER_OF_COMPONENT_CARRIERS];                                                          /*  (dBc)*/
	float64 meanIQGainImbalance [NUMBER_OF_COMPONENT_CARRIERS];                                                         /*  (dB) */
	float64 meanIQQuadratureError [NUMBER_OF_COMPONENT_CARRIERS];                                                       /*  (deg)*/
	float64 x0[NUMBER_OF_COMPONENT_CARRIERS];
	float64 dx[NUMBER_OF_COMPONENT_CARRIERS];
	float32* meanRMSEVMPerSubcarrier[NUMBER_OF_COMPONENT_CARRIERS];
}subblockMeasurements_t;

int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;

	char subblockString[NUMBER_OF_SUBBLOCKS][MAX_SELECTOR_STRING];
	char subblockCarrierString[MAX_SELECTOR_STRING];    

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0, lastErrorCode = 0;
	int i = 0, j = 0;

	char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;                                                                         /*(Hz) */

	float64 centerFrequency = 2.14e9;                                                                                   /*(Hz) */
	float64 referenceLevel = 0.0;                                                                                       /*(dBm) */
	float64 externalAttenuation = 0.0;                                                                                  /*(dB) */

	int32 enableTrigger = RFMXLTE_VAL_FALSE;
	char * digitalEdgeTriggerSource = RFMXLTE_VAL_PFI0_STR;
	int32 digitalEdgeTriggerEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.0;                                                                                         /*(s) */

	int32 linkDirection = RFMXLTE_VAL_LINK_DIRECTION_DOWNLINK;
	int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_FDD;
	int32 uplinkDownlinkConfiguraiton = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;

	subblockInputs_t subblocks[NUMBER_OF_SUBBLOCKS] = {						/*  Set up subblock 0 inputs  */
		{	0.0,											    /* subblockFrequency */	
		RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL, 
		-1,														/* componentCarrierAtCenterFrequency */
		1,														/* band */
		{20e6},													/* componentCarrierBandwidth */
		{0.0},													/* componentCarrierFrequency */
		{RFMXLTE_VAL_DOWNLINK_TEST_MODEL_TM1_1}		            /* downlinkTestModel */
		},
		{				/*  Set up subblock 1 inputs  */
			30e6,													/* subblockFrequency */	
			RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL, 
			-1,														/* componentCarrierAtCenterFrequency */
			1,														/* band */
			{20e6},													/* componentCarrierBandwidth */
			{0.0},													/* componentCarrierFrequency */
			{RFMXLTE_VAL_DOWNLINK_TEST_MODEL_TM1_1}					/* downlinkTestModel */
		}
	};    

	int32 averagingEnabled = RFMXLTE_VAL_MODACC_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;

	int32 synchronizationMode = RFMXLTE_VAL_MODACC_SYNCHRONIZATION_MODE_FRAME;
	int32 measurementOffset = 0;                                                                                        /*(slots) */
	int32 measurementLength = 1;                                                                                        /*(slots) */

	int32 evmUnit = RFMXLTE_VAL_MODACC_EVM_UNIT_PERCENTAGE;

	float64 timeout = 10.0;                             /*(s) */
	subblockMeasurements_t subblocksMsr[NUMBER_OF_SUBBLOCKS];
	NIComplexSingle* QPSKConstellation[NUMBER_OF_SUBBLOCKS] = {NULL};

	int32 actualArraySize = 0;
	int32 QPSKConstellationActualArraySize = 0;


	/* Set up subblock outputs */
	for( i = 0; i < NUMBER_OF_SUBBLOCKS; i++ )
	{	
		for( j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++ )
		{
			subblocksMsr[i].meanRMSCompositeEVM[j] = 0;
			subblocksMsr[i].maximumPeakCompositeEVM[j] = 0;
			subblocksMsr[i].meanFrequencyError[j] = 0;
			subblocksMsr[i].peakCompositeEVMSlotIndex[j] = 0;
			subblocksMsr[i].peakCompositeEVMSymbolIndex[j] = 0;
			subblocksMsr[i].peakCompositeEVMSubcarrierIndex[j] = 0;
			subblocksMsr[i].meanIQOriginOffset[j] = 0;
			subblocksMsr[i].meanIQGainImbalance[j] = 0;
			subblocksMsr[i].meanIQQuadratureError[j] = 0;
			subblocksMsr[i].meanRMSEVM[j] = 0;
			subblocksMsr[i].meanRMSQPSKEVM[j] = 0;
			subblocksMsr[i].meanRMS16QAMEVM[j] = 0;
			subblocksMsr[i].meanRMS64QAMEVM[j] = 0;
			subblocksMsr[i].meanRMS256QAMEVM[j] = 0;
         subblocksMsr[i].meanRMS1024QAMEVM[j] = 0;
			subblocksMsr[i].meanRMSEVMPerSubcarrier[j] = NULL;
		}
	}

	/* Initialize a session */
	RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));
	RFmxCheckWarn(RFmxLTE_CfgLinkDirection(instrumentHandle, "", linkDirection));
	RFmxCheckWarn(RFmxLTE_CfgNumberOfSubblocks(instrumentHandle, "", NUMBER_OF_SUBBLOCKS));
	for( i = 0; i < NUMBER_OF_SUBBLOCKS; i++ )
	{
		RFmxCheckWarn(RFmxLTE_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString[i]));
        RFmxCheckWarn(RFmxLTE_SetSubblockFrequency(instrumentHandle, subblockString[i], subblocks[i].subblockFrequency));
		RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, subblockString[i],
			subblocks[i].componentCarrierSpacingType,
			subblocks[i].componentCarrierAtCenterFrequency));
		RFmxCheckWarn(RFmxLTE_CfgBand(instrumentHandle, subblockString[i], subblocks[i].band));
		RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, subblockString[i], NUMBER_OF_COMPONENT_CARRIERS));
		RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, subblockString[i],
			subblocks[i].componentCarrierBandwidth,
			subblocks[i].componentCarrierFrequency, 
			0, NUMBER_OF_COMPONENT_CARRIERS));
		RFmxCheckWarn(RFmxLTE_CfgDownlinkTestModelArray(instrumentHandle, subblockString[i], subblocks[i].downlinkTestModel, 
			NUMBER_OF_COMPONENT_CARRIERS));    
	}

	RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_MODACC, RFMXLTE_VAL_TRUE));
	RFmxCheckWarn(RFmxLTE_ModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
	RFmxCheckWarn(RFmxLTE_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode, measurementOffset, measurementLength));
	RFmxCheckWarn(RFmxLTE_ModAccCfgEVMUnit(instrumentHandle, "", evmUnit));
	RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

	for( i = 0; i < NUMBER_OF_SUBBLOCKS; i++ )
	{              
		RFmxCheckWarn(RFmxLTE_ModAccFetchCompositeEVMArray(instrumentHandle, subblockString[i], timeout, 
			subblocksMsr[i].meanRMSCompositeEVM, subblocksMsr[i].maximumPeakCompositeEVM,
			subblocksMsr[i].meanFrequencyError, subblocksMsr[i].peakCompositeEVMSymbolIndex,
			subblocksMsr[i].peakCompositeEVMSubcarrierIndex, subblocksMsr[i].peakCompositeEVMSlotIndex,
			NUMBER_OF_COMPONENT_CARRIERS, NULL));

		RFmxCheckWarn(RFmxLTE_ModAccFetchIQImpairmentsArray(instrumentHandle, subblockString[i], timeout, 
			subblocksMsr[i].meanIQOriginOffset, subblocksMsr[i].meanIQGainImbalance,
			subblocksMsr[i].meanIQQuadratureError, NUMBER_OF_COMPONENT_CARRIERS, NULL));

		RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCHEVMArray(instrumentHandle, subblockString[i], timeout,
			subblocksMsr[i].meanRMSEVM, subblocksMsr[i].meanRMSQPSKEVM, subblocksMsr[i].meanRMS16QAMEVM,
			subblocksMsr[i].meanRMS64QAMEVM , subblocksMsr[i].meanRMS256QAMEVM,
			NUMBER_OF_COMPONENT_CARRIERS, NULL));

      RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCH1024QAMEVMArray(instrumentHandle, subblockString[i], timeout,
         subblocksMsr[i].meanRMS1024QAMEVM, NUMBER_OF_COMPONENT_CARRIERS, NULL));

		for( j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++ )
		{
			actualArraySize = 0;
			RFmxCheckWarn(RFmxLTE_BuildCarrierString(subblockString[i], j, MAX_SELECTOR_STRING, subblockCarrierString));
			RFmxCheckWarn(RFmxLTE_ModAccFetchEVMPerSubcarrierTrace(instrumentHandle, subblockCarrierString, timeout, 
				NULL, NULL, NULL, 0, &actualArraySize));
			if( actualArraySize > 0 )
			{
				subblocksMsr[i].meanRMSEVMPerSubcarrier[j] = (float32 *) malloc(sizeof(float32) * actualArraySize);
				if( subblocksMsr[i].meanRMSEVMPerSubcarrier[j] )
				{
					RFmxCheckWarn(RFmxLTE_ModAccFetchEVMPerSubcarrierTrace(instrumentHandle, subblockCarrierString, timeout, 
						&subblocksMsr[i].x0[j], &subblocksMsr[i].dx[j],
						subblocksMsr[i].meanRMSEVMPerSubcarrier[j], actualArraySize, NULL));
				}
				else
				{
					printf("malloc failed.\n");
					goto Error;
				}
			}
		}
	}

	for( i = 0; i < NUMBER_OF_SUBBLOCKS; i++ )
	{
		actualArraySize = 0;
		RFmxCheckWarn(RFmxLTE_BuildCarrierString(subblockString[i], 0, MAX_SELECTOR_STRING, subblockCarrierString));

		RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCHQPSKConstellation(instrumentHandle, subblockCarrierString,timeout,
			NULL,0,&QPSKConstellationActualArraySize));
		if(QPSKConstellationActualArraySize>0)
		{
			QPSKConstellation[i] = (NIComplexSingle *) malloc(sizeof(NIComplexSingle) * QPSKConstellationActualArraySize);
			if(QPSKConstellation[i])
			{
				RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCHQPSKConstellation(instrumentHandle, subblockCarrierString,timeout,
					QPSKConstellation[i],QPSKConstellationActualArraySize,NULL));
			}
			else
			{
				printf("malloc failed.\n");
				goto Error;
			}
		}
	}

	printf("----------------------Measurements--------------------\n");
	for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
	{
		printf("\nSubblock Number %d\n", i);

		printf("-----------Component Carrier Measurements----------------\n");
		for(j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++)
		{
			printf("Carrier %d\n", j);
			printf("Mean RMS Composite EVM  (%% or dB)      : %lf\n",subblocksMsr[i].meanRMSCompositeEVM[j]);
			printf("Mean RMS  EVM  (%% or dB)               : %lf\n",subblocksMsr[i].meanRMSEVM[j]);
			printf("Mean RMS QPSK EVM  (%% or dB)           : %lf\n",subblocksMsr[i].meanRMSQPSKEVM[j]);
			printf("Mean RMS 16QAM EVM  (%% or dB)          : %lf\n",subblocksMsr[i].meanRMS16QAMEVM[j]);
			printf("Mean RMS 64QAM EVM  (%% or dB)          : %lf\n",subblocksMsr[i].meanRMS64QAMEVM[j]);
			printf("Mean RMS 256QAM EVM  (%% or dB)         : %lf\n",subblocksMsr[i].meanRMS256QAMEVM[j]);
         printf("Mean RMS 1024QAM EVM  (%% or dB)        : %lf\n",subblocksMsr[i].meanRMS1024QAMEVM[j]);
			printf("Mean Frequency Error  (Hz)             : %lf\n",subblocksMsr[i].meanFrequencyError[j]);
			printf("Mean IQ Origin Offset  (dBc)           : %lf\n",subblocksMsr[i].meanIQOriginOffset[j]);
			printf("Mean IQ Gain Imbalance  (dB)           : %lf\n",subblocksMsr[i].meanIQGainImbalance[j]);
			printf("Mean IQ Quadrature Error  (deg)        : %lf\n",subblocksMsr[i].meanIQQuadratureError[j]);
			printf("-------------------------------------------------\n");
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

	/* Free allocated memory */
	for( i = 0; i < NUMBER_OF_SUBBLOCKS; i++ )
	{       
		for( j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++ )
		{
            if (subblocksMsr[i].meanRMSEVMPerSubcarrier[j])
            {
                free(subblocksMsr[i].meanRMSEVMPerSubcarrier[j]);
            }
		}
        if (QPSKConstellation[i])
        {
            free(QPSKConstellation[i]);
        }
	}

	printf("Press any key to exit\n");
	_getch();

	return error;
}
