#include <ansi_c.h>
/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Basic Standard Function Example source file                               */
/*                                                                           */
/* National Instruments, Austin Texas                                        */
/* PH. (800)433-3488   Fax (512)794-5678                                     */
/* Original Release: 4-99                                                    */
/*                                                                           */
/* Modification History:                                                     */
/*     Date    Initials  Description                                         */
/*     8-03    DC        Created                                             */
/*****************************************************************************/

#include <userint.h>
#include "BasicStandardFunction.h"
#include <cvirte.h> /* Needed if linking in external compiler; harmless otherwise */
#include "nifgen.h"
#include "niModInstCustCtrl.h"

static int fgenPanel;
static ViSession vi = VI_NULL;
static ViStatus error = VI_SUCCESS;
void func_gen(void);
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
        ResetTextBox(fgenPanel, FGEN_PANEL_ERROR_MESSAGE, errMsg);
        free(errMsg);
    }
    else if(error == VI_SUCCESS) ResetTextBox(fgenPanel, FGEN_PANEL_ERROR_MESSAGE, "");
}

/****************************************************************************\
  Starts the interative pannel
\****************************************************************************/
int main (int argc, char *argv[])
{
    if (InitCVIRTE (0, argv, 0) == 0)   /* Needed if linking in external compiler; harmless otherwise */
        return -1;  /* out of memory */
    if ((fgenPanel = LoadPanel (0, "BasicStandardFunction.uir", FGEN_PANEL)) < 0)
        return -1;
    niModInstCVICust_NewCtrl (fgenPanel, FGEN_PANEL_RESOURCE, "nifgen");        
    DisplayPanel (fgenPanel);
    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (fgenPanel, FGEN_PANEL_RESOURCE);    
    return 0;
}

/****************************************************************************\
  This reads all control values and starts waveform generation
\****************************************************************************/
void func_gen() {
    ViChar Resource[256], Channel[256];
    ViReal64 Frequency, Amplitude;
    ViInt32  Waveform;

    /*- Read all clontrol values -------------------------------------------*/
    GetCtrlVal(fgenPanel, FGEN_PANEL_RESOURCE, Resource);
    GetCtrlVal(fgenPanel, FGEN_PANEL_CHANNEL, Channel);
    GetCtrlVal(fgenPanel, FGEN_PANEL_FREQUENCY, &Frequency);
    GetCtrlVal(fgenPanel, FGEN_PANEL_AMPLITUDE, &Amplitude);
    GetCtrlVal(fgenPanel, FGEN_PANEL_WAVEFORM, &Waveform);
   
    if(vi != VI_NULL) {
        checkErr(niFgen_close(vi));
    }

    /*- Initialize the session ---------------------------------------------*/
    checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Configure output mode for standard functions -----------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_FUNC));
    
    /*- Select waveform to generate ----------------------------------------*/
    checkErr(niFgen_ConfigureStandardWaveform(vi, Channel, Waveform,
                                             Amplitude, 0, Frequency,
                                             0)); 
    
    /*- Start generating ---------------------------------------------------*/
    checkErr(niFgen_InitiateGeneration(vi));
    
    SetCtrlVal(fgenPanel, FGEN_PANEL_GENERATE, VI_TRUE);
Error:
	ErrorBox();
    /*- Update generate button---------------------------------------------*/
    if (error < 0) {
    	SetCtrlVal(fgenPanel, FGEN_PANEL_GENERATE, VI_FALSE);
    }
    return;
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
            if (vi != VI_NULL) {
                niFgen_AbortGeneration(vi);
                niFgen_close(vi);
                vi=VI_NULL;
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
            GetCtrlVal(fgenPanel, FGEN_PANEL_GENERATE, &Generate);
            if(Generate) {
                func_gen();
            }
            else {
                if(vi != VI_NULL) {
                    checkErr(niFgen_AbortGeneration(vi));
                    checkErr(niFgen_close(vi));
                    vi=VI_NULL;
Error:
                    ErrorBox();
            		/*- Update generate button---------------------------------------------*/
    				if((error < 0) && (vi != VI_NULL)) {
                		niFgen_close(vi);
                		vi = VI_NULL;
            		}
                }
            }
    };
    return 0;
}

