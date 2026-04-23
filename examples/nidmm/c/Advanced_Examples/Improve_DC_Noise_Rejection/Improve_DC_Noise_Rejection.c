/******************************************************************************/
/*                                                                            */
/* National Instruments - NI-DMM - C Examples                                 */
/*                                                                            */
/* DC Noise Rejection                                                         */
/*                                                                            */
/* This example describes how to program a National Instruments Multimeter    */
/* to perform a multi-point acquisition and use the DC Noise Rejection        */
/* attribute to improve the measurement.                                      */
/*                                                                            */
/* The example uses "low level" "set-attribute" functions to configure the    */
/* instrument.                                                                */
/*                                                                            */
/* The example uses the following functions:                                  */
/*                                                                            */
/* niDMM_init                                                                 */
/* niDMM_SetAttributeViInt32                                                  */
/* niDMM_SetAttributeViReal64                                                 */
/* niDMM_ConfigureMultiPoint                                                  */
/* niDMM_ReadMultiPoint                                                       */
/* niDMM_Close                                                                */
/*                                                                            */
/* The example uses the following attributes:                                 */
/* NIDMM_ATTR_FUNCTION                                                        */
/* NIDMM_ATTR_RANGE                                                           */
/* NIDMM_ATTR_APERTURE_TIME                                                   */
/* NIDMM_ATTR_AUTO_ZERO_MODE                                                  */
/* NIDMM_ATTR_DC_NOISE_REJECTION                                              */
/*                                                                            */
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

   ViInt32   measurementType     = NIDMM_VAL_DC_VOLTS;
   ViReal64  range               = 10.0;
   ViReal64  resolution          = 5.5;
   ViReal64  apertureTime        = 0.1;
   ViInt32   autoZeroMode        = NIDMM_VAL_AUTO_ZERO_OFF;
   ViInt32   dcNoiseRejection    = NIDMM_VAL_DCNR_NORMAL;

   ViInt32   numOfMeas           = 10;

   ViReal64  *measurements       = VI_NULL;
   ViReal64  average             = 0.00;
   ViInt32   numPointsRead       = 0;
   ViInt32   i                   = 0;

   /*- Ask user to specify device --------------------------------------*/
   promptForResourceName(resourceName);

   /*- Dynamic Memory Allocation  --------------------------------------*/
   measurements = (ViReal64 *) malloc (numOfMeas * sizeof(ViReal64));

   /*- Initialize the DMM and create a new session -----------------------*/
   checkErr(niDMM_init(resourceName, idQuery, reset, &vi));

   checkErr(niDMM_ConfigureMeasurementDigits (vi, measurementType, range, resolution));

   checkErr(niDMM_SetAttributeViReal64 (vi, "", NIDMM_ATTR_APERTURE_TIME, apertureTime));

   checkErr(niDMM_SetAttributeViInt32 (vi, "", NIDMM_ATTR_AUTO_ZERO, autoZeroMode));

   checkErr(niDMM_SetAttributeViInt32 (vi, "", NIDMM_ATTR_DC_NOISE_REJECTION, dcNoiseRejection));

   checkErr(niDMM_ConfigureMultiPoint (vi, 1, numOfMeas, NIDMM_VAL_IMMEDIATE, 0.0));

   /*- ReadMultipoint will start the acquisition and fetch data --------*/
   checkErr(niDMM_ReadMultiPoint (vi, NIDMM_VAL_TIME_LIMIT_AUTO, numOfMeas, measurements, &numPointsRead));

   /*- Calculate the Average -------------------------------------------*/
   for(i=0; i<numOfMeas; i++) average = average + measurements[i];
   average = average / numPointsRead;

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
      printf("average = %lf\n", average);
   }

   printf ("\n\n===================================\n\n");

   if(vi)
      niDMM_close(vi);
   if(measurements)
      free(measurements);

   return(0);
}
