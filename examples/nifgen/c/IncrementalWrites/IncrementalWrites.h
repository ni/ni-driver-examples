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
#define  GUI_CHANNEL                     3
#define  GUI_GENERATE                    4       /* callback function: generate */
#define  GUI_ERROR_MESSAGE               5
#define  GUI_NUM_CHUNKS                  6
#define  GUI_WAVEFORM_SIZE               7
#define  GUI_SAMPLE_RATE                 8
#define  GUI_DECORATION_3                9
#define  GUI_WAVEFORM                    10
#define  GUI_REVERSE                     11
#define  GUI_RESOURCE                    12


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stop(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
