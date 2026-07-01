/****************************************************************************
 *          National Instruments WLAN Signal Generator
 *---------------------------------------------------------------------------
 *   Copyright (c) National Instruments 2017.  All Rights Reserved.
 *---------------------------------------------------------------------------
 *
 * Title:    80211acGenerateWaveform(MultiUser).c
 * Purpose:  This example demonstrates Generation of a multi-user multiple-channel(MU MIMO) 802.11ac waveform 
 *			 using multiple NI vector signal generators.
 *
 Comments:
1. For each generator,
     A. Open an NI-RFSG session.
     B. Configure basic NI-RFSG properties. Set Generation Mode to Script and Power Level Type to Peak Power.
2. Open an NI WLAN Generation session.
3. Configure basic WLAN generation properties. 
    - Set Standard to 80211AC MIMOOFDM.
    - Set PPDU Type to MU PPDU to generate a multi-user PPDU.
    - Set Number of Users in the MU PPDU.
4. For each user,
    A. Create a user active channel string.
    B. Set MCS Index and Number of Space Time Streams.
5. Enable RF blanking to attenuate the RF OUT signal during the idle interval. 
6. Synchronize the generators. Configure the reference clock source for the master (the first generator) and share that with slaves. Configure the generators for daisy-chained local oscillator (LO) sharing.
7. Configure frequency on all the generators. 
8.  For each generator, configure NI-RFSG for the waveform mentioned in the script.
9. Initiate signal generation by calling niWLANG RFSG Multiple Device Initiate.
10. 
    A. Check the generation status.
    B. Exit if an error has occurred, the Stop button is pressed or generation is complete.
11. For each generator,
    A. Abort signal generation.
    B. Disable the output. This sets the noise floor as low as possible.
    C. Clear the waveform from the device memory. Clear the waveform properties from RFSG database.
    D. Close the NI-RFSG session.
12. Create the waveform and download it to the NI RF vector signal generators memory.
13. Read signal properties (waveform size and actual headroom) for display purpose.
14. Close the NI WLAN Generation session.

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
static ViSession rfsgSession[MAX_WLAN_CHANNELS];
static ViSession externalLOSession = 0;
static int32 isNewSession = 0;
static int32 standard,numTx,mappingMatrixType,noOfUsers;
static float64 channelBandwidth;
static float64 carrierFrequency;

static int32 externalAttenuation[] = {0,0,0,0};
static float64 powerLevel[] = {-10,-10,-10,-10};
static int32 mcsIndex[]={0,8};
static int32 noOfSpaceTimeStreams[] ={1,1};
static char	*rfsgResourceName[] = {"RIO0",
								   "RIO1",
								   "RIO2",
								   "RIO3"};
static char *clkSource =  "PXI_Clk";
static int32 triggerLines[] = {0,1};
static char script[1024];
/*--------------------------------------------------------------------------*/
/* Forward Declaration of the functions                                     */
/*--------------------------------------------------------------------------*/

int32 initGlobalVaribales(void);
int32 configureToolkitSession(void);
int32 configureRfsgSession(void);
int32 createAndDownloadWaveform(void);
int32 checkGeneration (void);
int32 stopGeneration (void);

int main (int argc, char *argv[])
{
	int32 error = 0,i, rfsgError;

	checkWarn(initGlobalVaribales());
	checkWarn(configureToolkitSession());
	checkWarn(configureRfsgSession());
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

	for (i=0; i<numTx; i++)
	{
		if(rfsgSession[i])
		{
			error = niRFSG_close (rfsgSession[i]);
		}
	}

	if(gSession)
		error = niWLANG_CloseSession (gSession);

	return error;
}

/*--------------------------------------------------------------------------*/
/* Function to initialize the global variables                              */
/*--------------------------------------------------------------------------*/
int32 initGlobalVaribales(void)
{
	//Set the parameters specific to Hardware
	carrierFrequency = 5.18e9;

	//Set the parameters specific to the toolkit
	channelBandwidth = 20e+6;
	numTx = 2;
	mappingMatrixType = NIWLANG_VAL_MAPPING_MATRIX_TYPE_DIRECT;
	noOfUsers=2;

	return 0;
}

/*--------------------------------------------------------------------------*/
/* Function to configure RFSG Session                                       */
/*--------------------------------------------------------------------------*/
int32 configureToolkitSession(void)
{
	int32 error = 0,i;
	char activeChannel[100];

	//open WLAN Session
	if(!gSession)
		checkWarn(niWLANG_OpenSession("WLANG", NIWLANG_VAL_COMPATIBILITY_VERSION_060000, &gSession, &isNewSession));

	/*Set Properties to the WLAN Generation Session*/
	checkWarn(niWLANG_SetStandard(gSession, NULL, NIWLANG_VAL_STANDARD_80211AC_MIMO_OFDM));	/*802.11ac standard*/
	checkWarn(niWLANG_SetNumberOfTransmitChannels(gSession, NULL, numTx));
	checkWarn(niWLANG_SetChannelBandwidth(gSession, NULL, channelBandwidth));
	checkWarn(niWLANG_SetMappingMatrixType(gSession, NULL, mappingMatrixType));
	checkWarn(niWLANG_SetPPDUType(gSession, NULL, NIWLANG_VAL_PPDU_TYPE_MU_PPDU));
	checkWarn(niWLANG_SetNumberOfUsers(gSession, NULL, noOfUsers));


	for( i=0; i<numTx; i++ )
	{
		sprintf(activeChannel,"user%d",i);
		checkWarn(niWLANG_SetMCSIndex(gSession, activeChannel, mcsIndex[i]));
		checkWarn(niWLANG_SetNumberOfSpaceTimeStreams(gSession, activeChannel, noOfSpaceTimeStreams[i]));
	}

	checkWarn(niWLANG_SetRFBlankingEnabled(gSession, NULL, NIWLANG_VAL_TRUE));

Error:
	return error;
}

/*--------------------------------------------------------------------------*/
/* Function to Configure the Hardware
/*--------------------------------------------------------------------------*/
int32 configureRfsgSession(void)
{
	int error = 0,i;

	//Open all RFSG Sessions
	for (i=0; i<numTx; i++)
	{
		if(!rfsgSession[i])
		{
			checkWarn(niRFSG_init(rfsgResourceName[i], VI_TRUE,VI_FALSE , &rfsgSession[i]));
		}
	}

	for(i=0;i<numTx;i++)
	{
		checkWarn(niRFSG_ConfigurePowerLevelType(rfsgSession[i], NIRFSG_VAL_PEAK_POWER));
		checkWarn(niRFSG_ConfigureGenerationMode(rfsgSession[i], NIRFSG_VAL_SCRIPT));	
		checkWarn(niRFSG_SetAttributeViReal64(rfsgSession[0], NULL, NIRFSG_ATTR_EXTERNAL_GAIN, -(externalAttenuation[i]) ));
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
/* Function to create waveform                                              */
/*--------------------------------------------------------------------------*/
int32 createAndDownloadWaveform(void)
{
	int error=0,i;
	float64 IQRate;
	float64 actualHeadroom[MAX_WLAN_CHANNELS];
	char channelString[100];

	checkWarn(niWLANG_RFSGConfigureMultipleDeviceSynchronization(gSession,rfsgSession,numTx,clkSource,triggerLines,2 ));
	checkWarn(niWLANG_RFSGConfigureFrequencySingleLO(gSession,rfsgSession,numTx,NIWLANG_VAL_LO_SOURCE_ONBOARD,externalLOSession,
		                                             carrierFrequency,NIWLANG_VAL_FALSE,NIWLANG_VAL_FALSE));
	checkWarn(niWLANG_RFSGCreateAndDownloadMIMOWaveforms(gSession,rfsgSession,NULL,numTx,"Wlan"));
	/*Configure Script*/
	for (i=0; i<numTx; i++)
	{
		checkWarn(niWLANG_RFSGConfigureScript(rfsgSession[i],"",script, powerLevel[i]));
	}
	checkWarn(niWLANG_RFSGMultipleDeviceInitiate(gSession,rfsgSession,numTx));
	checkWarn(niWLANG_GetIQRate(gSession, "", &IQRate));
	printf("\nIQ Rate : %lf",IQRate);
	for (i=0; i<numTx; i++)
	{
		sprintf(channelString,"channel%d",i);
		checkWarn(niWLANG_GetActualHeadroom(gSession,channelString, &actualHeadroom[i]));
		printf("\nActual Headroom User%d: %lf",i,actualHeadroom[i]);
	}
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
	int32 error = 0,i;
	ViBoolean isDone = 0;
	do
	{
  	   for(i=0; i < numTx; i++)
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
	int i,error=0;;
	
	/*- Disable the output.  This sets the noise floor as low as possible. -*/
	for (i=0; i<numTx; i++)
		if (rfsgSession[i]) 
		{
			checkWarn(niRFSG_Abort(rfsgSession[i]));
			checkWarn(niRFSG_ConfigureOutputEnabled (rfsgSession[i], VI_FALSE));
			checkWarn(niRFSG_Commit(rfsgSession[i]));
			checkWarn(niWLANG_RFSGClearDatabase(rfsgSession[i], "", NULL));
		}
 
Error:
	return error;
}
