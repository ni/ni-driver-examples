/**
*	Example Program:
*		NI Digital Create Time Sets.c
*
*	Description:
*		Demonstrates how to use the NI-Digital Pattern Driver API to configure
*		the time set for a pattern.
*
*/

/* Includes */
#include <conio.h>
#include <stdio.h>
#include <stdlib.h>
#include <niDigital.h>

int main(void)
{
	/* Initial Device Values */
	ViRsrc deviceID = "PXI1Slot2,PXI1Slot3";
	ViConstString channelList = "PinGroup1";

	/* Time Set Values */
	ViConstString tsetName = "tset0";
	ViInt32 driveFormat = NIDIGITAL_VAL_NR;
	ViReal64 period = 20.0e-9;
	ViReal64 driveDataEdge = 0, driveOnEdge = 0, driveReturnEdge = 15.0e-9,
		driveOffEdge = 20.0e-9;
	ViReal64 strobeEdge = 10.0e-9;

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

	/*
	* Load the pin map for the instrument at the beginning of the program to
	* allow referencing of pin names that the pin map defines in the channel
	* list of select driver functions
	*/
	checkErr(niDigital_LoadPinMap(vi, "PinMap.pinmap"));

	/*
	* Load the levels file you created in the Digital Pattern Editor on the
	* instrument. We do not load a specifications or timing file because this
	* example configures the time set.
	*/
	checkErr(niDigital_LoadLevels(vi, "PinLevels.digilevels"));
	checkErr(niDigital_ApplyLevelsAndTiming(vi, "", "PinLevels.digilevels", "",
		"", "", ""));

	/* Load the .digipat pattern file created in the Digital Pattern Editor */
	checkErr(niDigital_LoadPattern(vi, "Pattern.digipat"));

	/* Create a time set with the specified name */
	checkErr(niDigital_CreateTimeSet(vi, tsetName));

	/* Configure period for the time set */
	checkErr(niDigital_ConfigureTimeSetPeriod(vi, tsetName, period));

	/* Configure the drive edges for the time set and channel(s) */
	checkErr(niDigital_ConfigureTimeSetDriveEdges(vi, channelList, tsetName, driveFormat,
		driveOnEdge, driveDataEdge, driveReturnEdge, driveOffEdge));

	/* Configure compare (strobe) edges for time set */
	checkErr(niDigital_ConfigureTimeSetCompareEdgesStrobe(vi, channelList, tsetName,
		strobeEdge));

	/* Burst the pattern from the start label (specified here as new_pattern) */
	checkErr(niDigital_BurstPattern(vi, "", "new_pattern", VI_TRUE, VI_TRUE, 10));

	/* Disconnect all channels using programmable onboard switching */
	checkErr(niDigital_SelectFunction(vi, "", NIDIGITAL_VAL_DISCONNECT));

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
