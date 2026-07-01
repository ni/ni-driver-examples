/****************************************************************************
 *          National Instruments WLAN Signal Generator
 *---------------------------------------------------------------------------
 *   Copyright (c) National Instruments 2017.  All Rights Reserved.
 *---------------------------------------------------------------------------
 *
 * Title:    80211MIMOOFDMGenerateWaveform(IQPorts).c
 * Purpose:  This example demonstrates generation of multiple-channel 802.11n, 802.11ac, 802.11ah or 802.11af
 *           waveform using multiple NI vector signal generators.
 *
Comments:
1. For each generator,
     A. Open an NI-RFSG session.
     B. Configure basic NI-RFSG properties. Set Generation Mode to Script and Power Level Type to Peak Power.
     C. Configure IQ port specific properties. Set Output Port to IQ Out. Configure Port Level and Carrier Frequency.
2. Open an NI WLAN Generation session.
3. Configure basic WLAN generation properties.
   Configure 80211n PLCP Frame Format, Number of Space Time Streams and 80211ah Preamble Type 
4. Synchronize the generators. Configure the reference clock source for the master (the first generator) and share that with slaves.
5. Create the waveform and download it to the NI RF vector signal generators memory.
6.  For each generator, configure NI-RFSG for the waveform mentioned in the script.
7. Initiate signal generation by calling niWLANG RFSG Multiple Device Initiate Device.
8. 
    A. Check the generation status.
    B. Exit if an error has occurred, the Stop button is pressed or generation is complete.
9. For each generator,
    A. Abort signal generation.
    B. Disable the output. This sets the noise floor as low as possible.
    C. Commit settings to hardware.
    D. Clear the waveform from the device memory. Clear the waveform properties from RFSG database.
    E. Close the NI-RFSG session.
10. Read signal properties (waveform size and actual headroom) for display purpose.
11. Close the NI WLAN Generation session.

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
static int32 standard,mcsIndex,numTx,mappingMatrixType,frameFormat,noOfSpaceTimeStreams;
static float64 channelBandwidth;
static float64 carrierFrequency;
static int32 preambleType80211ah;
static int32 terminalConfiguration;
static float64 iqPortCarrierFrequency;
static int32 externalAttenuation[] = {0,0,0,0};
static float64 iqOutPortLevels[] = {0.5,0.5,0.5,0.5};

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
	terminalConfiguration = NIRFSG_VAL_DIFFERENTIAL;
	iqPortCarrierFrequency = 0;

	return 0;
}

/*--------------------------------------------------------------------------*/
/* Function to configure RFSG Session                                       */
/*--------------------------------------------------------------------------*/
int32 configureToolkitSession(void)
{
	int32 error = 0;

	//open WLAN Session
	if(!gSession)
		checkWarn(niWLANG_OpenSession("WLANG", NIWLANG_VAL_COMPATIBILITY_VERSION_060000, &gSession, &isNewSession));

	/*Set Properties to the WLAN Generation Session*/
	checkWarn(niWLANG_SetStandard(gSession, NULL, standard));	/*802.11n standard*/
	checkWarn(niWLANG_SetNumberOfTransmitChannels(gSession, NULL, numTx));
	checkWarn(niWLANG_SetChannelBandwidth(gSession, NULL, channelBandwidth));
	checkWarn(niWLANG_SetMCSIndex(gSession, NULL, mcsIndex));
	checkWarn(niWLANG_SetMappingMatrixType(gSession, NULL, mappingMatrixType));

	if(standard == NIWLANG_VAL_STANDARD_80211N_MIMO_OFDM)
		checkWarn(niWLANG_Set80211nPLCPFrameFormat(gSession, NULL, frameFormat));
	else if (standard == NIWLANG_VAL_STANDARD_80211AH_MIMO_OFDM)
	{
		checkWarn(niWLANG_SetNumberOfSpaceTimeStreams(gSession, NULL, noOfSpaceTimeStreams));
		checkWarn(niWLANG_Set80211ahPreambleType(gSession, NULL, preambleType80211ah));
		
	}
	else
		checkWarn(niWLANG_SetNumberOfSpaceTimeStreams(gSession, NULL, noOfSpaceTimeStreams));

	
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
		checkWarn(niRFSG_ConfigureGenerationMode(rfsgSession[i], NIRFSG_VAL_ARB_WAVEFORM));	
		checkWarn(niRFSG_SetAttributeViReal64(rfsgSession[i], NULL, NIRFSG_ATTR_EXTERNAL_GAIN, -(externalAttenuation[i]) ));
		checkWarn(niRFSG_SetAttributeViInt32(rfsgSession[i], NULL, NIRFSG_ATTR_OUTPUT_PORT, NIRFSG_VAL_IQ_OUT));
		checkWarn(niRFSG_SetAttributeViReal64(rfsgSession[i], NULL,NIRFSG_ATTR_IQ_OUT_PORT_LEVEL, iqOutPortLevels[i]));
		checkWarn(niRFSG_SetAttributeViInt32(rfsgSession[i], NULL, NIRFSG_ATTR_IQ_OUT_PORT_TERMINAL_CONFIGURATION, terminalConfiguration));
		checkWarn(niRFSG_SetAttributeViReal64(rfsgSession[i], NULL,NIRFSG_ATTR_IQ_OUT_PORT_CARRIER_FREQUENCY, iqPortCarrierFrequency)); 
	
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
	checkWarn(niWLANG_RFSGCreateAndDownloadMIMOWaveforms(gSession,rfsgSession,NULL,numTx,"Wlan"));	
	/*Configure Script*/
	for (i=0; i<numTx; i++)
	{
		checkWarn(niWLANG_RFSGConfigureScript(rfsgSession[i],"",script, -10));
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
	}while(!isDone &&  !_kbhit());
	
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
