/******************************************************************************/
/* National Instruments Function Generator                                    */
/* Basic Onboard Signal Processing IF Example source file                     */
/*                                                                            */
/* This example shows how to use Onboard Signal Processing (OSP) capabilities.*/ 
/* This is a basic example that generates some simple IQ (16-QAM) data. If    */
/* you have a National Instruments Digitizer and the Modulation Toolkit, you  */
/* can use the Modulation Toolkit's MT niScope QAM Eye Diagram.vi example to  */
/* demodulate and view the 16-QAM data.										  */
/******************************************************************************/

#include <utility.h>
#include <ansi_c.h>
#include <cvirte.h>     /* Needed if linking in external compiler; harmless otherwise */
#include <userint.h>
#include <string.h>
#include "nifgen.h"
#include "OSPBasicIF.h"
#include "niModInstCustCtrl.h"

#define WFM_SIZE 1000

static int gui;
ViSession vi=VI_NULL;
ViStatus error = VI_SUCCESS;
NIComplexNumber wfm[WFM_SIZE];
int insideGenerateCallback=0;

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
        ResetTextBox(gui, GUI_ERROR_MESSAGE, errMsg);
        free(errMsg);
    }
    else if(error == VI_SUCCESS) ResetTextBox(gui, GUI_ERROR_MESSAGE, "");
}

/****************************************************************************\
  Starts the interative pannel
\****************************************************************************/
int main (int argc, char *argv[])
{
    ViInt32 i, rndval;
    
    if (InitCVIRTE (0, argv, 0) == 0)   /* Needed if linking in external compiler; harmless otherwise */
        return -1;  /* out of memory */
    if ((gui = LoadPanel (0, "OSPBasicIF.uir", GUI)) < 0)
        return -1;
    niModInstCVICust_NewCtrl (gui, GUI_RESOURCE, "nifgen");        
    DisplayPanel (gui);

    /*- Create some intervleaved random 16-QAM waveform data ---------------*/
    for (i = 0; i < WFM_SIZE; i++) {
    	rndval = rand() % 4;
    	switch (rndval) {
    		case 0: wfm[i].real = .707;  break;
    		case 1: wfm[i].real = -.707; break;
    		case 2: wfm[i].real = 0.236;  break;
    		case 3: wfm[i].real = -0.236; break;
    	};
    	rndval = rand() % 4;
    	switch (rndval) {
    		case 0: wfm[i].imaginary = .707;  break;
    		case 1: wfm[i].imaginary = -.707; break;
    		case 2: wfm[i].imaginary = 0.236;  break;
    		case 3: wfm[i].imaginary = -0.236; break;
    	};
	}
    
    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (gui, GUI_RESOURCE);    
    return 0;
}


/****************************************************************************\
  Loads in the waveform and starts generation
\****************************************************************************/
void wfmGenerate() { 
    ViChar Resource[256];
    ViReal64 CarrierFrequency, SymbolRate;
    ViInt32 wfmHandle;
    ViInt32 FilterType;
    ViReal64 FilterParameter;
    
    /*- Get all the control values -------------------------------------------*/
    GetCtrlVal(gui, GUI_RESOURCE, Resource);
    GetCtrlVal(gui, GUI_CARRIER_FREQUENCY, &CarrierFrequency);
    GetCtrlVal(gui, GUI_SYMBOL_RATE, &SymbolRate);
    GetCtrlVal(gui, GUI_FILTER_TYPE, &FilterType);
    GetCtrlVal(gui, GUI_FILTER_PARAMETER, &FilterParameter);
    
    checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Prepare arb for sequence mode output --------------------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_ARB));

	/*- Set OSP attributes for IF generation --------------------------------*/
	checkErr(niFgen_SetAttributeViBoolean (vi, "", NIFGEN_ATTR_OSP_ENABLED,
								  VI_TRUE));
	checkErr(niFgen_SetAttributeViBoolean (vi, "", NIFGEN_ATTR_OSP_CARRIER_ENABLED,
								  VI_TRUE));
	checkErr(niFgen_SetAttributeViInt32 (vi, "",
								NIFGEN_ATTR_OSP_DATA_PROCESSING_MODE,
								NIFGEN_VAL_OSP_COMPLEX));
	checkErr(niFgen_SetAttributeViReal64 (vi, "",
								 NIFGEN_ATTR_OSP_CARRIER_FREQUENCY,
								 CarrierFrequency));
	checkErr(niFgen_SetAttributeViReal64 (vi, "", NIFGEN_ATTR_OSP_IQ_RATE,
								 SymbolRate));
								 
	/* Set the Pre-filter gain.  We use Pre-filter Gain to attenuate the waveform 
     * data before it is filtered by the OSP block. We do this to provide headroom 
     * for overshoot in the FIR filters during pulse shaping.  
     */
	checkErr(niFgen_SetAttributeViReal64 (vi, "",
								 NIFGEN_ATTR_OSP_PRE_FILTER_GAIN_I, 0.7));
	checkErr(niFgen_SetAttributeViReal64 (vi, "",
								 NIFGEN_ATTR_OSP_PRE_FILTER_GAIN_Q, 0.7));
	
	/*- Set the filter type and the appropriate filter parameter ---------------*/
	checkErr(niFgen_SetAttributeViInt32 (vi, "", NIFGEN_ATTR_OSP_FIR_FILTER_TYPE,
								FilterType));
								
	switch (FilterType) {
		case NIFGEN_VAL_OSP_FLAT:
			checkErr(niFgen_SetAttributeViReal64 (vi, "",
								 NIFGEN_ATTR_OSP_FIR_FILTER_FLAT_PASSBAND,
								 FilterParameter));
			break;
		case NIFGEN_VAL_OSP_RAISED_COSINE:
			checkErr(niFgen_SetAttributeViReal64 (vi, "",
								 NIFGEN_ATTR_OSP_FIR_FILTER_RAISED_COSINE_ALPHA,
								 FilterParameter));
			break;
		case NIFGEN_VAL_OSP_ROOT_RAISED_COSINE:
			checkErr(niFgen_SetAttributeViReal64 (vi, "",
								 NIFGEN_ATTR_OSP_FIR_FILTER_ROOT_RAISED_COSINE_ALPHA,
								 FilterParameter));
			break;
		case NIFGEN_VAL_OSP_GAUSSIAN:
			checkErr(niFgen_SetAttributeViReal64 (vi, "",
								 NIFGEN_ATTR_OSP_FIR_FILTER_GAUSSIAN_BT,
								 FilterParameter));
			break;
	};
	
	/*- Turn filters on -----------------------------------------------------*/
	checkErr(niFgen_EnableDigitalFilter (vi, "0"));	
	checkErr(niFgen_EnableAnalogFilter (vi, "0", 0));
								 
    /*- Create a waveform ----------------------------------------*/
    checkErr(niFgen_CreateWaveformComplexF64 (vi, "", WFM_SIZE, wfm, &wfmHandle));
    
    /*- Generate the waveform -----------------------------------------------*/
    checkErr(niFgen_InitiateGeneration(vi));

    /*- Update the control values -------------------------------------------*/
    SetCtrlVal(gui, GUI_GENERATE, VI_TRUE);
    
Error:
    ErrorBox();
    if (error != VI_SUCCESS) {
        if (vi != VI_NULL) {
        	niFgen_close(vi);
        	vi = VI_NULL;
        }
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
                vi = VI_NULL;
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
        void *callbackData, int eventData1, int eventData2)
{
    int Generate;
    
    /* This callback shouldn't be allowed to be re-entrant */
    if (insideGenerateCallback) return 0;
    
    insideGenerateCallback = 1;
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
                    niFgen_close(vi);
                    vi = VI_NULL;
Error:
                    ErrorBox();
                }
            }
            break;
        }
    insideGenerateCallback = 0;
    return 0;
}

/****************************************************************************\
  Modify the label on the filter paramter when the filter type is changed
\****************************************************************************/
int CVICALLBACK OnFilterTypeChange (int panel, int control, int event,
		void *callbackData, int eventData1, int eventData2)
{
	int filterType;
	switch (event)
		{
		case EVENT_COMMIT:
			GetCtrlVal(gui, GUI_FILTER_TYPE, &filterType);
			switch (filterType)
			{
				case NIFGEN_VAL_OSP_FLAT:
					SetCtrlAttribute (gui, GUI_FILTER_PARAMETER, ATTR_LABEL_TEXT, "Flat Passband");
					break;
				case NIFGEN_VAL_OSP_RAISED_COSINE:
					SetCtrlAttribute (gui, GUI_FILTER_PARAMETER, ATTR_LABEL_TEXT, "Raised Cosine Alpha");
					break;
				case NIFGEN_VAL_OSP_ROOT_RAISED_COSINE:
					SetCtrlAttribute (gui, GUI_FILTER_PARAMETER, ATTR_LABEL_TEXT, "Root Raised Cosine Alpha");
					break;
				case NIFGEN_VAL_OSP_GAUSSIAN:
					SetCtrlAttribute (gui, GUI_FILTER_PARAMETER, ATTR_LABEL_TEXT, "Gaussian BT");
					break;
			};
		};
	return 0;
}
