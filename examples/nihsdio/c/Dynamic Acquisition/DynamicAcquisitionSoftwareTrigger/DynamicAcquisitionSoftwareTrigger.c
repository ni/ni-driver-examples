/************************************************************************
*
*  Example Program:
*    DynamicAcquisitionSoftwareTrigger.c
*
*  Description:
*    Configures a software reference trigger in an acquisition
*    operation.
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
   ViConstString channelList = "0-15";
   ViReal64 sampleClockRate = 50.0e6;
   ViInt32 pretriggerSamples = 500;
   ViInt32 readTimeout = 10000; /* milliseconds */
   ViUInt8 patternReadU8[SAMPLES_TO_READ];
   ViUInt16 patternReadU16[SAMPLES_TO_READ];
   ViUInt32 patternReadU32[SAMPLES_TO_READ];
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

   /* Configure sotware reference trigger*/
   checkErr(niHSDIO_ConfigureSoftwareRefTrigger (vi, pretriggerSamples));

   /* Configure the number of samples to acquire to device */
   checkErr(niHSDIO_ConfigureAcquisitionSize(vi, SAMPLES_TO_READ, 1));

   /* Initiate acquisition */
   checkErr(niHSDIO_Initiate(vi));

   /* Send software trigger */
   printf("Waiting on software reference trigger. \n");
   printf("Hit <Enter> to send software reference trigger...\n");
   getchar();

   checkErr(niHSDIO_SendSoftwareEdgeTrigger(
            vi, NIHSDIO_VAL_REF_TRIGGER, VI_NULL));

   /* Wait for acquisition to complete */
   checkErr(niHSDIO_WaitUntilDone(vi, readTimeout));

   /* Query the Data Width attribute */
   checkErr(niHSDIO_GetAttributeViInt32(
            vi, VI_NULL, NIHSDIO_ATTR_DATA_WIDTH, &dataWidth));

   /* Fetch Waveform data from device */
   /* The Data Width attribute is used to determine which
      Fetch function should be used. */
   if (dataWidth == 1)
   {
      checkErr(niHSDIO_FetchWaveformU8(
               vi, SAMPLES_TO_READ, readTimeout,
               &numSamplesRead, patternReadU8));
   }
   else if (dataWidth == 2)
   {
      checkErr(niHSDIO_FetchWaveformU16(
               vi, SAMPLES_TO_READ, readTimeout,
               &numSamplesRead, patternReadU16));
   }
   else  /* dataWidth == 4 */
   {
      checkErr(niHSDIO_FetchWaveformU32(
               vi, SAMPLES_TO_READ, readTimeout,
               &numSamplesRead, patternReadU32));
   }

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
