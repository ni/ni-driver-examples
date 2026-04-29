/************************************************************************
*
*  Example Program:
*    DeviceDynamicAcqAndGen-SourceSynchronous.c
*
*  Description:
*    A single device both generates and acquires data.  The point of
*    this example is to account for cable delays that the data may
*    experience while traveling from the output channels to the
*    input channels. The sample clock is exported on the DDC Clk-Out
*    pin by the generation session, and the acquisition will use the
*    STROBE pin as its sample clock source. Similarly, the generation
*    session will also export its data active event to PFI 1 and the
*    acquisition will receive the start trigger from PFI 2.
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
*       - Use niHSDIO_FetchWaveformU16 instead of niHSDIO_FetchWaveformU32
*
*  Pin Connection Information:
*    Connect the exported sample clock (default: DDC Clk-Out) from the
*    generation device to the clock input terminal (default: Strobe)
*    on the acquisition device, and connect the Data Active Event
*    (default PFI1) to the Acquisition Start Trigger (default PFI2).
*
*
************************************************************************/

/* Includes */
#include <stdio.h>
#include "niHSDIO.h"

/* Defines */
#define WAVEFORM_SIZE 1024
#define SAMPLES_TO_READ 1024

/* These two functions were created simply to make the main program easier to read */
ViStatus setupGenerationDevice (ViRsrc genDeviceID, ViConstString genChannelList, ViConstString sampleClockOutputTerminal,
                                ViReal64 sampleClockRate, ViConstString dataActiveEventDestination, ViUInt32 waveformData[WAVEFORM_SIZE],
                                ViConstString waveformName, ViSession *genViPtr);

ViStatus setupAcquisitionDevice (ViRsrc acqDeviceID, ViConstString acqChannelList, ViConstString sampleClockSource,
                                 ViReal64 sampleClockRate, ViConstString startTriggerSource, ViSession *genViPtr);

int main(void)
{

   ViReal64 sampleClockRate = 50.0e6;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];
   ViInt32 i;

   ViRsrc deviceID = "PXI1Slot2";

   /* Generation */
   ViConstString genChannelList = "0-7";
   ViConstString sampleClockOutputTerminal = NIHSDIO_VAL_DDC_CLK_OUT_STR;
   ViConstString dataActiveEventDestination = NIHSDIO_VAL_PFI1_STR;
   ViUInt32 waveformData[WAVEFORM_SIZE];
   /* Note that the data type of waveformData may need to change
      if your device's default data width is not 4 bytes. */
   ViConstString waveformName = "myWfm";
   ViSession genVi = VI_NULL;

   /* Acquisition */
   ViConstString acqChannelList = "8-15";
   ViConstString sampleClockSource = NIHSDIO_VAL_STROBE_STR;
   ViConstString startTriggerSource = NIHSDIO_VAL_PFI2_STR;
   ViInt32 msReadTimeout = 10000;
   ViUInt32 patternRead[SAMPLES_TO_READ];
   /* Note that the data type of patternRead may need to change
      if your device's default data width is not 4 bytes. */
   ViInt32 numSamplesRead;
   ViSession acqVi = VI_NULL;

   /* Populate waveform with ramp data */
   for (i = 0; i < WAVEFORM_SIZE; i++)
      waveformData[i] = i;

   /* Initialize, configure, and write waveforms to generation device */
   checkErr(setupGenerationDevice (deviceID, genChannelList, sampleClockOutputTerminal,
            sampleClockRate, dataActiveEventDestination, waveformData, waveformName, &genVi));

   /* Commit settings on generation device so the sample clock will start.
   This must run before Initiate is run for the Acquisition session */
   checkErr(niHSDIO_CommitDynamic (genVi));

   /* Initialize, Configure, and Initiate Acquisition Sessions */
   checkErr(setupAcquisitionDevice  (deviceID, acqChannelList, sampleClockSource,
            sampleClockRate, startTriggerSource, &acqVi));

   /* Initiate generation. This function must be run after the
   acquisition session is initiated and waiting for a start trigger.
   Otherwise, the generation may begin before the acquisition is ready to receive data. */
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

ViStatus setupGenerationDevice(ViRsrc genDeviceID, ViConstString genChannelList, ViConstString sampleClockOutputTerminal,
                               ViReal64 sampleClockRate,ViConstString dataActiveEventDestination, ViUInt32 waveformData[WAVEFORM_SIZE],
                               ViConstString waveformName, ViSession *genViPtr)
{

   ViStatus error = VI_SUCCESS;
   ViSession genVi = VI_NULL;

   /* Initialize generation session */
   checkErr(niHSDIO_InitGenerationSession (
            genDeviceID, VI_FALSE, VI_FALSE, VI_NULL, &genVi));

   /* Assign channels for dynamic generation */
   checkErr(niHSDIO_AssignDynamicChannels (genVi, genChannelList));

   /* Configure Sample Clock */
   checkErr(niHSDIO_ConfigureSampleClock (
            genVi, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Export Sample Clock */
   checkErr(niHSDIO_ExportSignal (
            genVi, NIHSDIO_VAL_SAMPLE_CLOCK, VI_NULL, sampleClockOutputTerminal));

   /* Export data active event */
   checkErr(niHSDIO_ExportSignal(genVi, NIHSDIO_VAL_DATA_ACTIVE_EVENT,
            VI_NULL, dataActiveEventDestination));

   /* Write waveform to device */
   /* Note that you may need to use a different Write function
      if your device's default data width is not 4 bytes. */
   checkErr(niHSDIO_WriteNamedWaveformU32(genVi, waveformName, WAVEFORM_SIZE,
            waveformData));


Error:

   *genViPtr = genVi;

   return error;
}


ViStatus setupAcquisitionDevice (ViRsrc acqDeviceID, ViConstString acqChannelList, ViConstString sampleClockSource,
                                 ViReal64 sampleClockRate, ViConstString startTriggerSource, ViSession *acqViPtr)
{
   ViStatus error = VI_SUCCESS;
   ViSession acqVi = VI_NULL;

   /* Initialize acquisition session */
   checkErr(niHSDIO_InitAcquisitionSession(
      acqDeviceID, VI_FALSE, VI_FALSE, VI_NULL, &acqVi));

   /* Assign channels for dynamic acquisition */
   checkErr(niHSDIO_AssignDynamicChannels (acqVi, acqChannelList));


   /* Configure clocking parameters */
   checkErr(niHSDIO_ConfigureSampleClock(
            acqVi, sampleClockSource, sampleClockRate));

   /* Configure start trigger */
   checkErr(niHSDIO_ConfigureDigitalEdgeStartTrigger(acqVi,
            startTriggerSource, NIHSDIO_VAL_RISING_EDGE));

   /* Configure the number of samples to acquire to device */
   checkErr(niHSDIO_ConfigureAcquisitionSize(
            acqVi, SAMPLES_TO_READ, 1));

   /* Initiate Acquisition */
   checkErr(niHSDIO_Initiate (acqVi));

Error:

   *acqViPtr = acqVi;

   return error;

}
