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
/*     4-99    BC        Created                                             */
/*****************************************************************************/
#include <utility.h>
#include <ansi_c.h>
#include <cvirte.h>     /* Needed if linking in external compiler; harmless otherwise */
#include <userint.h>
#include <stdio.h>
#include "nifgen.h"
#include "ArbitraryWaveform.h"
#include "niModInstCustCtrl.h"

static int gui;
ViSession vi=VI_NULL;
ViStatus error = VI_SUCCESS;
void wfmGenerate(void);
void ErrorBox(void);
void LoadFile(void);

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
    if ((gui = LoadPanel (0, "ArbitraryWaveform.uir", GUI)) < 0)
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
void wfmGenerate() { 
    ViChar Resource[256], Channel[256], WfmFile[256], sampleClockSource[256];
    ViReal64 SampleRate, Gain, ActualSampleRate;
    ViInt32  clockMode, FilterEnable;
    ViInt32  wfmHandle;
    
    /*- Read in all of the control values ----------------------------------*/
    GetCtrlVal(gui, GUI_RESOURCE, Resource);
    GetCtrlVal(gui, GUI_CHANNEL, Channel);
    GetCtrlVal(gui, GUI_WFM_FILE, WfmFile);
    GetCtrlVal(gui, GUI_SAMPLE_RATE, &SampleRate);
    GetCtrlVal(gui, GUI_GAIN, &Gain);
    GetCtrlVal(gui, GUI_FILTER_ENABLE, &FilterEnable);
    GetCtrlVal(gui, GUI_CLOCK_MODE, &clockMode);
    GetCtrlVal(gui, GUI_SAMPLE_CLOCK, sampleClockSource);
    if(vi != VI_NULL) {
        checkErr(niFgen_close(vi));
    }

    /*- Initialize the session ---------------------------------------------*/
    checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Configure output mode for arbitrary waveform -----------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_ARB));

    /*- Configure clock mode -----------------------------------------------*/
    checkErr(niFgen_ConfigureClockMode(vi, clockMode));

    /*- Create the arbitrary waveform --------------------------------------*/
	checkErr(niFgen_CreateWaveformFromFileI16(
	   vi,
	   Channel,
	   WfmFile,
	   NIFGEN_VAL_BIG_ENDIAN,
	   &wfmHandle
	));
	
    /*- Select the waveform to generate ------------------------------------*/
    checkErr(niFgen_ConfigureArbWaveform(vi, Channel, wfmHandle, Gain,
                                         0)); 

    /*- Configure update clock settings ------------------------------------*/
    checkErr(niFgen_ConfigureSampleClockSource(vi, sampleClockSource));
    checkErr(niFgen_ConfigureSampleRate(vi, SampleRate));
    checkErr(niFgen_GetAttributeViReal64 (vi, VI_NULL,
                                          NIFGEN_ATTR_ACTUAL_ARB_SAMPLE_RATE,
                                          &ActualSampleRate));  

    /*- Configure filters --------------------------------------------------*/
    checkErr(niFgen_SetAttributeViBoolean(vi, Channel,
                                          NIFGEN_ATTR_DIGITAL_FILTER_ENABLED,
                                          FilterEnable)); 
    checkErr(niFgen_SetAttributeViBoolean(vi, Channel,
                                          NIFGEN_ATTR_ANALOG_FILTER_ENABLED,
                                          FilterEnable));

    /*- Enable output ------------------------------------------------------*/
    checkErr(niFgen_ConfigureOutputEnabled(vi, Channel, VI_TRUE));

    /*- Generate the waveform -------------------------------------*/
    checkErr(niFgen_InitiateGeneration(vi));


    /*- Update the control values ------------------------------------------*/
    SetCtrlVal(gui, GUI_SAMPLE_RATE, ActualSampleRate);
    SetCtrlVal(gui, GUI_GENERATE, VI_TRUE);
Error:
    /*- Display any errors -------------------------------------------------*/
    ErrorBox();
    if((error < 0) && (vi != VI_NULL)) {
        SetCtrlAttribute (gui, GUI_SAMPLE_RATE, ATTR_DIMMED, 0);
        niFgen_close(vi);
        vi = VI_NULL;
        SetCtrlVal(gui, GUI_GENERATE, VI_FALSE);
    }
}

/****************************************************************************\
  Display a file select dialog box when pick file button is pushed
\****************************************************************************/
int CVICALLBACK pick_file (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViChar FileName[512];
    switch (event)
        {
        case EVENT_COMMIT:
            if(FileSelectPopup (".", "*.bin", "", "Pick a 16 bit binary file from the Virtual Bench Waveform Editor",
                                VAL_LOAD_BUTTON, 0, 0, 1, 0, FileName) <=0) {
                SetCtrlVal(gui, GUI_WFM_FILE, "");
                return -1;
            }
            SetCtrlVal(gui, GUI_WFM_FILE, FileName);
            break;
        }
    return 0;
}

/****************************************************************************\
  Re-load the waveform to the Arb when the load waveform button is pushed
\****************************************************************************/
int CVICALLBACK load_button (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    switch (event)
        {
        case EVENT_COMMIT:
            if (vi != NULL) wfmGenerate();
            break;
        }
    return 0;
}


/****************************************************************************\
  Change the sample rate when the sample rate knob is changed  
\****************************************************************************/
int CVICALLBACK sample_rate (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViReal64 SampleRate;
    switch (event)
        {
        case EVENT_VAL_CHANGED:
            if(vi != VI_NULL) {
                GetCtrlVal(gui, GUI_SAMPLE_RATE, &SampleRate);
                checkErr(niFgen_SetAttributeViReal64(vi, VI_NULL, NIFGEN_ATTR_ARB_SAMPLE_RATE, 
                                                SampleRate));
                checkErr(niFgen_GetAttributeViReal64(vi, VI_NULL, NIFGEN_ATTR_ACTUAL_ARB_SAMPLE_RATE,
                         &SampleRate));
                SetCtrlVal(gui, GUI_SAMPLE_RATE, SampleRate);
Error:
                ErrorBox();
            }
            break;
        }
    return 0;
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
  Change the filters when the filtter button is changed
\****************************************************************************/
int CVICALLBACK filter_enable (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViInt32 filterEnable;
    ViBoolean filt;
    switch (event)
        {
        case EVENT_COMMIT:
            if(vi != VI_NULL) {
                GetCtrlVal(gui, GUI_FILTER_ENABLE, &filterEnable);
                filt = (filterEnable == 1) ? VI_TRUE : VI_FALSE;
                checkErr(niFgen_SetAttributeViBoolean(vi, VI_NULL, NIFGEN_ATTR_ANALOG_FILTER_ENABLED, 
                                                filt));
Error:
                ErrorBox();
            }
            break;
        }
    return 0;
}

/****************************************************************************\
  Start generation when the generate button is pushed
  Close the session when the generate button is released
\****************************************************************************/
int CVICALLBACK generate (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2) {
    int Generate;
    switch (event)
        {
        case EVENT_COMMIT:
            GetCtrlVal(gui, GUI_GENERATE, &Generate);
            if(Generate) {
                SetCtrlAttribute (gui, GUI_SAMPLE_RATE, ATTR_DIMMED, 1);
                wfmGenerate();
            }
            else {
                if(vi != VI_NULL) {
                    SetCtrlAttribute (gui, GUI_SAMPLE_RATE, ATTR_DIMMED, 0);
                    checkErr(niFgen_AbortGeneration(vi));
                    checkErr(niFgen_close(vi));
                    vi=VI_NULL;
Error:
                    ErrorBox();
                }
            }
            break;
        }
    return 0;
}
