/********************************************************************************
 * Differential Acquisition generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericDifferential.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericBinaryAcquisition
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericDifferential (void)
{
   ViStatus error = VI_SUCCESS;
   ViChar   errorMessage[MAX_ERROR_DESCRIPTION] = " ";
   ViChar   errorSource[MAX_FUNCTION_NAME_SIZE];
   ViSession vi;

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViInt32 terminalConfiguration;
   ViInt32  triggerType;
   ViReal64 verticalRange;
   ViReal64 probeAttenuation;
   ViInt32 verticalCoupling;
   ViReal64 minSampleRate;
   ViInt32 minRecordLength;
   ViInt32 actualRecordLength;
   ViInt32 triggerSlope;
   ViReal64 triggerLevel;
   ViReal64 inputImpedance;
   ViInt32 numWaveform;

   // Default values used in this example
   ViReal64 verticalOffset = 0.0;
   ViInt32 triggerCoupling = NISCOPE_VAL_DC;
   ViReal64 triggerHoldoff = 0.0;
   ViReal64 triggerDelay = 0.0;
   ViConstString triggerSource = "0";
   ViReal64 refPosition = 50.0;
   ViReal64 timeout = 5.0; // seconds
   ViInt32 stop;

   // Waveforms
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   ViReal64 *scaledWfmPtr = NULL;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   stop = NISCOPE_VAL_FALSE;

   // Loop until the stop flag is set.  This example loops around initiate acquisition
   // and fetch, using the same configuration for every acquisition.  This is the
   // most efficient method for acquiring multiple waveforms with the same configuration
   // parameters. (Although typically, you would scale the data at a later time.)
   while (!stop)
   {
      // Obtain the necessary parameters from the user interface
      GetParametersFromGUI (&terminalConfiguration, &verticalRange, &probeAttenuation,
                            &verticalCoupling, &inputImpedance, &minSampleRate,
                            &minRecordLength, &triggerType, &triggerLevel, &triggerSlope);

      // Set the terminal configuration property as configured
      handleErr (niScope_SetAttributeViInt32 (vi, "0", NISCOPE_ATTR_CHANNEL_TERMINAL_CONFIGURATION,
                                              terminalConfiguration));

      // Configure the vertical parameters
      handleErr (niScope_ConfigureVertical (vi, "0", verticalRange, verticalOffset,
                                            verticalCoupling, probeAttenuation,
                                            NISCOPE_VAL_TRUE));

      // Configure the impedance
      handleErr (niScope_ConfigureChanCharacteristics (vi, "0", inputImpedance, 0.0));

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
      handleErr (niScope_ActualNumWfms (vi, "0", &numWaveform));

      // Query the coerced record length
      handleErr (niScope_ActualRecordLength (vi, &actualRecordLength));

      // Allocate space for the waveform ,waveform info, and binary waveform
      // according to the record length and number of waveforms
      if (wfmInfoPtr)
         free (wfmInfoPtr);
      wfmInfoPtr = malloc (sizeof (struct niScope_wfmInfo) * numWaveform);

      if (scaledWfmPtr)
         free (scaledWfmPtr);
      scaledWfmPtr = (ViReal64*) malloc (sizeof (ViReal64) * actualRecordLength * numWaveform);

      // Check is memory allocations succeeded.
      if (scaledWfmPtr == NULL || wfmInfoPtr == NULL)
         handleErr (VI_ERROR_ALLOC);

      // Initiate the acquisition
      handleErr (niScope_Read (vi, "0", timeout, actualRecordLength, scaledWfmPtr, wfmInfoPtr));

      // Plot the binary and the scaled waveforms
      PlotWfm (numWaveform, scaledWfmPtr, wfmInfoPtr);

      // Find out whether to stop or not
      ProcessEvent ((int*)&stop);
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
