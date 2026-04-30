//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure LO Leakage Avoidance Enabled, LO source and Downconverter Frequency Offset(Hz).
//4. Configure basic RF signal properties(Center Frequency, RF Attenuation and External Attenuation)
//5. Enabling Pulse and Traces Result.
//6. Configure Trigger Type and Trigger Parameters.
//7. Configure Acquisition Settings.
//8. Configure Pulse Detection Settings
//9. Configure State and Threshold Level Settings.
//10. Configure Measurement Point Settings.
//11. Configure Frequency & Phase Settings.
//12. Configure Modulation Settings.
//13. Enabling Pulse Stability enabled as False and Pulse Metrics enabled settings as True.
//14. Initiate the Measurement.
//15. Wait for Measurement to complete.
//16. Results Phase, Frequency Measurements and FM Chirp results as well as their Statistical results,Phase(Wrapped) Trace, Frequency Trace. 
//17. Close RFmx Session.


#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxPulse.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096


int main(int argc, char *argv[])
{
	char* resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;

	char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
	int32 error = 0, lastErrorCode = 0;
	int i = 0;

	float64 centerFrequency = 1.0e9;                                              /* (Hz) */
	float64 referenceLevel = -10.0;                                               /* (dBm) */
	float64 externalAttenuation = 0.0;                                            /* (dB) */

	float64 downconverterFrequencyOffset = 0.0;                                   /* (Hz) */

	int32 LOLeakageAvoidanceEnabled = RFMXINSTR_VAL_LO_LEAKAGE_AVOIDANCE_ENABLED_TRUE;
	char* LOSource = RFMXINSTR_VAL_LO_SOURCE_ONBOARD;

	char* frequencyReferenceSource = RFMXPULSE_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10.0e6;                                 /* (Hz) */

	int32 IQPowerEdgeEnabled = RFMXPULSE_VAL_TRUE;
	float64 IQPowerEdgeLevel = -20.0;                                             /* (dBm) */
	float64 triggerDelay = 0.0;                                                   /* (s) */
	int32 minimumQuietTimeMode = RFMXPULSE_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
	float64 minimumQuietTime = 5.0e-6;                                            /* (s) */

	float64 measurementBandwidth = 80.0e6;                                        /* (Hz) */
	int32 measurementFilterType = RFMXPULSE_VAL_MEASUREMENT_FILTER_TYPE_GAUSSIAN;
	float64 acquisitionLength = 1.0e-3;                                           /* (s) */
	int32 maximumPulseCountEnabled = RFMXPULSE_VAL_MAXIMUM_PULSE_COUNT_ENABLED_FALSE;
	int32 maximumPulseCount = 100;

	int32 pulseLevelComputationMethod = RFMXPULSE_VAL_PULSE_LEVEL_COMPUTATION_METHOD_MEDIAN;
	float64 pulseUpperThresholdLevel = 90.0;                                      /* (%) */
	float64 pulseWidthThresholdLevel = 50.0;                                      /* (%) */
	float64 pulseLowerThresholdLevel = 10.0;                                      /* (%) */

	int32 pulseMeasurementPointReference = RFMXPULSE_VAL_PULSE_MEASUREMENT_POINT_REFERENCE_CENTER;
	float64 pulseMeasurementPointOffset = 0.0;                                    /* (s) */
	float64 pulseMeasurementPointAveragingDuration = 0.0;                         /* (s) */

	int32 pulseDetectionReference = RFMXPULSE_VAL_PULSE_DETECTION_REFERENCE_REFERENCE_LEVEL;
	float64 pulseDetectionThreshold = -20.0;
	float64 pulseDetectionHysteresis = 1.0;                                       /* (dB) */
	float64 pulseDetectionMinimumOffDuration = 50.0e-9;                                    /* (s) */
	
	int32 pulseFrequencyPhaseDeviationRangeReference = 
		RFMXPULSE_VAL_PULSE_FREQUENCY_AND_PHASE_DEVIATION_RANGE_REFERENCE_CENTER;
	float64 pulseFrequencyPhaseDeviationRangeLength = 75.0;                       /* (%) */
	float64 pulseFrequencyPhaseDeviationRangeEdgeStart = 0.0;                     /* (s) */
	float64 pulseFrequencyPhaseDeviationRangeEdgeStop = 0.0;                      /* (s) */

	int32 pulseModulationType = RFMXPULSE_VAL_PULSE_MODULATION_TYPE_CW;
	int32 pulseCWFrequencyOffsetAuto = RFMXPULSE_VAL_PULSE_CW_FREQUENCY_OFFSET_AUTO_TRUE;
	float64 pulseCWFrequencyOffset = 0.0;                                         /* (Hz) */

	float64* pulseResultsAveragePhase = NULL;                                     /* (deg) */
	float64* pulseResultsPhaseDeviation = NULL;                                   /* (deg) */
	float64* pulseResultsPhaseErrorRMS = NULL;                                    /* (deg) */
		
	float64* pulseResultsAverageFrequency = NULL;                                 /* (Hz) */
	float64* pulseResultsFrequencyDeviation = NULL;                               /* (Hz) */
	float64* pulseResultsFrequencyErrorRMS = NULL;                                /* (Hz) */

	float64* pulseResultsFMChirpRate = NULL;                                      /* (Hz/us)*/
	float64* pulseResultsFMChirpRate2 = NULL;                                     /* (Hz/us)*/
	    
	float64 pulseResultsAveragePhaseMean = 0.0;                                   /* (deg) */
	float64 pulseResultsAveragePhaseMaximum = 0.0;                                /* (deg) */
	float64 pulseResultsAveragePhaseMinimum = 0.0;                                /* (deg) */
	float64 pulseResultsAveragePhaseSD = 0.0;                                     /* (deg) */
	float64 pulseResultsPhaseDeviationMean = 0.0;                                 /* (deg) */
	float64 pulseResultsPhaseDeviationMaximum = 0.0;                              /* (deg) */
	float64 pulseResultsPhaseDeviationMinimum = 0.0;                              /* (deg) */
	float64 pulseResultsPhaseDeviationSD = 0.0;                                   /* (deg) */
	float64 pulseResultsPhaseErrorRMSMean = 0.0;                                  /* (deg) */
	float64 pulseResultsPhaseErrorRMSMaximum = 0.0;                               /* (deg) */
	float64 pulseResultsPhaseErrorRMSMinimum = 0.0;                               /* (deg) */
	float64 pulseResultsPhaseErrorRMSSD = 0.0;                                    /* (deg) */

	float64 pulseResultsAverageFrequencyMean = 0.0;                               /* (Hz) */
	float64 pulseResultsAverageFrequencyMaximum = 0.0;                            /* (Hz) */
	float64 pulseResultsAverageFrequencyMinimum = 0.0;                            /* (Hz) */
	float64 pulseResultsAverageFrequencySD = 0.0;                                 /* (Hz) */
	float64 pulseResultsFrequencyDeviationMean = 0.0;                             /* (Hz) */
	float64 pulseResultsFrequencyDeviationMaximum = 0.0;                          /* (Hz) */
	float64 pulseResultsFrequencyDeviationMinimum = 0.0;                          /* (Hz) */
	float64 pulseResultsFrequencyDeviationSD = 0.0;                               /* (Hz) */
	float64 pulseResultsFrequencyErrorRMSMean = 0.0;                              /* (Hz) */
	float64 pulseResultsFrequencyErrorRMSMaximum = 0.0;                           /* (Hz) */
	float64 pulseResultsFrequencyErrorRMSMinimum = 0.0;                           /* (Hz) */
	float64 pulseResultsFrequencyErrorRMSSD = 0.0;                                /* (Hz) */

	float64 pulseResultsFMChirpRateMean = 0.0;                                    /* (Hz/us) */
	float64 pulseResultsFMChirpRateMaximum = 0.0;                                 /* (Hz/us) */
	float64 pulseResultsFMChirpRateMinimum = 0.0;                                 /* (Hz/us) */
	float64 pulseResultsFMChirpRateSD = 0.0;                                      /* (Hz/us) */
	float64 pulseResultsFMChirpRate2Mean = 0.0;                                   /* (Hz/us) */
	float64 pulseResultsFMChirpRate2Maximum = 0.0;                                /* (Hz/us) */
	float64 pulseResultsFMChirpRate2Minimum = 0.0;                                /* (Hz/us) */
	float64 pulseResultsFMChirpRate2SD = 0.0;                                     /* (Hz/us) */

	float64 timeout = 10.0;                                                       /* (s) */

	int32 actualArraySize;

	int32 phaseArraySize;
	int32 frequencyArraySize;
	int32 chirpRateArraySize;


	float64 x0 = 0.0, dx = 0.0;
	float32* phaseWrappedTrace = NULL;
	float32* frequencyTrace = NULL;


	/* Initialize a session */
	RFmxCheckWarn(RFmxPulse_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxPulse_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
		frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxInstr_SetLOLeakageAvoidanceEnabled(instrumentHandle, "", LOLeakageAvoidanceEnabled));
	RFmxCheckWarn(RFmxInstr_SetLOSource(instrumentHandle, "", LOSource));
	RFmxCheckWarn(RFmxInstr_SetDownconverterFrequencyOffset(instrumentHandle, "", downconverterFrequencyOffset));
	RFmxCheckWarn(RFmxPulse_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxPulse_SelectMeasurements(instrumentHandle, "", RFMXPULSE_VAL_PULSE, 0));
	RFmxCheckWarn(RFmxPulse_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXPULSE_VAL_IQ_POWER_EDGE_RISING_SLOPE,
		IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
	RFMXPULSE_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, IQPowerEdgeEnabled));
	RFmxCheckWarn(RFmxPulse_SetMeasurementBandwidth(instrumentHandle, "", measurementBandwidth));
	RFmxCheckWarn(RFmxPulse_SetMeasurementFilterType(instrumentHandle, "", measurementFilterType));
	RFmxCheckWarn(RFmxPulse_SetAcquisitionLength(instrumentHandle, "", acquisitionLength));
	RFmxCheckWarn(RFmxPulse_SetMaximumPulseCountEnabled(instrumentHandle, "", maximumPulseCountEnabled));
	RFmxCheckWarn(RFmxPulse_SetMaximumPulseCount(instrumentHandle, "", maximumPulseCount));
	RFmxCheckWarn(RFmxPulse_PulseSetDetectionReference(instrumentHandle, "", pulseDetectionReference));
	RFmxCheckWarn(RFmxPulse_PulseSetDetectionThreshold(instrumentHandle, "", pulseDetectionThreshold));
	RFmxCheckWarn(RFmxPulse_PulseSetDetectionHysteresis(instrumentHandle, "", pulseDetectionHysteresis));
	RFmxCheckWarn(RFmxPulse_PulseSetDetectionMinimumOffDuration(instrumentHandle, "", pulseDetectionMinimumOffDuration));
	RFmxCheckWarn(RFmxPulse_PulseSetLevelComputationMethod(instrumentHandle, "", pulseLevelComputationMethod));
	RFmxCheckWarn(RFmxPulse_PulseSetUpperThresholdLevel(instrumentHandle, "", pulseUpperThresholdLevel));
	RFmxCheckWarn(RFmxPulse_PulseSetWidthThresholdLevel(instrumentHandle, "", pulseWidthThresholdLevel));
	RFmxCheckWarn(RFmxPulse_PulseSetLowerThresholdLevel(instrumentHandle, "", pulseLowerThresholdLevel));
	RFmxCheckWarn(RFmxPulse_PulseSetMeasurementPointReference(instrumentHandle, "", pulseMeasurementPointReference));
	RFmxCheckWarn(RFmxPulse_PulseSetMeasurementPointOffset(instrumentHandle, "", pulseMeasurementPointOffset));
	RFmxCheckWarn(RFmxPulse_PulseSetMeasurementPointAveragingDuration(instrumentHandle, "",
		pulseMeasurementPointAveragingDuration));
	RFmxCheckWarn(RFmxPulse_PulseSetFrequencyAndPhaseDeviationRangeReference(instrumentHandle, "", 
		pulseFrequencyPhaseDeviationRangeReference));
	RFmxCheckWarn(RFmxPulse_PulseSetFrequencyAndPhaseDeviationRangeLength(instrumentHandle, "", 
		pulseFrequencyPhaseDeviationRangeLength));
	RFmxCheckWarn(RFmxPulse_PulseSetFrequencyAndPhaseDeviationRangeEdgeStart(instrumentHandle, "", 
		pulseFrequencyPhaseDeviationRangeEdgeStart));
	RFmxCheckWarn(RFmxPulse_PulseSetFrequencyAndPhaseDeviationRangeEdgeStop(instrumentHandle, "", 
		pulseFrequencyPhaseDeviationRangeEdgeStop));
	RFmxCheckWarn(RFmxPulse_PulseSetFrequencyAndPhaseModulationType(instrumentHandle, "", pulseModulationType));
	RFmxCheckWarn(RFmxPulse_PulseSetFrequencyAndPhaseCWFrequencyOffsetAuto(instrumentHandle, "", 
		pulseCWFrequencyOffsetAuto));
	RFmxCheckWarn(RFmxPulse_PulseSetFrequencyAndPhaseCWFrequencyOffset(instrumentHandle, "", pulseCWFrequencyOffset));


	RFmxCheckWarn(RFmxPulse_PulseSetMetricsEnabled(instrumentHandle, "", RFMXPULSE_VAL_TRUE));
	RFmxCheckWarn(RFmxPulse_PulseSetStabilityEnabled(instrumentHandle, "", RFMXPULSE_VAL_FALSE));
	RFmxCheckWarn(RFmxPulse_PulseSetAllTracesEnabled(instrumentHandle, "", RFMXPULSE_VAL_TRUE));

	RFmxCheckWarn(RFmxPulse_Initiate(instrumentHandle, "", ""));

	RFmxCheckWarn(RFmxPulse_WaitForMeasurementComplete(instrumentHandle, "", timeout));

	/* Fetch results */
	RFmxCheckWarn(RFmxPulse_PulseGetResultsAveragePhase(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsAveragePhase = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsAveragePhase)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsAveragePhase(instrumentHandle, "", pulseResultsAveragePhase,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsPhaseDeviation(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsPhaseDeviation = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsPhaseDeviation)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsPhaseDeviation(instrumentHandle, "", pulseResultsPhaseDeviation,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsPhaseErrorRMS(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsPhaseErrorRMS = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsPhaseErrorRMS)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsPhaseErrorRMS(instrumentHandle, "", pulseResultsPhaseErrorRMS,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	phaseArraySize = actualArraySize;
	
	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsAverageFrequency(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsAverageFrequency = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsAverageFrequency)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsAverageFrequency(instrumentHandle, "", pulseResultsAverageFrequency,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}	
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFrequencyDeviation(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsFrequencyDeviation = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsFrequencyDeviation)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsFrequencyDeviation(instrumentHandle, "", pulseResultsFrequencyDeviation,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFrequencyErrorRMS(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsFrequencyErrorRMS = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsFrequencyErrorRMS)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsFrequencyErrorRMS(instrumentHandle, "", pulseResultsFrequencyErrorRMS,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	frequencyArraySize = actualArraySize;

	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFMChirpRate(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsFMChirpRate = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsFMChirpRate)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsFMChirpRate(instrumentHandle, "", pulseResultsFMChirpRate,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFMChirpRate2(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsFMChirpRate2 = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsFMChirpRate2)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsFMChirpRate2(instrumentHandle, "", pulseResultsFMChirpRate2,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	chirpRateArraySize = actualArraySize;

	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_FetchPhaseWrappedTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
		&actualArraySize));
	if (actualArraySize > 0)
	{
		phaseWrappedTrace = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if (phaseWrappedTrace)
		{
			RFmxCheckWarn(RFmxPulse_FetchPhaseWrappedTrace(instrumentHandle, "", timeout, &x0, &dx, phaseWrappedTrace,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

		actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_FetchFrequencyTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
		&actualArraySize));
	if (actualArraySize > 0)
	{
	 frequencyTrace = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if (frequencyTrace)
		{
			RFmxCheckWarn(RFmxPulse_FetchFrequencyTrace(instrumentHandle, "", timeout, &x0, &dx, frequencyTrace,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxPulse_PulseGetResultsAveragePhaseMean(instrumentHandle, "", &pulseResultsAveragePhaseMean));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsAveragePhaseMaximum(instrumentHandle, "", &pulseResultsAveragePhaseMaximum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsAveragePhaseMinimum(instrumentHandle, "", &pulseResultsAveragePhaseMinimum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsAveragePhaseStandardDeviation(instrumentHandle, "", &pulseResultsAveragePhaseSD));
	
	RFmxCheckWarn(RFmxPulse_PulseGetResultsPhaseDeviationMean(instrumentHandle, "", &pulseResultsPhaseDeviationMean));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsPhaseDeviationMaximum(instrumentHandle, "", &pulseResultsPhaseDeviationMaximum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsPhaseDeviationMinimum(instrumentHandle, "", &pulseResultsPhaseDeviationMinimum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsPhaseDeviationStandardDeviation(instrumentHandle, "", &pulseResultsPhaseDeviationSD));

	RFmxCheckWarn(RFmxPulse_PulseGetResultsPhaseErrorRMSMean(instrumentHandle, "", &pulseResultsPhaseErrorRMSMean));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsPhaseErrorRMSMaximum(instrumentHandle, "", &pulseResultsPhaseErrorRMSMaximum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsPhaseErrorRMSMinimum(instrumentHandle, "", &pulseResultsPhaseErrorRMSMinimum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsPhaseErrorRMSStandardDeviation(instrumentHandle, "", &pulseResultsPhaseErrorRMSSD));

	RFmxCheckWarn(RFmxPulse_PulseGetResultsAverageFrequencyMean(instrumentHandle, "", &pulseResultsAverageFrequencyMean));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsAverageFrequencyMaximum(instrumentHandle, "", &pulseResultsAverageFrequencyMaximum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsAverageFrequencyMinimum(instrumentHandle, "", &pulseResultsAverageFrequencyMinimum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsAverageFrequencyStandardDeviation(instrumentHandle, "", &pulseResultsAverageFrequencySD));
	
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFrequencyDeviationMean(instrumentHandle, "", &pulseResultsFrequencyDeviationMean));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFrequencyDeviationMaximum(instrumentHandle, "", &pulseResultsFrequencyDeviationMaximum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFrequencyDeviationMinimum(instrumentHandle, "", &pulseResultsFrequencyDeviationMinimum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFrequencyDeviationStandardDeviation(instrumentHandle, "", &pulseResultsFrequencyDeviationSD));

	RFmxCheckWarn(RFmxPulse_PulseGetResultsFrequencyErrorRMSMean(instrumentHandle, "", &pulseResultsFrequencyErrorRMSMean));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFrequencyErrorRMSMaximum(instrumentHandle, "", &pulseResultsFrequencyErrorRMSMaximum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFrequencyErrorRMSMinimum(instrumentHandle, "", &pulseResultsFrequencyErrorRMSMinimum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFrequencyErrorRMSStandardDeviation(instrumentHandle, "", &pulseResultsFrequencyErrorRMSSD));

	RFmxCheckWarn(RFmxPulse_PulseGetResultsFMChirpRateMean(instrumentHandle, "", &pulseResultsFMChirpRateMean));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFMChirpRateMaximum(instrumentHandle, "", &pulseResultsFMChirpRateMaximum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFMChirpRateMinimum(instrumentHandle, "", &pulseResultsFMChirpRateMinimum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFMChirpRateStandardDeviation(instrumentHandle, "", &pulseResultsFMChirpRateSD));

	RFmxCheckWarn(RFmxPulse_PulseGetResultsFMChirpRate2Mean(instrumentHandle, "", &pulseResultsFMChirpRate2Mean));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFMChirpRate2Maximum(instrumentHandle, "", &pulseResultsFMChirpRate2Maximum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFMChirpRate2Minimum(instrumentHandle, "", &pulseResultsFMChirpRate2Minimum));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFMChirpRate2StandardDeviation(instrumentHandle, "", &pulseResultsFMChirpRate2SD));



	printf("\n-----------------------Phase Results--------------------------\n\n");
	for (i = 0; i < phaseArraySize; i++)
	{
		printf("Index                                                : %d\n", i);
		printf("Average Phase (deg)                                  : %f\n", pulseResultsAveragePhase[i]);
		printf("Phase Deviation (deg)                                : %f\n", pulseResultsPhaseDeviation[i]);
		printf("Phase Error RMS (deg)                                : %f\n", pulseResultsPhaseErrorRMS[i]);
		printf("--------------------------------------------------------------\n");
	}

	printf("\n----------------------Frequency Results-----------------------\n\n");
	for (i = 0; i < frequencyArraySize; i++)
	{
		printf("Index                                                : %d\n", i);
		printf("Average Frequency (Hz)                               : %f\n", pulseResultsAverageFrequency[i]);
		printf("Frequency Deviation (Hz)                             : %f\n", pulseResultsFrequencyDeviation[i]);
		printf("Frequency Error RMS (Hz)                             : %f\n", pulseResultsFrequencyErrorRMS[i]);
		printf("--------------------------------------------------------------\n");
	}

	printf("\n\n---------------------FM Chirp Results-------------------------\n\n");
	for (i = 0; i < chirpRateArraySize; i++)
	{
		printf("Index                                                : %d\n", i);
		printf("Chrip Rate (Hz/us)                                   : %.11f\n", pulseResultsFMChirpRate[i]);
		printf("Chirp Rate2 (Hz/us)                                  : %.11f\n", pulseResultsFMChirpRate2[i]);
		printf("--------------------------------------------------------------\n");
	}

	printf("\n\n----------------Statistical Phase Results---------------------\n\n");
	printf("Average Phase Mean (deg)                             : %f\n", pulseResultsAveragePhaseMean);
	printf("Average Phase Maximum (deg)                          : %f\n", pulseResultsAveragePhaseMaximum);
	printf("Average Phase Minimum(deg)                           : %f\n", pulseResultsAveragePhaseMinimum);
	printf("Average Phase Standard Deviation (deg)               : %f\n", pulseResultsAveragePhaseSD);
	printf("Phase Deviation Mean (deg)                           : %f\n", pulseResultsPhaseDeviationMean);
	printf("Phase Deviation Maximum (deg)                        : %f\n", pulseResultsPhaseDeviationMaximum);
	printf("Phase Deviation Minimum(deg)                         : %f\n", pulseResultsPhaseDeviationMinimum);
	printf("Phase Deviation Standard Deviation (deg)             : %f\n", pulseResultsPhaseDeviationSD);	
	printf("Phase Error RMS Mean (deg)                           : %f\n", pulseResultsPhaseErrorRMSMean);
	printf("Phase Error RMS Maximum (deg)                        : %f\n", pulseResultsPhaseErrorRMSMaximum);
	printf("Phase Error RMS Minimum(deg)                         : %f\n", pulseResultsPhaseErrorRMSMinimum);
	printf("Phase Error RMS Standard Deviation (deg)             : %f\n", pulseResultsPhaseErrorRMSSD);
	printf("--------------------------------------------------------------\n");

	printf("\n\n--------------Statistical Frequency Results-------------------\n\n");
	printf("Average Frequency Mean (Hz)                          : %f\n", pulseResultsAverageFrequencyMean);
	printf("Average Frequency Maximum (Hz)                       : %f\n", pulseResultsAverageFrequencyMaximum);
	printf("Average Frequency Minimum(Hz)                        : %f\n", pulseResultsAverageFrequencyMinimum);
	printf("Average Frequency Standard Deviation (Hz)            : %f\n", pulseResultsAverageFrequencySD);
	printf("Frequency Deviation Mean (Hz)                        : %f\n", pulseResultsFrequencyDeviationMean);
	printf("Frequency Deviation Maximum (Hz)                     : %f\n", pulseResultsFrequencyDeviationMaximum);
	printf("Frequency Deviation Minimum(Hz)                      : %f\n", pulseResultsFrequencyDeviationMinimum);
	printf("Frequency Deviation Standard Deviation (Hz)          : %f\n", pulseResultsFrequencyDeviationSD);
	printf("Frequency Error RMS Mean (Hz)                        : %f\n", pulseResultsFrequencyErrorRMSMean);
	printf("Frequency Error RMS Maximum (Hz)                     : %f\n", pulseResultsFrequencyErrorRMSMaximum);
	printf("Frequency Error RMS Minimum(Hz)                      : %f\n", pulseResultsFrequencyErrorRMSMinimum);
	printf("Frequency Error RMS Standard Deviation (Hz)          : %f\n", pulseResultsFrequencyErrorRMSSD);
	printf("--------------------------------------------------------------\n");

	printf("\n\n--------------Statistical FM Chirp Results--------------------\n\n");
	printf("Chirp Rate Mean (Hz/us)                              : %f\n", pulseResultsFMChirpRateMean);
	printf("Chirp Rate Maximum (Hz/us)                           : %f\n", pulseResultsFMChirpRateMaximum);
	printf("Chirp Rate Minimum(Hz/us)                            : %f\n", pulseResultsFMChirpRateMinimum);
	printf("Chirp Rate Standard Deviation (Hz/us)               : %f\n", pulseResultsFMChirpRateSD);
	printf("Chirp Rate 2 Mean (Hz/us)                            : %f\n", pulseResultsFMChirpRate2Mean);
	printf("Chirp Rate 2 Maximum (Hz/us)                         : %f\n", pulseResultsFMChirpRate2Maximum);
	printf("Chirp Rate 2 Minimum(Hz/us)                          : %f\n", pulseResultsFMChirpRate2Minimum);
	printf("Chirp Rate 2 Standard Deviation (Hz/us)              : %f\n", pulseResultsFMChirpRate2SD);
	printf("--------------------------------------------------------------\n");

Error:
	if (error)
	{
		RFmxPulse_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if (error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}

	if (instrumentHandle)
	{
		RFmxPulse_Close(instrumentHandle, RFMXPULSE_VAL_FALSE);
	}

	/* Free allocated memory */
    if (pulseResultsAveragePhase)
    {
        free(pulseResultsAveragePhase);
    }
    if (pulseResultsPhaseDeviation)
    {
        free(pulseResultsPhaseDeviation);
    }
    if (pulseResultsPhaseErrorRMS)
    {
        free(pulseResultsPhaseErrorRMS);
    }
    if (pulseResultsAverageFrequency)
    {
        free(pulseResultsAverageFrequency);
    }
    if (pulseResultsFrequencyDeviation)
    {
        free(pulseResultsFrequencyDeviation);
    }
    if (pulseResultsFrequencyErrorRMS)
    {
        free(pulseResultsFrequencyErrorRMS);
    }
    if (pulseResultsFMChirpRate)
    {
        free(pulseResultsFMChirpRate);
    }
    if (pulseResultsFMChirpRate2)
    {
        free(pulseResultsFMChirpRate2);
    }
    if (phaseWrappedTrace)
    {
        free(phaseWrappedTrace);
    }
    if (frequencyTrace)
    {
        free(frequencyTrace);
    }

	printf("Press any key to exit\n");
	_getch();

	return error;
}
