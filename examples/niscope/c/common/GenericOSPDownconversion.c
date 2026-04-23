/********************************************************************************
 * OSP Downconversion generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericOSPDownconversion.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericConfiguredAcquisition
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericOSPDownconversion (void)
{
   ViStatus error = VI_SUCCESS;
   ViChar   errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar   errorMessage[MAX_ERROR_DESCRIPTION] = " ";
   ViSession vi;

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];


   ViReal64 verticalRange;
   ViInt32 inputImpedance;
   ViReal64 minSampleRate;
   ViInt32 minRecordLength;
   ViReal64 timeout;
   ViReal64 digitalGain;
   ViReal64 phaseI;
   ViReal64 centerFrequency;


   // Default values used in this example
   ViInt32 stop = NISCOPE_VAL_FALSE;
   ViInt32 numWaveform;
   ViInt32 arrayNumWaveform;
   ViInt32 actualRecordLength;
   ViInt32 arrayActualRecordLength;
   ViReal64 actualSampleRate;

   // Waveforms
   ViReal64* waveformPtr = NULL;
   ViReal64* frequencyWaveformPtr = NULL;
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   struct niScope_wfmInfo *frequencyWfmInfoPtr = NULL;

   ViBoolean addedProcessing=0;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Loop until the stop flag is set
   while (!stop)
   {
      // Obtain the necessary parameters from the user interface
      GetParametersFromGUI (channelName, &verticalRange, &inputImpedance,&minSampleRate,
                            &minRecordLength,&timeout,&digitalGain, &phaseI, &centerFrequency);

      // Configure the vertical parameters
      handleErr (niScope_SetAttributeViReal64 (vi, channelName, NISCOPE_ATTR_VERTICAL_RANGE, verticalRange));
      handleErr (niScope_SetAttributeViBoolean (vi, channelName, NISCOPE_ATTR_CHANNEL_ENABLED, NISCOPE_VAL_TRUE));

      // Configure the channel characteristics
      handleErr (niScope_ConfigureChanCharacteristics (vi, channelName, inputImpedance,
                                                      NISCOPE_VAL_BANDWIDTH_FULL));

      // Configure the horizontal parameters
      handleErr (niScope_ConfigureHorizontalTiming (vi, minSampleRate, minRecordLength,
                                                   50.0, 1, NISCOPE_VAL_TRUE));

      // Configure DDC parameters
      handleErr(niScope_SetAttributeViBoolean(vi, channelName, NISCOPE_ATTR_DDC_ENABLED, VI_TRUE));
      handleErr(niScope_SetAttributeViBoolean(vi, channelName, NISCOPE_ATTR_DDC_FREQUENCY_TRANSLATION_ENABLED, VI_TRUE));
      handleErr(niScope_SetAttributeViReal64(vi, channelName, NISCOPE_ATTR_DDC_FREQUENCY_TRANSLATION_PHASE_I,phaseI));
      handleErr(niScope_SetAttributeViReal64(vi, channelName, NISCOPE_ATTR_DDC_CENTER_FREQUENCY,centerFrequency));
      handleErr(niScope_SetAttributeViReal64(vi, channelName, NISCOPE_ATTR_DIGITAL_GAIN, digitalGain));

      handleErr(niScope_SetAttributeViInt32 (vi, VI_NULL, NISCOPE_ATTR_DDC_DATA_PROCESSING_MODE,
                                             NISCOPE_VAL_REAL));

      handleErr(niScope_SetAttributeViInt32 (vi, VI_NULL, NISCOPE_ATTR_OVERFLOW_ERROR_REPORTING,
                                             NISCOPE_VAL_ERROR_REPORTING_WARNING));

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

      // Fetch the data
      handleErr (niScope_Fetch (vi, channelName, timeout, actualRecordLength,
                                    waveformPtr, wfmInfoPtr));

      // Find out the current record length for the array measurement
      handleErr(niScope_ActualMeasWfmSize(vi, NISCOPE_VAL_FFT_AMP_SPECTRUM_DB, &arrayActualRecordLength));
      arrayNumWaveform = numWaveform;

      // Allocate space for the waveform and waveform info according to the record length and number of waveforms of the array measurement
      if (frequencyWfmInfoPtr)
         free (frequencyWfmInfoPtr);
      frequencyWfmInfoPtr = malloc(sizeof(struct niScope_wfmInfo) * arrayNumWaveform);

      if (frequencyWaveformPtr)
         free (frequencyWaveformPtr);
      frequencyWaveformPtr = malloc(sizeof(ViReal64) * arrayActualRecordLength * arrayNumWaveform);

      // If it doesn't have enough memory, give an error message
      if (frequencyWaveformPtr == NULL || frequencyWfmInfoPtr == NULL)
         handleErr (VI_ERROR_ALLOC);

      // Add hanning window waveform processing
      if(addedProcessing==0)
      {
         handleErr (niScope_AddWaveformProcessing (vi, channelName, NISCOPE_VAL_HANNING_WINDOW));
         addedProcessing=1;
      }
      niScope_FetchArrayMeasurement (vi, channelName, 5.0, NISCOPE_VAL_FFT_AMP_SPECTRUM_DB, arrayActualRecordLength,
                                     frequencyWaveformPtr, frequencyWfmInfoPtr);

      // Get the actual sample rate
      handleErr (niScope_SampleRate (vi, &actualSampleRate));

      // Plot the waveform
      PlotWfms (numWaveform, waveformPtr, wfmInfoPtr, frequencyWaveformPtr, frequencyWfmInfoPtr,
                actualSampleRate, actualRecordLength);

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

