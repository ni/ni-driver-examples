//[1] Steps:
//1. Open an NI - RFSG session.
//2. Configure RFSG Selected Ports and waveform generation to Script mode.
//3. Configure RFSG frequency reference.
//4. Configure frequency and power level of RF output signal.
//5. Cofigure RFSG External Gain, Power Level Type and Pre-filter Gain.
//  #4 and #5 ensure that the average power of the signal at the input of the DUT
//  matches the user configured DUT Average Input Power.
//6. Configure RFSG LO Source to Automatic SG SA Shared.
//7. Read waveform from file and download it to RFSG.
//
//[2] Steps to perform ModAcc measurement:
//8. Retrieve the waveform PAPR, Signal Bandwidth and IQ rate. 
//  Configure RFSG Signal Bandwidth, IQ rate, and PAPR.
//  With the signal bandwidth configured and the Upconverter Frequency Offset Mode set to Automatic by default,
//  the RFSG LO is placed outside the signal, if signal bandwidth is less than half of the device instantaneous bandwidth;
//  otherwise, the LO is placed at the center of the signal.
//9. Write script to generate the waveform specified in the script. This script is programmed to generate waveform continuously.
//10. Initiate signal generation.
//-------------------------------------------------------------------------------------------------------------------------------
//11.Open a new RFmx Session.
//12. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
//13.Configure Selected Ports.
//14.Configure Automatic SG SA Shared LO to Enabled.
//15. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
//16. Configure Trigger Type and Trigger Parameters.
//17. Configure Link Direction, Frequency Range, CC bandwidth, Cell ID, Band and BWP Subcarrier Spacing.
//18. Set LO Leakage Avoidance Enabled to True. This causes RFmx to place the SA LO outside the measurement bandwidth,
//   if the measurement bandwidth is less than half of the device instantaneous bandwidth;
//otherwise, the LO is placed at the center of the signal.
//19. Select ModAcc measurement and disable Traces.
//20. Initiate ModAcc measurement.
//21. Fetch ModAcc measurements.
//
//[3] Steps to perform SEM measurement:
//22. Stop signal generation.
//23. Write script to generate the waveform specified in the script. This script is programmed to generate waveform continuously.
//24. Initiate signal generation.
//-------------------------------------------------------------------------------------------------------------------------------
//25. Select SEM measurement and disable the traces.
//26. Initiate SEM measurement.
//27. Fetch SEM measurements.
//
//[4] Steps:
//28. Close the RFmx Session.
//29. Close the RFSG session.
//It is recommended to clear the waveform before closing RFSG session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxNR.h"
#include "niRFSG.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                  256

/* CheckWarn macro for RFSG API calls*/
#define RfsgCheckWarn(fCall)     if (1) {ViStatus _code_; if (_code_ = (fCall), _code_ < 0)    \
                                    {RFSGError = _code_;goto Error;}        \
                                    else RFSGError = (RFSGError==0)?_code_:RFSGError;}    \
                                    else RFSGError = RFSGError


int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   ViSession RFSGSession = VI_NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION] = { 0 };
   int32 error = 0, lastErrorCode = 0, errorOccured = 0;
   int32 RFSGError = 0;
   int i = 0;

   float64 centerFrequency = 3.5e9;                                              /* (Hz) */

   ViRsrc RFSGResourceName = "RFSG";
   ViConstString RFSGSelectedPorts = "";
   ViConstString waveformFileName = "..\\Support\\NR_FR2_UL_SISO_CC-1_BW-50MHz_SCS-120kHz.tdms";
   ViConstString waveformName = "Wfm";

   float64 powerLevel = -10.0;                                                   /* (dBm) */
   float64 RFSGExternalAttenuation = 0.0;                                        /* (dB) */

   ViChar* RFSGFrequencyReferenceSource = NIRFSG_VAL_ONBOARD_CLOCK_STR;
   ViReal64 RFSGFrequency = 10.0e6;                                              /* (Hz) */

   char* RFSAResourceName = "RFSA";
   char* RFSASelectedPorts = "";
   float64 referenceLevel = 0.0;                                                 /* (dBm) */
   float64 RFSAExternalAttenuation = 0.0;                                        /* (dB) */

   char* RFSAFrequencyReferenceSource = RFMXNR_VAL_ONBOARD_CLOCK_STR;
   float64 RFSAFrequency = 10.0e6;                                               /* (Hz) */

   int32 enableTrigger = RFMXNR_VAL_FALSE;
   char* digitalEdgeSource = RFMXNR_VAL_PXI_TRIG0_STR;
   int32 digitalEdge = RFMXNR_VAL_DIGITAL_EDGE_RISING_EDGE;
   float64 triggerDelay = 0.0;                                                   /* (s) */

   int32 frequencyRange = RFMXNR_VAL_FREQUENCY_RANGE_RANGE2;
   float64 carrierBandwidth = 50e6;                                              /* (Hz) */
   float64 subcarrierSpacing = 120e3;                                            /* (Hz) */
   int32 band = 257;
   int32 cellID = 0;

   float64 timeout = 10.0;                                                       /* (s) */

   ViReal64 externalGain;
   ViConstString script = "script GenerateWaveform\n  repeat forever\n    generate Wfm\n   end repeat\n  end script";

   float64 compositeRMSEVMMean = 0.0;                                            /* (%) */
   float64 compositePeakEVMMaximum = 0.0;                                        /* (%) */

   int32 measurementStatus = RFMXNR_VAL_SEM_MEASUREMENT_STATUS_FAIL;

   float64 absolutePower = 0.0;                                                  /* (dBm) */
   float64 relativePower = 0.0;
   float64 peakFrequency = 0.0;
   float64 peakAbsolutePower = 0.0;

   float64 *lowerOffsetMarginRelativePower = NULL;
   float64 *lowerOffsetMarginAbsolutePower = NULL;                               /* (dBm) */
   float64 *lowerOffsetMargin = NULL;                                            /* (dB) */
   float64 *lowerOffsetMarginFrequency = NULL;                                   /* (Hz) */
   int32 *lowerOffsetMeasurementStatus = NULL;

   float64 *upperOffsetMarginRelativePower = NULL;
   float64 *upperOffsetMarginAbsolutePower = NULL;                               /* (dBm) */
   float64 *upperOffsetMargin = NULL;                                            /* (dB) */
   float64 *upperOffsetMarginFrequency = NULL;                                   /* (Hz) */
   int32 *upperOffsetMeasurementStatus = NULL;

   int32 arraySize = 0;
   int32 actualArraySize = 0;

   /* Initialize a RFSG session */
   RfsgCheckWarn(niRFSG_init(RFSGResourceName, VI_TRUE, VI_FALSE, &RFSGSession));
   RfsgCheckWarn(niRFSG_ConfigureGenerationMode(RFSGSession, NIRFSG_VAL_SCRIPT));
   RfsgCheckWarn(niRFSG_SetAttributeViString(RFSGSession, "", NIRFSG_ATTR_SELECTED_PORTS, RFSGSelectedPorts));
   RfsgCheckWarn(niRFSG_ConfigureRefClock(RFSGSession, RFSGFrequencyReferenceSource, RFSGFrequency));
   RfsgCheckWarn(niRFSG_ConfigureRF(RFSGSession, centerFrequency, powerLevel));
   externalGain = -1 * RFSGExternalAttenuation;
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(RFSGSession, "", NIRFSG_ATTR_EXTERNAL_GAIN, externalGain));
   RfsgCheckWarn(niRFSG_ReadAndDownloadWaveformFromFileTDMS(RFSGSession, waveformName, waveformFileName, 0));
   RfsgCheckWarn(niRFSG_SetAttributeViString(RFSGSession, "", NIRFSG_ATTR_LO_SOURCE, NIRFSG_VAL_LO_SOURCE_AUTOMATIC_SG_SA_SHARED_STR));
   RfsgCheckWarn(niRFSG_WriteScript(RFSGSession, script));
   RfsgCheckWarn(niRFSG_Initiate(RFSGSession));

   /* Initialize a RFSA session */
   RFmxCheckWarn(RFmxInstr_Initialize(RFSAResourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxInstr_CfgFrequencyReference(instrumentHandle, "", RFSAFrequencyReferenceSource, RFSAFrequency));
   RFmxCheckWarn(RFmxInstr_SetLOSource(instrumentHandle, "", RFMXINSTR_VAL_LO_SOURCE_AUTOMATIC_SG_SA_SHARED));
   RFmxCheckWarn(RFmxNR_SetSelectedPorts(instrumentHandle, "", RFSASelectedPorts));
   RFmxCheckWarn(RFmxNR_CfgRF(instrumentHandle, "", centerFrequency, referenceLevel, RFSAExternalAttenuation));
   RFmxCheckWarn(RFmxNR_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdge,
      triggerDelay, enableTrigger));
   RFmxCheckWarn(RFmxNR_SetLinkDirection(instrumentHandle, "", RFMXNR_VAL_LINK_DIRECTION_UPLINK));
   RFmxCheckWarn(RFmxNR_SetFrequencyRange(instrumentHandle, "", frequencyRange));
   RFmxCheckWarn(RFmxNR_SetComponentCarrierBandwidth(instrumentHandle, "", carrierBandwidth));
   RFmxCheckWarn(RFmxNR_SetCellID(instrumentHandle, "", cellID));
   RFmxCheckWarn(RFmxNR_SetBand(instrumentHandle, "", band));
   RFmxCheckWarn(RFmxNR_SetBandwidthPartSubcarrierSpacing(instrumentHandle, "", subcarrierSpacing));
   RFmxCheckWarn(RFmxInstr_SetLOLeakageAvoidanceEnabled(instrumentHandle, "", RFMXINSTR_VAL_LO_LEAKAGE_AVOIDANCE_ENABLED_TRUE));
   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_MODACC, RFMXNR_VAL_FALSE));
   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

   RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositeRMSEVMMean(instrumentHandle, "", &compositeRMSEVMMean));
   RFmxCheckWarn(RFmxNR_ModAccGetResultsCompositePeakEVMMaximum(instrumentHandle, "", &compositePeakEVMMaximum));

   RfsgCheckWarn(niRFSG_Abort(RFSGSession));
   RfsgCheckWarn(niRFSG_WriteScript(RFSGSession, script));
   RfsgCheckWarn(niRFSG_Initiate(RFSGSession));

   RFmxCheckWarn(RFmxNR_SelectMeasurements(instrumentHandle, "", RFMXNR_VAL_SEM, RFMXNR_VAL_FALSE));
   RFmxCheckWarn(RFmxNR_Initiate(instrumentHandle, "", ""));

   RFmxCheckWarn(RFmxNR_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));
   RFmxCheckWarn(RFmxNR_SEMFetchComponentCarrierMeasurement(instrumentHandle, "", timeout, &absolutePower, &peakAbsolutePower,
      &peakFrequency, &relativePower));
   RFmxCheckWarn(RFmxNR_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL,
      NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      lowerOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * actualArraySize);
      lowerOffsetMargin = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerOffsetMarginRelativePower = (float64 *)malloc(sizeof(float64) * actualArraySize);

      RFmxCheckWarn(RFmxNR_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout,
         lowerOffsetMeasurementStatus, lowerOffsetMargin, lowerOffsetMarginFrequency,
         lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower, actualArraySize, NULL));
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxNR_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL,
      NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      upperOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * actualArraySize);
      upperOffsetMargin = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperOffsetMarginRelativePower = (float64 *)malloc(sizeof(float64) * actualArraySize);

      RFmxCheckWarn(RFmxNR_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout,
         upperOffsetMeasurementStatus, upperOffsetMargin, upperOffsetMarginFrequency,
         upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower, actualArraySize, &arraySize));
   }

   /* Print Results */
   printf("------------------ModAcc------------------\n\n");
   printf("------------------Measurement------------------\n\n");
   printf("Composite RMS EVM Mean (%%)                    : %lf\n", compositeRMSEVMMean);
   printf("Composite Peak EVM Maximum (%%)                : %lf\n", compositePeakEVMMaximum);

   printf("\n------------------SEM------------------\n\n");
   printf("Measurement Status                         : %s\n",
      measurementStatus == RFMXNR_VAL_SEM_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
   printf("Carrier Absolute Integrated Power (dBm)    : %lf\n", absolutePower);
   printf("\n----------Lower Offset Segment Measurements----------\n\n");
   for (i = 0; i < arraySize; i++)
   {
      printf("Offset %d\n", i);
      printf("Measurement Status                   : %s\n",
         lowerOffsetMeasurementStatus[i] == RFMXNR_VAL_SEM_LOWER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
      printf("Margin (dB)                          : %lf\n", lowerOffsetMargin[i]);
      printf("Margin Frequency (Hz)                : %lf\n", lowerOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)          : %lf\n\n", lowerOffsetMarginAbsolutePower[i]);
   }
   printf("\n----------Upper Offset Segment Measurements----------\n\n");
   for (i = 0; i < arraySize; i++)
   {
      printf("Offset %d\n", i);
      printf("Measurement Status                   : %s\n",
         upperOffsetMeasurementStatus[i] == RFMXNR_VAL_SEM_UPPER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
      printf("Margin (dB)                          : %lf\n", upperOffsetMargin[i]);
      printf("Margin Frequency (Hz)                : %lf\n", upperOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)          : %lf\n\n", upperOffsetMarginAbsolutePower[i]);
   }

Error:
   if (error)
   {
      errorOccured = error;
      RFmxNR_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (RFSGError)
   {
      errorOccured = RFSGError;
      niRFSG_GetError(RFSGSession, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (RFSGError < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (instrumentHandle)
   {
      RFmxNR_Close(instrumentHandle, RFMXNR_VAL_FALSE);
   }
   if (RFSGSession)
   {
      niRFSG_Abort(RFSGSession);
      niRFSG_ClearArbWaveform(RFSGSession, waveformName);
      niRFSG_close(RFSGSession);
   }

   /* Free allocated memory */
   if (lowerOffsetMeasurementStatus)
      free(lowerOffsetMeasurementStatus);
   if (lowerOffsetMarginAbsolutePower)
      free(lowerOffsetMarginAbsolutePower);
   if (lowerOffsetMarginFrequency)
      free(lowerOffsetMarginFrequency);
   if (lowerOffsetMarginRelativePower)
      free(lowerOffsetMarginRelativePower);
   if (lowerOffsetMargin)
      free(lowerOffsetMargin);
   if (upperOffsetMeasurementStatus)
      free(upperOffsetMeasurementStatus);
   if (upperOffsetMarginAbsolutePower)
      free(upperOffsetMarginAbsolutePower);
   if (upperOffsetMarginFrequency)
      free(upperOffsetMarginFrequency);
   if (upperOffsetMarginRelativePower)
      free(upperOffsetMarginRelativePower);
   if (upperOffsetMargin)
      free(upperOffsetMargin);

   printf("\nPress any key to exit\n");
   _getch();

   return errorOccured;
}