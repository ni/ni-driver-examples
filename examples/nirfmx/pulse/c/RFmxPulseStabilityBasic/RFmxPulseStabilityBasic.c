//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure LO Leakage Avoidance Enabled, LO source and Downconverter Frequency Offset(Hz).
//4. Configure basic RF signal properties(Center Frequency, RF Attenuation and External Attenuation)
//5. Enabling Pulse and Traces Result.
//6. Configure Trigger Type and Trigger Parameters.
//7. Configure Acquisition Settings.
//8. Configure Measurement Point Settings.
//9. Configure Stability Settings.
//10. Configure Selected Traces setting and enabling Pulse Stability enabled and disable Pulse Metrics enabled settings.
//11. Initiate the Measurement.
//12. Wait for Measurement to complete.
//13. Results Average Stability, Per Pulse Stability Measurements, Stability and Pulse to Pulse Stability Traces.
//14. Close RFmx Session.


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

	int32 frequencySettlingUnits = RFMXINSTR_VAL_FREQUENCY_SETTLING_UNITS_PPM;
	float64 frequencySettling = 1.0e-1;                                           /* (s) */

	char* frequencyReferenceSource = RFMXPULSE_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10.0e6;                                 /* (Hz) */

	int32 LOLeakageAvoidanceEnabled = RFMXINSTR_VAL_LO_LEAKAGE_AVOIDANCE_ENABLED_TRUE;
	char* LOSource = RFMXINSTR_VAL_LO_SOURCE_ONBOARD;

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

	int32 pulseMeasurementPointReference = RFMXPULSE_VAL_PULSE_MEASUREMENT_POINT_REFERENCE_CENTER;
	float64 pulseMeasurementPointOffset = 0.0;                                    /* (s) */
	float64 pulseMeasurementPointAveragingDuration = 0.0;                         /* (s) */

	int32 pulseStabilityMeasurmentOffset = 0;
	int32 pulseStabilityReferenceOffset = 0;
	int32 pulseStabilityPulseToPulseOffset = 1;
	int32 pulseStabilityFrequencyErrorCompensation = RFMXPULSE_VAL_PULSE_STABILITY_FREQUENCY_ERROR_COMPENSATION_ON;

	int32 pulseSelectedPulseTrace = 0;

	float64 timeout = 10.0;                                                       /* (s) */

	float64 averageAmplitudeStability = 0.0;                                      /* (dB) */
	float64 averagePhaseStability = 0.0;                                          /* (dB) */
	float64 averageTotalStability = 0.0;                                          /* (dB) */

	float64* amplitudeStability = NULL;                                           /* (dB) */
	float64* phaseStability = NULL;                                               /* (dB) */
	float64* totalStability = NULL;                                               /* (dB) */

	int32 actualArraySize = 0, arraySize = 0;
	float64 x0 = 0.0, dx = 0.0;
	float32* pulseAmplitudeStability = NULL;                                      /* (dB) */
	float32* pulsePhaseStability = NULL;                                          /* (dB) */
	float32* pulseTotalStability = NULL;                                          /* (dB) */

	int32* pulseIndex = NULL;

	float64* pulseToPulseAmplitudeStability = NULL;                               /* (dB) */
	float64* pulseToPulsePhaseStability = NULL;                                   /* (dB) */
	float64* pulseToPulseTotalStability = NULL;                                   /* (dB) */


	/* Initialize a session */
	RFmxCheckWarn(RFmxPulse_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxPulse_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
		frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxInstr_SetLOLeakageAvoidanceEnabled(instrumentHandle, "", LOLeakageAvoidanceEnabled));
	RFmxCheckWarn(RFmxInstr_SetLOSource(instrumentHandle, "", LOSource));
	RFmxCheckWarn(RFmxInstr_SetDownconverterFrequencyOffset(instrumentHandle, "", downconverterFrequencyOffset));
	RFmxCheckWarn(RFmxInstr_SetFrequencySettlingUnits(instrumentHandle, "", frequencySettlingUnits));
	RFmxCheckWarn(RFmxInstr_SetFrequencySettling(instrumentHandle, "", frequencySettling));
	RFmxCheckWarn(RFmxPulse_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxPulse_SelectMeasurements(instrumentHandle, "", RFMXPULSE_VAL_PULSE, 1));
	RFmxCheckWarn(RFmxPulse_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXPULSE_VAL_IQ_POWER_EDGE_RISING_SLOPE,
		IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
	RFMXPULSE_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, IQPowerEdgeEnabled));
	RFmxCheckWarn(RFmxPulse_SetMeasurementBandwidth(instrumentHandle, "", measurementBandwidth));
	RFmxCheckWarn(RFmxPulse_SetMeasurementFilterType(instrumentHandle, "", measurementFilterType));
	RFmxCheckWarn(RFmxPulse_SetAcquisitionLength(instrumentHandle, "", acquisitionLength));
	RFmxCheckWarn(RFmxPulse_SetMaximumPulseCountEnabled(instrumentHandle, "", maximumPulseCountEnabled));
	RFmxCheckWarn(RFmxPulse_SetMaximumPulseCount(instrumentHandle, "", maximumPulseCount));
	RFmxCheckWarn(RFmxPulse_PulseSetMeasurementPointReference(instrumentHandle, "", pulseMeasurementPointReference));
	RFmxCheckWarn(RFmxPulse_PulseSetMeasurementPointOffset(instrumentHandle, "", pulseMeasurementPointOffset));
	RFmxCheckWarn(RFmxPulse_PulseSetMeasurementPointAveragingDuration(instrumentHandle, "", 
		pulseMeasurementPointAveragingDuration));
	RFmxCheckWarn(RFmxPulse_PulseSetStabilityMeasurementOffset(instrumentHandle, "", pulseStabilityMeasurmentOffset));
	RFmxCheckWarn(RFmxPulse_PulseSetStabilityReferenceOffset(instrumentHandle, "", 
		pulseStabilityReferenceOffset));
	RFmxCheckWarn(RFmxPulse_PulseSetStabilityPulseToPulseOffset(instrumentHandle, "", 
		pulseStabilityPulseToPulseOffset));
	RFmxCheckWarn(RFmxPulse_PulseSetStabilityFrequencyErrorCompensation(instrumentHandle, "", pulseStabilityFrequencyErrorCompensation));
	RFmxCheckWarn(RFmxPulse_PulseSetSelectedPulseTrace(instrumentHandle, "", pulseSelectedPulseTrace));
	RFmxCheckWarn(RFmxPulse_PulseSetMetricsEnabled(instrumentHandle, "", RFMXPULSE_VAL_FALSE));
	RFmxCheckWarn(RFmxPulse_PulseSetStabilityEnabled(instrumentHandle, "", RFMXPULSE_VAL_TRUE));

	RFmxCheckWarn(RFmxPulse_Initiate(instrumentHandle, "", ""));

	RFmxCheckWarn(RFmxPulse_WaitForMeasurementComplete(instrumentHandle, "", timeout));

	/* Fetch results */
	RFmxCheckWarn(RFmxPulse_PulseGetResultsAverageAmplitudeStability(instrumentHandle, "", &averageAmplitudeStability));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsAveragePhaseStability(instrumentHandle, "", &averagePhaseStability));
	RFmxCheckWarn(RFmxPulse_PulseGetResultsAverageTotalStability(instrumentHandle, "", &averageTotalStability));

	RFmxCheckWarn(RFmxPulse_PulseGetResultsAmplitudeStability(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		amplitudeStability = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (amplitudeStability)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsAmplitudeStability(instrumentHandle, "", amplitudeStability,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	arraySize = actualArraySize;

	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsPhaseStability(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		phaseStability = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (phaseStability)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsPhaseStability(instrumentHandle, "", phaseStability,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsTotalStability(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		totalStability = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (totalStability)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsTotalStability(instrumentHandle, "", totalStability,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}
	
	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_FetchStabilityTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, 0, 
		&actualArraySize));
	if (actualArraySize > 0)
	{
		pulseAmplitudeStability = (float32 *)malloc(sizeof(float32) * actualArraySize);
		pulsePhaseStability = (float32 *)malloc(sizeof(float32) * actualArraySize);
		pulseTotalStability = (float32*)malloc(sizeof(float32) * actualArraySize);

		if (pulseAmplitudeStability && pulsePhaseStability && pulseTotalStability)
		{
			RFmxCheckWarn(RFmxPulse_FetchStabilityTrace(instrumentHandle, "", timeout, &x0, &dx, pulseAmplitudeStability,
				pulsePhaseStability, pulseTotalStability, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_FetchPulseToPulseStabilityTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, 0, 
		&actualArraySize));
	if (actualArraySize > 0)
	{
		pulseIndex = (int32 *)malloc(sizeof(int32) * actualArraySize);
		pulseToPulseAmplitudeStability = (float64 *)malloc(sizeof(float64) * actualArraySize);
		pulseToPulsePhaseStability = (float64 *)malloc(sizeof(float64) * actualArraySize);
		pulseToPulseTotalStability = (float64 *)malloc(sizeof(float64) * actualArraySize);

		if(pulseIndex && pulseToPulseAmplitudeStability && pulseToPulsePhaseStability && pulseToPulseTotalStability)
		{
			RFmxCheckWarn(RFmxPulse_FetchPulseToPulseStabilityTrace(instrumentHandle, "", timeout, pulseIndex, 
			pulseToPulseAmplitudeStability, pulseToPulsePhaseStability, pulseToPulseTotalStability, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	printf("\n----------- Average Stability Results ----------- \n");
	printf("\nAverage Amplitude Stability (dB)       : %f\n", averageAmplitudeStability);
	printf("Average Phase Stability (dB)           : %f\n", averagePhaseStability);
	printf("Average Total Stability (dB)           : %f\n", averageTotalStability);

	printf("\n------------- Stability results ----------------- \n\n");
	for (i = 0; i < arraySize; i++)
	{
		printf("Index                                  : %d\n", i);
		printf("Amplitude Stability (dB)               : %f\n", amplitudeStability[i]);
		printf("Phase Stability (dB)                   : %f\n", phaseStability[i]);
		printf("Total Stability (dB)                   : %f\n", totalStability[i]);
		printf("-------------------------------------------------\n\n");
	}

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
    if (amplitudeStability) 
	{
        free(amplitudeStability);
    }
    if (phaseStability) 
	{
        free(phaseStability);
    }
    if (totalStability) 
	{
        free(totalStability);
    }
    if (pulseAmplitudeStability) 
	{
        free(pulseAmplitudeStability);
    }
    if (pulsePhaseStability) 
	{
        free(pulsePhaseStability);
    }
    if (pulseTotalStability) 
	{
        free(pulseTotalStability);
    }
    if (pulseToPulseAmplitudeStability) 
	{
        free(pulseToPulseAmplitudeStability);
    }
    if (pulseToPulsePhaseStability) 
	{
        free(pulseToPulsePhaseStability);
    }
    if (pulseToPulseTotalStability) 
	{
        free(pulseToPulseTotalStability);
    }
    if (pulseIndex) 
	{
        free(pulseIndex);
    }

	printf("Press any key to exit\n");
	_getch();

	return error;
}
