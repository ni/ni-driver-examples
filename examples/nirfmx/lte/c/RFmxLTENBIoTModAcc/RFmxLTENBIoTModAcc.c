//Steps:
//1. Open a new RFmx Session.
//2. Configure Frequency Reference.
//3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//4. Configure Trigger Type and Trigger Parameters.
//5. Configure Carrier Bandwidth.
//6. Configure NB-IoT Component Carrier.
//7. Configure NPUSCH Format.
//8. Configure Auto NPUSCH Channel Detection Enabled.
//9. Configure NPUSCH Starting Slot.
//10. Configure NPUSCH DMRS.
//11. Select ModAcc measurement and enable Traces.
//12. Configure Measurement Interval.
//13. Configure EVM Unit.
//14. Initiate the Measurement.
//15. Fetch ModAcc Measurements and Traces.
//16. Close RFmx Session.

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

   char * frequencyReferenceSource = RFMXLTE_VAL_ONBOARD_CLOCK_STR;
   float64 frequency = 10e6;                                            /*(Hz) */

   float64 centerFrequency = 1.95e9;                                    /*(Hz) */
   float64 referenceLevel = 0.0;                                        /*(dBm) */
   float64 externalAttenuation = 0.0;                                   /*(dB) */

   int32 enableTrigger = RFMXLTE_VAL_TRUE;
   int32 IQPowerEdgeSlope = RFMXLTE_VAL_IQ_POWER_EDGE_RISING_SLOPE;
   float64 IQPowerEdgeLevel = -20.0;                                    /*(dB) */
   int32 minimumQuiteTimeMode = RFMXLTE_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_AUTO;
   float64 minimumQuietTime = 100e-6;                                   /*(s) */
   int32 IQPowerEdgeLevelType = RFMXLTE_VAL_IQ_POWER_EDGE_TRIGGER_LEVEL_TYPE_RELATIVE;
   float64 triggerDelay = 0.0;                                          /*(s) */

   int32 NCellID = 0;
   int32 uplinkSubcarrierSpacing = RFMXLTE_VAL_NB_IOT_UPLINK_SUBCARRIER_SPACING_15KHZ;
   int32 NPUSCHFormat = 1;
   int32 NPUSCHStartingSlot = 0;

   int32 baseSequenceMode = RFMXLTE_VAL_NPUSCH_DMRS_BASE_SEQUENCE_MODE_AUTO;
   int32 baseSequenceIndex = 0;
   int32 cyclicShift = 0;
   int32 groupHoppingEnabled = RFMXLTE_VAL_NPUSCH_DMRS_GROUP_HOPPING_ENABLED_FALSE;
   int32 deltaSS = 0;

   int32 synchronizationMode = RFMXLTE_VAL_MODACC_SYNCHRONIZATION_MODE_SLOT;
   int32 measurementOffset = 0;                                         /*(slots) */
   int32 measurementLength = 1;                                         /*(slots) */

   int32 evmUnit = RFMXLTE_VAL_MODACC_EVM_UNIT_PERCENTAGE;

   float64 componentCarrierBandwidth = 200e3;                           /*(Hz) */
   float64 componentCarrierFrequency = 0.0;                             /*(Hz) */
   int32 cellID = 0;

   int32 autoNPUSCHChannelDetectionEnabled = RFMXLTE_VAL_AUTO_NPUSCH_CHANNEL_DETECTION_ENABLED_TRUE;

   float64 timeout = 10.000000;                                         /*(s) */

   float64 meanRMSCompositeEVM = 0.0;                                   /*(% or dB) */
   float64 maxPeakCompositeEVM = 0.0;                                   /*(% or dB) */
   int32 peakCompositeEVMSlotIndex = 0;
   int32 peakCompositeEVMSymbolIndex = 0;
   int32 peakCompositeEVMSubcarrierIndex = 0;
   float64 meanFrequencyError = 0.0;                                    /*(Hz) */

   float64 meanIQOriginOffset = 0.0;                                    /*(dBc) */
   float64 meanIQGainImbalance = 0.0;                                   /*(dB) */
   float64 meanIQQuadratureError = 0.0;                                 /*(deg) */

   float64 inBandEmissionMargin = 0.0;                                  /*(dB) */

   NIComplexSingle* dataConstellation = NULL;
   NIComplexSingle* DMRSConstellation = NULL;

   float64 x0 = 0.0, dx = 0.0;
   float32* RMSEVMPerSymbol = NULL;                                    /*(% or dB) */

   int32 actualArraySize = 0, dataConstellationActualArraySize = 0, DMRSConstellationActualArraySize = 0;

   /* Initialize a session */
   RFmxCheckWarn(RFmxLTE_Initialize(resourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxLTE_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, frequency));
   RFmxCheckWarn(RFmxLTE_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, externalAttenuation));
   RFmxCheckWarn(RFmxLTE_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeSlope, IQPowerEdgeLevel,
      triggerDelay, minimumQuiteTimeMode, minimumQuietTime, IQPowerEdgeLevelType, enableTrigger));
   RFmxCheckWarn(RFmxLTE_CfgComponentCarrier(instrumentHandle, "", componentCarrierBandwidth,
      componentCarrierFrequency, cellID));
   RFmxCheckWarn(RFmxLTE_CfgNBIoTComponentCarrier(instrumentHandle, "", NCellID, uplinkSubcarrierSpacing));
   RFmxCheckWarn(RFmxLTE_CfgNPUSCHFormat(instrumentHandle, "", NPUSCHFormat));
   RFmxCheckWarn(RFmxLTE_CfgAutoNPUSCHChannelDetectionEnabled(instrumentHandle, "",
      autoNPUSCHChannelDetectionEnabled));
   RFmxCheckWarn(RFmxLTE_CfgNPUSCHStartingSlot(instrumentHandle, "", NPUSCHStartingSlot));
   RFmxCheckWarn(RFmxLTE_CfgNPUSCHDMRS(instrumentHandle, "", baseSequenceMode, baseSequenceIndex, cyclicShift,
      groupHoppingEnabled, deltaSS));
   RFmxCheckWarn(RFmxLTE_SelectMeasurements(instrumentHandle, "", RFMXLTE_VAL_MODACC, RFMXLTE_VAL_TRUE));
   RFmxCheckWarn(RFmxLTE_ModAccCfgSynchronizationModeAndInterval(instrumentHandle, "", synchronizationMode,
      measurementOffset, measurementLength));
   RFmxCheckWarn(RFmxLTE_ModAccCfgEVMUnit(instrumentHandle, "", evmUnit));
   RFmxCheckWarn(RFmxLTE_Initiate(instrumentHandle, "", ""));

   RFmxCheckWarn(RFmxLTE_ModAccFetchCompositeEVM(instrumentHandle, "", timeout, &meanRMSCompositeEVM,
      &maxPeakCompositeEVM, &meanFrequencyError, &peakCompositeEVMSymbolIndex, &peakCompositeEVMSubcarrierIndex,
      &peakCompositeEVMSlotIndex));
   RFmxCheckWarn(RFmxLTE_ModAccFetchIQImpairments(instrumentHandle, "", timeout,
      &meanIQOriginOffset, &meanIQGainImbalance, &meanIQQuadratureError));
   RFmxCheckWarn(RFmxLTE_ModAccFetchInBandEmissionMargin(instrumentHandle, "", timeout, &inBandEmissionMargin));

   RFmxCheckWarn(RFmxLTE_ModAccFetchNPUSCHConstellationTrace(instrumentHandle, "", timeout, NULL, 0,
      &dataConstellationActualArraySize, NULL, 0, &DMRSConstellationActualArraySize));
   if (dataConstellationActualArraySize > 0 && DMRSConstellationActualArraySize > 0)
   {
      dataConstellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * dataConstellationActualArraySize);
      DMRSConstellation = (NIComplexSingle *)malloc(sizeof(NIComplexSingle) * DMRSConstellationActualArraySize);
      if (dataConstellation && DMRSConstellation)
      {
         RFmxCheckWarn(RFmxLTE_ModAccFetchNPUSCHConstellationTrace(instrumentHandle, "", timeout, dataConstellation,
            dataConstellationActualArraySize, NULL, DMRSConstellation, DMRSConstellationActualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxLTE_ModAccFetchEVMPerSymbolTrace(instrumentHandle, "", timeout, NULL, NULL, NULL,
      0, &actualArraySize));
   if (actualArraySize > 0)
   {
      RMSEVMPerSymbol = (float32*)malloc(sizeof(float32) * actualArraySize);
      if (RMSEVMPerSymbol)
      {
         RFmxCheckWarn(RFmxLTE_ModAccFetchEVMPerSymbolTrace(instrumentHandle, "", timeout, &x0, &dx,
            RMSEVMPerSymbol, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("------------------Measurements------------------\n");
   printf("Mean RMS Composite EVM (%% or dB)        : %lf\n", meanRMSCompositeEVM);
   printf("Max Peak Composite EVM (%% or dB)        : %lf\n", maxPeakCompositeEVM);
   printf("Peak Composite EVM Slot Index           : %d\n", peakCompositeEVMSlotIndex);
   printf("Peak Composite EVM Symbol Index         : %d\n", peakCompositeEVMSymbolIndex);
   printf("Peak Composite EVM Subcarrier Index     : %d\n", peakCompositeEVMSubcarrierIndex);
   printf("Mean Frequency Error (Hz)               : %lf\n", meanFrequencyError);
   printf("Mean IQ Origin Offset (dBc)             : %lf\n", meanIQOriginOffset);
   printf("Mean IQ Gain Imbalance (dB)             : %lf\n", meanIQGainImbalance);
   printf("Mean IQ Quadrature Error (deg)          : %lf\n", meanIQQuadratureError);
   printf("In-Band Emission Margin (dB)            : %lf\n", inBandEmissionMargin);

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
   if (RMSEVMPerSymbol)
   {
      free(RMSEVMPerSymbol);
   }

   printf("Press any key to exit\n");
   _getch();

   return error;
}
