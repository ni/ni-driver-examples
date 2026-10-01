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
#define  GUI_FILEPATH                    3
#define  GUI_CHANNEL                     4
#define  GUI_GENERATE                    5       /* callback function: generate */
#define  GUI_ERROR_MESSAGE               6
#define  GUI_SAMPLE_RATE                 7
#define  GUI_DECORATION_4                8
#define  GUI_PICK_FILE                   9       /* callback function: pick_file */
#define  GUI_BYTE_ORDER                  10
#define  GUI_RESOURCE                    11


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK pick_file(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stop(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
