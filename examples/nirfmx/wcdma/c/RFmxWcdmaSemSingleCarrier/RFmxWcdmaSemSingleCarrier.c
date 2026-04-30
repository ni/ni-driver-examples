// Steps:
// 1. Open a new RFmx Session.
// 2. Configure Frequency Reference.
// 3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
// 4. Configure Trigger Type and Trigger Parameters.
// 5. Configure UARFCN Band.
// 6. Select SEM measurement and enable Traces.
// 7. Configure Sweep Time Parameters.
// 8. Configure Averaging Parameters for SEM measurement.
// 9. Initiate the Measurement.
// 10. Fetch SEM Measurements and Traces.
// 11. Close RFmx Session. 

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWCDMA.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main (int argc, char *argv[])
{
	char *resourceName = "RFSA";
    niRFmxInstrHandle instrumentHandle = NULL;    

    char errorMessage[MAX_ERROR_DESCRIPTION] = {0};
    int32 error = 0, lastErrorCode = 0;
    int i = 0;

    float64 centerFrequency = 1.95e9;			/*(Hz) */
    float64 referenceLevel = 0.000000;						/*(dBm) */
    float64 externalAttenuation = 0.000000;					/*(dB) */
   
    char * frequencyReferenceSource = RFMXWCDMA_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10e6;					/*(Hz) */
    
    int32 enableTrigger = RFMXWCDMA_VAL_FALSE;
    char * digitalEdgeSource = RFMXWCDMA_VAL_PFI0_STR;
    int32 digitalEdge = RFMXWCDMA_VAL_DIGITAL_EDGE_RISING_EDGE;
    float64 triggerDelay = 0.000000;						/*(s) */

    int32 band = 1;

    int32 sweepTimeAuto = RFMXWCDMA_VAL_SEM_SWEEP_TIME_AUTO_TRUE;
    float64 sweepTimeInterval = 0.000667;					/*(s) */

    int32 averagingEnabled = RFMXWCDMA_VAL_SEM_AVERAGING_ENABLED_FALSE;
    int32 averagingCount = 10;
    int32 averagingType = RFMXWCDMA_VAL_SEM_AVERAGING_TYPE_RMS;

    float64 timeout = 10.000000;							/*(s) */
    int32 actualArraySize = 0;
    int32 lowerOffsetMarginArraySize = 0, upperOffsetMarginArraySize = 0;
    float64 x0 = 0.0, dx = 0.0;

    int32* lowerOffsetMeasurementStatus = NULL;
    float64* lowerOffsetMargin = NULL;						/*(dB) */
    float64* lowerOffsetMarginFrequency = NULL;				/*(Hz) */
    float64* lowerOffsetMarginAbsolutePower = NULL;			/*(dBm) */
    float64* lowerOffsetMarginRelativePower = NULL;			/*(dB) */

    int32* upperOffsetMeasurementStatus = NULL;
    float64* upperOffsetMargin = NULL;						/*(dB) */
    float64* upperOffsetMarginFrequency = NULL;				/*(Hz) */
    float64* upperOffsetMarginAbsolutePower = NULL;			/*(dBm) */
    float64* upperOffsetMarginRelativePower = NULL;			/*(dB) */

    int32 measurementStatus = 0;
    float64 absoluteIntegratedPower = 0.0;							/*(dBm) */
    float64 relativeIntegratedPower = 0.0;							/*(dB) */
    
    float32* spectrum = NULL;								/*(dBm) */
    float32* absoluteMask = NULL;							/*(dBm) */
    float32* relativeMask = NULL;
    
    /* Initialize a session */
    RFmxCheckWarn(RFmxWCDMA_Initialize(resourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxWCDMA_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxWCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxWCDMA_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay,
		                                          enableTrigger));
    RFmxCheckWarn(RFmxWCDMA_CfgBand(instrumentHandle, "", band));
    RFmxCheckWarn(RFmxWCDMA_SelectMeasurements(instrumentHandle, "", RFMXWCDMA_VAL_SEM, RFMXWCDMA_VAL_TRUE));
    RFmxCheckWarn(RFmxWCDMA_SEMCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
    RFmxCheckWarn(RFmxWCDMA_SEMCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
    RFmxCheckWarn(RFmxWCDMA_Initiate(instrumentHandle, "", ""));

    RFmxCheckWarn(RFmxWCDMA_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, 0, 
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
            RFmxCheckWarn(RFmxWCDMA_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, lowerOffsetMeasurementStatus, 
                lowerOffsetMargin, lowerOffsetMarginFrequency, lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower, 
                lowerOffsetMarginArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    RFmxCheckWarn(RFmxWCDMA_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, 0, 
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
            RFmxCheckWarn(RFmxWCDMA_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, upperOffsetMeasurementStatus, 
                upperOffsetMargin, upperOffsetMarginFrequency, upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower, 
                upperOffsetMarginArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    RFmxCheckWarn(RFmxWCDMA_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));
    RFmxCheckWarn(RFmxWCDMA_SEMFetchCarrierMeasurement(instrumentHandle, "", timeout, &absoluteIntegratedPower, 
		                                               &relativeIntegratedPower));
    RFmxCheckWarn(RFmxWCDMA_SEMFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL, NULL, 0, &actualArraySize));
    if( actualArraySize > 0 )
    {
        spectrum = (float32 *) malloc(sizeof(float32) * actualArraySize);
        relativeMask = (float32 *) malloc(sizeof(float32) * actualArraySize);
        absoluteMask = (float32 *) malloc(sizeof(float32) * actualArraySize);
        if( spectrum && relativeMask && absoluteMask )
        {
            RFmxCheckWarn(RFmxWCDMA_SEMFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum, relativeMask,
				                                     absoluteMask, actualArraySize, NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    printf("Measurement Status                      : %s\n", 
              (measurementStatus)? "PASS":"FAIL");
    printf("Carrier Absoulte Integrated Power (dBm) : %lf\n",absoluteIntegratedPower);

    printf("\n---------------Lower Offset---------------\n");
    for( i = 0; i < lowerOffsetMarginArraySize; i++ )
    {
        printf("\nLower Offset Segment Measurements	: %d\n", i);
        printf("Margin (dB)                             : %lf\n",lowerOffsetMargin[i]);
        printf("Margin Absolute Power (dBm)             : %lf\n",lowerOffsetMarginAbsolutePower[i]);
        printf("Margin Relative Power (dB)              : %lf\n",lowerOffsetMarginRelativePower[i]);
        printf("Margin Frequency (Hz)                   : %lf\n",lowerOffsetMarginFrequency[i]);
        printf("Measurement Status                      : %s\n", 
              (lowerOffsetMeasurementStatus[i])?"PASS":"FAIL");
    }
    printf("\n---------------Upper Offset---------------\n");
    for( i = 0; i < upperOffsetMarginArraySize; i++ )
    {
        printf("\nUpper Offset Segment Measurements	: %d\n", i);
        printf("Margin (dB)                             : %lf\n",upperOffsetMargin[i]);
        printf("Margin Absolute Power (dBm)             : %lf\n",upperOffsetMarginAbsolutePower[i]);
        printf("Margin Relative Power (dB)              : %lf\n",upperOffsetMarginRelativePower[i]);
        printf("Margin Frequency (Hz)                   : %lf\n",upperOffsetMarginFrequency[i]);
        printf("Measurement Status                      : %s\n", 
              (upperOffsetMeasurementStatus[i])?"PASS":"FAIL");
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
    if (absoluteMask) 
    {
        free(absoluteMask);
    }
    if (relativeMask) 
    {
        free(relativeMask);
    }

    return error;
}
