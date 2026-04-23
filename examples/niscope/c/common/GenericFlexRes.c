/********************************************************************************
 * Flex Res generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericFlexRes.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericFlexRes
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericFlexRes (void)
{
   ViStatus error = VI_SUCCESS;
   ViChar   errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar   errorMessage[MAX_ERROR_DESCRIPTION] = " ";
   ViSession vi;

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViReal64 verticalRange;
   ViReal64 minSampleRate;
   ViInt32 minRecordLength;
   ViInt32 triggerType;
   ViInt32 numWaveform;
   ViInt32 actualRecordLength;
   ViReal64 actualSampleRate;
   ViInt32 resolution;
   ViInt32 arraySize;

   // Default values used in this example
   ViInt32 stop;
   ViConstString channel = "0";
   ViInt32 verticalCoupling = NISCOPE_VAL_DC;
   ViReal64 verticalOffset = 0.0;
   ViReal64 probeAttenuation = 1.0;
   ViReal64 timeout = 5.000;
   ViInt32  numRecords = 1;
   ViBoolean enforceRealTime = VI_FALSE;
   ViReal64 refPosition = 50.0;
   ViReal64 triggerLevel = 0.0;
   ViInt32 triggerSlope = NISCOPE_VAL_POSITIVE;
   ViInt32 triggerCoupling = NISCOPE_VAL_DC;
   ViReal64 triggerHoldoff = 0.0;
   ViReal64 triggerDelay = 0.0;

   // Waveforms
   struct niScope_wfmInfo wfmInfo;
   ViReal64 *waveformPtr = NULL;
   struct niScope_wfmInfo freqWfmInfo;
   ViReal64 *freqWaveformPtr = NULL;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Add a waveform processing Hanning Window to get a frequency plot later
   handleErr (niScope_AddWaveformProcessing (vi, channel, NISCOPE_VAL_HANNING_WINDOW));

   stop = NISCOPE_VAL_FALSE;
   arraySize = 0;
   // Loop while the stop flat is not set
   while (!stop)
   {
      // Obtain the necessary parameters from the user interface
      GetParametersFromGUI (&verticalRange, &minSampleRate, &minRecordLength,
                       &triggerType);

      // Configure the acquisition type, in this case flexible resolution
      handleErr (niScope_ConfigureAcquisition (vi, NISCOPE_VAL_FLEXRES));

      // Configure the vertical parameters
      handleErr (niScope_ConfigureVertical (vi, channel, verticalRange, verticalOffset,
                                            verticalCoupling, probeAttenuation,
                                            NISCOPE_VAL_TRUE));

      // Configure the horizontal parameters
      handleErr (niScope_ConfigureHorizontalTiming (vi, minSampleRate, minRecordLength,
                                                    refPosition, numRecords, enforceRealTime));

      // This example only works with 1 waveform.  Check and make sure only
      // one waveform is requested; return an error otherwise.
      handleErr (niScope_ActualNumWfms (vi, channel, &numWaveform));
      if (numWaveform > 1)
         handleErr (-1);


      // Configure the trigger
      if (triggerType == 0) // edge trigger
      {
         handleErr (niScope_ConfigureTriggerEdge (vi, channel, triggerLevel,
                                                  triggerSlope, triggerCoupling,
                                                  triggerHoldoff, triggerDelay));
      }
      else
      {
         handleErr (niScope_ConfigureTriggerImmediate (vi));
      }

      // Start acquiring data
      handleErr (niScope_InitiateAcquisition (vi));

      handleErr (niScope_ActualRecordLength (vi, &actualRecordLength));

      // Allocate space for the waveform and waveform
      if (waveformPtr)
         free (waveformPtr);
      waveformPtr = (ViReal64*) malloc (sizeof (ViReal64) * actualRecordLength);

      // If it doesn't have enough memory, give an error message
      if (waveformPtr == NULL)
         handleErr (VI_ERROR_ALLOC);

      // Fetch the data
      handleErr (niScope_Fetch (vi, channel, timeout, actualRecordLength,
                                waveformPtr, &wfmInfo));

      // Find out the actual points that are in the measurement array by fetching with array size = 0
      handleErr (niScope_ActualMeasWfmSize (vi, NISCOPE_VAL_FFT_AMP_SPECTRUM_DB, &arraySize));

      // Allocate space for the array measurement waveform with the new array size
      if (freqWaveformPtr)
         free (freqWaveformPtr);
      freqWaveformPtr = (ViReal64*) malloc (sizeof (ViReal64) * arraySize * numWaveform);

      // If it doesn't have enough memory, give an error message
      if (freqWaveformPtr == NULL)
         handleErr (VI_ERROR_ALLOC);

      // Fetch the waveform measurement array
      handleErr (niScope_FetchArrayMeasurement (vi, channel, timeout,
                                                NISCOPE_VAL_FFT_AMP_SPECTRUM_DB, arraySize,
                                                freqWaveformPtr, &freqWfmInfo));

      handleErr (niScope_GetAttributeViReal64 (vi, VI_NULL, NISCOPE_ATTR_HORZ_SAMPLE_RATE, &actualSampleRate));
      handleErr (niScope_GetAttributeViInt32 (vi, VI_NULL, NISCOPE_ATTR_RESOLUTION, &resolution));

      // Plot the waveform and the measurement
      PlotWfms (waveformPtr, &wfmInfo, freqWaveformPtr, &freqWfmInfo, actualSampleRate, resolution);

      // Find out wether to stop or not
      ProcessEvent ((int*)&stop);
   }

Error :

   // Free all the allocated memory
   if (waveformPtr)
      free (waveformPtr);

   if (freqWaveformPtr)
      free (freqWaveformPtr);

   // Display messages
   if (error == -1)
      strcpy (errorMessage, "This examples only works with one channel.");
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
