/********************************************************************************
 * Advanced Measurement Library generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericAdvancedMeasurementLibrary.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericAdvancedMeasurementLibrary
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericAdvancedMeasurementLibrary(void)
{
   ViStatus error = VI_SUCCESS;
   ViChar   errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar   errorMessage[MAX_ERROR_DESCRIPTION] = " ";
   ViSession vi;

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];
   ViInt32 processingStepMeasurement;
   ViInt32 scalarMeasurement;
   ViInt32 arrayMeasurement;
   ViInt32 filterType;
   ViReal64 cutoffFrequency;
   ViReal64 centerFrequency;
   ViReal64 bandpassWidth;
   ViInt32 numWaveform;
   ViInt32 actualRecordLength;
   ViInt32 arrayNumWaveform;
   ViInt32 arrayActualRecordLength;

   // Default values used in this example
   ViInt32 stop;
   ViReal64 maxTime = 5.000;
   ViInt32 numRec =  1;

   // Waveforms
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   ViReal64 *waveformPtr = NULL;

   // Variables for the measurements
   struct niScope_wfmInfo *arrayWfmInfoPtr = NULL;
   ViReal64 *arrayWaveformPtr = NULL;
   ViReal64 scalarResult[sizeof(ViReal64) * 2];

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI(resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr(niScope_init(resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Setup the device using default parameters
   handleErr(niScope_AutoSetup(vi));

   stop = NISCOPE_VAL_FALSE;
   // Loop while the stop flag is not set
   while (!stop)
   {
      // Obtain the necessary parameters from the user interface
      GetParametersFromGUI (channelName, &processingStepMeasurement, &scalarMeasurement,
                            &arrayMeasurement, &filterType, &cutoffFrequency,
                            &centerFrequency, &bandpassWidth);

      // Set the filter attributes
      handleErr (niScope_SetAttributeViInt32(vi, channelName, NISCOPE_ATTR_MEAS_FILTER_TYPE, filterType));
      handleErr (niScope_SetAttributeViReal64(vi, channelName, NISCOPE_ATTR_MEAS_FILTER_CUTOFF_FREQ, cutoffFrequency));
      handleErr (niScope_SetAttributeViReal64(vi, channelName, NISCOPE_ATTR_MEAS_FILTER_CENTER_FREQ, centerFrequency));
      handleErr (niScope_SetAttributeViReal64(vi, channelName, NISCOPE_ATTR_MEAS_FILTER_WIDTH, bandpassWidth));

      // Add a measurement step to the waveform
      handleErr (niScope_AddWaveformProcessing(vi, channelName, processingStepMeasurement));

      // Find out the current record length and number of waveforms
      handleErr(niScope_ActualNumWfms(vi, channelName, &numWaveform));

      handleErr(niScope_ActualRecordLength(vi, &actualRecordLength));

      // Allocate space for the waveform and waveform info according to the record length and number of waveforms
      if (wfmInfoPtr)
         free (wfmInfoPtr);
      wfmInfoPtr = malloc(sizeof(struct niScope_wfmInfo) * numWaveform);

      if (waveformPtr)
         free (waveformPtr);
      waveformPtr = malloc (sizeof(ViReal64) * actualRecordLength * numWaveform);

      // If it doesn't have enough memory, give an error message
      if (waveformPtr == NULL || wfmInfoPtr == NULL)
         handleErr (VI_ERROR_ALLOC);

      // Read the data (Initiate, wait and fetch)
      handleErr(niScope_Read(vi, channelName, maxTime, actualRecordLength,
                             waveformPtr, wfmInfoPtr));

      // Fetch the scalar measurement
      handleErr(niScope_FetchMeasurement(vi, channelName, maxTime, scalarMeasurement,
                                         scalarResult));

      // Find out the current record length for the array measurement
      handleErr(niScope_ActualMeasWfmSize(vi, arrayMeasurement, &arrayActualRecordLength));
      arrayNumWaveform = numWaveform;

      // Allocate space for the waveform and waveform info according to the record length and number of waveforms of the array measurement
      if (arrayWfmInfoPtr)
         free (arrayWfmInfoPtr);
      arrayWfmInfoPtr = malloc(sizeof(struct niScope_wfmInfo) * arrayNumWaveform);

      if (arrayWaveformPtr)
         free (arrayWaveformPtr);
      arrayWaveformPtr = malloc (sizeof(ViReal64) * arrayActualRecordLength * arrayNumWaveform);

      // If it doesn't have enough memory, give an error message
      if (arrayWaveformPtr == NULL || arrayWfmInfoPtr == NULL)
         handleErr (VI_ERROR_ALLOC);

      // Fetch the array measurement
      handleErr(niScope_FetchArrayMeasurement(vi, channelName, maxTime, arrayMeasurement,
                                              arrayActualRecordLength, arrayWaveformPtr, arrayWfmInfoPtr));

      // Remove the processing step measurement that we added before
      handleErr(niScope_ClearWaveformProcessing(vi, channelName));

      // Plot the waveform
      PlotWfms (numWaveform, arrayNumWaveform, waveformPtr, wfmInfoPtr,
                arrayWaveformPtr, arrayWfmInfoPtr, scalarResult);

      // Find out if we need to stop
      ProcessEvent (&stop);
   }


Error :

   // Free all the allocated memory
   if (wfmInfoPtr)
      free (wfmInfoPtr);

   if (waveformPtr)
      free (waveformPtr);

   if (arrayWfmInfoPtr)
      free (arrayWfmInfoPtr);

   if (arrayWaveformPtr)
      free (arrayWaveformPtr);

   // Display messages
   if (error != VI_SUCCESS)
      niScope_errorHandler (vi, error, errorSource, errorMessage);   // Intrepret the error
   else
      strcpy(errorMessage, "Acquisition successful!");

   DisplayErrorMessageInGUI (error, errorMessage);

   if (error < 0)
   {
     // Abort acquisition to allow NI SCOPE to initiate new acquisitions
     niScope_Abort (vi);
   }

   // Close the session
   if(vi)
     niScope_close(vi);

   return error;
}
