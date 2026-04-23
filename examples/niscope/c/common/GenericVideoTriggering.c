/********************************************************************************
 * Video Triggering generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericVideoTriggering.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericVideoTriggering
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericVideoTriggering (void)
{
   ViStatus error = VI_SUCCESS;
   ViChar   errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar   errorMessage[MAX_ERROR_DESCRIPTION] = " ";
   ViSession vi;

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];
   ViChar triggerSource[MAX_STRING_SIZE];

   ViReal64 verticalRange;
   ViInt32 verticalCoupling;
   ViReal64 minSampleRate;
   ViInt32 minRecordLength;
   ViInt32 triggerCoupling;
   ViReal64 triggerDelay;
   ViReal64 refPosition;
   ViInt32 signalFormat;
   ViInt32 event;
   ViInt32 lineNumber;
   ViBoolean enableDCRestore;

   // Default values used in this example
   ViInt32  numRecords = 1;
   ViReal64 verticalOffset = 0.0;
   ViReal64 probeAttenuation = 1.0;
   ViBoolean enforceRealTime = NISCOPE_VAL_TRUE;
   ViReal64 timeout = 5.0;
   ViReal64 triggerHoldoff = 0.0;
   ViInt32 polarity = NISCOPE_VAL_TV_NEGATIVE;

   // Other variables
   ViInt32 stop = NISCOPE_VAL_FALSE;
   ViInt32 numWaveform;
   ViInt32 actualRecordLength;
   ViReal64 actualSampleRate;

   // Waveforms
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   ViReal64 *waveformPtr = NULL;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Loop until the stop flag is set
   while (!stop)
   {
      // Obtain the necessary parameters from the user interface
      GetParametersFromGUI (channelName, &verticalRange, &verticalCoupling, &minSampleRate, &minRecordLength,
                            &refPosition, triggerSource, &triggerCoupling, &triggerDelay,
                            &signalFormat, &event, &lineNumber, &enableDCRestore);

      // Configure the vertical parameters
      handleErr (niScope_ConfigureVertical (vi, channelName, verticalRange, verticalOffset,
                                           verticalCoupling, probeAttenuation, NISCOPE_VAL_TRUE));


      // Configure the horizontal parameters
      handleErr (niScope_ConfigureHorizontalTiming (vi, minSampleRate, minRecordLength,
                                                   refPosition, numRecords, enforceRealTime));

      // Configure the tv/video trigger
      niScope_ConfigureTriggerVideo (vi,triggerSource, enableDCRestore, signalFormat, event, lineNumber,
      								 polarity, triggerCoupling, triggerHoldoff, triggerDelay);

      // Initiate the acquisition
      handleErr (niScope_InitiateAcquisition (vi));

      // Find out the current record length and number of waveforms
      handleErr (niScope_ActualNumWfms (vi, channelName, &numWaveform));

      handleErr (niScope_ActualRecordLength (vi, &actualRecordLength));

      // Allocate space for the waveform and waveform info according to the
      // record length and number of waveforms
      if (wfmInfoPtr)
         free (wfmInfoPtr);
      wfmInfoPtr = malloc (sizeof (struct niScope_wfmInfo) * numWaveform);

      if (waveformPtr)
         free (waveformPtr);
      waveformPtr = (ViReal64*) malloc (sizeof (ViReal64) * actualRecordLength * numWaveform);

      // If it doesn't have enough memory, give an error message
      if (waveformPtr == NULL || wfmInfoPtr == NULL)
         handleErr (VI_ERROR_ALLOC);

      // Fetch the data
      handleErr (niScope_Fetch (vi, channelName, timeout, actualRecordLength,
                                waveformPtr, wfmInfoPtr));

      // Get the actual sample rate
      handleErr (niScope_SampleRate (vi, &actualSampleRate));

      // Plot the waveform
      PlotWfms (numWaveform, waveformPtr, wfmInfoPtr, actualSampleRate, actualRecordLength);

      // Find out wether to stop or not
      ProcessEvent ((int*)&stop);
   }


Error :

   // Free all the allocated memory
   if (wfmInfoPtr)
      free (wfmInfoPtr);

   if (waveformPtr)
      free (waveformPtr);

   // Display messages
   if (error != VI_SUCCESS)
      niScope_errorHandler (vi, error, errorSource, errorMessage);
   else
      strcpy (errorMessage, "Acquisition successful!");

   DisplayErrorMessageInGUI (error, errorMessage);

   // Close the session
   if (vi)
      niScope_close (vi);

   return error;
}
