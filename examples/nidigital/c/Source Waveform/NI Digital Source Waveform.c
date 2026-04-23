/**
*	Example Program:
*		NI Digital Source Waveform.c
*
*	Description:
*		Demonstrates how to use the NI-Digital Pattern Driver API to configure
*		and create a source waveform using source memory.
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

	/* Source Waveform Values */
	ViInt32 dataMapping = NIDIGITAL_VAL_BROADCAST,
		bitOrder = NIDIGITAL_VAL_MSB_FIRST,
		waveformSize = 10, /* number of data samples in data array for broadcast operation */
		numWaveforms = 2, /* number of unique waveforms for site unique operation */
		samplesPerWaveform = 5; /* number of samples per unique waveform for site unique */
	ViUInt32 sampleWidth = 32;
	ViUInt32 data[] = { /* waveform data array */
		0x00, 0x02, 0x04, 0x06, 0x08,
		0x06, 0x04, 0x02, 0x00, 0x00
	};
	ViConstString waveName = "new_waveform",
		parallelChannelList = "PinGroup1",
		serialChannelList = "DUTPin1",
		siteUniqueList = "site0,site1",
		tdmsFilePath = "C:\\";

	/*
	* File paths to sheets
	*
	* Note: In this example, only the sheet file names are used in the string
	* constants as it is assumed the sheets will be in the same directory as
	* the compiled executable and are thus relative paths. If your sheets are
	* located anywhere else, change the path strings as needed.
	*/
	ViConstString pinMapPath = "PinMap.pinmap",
		specPath = "Specifications.specs",
		pinLevelsPath = "PinLevels.digilevels",
		timingPath = "Timing.digitiming",
		patternPath = "Pattern.digipat";

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
	checkErr(niDigital_LoadPinMap(vi, pinMapPath));

	/*
	* Load the specifications, levels, and timing files you created in the
	* Digital Pattern Editor on the instrument.
	*/
	checkErr(niDigital_LoadSpecifications(vi, specPath));
	checkErr(niDigital_LoadLevels(vi, pinLevelsPath));
	checkErr(niDigital_LoadTiming(vi, timingPath));
	checkErr(niDigital_ApplyLevelsAndTiming(vi, "", pinLevelsPath, timingPath,
		"", "", ""));

	/* Load the .digipat pattern file created in the Digital Pattern Editor */
	checkErr(niDigital_LoadPattern(vi, patternPath));

	/* Prompt user for source waveform type */
	printf("Select waveform source type [Serial (s), Parallel (p) or TDMS (t)]: ");
	char sourceType = _getch();
	switch (sourceType)
	{
	case 's': /* Serial */
		checkErr(niDigital_CreateSourceWaveformSerial(vi, serialChannelList, waveName,
			dataMapping, sampleWidth, bitOrder));
		break;

	case 'p': /* Parallel */
		checkErr(niDigital_CreateSourceWaveformParallel(vi, parallelChannelList,
			waveName, dataMapping));
		break;

	default: /* TDMS File (default) */
		checkErr(niDigital_CreateSourceWaveformFromFileTDMS(vi, waveName, tdmsFilePath,
			VI_TRUE));
	}

	/* Specify the data type to write the source waveform */
	if ((sourceType == 's') || (sourceType == 'p')) { /* Serial or parallel operation only */
		if (dataMapping == NIDIGITAL_VAL_BROADCAST) { /* Broadcast */
			checkErr(niDigital_WriteSourceWaveformBroadcastU32(vi, waveName,
				waveformSize, data));
		}
		else { /* Site unique */
			checkErr(niDigital_WriteSourceWaveformSiteUniqueU32(vi, siteUniqueList,
				waveName, numWaveforms, samplesPerWaveform, data));
		}
	}

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
