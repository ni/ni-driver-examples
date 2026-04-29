// Steps:
// 1.Open a new RFmx Session.
// 2.Configure Frequency Reference.
// 3.Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
// 4.Configure Trigger Type and Trigger Parameters.
// 5. Configure Subblock Configurations.
// 5A.Configure Number of Subblocks.
// 5B.Configure Subblock Frequency.
// 5C.Configure Component Carrier Spacing.
// 5D.Configure Number of Component Carriers.
// 5E.Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
// 6.Configure Duplex Scheme.
// 7.Select SlotPhase measurement and enable Traces.
// 8.Configure Measurement Interval.
// 9.Initiate the Measurement.
// 10. Fetch SlotPhase Measurements and Traces.
// 11. Close RFmx Session.  

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                     256

#define NUMBER_OF_SUBBLOCKS                     2
#define NUMBER_OF_COMPONENT_CARRIERS            1
#define NUMBER_OF_SLOTS			                20

/* Subblock inputs structure */
typedef struct
{
    float64 subblockFrequency;                                         /*(Hz) */
	int32 componentCarrierSpacingType;
	int32 componentCarrierAtCenterFrequency;
	float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS];   /*(Hz) */
	float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS];   /*(Hz) */
	int32 cellID[NUMBER_OF_COMPONENT_CARRIERS];
}subblockInput_t;

/* Subblock measurement outputs structure */
typedef struct
{
	float64 maximumPhaseDiscontinuity[NUMBER_OF_COMPONENT_CARRIERS];  /*(deg) */
}subblockMeasurement_t;


int main(int argc, char *argv[])
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;

	char subblockString[NUMBER_OF_SUBBLOCKS][MAX_SELECTOR_STRING];
	char subblockCarrierString[NUMBER_OF_SUBBLOCKS][MAX_SELECTOR_STRING];

	char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
	int32 error = 0, lastErrorCode = 0;
	int i = 0, j = 0;

	/* Frequency Reference */
	char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;						   /*(Hz) */


	/* Trigger */
	int32 enableTrigger = RFMXLTE_VAL_FALSE;
	char* digitalEdgeTriggerSource = RFMXLTE_VAL_PFI0_STR;
	int32 digitalEdgeTriggerEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.0;							               /* s */

	int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_FDD;
	int32 synchronizationMode = RFMXLTE_VAL_SLOTPHASE_SYNCHRONIZATION_MODE_SLOT;
	int32 measurementOffset = 0;                                      /* slots */
	int32 measurementLength = NUMBER_OF_SLOTS;                         /* slots */

	subblockInput_t subblockInput[NUMBER_OF_SUBBLOCKS] = {		       /*  Set up subblock 0 inputs  */
	    {	
		 0.0,				                                           /* subblockFrequency */	
         RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL,
		-1,														       /* componentCarrierAtCenterFrequency */
		{20e6},													       /* componentCarrierBandwidth */
		{0.0},														   /* componentCarrierFrequency */
		{0}                                                            /* cellID */
		},
		{				                                               /*  Set up subblock 1 inputs  */
		    30e6,					                                   /* subblockFrequency */	
		    RFMXLTE_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL,
		    -1,													       /* componentCarrierAtCenterFrequency */
			{20e6},													   /* componentCarrierBandwidth */
			{0.0},													   /* componentCarrierFrequency */
			{1}                                                        /* cellID */
		}
	};

	int32 uplinkDownlinkConfiguraiton = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;
	float64 timeout = 10.0;											   /*(s) */
	float64 centerFrequency = 1.95e+9;								   /* Hz */
	float64 referenceLevel = -10.0;									   /*(dBm) */
	float64 externalAttenuation = 0.0;								   /*(dB) */

	subblockMeasurement_t subblocksMsr[NUMBER_OF_SUBBLOCKS];

	/* variables to store SlotPhase Phase Discontinuities and traces */
	int32 actualArraySize = 0;
	float64 x0[NUMBER_OF_SUBBLOCKS] = { 0.0 }, dx[NUMBER_OF_SUBBLOCKS] = { 0.0 };
	float64 *slotPhaseDiscontinuity[NUMBER_OF_SUBBLOCKS] = { NULL };		   /*(dBm) */
	float32 *samplePhaseError[NUMBER_OF_SUBBLOCKS] = { NULL };		   /* deg */
	float32 *samplePhaseErrorLinearFit[NUMBER_OF_SUBBLOCKS] = { NULL };  /* deg */


	/* Set up subblock outputs */
	for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
	{
		for (j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++)
		{
			subblocksMsr[i].maximumPhaseDiscontinuity[j] = 0;
		}
	}

	/* Initialize a session */
	RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
		frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeTriggerSource,
		digitalEdgeTriggerEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxLTE_CfgNumberOfSubblocks(instrumentHandle, "", NUMBER_OF_SUBBLOCKS));

	for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
	{
		RFmxCheckWarn(RFmxLTE_BuildSubblockString("", i, MAX_SELECTOR_STRING, subblockString[i]));
        RFmxCheckWarn(RFmxLTE_SetSubblockFrequency(instrumentHandle, subblockString[i], subblockInput[i].subblockFrequency));
		RFmxCheckWarn(RFmxLTE_CfgComponentCarrierSpacing(instrumentHandle, subblockString[i],
			subblockInput[i].componentCarrierSpacingType,
			subblockInput[i].componentCarrierAtCenterFrequency));
		RFmxCheckWarn(RFmxLTE_CfgNumberOfComponentCarriers(instrumentHandle, subblockString[i],
			NUMBER_OF_COMPONENT_CARRIERS));
		RFmxCheckWarn(RFmxLTE_CfgComponentCarrierArray(instrumentHandle, subblockString[i],
			subblockInput[i].componentCarrierBandwidth, subblockInput[i].componentCarrierFrequency,
			subblockInput[i].cellID, NUMBER_OF_COMPONENT_CARRIERS));
	}

	RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));
	RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_SLOTPHASE, RFMXLTE_VAL_TRUE));
	RFmxCheckWarn(RFmxLTE_SlotPhaseCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode, measurementOffset, measurementLength));
	RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

	for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
	{
		RFmxCheckWarn(RFmxLTE_SlotPhaseFetchMaximumPhaseDiscontinuityArray(instrumentHandle, subblockString[i], timeout,
			subblocksMsr[i].maximumPhaseDiscontinuity, NUMBER_OF_COMPONENT_CARRIERS, NULL));

		actualArraySize = 0;
		RFmxCheckWarn(RFmxLTE_BuildCarrierString(subblockString[i], 0, MAX_SELECTOR_STRING, subblockCarrierString[i]));
		RFmxCheckWarn(RFmxLTE_SlotPhaseFetchPhaseDiscontinuities(instrumentHandle, subblockCarrierString[i], timeout, NULL, 0,
			&actualArraySize));
		if (actualArraySize > 0)
		{
			slotPhaseDiscontinuity[i] = (float64 *)malloc(sizeof(float64) * actualArraySize);
			if (slotPhaseDiscontinuity[i])
			{
				RFmxCheckWarn(RFmxLTE_SlotPhaseFetchPhaseDiscontinuities(instrumentHandle, subblockCarrierString[i], timeout,
					slotPhaseDiscontinuity[i], actualArraySize, NULL));
			}
			else
			{
				printf("malloc failed.\n");
				goto Error;
			}
		}

		actualArraySize = 0;
		RFmxCheckWarn(RFmxLTE_BuildCarrierString(subblockString[i], 0, MAX_SELECTOR_STRING, subblockCarrierString[i]));
		RFmxCheckWarn(RFmxLTE_SlotPhaseFetchSamplePhaseError(instrumentHandle, subblockCarrierString[i], timeout, NULL, NULL, NULL, 0,
			&actualArraySize));
		if (actualArraySize > 0)
		{
			samplePhaseError[i] = (float32 *)malloc(sizeof(float32) * actualArraySize);
			if (samplePhaseError[i])
			{
				RFmxCheckWarn(RFmxLTE_SlotPhaseFetchSamplePhaseError(instrumentHandle, subblockCarrierString[i], timeout, &x0[i], &dx[i],
					samplePhaseError[i], actualArraySize, NULL));
			}
			else
			{
				printf("malloc failed.\n");
				goto Error;
			}
		}

		actualArraySize = 0;
		RFmxCheckWarn(RFmxLTE_BuildCarrierString(subblockString[i], 0, MAX_SELECTOR_STRING, subblockCarrierString[i]));
		RFmxCheckWarn(RFmxLTE_SlotPhaseFetchSamplePhaseErrorLinearFitTrace(instrumentHandle, subblockCarrierString[i], timeout, NULL, NULL,
			NULL, 0, &actualArraySize));
		if (actualArraySize > 0)
		{
			samplePhaseErrorLinearFit[i] = (float32 *)malloc(sizeof(float32) * actualArraySize);
			if (samplePhaseErrorLinearFit[i])
			{
				RFmxCheckWarn(RFmxLTE_SlotPhaseFetchSamplePhaseErrorLinearFitTrace(instrumentHandle, subblockCarrierString[i], timeout, &x0[i], &dx[i],
					samplePhaseErrorLinearFit[i], actualArraySize, NULL));
			}
			else
			{
				printf("malloc failed.\n");
				goto Error;
			}
		}
	}

	printf("\n********** Subblock Measurements ********** \n");
	for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
	{
		printf("Subblock %d\n", i);
		for (j = 0; j < NUMBER_OF_COMPONENT_CARRIERS; j++)
		{
			printf("Carrier  %d\n", j);
			printf("Maximum  Phase Discontinuity (deg)                      : %f\n", subblocksMsr[i].maximumPhaseDiscontinuity[j]);
			printf("---------------------------------------------\n");
		}
	}

Error:
	if (error)
	{
		RFmxLTE_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if (error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}

	if (instrumentHandle)
	{
		RFmxLTE_Close(instrumentHandle, RFMXLTE_VAL_FALSE);
	}
	/* Free allocated memory */
	for (i = 0; i < NUMBER_OF_SUBBLOCKS; i++)
	{
		if (slotPhaseDiscontinuity[i])
			free(slotPhaseDiscontinuity[i]);
		if (samplePhaseError[i])
			free(samplePhaseError[i]);
		if (samplePhaseErrorLinearFit[i])
			free(samplePhaseErrorLinearFit[i]);
	}
	printf("Press any key to exit\n");
	_getch();

	return error;
}
