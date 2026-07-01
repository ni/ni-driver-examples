//===================================================================
//
// Title:       Generate ID packet.c
// Purpose:     Example to Generate ID packet
//
// 
// Copyright:   NI. All Rights Reserved.
//
//===================================================================

//===================================================================
// Include file

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <string.h>

#include "niBTGenerationRfsg.h"

static ViSession rfsgSession = 0;
static niBTSGSession gBTSession = NULL;

int32 isNewSession = 0;

char script[1024];
char errorMessage[NIBTSG_VAL_MAX_ERROR_MESSAGE_SIZE], clkopTerminal[IVI_MAX_MESSAGE_BUF_SIZE], model[50];
char rfsgResourceName[20], refClockSource[20], waveformName[20], freqSelection[50]; 

int32 addrLT, phflow, arqn, seqn, standard, frequencyBand;
int32 bdaddrLAP, bdaddrUAP, bdaddrNAP, chnNumber;
int32 autoHeadroomEnabled, AWGNEnabled, WEnabled,pClock;
	
float64 headroom,upConverterCenterFrequency;
float64 carrierFrequencyOffset, quadratureSkew;
float64 carrierFrequency, powerLevel, actualHeadroom;
float64 externalAttenuation, upConverterCenterFrequencyOffset;

int32  outPort; 
int32 terminalConfiguration;
float64 IOffset,QOffset,ICommonModeOffset,QCommonModeOffset; 

/*--------------------------------------------------------------------------*/
/* Forward Declaration of the functions                                     */
/*--------------------------------------------------------------------------*/
int32 initGlobalVaribales(void);
int32 configureToolkitSession(void);
int32 configureRfsgSession(void);
int32 createAndDownloadWaveform(void);

int main (int argc, char *argv[])
{
    int rfsgError;
	int error = 0;

	checkWarn(initGlobalVaribales());
	checkWarn(configureToolkitSession());
	checkWarn(configureRfsgSession());
	checkWarn(createAndDownloadWaveform());
	checkWarn(niRFSG_Initiate(rfsgSession));

	/*Check the status of the RFSG*/
	checkWarn(niRFSG_CheckGenerationStatus (rfsgSession, NULL));

	printf("Press any key to abort generation");
	_getch();

Error:
    rfsgError = error;
	niRFSG_GetError	(rfsgSession, &rfsgError, NIBTSG_VAL_MAX_ERROR_MESSAGE_SIZE, errorMessage);
	if(strlen(errorMessage) == 0)
   		niBTSG_GetErrorString (gBTSession, error, errorMessage, NIBTSG_VAL_MAX_ERROR_MESSAGE_SIZE);

	if (error < 0)
		printf("ERROR: %s\n", errorMessage);
	else if (error > 0)
		printf("WARNING: %s\n", errorMessage);

	niRFSG_Abort(rfsgSession);
	niRFSG_ConfigureOutputEnabled(rfsgSession, VI_FALSE);
	niRFSG_Commit(rfsgSession);
	niBTSG_RFSGClearDatabase(rfsgSession, NULL, "");
	niRFSG_ClearError(rfsgSession);
	niRFSG_close(rfsgSession);
	niBTSG_CloseSession(gBTSession);

	printf("\nPress any key to exit\n");
	_getch();
	
	return error;
}

/*--------------------------------------------------------------------------*/
/* Function to initialize the global variables                              */
/*--------------------------------------------------------------------------*/
int32 initGlobalVaribales(void)
{
	int32 error = 0;

	/* Set the parameters specific to Hardware */
	strcpy(freqSelection,"Channel Number");
	if(!strcmp(freqSelection,"Frequency"))
	{
		carrierFrequency=2.405000E+9;
	}
	else
	{
		chnNumber = 3;
	} 
	standard = NIBTSG_STANDARD_BASIC_EDR;
	frequencyBand = NIBTSG_VAL_FREQUENCY_BAND_2p4_GHZ;
	powerLevel = 0.0;

	strcpy(rfsgResourceName, "RFSG");
	strcpy(refClockSource, "PXI_CLK");
	strcpy(clkopTerminal, "");
	strcpy(waveformName, "ID");

	sprintf(script, "%s", "script GenerateIDPkt\nrepeat forever\ngenerate ID\ngenerate idle\nend repeat\nend script");

	autoHeadroomEnabled = NIBTSG_VAL_TRUE;
	
	arqn = NIBTSG_VAL_PACKET_HEADER_ARQN_NAK;
	WEnabled = NIBTSG_VAL_FALSE;
	outPort = NIRFSG_VAL_RF_OUT;
	terminalConfiguration = NIRFSG_VAL_DIFFERENTIAL;

	//Open BT Session
	if(gBTSession == NULL)
		checkWarn(niBTSG_OpenSession("BTSG", NIBTSG_VAL_TOOLKIT_COMPATIBILITY_VERSION_020000, &gBTSession, &isNewSession));

	//Open RFSG Session
	if(rfsgSession == 0)
		checkWarn(niRFSG_init(rfsgResourceName, VI_TRUE, VI_FALSE, &rfsgSession));
Error:
	return error;

}

/*--------------------------------------------------------------------------*/
/* Function to configure BT Session                                        */
/*--------------------------------------------------------------------------*/
int32 configureToolkitSession(void)
{
	int32 error = 0;

	/* Set BD Address */
	checkWarn(niBTSG_SetBDAddress(gBTSession, "", bdaddrLAP, bdaddrUAP, bdaddrNAP));	

	checkWarn(niBTSG_SetPacketType(gBTSession, "", NIBTSG_VAL_PACKET_TYPE_ID));

	/* set carrier mode */
	checkWarn(niBTSG_SetCarrierMode(gBTSession, "",NIBTSG_VAL_CARRIER_MODE_BURST ));	
	
	/* Set Packet Header */
	checkWarn(niBTSG_SetPacketHeaderLTAddress(gBTSession, "", addrLT));
	checkWarn(niBTSG_SetPacketHeaderFLOW(gBTSession, "", phflow));
	checkWarn(niBTSG_SetPacketHeaderARQN(gBTSession, "", arqn));
	checkWarn(niBTSG_SetPacketHeaderSEQN(gBTSession, "", seqn));

	/* Set Headroom Properties */
	checkWarn(niBTSG_SetAutoHeadroomEnabled(gBTSession, "", autoHeadroomEnabled));
	if(autoHeadroomEnabled == NIBTSG_VAL_FALSE)
	checkWarn(niBTSG_SetHeadroom(gBTSession, "", headroom));
	
	/* Whitening Properties */
	checkWarn(niBTSG_SetWhiteningEnabled(gBTSession, "", WEnabled));
	checkWarn(niBTSG_SetWhiteningClock(gBTSession, "", pClock));
	
	if(strcmp(freqSelection,"Frequency"))
	{
		checkWarn(niBTSG_ChannelNumberToCarrierFrequencyV2(chnNumber, standard, frequencyBand, &carrierFrequency));
	}

Error :
	return error;
}

/*--------------------------------------------------------------------------*/
/* Function to configure RFSG Session                                       */
/*--------------------------------------------------------------------------*/
int32 configureRfsgSession(void)
{
	int32 error = 0;

	checkWarn(niRFSG_ConfigureRefClock(rfsgSession, refClockSource, 10e6));
	checkWarn(niRFSG_ExportSignal(rfsgSession,NIRFSG_VAL_REF_CLOCK,"",clkopTerminal));
	
	checkWarn(niRFSG_GetAttributeViString(rfsgSession,NULL,NIRFSG_ATTR_INSTRUMENT_MODEL,50,model));
	if (!strcmp(model,"NI PXIe-5645R"))
	{
		
		if (outPort == NIRFSG_VAL_IQ_OUT)
		{
			checkWarn(niRFSG_ConfigurePowerLevelType(rfsgSession, NIRFSG_VAL_PEAK_POWER));
			checkWarn(niRFSG_ConfigureGenerationMode(rfsgSession, NIRFSG_VAL_SCRIPT));		
			checkWarn(niRFSG_SetAttributeViInt32(rfsgSession,NULL,NIRFSG_ATTR_IQ_OUT_PORT_TERMINAL_CONFIGURATION,terminalConfiguration));
			checkWarn(niRFSG_SetAttributeViReal64(rfsgSession,"I",NIRFSG_ATTR_IQ_OUT_PORT_COMMON_MODE_OFFSET,ICommonModeOffset));
			checkWarn(niRFSG_SetAttributeViReal64(rfsgSession,"I",NIRFSG_ATTR_IQ_OUT_PORT_OFFSET,IOffset));
			checkWarn(niRFSG_SetAttributeViReal64(rfsgSession,"Q",NIRFSG_ATTR_IQ_OUT_PORT_COMMON_MODE_OFFSET,QOffset)); 
			checkWarn(niRFSG_SetAttributeViReal64(rfsgSession,"Q",NIRFSG_ATTR_IQ_OUT_PORT_OFFSET,QCommonModeOffset));
			carrierFrequency = 0;					
		}
		else
		{
			checkWarn(niRFSG_SetAttributeViReal64(rfsgSession, NULL, NIRFSG_ATTR_FREQUENCY, carrierFrequency));
			checkWarn(niRFSG_ConfigurePowerLevelType(rfsgSession, NIRFSG_VAL_PEAK_POWER));
			checkWarn(niRFSG_ConfigureGenerationMode(rfsgSession, NIRFSG_VAL_SCRIPT));		
			upConverterCenterFrequencyOffset = 0;
			upConverterCenterFrequency = carrierFrequency + upConverterCenterFrequencyOffset;
			checkWarn(niRFSG_SetAttributeViReal64(rfsgSession, NULL, NIRFSG_ATTR_UPCONVERTER_CENTER_FREQUENCY, upConverterCenterFrequency));
		}
						
	}   
	else
	{
	    checkWarn(niRFSG_SetAttributeViReal64(rfsgSession, NULL, NIRFSG_ATTR_FREQUENCY, carrierFrequency));
		checkWarn(niRFSG_ConfigurePowerLevelType(rfsgSession, NIRFSG_VAL_PEAK_POWER));
		checkWarn(niRFSG_ConfigureGenerationMode(rfsgSession, NIRFSG_VAL_SCRIPT));
		upConverterCenterFrequencyOffset = 0;
	    upConverterCenterFrequency = carrierFrequency + upConverterCenterFrequencyOffset;
	    checkWarn(niRFSG_SetAttributeViReal64(rfsgSession, NULL, NIRFSG_ATTR_UPCONVERTER_CENTER_FREQUENCY, upConverterCenterFrequency));
    }
	
	checkWarn(niRFSG_SetAttributeViReal64(rfsgSession, NULL, NIRFSG_ATTR_EXTERNAL_GAIN, -externalAttenuation));

Error:
	return error;
}

/*--------------------------------------------------------------------------*/
/* Function to create and download waveform                                 */
/*--------------------------------------------------------------------------*/
int32 createAndDownloadWaveform(void)
{
	int32 error = 0;

	/* Create and Download Waveform */
	checkWarn(niBTSG_RFSGCreateAndDownloadWaveform(gBTSession, rfsgSession, "", waveformName));

	/* Get Actual Headroom after creating the waveform */
	checkWarn(niBTSG_GetActualHeadroom(gBTSession, "", &actualHeadroom));
	printf("Actual Headroom = %lf\n", actualHeadroom);

	/* Create and Download Idle Waveform */
	checkWarn(niBTSG_SetPacketType(gBTSession, "", NIBTSG_VAL_PACKET_TYPE_IDLE));
	checkWarn(niBTSG_RFSGCreateAndDownloadWaveform(gBTSession, rfsgSession, "", "idle"));

	/* Configure Script */
	checkWarn(niBTSG_RFSGConfigureScript(rfsgSession, NULL, script, powerLevel)); 
	niRFSG_ConfigureOutputEnabled (rfsgSession, VI_TRUE); 

Error:
	return error;
}


