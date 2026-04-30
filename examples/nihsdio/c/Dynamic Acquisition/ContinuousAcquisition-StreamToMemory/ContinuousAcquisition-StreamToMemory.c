/************************************************************************
*
*  Example Program:
*    ContinuousAcquisition-StreamToMemory.c
*
*  Description:
*    This example demonstrates how to continuously acquire dynamic data
*    using the niHSDIO Fetch Waveform VI at the specified clock rate.
*    The niHSDIO Fetch Waveform VI returns data previously acquired and
*    transfers the data from the device onboard memory to the PC memory.
*    Note that the maximum clock rate that can be used with this VI could
*    be much less than the maximum rate of the NI-HSDIO device. This
*    difference could be caused by the maximum rate of the PCI/PXI bus.
*
*  Pin Connection Information:
*    Connect your signals to the input terminals that match the channels
*    specified by Channel List. For this example, wire your signals to
*    first 16 digital channels of the device.
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
   ViConstString channelList = "0-15";
   ViReal64 sampleClockRate = 1.0e6;
   ViInt32 readTimeout = 10000; /* milliseconds */
   ViUInt8 patternReadU8[SAMPLES_TO_READ];
   ViUInt16 patternReadU16[SAMPLES_TO_READ];
   ViUInt32 patternReadU32[SAMPLES_TO_READ];
   ViInt32 dataWidth = 4;

   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViInt32 numSamplesRead = 0;
   ViChar errDesc[1024];
   int i;
   const int loopIterations=10;

   /* Initialize acquisition session */
   checkErr(niHSDIO_InitAcquisitionSession(
            deviceID, VI_FALSE, VI_FALSE, VI_NULL, &vi));

   /* Assign channels for dynamic acquisition */
   checkErr(niHSDIO_AssignDynamicChannels (vi, channelList));

   /* Configure Sample clock parameters */
   checkErr(niHSDIO_ConfigureSampleClock(
            vi, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Configure the acquistion to be continuous (not finite). */
   checkErr(niHSDIO_SetAttributeViBoolean (vi, "",
            NIHSDIO_ATTR_SAMPLES_PER_RECORD_IS_FINITE, VI_FALSE));

   /* Initiate acquisition to device */
   checkErr(niHSDIO_Initiate(vi));

   /* Query the Data Width attribute */
   checkErr(niHSDIO_GetAttributeViInt32(
            vi, VI_NULL, NIHSDIO_ATTR_DATA_WIDTH, &dataWidth));

   /* Configure Fetch */
   checkErr(niHSDIO_SetAttributeViInt32 (vi, "",
            NIHSDIO_ATTR_FETCH_RELATIVE_TO, NIHSDIO_VAL_FIRST_SAMPLE));

   for (i=0; i<loopIterations; i++)
   {
      /* Configure Fetch */
      checkErr(niHSDIO_SetAttributeViInt32 (vi, "",
               NIHSDIO_ATTR_FETCH_OFFSET, i*SAMPLES_TO_READ));

      /* Read Waveform data from device */
      /* The Data Width attribute is used to determine which
      Read function should be used. */
      switch (dataWidth)
      {
         case 1:
            checkErr(niHSDIO_FetchWaveformU8(
                     vi, SAMPLES_TO_READ, readTimeout,
                     &numSamplesRead, patternReadU8));
            break;
         case 2:
            checkErr(niHSDIO_FetchWaveformU16(
                     vi, SAMPLES_TO_READ, readTimeout,
                     &numSamplesRead, patternReadU16));
            break;
         default: /* dataWidth == 4 */
            checkErr(niHSDIO_FetchWaveformU32(
                     vi, SAMPLES_TO_READ, readTimeout,
                     &numSamplesRead, patternReadU32));
            break;
      }
   }

  Error:

   if (error == VI_SUCCESS)
   {
      /* print result */
      printf("Done without error.\n");
      printf("Number of samples read = %d.\n", numSamplesRead*loopIterations);
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
