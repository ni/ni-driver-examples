//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Channel Configuration Mode.
//6. Configure Physical Layer Subtype.
//7. Configure Uplink Data Modulation Type.
//8. Configure Uplink Spreading Parameters. 
//9. Select CDA measurement and enable traces.
//10. Configure Synchronization Mode and Interval.
//11. Configure Measurement Channel.
//12. Configure Power Unit.
//13. Initiate the Measurement.
//14. Fetch CDA Measurements and Traces.
//15. Close the RFmx session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxEVDO.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main (int argc, char *argv[])
{	    
	niRFmxInstrHandle instrumentHandle = NULL;
	int32 error = 0, lastErrorCode = 0;
	char errorMessage[MAX_ERROR_DESCRIPTION] = {'\0'};

	char* resourceName = "RFSA";
	float64 centerFrequency = 833.490e+6;								/* Hz */
	float64 externalAttenuation = 0.00;									/* dB */

	float64 referenceLevel = 0.00;										/* dBm */


	/* Frequency Reference */
	char *frequencyReferenceSource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10.0e+6;						/* Hz */

	/* Trigger */
	int32 enableTrigger = RFMXEVDO_VAL_FALSE;
	char* digitalEdgeSource = RFMXEVDO_VAL_PFI0_STR;
	int32 digitalEdge =  RFMXEVDO_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.0;											/* s */

	int64 uplinkSpreadingIMask = 0;
	int64 uplinkSpreadingQMask = 0;
	int32 synchronizationMode = RFMXEVDO_VAL_CDA_SYNCHRONIZATION_MODE_SLOT;
	int32 channelConfigurationMode = RFMXEVDO_VAL_CHANNEL_CONFIGURATION_MODE_AUTO_DETECT;
    int32 measurementOffset = 0 ;									    /* slots */
	int32 measurementLength = 1;									    /* slots */

	/* Measurement Channel */
	int32 walshCodeNumber = 0;
	int32 walshCodeLength = 16;											
	int32 branch = RFMXEVDO_VAL_UPLINK_BRANCH_I;

	int32 powerUnit = RFMXEVDO_VAL_CDA_POWER_UNIT_DB;
	int32 physicalLayerSubtype = RFMXEVDO_VAL_PHYSICAL_LAYER_SUBTYPE_0_1;
	int32 uplinkDataModulationType = RFMXEVDO_VAL_UPLINK_DATA_MODULATION_TYPE_AUTO;

	float64 timeout = 10.0;	
	int32 actualArraySize = 0;

	/* Variables to store CDA Code Domain Power */	
	float64 totalPower = 0.0;											/* dBm */
	float64 totalActivePower = 0.0;                                     /* dB or dBm */
	float64 meanActivePower = 0.0;										/* dB or dBm */
	float64 peakActivePower = 0.0;										/* dB or dBm */
	float64 meanInactivePower = 0.0;									/* dB or dBm */
	float64 peakInactivePower = 0.0;									/* dB or dBm */

	/* Variables to store Code Domain I and Q Power */	
	float64 IMeanActivePower = 0.0;										/* dB or dBm */
	float64 QMeanActivePower = 0.0;										/* dB or dBm */
	float64 IPeakInactivePower = 0.0;									/* dB or dBm */
	float64 QPeakInactivePower = 0.0;									/* dB or dBm */

	/* Variables to store IQ Impairments */	
    float64 IQOriginOffset = 0.0;										/* dB */
	float64 IQGainImbalance = 0.0;										/* dB */
	float64 IQQuadratureError = 0.0;									/* deg */

	/* Variables to store Symbol EVM */	
    float64 RMSSymbolEVM = 0.0;											/* % */
	float64 peakSymbolEVM = 0.0;										/* % */
	float64 frequencyError = 0.0;										/* Hz */										
	float64 RMSSymbolMagnitudeError = 0.0;								/* % */
	float64 RMSSymbolPhaseError = 0.0;									/* deg */
	float64 meanSymbolPower = 0.0;                                      /* dB or dBm */
	float64 chipRateError = 0.0;                                        /* ppm */

	/* variables to store traces */
	float32* ICodeDomainPowers = NULL;                                  /* dB or dBm */
	float32* QCodeDomainPowers = NULL;                                  /* dB or dBm */
	float32* symbolEVM = NULL;                                          /* % */
	NIComplexSingle* symbolConstellation = NULL;

	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxEVDO_Initialize(resourceName, "", &instrumentHandle, NULL));

	/* Configure EVDO CDA measurement parameters */
	RFmxCheckWarn(RFmxEVDO_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxEVDO_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxEVDO_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxEVDO_CfgChannelConfigurationMode(instrumentHandle, "", channelConfigurationMode));
	RFmxCheckWarn(RFmxEVDO_CfgPhysicalLayerSubtype(instrumentHandle, "", physicalLayerSubtype));
	RFmxCheckWarn(RFmxEVDO_CfgUplinkDataModulationType(instrumentHandle, "", uplinkDataModulationType));
	RFmxCheckWarn(RFmxEVDO_CfgUplinkSpreading(instrumentHandle,"", uplinkSpreadingIMask, uplinkSpreadingQMask));
	RFmxCheckWarn(RFmxEVDO_SelectMeasurements(instrumentHandle, "", RFMXEVDO_VAL_CDA, RFMXEVDO_VAL_TRUE));
	RFmxCheckWarn(RFmxEVDO_CDACfgSynchronizationModeAndInterval(instrumentHandle,"", synchronizationMode, measurementOffset, measurementLength)); 
	RFmxCheckWarn(RFmxEVDO_CDACfgUplinkMeasurementChannel(instrumentHandle, "", walshCodeLength, walshCodeNumber, branch));
	RFmxCheckWarn(RFmxEVDO_CDACfgPowerUnit(instrumentHandle, "", powerUnit));
	RFmxCheckWarn(RFmxEVDO_Initiate(instrumentHandle,"",""));

	/* Fetch diverse CDA Measurement Results */	

	RFmxCheckWarn(RFmxEVDO_CDAFetchUplinkCodeDomainPower(instrumentHandle, "", timeout, &totalPower, &totalActivePower, &meanActivePower, &peakActivePower,
		                                                       &meanInactivePower, &peakInactivePower));

	RFmxCheckWarn(RFmxEVDO_CDAFetchUplinkCodeDomainIAndQPower(instrumentHandle, "", timeout, &IMeanActivePower,
		                                                       &QMeanActivePower, &IPeakInactivePower, &QPeakInactivePower));
		
	RFmxCheckWarn(RFmxEVDO_CDAFetchUplinkSymbolEVM(instrumentHandle, "", timeout, &RMSSymbolEVM, &peakSymbolEVM, 
		                                                       &RMSSymbolMagnitudeError, &RMSSymbolPhaseError, &meanSymbolPower, &frequencyError, &chipRateError));

	RFmxCheckWarn(RFmxEVDO_CDAFetchIQImpairments(instrumentHandle, "", timeout, &IQOriginOffset, &IQGainImbalance, &IQQuadratureError));

	RFmxCheckWarn(RFmxEVDO_CDAFetchUplinkCodeDomainIAndQPowerTrace(instrumentHandle, "", timeout, NULL, NULL, 0, 
		&actualArraySize));
	if( actualArraySize > 0 )
	{
		ICodeDomainPowers = (float32 *)malloc(sizeof(float32) * actualArraySize);
		QCodeDomainPowers = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if(ICodeDomainPowers && QCodeDomainPowers)
		{
			RFmxCheckWarn(RFmxEVDO_CDAFetchUplinkCodeDomainIAndQPowerTrace(instrumentHandle, "", timeout, ICodeDomainPowers, QCodeDomainPowers, actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxEVDO_CDAFetchUplinkSymbolEVMTrace(instrumentHandle, "", timeout, NULL, 0, 
		&actualArraySize));
	if( actualArraySize > 0 )
	{
		symbolEVM = (float32 *)malloc(sizeof(float32) * actualArraySize);
		if(symbolEVM)
		{
			RFmxCheckWarn(RFmxEVDO_CDAFetchUplinkSymbolEVMTrace(instrumentHandle, "", timeout, symbolEVM, actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxEVDO_CDAFetchUplinkSymbolConstellationTrace(instrumentHandle, "", timeout, NULL, 0, 
		&actualArraySize));
	if( actualArraySize > 0 )
	{
		symbolConstellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * actualArraySize);
		if(symbolConstellation)
		{
			RFmxCheckWarn(RFmxEVDO_CDAFetchUplinkSymbolConstellationTrace(instrumentHandle, "", timeout, symbolConstellation, actualArraySize, NULL));		
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}


	/* Display Code Domain Power  Results */	
	printf("\n-------------- Code Domain Power  --------------\n");
	printf("Total Power (dBm)                          : %f\n", totalPower);
	printf("Total Active Power (dB or dBm)             : %f\n", totalActivePower);
	printf("Mean Inactive Power (dB or dBm)            : %f\n", meanInactivePower);
	printf("Peak Inactive Power (dB or dBm)            : %f\n", peakInactivePower);
	printf("I Peak Inactive Power (dB or dBm)          : %f\n", IPeakInactivePower);
	printf("Q Peak Inactive Power (dB or dBm)          : %f\n", QPeakInactivePower);

	/* Display IQ Impairments Results */
	printf("\n-------------- IQ Impairments  --------------\n");
	printf("I/Q Origin Offset (dB)                     : %f\n", IQOriginOffset);
	printf("I/Q Gain Imbalance (dB)                    : %f\n", IQGainImbalance);
	printf("I/Q Quadrature Error (deg)                 : %f\n", IQQuadratureError);


	/* Display Symbol EVM Results */	
	printf("\n-------------- Symbol EVM  --------------\n");
	printf("RMS Symbol EVM (%%)                         : %f\n", RMSSymbolEVM);
	printf("Peak Symbol EVM (%%)                        : %f\n", peakSymbolEVM);
	printf("Frequency Error (Hz)                       : %f\n", frequencyError);
	printf("RMS Symbol Magnitude Error (%%)             : %f\n", RMSSymbolMagnitudeError);
	printf("RMS Symbol Phase Error (deg)               : %f\n", RMSSymbolPhaseError);
	printf("Mean Symbol Power (dB or dBm)              : %f\n", meanSymbolPower);
	printf("Chip Rate Error (ppm)                      : %f\n", chipRateError);
	
Error:
	if( error ) 
	{
		RFmxEVDO_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if(error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}
	if(instrumentHandle)
	{
		RFmxEVDO_Close(instrumentHandle, RFMXEVDO_VAL_FALSE);
	}

	/* Free allocated memory */	
	if(ICodeDomainPowers)
		free(ICodeDomainPowers);
	if(QCodeDomainPowers)
		free(QCodeDomainPowers);
	if(symbolEVM)
		free(symbolEVM);
	if(symbolConstellation)
		free(symbolConstellation);
	
	printf("\nPress any key to exit");
	_getch();
	return error;
}
