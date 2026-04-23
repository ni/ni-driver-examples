/********************************************************************************
 * Binary Acquisition Example, uses GenericGettingStarted.h generic file
 *******************************************************************************/

#include "asciiPlot.h"
#include <locale.h>
#include <stdio.h>
#include "GenericGettingStarted.h"

int main ()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform a normal acquisition, using auto setup
   niScope_GenericGettingStarted();
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
int GetParametersFromGUI (ViChar* channel)
{
   strcpy(channel,"0");
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
int PlotWfms (ViReal64 *waveformPtr,
              struct niScope_wfmInfo *wfmInfoPtr,
              ViInt32 actualRecordLength,
              ViReal64 actualSampleRate)
{
   if (wfmInfoPtr && waveformPtr)
      asciiPlot(waveformPtr,wfmInfoPtr[0].actualSamples);
   printf("Actual record length: %d\n",actualRecordLength);
   printf("Actual sample rate: %.2f\n",actualSampleRate);
   return 0;
}
/*************************************************************************************\

                              End of example

\*************************************************************************************/
