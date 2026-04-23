/********************************************************************************
 * Time Stamps generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericTimestamps.h"
#include <math.h>

// Helper function to get the histogram of the timestamps, the mean time, the
// standard deviation, and the minimum and maximum values
ViStatus _histogram(ViReal64* triggerIntervals, ViInt32 numRecords, ViInt32 numBins,
                    ViReal64* mean, ViReal64* stdev, ViReal64* histogramXValues,
                    ViInt32* histogramYValues, ViReal64* min, ViReal64* max)
{
   ViStatus error = VI_SUCCESS;
   ViInt32 i,n,y;
   ViReal64 counterMean=0, counterStdev=0;
   ViReal64 dx;

   // We ignore the first time stamp because it is not a valid reference
   n = numRecords - 1;
   *min = *max = triggerIntervals[1];
   // Ignore timestamp 0
   for (i=1; i<numRecords; i++)
   {
      if (triggerIntervals[i] > *max)
         *max = triggerIntervals[i];
      if (triggerIntervals[i] < *min)
         *min = triggerIntervals[i];
      counterMean +=triggerIntervals[i];
   }
   // Calculate the mean -- sum{X[i]}/n
   *mean = counterMean/n;
   for (i=1; i<numRecords; i++)
   {
      counterStdev += pow(triggerIntervals[i] - *mean,2);
   }
   // Calculate the stdev -- sqrt(sum{ (X[i] - mean)^2 }/n )
   *stdev = sqrt(counterStdev/n);

   // Get the width of each bin
   dx = (*max - *min)/numBins;
   memset(histogramXValues,0,numBins*sizeof(ViReal64));
   memset(histogramYValues,0,numBins*sizeof(ViInt32));

   // Add the counts to each bin according to the timestamps
   for (i=1; i<numRecords; i++)
   {
      y = (ViInt32)((triggerIntervals[i] - *min) / dx);
      if (y==numBins)
         y--;
      histogramYValues[y]++;
   }

   // Form a corresponding x axis
   for (i=0; i<numBins; i++)
   {
      histogramXValues[i] = i*dx + *min;
   }

   return error;
}

//////////////////////////////////////////////////////////////////////////
// niScope_GenericTimestamps
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericTimestamps(void)
{
   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar errorMessage[MAX_ERROR_DESCRIPTION] = " ";

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];
   ViChar triggerSource[MAX_STRING_SIZE];
   ViReal64 minSampleRate;
   ViInt32 numRecords;
   ViInt32 numWaveform;
   ViInt32 triggerType;
   ViReal64 triggerHoldoff;
   ViReal64 mean, stdev, min, max;
   ViReal64 verticalRange;

   // Default values used in this example
   ViReal64 refPosition = 50.0;
   ViInt32 minRecordLength = 1;
   ViReal64 verticalOffset = 0.0;
   ViInt32  verticalCoupling = NISCOPE_VAL_DC;
   ViReal64 probeAttenuation = 1.0;
   ViBoolean enforceRealtime = NISCOPE_VAL_TRUE;
   ViReal64 timeout = 5.000;
   ViReal64 triggerLevel = 0.0;
   ViInt32 triggerSlope = NISCOPE_VAL_POSITIVE;
   ViInt32 triggerCoupling = NISCOPE_VAL_DC;
   ViReal64 triggerDelay = 0.0;
   ViInt32 recordNumber = 0;
   ViReal64 previousTriggerTime = 0;
   ViReal64 triggerTime = 0;
   ViInt32 numBins = 100;

   // Data storage
   struct niScope_wfmInfo wfmInfo;
   ViReal64* triggerIntervals = VI_NULL;
   ViReal64* histogramXValues = VI_NULL;
   ViInt32* histogramYValues = VI_NULL;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr(niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Obtain the necessary parameters from the user interface
   GetParametersFromGUI (channelName, &minSampleRate, &numRecords,
                         &triggerType, triggerSource, &triggerHoldoff, &verticalRange);

   // Configure the vertical parameters
   handleErr(niScope_ConfigureVertical(vi, channelName, verticalRange, verticalOffset,
                              verticalCoupling, probeAttenuation, NISCOPE_VAL_TRUE));

   // Configure the horizontal parameters, with the specified number of records and 1 point only
   handleErr(niScope_ConfigureHorizontalTiming(vi, minSampleRate, minRecordLength, refPosition, numRecords, enforceRealtime));

   // Find out the current number of waveforms
   handleErr(niScope_ActualNumWfms(vi, channelName, &numWaveform));
   if (numWaveform != numRecords)
      handleErr (-1);

   // Configure the trigger
   switch (triggerType)
   {
   case 0: // Edge trigger
      handleErr(niScope_ConfigureTriggerEdge (vi, triggerSource, triggerLevel,
                                              triggerSlope, triggerCoupling,
                                              triggerHoldoff, triggerDelay));
      break;
   case 1: // Digital trigger
      handleErr(niScope_ConfigureTriggerDigital (vi, triggerSource, triggerSlope,
                                                 triggerHoldoff, triggerDelay));
      break;
   }

   // Fetch only one record at a time
   handleErr(niScope_SetAttributeViInt32 (vi, VI_NULL,
                                          NISCOPE_ATTR_FETCH_NUM_RECORDS, 1));

   // Initiate the acquisition
   handleErr(niScope_InitiateAcquisition(vi));

   triggerIntervals = (ViReal64*) malloc(numRecords * sizeof(ViReal64));

   for (recordNumber=0; recordNumber < numRecords; recordNumber++)
   {
      // Fetch only one record at a time
      handleErr(niScope_SetAttributeViInt32 (vi, VI_NULL,
                                             NISCOPE_ATTR_FETCH_RECORD_NUMBER, recordNumber));

      // Fetch the timestamps, no data
      handleErr(niScope_Fetch (vi, channelName, timeout, 0, VI_NULL, &wfmInfo));

      triggerTime = wfmInfo.absoluteInitialX - wfmInfo.relativeInitialX;
      triggerIntervals[recordNumber] = triggerTime - previousTriggerTime;
      previousTriggerTime = triggerTime;
   }

   histogramXValues = (ViReal64*) malloc(numBins * sizeof(ViReal64));
   histogramYValues = (ViInt32*) malloc(numBins * sizeof(ViInt32));

   handleErr(_histogram(triggerIntervals, numRecords, numBins, &mean, &stdev,
                        histogramXValues, histogramYValues, &min, &max));

   // Plot the waveforms
   PlotWfms (histogramXValues, histogramYValues, numBins, mean, stdev, min, max);

Error :

   // Free memory
   if (triggerIntervals)
      free (triggerIntervals);

   if (histogramXValues)
      free (histogramXValues);

   if (histogramYValues)
      free (histogramYValues);

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

