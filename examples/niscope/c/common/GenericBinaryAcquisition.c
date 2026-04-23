/********************************************************************************
 * Binary Acquisition generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericBinaryAcquisition.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericBinaryAcquisition
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericBinaryAcquisition (void)
{
   ViStatus error = VI_SUCCESS;
   ViChar   errorMessage[MAX_ERROR_DESCRIPTION] = " ";
   ViChar   errorSource[MAX_FUNCTION_NAME_SIZE];
   ViSession vi;

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];
   ViInt32  triggerType;
   ViReal64 verticalRange;
   ViReal64 verticalOffset;
   ViReal64 minSampleRate;
   ViInt32 minRecordLength;
   ViInt32 binaryDataType;
   ViInt32 numWaveform;
   ViInt32 actualRecordLength;

   // Default values used in this example
   ViInt32  triggerCoupling = NISCOPE_VAL_DC;
   ViInt32  triggerSlope = NISCOPE_VAL_POSITIVE;
   ViReal64 triggerLevel = 0.0;
   ViReal64 triggerHoldoff = 0.0;
   ViReal64 triggerDelay = 0.0;
   ViConstString triggerSource = "0";
   ViReal64 refPosition = 50.0;
   ViReal64 timeout = 5.0; // seconds
   ViInt32 stop;
   ViInt32 i, j;

   // Waveforms
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   ViReal64 *scaledWfmPtr = NULL;
   void* binaryWfmPtr = NULL;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Obtain the necessary parameters from the user interface
   GetParametersFromGUI (channelName, &triggerType, &verticalRange,
                         &verticalOffset, &minSampleRate, &minRecordLength,
                         &binaryDataType);

   // Configure the vertical parameters
   handleErr (niScope_ConfigureVertical (vi, channelName, verticalRange, verticalOffset,
                                         NISCOPE_VAL_DC, 1.0, NISCOPE_VAL_TRUE));

   // Configure the horizontal parameters
   handleErr (niScope_ConfigureHorizontalTiming (vi, minSampleRate, minRecordLength, refPosition,
                                                 1, VI_TRUE));

   // Configure the trigger
   if (triggerType == 0) // edge
   {
      handleErr (niScope_ConfigureTriggerEdge (vi, triggerSource, triggerLevel,
                                               triggerSlope, triggerCoupling,
                                               triggerHoldoff, triggerDelay));
   }
   else
      handleErr (niScope_ConfigureTriggerImmediate (vi));

   // Find out the number of waveforms, based on # of channels and records
   handleErr (niScope_ActualNumWfms (vi, channelName, &numWaveform));

   // Query the coerced record length
   handleErr (niScope_ActualRecordLength (vi, &actualRecordLength));

   // Allocate space for the waveform ,waveform info, and binary waveform
   // according to the record length and number of waveforms
   wfmInfoPtr = malloc (sizeof (struct niScope_wfmInfo) * numWaveform);
   binaryWfmPtr = (void*) malloc (binaryDataType * actualRecordLength * numWaveform);
   scaledWfmPtr = (ViReal64*) malloc (sizeof (ViReal64) * actualRecordLength * numWaveform);

   // Check is memory allocations succeeded.
   if (scaledWfmPtr == NULL || wfmInfoPtr == NULL || binaryWfmPtr == NULL)
      handleErr (VI_ERROR_ALLOC);

   stop = NISCOPE_VAL_FALSE;

   // Loop until the stop flag is set.  This example loops around initiate acquisition
   // and fetch, using the same configuration for every acquisition.  This is the
   // most efficient method for acquiring multiple waveforms with the same configuration
   // parameters. (Although typically, you would scale the data at a later time.)
   while (!stop)
   {
      // Initiate the acquisition
      handleErr (niScope_InitiateAcquisition (vi));

      // Fetch the data according to the obtained data type
      switch (binaryDataType)
      {
      case 1:
         // Use the 8 bit fetching function
         handleErr (niScope_FetchBinary8 (vi, channelName, timeout, actualRecordLength,
                                          (ViInt8*) binaryWfmPtr, wfmInfoPtr));

         // Scaled waveform = ( 8 bit binary wavefrom * gain factor) + vertical offset
         for (j=0; j<numWaveform; j++)
            for (i=0; i<actualRecordLength; i++)
               scaledWfmPtr[i + j*actualRecordLength] = wfmInfoPtr[j].gain *
               ( (ViInt8*)binaryWfmPtr)[i+j*actualRecordLength] + wfmInfoPtr[j].offset;

         break;
      case 2:
         // Use the 16 bit fetching function
         handleErr (niScope_FetchBinary16 (vi, channelName, timeout, actualRecordLength,
                                           (ViInt16*) binaryWfmPtr, wfmInfoPtr));

         // Scaled waveform = ( 16 bit binary wavefrom * gain factor) + vertical offset
         for (j=0; j<numWaveform; j++)
            for (i=0; i<actualRecordLength; i++)
               scaledWfmPtr[i + j*actualRecordLength] = wfmInfoPtr[j].gain *
               ( (ViInt16*)binaryWfmPtr)[i + j*actualRecordLength] + wfmInfoPtr[j].offset;
         break;

      case 4:
         // Use the 32 bit fetching function
         handleErr (niScope_FetchBinary32 (vi, channelName, timeout, actualRecordLength,
                                           (ViInt32*) binaryWfmPtr, wfmInfoPtr));

         // Scaled waveform = ( 32 bit binary wavefrom * gain factor) + vertical offset
         for (j=0; j<numWaveform; j++)
            for (i=0; i<actualRecordLength; i++)
               scaledWfmPtr[i + j*actualRecordLength] = wfmInfoPtr[j].gain *
               ( (ViInt32*)binaryWfmPtr)[i + j*actualRecordLength] + wfmInfoPtr[j].offset;
         break;

      default:
         break;
      }

      // Plot the binary and the scaled waveforms
      PlotBinaryAndScaledWfms (numWaveform, binaryDataType, binaryWfmPtr,
                               scaledWfmPtr, wfmInfoPtr);

      // Find out whether to stop or not
      ProcessEvent ((int*)&stop);
   }

Error :

   // Free all the allocated memory
   if (wfmInfoPtr)
      free (wfmInfoPtr);

   if (scaledWfmPtr)
      free (scaledWfmPtr);

   if (binaryWfmPtr)
      free (binaryWfmPtr);

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
