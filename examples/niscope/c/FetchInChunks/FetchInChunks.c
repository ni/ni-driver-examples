/********************************************************************************
 * Fetch in Chunks Example, uses GenericFetchInChunks.h generic file
 *******************************************************************************/

#include "asciiPlot.h"
#include <locale.h>
#include <stdio.h>
#include "GenericFetchInChunks.h"

#define MAX_STRING_SIZE 50
#define MIN_RECORD_LENGTH 2048

int main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform a continuous acquisition fetching in chunks
   niScope_GenericFetchInChunks();
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
int GetParametersFromGUI (ViChar* channel,
                          ViReal64* verticalRange,
                          ViReal64* minSampleRate,
                          ViInt32* minRecordLength,
                          ViInt32* triggerType,
                          ViInt32* maxFetchSize)
{
   int tempMaxFetchSize;

   strcpy(channel,"0");
   *verticalRange = 2.0;
   *minSampleRate = 10000000;
   *minRecordLength = (ViInt32)MIN_RECORD_LENGTH;
   *triggerType = 0; //EDGE

   // Get the maximum fetch size from the user
   printf("Type the max fetch size (e.g. 128): ");
   scanf("%d", &tempMaxFetchSize);
   *maxFetchSize = tempMaxFetchSize;

   return 0;
}

// Display message - do a printf
int DisplayErrorMessageInGUI (ViInt32 error,
                              ViConstString errorMessage)
{
   printf("%s\n",errorMessage);
   return 0;
}

// Plot waveforms, uses ascii plot to display the waveforms in ascii
int PlotWfms (ViInt32 numPointsFetched,
              ViInt32 chunksFetched,
              ViReal64 *waveformPtr,
              struct niScope_wfmInfo *wfmInfoPtr)
{
   ViInt32 i = 0;
   static ViInt32 pointsCopied = 0;
   static ViReal64 wfm[MIN_RECORD_LENGTH];

   printf("Chunks Fetched: %d\tPoints Fetched: %d\n",
          chunksFetched, numPointsFetched);

   if (wfmInfoPtr && waveformPtr)
   {
          while (pointsCopied < MIN_RECORD_LENGTH &&
                     i < wfmInfoPtr->actualSamples)
                  wfm[pointsCopied++] = waveformPtr[i++];

      if (pointsCopied == MIN_RECORD_LENGTH)
         asciiPlot(wfm, MIN_RECORD_LENGTH);
   }
   return 0;
}
/*************************************************************************************\

                              End of example

\*************************************************************************************/
