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
#define  GUI_WAVEFORM                    2
#define  GUI_START_FREQ                  3
#define  GUI_END_FREQ                    4
#define  GUI_NUM_STEPS                   5
#define  GUI_GENERATE                    6       /* callback function: run */
#define  GUI_DURATION                    7
#define  GUI_AMPLITUDE                   8
#define  GUI_DC_OFFSET                   9
#define  GUI_CHANNEL                     10
#define  GUI_STOP                        11      /* callback function: stop */
#define  GUI_ERROR_MESSAGE               12
#define  GUI_RESOURCE                    13
#define  GUI_DECORATION                  14


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK run(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stop(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
