/* Steps:
1. Open RFSG session.
2. Configure RFSG frequency reference, generation mode to Script.
3. Configure marker0 to be generated from RFSG on the specified output terminal.
4. Configure frequency and power level of RF output signal.
5. Configure RFSG power level type.
6. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at the input of the DUT
   matches the user configured DUT Average Input Power.
7. a. Read waveform from.
   b. Write input waveform on RFSG device.
      Configure RFSG IQ rate, Pre-filter Gain and PAPR.
      Read waveform sample rate, multiply by 0.8 and set the result to the RFSG signal bandwidth.
      Write script to generate the waveform specified in the script. This script is programmed
      to generate waveform continuously, with marker0 aligned to sample index 0.
8. Initiate generation.
9. Open RFmx session.
10. Configure frequency reference of the analyser.
11. Configure Selected Ports.
12. Configure trigger to use as reference for signal acquisition.
13. Configure center frequency and external attenuation.
14. Select DPD measurement.
15. Configure pre-DPD CFR.
16. Configure waveform settings for pre-DPD CFR with filtering.
17. Apply pre-DPD CFR.
18. Retrieve the waveform PAPR.
19. configure the reference waveform.
20. Configure power of the signal at the input of the DUT. Select and configure the Memory
    polynomial or Generalized memory polynomial model and its parameters to estimate the predistotor.
21. Set the measurement sample rate and the measurement interval to use for analysis.
22. Enable iterative DPD.
23. Configure DPD NMSE Enabled.
24. Configure the Memory models Correction type.
25. Configure apply DPD CFR settings before calling RFmx initiate.
    This is because these settings are used by measurement when performing iterative DPD.
26. Perform Auto Level to compute an approximate reference level to use by the analyser.
27. Set the previous iteration polynomial, in case DPD is measured iteratively.
28. Initiates DPD measurement and then configure Apply Digital Predistortion to remove the
    effects of memory and nonlinearity introduced by the DUT.
29. a. Fetch DPD Polynomial.
    b. Fetch NMSE (dB).
30. Abort RFSG generation and write a new waveform that is predistorted by applying momory polynomial coefficients.
    Set RFSG Pre-filter Gain and sample rate computed from Apply Digital Predistortion.
    Set the final PAPR to the sum of the actual PAPR and the Power Offset as computed by Apply Digital Predistortion.
    Set the RFSG signal bandwidth by reading the waveform sample rate and multiplying it by 0.8.
    Initiate RFSG generation using the script that was selected earlier.
31. Perform Auto Level to compute an approximate reference level while generating the predistorted waveform.
32. Select and configure AMPM measurement in RFmx after DPD measurement is complete.
    AMPM measurement is used to inspect the measure the AM-AM and AM-PM response of the DUT.
33. Initiate and fetch AMPM results.
34. Close RFmx session.
35. Close RFSG session.
It is recommended to clear the waveform before closing RFSG session.*/

#include <stdlib.h>
#include <stdio.h>
#include <conio.h>
#include "niRFmxSpecAn.h"
#include "niRFSGPlayback.h"
#include "niRFSG.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION       4096

/* Maximum size of a selector string */
#define MAX_SELECTOR_STRING         256

/* CheckWarn macro for RFSG API calls*/
#define RfsgCheckWarn(fCall)     if (1) {ViStatus _code_; if (_code_ = (fCall), _code_ < 0)    \
                                    {rfsgError = _code_;goto Error;}        \
                                    else rfsgError = (rfsgError==0)?_code_:rfsgError;} else rfsgError = rfsgError

/* Trigger */
#define IQ_POWER_EDGE               0
#define DIGITAL_EDGE                1

#define NUMBER_OF_CARRIERS          1

float64 x0 = 0.0, dx = 0.0;
int32 numberOfSamples = 0;
float64 PAPR = 0.0;
NIComplexSingle *referenceWaveformF32 = NULL;
int32 error = 0, errorOccured = 0, lastErrorCode = 0;
char errorMessage[MAX_ERROR_DESCRIPTION];

/* Read waveform data from TDMS file into host memory for RFmx DPD reference waveform configuration. */
int32 ReadFromTDMSFile(char *fileName)
{
   int32 i = 0;
   ViReal64 x0v = 0, dxv = 0;
   int32 fileError = 0;
   niRFSGPlayback_ReadWaveformFromFileComplexF32(fileName, 0, NULL, NULL, NULL, &numberOfSamples);
   if (numberOfSamples > 0)
   {
      referenceWaveformF32 = (NIComplexSingle*)malloc(sizeof(NIComplexSingle)*numberOfSamples);
      if (referenceWaveformF32)
      {
         fileError = niRFSGPlayback_ReadWaveformFromFileComplexF32(fileName, numberOfSamples, &x0v, &dxv,
            (NIComplexNumberF32*)referenceWaveformF32, &numberOfSamples);
         if (fileError < 0)
         {
            errorOccured = fileError;
            return errorOccured;
         }
      }
      else
      {
         printf("malloc failed\n");
         return -1;
      }
      x0 = (float64)x0v;
      dx = (float64)dxv;
   }
   niRFSGPlayback_ReadPAPRFromFile(fileName, 0, &PAPR);
   return 0;
}

int main(int argc, char *argv[])
{
   ViSession rfsgSession = VI_NULL;
   ViRsrc rfsgResourceName = "RFSG";
   ViChar* refClockSource = NIRFSG_VAL_ONBOARD_CLOCK_STR;
   ViReal64 rfsgFrequency = 10e+6;                                      /*Hz*/
   ViChar *markerEventOutputTerminal = NIRFSG_VAL_PXI_TRIG0_STR;
   ViChar* script = "script DPDScript\n   repeat forever\n      generate Wfm marker0(0)\n   end repeat\nend script";
   ViReal64 rfsgExternalAttenuation = 0.0;                              /*dB*/
   ViReal64 externalGain;
   ViReal64 preFilterGain = -4.0;                                       /* dB */
   ViReal64 runtimeScaling = preFilterGain;
   char waveFormFileName[] = "..\\..\\Support\\LTE20MHz Waveform (Two Subframes).tdms";
   char waveformName[] = "Wfm";
   float64 DUTAverageInputPower = -20.0;                                /*dBm */
   ViInt32 powerLevelType = NIRFSG_VAL_PEAK_POWER;

   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;                                       /*Hz*/
   char carrierString[MAX_SELECTOR_STRING];

   niRFmxInstrHandle instrumentHandle = NULL;
   char *rfsaResourceName = "RFSA";
   float64 signalBandwidth = 20e+6;                                     /*Hz*/
   float64 autoLevelMeasurementInterval = 100e-6;                       /* seconds */
   float64 rfsaExternalAttenuation = 0.0;                               /*dB*/
   char *rfsaFrequencySource = RFMXSPECAN_VAL_ONBOARD_CLOCK_STR;
   float64 rfsaFrequency = 10e+6;                                       /*Hz*/
   int32 trigger = DIGITAL_EDGE;
   char * digitalEdgeSource = RFMXSPECAN_VAL_PXI_TRIG0_STR;
   int32 enableTrigger = RFMXSPECAN_VAL_TRUE;
   float64 IQPowerEdgeLevel = -20.0;                                    /*dBm*/
   float64 minQuietTime = 0.0;                                          /*seconds*/
   float64 triggerDelay = 0.0;                                          /*seconds*/

   /* Measurement Settings */
   float64 measurementInterval = 100e-6;                                /*seconds*/
   int32 sampleRateMode = RFMXSPECAN_VAL_DPD_MEASUREMENT_SAMPLE_RATE_MODE_REFERENCE_WAVEFORM;
   float64 sampleRate = 120e+6;                                         /* S/s */
   int32 DPDModel = RFMXSPECAN_VAL_DPD_MODEL_MEMORY_POLYNOMIAL;
   float64 autoReferenceLevel;
   int32 memoryPolynomialOrder = 3;
   int32 memoryPolynomialMemoryDepth = 2;
   int32 iterativeDPDEnabled = RFMXSPECAN_VAL_DPD_ITERATIVE_DPD_ENABLED_FALSE;
   int32 numberOfIterations = 3;
   int32 crossTermsLeadOrder = 2;
   int32 crossTermsLagOrder = 2;
   int32 crossTermsLeadMemoryDepth = 2;
   int32 crossTermsLagMemoryDepth = 2;
   int32 crossTermsMaximumLead = 2;
   int32 crossTermsMaximumLag = 2;
   int32 idleDurationPresent = RFMXSPECAN_VAL_DPD_REFERENCE_WAVEFORM_IDLE_DURATION_PRESENT_FALSE;
   int32 DPDApplyDPDIdleDurationPresent = RFMXSPECAN_VAL_DPD_APPLY_DPD_IDLE_DURATION_PRESENT_FALSE;
   int32 signalType = RFMXSPECAN_VAL_DPD_SIGNAL_TYPE_MODULATED;
   int32 memoryModelCorrectionType = RFMXSPECAN_VAL_DPD_APPLY_DPD_MEMORY_MODEL_CORRECTION_TYPE_MAGNITUDE_AND_PHASE;
   int32 NMSEEnabled = RFMXSPECAN_VAL_DPD_NMSE_ENABLED_FALSE;
   int32 referencePowerType = RFMXSPECAN_VAL_AMPM_REFERENCE_POWER_TYPE_INPUT;

   /* Pre DPD CFR */
   int32 preDPDCFREnabled = RFMXSPECAN_VAL_DPD_PRE_DPD_CFR_ENABLED_FALSE;
   int32 preDPDCFRMethod = RFMXSPECAN_VAL_DPD_PRE_DPD_CFR_METHOD_CLIPPING;
   int32 preDPDCFRMaxIterations = 10;
   float64 preDPDCFRTargetPAPR = 8.0;                                                         /* (dB) */
   int32 preDPDCFRWindowType = RFMXSPECAN_VAL_DPD_PRE_DPD_CFR_WINDOW_TYPE_KAISER_BESSEL;
   int32 preDPDCFRWindowLength = 10;
   float64 preDPDCFRShapingFactor = 5.0;
   float64 preDPDCFRShapingThreshold = -5.0;                                                  /* (dB) */
   int32 preDPDCFRFilterEnabled = RFMXSPECAN_VAL_DPD_PRE_DPD_CFR_FILTER_ENABLED_FALSE;
   float64 preDPDCarrierOffsets[NUMBER_OF_CARRIERS] = { 0.0 };                                /* (Hz) */
   float64 preDPDCarrierBandwidths[NUMBER_OF_CARRIERS] = { 20e6 };                            /* (Hz) */

   /* Apply DPD CFR */
   int32 applyDPDCFREnabled = RFMXSPECAN_VAL_DPD_APPLY_DPD_CFR_ENABLED_FALSE;
   int32 applyDPDCFRMethod = RFMXSPECAN_VAL_DPD_APPLY_DPD_CFR_METHOD_CLIPPING;
   int32 applyDPDCFRMaxIterations = 10;
   int32 applyDPDCFRTargetPAPRType = RFMXSPECAN_VAL_DPD_APPLY_DPD_CFR_TARGET_PAPR_TYPE_INPUT_PAPR;
   float64 applyDPDCFRTargetPAPR = 8.0;                                                       /* (dB) */
   int32 applyDPDCFRWindowType = RFMXSPECAN_VAL_DPD_APPLY_DPD_CFR_WINDOW_TYPE_KAISER_BESSEL;
   int32 applyDPDCFRWindowLength = 10;
   float64 applyDPDCFRShapingFactor = 5.0;
   float64 applyDPDCFRShapingThreshold = -5.0;                                                /* (dB) */

   int32 i;
   int32 actualArraySize = 0, DPDPolynomialSize = 0;
   int32 rfsgError = 0;
   float64 timeout = 10.0;                                                                    /* seconds */

   int32 waveformOutSize = 0, actualWaveformSize = 0;
   float64 maxOutputSize, outputSampleRate;

   NIComplexSingle *DPDPolynomial = NULL;
   float64 x0Out = 0.0;
   float64 dxOut = 0.0;
   NIComplexSingle* waveformWithDPDF32 = NULL;
   float64 preDPDx0 = 0.0;
   float64 preDPDdx = 0.0;
   NIComplexSingle* preDPDWaveformWithF32 = NULL;
   ViReal64 t0 = 0, dt = 0;

   float64 meanLinearGain = 0.0;
   float64 onedBCompressionPoint = 0.0;
   float64 meanRMSEVM = 0.0;
   float64 gainErrorRange = 0.0;
   float64 phaseErrorRange = 0.0;
   float64 meanPhaseError = 0.0;
   float64 powerOffset = 0.0;

   float32 *referencePowersAMToAM = NULL;
   float32 *measuredAMToAM = NULL;
   float32 *curveFitAMToAM = NULL;

   float32 *referencePowersAMToPM = NULL;
   float32 *measuredAMToPM = NULL;
   float32 *curveFitAMToPM = NULL;

   float64 AMToAMResidual = 0.0;
   float64 AMToPMResidual = 0.0;

   float64 NMSE = 0.0;

   RfsgCheckWarn(niRFSG_init(rfsgResourceName, VI_TRUE, VI_FALSE, &rfsgSession));
   RfsgCheckWarn(niRFSG_ConfigureGenerationMode(rfsgSession, NIRFSG_VAL_SCRIPT));
   RfsgCheckWarn(niRFSG_ConfigureRefClock(rfsgSession, refClockSource, rfsgFrequency));
   RfsgCheckWarn(niRFSG_ExportSignal(rfsgSession, NIRFSG_VAL_MARKER_EVENT, NIRFSG_VAL_MARKER0,
      markerEventOutputTerminal));
   RfsgCheckWarn(niRFSG_ConfigureRF(rfsgSession, centerFrequency, DUTAverageInputPower));
   RfsgCheckWarn(niRFSG_ConfigurePowerLevelType(rfsgSession, powerLevelType));
   externalGain = -1 * rfsgExternalAttenuation;
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_EXTERNAL_GAIN, externalGain));

   errorOccured = ReadFromTDMSFile(waveFormFileName);
   if (errorOccured)
   {
      printf("Cannot open the specified waveform file\n");
      goto Exit;
   }
   t0 = x0;
   dt = dx;
   waveformOutSize = numberOfSamples;

   /* Initialize a session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(rfsaResourceName, "", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", rfsaFrequencySource, rfsaFrequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   if (trigger == IQ_POWER_EDGE)
   {
      RFmxCheckWarn(RFmxSpecAn_CfgIQPowerEdgeTrigger(instrumentHandle, "", "0", IQPowerEdgeLevel,
         RFMXSPECAN_VAL_IQ_POWER_EDGE_RISING_SLOPE, triggerDelay, RFMXSPECAN_VAL_TRIGGER_MINIMUM_QUIET_TIME_MODE_MANUAL,
         minQuietTime, enableTrigger));
   }
   else
   {
      RFmxCheckWarn(RFmxSpecAn_CfgDigitalEdgeTrigger(instrumentHandle, "", digitalEdgeSource,
         RFMXSPECAN_VAL_DIGITAL_EDGE_RISING_EDGE, triggerDelay, enableTrigger));
   }

   RFmxCheckWarn(RFmxSpecAn_CfgRF(instrumentHandle, "", centerFrequency, 0, rfsaExternalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_DPD, RFMXSPECAN_VAL_TRUE));
   if (preDPDCFREnabled == RFMXSPECAN_VAL_DPD_PRE_DPD_CFR_ENABLED_TRUE)
   {
      RFmxCheckWarn(RFmxSpecAn_DPDSetPreDPDCFREnabled(instrumentHandle, "", preDPDCFREnabled));
      RFmxCheckWarn(RFmxSpecAn_DPDSetPreDPDCFRMethod(instrumentHandle, "", preDPDCFRMethod));
      RFmxCheckWarn(RFmxSpecAn_DPDSetPreDPDCFRMaximumIterations(instrumentHandle, "", preDPDCFRMaxIterations));
      RFmxCheckWarn(RFmxSpecAn_DPDSetPreDPDCFRTargetPAPR(instrumentHandle, "", preDPDCFRTargetPAPR));
      RFmxCheckWarn(RFmxSpecAn_DPDSetPreDPDCFRWindowType(instrumentHandle, "", preDPDCFRWindowType));
      RFmxCheckWarn(RFmxSpecAn_DPDSetPreDPDCFRWindowLength(instrumentHandle, "", preDPDCFRWindowLength));
      RFmxCheckWarn(RFmxSpecAn_DPDSetPreDPDCFRShapingFactor(instrumentHandle, "", preDPDCFRShapingFactor));
      RFmxCheckWarn(RFmxSpecAn_DPDSetPreDPDCFRShapingThreshold(instrumentHandle, "", preDPDCFRShapingThreshold));
      RFmxCheckWarn(RFmxSpecAn_DPDSetPreDPDCFRFilterEnabled(instrumentHandle, "", preDPDCFRFilterEnabled));
      RFmxCheckWarn(RFmxSpecAn_DPDSetPreDPDCFRNumberOfCarriers(instrumentHandle, "", NUMBER_OF_CARRIERS));
      for (i = 0; i < NUMBER_OF_CARRIERS; i++)
      {
         RFmxSpecAn_BuildCarrierString2("", i, MAX_SELECTOR_STRING, carrierString);
         RFmxCheckWarn(RFmxSpecAn_DPDSetPreDPDCarrierOffset(instrumentHandle, carrierString, preDPDCarrierOffsets[i]));
         RFmxCheckWarn(RFmxSpecAn_DPDSetPreDPDCarrierBandwidth(instrumentHandle, carrierString, preDPDCarrierBandwidths[i]));
      }
      if (waveformOutSize > 0)
      {
         preDPDWaveformWithF32 = (NIComplexSingle*)malloc(sizeof(NIComplexSingle)* waveformOutSize);
         if (preDPDWaveformWithF32)
         {
            RFmxCheckWarn(RFmxSpecAn_DPDApplyPreDPDSignalConditioning(instrumentHandle, "", t0, dt, referenceWaveformF32,
               numberOfSamples, DPDApplyDPDIdleDurationPresent, &preDPDx0, &preDPDdx, preDPDWaveformWithF32, waveformOutSize,
               &actualWaveformSize, &PAPR));
         }
         else
         {
            printf("malloc failed\n");
            goto Error;
         }
      }
   }

   if (preDPDCFREnabled == RFMXSPECAN_VAL_DPD_PRE_DPD_CFR_ENABLED_TRUE)
   {
      RfsgCheckWarn(niRFSG_WriteArbWaveformComplexF32(rfsgSession, "Wfm", waveformOutSize,
         (NIComplexNumberF32*)preDPDWaveformWithF32, VI_FALSE));
   }
   else
   {
      RfsgCheckWarn(niRFSG_WriteArbWaveformComplexF32(rfsgSession, "Wfm", waveformOutSize,
         (NIComplexNumberF32*)referenceWaveformF32, VI_FALSE));
   }

   RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_IQ_RATE, 1.0 / dx));
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_ARB_PRE_FILTER_GAIN, runtimeScaling));
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_PEAK_POWER_ADJUSTMENT, PAPR));
   RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_SIGNAL_BANDWIDTH, 0.8 * (1.0 / dx)));
   RfsgCheckWarn(niRFSG_WriteScript(rfsgSession, script));
   RfsgCheckWarn(niRFSG_Initiate(rfsgSession));

   if (preDPDCFREnabled == RFMXSPECAN_VAL_DPD_PRE_DPD_CFR_ENABLED_TRUE)
      RFmxCheckWarn(RFmxSpecAn_DPDCfgReferenceWaveform(instrumentHandle, "", preDPDx0, preDPDdx, preDPDWaveformWithF32,
         numberOfSamples, idleDurationPresent, signalType));
   else
      RFmxCheckWarn(RFmxSpecAn_DPDCfgReferenceWaveform(instrumentHandle, "", t0, dt, referenceWaveformF32,
         numberOfSamples, idleDurationPresent, signalType));

   RFmxCheckWarn(RFmxSpecAn_DPDCfgDUTAverageInputPower(instrumentHandle, "", DUTAverageInputPower));
   RFmxCheckWarn(RFmxSpecAn_DPDCfgDPDModel(instrumentHandle, "", DPDModel));
   RFmxCheckWarn(RFmxSpecAn_DPDCfgMemoryPolynomial(instrumentHandle, "", memoryPolynomialOrder,
      memoryPolynomialMemoryDepth));
   RFmxCheckWarn(RFmxSpecAn_DPDCfgGeneralizedMemoryPolynomialCrossTerms(instrumentHandle, "",
      crossTermsLeadOrder, crossTermsLagOrder, crossTermsLeadMemoryDepth, crossTermsLagMemoryDepth,
      crossTermsMaximumLead, crossTermsMaximumLag));
   RFmxCheckWarn(RFmxSpecAn_DPDCfgMeasurementSampleRate(instrumentHandle, "", sampleRateMode, sampleRate));
   RFmxCheckWarn(RFmxSpecAn_DPDCfgMeasurementInterval(instrumentHandle, "", measurementInterval));
   RFmxCheckWarn(RFmxSpecAn_DPDCfgIterativeDPDEnabled(instrumentHandle, "", iterativeDPDEnabled));
   if (iterativeDPDEnabled == RFMXSPECAN_VAL_DPD_ITERATIVE_DPD_ENABLED_FALSE)
      numberOfIterations = 1;
   RFmxCheckWarn(RFmxSpecAn_DPDSetNMSEEnabled(instrumentHandle, "", NMSEEnabled));
   RFmxCheckWarn(RFmxSpecAn_DPDCfgApplyDPDMemoryModelCorrectionType(instrumentHandle, "", memoryModelCorrectionType));
   RFmxCheckWarn(RFmxSpecAn_DPDSetApplyDPDCFREnabled(instrumentHandle, "", applyDPDCFREnabled));
   RFmxCheckWarn(RFmxSpecAn_DPDSetApplyDPDCFRMethod(instrumentHandle, "", applyDPDCFRMethod));
   RFmxCheckWarn(RFmxSpecAn_DPDSetApplyDPDCFRMaximumIterations(instrumentHandle, "", applyDPDCFRMaxIterations));
   RFmxCheckWarn(RFmxSpecAn_DPDSetApplyDPDCFRTargetPAPRType(instrumentHandle, "", applyDPDCFRTargetPAPRType));
   RFmxCheckWarn(RFmxSpecAn_DPDSetApplyDPDCFRTargetPAPR(instrumentHandle, "", applyDPDCFRTargetPAPR));
   RFmxCheckWarn(RFmxSpecAn_DPDSetApplyDPDCFRWindowType(instrumentHandle, "", applyDPDCFRWindowType));
   RFmxCheckWarn(RFmxSpecAn_DPDSetApplyDPDCFRWindowLength(instrumentHandle, "", applyDPDCFRWindowLength));
   RFmxCheckWarn(RFmxSpecAn_DPDSetApplyDPDCFRShapingFactor(instrumentHandle, "", applyDPDCFRShapingFactor));
   RFmxCheckWarn(RFmxSpecAn_DPDSetApplyDPDCFRShapingThreshold(instrumentHandle, "", applyDPDCFRShapingThreshold));
   RFmxCheckWarn(RFmxSpecAn_AutoLevel(instrumentHandle, "", signalBandwidth, autoLevelMeasurementInterval,
      &autoReferenceLevel));

   /* Calculations to get waveformOut size*/
   if (sampleRateMode == RFMXSPECAN_VAL_DPD_MEASUREMENT_SAMPLE_RATE_MODE_REFERENCE_WAVEFORM)
      outputSampleRate = 1 / dx;
   else
      outputSampleRate = sampleRate;
   maxOutputSize = outputSampleRate * dx * numberOfSamples;
   waveformOutSize = (int32)maxOutputSize;
   for (i = 0; i < numberOfIterations; i++)
   {
      RFmxCheckWarn(RFmxSpecAn_DPDCfgPreviousDPDPolynomial(instrumentHandle, "", DPDPolynomial, DPDPolynomialSize));
      RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));
      if (waveformOutSize > 0)
      {
         waveformWithDPDF32 = (NIComplexSingle*)malloc(sizeof(NIComplexSingle)* waveformOutSize);
         if (waveformWithDPDF32)
         {
            if (preDPDCFREnabled == RFMXSPECAN_VAL_DPD_PRE_DPD_CFR_ENABLED_TRUE)
            {
               RFmxCheckWarn(RFmxSpecAn_DPDApplyDigitalPredistortion(instrumentHandle, "", preDPDx0, preDPDdx,
                  preDPDWaveformWithF32, numberOfSamples, DPDApplyDPDIdleDurationPresent, timeout, &x0Out, &dxOut,
                  waveformWithDPDF32, waveformOutSize, &actualWaveformSize, &PAPR, &powerOffset));
            }
            else
            {
               RFmxCheckWarn(RFmxSpecAn_DPDApplyDigitalPredistortion(instrumentHandle, "", t0, dt,
                  referenceWaveformF32, numberOfSamples, DPDApplyDPDIdleDurationPresent, timeout, &x0Out, &dxOut,
                  waveformWithDPDF32, waveformOutSize, &actualWaveformSize, &PAPR, &powerOffset));
            }
         }
         else
         {
            printf("malloc failed\n");
            goto Error;
         }
      }

      if (DPDPolynomial)
      {
         free(DPDPolynomial);
         DPDPolynomial = NULL;
         DPDPolynomialSize = 0;
      }

      RFmxCheckWarn(RFmxSpecAn_DPDFetchDPDPolynomial(instrumentHandle, "", timeout, NULL, 0, &DPDPolynomialSize));
      if (DPDPolynomialSize > 0)
      {
         DPDPolynomial = (NIComplexSingle*)malloc(sizeof(NIComplexSingle)*DPDPolynomialSize);
         if (DPDPolynomial)
         {
            RFmxCheckWarn(RFmxSpecAn_DPDFetchDPDPolynomial(instrumentHandle, "", timeout, DPDPolynomial,
               DPDPolynomialSize, NULL));
         }
         else
         {
            printf("malloc failed\n");
            goto Error;
         }
      }

      RFmxCheckWarn(RFmxSpecAn_DPDFetchNMSE(instrumentHandle, "", timeout, &NMSE));
      printf("NMSE       :%lf\n", NMSE);

      RfsgCheckWarn(niRFSG_Abort(rfsgSession));
      RfsgCheckWarn(niRFSG_ClearArbWaveform(rfsgSession, waveformName));
      RfsgCheckWarn(niRFSG_WriteArbWaveformComplexF32(rfsgSession, "Wfm", waveformOutSize,
         (NIComplexNumberF32*)waveformWithDPDF32, VI_FALSE));
      RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_ARB_PRE_FILTER_GAIN, runtimeScaling));
      RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_IQ_RATE, 1.0 / dxOut));
      RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_PEAK_POWER_ADJUSTMENT, (ViReal64)(PAPR + powerOffset)));
      RfsgCheckWarn(niRFSG_SetAttributeViReal64(rfsgSession, "", NIRFSG_ATTR_SIGNAL_BANDWIDTH, 0.8 * (1.0 / dxOut)));
      RfsgCheckWarn(niRFSG_WriteScript(rfsgSession, script));
      RfsgCheckWarn(niRFSG_Initiate(rfsgSession));

      if (waveformWithDPDF32)
      {
         free(waveformWithDPDF32);
         waveformWithDPDF32 = NULL;
      }
   }

   RFmxCheckWarn(RFmxSpecAn_AutoLevel(instrumentHandle, "", signalBandwidth, autoLevelMeasurementInterval,
      &autoReferenceLevel));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_AMPM, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgMeasurementSampleRate(instrumentHandle, "",
      RFMXSPECAN_VAL_AMPM_MEASUREMENT_SAMPLE_RATE_MODE_REFERENCE_WAVEFORM, sampleRate));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgMeasurementInterval(instrumentHandle, "", measurementInterval));

   if (preDPDCFREnabled == RFMXSPECAN_VAL_DPD_PRE_DPD_CFR_ENABLED_TRUE)
      RFmxCheckWarn(RFmxSpecAn_AMPMCfgReferenceWaveform(instrumentHandle, "", preDPDx0, preDPDdx,
         preDPDWaveformWithF32, numberOfSamples, idleDurationPresent, signalType));
   else
      RFmxCheckWarn(RFmxSpecAn_AMPMCfgReferenceWaveform(instrumentHandle, "", t0, dt, referenceWaveformF32,
         numberOfSamples, idleDurationPresent, signalType));

   RFmxCheckWarn(RFmxSpecAn_AMPMCfgDUTAverageInputPower(instrumentHandle, "", DUTAverageInputPower));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgThreshold(instrumentHandle, "", RFMXSPECAN_VAL_AMPM_THRESHOLD_ENABLED_TRUE, -20,
      RFMXSPECAN_VAL_AMPM_THRESHOLD_TYPE_RELATIVE));
   RFmxCheckWarn(RFmxSpecAn_AMPMCfgReferencePowerType(instrumentHandle, "", referencePowerType));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   RFmxCheckWarn(RFmxSpecAn_AMPMFetchDUTCharacteristics(instrumentHandle, "", timeout, &meanLinearGain,
      &onedBCompressionPoint, &meanRMSEVM));
   RFmxCheckWarn(RFmxSpecAn_AMPMFetchError(instrumentHandle, "", timeout, &gainErrorRange, &phaseErrorRange,
      &meanPhaseError));
   RFmxCheckWarn(RFmxSpecAn_AMPMFetchCurveFitResidual(instrumentHandle, "", timeout, &AMToAMResidual, &AMToPMResidual));
   RFmxCheckWarn(RFmxSpecAn_AMPMFetchAMToAMTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      referencePowersAMToAM = (float32*)malloc(sizeof(float32)*actualArraySize);
      measuredAMToAM = (float32*)malloc(sizeof(float32)*actualArraySize);
      curveFitAMToAM = (float32*)malloc(sizeof(float32)*actualArraySize);
      if (referencePowersAMToAM && measuredAMToAM && curveFitAMToAM)
      {
         RFmxCheckWarn(RFmxSpecAn_AMPMFetchAMToAMTrace(instrumentHandle, "", timeout, referencePowersAMToAM, measuredAMToAM,
            curveFitAMToAM, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed\n");
         goto Error;
      }
   }

   RFmxCheckWarn(RFmxSpecAn_AMPMFetchAMToPMTrace(instrumentHandle, "", timeout, NULL, NULL, NULL, 0, &actualArraySize));
   if (actualArraySize > 0)
   {
      referencePowersAMToPM = (float32*)malloc(sizeof(float32)*actualArraySize);
      measuredAMToPM = (float32*)malloc(sizeof(float32)*actualArraySize);
      curveFitAMToPM = (float32*)malloc(sizeof(float32)*actualArraySize);
      if (referencePowersAMToPM && measuredAMToPM && curveFitAMToPM)
      {
         RFmxCheckWarn(RFmxSpecAn_AMPMFetchAMToPMTrace(instrumentHandle, "", timeout, referencePowersAMToPM,
            measuredAMToPM, curveFitAMToPM, actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed\n");
         goto Error;
      }
   }

   printf("------------------AMPM Measurement-----------------\n");
   printf("Mean Linear Gain (dB)       :%lf\n", meanLinearGain);
   printf("Mean Phase Error(deg)       :%lf\n", meanPhaseError);
   printf("Mean RMS EVM (%%)           :%lf\n", meanRMSEVM);
   printf("AM to AM Residual(dB)       :%lf\n", AMToAMResidual);
   printf("AM to PM Residual(deg)      :%lf\n", AMToPMResidual);
   printf("Gain Error Range(dB)        :%lf\n", gainErrorRange);
   printf("Phase Error Range (deg)     :%lf\n", phaseErrorRange);
   printf("1 dB Compression Point(dBm) :%lf\n", onedBCompressionPoint);

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
   if (preDPDWaveformWithF32)
      free(preDPDWaveformWithF32);

Exit:
   printf("\nPress any key to exit\n");
   _getch();

   return error;
}
