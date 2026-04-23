/******************************************************************************/
/*                                                                            */
/* National Instruments - NI-DMM - C Examples                                 */
/*                                                                            */
/* Continuous Measurement Multi-Functions Example                             */
/*                                                                            */
/* This examples describes how to program a National Instruments Multimeter   */
/* to perform a continuous acquisition.                                       */
/*                                                                            */
/* Functions Utilized:                                                        */
/*                                                                            */
/* niDMM_init                                                                 */
/* niDMM_ConfigurePowerLineFrequency                                          */
/* niDMM_ConfigureMeasurementDigits                                           */
/* niDMM_ConfigureMultiPoint                                                  */
/* niDMM_Initiate                                                             */
/* niDMM_ReadStatus                                                           */
/* niDMM_FetchMultiPoint                                                      */
/* niDMM_Close                                                                */
/*                                                                            */
/******************************************************************************/

#define MAXPTSTOREAD 1000

#include "nidmm.h"
#include <stdio.h>
#include <string.h>
#include <malloc.h>

ViBoolean StopMeas = VI_FALSE;

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

   ViInt32   measurementType     = NIDMM_VAL_DC_VOLTS;
   ViReal64  powerlineFreq       = NIDMM_VAL_60_HERTZ;

   ViReal64  range               = 10.00;
   ViReal64  coercedRange        = 10.00;

   ViReal64  resolution          = 5.5;
   ViInt32   numOfMeas           = 0;

   ViReal64  measurements[MAXPTSTOREAD];
   ViReal64  average             = 0.00;
   ViReal64  sum_meas            = 0.00;
   ViInt32   numPointsRead;

   ViInt32   i = 0;
   ViInt32   ptsAvailable        = 0;
   ViInt16   acqDone             = 0;

   /*- Ask user to specify device --------------------------------------*/
   promptForResourceName(resourceName);

   /*- Initiate the DMM and create a new session -----------------------*/
   checkErr(niDMM_init (resourceName, idQuery, reset, &vi));

   /*- Configure Measurement -------------------------------------------*/
   checkErr(niDMM_ConfigureMeasurementDigits (vi, measurementType, range, resolution));

   /*- Configure a Multipoint Acquisition ------------------------------*/
   checkErr(niDMM_ConfigureMultiPoint (vi, 1, 0, NIDMM_VAL_IMMEDIATE, 0.0));

   /*- Configure Powerline frequency -----------------------------------*/
   checkErr(niDMM_ConfigurePowerLineFrequency (vi, powerlineFreq));

   /*- Start or Initiate the Acquisition -------------------------------*/
   checkErr(niDMM_Initiate (vi));

   /*- Reseting While Loop Control -------------------------------------*/
   StopMeas = VI_FALSE;

   while (!StopMeas)
   {
      ptsAvailable = 0;
      /*- check for available data -*/
      checkErr(niDMM_ReadStatus(vi, &ptsAvailable, &acqDone));

      // if there are more than MAXPTSTOREAD measurements
      // available, set ptsAvailable to MAXPTSTOREAD in order to
      // avoid reallocating the array for measurements
      ptsAvailable = ptsAvailable < MAXPTSTOREAD ? ptsAvailable : MAXPTSTOREAD;

      if (ptsAvailable)
      {
         /*- Fetch the Data -------------------------------------------*/
    checkErr(niDMM_FetchMultiPoint(vi, NIDMM_VAL_TIME_LIMIT_AUTO, ptsAvailable,
                  measurements, &numPointsRead));

         /*- Incrementing Counter -------------------------------------*/
         numOfMeas = numOfMeas + numPointsRead;

         /*- Calculate the Average ------------------------------------*/
         for (i=0; i<numPointsRead; i++)
            sum_meas = sum_meas + measurements[i];
         average = sum_meas/numOfMeas;

         /*- Get coerced range to set chart axis correctly -------------------*/
         niDMM_GetAttributeViReal64 (vi, "", NIDMM_ATTR_AUTO_RANGE_VALUE, &coercedRange);

         //use the data here!
         //set StopMeas when done
         if(numOfMeas >= 10*MAXPTSTOREAD) StopMeas = 1;
      }
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
      printf("numOfMeas = %d\n", numOfMeas);
      printf("average = %lf\n", average);
   }

   printf ("\n\n===================================\n\n");

   if(vi)
      niDMM_close(vi);

   return(0);
}
