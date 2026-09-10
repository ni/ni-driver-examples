/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Arbitrary Script Example source file                                      */
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
#include "ArbitraryScript.h"
#include "niModInstCustCtrl.h"

static int gui;
ViSession vi=VI_NULL;
ViStatus error = VI_SUCCESS;
void wfmGenerate(void);
void ErrorBox(void);

static const char scriptTrigID[] = "scripttrigger0";

enum {
	WAVEFORM_SINE = 0,
	WAVEFORM_TRIANGLE,
	WAVEFORM_SQUARE,
	WAVEFORM_DC0V
};

typedef struct {
	ViChar   name[256];
	ViInt32  type;
	ViReal64 scaling;
	ViInt32  numPoints;
} WaveformInfo;

#define NUM_WAVEFORMS 4
WaveformInfo scriptWaveforms[NUM_WAVEFORMS] = 
	{ { "wfmSine",     WAVEFORM_SINE,     0.50, 256},
	  { "wfmSquare",   WAVEFORM_SQUARE,   1.00, 256},
	  { "wfmTriangle", WAVEFORM_TRIANGLE, 0.50, 256},
	  { "wfmDC0V",     WAVEFORM_DC0V,     0.00, 256} };
	  
ViChar scripts[6][1024] = { 
"script myScript0 \n"
"  Generate wfmSquare \n"
"  Generate wfmSine \n"
"  Generate wfmTriangle \n"
"  Generate wfmSine \n" 
"  Generate wfmDC0V \n"
"end script",

"script myScript1 \n"
"  Wait until scriptTrigger0 \n"
"  Generate wfmSquare \n"
"  Generate wfmSine \n"
"  Generate wfmTriangle \n"
"  Generate wfmSine \n"
"  Generate wfmDC0V \n"
"end script",

"script myScript2 \n"
"  Generate wfmSquare \n"
"  Repeat 2 \n"
"    Generate wfmSine \n"
"  end repeat \n"
"  Repeat 5 \n"
"    Generate wfmTriangle \n"
"  end repeat \n"
"  Repeat 2 \n"
"    Generate wfmSine \n"
"  end repeat \n"
"  Repeat 3 \n"
"    Generate wfmDC0V \n"
"  end repeat \n"
"end script",

"script myScript3 \n"
"  Wait until scriptTrigger0 \n"
"  Generate wfmSquare \n"
"  Repeat 2 \n"
"    Generate wfmSine \n"
"  end repeat \n"
"  Repeat 5 \n"
"    Generate wfmTriangle \n"
"  end repeat \n"
"  Repeat 2 \n"
"    Generate wfmSine \n"
"  end repeat \n"
"  Repeat 3 \n"
"    Generate wfmDC0V \n"
"  end repeat \n"
"end script",

"script myScript4 \n"
"  Repeat forever \n"
"    Generate wfmSquare \n"
"    Repeat 2 \n"
"      Generate wfmSine \n"
"    end repeat \n"
"    Repeat 5 \n"
"      Generate wfmTriangle \n"
"    end repeat \n"
"    Repeat 2 \n"
"      Generate wfmSine \n"
"    end repeat \n"
"    Repeat 3 \n"
"      Generate wfmDC0V \n"
"    end repeat \n"
"  end repeat \n"
"end script",

"script myScript5 \n"
"  Wait until scriptTrigger0 \n"
"  Repeat forever \n"
"    Generate wfmSquare \n"
"    Repeat 2 \n"
"      Generate wfmSine \n"
"    end repeat \n"
"    Repeat 5 \n"
"      Generate wfmTriangle \n"
"    end repeat \n"
"    Repeat 2 \n"
"      Generate wfmSine \n"
"    end repeat \n"
"    Repeat 3 \n"
"      Generate wfmDC0V \n"
"    end repeat \n"
"  end repeat \n"
"end script" };


	  

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
    if ((gui = LoadPanel (0, "ArbitraryScript.uir", GUI)) < 0)
        return -1;

    ResetTextBox(gui, GUI_SCRIPT, scripts[0]);
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
    ViChar   resource[256], scriptTrigSource[256];
	ViInt32	 currentScript, i, j, numPoints, scriptTrigType;
	ViInt16* wfmData = 0;
    
    /*- Read in all of the control values ----------------------------------*/
    GetCtrlVal(gui, GUI_RESOURCE, resource);
    GetCtrlVal(gui, GUI_SCRIPT_NUM, &currentScript);
    GetCtrlVal(gui, GUI_SCRIPT_TRIG_TYPE, &scriptTrigType);
    GetCtrlVal(gui, GUI_SCRIPT_TRIG_SOURCE, scriptTrigSource);
    if(vi != VI_NULL) {
        checkErr(niFgen_close(vi));
    }

    /*- Initialize the session ---------------------------------------------*/
    checkErr(niFgen_init(resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Configure output mode for script -----------------------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_SCRIPT));

	/*- Configure script trigger settings ----------------------------------*/
	switch (scriptTrigType) {
		case 0:
		default:
			checkErr(niFgen_DisableScriptTrigger(vi, scriptTrigID));
			break;
		case 1:
			checkErr(niFgen_ConfigureSoftwareEdgeScriptTrigger(vi, scriptTrigID));
			break;
		case 2:
			checkErr(niFgen_ConfigureDigitalEdgeScriptTrigger(vi, scriptTrigID, 
						scriptTrigSource, NIFGEN_VAL_RISING_EDGE));
			break;
		case 3:
			checkErr(niFgen_ConfigureDigitalLevelScriptTrigger(vi, scriptTrigID, 
						scriptTrigSource, NIFGEN_VAL_ACTIVE_HIGH));
			break;
	}

    /*- Create and download the waveforms ----------------------------------*/
	for (i = 0; i < NUM_WAVEFORMS; i++) {
		numPoints = scriptWaveforms[i].numPoints;
	    wfmData = malloc(numPoints * sizeof(ViInt16));

	    if (scriptWaveforms[i].type == WAVEFORM_SINE) {
	        // Sine wave:
		    for (j = 0; j < numPoints; j++) {
        		wfmData[j] = sin(((ViReal64)j/numPoints)*2*3.141596)*32767*scriptWaveforms[i].scaling;
        	}

        } else if (scriptWaveforms[i].type == WAVEFORM_SQUARE) {
	    	// Square
		    for (j = 0; j < numPoints; j++) {
		        if (j < numPoints/2) wfmData[j] = 32767;
		        else wfmData[j] = -32767;
		        wfmData[j] *= scriptWaveforms[i].scaling;
		    }
		    
        } else if (scriptWaveforms[i].type == WAVEFORM_TRIANGLE) {
		    // Triangle
		    for (j = 0; j < numPoints/2; j++)
		        wfmData[j] = ((ViReal64)j/numPoints*4 - 1)*32767*scriptWaveforms[i].scaling;
		    for (j = numPoints/2; j < numPoints; j++)
		        wfmData[j] = (-(ViReal64)(j-numPoints/2)/numPoints*4 + 1)*32767*scriptWaveforms[i].scaling;
        
        } else {
			// 0V DC waveform:
		    for (j = 0; j < numPoints; j++) {
		    	wfmData[j] = 0;
		    }
        }
        
		checkErr(niFgen_WriteNamedWaveformI16 (vi, "0", scriptWaveforms[i].name,
  											   numPoints, wfmData));

	    free(wfmData);
	    wfmData = 0;
    }

    /*- Write the script ---------------------------------------------------*/
	checkErr(niFgen_WriteScript(vi, VI_NULL, scripts[currentScript]));
	
    /*- Generate the waveform ----------------------------------------------*/
    checkErr(niFgen_InitiateGeneration(vi));

    /*- Update the control values ------------------------------------------*/
    SetCtrlVal(gui, GUI_GENERATE, VI_TRUE);

Error:
    /*- Display any errors -------------------------------------------------*/
    ErrorBox();
    if((error < 0) && (vi != VI_NULL)) {
        niFgen_close(vi);
        vi = VI_NULL;
        SetCtrlVal(gui, GUI_GENERATE, VI_FALSE);
    }
}


/****************************************************************************\
  Change the script that is displayed and ultimately downloaded to the arb
\****************************************************************************/
int CVICALLBACK changeScript (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
	ViInt32 scriptNum;
    switch (event)
        {
        case EVENT_VAL_CHANGED:
        	GetCtrlVal(gui, GUI_SCRIPT_NUM, &scriptNum);
        	ResetTextBox(gui, GUI_SCRIPT, scripts[scriptNum]);
            break;
        }
    return 0;
}

/****************************************************************************\
  Change the script that is displayed and ultimately downloaded to the arb
\****************************************************************************/
int CVICALLBACK sendSWTrigger (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    switch (event)
        {
        case EVENT_COMMIT:
        	checkErr(niFgen_SendSoftwareEdgeTrigger (vi, NIFGEN_VAL_SCRIPT_TRIGGER, scriptTrigID));
            break;
        }
Error:
    ErrorBox();
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
