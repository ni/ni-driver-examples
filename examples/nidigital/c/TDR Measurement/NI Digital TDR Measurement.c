/**
*	Example Program:
*		NI Digital TDR Measurement.c
*
*	Description:
*		Demonstrates how to use the NI-Digital Pattern Driver API to create an
*		instrument session and use Time-Domain Reflectometry (TDR) to measure
*		propagation delays through cables, connectors, and load boards on each
*		channel in the channel list.
*
*		Note: For TDR to operate properly, the cables and load board connected
*		to the digital pattern instrument must make a 50 Ohm controlled
*		impedance system. Ensure that the DUT sockets are empty and that the
*		channels you select are electrically open before you run the TDR
*		measurement.
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
	ViBoolean applyTDROffsets = VI_FALSE;
	ViReal64 * offsetsTDR = VI_NULL;

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
	* Use niDigital_TDR() to measure propagation delays through cables,
	* connectors, and load boards. Setting apply offsets to VI_TRUE
	* automatically applies measured offsets to the instrument. If you need to
	* adjust the measured offsets prior to applying, set apply offsets to
	* VI_FALSE, and call the niDigital_ApplyTDROffsets() function to specify
	* the adjusted TDR offsets values
	*
	* Call ni_Digital_TDR() once to find the array size necessary to hold all
	* offset values and then call again to get values back in a properly
	* allocated array
	*/
	ViInt32 numOffsets = 0;
	checkErr(niDigital_TDR(vi, "PinGroup1", applyTDROffsets, 0, NULL, &numOffsets));
	offsetsTDR = (ViReal64*)malloc(numOffsets * sizeof(ViReal64));
	checkErr(niDigital_TDR(vi, "PinGroup1", applyTDROffsets, numOffsets, offsetsTDR, &numOffsets));

	/* Disconnect all channels using programmable onboard switching */
	checkErr(niDigital_SelectFunction(vi, "", NIDIGITAL_VAL_DISCONNECT));

	/* Print Result */
	printf("Done without error.\n");
	printf("\nTDR Offsets found: ");
	for (int i = 0; i < numOffsets; i++)
		printf("%.4e ", offsetsTDR[i]);

Error:

	free(offsetsTDR);
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
