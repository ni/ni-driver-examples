#ifndef _GENERIC_FETCH_FOREVER
#define _GENERIC_FETCH_FOREVER

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

#define MAX_STRING_SIZE 50

// Forward declarations (functions defined in user interface file)
// Process event to find out when to stop
extern int ProcessEvent (int *stop);

// Obtain the resource name of the device from the user interface
extern int GetResourceNameFromGUI (ViRsrc resourceName);

// Obtain the necessary parameters from the user interface
extern int GetParametersFromGUI (ViChar* channel,
                                 ViReal64* verticalRange,
                                 ViReal64* minSampleRate,
                                 ViInt32* maxPointsFetched);

// Plot the waveforms and importat results in the user interface
extern int PlotWfms (ViReal64 *wfm,
                     struct niScope_wfmInfo *wfmInfoPtr,
                     ViInt32 totalPointsFetched);

// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericFetchForever(void);

/***********************************************************************************\

                        End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_FETCH_FOREVER */
