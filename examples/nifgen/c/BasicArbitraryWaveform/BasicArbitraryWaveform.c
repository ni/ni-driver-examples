/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Arbitrary Waveform Example source file                                    */
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
#include "BasicArbitraryWaveform.h"
#include "niModInstCustCtrl.h"

#define WFM_SIZE 256

static int gui;
ViSession vi=VI_NULL;
ViStatus error = VI_SUCCESS;
ViReal64 sine[WFM_SIZE], square[WFM_SIZE], ramp[WFM_SIZE];

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
    if ((gui = LoadPanel (0, "BasicArbitraryWaveform.uir", GUI)) < 0)
        return -1;
    niModInstCVICust_NewCtrl (gui, GUI_RESOURCE, "nifgen");        
    DisplayPanel (gui);

    /*- Create some waveform data --------------------------------------------*/
    // Sine:
    for (i = 0; i < WFM_SIZE; i++)
        sine[i] = sin(((ViReal64)i/WFM_SIZE)*2*3.141596);

    // Square
    for (i = 0; i < WFM_SIZE; i++) {
        if (i < WFM_SIZE/2) square[i] = 1;
        else square[i] = -1;
    }
    
    // Ramp
    for (i = 0; i < WFM_SIZE; i++)
        ramp[i] = (ViReal64)i/WFM_SIZE*2 - 1;
    
    PlotY (gui, GUI_GRAPH, sine, WFM_SIZE, VAL_DOUBLE, VAL_FAT_LINE,
           VAL_EMPTY_SQUARE, VAL_SOLID, 1, VAL_CYAN);
    SetCtrlVal(gui, GUI_WAVEFORM, 0);
    
    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (gui, GUI_RESOURCE);    
    return 0;
}


/****************************************************************************\
  Loads in the waveform and starts generation
\****************************************************************************/
void wfmGenerate() { 
    ViChar Resource[256], Channel[256];
    ViReal64 SampleRate, Gain, dcOffset, ActualSampleRate;
    ViInt32 wfmHandle, wfm;
    
    /*- Get all the control values -------------------------------------------*/
    GetCtrlVal(gui, GUI_RESOURCE, Resource);
    GetCtrlVal(gui, GUI_CHANNEL, Channel);
    GetCtrlVal(gui, GUI_SAMPLE_RATE, &SampleRate);
    GetCtrlVal(gui, GUI_GAIN, &Gain);
    GetCtrlVal(gui, GUI_DC_OFFSET, &dcOffset);
    GetCtrlVal(gui, GUI_WAVEFORM, &wfm);

    checkErr(niFgen_init (Resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Prepare arb for sequence mode output --------------------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_ARB));

    /*- Create an arbitrary sequence ----------------------------------------*/
    if (wfm == 0) 
        checkErr(niFgen_CreateWaveformF64 (vi, Channel, WFM_SIZE, sine, &wfmHandle));
    else if (wfm == 1) 
        checkErr(niFgen_CreateWaveformF64 (vi, Channel, WFM_SIZE, square, &wfmHandle));
    else if (wfm == 2) 
        checkErr(niFgen_CreateWaveformF64 (vi, Channel, WFM_SIZE, ramp, &wfmHandle));

    /*- Select arb waveform to generate, configure gain and offset ----------*/
    checkErr(niFgen_ConfigureArbWaveform(vi, Channel, wfmHandle, Gain,
                                        dcOffset)); 

    checkErr(niFgen_ConfigureSampleRate(vi, SampleRate));

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
  Change the gain when the gain knob is changed  
\****************************************************************************/
int CVICALLBACK gain (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViReal64 Gain;
    switch (event)
        {
        case EVENT_VAL_CHANGED:
            if(vi != VI_NULL) {
                GetCtrlVal(gui, GUI_GAIN, &Gain);
                checkErr(niFgen_SetAttributeViReal64(vi, VI_NULL, NIFGEN_ATTR_ARB_GAIN, 
                                                Gain));
                SetCtrlAttribute (gui, GUI_DC_OFFSET, ATTR_MAX_VALUE, Gain/2);
                SetCtrlAttribute (gui, GUI_DC_OFFSET, ATTR_MIN_VALUE, -Gain/2);
Error:
                ErrorBox();
            }
            break;
        }
    return 0;
}

/****************************************************************************\
  Change the dc offset when the dc offset knob is changed  
\****************************************************************************/
int CVICALLBACK dc_offset (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViReal64 dcOffset;
    switch (event)
        {
        case EVENT_COMMIT:
            if(vi != VI_NULL) {
                GetCtrlVal(gui, GUI_DC_OFFSET, &dcOffset);
                checkErr(niFgen_SetAttributeViReal64 (vi, "", NIFGEN_ATTR_ARB_OFFSET, dcOffset));
Error:
                ErrorBox();
            }
            break;
        }
    return 0;
}

/****************************************************************************\
  Change the waveform when the waveform ring is changed  
\****************************************************************************/
int CVICALLBACK waveform (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViInt32 wfm;
    switch (event)
        {
        case EVENT_COMMIT:
            GetCtrlVal(gui, GUI_WAVEFORM, &wfm);
            DeleteGraphPlot (gui, GUI_GRAPH, -1, VAL_IMMEDIATE_DRAW);
            if (wfm == 0)     
                PlotY (gui, GUI_GRAPH, sine, WFM_SIZE, VAL_DOUBLE, VAL_FAT_LINE,
                VAL_EMPTY_SQUARE, VAL_SOLID, 1, VAL_CYAN);
            else if (wfm == 1)
                PlotY (gui, GUI_GRAPH, square, WFM_SIZE, VAL_DOUBLE, VAL_FAT_LINE,
                VAL_EMPTY_SQUARE, VAL_SOLID, 1, VAL_CYAN);
            else if (wfm == 2)
                PlotY (gui, GUI_GRAPH, ramp, WFM_SIZE, VAL_DOUBLE, VAL_FAT_LINE,
                VAL_EMPTY_SQUARE, VAL_SOLID, 1, VAL_CYAN);

            break;
        }
    return 0;
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

