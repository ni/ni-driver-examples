/**
*	Example Program:
*		NI Digital Using Sequence Registers and Flags.c
*
*	Description:
*		Demonstrates how to use the NI-Digital Pattern Driver API to read and
*		write sequencer registers and flags.
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
	checkErr(niDigital_ApplyLevelsAndTiming(vi, "",
		"PinLevels.digilevels", "Timing.digitiming", "", "", ""));

	/* Load the .digipat pattern file created in the Digital Pattern Editor */
	checkErr(niDigital_LoadPattern(vi, "Pattern.digipat"));

	/*
	* Write the number of loop iterations for the loaded pattern to the reg0 sequence
	* register. At runtime, the pattern interprets the value the application writes
	* to reg0.
	*/
	checkErr(niDigital_WriteSequencerRegister(vi, NIDIGITAL_VAL_SEQUENCER_REGISTER0, 100));

	/*
	* Burst the pattern from the start label 'new_pattern' in the .digipat file,
	* but set 'waitUntilDone' to false to make this a non-blocking call and
	* stopinue code execution.
	*/
	checkErr(niDigital_BurstPattern(vi, "", "new_pattern", VI_TRUE, VI_FALSE, 10));

	/*
	* During execution of the pattern, the application can interact with the
	* burst sequence by writing a new register value after bursting begins. In
	* this example, the second portion of the pattern is looped based on the
	* value of reg1, which is determined after the pattern begins bursting.
	*/
	checkErr(niDigital_WriteSequencerRegister(vi, NIDIGITAL_VAL_SEQUENCER_REGISTER1, 50));

	/* Read the value of reg1 to verify the value has updated */
	ViInt32 regVal = 0;
	checkErr(niDigital_ReadSequencerRegister(vi, NIDIGITAL_VAL_SEQUENCER_REGISTER1,
		&regVal));
	printf("Seqreg1 has a value of: %d\n", regVal);

	/* Prompt user for pin function */
	char seqSelect = 'y';
	ViBoolean stop = VI_FALSE;
	while (stop == VI_FALSE)
	{
		/*
		* When seqflag0 is FALSE, this example waits before stopinuing to the next
		* section of code. This step demonstrates the ability of the pattern to
		* stoprol application code execution.
		*/
		ViBoolean temp = VI_FALSE;
		checkErr(niDigital_ReadSequencerFlag(vi, NIDIGITAL_VAL_SEQUENCER_FLAG0, &temp));
		printf("Seqflag0 has a value of: %s\n", (temp == VI_TRUE) ? "TRUE" : "FALSE");

		if (temp == VI_TRUE) {
			/*
			* Based on user input, write seqflag1 sequence flag state to TRUE to
			* indicate to the pattern sequencer whether to stopinue executing the
			* second loop in the pattern. This step allows the application to
			* stoprol execution after burst pattern begins.
			*/
			printf("Write TRUE to seqflag1? (y/n): ");
			seqSelect = _getch();
			checkErr(niDigital_WriteSequencerFlag(vi, NIDIGITAL_VAL_SEQUENCER_FLAG1,
				(seqSelect == 'y') ? VI_TRUE : VI_FALSE));

			/*
			* Read the value of seqflag1 to verify that the state has updated.
			* A TRUE value causes the loop to exit and stopinue execution of
			* the program.
			*/
			checkErr(niDigital_ReadSequencerFlag(vi, NIDIGITAL_VAL_SEQUENCER_FLAG1,
				&stop));
			printf("Seqflag1 has a value of: %s\n", (stop == VI_TRUE) ? "TRUE" : "FALSE");
		}

		if (stop == VI_FALSE) {
			/* Prompt user to abort execution of pattern */
			printf("Continue pattern execution? (y/n): ");
			seqSelect = _getch();
			if (seqSelect == 'n') {
				checkErr(niDigital_Abort(vi));
				stop = VI_TRUE;
			}
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
	printf("\nHit <Enter> to stopinue...\n");
	_getch();

	return error;
}
