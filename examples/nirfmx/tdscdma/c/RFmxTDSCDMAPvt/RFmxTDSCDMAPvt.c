//Steps:
//1. Open a new RFmx session.
//2. Configure Frequency Reference.
//3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters..
//5. Select PvT measurement and enable traces.
//6. Configure Midamble.
//7. Configure Measurement Method.
//8. Configure Averaging.
//9. Initiate the Measurement.
//10. Fetch PvT Measurements and Traces.
//11. Close the RFmx session.

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

    /*Frequency Reference*/
    char * frequencyReferenceSource = RFMXTDSCDMA_VAL_ONBOARD_CLOCK_STR;
    float64 frequencyReferenceFrequency = 10E+6;              /*(Hz) */
    float64 centerFrequency = 1.91E+9;                        /*(Hz) */

    float64 referenceLevel = 0.00;                            /*(dBm) */
    float64 externalAttenuation = 0.00;                       /*(dB) */

    /*Trigger */
    int32 enableTrigger = RFMXTDSCDMA_VAL_TRUE;
    char * IQPowerEdgeSource = "0";
    int32 IQPowerEdgeSlope = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_RISING_SLOPE;
    float64 IQPowerEdgeLevel = -20.00;                       /*(dB) */
    int32 IQPowerEdgeLevelType = RFMXTDSCDMA_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
    float64 triggerDelay = 0.00;                             /*(s) */

    int32 minimumQuietTimeMode = RFMXTDSCDMA_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
    float64 minimumQuietTime = 16E-6;                        /*(s) */


    /*Average Settings*/
    int32 averagingEnabled = RFMXTDSCDMA_VAL_PVT_AVERAGING_ENABLED_FALSE;       
    int32 averagingCount = 10;            
    int32 averagingType=RFMXTDSCDMA_VAL_PVT_AVERAGING_TYPE_RMS;

    int32 measurementMethod=RFMXTDSCDMA_VAL_PVT_MEASUREMENT_METHOD_NORMAL;
    

    /*Midamble Settings*/
    int32 midambleAutoDetectionMode = RFMXTDSCDMA_VAL_MIDAMBLE_AUTO_DETECTION_MODE_MIDAMBLE_SHIFT;       
    int32 maximumNumberOfUsers = 16;            
    int32 midambleShift=8;

    float64 timeout = 10.00;					           /*(s) */

    /*Variables to Store Results*/
    /*Measurement Status*/
     int32 measurementStatus=0;

    /*Absolute Powers*/
    float64 meanAbsoluteONPower = 0.00;				      /*(dBm) */
    float64 meanAbsoluteOFFPower = 0.00;				  /*(dBm) */
    
    /*Segment Measurement Array*/
    int32 *segmentStatus=NULL;
   
    float64 *segmentMargin=NULL;                          /*(dB) */
    float64 *segmentMarginTime=NULL;                      /*(sec) */
    float64 *segmentMeanAbsolutePower=NULL;               /*(dBm) */
    float64 *segmentMaximumAbsolutePower=NULL;            /*(dBm) */
    float64 *segmentMinimumAbsolutePower=NULL;            /*(dBm) */
    float64 x0 = 0.0, dx = 0.0;
    int32   segmentMeasurementArraySize=0;
    int32 i = 0;

    /*Signal Powers*/
    int32   actualArraySize=0;
    float32 *signalPower=NULL;
    float32 *absoluteLimit=NULL;
    


    /* Initialize a session */
    RFmxCheckWarn(RFmxTDSCDMA_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));
    RFmxCheckWarn(RFmxTDSCDMA_CfgFrequencyReference(instrumentHandle,"",frequencyReferenceSource,
		                                             frequencyReferenceFrequency));
    RFmxCheckWarn(RFmxTDSCDMA_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
    RFmxCheckWarn(RFmxTDSCDMA_CfgIQPowerEdgeTrigger(instrumentHandle, "", IQPowerEdgeSource, IQPowerEdgeSlope,   
                                                        IQPowerEdgeLevel,triggerDelay, minimumQuietTimeMode, 
                                                        minimumQuietTime,IQPowerEdgeLevelType,enableTrigger));
    RFmxCheckWarn(RFmxTDSCDMA_SelectMeasurements(instrumentHandle, "",RFMXTDSCDMA_VAL_PVT , RFMXTDSCDMA_VAL_TRUE));
    RFmxCheckWarn(RFmxTDSCDMA_CfgMidambleShift(instrumentHandle,"",midambleAutoDetectionMode,maximumNumberOfUsers,midambleShift));
    RFmxCheckWarn(RFmxTDSCDMA_PVTCfgMeasurementMethod(instrumentHandle,"",measurementMethod));
    RFmxCheckWarn(RFmxTDSCDMA_PVTCfgAveraging(instrumentHandle,"",averagingEnabled,averagingCount,averagingType));
	RFmxCheckWarn(RFmxTDSCDMA_Initiate(instrumentHandle, "", ""));
    RFmxCheckWarn(RFmxTDSCDMA_PVTFetchMeasurementStatus(instrumentHandle,"",timeout,&measurementStatus));
    RFmxCheckWarn(RFmxTDSCDMA_PVTFetchPowers(instrumentHandle,"",timeout,&meanAbsoluteONPower,&meanAbsoluteOFFPower));
    RFmxCheckWarn(RFmxTDSCDMA_PVTFetchSegmentMeasurementArray(instrumentHandle,"",timeout,NULL,NULL,NULL,NULL,NULL,
		                                                      NULL,0,&segmentMeasurementArraySize));

   if( segmentMeasurementArraySize > 0 )
    {
        segmentStatus = (int32 *) malloc(sizeof(int32) * segmentMeasurementArraySize);
        segmentMargin = (float64 *) malloc(sizeof(float64) * segmentMeasurementArraySize);
        segmentMarginTime = (float64 *) malloc(sizeof(float64) * segmentMeasurementArraySize);
        segmentMeanAbsolutePower = (float64 *) malloc(sizeof(float64) * segmentMeasurementArraySize);
        segmentMaximumAbsolutePower = (float64 *) malloc(sizeof(float64) * segmentMeasurementArraySize);
        segmentMinimumAbsolutePower = (float64 *) malloc(sizeof(float64) * segmentMeasurementArraySize);
        if( segmentStatus && segmentMargin && segmentMarginTime &&
             segmentMeanAbsolutePower && segmentMaximumAbsolutePower && segmentMinimumAbsolutePower)
        {
            RFmxCheckWarn(RFmxTDSCDMA_PVTFetchSegmentMeasurementArray(instrumentHandle,"",timeout,segmentStatus,segmentMargin,
					segmentMarginTime,segmentMeanAbsolutePower,segmentMaximumAbsolutePower,segmentMinimumAbsolutePower,
					segmentMeasurementArraySize,NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

   RFmxCheckWarn(RFmxTDSCDMA_PVTFetchSignalPowerTrace(instrumentHandle,"",timeout,&x0,&dx,NULL,NULL,0,&actualArraySize));

    if(actualArraySize>0)
    {
        signalPower = (float32*)malloc(sizeof(float32) * actualArraySize);
        absoluteLimit = (float32*)malloc(sizeof(float32) * actualArraySize);
        if(signalPower && absoluteLimit)
        {
            RFmxCheckWarn(RFmxTDSCDMA_PVTFetchSignalPowerTrace(instrumentHandle,"",timeout,&x0,&dx,signalPower,
				                                               absoluteLimit,actualArraySize,NULL));
        }
        else
        {
            printf("malloc failed.\n");
            goto Error;
        }
    }

    
    

      printf("PVT  Results                           : \n");
	  
      for( i = 0; i < segmentMeasurementArraySize; i++ )
    {
        printf("\nSegment                                : %d\n", i);
        printf("Measurement Status                     : %s\n",(segmentStatus[i])?"Pass":"Fail");
        printf("Margin (dB)                            : %lf\n",segmentMargin[i]);
        printf("Margin Time(sec)                       : %lf\n",segmentMarginTime[i]);
	    printf("Mean Absolute Power (dBm)              : %lf\n",segmentMeanAbsolutePower[i]);
        printf("Maximum Absolute Power (dBm)           : %lf\n",segmentMaximumAbsolutePower[i]);
        printf("Minimum Absolute Power (dBm)           : %lf\n",segmentMinimumAbsolutePower[i]);
    }

      printf("\nMeasurement Status                     : %s\n",(measurementStatus)?"Pass":"Fail");

      printf("\nAbsolute Powers                        : \n" );
      printf("Mean Absolute ON Power (dBm)           : %lf\n",meanAbsoluteONPower);
      printf("Mean Absolute OFF Power (dBm)          : %lf\n",meanAbsoluteOFFPower);


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
    if (segmentStatus)
    {
        free(segmentStatus);
    }
    if (segmentMargin)
    {
        free(segmentMargin);
    }
    if (segmentMarginTime)
    {
        free(segmentMarginTime);
    }
    if(segmentMeanAbsolutePower)
    {
        free(segmentMeanAbsolutePower);
    }
    if (segmentMaximumAbsolutePower)
    {
        free(segmentMaximumAbsolutePower);
    }
    if (segmentMinimumAbsolutePower)
    {
        free(segmentMinimumAbsolutePower);
    }
    if (signalPower)
    {
        free(signalPower);
    }
    if (absoluteLimit)
    {
        free(absoluteLimit);
    }

    printf("Press any key to exit\n");
    _getch();

    return error;
}
