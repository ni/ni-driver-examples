/********************************************************************************
 * External Clocking generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericExternalClocking.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericExternalClocking
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericExternalClocking (void)
{
   ViStatus error = VI_SUCCESS;
   ViChar   errorMessage[MAX_ERROR_DESCRIPTION] = " ";
   ViChar   errorSource[MAX_FUNCTION_NAME_SIZE];
   ViSession vi;

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];
   ViReal64 verticalRange;
   ViReal64 verticalOffset;
   ViInt32 minRecordLength;
   ViChar timebaseSource[MAX_STRING_SIZE];
   ViReal64 timebaseRate;
   ViInt32 timebaseDivisor;
   ViInt32 timebaseMultiplier;
   ViInt32 numWaveform;
   ViInt32 actualRecordLength;
   ViReal64 actualSampleRate;

   // Default values used in this example
   ViReal64 refPosition = 50.0;
   ViReal64 timeout = 5.0; // seconds
   ViInt32 stop;
   ViInt32 i, j;

   // Waveforms
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   ViReal64 *scaledWfmPtr = NULL;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Obtain the necessary parameters from the user interface
   GetParametersFromGUI (channelName, &verticalRange, &verticalOffset,
   						 &minRecordLength, timebaseSource, &timebaseRate,
                         &timebaseDivisor, &timebaseMultiplier);

   // Configure the vertical parameters
   handleErr (niScope_ConfigureVertical (vi, channelName, verticalRange, verticalOffset,
                                         NISCOPE_VAL_DC, 1.0, NISCOPE_VAL_TRUE));

   // Configure the record length and reference position
   handleErr (niScope_SetAttributeViInt32 (vi, VI_NULL, NISCOPE_ATTR_HORZ_MIN_NUM_PTS, minRecordLength));
   handleErr (niScope_SetAttributeViReal64 (vi, VI_NULL, NISCOPE_ATTR_HORZ_RECORD_REF_POSITION, refPosition));

   // Configure immediate triggering
   handleErr (niScope_ConfigureTriggerImmediate (vi));

   // Configure the external clocking attributes
   handleErr (niScope_SetAttributeViString (vi, VI_NULL, NISCOPE_ATTR_SAMP_CLK_TIMEBASE_SRC, timebaseSource));
   handleErr (niScope_SetAttributeViReal64 (vi, VI_NULL, NISCOPE_ATTR_SAMP_CLK_TIMEBASE_RATE, timebaseRate));
   handleErr (niScope_SetAttributeViInt32 (vi, VI_NULL, NISCOPE_ATTR_SAMP_CLK_TIMEBASE_DIV, timebaseDivisor));
   handleErr (niScope_SetAttributeViInt32 (vi, VI_NULL, NISCOPE_ATTR_SAMP_CLK_TIMEBASE_MULT, timebaseMultiplier));

   // Find out the number of waveforms, based on # of channels and records
   handleErr (niScope_ActualNumWfms (vi, channelName, &numWaveform));

   // Query the coerced record length
   handleErr (niScope_ActualRecordLength (vi, &actualRecordLength));

   // Query the actual sample rate
   handleErr (niScope_SampleRate (vi, &actualSampleRate));

   // Allocate space for the waveform and waveform info
   // according to the record length and number of waveforms
   wfmInfoPtr = malloc (sizeof (struct niScope_wfmInfo) * numWaveform);
   scaledWfmPtr = malloc (sizeof (ViReal64) * actualRecordLength * numWaveform);

   // Check if memory allocations succeeded.
   if (scaledWfmPtr == NULL || wfmInfoPtr == NULL)
      handleErr (VI_ERROR_ALLOC);

   stop = NISCOPE_VAL_FALSE;

   // Loop until the stop flag is set.  This example loops around initiate acquisition
   // and fetch, using the same configuration for every acquisition.  This is the
   // most efficient method for acquiring multiple waveforms with the same configuration
   // parameters. (Although typically, you would scale the data at a later time.)
   while (!stop)
   {
      // Use the 8 bit fetching function
      handleErr (niScope_Read (vi, channelName, timeout, actualRecordLength,
                               scaledWfmPtr, wfmInfoPtr));

      // Plot the binary and the scaled waveforms
      PlotWfm (numWaveform, scaledWfmPtr, wfmInfoPtr, actualSampleRate);

      // Find out whether to stop or not
      ProcessEvent (&stop);
   }

Error :

   // Free all the allocated memory
   if (wfmInfoPtr)
      free (wfmInfoPtr);

   if (scaledWfmPtr)
      free (scaledWfmPtr);

   // Display messages
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
