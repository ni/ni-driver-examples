/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Self Calibration Example source file                                      */
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
#include "SelfCalibration.h"
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
  Starts the interactive panel
\****************************************************************************/
int main (int argc, char *argv[])
{
    ViInt32 i;
    if (InitCVIRTE (0, argv, 0) == 0)   /* Needed if linking in external compiler; harmless otherwise */
        return -1;  /* out of memory */
    if ((gui = LoadPanel (0, "SelfCalibration.uir", GUI)) < 0)
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
            if (SelfCalSupported) {
                SetCtrlVal(gui, GUI_SUPPORTED, 1);
                checkErr(niFgen_SelfCal(vi));
            } else {
                SetCtrlVal(gui, GUI_SUPPORTED, 0);
                SetCtrlVal(gui, GUI_ERROR_MESSAGE, 
                    "Self Calibration not supported for this device.");
            }
            checkErr(niFgen_close(vi));
            
Error:
            SetCtrlVal(gui, GUI_CALIBRATE, 0);
            ErrorBox();
            if((error != VI_SUCCESS) && (vi != VI_NULL)) {
                niFgen_close(vi);
                vi = VI_NULL;
            }
            break;
        }
    return 0;
}

/****************************************************************************\
  Get User Defined Info from the board
\****************************************************************************/
int CVICALLBACK getcalinfo (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViChar Resource[256], *UserInfo, LastSelfCal[256];
    ViReal64 LastSelfCalTemp;
    ViInt32 UserInfoSize, Year, Month, Day, Hour, Minute;
    ViBoolean SelfCalSupported;
    switch (event)
        {
        case EVENT_COMMIT:
            /*- Get all the control values -------------------------------------------*/
            GetCtrlVal(gui, GUI_RESOURCE, Resource);
            
            checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));
            checkErr(niFgen_GetSelfCalSupported (vi, &SelfCalSupported));
            if (SelfCalSupported) {
                SetCtrlVal(gui, GUI_SUPPORTED, 1);

                /*- Get the size of the user info ------------------------------------*/
                checkErr(niFgen_GetCalUserDefinedInfoMaxSize(vi, &UserInfoSize));

                /*- Get the user defined info ----------------------------------------*/
                UserInfo = malloc(UserInfoSize+1);
                checkErr(niFgen_GetCalUserDefinedInfo(vi, UserInfo));

                /*- Get the date, time, hour, etc. of the last self calibraiont ------*/
                checkErr(niFgen_GetSelfCalLastDateAndTime (vi, &Year, &Month, 
                                                           &Day, &Hour, &Minute));

                /*- Get the board temperature of the last self calibration -----------*/
                checkErr(niFgen_GetSelfCalLastTemp(vi, &LastSelfCalTemp));
                
                /*- Update indicators ------------------------------------------------*/
                SetCtrlVal(gui, GUI_USERINFO, UserInfo);
                SetCtrlVal(gui, GUI_LASTTEMP, LastSelfCalTemp);
                sprintf(LastSelfCal, "%d/%d/%d, %d:%d", Month, Day, Year, Hour, Minute);
                SetCtrlVal(gui, GUI_LASTSELFCAL, LastSelfCal);
            } else {
                SetCtrlVal(gui, GUI_SUPPORTED, 0);
                SetCtrlVal(gui, GUI_ERROR_MESSAGE, 
                    "Self Calibration not supported for this device.");
            }
            checkErr(niFgen_close(vi));
            
Error:
            SetCtrlVal(gui, GUI_GETCALINFO, 0);
            ErrorBox();
            if((error != VI_SUCCESS) && (vi != VI_NULL)) {
                niFgen_close(vi);
                vi = VI_NULL;
            }
            break;
        }
    return 0;
}

/****************************************************************************\
  Set User Defined Calibration info
\****************************************************************************/
int CVICALLBACK setcalinfo (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViChar Resource[256], UserInfo[256];
    ViBoolean SelfCalSupported;
    switch (event)
        {
        case EVENT_COMMIT:
            /*- Get all the control values -------------------------------------------*/
            GetCtrlVal(gui, GUI_RESOURCE, Resource);
            GetCtrlVal(gui, GUI_NEWINFO, UserInfo);
            
            checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));
            checkErr(niFgen_GetSelfCalSupported (vi, &SelfCalSupported));
            if (SelfCalSupported) {
                SetCtrlVal(gui, GUI_SUPPORTED, 1);
                checkErr(niFgen_SetCalUserDefinedInfo(vi, UserInfo));
            } else {
                SetCtrlVal(gui, GUI_SUPPORTED, 0);
                SetCtrlVal(gui, GUI_ERROR_MESSAGE, 
                    "Self Calibration not supported for this device.");
            }
            checkErr(niFgen_close(vi));
            
Error:
            SetCtrlVal(gui, GUI_SETCALINFO, 0);
            ErrorBox();
            if((error != VI_SUCCESS) && (vi != VI_NULL)) {
                niFgen_close(vi);
                vi = VI_NULL;
            }
            break;
        }
    return 0;
}

/****************************************************************************\
  Exit the program when the stop button is pushed
\****************************************************************************/
int CVICALLBACK stop (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
        switch (event)
        {
        case EVENT_COMMIT:
            QuitUserInterface (0);
            break;
        }
    return 0;
}
