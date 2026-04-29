//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure UARFCN Band.
//6. Configure Contiguous Carriers.
//7. Configure Uplink Scrambling (Array).
//8. Select ModAcc,ACP,CHP,OBW and SEM measurements and enable Traces.
//9. Configure Synchronization Mode and Interval for ModAcc
//10. Configure Sweep Time Parameters for ACP.
//11. Configure Averaging Parameters for ACP.
//12. Configure Sweep Time Parameters for CHP. 
//13. Configure Averaging Parameters for CHP.
//14. Configure Sweep Time Parameters for OBW. 
//15. Configure Averaging Parameters for OBW.
//16. Configure Sweep Time Parameters for SEM. 
//17. Configure Averaging Parameters for SEM.
//18. Initiate the Measurement.
//19. Fetch ModAcc, ACP, CHP, SEM & OBW Measurements.
//20. Close RFmx Session.  

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096
/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING					256
#define NUMBER_OF_CARRIERS			2

int main (int argc, char *argv[])
{
    //RFSA Configuration
	char *rfsaResourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle = NULL;    

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0;
    int32 lastErrorCode = 0;
	int i = 0;

	char * frequencyReferenceSource =  RFMXWCDMA_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;                                   /*(Hz) */

    float64 centerFrequency = 1.95e9;                                             /*(Hz) */
    float64 referenceLevel = 0.000000;                                            /*(dBm) */
    float64 externalAttenuation = 0.000000;                                       /*(dB) */
    int32 enableTrigger = RFMXWCDMA_VAL_FALSE;
    char * digitalEdgeTriggerEdgeTriggerSource = RFMXWCDMA_VAL_PFI0_STR;
	int32 digitalEdgeTriggerEdge = RFMXWCDMA_VAL_DIGITAL_EDGE_RISING_EDGE; 
    float64 triggerDelay = 0.000000;                                              /*(s) */
    int32 band = 1;
	float64 timeout = 10.0;                                                       /*(s) */

	int32 uplingScramblingCode[NUMBER_OF_CARRIERS] = {0};
	int32 uplingScramblingType[NUMBER_OF_CARRIERS] = {RFMXWCDMA_VAL_UPLINK_SCRAMBLING_TYPE_LONG};
	int32 carrierAtCenterFrequency = -1;

	int32 synchronizationMode = RFMXWCDMA_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;
	int32 measurementOffset = 0;                                                  /* slots */
	int32 measurementLength = 1;                                                  /* slots */

    float64 sweepTimeInterval = 0.000667;                                         /*(s) */
    int32 averagingCount = 10;

    float64 *RMSEVM = NULL;                                                       /* % */
	float64 *peakEVM = NULL;                                                      /* % */
	float64 *rho = NULL;
	float64 *frequencyError = NULL;                                               /* Hz */
	float64 *chipRateError = NULL;                                                /* ppm */
	float64 *RMSMagnitudeError = NULL;                                            /* % */
	float64 *RMSPhaseError = NULL;                                                /* deg */
	int32 ModAccEVMArraySize = 0;

	float64 CHPTotalCarrierPower = 0.0;                                           /*(dBm) */	
	float64 CHPAbsolutePower[NUMBER_OF_CARRIERS] = {0};                           /*(dBm) */
	float64 CHPRelativePower[NUMBER_OF_CARRIERS] = {0};                           /*(dB) */ 

	float64 ACPTotalCarrierPower = 0.0;                                           /*(dBm) */
	float64 ACPAbsolutePower[NUMBER_OF_CARRIERS] = {0};                           /*(dBm) */
	float64 ACPRelativePower[NUMBER_OF_CARRIERS] = {0};                           /*(dB) */ 
	int32 ACPOffsetMeasArraySize = 0;
	float64* ACPLowerAbsolutePower = NULL;                                        /*(dBm) */
    float64* ACPUpperAbsolutePower = NULL;                                        /*(dBm) */
	float64* ACPLowerRelativePower = NULL;                                        /*(dB) */
    float64* ACPUpperRelativePower = NULL;                                        /*(dB) */

	float64 OBWOccupiedBandwidth = 0.0;                                           /*(Hz) */
	float64 OBWAbsolutePower = 0.0;                                               /*(dBm) */
    float64 OBWStopFrequency = 0.0;                                               /*(Hz) */
    float64 OBWStartFrequency = 0.0;                                              /*(Hz) */

	int32 SEMMeasurementStatus = 0;
	float64 SEMTotalCarrierPower = 0.0;                                           /*(dBm) */
	int32 SEMCCMeasArraySize = 0;
	float64 SEMAbsoluteIntegratedPower[NUMBER_OF_CARRIERS] = {0.0};               /*(dBm) */
	float64 SEMRelativeIntegratedPower[NUMBER_OF_CARRIERS] = {0.0};               /*(dB) */ 
	int32 SEMLowerOffsetMeasArraySize = 0, SEMUpperOffsetMeasArraySize = 0;
	int32* SEMLowerOffsetMeasurementStatus = NULL;
    float64* SEMLowerOffsetMargin = NULL;                                         /*(dB) */
    float64* SEMLowerOffsetMarginFrequency = NULL;                                /*(Hz) */		    
	float64* SEMLowerOffsetMarginAbsolutePower = NULL;                            /*(dBm) */
	int32* SEMUpperOffsetMeasurementStatus = NULL;
    float64* SEMUpperOffsetMargin = NULL;                                         /*(dB) */
    float64* SEMUpperOffsetMarginFrequency = NULL;                                /*(Hz) */		    
	float64* SEMUpperOffsetMarginAbsolutePower = NULL;                            /*(dBm) */

    /* Initialize a session */
    RFmxCheckWarn(RFmxWCDMA_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));

    RFmxCheckWarn(RFmxWCDMA_CfgFrequencyReference(instrumentHandle, "",frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxWCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxWCDMA_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeTriggerEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay,
		enableTrigger));
    RFmxCheckWarn(RFmxWCDMA_CfgBand(instrumentHandle, "", band));
    RFmxCheckWarn(RFmxWCDMA_CfgContiguousCarriers(instrumentHandle, "", NUMBER_OF_CARRIERS, carrierAtCenterFrequency));

	RFmxCheckWarn(RFmxWCDMA_CfgUplinkScramblingArray(instrumentHandle, "", uplingScramblingType, uplingScramblingCode, NUMBER_OF_CARRIERS));
	RFmxCheckWarn(RFmxWCDMA_SelectMeasurements(instrumentHandle, "", RFMXWCDMA_VAL_MODACC | RFMXWCDMA_VAL_ACP | RFMXWCDMA_VAL_CHP | 
											 RFMXWCDMA_VAL_OBW | RFMXWCDMA_VAL_SEM,
											 RFMXWCDMA_VAL_TRUE));
    RFmxCheckWarn(RFmxWCDMA_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode, measurementOffset, measurementLength));
    RFmxCheckWarn(RFmxWCDMA_ACPCfgSweepTime(instrumentHandle, "", RFMXWCDMA_VAL_ACP_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
    RFmxCheckWarn(RFmxWCDMA_ACPCfgAveraging(instrumentHandle, "", RFMXWCDMA_VAL_ACP_AVERAGING_ENABLED_FALSE, averagingCount, RFMXWCDMA_VAL_ACP_AVERAGING_TYPE_RMS));
    RFmxCheckWarn(RFmxWCDMA_CHPCfgSweepTime(instrumentHandle, "", RFMXWCDMA_VAL_CHP_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
    RFmxCheckWarn(RFmxWCDMA_CHPCfgAveraging(instrumentHandle, "", RFMXWCDMA_VAL_CHP_AVERAGING_ENABLED_FALSE, averagingCount, RFMXWCDMA_VAL_CHP_AVERAGING_TYPE_RMS));
    RFmxCheckWarn(RFmxWCDMA_OBWCfgSweepTime(instrumentHandle, "", RFMXWCDMA_VAL_OBW_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
    RFmxCheckWarn(RFmxWCDMA_OBWCfgAveraging(instrumentHandle, "", RFMXWCDMA_VAL_OBW_AVERAGING_ENABLED_FALSE, averagingCount, RFMXWCDMA_VAL_OBW_AVERAGING_TYPE_RMS));
    RFmxCheckWarn(RFmxWCDMA_SEMCfgSweepTime(instrumentHandle, "", RFMXWCDMA_VAL_SEM_SWEEP_TIME_AUTO_TRUE, sweepTimeInterval));
    RFmxCheckWarn(RFmxWCDMA_SEMCfgAveraging(instrumentHandle, "", RFMXWCDMA_VAL_SEM_AVERAGING_ENABLED_FALSE, averagingCount, RFMXWCDMA_VAL_SEM_AVERAGING_TYPE_RMS));
    RFmxCheckWarn(RFmxWCDMA_Initiate(instrumentHandle, "", ""));


	//ModAcc
	RFmxCheckWarn(RFmxWCDMA_ModAccFetchEVMArray(instrumentHandle, "", timeout, 
														NULL, NULL, NULL, NULL,
														 NULL, NULL, NULL,0, 
														&ModAccEVMArraySize));
	if( ModAccEVMArraySize > 0 )
	{
		RMSEVM = (float64 *) malloc(sizeof(float64) * ModAccEVMArraySize);
		peakEVM = (float64 *) malloc(sizeof(float64) * ModAccEVMArraySize);
		rho = (float64 *) malloc(sizeof(float64) * ModAccEVMArraySize);
		frequencyError = (float64 *) malloc(sizeof(float64) * ModAccEVMArraySize);	
		chipRateError = (float64 *) malloc(sizeof(float64) * ModAccEVMArraySize);
		RMSMagnitudeError = (float64 *) malloc(sizeof(float64) * ModAccEVMArraySize);
		RMSPhaseError = (float64 *) malloc(sizeof(float64) * ModAccEVMArraySize);

		if(RMSEVM && peakEVM && rho && frequencyError && chipRateError && RMSMagnitudeError && RMSPhaseError)
	        RFmxCheckWarn(RFmxWCDMA_ModAccFetchEVMArray(instrumentHandle, "", timeout, RMSEVM, peakEVM, rho, frequencyError, chipRateError, RMSMagnitudeError,
	                                                                           RMSPhaseError, ModAccEVMArraySize, NULL));
	    else
		 {
			printf("malloc failed.\n");
		 	goto Error;
		 }
	}

	//ACP
    RFmxCheckWarn(RFmxWCDMA_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, 
														NULL, NULL, NULL, NULL, 0, 
														&ACPOffsetMeasArraySize));
	if( ACPOffsetMeasArraySize > 0 )
	{
		ACPLowerRelativePower = (float64 *) malloc(sizeof(float64) * ACPOffsetMeasArraySize);
		ACPUpperRelativePower = (float64 *) malloc(sizeof(float64) * ACPOffsetMeasArraySize);
		ACPLowerAbsolutePower = (float64 *) malloc(sizeof(float64) * ACPOffsetMeasArraySize);
		ACPUpperAbsolutePower = (float64 *) malloc(sizeof(float64) * ACPOffsetMeasArraySize);

		if(ACPLowerRelativePower && ACPUpperRelativePower && ACPLowerAbsolutePower && ACPUpperAbsolutePower)
     		RFmxCheckWarn(RFmxWCDMA_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, 
															ACPLowerRelativePower, ACPUpperRelativePower, 
															ACPLowerAbsolutePower, ACPUpperAbsolutePower,
															ACPOffsetMeasArraySize, NULL));	
		 else
		 {
			printf("malloc failed.\n");
		 	goto Error;
		 }
	}

	RFmxCheckWarn(RFmxWCDMA_ACPFetchCarrierMeasurementArray(instrumentHandle, "", timeout,
															 ACPAbsolutePower, 
															 ACPRelativePower,
															 NUMBER_OF_CARRIERS, NULL));	


	//CHP
	RFmxCheckWarn(RFmxWCDMA_CHPFetchCarrierMeasurementArray(instrumentHandle, "", timeout,
															 CHPAbsolutePower, 
															 CHPRelativePower,
															 NUMBER_OF_CARRIERS, NULL));	
	//SEM
	RFmxCheckWarn(RFmxWCDMA_SEMFetchCarrierMeasurementArray(instrumentHandle, "", timeout,
															 SEMAbsoluteIntegratedPower, 
															 SEMRelativeIntegratedPower,
															 NUMBER_OF_CARRIERS, NULL));



	 
	RFmxCheckWarn(RFmxWCDMA_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, 
															NULL, NULL, NULL, 0, &SEMLowerOffsetMeasArraySize 
															));
	if( SEMLowerOffsetMeasArraySize > 0 )
	{
		SEMLowerOffsetMeasurementStatus = (int32 *) malloc(sizeof(int32) * SEMLowerOffsetMeasArraySize);
		SEMLowerOffsetMargin = (float64 *) malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
		SEMLowerOffsetMarginFrequency = (float64 *) malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);
		SEMLowerOffsetMarginAbsolutePower = (float64 *) malloc(sizeof(float64) * SEMLowerOffsetMeasArraySize);	

		if(SEMLowerOffsetMeasurementStatus && SEMLowerOffsetMargin && SEMLowerOffsetMarginFrequency && SEMLowerOffsetMarginAbsolutePower)
		   RFmxCheckWarn(RFmxWCDMA_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout,
															SEMLowerOffsetMeasurementStatus, 
															SEMLowerOffsetMargin, 
															SEMLowerOffsetMarginFrequency,
															SEMLowerOffsetMarginAbsolutePower,
															NULL,
															SEMLowerOffsetMeasArraySize, NULL));
		 else
		 {
			printf("malloc failed.\n");
		 	goto Error;
		 }
	}	
		
	RFmxCheckWarn(RFmxWCDMA_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, 
															NULL, NULL, NULL, 0, &SEMUpperOffsetMeasArraySize));
	if( SEMUpperOffsetMeasArraySize > 0 )
	{
		SEMUpperOffsetMeasurementStatus = (int32 *) malloc(sizeof(int32) * SEMUpperOffsetMeasArraySize);
		SEMUpperOffsetMargin = (float64 *) malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
		SEMUpperOffsetMarginFrequency = (float64 *) malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);
		SEMUpperOffsetMarginAbsolutePower = (float64 *) malloc(sizeof(float64) * SEMUpperOffsetMeasArraySize);		

		if(SEMUpperOffsetMeasurementStatus && SEMUpperOffsetMargin && SEMUpperOffsetMarginFrequency && SEMUpperOffsetMarginAbsolutePower)
		RFmxCheckWarn(RFmxWCDMA_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout,
															 SEMUpperOffsetMeasurementStatus, 
															 SEMUpperOffsetMargin, 
															 SEMUpperOffsetMarginFrequency,
															 SEMUpperOffsetMarginAbsolutePower,
															 NULL,
															 SEMUpperOffsetMeasArraySize, NULL));	
		 else
		 {
			printf("malloc failed.\n");
		 	goto Error;
		 }
	}	

    
	RFmxCheckWarn(RFmxWCDMA_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &SEMMeasurementStatus));
    
	RFmxCheckWarn(RFmxWCDMA_ACPFetchTotalCarrierPower(instrumentHandle, "", timeout, &ACPTotalCarrierPower));

	RFmxCheckWarn(RFmxWCDMA_CHPFetchTotalCarrierPower(instrumentHandle, "", timeout, &CHPTotalCarrierPower));

	//OBW
	RFmxCheckWarn(RFmxWCDMA_OBWFetchMeasurement(instrumentHandle, "", timeout, &OBWOccupiedBandwidth, &OBWAbsolutePower
		, &OBWStartFrequency, &OBWStopFrequency));


	RFmxCheckWarn(RFmxWCDMA_SEMFetchTotalCarrierPower(instrumentHandle, "", timeout, &SEMTotalCarrierPower));
    

    printf("\n************************* ModAcc *************************\n\n");
	for(i=0;i<ModAccEVMArraySize;i++)
	{
		printf("\nMeasurement %d\n",i); 
		printf("RMS EVM  (%%)                     : %lf\n", RMSEVM[i]);
		printf("Peak EVM  (%%)                    : %lf\n", peakEVM[i]);	
		printf("Rho                              : %lf\n", rho[i]);
		printf("Frequency Error (Hz)             : %lf\n", frequencyError[i]);	
		printf("Chip Rate Error  (ppm)           : %lf\n", chipRateError[i]);	
		printf("RMS Magnitude Error (%%)          : %lf\n", RMSMagnitudeError[i]);
		printf("RMS Phase Error (deg)            : %lf\n", RMSPhaseError[i]);
	}

	printf("\n************************* ACP *************************\n\n");
	printf("Total Carrier Power  (dBm)	 : %lf\n", ACPTotalCarrierPower);
	printf("\nCarrier Measurements	         : \n");
	for(i=0;i<NUMBER_OF_CARRIERS;i++)
	{
		printf("\nCarrier %d\n",i); 
		printf("Absolute Power  (dBm)            : %lf\n", ACPAbsolutePower[i]);
		printf("Relative Power  (dB)             : %lf\n", ACPRelativePower[i]);	
	}
	printf("\nOffset Channel Measurements      : \n");
	for(i=0;i<ACPOffsetMeasArraySize;i++)
	{
		printf("\nOffset %d\n", i);
		printf("Lower Relative Power (dB)        : %lf\n", ACPLowerRelativePower[i]);
		printf("Upper Relative Power (dB)        : %lf\n", ACPUpperRelativePower[i]);
		printf("Lower Absolute Power (dBm)       : %lf\n", ACPLowerAbsolutePower[i]);
		printf("Upper Absolute Power (dBm)       : %lf\n", ACPUpperAbsolutePower[i]);		
	}

    printf("\n************************* CHP *************************\n\n");
	printf("Total Carrier Power  (dBm)       : %lf\n", CHPTotalCarrierPower);
	printf("\nCarrier Measurements             : \n");
	for(i=0;i<NUMBER_OF_CARRIERS;i++)
	{		
		printf("\nCarrier %d\n",i); 
		printf("Absolute Power  (dBm)            : %lf\n", CHPAbsolutePower[i]);
		printf("Relative Power  (dB)             : %lf\n", CHPRelativePower[i]);	
	}

	printf("\n************************* OBW *************************\n\n");
	printf("Occupied Bandwidth  (Hz)	 : %lf\n", OBWOccupiedBandwidth);
	printf("Absolute Power  (dBm)		 : %lf\n", OBWAbsolutePower);    
    printf("Start Frequency  (Hz)		 : %lf\n", OBWStartFrequency);
	printf("Stop Frequency  (Hz)		 : %lf\n", OBWStopFrequency);

	printf("\n************************* SEM *************************\n\n");
	printf("Measurement Status               : %s\n", (SEMMeasurementStatus)? "PASS" : "FAIL");
	printf("Total Carrier Power  (dBm)	 : %lf\n", SEMTotalCarrierPower);	
	printf("\nCarrier Measurements	         : \n");
	for(i=0;i<SEMCCMeasArraySize;i++)
	{
		printf("\nCarrier %d\n",i); 
		printf("Absolute Integrated Power  (dBm)            : %lf\n", SEMAbsoluteIntegratedPower[i]);
		printf("Relative Integrated Power  (dB)             : %lf\n", SEMRelativeIntegratedPower[i]);	
	}
	printf("\nLower Offset Segment Measurements: \n");
	for(i=0;i<SEMLowerOffsetMeasArraySize;i++)
	{
		printf("\nOffset %d\n",i);
		printf("Margin  (dB)                     : %lf\n",SEMLowerOffsetMargin[i]);
		printf("Margin Absolute Power  (dBm)     : %lf\n",SEMLowerOffsetMarginAbsolutePower[i]);		
		printf("Margin Frequency  (Hz)           : %lf\n",SEMLowerOffsetMarginFrequency[i]);
		printf("Measurement Status               : %s\n", 
			  (SEMLowerOffsetMeasurementStatus[i])? "PASS" : "FAIL");		
	}	
	printf("\nUpper Offset Segment Measurements: \n");
	for(i=0;i<SEMUpperOffsetMeasArraySize;i++)
	{
		printf("\nOffset %d\n",i);
		printf("Margin  (dB)                     : %lf\n",SEMUpperOffsetMargin[i]);
		printf("Margin Absolute Power  (dBm)     : %lf\n",SEMUpperOffsetMarginAbsolutePower[i]);	
		printf("Margin Frequency  (Hz)           : %lf\n",SEMUpperOffsetMarginFrequency[i]);
		printf("Measurement Status               : %s\n", 
			  (SEMUpperOffsetMeasurementStatus[i])? "PASS" : "FAIL");
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
    printf("Press any key to exit\n");
    _getch();

	/* Free allocated memory */
    if (RMSEVM)
    {
        free(RMSEVM);
    }
    if (peakEVM)
    {
        free(peakEVM);
    }
    if (rho)
    {
        free(rho);
    }
    if (frequencyError)
    {
        free(frequencyError);
    }
    if (chipRateError)
    {
        free(chipRateError);
    }
    if (RMSMagnitudeError)
    {
        free(RMSMagnitudeError);
    }
    if (RMSPhaseError)
    {
        free(RMSPhaseError);
    }
    if (ACPLowerAbsolutePower)
    {
        free(ACPLowerAbsolutePower);
    }
    if (ACPUpperAbsolutePower)
    {
        free(ACPUpperAbsolutePower);
    }
    if (ACPLowerRelativePower)
    {
        free(ACPLowerRelativePower);
    }
    if (ACPUpperRelativePower)
    {
        free(ACPUpperRelativePower);
    }
    if (SEMLowerOffsetMeasurementStatus)
    {
        free(SEMLowerOffsetMeasurementStatus);
    }
    if (SEMLowerOffsetMargin)
    {
        free(SEMLowerOffsetMargin);
    }
    if (SEMLowerOffsetMarginFrequency)
    {
        free(SEMLowerOffsetMarginFrequency);
    }
    if (SEMLowerOffsetMarginAbsolutePower)
    {
        free(SEMLowerOffsetMarginAbsolutePower);
    }
    if (SEMUpperOffsetMeasurementStatus)
    {
        free(SEMUpperOffsetMeasurementStatus);
    }
    if (SEMUpperOffsetMargin)
    {
        free(SEMUpperOffsetMargin);
    }
    if (SEMUpperOffsetMarginFrequency)
    {
        free(SEMUpperOffsetMarginFrequency);
    }
    if (SEMUpperOffsetMarginAbsolutePower)
    {
        free(SEMUpperOffsetMarginAbsolutePower);
    }

    return error;
}
