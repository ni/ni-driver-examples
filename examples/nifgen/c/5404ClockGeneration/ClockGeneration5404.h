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
#define  PANEL_QUIT                      2       /* callback function: quit */
#define  PANEL_FREQUENCY                 3       /* callback function: freqChange */
#define  PANEL_RUNNING                   4
#define  PANEL_ERROR                     5
#define  PANEL_DUTYCYCLE                 6       /* callback function: dutyChange */
#define  PANEL_AMPLITUDE                 7       /* callback function: ampChange */
#define  PANEL_SESSION                   8       /* callback function: sessionToggle */
#define  PANEL_GENERATE                  9       /* callback function: generateToggle */
#define  PANEL_RESOURCE                  10
#define  PANEL_DECORATION                11
#define  PANEL_DECORATION_2              12
#define  PANEL_DECORATION_3              13
#define  PANEL_DECORATION_4              14
#define  PANEL_DECORATION_5              15


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK ampChange(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK dutyChange(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK freqChange(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK generateToggle(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK quit(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK sessionToggle(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
