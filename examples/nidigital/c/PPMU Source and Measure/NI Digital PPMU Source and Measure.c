/**
*	Example Program:
*		NI Digital PPMU Source and Measure.c
*
*	Description:
*		Demonstrates how to use the NI-Digital Pattern Driver API to configure
*		and source/measure voltage and current using the PPMU on selected
*		channels/pins of the digital pattern instrument.
*
*/

/* Includes */
#include <conio.h>
#include <stdio.h>
#include <stdlib.h>
#include <windows.h>
#include <niDigital.h>

int main(void)
{
	/* Initial Device Values */
	ViRsrc deviceID = "PXI1Slot2,PXI1Slot3";
	ViConstString channelName = "DUTPin1, SystemPin1";
	ViReal64 apertureTime = 4.0e-6;
	ViReal64 * measurements = VI_NULL;

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

	/* Configure the aperture time for all PPMU pperations */
	checkErr(niDigital_PPMU_ConfigureApertureTime(vi, channelName, apertureTime,
		NIDIGITAL_VAL_SECONDS));

	/* Prompt user for PPMU function mode */
	char ppmuSelect = 0;
	ViReal64 temp = 0, temp2 = 0;
	while ((ppmuSelect!= 'v') && (ppmuSelect!= 'c') && (ppmuSelect!= 'n'))
	{
		printf("\nSelect PPMU mode for the device [DC Voltage (v), DC Current (c),"
			" None (n)]: ");
		ppmuSelect= _getch();
		switch (ppmuSelect)
		{
		case 'v': /* DC Voltage */
			checkErr(niDigital_PPMU_ConfigureOutputFunction(vi, channelName,
				NIDIGITAL_VAL_DC_VOLTAGE));
			/*
			* Configure the DC current limit range. Note: Refer to the
			* instrument specifications to determine valid ranges. Some
			* instruments might not current limit the source voltage within the
			* selected range. If this is the case, this value determines the
			* accuracy of the voltage sourced.
			*/
			printf("Enter the current limit range (A): ");
			scanf_s("%lf", &temp);
			checkErr(niDigital_PPMU_ConfigureCurrentLimitRange(vi, channelName, temp));
			printf("Enter the DC voltage to source: ");
			scanf_s("%lf", &temp);
			checkErr(niDigital_PPMU_ConfigureVoltageLevel(vi, channelName, temp));
			checkErr(niDigital_PPMU_Source(vi, channelName));

			/* Insert a 10ms settling time between sourcing and measuring */
			Sleep(10);
			break;

		case 'c': /* DC Current */
			checkErr(niDigital_PPMU_ConfigureOutputFunction(vi, channelName,
				NIDIGITAL_VAL_DC_CURRENT));
			/*
			* Configure the DC current level range to source. Note: The range
			* you select determines the accuracy of the current sourced. Use
			* the smallest range that allows you to source the current level
			* you want.
			*/
			printf("Enter the current level range (A): ");
			scanf_s("%lf", &temp);
			checkErr(niDigital_PPMU_ConfigureCurrentLevelRange(vi, channelName, temp));
			printf("Enter the current level (A): ");
			scanf_s("%lf", &temp);
			checkErr(niDigital_PPMU_ConfigureCurrentLevel(vi, channelName, temp));
			printf("Enter the high voltage limit: ");
			scanf_s("%lf", &temp);
			printf("Enter the low voltage limit: ");
			scanf_s("%lf", &temp2);
			checkErr(niDigital_PPMU_ConfigureVoltageLimits(vi, channelName, temp2, temp));

			/* Insert a 10ms settling time between sourcing and measuring */
			Sleep(10);
			break;

		case 'n': /* None */
			/*
			* Bypass sourcing voltage or current and move to measure voltage
			* or current only
			*/
			break;

		default:
			printf("\nPlease enter either 'v', 'c' or 'n' character to specify"
				" PPMU type\n");
			break;
		}
	}

	/* Measure a voltage or current based on measurement type */
	ViInt32 measureType = NIDIGITAL_VAL_MEASURE_VOLTAGE, numRead = 0;
	printf("\nMeasure current (c) or voltage (v)? ");
	scanf_s("%c", &ppmuSelect, 1);
	if (ppmuSelect == 'c') /* Only change measurement type if user selects current */
		measureType = NIDIGITAL_VAL_MEASURE_CURRENT;

	checkErr(niDigital_PPMU_Measure(vi, channelName, measureType, 0, measurements,
		&numRead));
	measurements = (ViReal64*)malloc(numRead * sizeof(ViReal64));
	checkErr(niDigital_PPMU_Measure(vi, channelName, measureType, numRead, measurements,
		&numRead));

	/* Disconnect all channels using programmable onboard switching */
	checkErr(niDigital_SelectFunction(vi, channelName, NIDIGITAL_VAL_DISCONNECT));

	/* Print Result */
	printf("Done without error.\n");
	for (int i = 0; i < numRead; i++)
	{
		printf("%lf %c ", measurements[i],
			(measureType == NIDIGITAL_VAL_MEASURE_CURRENT) ? 'A' : 'V');
	}

Error:

	free(measurements);
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
