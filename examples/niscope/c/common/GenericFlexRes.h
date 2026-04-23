#ifndef _GENERIC_FLEX_RES
#define _GENERIC_FLEX_RES

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
extern int GetParametersFromGUI (ViReal64* verticalRange,
                                 ViReal64* minSampleRate,
                                 ViInt32* minRecordLength,
                                 ViInt32* triggerType);

// Plot the waveforms and importat results in the user interface
extern int PlotWfms (ViReal64 *waveform,
                    struct niScope_wfmInfo *wfmInfoPtr,
                    ViReal64 *freqWaveform,
                    struct niScope_wfmInfo *freqWfmInfoPtr,
                    ViReal64 actualSampleRate,
                    ViInt32 resolution);

// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericFlexRes(void);

/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_FLEX_RES */
