/**************************************************************************/
/* LabWindows/CVI User Interface Resource (UIR) Include File              */
/* Copyright (c) National Instruments 2008. All Rights Reserved.          */
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
#define  GUI_STOP                         2       /* callback function: stop */
#define  GUI_CHANNEL                      3
#define  GUI_GENERATE                     4       /* callback function: generate */
#define  GUI_ERROR_MESSAGE                5
#define  GUI_SAMPLE_RATE                  6
#define  GUI_DECORATION_3                 7
#define  GUI_GRAPH                        8
#define  GUI_TRIG_MODE                    9
#define  GUI_TRIG_TYPE                    10
#define  GUI_TRIG_SRC                     11
#define  GUI_RESOURCE                     12
#define  GUI_SW_TRIG                      13      /* callback function: sw_trig */
#define  GUI_GAIN                         14


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */

int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stop(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK sw_trig(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
