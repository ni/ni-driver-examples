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
#define  GUI_GENERATE                    3       /* callback function: generate */
#define  GUI_DECORATION                  4
#define  GUI_SCRIPT                      5
#define  GUI_SCRIPT_NUM                  6       /* callback function: changeScript */
#define  GUI_SCRIPT_TRIG_TYPE            7
#define  GUI_SCRIPT_TRIG_SOURCE          8
#define  GUI_SEND_SW_TRIG                9       /* callback function: sendSWTrigger */
#define  GUI_RESOURCE                    10


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK changeScript(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK sendSWTrigger(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stop(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
