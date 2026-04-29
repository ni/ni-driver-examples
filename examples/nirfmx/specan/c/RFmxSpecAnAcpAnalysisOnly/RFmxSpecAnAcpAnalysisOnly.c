//Steps:
//1. Open a new RFmx session.
//2. Configure Center Frequency.
//3. Select ACP measurement and enable the traces.
//4. Configure RBW filter parameters.
//5. Configure Sweep Time.
//6. Configure Integration BW of the Carrier and Offset Channels, Number of Offset Channels and Channel Spacing between the Offset channels. This function configures a Carrier Channel with Offset Channels as specified by the Number of Offsets. Refer to function help for more information.
//7. Configure Averaging Parameters.
//8. Commit the settings on the RFmx session and read the acquisition parameters (Span and Number of Records)from the RFmxInstr Property node.

//9. Open a new NI-RFSA session.
//10. Configure the NI-RFSA device reference clock.
//11. Configure the Center Frequency of the RFSA hardware for the 2 possible Acquisition types - IQ and Spectrum acquisition.
//12. Configure the External gain (in dB) of a device or cable connected before the RF IN connector of the NI-RFSA.
//13. Configure the Reference Level in dBm.
//14. Read the maximum instantaneous bandwidth of the RFSA device and compare with the Span requested by the measurement to decide the type of acquisition to be performed by RFSA.

//15. Configure the Acquisition Type to Spectrum.
//16. Read the Spectrum Acquisition parameters (Resolution Bandwidth and FFT Window Type) from the RFmx session and pass the same settings to the NI-RFSA session.
//17. Read the Power Spectrum from RFSA and pass it to RFmx AnalyzeSpectrum function for performing the measurement.
//    Note: NI-RFSA returns the power spectrum centered at the configured Center Frequency, but RFmx AnalyzeSpectrum function expects the spectrum to be at baseband i.e f0 is relative to 0Hz.

//18. Configure the Acquisition Type to IQ.
//19. Read the IQ Acquisition parameters (Sampling Rate and Acquisition Time) from the RFmx session and pass the same settings to the NI-RFSA session.
//20. Initiate the acquisition.
//21. Read the IQ Waveform from RFSA and pass it to RFmx AnalyzeIQ function for performing the measurement.
//    Note: In a multi-record acquisition case, the 'reset' parameter of the RFmxSpecAn AnalyzeIQ function is true for the first iteration and false for the remaining iterations.

//22. Fetch ACP Measurements and Traces.
//23. Close the RFmx and NI-RFSA Session.


#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <math.h>
#include "niRFSA.h"
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION    4096

#define rfsaCheckWarn(fCall)     if (1) {ViStatus _code_; if (_code_ = (fCall), _code_ < 0)    \
                                    {rfsaError = _code_;goto Error;}        \
                                    else rfsaError = (rfsaError==0)?_code_:rfsaError;} else rfsaError = rfsaError

#define NUMBER_OF_OFFSETS        1


int main(int argc, char *argv[])
{
   int i = 0, j = 0;
   ViSession rfsaSession = VI_NULL;
   niRFmxInstrHandle instrumentHandle = NULL;

   char errorMessage[MAX_ERROR_DESCRIPTION];
   int32 error = 0, lastErrorCode = 0, rfsaError = 0, errorOccured = 0;

   char *rfsaResourceName = "RFSA";
   ViReal64 referenceLevel = 0.00;           /* dBm */
   ViReal64 frequency = 10e+6;               /* Hz */
   ViConstString frequencySource = NIRFSA_VAL_ONBOARD_CLOCK_STR;

   float64 centerFrequency = 1e+9;           /* Hz */
   ViReal64 updatedCenterFrequency;          /* Hz */
   float64 externalAttenuation = 0.00;       /* dB */

   /* RBW Filter */
   int32 RBWFilterType = RFMXSPECAN_VAL_ACP_RBW_FILTER_TYPE_GAUSSIAN;
   int32 RBWAuto = RFMXSPECAN_VAL_ACP_RBW_AUTO_TRUE;
   float64 RBW = 10.0e+3;                    /* Hz */

   /* Sweep Time */
   int32 sweepTimeAuto = RFMXSPECAN_VAL_ACP_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 1.00e-3;      /* seconds */

   float64 integrationBandwidth = 1e+6;      /* Hz */
   int32 numberOfOffsets = NUMBER_OF_OFFSETS;
   float64 channelSpacing = 1e+6;            /* Hz */

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_ACP_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_ACP_AVERAGING_TYPE_RMS;

   float64 maxDeviceInstantaneousBandwidth;
   float64 IQAcquisitionTime;
   float64 IQMinimumSampleRate;
   float64 spectralAcquisitionSpan;
   float64 spectralRBW;
   int32 spectralFFTWindow;
   ViInt32 FFTWindowType;

   float64 timeout = 10.0;                   /* seconds */

   int32 numberOfRecords;
   int32 reset;
   int64 reserved = 0;
   ViInt64 numberOfSamples;
   NIComplexNumberF32 *IQData = NULL;
   niRFSA_wfmInfo IQDataWfmInfo;
   ViInt32 numberOfSpectralLines;
   ViReal64 *powerSpectrumData64 = NULL;
   ViReal32 *powerSpectrumData32 = NULL;
   niRFSA_spectrumInfo spectrumInfo;

   /* Variables to store the results */
   float64 absolutePower;
   int32 offsetMeasArraySize = 1;
   float64 *lowerRelativePower = (float64 *)NULL;
   float64 *upperRelativePower = (float64 *)NULL;
   float64 *lowerAbsolutePower = (float64 *)NULL;
   float64 *upperAbsolutePower = (float64 *)NULL;
   float64 x0 = 0.0, dx = 0.0;
   float32 *spectrum = (float32 *)NULL;
   int32 actualArraySize;

   rfsaCheckWarn(niRFSA_init(rfsaResourceName, VI_TRUE, VI_FALSE, &rfsaSession));
   rfsaCheckWarn(niRFSA_ConfigureRefClock(rfsaSession, frequencySource, frequency));
   rfsaCheckWarn(niRFSA_SetAttributeViReal64(rfsaSession, "", NIRFSA_ATTR_IQ_CARRIER_FREQUENCY,
      centerFrequency));
   rfsaCheckWarn(niRFSA_SetAttributeViReal64(rfsaSession, "", NIRFSA_ATTR_SPECTRUM_CENTER_FREQUENCY,
      centerFrequency));
   rfsaCheckWarn(niRFSA_SetAttributeViReal64(rfsaSession, "", NIRFSA_ATTR_EXTERNAL_GAIN,
      -externalAttenuation));
   rfsaCheckWarn(niRFSA_ConfigureReferenceLevel(rfsaSession, "", referenceLevel));
   rfsaCheckWarn(niRFSA_GetAttributeViReal64(rfsaSession, "", NIRFSA_ATTR_MAX_DEVICE_INSTANTANEOUS_BANDWIDTH,
      &maxDeviceInstantaneousBandwidth));

   RFmxCheckWarn(RFmxSpecAn_Initialize("", "AnalysisOnly=1", &instrumentHandle, NULL));
   RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_ACP,
      RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgRBWFilter(instrumentHandle, "", RBWAuto, RBW, RBWFilterType));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgCarrierAndOffsets(instrumentHandle, "", integrationBandwidth,
      numberOfOffsets, channelSpacing));
   RFmxCheckWarn(RFmxSpecAn_ACPCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount,
      averagingType));

   RFmxCheckWarn(RFmxSpecAn_Commit(instrumentHandle, ""));
   RFmxCheckWarn(RFmxInstr_GetRecommendedNumberOfRecords(instrumentHandle, "", &numberOfRecords));
   RFmxCheckWarn(RFmxInstr_GetRecommendedSpectralAcquisitionSpan(instrumentHandle, "", &spectralAcquisitionSpan));

   if (maxDeviceInstantaneousBandwidth >= spectralAcquisitionSpan) //IQ Acquisition
   {
      RFmxCheckWarn(RFmxInstr_GetRecommendedIQAcquisitionTime(instrumentHandle, "", &IQAcquisitionTime));
      RFmxCheckWarn(RFmxInstr_GetRecommendedIQMinimumSampleRate(instrumentHandle, "", &IQMinimumSampleRate));
      numberOfSamples = (long)ceil(IQMinimumSampleRate * IQAcquisitionTime);

      rfsaCheckWarn(niRFSA_ConfigureAcquisitionType(rfsaSession, NIRFSA_VAL_IQ));
      rfsaCheckWarn(niRFSA_ConfigureIQRate(rfsaSession, "", IQMinimumSampleRate));
      rfsaCheckWarn(niRFSA_ConfigureNumberOfSamples(rfsaSession, "", VI_TRUE, numberOfSamples));
      rfsaCheckWarn(niRFSA_ConfigureNumberOfRecords(rfsaSession, "", VI_TRUE, numberOfRecords));
      rfsaCheckWarn(niRFSA_Initiate(rfsaSession));

      IQData = (NIComplexNumberF32 *)malloc(sizeof(NIComplexNumberF32) * (int32)numberOfSamples);
      for (i = 0; i < numberOfRecords; i++)
      {
         if (i == 0)
            reset = 1;
         else
            reset = 0;

         rfsaCheckWarn(niRFSA_FetchIQSingleRecordComplexF32(rfsaSession, "", i, numberOfSamples, timeout,
            IQData, &IQDataWfmInfo));
         checkWarn(RFmxSpecAn_AnalyzeIQ1Waveform(instrumentHandle, "", "", IQDataWfmInfo.absoluteInitialX,
            IQDataWfmInfo.xIncrement, (NIComplexSingle*)IQData, (int32)IQDataWfmInfo.actualSamples, reset, reserved));
      }
   }
   else //Spectral Acquisition
   {
      RFmxCheckWarn(RFmxInstr_GetRecommendedSpectralResolutionBandwidth(instrumentHandle, "", &spectralRBW));
      RFmxCheckWarn(RFmxInstr_GetRecommendedSpectralFFTWindow(instrumentHandle, "", &spectralFFTWindow));

      rfsaCheckWarn(niRFSA_ConfigureAcquisitionType(rfsaSession, NIRFSA_VAL_SPECTRUM));

      switch (spectralFFTWindow)
      {
      case RFMXSPECAN_VAL_SPECTRUM_FFT_WINDOW_NONE: FFTWindowType = NIRFSA_VAL_UNIFORM;
         break;
      case RFMXSPECAN_VAL_SPECTRUM_FFT_WINDOW_FLAT_TOP: FFTWindowType = NIRFSA_VAL_FLAT_TOP;
         break;
      case RFMXSPECAN_VAL_SPECTRUM_FFT_WINDOW_HANNING: FFTWindowType = NIRFSA_VAL_HANNING;
         break;
      case RFMXSPECAN_VAL_SPECTRUM_FFT_WINDOW_HAMMING: FFTWindowType = NIRFSA_VAL_HAMMING;
         break;
      case RFMXSPECAN_VAL_SPECTRUM_FFT_WINDOW_GAUSSIAN: FFTWindowType = NIRFSA_VAL_GAUSSIAN;
         break;
      case RFMXSPECAN_VAL_SPECTRUM_FFT_WINDOW_BLACKMAN: FFTWindowType = NIRFSA_VAL_BLACKMAN;
         break;
      case RFMXSPECAN_VAL_SPECTRUM_FFT_WINDOW_BLACKMAN_HARRIS: FFTWindowType = NIRFSA_VAL_BLACKMAN_HARRIS;
         break;
      case RFMXSPECAN_VAL_SPECTRUM_FFT_WINDOW_KAISER_BESSEL: FFTWindowType = NIRFSA_VAL_KAISER_BESSEL;
         break;
      }

      rfsaCheckWarn(niRFSA_GetAttributeViReal64(rfsaSession, "", NIRFSA_ATTR_CENTER_FREQUENCY,
         &updatedCenterFrequency));
      rfsaCheckWarn(niRFSA_SetAttributeViReal64(rfsaSession, "", NIRFSA_ATTR_RESOLUTION_BANDWIDTH,
         spectralRBW));
      rfsaCheckWarn(niRFSA_SetAttributeViInt32(rfsaSession, "", NIRFSA_ATTR_FFT_WINDOW_TYPE, FFTWindowType));
      rfsaCheckWarn(niRFSA_SetAttributeViInt32(rfsaSession, "", NIRFSA_ATTR_RESOLUTION_BANDWIDTH_TYPE,
         NIRFSA_VAL_RBW_BIN_WIDTH));
      rfsaCheckWarn(niRFSA_SetAttributeViInt32(rfsaSession, "", NIRFSA_ATTR_POWER_SPECTRUM_UNITS,
         NIRFSA_VAL_DBM));
      rfsaCheckWarn(niRFSA_SetAttributeViReal64(rfsaSession, "", NIRFSA_ATTR_SPECTRUM_SPAN,
         spectralAcquisitionSpan));

      rfsaCheckWarn(niRFSA_GetNumberOfSpectralLines(rfsaSession, "", &numberOfSpectralLines));
      powerSpectrumData64 = (ViReal64*)malloc(sizeof(ViReal64) * numberOfSpectralLines);
      powerSpectrumData32 = (ViReal32*)malloc(sizeof(ViReal32) * numberOfSpectralLines);
      for (i = 0; i < numberOfRecords; i++)
      {
         if (i == 0)
            reset = 1;
         else
            reset = 0;

         rfsaCheckWarn(niRFSA_ReadPowerSpectrumF64(rfsaSession, "", timeout, powerSpectrumData64,
            numberOfSpectralLines, &spectrumInfo));

         spectrumInfo.initialFrequency = spectrumInfo.initialFrequency - updatedCenterFrequency;
         for (j = 0; j < numberOfSpectralLines; j++)
         {
            powerSpectrumData32[j] = (ViReal32)powerSpectrumData64[j];
         }
         checkWarn(RFmxSpecAn_AnalyzeSpectrum1Waveform(instrumentHandle, "", "", spectrumInfo.initialFrequency,
            spectrumInfo.frequencyIncrement, powerSpectrumData32, spectrumInfo.numberOfSpectralLines, reset, reserved));
      }
   }

   /* Retrieve results */

   if (offsetMeasArraySize > 0)
   {
      lowerRelativePower = (float64 *)malloc(sizeof(float64) * offsetMeasArraySize);
      upperRelativePower = (float64 *)malloc(sizeof(float64) * offsetMeasArraySize);
      lowerAbsolutePower = (float64 *)malloc(sizeof(float64) * offsetMeasArraySize);
      upperAbsolutePower = (float64 *)malloc(sizeof(float64) * offsetMeasArraySize);
      RFmxCheckWarn(RFmxSpecAn_ACPFetchOffsetMeasurementArray(instrumentHandle, "", timeout,
         lowerRelativePower, upperRelativePower,
         lowerAbsolutePower, upperAbsolutePower,
         offsetMeasArraySize, NULL));
   }
   RFmxCheckWarn(RFmxSpecAn_ACPFetchCarrierMeasurement(instrumentHandle, "", timeout,
      &absolutePower, NULL, NULL, NULL));

   actualArraySize = 0; x0 = 0.0; dx = 0.0;
   RFmxCheckWarn(RFmxSpecAn_ACPFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL, 0,
      &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32) * actualArraySize);
      if (spectrum)
      {
         RFmxCheckWarn(RFmxSpecAn_ACPFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }

   printf("Carrier Measurements: \n");
   printf("Absolute Power (dBm)                 %f\n", absolutePower);
   printf("---------------------------------------------------\n");

   printf("Offset Channel Measurements: \n");
   for (i = 0; i < offsetMeasArraySize; i++)
   {
      printf("Offset  :  %d\n", i);
      printf("Lower Relative Power (dB)            %f\n", lowerRelativePower[i]);
      printf("Upper Relative Power (dB)            %f\n", upperRelativePower[i]);
      printf("Lower Absolute Power (dBm)           %f\n", lowerAbsolutePower[i]);
      printf("Upper Absolute Power (dBm)           %f\n", upperAbsolutePower[i]);
      printf("-------------------------------------------------\n");
   }

Error:
   if (rfsaError)
   {
      errorOccured = rfsaError;
      niRFSA_GetError(rfsaSession, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      if (rfsaError < 0)
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
   if (instrumentHandle)
   {
      RFmxSpecAn_Close(instrumentHandle, RFMXSPECAN_VAL_FALSE);
   }
   if (rfsaSession)
   {
      niRFSA_close(rfsaSession);
   }
   /* Free allocated memory */
   if (IQData)
      free(IQData);
   if (powerSpectrumData64)
      free(powerSpectrumData64);
   if (powerSpectrumData32)
      free(powerSpectrumData32);
   if (spectrum)
      free(spectrum);
   if (lowerRelativePower)
      free(lowerRelativePower);
   if (upperRelativePower)
      free(upperRelativePower);
   if (upperAbsolutePower)
      free(upperAbsolutePower);
   if (lowerAbsolutePower)
      free(lowerAbsolutePower);

   printf("Press any key to exit\n");
   _getch();

   return errorOccured;
}
