//Steps:
//1. Open a new RFmx session.
//2. Configure the basic instrument properties (Clock Source and Clock Frequency).
//3. Configure Selected Ports.
//4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure PAVT measurement and enable the traces.
//7. Configure Measurement Location Type.

//8. Follow these steps depending upon Measurement Location Type :
// When Measurement Location Type is Time, configure
//8.1. Segment Start Time by :
//8.1.1. Configuring Number of Segments, Segment0 Start Time(s) and Segment Interval(s).
//8.2. Segment Start Time by :
//8.2.1. Configuring Segment Start Time(s).
//8.2.2. Configuring Number of Segments.
// When Measurement Location Type is Trigger, configure
//8.3. Number of Segments.

//9. Configure Measurement Bandwidth.
//10. Configure Measurement Interval.
//11. Initiate Measurement.
//12. Fetch PAVT Traces and Measurements.
//13. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION             4096

#define NUMBER_OF_SEGMENTS                1

#define MEASUREMENT_START_TIME_TYPE_STEP  0
#define MEASUREMENT_START_TIME_TYPE_LIST  1

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0;

   char *selectedPorts = "";
   float64 centerFrequency = 1.0e9;                                          /* (Hz) */
   float64 referenceLevel = 0.0;                                             /* (dBm) */
   float64 externalAttenuation = 0.0;                                        /* (dB) */

   char *frequencyReferenceSource = RFMXSPECAN_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e6;                             /* (Hz) */

   int32 enableTrigger = RFMXSPECAN_VAL_TRUE;
   char *digitalEdgeSource = RFMXSPECAN_VAL_PXI_TRIG0_STR;
   int32 digitalEdge = RFMXSPECAN_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                               /* (s) */

   int32 measurementLocationType = RFMXSPECAN_VAL_PAVT_MEASUREMENT_LOCATION_TYPE_TIME;
   int32 numberOfSegments = NUMBER_OF_SEGMENTS;

   /* Segment Step */
   float64 segment0StartTime = 0.0;                                          /* (s) */
   float64 segmentInterval = 1.0e-3;                                         /* (s) */

   /* Segment List */
   const int32 segmentStartTimeArraySize = 1;
   float64 segmentStartTime[] = { 0.0 };                                     /* (s) */

   int32 measurementStartTimeType = MEASUREMENT_START_TIME_TYPE_STEP;

   float64 measurementBandwidth = 10.0e6;                                    /* (Hz) */

   float64 measurementOffset = 0.0;                                          /* (s) */
   float64 measurementLength = 1.0e-3;                                       /* (s) */

   float64 timeout = 10.0;                                                   /* (s) */

   float64 meanRelativePhase[NUMBER_OF_SEGMENTS] = { 0.0 };                  /* (deg) */
   float64 meanRelativeAmplitude[NUMBER_OF_SEGMENTS] = { 0.0 };              /* (dB) */
   float64 meanAbsolutePhase[NUMBER_OF_SEGMENTS] = { 0.0 };                  /* (deg) */
   float64 meanAbsoluteAmplitude[NUMBER_OF_SEGMENTS] = { 0.0 };              /* (dBm) */

   int32 actualArraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32 *amplitude[NUMBER_OF_SEGMENTS] = { NULL };                        /* (dBm) */
   float32 *phase[NUMBER_OF_SEGMENTS] = { NULL };                            /* (deg) */

  /* Initialize a session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge, triggerDelay,
      enableTrigger));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_PAVT, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_PAVTCfgMeasurementLocationType(instrumentHandle, "", measurementLocationType));
   if (measurementLocationType == RFMXSPECAN_VAL_PAVT_MEASUREMENT_LOCATION_TYPE_TIME)
   {
      if (measurementStartTimeType == MEASUREMENT_START_TIME_TYPE_STEP)
      {
         RFmxCheckWarn(RFmxSpecAn_PAVTCfgSegmentStartTimeStep(instrumentHandle, "", numberOfSegments,
            segment0StartTime, segmentInterval));
      }
      else
      {
         RFmxCheckWarn(RFmxSpecAn_PAVTCfgNumberOfSegments(instrumentHandle, "", segmentStartTimeArraySize));
         RFmxCheckWarn(RFmxSpecAn_PAVTCfgSegmentStartTimeList(instrumentHandle, "", segmentStartTime,
            segmentStartTimeArraySize));
      }
   }
   else
   {
      RFmxCheckWarn(RFmxSpecAn_PAVTCfgNumberOfSegments(instrumentHandle, "", numberOfSegments));
   }
   RFmxCheckWarn(RFmxSpecAn_PAVTCfgMeasurementBandwidth(instrumentHandle, "", measurementBandwidth));
   RFmxCheckWarn(RFmxSpecAn_PAVTCfgMeasurementInterval(instrumentHandle, "", measurementOffset, measurementLength));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrive Results */
   RFmxCheckWarn(RFmxSpecAn_PAVTFetchPhaseAndAmplitudeArray(instrumentHandle, "", timeout, meanRelativePhase,
      meanRelativeAmplitude, meanAbsolutePhase, meanAbsoluteAmplitude, NUMBER_OF_SEGMENTS, NULL));

   for (i = 0; i < NUMBER_OF_SEGMENTS; i++)
   {
      actualArraySize = 0;
      RFmxCheckWarn(RFmxSpecAn_PAVTFetchPhaseTrace(instrumentHandle, "", timeout, i, NULL, NULL, NULL,
         0, &actualArraySize));
      if (actualArraySize > 0)
      {
         phase[i] = (float32 *)malloc(sizeof(float32) * actualArraySize);
         if (phase[i])
         {
            RFmxCheckWarn(RFmxSpecAn_PAVTFetchPhaseTrace(instrumentHandle, "", timeout, i, &x0, &dx,
               phase[i], actualArraySize, NULL));
         }
         else
         {
            printf("malloc failed.\n");
            goto Error;
         }
      }

      actualArraySize = 0;
      RFmxCheckWarn(RFmxSpecAn_PAVTFetchAmplitudeTrace(instrumentHandle, "", timeout, i, NULL, NULL, NULL,
         0, &actualArraySize));
      if (actualArraySize > 0)
      {
         amplitude[i] = (float32 *)malloc(sizeof(float32) * actualArraySize);
         if (amplitude[i])
         {
            RFmxCheckWarn(RFmxSpecAn_PAVTFetchAmplitudeTrace(instrumentHandle, "", timeout, i, &x0, &dx,
               amplitude[i], actualArraySize, NULL));
         }
         else
         {
            printf("malloc failed.\n");
            goto Error;
         }
      }
   }

   printf("Segment0 Mean Absolute Phase (deg)      : %lf\n", meanAbsolutePhase[0]);
   printf("Segment0 Mean Absolute Amplitude (dBm)  : %lf\n\n", meanAbsoluteAmplitude[0]);
   printf("Segment Measurements\n");
   for (i = 0; i < NUMBER_OF_SEGMENTS; i++)
   {
      printf("Segment  :  %d\n", i);
      printf("Mean Relative Phase (deg)               : %lf\n", meanRelativePhase[i]);
      printf("Mean Relative Amplitude (dB)            : %lf\n", meanRelativeAmplitude[i]);
      printf("-------------------------------------------------\n\n");
   }

Error:
   if (error)
   {
      RFmxSpecAn_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (instrumentHandle)
   {
      RFmxSpecAn_Close(instrumentHandle, RFMXSPECAN_VAL_FALSE);
   }

   /* Free allocated memory */
   for (i = 0; i < NUMBER_OF_SEGMENTS; i++)
   {
       if (phase[i])
       {
           free(phase[i]);
       }
       if (amplitude[i])
       {
           free(amplitude[i]);
       }
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
