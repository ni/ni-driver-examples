/****************************************************************************
 *          National Instruments WLAN Signal Generator
 *---------------------------------------------------------------------------
 *   Copyright (c) National Instruments 2017.  All Rights Reserved.
 *---------------------------------------------------------------------------
 *
 * Title:    80211MIMOOFDMGenerateWaveform(Multiple RFSG with Ext LO).c
 * Purpose:  This example demonstrates generation of multiple-channel 802.11n, 802.11ac, 802.11ah, 802.11af,
 *           802.11ax or 802.11be waveform using multiple NI vector signal generators.
 * 
Comments:
1. For each generator,
     A. Open an NI-RFSG session.
     B. Configure basic NI-RFSG properties. Set Generation Mode to Script and Power Level Type to Peak Power.
2. If an External LO is to be used, open the NI-RFSG session and configure reference Clock source and generation Mode to CW. 
3. Open an NI WLAN Generation session.
4. Configure basic WLAN generation properties. 
   Configure 80211n PLCP Frame Format, Number of Space Time Streams and 80211ah Preamble Type 
5. Enable RF blanking to attenuate the RF OUT signal during the idle interval. 
6.  
    A. Synchronize the generators. Configure the reference clock source for the master (the first generator) and share that with slaves. Configure the generators for daisy-chained local oscillator (LO) sharing.
     B. Configure the External LO and analyzers for frequency and daisy-chained local oscillator (LO) sharing.
7. Start External LO Initate
8. Create the waveform and download it to the NI RF vector signal generators memory.
9.  For each generator, configure NI-RFSG for the waveform mentioned in the script.
10. Initiate signal generation by calling niWLANG RFSG Multiple Device Initiate.
11. 
    A. Check the generation status.
    B. Exit if an error has occurred, the Stop button is pressed or generation is complete.
12 For each generator,
    A. Abort signal generation.
    B. Disable the output. This sets the noise floor as low as possible.
    C. Clear the waveform from the device memory. Clear the waveform properties from RFSG database.
    D. Close the NI-RFSG session.
13. Read signal properties (waveform size and actual headroom) for display purpose.
14. Close the NI WLAN Generation session.
15. For External LO
    A. Abort signal generation.
    B. Disable the output. This sets the noise floor as low as possible.
    C. Close the NI-RFSG session.
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
static ViSession rfsgSession[MAX_WLAN_CHANNELS] = {0,0,0,0};
static ViSession externalLOSession = 0;
static int32 isNewSession = 0;
static int32 LOSource,rfsgLODaisyChainEnabled,LOExportToExternalDevicesEnabled;

static char	*rfsgResourceName[MAX_WLAN_CHANNELS] = {"RIO0","RIO1","RIO2","RIO3"};
static char	*externalLOResourceName= NULL;
static float64 powerLevel[MAX_WLAN_CHANNELS] = {-10,-10,-10,-10};
static float64 externalAttenuation[MAX_WLAN_CHANNELS] = {0,0,0,0};
static float64 carrierFrequency;

static float64 channelBandwidth;
static int32 standard,MCSIndex,payloadDataLength;

static int32 numberOfTransmitChannels,mappingMatrixType,preambleType,
   	         numberOfSpaceTimeStreams,plcpFrameFormat;

static char *clkSource =  "PXI_CLK";
static char *externalLOClkSource = "OnboardClock";
static char *waveformName = "Wlan";
static int32 triggerLines[] = {0,1};
static char script[1024];
static char channelString[30];

/*--------------------------------------------------------------------------*/
/* Forward Declaration of the functions                                     */
/*--------------------------------------------------------------------------*/
int32 initGlobalVaribales(void);
int32 configureToolkitSessionForGeneration(void);
int32 configureRfsgSession(void);
int32 configureExternalLOSession(void);
int32 createAndDownloadWaveform(void);
int32 checkGeneration (void);
int32 stopGeneration (void);
int32 closeExternalLOHandle (void);

int main (int argc, char *argv[])
{
	int32 error = 0, rfsgError,i;

	checkWarn(initGlobalVaribales());
	checkWarn(configureRfsgSession());
	checkWarn(configureExternalLOSession());
	checkWarn(configureToolkitSessionForGeneration());
	checkWarn(createAndDownloadWaveform());
	checkWarn(checkGeneration());
	checkWarn(stopGeneration());
	checkWarn(closeExternalLOHandle());  
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
	if(error != 0)
	{
		printf("\nPress any key to exit...\n");
	   _getch();
	}
	/*Close Trigger-Frame session*/
	if(gSession)
		niWLANG_CloseSession(gSession);
	/*Close WLAN Generation session*/
	if (gSession)
		niWLANG_CloseSession(gSession);
	/*Close RFSG session*/	
	for (i=0; i<numberOfTransmitChannels; i++)
	{
		if(rfsgSession[i])
		{
			error = niRFSG_close (rfsgSession[i]);
		}
	}
	return error;
}

/*--------------------------------------------------------------------------*/
/* Function to initialize the global variables                              */
/*--------------------------------------------------------------------------*/
int32 initGlobalVaribales(void)
{
	//Set the parameters specific to the toolkit
	standard = NIWLANG_VAL_STANDARD_80211AX_MIMO_OFDM;
	numberOfTransmitChannels = 1;
	mappingMatrixType  = NIWLANG_VAL_MAPPING_MATRIX_TYPE_DIRECT;
	preambleType = NIWLANG_VAL_80211AH_PREAMBLE_TYPE_SHORT;
   	numberOfSpaceTimeStreams = 1;
	channelBandwidth = 80e+6;
	MCSIndex = 0;
	plcpFrameFormat = NIWLANG_VAL_80211N_PLCP_FRAME_FORMAT_MIXED; 

	LOSource = NIWLANG_VAL_LO_SOURCE_EXTERNAL;
	rfsgLODaisyChainEnabled = NIWLANG_VAL_FALSE;
	LOExportToExternalDevicesEnabled= NIWLANG_VAL_FALSE;

	carrierFrequency = 5.18e9;
	
	return 0;
}
/*--------------------------------------------------------------------------*/
/* Function to Configure the Hardware
/*--------------------------------------------------------------------------*/
int32 configureRfsgSession(void)
{
	int error = 0,i;
	for( i = 0; i< numberOfTransmitChannels; i++)
	{
		//Open the RFSG Session
		if(!rfsgSession[i])
			checkWarn(niRFSG_init(rfsgResourceName[i], VI_TRUE , VI_FALSE, &rfsgSession[i]));
	
		checkWarn(niRFSG_ConfigureGenerationMode(rfsgSession[i], NIRFSG_VAL_SCRIPT));
		checkWarn(niRFSG_ConfigurePowerLevelType(rfsgSession[i], NIRFSG_VAL_PEAK_POWER));
		checkWarn(niRFSG_SetAttributeViReal64(rfsgSession[i], NULL, NIRFSG_ATTR_EXTERNAL_GAIN, -externalAttenuation[i]));
	}
Error:
	return error;
}

int32 configureExternalLOSession(void)
{
	int error = 0;
	if(carrierFrequency > 3.2e+9 && LOSource == NIWLANG_VAL_LO_SOURCE_EXTERNAL && externalLOResourceName != NULL)
	{
		if(!externalLOSession)
	   	  checkWarn(niRFSG_init(externalLOResourceName, VI_TRUE , VI_FALSE, &externalLOSession));

		checkWarn(niRFSG_ConfigureRefClock(externalLOSession, externalLOClkSource, 10e6));
		checkWarn(niRFSG_ConfigureGenerationMode(externalLOSession, NIRFSG_VAL_CW));
	}
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
		//Open the WLAN Generation Session
	if(!gSession)
		checkWarn(niWLANG_OpenSession("WLANG", NIWLANG_VAL_COMPATIBILITY_VERSION_060000, &gSession, &isNewSession));
	checkWarn(niWLANG_SetStandard(gSession, NULL, standard));
	checkWarn(niWLANG_SetChannelBandwidth(gSession, NULL, channelBandwidth));
	checkWarn(niWLANG_SetNumberOfTransmitChannels(gSession,NULL,numberOfTransmitChannels));
	checkWarn(niWLANG_SetMCSIndex(gSession, NULL, MCSIndex));
	checkWarn(niWLANG_SetMappingMatrixType(gSession,NULL,mappingMatrixType));
	checkWarn(niWLANG_Set80211nPLCPFrameFormat(gSession,NULL, plcpFrameFormat));
	checkWarn(niWLANG_SetNumberOfSpaceTimeStreams(gSession,NULL, numberOfSpaceTimeStreams));
	checkWarn(niWLANG_Set80211ahPreambleType(gSession,NULL,preambleType));
	checkWarn(niWLANG_SetLOFrequencyOffsetMode(gSession,NULL, NIWLANG_VAL_LO_FREQUENCY_OFFSET_MODE_AUTO));
	checkWarn(niWLANG_SetRFBlankingEnabled(gSession,NULL,NIWLANG_VAL_TRUE));
Error:
	return error;
}

/*--------------------------------------------------------------------------*/
/* Function to create waveform                                              */
/*--------------------------------------------------------------------------*/
int32 createAndDownloadWaveform(void)
{
	int error = 0,i;
	float64 IQRate, waveformDuration;
	float64 *headroom = NULL;
	int32 waveFormSize;
	
	checkWarn(niWLANG_RFSGConfigureMultipleDeviceSynchronization(gSession,rfsgSession,numberOfTransmitChannels,clkSource,triggerLines,2 ));
	checkWarn(niWLANG_RFSGConfigureFrequencySingleLO(gSession,rfsgSession,numberOfTransmitChannels,LOSource,externalLOSession,carrierFrequency,rfsgLODaisyChainEnabled,LOExportToExternalDevicesEnabled));
	
	if(carrierFrequency > 3.2e+9 && LOSource == NIWLANG_VAL_LO_SOURCE_EXTERNAL && externalLOResourceName != NULL)
  	   checkWarn(niRFSG_Initiate(externalLOSession));

	checkWarn(niWLANG_RFSGCreateAndDownloadMIMOWaveforms(gSession, rfsgSession, NULL, numberOfTransmitChannels, "Wlan"));
	
	/*Configure Script*/
	for( i = 0; i < numberOfTransmitChannels; i++)
	{
		checkWarn(niWLANG_RFSGConfigureScript(rfsgSession[i], "", script, powerLevel[i]));
	}

	checkWarn(niWLANG_RFSGMultipleDeviceInitiate(gSession, rfsgSession,numberOfTransmitChannels));

    checkWarn(niWLANG_GetIQRate(gSession, "", &IQRate));	
	checkWarn(niWLANG_GetIQWaveformSize(gSession, "", &waveFormSize));
	waveformDuration = waveFormSize/IQRate;
	headroom = (float64*)(malloc(sizeof(float64) *numberOfTransmitChannels));
	for( i = 0; i< numberOfTransmitChannels; i++)
	{
		sprintf(channelString,"Channel%d",i);
		checkWarn(niWLANG_GetActualHeadroom(gSession, channelString, &headroom[i]));
	}
	printf("\nWaveform Duration (s) : %lf\n", waveformDuration);
	for( i = 0; i< numberOfTransmitChannels; i++)
		printf("Actual Headroom [%d] (dB) : %lf \n",i, headroom[i]);
	
	printf("\n\nGenerating Waveform");
	printf("\nPress any key to abort generation\n");
Error:
	return error;
}
/*--------------------------------------------------------------------------*/
/* Check the Generation Status on the timer tick							*/
/*--------------------------------------------------------------------------*/
int32 checkGeneration (void)
{
	int32 error = 0,i;
	ViBoolean isDone = 0;
	do
	{
    	for(i=0; i < numberOfTransmitChannels; i++)
	    {
			/*Check Generation Status*/
		    checkWarn(niRFSG_CheckGenerationStatus (rfsgSession[i],&isDone));
			if(isDone)
				break;
		}
	}while(!isDone && !_kbhit());
Error:
	return error;
}
/*--------------------------------------------------------------------------*/
/*Stop the Generation on pressing Stop or Quit Button						*/
/*--------------------------------------------------------------------------*/
int32 stopGeneration (void)
{
	int error=0,i;
	
    /*- Disable the output.  This sets the noise floor as low as possible. -*/
	for (i=0; i<numberOfTransmitChannels; i++)
		if (rfsgSession[i]) 
		{
			checkWarn(niRFSG_Abort(rfsgSession[i]));
			checkWarn(niRFSG_ConfigureOutputEnabled (rfsgSession[i], VI_FALSE));
			checkWarn(niRFSG_Commit(rfsgSession[i]));
			checkWarn(niWLANG_RFSGClearDatabase(rfsgSession[i], "", "Wlan"));
		}
Error:
	return error;
}


int32 closeExternalLOHandle(void)
{
	int error=0;
	
	if (externalLOSession) 
	{
		checkWarn(niRFSG_Abort(externalLOSession));
		checkWarn(niRFSG_ConfigureOutputEnabled (externalLOSession, VI_FALSE));
		checkWarn(niRFSG_close(externalLOSession));
	}
Error:
	return error;
}