//===================================================================
//
// Title:		Save Data packet Config to File.c
// Purpose:     Example to Generate Data packet
//
// 
// Copyright:   NI. All Rights Reserved.
//
//===================================================================

//===================================================================
// Include files

#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <string.h>

#include "niBTGenerationRfsg.h"

static ViSession rfsgSession = 0;
static niBTSGSession gBTSession = NULL;

int32 isNewSession = 0;

char fileName[1024];
char errorMessage[NIBTSG_VAL_MAX_ERROR_MESSAGE_SIZE];

int32 addrLT, phflow, arqn, seqn, allIQImpEn, numUniqPkts, numIdleSlots;
int32 bdaddrLAP, bdaddrUAP, bdaddrNAP, packetType, standard;
int32 flow, payloadLengthMode, payloadLength, pClock;
int32 autoHeadroomEnabled, AWGNEnabled, WEnabled, saveWaveform;
int32 dataType, dataOrder, dataSeed, llid, actualPaylen,letpPayloadType;
	
float64 upConverterCenterFrequency, actualHeadroom;
float64 headroom, carrierFrequencyOffset, quadratureSkew;
float64 IDCOffset, QDCOffset, IQGainImbalance, CNR;

int32 payloadUserDefinedBits[]={0,0,0,0,0,0,0,0};
/*--------------------------------------------------------------------------*/
/* Forward Declaration of the functions                                     */
/*--------------------------------------------------------------------------*/
int32 initGlobalVaribales(void);
int32 configureToolkitSession(void);
int32 saveDataPktConfig(void);

int main (int argc, char *argv[])
{
	int error = 0;
	int rfsgError;

	checkWarn(initGlobalVaribales());
	checkWarn(configureToolkitSession());
	checkWarn(saveDataPktConfig());

	printf("Data packet configuration is saved");

Error:
	rfsgError = error;
	
   	niBTSG_GetErrorString (gBTSession, error, errorMessage, NIBTSG_VAL_MAX_ERROR_MESSAGE_SIZE);

	if (error < 0)
		printf("ERROR: %s\n", errorMessage);
	else if (error > 0)
		printf("WARNING: %s\n", errorMessage);

	niBTSG_RFSGClearDatabase(rfsgSession, NULL, "");
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

	// Packet Type
	packetType = NIBTSG_VAL_PACKET_TYPE_DH1;

	numUniqPkts = 1;
	numIdleSlots = 1;


	// Data Order
	dataType = NIBTSG_VAL_PAYLOAD_DATA_TYPE_PN_SEQUENCE; 
	dataOrder = 9;
	dataSeed = 0x1F1;

	CNR = 50.00;

	arqn = NIBTSG_VAL_PACKET_HEADER_ARQN_NAK;
	payloadLengthMode = NIBTSG_VAL_PAYLOAD_LENGTH_MODE_MAXIMUM_LENGTH;
	letpPayloadType = NIBTSG_VAL_LE_TP_PAYLOAD_TYPE_PRBS9;
	autoHeadroomEnabled = NIBTSG_VAL_TRUE;
	allIQImpEn = NIBTSG_VAL_FALSE;
	AWGNEnabled = NIBTSG_VAL_FALSE;

	WEnabled = NIBTSG_VAL_FALSE;
	
	saveWaveform = NIBTSG_VAL_FALSE;
	//File path to save the data packet configuration.
	sprintf(fileName, "%s", "C:\\Users\\Public\\Documents\\saveConfig.tdms");

	//Open BT Session
	if(gBTSession == NULL)
		checkWarn(niBTSG_OpenSession("BTSG", NIBTSG_VAL_TOOLKIT_COMPATIBILITY_VERSION_020000, &gBTSession, &isNewSession));

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

	/* Set Packet Type */
	checkWarn(niBTSG_SetPacketType(gBTSession, "", packetType));

	/* Set Carrier Mode */
	checkWarn(niBTSG_SetCarrierMode(gBTSession, "",NIBTSG_VAL_CARRIER_MODE_BURST ));

	/* Set Number of Unique Packets and Number of Idle Slots */
	checkWarn(niBTSG_SetNumberOfUniquePackets(gBTSession, "", numUniqPkts));
	checkWarn(niBTSG_SetNumberOfIdleSlots(gBTSession, "", numIdleSlots));

	/* Set Packet Header */
	checkWarn(niBTSG_SetPacketHeaderLTAddress(gBTSession, "", addrLT));
	checkWarn(niBTSG_SetPacketHeaderFLOW(gBTSession, "", phflow));
	checkWarn(niBTSG_SetPacketHeaderARQN(gBTSession, "", arqn));
	checkWarn(niBTSG_SetPacketHeaderSEQN(gBTSession, "", seqn));

	/* Set Packet Data */	
	checkWarn(niBTSG_SetPayloadHeaderLLID(gBTSession, "", llid));
	checkWarn(niBTSG_SetPayloadHeaderFLOW(gBTSession, "", flow));
	checkWarn(niBTSG_SetPayloadLengthMode(gBTSession, "", payloadLengthMode));

	if(payloadLengthMode == NIBTSG_VAL_PAYLOAD_LENGTH_MODE_USER_DEFINED)
		checkWarn(niBTSG_SetPayloadLength(gBTSession, "", payloadLength));

	checkWarn(niBTSG_GetActualPayloadLength(gBTSession, "", &actualPaylen));
	
	/* Set Payload Data */
	checkWarn(niBTSG_SetPayloadDataType(gBTSession, "", dataType));
	checkWarn(niBTSG_SetPayloadPNOrder(gBTSession, "", dataOrder));
	checkWarn(niBTSG_SetPayloadPNSeed(gBTSession, "", dataSeed));
	checkWarn(niBTSG_SetPayloadUserDefinedBits(gBTSession, "", payloadUserDefinedBits, 8));
	checkWarn(niBTSG_SetLE_TPPayloadType(gBTSession,"",letpPayloadType));
	/* Set Headroom Properties */
	checkWarn(niBTSG_SetAutoHeadroomEnabled(gBTSession, "", autoHeadroomEnabled));
	if(autoHeadroomEnabled == NIBTSG_VAL_FALSE)
		checkWarn(niBTSG_SetHeadroom(gBTSession, "", headroom));

	/* Set Impairments */
	checkWarn(niBTSG_SetAllIQImpairmentsEnabled(gBTSession, "", allIQImpEn));
	if(allIQImpEn == NIBTSG_VAL_TRUE) {
		checkWarn(niBTSG_SetQuadratureSkew(gBTSession, "", quadratureSkew));
		checkWarn(niBTSG_SetIDCOffset(gBTSession, "", IDCOffset));
		checkWarn(niBTSG_SetQDCOffset(gBTSession, "", QDCOffset));
		checkWarn(niBTSG_SetIQGainImbalance(gBTSession, "", IQGainImbalance));
	}

	checkWarn(niBTSG_SetCarrierFrequencyOffset(gBTSession, "", carrierFrequencyOffset));
	checkWarn(niBTSG_SetAWGNEnabled(gBTSession, "", AWGNEnabled));
	checkWarn(niBTSG_SetCarrierToNoiseRatio(gBTSession, "", CNR));

	/* Whitening Properties */
	checkWarn(niBTSG_SetWhiteningEnabled(gBTSession, "", WEnabled));
	checkWarn(niBTSG_SetWhiteningClock(gBTSession, "", pClock));
	
Error :
	return error;
}


/*--------------------------------------------------------------------------*/
/* Function to save data packet configuration                               */
/*--------------------------------------------------------------------------*/
int32 saveDataPktConfig(void)
{
	int32 error = 0;

	/* Create and Download Waveform */
	checkWarn(niBTSG_SaveConfigurationToFile(gBTSession, fileName, NIBTSG_FILE_OPERATION_MODE_CREATE_OR_REPLACE)); 
	
	if(saveWaveform)
	{
		checkWarn(niBTSG_CreateAndWriteWaveformsToFile(gBTSession, fileName, NIBTSG_FILE_OPERATION_MODE_OPEN));	
	}

Error:
	return error;
}


