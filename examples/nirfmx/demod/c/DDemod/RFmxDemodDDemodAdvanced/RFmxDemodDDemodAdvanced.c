//Steps:
//1. Open a new RFmxInstrMX session and create a Demod Signal
//2. Configure the basic instrument properties (Clock Source, Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time)
//6. Select DDemod Measurement and enable  the traces 
//7. Configure Modulation Type
//8. Configure DDemod Symbol Rate, Sample Per Symbol, Number of Symbols
//9. Configure DDemod PSK Format and EVM Norm Reference
//10. Configure DDemod FSK Deviation
//11. Configure DDemod Pulse Shaping Filter Pulse Shaping Filter Custom Coefficients will be applied if Pulse Shaping Filter Type is Custom
//12. Configure DDemod Measurement Filter Measurement Filter Custom Coefficients will be applied if Measurement Filter Type is Custom
//13. Configure DDemod Equalizer
//14. Configure DDemod Bit Synchronization
//15. Configure DDemod Averaging
//16. Initiate Measurement
//17. Fetch DDemod Measurements and Traces
//18. Dispose Demod Signal and Close the RFmxInstrMX Session

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
	
	/* Trigger */
	int32 IQPowerEdgeEnabled = RFMXDEMOD_VAL_FALSE;
	float64 IQPowerEdgeLevel = -20.00;		/* dBm */
	float64 triggerDelay = 0.00;			/* seconds */
	float64 minQuietTime = 0.00;			/* seconds */
	
	/* Modulation Type */
	int32 modulationType = RFMXDEMOD_VAL_DDEMOD_MODULATION_TYPE_PSK;
	/* Differential Enabled */
	int32 differentialEnabled = RFMXDEMOD_VAL_DDEMOD_DIFFERENTIAL_ENABLED_FALSE;
	/* FSK Deviation*/
	float64 FSKDeviation = 15.000e+3;		/* Hz */
	/* Signal Structure*/
	int32 signalStructure = RFMXDEMOD_VAL_DDEMOD_SIGNAL_STRUCTURE_CONTINUOUS;
	/* Burst Start Exclusion Symbols */
	int32 burstStartExclusionSymbols = 0;
	/* Burst End Exclusion Symbols */
	int32 burstEndExclusionSymbols = 0;
	/* Symbol Rate */
	float64 symbolRate = 100.000e+3;		/* Hz */
	/* Number of Symbols */
	int32 numOfSymbols = 1000;
	/* M */
	int32 M = RFMXDEMOD_VAL_DDEMOD_M_4;
	/* PSK Format */
	int32 PSKFormat = RFMXDEMOD_VAL_DDEMOD_PSK_FORMAT_NORMAL;
	/* Samples per Symbol */
	int32 samplesPerSymbol = RFMXDEMOD_VAL_DDEMOD_SAMPLES_PER_SYMBOL_AUTO;
	/* EVM Norm Reference */
	int32 EVMNormReference = RFMXDEMOD_VAL_DDEMOD_EVM_NORMALIZATION_REFERENCE_PEAK;
	/* FSK Ref Comp Enabled */
	int32 FSKRefCompEnabled = RFMXDEMOD_VAL_DDEMOD_FSK_REFERENCE_COMPENSATION_ENABLED_FALSE;
	/* APSK Ring Ratio */
	float64 APSKR2ToR1Ratio = 2.84;
    float64 APSKR3ToR1Ratio = 5.27;

	/* Synchronization */
	int32 syncEnabled = RFMXDEMOD_VAL_DDEMOD_SYNCHRONIZATION_ENABLED_FALSE;
	int32 measurementOffset = 0;	
	
	/* Averaging */
	int32 averagingEnabled = RFMXDEMOD_VAL_DDEMOD_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;

	/* Pulse Shaping Filter */
	int32 pulseShappingFilterType = RFMXDEMOD_VAL_DDEMOD_PULSE_SHAPING_FILTER_TYPE_ROOT_RAISED_COSINE;
	float64 pulseShappingFilterParameter = 0.50;
	float64 pulseShappingFilterCustomCoefficientx0 = 0;
	float64 pulseShappingFilterCustomCoefficientdx = 1.00e+0;
	
	/* Measurement Filter */
	int32 measurementFilterType = RFMXDEMOD_VAL_DDEMOD_MEASUREMENT_FILTER_TYPE_AUTO;	
	float64 measurementFilterCustomCoefficientx0 = 0.00e+0;
	float64 measurementFilterCustomCoefficientdx = 1.00e+0;
	
	/* Equalizer */
	int32 equalizerMode = RFMXDEMOD_VAL_DDEMOD_EQUALIZER_MODE_OFF;
	int32 equalizerLength = 20;
	int32 equalizerTrainingCount = 10;
	float64 equalizerConvergenceFactor = 0.01e+0;
	float64 equalizerInitialCoefficientx0 = 0.00e+0;
	float64 equalizerInitialCoefficientdx = 1.00e+0;
	
	/* Variables to store measurement results */
	float64 x0=0.0, dx=0.0;
	NIComplexSingle *constellationTrace =(NIComplexSingle *)NULL;
	float32 *EVMTrace =(float32 *)NULL;
	float32 *offsetEVMTrace =(float32 *)NULL;
	float32 *I =(float32 *)NULL;
	float32 *Q =(float32 *)NULL;
	int32 actualArraySize = 0;
	int32 i=0;
	
	int32 syncFound = 0;
	float64 meanCarrierFrequencyError = 0;
	float64 meanFrequencyDrift = 0;
	float64 meanCarrierPhaseError = 0;
	
	float64 meanRMSEVM = 0;
	float64 meanPeakEVM = 0;
	float64 meanRMSOffsetEVM = 0;
	float64 meanPeakOffsetEVM = 0;
	float64 meanMER = 0;
	float64 maxRMSEVM = 0;
	float64 maxPeakEVM = 0;
	float64 maxRMSOffsetEVM = 0;
	float64 maxPeakOffsetEVM = 0;
	
	float64 meanFSKDeviation = 0;
	float64 meanRMSFSKError = 0;
	float64 maximumPeakFSKError = 0;

	float64 meanMagnitudeError = 0;
	float64 maxMagnitudeError = 0;
	float64 meanPhaseError = 0;
	float64 maxPhaseError = 0;
	float64 meanIQOriginOffset = 0;
	float64 meanIQGainImbalance = 0;
	float64 meanQuadratureSkew = 0; 
	float64 meanRhoFactor = 0;
	float64 meanAmplitudeDroop = 0;
	
	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxDemod_Initialize(resourceName,  "",  &instrumentHandle, NULL));
	
	/* Configure DDemod measurement parameters */
	RFmxCheckWarn(RFmxDemod_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
	RFmxCheckWarn(RFmxDemod_SetSelectedPorts(instrumentHandle, "", selectedPorts));
	RFmxCheckWarn(RFmxDemod_CfgFrequency(instrumentHandle, "", centerFrequency));
	RFmxCheckWarn(RFmxDemod_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
	RFmxCheckWarn(RFmxDemod_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
	RFmxCheckWarn(RFmxDemod_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeLevel, 
											  RFMXDEMOD_VAL_IQ_POWER_EDGE_RISING_SLOPE, triggerDelay, 
											  RFMXDEMOD_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_MANUAL, minQuietTime, 
											  IQPowerEdgeEnabled));
	RFmxCheckWarn(RFmxDemod_SelectMeasurements(instrumentHandle, "", RFMXDEMOD_VAL_DDEMOD, RFMXDEMOD_VAL_TRUE));	
	RFmxCheckWarn(RFmxDemod_DDemodCfgModulationType(instrumentHandle, "", modulationType, M, differentialEnabled));	
	RFmxCheckWarn(RFmxDemod_DDemodCfgSymbolRate(instrumentHandle, "", symbolRate));
	RFmxCheckWarn(RFmxDemod_DDemodCfgSamplesPerSymbol(instrumentHandle, "", samplesPerSymbol));	
	RFmxCheckWarn(RFmxDemod_DDemodCfgNumberOfSymbols(instrumentHandle, "", numOfSymbols));	
	RFmxCheckWarn(RFmxDemod_DDemodCfgPSKFormat(instrumentHandle, "", PSKFormat));
	RFmxCheckWarn(RFmxDemod_DDemodCfgEVMNormalizationReference(instrumentHandle, "", EVMNormReference));	
	RFmxCheckWarn(RFmxDemod_DDemodCfgFSKDeviation(instrumentHandle, "", FSKDeviation, FSKRefCompEnabled));
    RFmxCheckWarn(RFmxDemod_DDemodSetAPSKR2ToR1Ratio(instrumentHandle, "", APSKR2ToR1Ratio));
    RFmxCheckWarn(RFmxDemod_DDemodSetAPSKR3ToR1Ratio(instrumentHandle, "", APSKR3ToR1Ratio));
	RFmxCheckWarn(RFmxDemod_DDemodCfgPulseShapingFilter(instrumentHandle, "", pulseShappingFilterType, 
													pulseShappingFilterParameter, 
													pulseShappingFilterCustomCoefficientx0, 
													pulseShappingFilterCustomCoefficientdx, 
													NULL, 0));		
	RFmxCheckWarn(RFmxDemod_DDemodCfgMeasurementFilter(instrumentHandle, "", measurementFilterType, 
												   measurementFilterCustomCoefficientx0, 
												   measurementFilterCustomCoefficientdx, 
												   NULL, 0));
	RFmxCheckWarn(RFmxDemod_DDemodCfgEqualizer(instrumentHandle, "", equalizerMode, 
										   equalizerLength, equalizerInitialCoefficientx0, 
										   equalizerInitialCoefficientdx, NULL, 
										   equalizerTrainingCount, equalizerConvergenceFactor, 0));		
	RFmxCheckWarn(RFmxDemod_DDemodCfgSynchronization(instrumentHandle, "", syncEnabled, NULL, 
												 measurementOffset, 0));	
	RFmxCheckWarn(RFmxDemod_DDemodCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
	RFmxCheckWarn(RFmxDemod_DDemodCfgSignalStructure(instrumentHandle, "", signalStructure));
	RFmxCheckWarn(RFmxDemod_DDemodSetBurstStartExclusionSymbols(instrumentHandle, "", burstStartExclusionSymbols));
    RFmxCheckWarn(RFmxDemod_DDemodSetBurstEndExclusionSymbols(instrumentHandle, "", burstEndExclusionSymbols));
    RFmxCheckWarn(RFmxDemod_Initiate(instrumentHandle, "", ""));
	
	/* Retrieve Results */
	RFmxCheckWarn(RFmxDemod_DDemodFetchCarrierMeasurement(instrumentHandle, "", timeout, &meanCarrierFrequencyError, 
													  &meanFrequencyDrift, &meanCarrierPhaseError));
	RFmxCheckWarn(RFmxDemod_DDemodFetchEVM(instrumentHandle, "", timeout, &meanRMSEVM, &maxRMSEVM, 
									   &meanMER, &maxPeakEVM, &meanPeakEVM));
	RFmxCheckWarn(RFmxDemod_DDemodFetchOffsetEVM(instrumentHandle, "", timeout, &meanRMSOffsetEVM, 
											 &maxRMSOffsetEVM, &maxPeakOffsetEVM, &meanPeakOffsetEVM));
	RFmxCheckWarn(RFmxDemod_DDemodFetchMagnitudeError(instrumentHandle, "", timeout, &meanMagnitudeError, 
												  &maxMagnitudeError));
	RFmxCheckWarn(RFmxDemod_DDemodFetchPhaseError(instrumentHandle, "", timeout, &meanPhaseError, 
											  &maxPhaseError));	
	RFmxCheckWarn(RFmxDemod_DDemodFetchFSKResults(instrumentHandle, "", timeout, &meanFSKDeviation, 
											  &meanRMSFSKError, &maximumPeakFSKError));
	RFmxCheckWarn(RFmxDemod_DDemodFetchIQImpairments(instrumentHandle, "", timeout, &meanIQGainImbalance, 
												 &meanQuadratureSkew, &meanIQOriginOffset));
	RFmxCheckWarn(RFmxDemod_DDemodFetchSyncFound(instrumentHandle, "", timeout, &syncFound));
	RFmxCheckWarn(RFmxDemod_DDemodFetchMeanRhoFactor(instrumentHandle, "", timeout, &meanRhoFactor));
	RFmxCheckWarn(RFmxDemod_DDemodFetchMeanAmplitudeDroop(instrumentHandle, "", timeout, &meanAmplitudeDroop));	
	
	RFmxCheckWarn(RFmxDemod_DDemodFetchConstellationTrace(instrumentHandle, "", timeout,NULL,0,&actualArraySize));

	if(actualArraySize > 0)
	{
		constellationTrace = (NIComplexSingle *)malloc(sizeof(NIComplexSingle)*actualArraySize);
		if(constellationTrace)
		{
			RFmxCheckWarn(RFmxDemod_DDemodFetchConstellationTrace(instrumentHandle,"",timeout,constellationTrace,actualArraySize,NULL));
			I=(float32 *)malloc(sizeof(float32)*actualArraySize);
			Q=(float32 *)malloc(sizeof(float32)*actualArraySize);
			if(I && Q)
			{
				for(i=0;i<actualArraySize;i++)
				{
					I[i]=(constellationTrace+i)->real;
					Q[i]=(constellationTrace+i)->imaginary;
				}
			}
			else
			{
				printf("malloc failed.\n");
				goto Error;
			}
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}
		
	actualArraySize = 0; x0 = 0; dx = 0;
	RFmxCheckWarn(RFmxDemod_DDemodFetchEVMTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, 
											&actualArraySize));
	if(actualArraySize > 0)
	{
	
		EVMTrace =(float32 *)malloc(sizeof(float32)*actualArraySize);
		if(EVMTrace)
		{
			RFmxCheckWarn(RFmxDemod_DDemodFetchEVMTrace(instrumentHandle, "", timeout, &x0, &dx, EVMTrace, 
													actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}
	
	actualArraySize = 0; x0 = 0; dx = 0;
	RFmxCheckWarn(RFmxDemod_DDemodFetchOffsetEVMTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, 
												  &actualArraySize));
	if(actualArraySize > 0)
	{
		offsetEVMTrace =(float32 *)malloc(sizeof(float32)*actualArraySize);
		if(offsetEVMTrace)
		{
			RFmxCheckWarn(RFmxDemod_DDemodFetchOffsetEVMTrace(instrumentHandle, "", timeout, &x0, &dx, offsetEVMTrace, 
														  actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}  

	/* Display Results */
	printf("-------------------Carrier measurements----------\n");
	printf("Mean Carrier Frequency Error(Hz)       : %f\n", meanCarrierFrequencyError);
	printf("Mean Frequency Drift(Hz)               : %f\n", meanFrequencyDrift);
	printf("Mean Carreir Phase Error(deg)          : %f\n", meanCarrierPhaseError);

	printf("\n---------------------------EVM-----------------\n");
	printf("Mean MER(dB)                           : %f\n", meanMER);
	printf("Mean RMS EVM(%%)                        : %f\n", meanRMSEVM);
	printf("Max RMS EVM(%%)                         : %f\n", maxRMSEVM);
	printf("Mean Peak EVM(%%)                       : %f\n", meanPeakEVM);
	printf("Max Peak EVM(%%)                        : %f\n",maxPeakEVM);
	printf("Mean RMS Offset EVM(%%)                 : %f\n", meanRMSOffsetEVM);
	printf("Max RMS Offset EVM(%%)                  : %f\n",maxRMSOffsetEVM);
	printf("Mean Peak Offset EVM(%%)                : %f\n", meanPeakOffsetEVM);
	printf("Max Peak Offset EVM(%%)                 : %f\n",maxPeakOffsetEVM);
	
	printf("\n--------------------------FSK Results--------------\n");
	printf("Mean Deviation(Hz)                     : %f\n", meanFSKDeviation);
	printf("Mean RMS Error(Hz)                     : %f\n", meanRMSFSKError);
	printf("Maximum Peak Error(%%)                  : %f\n", maximumPeakFSKError);	

	printf("\n--------------------------Measurements------------\n");	
	if(syncFound)
		printf("Sync Found\n");
	else
		printf("Sync not Found\n");
	
	printf("Mean Magnitude Error(%%)                : %f\n", meanMagnitudeError);
	printf("Maximum Magnitude Error(%%)             : %f\n", maxMagnitudeError);
	printf("Mean Phase Error(deg)                  : %f\n", meanPhaseError);
	printf("Maximum Phase Error(deg)               : %f\n", maxPhaseError);
	printf("Mean IQ Origin Offset(dB)              : %f\n", meanIQOriginOffset);
	printf("Mean IQ Gain Imbalance(dB)             : %f\n", meanIQGainImbalance);
	printf("Mean Quadrature Skew(deg)              : %f\n", meanQuadratureSkew);
	printf("Mean Rho Factor                        : %f\n", meanRhoFactor);
	printf("Mean Amplitude Droop(dB/Symbol)        : %f\n", meanAmplitudeDroop);	
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
	/* Free allocated memory */
	if(I)
       free(I);
    if(Q)
	   free(Q);
	if(constellationTrace)
		free(constellationTrace);
	if(EVMTrace)
		free(EVMTrace);
	if(offsetEVMTrace)
		free(offsetEVMTrace);
	printf("Press any key to exit\n");
	_getch();
	return error;
}
