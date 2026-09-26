/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Clock Mode Example source file                                            */
/*                                                                           */
/* National Instruments, Austin Texas                                        */
/* PH. (800)433-3488   Fax (512)794-5678                                     */
/* Original Release: 4-99                                                    */
/*                                                                           */
/* Modification History:                                                     */
/*     Date    Initials  Description                                         */
/*     8-03    DC        Created                                             */
/*****************************************************************************/

#include <utility.h>
#include <ansi_c.h>
#include <cvirte.h>     /* Needed if linking in external compiler; harmless otherwise */
#include <userint.h>
#include <string.h>
#include "nifgen.h"
#include "ClockMode.h"
#include "niModInstCustCtrl.h"


#define WFM_SIZE 256

static int gui;
ViSession vi=VI_NULL;
ViStatus error = VI_SUCCESS;
ViReal64 sine[WFM_SIZE];

void ErrorBox(void);

/****************************************************************************\
  Function for displaying messages in the error box
\****************************************************************************/
void ErrorBox() {
    ViUInt32 errMsgSize;
    ViChar*  errMsg;
    if(error <0) {
        errMsgSize = niFgen_GetError(vi, VI_NULL, 0, VI_NULL);
        errMsg = (ViChar *) malloc(sizeof(ViChar) * errMsgSize);
        niFgen_GetError(vi, &error, errMsgSize, errMsg);
        ResetTextBox(gui, GUI_ERROR_MESSAGE, errMsg);
        free(errMsg);
    }
    else if(error == VI_SUCCESS) ResetTextBox(gui, GUI_ERROR_MESSAGE, "");
}

/****************************************************************************\
  Starts the interative pannel
\****************************************************************************/
int main (int argc, char *argv[])
{
    ViInt32 i;
    if (InitCVIRTE (0, argv, 0) == 0)   /* Needed if linking in external compiler; harmless otherwise */
        return -1;  /* out of memory */
    if ((gui = LoadPanel (0, "ClockMode.uir", GUI)) < 0)
        return -1;
    niModInstCVICust_NewCtrl (gui, GUI_RESOURCE, "nifgen");        
    DisplayPanel (gui);

    /*- Create waveform data --------------------------------------------*/
    // Sine:
    for (i = 0; i < WFM_SIZE; i++)
        sine[i] = sin(((ViReal64)i/WFM_SIZE)*2*3.141596);
    
    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (gui, GUI_RESOURCE);    
    return 0;
}


/****************************************************************************\
  Loads in the waveform and starts generation
\****************************************************************************/
void wfmGenerate() { 
    ViChar Resource[256], Channel[256];
    ViReal64 SampleRate, ActualSampleRate;
    ViInt32 wfmHandle, ClockMode;
    
    /*- Get all the control values -------------------------------------------*/
    GetCtrlVal(gui, GUI_RESOURCE, Resource);
    GetCtrlVal(gui, GUI_CHANNEL, Channel);
    GetCtrlVal(gui, GUI_SAMPLE_RATE, &SampleRate);
    GetCtrlVal(gui, GUI_CLOCK_MODE, &ClockMode);

    checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Prepare arb for sequence mode output --------------------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_ARB));

    /*- Create arbitrary waveform ----------------------------------------*/
    checkErr(niFgen_CreateWaveformF64 (vi, Channel, WFM_SIZE, sine, &wfmHandle));

    /*- Select arb waveform to generate, configure clock --------------------*/
    checkErr(niFgen_ConfigureArbWaveform(vi, Channel, wfmHandle, 1, 0)); 
    checkErr(niFgen_ConfigureSampleRate(vi, SampleRate));
    checkErr(niFgen_ConfigureClockMode(vi, ClockMode));

    /*- Generate the sequence -----------------------------------------------*/
    checkErr(niFgen_InitiateGeneration(vi));

    /*- Update the control values -------------------------------------------*/
    SetCtrlVal(gui, GUI_GENERATE, VI_TRUE);
    checkErr(niFgen_GetAttributeViInt32 (vi, "", NIFGEN_ATTR_CLOCK_MODE,
                                         &ClockMode));
    SetCtrlVal(gui, GUI_ACTUAL_CLOCK_MODE, ClockMode);
    checkErr(niFgen_GetAttributeViReal64 (vi, "",
                                          NIFGEN_ATTR_ACTUAL_ARB_SAMPLE_RATE,
                                          &SampleRate));
    SetCtrlVal(gui, GUI_ACTUAL_SAMPLE_RATE, SampleRate);                                         
    
Error:
    ErrorBox();
    if((error != VI_SUCCESS) && (vi != VI_NULL)) {
        niFgen_close(vi);
        vi = VI_NULL;
        SetCtrlVal(gui, GUI_GENERATE, VI_FALSE);
    }
}


/****************************************************************************\
  Close the session and exit the program when the stop button is pushed
\****************************************************************************/
int CVICALLBACK stop (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
        switch (event)
        {
        case EVENT_COMMIT:
            if(vi != VI_NULL) {
                niFgen_AbortGeneration(vi);
                niFgen_close(vi);
            }
            QuitUserInterface (0);
            break;
        }
    return 0;
}

/****************************************************************************\
  Start generation when the generate button is pushed
  Close the session when the generate button is released
\****************************************************************************/
int CVICALLBACK generate (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    int Generate;
    switch (event)
        {
        case EVENT_COMMIT:
            GetCtrlVal(gui, GUI_GENERATE, &Generate);
            if(Generate) {
                wfmGenerate();
            }
            else {
                if(vi != VI_NULL) {
                    checkErr(niFgen_AbortGeneration(vi));
Error:
                    ErrorBox();
                }
            }
            break;
        }
    return 0;
}

