/********************************************************************************
 * Binary Acquisition Example, uses GenericBinaryAcquisition.h generic file
 *******************************************************************************/

#include <locale.h>
#include <stdio.h>
#include "asciiPlot.h"
#include "GenericBinaryAcquisition.h"

int main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform a binary acquisition
   niScope_GenericBinaryAcquisition ();
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
                         ViInt32 *triggerType,
                         ViReal64 *verticalRange,
                         ViReal64 *verticalOffset,
                         ViReal64 *minSampleRate,
                         ViInt32 *minRecordLength,
                         ViInt32  *binaryDataType)
{
   strcpy(channel,"0"); // Channel 0
   *triggerType = 0; //Edge
   *verticalRange = 10.0;
   *verticalOffset = 0.0;
   *minSampleRate = 10000000.0;
   *minRecordLength = 1000;

   // Get the type of the binary acquisition -- Only 1 character
   do
   {
      printf("Binary type of the data (1-Binary 8, 2-Binary 16, 4-Binary 32): ");
      scanf("%d", binaryDataType);
   } while (*binaryDataType != 1 && *binaryDataType != 2 && *binaryDataType != 4);

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
int PlotBinaryAndScaledWfms (ViInt32 numWaveforms,
                             ViInt32 binaryDataType,
                             void* binaryWfm,
                             ViReal64 *waveformPtr,
                             struct niScope_wfmInfo *wfmInfoPtr)
{
   // Display only the scaled waveform
   if (wfmInfoPtr && waveformPtr)
   {
      int i;
      for (i=0; i<numWaveforms; i++)
      {
         asciiPlot(waveformPtr, wfmInfoPtr[i].actualSamples);
         waveformPtr = waveformPtr + wfmInfoPtr[i].actualSamples;
      }
      // Display the vertical offset, gain factor and data type
      printf("Vertical Offset: %f\n", wfmInfoPtr[0].offset);
      printf("Gain Factor: %f\n", wfmInfoPtr[0].gain);
      switch (binaryDataType)
      {
      case 1:
         printf("Binary Data Type: Binary 8\n");
         break;
      case 2:
         printf("Binary Data Type: Binary 16\n");
         break;
      case 4:
         printf("Binary Data Type: Binary 32\n");
         break;
      }
   }
   return 0;
}

/*************************************************************************************\

                              End of example

\*************************************************************************************/
