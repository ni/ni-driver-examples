//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties(Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Link Direction as Downlink, Frequency Range, Carrier Bandwidthand BWP Subcarrier Spacing.
//7. Configure DL Test Model and DL Test Model Duplex Scheme.
//8. Configure gNodeB Type.
//9. Configure Rated TRP and Rated EIRP.
//10. Select PVT measurement and enable Traces.
//11. Configure Measurement Methods.
//12. Configure OFF Power Exclusion Periods.
//13. Configure Averaging Parameters for PVT measurement.
//14. Configure Measurement Interval.
//15. Initiate the Measurement.
//16. Fetch PVT Measurements and Traces.
//17. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxNR.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                   4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                     256

#define NUMBER_OF_RESOURCE_BLOCK_CLUSTERS       1

int main(int argc, char *argv[])
{
   char* resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0;

   char subblockString[MAX_SELECTOR_STRING];
   char carrierString[MAX_SELECTOR_STRING];
   char bandwidthPartString[MAX_SELECTOR_STRING];
   char userString[MAX_SELECTOR_STRING];
   char PUSCHString[MAX_SELECTOR_STRING];
   char PUSCHClusterString[MAX_SELECTOR_STRING];

   char* selectedPorts = "";
   float64 centerFrequency = 3.5e9;                                                       /* (Hz) */
   float64 referenceLevel = 0.0;                                                          /* (dBm) */
   float64 externalAttenuation = 0.0;                                                     /* (dB) */

   char* frequencyReferenceSource = RFMXNR_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e6;                                          /* (Hz) */

   float64 IQPowerEdgeLevel = -20.0;                                                      /* (dB) */
   float64 triggerDelay = 0.0;                                                            /* (s) */
   int32 minimumQuietTimeMode = RFMXNR_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 8.0e-6;                                                     /* (s) */

   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;
   float64 carrierBandwidth = 100e6;                                                      /* (Hz) */
   float64 subcarrierSpacing = 30e3;                                                      /* (Hz) */

   int32 downlinkTestModelDuplexScheme = RFMXNR_VAL_DOWNLINK_TEST_MODEL_DUPLEX_SCHEME_TDD;
   int32 downlinkTestModel = RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM1_1;

   int32 gNodeBType = RFMXNR_VAL_GNODEB_TYPE_1C;
   float64 ratedTRP = 0.0;                                                                 /* (dBm) */
   float64 ratedEIRP = 0.0;                                                                /* (dBm) */

   int32 measurementMethod = RFMXNR_VAL_PVT_MEASUREMENT_METHOD_NORMAL;
   float64 OFFPowerExclusionBefore = 0.0;                                                 /* (s) */
   float64 OFFPowerExclusionAfter = 0.0;                                                  /* (s) */

   int32 averagingEnabled = RFMXNR_VAL_PVT_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXNR_VAL_PVT_AVERAGING_TYPE_RMS;

   int32 measurementIntervalAuto = RFMXNR_VAL_PVT_MEASUREMENT_INTERVAL_AUTO_TRUE;
   float64 measurementInterval = 0.01;                                                    /* (s) */

   float64 timeout = 10.0;                                                                /* (s) */

   int32 measurementStatus = RFMXNR_VAL_PVT_MEASUREMENT_STATUS_FAIL;
   float64 PVTResultsPkWindowedOFFPwr = 0.0;                                              /* (dBm/MHz) */
   float64 PVTResultsPkWindowedOFFPwrMargin = 0.0;                                        /* (dB) */
   float64 PVTResultsPkWindowedOFFPwrTime = 0.0;                                          /* (s) */
   float64 absoluteONPower = 0.0;                                                         /* (dBm) */

   int32 actualArraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32* signalPower = NULL;                                                           /* (dBm/MHz) */
   float32* absoluteLimit = NULL;                                                         /* (dBm/MHz) */
   float32* windowedSignalPower = NULL;                                                   /* (dBm/MHz) */

   /* Initialize a session */
   RFmxCheckWarn(RFmxNR_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxNR_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource,
      frequencyReferenceFrequency));
   RFmxCheckWarn(RFmxNR_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   RFmxCheckWarn(RFmxNR_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxNR_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXNR_VAL_IQ_POWER_EDGE_RISING_SLOPE,
      IQPowerEdgeLevel, triggerDelay, minimumQuietTimeMode, minimumQuietTime,
      RFMXNR_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, RFMXNR_VAL_TRUE));

   RFmxCheckWarn(RFmxNR_SetLinkDirection(instrumentHandle, "", RFMXNR_VAL_LINK_DIRECTION_DOWNLINK));
   RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, "", frequencyRange));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, "", carrierBandwidth));
   RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, "", subcarrierSpacing));

   RFmxCheckWarn(RFmxNR_SetDownlinkTestModel(instrumentHandle, "", downlinkTestModel));
   RFmxCheckWarn(RFmxNR_SetDownlinkTestModelDuplexScheme(instrumentHandle, "", downlinkTestModelDuplexScheme));

   RFmxCheckWarn(RFmxNR_SetgNodeBType(instrumentHandle, "", gNodeBType));

   RFmxCheckWarn(RFmxNR_SetRatedTRP(instrumentHandle, "", ratedTRP));
   RFmxCheckWarn(RFmxNR_SetRatedEIRP(instrumentHandle, "", ratedEIRP));

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_PVT, RFMXNR_VAL_TRUE));

   RFmxCheckWarn(RFmxNR_PVTCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
   RFmxCheckWarn(RFmxNR_PVTCfgOFFPowerExclusionPeriods(instrumentHandle, "", OFFPowerExclusionBefore,
      OFFPowerExclusionAfter));
   RFmxCheckWarn(RFmxNR_PVTCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));

   RFmxCheckWarn(RFmxNR_PVTSetMeasurementIntervalAuto(instrumentHandle, "", measurementIntervalAuto));
   RFmxCheckWarn(RFmxNR_PVTSetMeasurementInterval(instrumentHandle, "", measurementInterval));

   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

   /* Fetch results */

   RFmxCheckWarn(RFmxNR_PVTGetResultsMeasurementStatus(instrumentHandle, "", &measurementStatus));
   RFmxCheckWarn(RFmxNR_PVTGetResultsPeakWindowedOFFPower(instrumentHandle, "",
       &PVTResultsPkWindowedOFFPwr));
   RFmxCheckWarn(RFmxNR_PVTGetResultsPeakWindowedOFFPowerMargin(instrumentHandle, "",
       &PVTResultsPkWindowedOFFPwrMargin));
   RFmxCheckWarn(RFmxNR_PVTGetResultsPeakWindowedOFFPowerTime(instrumentHandle, "",
       &PVTResultsPkWindowedOFFPwrTime));
   RFmxCheckWarn(RFmxNR_PVTGetResultsAbsoluteONPower(instrumentHandle, "", &absoluteONPower));

   RFmxCheckWarn(RFmxNR_PVTFetchSignalPowerTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, NULL,
      0, &actualArraySize));

   if (actualArraySize > 0)
   {
      signalPower = (float32*)malloc(sizeof(float32) * actualArraySize);
      absoluteLimit = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (signalPower && absoluteLimit)
      {
         RFmxCheckWarn(RFmxNR_PVTFetchSignalPowerTrace(instrumentHandle, "", timeout, &x0, &dx,
            signalPower, absoluteLimit, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxNR_PVTFetchWindowedSignalPowerTrace(instrumentHandle, "", timeout, NULL, NULL, NULL,
       0, &actualArraySize));
   if (actualArraySize > 0)
   {
       windowedSignalPower = (float32*)malloc(sizeof(float32) * actualArraySize);
       if (windowedSignalPower)
       {
           RFmxCheckWarn(RFmxNR_PVTFetchWindowedSignalPowerTrace(instrumentHandle, "", timeout, &x0, &dx,
               windowedSignalPower, actualArraySize, NULL));
       }
       else
       {
           printf("malloc failed.\n");
           goto Error;
       }
   }

   printf("------------------Measurement------------------\n");
   printf("Measurement Status                             : %s\n",
      measurementStatus == RFMXNR_VAL_PVT_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
   printf("PVT Results Pk Windowed OFF Pwr (dBm/MHz)      : %lf\n", PVTResultsPkWindowedOFFPwr);
   printf("PVT Results Pk Windowed OFF Pwr Margin (dB)    : %lf\n", PVTResultsPkWindowedOFFPwrMargin);
   printf("PVT Results Pk Windowed OFF Pwr Time (s)       : %lf\n", PVTResultsPkWindowedOFFPwrTime);
   printf("Absolute ON Power (dBm)                        : %lf\n\n", absoluteONPower);

Error:
   if (error)
   {
      RFmxNR_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (instrumentHandle)
   {
      RFmxNR_Close(instrumentHandle, RFMXNR_VAL_FALSE);
   }

   /* Free allocated memory */
   if (signalPower)
   {
      free(signalPower);
   }
   if (absoluteLimit)
   {
      free(absoluteLimit);
   }
   if (windowedSignalPower)
   {
      free(windowedSignalPower);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
