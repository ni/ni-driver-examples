//[1] Steps:
//1. Open an NI - RFSG session.
//2. Configure RFSG frequency reference  & waveform to script mode.
//3. Configure frequency and power level of RF output signal.
//4. Set RFSG External Gain, Power Level Type and Pre-filter Gain.
//5. Export the Marker Event marker0 to the to the terminal specified by the user, which is also used as the source for the digital edge trigger on RFSA.
//6. Configure RFSG LO Source to Automatic SG SA Shared.
//7. Read waveform from file and download it to RFSG.

//[2] Steps to perform ModAcc measurement :
//8. Retrieve the waveform PAPR, Signal Bandwidth and IQ rate. Add the waveform PAPR to the RFSA reference level while configuring to every list step.
//   Configure RFSG Signal Bandwidth, IQ rate, and PAPR. With the signal bandwidth configured and the Upconverter Frequency Offset Mode set to
//   Automatic by default, the RFSG LO is placed outside the signal if the signal bandwidth is less than half of the device instantaneous bandwidth;
//   otherwise, the LO is placed at the center of the signal.
//9. Write script to generate the waveform specified in the script.This script is programmed to generate waveform continuously. The marker0 is configured at sample0.
//10. Initiate signal generation.
//-------------------------------------------------------------------------------------------------------------------------------------------------- -
//11. Open a new RFmx session.
//12. Configure the Frequency Reference properties(Clock Source and Clock Frequency).
//13. Configure the basic signal properties(Center Frequency, Reference Level and External Attenuation).
//14. Configure Digital Edge Trigger properties(Digital Edge Source, Digital Edge, Trigger Delay).
//15. Configure Standard and Channel Bandwidth properties.
//16. Set LO Source to Automatic_SG_SA_Shared.
//17. Set LO Leakage Avoidance Enabled to True.This causes RFmx to place the SA LO outside the measurement bandwidth, if the measurement bandwidth is less than half of the device instantaneous bandwidth; otherwise, the LO is placed at the center of the signal.
//18. Select OFDMModAcc measurement and disable traces.
//19. Configure OFDMModAcc Averaging properties(Averaging Enabled, Averaging Count, Averaging Type, Vector Averaging Time Alignment Enabled, Vector Averaging Phase Alignment Enabled).
//20. Initiate OFDMModAcc measurement.
//21. Fetch OFDMModAcc measurements.

//[3] Steps to perform SEM measurement :
//22. Stop signal generation.
//23.  Initiate signal generation.
//-------------------------------------------------------------------------------------------------------------------------------------------------- -
//24. Select SEM measurement and disable traces.
//25. Configure SEM Averaging properties (Averaging Enabled, Averaging Count, Averaging Type)
//26. Initiate SEM measurement.
//27. Fetch SEM measurements.

// [4] Steps:
//28. Close the RFmx Session.
//29. Close the RFSG session.
//It is recommended to clear the waveform before closing RFSG session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

#include "niRFmxWLAN.h"
#include "niRFSG.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION                4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING                  256

/* CheckWarn macro for RFSG and Playback API calls*/
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

   float64 centerFrequency = 2.412e9;                                            /* (Hz) */

   ViRsrc RFSGResourceName = "RFSG";
   ViConstString waveformFileName = "..\\Support\\WLAN_80211ac_BW-80MHz_SISO.tdms";
   ViConstString waveformName = "Wfm";

   float64 powerLevel = -10.0;                                                   /* (dBm) */
   float64 RFSGExternalAttenuation = 0.0;                                        /* (dB) */

   ViChar* referenceFrequencySource = NIRFSG_VAL_ONBOARD_CLOCK_STR;
   ViReal64 RFSGFrequency = 10.0e6;                                              /* (Hz) */

   char *RFSAResourceName = "RFSA";
   float64 referenceLevel = 0.0;                                                 /* (dBm) */
   float64 RFSAExternalAttenuation = 0.0;                                        /* (dB) */

   char *frequencyReferenceSource = RFMXWLAN_VAL_ONBOARD_CLOCK_STR;
   float64 RFSAFrequency = 10.0e6;                                               /* (Hz) */

   int32 digitalTriggerEnabled = RFMXWLAN_VAL_TRUE;
   int32 digitalEdgeTriggerEdge = RFMXWLAN_VAL_DIGITAL_EDGE_RISING_EDGE;
   ViConstString digitalEdgeSource = RFMXWLAN_VAL_PFI0_STR;
   float64 triggerDelay = 0.0;                                                   /* (s) */
   
   int32 standard = RFMXWLAN_VAL_STANDARD_802_11_AC;
   float64 channelBandwidth = 80e6;                                              /* (Hz) */

   int32 OFDMModAccAveragingEnabled = RFMXWLAN_VAL_OFDMMODACC_AVERAGING_ENABLED_FALSE;
   int32 OFDMModAccAveragingCount = 10;
   int32 OFDMModAccAveragingType = RFMXWLAN_VAL_OFDMMODACC_AVERAGING_TYPE_RMS;
   int32 vectorAveragingTimeAlignmentEnabled = RFMXWLAN_VAL_OFDMMODACC_VECTOR_AVERAGING_TIME_ALIGNMENT_ENABLED_TRUE;
   int32 vectorAveragingPhaseAlignmentEnabled = RFMXWLAN_VAL_OFDMMODACC_VECTOR_AVERAGING_PHASE_ALIGNMENT_ENABLED_TRUE;

   int32 SEMAveragingEnabled = RFMXWLAN_VAL_SEM_AVERAGING_ENABLED_FALSE;
   int32 SEMAveragingCount = 10;
   int32 SEMAveragingType = RFMXWLAN_VAL_SEM_AVERAGING_TYPE_RMS;

   float64 timeout = 10.0;                                                       /* (s) */

   ViReal64 externalGain;
   ViConstString script = "script GenerateWaveform\n  repeat forever\n    generate Wfm marker0(0)\n   end repeat\n  end script";

   float64 compositeRMSEVMMean = 0.0;                                            /* (dB) */
   float64 compositeDataRMSEVMMean = 0.0;                                        /* (dB) */
   float64 compositePilotRMSEVMMean = 0.0;                                       /* (dB) */

   int32 measurementStatus = RFMXWLAN_VAL_SEM_MEASUREMENT_STATUS_FAIL;

   float64 absolutePower = 0.0;                                                  /* (dBm) */

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
   RfsgCheckWarn(niRFSG_ConfigureRefClock(RFSGSession, referenceFrequencySource, RFSGFrequency));
   RfsgCheckWarn(niRFSG_ConfigureGenerationMode(RFSGSession, NIRFSG_VAL_SCRIPT));
   RfsgCheckWarn(niRFSG_SetAttributeViInt32(RFSGSession, "", NIRFSG_ATTR_POWER_LEVEL_TYPE, NIRFSG_VAL_PEAK_POWER));
   RfsgCheckWarn(niRFSG_ConfigureRF(RFSGSession, centerFrequency, powerLevel));
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(RFSGSession, "", NIRFSG_ATTR_ARB_PRE_FILTER_GAIN, -1.5));
   externalGain = -1 * RFSGExternalAttenuation;
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(RFSGSession, "", NIRFSG_ATTR_EXTERNAL_GAIN, externalGain));
   RfsgCheckWarn(niRFSG_ExportSignal(RFSGSession, NIRFSG_VAL_MARKER_EVENT, NIRFSG_VAL_MARKER0,
       digitalEdgeSource));
   RfsgCheckWarn(niRFSG_SetAttributeViString(RFSGSession, "", NIRFSG_ATTR_LO_SOURCE, NIRFSG_VAL_LO_SOURCE_AUTOMATIC_SG_SA_SHARED_STR));
   RfsgCheckWarn(niRFSG_ReadAndDownloadWaveformFromFileTDMS(RFSGSession, waveformName, waveformFileName, 0));
   RfsgCheckWarn(niRFSG_WriteScript(RFSGSession, script));
   RfsgCheckWarn(niRFSG_Initiate(RFSGSession));

   /* Initialize a RFSA session */
   RFmxCheckWarn(RFmxInstr_Initialize(RFSAResourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxInstr_CfgFrequencyReference(instrumentHandle, "", frequencyReferenceSource, RFSAFrequency));
   RFmxCheckWarn(RFmxWLAN_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxWLAN_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxWLAN_CfgExternalAttenuation(instrumentHandle, "", RFSAExternalAttenuation));
   RFmxCheckWarn(RFmxWLAN_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource, digitalEdgeTriggerEdge, 
       triggerDelay, digitalTriggerEnabled));
   RFmxCheckWarn(RFmxWLAN_CfgStandard(instrumentHandle, "", standard));
   RFmxCheckWarn(RFmxWLAN_CfgChannelBandwidth(instrumentHandle, "", channelBandwidth));
   RFmxCheckWarn(RFmxInstr_SetLOSource(instrumentHandle, "", RFMXINSTR_VAL_LO_SOURCE_AUTOMATIC_SG_SA_SHARED));
   RFmxCheckWarn(RFmxInstr_SetLOLeakageAvoidanceEnabled(instrumentHandle, "", RFMXINSTR_VAL_LO_LEAKAGE_AVOIDANCE_ENABLED_TRUE));
   RFmxCheckWarn(RFmxWLAN_SelectMeasurements(instrumentHandle, "", RFMXWLAN_VAL_OFDMMODACC, RFMXWLAN_VAL_FALSE));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccCfgAveraging(instrumentHandle, "", OFDMModAccAveragingEnabled, OFDMModAccAveragingCount));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccSetAveragingType(instrumentHandle, "", OFDMModAccAveragingType));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccSetVectorAveragingTimeAlignmentEnabled(instrumentHandle, "",
       vectorAveragingTimeAlignmentEnabled));
   RFmxCheckWarn(RFmxWLAN_OFDMModAccSetVectorAveragingPhaseAlignmentEnabled(instrumentHandle, "", 
       vectorAveragingPhaseAlignmentEnabled));
   RFmxCheckWarn(RFmxWLAN_Initiate(instrumentHandle, "", ""));

   RFmxCheckWarn(RFmxWLAN_OFDMModAccFetchCompositeRMSEVM(instrumentHandle, "", timeout, &compositeRMSEVMMean,
      &compositeDataRMSEVMMean, &compositePilotRMSEVMMean));

   RfsgCheckWarn(niRFSG_Abort(RFSGSession));
   RfsgCheckWarn(niRFSG_Initiate(RFSGSession));

   RFmxCheckWarn(RFmxWLAN_SelectMeasurements(instrumentHandle, "", RFMXWLAN_VAL_SEM, RFMXWLAN_VAL_FALSE));
   RFmxCheckWarn(RFmxWLAN_SEMCfgAveraging(instrumentHandle, "", SEMAveragingEnabled, SEMAveragingCount, SEMAveragingType));
   RFmxCheckWarn(RFmxWLAN_Initiate(instrumentHandle, "", ""));

   RFmxCheckWarn(RFmxWLAN_SEMFetchMeasurementStatus(instrumentHandle, "", timeout, &measurementStatus));
   RFmxCheckWarn(RFmxWLAN_SEMFetchCarrierMeasurement(instrumentHandle, "", timeout, &absolutePower, NULL));
   RFmxCheckWarn(RFmxWLAN_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout, NULL,
      NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      lowerOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * actualArraySize);
      lowerOffsetMargin = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
      lowerOffsetMarginRelativePower = (float64 *)malloc(sizeof(float64) * actualArraySize);

      RFmxCheckWarn(RFmxWLAN_SEMFetchLowerOffsetMarginArray(instrumentHandle, "", timeout,
         lowerOffsetMeasurementStatus, lowerOffsetMargin, lowerOffsetMarginFrequency,
         lowerOffsetMarginAbsolutePower, lowerOffsetMarginRelativePower, actualArraySize, NULL));
   }

   actualArraySize = 0;
   RFmxCheckWarn(RFmxWLAN_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout, NULL,
      NULL, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      upperOffsetMeasurementStatus = (int32 *)malloc(sizeof(int32) * actualArraySize);
      upperOffsetMargin = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperOffsetMarginFrequency = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperOffsetMarginAbsolutePower = (float64 *)malloc(sizeof(float64) * actualArraySize);
      upperOffsetMarginRelativePower = (float64 *)malloc(sizeof(float64) * actualArraySize);

      RFmxCheckWarn(RFmxWLAN_SEMFetchUpperOffsetMarginArray(instrumentHandle, "", timeout,
         upperOffsetMeasurementStatus, upperOffsetMargin, upperOffsetMarginFrequency,
         upperOffsetMarginAbsolutePower, upperOffsetMarginRelativePower, actualArraySize, &arraySize));
   }

   /*Print Results */
   printf("------------------OFDMModAcc------------------\n\n");
   printf("------------------Composite EVM------------------\n");
   printf("RMS EVM Mean (dB)                       : %lf\n", compositeRMSEVMMean);
   printf("Data RMS EVM Mean (dB)                  : %lf\n", compositeDataRMSEVMMean);
   printf("Pilot RMS EVM Mean (dB)                 : %lf\n\n", compositePilotRMSEVMMean);

   printf("\n------------------SEM------------------\n\n");
   printf("Measurement Status                   : %s\n",
      measurementStatus == RFMXWLAN_VAL_SEM_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
   printf("Carrier Absolute Power (dBm)         : %lf\n", absolutePower);
   printf("\n----------Lower Offset Measurements----------\n\n");
   for (i = 0; i < arraySize; i++)
   {
      printf("Offset %d\n", i);
      printf("Measurement Status                   : %s\n",
         lowerOffsetMeasurementStatus[i] == RFMXWLAN_VAL_SEM_LOWER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
      printf("Margin (dB)                          : %lf\n", lowerOffsetMargin[i]);
      printf("Margin Frequency (Hz)                : %lf\n", lowerOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)          : %lf\n\n", lowerOffsetMarginAbsolutePower[i]);
   }
   printf("\n----------Upper Offset Measurements----------\n\n");
   for (i = 0; i < arraySize; i++)
   {
      printf("Offset %d\n", i);
      printf("Measurement Status                   : %s\n",
         upperOffsetMeasurementStatus[i] == RFMXWLAN_VAL_SEM_UPPER_OFFSET_MEASUREMENT_STATUS_PASS ? "PASS" : "FAIL");
      printf("Margin (dB)                          : %lf\n", upperOffsetMargin[i]);
      printf("Margin Frequency (Hz)                : %lf\n", upperOffsetMarginFrequency[i]);
      printf("Margin Absolute Power (dBm)          : %lf\n\n", upperOffsetMarginAbsolutePower[i]);
   }

Error:
   if (error)
   {
      errorOccured = error;
      RFmxWLAN_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
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
      RFmxWLAN_Close(instrumentHandle, RFMXWLAN_VAL_FALSE);
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