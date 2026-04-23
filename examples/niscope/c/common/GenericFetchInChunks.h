#ifndef _GENERIC_FETCH_IN_CHUNKS
#define _GENERIC_FETCH_IN_CHUNKS

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
// Obtain the resource name of the device from the user interface
extern int GetResourceNameFromGUI (ViRsrc resourceName);

// Obtain the necessary parameters from the user interface
extern int GetParametersFromGUI (ViChar* channel,
                                 ViReal64* verticalRange,
                                 ViReal64* minSampleRate,
                                 ViInt32* minRecordLength,
                                 ViInt32* triggerType,
                                 ViInt32* maxFetchSize);

// Plot the waveforms and importat results in the user interface
extern int PlotWfms (ViInt32 numPointsFetched,
                     ViInt32 chunksFetched,
                     ViReal64 *wfm,
                     struct niScope_wfmInfo *wfmInfoPtr);

// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericFetchInChunks(void);

/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_FETCH_IN_CHUNKS */
