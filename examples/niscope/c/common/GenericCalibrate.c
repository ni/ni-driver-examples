/********************************************************************************
 *  Calibrate generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericCalibrate.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericCalibrate
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericCalibrate(void)
{
   ViSession   vi = VI_NULL;
   ViStatus    error = VI_SUCCESS;
   ViChar      errorMessage[MAX_ERROR_DESCRIPTION] = " ";
   ViChar      errorSource[MAX_FUNCTION_NAME_SIZE];

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViInt32  option;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Obtain the necessary parameters from the user interface
   GetParametersFromGUI (&option);

   strcpy (errorMessage, "Calibration in progress (May take a couple of minutes)...");
   DisplayErrorMessageInGUI (VI_SUCCESS,errorMessage);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Calibrate the device according to the obtained option
   handleErr (niScope_CalSelfCalibrate (vi, "", option));

Error :

   // Display messages
   if (error != VI_SUCCESS)
      niScope_errorHandler (vi, error, errorSource, errorMessage);   // Interpret the error
   else
      strcpy(errorMessage, "Calibration Successful!");

   DisplayErrorMessageInGUI (error, errorMessage);

   // Close the session
   if (vi)
      niScope_close(vi);

   return error;
}
