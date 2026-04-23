/********************************************************************************
 * OSP Quadrature Downconversion Example, uses GenericOSPQuadratureDownconversion.h generic file.
 *
 * This file contains all the interface code for your application.  The
 * GenericOSPQuadratureDownconversion file contains the implementation code that configures
 * and acquires data using the digitizer.
 *******************************************************************************/

#include <locale.h>
#include <stdio.h>
#include <conio.h>
#include "GenericOSPQuadratureDownconversion.h"
#include "asciiPlot.h"

void main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform an acquisition
   niScope_GenericOSPQuadratureDownconversion();
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
int GetParametersFromGUI (
   ViChar* channelName,
   ViReal64* verticalRange,
   ViInt32* inputImpedance,
   ViReal64* minIQRate,
   ViInt32* minRecordLength,
   ViReal64* timeout,
   ViReal64* digitalGain,
   ViReal64* centerFrequency,
   ViReal64* phaseI,
   ViReal64* phaseQ,
   ViInt32* triggerType,
   ViReal64* triggerLevel,
   ViReal64* triggerMinQuietTime,
   ViBoolean* enableFracResample)
{
   // hardcoded attribute values
   strcpy(channelName,"0");
   *verticalRange = 1.0;
   *minRecordLength = 100;
   *timeout = 5.000;
   *digitalGain = 1.0;
   *centerFrequency = 10e6;
   *minIQRate = 1e6;
   *phaseI = 0.0;
   *phaseQ = 90.0;
   *triggerType = 0; // immediate
   *triggerLevel = 0.0;
   *triggerMinQuietTime = 0.0;
   *enableFracResample = VI_FALSE;
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
int PlotWfms (
   ViInt32 numWaveforms,
   ViReal64* waveformPtr,
   struct niScope_wfmInfo* wfmInfoPtr,
   ViReal64 actualSampleRate,
   ViInt32 actualRecordLength)
{
   ViReal64* XYPlotPtr = NULL;
   ViReal64* uniqueValues = NULL;
   ViBoolean wasFound = 0;
   if (wfmInfoPtr && waveformPtr)
   {
      int i;
      for (i=0; i<numWaveforms; i++)
      {
         asciiPlot(waveformPtr,wfmInfoPtr[i].actualSamples);
      }
      printf("Actual Sample Rate : %.2f\n",actualSampleRate);
      printf("Actual Record Length : %d\n",actualRecordLength);
   }

   return 0;
}
/*************************************************************************************\

                              End of example

\*************************************************************************************/
