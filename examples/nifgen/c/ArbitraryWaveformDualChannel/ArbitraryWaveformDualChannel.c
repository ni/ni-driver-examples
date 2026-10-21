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
/*     4-17    BA        Write Waveforms Interleaved                         */
/*****************************************************************************/

#include <utility.h>
#include <ansi_c.h>
#include <cvirte.h>     /* Needed if linking in external compiler; harmless otherwise */
#include <userint.h>
#include <string.h>
#include "nifgen.h"
#include "ArbitraryWaveformDualChannel.h"
#include "niModInstCustCtrl.h"

#define WFM_SIZE 256

static int gui;
ViSession vi=VI_NULL;
ViStatus error = VI_SUCCESS;
ViReal64 sine[WFM_SIZE], square[WFM_SIZE], ramp[WFM_SIZE];
ViReal64 interleavedWfm[WFM_SIZE * 2];

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
    if ((gui = LoadPanel (0, "ArbitraryWaveformDualChannel.uir", GUI)) < 0)
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


    // Plot the default waveforms for the graph
    PlotY (gui, GUI_GRAPH, sine, WFM_SIZE, VAL_DOUBLE, VAL_FAT_LINE,
           VAL_EMPTY_SQUARE, VAL_SOLID, 1, VAL_CYAN);
    PlotY (gui, GUI_GRAPH, square, WFM_SIZE, VAL_DOUBLE, VAL_FAT_LINE,
        VAL_EMPTY_SQUARE, VAL_SOLID, 1, VAL_BLUE);
    SetCtrlVal(gui, GUI_WFM_CH0, 0);
    SetCtrlVal(gui, GUI_WFM_CH1, 1);

    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (gui, GUI_RESOURCE);
    return 0;
}


/****************************************************************************\
  Loads in the waveform and starts generation
\****************************************************************************/
void wfmGenerate() {
    ViChar Resource[256];
    ViReal64 SampleRate, Gain, ActualSampleRate;
    ViInt32 wfmHandle, wfm0, wfm1;
    ViReal64 *wfmData0 = NULL, *wfmData1 = NULL;
    ViInt32 i;

    /*- Get all the control values -------------------------------------------*/
    GetCtrlVal(gui, GUI_RESOURCE, Resource);
    GetCtrlVal(gui, GUI_SAMPLE_RATE, &SampleRate);
    GetCtrlVal(gui, GUI_GAIN, &Gain);
    GetCtrlVal(gui, GUI_WFM_CH0, &wfm0);
    GetCtrlVal(gui, GUI_WFM_CH1, &wfm1);

    checkErr(niFgen_init (Resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0,1"));

    /*- Prepare arb for sequence mode output --------------------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_ARB));

    /*- Allocate the waveform buffer ----------------------------------------*/
    checkErr(niFgen_AllocateWaveform(vi, VI_NULL, WFM_SIZE, &wfmHandle));


    /*- Interleave the waveforms ------------------------------------------------*/
    if (wfm0 == 0)
        wfmData0 = sine;
    else if (wfm0 == 1)
        wfmData0 = square;
    else if (wfm0 == 2)
        wfmData0 = ramp;
    else
        checkErr(IVI_ERROR_VALUE_NOT_SUPPORTED);

    if (wfm1 == 0)
        wfmData1 = sine;
    else if (wfm1 == 1)
        wfmData1 = square;
    else if (wfm1 == 2)
        wfmData1 = ramp;
    else
        checkErr(IVI_ERROR_VALUE_NOT_SUPPORTED);

    for (i = 0; i < WFM_SIZE; i++)
    {
        interleavedWfm[i * 2] = wfmData0[i];
        interleavedWfm[i * 2 + 1] = wfmData1[i];
    }

    /*- Write the waveforms ------------------------------------------------*/
    checkErr(niFgen_WriteWaveform(vi, "0,1", wfmHandle, WFM_SIZE * 2, interleavedWfm));

    /*- Select arb waveform to generate, configure gain and offset ----------*/
    checkErr(niFgen_ConfigureArbWaveform(vi, "0,1", wfmHandle, Gain,
                                        0.0));

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
    ViInt32 wfm0, wfm1;

    switch (event)
        {
        case EVENT_COMMIT:


            GetCtrlVal(panel, GUI_WFM_CH0, &wfm0);
            GetCtrlVal(panel, GUI_WFM_CH1, &wfm1);

            DeleteGraphPlot (gui, GUI_GRAPH, -1, VAL_IMMEDIATE_DRAW);
            if (wfm0 == 0)
                PlotY (gui, GUI_GRAPH, sine, WFM_SIZE, VAL_DOUBLE, VAL_FAT_LINE,
                VAL_EMPTY_SQUARE, VAL_SOLID, 1, VAL_CYAN);
            else if (wfm0 == 1)
                PlotY (gui, GUI_GRAPH, square, WFM_SIZE, VAL_DOUBLE, VAL_FAT_LINE,
                VAL_EMPTY_SQUARE, VAL_SOLID, 1, VAL_CYAN);
            else if (wfm0 == 2)
                PlotY (gui, GUI_GRAPH, ramp, WFM_SIZE, VAL_DOUBLE, VAL_FAT_LINE,
                VAL_EMPTY_SQUARE, VAL_SOLID, 1, VAL_CYAN);

            if (wfm1 == 0)
                PlotY (gui, GUI_GRAPH, sine, WFM_SIZE, VAL_DOUBLE, VAL_FAT_LINE,
                VAL_EMPTY_SQUARE, VAL_SOLID, 1, VAL_BLUE);
            else if (wfm1 == 1)
                PlotY (gui, GUI_GRAPH, square, WFM_SIZE, VAL_DOUBLE, VAL_FAT_LINE,
                VAL_EMPTY_SQUARE, VAL_SOLID, 1, VAL_BLUE);
            else if (wfm1 == 2)
                PlotY (gui, GUI_GRAPH, ramp, WFM_SIZE, VAL_DOUBLE, VAL_FAT_LINE,
                VAL_EMPTY_SQUARE, VAL_SOLID, 1, VAL_BLUE);


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

