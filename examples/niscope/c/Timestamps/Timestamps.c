/********************************************************************************
 * Time Stamps Example, uses GenericTimestamps.h generic file
 *******************************************************************************/

#include "asciiPlot.h"
#include <locale.h>
#include <stdio.h>
#include "GenericTimestamps.h"

#define MAX_STRING_SIZE 50

int main()
{
   setlocale(LC_ALL, "");
   // Call the generic function to perform a Time Stamps acquisition
   niScope_GenericTimestamps();
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
                          ViReal64* minSampleRate,
                          ViInt32* numRecords,
                          ViInt32* triggerType,
                          ViChar* triggerSource,
                          ViReal64* triggerHoldoff,
                          ViReal64* verticalRange)
{
   float rate;

   strcpy(channelName,"0");

   // Get the sample rate from the user
   printf("Type sample rate: ");
   scanf("%f", &rate);
   *minSampleRate = rate;

   // Get the number of records from the user
   printf("Type number of records: ");
   scanf("%d", numRecords);

   *triggerType = 0;
   strcpy(triggerSource,"0");
   *triggerHoldoff = 0.0;
   *verticalRange = 10.0;
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
int PlotWfms (ViReal64* xValues,
              ViInt32* yValues,
              ViInt32 numBins,
              ViReal64 mean,
              ViReal64 stdev,
              ViReal64 min,
              ViReal64 max)
{
   printf("\n\n");
   printf("Mean time between triggers (S): %.4e\n",mean);
   printf("Std. deviation time between triggers (S): %.4e\n",stdev);
   printf("Mean frequency of triggers (Hz): %.4e\n",1/mean);
   return 0;
}

/*************************************************************************************\

                              End of example

\*************************************************************************************/
