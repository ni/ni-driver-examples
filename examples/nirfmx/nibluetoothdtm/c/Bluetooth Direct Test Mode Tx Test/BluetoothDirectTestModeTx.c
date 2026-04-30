//===================================================================
//
// Title:		BluetoothDirectTestModeRx.c
// Purpose:      This example performs LE DUT receiver test.
//
// 
// Copyright:   NI. All Rights Reserved.
//
//===================================================================

//===================================================================
// Include files
#include <stdio.h>
#include <conio.h>
#include<stdlib.h>
#include <string.h>
#include "visa.h"
#include "ivi.h"
#include "niBluetoothDTM.h"

int main (int argc, char *argv[])
{	    
	int32 error = 0, lastErrorCode = 0;

	char VISAResourceName[50];
	niBluetoothDTMSession DTMBluetoothSession;
	uInt16 dataBits = 8;
	char errorMessage[NIBLUETOOTHDTM_VAL_MAXIMUM_ERROR_MESSAGE_LENGTH];
	uInt32 baudRate = 115200;
	int32 flowControl = VI_ASRL_FLOW_RTS_CTS;
	uInt16 stopBits = VI_ASRL_STOP_ONE;
	uInt16 parity = VI_ASRL_PAR_NONE;
	int32 status =10;
	int32 payloadLength = 37;
	int32 LEPatternType = NIBLUETOOTHDTM_VAL_LE_PATTERN_TYPE_PRBS9;
	int32 channelNumber = 3;
	int32 packetCount =0;
	strcpy(VISAResourceName, "COM1");

	checkWarn(niBluetoothDTM_OpenVISASession(VISAResourceName,&DTMBluetoothSession));
	checkWarn(niBluetoothDTM_ConfigureVISASerialSettings(DTMBluetoothSession,dataBits,baudRate,flowControl,stopBits,parity));
	checkWarn(niBluetoothDTM_SetVISATimeout(DTMBluetoothSession,2000));
	checkWarn(niBluetoothDTM_HCIReset(DTMBluetoothSession,&status));
	checkWarn(niBluetoothDTM_HCILETransmitterTest(DTMBluetoothSession,channelNumber,payloadLength,LEPatternType,&status));

	 
	checkWarn(niBluetoothDTM_HCILETestEnd(DTMBluetoothSession, &packetCount,&status));
	checkWarn(niBluetoothDTM_HCIReset(DTMBluetoothSession,&status));
	checkWarn(niBluetoothDTM_CloseVISASession(DTMBluetoothSession));
	printf("Number of packet count: %d\n",packetCount);
    Error:
	if(error)
	{  
	     niBluetoothDTM_GetErrorString(DTMBluetoothSession,error,errorMessage,NIBLUETOOTHDTM_VAL_MAXIMUM_ERROR_MESSAGE_LENGTH);
         if (error < 0)
            printf("ERROR: %s\n", errorMessage);
         else
            printf("WARNING: %s\n", errorMessage);
	}
    printf("Press any key to exit\n");
    getch();

}