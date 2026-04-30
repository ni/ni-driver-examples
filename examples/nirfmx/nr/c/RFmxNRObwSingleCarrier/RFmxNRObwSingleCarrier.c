//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Parameters for IQ Power Edge Trigger.
//6. Configure Link Direction, Frequency Range, Carrier Bandwidth and Subcarrier Spacing.
//7. Select OBW measurement and enable Traces.
//8. Configure Sweep Time Parameters.
//9. Configure Span Parameters for OBW measurement.
//10. Configure Averaging Parameters for OBW measurement.
//11. Initiate the Measurement.
//12. Fetch OBW Measurements and Traces.
//13. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxNR.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0;
   int32 lastErrorCode = 0;

   char* selectedPorts = "";
   float64 centerFrequency = 3.5e9;                                                 /* (Hz) */
   float64 referenceLevel = 0.0;                                                    /* (dBm) */
   float64 externalAttenuation = 0.0;                                               /* (dB) */

   char * frequencyReferenceSource = RFMXNR_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e6;                                    /* (Hz) */

   int32 IQPowerEdgeEnabled = RFMXNR_VAL_FALSE;
   float64 IQPowerEdgeLevel = -20.0;                                                /* (dB or dBm) */
   float64 triggerDelay = 0.0;                                                      /* (s) */
   int32 minimumQuietTimeMode = RFMXNR_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 8.0e-6;                                               /* (s) */

   int32 linkDirection = RFMXNR_VAL_LINK_DIRECTION_UPLINK;
   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;
   float64 carrierBandwidth = 100e6;                                                /* (Hz) */
   float64 subcarrierSpacing = 30e3;                                                /* (Hz) */
   int32 powerIntegrationMethod = RFMXNR_VAL_OBW_POWER_INTEGRATION_METHOD_NORMAL;

   int32 sweepTimeAuto = RFMXNR_VAL_OBW_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 1.0e-3;                                              /* (s) */

   int32 averagingEnabled = RFMXNR_VAL_OBW_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXNR_VAL_OBW_AVERAGING_TYPE_RMS;

   int32 spanAuto = RFMXNR_VAL_OBW_SPAN_AUTO_TRUE;
   float64 span = 200e6;                                                            /* (Hz) */
 
   float64 timeout = 10.0;                                                          /* (s) */

   float64 occupiedBandwidth = 0.0;                                                 /* (Hz) */
   float64 absolutePower = 0.0;                                                     /* (dBm) */
   float64 startFrequency = 0.0;                                                    /* (Hz) */
   float64 stopFrequency = 0.0;                                                     /* (Hz) */

   int32 actualArraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32* spectrum = NULL;

   /* Initialize a session */
   RFmxCheckWarn(RFmxNR_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxNR_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxNR_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxNR_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxNR_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXNR_VAL_IQ_POWER_EDGE_RISING_SLOPE,
      IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
      RFMXNR_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, IQPowerEdgeEnabled));

   RFmxCheckWarn(RFmxNR_SetLinkDirection(instrumentHandle, "", linkDirection));
   RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, "", frequencyRange));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, "", carrierBandwidth));
   RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, "", subcarrierSpacing));

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_OBW, RFMXNR_VAL_TRUE));

   RFmxCheckWarn(RFmxNR_OBWCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxNR_OBWSetSpanAuto(instrumentHandle, "", spanAuto));
   RFmxCheckWarn(RFmxNR_OBWSetPowerIntegrationMethod(instrumentHandle, "", powerIntegrationMethod));
   RFmxCheckWarn(RFmxNR_OBWSetSpan(instrumentHandle, "subblock0", span));
   RFmxCheckWarn(RFmxNR_OBWCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

   /* Fetch results */
   RFmxCheckWarn(RFmxNR_OBWFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
      if (spectrum)
      {
         RFmxCheckWarn(RFmxNR_OBWFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxNR_OBWFetchMeasurement(instrumentHandle, "", timeout, &occupiedBandwidth, &absolutePower,
      &startFrequency, &stopFrequency));

   printf("----------------- Measurement -----------------\n\n");
   printf("Occupied Bandwidth (Hz)  : %lf\n", occupiedBandwidth);
   printf("Absolute Power (dBm)     : %lf\n", absolutePower);
   printf("Start Frequency (Hz)     : %lf\n", startFrequency);
   printf("Stop Frequency (Hz)      : %lf\n\n", stopFrequency);

Error:
   if (error)
   {
      RFmxNR_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s %d\n", errorMessage, error);
   }

   if (instrumentHandle)
   {
      RFmxNR_Close(instrumentHandle, RFMXNR_VAL_FALSE);
   }

   /* Free allocated memory */
   if (spectrum)
   {
       free(spectrum);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
