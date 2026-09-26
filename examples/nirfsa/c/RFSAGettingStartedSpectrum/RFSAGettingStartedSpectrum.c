#include <stdio.h>
#include <stdlib.h>
#include "niRFSA.h"


/* Maximum length of an niRFSA function name */
#define MAX_FUNCTION_NAME_SIZE      55
/* Maximum size of an error message returned from niRFSA_GetError */
#define MAX_ERROR_DESCRIPTION       (IVI_MAX_MESSAGE_BUF_SIZE * 2 + MAX_FUNCTION_NAME_SIZE + 75)


int main (int argc, char *argv[])
{
   ViSession session = VI_NULL;
   ViInt32 numberOfSpectralLines;
   ViInt32 i;
   niRFSA_spectrumInfo spectrumInfo;
   ViReal64* spectrumDataPtr = NULL;
   ViReal64 greatestPeakPower;
   ViReal64 greatestPeakFrequency;
   ViStatus error = VI_SUCCESS;
   ViChar errorMessage[MAX_ERROR_DESCRIPTION];
   ViStatus lastErrorCode;

   /* Initialize a session */
   checkWarn(
      niRFSA_init ("PXI1Slot2", VI_TRUE, VI_FALSE, &session));
   
   /* Configure NI-RFSA for a simple IQ acquisition */
   checkWarn(
      niRFSA_ConfigureRefClock (session, "OnboardClock", 10E6));
   checkWarn(
      niRFSA_ConfigureReferenceLevel (session, "", 0));
   checkWarn(
      niRFSA_ConfigureAcquisitionType (session, NIRFSA_VAL_SPECTRUM));
   checkWarn(
      niRFSA_ConfigureSpectrumFrequencyStartStop (session, "", 990e6, 1010e6));
   checkWarn(
      niRFSA_ConfigureResolutionBandwidth (session, "", 10e3));

   /* Read the power spectrum */
   /* We need the number of spectral lines in order to know the size of the
    * spectrum array. */
   checkWarn(
      niRFSA_GetNumberOfSpectralLines (session, "", &numberOfSpectralLines));
   
   spectrumDataPtr = (ViReal64*)malloc(sizeof(ViReal64) * numberOfSpectralLines);

   checkWarn(
      niRFSA_ReadPowerSpectrumF64 (
         session, "", 10.0, spectrumDataPtr, numberOfSpectralLines, &spectrumInfo));

   /* Do something useful with the data */
   /* We will find the highest peak in a bin, which is not the actual highest
    * peak and frequency we could find in the acquisition.  For an accurate
    * peak search, we can analyze the data with the Spectral Measurements
    * Toolset. */
   for (i = 0; i < numberOfSpectralLines; ++i)
   {
      if (
            (i == 0) ||
            (spectrumDataPtr[i] > greatestPeakPower)
         )
      {
         greatestPeakPower = spectrumDataPtr[i];
         greatestPeakFrequency = spectrumInfo.initialFrequency + spectrumInfo.frequencyIncrement * i;
      }
   }

   printf("The highest peak in a bin is %0.1f dBm at %0.3f MHz.\n", greatestPeakPower, greatestPeakFrequency / 1e6);


Error:
   if (error < VI_SUCCESS)
   {
      niRFSA_GetError (session, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      printf("ERROR: %s\n", errorMessage);
   }
   else if (error > VI_SUCCESS)
   {
      niRFSA_GetError (session, &lastErrorCode, MAX_ERROR_DESCRIPTION, errorMessage);
      printf("WARNING: %s\n", errorMessage);
   }

   if (session)
      niRFSA_close(session);
   if (spectrumDataPtr)
      free (spectrumDataPtr);

   return 0;
}

