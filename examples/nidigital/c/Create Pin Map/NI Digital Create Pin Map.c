/**
*	Example Program:
*		NI Digital Create Pin Map.c
*
*	Description:
*		Demonstrates how to use the NI-Digital Pattern Driver API to create a
*		pin map.
*
*/

/* Includes */
#include <conio.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <niDigital.h>

int main(void)
{
	/* Initial Device Values */
	ViRsrc deviceID = "PXI1Slot2,PXI1Slot3";

	/* Pin Map & System Values */
	char dutPinList[] = "SCL,SDA,CS",
		dutPinListForMapping[] = "SCL,SDA,CS,SCL,SDA,CS",
		dutSite[] = "0,0,0,1,1,1",
		dutPinChannels[] = "PXI1Slot2/0,PXI1Slot2/1,PXI1Slot2/30,PXI1Slot3/0,PXI1Slot3/1,PXI1Slot3/30",
		sysPinList[] = "Vcc",
		sysPinChannels[] = "PXI1Slot2/3",
		pinGroupName[] = "I2C_bus",
		pinGroupList[] = "SCL,SDA,CS,Vcc";
	ViInt32 numSites = 2;

	/* IVI Driver Variables */
	ViSession vi = VI_NULL;
	ViStatus error = VI_SUCCESS;
	ViChar errDesc[IVI_MAX_MESSAGE_BUF_SIZE];

	/*
	* Initialize the digital pattern instrument session. The reset input of the
	* niDigital Initialize With Options VI is TRUE by default to ensure that
	* the instrument starts in a known state. All channels are in a
	* high-impedance state, and the I/O switches are open
	*/
	checkErr(niDigital_InitWithOptions(deviceID, VI_TRUE, VI_TRUE, "Simulate=1,DriverSetup=Model:6570", &vi));

	/* Create a pin map with DUT pin names and system pin names */
	checkErr(niDigital_CreatePinMap(vi, dutPinList, sysPinList));

	/* Create a channel map to add the number of sites to use */
	checkErr(niDigital_CreateChannelMap(vi, numSites));

	/* Map the system pin names in the pin map to channels but igonre site number */
	char * nextStrPtr0 = NULL;
	char * nextStrPtr1 = NULL;
	char * strPtr0 = strtok_s(sysPinList, ",", &nextStrPtr0);
	char * strPtr1 = strtok_s(sysPinChannels, ",", &nextStrPtr1);
	while ((strPtr0 != NULL) && (strPtr1 != NULL))
	{
		checkErr(niDigital_MapPinToChannel(vi, sysPinList, -1, sysPinChannels));
		strPtr0 = strtok_s(NULL, ",", &nextStrPtr0);
		strPtr1 = strtok_s(NULL, ",", &nextStrPtr1);
	}

	/*
	* Map DUT Pin names in the pin map to channels and sites. Iterate through
	* DUT pin list, site and pin name comma-delimited strings.
	*/
	nextStrPtr0 = NULL;
	nextStrPtr1 = NULL;
	char * nextStrPtr2 = NULL;
	strPtr0 = strtok_s(dutPinListForMapping, ",", &nextStrPtr0);
	strPtr1 = strtok_s(dutSite, ",", &nextStrPtr1);
	char * strPtr2 = strtok_s(dutPinChannels, ",", &nextStrPtr2);
	while ((strPtr0 != NULL) && (strPtr1 != NULL) && (strPtr2 != NULL))
	{
		checkErr(niDigital_MapPinToChannel(vi, strPtr0, atoi(strPtr1),
			strPtr2));
		strPtr0 = strtok_s(NULL, ",", &nextStrPtr0);
		strPtr1 = strtok_s(NULL, ",", &nextStrPtr1);
		strPtr2 = strtok_s(NULL, ",", &nextStrPtr2);
	}

	/* Once pin map configuration completes, end the channel map process */
	checkErr(niDigital_EndChannelMap(vi));

	/* Create pin groups from pins in the pin map */
	checkErr(niDigital_CreatePinGroup(vi, pinGroupName, pinGroupList));

	/* Load the levels and timing files */
	checkErr(niDigital_LoadLevels(vi, "PinLevels.digilevels"));
	checkErr(niDigital_LoadTiming(vi, "Timing.digitiming"));

	/* Apply the settings from the levels and timing files loaded on the instrument */
	checkErr(niDigital_ApplyLevelsAndTiming(vi, "", "PinLevels.digilevels",
		"Timing.digitiming", "", "", ""));

	/* Load the .digipat pattern file created in the Digital Pattern Editor */
	checkErr(niDigital_LoadPattern(vi, "Pattern.digipat"));

	/*
	* Burst the pattern from the start label you specify, which can be a label
	* in the .digipat pattern file or the name of the pattern.
	*/
	checkErr(niDigital_BurstPattern(vi, "", "new_pattern", VI_TRUE, VI_TRUE, 10));

	/* Disconnect selected channels using programmable onboard switching */
	nextStrPtr0 = NULL;
	strPtr0 = strtok_s(dutPinList, ",", &nextStrPtr0);
	while (strPtr0 != NULL)
	{
		checkErr(niDigital_SelectFunction(vi, strPtr0, NIDIGITAL_VAL_DISCONNECT));
		strPtr0 = strtok_s(NULL, ",", &nextStrPtr0);
	}

	nextStrPtr0 = NULL;
	strPtr0 = strtok_s(sysPinList, ",", &nextStrPtr0);
	while (strPtr0 != NULL)
	{
		checkErr(niDigital_SelectFunction(vi, strPtr0, NIDIGITAL_VAL_DISCONNECT));
		strPtr0 = strtok_s(NULL, ",", &nextStrPtr0);
	}

	/* Print Result */
	printf("Done without error.\n");

Error:

	if (error != VI_SUCCESS) /* Error Occured */
	{
		/* Get error description and print */
		niDigital_GetError(vi, &error, sizeof(errDesc) / sizeof(ViChar), errDesc);
		printf("\nError encountered\n===================\n%s\n", errDesc);
	}

	/* Close the session to the instrument */
	niDigital_close(vi);

	/* Prompt to exit (for pop-up console windows) */
	printf("\nHit <Enter> to continue...\n");
	_getch();

	return error;
}
