/**
*	Example Program:
*		NI Digital Self-Calibration and Self-Test.c
*
*	Description:
*		Demonstrates how to use the NI-Digital Pattern Driver API to self-test
*		and self-calibrate digital pattern instruments.
*
*	Instructions:
*		1. Populate the deviceIDs with comma-separated one or more resource names of NI
*			Digital Pattern Instruments you intend to self-calibrate and self-
*			test.
*		2. Build and run the code. Self-calibration and self-test results will
*			be printed out.
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
	* This example call self-calibrate and self-test with the session.
	*/
	ViInt16 testResult = 0; /* Used to indicate if self-test passed (0) or failed (!=0) */
	ViChar testMessage[256]; /* Used to return self-test status message */

	/*
	* Initialize the digital pattern instrument session. The reset input of the
	* niDigital Initialize With Options VI is TRUE by default to ensure that
	* the instrument starts in a known state. All channels are in a
	* high-impedance state, and the I/O switches are open
	*/
	checkErr(niDigital_InitWithOptions(deviceID, VI_TRUE, VI_TRUE, "Simulate=1,DriverSetup=Model:6570", &vi));

	/* Run self-calibration. This calibration might take several minutes to complete */
	printf("Self-calibrating device; this may take a couple minutes... ");
	checkErr(niDigital_SelfCalibrate(vi));
	printf("DONE\n");

	/* Run self-test on the instrument and print reported results */
	printf("Self-testing device; this may take a couple minutes... ");
	checkErr(niDigital_self_test(vi, &testResult, testMessage));
	printf("DONE. Self-test %s: %s\n", (testResult == 0) ? "passed" : "failed", testMessage);

	/* Close the session to the instrument */
	checkErr(niDigital_close(vi));

Error:

	if (error != VI_SUCCESS) { /* Error Occured */
		/* Get error description and print */
		niDigital_GetError(vi, &error, sizeof(errDesc) / sizeof(ViChar), errDesc);
		printf("\nError encountered\n===================\n%s\n", errDesc);
	}

	/* Prompt to exit (for pop-up console windows) */
	printf("\nHit <Enter> to continue...\n");
	_getch();

	return error;
}
