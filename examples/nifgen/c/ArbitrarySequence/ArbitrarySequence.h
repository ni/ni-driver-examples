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
#define  GUI_DC_OFFSET                   4       /* callback function: dc_offset */
#define  GUI_GAIN                        5       /* callback function: gain */
#define  GUI_WFM_FILE_3                  6
#define  GUI_WFM_FILE_2                  7
#define  GUI_PICK_WFM_FILE_3             8       /* callback function: pick_wfm_file_3 */
#define  GUI_WFM_FILE_1                  9
#define  GUI_PICK_WFM_FILE_2             10      /* callback function: pick_wfm_file_2 */
#define  GUI_PICK_WFM_FILE_1             11      /* callback function: pick_wfm_file_1 */
#define  GUI_FILTER_ENABLE               12      /* callback function: filter_enable */
#define  GUI_QUIT                        13      /* callback function: quit */
#define  GUI_CHANNEL                     14
#define  GUI_GENERATE                    15      /* callback function: generate */
#define  GUI_ERROR_MESSAGE               16
#define  GUI_SAMPLE_RATE                 17
#define  GUI_TRIGGER_MODE                18
#define  GUI_TRIGGER_TYPE                19
#define  GUI_TRIGGER_SOURCE              20
#define  GUI_SW_TRIG                     21      /* callback function: sw_trig */
#define  GUI_LOOP_COUNT_3                22
#define  GUI_MARKER_3                    23
#define  GUI_DECORATION_3                24
#define  GUI_DECORATION_4                25
#define  GUI_LOOP_COUNT_2                26
#define  GUI_MARKER_2                    27
#define  GUI_DECORATION_5                28
#define  GUI_DECORATION                  29
#define  GUI_LOOP_COUNT_1                30
#define  GUI_MARKER_1                    31
#define  GUI_RESOURCE                    32


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */ 

int  CVICALLBACK dc_offset(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK filter_enable(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK gain(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK generate(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK pick_wfm_file_1(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK pick_wfm_file_2(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK pick_wfm_file_3(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK quit(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);
int  CVICALLBACK sw_trig(int panel, int control, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif
