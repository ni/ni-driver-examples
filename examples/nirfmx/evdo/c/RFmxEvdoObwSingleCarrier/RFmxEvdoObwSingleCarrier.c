//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select OBW measurement and enable traces.
//6. Configure Sweep Time Parameters.
//7. Configure Averaging Parameters.
//8. Initiate the Measurement.
//9. Fetch OBW Measurements and Traces.
//10. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxEVDO.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main ()
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;    

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0, lastErrorCode = 0;

	float64 centerFrequency = 833490000.000000;    /*(Hz) */
	float64 referenceLevel = 0.000000;                /*(dBm) */
	float64 externalAttenuation = 0.000000;            /*(dB) */

	char * frequencyReferenceSource = RFMXEVDO_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10000000.000000;            /*(Hz) */

	int32 enableTrigger = RFMXEVDO_VAL_FALSE;
	char * digitalEdgeSource = RFMXEVDO_VAL_PFI0_STR;
	int32 digitalEdge = RFMXEVDO_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.000000;                /*(s) */

	int32 sweepTimeAuto = RFMXEVDO_VAL_OBW_SWEEP_TIME_AUTO_TRUE;
	float64 sweepTimeInterval = 0.001670;            /*(s) */

	int32 averagingEnabled = RFMXEVDO_VAL_OBW_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;
	int32 averagingType = RFMXEVDO_VAL_OBW_AVERAGING_TYPE_RMS ;

	float64 timeout = 10.000000;                    /*(s) */
	int32 actualArraySize = 0;
	float64 x0 = 0.0, dx = 0.0;

	float64 stopFrequency = 0.000000;                /*(Hz) */
	float64 startFrequency = 0.000000;                /*(Hz) */
	float64 occupiedBandwidth = 0.000000;            /*(Hz) */
	float64 absolutePower = 0.000000;                /*(dBm) */
	float32* spectrum = NULL ;                        /*(dBm) */

	/* Initialize a session */
	RFmxCheckWarn(RFmxEVDO_Initialize(resourceName, "", &instrumentHandle, NULL));

	RFmxCheckWarn(RFmxEVDO_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxEVDO_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxEVDO_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxEVDO_SelectMeasurements(instrumentHandle, "", RFMXEVDO_VAL_OBW, RFMXEVDO_VAL_TRUE));
	RFmxCheckWarn(RFmxEVDO_OBWCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
	RFmxCheckWarn(RFmxEVDO_OBWCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
	RFmxCheckWarn(RFmxEVDO_Initiate(instrumentHandle, "", ""));

	RFmxCheckWarn(RFmxEVDO_OBWFetchMeasurement(instrumentHandle, "", timeout, &occupiedBandwidth, &absolutePower, 
		&startFrequency, &stopFrequency));
	RFmxCheckWarn(RFmxEVDO_OBWFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		spectrum = (float32 *) malloc(sizeof(float32) * actualArraySize);
		if( spectrum )
		{
			RFmxCheckWarn(RFmxEVDO_OBWFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	printf("------------Measurement------------\n");
	printf("Occupied Bandwidth (Hz)     : %lf\n",occupiedBandwidth);
	printf("Absolute Power (dBm)        : %lf\n",absolutePower);
	printf("Start Frequency (Hz)        : %lf\n",startFrequency);
	printf("Stop Frequency (Hz)         : %lf\n",stopFrequency);

Error:
	if( error )
	{
		RFmxEVDO_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if (error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}

	if(instrumentHandle)
	{
		RFmxEVDO_Close(instrumentHandle, RFMXEVDO_VAL_FALSE);
	}

	/* Free allocated memory */
    if (spectrum)
    {
        free(spectrum);
    }

	printf("Press any key to exit\n");
	_getch();

	return error;
}
