/**
*	Example Program:
*		NI Digital Burst with Start Trigger.c
*
*	Description:
*		Demonstrates how to use the NI-Digital Pattern Driver API to create and
*		configure an instrument session and to burst a pattern on the digital
*		pattern instrument using a start trigger.
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
	ViConstString trigSource = "/PXI1Slot2/PXI_TRIG0";
	ViInt32 digiEdge = NIDIGITAL_VAL_RISING_EDGE;

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
	* Load the specifications, levels, and timing files created in the Digital
	* Pattern Editor on the instrument. These settings are not applied until
	* you call the niDigital_ApplyLevelsandTiming() function but in this
	* example we call it immediately after
	*/
	checkErr(niDigital_LoadSpecifications(vi, "Specifications.specs"));
	checkErr(niDigital_LoadLevels(vi, "PinLevels.digilevels"));
	checkErr(niDigital_LoadTiming(vi, "Timing.digitiming"));
	checkErr(niDigital_ApplyLevelsAndTiming(vi, "", "PinLevels.digilevels",
		"Timing.digitiming", "", "", ""));

	/* Load the .digipat pattern file created in the Digital Pattern Editor */
	checkErr(niDigital_LoadPattern(vi, "Pattern.digipat"));

	char chSelect = 0;
	while ((chSelect != 'n') && (chSelect != 'd') && (chSelect != 's'))
	{
		printf("\nSelect Trigger Mode [None (n), Digital (d), Software (s)]:\n");
		chSelect = _getch();
		switch (chSelect)
		{
			/*
			* Select None (n) to burst the pattern from the start label you specify.
			* The wait until done? input is TRUE by default to ensure that the
			* pattern burst completes before moving to the clean up tasks
			*/
		case 'n':
			checkErr(niDigital_BurstPattern(vi, "", "new_pattern", VI_TRUE, VI_TRUE, 10));
			break;

			/*
			* Select Start Digital Edge to specify a source and edge for the start
			* trigger. In this example, the trigger source is specified by
			* trigSource to be coming fromt the PXI_TRIG0 trigger line
			*/
		case 'd':
			checkErr(niDigital_ConfigureDigitalEdgeStartTrigger(vi, trigSource, digiEdge));
			checkErr(niDigital_BurstPattern(vi, "", "new_pattern", VI_TRUE, VI_TRUE, 10));
			break;

			/*
			* Select Start Software Edge to make a pattern wait until it receives a
			* software start trigger you send via host code
			*/
		case 's':
			checkErr(niDigital_ConfigureSoftwareEdgeStartTrigger(vi));
			/* Set waitUntilDone to false so this function does not block execution */
			checkErr(niDigital_BurstPattern(vi, "", "new_pattern", VI_TRUE, VI_FALSE, 10));
			printf("\nDevice armed for software trigger. Hit any key to send software trigger...");
			_getch(); /* _getch() will block until key stroke made, then send SW trigger */
			checkErr(niDigital_SendSoftwareEdgeTrigger(vi, NIDIGITAL_VAL_START_TRIGGER, ""));
			checkErr(niDigital_WaitUntilDone(vi, 10));
			break;

		default:
			printf("\nPlease enter either 'n', 'd' or 's' character to specify trigger mode\n");
			break;
		}
	}

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
