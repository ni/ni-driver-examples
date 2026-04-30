/************************************************************************
*
*  Example Program:
*    DynamicGenerationOfMultipleWaveforms.c
*
*  Description:
*    Writes multiple waveforms to device.
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
#define WAVEFORM_SIZE_1 1024
#define WAVEFORM_SIZE_2 2048
#define WAVEFORM_SIZE_3 4096

int main(void)
{

   ViRsrc deviceID = "PXI1Slot2";
   ViConstString channelList = "0-15";
   ViReal64 sampleClockRate = 50.0e6;
   ViUInt8 waveformDataU8[WAVEFORM_SIZE_3];
   ViUInt16 waveformDataU16[WAVEFORM_SIZE_3];
   ViUInt32 waveformDataU32[WAVEFORM_SIZE_3];
   ViInt32 dataWidth = 4;
   ViConstString waveformName1 = "myWfm1";
   ViConstString waveformName2 = "myWfm2";
   ViConstString waveformName3 = "myWfm3";
   ViInt32 timeout = 10000; /* milliseconds */

   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];
   ViInt32 i;


   /* Initialize generation session */
   checkErr(niHSDIO_InitGenerationSession(
            deviceID, VI_FALSE, VI_FALSE, VI_NULL, &vi));

   /* Assign channels for dynamic generation */
   checkErr(niHSDIO_AssignDynamicChannels (vi, channelList));

   /* Configure clocking parameters */
   checkErr(niHSDIO_ConfigureSampleClock(
            vi, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Query the Data Width attribute */
   checkErr(niHSDIO_GetAttributeViInt32(
            vi, VI_NULL, NIHSDIO_ATTR_DATA_WIDTH, &dataWidth));

   /* Populate first waveform with ramp data */
   for (i = 0; i < WAVEFORM_SIZE_1; i++)
   {
      waveformDataU8[i] = (ViUInt8)(i%UCHAR_MAX);
      waveformDataU16[i] = (ViUInt16)(i%USHRT_MAX);
      waveformDataU32[i] = i;
   }

   /* Write first waveform to device */


   if (dataWidth == 1)
   {
      checkErr(niHSDIO_WriteNamedWaveformU8(
               vi, waveformName1, WAVEFORM_SIZE_1,
               waveformDataU8));
   }
   else if (dataWidth == 2)
   {
      checkErr(niHSDIO_WriteNamedWaveformU16(
               vi, waveformName1, WAVEFORM_SIZE_1,
               waveformDataU16));
   }
   else /*dataWidth == 4 */
   {
      checkErr(niHSDIO_WriteNamedWaveformU32(
               vi, waveformName1, WAVEFORM_SIZE_1,
               waveformDataU32));
   }


   /* Populate second waveform with toggle data */
   for (i = 0; i < WAVEFORM_SIZE_2; i++)
   {
      waveformDataU8[i] = (i&1) ? 0x0 : 0xFF;
      waveformDataU16[i] = (i&1) ? 0x0 : 0xFFFF;
      waveformDataU32[i] = (i&1) ? 0x0 : 0xFFFFFFFF;
   }

   /* Write second waveform to device */
   if (dataWidth == 1)
   {
      checkErr(niHSDIO_WriteNamedWaveformU8(
               vi, waveformName2, WAVEFORM_SIZE_2,
               waveformDataU8));
   }
   else if (dataWidth == 2)
   {
      checkErr(niHSDIO_WriteNamedWaveformU16(
               vi, waveformName2, WAVEFORM_SIZE_2,
               waveformDataU16));
   }
   else /* dataWidth == 4 */
   {
      checkErr(niHSDIO_WriteNamedWaveformU32(
               vi, waveformName2, WAVEFORM_SIZE_2,
               waveformDataU32));
   }

   /* Populate thrid waveform with constant data */
   for (i = 0; i < WAVEFORM_SIZE_3; i++)
   {
      waveformDataU8[i] = 0x12;
      waveformDataU16[i] = 0x1234;
      waveformDataU32[i] = 0x12345678;
   }

   /* Write third waveform to device */
   if (dataWidth == 1)
   {
      checkErr(niHSDIO_WriteNamedWaveformU8(
               vi, waveformName3, WAVEFORM_SIZE_3,
               waveformDataU8));
   }
   else if (dataWidth == 2)
   {
      checkErr(niHSDIO_WriteNamedWaveformU16(
               vi, waveformName3, WAVEFORM_SIZE_3,
               waveformDataU16));
   }
   else /* dataWidth == 4 */
   {
      checkErr(niHSDIO_WriteNamedWaveformU32(
               vi, waveformName3, WAVEFORM_SIZE_3,
               waveformDataU32));
   }

   /* Generate first waveform */
   printf("Generating waveform: %s...\n", waveformName1);
   checkErr(niHSDIO_ConfigureWaveformToGenerate(vi, waveformName1));
   checkErr(niHSDIO_Initiate(vi));
   checkErr(niHSDIO_WaitUntilDone(vi, timeout));

   /* Generate second waveform */
   printf("Generating waveform: %s...\n", waveformName2);
   checkErr(niHSDIO_ConfigureWaveformToGenerate(vi, waveformName2));
   checkErr(niHSDIO_Initiate(vi));
   checkErr(niHSDIO_WaitUntilDone(vi, timeout));

   /* Generate third waveform */
   printf("Generating waveform: %s...\n", waveformName3);
   checkErr(niHSDIO_ConfigureWaveformToGenerate(vi, waveformName3));
   checkErr(niHSDIO_Initiate(vi));
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
