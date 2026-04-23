/********************************************************************************
 * External Amplifier Example, uses GenericExternalAmplifier.h generic file
 *******************************************************************************/

#include "asciiPlot.h"
#include <locale.h>
#include <stdio.h>
#include "GenericExternalAmplifier.h"

int main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform an acquisition
   niScope_GenericExternalAmplifier();
   // Wait and exit
   printf("Press <RETURN> to exit.\n");
   getc(stdin); getc(stdin);
   return 0;
}

// Obtain the resource name of the device from the user
int GetResourceNameFromGUI (ViRsrc resourceName)
{
   //Get the device name from the user
   printf("Type the oscilloscope device name (e.g., Dev1, PXI1Slot2, Digitizer1, ...): ");
   scanf("%s", resourceName);

   return 0;
}

// Obtain the accessory name of the device from the user
int GetAccessoryNameFromGUI (ViRsrc accessoryName)
{
   //Get the accessory name from the user
   printf("Type the accessory device name (e.g., Dev1, PXI1Slot2, Digitizer1, ...): ");
   scanf("%s", accessoryName);

   return 0;
}

// Obtain the necessary parameters
int GetParametersFromGUI (ViChar* channel,
                          ViInt32* acquisitionType,
                          ViReal64* verticalRange,
                          ViReal64* verticalOffset,
                          ViInt32* verticalCoupling,
                          ViReal64* inputImpedance,
                          ViReal64* maxInputFrequency,
                          ViReal64* minSampleRate,
                          ViInt32* minRecordLength,
                          ViReal64* timeout,
                          ViInt32 *numRecords,
                          ViReal64 *refPos,
                          ViChar *triggerSource,
                          ViInt32 *triggerCoupling,
                          ViInt32 *triggerSlope,
                          ViReal64 *triggerLevel,
                          ViReal64 *triggerHoldoff,
                          ViReal64 *triggerDelay)
{
   //////////////////////////////////////////////////////////////////////////////////////////////////////
   // NOTE: Some devices may require configuring input impedance, vertical coupling and vertical range.
   //////////////////////////////////////////////////////////////////////////////////////////////////////


   strcpy(channel,"0");
   *acquisitionType = NISCOPE_VAL_NORMAL;
   *verticalRange = 10.0;
   *verticalOffset = 0.0;
   *verticalCoupling = NISCOPE_VAL_DC;
   *inputImpedance = NISCOPE_VAL_1_MEG_OHM; //the other option for input impedance is 50 or 75 ohm
   *maxInputFrequency = 0.0;
   *minSampleRate = 10000000;
   *minRecordLength = 1000;

   *timeout = 5.000;
   *numRecords = 1;
   *refPos = 50.0;

   *triggerHoldoff = 0.0;
   *triggerDelay = 0.0;

   //Assign the default values for the trigger
   strcpy(triggerSource,"0");
   *triggerLevel = 0.0;
   *triggerSlope = NISCOPE_VAL_POSITIVE;
   *triggerCoupling = NISCOPE_VAL_DC;

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
              ViReal64 *waveformPtr,
              struct niScope_wfmInfo *wfmInfoPtr,
              ViReal64 actualSampleRate,
              ViInt32 actualRecordLength)
{
   if (wfmInfoPtr && waveformPtr)
   {
      int i;
      for (i=0; i<numWaveforms; i++)
      {
         asciiPlot(waveformPtr,wfmInfoPtr[i].actualSamples);
         waveformPtr = waveformPtr + wfmInfoPtr[i].actualSamples;
      }
      printf("Actual Sample Rate: %.2f\n",actualSampleRate);
      printf("Actual Record Length: %d\n",actualRecordLength);
   }
   return 0;
}
/*************************************************************************************\

                              End of example

\*************************************************************************************/
