/********************************************************************************
 * Multi Record Example, uses GenericFetchForever.h generic file
 *******************************************************************************/

#include "asciiPlot.h"
#include <locale.h>
#include <stdio.h>
#include <conio.h>
#include <time.h>
#include "GenericFetchForever.h"

#define MAX_STRING_SIZE 50
clock_t initialTime;

void main()
{
   setlocale(LC_ALL, "");

   // Reset the iteration counter
   initialTime = clock();
   printf("This example will fetch until any key is pressed.\n");
   printf("The screen will be updated everysecond.\n");

   // Call the generic function to perform a multi record acquisition
   niScope_GenericFetchForever();
   // Wait and exit
   printf("Press any key to exit.\n");
   _getch();
}

// Obtain the resource name of the device from the user
int GetResourceNameFromGUI (ViRsrc resourceName)
{
   //Get the device name from the user
   printf("Type the device name (e.g., Dev1, PXI1Slot2, Digitizer1, ...) from Measurement & Automation Explorer: ");
   scanf("%s", resourceName);

   return 0;
}

// Obtain the necessary parameters
int GetParametersFromGUI (ViChar* channel,
                          ViReal64* verticalRange,
                          ViReal64* minSampleRate,
                          ViInt32* maxPointsFetched)
{
   float sample;
   strcpy(channel,"0");
   *verticalRange = 10.0;
   *maxPointsFetched = 50000;

   // Get the sample rate
   printf("Type the device sample rate : ");
   scanf("%f", &sample);
   *minSampleRate = sample;

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
              ViInt32 totalPointsFetched)
{
   clock_t currentTime;
   currentTime = clock();
   // Plot only every second
   if ((double)(currentTime - initialTime) / CLOCKS_PER_SEC > 1)
   {
      initialTime = currentTime;
      // Display the points fetched
      printf("Total points fetched : %d\n",totalPointsFetched);
      if (waveformPtr && wfmInfoPtr)
      {
         // Plot the waveform
         printf("Points fetched last time : %d\n",wfmInfoPtr[0].actualSamples);
         asciiPlot(waveformPtr,wfmInfoPtr[0].actualSamples);
      }
   }
   return 0;
}

// Return true to stop after the first acquisition
int ProcessEvent (int *stopPtr)
{
   // Stop when keyboard is pressed
   if (_kbhit())
   {
      *stopPtr = VI_TRUE;
      _getch();
   }
   else
      *stopPtr = VI_FALSE;
   return 0;
}


/*************************************************************************************\

                              End of example

\*************************************************************************************/
