/********************************************************************************
 * Getting started generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericGettingStarted.h"

#define MAX_STRING_SIZE 50

//////////////////////////////////////////////////////////////////////////
// niScope_GenericGettingStarted
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericGettingStarted (void)
{
   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar errorMessage[MAX_ERROR_DESCRIPTION] = " ";

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];
   ViInt32 actualRecordLength;
   ViReal64 actualSampleRate;

   // Default values used in this example
   ViReal64 timeout = 5.000;

   // Waveforms
   struct niScope_wfmInfo wfmInfo;
   ViReal64 *waveformPtr = NULL;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);
   GetParametersFromGUI (channelName);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Call auto setup, finds a signal and configures all necessary parameters
   handleErr (niScope_AutoSetup (vi));

   // Get the actual record length and actual sample rate that will be used
   handleErr (niScope_ActualRecordLength (vi, &actualRecordLength));

   handleErr (niScope_SampleRate (vi, &actualSampleRate));

   waveformPtr = (ViReal64*) malloc (sizeof (ViReal64) * actualRecordLength);

   // If memory allocation fails, return an error message
   if (waveformPtr == NULL)
      handleErr (VI_ERROR_ALLOC);

   // Read the data (Initiate the acquisition, and fetch the data)
   handleErr (niScope_Read (vi, channelName, timeout, actualRecordLength, waveformPtr, &wfmInfo));

   // Plot the waveform
   PlotWfms (waveformPtr, &wfmInfo, actualRecordLength, actualSampleRate);

Error:
   if (waveformPtr)
      free (waveformPtr);

   // Display messages
   if (error != VI_SUCCESS)
      niScope_errorHandler (vi, error, errorSource, errorMessage);   // Intrepret the error
   else
      strcpy (errorMessage, "Acquisition successful!");

   DisplayErrorMessageInGUI (error, errorMessage);

   // Close the session
   if (vi)
      niScope_close (vi);

   return error;
}
