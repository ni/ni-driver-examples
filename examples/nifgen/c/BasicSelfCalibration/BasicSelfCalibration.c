/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Basic Self Calibration Example source file                                */
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
#include "BasicSelfCalibration.h"
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
    if ((gui = LoadPanel (0, "BasicSelfCalibration.uir", GUI)) < 0)
        return -1;
    niModInstCVICust_NewCtrl (gui, GUI_RESOURCE, "nifgen");        
    DisplayPanel (gui);
    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (gui, GUI_RESOURCE);    
    return 0;
}

/****************************************************************************\
  Calibrate the device
\****************************************************************************/
int CVICALLBACK calibrate (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViChar Resource[256];
    ViBoolean SelfCalSupported;
    switch (event)
        {
        case EVENT_COMMIT:
            /*- Get all the control values -------------------------------------------*/
            GetCtrlVal(gui, GUI_RESOURCE, Resource);
            
            checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));
            checkErr(niFgen_GetSelfCalSupported (vi, &SelfCalSupported));
            if (SelfCalSupported) 
                checkErr(niFgen_SelfCal(vi));
            else
                SetCtrlVal(gui, GUI_ERROR_MESSAGE, 
                    "Self Calibration not supported for this device.");
            checkErr(niFgen_close(vi));
            vi = VI_NULL;
            
Error:
            SetCtrlVal(gui, GUI_CALIBRATE, 0);
            if (error != VI_SUCCESS) ErrorBox();
            if((error != VI_SUCCESS) && (vi != VI_NULL)) {
                niFgen_close(vi);
                vi = VI_NULL;
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
                vi = VI_NULL;
            }
            QuitUserInterface (0);
            break;
        }
    return 0;
}
