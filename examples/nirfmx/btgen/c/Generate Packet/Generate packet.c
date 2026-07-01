//===================================================================
//
// Title:       Generate packet.c
// Purpose:     Example to Generate packet
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

char script[1024], inputPacketType[50];
char errorMessage[NIBTSG_VAL_MAX_ERROR_MESSAGE_SIZE], clkopTerminal[IVI_MAX_MESSAGE_BUF_SIZE], model[50];
char rfsgResourceName[20], refClockSource[20], waveformName[20], freqSelection[50]; 

int32 vceDataType, vceDataOrder, vceDataSeed;
int32 addrLT, phflow, arqn, seqn, allIQImpEn, chnNumber;
int32 slvbdaddrLAP, slvbdaddrUAP, slvbdaddrNAP;
int32 bdaddrLAP, bdaddrUAP, bdaddrNAP, packetType, standard, frequencyBand;
int32 flow, addrLT, seqn, payload, payloadLength, pClock;
int32 autoHeadroom, AWGNEnabled, WEnabled;
int32 dataType, dataOrder, dataSeed, llid, actualPaylen;
int32 fhsLTAddress, fhsDevClass, fhsScanRep, fhsPageScan, fhsDevClock;

float64 upConverterCenterFrequency;
float64 headroom, carrierFrequencyOffset, quadratureSkew;
float64 IDCOffset, QDCOffset, IQGainImbalance, CNR;
float64 upConverterCenterFrequencyOffset, externalAttenuation;
float64 carrierFrequency, powerLevel, actualHeadroom, IQRate;

int32 packetIndex = 0;
char *packetTypes[] = {"DATA", "DV", "FHS", "ID", "POLL", "NULL"};
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
	short packetTypeCount, invPackets = 1, loop;
	int error = 0, rfsgError;

	if(argc < 2) {
		printf("\n\tUSAGE : %s <Packet Type>\n", argv[0]);
		printf("\tPacket Type : <DATA>, <DV>, <FHS>, <ID>, <NULL>, <POLL>\n");
		return 0;
	}

	packetTypeCount = sizeof(packetTypes)/sizeof(packetTypes[0]);
	for(loop = 0; loop < packetTypeCount; loop++)
	{
		invPackets  = invPackets && strcmp(argv[1], packetTypes[loop]);
	}

	if(invPackets)
	{
		printf("\n\tInvalid Packet Type\n");
		printf("\tPacket Type : <DATA>, <DV>, <FHS>, <ID>, <NULL>, <POLL>\n");
		return 0;
	}

	strcpy(inputPacketType, argv[1]);
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
	strcpy(waveformName, inputPacketType);

	sprintf(script, "script Generate%sPkt\nrepeat forever\ngenerate %s\ngenerate idle\nend repeat\nend script", inputPacketType, inputPacketType);

	autoHeadroom = NIBTSG_VAL_TRUE;
	allIQImpEn = NIBTSG_VAL_FALSE;
	AWGNEnabled = NIBTSG_VAL_FALSE;

	if(!strcmp(inputPacketType, "DATA")) {
		// Packet Type
		packetType = NIBTSG_VAL_PACKET_TYPE_2DH1;
	}else if (!strcmp(inputPacketType, "DV")) {
		// Packet Type
		packetType = NIBTSG_VAL_PACKET_TYPE_DV;
	}else if (!strcmp(inputPacketType, "FHS")) {
		// Packet Type
		packetType = NIBTSG_VAL_PACKET_TYPE_FHS;
	}else if (!strcmp(inputPacketType, "ID")) {
		// Packet Type
		packetType = NIBTSG_VAL_PACKET_TYPE_ID;
	}else if (!strcmp(inputPacketType, "NULL")) {
		// Packet Type
		packetType = NIBTSG_VAL_PACKET_TYPE_NULL;
	}else if (!strcmp(inputPacketType, "POLL")) {
		// Packet Type
		packetType = NIBTSG_VAL_PACKET_TYPE_POLL;
	}

	if(!strcmp(inputPacketType, "DATA") || !strcmp(inputPacketType, "DV")) {
		// Data Order
		dataType = NIBTSG_VAL_PAYLOAD_DATA_TYPE_PN_SEQUENCE; 
		dataOrder = 9;
		dataSeed = 0x1F1;
	}

	if(!strcmp(inputPacketType, "DV")) {
		vceDataType = NIBTSG_VAL_PAYLOAD_DATA_TYPE_PN_SEQUENCE; 
		vceDataOrder = 9;
		vceDataSeed = 0x1F1;
	}
	
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
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_FHS_PAYLOAD_BD_ADDRESS_LAP, bdaddrLAP));
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_FHS_PAYLOAD_BD_ADDRESS_UAP, bdaddrUAP));
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_FHS_PAYLOAD_BD_ADDRESS_NAP, bdaddrNAP));

	/* Set Packet Type */
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PACKET_TYPE, packetType));

	/* Set Packet Header (For ALL Packets) */
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PACKET_HEADER_LT_ADDRESS, addrLT));
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PACKET_HEADER_FLOW, phflow));
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PACKET_HEADER_ARQN, arqn));
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PACKET_HEADER_SEQN, seqn));

	if(!strcmp(inputPacketType, "DATA") || !strcmp(inputPacketType, "DV")) {
		/* Set Packet Data */	
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PACKET_HEADER_LT_ADDRESS, llid));
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PACKET_HEADER_FLOW, flow));
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PAYLOAD_LENGTH_MODE, payload));

		if(payload)
			checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PAYLOAD_LENGTH_MODE, NIBTSG_VAL_PAYLOAD_LENGTH_MODE_USER_DEFINED));	
		else 
			checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PAYLOAD_LENGTH_MODE, NIBTSG_VAL_PAYLOAD_LENGTH_MODE_MAXIMUM_LENGTH));	

		if(payload)
			checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PAYLOAD_LENGTH, payloadLength));	

		checkWarn(niBTSG_GetScalarAttributeI32(gBTSession, NULL, NIBTSG_ACTUAL_PAYLOAD_LENGTH, &actualPaylen));
		printf("Actual Payload Length = %d\n", actualPaylen);

		/* Set Payload Data */
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PAYLOAD_DATA_TYPE, dataType));
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PAYLOAD_PN_ORDER, dataOrder));
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PAYLOAD_PN_SEED, dataSeed));
	}

	if(!strcmp(inputPacketType, "DV")) {
		/* Set Payload Voice */
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PAYLOAD_DATA_TYPE, vceDataType));
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PAYLOAD_PN_ORDER, vceDataOrder));
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PAYLOAD_PN_SEED, vceDataSeed));
	}

	if(!strcmp(inputPacketType, "FHS")) {
		/* Set Slave BD Address */
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_FHS_PAYLOAD_BD_ADDRESS_LAP, slvbdaddrLAP));
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_FHS_PAYLOAD_BD_ADDRESS_UAP, slvbdaddrUAP));
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_FHS_PAYLOAD_BD_ADDRESS_NAP, slvbdaddrNAP));

		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_FHS_PAYLOAD_LT_ADDRESS, fhsLTAddress));
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_FHS_PAYLOAD_DEVICE_CLASS, fhsDevClass));
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_FHS_PAYLOAD_SCAN_REPETITION, fhsScanRep));
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_FHS_PAYLOAD_PAGE_SCAN_MODE, fhsPageScan));
		checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_FHS_PAYLOAD_DEVICE_CLOCK, fhsDevClock));
	}

	/* Set Headroom Properties */
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_AUTO_HEADROOM_ENABLED, autoHeadroom));
	if(autoHeadroom == NIBTSG_VAL_FALSE)
		checkWarn(niBTSG_SetScalarAttributeF64(gBTSession, NULL, NIBTSG_HEADROOM, headroom));	

	/* Set Impairments */
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_ALL_IQ_IMPAIRMENTS_ENABLED, allIQImpEn));
	if(allIQImpEn == NIBTSG_VAL_TRUE) {
		checkWarn(niBTSG_SetScalarAttributeF64(gBTSession, NULL, NIBTSG_QUADRATURE_SKEW, quadratureSkew));
		checkWarn(niBTSG_SetScalarAttributeF64(gBTSession, NULL, NIBTSG_I_DC_OFFSET, IDCOffset));
		checkWarn(niBTSG_SetScalarAttributeF64(gBTSession, NULL, NIBTSG_Q_DC_OFFSET, QDCOffset));
		checkWarn(niBTSG_SetScalarAttributeF64(gBTSession, NULL, NIBTSG_IQ_GAIN_IMBALANCE, IQGainImbalance));
	}
	checkWarn(niBTSG_SetScalarAttributeF64(gBTSession, NULL, NIBTSG_CARRIER_FREQUENCY_OFFSET, carrierFrequencyOffset));
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_AWGN_ENABLED, AWGNEnabled));
	checkWarn(niBTSG_SetScalarAttributeF64(gBTSession, NULL, NIBTSG_CARRIER_TO_NOISE_RATIO, CNR));

	/* Whitening Properties */
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_WHITENING_ENABLED, WEnabled));
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_WHITENING_ENABLED, pClock));

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
	checkWarn(niBTSG_GetScalarAttributeF64(gBTSession, "", NIBTSG_ACTUAL_HEADROOM, &actualHeadroom));
	printf("Actual Headroom = %lf\n", actualHeadroom);

	/* Create and Download Idle Waveform */
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, NULL, NIBTSG_PACKET_TYPE, NIBTSG_VAL_PACKET_TYPE_IDLE));
	checkWarn(niBTSG_RFSGCreateAndDownloadWaveform(gBTSession, rfsgSession, "", "idle"));

	/* Configure Script */
	checkWarn(niBTSG_RFSGConfigureScript(rfsgSession, NULL, script, powerLevel)); 
	niRFSG_ConfigureOutputEnabled (rfsgSession, VI_TRUE); 

Error:
	return error;
}


