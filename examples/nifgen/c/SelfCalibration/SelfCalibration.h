/**************************************************************************/
/* LabWindows/CVI User Interface Resource (UIR) Include File              */
/* Copyright (c) National Instruments 2007. All Rights Reserved.          */
/*                                                                        */
/* WARNING: Do not add to, delete from, or otherwise modify the contents  */
/*          of this include file.                                         */
/**************************************************************************/

#include <userint.h>

#ifdef __cplusplus
    extern "C" {
#endif

     /* Panels and Controls: */

#define  GUI                             1
#define  GUI_STOP                        2       /* callback function: stop */
#define  GUI_SETCALINFO                  3       /* callback function: setcalinfo */
#define  GUI_GETCALINFO                  4       /* callback function: getcalinfo */
#define  GUI_CALIBRATE                   5       /* callback function: calibrate */
#define  GUI_ERROR_MESSAGE               6
#define  GUI_USERINFO                    7
#define  GUI_LASTSELFCAL                 8
#define  GUI_NEWINFO                     9
#define  GUI_SUPPORTED                   10
#define  GUI_LASTTEMP                    11
#define  GUI_RESOURCE                    12


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK calibrate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK getcalinfo(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK setcalinfo(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stop(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
