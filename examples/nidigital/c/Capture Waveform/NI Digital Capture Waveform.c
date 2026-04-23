/**
*	Example Program:
*		NI Digital Capture Waveform.c
*
*	Description:
*		Demonstrates how to use the NI-Digital Pattern Driver API to capture
*		data using a capture waveform.
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
	ViConstString pinList = "DUTPin1";
	ViConstString waveName = "new_waveform";
	ViInt32 numSamples = 4;
	ViUInt32 * data = VI_NULL;

	/*
	* When using a file to set the capture waveform, set this constant to the
	* fully qualified file path to the *.digicapture file.
	*/
	ViConstString waveFilePath = "CaptureWaveform.digicapture";

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

	/* User selects type of Waveform Capture */
	char chSelect = 0;
	while ((chSelect != 's') && (chSelect != 'p') && (chSelect != 'f'))
	{
		printf("\nSelect Capture Waveform Type [Serial (s), Parallel (p), From File (f)]:\n");
		chSelect = _getch();
		switch (chSelect)
		{
		case 's': /* Serial */
			/* This example sets sampleWidth to 32 and bitOrder to MSB first */
			checkErr(niDigital_CreateCaptureWaveformSerial(vi, pinList, waveName, 32,
				NIDIGITAL_VAL_MSB_FIRST));
			break;

		case 'p': /* Parallel */
			checkErr(niDigital_CreateCaptureWaveformParallel(vi, pinList, waveName));
			break;

		case 'f': /* From File */
			checkErr(niDigital_CreateCaptureWaveformFromFileDigicapture(vi, waveName,
				waveFilePath));
			break;

		default:
			printf("\nPlease enter either 's', 'p' or 'f' character to specify capture type\n");
			break;
		}
	}

	/* Burst the pattern from the start label (specified here as new_pattern) */
	checkErr(niDigital_BurstPattern(vi, "", "new_pattern", VI_TRUE, VI_TRUE, 10));

	/* Fetch the capture waveform from memory instrument memory */
	ViInt32 numWaveforms = 0, numSamplesPerWave = 0;
	/* Get the numver of waveforms and samples per waveform to be returned*/
	checkErr(niDigital_FetchCaptureWaveformU32(vi, "", waveName, numSamples, 10, 0,
		VI_NULL, &numWaveforms, &numSamplesPerWave));
	/*
	* The size of the data buffer is the number of waveforms multiplied by the
	* number of samples per waveform.
	*/
	ViInt32 dataSize = numWaveforms * numSamplesPerWave;
	data = (ViUInt32*)malloc(dataSize * sizeof(ViUInt32));
	checkErr(niDigital_FetchCaptureWaveformU32(vi, "", waveName, numSamples, 10,
		dataSize, data, &numWaveforms, &numSamplesPerWave));

	/* Disconnect all channels using programmable onboard switching */
	checkErr(niDigital_SelectFunction(vi, "", NIDIGITAL_VAL_DISCONNECT));

	/* Print Result */
	printf("Done without error.\n");
	printf("Capture Waveform Data:\n");
	for (ViUInt64 i = 0; i < dataSize; i++)
		printf("%u, ", data[i]);

Error:

	free(data);
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
