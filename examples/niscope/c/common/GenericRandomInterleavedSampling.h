#ifndef _GENERIC_RANDOM_INTERLEAVED_SAMPLING
#define _GENERIC_RANDOM_INTERLEAVED_SAMPLING

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
                         ViReal64* verticalRange,
                         ViReal64* minRISRate,
                         ViInt32* minRecordLength,
                         ViInt32* numAverages,
                         ViInt32* risMethod);

// Plot the waveforms and importat results in the user interface
extern int PlotWfms (ViReal64 *waveformPtr,
                struct niScope_wfmInfo *wfmInfoPtr,
                ViReal64 *risWaveformPtr,
                struct niScope_wfmInfo *risWfmInfoPtr,
                ViReal64 actualSampleRate,
                ViReal64 risActualSampleRate,
                ViInt32 oversamplingFactor);

// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericRandomInterleavedSampling(void);

/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_RANDOM_INTERLEAVED_SAMPLING */
