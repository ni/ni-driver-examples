/********************************************************************************
 * Flexible Resolution Example, uses GenericFlexRes.h generic file
 *******************************************************************************/

#include "asciiPlot.h"
#include <locale.h>
#include <stdio.h>
#include "GenericFlexRes.h"

#define MAX_STRING_SIZE 50

int main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform a flex res acquisition
   niScope_GenericFlexRes();
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
int GetParametersFromGUI (ViReal64* verticalRange,
                          ViReal64* minSampleRate,
                          ViInt32* minRecordLength,
                          ViInt32* triggerType)
{
   float minRate;
   *verticalRange = 10.0;
   *minRecordLength = 1000;
   *triggerType = 0;

   // Get the desired sample frequency from the user
   printf("Type minimum sample frequency: ");
   scanf("%f", &minRate);
   printf("\n");

   *minSampleRate = minRate;

   return 0;
}

// Return true to stop after the first acquisition
int ProcessEvent (int *stopPtr)
{
   // Acquire once, stop after first acquisition
   *stopPtr = NISCOPE_VAL_TRUE;
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
int PlotWfms (ViReal64 *waveform,
              struct niScope_wfmInfo *wfmInfoPtr,
              ViReal64 *freqWaveform,
              struct niScope_wfmInfo *freqWfmInfoPtr,
              ViReal64 actualSampleRate,
              ViInt32 resolution)
{
   if (waveform && wfmInfoPtr)
   {
      asciiPlot(waveform, wfmInfoPtr[0].actualSamples);
   }
   printf("Actual Sample Rate: %f\n", actualSampleRate);
   printf("Advertised Effective Number of Bits: %d\n", resolution);
   return 0;
}
/*************************************************************************************\

                              End of example

\*************************************************************************************/
