// Steps:
// 1. Open a new RFmx Session.
// 2. Configure Frequency Reference.
// 3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
// 4. Configure Trigger Type and Trigger Parameters.
// 5. Configure UARFCN Band.
// 6. Configure Uplink Scrambling.
// 7. Select ACP,CHP,ModAcc,OBW and SEM measurements and enable Traces.
// 8. Configure Synchronization Mode and Measurement Interval.
// 9. Configure Sweep Time Parameters for ACP. 
// 10. Configure Averaging Parameters for ACP.
// 11. Configure Sweep Time Parameters for CHP. 
// 12. Configure Averaging Parameters for CHP.
// 13. Configure Sweep Time Parameters for OBW. 
// 14. Configure Averaging Parameters for OBW.
// 15. Configure Sweep Time Parameters for SEM. 
// 16. Configure Averaging Parameters for SEM.
// 17. Initiate the Measurement.
// 18. Fetch ACP,SEM,ModAcc,CHP & OBW Measurements.
// 19. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main (int argc, char *argv[])
{
    niRFmxInstrHandle instrumentHandle = NULL;
    
	char *resourceName = "RFSA";

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0, lastErrorCode = 0;
	int i = 0;

    char * frequencyReferenceSource = RFMXWCDMA_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;		/*(Hz) */

    float64 centerFrequency = 1.95e9;				/*(Hz) */
    float64 referenceLevel = 0.000000;				/*(dBm) */
    float64 externalAttenuation = 0.000000;			/*(dB) */

    int32 enableTrigger = RFMXWCDMA_VAL_FALSE;
    char * digitalEdgeSource = RFMXWCDMA_VAL_PFI0_STR;
    int32 digitalEdge = RFMXWCDMA_VAL_DIGITAL_EDGE_RISING_EDGE;
    float64 triggerDelay = 0.000000;				/*(s) */

    int32 band = 1;

	int32 synchronizationMode = RFMXWCDMA_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;
    int32 measurementOffset = 0;					/*(slots) */
    int32 measurementLength = 1;					/*(slots) */
    int32 uplinkScramblingType = RFMXWCDMA_VAL_UPLINK_SCRAMBLING_TYPE_LONG;
    int32 uplinkScramblingCode = 0x0;

    float64 sweepTimeInterval = 0.000667;			/*(s) */
    int32 averagingCount = 10;

	int32 ACPLowerOffsetMeasArraySize = 0;
    float64* ACPLowerAbsolutePower = NULL;			/*(dBm) */
    float64* ACPUpperAbsolutePower = NULL;			/*(dBm) */
	float64* ACPLowerRelativePower = NULL;			/*(dB) */
    float64* ACPUpperRelativePower = NULL;			/*(dB) */
	float64 ACPAbsolutePower = 0.000000;			/*(dBm) */
    float64 ACPRelativePower = 0.000000;			/*(dB) */

    float64 timeout = 10.000000;					/*(s) */
   
    float64 chipRateError = 0.000000;				/*(ppm) */
    float64 frequencyError = 0.000000;				/*(Hz) */
    float64 rmsEVM = 0.000000;						/*(%) */
    float64 peakEVM = 0.000000;						/*(%) */
    float64 rho = 0.000000;
    float64 rmsPhaseError = 0.000000;				/*(deg) */
    float64 rmsMagnitudeError = 0.000000;			/*(%) */
	
	int32 SEMMeasurementStatus = 0;

	float64 CHPAbsolutePower = 0.000000;			/*(dBm) */
    float64 CHPRelativePower = 0.000000;			/*(dB) */

    float64 OBWAbsolutePower = 0.000000;			/*(dBm) */
	float64 OBWStopFrequency = 0.000000;			/*(Hz) */
    float64 OBWStartFrequency = 0.000000;			/*(Hz) */
    float64 OBWOccupiedBandwidth = 0.000000;		/*(Hz) */

	float64 SEMAbsoluteIntegratedPower = 0.000000;			/*(dBm) */
	float64 SEMRelativeIntegratedPower = 0.000000;			/*(dB) */

	int32 SEMLowerOffsetMeasArraySize = 0, SEMUpperOffsetMeasArraySize = 0;
    float64* SEMLowerOffsetMarginRelativePower = NULL;		/*(dB) */
    float64* SEMLowerOffsetMarginAbsolutePower = NULL;		/*(dBm) */
    int32* SEMLowerOffsetMeasurementStatus = NULL;
    float64* SEMLowerOffsetMargin = NULL;					/*(dB) */
    float64* SEMLowerOffsetMarginFrequency = NULL;			/*(Hz) */
	float64* SEMUpperOffsetMarginRelativePower = NULL;		/*(dB) */
    float64* SEMUpperOffsetMarginAbsolutePower = NULL;		/*(dBm) */
    int32* SEMUpperOffsetMeasurementStatus = NULL;
    float64* SEMUpperOffsetMargin = NULL;					/*(dB) */
    float64* SEMUpperOffsetMarginFrequency = NULL;			/*(Hz) */

    /* Initialize a session */
    RFmxCheckWarn(RFmxWCDMA_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxWCDMA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxWCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxWCDMA_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
    RFmxCheckWarn(RFmxWCDMA_CfgBand(instrumentHandle, "", band));
    RFmxCheckWarn(RFmxWCDMA_CfgUplinkScrambling(instrumentHandle, "", uplinkScramblingCode, uplinkScramblingType));
    RFmxCheckWarn(RFmxWCDMA_SelectMeasurements(instrumentHandle, "", RFMXWCDMA_VAL_MODACC | RFMXWCDMA_VAL_ACP | RFMXWCDMA_VAL_CHP | 
																		RFMXWCDMA_VAL_OBW | RFMXWCDMA_VAL_SEM, RFMXWCDMA_VAL_TRUE));
    RFmxCheckWarn(RFmxWCDMA_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode, measurementOffset,
																			measurementLength));
    RFmxCheckWarn(RFmxWCDMA_ACPCfgSweepTime(instrumentHandle, "", RFMXWCDMA_VAL_ACP_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
    RFmxCheckWarn(RFmxWCDMA_ACPCfgAveraging(instrumentHandle, "", RFMXWCDMA_VAL_ACP_AVERAGING_ENABLED_FALSE, averagingCount, RFMXWCDMA_VAL_ACP_AVERAGING_TYPE_RMS));
    RFmxCheckWarn(RFmxWCDMA_CHPCfgSweepTime(instrumentHandle, "", RFMXWCDMA_VAL_CHP_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
    RFmxCheckWarn(RFmxWCDMA_CHPCfgAveraging(instrumentHandle, "", RFMXWCDMA_VAL_CHP_AVERAGING_ENABLED_FALSE, averagingCount, RFMXWCDMA_VAL_CHP_AVERAGING_TYPE_RMS));
    RFmxCheckWarn(RFmxWCDMA_OBWCfgSweepTime(instrumentHandle, "", RFMXWCDMA_VAL_OBW_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
    RFmxCheckWarn(RFmxWCDMA_OBWCfgAveraging(instrumentHandle, "", RFMXWCDMA_VAL_OBW_AVERAGING_ENABLED_FALSE, averagingCount, RFMXWCDMA_VAL_OBW_AVERAGING_TYPE_RMS));
    RFmxCheckWarn(RFmxWCDMA_SEMCfgSweepTime(instrumentHandle, "", RFMXWCDMA_VAL_SEM_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
    RFmxCheckWarn(RFmxWCDMA_SEMCfgAveraging(instrumentHandle, "", RFMXWCDMA_VAL_SEM_AVERAGING_ENABLED_FALSE, averagingCount, RFMXWCDMA_VAL_SEM_AVERAGING_TYPE_RMS));
    RFmxCheckWarn(RFmxWCDMA_Initiate(instrumentHandle, "", ""));

	RFmxCheckWarn(RFmxWCDMA_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, 0, &ACPLowerOffsetMeasArraySize));
	if( ACPLowerOffsetMeasArraySize > 0 )
	{
		ACPLowerRelativePower = (float64 *) malloc(sizeof(float64) * ACPLowerOffsetMeasArraySize);
		ACPUpperRelativePower = (float64 *) malloc(sizeof(float64) * ACPLowerOffsetMeasArraySize);
		ACPLowerAbsolutePower = (float64 *) malloc(sizeof(float64) * ACPLowerOffsetMeasArraySize);
		ACPUpperAbsolutePower = (float64 *) malloc(sizeof(float64) * ACPLowerOffsetMeasArraySize);
		if( ACPLowerRelativePower && ACPUpperRelativePower && ACPLowerAbsolutePower && ACPUpperAbsolutePower )
		{
			RFmxCheckWarn(RFmxWCDMA_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, 
																	ACPLowerRelativePower, 
																	ACPUpperRelativePower, 
																	ACPLowerAbsolutePower, 
																	ACPUpperAbsolutePower, 
																	ACPLowerOffsetMeasArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}
	
	RFmxCheckWarn(RFmxWCDMA_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, 0, &SEMLowerOffsetMeasArraySize));
	if( SEMLowerOffsetMeasArraySize > 0 )
	{
		SEMLowerOffsetMeasurementStatus = (int32 *) malloc(sizeof(int32) * SEMLowerOffsetMeasArraySize);
		SEMLowerOffsetMargin = (float64 *) malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
		SEMLowerOffsetMarginFrequency = (float64 *) malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
		SEMLowerOffsetMarginAbsolutePower = (float64 *) malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
		SEMLowerOffsetMarginRelativePower = (float64 *) malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
		if( SEMLowerOffsetMeasurementStatus && SEMLowerOffsetMargin && SEMLowerOffsetMarginFrequency && SEMLowerOffsetMarginAbsolutePower && 
			SEMLowerOffsetMarginRelativePower )
		{
			RFmxCheckWarn(RFmxWCDMA_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, SEMLowerOffsetMeasurementStatus, 
											SEMLowerOffsetMargin, SEMLowerOffsetMarginFrequency, SEMLowerOffsetMarginAbsolutePower, 
											SEMLowerOffsetMarginRelativePower, SEMLowerOffsetMeasArraySize, NULL ));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxWCDMA_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, 0, &SEMUpperOffsetMeasArraySize));
	if( SEMUpperOffsetMeasArraySize > 0 )
	{
		SEMUpperOffsetMeasurementStatus = (int32 *) malloc(sizeof(int32) * SEMUpperOffsetMeasArraySize);
		SEMUpperOffsetMargin = (float64 *) malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
		SEMUpperOffsetMarginFrequency = (float64 *) malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
		SEMUpperOffsetMarginAbsolutePower = (float64 *) malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
		SEMUpperOffsetMarginRelativePower = (float64 *) malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
		if( SEMUpperOffsetMeasurementStatus && SEMUpperOffsetMargin && SEMUpperOffsetMarginFrequency && SEMUpperOffsetMarginAbsolutePower && 
			SEMUpperOffsetMarginRelativePower )
		{
			RFmxCheckWarn(RFmxWCDMA_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, SEMUpperOffsetMeasurementStatus, 
													SEMUpperOffsetMargin, SEMUpperOffsetMarginFrequency,SEMUpperOffsetMarginAbsolutePower, 
													SEMUpperOffsetMarginRelativePower, SEMUpperOffsetMeasArraySize, NULL ));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}
   
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchEVM(instrumentHandle, "", timeout, &rmsEVM, &peakEVM, &rho, &frequencyError, &chipRateError, 
											&rmsMagnitudeError, &rmsPhaseError));
    RFmxCheckWarn(RFmxWCDMA_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &SEMMeasurementStatus));
    RFmxCheckWarn(RFmxWCDMA_ACPFetchCarrierMeasurement(instrumentHandle, "", timeout, &ACPAbsolutePower, &ACPRelativePower));
    RFmxCheckWarn(RFmxWCDMA_CHPFetchCarrierMeasurement(instrumentHandle, "", timeout, &CHPAbsolutePower, &CHPRelativePower));
    RFmxCheckWarn(RFmxWCDMA_OBWFetchMeasurement(instrumentHandle, "", timeout, &OBWOccupiedBandwidth, &OBWAbsolutePower, &OBWStartFrequency, 
															&OBWStopFrequency));
    RFmxCheckWarn(RFmxWCDMA_SEMFetchCarrierMeasurement(instrumentHandle, "", timeout, &SEMAbsoluteIntegratedPower, &SEMRelativeIntegratedPower));

	printf("************************* ModAcc *************************\n");
	printf("---------------Measurement---------------\n");
	printf("RMS EVM (%%)			: %lf\n",rmsEVM);
	printf("Peak EVM (%%)			: %lf\n",peakEVM);
	printf("Rho				: %lf\n",rho);
	printf("Frequency Error (Hz)		: %lf\n",frequencyError);
	printf("Chip Rate Error (ppm)		: %lf\n",chipRateError);
	printf("RMS Magnitude Error (%%)      	: %lf\n",rmsMagnitudeError);
	printf("RMS Phase Error (deg)		: %lf\n",rmsPhaseError);

	printf("\n************************* ACP *************************\n");
	printf("Carrier Absolute Power (dBm)	:  %lf\n",ACPAbsolutePower); 

	for( i = 0; i < ACPLowerOffsetMeasArraySize; i++ )
	{
		printf("\nOffset Channel Measurements	:  %d\n", i);
		printf("Lower Relative Power (dB)	:  %lf\n",ACPLowerRelativePower[i]);
		printf("Upper Relative Power (dB)	:  %lf\n",ACPUpperRelativePower[i]);
		printf("Lower Absolute Power (dBm)	:  %lf\n",ACPLowerAbsolutePower[i]);
		printf("Upper Absolute Power (dBm)	:  %lf\n",ACPUpperAbsolutePower[i]);
	}

	printf("\n************************* CHP *************************\n");
	printf("Carrier Absolute Power (dBm)	:  %lf\n",CHPAbsolutePower); 
	
	printf("\n************************* OBW *************************\n");
	printf("---------------Measurement---------------\n");
	printf("Occupied Bandwidth (Hz)		:  %lf\n",OBWOccupiedBandwidth);
	printf("Absoulte Power (dBm)		:  %lf\n",OBWAbsolutePower);
	printf("Start Frequency (Hz)		:  %lf\n",OBWStartFrequency);
	printf("Stop Frequency (Hz)		:  %lf\n",OBWStopFrequency);
	
	printf("\n************************* SEM *************************\n");
	printf("Measurement Status		        :  %s\n",SEMMeasurementStatus?"Pass":"Fail");
	printf("Carrier Absolute Integrated Power (dBm)	:  %lf\n",SEMAbsoluteIntegratedPower); 
	printf("\n---------------Lower Offset---------------\n");
	for( i = 0; i < SEMLowerOffsetMeasArraySize; i++ )
	{
		printf("\nOffset Channel Measurements	:  %d\n", i);
		printf("Margin (dB)			:  %lf\n",SEMLowerOffsetMargin[i]);
		printf("Margin Absolute Power (dBm)	:  %lf\n",SEMLowerOffsetMarginAbsolutePower[i]);
		printf("Margin Relative Power (dB)	:  %lf\n",SEMLowerOffsetMarginRelativePower[i]);
		printf("Margin Frequency (Hz)		:  %lf\n",SEMLowerOffsetMarginFrequency[i]);
		printf("Measurement Status              : %s\n", 
			  (SEMLowerOffsetMeasurementStatus[i])? " PASS" : " FAIL");
	}
	printf("\n---------------Upper Offset---------------\n");
	for( i = 0; i < SEMUpperOffsetMeasArraySize; i++ )
	{
		printf("\nOffset Channel Measurements	:  %d\n", i);
		printf("Margin (dB)			:  %lf\n",SEMUpperOffsetMargin[i]);
		printf("Margin Absolute Power (dBm)	:  %lf\n",SEMUpperOffsetMarginAbsolutePower[i]);
		printf("Margin Relative Power (dB)	:  %lf\n",SEMUpperOffsetMarginRelativePower[i]);
		printf("Margin Frequency (Hz)		:  %lf\n",SEMUpperOffsetMarginFrequency[i]);
		printf("Measurement Status              : %s\n", 
			  (SEMUpperOffsetMeasurementStatus[i])? " PASS" : " FAIL");
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

	if (ACPLowerAbsolutePower) 
	{
        free(ACPLowerAbsolutePower);
    }
    if (ACPUpperAbsolutePower) 
	{
        free(ACPUpperAbsolutePower);
    }
    if (ACPLowerRelativePower) 
	{
        free(ACPLowerRelativePower);
    }
    if (ACPUpperRelativePower) 
	{
        free(ACPUpperRelativePower);
    }
    if (SEMLowerOffsetMarginRelativePower)
    {
        free(SEMLowerOffsetMarginRelativePower);
    }
    if (SEMLowerOffsetMarginAbsolutePower)
    {
        free(SEMLowerOffsetMarginAbsolutePower);
    }
    if (SEMLowerOffsetMeasurementStatus) 
	{
        free(SEMLowerOffsetMeasurementStatus);
    }
    if (SEMLowerOffsetMargin) 
	{
        free(SEMLowerOffsetMargin);
    }
    if (SEMLowerOffsetMarginFrequency) 
	{
        free(SEMLowerOffsetMarginFrequency);
    };
    if (SEMUpperOffsetMarginRelativePower)
    {
        free(SEMUpperOffsetMarginRelativePower);
    }
    if (SEMUpperOffsetMarginAbsolutePower)
    {
        free(SEMUpperOffsetMarginAbsolutePower);
    }
    if (SEMUpperOffsetMeasurementStatus) 
	{
        free(SEMUpperOffsetMeasurementStatus);
    }
    if (SEMUpperOffsetMargin) 
	{
        free(SEMUpperOffsetMargin);
    }
    if (SEMUpperOffsetMarginFrequency) 
	{
        free(SEMUpperOffsetMarginFrequency);
    };			

    printf("Press any key to exit\n");
    _getch();

    return error;
}
