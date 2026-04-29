//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Configure operating Band.
//7. Configure Duplex Scheme.
//8. Select Downlink as Link Direction.
//9. Configure Downlink Test Model.
//10. Select ModAcc measurement and enable Traces.
//11. Configure Averaging Parameters for ModAcc measurement.
//12. Select Frame as Synchronization Mode and configure Measurement Interval.
//13. Configure EVM Unit.
//14. Initiate the Measurement.
//15[A-E]. Fetch ModAcc Measurements and Traces.
//16. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;

	char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
	int32 error = 0, lastErrorCode = 0;

	char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
	float64 frequency = 10e6;														/*(Hz) */

	float64 centerFrequency = 2.14e9;												/*(Hz) */
	float64 referenceLevel = 0.0;													/*(dBm) */
	float64 externalAttenuation = 0.0;												/*(dB) */

	int32 enableTrigger = RFMXLTE_VAL_FALSE;
	char * digitalEdgeTriggerEdgeTriggerSource = RFMXLTE_VAL_PFI0_STR;
	int32 digitalEdgeTriggerEdge = RFMXLTE_VAL_DIGITAL_EDGE_RISING_EDGE;
	float64 triggerDelay = 0.0;														/*(s) */

	float64 componentCarrierBandwidth = 10e6;                                       /*(Hz) */
	int32 downlinkTestModel = RFMXLTE_VAL_DOWNLINK_TEST_MODEL_TM1_1;

	int32 band = 1;

	int32 linkDirection = RFMXLTE_VAL_LINK_DIRECTION_DOWNLINK;
	int32 duplexScheme = RFMXLTE_VAL_DUPLEX_SCHEME_FDD;
	int32 uplinkDownlinkConfiguraiton = RFMXLTE_VAL_UPLINK_DOWNLINK_CONFIGURATION_0;

	int32 averagingEnabled = RFMXLTE_VAL_MODACC_AVERAGING_ENABLED_FALSE;
	int32 averagingCount = 10;

	int32 synchronizationMode = RFMXLTE_VAL_MODACC_SYNCHRONIZATION_MODE_FRAME;
	int32 measurementOffset = 0;													/*(slots) */
	int32 measurementLength = 1;													/*(slots) */

	int32 evmUnit = RFMXLTE_VAL_MODACC_EVM_UNIT_PERCENTAGE;

	float64 timeout = 10.000000;													/*(s) */
	int32 peakCompositeEVMSubcarrierIndex = 0;
	int32 peakCompositeEVMSymbolIndex = 0;
	float64 meanRMSCompositeEVM = 0.0;												/*(% or dB) */
	float64 maxPeakCompositeEVM = 0.0;												/*(% or dB) */
	float64 meanRMSEVM = 0.0;                                                      /*(% or dB) */
	float64 meanRMSQPSKEVM = 0.0;                                                  /*(% or dB) */
	float64 meanRMS16QAMEVM = 0.0;                                                 /*(% or dB) */
	float64 meanRMS64QAMEVM = 0.0;                                                 /*(% or dB) */
	float64 meanRMS256QAMEVM = 0.0;                                                /*(% or dB) */
   float64 meanRMS1024QAMEVM = 0.0;                                               /*(% or dB) */
	float64 meanFrequencyError = 0.0;												/*(Hz) */
	int32 peakCompositeEVMSlotIndex = 0;

	float64 meanIQOriginOffset = 0.0;												/*(dBc) */
	float64 meanIQGainImbalance = 0.0;												/*(dB) */
	float64 meanIQQuadratureError = 0.0;											/*(deg) */
	NIComplexSingle* QPSKConstellation = NULL;
	NIComplexSingle* QAM16Constellation = NULL;
	NIComplexSingle* QAM64Constellation = NULL;
	NIComplexSingle* QAM256Constellation = NULL;
   NIComplexSingle* QAM1024Constellation = NULL;
	float32* meanRMSEVMPerSubcarrier = NULL;										/*(% or dB) */

	float64 x0 = 0.0, dx = 0.0;

	int32 actualArraySize = 0;
	int32 QPSKConstellationActualArraySize = 0;
	int32 QAM16ConstellationActualArraySize = 0;
	int32 QAM64ConstellationActualArraySize = 0;
	int32 QAM256ConstellationActualArraySize = 0;
   int32 QAM1024ConstellationActualArraySize = 0;

	/* Initialize a session */
	RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
	RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequency));
	RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
	RFmxCheckWarn(RFmxLTE_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeTriggerEdgeTriggerSource, digitalEdgeTriggerEdge, triggerDelay, enableTrigger));	
	RFmxCheckWarn(RFmxLTE_CfgComponentCarrier(instrumentHandle, "", componentCarrierBandwidth, 0.0, 0));
	RFmxCheckWarn(RFmxLTE_CfgBand(instrumentHandle, "", band));
	RFmxCheckWarn(RFmxLTE_CfgDuplexScheme(instrumentHandle, "", duplexScheme, uplinkDownlinkConfiguraiton));
	RFmxCheckWarn(RFmxLTE_CfgLinkDirection(instrumentHandle, "", linkDirection));
	RFmxCheckWarn(RFmxLTE_CfgDownlinkTestModel(instrumentHandle, "", downlinkTestModel));
	RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_MODACC, RFMXLTE_VAL_TRUE));
	RFmxCheckWarn(RFmxLTE_ModAccCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount));
	RFmxCheckWarn(RFmxLTE_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode, measurementOffset, measurementLength));
	RFmxCheckWarn(RFmxLTE_ModAccCfgEVMUnit(instrumentHandle, "", evmUnit));
	RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

	RFmxCheckWarn(RFmxLTE_ModAccFetchCompositeEVM(instrumentHandle, "", timeout,
		&meanRMSCompositeEVM, &maxPeakCompositeEVM,
		&meanFrequencyError, &peakCompositeEVMSymbolIndex,
		&peakCompositeEVMSubcarrierIndex, &peakCompositeEVMSlotIndex));
	RFmxCheckWarn(RFmxLTE_ModAccFetchIQImpairments(instrumentHandle, "", timeout,
		&meanIQOriginOffset, &meanIQGainImbalance, &meanIQQuadratureError));
	RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCHEVM(instrumentHandle, "", timeout, &meanRMSEVM, &meanRMSQPSKEVM,
		&meanRMS16QAMEVM, &meanRMS64QAMEVM , &meanRMS256QAMEVM));
   RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCH1024QAMEVM(instrumentHandle, "", timeout, &meanRMS1024QAMEVM));
	RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCHQPSKConstellation(instrumentHandle,"",timeout,NULL,0,&QPSKConstellationActualArraySize));
	if (QPSKConstellationActualArraySize > 0)
	{
		QPSKConstellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * QPSKConstellationActualArraySize);
		if (QPSKConstellation)
		{
			RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCHQPSKConstellation(instrumentHandle, "", timeout, QPSKConstellation,
				QPSKConstellationActualArraySize,NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCH16QAMConstellation(instrumentHandle, "", timeout,NULL,0,&QAM16ConstellationActualArraySize));
	if (QAM16ConstellationActualArraySize > 0)
	{
		QAM16Constellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * QAM16ConstellationActualArraySize);
		if (QAM16Constellation)
		{
			RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCH16QAMConstellation(instrumentHandle, "", timeout, QAM16Constellation,
				QAM16ConstellationActualArraySize,NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCH64QAMConstellation(instrumentHandle, "", timeout, NULL, 0, &QAM64ConstellationActualArraySize));
	if (QAM64ConstellationActualArraySize > 0)
	{
		QAM64Constellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * QAM64ConstellationActualArraySize);
		if (QAM64Constellation)
		{
			RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCH64QAMConstellation(instrumentHandle, "", timeout, QAM64Constellation,
				QAM64ConstellationActualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCH256QAMConstellation(instrumentHandle, "", timeout, NULL, 0, &QAM256ConstellationActualArraySize));
	if (QAM256ConstellationActualArraySize > 0)
	{
		QAM256Constellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * QAM256ConstellationActualArraySize);
		if (QAM256Constellation)
		{
			RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCH256QAMConstellation(instrumentHandle, "", timeout, QAM256Constellation,
				QAM256ConstellationActualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

   RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCH1024QAMConstellation(instrumentHandle, "", timeout, NULL, 0, &QAM1024ConstellationActualArraySize));
	if (QAM1024ConstellationActualArraySize > 0)
	{
		QAM1024Constellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * QAM1024ConstellationActualArraySize);
		if (QAM1024Constellation)
		{
			RFmxCheckWarn(RFmxLTE_ModAccFetchPDSCH1024QAMConstellation(instrumentHandle, "", timeout, QAM1024Constellation,
				QAM1024ConstellationActualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	actualArraySize = 0;
	RFmxCheckWarn(RFmxLTE_ModAccFetchEVMPerSubcarrierTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
	if( actualArraySize > 0 )
	{
		meanRMSEVMPerSubcarrier = (float32* ) malloc(sizeof(float32) * actualArraySize);
		if( meanRMSEVMPerSubcarrier )
		{
			RFmxCheckWarn(RFmxLTE_ModAccFetchEVMPerSubcarrierTrace(instrumentHandle, "", timeout, &x0, &dx, meanRMSEVMPerSubcarrier, 
				actualArraySize, NULL));
		}
		else
		{
			printf("malloc failed.\n");
			goto Error;
		}
	}

	printf("------------------Measurement------------------\n");
	printf("Mean RMS Composite EVM (%% or dB)     : %lf\n",meanRMSCompositeEVM);
	printf("Mean RMS  EVM  (%% or dB)             : %lf\n",meanRMSEVM);
	printf("Mean RMS QPSK EVM  (%% or dB)         : %lf\n",meanRMSQPSKEVM);
	printf("Mean RMS 16QAM EVM  (%% or dB)        : %lf\n",meanRMS16QAMEVM);
	printf("Mean RMS 64QAM EVM  (%% or dB)        : %lf\n",meanRMS64QAMEVM);
	printf("Mean RMS 256QAM EVM  (%% or dB)       : %lf\n",meanRMS256QAMEVM);
   printf("Mean RMS 1024QAM EVM  (%% or dB)      : %lf\n",meanRMS1024QAMEVM);
	printf("Mean Frequency Error (Hz)            : %lf\n",meanFrequencyError);
	printf("Mean IQ Origin Offset (dBc)          : %lf\n",meanIQOriginOffset);
	printf("Mean IQ Gain Imbalance (dB)          : %lf\n",meanIQGainImbalance);
	printf("Mean IQ Quadrature Error (deg)       : %lf\n",meanIQQuadratureError);

Error:
	if( error )
	{
		RFmxLTE_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
		if (error < 0)
			printf("ERROR: %s\n", errorMessage);
		else
			printf("WARNING: %s\n", errorMessage);
	}

	if(instrumentHandle)
	{
		RFmxLTE_Close(instrumentHandle, RFMXLTE_VAL_FALSE);
	}

	/* Free allocated memory */
    if (QPSKConstellation)
    {
        free(QPSKConstellation);
    }
    if (QAM16Constellation)
    {
        free(QAM16Constellation);
    }
    if (QAM64Constellation)
    {
        free(QAM64Constellation);
    }
    if (QAM256Constellation)
    {
        free(QAM256Constellation);
    }
    if (QAM1024Constellation)
    {
        free(QAM1024Constellation);
    }
    if (meanRMSEVMPerSubcarrier)
    {
        free(meanRMSEVMPerSubcarrier);
    }

	printf("Press any key to exit\n");
	_getch();

	return error;
}
