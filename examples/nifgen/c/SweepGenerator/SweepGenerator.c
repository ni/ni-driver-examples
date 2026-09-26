/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Sweep Generator Example source file                                       */
/*                                                                           */
/* National Instruments, Austin Texas                                        */
/* PH. (800)433-3488   Fax (512)794-5678                                     */
/* Original Release: 4-99                                                    */
/*                                                                           */
/* Modification History:                                                     */
/*     Date    Initials  Description                                         */
/*     4-99    BC        Created                                             */
/*****************************************************************************/

#include <ansi_c.h>
#include <cvirte.h>     /* Needed if linking in external compiler; harmless otherwise */
#include <userint.h>
#include <stdio.h>
#include <nifgen.h>
#include "SweepGenerator.h"
#include "niModInstCustCtrl.h"

static int gui;
ViSession vi = VI_NULL;
ViStatus error = VI_SUCCESS;
void generate(void);
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
    if (InitCVIRTE (0, argv, 0) == 0)   /* Needed if linking in external compiler; harmless otherwise */
        return -1;  /* out of memory */
    if ((gui = LoadPanel (0, "SweepGenerator.uir", GUI)) < 0)
        return -1;
    niModInstCVICust_NewCtrl (gui, GUI_RESOURCE, "nifgen");        
    DisplayPanel (gui);
    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (gui, GUI_RESOURCE);    
    return 0;
}

/****************************************************************************\
  This reads all control values and starts waveform generation
\****************************************************************************/
void generate(void) {
    ViChar Resource[256], Channel[256];
    ViReal64 StartFreq, EndFreq, Duration, Amplitude, DCOffset;
    ViReal64* frequencies = VI_NULL;
    ViReal64* durations = VI_NULL;
    ViReal64 stepsize, dur;
    ViInt32  wfmType, Steps, i, fListHandle;

    /*- Read all clontrol values -------------------------------------------*/
    GetCtrlVal(gui, GUI_RESOURCE, Resource);
    GetCtrlVal(gui, GUI_CHANNEL, Channel);
    GetCtrlVal(gui, GUI_WAVEFORM, &wfmType);
    GetCtrlVal(gui, GUI_START_FREQ, &StartFreq);
    GetCtrlVal(gui, GUI_END_FREQ, &EndFreq);
    GetCtrlVal(gui, GUI_NUM_STEPS, &Steps);
    GetCtrlVal(gui, GUI_DURATION, &Duration);
    GetCtrlVal(gui, GUI_AMPLITUDE, &Amplitude);
    GetCtrlVal(gui, GUI_DC_OFFSET, &DCOffset);
    
    /*- Generate a frequency list to sweep the given frequencies ------------*/
    frequencies = malloc (Steps * sizeof(ViReal64));
    durations = malloc (Steps * sizeof(ViReal64));
    stepsize = (EndFreq - StartFreq) / Steps;
    dur = Duration;
    for(i=0; i<Steps; i++) {
        frequencies[i] = StartFreq + i*stepsize;
        durations[i] = dur;
    }
    if(vi != VI_NULL) niFgen_close(vi);
    
    /*- Initialize the session ---------------------------------------------*/
    checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Configure output mode for frequency list ---------------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_FREQ_LIST));

    /*- Create the frequency list ------------------------------------------*/
    checkErr(niFgen_CreateFreqList(vi, wfmType, Steps, frequencies,
                                   durations, &fListHandle)); 

    /*- Select the frequency list to generate ------------------------------*/
    checkErr(niFgen_ConfigureFreqList(vi, Channel, fListHandle, Amplitude,
                                      DCOffset, 0));

    /*- Enable output ------------------------------------------------------*/
    checkErr(niFgen_ConfigureOutputEnabled(vi, Channel, VI_TRUE));

    /*- Generate the frequency list ----------------------------------------*/
    checkErr(niFgen_InitiateGeneration(vi));


Error:
    free(frequencies);
    free(durations);
    /*- Display any errors -------------------------------------------------*/
    ErrorBox();
    if((error < 0) && (vi != VI_NULL)) {
        niFgen_close(vi);
        vi = VI_NULL;
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
                vi = VI_NULL;
            }    
            QuitUserInterface (0);
            break;
        }
    return 0;
}

/****************************************************************************\
  Start generation when the run button is pushed
\****************************************************************************/
int CVICALLBACK run (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    int Generate;

    switch (event)
        {
        case EVENT_COMMIT:
            GetCtrlVal(gui, GUI_GENERATE, &Generate);
            if(Generate) {
                generate();
            }
            else {
                if(vi != VI_NULL) {
                    checkErr(niFgen_AbortGeneration(vi));
                    checkErr(niFgen_close(vi));
                    vi=VI_NULL;
Error:
                    ErrorBox();
            		/*- Update generate button---------------------------------------------*/
    				if((error < 0) && (vi != VI_NULL)) {
                		niFgen_close(vi);
                		vi = VI_NULL;
            		}
                }
            }            
            break;
        }
    return 0;
}
