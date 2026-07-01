//===================================================================
//
// Title:       GenerateLE-TPPacket.c
// Purpose:     Example to Generate LE-TP packet
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
char errorMessage[NIBTSG_VAL_MAX_ERROR_MESSAGE_SIZE];
char rfsgResourceName[20], refClockSource[20], waveformName[20], freqSelection[50], clkopTerminal[IVI_MAX_MESSAGE_BUF_SIZE],model[50]; 
int32 userDefinedBits[]={0,0,0};
float64 relativeAmplitudeDB[] = {0};
float64 relativePhaseDeg[] = {0};
int32 relativeAmplitudeAndPhaseArraySize = 0;
char antennaSwitchingPattern[]="A0";
float64 antennaSwitchingDuration;
	
int32 chnNumber, autoHeadroomEnabled, standard, frequencyBand, actualPaylen ;
int32 payloadType, payloadLengthMode, payloadLength, packetType, dirtyTxEnbld, corruptAlternativeCRC, numUniquePackets;
int32 allIQImpEn, AWGNEnabled, directionFindingMode, numberOfAntennas, antennaSwitchingEnabled ;
float64 cteLength, cteSlotDuration;

float64 upConverterCenterFrequency;
float64 headroom, carrierFrequencyOffset, quadratureSkew;
float64 IDCOffset, QDCOffset, IQGainImbalance, CNR;
float64 upConverterCenterFrequencyOffset, externalAttenuation;
float64 carrierFrequency, powerLevel, actualHeadroom, antennaSwitchingDurationUsed;

int32  outPort, oversamplingFactor; 
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
	numUniquePackets = 1;
	standard = NIBTSG_STANDARD_LE;
	frequencyBand = NIBTSG_VAL_FREQUENCY_BAND_2p4_GHZ;
	powerLevel = 0.0;

	strcpy(rfsgResourceName, "RIO0");
	strcpy(refClockSource, "PXI_CLK");
	strcpy(clkopTerminal, "");
	strcpy(waveformName, "LETP");

	sprintf(script, "%s", "script GenerateLEPkt\nrepeat forever\ngenerate LETP\nend repeat\nend script");

	// Packet Type
	packetType = NIBTSG_VAL_PACKET_TYPE_LE_TP;

	//Payload Data
	payloadType = NIBTSG_VAL_LE_TP_PAYLOAD_TYPE_PRBS9;
	payloadLengthMode = NIBTSG_VAL_PAYLOAD_LENGTH_MODE_MAXIMUM_LENGTH;
	payloadLength = 0;

	dirtyTxEnbld = NIBTSG_VAL_FALSE;

	//Direction Finding
	antennaSwitchingEnabled = NIBTSG_VAL_FALSE;
	directionFindingMode = NIBTSG_VAL_DIRECTION_FINDING_MODE_DISABLED;
	cteLength = 0.00016;   //Seconds
	cteSlotDuration = 0.000001;   //Seconds
	numberOfAntennas = 1;
	antennaSwitchingDuration = 0.0000005;    //Seconds
	
	oversamplingFactor = 8;
	
	autoHeadroomEnabled = NIBTSG_VAL_TRUE;
	allIQImpEn = NIBTSG_VAL_FALSE;
	AWGNEnabled = NIBTSG_VAL_FALSE;
	CNR = 50.0;
	corruptAlternativeCRC = NIBTSG_VAL_FALSE;

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

	/* Set Packet Type */
	checkWarn(niBTSG_SetPacketType(gBTSession, "", packetType));
	
	/* Set Carrier mode */
	checkWarn(niBTSG_SetCarrierMode(gBTSession, "",NIBTSG_VAL_CARRIER_MODE_BURST ));

	checkWarn(niBTSG_SetDirtyTxEnabled(gBTSession, "", dirtyTxEnbld));
	
	/* Set LE-TP Payload*/
	checkWarn(niBTSG_SetLE_TPPayloadType(gBTSession, NULL, payloadType)); 
	if(payloadType == NIBTSG_VAL_LE_TP_PAYLOAD_TYPE_USER_DEFINED_BITS)
	{
		checkWarn(niBTSG_SetPayloadUserDefinedBits(gBTSession, "", userDefinedBits, (sizeof(userDefinedBits)/sizeof(int32))));
	}
	
	/* Set Payload Header */	
	checkWarn(niBTSG_SetPayloadLengthMode(gBTSession, "", payloadLengthMode));

	if(payloadLengthMode)
		checkWarn(niBTSG_SetPayloadLengthMode(gBTSession, "", NIBTSG_VAL_PAYLOAD_LENGTH_MODE_USER_DEFINED));
	else 
		checkWarn(niBTSG_SetPayloadLengthMode(gBTSession, "", NIBTSG_VAL_PAYLOAD_LENGTH_MODE_MAXIMUM_LENGTH));

	if(payloadLengthMode)
		checkWarn(niBTSG_SetPayloadLength(gBTSession, "", payloadLength));

	checkWarn(niBTSG_GetActualPayloadLength(gBTSession, "", &actualPaylen));
	printf("Actual Payload Length = %d\n", actualPaylen);

	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, "", NIBTSG_LE_TP_CORRUPT_ALTERNATE_CRC, corruptAlternativeCRC));
	checkWarn(niBTSG_SetNumberOfUniquePackets(gBTSession, "", numUniquePackets));

	 /* Set Direction Finding Properties */
	checkWarn(niBTSG_SetDirectionFindingMode(gBTSession, "", directionFindingMode));
	checkWarn(niBTSG_SetDirectionFindingConstantToneExtensionLength(gBTSession, "", cteLength));
	checkWarn(niBTSG_SetDirectionFindingConstantToneExtensionSlotDuration(gBTSession, "", cteSlotDuration));
	checkWarn(niBTSG_SetDirectionFindingAntennaSwitchingEnabled(gBTSession, "", antennaSwitchingEnabled));
	checkWarn(niBTSG_SetDirectionFindingNumberOfAntennas(gBTSession, "", numberOfAntennas));
	checkWarn(niBTSG_SetDirectionFindingAntennaSwitchingPattern(gBTSession, "", antennaSwitchingPattern));
	checkWarn(niBTSG_SetDirectionFindingAntennaSwitchingDuration(gBTSession, "", antennaSwitchingDuration));
	
	/* Set Oversampling Factor Properties */
	checkWarn(niBTSG_SetOversamplingFactor(gBTSession, "", oversamplingFactor));

   /* Set Antenna Relative Phase and Amplitude */
   checkWarn(niBTSG_SetAntennaRelativePhaseAndAmplitude(gBTSession, "", relativeAmplitudeDB, relativePhaseDeg, relativeAmplitudeAndPhaseArraySize));

	/* Set Headroom Properties */
	
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
	
	checkWarn(niBTSG_GetDirectionFindingAntennaSwitchingDurationUsed(gBTSession, "", &antennaSwitchingDurationUsed));
	printf("Antenna Switching Duration Used = %1.10f\n", antennaSwitchingDurationUsed);
	
	/* Get Actual Headroom after creating the waveform */
	checkWarn(niBTSG_GetActualHeadroom(gBTSession, "", &actualHeadroom));
	printf("Actual Headroom = %lf\n", actualHeadroom);

	/* Configure Script */
	checkWarn(niBTSG_RFSGConfigureScript(rfsgSession, NULL, script, powerLevel)); 
	niRFSG_ConfigureOutputEnabled (rfsgSession, VI_TRUE); 

Error:
	return error;
}


