//Steps:
//1. Open a new RFmxInstrMX session and create a Demod Signal
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Select QAM Modulation and M
//5. Configure DDemod Symbol Rate, Number of Symbols and Pulse Shaping Filter
//6. Configure DDemod Measurement Filter Type as Auto
//7. Configure DDemod Averaging
//8. Read DDemod Measurement Results
//9. Dispose Demod Signal and Close the RFmxInstrMX Session

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
	char *selectedPorts = "";
	float64 centerFrequency = 1e+9;			/* Hz */
	float64 referenceLevel = 0.00;			/* dBm */
	float64 externalAttenuation = 0.00;		/* dB */
	
	float64 timeout = 10.0;					/* seconds */   
	
	int32 M = RFMXDEMOD_VAL_DDEMOD_M_16;
	float64 symbolRate = 100.000e+3;
	int32 numOfSymbols = 1000;
	
	/* Pulse shaping filter */
	int32 pulseShapingFilterType = RFMXDEMOD_VAL_DDEMOD_PULSE_SHAPING_FILTER_TYPE_ROOT_RAISED_COSINE;
	float64 pulseShapingFilterParameter = 0.50;
	
	/* Averaging */
	int32 averagingEnabled = RFMXDEMOD_VAL_DDEMOD_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;
	
	/* Variables to store the measurement results */
	float64 meanFrequencyOffset = 0;
	float64 meanRMSEVM = 0;
	float64 maximumPeakEVM = 0;
	float64 meanModulationErrorRatio = 0;
	
	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxDemod_Initialize(resourceName,  "",  &instrumentHandle, NULL));
	
	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxDemod_SetSelectedPorts(instrumentHandle, "", selectedPorts));
	RFmxCheckWarn(RFmxDemod_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxDemod_DDemodCfgModulationType(instrumentHandle, "", RFMXDEMOD_VAL_DDEMOD_MODULATION_TYPE_QAM, 
															M, RFMXDEMOD_VAL_DDEMOD_DIFFERENTIAL_ENABLED_FALSE));
	RFmxCheckWarn(RFmxDemod_DDemodCfgSymbolRate(instrumentHandle, "", symbolRate));
	RFmxCheckWarn(RFmxDemod_DDemodCfgNumberOfSymbols(instrumentHandle, "", numOfSymbols));	
	RFmxCheckWarn(RFmxDemod_DDemodCfgPulseShapingFilter(instrumentHandle, "", pulseShapingFilterType,
		                                                pulseShapingFilterParameter, 0.0, 1.0, NULL, 0));
	RFmxCheckWarn(RFmxDemod_DDemodCfgMeasurementFilter(instrumentHandle, "", RFMXDEMOD_VAL_DDEMOD_MEASUREMENT_FILTER_TYPE_AUTO,
		                                               0.0, 1.0, NULL, 0));
	RFmxCheckWarn(RFmxDemod_DDemodCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));

	/* Retrieve results */
	RFmxCheckWarn(RFmxDemod_DDemodRead(instrumentHandle, "", timeout, &meanFrequencyOffset, &meanRMSEVM, 
															&maximumPeakEVM, &meanModulationErrorRatio));

	/* Display results */
	printf("Mean Frequency Offset(Hz)        : %f\n", meanFrequencyOffset);
	printf("Mean RMS EVM(%%)                  : %f\n", meanRMSEVM);
	printf("Maximum Peak EVM(%%)              : %f\n", maximumPeakEVM);
	printf("Mean Modulation Error Ratio(dB)  : %f\n", meanModulationErrorRatio);

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
