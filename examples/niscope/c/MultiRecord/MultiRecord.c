/********************************************************************************
 * Multi Record Example, uses GenericMultiRecord.h generic file
 *******************************************************************************/

#include "asciiPlot.h"
#include <locale.h>
#include <stdio.h>
#include "GenericMultiRecord.h"

#define MAX_STRING_SIZE 50

int main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform a multi record acquisition
   niScope_GenericMultiRecord();
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
                          ViInt32* numRecords)
{
   strcpy(channelName,"0");
   *verticalRange = 10.0;
   *minSampleRate = 10000000;
   *minRecordLength = 1000;

   // Get the number of records from the user
   printf("Type number of records: ");
   scanf("%d", numRecords);

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
int PlotWfms (ViInt32 numWaveforms,
              ViReal64 *waveformPtr,
              struct niScope_wfmInfo *wfmInfoPtr,
              ViReal64 actualSampleRate,
              ViInt32 actualRecordLength)
{
   if (waveformPtr && wfmInfoPtr)
   {
      int i;
      for (i=0; i<numWaveforms; i++)
      {
         asciiPlot(waveformPtr,wfmInfoPtr[i].actualSamples);
         printf("Actual sample rate: %.2f\n",actualSampleRate);
         printf("Actual record length: %d\n",actualRecordLength);
         waveformPtr = waveformPtr + wfmInfoPtr[i].actualSamples;
      }
   }
   return 0;
}

/*************************************************************************************\

                              End of example

\*************************************************************************************/
