/* Steps:
1. Open RFSG session.
2. Configure RFSG frequency reference.
3. Configure marker0 to be generated from RFSG on the specified output terminal.
4. Configure frequency and power level of RF output signal.
5. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at
   the input of the DUT matches the user configured DUT Average Input Power.
6. a. Read waveform from file and download Waveform from file to RFSG
   b. Set Waveform Runtime Scaling to the negative of the desired Pre-filter Gain.
   c. Read waveform sample rate, multiply by 0.8 and set the result to the signal bandwidth.
   d. Write script to generate the waveform specified in the script.
   e. This script is programmed to generate waveform continuously, with marker0 aligned to sample index 0.
7. Initiate RFSG generation as per the selected script.
8. Open RFmx session.
9. Configure frequency reference of the analyser.
10. Configure Selected Ports.
11. Configure trigger to use as reference for signal acquisition.
12. Configure center frequency and external attenuation.
13. Select AMPM measurement, configure the reference waveform and power of this signal at the input of the DUT.
14. Set the measurement sample rate and the measurement interval to use for analysis.
15. Set threshold.
16. Configure Reference Power Type.
17. Set Reference Level or perform Auto Level to compute an approximate reference level to use by the analyser.
18. Initiate and fetch AMPM results.
19. Close RFmx session.
20. Close RFSG session.
It is recommended to clear the waveform before closing RFSG session.
*/

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"
#include "niRFSGPlayback.h"
#include "niRFSG.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

#define AUTO_LEVEL_OFF              0
#define AUTO_LEVEL_ON               1

#define IQ_POWER_EDGE               0
#define DIGITAL_EDGE                1

#define rfsgCheckWarn(fCall)     if (1) {ViStatus _code_; if (_code_ = (fCall), _code_ < 0)    \
                                 {rfsgError = _code_;goto Error;}        \
                                 else rfsgError = (rfsgError==0)?_code_:rfsgError;} else rfsgError = rfsgError

#define playbackCheckWarn(fCall) if (1) {ViStatus _code_; if (_code_ = (fCall), _code_ < 0)    \
                                 {playbackError = _code_;goto Error;}        \
                                 else playbackError = (playbackError==0)?_code_:playbackError;} \
                                 else playbackError = playbackError

float64 x0 = 0.0, dx = 0.0;
int32 numberOfSamples = 0;
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
   float64 centerFrequency = 1e+9;                                                  /* Hz */

   ViSession  rfsgSession = VI_NULL;
   ViRsrc     rfsgResourceName = "RFSG";

   char waveFormFileName[] = "..\\Support\\LTE20MHz Waveform (Two Subframes).tdms";
   ViReal64 rfsgExternalAttenuation = 0.00;                                         /* dB */
   float64 DUTAverageInputPower = -20.00;                                           /* dBm */
   ViConstString   rfsgOutputTerminal = NIRFSG_VAL_PXI_TRIG0_STR;
   ViReal64 preFilterGain = -4.00;                                                  /* dB */
   ViReal64 runtimeScaling = preFilterGain;

   ViConstString rfsgFrequencySource = NIRFSG_VAL_ONBOARD_CLOCK_STR;
   ViReal64 rfsgFrequency = 10e+6;

   ViConstString waveformName = "Wfm";
   ViConstString script = "script AMPMScript\n  repeat forever\n  generate Wfm marker0(0)\n  end repeat\nend script";

   ViReal64 externalGain = 0.0;

   //RFSA Configuration
   niRFmxInstrHandle instrumentHandle = NULL;
   char *rfsaResourceName = "RFSA";

   float64 signalBandwidth = 20e+6;
   float64 autoMeasurementInterval = 100e-6;
   float64 autoReferenceLevel;                                                      /* dBm */

   float64 referenceLevel = -14.00;                                                 /* dBm */

   float64 externalAttenuation = 0.00;                                              /* dB */

   char *rfsaFrequencySource = RFMXSPECAN_VAL_ONBOARD_CLOCK_STR;
   float64 rfsaFrequency = 10e+6;                                                   /* Hz */

   int32 enableTrigger = RFMXSPECAN_VAL_TRUE;

   char *digitalEdgeSource = RFMXSPECAN_VAL_PXI_TRIG0_STR;

   float64 iqPowerEdgeLevel = -20.00;
   int32 minQuietTimeMode = RFMXSPECAN_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_MANUAL;
   float64 minQuietTime = 0.00;

   float64 triggerDelay = 0.00;                                                     /*s   */

   float64 measurementInterval = 100e-6;                                            /*s   */

   int32 sampleRateMode = RFMXSPECAN_VAL_AMPM_MEASUREMENT_SAMPLE_RATE_MODE_REFERENCE_WAVEFORM;
   float64 sampleRate = 120e+6;                                                     /* S/s */

   int32 thresholdEnabled = RFMXSPECAN_VAL_AMPM_THRESHOLD_ENABLED_TRUE;
   int32 thresholdType = RFMXSPECAN_VAL_AMPM_THRESHOLD_TYPE_RELATIVE;
   float64 thresholdLevel = -20.00;                                                 /* dBm */

   int32 referencePowerType = RFMXSPECAN_VAL_AMPM_REFERENCE_POWER_TYPE_INPUT;

   int32 idleDurationPresent = RFMXSPECAN_VAL_AMPM_REFERENCE_WAVEFORM_IDLE_DURATION_PRESENT_FALSE;
   int32 signalType = RFMXSPECAN_VAL_AMPM_SIGNAL_TYPE_MODULATED;

   float64 timeout = 10;                                                            /* seconds */

   int32 autoLevel = AUTO_LEVEL_ON;

   int32 trigger = DIGITAL_EDGE;

   int32 error = 0, rfsgError = 0;

   /* Variables to store the result */
   int32 actualArraySizeAMToAM;
   float32 *referencePowersAMToAM = NULL;
   float32 *measuredAMToAM = NULL;
   float32 *curveFitAMToAM = NULL;
   int32 actualArraySizeAMToPM;
   float32 *referencePowersAMToPM = NULL;
   float32 *measuredAMToPM = NULL;
   float32 *curveFitAMToPM = NULL;
   float64 meanLinearGain;
   float64 meanRMSEVM;
   float64 gainErrorRange;
   float64 phaseErrorRange;
   float64 meanPhaseError;
   float64 onedBCompressionPoint;
   float64 AMToAMResidual;
   float64 AMToPMResidual;

   errorOccured = ReadFromTDMSFile(waveFormFileName);
   if (errorOccured)
   {
      printf("Cannot open the specified waveform file\n");
      goto Exit;
   }

   rfsgCheckWarn(niRFSG_init(rfsgResourceName, VI_TRUE, VI_FALSE, &rfsgSession));
   rfsgCheckWarn(niRFSG_ConfigureRefClock(rfsgSession, rfsgFrequencySource, rfsgFrequency));
   rfsgCheckWarn(niRFSG_ExportSignal(rfsgSession, NIRFSG_VAL_MARKER_EVENT, NIRFSG_VAL_MARKER0, rfsgOutputTerminal));
   rfsgCheckWarn(niRFSG_ConfigureRF(rfsgSession, centerFrequency, DUTAverageInputPower));
   externalGain = -1 * rfsgExternalAttenuation;
   rfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_EXTERNAL_GAIN, externalGain));
   playbackCheckWarn(niRFSGPlayback_ReadAndDownloadWaveformFromFile(rfsgSession, waveFormFileName, waveformName));
   playbackCheckWarn(niRFSGPlayback_StoreWaveformRuntimeScaling(rfsgSession, waveformName, runtimeScaling));
   playbackCheckWarn(niRFSGPlayback_RetrieveWaveformSampleRate(rfsgSession, waveformName, &sampleRate));
   playbackCheckWarn(niRFSGPlayback_StoreWaveformSignalBandwidth(rfsgSession, waveformName, 0.8*sampleRate));
   playbackCheckWarn(niRFSGPlayback_SetScriptToGenerateSingleRFSG(rfsgSession, script));
   rfsgCheckWarn(niRFSG_Initiate(rfsgSession));

   /* Initialize a session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", rfsaFrequencySource, rfsaFrequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   if (trigger == DIGITAL_EDGE)
   {
      RFmxCheckWarn(RFmxSpecAn_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource,
         RFMXSPECAN_VAL_DIGITAL_EDGE_RISING_EDGE, triggerDelay, enableTrigger));
   }
   else
   {
      RFmxCheckWarn(RFmxSpecAn_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", iqPowerEdgeLevel,
         RFMXSPECAN_VAL_IQ_POWER_EDGE_RISING_SLOPE, triggerDelay, minQuietTimeMode, minQuietTime, enableTrigger));
   }

   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, 0.0, externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_AMPM, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgDUTAverageInputPower(instrumentHandle, "", DUTAverageInputPower));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgReferenceWaveform(instrumentHandle, "", x0, dx, referenceWaveformF32,
      numberOfSamples, idleDurationPresent, signalType));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgMeasurementSampleRate(instrumentHandle, "", sampleRateMode, sampleRate));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgMeasurementInterval(instrumentHandle, "", measurementInterval));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgThreshold(instrumentHandle, "", thresholdEnabled, thresholdLevel, thresholdType));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgReferencePowerType(instrumentHandle, "", referencePowerType));
   if (autoLevel)
   {
      RFmxCheckWarn(RFmxSpecAn_AutoLevel(instrumentHandle, "", signalBandwidth, autoMeasurementInterval,
         &autoReferenceLevel));
      printf("Reference level(dBm)           %f\n", autoReferenceLevel);
   }
   else
   {
      RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   }
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Fetch Results */
   RFmxCheckWarn(RFmxSpecAn_AMPMFetchDUTCharacteristics(instrumentHandle, "", timeout, &meanLinearGain,
      &onedBCompressionPoint, &meanRMSEVM));
   RFmxCheckWarn(RFmxSpecAn_AMPMFetchError(instrumentHandle, "", timeout, &gainErrorRange, &phaseErrorRange,
      &meanPhaseError));
   RFmxCheckWarn(RFmxSpecAn_AMPMFetchCurveFitResidual(instrumentHandle, "", timeout, &AMToAMResidual,
      &AMToPMResidual));

   /* Fetch AMToAM trace */
   actualArraySizeAMToAM = 0;
   RFmxCheckWarn(RFmxSpecAn_AMPMFetchAMToAMTrace(instrumentHandle, "", timeout, NULL, NULL, NULL,
      0, &actualArraySizeAMToAM));

   if (actualArraySizeAMToAM > 0)
   {
      referencePowersAMToAM = (float32 *)malloc(sizeof(float32) * actualArraySizeAMToAM);
      measuredAMToAM = (float32 *)malloc(sizeof(float32) * actualArraySizeAMToAM);
      curveFitAMToAM = (float32 *)malloc(sizeof(float32) * actualArraySizeAMToAM);
      if (referencePowersAMToAM && measuredAMToAM && curveFitAMToAM)
      {
         RFmxCheckWarn(RFmxSpecAn_AMPMFetchAMToAMTrace(instrumentHandle, "", timeout, referencePowersAMToAM,
            measuredAMToAM, curveFitAMToAM, actualArraySizeAMToAM, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   /* Fetch AMToPM trace */
   actualArraySizeAMToPM = 0;
   RFmxCheckWarn(RFmxSpecAn_AMPMFetchAMToPMTrace(instrumentHandle, "", timeout, NULL, NULL, NULL,
      0, &actualArraySizeAMToPM));

   if (actualArraySizeAMToPM > 0)
   {
      referencePowersAMToPM = (float32 *)malloc(sizeof(float32) * actualArraySizeAMToPM);
      measuredAMToPM = (float32 *)malloc(sizeof(float32) * actualArraySizeAMToPM);
      curveFitAMToPM = (float32 *)malloc(sizeof(float32) * actualArraySizeAMToPM);
      if (referencePowersAMToPM && measuredAMToPM && curveFitAMToPM)
      {
         RFmxCheckWarn(RFmxSpecAn_AMPMFetchAMToPMTrace(instrumentHandle, "", timeout, referencePowersAMToPM,
            measuredAMToPM, curveFitAMToPM, actualArraySizeAMToPM, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("-----------------Measurement-----------------\n");
   printf("Mean Linear Gain (dB)        :%f\n", meanLinearGain);
   printf("Mean Phase Error (deg)       :%f\n", meanPhaseError);
   printf("Mean RMS EVM (%%)             :%f\n", meanRMSEVM);
   printf("AM to AM Residual (dB)       :%f\n", AMToAMResidual);
   printf("AM to PM Residual (deg)      :%f\n", AMToPMResidual);
   printf("Gain Error Range (dB)        :%f\n", gainErrorRange);
   printf("Phase Error Range (deg)      :%f\n", phaseErrorRange);
   printf("1 dB Compression Point (dBm) :%f\n", onedBCompressionPoint);


Error:
   if (rfsgError)
   {
      errorOccured = rfsgError;
      niRFSG_GetError(rfsgSession, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (rfsgError < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (error)
   {
      errorOccured = error;
      RFmxSpecAn_GetError(instrumentHandle, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (error < 0)
         printf("ERROR: %s\n", errorMessage);
      else
         printf("WARNING: %s\n", errorMessage);
   }

   if (playbackError)
   {
      errorOccured = playbackError;
      niRFSGPlayback_GetError(&lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (playbackError < 0)
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
      niRFSGPlayback_ClearWaveform(rfsgSession, waveformName);
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
Exit:
   printf("\nPress any key to exit\n");
   _getch();

   return errorOccured;
}
