/********************************************************************************
 * Sample width generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericSampleWidth.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericSampleWidth
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericSampleWidth (void)
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
   ViInt32 numWaveform;
   ViInt32 actualRecordLength;
   ViInt32 binarySampleWidth;

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

   // Waveforms
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   void* binaryWfmPtr = NULL;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Obtain the necessary parameters from the user interface
   GetParametersFromGUI (channelName, &triggerType, &verticalRange,
                         &verticalOffset, &minSampleRate, &minRecordLength,
                         &binarySampleWidth);

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
   binaryWfmPtr = (void*) malloc ((binarySampleWidth/8) * actualRecordLength * numWaveform);

   // Check is memory allocations succeeded.
   if (wfmInfoPtr == NULL || binaryWfmPtr == NULL)
      handleErr (VI_ERROR_ALLOC);

   stop = NISCOPE_VAL_FALSE;

   // Loop until the stop flag is set.  This example loops around initiate acquisition
   // and fetch, using the same configuration for every acquisition.  This is the
   // most efficient method for acquiring multiple waveforms with the same configuration
   // parameters. (Although typically, you would scale the data at a later time.)
   while (!stop)
   {
      // Configure the width of the samples that will be stored in the onboard
      // memory of the device. This will save memory space and improve fetching
      // speed at the cost of losing resolution. Only some devices support setting
      // this width to a lower value than the native width.
      handleErr (niScope_SetAttributeViInt32 (vi, VI_NULL, NISCOPE_ATTR_BINARY_SAMPLE_WIDTH,
                                              binarySampleWidth));

      // Initiate the acquisition
      handleErr (niScope_InitiateAcquisition (vi));

      // Fetch the data according to the obtained data type
      switch (binarySampleWidth)
      {
         case 8:
            // Use the 8 bit fetching function
            handleErr (niScope_FetchBinary8 (vi, channelName, timeout, actualRecordLength,
                                             (ViInt8*) binaryWfmPtr, wfmInfoPtr));
            break;

         case 16:
            // Use the 16 bit fetching function
            handleErr (niScope_FetchBinary16 (vi, channelName, timeout, actualRecordLength,
                                              (ViInt16*) binaryWfmPtr, wfmInfoPtr));
            break;

         case 32:
            // Use the 32 bit fetching function
            handleErr (niScope_FetchBinary32 (vi, channelName, timeout, actualRecordLength,
                                              (ViInt32*) binaryWfmPtr, wfmInfoPtr));
            break;

         default:
            break;
      }

      // Plot the binary and the scaled waveforms
      PlotWfms (numWaveform, binarySampleWidth, binaryWfmPtr, wfmInfoPtr);

      // Find out whether to stop or not
      ProcessEvent ((int*)&stop);
   }

 Error :

   // Free all the allocated memory
   if (wfmInfoPtr)
      free (wfmInfoPtr);

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
