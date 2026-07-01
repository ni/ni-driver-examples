/****************************************************************************
 *          National Instruments WLAN Signal Generator
 *---------------------------------------------------------------------------
 *   Copyright (c) National Instruments 2017.  All Rights Reserved.
 *---------------------------------------------------------------------------
 *
 * Title:    80211GenerateTriggerFrameWaveform.c
 * Purpose:  This example demonstrates generation of 802.11ax Trigger Frame waveform with standards 802.11a/g,
 *           single-channel 802.11n, single-channel 802.11ac, single-channel 802.11ax 
 *           or single-channel 802.11be using a NI vector signal generator.
 *
 Comments:
1. Open an NI-RFSG session. 
2. Configure basic NI-RFSG properties. Set Generation Mode to Script and Power Level Type to Peak Power.
3. Configure frequency on the analyser.
4. Open an NI WLAN Generation session.
5. Configure basic WLAN generation properties for Trigger Frame Content.
6. Generate MSDU bits for Trgger Frame.
7. Open another NI WLAN Generation session.
8.Configure basic WLAN generation properties specific to the standard set for Trigger Frame generation.
9. Enable RF blanking to attenuate the RF OUT signal during the idle interval. 
10.Set payload data type as user    defined pattern and Trigger Frame MSDU bits as user defined bits with appropriate payload data length
11. Create the waveform and download it to the NI RF vector signal generator memory.
12. Configure NI-RFSG for the waveform mentioned in the script.
13. Initiate signal generation.
14. 
    A. Check the generation status.
    B. Exit if an error has occurred or the Stop button is pressed.
15 Abort signal generation.
16. Disable the output. This sets the noise floor as low as possible.
17. Clear the waveform from the device memory. Clear the waveform properties from RFSG database.
18. Close the NI-RFSG session.
19. Read waveform size and actual headroom results.
20. Close NI WLAN Generation sessions.
 ****************************************************************************/
#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include "niWLANGenerationRfsg.h"
/*--------------------------------------------------------------------------*/
/* Global Variables                                                         */
/*--------------------------------------------------------------------------*/
static niWLANGenerationSession gSession = NULL;
static niWLANGenerationSession triggerFrameGSession = NULL ;
static char errorMessage[NIWLANG_VAL_MAX_ERROR_MESSAGE_SIZE];
static ViSession rfsgSession = 0;
static ViSession externalLOSession = 0;
static int32 isNewSession = 0;

static float64 powerLevel,externalAttenuation;
static float64 carrierFrequency;

static float64 channelBandwidth;
static int32 standard,MCSIndex,payloadDataLength,ofdmDataRate;
static int32 macFrameControl,macDurationOrID,frameType,macPaddingDuration,midamblePeriodicity;
static int64 macHeaderRA,macHeaderTA;

static int32 lSIGLength,preFECPaddingFactor, peDisambiguity, ldpcExtraSymbolSegment;

static int32 triggerFrameGuardIntervalType;
static int32 triggerFrameNumberOfUsers,triggerFrameAPTxPower;
static int32 csRequired,heLTFSize;
static int32* triggerFrameMSDUBits = NULL;
static int32 triggerFrameRUSize;
static int32 triggerFrameRUOffset;
static int32 triggerFrameMCSIndex;
static int32 triggerFrameNumOfSpaceTimeStreams;
static int32 triggerFrameDCMEnabled;
static int32 triggerFrameFECCodingTypes;
static int32 triggerFramePayloadDataLength;
static int32 triggerFrameTargetRSSI;
static int32 triggerFrameAID12;
static float64 triggerFramechannelBandwidth;
static int32 triggerFrameSTBCAllStreamEnabled,triggerFrameNumberOfHELTFSymbols;


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
	int32 error = 0, rfsgError;

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
  		niRFSG_GetError	(rfsgSession, &rfsgError, NIWLANG_VAL_MAX_ERROR_MESSAGE_SIZE, errorMessage);
		if(strlen(errorMessage) == 0 || error!=rfsgError)
   		niWLANG_GetErrorString (gSession, error, errorMessage, NIWLANG_VAL_MAX_ERROR_MESSAGE_SIZE);
	}
	if (error < 0)
		printf("ERROR: %s\n", errorMessage);
	else if (error > 0)
		printf("WARNING: %s\n", errorMessage);
	if(error !=0)
	{
	  printf("\nPress any key to exit...\n");
	  _getch();
	}
	/*Close Trigger-Frame session*/
	if(triggerFrameGSession)
		niWLANG_CloseSession(triggerFrameGSession);
	/*Close WLAN Generation session*/
	if (gSession)
		niWLANG_CloseSession(gSession);
	/*Close RFSG session*/	
	if (rfsgSession)
		niRFSG_close (rfsgSession);

	return error;
}

/*--------------------------------------------------------------------------*/
/* Function to initialize the global variables                              */
/*--------------------------------------------------------------------------*/
int32 initGlobalVaribales(void)
{
	//Set the parameters specific to the toolkit
	standard =  NIWLANG_VAL_STANDARD_80211AG_OFDM;
	triggerFrameRUSize = NIWLANG_VAL_RU_SIZE_26;
    triggerFrameRUOffset = 0;
    triggerFrameMCSIndex = 0;
    triggerFrameNumOfSpaceTimeStreams = 1;
    triggerFrameDCMEnabled = NIWLANG_VAL_FALSE;
    triggerFrameFECCodingTypes = NIWLANG_VAL_FEC_CODING_TYPE_LDPC;
	midamblePeriodicity = NIWLANG_VAL_MIDAMBLE_PERIODICITY_NONE;
    triggerFramePayloadDataLength = 100;
    triggerFrameTargetRSSI = 78;
    triggerFrameAID12 = 0;

	macFrameControl = 0x0000;
	macDurationOrID = 0x0000;
    macHeaderRA = 0x000000000000;
	macHeaderTA = 0x000000000000;

	channelBandwidth = 20e+6;
	MCSIndex = 0;

	lSIGLength = -1;
	preFECPaddingFactor = -1;
	peDisambiguity = -1;
	ldpcExtraSymbolSegment = -1;

	//Trigger Frame Controls
	triggerFramechannelBandwidth = 20e+6;
	triggerFrameGuardIntervalType = NIWLANG_VAL_GUARD_INTERVAL_TYPE_ONE_BY_FOUR;
	csRequired = 0;
	heLTFSize = NIWLANG_VAL_HE_LTF_SIZE_AUTO;
	triggerFrameSTBCAllStreamEnabled = NIWLANG_VAL_FALSE;
	triggerFrameNumberOfHELTFSymbols = -1;
	triggerFrameAPTxPower = 0;
	triggerFrameNumberOfUsers = 1;	
	ofdmDataRate = NIWLANG_VAL_OFDM_DATA_RATE_6;
	frameType = NIWLANG_VAL_PAYLOAD_MAC_FRAME_TYPE_TRIGGER_FRAME;
	macPaddingDuration = NIWLANG_VAL_PADDING_DURATION_0US;

	carrierFrequency = 5.18e9;
	powerLevel = -10;
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
	if(!rfsgSession)
		checkWarn(niRFSG_init(rfsgResourceName, VI_TRUE , VI_FALSE, &rfsgSession));
    
	checkWarn(niRFSG_ConfigureRefClock(rfsgSession, clkSource, 10e6));
	checkWarn(niRFSG_SetAttributeViReal64(rfsgSession, NULL, NIRFSG_ATTR_EXTERNAL_GAIN, -externalAttenuation));
	checkWarn(niRFSG_ConfigurePowerLevelType(rfsgSession, NIRFSG_VAL_PEAK_POWER));
	checkWarn(niRFSG_ConfigureGenerationMode(rfsgSession, NIRFSG_VAL_SCRIPT));
 
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
	int32 actualArraySize;

	/* Create Trigger Frame MSDU Bits*/
	if(!triggerFrameGSession)
		checkWarn(niWLANG_OpenSession("WLANG", NIWLANG_VAL_COMPATIBILITY_VERSION_060000, &triggerFrameGSession, &isNewSession));
	checkWarn(niWLANG_SetStandard(triggerFrameGSession, NULL, NIWLANG_VAL_STANDARD_80211AX_MIMO_OFDM));
	checkWarn(niWLANG_SetChannelBandwidth(triggerFrameGSession, NULL, triggerFramechannelBandwidth));
	checkWarn(niWLANG_SetOFDMGuardIntervalType(triggerFrameGSession, NULL, triggerFrameGuardIntervalType));
	checkWarn(niWLANG_SetPPDUType(triggerFrameGSession, NULL, NIWLANG_VAL_PPDU_TYPE_TRIGGER_BASED_PPDU));
	checkWarn(niWLANG_SetAPTXPower(triggerFrameGSession, NULL, triggerFrameAPTxPower));
	checkWarn(niWLANG_SetAMPDUEnabled(triggerFrameGSession, NULL, NIWLANG_VAL_FALSE));
	checkWarn(niWLANG_SetSTBCAllStreamsEnabled(triggerFrameGSession, NULL, triggerFrameSTBCAllStreamEnabled));
	checkWarn(niWLANG_SetNumberOfHELTFSymbols(triggerFrameGSession, NULL, triggerFrameNumberOfHELTFSymbols));
	checkWarn(niWLANG_SetNumberOfUsers(triggerFrameGSession, NULL,triggerFrameNumberOfUsers));
	checkWarn(niWLANG_SetCSRequired(triggerFrameGSession,NULL, csRequired));
	checkWarn(niWLANG_SetOFDMHELTFSize(triggerFrameGSession,NULL, heLTFSize));
	checkWarn(niWLANG_SetOFDMMidamblePeriodicity(triggerFrameGSession,NULL, midamblePeriodicity));

    sprintf(channelString,"user0");
	checkWarn(niWLANG_SetRUSize(triggerFrameGSession, channelString, triggerFrameRUSize));
	checkWarn(niWLANG_SetRUOffset(triggerFrameGSession, channelString, triggerFrameRUOffset));
	checkWarn(niWLANG_SetMCSIndex(triggerFrameGSession, channelString, triggerFrameMCSIndex));
	checkWarn(niWLANG_SetNumberOfSpaceTimeStreams(triggerFrameGSession, channelString, triggerFrameNumOfSpaceTimeStreams));
	checkWarn(niWLANG_SetDualCarrierModulationEnabled(triggerFrameGSession, channelString, triggerFrameDCMEnabled));
	checkWarn(niWLANG_SetFECCodingType(triggerFrameGSession, channelString, triggerFrameFECCodingTypes));
	checkWarn(niWLANG_SetPayloadDataLength(triggerFrameGSession, channelString, triggerFramePayloadDataLength));
	checkWarn(niWLANG_SetTargetRSSI(triggerFrameGSession, channelString, triggerFrameTargetRSSI));
	checkWarn(niWLANG_SetSTAID(triggerFrameGSession, channelString, triggerFrameAID12));
	
	if(lSIGLength == -1)
	{
		checkWarn(niWLANG_SetScalarAttributeI32(triggerFrameGSession,channelString,NIWLANG_MAC_FCS_ENABLED,NIWLANG_VAL_FALSE));
		checkWarn(niWLANG_SetScalarAttributeI32(triggerFrameGSession,channelString,NIWLANG_MAC_HEADER_ENABLED,NIWLANG_VAL_FALSE));
		checkWarn(niWLANG_SetScalarAttributeI32(triggerFrameGSession,channelString,NIWLANG_PAYLOAD_DATA_LENGTH,triggerFramePayloadDataLength));
	}
	else
	{
	    checkWarn(niWLANG_SetScalarAttributeI32(triggerFrameGSession,NULL,NIWLANG_L_SIG_LENGTH,lSIGLength));
	    checkWarn(niWLANG_SetScalarAttributeI32(triggerFrameGSession,NULL,NIWLANG_PRE_FEC_PADDING_FACTOR,preFECPaddingFactor));
	    checkWarn(niWLANG_SetScalarAttributeI32(triggerFrameGSession,NULL,NIWLANG_PE_DISAMBIGUITY,peDisambiguity));
	    checkWarn(niWLANG_SetScalarAttributeI32(triggerFrameGSession,NULL,NIWLANG_LDPC_EXTRA_SYMBOL_SEGMENT,ldpcExtraSymbolSegment));
	    checkWarn(niWLANG_SetScalarAttributeI32(triggerFrameGSession,NULL,NIWLANG_PAYLOAD_AUTO_NUMBER_OF_MPDUS,NIWLANG_VAL_TRUE));
	    checkWarn(niWLANG_SetScalarAttributeI32(triggerFrameGSession,NULL,NIWLANG_PAYLOAD_AUTO_DATA_LENGTH,NIWLANG_VAL_TRUE));
	}
	    


	checkWarn(niWLANG_CreateTriggerFrameMSDU(triggerFrameGSession, NULL, NULL, NULL, 0, &actualArraySize));
	if(actualArraySize >0)
	{
    	triggerFrameMSDUBits = (int32*)malloc(sizeof(int32)*actualArraySize);
		checkWarn(niWLANG_CreateTriggerFrameMSDU(triggerFrameGSession, NULL, NULL, triggerFrameMSDUBits, actualArraySize, NULL));
	}

	/*Configure toolkit Session for Trigger Frame Generation*/
	if(!gSession)
		checkWarn(niWLANG_OpenSession("WLANTG", NIWLANG_VAL_COMPATIBILITY_VERSION_060000, &gSession, &isNewSession));
	checkWarn(niWLANG_SetStandard(gSession, NULL, standard));
	checkWarn(niWLANG_SetChannelBandwidth(gSession, NULL, channelBandwidth));
	checkWarn(niWLANG_SetOFDMDataRate(gSession, NULL,ofdmDataRate));
	checkWarn(niWLANG_SetMCSIndex(gSession, NULL, MCSIndex));

	checkWarn(niWLANG_SetMACFrameType(gSession, NULL, frameType));
	checkWarn(niWLANG_SetMACFrameControl(gSession, NULL, macFrameControl));
	checkWarn(niWLANG_SetMACDurationOrID(gSession, NULL, macDurationOrID));
	checkWarn(niWLANG_SetMACAddress1(gSession, NULL, macHeaderRA));
	checkWarn(niWLANG_SetMACAddress2(gSession, NULL, macHeaderTA));
	checkWarn(niWLANG_SetMACPaddingDuration(gSession, NULL, macPaddingDuration));
	
	checkWarn(niWLANG_SetRFBlankingEnabled(gSession, NULL, NIWLANG_VAL_TRUE));

    checkWarn(niWLANG_SetPayloadDataType(gSession, NULL, NIWLANG_VAL_USER_DEFINED));
	checkWarn(niWLANG_SetPayloadUserDefinedBits(gSession, NULL, triggerFrameMSDUBits, actualArraySize));

	payloadDataLength = (int32) (ceil(actualArraySize/8));
	checkWarn(niWLANG_SetPayloadDataLength(gSession, NULL, payloadDataLength));

Error:
	if(triggerFrameMSDUBits)
	{
		free(triggerFrameMSDUBits);
		triggerFrameMSDUBits=NULL;
	}
	return error;
}

/*--------------------------------------------------------------------------*/
/* Function to create waveform                                              */
/*--------------------------------------------------------------------------*/
int32 createAndDownloadWaveform(void)
{
	int error = 0;
	float64 headroom, IQRate, waveformDuration;
	int32 waveFormSize;

	checkWarn(niWLANG_RFSGConfigureFrequencySingleLO(gSession,&rfsgSession,1,NIWLANG_VAL_LO_SOURCE_ONBOARD,externalLOSession,
		                                             carrierFrequency, NIWLANG_VAL_FALSE, NIWLANG_VAL_FALSE));
	if(standard == NIWLANG_VAL_STANDARD_80211N_MIMO_OFDM || standard == NIWLANG_VAL_STANDARD_80211AC_MIMO_OFDM  ||
		standard == NIWLANG_VAL_STANDARD_80211AX_MIMO_OFDM || standard == NIWLANG_VAL_STANDARD_80211BE_MIMO_OFDM)
	{	
		checkWarn(niWLANG_RFSGCreateAndDownloadMIMOWaveforms(gSession, &rfsgSession, NULL, 1, "Wlan"));
		strcpy (channelString, "channel0");
	}
	else
	{
		checkWarn(niWLANG_RFSGCreateAndDownloadWaveform(gSession, rfsgSession, NULL, "Wlan"));
		strcpy (channelString, "");
	}	

    checkWarn(niWLANG_GetIQRate(gSession, "", &IQRate));	
	checkWarn(niWLANG_GetIQWaveformSize(gSession, "", &waveFormSize));
	waveformDuration = waveFormSize/IQRate;
	checkWarn(niWLANG_GetActualHeadroom(gSession, channelString, &headroom));
	
	printf("\nWaveform Duration (s) : %lf\n", waveformDuration);
	printf("Actual Headroom (dB) : %lf ", headroom);

	/*Configure Script*/
	checkWarn(niWLANG_RFSGConfigureScript(rfsgSession, "", script, powerLevel)); 
	checkWarn(niRFSG_Commit(rfsgSession));
	niRFSG_ConfigureOutputEnabled (rfsgSession, VI_TRUE);
		
	/*Initiate Generation*/ 
	checkWarn(niRFSG_Initiate(rfsgSession));

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
    printf("\n\nGenerating Trigger Frame Waveform");
	printf("\nPress any key to abort generation\n");
	do
	{
		/*Check Generation Status*/
		checkWarn(niRFSG_CheckGenerationStatus (rfsgSession,&isDone));
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
	
	checkWarn(niRFSG_Abort(rfsgSession));
	checkWarn(niRFSG_ConfigureOutputEnabled (rfsgSession, VI_FALSE));
	checkWarn(niRFSG_Commit(rfsgSession));	
	checkWarn(niWLANG_RFSGClearDatabase(rfsgSession , "", "Wlan"));

Error:
	return error;
}
