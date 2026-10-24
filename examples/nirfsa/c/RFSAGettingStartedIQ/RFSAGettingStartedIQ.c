#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include "niRFSA.h"


/* Maximum length of an niRFSA function name */
#define MAX_FUNCTION_NAME_SIZE      55
/* Maximum size of an error message returned from niRFSA_GetError */
#define MAX_ERROR_DESCRIPTION       (IVI_MAX_MESSAGE_BUF_SIZE * 2 + MAX_FUNCTION_NAME_SIZE + 75)


int main (int argc, char *argv[])
{
   ViSession session = VI_NULL;
   NIComplexNumber* dataPtr = NULL;
   niRFSA_wfmInfo wfmInfo;
   ViInt32 numberOfSamples = 1000;
   ViInt32 i;
   ViReal64 magnitudeSquared;
   ViReal64 accumulator = 0.0;
   ViStatus error = VI_SUCCESS;
   ViChar errorMessage[MAX_ERROR_DESCRIPTION];
   ViStatus lastErrorCode;

   /* Initialize a session */
   checkWarn(
      niRFSA_init ("PXI1Slot2", VI_TRUE, VI_FALSE, &session));
   
   /* Configure NI-RFSA for a simple IQ acquisition */
   checkWarn(
      niRFSA_ConfigureRefClock (session, "OnboardClock", 10e6));
   checkWarn(
      niRFSA_ConfigureReferenceLevel (session, "", 0));
   checkWarn(
      niRFSA_ConfigureAcquisitionType (session, NIRFSA_VAL_IQ));
   checkWarn(
      niRFSA_ConfigureIQCarrierFrequency (session, "", 1.0e9));
   checkWarn(
      niRFSA_ConfigureNumberOfSamples (session, "", VI_TRUE, numberOfSamples));
   checkWarn(
      niRFSA_ConfigureIQRate (session, "", 1e6));

   /* Read the complex interleaved IQ data */
   dataPtr = (NIComplexNumber*)malloc (sizeof (ViReal64) * 2 * numberOfSamples);
      
   checkWarn(
      niRFSA_ReadIQSingleRecordComplexF64 (
         session, 
         "", 
         10.0,  /* seconds */
         dataPtr, 
         numberOfSamples, 
         &wfmInfo));

   /* Do something useful with the data */
   /* We will present average power: 10log(((I^2 + Q ^2) / 2R) * 1000), where
    * R = 50 Ohms */

   if (numberOfSamples > 0)
   {
      for (i = 0; i < numberOfSamples; ++i)
      {
         magnitudeSquared = dataPtr[i].real * dataPtr[i].real + dataPtr[i].imaginary * dataPtr[i].imaginary;

         /* we need to handle this because log(0) return a range error. */
         if (magnitudeSquared == 0.0)
         {
            magnitudeSquared = 0.00000001;
         }

         accumulator += 10.0 * log10((magnitudeSquared / (2.0 * 50.0)) * 1000.0);
      }

      printf("Average power = %0.1lf dBm\n", accumulator / numberOfSamples);
   }

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
   if (dataPtr)
      free (dataPtr);

   return 0;
}

