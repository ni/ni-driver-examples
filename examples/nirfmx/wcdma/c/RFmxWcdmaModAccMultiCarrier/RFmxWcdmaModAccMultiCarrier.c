/*Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Contiguous Carriers.
6. Configure Uplink Scramble (Array).
7. Select ModAcc measurement and enable Traces.
8. Configure Syncronization Mode and Interval.
9. Initiate the Measurement.
10[A-I]. Fetch ModAcc Measurements and Traces.
11.Close RFmx Session.*/ 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWCDMA.h"

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING			256


/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096
#define NUMBER_OF_CARRIERS			2


typedef struct {	
	float64 x0;
	float64 dx;
	float32* EVM;
	NIComplexSingle* constellation;
}carrierMeas_t;


int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	char carrierString[MAX_SELECTOR_STRING];
	int32 error = 0, lastErrorCode = 0;

	float64 centerFrequency = 1.95e9;						/*(Hz) */
	float64 referenceLevel = 0.000000;						/*(dBm) */
	float64 externalAttenuation = 0.000000;					/*(dB) */

	char * frequencyReferenceSource = RFMXWCDMA_VAL_ONBOARD_CLOCK_STR;
	float64 frequencyReferenceFrequency = 10e6;				/*(Hz) */

	int32 enableTrigger = RFMXWCDMA_VAL_FALSE;
	char * digitalEdgeSource = RFMXWCDMA_VAL_PFI0_STR;
	int32 digitalEdge = RFMXWCDMA_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.000000;						/*(s) */

	int32 uplinkScramblingType[NUMBER_OF_CARRIERS];
	int32 uplinkScramblingCode[NUMBER_OF_CARRIERS];

	int32 synchronizationMode = RFMXWCDMA_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;
	int32 measurementOffset = 0;							/*(slots) */
	int32 measurementLength = 1;							/*(slots) */

	int32 i = 0;
	int32 carrierAtCenterFrequency = -1;

	float64 timeout = 10.000000;							/*(s) */

	int32 actualArraySize = 0;

	float64 RMSEVM[NUMBER_OF_CARRIERS] = {0.0};				/*(%) */
	float64 peakEVM[NUMBER_OF_CARRIERS] = {0.0};			/*(%) */
	float64 rho[NUMBER_OF_CARRIERS] = {0.0};
	float64 frequencyError[NUMBER_OF_CARRIERS] = {0.0};		/*(Hz) */
	float64 RMSMagnitudeError[NUMBER_OF_CARRIERS] = {0.0};	/*(%) */
	float64 chipRateError[NUMBER_OF_CARRIERS] = {0.0};		/*(ppm) */	
	float64 RMSPhaseError[NUMBER_OF_CARRIERS] = {0.0};		/*(deg) */

	float64 IQOriginOffset[NUMBER_OF_CARRIERS] = {0.0};		/*(dB) */
	float64 IQGainImbalance[NUMBER_OF_CARRIERS] = {0.0};	/*(dB) */
	float64 IQQuadratureError[NUMBER_OF_CARRIERS] = {0.0};	/*(deg) */

	int32 peakCDEBranch[NUMBER_OF_CARRIERS] = {RFMXWCDMA_VAL_MODACC_PEAK_CDE_BRANCH_I};
	int32 peakCDECode[NUMBER_OF_CARRIERS] = {0};
	float64 peakCDE[NUMBER_OF_CARRIERS] = {0.0};			/*(dB) */

	float64 peakActiveCDE[NUMBER_OF_CARRIERS] = {0.0};		/*(dB) */
	int32 peakActiveCDESpreadingFactor[NUMBER_OF_CARRIERS] = {0};
	int32 peakActiveCDECode[NUMBER_OF_CARRIERS] = {0};
	int32 peakActiveCDEBranch[NUMBER_OF_CARRIERS] = {RFMXWCDMA_VAL_MODACC_PEAK_ACTIVE_CDE_BRANCH_I};


	float64 peakRCDE[NUMBER_OF_CARRIERS] = {0.0};			/*(dB) */
	int32 peakRCDESpreadingFactor[NUMBER_OF_CARRIERS] = {0};
	int32 peakRCDECode[NUMBER_OF_CARRIERS] = {0};
	int32 peakRCDEBranch[NUMBER_OF_CARRIERS] = {RFMXWCDMA_VAL_MODACC_PEAK_RCDE_BRANCH_I};


	carrierMeas_t			carrierChannelOutput[ NUMBER_OF_CARRIERS ];

	for(i=0; i<NUMBER_OF_CARRIERS; i++)
	{
	 uplinkScramblingType[i] = RFMXWCDMA_VAL_UPLINK_SCRAMBLING_TYPE_LONG;
	 uplinkScramblingCode[i] = 0x0;
	 carrierChannelOutput[i].x0 = 0;
     carrierChannelOutput[i].dx = 0;
	 carrierChannelOutput[i].EVM = NULL;
	 carrierChannelOutput[i].constellation = NULL;
	}

	/* Initialize a session */
	RFmxCheckWarn(RFmxWCDMA_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxWCDMA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
	RFmxCheckWarn(RFmxWCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxWCDMA_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay, enableTrigger));
	RFmxCheckWarn(RFmxWCDMA_CfgContiguousCarriers(instrumentHandle, "", NUMBER_OF_CARRIERS, carrierAtCenterFrequency));

	RFmxCheckWarn(RFmxWCDMA_CfgUplinkScramblingArray(instrumentHandle, "", uplinkScramblingType, uplinkScramblingCode, NUMBER_OF_CARRIERS));
	RFmxCheckWarn(RFmxWCDMA_SelectMeasurements(instrumentHandle, "", RFMXWCDMA_VAL_MODACC, RFMXWCDMA_VAL_TRUE));
	RFmxCheckWarn(RFmxWCDMA_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode, measurementOffset, measurementLength));
	RFmxCheckWarn(RFmxWCDMA_Initiate(instrumentHandle, "", ""));

	/* Retrieve results */
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchEVMArray(instrumentHandle, "", timeout, RMSEVM, peakEVM, rho, frequencyError,
		                    chipRateError, RMSMagnitudeError, RMSPhaseError, NUMBER_OF_CARRIERS, NULL));

	RFmxCheckWarn(RFmxWCDMA_ModAccFetchIQImpairmentsArray(instrumentHandle, "", timeout, IQOriginOffset, IQGainImbalance,
		                    IQQuadratureError,  NUMBER_OF_CARRIERS, NULL));
			
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchPeakCDEArray(instrumentHandle, "", timeout, peakCDE, peakCDECode, peakCDEBranch,
		                    NUMBER_OF_CARRIERS, NULL));
	
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchPeakActiveCDEArray(instrumentHandle, "", timeout, peakActiveCDE,
				           peakActiveCDESpreadingFactor, peakActiveCDECode, peakActiveCDEBranch, NUMBER_OF_CARRIERS, NULL));

	RFmxCheckWarn(RFmxWCDMA_ModAccFetchRCDEArray(instrumentHandle, "", timeout, peakRCDE, 
				           peakRCDESpreadingFactor, peakRCDECode, peakRCDEBranch, NUMBER_OF_CARRIERS, NULL));


	for(i=0; i<NUMBER_OF_CARRIERS; i++) 
	{
	  RFmxWCDMA_BuildCarrierString("", i, MAX_SELECTOR_STRING, carrierString);
	  actualArraySize = 0;
	  RFmxCheckWarn(RFmxWCDMA_ModAccFetchEVMTrace(instrumentHandle, carrierString, timeout, NULL, NULL, NULL, 0, &actualArraySize));
	  if( actualArraySize > 0 )
	  {
		 carrierChannelOutput[i].EVM = (float32 *) malloc(sizeof(float32) * actualArraySize);
		 if( carrierChannelOutput[i].EVM )
		 {
			 RFmxCheckWarn(RFmxWCDMA_ModAccFetchEVMTrace(instrumentHandle, carrierString, timeout, &carrierChannelOutput[i].x0, &carrierChannelOutput[i].dx, carrierChannelOutput[i].EVM, actualArraySize, NULL));
		 }
		 else
		 {
			 printf("malloc failed.\n");
			 goto Error;
		 }
	  }

	 actualArraySize = 0;
	 RFmxCheckWarn(RFmxWCDMA_ModAccFetchConstellationTrace(instrumentHandle, carrierString, timeout, NULL, 0, &actualArraySize));
	 if( actualArraySize > 0 )
	 {
		 carrierChannelOutput[i].constellation = (NIComplexSingle *) malloc(sizeof(NIComplexSingle) * actualArraySize);
		 if( carrierChannelOutput[i].constellation )
		 {
			RFmxCheckWarn(RFmxWCDMA_ModAccFetchConstellationTrace(instrumentHandle, carrierString, timeout, carrierChannelOutput[i].constellation, actualArraySize, NULL));
		 }
		 else
		 {
			printf("malloc failed.\n");
			goto Error;
		 }
	 }
	}


	printf("-----------------------------EVM------------------------\n");
	for(i=0; i<NUMBER_OF_CARRIERS; i++)
	{
	 printf("\nCarrier %d\n",i);
	 printf("RMS EVM (%%)                        : %lf\n",RMSEVM[i]);
	 printf("Peak EVM (%%)			   : %lf\n",peakEVM[i]);
	 printf("Rho				   : %lf\n",rho[i]);
	 printf("Frequency Error (Hz)	           : %lf\n",frequencyError[i]);
	 printf("Chip Rate Error (ppm)	           : %lf\n",chipRateError[i]);
	 printf("RMS Magnitude Error (%%)            : %lf\n",RMSMagnitudeError[i]);
	 printf("RMS Phase Error (deg)	           : %lf\n",RMSPhaseError[i]);
	}

	 printf("\n---------------------IQ Impairments------------------\n");
	 for(i=0; i<NUMBER_OF_CARRIERS; i++)
	{
	 printf("\nCarrier %d\n",i);
	 printf("I/Q Origin Offset (dB)             : %lf\n",IQOriginOffset[i]);
	 printf("I/Q Gain Imbalance (dB)            : %lf\n",IQGainImbalance[i]);
	 printf("I/Q Quadrature Error (deg)         : %lf\n",IQQuadratureError[i]);
	}

	printf("\n------------------------Peak CDE---------------------\n");
	for(i=0; i<NUMBER_OF_CARRIERS; i++)
	{
	 printf("\nCarrier %d\n",i);
	 printf("Peak CDE (dB)                      : %lf\n",peakCDE[i]);
	 printf("Peak CDE Code                      : %d\n",peakCDECode[i]);
	 printf("Peak CDE Branch                    : %c\n",peakCDEBranch[i]?'Q':'I');
	}

	printf("\n----------------------Peak Active CDE----------------\n");
	for(i=0; i<NUMBER_OF_CARRIERS; i++)
	{
	 printf("\nCarrier %d\n",i);
	 printf("Peak Active CDE (dB)              : %lf\n",peakActiveCDE[i]);
	 printf("Peak Active CDE Spreading Factor  : %d\n",peakActiveCDESpreadingFactor[i]);
	 printf("Peak Active CDE Code              : %d\n",peakActiveCDECode[i]);
	 printf("Peak Active CDE Branch            : %c\n",peakActiveCDEBranch[i]?'Q':'I');
	}
    printf("\n------------Peak RCDE------------\n");
	for(i=0; i<NUMBER_OF_CARRIERS; i++)
	{
	 printf("\nCarrier %d\n",i);
	 printf("Peak RCDE (dB)                    : %lf\n",peakRCDE[i]);
	 printf("Peak RCDE Spreading Factor        : %d\n",peakRCDESpreadingFactor[i]);
	 printf("Peak RCDE Code                    : %d\n",peakRCDECode[i]);
	 printf("Peak RCDE Branch                  : %c\n",peakRCDEBranch[i]?'Q':'I');
	}



Error:
	if( error )
	{
		RFmxWCDMA_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if (error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}
	if(instrumentHandle)
	{
		RFmxWCDMA_Close(instrumentHandle, RFMXWCDMA_VAL_FALSE);
	}

	/* Free allocated memory */
	for(i=0; i<NUMBER_OF_CARRIERS; i++)
	{
        if (carrierChannelOutput[i].EVM)
        {
            free(carrierChannelOutput[i].EVM);
        }
        if (carrierChannelOutput[i].constellation)
        {
            free(carrierChannelOutput[i].constellation);
        }
	}

	printf("Press any key to exit\n");
	_getch();

	return error;
}
