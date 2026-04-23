/********************************************************************************
 * Configured Acquisition generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericConfiguredAcquisition.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericConfiguredAcquisition
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericConfiguredAcquisition (void)
{
   ViStatus error = VI_SUCCESS;
   ViChar   errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar   errorMessage[MAX_ERROR_DESCRIPTION] = " ";
   ViSession vi;

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];
   ViChar triggerSource[MAX_STRING_SIZE];

   ViInt32 acquisitionType;
   ViReal64 verticalRange;
   ViReal64 verticalOffset;
   ViInt32 verticalCoupling;
   ViReal64 probeAttenuation;
   ViReal64 inputImpedance;
   ViReal64 maxInputFrequency;
   ViReal64 minSampleRate;
   ViInt32 minRecordLength;
   ViBoolean enforceRealTime;
   ViBoolean enableTIS;
   ViReal64 timeout;
   ViInt32 triggerType;
   ViInt32 triggerCoupling;
   ViInt32 triggerSlope;
   ViReal64 triggerLevel;
   ViReal64 triggerHoldoff;
   ViReal64 triggerDelay;
   ViInt32 windowMode;
   ViReal64 lowWindowLevel;
   ViReal64 highWindowLevel;
   ViReal64 hysteresis;
   ViReal64 refPosition;


   // Default values used in this example
   ViInt32  numRecords;
   ViInt32 stop = NISCOPE_VAL_FALSE;
   ViInt32 numWaveform;
   ViInt32 actualRecordLength;
   ViReal64 actualSampleRate;

   // Waveforms
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   ViReal64 *waveformPtr = NULL;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Loop until the stop flag is set
   while (!stop)
   {
      // Obtain the necessary parameters from the user interface
      GetParametersFromGUI (channelName, &acquisitionType, &verticalRange, &verticalOffset,
                            &verticalCoupling, &probeAttenuation, &inputImpedance, &maxInputFrequency,
                            &minSampleRate, &minRecordLength, &enforceRealTime, &enableTIS, &timeout, &numRecords,
                            &refPosition, &triggerType, triggerSource, &triggerCoupling, &triggerSlope,
                            &triggerLevel, &triggerHoldoff, &triggerDelay, &windowMode, &lowWindowLevel,
                            &highWindowLevel, &hysteresis);

      // Configure the acquisition type
      handleErr (niScope_ConfigureAcquisition (vi, acquisitionType));

      // Configure the vertical parameters
      handleErr (niScope_ConfigureVertical (vi, channelName, verticalRange, verticalOffset,
                                           verticalCoupling, probeAttenuation, NISCOPE_VAL_TRUE));

      // Configure the channel characteristics
      handleErr (niScope_ConfigureChanCharacteristics (vi, channelName, inputImpedance,
                                                      maxInputFrequency));

      // Configure the horizontal parameters
      handleErr (niScope_ConfigureHorizontalTiming (vi, minSampleRate, minRecordLength,
                                                   refPosition, numRecords, enforceRealTime));

      handleErr (niScope_SetAttributeViBoolean(vi, channelName, NISCOPE_ATTR_ENABLE_TIME_INTERLEAVED_SAMPLING, enableTIS));

      // Configure the trigger type
      switch (triggerType)
      {
      case 0: //Edge Trigger
         handleErr (niScope_ConfigureTriggerEdge (vi, triggerSource, triggerLevel,
                                                  triggerSlope, triggerCoupling,
                                                  triggerHoldoff, triggerDelay));
         break;

      case 1: //Hysteresis Trigger
         handleErr (niScope_ConfigureTriggerHysteresis (vi, triggerSource, triggerLevel,
                                                        hysteresis, triggerSlope,
                                                        triggerCoupling, triggerHoldoff,
                                                        triggerDelay));
         break;

      case 2: //Digital Trigger
         handleErr (niScope_ConfigureTriggerDigital (vi, triggerSource, triggerSlope,
                                                     triggerHoldoff, triggerDelay));
         break;

      case 3: //Window Trigger
         handleErr (niScope_ConfigureTriggerWindow (vi, triggerSource, lowWindowLevel,
                                                    highWindowLevel, windowMode,
                                                    triggerCoupling, triggerHoldoff,
                                                    triggerDelay));
         break;

      case 4: //Immediate Triggering
         handleErr (niScope_ConfigureTriggerImmediate (vi));
         break;

      default:
         //don't do anything
         break;
      }

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
