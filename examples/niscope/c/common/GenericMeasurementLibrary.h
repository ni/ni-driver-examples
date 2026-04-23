#ifndef _GENERIC_MEASUREMENT_LIBRARY
#define _GENERIC_MEASUREMENT_LIBRARY

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
extern int GetParametersFromGUI (ViChar* channelName,
                                 ViReal64* verticalRange,
                                 ViInt32* verticalCoupling,
                                 ViReal64* minSampleRate,
                                 ViInt32* minRecordLength,
                                 ViInt32* refLevelUnits,
                                 ViReal64* chanBasedLowRef,
                                 ViReal64* chanBasedMidRef,
                                 ViReal64* chanBasedHighRef,
                                 ViInt32* scalarMeasurement,
                                 ViBoolean* clearStatistics,
                                 ViInt32* triggerType);

// Plot the waveforms and importat results in the user interface
extern int PlotWfms (ViInt32 numWaveforms,
                     ViReal64 *waveform,
                     struct niScope_wfmInfo *wfmInfoPtr,
                     ViReal64 *measurementResult,
                     ViReal64 *mean,
                     ViReal64 *stdev,
                     ViReal64 *min,
                     ViReal64 *max,
                     ViInt32 *numInStats);

// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericMeasurementLibrary(void);

/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_MEASUREMENT_LIBRARY */
