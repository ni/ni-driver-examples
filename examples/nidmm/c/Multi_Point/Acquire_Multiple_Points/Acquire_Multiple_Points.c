/***************************************************************************************/
/*                                                                                     */
/* National Instruments - NI-DMM - C Examples                                          */
/*                                                                                     */
/* Acquire Multiple Samples                                                            */
/*                                                                                     */
/* This example describes how to program a National Instruments Multimeter to perform  */
/* a multiple samples acquisition.                                                     */
/*                                                                                     */
/* Functions Utilized:                                                                 */
/*                                                                                     */
/* niDMM_init                                                                          */
/* niDMM_niDMM_GetAttributeViString                                                    */
/* niDMM_ConfigureCurrentSource                                                        */
/* niDMM_ConfigureMeasurementDigits                                                    */
/* niDMM_ConfigurePowerLineFrequency                                                   */
/* niDMM_Read                                                                          */
/* niDMM_Close                                                                         */
/*                                                                                     */
/***************************************************************************************/

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
   ViReal64  powerlineFreq       = NIDMM_VAL_60_HERTZ;

   ViReal64  range               = 10.00;
   ViReal64  coercedRange        = 10.00;

   ViReal64  resolution          = 5.5;
   ViInt32   numOfMeas           = 10;

   ViReal64  *measurements;
   ViReal64  average             = 0.00;
   ViInt32   numPointsRead;

   ViInt32   i;

   /*- Ask user to specify device --------------------------------------*/
   promptForResourceName(resourceName);

   /*- Dynamic Memory Allocation  --------------------------------------*/
   measurements = (ViReal64 *) malloc (numOfMeas * sizeof(ViReal64));

   /*- Initiate the DMM and create a new session -----------------------*/
   checkErr(niDMM_init(resourceName, idQuery, reset, &vi));

   /*- Configure Powerline Frequency -----------------------------------*/
   checkErr(niDMM_ConfigurePowerLineFrequency (vi, powerlineFreq));

   /*- Call NIDMM_ConfigureMeasurement() to set function, range and res */
   checkErr(niDMM_ConfigureMeasurementDigits (vi, measurementType, range, resolution));

   /*- Configure a multipoint acquisition ------------------------------*/
   checkErr(niDMM_ConfigureMultiPoint (vi, 1, numOfMeas, NIDMM_VAL_IMMEDIATE, 0.0));

   /*- ReadMultipoint will start the acquisition and fetch data --------*/
   checkErr(niDMM_ReadMultiPoint (vi, NIDMM_VAL_TIME_LIMIT_AUTO, numOfMeas, measurements, &numPointsRead));
   /*- Calculate the Average -------------------------------------------*/
   for(i=0; i<numOfMeas; i++) average = average + measurements[i];
   average = average / numPointsRead;

   /*- Get coerced range to set chart axis correctly -------------------*/
   niDMM_GetAttributeViReal64 (vi, "", NIDMM_ATTR_AUTO_RANGE_VALUE, &coercedRange);

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
      printf("numPointsRead =  %d\n", numPointsRead);
      printf("average = %lf\n", average);
      printf("coercedRange = %lf\n", coercedRange);
   }

   printf ("\n\n===================================\n\n");

   if(vi)
      niDMM_close(vi);

   return(0);
}
