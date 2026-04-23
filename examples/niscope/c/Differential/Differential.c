/********************************************************************************
 * Differential Acquisition Example, uses GenericDifferential.h generic file
 *******************************************************************************/

#include <locale.h>
#include <stdio.h>
#include "asciiPlot.h"
#include "GenericDifferential.h"

int main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform a binary acquisition
   niScope_GenericDifferential ();
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
int GetParametersFromGUI (ViInt32 *terminalConfiguration,
                          ViReal64 *verticalRange,
                          ViReal64 *probeAttenuation,
                          ViInt32 *verticalCoupling,
                          ViReal64 *inputImpedance,
                          ViReal64 *minSampleRate,
                          ViInt32 *minRecordLength,
                          ViInt32 *triggerType,
                          ViReal64 *triggerLevel,
                          ViInt32 *triggerSlope)
{
   ViInt32 termConfigOption;

   // Get the type of the binary acquisition -- Only 1 character
   do
   {
      printf("Channel terminal configuration:\n");
      printf("1-Single Ended\n");
      printf("2-Unbalanced Differential\n");
      printf("3-Differential\n");
      printf("Option: ");
      scanf("%d", &termConfigOption);
   } while (termConfigOption != 1 && termConfigOption != 2 && termConfigOption != 3);

   switch (termConfigOption)
   {
   case 1:
      *terminalConfiguration = NISCOPE_VAL_SINGLE_ENDED;
      break;
   case 2:
      *terminalConfiguration = NISCOPE_VAL_UNBALANCED_DIFFERENTIAL;
      break;
   case 3:
      *terminalConfiguration = NISCOPE_VAL_DIFFERENTIAL;
      break;
   }

   *verticalRange = 10.0;
   *probeAttenuation = 1.0;
   *verticalCoupling = NISCOPE_VAL_DC;
   *inputImpedance = 1000000;
   *minSampleRate = 10000000.0;
   *minRecordLength = 1000;
   *triggerType = 0; //Edge
   *triggerLevel = 0.0;
   *triggerSlope = NISCOPE_VAL_POSITIVE;

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
int PlotWfm (ViInt32 numWaveforms,
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
   }
   return 0;
}

/*************************************************************************************\

                              End of example

\*************************************************************************************/
