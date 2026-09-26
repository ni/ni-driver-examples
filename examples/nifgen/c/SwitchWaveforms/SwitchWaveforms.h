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
#define  GUI_SWITCH_WFMS                 3       /* callback function: switchWaveforms */
#define  GUI_DECORATION_2                4
#define  GUI_DECORATION                  5
#define  GUI_WFM2_TYPE                   6
#define  GUI_WFM1_TYPE                   7
#define  GUI_GEN_WFM2                    8
#define  GUI_GEN_WFM1                    9
#define  GUI_GENERATE                    10      /* callback function: generate */
#define  GUI_WFM2_SCALING                11
#define  GUI_WFM1_SCALING                12
#define  GUI_RESOURCE                    13


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK stop(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK switchWaveforms(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
