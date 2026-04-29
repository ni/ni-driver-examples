/************************************************************************
*
*  Example Program:
*    DynamicAcquisitionWithExportedClocks.c
*
*  Description:
*    Exports the reference and sample clocks in an acquisition
*    operation.
*
*  Pin Connection Information:
*    10MHz reference clock on terminal specified
*    (default: PXI clock 10).
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
   ViInt8 VoltageConfigurationSupported = 0;
   ViReal64 sampleClockRate = 50.0e6;
   ViConstString sampleClockOutputTerminal = NIHSDIO_VAL_DDC_CLK_OUT_STR;
   ViConstString referenceClockSource = NIHSDIO_VAL_PXI_CLK_STR;
   ViConstString referenceClockOutputTerminal = NIHSDIO_VAL_CLK_OUT_STR;
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

   /* Configure acquisition and event voltages if the device supports voltage configuration*/
   if (VoltageConfigurationSupported)
   {
      checkErr(niHSDIO_ConfigureDataVoltageLogicFamily(
               vi, channelList, NIHSDIO_VAL_3_3V_LOGIC));

      checkErr(niHSDIO_ConfigureEventVoltageLogicFamily(
               vi, NIHSDIO_VAL_3_3V_LOGIC));
   }

   /* Configure clocking parameters */
   checkErr(niHSDIO_ConfigureRefClock(
            vi, referenceClockSource, 10.0e6));

   checkErr(niHSDIO_ConfigureSampleClock(
            vi, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Configure clocks for export */
   checkErr(niHSDIO_ExportSignal(
            vi, NIHSDIO_VAL_REF_CLOCK, VI_NULL, referenceClockOutputTerminal));

   checkErr(niHSDIO_ExportSignal(
            vi, NIHSDIO_VAL_SAMPLE_CLOCK, VI_NULL, sampleClockOutputTerminal));

   /* Configure the number of samples to acquire to device */
   checkErr(niHSDIO_ConfigureAcquisitionSize(
            vi, SAMPLES_TO_READ, 1));

   /* Query the Data Width attribute */
   checkErr(niHSDIO_GetAttributeViInt32(
            vi, VI_NULL, NIHSDIO_ATTR_DATA_WIDTH, &dataWidth));

   /* Read Waveform data from device */
   /* The Data Width attribute is used to determine which
      Read and function should be used. */
   if (dataWidth == 1)
   {
      checkErr(niHSDIO_ReadWaveformU8(
               vi, SAMPLES_TO_READ, readTimeout,
               &numSamplesRead, patternReadU8));
   }
   else if (dataWidth == 2)
   {
      checkErr(niHSDIO_ReadWaveformU16(
               vi, SAMPLES_TO_READ, readTimeout,
               &numSamplesRead, patternReadU16));
   }
   else  /* dataWidth == 4 */
   {
      checkErr(niHSDIO_ReadWaveformU32(
               vi, SAMPLES_TO_READ, readTimeout,
                  &numSamplesRead, patternReadU32));
   }

   /* Un-export clocks */
   checkErr(niHSDIO_ExportSignal(
            vi, NIHSDIO_VAL_REF_CLOCK, VI_NULL, VI_NULL));

   checkErr(niHSDIO_ExportSignal(
            vi, NIHSDIO_VAL_SAMPLE_CLOCK, VI_NULL, VI_NULL));

   checkErr(niHSDIO_CommitDynamic(vi));


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
