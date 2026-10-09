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
#define  GUI_TONES_FILE                  2       /* callback function: tones_file */
#define  GUI_PICK_TONES_FILE             3       /* callback function: pick_tones_file */
#define  GUI_DC_OFFSET                   4       /* callback function: dc_offset */
#define  GUI_EDIT_TONES_FILE             5       /* callback function: edit_tones_file */
#define  GUI_QUIT                        6       /* callback function: quit */
#define  GUI_CHANNEL                     7
#define  GUI_GENERATE                    8       /* callback function: generate */
#define  GUI_NUM_SAMPLES                 9
#define  GUI_SAMPLE_RATE                 10
#define  GUI_ANALOG_FILTER               11      /* callback function: analog_filter */
#define  GUI_DIGITAL_FILTER              12      /* callback function: digital_filter */
#define  GUI_DECORATION_2                13
#define  GUI_DECORATION_4                14
#define  GUI_TEXTMSG_2                   15
#define  GUI_RESOURCE                    16


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK analog_filter(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK dc_offset(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK digital_filter(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK edit_tones_file(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK pick_tones_file(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK quit(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK tones_file(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
