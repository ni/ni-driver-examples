/**
*	Example Program:
*		NI Digital Multi-Instrument Match Fail Combination.c
*
*	Description:
*		Demonstrates how to use the NI-Digital Pattern Driver API to burst a
*		pattern on multiple synchronized instruments and detect a matched or
*		failed condition across instruments.
*
*		Ensure that the instrument names in the PinMap.pinmap file matches the
*		resource names of the digital pattern instruments and NI-Sync Instrument
*		in the variables digiDevIDs and syncDevID.
*
*		Note: Also ensure C Language Support is installed when installing the
*		NI-Sync driver.
*
*/

/* Includes */
#include <conio.h>
#include <stdio.h>
#include <stdlib.h>
#include <niDigital.h>
#include <niSync.h>

int main(void)
{
	/* Initial Device Values */
	ViRsrc digiDevID = "PXI1Slot2,PXI1Slot3";   /* Digital Pattern Instruments */
	ViRsrc syncDevID = "PXI1Slot10";	/* 6674T NI-Sync Instrument */

	/* IVI Driver Variables */
	ViSession niSyncSession = VI_NULL;
	ViSession niDigitalSession = VI_NULL;
	ViStatus error = VI_SUCCESS;
	ViChar errDesc[IVI_MAX_MESSAGE_BUF_SIZE];

	/* Initialize the NI-Sync instrument session */
	checkErr(niSync_init(syncDevID, VI_TRUE, VI_FALSE, &niSyncSession));

	/*
	* Initialize the digital pattern instrument session. The reset input of the
	* niDigital Initialize With Options VI is TRUE by default to ensure that
	* the instrument starts in a known state. All channels are in a
	* high-impedance state, and the I/O switches are open
	*/
	checkErr(niDigital_InitWithOptions(digiDevID, VI_TRUE, VI_TRUE, "Simulate=1,DriverSetup=Model:6570", &niDigitalSession));

	/*
	* Load the pin map for the instrument at the beginning of the program to
	* allow referencing of pin names that the pin map defines in the channel
	* list of select driver functions
	*/
	checkErr(niDigital_LoadPinMap(niDigitalSession, "PinMap.pinmap"));

	/*
	* Load the specifications, levels, and timing files created in the Digital
	* Pattern Editor on the instrument. These settings are not applied until
	* you call the niDigital_ApplyLevelsandTiming() function but in this
	* example we call it immediately after
	*/
	checkErr(niDigital_LoadSpecifications(niDigitalSession, "Specifications.specs"));
	checkErr(niDigital_LoadLevels(niDigitalSession, "PinLevels.digilevels"));
	checkErr(niDigital_LoadTiming(niDigitalSession, "Timing.digitiming"));
	checkErr(niDigital_ApplyLevelsAndTiming(niDigitalSession, "",
		"PinLevels.digilevels", "Timing.digitiming", "", "", ""));

	/* Load the .digipat pattern file created in the Digital Pattern Editor */
	checkErr(niDigital_LoadPattern(niDigitalSession, "Pattern.digipat"));

	/*
	* Configure digital pattern instruments and the PXIe-6674T timing and sync
	* instrument to combine pattern combination results and control subsequent
	* pattern execution across digital pattern instruments based on the results
	*/

	const ViUInt32 sessionCount = 1;
	checkErr(niDigital_EnableMatchFailCombination(sessionCount, &niDigitalSession,
		niSyncSession));

	/*
	* Burst the pattern from the start label you specify, which can be a label
	* in the .digipat pattern file or the name of the pattern
	*/
	checkErr(niDigital_BurstPattern(niDigitalSession,
		"", "new_pattern", VI_TRUE, VI_TRUE, 10));

	ViBoolean result = VI_FALSE; /* VI_TRUE means there was an error detected */

	/* Read the sequencer flag to determine if the pattern matched across all instruments */
	ViBoolean temp = VI_FALSE;
	checkErr(niDigital_ReadSequencerFlag(niDigitalSession,
		NIDIGITAL_VAL_SEQUENCER_FLAG0, &temp));
	result |= temp;

	/* Disconnect all channels using programmable onboard switching */
	checkErr(niDigital_SelectFunction(niDigitalSession, "",
		NIDIGITAL_VAL_DISCONNECT));

	/* Close the session to each instrument */
	checkErr(niDigital_close(niDigitalSession));

	/* Close NI-Sync session */
	checkErr(niSync_close(niSyncSession));

	/* Print Result */
	printf("Pattern match %s\n", (result == VI_TRUE) ? "failed" : "passed");
	printf("Done without error.\n");

Error:

	if (error != VI_SUCCESS) /* Error Occured */
	{
		/* Get error description and print */
		niDigital_GetError(niSyncSession, &error, sizeof(errDesc) / sizeof(ViChar),
			errDesc);
		printf("\nError encountered\n===================\n%s\n", errDesc);
	}

	/* Prompt to exit (for pop-up console windows) */
	printf("\nHit <Enter> to continue...\n");
	_getch();

	return error;
}
