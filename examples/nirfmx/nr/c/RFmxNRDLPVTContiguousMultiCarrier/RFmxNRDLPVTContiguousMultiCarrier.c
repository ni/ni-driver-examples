//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure Selected Ports.
//4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//5. Configure Trigger Type and Trigger Parameters.
//6. Configure Link Direction as Downlink, Frequency Range, Channel Raster, Component Carrier Spacing and gNodeB Type.
//7. Configure Carrier.
//8. Configure BWP Subcarrier Spacing, DL Test Model Duplex Scheme and DL Test Model for all carriers
//9. Select PVT measurement and enable Traces.
//10. Configure Measurement Methods.
//11. Configure OFF Power Exclusion Periods.
//12. Configure Averaging Parameters for PVT measurement.
//13. Configure Measurement Interval.
//14. Initiate the Measurement.
//15. Fetch PVT Measurements and Traces.
//16. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <string.h>

#include "niRFmxNR.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                  256

#define NUMBER_OF_COMPONENT_CARRIERS         2

int main(int argc, char *argv[])
{
   char* resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;
   int i = 0, j = 0;

   char subblockString[MAX_SELECTOR_STRING];
   char carrierString[MAX_SELECTOR_STRING];

   char* selectedPorts = "";
   float64 centerFrequency = 3.5e9;                                                          /* (Hz) */
   float64 referenceLevel = 0.0;                                                             /* (dBm) */
   float64 externalAttenuation = 0.0;                                                        /* (dB) */

   char* frequencyReferenceSource = RFMXNR_VAL_ONBOARD_CLOCK_STR;
   float64 frequencyReferenceFrequency = 10.0e6;                                             /* (Hz) */

   float64 IQPowerEdgeLevel = -20.0;                                                         /* (dB) */
   float64 triggerDelay = 0.0;                                                               /* (s) */
   int32 minimumQuietTimeMode = RFMXNR_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 8.0e-6;                                                        /* (s) */

   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE1;
   int32 measurementMethod = RFMXNR_VAL_PVT_MEASUREMENT_METHOD_NORMAL;
   float64 subcarrierSpacing = 30e3;                                                         /* (Hz) */
   float64 OFFPowerExclusionBefore = 0.0;                                                    /* (s) */
   int32 gNodeBType = RFMXNR_VAL_GNODEB_TYPE_1C;
   float64 OFFPowerExclusionAfter = 0.0;                                                     /* (s) */

   int32 componentCarrierSpacingType = RFMXNR_VAL_COMPONENT_CARRIER_SPACING_TYPE_NOMINAL;
   float64 channelRaster = 15e3;                                                             /* (Hz) */
   int32 componentCarrierAtCenterFrequency = -1;                                      

   float64 componentCarrierBandwidth[NUMBER_OF_COMPONENT_CARRIERS] = { 100e6, 100e6 };       /* (Hz) */
   float64 componentCarrierFrequency[NUMBER_OF_COMPONENT_CARRIERS] = { -49.98e6, 50.01e6 };  /* (Hz) */
   float64 ratedTRP[NUMBER_OF_COMPONENT_CARRIERS] = { 0.00, 0.00 };                          /* (dBm) */
   float64 ratedEIRP[NUMBER_OF_COMPONENT_CARRIERS] = { 0.00, 0.00 };                         /* (dBm) */

   int32 downlinkTestModel = RFMXNR_VAL_DOWNLINK_TEST_MODEL_TM1_1;
   int32 downlinkTestModelDuplexScheme = RFMXNR_VAL_DOWNLINK_TEST_MODEL_DUPLEX_SCHEME_TDD;

   int32 averagingEnabled = RFMXNR_VAL_PVT_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXNR_VAL_PVT_AVERAGING_TYPE_RMS;

   int32 measurementIntervalAuto = RFMXNR_VAL_PVT_MEASUREMENT_INTERVAL_AUTO_TRUE;
   float64 measurementInterval = 0.01;                                                       /* (s) */

   float64 timeout = 10.0;                                                                   /* (s) */

   int32 measurementStatus[NUMBER_OF_COMPONENT_CARRIERS] = { RFMXNR_VAL_PVT_MEASUREMENT_STATUS_FAIL };
   float64 PVTResultsPkWindowedOFFPwr[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };               /* (dBm/MHz) */
   float64 PVTResultsPkWindowedOFFPwrMargin[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };         /* (dB) */
   float64 PVTResultsPkWindowedOFFPwrTime[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };           /* (s) */
   float64 absoluteONPower[NUMBER_OF_COMPONENT_CARRIERS] = { 0.0 };                          /* (dBm) */

   int32 actualArraySize = 0;
   float64 x0 = 0.0, dx = 0.0;
   float32* signalPower[NUMBER_OF_COMPONENT_CARRIERS] = { NULL };                             /* (dBm/MHz) */
   float32* absoluteLimit[NUMBER_OF_COMPONENT_CARRIERS] = { NULL };                           /* (dBm/MHz) */
   float32* windowedSignalPower[NUMBER_OF_COMPONENT_CARRIERS] = { NULL };                     /* (dBm/MHz) */

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
   RFmxCheckWarn(RFmxNR_SetChannelRaster(instrumentHandle, "", channelRaster));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierSpacingType(instrumentHandle, "", componentCarrierSpacingType));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierAtCenterFrequency(instrumentHandle, "", componentCarrierAtCenterFrequency));
   RFmxCheckWarn(RFmxNR_SetgNodeBType(instrumentHandle, "", gNodeBType));

   RFmxCheckWarn(RFmxNR_SetNumberOfComponentCarriers(instrumentHandle, "", NUMBER_OF_COMPONENT_CARRIERS));

   RFmxNR_BuildSubblockString("", 0, MAX_SELECTOR_STRING, subblockString);
   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      RFmxNR_BuildCarrierString(subblockString, i, MAX_SELECTOR_STRING, carrierString);
      RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, carrierString, componentCarrierBandwidth[i]));
      RFmxCheckWarn(RFmxNR_SetComponentCarrierFrequency(instrumentHandle, carrierString, componentCarrierFrequency[i]));
      RFmxCheckWarn(RFmxNR_SetRatedTRP(instrumentHandle, carrierString, ratedTRP[i]));
      RFmxCheckWarn(RFmxNR_SetRatedEIRP(instrumentHandle, carrierString, ratedEIRP[i]));
   }

   strcpy_s(carrierString, sizeof("carrier::all"), "carrier::all");
   RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, carrierString, subcarrierSpacing));
   RFmxCheckWarn(RFmxNR_SetDownlinkTestModelDuplexScheme(instrumentHandle, carrierString, downlinkTestModelDuplexScheme));
   RFmxCheckWarn(RFmxNR_SetDownlinkTestModel(instrumentHandle, carrierString, downlinkTestModel));

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_PVT, RFMXNR_VAL_TRUE));

   RFmxCheckWarn(RFmxNR_PVTCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
   RFmxCheckWarn(RFmxNR_PVTCfgOFFPowerExclusionPeriods(instrumentHandle, "", OFFPowerExclusionBefore,
      OFFPowerExclusionAfter));
   RFmxCheckWarn(RFmxNR_PVTCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount, averagingType));
   RFmxCheckWarn(RFmxNR_PVTSetMeasurementIntervalAuto(instrumentHandle, "", measurementIntervalAuto));
   RFmxCheckWarn(RFmxNR_PVTSetMeasurementInterval(instrumentHandle, "", measurementInterval));
   
   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

   /* Fetch results */

   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
       RFmxNR_BuildCarrierString(subblockString, i, MAX_SELECTOR_STRING, carrierString);
       RFmxCheckWarn(RFmxNR_PVTGetResultsMeasurementStatus(instrumentHandle, carrierString, 
           &measurementStatus[i]));
       RFmxCheckWarn(RFmxNR_PVTGetResultsPeakWindowedOFFPower(instrumentHandle, carrierString,
           &PVTResultsPkWindowedOFFPwr[i]));
       RFmxCheckWarn(RFmxNR_PVTGetResultsPeakWindowedOFFPowerMargin(instrumentHandle, carrierString,
           &PVTResultsPkWindowedOFFPwrMargin[i]));
       RFmxCheckWarn(RFmxNR_PVTGetResultsPeakWindowedOFFPowerTime(instrumentHandle, carrierString,
           &PVTResultsPkWindowedOFFPwrTime[i]));
       RFmxCheckWarn(RFmxNR_PVTGetResultsAbsoluteONPower(instrumentHandle, carrierString,
           &absoluteONPower[i]));
   }
   
   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      RFmxNR_BuildCarrierString("", i, MAX_SELECTOR_STRING, carrierString);

      actualArraySize = 0;
      RFmxCheckWarn(RFmxNR_PVTFetchSignalPowerTrace(instrumentHandle, carrierString, timeout, NULL, NULL, NULL, NULL,
         0, &actualArraySize));
      if (actualArraySize > 0)
      {
         signalPower[i] = (float32*)malloc(sizeof(float32) * actualArraySize);
         absoluteLimit[i] = (float32*)malloc(sizeof(float32) * actualArraySize);
         if (signalPower[i] && absoluteLimit[i])
         {
            RFmxCheckWarn(RFmxNR_PVTFetchSignalPowerTrace(instrumentHandle, carrierString, timeout, &x0, &dx,
               signalPower[i], absoluteLimit[i], actualArraySize, NULL));
         }
         else
         {
            printf("malloc failed.\n");
            goto Error;
         }
      }

      actualArraySize = 0;
      RFmxCheckWarn(RFmxNR_PVTFetchWindowedSignalPowerTrace(instrumentHandle, carrierString, timeout, NULL, NULL, NULL,
          0, &actualArraySize));
      if (actualArraySize > 0)
      {
          windowedSignalPower[i] = (float32*)malloc(sizeof(float32) * actualArraySize);
          if (windowedSignalPower[i])
          {
              RFmxCheckWarn(RFmxNR_PVTFetchWindowedSignalPowerTrace(instrumentHandle, carrierString, timeout, &x0, &dx,
                  windowedSignalPower[i], actualArraySize, NULL));
          }
          else
          {
              printf("malloc failed.\n");
              goto Error;
          }
      }
   }

   printf("------------------------Measurements------------------------\n\n");
   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      printf("Carrier  : %d\n", i);
      printf("Measurement Status                                 : %s\n",
         measurementStatus[i] == RFMXNR_VAL_PVT_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
      printf("PVT Results Pk Windowed OFF Pwr (dBm/MHz)          : %lf\n", PVTResultsPkWindowedOFFPwr[i]);
      printf("PVT Results Pk Windowed OFF Pwr Margin (dB)        : %lf\n", PVTResultsPkWindowedOFFPwrMargin[i]);
      printf("PVT Results Pk Windowed OFF Pwr Time (s)           : %lf\n", PVTResultsPkWindowedOFFPwrTime[i]);
      printf("Absolute ON Power (dBm)                            : %lf\n", absoluteONPower[i]);
      printf("-------------------------------------------------\n\n");
   }
   
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
   for (i = 0; i < NUMBER_OF_COMPONENT_CARRIERS; i++)
   {
      if (signalPower[i])
      {
         free(signalPower[i]);
      }
      if (absoluteLimit[i])
      {
         free(absoluteLimit[i]);
      }
      if (windowedSignalPower[i])
      {
         free(windowedSignalPower[i]);
      }
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
