/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Arbitrary Waveform Streaming Example source file                          */
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
#include "ArbitraryWaveformStreaming.h"
#include "niModInstCustCtrl.h"

static int gui;
ViSession vi=VI_NULL;
ViStatus error = VI_SUCCESS;
ViInt32 numPoints, wfmHandle, blocksWritten, streamingWaveformRepeatCount, writeInNBlocks;
ViInt16* wfmData = 0;
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
  Starts the interative pannel
\****************************************************************************/
int main (int argc, char *argv[])
{
    if (InitCVIRTE (0, argv, 0) == 0)   /* Needed if linking in external compiler; harmless otherwise */
        return -1;  /* out of memory */
    if ((gui = LoadPanel (0, "ArbitraryWaveformStreaming.uir", GUI)) < 0)
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
    ViChar   resource[256];
	ViInt32	 streamingWaveformSize, i;
	ViReal64 sampleRate;
    
    /*- Read in all of the control values ----------------------------------*/
    GetCtrlVal(gui, GUI_RESOURCE, resource);
    GetCtrlVal(gui, GUI_SAMPLE_RATE, &sampleRate);
    GetCtrlVal(gui, GUI_WAVEFORM_SIZE, &streamingWaveformSize);
    GetCtrlVal(gui, GUI_WAVEFORM_REPEAT_COUNT, &streamingWaveformRepeatCount);
    GetCtrlVal(gui, GUI_WRITE_IN_N_BLOCKS, &writeInNBlocks);
    if(vi != VI_NULL) {
        checkErr(niFgen_close(vi));
    }

    /*- Prepare the waveform data ------------------------------------------*/
    numPoints = streamingWaveformSize / writeInNBlocks;
    wfmData = malloc(numPoints * sizeof(ViInt16));
    
    // Sine wave:
    for (i = 0; i < numPoints; i++)
        wfmData[i] = sin(((ViReal64)i/numPoints)*2*3.141596)*32767;

    /*- Initialize the session ---------------------------------------------*/
    checkErr(niFgen_init(resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Configure output mode for arbitrary waveform -----------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_ARB));

    /*- Configure the Sample Rate ------------------------------------------*/
	checkErr(niFgen_ConfigureSampleRate(vi, sampleRate));	

    /*- Allocate and configure the streaming waveform ----------------------*/
	checkErr(niFgen_AllocateWaveform(vi, VI_NULL, streamingWaveformSize, &wfmHandle));
	checkErr(niFgen_SetAttributeViInt32(vi, VI_NULL, NIFGEN_ATTR_STREAMING_WAVEFORM_HANDLE,
										wfmHandle));
	checkErr(niFgen_SetAttributeViInt32(vi, VI_NULL, NIFGEN_ATTR_TRIGGER_MODE, 
										NIFGEN_VAL_SINGLE));
	checkErr(niFgen_SetAttributeViInt32(vi, VI_NULL, NIFGEN_ATTR_ARB_REPEAT_COUNT,
										streamingWaveformRepeatCount));

    /*- Configure the maximum timeout value (in seconds) to wait during write waveform --*/
    checkErr(niFgen_SetAttributeViReal64(vi, VI_NULL, NIFGEN_ATTR_STREAMING_WRITE_TIMEOUT, 10.0));

    /*- Fill up the streaming waveform with data ---------------------------*/
    for (i = 0; i < writeInNBlocks; i++) {
		checkErr(niFgen_WriteBinary16Waveform (vi, VI_NULL, wfmHandle, numPoints,
											   wfmData));
	}
	blocksWritten = writeInNBlocks;
    

    /*- Generate the waveform ----------------------------------------------*/
    checkErr(niFgen_InitiateGeneration(vi));
	SetCtrlAttribute(gui, GUI_TIMER, ATTR_ENABLED, 1);

    /*- Update the control values ------------------------------------------*/
    SetCtrlAttribute(gui, GUI_BLOCKS_WRITTEN, ATTR_MAX_VALUE, writeInNBlocks * streamingWaveformRepeatCount);
    SetCtrlVal(gui, GUI_BLOCKS_WRITTEN, blocksWritten);
    SetCtrlVal(gui, GUI_GENERATE, VI_TRUE);
    

Error:
    /*- Display any errors -------------------------------------------------*/
    ErrorBox();
    if((error < 0) && (vi != VI_NULL)) {
		free(wfmData);
		wfmData = 0;
        niFgen_close(vi);
        vi = VI_NULL;
        SetCtrlVal(gui, GUI_GENERATE, VI_FALSE);
    }
}

/****************************************************************************\
  Fill up the streaming waveform with fresh data
\****************************************************************************/
int CVICALLBACK writeNewData (int panel, int control, int event, 
		void *callbackData, int eventData1, int eventData2) 
{
    switch (event)
        {
        case EVENT_TIMER_TICK:
            if(vi != VI_NULL) {
			    
				checkErr(niFgen_WriteBinary16Waveform(vi, VI_NULL, wfmHandle, 
							 							  numPoints, wfmData));
				blocksWritten++;
				SetCtrlVal(gui, GUI_BLOCKS_WRITTEN, blocksWritten);

			    /*- If we're done writing, close the session ---------------*/
    			if (blocksWritten >= writeInNBlocks * streamingWaveformRepeatCount) {
					SetCtrlAttribute(gui, GUI_TIMER, ATTR_ENABLED, 0);
					checkErr(niFgen_WaitUntilDone(vi, 10000));   
					checkErr(niFgen_AbortGeneration(vi));
					checkErr(niFgen_close(vi));
					vi = VI_NULL;
					/* free the waveform data since we are done with it! */
					free(wfmData);
					wfmData = 0;
					SetCtrlVal(gui, GUI_GENERATE, VI_FALSE);
    			}
            }
            break;
        }
Error:
	ErrorBox();
	if((error < 0) && (vi != VI_NULL)) {
		free(wfmData);
		wfmData = 0;
		SetCtrlAttribute(gui, GUI_TIMER, ATTR_ENABLED, 0);
        SetCtrlVal(gui, GUI_GENERATE, VI_FALSE);
		niFgen_AbortGeneration(vi);
		niFgen_close(vi);
		vi = VI_NULL;
	}
    return 0;
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
			SetCtrlAttribute(gui, GUI_TIMER, ATTR_ENABLED, 0);
		 	free(wfmData);
			wfmData = 0;
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
				SetCtrlAttribute(gui, GUI_TIMER, ATTR_ENABLED, 0);
				free(wfmData);
				wfmData = 0;
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
