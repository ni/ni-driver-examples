/********************************************************************************
 * Baseband Decimation Example, uses GenericOSPDownconversion.h generic file.
 *
 * This file contains all the interface code for your application.  The
 * GenericBasebandDecimation file contains the implementation code that configures
 * and acquires data using the digitizer.
 *******************************************************************************/

#include <locale.h>
#include <stdio.h>
#include <conio.h>
#include "GenericOSPDownconversion.h"
#include "asciiPlot.h"

void main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform an acquisition
   niScope_GenericOSPDownconversion();
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
int GetParametersFromGUI (ViChar* channelName,
                             ViReal64* verticalRange,
                             ViInt32* inputImpedance,
                             ViReal64* minSampleRate,
                             ViInt32* minRecordLength,
                             ViReal64* timeout,
                             ViReal64 *digitalGain,
                             ViReal64 *phaseI,
                             ViReal64 *centerFrequency)
{
   // hardcoded attribute values
   strcpy(channelName,"0");
   *verticalRange = 1.00;
   *inputImpedance = 50;
   *minSampleRate = 1000000;
   *minRecordLength = 100;
   *timeout = 5.000;
   *digitalGain = 1.0;
   *phaseI = 0.00;
   *centerFrequency = 10000000;

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
int DisplayErrorMessageInGUI (
   ViInt32 error,
   ViConstString errorMessage)
{
   printf("%s\n",errorMessage);
   return 0;
}

// Plot waveforms writes the data from the acquisition to a file.
// Plot the waveforms
int PlotWfms (ViInt32 numWaveforms,
              ViReal64 *waveformPtr,
              struct niScope_wfmInfo *wfmInfoPtr,
              ViReal64 *frequencyWaveformPtr,
              struct niScope_wfmInfo *frequencyWfmInfoPtr,
              ViReal64 actualSampleRate,
              ViInt32 actualRecordLength)
{

   if (waveformPtr && wfmInfoPtr && frequencyWaveformPtr && frequencyWfmInfoPtr)
   {
      int i;
      for(i=0; i<numWaveforms; i++)
      {
         // Plot waveform
         asciiPlot(waveformPtr,wfmInfoPtr[i].actualSamples);
         asciiPlot(frequencyWaveformPtr,frequencyWfmInfoPtr[i].actualSamples);

         // Increment waveform pointers to the next waveform
         waveformPtr = waveformPtr + wfmInfoPtr[i].actualSamples;
         frequencyWaveformPtr = frequencyWaveformPtr + frequencyWfmInfoPtr[i].actualSamples;
      }
      printf("Actual Sample Rate : %.2f\n",actualSampleRate);
      printf("Actual Record Length : %d\n",actualRecordLength);
   }
   return 0;
}
/*************************************************************************************\

                              End of example

\*************************************************************************************/
