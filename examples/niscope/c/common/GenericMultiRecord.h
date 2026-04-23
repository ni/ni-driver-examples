#ifndef _GENERIC_MULTI_RECORD
#define _GENERIC_MULTI_RECORD

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
                         ViReal64* minSampleRate,
                         ViInt32* minRecordLength,
                         ViInt32* numRecords);

// Plot the waveforms and importat results in the user interface
extern int PlotWfms (ViInt32 numWaveforms,
                     ViReal64 *wfm,
                     struct niScope_wfmInfo *wfmInfoPtr,
                ViReal64 actualSampleRate,
                ViInt32 actualRecordLength);


// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericMultiRecord(void);

/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_MULTI_RECORD */
