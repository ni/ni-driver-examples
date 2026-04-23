#ifndef _GENERIC_GETTING_STARTED
#define _GENERIC_GETTING_STARTED

#if defined(__cplusplus) || defined(__cplusplus__)
extern "C" {
#endif

/********************************************************************************\

                  Include Files

\********************************************************************************/


#include <stdlib.h>
#include "niScope.h"

/*********************************************************************************\

                  Function Prototype

\**********************************************************************************/

// Forward declarations (functions defined in user interface file)
// Obtain the resource name of the device from the user interface
extern int GetResourceNameFromGUI (ViRsrc resourceName);

// Obtain the necessary parameters from the user interface
extern int GetParametersFromGUI (ViChar* channel);

// Plot the waveforms and importat results in the user interface
extern int PlotWfms (ViReal64 *wfm,
                     struct niScope_wfmInfo *wfmInfoPtr,
                     ViInt32 actualRecordLength,
                     ViReal64 actualSampleRate);

// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericGettingStarted(void);

/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_GETTING_STARTED */


