/********************************************************************************
 * Sample Width Example, uses GenericSampleWidth.h generic file
 *******************************************************************************/

#include <locale.h>
#include <stdio.h>
#include "asciiPlot.h"
#include "GenericSampleWidth.h"

int main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform an acquisition and change the sample width
   niScope_GenericSampleWidth ();
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
                         ViInt32  *binarySampleWidth)
{
   strcpy(channel,"0"); // Channel 0
   *triggerType = 0; //Edge
   *verticalRange = 10.0;
   *verticalOffset = 0.0;
   *minSampleRate = 10000000.0;
   *minRecordLength = 50;

   // Get the type of the binary acquisition -- Only 1 character
   do
   {
      printf("Binary width of the data (8-(8 Bits), 16-(16 Bits), 32-(32 Bits)): ");
      scanf("%d", binarySampleWidth);
   } while (*binarySampleWidth != 8 && *binarySampleWidth != 16 && *binarySampleWidth != 32);

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
int PlotWfms (ViInt32 numWaveforms,
              ViInt32 binarySampleWidth,
              void* binaryWfm,
              struct niScope_wfmInfo *wfmInfoPtr)
{
   // Display only the waveform
   if (wfmInfoPtr && binaryWfm)
   {
      int i;
      switch (binarySampleWidth)
      {
      case 8:
         {
            ViInt8* ptr = (ViInt8*)binaryWfm;
            for (i=0; i<wfmInfoPtr[0].actualSamples; i++)
               printf ("Value %d: %d\n", i, ptr[i]);
         }
         break;

      case 16:
         {
            ViInt16* ptr = (ViInt16*)binaryWfm;
            for (i=0; i<wfmInfoPtr[0].actualSamples; i++)
               printf ("Value %d: %d\n", i, ptr[i]);
         }
         break;

      case 32:
         {
            ViInt32* ptr = (ViInt32*)binaryWfm;
            for (i=0; i<wfmInfoPtr[0].actualSamples; i++)
               printf ("Value %d: %d\n", i, ptr[i]);
         }
         break;

      default:
         break;
      }
   }
   return 0;
}

/*************************************************************************************\

                              End of example

\*************************************************************************************/
