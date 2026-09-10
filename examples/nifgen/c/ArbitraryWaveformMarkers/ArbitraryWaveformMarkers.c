/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Arbitrary Waveform Markers Example source file                            */
/*                                                                           */
/* National Instruments, Austin Texas                                        */
/* PH. (800)433-3488   Fax (512)794-5678                                     */
/*****************************************************************************/
#include <utility.h>
#include <ansi_c.h>
#include <cvirte.h>     /* Needed if linking in external compiler; harmless otherwise */
#include <userint.h>
#include <stdio.h>
#include "nifgen.h"
#include "ArbitraryWaveformMarkers.h"
#include "niModInstCustCtrl.h"

static int gui;
ViSession vi=VI_NULL;
ViStatus error = VI_SUCCESS;
void wfmGenerate(void);
void ErrorBox(void);

/****************************************************************************\
  Function for displaying messages in the error box
\****************************************************************************/
void ErrorBox() {
    ViUInt32 errMsgSize;
    ViChar*  errMsg;
    if(error <0) {
        errMsgSize = niFgen_GetError(vi, VI_NULL, 0, VI_NULL);
        errMsg = (ViChar *) malloc(sizeof(ViChar) * errMsgSize);
        niFgen_GetError(vi, &error, errMsgSize, errMsg);
        MessagePopup ("Error", errMsg);
        free(errMsg);
    }
}


/****************************************************************************\
  Starts the interactive panel
\****************************************************************************/
int main (int argc, char *argv[])
{
    if (InitCVIRTE (0, argv, 0) == 0)   /* Needed if linking in external compiler; harmless otherwise */
        return -1;  /* out of memory */
    if ((gui = LoadPanel (0, "ArbitraryWaveformMarkers.uir", GUI)) < 0)
        return -1;
    niModInstCVICust_NewCtrl (gui, GUI_RESOURCE, "nifgen");
    DisplayPanel (gui);
    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (gui, GUI_RESOURCE);
    return 0;
}

/****************************************************************************\
  This reads all control values and starts waveform generation
\****************************************************************************/
void wfmGenerate() { 
    ViChar   resource[256], wfmMarkerTerminal[256], dataMarkerTerminal[256];
    ViChar   activeDataMarker[20];
    ViInt32  wfmType, wfmNumPoints, wfmMarkerPos, wfmHandle, i;
    ViInt32  wfmMarkerOutputBehavior, activeDataMarkerNumber, dataMarkerBitNum;    
    ViReal64 wfmAmplitude;
    ViInt16* wfmData = VI_NULL;
    ViInt32  analogMask = 0xFFF0;
    
    /*- Read in all of the control values ----------------------------------*/
    GetCtrlVal(gui, GUI_RESOURCE, resource);
    GetCtrlVal(gui, GUI_WFM_TYPE, &wfmType);
    GetCtrlVal(gui, GUI_WFM_AMPLITUDE, &wfmAmplitude);
    GetCtrlVal(gui, GUI_WFM_NUM_POINTS, &wfmNumPoints);
    GetCtrlVal(gui, GUI_WFM_MARKER_POS, &wfmMarkerPos);
    GetCtrlVal(gui, GUI_WFM_MARKER_TERMINAL, wfmMarkerTerminal);
    GetCtrlVal(gui, GUI_WFM_MARKER_OUT_BEHAVE, &wfmMarkerOutputBehavior);
    GetCtrlVal(gui, GUI_ACTIVE_DATA_MARKER, &activeDataMarkerNumber);
    GetCtrlVal(gui, GUI_DATA_MARKER_BIT_NUM, &dataMarkerBitNum);
    GetCtrlVal(gui, GUI_DATA_MARKER_TERMINAL, dataMarkerTerminal);
    if(vi != VI_NULL) {
        checkErr(niFgen_close(vi));
    }

    /*- Prepare the waveform data ------------------------------------------*/
    wfmData = malloc(wfmNumPoints * sizeof(ViInt16));
    
	if (wfmType == 1) {
        // Sine:
	    for (i = 0; i < wfmNumPoints; i++)
	        wfmData[i] = sin(((ViReal64)i/wfmNumPoints)*2*3.141596)*32767;
	
	} else if (wfmType == 2) {
	    // Square
	    for (i = 0; i < wfmNumPoints; i++) {
	        if (i < wfmNumPoints/2) wfmData[i] = 32767;
	        else wfmData[i] = -32767;
	    }
	
	} else {    
	    // Triangle
	    for (i = 0; i < wfmNumPoints/2; i++)
	        wfmData[i] = ((ViReal64)i/wfmNumPoints*4 - 1)*32767;
	    for (i = wfmNumPoints/2; i < wfmNumPoints; i++)
	        wfmData[i] = (-(ViReal64)(i-wfmNumPoints/2)/wfmNumPoints*4 + 1)*32767;
	}
	
	// Mask out the bottom bits and replace them with a ramp pattern
	for (i = 0; i < wfmNumPoints; i++) {
		wfmData[i] = (wfmData[i] & analogMask) | ((i % 8) & ~analogMask);
	}
	
    /*- Initialize the session ---------------------------------------------*/
    checkErr(niFgen_init(resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Configure output mode for arbitrary waveform -----------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_ARB));

    /*- Create the arbitrary waveform --------------------------------------*/
	checkErr(niFgen_CreateWaveformI16 (vi, VI_NULL, wfmNumPoints, wfmData,
									   &wfmHandle));
	
    /*- Configure the waveform marker --------------------------------------*/
	checkErr(niFgen_SetAttributeViInt32(vi, VI_NULL, NIFGEN_ATTR_ARB_MARKER_POSITION, 
										wfmMarkerPos));
	checkErr(niFgen_SetAttributeViString(vi, "marker0", NIFGEN_ATTR_MARKER_EVENT_OUTPUT_TERMINAL, 
										 wfmMarkerTerminal));
	checkErr(niFgen_SetAttributeViInt32(vi, "marker0", NIFGEN_ATTR_MARKER_EVENT_OUTPUT_BEHAVIOR,
										wfmMarkerOutputBehavior));
										
    /*- Configure the data marker ------------------------------------------*/
    if (strcmp(dataMarkerTerminal, "")) {
		sprintf(activeDataMarker, "datamarker%d", activeDataMarkerNumber);										
		checkErr(niFgen_SetAttributeViInt32(vi, activeDataMarker, 
											NIFGEN_ATTR_DATA_MARKER_EVENT_DATA_BIT_NUMBER,
											dataMarkerBitNum));
		checkErr(niFgen_SetAttributeViString(vi, activeDataMarker, 
											NIFGEN_ATTR_DATA_MARKER_EVENT_OUTPUT_TERMINAL,
											dataMarkerTerminal));
	}
	
    /*- Configure the analog data mask -------------------------------------*/
	checkErr(niFgen_SetAttributeViInt32(vi, VI_NULL, NIFGEN_ATTR_ANALOG_DATA_MASK, analogMask));
	checkErr(niFgen_SetAttributeViInt32(vi, VI_NULL, NIFGEN_ATTR_ANALOG_STATIC_VALUE, 0));

    /*- Generate the waveform -------------------------------------*/
    checkErr(niFgen_InitiateGeneration(vi));

    /*- Update the control values ------------------------------------------*/
    SetCtrlVal(gui, GUI_GENERATE, VI_TRUE);

Error:
    //free the memory  before returning
    if(wfmData != VI_NULL) {
        free(wfmData);
        wfmData = VI_NULL;
    }

    /*- Display any errors -------------------------------------------------*/
    ErrorBox();
    if((error < 0) && (vi != VI_NULL)) {
        niFgen_close(vi);
        vi = VI_NULL;
        SetCtrlVal(gui, GUI_GENERATE, VI_FALSE);
    }
}

/****************************************************************************\
  Close the session and exit the program when the stop button is pushed
\****************************************************************************/
int CVICALLBACK stop (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    switch (event)
        {
        case EVENT_COMMIT:
            if(vi != VI_NULL) {
                niFgen_AbortGeneration(vi);
                niFgen_close(vi);
            }
            QuitUserInterface (0);
            break;
        }
    return 0;
}

/****************************************************************************\
  Start generation when the generate button is pushed
  Close the session when the generate button is released
\****************************************************************************/
int CVICALLBACK generate (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2) {
    int Generate;
    switch (event)
        {
        case EVENT_COMMIT:
            GetCtrlVal(gui, GUI_GENERATE, &Generate);
            if(Generate) {
                wfmGenerate();
            }
            else {
                if(vi != VI_NULL) {
                    checkErr(niFgen_AbortGeneration(vi));
                    checkErr(niFgen_close(vi));
                    vi=VI_NULL;
Error:
                    ErrorBox();
                }
            }
            break;
        }
    return 0;
}
