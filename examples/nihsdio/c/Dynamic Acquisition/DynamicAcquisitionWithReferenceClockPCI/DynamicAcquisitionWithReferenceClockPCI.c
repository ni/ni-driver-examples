/************************************************************************
*
*  Example Program:
*    DynamicAcquisitionWithReferenceClockPCI.c
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
#include "niHSDIO.h"

/* Defines */
#define SAMPLES_TO_READ 1024

int main(void)
{

   ViRsrc masterDeviceID = "Dev1";
   ViRsrc slaveDeviceID = "Dev2";
   ViConstString channelList = "0-15";
   ViReal64 sampleClockRate = 50.0e6;
   ViInt32 readTimeout = 10000; /* milliseconds */

   ViUInt8 patternReadMasterU8[SAMPLES_TO_READ];
   ViUInt16 patternReadMasterU16[SAMPLES_TO_READ];
   ViUInt32 patternReadMasterU32[SAMPLES_TO_READ];

   ViUInt8 patternReadSlaveU8[SAMPLES_TO_READ];
   ViUInt16 patternReadSlaveU16[SAMPLES_TO_READ];
   ViUInt32 patternReadSlaveU32[SAMPLES_TO_READ];

   ViInt32 dataWidthMaster = 4;
   ViInt32 dataWidthSlave = 4;


   ViSession viMaster = VI_NULL;
   ViSession viSlave = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViInt32 numSamplesRead = 0;
   ViChar errDesc[1024];


   /* Initialize acquisition sessions */
   checkErr(niHSDIO_InitAcquisitionSession(
            masterDeviceID, VI_FALSE, VI_FALSE, VI_NULL, &viMaster));

   checkErr(niHSDIO_InitAcquisitionSession(
            slaveDeviceID, VI_FALSE, VI_FALSE, VI_NULL, &viSlave));

   /* Assign channels for dynamic acquisition */
   checkErr(niHSDIO_AssignDynamicChannels (viMaster, channelList));
   checkErr(niHSDIO_AssignDynamicChannels (viSlave, channelList));


   /* Export Onboard Reference Clock To RTSI7*/
   checkErr(niHSDIO_ExportSignal(
            viMaster, NIHSDIO_VAL_ONBOARD_REF_CLOCK, "", NIHSDIO_VAL_RTSI7_STR));

   /* Configure clocking parameters. Import Reference Clock from RTSI7 */
   checkErr(niHSDIO_ConfigureRefClock(
            viMaster, NIHSDIO_VAL_RTSI7_STR, 10.0e6));

   checkErr(niHSDIO_ConfigureRefClock(
            viSlave, NIHSDIO_VAL_RTSI7_STR, 10.0e6));

   checkErr(niHSDIO_ConfigureSampleClock(
            viMaster, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   checkErr(niHSDIO_ConfigureSampleClock(
            viSlave, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Configure the number of samples to acquire to device */
   checkErr(niHSDIO_ConfigureAcquisitionSize(
            viMaster, SAMPLES_TO_READ, 1));

   checkErr(niHSDIO_ConfigureAcquisitionSize(
            viSlave, SAMPLES_TO_READ, 1));

   /* Initiate acquisitions. Initiate master device acquisition first
      to export onboard reference clock */
   checkErr(niHSDIO_Initiate(viMaster));
   checkErr(niHSDIO_Initiate(viSlave));

   /* Query the Data Width attribute for each device*/
   checkErr(niHSDIO_GetAttributeViInt32(
            viMaster, VI_NULL, NIHSDIO_ATTR_DATA_WIDTH, &dataWidthMaster));
   checkErr(niHSDIO_GetAttributeViInt32(
            viSlave, VI_NULL, NIHSDIO_ATTR_DATA_WIDTH, &dataWidthSlave));

   /* Fetch Waveform data from devices */
   /* The Data Width attribute is used to determine which
      Read and function should be used. */

   if (dataWidthMaster == 1)
   {
      checkErr(niHSDIO_FetchWaveformU8(
               viMaster, SAMPLES_TO_READ, readTimeout,
               &numSamplesRead, patternReadMasterU8));
   }
   else if (dataWidthMaster == 2)
   {
      checkErr(niHSDIO_FetchWaveformU16(
               viMaster, SAMPLES_TO_READ, readTimeout,
               &numSamplesRead, patternReadMasterU16));
   }
   else  /* dataWidthMaster == 4 */
   {
      checkErr(niHSDIO_FetchWaveformU32(
               viMaster, SAMPLES_TO_READ, readTimeout,
                  &numSamplesRead, patternReadMasterU32));
   }

   if (dataWidthSlave == 1)
   {
      checkErr(niHSDIO_FetchWaveformU8(
               viSlave, SAMPLES_TO_READ, readTimeout,
               &numSamplesRead, patternReadSlaveU8));
   }
   else if (dataWidthSlave == 2)
   {
      checkErr(niHSDIO_FetchWaveformU16(
               viSlave, SAMPLES_TO_READ, readTimeout,
               &numSamplesRead, patternReadSlaveU16));
   }
   else  /* dataWidthSlave == 4 */
   {
      checkErr(niHSDIO_FetchWaveformU32(
               viSlave, SAMPLES_TO_READ, readTimeout,
                  &numSamplesRead, patternReadSlaveU32));
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
