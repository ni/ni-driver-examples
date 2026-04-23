/********************************************************************************
 * Multi Record Fetch More Than Available Memory Example, uses GenericMultiRecordFetchMoreThanAvailableMemory.h generic file
 *******************************************************************************/

#include "asciiPlot.h"
#include <locale.h>
#include <stdio.h>
#include "GenericMultiRecordFetchMoreThanAvailableMemory.h"

#define MAX_STRING_SIZE 50
static int plotWfmOption; // plot flag

int main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform multi record continuous acquisition
   niScope_GenericMultiRecordFetchMoreThanAvailableMemory();
   // Wait and exit
   printf("Press <RETURN> to exit.\n");
   getc(stdin); getc(stdin);
   return 0;
}

// Obtain the resource name of the device from the user
int GetResourceNameFromGUI (ViRsrc resourceName)
{
   //Get the device name from the user
   printf("Type the device name (e.g., Dev1, PXI1Slot2, Digitizer1, ...): ");
   scanf("%s", resourceName);

   return 0;
}

// Obtain the necessary parameters
int GetParametersFromGUI (ViChar* channelName,
                          ViReal64* verticalRange,
                          ViReal64* minSampleRate,
                          ViInt32* minRecordLength,
                          ViInt32* numRecords,
                          ViBoolean* allowRecords)
{
   strcpy(channelName,"0");
   *verticalRange = 10.0;
   *minSampleRate = 10000000.0;
   *minRecordLength = 4096;

   // Get the number of records from the user
   printf("Type number of records: ");
   scanf("%d", numRecords);

   *allowRecords = VI_TRUE;

   // Find out if the user wants to plot or not
   printf("Plot waveforms? (1-Yes, 0-No): ");
   scanf("%d", &plotWfmOption);

   return 0;
}

// Return true to stop after the first acquisition
int ProcessEvent(int* stop)
{
   // Acquire once, stop after first acquisition
   *stop = NISCOPE_VAL_FALSE;
   return 0;
}

// Display message - do a printf
int DisplayErrorMessageInGUI (ViInt32 error,
                              ViConstString errorMessage)
{
   printf("%s\n", errorMessage);
   return 0;
}

// Plot waveforms, uses ascii plot to display the waveforms in ascii
int PlotWfms (ViInt32 record,
              ViInt32 recordsAcquired,
              ViInt32 numWaveforms,
              ViReal64 *waveformPtr,
              struct niScope_wfmInfo *wfmInfoPtr)
{
   printf("Records Fetched: %d\t Records acquired to on-board memory: %d\n",
          record,
          recordsAcquired);
   if (waveformPtr && wfmInfoPtr && plotWfmOption == 1)
   {
      int i;
      for (i=0; i<numWaveforms; i++)
      {
         asciiPlot(waveformPtr, wfmInfoPtr[i].actualSamples);
         waveformPtr = waveformPtr + wfmInfoPtr[i].actualSamples;
      }
   }
   return 0;
}

/*************************************************************************************\

                              End of example

\*************************************************************************************/
