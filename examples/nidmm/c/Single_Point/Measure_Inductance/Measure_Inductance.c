/******************************************************************************/
/*                                                                            */
/* National Instruments - NI-DMM - C Examples                                 */
/*                                                                            */
/* Measure Inductance                                                         */
/*                                                                            */
/* This example describes how to program a National Instruments Multimeter    */
/* to perform a simple Inductance measurement.                                */
/*                                                                            */
/* Functions Utilized:                                                        */
/*                                                                            */
/* niDMM_init                                                                 */
/* niDMM_ConfigurePowerLineFrequency                                          */
/* niDMM_ConfigureMeasurementDigits                                           */
/* niDMM_Read                                                                 */
/* niDMM_Close                                                                */
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
   ViChar    resourceName[256]     = "Dev1";
   ViChar    modeString[64];
   ViChar    rangeString[64];
   ViChar    dataString[64];
   ViSession vi                    = VI_NULL;
   ViBoolean idQuery               = VI_TRUE;
   ViBoolean reset                 = VI_TRUE;
   ViStatus  error                 = VI_SUCCESS;
   ViInt32   measurementType       = NIDMM_VAL_INDUCTANCE;
   ViInt32   measurementsToAverage = 1;
   ViReal64  range                 = 100e-3;
   ViReal64  reading               = 0.000;
   ViReal64  quality               = 0.000;
   ViReal64  dissipation           = 0.000;
   // This resolution value will be ignored, but is necessary to be passed into
   // a function call.
   ViReal64  resolution            = 5.5;
   ViReal64  displayDigits         = 4.5;

   /*- Ask user to specify device --------------------------------------*/
   promptForResourceName(resourceName);

   /*- Initiate the DMM and create a session ---------------------------*/
   checkErr(niDMM_init(resourceName, idQuery, reset, &vi));

   /*- Configure Inductance Measurement --------------------------------*/
   checkErr(niDMM_ConfigureMeasurementDigits(vi, measurementType, range,
                resolution));

   /*- Configure Measurements to Average -------------------------------*/
   checkErr(niDMM_SetAttributeViInt32(vi, "", NIDMM_ATTR_LC_NUMBER_MEAS_TO_AVERAGE, measurementsToAverage));

   /*- Read Inductance -------------------------------------------------*/
   checkErr(niDMM_Read (vi, NIDMM_VAL_TIME_LIMIT_AUTO, &reading));

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

      /*- Format reading into string with units -------------------------*/
      niDMM_FormatMeas(measurementType,range,displayDigits,reading,
                       modeString,rangeString,dataString);
      printf("    Range:%7s %s\n",rangeString,modeString);
      printf("  Reading:%7s %s",dataString,modeString);

   }

   printf ("\n\n===================================\n\n");

   if(vi)
      niDMM_close(vi);

   return(0);
}
