/****************************************************************************
 *          National Instruments WLAN Signal Generator
 *---------------------------------------------------------------------------
 *   Copyright (c) National Instruments 2017.  All Rights Reserved.
 *---------------------------------------------------------------------------
 *
 * Title:    80211beGenerateWaveform(MultiUser).c
 * Purpose:  This example demonstrates generation of multiple-channel 802.11be waveform 
 *			  using multiple NI vector signal generators.
 *
 Comments

1.
     A. Compute number of generators as the number of segments times the number of transmit channels (N_tx).  
     B. Create an array of carrier frequencies with segment 0 carrier frequency, segment 1 carrier frequency if the Number of Segments is equal to 2. otherwise, use the segment 0 carrier frequency.
2. For each generator,
     A. Open an NI-RFSG session.
     B. Configure basic NI-RFSG properties. Set Generation Mode to Script and Power Level Type to Peak Power. 
3. Open an NI WLAN Generation session.
4. Configure basic WLAN generation properties. 
   - Set Standard to 80211BE MIMOOFDM.
5. Set headroom for each channel in each segment.
6. Configures advanced WLANG properties and user specific properties.
7. Configure payload and spectrum control properties.
8. Enable RF blanking to attenuate the RF OUT signal during the idle interval. 
9. Synchronize the generators. Configure the reference clock source for the master (the first generator) and share that with slaves. 
10. Configure frequency on the generators.
11. Create the waveform and download it to the NI RF vector signal generators memory.
12. For each generator, configure NI-RFSG for the waveform mentioned in the script.
13.  Initiate signal generation by calling niWLANG RFSG Multiple Device Initiate VI.
14. 
    A. Check the generation status.
    B. Exit if an error has occurred, the Stop button is pressed or generation is complete.
15. For each generator,
    A. Abort signal generation.
    B. Disable the output. This sets the noise floor as low as possible.
    C. Clear the waveform from the device memory. Clear the waveform properties from RFSG database.
    D. Close the NI-RFSG session.
16. Read signal properties (waveform size, actual headroom and packet extension duration) for display purpose.
17. Close the NI WLAN Generation session

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
static ViSession externalLOSession[MAX_WLAN_CHANNELS] = {NULL};
static int32 isNewSession = 0;
static int32 numTx,mappingMatrixType,numberOfUsers,guardIntervalType;
static int32 swapIandQEnabled, SigCompression, ltfSize, autoHeadroomEnabled;
static int32 numberOfFrames, transmissionMode, nominalPacketPadding, numSeg, ppduType;
static int32 preamblePuncturingEnabled, primary20MhzChannelIndex, preamblePuncturingMask;
static int32 pulseShapingFilterEnabled, filterType, filterLength, ofdmWindowLength, windowingMethod, ruAllocationMode;
static float64 filterParameter;
static float64 packetExtensionDuration, sampleClockRateFactor, idleInterval;
static float64 channelBandwidth;
static float64 carrierFrequency[1] = { 5.18e9 };
static float64 overSamplingRatio;

static char *rfsgResourceName[MAX_WLAN_CHANNELS] = {"RIO0","RIO1","RIO2","RIO3","RIO4","RIO5","RIO6","RIO7" };
static float64 powerLevel[MAX_WLAN_CHANNELS] = {-10,-10,-10,-10,-10,-10,-10,-10};
static int32 externalAttenuation[MAX_WLAN_CHANNELS] = {0,0,0,0,0,0,0,0};
static float64 headroom[MAX_WLAN_CHANNELS] = {12,12,12,12,12,12,12,12};
static int32 mcsIndex[]={0,0,0,0,0};
static int32 ruSize[] = {NIWLANG_VAL_RU_SIZE_26,NIWLANG_VAL_RU_SIZE_26,NIWLANG_VAL_RU_SIZE_52,
	                     NIWLANG_VAL_RU_SIZE_26,NIWLANG_VAL_RU_SIZE_106};
static int32 ruOffset[] = {0,1,2,4,5};
static int32 ruAllocation[] = {80};
static int32 userEnabled[] = {NIWLANG_VAL_TRUE, NIWLANG_VAL_TRUE, NIWLANG_VAL_TRUE, NIWLANG_VAL_TRUE, NIWLANG_VAL_TRUE};
static int32 dcmEnabled[] = {NIWLANG_VAL_FALSE, NIWLANG_VAL_FALSE, NIWLANG_VAL_FALSE, NIWLANG_VAL_FALSE, NIWLANG_VAL_FALSE};
static int32 staID[] = {0,1,2,3,4};
static int32 fecCodingType[] = {NIWLANG_VAL_FEC_CODING_TYPE_LDPC, NIWLANG_VAL_FEC_CODING_TYPE_LDPC, NIWLANG_VAL_FEC_CODING_TYPE_LDPC,
                              NIWLANG_VAL_FEC_CODING_TYPE_LDPC, NIWLANG_VAL_FEC_CODING_TYPE_LDPC};
static int32 powerBoostFactor[] = {1,1,1,1,1};
static int32 numberOfSpaceTimeStreams[] ={1,1,1,1,1};
static int32 payloadDataLength[] = {100,100,100,100,100};
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
	//Set the parameters specific to the toolkit
	channelBandwidth = 20e+6;
	numTx = 1;	
	mappingMatrixType = NIWLANG_VAL_MAPPING_MATRIX_TYPE_DIRECT;
	guardIntervalType = NIWLANG_VAL_GUARD_INTERVAL_TYPE_ONE_BY_FOUR;
	numberOfUsers = 5;
	numSeg = 1;
	swapIandQEnabled = NIWLANG_VAL_FALSE;
	sampleClockRateFactor = 1;
	SigCompression = NIWLANG_VAL_SIG_COMPRESSION_ENABLED_TRUE;
	ltfSize = NIWLANG_VAL_LTF_SIZE_AUTO;
	overSamplingRatio = 1.25;
	autoHeadroomEnabled = NIWLANG_VAL_TRUE;
	numberOfFrames = 1;
	transmissionMode = NIWLANG_VAL_TRANSMISSION_MODE_DOWNLINK;
	nominalPacketPadding = NIWLANG_VAL_NOMINAL_PACKET_PADDING_AUTO;
	preamblePuncturingEnabled = NIWLANG_VAL_FALSE;
	primary20MhzChannelIndex = 0;
	preamblePuncturingMask = 0xFFFF;
	pulseShapingFilterEnabled = NIWLANG_VAL_FALSE;
	filterType = NIWLANG_VAL_FILTER_RECTANGULAR;
	filterLength = 8;
	ofdmWindowLength = 2;
	windowingMethod = NIWLANG_VAL_WIN_METHOD_CENTERED_AT_SYMBOL_BOUNDARY;
	ruAllocationMode = NIWLANG_VAL_RU_ALLOCATION_MODE_INDIVIDUAL;
	filterParameter = 0.5;
	sampleClockRateFactor = 1;
	idleInterval = 100e-6;
	ppduType = NIWLANG_VAL_PPDU_TYPE_MU_PPDU;

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
	checkWarn(niWLANG_SetStandard(gSession, NULL, NIWLANG_VAL_STANDARD_80211BE_MIMO_OFDM));	/*802.11be standard*/
	checkWarn(niWLANG_SetChannelBandwidth(gSession,NULL,channelBandwidth));
	checkWarn(niWLANG_SetOverSamplingRatio(gSession, NULL, overSamplingRatio));
	checkWarn(niWLANG_SetIdleInterval(gSession, NULL, idleInterval));
	checkWarn(niWLANG_SetNumberOfFrames(gSession, NULL, numberOfFrames));
	checkWarn(niWLANG_SetAutoHeadroomEnabled(gSession, NULL, autoHeadroomEnabled));
	checkWarn(niWLANG_SetLTFSize(gSession, NULL, ltfSize));
	checkWarn(niWLANG_SetSIGCompressionEnabled(gSession, NULL, SigCompression));
	checkWarn(niWLANG_SetOFDMGuardIntervalType(gSession,NULL, guardIntervalType));
	
	for (i = 0; i < (numSeg*numTx); i++)
	{
		sprintf(activeChannel, "channel%d", i);
		checkWarn(niWLANG_SetHeadroom(gSession, activeChannel, headroom[i]));
	}

	checkWarn(niWLANG_SetNumberOfTransmitChannels(gSession, NULL, numTx));
	checkWarn(niWLANG_SetMappingMatrixType(gSession, NULL, mappingMatrixType));
	checkWarn(niWLANG_SetNumberOfSegments(gSession, NULL, numSeg));
	checkWarn(niWLANG_SetNumberOfUsers(gSession, NULL, numberOfUsers));
	checkWarn(niWLANG_SetTransmissionMode(gSession, NULL, transmissionMode));
	checkWarn(niWLANG_SetPreamblePuncturingEnabled(gSession, NULL, preamblePuncturingEnabled));
	checkWarn(niWLANG_SetPrimary20MHzChannelIndex(gSession, NULL, primary20MhzChannelIndex));
	checkWarn(niWLANG_SetPreamblePuncturingMask(gSession, NULL, preamblePuncturingMask));
	checkWarn(niWLANG_SetOFDMNominalPacketPadding(gSession, NULL, nominalPacketPadding));

	if (ppduType == NIWLANG_VAL_PPDU_TYPE_SU_PPDU)
	{
		checkWarn(niWLANG_SetPPDUType(gSession, NULL, NIWLANG_VAL_PPDU_TYPE_SU_PPDU));
		checkWarn(niWLANG_SetMCSIndex(gSession, NULL, mcsIndex[0]));
		checkWarn(niWLANG_SetNumberOfSpaceTimeStreams(gSession, NULL, numberOfSpaceTimeStreams[0]));
		checkWarn(niWLANG_SetDualCarrierModulationEnabled(gSession, NULL, dcmEnabled[0]));
		checkWarn(niWLANG_SetFECCodingType(gSession, NULL, fecCodingType[0]));
		checkWarn(niWLANG_SetPayloadDataLength(gSession, "mpdu0", payloadDataLength[0]));
	}

	else
	{
		checkWarn(niWLANG_SetPPDUType(gSession, NULL, NIWLANG_VAL_PPDU_TYPE_MU_PPDU));
		checkWarn(niWLANG_SetRUAllocationMode(gSession, NULL, ruAllocationMode));
		checkWarn(niWLANG_SetRUAllocation(gSession, NULL, ruAllocation, 1));

		if (ruAllocationMode == NIWLANG_VAL_RU_ALLOCATION_MODE_INDIVIDUAL)
		{
			for (i = 0; i < numberOfUsers; i++)
			{
				sprintf(activeChannel, "user%d", i);
				checkWarn(niWLANG_SetRUSize(gSession, activeChannel, ruSize[i]));
				checkWarn(niWLANG_SetRUOffset(gSession, activeChannel, ruOffset[i]));
			}

		}

		else
		{
			checkWarn(niWLANG_GetNumberOfUsersFromRUAllocation(gSession, NULL, &numberOfUsers));
		}

		for (i = 0; i < numberOfUsers; i++)
		{
			sprintf(activeChannel, "user%d", i);
			checkWarn(niWLANG_SetMCSIndex(gSession, activeChannel, mcsIndex[i]));
			checkWarn(niWLANG_SetNumberOfSpaceTimeStreams(gSession, activeChannel, numberOfSpaceTimeStreams[i]));
			checkWarn(niWLANG_SetSTAID(gSession, activeChannel, staID[i]));
			checkWarn(niWLANG_SetDualCarrierModulationEnabled(gSession, activeChannel, dcmEnabled[i]));
			checkWarn(niWLANG_SetFECCodingType(gSession, activeChannel, fecCodingType[i]));
			checkWarn(niWLANG_SetPowerBoostFactor(gSession, activeChannel, powerBoostFactor[i]));
			checkWarn(niWLANG_SetUserEnabled(gSession, activeChannel, userEnabled[i]));
			sprintf(activeChannel, "user%d/mpdu0", i);
			checkWarn(niWLANG_SetPayloadDataLength(gSession, activeChannel, payloadDataLength[i]));
		}		
	}

	checkWarn(niWLANG_SetPulseShapingFilterEnabled(gSession, NULL, pulseShapingFilterEnabled));
	checkWarn(niWLANG_SetPulseShapingFilterType(gSession, NULL, filterType));
	checkWarn(niWLANG_SetPulseShapingFilterParameter(gSession, NULL, filterParameter));
	checkWarn(niWLANG_SetOFDMWindowLength(gSession, NULL, ofdmWindowLength));
	checkWarn(niWLANG_SetWindowingMethod(gSession, NULL, windowingMethod));
	checkWarn(niWLANG_SetPulseShapingFilterLength(gSession, NULL, filterLength));
	checkWarn(niWLANG_SetSwapIAndQEnabled(gSession, NULL, swapIandQEnabled));
	checkWarn(niWLANG_SetSampleClockRateFactor(gSession, NULL, sampleClockRateFactor));

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
	for (i=0; i<(numTx*numSeg); i++)
	{
		if(!rfsgSession[i])
		{
			checkWarn(niRFSG_init(rfsgResourceName[i], VI_TRUE ,VI_FALSE, &rfsgSession[i]));
		}
	}

	for(i=0;i<(numTx*numSeg);i++)
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

	checkWarn(niWLANG_RFSGConfigureMultipleDeviceSynchronization(gSession,rfsgSession,(numTx*numSeg),clkSource,triggerLines,2));
	checkWarn(niWLANG_RFSGConfigureFrequencyMultipleLO(gSession,rfsgSession,(numTx*numSeg),NIWLANG_VAL_LO_SOURCE_ONBOARD,externalLOSession,0,carrierFrequency,1,
                                           		    NIWLANG_VAL_FALSE,NIWLANG_VAL_FALSE));
	checkWarn(niWLANG_RFSGCreateAndDownloadMIMOWaveforms(gSession,rfsgSession,NULL,numTx,"Wlan"));
	
	for (i=0; i<(numTx*numSeg); i++)
	{
		checkWarn(niWLANG_RFSGConfigureScript(rfsgSession[i],"",script, powerLevel[i]));
	}
	checkWarn(niWLANG_RFSGMultipleDeviceInitiate(gSession,rfsgSession,numTx));
	checkWarn(niWLANG_GetIQWaveformSize(gSession, "",&waveFormSize));
	checkWarn(niWLANG_GetIQRate(gSession, "", &IQRate));
	printf("Waveform Duration (s): %lf",(waveFormSize/IQRate));
	checkWarn(niWLANG_GetOFDMPacketExtensionDuration(gSession, "", &packetExtensionDuration));
	printf("Packet Extension Duration (s): %lf", packetExtensionDuration);

	for (i=0; i<numTx; i++)
	{
		sprintf(channelString,"channel%d",i);
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
