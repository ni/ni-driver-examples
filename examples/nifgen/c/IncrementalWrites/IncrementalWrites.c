/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Incremental Writes Example source file                                    */
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
#include "IncrementalWrites.h"
#include "niModInstCustCtrl.h"

static int gui;
ViSession vi=VI_NULL;
ViStatus error = VI_SUCCESS;

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
    if ((gui = LoadPanel (0, "IncrementalWrites.uir", GUI)) < 0)
        return -1;
    niModInstCVICust_NewCtrl (gui, GUI_RESOURCE, "nifgen");
    DisplayPanel (gui);

    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (gui, GUI_RESOURCE);    
    return 0;
}


/****************************************************************************\
  Loads in the waveform and starts generation
\****************************************************************************/
void wfmGenerate() { 
    ViChar Resource[256], Channel[256];
    ViReal64 SampleRate;
    ViInt32 wfmHandle, WaveformType, WaveformSize, NumWrites, i;
    ViReal64 *WaveformBuffer;
    ViInt32 Reverse;
    
    /*- Get all the control values -------------------------------------------*/
    GetCtrlVal(gui, GUI_RESOURCE, Resource);
    GetCtrlVal(gui, GUI_CHANNEL, Channel);
    GetCtrlVal(gui, GUI_SAMPLE_RATE, &SampleRate);
    GetCtrlVal(gui, GUI_WAVEFORM, &WaveformType);
    GetCtrlVal(gui, GUI_WAVEFORM_SIZE, &WaveformSize);
    GetCtrlVal(gui, GUI_NUM_CHUNKS, &NumWrites);
    GetCtrlVal(gui, GUI_REVERSE, &Reverse);
    WaveformBuffer = malloc(WaveformSize*sizeof(ViReal64));

    checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Prepare arb for sequence mode output --------------------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_ARB));

    /*- Create waveform data ------------------------------------------------*/
    if (WaveformType == 0) // Sine
        for (i = 0; i < WaveformSize; i++)
            WaveformBuffer[i] = sin(((ViReal64)i/WaveformSize)*2*3.141596);
    else if (WaveformType == 1) // Square
        for (i = 0; i < WaveformSize; i++) {
            if (i < WaveformSize/2) WaveformBuffer[i] = 1;
            else WaveformBuffer[i] = -1;
        }
    else if (WaveformType == 2) // Ramp
        for (i = 0; i < WaveformSize; i++)
            WaveformBuffer[i] = (ViReal64)i/WaveformSize*2 - 1;
    
    /*- Write waveform in chunks --------------------------------------------*/
    checkErr(niFgen_AllocateWaveform (vi, Channel, WaveformSize, &wfmHandle));
    for (i = 0; i < NumWrites; i++) {
        if (Reverse) 
            checkErr(niFgen_SetWaveformNextWritePosition (vi, Channel, wfmHandle,
                                                          NIFGEN_VAL_WAVEFORM_POSITION_START,
                                                          WaveformSize-(ViReal64)(i+1)/NumWrites*WaveformSize));
        checkErr(niFgen_WriteWaveform (vi, Channel, wfmHandle, WaveformSize/NumWrites,
                                       &WaveformBuffer[i*WaveformSize/NumWrites]));
    }

    /*- Select arb waveform to generate, configure sample rate --------------*/
    checkErr(niFgen_ConfigureArbWaveform(vi, Channel, wfmHandle, 1, 0)); 
    checkErr(niFgen_ConfigureSampleRate(vi, SampleRate));

    /*- Generate the sequence -----------------------------------------------*/
    checkErr(niFgen_InitiateGeneration(vi));

    /*- Update the control values -------------------------------------------*/
    SetCtrlVal(gui, GUI_GENERATE, VI_TRUE);
    
Error:
    free(WaveformBuffer);
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

