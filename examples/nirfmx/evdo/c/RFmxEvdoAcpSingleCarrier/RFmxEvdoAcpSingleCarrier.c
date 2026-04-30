//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure RF Attenuation.
//4. Configure the basic signal properties (External Attenuation, Center Frequency and Reference Level).
//5. Configure Trigger Type and Trigger Parameters.
//6. Select ACP measurement and enable traces.
//7. Configure Band Class.
//8. Configure Measurement Method.
//9. Configure Averaging Parameters.
//10. Configure Sweep Time Parameters.
//11. Configure Noise Compensation Parameter.
//12. Configure Number of Offsets.
//13. Initiate the Measurement.
//14. Fetch ACP Measurements and Traces.
//15. Close the RFmx Session.

// Include files

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <stdlib.h>

#include "niRFmxEVDO.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION 4096
#define NUMBER_OF_OFFSETS	  2

int main ()
{
	//RFSA Configuration
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;

	char errorMessage[MAX_ERROR_DESCRIPTION];
	int32 error = 0, lastErrorCode = 0;

	int i = 0;    

	char * frequencyReferenceSource = RFMXEVDO_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;                         /*(Hz) */

	float64 centerFrequency = 833490000.000000;                         /*(Hz) */
	float64 externalAttenuation = 0.000000;                             /*(dB) */

	int32 RFAttenuationAuto = 1;
	float64 RFAttenuation = 10.000000;                                  /*(dB) */

	int32 enableTrigger = RFMXEVDO_VAL_FALSE;
	char * digitalEdgeSource = RFMXEVDO_VAL_PFI0_STR;
	int32 digitalEdge = RFMXEVDO_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.000000;                                    /*(s) */
	int32 bandClass = 0;

	float64 measurementInterval = 0.026670;                             /*(s) */
	float64 referenceLevel = 0.000000;                                  /*(dBm) */
	int32 measurementMethod = RFMXEVDO_VAL_ACP_MEASUREMENT_METHOD_NORMAL;
	int32 averagingEnabled = RFMXEVDO_VAL_ACP_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;
	int32 averagingType = RFMXEVDO_VAL_ACP_AVERAGING_TYPE_RMS;

	int32 autoLevel = RFMXEVDO_VAL_TRUE;

	int32 sweepTimeAuto = RFMXEVDO_VAL_ACP_SWEEP_TIME_AUTO_TRUE;
	float64 sweepTimeInterval = 0.001670;                               /*(s) */

	int32 noiseCompensationEnabled = RFMXEVDO_VAL_ACP_NOISE_COMPENSATION_ENABLED_FALSE;

	float64 lowerAbsolutePower[NUMBER_OF_OFFSETS] = {0};                /*(dBm) */ /*array*/
	float64 upperAbsolutePower[NUMBER_OF_OFFSETS] = {0};                /*(dBm) */ /*array*/
	float64 lowerRelativePower[NUMBER_OF_OFFSETS] = {0};                /*(dB) */ /*array*/
	float64 upperRelativePower[NUMBER_OF_OFFSETS] = {0};                /*(dB) */ /*array*/
	float64 timeout = 10.000000;                                        /*(s) */
	float64 absolutePower = 0.000000;                                   /*(dBm) */
	float64 relativePower = 0.000000;                                   /*(dB) */

	int32 actualArraySize = 0;
	float64 x0=0.0,dx=0.0;
	float32 *spectrum = NULL;                                           /*(dBm) */

	/* Initialize a session */
	RFmxCheckWarn(RFmxEVDO_Initialize(resourceName, "", &instrumentHandle, NULL));

	RFmxCheckWarn(RFmxEVDO_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxEVDO_CfgFrequency(instrumentHandle, "", centerFrequency));
	RFmxCheckWarn(RFmxEVDO_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
	RFmxCheckWarn(RFmxEVDO_CfgRFAttenuation(instrumentHandle, "", RFAttenuationAuto, RFAttenuation));
	if( autoLevel )
	{
		RFmxCheckWarn(RFmxEVDO_AutoLevel(instrumentHandle, "", measurementInterval, &referenceLevel));
		printf("Reference level (dBm)               : %f\n", referenceLevel);
	}
	else
	{
		RFmxCheckWarn(RFmxEVDO_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
	}
	RFmxCheckWarn(RFmxEVDO_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxEVDO_SelectMeasurements(instrumentHandle, "", RFMXEVDO_VAL_ACP, RFMXEVDO_VAL_TRUE));
	RFmxCheckWarn(RFmxEVDO_CfgBandClass(instrumentHandle, "", bandClass));
	RFmxCheckWarn(RFmxEVDO_ACPCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
	RFmxCheckWarn(RFmxEVDO_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
	RFmxCheckWarn(RFmxEVDO_ACPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
	RFmxCheckWarn(RFmxEVDO_ACPCfgNoiseCompensationEnabled(instrumentHandle, "", noiseCompensationEnabled));
	RFmxCheckWarn(RFmxEVDO_ACPCfgNumberOfOffsets(instrumentHandle, "", NUMBER_OF_OFFSETS));
	RFmxCheckWarn(RFmxEVDO_Initiate(instrumentHandle, "", ""));

	/* Fetch the offset measurements array */
	RFmxCheckWarn(RFmxEVDO_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout,
		lowerRelativePower, upperRelativePower,
		lowerAbsolutePower, upperAbsolutePower,
		NUMBER_OF_OFFSETS, NULL));

	RFmxCheckWarn(RFmxEVDO_ACPFetchCarrierMeasurement(instrumentHandle, "", timeout, &absolutePower, &relativePower));
	RFmxCheckWarn(RFmxEVDO_ACPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if(spectrum)
		{
			RFmxCheckWarn(RFmxEVDO_ACPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, actualArraySize, NULL));	
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	printf("Carrier Absolute Power (dBm)        : %f \n\n", absolutePower);

	for(i=0;i<NUMBER_OF_OFFSETS;i++)
	{
		printf("\nOffset  :  %d\n", i);
		printf("Lower Relative Power  (dB)          : %f\n", lowerRelativePower[i]);
		printf("Upper Relative Power  (dB)          : %f\n", upperRelativePower[i]);
		printf("Lower Absolute Power  (dBm)         : %f\n", lowerAbsolutePower[i]);
		printf("Upper Absolute Power  (dBm)         : %f\n", upperAbsolutePower[i]);
	}

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
