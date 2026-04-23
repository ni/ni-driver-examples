#ifndef _GENERIC_SAVE_TO_FILE
#define _GENERIC_SAVE_TO_FILE

#if defined(__cplusplus) || defined(__cplusplus__)
extern "C" {
#endif

/********************************************************************************\

                  Include Files

\********************************************************************************/


#include <stdlib.h>
#include "niScope.h"
#define MAX_STRING_SIZE 50
#define MAX_FILE_PATH 255
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
                                 ViChar* filePath,
                                 ViBoolean* acquireOption);

// Plot the waveforms and importat results in the user interface
extern int PlotWfms (ViReal64 *wfm,
                     ViInt32 actualSamples);


// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericSaveToFile(void);

/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_SAVE_TO_FILE */
