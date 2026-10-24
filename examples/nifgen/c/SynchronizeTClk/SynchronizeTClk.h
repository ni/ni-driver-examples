/**************************************************************************/
/* LabWindows/CVI User Interface Resource (UIR) Include File              */
/* Copyright (c) National Instruments 2010. All Rights Reserved.          */
/*                                                                        */
/* WARNING: Do not add to, delete from, or otherwise modify the contents  */
/*          of this include file.                                         */
/**************************************************************************/

#include <userint.h>

#ifdef __cplusplus
    extern "C" {
#endif

     /* Panels and Controls: */

#define  GUI                              1
#define  GUI_RESOURCES                    2       /* control type: string, callback function: (none) */
#define  GUI_TEXTMSG_2                    3       /* control type: textMsg, callback function: (none) */
#define  GUI_TEXTMSG                      4       /* control type: textMsg, callback function: (none) */
#define  GUI_DECORATION                   5       /* control type: deco, callback function: (none) */
#define  GUI_SAMPLE_RATE                  6       /* control type: numeric, callback function: (none) */
#define  GUI_QUIT                         7       /* control type: command, callback function: Quit */
#define  GUI_GO                           8       /* control type: textButton, callback function: Go */
#define  GUI_DECORATION_2                 9       /* control type: deco, callback function: (none) */


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */

int  CVICALLBACK Go(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK Quit(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
