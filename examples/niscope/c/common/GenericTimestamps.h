#ifndef _GENERIC_TIME_STAMPS
#define _GENERIC_TIME_STAMPS

#if defined(__cplusplus) || defined(__cplusplus__)
extern "C" {
#endif

/********************************************************************************\

                  Include Files

\********************************************************************************/


#include <stdlib.h>
#include "niScope.h"
#define MAX_STRING_SIZE 50
/*********************************************************************************\

                  Function Prototype

\**********************************************************************************/

// Forward declarations (functions defined in user interface file)
// Obtain the resource name of the device from the user interface
extern int GetResourceNameFromGUI (ViRsrc resourceName);

// Obtain the necessary parameters from the user interface
extern int GetParametersFromGUI (ViChar* channelName,
                                 ViReal64* minSampleRate,
                                 ViInt32* numRecords,
                                 ViInt32* triggerType,
                                 ViChar* triggerSource,
                                 ViReal64* triggerHoldoff,
                                 ViReal64* verticalRange);

// Plot the waveforms and importat results in the user interface
extern int PlotWfms (ViReal64* xValues,
                     ViInt32* yValues,
                     ViInt32 numBins,
                     ViReal64 mean,
                     ViReal64 stdev,
                     ViReal64 min,
                     ViReal64 max);


// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericTimestamps(void);

/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_TIME_STAMPS */
