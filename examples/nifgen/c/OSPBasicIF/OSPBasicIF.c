/******************************************************************************/
/* National Instruments Function Generator                                    */
/* OSP Basic IF Example source file                                           */
/*                                                                            */
/* This example shows how to use Onboard Signal Processing (OSP) capabilities.*/ 
/* This is a basic example that generates some simple IQ (16-QAM) data. If    */
/* you have a National Instruments Digitizer and the Modulation Toolkit, you  */
/* can use the Modulation Toolkit's MT niScope QAM Eye Diagram.vi example to  */
/* demodulate and view the 16-QAM data.		                                  */
/******************************************************************************/
#include "niFgen.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

#define WFM_SIZE 1000

int main(int argc, char *argv[]) {
    ViChar Resource[256];
    const ViChar * ChannelName = "0";
    char *FilterParamString = NULL;
    ViReal64 SymbolRate, CarrierFrequency, FilterParameter;
    ViInt32 FilterType;
    ViStatus error = VI_SUCCESS;
    ViSession vi=VI_NULL;
    ViInt32 i, rndval;
    ViInt32 wfmHandle;
    NIComplexNumber wfm[WFM_SIZE];
    
    if(argc == 1) {
        /*- Prompt for parameters -----------------------------------------------*/
        #define BUFSIZE 256
        ViChar inputLine[BUFSIZE];
        
        // set default values
        strcpy(Resource, "PXI1Slot2");
        SymbolRate = 1e+5;
        CarrierFrequency = 1.5e+7;
        FilterType = 2;               // Root raised cosine
        FilterParameter = 0.4;
        
        // Optionally override defaults from the command line.
        // If user simply hits enter, inputLine is an empty string, and sscanf will not
        // update its target variable (so the default value is preserved)
        
        printf("\nSpecify Inputs (Enter to accept default)\n");
        
        printf("Resource Name (%s): ", Resource);
        if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%s", Resource);
        
        printf("Symbol Rate in Symbols/sec (%lf): ", SymbolRate);
        if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &SymbolRate);
        
        printf("Carrier Frequency in Hz (%lf): ", CarrierFrequency);
        if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &CarrierFrequency);
        
        printf("FIR Filter Type {%ld=Flat | %ld=Raised Cosine | %ld=Root Raised Cosine | %ld=Gaussian} (%d): ",
            NIFGEN_VAL_OSP_FLAT, 
            NIFGEN_VAL_OSP_RAISED_COSINE,
            NIFGEN_VAL_OSP_ROOT_RAISED_COSINE,
            NIFGEN_VAL_OSP_GAUSSIAN,
            FilterType);
        if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%d", &FilterType);
        
        switch (FilterType) {
        case NIFGEN_VAL_OSP_FLAT:
            FilterParamString = "Flat Passband";
            break;
        case NIFGEN_VAL_OSP_RAISED_COSINE:
            FilterParamString = "Raised Cosine Alpha";
            break;
        case NIFGEN_VAL_OSP_GAUSSIAN:
            FilterParamString = "Gaussian BT";
            break;
        case NIFGEN_VAL_OSP_ROOT_RAISED_COSINE:
        default:
            FilterParamString = "Root Raised Cosine Alpha";
            break;	
        };
        
        printf("%s (%lf): ", FilterParamString, FilterParameter);
        if ( fgets(inputLine,BUFSIZE,stdin) ) sscanf(inputLine, "%lf", &FilterParameter);
        
        // Print newline(s) to visually separate output from input
        printf("\n\n");
    }
    else if(argc == 5) {
        /*- Get parameters from the command line ------------------------------*/
        strcpy(Resource, argv[1]);
        SymbolRate = strtod(argv[2], NULL);
        CarrierFrequency = strtod(argv[2], NULL);
        FilterType = strtol(argv[3], NULL, 0);
        FilterParameter = strtod(argv[4], NULL);
    }
    else {
        /*- Show usage --------------------------------------------------------*/
        printf("Usage: %s <resource> <symbol rate> <carrier frequency> <FIR Filter Type> <FIR Filter Param>\n", argv[0]);
        return -1;
    }
    
    /*- Create some random complex (I/Q) waveform data -----------------------*/
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
    
    /*- Intialize a session -------------------------------------------------*/
    checkErr(niFgen_init (Resource, VI_TRUE, VI_TRUE, &vi));

    /*- Configure the active channels for the session -----------------------*/
    checkErr(niFgen_ConfigureChannels(vi, "0"));

    /*- Prepare arb for sequence mode output --------------------------------*/
    checkErr(niFgen_ConfigureOutputMode(vi, NIFGEN_VAL_OUTPUT_ARB));

    /*- Set OSP attributes for IF generation --------------------------------*/
    checkErr(niFgen_SetAttributeViBoolean (vi, "", NIFGEN_ATTR_OSP_ENABLED,
        VI_TRUE));
    checkErr(niFgen_SetAttributeViBoolean (vi, "", NIFGEN_ATTR_OSP_CARRIER_ENABLED,
        VI_TRUE));
    checkErr(niFgen_SetAttributeViInt32 (vi, "", NIFGEN_ATTR_OSP_DATA_PROCESSING_MODE,
        NIFGEN_VAL_OSP_COMPLEX));
    checkErr(niFgen_SetAttributeViReal64 (vi, "", NIFGEN_ATTR_OSP_CARRIER_FREQUENCY,
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
    
    /*- Create an arbitrary sequence ----------------------------------------*/
    checkErr(niFgen_CreateWaveformComplexF64 (vi, "", WFM_SIZE, wfm, &wfmHandle));
    
    /*- Turn filters on -----------------------------------------------------*/
    checkErr(niFgen_EnableAnalogFilter (vi, "0", 0));
    checkErr(niFgen_EnableDigitalFilter (vi, "0"));
    
    /*- Generate the sequence -----------------------------------------------*/
    checkErr(niFgen_InitiateGeneration(vi));
    
    printf("Generating waveform...\n");
    
    // clear the input buffer, then wait for user input before closing the session.
    // (DAQmx devices will quit generating the output when the session is closed).
    fflush( stdin );
    printf("Press Enter to continue...\n");
    getchar();
    
Error:
    /*- Process any errors ---------------------------------------------------*/
    if(error != VI_SUCCESS) {
        ViChar errMsg[256];
        niFgen_ErrorHandler(vi, error, errMsg);
        printf("Error %x: %s\n", error, errMsg);
    }
    
    /*- Close the session ----------------------------------------------------*/
    if (vi) niFgen_close (vi);
    return 0;
}

