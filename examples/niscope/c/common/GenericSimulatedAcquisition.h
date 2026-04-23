#ifndef _GENERIC_SIMULATED_ACQUISITION
#define _GENERIC_SIMULATED_ACQUISITION

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

// Obtain the resource name and simulation model from the user interface
extern int GetResourceNameFromGUI (ViRsrc resourceName,
                                   ViChar* model,
                                   ViChar* type,
                                   ViReal64* verticalNoise);

// Obtain the necessary parameters from the user interface
extern int GetParametersFromGUI (ViChar* channel,
                                 ViReal64* verticalRange,
                                 ViInt32* verticalCoupling,
                                 ViReal64* probeAttenuation,
                                 ViReal64* inputImpedance,
                                 ViReal64* maxInputFrequency,
                                 ViReal64* minSampleRate,
                                 ViInt32* minRecordLength,
                                 ViBoolean* enforceRealTime,
                                 ViInt32* numRecords,
                                 ViReal64* refPos,
                                 ViInt32* triggerType,
                                 ViChar* triggerSource,
                                 ViInt32* triggerSlope,
                                 ViReal64* triggerLevel,
                                 ViInt32* measurement);

// Plot the waveforms and importat results in the user interface
extern int PlotWfms (ViInt32 numWaveforms,
                     ViReal64 *waveformPtr,
                     struct niScope_wfmInfo *wfmInfoPtr,
                     ViReal64 actualSampleRate,
                     ViInt32 actualRecordLength,
                     ViReal64 *measurementResult,
                     ViReal64 *mean,
                     ViReal64 *stdev,
                     ViReal64 *min,
                     ViReal64 *max,
                     ViInt32 *numInStats);

// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericSimulatedAcquisition(void);

/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_SIMULATED_ACQUISITION */
