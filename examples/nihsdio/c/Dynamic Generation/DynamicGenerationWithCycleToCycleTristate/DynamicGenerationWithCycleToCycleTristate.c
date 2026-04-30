/************************************************************************
*
*  Example Program:
*    DynamicGenerationWithCycleToCycleTristate.c
*
*  Description:
*    Generates a pattern on specified channels and allows each channel
*    to be tristated on individual samples.
*
*  Pin Connection Information:
*    None.
*
************************************************************************/


/* Includes */
#include <stdio.h>
#include "niHSDIO.h"

/* Defines */

/* Number of Samples */
#define WAVEFORM_SIZE 256

/* Number of channels
   If you change the number of channels in channelList,
   you will need to change this value in order to
   allocate waveformData correctly */
#define NUMBER_OF_CHANNELS 16

int main(void)
{
   ViRsrc deviceID = "PXI1Slot2";

   ViConstString channelList = "0-15";
   /* Note: If you modify channelList, you must
      modify NUMBER_OF_CHANNELS to reflect the
      number of channels that will be used. */

   ViReal64 sampleClockRate = 50.0e6;
   ViUInt8 waveformData[WAVEFORM_SIZE * NUMBER_OF_CHANNELS];
   ViConstString waveformName = "myWfm";
   ViInt32 timeout = 10000; /* milliseconds */

   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];
   ViInt32 sampleNumber,channel;


   /* Initialize generation session */
   checkErr(niHSDIO_InitGenerationSession(
            deviceID, VI_FALSE, VI_FALSE, VI_NULL, &vi));

   /* Assign channels for dynamic generation */
   checkErr(niHSDIO_AssignDynamicChannels (vi, channelList));


   /* Configure sample clock parameters */
   checkErr(niHSDIO_ConfigureSampleClock(
            vi, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Enable extended digital states */
   checkErr(niHSDIO_SetAttributeViInt32(
      vi, "", NIHSDIO_ATTR_SUPPORTED_DATA_STATES,
      NIHSDIO_VAL_STATES_0_1_Z));

   /* Populate waveform with ramp data */
   for (sampleNumber = 0; sampleNumber < WAVEFORM_SIZE; sampleNumber++)
   {
      for (channel = 0; channel < NUMBER_OF_CHANNELS; channel++)
      {
         waveformData[(sampleNumber * NUMBER_OF_CHANNELS) + channel] = (sampleNumber >> channel) & 1;
      }
   }

   /* Insert Tristate samples with the Z state in a marching pattern */
   for (sampleNumber = 0; sampleNumber < WAVEFORM_SIZE; sampleNumber++)
   {
      channel = sampleNumber % NUMBER_OF_CHANNELS;
      waveformData[(sampleNumber * NUMBER_OF_CHANNELS) + channel] = NI_DIO_Z;
   }

   /* Write waveform to device */
   checkErr(niHSDIO_WriteNamedWaveformWDT(
            vi, waveformName, WAVEFORM_SIZE,
            NIHSDIO_VAL_GROUP_BY_SAMPLE,waveformData));

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
