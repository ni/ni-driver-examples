//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters..
//5. Select CDA measurement and enable traces.
//6. Configure Synchronization Mode and Offset.
//7. Configure Measurement Channel.
//8. Configure Power Unit.
//9. Initiate the Measurement.
//10. Fetch CDA Measurements and Traces.
//11. Close the RFmx session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxTDSCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096
#define MAX_NUMBER_OF_USERS         16

int main()
{
	//RFSA Configuration
	char *rfsaResourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;
	char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
	int32 error = 0;
	int32 lastErrorCode = 0;

	/*Frequency Reference*/
	char * frequencyReferenceSource = RFMXTDSCDMA_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10E+6;    /*(Hz) */
	float64 centerFrequency = 1.91E+9;              /*(Hz) */

	float64 referenceLevel = 0.00;                  /*(dBm) */
	float64 externalAttenuation = 0.00;             /*(dB) */

	/*Trigger */
	int32 enableTrigger = RFMXTDSCDMA_VAL_TRUE;

	int32 averagingEnabled = RFMXTDSCDMA_VAL_CDA_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;

	int32 midambleShift = 8;
	int32 maximumNumberOfUsers = MAX_NUMBER_OF_USERS;
	int32 midambleAutoDetectionMode = RFMXTDSCDMA_VAL_MIDAMBLE_AUTO_DETECTION_MODE_MIDAMBLE_SHIFT;

	char * IQPowerEdgeSource = "0";
	int32 IQPowerEdgeSlope = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_RISING_SLOPE;
	float64 IQPowerEdgeLevel = -20.00;              /*(dB) */
	int32 IQPowerEdgeLevelType = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
	float64 triggerDelay = 0.00;                    /*(s) */

	int32 minimumQuietTimeMode = RFMXTDSCDMA_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
	float64 minimumQuietTime = 16E-6;               /*(s) */


	/*Measurement Settings*/
	int32 synchronizationMode = RFMXTDSCDMA_VAL_CDA_SYNCHRONIZATION_MODE_SLOT;
	int32 measurementOffset = 0;                    /*(slots) */

	int32 spreadingFactor = 16;
	int32 channelizationCode = 1;
	int32 powerUnit = RFMXTDSCDMA_VAL_CDA_POWER_UNIT_DB;

	float64 timeout = 10.00;					    /*(s) */

	/*Variables to Store Results*/
	/*Symbol EVM*/

	float64 meanRMSSymbolEVM = 0.00;				/*(%) */
	float64 maximumPeakSymbolEVM = 0.00;			/*(%) */
	float64 frequencyError = 0.00;				    /*(Hz) */
	float64 chipRateError = 0.00;				    /*(ppm) */
	float64 meanRMSSymbolMagnitudeError = 0.00;     /*(%) */
	float64 meanRMSSymbolPhaseError = 0.00;         /*(deg) */
	float64 meanSymbolPower = 0.00;				    /*(dB or dBm) */

	/*Code Domain Power*/

	float64 meanTotalPower = 0.00;				    /*(dBm) */
	float64 meanTotalActivePower = 0.00;			/*(dB or dBm) */
	float64 meanActivePower = 0.00;				    /*(dB or dBm) */
	float64 maximumPeakActivePower = 0.00;		    /*(dB or dBm) */
	float64 meanInactivePower = 0.00;				/*(dB or dBm) */
	float64 maximumPeakInactivePower = 0.00;		/*(dB or dBm) */

	/*IQ Impairments*/

	float64 IQOriginOffset = 0.00;				    /*(dB) */
	float64 IQGainImbalance = 0.00;				    /*(dB) */
	float64 IQQuadratureError = 0.00;				/*(deg) */

	float32 *codeDomainPowers = NULL;
	float32 *symbolEVM = NULL;
	int32   actualArraySize = 0;
	NIComplexSingle* constellation = (NIComplexSingle*)NULL;


	/* Initialize a session */
	RFmxCheckWarn(RFmxTDSCDMA_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxTDSCDMA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxTDSCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxTDSCDMA_CfgIQPowerEdgeTrigger(instrumentHandle, "", IQPowerEdgeSource, IQPowerEdgeSlope,
		IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode,
		minimumQuietTime, IQPowerEdgeLevelType, enableTrigger));
	RFmxCheckWarn(RFmxTDSCDMA_SelectMeasurements(instrumentHandle, "", RFMXTDSCDMA_VAL_CDA, RFMXTDSCDMA_VAL_TRUE));

	RFmxCheckWarn(RFmxTDSCDMA_CDACfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
	RFmxCheckWarn(RFmxTDSCDMA_CDACfgSynchronizationModeAndOffset(instrumentHandle, "", synchronizationMode,
		measurementOffset));
	RFmxCheckWarn(RFmxTDSCDMA_CDACfgMeasurementChannel(instrumentHandle, "", spreadingFactor, channelizationCode));
	RFmxCheckWarn(RFmxTDSCDMA_CDACfgPowerUnit(instrumentHandle, "", powerUnit));

	RFmxCheckWarn(RFmxTDSCDMA_CfgMidambleShift(instrumentHandle, "", midambleAutoDetectionMode, maximumNumberOfUsers, midambleShift));
	RFmxCheckWarn(RFmxTDSCDMA_CfgUplinkScramblingCode(instrumentHandle, "", 0));

	RFmxCheckWarn(RFmxTDSCDMA_Initiate(instrumentHandle, "", ""));

	RFmxCheckWarn(RFmxTDSCDMA_CDAFetchIQImpairments(instrumentHandle, "", timeout, &IQOriginOffset, &IQGainImbalance,
		&IQQuadratureError));
	RFmxCheckWarn(RFmxTDSCDMA_CDAFetchCodeDomainPower(instrumentHandle, "", timeout, &meanTotalPower, &meanTotalActivePower,
		&meanActivePower, &maximumPeakActivePower, &meanInactivePower, &maximumPeakInactivePower));
	RFmxCheckWarn(RFmxTDSCDMA_CDAFetchSymbolEVM(instrumentHandle, "", timeout, &meanRMSSymbolEVM, &maximumPeakSymbolEVM,
		&frequencyError, &chipRateError, &meanRMSSymbolMagnitudeError, &meanRMSSymbolPhaseError, &meanSymbolPower));
	RFmxCheckWarn(RFmxTDSCDMA_CDAFetchMeanCodeDomainPowerTrace(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));

	if (actualArraySize > 0)
	{
		codeDomainPowers = (float32*)malloc(sizeof(float32) * actualArraySize);
		if (codeDomainPowers)
		{
			RFmxCheckWarn(RFmxTDSCDMA_CDAFetchMeanCodeDomainPowerTrace(instrumentHandle, "", timeout, codeDomainPowers,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxTDSCDMA_CDAFetchMeanSymbolEVMTrace(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		symbolEVM = (float32*)malloc(sizeof(float32) * actualArraySize);
		if (symbolEVM)
		{
			RFmxCheckWarn(RFmxTDSCDMA_CDAFetchMeanSymbolEVMTrace(instrumentHandle, "", timeout, symbolEVM,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxTDSCDMA_CDAFetchSymbolConstellationTrace(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		constellation = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * actualArraySize);
		if (constellation)
		{
			RFmxCheckWarn(RFmxTDSCDMA_CDAFetchSymbolConstellationTrace(instrumentHandle, "", timeout, constellation,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	printf("Code Domain Power\n");
	printf("Mean Total Power(dBm)                 : %lf\n", meanTotalPower);
	printf("Mean Total Active Power(dB or dBm)    : %lf\n", meanTotalActivePower);
	printf("Mean Active Power(dB or dBm)          : %lf\n", meanActivePower);
	printf("Maximum Peak Active Power(dB or dBm)  : %lf\n", maximumPeakActivePower);
	printf("Mean Inactive Power(dB or dBm)        : %lf\n", meanInactivePower);
	printf("Maximum Peak Inactive Power(dB or dBm): %lf\n", maximumPeakInactivePower);

	printf("\nSymbol EVM\n");
	printf("Mean RMS Symbol EVM (%%)               : %lf\n", meanRMSSymbolEVM);
	printf("Maximum Peak Symbol EVM (%%)           : %lf\n", maximumPeakSymbolEVM);
	printf("Frequency  Error (Hz)                 : %lf\n", frequencyError);
	printf("Chip Rate Error (ppm)                 : %lf\n", chipRateError);
	printf("Mean RMS Symbol Magnitude Error(%%)    : %lf\n", meanRMSSymbolMagnitudeError);
	printf("Mean RMS Symbol Phase Error (deg)     : %lf\n", meanRMSSymbolPhaseError);
	printf("Mean Symbol Power  (dB or dBm)        : %lf\n", meanSymbolPower);

	printf("\nIQ Impairments\n");
	printf("IQ Origin Offset  (dB)                : %lf\n", IQOriginOffset);
	printf("IQ Gain Imbalance  (dB)               : %lf\n", IQGainImbalance);
	printf("IQ Quadrature Error  (deg)            : %lf\n", IQQuadratureError);

Error:
	if (error)
	{
		RFmxTDSCDMA_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if (error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}
	if (instrumentHandle)
	{
		RFmxTDSCDMA_Close(instrumentHandle, RFMXTDSCDMA_VAL_FALSE);
	}
    if (codeDomainPowers)
    {
        free(codeDomainPowers);
    }
    if (symbolEVM)
    {
        free(symbolEVM);
    }
    if (constellation)
    {
        free(constellation);
    }

	printf("Press any key to exit\n");
	_getch();

	return error;
}
