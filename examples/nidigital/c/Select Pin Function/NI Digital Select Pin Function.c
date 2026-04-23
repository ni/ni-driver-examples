/**
*	Example Program:
*		NI Digital Select Pin Function.c
*
*	Description:
*		Demonstrates the use of the NI-Digital Pattern Driver API to switch
*		between different pin selected functions on the digital pattern
*		instrument.
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
	ViConstString channelList = "PinGroup1";
	ViInt32 numRead = 0;
	ViUInt8 writeData = NIDIGITAL_VAL_1; /* write state to drive pin high */
	ViUInt8 * readData = VI_NULL;
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

	/* Prompt user for pin function */
	char pinSelect = 'p';
	while ((pinSelect == 'p') || (pinSelect == 'd') || (pinSelect == 'o') || (pinSelect == 'x'))
	{
		printf("\nSelect pin mode for the device [PPMU (p), Digital (d), Off (o),"
			" Disconnect (x)] or press any other character to exit: ");
		pinSelect= _getch();
		switch (pinSelect)
		{
		case 'p': /* PPMU */
			checkErr(niDigital_SelectFunction(vi, channelList, NIDIGITAL_VAL_PPMU));
			printf("Measuring pin voltages with PPMU\n");

			/* Read voltage measurements from PPMU */
			checkErr(niDigital_PPMU_Measure(vi, channelList, NIDIGITAL_VAL_MEASURE_VOLTAGE,
				0, measurements, &numRead));
			measurements = (ViReal64*)malloc(numRead * sizeof(ViReal64));
			if (measurements == NULL) {
				printf("Could not allocate host memory!\n");
				return EXIT_FAILURE;
			}
			checkErr(niDigital_PPMU_Measure(vi, channelList, NIDIGITAL_VAL_MEASURE_VOLTAGE,
				numRead, measurements, &numRead));

			/* Print out measurement results */
			for (int i = 0; i < numRead; i++)
				printf("Pin %d: %lfV\n", i, measurements[i]);
			break;

		case 'd': /* Digital */
			checkErr(niDigital_SelectFunction(vi, channelList, NIDIGITAL_VAL_DIGITAL));
			printf("Executing static read and write\n");

			/* Perform static write */
			checkErr(niDigital_WriteStatic(vi, channelList, writeData));

			/* Read states of comparators for pins you specify in the channelList */
			checkErr(niDigital_ReadStatic(vi, channelList, 0, VI_NULL, &numRead));
			readData = (ViUInt8*)malloc(numRead * sizeof(ViUInt8));
			if (readData == NULL) {
				printf("Could not allocate host memory!\n");
				return EXIT_FAILURE;
			}
			checkErr(niDigital_ReadStatic(vi, channelList, numRead, readData, &numRead));

			/* Print out read in states */
			for (int i = 0; i < numRead; i++)
			{
				char str[16];
				switch (readData[i])
				{
				case NIDIGITAL_VAL_L:
					const char * logicLowStr = "logic low";
					strcpy_s(str, strlen(logicLowStr) + 1, logicLowStr);
					break;

				case NIDIGITAL_VAL_H:
					const char * logicHighStr = "logic high";
					strcpy_s(str, strlen(logicHighStr) + 1, logicHighStr);
					break;

				case NIDIGITAL_VAL_M:
					const char * midbandStr = "midband";
					strcpy_s(str, strlen(midbandStr) + 1, midbandStr);
					break;

				case NIDIGITAL_VAL_V:
					const char * invertedStr = "VOL > VOH";
					strcpy_s(str, strlen(invertedStr) + 1, invertedStr);
					break;

				default:
					const char * readErrorStr = "Read Error";
					strcpy_s(str, strlen(readErrorStr) + 1, readErrorStr);
					break;
				}
				printf("Pin %d state: %s\n", i, str);
			}
			free(readData);
			readData = VI_NULL;
			break;

		case 'o': /* Off */
			checkErr(niDigital_SelectFunction(vi, channelList, NIDIGITAL_VAL_OFF));
			printf("Pin is electrically connected but PPMU and digital driver are off\n");
			break;

		case 'x': /* Disconnect */
			checkErr(niDigital_SelectFunction(vi, channelList, NIDIGITAL_VAL_DISCONNECT));
			printf("Pin is electrically disconnected from instrument functions\n");
			break;

		default:
			break;
		}
	}

	/* Disconnect all channels using programmable onboard switching */
	checkErr(niDigital_SelectFunction(vi, channelList, NIDIGITAL_VAL_DISCONNECT));

	/* Print Result */
	printf("Done without error.\n");

Error:

	free(measurements);
	free(readData);

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
