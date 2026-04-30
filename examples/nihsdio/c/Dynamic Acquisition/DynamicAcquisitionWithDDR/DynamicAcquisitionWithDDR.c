/************************************************************************
*
*  Example Program:
*    DynamicAcquisitionWithDDR.c
*
*  Description:
*    Acquires a simple pattern on specified channels using read
*    function and configures the device to acquire Double Data Rate
*    (DDR) data.  DDR data is acquired on both the rising and
*    falling edges of the sample clock.
*
*  Pin Connection Information:
*    None.
*
************************************************************************/


/* Includes */
#include <stdio.h>
#include "niHSDIO.h"

/* Defines */
#define SAMPLES_TO_READ 1024

int main(void)
{
   ViRsrc deviceID = "PXI1Slot2";
   ViConstString channelList = "8-15";
   ViReal64 sampleClockRate = 50.0e6;
   ViInt32 readTimeout = 10000; /* milliseconds */
   ViUInt8 patternReadU8[SAMPLES_TO_READ];
   ViUInt16 patternReadU16[SAMPLES_TO_READ];
   ViInt32 dataWidth = 1;

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
   checkErr(niHSDIO_ConfigureAcquisitionSize(vi, SAMPLES_TO_READ, 1));

   /* Configure the Data Rate Multiplier */
   checkErr(niHSDIO_SetAttributeViInt32(
            vi, VI_NULL, NIHSDIO_ATTR_DATA_RATE_MULTIPLIER,
            NIHSDIO_VAL_DOUBLE_DATA_RATE));

   /* Query the Data Width attribute to determine which
      Read function to use */
   checkErr(niHSDIO_GetAttributeViInt32(
            vi, VI_NULL, NIHSDIO_ATTR_DATA_WIDTH, &dataWidth));

   /* Read Waveform data from device */

   if (dataWidth == 1)
      checkErr(niHSDIO_ReadWaveformU8(
               vi, SAMPLES_TO_READ, readTimeout,
               &numSamplesRead, patternReadU8));
   else
      checkErr(niHSDIO_ReadWaveformU16(
               vi, SAMPLES_TO_READ, readTimeout,
               &numSamplesRead, patternReadU16));

Error:

   if (error == VI_SUCCESS)
   {
      /* print result */
      printf("Done without error.\n");
      printf("Number of samples read = %d.\n", numSamplesRead);
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
