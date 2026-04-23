#ifndef _GENERIC_BINARY_ACQUISITION
#define _GENERIC_BINARY_ACQUISITION

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
extern int GetParametersFromGUI (ViInt32 *terminalConfiguration,
                                 ViReal64 *verticalRange,
                                 ViReal64 *probeAttenuation,
                                 ViInt32 *verticalCoupling,
                                 ViReal64 *inputImpedance,
                                 ViReal64 *minSampleRate,
                                 ViInt32 *minRecordLength,
                                 ViInt32 *triggerType,
                                 ViReal64 *triggerLevel,
                                 ViInt32 *triggerSlope);

// Plot the waveforms and importat results in the user interface
extern int PlotWfm (ViInt32 numWaveforms,
                    ViReal64 *scaledWfm,
                    struct niScope_wfmInfo *wfmInfoPtr);

// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericDifferential(void);


/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_BINARY_ACQUISITION */
