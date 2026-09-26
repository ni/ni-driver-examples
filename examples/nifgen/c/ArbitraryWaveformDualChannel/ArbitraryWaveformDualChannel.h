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

#define  GUI                             1
#define  GUI_SAMPLE_CLOCK                2
#define  GUI_CLOCK_MODE                  3
#define  GUI_SAMPLE_RATE                 4
#define  GUI_GAIN                        5       /* callback function: gain */
#define  GUI_ERROR_MESSAGE               6
#define  GUI_STOP                        7       /* callback function: stop */
#define  GUI_GENERATE                    8       /* callback function: generate */
#define  GUI_RESOURCE                    9
#define  GUI_GRAPH                       10
#define  GUI_WFM_CH1                     11      /* callback function: waveform */
#define  GUI_WFM_CH0                     12      /* callback function: waveform */
#define  GUI_DECORATION                  13
#define  GUI_DECORATION_2                14


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK gain(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stop(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK waveform(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
