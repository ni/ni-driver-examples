#include "niTClk.h"

/********************************************************************************
 * Configured Acquisition generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericMultiDeviceConfiguredAcquisitionTClk.h"

ViStatus error = VI_SUCCESS;
ViChar errorMessage[MAX_ERROR_DESCRIPTION];
ViChar errorSource[MAX_FUNCTION_NAME_SIZE];

// Helper function that
// - gets comma-separated list of resource names from GUI,
// - parses the list
// - initializes resources and
// - reports initialized sessions and the number of sessions.
ViStatus _initSessions(ViUInt32* numberOfSessions,
                       ViSession sessions[])
{
   resourceNameType commaSeparatedResourceNames;
   ViChar * currentResource = commaSeparatedResourceNames;
   ViChar * currentResourceTrimmed = currentResource;
   unsigned int i = 0;
   unsigned int j = 0;
   unsigned int numWhitespace=0;
   // Obtain the comma-separated resource names from the user interface
   GetResourceNamesFromGUI(commaSeparatedResourceNames);

   // Set up strtok to iterate on the comma separated list
   // NOTE: if you care about thread-safety, try strsep instead of strtok
   currentResource = strtok(commaSeparatedResourceNames,",");

   // Now parse resource names into an array and establish the number of resources
   while( currentResource != NULL && i < MAX_NUM_DEVICES)
   {
      j=0;
      numWhitespace=0;
      // Removes whitespace from the identifier
      while(currentResource[j])
      {
         if(currentResource[j]!=' ')
            currentResourceTrimmed[j-numWhitespace]=currentResource[j];
         else numWhitespace++;
         j++;
      }
      currentResourceTrimmed[j-numWhitespace]= '\0';
      // Initialize the digitizer
      handleNIScopeErr(niScope_init(currentResourceTrimmed, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &sessions[i]));

      // communicate where the next token starts to the next iteration
      currentResource = strtok(NULL,",");

      // By the loop invariant, we have an additional session
      i++;
   }


 Error:
   // even if an error occurs, return the number of opened sessions so that
   // they can be closed in the caller
   DisplayErrorMessageInGUI (error, errorMessage);
   (*numberOfSessions) = i;
   return error;
}

ViStatus _fetchAndPlotData(ViSession vi, ViChar* channelName, ViReal64 timeout)
{
   // Waveforms
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   ViReal64 *waveformPtr = NULL;
   ViInt32 actualRecordLength;
   ViReal64 actualSampleRate;
   ViInt32 numWaveform;

   // Find out the current record length and number of waveforms
   checkErr (niScope_ActualNumWfms (vi, channelName, &numWaveform));

   checkErr (niScope_ActualRecordLength (vi, &actualRecordLength));

   // Allocate space for the waveform and waveform info according to the
   // record length and number of waveforms
   wfmInfoPtr = malloc (sizeof (struct niScope_wfmInfo) * numWaveform);
   waveformPtr = (ViReal64*)malloc (sizeof (ViReal64) * actualRecordLength * numWaveform);

   // If it doesn't have enough memory, give an error message
   if (waveformPtr == NULL || wfmInfoPtr == NULL)
      checkErr (VI_ERROR_ALLOC);

   // Get the actual sample rate
   checkErr (niScope_SampleRate (vi, &actualSampleRate));

   // Fetch the data
   checkErr (niScope_Fetch (vi, channelName, timeout, actualRecordLength,
                            waveformPtr, wfmInfoPtr));

   // Plot the waveform
   PlotWfms (numWaveform, waveformPtr, wfmInfoPtr, actualSampleRate, actualRecordLength);

 Error:
   // Error display is handled by caller
   if (wfmInfoPtr)
      free (wfmInfoPtr);

   if (waveformPtr)
      free (waveformPtr);
   return error;
}

//////////////////////////////////////////////////////////////////////////
// niScope_GenericMultiDeviceConfiguredAcquisitionTClk
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericMultiDeviceConfiguredAcquisitionTClk (void)
{
   ViSession vi;
   ViUInt32 i = 0;

   // Variables used to get values from the GUI
   ViChar channelName[MAX_STRING_SIZE];
   ViChar triggerSource[MAX_STRING_SIZE];

   ViInt32 acquisitionType;
   ViReal64 verticalRange;
   ViReal64 verticalOffset;
   ViInt32 verticalCoupling;
   ViReal64 probeAttenuation;
   ViReal64 inputImpedance;
   ViReal64 maxInputFrequency, previousMaxInputFrequency = -10;
   ViReal64 minSampleRate, previousMinSampleRate = -10;
   ViInt32 minRecordLength, previousMinRecordLength = -10;
   ViReal64 timeout;
   ViInt32 triggerType, previousTriggerType = -10;
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

   ViUInt32 numberOfSessions=0;
   ViSession sessions[MAX_NUM_DEVICES] = { VI_NULL }; // Session for each device



   // Default values used in this example
   ViInt32  numRecords;
   ViInt32 stop = NISCOPE_VAL_FALSE;

   // Clear our error messages
   error = VI_SUCCESS;
   strcpy(errorMessage,"");
   strcpy(errorSource,"");

   checkErr(_initSessions(&numberOfSessions,sessions));

   // Loop until the stop flag is set
   while (!stop)
   {
      // Obtain the necessary parameters from the user interface
      GetParametersFromGUI (channelName, &acquisitionType, &verticalRange, &verticalOffset,
                            &verticalCoupling, &probeAttenuation, &inputImpedance, &maxInputFrequency,
                            &minSampleRate, &minRecordLength, &timeout, &numRecords,
                            &refPosition, &triggerType, triggerSource, &triggerCoupling, &triggerSlope,
                            &triggerLevel, &triggerHoldoff, &triggerDelay, &windowMode, &lowWindowLevel,
                            &highWindowLevel, &hysteresis);

      // Configure the common device parameters
      for(i=0; i<numberOfSessions; i++)
      {
         vi = sessions[i];
         // Configure the acquisition type
         handleNIScopeErr (niScope_ConfigureAcquisitionType (vi, acquisitionType));

         // Configure the vertical parameters
         handleNIScopeErr (niScope_ConfigureVertical (vi, channelName, verticalRange, verticalOffset,
                                                      verticalCoupling, probeAttenuation, NISCOPE_VAL_TRUE));

         // Configure the channel characteristics
         handleNIScopeErr (niScope_ConfigureChanCharacteristics (vi, channelName, inputImpedance,
                                                                 maxInputFrequency));

         // Configure the horizontal parameters
         handleNIScopeErr (niScope_ConfigureHorizontalTiming (vi, minSampleRate, minRecordLength,
                                                              refPosition, numRecords, VI_TRUE));
      }

      vi = sessions[0];
      // Configure the trigger type for the master only
      switch (triggerType)
      {
         case 0: //Edge Trigger
            handleNIScopeErr (niScope_ConfigureTriggerEdge (vi, triggerSource, triggerLevel,
                                                            triggerSlope, triggerCoupling,
                                                            triggerHoldoff, triggerDelay));
            break;

         case 1: //Hysteresis Trigger
            handleNIScopeErr (niScope_ConfigureTriggerHysteresis (vi, triggerSource, triggerLevel,
                                                                  hysteresis, triggerSlope,
                                                                  triggerCoupling, triggerHoldoff,
                                                                  triggerDelay));
            break;

         case 2: //Digital Trigger
            handleNIScopeErr (niScope_ConfigureTriggerDigital (vi, triggerSource, triggerSlope,
                                                               triggerHoldoff, triggerDelay));
            break;

         case 3: //Window Trigger
            handleNIScopeErr (niScope_ConfigureTriggerWindow (vi, triggerSource, lowWindowLevel,
                                                              highWindowLevel, windowMode,
                                                              triggerCoupling, triggerHoldoff,
                                                              triggerDelay));
            break;

         case 4: //Immediate Triggering
            handleNIScopeErr (niScope_ConfigureTriggerImmediate (vi));
            break;

         default:
            //don't do anything
            break;
      }

      // Resync the devices if any sync related properties change
      // Note: the NI 5922 digitizer requires resynchronization after changing any
      // channel parameters.
      if(    previousTriggerType != triggerType
             || previousMinSampleRate != minSampleRate
             || previousMaxInputFrequency != maxInputFrequency
             || previousMinRecordLength != minRecordLength )
      {
         // Use NI-TClk to configure appropriate parameters, synchronize digitizers, and initiate operation.
         handleNITClkErr(niTClk_ConfigureForHomogeneousTriggers(numberOfSessions, sessions));
         handleNITClkErr(niTClk_Synchronize(numberOfSessions, sessions, 0.0));
      }
      previousTriggerType = triggerType;
      previousMinSampleRate = minSampleRate;
      previousMaxInputFrequency = maxInputFrequency;
      previousMinRecordLength = minRecordLength;

      handleNITClkErr(niTClk_Initiate(numberOfSessions, sessions));

      ClearPlots();
      for( i=0; i<numberOfSessions; i++ )
      {
         handleNIScopeErr(_fetchAndPlotData(sessions[i], channelName, timeout));
      }
      CommitPlots();

      // Find out whether to stop or not
      ProcessEvent ((int*)&stop);
   }


 Error :
   // Even on error, redraw the graph
   CommitPlots();
   // Display messages
   if (error == VI_SUCCESS)
      strcpy (errorMessage, "Acquisition successful!");

   DisplayErrorMessageInGUI (error, errorMessage);

   // Close the sessions
   for(i=0; i<numberOfSessions; i++)
      niScope_close (sessions[i]);

   return error;
}
