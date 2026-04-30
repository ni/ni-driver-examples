//Steps:
//1. Open a new RFmx session and create a Demod Signal
//2. Configure the basic instrument properties (Clock Source, Clock Frequency) 
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select DDemod Measurement
//6. Configure FSK Modulation and M
//7. Configure DDemod FSK Deviation 
//8. Configure DDemod Symbol Rate, Number of Symbols and Pulse Shaping Filter
//9. Configure DDemod Measurement Filter Type as Auto
//10. Configure DDemod Averaging
//11. Initiate Measurement
//12. Read FSK Measurement Results
//13. Dispose Demod Signal and Close the RFmx Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxDemod.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char *argv[])
{
	niRFmxInstrHandle instrumentHandle = NULL;
	int32 error = 0, lastErrorCode = 0;
	char errorMessage[MAX_ERROR_DESCRIPTION];
		
	char *resourceName = "RFSA";
	char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR ;	
	char *selectedPorts = "";
	float64 centerFrequency = 1e+9;			/* Hz */
	float64 referenceLevel = 0.00;			/* dBm */
	float64 externalAttenuation = 0.00;		/* dB */
	float64 frequency = 10.0e+6;			/* Hz */
	float64 timeout = 10.0;					/* seconds */   
	
	int32 M = RFMXDEMOD_VAL_DDEMOD_M_2;
	
	float64 FSKDeviation = 15.000e+3;	/* Hz */
	float64 symbolRate = 100.000e+3;	/* Hz */
	int32 numOfSymbols = 1000;
	
	/* Pulse shaping filter */
	int32 pulseShapingFilterType = RFMXDEMOD_VAL_DDEMOD_PULSE_SHAPING_FILTER_TYPE_GAUSSIAN;
	float64 pulseShapingFilterParameter = 0.50;
	
	/* Averaging */
	int32 averagingEnabled = RFMXDEMOD_VAL_DDEMOD_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;

	/* Variables to store the measurement results */
	float64 meanFSKDeviation = 0; 
	float64 meanRMSFSKError = 0;
	float64 maximumPeakFSKError = 0;
	
	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxDemod_Initialize(resourceName,  "",  &instrumentHandle, NULL));
	
	/* Configure DDemod parameters */
	RFmxCheckWarn(RFmxDemod_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
	RFmxCheckWarn(RFmxDemod_SetSelectedPorts(instrumentHandle, "", selectedPorts));
	RFmxCheckWarn(RFmxDemod_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxDemod_SelectMeasurements(instrumentHandle, "", RFMXDEMOD_VAL_DDEMOD, RFMXDEMOD_VAL_TRUE));
	RFmxCheckWarn(RFmxDemod_DDemodCfgModulationType(instrumentHandle, "", RFMXDEMOD_VAL_DDEMOD_MODULATION_TYPE_FSK, 
												M, RFMXDEMOD_VAL_DDEMOD_DIFFERENTIAL_ENABLED_FALSE));
	RFmxCheckWarn(RFmxDemod_DDemodCfgFSKDeviation(instrumentHandle, "", FSKDeviation, 
											  RFMXDEMOD_VAL_DDEMOD_FSK_REFERENCE_COMPENSATION_ENABLED_TRUE));
	RFmxCheckWarn(RFmxDemod_DDemodCfgSymbolRate(instrumentHandle, "", symbolRate));
	RFmxCheckWarn(RFmxDemod_DDemodCfgNumberOfSymbols(instrumentHandle, "", numOfSymbols));	
	RFmxCheckWarn(RFmxDemod_DDemodCfgPulseShapingFilter(instrumentHandle, "", pulseShapingFilterType,
		                                                pulseShapingFilterParameter, 0.0, 1.0, NULL, 0));
	RFmxCheckWarn(RFmxDemod_DDemodCfgMeasurementFilter(instrumentHandle, "", RFMXDEMOD_VAL_DDEMOD_MEASUREMENT_FILTER_TYPE_AUTO,
		                                               0.0, 1.0, NULL, 0));
	RFmxCheckWarn(RFmxDemod_DDemodCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
	RFmxCheckWarn(RFmxDemod_Initiate(instrumentHandle, "", ""));
	
	/* Retrieve results */
	RFmxCheckWarn(RFmxDemod_DDemodFetchFSKResults(instrumentHandle, "", timeout, &meanFSKDeviation, 
											  &meanRMSFSKError, &maximumPeakFSKError));
	
	/* Display results */
	printf("Mean FSK Deviation(Hz)               : %f\n", meanFSKDeviation);
	printf("Mean RMS FSK Error(Hz)               : %f\n", meanRMSFSKError);
	printf("Maximum Peak FSK Error(%%)            : %f\n", maximumPeakFSKError);
	
Error:
	if( error ) 
	{
		RFmxDemod_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if(error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}
	if(instrumentHandle)
	{
		RFmxDemod_Close(instrumentHandle, RFMXDEMOD_VAL_FALSE);
	}
	printf("Press any key to exit\n");
	_getch();
	return error;
}
