/********************************************************************************
 * Multi Record generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericMultiRecord.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericMultiRecord
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericMultiRecord(void)
{
   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar errorMessage[MAX_ERROR_DESCRIPTION] = " ";

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];
   ViReal64 verticalRange;
   ViReal64 minSampleRate;
   ViInt32 minRecordLength;
   ViInt32 numRecords;
   ViReal64 actualSampleRate;
   ViInt32 actualRecordLength;
   ViInt32 numWaveform;

   // Default values used in this example
   ViReal64 refPosition = 50.0;
   ViReal64 verticalOffset = 0.0;
   ViInt32  verticalCoupling = NISCOPE_VAL_DC;
   ViReal64 probeAttenuation = 1.0;
   ViBoolean enforceRealtime = NISCOPE_VAL_TRUE;
   ViReal64 timeout = 5.000;
   ViReal64 triggerLevel = 0.0;
   ViInt32 triggerSlope = NISCOPE_VAL_POSITIVE;
   ViInt32 triggerCoupling = NISCOPE_VAL_DC;
   ViReal64 triggerHoldoff = 0.0;
   ViReal64 triggerDelay = 0.0;

   // Waveforms
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   ViReal64 *waveformPtr = NULL;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr(niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Obtain the necessary parameters from the user interface
   GetParametersFromGUI (channelName, &verticalRange, &minSampleRate,
                         &minRecordLength, &numRecords);

   // Configure the vertical parameters
   handleErr(niScope_ConfigureVertical(vi, channelName, verticalRange, verticalOffset,
                              verticalCoupling, probeAttenuation, NISCOPE_VAL_TRUE));

   // Configure the horizontal parameters, with the specified number of records
   handleErr(niScope_ConfigureHorizontalTiming(vi, minSampleRate, minRecordLength, refPosition, numRecords, enforceRealtime));

   // Find out the current record length and number of waveforms
   handleErr(niScope_ActualNumWfms(vi, channelName, &numWaveform));
   if (numWaveform != numRecords)
      handleErr (-1);

   handleErr(niScope_ConfigureTriggerEdge (vi, channelName, triggerLevel,
                                           triggerSlope, triggerCoupling,
                                           triggerHoldoff, triggerDelay));

   // Initiate the acquisition
   handleErr(niScope_InitiateAcquisition(vi));

   // Find out the actual sample rate
   handleErr(niScope_SampleRate(vi, &actualSampleRate));

   handleErr(niScope_ActualRecordLength (vi, &actualRecordLength));

   // Allocate space for the waveform and waveform info according to the record length and number of waveforms
   if (wfmInfoPtr)
      free (wfmInfoPtr);
   wfmInfoPtr = malloc(sizeof(struct niScope_wfmInfo) * numWaveform);

   if (waveformPtr)
      free (waveformPtr);
   waveformPtr = (ViReal64*) malloc (sizeof(ViReal64) * actualRecordLength * numWaveform);

   // If it doesn't have enough memory, give an error message
   if (waveformPtr == NULL || wfmInfoPtr == NULL)
      handleErr (VI_ERROR_ALLOC);

   // Fetch the data
   handleErr(niScope_Fetch (vi, channelName, timeout, actualRecordLength,  waveformPtr, wfmInfoPtr));

   // Plot the waveforms
   PlotWfms (numWaveform, waveformPtr, wfmInfoPtr, actualSampleRate, actualRecordLength);

Error :

   // Free all the allocated memory
   if (wfmInfoPtr)
      free (wfmInfoPtr);

   if (waveformPtr)
      free (waveformPtr);

   // Display messages
   if (error == -1)
      strcpy (errorMessage, "This example only works with one channel.");
   else if (error != VI_SUCCESS)
      niScope_errorHandler (vi, error, errorSource, errorMessage);   // Intrepret the error
   else
      strcpy(errorMessage, "Acquisition successful!");

   DisplayErrorMessageInGUI (error, errorMessage);

   // Close the session
   if(vi)
     niScope_close(vi);

   return error;
}

