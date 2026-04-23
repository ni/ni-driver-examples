/********************************************************************************
 * Random Interleaved Sampling generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericRandomInterleavedSampling.h"

#define MAX_DRIVER_REVISION 100
#include <math.h> // for the ceil function

//////////////////////////////////////////////////////////////////////////
// niScope_GenericRandomInterleavedSampling
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericRandomInterleavedSampling (void)
{
   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar errorMessage[MAX_ERROR_DESCRIPTION] = " ";

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];
   ViReal64 maxRealTimeSampleRate;
   ViReal64 verticalRange, minRISRate;
   ViInt32 minRISRecordLength, minRealTimeRecordLength;
   ViInt32 numAverages, numWaveform, risMethod;
   ViReal64 actualRealtimeSampleRate, actualRISSampleRate;
   ViInt32 oversamplingFactor;

   // Default values used in this example
   ViReal64 timeout = 10.000;
   ViReal64 verticalOffset = 0.0;
   ViInt32  verticalCoupling = NISCOPE_VAL_DC;
   ViReal64 probeAttenuation = 1.0;
   ViReal64 refPosition = 50.0;
   ViInt32  numRecords = 1;
   ViBoolean enforceRealtime = NISCOPE_VAL_FALSE;
   ViReal64 triggerLevel = 0.0;
   ViReal64 triggerHoldoff = 0.0;
   ViReal64 triggerDelay = 0.0;
   ViInt32 triggerSlope = NISCOPE_VAL_POSITIVE;
   ViInt32 triggerCoupling = NISCOPE_VAL_DC;

   // Waveforms
   struct niScope_wfmInfo wfmInfo;
   ViReal64 *realtimeWfmPtr = NULL;
   struct niScope_wfmInfo risWfmInfo;
   ViReal64 *risWfmPtr = NULL;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Obtain the necessary parameters from the user interface
   GetParametersFromGUI (channelName, &verticalRange, &minRISRate,
                         &minRISRecordLength, &numAverages, &risMethod);

   // Set the attributes for the RIS method and number of averages
   checkErr (niScope_SetAttributeViInt32 (vi, VI_NULL, NISCOPE_ATTR_RIS_NUM_AVERAGES,
                                          numAverages));
   checkErr (niScope_SetAttributeViInt32 (vi, VI_NULL, NISCOPE_ATTR_RIS_METHOD,
                                          risMethod));

   // Get the max real time sample rate for this board
   checkErr (niScope_GetAttributeViReal64 (vi, VI_NULL,
                                          NISCOPE_ATTR_MAX_REAL_TIME_SAMPLING_RATE,
                                          &maxRealTimeSampleRate));

   // Calculate the oversampling factor and the equivalent record length for the RIS rate
   oversamplingFactor = (ViInt32) ceil (minRISRate / maxRealTimeSampleRate);
   minRISRate = oversamplingFactor * maxRealTimeSampleRate;
   minRealTimeRecordLength = minRISRecordLength / oversamplingFactor;

   // Perform an acquisition in Real Time
   // Configure the horizontal parameters (real time rate and record length)
   handleErr (niScope_ConfigureHorizontalTiming (vi, maxRealTimeSampleRate, minRealTimeRecordLength,
                                                 refPosition, numRecords, enforceRealtime));

   // Configure the vertical parameters
   handleErr (niScope_ConfigureVertical (vi, channelName, verticalRange, verticalOffset,
                                         verticalCoupling, probeAttenuation,
                                         NISCOPE_VAL_TRUE));

   // Find out the requested number of waveforms.  Since this example only handles 1,
   // return an error if more are requested.
   handleErr (niScope_ActualNumWfms (vi, channelName, &numWaveform));
   if (numWaveform > 1)
      handleErr (-1);

   handleErr (niScope_ConfigureTriggerEdge (vi, channelName, triggerLevel,
                                           triggerSlope, triggerCoupling,
                                           triggerHoldoff, triggerDelay));

   // Allocate space for the waveforms according to the minRecordLengths
   realtimeWfmPtr = (ViReal64*) malloc (sizeof (ViReal64) * minRealTimeRecordLength);
   risWfmPtr = (ViReal64*) malloc (sizeof (ViReal64) * minRISRecordLength);
   if (realtimeWfmPtr == NULL || risWfmPtr == NULL)
      handleErr (VI_ERROR_ALLOC);

   // Acquire the real time data
   handleErr (niScope_Read (vi, channelName, timeout, minRealTimeRecordLength,
                            realtimeWfmPtr, &wfmInfo));

   // Get the actual realtime sample rate
   handleErr (niScope_SampleRate (vi, &actualRealtimeSampleRate));

   // Now perform an RIS acquisition
   // Configure the horizontal parameters (RIS rate and record length)
   handleErr (niScope_ConfigureHorizontalTiming (vi, minRISRate, minRISRecordLength, refPosition,
                                                 numRecords, enforceRealtime));

   // Acquire the RIS data
   handleErr (niScope_Read (vi, channelName, timeout, minRISRecordLength,
                            risWfmPtr, &risWfmInfo));

   // Get the actual RIS sample rate
   handleErr (niScope_SampleRate (vi, &actualRISSampleRate));

   // Plot both real time and RIS waveforms
   PlotWfms (realtimeWfmPtr, &wfmInfo, risWfmPtr, &risWfmInfo,
             actualRealtimeSampleRate, actualRISSampleRate, oversamplingFactor);

Error :

   // Free all the allocated memory
   if (realtimeWfmPtr)
      free (realtimeWfmPtr);

   if (risWfmPtr)
      free (risWfmPtr);

   // Display messages
   if (error == -1)
      strcpy (errorMessage, "This example only works with one channel.");
   else if (error != VI_SUCCESS)
      niScope_errorHandler (vi, error, errorSource, errorMessage);
   else
      strcpy (errorMessage, "Acquisition successful!");

   DisplayErrorMessageInGUI (error, errorMessage);

   // Close the session
   if (vi)
     niScope_close (vi);

   return error;
}
