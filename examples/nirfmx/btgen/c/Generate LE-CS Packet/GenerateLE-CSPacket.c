//===================================================================
//
// Title:       GenerateLE-CSPacket.c
// Purpose:     Example to Generate LE-CS packet
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
int32 soundingSeqMarkerSig[] = {NIBTSG_VAL_SOUNDING_SEQUENCE_MARKER_SIGNALS_1100, NIBTSG_VAL_SOUNDING_SEQUENCE_MARKER_SIGNALS_1100};
int32 soundingSequenceMarkerPositions[] = {0, 67};

	
int32 chnNumber, autoHeadroomEnabled, standard, frequencyBand;
int32 packetType, corruptAlternativeCRC, numUniquePackets;
int32 allIQImpEn, AWGNEnabled;
int32 CSPacketFormat, CSSYNCSequence, soundingSeqLength, CSToneExtSlotEnable;
int32 soundingSeqMarkerSigLen = sizeof(soundingSeqMarkerSig) / sizeof(soundingSeqMarkerSig[0]);
int32 soundingSeqMarkerPosLen = sizeof(soundingSequenceMarkerPositions) / sizeof(soundingSequenceMarkerPositions[0]);
float64 CSPhaseMeasPeriod;

float64 upConverterCenterFrequency;
float64 headroom, carrierFrequencyOffset, quadratureSkew;
float64 IDCOffset, QDCOffset, IQGainImbalance, CNR;
float64 upConverterCenterFrequencyOffset, externalAttenuation;
float64 carrierFrequency, powerLevel, actualHeadroom;

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
	standard = NIBTSG_STANDARD_LE_CS;
	frequencyBand = NIBTSG_VAL_FREQUENCY_BAND_2p4_GHZ;
	powerLevel = 0.0;

	strcpy(rfsgResourceName, "RIO0");
	strcpy(refClockSource, "PXI_CLK");
	strcpy(clkopTerminal, "");
	strcpy(waveformName, "LECS");

	sprintf(script, "%s", "script GenerateLEPkt\nrepeat forever\ngenerate LECS\nend repeat\nend script");

	// Packet Type
	packetType = NIBTSG_VAL_PACKET_TYPE_LE_CS_1M;

	//Channel Sounding
	CSPacketFormat = NIBTSG_VAL_CS_PACKET_FORMAT_SYNC;
	CSSYNCSequence = NIBTSG_VAL_CS_SYNC_SEQUENCE_SOUNDING_SEQUENCE;
	CSPhaseMeasPeriod = 0.00001;   //Seconds
	soundingSeqLength = 32;   //Bits
	CSToneExtSlotEnable = NIBTSG_VAL_FALSE;
	
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
	
	checkWarn(niBTSG_SetScalarAttributeI32(gBTSession, "", NIBTSG_LE_TP_CORRUPT_ALTERNATE_CRC, corruptAlternativeCRC));
	checkWarn(niBTSG_SetNumberOfUniquePackets(gBTSession, "", numUniquePackets));

	 /* Set Channel Sounding Properties */
	checkWarn(niBTSG_SetCSPacketFormat(gBTSession, "", CSPacketFormat));
	checkWarn(niBTSG_SetCSSyncSequence(gBTSession, "", CSSYNCSequence));
	checkWarn(niBTSG_SetCSPhaseMeasurementPeriod(gBTSession, "", CSPhaseMeasPeriod));
	checkWarn(niBTSG_SetSoundingSequenceLength(gBTSession, "", soundingSeqLength));
   checkWarn(niBTSG_SetSoundingSequenceMarkerSignals(gBTSession, "", soundingSeqMarkerSig, soundingSeqMarkerSigLen));
   checkWarn(niBTSG_SetSoundingSequenceMarkerPositions(gBTSession, "", soundingSequenceMarkerPositions, soundingSeqMarkerPosLen));
   checkWarn(niBTSG_SetCSToneExtensionSlotEnabled(gBTSession, "", CSToneExtSlotEnable));
	
	/* Set Oversampling Factor Properties */
	checkWarn(niBTSG_SetOversamplingFactor(gBTSession, "", oversamplingFactor));
	
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
	
	/* Get Actual Headroom after creating the waveform */
	checkWarn(niBTSG_GetActualHeadroom(gBTSession, "", &actualHeadroom));
	printf("Actual Headroom = %lf\n", actualHeadroom);

	/* Configure Script */
	checkWarn(niBTSG_RFSGConfigureScript(rfsgSession, NULL, script, powerLevel)); 
	niRFSG_ConfigureOutputEnabled (rfsgSession, VI_TRUE); 

Error:
	return error;
}


