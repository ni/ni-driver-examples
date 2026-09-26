/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Triggering Example source file                                            */
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
#include "Triggers.h"
#include "niModInstCustCtrl.h"

#define WFM_SIZE 256
#define SEQ_SIZE 3

static int gui;
ViSession vi=VI_NULL;
ViStatus error = VI_SUCCESS;
ViReal64 sine[WFM_SIZE], square[WFM_SIZE], ramp[WFM_SIZE], graph[WFM_SIZE*SEQ_SIZE];

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
    if ((gui = LoadPanel (0, "Triggers.uir", GUI)) < 0)
        return -1;
    niModInstCVICust_NewCtrl (gui, GUI_RESOURCE, "nifgen");        
    DisplayPanel (gui);

    /*- Create some waveform data --------------------------------------------*/
    // Sine:
    for (i = 0; i < WFM_SIZE; i++)
        sine[i] = sin(((ViReal64)i/WFM_SIZE)*2*3.141596);

    // Square
    for (i = 0; i < WFM_SIZE; i++) {
        if (i < WFM_SIZE/2) square[i] = .9;
        else square[i] = -.9;
    }
    
    // Ramp
    for (i = 0; i < WFM_SIZE; i++)
        ramp[i] = (ViReal64)i/WFM_SIZE - .5;
    
    // All in one buffer for graph
    memcpy (graph, sine, WFM_SIZE*sizeof(ViReal64));
    memcpy ((graph+WFM_SIZE), square, WFM_SIZE*sizeof(ViReal64));
    memcpy ((graph+2*WFM_SIZE), ramp, WFM_SIZE*sizeof(ViReal64));
    PlotY (gui, GUI_GRAPH, graph, WFM_SIZE*3, VAL_DOUBLE, VAL_FAT_LINE,
           VAL_EMPTY_SQUARE, VAL_SOLID, 1, VAL_CYAN);

    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (gui, GUI_RESOURCE);    
    return 0;
}


/****************************************************************************\
  Loads in the sequence and starts generation
\****************************************************************************/
void wfmSequence() { 
    ViChar Resource[256], Channel[256], TriggerSource[256];
    ViReal64 SampleRate, ActualSampleRate, Gain;
    ViInt32 TriggerMode, TriggerType;
    ViInt32 seqHandle;
    ViInt32 wfmHandles[SEQ_SIZE];
    ViInt32 loopCounts[SEQ_SIZE];
    
    /*- Get all the control values -------------------------------------------*/
    GetCtrlVal(gui, GUI_RESOURCE, Resource);
    GetCtrlVal(gui, GUI_CHANNEL, Channel);
    GetCtrlVal(gui, GUI_SAMPLE_RATE, &SampleRate);
    GetCtrlVal(gui, GUI_TRIG_MODE, &TriggerMode);
    GetCtrlVal(gui, GUI_TRIG_SRC, TriggerSource);
    GetCtrlVal(gui, GUI_TRIG_TYPE, &TriggerType);
    GetCtrlVal(gui, GUI_GAIN, &Gain);

    checkErr(niFgen_init (Resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Prepare arb for sequence mode output --------------------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_SEQ));

    /*- Create an arbitrary sequence ----------------------------------------*/
    checkErr(niFgen_CreateWaveformF64 (vi, Channel, WFM_SIZE, sine, &wfmHandles[0]));
    loopCounts[0] = 1;
    checkErr(niFgen_CreateWaveformF64 (vi, Channel, WFM_SIZE, square, &wfmHandles[1]));
    loopCounts[1] = 1;
    checkErr(niFgen_CreateWaveformF64 (vi, Channel, WFM_SIZE, ramp, &wfmHandles[2]));
    loopCounts[2] = 1;

    checkErr(niFgen_CreateArbSequence (vi, SEQ_SIZE, wfmHandles, loopCounts, &seqHandle));

    /*- Select arb sequence to generate, configure triggering ---------------*/
    checkErr(niFgen_ConfigureArbSequence(vi, Channel, seqHandle, Gain, 0)); 
    checkErr(niFgen_ConfigureSampleRate(vi, SampleRate));
    checkErr(niFgen_ConfigureTriggerMode(vi, Channel, TriggerMode));
    if (TriggerType == 0) {
    	checkErr(niFgen_DisableStartTrigger(vi));
    } else if (TriggerType == 1) {
    	checkErr(niFgen_ConfigureSoftwareEdgeStartTrigger(vi));
    } else if (TriggerType == 2) {
    	checkErr(niFgen_ConfigureDigitalEdgeStartTrigger(vi, TriggerSource, NIFGEN_VAL_RISING_EDGE));
    }

    /*- Generate the sequence -----------------------------------------------*/
    checkErr(niFgen_InitiateGeneration(vi));

    /*- Update the control values ------------------------------------------*/
    SetCtrlVal(gui, GUI_GENERATE, VI_TRUE);
    
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
                wfmSequence();
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

/****************************************************************************\
  Send a software trigger when the trigger button is pushed
\****************************************************************************/
int CVICALLBACK sw_trig (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViInt32 trig;
    switch (event)
        {
        case EVENT_COMMIT:
            if(vi != VI_NULL) {
                checkErr(niFgen_SendSoftwareEdgeTrigger (vi, NIFGEN_VAL_START_TRIGGER, ""));
Error:
                ErrorBox();
            }
            break;
        }
    return 0;
}
