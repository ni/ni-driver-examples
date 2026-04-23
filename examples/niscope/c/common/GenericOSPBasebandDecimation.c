/********************************************************************************
 * DDC Advanced Low Level generic file
 *******************************************************************************/

#include "GenericOSPBasebandDecimation.h"

//////////////////////////////////////////////////////////////////////////
// niScope_GenericOSPBasebandDecimation
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericOSPBasebandDecimation (void)
{
   ViStatus error = VI_SUCCESS;
   ViChar   errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar   errorMessage[MAX_ERROR_DESCRIPTION] = " ";
   ViSession vi;

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar triggerSource[MAX_STRING_SIZE];


   ViReal64 verticalRange;
   ViInt32 inputImpedance;
   ViReal64 minSampleRate;
   ViInt32 minRecordLength;
   ViReal64 timeout;
   ViInt32 triggerType;
   ViReal64 triggerLevel;


   // Default values used in this example
   ViInt32 stop = NISCOPE_VAL_FALSE;
   ViInt32 numWaveform;
   ViInt32 actualRecordLength;
   ViReal64 actualSampleRate;

   // Waveforms
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   ViReal64 *waveformPtr = NULL;

   int i=0;

   // Obtain the resource name of the device from the user interface
   GetResourceNameFromGUI (resourceName);

   // Open the NI-SCOPE instrument handle
   handleErr (niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

   // Loop until the stop flag is set
   while (!stop)
   {
      // Obtain the necessary parameters from the user interface
      GetParametersFromGUI (&verticalRange, &inputImpedance,&minSampleRate,
                            &minRecordLength,&timeout, &triggerType, triggerSource, &triggerLevel);




      // Configure the vertical parameters
     handleErr (niScope_ConfigureVertical (vi, "0,1", verticalRange, 0, NISCOPE_VAL_DC,
                                           1.0, NISCOPE_VAL_TRUE));

      // Configure the channel characteristics
      handleErr (niScope_ConfigureChanCharacteristics (vi, "0,1", inputImpedance,
                                                      NISCOPE_VAL_BANDWIDTH_FULL));

      // Configure the horizontal parameters
      handleErr (niScope_ConfigureHorizontalTiming (vi, minSampleRate, minRecordLength,
                                                   50.0, 1, NISCOPE_VAL_TRUE));

      // Configure the trigger type
      switch (triggerType)
      {
         case 0: //Edge Trigger
            handleErr (niScope_ConfigureTriggerEdge (vi, triggerSource, triggerLevel,
                                                     NISCOPE_VAL_POSITIVE, NISCOPE_VAL_DC, 0.0, 0.0));
            break;

         case 1: //Immediate Triggering
            handleErr (niScope_ConfigureTriggerImmediate (vi));
            break;

         default:
            //don't do anything
            break;
      }

      // Configure DDC parameters
     handleErr(niScope_SetAttributeViBoolean(vi, "0", NISCOPE_ATTR_DDC_ENABLED, VI_TRUE));
     handleErr(niScope_SetAttributeViBoolean(vi, "0", NISCOPE_ATTR_DDC_FREQUENCY_TRANSLATION_ENABLED,
                                             VI_FALSE));

     handleErr(niScope_SetAttributeViString(vi, "0", NISCOPE_ATTR_DDC_Q_SOURCE, "1"));

     handleErr(niScope_SetAttributeViInt32 (vi, VI_NULL, NISCOPE_ATTR_DDC_DATA_PROCESSING_MODE,
                                            NISCOPE_VAL_COMPLEX));

     handleErr(niScope_SetAttributeViInt32 (vi, VI_NULL, NISCOPE_ATTR_OVERFLOW_ERROR_REPORTING,
                                            NISCOPE_VAL_ERROR_REPORTING_WARNING));

     //Set attribute so when we fetch complex data, it is split between real and imaginary (i and q)
     //If we don't set this, the data is interleaved and must be fetched using niScopeFetchComplex()
     handleErr(niScope_SetAttributeViBoolean(vi, VI_NULL, NISCOPE_ATTR_FETCH_INTERLEAVED_IQ_DATA, VI_FALSE));


      // Initiate the acquisition
      handleErr (niScope_InitiateAcquisition (vi));

      // Find out the current record length and number of waveforms
      handleErr (niScope_ActualNumWfms (vi, "0", &numWaveform));

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

      // Fetch the data
      handleErr (niScope_Fetch (vi, "0", timeout, actualRecordLength,
                                waveformPtr, wfmInfoPtr));

      // Get the actual sample rate
      handleErr (niScope_SampleRate (vi, &actualSampleRate));

      // Plot the waveform

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
