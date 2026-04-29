/************************************************************************
*
*  Example Program:
*    DynamicGenerationHardwareStartTrigger.c
*
*  Description:
*    Configures a hardware trigger to start pattern generation.
*    Also configures the state of the channels before the start trigger
*    is received.
*
*  Pin Connection Information:
*    Start trigger signal on terminal specified
*    (default: PFI0).
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
   ViConstString channelList = "0-15";
   ViInt8 VoltageConfigurationSupported = 0;
   ViReal64 sampleClockRate = 50.0e6;
   ViInt32 dataWidth = 4;
   ViUInt8 waveformDataU8[WAVEFORM_SIZE];
   ViUInt16 waveformDataU16[WAVEFORM_SIZE];
   ViUInt32 waveformDataU32[WAVEFORM_SIZE];
   ViConstString waveformName = "myWfm";
   ViConstString triggerTerminal = NIHSDIO_VAL_PFI0_STR;
   ViInt32 timeout = 10000; /* milliseconds */

   ViConstString initialStateChannelList = "0-15";
   /* 0 - low, 1 - high, x - current state, z - tristate */
   ViConstString initialState = "xxxx xxxx xxxx xxxx";

   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];
   ViInt32 i;


   /* Initialize generation session */
   checkErr(niHSDIO_InitGenerationSession(
            deviceID, VI_FALSE, VI_FALSE, VI_NULL, &vi));

   /* Assign channels for dynamic generation */
   checkErr(niHSDIO_AssignDynamicChannels (vi, channelList));

   /* Configure generation and trigger voltages if the device supports voltage configuration*/
   if (VoltageConfigurationSupported)
   {
      checkErr(niHSDIO_ConfigureDataVoltageLogicFamily(
               vi, channelList, NIHSDIO_VAL_3_3V_LOGIC));

      checkErr(niHSDIO_ConfigureTriggerVoltageLogicFamily(
               vi, NIHSDIO_VAL_3_3V_LOGIC));
   }

   /* Configure clocking parameters */
   checkErr(niHSDIO_ConfigureSampleClock(
            vi, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Configure initial channel state */
   checkErr(niHSDIO_ConfigureInitialState(
            vi, initialStateChannelList, initialState));

   /* Configure hardware start trigger */
   checkErr(niHSDIO_ConfigureDigitalEdgeStartTrigger(
            vi, triggerTerminal, NIHSDIO_VAL_RISING_EDGE));

   /* Query the Data Width Attribute */
   checkErr(niHSDIO_GetAttributeViInt32(
            vi, VI_NULL, NIHSDIO_ATTR_DATA_WIDTH, &dataWidth));

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

   if (dataWidth == 1)
   {
   checkErr(niHSDIO_WriteNamedWaveformU8(
            vi, waveformName, WAVEFORM_SIZE, waveformDataU8));
   }
   else if (dataWidth == 2)
   {
   checkErr(niHSDIO_WriteNamedWaveformU16(
            vi, waveformName, WAVEFORM_SIZE, waveformDataU16));
   }
   else   /*dataWidth == 4*/
   {
   checkErr(niHSDIO_WriteNamedWaveformU32(
            vi, waveformName, WAVEFORM_SIZE, waveformDataU32));
   }

   /* Initiate generation */
   checkErr(niHSDIO_Initiate(vi));
   printf("Generation initiated.  Provide start trigger on %s.\n",
          triggerTerminal);

   /* Wait for generation to complete.
      Times out after time-out value specified if start
      trigger is not received                             */
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
