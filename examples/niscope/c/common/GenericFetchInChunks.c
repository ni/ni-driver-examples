/********************************************************************************
 * Fetch in chunks generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericFetchInChunks.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericFetchInChunks
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericFetchInChunks (void)
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
   ViInt32 minRecordLength;
   ViInt32 triggerType;
   ViInt32 maxFetchSize;
   ViInt32 numWfms;

   // Default values used in this example
   ViReal64 verticalOffset = 0.0;
   ViInt32  verticalCoupling = NISCOPE_VAL_DC;
   ViReal64 probeAttenuation = 1.0;
   ViInt32  numRec = 1;
   ViBoolean enforceRealtime = NISCOPE_VAL_TRUE;
   ViReal64 timeout = 5.0;
   ViInt32 pointsFetched;
   ViInt32 chunksFetched;
   ViReal64 triggerLevel = 0.0;
   ViInt32 triggerSlope = NISCOPE_VAL_POSITIVE;
   ViInt32 triggerCoupling = NISCOPE_VAL_DC;
   ViReal64 triggerHoldoff = 0.0;
   ViReal64 triggerDelay = 0.0;
   ViReal64 refPosition = 25.0;

   // Waveforms
   struct niScope_wfmInfo wfmInfo;
   ViReal64 *waveformPtr = NULL;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Obtain the necessary parameters from the user interface
   GetParametersFromGUI (channelName, &verticalRange, &minSampleRate,
                         &minRecordLength, &triggerType, &maxFetchSize);

   // Configure the vertical parameters
   handleErr (niScope_ConfigureVertical (vi, channelName, verticalRange, verticalOffset,
                                         verticalCoupling, probeAttenuation, NISCOPE_VAL_TRUE));

   // Configure the horizontal parameters
   handleErr (niScope_ConfigureHorizontalTiming (vi, minSampleRate, minRecordLength,
                                                 refPosition, numRec, enforceRealtime));

   // This example only handles 1 waveform.  Check to make sure the user
   // hasn't specified a list of channels.
   handleErr (niScope_ActualNumWfms (vi, channelName, &numWfms));
   if (numWfms > 1)
      handleErr (-1);

   // Configure the trigger
   if (triggerType == 0) // Edge Trigger
   {
      handleErr (niScope_ConfigureTriggerEdge (vi, channelName,
                                               triggerLevel,
                                               triggerSlope,
                                               triggerCoupling,
                                               triggerHoldoff,
                                               triggerDelay));
   }
   else
   {
      handleErr (niScope_ConfigureTriggerImmediate (vi));
   }

   // Initiate the acquisition
   handleErr (niScope_InitiateAcquisition (vi));

   // Allocate space for 1 chunk of the waveform
   if (waveformPtr)
      free (waveformPtr);
   waveformPtr = (ViReal64*) malloc (sizeof (ViReal64) * maxFetchSize);

   // If it doesn't have enough memory, give an error message
   if (waveformPtr == NULL)
      handleErr (VI_ERROR_ALLOC);

   DisplayErrorMessageInGUI (error, "Acquisition in progress");

   pointsFetched = 0;
   chunksFetched = 0;

   // Loop until the min record length is reached
   while (pointsFetched < minRecordLength)
   {
      checkErr (niScope_SetAttributeViInt32 (vi, VI_NULL, NISCOPE_ATTR_FETCH_OFFSET, pointsFetched));

      // Fetch the data that is ready
      handleErr (niScope_Fetch (vi, channelName, timeout, maxFetchSize, waveformPtr, &wfmInfo));

      // Add the points fetched
      pointsFetched += wfmInfo.actualSamples;
      chunksFetched++;

      // Plot the total waveform (with all the points fetched until now)
      PlotWfms (pointsFetched, chunksFetched, waveformPtr, &wfmInfo);
   }

Error:
   // Free all the allocated memory
   if (waveformPtr)
      free (waveformPtr);

   // Display messages
   if (error == -1)
      strcpy (errorMessage, "Invalid channel list. This example only works with one channel.");
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
