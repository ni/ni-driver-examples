#ifndef _GENERIC_ADVANCED_MEASUREMENT_LIBRARY
#define _GENERIC_ADVANCED_MEASUREMENT_LIBRARY

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


struct niScope_triggerAttr{
   ViChar triggerSource[MAX_STRING_SIZE];
   ViInt32 triggerCoupling;
   ViInt32 triggerSlope;
   ViReal64 triggerLevel;
   ViReal64 triggerHoldoff;
   ViReal64 triggerDelay;
   ViInt32 windowMode;
   ViReal64 lowWindowLevel;
   ViReal64 highWindowLevel;
   ViReal64 hysteresis;
};

// Forward declarations (functions defined in user interface file)
// Process Event to find out when to stop
extern int ProcessEvent (int *stop);

// Obtain the resource name of the device from the user interface
extern int GetResourceNameFromGUI (ViRsrc resourceName);

// Obtain the necessary parameters from the user interface
extern int GetParametersFromGUI (ViChar* channelName,
                                 ViInt32* processingStepMeasurement,
                                 ViInt32* scalarMeasurement,
                                 ViInt32* arrayMeasurement,
                                 ViInt32* filterType,
                                 ViReal64* cutoffFrequency,
                                 ViReal64* centerFrequency,
                                 ViReal64* bandpassWidth);

// Plot the waveforms and importat results in the user interface
extern int PlotWfms (ViInt32 numWaveforms,
                     ViInt32 arrayNumWaveforms,
                     ViReal64 *waveformPtr,
                     struct niScope_wfmInfo *wfmInfoPtr,
                     ViReal64 *arrayWaveformPtr,
                     struct niScope_wfmInfo *arrayWfmInfoPtr,
                     ViReal64 *measurementResult);

// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericAdvancedMeasurementLibrary(void);

/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_ADVANCED_MEASUREMENT_LIBRARY */
