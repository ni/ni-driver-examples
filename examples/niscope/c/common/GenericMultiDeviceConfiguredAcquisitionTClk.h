#ifndef _GENERIC_MULTI_DEVICE_CONFIGURED_ACQUISITION
#define _GENERIC_MULTI_DEVICE_CONFIGURED_ACQUISITION

#if defined(__cplusplus) || defined(__cplusplus__)
extern "C" {
#endif

/********************************************************************************\

                  Include Files

\********************************************************************************/


#include <stdlib.h>
#include "niScope.h"
#include <stdio.h>

/*********************************************************************************\

                  Function Prototype

\**********************************************************************************/


#define  MAX_STRING_SIZE 80
#define  MAX_NUM_DEVICES 18
typedef ViChar resourceNameType[MAX_STRING_SIZE];

// Forward declarations (functions defined in user interface file)
// Process Event to find out when to stop
extern int ProcessEvent (int *stop);

// Obtain the resource name of the device from the user interface
extern int GetResourceNamesFromGUI (resourceNameType resourceName);

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

// PlotWfms is persistant, so provide a way to clear the plots
extern int ClearPlots(void);

// PlotWfms does not necessarily draw the graph.  When all wfms are
// plotted, this function is called to actually commit the graph.
extern int CommitPlots(void);

// Display error message in user interface
extern int DisplayErrorMessageInGUI (ViInt32 error,
                                     ViConstString errorMessage);

ViStatus _VI_FUNC niScope_GenericMultiDeviceConfiguredAcquisitionTClk(void);


/********************************************************************************\

  Set of macros for handling errors from NI-SCOPE, NI-TClk, and this example.

\********************************************************************************/

#define handleNIScopeErr(fCall)  {                                                  \
         ViChar sessionNumber[4000];                                                \
         ViChar errorSource[4000];                                                  \
         ViStatus code = (fCall);                                                   \
         if ( (code < 0) || ((error == 0) && (code > 0)) ) {                        \
            error = code;                                                           \
            strncpy(errorSource, #fCall, MAX_FUNCTION_NAME_SIZE);                   \
            errorSource[MAX_FUNCTION_NAME_SIZE - 1] = '\0';                         \
            strcpy(errorSource, strtok( errorSource, " (") );                       \
            sprintf(sessionNumber, " for session with (zero-based) index %d", i);   \
            strcat(errorSource, sessionNumber);                                     \
            niScope_errorHandler(sessions[i], error, errorSource, errorMessage);    \
         }                                                                          \
         if (code < 0) goto Error;                                                  \
      }

#define handleNITClkErr(fCall)  {                                                   \
         ViStatus code = (fCall);                                                   \
         if ( (code < 0) || ((error == 0) && (code > 0)) ) {                        \
            error = code;                                                           \
            niTClk_GetExtendedErrorInfo(errorMessage, 4000);                        \
         }                                                                          \
         if (code < 0) goto Error;                                                  \
      }

#define handleSimpleExampleErr(fCall)  {                                            \
         ViStatus code = (fCall);                                                   \
         if ( (code < 0) || ((error == 0) && (code > 0)) ) {                        \
            error = code;                                                           \
         }                                                                          \
         if (code < 0) goto Error;                                                  \
      }

#define handleExampleErrWithText(text)  {                                           \
         error = -1;                                                                \
         strcpy(errorMessage, text);                                                \
         goto Error;                                                                \
      }



/***********************************************************************************\

                  End Include File

\***********************************************************************************/

#if defined(__cplusplus) || defined (__cplusplus__)
}
#endif

#endif /*_GENERIC_MULTI_DEVICE_CONFIGURE_ACQUISITION */
