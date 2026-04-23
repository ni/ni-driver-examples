#ifndef _GENERIC_VIDEO_TRIGGERING
#define _GENERIC_VIDEO_TRIGGERING

#if defined(__cplusplus) || defined(__cplusplus__)
extern "C" {
#endif

/********************************************************************************\

                  Include Files

\********************************************************************************/


#include <stdlib.h>
#include "niScope.h"
#define  MAX_STRING_SIZE 50

/*********************************************************************************\

                  Function Prototype

\**********************************************************************************/

// Forward declarations (functions defined in user interface file)
// Process Event to find out when to stop
extern int ProcessEvent (int *stop);

// Obtain the resource name of the device from the user interface
extern int GetResourceNameFromGUI (ViRsrc resourceName);

// Obtain the necessary parameters from the user interface
extern int GetParametersFromGUI (ViChar* channel,
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
                                 ViBoolean* enableDCRestore);

// Plot the waveforms and importat results in the user interface
extern int PlotWfms (ViInt32 numWaveforms,
                     ViReal64 *wfm,
                     struct niScope_wfmInfo *wfmInfoPtr,
                     ViReal64 actualSampleRate,
                     ViInt32 actualRecordLength);

// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericVideoTriggering(void);

/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_VIDEO_TRIGGERING */
