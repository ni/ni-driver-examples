/************************************************************************
*
*  Example Program:
*    DynamicGenerationWithReferenceClockPCI.c
*
*  Description:
*    Master board exports the Onboard Reference Clock to RTSI7.
*    Master and slave boards use RTSI7 as reference clock.
*    Master and slave boards lock the sample clock to the configured
*    reference clock.
*
*  Pin Connection Information:
*    10MHz reference clock on terminal specified
*    (default: RTSI7).
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

   ViRsrc masterDeviceID = "Dev1";
   ViRsrc slaveDeviceID = "Dev2";
   ViConstString channelList = "0-15";
   ViReal64 sampleClockRate = 50.0e6;
   ViInt32 dataWidthMaster = 4;
   ViInt32 dataWidthSlave = 4;
   ViUInt8 waveformDataU8[WAVEFORM_SIZE];
   ViUInt16 waveformDataU16[WAVEFORM_SIZE];
   ViUInt32 waveformDataU32[WAVEFORM_SIZE];
   ViConstString waveformName = "myWfm";
   ViInt32 timeout = 10000;

   ViSession viMaster = VI_NULL;
   ViSession viSlave = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];
   ViInt32 i;


   /* Initialize generation session */
   checkErr(niHSDIO_InitGenerationSession(
            masterDeviceID, VI_FALSE, VI_FALSE, VI_NULL, &viMaster));

   checkErr(niHSDIO_InitGenerationSession(
            slaveDeviceID, VI_FALSE, VI_FALSE, VI_NULL, &viSlave));

   /* Assign channels for dynamic generation */
   checkErr(niHSDIO_AssignDynamicChannels (viMaster, channelList));

   checkErr(niHSDIO_AssignDynamicChannels (viSlave, channelList));

   /* Export Onboard Reference Clock */
   checkErr(niHSDIO_ExportSignal(
            viMaster, NIHSDIO_VAL_ONBOARD_REF_CLOCK, "", NIHSDIO_VAL_RTSI7_STR));

   /* Configure clocking parameters */
   checkErr(niHSDIO_ConfigureRefClock(
            viMaster, NIHSDIO_VAL_RTSI7_STR, 10.0e6));

   checkErr(niHSDIO_ConfigureRefClock(
            viSlave, NIHSDIO_VAL_RTSI7_STR, 10.0e6));

   checkErr(niHSDIO_ConfigureSampleClock(
            viMaster, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   checkErr(niHSDIO_ConfigureSampleClock(
            viSlave, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Query the Data Width Attribute */
   checkErr(niHSDIO_GetAttributeViInt32(
            viMaster, VI_NULL, NIHSDIO_ATTR_DATA_WIDTH, &dataWidthMaster));
   checkErr(niHSDIO_GetAttributeViInt32(
            viSlave, VI_NULL, NIHSDIO_ATTR_DATA_WIDTH, &dataWidthSlave));

   /* Populate waveform with ramp data */
   for (i = 0; i < WAVEFORM_SIZE; i++)
   {
      waveformDataU8[i] = (ViUInt8)(i%UCHAR_MAX);
      waveformDataU16[i] = (ViUInt16)(i%USHRT_MAX);
      waveformDataU32[i] = i;
   }

   /* Write waveform to device */
   /* The Data Width attribure is used to determine which
      Write function should be used */

   if (dataWidthMaster == 1)
   {
   checkErr(niHSDIO_WriteNamedWaveformU8(
            viMaster, waveformName, WAVEFORM_SIZE, waveformDataU8));
   checkErr(niHSDIO_WriteNamedWaveformU8(
            viMaster, waveformName, WAVEFORM_SIZE, waveformDataU8));
   }
   else if (dataWidthMaster == 2)
   {
   checkErr(niHSDIO_WriteNamedWaveformU16(
            viMaster, waveformName, WAVEFORM_SIZE, waveformDataU16));
   checkErr(niHSDIO_WriteNamedWaveformU16(
            viMaster, waveformName, WAVEFORM_SIZE, waveformDataU16));
   }
   else   /*dataWidthMaster == 4*/
   {
   checkErr(niHSDIO_WriteNamedWaveformU32(
            viMaster, waveformName, WAVEFORM_SIZE, waveformDataU32));
   checkErr(niHSDIO_WriteNamedWaveformU32(
            viMaster, waveformName, WAVEFORM_SIZE, waveformDataU32));
   }

   if (dataWidthSlave == 1)
   {
   checkErr(niHSDIO_WriteNamedWaveformU8(
            viSlave, waveformName, WAVEFORM_SIZE, waveformDataU8));
   checkErr(niHSDIO_WriteNamedWaveformU8(
            viSlave, waveformName, WAVEFORM_SIZE, waveformDataU8));
   }
   else if (dataWidthSlave == 2)
   {
   checkErr(niHSDIO_WriteNamedWaveformU16(
            viSlave, waveformName, WAVEFORM_SIZE, waveformDataU16));
   checkErr(niHSDIO_WriteNamedWaveformU16(
            viSlave, waveformName, WAVEFORM_SIZE, waveformDataU16));
   }
   else   /*dataWidthSlave == 4*/
   {
   checkErr(niHSDIO_WriteNamedWaveformU32(
            viSlave, waveformName, WAVEFORM_SIZE, waveformDataU32));
   checkErr(niHSDIO_WriteNamedWaveformU32(
            viSlave, waveformName, WAVEFORM_SIZE, waveformDataU32));
   }

   /* Initiate generation */
   checkErr(niHSDIO_Initiate(viMaster));

   checkErr(niHSDIO_Initiate(viSlave));

   /* Wait for generation to complete */
   checkErr(niHSDIO_WaitUntilDone(viMaster, timeout));

   checkErr(niHSDIO_WaitUntilDone(viSlave, timeout));

Error:

   if (error == VI_SUCCESS)
   {
      /* print result */
      printf("Done without error.\n");
   }
   else
   {
      /* Get error description and print */
      printf("\nError encountered\n");

      niHSDIO_GetError(viMaster, &error, sizeof(errDesc)/sizeof(ViChar), errDesc);
      printf("%s\n", errDesc);

      niHSDIO_GetError(viSlave, &error, sizeof(errDesc)/sizeof(ViChar), errDesc);
      printf("%s\n", errDesc);
   }

   /* close the session. Close session on slave device first to avoid problems with reference clock */
   niHSDIO_close(viSlave);
   niHSDIO_close(viMaster);

   /* prompt to exit (for pop-up console windows) */
   printf("\nHit <Enter> to continue...\n");
   getchar();

   return error;
}
