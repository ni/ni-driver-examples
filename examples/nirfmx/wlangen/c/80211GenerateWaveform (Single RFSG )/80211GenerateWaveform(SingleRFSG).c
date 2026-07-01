/****************************************************************************
 *          National Instruments WLAN Signal Generator
 *---------------------------------------------------------------------------
 *   Copyright (c) National Instruments 2017.  All Rights Reserved.
 *---------------------------------------------------------------------------
 *
 * Title:   80211GenerateWaveform(SingleRFSG).c
 * Purpose: This example demonstrates generation of 802.11a/g, 802.11j, 802.11p, 802.11b, single-channel 802.11n, 
 *          single-channel 802.11ac, single-channel 802.11ah, single-channel 802.11af, single-channel 802.11ax or  
 *          single-channel 802.11be waveform using an NI vector signal generator.
 
Comments:
1. Open an NI-RFSG session. 
2. Configure basic NI-RFSG properties. Set Generation Mode to Script and Power Level Type to Peak Power.
3. Open an NI WLAN Generation session.
4. Configure basic WLAN generation properties that are applicable to the specified standard.
5. Enable RF blanking to attenuate the RF OUT signal during the idle interval.
6. Configure frequency on the generator.
7. Create the waveform and download it to the NI RF vector signal generator memory.
8. Configure NI-RFSG for the waveform mentioned in the script.
9. Initiate signal generation.
10. 
    A. Check the generation status.
    B. Exit if an error has occurred or the Stop button is pressed.
11. Abort signal generation.
12. Disable the output. This sets the noise floor as low as possible.
13. Commit settings to hardware.
14. Clear the waveform from the device memory. Clear the waveform properties from RFSG database.
15. Close the NI-RFSG session.
16. Read signal properties (waveform size and actual headroom) for display purpose.
17. Close the NI WLAN Generation session.
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
static ViSession externalLOSession = 0;

static int32 isNewSession = 0;
static int32 standard,ofdmDataRate,dsssDataRate,mcsIndex;
static float64 channelBandwidth;
static float64 carrierFrequency,powerLevel,externalAttenuation;

static char	*rfsgResourceName = "RIO0" ;
static char *clkSource =  "OnboardClock";
static char *waveformName = "Wlan";
static char script[1024];
static char channelString[30];
	
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
	//Set the parameters specific to Hardware
	carrierFrequency = 5.18e9;
	powerLevel = -10.0;
	externalAttenuation = 0;

	//Set the parameters specific to the toolkit
	standard = NIWLANG_VAL_STANDARD_80211AG_OFDM;
	channelBandwidth = 20e+6;
	ofdmDataRate = 6;
	dsssDataRate = 1;
	mcsIndex = 0;
	
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

	/*Set Properties to the WLAN Generation Session*/
	if((standard == NIWLANG_VAL_STANDARD_80211AC_MIMO_OFDM) || (standard == NIWLANG_VAL_STANDARD_80211N_MIMO_OFDM) ||
		standard == NIWLANG_VAL_STANDARD_80211AH_MIMO_OFDM || standard == NIWLANG_VAL_STANDARD_80211AF_MIMO_OFDM ||
		standard == NIWLANG_VAL_STANDARD_80211AX_MIMO_OFDM || standard == NIWLANG_VAL_STANDARD_80211BE_MIMO_OFDM)
	{
		checkWarn(niWLANG_SetStandard(gSession, NULL, standard));
		checkWarn(niWLANG_SetChannelBandwidth(gSession, NULL, channelBandwidth));
		checkWarn(niWLANG_SetMCSIndex(gSession, NULL, mcsIndex));
        strcpy (channelString, "channel0");
	}
	else if(standard == NIWLANG_VAL_STANDARD_80211BG_DSSS)
	{
		checkWarn(niWLANG_SetStandard(gSession, NULL, standard));
		checkWarn(niWLANG_SetDSSSDataRate(gSession, NULL, dsssDataRate));
		strcpy (channelString, "");
	}
	else
	{
		checkWarn(niWLANG_SetStandard(gSession, NULL, standard));
		checkWarn(niWLANG_SetChannelBandwidth(gSession, NULL, channelBandwidth));
		checkWarn(niWLANG_SetOFDMDataRate(gSession, NULL, ofdmDataRate));
		strcpy (channelString, "");
	}

	checkWarn(niWLANG_SetRFBlankingEnabled(gSession, NULL, NIWLANG_VAL_TRUE));
	checkWarn(niWLANG_RFSGConfigureFrequencySingleLO(gSession,rfsgSession,1,NIWLANG_VAL_LO_SOURCE_ONBOARD,externalLOSession,
		carrierFrequency, NIWLANG_VAL_FALSE, NIWLANG_VAL_FALSE));
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
	int32 standard;

	checkWarn(niWLANG_GetStandard(gSession, NULL, &standard));

	if(standard == NIWLANG_VAL_STANDARD_80211AC_MIMO_OFDM || standard == NIWLANG_VAL_STANDARD_80211AF_MIMO_OFDM ||
		standard == NIWLANG_VAL_STANDARD_80211N_MIMO_OFDM || standard == NIWLANG_VAL_STANDARD_80211AH_MIMO_OFDM ||
		standard == NIWLANG_VAL_STANDARD_80211AX_MIMO_OFDM || standard == NIWLANG_VAL_STANDARD_80211BE_MIMO_OFDM)
	{	
		checkWarn(niWLANG_RFSGCreateAndDownloadMIMOWaveforms(gSession,rfsgSession,NULL, 1, "Wlan"));
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
	
	checkWarn(niRFSG_ConfigureOutputEnabled (rfsgSession[0], VI_FALSE));
	checkWarn(niRFSG_Abort(rfsgSession[0]));
	checkWarn(niRFSG_Commit(rfsgSession[0]));
	checkWarn(niWLANG_RFSGClearDatabase(rfsgSession[0], "", NULL));

Error:
	return error;
}
