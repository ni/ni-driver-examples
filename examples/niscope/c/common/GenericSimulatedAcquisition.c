#include <stdio.h>
/********************************************************************************
 * Simulated Acquisition generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericSimulatedAcquisition.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericConfiguredAcquisition
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericSimulatedAcquisition (void)
{
   ViStatus error = VI_SUCCESS;
   ViChar   errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar   errorMessage[MAX_ERROR_DESCRIPTION] = " ";
   ViSession vi;

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];
   ViChar model[MAX_STRING_SIZE];
   ViChar type[MAX_STRING_SIZE];
   ViChar triggerSource[MAX_STRING_SIZE];
   ViChar tempStr[MAX_STRING_SIZE];
   ViChar optionString[MAX_STRING_SIZE*2]; // Twice as big since we concatenate various strings;

   ViReal64 verticalRange;
   ViInt32 verticalCoupling;
   ViReal64 probeAttenuation;
   ViReal64 verticalNoise;
   ViReal64 inputImpedance;
   ViReal64 maxInputFrequency;
   ViReal64 minSampleRate;
   ViInt32 minRecordLength;
   ViBoolean enforceRealTime;
   ViInt32 numRecords;
   ViReal64 refPos;
   ViInt32 triggerType;
   ViInt32 triggerSlope;
   ViReal64 triggerLevel;
   ViInt32 measurement;

   // Default values used in this example
   ViReal64 verticalOffset = 0.0;
   ViReal64 triggerHoldoff = 0.0;
   ViReal64 triggerDelay = 0.0;
   ViInt32 triggerCoupling = NISCOPE_VAL_DC;
   ViReal64 timeout = 5.0;
   ViInt32 stop = NISCOPE_VAL_FALSE;
   ViInt32 numWaveform;
   ViInt32 actualRecordLength;
   ViReal64 actualSampleRate;

   // Waveforms
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   ViReal64 *waveformPtr = NULL;
   ViReal64 *scalarResultPtr = NULL;
   ViReal64 *meanPtr = NULL;
   ViReal64 *stdevPtr = NULL;
   ViReal64 *minPtr = NULL;
   ViReal64 *maxPtr = NULL;
   ViInt32 *numInStatsPtr = NULL;

   // Obtain the resource name and simulation parameters from user interface
   GetResourceNameFromGUI (resourceName, model, type, &verticalNoise);

   // The option string must be in the form of: Simulate=1,DriverSetup=Noise:4.0;Model:5122;BoardType:PXI;
   strcpy (optionString, "Simulate=1,DriverSetup=Noise:");
   // Convert the noise to a string and append to what we had
   sprintf (tempStr, "%f", verticalNoise);
   strcat (optionString, tempStr);
   // Append model
   strcat (optionString, ";Model:");
   strcat (optionString, model);
   // Append type
   strcat (optionString, ";BoardType:");
   strcat (optionString, type);
   strcat (optionString, ";");

   // Open the NI-SCOPE instrument handle with the options for simulation
   handleErr (niScope_InitWithOptions (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE,
                                       optionString, &vi));

   // Loop until the stop flag is set
   while (!stop)
   {
      // Obtain the necessary parameters from the user interface
      GetParametersFromGUI (channelName, &verticalRange, &verticalCoupling,
                            &probeAttenuation, &inputImpedance, &maxInputFrequency,
                            &minSampleRate, &minRecordLength, &enforceRealTime, &numRecords, &refPos,
                            &triggerType, triggerSource, &triggerSlope, &triggerLevel, &measurement);

      // Configure the vertical parameters
      handleErr (niScope_ConfigureVertical (vi, channelName, verticalRange, verticalOffset,
                                            verticalCoupling, probeAttenuation, NISCOPE_VAL_TRUE));

      // Configure the channel characteristics
      handleErr (niScope_ConfigureChanCharacteristics (vi, channelName, inputImpedance,
                                                       maxInputFrequency));

      // Configure the horizontal parameters
      handleErr (niScope_ConfigureHorizontalTiming (vi, minSampleRate, minRecordLength,
                                                    refPos, numRecords, enforceRealTime));

      // Configure the trigger type
      switch (triggerType)
      {
      case 0: //Edge Trigger
         handleErr (niScope_ConfigureTriggerEdge (vi, triggerSource, triggerLevel,
                                                  triggerSlope, triggerCoupling,
                                                  triggerHoldoff, triggerDelay));
         break;

      case 1: //Digital Trigger
         handleErr (niScope_ConfigureTriggerDigital (vi, triggerSource, triggerSlope,
                                                     triggerHoldoff, triggerDelay));
         break;

      case 2: //Immediate Triggering
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

      // Allocate space for the waveform and measurements according to the
      // record length and number of waveforms
      if (wfmInfoPtr)
         free (wfmInfoPtr);
      wfmInfoPtr = (struct niScope_wfmInfo*) malloc (sizeof (struct niScope_wfmInfo) * numWaveform);
      if (waveformPtr)
         free (waveformPtr);
      waveformPtr = (ViReal64*) malloc (sizeof (ViReal64) * actualRecordLength * numWaveform);
      if (scalarResultPtr)
         free (scalarResultPtr);
      scalarResultPtr = (ViReal64*) malloc (sizeof (ViReal64) * numWaveform);
      if (meanPtr)
         free (meanPtr);
      meanPtr = (ViReal64*) malloc (sizeof (ViReal64) * numWaveform);
      if (stdevPtr)
         free (stdevPtr);
      stdevPtr = (ViReal64*) malloc (sizeof (ViReal64) * numWaveform);
      if (minPtr)
         free (minPtr);
      minPtr = (ViReal64*) malloc (sizeof (ViReal64) * numWaveform);
      if (maxPtr)
         free (maxPtr);
      maxPtr = (ViReal64*) malloc (sizeof (ViReal64) * numWaveform);
      if (numInStatsPtr)
         free (numInStatsPtr);
      numInStatsPtr = (ViInt32*) malloc (sizeof (ViInt32) * numWaveform);

      // If it doesn't have enough memory, give an error message
      if (waveformPtr == NULL || wfmInfoPtr == NULL || scalarResultPtr == NULL ||
          meanPtr == NULL || stdevPtr == NULL || minPtr == NULL || maxPtr == NULL ||
          numInStatsPtr == NULL)
         handleErr (VI_ERROR_ALLOC);

      // Fetch the data
      handleErr (niScope_Fetch (vi, channelName, timeout, actualRecordLength,
                                waveformPtr, wfmInfoPtr));

      // Fetch the measurement
      handleErr (niScope_FetchMeasurementStats (vi, channelName, timeout, measurement,
                                                scalarResultPtr, meanPtr, stdevPtr, minPtr,
                                                maxPtr, numInStatsPtr));

      // Get the actual sample rate
      handleErr (niScope_SampleRate (vi, &actualSampleRate));

      // Plot the waveform
      PlotWfms (numWaveform, waveformPtr, wfmInfoPtr, actualSampleRate, actualRecordLength,
                scalarResultPtr, meanPtr, stdevPtr, minPtr, maxPtr, numInStatsPtr);

      // Find out wether to stop or not
      ProcessEvent ((int*)&stop);
   }


Error :

   // Free all the allocated memory
   if (wfmInfoPtr)
      free (wfmInfoPtr);
   if (waveformPtr)
      free (waveformPtr);
   if (scalarResultPtr)
      free (scalarResultPtr);
   if (meanPtr)
      free (meanPtr);
   if (stdevPtr)
      free (stdevPtr);
   if (minPtr)
      free (minPtr);
   if (maxPtr)
      free (maxPtr);
   if (numInStatsPtr)
      free (numInStatsPtr);

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
