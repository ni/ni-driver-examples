/******************************************************************************/
/*                                                                            */
/* National Instruments - NI-DMM - C Examples                                 */
/*                                                                            */
/* Improve Stability with Auto Zero                                           */
/*                                                                            */
/* This examples describes how to program a National Instruments Multimeter   */
/* to perform multiple types of measurements with the Auto Zero option.       */
/*                                                                            */
/* Functions Utilized:                                                        */
/*                                                                            */
/* niDMM_init                                                                 */
/* niDMM_ConfigureMeasurementDigits                                           */
/* niDMM_ConfigurePowerLineFrequency                                          */
/* niDMM_ConfigureAutoZeroMode                                                */
/* niDMM_Read                                                                 */
/* niDMM_Close                                                                */
/*                                                                            */
/* DigitsToAbsoluteResolution                                                 */
/*                                                                            */
/*                                                                            */
/******************************************************************************/

#include "nidmm.h"
#include <stdio.h>
#include <string.h>
#include <math.h>
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
   ViChar    resourceName[256]      = "Dev1";
   ViSession vi                     = VI_NULL;
   ViBoolean idQuery                = VI_TRUE;
   ViBoolean reset                  = VI_TRUE;
   ViStatus  error                  = VI_SUCCESS;
   ViInt32   measurementType        = NIDMM_VAL_DC_VOLTS;
   ViReal64  powerlineFreq          = NIDMM_VAL_60_HERTZ;
   ViReal64  range                  = 0.1;
   ViReal64  resolution             = 5.5;
   ViReal64  absoluteResolution     = 0.0;
   ViReal64  reading                = 0.0;
   ViInt32   autoZero               = NIDMM_VAL_AUTO_ZERO_ON;

   /*- Ask user to specify device --------------------------------------*/
   promptForResourceName(resourceName);

   /*- Initiate the DMM ------------------------------------------------*/
   checkErr(niDMM_init (resourceName, idQuery, reset, &vi));

   /*- Configure Measurement -------------------------------------------*/
   checkErr(niDMM_ConfigureMeasurementDigits (vi, measurementType, range, resolution));

   /*- Configure Powerline Frequency -----------------------------------*/
   checkErr(niDMM_ConfigurePowerLineFrequency (vi, powerlineFreq));

   /*- Configure Auto Zero Mode ----------------------------------------*/
   checkErr(niDMM_ConfigureAutoZeroMode (vi, autoZero));

   /*- Read Measurement ------------------------------------------------*/
   checkErr(niDMM_Read (vi, NIDMM_VAL_TIME_LIMIT_AUTO, &reading));

   /*- Get Absolute Resolution -----------------------------------------*/
   checkErr(niDMM_GetAttributeViReal64 (vi, "", NIDMM_ATTR_RESOLUTION_ABSOLUTE, &absoluteResolution));

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
      printf("reading = %lf\n", reading);
      printf("absoluteResolution = %lf\n", absoluteResolution);
   }

   printf ("\n\n===================================\n\n");

   if(vi)
      niDMM_close(vi);

   return(0);
}


/******************************************************************************/
/* Definition of function DigitsToAbsoluteResolution                          */
/******************************************************************************/
void DigitsToAbsoluteResolution(ViSession vi, ViInt32 measurementType,
   ViReal64 range, ViReal64 resolution, ViReal64 *calculatedResolution)
{
   ViReal64 actualRange;
   ViReal64 logOfActualRange;
   ViInt32  coercedLogOfActualRange;
   ViChar   model[20] = "";
   ViStatus status = VI_SUCCESS;

   if(range < 0.0)
   {
      // in case that range value is not a specific range, but one of the
      // values that represents auto-range variations, the function
      // returns without changing *calculatedResolution
      return;
   }

   // Set the resolution equal to the range, so when range gets coerced up to
   // the appropriate range, resolution will still be close enough to be valid.
   status = niDMM_ConfigureMeasurement(vi, measurementType, range, range);

   // This error will be caught when configure measurement is called later
   // from the calling function.
   if(status < 0)
      return;

   niDMM_GetAttributeViReal64(vi, VI_NULL, NIDMM_ATTR_RANGE, &actualRange);

   niDMM_GetAttributeViString(vi, VI_NULL, NIDMM_ATTR_INSTRUMENT_MODEL, 20, model);

   if ( strstr(model,"4070") && (measurementType != NIDMM_VAL_DC_CURRENT) )
   {
      // For the 4070 we coerce the log of the actual range to the next higher
      // integer.
      coercedLogOfActualRange = (ViInt32)ceil(log10(actualRange));
   }
   else
   {
      // If we are not using a 4070 or the mode is DC Current, then we coerce the
      // log of the actual range to the nearest integer.
      logOfActualRange = log10(actualRange);

      if ( logOfActualRange < 0 )
      {
         coercedLogOfActualRange = (ViInt32)(logOfActualRange - 0.5);
      }
      else
      {
         coercedLogOfActualRange = (ViInt32)(logOfActualRange + 0.5);
      }
   }

   *calculatedResolution = pow (10, (coercedLogOfActualRange -
                              floor (resolution)));
}

/*****************************************************************************\
                           End example source code
\*****************************************************************************/
