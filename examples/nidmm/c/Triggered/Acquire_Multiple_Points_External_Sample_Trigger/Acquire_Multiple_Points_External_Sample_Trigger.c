/******************************************************************************/
/*                                                                            */
/* National Instruments - NI-DMM - C Examples                                 */
/*                                                                            */
/* Acquire Multiple Samples - External Sample Trigger                         */
/*                                                                            */
/* This examples describes how to program a National Instruments Multimeter   */
/* to perform a multiple samples acquisition with external sample triggering. */
/* The external trigger is an external digital pulse when the trigger sample  */
/* is set for "External Trigger"                                              */
/*                                                                            */
/* Utilized Functions:                                                        */
/*                                                                            */
/* niDMM_init                                                                 */
/* niDMM_ConfigurePowerLineFrequency                                          */
/* niDMM_ConfigureMeasurementDigits                                           */
/* niDMM_ConfigureTrigger                                                     */
/* niDMM_ConfigureMultiPoint                                                  */
/* niDMM_Initiate                                                             */
/* niDMM_SendSoftwareTrigger                                                  */
/* niDMM_FetchMultiPoint                                                      */
/* niDMM_Fetch                                                                */
/* niDMM_Close                                                                */
/*                                                                            */
/*                                                                            */
/******************************************************************************/

#include "nidmm.h"
#include <stdio.h>
#include <string.h>
#include <malloc.h>

ViBoolean  SWTrigger = VI_FALSE;
ViBoolean  RequestToQuit = VI_FALSE;

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
   ViChar    resourceName[256]     = "Dev1";
   ViSession vi                    = VI_NULL;
   ViBoolean idQuery               = VI_TRUE;
   ViBoolean reset                 = VI_TRUE;
   ViStatus  error                 = VI_SUCCESS;

   ViInt32   measurementType       = NIDMM_VAL_DC_VOLTS;
   ViReal64  powerlineFreq         = NIDMM_VAL_60_HERTZ;

   ViReal64  range                 = 10.00;
   ViReal64  resolution            = 5.5;
   ViInt32   numOfMeas             = 10;

   ViReal64  reading               = 0.00;
   ViReal64  *measurements         = VI_NULL;
   ViReal64  average               = 0.00;
   ViReal64  sum_meas              = 0.00;
   ViInt32   timeoutInMilliSeconds = 5000;
   ViInt32   numPointsRead         = 0;

   ViInt32   triggerSource         = NIDMM_VAL_EXTERNAL;
   ViReal64  triggerDelay          = 0.00;
   ViInt32   triggerSample         = NIDMM_VAL_EXTERNAL;

   ViInt32   i;

   /*- Ask user to specify device --------------------------------------*/
   promptForResourceName(resourceName);

   /*- Dynamic Memory Allocation  --------------------------------------*/
   measurements = (ViReal64 *) malloc(numOfMeas * sizeof(ViReal64));

   /*- Initialize the DMM and create a new Session ---------------------*/
   checkErr(niDMM_init(resourceName, idQuery, reset, &vi));

   /*- Configure Powerline frequency -----------------------------------*/
   checkErr(niDMM_ConfigurePowerLineFrequency(vi, powerlineFreq));

   /*- Configure Measurement -------------------------------------------*/
   checkErr(niDMM_ConfigureMeasurementDigits(vi, measurementType, range, resolution));

   /*- Configure Trigger -----------------------------------------------*/
   checkErr(niDMM_ConfigureTrigger (vi, triggerSource, triggerDelay));

   /*- Configure Multipoint acquisition --------------------------------*/
   checkErr(niDMM_ConfigureMultiPoint (vi, 1, numOfMeas, triggerSample, triggerDelay));

   /*- Start or initiate the acquisition -------------------------------*/
   checkErr(niDMM_Initiate(vi));

   /*- Fetch first measurement since Trigger Source is immediate--------*/
   checkErr(niDMM_Fetch (vi, timeoutInMilliSeconds, measurements));

   if(triggerSample == NIDMM_VAL_SOFTWARE_TRIG)
   {
      for(i=0; i < numOfMeas - 1 ; i++)
      {
         checkErr(niDMM_SendSoftwareTrigger (vi));

         /*- Fetch Measurement -----------------------------------------*/
         checkErr(niDMM_Fetch (vi, timeoutInMilliSeconds, measurements));
         //note numPointsRead never gets set in this code path
      }
   }
   else
   {
      /*- Fetch the data -----------------------------------------------*/
      checkErr(niDMM_FetchMultiPoint(vi, timeoutInMilliSeconds,
               numOfMeas - 1, measurements, &numPointsRead));
   }

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
      printf("numPointsRead = %d\n", numPointsRead);
   }

   printf ("\n\n===================================\n\n");

   if(vi)
      niDMM_close(vi);

   if(measurements)
      free (measurements);

   return(0);
}
