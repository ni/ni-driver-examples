/********************************************************************************
 * Multi Device Configured Acquisition Example, uses GenericMultiDeviceConfiguredAcquisitionTClk.h generic file
 *******************************************************************************/

#include "asciiPlot.h"
#include <locale.h>
#include <stdio.h>
#include "GenericMultiDeviceConfiguredAcquisitionTClk.h"

int main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform an acquisition
   niScope_GenericMultiDeviceConfiguredAcquisitionTClk();
   // Wait and exit
   printf("Press <RETURN> to exit.\n");
   getc(stdin); getc(stdin);
   return 0;
}

// Obtain the resource name of the device from the user
int GetResourceNamesFromGUI (ViRsrc resourceName)
{
   //Get the device name from the user
   printf("Type the device names (e.g., Dev1,Dev2, ...): ");
   scanf("%s", resourceName);

   return 0;
}

// Obtain the necessary parameters
int GetParametersFromGUI (ViChar* channel,
                          ViInt32* acquisitionType,
                          ViReal64* verticalRange,
                          ViReal64* verticalOffset,
                          ViInt32* verticalCoupling,
                          ViReal64* probeAttenuation,
                          ViReal64* inputImpedance,
                          ViReal64* maxInputFrequency,
                          ViReal64* minSampleRate,
                          ViInt32* minRecordLength,
                          ViReal64* timeout,
                          ViInt32 *numRecords,
                          ViReal64 *refPos,
                          ViInt32* triggerType,
                          ViChar *triggerSource,
                          ViInt32 *triggerCoupling,
                          ViInt32 *triggerSlope,
                          ViReal64 *triggerLevel,
                          ViReal64 *triggerHoldoff,
                          ViReal64 *triggerDelay,
                          ViInt32 *windowMode,
                          ViReal64 *lowWindowLevel,
                          ViReal64 *highWindowLevel,
                          ViReal64 *hysteresis)
{
   ViInt32 triggerOption;

   strcpy(channel,"0");
   *acquisitionType = NISCOPE_VAL_NORMAL;
   *verticalRange = 10.0;
   *verticalOffset = 0.0;
   *verticalCoupling = NISCOPE_VAL_DC;
   *probeAttenuation = 1.0;
   *inputImpedance = NISCOPE_VAL_1_MEG_OHM; //the other option for input impedance is 50 or 75 ohm
   *maxInputFrequency = 0.0;
   *minSampleRate = 100000000;
   *minRecordLength = 1000;
   //   *enforceRealTime = NISCOPE_VAL_TRUE;
   *timeout = 5.000;
   *numRecords = 1;
   *refPos = 50.0;


   // Get the trigger type from the user
   printf("Type the trigger type\n");
   printf("1 - Edge\n");
   printf("2 - Hysteresis\n");
   printf("3 - Digital\n");
   printf("4 - Window\n");
   printf("5 - Immediate\n");
   printf("Option: ");
   scanf("%d", &triggerOption);
   printf("\n");

   *triggerHoldoff = 0.0;
   *triggerDelay = 0.0;

   //Assign the default values for the trigger attribute that depends on the trigger type
   switch(triggerOption)
   {
      case (1): //NISCOPE_VAL_EDGE:
         *triggerType = 0;
         strcpy(triggerSource,"0");
         *triggerLevel = 0.0;
         *triggerSlope = NISCOPE_VAL_POSITIVE;
         *triggerCoupling = NISCOPE_VAL_DC;
         break;
      case (2): //NISCOPE_VAL_HYSTERESIS:
         *triggerType = 1;
         strcpy(triggerSource,"0");
         *triggerLevel = 0.0;
         *hysteresis = 0.1;
         *triggerSlope = NISCOPE_VAL_POSITIVE;
         *triggerCoupling = NISCOPE_VAL_DC;
         break;
      case (3): //NISCOPE_VAL_DIGITAL:
         *triggerType = 2;
         strcpy(triggerSource,"VAL_PFI_1");
         *triggerSlope = NISCOPE_VAL_POSITIVE;
         break;
      case (4): //NISCOPE_VAL_WINDOW:
         *triggerType = 3;
         strcpy(triggerSource,"0");
         *lowWindowLevel = -0.1;
         *highWindowLevel = 0.1;
         *windowMode = NISCOPE_VAL_ENTERING_WINDOW; //Other option is leaving window
         *triggerCoupling = NISCOPE_VAL_DC;
         break;
      case (5): //IMMEDIATE:
         *triggerType = 4;
         break;
      default: //assign the edge trigger as the default value
         *triggerType = 0;
         strcpy(triggerSource,"0");
         *triggerLevel = 0.0;
         *triggerSlope = NISCOPE_VAL_POSITIVE;
         *triggerCoupling = NISCOPE_VAL_DC;
         break;
   }

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

int ClearPlots (void)
{ return 0; }

int CommitPlots(void)
{ return 0; }


/*************************************************************************************\

                              End of example

\*************************************************************************************/
