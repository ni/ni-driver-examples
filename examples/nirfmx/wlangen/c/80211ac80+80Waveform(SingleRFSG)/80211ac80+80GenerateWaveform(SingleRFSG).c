/****************************************************************************
 *          National Instruments WLAN Signal Generator
 *---------------------------------------------------------------------------
 *   Copyright (c) National Instruments 2017.  All Rights Reserved.
 *---------------------------------------------------------------------------
 *
 * Title:    80211ac80+80GenerateWaveform(SingleRFSG).c
 * Purpose: This example demonstrates generation of 802.11a/g/j/p, 802.11b, single-channel 802.11n,
 *          single-channel 802.11ac, single-channel 802.11ah or single-channel 802.11af waveform using an 
 *          NI vector signal generator.
 
 Comments:
1. 
    A. Open an NI-RFSG session.
    B. Configure RFSG reference clock source.
    C. Configure basic NI-RFSG properties. Set Generation Mode to Script and Power Level Type to Peak Power. 
2. 
     A. Open an NI WLAN Generation session.
      B. Set Multi Segment Generation mode to "Single Generator" to transmit the 80M +80M segments using single RF generator.Set the standard to 80211AC MIMOOFDM, no of segments to 2. Configure the MCS index.
      C. Carrier frequencies are set to wlan generation session per segments based.
      
3. Create the waveform and download it to the NI RF vector signal generators memory.
4. Configure NI-RFSG for the waveform mentioned in the script.
5. Initiate generation.
6. 
    A. Check the generation status.
    B. Exit if an error has occurred, the Stop button is pressed or generation is complete.
7. 
    A. Abort signal generation.
    B. Disable the output. This sets the noise floor as low as possible.
    C. Clear the waveform from the device memory. Clear the waveform properties from RFSG database.
    D. Close the NI-RFSG session.
8. Read signal properties (waveform size and actual headroom) for display purpose.
9. Close the NI WLAN Generation session.
 ****************************************************************************/
#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <string.h>
#include "niWLANGenerationRfsg.h"
#include "niTCLK.h"

#define NUMBER_OF_SEGMENTS  2
#define MAX_WLAN_CHANNELS 4
/*--------------------------------------------------------------------------*/
/* Global Variables                                                         */
/*--------------------------------------------------------------------------*/
static niWLANGenerationSession gSession = NULL;
static char errorMessage[NIWLANG_VAL_MAX_ERROR_MESSAGE_SIZE];
static ViSession rfsgSession[1];
static ViSession externalLOSession =0;
static int32 isNewSession = 0;
static int32 standard,numberOfSegments;
static int32 mcsIndex,multiSegmentGenerationMode;
static float64 channelBandwidth;
static float64 powerLevel,externalAttenuation;
static float64 carrierFrequency[NUMBER_OF_SEGMENTS]={5.18e9,5.26e9};

static char	*rfsgResourceName = "RIO0" ;
static char *clkSource =  "OnboardClock";
static char *waveformName = "Wlan";
static char script[1024];
static char channelString[30];
	
/*--------------------------------------------------------------------------*/
/* Forward Declaration of the functions                                     */
/*--------------------------------------------------------------------------*/

int32 initGlobalVaribales(void);
int32 configureToolkitSessionForGeneration(void);
int32 configureRfsgSession(void);
int32 createAndDownloadWaveform(void);
int32 checkGeneration (void);
int32 stopGeneration (void);

int main (int argc, char *argv[])
{
	int32 error = 0,rfsgError;

	checkWarn(initGlobalVaribales());
	checkWarn(configureRfsgSession());
	checkWarn(configureToolkitSessionForGeneration());
	checkWarn(createAndDownloadWaveform());
	checkWarn(checkGeneration());
	checkWarn(stopGeneration()); 
	
Error:
	rfsgError = error;
	if(error != 0)
	{
  		niRFSG_GetError	(rfsgSession[0], &rfsgError, NIWLANG_VAL_MAX_ERROR_MESSAGE_SIZE, errorMessage);
		if(strlen(errorMessage) == 0 || error!=rfsgError)
   		niWLANG_GetErrorString (gSession, error, errorMessage, NIWLANG_VAL_MAX_ERROR_MESSAGE_SIZE);
	}
	
	if (error < 0)
		printf("ERROR: %s\n", errorMessage);

	else if (error > 0)
		printf("WARNING: %s\n", errorMessage);

	if(error != 0) {
		printf("\nPress any key to exit...\n");
		_getch();
	}

	error = niRFSG_close (rfsgSession[0]);
	/*Close WLAN session*/
	if(gSession)
		error = niWLANG_CloseSession(gSession);

	return error;
}

/*--------------------------------------------------------------------------*/
/* Function to initialize the global variables                              */
/*--------------------------------------------------------------------------*/
int32 initGlobalVaribales(void)
{
	//Set the parameters specific to the toolkit
	standard = NIWLANG_VAL_STANDARD_80211AC_MIMO_OFDM;
	channelBandwidth = 80e+6;
	numberOfSegments = 2;
	mcsIndex = 0;
	multiSegmentGenerationMode = NIWLANG_VAL_SINGLE_GENERATOR;
	powerLevel = -10;
	externalAttenuation =0;
	return 0;
}

/*--------------------------------------------------------------------------*/
/* Function to Configure the Hardware
/*--------------------------------------------------------------------------*/
int32 configureRfsgSession(void)
{
	int error = 0;

	//Open the RFSG Session
	if(!rfsgSession[0])
		checkWarn(niRFSG_init(rfsgResourceName, VI_TRUE,VI_FALSE , &rfsgSession[0]));
    checkWarn(niRFSG_ConfigurePowerLevelType(rfsgSession[0], NIRFSG_VAL_PEAK_POWER));
	checkWarn(niRFSG_ConfigureGenerationMode(rfsgSession[0], NIRFSG_VAL_SCRIPT));
	checkWarn(niRFSG_ConfigureRefClock(rfsgSession[0], clkSource, 10e6));
	checkWarn(niRFSG_SetAttributeViReal64(rfsgSession[0], NULL, NIRFSG_ATTR_EXTERNAL_GAIN, -(externalAttenuation) ));
	

 	sprintf(script,"%s",
			"script GenerateWlan\n \
				repeat forever\n \
					generate Wlan\n \
				end repeat\n \
			end script");
Error:
	return error;
}

/*--------------------------------------------------------------------------*/
/* Function to configure Toolkit Session for Generation                     */
/*--------------------------------------------------------------------------*/
int32 configureToolkitSessionForGeneration(void)
{
	int32 error = 0;

	if(!gSession)
		checkWarn(niWLANG_OpenSession("WLANG", NIWLANG_VAL_COMPATIBILITY_VERSION_060000, &gSession, &isNewSession));
	checkWarn(niWLANG_SetStandard(gSession, NULL, standard));
	checkWarn(niWLANG_SetChannelBandwidth(gSession, NULL, channelBandwidth));
	checkWarn(niWLANG_SetNumberOfSegments(gSession,NULL, numberOfSegments));
	checkWarn(niWLANG_SetMCSIndex(gSession,NULL,mcsIndex));

	checkWarn(niWLANG_SetRFBlankingEnabled(gSession, NULL, NIWLANG_VAL_TRUE));

Error:
	return error;
}

/*--------------------------------------------------------------------------*/
/* Function to create waveform                                              */
/*--------------------------------------------------------------------------*/
int32 createAndDownloadWaveform(void)
{
	int error = 0,i;
	float64 headroom, IQRate;
	checkWarn(niWLANG_RFSGConfigureFrequencySingleLO(gSession,rfsgSession,1,NIWLANG_VAL_LO_SOURCE_ONBOARD,externalLOSession,
		(carrierFrequency[0] +carrierFrequency[1])/2,NIWLANG_VAL_FALSE,NIWLANG_VAL_FALSE));
	checkWarn(niWLANG_SetMultiSegmentGenerationMode(gSession,NULL,multiSegmentGenerationMode));
	for(i=0;i<NUMBER_OF_SEGMENTS;i++)
	{
		sprintf(channelString,"segment%d",i);
		checkWarn(niWLANG_SetCarrierFrequency(gSession,channelString,carrierFrequency[i]));
	}
	checkWarn(niWLANG_RFSGCreateAndDownloadMIMOWaveforms(gSession,rfsgSession,NULL,1,"Wlan"));

    checkWarn(niWLANG_GetIQRate(gSession, "", &IQRate));	
	strcpy (channelString,"channel0");
	checkWarn(niWLANG_GetActualHeadroom(gSession,channelString, &headroom));
	printf("\nIQ Rate : %lf ",IQRate);
	printf("\nActual Headroom : %lf ",headroom);

	/*Configure Script*/
	checkWarn(niWLANG_RFSGConfigureScript(rfsgSession[0],"",script, powerLevel)); 
	niRFSG_ConfigureOutputEnabled (rfsgSession[0], VI_TRUE);
		
	/*Initiate Generation*/ 
	checkWarn(niRFSG_Initiate(rfsgSession[0]));

	printf("\nGenerating Signal Waveform");
	printf("\nPress any key to abort generation\n");	

Error:
	return error;
}
/*--------------------------------------------------------------------------*/
/* Check the Generation Status on the timer tick							*/
/*--------------------------------------------------------------------------*/
int32 checkGeneration (void)
{
	int32 error = 0;
	ViBoolean isDone = 0;
	/*Check Generation Status*/
	do
	{
		checkWarn(niRFSG_CheckGenerationStatus (rfsgSession[0],&isDone));
	}while(!isDone && !_kbhit() );
Error:
	return error;
}


/*--------------------------------------------------------------------------*/
/*Stop the Generation on pressing Stop or Quit Button						*/
/*--------------------------------------------------------------------------*/
int32 stopGeneration (void)
{
	int error=0;
	checkWarn(niRFSG_Abort(rfsgSession[0]));
	checkWarn(niRFSG_ConfigureOutputEnabled (rfsgSession[0], VI_FALSE));
	checkWarn(niRFSG_Commit(rfsgSession[0]));
	checkWarn(niWLANG_RFSGClearDatabase(rfsgSession[0], "", waveformName));
	checkWarn(niRFSG_close(rfsgSession[0]));
Error:
	return error;
}
