/************************************************************************
*
*  Example Program:
*    Propagation Delay Measurement.c
*
*  Description:
*    This example demonstrates how to measure the propagation delay
*    between two channels.  External connections are required for this
*    example. Refer to the Pin Connection Information section for more
*    information about signal connections.
*
*    The Sample clock is exported on the DDC CLK OUT pin by the generation
*    session, and the acquisition uses the STROBE pin as its Sample
*    clock source. Similarly, the generation session also exports its
*    Data Active event to PFI 1, and the acquisition receives the
*    Start trigger from PFI 2.
*
*    Please note that this example was written for devices with a
*    default data width of 4 bytes. If your devices has a different
*    default data width, you will need to make some changes to this
*    example before it will run. For example, the NI 656x devices have
*    a default data width of 2 bytes. If you are using one of these
*    devices, you will need to make the following changes:
*       - Change the data type of waveformData to ViUInt16
*       - Change the data type of patternRead to ViUInt16
*       - Use niHSDIO_WriteNamedWaveformU16 instead of niHSDIO_WriteNamedWaveformU32
*       - Use niHSDIO_FetchWaveformU16 instead of niHSDIO_FetchWaveformU32
*
*  Pin Connection Information:
*    1.  Connect the exported Sample clock (default: DDC CLK OUT) from
*        the generation device to the clock input terminal (default: STROBE)
*        on the acquisition device.
*    2.  Connect the Data Active event (default PFI 1) to the acquisition
*        Start trigger (default PFI 2).
*    3.  Connect DIO 0 to DIO 8 and DIO 1 to DIO 9.
*
*    A simple test is measuring the propagation delay of a cable. To do
*    this test, use equal lengths of cables to make the following connections:
*        A. Exported sample clock -> Clock input terminal
*        B. Data Active event -> Acquisition Start trigger
*        C. DIO 0 to DIO 8
*    Use a longer cable to connect DIO 1 to DIO 9. When this code is run,
*    the propagation delay (DIO 8 - DIO 9) is returned.
************************************************************************/


/* Includes */
#include <stdio.h>
#include "niHSDIO.h"

/* Defines */
#define WAVEFORM_SIZE 512
#define SAMPLES_TO_READ 512

/* These two functions were created simply to make the main program easier to read */
ViStatus setupGenerationDevice (
      ViRsrc genDeviceID, ViConstString genChannelList, ViConstString sampleClockOutputTerminal,
      ViReal64 sampleClockRate, ViConstString dataActiveEventDestination, ViUInt32 waveformData[WAVEFORM_SIZE],
      ViConstString waveformName, ViSession *genViPtr);

ViStatus setupAcquisitionDevice (
      ViRsrc acqDeviceID, ViConstString acqChannelList, ViConstString sampleClockSource,
      ViReal64 sampleClockRate, ViConstString startTriggerSource, ViSession *genViPtr);

int main(void)
{
   ViReal64 sampleClockRate = 50.0e6;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];
   ViInt32 i,j;

   ViRsrc deviceID = "PXI1Slot2";

   int edge0Found = 0;
   int edge1Found = 0;
   int fineEdgeFound0 = 0;
   int fineEdgeFound1 = 0;
   int initialEdge0 = 0;
   int initialEdge1 = 0;
   int fineEdge0 = 0;
   int fineEdge1 = 0;
   ViReal64 PropagationDelay;
   ViReal64 actualSampleClockRate;

   /* Generation */
   ViConstString genChannelList = "0-1";
   ViConstString sampleClockOutputTerminal = NIHSDIO_VAL_DDC_CLK_OUT_STR;
   ViConstString dataActiveEventDestination = NIHSDIO_VAL_PFI1_STR;
   ViUInt32 waveformData[WAVEFORM_SIZE];
   /* Note that the data type of waveformData may need to change
   if your device's default data width is not 4 bytes. */
   ViConstString waveformName = "myWfm";
   ViSession genVi = VI_NULL;

   /* Acquisition */
   ViConstString acqChannelList = "8-9";
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
   {
      if (i<100)
         waveformData[i] = 0;
      else
         waveformData[i] = 3;
   }

   /* Initialize, configure, and write waveforms to generation device */
   checkErr(setupGenerationDevice (
            deviceID, genChannelList, sampleClockOutputTerminal,
            sampleClockRate, dataActiveEventDestination, waveformData, waveformName, &genVi));

   /* Commit settings on generation device so the sample clock will start.
   This must run before Initiate is run for the Acquisition session */
   checkErr(niHSDIO_CommitDynamic (genVi));

   /* Initialize, Configure, and Initiate Acquisition Sessions */
   checkErr(setupAcquisitionDevice  (deviceID, acqChannelList, sampleClockSource,
      sampleClockRate, startTriggerSource, &acqVi));

   for(i=0; i < 256; i++)
   {
      niHSDIO_Abort (acqVi);

      /* Increase data delay */
      niHSDIO_ConfigureDataPositionDelay (acqVi, acqChannelList, (i / 256.0));

      /* Initiate acquisition */
      checkErr(niHSDIO_Initiate (acqVi));

      /* Initiate generation. This function must be run after the
      acquisition session is initiated and waiting for a Start trigger.
      Otherwise, the generation may begin before the acquisition is ready
      to receive data. */
      checkErr(niHSDIO_Initiate (genVi));

      /* Fetch waveform data from device */
      /* Note that you may need to use a different Fetch function
      if your device's default data width is not 4 bytes. */
      checkErr(niHSDIO_FetchWaveformU32 (acqVi, SAMPLES_TO_READ, msReadTimeout,
         &numSamplesRead, patternRead));

      for (j=0; j<WAVEFORM_SIZE; j++)
      {
         /* Check for 0->1 edge on DIO 8 */
         if ((patternRead[j] & (0x00000001 << 8)) && !edge0Found)
         {
            edge0Found=1; // 1 indicates 0->1 edge found DIO 8

            if (i==0)
            {
               initialEdge0 = j; //edge with no data delay
            }

            else if ((initialEdge0 != j) && !fineEdgeFound0)
            {
               fineEdge0 = i;    //Data delay needed to shift edge
               fineEdgeFound0=1; //Fine edge found
            }

         }
         /* Check for 0->1 edge on DIO 9 */
         if ((patternRead[j] & (0x00000001 << 9)) && !edge1Found)
         {
            edge1Found=1; // 1 indicates 0->1 edge found DIO 9

            if (i==0)
            {
               initialEdge1 = j; //edge with no data delay
            }

            else if ((initialEdge1 != j) && !fineEdgeFound1)
            {
               fineEdge1 = i;    //Data delay needed to shift edge
               fineEdgeFound1=1; //Fine edge found
            }
         }
      }
      edge0Found=0;
      edge1Found=0;
   }
   if (fineEdge0==0 || fineEdge1==0)
   {
      PropagationDelay = -9999999; //no edge found on either DIO 8 or DIO 9
   }
   else
   {
      /* Get actual Sample clock rate */
      niHSDIO_GetAttributeViReal64 (acqVi, "", NIHSDIO_ATTR_SAMPLE_CLOCK_RATE, &actualSampleClockRate);

      /* Calculate propagation delay */
      PropagationDelay = (initialEdge0 - initialEdge1)/
         actualSampleClockRate + (fineEdge0-fineEdge1) /
         (256 * actualSampleClockRate);
   }

   printf("Propagation Delay = %e.\n", PropagationDelay);

Error:

   if (error == VI_SUCCESS)
   {
      /* print result */
      if (PropagationDelay == -9999999)
      {
         printf("Error: No Edge Found.  Check connections and try again.\n");
      }
      else
      {
         printf("Done without error.\n");
      }
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

   /* Configure Idle state */
   niHSDIO_ConfigureIdleStateU32 (genVi, 0);

   /* Configure Sample clock */
   checkErr(niHSDIO_ConfigureSampleClock (
      genVi, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Export Sample clock */
   checkErr(niHSDIO_ExportSignal (
      genVi, NIHSDIO_VAL_SAMPLE_CLOCK, VI_NULL, sampleClockOutputTerminal));

   /* Export Data Active event */
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

   /* Tristate acquisition channels */
   checkErr(niHSDIO_TristateChannels (acqVi, acqChannelList, VI_TRUE));

   /* Configure data relative to the Sample clock */
   niHSDIO_ConfigureDataPosition (acqVi, acqChannelList,
      NIHSDIO_VAL_DELAY_FROM_SAMPLE_CLOCK_RISING_EDGE);

   /* Configure clocking parameters */
   checkErr(niHSDIO_ConfigureSampleClock(
      acqVi, sampleClockSource, sampleClockRate));

   /* Configure Start trigger */
   checkErr(niHSDIO_ConfigureDigitalEdgeStartTrigger(acqVi,
      startTriggerSource, NIHSDIO_VAL_RISING_EDGE));

   /* Configure the number of samples to acquire to device */
   checkErr(niHSDIO_ConfigureAcquisitionSize (acqVi, SAMPLES_TO_READ, 1000));

   /* Start the data position at 0 */
   checkErr(niHSDIO_ConfigureDataPositionDelay (acqVi, acqChannelList, 0));

Error:

   *acqViPtr = acqVi;

   return error;

}
