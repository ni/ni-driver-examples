/********************************************************************************
 * Multi Record Fetch More Than Available Memory generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericMultiRecordFetchMoreThanAvailableMemory.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericMultiRecordFetchForever
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericMultiRecordFetchMoreThanAvailableMemory(void)
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
   ViBoolean allowRecords;
   ViInt32 record;
   ViInt32 actualRecordLength;
   ViInt32 numWaveform;
   ViInt32 recordsAcquired;

   // Default values used in this example
   int stop;
   ViReal64 verticalOffset = 0.0;
   ViInt32  verticalCoupling = NISCOPE_VAL_DC;
   ViReal64 probeAttenuation = 1.0;
   ViBoolean enforceRealtime = NISCOPE_VAL_TRUE;
   ViReal64 refPosition = 50.0;
   ViReal64 timeout = 5.000;
   ViReal64 triggerLevel = 0.0;
   ViReal64 triggerHoldoff = 0.0;
   ViReal64 triggerDelay = 0.0;
   ViInt32  triggerSlope = NISCOPE_VAL_POSITIVE;
   ViInt32 triggerCoupling = NISCOPE_VAL_DC;

   // Waveforms
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   ViReal64 *waveformPtr = NULL;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr(niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Obtain the necessary parameters from the user interface
   GetParametersFromGUI (channelName, &verticalRange, &minSampleRate, &minRecordLength,
                    &numRecords, &allowRecords);

   // Set the attribute to allow more records than memory
   handleErr (niScope_SetAttributeViBoolean(vi, VI_NULL, NISCOPE_ATTR_ALLOW_MORE_RECORDS_THAN_MEMORY, allowRecords));

   // Configure the vertical parameters
   handleErr(niScope_ConfigureVertical(vi, channelName, verticalRange, verticalOffset,
                              verticalCoupling, probeAttenuation, NISCOPE_VAL_TRUE));

   // Configure the horizontal parameters
   handleErr(niScope_ConfigureHorizontalTiming(vi, minSampleRate, minRecordLength, refPosition, numRecords, enforceRealtime));

   // Configure the trigger
   handleErr(niScope_ConfigureTriggerEdge (vi, channelName, triggerLevel,
                                           triggerSlope, triggerCoupling,
                                           triggerHoldoff, triggerDelay));

   // Initiate the acquisition
   handleErr(niScope_InitiateAcquisition(vi));

   stop = 0;
   record = 0;
   // Find out the current record length and number of waveforms
   handleErr(niScope_ActualNumWfms(vi, channelName, &numWaveform));

   handleErr(niScope_ActualRecordLength (vi, &actualRecordLength));

   // The real number of waveforms is the result of dividing by the number of records.
   numWaveform = numWaveform / numRecords;

   // Loop until the records are completed or the stop flag is set
   while (!stop && (record < numRecords))
   {
      // Allocate space for the waveform and waveform info according to the record length and number of waveforms
      wfmInfoPtr = malloc(sizeof(struct niScope_wfmInfo) * numWaveform);

      waveformPtr = (ViReal64*) malloc (sizeof(ViReal64) * actualRecordLength * numWaveform);

      // If it doesn't have enough memory, give an error message
      if (waveformPtr == NULL || wfmInfoPtr == NULL)
         handleErr (VI_ERROR_ALLOC);

      // Find out how many records have been acquired
      checkErr (niScope_GetAttributeViInt32 ( vi, VI_NULL, NISCOPE_ATTR_RECORDS_DONE, &recordsAcquired));

      // Fetch the next record
      checkErr (niScope_SetAttributeViInt32 ( vi, VI_NULL, NISCOPE_ATTR_FETCH_NUM_RECORDS, 1));
      checkErr (niScope_SetAttributeViInt32 ( vi, VI_NULL, NISCOPE_ATTR_FETCH_RECORD_NUMBER, record));
      handleErr(niScope_Fetch (vi, channelName, timeout, actualRecordLength, waveformPtr, wfmInfoPtr));

      // Increase the number of records
      record++;

      // Plot the record
      PlotWfms (record, recordsAcquired, numWaveform, waveformPtr, wfmInfoPtr);

      // Free the memory of the waveforms
      if (wfmInfoPtr)
      {
         free (wfmInfoPtr);
         wfmInfoPtr = NULL;
      }

      if (waveformPtr)
      {
         free (waveformPtr);
         waveformPtr = NULL;
      }

      // Find out wether to stop or not
      ProcessEvent((int*)&stop);
   }

Error :

   // Free all the allocated memory
   if (wfmInfoPtr)
      free (wfmInfoPtr);

   if (waveformPtr)
      free (waveformPtr);

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
