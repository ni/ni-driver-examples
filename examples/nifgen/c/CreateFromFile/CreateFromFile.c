/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Clock Mode Example source file                                            */
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
#include "CreateFromFile.h"
#include "niModInstCustCtrl.h"

#define WFM_SIZE 256

static int gui;
ViSession vi=VI_NULL;
ViStatus error = VI_SUCCESS;
ViReal64 sine[WFM_SIZE];

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
    if ((gui = LoadPanel (0, "CreateFromFile.uir", GUI)) < 0)
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
    ViChar FileName[1024] = "";
    ViReal64 SampleRate;
    ViInt32 WfmHandle;
    ViInt32 ByteOrder;
    
    /*- Get all the control values -----------------------------------------*/
    GetCtrlVal(gui, GUI_RESOURCE, Resource);
    GetCtrlVal(gui, GUI_CHANNEL, Channel);
    GetCtrlVal(gui, GUI_FILEPATH, FileName);
    GetCtrlVal(gui, GUI_SAMPLE_RATE, &SampleRate);
    GetCtrlVal(gui, GUI_BYTE_ORDER, &ByteOrder);
	
    checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Prepare arb for arbitrary waveform mode output ----------------------*/
    checkErr(niFgen_ConfigureOutputMode (vi, NIFGEN_VAL_OUTPUT_ARB));

	/*- Set sample rate ---------------------------------------------------- */
    checkErr(niFgen_ConfigureSampleRate(vi, SampleRate));

    /*- Create arbitrary waveform -------------------------------------------*/
    checkErr(niFgen_CreateWaveformFromFileI16 (vi, Channel, FileName,
											   ByteOrder, &WfmHandle));

    /*- Generate the waveform -----------------------------------------------*/
    checkErr(niFgen_InitiateGeneration(vi));

    /*- Update the control values -------------------------------------------*/
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
  Display a file select dialog box when pick file button is pushed
\****************************************************************************/
int CVICALLBACK pick_file (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViChar FileName[1024];
    switch (event)
        {
        case EVENT_COMMIT:
            if(FileSelectPopup (".", "*.bin", "", "Pick a binary file containing the waveform data.",
                                VAL_LOAD_BUTTON, 0, 0, 1, 0, FileName) <=0) {
                SetCtrlVal(gui, GUI_FILEPATH, "");
                return -1;
            }
            SetCtrlVal(gui, GUI_FILEPATH, FileName);
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

