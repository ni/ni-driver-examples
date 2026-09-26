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
#define  GUI_SAMPLE_CLOCK                2
#define  GUI_CLOCK_MODE                  3
#define  GUI_WFM_FILE                    4
#define  GUI_PICK_FILE                   5       /* callback function: pick_file */
#define  GUI_SAMPLE_RATE                 6       /* callback function: sample_rate */
#define  GUI_GAIN                        7       /* callback function: gain */
#define  GUI_ERROR_MESSAGE               8
#define  GUI_STOP                        9       /* callback function: stop */
#define  GUI_FILTER_ENABLE               10      /* callback function: filter_enable */
#define  GUI_CHANNEL                     11
#define  GUI_GENERATE                    12      /* callback function: generate */
#define  GUI_RESOURCE                    13
#define  GUI_DECORATION                  14
#define  GUI_DECORATION_2                15


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK filter_enable(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK gain(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK pick_file(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK sample_rate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stop(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
