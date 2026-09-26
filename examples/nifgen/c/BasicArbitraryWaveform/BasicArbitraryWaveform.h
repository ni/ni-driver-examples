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
#define  GUI_DC_OFFSET                   2       /* callback function: dc_offset */
#define  GUI_GAIN                        3       /* callback function: gain */
#define  GUI_STOP                        4       /* callback function: stop */
#define  GUI_CHANNEL                     5
#define  GUI_GENERATE                    6       /* callback function: generate */
#define  GUI_ERROR_MESSAGE               7
#define  GUI_SAMPLE_RATE                 8
#define  GUI_DECORATION_3                9
#define  GUI_DECORATION_4                10
#define  GUI_GRAPH                       11
#define  GUI_WAVEFORM                    12      /* callback function: waveform */
#define  GUI_RESOURCE                    13


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK dc_offset(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK gain(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stop(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK waveform(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
