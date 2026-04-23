#ifndef _GENERIC_EXTERNAL_CLOCKING
#define _GENERIC_EXTERNAL_CLOCKING

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
// Process Event to find out when to stop
extern int ProcessEvent (int *stop);

// Obtain the resource name of the device from the user interface
extern int GetResourceNameFromGUI (ViRsrc resourceName);

// Obtain the necessary parameters from the user interface
extern int GetParametersFromGUI (ViChar* channel,
                                 ViReal64 *verticalRange,
                                 ViReal64 *verticalOffset,
                                 ViInt32 *minRecordLength,
                                 ViChar* timebaseSource,
                                 ViReal64* timebaseRate,
                                 ViInt32* timebaseDivisor,
                                 ViInt32* timebaseMultiplier);

// Plot the waveforms and importat results in the user interface
extern int PlotWfm (ViInt32 numWaveforms,
                    ViReal64 *scaledWfm,
                    struct niScope_wfmInfo *wfmInfoPtr,
                    ViReal64 actualSampleRate);

// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericExternalClocking(void);


/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_EXTERNAL_CLOCKING */
