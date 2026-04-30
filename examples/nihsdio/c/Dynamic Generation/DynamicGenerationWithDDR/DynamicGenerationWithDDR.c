/************************************************************************
*
*  Example Program:
*    DynamicGenerationWithDDR.c
*
*  Description:
*    Generates a simple pattern on specified channels with Double Data
*    (DDR) data.  DDR data toggles on both the rising and falling edges
*    of the sample clock.
*
*  Pin Connection Information:
*    None.
*
************************************************************************/


/* Includes */
#include <stdio.h>
#include <limits.h>
#include "niHSDIO.h"

/* Defines */
#define WAVEFORM_SIZE 1024

int main(void)
{
   ViRsrc deviceID = "PXI1Slot2";
   ViConstString channelList = "0-7";
   ViReal64 sampleClockRate = 50.0e6;
   ViUInt8 waveformDataU8[WAVEFORM_SIZE];
   ViUInt16 waveformDataU16[WAVEFORM_SIZE];
   ViConstString waveformName = "myWfm";
   ViInt32 timeout = 10000; /* milliseconds */
   ViInt32 dataWidth = 1;

   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];
   ViInt32 i;


   /* Initialize generation session */
   checkErr(niHSDIO_InitGenerationSession(
            deviceID, VI_FALSE, VI_FALSE, VI_NULL, &vi));

   /* Assign channels for dynamic generation */
   checkErr(niHSDIO_AssignDynamicChannels (vi, channelList));

   /* Configure sample clock parameters */
   checkErr(niHSDIO_ConfigureSampleClock(
            vi, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Configure the Data Rate Multiplier Attribute */
   checkErr(niHSDIO_SetAttributeViInt32(
            vi, VI_NULL, NIHSDIO_ATTR_DATA_RATE_MULTIPLIER,
            NIHSDIO_VAL_DOUBLE_DATA_RATE));

   /* Query the Data Width Attribute to determine which
      Write function to use */
   checkErr(niHSDIO_GetAttributeViInt32(
            vi, VI_NULL, NIHSDIO_ATTR_DATA_WIDTH, &dataWidth));

   /* Populate waveform with ramp data */
   for (i = 0; i < WAVEFORM_SIZE; i++)
   {
      waveformDataU8[i] = (ViUInt8)(i%UCHAR_MAX);
      waveformDataU16[i] = (ViUInt16)(i%USHRT_MAX);
   }

   /* Write waveform to device */
   if (dataWidth == 1)
      checkErr(niHSDIO_WriteNamedWaveformU8(
               vi, waveformName, WAVEFORM_SIZE, waveformDataU8));
   else
      checkErr(niHSDIO_WriteNamedWaveformU16(
               vi, waveformName, WAVEFORM_SIZE, waveformDataU16));

   /* Initiate generation */
   checkErr(niHSDIO_Initiate(vi));

   /* Wait for generation to complete */
   checkErr(niHSDIO_WaitUntilDone(vi, timeout));

Error:

   if (error == VI_SUCCESS)
   {
      /* print result */
      printf("Done without error.\n");
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
