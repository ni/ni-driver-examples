//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select ModAcc, ACP, CHP, OBW and SEM measurements and enable Traces.
//6. Configure Uplink Spreading Parameters.
//7. Configure Uplink Data Modulation Type Parameter.
//8. Configure Physical Layer Subtype Parameter.
//9. Configure Synchronization Mode and Measurement Interval.
//10. Configure Sweep Time Parameters for ACP.
//11. Configure Averaging Parameters for ACP.
//12. Configure Sweep Time Parameters for CHP.
//13. Configure Averaging Parameters for CHP.
//14. Configure Sweep Time Parameters for OBW.
//15. Configure Averaging Parameters for OBW.
//16. Configure Sweep Time Parameters for SEM.
//17. Configure Averaging Parameters for SEM.
//18. Initiate the Measurement.
//19. Fetch ModAcc, ACP, CHP, OBW and SEM Measurements.
//20. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxEVDO.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main ()
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;    

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0, lastErrorCode = 0;
	int i = 0;

	char * frequencyReferenceSource = RFMXEVDO_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10000000.000000;            /*(Hz) */

	float64 centerFrequency = 833490000.000000;    /*(Hz) */
	float64 referenceLevel = 0.000000;                /*(dBm) */
	float64 externalAttenuation = 0.000000;            /*(dB) */

	int32 enableTrigger = RFMXEVDO_VAL_FALSE;
	char * digitalEdgeSource = RFMXEVDO_VAL_PFI0_STR;
	int32 digitalEdge = RFMXEVDO_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.000000;                /*(s) */

	int64 uplinkSpreadingIMask = 0x0;
	int64 uplinkSpreadingQMask = 0x0;
	int32 uplinkDataModulationType = RFMXEVDO_VAL_UPLINK_DATA_MODULATION_TYPE_AUTO;
	int32 physicalLayerSubtype = RFMXEVDO_VAL_PHYSICAL_LAYER_SUBTYPE_0_1;

	int32 synchronizationMode = RFMXEVDO_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;
	int32 measurementOffset = 0;                    /*(slots) */
	int32 measurementLength = 1;                    /*(slots) */

	float64 sweepTimeInterval = 0.001670;            /*(s) */
	int32 averagingCount = 10;

	int32 ACPLowerOffsetMeasArraySize = 0;
	float64* ACPLowerAbsolutePower = NULL;            /*(dBm) */
	float64* ACPUpperAbsolutePower = NULL;            /*(dBm) */
	float64* ACPLowerRelativePower = NULL;            /*(dB) */
	float64* ACPUpperRelativePower = NULL;            /*(dB) */
	float64 ACPTotalCarrierPower = 0.000000;        /*(dBm) */

	float64 timeout = 10.000000;                    /*(s) */
	float64 chipRateError = 0.000000;                /*(ppm) */
	float64 frequencyError = 0.000000;                /*(Hz) */
	float64 rmsEVM = 0.000000;                        /*(%) */
	float64 peakEVM = 0.000000;                        /*(%) */
	float64 rho = 0.000000;
	float64 rmsPhaseError = 0.000000;                /*(deg) */
	float64 rmsMagnitudeError = 0.000000;            /*(%) */
	int32 SEMMeasurementStatus = 0;

	float64 CHPTotalCarrierPower = 0.000000;        /*(dBm) */

	float64 OBWAbsolutePower = 0.000000;            /*(dBm) */
	float64 OBWStopFrequency = 0.000000;            /*(Hz) */
	float64 OBWStartFrequency = 0.000000;            /*(Hz) */
	float64 OBWOccupiedBandwidth = 0.000000;        /*(Hz) */

	float64 SEMTotalCarrierPower = 0.000000;        /*(dBm) */
	int32 SEMLowerOffsetMeasArraySize = 0, SEMUpperOffsetMeasArraySize = 0;
	float64* SEMLowerOffsetMarginRelativePower = NULL;        /*(dB) */
	float64* SEMLowerOffsetMarginAbsolutePower = NULL;        /*(dBm) */
	int32* SEMLowerOffsetMeasurementStatus = NULL;
	float64* SEMLowerOffsetMargin = NULL;                    /*(dB) */
	float64* SEMLowerOffsetMarginFrequency = NULL;            /*(Hz) */
	float64* SEMUpperOffsetMarginRelativePower = NULL;        /*(dB) */
	float64* SEMUpperOffsetMarginAbsolutePower = NULL;        /*(dBm) */
	int32* SEMUpperOffsetMeasurementStatus = NULL;
	float64* SEMUpperOffsetMargin = NULL;                    /*(dB) */
	float64* SEMUpperOffsetMarginFrequency = NULL;            /*(Hz) */

	/* Initialize a session */
	RFmxCheckWarn(RFmxEVDO_Initialize(resourceName, "", &instrumentHandle, NULL));

	RFmxCheckWarn(RFmxEVDO_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxEVDO_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxEVDO_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxEVDO_SelectMeasurements(instrumentHandle, "", RFMXEVDO_VAL_MODACC | RFMXEVDO_VAL_ACP | RFMXEVDO_VAL_CHP | 
		RFMXEVDO_VAL_OBW | RFMXEVDO_VAL_SEM, RFMXEVDO_VAL_TRUE));
	RFmxCheckWarn(RFmxEVDO_CfgUplinkSpreading(instrumentHandle, "", uplinkSpreadingIMask, uplinkSpreadingQMask));
	RFmxCheckWarn(RFmxEVDO_CfgUplinkDataModulationType(instrumentHandle, "", uplinkDataModulationType));
	RFmxCheckWarn(RFmxEVDO_CfgPhysicalLayerSubtype(instrumentHandle, "", physicalLayerSubtype));
	RFmxCheckWarn(RFmxEVDO_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode, measurementOffset,
		measurementLength));
	RFmxCheckWarn(RFmxEVDO_ACPCfgSweepTime(instrumentHandle, "", RFMXEVDO_VAL_ACP_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
	RFmxCheckWarn(RFmxEVDO_ACPCfgAveraging(instrumentHandle, "", RFMXEVDO_VAL_ACP_AVERAGING_ENABLED_FALSE, averagingCount,
		RFMXEVDO_VAL_ACP_AVERAGING_TYPE_RMS));
	RFmxCheckWarn(RFmxEVDO_CHPCfgSweepTime(instrumentHandle, "", RFMXEVDO_VAL_CHP_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
	RFmxCheckWarn(RFmxEVDO_CHPCfgAveraging(instrumentHandle, "", RFMXEVDO_VAL_CHP_AVERAGING_ENABLED_FALSE, averagingCount,
		RFMXEVDO_VAL_CHP_AVERAGING_TYPE_RMS));
	RFmxCheckWarn(RFmxEVDO_OBWCfgSweepTime(instrumentHandle, "", RFMXEVDO_VAL_OBW_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
	RFmxCheckWarn(RFmxEVDO_OBWCfgAveraging(instrumentHandle, "", RFMXEVDO_VAL_OBW_AVERAGING_ENABLED_FALSE, averagingCount
		, RFMXEVDO_VAL_OBW_AVERAGING_TYPE_RMS));
	RFmxCheckWarn(RFmxEVDO_SEMCfgSweepTime(instrumentHandle, "", RFMXEVDO_VAL_SEM_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
	RFmxCheckWarn(RFmxEVDO_SEMCfgAveraging(instrumentHandle, "", RFMXEVDO_VAL_SEM_AVERAGING_ENABLED_FALSE, averagingCount,
		RFMXEVDO_VAL_SEM_AVERAGING_TYPE_RMS));
	RFmxCheckWarn(RFmxEVDO_Initiate(instrumentHandle, "", ""));

	// ACP Fetches
	RFmxCheckWarn(RFmxEVDO_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, 0,
		&ACPLowerOffsetMeasArraySize));
	if( ACPLowerOffsetMeasArraySize > 0 )
	{
		ACPLowerRelativePower = (float64 *) malloc(sizeof(float64) * ACPLowerOffsetMeasArraySize);
		ACPUpperRelativePower = (float64 *) malloc(sizeof(float64) * ACPLowerOffsetMeasArraySize);
		ACPLowerAbsolutePower = (float64 *) malloc(sizeof(float64) * ACPLowerOffsetMeasArraySize);
		ACPUpperAbsolutePower = (float64 *) malloc(sizeof(float64) * ACPLowerOffsetMeasArraySize);
		if( ACPLowerRelativePower && ACPUpperRelativePower && ACPLowerAbsolutePower && ACPUpperAbsolutePower )
		{
			RFmxCheckWarn(RFmxEVDO_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, 
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

	RFmxCheckWarn(RFmxEVDO_ACPFetchTotalCarrierPower(instrumentHandle, "", timeout, &ACPTotalCarrierPower));

	// ModAcc Fetches
	RFmxCheckWarn(RFmxEVDO_ModAccFetchUplinkEVM(instrumentHandle, "", timeout, &rmsEVM, &peakEVM, &rho, &frequencyError,
		&chipRateError, &rmsMagnitudeError, &rmsPhaseError));

	// CHP Fetches
	RFmxCheckWarn(RFmxEVDO_CHPFetchTotalCarrierPower(instrumentHandle, "", timeout, &CHPTotalCarrierPower));

	// OBW Fetches
	RFmxCheckWarn(RFmxEVDO_OBWFetchMeasurement(instrumentHandle, "", timeout, &OBWOccupiedBandwidth, &OBWAbsolutePower, 
		&OBWStartFrequency, &OBWStopFrequency));

	// SEM Fetches
	RFmxCheckWarn(RFmxEVDO_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL,
		0, &SEMLowerOffsetMeasArraySize));
	if( SEMLowerOffsetMeasArraySize > 0 )
	{
		SEMLowerOffsetMeasurementStatus = (int32 *) malloc(sizeof(int32) * SEMLowerOffsetMeasArraySize);
		SEMLowerOffsetMargin = (float64 *) malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
		SEMLowerOffsetMarginFrequency = (float64 *) malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
		SEMLowerOffsetMarginAbsolutePower = (float64 *) malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
		SEMLowerOffsetMarginRelativePower = (float64 *) malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
		if( SEMLowerOffsetMeasurementStatus && SEMLowerOffsetMargin && SEMLowerOffsetMarginFrequency &&
			SEMLowerOffsetMarginAbsolutePower && SEMLowerOffsetMarginRelativePower )
		{
			RFmxCheckWarn(RFmxEVDO_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, SEMLowerOffsetMeasurementStatus, 
				SEMLowerOffsetMargin, SEMLowerOffsetMarginFrequency, SEMLowerOffsetMarginAbsolutePower, 
				SEMLowerOffsetMarginRelativePower, SEMLowerOffsetMeasArraySize, NULL ));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxEVDO_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, 0,
		&SEMUpperOffsetMeasArraySize));
	if( SEMUpperOffsetMeasArraySize > 0 )
	{
		SEMUpperOffsetMeasurementStatus = (int32 *) malloc(sizeof(int32) * SEMUpperOffsetMeasArraySize);
		SEMUpperOffsetMargin = (float64 *) malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
		SEMUpperOffsetMarginFrequency = (float64 *) malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
		SEMUpperOffsetMarginAbsolutePower = (float64 *) malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
		SEMUpperOffsetMarginRelativePower = (float64 *) malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
		if( SEMUpperOffsetMeasurementStatus && SEMUpperOffsetMargin && SEMUpperOffsetMarginFrequency &&
			SEMUpperOffsetMarginAbsolutePower && SEMUpperOffsetMarginRelativePower )
		{
			RFmxCheckWarn(RFmxEVDO_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, SEMUpperOffsetMeasurementStatus, 
				SEMUpperOffsetMargin, SEMUpperOffsetMarginFrequency,SEMUpperOffsetMarginAbsolutePower, 
				SEMUpperOffsetMarginRelativePower, SEMUpperOffsetMeasArraySize, NULL ));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxEVDO_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &SEMMeasurementStatus));

	RFmxCheckWarn(RFmxEVDO_SEMFetchTotalCarrierPower(instrumentHandle, "", timeout, &SEMTotalCarrierPower));



	printf("************************* ModAcc *************************\n");
	printf("---------------Measurement---------------\n");
	printf("RMS EVM (%%)                     : %lf\n",rmsEVM);
	printf("Peak EVM (%%)                    : %lf\n",peakEVM);
	printf("Rho                             : %lf\n",rho);
	printf("Frequency Error (Hz)            : %lf\n",frequencyError);
	printf("Chip Rate Error (ppm)           : %lf\n",chipRateError);
	printf("Rms Magnitude Error (%%)         : %lf\n",rmsMagnitudeError);
	printf("Rms Phase Error (deg)           : %lf\n",rmsPhaseError);

	printf("\n************************* ACP *************************\n");
	printf("Carrier Absolute Power (dBm)    :  %lf\n",ACPTotalCarrierPower); 
	for( i = 0; i < ACPLowerOffsetMeasArraySize; i++ )
	{
		printf("\nOffset Channel Measurements     :  %d\n", i);
		printf("Lower Relative Power (dB)       :  %lf\n",ACPLowerRelativePower[i]);
		printf("Upper Relative Power (dB)       :  %lf\n",ACPUpperRelativePower[i]);
		printf("Lower Absolute Power (dBm)      :  %lf\n",ACPLowerAbsolutePower[i]);
		printf("Upper Absolute Power (dBm)      :  %lf\n",ACPUpperAbsolutePower[i]);
	}

	printf("\n************************* CHP *************************\n");
	printf("Carrier Absolute Power (dBm)    :  %lf\n",CHPTotalCarrierPower); 

	printf("\n************************* OBW *************************\n");
	printf("---------------Measurement---------------\n");
	printf("Occupied Bandwidth (Hz)         :  %lf\n",OBWOccupiedBandwidth);
	printf("Absoulte Power (dBm)            :  %lf\n",OBWAbsolutePower);
	printf("Start Frequency (Hz)            :  %lf\n",OBWStartFrequency);
	printf("Stop Frequency (Hz)             :  %lf\n",OBWStopFrequency);

	printf("\n************************* SEM *************************\n");
	printf("Measurement Status              :  %s\n",(SEMMeasurementStatus)? "PASS" : "FAIL");
	printf("Carrier Absolute Power (dBm)    :  %lf\n",SEMTotalCarrierPower); 
	printf("\n---------------Lower Offset---------------\n");
	for( i = 0; i < SEMLowerOffsetMeasArraySize; i++ )
	{
		printf("\nOffset Channel Measurements     :  %d\n", i);
		printf("Margin (dB)                     :  %lf\n",SEMLowerOffsetMargin[i]);
		printf("Margin Absolute Power (dBm)     :  %lf\n",SEMLowerOffsetMarginAbsolutePower[i]);
		printf("Margin Relative Power (dB)      :  %lf\n",SEMLowerOffsetMarginRelativePower[i]);
		printf("Margin Frequency (Hz)           :  %lf\n",SEMLowerOffsetMarginFrequency[i]);
		printf("Measurement Status              :  %s\n",(SEMLowerOffsetMeasurementStatus[i])? "PASS" : "FAIL");
	}
	printf("\n---------------Upper Offset---------------\n");
	for( i = 0; i < SEMUpperOffsetMeasArraySize; i++ )
	{
		printf("\nOffset Channel Measurements     :  %d\n", i);
		printf("Margin (dB)                     :  %lf\n",SEMUpperOffsetMargin[i]);
		printf("Margin Absolute Power (dBm)     :  %lf\n",SEMUpperOffsetMarginAbsolutePower[i]);
		printf("Margin Relative Power (dB)      :  %lf\n",SEMUpperOffsetMarginRelativePower[i]);
		printf("Margin Frequency (Hz)           :  %lf\n",SEMUpperOffsetMarginFrequency[i]);
		printf("Measurement Status              :  %s\n",(SEMUpperOffsetMeasurementStatus[i])? "PASS" : "FAIL");
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
    }
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
    }

	printf("Press any key to exit\n");
	_getch();

	return error;
}
