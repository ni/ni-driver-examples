//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic RF signal properties(Center Frequency, RF Attenuation and External Attenuation)
//4. Enabling Pulse and Traces Result
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Acquisition Settings.
//7. Configure Pulse Detection Settings.
//8. Configure State and Thershold Level Settings.
//9. Configure Selected Traces Settings and constant control, Pulse Metrics and Pulse Stability enabled settings.
//10. Initiate the Measurement.
//11. Wait for Measurement to complete.
//12. Fetch  Pulse Count, Timing, Amplitude Measurements and Amplitude Traces.
//13. Close RFmx Session.


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

	int32 pulseDetectionReference = RFMXPULSE_VAL_PULSE_DETECTION_REFERENCE_REFERENCE_LEVEL;
	float64 pulseDetectionThreshold = -20.0;
	float64 pulseDetectionHysteresis = 1.0;                                       /* (dB) */
	float64 pulseDetectionMinimumOffDuration = 50.0e-9;                                    /* (s) */

	int32 pulseLevelComputationMethod = RFMXPULSE_VAL_PULSE_LEVEL_COMPUTATION_METHOD_MEDIAN;
	int32 pulseDroopCompensationEnabled = RFMXPULSE_VAL_PULSE_DROOP_COMPENSATION_ENABLED_TRUE;

	int32 pulseSelectedPulseTrace = 0;
    int32 pulseAmplitudeTraceUnit = RFMXPULSE_VAL_PULSE_AMPLITUDE_TRACE_UNIT_DBM;

	float64 timeout = 10.0;                                                       /* (s) */

	int32 pulseCount = 0;

	float64* pulseResultsRiseTime = NULL;                                         /* (s) */
	float64* pulseResultsFallTime = NULL;                                         /* (s) */
	float64* pulseResultsPulseWidth = NULL;                                       /* (s) */
	float64* pulseResultsPusleRepetitionInterval = NULL;                          /* (s) */
	
	float64* pulseResultsTopLevel = NULL;                                         /* (dBm) */
	float64* pulseResultsBaseLevel = NULL;                                        /* (dBm) */
	float64* pulseResultsAverageOnLevel = NULL;                                   /* (dBm) */
	float64* pulseResultsOvershoot = NULL;                                        /* (%) */
	float64* pulseResultsDroop = NULL;                                            /* (%) */
	float64* pulseResultsRipple = NULL;                                           /* (%) */

	int32 actualArraySize = 0;
    int32 timingResultsArraySize = 0, levelResultsArraySize = 0;
	float64 x0 = 0.0, dx = 0.0;
	float32* amplitudeTrace = NULL;                                                    /* (dB) */


	/* Initialize a session */
	RFmxCheckWarn(RFmxPulse_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxPulse_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
		frequencyReferenceFrequency));
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
	RFmxCheckWarn(RFmxPulse_PulseSetDetectionReference(instrumentHandle, "", pulseDetectionReference));
	RFmxCheckWarn(RFmxPulse_PulseSetDetectionThreshold(instrumentHandle, "", pulseDetectionThreshold));
	RFmxCheckWarn(RFmxPulse_PulseSetDetectionHysteresis(instrumentHandle, "", pulseDetectionHysteresis));
	RFmxCheckWarn(RFmxPulse_PulseSetDetectionMinimumOffDuration(instrumentHandle, "", pulseDetectionMinimumOffDuration));
	RFmxCheckWarn(RFmxPulse_PulseSetLevelComputationMethod(instrumentHandle, "", pulseLevelComputationMethod));
	RFmxCheckWarn(RFmxPulse_PulseSetDroopCompensationEnabled(instrumentHandle, "", pulseDroopCompensationEnabled));
	RFmxCheckWarn(RFmxPulse_PulseSetMetricsEnabled(instrumentHandle, "", RFMXPULSE_VAL_TRUE));
	RFmxCheckWarn(RFmxPulse_PulseSetStabilityEnabled(instrumentHandle, "", RFMXPULSE_VAL_FALSE));
	RFmxCheckWarn(RFmxPulse_PulseSetSelectedPulseTrace(instrumentHandle, "", pulseSelectedPulseTrace));
    RFmxCheckWarn(RFmxPulse_PulseSetAmplitudeTraceUnit(instrumentHandle, "", pulseAmplitudeTraceUnit));

	RFmxCheckWarn(RFmxPulse_Initiate(instrumentHandle, "", ""));

	RFmxCheckWarn(RFmxPulse_WaitForMeasurementComplete(instrumentHandle, "", timeout));

	/* Fetch results */
	RFmxCheckWarn(RFmxPulse_PulseGetResultsPulseCount(instrumentHandle, "", &pulseCount));

	RFmxCheckWarn(RFmxPulse_PulseGetResultsRiseTime(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsRiseTime = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsRiseTime)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsRiseTime(instrumentHandle, "", pulseResultsRiseTime,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsFallTime(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsFallTime = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsFallTime)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsFallTime(instrumentHandle, "", pulseResultsFallTime,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}	
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsPulseWidth(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsPulseWidth = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsPulseWidth)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsPulseWidth(instrumentHandle, "", pulseResultsPulseWidth,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}
	
	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsPulseRepetitionInterval(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsPusleRepetitionInterval = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsPusleRepetitionInterval)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsPulseRepetitionInterval(instrumentHandle, "", 
				pulseResultsPusleRepetitionInterval, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}	
	}
	
	timingResultsArraySize = actualArraySize;

	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsTopLevel(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsTopLevel = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsTopLevel)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsTopLevel(instrumentHandle, "", pulseResultsTopLevel,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}	
	}
		
	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsBaseLevel(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsBaseLevel = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsBaseLevel)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsBaseLevel(instrumentHandle, "", pulseResultsBaseLevel,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}	
	}
		
	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsAverageOnLevel(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsAverageOnLevel = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsAverageOnLevel)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsAverageOnLevel(instrumentHandle, "", pulseResultsAverageOnLevel,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}	
	}
		
	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsOvershoot(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsOvershoot = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsOvershoot)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsOvershoot(instrumentHandle, "", pulseResultsOvershoot,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}	
	}
		
	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsDroop(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsDroop = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsDroop)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsDroop(instrumentHandle, "", pulseResultsDroop,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}	
	}
		
	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsRipple(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsRipple = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsRipple)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsRipple(instrumentHandle, "", pulseResultsRipple,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}	
	}
	
	levelResultsArraySize = actualArraySize;

	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_FetchAmplitudeTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, 
		&actualArraySize));
	if (actualArraySize > 0)
	{
		amplitudeTrace = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if (amplitudeTrace)
		{
			RFmxCheckWarn(RFmxPulse_FetchAmplitudeTrace(instrumentHandle, "", timeout, &x0, &dx, amplitudeTrace,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	printf("\nPulse Count                                : %d\n\n", pulseCount);

	printf("\n--------------------Timing Results---------------------\n\n");
	for (i = 0; i < timingResultsArraySize; i++)
	{
		printf("Index                                  : %d\n", i);
		printf("Rise Time (s)                          : %.11f\n", pulseResultsRiseTime[i]);
		printf("Fall Time (s)                          : %.11f\n", pulseResultsFallTime[i]);
		printf("Pulse Width (s)                        : %.9f\n", pulseResultsPulseWidth[i]);
		printf("Pulse Repitition Interval (s)          : %.9f\n", pulseResultsPusleRepetitionInterval[i]);
		printf("-------------------------------------------------------\n");
	}

	printf("\n\n--------------------Level Results----------------------\n\n");
	for (i = 0; i < levelResultsArraySize; i++)
	{
		printf("Index                                  : %d\n", i);
		printf("Top Level (dBm)                        : %f\n", pulseResultsTopLevel[i]);
		printf("Base Level (dBm)                       : %f\n", pulseResultsBaseLevel [i]);
		printf("Average on Level (dBm)                 : %f\n", pulseResultsAverageOnLevel[i]);
		printf("Overshoot (%%)                          : %f\n", pulseResultsOvershoot[i]);
		printf("Droop (%%)                              : %f\n", pulseResultsDroop[i]);
		printf("Ripple (%%)                             : %f\n", pulseResultsRipple[i]);
		printf("-------------------------------------------------------\n");
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
    if (pulseResultsRiseTime)
    {
        free(pulseResultsRiseTime);
    }
    if (pulseResultsFallTime)
    {
        free(pulseResultsFallTime);
    }
    if (pulseResultsPulseWidth)
    {
        free(pulseResultsPulseWidth);
    }
    if (pulseResultsPusleRepetitionInterval)
    {
        free(pulseResultsPusleRepetitionInterval);
    }
    if (pulseResultsTopLevel)
    {
        free(pulseResultsTopLevel);
    }
    if (pulseResultsBaseLevel)
    {
        free(pulseResultsBaseLevel);
    }
    if (pulseResultsAverageOnLevel)
    {
        free(pulseResultsAverageOnLevel);
    }
    if (pulseResultsOvershoot)
    {
        free(pulseResultsOvershoot);
    }
    if (pulseResultsDroop)
    {
        free(pulseResultsDroop);
    }
    if (pulseResultsRipple)
    {
        free(pulseResultsRipple);
    }
    if (amplitudeTrace)
    {
        free(amplitudeTrace);
    }

	printf("Press any key to exit\n");
	_getch();

	return error;
}
