//Steps:
//1. Open a new RFmxInstrMX session and create a Demod Signal
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure ADemod RBW, Measurement Interval, AM Carrier Suppressed and Averaging
//5. Read ADemod AM Measurement Results
//6. Dispose Demod Signal and Close the RFmxInstrMX Session

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxDemod.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main (int argc, char *argv[])
{
	niRFmxInstrHandle instrumentHandle = NULL;
	int32 error = 0, lastErrorCode = 0;
	char errorMessage[MAX_ERROR_DESCRIPTION];
		
	char *resourceName = "RFSA";
	char *selectedPorts = "";
	float64 centerFrequency = 1e+9;			/* Hz */
	float64 referenceLevel = 0.00;			/* dBm */
	float64 externalAttenuation = 0.00;		/* dB */
	
	float64 timeout = 10.0;					/* seconds */  

	/* Measurement Interval */
	float64 measurementInterval = 10.00e-3 ;	/* seconds */ 

	/* AM Carrier Suppressed*/
	int32 AMCarrierSuppressed = RFMXDEMOD_VAL_ADEMOD_AM_CARRIER_SUPPRESSED_FALSE;
	
	/* RBW Filter */
	int32 RBWFilterType = RFMXDEMOD_VAL_ADEMOD_RBW_FILTER_TYPE_FLAT;
	float64 RRCAlpha = 0.1;
	float64 RBW = 100.00e+3;			/* Hz */
	
	/* Averaging */
	int32 averagingEnabled = RFMXDEMOD_VAL_ADEMOD_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;
	int32 averagingType = RFMXDEMOD_VAL_ADEMOD_AVERAGING_TYPE_LINEAR;
	
	/* Variables to store the measurement results */
	float64 meanModulationDepth = 0;	/* %age */
	float64 meanCarrierPower = 0;		/* dBm */

	/* Create a new RFmx Session */
	RFmxCheckWarn(RFmxDemod_Initialize(resourceName,  "",  &instrumentHandle, NULL));
	
	/* Configure ADemod measurement parameters */
	RFmxCheckWarn(RFmxDemod_SetSelectedPorts(instrumentHandle, "", selectedPorts));
	RFmxCheckWarn(RFmxDemod_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxDemod_ADemodCfgRBWFilter(instrumentHandle, "", RBW, RBWFilterType, RRCAlpha));		
    RFmxCheckWarn(RFmxDemod_ADemodCfgMeasurementInterval(instrumentHandle, "", measurementInterval));	
	RFmxCheckWarn(RFmxDemod_ADemodCfgAMCarrierSuppressed(instrumentHandle, "", AMCarrierSuppressed));
	RFmxCheckWarn(RFmxDemod_ADemodCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));

	/* Retrieve results */
	RFmxCheckWarn(RFmxDemod_ADemodReadAM(instrumentHandle, "", timeout, &meanModulationDepth, 
									 &meanCarrierPower));

	/* Display results */
	printf("Mean Modulation Depth (%%)   : %f\n", meanModulationDepth);
	printf("Mean Carrier Power (dBm)    : %f\n", meanCarrierPower);
	
		
Error:
	if( error ) 
	{
		RFmxDemod_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if(error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}
	if(instrumentHandle)
	{
		RFmxDemod_Close(instrumentHandle, RFMXDEMOD_VAL_FALSE);
	}
	printf("Press any key to exit\n");
	_getch();
	return error;
}
