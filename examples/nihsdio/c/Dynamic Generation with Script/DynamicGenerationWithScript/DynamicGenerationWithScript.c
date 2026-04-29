/************************************************************************
*
*  Example Program:
*    DynamicGenerationWithScript.c
*
*  Description:
*    Generates a waveform based on specified script.
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
#define WAVEFORM_SIZE 1024

int main(void)
{

   ViRsrc deviceID = "PXI1Slot2";
   ViConstString channelList = "0-15";
   ViReal64 sampleClockRate = 50.0e6;
   ViInt32 dataWidth = 4;
   ViUInt8 waveformDataU8[WAVEFORM_SIZE];
   ViUInt16 waveformDataU16[WAVEFORM_SIZE];
   ViUInt32 waveformDataU32[WAVEFORM_SIZE];
   ViConstString waveformName = "myWfm";

   /* Note: waveform names are referenced in script */
   ViConstString script =
      "script myScript "
      "  repeat forever "
      "     generate myWfm "
      "  end repeat "
      "end script";

   ViSession vi = VI_NULL;
   ViStatus error = VI_SUCCESS;
   ViChar errDesc[1024];
   ViUInt32 i;


   /* Initialize generation session */
   checkErr(niHSDIO_InitGenerationSession(
            deviceID, VI_FALSE, VI_FALSE, VI_NULL, &vi));

   /* Assign channels for dynamic generation */
   checkErr(niHSDIO_AssignDynamicChannels (vi, channelList));


   /* Configure sample clock parameters */
   checkErr(niHSDIO_ConfigureSampleClock(
            vi, NIHSDIO_VAL_ON_BOARD_CLOCK_STR, sampleClockRate));

   /* Configure generation mode for scripted generation */
   checkErr(niHSDIO_ConfigureGenerationMode(vi, NIHSDIO_VAL_SCRIPTED));

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

   /* Write script to device */
   checkErr(niHSDIO_WriteScript(vi, script));

   /* Initiate generation */
   checkErr(niHSDIO_Initiate(vi));

   /* Wait for user input to abort generation */
   printf("Generation initiated.  Hit <Enter> to abort generation.\n");
   getchar();

   /* Abort generation */
   checkErr(niHSDIO_Abort(vi));

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
