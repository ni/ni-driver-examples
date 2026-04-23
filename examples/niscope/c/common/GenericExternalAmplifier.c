/********************************************************************************
 * External Amplifier generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericExternalAmplifier.h"
#include <stdio.h>
#include <string.h>


//////////////////////////////////////////////////////////////////////////
// niScope_GenericExternalAmplifier
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericExternalAmplifier (void)
{
   ViStatus error = VI_SUCCESS;
   ViChar   errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar   errorMessage[MAX_ERROR_DESCRIPTION] = " ";
   ViSession vi;

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar accessoryName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];
   ViChar triggerSource[MAX_STRING_SIZE];
   ViChar optionString[MAX_STRING_SIZE] = "DriverSetup=Accessory:";

   ViInt32 acquisitionType;
   ViReal64 verticalRange;
   ViReal64 verticalOffset;
   ViInt32 verticalCoupling;
   ViReal64 inputImpedance;
   ViReal64 maxInputFrequency;
   ViReal64 minSampleRate;
   ViInt32 minRecordLength;
   ViReal64 timeout;
   ViInt32 triggerCoupling;
   ViInt32 triggerSlope;
   ViReal64 triggerLevel;
   ViReal64 triggerHoldoff;
   ViReal64 triggerDelay;
   ViReal64 refPosition;


   // Default values used in this example
   ViInt32  numRecords;
   ViReal64 probeAttenuation = 1;
   ViInt32 stop = NISCOPE_VAL_FALSE;
   ViInt32 numWaveform;
   ViInt32 actualRecordLength;
   ViReal64 actualSampleRate;

   // Waveforms
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   ViReal64 *waveformPtr = NULL;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Obtain the accessory name of the device from the user interface and build full string
   GetAccessoryNameFromGUI (accessoryName);
   strcat(optionString, accessoryName);

   // Open the NI-SCOPE instrument handle using the option string to configure the accessory
   handleErr (niScope_InitWithOptions (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_TRUE, optionString, &vi));

   // Loop until the stop flag is set
   while (!stop)
   {
      // Obtain the necessary parameters from the user interface
      GetParametersFromGUI (channelName, &acquisitionType, &verticalRange, &verticalOffset,
                            &verticalCoupling, &inputImpedance, &maxInputFrequency,
                            &minSampleRate, &minRecordLength, &timeout, &numRecords,
                            &refPosition, triggerSource, &triggerCoupling, &triggerSlope,
                            &triggerLevel, &triggerHoldoff, &triggerDelay);

      // Configure the acquisition type
      handleErr (niScope_ConfigureAcquisitionType (vi, acquisitionType));

      // Configure the vertical parameters
      handleErr (niScope_ConfigureVertical (vi, channelName, verticalRange, verticalOffset,
                                           verticalCoupling, probeAttenuation, NISCOPE_VAL_TRUE));

      // Configure the channel characteristics
      handleErr (niScope_ConfigureChanCharacteristics (vi, channelName, inputImpedance,
                                                      maxInputFrequency));

      // Configure the horizontal parameters
      handleErr (niScope_ConfigureHorizontalTiming (vi, minSampleRate, minRecordLength,
                                                   refPosition, numRecords, NISCOPE_VAL_TRUE));

      // Configure the trigger
      handleErr (niScope_ConfigureTriggerEdge (vi, triggerSource, triggerLevel,
                                                  triggerSlope, triggerCoupling,
                                                  triggerHoldoff, triggerDelay));

      // Initiate the acquisition
      handleErr (niScope_InitiateAcquisition (vi));

      // Find out the current record length and number of waveforms
      handleErr (niScope_ActualNumWfms (vi, channelName, &numWaveform));

      handleErr (niScope_ActualRecordLength (vi, &actualRecordLength));

      // Allocate space for the waveform and waveform info according to the
      // record length and number of waveforms
      if (wfmInfoPtr)
         free (wfmInfoPtr);
      wfmInfoPtr = malloc (sizeof (struct niScope_wfmInfo) * numWaveform);

      if (waveformPtr)
         free (waveformPtr);
      waveformPtr = (ViReal64*) malloc (sizeof (ViReal64) * actualRecordLength * numWaveform);

      // If it doesn't have enough memory, give an error message
      if (waveformPtr == NULL || wfmInfoPtr == NULL)
         handleErr (VI_ERROR_ALLOC);

      // Fetch the data
      handleErr (niScope_Fetch (vi, channelName, timeout, actualRecordLength,
                                waveformPtr, wfmInfoPtr));

      // Get the actual sample rate
      handleErr (niScope_SampleRate (vi, &actualSampleRate));

      // Plot the waveform
      PlotWfms (numWaveform, waveformPtr, wfmInfoPtr, actualSampleRate, actualRecordLength);

      // Find out whether to stop or not
      ProcessEvent ((int*)&stop);
   }


Error :

   // Free all the allocated memory
   if (wfmInfoPtr)
      free (wfmInfoPtr);

   if (waveformPtr)
      free (waveformPtr);

   // Display messages
   if (error != VI_SUCCESS)
      niScope_errorHandler (vi, error, errorSource, errorMessage);
   else
      strcpy (errorMessage, "Acquisition successful!");

   DisplayErrorMessageInGUI (error, errorMessage);

   // Close the session
   if (vi)
      niScope_close (vi);

   return error;
}
