//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Contiguous Carriers.
//6. Select ACP, CHP, OBW and SEM measurements and enable Traces.
//7. Configure Sweep Time Parameters for ACP.
//8. Configure Averaging Parameters for ACP.
//9. Configure Sweep Time Parameters for CHP.
//10. Configure Averaging Parameters for CHP.
//11. Configure Sweep Time Parameters for OBW.
//12. Configure Averaging Parameters for OBW.
//13. Configure Sweep Time Parameters for SEM.
//14. Configure Averaging Parameters for SEM.
//15. Initiate the Measurement.
//16. Fetch ACP, CHP, OBW and SEM Measurements.
//17. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxEVDO.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION               4096
#define NUMBER_OF_CARRIERS                   3

int main ()
{
	//RFSA Configuration
	char *rfsaResourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;    

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0;
	int32 lastErrorCode = 0;
	int i = 0;

	char * frequencyReferenceSource = "OnboardClock";
	float64 frequencyReferenceFrequency = 10e6;/*(Hz) */

	float64 centerFrequency = 833490000.000000;/*(Hz) */
	float64 referenceLevel = 0.000000;/*(dBm) */
	float64 externalAttenuation = 0.000000;/*(dB) */
	int32 enableTrigger = RFMXEVDO_VAL_FALSE;
	char * digitalEdgeSource = RFMXEVDO_VAL_PFI0_STR;
	int32 digitalEdge = RFMXEVDO_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.000000;/*(s) */
	float64 timeout = 10.0;/*(s) */

	int32 carrierAtCenterFrequency = -1;
	int32 bandClass = 0;

	float64 sweepTimeInterval = 0.001670;/*(s) */
	int32 averagingCount = 10;

	float64 CHPTotalCarrierPower = 0.0;/*(dBm) */    
	float64 CHPCarrierAbsolutePower[NUMBER_OF_CARRIERS] = {0};/*(dBm) */
	float64 CHPCarrierRelativePower[NUMBER_OF_CARRIERS] = {0};/*(dB) */ 

	float64 ACPTotalCarrierPower = 0.0;/*(dBm) */
	float64 ACPAbsolutePower[NUMBER_OF_CARRIERS] = {0};/*(dBm) */
	float64 ACPRelativePower[NUMBER_OF_CARRIERS] = {0};/*(dB) */ 
	int32 ACPOffsetMeasArraySize = 0;
	float64* ACPLowerAbsolutePower = NULL;/*(dBm) */
	float64* ACPUpperAbsolutePower = NULL;/*(dBm) */
	float64* ACPLowerRelativePower = NULL;/*(dB) */
	float64* ACPUpperRelativePower = NULL;/*(dB) */

	float64 OBWOccupiedBandwidth = 0.0;/*(Hz) */
	float64 OBWAbsolutePower = 0.0;/*(dBm) */
	float64 OBWStopFrequency = 0.0;/*(Hz) */
	float64 OBWStartFrequency = 0.0;/*(Hz) */

	int32 SEMMeasurementStatus = 0;
	float64 SEMTotalCarrierPower = 0.0;/*(dBm) */
	int32 SEMCCMeasArraySize = 0;
	float64* SEMAbsoluteIntegratedPower = NULL;/*(dBm) */
	float64* SEMRelativeIntegratedPower = NULL;/*(dB) */
	int32 SEMLowerOffsetMeasArraySize = 0, SEMUpperOffsetMeasArraySize = 0;
	int32* SEMLowerOffsetMeasurementStatus = NULL;
	float64* SEMLowerOffsetMargin = NULL;/*(dB) */
	float64* SEMLowerOffsetMarginFrequency = NULL;/*(Hz) */
	float64* SEMLowerOffsetMarginAbsolutePower = NULL;/*(dBm) */
	float64* SEMLowerOffsetMarginRelativePower = NULL;/*(dBm) */
	int32* SEMUpperOffsetMeasurementStatus = NULL;
	float64* SEMUpperOffsetMargin = NULL;/*(dB) */
	float64* SEMUpperOffsetMarginFrequency = NULL;/*(Hz) */
	float64* SEMUpperOffsetMarginAbsolutePower = NULL;/*(dBm) */
	float64* SEMUpperOffsetMarginRelativePower = NULL;/*(dBm) */

	/* Initialize a session */
	RFmxCheckWarn(RFmxEVDO_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));

	RFmxCheckWarn(RFmxEVDO_CfgFrequencyReference(instrumentHandle, "",frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxEVDO_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxEVDO_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxEVDO_CfgContiguousCarriers(instrumentHandle, "", NUMBER_OF_CARRIERS, carrierAtCenterFrequency, bandClass));

	RFmxCheckWarn(RFmxEVDO_SelectMeasurements(instrumentHandle, "", RFMXEVDO_VAL_ACP | RFMXEVDO_VAL_CHP | 
		RFMXEVDO_VAL_OBW | RFMXEVDO_VAL_SEM, RFMXEVDO_VAL_TRUE));
	RFmxCheckWarn(RFmxEVDO_ACPCfgSweepTime(instrumentHandle, "", RFMXEVDO_VAL_ACP_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
	RFmxCheckWarn(RFmxEVDO_ACPCfgAveraging(instrumentHandle, "", RFMXEVDO_VAL_ACP_AVERAGING_ENABLED_FALSE, averagingCount, RFMXEVDO_VAL_ACP_AVERAGING_TYPE_RMS));
	RFmxCheckWarn(RFmxEVDO_CHPCfgSweepTime(instrumentHandle, "", RFMXEVDO_VAL_CHP_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
	RFmxCheckWarn(RFmxEVDO_CHPCfgAveraging(instrumentHandle, "", RFMXEVDO_VAL_CHP_AVERAGING_ENABLED_FALSE, averagingCount, RFMXEVDO_VAL_CHP_AVERAGING_TYPE_RMS));
	RFmxCheckWarn(RFmxEVDO_OBWCfgSweepTime(instrumentHandle, "", RFMXEVDO_VAL_OBW_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
	RFmxCheckWarn(RFmxEVDO_OBWCfgAveraging(instrumentHandle, "", RFMXEVDO_VAL_OBW_AVERAGING_ENABLED_FALSE, averagingCount, RFMXEVDO_VAL_OBW_AVERAGING_TYPE_RMS));
	RFmxCheckWarn(RFmxEVDO_SEMCfgSweepTime(instrumentHandle, "", RFMXEVDO_VAL_SEM_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
	RFmxCheckWarn(RFmxEVDO_SEMCfgAveraging(instrumentHandle, "", RFMXEVDO_VAL_SEM_AVERAGING_ENABLED_FALSE, averagingCount, RFMXEVDO_VAL_SEM_AVERAGING_TYPE_RMS));
	RFmxCheckWarn(RFmxEVDO_Initiate(instrumentHandle, "", ""));

	// ACP Fetches
	RFmxCheckWarn(RFmxEVDO_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, 
		NULL, NULL, NULL, NULL, 0, &ACPOffsetMeasArraySize));
	if( ACPOffsetMeasArraySize > 0 )
	{
		ACPLowerRelativePower = (float64 *) malloc(sizeof(float64) * ACPOffsetMeasArraySize);
		ACPUpperRelativePower = (float64 *) malloc(sizeof(float64) * ACPOffsetMeasArraySize);
		ACPLowerAbsolutePower = (float64 *) malloc(sizeof(float64) * ACPOffsetMeasArraySize);
		ACPUpperAbsolutePower = (float64 *) malloc(sizeof(float64) * ACPOffsetMeasArraySize);
		RFmxCheckWarn(RFmxEVDO_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout,
			ACPLowerRelativePower, ACPUpperRelativePower,
			ACPLowerAbsolutePower, ACPUpperAbsolutePower,
			ACPOffsetMeasArraySize, NULL));
	}

	RFmxCheckWarn(RFmxEVDO_ACPFetchCarrierMeasurementArray(instrumentHandle, "", timeout,
		ACPAbsolutePower, ACPRelativePower,
		NUMBER_OF_CARRIERS, NULL));

	RFmxCheckWarn(RFmxEVDO_ACPFetchTotalCarrierPower(instrumentHandle, "", timeout, &ACPTotalCarrierPower));

	// CHP Fetches

	RFmxCheckWarn(RFmxEVDO_CHPFetchCarrierMeasurementArray(instrumentHandle, "", timeout,
		CHPCarrierAbsolutePower, CHPCarrierRelativePower,
		NUMBER_OF_CARRIERS, NULL));

	RFmxCheckWarn(RFmxEVDO_CHPFetchTotalCarrierPower(instrumentHandle, "", timeout, &CHPTotalCarrierPower));

	// SEM Fetches
	RFmxCheckWarn(RFmxEVDO_SEMFetchCarrierMeasurementArray(instrumentHandle, "", timeout,
		NULL, NULL, 0, &SEMCCMeasArraySize));
	if( SEMCCMeasArraySize > 0 )
	{
		SEMAbsoluteIntegratedPower = (float64 *) malloc(sizeof(float64) * SEMCCMeasArraySize);
		SEMRelativeIntegratedPower = (float64 *) malloc(sizeof(float64) * SEMCCMeasArraySize);
		RFmxCheckWarn(RFmxEVDO_SEMFetchCarrierMeasurementArray(instrumentHandle, "", timeout,
			SEMAbsoluteIntegratedPower, SEMRelativeIntegratedPower,
			SEMCCMeasArraySize, NULL));
	}

	RFmxCheckWarn(RFmxEVDO_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, 
		NULL, NULL, NULL, 0, &SEMLowerOffsetMeasArraySize 
		));
	if( SEMLowerOffsetMeasArraySize > 0 )
	{
		SEMLowerOffsetMeasurementStatus = (int32 *) malloc(sizeof(int32) * SEMLowerOffsetMeasArraySize);
		SEMLowerOffsetMargin = (float64 *) malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
		SEMLowerOffsetMarginFrequency = (float64 *) malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
		SEMLowerOffsetMarginAbsolutePower = (float64 *) malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
		SEMLowerOffsetMarginRelativePower = (float64 *) malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
		RFmxCheckWarn(RFmxEVDO_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout,
			SEMLowerOffsetMeasurementStatus,
			SEMLowerOffsetMargin, 
			SEMLowerOffsetMarginFrequency,
			SEMLowerOffsetMarginAbsolutePower,
			SEMLowerOffsetMarginRelativePower,
			SEMLowerOffsetMeasArraySize, NULL));
	}

	RFmxCheckWarn(RFmxEVDO_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, 
		NULL, NULL, NULL, 0, &SEMUpperOffsetMeasArraySize));
	if( SEMUpperOffsetMeasArraySize > 0 )
	{
		SEMUpperOffsetMeasurementStatus = (int32 *) malloc(sizeof(int32) * SEMUpperOffsetMeasArraySize);
		SEMUpperOffsetMargin = (float64 *) malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
		SEMUpperOffsetMarginFrequency = (float64 *) malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
		SEMUpperOffsetMarginAbsolutePower = (float64 *) malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
		SEMUpperOffsetMarginRelativePower = (float64 *) malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
		RFmxCheckWarn(RFmxEVDO_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout,
			SEMUpperOffsetMeasurementStatus, 
			SEMUpperOffsetMargin, 
			SEMUpperOffsetMarginFrequency,
			SEMUpperOffsetMarginAbsolutePower,
			SEMUpperOffsetMarginRelativePower,
			SEMUpperOffsetMeasArraySize, NULL));
	}

	RFmxCheckWarn(RFmxEVDO_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &SEMMeasurementStatus));

	RFmxCheckWarn(RFmxEVDO_SEMFetchTotalCarrierPower(instrumentHandle, "", timeout, &SEMTotalCarrierPower));

	// OBW Fetches
	RFmxCheckWarn(RFmxEVDO_OBWFetchMeasurement(instrumentHandle, "", timeout, &OBWOccupiedBandwidth, &OBWAbsolutePower, &OBWStartFrequency, &OBWStopFrequency));

	printf("\n************************* ACP *************************\n\n");
	printf("Total Carrier Power  (dBm)      : %lf\n", ACPTotalCarrierPower);
	printf("\nCarrier Measurements: \n");
	for(i=0;i<NUMBER_OF_CARRIERS;i++)
	{
		printf("\nCarrier                         : %d\n", i); 
		printf("Absolute Power  (dBm)           : %lf\n", ACPAbsolutePower[i]);
		printf("Relative Power  (dB)            : %lf\n", ACPRelativePower[i]);
	}
	printf("\nOffset Channel Measurements: \n");
	for(i=0;i<ACPOffsetMeasArraySize;i++)
	{
		printf("\nOffset                          : %d\n", i);
		printf("Lower Relative Power (dB)       : %lf\n", ACPLowerRelativePower[i]);
		printf("Upper Relative Power (dB)       : %lf\n", ACPUpperRelativePower[i]);
		printf("Lower Absolute Power (dBm)      : %lf\n", ACPLowerAbsolutePower[i]);
		printf("Upper Absolute Power (dBm)      : %lf\n", ACPUpperAbsolutePower[i]);
	}

	printf("\n************************* CHP *************************\n\n");
	printf("Total Carrier Power  (dBm)      : %lf\n", CHPTotalCarrierPower);
	printf("\nCarrier Measurements: \n");
	for(i=0;i<NUMBER_OF_CARRIERS;i++)
	{        
		printf("\nCarrier                         : %d\n", i); 
		printf("Carrier Absolute Power  (dBm)   : %lf\n", CHPCarrierAbsolutePower[i]);
		printf("Carrier Relative Power  (dB)    : %lf\n", CHPCarrierRelativePower[i]);
	}

	printf("\n************************* OBW *************************\n\n");
	printf("Occupied Bandwidth  (Hz)        : %lf\n", OBWOccupiedBandwidth);
	printf("Absolute Power  (dBm)           : %lf\n", OBWAbsolutePower);
	printf("Start Frequency  (Hz)           : %lf\n", OBWStartFrequency);
	printf("Stop Frequency  (Hz)            : %lf\n", OBWStopFrequency);

	printf("\n************************* SEM *************************\n\n");
	printf("Measurement Status              : %s\n", (SEMMeasurementStatus)? "PASS" : "FAIL");
	printf("Total Carrier Power (dBm)       : %lf\n", SEMTotalCarrierPower);
	printf("\nCarrier Measurements: \n");
	for(i=0;i<SEMCCMeasArraySize;i++)
	{
		printf("\nCarrier                         : %d\n", i); 
		printf("Absolute Integrated Power  (dBm): %lf\n", SEMAbsoluteIntegratedPower[i]);
		printf("Relative Integrated Power  (dB) : %lf\n", SEMRelativeIntegratedPower[i]);
	}
	printf("\nLower Offset Segment Measurements: \n");
	for(i=0;i<SEMLowerOffsetMeasArraySize;i++)
	{
		printf("\nOffset                          : %d\n", i);
		printf("Margin  (dB)                    : %lf\n",SEMLowerOffsetMargin[i]);
		printf("Margin Absolute Power  (dBm)    : %lf\n",SEMLowerOffsetMarginAbsolutePower[i]);
		printf("Margin Relative Power  (dBm)    : %lf\n",SEMLowerOffsetMarginRelativePower[i]);
		printf("Margin Frequency  (Hz)          : %lf\n",SEMLowerOffsetMarginFrequency[i]);
		printf("Measurement Status              : %s\n", (SEMLowerOffsetMeasurementStatus[i])? "PASS" : "FAIL");
	}    
	printf("\nUpper Offset Segment Measurements: \n");
	for(i=0;i<SEMUpperOffsetMeasArraySize;i++)
	{
		printf("\nOffset                          : %d\n", i);
		printf("Margin  (dB)                    : %lf\n",SEMUpperOffsetMargin[i]);
		printf("Margin Absolute Power  (dBm)    : %lf\n",SEMUpperOffsetMarginAbsolutePower[i]);
		printf("Margin Relative Power  (dBm)    : %lf\n",SEMUpperOffsetMarginRelativePower[i]);
		printf("Margin Frequency  (Hz)          : %lf\n",SEMUpperOffsetMarginFrequency[i]);
		printf("Measurement Status              : %s\n", (SEMUpperOffsetMeasurementStatus[i])? "PASS" : "FAIL");
	}

Error:
	if( error )
	{
		RFmxEVDO_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if (error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}
	if(instrumentHandle)
	{
		RFmxEVDO_Close(instrumentHandle, RFMXEVDO_VAL_FALSE);
	}

	/* Free allocated memory */
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
    if (SEMAbsoluteIntegratedPower)
    {
        free(SEMAbsoluteIntegratedPower);
    }
    if (SEMRelativeIntegratedPower)
    {
        free(SEMRelativeIntegratedPower);
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
    }
    if (SEMLowerOffsetMarginAbsolutePower)
    {
        free(SEMLowerOffsetMarginAbsolutePower);
    }
    if (SEMLowerOffsetMarginRelativePower)
    {
        free(SEMLowerOffsetMarginRelativePower);
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
    }
    if (SEMUpperOffsetMarginAbsolutePower)
    {
        free(SEMUpperOffsetMarginAbsolutePower);
    }
    if (SEMUpperOffsetMarginRelativePower)
    {
        free(SEMUpperOffsetMarginRelativePower);
    }

	printf("Press any key to exit\n");
	_getch();

	return error;
}
