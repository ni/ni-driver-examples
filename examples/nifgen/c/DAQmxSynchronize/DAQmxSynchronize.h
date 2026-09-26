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
#define  FGEN_PANEL_ERROR_MESSAGE        2
#define  FGEN_PANEL_STOP                 3       /* callback function: stop */
#define  FGEN_PANEL_START                4       /* callback function: generate */
#define  FGEN_PANEL_NUM_SAMPLES          5
#define  FGEN_PANEL_ACQUISITION_RATE     6
#define  FGEN_PANEL_FREQUENCY            7
#define  FGEN_PANEL_AMPLITUDE            8
#define  FGEN_PANEL_WAVEFORM             9
#define  FGEN_PANEL_DIVISOR              10
#define  FGEN_PANEL_SAMPLE_RATE          11
#define  FGEN_PANEL_DAQMX_TRIG_SOURCE    12
#define  FGEN_PANEL_ARB_TRIG_SOURCE      13
#define  FGEN_PANEL_DAQMX_SCLK_SOURCE    14
#define  FGEN_PANEL_DAQMX_CHANNEL        15
#define  FGEN_PANEL_TEXTMSG              16
#define  FGEN_PANEL_RESOURCE             17
#define  FGEN_PANEL_STRIPCHART           18


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stop(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
