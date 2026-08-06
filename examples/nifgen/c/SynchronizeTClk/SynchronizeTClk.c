/*****************************************************************************/
/* National Instruments T Clock Synchronization                              */
/* T Clock Synchronization Example source file                               */
/*                                                                           */
/* National Instruments, Austin Texas                                        */
/* PH. (800)433-3488   Fax (512)794-5678                                     */
/* Original Release: 10/03                                                   */
/*                                                                           */
/* Modification History:                                                     */
/*     Date    Initials  Description                                         */
/*     10-03   DC        Created                                             */
/*     4-17    BA        Rename index variable                               */
/*****************************************************************************/
#include <utility.h>
#include <ansi_c.h>
#include <cvirte.h>     /* Needed if linking in external compiler; harmless otherwise */
#include <userint.h>
#include <stdio.h>
#include "nifgen.h"
#include "niTClk.h"
#include "SynchronizeTClk.h"

#define WFM_SIZE 64

static int gui;
ViSession* vi = VI_NULL;
ViReal64   wfmData[WFM_SIZE];
ViInt32    numResources = 0;
ViInt32    activeFgenSessionIndex = 0;
ViStatus   error = VI_SUCCESS;
void ErrorBox(void);

/****************************************************************************\
  Function for displaying messages in the error box
\****************************************************************************/
void ErrorBox() {
    ViUInt32 errMsgSize;
    ViChar*  errMsg;
    if(error!=VI_SUCCESS) {
        if (activeFgenSessionIndex >= numResources) {
            // NI-TCLK error
            errMsgSize = 4096;      // NI-TCLK errors can be lengthy
            errMsg = (ViChar *) malloc(sizeof(ViChar) * errMsgSize);
            niTClk_GetExtendedErrorInfo(errMsg, errMsgSize);
        } else {
            // NI-FGEN error
            errMsgSize = niFgen_GetError(vi[activeFgenSessionIndex], VI_NULL, 0, VI_NULL);
            errMsg = (ViChar *) malloc(sizeof(ViChar) * errMsgSize);
            niFgen_GetError(vi[activeFgenSessionIndex], &error, errMsgSize, errMsg);
        }
        MessagePopup("Error", errMsg);
        free(errMsg);
        error = VI_SUCCESS;
    }
}

/****************************************************************************\
  Starts the interative pannel
\****************************************************************************/
int main (int argc, char *argv[])
{
    ViInt32 i;

    if (InitCVIRTE (0, argv, 0) == 0)   /* Needed if linking in external compiler; harmless otherwise */
        return -1;  /* out of memory */
    if ((gui = LoadPanel (0, "SynchronizeTClk.uir", GUI)) < 0)
        return -1;

    /*- Create waveform data -----------------------------------------------*/
    for (i = 0; i < WFM_SIZE; i++)
        wfmData[i] = 0.0;
    wfmData[0] = 1.0;

    DisplayPanel (gui);
    RunUserInterface ();
    return 0;
}

/****************************************************************************\
  This reads all control values and starts waveform generation
\****************************************************************************/
int CVICALLBACK Go(int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViChar ResourceInput[4096];
    ViChar* currentResource = VI_NULL;
    ViInt32 wfmHandle;
    ViReal64 sampleRate;
    int Go;
    switch (event)
        {
        case EVENT_COMMIT:
            GetCtrlVal(gui, GUI_GO, &Go);
            if(Go) {

                /*- Read in all of the control values ----------------------------------*/
                GetCtrlVal(gui, GUI_RESOURCES, ResourceInput);
                GetCtrlVal(gui, GUI_SAMPLE_RATE, &sampleRate);

               /*- Parse the resource input string -------------------------------------*/
                numResources = 1;
                for (activeFgenSessionIndex = 0; activeFgenSessionIndex < strlen(ResourceInput); activeFgenSessionIndex++)
                    if (ResourceInput[activeFgenSessionIndex] == ',') numResources++;
                currentResource = strtok(ResourceInput, ",");
                vi = (ViSession *) malloc(numResources * sizeof(ViSession));

                for (activeFgenSessionIndex = 0; activeFgenSessionIndex < numResources; activeFgenSessionIndex++)
                {
                    /*- Initialize the sessions ----------------------------------------*/
                    checkErr(niFgen_init(currentResource, VI_TRUE, VI_TRUE, &vi[activeFgenSessionIndex]));

                    /*- Configure the active channels for the session ----------------------*/
                    checkErr(niFgen_ConfigureChannels(vi[activeFgenSessionIndex], "0"));

                    /*- Set the sample rate --------------------------------------------*/
                    checkErr(niFgen_ConfigureSampleRate(vi[activeFgenSessionIndex], sampleRate));

                    /*- Create and download the waveform -------------------------------*/
                    checkErr(niFgen_CreateWaveformF64(vi[activeFgenSessionIndex], "0" /*channel*/,
                       WFM_SIZE, wfmData, &wfmHandle));

                    currentResource = strtok(NULL, ",");
                }

                /*- Configure for homogeneous triggers for niTClk ----------------------*/
                checkErr(niTClk_ConfigureForHomogeneousTriggers(numResources, vi));

                /*- Synchronize arbs and start generation ------------------------------*/
                checkErr(niTClk_Synchronize(numResources, vi, 0));
                checkErr(niTClk_Initiate(numResources, vi));

            } else {
                if (vi != VI_NULL) {
                    for (activeFgenSessionIndex = 0; activeFgenSessionIndex < numResources; activeFgenSessionIndex++)
                        if (vi[activeFgenSessionIndex] != VI_NULL) niFgen_close (vi[activeFgenSessionIndex]);
                    free(vi);
                    vi = VI_NULL;
                }
            }
         break;
    }

Error:
    /*- Display any errors -------------------------------------------------*/
    if(error < 0) {
        ErrorBox();
        if (vi != VI_NULL) {
            for (activeFgenSessionIndex = 0; activeFgenSessionIndex < numResources; activeFgenSessionIndex++)
                if (vi[activeFgenSessionIndex] != VI_NULL) niFgen_close (vi[activeFgenSessionIndex]);
            free(vi);
            vi = VI_NULL;
        }
    }
    return error;
}


/****************************************************************************\
  Close the session and exit the program when the stop button is pushed
\****************************************************************************/
int CVICALLBACK Quit (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    switch (event)
        {
        case EVENT_COMMIT:
            if (vi != VI_NULL) {
               for (activeFgenSessionIndex = 0; activeFgenSessionIndex < numResources; activeFgenSessionIndex++)
                  if (vi[activeFgenSessionIndex] != VI_NULL) niFgen_close (vi[activeFgenSessionIndex]);
               free(vi);
               vi = VI_NULL;
            }
            QuitUserInterface (0);
            break;
        }
    return 0;
}


