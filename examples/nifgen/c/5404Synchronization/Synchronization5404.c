/*************************************************************\
    Example of synchronizing two signals using FGEN with
    the 5404 in a LabWindows/CVI environment.  Two sessions are
    established and two separate waveforms are generated.  Using
    software triggers, the two waveforms start at the same time.
    The phase can be changed on the fly to put the waves in or
    out of phase.
    
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
#include "Synchronization5404.h"
#include "nifgen.h"
#include "niModInstCustCtrl.h"

static int panelHandle;
static ViSession instrHandle1 = VI_NULL;
static ViSession instrHandle2 = VI_NULL;
static ViStatus error = VI_SUCCESS;

void runGeneration(void);
void handleError(void);

int main (int argc, char *argv[])
{
    if (InitCVIRTE (0, argv, 0) == 0)
        return -1;  /* out of memory */
    if ((panelHandle = LoadPanel (0, "Synchronization5404.uir", PANEL)) < 0)
        return -1;
    niModInstCVICust_NewCtrl (panelHandle, PANEL_RESOURCE1, "nifgen");
    niModInstCVICust_NewCtrl (panelHandle, PANEL_RESOURCE2, "nifgen");
    DisplayPanel (panelHandle);
    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (panelHandle, PANEL_RESOURCE1);
    niModInstCVICust_DiscardCtrl (panelHandle, PANEL_RESOURCE2);
    DiscardPanel (panelHandle);
    return 0;
}

/********************************************************\
    Error handler.  Generation of waveforms is halted upon any error.
\********************************************************/
void handleError() {
    ViUInt32 errorMessageSize;
    ViChar*  errorMessage;
    ViSession instrHandleErr;
        
    if (error != VI_SUCCESS) {
        if (niFgen_GetError(instrHandle1, VI_NULL, 0, VI_NULL) > 1)
            instrHandleErr = instrHandle1;
        else
            instrHandleErr = instrHandle2;
            
        errorMessageSize = niFgen_GetError(instrHandleErr, VI_NULL, 0, VI_NULL);
        errorMessage = (ViChar *) malloc(sizeof(ViChar) * errorMessageSize);
        niFgen_GetError(instrHandleErr, &error, errorMessageSize, errorMessage);
        ResetTextBox(panelHandle, PANEL_ERROR, errorMessage);
        free(errorMessage);
        
        if (instrHandle1 != VI_NULL) {
            niFgen_ConfigureOutputEnabled(instrHandle1, "0", VI_FALSE);
            niFgen_AbortGeneration(instrHandle1);
            }
        if (instrHandle2 != VI_NULL) {
            niFgen_ConfigureOutputEnabled(instrHandle2, "0", VI_FALSE);
            niFgen_AbortGeneration(instrHandle2);
        }
        SetCtrlVal(panelHandle, PANEL_RUNNING, 0);
        SetCtrlVal(panelHandle, PANEL_GENERATE, 0);
    } else
        ResetTextBox (panelHandle, PANEL_ERROR, "");
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
            if (instrHandle1 != VI_NULL) {
                niFgen_close(instrHandle1);
                instrHandle1 = VI_NULL;
            }
            if (instrHandle2 != VI_NULL) {
                niFgen_close(instrHandle2);
                instrHandle2 = VI_NULL;
            }
            QuitUserInterface (0);
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
    ViChar resource1[256], resource2[256];
    
    switch (event)
        {
        case EVENT_COMMIT:
            GetCtrlVal(panelHandle, PANEL_SESSION, &toggleState);
            if (toggleState) {                  // initiate new sessions
                GetCtrlVal(panelHandle, PANEL_RESOURCE1, resource1);
                checkErr(niFgen_init (resource1, VI_TRUE, VI_TRUE, &instrHandle1));
                GetCtrlVal(panelHandle, PANEL_RESOURCE2, resource2);
                checkErr(niFgen_init (resource2, VI_TRUE, VI_TRUE, &instrHandle2));
                SetCtrlAttribute(panelHandle, PANEL_FREQUENCY, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_AMPLITUDE, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_PHASE, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_GENERATE, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_RUNNING, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_RESOURCE1, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_RESOURCE2, ATTR_DIMMED, 1);
            }
            else {                              // toggle being released, end sessions
                                                // but keep in current states
                SetCtrlAttribute(panelHandle, PANEL_FREQUENCY, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_AMPLITUDE, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_PHASE, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_GENERATE, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_RUNNING, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_RESOURCE1, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_RESOURCE2, ATTR_DIMMED, 0);
                checkErr(niFgen_close(instrHandle1));
                instrHandle1 = VI_NULL;
                checkErr(niFgen_close(instrHandle2));
                instrHandle2 = VI_NULL;
            }
        Error:
            handleError();
            break;
        }
    return 0;
}

/***************************************************************\
    Allows for on-the-fly adjustment of phase offset for device 2.
    The two devices can be brought into phase or out-of-phase by
    an arbitrary amount.
\***************************************************************/
int CVICALLBACK phaseChange (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViReal64 phase;
    switch (event)
        {
        case EVENT_VAL_CHANGED:
            GetCtrlVal(panelHandle, PANEL_PHASE, &phase);
            checkErr(niFgen_SetAttributeViReal64 (instrHandle2, VI_NULL,
                                                  NIFGEN_ATTR_FUNC_START_PHASE, phase));
            checkErr(niFgen_SendSoftwareEdgeTrigger (instrHandle2,
													 NIFGEN_VAL_START_TRIGGER, ""));
        Error:
            handleError();
            break;
        }
    return 0;
}

/**********************************************************\
    Toggle between generating and ending waveforms.
    An open session must exist in order to generate.
\**********************************************************/
int CVICALLBACK generate (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    int generate;
    
    switch (event)
        {
        case EVENT_COMMIT:
            GetCtrlVal(panelHandle, PANEL_GENERATE, &generate);
            if (generate)                   // toggle pressed, generate waveforms
                runGeneration();
            else {                          // else, end all generations
                // disable outputs
                niFgen_ConfigureOutputEnabled(instrHandle1, "0", VI_FALSE);
                niFgen_ConfigureOutputEnabled(instrHandle2, "0", VI_FALSE);
                // kill generations
                niFgen_AbortGeneration(instrHandle1);
                niFgen_AbortGeneration(instrHandle2);
                SetCtrlVal(panelHandle, PANEL_RUNNING, 0);
                SetCtrlAttribute(panelHandle, PANEL_FREQUENCY, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_AMPLITUDE, ATTR_DIMMED, 0);
            }
            break;
        }
    return 0;
}
/**********************************************************\
    Generates two sine waveforms (with given frequency and
    amplitudes) and locks them to a common clock
    via a software trigger and using the RTSI 0 bus.
\**********************************************************/
void runGeneration() {
    ViReal64 amplitude, frequency;
    
    GetCtrlVal(panelHandle, PANEL_AMPLITUDE, &amplitude);
    GetCtrlVal(panelHandle, PANEL_FREQUENCY, &frequency);
    
    // configure both devices with the given frequency and amplitude
    checkErr(niFgen_ConfigureStandardWaveform(instrHandle1, VI_NULL,
                                              NIFGEN_VAL_WFM_SINE, amplitude,
                                              0, frequency, 0.00));
    checkErr(niFgen_ConfigureStandardWaveform(instrHandle2, VI_NULL,
                                              NIFGEN_VAL_WFM_SINE, amplitude,
                                              0, frequency, 0.00));
                                              
    // configure the reference clock for each device.
    checkErr(niFgen_ConfigureReferenceClock(instrHandle1, "PXI_Clk10", 10e6));
    checkErr(niFgen_ConfigureReferenceClock(instrHandle2, "PXI_Clk10", 10e6));
    
    // tell device 1 to wait for a software trigger
    checkErr(niFgen_ConfigureSoftwareEdgeStartTrigger (instrHandle1));
    // and route the software trigger to RTSI 0
    checkErr(niFgen_ExportSignal (instrHandle1, NIFGEN_VAL_START_TRIGGER, "",
								  "PXI_Trig0"));

    // tell device 2 to wait for a trigger on RTSI 0
    checkErr(niFgen_ConfigureDigitalEdgeStartTrigger (instrHandle2, "PXI_Trig0",
													  NIFGEN_VAL_RISING_EDGE));
                                           
    // initiate generation for device 2.  it is now waiting for a trigger.
    checkErr(niFgen_InitiateGeneration(instrHandle2));
    checkErr(niFgen_ConfigureOutputEnabled(instrHandle2, "0", VI_TRUE));
    
    // initiate generation for device 1.  it is also waiting for a trigger
    checkErr(niFgen_InitiateGeneration(instrHandle1));
    checkErr(niFgen_ConfigureOutputEnabled(instrHandle1, "0", VI_TRUE));
    // fire the trigger.  both waveforms start at the same time.
    checkErr(niFgen_SendSoftwareEdgeTrigger (instrHandle1,
											 NIFGEN_VAL_START_TRIGGER, ""));
    
    SetCtrlAttribute(panelHandle, PANEL_FREQUENCY, ATTR_DIMMED, 1);
    SetCtrlAttribute(panelHandle, PANEL_AMPLITUDE, ATTR_DIMMED, 1);
    SetCtrlVal(panelHandle, PANEL_RUNNING, 1);
    
Error:
    handleError();
}
