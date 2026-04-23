/**
*	Example Program:
*		NI Digital Loopback and Compare.c
*
*	Description:
*		Demonstrates how to use the NI-Digital Pattern Driver API to create and
*		configure an instrument session and to burst a pattern on the digital
*		pattern instrument, while checking for pass/fail conditions in a
*		loopback configuration.
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
	ViConstString burstSiteList = "";
	ViBoolean * passFailList = VI_NULL;
	ViInt32 pfListSize = 0;

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

	/*
	* Burst the pattern from the start label you specify, which can be a label
	* in the .digipat pattern file or the name of the pattern. The front panel
	* displays pass/fail information.
	*/
	checkErr(niDigital_BurstPattern(vi, burstSiteList, "new_pattern", VI_TRUE,
		VI_TRUE, 10));
	/* Set passFailBufferSize to 0 to get the size of the pass/fail array */
	checkErr(niDigital_GetSitePassFail(vi, burstSiteList, 0, VI_NULL,
		&pfListSize));
	passFailList = (ViBoolean*)malloc(pfListSize * sizeof(ViBoolean));
	checkErr(niDigital_GetSitePassFail(vi, burstSiteList, pfListSize, passFailList,
		&pfListSize));

	/* Disconnect all channels using programmable onboard switching */
	checkErr(niDigital_SelectFunction(vi, "", NIDIGITAL_VAL_DISCONNECT));

	/* Print Result */
	printf("Done without error.\n");
	printf("\nPass-Fail Results after burst:\n");
	for (int i = 0; i < pfListSize; i++)
	{
		if (passFailList[i] == VI_TRUE)
			printf("%i PASS", i);
		else
			printf("%i FAIL", i);
	}

Error:

	free(passFailList);
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
