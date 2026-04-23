/******************************************************************************/
/*                                                                            */
/* National Instruments - NI-DMM - C Examples                                 */
/*                                                                            */
/* ReadWaveform                                                               */
/*                                                                            */
/* This example acquires a waveform. Set the waveform acquisition             */
/* function, range, sample rate, and number of points in the waveform.        */
/* The Read Waveform function is used to acquire the requested waveform.      */
/*                                                                            */
/* Functions Utilized:                                                        */
/*                                                                            */
/* niDMM_init                                                                 */
/* niDMM_ConfigureWaveformAcquisition                                         */
/* niDMM_ReadWaveform                                                         */
/* niDMM_Close                                                                */
/*                                                                            */
/******************************************************************************/

#include "nidmm.h"
#include <stdio.h>
#include <string.h>
#include <malloc.h>

int promptForResourceName(ViChar resourceName[] )
{
   //Get the device name from the user
   printf("Type the device name (e.g., DAQ::1, Dev1, PXI0Slot1, ...): ");
   fgets(resourceName, 255, stdin);
   resourceName[strlen(resourceName) - 1] = '\0'; // Remove newline character

   return 0;
}

void ErrorHandler(ViSession vi, ViStatus error)
{
   ViChar   *errorText;
   ViStatus errorCode = VI_SUCCESS;
   ViInt32  bufferSize = 0;

   if (error == VI_SUCCESS)
   {
      // No error, nothing to do
      return;
   }

   // First find the length of the error description.  Pass VI_NULL for the
   // buffer to retrieve the length of the error message.
   bufferSize = niDMM_GetError(vi, &errorCode, 0, VI_NULL);
   if(bufferSize > 0)
   {
      // Return code >0 from first call to GetError represents the size of
      // the description.  Call it again.
      // Ignore incoming IVI error code and return description from the driver
      // (trust that the IVI error code was properly stored in the session
      // by the driver)
      errorText = (ViChar *)malloc(bufferSize);
      niDMM_GetError(vi, &errorCode, bufferSize, errorText);
   }
   else
   {
      // Return code <= 0 from GetError indicates a problem.  This is expected
      // when the session is invalid (IVI spec requires GetError to fail).
      // Use GetErrorMessage instead.  It doesn't require a session.

      // Call niDMM_GetErrorMessage, pass VI_NULL for the buffer in order to retrieve
      // the length of the error message.
      errorCode = bufferSize;
      bufferSize = niDMM_GetErrorMessage(VI_NULL, errorCode, 0, VI_NULL);
      errorText = (ViChar *)malloc(bufferSize);

      // Get the error description
      niDMM_GetErrorMessage(VI_NULL, errorCode, bufferSize, errorText);
   }

   if(errorCode < VI_SUCCESS)
   {
      // Print Error
      printf("Failure; error = %d\n", error);
      printf("error msg: %s\n", errorText);
   }
   else
   {
      // Print Warning
      printf("warning = %d\n", error);
      printf("warning msg: %s\n\n", errorText);
   }

   free(errorText);
   return;
}

int main (int argc, char *argv[])
{
   /*- Variables definition --------------------------------------------------*/

   ViChar    resourceName[256]   = "Dev1";
   ViSession vi                  = VI_NULL;
   ViBoolean idQuery             = VI_TRUE;
   ViBoolean reset               = VI_TRUE;
   ViStatus  error               = VI_SUCCESS;

   ViInt32   acquisitionType     = NIDMM_VAL_WAVEFORM_VOLTAGE;

   ViReal64  range               = 10;
   ViReal64  rate                = 1800000.0;
   ViInt32   numOfSamples        = 1000;

   ViReal64  *waveform           = VI_NULL;
   ViInt32   numSamplesAcquired  = 0;

   /*- Allocate memory for the waveform data ---------------------------*/
   waveform = (ViReal64*) malloc (numOfSamples*sizeof(ViReal64));
   if (waveform == VI_NULL)
   {
      // go to Error label if memory allocation fails
      checkErr(-1);
   }

   /*- Ask user to specify device --------------------------------------*/
   promptForResourceName(resourceName);

   /*- Initiatlize the DMM and create a new session --------------------*/
   checkErr(niDMM_init (resourceName, idQuery, reset, &vi));

   /*- Call NIDMM_ConfigureWaveformAcquisition() to set function, range,
       rate, and the number of samples ---------------------------------*/
   checkErr(niDMM_ConfigureWaveformAcquisition (vi, acquisitionType, range, rate, numOfSamples));

   /*- ReadWaveform will start the acquisition and fetch the waveform --*/
   checkWarn(niDMM_ReadWaveform (vi, NIDMM_VAL_TIME_LIMIT_AUTO, numOfSamples,
             waveform, &numSamplesAcquired));

Error:

   printf ("\n\n===================================\n\n");

   if (error < VI_SUCCESS)
   {
      ErrorHandler(vi, error);
   }
   else
   {
      if (error)
      {
         // Function call returned a warning, print it first
         ErrorHandler(vi, error);
      }
      printf("numSamplesAcquired = %d\n", numSamplesAcquired);
   }

   printf ("\n\n===================================\n\n");

   if(vi)
      niDMM_close(vi);

   /*- free allocated memory -------------------------------------------------*/
   if (waveform)
      free (waveform);

   return(0);
}
