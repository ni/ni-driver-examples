/********************************************************************************
 * DDC Advanced Low Level generic file
 *******************************************************************************/

#include "GenericOSPQuadratureDownconversion.h"


//////////////////////////////////////////////////////////////////////////
// niScope_GenericOSPQuadratureDownconversion
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericOSPQuadratureDownconversion (void)
{
   ViStatus error = VI_SUCCESS;
   ViChar   errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar   errorMessage[MAX_ERROR_DESCRIPTION] = " ";
   ViSession vi;

   ViChar resourceName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];

   // Variables used to get values from the GUI
   ViReal64 verticalRange;
   ViReal64 minIQRate;
   ViInt32 minRecordLength;
   ViReal64 digitalGain;
   ViReal64 timeout;
   ViReal64 centerFrequency;
   ViReal64 phaseI;
   ViReal64 phaseQ;
   ViInt32 triggerType;
   ViReal64 triggerLevel;
   ViReal64 triggerMinQuietTime;

   // Other variables
   ViInt32 numWaveform;
   ViInt32 actualRecordLength;
   ViReal64 actualSampleRate;
   ViBoolean enableFracResample;

   // Default values used in this example
   ViInt32 stop = NISCOPE_VAL_FALSE;
   ViInt32 inputImpedance = (ViInt32)NISCOPE_VAL_50_OHMS;
   ViReal64 refPosition = 50.0;
   ViInt32 numRecords = 1;
   ViInt32  triggerCoupling = NISCOPE_VAL_DC;
   ViInt32  triggerSlope = NISCOPE_VAL_POSITIVE;
   ViReal64 triggerHoldoff = 0.0;
   ViReal64 triggerDelay = 0.0;
   ViConstString triggerSource = "0";

   // Waveforms
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   ViReal64 *waveformPtr = NULL;


   strcpy(channelName, "0");

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));


   // Loop until the stop flag is set
   while (!stop)
   {
      // Obtain the necessary parameters from the user interface
      GetParametersFromGUI (channelName, &verticalRange, &inputImpedance, &minIQRate,
                            &minRecordLength, &timeout, &digitalGain, &centerFrequency,
                            &phaseI, &phaseQ, &triggerType, &triggerLevel, &triggerMinQuietTime,
                            &enableFracResample);

      // Configure the vertical parameters
      handleErr (niScope_SetAttributeViReal64 (vi, channelName, NISCOPE_ATTR_VERTICAL_RANGE, verticalRange));
      handleErr (niScope_SetAttributeViBoolean (vi, channelName, NISCOPE_ATTR_CHANNEL_ENABLED, NISCOPE_VAL_TRUE));

      // Enable/Disable HW fractional resample
      //if ( enableFracResample )
      handleErr (niScope_SetAttributeViBoolean (vi, VI_NULL, NISCOPE_ATTR_FRACTIONAL_RESAMPLE_ENABLED,
                                                enableFracResample));
      //if ( !enableFracResample )
      //        handleErr (niScope_SetAttributeViBoolean (vi, VI_NULL, NISCOPE_ATTR_FRACTIONAL_RESAMPLE_ENABLED,
      //        NISCOPE_VAL_FALSE));

      // Configure the channel characteristics
      handleErr (niScope_ConfigureChanCharacteristics (vi, channelName, inputImpedance,
                                                       NISCOPE_VAL_BANDWIDTH_FULL));

      // Configure the horizontal parameters
      handleErr (niScope_ConfigureHorizontalTiming (vi, minIQRate, minRecordLength, refPosition,
                                                    numRecords, NISCOPE_VAL_TRUE));

      // Configure DDC parameters
      handleErr (niScope_SetAttributeViBoolean (vi, channelName, NISCOPE_ATTR_DDC_ENABLED,
                                                NISCOPE_VAL_TRUE));
      handleErr (niScope_SetAttributeViBoolean (vi, channelName, NISCOPE_ATTR_DDC_FREQUENCY_TRANSLATION_ENABLED,
                                                NISCOPE_VAL_TRUE));
      handleErr (niScope_SetAttributeViReal64 (vi, channelName, NISCOPE_ATTR_DDC_CENTER_FREQUENCY,
                                               centerFrequency));
      handleErr (niScope_SetAttributeViReal64 (vi, channelName, NISCOPE_ATTR_DDC_FREQUENCY_TRANSLATION_PHASE_I,
                                               phaseI));
      handleErr (niScope_SetAttributeViReal64 (vi, channelName, NISCOPE_ATTR_DDC_FREQUENCY_TRANSLATION_PHASE_Q,
                                               phaseQ));

      handleErr (niScope_SetAttributeViInt32 (vi, VI_NULL, NISCOPE_ATTR_DDC_DATA_PROCESSING_MODE,
                                              NISCOPE_VAL_COMPLEX));

      handleErr(niScope_SetAttributeViInt32 (vi, VI_NULL, NISCOPE_ATTR_OVERFLOW_ERROR_REPORTING,
                                             NISCOPE_VAL_ERROR_REPORTING_WARNING));

      // Configure DDC triggering parameters
      handleErr (niScope_SetAttributeViInt32 (vi, VI_NULL, NISCOPE_ATTR_REF_TRIGGER_DETECTOR_LOCATION,
                                              NISCOPE_VAL_DDC_OUTPUT));
      handleErr (niScope_SetAttributeViReal64 (vi, VI_NULL, NISCOPE_ATTR_REF_TRIGGER_MINIMUM_QUIET_TIME,                                                                                       0.0));

      switch (triggerType)
      {
         case 0: // Immediate
            handleErr (niScope_ConfigureTriggerImmediate (vi));
            break;

         case 1: // Edge
            handleErr (niScope_ConfigureTriggerEdge (vi, triggerSource, triggerLevel,
                                                     triggerSlope, triggerCoupling,
                                                     triggerHoldoff, triggerDelay));
            break;

         default:
            break;
      }

      //Set attribute so when we fetch complex data, it is split between real and imaginary (i and q)
      //If we don't set this, the data is interleaved and must be fetched using niScopeFetchComplex()
      handleErr(niScope_SetAttributeViBoolean(vi, VI_NULL, NISCOPE_ATTR_FETCH_INTERLEAVED_IQ_DATA, VI_FALSE));

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
      waveformPtr = malloc (sizeof (ViReal64) * actualRecordLength * numWaveform);

      // If it doesn't have enough memory, give an error message
      if (waveformPtr == NULL || wfmInfoPtr == NULL)
         handleErr (VI_ERROR_ALLOC);

      handleErr (niScope_Fetch (vi, channelName, timeout, actualRecordLength, waveformPtr, wfmInfoPtr));

      handleErr (niScope_SampleRate (vi, &actualSampleRate));

      // Plot the waveform (logged to a file)
      PlotWfms (numWaveform, waveformPtr, wfmInfoPtr, actualSampleRate, actualRecordLength);

      // Find out whether to stop or not
      ProcessEvent (&stop);
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
