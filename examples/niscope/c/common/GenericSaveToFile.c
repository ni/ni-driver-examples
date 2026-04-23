/********************************************************************************
 * Save To File generic file, used by examples in CVI, C, and C++
 *******************************************************************************/

#include "GenericSaveToFile.h"
#include <stdio.h>

//////////////////////////////////////////////////////////////////////////
// niScope_GenericSaveToFile
// ///////////////////////////////////////////////////////////////////////
ViStatus _VI_FUNC niScope_GenericSaveToFile(void)
{
   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errorSource[MAX_FUNCTION_NAME_SIZE];
   ViChar errorMessage[MAX_ERROR_DESCRIPTION] = " ";

   // Variables used to get values from the GUI
   ViChar resourceName[MAX_STRING_SIZE];
   ViChar channelName[MAX_STRING_SIZE];
   ViChar filePath[MAX_FILE_PATH];
   ViChar binPath[MAX_FILE_PATH];
   ViReal64 verticalRange;
   ViReal64 minSampleRate;
   ViInt32 minRecordLength;
   ViInt32 actualRecordLength;
   ViBoolean acquireOption;
   ViInt32 numWaveform,i;

   // Default values used in this example
   ViReal64 refPosition = 50.0;
   ViReal64 verticalOffset = 0.0;
   ViInt32  verticalCoupling = NISCOPE_VAL_DC;
   ViReal64 probeAttenuation = 1.0;
   ViBoolean enforceRealtime = NISCOPE_VAL_TRUE;
   ViReal64 timeout = 5.000;
   ViInt32 numRecords = 1;
   FILE* fileHandle;

   // Waveforms
   struct niScope_wfmInfo *wfmInfoPtr = NULL;
   ViReal64 *waveformPtr = NULL;

   // Obtain the necessary parameters from the user interface
   GetParametersFromGUI (channelName, &verticalRange, &minSampleRate,
                         &minRecordLength, filePath, &acquireOption);

   // Create a duplicate file name with .bin extension
   strcpy(binPath,  filePath);
   strcat(binPath,  ".BIN");
   strcat(filePath, ".TXT");

   // Find out if we need to acquire or read from the file
   if (acquireOption)
   {
      // Obtain the resource name of the device from the user interface
      GetResourceNameFromGUI (resourceName);

      // Open the NI-SCOPE instrument handle
      handleErr(niScope_init (resourceName, NISCOPE_VAL_FALSE, NISCOPE_VAL_FALSE, &vi));

      // Configure the vertical parameters
      handleErr(niScope_ConfigureVertical(vi, channelName, verticalRange, verticalOffset,
                                 verticalCoupling, probeAttenuation, NISCOPE_VAL_TRUE));

      // Configure the horizontal parameters, with the specified number of records
      handleErr(niScope_ConfigureHorizontalTiming(vi, minSampleRate, minRecordLength, refPosition, numRecords, enforceRealtime));

      // Find out the current number of waveforms
      handleErr(niScope_ActualNumWfms(vi, channelName, &numWaveform));
      // Do not support multiple channels in this example
      if (numWaveform > 1)
         handleErr (-1);

      // Configure immediate triggering
      handleErr(niScope_ConfigureTriggerImmediate (vi));

      // Initiate the acquisition
      handleErr(niScope_InitiateAcquisition(vi));

      // Find out the current record length
      handleErr(niScope_ActualRecordLength (vi, &actualRecordLength));

      // Allocate space for the waveform and waveform info according to the record length and number of waveforms
      if (wfmInfoPtr)
         free (wfmInfoPtr);
      wfmInfoPtr = malloc(sizeof(struct niScope_wfmInfo));

      if (waveformPtr)
         free (waveformPtr);
      waveformPtr = (ViReal64*) malloc (sizeof(ViReal64) * actualRecordLength);

      // If it doesn't have enough memory, give an error message
      if (waveformPtr == NULL || wfmInfoPtr == NULL)
         handleErr (VI_ERROR_ALLOC);

      // Fetch the data
      handleErr(niScope_Fetch (vi, channelName, timeout, actualRecordLength,  waveformPtr, wfmInfoPtr));

      // Open the text file
      fileHandle = fopen(filePath,"wt");
      if (fileHandle)
      {
         // Write the data
         for (i = 0; i<actualRecordLength; i++)
         {
            fprintf(fileHandle,"%f\n",waveformPtr[i]);
         }
         fclose(fileHandle);
      }
      else
      {
         handleErr(-3);
      }
      // Now pen a binary file easier to read back

      fileHandle = fopen(binPath,"wb");
      if (fileHandle)
      {
         // Write the number of samples
         fwrite(&actualRecordLength,4,1,fileHandle);
         // Write the data
         fwrite(waveformPtr,8,actualRecordLength,fileHandle);
         fclose(fileHandle);
      }
      else
      {
         handleErr(-3);
      }
   }
   else
   {
      // Open the binary file
      fileHandle = fopen(binPath,"rb");
      if (fileHandle)
      {
         // Read the number of samples
         fread(&actualRecordLength,4,1,fileHandle);

         // Allocate space
         if (waveformPtr)
            free (waveformPtr);
         waveformPtr = (ViReal64*) malloc (sizeof(ViReal64) * actualRecordLength);

         // If it doesn't have enough memory, give an error message
         if (waveformPtr == NULL)
            handleErr (VI_ERROR_ALLOC);

         // Read the data
         fread(waveformPtr,8,actualRecordLength,fileHandle);
         fclose(fileHandle);
      }
      else
      {
         handleErr (-2);
      }
   }

   // Plot the waveforms
   PlotWfms (waveformPtr, actualRecordLength);

Error :

   // Free all the allocated memory
   if (wfmInfoPtr)
      free (wfmInfoPtr);

   if (waveformPtr)
      free (waveformPtr);

   // Display messages
   if (error == -1)
      strcpy (errorMessage, "This example only works with one channel.");
   else if (error == -2)
      strcpy (errorMessage, "Unable to find requested BIN file.");
   else if (error == -3)
      strcpy (errorMessage, "Unable to open requested file for write.");
   else if (error != VI_SUCCESS)
      niScope_errorHandler (vi, error, errorSource, errorMessage);   // Intrepret the error
   else
      strcpy(errorMessage, "Acquisition successful!");

   DisplayErrorMessageInGUI (error, errorMessage);

   // Close the session
   if(vi)
     niScope_close(vi);

   return error;
}
