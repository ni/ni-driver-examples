/* Steps:
1. Open RFSG session.
2. Configure RFSG frequency reference and Generation mode to Script.
3. Configure marker0 to be generated from RFSG on the specified output terminal.
4. Configure frequency and power level of RF output signal.
5. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at the input of the DUT
   matches the user configured DUT Average Input Power.
6. Read waveform from file and download Waveform from file to RFSG.
   Set RFSG IQ Rate and Pre-filter Gain.
   Read waveform sample rate, multiply by 0.8 and set the result to the signal bandwidth.
   Write script to generate the waveform specified in the script. This script is programmed
   to generate waveform continuously, with marker0 aligned to sample index 0.
7. Initiate generation.
8. Open RFmx session.
9. Configure frequency reference of the analyser.
10. Configure Selected Ports.
11. Configure trigger to use as reference for signal acquisition.
12. Configure center frequency and external attenuation.
13. Select DPD measurement, configure the reference waveform
    and power of this signal at the input of the DUT.
14. Select and configure the Lookup Table (LUT) model to estimate the predistotor.
15. Set the measurement sample rate and the measurement interval to use for analysis.
16. Set Reference Level or perform Auto Level to compute an approximate reference level
    to use by the analyser.
17. Initiate DPD measurement.
18. Configure LUT correction type and Apply Digital Predistortion based on LUT computed
    by the DPD measurement.
19. a. Abort RFSG generation and write a new waveform that is predistorted by applying LUT.
       Set Waveform Runtime Scaling to the negative of the desired Pre-filter Gain.
       Set the sample rate computed from Apply Digital Predistortion.
       Set the final PAPR to the sum of the actual PAPR and the Power Offset as computed
       by Apply Digital Predistortion.
       Set the Signal Bandwidth.
       Initiate RFSG generation using the script that was selected earlier.
    b. Fetch DPD Lookup Table.
20. Select and configure AMPM measurement in RFmx after DPD measurement is complete.
    AMPM measurement is used to inspect the measure the AM-AM and AM-PM response of the DUT.
21. Initiate and fetch AMPM results.
22. Close RFmx session.
23. Close RFSG session.
It is recommended to clear the waveform before closing RFSG session.*/

#include <stdlib.h>
#include <stdio.h>
#include <conio.h>
#include "niRFmxSpecAn.h"
#include "niRFSGPlayback.h"
#include "niRFSG.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

/* CheckWarn macro for RFSG API calls*/
#define RfsgCheckWarn(fCall)     if (1) {ViStatus _code_; if (_code_ = (fCall), _code_ < 0)    \
                                 {rfsgError = _code_;goto Error;}        \
                                 else rfsgError = (rfsgError==0)?_code_:rfsgError;} else rfsgError = rfsgError

#define playbackCheckWarn(fCall)    if (1) {ViStatus _code_; if (_code_ = (fCall), _code_ < 0)    \
                                    {playbackError = _code_;goto Error;}        \
                                    else playbackError = (playbackError==0)?_code_:playbackError;} \
                                    else playbackError = playbackError

/* Auto Level */
#define AUTO_LEVEL_OFF              0
#define AUTO_LEVEL_ON               1

/* Trigger */
#define IQ_POWER_EDGE               0
#define DIGITAL_EDGE                1

float64 x0 = 0.0, dx = 0.0;
int32 numberOfSamples = 0;
float64 PAPR = 0.00;                                         /* dB */
NIComplexSingle *referenceWaveformF32 = NULL;
int32 error = 0, errorOccured = 0, playbackError = 0, lastErrorCode = 0;
char errorMessage[MAX_ERROR_DESCRIPTION];

int32 ReadFromTDMSFile(char *fileName)
{
   int i = 0;
   ViReal64 x0v = 0, dxv = 0;
   playbackCheckWarn(niRFSGPlayback_ReadWaveformFromFileComplexF32(fileName, 0, NULL, NULL, NULL, &numberOfSamples));
   if (numberOfSamples > 0)
   {
      referenceWaveformF32 = (NIComplexSingle*)malloc(sizeof(NIComplexSingle)*numberOfSamples);
      if (referenceWaveformF32)
         playbackCheckWarn(niRFSGPlayback_ReadWaveformFromFileComplexF32(fileName, numberOfSamples, &x0v, &dxv,
         (NIComplexNumberF32*)referenceWaveformF32, &numberOfSamples));
      else
      {
         printf("malloc failed\n");
         return -1;
      }
   }

   x0 = (float64)x0v;
   dx = (float64)dxv;

Error:
   if (playbackError)
   {
      errorOccured = playbackError;
      niRFSGPlayback_GetError(&lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (playbackError < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }
   return errorOccured;
}

int main(int argc, char *argv[])
{
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;
   float64 DUTAverageInputPower = -20.00;                         /*dBm*/

   ViRsrc rfsgResourceName = "RFSG";
   ViSession  rfsgSession = VI_NULL;
   ViChar* refClockSource = NIRFSG_VAL_ONBOARD_CLOCK_STR;
   ViReal64 rfsgFrequency = 10e+6;                                /*Hz*/
   ViChar *markerEventOutputTerminal = NIRFSG_VAL_PXI_TRIG0_STR;
   ViChar* script = "script LUTDPDScript\n   repeat forever\n  generate Wfm marker0(0)\n  end repeat\nend script";
   ViReal64 rfsgExternalAttenuation = 0.00;                       /* dB */
   ViReal64 externalGain;
   ViReal64 preFilterGain = -4.00;                                /* dB */
   ViReal64 runtimeScaling = preFilterGain;
   char waveFormFileName[] = "..\\Support\\LTE20MHz Waveform (Two Subframes).tdms";
   char waveformName[] = "Wfm";

   niRFmxInstrHandle instrumentHandle = NULL;
   char *rfsaResourceName = "RFSA";
   char *rfsaFrequencySource = RFMXSPECAN_VAL_ONBOARD_CLOCK_STR;
   float64 rfsafrequency = 10e+6;                                 /*Hz*/

   int32 trigger = DIGITAL_EDGE;
   float64 IQPowerEdgeLevel = -20.00;
   float64 minQuietTime = 0.00;                                   /*seconds*/
   float64 rfsaExternalAttenuation = 0.00;                        /* dB */
   float64 triggerDelay = 0.00;                                   /*seconds*/
   int32 digitalEdge = 0;
   int32 enableTrigger = RFMXSPECAN_VAL_TRUE;
   int32 IQPowerEdgeSlope = 0;
   int32 minQuietTimeMode = 0;

   float64 referenceLevel = -14.00;                               /* dBm */

   int32 idleDurationPresent = RFMXSPECAN_VAL_DPD_REFERENCE_WAVEFORM_IDLE_DURATION_PRESENT_FALSE;
   int32 signalType = RFMXSPECAN_VAL_DPD_SIGNAL_TYPE_MODULATED;

   int32 sampleRateMode = RFMXSPECAN_VAL_DPD_MEASUREMENT_SAMPLE_RATE_MODE_REFERENCE_WAVEFORM;
   float64 sampleRate = 120e+6;                                   /*S/s*/

   float64 measurementInterval = 100e-6;                          /*seconds*/

   int32 thresholdEnabled = RFMXSPECAN_VAL_DPD_LOOKUP_TABLE_THRESHOLD_ENABLED_TRUE;
   float64 thresholdLevel = -20.00;                               /* dBm */
   int32 thresholdType = RFMXSPECAN_VAL_DPD_LOOKUP_TABLE_THRESHOLD_TYPE_RELATIVE;

   int32 referencePowerType = RFMXSPECAN_VAL_AMPM_REFERENCE_POWER_TYPE_INPUT;

   float64 LUTStepSize = 0.1;

   int32 autoLevel = AUTO_LEVEL_ON;
   float64 measurementIntervalAutoLevel = 100e-6;
   float64 signalBandwidth = 20e+6;                               /*Hz*/
   float64 autoReferenceLevel = 0;                                /* dBm */

   int32 LUTCorrectionType = RFMXSPECAN_VAL_DPD_APPLY_DPD_LOOKUP_TABLE_CORRECTION_TYPE_MAGNITUDE_AND_PHASE;

   int32 actualArraySize = 0;
   int32 rfsgError = 0;
   float64 timeout = 10.0;                                        /*seconds*/

   int32 waveformOutSize = 0, actualWaveformSize = 0;
   float64 maxOutputSize, outputSampleRate;

   float64 x0Out = 0.00;
   float64 dxOut = 0.00;
   NIComplexSingle	*waveformWithDPDF32 = NULL;

   float64 meanLinearGain = 0.00;
   float64 onedBCompressionPoint = 0.00;
   float64 meanRMSEVM = 0.00;
   float64 gainErrorRange = 0.00;
   float64 phaseErrorRange = 0.00;
   float64 meanPhaseError = 0.00;
   float64 powerOffset = 0.00;

   float32 *inputPowers = NULL;
   NIComplexSingle *complexGains = NULL;

   float32 *referencePowersAMToAM = NULL;
   float32 *measuredAMToAM = NULL;
   float32 *curveFitAMToAM = NULL;

   float32 *referencePowersAMToPM = NULL;
   float32 *measuredAMToPM = NULL;
   float32 *curveFitAMToPM = NULL;

   float64 AMToAMResidual = 0;
   float64 AMToPMResidual = 0;

   errorOccured = ReadFromTDMSFile(waveFormFileName);
   if (errorOccured)
   {
      printf("Cannot open the specified waveform file\n");
      goto Exit;
   }

   RfsgCheckWarn(niRFSG_init(rfsgResourceName, VI_TRUE, VI_FALSE, &rfsgSession));
   RfsgCheckWarn(niRFSG_ConfigureRefClock(rfsgSession, refClockSource, rfsgFrequency));
   RfsgCheckWarn(niRFSG_ConfigureGenerationMode(rfsgSession, NIRFSG_VAL_SCRIPT));
   RfsgCheckWarn(niRFSG_SetAttributeViInt32(rfsgSession, "", NIRFSG_ATTR_POWER_LEVEL_TYPE, NIRFSG_VAL_PEAK_POWER));
   RfsgCheckWarn(niRFSG_ExportSignal(rfsgSession, NIRFSG_VAL_MARKER_EVENT, NIRFSG_VAL_MARKER0,
      markerEventOutputTerminal));
   RfsgCheckWarn(niRFSG_ConfigureRF(rfsgSession, centerFrequency, DUTAverageInputPower));
   externalGain = -1 * rfsgExternalAttenuation;
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_EXTERNAL_GAIN, externalGain));
   RfsgCheckWarn(niRFSG_ReadAndDownloadWaveformFromFileTDMS(rfsgSession, waveformName, waveFormFileName, 0));
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_ARB_PRE_FILTER_GAIN, runtimeScaling));
   RfsgCheckWarn(niRFSG_GetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_IQ_RATE, &sampleRate));
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_SIGNAL_BANDWIDTH, 0.8*sampleRate));
   RfsgCheckWarn(niRFSG_WriteScript(rfsgSession, script));
   RfsgCheckWarn(niRFSG_Initiate(rfsgSession));

   /* Initialize a session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", rfsaFrequencySource, rfsafrequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   if (trigger == DIGITAL_EDGE)
   {
      RFmxCheckWarn(RFmxSpecAn_CfgDigitalEdgeTrigger(instrumentHandle, "", "PXI_Trig0", digitalEdge,
         triggerDelay, enableTrigger));
   }
   else
   {
      RFmxCheckWarn(RFmxSpecAn_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeLevel, IQPowerEdgeSlope,
         triggerDelay, minQuietTimeMode, minQuietTime, enableTrigger));
   }
   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, 0, rfsaExternalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_DPD, RFMXSPECAN_VAL_TRUE));

   RFmxCheckWarn(RFmxSpecAn_DPDCfgReferenceWaveform(instrumentHandle, "", x0, dx, referenceWaveformF32,
      numberOfSamples, idleDurationPresent, signalType));
   RFmxCheckWarn(RFmxSpecAn_DPDCfgDUTAverageInputPower(instrumentHandle, "", DUTAverageInputPower));
   RFmxCheckWarn(RFmxSpecAn_DPDCfgDPDModel(instrumentHandle, "", RFMXSPECAN_VAL_DPD_MODEL_LOOKUP_TABLE));
   RFmxCheckWarn(RFmxSpecAn_DPDCfgLookupTableThreshold(instrumentHandle, "", thresholdEnabled, thresholdLevel,
      thresholdType));
   RFmxCheckWarn(RFmxSpecAn_DPDCfgLookupTableStepSize(instrumentHandle, "", LUTStepSize));
   RFmxCheckWarn(RFmxSpecAn_DPDCfgMeasurementSampleRate(instrumentHandle, "", sampleRateMode, sampleRate));
   RFmxCheckWarn(RFmxSpecAn_DPDCfgMeasurementInterval(instrumentHandle, "", measurementInterval));
   if (autoLevel == AUTO_LEVEL_ON)
   {
      RFmxCheckWarn(RFmxSpecAn_AutoLevel(instrumentHandle, "", signalBandwidth, measurementIntervalAutoLevel,
         &autoReferenceLevel));
      printf("Reference level : %f\n", autoReferenceLevel);
   }
   else
   {
      RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   }
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));
   RFmxCheckWarn(RFmxSpecAn_DPDCfgApplyDPDLookupTableCorrectionType(instrumentHandle, "", LUTCorrectionType));

   /* Calculations to get waveformOut size*/
   if (sampleRateMode == RFMXSPECAN_VAL_DPD_MEASUREMENT_SAMPLE_RATE_MODE_REFERENCE_WAVEFORM)
      outputSampleRate = 1 / dx;
   else
      outputSampleRate = sampleRate;

   maxOutputSize = outputSampleRate * dx * numberOfSamples;
   waveformOutSize = (int32)maxOutputSize;
   if (waveformOutSize > 0)
   {
      waveformWithDPDF32 = (NIComplexSingle*)malloc(sizeof(NIComplexSingle)* waveformOutSize);
      if (waveformWithDPDF32)
      {
         RFmxCheckWarn(RFmxSpecAn_DPDApplyDigitalPredistortion(instrumentHandle, "", x0, dx, referenceWaveformF32,
            numberOfSamples, idleDurationPresent, timeout, &x0Out, &dxOut, waveformWithDPDF32, waveformOutSize,
            &actualWaveformSize, &PAPR, &powerOffset));
      }
      else
      {
         printf("malloc failed\n");
         goto Error;
      }
   }

   RfsgCheckWarn(niRFSG_Abort(rfsgSession));
   RfsgCheckWarn(niRFSG_ClearArbWaveform(rfsgSession, waveformName));

   RfsgCheckWarn(niRFSG_WriteArbWaveformComplexF32(rfsgSession, waveformName, actualWaveformSize,
      (NIComplexNumberF32*)waveformWithDPDF32, VI_FALSE));
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_ARB_PRE_FILTER_GAIN, runtimeScaling));
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_IQ_RATE, 1 / dxOut));
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_PEAK_POWER_ADJUSTMENT, (ViReal64)(PAPR + powerOffset)));
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_SIGNAL_BANDWIDTH, 0.8*(1 / dxOut)));
   RfsgCheckWarn(niRFSG_WriteScript(rfsgSession, script));
   RfsgCheckWarn(niRFSG_Initiate(rfsgSession));

   RFmxCheckWarn(RFmxSpecAn_DPDFetchLookupTable(instrumentHandle, "", timeout, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      inputPowers = (float32 *)malloc(sizeof(float32)* actualArraySize);
      complexGains = (NIComplexSingle *)malloc(sizeof(NIComplexSingle)* actualArraySize);
      if (inputPowers && complexGains)
      {
         RFmxCheckWarn(RFmxSpecAn_DPDFetchLookupTable(instrumentHandle, "", timeout, inputPowers, complexGains,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_AMPM, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgMeasurementSampleRate(instrumentHandle, "",
      RFMXSPECAN_VAL_AMPM_MEASUREMENT_SAMPLE_RATE_MODE_REFERENCE_WAVEFORM, sampleRate));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgMeasurementInterval(instrumentHandle, "", measurementInterval));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgReferenceWaveform(instrumentHandle, "", x0, dx, referenceWaveformF32,
      numberOfSamples, idleDurationPresent, signalType));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgThreshold(instrumentHandle, "", thresholdEnabled, thresholdLevel, thresholdType));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgDUTAverageInputPower(instrumentHandle, "", DUTAverageInputPower));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgReferencePowerType(instrumentHandle, "", referencePowerType));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   RFmxCheckWarn(RFmxSpecAn_AMPMFetchDUTCharacteristics(instrumentHandle, "", timeout, &meanLinearGain,
      &onedBCompressionPoint, &meanRMSEVM));
   RFmxCheckWarn(RFmxSpecAn_AMPMFetchError(instrumentHandle, "", timeout, &gainErrorRange, &phaseErrorRange,
      &meanPhaseError));
   RFmxCheckWarn(RFmxSpecAn_AMPMFetchCurveFitResidual(instrumentHandle, "", timeout, &AMToAMResidual,
      &AMToPMResidual));
   RFmxCheckWarn(RFmxSpecAn_AMPMFetchAMToAMTrace(instrumentHandle, "", timeout, NULL, NULL, NULL,
      0, &actualArraySize));
   if (actualArraySize > 0)
   {
      referencePowersAMToAM = (float32 *)malloc(sizeof(float32)*actualArraySize);
      measuredAMToAM = (float32 *)malloc(sizeof(float32)*actualArraySize);
      curveFitAMToAM = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (referencePowersAMToAM && measuredAMToAM && curveFitAMToAM)
      {
         RFmxCheckWarn(RFmxSpecAn_AMPMFetchAMToAMTrace(instrumentHandle, "", timeout, referencePowersAMToAM,
            measuredAMToAM, curveFitAMToAM, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }
   actualArraySize = 0;
   RFmxCheckWarn(RFmxSpecAn_AMPMFetchAMToPMTrace(instrumentHandle, "", timeout, NULL, NULL, NULL,
      0, &actualArraySize));
   if (actualArraySize > 0)
   {
      referencePowersAMToPM = (float32 *)malloc(sizeof(float32)*actualArraySize);
      measuredAMToPM = (float32 *)malloc(sizeof(float32)*actualArraySize);
      curveFitAMToPM = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (referencePowersAMToPM && measuredAMToPM && curveFitAMToPM)
      {
         RFmxCheckWarn(RFmxSpecAn_AMPMFetchAMToPMTrace(instrumentHandle, "", timeout, referencePowersAMToPM,
            measuredAMToPM, curveFitAMToPM, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("-----------------Measurement-------------------\n");
   printf("Mean Linear Gain (dB)        :%f\n", meanLinearGain);
   printf("Mean Phase error (deg)       :%f\n", meanPhaseError);
   printf("Mean RMS EVM (%%)             :%f\n", meanRMSEVM);
   printf("AM to AM Residual (dB)       :%f\n", AMToAMResidual);
   printf("AM to PM Residual (deg)      :%f\n", AMToPMResidual);
   printf("Gain Error Range (dB)        :%f\n", gainErrorRange);
   printf("Phase Error Range (deg)      :%f\n", phaseErrorRange);
   printf("1 dB Compression Point (dBm) :%f\n", onedBCompressionPoint);

Error:
   if (error)
   {
      errorOccured = error;
      RFmxSpecAn_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (rfsgError)
   {
      errorOccured = rfsgError;
      niRFSG_GetError(rfsgSession, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (rfsgError < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (instrumentHandle)
   {
      RFmxSpecAn_Close(instrumentHandle, RFMXSPECAN_VAL_FALSE);
   }
   if (rfsgSession)
   {
      niRFSG_Abort(rfsgSession);
      niRFSG_ClearArbWaveform(rfsgSession, waveformName);
      niRFSG_close(rfsgSession);
   }
   if (referencePowersAMToAM)
      free(referencePowersAMToAM);
   if (measuredAMToAM)
      free(measuredAMToAM);
   if (curveFitAMToAM)
      free(curveFitAMToAM);
   if (referencePowersAMToPM)
      free(referencePowersAMToPM);
   if (measuredAMToPM)
      free(measuredAMToPM);
   if (curveFitAMToPM)
      free(curveFitAMToPM);
   if (referenceWaveformF32)
      free(referenceWaveformF32);
   if (waveformWithDPDF32)
      free(waveformWithDPDF32);
   if (inputPowers)
      free(inputPowers);
   if (complexGains)
      free(complexGains);

Exit:
   printf("\nPress any key to exit\n");
   _getch();

   return errorOccured;
}
