//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Select SEM measurement and enable traces.
//6. Configure Sweep Time Parameters.
//7. Configure Averaging Parameters.
//8. Initiate the Measurement.
//9. Fetch SEM Measurements and Traces.
//10. Close the RFmx session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxTDSCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main ()
{
    //RFSA Configuration
    char *rfsaResourceName = "RFSA";
	niRFmxInstrHandle instrumentHandle = NULL;
    
    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0;
    int32 lastErrorCode = 0;
    float64 centerFrequency = 1.91e+9;          /*(Hz) */
    float64 referenceLevel = 0.000000;          /*(dBm) */
    float64 externalAttenuation = 0.000000;     /*(dB) */
    float64 triggerDelay = 0.000000;            /*(s) */

    /*Frequency Reference*/
    char * frequencyReferenceSource = RFMXTDSCDMA_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10E+6;                  /*(Hz) */

    /*Trigger*/
    int32 enableTrigger = RFMXTDSCDMA_VAL_TRUE;
    char * IQPowerEdgeSource = "0";
    int32 IQPowerEdgeSlope = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_RISING_SLOPE;
    float64 IQPowerEdgeLevel = -20.000000;      /*(dB) */
    int32 minimumQuietTimeMode = RFMXTDSCDMA_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
    float64 minimumQuietTime = 16E-6;           /*(s) */
    int32 IQPowerEdgeLevelType = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;

    /*Sweep Time*/
    int32 sweepTimeAuto = RFMXTDSCDMA_VAL_SEM_SWEEP_TIME_AUTO_TRUE;
    float64 sweepTimeInterval = 660E-6;         /*(s) */

    /*Averaging*/
    int32 averagingEnabled = RFMXTDSCDMA_VAL_SEM_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;
    int32 averagingType = RFMXTDSCDMA_VAL_SEM_AVERAGING_TYPE_RMS;

    float64 timeout = 10.000000;                /*(s) */

    /* Variables to store the results */
    int32 measurementStatus = RFMXTDSCDMA_VAL_SEM_MEASUREMENT_STATUS_FAIL;
    int32 actualArraySize = 0;
    float64 x0 = 0, dx = 0;
    int32 lowerOffsetMarginArraySize = 0, upperOffsetMarginArraySize = 0;
    float64 carrierAbsoluteIntegratedPower = 0.000000;      /*(dBm) */
    float32* spectrum = NULL;                               /*(dBm) */
    float32* absoluteMask = NULL;                           /*(dBm) */
    float32* relativeMask = NULL;                           /*(dBm) */
    int32* lowerOffsetMeasurementStatus = NULL;
    float64* lowerOffsetMargin = NULL;                      /*(dB) */
    float64* lowerOffsetMarginFrequency = NULL;             /*(Hz) */
    float64* lowerOffsetMarginAbsolutePower = NULL;         /*(dBm) */
    float64* lowerOffsetMarginRelativePower = NULL;         /*(dB) */
    int32 i = 0;
    int32* upperOffsetMeasurementStatus = NULL;
    float64* upperOffsetMargin = NULL;                      /*(dB) */
    float64* upperOffsetMarginFrequency = NULL;             /*(Hz) */
    float64* upperOffsetMarginAbsolutePower = NULL;         /*(dBm) */
    float64* upperOffsetMarginRelativePower = NULL;         /*(dB) */

    /* Initialize a session */
    RFmxCheckWarn(RFmxTDSCDMA_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxTDSCDMA_CfgFrequencyReference(instrumentHandle, "",frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxTDSCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxTDSCDMA_CfgIQPowerEdgeTrigger(instrumentHandle, "", IQPowerEdgeSource, IQPowerEdgeSlope,
                                                        IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode,
                                                        minimumQuietTime, IQPowerEdgeLevelType, enableTrigger));
    RFmxCheckWarn(RFmxTDSCDMA_SelectMeasurements(instrumentHandle, "", RFMXTDSCDMA_VAL_SEM, RFMXTDSCDMA_VAL_TRUE));
    RFmxCheckWarn(RFmxTDSCDMA_SEMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
    RFmxCheckWarn(RFmxTDSCDMA_SEMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
    RFmxCheckWarn(RFmxTDSCDMA_Initiate(instrumentHandle, "", ""));

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
    RFmxCheckWarn(RFmxTDSCDMA_SEMFetchCarrierAbsoluteIntegratedPower(instrumentHandle, "", timeout, &carrierAbsoluteIntegratedPower));

    RFmxCheckWarn(RFmxTDSCDMA_SEMFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, 0, &actualArraySize));
    if(actualArraySize)
    {
        spectrum = (float32*)malloc(sizeof(float32)*actualArraySize);
        relativeMask = (float32*)malloc(sizeof(float32)*actualArraySize);
        absoluteMask = (float32*)malloc(sizeof(float32)*actualArraySize);
        if(spectrum)
        {
            RFmxCheckWarn(RFmxTDSCDMA_SEMFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, relativeMask, absoluteMask,
                                                                actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }
    
    printf("Measurement Status                  : %s",(measurementStatus)?"Pass":"Fail");
    printf("\nCarrier Absolute Power (dBm)        : %lf",carrierAbsoluteIntegratedPower);

    printf("\n---------------Lower Offset---------------\n");
    for( i = 0; i < lowerOffsetMarginArraySize; i++ )
    {
        printf("\nLower Offset Segment Measurements   : %d\n", i);
        printf("Margin (dB)                         : %lf\n",lowerOffsetMargin[i]);
        printf("Margin Absolute Power (dBm)         : %lf\n",lowerOffsetMarginAbsolutePower[i]);
        printf("Margin Relative Power (dB)          : %lf\n",lowerOffsetMarginRelativePower[i]);
        printf("Margin Frequency (Hz)               : %lf\n",lowerOffsetMarginFrequency[i]);
        printf("Measurement Status                  : %s\n",(lowerOffsetMeasurementStatus[i])?"Pass":"Fail");
    }

    printf("\n---------------Upper Offset---------------\n");
    for( i = 0; i < upperOffsetMarginArraySize; i++ )
    {
        printf("\nUpper Offset Segment Measurements   : %d\n", i);
        printf("Margin (dB)                         : %lf\n",upperOffsetMargin[i]);
        printf("Margin Absolute Power (dBm)         : %lf\n",upperOffsetMarginAbsolutePower[i]);
        printf("Margin Relative Power (dB)          : %lf\n",upperOffsetMarginRelativePower[i]);
        printf("Margin Frequency (Hz)               : %lf\n",upperOffsetMarginFrequency[i]);
        printf("Measurement Status                  : %s\n",(upperOffsetMeasurementStatus[i])?"Pass":"Fail");
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
        RFmxTDSCDMA_Close(instrumentHandle, RFMXTDSCDMA_VAL_TRUE);
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
    if (spectrum)
    {
        free(spectrum);
    }
    if (relativeMask)
    {
        free(relativeMask);
    }
    if (absoluteMask)
    {
        free(absoluteMask);
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}
