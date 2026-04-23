/********************************************************************************
 * Video Triggering Example, uses GenericVideoTriggering.h generic file
 *******************************************************************************/

#include "asciiPlot.h"
#include <locale.h>
#include <stdio.h>
#include "GenericVideoTriggering.h"

int main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform an acquisition
   niScope_GenericVideoTriggering();
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
int GetParametersFromGUI  (ViChar* channel,
                           ViReal64* verticalRange,
                           ViInt32* verticalCoupling,
                           ViReal64* minSampleRate,
                           ViInt32* minRecordLength,
                           ViReal64 *refPos,
                           ViChar *triggerSource,
                           ViInt32 *triggerCoupling,
                           ViReal64 *triggerDelay,
                           ViInt32* signalFormat,
                           ViInt32* event,
                           ViInt32* lineNumber,
                           ViBoolean* enableDCRestore)
{

   strcpy(channel,"0");
   *verticalRange = 4.0;
   *verticalCoupling = NISCOPE_VAL_AC;  // the other option is NISCOPE_VAL_DC
   *minSampleRate = 100000000;
   *minRecordLength = 15000;
   *refPos = 50.0;
   *triggerDelay = 0.0;

   /* Trigger Coupling Options
      ------------------------
      NISCOPE_VAL_AC
      NISCOPE_VAL_DC
      NISCOPE_VAL_HF_REJECT
      NISCOPE_VAL_LF_REJECT
      NISCOPE_VAL_AC_PLUS_HF_REJECT */

   *triggerCoupling = NISCOPE_VAL_DC;
   strcpy(triggerSource,"0");            // trigger off of channel 0
   *enableDCRestore = NISCOPE_VAL_FALSE; // restores the zero reference of each video line back to zero

   /* Video Trigger Signal Formats
      ----------------------------
      NISCOPE_VAL_NTSC  - supports line numbers from 1 to 525
      NISCOPE_VAL_PAL   - supports line numbers from 1 to 625
      NISCOPE_VAL_SECAM - supports line numbers from 1 to 625 */

   *signalFormat = NISCOPE_VAL_NTSC;

   /* Video Trigger Event Options
      ---------------------------
      NISCOPE_VAL_TV_EVENT_FIELD1
      NISCOPE_VAL_TV_EVENT_FIELD2
      NISCOPE_VAL_TV_EVENT_ANY_FIELD
      NISCOPE_VAL_TV_EVENT_ANY_LINE
      NISCOPE_VAL_TV_EVENT_LINE_NUMBER */

   *event = NISCOPE_VAL_TV_EVENT_ANY_LINE;

   *lineNumber = 1; // only used if event is set to NISCOPE_VAL_TV_EVENT_LINE_NUMBER

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
