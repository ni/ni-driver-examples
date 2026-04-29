//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5[A-L]. Configure Subblock Parameters.
//6. Configure Link Direction.
//7. Select SEM measurement and enable Traces.
//8. Configure Sweep Time Parameters.
//9. Configure Averaging Parameters for SEM measurement.
//10. Configure Uplink Mask Type, or Downlink Mask, eNodeB Category and Component Carrier Maximum Output Power depending on Link Direction.
//11. Initiate the Measurement.
//12. Fetch SEM Measurements and Traces.
//13. Close RFmx Session.  
//
//Step 5 :
//A. Configure Number of Subblocks.
//B. Configure subblock Frequency.
//C. Configure Component Carrier Spacing.
//D.  Configure Number of Component Carriers.
//E. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
//F. Configure Number of Offsets.
//G. Configure Offset Frequency.
//H. Configure Offset RBW Filter.
//I. Configure Offset Bandwidth Integral.
//J. Configure Offset Absolute Limit.
//K. Configure Offset Relative Limit.
//L. Configure Offset Limit Fail Mask.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096

#define MAX_SELECTOR_STRING                     256

#define NUMBER_OF_COMPONENT_CARRIERS            1
#define NUMBER_OF_SUBBLOCKS                     2
#define NUMBER_OF_OFFSET_SEGMENT                4

/* Input: Subblock inputs structure */
typedef struct 
{
    float64 subblockFrequency;                                                /*(Hz) */
    int32 componentCarrierSpacingType;
    int32 componentCarrierAtCenterFrequency;
    float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS];          /*(Hz) */
    float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS];          /*(Hz) */
	float64 componentCarrierMaximumOutputPower[NUMBER_OF_COMPONENT_CARRIERS]; /*(dBm) */
	float64 startFrequency[NUMBER_OF_OFFSET_SEGMENT];                         /*(Hz) */
	float64 stopFrequency[NUMBER_OF_OFFSET_SEGMENT];                          /*(Hz) */
	int32 sideband[NUMBER_OF_OFFSET_SEGMENT];
	float64 RBW[NUMBER_OF_OFFSET_SEGMENT];                                    /*(Hz) */
	int32 RBWFilterType[NUMBER_OF_OFFSET_SEGMENT];                  
	int32 bandwidthIntegral[NUMBER_OF_OFFSET_SEGMENT];
	float64 offsetAbsoluteLimitStart[NUMBER_OF_OFFSET_SEGMENT];               /*(dBm) */
	float64 offsetAbsoluteLimitStop[NUMBER_OF_OFFSET_SEGMENT];                /*(dBm) */
	float64 offsetRelativeLimitStart[NUMBER_OF_OFFSET_SEGMENT];               /*(dBm) */
	float64 offsetRelativeLimitStop[NUMBER_OF_OFFSET_SEGMENT];                /*(dBm) */
	int32 offsetLimitFailMask[NUMBER_OF_OFFSET_SEGMENT];
}subblockInputs_t;

typedef struct 
{
	float64 subblockPower;                                                    /*(dBm) */
	float64 integrationBandwidth;                                             /*(Hz) */
	float64 frequency;                                                        /*(Hz) */
	int32 lowerOffsetMeasurementStatus[NUMBER_OF_OFFSET_SEGMENT];
	float64 lowerOffsetMargin[NUMBER_OF_OFFSET_SEGMENT];                      /*(dB) */
	float64 lowerOffsetMarginFrequency[NUMBER_OF_OFFSET_SEGMENT];             /*(Hz) */
	float64 lowerOffsetMarginAbsolutePower[NUMBER_OF_OFFSET_SEGMENT];         /*(dBm) */
	int32 upperOffsetMeasurementStatus[NUMBER_OF_OFFSET_SEGMENT];
	float64 upperOffsetMargin[NUMBER_OF_OFFSET_SEGMENT];                      /*(dB) */
	float64 upperOffsetMarginFrequency[NUMBER_OF_OFFSET_SEGMENT];             /*(Hz) */
	float64 upperOffsetMarginAbsolutePower[NUMBER_OF_OFFSET_SEGMENT];         /*(dBm) */
}subblockOutput_t;

int main(int argc, char *argv[])
{

	char *resourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle = NULL;

	char subblockString[MAX_SELECTOR_STRING];

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0;
    int32 lastErrorCode = 0;

	char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;                               /*(Hz) */

	float64 centerFrequency = 1.95e9;                                         /* (Hz) */
	float64 referenceLevel = 0.0;                                             /*(dBm) */
    float64 externalAttenuation = 0.0;                                        /*(dB) */
    int32 enableTrigger = RFMXLTE_VAL_FALSE;

    char * digitalEdgeSource = RFMXLTE_VAL_PFI0_STR;
    int32 digitalEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
	int32 linkDirection = RFMXLTE_VAL_LINK_DIRECTION_UPLINK;
    
	/* Uplink */
	int32 uplinkMaskType = RFMXLTE_VAL_SEM_UPLINK_MASK_TYPE_GENERAL_NS01;

	/* Downlink */
	int32 eNodeBCategory = RFMXLTE_VAL_ENODEB_WIDE_AREA_BASE_STATION_CATEGORY_A;
	int32 downlinkMaskType =  RFMXLTE_VAL_SEM_DOWNLINK_MASK_TYPE_ENODEB_CATEGORY_BASED;
	float64 deltaFMaximum = 15.00e6;                                          /*(Hz) */
	float64 aggregatedMaximumPower = 0.00;                                    /*(dBm) */


	subblockInputs_t    subblocks[NUMBER_OF_SUBBLOCKS] = {	 
															{	
			                                                  0.0,													/* subblockFrequency */	
															  RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL,
															  -1,
															  { 20e+6},
															  { 0.00},
															  { 0.00},
															  {15e+3,1.5e+6,5.5e+6,20.5e+6},
															  {985e+3,4.5e+6,19.5e+6,24.5e+6},
															  {RFMXLTE_VAL_SEM_OFFSET_SIDEBAND_BOTH,
															   RFMXLTE_VAL_SEM_OFFSET_SIDEBAND_BOTH,
															   RFMXLTE_VAL_SEM_OFFSET_SIDEBAND_BOTH,
															   RFMXLTE_VAL_SEM_OFFSET_SIDEBAND_BOTH
															  },
															  {10e+3,250e+3,250e+3,250e+3},
															  {RFMXLTE_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN,
															   RFMXLTE_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN,
															   RFMXLTE_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN,
															   RFMXLTE_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN
															  },
															  {3,4,4,4},
															  {-19.5,-8.5,-11.5,-23.5},
															  {-19.5,-8.5,-11.5,-23.5},
															  {-51.50,-51.50,-51.50,-51.50},
															  {-58.50,-58.50,-58.50,-58.50},
															  {RFMXLTE_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE,
															   RFMXLTE_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE,
															   RFMXLTE_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE,
															   RFMXLTE_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE
															  }
														   },
														   {
															30e6,													/* subblockFrequency */	
															RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL,
															-1,
															{ 20e+6},
															{ 0.00},
															{ 0.00},
															{15e+3,1.5e+6,5.5e+6,20.5e+6},
															{985e+3,4.5e+6,19.5e+6,24.5e+6},
															{RFMXLTE_VAL_SEM_OFFSET_SIDEBAND_BOTH,
															 RFMXLTE_VAL_SEM_OFFSET_SIDEBAND_BOTH,
															 RFMXLTE_VAL_SEM_OFFSET_SIDEBAND_BOTH,
															 RFMXLTE_VAL_SEM_OFFSET_SIDEBAND_BOTH
															},
															{10e+3,250e+3,250e+3,250e+3},
															{RFMXLTE_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN,
															 RFMXLTE_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN,
															 RFMXLTE_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN,
															 RFMXLTE_VAL_SEM_OFFSET_RBW_FILTER_TYPE_GAUSSIAN
															},
															{3,4,4,4},
															{-19.5,-8.5,-11.5,-23.5},
															{-19.5,-8.5,-11.5,-23.5},
															{-51.50,-51.50,-51.50,-51.50},
															{-58.50,-58.50,-58.50,-58.50},
															{RFMXLTE_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE,
															 RFMXLTE_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE,
															 RFMXLTE_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE,
															 RFMXLTE_VAL_SEM_OFFSET_LIMIT_FAIL_MASK_ABSOLUTE
															}
														   }
                                                 	    }; 
	float64 triggerDelay = 0.0;                                               /*(s) */
	int32 sweepTimeAuto  = RFMXLTE_VAL_SEM_SWEEP_TIME_AUTO_TRUE;
	float64 sweepTimeInterval = 1e-3;                                         /*(s) */              


	int32 averagingEnabled = RFMXLTE_VAL_SEM_AVERAGING_ENABLED_FALSE;

	int32 averagingCount = 10;
	int32 averagingType = RFMXLTE_VAL_SEM_AVERAGING_TYPE_RMS;

	float64 timeout = 10.0;                                                   /*(s) */
	subblockOutput_t subblockOutput[5];
	
	float64 totalAggregatedPower = 0.0;   
	int32 measurementStatus = 0;
	char *str = "Fail";
	int32 actualArraySize = 0;
	float64 x0 = 0.0;
	float64 dx = 0.0;
	float32 *spectrum = (float32 *)NULL;
	float32 *absoluteMask = (float32 *)NULL;


	int i,j;

	RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, 
		           frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, 
		           triggerDelay, enableTrigger));
    RFmxCheckWarn(RFmxLTE_CfgNumberOfSubblocks(instrumentHandle, "", NUMBER_OF_SUBBLOCKS));
    
	for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
    {
		RFmxCheckWarn(RFmxLTE_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString));
		RFmxCheckWarn(RFmxLTE_SetSubblockFrequency(instrumentHandle, subblockString, subblocks[i].subblockFrequency));
		RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, subblockString, subblocks[i].componentCarrierSpacingType,
			                                             subblocks[i].componentCarrierAtCenterFrequency));
		RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, subblockString, NUMBER_OF_COMPONENT_CARRIERS));
	
	    RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle,subblockString, subblocks[i].componentCarrierBandwidth,
		                                           subblocks[i].componentCarrierFrequency, NULL, NUMBER_OF_COMPONENT_CARRIERS));
       			                                                 
        RFmxCheckWarn(RFmxLTE_SEMCfgNumberOfOffsets(instrumentHandle, subblockString,NUMBER_OF_OFFSET_SEGMENT));
	    RFmxCheckWarn(RFmxLTE_SEMCfgOffsetFrequencyArray(instrumentHandle, subblockString, subblocks[i].startFrequency,
		                                              subblocks[i].stopFrequency, subblocks[i].sideband,
													  NUMBER_OF_OFFSET_SEGMENT));
	   RFmxCheckWarn(RFmxLTE_SEMCfgOffsetRBWFilterArray(instrumentHandle, subblockString, subblocks[i].RBW,
		                                              subblocks[i].RBWFilterType,NUMBER_OF_OFFSET_SEGMENT));
       RFmxCheckWarn(RFmxLTE_SEMCfgOffsetBandwidthIntegralArray(instrumentHandle, subblockString, subblocks[i].bandwidthIntegral,
		                                              NUMBER_OF_OFFSET_SEGMENT));
	   RFmxCheckWarn(RFmxLTE_SEMCfgOffsetAbsoluteLimitArray(instrumentHandle, subblockString,subblocks[i].offsetAbsoluteLimitStart,
		   subblocks[i].offsetAbsoluteLimitStop,NUMBER_OF_OFFSET_SEGMENT));
	}

	RFmxCheckWarn(RFmxLTE_CfgLinkDirection(instrumentHandle, "", linkDirection));
	RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_SEM, RFMXLTE_VAL_TRUE));
	RFmxCheckWarn(RFmxLTE_SEMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
	RFmxCheckWarn(RFmxLTE_SEMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
	if(linkDirection == RFMXLTE_VAL_LINK_DIRECTION_UPLINK)
       RFmxCheckWarn(RFmxLTE_SEMCfgUplinkMaskType(instrumentHandle, "", uplinkMaskType));
	else
	{
		RFmxCheckWarn(RFmxLTE_CfgeNodeBCategory(instrumentHandle, "", eNodeBCategory));
        RFmxCheckWarn(RFmxLTE_SEMCfgDownlinkMask(instrumentHandle, "", downlinkMaskType, deltaFMaximum,
			                  aggregatedMaximumPower));
		for( i=0; i < NUMBER_OF_SUBBLOCKS; i++)
	    {
		  RFmxCheckWarn(RFmxLTE_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString));
		  RFmxCheckWarn(RFmxLTE_SEMCfgComponentCarrierMaximumOutputPowerArray(instrumentHandle, subblockString, 
			                  subblocks[i].componentCarrierMaximumOutputPower, NUMBER_OF_COMPONENT_CARRIERS));
		}
	}
	RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));
	for(i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
	{
		RFmxCheckWarn(RFmxLTE_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString));
		RFmxCheckWarn(RFmxLTE_SEMFetchSubblockMeasurement(instrumentHandle, "", timeout, &subblockOutput[i].subblockPower,
			                                              &subblockOutput[i].integrationBandwidth,&subblockOutput[i].frequency));	
				
		for( j = 0; j <  NUMBER_OF_OFFSET_SEGMENT; j++)
		{
			
			RFmxCheckWarn(RFmxLTE_SEMFetchUpperOffsetMarginArray(instrumentHandle, subblockString, timeout, 
			                                                 subblockOutput[i].upperOffsetMeasurementStatus,
															 subblockOutput[i].upperOffsetMargin,
															 subblockOutput[i].upperOffsetMarginFrequency,
														     subblockOutput[i].upperOffsetMarginAbsolutePower,
															 NULL, NUMBER_OF_OFFSET_SEGMENT, NULL));
			
			RFmxCheckWarn(RFmxLTE_SEMFetchLowerOffsetMarginArray(instrumentHandle, subblockString, timeout,
				                                                 subblockOutput[i].lowerOffsetMeasurementStatus,
																 subblockOutput[i].lowerOffsetMargin,
																 subblockOutput[i].lowerOffsetMarginFrequency,
																 subblockOutput[i].lowerOffsetMarginAbsolutePower,
																 NULL, NUMBER_OF_OFFSET_SEGMENT, NULL));
				}
	}

	RFmxCheckWarn(RFmxLTE_SEMFetchTotalAggregatedPower(instrumentHandle, "", timeout,  &totalAggregatedPower));
	RFmxCheckWarn(RFmxLTE_SEMFetchMeasurementStatus(instrumentHandle, "",timeout, &measurementStatus));
	RFmxCheckWarn(RFmxLTE_SEMFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL,NULL, 0, &actualArraySize));
	if(actualArraySize > 0)
	{
		spectrum = (float32*)malloc(sizeof(float32)*actualArraySize);
		absoluteMask = (float32*)malloc(sizeof(float32)*actualArraySize);
		if(spectrum && absoluteMask)
		{
			RFmxCheckWarn(RFmxLTE_SEMFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, 
				                                   absoluteMask,actualArraySize,NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	if( measurementStatus == RFMXLTE_VAL_SEM_UPPER_OFFSET_MEASUREMENT_STATUS_PASS)
		str = "Pass";
	printf("Total Aggregated Power (dBm)               : %lf\n",totalAggregatedPower);
	printf("Measurement Status                         : %s\n",str);

	for(i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
	{
		  printf("\nSubblock %d\n\n",i);
		  printf("Subblock Power (dBm)                       : %lf\n",subblockOutput[i].subblockPower);
		  printf("Integration Bandwidth (Hz)                 : %lf\n",subblockOutput[i].integrationBandwidth);
		  printf("Frequency (Hz)                             : %lf\n",subblockOutput[i].frequency);
		  printf("\n Offset Segment Measurements\n");
		  for( j = 0; j <  NUMBER_OF_OFFSET_SEGMENT; j++)
		  {
				printf("\nOffset  %d\n",j);
				printf("Lower Offset Segment Measurement \n");
				str = "Fail";
				if(subblockOutput[i].lowerOffsetMeasurementStatus[j] == RFMXLTE_VAL_SEM_LOWER_OFFSET_MEASUREMENT_STATUS_PASS)
					str = "Pass";
				printf("Measurement Status                         : %s\n",str);
				printf("Margin (dB)                                : %lf\n",subblockOutput[i].lowerOffsetMargin[j]);
				printf("Margin Frequency (Hz)                      : %lf\n",subblockOutput[i].lowerOffsetMarginFrequency[j]);
				printf("Margin Absolute Power (dBm)                : %lf\n",subblockOutput[i].lowerOffsetMarginAbsolutePower[j]);
				printf("\nUpper Offset Segment Measurement \n");
				str = "Fail";
				if(subblockOutput[i].upperOffsetMeasurementStatus[j] == RFMXLTE_VAL_SEM_UPPER_OFFSET_MEASUREMENT_STATUS_PASS)
					str = "Pass";
				printf("Measurement status                         : %s\n",str);
				printf("Margin (dB)                                : %lf\n",subblockOutput[i].upperOffsetMargin[j]);
				printf("Margin Frequency (Hz)                      : %lf\n",subblockOutput[i].upperOffsetMarginFrequency[j]);
				printf("Margin Absolute Power (dBm)                : %lf\n",subblockOutput[i].upperOffsetMarginAbsolutePower[j]);
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
	if(spectrum)
		free(spectrum);
	if(absoluteMask)
		free(absoluteMask);
	
    printf("Press any key to exit\n");
    _getch();   

    return error;
}
