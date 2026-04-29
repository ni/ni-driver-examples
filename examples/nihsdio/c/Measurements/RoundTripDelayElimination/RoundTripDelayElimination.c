/************************************************************************
*
*  Example Program:
*    RoundTripDelayElimination.c
*
*  Description:
*    This example demonstrates how to compensate for the delay caused
*    when data travels to the DUT and back. An internal version of the
*    data active event is delayed a configured number of clock cycles;
*    this delayed signal is then used as a start trigger in the acquisition.
*
*    Acquisition data delay is used as a fine resolution delay. Data is acquired
*    on a configured point in the clock cycle, this compensates the phase shift
*    caused by the round trip delay.
*
*    Please note that this example was written for devices with a
*    default data width of 4 bytes. If your devices has a different
*    default data width, you will need to make some changes to this
*    example before it will run. For example, the 656x devices have
*    a default data width of 2 bytes. If you are using one of these
*    devices, you will need to make the following changes:
*       - Change the datatype of waveformData to ViUInt16
*       - Change the datatype of patternRead to ViUInt16
*       - Use niHSDIO_WriteNamedWaveformU16 instead of niHSDIO_WriteNamedWaveformU32
*       - Use niHSDIO_FetchWaveformU16 instead of niHSDIO_FetchWaveformU32*
*  Pin Connection Information:
*    None.
*
************************************************************************/


/* Includes */
#include <stdio.h>
#include "niHSDIO.h"

/* Defines */
#define WAVEFORM_SIZE 1024
#define SAMPLES_TO_READ 1024

int main(void)
{

   ViReal64 sampleClockRate = 50.0e6;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];
   ViInt32 i;

   ViRsrc deviceID = "PXI1Slot2";

   /* Generation */
   ViConstString genChannelList = "0-7";
   ViUInt32 waveformData[WAVEFORM_SIZE];
   /* Note that the data type of waveformData may need to change
      if your device's default data width is not 4 bytes. */
   ViSession genVi = VI_NULL;

   /* Acquisition */
   ViConstString acqChannelList = "8-15";
   ViInt32 msReadTimeout = 10000;
   ViUInt32 patternRead[SAMPLES_TO_READ];
   /* Note that the data type of patternRead may need to change
      if your device's default data width is not 4 bytes. */
   ViInt32 numSamplesRead;
   ViSession acqVi = VI_NULL;
   /* Round Trip Delay Parameters */
   ViInt32 dataActiveDelay = 3;/* clock cycles */
   ViReal64 dataDelay = 0.65;/* % of clock cycle */

   /****** Generation Configuration *******/

   /* Initialize generation session */
   checkErr(niHSDIO_InitGenerationSession (
            deviceID, VI_FALSE, VI_FALSE, VI_NULL, &genVi));

   /* Assign channels for dynamic generation */
   checkErr(niHSDIO_AssignDynamicChannels (genVi, genChannelList));

   /* Configure generation Sample Clock */
   checkErr(niHSDIO_ConfigureSampleClock (
            genVi, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Populate waveform with ramp data */
   for (i = 0; i < WAVEFORM_SIZE; i++)
      waveformData[i] = i;

   /* Write waveform to device */
   checkErr(niHSDIO_WriteNamedWaveformU32(genVi, "", WAVEFORM_SIZE, waveformData));

   /****** Acquisition Configuration *******/

   /* Initialize acquisition session */
   checkErr(niHSDIO_InitAcquisitionSession(
            deviceID, VI_FALSE, VI_FALSE, VI_NULL, &acqVi));

   /* Assign channels for dynamic acquisition */
   checkErr(niHSDIO_AssignDynamicChannels (acqVi, acqChannelList));

   /* Configure clocking parameters */
   checkErr(niHSDIO_ConfigureSampleClock(
            acqVi, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Configure the number of samples to acquire to device */
   checkErr(niHSDIO_ConfigureAcquisitionSize(
            acqVi, SAMPLES_TO_READ, 1));

   /* Configure start trigger to use internal Data Active Event*/
   checkErr(niHSDIO_ConfigureDigitalEdgeStartTrigger(
            acqVi, "DelayedDataActiveEvent", NIHSDIO_VAL_RISING_EDGE));

   /* Configure delay applied to internal data active event (clock cycles) */
   checkErr(niHSDIO_SetAttributeViInt32 (
            acqVi, "", NIHSDIO_ATTR_DATA_ACTIVE_INTERNAL_ROUTE_DELAY, dataActiveDelay));

   /* Configure acquisition data position to delayed from sample clock */
   checkErr(niHSDIO_ConfigureDataPosition (
            acqVi, acqChannelList, NIHSDIO_VAL_DELAY_FROM_SAMPLE_CLOCK_RISING_EDGE));

   /* Configure delay applied to the data channels as a percentage of the clock */
   checkErr(niHSDIO_ConfigureDataPositionDelay (acqVi, acqChannelList, dataDelay));

   /****** Start Acquisition and Generation ******/

   /* Initiate Acquisition */
   /* acquisition starts and waits for a start trigger */
   checkErr(niHSDIO_Initiate (acqVi));


   /* Initiate generation */
   /* Generation starts and after the delayed configured internaly
      generates the delayed data active event to trigger the acquisition*/
   checkErr(niHSDIO_Initiate (genVi));

   /* Fetch Waveform data from device */
   /* Note that you may need to use a different Fetch function
      if your device's default data width is not 4 bytes. */
   checkErr(niHSDIO_FetchWaveformU32 (acqVi, SAMPLES_TO_READ, msReadTimeout,
            &numSamplesRead, patternRead));

Error:

   if (error == VI_SUCCESS)
   {
      /* print result */
      printf("Done without error.\n");
      printf("Number of samples read = %d.\n", numSamplesRead);
   }
   else
   {
      printf("Error encountered\n===================\n");

      /*  Get errors from NI-HSDIO sessions. Calling niHSDIO_GetError returns
          the error and clears the error for that session. */

      niHSDIO_GetError(genVi, &error, sizeof(errDesc)/sizeof(ViChar), errDesc);
      if (error != VI_SUCCESS)
         printf("Device ID: %s\nError Description: %s\n", deviceID, errDesc);

      niHSDIO_GetError(acqVi, &error, sizeof(errDesc)/sizeof(ViChar), errDesc);
      if (error != VI_SUCCESS)
         printf("Device ID: %s\nError Description: %s\n", deviceID, errDesc);

   }

   /* close the sessions */
   niHSDIO_close(genVi);

   niHSDIO_close(acqVi);

   /* prompt to exit (for pop-up console windows) */
   printf("\nHit <Enter> to continue...\n");
   getchar();

   return error;
}
