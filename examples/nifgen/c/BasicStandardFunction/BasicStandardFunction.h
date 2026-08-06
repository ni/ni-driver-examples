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

#define  FGEN_PANEL                      1
#define  FGEN_PANEL_WAVEFORM             2
#define  FGEN_PANEL_CHANNEL              3
#define  FGEN_PANEL_ERROR_MESSAGE        4
#define  FGEN_PANEL_STOP                 5       /* callback function: stop */
#define  FGEN_PANEL_GENERATE             6       /* callback function: generate */
#define  FGEN_PANEL_DECORATION           7
#define  FGEN_PANEL_FREQUENCY            8
#define  FGEN_PANEL_AMPLITUDE            9
#define  FGEN_PANEL_RESOURCE             10


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stop(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
