//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select ModAcc, ACP, CHP, OBW and SEM and enable traces.
//6. Configure Synchronization Mode and Measurement Interval, Uplink Scrambling Code and Midamble for ModAcc.
//7. Configure Sweep Time and Averaging Parameters for ACP.
//8. Configure Sweep Time and Averaging Parameters for CHP.
//9. Configure Sweep Time and Averaging Parameters for OBW.
//10. Configure Sweep Time and Averaging Parameters for SEM.
//11. Initiate the Measurement.
//12. Fetch SEM Measurements.
//13. Fetch OBW Measurements.
//14. Fetch CHP Measurements.
//15. Fetch ACP Measurements.
//16. Fetch ModAcc Measurements.
//17. Close the RFmx session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxTDSCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096
#define NUMBER_OF_OFFSETS			2
#define MAX_NUMBER_OF_USERS         16

int main ()
{
    //RFSA Configuration
    char *rfsaResourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;
    
    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0;
    int32 lastErrorCode = 0;
    float64 centerFrequency = 1.91e+9;                        /*(Hz) */
    float64 referenceLevel = 0.00;                            /*(dBm) */
    float64 externalAttenuation = 0.00;                       /*(dB) */

    /*Trigger*/
    float64 triggerDelay = 0.00;                              /*(s) */
    int32 enableTrigger = RFMXTDSCDMA_VAL_TRUE;
    char * IQPowerEdgeSource = "0";
    int32 IQPowerEdgeSlope = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_RISING_SLOPE;
    float64 IQPowerEdgeLevel = -20.00;                        /*(dB) */
    int32 minimumQuietTimeMode = RFMXTDSCDMA_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
    float64 minimumQuietTime = 80E-6;                         /*(s) */
    int32 IQPowerEdgeLevelType = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;

    /*ModAcc Measurement Settings*/
    int32 midambleAutoDetectionMode = RFMXTDSCDMA_VAL_MIDAMBLE_AUTO_DETECTION_MODE_MIDAMBLE_SHIFT;
    int32 maximumNumberOfUsers = MAX_NUMBER_OF_USERS;
    int32 midambleShift = 8;                                  /*(chips) */
    int32 synchronizationMode = RFMXTDSCDMA_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;
    int32 measurementOffset = 0;                              /*(slots) */
    int32 measurementLength = 1;                              /*(slots) */
    int32 uplinkScramblingCode = 0;

    /*Averaging*/
    int32 averagingEnabled = RFMXTDSCDMA_VAL_ACP_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;
    int32 averagingType = RFMXTDSCDMA_VAL_ACP_AVERAGING_TYPE_RMS;

    /*Frequency Reference*/
    char * frequencyReferenceSource = RFMXTDSCDMA_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e+6;                /*(Hz) */

    int32 sweepTimeAuto = RFMXTDSCDMA_VAL_SEM_SWEEP_TIME_AUTO_TRUE;
    float64 sweepTimeInterval = 0.000660;                       /*(s) */

    float64 timeout = 10.00;

    /*Variables to Store Results*/
    float64 ACPCarrierAbsolutePower = 0.00;                     /*(dBm) */
    float64 SEMCarrierAbsoluteIntegratedPower = 0.00;           /*(dBm) */
    float64 CHPCarrierAbsolutePower = 0.00;                     /*(dBm) */

    float64 occupiedBandwidth = 0.00;                           /*(Hz) */
    float64 absolutePower = 0.00;                               /*(dBm) */
    float64 startFrequency = 0.00;                              /*(Hz) */
    float64 stopFrequency = 0.00;                               /*(Hz) */
    float64 chipRateError = 0.00;                               /*(ppm) */
    float64 frequencyError = 0.00;                              /*(Hz) */
    float64 RMSCompositeEVM = 0.00;                             /*(%) */
    float64 peakCompositeEVM = 0.00;                            /*(%) */
    float64 compositeRho = 0.00;
    float64 RMSCompositePhaseError = 0.00;                      /*(deg) */
    float64 RMSCompositeMagnitudeError = 0.00;                  /*(%) */
    int32 i = 0;

    int32 measurementStatus = 0;
    int32 lowerOffsetMarginArraySize = 0;
    int32 upperOffsetMarginArraySize = 0;

    int32* lowerOffsetMeasurementStatus = NULL;
    float64* lowerOffsetMargin = NULL;                          /*(dB) */
    float64* lowerOffsetMarginFrequency = NULL;                 /*(Hz) */
    float64* lowerOffsetMarginAbsolutePower = NULL;             /*(dBm) */
    float64* lowerOffsetMarginRelativePower = NULL;             /*(dB) */

    int32* upperOffsetMeasurementStatus = NULL;
    float64* upperOffsetMargin = NULL;                          /*(dB) */
    float64* upperOffsetMarginFrequency = NULL;                 /*(Hz) */
    float64* upperOffsetMarginAbsolutePower = NULL;             /*(dBm) */
    float64* upperOffsetMarginRelativePower = NULL;             /*(dB) */

	float64 lowerAbsolutePower[NUMBER_OF_OFFSETS] = {0};
	float64 upperAbsolutePower[NUMBER_OF_OFFSETS] = {0};
	float64 lowerRelativePower[NUMBER_OF_OFFSETS] = {0};
	float64 upperRelativePower[NUMBER_OF_OFFSETS] = {0};

    /* Initialize a session */
    RFmxCheckWarn(RFmxTDSCDMA_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));     
    RFmxCheckWarn(RFmxTDSCDMA_CfgFrequencyReference(instrumentHandle, "",frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxTDSCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxTDSCDMA_CfgIQPowerEdgeTrigger(instrumentHandle, "", IQPowerEdgeSource, IQPowerEdgeSlope,
                                                        IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode,
														minimumQuietTime, IQPowerEdgeLevelType, enableTrigger));
    RFmxCheckWarn(RFmxTDSCDMA_SelectMeasurements(instrumentHandle, "", RFMXTDSCDMA_VAL_MODACC|RFMXTDSCDMA_VAL_SEM|
                                                        RFMXTDSCDMA_VAL_ACP|RFMXTDSCDMA_VAL_CHP|RFMXTDSCDMA_VAL_OBW,
														RFMXTDSCDMA_VAL_TRUE));
   RFmxCheckWarn(RFmxTDSCDMA_CfgMidambleShift(instrumentHandle, "",midambleAutoDetectionMode, maximumNumberOfUsers, midambleShift));
    RFmxCheckWarn(RFmxTDSCDMA_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode,
														measurementOffset, measurementLength));
    RFmxCheckWarn(RFmxTDSCDMA_CfgUplinkScramblingCode(instrumentHandle, "", uplinkScramblingCode));
    RFmxCheckWarn(RFmxTDSCDMA_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
    RFmxCheckWarn(RFmxTDSCDMA_ACPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
    RFmxCheckWarn(RFmxTDSCDMA_CHPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
    RFmxCheckWarn(RFmxTDSCDMA_CHPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
    RFmxCheckWarn(RFmxTDSCDMA_OBWCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
    RFmxCheckWarn(RFmxTDSCDMA_OBWCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
    RFmxCheckWarn(RFmxTDSCDMA_SEMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
    RFmxCheckWarn(RFmxTDSCDMA_SEMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
    RFmxCheckWarn(RFmxTDSCDMA_Initiate(instrumentHandle, "", ""));

    RFmxCheckWarn(RFmxTDSCDMA_ModAccFetchCompositeEVM(instrumentHandle, "", timeout,
														 &RMSCompositeEVM, &peakCompositeEVM,  &compositeRho,                                              
														 &frequencyError, &chipRateError, 
                                                         &RMSCompositeMagnitudeError, &RMSCompositePhaseError));

    /* Fetch the measurements array */
    RFmxCheckWarn(RFmxTDSCDMA_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout, 
                                                                lowerRelativePower, upperRelativePower, 
                                                                lowerAbsolutePower, upperAbsolutePower,
                                                                NUMBER_OF_OFFSETS, NULL));
    RFmxCheckWarn(RFmxTDSCDMA_ACPFetchCarrierAbsolutePower(instrumentHandle, "", timeout, &ACPCarrierAbsolutePower));

    RFmxCheckWarn(RFmxTDSCDMA_CHPFetchCarrierAbsolutePower(instrumentHandle, "", timeout, &CHPCarrierAbsolutePower));
    
    RFmxCheckWarn(RFmxTDSCDMA_OBWFetchMeasurement(instrumentHandle, "", timeout, &occupiedBandwidth, &absolutePower,
																&startFrequency, &stopFrequency));

    RFmxCheckWarn(RFmxTDSCDMA_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, 0,
                                                                &lowerOffsetMarginArraySize));
    if( lowerOffsetMarginArraySize > 0 )
    {
        lowerOffsetMeasurementStatus = (int32 *) malloc(sizeof(int32) * lowerOffsetMarginArraySize);
        lowerOffsetMargin = (float64 *) malloc(sizeof(float64) * lowerOffsetMarginArraySize);
        lowerOffsetMarginFrequency = (float64 *) malloc(sizeof(float64) * lowerOffsetMarginArraySize);
        lowerOffsetMarginAbsolutePower = (float64 *) malloc(sizeof(float64) * lowerOffsetMarginArraySize);
        lowerOffsetMarginRelativePower = (float64 *) malloc(sizeof(float64) * lowerOffsetMarginArraySize);
        if( lowerOffsetMeasurementStatus && lowerOffsetMargin && lowerOffsetMarginFrequency &&
            lowerOffsetMarginAbsolutePower && lowerOffsetMarginRelativePower )
        {
            RFmxCheckWarn(RFmxTDSCDMA_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout,
                                                                        lowerOffsetMeasurementStatus, lowerOffsetMargin,
                                                                        lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower,
                                                                        lowerOffsetMarginRelativePower, lowerOffsetMarginArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    RFmxCheckWarn(RFmxTDSCDMA_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, 0,
                                                                &upperOffsetMarginArraySize));
    if( upperOffsetMarginArraySize > 0 )
    {
        upperOffsetMeasurementStatus = (int32 *) malloc(sizeof(int32) * upperOffsetMarginArraySize);
        upperOffsetMargin = (float64 *) malloc(sizeof(float64) * upperOffsetMarginArraySize);
        upperOffsetMarginFrequency = (float64 *) malloc(sizeof(float64) * upperOffsetMarginArraySize);
        upperOffsetMarginAbsolutePower = (float64 *) malloc(sizeof(float64) * upperOffsetMarginArraySize);
        upperOffsetMarginRelativePower = (float64 *) malloc(sizeof(float64) * upperOffsetMarginArraySize);
        if( upperOffsetMeasurementStatus && upperOffsetMargin && upperOffsetMarginFrequency &&
            upperOffsetMarginAbsolutePower && upperOffsetMarginRelativePower )
        {
            RFmxCheckWarn(RFmxTDSCDMA_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout,
                                                                        upperOffsetMeasurementStatus, upperOffsetMargin,
                                                                        upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower,
                                                                        upperOffsetMarginRelativePower, upperOffsetMarginArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    RFmxCheckWarn(RFmxTDSCDMA_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));
    RFmxCheckWarn(RFmxTDSCDMA_SEMFetchCarrierAbsoluteIntegratedPower(instrumentHandle, "", timeout, &SEMCarrierAbsoluteIntegratedPower));;

    printf("\nModAcc Results:\n");
    printf("Rms Composite EVM  (%%)                 : %lf\n",RMSCompositeEVM);
    printf("Peak Composite EVM  (%%)                : %lf\n",peakCompositeEVM);
    printf("Composite Rho                          : %lf\n",compositeRho);
    printf("Frequency Error  (Hz)                  : %lf\n",frequencyError);
    printf("Chip Rate Error  (ppm)                 : %lf\n",chipRateError);
    printf("Rms Composite Phase Error  (deg)       : %lf\n",RMSCompositePhaseError);
    printf("Rms Composite Magnitude Error (%%)      : %lf\n",RMSCompositeMagnitudeError);

    printf("\nACP Results:\n");
    printf("Carrier Absolute Power  (dBm)          : %lf\n",ACPCarrierAbsolutePower);
    printf("-------------------------------------------------\n");
    for(i=0;i<NUMBER_OF_OFFSETS;i++)
    {
        printf("Offset                                 : %d\n", i);
        printf("Lower Relative Power (dB)              : %f\n", lowerRelativePower[i]);
        printf("Upper Relative Power (dB)              : %f\n", upperRelativePower[i]);
        printf("Lower Absolute Power (dBm)             : %f\n", lowerAbsolutePower[i]);
        printf("Upper Absolute Power (dBm)             : %f\n", upperAbsolutePower[i]);
        printf("-------------------------------------------------\n");
    }

    printf("\nCHP Results:\n");
    printf("Carrier Absolute Power(dBm)            : %lf\n",CHPCarrierAbsolutePower);
												   
    printf("\nOBW Results:\n");					   
    printf("Occupied Bandwidth (Hz)                : %lf\n",occupiedBandwidth);
    printf("Absolute Power (dBm)                   : %lf\n",absolutePower);
    printf("Start Frequency  (Hz)                  : %lf\n",startFrequency);
    printf("Stop Frequency  (Hz)                   : %lf\n",stopFrequency);

    printf("\nSEM Results\n");
    printf("Measurement Status                     : %s\n",(measurementStatus)?"Pass":"Fail");
    printf("Carrier Absolute Integrated Power (dBm): %lf\n",SEMCarrierAbsoluteIntegratedPower);

    printf("\n---------------Lower Offset---------------\n");
    for( i = 0; i < lowerOffsetMarginArraySize; i++ )
    {
        printf("\nLower Offset Segment Measurements      : %d\n", i);
        printf("Margin (dB)                            : %lf\n",lowerOffsetMargin[i]);
        printf("Margin Absolute Power (dBm)            : %lf\n",lowerOffsetMarginAbsolutePower[i]);
        printf("Margin Relative Power (dB)             : %lf\n",lowerOffsetMarginRelativePower[i]);
        printf("Margin Frequency (Hz)                  : %lf\n",lowerOffsetMarginFrequency[i]);
        printf("Measurement Status                     : %s\n",(lowerOffsetMeasurementStatus[i])?"Pass":"Fail");
    }

    printf("\n---------------Upper Offset---------------\n");
    for( i = 0; i < upperOffsetMarginArraySize; i++ )
    {
        printf("\nUpper Offset Segment Measurements      : %d\n", i);
        printf("Margin (dB)                            : %lf\n",upperOffsetMargin[i]);
        printf("Margin Absolute Power (dBm)            : %lf\n",upperOffsetMarginAbsolutePower[i]);
        printf("Margin Relative Power (dB)             : %lf\n",upperOffsetMarginRelativePower[i]);
        printf("Margin Frequency (Hz)                  : %lf\n",upperOffsetMarginFrequency[i]);
        printf("Measurement Status                     : %s\n",(upperOffsetMeasurementStatus[i])?"Pass":"Fail");
    }

Error:
    if( error )
    {
        RFmxTDSCDMA_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
        if (error < 0)
            printf("ERROR: %s\n", errorMessage);
        else
            printf("WARNING: %s\n", errorMessage);
    }
    if(instrumentHandle)
    {
        RFmxTDSCDMA_Close(instrumentHandle, RFMXTDSCDMA_VAL_FALSE);
    }

    if (lowerOffsetMeasurementStatus) 
    {
        free(lowerOffsetMeasurementStatus);
    }
    if (lowerOffsetMargin) 
    {
        free(lowerOffsetMargin);
    }
    if (lowerOffsetMarginFrequency) 
    {
        free(lowerOffsetMarginFrequency);
    }
    if (lowerOffsetMarginAbsolutePower) 
    {
        free(lowerOffsetMarginAbsolutePower);
    }
    if (lowerOffsetMarginRelativePower) 
    {
        free(lowerOffsetMarginRelativePower);
    }
    if (upperOffsetMeasurementStatus) 
    {
        free(upperOffsetMeasurementStatus);
    }
    if (upperOffsetMargin) 
    {
        free(upperOffsetMargin);
    }
    if (upperOffsetMarginFrequency) 
    {
        free(upperOffsetMarginFrequency);
    }
    if (upperOffsetMarginAbsolutePower) 
    {
        free(upperOffsetMarginAbsolutePower);
    }
    if (upperOffsetMarginRelativePower) 
    {
        free(upperOffsetMarginRelativePower);
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}
