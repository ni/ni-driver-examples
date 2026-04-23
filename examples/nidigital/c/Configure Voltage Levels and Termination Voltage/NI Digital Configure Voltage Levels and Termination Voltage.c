/**
*	Example Program:
*		NI Digital Configure Voltage Levels and Termination Voltage.c
*
*	Description:
*		Demonstrates how to use the NI-Digital Pattern Driver API to create an
*		instrument session, configure various voltage levels, and then burst a
*		pattern. The vol, voh, vih, vil values are set at run-time and the
*		user can select from High-Z, Active Load and Three-Level Drive
*		termination options.
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
	/* Voltage & Termination Variables */
	ViReal64 vil = 0, vih = 0, vol = 0, voh = 0, vterm = 0, vcom = 0, iol = 0,
		ioh = 0;

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
	* Load the specifications, and timing files created in the Digital
	* Pattern Editor on the instrument. These settings are not applied until
	* you call the niDigital_ApplyLevelsandTiming() function but in this
	* example we call it immediately after
	*/
	checkErr(niDigital_LoadSpecifications(vi, "Specifications.specs"));
	checkErr(niDigital_LoadTiming(vi, "Timing.digitiming"));
	/* Since levels are applied by user, we will not apply a level sheet */
	checkErr(niDigital_ApplyLevelsAndTiming(vi, "", "",
		"Timing.digitiming", "", "", ""));

	/* Load the .digipat pattern file created in the Digital Pattern Editor */
	checkErr(niDigital_LoadPattern(vi, "Pattern.digipat"));

	/* Prompt user for voltage levels */
	printf("Enter the voltage the instrument will apply to the DUT input pin "
		"when driving a logic low (0): ");
	scanf_s("%lf", &vil);
	printf("Enter the voltage the instrument will apply to the DUT input pin "
		"when driving a logic high (1): ");
	scanf_s("%lf", &vih);
	printf("Enter the voltage below which the instrument pin comparator will "
		"interpret a logic low (L): ");
	scanf_s("%lf", &vol);
	printf("Enter the voltage above which the instrument pin comparator will "
		"interpret a logic high (H): ");
	scanf_s("%lf", &voh);
	printf("Enter the termination voltage the instrument applies during "
		"non-drive cycles when the termination mode is set to Vterm: ");
	scanf_s("%lf", &vterm);

	/* Apply given voltage levels */
	checkErr(niDigital_ConfigureVoltageLevels(vi, channelList, vil, vih, vol, voh, vterm));

	/* Prompt user for termination mode */
	char termSelect = 0;
	while ((termSelect != 'h') && (termSelect != 'a') && (termSelect != 't'))
	{
		printf("\nSelect device termination mode [High-Z (h), Active Load (a),"
			" Three-Level Drive (t)]: ");
		termSelect = _getch();
		switch (termSelect)
		{
		case 'h': /* High-Z */
			checkErr(niDigital_ConfigureTerminationMode(vi, channelList,
				NIDIGITAL_VAL_HIGH_Z));
			break;

		case 'a': /* Active Load */
			checkErr(niDigital_ConfigureTerminationMode(vi, channelList,
				NIDIGITAL_VAL_ACTIVE_LOAD));
			printf("Enter the maximum current (A) the DUT sinks while outputting"
				" a voltage below VCOM: ");
			scanf_s("%lf", &iol);
			printf("Enter the maximum current (A) the DUT sources while"
				" outputting a voltage above VCOM: ");
			scanf_s("%lf", &ioh);
			printf("Enter the commutating voltage level at which the active "
				"load circuit switches between sourcing current and sinking "
				"current: ");
			scanf_s("%lf", &vcom);
			checkErr(niDigital_ConfigureActiveLoadLevels(vi, channelList, iol, ioh,
				vcom));
			break;

		case 't':
			checkErr(niDigital_ConfigureTerminationMode(vi, channelList,
				NIDIGITAL_VAL_VTERM));
			break;

		default:
			printf("\nPlease enter either 'h', 'a' or 't' character to specify"
				" capture type\n");
			break;
		}
	}

	/* Burst the pattern from the start label (specified here as new_pattern) */
	checkErr(niDigital_BurstPattern(vi, "", "new_pattern", VI_TRUE, VI_TRUE, 10));

	/* Disconnect all channels using programmable onboard switching */
	checkErr(niDigital_SelectFunction(vi, channelList, NIDIGITAL_VAL_DISCONNECT));

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
