//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Contiguous Carriers.
//6. Select CHP measurement and enable traces.
//7. Configure Sweep Time Parameters.
//8. Configure Averaging Parameters.
//9. Initiate the Measurement.
//10. Fetch CHP Measurements and Traces.
//11. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxEVDO.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096
#define NUMBER_OF_CARRIERS			3

int main ()
{
	//RFSA Configuration
	char *rfsaResourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;

	int i = 0;
	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0;
	int32 lastErrorCode = 0;

	char * frequencyReferenceSource = RFMXEVDO_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;   /*(Hz) */

	float64 centerFrequency = 833.49e6;         /*(Hz) */
	float64 referenceLevel = 0.00;              /*(dBm) */
	float64 externalAttenuation = 0.00;         /*(dB) */
	int32 bandClass = 0;

	int32 enableTrigger = RFMXEVDO_VAL_FALSE;
	char * digitalEdgeSource = RFMXEVDO_VAL_PFI0_STR;
	int32 digitalEdge = RFMXEVDO_VAL_DIGITAL_EDGE_RISING_EDGE; /*ring*/
	float64 triggerDelay = 0.00;                               /*(s) */

	int32 carrierAtCenterFrequency = -1;
	int32 sweepTimeAuto = RFMXEVDO_VAL_CHP_SWEEP_TIME_AUTO_TRUE;       /*ring*/
	float64 sweepTimeInterval = 0.001670;                              /*(s) */
	int32 averagingEnabled = RFMXEVDO_VAL_CHP_AVERAGING_ENABLED_FALSE; /*ring*/
	int32 averagingCount = 10;
	int32 averagingType = RFMXEVDO_VAL_CHP_AVERAGING_TYPE_RMS;         /*ring*/

	float64 timeout = 10.00;										   /*(s) */
	float64 carrierAbsolutePower[NUMBER_OF_CARRIERS] = {0};			   /*(dBm) */
	float64 carrierRelativePower[NUMBER_OF_CARRIERS] = {0};            /*(dB) */
	float64 totalCarrierPower = 0.00;								   /*(dBm) */

	int32 actualArraySize = 0;
	float64 x0=0.0, dx=0.0;
	float32 *spectrum = NULL;										   /*(dBm) */

	/* Initialize a session */
	RFmxCheckWarn(RFmxEVDO_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));

	RFmxCheckWarn(RFmxEVDO_CfgFrequencyReference(instrumentHandle, "",frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxEVDO_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxEVDO_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxEVDO_CfgContiguousCarriers(instrumentHandle, "", NUMBER_OF_CARRIERS, carrierAtCenterFrequency, bandClass));
	RFmxCheckWarn(RFmxEVDO_SelectMeasurements(instrumentHandle, "", RFMXEVDO_VAL_CHP, RFMXEVDO_VAL_TRUE));
	RFmxCheckWarn(RFmxEVDO_CHPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
	RFmxCheckWarn(RFmxEVDO_CHPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
	RFmxCheckWarn(RFmxEVDO_Initiate(instrumentHandle, "", ""));


	/* Fetch the measurements array */
	RFmxCheckWarn(RFmxEVDO_CHPFetchCarrierMeasurementArray(instrumentHandle, "", timeout, carrierAbsolutePower,
		carrierRelativePower, NUMBER_OF_CARRIERS, NULL));

	RFmxCheckWarn(RFmxEVDO_CHPFetchTotalCarrierPower(instrumentHandle, "", timeout, &totalCarrierPower));

	RFmxCheckWarn(RFmxEVDO_CHPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, 
		&actualArraySize));
	if( actualArraySize > 0 )
	{
		spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if(spectrum)
		{
			RFmxCheckWarn(RFmxEVDO_CHPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	printf("Total Carrier Power (dBm)           : %f \n\n",totalCarrierPower);

	for(i=0;i<NUMBER_OF_CARRIERS;i++)
	{
		printf("\nCarrier Measurement                 : %d\n", i);
		printf("Absolute Power  (dBm)               : %f\n", carrierAbsolutePower[i]);
		printf("Relative Power  (dB)                : %f\n", carrierRelativePower[i]);
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

    if (spectrum)
    {
        free(spectrum);
    }

	printf("Press any key to exit\n");
	_getch();

	return error;
}
