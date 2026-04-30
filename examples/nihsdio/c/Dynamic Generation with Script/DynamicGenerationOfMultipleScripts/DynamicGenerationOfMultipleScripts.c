/************************************************************************
*
*  Example Program:
*    DynamicGenerationOfMultipleScripts.c
*
*  Description:
*    Generates different patterns based on multiple scripts.
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
   ViUInt8 waveformDataU8[WAVEFORM_SIZE];
   ViUInt16 waveformDataU16[WAVEFORM_SIZE];
   ViUInt32 waveformDataU32[WAVEFORM_SIZE];
   ViInt32 dataWidth = 4;
   ViConstString waveformName1 = "myWfm1";
   ViConstString waveformName2 = "myWfm2";
   ViConstString scriptName1 = "myScript1";
   ViConstString scriptName2 = "myScript2";

   /* Note: waveform names and script names are referenced in script */
   ViConstString script1 =
      "script myScript1 "
      "  repeat forever "
      "     generate myWfm1 "
      "  end repeat "
      "end script";

   /* Note: waveform names and script names are referenced in script */
   ViConstString script2 =
      "script myScript2 "
      "  repeat forever "
      "     repeat 3 "
      "        generate myWfm2 "
      "     end repeat "
      "     generate myWfm1 "
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

   /* Query the Data Width attribute */
   checkErr(niHSDIO_GetAttributeViInt32(
            vi, VI_NULL, NIHSDIO_ATTR_DATA_WIDTH, &dataWidth));

   /* Configure generation mode for scripted generation */
   checkErr(niHSDIO_ConfigureGenerationMode (vi, NIHSDIO_VAL_SCRIPTED));

   /* Populate waveform with ramp data */
   for (i = 0; i < WAVEFORM_SIZE; i++)
   {
      waveformDataU8[i] = (ViUInt8)(i%UCHAR_MAX);
      waveformDataU16[i] = (ViUInt16)(i%USHRT_MAX);
      waveformDataU32[i] = i;
   }

   /* Populate waveform with toggle data */
   for (i = 0; i < WAVEFORM_SIZE; i++)
   {
      waveformDataU8[i] = (i & 1) ? 0x0 : 0xFF;
      waveformDataU16[i] = (i & 1) ? 0x0 : 0xFFFF;
      waveformDataU32[i] = (i & 1) ? 0x0 : 0xFFFF;
   }

   /* Write waveform to device */
   /* The Data Width attribute is used to determine
      which Write fucntion to use */
   if (dataWidth == 1)
   {

      checkErr(niHSDIO_WriteNamedWaveformU8(
               vi, waveformName1, WAVEFORM_SIZE,
               waveformDataU8));

      checkErr(niHSDIO_WriteNamedWaveformU8(
               vi, waveformName2, WAVEFORM_SIZE,
               waveformDataU8));
   }

   else if (dataWidth == 2)
   {
      checkErr(niHSDIO_WriteNamedWaveformU16(
               vi, waveformName1, WAVEFORM_SIZE,
               waveformDataU16));

      checkErr(niHSDIO_WriteNamedWaveformU16(
               vi, waveformName2, WAVEFORM_SIZE,
               waveformDataU16));
   }

   else /* dataWidth == 4 */
   {
      checkErr(niHSDIO_WriteNamedWaveformU32(
               vi, waveformName1, WAVEFORM_SIZE,
               waveformDataU32));

      checkErr(niHSDIO_WriteNamedWaveformU32(
               vi, waveformName2, WAVEFORM_SIZE,
               waveformDataU32));
   }

   /* Write scripts to device */
   checkErr(niHSDIO_WriteScript(vi, script1));
   checkErr(niHSDIO_WriteScript(vi, script2));

   /* Execute the first script */
   printf("Hit <Enter> to execute script, \"%s\"...\n", scriptName1);
   getchar();
   checkErr(niHSDIO_ConfigureScriptToGenerate(vi, scriptName1));
   checkErr(niHSDIO_Initiate(vi));

   /* Wait for user input to abort generation */
   printf("Generation initiated.  Hit <Enter> to abort script, \"%s\"...\n",
          scriptName1);
   getchar();
   checkErr(niHSDIO_Abort(vi));

   /* Execute the second script */
   printf("Hit <Enter> to execute script, \"%s\"...\n", scriptName2);
   getchar();
   checkErr(niHSDIO_ConfigureScriptToGenerate(vi, scriptName2));
   checkErr(niHSDIO_Initiate(vi));

   /* Wait for user input to abort generation */
   printf("Generation initiated.  Hit <Enter> to abort script, \"%s\"...\n",
          scriptName2);
   getchar();
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
