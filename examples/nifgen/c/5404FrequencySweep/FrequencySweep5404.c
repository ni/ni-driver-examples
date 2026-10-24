/*************************************************************\
    Example of a frequency sweep using FGEN with
    the 5404 in a LabWindows/CVI environment.  A session is
    established, the board is initialized, values for start and
    stop frequencies and amplitude are read, and the appropriate
    sine wave is outputted.
    
    The frequencies are computed using logarithmic scaling.
    
    NI  7/10/2002
\*************************************************************/

/*
    checkErr macro explanation:
    The error flag of any niFgen function call is stored in variable
    ViStatus error.  If there is an error, control is directed towards
    the Error label.  The code basically reads as:
    
    if (error < 0)
        goto Error;
*/

#include <cvirte.h>     
#include <userint.h>
#include "FrequencySweep5404.h"
#include "nifgen.h"
#include <utility.h>
#include <ansi_c.h>
#include "niModInstCustCtrl.h"  

void triggeredMethod(void);
void timedMethod(void);
void stopGen(void);

static int panelHandle;
static ViSession instrHandle = VI_NULL;
static ViStatus error = VI_NULL;
static ViReal64 freqAry[512];
static int numSteps = 0, triggeredCount;

int main (int argc, char *argv[])
{
    if (InitCVIRTE (0, argv, 0) == 0)
        return -1;  /* out of memory */
    if ((panelHandle = LoadPanel (0, "FrequencySweep5404.uir", PANEL)) < 0)
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
    ViInt32 contGen;
    switch (event)
        {
        case EVENT_COMMIT:
            if (instrHandle != VI_NULL) {
                niFgen_ConfigureOutputEnabled(instrHandle, "0", VI_FALSE);
                niFgen_AbortGeneration(instrHandle);
                niFgen_close(instrHandle);
                instrHandle = VI_NULL;
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
    ViChar resource[256];

    switch (event)
        {
        case EVENT_COMMIT:
            GetCtrlVal(panelHandle, PANEL_SESSION, &toggleState);
            if (toggleState) {          // initiate new session
                GetCtrlVal(panelHandle, PANEL_RESOURCE, resource);
                checkErr(niFgen_init (resource, VI_TRUE, VI_TRUE, &instrHandle));
                SetCtrlAttribute(panelHandle, PANEL_AMPLITUDE, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_STARTFREQ, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_STOPFREQ, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_STEPS, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_STEPMETHOD, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_TIME, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_GENERATE, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_RUNNING, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_RESOURCE, ATTR_DIMMED, 1);
            } 
            else {                  // end current session
                checkErr(niFgen_close(instrHandle));
                instrHandle = VI_NULL;
                SetCtrlAttribute(panelHandle, PANEL_AMPLITUDE, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_STARTFREQ, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_STOPFREQ, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_STEPS, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_STEPMETHOD, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_TIME, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_GENERATE, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_SINGLESTEP, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_RUNNING, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_RESOURCE, ATTR_DIMMED, 0);
                
                SetCtrlVal(panelHandle, PANEL_GENERATE, 0);
                
                SetCtrlVal(panelHandle, PANEL_RUNNING, 0);
            }
        Error:
            handleError();
            break;
        }
    return 0;
}


/*****************************************************
Callback function when the generate button is clicked.
The step method switch value is read and control is
transferred to the appropriate function.
*****************************************************/
int CVICALLBACK generate (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{

    int stepMethodVal, gen, i;
    ViReal64 logFactor, startFreq, stopFreq, logStart; 
    
    switch (event)
        {
        case EVENT_COMMIT:
            GetCtrlVal(panelHandle, PANEL_GENERATE, &gen);
            if (gen) {          // toggle pressed
            
                // compute the frequency array using logarithmic spacing
                GetCtrlVal(panelHandle, PANEL_STARTFREQ, &startFreq);
                GetCtrlVal(panelHandle, PANEL_STOPFREQ, &stopFreq);
                GetCtrlVal(panelHandle, PANEL_STEPS, &numSteps);
                logStart = log10(startFreq);
                logFactor = (log10(stopFreq) - logStart) / numSteps;
                for (i=0; i<numSteps; i++)
                    freqAry[i] = pow(10, logFactor * i + logStart);
                    
                // dim appropriate fields in GUI for current state of program.
                SetCtrlAttribute(panelHandle, PANEL_AMPLITUDE, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_STARTFREQ, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_STOPFREQ, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_STEPS, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_STEPMETHOD, ATTR_DIMMED, 1);
                
                SetCtrlVal(panelHandle, PANEL_RUNNING, 1);
                    
                GetCtrlVal(panelHandle, PANEL_STEPMETHOD, &stepMethodVal);
            
                if (stepMethodVal)         // triggered method
                    triggeredMethod();
                else {                     // timed method
                    timedMethod();
                    stopGen();
                }
            }
            else            // toggle release.  stop generation
                stopGen();
            break;
        }
    return 0;
}

/*****************************************************
    Helper function.  Simply stops the generation.
    Dedicated a function to it because it's called in
    several different locations in the program.
    
    Sets the state of the GUI back to 'no generation' state.
    Undims the appropriate fields.
******************************************************/
void stopGen() {
    int contGen;
    
    niFgen_ConfigureOutputEnabled(instrHandle, "0", VI_FALSE);
    niFgen_AbortGeneration(instrHandle);
    
    SetCtrlAttribute(panelHandle, PANEL_AMPLITUDE, ATTR_DIMMED, 0);
    SetCtrlAttribute(panelHandle, PANEL_STARTFREQ, ATTR_DIMMED, 0);
    SetCtrlAttribute(panelHandle, PANEL_STOPFREQ, ATTR_DIMMED, 0);
    SetCtrlAttribute(panelHandle, PANEL_STEPS, ATTR_DIMMED, 0);
    SetCtrlAttribute(panelHandle, PANEL_STEPMETHOD, ATTR_DIMMED, 0);
    
    SetCtrlVal(panelHandle, PANEL_RUNNING, 0);
    SetCtrlVal(panelHandle, PANEL_GENERATE, 0);
}

/******************************************************
  timedMethod() runs the frequency sweep with a time delay between each step
  frequency.  The delay is controlled by the user.
******************************************************/
void timedMethod() {

    ViReal64 amplitude, startFreq;
    double mark, interval;
    int timeDelay, i;

    GetCtrlVal(panelHandle, PANEL_AMPLITUDE, &amplitude);
    GetCtrlVal(panelHandle, PANEL_STARTFREQ, &startFreq);
    GetCtrlVal(panelHandle, PANEL_TIME, &timeDelay);
    checkErr(niFgen_ConfigureStandardWaveform (instrHandle, "0",
                                               NIFGEN_VAL_WFM_SINE, amplitude,
                                               0.00, startFreq, 0.00));
    checkErr(niFgen_InitiateGeneration(instrHandle));
    checkErr(niFgen_ConfigureOutputEnabled(instrHandle, "0", VI_TRUE));
    
    // step through the frequency array and delay before proceeding
    // to the next iteration.
    for (i=0; i<=numSteps; i++) {
        checkErr(niFgen_SetAttributeViReal64 (instrHandle, "",
                                              NIFGEN_ATTR_FUNC_FREQUENCY, freqAry[i]));
        mark = Timer();
        interval = timeDelay / 1000.0;
        SyncWait (mark, interval);
    }
        
Error:
    handleError();
    
}

/******************************************************
  triggeredMethod() runs the frequency sweep manually.  User controls
  rate of sweep by clicking on the "single step" button to iterate
  through the array of frequencies.
******************************************************/
void triggeredMethod() {

    ViReal64 amplitude, startFreq;
    
    GetCtrlVal(panelHandle, PANEL_AMPLITUDE, &amplitude);
    GetCtrlVal(panelHandle, PANEL_STARTFREQ, &startFreq);
    checkErr(niFgen_ConfigureStandardWaveform (instrHandle, "0",
                                               NIFGEN_VAL_WFM_SINE, amplitude,
                                               0.00, startFreq, 0.00));
    checkErr(niFgen_InitiateGeneration(instrHandle));
    checkErr(niFgen_ConfigureOutputEnabled(instrHandle, "0", VI_TRUE));

    // triggeredCount is a global variable that helps the callback
    // function singleStep() keep track of where in the frequency array
    // the sweep is at.  It is initialized to 1 (the second element in the
    // array) because the initial frequency is the first element.
    triggeredCount = 1;

Error:
    handleError();
    
}

/******************************************************
  singleStep() updates the frequency and iterates the triggeredCount
  variable if doing a triggered sweep.
******************************************************/
int CVICALLBACK singleStep (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    switch (event)
        {
        case EVENT_COMMIT:
            if (triggeredCount < numSteps) {      // if still in sweep
                checkErr(niFgen_SetAttributeViReal64 (instrHandle, "",
                                                      NIFGEN_ATTR_FUNC_FREQUENCY,
                                                      freqAry[triggeredCount]));
                triggeredCount++;
            }
            else                                  // done with sweep
                stopGen();
        Error:
            handleError();  
            break;
        }
    return 0;
}

/******************************************************
    stepMethod() simply sets the GUI appropriately depending
    on the method chosen by the user.
******************************************************/
int CVICALLBACK stepMethod (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    int value;
    switch (event)
        {
        case EVENT_COMMIT:
            GetCtrlVal(panelHandle, PANEL_STEPMETHOD, &value);
            if (value) {
                SetCtrlAttribute(panelHandle, PANEL_SINGLESTEP, ATTR_DIMMED, 0);
                SetCtrlAttribute(panelHandle, PANEL_TIME, ATTR_DIMMED, 1);
            } else {
                SetCtrlAttribute(panelHandle, PANEL_SINGLESTEP, ATTR_DIMMED, 1);
                SetCtrlAttribute(panelHandle, PANEL_TIME, ATTR_DIMMED, 0);
            }
            break;
        }
    return 0;
}
