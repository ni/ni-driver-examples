//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Configure Link Direction to Sidelink.
//7. Configure operating Band to 47.
//8. Configure Auto Resource Block Detection Enabled to True.
//9. Configure Auto DMRS Detection Enabled to True.
//10. Select ModAcc measurement and enable Traces.
//11. Configure Synchronization Mode and Measurement Interval.
//12. Configure EVM Unit.
//13. Initiate the Measurement.
//14. Fetch ModAcc Measurements and Traces.
//15. Close RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxLTE.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

int main(int argc, char *argv[])
{
   char *resourceName = "RFSA";
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0;

   float64 centerFrequency = 5.89e9;                                       /* (Hz) */
   float64 referenceLevel = 0.0;                                           /* (dBm) */
   float64 externalAttenuation = 0.0;                                      /* (dB) */

   char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
   float64 frequency = 10e6;                                               /* (Hz) */

   int32 IQPowerEdgeEnabled = RFMXLTE_VAL_TRUE;
   float64 IQPowerEdgeLevel = -20.0;                                       /* (dB) */
   float64 triggerDelay = 0.0;                                             /* (s) */
   int32 minimumQuiteTimeMode = RFMXLTE_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 50.0e-6;                                     /* (s) */

   float64 carrierBandwidth = 10.0e6;                                      /* (Hz) */

   int32 measurementOffset = 0;                                            /* (slots) */
   int32 measurementLength = 1;                                            /* (slots) */

   int32 evmUnit = RFMXLTE_VAL_MODACC_EVM_UNIT_PERCENTAGE;

   float64 timeout = 10.000000;                                            /* (s) */
   int32 peakCompositeEVMSubcarrierIndex = 0;
   int32 peakCompositeEVMSymbolIndex = 0;
   float64 meanRMSCompositeEVM = 0.0;                                      /* (% or dB) */
   float64 maxPeakCompositeEVM = 0.0;                                      /* (% or dB) */
   float64 meanFrequencyError = 0.0;                                       /* (Hz) */
   int32 peakCompositeEVMSlotIndex = 0;

   float64 meanIQOriginOffset = 0.0;                                       /* (dBc) */
   float64 meanIQGainImbalance = 0.0;                                      /* (dB) */
   float64 meanIQQuadratureError = 0.0;                                    /* (deg) */
   float64 inBandEmissionMargin = 0.0;                                     /* (dB) */
   NIComplexSingle* dataConstellation = NULL;
   NIComplexSingle* DMRSConstellation = NULL;
   float32* meanRMSEVMPerSubcarrier = NULL;                                /* (% or dB) */

   float64 x0 = 0.0, dx = 0.0;

   int32 actualArraySize = 0;
   int32 dataConstellationActualArraySize = 0;
   int32 DMRSConstellationActualArraySize = 0;

   /* Initialize a session */
   RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequency));
   RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxLTE_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", RFMXLTE_VAL_IQ_POWER_EDGE_RISING_SLOPE,
      IQPowerEdgeLevel, triggerDelay, minimumQuiteTimeMode, minimumQuietTime,
      RFMXLTE_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE, IQPowerEdgeEnabled));
   RFmxCheckWarn(RFmxLTE_CfgComponentCarrier(instrumentHandle, "", carrierBandwidth, 0.0, 0));
   RFmxCheckWarn(RFmxLTE_CfgLinkDirection(instrumentHandle, "", RFMXLTE_VAL_LINK_DIRECTION_SIDELINK));
   RFmxCheckWarn(RFmxLTE_CfgBand(instrumentHandle, "", 47));
   RFmxCheckWarn(RFmxLTE_CfgAutoResourceBlockDetectionEnabled(instrumentHandle, "",
      RFMXLTE_VAL_AUTO_RESOURCE_BLOCK_DETECTION_ENABLED_TRUE));
   RFmxCheckWarn(RFmxLTE_CfgAutoDMRSDetectionEnabled(instrumentHandle, "", RFMXLTE_VAL_AUTO_DMRS_DETECTION_ENABLED_TRUE));
   RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_MODACC, RFMXLTE_VAL_TRUE));
   RFmxCheckWarn(RFmxLTE_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "",
      RFMXLTE_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT, measurementOffset, measurementLength));
   RFmxCheckWarn(RFmxLTE_ModAccCfgEVMUnit(instrumentHandle, "", evmUnit));
   RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxLTE_ModAccFetchCompositeEVM(instrumentHandle, "", timeout, &meanRMSCompositeEVM, &maxPeakCompositeEVM,
      &meanFrequencyError, &peakCompositeEVMSymbolIndex, &peakCompositeEVMSubcarrierIndex, &peakCompositeEVMSlotIndex));
   RFmxCheckWarn(RFmxLTE_ModAccFetchIQImpairments(instrumentHandle, "", timeout, &meanIQOriginOffset,
      &meanIQGainImbalance, &meanIQQuadratureError));
   RFmxCheckWarn(RFmxLTE_ModAccFetchInBandEmissionMargin(instrumentHandle, "", timeout, &inBandEmissionMargin));

   RFmxCheckWarn(RFmxLTE_ModAccFetchPSSCHConstellationTrace(instrumentHandle, "", timeout,
      NULL, 0, &dataConstellationActualArraySize,
      NULL, 0, &DMRSConstellationActualArraySize));
   if (dataConstellationActualArraySize > 0 && DMRSConstellationActualArraySize > 0)
   {
      dataConstellation = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * dataConstellationActualArraySize);
      DMRSConstellation = (NIComplexSingle*)malloc(sizeof(NIComplexSingle) * DMRSConstellationActualArraySize);
      if (dataConstellation && DMRSConstellation)
      {
         RFmxCheckWarn(RFmxLTE_ModAccFetchPSSCHConstellationTrace(instrumentHandle, "", timeout,
            dataConstellation, dataConstellationActualArraySize, NULL,
            DMRSConstellation, DMRSConstellationActualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxLTE_ModAccFetchEVMPerSubcarrierTrace(instrumentHandle, "", timeout, NULL, NULL, NULL,
      0, &actualArraySize));
   if (actualArraySize > 0)
   {
      meanRMSEVMPerSubcarrier = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (meanRMSEVMPerSubcarrier)
      {
         RFmxCheckWarn(RFmxLTE_ModAccFetchEVMPerSubcarrierTrace(instrumentHandle, "", timeout, &x0, &dx,
            meanRMSEVMPerSubcarrier, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   /* Display results */
   printf("------------------Measurement------------------\n");
   printf("Mean RMS Composite EVM (%% or dB)     : %lf\n", meanRMSCompositeEVM);
   printf("Max Peak Composite EVM (%% or dB)     : %lf\n", maxPeakCompositeEVM);
   printf("Peak Composite EVM Slot Index        : %d\n", peakCompositeEVMSlotIndex);
   printf("Peak Composite EVM Symbol Index      : %d\n", peakCompositeEVMSymbolIndex);
   printf("Peak Composite EVM Subcarrier Index  : %d\n", peakCompositeEVMSubcarrierIndex);
   printf("Mean Frequency Error (Hz)            : %lf\n", meanFrequencyError);
   printf("Mean IQ Origin Offset (dBc)          : %lf\n", meanIQOriginOffset);
   printf("Mean IQ Gain Imbalance (dB)          : %lf\n", meanIQGainImbalance);
   printf("Mean IQ Quadrature Error (deg)       : %lf\n", meanIQQuadratureError);
   printf("In Band Emission Margin (dB)         : %lf\n", inBandEmissionMargin);

Error:
   if (error)
   {
      RFmxLTE_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (instrumentHandle)
   {
      RFmxLTE_Close(instrumentHandle, RFMXLTE_VAL_FALSE);
   }

   /* Free allocated memory */
   if (dataConstellation)
   {
      free(dataConstellation);
   }
   if (DMRSConstellation)
   {
      free(DMRSConstellation);
   }
   if (meanRMSEVMPerSubcarrier)
   {
      free(meanRMSEVMPerSubcarrier);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
