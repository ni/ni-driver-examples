//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Uplink Scrambling.
//6. Select CDA measurement and enable traces.
//7. Configure Synchronization Mode and Interval.
//8. Configure Measurement Channel.
//9. Configure Power Unit.
//10. Initiate the Measurement.
//11. Fetch CDA Measurements and Traces.
//12. Close the RFmx session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxWCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096
#define NUMBER_OF_OFFSETS			2

int main (int argc, char *argv[])
{	    
	niRFmxInstrHandle instrumentHandle = NULL;
	int32 error = 0, lastErrorCode = 0;
	char errorMessage[MAX_ERROR_DESCRIPTION] = {'\0'};

	char* resourceName = "RFSA";
	float64 centerFrequency = 1.95e+9;											/* Hz */
	float64 externalAttenuation = 0.00;											/* dB */

	float64 referenceLevel = 0.00;												/* dBm */


	/* Frequency Reference */
	char *frequencyReferenceSource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10.0e+6;												/* Hz */

	/* Trigger */
	int32 enableTrigger = RFMXWCDMA_VAL_FALSE;
	char* digitalEdgeTriggerSource = RFMXWCDMA_VAL_PFI0_STR;
	int32 digitalEdgeTriggerEdge =  RFMXWCDMA_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.0;													/* s */

	/* Uplink Scrambling */
	int32 uplinkScramblingType = RFMXWCDMA_VAL_UPLINK_SCRAMBLING_TYPE_LONG;
	int32 uplinkScramblingCode = 0;

	/* Synchronization Mode and Interval */
	int32 synchronizationMode = RFMXWCDMA_VAL_CDA_SYNCHRONIZATION_MODE_SLOT;
	int32 measurementOffset = 0;                                                /* slots */
	int32 measurementLength = 1;                                                /* slots */

	/* Measurement Channel */
	int32 spreadingFactor = 256;
	int32 spreadingCode = 0;													
	int32 modulationType =  RFMXWCDMA_VAL_MODULATION_TYPE_BPSK_QPSK;
	int32 branch = RFMXWCDMA_VAL_BRANCH_Q;

	int32 powerUnit = RFMXWCDMA_VAL_CDA_POWER_UNIT_DB;

	float64 timeout = 10.0;	
	int32 actualArraySize = 0;

	/* Variables to store CDA Code Domain Power */	
	float64 totalPower = 0.0;													/* dBm */
	float64 totalActivePower = 0.0;												/* dB or dBm */
	float64 meanActivePower = 0.0;												/* dB or dBm */
	float64 peakActivePower = 0.0;												/* dB or dBm */
	float64 meanInactivePower = 0.0;											/* dB or dBm */
	float64 peakInactivePower = 0.0;											/* dB or dBm */

	/* Variables to store Code Domain I and Q Power */	
	float64 IMeanActivePower = 0.0;												/* dB or dBm */					
	float64 QMeanActivePower = 0.0;												/* dB or dBm */
	float64 IPeakInactivePower = 0.0;											/* dB or dBm */
	float64 QPeakInactivePower = 0.0;											/* dB or dBm */

	/* Variables to store Symbol EVM */	
    float64 RMSSymbolEVM = 0.0;													/* % */
	float64 peakSymbolEVM = 0.0;												/* % */
	float64 RMSSymbolMagnitudeError = 0.0;										/* % */									
	float64 RMSSymbolPhaseError = 0.0;											/* deg */
	float64 meanSymbolPower = 0.0;												/* dB or dBm */
	float64 chipRateError = 0.0;												/* ppm */

	/* variables to store traces */
	float32* iCodeDomainPowers = NULL;											/* dB or dBm */
	float32* qCodeDomainPowers = NULL;											/* dB or dBm */
	float32* symbolEVM = NULL;													/* % */

	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxWCDMA_Initialize(resourceName, "", &instrumentHandle, NULL));

	/* Configure WCDMA CDA measurement parameters */
	RFmxCheckWarn(RFmxWCDMA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxWCDMA_CfgRF(instrumentHandle,"",centerFrequency,referenceLevel,externalAttenuation));
	RFmxCheckWarn(RFmxWCDMA_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxWCDMA_CfgUplinkScrambling(instrumentHandle, "", uplinkScramblingCode, uplinkScramblingType));
	RFmxCheckWarn(RFmxWCDMA_SelectMeasurements(instrumentHandle, "", RFMXWCDMA_VAL_CDA, RFMXWCDMA_VAL_TRUE));
	RFmxCheckWarn(RFmxWCDMA_CDACfgSynchronizationModeAndInterval(instrumentHandle,"", synchronizationMode, measurementOffset, measurementLength)); 
	RFmxCheckWarn(RFmxWCDMA_CDACfgMeasurementChannel(instrumentHandle, "",spreadingFactor, spreadingCode, modulationType , branch));
	RFmxCheckWarn(RFmxWCDMA_CDACfgPowerUnit(instrumentHandle, "", powerUnit));
	RFmxCheckWarn(RFmxWCDMA_Initiate(instrumentHandle,"",""));


	/* Fetch diverse CDA Measurement Results */	

	RFmxCheckWarn(RFmxWCDMA_CDAFetchSymbolEVM(instrumentHandle, "", timeout, &RMSSymbolEVM, &peakSymbolEVM, &RMSSymbolMagnitudeError, &RMSSymbolPhaseError, &meanSymbolPower, &chipRateError));


	RFmxCheckWarn(RFmxWCDMA_CDAFetchCodeDomainPower(instrumentHandle, "", timeout, &totalPower, &totalActivePower, &meanActivePower, &peakActivePower,
		                                                      &meanInactivePower, &peakInactivePower));

	RFmxCheckWarn(RFmxWCDMA_CDAFetchCodeDomainIAndQPower(instrumentHandle, "", timeout, &IMeanActivePower, &QMeanActivePower, &IPeakInactivePower, &QPeakInactivePower));
		

	
	

	RFmxCheckWarn(RFmxWCDMA_CDAFetchSymbolEVMTrace(instrumentHandle, "", timeout, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		symbolEVM = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if(symbolEVM)
		{
			RFmxCheckWarn(RFmxWCDMA_CDAFetchSymbolEVMTrace(instrumentHandle, "", timeout, symbolEVM, actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxWCDMA_CDAFetchCodeDomainIAndQPowerTrace(instrumentHandle, "", timeout, NULL, NULL, 0, 
		&actualArraySize));
	if( actualArraySize > 0 )
	{
		iCodeDomainPowers = (float32 *)malloc(sizeof(float32) * actualArraySize);
		qCodeDomainPowers = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if(iCodeDomainPowers && qCodeDomainPowers)
		{
			RFmxCheckWarn(RFmxWCDMA_CDAFetchCodeDomainIAndQPowerTrace(instrumentHandle, "", timeout, iCodeDomainPowers, qCodeDomainPowers, actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}




	/* Display Code Domain Power Results */	
	printf("\n---------------------- Code Domain Power -------------------\n");
	printf("Total Power (dBm)                           : %f\n", totalPower);
	printf("Total Active Power (dB or dBm)              : %f\n", totalActivePower);
	printf("Mean Inactive Power (dB or dBm)             : %f\n", meanInactivePower);
	printf("Peak Inactive Power (dB or dBm)             : %f\n", peakInactivePower);
	printf("I Peak Inactive Power (dB or dBm)           : %f\n", IPeakInactivePower);
	printf("Q Peak Inactive Power (dB or dBm)           : %f\n", QPeakInactivePower);



	/* Display Symbol EVM Results */	
	printf("\n------------------------- Symbol EVM -----------------------\n");
	printf("RMS Symbol EVM (%%)                          : %f\n", RMSSymbolEVM);
	printf("Peak Symbol EVM (%%)                         : %f\n", peakSymbolEVM);
	printf("RMS Symbol Magnitude Error (%%)              : %f\n", RMSSymbolMagnitudeError);
	printf("RMS Symbol Phase Error (deg)                : %f\n", RMSSymbolPhaseError);
	printf("Mean Symbol Power (dB or dBm)               : %f\n", meanSymbolPower);
	printf("Chip Rate Error (ppm)                       : %f\n", chipRateError);
	
Error:
	if( error ) 
	{
		RFmxWCDMA_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if(error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}
	if(instrumentHandle)
	{
		RFmxWCDMA_Close(instrumentHandle, RFMXWCDMA_VAL_FALSE);
	}

	/* Free allocated memory */	
	if(iCodeDomainPowers)
		free(iCodeDomainPowers);
	if(qCodeDomainPowers)
		free(qCodeDomainPowers);
	if(symbolEVM)
		free(symbolEVM);
	
	printf("\nPress any key to exit");
	_getch();
	return error;
}
