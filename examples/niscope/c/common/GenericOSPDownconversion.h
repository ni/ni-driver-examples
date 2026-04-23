#ifndef _GENERIC_OSP_DOWNCONVERSION
#define _GENERIC_OSP_DOWNCONVERSION

#if defined(__cplusplus) || defined(__cplusplus__)
extern "C" {
#endif



#include <stdlib.h>
#include "niscope.h"
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
extern int GetParametersFromGUI (ViChar* channelName,
                             ViReal64* verticalRange,
                             ViInt32* inputImpedance,
                             ViReal64* minSampleRate,
                             ViInt32* minRecordLength,
                             ViReal64* timeout,
                             ViReal64 *digitalGain,
                             ViReal64 *phaseI,
                             ViReal64 *centerFrequency);


// Plot the waveforms and importat results in the user interface
extern int PlotWfms (ViInt32 numWaveforms,
                     ViReal64 *wfm,
                     struct niScope_wfmInfo *wfmInfoPtr,
                     ViReal64 *frequencyWaveformPtr,
                     struct niScope_wfmInfo *frequencyWfmInfoPtr,
                     ViReal64 actualSampleRate,
                     ViInt32 actualRecordLength);

// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericOSPDownconversion(void);

/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_OSP_DOWNCONVERSION */
