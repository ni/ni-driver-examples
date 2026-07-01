/****************************************************************************
 *          National Instruments WLAN Signal Generator
 *---------------------------------------------------------------------------
 *   Copyright (c) National Instruments 2017.  All Rights Reserved.
 *---------------------------------------------------------------------------
 *
 * Title:    80211SignalGenerationFromFile(SingleRFSG).c
 * Purpose:  This example demonstrates generation of 802.11 single-channel waveform using NI vector signal generator. 
 *			 The example reads the waveform from a TDMS file.

 Comments:
1. Open an NI-RFSG session. 
2. Configure basic NI-RFSG properties. Set Generation Mode to Script and Power Level Type to Peak Power.
3. Read the waveform from the file and write it to NI vector signal generator memory.
4. Initiate signal generation.
5. 
    A. Check the generation status.
    B. Exit if an error has occurred or the Stop button is pressed.
6. Abort signal generation.
7. Disable the output. This sets the noise floor as low as possible.
8. Clear the waveform from the device memory. Clear the waveform properties from RFSG database.
9. Close the NI-RFSG session.
 *
 ****************************************************************************/
#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <string.h>
#include "niWLANGenerationRfsg.h"
#include "niTCLK.h"

#define MAX_WLAN_CHANNELS 4
/*--------------------------------------------------------------------------*/
/* Global Variables                                                         */
/*--------------------------------------------------------------------------*/
static niWLANGenerationSession WlanSession = NULL;
static char errorMessage[NIWLANG_VAL_MAX_ERROR_MESSAGE_SIZE];
static ViSession rfsgSession[1];
static int32 isNewSession = 0;
static float64 carrierFrequency,powerLevel,externalAttenuation;

static char	*rfsgResourceName = "RIO0" ;
static char *clkSource =  "OnboardClock";
static char *waveformName = "Wlan";
static char *filepath = "";
static char script[1024];
/*--------------------------------------------------------------------------*/
/* Forward Declaration of the functions                                     */
/*--------------------------------------------------------------------------*/

int32 initGlobalVaribales(void);
int32 configureRfsgSession(void);
int32 readAndDownloadWaveform(void);
int32 checkGeneration (void);
int32 stopGeneration (void);

int main (int argc, char *argv[])
{
	int32 error = 0,rfsgError;

	checkWarn(initGlobalVaribales());
	checkWarn(configureRfsgSession());
	checkWarn(readAndDownloadWaveform());
	checkWarn(checkGeneration());
	checkWarn(stopGeneration());
	
Error:
	rfsgError = error;
	niRFSG_GetError	(rfsgSession[0], &rfsgError, NIWLANG_VAL_MAX_ERROR_MESSAGE_SIZE, errorMessage);
	
	if (error < 0)
		printf("ERROR: %s\n", errorMessage);

	else if (error > 0)
		printf("WARNING: %s\n", errorMessage);


	error = niRFSG_close (rfsgSession[0]);

	return error;
}

/*--------------------------------------------------------------------------*/
/* Function to initialize the global variables                              */
/*--------------------------------------------------------------------------*/
int32 initGlobalVaribales(void)
{
	//Set the parameters specific to Hardware
	carrierFrequency = 5.18e9;
	powerLevel = -10.0;
	externalAttenuation = 0;
	
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

	checkWarn(niRFSG_SetAttributeViReal64(rfsgSession[0], NULL, NIRFSG_ATTR_FREQUENCY, carrierFrequency)); 
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
/* Function to create waveform                                              */
/*--------------------------------------------------------------------------*/
int32 readAndDownloadWaveform(void)
{
	int error = 0;
	
	checkWarn(niWLANG_RFSGReadAndDownloadWaveformsFromFile(&rfsgSession[0],1,waveformName,filepath));

	/*Configure Script*/
	checkWarn(niWLANG_RFSGConfigureScript(rfsgSession[0],"",script, powerLevel)); 
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
	do
	{
		/*Check Generation Status*/
		checkWarn(niRFSG_CheckGenerationStatus (rfsgSession[0],&isDone));
	}while(!isDone &&  !_kbhit());
Error:
	return error;
}


/*--------------------------------------------------------------------------*/
/*Stop the Generation on pressing Stop or Quit Button						*/
/*--------------------------------------------------------------------------*/
int32 stopGeneration (void)
{
	int error=0;;
	
	checkWarn(niRFSG_Abort(rfsgSession[0]));
	checkWarn(niRFSG_ConfigureOutputEnabled (rfsgSession[0], VI_FALSE));
	checkWarn(niRFSG_Commit(rfsgSession[0]));
	checkWarn(niWLANG_RFSGClearDatabase(rfsgSession[0], "", NULL));

Error:
	return error;
}
