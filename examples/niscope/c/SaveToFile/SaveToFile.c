/********************************************************************************
 * Multi Record Example, uses GenericSaveToFile.h generic file
 *******************************************************************************/

#include "asciiPlot.h"
#include <locale.h>
#include <stdio.h>
#include "GenericSaveToFile.h"

#define MAX_STRING_SIZE 50

int main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform a multi record acquisition
   niScope_GenericSaveToFile();
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
                          ViChar* filePath,
                          ViBoolean* acquireOption)
{
   int option;
   strcpy(channelName,"0");
   *verticalRange = 10.0;
   *minSampleRate = 10000000;
   *minRecordLength = 1000;

   // Get the number of records from the user
   printf("Type the path of the file to use without the file extension: ");
   scanf("%s", filePath);

   // Get the option to acquire or read
   printf("Acquire and save (1) or Read from BIN file (0): ");
   scanf("%d", &option);

   *acquireOption = option;
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
              ViInt32 actualSamples)
{
   if (waveformPtr)
   {
      asciiPlot(waveformPtr, actualSamples);
   }
   return 0;
}

/*************************************************************************************\

                              End of example

\*************************************************************************************/
