/****************************************************************************
 *          National Instruments WLAN Signal Generator
 *---------------------------------------------------------------------------
 *   Copyright (c) National Instruments 2017.  All Rights Reserved.
 *---------------------------------------------------------------------------
 *
 * Title:    80211LoadConfigurationFromFileAndGenerateWaveform(SingleRFSG).c
 * Purpose:  This example demonstrates how to load toolkit configuration from a TDMS file and generate an 802.11 waveform 
 *			 using an NI vector signal generator.
 *
 Comments:
1. Open an NI-RFSG session. 
2. Configure basic NI-RFSG properties. Set Generation Mode to Script and Power Level Type to Peak Power.
3. Configure frequency on the generator.
4. Open an NI WLAN Generation session.
5. Load NI WLAN Generation settings from a .tdms file.
6. Create the waveform and download it to the NI RF vector signal generator memory.
7. Configure NI-RFSG for the waveform mentioned in the script.
8. Initiate signal generation.
9. 
    A. Check the generation status.
    B. Exit if an error has occurred or the Stop button is pressed.
10. Abort signal generation.
11. Disable the output. This sets the noise floor as low as possible.
12. Clear the waveform from the device memory. Clear the waveform properties from RFSG database.
13. Close the NI-RFSG session.
14. Read signal properties (waveform size and actual headroom) for display purpose.
15. Close the NI WLAN Generation session.
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
static niWLANGenerationSession gSession = NULL;
static char errorMessage[NIWLANG_VAL_MAX_ERROR_MESSAGE_SIZE];
static ViSession rfsgSession[1];
static ViSession externalLOSession =0;
static int32 isNewSession = 0;
static float64 carrierFrequency,powerLevel,externalAttenuation;

static char	*rfsgResourceName = "RIO0" ;
static char *clkSource =  "OnboardClock";
static char *waveformName = "Wlan";
static char *filePath = "configurationFilePathToBeAdded";
static char script[1024];
/*--------------------------------------------------------------------------*/
/* Forward Declaration of the functions                                     */
/*--------------------------------------------------------------------------*/

int32 initGlobalVaribales(void);
int32 configureRfsgSession(void);
int32 configureToolkitSessionForGeneration(void);
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

	if(gSession) 
 	 error = niWLANG_CloseSession(gSession);

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
		checkWarn(niRFSG_init(rfsgResourceName, VI_TRUE, VI_FALSE ,&rfsgSession[0]));

	checkWarn(niRFSG_ConfigurePowerLevelType(rfsgSession[0], NIRFSG_VAL_PEAK_POWER));
	checkWarn(niRFSG_ConfigureGenerationMode(rfsgSession[0], NIRFSG_VAL_SCRIPT));
	checkWarn(niRFSG_ConfigureRefClock(rfsgSession[0], clkSource, 10e6));
	checkWarn(niRFSG_SetAttributeViReal64(rfsgSession[0], NULL, NIRFSG_ATTR_EXTERNAL_GAIN,-(externalAttenuation)));

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

	/*Set Properties to the WLAN Generation Session*/
	/*Load Configuration from File*/	
	checkWarn(niWLANG_LoadConfigurationFromFile(gSession, filePath, NIWLANG_VAL_TRUE));

Error:
	return error;
}

/*--------------------------------------------------------------------------*/
/* Function to create waveform                                              */
/*--------------------------------------------------------------------------*/
int32 createAndDownloadWaveform(void)
{
	int error = 0;
	float64 headroom, IQRate;
	char channelString[30];
	int32 standard;

	checkWarn(niWLANG_GetStandard(gSession, NULL, &standard));
	
	checkWarn(niWLANG_RFSGConfigureFrequencySingleLO(gSession,rfsgSession,1,NIWLANG_VAL_LO_SOURCE_ONBOARD,externalLOSession,
  		                                             carrierFrequency,NIWLANG_VAL_FALSE,NIWLANG_VAL_FALSE));

	if(standard == NIWLANG_VAL_STANDARD_80211AC_MIMO_OFDM || standard == NIWLANG_VAL_STANDARD_80211AF_MIMO_OFDM ||
	    standard == NIWLANG_VAL_STANDARD_80211N_MIMO_OFDM || standard == NIWLANG_VAL_STANDARD_80211AH_MIMO_OFDM ||
		standard == NIWLANG_VAL_STANDARD_80211AX_MIMO_OFDM || standard == NIWLANG_VAL_STANDARD_80211BE_MIMO_OFDM)
	{	
		checkWarn(niWLANG_RFSGCreateAndDownloadMIMOWaveforms(gSession,rfsgSession,NULL, 1, "Wlan"));
		strcpy (channelString, "channel0");
	}
	else
	{
		checkWarn(niWLANG_RFSGCreateAndDownloadWaveform(gSession,rfsgSession[0],NULL,"Wlan"));
		strcpy (channelString, "");
	}
    checkWarn(niWLANG_GetIQRate(gSession, "", &IQRate));	
	checkWarn(niWLANG_GetActualHeadroom(gSession,channelString, &headroom));

	printf("\nStandard : %d ", standard);
	printf("\nIQ Rate : %lf ",IQRate);
	printf("\nActual Headroom : %lf ",headroom);

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
	}while(!isDone && !_kbhit());
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
