//===================================================================
//
// Title:       Generate Data packet with Dirty Tx Impairments.c
// Purpose:     Example to Generate Data packet with Dirty Tx Impairemnts
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
#include <math.h>

#include "niBTGenerationRfsg.h"

static ViSession rfsgSession = 0;
static niBTSGSession gBTSession = NULL;

int32 isNewSession = 0;

char script[1024];
char errorMessage[NIBTSG_VAL_MAX_ERROR_MESSAGE_SIZE];
char rfsgResourceName[20], refClockSource[20], waveformName[20], freqSelection[50], clkopTerminal[IVI_MAX_MESSAGE_BUF_SIZE],model[50]; 

int32 addrLT, phflow, arqn, seqn, allIQImpEn, chnNumber;
int32 bdaddrLAP, bdaddrUAP, bdaddrNAP, packetType, standard, frequencyBand;
int32 flow, addrLT, seqn, payloadLengthMode, payloadLength, dirtyTxEnabled,dirtyTxModulationIndexType;
int32 autoHeadroom, AWGNEnabled, numUniqPkts, numIdleSlots;
int32 dataType, dataOrder, dataSeed, llid, actualPaylen;
	
float64 upConverterCenterFrequency;
float64 headroom, carrierFrequencyOffset, quadratureSkew;
float64 IDCOffset, QDCOffset, IQGainImbalance, CNR;
float64 upConverterCenterFrequencyOffset, externalAttenuation;
float64 carrierFrequency, powerLevel, actualHeadroom, IQRate;

int32 payloadUserDefinedBits[]={0,0,0,0,0,0,0,0};
int32 outPort,terminalConfiguration;
float64 IOffset,QOffset,ICommonModeOffset,QCommonModeOffset;

/*--------------------------------------------------------------------------*/
/* Forward Declaration of the functions                                     */
/*--------------------------------------------------------------------------*/
int32 initGlobalVaribales(void);
int32 configureToolkitSession(void);
int32 configureRfsgSession(void);
int32 createAndDownloadWaveform(void);

int32 LETPPayloadType;
int32 LETPCorruptAlternateCRC;
int32 TXMode;
int32 TXParametersEnabledSet[]={0};
int32 TXParametersEnabledSetLength = 0;
int32 TXCarrierFrequencyOffsetSet[]={0};
int32 TXCarrierFrequencyOffsetLength = 0;
float64 TXModulationIndexSet[]={0};
int32 TXModulationIndexLength = 0;
int32 TXSymbolTimingErrorSet[]={0};
int32 TXSymbolTimingErrorLength = 0;
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
	strcpy(waveformName, "Data");

	sprintf(script, "%s", "script GenerateDATAPkt\nrepeat forever\ngenerate Data\nend repeat\nend script");

	// Packet Type
	packetType = NIBTSG_VAL_PACKET_TYPE_2DH1;

	numUniqPkts = 1;
	numIdleSlots = 1;
	
	if(packetType == NIBTSG_VAL_PACKET_TYPE_LE_TP ||packetType ==  NIBTSG_VAL_PACKET_TYPE_LE_ENHANCED ||packetType == NIBTSG_VAL_PACKET_TYPE_LE_TP_EXT || packetType == NIBTSG_VAL_PACKET_TYPE_LE_LR_500K )
		numIdleSlots = 0;

	// Data Order
	dataType = NIBTSG_VAL_PAYLOAD_DATA_TYPE_PN_SEQUENCE; 
	dataOrder = 9;
	dataSeed = 0x1F1;
	
	arqn = NIBTSG_VAL_PACKET_HEADER_ARQN_NAK;
	payloadLengthMode = NIBTSG_VAL_PAYLOAD_LENGTH_MODE_MAXIMUM_LENGTH;

	// Drity Tx Enabled
	dirtyTxEnabled = NIBTSG_VAL_TRUE;
	dirtyTxModulationIndexType = NIBTSG_VAL_DIRTY_TX_MODULATION_INDEX_TYPE_STANDARD;

	CNR = 50.00;
	autoHeadroom = NIBTSG_VAL_TRUE;
	allIQImpEn = NIBTSG_VAL_FALSE;
	AWGNEnabled = NIBTSG_VAL_FALSE;

	outPort = NIRFSG_VAL_RF_OUT;
	terminalConfiguration = NIRFSG_VAL_DIFFERENTIAL;

	//Open BT Session
	if(gBTSession == NULL)
		checkWarn(niBTSG_OpenSession("BTSG", NIBTSG_VAL_TOOLKIT_COMPATIBILITY_VERSION_020000, &gBTSession, &isNewSession));

	//Open RFSG Session
	if(rfsgSession == 0)
		checkWarn(	niRFSG_init(rfsgResourceName, VI_TRUE, VI_FALSE, &rfsgSession));

	
	LETPPayloadType = NIBTSG_VAL_LE_TP_PAYLOAD_TYPE_PRBS9;
	LETPCorruptAlternateCRC = NIBTSG_VAL_FALSE;

	if(strcmp(freqSelection,"Frequency"))
	{
		if(NIBTSG_VAL_PACKET_TYPE_LE_TP == packetType || packetType ==  NIBTSG_VAL_PACKET_TYPE_LE_ENHANCED || packetType == NIBTSG_VAL_PACKET_TYPE_LE_TP_EXT ||
		    packetType == NIBTSG_VAL_PACKET_TYPE_LE_LR_500K ||packetType == NIBTSG_VAL_PACKET_TYPE_LE_LR_125K  || packetType == NIBTSG_VAL_PACKET_TYPE_LE_LR_500K )
			      standard = NIBTSG_STANDARD_LE;
		else
			standard = NIBTSG_STANDARD_BASIC_EDR;
		checkWarn(niBTSG_ChannelNumberToCarrierFrequencyV2(chnNumber, standard, frequencyBand, &carrierFrequency));
	}
	TXMode = NIBTSG_VAL_DIRTY_TX_MODE_STANDARD;

Error:
	return error;

}

/*--------------------------------------------------------------------------*/
/* Function to configure BT Session                                        */
/*--------------------------------------------------------------------------*/
int32 configureToolkitSession(void)
{
	int32 error = 0;
	
	if( packetType == NIBTSG_VAL_PACKET_TYPE_LE_TP && LETPPayloadType == NIBTSG_VAL_LE_TP_PAYLOAD_TYPE_USER_DEFINED_BITS)
		checkWarn(niBTSG_SetPayloadUserDefinedBits(gBTSession, "", payloadUserDefinedBits, sizeof(payloadUserDefinedBits)/sizeof(int32)));

	/* Set Packet Type */
	checkWarn(niBTSG_SetCarrierMode(gBTSession, "",NIBTSG_VAL_CARRIER_MODE_BURST ));
	checkWarn(niBTSG_SetPacketType(gBTSession, "", packetType));
	
	/* Set BD Address */
	checkWarn(niBTSG_SetBDAddress(gBTSession, "", bdaddrLAP, bdaddrUAP, bdaddrNAP));

	/* Set Packet Header */
	checkWarn(niBTSG_SetPacketHeaderLTAddress(gBTSession, "", addrLT));
	checkWarn(niBTSG_SetPacketHeaderFLOW(gBTSession, "", phflow));
	checkWarn(niBTSG_SetPacketHeaderARQN(gBTSession, "", arqn));
	checkWarn(niBTSG_SetPacketHeaderSEQN(gBTSession, "", seqn));
	
	

	/* Set Packet Data */	
	checkWarn(niBTSG_SetPayloadHeaderLLID(gBTSession, "", llid));
	checkWarn(niBTSG_SetPayloadHeaderFLOW(gBTSession, "", flow));
	checkWarn(niBTSG_SetPayloadLengthMode(gBTSession, "", payloadLengthMode));

	if(payloadLengthMode)
		checkWarn(niBTSG_SetPayloadLengthMode(gBTSession, "", NIBTSG_VAL_PAYLOAD_LENGTH_MODE_USER_DEFINED));
	else 
		checkWarn(niBTSG_SetPayloadLengthMode(gBTSession, "", NIBTSG_VAL_PAYLOAD_LENGTH_MODE_MAXIMUM_LENGTH));

	if(payloadLengthMode)
		checkWarn(niBTSG_SetPayloadLength(gBTSession, "", payloadLength));
	checkWarn(niBTSG_SetPayloadDataType(gBTSession, "", dataType));
	
	/* Set Payload Data */

	if(dataType == NIBTSG_VAL_PAYLOAD_DATA_TYPE_PN_SEQUENCE)
	{
		checkWarn(niBTSG_SetPayloadPNOrder(gBTSession, "", dataOrder));
		checkWarn(niBTSG_SetPayloadPNSeed(gBTSession, "", dataSeed));
	}else
	{		
		checkWarn(niBTSG_SetPayloadUserDefinedBits(gBTSession, "", payloadUserDefinedBits, sizeof(payloadUserDefinedBits)/sizeof(int32)));
	}
	checkWarn(niBTSG_SetLE_TPPayloadType(gBTSession,"",LETPPayloadType));
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, "", NIBTSG_LE_TP_CORRUPT_ALTERNATE_CRC, LETPCorruptAlternateCRC));

	/* Set Dirty Tx Enabled */
	checkWarn(niBTSG_SetDirtyTxEnabled(gBTSession, "",dirtyTxEnabled));	
	checkWarn(niBTSG_SetDirtyTxMode(gBTSession, "", TXMode));
	checkWarn(niBTSG_SetDirtyTxParametersEnabledSet(gBTSession, "", TXParametersEnabledSet, TXParametersEnabledSetLength));
	checkWarn(niBTSG_SetDirtyTxCarrierFrequencyOffsetSet(gBTSession, "", TXCarrierFrequencyOffsetSet, TXCarrierFrequencyOffsetLength));
	checkWarn(niBTSG_SetDirtyTxModulationIndexSet(gBTSession, "", TXModulationIndexSet, TXModulationIndexLength));
	checkWarn(niBTSG_SetDirtyTxSymbolTimingErrorSet(gBTSession, "", TXSymbolTimingErrorSet, TXSymbolTimingErrorLength));

	/* Set Number of Unique Packets and Number of Idle Slots */
	checkWarn(niBTSG_SetNumberOfUniquePackets(gBTSession, "", numUniqPkts));
	checkWarn(niBTSG_SetDirtyTxModulationIndexType(gBTSession, "",dirtyTxModulationIndexType));
	checkWarn(niBTSG_SetNumberOfIdleSlots(gBTSession, "", numIdleSlots));

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

	/* Set Headroom Properties */
	checkWarn(niBTSG_SetAutoHeadroomEnabled(gBTSession, "", autoHeadroom));
	if(autoHeadroom == NIBTSG_VAL_FALSE)
		checkWarn(niBTSG_SetHeadroom(gBTSession, "", headroom));

	
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
	
	
	checkWarn(niBTSG_GetActualPayloadLength(gBTSession, "", &actualPaylen));
	printf("Actual Payload Length = %d\n", actualPaylen);

	/* Get Actual Headroom after creating the waveform */
	checkWarn(niBTSG_GetActualHeadroom(gBTSession, "", &actualHeadroom));
	printf("Actual Headroom = %lf\n", actualHeadroom);

	/* Configure Script */
	checkWarn(niBTSG_RFSGConfigureScript(rfsgSession, NULL, script, powerLevel)); 
	checkWarn(niRFSG_ConfigureOutputEnabled (rfsgSession, VI_TRUE)); 

Error:
	return error;
}


