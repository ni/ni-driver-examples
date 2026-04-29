//Steps:
//1. Open a new RFmxInstrMX session and create a Demod Signal
//2. Configure the basic instrument properties (Clock Source, Clock Frequency)
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select ADemod Measurement and enable  the traces 
//6. Configure FM Modulation
//7. Configure ADemod RBW Filter, Measurement Interval, and Carrier Correction
//8. Configure ADemod FM DeEmphasis, Audio Filter and Averaging
//9. Initiate Measurement
//10. Fetch ADemod Measurements and Traces
//11. Dispose Demod Signal and Close the RFmxInstrMX Session

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

	/* Measurement Interval */
	float64 measurementInterval = 10.00e-3;		/* seconds */

	/* FM Deemphasis */
	float64 deEmphasis = 0.0;					/* seconds */

	/* RBW Filter */
	int32 RBWFilterType = RFMXDEMOD_VAL_ADEMOD_RBW_FILTER_TYPE_FLAT;
	float64 RBW = 100.00e+3;				/* Hz */
	float64 RBWRRCAlpha = 0.100;

	/* Audio Measurement */
	int32 audioMeasurementEnabled = RFMXDEMOD_VAL_ADEMOD_AUDIO_MEASUREMENT_ENABLED_TRUE;

	/* Carrier Corrections */
	int32 carrierFreqCorrectionEnabled = RFMXDEMOD_VAL_ADEMOD_CARRIER_FREQUENCY_CORRECTION_ENABLED_TRUE;
	int32 CarrierPhaseCorrectionEnabled = RFMXDEMOD_VAL_ADEMOD_CARRIER_PHASE_CORRECTION_ENABLED_TRUE;

	/* Audio Filter */
	int32 audioFilterType = RFMXDEMOD_VAL_ADEMOD_AUDIO_FILTER_TYPE_NONE;
	float64 audioFilterLowerCutoff = 100.000;		/* Hz */
	float64 audioFilterUpperCutoff = 10.000e+3;		/* Hz */

	/* Averaging */
	int32 averagingEnabled = RFMXDEMOD_VAL_ADEMOD_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;
	int32 averagingType = RFMXDEMOD_VAL_ADEMOD_AVERAGING_TYPE_LINEAR;

	/* Variables to store the results */
	float64 x0=0.0, dx=0.0;
	float32 *spectrumTrace =(float32 *)NULL;
	float32 *signalTrace =(float32 *)NULL;
	int32 actualArraySize = 0;	

	/* Distortions */
	float64 averageSINAD = 0;			/* dB */
	float64 averageTHDwithNoise = 0;	/* %age */
	float64 averageSNR = 0;				/* dB */
	float64 averageTHD = 0;				/* %age */

	/* Mean Modulation Frequency */
	float64 meanModulationFrequency = 0;	/* Hz */

	/* Carrier measurement */
	float64 meanCarrierFrequencyError = 0;		/* Hz */
	float64 meanCarrierPower = 0;		/* dBm */

	/* FM Mean deviation */
	float64 meanDeviation = 0;		/* Hz */
	float64 meanPeaktoPeak = 0;		/* Hz */
	float64 meanPositivePeak = 0;	/* Hz */
	float64 meanNegativePeak = 0;	/* Hz */
	float64 meanRMS = 0;

	/* FM Max deviation */
	float64 maxDeviation = 0;		/* Hz */
	float64 maxPeaktoPeak = 0;		/* Hz */
	float64 maxPositivePeak = 0;	/* Hz */
	float64 maxNegativePeak = 0;	/* Hz */
	float64 maxRMS = 0;				/* Hz */

	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxDemod_Initialize(resourceName,  "",  &instrumentHandle, NULL));

	/* Configure ADemod parameters */
	RFmxCheckWarn(RFmxDemod_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
	RFmxCheckWarn(RFmxDemod_SetSelectedPorts(instrumentHandle, "", selectedPorts));
	RFmxCheckWarn(RFmxDemod_CfgFrequency(instrumentHandle, "", centerFrequency));
	RFmxCheckWarn(RFmxDemod_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
	RFmxCheckWarn(RFmxDemod_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));

	RFmxCheckWarn(RFmxDemod_SelectMeasurements(instrumentHandle, "", RFMXDEMOD_VAL_ADEMOD, RFMXDEMOD_VAL_TRUE));
	RFmxCheckWarn(RFmxDemod_ADemodSetAudioMeasurementEnabled(instrumentHandle, "", audioMeasurementEnabled));
	RFmxCheckWarn(RFmxDemod_ADemodCfgModulationType(instrumentHandle, "", RFMXDEMOD_VAL_ADEMOD_MODULATION_TYPE_FM));
	RFmxCheckWarn(RFmxDemod_ADemodCfgRBWFilter(instrumentHandle, "", RBW, RBWFilterType, RBWRRCAlpha));	
	RFmxCheckWarn(RFmxDemod_ADemodCfgMeasurementInterval(instrumentHandle, "", measurementInterval));
	RFmxCheckWarn(RFmxDemod_ADemodCfgCarrierCorrection(instrumentHandle, "", carrierFreqCorrectionEnabled, 
		CarrierPhaseCorrectionEnabled));	
	RFmxCheckWarn(RFmxDemod_ADemodCfgFMDeEmphasis(instrumentHandle, "", deEmphasis));
	RFmxCheckWarn(RFmxDemod_ADemodCfgAudioFilter(instrumentHandle, "", audioFilterType, audioFilterLowerCutoff, audioFilterUpperCutoff));    
	RFmxCheckWarn(RFmxDemod_ADemodCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, 
		averagingType));		
	RFmxCheckWarn(RFmxDemod_Initiate(instrumentHandle, "", ""));

	/* Retrieve results */
	RFmxCheckWarn(RFmxDemod_ADemodFetchDistortions(instrumentHandle, "", timeout, &averageSINAD, &averageSNR, 
		&averageTHD, &averageTHDwithNoise));
	RFmxCheckWarn(RFmxDemod_ADemodFetchMeanModulationFrequency(instrumentHandle, "", timeout, &meanModulationFrequency));
	RFmxCheckWarn(RFmxDemod_ADemodFetchCarrierMeasurement(instrumentHandle, "", timeout, 
		&meanCarrierFrequencyError, &meanCarrierPower));
	RFmxCheckWarn(RFmxDemod_ADemodFetchFMMeanDeviation(instrumentHandle, "", timeout, 
		&meanDeviation, &meanPeaktoPeak, &meanRMS, 
		&meanPositivePeak, &meanNegativePeak));
	RFmxCheckWarn(RFmxDemod_ADemodFetchFMMaximumDeviation(instrumentHandle, "", timeout, &maxDeviation, 
		&maxPeaktoPeak, &maxRMS, &maxPositivePeak, 
		&maxNegativePeak));
	/* Retrieve waveforms */
	RFmxCheckWarn(RFmxDemod_ADemodFetchDemodSpectrumTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if(actualArraySize > 0)
	{
		spectrumTrace =(float32 *)malloc(sizeof(float32)*actualArraySize);
		if(spectrumTrace)
		{
			RFmxCheckWarn(RFmxDemod_ADemodFetchDemodSpectrumTrace(instrumentHandle, "", timeout, &x0, &dx, spectrumTrace, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	actualArraySize=0, x0=0.0;dx=0.0;
	RFmxCheckWarn(RFmxDemod_ADemodFetchDemodSignalTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if(actualArraySize > 0)
	{
		signalTrace =(float32 *)malloc(sizeof(float32)*actualArraySize);
		if(signalTrace)
		{
			RFmxCheckWarn(RFmxDemod_ADemodFetchDemodSignalTrace(instrumentHandle, "", timeout, &x0, &dx, signalTrace, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	/* Display results */
	printf("Mean Carrier Power(dBm)             %f\n", meanCarrierPower);
	printf("Average SINAD(dB)                   %f\n", averageSINAD);
	printf("Average THD with Noise(%%)           %f\n", averageTHDwithNoise);
	printf("Mean Carrier Frequency Error(Hz)    %f\n", meanCarrierFrequencyError);
	printf("Average SNR(dB)                     %f\n", averageSNR);
	printf("Average THD(%%)                      %f\n", averageTHD);
	printf("Mean Modulation Frequency(Hz)       %f\n", meanModulationFrequency);	


	printf("\n-------------------FM Deviations---------------------\n");
	printf("Mean Deviation(Hz)                  %f\n", meanDeviation);
	printf("Max Deviation(Hz)                   %f\n",maxDeviation);
	printf("Mean Peak to Peak/2(Hz)             %f\n", meanPeaktoPeak);
	printf("Max Peak to Peak/2(Hz)              %f\n",maxPeaktoPeak);
	printf("Mean Positive Peak(Hz)              %f\n", meanPositivePeak);
	printf("Max Positive Peak(Hz)               %f\n", maxPositivePeak);
	printf("Mean Negative peak(Hz)              %f\n", meanNegativePeak);
	printf("Max Negative peak(Hz)               %f\n", maxNegativePeak);
	printf("Mean RMS(Hz)                        %f\n", meanRMS);
	printf("Max RMS(Hz)                         %f\n",maxRMS);

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
	if(spectrumTrace)
		free(spectrumTrace);
	if(signalTrace)
		free(signalTrace);
	printf("Press any key to exit\n");
	_getch();
	return error;
}
