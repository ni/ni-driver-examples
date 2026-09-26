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
	  
#include <ansi_c.h>
#include <cvirte.h>     /* Needed if linking in external compiler; harmless otherwise */
#include <userint.h>
#include "nifgen.h"
#include "ArbitrarySequence.h"
#include "niModInstCustCtrl.h"

// This example program assumes a sequence with 3 stages
#define sequenceLength 3

static int gui;
ViSession vi=VI_NULL;
ViStatus error = VI_SUCCESS;
ViInt32 wfmHandles[sequenceLength];
ViInt32 loopCounts[sequenceLength];
ViInt32 markerPositions[sequenceLength];

void configureAndInitiateGeneration(void);
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
    if ((gui = LoadPanel (0, "ArbitrarySequence.uir", GUI)) < 0)
        return -1;
    niModInstCVICust_NewCtrl (gui, GUI_RESOURCE, "nifgen");
    DisplayPanel (gui);
    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (gui, GUI_RESOURCE);
    return 0;
}


/****************************************************************************\
  Loads in the sequence and starts generation
\****************************************************************************/
void configureAndInitiateGeneration() { 
    ViChar Resource[256], Channel[256], wfmFiles[sequenceLength][256], 
           triggerSource[256], sampleClockSource[256];
    ViReal64 SampleRate, Gain, dcOffset, ActualSampleRate;
    ViInt32  clockMode, FilterEnable, triggerType, triggerMode, seqHandle;
    
    /*- Get all the control values -------------------------------------------*/
    GetCtrlVal(gui, GUI_RESOURCE, Resource);
    GetCtrlVal(gui, GUI_CHANNEL, Channel);
    GetCtrlVal(gui, GUI_SAMPLE_RATE, &SampleRate);
    GetCtrlVal(gui, GUI_GAIN, &Gain);
    GetCtrlVal(gui, GUI_FILTER_ENABLE, &FilterEnable);
    GetCtrlVal(gui, GUI_CLOCK_MODE, &clockMode);
    GetCtrlVal(gui, GUI_SAMPLE_CLOCK, sampleClockSource);
    GetCtrlVal(gui, GUI_TRIGGER_TYPE, &triggerType);
    GetCtrlVal(gui, GUI_TRIGGER_SOURCE, triggerSource);
    GetCtrlVal(gui, GUI_TRIGGER_MODE, &triggerMode);
    GetCtrlVal(gui, GUI_DC_OFFSET, &dcOffset);
    GetCtrlVal(gui, GUI_WFM_FILE_1, wfmFiles[0]);
    GetCtrlVal(gui, GUI_WFM_FILE_2, wfmFiles[1]);
    GetCtrlVal(gui, GUI_WFM_FILE_3, wfmFiles[2]);
    GetCtrlVal(gui, GUI_LOOP_COUNT_1, &loopCounts[0]);
    GetCtrlVal(gui, GUI_LOOP_COUNT_2, &loopCounts[1]);
    GetCtrlVal(gui, GUI_LOOP_COUNT_3, &loopCounts[2]);
    GetCtrlVal(gui, GUI_MARKER_1, &markerPositions[0]);
    GetCtrlVal(gui, GUI_MARKER_2, &markerPositions[1]);
    GetCtrlVal(gui, GUI_MARKER_3, &markerPositions[0]);

    /*- Open a session to the device -----------------------------------------*/
	checkErr(niFgen_init(Resource, 0, 0, &vi));

   /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));
    
    /*- Abort any previous generation ----------------------------------------*/
    checkErr(niFgen_AbortGeneration(vi));

    /*- Prepare arb for sequence mode output ---------------------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_SEQ));
    checkErr(niFgen_ClearArbSequence(vi, NIFGEN_VAL_ALL_SEQUENCES));

    /*- Configure other parameters (sample rate, filters, triggers) ----------*/
    checkErr(niFgen_ConfigureClockMode(vi, clockMode));
    checkErr(niFgen_ConfigureSampleClockSource(vi, sampleClockSource));
    checkErr(niFgen_ConfigureSampleRate(vi, SampleRate));
    checkErr(niFgen_GetAttributeViReal64 (vi, VI_NULL,
                                         NIFGEN_ATTR_ACTUAL_ARB_SAMPLE_RATE,
                                         &ActualSampleRate));  
    checkErr(niFgen_SetAttributeViBoolean(vi, Channel,
                                         NIFGEN_ATTR_DIGITAL_FILTER_ENABLED,
                                         FilterEnable)); 
    checkErr(niFgen_SetAttributeViBoolean(vi, Channel,
                                         NIFGEN_ATTR_ANALOG_FILTER_ENABLED,
                                         FilterEnable)); 

	if (triggerType == 0) {
		checkErr(niFgen_DisableStartTrigger(vi));
	} else if (triggerType == 1) {
		checkErr(niFgen_ConfigureSoftwareEdgeStartTrigger (vi));
	} else if (triggerType == 2) {
	    checkErr(niFgen_ConfigureDigitalEdgeStartTrigger (vi, triggerSource,
														  NIFGEN_VAL_RISING_EDGE));
	}
	
	checkErr(niFgen_ConfigureTriggerMode(vi, Channel, triggerMode));   
                                         
    /*- Create waveforms ----------------------------------------------------*/
    checkErr(niFgen_CreateWaveformFromFileI16(vi, Channel, wfmFiles[0],
                                              NIFGEN_VAL_BIG_ENDIAN, &wfmHandles[0]));
    checkErr(niFgen_CreateWaveformFromFileI16(vi, Channel, wfmFiles[1],
                                              NIFGEN_VAL_BIG_ENDIAN, &wfmHandles[1]));
    checkErr(niFgen_CreateWaveformFromFileI16(vi, Channel, wfmFiles[2],
                                              NIFGEN_VAL_BIG_ENDIAN, &wfmHandles[2]));
    
    /*- Create the arbitrary sequence----------------------------------------*/
    checkErr(niFgen_CreateAdvancedArbSequence(vi, sequenceLength, wfmHandles, 
                                             loopCounts, VI_NULL, markerPositions,
                                             VI_NULL, &seqHandle));

    /*- Select arb sequence to generate, configure gain and offset ----------*/
    checkErr(niFgen_ConfigureArbSequence(vi, Channel, seqHandle, Gain,
                                        dcOffset)); 
                                        
    /*- Enable output -------------------------------------------------------*/
    checkErr(niFgen_ConfigureOutputEnabled(vi, Channel, VI_TRUE));

    /*- Generate the sequence -----------------------------------------------*/
    checkErr(niFgen_InitiateGeneration(vi));

    /*- Update the control values -------------------------------------------*/
    SetCtrlVal(gui, GUI_SAMPLE_RATE, ActualSampleRate);
    SetCtrlVal(gui, GUI_GENERATE, VI_TRUE);
    
Error:

    /*- Free up memory and desplay any errors -------------------------------*/
    ErrorBox();
    if((error != VI_SUCCESS) && (vi != VI_NULL)) {
        niFgen_close(vi);
        vi = VI_NULL;
        SetCtrlVal(gui, GUI_GENERATE, VI_FALSE);
    }
}


/****************************************************************************\
  Displays a file selection dialog box to select a waveform
\****************************************************************************/
int CVICALLBACK pick_wfm_file_1 (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViChar FileName[512];
    switch (event)
        {
        case EVENT_COMMIT:
            if(FileSelectPopup (".", "*.bin", "", "Waveform File", VAL_LOAD_BUTTON, 0,
                                0, 1, 0, FileName) <=0) {
                SetCtrlVal(gui, GUI_WFM_FILE_1, "");
                return -1;
            }
            SetCtrlVal(gui, GUI_WFM_FILE_1, FileName);
            break;
        }
    return 0;
}

int CVICALLBACK pick_wfm_file_2 (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViChar FileName[512];
    switch (event)
        {
        case EVENT_COMMIT:
            if(FileSelectPopup (".", "*.bin", "", "Waveform File", VAL_LOAD_BUTTON, 0,
                                0, 1, 0, FileName) <=0) {
                SetCtrlVal(gui, GUI_WFM_FILE_2, "");
                return -1;
            }
            SetCtrlVal(gui, GUI_WFM_FILE_2, FileName);
            break;
        }
    return 0;
}

int CVICALLBACK pick_wfm_file_3 (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViChar FileName[512];
    switch (event)
        {
        case EVENT_COMMIT:
            if(FileSelectPopup (".", "*.bin", "", "Waveform File", VAL_LOAD_BUTTON, 0,
                                0, 1, 0, FileName) <=0) {
                SetCtrlVal(gui, GUI_WFM_FILE_3, "");
                return -1;
            }
            SetCtrlVal(gui, GUI_WFM_FILE_3, FileName);
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
  Change the filters when the filter button is changed
 ****************************************************************************/
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
                GetCtrlVal(gui, GUI_SW_TRIG, &trig);
                if(trig == 1) {
                    checkErr(niFgen_SendSoftwareEdgeTrigger (vi, NIFGEN_VAL_START_TRIGGER, ""));
                }
Error:
                SetCtrlVal(gui, GUI_SW_TRIG, 0);
                ErrorBox();
            }
            break;
        }
    return 0;
}


/****************************************************************************\
  Close the session and exit the program when the quit button is pushed
\****************************************************************************/
int CVICALLBACK quit (int panel, int control, int event,
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
            	// clear any errors from previous generations
                error = VI_SUCCESS;
                if(vi==VI_NULL) {
                    SetCtrlVal(gui, GUI_GENERATE, VI_FALSE);
                }
                if(error == VI_SUCCESS) configureAndInitiateGeneration();
            }
            else {
                if(vi != VI_NULL) {
                    checkErr(niFgen_AbortGeneration(vi));
Error:
                    vi = VI_NULL;
                    ErrorBox();
                }
            }
            break;
        }
    return 0;
}


