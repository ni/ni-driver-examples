//Steps:
//1. Open a new RFmx session.
//2. Configure the basic instrument properties(Clock Source, Clock Frequency).
//3. Configure Selected Ports.
//4. Configure the Center Frequency, Span or Start, Stop Frequency based on the Tab Selection.
//5. Configure the basic signal properties(Reference Level, External Attenuation).
//6. Select Spectrum measurement and enable the traces.
// 7. Configure Measurement Method
// 8. Configure RBW Filter Parameters.
// 9. Configure Power Units.
// 10. Configure Sweep Time.
// 11. Configure Spectrum Averaging.
// 12. Configure FFT parameters.
// 13. Configure Noise Compensation Enabled.
// 14. Configure Detectors.
// 15. Configure VBW Filter Parameters.
// 16. If Measurement Method is Sequential FFT, Configure Sequential FFT Parameters.
// 17. Configure Cleaner Spectrum.
// 18. Initiate Measurement.
// 19. Fetch Spectrum Traces and Measurements.
// 20. Close the RFmx Session.

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include "niRFmxSpecAn.h"

/* Maximum size of an error message */
#define MAX_ERROR_DESCRIPTION             4096

#define CONFIGURE_START_STOP_FREQ_OFF     0
#define CONFIGURE_START_STOP_FREQ_ON      1

int main(int argc, char *argv[])
{
   niRFmxInstrHandle instrumentHandle = NULL;
   int32 error = 0, lastErrorCode = 0;
   char errorMessage[MAX_ERROR_DESCRIPTION];

   char *resourceName = "RFSA";
   char *frequencySource = RFMXINSTR_VAL_ONBOARD_CLOCK_STR;
   char *selectedPorts = "";
   float64 centerFrequency = 1e+9;                                /* Hz */
   float64 referenceLevel = 0.00;                                 /* dBm */
   float64 externalAttenuation = 0.00;                            /* dB */
   float64 frequency = 10.0e+6;                                   /* Hz */
   float64 startFrequency = 9.95e+8;                              /* Hz */
   float64 stopFrequency = 1.005e+9;                              /* Hz */
   int32 isCfgStartStopFreqValid = CONFIGURE_START_STOP_FREQ_OFF;

   float64 timeout = 10.0;                                        /* seconds */
   /* Measurement Method*/
   int32 measurementMethod = RFMXSPECAN_VAL_SPECTRUM_MEASUREMENT_METHOD_NORMAL;

   /* Cleaner Spectrum*/
   int32 cleanerSpectrum = RFMXINSTR_VAL_CLEANER_SPECTRUM_DISABLED;

   /* Power Units */
   int32 powerUnits = RFMXSPECAN_VAL_SPECTRUM_POWER_UNITS_DBM;

   /* Span */
   float64 span = 1.0e+6;                                         /* Hz */

   /* RBW Filter */
   int32 RBWFilterType = RFMXSPECAN_VAL_SPECTRUM_RBW_FILTER_TYPE_GAUSSIAN;
   int32 RBWAuto = RFMXSPECAN_VAL_SPECTRUM_RBW_AUTO_TRUE;
   float64 RBW = 10.0e+3;                                         /* Hz */

   /* VBW */
   int32 VBWAuto = RFMXSPECAN_VAL_SPECTRUM_VBW_FILTER_AUTO_BANDWIDTH_TRUE;
   float64 VBW = 30.0e3;                                          /* Hz */
   float64 VBWToRBWRatio = 3;

   /* Detectors */
   int32 detectorType = RFMXSPECAN_VAL_SPECTRUM_DETECTOR_TYPE_NONE;
   int32 detectorPoints = 1001;

   /* Sweep time */
   int32 sweepTimeAuto = RFMXSPECAN_VAL_SPECTRUM_SWEEP_TIME_AUTO_TRUE;
   float64 sweepTimeInterval = 1.00e-3;                           /* seconds */

   /* Averaging */
   int32 averagingEnabled = RFMXSPECAN_VAL_SPECTRUM_AVERAGING_ENABLED_FALSE;
   int32 averagingCount = 10;
   int32 averagingType = RFMXSPECAN_VAL_SPECTRUM_AVERAGING_TYPE_RMS;

   /* FFT */
   int32 FFTWindow = RFMXSPECAN_VAL_SPECTRUM_FFT_WINDOW_FLAT_TOP;
   float64 FFTPadding = -1.0;
   int32 FFTOverlapMode = RFMXSPECAN_VAL_SPECTRUM_FFT_OVERLAP_MODE_DISABLED;
   float64 FFTOverlapPercent = 0;                                /* Percentage */
   int32 FFTOverlapType = RFMXSPECAN_VAL_SPECTRUM_FFT_OVERLAP_TYPE_RMS;
   int32 sequentialFFTSize = 512;

   int32 noiseCompensationEnabled = RFMXSPECAN_VAL_SPECTRUM_NOISE_COMPENSATION_ENABLED_FALSE;

   /* Variables to store the measurement results */
   float64 x0 = 0.0, dx = 0.0;                                    /* Variables to store the spectrum */
   float32 *spectrum = (float32 *)NULL;
   int32 actualArraySize;

   float64 peakAmplitude = 0;
   float64 peakFrequency = 0;                                     /* Hz */

   /* Create a new RFmx Session */
   RFmxCheckWarn(RFmxSpecAn_Initialize(resourceName, "", &instrumentHandle, NULL));

   /* Configure Spectrum measurement parameters */
   RFmxCheckWarn(RFmxSpecAn_CfgFrequencyReference(instrumentHandle, "", frequencySource, frequency));
   RFmxCheckWarn(RFmxSpecAn_SetSelectedPorts(instrumentHandle, "", selectedPorts));
   if (isCfgStartStopFreqValid)
   {
      RFmxCheckWarn(RFmxSpecAn_SpectrumCfgFrequencyStartStop(instrumentHandle, "", startFrequency, stopFrequency));
   }
   else
   {
      RFmxCheckWarn(RFmxSpecAn_CfgFrequency(instrumentHandle, "", centerFrequency));
      RFmxCheckWarn(RFmxSpecAn_SpectrumCfgSpan(instrumentHandle, "", span));
   }
   RFmxCheckWarn(RFmxSpecAn_CfgReferenceLevel(instrumentHandle, "", referenceLevel));
   RFmxCheckWarn(RFmxSpecAn_CfgExternalAttenuation(instrumentHandle, "", externalAttenuation));
   RFmxCheckWarn(RFmxSpecAn_SelectMeasurements(instrumentHandle, "", RFMXSPECAN_VAL_SPECTRUM, RFMXSPECAN_VAL_TRUE));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgMeasurementMethod(instrumentHandle, "", measurementMethod));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgRBWFilter(instrumentHandle, "", RBWAuto, RBW, RBWFilterType));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgPowerUnits(instrumentHandle, "", powerUnits));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgSweepTime(instrumentHandle, "", sweepTimeAuto, sweepTimeInterval));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgAveraging(instrumentHandle, "", averagingEnabled, averagingCount,
      averagingType));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgFFT(instrumentHandle, "", FFTWindow, FFTPadding));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgNoiseCompensationEnabled(instrumentHandle, "", noiseCompensationEnabled));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgDetector(instrumentHandle, "", detectorType, detectorPoints));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgVBWFilter(instrumentHandle, "", VBWAuto, VBW, VBWToRBWRatio));
   RFmxCheckWarn(RFmxSpecAn_SpectrumCfgVBWFilter(instrumentHandle, "", VBWAuto, VBW, VBWToRBWRatio));
   RFmxCheckWarn(RFmxSpecAn_SpectrumSetFFTOverlapMode(instrumentHandle, "", FFTOverlapMode));
   RFmxCheckWarn(RFmxSpecAn_SpectrumSetFFTOverlap(instrumentHandle, "", FFTOverlapPercent));
   RFmxCheckWarn(RFmxSpecAn_SpectrumSetFFTOverlapType(instrumentHandle, "", FFTOverlapType));
   RFmxCheckWarn(RFmxSpecAn_SpectrumSetSequentialFFTSize(instrumentHandle, "", sequentialFFTSize));
   RFmxCheckWarn(RFmxInstr_SetCleanerSpectrum(instrumentHandle, "", cleanerSpectrum));
   RFmxCheckWarn(RFmxSpecAn_Initiate(instrumentHandle, "", ""));

   /* Retrieve results */
   RFmxCheckWarn(RFmxSpecAn_SpectrumFetchSpectrum(instrumentHandle, "", timeout, NULL, NULL, NULL,
      0, &actualArraySize));
   if (actualArraySize > 0)
   {
      spectrum = (float32 *)malloc(sizeof(float32)*actualArraySize);
      if (spectrum)
      {
         RFmxCheckWarn(RFmxSpecAn_SpectrumFetchSpectrum(instrumentHandle, "", timeout, &x0, &dx, spectrum,
            actualArraySize, NULL));
      }
      else
      {
         printf("malloc failed.\n");
         goto Error;
      }
   }
   RFmxCheckWarn(RFmxSpecAn_SpectrumFetchMeasurement(instrumentHandle, "", timeout, &peakAmplitude,
      &peakFrequency, NULL));

   /* Display results */
   printf("Peak Amplitude(dBm)            %f\n", peakAmplitude);
   printf("Peak Frequency(Hz)             %f\n", peakFrequency);
Error:
   if (error)
   {
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
   /* Free allocated memory */
   if (spectrum)
      free(spectrum);
   printf("Press any key to exit\n");
   _getch();

   return error;
}
