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
#define  GUI_WFM_TYPE                    5
#define  GUI_DATA_MARKER_BIT_NUM         6
#define  GUI_WFM_MARKER_POS              7
#define  GUI_WFM_NUM_POINTS              8
#define  GUI_WFM_AMPLITUDE               9
#define  GUI_DATA_MARKER_TERMINAL        10
#define  GUI_WFM_MARKER_TERMINAL         11
#define  GUI_TEXTMSG                     12
#define  GUI_WFM_MARKER_OUT_BEHAVE       13
#define  GUI_DECORATION_2                14
#define  GUI_ACTIVE_DATA_MARKER          15
#define  GUI_RESOURCE                    16
#define  GUI_DECORATION_3                17


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stop(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
