/*****************************************************************************/
/* National Instruments Function Generator                                   */
/* Basic Standard Function Example source file                               */
/*                                                                           */
/* National Instruments, Austin Texas                                        */
/* PH. (800)433-3488   Fax (512)794-5678                                     */
/* Original Release: 4-99                                                    */
/*                                                                           */
/* Modification History:                                                     */
/*     Date    Initials  Description                                         */
/*     8-03    DC        Created                                             */
/*****************************************************************************/

#include <cvirte.h> /* Needed if linking in external compiler; harmless otherwise */
//#include <ansi_c.h>
#include <userint.h>
#include <stdlib.h>
#include <math.h>
#include "NIDAQmx.h"
#include "DAQmxIOctrl.h"
#include "DAQmxSynchronize.h"
#include "nifgen.h"
#include "niModInstCustCtrl.h"

static int fgenPanel;
static ViSession vi = VI_NULL;
static ViStatus error = VI_SUCCESS;
static ViBoolean generating = VI_FALSE; 
void func_gen(void);
void ErrorBox(void);

/****************************************************************************\
  Function for displaying messages in the error box
\****************************************************************************/
void ErrorBox() {
    ViUInt32 errMsgSize;
    ViChar*  errMsg;
    if(error <0 && generating == VI_TRUE) {
        errMsgSize = niFgen_GetError(vi, VI_NULL, 0, VI_NULL);
        errMsg = (ViChar *) malloc(sizeof(ViChar) * errMsgSize);
        niFgen_GetError(vi, &error, errMsgSize, errMsg);
        ResetTextBox(fgenPanel, FGEN_PANEL_ERROR_MESSAGE, errMsg);
        free(errMsg);
    }
    else if(error == VI_SUCCESS) ResetTextBox(fgenPanel, FGEN_PANEL_ERROR_MESSAGE, "");
}

/****************************************************************************\
  Starts the interative pannel
\****************************************************************************/
int main (int argc, char *argv[])
{
    if (InitCVIRTE (0, argv, 0) == 0)   /* Needed if linking in external compiler; harmless otherwise */
        return -1;  /* out of memory */
    if ((fgenPanel = LoadPanel (0, "DAQmxSynchronize.uir", FGEN_PANEL)) < 0)
        return -1;
	NIDAQmx_NewPhysChanAICtrl (fgenPanel, FGEN_PANEL_DAQMX_CHANNEL, 1);
	NIDAQmx_NewTerminalCtrl (fgenPanel, FGEN_PANEL_DAQMX_SCLK_SOURCE, 0);
	NIDAQmx_NewTerminalCtrl (fgenPanel, FGEN_PANEL_DAQMX_TRIG_SOURCE, 0);
    niModInstCVICust_NewCtrl (fgenPanel, FGEN_PANEL_RESOURCE, "nifgen");	
    DisplayPanel (fgenPanel);
    RunUserInterface ();
    niModInstCVICust_DiscardCtrl (fgenPanel, FGEN_PANEL_RESOURCE);    
    return 0;
}

/****************************************************************************\
  Generates a buffer full of the desired data
  Note: This function allocates a buffer of a calculated size for the waveform
  data.  The caller of this function is responsible for de-allocation!
\****************************************************************************/
ViStatus generateWaveformData(int wfmType, double frequency, double sampleRate, int* wfmSize, double** wfmData)
{
    ViStatus error = VI_SUCCESS;
    int periodSize = floor(sampleRate/frequency + .5);
    int numPeriods = 1;
    int waveformQuantum = 0;
    int p, x;
    double position;
    
    *wfmSize = 0;
    *wfmData = 0;
    
    // Make sure we have a reasonably large waveform buffer size.  We'll use multiple periods if necessary
    // to avoid data underflow errors.
    if (periodSize < 32) numPeriods *= ceil(32/periodSize);
        
    // Make sure our total number of samples is a multiple of the waveform quantum
    checkErr(niFgen_GetAttributeViInt32(vi, VI_NULL, NIFGEN_ATTR_WAVEFORM_QUANTUM, &waveformQuantum));        
    while ((periodSize * numPeriods) % waveformQuantum) numPeriods ++;
        
    // Allocate the buffer for the data
    *wfmSize = periodSize * numPeriods;
    *wfmData = (double *) malloc(*wfmSize * sizeof(double));
        
    // Create the data
    for (p = 0; p < numPeriods; p++) {
      	for (x = 0; x < periodSize; x++) {        
         	switch(wfmType) {
              	case 1:         // Sine
                   	(*wfmData)[p*periodSize + x] = sin(x/((double) periodSize) * 2.0 * 3.141593);              
                    break;
                case 2:         // Square
                    (*wfmData)[p*periodSize + x] = floor(x*2/((double) periodSize)) == 0 ? 1.0 : -1.0;
                    break;
                case 3:         // Triangle
                    if (x < periodSize/2) 
                        (*wfmData)[p*periodSize + x] = x*4.0/((double) periodSize) - 1.0;
                    else 
                       (*wfmData)[p*periodSize + x] = -x*4.0/((double) periodSize) + 3.0;
                    break;
                default:        // Sawtooth
                    (*wfmData)[p*periodSize + x] = ((double) x)/periodSize * 2.0 - 1.0;
            }
        }
    }
    
        
Error:
    if (error < VI_SUCCESS) {
        free(wfmData);
        *wfmData = 0;
        *wfmSize = 0;
    }
    return error;
}

/****************************************************************************\
  This reads all control values and starts waveform generation
\****************************************************************************/
void func_gen() {
    ViChar    resource[256], daqChannel[256], arbTriggerSource[256];
    ViReal64  frequency, amplitude;
    ViInt32   wfmType, wfmSize = 0;
    ViInt32   wfmHandle = 0, seqHandle = 0;
    ViReal64* wfmData = 0;
    ViReal64* acquiredData = 0;
    ViInt32   loopCounts, markers;
    ViReal64  sampleRate, acquisitionRate;
    ViInt32   exportedSampleClockDivisor;
    ViInt32   numSamples, numRead;
    ViChar    daqSampleClockSource[256], daqTriggerSource[256];
    TaskHandle   daqTaskHandle;

    /*- Read all control values -------------------------------------------*/
    GetCtrlVal(fgenPanel, FGEN_PANEL_RESOURCE, resource);
    GetCtrlVal(fgenPanel, FGEN_PANEL_WAVEFORM, &wfmType);
    GetCtrlVal(fgenPanel, FGEN_PANEL_FREQUENCY, &frequency);
    GetCtrlVal(fgenPanel, FGEN_PANEL_AMPLITUDE, &amplitude);
    GetCtrlVal(fgenPanel, FGEN_PANEL_ARB_TRIG_SOURCE, arbTriggerSource);
    GetCtrlVal(fgenPanel, FGEN_PANEL_DAQMX_CHANNEL, daqChannel);
    GetCtrlVal(fgenPanel, FGEN_PANEL_ACQUISITION_RATE, &acquisitionRate);
    GetCtrlVal(fgenPanel, FGEN_PANEL_NUM_SAMPLES, &numSamples);
    GetCtrlVal(fgenPanel, FGEN_PANEL_DAQMX_SCLK_SOURCE, daqSampleClockSource);
    GetCtrlVal(fgenPanel, FGEN_PANEL_DAQMX_TRIG_SOURCE, daqTriggerSource);
   
    acquiredData = (ViReal64 *) malloc (numSamples * sizeof(ViReal64));
   
    if(vi != VI_NULL) {
        checkErr (niFgen_close(vi));
    }

    /*- ARB CONFIGURATION --------------------------------------------------*/
    /*- Initialize the session ---------------------------------------------*/
    checkErr (niFgen_init(resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session ----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Switch to sequence mode --------------------------------------------*/
    checkErr (niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_SEQ));
    
    /*- Create and download the arb waveform -------------------------------*/
    checkErr (niFgen_GetAttributeViReal64 (vi, VI_NULL, NIFGEN_ATTR_ARB_SAMPLE_RATE,
                                           &sampleRate));
    checkErr (generateWaveformData(wfmType, frequency, sampleRate, &wfmSize, &wfmData));
    checkErr (niFgen_CreateWaveformF64(vi, "0", wfmSize, wfmData, &wfmHandle));

    /*- Create the sequence with a marker at sample zero -------------------*/
    loopCounts = 1;
    markers = 0;
    checkErr (niFgen_CreateAdvancedArbSequence (vi, 1, &wfmHandle, &loopCounts,
                                                VI_NULL, &markers, VI_NULL,
                                                &seqHandle));

    /*- Configure the sequence to generate ---------------------------------*/
    checkErr (niFgen_ConfigureArbSequence (vi, "0", seqHandle, amplitude/2, 0));
    
    /*- Configure the arb's trigger source ---------------------------------*/
    checkErr (niFgen_ConfigureDigitalEdgeStartTrigger (vi, arbTriggerSource, 
                                                       NIFGEN_VAL_RISING_EDGE));    

    /*-----------------------------------------------------------------------
      Route the sample clock out to RTSI 1.  We set the exported sample clock 
      divider so that the exported sample clock is equal to the desired 
      acquisition rate for the DAQmx device.
      ----------------------------------------------------------------------*/
    exportedSampleClockDivisor = (ViInt32) sampleRate / acquisitionRate;
    checkErr (niFgen_SetAttributeViInt32 (vi, VI_NULL, 
                                         NIFGEN_ATTR_EXPORTED_SAMPLE_CLOCK_DIVISOR,
                                         exportedSampleClockDivisor));
    checkErr (niFgen_ExportSignal(vi, NIFGEN_VAL_SAMPLE_CLOCK, "", "RTSI1"));
    
    /*- Update indicators for sample rate and sample clock divisor ---------*/
    SetCtrlVal(fgenPanel, FGEN_PANEL_SAMPLE_RATE, sampleRate);
    SetCtrlVal(fgenPanel, FGEN_PANEL_DIVISOR, exportedSampleClockDivisor);
                                         
    /*- Commit the arb's settings to hardware ------------------------------*/
    checkErr (niFgen_Commit(vi));

    /*- DAQMX DEVICE CONFIGURATION -----------------------------------------*/
    /*- Create an input voltage channel ------------------------------------*/
    checkErr (DAQmxCreateTask ("", &daqTaskHandle));
    checkErr (DAQmxCreateAIVoltageChan (daqTaskHandle, daqChannel,"", 
                                           DAQmx_Val_RSE, -2.5, 2.5, 
                                           DAQmx_Val_Volts, ""));

    /*- Configure sample clock settings ------------------------------------*/    
    checkErr (DAQmxCfgSampClkTiming (daqTaskHandle, "", acquisitionRate, 
                                        DAQmx_Val_Rising, DAQmx_Val_ContSamps,
                                        1000));
    checkErr (DAQmxSetTimingAttribute (daqTaskHandle, DAQmx_SampClk_Src, 
                                          daqSampleClockSource));

    /*- Configure trigger settings -----------------------------------------*/    
    checkErr (DAQmxCfgDigEdgeStartTrig (daqTaskHandle, daqTriggerSource, 
                                           DAQmx_Val_Falling));
                                                           
    /*- Initiate generation on the arb and DAQmx device --------------------*/
    checkErr (DAQmxStartTask (daqTaskHandle));
    checkErr (niFgen_InitiateGeneration (vi));
    generating = VI_TRUE;

    /*- Acquire and display data until the stop button is pressed ----------*/
    while (generating) {
        checkErr (DAQmxReadAnalogF64 (daqTaskHandle, 10000, 10.0,
									  DAQmx_Val_GroupByScanNumber, acquiredData,
									  numSamples, &numRead, NULL));
        if( numRead>0 )
            PlotStripChart(fgenPanel, FGEN_PANEL_STRIPCHART, acquiredData, numRead, 0,0, VAL_DOUBLE);
            
        ProcessSystemEvents();    
    }

    /*- Close sessions and clean up ----------------------------------------*/    
    checkErr (DAQmxStopTask (daqTaskHandle));
    checkErr (DAQmxClearTask (daqTaskHandle));
    checkErr( niFgen_AbortGeneration (vi));
    checkErr( niFgen_close (vi));    
    vi = VI_NULL;
    
    free(acquiredData);
    
    SetCtrlVal(fgenPanel, FGEN_PANEL_START, VI_FALSE);
Error:
    return;
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
            GetCtrlVal(fgenPanel, FGEN_PANEL_START, &Generate);
            if(Generate) {
                func_gen();
            }
            else {
                generating = VI_FALSE;
            }
Error:
            ErrorBox();
            /*- Display any errors ----------------------------------------*/
            if((error < 0) && (vi != VI_NULL)) {
                SetCtrlVal(fgenPanel, FGEN_PANEL_START, VI_FALSE);
                generating = VI_FALSE;
            }
            break;
        }
    return 0;
}

