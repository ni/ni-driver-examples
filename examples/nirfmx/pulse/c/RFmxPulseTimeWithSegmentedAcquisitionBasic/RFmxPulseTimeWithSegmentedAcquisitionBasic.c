//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic RF signal properties(Center Frequency, RF Attenuation and External Attenuation)
//4. Enabling Pulse and Disabling Traces Result
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Bandwidth and Filter Type.
//7. Configure Segmented Acquisition Settings.
//8. Configure Pulse Detection Settings.
//9. Configure State and Thershold Level Settings.
//10. Configure Pulse Metrics and Pulse Stability enabled settings.
//11. Initiate the Measurement.
//12. Wait for Measurement to complete.
//13. Fetch Pulse Count and Timing Results.
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

	char* frequencyReferenceSource = RFMXPULSE_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10.0e6;                                 /* (Hz) */

	int32 IQPowerEdgeEnabled = RFMXPULSE_VAL_TRUE;
	float64 IQPowerEdgeLevel = -20.0;                                             /* (dBm) */
	float64 triggerDelay = 0.0;                                                   /* (s) */
	int32 minimumQuietTimeMode = RFMXPULSE_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
	float64 minimumQuietTime = 5.0e-6;                                            /* (s) */

	float64 measurementBandwidth = 80.0e6;                                        /* (Hz) */
	int32 measurementFilterType = RFMXPULSE_VAL_MEASUREMENT_FILTER_TYPE_GAUSSIAN;
	int32 segmentAcquisitionEnabled = RFMXPULSE_VAL_SEGMENTED_ACQUISITION_ENABLED_TRUE;
	float64 segmentAcquisitionLength = 15.0e-6;                                   /* (s) */
	int32 numberOfSegments = 100;

	int32 pulseDetectionReference = RFMXPULSE_VAL_PULSE_DETECTION_REFERENCE_REFERENCE_LEVEL;
	float64 pulseDetectionThreshold = -20.0;
	float64 pulseDetectionHysteresis = 1.0;                                       /* (dB) */
	float64 pulseDetectionMinimumOffDuration = 50.0e-9;                                    /* (s) */

	int32 pulseLevelComputationMethod = RFMXPULSE_VAL_PULSE_LEVEL_COMPUTATION_METHOD_MEDIAN;
	int32 pulseAmplitudeLevelDomain = RFMXPULSE_VAL_PULSE_AMPLITUDE_LEVEL_DOMAIN_VOLTS;
	float64 pulseUpperThresholdLevel = 90.0;                                      /* (%) */
	float64 pulseWidthThresholdLevel = 50.0;                                      /* (%) */
	float64 pulseLowerThresholdLevel = 10.0;                                      /* (%) */

	float64 timeout = 10.0;                                                       /* (s) */

	int32 pulseCount = 0;

	float64* pulseResultsRiseTime = NULL;                                         /* (s) */
	float64* pulseResultsFallTime = NULL;                                         /* (s) */
	float64* pulseResultsPulseWidth = NULL;                                       /* (s) */
	float64* pulseResultsPulseOffDuration = NULL;                                 /* (s) */
	float64* pulseResultsDutyCycle = NULL;                                        /* (%) */
	float64* pulseResultsPusleRepetitionInterval = NULL;                          /* (s) */

	int32 actualArraySize = 0;
	float64 x0 = 0.0, dx = 0.0;


	/* Initialize a session */
	RFmxCheckWarn(RFmxPulse_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxPulse_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
		frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxPulse_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxPulse_SelectMeasurements(instrumentHandle, "", RFMXPULSE_VAL_PULSE, RFMXPULSE_VAL_TRUE));
	RFmxCheckWarn(RFmxPulse_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXPULSE_VAL_IQ_POWER_EDGE_RISING_SLOPE,
		IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
	RFMXPULSE_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, IQPowerEdgeEnabled));
	RFmxCheckWarn(RFmxPulse_SetMeasurementBandwidth(instrumentHandle, "", measurementBandwidth));
	RFmxCheckWarn(RFmxPulse_SetMeasurementFilterType(instrumentHandle, "", measurementFilterType));
	RFmxCheckWarn(RFmxPulse_SetSegmentedAcquisitionEnabled(instrumentHandle, "", segmentAcquisitionEnabled));
	RFmxCheckWarn(RFmxPulse_SetNumberOfSegments(instrumentHandle, "", numberOfSegments));
	RFmxCheckWarn(RFmxPulse_SetAcquisitionLength(instrumentHandle, "", segmentAcquisitionLength));
	RFmxCheckWarn(RFmxPulse_PulseSetDetectionReference(instrumentHandle, "", pulseDetectionReference));
	RFmxCheckWarn(RFmxPulse_PulseSetDetectionThreshold(instrumentHandle, "", pulseDetectionThreshold));
	RFmxCheckWarn(RFmxPulse_PulseSetDetectionHysteresis(instrumentHandle, "", pulseDetectionHysteresis));
	RFmxCheckWarn(RFmxPulse_PulseSetDetectionMinimumOffDuration(instrumentHandle, "", pulseDetectionMinimumOffDuration));
	RFmxCheckWarn(RFmxPulse_PulseSetLevelComputationMethod(instrumentHandle, "", pulseLevelComputationMethod));
	RFmxCheckWarn(RFmxPulse_PulseSetAmplitudeLevelDomain(instrumentHandle, "", pulseAmplitudeLevelDomain));
	RFmxCheckWarn(RFmxPulse_PulseSetUpperThresholdLevel(instrumentHandle, "", pulseUpperThresholdLevel));
	RFmxCheckWarn(RFmxPulse_PulseSetWidthThresholdLevel(instrumentHandle, "", pulseWidthThresholdLevel));
	RFmxCheckWarn(RFmxPulse_PulseSetLowerThresholdLevel(instrumentHandle, "", pulseLowerThresholdLevel));
	RFmxCheckWarn(RFmxPulse_PulseSetMetricsEnabled(instrumentHandle, "", RFMXPULSE_VAL_TRUE));
	RFmxCheckWarn(RFmxPulse_PulseSetStabilityEnabled(instrumentHandle, "", RFMXPULSE_VAL_FALSE));

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
	RFmxCheckWarn(RFmxPulse_PulseGetResultsPulseOffDuration(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsPulseOffDuration = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsPulseOffDuration)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsPulseOffDuration(instrumentHandle, "", pulseResultsPulseOffDuration,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxPulse_PulseGetResultsDutyCycle(instrumentHandle, "", NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		pulseResultsDutyCycle = (float64 *)malloc(sizeof(float64) * actualArraySize);
		if (pulseResultsDutyCycle)
		{
			RFmxCheckWarn(RFmxPulse_PulseGetResultsDutyCycle(instrumentHandle, "", pulseResultsDutyCycle,
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

	printf("\nPulse Count                                : %d\n\n", pulseCount);

	printf("\n--------------------Timing Results---------------------\n\n");
	for (i = 0; i < actualArraySize; i++)
	{
		printf("Index                                  : %d\n", i);
		printf("Rise Time (s)                          : %.11f\n", pulseResultsRiseTime[i]);
		printf("Fall Time (s)                          : %.11f\n", pulseResultsFallTime[i]);
		printf("Pulse Width (s)                        : %.9f\n", pulseResultsPulseWidth[i]);
		printf("Pulse Off Duration (s)                 : %.9f\n", pulseResultsPulseOffDuration[i]);
		printf("Duty Cycle (%%)                        : %.9f\n", pulseResultsDutyCycle[i]);
		printf("Pulse Repitition Interval (s)          : %.9f\n", pulseResultsPusleRepetitionInterval[i]);
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
    if (pulseResultsPulseOffDuration)
    {
        free(pulseResultsPulseOffDuration);
    }
    if (pulseResultsDutyCycle)
    {
        free(pulseResultsDutyCycle);
    }
    if (pulseResultsPusleRepetitionInterval)
    {
        free(pulseResultsPusleRepetitionInterval);
    }
	printf("Press any key to exit\n");
	_getch();

	return error;
}
