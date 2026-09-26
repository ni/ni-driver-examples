/**************************************************************************/
/* LabWindows/CVI User Interface Resource (UIR) Include File              */
/* Copyright (c) National Instruments 2011. All Rights Reserved.          */
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
#define  GUI_STOP                         2       /* control type: command, callback function: stop */
#define  GUI_GENERATE                     3       /* control type: textButton, callback function: generate */
#define  GUI_DECORATION                   4       /* control type: deco, callback function: (none) */
#define  GUI_WRITE_IN_N_BLOCKS            5       /* control type: numeric, callback function: (none) */
#define  GUI_WAVEFORM_REPEAT_COUNT        6       /* control type: numeric, callback function: (none) */
#define  GUI_WAVEFORM_SIZE                7       /* control type: numeric, callback function: (none) */
#define  GUI_SAMPLE_RATE                  8       /* control type: numeric, callback function: (none) */
#define  GUI_BLOCKS_WRITTEN               9       /* control type: scale, callback function: (none) */
#define  GUI_TIMER                        10      /* control type: timer, callback function: writeNewData */
#define  GUI_RESOURCE                     11      /* control type: string, callback function: (none) */


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */

int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stop(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK writeNewData(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
