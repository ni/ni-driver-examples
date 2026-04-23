#ifndef _GENERIC_CONFIGURED_ACQUISITION
#define _GENERIC_CONFIGURED_ACQUISITION

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

// Obtain the resource name of the device from the user interface
extern int GetResourceNameFromGUI (ViRsrc resourceName);

// Obtain the necessary parameters from the user interface
extern int GetParametersFromGUI (ViChar* channel,
                             ViInt32* acquisitionType,
                             ViReal64* verticalRange,
                             ViReal64* verticalOffset,
                             ViInt32* verticalCoupling,
                             ViReal64* probeAttenuation,
                             ViReal64* inputImpedance,
                             ViReal64* maxInputFrequency,
                             ViReal64* minSampleRate,
                             ViInt32* minRecordLength,
                             ViBoolean* enforceRealTime,
                             ViBoolean* enableTIS,
                             ViReal64* timeout,
                             ViInt32 *numRecords,
                             ViReal64 *refPos,
                             ViInt32* triggerType,
                             ViChar *triggerSource,
                             ViInt32 *triggerCoupling,
                             ViInt32 *triggerSlope,
                             ViReal64 *triggerLevel,
                             ViReal64 *triggerHoldoff,
                             ViReal64 *triggerDelay,
                             ViInt32 *windowMode,
                             ViReal64 *lowWindowLevel,
                             ViReal64 *highWindowLevel,
                             ViReal64 *hysteresis);

// Plot the waveforms and importat results in the user interface
extern int PlotWfms (ViInt32 numWaveforms,
                     ViReal64 *wfm,
                     struct niScope_wfmInfo *wfmInfoPtr,
                     ViReal64 actualSampleRate,
                     ViInt32 actualRecordLength);

// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericConfiguredAcquisition(void);

/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_CONFIGURE_ACQUISITION */
