/*
* Example Program:
*  NI Digital Log History RAM Results to File.c
*
* Description:
*  Demonstrates how to use the NI-Digital Pattern Driver API to log History RAM results.
*/

/* Includes */
#include <conio.h>
#include <stdio.h>
#include <stdlib.h>
#include <niDigital.h>
#include <errno.h>
#include <string.h>

char GetPinStateChar(ViUInt8 pinStateFromHram);
ViString AllocateViString(ViInt64 size);
ViString* AllocateViStringArray(ViInt64 size);
ViInt32* AllocateViInt32Array(ViInt64 size);
ViInt64* AllocateViInt64Array(ViInt64 size);
ViUInt8* AllocateViUInt8Array(ViInt64 size);
ViUInt8** AllocateViUInt8PtrArray(ViInt64 size);
void DeallocateViStringArray(ViString array[], ViInt64 size);
void DeallocateViUInt8PtrArray(ViUInt8* array[], ViInt64 size);

int main(void)
{
	/* Initial Device Values */
	ViRsrc deviceID = "PXI1Slot2,PXI1Slot3";
	ViInt32 preTriggerSamples = 0;
	ViInt32 cyclesToAcquire = NIDIGITAL_VAL_FAILED_CYCLES;
	ViInt32 actualNumPins;
	ViConstString fileName = "HistoryRAMResults.csv";
	ViBoolean selectDigitalFunction = VI_TRUE;
	ViBoolean waitUntilDone = VI_TRUE;
	ViReal64 timeout = 10;
	ViInt64 sampleCount;
	ViInt32 actualNumPinData = 0;

	/* IVI Driver Variables */
	ViSession vi = VI_NULL;
	ViStatus error = VI_SUCCESS;
	ViChar errDesc[IVI_MAX_MESSAGE_BUF_SIZE];
	ViConstString startLabel = "new_pattern";
	ViConstString siteList = "site0";

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

	/* Configure the History RAM to trigger on first failure */
	checkErr(niDigital_ConfigureFirstFailureHistoryRAMTrigger(vi, preTriggerSamples));

	/* Configure which cycles to acquire for History RAM */
	checkErr(niDigital_ConfigureHistoryRAMCyclesToAcquire(vi, cyclesToAcquire));

	/* Burst the pattern */
	checkErr(niDigital_BurstPattern(vi, siteList, startLabel, selectDigitalFunction,
		waitUntilDone, timeout));

	/*
	* Get the pin list for the pattern. This is used to make sure the fetched History
	* RAM results align with the pin names in the HistoryRAMResults.csv file
	*/
	ViInt32 pinListSize = niDigital_GetPatternPinList(vi, startLabel, 0, VI_NULL);
	ViString pinList = AllocateViString(pinListSize);
	checkErr(niDigital_GetPatternPinList(vi, startLabel, pinListSize, pinList));

	/*
	* Get the History RAM sample count for the specified site. This will be used to
	* determine how many History RAM samples to fetch
	*/
	checkErr(niDigital_GetHistoryRAMSampleCount(vi, siteList, &sampleCount));

	/*
	* Fetch the pattern index, the timeset index, the vector number, and the cycle
	* number for each History RAM sample
	*/
	ViInt32* patternIndexes = AllocateViInt32Array(sampleCount);
	ViInt32* timeSetIndexes = AllocateViInt32Array(sampleCount);
	ViInt64* vectorNumbers = AllocateViInt64Array(sampleCount);
	ViInt64* cycleNumbers = AllocateViInt64Array(sampleCount);
	for (ViInt64 i = 0; i < sampleCount; ++i)
	{
		checkErr(niDigital_FetchHistoryRAMCycleInformation(vi, siteList, i, &patternIndexes[i],
			&timeSetIndexes[i], &vectorNumbers[i], &cycleNumbers[i], VI_NULL));
	}

	/* Get the pattern and timeset name for each pattern and timeset index */
	ViString* patternNames = AllocateViStringArray(sampleCount);
	ViString* timeSetNames = AllocateViStringArray(sampleCount);
	for (ViInt64 i = 0; i < sampleCount; ++i)
	{
		ViInt32 patternNameSize = niDigital_GetPatternName(vi, patternIndexes[i], 0, VI_NULL);
		patternNames[i] = AllocateViString(patternNameSize);
		checkErr(niDigital_GetPatternName(vi, patternIndexes[i], patternNameSize, patternNames[i]));

		ViInt32 timeSetNameSize = niDigital_GetTimeSetName(vi, timeSetIndexes[i], 0, VI_NULL);
		timeSetNames[i] = AllocateViString(timeSetNameSize);
		checkErr(niDigital_GetTimeSetName(vi, timeSetIndexes[i], timeSetNameSize, timeSetNames[i]));
	}
	free(patternIndexes);
	free(timeSetIndexes);

	/* Fetch the expected and actual pin states from History RAM */
	ViUInt8** expectedPinStates = AllocateViUInt8PtrArray(sampleCount);
	ViUInt8** actualPinStates = AllocateViUInt8PtrArray(sampleCount);
	for (ViInt64 i = 0; i < sampleCount; ++i)
	{
		checkErr(niDigital_FetchHistoryRAMCyclePinData(vi, siteList, pinList, i, 0, 0, VI_NULL, VI_NULL,
			VI_NULL, &actualNumPinData));
		expectedPinStates[i] = AllocateViUInt8Array(actualNumPinData);
		actualPinStates[i] = AllocateViUInt8Array(actualNumPinData);
		checkErr(niDigital_FetchHistoryRAMCyclePinData(vi, siteList, pinList, i, 0, actualNumPinData,
			expectedPinStates[i], actualPinStates[i], VI_NULL, VI_NULL));
	}
	free(pinList);

	/*
	* Get pin names for loaded pattern. Pin names will be used in the header of the
	* HistoryRAMResults.csv file
	*/
	checkErr(niDigital_GetPatternPinIndexes(vi, startLabel, 0, VI_NULL, &actualNumPins));
	ViInt32* pinIndexes = AllocateViInt32Array(actualNumPins);
	checkErr(niDigital_GetPatternPinIndexes(vi, startLabel, actualNumPins, pinIndexes, VI_NULL));
	ViString* pinNames = AllocateViStringArray(actualNumPins);
	for (ViInt32 i = 0; i < actualNumPins; ++i)
	{
		ViInt32 pinNameSize = niDigital_GetPinName(vi, pinIndexes[i], 0, VI_NULL);
		pinNames[i] = AllocateViString(pinNameSize);
		checkErr(niDigital_GetPinName(vi, pinIndexes[i], pinNameSize, pinNames[i]));
	}
	free(pinIndexes);

	/* Open the HistoryRAMResults.csv file */
	FILE *fp;
	checkErr(fopen_s(&fp, fileName, "w"));

	/* Write the header to the HistoryRAMResults.csv file */
	fprintf(fp, "TimeSet,Pattern,Vector,Cycle");
	for (ViInt32 i = 0; i < actualNumPins; ++i)
	{
		fprintf(fp, ",%s actual", pinNames[i]);
	}
	for (ViInt32 i = 0; i < actualNumPins; ++i)
	{
		fprintf(fp, ",%s expected", pinNames[i]);
	}
	DeallocateViStringArray(pinNames, actualNumPinData);
	fprintf(fp, "\n");

	/* Write the History RAM results to the HistoryRAMResults.csv file */
	for (ViInt64 i = 0; i < sampleCount; ++i)
	{
		fprintf(fp, "%s,%s,%llu,%llu", timeSetNames[i], patternNames[i],
			vectorNumbers[i], cycleNumbers[i]);
		for (ViInt32 n = 0; n < actualNumPinData; ++n)
		{
			fprintf(fp, ",%c", GetPinStateChar(actualPinStates[i][n]));
		}
		for (ViInt32 n = 0; n < actualNumPinData; ++n)
		{
			fprintf(fp, ",%c", GetPinStateChar(expectedPinStates[i][n]));
		}
		fprintf(fp, "\n");
	}
	fclose(fp);

	/* Clean up all remaining allocations */
	DeallocateViStringArray(timeSetNames, sampleCount);
	DeallocateViStringArray(patternNames, sampleCount);
	free(vectorNumbers);
	free(cycleNumbers);
	DeallocateViUInt8PtrArray(actualPinStates, sampleCount);
	DeallocateViUInt8PtrArray(expectedPinStates, sampleCount);

	/* Disconnect all channels using programmable onboard switching */
	checkErr(niDigital_SelectFunction(vi, "", NIDIGITAL_VAL_DISCONNECT));

	/* Print Result */
	printf("Done without error.\n");

Error:

	if (error != VI_SUCCESS) /* Error Occurred */
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

/* Convert the ViUInt8 pin state from the fetched History RAM results into a string */
char GetPinStateChar(ViUInt8 pinStateFromHram)
{
	switch (pinStateFromHram)
	{
	case NIDIGITAL_VAL_0: return '0';
	case NIDIGITAL_VAL_1: return '1';
	case NIDIGITAL_VAL_L: return 'L';
	case NIDIGITAL_VAL_H: return 'H';
	case NIDIGITAL_VAL_X: return 'X';
	case NIDIGITAL_VAL_M: return 'M';
	default: return VI_NULL;
	}
}

/* Memory allocation helper functions */
ViString AllocateViString(ViInt64 size)
{
	return (ViString)malloc(sizeof(ViChar) * (size_t)size);
}

ViString* AllocateViStringArray(ViInt64 size)
{
	return (ViString*)malloc(sizeof(ViString) * (size_t)size);
}

ViInt32* AllocateViInt32Array(ViInt64 size)
{
	return (ViInt32*)malloc(sizeof(ViInt32) * (size_t)size);
}

ViInt64* AllocateViInt64Array(ViInt64 size)
{
	return (ViInt64*)malloc(sizeof(ViInt64) * (size_t)size);
}

ViUInt8* AllocateViUInt8Array(ViInt64 size)
{
	return (ViUInt8*)malloc(sizeof(ViUInt8) * (size_t)size);
}

ViUInt8** AllocateViUInt8PtrArray(ViInt64 size)
{
	return (ViUInt8**)malloc(sizeof(ViUInt8*) * (size_t)size);
}

void DeallocateViStringArray(ViString array[], ViInt64 size)
{
	for (ViInt64 i = 0; i < size; ++i)
	{
		free(array[i]);
	}
	free(array);
}

void DeallocateViUInt8PtrArray(ViUInt8* array[], ViInt64 size)
{
	for (ViInt64 i = 0; i < size; ++i)
	{
		free(array[i]);
	}
	free(array);
}
