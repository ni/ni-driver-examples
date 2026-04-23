/********************************************************************************
 * Random Interleaved Sampling Example, uses GenericRandomInterleavedSampling.h generic file
 *******************************************************************************/

#include "asciiPlot.h"
#include <locale.h>
#include <stdio.h>
#include "GenericRandomInterleavedSampling.h"

#define MAX_STRING_SIZE 50

int main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform an RIS acquisition
   niScope_GenericRandomInterleavedSampling();
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
                         ViReal64* minRISRate,
                         ViInt32* minRISRecordLength,
                         ViInt32* numAverages,
                         ViInt32* risMethod)
{
   float tempRate;

   strcpy(channelName,"0");
   *verticalRange = 10.0;
   *minRISRecordLength = 256;

   // Get the RIS rate from the user
   printf("Type minimum RIS rate: ");
   scanf("%f", &tempRate);
   *minRISRate = (ViReal64)tempRate;

   // Get the number of averages for RIS
   printf("Type number of averages: ");
   scanf("%d", numAverages);

   *risMethod = NISCOPE_VAL_RIS_EXACT_NUM_AVERAGES;

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
int PlotWfms (ViReal64 *realtimeWfmPtr,
                struct niScope_wfmInfo *realtimeWfmInfoPtr,
                ViReal64 *risWaveformPtr,
                struct niScope_wfmInfo *risWfmInfoPtr,
                ViReal64 realtimeActualSampleRate,
                ViReal64 risActualSampleRate,
                ViInt32 oversamplingFactor)
{
   // Display real time waveform
   if (realtimeWfmPtr && realtimeWfmInfoPtr)
   {
      printf("Real time acquisition waveform, %d samples:\n",
             realtimeWfmInfoPtr->actualSamples);
      asciiPlot(realtimeWfmPtr, realtimeWfmInfoPtr->actualSamples);
      printf("Real time sample rate: %.2f\n", realtimeActualSampleRate);
   }
   // Display RIS waveform
   if (risWfmInfoPtr && risWaveformPtr)
   {
      printf("Random Interleaved Sampling acquisition waveform, %d samples:\n",
             risWfmInfoPtr->actualSamples);
      asciiPlot(risWaveformPtr, risWfmInfoPtr->actualSamples);
      printf("Random Interleaved Sampling sample rate: %.2f\n",risActualSampleRate);
      printf("Oversampling Factor: %d\n",oversamplingFactor);
   }
   return 0;
}

/*************************************************************************************\

                              End of example

\*************************************************************************************/
