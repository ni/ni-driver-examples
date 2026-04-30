/************************************************************************
*
*  Example Program:
*    DynamicAcquisitionofMultipleRecords.c
*
*  Description:
*    Acquires multiple records using the MultiRecord capability and
*    the Advance trigger. The advance trigger tells the device to
*    begin acquiring the next record once the previous record has
*    completed.
*
*  Pin Connection Information:
*    Connect the advance trigger to the appporiate terminal
*    (PFI 0 by default).
*
************************************************************************/


/* Includes */
#include <stdio.h>
#include "niHSDIO.h"

/* Defines */
#define SAMPLES_TO_READ 1024
#define NUMBER_OF_RECORDS 2

int main(void)
{
   ViRsrc deviceID = "PXI1Slot2";
   ViConstString channelList = "0-15";
   ViReal64 sampleClockRate = 50.0e6;
   ViConstString triggerTerminal = NIHSDIO_VAL_PFI0_STR;
   ViInt32 triggerEdge = NIHSDIO_VAL_RISING_EDGE;
   ViInt32 readTimeout = 10000; /* milliseconds */
   ViUInt8 patternReadU8[SAMPLES_TO_READ * NUMBER_OF_RECORDS];
   ViUInt16 patternReadU16[SAMPLES_TO_READ * NUMBER_OF_RECORDS];
   ViUInt32 patternReadU32[SAMPLES_TO_READ * NUMBER_OF_RECORDS];
   ViInt32 dataWidth = 4;

   ViInt32 startingRecord = 0;
   struct niHSDIO_wfmInfo waveformInfo[NUMBER_OF_RECORDS];
   ViInt32 sampleOffset = 0;
   ViUInt32 testSample = 0;

   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];


   /* Initialize acquisition session */
   checkErr(niHSDIO_InitAcquisitionSession(
            deviceID, VI_FALSE, VI_FALSE, VI_NULL, &vi));

   /* Assign channels for dynamic acquisition */
   checkErr(niHSDIO_AssignDynamicChannels (vi, channelList));

   /* Configure sample clock parameters */
   checkErr(niHSDIO_ConfigureSampleClock(
            vi, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Configure advance trigger */
   checkErr(niHSDIO_ConfigureDigitalEdgeAdvanceTrigger(
            vi, triggerTerminal, triggerEdge));

   /* Configure the number of samples to acquire to device */
   checkErr(niHSDIO_ConfigureAcquisitionSize(vi, SAMPLES_TO_READ, NUMBER_OF_RECORDS));

   /* Query the Data Width attribute */
   checkErr(niHSDIO_GetAttributeViInt32(
            vi, VI_NULL, NIHSDIO_ATTR_DATA_WIDTH, &dataWidth));

   /* Read Waveform data from device */
   /* The Data Width attribute is used to determine which
      Read and function should be used. */
   /* The 5th sample of the last record is placed into testSample
      to demostrate the use of the wfmInfo structure. */
   if (dataWidth == 1)
   {
      checkErr(niHSDIO_ReadMultiRecordU8(
               vi, SAMPLES_TO_READ, readTimeout,startingRecord,
               NUMBER_OF_RECORDS,patternReadU8, waveformInfo));

      sampleOffset = (waveformInfo[0].actualSamplesRead)+5;
      testSample = patternReadU8[sampleOffset];
   }
   else if (dataWidth == 2)
   {
      checkErr(niHSDIO_ReadMultiRecordU16(
               vi, SAMPLES_TO_READ, readTimeout,startingRecord,
               NUMBER_OF_RECORDS,patternReadU16, waveformInfo));

      sampleOffset = (waveformInfo[0].actualSamplesRead)+5;
      testSample = patternReadU16[sampleOffset];
   }
   else  /* dataWidth == 4 */
   {
      checkErr(niHSDIO_ReadMultiRecordU32(
               vi, SAMPLES_TO_READ, readTimeout,startingRecord,
               NUMBER_OF_RECORDS,patternReadU32, waveformInfo));

      sampleOffset = ((NUMBER_OF_RECORDS - 1) * SAMPLES_TO_READ) + 5;
      testSample = patternReadU32[sampleOffset];
   }

Error:

   if (error == VI_SUCCESS)
   {
      /* print result */
      printf("Done without error.\n");
      printf("Number of samples read = %d samples per record. The 5th sample of the last record is %d.\n", waveformInfo[0].actualSamplesRead, testSample);
   }
   else
   {
      /* Get error description and print */
      niHSDIO_GetError(vi, &error, sizeof(errDesc)/sizeof(ViChar), errDesc);

      printf("\nError encountered\n===================\n%s\n", errDesc);
   }

   /* close the session */
   niHSDIO_close(vi);

   /* prompt to exit (for pop-up console windows) */
   printf("\nHit <Enter> to continue...\n");
   getchar();

   return error;
}
