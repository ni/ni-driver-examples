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

#define  PANEL                           1
#define  PANEL_SESSION                   2       /* callback function: sessionToggle */
#define  PANEL_AMPLITUDE                 3
#define  PANEL_STARTFREQ                 4
#define  PANEL_STOPFREQ                  5
#define  PANEL_STEPS                     6
#define  PANEL_STEPMETHOD                7       /* callback function: stepMethod */
#define  PANEL_SINGLESTEP                8       /* callback function: singleStep */
#define  PANEL_TIME                      9
#define  PANEL_ERROR                     10
#define  PANEL_QUIT                      11      /* callback function: quit */
#define  PANEL_RUNNING                   12
#define  PANEL_GENERATE                  13      /* callback function: generate */
#define  PANEL_DECORATION                14
#define  PANEL_RESOURCE                  15
#define  PANEL_DECORATION_3              16
#define  PANEL_DECORATION_4              17
#define  PANEL_DECORATION_5              18
#define  PANEL_DECORATION_2              19
#define  PANEL_DECORATION_6              20
#define  PANEL_DECORATION_7              21
#define  PANEL_DECORATION_8              22
#define  PANEL_DECORATION_9              23


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK quit(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK sessionToggle(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK singleStep(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stepMethod(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
