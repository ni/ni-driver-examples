/****************************************************************************
 *          National Instruments WLAN Signal Generator
 *---------------------------------------------------------------------------
 *   Copyright (c) National Instruments 2017.  All Rights Reserved.
 *---------------------------------------------------------------------------
 *
 * Title:    80211GenerateWaveform(MultiUserOFDMA).c
 * Purpose:  This example demonstrates generation of a mulit-user 802.11ax or mulit-user 802.11be waveform 
 *			 using multiple NI vector signal generators.
 *
 Comments

1. For each generator,
     A. Open an NI-RFSG session.
     B. Configure basic NI-RFSG properties. Set Generation Mode to Script and Power Level Type to Peak Power.
2. Open an NI WLAN Generation session.
3. Configure basic WLAN generation properties. 
    - Set Standard to 80211AX MIMOOFDM or 80211BE MIMOOFDM.
    - Set PPDU Type to MU PPDU to generate a multi-user PPDU or "Trigger based PPDU" to generate Trigger based PPDU.
    - Set Number of Users in the MU PPDU/Trigger based PPDU.
4. For each user,
    A. Create a user active channel string.
    B. Set RU Size, RU Offset, MCS Index and Number of Space Time Streams, Payload Data Length (bytes).
5. For Trigge-based PPDU, set Triiger frame common info parameters.
6. Enable RF blanking to attenuate the RF OUT signal during the idle interval. 
7. Synchronize the generators. Configure the reference clock source for the master (the first generator) and
   share that with slaves. Configure the generators for daisy-chained local oscillator (LO) sharing.
8. Configure frequency on the generator (s). 
9. Create the waveform and download it to the NI RF vector signal generators memory.
10. For each generator, configure NI-RFSG for the waveform mentioned in the script11. Initiate signal generation
    by calling niWLANG RFSG Multiple Device Initiate.
11. Initiate signal generation by calling niWLANG RFSG Multiple Device Initiate VI.
12. 
    A. Check the generation status.
    B. Exit if an error has occurred, the Stop button is pressed or generation is complete.
13. For each generator,
    A. Abort signal generation.
    B. Disable the output. This sets the noise floor as low as possible.
    C. Clear the waveform from the device memory. Clear the waveform properties from RFSG database.
    D. Close the NI-RFSG session.
14. Read signal properties (waveform size and actual headroom) for display purpose.
15. Close the NI WLAN Generation session.

 ****************************************************************************/
#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <string.h>
#include "niWLANGenerationRfsg.h"
#include "niTCLK.h"

#define MAX_WLAN_CHANNELS 8
/*--------------------------------------------------------------------------*/
/* Global Variables                                                         */
/*--------------------------------------------------------------------------*/
static niWLANGenerationSession gSession = NULL;
static char errorMessage[NIWLANG_VAL_MAX_ERROR_MESSAGE_SIZE];
static ViSession rfsgSession[MAX_WLAN_CHANNELS];
static ViSession externalLOSession = 0;
static int32 isNewSession = 0;
static int32 numTx,mappingMatrixType,numberOfUsers,guardIntervalType;
static float64 channelBandwidth,carrierFrequency;

static char *rfsgResourceName[MAX_WLAN_CHANNELS] = {"RIO0","RIO1","RIO2","RIO3","RIO4","RIO5","RIO6","RIO7"};
static float64 powerLevel[MAX_WLAN_CHANNELS] = {-10,-10,-10,-10,-10,-10,-10,-10};
static int32 externalAttenuation[MAX_WLAN_CHANNELS] = {0,0,0,0,0,0,0,0};
static int32 mcsIndex[]={0,0,0,0,0};
static int32 ruSize[] = {NIWLANG_VAL_RU_SIZE_26,NIWLANG_VAL_RU_SIZE_26,NIWLANG_VAL_RU_SIZE_52,
	                     NIWLANG_VAL_RU_SIZE_26,NIWLANG_VAL_RU_SIZE_106};
static int32 ruOffsetMruIndex[] = {0,1,2,4,5};
static int32 numberOfSpaceTimeStreams[] ={1,1,1,1,1};
static int32 payloadDataLength[] = {100,100,100,100,100};
static int32 autoPayloadLength,lSIGLength,preFECPaddingFactor,peDisambiguity,ldpcExtraSymbolSegment;
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
	numTx = 1;	
	mappingMatrixType = NIWLANG_VAL_MAPPING_MATRIX_TYPE_DIRECT;
	guardIntervalType = NIWLANG_VAL_GUARD_INTERVAL_TYPE_ONE_BY_FOUR;
	numberOfUsers = 5;
	autoPayloadLength = NIWLANG_VAL_FALSE;
	lSIGLength = -1;
	preFECPaddingFactor = -1;
	ldpcExtraSymbolSegment = -1;
	peDisambiguity = -1;
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
	checkWarn(niWLANG_SetStandard(gSession, NULL, NIWLANG_VAL_STANDARD_80211AX_MIMO_OFDM));	/*802.11ax standard*/
	checkWarn(niWLANG_SetChannelBandwidth(gSession,NULL,channelBandwidth));
	checkWarn(niWLANG_SetNumberOfTransmitChannels(gSession, NULL, numTx));
	checkWarn(niWLANG_SetMappingMatrixType(gSession, NULL, mappingMatrixType));
	checkWarn(niWLANG_SetPPDUType(gSession, NULL, NIWLANG_VAL_PPDU_TYPE_MU_PPDU));
	checkWarn(niWLANG_SetOFDMGuardIntervalType(gSession,NULL, guardIntervalType));
	checkWarn(niWLANG_SetNumberOfUsers(gSession, NULL, numberOfUsers));

	for( i=0; i<numberOfUsers; i++ )
	{
		sprintf(activeChannel,"user%d",i);
		checkWarn(niWLANG_SetRUSize(gSession,activeChannel,ruSize[i]));
		checkWarn(niWLANG_SetRUOffsetMRUIndex(gSession, activeChannel, ruOffsetMruIndex[i]));
		checkWarn(niWLANG_SetMCSIndex(gSession, activeChannel, mcsIndex[i]));
		checkWarn(niWLANG_SetNumberOfSpaceTimeStreams(gSession, activeChannel, numberOfSpaceTimeStreams[i]));
		sprintf(activeChannel,"user%d/mpdu0",i);
		checkWarn(niWLANG_SetPayloadDataLength(gSession,activeChannel,payloadDataLength[i]));
	}
	if(autoPayloadLength == NIWLANG_VAL_OFDM_DATA_RATE_18)
	{
		checkWarn(niWLANG_SetAutoNumberOfMPDUs(gSession,activeChannel,autoPayloadLength));
		checkWarn(niWLANG_SetAutoDataLength(gSession,activeChannel,autoPayloadLength));
		checkWarn(niWLANG_SetOFDMLSIGLength(gSession, activeChannel,lSIGLength));
		checkWarn(niWLANG_SetOFDMPreFECPaddingFactor(gSession, activeChannel, preFECPaddingFactor));
		checkWarn(niWLANG_SetOFDMPEDisambiguity(gSession, activeChannel, peDisambiguity));
		checkWarn(niWLANG_SetOFDMLDPCExtraSymbolsUsed(gSession,activeChannel,ldpcExtraSymbolSegment));

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
			checkWarn(niRFSG_init(rfsgResourceName[i], VI_TRUE ,VI_FALSE, &rfsgSession[i]));
		}
	}

	for(i=0;i<numTx;i++)
	{
		checkWarn(niRFSG_ConfigurePowerLevelType(rfsgSession[i], NIRFSG_VAL_PEAK_POWER));
		checkWarn(niRFSG_ConfigureGenerationMode(rfsgSession[i], NIRFSG_VAL_SCRIPT));	
		checkWarn(niRFSG_SetAttributeViReal64(rfsgSession[i], NULL, NIRFSG_ATTR_EXTERNAL_GAIN, -(externalAttenuation[i])));
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
	int32 waveFormSize;
	float64 actualHeadroom[MAX_WLAN_CHANNELS];
	char channelString[100];

	checkWarn(niWLANG_RFSGConfigureMultipleDeviceSynchronization(gSession,rfsgSession,numTx,clkSource,triggerLines,2 ));
	checkWarn(niWLANG_RFSGConfigureFrequencySingleLO(gSession,rfsgSession,numTx,NIWLANG_VAL_LO_SOURCE_ONBOARD,externalLOSession,carrierFrequency,
                                           		    NIWLANG_VAL_FALSE,NIWLANG_VAL_FALSE));
	checkWarn(niWLANG_RFSGCreateAndDownloadMIMOWaveforms(gSession,rfsgSession,NULL,numTx,"Wlan"));
	
	for (i=0; i<numTx; i++)
	{
		checkWarn(niWLANG_RFSGConfigureScript(rfsgSession[i],"",script, powerLevel[i]));
	}
	checkWarn(niWLANG_RFSGMultipleDeviceInitiate(gSession,rfsgSession,numTx));
	checkWarn(niWLANG_GetIQWaveformSize(gSession, "",&waveFormSize));
	checkWarn(niWLANG_GetIQRate(gSession, "", &IQRate));
	printf("Waveform Duration (s): %lf",(waveFormSize/IQRate));
	for (i=0; i<numTx; i++)
	{
		sprintf(channelString,"channel %d",i);
		checkWarn(niWLANG_GetActualHeadroom(gSession,channelString, &actualHeadroom[i]));
		printf("\nActual Headroom (dB) [%d]: %lf",i,actualHeadroom[i]);
	}

	printf("\nGenerating Signal...\n");
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
	}while (!isDone && !_kbhit() );
Error:
	return error;
}


/*--------------------------------------------------------------------------*/
/*Stop the Generation on pressing Stop or Quit Button						*/
/*--------------------------------------------------------------------------*/
int32 stopGeneration (void)
{
	int i,error=0;;
	
	for (i=0; i<numTx; i++)
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
