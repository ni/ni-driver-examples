/*********************************************************************
*
* NI-FGEN multitone generation example program for CVI
*
* This example demonstrates how to generate a multitone waveform
* using arbitrary waveform mode.
*
*********************************************************************/

#include <utility.h>
#include <ansi_c.h>
#include <cvirte.h>     /* Needed if linking in external compiler; harmless otherwise */
#include <userint.h>
#include <string.h>
#include "nifgen.h"
#include "MultitoneGeneration.h"
#include "niModInstCustCtrl.h"

typedef struct WfmData_s {
   ViInt32 wfmSize;
   ViReal64 *data;
} WfmData;

static int gui;

ViInt32 WfmHandle=NIFGEN_VAL_NO_WAVEFORM;

ViReal64* Frequencies=VI_NULL;
ViReal64* Amplitudes=VI_NULL;
ViReal64* Phases=VI_NULL;
ViInt32 NumTones = 0;
ViReal64 Gain;

ViSession vi=VI_NULL;
ViStatus error = VI_SUCCESS;

void wfmLoad(void);
void wfmGenerate(void);
void ErrorBox(void);
ViStatus ReadWfmFile(ViString File, ViReal64 *Frequencies[], ViReal64 *Amplitudes[], ViReal64 *Phases[],
                     ViInt32 *ListSize);
ViStatus CreateData(ViReal64 **Buffer, 
					ViReal64 Frequencies[], ViReal64 Amplitudes[], ViReal64 Phases[], 
					ViReal64 SampleRate, ViInt32 NumSamples, ViInt32 NumTones,
					ViReal64 *max);

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
    if ((gui = LoadPanel (0, "MultitoneGeneration.uir", GUI)) < 0)
        return -1;
    niModInstCVICust_NewCtrl (gui, GUI_RESOURCE, "nifgen");        
    DisplayPanel (gui);
    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (gui, GUI_RESOURCE);    
    return 0;
}

/****************************************************************************\
  Reads in the waveform files and loads them into the Arb
\****************************************************************************/
void wfmLoad() {
    ViChar Resource[256], TonesFile[256], Channel[256];
    ViInt32 i, Generate, wfm, numTones, NumSamples;
    ViReal64 *Frequencies=VI_NULL, *Amplitudes=VI_NULL, *Phases=VI_NULL;
    ViReal64 SampleRate, ActualSampleRate;
    WfmData waveform = {VI_NULL, VI_NULL};

    /*- Read in all of the control values ------------------------------------*/
    GetCtrlVal(gui, GUI_RESOURCE, Resource);
    GetCtrlVal(gui, GUI_TONES_FILE, TonesFile);
    GetCtrlVal(gui, GUI_CHANNEL, Channel);
    GetCtrlVal(gui, GUI_SAMPLE_RATE, &SampleRate);
    GetCtrlVal(gui, GUI_NUM_SAMPLES, &NumSamples);
    
    checkErr(niFgen_ConfigureSampleRate(vi, SampleRate));
	checkErr(niFgen_GetAttributeViReal64 (vi, VI_NULL,
                                          NIFGEN_ATTR_ACTUAL_ARB_SAMPLE_RATE,
                                          &ActualSampleRate));
    SetCtrlVal(gui, GUI_SAMPLE_RATE, ActualSampleRate);

    /*- Configure the signal generator for arb mode --------------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_ARB));

	/* Read file listing tones -----------------------------------------------*/
	checkErr (ReadWfmFile(TonesFile, &Frequencies, &Amplitudes, &Phases, &NumTones));

    /*- Create an array of waveforms from the data files ---------------------*/
    waveform.wfmSize = NumSamples;
    checkErr (CreateData(&waveform.data, Frequencies, Amplitudes, Phases, 
    		  			 ActualSampleRate, NumSamples, NumTones, &Gain));
    
    /*- Load the waveform into the arb, and keep the handle ------------------*/
    checkErr(niFgen_CreateWaveformF64(vi, Channel, waveform.wfmSize,
                                      waveform.data,  
                                      &WfmHandle));
                                      
Error:

    /*- Clean up  ------------------------------------------------------------*/
    free(waveform.data);
    free(Frequencies);
    free(Amplitudes);
    free(Phases);
    waveform.data = Frequencies = Amplitudes = Phases = VI_NULL;
    ErrorBox();
}

/****************************************************************************\
  Loads in the sequence and starts generation
\****************************************************************************/
void wfmGenerate() { 
    ViChar Resource[256], Channel[256], TonesFile[256];
    ViReal64 SampleRate, dcOffset, ActualSampleRate;
    ViInt32 DigitalFilter, AnalogFilter;
    ViBoolean DigitalFilterEnable, AnalogFilterEnable;
    
    /*- Get all the control values ------------------------------------------*/
    GetCtrlVal(gui, GUI_RESOURCE, Resource);
    GetCtrlVal(gui, GUI_CHANNEL, Channel);
    GetCtrlVal(gui, GUI_DC_OFFSET, &dcOffset);
    GetCtrlVal(gui, GUI_DIGITAL_FILTER, &DigitalFilter);
    DigitalFilterEnable = (DigitalFilter == 1) ? VI_TRUE : VI_FALSE;
    GetCtrlVal(gui, GUI_ANALOG_FILTER, &AnalogFilter);
    AnalogFilterEnable = (AnalogFilter == 1) ? VI_TRUE : VI_FALSE;
    
    /*- Close any existing session and start a new one. ---------------------*/
    if(vi)
    {
        checkErr(niFgen_close(vi));
        vi = VI_NULL;
    }
    
    checkErr(niFgen_init(Resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Prepare arb for arb mode output -------------------------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_ARB));

    /*- Create waveform and load into signal generator ----------------------*/
    wfmLoad();
    
    /*- Select arb sequence to generate, configure gain and offset ----------*/
    checkErr(niFgen_ConfigureArbWaveform(vi, Channel, WfmHandle, Gain,
                                        dcOffset)); 

    /*- Configure other parameters: sample rate, filters --------------------*/
    checkErr(niFgen_SetAttributeViBoolean(vi, Channel,
                                         NIFGEN_ATTR_DIGITAL_FILTER_ENABLED,
                                         DigitalFilterEnable));
    checkErr(niFgen_SetAttributeViBoolean(vi, Channel,
                                         NIFGEN_ATTR_ANALOG_FILTER_ENABLED,
                                         AnalogFilterEnable));
    
    /*- Enable output -------------------------------------------------------*/
    checkErr(niFgen_ConfigureOutputEnabled(vi, Channel, VI_TRUE));

    /*- Generate the sequence -----------------------------------------------*/
    checkErr(niFgen_InitiateGeneration(vi));

    /*- Update the control values ------------------------------------------*/
    SetCtrlVal(gui, GUI_GENERATE, VI_TRUE);
    
Error:

    /*- Free up memory and desplay any errors ------------------------------*/
    
    ErrorBox();
    if((error != VI_SUCCESS) && (vi != VI_NULL)) {
        niFgen_close(vi);
        vi = VI_NULL;
        SetCtrlVal(gui, GUI_GENERATE, VI_FALSE);
    }
}


/****************************************************************************\
  Don't do anything when the sequence text changes, wait for a button pushed
\****************************************************************************/
int CVICALLBACK tones_file (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    switch (event)
        {
        case EVENT_COMMIT:
            break;
        }
    return 0;
}


/****************************************************************************\
  Displays a file selection dialog box to select the sequence
\****************************************************************************/
int CVICALLBACK pick_tones_file (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViChar FileName[512];
    switch (event)
        {
        case EVENT_COMMIT:
            if(FileSelectPopup (".", "*.txt", "", "Tones File", VAL_LOAD_BUTTON, 0,
                                0, 1, 0, FileName) <=0) {
                SetCtrlVal(gui, GUI_TONES_FILE, "");
                return -1;
            }
            SetCtrlVal(gui, GUI_TONES_FILE, FileName);
            break;
        }
    return 0;
}

/****************************************************************************\
  Use notepad to edit the sequence file
\****************************************************************************/
int CVICALLBACK edit_tones_file (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViChar ToneFile[256];
    ViChar command[512];
    switch (event)
        {
        case EVENT_COMMIT:
            GetCtrlVal(gui, GUI_TONES_FILE, ToneFile);
            sprintf (command, "notepad %s", ToneFile);
            LaunchExecutable(command);
            break;
        }
    return 0;
}

/****************************************************************************\
  Don't do anything when filters changed--wait for next update
  \****************************************************************************/
int CVICALLBACK digital_filter (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    switch (event)
        {
        case EVENT_COMMIT:
            break;
        }
    return 0;
}

int CVICALLBACK analog_filter (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    switch (event)
        {
        case EVENT_COMMIT:
            break;
        }
    return 0;
}

/****************************************************************************\
  Change the dc offset when the dc offset knob is changed  
\****************************************************************************/
int CVICALLBACK dc_offset (int panel, int control, int event,
        void *callbackData, int eventData1, int eventData2)
{
    ViReal64 dcOffset;
    switch (event)
        {
        case EVENT_COMMIT:
            if(vi != VI_NULL) {
                GetCtrlVal(gui, GUI_DC_OFFSET, &dcOffset);
                checkErr(niFgen_SetAttributeViReal64 (vi, "", NIFGEN_ATTR_ARB_OFFSET, dcOffset));
Error:
                ErrorBox();
            }
            break;
        }
    return 0;
}

/****************************************************************************\
  Close the session and exit the program when the stop button is pushed
\****************************************************************************/
int CVICALLBACK quit (int panel, int control, int event,
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
                    vi = VI_NULL;
Error:
                    ErrorBox();
                }
            }
            break;
        }
    return 0;
}

/****************************************************************************\
  Read in the tone file
\****************************************************************************/
ViStatus ReadWfmFile(ViString File, ViReal64 **Frequencies, ViReal64 **Amplitudes, ViReal64 **Phases,
                     ViInt32 *NumTones) {
    FILE *wfmfile;
    ViChar buffer[1024];
    ViInt32 lines, values, tones;
    ViReal64 frequency, amplitude, phase;
    *Frequencies = VI_NULL;
    *Amplitudes = VI_NULL;
    *Phases = VI_NULL;
  
    error = 0;
    
    if(!(wfmfile = fopen (File, "r"))) {
        sprintf(buffer, "Unable to open file %s.", File);
        MessagePopup ("Error", buffer);
        error = -1;
        goto Error;
    }
    
    /*- Check file format --------------------------------------------------*/
    for(lines = 0; ; lines++) {
        values = fscanf(wfmfile, "%lf %lf %lf\n", &frequency, &amplitude, &phase);
        if((values < 1)||(values > 3)) break;
    }
    rewind (wfmfile);
    if(lines == 0) {
            MessagePopup("Error", "Empty waveform file or invalid waveform file format");
            error = -1;
    }
    
    /*- Check file format --------------------------------------------------*/
    free (*Frequencies);
    free (*Amplitudes);
    free (*Phases);
    checkAlloc(*Frequencies = malloc(lines * sizeof(ViReal64)));
    checkAlloc(*Amplitudes = malloc(lines * sizeof(ViReal64)));
    checkAlloc(*Phases = malloc(lines * sizeof(ViReal64)));
    for(tones=0; tones < lines; tones++) {
        values = fscanf(wfmfile, "%lf %lf %lf\n", &(*Frequencies)[tones], &(*Amplitudes)[tones], &(*Phases)[tones]);
        if(values < 2) {
            (*Amplitudes)[tones] = 1.0;
        }
        if(values < 3) {
            (*Phases)[tones] = 0.0;
        }
    }
    fclose(wfmfile);
    *NumTones = tones;
Error:
    return error;
}

/****************************************************************************\
  Create waveform containing sum of tones
\****************************************************************************/
ViStatus CreateData(ViReal64 **Buffer, 
					ViReal64 Frequencies[], ViReal64 Amplitudes[], ViReal64 Phases[], 
					ViReal64 SampleRate, ViInt32 NumSamples, ViInt32 NumTones,
					ViReal64 *max) {
    ViInt32 sample, tone;
                      
    /*- Create buffer -------------------------------------------------------*/
    free(*Buffer);
    checkAlloc(*Buffer = malloc (NumSamples * sizeof(ViReal64)));
    
    if (SampleRate <= 0.0)
        goto Error;

	/*- Sum requested tone --------------------------------------------------*/      
    for (sample = 0; sample < NumSamples; sample++) {
    	(*Buffer)[sample] = 0.0;
        for (tone = 0; tone < NumTones; tone++) {
        	ViReal64 debug = Amplitudes[tone]*sin(((ViReal64)sample*Frequencies[tone]/SampleRate+Phases[tone])
        			*2.0*3.141596);
        	(*Buffer)[sample] += Amplitudes[tone]*sin(((ViReal64)sample*Frequencies[tone]/SampleRate+Phases[tone])
        			*2.0*3.141596);
        	}
    }
    
	/*- Waveform data must be normalized to +/- 1.0 --------------------------*/
	for (sample = 0; sample < NumSamples; sample++) {
	    if (*max < (*Buffer)[sample])
	        *max = (*Buffer)[sample];
	    if (*max < -(*Buffer)[sample])
	        *max = -(*Buffer)[sample];
	}
	if (*max > 0.0)
	{
    	for (sample = 0; sample < NumSamples; sample++)
    		(*Buffer)[sample] /= *max;
	}
     
Error:
    if (error != VI_SUCCESS) MessagePopup("Error", "Error reading waveform files.");
    return error;
}


