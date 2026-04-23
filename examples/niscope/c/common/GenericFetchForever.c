/********************************************************************************
 *  Fetch Forever generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericFetchForever.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericFetchForever
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericFetchForever (void)
{
   ViStatus error = VI_SUCCESS;
   ViChar   errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar   errorMessage[MAX_ERROR_DESCRIPTION] = " ";
   ViSession vi;

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];
   ViReal64 verticalRange;
   ViReal64 minSampleRate;
   ViInt32 maxNumSamplesPerFetch;
   ViInt32 numWfms;
   ViInt32 stop;
   ViInt32 totalPointsFetched;

   // Default values used in this example
   ViReal64 verticalOffset = 0.0;
   ViInt32  verticalCoupling = NISCOPE_VAL_DC;
   ViReal64 probeAttenuation = 1.0;
   ViInt32  minRecordLength = 1;
   ViReal64 refPosition = 0.0;
   ViInt32  numRecords = 1;
   ViBoolean enforceRealtime = NISCOPE_VAL_TRUE;

   // Waveforms
   struct niScope_wfmInfo wfmInfo;
   ViReal64 *waveformPtr = NULL;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE,
                            NISCOPE_VAL_FALSE, &vi));

   // Obtain the necessary parameters from the user interface
   GetParametersFromGUI (channelName, &verticalRange, &minSampleRate,
                         &maxNumSamplesPerFetch);

   // Configure some common properties
   handleErr (niScope_ConfigureVertical (vi, channelName, verticalRange,
                                         verticalOffset, verticalCoupling,
                                         probeAttenuation, NISCOPE_VAL_TRUE));

   handleErr (niScope_ConfigureHorizontalTiming (vi, minSampleRate,
                                                 minRecordLength,
                                                 refPosition, numRecords,
                                                 enforceRealtime));

   // Configure software trigger, but never send the trigger.
   // This starts an infinite acquisition, until you call niScope_Abort
   // or niScope_close
   handleErr (niScope_ConfigureTriggerSoftware (vi, 0.0, 0.0));

   // Start acquiring data
   handleErr (niScope_InitiateAcquisition (vi));

   // This example only works with one channel, so we make sure the user
   // didn't specify 2 channels or multiple records.
   handleErr (niScope_ActualNumWfms (vi, channelName, &numWfms));
   if (numWfms > 1)
      handleErr (-1);

   // Allocate space for the waveform according to the max number of
   // points to fetch and the number of waveforms
   waveformPtr = malloc (sizeof (ViReal64) * maxNumSamplesPerFetch);
   if (waveformPtr == NULL)
      handleErr (VI_ERROR_ALLOC);

   totalPointsFetched = 0;
   stop = NISCOPE_VAL_FALSE;
   checkErr (niScope_SetAttributeViInt32 (vi, VI_NULL,
                                          NISCOPE_ATTR_FETCH_RELATIVE_TO,
                                          NISCOPE_VAL_READ_POINTER ));

   // Loop until the stop flag is set
   while (!stop)
   {
      // Fetch the data that is available without waiting
      handleErr (niScope_Fetch (vi, channelName, 0.0, maxNumSamplesPerFetch,
                                waveformPtr, &wfmInfo));

      // Add the total points fetched
      totalPointsFetched += wfmInfo.actualSamples;

      // Plot the waveform
      PlotWfms (waveformPtr, &wfmInfo, totalPointsFetched);

      // Find out whether to stop or not
      ProcessEvent (&stop);
   }

Error:
   // Free all the allocated memory
   if (waveformPtr)
      free (waveformPtr);

   // Display messages
   if (error == -1)
      strcpy (errorMessage, "This example only supports 1 channel.");
   else if (error != VI_SUCCESS)
      niScope_errorHandler (vi, error, errorSource, errorMessage);   // Intrepret the error
   else
      strcpy (errorMessage, "Acquisition successful!");

   DisplayErrorMessageInGUI (error, errorMessage);

   // Close the session
   if (vi)
      niScope_close (vi);

   return error;
}
