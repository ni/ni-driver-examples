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
#define  PANEL_AMPLITUDE                 2
#define  PANEL_FREQUENCY                 3
#define  PANEL_PHASE                     4       /* callback function: phaseChange */
#define  PANEL_ERROR                     5
#define  PANEL_RUNNING                   6
#define  PANEL_QUIT                      7       /* callback function: quit */
#define  PANEL_SESSION                   8       /* callback function: sessionToggle */
#define  PANEL_GENERATE                  9       /* callback function: generate */
#define  PANEL_RESOURCE1                 10
#define  PANEL_RESOURCE2                 11
#define  PANEL_DECORATION                12
#define  PANEL_DECORATION_2              13
#define  PANEL_DECORATION_3              14
#define  PANEL_DECORATION_4              15
#define  PANEL_DECORATION_5              16
#define  PANEL_DECORATION_6              17


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK phaseChange(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK quit(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK sessionToggle(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
