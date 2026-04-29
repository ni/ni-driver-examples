//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Parameters for Digital Edge Trigger.
//6. Configure Link Direction, Frequency Range, Channel Raster, Component Carrier Spacing and Number of Component Carriers.
//7. Configure Component Carriers.
//8. Configure Subcarrier Spacing for all Component Carriers.
//9. Select TXP measurement and enable Traces.
//10. Configure Measurement Offset & Measurement Length Parameters and Averaging Parameters for TXP measurement.
//11. Initiate the Measurement.
//12. Fetch TXP Traces and Measurements.
//13. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <string.h>

#include "niRFmxNR.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION               4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                 256

#define NUMBER_OF_COMPONENT_CARRIERS        2

int main(int argc, char *argv[])
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;

	char subblockString[MAX_SELECTOR_STRING];
   char carrierString[MAX_SELECTOR_STRING];

	char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
	int32 error = 0, lastErrorCode = 0;
	int i = 0;

	char* selectedPorts = "";
	float64 centerFrequency = 3.5e9;                                                          /* (Hz) */
	float64 referenceLevel = 0.0;                                                             /* (dBm) */
	float64 externalAttenuation = 0.0;                                                        /* (dB) */

	char *frequencyReferenceSource = RFMXNR_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;                                               /* (Hz) */

	int32 digitalTriggerEnabled = RFMXNR_VAL_FALSE;
	char* digitalEdgeSource = RFMXNR_VAL_PXI_TRIG0_STR;
	float64 triggerDelay = 0.0;                                                               /* (s) */
	int32 digitalEdge = RFMXNR_VAL_DIGITAL_EDGE_RISING_EDGE;

	int32 linkDirection = RFMXNR_VAL_LINK_DIRECTION_UPLINK;

	int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;

	int32 componentCarrierSpacingType = RFMXNR_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL;
	float64 channelRaster = 15e3;                                                             /* (Hz) */
	int32 componentCarrierAtCenterFrequency = -1;
	float64 subcarrierSpacing = 30e3;                                                         /* (Hz) */
	
	float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS] = { 100e6, 100e6 };       /* (Hz) */
	float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS] = { -49.98e6, 50.01e6 };  /* (Hz) */

	float64 measurementOffset = 0.0;                                                          /* (s) */
	float64 measurementLength = 1.0e-3;                                                       /* (s) */

	int32 averagingEnabled = RFMXNR_VAL_TXP_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;

	float64 timeout = 10.0;                                                                   /* (s) */

	float64 aveargePowerMean = 0.0;                                                          /* (dBm) */
	float64 peakPowerMaximum = 0.0;                                                          /* (dBm) */

	int32 actualArraySize = 0, arraySize = 0;
	float64 x0 = 0.0, dx = 0.0;
	float32 *power = NULL;

	/* Initialize a session */
	RFmxCheckWarn(RFmxNR_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxNR_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
		frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxNR_SetSelectedPorts(instrumentHandle, "", selectedPorts));
	RFmxCheckWarn(RFmxNR_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxNR_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge,
		triggerDelay, digitalTriggerEnabled));

	RFmxCheckWarn(RFmxNR_SetLinkDirection(instrumentHandle, "", linkDirection));
	RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, "", frequencyRange));
	RFmxCheckWarn(RFmxNR_SetChannelRaster(instrumentHandle, "", channelRaster));
	RFmxCheckWarn(RFmxNR_SetComponentCarrierSpacingType(instrumentHandle, "", componentCarrierSpacingType));
	RFmxCheckWarn(RFmxNR_SetComponentCarrierAtCenterFrequency(instrumentHandle, "", componentCarrierAtCenterFrequency));

	RFmxCheckWarn(RFmxNR_SetNumberOfComponentCarriers(instrumentHandle, "", NUMBER_OF_COMPONENT_CARRIERS));

	RFmxNR_BuildSubblockString("", 0, MAX_SELECTOR_STRING, subblockString);
	for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
	{
		RFmxNR_BuildCarrierString(subblockString, i, MAX_SELECTOR_STRING, carrierString);
		RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, carrierString,
			componentCarrierBandwidth[i]));
		RFmxCheckWarn(RFmxNR_SetComponentCarrierFrequency(instrumentHandle, carrierString,
			componentCarrierFrequency[i]));
	}

	strcpy_s(carrierString, sizeof("carrier::all"), "carrier::all");
	RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, carrierString, subcarrierSpacing));

	RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_TXP, RFMXNR_VAL_TRUE));

	RFmxCheckWarn(RFmxNR_TXPSetMeasurementInterval(instrumentHandle, "", measurementLength));
	RFmxCheckWarn(RFmxNR_TXPSetMeasurementOffset(instrumentHandle, "", measurementOffset));
	RFmxCheckWarn(RFmxNR_TXPSetAveragingEnabled(instrumentHandle, "", averagingEnabled));
	RFmxCheckWarn(RFmxNR_TXPSetAveragingCount(instrumentHandle, "", averagingCount));

	RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

	/* Fetch results */

	RFmxCheckWarn(RFmxNR_TXPFetchMeasurement(instrumentHandle, "", timeout, &aveargePowerMean,
		&peakPowerMaximum));

	RFmxCheckWarn(RFmxNR_TXPFetchPowerTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if (actualArraySize > 0)
	{
		power = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if (power)
		{
			RFmxCheckWarn(RFmxNR_TXPFetchPowerTrace(instrumentHandle, "", timeout, &x0, &dx, power,
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	printf("\n-------------Measurement-------------\n\n");
	printf("Average Power Mean (dBm) : %lf\n", aveargePowerMean);
	printf("Peak Power Maximum (dBm) : %lf\n", peakPowerMaximum);

Error:
	if (error)
	{
		RFmxNR_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if (error < 0)
			printf("\nERROR: %s\n", errorMessage);
		else
			printf("\nWARNING: %s\n", errorMessage);
	}
	if (instrumentHandle)
	{
		RFmxNR_Close(instrumentHandle, RFMXNR_VAL_FALSE);
	}

	if (power)
		free(power);

	printf("\nPress any key to exit\n");
	_getch();

	return error;
}
