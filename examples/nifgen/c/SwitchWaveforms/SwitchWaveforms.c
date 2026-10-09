/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Switch Waveforms Example source file                                      */
/*                                                                           */
/* National Instruments, Austin Texas                                        */
/* PH. (800)433-3488   Fax (512)794-5678                                     */
/*                                                                           */
/* Modification History:                                                     */
/*     Date    Initials  Description                                         */
/*     9-06    KM        Created                                             */
/*****************************************************************************/

#include <utility.h>
#include <ansi_c.h>
#include <cvirte.h>     /* Needed if linking in external compiler; harmless otherwise */   
#include <userint.h>
#include <stdio.h>
#include "nifgen.h"
#include "SwitchWaveforms.h"
#include "niModInstCustCtrl.h"

static int gui;
ViSession vi = VI_NULL;
ViStatus error = VI_SUCCESS;
ViInt32 waveform;
ViInt32 numPoints = 512;
void wfmGenerate(void);
void createWfmData(ViInt16*);
void ErrorBox(void);

ViConstString script = { 
	"script SwitchWaveforms \n"
	   	"repeat forever \n"
	      	"repeat until scriptTrigger0 \n"
	         	"generate Waveform1 \n"
			"end repeat \n"
			"repeat until scriptTrigger0 \n"
				"generate Waveform2 \n"
			"end repeat \n"
		"end repeat \n"
	"end script \n"
};


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
	if (InitCVIRTE (0, argv, 0) == 0)
		return -1;    /* out of memory */
	if ((gui = LoadPanel (0, "SwitchWaveforms.uir", GUI)) < 0)
        return -1;
    niModInstCVICust_NewCtrl (gui, GUI_RESOURCE, "nifgen");        
    DisplayPanel (gui);
    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (gui, GUI_RESOURCE);    
	return 0;
}

/****************************************************************************\
  Configures the session and starts generation of the first waveform
\****************************************************************************/
void wfmGenerate()
{
    ViChar resource[256];
    ViInt16* wfmData = VI_NULL;
    wfmData = malloc(numPoints * sizeof(ViInt16));

    /*- Read in the control values -----------------------------------------*/  
    GetCtrlVal(gui, GUI_RESOURCE, resource);

    if(vi != VI_NULL) {
        checkErr(niFgen_close(vi)); 
    }   

    /*- Initialize the session ---------------------------------------------*/
    checkErr(niFgen_init(resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Configure output mode for script -----------------------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_SCRIPT));
	
	/*- Clear the waveform memory on the Arbitrary Waveform Generator (AWG) */
	checkErr(niFgen_ClearArbMemory (vi));

	/*- Allocate memory on the AWG for the two waveforms. ------------------*/
	checkErr(niFgen_AllocateNamedWaveform (vi, "0", "Waveform1", numPoints));
    checkErr(niFgen_AllocateNamedWaveform (vi, "0", "Waveform2", numPoints));   

 	/*- Configure script trigger settings ----------------------------------*/
	checkErr(niFgen_ConfigureSoftwareEdgeScriptTrigger (vi, "ScriptTrigger0"));

	/*- Create and download the first waveform to the AWG ------------------*/
	createWfmData(wfmData);
	checkErr(niFgen_WriteNamedWaveformI16 (vi, "0", "Waveform1",
  											   numPoints, wfmData));

    /*- Write the script ---------------------------------------------------*/
	checkErr(niFgen_WriteScript (vi, "0", script));

    /*- Generate the first waveform ----------------------------------------*/
    checkErr(niFgen_InitiateGeneration(vi));

    /*- Change the indicators to show which waveform is generating ---------*/
    SetCtrlVal(gui, GUI_GEN_WFM1, 1);
    SetCtrlVal(gui, GUI_GEN_WFM2, 0);
    
Error:
    free(wfmData);    
    /*- Display any errors -------------------------------------------------*/
    ErrorBox();
    if((error < 0) && (vi != VI_NULL)) {
        niFgen_close(vi);
        vi = VI_NULL;
        SetCtrlVal(gui, GUI_GENERATE, VI_FALSE);
    }
}


/****************************************************************************\
  Creates the waveform data
\****************************************************************************/
void createWfmData(ViInt16 *wfmData)
{
    ViInt32 wfmType, j, i;
    ViReal64 scaling = 1.0;
	
	/*- Read in the control values for the waveform that isn't generating --*/  
	if (waveform == 0)
	{
	    GetCtrlVal(gui, GUI_WFM1_TYPE, &wfmType);
	    GetCtrlVal(gui, GUI_WFM1_SCALING, &scaling);
	}
	else 
	{
		GetCtrlVal(gui, GUI_WFM2_TYPE, &wfmType);
		GetCtrlVal(gui, GUI_WFM2_SCALING, &scaling);
	}
	
	/*- Create the waveform ------------------------------------------------*/
	switch(wfmType)											   
	{
		case 0:
	        // Sine wave
		    for (j = 0; j < numPoints; j++) 
		    {
        		wfmData[j] = sin(((ViReal64)j/numPoints)*2*3.141596)*32767*scaling;
        	}
			break;
		case 1:
        	// Square wave
		    for (j = 0; j < numPoints; j++) 
		    {
		        if (j < numPoints/2) wfmData[j] = 32767;
		        else wfmData[j] = -32767;
		        wfmData[j] *= scaling;
		    }
		    break;
		case 2:
			// Triangle wave
		    for (j = 0; j < numPoints/2; j++)
		        wfmData[j] = ((ViReal64)j/numPoints*4 - 1)*32767*scaling;
		    for (j = numPoints/2; j < numPoints; j++)
		        wfmData[j] = (-(ViReal64)(j-numPoints/2)/numPoints*4 + 1)*32767*scaling;
        	break;
    }
}

/****************************************************************************\
  Switch which waveform is currently generating.
\****************************************************************************/
int CVICALLBACK switchWaveforms(int panel, int control, int event, 
		void *callbackData, int eventData1, int eventData2)
{
    ViInt16* wfmData = VI_NULL;
    wfmData = malloc(numPoints * sizeof(ViInt16)); 	
	
	switch (event)
	   {
		   case EVENT_COMMIT: 
			   if (waveform == 0)
			   {
				  /*- Create and download the second waveform -----------------------------*/
				  waveform = 1;
				  createWfmData(wfmData);
				  checkErr(niFgen_SetNamedWaveformNextWritePosition (vi, "0", "Waveform2",
															NIFGEN_VAL_WAVEFORM_POSITION_START,
															0));
				  checkErr(niFgen_WriteNamedWaveformI16 (vi, "0", "Waveform2",
				  										 numPoints, wfmData));

				  /*- Send scriptTrigger to switch waveforms ------------------------------*/
				  checkErr(niFgen_SendSoftwareEdgeTrigger (vi, NIFGEN_VAL_SCRIPT_TRIGGER,
												  "ScriptTrigger0"));

				  /*- Change the indicators to show which waveform is generating ----------*/   
			      SetCtrlVal(gui, GUI_GEN_WFM1, 0);
			      SetCtrlVal(gui, GUI_GEN_WFM2, 1);
			   }
			   else
			   {
				  /*- Create and download the first waveform ------------------------------*/
				  waveform = 0;
				  createWfmData(wfmData);
				  checkErr(niFgen_SetNamedWaveformNextWritePosition (vi, "0", "Waveform1",
															NIFGEN_VAL_WAVEFORM_POSITION_START,
															0));
				  checkErr(niFgen_WriteNamedWaveformI16 (vi, "0", "Waveform1",
				  											   numPoints, wfmData));
		
		          /*- Send scriptTrigger to switch waveforms ------------------------------*/
				  checkErr(niFgen_SendSoftwareEdgeTrigger (vi, NIFGEN_VAL_SCRIPT_TRIGGER,
												  "ScriptTrigger0"));
			   			   	  
			   	  /*- Change the indicators to show which waveform is generating ----------*/   
			   	  SetCtrlVal(gui, GUI_GEN_WFM1, 1);
			   	  SetCtrlVal(gui, GUI_GEN_WFM2, 0);
			   }
		   break;
     }
    free(wfmData);
	 return 0;

Error:
    free(wfmData);
    /*- Display any errors ----------------------------------------------------------------*/
    ErrorBox();
    if((error < 0) && (vi != VI_NULL)) {
        niFgen_close(vi);
        vi = VI_NULL;
        SetCtrlVal(gui, GUI_GENERATE, VI_FALSE);
    }
    return 0;
}


/****************************************************************************\
  Close the session and exit the program when the quit button is pushed.
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
  Stop generation and close the session when the generate button is released
\****************************************************************************/
int CVICALLBACK generate (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2) {
    int Generate;
    switch (event)
        {
        case EVENT_COMMIT:
            GetCtrlVal(gui, GUI_GENERATE, &Generate);
            if(Generate) {
            	waveform = 0;
                wfmGenerate();
            }
            else {
                if(vi != VI_NULL) {
                    checkErr(niFgen_AbortGeneration(vi));
                    checkErr(niFgen_close(vi));
                    vi=VI_NULL;
                    SetCtrlVal(gui, GUI_GEN_WFM1, 0);
                    SetCtrlVal(gui, GUI_GEN_WFM2, 0);
Error:
                    ErrorBox();
                }
            }
            break;
        }
    return 0;
}
