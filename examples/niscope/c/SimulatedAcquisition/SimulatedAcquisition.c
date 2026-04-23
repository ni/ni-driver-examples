/********************************************************************************
 * Configured Acquisition Example, uses GenericConfiguredAcquisition.h generic file
 *******************************************************************************/

#include "asciiPlot.h"
#include <locale.h>
#include <stdio.h>
#ifndef kGCC
   #include <conio.h>
#endif
#include "GenericSimulatedAcquisition.h"

int main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform an acquisition
   niScope_GenericSimulatedAcquisition ();
   // Wait and exit
#ifndef kGCC
   printf("Press any key to exit.\n");
   _getch();
#else
   printf("Press <RETURN> to exit.\n");
   getc(stdin); getc(stdin);
#endif
   return 0;
}

// Obtain the resource name of the device from the user
int GetResourceNameFromGUI (ViRsrc resourceName,
                            ViChar* model,
                            ViChar* type,
                            ViReal64* verticalNoise)
{
   //Get the resource name, model and type from the user
   printf ("Type the simulated device name (e.g., Dev1, PXI1Slot2, Digitizer1) : ");
   scanf ("%s", resourceName);

   printf ("Type the desired model to be simulated (e.g. 5114, 5122) : ");
   scanf ("%s", model);

   printf ("Type the desired board type to be simulated (e.g. PXI, PCI, USB, PXIe) : ");
   scanf ("%s", type);

   *verticalNoise = 4.0;

   return 0;
}

// Obtain the necessary parameters
int GetParametersFromGUI (ViChar* channel,
                          ViReal64* verticalRange,
                          ViInt32* verticalCoupling,
                          ViReal64* probeAttenuation,
                          ViReal64* inputImpedance,
                          ViReal64* maxInputFrequency,
                          ViReal64* minSampleRate,
                          ViInt32* minRecordLength,
                          ViBoolean* enforceRealTime,
                          ViInt32* numRecords,
                          ViReal64* refPos,
                          ViInt32* triggerType,
                          ViChar* triggerSource,
                          ViInt32* triggerSlope,
                          ViReal64* triggerLevel,
                          ViInt32* measurement)
{
   printf("Type the desired channel to acquire on (e.g. 0, 1) : ");
   scanf("%s", channel);

   *verticalRange = 10.0;
   *verticalCoupling = NISCOPE_VAL_DC;
   *probeAttenuation = 1.0;
   *inputImpedance = NISCOPE_VAL_1_MEG_OHM;
   *maxInputFrequency = 0.0;
   *minSampleRate = 10000000;
   *minRecordLength = 1000;
   *enforceRealTime = NISCOPE_VAL_TRUE;
   *numRecords = 1;
   *refPos = 50.0;
   *triggerType = 2; // Immediate for this example
   strcpy(triggerSource, "0");
   *triggerSlope = NISCOPE_VAL_POSITIVE;
   *triggerLevel = 0.0;
   *measurement = NISCOPE_VAL_FREQUENCY;

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
              ViInt32 actualRecordLength,
              ViReal64 *measurementResult,
              ViReal64 *mean,
              ViReal64 *stdev,
              ViReal64 *min,
              ViReal64 *max,
              ViInt32 *numInStats)
{
   if (wfmInfoPtr && waveformPtr)
   {
      int i;
      for (i=0; i<numWaveforms; i++)
      {
         asciiPlot(waveformPtr,wfmInfoPtr[i].actualSamples);
         waveformPtr = waveformPtr + wfmInfoPtr[i].actualSamples;
         printf ("Frequency %f : \n", measurementResult[i]);
      }
      printf("Actual Sample Rate : %.2f\n",actualSampleRate);
      printf("Actual Record Length : %d\n",actualRecordLength);
   }
   return 0;
}
/*************************************************************************************\

                              End of example

\*************************************************************************************/
