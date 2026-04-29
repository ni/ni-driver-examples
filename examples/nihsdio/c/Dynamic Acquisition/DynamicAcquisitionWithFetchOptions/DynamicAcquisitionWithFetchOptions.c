/************************************************************************
*
*  Example Program:
*    DynamicAcquisitionWithFetchOptions.c
*
*  Description:
*    Fetches a simple pattern from a specified location within
*    the acquired data buffer.
*
*  Pin Connection Information:
*    None.
*
************************************************************************/


/* Includes */
#include <stdio.h>
#include "niHSDIO.h"

/* Defines */
#define SAMPLES_TO_FETCH 512

int main(void)
{

   ViRsrc deviceID = "PXI1Slot2";
   ViConstString channelList = "0-15";
   ViReal64 sampleClockRate = 50.0e6;
   ViInt32 readTimeout = 10000; /* milliseconds */
   ViInt32 acquisitionSize = 1024;
   ViInt32 fetchLocation = NIHSDIO_VAL_FIRST_SAMPLE;
   ViInt32 fetchOffset = 512;
   ViUInt8 patternReadU8[SAMPLES_TO_FETCH];
   ViUInt16 patternReadU16[SAMPLES_TO_FETCH];
   ViUInt32 patternReadU32[SAMPLES_TO_FETCH];
   ViInt32 dataWidth = 4;

   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViInt32 numSamplesRead = 0;
   ViChar errDesc[1024];


   /* Initialize acquisition session */
   checkErr(niHSDIO_InitAcquisitionSession(
            deviceID, VI_FALSE, VI_FALSE, VI_NULL, &vi));

   /* Assign channels for dynamic acquisition */
   checkErr(niHSDIO_AssignDynamicChannels (vi, channelList));


   /* Configure sample clock parameters */
   checkErr(niHSDIO_ConfigureSampleClock(
            vi, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Configure the number of samples to acquire to device */
   checkErr(niHSDIO_ConfigureAcquisitionSize(vi, acquisitionSize, 1));

   /* Configure fetch location and offset */
   checkErr(niHSDIO_SetAttributeViInt32(
            vi, VI_NULL, NIHSDIO_ATTR_FETCH_RELATIVE_TO, fetchLocation));

   checkErr(niHSDIO_SetAttributeViInt32(
            vi, VI_NULL, NIHSDIO_ATTR_FETCH_OFFSET, fetchOffset));

   /* Initiate acquisition to device */
   checkErr(niHSDIO_Initiate(vi));

   /* Wait for acquisition to complete */
   checkErr(niHSDIO_WaitUntilDone(vi, readTimeout));

   /* Query the Data Width attribute */
   checkErr(niHSDIO_GetAttributeViInt32(
            vi, VI_NULL, NIHSDIO_ATTR_DATA_WIDTH, &dataWidth));

   /* Read Waveform data from device */
   /* The Data Width attribute is used to determine which
      Read and function should be used. */
   if (dataWidth == 1)
   {
      checkErr(niHSDIO_FetchWaveformU8(
               vi, SAMPLES_TO_FETCH, readTimeout,
               &numSamplesRead, patternReadU8));
   }
   else if (dataWidth == 2)
   {
      checkErr(niHSDIO_FetchWaveformU16(
               vi, SAMPLES_TO_FETCH, readTimeout,
               &numSamplesRead, patternReadU16));
   }
   else  /* dataWidth == 4 */
   {
      checkErr(niHSDIO_FetchWaveformU32(
               vi, SAMPLES_TO_FETCH, readTimeout,
                  &numSamplesRead, patternReadU32));
   }

Error:

   if (error == VI_SUCCESS)
   {
      /* print result */
      printf("Done without error.\n");
      printf("Number of samples fetched = %d.\n", numSamplesRead);
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
