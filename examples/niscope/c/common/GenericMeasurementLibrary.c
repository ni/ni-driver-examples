/********************************************************************************
 *  Measurement Library generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericMeasurementLibrary.h"
#include <stdio.h>

// Arbitrarily limit the max number of waveforms to simplify
// memory allocation.
#define ARBITRARY_MAX_NUM_WFMS 2

//////////////////////////////////////////////////////////////////////////
// niScope_GenericMeasurementLibrary
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericMeasurementLibrary (void)
{
   ViStatus error = VI_SUCCESS;
   ViChar   errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar   errorMessage[MAX_ERROR_DESCRIPTION] = " ";
   ViSession vi;

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];
   ViReal64 verticalRange;
   ViInt32 verticalCoupling;
   ViReal64 minSampleRate;
   ViInt32 minRecordLength;
   ViInt32 refLevelUnits;
   ViReal64 chanBasedLowRef;
   ViReal64 chanBasedMidRef;
   ViReal64 chanBasedHighRef;
   ViInt32 scalarMeasurement;
   ViBoolean clearStatistics;
   ViInt32 triggerType;
   ViInt32 numWaveform;
   ViInt32 actualRecordLength;

   // Default values used in this example
   ViInt32 stop;
   ViReal64 verticalOffset = 0.0;
   ViReal64 probeAttenuation = 1.0;
   ViReal64 refPosition = 50;
   ViInt32 numRec =  1;
   ViConstString triggerSource = "0";
   ViReal64 triggerLevel = 0.0;
   ViReal64 triggerHoldoff = 0.0;
   ViReal64 triggerDelay = 0.0;
   ViInt32 triggerCoupling = NISCOPE_VAL_DC;
   ViInt32 triggerSlope = NISCOPE_VAL_POSITIVE;
   ViBoolean enforceRealTime = VI_TRUE;
   ViReal64 timeout = 5.0;

    // Waveforms
   struct niScope_wfmInfo wfmInfo[ARBITRARY_MAX_NUM_WFMS];
   ViReal64 *waveformPtr = NULL;

    // Variables for the measurements
   ViReal64 scalarResult[ARBITRARY_MAX_NUM_WFMS];
   ViReal64 mean[ARBITRARY_MAX_NUM_WFMS];
   ViReal64 stdev[ARBITRARY_MAX_NUM_WFMS];
   ViReal64 min[ARBITRARY_MAX_NUM_WFMS];
   ViReal64 max[ARBITRARY_MAX_NUM_WFMS];
   ViInt32 numInStats[ARBITRARY_MAX_NUM_WFMS];


   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   stop = NISCOPE_VAL_FALSE;
   // Loop while the stop flag is not set
   while (!stop)
   {
      // Obtain the necessary parameters from the user interface
      GetParametersFromGUI (channelName, &verticalRange, &verticalCoupling, &minSampleRate,
                            &minRecordLength, &refLevelUnits, &chanBasedLowRef,
                            &chanBasedMidRef, &chanBasedHighRef, &scalarMeasurement,
                            &clearStatistics, &triggerType);

      // Configure the type of the acquisition
      handleErr (niScope_ConfigureAcquisition (vi, NISCOPE_VAL_NORMAL));

      // Configure the vertical parameters
      handleErr (niScope_ConfigureVertical (vi, channelName, verticalRange, verticalOffset,
                                    verticalCoupling, probeAttenuation, NISCOPE_VAL_TRUE));

      // Configure the horizontal parameters
      handleErr (niScope_ConfigureHorizontalTiming (vi, minSampleRate, minRecordLength,
                                                    refPosition, numRec, enforceRealTime));

      // Configure the trigger
      if (triggerType == 1) // edge
      {
         handleErr (niScope_ConfigureTriggerEdge (vi,
                                                  triggerSource,
                                                  triggerLevel,
                                                  triggerSlope,
                                                  triggerCoupling,
                                                  triggerHoldoff,
                                                  triggerDelay));
      }
      else
         handleErr (niScope_ConfigureTriggerImmediate (vi));


      // Initiate the acquisition
      handleErr (niScope_InitiateAcquisition (vi));

      // Make sure we're not exceeding the arbitrary limit on number of waveforms
      handleErr (niScope_ActualNumWfms (vi, channelName, &numWaveform));
      if (numWaveform > ARBITRARY_MAX_NUM_WFMS)
         handleErr (-1);

      handleErr (niScope_ActualRecordLength (vi, &actualRecordLength));

      // Allocate space for the waveform according to the record length and number of waveforms
      if (waveformPtr)
         free (waveformPtr);
      waveformPtr = malloc (sizeof (ViReal64) * actualRecordLength * numWaveform);

      // If it doesn't have enough memory, give an error message
      if (waveformPtr == NULL)
         handleErr (VI_ERROR_ALLOC);

      // Fetch the data
      handleErr (niScope_Fetch (vi, channelName, timeout, actualRecordLength,
                                waveformPtr, wfmInfo));

      // Set the values for the attributes of reference units and the reference levels
      handleErr (niScope_SetAttributeViInt32 (vi, channelName,
                                            NISCOPE_ATTR_MEAS_REF_LEVEL_UNITS, refLevelUnits));
      handleErr (niScope_SetAttributeViReal64 (vi, channelName,
                                               NISCOPE_ATTR_MEAS_CHAN_LOW_REF_LEVEL,
                                               chanBasedLowRef));
      handleErr (niScope_SetAttributeViReal64 (vi, channelName,
                                               NISCOPE_ATTR_MEAS_CHAN_MID_REF_LEVEL,
                                               chanBasedMidRef));
      handleErr (niScope_SetAttributeViReal64 (vi, channelName,
                                               NISCOPE_ATTR_MEAS_CHAN_HIGH_REF_LEVEL,
                                               chanBasedHighRef));

      // Fetch the measurement
      handleErr (niScope_FetchMeasurementStats (vi, channelName, timeout, scalarMeasurement,
                                                scalarResult, mean, stdev, min,
                                                max, numInStats));

        // Clear the stats if necessary
      if (clearStatistics)
         handleErr (niScope_ClearWaveformMeasurementStats (vi, channelName, NISCOPE_VAL_ALL_MEASUREMENTS));

      // Plot the waveform
      PlotWfms (numWaveform, waveformPtr, wfmInfo, scalarResult, mean, stdev, min, max, numInStats);

      // Find out if we need to stop
      ProcessEvent (&stop);
   }


Error :

   if (waveformPtr)
      free (waveformPtr);

   // Display messages
   if (error == -1)
      sprintf (errorMessage, "This example is limited to %d waveforms. "
                             "It can be easily modified to increase this limit.",
                             ARBITRARY_MAX_NUM_WFMS);
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
