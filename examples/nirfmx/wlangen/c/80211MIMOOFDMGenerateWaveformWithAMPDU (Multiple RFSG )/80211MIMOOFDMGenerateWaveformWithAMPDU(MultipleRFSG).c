/****************************************************************************
 *          National Instruments WLAN Signal Generator
 *---------------------------------------------------------------------------
 *   Copyright (c) National Instruments 2017.  All Rights Reserved.
 *---------------------------------------------------------------------------
 *
 * Title:    80211MIMOOFDMGenerateWaveformWithAMPDU(MultipleRFSG).c
 * Purpose:  This example demonstrates generation of 802.11n, 802.11ac, 802.11ah, 802.11af, 802.11ax or 802.11be waveform using multiple NI vector signal generators. 
 *			 The example also demostrates how to generate a PPDU with A-MPDU when PPDU Type is single-user (SU) or multi-user (MU).
 *
Comments:

1. For each generator,
     A. Open an NI-RFSG session.
     B. Configure basic NI-RFSG properties. Set Generation Mode to Script and Power Level Type to Peak Power.
2. Open an NI WLAN Generation session.
3. Configure basic WLAN generation properties. 
    - Set A-MPDU Enabled to True.
4. Enable RF blanking to attenuate the RF OUT signal during the idle interval. 
5. If PPDU Type is MU PPDU, do the following configurations for each user. If PPDU Type is SU PPDU or Extended Range SU PPDU, do the following configurations only once.
    A. Create a user active channel string if PPDU Type is MU PPDU. Use empty active channel string if PPDU Type is SU PPDU or Extended Range SU PPDU.
    B. Set MCS Index, Number of Space Time Streams and Number of MPDUs. In this example, Number of MPDUs is derived from the size of AMPDU Payload Configuration control.
    C. For each MPDU, configure payload settings and MAC header properties.
6. Synchronize the generators. Configure the reference clock source for the master (the first generator) and share that with slaves. 
7. Configure frequency on the generators.
8. Create the waveform and download it to the NI RF vector signal generators memory.
9. For each generator, configure NI-RFSG for the waveform mentioned in the script.
10. Initiate signal generation by calling niWLANG RFSG Multiple Device Initiate.
11. 
    A. Check the generation status.
    B. Exit if an error has occurred, the Stop button is pressed or generation is complete.
12. For each generator,
    A. Abort signal generation.
    B. Disable the output. This sets the noise floor as low as possible.
    C. Clear the waveform from the device memory. Clear the waveform properties from RFSG database.
    D. Close the NI-RFSG session.
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
#define NO_OF_MPDUS 1

typedef struct AMPDUPayloadConfiguration
{
	int32 dataLength;
	int32 dataType;
	int32 PNOrder;
	int32 PNSeed;
	int32 userDefinedBits[1];
	int32 macHeaderEnabled;
	int32 durationID;
	int32 frameControl;
	int32 address1Enabled;
	int32 address1;
	int32 address2Enabled;
	int32 address2;
	int32 address3Enabled;
	int32 address3;
	int32 sequenceControlEnabled;
	int32 sequenceControl;
	int32 address4Enabled;
	int32 address4;
	int32 qosControlEnabled;
	int32 qosControl;
	int32 htControlEnabled;
	int32 htControl;
	int32 sequenceNumberIncrementEnabled;
	int32 sequenceNumberIncrementInterval;
	int32 fragmentNumberIncrementEnabled;
	int32 macFCSEnabled;
}AMPDUPayloadConfig;
/*--------------------------------------------------------------------------*/
/* Global Variables                                                         */
/*--------------------------------------------------------------------------*/
static niWLANGenerationSession gSession = NULL;
static char errorMessage[NIWLANG_VAL_MAX_ERROR_MESSAGE_SIZE];
static ViSession rfsgSession[MAX_WLAN_CHANNELS];
static ViSession externalLOSession = 0;
static int32 isNewSession = 0;
static int32 standard,mcsIndex,numTx,mappingMatrixType,frameFormat,noOfSpaceTimeStreams;
static float64 channelBandwidth;
static float64 carrierFrequency;

static int32 externalAttenuation[] = {0,0,0,0};
static float64 powerLevel[] = {-10,-10,-10,-10};
static int32 preambleType80211ah;

/* Change the channel string for ac/n standards here */
static char *MIMOChannelString[] = {"channel0", 
									"channel1", 
									"channel2", 
									"channel3" };
static char	*rfsgResourceName[] = {"RIO0",
								   "RIO1",
								   "RIO2",
								   "RIO3"};
static char *clkSource =  "PXI_Clk";
static int32 triggerLines[] = {0,1};
static char script[1024];
static int32 ampduEnabled, ppduType, noOfUsers;
static AMPDUPayloadConfig ampduPayloadConfig[NO_OF_MPDUS];

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
	standard = NIWLANG_VAL_STANDARD_80211AC_MIMO_OFDM;
	channelBandwidth = 20e+6;
	numTx = 2;
	mcsIndex = 0;
	mappingMatrixType = NIWLANG_VAL_MAPPING_MATRIX_TYPE_DIRECT;
	frameFormat = NIWLANG_VAL_80211N_PLCP_FRAME_FORMAT_MIXED;
	preambleType80211ah = NIWLANG_VAL_80211AH_PREAMBLE_TYPE_SHORT;
	
	noOfSpaceTimeStreams = 2;
	ampduEnabled = NIWLANG_VAL_TRUE;
	ppduType = NIWLANG_VAL_PPDU_TYPE_SU_PPDU;
	noOfUsers = 1;
	//AMPDU Payload Configuration
	ampduPayloadConfig[0].dataLength = 1024;
	ampduPayloadConfig[0].dataType = NIWLANG_VAL_PN_SEQUENCE;
	ampduPayloadConfig[0].PNOrder = 9;
	ampduPayloadConfig[0].PNSeed = 0xD6BF7DF2;
	ampduPayloadConfig[0].userDefinedBits[0] = 0;
	ampduPayloadConfig[0].macHeaderEnabled = NIWLANG_VAL_TRUE;
	ampduPayloadConfig[0].durationID = 0;
	ampduPayloadConfig[0].frameControl = 0;
	ampduPayloadConfig[0].address1Enabled = NIWLANG_VAL_TRUE;
	ampduPayloadConfig[0].address1 = 0;
	ampduPayloadConfig[0].address2Enabled = NIWLANG_VAL_TRUE;
	ampduPayloadConfig[0].address2 = 0;
	ampduPayloadConfig[0].address3Enabled = NIWLANG_VAL_TRUE;
	ampduPayloadConfig[0].address3 = 0;
	ampduPayloadConfig[0].sequenceControlEnabled = NIWLANG_VAL_TRUE;
	ampduPayloadConfig[0].sequenceControl = 0;
	ampduPayloadConfig[0].address4Enabled = NIWLANG_VAL_TRUE;
	ampduPayloadConfig[0].address4 = 0;
	ampduPayloadConfig[0].qosControlEnabled = NIWLANG_VAL_FALSE;
	ampduPayloadConfig[0].qosControl = 0;
	ampduPayloadConfig[0].htControlEnabled = NIWLANG_VAL_FALSE;
	ampduPayloadConfig[0].htControl = 0;
	ampduPayloadConfig[0].sequenceNumberIncrementEnabled = NIWLANG_VAL_FALSE;
	ampduPayloadConfig[0].sequenceNumberIncrementInterval = 1;
	ampduPayloadConfig[0].fragmentNumberIncrementEnabled = NIWLANG_VAL_FALSE;
	ampduPayloadConfig[0].macFCSEnabled = NIWLANG_VAL_TRUE;

	return 0;
}

/*--------------------------------------------------------------------------*/
/* Function to configure RFSG Session                                       */
/*--------------------------------------------------------------------------*/
int32 configureToolkitSession(void)
{
	int32 error = 0,i,j,numberOfIterations = noOfUsers;
	char userString[50];
	char mpduString[50];

	//open WLAN Session
	if(!gSession)
		checkWarn(niWLANG_OpenSession("WLANG", NIWLANG_VAL_COMPATIBILITY_VERSION_060000, &gSession, &isNewSession));

	/*Set Properties to the WLAN Generation Session*/
	checkWarn(niWLANG_SetStandard(gSession, NULL, standard));	/*802.11n standard*/
	checkWarn(niWLANG_SetNumberOfTransmitChannels(gSession, NULL, numTx));
	checkWarn(niWLANG_SetChannelBandwidth(gSession, NULL, channelBandwidth));
	checkWarn(niWLANG_SetMappingMatrixType(gSession, NULL, mappingMatrixType));
	checkWarn(niWLANG_SetAMPDUEnabled(gSession, NULL, ampduEnabled));
	checkWarn(niWLANG_SetPPDUType(gSession, NULL, ppduType));
	checkWarn(niWLANG_SetNumberOfUsers(gSession, NULL, noOfUsers));
	checkWarn(niWLANG_Set80211nPLCPFrameFormat(gSession, NULL, frameFormat));
	checkWarn(niWLANG_Set80211ahPreambleType(gSession, NULL, preambleType80211ah));

    checkWarn(niWLANG_SetRFBlankingEnabled(gSession, NULL, NIWLANG_VAL_TRUE));

	if(standard == NIWLANG_VAL_STANDARD_80211AC_MIMO_OFDM || standard == NIWLANG_VAL_STANDARD_80211BE_MIMO_OFDM)
	{
		if(ppduType != NIWLANG_VAL_PPDU_TYPE_MU_PPDU)
		{
			numberOfIterations = 1;
		}
		else
		{
			numberOfIterations = noOfUsers;
		}
	}
	else if(standard == NIWLANG_VAL_STANDARD_80211AX_MIMO_OFDM)
	{
		numberOfIterations = 1;
	}
	else
		numberOfIterations = 1;


	for(i=0; i<numberOfIterations; i++)
	{
		if( ppduType == NIWLANG_VAL_PPDU_TYPE_SU_PPDU)
			sprintf(userString, "");
		else
			sprintf(userString,"user%d",i);
		checkWarn(niWLANG_SetMCSIndex(gSession, userString, mcsIndex));
		checkWarn(niWLANG_SetNumberOfSpaceTimeStreams(gSession, userString, noOfSpaceTimeStreams));
		checkWarn(niWLANG_SetNumberOfMPDUs(gSession, userString, NO_OF_MPDUS));

		for(j = 0; j < NO_OF_MPDUS ; j++ )
		{
			if(ppduType == NIWLANG_VAL_PPDU_TYPE_SU_PPDU)
				sprintf(mpduString,"mpdu%d",j);
			else
				sprintf(mpduString,"user%d/mpdu%d",i,j);
			
			checkWarn(niWLANG_SetPayloadDataLength(gSession, mpduString, ampduPayloadConfig[j].dataLength));
			checkWarn(niWLANG_SetPayloadDataType(gSession, mpduString, ampduPayloadConfig[j].dataType));
			checkWarn(niWLANG_SetPayloadPNOrder(gSession, mpduString, ampduPayloadConfig[j].PNOrder));
			checkWarn(niWLANG_SetPayloadPNSeed(gSession, mpduString, ampduPayloadConfig[j].PNSeed));
			checkWarn(niWLANG_SetPayloadUserDefinedBits(gSession, mpduString, ampduPayloadConfig[j].userDefinedBits,1));
			checkWarn(niWLANG_SetMACHeaderEnabled(gSession, mpduString, ampduPayloadConfig[j].macHeaderEnabled));
			checkWarn(niWLANG_SetMACDurationOrID(gSession, mpduString, ampduPayloadConfig[j].durationID));
			checkWarn(niWLANG_SetMACFrameControl(gSession, mpduString, ampduPayloadConfig[j].frameControl));
			checkWarn(niWLANG_SetMACAddress1Enabled(gSession, mpduString, ampduPayloadConfig[j].address1Enabled));
			checkWarn(niWLANG_SetMACAddress1(gSession, mpduString, ampduPayloadConfig[j].address1));
			checkWarn(niWLANG_SetMACAddress2Enabled(gSession, mpduString, ampduPayloadConfig[j].address2Enabled));
			checkWarn(niWLANG_SetMACAddress2(gSession, mpduString, ampduPayloadConfig[j].address2));
			checkWarn(niWLANG_SetMACAddress3Enabled(gSession, mpduString, ampduPayloadConfig[j].address3Enabled));
			checkWarn(niWLANG_SetMACAddress3(gSession, mpduString, ampduPayloadConfig[j].address3));
			checkWarn(niWLANG_SetMACSequenceControlEnabled(gSession, mpduString, ampduPayloadConfig[j].sequenceControlEnabled));
			checkWarn(niWLANG_SetMACSequenceControl(gSession, mpduString, ampduPayloadConfig[j].sequenceControl));
			checkWarn(niWLANG_SetMACAddress4Enabled(gSession, mpduString, ampduPayloadConfig[j].address4Enabled));
			checkWarn(niWLANG_SetMACAddress4(gSession, mpduString, ampduPayloadConfig[j].address4));
			checkWarn(niWLANG_SetMACQOSControlEnabled(gSession, mpduString, ampduPayloadConfig[j].qosControlEnabled));
			checkWarn(niWLANG_SetMACQOSControl(gSession, mpduString, ampduPayloadConfig[j].qosControl));
			checkWarn(niWLANG_SetMACHTControlEnabled(gSession, mpduString, ampduPayloadConfig[j].htControlEnabled));
			checkWarn(niWLANG_SetMACHTControl(gSession, mpduString, ampduPayloadConfig[j].htControl));
			checkWarn(niWLANG_SetMACSequenceNumberIncrementEnabled(gSession, mpduString, ampduPayloadConfig[j].sequenceNumberIncrementEnabled));
			checkWarn(niWLANG_SetMACSequenceNumberIncrementInterval(gSession, mpduString, ampduPayloadConfig[j].sequenceNumberIncrementInterval));
			checkWarn(niWLANG_SetMACFragmentNumberIncrementEnabled(gSession, mpduString, ampduPayloadConfig[j].fragmentNumberIncrementEnabled));
			checkWarn(niWLANG_SetMACFCSEnabled(gSession, mpduString, ampduPayloadConfig[j].macFCSEnabled));
		}
	}


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
		checkWarn(niRFSG_ConfigureGenerationMode(rfsgSession[i], NIRFSG_VAL_ARB_WAVEFORM));	
		checkWarn(niRFSG_SetAttributeViReal64(rfsgSession[i], NULL, NIRFSG_ATTR_EXTERNAL_GAIN, -(externalAttenuation[i]) ));
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
		checkWarn(niWLANG_GetActualHeadroom(gSession,MIMOChannelString[i], &actualHeadroom[i]));
		printf("\nActual Headroom for %s : %lf",MIMOChannelString[i],actualHeadroom[i]);
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
			checkWarn(niRFSG_Commit(rfsgSession[i]));
			checkWarn(niRFSG_ConfigureOutputEnabled (rfsgSession[i], VI_FALSE));
			checkWarn(niWLANG_RFSGClearDatabase(rfsgSession[i], "", NULL));
		}
 
Error:
	return error;
}
