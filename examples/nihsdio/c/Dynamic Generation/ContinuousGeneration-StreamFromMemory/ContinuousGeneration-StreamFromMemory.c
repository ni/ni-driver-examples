/************************************************************************
*
*  Example Program:
*    StreamingGeneration.c
*
*  Description:
*    This example demonstrates how to continuously generate dynamic data
*    using the niHSDIO Allocate Named Waveform and niHSDIO Write Named
*    waveform functions at the specified clock rate.
*    The niHSDIO Allocate Named Waveform VI reserves onboard memory for
*    the waveform that will be continuously generated. niHSDIO Write
*    Named Waveform will download data continuously from PC memory to
*    onboard memory and then generate it.
*    Note that the maximum clock rate that can be used with this VI could
*    be much less than the maximum rate of the NI-HSDIO device. This
*    difference could be caused by the maximum rate of the PCI/PXI bus.
*    This example was design for devices with high onboard memory option.
*    For lower memory options redunce the number of samples, this will however
*    result in less optimum performance.
*
*  Pin Connection Information:
*    Connect your signals to the output terminals that match the channels
*    specified by Channel List. For this example, wire your signals to
*    first 16 digital channels of the device.
*
************************************************************************/


/* Includes */
#include <stdio.h>
#include <stdlib.h>
#include <limits.h>
#include "niHSDIO.h"

int main(void)
{
   ViRsrc deviceID = "PXI1Slot2";
   ViConstString channelList = "0-15";
   ViReal64 sampleClockRate = 1.0e6;
   ViInt32 dataWidth = 4;

   ViConstString waveformName = "myWfm";
   ViInt32 OnboardStreamingWfmSize = 10485760; /* samples */
   ViInt32 WriteStreamingWaveformInNBlocks = 10;
   ViInt32 WaveformSize = 104857600; /* samples */

   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];
   ViInt32 i;
   ViInt32 chunkSize = OnboardStreamingWfmSize / WriteStreamingWaveformInNBlocks;
   ViInt32 FreeSamplesinWaveform;
   ViInt32 chunksToWrite = WaveformSize/chunkSize;

   /* To reduce memory useage, delete unused arrays */
   ViUInt8 *waveformDataU8 = malloc(chunkSize);
   ViUInt16 *waveformDataU16 = malloc(chunkSize * 2);
   ViUInt32 *waveformDataU32 = malloc(chunkSize * 4);

   /* Initialize generation session */
   checkErr(niHSDIO_InitGenerationSession(
            deviceID, VI_FALSE, VI_FALSE, VI_NULL, &vi));

   /* Reset HSDIO Card */
   niHSDIO_ResetDevice (vi);

   /* Assign channels for dynamic generation */
   checkErr(niHSDIO_AssignDynamicChannels (vi, channelList));


   /* Configure sample clock parameters */
   checkErr(niHSDIO_ConfigureSampleClock(
            vi, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Set generation mode to repeat */
   checkErr(niHSDIO_ConfigureGenerationRepeat (vi, NIHSDIO_VAL_CONTINUOUS, 1));

   /* Allocate waveform size for streaming */
   checkErr(niHSDIO_AllocateNamedWaveform (vi, "myWfm",
           OnboardStreamingWfmSize));

   /* Query the Data Width Attribute */
   checkErr(niHSDIO_GetAttributeViInt32(
            vi, VI_NULL, NIHSDIO_ATTR_DATA_WIDTH, &dataWidth));

   /* Enable Streaming */
   checkErr(niHSDIO_SetAttributeViBoolean (vi, "",
         NIHSDIO_ATTR_STREAMING_ENABLED, VI_TRUE));

   /* Specify Streaming Waveform Name */
   checkErr(niHSDIO_SetAttributeViString (vi, "",
           NIHSDIO_ATTR_STREAMING_WAVEFORM_NAME, "myWfm"));

   /* Create waveform data */
   for (i = 0; i < chunkSize; i++)
   {
      if (dataWidth == 1)
           waveformDataU8[i] = (ViUInt8)(i%UCHAR_MAX);
     else if (dataWidth == 2)
            waveformDataU16[i] = (ViUInt16)(i%USHRT_MAX);
     else /* dataWidth == 4 */
           waveformDataU32[i] = i;
   }

   /* Write waveform to device */
   /* The Data Width attribure is used to determine which
      Write function should be used */
   for (i = 0; i < WriteStreamingWaveformInNBlocks; i++)
   {
     if (dataWidth == 1)
      {
            checkErr(niHSDIO_WriteNamedWaveformU8(
               vi, waveformName, chunkSize,
               waveformDataU8));
       }
       else if (dataWidth == 2)
       {
            checkErr(niHSDIO_WriteNamedWaveformU16(
               vi, waveformName, chunkSize,
               waveformDataU16));
       }
       else   /*dataWidth == 4*/
       {
            checkErr(niHSDIO_WriteNamedWaveformU32(
               vi, waveformName, chunkSize,
               waveformDataU32));
       }
   }

   /* Initiate generation */
   checkErr(niHSDIO_Initiate(vi));

   for (i = WriteStreamingWaveformInNBlocks; i < chunksToWrite; i++)
   {
      /* Check how much space is available in streaming waveform */
      checkErr(niHSDIO_GetAttributeViInt32 (vi, "",
         NIHSDIO_ATTR_SPACE_AVAILABLE_IN_STREAMING_WAVEFORM,
         &FreeSamplesinWaveform));

      /* Write waveorm if there is enough space in streaming waveform */
      if (FreeSamplesinWaveform >= chunkSize)
      {
         if (dataWidth == 1)
         {
            checkErr(niHSDIO_WriteNamedWaveformU8(
               vi, waveformName, chunkSize,
               waveformDataU8));
         }
         else if (dataWidth == 2)
         {
            checkErr(niHSDIO_WriteNamedWaveformU16(
               vi, waveformName, chunkSize,
               waveformDataU16));
         }
         else   /*dataWidth == 4*/
         {
            checkErr(niHSDIO_WriteNamedWaveformU32(
               vi, waveformName, chunkSize,
               waveformDataU32));
         }
      }
}

   /* close the session */
   niHSDIO_close(vi);


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

   /* deallocate arrays */

   free(waveformDataU8);
   free(waveformDataU16);
   free(waveformDataU32);

   /* prompt to exit (for pop-up console windows) */
   printf("\nHit <Enter> to continue...\n");
   getchar();

   return error;
}
