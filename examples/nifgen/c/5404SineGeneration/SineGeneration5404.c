/*************************************************************\
    Example of producing a simple sine wave using FGEN with
    the 5404 in a LabWindows/CVI environment.  A session is
    established, the board is initialized, values for frequency
    and amplitude are read, and the appropriate sine wave is
    outputted.
    
    Control of amplitude and frequency is given to the user.
    Changes can be made while generating.
    
    NI  6/19/2002
\*************************************************************/

/*
    checkErr macro explanation:
    The error flag of any niFgen function call is stored in variable
    ViStatus error.  If there is an error, control is directed towards
    the Error label.  The code basically reads as:
    
    if (error < 0)
        goto Error;
*/

#include <ansi_c.h>
#include <cvirte.h>     
#include <userint.h>
#include "SineGeneration5404.h"
#include "nifgen.h"
#include "niModInstCustCtrl.h"

static int panelHandle;
static ViSession instrHandle = VI_NULL;
static ViStatus error = VI_SUCCESS;

void runGeneration(void);
void handleError(void);

int main (int argc, char *argv[])
{
    if (InitCVIRTE (0, argv, 0) == 0)
        return -1;  /* out of memory */
    if ((panelHandle = LoadPanel (0, "SineGeneration5404.uir", PANEL)) < 0)
        return -1;
    niModInstCVICust_NewCtrl (panelHandle, PANEL_RESOURCE, "nifgen");        
    DisplayPanel (panelHandle);
    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (panelHandle, PANEL_RESOURCE);
    DiscardPanel (panelHandle);
    return 0;
}

/********************************************************\
    Error handler.  Generation of waveform is halted upon any error.
\********************************************************/
void handleError() {
    ViUInt32 errorMessageSize;
    ViChar*  errorMessage;
    if (error != VI_SUCCESS) {
        errorMessageSize = niFgen_GetError(instrHandle, VI_NULL, 0, VI_NULL);
        errorMessage = (ViChar *) malloc(sizeof(ViChar) * errorMessageSize);
        niFgen_GetError(instrHandle, &error, errorMessageSize, errorMessage);
        ResetTextBox(panelHandle, PANEL_ERROR, errorMessage);
        free(errorMessage);
        if (instrHandle != VI_NULL) {
            niFgen_ConfigureOutputEnabled(instrHandle, "0", VI_FALSE);
            niFgen_AbortGeneration(instrHandle);
            SetCtrlVal(panelHandle, PANEL_RUNNING, 0);
            SetCtrlVal(panelHandle, PANEL_GENERATE, 0);
        }
    }
    else
        ResetTextBox(panelHandle, PANEL_ERROR, "");
}

/********************************************************\
    Quit callback function that closes the current session,
    leaving the current state of the board intact.
\********************************************************/
int CVICALLBACK quit (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    switch (event)
        {
        case EVENT_COMMIT:
            if (instrHandle != VI_NULL) {
                niFgen_close (instrHandle);
                instrHandle = VI_NULL;
            }
            QuitUserInterface (0);
            break;
        }
    return 0;
}

/**********************************************************\
    Toggle between generating and ending a sine waveform.
    There must be an open session in order to generate.
\**********************************************************/
int CVICALLBACK generateToggle (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    int generate;
    
    switch (event)
        {
        case EVENT_COMMIT:
            GetCtrlVal(panelHandle, PANEL_GENERATE, &generate);
            if (generate)                   // toggle pressed, generate sine waveform
                runGeneration();
            else {                          // toggle released, kill waveform
                // disable output
                niFgen_ConfigureOutputEnabled(instrHandle, "0", VI_FALSE);
                // kill generation
                niFgen_AbortGeneration(instrHandle);
                SetCtrlVal(panelHandle, PANEL_RUNNING, 0);
            }
            break;
        }
    return 0;
}

/*************************************************************\
    Configure a sine wave generation with the specified amplitude
    and frequency.  Output is disabled at initialization by default
    so it needs to be enabled upon generation.
\*************************************************************/
void runGeneration() {
    ViChar resource[256];
    ViReal64 amplitude, frequency;
    
    // read in values for waveform
    GetCtrlVal(panelHandle, PANEL_RESOURCE, resource);
    GetCtrlVal(panelHandle, PANEL_AMPLITUDE, &amplitude);
    GetCtrlVal(panelHandle, PANEL_FREQUENCY, &frequency);

    /*
        configures the current waveform.  Parameters are as follows:
        instrHandle - session ID (instrument handle)
        VI_NULL - channel name (default is '0')
        NIFGEN_VAL_WFM_SINE - nifgen constant for a sine wave
        amplitude - amplitude of waveform (range: 1 - 2 Vpp)
        0 - DC Offset.  5404 does not allow control of DC Offset
        frequency - frequency of waveform (range: 9kHz - 100MHz)
        0.00 - start phase (degrees)
    */
    checkErr(niFgen_ConfigureStandardWaveform (instrHandle, VI_NULL,
                                              NIFGEN_VAL_WFM_SINE,
                                              amplitude, 0, frequency,
                                              0.00));
                                              
    // starts generation of waveform
    checkErr(niFgen_InitiateGeneration (instrHandle));
    
    // enables output.  This is necessary to actually see the produced waveform.
    // Otherwise, the board would simply generate, but not output the signal.
    checkErr(niFgen_ConfigureOutputEnabled (instrHandle, "0", VI_TRUE));
        
    SetCtrlVal(panelHandle, PANEL_RUNNING, 1);
    
Error :
    handleError();

}

/*******************************************************\
    Allows for on-the-fly adjustment of amplitude.
\*******************************************************/
int CVICALLBACK ampChange (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{

    ViReal64 amplitude;
    
    switch (event)
        {
        case EVENT_VAL_CHANGED:
            GetCtrlVal(panelHandle, PANEL_AMPLITUDE, &amplitude);
            checkErr(niFgen_SetAttributeViReal64 (instrHandle, VI_NULL,
                                                 NIFGEN_ATTR_FUNC_AMPLITUDE,
                                                 amplitude));
        Error:                                              
            handleError();
            break;
        }
    return 0;
}

/*******************************************************\
    Allows for on-the-fly adjustment of frequency.
\*******************************************************/
int CVICALLBACK freqChange (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{

    ViReal64 frequency;
    
    switch (event)
        {
        case EVENT_VAL_CHANGED:
            GetCtrlVal(panelHandle, PANEL_FREQUENCY, &frequency);
            checkErr(niFgen_SetAttributeViReal64 (instrHandle, VI_NULL,
                                                 NIFGEN_ATTR_FUNC_FREQUENCY,
                                                 frequency));
        Error:                                              
            handleError();
            break;
        }
    return 0;
}

/****************************************************************\
    Open or close a session for use (depending on toggle state).
    When closing a session, the state of the waveform (running or not, etc)
        is kept intact.
\****************************************************************/

int CVICALLBACK sessionToggle (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    int toggleState;
    ViChar resource[256];
    
    switch (event)
        {
        case EVENT_COMMIT:
            GetCtrlVal(panelHandle, PANEL_SESSION, &toggleState);
            if (toggleState) {              // initiate new session
                GetCtrlVal(panelHandle, PANEL_RESOURCE, resource);
                checkErr(niFgen_init (resource, VI_TRUE, VI_TRUE, &instrHandle));
                SetCtrlAttribute(panelHandle, PANEL_FREQUENCY, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_AMPLITUDE, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_GENERATE, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_RUNNING, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_RESOURCE, ATTR_DIMMED, 1);
            }
            else {                          // toggle being released, end current session
                                            // but keep in current state
                checkErr(niFgen_close(instrHandle));
                instrHandle = VI_NULL;
                SetCtrlAttribute(panelHandle, PANEL_FREQUENCY, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_AMPLITUDE, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_GENERATE, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_RUNNING, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_RESOURCE, ATTR_DIMMED, 0);
            }
        Error:                                              
            handleError();
            break;
        }
    return 0;
}
