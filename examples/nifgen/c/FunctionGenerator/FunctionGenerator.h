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
#define  FGEN_PANEL_FREQUENCY            4       /* callback function: update_frequency */
#define  FGEN_PANEL_DC_OFFSET            5       /* callback function: update_dc_offset */
#define  FGEN_PANEL_AMPLITUDE            6       /* callback function: update_amplitude */
#define  FGEN_PANEL_ERROR_MESSAGE        7
#define  FGEN_PANEL_UPDATE               8       /* callback function: update */
#define  FGEN_PANEL_STOP                 9       /* callback function: stop */
#define  FGEN_PANEL_GENERATE             10      /* callback function: generate */
#define  FGEN_PANEL_RESOURCE             11
#define  FGEN_PANEL_DECORATION           12
#define  FGEN_PANEL_DECORATION_2         13


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stop(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK update(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK update_amplitude(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK update_dc_offset(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK update_frequency(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
